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
    public class SRVSRN : SRVBase, ISRVSRN
    {
        public SRVSRN() { }
        public SRNModel GetBySrnId(int SRNID)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@SRNID", SRNID));
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_SRN", param.ToArray()).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    return RowOfSRN(dt.Rows[0]);
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        private List<SRNModel> LoopinData(DataTable dt)
        {
            List<SRNModel> SRNL = new List<SRNModel>();

            foreach (DataRow r in dt.Rows)
            {
                SRNL.Add(RowOfSRN(r));

            }
            return SRNL;
        }
        public List<SRNModel> FetchSRN(SRNModel mod)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                //param.Add(new SqlParameter("@SchemeId", SRN.SrnId));
                //param.Add(new SqlParameter("@TspId", SRN.ReportId));
                //param.Add( new SqlParameter("@ClassId", SRN.TraineeId));
                param.Add(new SqlParameter("@Month", mod.Month));
                param.Add(new SqlParameter("@OID", mod.OID));
                param.Add(new SqlParameter("@KAMID", mod.KAMID));
                param.Add(new SqlParameter("@SchemeId", mod.SchemeID));
                //param.Add(new SqlParameter("@TspId", mod.TSPID));
                param.Add(new SqlParameter("@TSPMasterID", mod.TSPMasterID));
                param.Add(new SqlParameter("@UserID", mod.UserID));
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_SRN", param.ToArray()).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<SRNModel> FetchSRN()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_SRN").Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public List<SRNModel> FetchSRN(bool InActive)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_SRN", new SqlParameter("@InActive", InActive)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        //public List<SRNModel> GetByTraineeId(int TraineeId)
        //{
        //    try
        //    {
        //        DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_SRN", new SqlParameter("@TraineeId", TraineeId)).Tables[0];
        //        return LoopinData(dt);
        //    }
        //    catch (Exception ex) { throw new Exception(ex.Message); }
        //}
        public void ActiveInActive(int SrnId, bool? InActive, int CurUserID)
        {

            SqlParameter[] PLead = new SqlParameter[3];
            PLead[0] = new SqlParameter("@SrnId", SrnId);
            PLead[1] = new SqlParameter("@InActive", InActive);
            PLead[2] = new SqlParameter("@CurUserID", CurUserID);
            SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AI_SRN]", PLead);
        }
        private SRNModel RowOfSRN(DataRow row)
        {
            SRNModel SRN = new SRNModel();
            SRN.SRNID = row.Field<int>("SRNID");
            SRN.ClassID = row.Field<int>("ClassID");
            SRN.ClassCode = row.Field<string>("ClassCode");
            SRN.Month = row.Field<DateTime>("Month");
            SRN.ReportDate = row.Field<DateTime>("ReportDate");
            //SRN.NumberOfMonths = row.Field<int>("NumberOfMonths");
            SRN.IsApproved = row.Field<bool>("IsApproved");
            SRN.IsRejected = row.Field<bool>("IsRejected");
            SRN.Batch = row.Field<int>("Batch");
            SRN.TSPName = row.Field<string>("TSPName");
            SRN.TrainingDistrict = row.Field<string>("TrainingDistrict");
            SRN.TradeName = row.Field<string>("TradeName");
            SRN.SchemeID = row.Field<int>("SchemeID");
            SRN.SchemeName = row.Field<string>("SchemeName");
            SRN.SchemeCode = row.Field<string>("SchemeCode");
            SRN.ApprovalBatchNo = row.Field<int>("ApprovalBatchNo");

            SRN.CreatedUserID = row.Field<int>("CreatedUserID");
            SRN.CreatedDate = row.Field<DateTime?>("CreatedDate"); ;
            SRN.ModifiedUserID = row.Field<int>("ModifiedUserID");
            SRN.ModifiedDate = row.Field<DateTime?>("ModifiedDate"); ;
            SRN.InActive = row.Field<bool?>("InActive");

            
           if (row.Table.Columns.Contains("ProcessKey"))
            {
                SRN.ProcessKey = row.Field<string>("ProcessKey");
            }

            return SRN;
        }

        public bool SRNApproveReject(SRNModel model, SqlTransaction transaction = null)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@SRNID", model.SRNID));
                param.Add(new SqlParameter("@IsApproved", model.IsApproved));
                param.Add(new SqlParameter("@IsRejected", model.IsRejected));
                param.Add(new SqlParameter("@CurUserID", model.CurUserID));

                if (transaction != null)
                {
                    SqlHelper.ExecuteScalar(transaction, CommandType.StoredProcedure, "U_SRNApproveReject", param.ToArray());
                }
                else
                {
                    SqlHelper.ExecuteScalar(SqlHelper.GetCon(), CommandType.StoredProcedure, "U_SRNApproveReject", param.ToArray());
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
        public void GenerateInvoiceHeader_SRN(int PoHeaderID, SqlTransaction _transaction, string ProcessKey)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@POHeaderID", PoHeaderID);
                param[1] = new SqlParameter("@ProcessKey", ProcessKey);

                if (_transaction != null)
                    SqlHelper.ExecuteNonQuery(_transaction, CommandType.StoredProcedure, "GenerateInvoiceHeader_SRN", param);
                else
                    SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "GenerateInvoiceHeader_SRN", param);

            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
    }
}
