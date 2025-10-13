using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using DataLayer.Models;
using DataLayer.Interfaces;
namespace DataLayer.Services
{
    public class SRVTakamolRecommendationNote : SRVBase, ISRVTakamolRecommendationNote
    {
        public SRVTakamolRecommendationNote() { }
        public TakamolRecommendationNoteModel GetByTakamolRecommendationNoteId(int TakamolRecommendationNoteID)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@TakamolRecommendationNoteID", TakamolRecommendationNoteID));
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TakamolRecommendationNote", param.ToArray()).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    return RowOfTakamolRecommendationNote(dt.Rows[0]);
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        private List<TakamolRecommendationNoteModel> LoopinData(DataTable dt)
        {
            List<TakamolRecommendationNoteModel> TakamolRecommendationNoteL = new List<TakamolRecommendationNoteModel>();
            foreach (DataRow r in dt.Rows)
            {
                TakamolRecommendationNoteL.Add(RowOfTakamolRecommendationNote(r));
            }
            return TakamolRecommendationNoteL;
        }
        public List<TakamolRecommendationNoteModel> FetchTakamolRecommendationNote(TakamolRecommendationNoteModel mod)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                //param.Add(new SqlParameter("@SchemeId", TakamolRecommendationNote.TakamolRecommendationNoteId));
                //param.Add(new SqlParameter("@TspId", TakamolRecommendationNote.ReportId));
                //param.Add( new SqlParameter("@ClassId", TakamolRecommendationNote.TraineeId));
                param.Add(new SqlParameter("@Month", mod.Month));
                param.Add(new SqlParameter("@OID", mod.OID));
                param.Add(new SqlParameter("@KAMID", mod.KAMID));
                param.Add(new SqlParameter("@SchemeId", mod.SchemeID));
                //param.Add(new SqlParameter("@TspId", mod.TSPID));
                param.Add(new SqlParameter("@TSPMasterID", mod.TSPMasterID));
                param.Add(new SqlParameter("@UserID", mod.UserID));
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TakamolRecommendationNote", param.ToArray()).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<TakamolRecommendationNoteModel> FetchVRN(TakamolRecommendationNoteModel mod)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                //param.Add(new SqlParameter("@SchemeId", TakamolRecommendationNote.TakamolRecommendationNoteId));
                //param.Add(new SqlParameter("@TspId", TakamolRecommendationNote.ReportId));
                //param.Add( new SqlParameter("@ClassId", TakamolRecommendationNote.TraineeId));
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
        public List<TakamolRecommendationNoteModel> FetchTakamolRecommendationNote()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TakamolRecommendationNote").Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<TakamolRecommendationNoteModel> FetchTakamolRecommendationNote(bool InActive)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TakamolRecommendationNote", new SqlParameter("@InActive", InActive)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        //public List<TakamolRecommendationNoteModel> GetByTraineeId(int TraineeId)
        //{
        //    try
        //    {
        //        DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TakamolRecommendationNote", new SqlParameter("@TraineeId", TraineeId)).Tables[0];
        //        return LoopinData(dt);
        //    }
        //    catch (Exception ex) { throw new Exception(ex.Message); }
        //}
        public void ActiveInActive(int TakamolRecommendationNoteId, bool? InActive, int CurUserID)
        {
            SqlParameter[] PLead = new SqlParameter[3];
            PLead[0] = new SqlParameter("@TakamolRecommendationNoteId", TakamolRecommendationNoteId);
            PLead[1] = new SqlParameter("@InActive", InActive);
            PLead[2] = new SqlParameter("@CurUserID", CurUserID);
            SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AI_TakamolRecommendationNote]", PLead);
        }
        private TakamolRecommendationNoteModel RowOfTakamolRecommendationNote(DataRow row)
        {
            TakamolRecommendationNoteModel TakamolRecommendationNote = new TakamolRecommendationNoteModel();
            TakamolRecommendationNote.TakamolRecommendationNoteID = row.Field<int>("TakamolRecommendationNoteID");
            TakamolRecommendationNote.ClassID = row.Field<int>("ClassID");
            TakamolRecommendationNote.ClassCode = row.Field<string>("ClassCode");
            TakamolRecommendationNote.Month = row.Field<DateTime>("Month");
            TakamolRecommendationNote.ReportDate = row.Field<DateTime>("ReportDate");
            //TakamolRecommendationNote.NumberOfMonths = row.Field<int>("NumberOfMonths");
            TakamolRecommendationNote.IsApproved = row.Field<bool>("IsApproved");
            TakamolRecommendationNote.IsRejected = row.Field<bool>("IsRejected");
            TakamolRecommendationNote.Batch = row.Field<int>("Batch");
            TakamolRecommendationNote.TSPName = row.Field<string>("TSPName");
            TakamolRecommendationNote.TrainingDistrict = row.Field<string>("TrainingDistrict");
            TakamolRecommendationNote.TradeName = row.Field<string>("TradeName");
            TakamolRecommendationNote.SchemeID = row.Field<int>("SchemeID");
            TakamolRecommendationNote.SchemeName = row.Field<string>("SchemeName");
            TakamolRecommendationNote.SchemeCode = row.Field<string>("SchemeCode");
            TakamolRecommendationNote.ApprovalBatchNo = row.Field<int>("ApprovalBatchNo");
            TakamolRecommendationNote.CreatedUserID = row.Field<int>("CreatedUserID");
            TakamolRecommendationNote.CreatedDate = row.Field<DateTime?>("CreatedDate"); ;
            TakamolRecommendationNote.ModifiedUserID = row.Field<int>("ModifiedUserID");
            TakamolRecommendationNote.ModifiedDate = row.Field<DateTime?>("ModifiedDate"); ;
            TakamolRecommendationNote.InActive = row.Field<bool?>("InActive");
            if (row.Table.Columns.Contains("ProcessKey"))
            {
                TakamolRecommendationNote.ProcessKey = row.Field<string>("ProcessKey");
            }
            return TakamolRecommendationNote;
        }
        public bool TakamolRecommendationNoteApproveReject(TakamolRecommendationNoteModel model, SqlTransaction transaction = null)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@TakamolRecommendationNoteID", model.TakamolRecommendationNoteID));
                param.Add(new SqlParameter("@IsApproved", model.IsApproved));
                param.Add(new SqlParameter("@IsRejected", model.IsRejected));
                param.Add(new SqlParameter("@CurUserID", model.CurUserID));
                if (transaction != null)
                {
                    SqlHelper.ExecuteScalar(transaction, CommandType.StoredProcedure, "U_TakamolRecommendationNoteApproveReject", param.ToArray());
                }
                else
                {
                    SqlHelper.ExecuteScalar(SqlHelper.GetCon(), CommandType.StoredProcedure, "U_TakamolRecommendationNoteApproveReject", param.ToArray());
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
        public void GenerateInvoiceHeader_TakamolRecommendationNote(int PoHeaderID, SqlTransaction _transaction, string ProcessKey)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@POHeaderID", PoHeaderID);
                param[1] = new SqlParameter("@ProcessKey", ProcessKey);
                if (_transaction != null)
                    SqlHelper.ExecuteNonQuery(_transaction, CommandType.StoredProcedure, "GenerateInvoiceHeader_TakamolRecommendationNote", param);
                else
                    SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "GenerateInvoiceHeader_TakamolRecommendationNote", param);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
    }
}
