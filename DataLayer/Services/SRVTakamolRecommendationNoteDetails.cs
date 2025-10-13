using DataLayer.Interfaces;
using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;

namespace DataLayer.Services
{
    public class SRVTakamolRecommendationNoteDetails : SRVBase, ISRVTakamolRecommendationNoteDetails
    {
        public List<TakamolRecommendationNoteDetailsModel> FetchTakamolRecommendationNoteDetails(TakamolRecommendationNoteDetailsModel model)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@TakamolRecommendationNoteID", model.TakamolRecommendationNoteID));
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TakamolRecommendationNoteDetails", param.ToArray()).Tables[0];
                return LoopinTakamolRecommendationNoteDetails(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<TakamolRecommendationNoteDetailsModel> GetTakamolRecommendationNoteExcelExportByIDs(string ids)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TakamolRecommendationNoteDetails_By_IDs", new SqlParameter("@TakamolRecommendationNoteMasterIDs", ids)).Tables[0];
            return LoopinTakamolRecommendationNoteDetails(dt);
        }
        public List<TakamolRecommendationNoteDetailsModel> FetchTakamolRecommendationNoteDetailsFiltered(TakamolRecommendationNoteDetailsModel model)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@TakamolRecommendationNoteID", model.TakamolRecommendationNoteID));//temp
                                                                     //param.AddRange(Common.GetPagingParams(model));
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TakamolRecommendationNoteDetailsFiltered", param.ToArray()).Tables[0];
                return LoopinTakamolRecommendationNoteDetails(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        private List<TakamolRecommendationNoteDetailsModel> LoopinTakamolRecommendationNoteDetails(DataTable dt, bool ForExcel = false)
        {
            List<TakamolRecommendationNoteDetailsModel> TakamolRecommendationNoteModel = new List<TakamolRecommendationNoteDetailsModel>();
            foreach (DataRow r in dt.Rows)
            {
                TakamolRecommendationNoteModel.Add(RowOfTakamolRecommendationNoteDetails(r, ForExcel));
            }
            return TakamolRecommendationNoteModel;
        }
        public List<TakamolRecommendationNoteDetailsModel> GetTakamolRecommendationNoteExcelExport(TakamolRecommendationNoteDetailsModel model)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@TakamolRecommendationNoteID", model.TakamolRecommendationNoteID));
                param.Add(new SqlParameter("@month", model.Month));
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "TakamolRecommendationNote_Excel_Export", param.ToArray()).Tables[0];
                return LoopinTakamolRecommendationNoteDetails(dt, true); // ForExcel = true
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public TakamolRecommendationNoteDetailsModel RowOfTakamolRecommendationNoteDetails(DataRow row, bool ForExcel = false)
        {
            TakamolRecommendationNoteDetailsModel model = new TakamolRecommendationNoteDetailsModel();
            model.TakamolRecommendationNoteID = row.Field<int>("TakamolRecommendationNoteID");
            model.ReportId = row.Field<string>("ReportId");
            model.Amount = row.Field<decimal>("Amount");
            model.TokenNumber = row.Field<string>("TokenNumber");
            model.TransactionNumber = row.Field<string>("TransactionNumber");
            model.Comments = row.Field<string>("Comments");
            model.IsPaid = row.Field<bool>("IsPaid");
            model.IsVarified = row.Field<bool?>("IsVarified");
            model.TraineeName = row.Field<string>("TraineeName");
            if (row.Table.Columns.Contains("FundingCategory"))
            {
                model.TSPName = row.Field<string>("TSPName");
                model.SchemeName = row.Field<string>("SchemeName");
                model.FundingCategory = row.Field<string>("FundingCategory");
                model.ClassStartDate = row.Field<string>("ClassStartDate");
                model.ClassEndDate = row.Field<string>("ClassEndDate");
            }
            if (ForExcel)
            {
                model.TSPName = row.Field<string>("TSPName");
                model.ClassCode = row.Field<string>("ClassCode");
                model.Month = row.Field<DateTime?>("Month");
            }
            else
            {
                //================Azhar iqbal========================
                if (row.Table.Columns.Contains("Project"))
                {
                    model.ProjectName = row.Field<string>("Project");
                    model.SchemeName = row.Field<string>("Scheme");
                    model.TSPNameTakamolRecommendationNoteDetail = row.Field<string>("TSP");
                    model.ClassCodeTakamolRecommendationNoteDetail = row.Field<string>("Classcode");
                    model.ClassStartdateTakamolRecommendationNoteDetail = row.Field<string>("Classstartdate");
                    model.ClassEnddateTakamolRecommendationNoteDetail = row.Field<string>("Classenddate");
                }
                //model.ClassStartdateTakamolRecommendationNoteDetail = row["Classstartdate"].ToString().GetDate();
                //model.ClassEnddateTakamolRecommendationNoteDetail = row["Classenddate"].ToString().GetDate();
                //model.ClassStartdateTakamolRecommendationNoteDetail = row["ClassStartDate"].ToString().GetDate();
                //model.ClassEnddateTakamolRecommendationNoteDetail = row["ClassEndDate"].ToString().GetDate();
                //====================================================
                model.TraineeCode = row.Field<string>("TraineeCode");
                model.TraineeCNIC = row.Field<string>("TraineeCNIC");
                model.FatherName = row.Field<string>("FatherName");
                model.ContactNumber1 = row.Field<string>("ContactNumber1");
                model.DistrictName = row.Field<string>("DistrictName");
            }
            return model;
        }
        public TakamolRecommendationNoteDetailsModel UpdateTakamolRecommendationNoteDetails(TakamolRecommendationNoteDetailsModel TakamolRecommendationNote)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[12];
                param[0] = new SqlParameter("@TakamolRecommendationNoteID", TakamolRecommendationNote.TakamolRecommendationNoteID);
                param[1] = new SqlParameter("@ReportId", TakamolRecommendationNote.ReportId);
                //param[2] = new SqlParameter("@TraineeId", TakamolRecommendationNote.TraineeId);
                //param[3] = new SqlParameter("@Amount", TakamolRecommendationNote.Amount);
                param[4] = new SqlParameter("@TokenNumber", TakamolRecommendationNote.TokenNumber);
                param[5] = new SqlParameter("@TransactionNumber", TakamolRecommendationNote.TransactionNumber);
                param[6] = new SqlParameter("@Comments", TakamolRecommendationNote.Comments);
                param[7] = new SqlParameter("@IsPaid", TakamolRecommendationNote.IsPaid);
                //param[8] = new SqlParameter("@IsVarified", TakamolRecommendationNote.IsVarified);
                //param[9] = new SqlParameter("@Month", TakamolRecommendationNote.Month);
                //param[10] = new SqlParameter("@NumberOfMonths", TakamolRecommendationNote.NumberOfMonths);
                param[11] = new SqlParameter("@CurUserID", TakamolRecommendationNote.CurUserID);
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[U_TakamolRecommendationNoteDetails]", param);
                return TakamolRecommendationNote;
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }
    }
}
