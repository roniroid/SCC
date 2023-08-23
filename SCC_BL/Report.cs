using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCC_BL
{
    public class Report : IDisposable
	{
		public List<SCC_BL.Reports.Results.OverallAccuracy> OverallAccuracy(DateTime? transactionStartDate, DateTime? transactionEndDate, DateTime? evaluationStartDate, DateTime? evaluationEndDate, string programIDList, string userIDArray, string supervisorUserIDArray, string evaluatorUserIDArray, string errorTypeIDArray, bool? attributeControllable, bool? attributeKnown, string transactionCustomFieldCatalogList)
		{
			List<SCC_BL.Reports.Results.OverallAccuracy> overallAccuracyResultList = new List<SCC_BL.Reports.Results.OverallAccuracy>();

			using (SCC_DATA.Repositories.Report report = new SCC_DATA.Repositories.Report())
			{
				DataTable dt = report.OverallAccuracy(transactionStartDate, transactionEndDate, evaluationStartDate, evaluationEndDate, programIDList, userIDArray, supervisorUserIDArray, evaluatorUserIDArray, errorTypeIDArray, attributeControllable, attributeKnown, transactionCustomFieldCatalogList);

				foreach (DataRow dr in dt.Rows)
				{
					SCC_BL.Reports.Results.OverallAccuracy overallAccuracyResult = new SCC_BL.Reports.Results.OverallAccuracy(
						Convert.ToInt32(dr[SCC_DATA.Queries.Report.StoredProcedures.OverallAccuracy.ResultFields.TRANSACTION_ID]),

                        Convert.ToInt32(dr[SCC_DATA.Queries.Report.StoredProcedures.OverallAccuracy.ResultFields.GENERALRESULTID]),
                        Convert.ToInt32(dr[SCC_DATA.Queries.Report.StoredProcedures.OverallAccuracy.ResultFields.GENERALFINALUSERCRITICALERRORRESULTID]),
                        Convert.ToInt32(dr[SCC_DATA.Queries.Report.StoredProcedures.OverallAccuracy.ResultFields.GENERALBUSINESSCRITICALERRORRESULTID]),
                        Convert.ToInt32(dr[SCC_DATA.Queries.Report.StoredProcedures.OverallAccuracy.ResultFields.GENERALFULFILLMENTCRITICALERRORRESULTID]),
                        Convert.ToDouble(dr[SCC_DATA.Queries.Report.StoredProcedures.OverallAccuracy.ResultFields.GENERALNONCRITICALERRORRESULT]),

                        Convert.ToInt32(dr[SCC_DATA.Queries.Report.StoredProcedures.OverallAccuracy.ResultFields.ACCURATERESULTID]),
                        Convert.ToInt32(dr[SCC_DATA.Queries.Report.StoredProcedures.OverallAccuracy.ResultFields.ACCURATEFINALUSERCRITICALERRORRESULTID]),
                        Convert.ToInt32(dr[SCC_DATA.Queries.Report.StoredProcedures.OverallAccuracy.ResultFields.ACCURATEBUSINESSCRITICALERRORRESULTID]),
                        Convert.ToInt32(dr[SCC_DATA.Queries.Report.StoredProcedures.OverallAccuracy.ResultFields.ACCURATEFULFILLMENTCRITICALERRORRESULTID]),
                        Convert.ToDouble(dr[SCC_DATA.Queries.Report.StoredProcedures.OverallAccuracy.ResultFields.ACCURATENONCRITICALERRORRESULT]),

                        Convert.ToInt32(dr[SCC_DATA.Queries.Report.StoredProcedures.OverallAccuracy.ResultFields.CONTROLLABLERESULTID]),
                        Convert.ToInt32(dr[SCC_DATA.Queries.Report.StoredProcedures.OverallAccuracy.ResultFields.CONTROLLABLEFINALUSERCRITICALERRORRESULTID]),
                        Convert.ToInt32(dr[SCC_DATA.Queries.Report.StoredProcedures.OverallAccuracy.ResultFields.CONTROLLABLEBUSINESSCRITICALERRORRESULTID]),
                        Convert.ToInt32(dr[SCC_DATA.Queries.Report.StoredProcedures.OverallAccuracy.ResultFields.CONTROLLABLEFULFILLMENTCRITICALERRORRESULTID]),
                        Convert.ToDouble(dr[SCC_DATA.Queries.Report.StoredProcedures.OverallAccuracy.ResultFields.CONTROLLABLENONCRITICALERRORRESULT]));

					overallAccuracyResultList.Add(overallAccuracyResult);
				}
			}

			return overallAccuracyResultList;
		}
		
		public List<SCC_BL.Reports.Results.ComparativeByUser> ComparativeByUser(DateTime? transactionStartDate, DateTime? transactionEndDate, DateTime? evaluationStartDate, DateTime? evaluationEndDate, string programIDList, string userIDArray, string supervisorUserIDArray, string evaluatorUserIDArray, string errorTypeIDArray, bool? attributeControllable, bool? attributeKnown, string transactionCustomFieldCatalogList)
		{
			List<SCC_BL.Reports.Results.ComparativeByUser> comparativeByUserResultList = new List<SCC_BL.Reports.Results.ComparativeByUser>();

			using (SCC_DATA.Repositories.Report report = new SCC_DATA.Repositories.Report())
			{
				DataTable dt = report.ComparativeByUser(transactionStartDate, transactionEndDate, evaluationStartDate, evaluationEndDate, programIDList, userIDArray, supervisorUserIDArray, evaluatorUserIDArray, errorTypeIDArray, attributeControllable, attributeKnown, transactionCustomFieldCatalogList);

				foreach (DataRow dr in dt.Rows)
				{
					SCC_BL.Reports.Results.ComparativeByUser comparativeByUserResult = new SCC_BL.Reports.Results.ComparativeByUser(
						Convert.ToInt32(dr[SCC_DATA.Queries.Report.StoredProcedures.ComparativeByUser.ResultFields.TRANSACTION_ID]),

                        Convert.ToInt32(dr[SCC_DATA.Queries.Report.StoredProcedures.ComparativeByUser.ResultFields.GENERALRESULTID]),
                        Convert.ToInt32(dr[SCC_DATA.Queries.Report.StoredProcedures.ComparativeByUser.ResultFields.GENERALFINALUSERCRITICALERRORRESULTID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.Report.StoredProcedures.ComparativeByUser.ResultFields.GENERALBUSINESSCRITICALERRORRESULTID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.Report.StoredProcedures.ComparativeByUser.ResultFields.GENERALFULFILLMENTCRITICALERRORRESULTID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.Report.StoredProcedures.ComparativeByUser.ResultFields.GENERALNONCRITICALERRORRESULT]),

                        Convert.ToInt32(dr[SCC_DATA.Queries.Report.StoredProcedures.ComparativeByUser.ResultFields.ACCURATERESULTID]),
                        Convert.ToInt32(dr[SCC_DATA.Queries.Report.StoredProcedures.ComparativeByUser.ResultFields.ACCURATEFINALUSERCRITICALERRORRESULTID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.Report.StoredProcedures.ComparativeByUser.ResultFields.ACCURATEBUSINESSCRITICALERRORRESULTID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.Report.StoredProcedures.ComparativeByUser.ResultFields.ACCURATEFULFILLMENTCRITICALERRORRESULTID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.Report.StoredProcedures.ComparativeByUser.ResultFields.ACCURATENONCRITICALERRORRESULT]),

                        Convert.ToInt32(dr[SCC_DATA.Queries.Report.StoredProcedures.ComparativeByUser.ResultFields.CONTROLLABLERESULTID]),
                        Convert.ToInt32(dr[SCC_DATA.Queries.Report.StoredProcedures.ComparativeByUser.ResultFields.CONTROLLABLEFINALUSERCRITICALERRORRESULTID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.Report.StoredProcedures.ComparativeByUser.ResultFields.CONTROLLABLEBUSINESSCRITICALERRORRESULTID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.Report.StoredProcedures.ComparativeByUser.ResultFields.CONTROLLABLEFULFILLMENTCRITICALERRORRESULTID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.Report.StoredProcedures.ComparativeByUser.ResultFields.CONTROLLABLENONCRITICALERRORRESULT]),

						Convert.ToInt32(dr[SCC_DATA.Queries.Report.StoredProcedures.ComparativeByUser.ResultFields.USER_ID]));

					comparativeByUserResultList.Add(comparativeByUserResult);
				}
			}

			return comparativeByUserResultList;
		}
		
		public List<SCC_BL.Reports.Results.ComparativeByProgram> ComparativeByProgram(DateTime? transactionStartDate, DateTime? transactionEndDate, DateTime? evaluationStartDate, DateTime? evaluationEndDate, string programIDList, string userIDArray, string supervisorUserIDArray, string evaluatorUserIDArray, string errorTypeIDArray, bool? attributeControllable, bool? attributeKnown, string transactionCustomFieldCatalogList)
		{
			List<SCC_BL.Reports.Results.ComparativeByProgram> comparativeByProgramResultList = new List<SCC_BL.Reports.Results.ComparativeByProgram>();

			using (SCC_DATA.Repositories.Report report = new SCC_DATA.Repositories.Report())
			{
				DataTable dt = report.ComparativeByProgram(transactionStartDate, transactionEndDate, evaluationStartDate, evaluationEndDate, programIDList, userIDArray, supervisorUserIDArray, evaluatorUserIDArray, errorTypeIDArray, attributeControllable, attributeKnown, transactionCustomFieldCatalogList);

				foreach (DataRow dr in dt.Rows)
				{
					SCC_BL.Reports.Results.ComparativeByProgram comparativeByProgramResult = new SCC_BL.Reports.Results.ComparativeByProgram(
						Convert.ToInt32(dr[SCC_DATA.Queries.Report.StoredProcedures.ComparativeByProgram.ResultFields.TRANSACTION_ID]),

                        Convert.ToInt32(dr[SCC_DATA.Queries.Report.StoredProcedures.ComparativeByProgram.ResultFields.GENERALRESULTID]),
                        Convert.ToInt32(dr[SCC_DATA.Queries.Report.StoredProcedures.ComparativeByProgram.ResultFields.GENERALFINALUSERCRITICALERRORRESULTID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.Report.StoredProcedures.ComparativeByProgram.ResultFields.GENERALBUSINESSCRITICALERRORRESULTID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.Report.StoredProcedures.ComparativeByProgram.ResultFields.GENERALFULFILLMENTCRITICALERRORRESULTID]),

                        Convert.ToInt32(dr[SCC_DATA.Queries.Report.StoredProcedures.ComparativeByProgram.ResultFields.ACCURATERESULTID]),
                        Convert.ToInt32(dr[SCC_DATA.Queries.Report.StoredProcedures.ComparativeByProgram.ResultFields.ACCURATEFINALUSERCRITICALERRORRESULTID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.Report.StoredProcedures.ComparativeByProgram.ResultFields.ACCURATEBUSINESSCRITICALERRORRESULTID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.Report.StoredProcedures.ComparativeByProgram.ResultFields.ACCURATEFULFILLMENTCRITICALERRORRESULTID]),

                        Convert.ToInt32(dr[SCC_DATA.Queries.Report.StoredProcedures.ComparativeByProgram.ResultFields.CONTROLLABLERESULTID]),
                        Convert.ToInt32(dr[SCC_DATA.Queries.Report.StoredProcedures.ComparativeByProgram.ResultFields.CONTROLLABLEFINALUSERCRITICALERRORRESULTID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.Report.StoredProcedures.ComparativeByProgram.ResultFields.CONTROLLABLEBUSINESSCRITICALERRORRESULTID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.Report.StoredProcedures.ComparativeByProgram.ResultFields.CONTROLLABLEFULFILLMENTCRITICALERRORRESULTID]),

						Convert.ToInt32(dr[SCC_DATA.Queries.Report.StoredProcedures.ComparativeByProgram.ResultFields.PROGRAM_ID]));

					comparativeByProgramResultList.Add(comparativeByProgramResult);
				}
			}

			return comparativeByProgramResultList;
		}
		
		public List<SCC_BL.Reports.Results.CalibratorComparison> CalibratorComparison(DateTime? calibrationStartDate, DateTime? calibrationEndDate, string programIDList, string calibratedUserIDArray, string calibratedSupervisorUserIDArray, string calibratorUserIDArray, string calibrationTypeIDArray, string errorTypeIDArray)
		{
			List<SCC_BL.Reports.Results.CalibratorComparison> calibratorComparisonResultList = new List<SCC_BL.Reports.Results.CalibratorComparison>();

			using (SCC_DATA.Repositories.Report report = new SCC_DATA.Repositories.Report())
			{
				DataTable dt = report.CalibratorComparison(calibrationStartDate, calibrationEndDate, programIDList, calibratedUserIDArray, calibratedSupervisorUserIDArray, calibratorUserIDArray, calibrationTypeIDArray, errorTypeIDArray);

				foreach (DataRow dr in dt.Rows)
				{
					SCC_BL.Reports.Results.CalibratorComparison calibratorComparisonResult = new SCC_BL.Reports.Results.CalibratorComparison(
						Convert.ToInt32(dr[SCC_DATA.Queries.Report.StoredProcedures.CalibratorComparison.ResultFields.TRANSACTION_ID]),

                        Convert.ToInt32(dr[SCC_DATA.Queries.Report.StoredProcedures.CalibratorComparison.ResultFields.GLOBALGENERALRESULTID]),
                        Convert.ToInt32(dr[SCC_DATA.Queries.Report.StoredProcedures.CalibratorComparison.ResultFields.GLOBALGENERALFINALUSERCRITICALERRORRESULTID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.Report.StoredProcedures.CalibratorComparison.ResultFields.GLOBALGENERALBUSINESSCRITICALERRORRESULTID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.Report.StoredProcedures.CalibratorComparison.ResultFields.GLOBALGENERALFULFILLMENTCRITICALERRORRESULTID]),

                        Convert.ToInt32(dr[SCC_DATA.Queries.Report.StoredProcedures.CalibratorComparison.ResultFields.GLOBALACCURATERESULTID]),
                        Convert.ToInt32(dr[SCC_DATA.Queries.Report.StoredProcedures.CalibratorComparison.ResultFields.GLOBALACCURATEFINALUSERCRITICALERRORRESULTID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.Report.StoredProcedures.CalibratorComparison.ResultFields.GLOBALACCURATEBUSINESSCRITICALERRORRESULTID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.Report.StoredProcedures.CalibratorComparison.ResultFields.GLOBALACCURATEFULFILLMENTCRITICALERRORRESULTID]),

                        Convert.ToInt32(dr[SCC_DATA.Queries.Report.StoredProcedures.CalibratorComparison.ResultFields.GLOBALCONTROLLABLERESULTID]),
                        Convert.ToInt32(dr[SCC_DATA.Queries.Report.StoredProcedures.CalibratorComparison.ResultFields.GLOBALCONTROLLABLEFINALUSERCRITICALERRORRESULTID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.Report.StoredProcedures.CalibratorComparison.ResultFields.GLOBALCONTROLLABLEBUSINESSCRITICALERRORRESULTID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.Report.StoredProcedures.CalibratorComparison.ResultFields.GLOBALCONTROLLABLEFULFILLMENTCRITICALERRORRESULTID]),

						Convert.ToInt32(dr[SCC_DATA.Queries.Report.StoredProcedures.CalibratorComparison.ResultFields.CALIBRATOR_USER_ID]));

					calibratorComparisonResultList.Add(calibratorComparisonResult);
				}
			}

			return calibratorComparisonResultList;
		}
		
		public List<SCC_BL.Reports.Results.AccuracyTrend> AccuracyTrend(DateTime? transactionStartDate, DateTime? transactionEndDate, DateTime? evaluationStartDate, DateTime? evaluationEndDate, string programIDList, string userIDArray, string supervisorUserIDArray, string evaluatorUserIDArray, string errorTypeIDArray, bool? attributeControllable, bool? attributeKnown, string transactionCustomFieldCatalogList)
		{
			List<SCC_BL.Reports.Results.AccuracyTrend> accuracyTrendResultList = new List<SCC_BL.Reports.Results.AccuracyTrend>();

			using (SCC_DATA.Repositories.Report report = new SCC_DATA.Repositories.Report())
			{
				DataTable dt = report.AccuracyTrend(transactionStartDate, transactionEndDate, evaluationStartDate, evaluationEndDate, programIDList, userIDArray, supervisorUserIDArray, evaluatorUserIDArray, errorTypeIDArray, attributeControllable, attributeKnown, transactionCustomFieldCatalogList);

				foreach (DataRow dr in dt.Rows)
				{
					SCC_BL.Reports.Results.AccuracyTrend accuracyTrendResult = new SCC_BL.Reports.Results.AccuracyTrend(
						Convert.ToInt32(dr[SCC_DATA.Queries.Report.StoredProcedures.AccuracyTrend.ResultFields.TRANSACTION_ID]),
						Convert.ToDateTime(dr[SCC_DATA.Queries.Report.StoredProcedures.AccuracyTrend.ResultFields.TRANSACTION_DATE]),

						Convert.ToInt32(dr[SCC_DATA.Queries.Report.StoredProcedures.AccuracyTrend.ResultFields.GENERALFINALUSERCRITICALERRORRESULTID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.Report.StoredProcedures.AccuracyTrend.ResultFields.GENERALBUSINESSCRITICALERRORRESULTID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.Report.StoredProcedures.AccuracyTrend.ResultFields.GENERALFULFILLMENTCRITICALERRORRESULTID]),
						Convert.ToDouble(dr[SCC_DATA.Queries.Report.StoredProcedures.AccuracyTrend.ResultFields.GENERALNONCRITICALERRORRESULT]),
                        Convert.ToInt32(dr[SCC_DATA.Queries.Report.StoredProcedures.AccuracyTrend.ResultFields.GENERALRESULTID]),

						Convert.ToInt32(dr[SCC_DATA.Queries.Report.StoredProcedures.AccuracyTrend.ResultFields.ACCURATEFINALUSERCRITICALERRORRESULTID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.Report.StoredProcedures.AccuracyTrend.ResultFields.ACCURATEBUSINESSCRITICALERRORRESULTID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.Report.StoredProcedures.AccuracyTrend.ResultFields.ACCURATEFULFILLMENTCRITICALERRORRESULTID]),
						Convert.ToDouble(dr[SCC_DATA.Queries.Report.StoredProcedures.AccuracyTrend.ResultFields.ACCURATENONCRITICALERRORRESULT]),
                        Convert.ToInt32(dr[SCC_DATA.Queries.Report.StoredProcedures.AccuracyTrend.ResultFields.ACCURATERESULTID]),

						Convert.ToInt32(dr[SCC_DATA.Queries.Report.StoredProcedures.AccuracyTrend.ResultFields.CONTROLLABLEFINALUSERCRITICALERRORRESULTID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.Report.StoredProcedures.AccuracyTrend.ResultFields.CONTROLLABLEBUSINESSCRITICALERRORRESULTID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.Report.StoredProcedures.AccuracyTrend.ResultFields.CONTROLLABLEFULFILLMENTCRITICALERRORRESULTID]),
                        Convert.ToDouble(dr[SCC_DATA.Queries.Report.StoredProcedures.AccuracyTrend.ResultFields.CONTROLLABLENONCRITICALERRORRESULT]),
                        Convert.ToInt32(dr[SCC_DATA.Queries.Report.StoredProcedures.AccuracyTrend.ResultFields.CONTROLLABLERESULTID]));

					accuracyTrendResultList.Add(accuracyTrendResult);
				}
			}

			return accuracyTrendResultList;
		}
		
		public List<SCC_BL.Reports.Results.AccuracyByAttribute> AccuracyByAttribute(string transactionIDArray, string errorTypeIDList, int constraintTypeID, bool mustBeControllable, string attributeIDArray = null)
		{
			List<SCC_BL.Reports.Results.AccuracyByAttribute> accuracyByAttributeResultList = new List<SCC_BL.Reports.Results.AccuracyByAttribute>();

			using (SCC_DATA.Repositories.Report report = new SCC_DATA.Repositories.Report())
			{
				DataTable dt = report.AccuracyByAttribute(
					transactionIDArray, 
					errorTypeIDList,
                    constraintTypeID,
					attributeIDArray);

				foreach (DataRow dr in dt.Rows)
				{
					SCC_BL.Reports.Results.AccuracyByAttribute accuracyByAttributeResult = new SCC_BL.Reports.Results.AccuracyByAttribute(
						Convert.ToInt32(dr[SCC_DATA.Queries.Report.StoredProcedures.AccuracyByAttribute.ResultFields.TRANSACTION_ATTRIBUTE_ID]),
                        Convert.ToInt32(dr[SCC_DATA.Queries.Report.StoredProcedures.AccuracyByAttribute.ResultFields.TRANSACTION_ID]),
                        Convert.ToInt32(dr[SCC_DATA.Queries.Report.StoredProcedures.AccuracyByAttribute.ResultFields.ATTRIBUTE_ID]),
						Convert.ToString(dr[SCC_DATA.Queries.Report.StoredProcedures.AccuracyByAttribute.ResultFields.ATTRIBUTE_NAME]),
						Convert.ToBoolean(dr[SCC_DATA.Queries.Report.StoredProcedures.AccuracyByAttribute.ResultFields.SUCCESSFUL_RESULT]),
                        mustBeControllable);

					accuracyByAttributeResultList.Add(accuracyByAttributeResult);
				}
			}

			return accuracyByAttributeResultList;
		}
		
		public List<SCC_BL.Reports.Results.AccuracyTrendByAttribute> AccuracyTrendByAttribute(string transactionIDArray, string errorTypeIDList, string attributeIDArray = null)
		{
			List<SCC_BL.Reports.Results.AccuracyTrendByAttribute> accuracyTrendByAttributeResultList = new List<SCC_BL.Reports.Results.AccuracyTrendByAttribute>();

			using (SCC_DATA.Repositories.Report report = new SCC_DATA.Repositories.Report())
			{
				DataTable dt = report.AccuracyTrendByAttribute(
					transactionIDArray, 
					errorTypeIDList,
					attributeIDArray);

				foreach (DataRow dr in dt.Rows)
				{
					SCC_BL.Reports.Results.AccuracyTrendByAttribute accuracyTrendByAttributeResult = new SCC_BL.Reports.Results.AccuracyTrendByAttribute(
						Convert.ToInt32(dr[SCC_DATA.Queries.Report.StoredProcedures.AccuracyTrendByAttribute.ResultFields.TRANSACTION_ATTRIBUTE_ID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.Report.StoredProcedures.AccuracyTrendByAttribute.ResultFields.TRANSACTION_ID]),
						Convert.ToDateTime(dr[SCC_DATA.Queries.Report.StoredProcedures.AccuracyTrendByAttribute.ResultFields.TRANSACTION_DATE]),
						Convert.ToInt32(dr[SCC_DATA.Queries.Report.StoredProcedures.AccuracyTrendByAttribute.ResultFields.ATTRIBUTE_ID]),
						Convert.ToString(dr[SCC_DATA.Queries.Report.StoredProcedures.AccuracyTrendByAttribute.ResultFields.ATTRIBUTE_NAME]),
						Convert.ToBoolean(dr[SCC_DATA.Queries.Report.StoredProcedures.AccuracyTrendByAttribute.ResultFields.SUCCESSFUL_RESULT]));

					accuracyTrendByAttributeResultList.Add(accuracyTrendByAttributeResult);
				}
			}

			return accuracyTrendByAttributeResultList;
		}
		
		public List<SCC_BL.Reports.Results.AccuracyBySubattribute> AccuracyBySubattribute(int selectedAttributeID, string transactionAttributeIDList, bool mustBeControllable)
		{
			List<SCC_BL.Reports.Results.AccuracyBySubattribute> accuracyBySubattributeResultList = new List<SCC_BL.Reports.Results.AccuracyBySubattribute>();

			using (SCC_DATA.Repositories.Report report = new SCC_DATA.Repositories.Report())
			{
				DataTable dt = report.AccuracyBySubattribute(selectedAttributeID, transactionAttributeIDList);

				foreach (DataRow dr in dt.Rows)
				{
					SCC_BL.Reports.Results.AccuracyBySubattribute accuracyBySubattributeResult = new SCC_BL.Reports.Results.AccuracyBySubattribute(
						Convert.ToInt32(dr[SCC_DATA.Queries.Report.StoredProcedures.AccuracyBySubattribute.ResultFields.TRANSACTION_ATTRIBUTE_ID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.Report.StoredProcedures.AccuracyBySubattribute.ResultFields.TRANSACTION_ID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.Report.StoredProcedures.AccuracyBySubattribute.ResultFields.ATTRIBUTE_ID]),
						Convert.ToString(dr[SCC_DATA.Queries.Report.StoredProcedures.AccuracyBySubattribute.ResultFields.ATTRIBUTE_NAME]),
						Convert.ToBoolean(dr[SCC_DATA.Queries.Report.StoredProcedures.AccuracyBySubattribute.ResultFields.SUCCESSFUL_RESULT]),
						Convert.ToBoolean(dr[SCC_DATA.Queries.Report.StoredProcedures.AccuracyBySubattribute.ResultFields.HAS_CHILDREN]),
						Convert.ToInt32(dr[SCC_DATA.Queries.Report.StoredProcedures.AccuracyBySubattribute.ResultFields.ERROR_TYPE_ID]),
                        mustBeControllable);

					accuracyBySubattributeResultList.Add(accuracyBySubattributeResult);
				}
			}

			return accuracyBySubattributeResultList;
		}
		
		public List<SCC_BL.Reports.Results.ParetoBI> ParetoBI(string transactionIDList, string biFieldIDList)
		{
			List<SCC_BL.Reports.Results.ParetoBI> paretoBIResultList = new List<SCC_BL.Reports.Results.ParetoBI>();

			using (SCC_DATA.Repositories.Report report = new SCC_DATA.Repositories.Report())
			{
				DataTable dt = report.ParetoBI(transactionIDList, biFieldIDList);

				foreach (DataRow dr in dt.Rows)
				{
					SCC_BL.Reports.Results.ParetoBI paretoBIResult = new SCC_BL.Reports.Results.ParetoBI(
						Convert.ToInt32(dr[SCC_DATA.Queries.Report.StoredProcedures.ParetoBI.ResultFields.TRANSACTION_BI_FIELD_CATALOG_ID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.Report.StoredProcedures.ParetoBI.ResultFields.BUSINESS_INTELLIGENCE_FIELD_ID]),
						Convert.ToString(dr[SCC_DATA.Queries.Report.StoredProcedures.ParetoBI.ResultFields.BUSINESS_INTELLIGENCE_FIELD_NAME]),
						Convert.ToBoolean(dr[SCC_DATA.Queries.Report.StoredProcedures.ParetoBI.ResultFields.SUCCESSFUL_RESULT]),
						Convert.ToBoolean(dr[SCC_DATA.Queries.Report.StoredProcedures.ParetoBI.ResultFields.HAS_CHILDREN]));

					paretoBIResultList.Add(paretoBIResult);
				}
			}

			return paretoBIResultList;
		}

		public void Dispose()
        {

        }
	}
}
