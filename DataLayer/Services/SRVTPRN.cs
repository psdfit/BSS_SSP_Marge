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
    public class SRVTPRN : SRVBase, ISRVTPRN
    {
        public SRVTPRN() { }
        public TPRNModel GetByTPRNId(int TPRNID)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@TPRNID", TPRNID));
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TPRN", param.ToArray()).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    return RowOfTPRN(dt.Rows[0]);
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        private List<TPRNModel> LoopinData(DataTable dt)
        {
            List<TPRNModel> TPRNL = new List<TPRNModel>();

            foreach (DataRow r in dt.Rows)
            {
                TPRNL.Add(RowOfTPRN(r));

            }
            return TPRNL;
        }
        public List<TPRNModel> FetchTPRN(TPRNModel mod)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                //param.Add(new SqlParameter("@SchemeId", TPRN.TPRNId));
                //param.Add(new SqlParameter("@TspId", TPRN.ReportId));
                //param.Add( new SqlParameter("@ClassId", TPRN.TraineeId));
                param.Add(new SqlParameter("@Month", mod.Month));
                param.Add(new SqlParameter("@OID", mod.OID));
                param.Add(new SqlParameter("@KAMID", mod.KAMID));
                param.Add(new SqlParameter("@SchemeId", mod.SchemeID));
                //param.Add(new SqlParameter("@TspId", mod.TSPID));
                param.Add(new SqlParameter("@TSPMasterID", mod.TSPMasterID));
                param.Add(new SqlParameter("@UserID", mod.UserID));
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TPRN", param.ToArray()).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<TPRNModel> FetchVRN(TPRNModel mod)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                //param.Add(new SqlParameter("@SchemeId", TPRN.TPRNId));
                //param.Add(new SqlParameter("@TspId", TPRN.ReportId));
                //param.Add( new SqlParameter("@ClassId", TPRN.TraineeId));
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
        public List<TPRNModel> FetchTPRN()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TPRN").Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public List<TPRNModel> FetchTPRN(bool InActive)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TPRN", new SqlParameter("@InActive", InActive)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        //public List<TPRNModel> GetByTraineeId(int TraineeId)
        //{
        //    try
        //    {
        //        DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TPRN", new SqlParameter("@TraineeId", TraineeId)).Tables[0];
        //        return LoopinData(dt);
        //    }
        //    catch (Exception ex) { throw new Exception(ex.Message); }
        //}
        public void ActiveInActive(int TPRNId, bool? InActive, int CurUserID)
        {

            SqlParameter[] PLead = new SqlParameter[3];
            PLead[0] = new SqlParameter("@TPRNId", TPRNId);
            PLead[1] = new SqlParameter("@InActive", InActive);
            PLead[2] = new SqlParameter("@CurUserID", CurUserID);
            SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AI_TPRN]", PLead);
        }
        private TPRNModel RowOfTPRN(DataRow row)
        {
            TPRNModel TPRN = new TPRNModel();
            TPRN.TPRNID = row.Field<int>("TPRNID");
            TPRN.ClassID = row.Field<int>("ClassID");
            TPRN.ClassCode = row.Field<string>("ClassCode");
            TPRN.Month = row.Field<DateTime>("Month");
            TPRN.ReportDate = row.Field<DateTime>("ReportDate");
            //TPRN.NumberOfMonths = row.Field<int>("NumberOfMonths");
            TPRN.IsApproved = row.Field<bool>("IsApproved");
            TPRN.IsRejected = row.Field<bool>("IsRejected");
            TPRN.Batch = row.Field<int>("Batch");
            TPRN.TSPName = row.Field<string>("TSPName");
            TPRN.TrainingDistrict = row.Field<string>("TrainingDistrict");
            TPRN.TradeName = row.Field<string>("TradeName");
            TPRN.SchemeID = row.Field<int>("SchemeID");
            TPRN.SchemeName = row.Field<string>("SchemeName");
            TPRN.SchemeCode = row.Field<string>("SchemeCode");
            TPRN.ApprovalBatchNo = row.Field<int>("ApprovalBatchNo");

            TPRN.CreatedUserID = row.Field<int>("CreatedUserID");
            TPRN.CreatedDate = row.Field<DateTime?>("CreatedDate"); ;
            TPRN.ModifiedUserID = row.Field<int>("ModifiedUserID");
            TPRN.ModifiedDate = row.Field<DateTime?>("ModifiedDate"); ;
            TPRN.InActive = row.Field<bool?>("InActive");

            
           if (row.Table.Columns.Contains("ProcessKey"))
            {
                TPRN.ProcessKey = row.Field<string>("ProcessKey");
            }

            return TPRN;
        }

        public bool TPRNApproveReject(TPRNModel model, SqlTransaction transaction = null)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@TPRNID", model.TPRNID));
                param.Add(new SqlParameter("@IsApproved", model.IsApproved));
                param.Add(new SqlParameter("@IsRejected", model.IsRejected));
                param.Add(new SqlParameter("@CurUserID", model.CurUserID));

                if (transaction != null)
                {
                    SqlHelper.ExecuteScalar(transaction, CommandType.StoredProcedure, "U_TPRNApproveReject", param.ToArray());
                }
                else
                {
                    SqlHelper.ExecuteScalar(SqlHelper.GetCon(), CommandType.StoredProcedure, "U_TPRNApproveReject", param.ToArray());
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
        public void GenerateInvoiceHeader_TPRN(int PoHeaderID, SqlTransaction _transaction, string ProcessKey)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@POHeaderID", PoHeaderID);
                param[1] = new SqlParameter("@ProcessKey", ProcessKey);

                if (_transaction != null)
                    SqlHelper.ExecuteNonQuery(_transaction, CommandType.StoredProcedure, "GenerateInvoiceHeader_TPRN", param);
                else
                    SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "GenerateInvoiceHeader_TPRN", param);

            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
    }
}
