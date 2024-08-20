using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SCC_BL.Helpers.Transaction.Import
{
    public class Error
    {
        public string ElementName { get; set; }
        public string Content { get; set; }
        public int RowNumber { get; set; }
        public int ColumnNumber { get; set; }
        public string CellReference { get; set; }
        public SCC_BL.Settings.Notification.Type Type { get; set; } = SCC_BL.Settings.Notification.Type.ERROR;

        public Error(string elementName, string content, int rowIndex, int columnIndex, SCC_BL.Settings.Notification.Type type = SCC_BL.Settings.Notification.Type.ERROR)
        {
            this.ElementName = elementName;
            this.Content = content;
            this.RowNumber = rowIndex + 2;
            this.ColumnNumber = columnIndex + 1;
            this.Type = type;
            this.CellReference = SCC_BL.Tools.ExcelParser.GetExcelCellReference(rowIndex + 3, columnIndex);
        }
    }
}
