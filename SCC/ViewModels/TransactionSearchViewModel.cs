using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCC.ViewModels
{
    public class TransactionSearchViewModel
    {
        public SCC_BL.Helpers.Transaction.Search.TransactionSearchHelper TransactionSearchHelper { get; set; } = new SCC_BL.Helpers.Transaction.Search.TransactionSearchHelper();
        public List<SCC_BL.Transaction> TransactionList { get; set; } = new List<SCC_BL.Transaction>();
    }
}