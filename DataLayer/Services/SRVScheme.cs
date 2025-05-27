using DataLayer.Classes;
using DataLayer.Interfaces;
using DataLayer.Models;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DataLayer.Services
{
    public class SRVScheme : ISRVScheme
    {
        private int LastSchemeID;
        private readonly ISRVSendEmail srvSendEmail;

        public SRVScheme()
        { }

        public DataTable FetchReport(int UserID, string SpName)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@UserID", UserID));
            DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, SpName, param.ToArray()).Tables[0];
            return dt;
        }
        public SRVScheme(ISRVSendEmail srvSendEmail)
        {
            this.srvSendEmail = srvSendEmail;
        }

        public SchemeModel GetBySchemeID_Notification(int SchemeID, SqlTransaction transaction = null)
        {
            try
            {
                SqlParameter param = new SqlParameter("@SchemeID", SchemeID);
                DataTable dt = new DataTable();
                List<SchemeModel> SchemeModel = new List<SchemeModel>();
                // dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Scheme_Notification", param).Tables[0];

                if (transaction != null)
                {
                    dt = SqlHelper.ExecuteDataset(transaction, CommandType.StoredProcedure, "RD_Scheme_Notification", param).Tables[0];
                }
                else
                {
                    dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Scheme_Notification", param).Tables[0];
                }

                if (dt.Rows.Count > 0)
                {
                    SchemeModel = Helper.ConvertDataTableToModel<SchemeModel>(dt);
                    return SchemeModel[0];
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public SchemeModel GetLastScheme(int UserID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_LastScheme", new SqlParameter("@UserID", UserID)).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    return RowOfScheme(dt.Rows[0]);
                }
                else
                    return null;
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }

        public SchemeModel GetBySchemeID(int SchemeID, SqlTransaction transaction = null)
        {
            try
            {
                SqlParameter param = new SqlParameter("@SchemeID", SchemeID);
                DataTable dt = new DataTable();

                if (transaction != null)
                {
                    //SqlDataReader sReader = SqlHelper.ExecuteReader(transaction, CommandType.StoredProcedure, "RD_Scheme", param);
                    //Create a new DataTable.
                    //dt.Load(sReader);
                    dt = SqlHelper.ExecuteDataset(transaction, CommandType.StoredProcedure, "RD_Scheme", param).Tables[0];
                }
                else
                {
                    dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Scheme", param).Tables[0];
                }

                if (dt.Rows.Count > 0)
                {
                    return RowOfScheme(dt.Rows[0]);
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<SchemeModel> SaveScheme(SchemeModel Scheme)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[30];
                param[0] = new SqlParameter("@SchemeID", Scheme.SchemeID);
                //param[1] = new SqlParameter("@SchemeUserID_OLD", Scheme.SchemeUserID_OLD);
                param[1] = new SqlParameter("@SchemeName", Scheme.SchemeName);
                param[2] = new SqlParameter("@SchemeCode", Scheme.SchemeCode);
                //param[4] = new SqlParameter("@SchemeTypeID_OLD", Scheme.SchemeTypeID_OLD);
                param[3] = new SqlParameter("@ProgramTypeID", Scheme.ProgramTypeID);
                param[4] = new SqlParameter("@PCategoryID", Scheme.PCategoryID);
                //param[7] = new SqlParameter("@ProjectID_OLD", Scheme.ProjectID_OLD);
                param[5] = new SqlParameter("@FundingSourceID", Scheme.FundingSourceID);
                param[6] = new SqlParameter("@FundingCategoryID", Scheme.FundingCategoryID);
                param[7] = new SqlParameter("@PaymentSchedule", Scheme.PaymentSchedule);
                param[8] = new SqlParameter("@Description", Scheme.Description);
                param[9] = new SqlParameter("@Stipend", Scheme.Stipend);
                param[10] = new SqlParameter("@StipendMode", Scheme.StipendMode);
                param[11] = new SqlParameter("@UniformAndBag", Scheme.UniformAndBag);
                param[12] = new SqlParameter("@MinimumEducation", Scheme.MinimumEducation);
                param[13] = new SqlParameter("@MaximumEducation", Scheme.MaximumEducation);
                param[14] = new SqlParameter("@MinAge", Scheme.MinAge);
                param[15] = new SqlParameter("@MaxAge", Scheme.MaxAge);
                param[16] = new SqlParameter("@GenderID", Scheme.GenderID);
                param[17] = new SqlParameter("@DualEnrollment", Scheme.DualEnrollment);
                param[18] = new SqlParameter("@ContractAwardDate", Scheme.ContractAwardDate);
                param[19] = new SqlParameter("@BusinessRuleType", Scheme.BusinessRuleType);
                //param[20] = new SqlParameter("@InActive", Scheme.InActive);
                param[20] = new SqlParameter("@CurUserID", Scheme.CurUserID);
                param[21] = new SqlParameter("@OrganizationID", Scheme.OrganizationID);

                param[22] = new SqlParameter("@Ident", SqlDbType.Int);
                param[22].Direction = ParameterDirection.Output;

                param[23] = new SqlParameter("@FinalSubmitted", Scheme.FinalSubmitted);

                Scheme.UID = Guid.NewGuid().ToString();
                Scheme.isMigrated = false;

                param[24] = new SqlParameter("@UID", Scheme.UID);
                param[25] = new SqlParameter("@isMigrated", Scheme.isMigrated);
                param[26] = new SqlParameter("@IsApproved", Scheme.IsApproved);
                param[27] = new SqlParameter("@IsRejected", Scheme.IsRejected);

                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_Scheme]", param);

                LastSchemeID = Convert.ToInt32(param[22].Value);
                SchemeModel mod = new SchemeModel();
                mod.SchemeID = LastSchemeID;

                return FetchAllScheme(mod);
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }

        public void FinalSubmit(SchemeModel model)
        {
            try
            {
                ///Update Scheme's FinalSubmitted Status
                //SaveScheme(new SchemeModel() { SchemeID = model.SchemeID, FinalSubmitted = true, CurUserID = model.CurUserID });
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@SchemeID", model.SchemeID));
                param.Add(new SqlParameter("@FinalSubmitted", model.FinalSubmitted));
                param.Add(new SqlParameter("@CurUserID", model.CurUserID));
                param.Add(new SqlParameter("@ProcessKey", model.ProcessKey));
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[FinalSubmitScheme]", param.ToArray());

                //var historyModel = new ApprovalHistoryModel()
                //{
                //    ApprovalHistoryID = 0,
                //    ProcessKey = "AP",
                //    Step = 1,
                //    FormID = model.SchemeID,
                //    ApprovalStatusID = (int)EnumApprovalStatus.Pending,
                //    Comments = "Pending",
                //    ApproverID = null,
                //    CurUserID = model.CurUserID,
                //    InActive = false
                //};
                var firstApproval = new SRVApproval().FetchApproval(new ApprovalModel() { Step = 1, ProcessKey = model.ProcessKey }).FirstOrDefault();

                ///Add Scheme For Approval Process
                //new SRVApprovalHistory().SaveApprovalHistory(historyModel);
                ApprovalModel approvalsModelForNotification = new ApprovalModel();
                ApprovalHistoryModel model1 = new ApprovalHistoryModel();
                model1.ApprovalStatusID = (int)EnumApprovalStatus.Pending;
                approvalsModelForNotification.UserIDs = firstApproval.UserIDs;
                approvalsModelForNotification.ProcessKey = firstApproval.ProcessKey;
                approvalsModelForNotification.CustomComments = "created";
                srvSendEmail.GenerateEmailAndSendNotification(approvalsModelForNotification, model1);

                //   srvSendEmail.GenerateEmailToApprovers(firstApproval, new ApprovalHistoryModel() { ApprovalStatusID = (int)EnumApprovalStatus.Pending });
            }
            catch (Exception e)
            { throw new Exception(e.Message); }
        }

        public void DeleteDraftAppendix(int schemeID)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@SchemeID", schemeID));
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "DeleteDraftAppendix", param.ToArray());
            }
            catch (Exception e)
            { throw new Exception(e.Message); }
        }

        private List<SchemeModel> LoopinData(DataTable dt, bool user = false)
        {
            List<SchemeModel> SchemeL = new List<SchemeModel>();

            foreach (DataRow r in dt.Rows)
            {
                SchemeL.Add(RowOfScheme(r, user));
            }
            return SchemeL;
        }

        public List<SchemeModel> FetchScheme(SchemeModel mod)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Scheme", Common.GetParams(mod)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<SchemeModel> FetchAllScheme(SchemeModel mod)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_AllSchemes", Common.GetParams(mod)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public SchemeModel GetAllSchemeBySchemeID(int SchemeID, SqlTransaction transaction = null)
        {
            try
            {
                SqlParameter param = new SqlParameter("@SchemeID", SchemeID);
                DataTable dt = new DataTable();

                if (transaction != null)
                {
                    //SqlDataReader sReader = SqlHelper.ExecuteReader(transaction, CommandType.StoredProcedure, "RD_Scheme", param);
                    //Create a new DataTable.
                    //dt.Load(sReader);
                    dt = SqlHelper.ExecuteDataset(transaction, CommandType.StoredProcedure, "RD_AllSchemes", param).Tables[0];
                }
                else
                {
                    dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_AllSchemes", param).Tables[0];
                }

                if (dt.Rows.Count > 0)
                {
                    return RowOfScheme(dt.Rows[0]);
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public bool SchemeApproveReject(SchemeModel model, SqlTransaction transaction = null)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@SchemeID", model.SchemeID));
                param.Add(new SqlParameter("@IsApproved", model.IsApproved));
                param.Add(new SqlParameter("@IsRejected", model.IsRejected));
                param.Add(new SqlParameter("@CurUserID", model.CurUserID));

                if (transaction != null)
                {
                    SqlHelper.ExecuteScalar(transaction, CommandType.StoredProcedure, "U_SchemeApproveReject", param.ToArray());
                }
                else
                {
                    SqlHelper.ExecuteScalar(SqlHelper.GetCon(), CommandType.StoredProcedure, "U_SchemeApproveReject", param.ToArray());
                }
                return true;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public bool UpdateSchemeSAPID(int schemeID, string sapObjId, SqlTransaction transaction = null)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[10];
                param[0] = new SqlParameter("@SchemeID", schemeID);
                param[1] = new SqlParameter("@SAPID", sapObjId);
                if (transaction != null)
                {
                    SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, "[AU_SchemeSAPID]", param);
                }
                else
                {
                    SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_SchemeSAPID]", param);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<SchemeModel> FetchScheme()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Scheme").Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        //================Azhar Iqbal ===========================
        public List<SchemeModel> FetchSchemeBusinessRuleType(string BusinessRuleType)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Scheme", new SqlParameter("@BusinessRuleType", BusinessRuleType)).Tables[0];
                return LoopinData(dt, true);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        //=======================================================

        public List<SchemeModel> FetchScheme(bool InActive)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Scheme", new SqlParameter("@InActive", InActive)).Tables[0];
                return LoopinData(dt, true);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<SchemeModel> FetchSchemeForFilter(bool InActive)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_SchemeFilter", new SqlParameter("@InActive", InActive)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<SchemeModel> FetchSchemeForROSIFilter(ROSIFiltersModel rosiFilters)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[10];
                param[0] = new SqlParameter("@TSPIDs", rosiFilters.TSPIDs);
                param[1] = new SqlParameter("@PTypeIDs", rosiFilters.PTypeIDs);
                param[2] = new SqlParameter("@SectorIDs", rosiFilters.SectorIDs);
                param[3] = new SqlParameter("@ClusterIDs", rosiFilters.ClusterIDs);
                param[4] = new SqlParameter("@DistrictIDs", rosiFilters.DistrictIDs);
                param[5] = new SqlParameter("@OIDs", rosiFilters.OrganizationIDs);
                param[6] = new SqlParameter("@TradeIDs", rosiFilters.TradeIDs);
                param[7] = new SqlParameter("@FundingSourceIDs", rosiFilters.FundingSourceIDs);
                param[8] = new SqlParameter("@GenderIDs", rosiFilters.GenderIDs);
                param[9] = new SqlParameter("@DurationIDs", rosiFilters.DurationIDs);
                //param[1] = new SqlParameter("@CurUserID", CurUserID);

                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_SchemeFilterForROSI", param).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        //public List<SchemeModel> LoadNotSubmittedSchemes(int UserID)
        //{
        //    try
        //    {
        //        SqlParameter[] param = new SqlParameter[2];
        //        param[0] = new SqlParameter("@CreatedUserID", UserID);
        //        param[1] = new SqlParameter("@FinalSubmitted", false);

        //        DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_NotSubmittedSchemes", param).Tables[0];
        //        return LoopinData(dt);
        //    }
        //    catch (Exception ex) { throw new Exception(ex.Message); }
        //}

        public int BatchInsert(List<SchemeModel> ls, int @BatchFkey, int CurUserID)
        {
            SqlParameter[] param = new SqlParameter[3];

            param[0] = new SqlParameter("@Json", JsonConvert.SerializeObject(ls));
            param[1] = new SqlParameter("@", @BatchFkey);
            param[2] = new SqlParameter("@CurUserID", CurUserID);
            return SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[BAU_Scheme]", param);
        }

        public List<SchemeModel> GetByPCategoryID(int PCategoryID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Scheme", new SqlParameter("@PCategoryID", PCategoryID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<SchemeModel> GetByFundingSourceID(int FundingSourceID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Scheme", new SqlParameter("@FundingSourceID", FundingSourceID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<SchemeModel> GetByFundingCategoryID(int FundingCategoryID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Scheme", new SqlParameter("@FundingCategoryID", FundingCategoryID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<SchemeModel> GetByGenderID(int GenderID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Scheme", new SqlParameter("@GenderID", GenderID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public void RemoveFromAppendix(int FormID, string Form)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[2];

                param[0] = new SqlParameter("@FormID", FormID);
                param[1] = new SqlParameter("@Form", Form);

                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "RemoveFromAppendix", param);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public void ActiveInActive(int SchemeID, bool? InActive, int CurUserID)
        {
            SqlParameter[] PLead = new SqlParameter[3];
            PLead[0] = new SqlParameter("@SchemeID", SchemeID);
            PLead[1] = new SqlParameter("@InActive", InActive);
            PLead[2] = new SqlParameter("@CurUserID", CurUserID);
            SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AI_Scheme]", PLead);
        }

        private SchemeModel RowOfScheme(DataRow r, bool user = false)
        {
            SchemeModel Scheme = new SchemeModel();
            Scheme.SchemeID = Convert.ToInt32(r["SchemeID"]);
            //Scheme.SchemeUserID_OLD = Convert.ToInt32(r["SchemeUserID_OLD"]);
            Scheme.SchemeName = r["SchemeName"].ToString();
            Scheme.SchemeCode = r["SchemeCode"].ToString();
            //Scheme.SchemeTypeID_OLD = Convert.ToInt32(r["SchemeTypeID_OLD"]);
            Scheme.ProgramTypeID = Convert.ToInt32(r["ProgramTypeID"]);
            Scheme.PTypeName = r["PTypeName"].ToString();
            Scheme.PCategoryID = Convert.ToInt32(r["PCategoryID"]);
            Scheme.PCategoryName = r["PCategoryName"].ToString();
            //Scheme.ProjectID_OLD = Convert.ToInt32(r["ProjectID_OLD"]);
            Scheme.FundingSourceID = Convert.ToInt32(r["FundingSourceID"]);
            Scheme.FundingSourceName = r["FundingSourceName"].ToString();
            Scheme.FundingCategoryID = Convert.ToInt32(r["FundingCategoryID"]);
            Scheme.FundingCategoryName = r["FundingCategoryName"].ToString();
            Scheme.PaymentSchedule = r["PaymentSchedule"].ToString();
            Scheme.Description = r["Description"].ToString();
            Scheme.Stipend = Convert.ToDouble(r["Stipend"]);
            Scheme.StipendMode = r["StipendMode"].ToString();
            Scheme.UniformAndBag = Convert.ToDouble(r["UniformAndBag"]);
            //Scheme.MinimumEducation = r["MinimumEducation"] == "" ? 0 : Convert.ToInt32(r["MinimumEducation"]);
            Scheme.MinimumEducation = Convert.ToInt32(r["MinimumEducation"]);
            Scheme.MaximumEducation = Convert.ToInt32(r["MaximumEducation"]);
            Scheme.MinAge = Convert.ToInt32(r["MinAge"]);
            Scheme.MaxAge = Convert.ToInt32(r["MaxAge"]);
            Scheme.GenderID = Convert.ToInt32(r["GenderID"]);
            Scheme.OrganizationID = Convert.ToInt32(r["OrganizationID"]);
            Scheme.OName = r["OName"].ToString();
            Scheme.GenderName = r["GenderName"].ToString();
            //Scheme.DualEnrollment =  Convert.ToBoolean(r["DualEnrollment"]) ;
            Scheme.DualEnrollment = r.Field<bool?>("DualEnrollment");
            Scheme.CreatedDate = r["CreatedDate"].ToString().GetDate();
            Scheme.ModifiedDate = r["ModifiedDate"].ToString().GetDate();
            Scheme.ContractAwardDate = r["ContractAwardDate"].ToString().GetDate();
            Scheme.BusinessRuleType = r["BusinessRuleType"].ToString();
            Scheme.CreatedUserID = Convert.ToInt32(r["CreatedUserID"]);
            Scheme.ModifiedUserID = Convert.ToInt32(r["ModifiedUserID"]);
            Scheme.InActive = Convert.ToBoolean(r["InActive"]);
            Scheme.FinalSubmitted = Convert.ToBoolean(r["FinalSubmitted"]);
            Scheme.UID = r["UID"].ToString();
            Scheme.isMigrated = Convert.ToBoolean(r["isMigrated"]);
            //Scheme.IsManual = Convert.ToBoolean(r["IsManual"]);
            Scheme.IsApproved = Convert.ToBoolean(r["IsApproved"]);
            Scheme.MinimumEducationName = r["MinimumEducationName"].ToString();
            Scheme.MaximumEducationName = r["MaximumEducationName"].ToString();
            if (r.Table.Columns.Contains("SAPID"))
            {
                if (r["SAPID"] == null || r["SAPID"].ToString().Length == 0)
                    Scheme.SAPID = "";
                else
                    Scheme.SAPID = r["SAPID"].ToString();
            }
            if (user == true)
            {
                if (r["Username"] != null || r["Username"].ToString() != "")
                {
                    Scheme.UserName = r["Username"].ToString();
                }
            }

            return Scheme;
        }

        //public List<SubmittedSchemesModel> GetSubmittedSchemes()
        //{
        //    try
        //    {
        //        DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_SubmittedSchemes").Tables[0];
        //        return LoopinDataSubmitted(dt, true);
        //    }
        //    catch (Exception ex)
        //    { throw new Exception(ex.Message); }
        //}

        public int GetSchemeSequence()
        {
            try
            {
                int seq = Convert.ToInt32(SqlHelper.ExecuteScalar(SqlHelper.GetCon(), CommandType.StoredProcedure, "GetSchemeSequence"));
                return seq;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        //private List<SubmittedSchemesModel> LoopinDataSubmitted(DataTable dt, bool user = false)
        //{
        //    List<SubmittedSchemesModel> SchemeL = new List<SubmittedSchemesModel>();

        //    foreach (DataRow r in dt.Rows)
        //    {
        //        SchemeL.Add(RowOfSubmittedSchemes(r, user));
        //    }
        //    return SchemeL;
        //}

        //private SubmittedSchemesModel RowOfSubmittedSchemes(DataRow r, bool user = false)
        //{
        //    SubmittedSchemesModel Scheme = new SubmittedSchemesModel();
        //    Scheme.SchemeID = Convert.ToInt32(r["SchemeID"]);
        //    //Scheme.SchemeUserID_OLD = Convert.ToInt32(r["SchemeUserID_OLD"]);
        //    Scheme.SchemeName = r["SchemeName"].ToString();
        //    Scheme.SchemeCode = r["SchemeCode"].ToString();
        //    //Scheme.SchemeTypeID_OLD = Convert.ToInt32(r["SchemeTypeID_OLD"]);
        //    Scheme.ProgramTypeID = Convert.ToInt32(r["ProgramTypeID"]);
        //    Scheme.PTypeName = r["PTypeName"].ToString();
        //    Scheme.PCategoryID = Convert.ToInt32(r["PCategoryID"]);
        //    Scheme.PCategoryName = r["PCategoryName"].ToString();
        //    //Scheme.ProjectID_OLD = Convert.ToInt32(r["ProjectID_OLD"]);
        //    Scheme.FundingSourceID = Convert.ToInt32(r["FundingSourceID"]);
        //    Scheme.FundingSourceName = r["FundingSourceName"].ToString();
        //    Scheme.FundingCategoryID = Convert.ToInt32(r["FundingCategoryID"]);
        //    Scheme.FundingCategoryName = r["FundingCategoryName"].ToString();
        //    Scheme.PaymentSchedule = r["PaymentSchedule"].ToString();
        //    Scheme.Description = r["Description"].ToString();
        //    Scheme.Stipend = Convert.ToDouble(r["Stipend"]);
        //    Scheme.StipendMode = r["StipendMode"].ToString();
        //    Scheme.UniformAndBag = Convert.ToDouble(r["UniformAndBag"]);
        //    Scheme.MinimumEducation = Convert.ToInt32(r["MinimumEducation"]);
        //    Scheme.MaximumEducation = Convert.ToInt32(r["MaximumEducation"]);
        //    Scheme.MinAge = Convert.ToInt32(r["MinAge"]);
        //    Scheme.MaxAge = Convert.ToInt32(r["MaxAge"]);
        //    Scheme.GenderID = Convert.ToInt32(r["GenderID"]);
        //    Scheme.OrganizationID = Convert.ToInt32(r["OrganizationID"]);
        //    Scheme.OName = r["OName"].ToString();
        //    Scheme.GenderName = r["GenderName"].ToString();
        //    Scheme.DualEnrollment = Convert.ToBoolean(r["DualEnrollment"]);
        //    Scheme.CreatedDate = r["CreatedDate"].ToString().GetDate();
        //    Scheme.ModifiedDate = r["ModifiedDate"].ToString().GetDate();
        //    Scheme.ContractAwardDate = r["ContractAwardDate"].ToString().GetDate();
        //    Scheme.BusinessRuleType = r["BusinessRuleType"].ToString();
        //    Scheme.CreatedUserID = Convert.ToInt32(r["CreatedUserID"]);
        //    Scheme.ModifiedUserID = Convert.ToInt32(r["ModifiedUserID"]);
        //    Scheme.InActive = Convert.ToBoolean(r["InActive"]);
        //    Scheme.FinalSubmitted = Convert.ToBoolean(r["FinalSubmitted"]);
        //    Scheme.UID = r["UID"].ToString();
        //    Scheme.isMigrated = Convert.ToBoolean(r["isMigrated"]);
        //    Scheme.IsApproved = Convert.ToBoolean(r["IsApproved"]);
        //    Scheme.MinimumEducationName = r["MinimumEducationName"].ToString();
        //    Scheme.MaximumEducationName = r["MaximumEducationName"].ToString();

        //    if (user == true)
        //    {
        //        if (r["Username"] != null || r["Username"].ToString() != "")
        //        {
        //            Scheme.UserName = r["Username"].ToString();
        //        }
        //    }

        //    //Scheme.IsApprovedForm = Convert.ToBoolean(r["IsApproved"]);
        //    //Scheme.FormApprovalID = Convert.ToInt32(r["FormApprovalID"]);
        //    //Scheme.SendBack = Convert.ToBoolean(r["SendBack"]);
        //    //Scheme.Comment = r["Comment"].ToString();

        //    return Scheme;
        //}

        public List<SchemeModel> FetchSchemesByTSPUser(int userID = 0)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_SchemesByTSPUser", new SqlParameter("@UserID", userID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<SchemeModel> FetchSchemesByTSPUserOnJob(int userID = 0)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_SchemesByTSPUserOnJob", new SqlParameter("@UserID", userID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<SchemeModel> FetchSchemesBySkillScholarshipType(int userID = 0)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_SchemesByTSPUserSkillScholarShip", new SqlParameter("@UserID", userID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<SchemeModel> FetchSchemesByKAMUser(int userID = 0)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_SchemesByKAMUser", new SqlParameter("@UserID", userID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<SchemeModel> FetchSchemeByUser(QueryFilters filters)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@UserID", filters.UserID));
                param.Add(new SqlParameter("@OID", filters.OID));
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_SchemesByUser", param.ToArray()).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<SchemeModel> FetchSchemeDataByUser(int UserID)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@UserID", UserID));

                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_SchemesByUser", param.ToArray()).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public int GetUnverifiedTraineeEmail(int? tspId)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@tspId", tspId)); // Use @tspId instead of @TspId

                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_UnverifiedTraineeEmail", param.ToArray()).Tables[0];

                return dt.Rows.Count;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public int GetUnverifiedTraineeEmailByKam(int KamId)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@KamId", KamId)); // Use @tspId instead of @TspId

                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_UnverifiedTraineeEmail", param.ToArray()).Tables[0];

                return dt.Rows.Count;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<SchemeModel> FetchSchemeByUser_DVV(int userID)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@UserID", userID));
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_SchemesByUser_DVV", param.ToArray()).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<SchemeModel> FetchSchemeByUserPaged(PagingModel pagingModel, SearchFilter filterModel, out int totalCount)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@SchemeID", filterModel.SchemeID));
                param.Add(new SqlParameter("@TSPID", filterModel.TSPID));
                param.Add(new SqlParameter("@ClassID", filterModel.ClassID));
                param.Add(new SqlParameter("@TraineeID", filterModel.TraineeID));
                param.Add(new SqlParameter("@UserID", filterModel.UserID));
                param.Add(new SqlParameter("@OID", filterModel.OID));
                param.AddRange(Common.GetPagingParams(pagingModel));

                DataTable dt = new DataTable();
                dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_SchemesByUserPaged", param.ToArray()).Tables[0];

                if (dt.Rows.Count > 0)
                    totalCount = Convert.ToInt32(dt.Rows[0]["TotalRows"]);
                else
                    totalCount = 0;
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<SchemeModel> SSPFetchSchemeByUser(int UserID)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@UserID", UserID));
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_SSPProgramByUser", param.ToArray()).Tables[0];
                return SSPLoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        private List<SchemeModel> SSPLoopinData(DataTable dt, bool user = false)
        {
            List<SchemeModel> SchemeL = new List<SchemeModel>();

            foreach (DataRow r in dt.Rows)
            {
                SchemeL.Add(RowOfSchemeSSP(r, user));
            }
            return SchemeL;
        }
        private SchemeModel RowOfSchemeSSP(DataRow r, bool user = false)
        {
            SchemeModel Scheme = new SchemeModel();
            Scheme.ProgramID = Convert.ToInt32(r["ProgramID"]);
            Scheme.ProgramName = r["ProgramName"].ToString();
            Scheme.SchemeCode = r["ProgramCode"].ToString();
            Scheme.Description = r["ProgramDescription"].ToString();
            Scheme.PaymentSchedule = r["PaymentStructure"].ToString();
            Scheme.ProcessStartDate = Convert.ToDateTime(r["ProcessStartDate"]);
            Scheme.ProcessEndDate = Convert.ToDateTime(r["ProcessEndDate"]);
            Scheme.IsLocked = r["IsLocked"].ToString();

            return Scheme;
        }

        public void GenerateAutoAppendix(int schemeID, int CurUserID)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@ProgramID", schemeID));
                param.Add(new SqlParameter("@CreatedUserID", CurUserID));
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "SSPBSSCreateScheme", param.ToArray());
            }
            catch (Exception e)
            { throw new Exception(e.Message); }
        }

    }
}