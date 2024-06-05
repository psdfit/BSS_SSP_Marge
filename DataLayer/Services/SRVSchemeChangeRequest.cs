using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using DataLayer.Classes;
using DataLayer.Models;
using Newtonsoft.Json;
using DataLayer.Interfaces;
namespace DataLayer.Services
{
    public class SRVSchemeChangeRequest : SRVBase, ISRVSchemeChangeRequest
    {
        public SRVSchemeChangeRequest() { }
        public SchemeChangeRequestModel GetBySchemeChangeRequestID(int SchemeChangeRequestID, SqlTransaction transaction = null)
        {
            try
            {
                SqlParameter param = new SqlParameter("@SchemeChangeRequestID", SchemeChangeRequestID);
                DataTable dt = new DataTable();

                if (transaction != null)
                {
                    dt = SqlHelper.ExecuteDataset(transaction, CommandType.StoredProcedure, "RD_SchemeChangeRequest", param).Tables[0];
                }
                else
                {
                    dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_SchemeChangeRequest", param).Tables[0];
                }

                if (dt.Rows.Count > 0)
                {
                    return RowOfSchemeChangeRequest(dt.Rows[0]);
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public List<SchemeChangeRequestModel> SaveSchemeChangeRequest(SchemeChangeRequestModel SchemeChangeRequest)
        {
            try
            {

                SqlParameter[] param = new SqlParameter[9];
                param[0] = new SqlParameter("@SchemeChangeRequestID", SchemeChangeRequest.SchemeChangeRequestID);
                param[1] = new SqlParameter("@SchemeID", SchemeChangeRequest.SchemeID);
                param[2] = new SqlParameter("@SchemeName", SchemeChangeRequest.SchemeName);
                param[3] = new SqlParameter("@SchemeCode", SchemeChangeRequest.SchemeCode);
                param[4] = new SqlParameter("@BusinessRuleType", SchemeChangeRequest.BusinessRuleType);
                param[5] = new SqlParameter("@Stipend", SchemeChangeRequest.Stipend);
                param[6] = new SqlParameter("@StipendMode", SchemeChangeRequest.StipendMode);
                param[7] = new SqlParameter("@UniformAndBag", SchemeChangeRequest.UniformAndBag);

                param[8] = new SqlParameter("@CurUserID", SchemeChangeRequest.CurUserID);
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_SchemeChangeRequest]", param);
                return FetchSchemeChangeRequest();
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }

        public void Update_CR_SchemeApprovalHistory(SchemeChangeRequestModel model)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@SchemeChangeRequestID", model.SchemeChangeRequestID));
                //param.Add(new SqlParameter("@FinalSubmitted", model.FinalSubmitted));
                param.Add(new SqlParameter("@CurUserID", model.CurUserID));
                param.Add(new SqlParameter("@ProcessKey", model.ProcessKey));
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_CR_Scheme_ApprovalHistory]", param.ToArray());


            }
            catch (Exception e)
            { throw new Exception(e.Message); }
        }


        public bool CrSchemeApproveReject(SchemeChangeRequestModel model, SqlTransaction transaction = null)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@SchemeChangeRequestID", model.SchemeChangeRequestID));
                param.Add(new SqlParameter("@IsApproved", model.IsApproved));
                param.Add(new SqlParameter("@IsRejected", model.IsRejected));
                param.Add(new SqlParameter("@CurUserID", model.CurUserID));

                if (transaction != null)
                {
                    SqlHelper.ExecuteScalar(transaction, CommandType.StoredProcedure, "U_Cr_SchemeApproveReject", param.ToArray());
                }
                else
                {
                    SqlHelper.ExecuteScalar(SqlHelper.GetCon(), CommandType.StoredProcedure, "U_Cr_SchemeApproveReject", param.ToArray());
                }
                return true;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }




        private List<SchemeChangeRequestModel> LoopinData(DataTable dt)
        {
            List<SchemeChangeRequestModel> SchemeChangeRequestL = new List<SchemeChangeRequestModel>();

            foreach (DataRow r in dt.Rows)
            {
                SchemeChangeRequestL.Add(RowOfSchemeChangeRequest(r));

            }
            return SchemeChangeRequestL;
        }
        public List<SchemeChangeRequestModel> FetchSchemeChangeRequest(SchemeChangeRequestModel mod)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_SchemeChangeRequest", Common.GetParams(mod)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<SchemeChangeRequestModel> FetchSchemeChangeRequest()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_SchemeChangeRequest").Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public List<SchemeChangeRequestModel> FetchSchemeChangeRequest(bool InActive)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_SchemeChangeRequest", new SqlParameter("@InActive", InActive)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public List<SchemeChangeRequestModel> GetBySchemeID(int SchemeID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_SchemeChangeRequest", new SqlParameter("@SchemeID", SchemeID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public void ActiveInActive(int SchemeChangeRequestID, bool? InActive, int CurUserID)
        {

            SqlParameter[] PLead = new SqlParameter[3];
            PLead[0] = new SqlParameter("@SchemeChangeRequestID", SchemeChangeRequestID);
            PLead[1] = new SqlParameter("@InActive", InActive);
            PLead[2] = new SqlParameter("@CurUserID", CurUserID);
            SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AI_SchemeChangeRequest]", PLead);
        }
        private SchemeChangeRequestModel RowOfSchemeChangeRequest(DataRow r)
        {
            SchemeChangeRequestModel SchemeChangeRequest = new SchemeChangeRequestModel();
            SchemeChangeRequest.SchemeChangeRequestID = Convert.ToInt32(r["SchemeChangeRequestID"]);
            SchemeChangeRequest.SchemeID = Convert.ToInt32(r["SchemeID"]);
            SchemeChangeRequest.SchemeName = r["SchemeName"].ToString();
            SchemeChangeRequest.SchemeName = r["SchemeName"].ToString();
            SchemeChangeRequest.SchemeCode = r["SchemeCode"].ToString();
            SchemeChangeRequest.BusinessRuleType = r["BusinessRuleType"].ToString();
            SchemeChangeRequest.Stipend = Convert.ToInt32(r["Stipend"]);
            SchemeChangeRequest.StipendMode = r["StipendMode"].ToString();
            SchemeChangeRequest.UniformAndBag = Convert.ToInt32(r["UniformAndBag"]);
            SchemeChangeRequest.InActive = Convert.ToBoolean(r["InActive"]);
            SchemeChangeRequest.CreatedUserID = Convert.ToInt32(r["CreatedUserID"]);
            SchemeChangeRequest.ModifiedUserID = Convert.ToInt32(r["ModifiedUserID"]);
            SchemeChangeRequest.CreatedDate = r["CreatedDate"].ToString().GetDate();
            SchemeChangeRequest.ModifiedDate = r["ModifiedDate"].ToString().GetDate();
            return SchemeChangeRequest;
        }
    }
}
