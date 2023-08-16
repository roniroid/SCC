using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using SCC_DATA.Queries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace SCC_BL.Tools
{
    public class ExcelParser : IDisposable
    {
        public List<T> ProcesarExcel<T>(string filePath, string dynamicMethodName = "")
        {
            List<T> objectList = new List<T>();

            if (!string.IsNullOrEmpty(dynamicMethodName))
            {
                try
                {
                    using (SpreadsheetDocument document = SpreadsheetDocument.Open(filePath, false))
                    {
                        WorkbookPart wbPart = document.WorkbookPart;
                        var workSheet = wbPart.Workbook.Descendants<Sheet>().FirstOrDefault();
                        if (workSheet != null)
                        {
                            WorksheetPart wsPart = (WorksheetPart)(wbPart.GetPartById(workSheet.Id));
                            IEnumerable<Row> rows = wsPart.Worksheet.Descendants<Row>();
                            var headersCount = rows.ElementAt(0).Count();

                            foreach (Row row in rows.Skip(1))//Skipeamos el header
                            {
                                var newRow = GetRowCells(row, headersCount).ToArray();

                                dynamic @object = new Object();
                                try
                                {
                                    @object = (T)Activator.CreateInstance(typeof(T));
                                }
                                catch (Exception ex)
                                {
                                }

                                typeof(T).GetType().GetMethod(dynamicMethodName).Invoke(@object, new object[] { newRow });

                                objectList.Add(@object);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return objectList;
        }

        public int? NextColumnWithDataIndex(DocumentFormat.OpenXml.Spreadsheet.Cell[] cellCollection, int startIndex = 0)
        {
            int headersCount = cellCollection.Count();

            for (int i = startIndex; i < headersCount; i++)
            {
                string auxCellValue = GetCellValue(cellCollection[i]).ToString().Trim();

                if (!string.IsNullOrEmpty(auxCellValue)) return i;
            }

            return null;
        }

        public bool CellHasData(DocumentFormat.OpenXml.Spreadsheet.Cell[] cellCollection, int index = 0)
        {
            string auxCellValue = GetCellValue(cellCollection[index]).ToString().Trim();

            if (!string.IsNullOrEmpty(auxCellValue)) return true;

            return false;
        }

        public bool LookForValidData(IEnumerable<DocumentFormat.OpenXml.Spreadsheet.Row> rowCollection)
        {
            int rowCount = 0;
            int headersCount = rowCollection.ElementAt(0).Count();

            while (rowCount < SCC_BL.Settings.Overall.MAX_NUMBER_OF_ROWS_TO_CHECK)
            {
                string tempCellValue = GetCellValue(
                    GetRowCells(
                        rowCollection.ElementAt(rowCount),
                        headersCount
                    ).ToArray()[0])
                .ToString()
                .Trim();

                if (!string.IsNullOrEmpty(tempCellValue)) return true;

                rowCount++;
            }

            return false;
        }

        public void ExportDataTable(DataTable table, string destination)
        {

            using (var workbook = SpreadsheetDocument.Create(destination, SpreadsheetDocumentType.Workbook))
            {
                var workbookPart = workbook.AddWorkbookPart();

                workbook.WorkbookPart.Workbook = new Workbook();

                workbook.WorkbookPart.Workbook.Sheets = new Sheets();

                var sheetPart = workbook.WorkbookPart.AddNewPart<WorksheetPart>();
                var sheetData = new SheetData();
                sheetPart.Worksheet = new Worksheet(sheetData);

                Sheets sheets = workbook.WorkbookPart.Workbook.GetFirstChild<Sheets>();
                string relationshipId = workbook.WorkbookPart.GetIdOfPart(sheetPart);

                uint sheetId = 1;
                if (sheets.Elements<Sheet>().Count() > 0)
                {
                    sheetId =
                        sheets.Elements<Sheet>().Select(s => s.SheetId.Value).Max() + 1;
                }

                Sheet sheet = new Sheet() { Id = relationshipId, SheetId = sheetId, Name = table.TableName };
                sheets.Append(sheet);

                Row headerRow = new Row();

                List<String> columns = new List<string>();
                foreach (DataColumn column in table.Columns)
                {
                    columns.Add(column.ColumnName);

                    Cell cell = new Cell();
                    cell.DataType = CellValues.String;
                    cell.CellValue = new CellValue(column.ColumnName);
                    headerRow.AppendChild(cell);
                }


                sheetData.AppendChild(headerRow);

                foreach (DataRow dsrow in table.Rows)
                {
                    Row newRow = new Row();
                    foreach (String col in columns)
                    {
                        Cell cell = new Cell();
                        cell.DataType = CellValues.String;
                        cell.CellValue = new CellValue(dsrow[col].ToString()); //
                        newRow.AppendChild(cell);
                    }

                    sheetData.AppendChild(newRow);
                }

            }
        }

        public IEnumerable<Cell> GetRowCells(Row row, int headersCount)
        {
            int currentCount = 0;
            List<Cell> cells = new List<Cell>();
            foreach (Cell cell in
                row.Descendants<Cell>())
            {
                string columnName = GetColumnName(cell.CellReference);

                int currentColumnIndex = ConvertColumnNameToNumber(columnName);
                int columnDifference = currentColumnIndex - currentCount;
                //Fill the column gap
                if (columnDifference > 0)
                {
                    for (int i = 0; i < columnDifference; i++)
                    {
                        cells.Add(new Cell());
                        currentCount++;
                    }
                }
                cells.Add(cell);

                currentCount++;
            }
            // Fill missing columns
            for (int i = cells.Count; i < headersCount; i++)
            {
                cells.Add(new Cell());
            }
            return cells;
        }

        public string GetCellValue(Cell cell)
        {
            if (cell == null)
                return null;
            if (cell.DataType == null)
                return cell.InnerText;

            string value = cell.InnerText;
            switch (cell.DataType.Value)
            {
                case CellValues.SharedString:
                    // For shared strings, look up the value in the shared strings table.
                    // Get worksheet from cell
                    OpenXmlElement parent = cell.Parent;
                    while (parent.Parent != null && parent.Parent != parent
                            && string.Compare(parent.LocalName, "worksheet", true) != 0)
                    {
                        parent = parent.Parent;
                    }
                    if (string.Compare(parent.LocalName, "worksheet", true) != 0)
                    {
                        throw new Exception("Unable to find parent worksheet.");
                    }

                    Worksheet ws = parent as Worksheet;
                    SpreadsheetDocument ssDoc = ws.WorksheetPart.OpenXmlPackage as SpreadsheetDocument;
                    SharedStringTablePart sstPart = ssDoc.WorkbookPart.GetPartsOfType<SharedStringTablePart>().FirstOrDefault();

                    // lookup value in shared string table

                    if (sstPart != null && sstPart.SharedStringTable != null)
                        value = sstPart.SharedStringTable.ElementAt(int.Parse(value)).InnerText;

                    break;

                //this case within a case is copied from msdn. 
                case CellValues.Boolean:
                    switch (value)
                    {
                        case "0":
                            value = "FALSE";
                            break;
                        default:
                            value = "TRUE";
                            break;
                    }
                    break;
            }
            return value;
        }

        public string FormatDate(string FechaDecimal)
        {
            try
            {
                DateTime Fecha;
                //a veces la fecha puede venir lista, y no en decimal
                if (DateTime.TryParse(FechaDecimal, out Fecha))
                {
                    return FechaDecimal;
                }
                else
                {
                    try
                    {
                        return new DateTime(1899, 12, 30).AddDays(Convert.ToDouble(FechaDecimal)).ToString("yyyy-MM-dd");
                    }
                    catch (Exception)
                    {
                        if (FechaDecimal.Contains('.'))
                            return new DateTime(1899, 12, 30).AddDays(Convert.ToDouble(FechaDecimal.Replace('.', ','))).ToString("yyyy-MM-dd");
                        else
                            return new DateTime(1899, 12, 30).AddDays(Convert.ToDouble(FechaDecimal.Replace(',', '.'))).ToString("yyyy-MM-dd");
                    }
                }
            }
            catch
            {
                return FechaDecimal;
            }

        }

        public DateTime? FormatDateTime(string excelDateText)
        {
            string pattern = @"\d+([\.,]\d+)?";
            if (!Regex.IsMatch(excelDateText, pattern)) return null;

            if (!excelDateText.Contains('.') && !excelDateText.Contains(',')) return null;

            excelDateText = excelDateText.Replace(',', '.');

            double days = 0;

            if (!double.TryParse(excelDateText, System.Globalization.NumberStyles.Any, CultureInfo.InvariantCulture, out days)) return null;

            DateTime date = new DateTime(1900, 1, 1);
            date = date.AddDays(days - 2);

            return date;
        }

        /*public enum DoubleSeparator
        {
            NONE,
            DOT,
            COMMA
        }
        
        public DateTime? FormatDateTime(string excelDateText, DoubleSeparator doubleSeparator = DoubleSeparator.DOT)
        {
            string pattern = @"\d+([\.,]\d+)?";
            if (!Regex.IsMatch(excelDateText, pattern)) return null;

            int days = 0;
            string missing = "0";
            double missingSeconds = 0;

            if (excelDateText.Contains('.'))
            {
                days = Convert.ToInt32(excelDateText.Substring(0, excelDateText.IndexOf('.')));
                missing = excelDateText.Substring(excelDateText.IndexOf('.') + 1);
            }
            else if (excelDateText.Contains(','))
            {
                days = Convert.ToInt32(excelDateText.Substring(0, excelDateText.IndexOf(',')));
                missing = excelDateText.Substring(excelDateText.IndexOf(',') + 1);
            }

            char separator = ',';

            switch (doubleSeparator)
            {
                case DoubleSeparator.DOT:
                    separator = '.';
                    break;
                case DoubleSeparator.COMMA:
                    separator = ',';
                    break;
                default:
                    separator = ',';
                    break;
            }

            missingSeconds = Convert.ToDouble($"0{ separator }{ missing }");

            if (!excelDateText.Contains('.') && !excelDateText.Contains(',')) return null;

            DateTime date = new DateTime(1900, 1, 1);
            date = date.AddDays(days - 2);

            TimeSpan time = new TimeSpan(0, 0, 0);

            try
            {
                double seconds = missingSeconds * 86400;
                time = new TimeSpan(0, 0, Convert.ToInt32(seconds));
            }
            catch (Exception ex0)
            {
                try
                {
                    double minutes = missingSeconds * (24 * 60);
                    double seconds = (minutes % 1) * 60;

                    time = new TimeSpan(0, Convert.ToInt32(minutes), Convert.ToInt32(seconds));
                }
                catch (Exception ex1)
                {
                    try
                    {
                        double hours = missingSeconds * 24;
                        double minutes = (hours % 1) * 60;
                        double seconds = (minutes % 1) * 60;

                        time = new TimeSpan(Convert.ToInt32(hours), Convert.ToInt32(minutes), Convert.ToInt32(seconds));
                    }
                    catch (Exception ex2)
                    {
                    }
                }
            }

            if (doubleSeparator == DoubleSeparator.NONE)
            {
                try
                {
                    date = date.Add(time);
                }
                catch (Exception ex0)
                {
                }
            }
            else
            {
                date = date.Add(time);
            }

            return date;
        }*/

        /*public DateTime? FormatDateTime(string excelDateText)
        {
            string pattern = @"\d+([\.,]\d+)?";
            if (!Regex.IsMatch(excelDateText, pattern)) return null;

            int days = Convert.ToInt32(excelDateText.Substring(0, excelDateText.IndexOf('.')).Substring(0, excelDateText.IndexOf(',')));
            int missing = Convert.ToInt32(excelDateText.Substring(excelDateText.IndexOf('.')).Substring(excelDateText.IndexOf(',')));
            double excelDate = 0;

            if (!excelDateText.Contains('.') && !excelDateText.Contains(',')) return null;

            try
            {
                excelDate = Convert.ToDouble(excelDateText);
            }
            catch (Exception ex)
            {
                if (excelDateText.Contains('.'))
                    excelDate = Convert.ToDouble(excelDateText.Replace('.', ','));
                else
                    excelDate = Convert.ToDouble(excelDateText.Replace(',', '.'));
            }

            DateTime date = new DateTime(1900, 1, 1);
            date = date.AddDays(excelDate - 2);

            double fractionalPart = excelDate - Math.Truncate(excelDate);
            int seconds = (int)Math.Round(fractionalPart * 86400);

            TimeSpan time = new TimeSpan(0, 0, seconds);
            date = date.Add(time);

            return date;
        }*/

        /// <summary>
        /// Given a cell name, parses the specified cell to get the column name.
        /// </summary>
        /// <param name="cellReference">Address of the cell (ie. B2)</param>
        /// <returns>Column Name (ie. B)</returns>
        public string GetColumnName(string cellReference)
        {
            // Match the column name portion of the cell name.
            Regex regex = new Regex("[A-Za-z]+");
            Match match = regex.Match(cellReference);

            return match.Value;
        }

        /// <summary>
        /// Given just the column name (no row index),
        /// it will return the zero based column index.
        /// </summary>
        /// <param name="columnName">Column Name (ie. A or AB)</param>
        /// <returns>Zero based index if the conversion was successful</returns>
        /// <exception cref="ArgumentException">thrown if the given string
        /// contains characters other than uppercase letters</exception>
        public int ConvertColumnNameToNumber(string columnName)
        {
            Regex alpha = new Regex("^[A-Z]+$");
            if (!alpha.IsMatch(columnName)) throw new ArgumentException();

            char[] colLetters = columnName.ToCharArray();
            Array.Reverse(colLetters);

            int convertedValue = 0;
            for (int i = 0; i < colLetters.Length; i++)
            {
                char letter = colLetters[i];
                int current = i == 0 ? letter - 65 : letter - 64; // ASCII 'A' = 65
                convertedValue += current * (int)Math.Pow(26, i);
            }

            return convertedValue;
        }
        /*public void ExportExcel(System.Web.HttpContext context, string fileName, System.Data.DataTable table)
        {
            context.Response.Clear();
            context.Response.Buffer = true;
            context.Response.AddHeader("content-disposition", "attachment;filename=" + fileName + ".xls");
            context.Response.Charset = "";
            context.Response.ContentType = "application/vnd.ms-excel";
            using (System.IO.StringWriter sw = new System.IO.StringWriter())
            {
                System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(sw);
                System.Web.UI.WebControls.GridView gvData = new System.Web.UI.WebControls.GridView();
                gvData.DataSource = table;
                gvData.DataBind();
                #region "style"
                //gvData.HeaderRow.BackColor = System.Drawing.Color.White;
                //foreach (System.Web.UI.WebControls.TableCell cell in gvData.HeaderRow.Cells)
                //{
                //    cell.BackColor = System.Drawing.Color.SteelBlue;
                //}
                //foreach (System.Web.UI.WebControls.GridViewRow row in gvData.Rows)
                //{
                //    row.BackColor = System.Drawing.Color.White;
                //    foreach (System.Web.UI.WebControls.TableCell cell in row.Cells)
                //    {
                //        if (row.RowIndex % 2 == 0)
                //        {
                //            cell.BackColor = System.Drawing.Color.LightGray;
                //        }
                //        else
                //        {
                //            cell.BackColor = gvData.RowStyle.BackColor;
                //        }
                //        cell.CssClass = "textmode";
                //    }
                //}

                gvData.BorderColor = System.Drawing.Color.LightGray;
                foreach (System.Web.UI.WebControls.TableCell cell in gvData.HeaderRow.Cells)
                {
                    cell.BorderColor = System.Drawing.Color.LightGray;
                }
                foreach (System.Web.UI.WebControls.GridViewRow row in gvData.Rows)
                {
                    foreach (System.Web.UI.WebControls.TableCell cell in row.Cells)
                    {
                        cell.BorderColor = System.Drawing.Color.LightGray;
                    }
                }
                #endregion
                gvData.RenderControl(hw);

                string style = @"<style> .textmode { } </style>";
                context.Response.Write(style);
                context.Response.Output.Write(sw.ToString());
                context.Response.Flush();
                context.Response.End();
            }

        }*/
        public class SpreedsheetHelper
        {
            ///<summary>returns an empty cell when a blank cell is encountered
            ///</summary>
            public static IEnumerable<Cell> GetRowCells(Row row)
            {
                int currentCount = 0;

                foreach (DocumentFormat.OpenXml.Spreadsheet.Cell cell in
                    row.Descendants<DocumentFormat.OpenXml.Spreadsheet.Cell>())
                {
                    string columnName = GetColumnName(cell.CellReference);

                    int currentColumnIndex = ConvertColumnNameToNumber(columnName);

                    for (; currentCount < currentColumnIndex; currentCount++)
                    {
                        yield return new DocumentFormat.OpenXml.Spreadsheet.Cell();
                    }

                    yield return cell;
                    currentCount++;
                }
            }

            /// <summary>
            /// Given a cell name, parses the specified cell to get the column name.
            /// </summary>
            /// <param name="cellReference">Address of the cell (ie. B2)</param>
            /// <returns>Column Name (ie. B)</returns>
            public static string GetColumnName(string cellReference)
            {
                // Match the column name portion of the cell name.
                var regex = new System.Text.RegularExpressions.Regex("[A-Za-z]+");
                var match = regex.Match(cellReference);

                return match.Value;
            }

            /// <summary>
            /// Given just the column name (no row index),
            /// it will return the zero based column index.
            /// </summary>
            /// <param name="columnName">Column Name (ie. A or AB)</param>
            /// <returns>Zero based index if the conversion was successful</returns>
            /// <exception cref="ArgumentException">thrown if the given string
            /// contains characters other than uppercase letters</exception>
            public static int ConvertColumnNameToNumber(string columnName)
            {
                var alpha = new System.Text.RegularExpressions.Regex("^[A-Z]+$");
                if (!alpha.IsMatch(columnName)) throw new ArgumentException();

                char[] colLetters = columnName.ToCharArray();
                Array.Reverse(colLetters);

                int convertedValue = 0;
                for (int i = 0; i < colLetters.Length; i++)
                {
                    char letter = colLetters[i];
                    int current = i == 0 ? letter - 65 : letter - 64; // ASCII 'A' = 65
                    convertedValue += current * (int)Math.Pow(26, i);
                }

                return convertedValue;
            }
        }

        public DataTable ReadAsDataTable(string fileName)
        {
            DataTable dataTable = new DataTable();
            using (SpreadsheetDocument spreadSheetDocument = SpreadsheetDocument.Open(fileName, false))
            {
                WorkbookPart workbookPart = spreadSheetDocument.WorkbookPart;
                IEnumerable<Sheet> sheets = spreadSheetDocument.WorkbookPart.Workbook.GetFirstChild<Sheets>().Elements<Sheet>();
                string relationshipId = sheets.First().Id.Value;
                WorksheetPart worksheetPart = (WorksheetPart)spreadSheetDocument.WorkbookPart.GetPartById(relationshipId);
                Worksheet workSheet = worksheetPart.Worksheet;
                SheetData sheetData = workSheet.GetFirstChild<SheetData>();
                IEnumerable<Row> rows = sheetData.Descendants<Row>();

                foreach (Cell cell in rows.ElementAt(0))
                {
                    dataTable.Columns.Add(GetCellValue(spreadSheetDocument, cell));
                }

                foreach (Row row in rows)
                {
                    DataRow dataRow = dataTable.NewRow();
                    for (int i = 0; i < row.Descendants<Cell>().Count(); i++)
                    {
                        IEnumerable<Cell> cellList = SpreedsheetHelper.GetRowCells(row); ;
                        //IEnumerable<Cell> cellList = row.Descendants<Cell>();
                        /*string rowName = GetCellValue(spreadSheetDocument, rows.ElementAt(0).Descendants<Cell>().ElementAt(i));
                        var cellValue = GetCellValue(spreadSheetDocument, cellList.ElementAt(i));*/
                        dataRow[i] = GetCellValue(spreadSheetDocument, cellList.ElementAt(i));
                    }

                    dataTable.Rows.Add(dataRow);
                }

            }

            dataTable.Rows.RemoveAt(0);

            return dataTable;
        }

        public DataTable ReadAsDataTableForTransactionImport(string fileName)
        {
            DataTable dataTable = new DataTable();
            using (SpreadsheetDocument spreadSheetDocument = SpreadsheetDocument.Open(fileName, false))
            {
                WorkbookPart workbookPart = spreadSheetDocument.WorkbookPart;
                IEnumerable<Sheet> sheets = spreadSheetDocument.WorkbookPart.Workbook.GetFirstChild<Sheets>().Elements<Sheet>();
                string relationshipId = sheets.First().Id.Value;
                WorksheetPart worksheetPart = (WorksheetPart)spreadSheetDocument.WorkbookPart.GetPartById(relationshipId);
                Worksheet workSheet = worksheetPart.Worksheet;
                SheetData sheetData = workSheet.GetFirstChild<SheetData>();
                IEnumerable<Row> rows = sheetData.Descendants<Row>();

                foreach (Cell cell in rows.ElementAt(0))
                {
                    string cellValue = GetCellValue(spreadSheetDocument, cell);
                    string newCellValue = cellValue;
                    int count = 1;

                    while (dataTable.Columns.Cast<System.Data.DataColumn>().Select(e => e.ColumnName).Contains(newCellValue))
                    {
                        newCellValue =
                            cellValue +
                            SCC_BL.Settings.Overall.ImportTasks.Transaction.REPEATED_COLUMN +
                            SCC_BL.Settings.Overall.ImportTasks.Transaction.REPEATED_COLUMN_COUNT
                                .Replace(SCC_BL.Settings.Overall.ImportTasks.Transaction.REPEATED_COLUMN_COUNT_NUMBER, count.ToString());
                        count++;
                    }

                    dataTable.Columns.Add(newCellValue);
                }

                bool foundNonEmptyRow = false;

                foreach (Row row in rows)
                {
                    if (!foundNonEmptyRow)
                    {
                        // Check if the row is empty by examining its cell count
                        if (row.Descendants<Cell>().Any())
                            foundNonEmptyRow = true;
                        else
                            continue;
                    }

                    DataRow dataRow = dataTable.NewRow();
                    for (int i = 0; i < row.Descendants<Cell>().Count(); i++)
                    {
                        IEnumerable<Cell> cellList = SpreedsheetHelper.GetRowCells(row); ;
                        //IEnumerable<Cell> cellList = row.Descendants<Cell>();
                        /*string rowName = GetCellValue(spreadSheetDocument, rows.ElementAt(0).Descendants<Cell>().ElementAt(i));
                        var cellValue = GetCellValue(spreadSheetDocument, cellList.ElementAt(i));*/
                        dataRow[i] = GetCellValue(spreadSheetDocument, cellList.ElementAt(i));
                    }

                    dataTable.Rows.Add(dataRow);
                }

            }

            dataTable.Rows.RemoveAt(0);

            return dataTable;
        }

        private string GetCellValue(SpreadsheetDocument document, Cell cell)
        {
            if (cell == null)
                return null;

            if (cell.CellValue == null)
                return null;

            if (cell.CellValue.InnerXml == null)
                return null;

            SharedStringTablePart stringTablePart = document.WorkbookPart.SharedStringTablePart;

            string value = cell.CellValue.InnerXml;

            if (cell.DataType == null)
                return value;

            switch (cell.DataType.Value)
            {
                case CellValues.SharedString:
                    return stringTablePart.SharedStringTable.ChildElements[Int32.Parse(value)].InnerText;
                case CellValues.Boolean:
                    switch (value)
                    {
                        case "0":
                            return "FALSE";
                        default:
                            return "TRUE";
                    }
                default:
                    return value;
            }
        }
        public void ExportDataTableToExcelBytedFile(DataTable dataTable, string path)
        {
            // Create a new spreadsheet document
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Create(path, SpreadsheetDocumentType.Workbook))
            {
                // Add a new workbook to the document
                WorkbookPart workbookPart = spreadsheetDocument.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();

                // Add a new worksheet to the workbook
                WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                worksheetPart.Worksheet = new Worksheet(new SheetData());

                // Add a new sheet to the workbook
                Sheets sheets = spreadsheetDocument.WorkbookPart.Workbook.AppendChild<Sheets>(new Sheets());
                Sheet sheet = new Sheet() { Id = spreadsheetDocument.WorkbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "Sheet1" };
                sheets.Append(sheet);

                // Write the column headers to the worksheet
                SheetData sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();
                Row headerRow = new Row();
                foreach (DataColumn column in dataTable.Columns)
                {
                    headerRow.Append(new Cell() { DataType = CellValues.String, CellValue = new CellValue(column.ColumnName) });
                }
                sheetData.AppendChild(headerRow);

                // Write the data rows to the worksheet
                foreach (DataRow row in dataTable.Rows)
                {
                    Row dataRow = new Row();
                    foreach (DataColumn column in dataTable.Columns)
                    {
                        dataRow.Append(new Cell() { DataType = CellValues.String, CellValue = new CellValue(row[column].ToString()) });
                    }
                    sheetData.AppendChild(dataRow);
                }

                // Save the workbook and close the document
                workbookPart.Workbook.Save();
                spreadsheetDocument.Close();

                // Read the file content into a byte array and return a System.IO.File object.
                byte[] fileContent = System.IO.File.ReadAllBytes(path);
                System.IO.File.WriteAllBytes(path, fileContent);
            }
        }
        public static string GetExcelCellReference(int rowIndex, int columnIndex)
        {
            int dividend = columnIndex + 1;
            string columnName = String.Empty;

            while (dividend > 0)
            {
                int modulo = (dividend - 1) % 26;
                columnName = Convert.ToChar(65 + modulo).ToString() + columnName;
                dividend = (int)((dividend - modulo) / 26);
            }

            return columnName + (rowIndex - 1).ToString();
        }

        /*public void ExportDataTableToExcelBytedFile(DataTable dataTable, string path)
        {
            // Create a new spreadsheet document.
            using (SpreadsheetDocument document = SpreadsheetDocument.Create(path, SpreadsheetDocumentType.Workbook))
            {
                // Add a new workbook to the spreadsheet document.
                WorkbookPart workbookPart = document.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();

                // Add a new worksheet to the workbook.
                WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                worksheetPart.Worksheet = new Worksheet(new SheetData());

                // Add the data from the DataTable to the worksheet.
                SheetData sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();
                foreach (DataRow row in dataTable.Rows)
                {
                    Row worksheetRow = new Row();
                    foreach (DataColumn column in dataTable.Columns)
                    {
                        Cell worksheetCell = new Cell();
                        worksheetCell.DataType = CellValues.String;
                        worksheetCell.CellValue = new CellValue(row[column].ToString());
                        worksheetRow.AppendChild(worksheetCell);
                    }
                    sheetData.AppendChild(worksheetRow);
                }

                // Save the changes and close the spreadsheet document.
                workbookPart.Workbook.Save();
                document.Close();

                // Read the file content into a byte array and return a System.IO.File object.
                byte[] fileContent = System.IO.File.ReadAllBytes(path);
                System.IO.File.WriteAllBytes(path, fileContent);
            }
        }*/

        /*public void ExportDataTableToExcelBytedFile(DataTable dataTable, string path)
        {
            // Create a new spreadsheet document.
            using (SpreadsheetDocument document = SpreadsheetDocument.Create(path, SpreadsheetDocumentType.Workbook))
            {
                // Add a new workbook to the spreadsheet document.
                WorkbookPart workbookPart = document.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();

                // Add a new worksheet to the workbook.
                WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                worksheetPart.Worksheet = new Worksheet(new SheetData());

                // Add the data from the DataTable to the worksheet.
                SheetData sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();
                foreach (DataRow row in dataTable.Rows)
                {
                    Row worksheetRow = new Row();
                    foreach (DataColumn column in dataTable.Columns)
                    {
                        Cell worksheetCell = new Cell();
                        worksheetCell.DataType = CellValues.String;
                        worksheetCell.CellValue = new CellValue(row[column].ToString());
                        worksheetRow.AppendChild(worksheetCell);
                    }
                    sheetData.AppendChild(worksheetRow);
                }

                // Save the changes and close the spreadsheet document.
                workbookPart.Workbook.Save();
                document.Close();

                // Read the file content into a byte array and return a System.IO.File object.
                byte[] fileContent = System.IO.File.ReadAllBytes(path);
                System.IO.File.WriteAllBytes(path, fileContent);
            }
        }*/

        public void ExportTransactionListToExcel(List<Transaction> transactionList, string filePath)
        {
            if (transactionList.Count() == 0) return;

            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Create(filePath, SpreadsheetDocumentType.Workbook))
            {
                WorkbookPart workbookPart = spreadsheetDocument.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();

                WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                worksheetPart.Worksheet = new Worksheet(new SheetData());

                Sheets sheets = spreadsheetDocument.WorkbookPart.Workbook.AppendChild<Sheets>(new Sheets());

                Sheet sheet = new Sheet()
                {
                    Id = spreadsheetDocument.WorkbookPart.GetIdOfPart(worksheetPart),
                    SheetId = 1,
                    Name = "Transacciones"
                };

                sheets.Append(sheet);

                SheetData sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();

                // Create header row
                Row headerRow = new Row();

                headerRow.Append(CreateCell("Id de transacción"));
                headerRow.Append(CreateCell("Identificador de empleado"));
                headerRow.Append(CreateCell("Nombre del agente"));
                headerRow.Append(CreateCell("Nombre de supervisor"));
                headerRow.Append(CreateCell("Evaluador"));
                headerRow.Append(CreateCell("Programa"));
                headerRow.Append(CreateCell("Etiquetas"));
                headerRow.Append(CreateCell("Fecha de transacción"));
                headerRow.Append(CreateCell("Fecha de evaluación"));
                headerRow.Append(CreateCell("Fecha de Carga"));
                headerRow.Append(CreateCell("Fecha de Modificación"));
                headerRow.Append(CreateCell("Actualizado por el Usuario"));
                headerRow.Append(CreateCell("Tiempo invertido en monitoreo"));
                headerRow.Append(CreateCell("General (pasó/Falló)"));
                headerRow.Append(CreateCell("ECUF (Pasó/Falló)"));
                headerRow.Append(CreateCell("ECN (Pasó/Falló)"));
                headerRow.Append(CreateCell("ECC (Pasó/Falló)"));
                headerRow.Append(CreateCell("ENC (pts)"));
                headerRow.Append(CreateCell("Objetivo ECUF"));
                headerRow.Append(CreateCell("Objetivo ECN"));
                headerRow.Append(CreateCell("Objetivo ECC"));
                headerRow.Append(CreateCell("Objetivo ENC"));
                headerRow.Append(CreateCell("ECUF (Precisión)"));
                headerRow.Append(CreateCell("ECN (Precisión)"));
                headerRow.Append(CreateCell("ECC (Precisión)"));
                headerRow.Append(CreateCell("ECUF (Controlable)"));
                headerRow.Append(CreateCell("ECN (Controlable)"));
                headerRow.Append(CreateCell("ECC (Controlable)"));
                headerRow.Append(CreateCell("Comentario de la transacción"));
                headerRow.Append(CreateCell("Disputa"));
                headerRow.Append(CreateCell("Comentario de Disputa"));
                headerRow.Append(CreateCell("Usuario en disputa"));
                headerRow.Append(CreateCell("Fecha de Disputa"));
                headerRow.Append(CreateCell("Fecha de Devolución"));
                headerRow.Append(CreateCell("Comentario de Devolución"));
                headerRow.Append(CreateCell("Fortalezas"));
                headerRow.Append(CreateCell("Oportunidades"));
                headerRow.Append(CreateCell("Devolución al usuario"));
                headerRow.Append(CreateCell("Acción"));
                headerRow.Append(CreateCell("Fecha de acción"));
                headerRow.Append(CreateCell("Plan de acción"));
                headerRow.Append(CreateCell("Detalle de Seguimiento"));
                headerRow.Append(CreateCell("Fecha de seguimiento"));
                headerRow.Append(CreateCell("Cumplimiento de seguimiento"));
                headerRow.Append(CreateCell("Invalidado"));
                headerRow.Append(CreateCell("Usuario invalidado"));
                headerRow.Append(CreateCell("Fecha de invalidación"));
                headerRow.Append(CreateCell("Comentario de invalidación"));
                headerRow.Append(CreateCell("Coaching enviado"));
                headerRow.Append(CreateCell("Coaching leído"));

                Transaction templateTransaction = transactionList.FirstOrDefault();

                List<CustomField> customFieldList = new List<CustomField>();
                List<CustomControl> customControlList = new List<CustomControl>();
                List<Attribute> attributeList = new List<Attribute>();
                List<BusinessIntelligenceField> businessIntelligenceFieldList = new List<BusinessIntelligenceField>();

                foreach (TransactionCustomFieldCatalog currentTransactionCustomFieldCatalog in templateTransaction.CustomFieldList)
                {
                    using (CustomField currentCustomField = new CustomField(currentTransactionCustomFieldCatalog.CustomFieldID))
                    {
                        currentCustomField.SetDataByID();
                        customFieldList.Add(currentCustomField);

                        using (CustomControl currentCustomControl = new CustomControl(currentCustomField.CustomControlID))
                        {
                            currentCustomControl.SetDataByID();
                            customControlList.Add(currentCustomControl);
                        }
                    }
                }

                foreach (TransactionAttributeCatalog currentTransactionAttributeCatalog in templateTransaction.AttributeList)
                {
                    using (Attribute currentAttribute = new Attribute(currentTransactionAttributeCatalog.AttributeID))
                    {
                        currentAttribute.SetDataByID();

                        //if (currentAttribute.ParentAttributeID == null || currentAttribute.ParentAttributeID == 0)
                            attributeList.Add(currentAttribute);
                    }
                }

                foreach (TransactionBIFieldCatalog currentTransactionBIFieldCatalog in templateTransaction.BIFieldList)
                {
                    using (BusinessIntelligenceField currentBusinessIntelligenceField = new BusinessIntelligenceField(currentTransactionBIFieldCatalog.BIFieldID))
                    {
                        currentBusinessIntelligenceField.SetDataByID();

                        //if (currentBusinessIntelligenceField.ParentBIFieldID == null || currentBusinessIntelligenceField.ParentBIFieldID == 0)
                            businessIntelligenceFieldList.Add(currentBusinessIntelligenceField);
                    }
                }

                /*foreach (CustomField currentCustomField in customFieldList)
                {
                    using (CustomControl currentCustomControl = customControlList.Where(e => e.ID == currentCustomField.CustomControlID).FirstOrDefault())
                    {
                        headerRow.Append(CreateCell(currentCustomControl.Label));
                    }
                }*/

                foreach (CustomControl currentCustomControl in customControlList)
                {
                    headerRow.Append(CreateCell(currentCustomControl.Label));
                }

                int countField = 1;

                foreach (Attribute currentAttribute in attributeList.Where(e => e.ParentAttributeID == null || e.ParentAttributeID == 0))
                {
                    string errorType = string.Empty;

                    switch ((SCC_BL.DBValues.Catalog.ATTRIBUTE_ERROR_TYPE)currentAttribute.ErrorTypeID)
                    {
                        case DBValues.Catalog.ATTRIBUTE_ERROR_TYPE.FUCE:
                            errorType = "EUCE";
                            break;
                        case DBValues.Catalog.ATTRIBUTE_ERROR_TYPE.BCE:
                            errorType = "BCE";
                            break;
                        case DBValues.Catalog.ATTRIBUTE_ERROR_TYPE.FCE:
                            errorType = "CCE";
                            break;
                        case DBValues.Catalog.ATTRIBUTE_ERROR_TYPE.NCE:
                            errorType = "NCE";
                            break;
                        default:
                            break;
                    }

                    headerRow.Append(CreateCell($"{ currentAttribute.Name } ({ errorType })"));
                    headerRow.Append(CreateCell($"Comentarios_{ countField }"));
                    headerRow.Append(CreateCell($"Subatributos_{ countField }"));

                    countField++;
                }

                foreach (BusinessIntelligenceField currentBusinessIntelligenceField in businessIntelligenceFieldList.Where(e => e.ParentBIFieldID == null || e.ParentBIFieldID == 0))
                {
                    headerRow.Append(CreateCell($"BI: { currentBusinessIntelligenceField.Name }"));
                    headerRow.Append(CreateCell($"Comentarios_{countField}"));

                    countField++;
                }

                sheetData.Append(headerRow);

                foreach (Transaction transaction in transactionList)
                {
                    Row dataRow = new Row();

                    string currentSupervisorName = string.Empty;

                    if (transaction.UserToEvaluate.SupervisorList.Count > 0)
                    {
                        using (User supervisorUser = new User(transaction.UserToEvaluate.SupervisorList[0].SupervisorID))
                        {
                            supervisorUser.SetDataByID();

                            currentSupervisorName = $"{ supervisorUser.Person.SurName } { supervisorUser.Person.LastName }, { supervisorUser.Person.FirstName }";
                        }
                    }

                    dataRow.Append(CreateCell($"{ transaction.Identifier }"));
                    dataRow.Append(CreateCell($"{transaction.UserToEvaluate.Username}"));
                    dataRow.Append(CreateCell($"{ transaction.UserToEvaluate.Person.SurName } { transaction.UserToEvaluate.Person.LastName }, { transaction.UserToEvaluate.Person.FirstName }"));
                    dataRow.Append(CreateCell(currentSupervisorName));
                    dataRow.Append(CreateCell($"{transaction.EvaluatorUser.Person.SurName} {transaction.EvaluatorUser.Person.LastName}, {transaction.EvaluatorUser.Person.FirstName}"));
                    dataRow.Append(CreateCell($"{ transaction.Program.Name }"));
                    dataRow.Append(CreateCell(String.Join(",", transaction.TransactionLabelList.Select(e => e.Description))));
                    dataRow.Append(CreateCell(transaction.TransactionDate.ToString("dd/MM/yyyy HH:mm")));
                    dataRow.Append(CreateCell(transaction.EvaluationDate.ToString("dd/MM/yyyy HH:mm")));
                    dataRow.Append(CreateCell(string.Empty));
                    dataRow.Append(CreateCell(string.Empty));
                    dataRow.Append(CreateCell(string.Empty));
                    dataRow.Append(CreateCell(transaction.TimeElapsed.ToString(@"hh\:mm\:ss")));
                    dataRow.Append(CreateCell(transaction.GeneralResultID == (int)SCC_BL.DBValues.Catalog.TRANSACTION_GENERAL_RESULT_FINAL.SUCCESS ? "Pasó" : "Falló"));
                    dataRow.Append(CreateCell(transaction.GeneralFinalUserCriticalErrorResultID == (int)SCC_BL.DBValues.Catalog.TRANSACTION_GENERAL_RESULT_FINAL_USER_CRITICAL_ERROR.SUCCESS ? "Pasó" : "Falló"));
                    dataRow.Append(CreateCell(transaction.GeneralBusinessCriticalErrorResultID == (int)SCC_BL.DBValues.Catalog.TRANSACTION_GENERAL_RESULT_BUSINESS_CRITICAL_ERROR.SUCCESS ? "Pasó" : "Falló"));
                    dataRow.Append(CreateCell(transaction.GeneralFulfillmentCriticalErrorResultID == (int)SCC_BL.DBValues.Catalog.TRANSACTION_GENERAL_RESULT_FULFILLMENT_CRITICAL_ERROR.SUCCESS ? "Pasó" : "Falló"));
                    dataRow.Append(CreateCell(transaction.GeneralNonCriticalErrorResult.ToString() + "%"));
                    dataRow.Append(CreateCell("N/A"));
                    dataRow.Append(CreateCell("N/A"));
                    dataRow.Append(CreateCell("N/A"));
                    dataRow.Append(CreateCell("N/A"));
                    dataRow.Append(CreateCell(transaction.AccurateFinalUserCriticalErrorResultID == (int)SCC_BL.DBValues.Catalog.TRANSACTION_ACCURATE_RESULT_FINAL_USER_CRITICAL_ERROR.SUCCESS ? "Pasó" : "Falló"));
                    dataRow.Append(CreateCell(transaction.AccurateBusinessCriticalErrorResultID == (int)SCC_BL.DBValues.Catalog.TRANSACTION_ACCURATE_RESULT_BUSINESS_CRITICAL_ERROR.SUCCESS ? "Pasó" : "Falló"));
                    dataRow.Append(CreateCell(transaction.AccurateFulfillmentCriticalErrorResultID == (int)SCC_BL.DBValues.Catalog.TRANSACTION_ACCURATE_RESULT_FULFILLMENT_CRITICAL_ERROR.SUCCESS ? "Pasó" : "Falló"));
                    dataRow.Append(CreateCell(transaction.ControllableFinalUserCriticalErrorResultID == (int)SCC_BL.DBValues.Catalog.TRANSACTION_CONTROLLABLE_RESULT_FINAL_USER_CRITICAL_ERROR.SUCCESS ? "Pasó" : "Falló"));
                    dataRow.Append(CreateCell(transaction.ControllableBusinessCriticalErrorResultID == (int)SCC_BL.DBValues.Catalog.TRANSACTION_CONTROLLABLE_RESULT_BUSINESS_CRITICAL_ERROR.SUCCESS ? "Pasó" : "Falló"));
                    dataRow.Append(CreateCell(transaction.ControllableFulfillmentCriticalErrorResultID == (int)SCC_BL.DBValues.Catalog.TRANSACTION_CONTROLLABLE_RESULT_FULFILLMENT_CRITICAL_ERROR.SUCCESS ? "Pasó" : "Falló"));
                    dataRow.Append(CreateCell(transaction.Comment));
                    dataRow.Append(CreateCell(transaction.DisputeCommentaries.Count() > 0 ? "Yes" : "No"));
                    dataRow.Append(CreateCell(transaction.DisputeCommentaries.Count() > 0 ? transaction.DisputeCommentaries[0].Comment : string.Empty));
                    dataRow.Append(CreateCell($"{ transaction.UserToEvaluate.Username }"));
                    dataRow.Append(CreateCell(transaction.DisputeCommentaries.Count() > 0 ? transaction.DisputeCommentaries[0].BasicInfo.CreationDate.ToString("dd/MM/yyyy HH:mm:ss") : string.Empty));
                    dataRow.Append(CreateCell(transaction.DevolutionCommentaries.Count() > 0 ? transaction.DevolutionCommentaries[0].BasicInfo.CreationDate.ToString("dd/MM/yyyy HH:mm:ss") : string.Empty));
                    dataRow.Append(CreateCell(transaction.DevolutionCommentaries.Count() > 0 ? transaction.DevolutionCommentaries[0].Comment : string.Empty));
                    dataRow.Append(CreateCell(transaction.DevolutionUserStrengths.Count() > 0 ? transaction.DevolutionUserStrengths[0].Comment : string.Empty));
                    dataRow.Append(CreateCell(transaction.DevolutionImprovementSteps.Count() > 0 ? transaction.DevolutionImprovementSteps[0].Comment : string.Empty));
                    dataRow.Append(CreateCell($"{transaction.UserToEvaluate.Username}"));
                    dataRow.Append(CreateCell(string.Empty));
                    dataRow.Append(CreateCell(string.Empty));
                    dataRow.Append(CreateCell(string.Empty));
                    dataRow.Append(CreateCell(string.Empty));
                    dataRow.Append(CreateCell(string.Empty));
                    dataRow.Append(CreateCell(string.Empty));
                    dataRow.Append(CreateCell(transaction.InvalidationCommentaries.Count() > 0 ? "Yes" : "No"));
                    dataRow.Append(CreateCell($"{ transaction.UserToEvaluate.Username }"));
                    dataRow.Append(CreateCell(transaction.InvalidationCommentaries.Count() > 0 ? transaction.InvalidationCommentaries[0].BasicInfo.CreationDate.ToString("dd/MM/yyyy HH:mm:ss") : string.Empty));
                    dataRow.Append(CreateCell(transaction.InvalidationCommentaries.Count() > 0 ? transaction.InvalidationCommentaries[0].Comment : string.Empty));
                    dataRow.Append(CreateCell(string.Empty));
                    dataRow.Append(CreateCell(string.Empty));

                    /*foreach (CustomField currentCustomField in customFieldList)
                    {
                        if (transaction.CustomFieldList.Select(e => e.CustomFieldID).Contains(currentCustomField.ID))
                        {
                            using (CustomControl currentCustomControl = customControlList.Where(e => e.ID == currentCustomField.CustomControlID).FirstOrDefault())
                            {
                                currentCustomField.SetDataByID();
                                customFieldList.Add(currentCustomField);

                                using (CustomControl currentCustomControl = new CustomControl(currentCustomField.CustomControlID))
                                {
                                    currentCustomControl.SetDataByID();
                                    customControlList.Add(currentCustomControl);
                                }
                            }
                        }
                        else
                        {
                            dataRow.Append(CreateCell(string.Empty));
                        }
                    }*/

                    foreach (CustomControl currentCustomControl in customControlList)
                    {
                        string customControlValue = string.Empty;

                        using (CustomField currentCustomField = customFieldList.Where(e => e.CustomControlID == currentCustomControl.ID).FirstOrDefault())
                        {
                            if (transaction.CustomFieldList.Select(e => e.CustomFieldID).Contains(currentCustomField.ID))
                            {
                                TransactionCustomFieldCatalog currentTransactionCustomFieldCatalog = transaction.CustomFieldList.Where(e => e.CustomFieldID == currentCustomField.ID).FirstOrDefault();

                                SCC_BL.DBValues.Catalog.CUSTOM_CONTROL_TYPE customControlType = (SCC_BL.DBValues.Catalog.CUSTOM_CONTROL_TYPE)currentCustomControl.ControlTypeID;

                                if (
                                    customControlType == SCC_BL.DBValues.Catalog.CUSTOM_CONTROL_TYPE.TEXT_AREA ||
                                    customControlType == SCC_BL.DBValues.Catalog.CUSTOM_CONTROL_TYPE.TEXT_BOX)
                                {
                                    switch (currentCustomControl.Mask)
                                    {
                                        case SCC_BL.Settings.AppValues.Masks.Alphanumeric1.MASK:
                                            break;
                                        case SCC_BL.Settings.AppValues.Masks.Date1.MASK:
                                            break;
                                        case SCC_BL.Settings.AppValues.Masks.Email1.MASK:
                                            break;
                                        case SCC_BL.Settings.AppValues.Masks.LastName1.MASK:
                                            break;
                                        case SCC_BL.Settings.AppValues.Masks.Name1.MASK:
                                            break;
                                        case SCC_BL.Settings.AppValues.Masks.PhoneNumber1.MASK:
                                            break;
                                        case SCC_BL.Settings.AppValues.Masks.PhoneNumber2.MASK:
                                            break;
                                        case SCC_BL.Settings.AppValues.Masks.Time1.MASK:
                                            break;
                                        default:
                                            customControlValue = currentTransactionCustomFieldCatalog.Comment;
                                            break;
                                    }
                                }
                                else
                                if (
                                    customControlType == SCC_BL.DBValues.Catalog.CUSTOM_CONTROL_TYPE.CHECKBOX ||
                                    customControlType == SCC_BL.DBValues.Catalog.CUSTOM_CONTROL_TYPE.RADIO_BUTTON ||
                                    customControlType == SCC_BL.DBValues.Catalog.CUSTOM_CONTROL_TYPE.SELECT_LIST)
                                {
                                    var auxCurrentCustomControlValue = currentCustomControl.ValueList.Where(e => e.ID == currentTransactionCustomFieldCatalog.ValueID).FirstOrDefault();
                                    customControlValue =
                                        auxCurrentCustomControlValue != null
                                            ? auxCurrentCustomControlValue.Value
                                            : string.Empty;
                                }
                            }
                        }

                        dataRow.Append(CreateCell(customControlValue));
                    }

                    foreach (Attribute currentAttribute in attributeList.Where(e => e.ParentAttributeID == null || e.ParentAttributeID == 0))
                    {
                        string attributeSuccess = "SI";
                        string attributeComment = "-";
                        string attributeSubattributes = "~";

                        if (transaction.AttributeList.Select(e => e.AttributeID).Contains(currentAttribute.ID))
                        {
                            TransactionAttributeCatalog currentTransactionAttributeCatalog = transaction.AttributeList.Where(e => e.AttributeID == currentAttribute.ID).FirstOrDefault();
                            AttributeValueCatalog currrentAttributeValueCatalog = currentAttribute.ValueList.Where(e => e.ID == currentTransactionAttributeCatalog.ValueID).FirstOrDefault();

                            attributeComment = currentTransactionAttributeCatalog.Comment;

                            if (currrentAttributeValueCatalog.TriggersChildVisualization)
                            {
                                attributeSubattributes = "";

                                attributeSuccess = "NO";

                                List<Attribute> childAttributeList = currentAttribute.ChildrenList;

                                foreach (TransactionAttributeCatalog childTransactionAttributeCatalog in transaction.AttributeList.Where(e => childAttributeList.Select(s => s.ID).Contains(e.AttributeID) && e.Checked))
                                {
                                    Attribute auxCurrentAttribute = childAttributeList.Where(e => e.ID == childTransactionAttributeCatalog.AttributeID).FirstOrDefault();

                                    attributeSubattributes += $"~~{ auxCurrentAttribute.Name }";

                                    List<Attribute> auxChildAttributeList = attributeList.Where(e => e.ParentAttributeID == auxCurrentAttribute.ID).ToList();
                                    TransactionAttributeCatalog auxChildTransactionAttributeCatalog = transaction.AttributeList.Where(e => auxChildAttributeList.Select(s => s.ID).Contains(e.AttributeID) && e.Checked).FirstOrDefault();

                                    Attribute auxChildAttribute =
                                        auxChildTransactionAttributeCatalog != null
                                            ? attributeList
                                                .Where(e => e.ID == auxChildTransactionAttributeCatalog.AttributeID)
                                                .FirstOrDefault()
                                            : null;

                                    attributeSubattributes +=
                                        auxChildAttribute != null 
                                            ? $"~{auxChildAttribute.Name}"
                                            : "~";

                                    if (auxChildAttribute == null) continue;

                                    while (attributeList.Where(e => e.ParentAttributeID == auxChildAttribute.ID).Count() > 0)
                                    {
                                        auxChildAttributeList = attributeList.Where(e => e.ParentAttributeID == auxChildAttribute.ID).ToList();
                                        auxChildTransactionAttributeCatalog = transaction.AttributeList.Where(e => auxChildAttributeList.Select(s => s.ID).Contains(e.AttributeID) && e.Checked).FirstOrDefault();

                                        auxChildAttribute = attributeList.Where(e => e.ID == auxChildTransactionAttributeCatalog.AttributeID).FirstOrDefault();

                                        attributeSubattributes += $"~{auxChildAttribute.Name}";
                                    }
                                }
                            }
                        }

                        dataRow.Append(CreateCell(attributeSuccess));
                        dataRow.Append(CreateCell(attributeComment));
                        dataRow.Append(CreateCell(attributeSubattributes));
                    }

                    foreach (BusinessIntelligenceField currentBusinessIntelligenceField in businessIntelligenceFieldList.Where(e => e.ParentBIFieldID == null || e.ParentBIFieldID == 0))
                    {
                        string businessIntelligenceFieldComment = "-";
                        string businessIntelligenceFieldChildren = string.Empty;

                        if (transaction.BIFieldList.Select(e => e.BIFieldID).Contains(currentBusinessIntelligenceField.ID))
                        {
                            TransactionBIFieldCatalog currentTransactionBIFieldCatalog = transaction.BIFieldList.Where(e => e.BIFieldID == currentBusinessIntelligenceField.ID).FirstOrDefault();

                            businessIntelligenceFieldComment = currentTransactionBIFieldCatalog.Comment;

                            if (currentTransactionBIFieldCatalog.Checked)
                            {
                                businessIntelligenceFieldChildren = "";

                                List<BusinessIntelligenceField> childBusinessIntelligenceFieldList = currentBusinessIntelligenceField.ChildList;

                                foreach (TransactionBIFieldCatalog childTransactionBIFieldCatalog in transaction.BIFieldList.Where(e => childBusinessIntelligenceFieldList.Select(s => s.ID).Contains(e.BIFieldID) && e.Checked))
                                {
                                    BusinessIntelligenceField auxCurrentBusinessIntelligenceField = childBusinessIntelligenceFieldList.Where(e => e.ID == childTransactionBIFieldCatalog.BIFieldID).FirstOrDefault();

                                    businessIntelligenceFieldChildren += $"~~{auxCurrentBusinessIntelligenceField.Name}";

                                    List<BusinessIntelligenceField> auxChildBusinessIntelligenceFieldList = businessIntelligenceFieldList.Where(e => e.ParentBIFieldID == auxCurrentBusinessIntelligenceField.ID).ToList();
                                    TransactionBIFieldCatalog auxChildTransactionBIFieldCatalog = 
                                        transaction.BIFieldList
                                            .Where(e => 
                                                auxChildBusinessIntelligenceFieldList.Select(s => 
                                                    s.ID)
                                                .Contains(e.BIFieldID) && e.Checked)
                                            .FirstOrDefault();

                                    BusinessIntelligenceField auxChildBusinessIntelligenceField =
                                        auxChildTransactionBIFieldCatalog != null
                                            ? businessIntelligenceFieldList
                                                .Where(e => e.ID == auxChildTransactionBIFieldCatalog.BIFieldID)
                                                .FirstOrDefault()
                                            : null;

                                    businessIntelligenceFieldChildren +=
                                        auxChildBusinessIntelligenceField!= null
                                            ? $"~{auxChildBusinessIntelligenceField.Name}"
                                            : string.Empty;

                                    if (auxChildBusinessIntelligenceField == null) continue;

                                    while (businessIntelligenceFieldList.Where(e => e.ParentBIFieldID == auxChildBusinessIntelligenceField.ID).Count() > 0)
                                    {
                                        auxChildBusinessIntelligenceFieldList = businessIntelligenceFieldList.Where(e => e.ParentBIFieldID == auxChildBusinessIntelligenceField.ID).ToList();
                                        auxChildTransactionBIFieldCatalog = transaction.BIFieldList.Where(e => auxChildBusinessIntelligenceFieldList.Select(s => s.ID).Contains(e.BIFieldID) && e.Checked).FirstOrDefault();

                                        auxChildBusinessIntelligenceField = businessIntelligenceFieldList.Where(e => e.ID == auxChildTransactionBIFieldCatalog.BIFieldID).FirstOrDefault();

                                        businessIntelligenceFieldChildren +=
                                            auxChildBusinessIntelligenceField != null
                                                ? $"~{auxChildBusinessIntelligenceField.Name}"
                                                : string.Empty;
                                    }
                                }
                            }
                        }

                        dataRow.Append(CreateCell(businessIntelligenceFieldChildren));
                        dataRow.Append(CreateCell(businessIntelligenceFieldComment));
                    }

                    sheetData.Append(dataRow);
                }

                worksheetPart.Worksheet.Save();
                spreadsheetDocument.Close();
            }
        }

        private Cell CreateCell(string cellValue)
        {
            return new Cell
            {
                DataType = CellValues.InlineString,
                InlineString = new InlineString(new DocumentFormat.OpenXml.Spreadsheet.Text { Space = SpaceProcessingModeValues.Preserve, Text = cellValue })
            };

            //return new Cell(new InlineString(new DocumentFormat.OpenXml.Drawing.Text(cellValue)));
        }

        public void Dispose()
        {

        }
    }
}
