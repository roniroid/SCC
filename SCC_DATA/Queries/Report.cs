using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCC_DATA.Queries
{
    public class Report
	{
		public struct StoredProcedures
		{
			public struct OverallAccuracy
			{
				public const string NAME = "[dbo].[usp_ReportOverallAccuracy]";

				public struct Parameters
				{
					public const string TRANSACTION_START_DATE = "@transactionStartDate";
					public const string TRANSACTION_END_DATE = "@transactionEndDate";
					public const string EVALUATION_START_DATE = "@evaluationStartDate";
					public const string EVALUATION_END_DATE = "@evaluationEndDate";
					public const string PROGRAM_ID_LIST = "@programIDList";
					public const string USER_ID_LIST = "@userIDArray";
					public const string SUPERVISOR_USER_ID_LIST = "@supervisorUserIDArray";
					public const string EVALUATOR_USER_ID_LIST = "@evaluatorUserIDArray";
					public const string ERROR_TYPE_ID_LIST = "@errorTypeIDArray";
					public const string ATTRIBUTE_CONTROLLABLE = "@attributeControllable";
					public const string ATTRIBUTE_KNOWN = "@attributeKnown";
					public const string TRANSACTION_CUSTOM_FIELD_CATALOG_LIST = "@transactionCustomFieldCatalogList";
				}

				public struct ResultFields
				{
					public const string TRANSACTION_ID = "TransactionID";

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
                }
			}
			
			public struct ComparativeByUser
			{
				public const string NAME = "[dbo].[usp_ReportComparativeByUser]";

				public struct Parameters
				{
					public const string TRANSACTION_START_DATE = "@transactionStartDate";
					public const string TRANSACTION_END_DATE = "@transactionEndDate";
					public const string EVALUATION_START_DATE = "@evaluationStartDate";
					public const string EVALUATION_END_DATE = "@evaluationEndDate";
					public const string PROGRAM_ID_LIST = "@programIDList";
					public const string USER_ID_LIST = "@userIDArray";
					public const string SUPERVISOR_USER_ID_LIST = "@supervisorUserIDArray";
					public const string EVALUATOR_USER_ID_LIST = "@evaluatorUserIDArray";
					public const string ERROR_TYPE_ID_LIST = "@errorTypeIDArray";
					public const string ATTRIBUTE_CONTROLLABLE = "@attributeControllable";
					public const string ATTRIBUTE_KNOWN = "@attributeKnown";
                    public const string TRANSACTION_CUSTOM_FIELD_CATALOG_LIST = "@transactionCustomFieldCatalogList";
                }

				public struct ResultFields
				{
					public const string TRANSACTION_ID = "TransactionID";

                    public const string GENERALRESULTID = "GeneralResultID";
                    public const string GENERALFINALUSERCRITICALERRORRESULTID = "GeneralFinalUserCriticalErrorResultID";
                    public const string GENERALBUSINESSCRITICALERRORRESULTID = "GeneralBusinessCriticalErrorResultID";
                    public const string GENERALFULFILLMENTCRITICALERRORRESULTID = "GeneralFulfillmentCriticalErrorResultID";

                    public const string ACCURATERESULTID = "AccurateResultID";
                    public const string ACCURATEFINALUSERCRITICALERRORRESULTID = "AccurateFinalUserCriticalErrorResultID";
                    public const string ACCURATEBUSINESSCRITICALERRORRESULTID = "AccurateBusinessCriticalErrorResultID";
                    public const string ACCURATEFULFILLMENTCRITICALERRORRESULTID = "AccurateFulfillmentCriticalErrorResultID";

                    public const string CONTROLLABLERESULTID = "ControllableResultID";
                    public const string CONTROLLABLEFINALUSERCRITICALERRORRESULTID = "ControllableFinalUserCriticalErrorResultID";
                    public const string CONTROLLABLEBUSINESSCRITICALERRORRESULTID = "ControllableBusinessCriticalErrorResultID";
                    public const string CONTROLLABLEFULFILLMENTCRITICALERRORRESULTID = "ControllableFulfillmentCriticalErrorResultID";

                    public const string USER_ID = "UserID";
				}
			}
			
			public struct ComparativeByProgram
			{
				public const string NAME = "[dbo].[usp_ReportComparativeByProgram]";

				public struct Parameters
				{
					public const string TRANSACTION_START_DATE = "@transactionStartDate";
					public const string TRANSACTION_END_DATE = "@transactionEndDate";
					public const string EVALUATION_START_DATE = "@evaluationStartDate";
					public const string EVALUATION_END_DATE = "@evaluationEndDate";
					public const string PROGRAM_ID_LIST = "@programIDList";
					public const string USER_ID_LIST = "@userIDArray";
					public const string SUPERVISOR_USER_ID_LIST = "@supervisorUserIDArray";
					public const string EVALUATOR_USER_ID_LIST = "@evaluatorUserIDArray";
					public const string ERROR_TYPE_ID_LIST = "@errorTypeIDArray";
					public const string ATTRIBUTE_CONTROLLABLE = "@attributeControllable";
					public const string ATTRIBUTE_KNOWN = "@attributeKnown";
                    public const string TRANSACTION_CUSTOM_FIELD_CATALOG_LIST = "@transactionCustomFieldCatalogList";
                }

				public struct ResultFields
				{
					public const string TRANSACTION_ID = "TransactionID";

                    public const string GENERALRESULTID = "GeneralResultID";
                    public const string GENERALFINALUSERCRITICALERRORRESULTID = "GeneralFinalUserCriticalErrorResultID";
					public const string GENERALBUSINESSCRITICALERRORRESULTID = "GeneralBusinessCriticalErrorResultID";
					public const string GENERALFULFILLMENTCRITICALERRORRESULTID = "GeneralFulfillmentCriticalErrorResultID";

                    public const string ACCURATERESULTID = "AccurateResultID";
                    public const string ACCURATEFINALUSERCRITICALERRORRESULTID = "AccurateFinalUserCriticalErrorResultID";
                    public const string ACCURATEBUSINESSCRITICALERRORRESULTID = "AccurateBusinessCriticalErrorResultID";
                    public const string ACCURATEFULFILLMENTCRITICALERRORRESULTID = "AccurateFulfillmentCriticalErrorResultID";

                    public const string CONTROLLABLERESULTID = "ControllableResultID";
                    public const string CONTROLLABLEFINALUSERCRITICALERRORRESULTID = "ControllableFinalUserCriticalErrorResultID";
                    public const string CONTROLLABLEBUSINESSCRITICALERRORRESULTID = "ControllableBusinessCriticalErrorResultID";
                    public const string CONTROLLABLEFULFILLMENTCRITICALERRORRESULTID = "ControllableFulfillmentCriticalErrorResultID";

                    public const string PROGRAM_ID = "ProgramID";
				}
			}
			
			public struct CalibratorComparison
			{
				public const string NAME = "[dbo].[usp_ReportCalibratorComparison]";

				public struct Parameters
				{
					public const string CALIBRATION_START_DATE = "@calibrationStartDate";
					public const string CALIBRATION_END_DATE = "@calibrationEndDate";
					public const string PROGRAM_ID_LIST = "@programIDList";
					public const string CALIBRATED_USER_ID_LIST = "@calibratedUserIDArray";
					public const string CALIBRATED_SUPERVISOR_USER_ID_LIST = "@calibratedSupervisorUserIDArray";
					public const string CALIBRATOR_USER_ID_LIST = "@calibratorUserIDArray";
					public const string CALIBRATION_TYPE_ID_LIST = "@calibrationTypeIDArray";
					public const string ERROR_TYPE_ID_LIST = "@errorTypeIDArray";
				}

				public struct ResultFields
				{
					public const string TRANSACTION_ID = "TransactionID";

                    public const string GLOBALGENERALRESULTID = "GeneralResultID";
                    public const string GLOBALGENERALFINALUSERCRITICALERRORRESULTID = "GeneralFinalUserCriticalErrorResultID";
					public const string GLOBALGENERALBUSINESSCRITICALERRORRESULTID = "GeneralBusinessCriticalErrorResultID";
					public const string GLOBALGENERALFULFILLMENTCRITICALERRORRESULTID = "GeneralFulfillmentCriticalErrorResultID";

                    public const string GLOBALACCURATERESULTID = "AccurateResultID";
                    public const string GLOBALACCURATEFINALUSERCRITICALERRORRESULTID = "AccurateFinalUserCriticalErrorResultID";
					public const string GLOBALACCURATEBUSINESSCRITICALERRORRESULTID = "AccurateBusinessCriticalErrorResultID";
					public const string GLOBALACCURATEFULFILLMENTCRITICALERRORRESULTID = "AccurateFulfillmentCriticalErrorResultID";

                    public const string GLOBALCONTROLLABLERESULTID = "ControllableResultID";
                    public const string GLOBALCONTROLLABLEFINALUSERCRITICALERRORRESULTID = "ControllableFinalUserCriticalErrorResultID";
					public const string GLOBALCONTROLLABLEBUSINESSCRITICALERRORRESULTID = "ControllableBusinessCriticalErrorResultID";
					public const string GLOBALCONTROLLABLEFULFILLMENTCRITICALERRORRESULTID = "ControllableFulfillmentCriticalErrorResultID";

					public const string CALIBRATOR_USER_ID = "CalibratorUserID";
				}
			}
			
			public struct AccuracyTrend
			{
				public const string NAME = "[dbo].[usp_ReportAccuracyTrend]";

				public struct Parameters
				{
					public const string TRANSACTION_START_DATE = "@transactionStartDate";
					public const string TRANSACTION_END_DATE = "@transactionEndDate";
					public const string EVALUATION_START_DATE = "@evaluationStartDate";
					public const string EVALUATION_END_DATE = "@evaluationEndDate";
					public const string PROGRAM_ID_LIST = "@programIDList";
					public const string USER_ID_LIST = "@userIDArray";
					public const string SUPERVISOR_USER_ID_LIST = "@supervisorUserIDArray";
					public const string EVALUATOR_USER_ID_LIST = "@evaluatorUserIDArray";
					public const string ERROR_TYPE_ID_LIST = "@errorTypeIDArray";
					public const string ATTRIBUTE_CONTROLLABLE = "@attributeControllable";
					public const string ATTRIBUTE_KNOWN = "@attributeKnown";
                    public const string TRANSACTION_CUSTOM_FIELD_CATALOG_LIST = "@transactionCustomFieldCatalogList";
                }

				public struct ResultFields
				{
					public const string TRANSACTION_ID = "TransactionID";
					public const string TRANSACTION_DATE = "TransactionDate";

                    public const string GENERALRESULTID = "GeneralResultID";
                    public const string GENERALFINALUSERCRITICALERRORRESULTID = "GeneralFinalUserCriticalErrorResultID";
					public const string GENERALBUSINESSCRITICALERRORRESULTID = "GeneralBusinessCriticalErrorResultID";
					public const string GENERALFULFILLMENTCRITICALERRORRESULTID = "GeneralFulfillmentCriticalErrorResultID";

                    public const string ACCURATERESULTID = "AccurateResultID";
                    public const string ACCURATEFINALUSERCRITICALERRORRESULTID = "AccurateFinalUserCriticalErrorResultID";
					public const string ACCURATEBUSINESSCRITICALERRORRESULTID = "AccurateBusinessCriticalErrorResultID";
					public const string ACCURATEFULFILLMENTCRITICALERRORRESULTID = "AccurateFulfillmentCriticalErrorResultID";

                    public const string CONTROLLABLERESULTID = "ControllableResultID";
                    public const string CONTROLLABLEFINALUSERCRITICALERRORRESULTID = "ControllableFinalUserCriticalErrorResultID";
					public const string CONTROLLABLEBUSINESSCRITICALERRORRESULTID = "ControllableBusinessCriticalErrorResultID";
					public const string CONTROLLABLEFULFILLMENTCRITICALERRORRESULTID = "ControllableFulfillmentCriticalErrorResultID";
				}
			}
			
			public struct AccuracyTrendByAttribute
			{
				public const string NAME = "[dbo].[usp_ReportAccuracyTrendByAttribute]";

				public struct Parameters
				{
					public const string TRANSACTION_ID_LIST = "@transactionIDList";
					public const string ERROR_TYPE_ID_LIST = "@errorTypeIDList";
					public const string ATTRIBUTE_ID_LIST = "@attributeIDList";
				}

				public struct ResultFields
				{
					public const string TRANSACTION_ID = "TransactionID";
					public const string TRANSACTION_DATE = "TransactionDate";
					public const string TRANSACTION_ATTRIBUTE_ID = "TransactionAttributeID";
					public const string ATTRIBUTE_ID = "AttributeID";
					public const string ATTRIBUTE_NAME = "AttributeName";
					public const string SUCCESSFUL_RESULT = "SuccessfulResult";
				}
			}

			public struct AccuracyByAttribute
			{
				public const string NAME = "[dbo].[usp_ReportAccuracyByAttribute]";

				public struct Parameters
				{
					public const string TRANSACTION_ID_LIST = "@transactionIDList";
					public const string ERROR_TYPE_ID_LIST = "@errorTypeIDList";
					public const string ATTRIBUTE_ID_LIST = "@attributeIDList";
				}

				public struct ResultFields
				{
					public const string TRANSACTION_ATTRIBUTE_ID = "TransactionAttributeID";
					public const string ATTRIBUTE_ID = "AttributeID";
					public const string ATTRIBUTE_NAME = "AttributeName";
					public const string SUCCESSFUL_RESULT = "SuccessfulResult";
				}
			}

			public struct AccuracyBySubattribute
			{
				public const string NAME = "[dbo].[usp_ReportAccuracyBySubattribute]";

				public struct Parameters
				{
					public const string TRANSACTION_ATTRIBUTE_ID_LIST = "@transactionAttributeIDList";
				}

				public struct ResultFields
				{
					public const string TRANSACTION_ATTRIBUTE_ID = "TransactionAttributeID";
					public const string ATTRIBUTE_ID = "AttributeID";
					public const string ATTRIBUTE_NAME = "AttributeName";
					public const string SUCCESSFUL_RESULT = "SuccessfulResult";
					public const string HAS_CHILDREN = "HasChildren";
					public const string ERROR_TYPE_ID = "ErrorTypeID";
				}
			}

			public struct ParetoBI
			{
				public const string NAME = "[dbo].[usp_ReportParetoBI]";

				public struct Parameters
				{
					public const string TRANSACTION_ID_LIST = "@transactionIDList";
					public const string BI_FIELD_ID_LIST = "@biFieldIDList";
				}

				public struct ResultFields
				{
					public const string TRANSACTION_BI_FIELD_CATALOG_ID = "TransactionBIFieldCatalogID";
					public const string BUSINESS_INTELLIGENCE_FIELD_ID = "BusinessIntelligenceFieldID";
					public const string BUSINESS_INTELLIGENCE_FIELD_NAME = "BusinessIntelligenceFieldName";
					public const string SUCCESSFUL_RESULT = "SuccessfulResult";
					public const string HAS_CHILDREN = "HasChildren";
				}
			}
		}
	}
}
