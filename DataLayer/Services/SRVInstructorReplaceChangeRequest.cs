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
    public class SRVInstructorReplaceChangeRequest : SRVBase, ISRVInstructorReplaceChangeRequest
    {
        public SRVInstructorReplaceChangeRequest() { }
        public InstructorReplaceChangeRequestModel GetByInstructorReplaceChangeRequestID(int InstructorReplaceChangeRequestID, SqlTransaction transaction = null)
        {
            try
            {
                SqlParameter param = new SqlParameter("@InstructorReplaceChangeRequestID", InstructorReplaceChangeRequestID);
                DataTable dt = new DataTable(); //= SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_InstructorReplaceChangeRequest", param).Tables[0];

                if (transaction != null)
                {
                    dt = SqlHelper.ExecuteDataset(transaction, CommandType.StoredProcedure, "RD_InstructorReplaceChangeRequest", param).Tables[0];
                }
                else
                {
                    dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_InstructorReplaceChangeRequest", param).Tables[0];

                }


                if (dt.Rows.Count > 0)
                {
                    return RowOfInstructorReplaceChangeRequest(dt.Rows[0]);
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public List<InstructorReplaceChangeRequestModel> SaveInstructorReplaceChangeRequest(InstructorReplaceChangeRequestModel InstructorReplaceChangeRequest)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[5];
                param[0] = new SqlParameter("@InstructorReplaceChangeRequestID", InstructorReplaceChangeRequest.InstructorReplaceChangeRequestID);
                param[1] = new SqlParameter("@IncepReportID", InstructorReplaceChangeRequest.IncepReportID);
                param[2] = new SqlParameter("@ClassID", InstructorReplaceChangeRequest.ClassID);
              
                param[3] = new SqlParameter("@InstrIDs", InstructorReplaceChangeRequest.InstrIDs);

                param[4] = new SqlParameter("@CurUserID", InstructorReplaceChangeRequest.CurUserID);
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_InstructorReplaceChangeRequest]", param);
                return FetchInstructorReplaceChangeRequest();
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }
        private List<InstructorReplaceChangeRequestModel> LoopinData(DataTable dt)
        {
            List<InstructorReplaceChangeRequestModel> InstructorReplaceChangeRequestL = new List<InstructorReplaceChangeRequestModel>();

            foreach (DataRow r in dt.Rows)
            {
                InstructorReplaceChangeRequestL.Add(RowOfInstructorReplaceChangeRequest(r));

            }
            return InstructorReplaceChangeRequestL;
        }
        public List<InstructorReplaceChangeRequestModel> FetchInstructorReplaceChangeRequest(InstructorReplaceChangeRequestModel mod)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_InstructorReplaceChangeRequest", Common.GetParams(mod)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<InstructorReplaceChangeRequestModel> FetchInstructorReplaceChangeRequestByUserID(int userID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_InstructorReplaceChangeRequest", new SqlParameter("userID", userID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<InstructorReplaceChangeRequestModel> FetchInstructorReplaceChangeRequest()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_InstructorReplaceChangeRequest").Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public List<InstructorReplaceChangeRequestModel> FetchInstructorReplaceChangeRequest(bool InActive)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_InstructorReplaceChangeRequest", new SqlParameter("@InActive", InActive)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public List<InstructorReplaceChangeRequestModel> GetByClassID(int ClassID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_InstructorReplaceChangeRequest", new SqlParameter("@ClassID", ClassID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public bool CrInstructorReplaceApproveReject(InstructorReplaceChangeRequestModel model, SqlTransaction transaction = null)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@InstructorReplaceChangeRequestID", model.InstructorReplaceChangeRequestID));
                param.Add(new SqlParameter("@IsApproved", model.IsApproved));
                param.Add(new SqlParameter("@IsRejected", model.IsRejected));
                param.Add(new SqlParameter("@CurUserID", model.CurUserID));

                if (transaction != null)
                {
                    SqlHelper.ExecuteScalar(transaction, CommandType.StoredProcedure, "U_Cr_InstructorReplaceApproveReject", param.ToArray());
                }
                else
                {
                    SqlHelper.ExecuteScalar(SqlHelper.GetCon(), CommandType.StoredProcedure, "U_Cr_InstructorReplaceApproveReject", param.ToArray());
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public void ActiveInActive(int InstructorReplaceChangeRequestID, bool? InActive, int CurUserID)
        {

            SqlParameter[] PLead = new SqlParameter[3];
            PLead[0] = new SqlParameter("@InstructorReplaceChangeRequestID", InstructorReplaceChangeRequestID);
            PLead[1] = new SqlParameter("@InActive", InActive);
            PLead[2] = new SqlParameter("@CurUserID", CurUserID);
            SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AI_InstructorReplaceChangeRequest]", PLead);
        }
        private InstructorReplaceChangeRequestModel RowOfInstructorReplaceChangeRequest(DataRow r)
        {
            InstructorReplaceChangeRequestModel InstructorReplaceChangeRequest = new InstructorReplaceChangeRequestModel();
            InstructorReplaceChangeRequest.InstructorReplaceChangeRequestID = Convert.ToInt32(r["InstructorReplaceChangeRequestID"]);
            InstructorReplaceChangeRequest.IncepReportID = Convert.ToInt32(r["IncepReportID"]);
            InstructorReplaceChangeRequest.ClassID = Convert.ToInt32(r["ClassID"]);
            InstructorReplaceChangeRequest.ClassCode = r["ClassCode"].ToString();
           
            InstructorReplaceChangeRequest.InstrIDs = r["InstrIDs"].ToString();
            InstructorReplaceChangeRequest.InstructorName = r["InstructorName"].ToString();
            InstructorReplaceChangeRequest.SchemeName = r["SchemeName"].ToString();
            InstructorReplaceChangeRequest.TSPName = r["TSPName"].ToString();
            InstructorReplaceChangeRequest.TradeName = r["TradeName"].ToString();
            InstructorReplaceChangeRequest.InActive = Convert.ToBoolean(r["InActive"]);
            InstructorReplaceChangeRequest.CreatedUserID = Convert.ToInt32(r["CreatedUserID"]);
            InstructorReplaceChangeRequest.ModifiedUserID = Convert.ToInt32(r["ModifiedUserID"]);
            InstructorReplaceChangeRequest.CreatedDate = r["CreatedDate"].ToString().GetDate();
            InstructorReplaceChangeRequest.ModifiedDate = r["ModifiedDate"].ToString().GetDate();
            InstructorReplaceChangeRequest.IsApproved = Convert.ToBoolean(r["IsApproved"]);
            InstructorReplaceChangeRequest.IsRejected = Convert.ToBoolean(r["IsRejected"]);

            return InstructorReplaceChangeRequest;
        }
    }
}
