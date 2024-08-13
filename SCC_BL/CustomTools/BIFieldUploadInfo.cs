using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCC_BL.CustomTools
{
    public class BIFieldUploadInfo
    {
        public List<UploadInfo> UploadInfoList { get; set; } = new List<UploadInfo>();

        public class UploadInfo
        {
            public int FirstIndex { get; set; }
            public int LastIndex { get; set; }
            public int DescriptionIndex { get; set; }
            public List<BIFieldListInfo> BIFieldList { get; set; } = new List<BIFieldListInfo>();

            public class BIFieldListInfo
            {
                public int RowIndex { get; set; }
                public int ColumnIndex { get; set; }

                public BIFieldListInfo()
                {

                }

                public BIFieldListInfo(int rowIndex, int columnIndex)
                {
                    this.RowIndex = rowIndex;
                    this.ColumnIndex = columnIndex;
                }
            }

            public UploadInfo()
            {

            }

            public UploadInfo(int firstIndex, int lastIndex)
            {
                this.FirstIndex = firstIndex;
                this.LastIndex = lastIndex;
            }

            public void FillBIFieldListInfo(IEnumerable<DocumentFormat.OpenXml.Spreadsheet.Row> rowCollection)
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
                            //cellIndex = excelParser.NextColumnWithDataIndex(currentCellArray, cellIndex.Value + 1);
                            cellIndex = excelParser.NextColumnWithDataIndex(currentCellArray, cellIndex.Value);

                            if (cellIndex == null) break;

                            if (cellIndex >= DescriptionIndex) break;

                            string cellValue = excelParser.GetCellValue(currentCellArray[cellIndex.Value]).ToString().Trim();

                            if (!string.IsNullOrEmpty(cellValue))
                            {
                                this.BIFieldList.Add(new BIFieldListInfo(rowIndex, cellIndex.Value));
                            }

                            cellIndex++;
                        }
                    }
                }
            }
        }

        public void FillUploadInfo(IEnumerable<DocumentFormat.OpenXml.Spreadsheet.Row> rowCollection, int headersCount)
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
                        SCC_BL.CustomTools.BIFieldUploadInfo.UploadInfo uploadInfo = new SCC_BL.CustomTools.BIFieldUploadInfo.UploadInfo();

                        uploadInfo.FirstIndex = rowCount;

                        for (int i = rowCount + 1; i < rowCollection.Count(); i++)
                        {
                            DocumentFormat.OpenXml.Spreadsheet.Cell[] auxCurrentRow = excelParser.GetRowCells(rowCollection.ElementAt(i), headersCount).ToArray();

                            string auxCellValue = excelParser.GetCellValue(auxCurrentRow[0]).ToString().Trim();

                            if (!string.IsNullOrEmpty(auxCellValue)) {
                                uploadInfo.LastIndex = i - 1;
                                break;
                            }
                        }

                        if (uploadInfo.LastIndex <= uploadInfo.FirstIndex) uploadInfo.LastIndex = rowCollection.Count() - 1;

                        if (this.UploadInfoList.Count > 0)
                        {
                            uploadInfo.DescriptionIndex = this.UploadInfoList.FirstOrDefault().DescriptionIndex;
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
                                        uploadInfo.DescriptionIndex = nextColumnIndex.Value;
                                }
                                else
                                {
                                    uploadInfo.DescriptionIndex = nextColumnWithDataIndex.Value;
                                }
                            }
                        }

                        uploadInfo.FillBIFieldListInfo(rowCollection);

                        this.UploadInfoList.Add(uploadInfo);
                    }
                }

                rowCount++;
            }
        }
    }
}
