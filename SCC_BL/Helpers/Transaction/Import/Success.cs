using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCC_BL.Helpers.Transaction.Import
{
    public class Success
    {
        public string OldIdentifier { get; set; }
        public string NewIdentifier { get; set; }
        public int RowIndex { get; set; }

        public Success(string oldIdentifier, string newIdentifier, int rowIndex)
        {
            this.OldIdentifier = oldIdentifier;
            this.NewIdentifier = newIdentifier;
            this.RowIndex = rowIndex + 2;
        }
    }
}
