using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCC.ViewModels
{
    public class TransactionImportResults
    {
        public List<SCC_BL.Helpers.Transaction.Import.Error> TransactionImportErrorList { get; set; } = new List<SCC_BL.Helpers.Transaction.Import.Error>();
        public List<SCC_BL.Helpers.Transaction.Import.Success> TransactionImportSuccessList { get; set; } = new List<SCC_BL.Helpers.Transaction.Import.Success>();
        public string FilePath { get; set; }

        public TransactionImportResults(List<SCC_BL.Helpers.Transaction.Import.Error> transactionImportErrorList, List<SCC_BL.Helpers.Transaction.Import.Success> transactionImportSuccessList, string filePath)
        {
            this.TransactionImportErrorList = transactionImportErrorList;
            this.TransactionImportSuccessList = transactionImportSuccessList;
            this.FilePath = filePath;
        }
    }
}