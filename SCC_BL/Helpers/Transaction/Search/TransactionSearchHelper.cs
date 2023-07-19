using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCC_BL.Helpers.Transaction.Search
{
    public class TransactionSearchHelper
    {
        public int? UserIdentificationTypeID { get; set; }
        public string UserIdentification { get; set; }
        public int? UserStatusID { get; set; }
        public int[] WorkspaceIDList { get; set; }
        public int? MonitorUserIdentificationTypeID { get; set; }
        public string MonitorUserIdentification { get; set; }
        public int[] ProgramIDList { get; set; }
        public string TransactionIdentifier { get; set; }
        public DateTime? TransactionDateFrom { get; set; }
        public DateTime? TransactionDateTo { get; set; }
        public int? TransactionDateSinceTypeID { get; set; }
        public int? TransactionDateSince { get; set; }
        public DateTime? EvaluationDateFrom { get; set; }
        public DateTime? EvaluationDateTo { get; set; }
        public int? EvaluationDateSinceTypeID { get; set; }
        public int? EvaluationDateSince { get; set; }
        public string TransactionCommentText { get; set; }
        public string AttributeNameText { get; set; }
        public string AttributeCommentText { get; set; }
        public string TransactionLabelText { get; set; }
        public string DisputationText { get; set; }
        public string InvalidationText { get; set; }
        public bool? CoachingSent { get; set; }
        public bool? CoachingRead { get; set; }
        public int? ControllableErrorFilterTypeID { get; set; }
        public string DevolutionCommentText { get; set; }
        public string DevolutionStrengthCommentText { get; set; }
        public string DevolutionImprovementStepsCommentText { get; set; }
        public string CustomFieldLabelText { get; set; }
        public string CustomFieldValueText { get; set; }
    }
}
