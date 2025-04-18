using DataLayer.Interfaces;
using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;

namespace DataLayer.Services
{
    public class SRVOTRNDetails : SRVBase, ISRVOTRNDetails
    {
        public List<OTRNDetailsModel> FetchOTRNDetails(OTRNDetailsModel model)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@OTRNID", model.OTRNID));
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_OTRNDetails", param.ToArray()).Tables[0];
                return LoopinOTRNDetails(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<OTRNDetailsModel> GetOTRNExcelExportByIDs(string ids)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_OTRNDetails_By_IDs", new SqlParameter("@OTRNMasterIDs", ids)).Tables[0];
            return LoopinOTRNDetails(dt);
        }
        public List<OTRNDetailsModel> FetchOTRNDetailsFiltered(OTRNDetailsModel model)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@OTRNID", model.OTRNID));//temp
                                                                     //param.AddRange(Common.GetPagingParams(model));
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_OTRNDetailsFiltered", param.ToArray()).Tables[0];
                return LoopinOTRNDetails(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        private List<OTRNDetailsModel> LoopinOTRNDetails(DataTable dt, bool ForExcel = false)
        {
            List<OTRNDetailsModel> OTRNModel = new List<OTRNDetailsModel>();
            foreach (DataRow r in dt.Rows)
            {
                OTRNModel.Add(RowOfOTRNDetails(r, ForExcel));
            }
            return OTRNModel;
        }
        public List<OTRNDetailsModel> GetOTRNExcelExport(OTRNDetailsModel model)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@OTRNID", model.OTRNID));
                param.Add(new SqlParameter("@month", model.Month));
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "OTRN_Excel_Export", param.ToArray()).Tables[0];
                return LoopinOTRNDetails(dt, true); // ForExcel = true
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public OTRNDetailsModel RowOfOTRNDetails(DataRow row, bool ForExcel = false)
        {
            OTRNDetailsModel model = new OTRNDetailsModel();
            model.OTRNID = row.Field<int>("OTRNID");
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
                    model.TSPNameOTRNDetail = row.Field<string>("TSP");
                    model.ClassCodeOTRNDetail = row.Field<string>("Classcode");
                    model.ClassStartdateOTRNDetail = row.Field<string>("Classstartdate");
                    model.ClassEnddateOTRNDetail = row.Field<string>("Classenddate");
                }
                //model.ClassStartdateOTRNDetail = row["Classstartdate"].ToString().GetDate();
                //model.ClassEnddateOTRNDetail = row["Classenddate"].ToString().GetDate();
                //model.ClassStartdateOTRNDetail = row["ClassStartDate"].ToString().GetDate();
                //model.ClassEnddateOTRNDetail = row["ClassEndDate"].ToString().GetDate();
                //====================================================
                model.TraineeCode = row.Field<string>("TraineeCode");
                model.TraineeCNIC = row.Field<string>("TraineeCNIC");
                model.FatherName = row.Field<string>("FatherName");
                model.ContactNumber1 = row.Field<string>("ContactNumber1");
                model.DistrictName = row.Field<string>("DistrictName");
            }
            return model;
        }
        public OTRNDetailsModel UpdateOTRNDetails(OTRNDetailsModel OTRN)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[12];
                param[0] = new SqlParameter("@OTRNID", OTRN.OTRNID);
                param[1] = new SqlParameter("@ReportId", OTRN.ReportId);
                //param[2] = new SqlParameter("@TraineeId", OTRN.TraineeId);
                //param[3] = new SqlParameter("@Amount", OTRN.Amount);
                param[4] = new SqlParameter("@TokenNumber", OTRN.TokenNumber);
                param[5] = new SqlParameter("@TransactionNumber", OTRN.TransactionNumber);
                param[6] = new SqlParameter("@Comments", OTRN.Comments);
                param[7] = new SqlParameter("@IsPaid", OTRN.IsPaid);
                //param[8] = new SqlParameter("@IsVarified", OTRN.IsVarified);
                //param[9] = new SqlParameter("@Month", OTRN.Month);
                //param[10] = new SqlParameter("@NumberOfMonths", OTRN.NumberOfMonths);
                param[11] = new SqlParameter("@CurUserID", OTRN.CurUserID);
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[U_OTRNDetails]", param);
                return OTRN;
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }
    }
}
