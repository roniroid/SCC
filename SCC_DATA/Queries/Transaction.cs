using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCC_DATA.Queries
{
	public class Transaction
	{
		public struct Fields
		{
			public const string ID = "ID";
			public const string IDENTIFIER = "Identifier";
			public const string USERTOEVALUATEID = "UserToEvaluateID";
			public const string EVALUATORUSERID = "EvaluatorUserID";
			public const string EVALUATIONDATE = "EvaluationDate";
			public const string TRANSACTIONDATE = "TransactionDate";
			public const string FORMID = "FormID";
			public const string COMMENT = "Comment";
			public const string GENERALRESULTID = "GeneralResultID";
			public const string GENERALFINALUSERCRITICALERRORRESULTID = "GeneralFinalUserCriticalErrorResultID";
			public const string GENERALBUSINESSCRITICALERRORRESULTID = "GeneralBusinessCriticalErrorResultID";
			public const string GENERALFULFILLMENTCRITICALERRORRESULTID = "GeneralFulfillmentCriticalErrorResultID";
			public const string GENERALNONCRITICALERRORRESULT = "GeneralNonCriticalErrorResult";
			public const string ACCURATERESULTID = "AccurateResultID";
			public const string ACCURATEFINALUSERCRITICALERRORRESULTID = "AccurateFinalUserCriticalErrorResultID";
			public const string ACCURATEBUSINESSCRITICALERRORRESULTID = "AccurateBusinessCriticalErrorResultID";
			public const string ACCURATEFULFILLMENTCRITICALERRORRESULTID = "AccurateFulfillmentCriticalErrorResultID";
			public const string ACCURATENONCRITICALERRORRESULT = "AccurateNonCriticalErrorResult";
			public const string CONTROLLABLERESULTID = "ControllableResultID";
			public const string CONTROLLABLEFINALUSERCRITICALERRORRESULTID = "ControllableFinalUserCriticalErrorResultID";
			public const string CONTROLLABLEBUSINESSCRITICALERRORRESULTID = "ControllableBusinessCriticalErrorResultID";
			public const string CONTROLLABLEFULFILLMENTCRITICALERRORRESULTID = "ControllableFulfillmentCriticalErrorResultID";
			public const string CONTROLLABLENONCRITICALERRORRESULT = "ControllableNonCriticalErrorResult";
			public const string TIMEELAPSED = "TimeElapsed";
			public const string BASICINFOID = "BasicInfoID";
			public const string TYPE_ID = "TypeID";
			public const string CALIBRATED_TRANSACTION_ID = "CalibratedTransactionID";
		}

		public struct StoredProcedures
		{
			public struct DeleteByID
			{
				public const string NAME = "[dbo].[usp_TransactionDeleteByID]";

				public struct Parameters
				{
					public const string ID = "@id";
				}
			}

			public struct Insert
			{
				public const string NAME = "[dbo].[usp_TransactionInsert]";

				public struct Parameters
				{
					public const string IDENTIFIER = "@identifier";
					public const string USERTOEVALUATEID = "@userToEvaluateID";
					public const string EVALUATORUSERID = "@evaluatorUserID";
					public const string EVALUATIONDATE = "@evaluationDate";
					public const string TRANSACTIONDATE = "@transactionDate";
					public const string FORMID = "@formID";
					public const string COMMENT = "@comment";
					public const string GENERALRESULTID = "@generalResultID";
					public const string GENERALFINALUSERCRITICALERRORRESULTID = "@generalFinalUserCriticalErrorResultID";
					public const string GENERALBUSINESSCRITICALERRORRESULTID = "@generalBusinessCriticalErrorResultID";
					public const string GENERALFULFILLMENTCRITICALERRORRESULTID = "@generalFulfillmentCriticalErrorResultID";
					public const string GENERALNONCRITICALERRORRESULT = "@generalNonCriticalErrorResult";
					public const string ACCURATERESULTID = "@accurateResultID";
					public const string ACCURATEFINALUSERCRITICALERRORRESULTID = "@accurateFinalUserCriticalErrorResultID";
					public const string ACCURATEBUSINESSCRITICALERRORRESULTID = "@accurateBusinessCriticalErrorResultID";
					public const string ACCURATEFULFILLMENTCRITICALERRORRESULTID = "@accurateFulfillmentCriticalErrorResultID";
					public const string ACCURATENONCRITICALERRORRESULT = "@accurateNonCriticalErrorResult";
					public const string CONTROLLABLERESULTID = "@controllableResultID";
					public const string CONTROLLABLEFINALUSERCRITICALERRORRESULTID = "@controllableFinalUserCriticalErrorResultID";
					public const string CONTROLLABLEBUSINESSCRITICALERRORRESULTID = "@controllableBusinessCriticalErrorResultID";
					public const string CONTROLLABLEFULFILLMENTCRITICALERRORRESULTID = "@controllableFulfillmentCriticalErrorResultID";
					public const string CONTROLLABLENONCRITICALERRORRESULT = "@controllableNonCriticalErrorResult";
					public const string TIMEELAPSED = "@timeElapsed";
					public const string BASICINFOID = "@basicInfoID";
					public const string TYPE_ID = "@typeID";
					public const string CALIBRATED_TRANSACTION_ID = "@calibratedTransactionID";
				}

				public struct ResultFields
				{
					public const string ID = "ID";
				}
			}

			public struct SelectByID
			{
				public const string NAME = "[dbo].[usp_TransactionSelectByID]";

				public struct Parameters
				{
					public const string ID = "@id";
				}

				public struct ResultFields
				{
					public const string ID = "ID";
					public const string IDENTIFIER = "Identifier";
					public const string USERTOEVALUATEID = "UserToEvaluateID";
					public const string EVALUATORUSERID = "EvaluatorUserID";
					public const string EVALUATIONDATE = "EvaluationDate";
					public const string TRANSACTIONDATE = "TransactionDate";
					public const string FORMID = "FormID";
					public const string COMMENT = "Comment";
					public const string GENERALRESULTID = "GeneralResultID";
					public const string GENERALFINALUSERCRITICALERRORRESULTID = "GeneralFinalUserCriticalErrorResultID";
					public const string GENERALBUSINESSCRITICALERRORRESULTID = "GeneralBusinessCriticalErrorResultID";
					public const string GENERALFULFILLMENTCRITICALERRORRESULTID = "GeneralFulfillmentCriticalErrorResultID";
					public const string GENERALNONCRITICALERRORRESULT = "GeneralNonCriticalErrorResult";
					public const string ACCURATERESULTID = "AccurateResultID";
					public const string ACCURATEFINALUSERCRITICALERRORRESULTID = "AccurateFinalUserCriticalErrorResultID";
					public const string ACCURATEBUSINESSCRITICALERRORRESULTID = "AccurateBusinessCriticalErrorResultID";
					public const string ACCURATEFULFILLMENTCRITICALERRORRESULTID = "AccurateFulfillmentCriticalErrorResultID";
					public const string ACCURATENONCRITICALERRORRESULT = "AccurateNonCriticalErrorResult";
					public const string CONTROLLABLERESULTID = "ControllableResultID";
					public const string CONTROLLABLEFINALUSERCRITICALERRORRESULTID = "ControllableFinalUserCriticalErrorResultID";
					public const string CONTROLLABLEBUSINESSCRITICALERRORRESULTID = "ControllableBusinessCriticalErrorResultID";
					public const string CONTROLLABLEFULFILLMENTCRITICALERRORRESULTID = "ControllableFulfillmentCriticalErrorResultID";
					public const string CONTROLLABLENONCRITICALERRORRESULT = "ControllableNonCriticalErrorResult";
					public const string TIMEELAPSED = "TimeElapsed";
					public const string BASICINFOID = "BasicInfoID";
					public const string TYPE_ID = "TypeID";
					public const string CALIBRATED_TRANSACTION_ID = "CalibratedTransactionID";
				}
			}

			public struct SelectByIdentifier
			{
				public const string NAME = "[dbo].[usp_TransactionSelectByIdentifier]";

				public struct Parameters
				{
					public const string IDENTIFIER = "@identifier";
				}

				public struct ResultFields
				{
					public const string ID = "ID";
					public const string IDENTIFIER = "Identifier";
					public const string USERTOEVALUATEID = "UserToEvaluateID";
					public const string EVALUATORUSERID = "EvaluatorUserID";
					public const string EVALUATIONDATE = "EvaluationDate";
					public const string TRANSACTIONDATE = "TransactionDate";
					public const string FORMID = "FormID";
					public const string COMMENT = "Comment";
					public const string GENERALRESULTID = "GeneralResultID";
					public const string GENERALFINALUSERCRITICALERRORRESULTID = "GeneralFinalUserCriticalErrorResultID";
					public const string GENERALBUSINESSCRITICALERRORRESULTID = "GeneralBusinessCriticalErrorResultID";
					public const string GENERALFULFILLMENTCRITICALERRORRESULTID = "GeneralFulfillmentCriticalErrorResultID";
					public const string GENERALNONCRITICALERRORRESULT = "GeneralNonCriticalErrorResult";
					public const string ACCURATERESULTID = "AccurateResultID";
					public const string ACCURATEFINALUSERCRITICALERRORRESULTID = "AccurateFinalUserCriticalErrorResultID";
					public const string ACCURATEBUSINESSCRITICALERRORRESULTID = "AccurateBusinessCriticalErrorResultID";
					public const string ACCURATEFULFILLMENTCRITICALERRORRESULTID = "AccurateFulfillmentCriticalErrorResultID";
					public const string ACCURATENONCRITICALERRORRESULT = "AccurateNonCriticalErrorResult";
					public const string CONTROLLABLERESULTID = "ControllableResultID";
					public const string CONTROLLABLEFINALUSERCRITICALERRORRESULTID = "ControllableFinalUserCriticalErrorResultID";
					public const string CONTROLLABLEBUSINESSCRITICALERRORRESULTID = "ControllableBusinessCriticalErrorResultID";
					public const string CONTROLLABLEFULFILLMENTCRITICALERRORRESULTID = "ControllableFulfillmentCriticalErrorResultID";
					public const string CONTROLLABLENONCRITICALERRORRESULT = "ControllableNonCriticalErrorResult";
					public const string TIMEELAPSED = "TimeElapsed";
					public const string BASICINFOID = "BasicInfoID";
					public const string TYPE_ID = "TypeID";
					public const string CALIBRATED_TRANSACTION_ID = "CalibratedTransactionID";
				}
			}

			public struct SelectByCalibratedTransactionID
			{
				public const string NAME = "[dbo].[usp_TransactionSelectByCalibratedTransactionID]";

				public struct Parameters
				{
					public const string CALIBRATED_TRANSACTION_ID = "@calibratedTransactionID";
				}

				public struct ResultFields
				{
					public const string ID = "ID";
					public const string IDENTIFIER = "Identifier";
					public const string USERTOEVALUATEID = "UserToEvaluateID";
					public const string EVALUATORUSERID = "EvaluatorUserID";
					public const string EVALUATIONDATE = "EvaluationDate";
					public const string TRANSACTIONDATE = "TransactionDate";
					public const string FORMID = "FormID";
					public const string COMMENT = "Comment";
					public const string GENERALRESULTID = "GeneralResultID";
					public const string GENERALFINALUSERCRITICALERRORRESULTID = "GeneralFinalUserCriticalErrorResultID";
					public const string GENERALBUSINESSCRITICALERRORRESULTID = "GeneralBusinessCriticalErrorResultID";
					public const string GENERALFULFILLMENTCRITICALERRORRESULTID = "GeneralFulfillmentCriticalErrorResultID";
					public const string GENERALNONCRITICALERRORRESULT = "GeneralNonCriticalErrorResult";
					public const string ACCURATERESULTID = "AccurateResultID";
					public const string ACCURATEFINALUSERCRITICALERRORRESULTID = "AccurateFinalUserCriticalErrorResultID";
					public const string ACCURATEBUSINESSCRITICALERRORRESULTID = "AccurateBusinessCriticalErrorResultID";
					public const string ACCURATEFULFILLMENTCRITICALERRORRESULTID = "AccurateFulfillmentCriticalErrorResultID";
					public const string ACCURATENONCRITICALERRORRESULT = "AccurateNonCriticalErrorResult";
					public const string CONTROLLABLERESULTID = "ControllableResultID";
					public const string CONTROLLABLEFINALUSERCRITICALERRORRESULTID = "ControllableFinalUserCriticalErrorResultID";
					public const string CONTROLLABLEBUSINESSCRITICALERRORRESULTID = "ControllableBusinessCriticalErrorResultID";
					public const string CONTROLLABLEFULFILLMENTCRITICALERRORRESULTID = "ControllableFulfillmentCriticalErrorResultID";
					public const string CONTROLLABLENONCRITICALERRORRESULT = "ControllableNonCriticalErrorResult";
					public const string TIMEELAPSED = "TimeElapsed";
					public const string BASICINFOID = "BasicInfoID";
					public const string TYPE_ID = "TypeID";
					public const string CALIBRATED_TRANSACTION_ID = "CalibratedTransactionID";
				}
			}

			public struct Update
			{
				public const string NAME = "[dbo].[usp_TransactionUpdate]";

				public struct Parameters
				{
					public const string ID = "@id";
					public const string IDENTIFIER = "@identifier";
					public const string USERTOEVALUATEID = "@userToEvaluateID";
					public const string EVALUATORUSERID = "@evaluatorUserID";
					public const string EVALUATIONDATE = "@evaluationDate";
					public const string TRANSACTIONDATE = "@transactionDate";
					public const string FORMID = "@formID";
					public const string COMMENT = "@comment";
					public const string GENERALRESULTID = "@generalResultID";
					public const string GENERALFINALUSERCRITICALERRORRESULTID = "@generalFinalUserCriticalErrorResultID";
					public const string GENERALBUSINESSCRITICALERRORRESULTID = "@generalBusinessCriticalErrorResultID";
					public const string GENERALFULFILLMENTCRITICALERRORRESULTID = "@generalFulfillmentCriticalErrorResultID";
					public const string GENERALNONCRITICALERRORRESULT = "@generalNonCriticalErrorResult";
					public const string ACCURATERESULTID = "@accurateResultID";
					public const string ACCURATEFINALUSERCRITICALERRORRESULTID = "@accurateFinalUserCriticalErrorResultID";
					public const string ACCURATEBUSINESSCRITICALERRORRESULTID = "@accurateBusinessCriticalErrorResultID";
					public const string ACCURATEFULFILLMENTCRITICALERRORRESULTID = "@accurateFulfillmentCriticalErrorResultID";
					public const string ACCURATENONCRITICALERRORRESULT = "@accurateNonCriticalErrorResult";
					public const string CONTROLLABLERESULTID = "@controllableResultID";
					public const string CONTROLLABLEFINALUSERCRITICALERRORRESULTID = "@controllableFinalUserCriticalErrorResultID";
					public const string CONTROLLABLEBUSINESSCRITICALERRORRESULTID = "@controllableBusinessCriticalErrorResultID";
					public const string CONTROLLABLEFULFILLMENTCRITICALERRORRESULTID = "@controllableFulfillmentCriticalErrorResultID";
					public const string CONTROLLABLENONCRITICALERRORRESULT = "@controllableNonCriticalErrorResult";
					public const string TIMEELAPSED = "@timeElapsed";
				}
			}

			public struct CheckIdentifierExistence
			{
				public const string NAME = "[dbo].[usp_TransactionCheckIdentifierExistence]";

				public struct Parameters
				{
					public const string IDENTIFIER = "@identifier";
				}

				public struct ResultFields
				{
					public const string ID = "ID";
				}
			}

			public struct Search
			{
				public const string NAME = "[dbo].[usp_TransactionSearch]";

				public struct Parameters
				{
					public const string USER_IDENTIFICATION_TYPE = "@userIdentificationType";
					public const string USER_IDENTIFICATION = "@userIdentification";
					public const string USER_STATUS_ID = "@userStatusID";
					public const string WORKSPACE_ID_LIST = "@workspaceIDList";
					public const string MONITOR_USER_IDENTIFICATION_TYPE = "@monitorUserIdentificationTypeID";
					public const string MONITOR_USER_IDENTIFICATION = "@monitorUserIdentification";
					public const string PROGRAM_ID_LIST = "@programIDList";
					public const string TRANSACTION_IDENTIFIER = "@transactionIdentifier";
					public const string TRANSACTION_DATE_FROM = "@transactionDateFrom";
					public const string TRANSACTION_DATE_TO = "@transactionDateTo";
					public const string TRANSACTION_DATE_SINCE_TYPE_ID = "@transactionDateSinceTypeID";
					public const string TRANSACTION_DATE_SINCE = "@transactionDateSince";
					public const string EVALUATION_DATE_FROM = "@evaluationDateFrom";
					public const string EVALUATION_DATE_TO = "@evaluationDateTo";
					public const string EVALUATION_DATE_SINCE_TYPE_ID = "@evaluationDateSinceTypeID";
					public const string EVALUATION_DATE_SINCE = "@evaluationDateSince";
					public const string TRANSACTION_COMMENT_TEXT = "@transactionCommentText";
					public const string ATTRIBUTE_NAME_TEXT = "@attributeNameText";
					public const string ATTRIBUTE_COMMENT_TEXT = "@attributeCommentText";
					public const string TRANSACTION_LABEL_TEXT = "@transactionLabelText";
					public const string BI_FIELD_ID_LIST = "@biFieldIDList";
					public const string DISPUTATION_TEXT = "@disputationText";
					public const string INVALIDATION_TEXT = "@invalidationText";
					public const string COACHING_SENT = "@coachingSent";
					public const string COACHING_READ = "@coachingRead";
					public const string CONTROLLABLE_ERROR_FILTER_TYPE_ID = "@controllableErrorFilterTypeID";
					public const string DEVOLUTION_COMMENT_TEXT = "@devolutionCommentText";
					public const string DEVOLUTION_STRENGTHS_COMMENT_TEXT = "@devolutionStrengthCommentText";
					public const string DEVOLUTION_IMPROVEMENT_STEPS_COMMENT_TEXT = "@devolutionImprovementStepsCommentText";
					public const string CUSTOM_FIELD_LABEL_TEXT = "@customFieldLabelText";
					public const string CUSTOM_FIELD_VALUE_TEXT = "@customFieldValueText";
				}

				public struct ResultFields
				{
					public const string TRANSACTIONID = "TransactionID";
					public const string USERTOEVALUATEUSERNAME = "UserToEvaluateUsername";
					public const string PROGRAMNAME = "ProgramName";
					public const string TRANSACTIONEVALUATIONDATE = "TransactionEvaluationDate";
					public const string TRANSACTIONTRANSACTIONDATE = "TransactionTransactionDate";
					public const string TRANSACTIONCOMMENT = "TransactionComment";
					public const string GENERALRESULTDESCRIPTION = "GeneralResultDescription";
					public const string GENERALRESULTFUCEDESCRIPTION = "GeneralResultFUCEDescription";
					public const string GENERALRESULTBCEDESCRIPTION = "GeneralResultBCEDescription";
					public const string GENERALRESULTFCEDESCRIPTION = "GeneralResultFCEDescription";
					public const string GENERALRESULTNCE = "GeneralResultNCE";
					public const string TRANSACTIONTIMEELAPSED = "TransactionTimeElapsed";
				}
			}
		}
	}
}