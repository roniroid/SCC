using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCC.ViewModels
{
    public class TransactionFormViewModel
    {
        public SCC_BL.Form Form { get; set; } = new SCC_BL.Form();
        public SCC_BL.Transaction Transaction { get; set; } = new SCC_BL.Transaction();
    }
}