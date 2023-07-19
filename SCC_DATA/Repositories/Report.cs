using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCC_DATA.Repositories
{
    public class Report : IDisposable
	{
		public System.Data.DataTable OverallAccuracy(DateTime? transactionStartDate, DateTime? transactionEndDate, DateTime? evaluationStartDate, DateTime? evaluationEndDate, string programIDList, string userIDArray, string supervisorUserIDArray, string evaluatorUserIDArray, string errorTypeIDArray, bool? attributeControllable, bool? attributeKnown, string transactionCustomFieldCatalogList)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.Report.StoredProcedures.OverallAccuracy.Parameters.TRANSACTION_START_DATE, transactionStartDate, System.Data.SqlDbType.DateTime),
						db.CreateParameter(Queries.Report.StoredProcedures.OverallAccuracy.Parameters.TRANSACTION_END_DATE, transactionEndDate, System.Data.SqlDbType.DateTime),
						db.CreateParameter(Queries.Report.StoredProcedures.OverallAccuracy.Parameters.EVALUATION_START_DATE, evaluationStartDate, System.Data.SqlDbType.DateTime),
						db.CreateParameter(Queries.Report.StoredProcedures.OverallAccuracy.Parameters.EVALUATION_END_DATE, evaluationEndDate, System.Data.SqlDbType.DateTime),
						db.CreateParameter(Queries.Report.StoredProcedures.OverallAccuracy.Parameters.PROGRAM_ID_LIST, programIDList, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.Report.StoredProcedures.OverallAccuracy.Parameters.USER_ID_LIST, userIDArray, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.Report.StoredProcedures.OverallAccuracy.Parameters.SUPERVISOR_USER_ID_LIST, supervisorUserIDArray, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.Report.StoredProcedures.OverallAccuracy.Parameters.EVALUATOR_USER_ID_LIST, evaluatorUserIDArray, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.Report.StoredProcedures.OverallAccuracy.Parameters.ERROR_TYPE_ID_LIST, errorTypeIDArray, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.Report.StoredProcedures.OverallAccuracy.Parameters.ATTRIBUTE_CONTROLLABLE, attributeControllable, System.Data.SqlDbType.Bit),
						db.CreateParameter(Queries.Report.StoredProcedures.OverallAccuracy.Parameters.ATTRIBUTE_KNOWN, attributeKnown, System.Data.SqlDbType.Bit),
						db.CreateParameter(Queries.Report.StoredProcedures.OverallAccuracy.Parameters.TRANSACTION_CUSTOM_FIELD_CATALOG_LIST, transactionCustomFieldCatalogList, System.Data.SqlDbType.VarChar)
					};

					return
						db.Select(
							Queries.Report.StoredProcedures.OverallAccuracy.NAME,
							parameters
						);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		
		public System.Data.DataTable ComparativeByUser(DateTime? transactionStartDate, DateTime? transactionEndDate, DateTime? evaluationStartDate, DateTime? evaluationEndDate, string programIDList, string userIDArray, string supervisorUserIDArray, string evaluatorUserIDArray, string errorTypeIDArray, bool? attributeControllable, bool? attributeKnown, string transactionCustomFieldCatalogList)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.Report.StoredProcedures.ComparativeByUser.Parameters.TRANSACTION_START_DATE, transactionStartDate, System.Data.SqlDbType.DateTime),
						db.CreateParameter(Queries.Report.StoredProcedures.ComparativeByUser.Parameters.TRANSACTION_END_DATE, transactionEndDate, System.Data.SqlDbType.DateTime),
						db.CreateParameter(Queries.Report.StoredProcedures.ComparativeByUser.Parameters.EVALUATION_START_DATE, evaluationStartDate, System.Data.SqlDbType.DateTime),
						db.CreateParameter(Queries.Report.StoredProcedures.ComparativeByUser.Parameters.EVALUATION_END_DATE, evaluationEndDate, System.Data.SqlDbType.DateTime),
						db.CreateParameter(Queries.Report.StoredProcedures.ComparativeByUser.Parameters.PROGRAM_ID_LIST, programIDList, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.Report.StoredProcedures.ComparativeByUser.Parameters.USER_ID_LIST, userIDArray, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.Report.StoredProcedures.ComparativeByUser.Parameters.SUPERVISOR_USER_ID_LIST, supervisorUserIDArray, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.Report.StoredProcedures.ComparativeByUser.Parameters.EVALUATOR_USER_ID_LIST, evaluatorUserIDArray, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.Report.StoredProcedures.ComparativeByUser.Parameters.ERROR_TYPE_ID_LIST, errorTypeIDArray, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.Report.StoredProcedures.ComparativeByUser.Parameters.ATTRIBUTE_CONTROLLABLE, attributeControllable, System.Data.SqlDbType.Bit),
						db.CreateParameter(Queries.Report.StoredProcedures.ComparativeByUser.Parameters.ATTRIBUTE_KNOWN, attributeKnown, System.Data.SqlDbType.Bit),
                        db.CreateParameter(Queries.Report.StoredProcedures.ComparativeByUser.Parameters.TRANSACTION_CUSTOM_FIELD_CATALOG_LIST, transactionCustomFieldCatalogList, System.Data.SqlDbType.VarChar)
                    };

					return
						db.Select(
							Queries.Report.StoredProcedures.ComparativeByUser.NAME,
							parameters
						);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		
		public System.Data.DataTable ComparativeByProgram(DateTime? transactionStartDate, DateTime? transactionEndDate, DateTime? evaluationStartDate, DateTime? evaluationEndDate, string programIDList, string userIDArray, string supervisorUserIDArray, string evaluatorUserIDArray, string errorTypeIDArray, bool? attributeControllable, bool? attributeKnown, string transactionCustomFieldCatalogList)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.Report.StoredProcedures.ComparativeByProgram.Parameters.TRANSACTION_START_DATE, transactionStartDate, System.Data.SqlDbType.DateTime),
						db.CreateParameter(Queries.Report.StoredProcedures.ComparativeByProgram.Parameters.TRANSACTION_END_DATE, transactionEndDate, System.Data.SqlDbType.DateTime),
						db.CreateParameter(Queries.Report.StoredProcedures.ComparativeByProgram.Parameters.EVALUATION_START_DATE, evaluationStartDate, System.Data.SqlDbType.DateTime),
						db.CreateParameter(Queries.Report.StoredProcedures.ComparativeByProgram.Parameters.EVALUATION_END_DATE, evaluationEndDate, System.Data.SqlDbType.DateTime),
						db.CreateParameter(Queries.Report.StoredProcedures.ComparativeByProgram.Parameters.PROGRAM_ID_LIST, programIDList, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.Report.StoredProcedures.ComparativeByProgram.Parameters.USER_ID_LIST, userIDArray, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.Report.StoredProcedures.ComparativeByProgram.Parameters.SUPERVISOR_USER_ID_LIST, supervisorUserIDArray, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.Report.StoredProcedures.ComparativeByProgram.Parameters.EVALUATOR_USER_ID_LIST, evaluatorUserIDArray, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.Report.StoredProcedures.ComparativeByProgram.Parameters.ERROR_TYPE_ID_LIST, errorTypeIDArray, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.Report.StoredProcedures.ComparativeByProgram.Parameters.ATTRIBUTE_CONTROLLABLE, attributeControllable, System.Data.SqlDbType.Bit),
						db.CreateParameter(Queries.Report.StoredProcedures.ComparativeByProgram.Parameters.ATTRIBUTE_KNOWN, attributeKnown, System.Data.SqlDbType.Bit),
                        db.CreateParameter(Queries.Report.StoredProcedures.ComparativeByProgram.Parameters.TRANSACTION_CUSTOM_FIELD_CATALOG_LIST, transactionCustomFieldCatalogList, System.Data.SqlDbType.VarChar)
                    };

					return
						db.Select(
							Queries.Report.StoredProcedures.ComparativeByProgram.NAME,
							parameters
						);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		
		public System.Data.DataTable CalibratorComparison(DateTime? calibrationStartDate, DateTime? calibrationEndDate, string programIDList, string calibratedUserIDArray, string calibratedSupervisorUserIDArray, string calibratorUserIDArray, string calibrationTypeIDArray, string errorTypeIDArray)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.Report.StoredProcedures.CalibratorComparison.Parameters.CALIBRATION_START_DATE, calibrationStartDate, System.Data.SqlDbType.DateTime),
						db.CreateParameter(Queries.Report.StoredProcedures.CalibratorComparison.Parameters.CALIBRATION_END_DATE, calibrationEndDate, System.Data.SqlDbType.DateTime),
						db.CreateParameter(Queries.Report.StoredProcedures.CalibratorComparison.Parameters.PROGRAM_ID_LIST, programIDList, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.Report.StoredProcedures.CalibratorComparison.Parameters.CALIBRATED_USER_ID_LIST, calibratedUserIDArray, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.Report.StoredProcedures.CalibratorComparison.Parameters.CALIBRATED_SUPERVISOR_USER_ID_LIST, calibratedSupervisorUserIDArray, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.Report.StoredProcedures.CalibratorComparison.Parameters.CALIBRATOR_USER_ID_LIST, calibratorUserIDArray, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.Report.StoredProcedures.CalibratorComparison.Parameters.CALIBRATION_TYPE_ID_LIST, calibrationTypeIDArray, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.Report.StoredProcedures.CalibratorComparison.Parameters.ERROR_TYPE_ID_LIST, errorTypeIDArray, System.Data.SqlDbType.VarChar)
					};

					return
						db.Select(
							Queries.Report.StoredProcedures.CalibratorComparison.NAME,
							parameters
						);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		
		public System.Data.DataTable AccuracyTrend(DateTime? transactionStartDate, DateTime? transactionEndDate, DateTime? evaluationStartDate, DateTime? evaluationEndDate, string programIDList, string userIDArray, string supervisorUserIDArray, string evaluatorUserIDArray, string errorTypeIDArray, bool? attributeControllable, bool? attributeKnown, string transactionCustomFieldCatalogList)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.Report.StoredProcedures.AccuracyTrend.Parameters.TRANSACTION_START_DATE, transactionStartDate, System.Data.SqlDbType.DateTime),
						db.CreateParameter(Queries.Report.StoredProcedures.AccuracyTrend.Parameters.TRANSACTION_END_DATE, transactionEndDate, System.Data.SqlDbType.DateTime),
						db.CreateParameter(Queries.Report.StoredProcedures.AccuracyTrend.Parameters.EVALUATION_START_DATE, evaluationStartDate, System.Data.SqlDbType.DateTime),
						db.CreateParameter(Queries.Report.StoredProcedures.AccuracyTrend.Parameters.EVALUATION_END_DATE, evaluationEndDate, System.Data.SqlDbType.DateTime),
						db.CreateParameter(Queries.Report.StoredProcedures.AccuracyTrend.Parameters.PROGRAM_ID_LIST, programIDList, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.Report.StoredProcedures.AccuracyTrend.Parameters.USER_ID_LIST, userIDArray, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.Report.StoredProcedures.AccuracyTrend.Parameters.SUPERVISOR_USER_ID_LIST, supervisorUserIDArray, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.Report.StoredProcedures.AccuracyTrend.Parameters.EVALUATOR_USER_ID_LIST, evaluatorUserIDArray, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.Report.StoredProcedures.AccuracyTrend.Parameters.ERROR_TYPE_ID_LIST, errorTypeIDArray, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.Report.StoredProcedures.AccuracyTrend.Parameters.ATTRIBUTE_CONTROLLABLE, attributeControllable, System.Data.SqlDbType.Bit),
						db.CreateParameter(Queries.Report.StoredProcedures.AccuracyTrend.Parameters.ATTRIBUTE_KNOWN, attributeKnown, System.Data.SqlDbType.Bit),
                        db.CreateParameter(Queries.Report.StoredProcedures.AccuracyTrend.Parameters.TRANSACTION_CUSTOM_FIELD_CATALOG_LIST, transactionCustomFieldCatalogList, System.Data.SqlDbType.VarChar)
                    };

					return
						db.Select(
							Queries.Report.StoredProcedures.AccuracyTrend.NAME,
							parameters
						);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		
		public System.Data.DataTable AccuracyByAttribute(string transactionIDArray, string errorTypeIDList, string attributeIDArray = null)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.Report.StoredProcedures.AccuracyByAttribute.Parameters.TRANSACTION_ID_LIST, transactionIDArray, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.Report.StoredProcedures.AccuracyByAttribute.Parameters.ERROR_TYPE_ID_LIST, errorTypeIDList, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.Report.StoredProcedures.AccuracyByAttribute.Parameters.ATTRIBUTE_ID_LIST, attributeIDArray, System.Data.SqlDbType.VarChar)
					};

					return
						db.Select(
							Queries.Report.StoredProcedures.AccuracyByAttribute.NAME,
							parameters
						);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		
		public System.Data.DataTable AccuracyTrendByAttribute(string transactionIDArray, string errorTypeIDList, string attributeIDArray = null)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.Report.StoredProcedures.AccuracyTrendByAttribute.Parameters.TRANSACTION_ID_LIST, transactionIDArray, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.Report.StoredProcedures.AccuracyTrendByAttribute.Parameters.ERROR_TYPE_ID_LIST, errorTypeIDList, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.Report.StoredProcedures.AccuracyTrendByAttribute.Parameters.ATTRIBUTE_ID_LIST, attributeIDArray, System.Data.SqlDbType.VarChar)
					};

					return
						db.Select(
							Queries.Report.StoredProcedures.AccuracyTrendByAttribute.NAME,
							parameters
						);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		
		public System.Data.DataTable AccuracyBySubattribute(string transactionAttributeIDList)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.Report.StoredProcedures.AccuracyBySubattribute.Parameters.TRANSACTION_ATTRIBUTE_ID_LIST, transactionAttributeIDList, System.Data.SqlDbType.VarChar)
					};

					return
						db.Select(
							Queries.Report.StoredProcedures.AccuracyBySubattribute.NAME,
							parameters
						);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		
		public System.Data.DataTable ParetoBI(string transactionIDList, string biFieldIDList)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.Report.StoredProcedures.ParetoBI.Parameters.TRANSACTION_ID_LIST, transactionIDList, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.Report.StoredProcedures.ParetoBI.Parameters.BI_FIELD_ID_LIST, biFieldIDList, System.Data.SqlDbType.VarChar)
					};

					return
						db.Select(
							Queries.Report.StoredProcedures.ParetoBI.NAME,
							parameters
						);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public void Dispose()
        {

        }
    }
}
