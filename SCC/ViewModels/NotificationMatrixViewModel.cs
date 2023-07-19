using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCC.ViewModels
{
    public class NotificationMatrixViewModel
    {
        public bool DirectSupervisorTransactionDispute { get; set; }
        public bool DirectSupervisorTransactionDevolution { get; set; }
        public bool DirectSupervisorTransactionInvalidation { get; set; }
        public bool DirectSupervisorTransactionConfirmation { get; set; }
        public bool DirectSupervisorTeamMonitoringZero { get; set; }
        public bool DirectSupervisorCalibration { get; set; }
        
        public bool IndirectSupervisorTransactionDispute { get; set; }
        public bool IndirectSupervisorTransactionDevolution { get; set; }
        public bool IndirectSupervisorTransactionInvalidation { get; set; }
        public bool IndirectSupervisorTransactionConfirmation { get; set; }
        public bool IndirectSupervisorTeamMonitoringZero { get; set; }
        public bool IndirectSupervisorCalibration { get; set; }
        
        public bool MonitoredUserTransactionDispute { get; set; }
        public bool MonitoredUserTransactionDevolution { get; set; }
        public bool MonitoredUserTransactionInvalidation { get; set; }
        public bool MonitoredUserTransactionConfirmation { get; set; }
        public bool MonitoredUserTeamMonitoringZero { get; set; }
        public bool MonitoredUserCalibration { get; set; }
        
        public bool MonitoringUserTransactionDispute { get; set; }
        public bool MonitoringUserTransactionDevolution { get; set; }
        public bool MonitoringUserTransactionInvalidation { get; set; }
        public bool MonitoringUserTransactionConfirmation { get; set; }
        public bool MonitoringUserTeamMonitoringZero { get; set; }
        public bool MonitoringUserCalibration { get; set; }
    }
}