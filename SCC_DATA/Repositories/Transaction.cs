using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCC_DATA.Repositories
{
	public class Transaction : IDisposable
	{
		public int DeleteByID(int id)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.Transaction.StoredProcedures.DeleteByID.Parameters.ID, id, System.Data.SqlDbType.Int)
					};

					return
						db.Execute(
							Queries.Transaction.StoredProcedures.DeleteByID.NAME,
							parameters
						);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public int Insert(
			string identifier, 
			int userToEvaluateID, 
			int evaluatorUserID, 
			DateTime evaluationDate, 
			DateTime transactionDate, 
			DateTime loadDate, 
			int formID, 
			string comment, 
			int generalResultID, 
			int generalFinalUserCriticalErrorResultID, 
			int generalBusinessCriticalErrorResultID, 
			int generalFulfillmentCriticalErrorResultID, 
			int generalNonCriticalErrorResult, 
			int accurateResultID, 
			int accurateFinalUserCriticalErrorResultID, 
			int accurateBusinessCriticalErrorResultID, 
			int accurateFulfillmentCriticalErrorResultID, 
			int accurateNonCriticalErrorResult, 
			int controllableResultID, 
			int controllableFinalUserCriticalErrorResultID, 
			int controllableBusinessCriticalErrorResultID, 
			int controllableFulfillmentCriticalErrorResultID, 
			int controllableNonCriticalErrorResult, 
			TimeSpan timeElapsed, 
			int basicInfoID,
			int typeID, 
			int? calibratedTransactionID)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.Transaction.StoredProcedures.Insert.Parameters.IDENTIFIER, identifier, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.Transaction.StoredProcedures.Insert.Parameters.USERTOEVALUATEID, userToEvaluateID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.Transaction.StoredProcedures.Insert.Parameters.EVALUATORUSERID, evaluatorUserID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.Transaction.StoredProcedures.Insert.Parameters.EVALUATIONDATE, evaluationDate, System.Data.SqlDbType.DateTime),
						db.CreateParameter(Queries.Transaction.StoredProcedures.Insert.Parameters.TRANSACTIONDATE, transactionDate, System.Data.SqlDbType.DateTime),
						db.CreateParameter(Queries.Transaction.StoredProcedures.Insert.Parameters.LOAD_DATE, loadDate, System.Data.SqlDbType.DateTime),
						db.CreateParameter(Queries.Transaction.StoredProcedures.Insert.Parameters.FORMID, formID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.Transaction.StoredProcedures.Insert.Parameters.COMMENT, comment, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.Transaction.StoredProcedures.Insert.Parameters.GENERALRESULTID, generalResultID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.Transaction.StoredProcedures.Insert.Parameters.GENERALFINALUSERCRITICALERRORRESULTID, generalFinalUserCriticalErrorResultID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.Transaction.StoredProcedures.Insert.Parameters.GENERALBUSINESSCRITICALERRORRESULTID, generalBusinessCriticalErrorResultID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.Transaction.StoredProcedures.Insert.Parameters.GENERALFULFILLMENTCRITICALERRORRESULTID, generalFulfillmentCriticalErrorResultID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.Transaction.StoredProcedures.Insert.Parameters.GENERALNONCRITICALERRORRESULT, generalNonCriticalErrorResult, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.Transaction.StoredProcedures.Insert.Parameters.ACCURATERESULTID, accurateResultID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.Transaction.StoredProcedures.Insert.Parameters.ACCURATEFINALUSERCRITICALERRORRESULTID, accurateFinalUserCriticalErrorResultID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.Transaction.StoredProcedures.Insert.Parameters.ACCURATEBUSINESSCRITICALERRORRESULTID, accurateBusinessCriticalErrorResultID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.Transaction.StoredProcedures.Insert.Parameters.ACCURATEFULFILLMENTCRITICALERRORRESULTID, accurateFulfillmentCriticalErrorResultID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.Transaction.StoredProcedures.Insert.Parameters.ACCURATENONCRITICALERRORRESULT, accurateNonCriticalErrorResult, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.Transaction.StoredProcedures.Insert.Parameters.CONTROLLABLERESULTID, controllableResultID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.Transaction.StoredProcedures.Insert.Parameters.CONTROLLABLEFINALUSERCRITICALERRORRESULTID, controllableFinalUserCriticalErrorResultID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.Transaction.StoredProcedures.Insert.Parameters.CONTROLLABLEBUSINESSCRITICALERRORRESULTID, controllableBusinessCriticalErrorResultID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.Transaction.StoredProcedures.Insert.Parameters.CONTROLLABLEFULFILLMENTCRITICALERRORRESULTID, controllableFulfillmentCriticalErrorResultID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.Transaction.StoredProcedures.Insert.Parameters.CONTROLLABLENONCRITICALERRORRESULT, controllableNonCriticalErrorResult, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.Transaction.StoredProcedures.Insert.Parameters.TIMEELAPSED, timeElapsed, System.Data.SqlDbType.Time),
						db.CreateParameter(Queries.Transaction.StoredProcedures.Insert.Parameters.BASICINFOID, basicInfoID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.Transaction.StoredProcedures.Insert.Parameters.TYPE_ID, typeID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.Transaction.StoredProcedures.Insert.Parameters.CALIBRATED_TRANSACTION_ID, calibratedTransactionID, System.Data.SqlDbType.Int)
					};

					return
						(int)db.ReadFirstColumn(
							Queries.Transaction.StoredProcedures.Insert.NAME,
							parameters
						);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public System.Data.DataRow SelectByID(int id)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.Transaction.StoredProcedures.SelectByID.Parameters.ID, id, System.Data.SqlDbType.Int)
					};

					return
						db.Select(
							Queries.Transaction.StoredProcedures.SelectByID.NAME,
							parameters
						).Rows[0];
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public System.Data.DataRow GetProgramID(int transactionID)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.Transaction.StoredProcedures.GetProgramID.Parameters.TRANSACTION_ID, transactionID, System.Data.SqlDbType.Int)
					};

					return
						db.Select(
							Queries.Transaction.StoredProcedures.GetProgramID.NAME,
							parameters
						).Rows[0];
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public System.Data.DataRow SelectByIdentifier(string identifier)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.Transaction.StoredProcedures.SelectByIdentifier.Parameters.IDENTIFIER, identifier, System.Data.SqlDbType.VarChar)
					};

					System.Data.DataTable response = new System.Data.DataTable();

					response =
						db.Select(
							Queries.Transaction.StoredProcedures.SelectByIdentifier.NAME,
							parameters
						);

					return
						response.Rows.Count > 0
							? response.Rows[0]
							: null;
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public System.Data.DataTable SelectByCalibratedTransactionID(int calibratedTransactionID)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.Transaction.StoredProcedures.SelectByCalibratedTransactionID.Parameters.CALIBRATED_TRANSACTION_ID, calibratedTransactionID, System.Data.SqlDbType.Int)
					};

					return
						db.Select(
							Queries.Transaction.StoredProcedures.SelectByCalibratedTransactionID.NAME,
							parameters
						);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public System.Data.DataTable Search(
			int? userIdentificationTypeID,
			string userIdentification,
			int? userStatusID,
			int[] workspaceIDList,
			int? supervisorUserIdentificationTypeID,
			string supervisorUserIdentification,
			int[] programIDList,
			string transactionIdentifier,
			DateTime? transactionDateFrom,
			DateTime? transactionDateTo,
			int? lastCallDateTypeID,
			int? lastCallDate,
			DateTime? evaluationDateFrom,
			DateTime? evaluationDateTo,
			int? lastEvaluationDateTypeID,
			int? lastEvaluationDate,
			string generalObservationText,
			string attributeText,
			string attributeCommentText,
			string labelText,
			string disputedTransactionText,
			string invalidatedTransactionText,
			bool? coachingSent,
			bool? coachingRead,
			int? controllableErrorFilterTypeID,
			string devolutionCommentText,
			string devolutionStrengthCommentText,
			string devolutionImprovementStepsCommentText,
			string customFieldLabelText,
			string customFieldValueText)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.Transaction.StoredProcedures.Search.Parameters.USER_IDENTIFICATION_TYPE, userIdentificationTypeID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.Transaction.StoredProcedures.Search.Parameters.USER_IDENTIFICATION, userIdentification, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.Transaction.StoredProcedures.Search.Parameters.USER_STATUS_ID, userStatusID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.Transaction.StoredProcedures.Search.Parameters.WORKSPACE_ID_LIST, workspaceIDList != null ? String.Join(",", workspaceIDList) : null, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.Transaction.StoredProcedures.Search.Parameters.MONITOR_USER_IDENTIFICATION_TYPE, supervisorUserIdentificationTypeID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.Transaction.StoredProcedures.Search.Parameters.MONITOR_USER_IDENTIFICATION, supervisorUserIdentification, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.Transaction.StoredProcedures.Search.Parameters.PROGRAM_ID_LIST, programIDList != null ? String.Join(",", programIDList) : null, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.Transaction.StoredProcedures.Search.Parameters.TRANSACTION_IDENTIFIER, transactionIdentifier, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.Transaction.StoredProcedures.Search.Parameters.TRANSACTION_DATE_FROM, transactionDateFrom, System.Data.SqlDbType.DateTime),
						db.CreateParameter(Queries.Transaction.StoredProcedures.Search.Parameters.TRANSACTION_DATE_TO, transactionDateTo, System.Data.SqlDbType.DateTime),
						db.CreateParameter(Queries.Transaction.StoredProcedures.Search.Parameters.TRANSACTION_DATE_SINCE_TYPE_ID, lastCallDateTypeID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.Transaction.StoredProcedures.Search.Parameters.TRANSACTION_DATE_SINCE, lastCallDate, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.Transaction.StoredProcedures.Search.Parameters.EVALUATION_DATE_FROM, evaluationDateFrom, System.Data.SqlDbType.DateTime),
						db.CreateParameter(Queries.Transaction.StoredProcedures.Search.Parameters.EVALUATION_DATE_TO, evaluationDateTo, System.Data.SqlDbType.DateTime),
						db.CreateParameter(Queries.Transaction.StoredProcedures.Search.Parameters.EVALUATION_DATE_SINCE_TYPE_ID, lastEvaluationDateTypeID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.Transaction.StoredProcedures.Search.Parameters.EVALUATION_DATE_SINCE, lastEvaluationDate, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.Transaction.StoredProcedures.Search.Parameters.TRANSACTION_COMMENT_TEXT, generalObservationText, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.Transaction.StoredProcedures.Search.Parameters.ATTRIBUTE_NAME_TEXT, attributeText, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.Transaction.StoredProcedures.Search.Parameters.ATTRIBUTE_COMMENT_TEXT, attributeCommentText, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.Transaction.StoredProcedures.Search.Parameters.TRANSACTION_LABEL_TEXT, labelText, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.Transaction.StoredProcedures.Search.Parameters.DISPUTATION_TEXT, disputedTransactionText, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.Transaction.StoredProcedures.Search.Parameters.INVALIDATION_TEXT, invalidatedTransactionText, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.Transaction.StoredProcedures.Search.Parameters.COACHING_SENT, coachingSent, System.Data.SqlDbType.Bit),
						db.CreateParameter(Queries.Transaction.StoredProcedures.Search.Parameters.COACHING_READ, coachingRead, System.Data.SqlDbType.Bit),
						db.CreateParameter(Queries.Transaction.StoredProcedures.Search.Parameters.CONTROLLABLE_ERROR_FILTER_TYPE_ID, controllableErrorFilterTypeID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.Transaction.StoredProcedures.Search.Parameters.DEVOLUTION_COMMENT_TEXT, devolutionCommentText, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.Transaction.StoredProcedures.Search.Parameters.DEVOLUTION_STRENGTHS_COMMENT_TEXT, devolutionStrengthCommentText, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.Transaction.StoredProcedures.Search.Parameters.DEVOLUTION_IMPROVEMENT_STEPS_COMMENT_TEXT, devolutionImprovementStepsCommentText, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.Transaction.StoredProcedures.Search.Parameters.CUSTOM_FIELD_LABEL_TEXT, customFieldLabelText, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.Transaction.StoredProcedures.Search.Parameters.CUSTOM_FIELD_VALUE_TEXT, customFieldValueText, System.Data.SqlDbType.VarChar)
					};

					return
						db.Select(
							Queries.Transaction.StoredProcedures.Search.NAME,
							parameters
						);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public int Update(
			int id, 
			string identifier, 
			int userToEvaluateID, 
			int evaluatorUserID, 
			DateTime evaluationDate, 
			DateTime transactionDate, 
			DateTime loadDate, 
			int formID, 
			string comment, 
			int generalResultID, 
			int generalFinalUserCriticalErrorResultID, 
			int generalBusinessCriticalErrorResultID, 
			int generalFulfillmentCriticalErrorResultID, 
			int generalNonCriticalErrorResult, 
			int accurateResultID, 
			int accurateFinalUserCriticalErrorResultID, 
			int accurateBusinessCriticalErrorResultID, 
			int accurateFulfillmentCriticalErrorResultID, 
			int accurateNonCriticalErrorResult, 
			int controllableResultID, 
			int controllableFinalUserCriticalErrorResultID, 
			int controllableBusinessCriticalErrorResultID, 
			int controllableFulfillmentCriticalErrorResultID, 
			int controllableNonCriticalErrorResult, 
			TimeSpan timeElapsed)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.Transaction.StoredProcedures.Update.Parameters.ID, id, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.Transaction.StoredProcedures.Update.Parameters.IDENTIFIER, identifier, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.Transaction.StoredProcedures.Update.Parameters.USERTOEVALUATEID, userToEvaluateID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.Transaction.StoredProcedures.Update.Parameters.EVALUATORUSERID, evaluatorUserID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.Transaction.StoredProcedures.Update.Parameters.EVALUATIONDATE, evaluationDate, System.Data.SqlDbType.DateTime),
						db.CreateParameter(Queries.Transaction.StoredProcedures.Update.Parameters.TRANSACTIONDATE, transactionDate, System.Data.SqlDbType.DateTime),
						db.CreateParameter(Queries.Transaction.StoredProcedures.Update.Parameters.LOAD_DATE, loadDate, System.Data.SqlDbType.DateTime),
						db.CreateParameter(Queries.Transaction.StoredProcedures.Update.Parameters.FORMID, formID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.Transaction.StoredProcedures.Update.Parameters.COMMENT, comment, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.Transaction.StoredProcedures.Update.Parameters.GENERALRESULTID, generalResultID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.Transaction.StoredProcedures.Update.Parameters.GENERALFINALUSERCRITICALERRORRESULTID, generalFinalUserCriticalErrorResultID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.Transaction.StoredProcedures.Update.Parameters.GENERALBUSINESSCRITICALERRORRESULTID, generalBusinessCriticalErrorResultID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.Transaction.StoredProcedures.Update.Parameters.GENERALFULFILLMENTCRITICALERRORRESULTID, generalFulfillmentCriticalErrorResultID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.Transaction.StoredProcedures.Update.Parameters.GENERALNONCRITICALERRORRESULT, generalNonCriticalErrorResult, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.Transaction.StoredProcedures.Update.Parameters.ACCURATERESULTID, accurateResultID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.Transaction.StoredProcedures.Update.Parameters.ACCURATEFINALUSERCRITICALERRORRESULTID, accurateFinalUserCriticalErrorResultID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.Transaction.StoredProcedures.Update.Parameters.ACCURATEBUSINESSCRITICALERRORRESULTID, accurateBusinessCriticalErrorResultID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.Transaction.StoredProcedures.Update.Parameters.ACCURATEFULFILLMENTCRITICALERRORRESULTID, accurateFulfillmentCriticalErrorResultID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.Transaction.StoredProcedures.Update.Parameters.ACCURATENONCRITICALERRORRESULT, accurateNonCriticalErrorResult, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.Transaction.StoredProcedures.Update.Parameters.CONTROLLABLERESULTID, controllableResultID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.Transaction.StoredProcedures.Update.Parameters.CONTROLLABLEFINALUSERCRITICALERRORRESULTID, controllableFinalUserCriticalErrorResultID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.Transaction.StoredProcedures.Update.Parameters.CONTROLLABLEBUSINESSCRITICALERRORRESULTID, controllableBusinessCriticalErrorResultID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.Transaction.StoredProcedures.Update.Parameters.CONTROLLABLEFULFILLMENTCRITICALERRORRESULTID, controllableFulfillmentCriticalErrorResultID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.Transaction.StoredProcedures.Update.Parameters.CONTROLLABLENONCRITICALERRORRESULT, controllableNonCriticalErrorResult, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.Transaction.StoredProcedures.Update.Parameters.TIMEELAPSED, timeElapsed, System.Data.SqlDbType.Time)
					};

					return
						db.Execute(
							Queries.Transaction.StoredProcedures.Update.NAME,
							parameters
						);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public int CheckIdentifierExistence(string identifier)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.Transaction.StoredProcedures.CheckIdentifierExistence.Parameters.IDENTIFIER, identifier, System.Data.SqlDbType.VarChar)
					};

					return
						(int)db.ReadFirstColumn(
							Queries.Transaction.StoredProcedures.CheckIdentifierExistence.NAME,
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