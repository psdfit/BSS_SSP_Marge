using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using DataLayer.Models;
using DataLayer.Interfaces;
namespace DataLayer.Services
{
    public class SRVOTRN : SRVBase, ISRVOTRN
    {
        public SRVOTRN() { }
        public OTRNModel GetByOTRNId(int OTRNID)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@OTRNID", OTRNID));
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_OTRN", param.ToArray()).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    return RowOfOTRN(dt.Rows[0]);
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        private List<OTRNModel> LoopinData(DataTable dt)
        {
            List<OTRNModel> OTRNL = new List<OTRNModel>();
            foreach (DataRow r in dt.Rows)
            {
                OTRNL.Add(RowOfOTRN(r));
            }
            return OTRNL;
        }
        public List<OTRNModel> FetchOTRN(OTRNModel mod)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                //param.Add(new SqlParameter("@SchemeId", OTRN.OTRNId));
                //param.Add(new SqlParameter("@TspId", OTRN.ReportId));
                //param.Add( new SqlParameter("@ClassId", OTRN.TraineeId));
                param.Add(new SqlParameter("@Month", mod.Month));
                param.Add(new SqlParameter("@OID", mod.OID));
                param.Add(new SqlParameter("@KAMID", mod.KAMID));
                param.Add(new SqlParameter("@SchemeId", mod.SchemeID));
                //param.Add(new SqlParameter("@TspId", mod.TSPID));
                param.Add(new SqlParameter("@TSPMasterID", mod.TSPMasterID));
                param.Add(new SqlParameter("@UserID", mod.UserID));
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_OTRN", param.ToArray()).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<OTRNModel> FetchVRN(OTRNModel mod)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                //param.Add(new SqlParameter("@SchemeId", OTRN.OTRNId));
                //param.Add(new SqlParameter("@TspId", OTRN.ReportId));
                //param.Add( new SqlParameter("@ClassId", OTRN.TraineeId));
                param.Add(new SqlParameter("@Month", mod.Month));
                param.Add(new SqlParameter("@OID", mod.OID));
                param.Add(new SqlParameter("@KAMID", mod.KAMID));
                param.Add(new SqlParameter("@SchemeId", mod.SchemeID));
                //param.Add(new SqlParameter("@TspId", mod.TSPID));
                param.Add(new SqlParameter("@TSPMasterID", mod.TSPMasterID));
                param.Add(new SqlParameter("@UserID", mod.UserID));
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_VRN", param.ToArray()).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<OTRNModel> FetchOTRN()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_OTRN").Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<OTRNModel> FetchOTRN(bool InActive)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_OTRN", new SqlParameter("@InActive", InActive)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        //public List<OTRNModel> GetByTraineeId(int TraineeId)
        //{
        //    try
        //    {
        //        DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_OTRN", new SqlParameter("@TraineeId", TraineeId)).Tables[0];
        //        return LoopinData(dt);
        //    }
        //    catch (Exception ex) { throw new Exception(ex.Message); }
        //}
        public void ActiveInActive(int OTRNId, bool? InActive, int CurUserID)
        {
            SqlParameter[] PLead = new SqlParameter[3];
            PLead[0] = new SqlParameter("@OTRNId", OTRNId);
            PLead[1] = new SqlParameter("@InActive", InActive);
            PLead[2] = new SqlParameter("@CurUserID", CurUserID);
            SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AI_OTRN]", PLead);
        }
        private OTRNModel RowOfOTRN(DataRow row)
        {
            OTRNModel OTRN = new OTRNModel();
            OTRN.OTRNID = row.Field<int>("OTRNID");
            OTRN.ClassID = row.Field<int>("ClassID");
            OTRN.ClassCode = row.Field<string>("ClassCode");
            OTRN.Month = row.Field<DateTime>("Month");
            OTRN.ReportDate = row.Field<DateTime>("ReportDate");
            //OTRN.NumberOfMonths = row.Field<int>("NumberOfMonths");
            OTRN.IsApproved = row.Field<bool>("IsApproved");
            OTRN.IsRejected = row.Field<bool>("IsRejected");
            OTRN.Batch = row.Field<int>("Batch");
            OTRN.TSPName = row.Field<string>("TSPName");
            OTRN.TrainingDistrict = row.Field<string>("TrainingDistrict");
            OTRN.TradeName = row.Field<string>("TradeName");
            OTRN.SchemeID = row.Field<int>("SchemeID");
            OTRN.SchemeName = row.Field<string>("SchemeName");
            OTRN.SchemeCode = row.Field<string>("SchemeCode");
            OTRN.ApprovalBatchNo = row.Field<int>("ApprovalBatchNo");
            OTRN.CreatedUserID = row.Field<int>("CreatedUserID");
            OTRN.CreatedDate = row.Field<DateTime?>("CreatedDate"); ;
            OTRN.ModifiedUserID = row.Field<int>("ModifiedUserID");
            OTRN.ModifiedDate = row.Field<DateTime?>("ModifiedDate"); ;
            OTRN.InActive = row.Field<bool?>("InActive");
            if (row.Table.Columns.Contains("ProcessKey"))
            {
                OTRN.ProcessKey = row.Field<string>("ProcessKey");
            }
            return OTRN;
        }
        public bool OTRNApproveReject(OTRNModel model, SqlTransaction transaction = null)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@OTRNID", model.OTRNID));
                param.Add(new SqlParameter("@IsApproved", model.IsApproved));
                param.Add(new SqlParameter("@IsRejected", model.IsRejected));
                param.Add(new SqlParameter("@CurUserID", model.CurUserID));
                if (transaction != null)
                {
                    SqlHelper.ExecuteScalar(transaction, CommandType.StoredProcedure, "U_OTRNApproveReject", param.ToArray());
                }
                else
                {
                    SqlHelper.ExecuteScalar(SqlHelper.GetCon(), CommandType.StoredProcedure, "U_OTRNApproveReject", param.ToArray());
                }
                return true;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public bool TRNApproveReject(TRNMasterModel model, SqlTransaction transaction = null)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@TRNMasterID", model.TRNMasterID));
                param.Add(new SqlParameter("@IsApproved", model.IsApproved));
                param.Add(new SqlParameter("@IsRejected", model.IsRejected));
                param.Add(new SqlParameter("@CurUserID", model.ModifiedUserID));
                if (transaction != null)
                {
                    SqlHelper.ExecuteScalar(transaction, CommandType.StoredProcedure, "U_TRNApproveReject", param.ToArray());
                }
                else
                {
                    SqlHelper.ExecuteScalar(SqlHelper.GetCon(), CommandType.StoredProcedure, "U_TRNApproveReject", param.ToArray());
                }
                return true;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public bool PO_TRNApproveReject(POHeaderModel model, SqlTransaction transaction = null)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@PoHeaderID", model.POHeaderID));
                param.Add(new SqlParameter("@IsApproved", model.IsApproved));
                param.Add(new SqlParameter("@IsRejected", model.IsRejected));
                param.Add(new SqlParameter("@CurUserID", model.ModifiedUserID));
                if (transaction != null)
                {
                    SqlHelper.ExecuteScalar(transaction, CommandType.StoredProcedure, "U_PO_TRNApproveReject", param.ToArray());
                }
                else
                {
                    SqlHelper.ExecuteScalar(SqlHelper.GetCon(), CommandType.StoredProcedure, "U_PO_TRNApproveReject", param.ToArray());
                }
                return true;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public void GenerateInvoiceHeader_OTRN(int PoHeaderID, SqlTransaction _transaction, string ProcessKey)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@POHeaderID", PoHeaderID);
                param[1] = new SqlParameter("@ProcessKey", ProcessKey);
                if (_transaction != null)
                    SqlHelper.ExecuteNonQuery(_transaction, CommandType.StoredProcedure, "GenerateInvoiceHeader_OTRN", param);
                else
                    SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "GenerateInvoiceHeader_OTRN", param);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
    }
}
