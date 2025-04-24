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
    public class SRVGURN : SRVBase, ISRVGURN
    {
        public SRVGURN() { }
        public GURNModel GetByGurnId(int GURNID)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@GURNID", GURNID));
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_GURN", param.ToArray()).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    return RowOfGURN(dt.Rows[0]);
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        private List<GURNModel> LoopinData(DataTable dt)
        {
            List<GURNModel> GURNL = new List<GURNModel>();

            foreach (DataRow r in dt.Rows)
            {
                GURNL.Add(RowOfGURN(r));

            }
            return GURNL;
        }
        public List<GURNModel> FetchGURN(GURNModel mod)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                //param.Add(new SqlParameter("@SchemeId", GURN.GurnId));
                //param.Add(new SqlParameter("@TspId", GURN.ReportId));
                //param.Add( new SqlParameter("@ClassId", GURN.TraineeId));
                param.Add(new SqlParameter("@Month", mod.Month));
                param.Add(new SqlParameter("@OID", mod.OID));
                param.Add(new SqlParameter("@KAMID", mod.KAMID));
                param.Add(new SqlParameter("@SchemeId", mod.SchemeID));
                //param.Add(new SqlParameter("@TspId", mod.TSPID));
                param.Add(new SqlParameter("@TSPMasterID", mod.TSPMasterID));
                param.Add(new SqlParameter("@UserID", mod.UserID));
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_GURN", param.ToArray()).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<GURNModel> FetchVRN(GURNModel mod)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                //param.Add(new SqlParameter("@SchemeId", GURN.GurnId));
                //param.Add(new SqlParameter("@TspId", GURN.ReportId));
                //param.Add( new SqlParameter("@ClassId", GURN.TraineeId));
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
        public List<GURNModel> FetchGURN()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_GURN").Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public List<GURNModel> FetchGURN(bool InActive)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_GURN", new SqlParameter("@InActive", InActive)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        //public List<GURNModel> GetByTraineeId(int TraineeId)
        //{
        //    try
        //    {
        //        DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_GURN", new SqlParameter("@TraineeId", TraineeId)).Tables[0];
        //        return LoopinData(dt);
        //    }
        //    catch (Exception ex) { throw new Exception(ex.Message); }
        //}
        public void ActiveInActive(int GurnId, bool? InActive, int CurUserID)
        {

            SqlParameter[] PLead = new SqlParameter[3];
            PLead[0] = new SqlParameter("@GurnId", GurnId);
            PLead[1] = new SqlParameter("@InActive", InActive);
            PLead[2] = new SqlParameter("@CurUserID", CurUserID);
            SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AI_GURN]", PLead);
        }
        private GURNModel RowOfGURN(DataRow row)
        {
            GURNModel GURN = new GURNModel();
            GURN.GURNID = row.Field<int>("GURNID");
            GURN.ClassID = row.Field<int>("ClassID");
            GURN.ClassCode = row.Field<string>("ClassCode");
            GURN.Month = row.Field<DateTime>("Month");
            GURN.ReportDate = row.Field<DateTime>("ReportDate");
            //GURN.NumberOfMonths = row.Field<int>("NumberOfMonths");
            GURN.IsApproved = row.Field<bool?>("IsApproved");  // Nullable bool
            GURN.IsRejected = row.Field<bool?>("IsRejected");  // Nullable bool
            GURN.Batch = row.Field<int>("Batch");
            GURN.TSPName = row.Field<string>("TSPName");
            GURN.TrainingDistrict = row.Field<string>("TrainingDistrict");
            GURN.TradeName = row.Field<string>("TradeName");
            GURN.SchemeID = row.Field<int>("SchemeID");
            GURN.SchemeName = row.Field<string>("SchemeName");
            GURN.SchemeCode = row.Field<string>("SchemeCode");
            GURN.ApprovalBatchNo = row.Field<int>("ApprovalBatchNo");

            GURN.CreatedUserID = row.Field<int>("CreatedUserID");
            GURN.CreatedDate = row.Field<DateTime?>("CreatedDate");
            GURN.ModifiedUserID = row.Field<int>("ModifiedUserID");
            GURN.ModifiedDate = row.Field<DateTime?>("ModifiedDate");
            GURN.InActive = row.Field<bool?>("InActive");  // Nullable bool

            if (row.Table.Columns.Contains("ProcessKey"))
            {
                GURN.ProcessKey = row.Field<string>("ProcessKey");
            }

            return GURN;
        }


        public bool GURNApproveReject(GURNModel model, SqlTransaction transaction = null)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@GURNID", model.GURNID));
                param.Add(new SqlParameter("@IsApproved", model.IsApproved));
                param.Add(new SqlParameter("@IsRejected", model.IsRejected));
                param.Add(new SqlParameter("@CurUserID", model.CurUserID));

                if (transaction != null)
                {
                    SqlHelper.ExecuteScalar(transaction, CommandType.StoredProcedure, "U_GURNApproveReject", param.ToArray());
                }
                else
                {
                    SqlHelper.ExecuteScalar(SqlHelper.GetCon(), CommandType.StoredProcedure, "U_GURNApproveReject", param.ToArray());
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
        public void GenerateInvoiceHeader_GURN(int PoHeaderID, SqlTransaction _transaction, string ProcessKey)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@POHeaderID", PoHeaderID);
                param[1] = new SqlParameter("@ProcessKey", ProcessKey);

                if (_transaction != null)
                    SqlHelper.ExecuteNonQuery(_transaction, CommandType.StoredProcedure, "GenerateInvoiceHeader_GURN", param);
                else
                    SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "GenerateInvoiceHeader_GURN", param);

            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
    }
}
