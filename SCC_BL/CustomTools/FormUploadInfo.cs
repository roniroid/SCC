using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCC_BL.CustomTools
{
    public class FormUploadInfo
    {
        public List<ErrorTypeInfo> ErrorTypeList { get; set; } = new List<ErrorTypeInfo>();

        public class ErrorTypeInfo
        {
            public SCC_BL.DBValues.Catalog.ATTRIBUTE_ERROR_TYPE ErrorTypeEnum { get; set; }
            public int FirstIndex { get; set; }
            public int LastIndex { get; set; }
            public int DescriptionIndex { get; set; }
            public List<AttributeListInfo> AttributeList { get; set; } = new List<AttributeListInfo>();

            public class AttributeListInfo
            {
                public int RowIndex { get; set; }
                public int ColumnIndex { get; set; }

                public AttributeListInfo()
                {

                }

                public AttributeListInfo(int rowIndex, int columnIndex)
                {
                    this.RowIndex = rowIndex;
                    this.ColumnIndex = columnIndex;
                }
            }

            public ErrorTypeInfo()
            {

            }

            public ErrorTypeInfo(SCC_BL.DBValues.Catalog.ATTRIBUTE_ERROR_TYPE errorTypeEnum, int firstIndex, int lastIndex)
            {
                this.ErrorTypeEnum = errorTypeEnum;
                this.FirstIndex = firstIndex;
                this.LastIndex = lastIndex;
            }

            public void FillAttributeListInfo(IEnumerable<DocumentFormat.OpenXml.Spreadsheet.Row> rowCollection)
            {
                int headersCount = rowCollection.ElementAt(0).Count();

                using (SCC_BL.Tools.ExcelParser excelParser = new SCC_BL.Tools.ExcelParser())
                {
                    for (int rowIndex = FirstIndex; rowIndex <= LastIndex; rowIndex++)
                    {
                        DocumentFormat.OpenXml.Spreadsheet.Row currentRow = rowCollection.ElementAt(rowIndex);
                        DocumentFormat.OpenXml.Spreadsheet.Cell[] currentCellArray = excelParser.GetRowCells(currentRow, headersCount).ToArray();

                        int? cellIndex = 0;

                        while (cellIndex < DescriptionIndex)
                        {
                            cellIndex = excelParser.NextColumnWithDataIndex(currentCellArray, cellIndex.Value + 1);

                            if (cellIndex == null) break;

                            if (cellIndex >= DescriptionIndex) break;

                            string cellValue = excelParser.GetCellValue(currentCellArray[cellIndex.Value]).ToString().Trim();

                            if (!string.IsNullOrEmpty(cellValue))
                            {
                                this.AttributeList.Add(new AttributeListInfo(rowIndex, cellIndex.Value));
                            }
                        }
                    }
                }
            }
        }

        public void FillErrorTypeInfo(IEnumerable<DocumentFormat.OpenXml.Spreadsheet.Row> rowCollection, int headersCount)
        {
            int rowCount = 0;

            foreach (DocumentFormat.OpenXml.Spreadsheet.Row row in rowCollection)
            {
                using (SCC_BL.Tools.ExcelParser excelParser = new SCC_BL.Tools.ExcelParser())
                {
                    DocumentFormat.OpenXml.Spreadsheet.Cell[] currentRow = excelParser.GetRowCells(row, headersCount).ToArray();

                    string cellValue = excelParser.GetCellValue(currentRow[0]).ToString().Trim();

                    if (!string.IsNullOrEmpty(cellValue))
                    {
                        SCC_BL.CustomTools.FormUploadInfo.ErrorTypeInfo errorTypeInfo = new SCC_BL.CustomTools.FormUploadInfo.ErrorTypeInfo();

                        errorTypeInfo.FirstIndex = rowCount;

                        for (int i = rowCount + 1; i < rowCollection.Count(); i++)
                        {
                            DocumentFormat.OpenXml.Spreadsheet.Cell[] auxCurrentRow = excelParser.GetRowCells(rowCollection.ElementAt(i), headersCount).ToArray();

                            string auxCellValue = excelParser.GetCellValue(auxCurrentRow[0]).ToString().Trim();

                            if (!string.IsNullOrEmpty(auxCellValue)) {
                                errorTypeInfo.LastIndex = i - 1;
                                break;
                            }
                        }

                        if (errorTypeInfo.LastIndex < errorTypeInfo.FirstIndex) errorTypeInfo.LastIndex = rowCollection.Count() - 1;

                        switch (cellValue)
                        {
                            case SCC_BL.Settings.Overall.ErrorType.ECUF:
                                errorTypeInfo.ErrorTypeEnum = DBValues.Catalog.ATTRIBUTE_ERROR_TYPE.FUCE;
                                break;
                            case SCC_BL.Settings.Overall.ErrorType.ECN:
                                errorTypeInfo.ErrorTypeEnum = DBValues.Catalog.ATTRIBUTE_ERROR_TYPE.BCE;
                                break;
                            case SCC_BL.Settings.Overall.ErrorType.ECC:
                                errorTypeInfo.ErrorTypeEnum = DBValues.Catalog.ATTRIBUTE_ERROR_TYPE.FCE;
                                break;
                            case SCC_BL.Settings.Overall.ErrorType.ENC:
                                errorTypeInfo.ErrorTypeEnum = DBValues.Catalog.ATTRIBUTE_ERROR_TYPE.NCE;
                                break;
                            case SCC_BL.Settings.Overall.ErrorType.BCE:
                                errorTypeInfo.ErrorTypeEnum = DBValues.Catalog.ATTRIBUTE_ERROR_TYPE.BCE;
                                break;
                            case SCC_BL.Settings.Overall.ErrorType.CCE:
                                errorTypeInfo.ErrorTypeEnum = DBValues.Catalog.ATTRIBUTE_ERROR_TYPE.FCE;
                                break;
                            case SCC_BL.Settings.Overall.ErrorType.NCE:
                                errorTypeInfo.ErrorTypeEnum = DBValues.Catalog.ATTRIBUTE_ERROR_TYPE.NCE;
                                break;
                            case SCC_BL.Settings.Overall.ErrorType.UCE:
                                errorTypeInfo.ErrorTypeEnum = DBValues.Catalog.ATTRIBUTE_ERROR_TYPE.FUCE;
                                break;
                            case SCC_BL.Settings.Overall.ErrorType.EUCE:
                                errorTypeInfo.ErrorTypeEnum = DBValues.Catalog.ATTRIBUTE_ERROR_TYPE.FUCE;
                                break;
                            default:
                                break;
                        }

                        if (this.ErrorTypeList.Count > 0)
                        {
                            errorTypeInfo.DescriptionIndex = this.ErrorTypeList.FirstOrDefault().DescriptionIndex;
                        }
                        else
                        {
                            int? nextColumnWithDataIndex = excelParser.NextColumnWithDataIndex(currentRow, 1);

                            if (nextColumnWithDataIndex != null)
                            {
                                //1 being the second column
                                if (nextColumnWithDataIndex.Value == 1)
                                {
                                    int? nextColumnIndex = excelParser.NextColumnWithDataIndex(currentRow, nextColumnWithDataIndex.Value + 1);

                                    if (nextColumnIndex != null)
                                        errorTypeInfo.DescriptionIndex = nextColumnIndex.Value;
                                }
                                else
                                {
                                    errorTypeInfo.DescriptionIndex = nextColumnWithDataIndex.Value;
                                }
                            }
                        }

                        errorTypeInfo.FillAttributeListInfo(rowCollection);

                        this.ErrorTypeList.Add(errorTypeInfo);
                    }
                }

                rowCount++;
            }
        }
    }
}
