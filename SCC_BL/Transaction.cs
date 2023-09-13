using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCC_BL
{
	public class Transaction : IDisposable
	{
		public int ID { get; set; }
		public string Identifier { get; set; }
		public int UserToEvaluateID { get; set; }
		public int EvaluatorUserID { get; set; }
		public DateTime EvaluationDate { get; set; }
		public DateTime TransactionDate { get; set; }
		public int FormID { get; set; }
		public string Comment { get; set; }

		public int GeneralResultID { get; set; }
		public int GeneralFinalUserCriticalErrorResultID { get; set; }
		public int GeneralBusinessCriticalErrorResultID { get; set; }
		public int GeneralFulfillmentCriticalErrorResultID { get; set; }
		public int GeneralNonCriticalErrorResult { get; set; }

		public int AccurateResultID { get; set; }
		public int AccurateFinalUserCriticalErrorResultID { get; set; }
		public int AccurateBusinessCriticalErrorResultID { get; set; }
		public int AccurateFulfillmentCriticalErrorResultID { get; set; }
		public int AccurateNonCriticalErrorResult { get; set; }

		public int ControllableResultID { get; set; }
		public int ControllableFinalUserCriticalErrorResultID { get; set; }
		public int ControllableBusinessCriticalErrorResultID { get; set; }
		public int ControllableFulfillmentCriticalErrorResultID { get; set; }
		public int ControllableNonCriticalErrorResult { get; set; }

		public TimeSpan TimeElapsed { get; set; }
		public int BasicInfoID { get; set; }
		public int TypeID { get; set; }
		public int? CalibratedTransactionID { get; set; }
		//----------------------------------------------------
		public BasicInfo BasicInfo { get; set; }
		public List<TransactionCustomFieldCatalog> CustomFieldList { get; set; } = new List<TransactionCustomFieldCatalog>();
		public List<TransactionBIFieldCatalog> BIFieldList { get; set; } = new List<TransactionBIFieldCatalog>();
		public List<TransactionAttributeCatalog> AttributeList { get; set; } = new List<TransactionAttributeCatalog>();
		public List<TransactionLabelCatalog> TransactionLabelCatalogList { get; set; } = new List<TransactionLabelCatalog>();
		public List<TransactionCommentary> DisputeCommentaries { get; set; } = new List<TransactionCommentary>();
		public List<TransactionCommentary> InvalidationCommentaries { get; set; } = new List<TransactionCommentary>();
		public List<TransactionCommentary> DevolutionCommentaries { get; set; } = new List<TransactionCommentary>();
		public List<TransactionCommentary> DevolutionUserStrengths { get; set; } = new List<TransactionCommentary>();
		public List<TransactionCommentary> DevolutionImprovementSteps { get; set; } = new List<TransactionCommentary>();
		public User UserToEvaluate { get; set; } = new User();
		public User EvaluatorUser { get; set; } = new User();
		public Program Program { get; set; } = new Program();
		public Catalog GeneralResult { get; set; } = new Catalog();
		public Catalog GeneralFinalUserCriticalErrorResult { get; set; } = new Catalog();
		public Catalog GeneralBusinessCriticalErrorResult { get; set; } = new Catalog();
		public Catalog GeneralFulfillmentCriticalErrorResult { get; set; } = new Catalog();
		public Catalog AccurateResult { get; set; } = new Catalog();
		public Catalog AccurateFinalUserCriticalErrorResult { get; set; } = new Catalog();
		public Catalog AccurateBusinessCriticalErrorResult { get; set; } = new Catalog();
		public Catalog AccurateFulfillmentCriticalErrorResult { get; set; } = new Catalog();
		public Catalog ControllableResult { get; set; } = new Catalog();
		public Catalog ControllableFinalUserCriticalErrorResult { get; set; } = new Catalog();
		public Catalog ControllableBusinessCriticalErrorResult { get; set; } = new Catalog();
		public Catalog ControllableFulfillmentCriticalErrorResult { get; set; } = new Catalog();
		//----------------------------------------------------
		public List<TransactionLabel> TransactionLabelList { get; set; } = new List<TransactionLabel>();

		public Transaction()
		{
		}

		//With Type Enum
		public Transaction(SCC_BL.DBValues.Catalog.TRANSACTION_TYPE transactionType, int? calibratedTransactionID)
		{
			this.TypeID = (int)transactionType;
			this.CalibratedTransactionID = calibratedTransactionID;

            if (calibratedTransactionID != null)
            {
                using (Transaction transaction = new Transaction(calibratedTransactionID.Value))
                {
					transaction.SetDataByID();

					this.FormID = transaction.FormID;
				}
            }
		}

		//For SelectByID and DeleteByID
		public Transaction(int id)
		{
			this.ID = id;
		}

		//For SelectByIdentifier
		public Transaction(string identifier)
		{
			this.Identifier = identifier;
		}

		//For SelectByCalibratedTransactionID
		public static Transaction TransactionWithCalibratedTransactionID(int calibratedTransactionID)
		{
			Transaction @object = new Transaction();
			@object.CalibratedTransactionID = calibratedTransactionID;
			return @object;
		}

		//For Insert
		public Transaction(string identifier, int userToEvaluateID, int evaluatorUserID, DateTime evaluationDate, DateTime transactionDate, int formID, string comment, int generalResultID, int generalFinalUserCriticalErrorResultID, int generalBusinessCriticalErrorResultID, int generalFulfillmentCriticalErrorResultID, int generalNonCriticalErrorResult, int accurateResultID, int accurateFinalUserCriticalErrorResultID, int accurateBusinessCriticalErrorResultID, int accurateFulfillmentCriticalErrorResultID, int accurateNonCriticalErrorResult, int controllableResultID, int controllableFinalUserCriticalErrorResultID, int controllableBusinessCriticalErrorResultID, int controllableFulfillmentCriticalErrorResultID, int controllableNonCriticalErrorResult, TimeSpan timeElapsed, int creationUserID, int statusID, int typeID, int? calibratedTransactionID)
		{
			this.Identifier = identifier;
			this.UserToEvaluateID = userToEvaluateID;
			this.EvaluatorUserID = evaluatorUserID;
			this.EvaluationDate = evaluationDate;
			this.TransactionDate = transactionDate;
			this.FormID = formID;
			this.Comment = comment;
			this.GeneralResultID = generalResultID;
			this.GeneralFinalUserCriticalErrorResultID = generalFinalUserCriticalErrorResultID;
			this.GeneralBusinessCriticalErrorResultID = generalBusinessCriticalErrorResultID;
			this.GeneralFulfillmentCriticalErrorResultID = generalFulfillmentCriticalErrorResultID;
			this.GeneralNonCriticalErrorResult = generalNonCriticalErrorResult;
			this.AccurateResultID = accurateResultID;
			this.AccurateFinalUserCriticalErrorResultID = accurateFinalUserCriticalErrorResultID;
			this.AccurateBusinessCriticalErrorResultID = accurateBusinessCriticalErrorResultID;
			this.AccurateFulfillmentCriticalErrorResultID = accurateFulfillmentCriticalErrorResultID;
			this.AccurateNonCriticalErrorResult = accurateNonCriticalErrorResult;
			this.ControllableResultID = controllableResultID;
			this.ControllableFinalUserCriticalErrorResultID = controllableFinalUserCriticalErrorResultID;
			this.ControllableBusinessCriticalErrorResultID = controllableBusinessCriticalErrorResultID;
			this.ControllableFulfillmentCriticalErrorResultID = controllableFulfillmentCriticalErrorResultID;
			this.ControllableNonCriticalErrorResult = controllableNonCriticalErrorResult;
			this.TimeElapsed = timeElapsed;
			this.TypeID = typeID;
			this.CalibratedTransactionID = calibratedTransactionID;

			this.BasicInfo = new BasicInfo(creationUserID, statusID);
		}

		//For Update
		public Transaction(int id, string identifier, int userToEvaluateID, int evaluatorUserID, DateTime evaluationDate, DateTime transactionDate, int formID, string comment, int generalResultID, int generalFinalUserCriticalErrorResultID, int generalBusinessCriticalErrorResultID, int generalFulfillmentCriticalErrorResultID, int generalNonCriticalErrorResult, int accurateResultID, int accurateFinalUserCriticalErrorResultID, int accurateBusinessCriticalErrorResultID, int accurateFulfillmentCriticalErrorResultID, int accurateNonCriticalErrorResult, int controllableResultID, int controllableFinalUserCriticalErrorResultID, int controllableBusinessCriticalErrorResultID, int controllableFulfillmentCriticalErrorResultID, int controllableNonCriticalErrorResult, TimeSpan timeElapsed, int basicInfoID, int modificationUserID, int statusID)
		{
			this.ID = id;
			this.Identifier = identifier;
			this.UserToEvaluateID = userToEvaluateID;
			this.EvaluatorUserID = evaluatorUserID;
			this.EvaluationDate = evaluationDate;
			this.TransactionDate = transactionDate;
			this.FormID = formID;
			this.Comment = comment;
			this.GeneralResultID = generalResultID;
			this.GeneralFinalUserCriticalErrorResultID = generalFinalUserCriticalErrorResultID;
			this.GeneralBusinessCriticalErrorResultID = generalBusinessCriticalErrorResultID;
			this.GeneralFulfillmentCriticalErrorResultID = generalFulfillmentCriticalErrorResultID;
			this.GeneralNonCriticalErrorResult = generalNonCriticalErrorResult;
			this.AccurateResultID = accurateResultID;
			this.AccurateFinalUserCriticalErrorResultID = accurateFinalUserCriticalErrorResultID;
			this.AccurateBusinessCriticalErrorResultID = accurateBusinessCriticalErrorResultID;
			this.AccurateFulfillmentCriticalErrorResultID = accurateFulfillmentCriticalErrorResultID;
			this.AccurateNonCriticalErrorResult = accurateNonCriticalErrorResult;
			this.ControllableResultID = controllableResultID;
			this.ControllableFinalUserCriticalErrorResultID = controllableFinalUserCriticalErrorResultID;
			this.ControllableBusinessCriticalErrorResultID = controllableBusinessCriticalErrorResultID;
			this.ControllableFulfillmentCriticalErrorResultID = controllableFulfillmentCriticalErrorResultID;
			this.ControllableNonCriticalErrorResult = controllableNonCriticalErrorResult;
			this.TimeElapsed = timeElapsed;

			this.BasicInfo = new BasicInfo(basicInfoID, modificationUserID, statusID);
		}

		//For SelectByID (RESULT)
		public Transaction(int id, string identifier, int userToEvaluateID, int evaluatorUserID, DateTime evaluationDate, DateTime transactionDate, int formID, string comment, int generalResultID, int generalFinalUserCriticalErrorResultID, int generalBusinessCriticalErrorResultID, int generalFulfillmentCriticalErrorResultID, int generalNonCriticalErrorResult, int accurateResultID, int accurateFinalUserCriticalErrorResultID, int accurateBusinessCriticalErrorResultID, int accurateFulfillmentCriticalErrorResultID, int accurateNonCriticalErrorResult, int controllableResultID, int controllableFinalUserCriticalErrorResultID, int controllableBusinessCriticalErrorResultID, int controllableFulfillmentCriticalErrorResultID, int controllableNonCriticalErrorResult, TimeSpan timeElapsed, int basicInfoID, int typeID, int? calibratedTransactionID)
		{
			this.ID = id;
			this.Identifier = identifier;
			this.UserToEvaluateID = userToEvaluateID;
			this.EvaluatorUserID = evaluatorUserID;
			this.EvaluationDate = evaluationDate;
			this.TransactionDate = transactionDate;
			this.FormID = formID;
			this.Comment = comment;
			this.GeneralResultID = generalResultID;
			this.GeneralFinalUserCriticalErrorResultID = generalFinalUserCriticalErrorResultID;
			this.GeneralBusinessCriticalErrorResultID = generalBusinessCriticalErrorResultID;
			this.GeneralFulfillmentCriticalErrorResultID = generalFulfillmentCriticalErrorResultID;
			this.GeneralNonCriticalErrorResult = generalNonCriticalErrorResult;
			this.AccurateResultID = accurateResultID;
			this.AccurateFinalUserCriticalErrorResultID = accurateFinalUserCriticalErrorResultID;
			this.AccurateBusinessCriticalErrorResultID = accurateBusinessCriticalErrorResultID;
			this.AccurateFulfillmentCriticalErrorResultID = accurateFulfillmentCriticalErrorResultID;
			this.AccurateNonCriticalErrorResult = accurateNonCriticalErrorResult;
			this.ControllableResultID = controllableResultID;
			this.ControllableFinalUserCriticalErrorResultID = controllableFinalUserCriticalErrorResultID;
			this.ControllableBusinessCriticalErrorResultID = controllableBusinessCriticalErrorResultID;
			this.ControllableFulfillmentCriticalErrorResultID = controllableFulfillmentCriticalErrorResultID;
			this.ControllableNonCriticalErrorResult = controllableNonCriticalErrorResult;
			this.TimeElapsed = timeElapsed;
			this.BasicInfoID = basicInfoID;
			this.TypeID = typeID;
			this.CalibratedTransactionID = calibratedTransactionID;
		}

		void SetTransactionLabelList()
        {
			this.TransactionLabelList = new List<TransactionLabel>();

            foreach (TransactionLabelCatalog transactionLabelCatalog in this.TransactionLabelCatalogList)
			{
				using (TransactionLabel transactionLabel = new TransactionLabel(transactionLabelCatalog.LabelID))
				{
					transactionLabel.SetDataByID();
					this.TransactionLabelList.Add(transactionLabel);
				}
			}
		}

		public void SetDataByID(bool simpleData = false)
		{
			using (SCC_DATA.Repositories.Transaction repoTransaction = new SCC_DATA.Repositories.Transaction())
			{
				DataRow dr = repoTransaction.SelectByID(this.ID);

				int? calibratedTransactionID = null;

				try { calibratedTransactionID = Convert.ToInt32(dr[SCC_DATA.Queries.Transaction.StoredProcedures.SelectByID.ResultFields.CALIBRATED_TRANSACTION_ID]); } catch (Exception) { }

				this.ID = Convert.ToInt32(dr[SCC_DATA.Queries.Transaction.StoredProcedures.SelectByID.ResultFields.ID]);
				this.Identifier = Convert.ToString(dr[SCC_DATA.Queries.Transaction.StoredProcedures.SelectByID.ResultFields.IDENTIFIER]);
				this.UserToEvaluateID = Convert.ToInt32(dr[SCC_DATA.Queries.Transaction.StoredProcedures.SelectByID.ResultFields.USERTOEVALUATEID]);
				this.EvaluatorUserID = Convert.ToInt32(dr[SCC_DATA.Queries.Transaction.StoredProcedures.SelectByID.ResultFields.EVALUATORUSERID]);
				this.EvaluationDate = Convert.ToDateTime(dr[SCC_DATA.Queries.Transaction.StoredProcedures.SelectByID.ResultFields.EVALUATIONDATE]);
				this.TransactionDate = Convert.ToDateTime(dr[SCC_DATA.Queries.Transaction.StoredProcedures.SelectByID.ResultFields.TRANSACTIONDATE]);
				this.FormID = Convert.ToInt32(dr[SCC_DATA.Queries.Transaction.StoredProcedures.SelectByID.ResultFields.FORMID]);
				this.Comment = Convert.ToString(dr[SCC_DATA.Queries.Transaction.StoredProcedures.SelectByID.ResultFields.COMMENT]);
				this.GeneralResultID = Convert.ToInt32(dr[SCC_DATA.Queries.Transaction.StoredProcedures.SelectByID.ResultFields.GENERALRESULTID]);
				this.GeneralFinalUserCriticalErrorResultID = Convert.ToInt32(dr[SCC_DATA.Queries.Transaction.StoredProcedures.SelectByID.ResultFields.GENERALFINALUSERCRITICALERRORRESULTID]);
				this.GeneralBusinessCriticalErrorResultID = Convert.ToInt32(dr[SCC_DATA.Queries.Transaction.StoredProcedures.SelectByID.ResultFields.GENERALBUSINESSCRITICALERRORRESULTID]);
				this.GeneralFulfillmentCriticalErrorResultID = Convert.ToInt32(dr[SCC_DATA.Queries.Transaction.StoredProcedures.SelectByID.ResultFields.GENERALFULFILLMENTCRITICALERRORRESULTID]);
				this.GeneralNonCriticalErrorResult = Convert.ToInt32(dr[SCC_DATA.Queries.Transaction.StoredProcedures.SelectByID.ResultFields.GENERALNONCRITICALERRORRESULT]);
				this.AccurateResultID = Convert.ToInt32(dr[SCC_DATA.Queries.Transaction.StoredProcedures.SelectByID.ResultFields.ACCURATERESULTID]);
				this.AccurateFinalUserCriticalErrorResultID = Convert.ToInt32(dr[SCC_DATA.Queries.Transaction.StoredProcedures.SelectByID.ResultFields.ACCURATEFINALUSERCRITICALERRORRESULTID]);
				this.AccurateBusinessCriticalErrorResultID = Convert.ToInt32(dr[SCC_DATA.Queries.Transaction.StoredProcedures.SelectByID.ResultFields.ACCURATEBUSINESSCRITICALERRORRESULTID]);
				this.AccurateFulfillmentCriticalErrorResultID = Convert.ToInt32(dr[SCC_DATA.Queries.Transaction.StoredProcedures.SelectByID.ResultFields.ACCURATEFULFILLMENTCRITICALERRORRESULTID]);
				this.AccurateNonCriticalErrorResult = Convert.ToInt32(dr[SCC_DATA.Queries.Transaction.StoredProcedures.SelectByID.ResultFields.ACCURATENONCRITICALERRORRESULT]);
				this.ControllableResultID = Convert.ToInt32(dr[SCC_DATA.Queries.Transaction.StoredProcedures.SelectByID.ResultFields.CONTROLLABLERESULTID]);
				this.ControllableFinalUserCriticalErrorResultID = Convert.ToInt32(dr[SCC_DATA.Queries.Transaction.StoredProcedures.SelectByID.ResultFields.CONTROLLABLEFINALUSERCRITICALERRORRESULTID]);
				this.ControllableBusinessCriticalErrorResultID = Convert.ToInt32(dr[SCC_DATA.Queries.Transaction.StoredProcedures.SelectByID.ResultFields.CONTROLLABLEBUSINESSCRITICALERRORRESULTID]);
				this.ControllableFulfillmentCriticalErrorResultID = Convert.ToInt32(dr[SCC_DATA.Queries.Transaction.StoredProcedures.SelectByID.ResultFields.CONTROLLABLEFULFILLMENTCRITICALERRORRESULTID]);
				this.ControllableNonCriticalErrorResult = Convert.ToInt32(dr[SCC_DATA.Queries.Transaction.StoredProcedures.SelectByID.ResultFields.CONTROLLABLENONCRITICALERRORRESULT]);
				this.TimeElapsed = (TimeSpan)(dr[SCC_DATA.Queries.Transaction.StoredProcedures.SelectByID.ResultFields.TIMEELAPSED]);
				this.BasicInfoID = Convert.ToInt32(dr[SCC_DATA.Queries.Transaction.StoredProcedures.SelectByID.ResultFields.BASICINFOID]);
				this.TypeID = Convert.ToInt32(dr[SCC_DATA.Queries.Transaction.StoredProcedures.SelectByID.ResultFields.TYPE_ID]);
				this.CalibratedTransactionID = calibratedTransactionID;

				this.BasicInfo = new BasicInfo(this.BasicInfoID);
				this.BasicInfo.SetDataByID();

                if (!simpleData)
                {
                    this.CustomFieldList = TransactionCustomFieldCatalog.TransactionCustomFieldCatalogWithTransactionID(this.ID).SelectByTransactionID();
                    this.BIFieldList = TransactionBIFieldCatalog.TransactionBIFieldCatalogWithTransactionID(this.ID).SelectByTransactionID();
                    this.SetAttributeList();
                    this.TransactionLabelCatalogList = TransactionLabelCatalog.TransactionLabelCatalogWithTransactionID(this.ID).SelectByTransactionID();

                    List<TransactionCommentary> transactionCommentaryList = new List<TransactionCommentary>();

                    using (TransactionCommentary transactionCommentary = TransactionCommentary.TransactionCommentaryWithTransactionID(this.ID))
                        transactionCommentaryList = transactionCommentary.SelectByTransactionID();

                    this.DisputeCommentaries = transactionCommentaryList.Where(e => e.TypeID == (int)SCC_BL.DBValues.Catalog.TRANSACTION_COMMENT_TYPE.DISPUTE).ToList();
                    this.InvalidationCommentaries = transactionCommentaryList.Where(e => e.TypeID == (int)SCC_BL.DBValues.Catalog.TRANSACTION_COMMENT_TYPE.INVALIDATION).ToList();
                    this.DevolutionCommentaries = transactionCommentaryList.Where(e => e.TypeID == (int)SCC_BL.DBValues.Catalog.TRANSACTION_COMMENT_TYPE.DEVOLUTION_COMMENTARIES).ToList();
                    this.DevolutionUserStrengths = transactionCommentaryList.Where(e => e.TypeID == (int)SCC_BL.DBValues.Catalog.TRANSACTION_COMMENT_TYPE.DEVOLUTION_USER_STRENGTHS).ToList();
                    this.DevolutionImprovementSteps = transactionCommentaryList.Where(e => e.TypeID == (int)SCC_BL.DBValues.Catalog.TRANSACTION_COMMENT_TYPE.DEVOLUTION_IMPROVEMENT_STEPS).ToList();

                    this.SetUserToEvaluate();
                    this.SetEvaluatorUser();
                    this.SetProgram();
                    this.SetGeneralResult();
                    this.SetGeneralFinalUserCriticalErrorResult();
                    this.SetGeneralBusinessCriticalErrorResult();
                    this.SetGeneralFulfillmentCriticalErrorResult();
                    this.SetAccurateResult();
                    this.SetAccurateFinalUserCriticalErrorResult();
                    this.SetAccurateBusinessCriticalErrorResult();
                    this.SetAccurateFulfillmentCriticalErrorResult();
                    this.SetControllableResult();
                    this.SetControllableFinalUserCriticalErrorResult();
                    this.SetControllableBusinessCriticalErrorResult();
                    this.SetControllableFulfillmentCriticalErrorResult();

                    this.SetTransactionLabelList();
                }
			}
        }

		public void SetDataByIDForSearch()
		{
			using (SCC_DATA.Repositories.Transaction repoTransaction = new SCC_DATA.Repositories.Transaction())
			{
				DataRow dr = repoTransaction.SelectByID(this.ID);

				int? calibratedTransactionID = null;

				try { calibratedTransactionID = Convert.ToInt32(dr[SCC_DATA.Queries.Transaction.StoredProcedures.SelectByID.ResultFields.CALIBRATED_TRANSACTION_ID]); } catch (Exception) { }

				this.ID = Convert.ToInt32(dr[SCC_DATA.Queries.Transaction.StoredProcedures.SelectByID.ResultFields.ID]);
				this.Identifier = Convert.ToString(dr[SCC_DATA.Queries.Transaction.StoredProcedures.SelectByID.ResultFields.IDENTIFIER]);
				this.UserToEvaluateID = Convert.ToInt32(dr[SCC_DATA.Queries.Transaction.StoredProcedures.SelectByID.ResultFields.USERTOEVALUATEID]);
				this.EvaluatorUserID = Convert.ToInt32(dr[SCC_DATA.Queries.Transaction.StoredProcedures.SelectByID.ResultFields.EVALUATORUSERID]);
				this.EvaluationDate = Convert.ToDateTime(dr[SCC_DATA.Queries.Transaction.StoredProcedures.SelectByID.ResultFields.EVALUATIONDATE]);
				this.TransactionDate = Convert.ToDateTime(dr[SCC_DATA.Queries.Transaction.StoredProcedures.SelectByID.ResultFields.TRANSACTIONDATE]);
				this.FormID = Convert.ToInt32(dr[SCC_DATA.Queries.Transaction.StoredProcedures.SelectByID.ResultFields.FORMID]);
				this.Comment = Convert.ToString(dr[SCC_DATA.Queries.Transaction.StoredProcedures.SelectByID.ResultFields.COMMENT]);
				this.GeneralResultID = Convert.ToInt32(dr[SCC_DATA.Queries.Transaction.StoredProcedures.SelectByID.ResultFields.GENERALRESULTID]);
				this.GeneralFinalUserCriticalErrorResultID = Convert.ToInt32(dr[SCC_DATA.Queries.Transaction.StoredProcedures.SelectByID.ResultFields.GENERALFINALUSERCRITICALERRORRESULTID]);
				this.GeneralBusinessCriticalErrorResultID = Convert.ToInt32(dr[SCC_DATA.Queries.Transaction.StoredProcedures.SelectByID.ResultFields.GENERALBUSINESSCRITICALERRORRESULTID]);
				this.GeneralFulfillmentCriticalErrorResultID = Convert.ToInt32(dr[SCC_DATA.Queries.Transaction.StoredProcedures.SelectByID.ResultFields.GENERALFULFILLMENTCRITICALERRORRESULTID]);
				this.GeneralNonCriticalErrorResult = Convert.ToInt32(dr[SCC_DATA.Queries.Transaction.StoredProcedures.SelectByID.ResultFields.GENERALNONCRITICALERRORRESULT]);
				this.AccurateResultID = Convert.ToInt32(dr[SCC_DATA.Queries.Transaction.StoredProcedures.SelectByID.ResultFields.ACCURATERESULTID]);
				this.AccurateFinalUserCriticalErrorResultID = Convert.ToInt32(dr[SCC_DATA.Queries.Transaction.StoredProcedures.SelectByID.ResultFields.ACCURATEFINALUSERCRITICALERRORRESULTID]);
				this.AccurateBusinessCriticalErrorResultID = Convert.ToInt32(dr[SCC_DATA.Queries.Transaction.StoredProcedures.SelectByID.ResultFields.ACCURATEBUSINESSCRITICALERRORRESULTID]);
				this.AccurateFulfillmentCriticalErrorResultID = Convert.ToInt32(dr[SCC_DATA.Queries.Transaction.StoredProcedures.SelectByID.ResultFields.ACCURATEFULFILLMENTCRITICALERRORRESULTID]);
				this.AccurateNonCriticalErrorResult = Convert.ToInt32(dr[SCC_DATA.Queries.Transaction.StoredProcedures.SelectByID.ResultFields.ACCURATENONCRITICALERRORRESULT]);
				this.ControllableResultID = Convert.ToInt32(dr[SCC_DATA.Queries.Transaction.StoredProcedures.SelectByID.ResultFields.CONTROLLABLERESULTID]);
				this.ControllableFinalUserCriticalErrorResultID = Convert.ToInt32(dr[SCC_DATA.Queries.Transaction.StoredProcedures.SelectByID.ResultFields.CONTROLLABLEFINALUSERCRITICALERRORRESULTID]);
				this.ControllableBusinessCriticalErrorResultID = Convert.ToInt32(dr[SCC_DATA.Queries.Transaction.StoredProcedures.SelectByID.ResultFields.CONTROLLABLEBUSINESSCRITICALERRORRESULTID]);
				this.ControllableFulfillmentCriticalErrorResultID = Convert.ToInt32(dr[SCC_DATA.Queries.Transaction.StoredProcedures.SelectByID.ResultFields.CONTROLLABLEFULFILLMENTCRITICALERRORRESULTID]);
				this.ControllableNonCriticalErrorResult = Convert.ToInt32(dr[SCC_DATA.Queries.Transaction.StoredProcedures.SelectByID.ResultFields.CONTROLLABLENONCRITICALERRORRESULT]);
				this.TimeElapsed = (TimeSpan)(dr[SCC_DATA.Queries.Transaction.StoredProcedures.SelectByID.ResultFields.TIMEELAPSED]);
				this.BasicInfoID = Convert.ToInt32(dr[SCC_DATA.Queries.Transaction.StoredProcedures.SelectByID.ResultFields.BASICINFOID]);
				this.TypeID = Convert.ToInt32(dr[SCC_DATA.Queries.Transaction.StoredProcedures.SelectByID.ResultFields.TYPE_ID]);
				this.CalibratedTransactionID = calibratedTransactionID;

				this.BasicInfo = new BasicInfo(this.BasicInfoID);
				this.BasicInfo.SetDataByID();

                this.SetUserToEvaluate();
                this.SetProgram();

                List<TransactionCommentary> transactionCommentaryList = new List<TransactionCommentary>();

                using (TransactionCommentary transactionCommentary = TransactionCommentary.TransactionCommentaryWithTransactionID(this.ID))
                    transactionCommentaryList = transactionCommentary.SelectByTransactionID();

                this.DisputeCommentaries = transactionCommentaryList.Where(e => e.TypeID == (int)SCC_BL.DBValues.Catalog.TRANSACTION_COMMENT_TYPE.DISPUTE).ToList();
                this.InvalidationCommentaries = transactionCommentaryList.Where(e => e.TypeID == (int)SCC_BL.DBValues.Catalog.TRANSACTION_COMMENT_TYPE.INVALIDATION).ToList();
                this.DevolutionCommentaries = transactionCommentaryList.Where(e => e.TypeID == (int)SCC_BL.DBValues.Catalog.TRANSACTION_COMMENT_TYPE.DEVOLUTION_COMMENTARIES).ToList();
                this.DevolutionUserStrengths = transactionCommentaryList.Where(e => e.TypeID == (int)SCC_BL.DBValues.Catalog.TRANSACTION_COMMENT_TYPE.DEVOLUTION_USER_STRENGTHS).ToList();
                this.DevolutionImprovementSteps = transactionCommentaryList.Where(e => e.TypeID == (int)SCC_BL.DBValues.Catalog.TRANSACTION_COMMENT_TYPE.DEVOLUTION_IMPROVEMENT_STEPS).ToList();
			}
        }

        void SetAttributeList()
        {
            //Arreglar
            /*//this.AttributeList =TransactionAttributeCatalog.TransactionAttributeCatalogWithTransactionID(this.ID).SelectByTransactionID();

            this.AttributeList = new List<TransactionAttributeCatalog>();

            int[] attributeIDArray = new int[0];

            using (TransactionAttributeCatalog transactionAttributeCatalog = TransactionAttributeCatalog.TransactionAttributeCatalogWithTransactionID(this.ID))
                attributeIDArray = transactionAttributeCatalog.SelectAttributeIDArrayByTransactionID();

            // Create a list to store the tasks
            List<Task> tasks = new List<Task>();

            foreach (int e in transactionAttributeIDArray)
            {
                // Start a new task for each iteration
                Task task = Task.Run(async () =>
                {
                    using (TransactionAttributeCatalog transactionAttributeCatalog = new TransactionAttributeCatalog(e))
                    {
                        transactionAttributeCatalog.SetDataByID();

                        if (transactionAttributeCatalog.ID > 0)
                        {
                            lock (this.AttributeList)
                            {
                                this.AttributeList.Add(transactionAttributeCatalog);
                            }
                        }
                    }
                });

                tasks.Add(task);
            }

            // Wait for all the tasks to complete
            Task.WaitAll(tasks.ToArray());*/

            this.AttributeList = TransactionAttributeCatalog.TransactionAttributeCatalogWithTransactionID(this.ID).SelectByTransactionID();
        }

		void SetUserToEvaluate()
		{
			using (User userToEvaluate = new User(this.UserToEvaluateID))
			{
				userToEvaluate.SetDataByID();
				this.UserToEvaluate = userToEvaluate;
			}
		}

		void SetEvaluatorUser()
		{
			using (User evaluatorUser = new User(this.EvaluatorUserID))
			{
                evaluatorUser.SetDataByID();
				this.EvaluatorUser = evaluatorUser;
			}
		}

		void SetProgram()
        {
            int? programID = null;

            using (ProgramFormCatalog programFormCatalog = ProgramFormCatalog.ProgramFormCatalogWithFormID(this.FormID))
            {
                List<ProgramFormCatalog> programFormCatalogList = programFormCatalog.SelectByFormID();
                programID = programFormCatalogList.LastOrDefault().ProgramID;
            }

			if (programID == null) return;

            Program program = new Program(programID.Value);
            program.SetDataByID();

            this.Program = program;
        }

		void SetGeneralResult()
        {
            using (Catalog generalResult = new Catalog(this.GeneralResultID))
            {
                generalResult.SetDataByID();
                this.GeneralResult = generalResult;
            }
        }

		void SetGeneralFinalUserCriticalErrorResult()
        {
            using (Catalog generalFinalUserCriticalErrorResultID = new Catalog(this.GeneralFinalUserCriticalErrorResultID))
            {
                generalFinalUserCriticalErrorResultID.SetDataByID();
                this.GeneralFinalUserCriticalErrorResult = generalFinalUserCriticalErrorResultID;
            }
        }

		void SetGeneralBusinessCriticalErrorResult()
        {
            using (Catalog generalBusinessCriticalErrorResultID = new Catalog(this.GeneralBusinessCriticalErrorResultID))
            {
                generalBusinessCriticalErrorResultID.SetDataByID();
                this.GeneralBusinessCriticalErrorResult = generalBusinessCriticalErrorResultID;
            }
        }

		void SetGeneralFulfillmentCriticalErrorResult()
        {
            using (Catalog generalFulfillmentCriticalErrorResultID = new Catalog(this.GeneralFulfillmentCriticalErrorResultID))
            {
                generalFulfillmentCriticalErrorResultID.SetDataByID();
                this.GeneralFulfillmentCriticalErrorResult = generalFulfillmentCriticalErrorResultID;
            }
        }

		void SetAccurateResult()
        {
            using (Catalog accurateResult = new Catalog(this.AccurateResultID))
            {
                accurateResult.SetDataByID();
                this.AccurateResult = accurateResult;
            }
        }

		void SetAccurateFinalUserCriticalErrorResult()
        {
            using (Catalog accurateFinalUserCriticalErrorResultID = new Catalog(this.AccurateFinalUserCriticalErrorResultID))
            {
                accurateFinalUserCriticalErrorResultID.SetDataByID();
                this.AccurateFinalUserCriticalErrorResult = accurateFinalUserCriticalErrorResultID;
            }
        }

		void SetAccurateBusinessCriticalErrorResult()
        {
            using (Catalog accurateBusinessCriticalErrorResultID = new Catalog(this.AccurateBusinessCriticalErrorResultID))
            {
                accurateBusinessCriticalErrorResultID.SetDataByID();
                this.AccurateBusinessCriticalErrorResult = accurateBusinessCriticalErrorResultID;
            }
        }

		void SetAccurateFulfillmentCriticalErrorResult()
        {
            using (Catalog accurateFulfillmentCriticalErrorResultID = new Catalog(this.AccurateFulfillmentCriticalErrorResultID))
            {
                accurateFulfillmentCriticalErrorResultID.SetDataByID();
                this.AccurateFulfillmentCriticalErrorResult = accurateFulfillmentCriticalErrorResultID;
            }
        }

		void SetControllableResult()
        {
            using (Catalog controllableResult = new Catalog(this.ControllableResultID))
            {
                controllableResult.SetDataByID();
                this.ControllableResult = controllableResult;
            }
        }

		void SetControllableFinalUserCriticalErrorResult()
        {
            using (Catalog controllableFinalUserCriticalErrorResultID = new Catalog(this.ControllableFinalUserCriticalErrorResultID))
            {
                controllableFinalUserCriticalErrorResultID.SetDataByID();
                this.ControllableFinalUserCriticalErrorResult = controllableFinalUserCriticalErrorResultID;
            }
        }

		void SetControllableBusinessCriticalErrorResult()
        {
            using (Catalog controllableBusinessCriticalErrorResultID = new Catalog(this.ControllableBusinessCriticalErrorResultID))
            {
                controllableBusinessCriticalErrorResultID.SetDataByID();
                this.ControllableBusinessCriticalErrorResult = controllableBusinessCriticalErrorResultID;
            }
        }

		void SetControllableFulfillmentCriticalErrorResult()
        {
            using (Catalog controllableFulfillmentCriticalErrorResultID = new Catalog(this.ControllableFulfillmentCriticalErrorResultID))
            {
                controllableFulfillmentCriticalErrorResultID.SetDataByID();
                this.ControllableFulfillmentCriticalErrorResult = controllableFulfillmentCriticalErrorResultID;
            }
        }

		public void SetDataByIdentifier()
		{
			using (SCC_DATA.Repositories.Transaction repoTransaction = new SCC_DATA.Repositories.Transaction())
			{
				DataRow dr = repoTransaction.SelectByIdentifier(this.Identifier);

                if (dr == null)
                {
					this.ID = -1;
					return;
                }

				int? calibratedTransactionID = null;

				try { calibratedTransactionID = Convert.ToInt32(dr[SCC_DATA.Queries.Transaction.StoredProcedures.SelectByIdentifier.ResultFields.CALIBRATED_TRANSACTION_ID]); } catch (Exception) { }

				this.ID = Convert.ToInt32(dr[SCC_DATA.Queries.Transaction.StoredProcedures.SelectByIdentifier.ResultFields.ID]);
				this.Identifier = Convert.ToString(dr[SCC_DATA.Queries.Transaction.StoredProcedures.SelectByIdentifier.ResultFields.IDENTIFIER]);
				this.UserToEvaluateID = Convert.ToInt32(dr[SCC_DATA.Queries.Transaction.StoredProcedures.SelectByIdentifier.ResultFields.USERTOEVALUATEID]);
				this.EvaluatorUserID = Convert.ToInt32(dr[SCC_DATA.Queries.Transaction.StoredProcedures.SelectByIdentifier.ResultFields.EVALUATORUSERID]);
				this.EvaluationDate = Convert.ToDateTime(dr[SCC_DATA.Queries.Transaction.StoredProcedures.SelectByIdentifier.ResultFields.EVALUATIONDATE]);
				this.TransactionDate = Convert.ToDateTime(dr[SCC_DATA.Queries.Transaction.StoredProcedures.SelectByIdentifier.ResultFields.TRANSACTIONDATE]);
				this.FormID = Convert.ToInt32(dr[SCC_DATA.Queries.Transaction.StoredProcedures.SelectByIdentifier.ResultFields.FORMID]);
				this.Comment = Convert.ToString(dr[SCC_DATA.Queries.Transaction.StoredProcedures.SelectByIdentifier.ResultFields.COMMENT]);
				this.GeneralResultID = Convert.ToInt32(dr[SCC_DATA.Queries.Transaction.StoredProcedures.SelectByIdentifier.ResultFields.GENERALRESULTID]);
				this.GeneralFinalUserCriticalErrorResultID = Convert.ToInt32(dr[SCC_DATA.Queries.Transaction.StoredProcedures.SelectByIdentifier.ResultFields.GENERALFINALUSERCRITICALERRORRESULTID]);
				this.GeneralBusinessCriticalErrorResultID = Convert.ToInt32(dr[SCC_DATA.Queries.Transaction.StoredProcedures.SelectByIdentifier.ResultFields.GENERALBUSINESSCRITICALERRORRESULTID]);
				this.GeneralFulfillmentCriticalErrorResultID = Convert.ToInt32(dr[SCC_DATA.Queries.Transaction.StoredProcedures.SelectByIdentifier.ResultFields.GENERALFULFILLMENTCRITICALERRORRESULTID]);
				this.GeneralNonCriticalErrorResult = Convert.ToInt32(dr[SCC_DATA.Queries.Transaction.StoredProcedures.SelectByIdentifier.ResultFields.GENERALNONCRITICALERRORRESULT]);
				this.AccurateResultID = Convert.ToInt32(dr[SCC_DATA.Queries.Transaction.StoredProcedures.SelectByIdentifier.ResultFields.ACCURATERESULTID]);
				this.AccurateFinalUserCriticalErrorResultID = Convert.ToInt32(dr[SCC_DATA.Queries.Transaction.StoredProcedures.SelectByIdentifier.ResultFields.ACCURATEFINALUSERCRITICALERRORRESULTID]);
				this.AccurateBusinessCriticalErrorResultID = Convert.ToInt32(dr[SCC_DATA.Queries.Transaction.StoredProcedures.SelectByIdentifier.ResultFields.ACCURATEBUSINESSCRITICALERRORRESULTID]);
				this.AccurateFulfillmentCriticalErrorResultID = Convert.ToInt32(dr[SCC_DATA.Queries.Transaction.StoredProcedures.SelectByIdentifier.ResultFields.ACCURATEFULFILLMENTCRITICALERRORRESULTID]);
				this.AccurateNonCriticalErrorResult = Convert.ToInt32(dr[SCC_DATA.Queries.Transaction.StoredProcedures.SelectByIdentifier.ResultFields.ACCURATENONCRITICALERRORRESULT]);
				this.ControllableResultID = Convert.ToInt32(dr[SCC_DATA.Queries.Transaction.StoredProcedures.SelectByIdentifier.ResultFields.CONTROLLABLERESULTID]);
				this.ControllableFinalUserCriticalErrorResultID = Convert.ToInt32(dr[SCC_DATA.Queries.Transaction.StoredProcedures.SelectByIdentifier.ResultFields.CONTROLLABLEFINALUSERCRITICALERRORRESULTID]);
				this.ControllableBusinessCriticalErrorResultID = Convert.ToInt32(dr[SCC_DATA.Queries.Transaction.StoredProcedures.SelectByIdentifier.ResultFields.CONTROLLABLEBUSINESSCRITICALERRORRESULTID]);
				this.ControllableFulfillmentCriticalErrorResultID = Convert.ToInt32(dr[SCC_DATA.Queries.Transaction.StoredProcedures.SelectByIdentifier.ResultFields.CONTROLLABLEFULFILLMENTCRITICALERRORRESULTID]);
				this.ControllableNonCriticalErrorResult = Convert.ToInt32(dr[SCC_DATA.Queries.Transaction.StoredProcedures.SelectByIdentifier.ResultFields.CONTROLLABLENONCRITICALERRORRESULT]);
				this.TimeElapsed = (TimeSpan)(dr[SCC_DATA.Queries.Transaction.StoredProcedures.SelectByIdentifier.ResultFields.TIMEELAPSED]);
				this.BasicInfoID = Convert.ToInt32(dr[SCC_DATA.Queries.Transaction.StoredProcedures.SelectByIdentifier.ResultFields.BASICINFOID]);
				this.TypeID = Convert.ToInt32(dr[SCC_DATA.Queries.Transaction.StoredProcedures.SelectByIdentifier.ResultFields.TYPE_ID]);
				this.CalibratedTransactionID = calibratedTransactionID;

				this.BasicInfo = new BasicInfo(this.BasicInfoID);
				this.BasicInfo.SetDataByID();

				this.CustomFieldList = TransactionCustomFieldCatalog.TransactionCustomFieldCatalogWithTransactionID(this.ID).SelectByTransactionID();
				this.BIFieldList = TransactionBIFieldCatalog.TransactionBIFieldCatalogWithTransactionID(this.ID).SelectByTransactionID();
                this.SetAttributeList();
                this.TransactionLabelCatalogList = TransactionLabelCatalog.TransactionLabelCatalogWithTransactionID(this.ID).SelectByTransactionID();

				List<TransactionCommentary> transactionCommentaryList = new List<TransactionCommentary>();

				using (TransactionCommentary transactionCommentary = TransactionCommentary.TransactionCommentaryWithTransactionID(this.ID))
					transactionCommentaryList = transactionCommentary.SelectByTransactionID();

				this.DisputeCommentaries = transactionCommentaryList.Where(e => e.TypeID == (int)SCC_BL.DBValues.Catalog.TRANSACTION_COMMENT_TYPE.DISPUTE).ToList();
				this.InvalidationCommentaries = transactionCommentaryList.Where(e => e.TypeID == (int)SCC_BL.DBValues.Catalog.TRANSACTION_COMMENT_TYPE.INVALIDATION).ToList();
				this.DevolutionCommentaries = transactionCommentaryList.Where(e => e.TypeID == (int)SCC_BL.DBValues.Catalog.TRANSACTION_COMMENT_TYPE.DEVOLUTION_COMMENTARIES).ToList();
				this.DevolutionUserStrengths = transactionCommentaryList.Where(e => e.TypeID == (int)SCC_BL.DBValues.Catalog.TRANSACTION_COMMENT_TYPE.DEVOLUTION_USER_STRENGTHS).ToList();
				this.DevolutionImprovementSteps = transactionCommentaryList.Where(e => e.TypeID == (int)SCC_BL.DBValues.Catalog.TRANSACTION_COMMENT_TYPE.DEVOLUTION_IMPROVEMENT_STEPS).ToList();

                this.SetUserToEvaluate();
                this.SetEvaluatorUser();
                this.SetProgram();
				this.SetGeneralResult();
				this.SetGeneralFinalUserCriticalErrorResult();
				this.SetGeneralBusinessCriticalErrorResult();
				this.SetGeneralFulfillmentCriticalErrorResult();
				this.SetAccurateResult();
				this.SetAccurateFinalUserCriticalErrorResult();
				this.SetAccurateBusinessCriticalErrorResult();
				this.SetAccurateFulfillmentCriticalErrorResult();
				this.SetControllableResult();
                this.SetControllableFinalUserCriticalErrorResult();
                this.SetControllableBusinessCriticalErrorResult();
                this.SetControllableFulfillmentCriticalErrorResult();

                this.SetTransactionLabelList();
			}
		}

		public List<Transaction> SelectByCalibratedTransactionID(bool simpleData = false)
		{
			List<Transaction> transactionList = new List<Transaction>();

			if (this.CalibratedTransactionID == null)
				return transactionList;

			using (SCC_DATA.Repositories.Transaction repoTransaction = new SCC_DATA.Repositories.Transaction())
			{
				DataTable dt = repoTransaction.SelectByCalibratedTransactionID(this.CalibratedTransactionID.Value);

				foreach (DataRow dr in dt.Rows)
				{
					int? calibratedTransactionID = null;

					try { calibratedTransactionID = Convert.ToInt32(dr[SCC_DATA.Queries.Transaction.StoredProcedures.SelectByCalibratedTransactionID.ResultFields.CALIBRATED_TRANSACTION_ID]); } catch (Exception) { }

					Transaction transaction = new Transaction(
						Convert.ToInt32(dr[SCC_DATA.Queries.Transaction.StoredProcedures.SelectByCalibratedTransactionID.ResultFields.ID]),
						Convert.ToString(dr[SCC_DATA.Queries.Transaction.StoredProcedures.SelectByCalibratedTransactionID.ResultFields.IDENTIFIER]),
						Convert.ToInt32(dr[SCC_DATA.Queries.Transaction.StoredProcedures.SelectByCalibratedTransactionID.ResultFields.USERTOEVALUATEID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.Transaction.StoredProcedures.SelectByCalibratedTransactionID.ResultFields.EVALUATORUSERID]),
						Convert.ToDateTime(dr[SCC_DATA.Queries.Transaction.StoredProcedures.SelectByCalibratedTransactionID.ResultFields.EVALUATIONDATE]),
						Convert.ToDateTime(dr[SCC_DATA.Queries.Transaction.StoredProcedures.SelectByCalibratedTransactionID.ResultFields.TRANSACTIONDATE]),
						Convert.ToInt32(dr[SCC_DATA.Queries.Transaction.StoredProcedures.SelectByCalibratedTransactionID.ResultFields.FORMID]),
						Convert.ToString(dr[SCC_DATA.Queries.Transaction.StoredProcedures.SelectByCalibratedTransactionID.ResultFields.COMMENT]),
						Convert.ToInt32(dr[SCC_DATA.Queries.Transaction.StoredProcedures.SelectByCalibratedTransactionID.ResultFields.GENERALRESULTID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.Transaction.StoredProcedures.SelectByCalibratedTransactionID.ResultFields.GENERALFINALUSERCRITICALERRORRESULTID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.Transaction.StoredProcedures.SelectByCalibratedTransactionID.ResultFields.GENERALBUSINESSCRITICALERRORRESULTID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.Transaction.StoredProcedures.SelectByCalibratedTransactionID.ResultFields.GENERALFULFILLMENTCRITICALERRORRESULTID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.Transaction.StoredProcedures.SelectByCalibratedTransactionID.ResultFields.GENERALNONCRITICALERRORRESULT]),
						Convert.ToInt32(dr[SCC_DATA.Queries.Transaction.StoredProcedures.SelectByCalibratedTransactionID.ResultFields.ACCURATERESULTID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.Transaction.StoredProcedures.SelectByCalibratedTransactionID.ResultFields.ACCURATEFINALUSERCRITICALERRORRESULTID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.Transaction.StoredProcedures.SelectByCalibratedTransactionID.ResultFields.ACCURATEBUSINESSCRITICALERRORRESULTID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.Transaction.StoredProcedures.SelectByCalibratedTransactionID.ResultFields.ACCURATEFULFILLMENTCRITICALERRORRESULTID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.Transaction.StoredProcedures.SelectByCalibratedTransactionID.ResultFields.ACCURATENONCRITICALERRORRESULT]),
						Convert.ToInt32(dr[SCC_DATA.Queries.Transaction.StoredProcedures.SelectByCalibratedTransactionID.ResultFields.CONTROLLABLERESULTID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.Transaction.StoredProcedures.SelectByCalibratedTransactionID.ResultFields.CONTROLLABLEFINALUSERCRITICALERRORRESULTID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.Transaction.StoredProcedures.SelectByCalibratedTransactionID.ResultFields.CONTROLLABLEBUSINESSCRITICALERRORRESULTID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.Transaction.StoredProcedures.SelectByCalibratedTransactionID.ResultFields.CONTROLLABLEFULFILLMENTCRITICALERRORRESULTID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.Transaction.StoredProcedures.SelectByCalibratedTransactionID.ResultFields.CONTROLLABLENONCRITICALERRORRESULT]),
						(TimeSpan)(dr[SCC_DATA.Queries.Transaction.StoredProcedures.SelectByCalibratedTransactionID.ResultFields.TIMEELAPSED]),
						Convert.ToInt32(dr[SCC_DATA.Queries.Transaction.StoredProcedures.SelectByCalibratedTransactionID.ResultFields.BASICINFOID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.Transaction.StoredProcedures.SelectByCalibratedTransactionID.ResultFields.TYPE_ID]),
						calibratedTransactionID);

					transaction.BasicInfo = new BasicInfo(transaction.BasicInfoID);
					transaction.BasicInfo.SetDataByID();

					if (!simpleData)
                    {
                        transaction.CustomFieldList = TransactionCustomFieldCatalog.TransactionCustomFieldCatalogWithTransactionID(transaction.ID).SelectByTransactionID();
                        transaction.BIFieldList = TransactionBIFieldCatalog.TransactionBIFieldCatalogWithTransactionID(transaction.ID).SelectByTransactionID();
                        transaction.SetAttributeList();
                        transaction.TransactionLabelCatalogList = TransactionLabelCatalog.TransactionLabelCatalogWithTransactionID(transaction.ID).SelectByTransactionID();

                        List<TransactionCommentary> transactionCommentaryList = new List<TransactionCommentary>();

                        using (TransactionCommentary transactionCommentary = TransactionCommentary.TransactionCommentaryWithTransactionID(transaction.ID))
                            transactionCommentaryList = transactionCommentary.SelectByTransactionID();

                        transaction.DisputeCommentaries = transactionCommentaryList.Where(e => e.TypeID == (int)SCC_BL.DBValues.Catalog.TRANSACTION_COMMENT_TYPE.DISPUTE).ToList();
                        transaction.InvalidationCommentaries = transactionCommentaryList.Where(e => e.TypeID == (int)SCC_BL.DBValues.Catalog.TRANSACTION_COMMENT_TYPE.INVALIDATION).ToList();
                        transaction.DevolutionCommentaries = transactionCommentaryList.Where(e => e.TypeID == (int)SCC_BL.DBValues.Catalog.TRANSACTION_COMMENT_TYPE.DEVOLUTION_COMMENTARIES).ToList();
                        transaction.DevolutionUserStrengths = transactionCommentaryList.Where(e => e.TypeID == (int)SCC_BL.DBValues.Catalog.TRANSACTION_COMMENT_TYPE.DEVOLUTION_USER_STRENGTHS).ToList();
                        transaction.DevolutionImprovementSteps = transactionCommentaryList.Where(e => e.TypeID == (int)SCC_BL.DBValues.Catalog.TRANSACTION_COMMENT_TYPE.DEVOLUTION_IMPROVEMENT_STEPS).ToList();

                        transaction.SetUserToEvaluate();
                        transaction.SetEvaluatorUser();
                        transaction.SetProgram();
						transaction.SetGeneralResult();
						transaction.SetGeneralFinalUserCriticalErrorResult();
						transaction.SetGeneralBusinessCriticalErrorResult();
						transaction.SetGeneralFulfillmentCriticalErrorResult();
						transaction.SetAccurateResult();
						transaction.SetAccurateFinalUserCriticalErrorResult();
						transaction.SetAccurateBusinessCriticalErrorResult();
						transaction.SetAccurateFulfillmentCriticalErrorResult();
						transaction.SetControllableResult();
                        transaction.SetControllableFinalUserCriticalErrorResult();
                        transaction.SetControllableBusinessCriticalErrorResult();
                        transaction.SetControllableFulfillmentCriticalErrorResult();

                        transaction.SetTransactionLabelList();
                    }

					transactionList.Add(transaction);
				}
			}

			return transactionList;
		}

		public List<Transaction> Search(SCC_BL.Helpers.Transaction.Search.TransactionSearchHelper transactionSearchHelper)
		{
			List<Transaction> transactionList = new List<Transaction>();

			using (SCC_DATA.Repositories.Transaction repoTransaction = new SCC_DATA.Repositories.Transaction())
			{
				DataTable dt = repoTransaction.Search(
					transactionSearchHelper.UserIdentificationTypeID,
					transactionSearchHelper.UserIdentification,
					transactionSearchHelper.UserStatusID,
					transactionSearchHelper.WorkspaceIDList,
					transactionSearchHelper.MonitorUserIdentificationTypeID,
					transactionSearchHelper.MonitorUserIdentification,
					transactionSearchHelper.ProgramIDList,
					transactionSearchHelper.TransactionIdentifier,
					transactionSearchHelper.TransactionDateFrom,
					transactionSearchHelper.TransactionDateTo,
					transactionSearchHelper.TransactionDateSinceTypeID,
					transactionSearchHelper.TransactionDateSince,
					transactionSearchHelper.EvaluationDateFrom,
					transactionSearchHelper.EvaluationDateTo,
					transactionSearchHelper.EvaluationDateSinceTypeID,
					transactionSearchHelper.EvaluationDateSince,
					transactionSearchHelper.TransactionCommentText,
					transactionSearchHelper.AttributeNameText,
					transactionSearchHelper.AttributeCommentText,
					transactionSearchHelper.TransactionLabelText,
					transactionSearchHelper.DisputationText,
					transactionSearchHelper.InvalidationText,
					transactionSearchHelper.CoachingSent,
					transactionSearchHelper.CoachingRead,
					transactionSearchHelper.ControllableErrorFilterTypeID,
					transactionSearchHelper.DevolutionCommentText,
					transactionSearchHelper.DevolutionStrengthCommentText,
					transactionSearchHelper.DevolutionImprovementStepsCommentText,
					transactionSearchHelper.CustomFieldLabelText,
					transactionSearchHelper.CustomFieldValueText);

                /*Parallel.ForEach<System.Data.DataRow>(dt.Rows.Cast<System.Data.DataRow>(), (row) => {
                    Transaction transaction = new Transaction(Convert.ToInt32(row[SCC_DATA.Queries.Transaction.StoredProcedures.Search.ResultFields.TRANSACTIONID]));
                    transaction.SetDataByID();

                    transactionList.Add(transaction);
                });*/

                foreach (DataRow dr in dt.Rows)
                {
					Transaction transaction = new Transaction(Convert.ToInt32(dr[SCC_DATA.Queries.Transaction.StoredProcedures.Search.ResultFields.TRANSACTIONID]));
					transaction.SetDataByIDForSearch();

					transactionList.Add(transaction);
				}
            }

            return transactionList;
		}

		public int DeleteByID()
		{
			using (SCC_DATA.Repositories.Transaction repoTransaction = new SCC_DATA.Repositories.Transaction())
			{
				int response = repoTransaction.DeleteByID(this.ID);

				this.BasicInfo.DeleteByID();
				return response;
			}
		}

		public int Insert()
		{
			this.BasicInfoID = this.BasicInfo.Insert();

			using (SCC_DATA.Repositories.Transaction repoTransaction = new SCC_DATA.Repositories.Transaction())
			{
				this.ID = repoTransaction.Insert(this.Identifier, this.UserToEvaluateID, this.EvaluatorUserID, this.EvaluationDate, this.TransactionDate, this.FormID, this.Comment, this.GeneralResultID, this.GeneralFinalUserCriticalErrorResultID, this.GeneralBusinessCriticalErrorResultID, this.GeneralFulfillmentCriticalErrorResultID, this.GeneralNonCriticalErrorResult, this.AccurateResultID, this.AccurateFinalUserCriticalErrorResultID, this.AccurateBusinessCriticalErrorResultID, this.AccurateFulfillmentCriticalErrorResultID, this.AccurateNonCriticalErrorResult, this.ControllableResultID, this.ControllableFinalUserCriticalErrorResultID, this.ControllableBusinessCriticalErrorResultID, this.ControllableFulfillmentCriticalErrorResultID, this.ControllableNonCriticalErrorResult, this.TimeElapsed, this.BasicInfoID, this.TypeID, this.CalibratedTransactionID);

				return this.ID;
			}
		}

		public int Update()
		{
			this.BasicInfo.Update();

			using (SCC_DATA.Repositories.Transaction repoTransaction = new SCC_DATA.Repositories.Transaction())
			{
				return repoTransaction.Update(this.ID, this.Identifier, this.UserToEvaluateID, this.EvaluatorUserID, this.EvaluationDate, this.TransactionDate, this.FormID, this.Comment, this.GeneralResultID, this.GeneralFinalUserCriticalErrorResultID, this.GeneralBusinessCriticalErrorResultID, this.GeneralFulfillmentCriticalErrorResultID, this.GeneralNonCriticalErrorResult, this.AccurateResultID, this.AccurateFinalUserCriticalErrorResultID, this.AccurateBusinessCriticalErrorResultID, this.AccurateFulfillmentCriticalErrorResultID, this.AccurateNonCriticalErrorResult, this.ControllableResultID, this.ControllableFinalUserCriticalErrorResultID, this.ControllableBusinessCriticalErrorResultID, this.ControllableFulfillmentCriticalErrorResultID, this.ControllableNonCriticalErrorResult, this.TimeElapsed);
			}
		}

		public int CheckIdentifierExistence(string identifier)
		{
			using (SCC_DATA.Repositories.Transaction repoTransaction = new SCC_DATA.Repositories.Transaction())
			{
				return repoTransaction.CheckIdentifierExistence(identifier);
			}
		}

		public void SetIdentifier()
		{
			Guid guid = Guid.NewGuid();
			string identifier = guid.ToString();

			while (CheckIdentifierExistence(identifier) > 0)
            {
				guid = Guid.NewGuid();
				identifier = guid.ToString();
			}

			this.Identifier = identifier;
		}

		public Results.Transaction.UpdateAttributeList.CODE UpdateAttributeList(List<TransactionAttributeCatalog> transactionAttributeCatalogList, int creationUserID)
		{
			try
			{
				if (transactionAttributeCatalogList == null) transactionAttributeCatalogList = new List<TransactionAttributeCatalog>();

				//Delete old ones
				this.AttributeList
					.ForEach(e => {
						if (!transactionAttributeCatalogList
							.Where(w =>
								w.ID != null &&
								w.ID > 0)
							.Select(s => s.ID)
							.Contains(e.ID))
							e.DeleteByID();
					});

				//Update existing ones
				foreach (TransactionAttributeCatalog transactionAttributeCatalog in transactionAttributeCatalogList)
				{
					if (this.AttributeList.Select(e => e.ID).Contains(transactionAttributeCatalog.ID))
					{
						int currentBasicInfoID = 0;

						using (TransactionAttributeCatalog auxTransactionAttributeCatalog = new TransactionAttributeCatalog(transactionAttributeCatalog.ID))
						{
							auxTransactionAttributeCatalog.SetDataByID();

							if (auxTransactionAttributeCatalog.ID <= 0) continue;

							currentBasicInfoID = auxTransactionAttributeCatalog.BasicInfoID;
						}

						TransactionAttributeCatalog newTransactionAttributeCatalog = new TransactionAttributeCatalog(
							transactionAttributeCatalog.ID,
							this.ID,
							transactionAttributeCatalog.AttributeID,
							transactionAttributeCatalog.Comment,
							transactionAttributeCatalog.ValueID,
							transactionAttributeCatalog.ScoreValue,
							transactionAttributeCatalog.Checked,
							currentBasicInfoID,
							creationUserID,
							(int)SCC_BL.DBValues.Catalog.STATUS_TRANSACTION_ATTRIBUTE_CATALOG.UPDATED);

						int result = newTransactionAttributeCatalog.Update();
					}
				}

				//Create new ones
				foreach (TransactionAttributeCatalog transactionAttributeCatalog in transactionAttributeCatalogList)
				{
					if (!this.AttributeList.Select(e => e.ID).Contains(transactionAttributeCatalog.ID))
					{
                        if (transactionAttributeCatalog.ValueID != null || transactionAttributeCatalog.ScoreValue >= 0 || transactionAttributeCatalog.Checked)
						{
							TransactionAttributeCatalog newTransactionAttributeCatalog = new TransactionAttributeCatalog(
								this.ID,
								transactionAttributeCatalog.AttributeID,
								transactionAttributeCatalog.Comment ?? string.Empty,
								transactionAttributeCatalog.ValueID,
								transactionAttributeCatalog.ScoreValue,
								transactionAttributeCatalog.Checked,
								creationUserID,
								(int)SCC_BL.DBValues.Catalog.STATUS_TRANSACTION_ATTRIBUTE_CATALOG.CREATED);

							int result = newTransactionAttributeCatalog.Insert();
						}
					}
				}

				return Results.Transaction.UpdateAttributeList.CODE.SUCCESS;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public Results.Transaction.UpdateCustomFieldList.CODE UpdateCustomFieldList(List<TransactionCustomFieldCatalog> transactionCustomFieldCatalogList, int creationUserID)
		{
			try
			{
				if (transactionCustomFieldCatalogList == null) transactionCustomFieldCatalogList = new List<TransactionCustomFieldCatalog>();

				//Delete old ones
				this.CustomFieldList
					.ForEach(e => {
						if (!transactionCustomFieldCatalogList
							.Where(w =>
								w.ID != null &&
								w.ID > 0)
							.Select(s => s.ID)
							.Contains(e.ID))
							e.DeleteByID();
					});

				//Update existing ones
				foreach (TransactionCustomFieldCatalog transactionCustomFieldCatalog in transactionCustomFieldCatalogList)
				{
					if (this.CustomFieldList.Select(e => e.ID).Contains(transactionCustomFieldCatalog.ID))
					{
						int currentBasicInfoID = 0;

						using (TransactionCustomFieldCatalog auxTransactionCustomFieldCatalog = new TransactionCustomFieldCatalog(transactionCustomFieldCatalog.ID))
						{
							auxTransactionCustomFieldCatalog.SetDataByID();
							currentBasicInfoID = auxTransactionCustomFieldCatalog.BasicInfoID;
						}

                        TransactionCustomFieldCatalog newTransactionCustomFieldCatalog = new TransactionCustomFieldCatalog(
							transactionCustomFieldCatalog.ID,
							this.ID,
							transactionCustomFieldCatalog.CustomFieldID,
							transactionCustomFieldCatalog.Comment,
                                transactionCustomFieldCatalog.ValueID == 0
                                    ? null
                                    : transactionCustomFieldCatalog.ValueID,
                            currentBasicInfoID,
							creationUserID,
							(int)SCC_BL.DBValues.Catalog.STATUS_TRANSACTION_CUSTOM_FIELD_CATALOG.UPDATED);

						int result = newTransactionCustomFieldCatalog.Update();
					}
				}

                //Create new ones
                foreach (TransactionCustomFieldCatalog transactionCustomFieldCatalog in transactionCustomFieldCatalogList)
				{
					if (!this.CustomFieldList.Select(e => e.ID).Contains(transactionCustomFieldCatalog.ID))
					{
                        if (transactionCustomFieldCatalog.ValueID != null && transactionCustomFieldCatalog.CustomFieldID > 0)
						{
                            TransactionCustomFieldCatalog newTransactionCustomFieldCatalog = new TransactionCustomFieldCatalog(
								this.ID,
								transactionCustomFieldCatalog.CustomFieldID,
								transactionCustomFieldCatalog.Comment,
								transactionCustomFieldCatalog.ValueID == 0
									? null
									: transactionCustomFieldCatalog.ValueID,
								creationUserID,
								(int)SCC_BL.DBValues.Catalog.STATUS_TRANSACTION_CUSTOM_FIELD_CATALOG.CREATED);

							int result = newTransactionCustomFieldCatalog.Insert();
						}
					}
				}

				return Results.Transaction.UpdateCustomFieldList.CODE.SUCCESS;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public Results.Transaction.UpdateBIFieldList.CODE UpdateBIFieldList(List<TransactionBIFieldCatalog> transactionBIFieldCatalogList, int creationUserID)
		{
			try
			{
				if (transactionBIFieldCatalogList == null) transactionBIFieldCatalogList = new List<TransactionBIFieldCatalog>();

				//Delete old ones
				this.BIFieldList
					.ForEach(e => {
						if (!transactionBIFieldCatalogList
							.Where(w =>
								w.ID != null &&
								w.ID > 0)
							.Select(s => s.ID)
							.Contains(e.ID))
							e.DeleteByID();
					});

				//Update existing ones
				foreach (TransactionBIFieldCatalog transactionBIFieldCatalog in transactionBIFieldCatalogList)
				{
					if (this.BIFieldList.Select(e => e.ID).Contains(transactionBIFieldCatalog.ID))
					{
						int currentBasicInfoID = 0;

						using (TransactionBIFieldCatalog auxTransactionBIFieldCatalog = new TransactionBIFieldCatalog(transactionBIFieldCatalog.ID))
						{
							auxTransactionBIFieldCatalog.SetDataByID();
							currentBasicInfoID = auxTransactionBIFieldCatalog.BasicInfoID;
						}

                        TransactionBIFieldCatalog newTransactionBIFieldCatalog = new TransactionBIFieldCatalog(
							transactionBIFieldCatalog.ID,
							this.ID,
							transactionBIFieldCatalog.BIFieldID,
							transactionBIFieldCatalog.Comment,
							transactionBIFieldCatalog.Checked,
							currentBasicInfoID,
							creationUserID,
							(int)SCC_BL.DBValues.Catalog.STATUS_TRANSACTION_BI_FIELD_CATALOG.UPDATED);

						int result = newTransactionBIFieldCatalog.Update();
					}
				}

				//Create new ones
				foreach (TransactionBIFieldCatalog transactionBIFieldCatalog in transactionBIFieldCatalogList)
				{
					if (!this.BIFieldList.Select(e => e.ID).Contains(transactionBIFieldCatalog.ID))
					{
                        if (transactionBIFieldCatalog.BIFieldID != null || transactionBIFieldCatalog.Checked != null)
						{
                            TransactionBIFieldCatalog newTransactionBIFieldCatalog = new TransactionBIFieldCatalog(
                                this.ID,
                                transactionBIFieldCatalog.BIFieldID,
								transactionBIFieldCatalog.Comment,
								transactionBIFieldCatalog.Checked,
								creationUserID,
								(int)SCC_BL.DBValues.Catalog.STATUS_TRANSACTION_BI_FIELD_CATALOG.CREATED);

							int result = newTransactionBIFieldCatalog.Insert();
						}
					}
				}

				return Results.Transaction.UpdateBIFieldList.CODE.SUCCESS;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public Results.Transaction.UpdateDisputeCommentList.CODE UpdateDisputeCommentList(List<TransactionCommentary> transactionDisputeCommentList, int creationUserID)
		{
			try
			{
				if (transactionDisputeCommentList == null) transactionDisputeCommentList = new List<TransactionCommentary>();

				//Delete old ones
				this.DisputeCommentaries
					.ForEach(e => {
						if (!transactionDisputeCommentList
							.Where(w =>
								w.ID != null &&
								w.ID > 0)
							.Select(s => s.ID)
							.Contains(e.ID))
							e.DeleteByID();
					});

				//Update existing ones
				foreach (TransactionCommentary transactionDisputeComment in transactionDisputeCommentList)
				{
					if (this.DisputeCommentaries.Select(e => e.ID).Contains(transactionDisputeComment.ID))
					{
						int currentBasicInfoID = 0;

						using (TransactionCommentary auxTransactionDisputeComment = new TransactionCommentary(transactionDisputeComment.ID))
						{
							auxTransactionDisputeComment.SetDataByID();
							currentBasicInfoID = auxTransactionDisputeComment.BasicInfoID;
						}

						TransactionCommentary newTransactionDisputeComment = new TransactionCommentary(
							transactionDisputeComment.ID,
							transactionDisputeComment.TypeID,
							transactionDisputeComment.TransactionID,
							transactionDisputeComment.Comment,
							currentBasicInfoID,
							creationUserID,
							(int)SCC_BL.DBValues.Catalog.STATUS_TRANSACTION_COMMENTARY.UPDATED);

						int result = newTransactionDisputeComment.Update();
					}
				}

				//Create new ones
				foreach (TransactionCommentary transactionDisputeComment in transactionDisputeCommentList)
				{
					if (!this.DisputeCommentaries.Select(e => e.ID).Contains(transactionDisputeComment.ID))
					{
                        if (!string.IsNullOrEmpty(transactionDisputeComment.Comment))
						{
							TransactionCommentary newTransactionDisputeComment = new TransactionCommentary(
								transactionDisputeComment.TypeID,
								transactionDisputeComment.TransactionID,
								transactionDisputeComment.Comment ?? string.Empty,
								creationUserID,
								(int)SCC_BL.DBValues.Catalog.STATUS_TRANSACTION_COMMENTARY.CREATED);

							int result = newTransactionDisputeComment.Insert();
						}
					}
				}

				return Results.Transaction.UpdateDisputeCommentList.CODE.SUCCESS;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public Results.Transaction.UpdateInvalidationCommentList.CODE UpdateInvalidationCommentList(List<TransactionCommentary> transactionInvalidationCommentList, int creationUserID)
		{
			try
			{
				if (transactionInvalidationCommentList == null) transactionInvalidationCommentList = new List<TransactionCommentary>();

				//Delete old ones
				this.InvalidationCommentaries
					.ForEach(e => {
						if (!transactionInvalidationCommentList
							.Where(w =>
								w.ID != null &&
								w.ID > 0)
							.Select(s => s.ID)
							.Contains(e.ID))
							e.DeleteByID();
					});

				//Update existing ones
				foreach (TransactionCommentary transactionInvalidationComment in transactionInvalidationCommentList)
				{
					if (this.InvalidationCommentaries.Select(e => e.ID).Contains(transactionInvalidationComment.ID))
					{
						int currentBasicInfoID = 0;

						using (TransactionCommentary auxTransactionInvalidationComment = new TransactionCommentary(transactionInvalidationComment.ID))
						{
							auxTransactionInvalidationComment.SetDataByID();
							currentBasicInfoID = auxTransactionInvalidationComment.BasicInfoID;
						}

						TransactionCommentary newTransactionInvalidationComment = new TransactionCommentary(
							transactionInvalidationComment.ID,
							transactionInvalidationComment.TypeID,
							transactionInvalidationComment.TransactionID,
							transactionInvalidationComment.Comment,
							currentBasicInfoID,
							creationUserID,
							(int)SCC_BL.DBValues.Catalog.STATUS_TRANSACTION_COMMENTARY.UPDATED);

						int result = newTransactionInvalidationComment.Update();
					}
				}

				//Create new ones
				foreach (TransactionCommentary transactionInvalidationComment in transactionInvalidationCommentList)
				{
					if (!this.InvalidationCommentaries.Select(e => e.ID).Contains(transactionInvalidationComment.ID))
					{
                        if (!string.IsNullOrEmpty(transactionInvalidationComment.Comment))
						{
							TransactionCommentary newTransactionInvalidationComment = new TransactionCommentary(
								transactionInvalidationComment.TypeID,
								transactionInvalidationComment.TransactionID,
								transactionInvalidationComment.Comment ?? string.Empty,
								creationUserID,
								(int)SCC_BL.DBValues.Catalog.STATUS_TRANSACTION_COMMENTARY.CREATED);

							int result = newTransactionInvalidationComment.Insert();
						}
					}
				}

				return Results.Transaction.UpdateInvalidationCommentList.CODE.SUCCESS;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public Results.Transaction.UpdateDevolutionCommentList.CODE UpdateDevolutionCommentList(List<TransactionCommentary> transactionDevolutionCommentList, int creationUserID)
		{
			try
			{
				if (transactionDevolutionCommentList == null) transactionDevolutionCommentList = new List<TransactionCommentary>();

				//Delete old ones
				this.DevolutionCommentaries
					.Where(e => e.TypeID == (int)SCC_BL.DBValues.Catalog.TRANSACTION_COMMENT_TYPE.DEVOLUTION_COMMENTARIES)
					.ToList()
					.ForEach(e => {
						if (!transactionDevolutionCommentList
							.Where(w =>
								w.ID != null &&
								w.ID > 0 &&
								w.TypeID == (int)SCC_BL.DBValues.Catalog.TRANSACTION_COMMENT_TYPE.DEVOLUTION_COMMENTARIES)
							.Select(s => s.ID)
							.Contains(e.ID))
							e.DeleteByID();
					});

				this.DevolutionImprovementSteps
					.Where(e => e.TypeID == (int)SCC_BL.DBValues.Catalog.TRANSACTION_COMMENT_TYPE.DEVOLUTION_IMPROVEMENT_STEPS)
					.ToList()
					.ForEach(e => {
						if (!transactionDevolutionCommentList
							.Where(w =>
								w.ID != null &&
								w.ID > 0 &&
								w.TypeID == (int)SCC_BL.DBValues.Catalog.TRANSACTION_COMMENT_TYPE.DEVOLUTION_IMPROVEMENT_STEPS)
							.Select(s => s.ID)
							.Contains(e.ID))
							e.DeleteByID();
					});

				this.DevolutionUserStrengths
					.Where(e => e.TypeID == (int)SCC_BL.DBValues.Catalog.TRANSACTION_COMMENT_TYPE.DEVOLUTION_USER_STRENGTHS)
					.ToList()
					.ForEach(e => {
						if (!transactionDevolutionCommentList
							.Where(w =>
								w.ID != null &&
								w.ID > 0 &&
								w.TypeID == (int)SCC_BL.DBValues.Catalog.TRANSACTION_COMMENT_TYPE.DEVOLUTION_USER_STRENGTHS)
							.Select(s => s.ID)
							.Contains(e.ID))
							e.DeleteByID();
					});

				//Update existing ones
				foreach (TransactionCommentary transactionDevolutionComment in transactionDevolutionCommentList.Where(e => e.TypeID == (int)SCC_BL.DBValues.Catalog.TRANSACTION_COMMENT_TYPE.DEVOLUTION_COMMENTARIES))
				{
					if (this.DevolutionCommentaries.Select(e => e.ID).Contains(transactionDevolutionComment.ID))
					{
						int currentBasicInfoID = 0;

						using (TransactionCommentary auxTransactionDevolutionComment = new TransactionCommentary(transactionDevolutionComment.ID))
						{
							auxTransactionDevolutionComment.SetDataByID();
							currentBasicInfoID = auxTransactionDevolutionComment.BasicInfoID;
						}

						TransactionCommentary newTransactionDevolutionComment = new TransactionCommentary(
							transactionDevolutionComment.ID,
							transactionDevolutionComment.TypeID,
							transactionDevolutionComment.TransactionID,
							transactionDevolutionComment.Comment,
							currentBasicInfoID,
							creationUserID,
							(int)SCC_BL.DBValues.Catalog.STATUS_TRANSACTION_COMMENTARY.UPDATED);

						int result = newTransactionDevolutionComment.Update();
					}
				}

				foreach (TransactionCommentary transactionDevolutionImprovementSteps in transactionDevolutionCommentList.Where(e => e.TypeID == (int)SCC_BL.DBValues.Catalog.TRANSACTION_COMMENT_TYPE.DEVOLUTION_IMPROVEMENT_STEPS))
				{
					if (this.DevolutionImprovementSteps.Select(e => e.ID).Contains(transactionDevolutionImprovementSteps.ID))
					{
						int currentBasicInfoID = 0;

						using (TransactionCommentary auxTransactionDevolutionImprovementSteps = new TransactionCommentary(transactionDevolutionImprovementSteps.ID))
						{
							auxTransactionDevolutionImprovementSteps.SetDataByID();
							currentBasicInfoID = auxTransactionDevolutionImprovementSteps.BasicInfoID;
						}

						TransactionCommentary newTransactionDevolutionImprovementSteps = new TransactionCommentary(
							transactionDevolutionImprovementSteps.ID,
							transactionDevolutionImprovementSteps.TypeID,
							transactionDevolutionImprovementSteps.TransactionID,
							transactionDevolutionImprovementSteps.Comment,
							currentBasicInfoID,
							creationUserID,
							(int)SCC_BL.DBValues.Catalog.STATUS_TRANSACTION_COMMENTARY.UPDATED);

						int result = newTransactionDevolutionImprovementSteps.Update();
					}
				}

				foreach (TransactionCommentary transactionDevolutionUserStrengths in transactionDevolutionCommentList.Where(e => e.TypeID == (int)SCC_BL.DBValues.Catalog.TRANSACTION_COMMENT_TYPE.DEVOLUTION_USER_STRENGTHS))
				{
					if (this.DevolutionUserStrengths.Select(e => e.ID).Contains(transactionDevolutionUserStrengths.ID))
					{
						int currentBasicInfoID = 0;

						using (TransactionCommentary auxTransactionDevolutionUserStrengths = new TransactionCommentary(transactionDevolutionUserStrengths.ID))
						{
							auxTransactionDevolutionUserStrengths.SetDataByID();
							currentBasicInfoID = auxTransactionDevolutionUserStrengths.BasicInfoID;
						}

						TransactionCommentary newTransactionDevolutionUserStrengths = new TransactionCommentary(
							transactionDevolutionUserStrengths.ID,
							transactionDevolutionUserStrengths.TypeID,
							transactionDevolutionUserStrengths.TransactionID,
							transactionDevolutionUserStrengths.Comment,
							currentBasicInfoID,
							creationUserID,
							(int)SCC_BL.DBValues.Catalog.STATUS_TRANSACTION_COMMENTARY.UPDATED);

						int result = newTransactionDevolutionUserStrengths.Update();
					}
				}

				//Create new ones
				foreach (TransactionCommentary transactionDevolutionComment in transactionDevolutionCommentList.Where(e => e.TypeID == (int)SCC_BL.DBValues.Catalog.TRANSACTION_COMMENT_TYPE.DEVOLUTION_COMMENTARIES))
				{
					if (!this.DevolutionCommentaries.Select(e => e.ID).Contains(transactionDevolutionComment.ID))
					{
                        if (!string.IsNullOrEmpty(transactionDevolutionComment.Comment))
						{
							TransactionCommentary newTransactionDevolutionComment = new TransactionCommentary(
								transactionDevolutionComment.TypeID,
								transactionDevolutionComment.TransactionID,
								transactionDevolutionComment.Comment ?? string.Empty,
								creationUserID,
								(int)SCC_BL.DBValues.Catalog.STATUS_TRANSACTION_COMMENTARY.CREATED);

							int result = newTransactionDevolutionComment.Insert();
						}
					}
				}

				foreach (TransactionCommentary transactionDevolutionImprovementSteps in transactionDevolutionCommentList.Where(e => e.TypeID == (int)SCC_BL.DBValues.Catalog.TRANSACTION_COMMENT_TYPE.DEVOLUTION_IMPROVEMENT_STEPS))
				{
					if (!this.DevolutionImprovementSteps.Select(e => e.ID).Contains(transactionDevolutionImprovementSteps.ID))
					{
                        if (!string.IsNullOrEmpty(transactionDevolutionImprovementSteps.Comment))
						{
							TransactionCommentary newTransactionDevolutionImprovementSteps = new TransactionCommentary(
								transactionDevolutionImprovementSteps.TypeID,
								transactionDevolutionImprovementSteps.TransactionID,
								transactionDevolutionImprovementSteps.Comment ?? string.Empty,
								creationUserID,
								(int)SCC_BL.DBValues.Catalog.STATUS_TRANSACTION_COMMENTARY.CREATED);

							int result = newTransactionDevolutionImprovementSteps.Insert();
						}
					}
				}

				foreach (TransactionCommentary transactionDevolutionUserStrengths in transactionDevolutionCommentList.Where(e => e.TypeID == (int)SCC_BL.DBValues.Catalog.TRANSACTION_COMMENT_TYPE.DEVOLUTION_USER_STRENGTHS))
				{
					if (!this.DevolutionUserStrengths.Select(e => e.ID).Contains(transactionDevolutionUserStrengths.ID))
					{
                        if (!string.IsNullOrEmpty(transactionDevolutionUserStrengths.Comment))
						{
							TransactionCommentary newTransactionDevolutionUserStrengths = new TransactionCommentary(
								transactionDevolutionUserStrengths.TypeID,
								transactionDevolutionUserStrengths.TransactionID,
								transactionDevolutionUserStrengths.Comment ?? string.Empty,
								creationUserID,
								(int)SCC_BL.DBValues.Catalog.STATUS_TRANSACTION_COMMENTARY.CREATED);

							int result = newTransactionDevolutionUserStrengths.Insert();
						}
					}
				}

				return Results.Transaction.UpdateDevolutionCommentList.CODE.SUCCESS;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public Results.Transaction.UpdateTransactionLabelList.CODE UpdateTransactionLabelList(string[] transactionLabelList, int creationUserID)
		{
			try
			{
				List<TransactionLabel> currentTransactionLabelList = new List<TransactionLabel>();

				this.TransactionLabelCatalogList
					.ForEach(e => {
						using (TransactionLabel transactionLabel = new TransactionLabel(e.LabelID))
						{
							transactionLabel.SetDataByID();
							currentTransactionLabelList.Add(transactionLabel);
						}
					});

				if (transactionLabelList == null) transactionLabelList = new string[0];

				transactionLabelList = transactionLabelList.Select(e => e.Trim()).ToArray();

				transactionLabelList = transactionLabelList.Where(e => !string.IsNullOrEmpty(e)).ToArray();

				//Delete old ones
				this.TransactionLabelCatalogList
					.ForEach(e => {
                        using (TransactionLabel transactionLabel = new TransactionLabel(e.LabelID))
						{
							transactionLabel.SetDataByID();

							if (!transactionLabelList.Contains(transactionLabel.Description))
								e.DeleteByID();
						}
					});

				//Create new ones
				foreach (string transactionLabel in transactionLabelList)
				{
					if (!currentTransactionLabelList.Select(e => e.Description).Contains(transactionLabel))
					{
						using (TransactionLabel auxTransactionLabel = new TransactionLabel(transactionLabel, creationUserID, (int)SCC_BL.DBValues.Catalog.STATUS_TRANSACTION_LABEL.CREATED))
						{
							auxTransactionLabel.Insert();

							using (TransactionLabelCatalog transactionLabelCatalog = TransactionLabelCatalog.TransactionLabelCatalogForInsert(this.ID, auxTransactionLabel.ID, creationUserID, (int)SCC_BL.DBValues.Catalog.STATUS_TRANSACTION_LABEL_CATALOG.CREATED))
							{
								transactionLabelCatalog.Insert();
							}
						}
					}
				}

				return Results.Transaction.UpdateTransactionLabelList.CODE.SUCCESS;
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