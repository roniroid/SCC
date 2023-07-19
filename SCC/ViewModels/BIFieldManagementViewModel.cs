using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCC_BL;

namespace SCC.ViewModels
{
    public class BIFieldManagementViewModel
    {
        public BusinessIntelligenceField BusinessIntelligenceField { get; set; } = new BusinessIntelligenceField();
        public List<BusinessIntelligenceField> BIFieldList { get; set; } = new List<BusinessIntelligenceField>();
    }
}