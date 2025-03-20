using DataLayer.Interfaces;
using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;

namespace DataLayer.Services
{
    public class SRVPCRNDetails : SRVBase, ISRVPCRNDetails
    {
        public List<PCRNDetailsModel> FetchPCRNDetails(PCRNDetailsModel model)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@PCRNID", model.PCRNID));
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_PCRNDetails", param.ToArray()).Tables[0];
                return LoopinPCRNDetails(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<PCRNDetailsModel> GetPCRNExcelExportByIDs(string ids)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_PCRNDetails_By_IDs", new SqlParameter("@PCRNMasterIDs", ids)).Tables[0];
            return LoopinPCRNDetails(dt);
        }
        public List<PCRNDetailsModel> FetchPCRNDetailsFiltered(PCRNDetailsModel model)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@PCRNID", model.PCRNID));//temp
                                                                     //param.AddRange(Common.GetPagingParams(model));
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_PCRNDetailsFiltered", param.ToArray()).Tables[0];
                return LoopinPCRNDetails(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        private List<PCRNDetailsModel> LoopinPCRNDetails(DataTable dt, bool ForExcel = false)
        {
            List<PCRNDetailsModel> PCRNModel = new List<PCRNDetailsModel>();
            foreach (DataRow r in dt.Rows)
            {
                PCRNModel.Add(RowOfPCRNDetails(r, ForExcel));
            }
            return PCRNModel;
        }
        public List<PCRNDetailsModel> GetPCRNExcelExport(PCRNDetailsModel model)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@PCRNID", model.PCRNID));
                param.Add(new SqlParameter("@month", model.Month));
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "PCRN_Excel_Export", param.ToArray()).Tables[0];
                return LoopinPCRNDetails(dt, true); // ForExcel = true
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public PCRNDetailsModel RowOfPCRNDetails(DataRow row, bool ForExcel = false)
        {
            PCRNDetailsModel model = new PCRNDetailsModel();
            model.PCRNID = row.Field<int>("PCRNID");
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
                    model.TSPNamePCRNDetail = row.Field<string>("TSP");
                    model.ClassCodePCRNDetail = row.Field<string>("Classcode");
                    model.ClassStartdatePCRNDetail = row.Field<string>("Classstartdate");
                    model.ClassEnddatePCRNDetail = row.Field<string>("Classenddate");
                }
                //model.ClassStartdatePCRNDetail = row["Classstartdate"].ToString().GetDate();
                //model.ClassEnddatePCRNDetail = row["Classenddate"].ToString().GetDate();
                //model.ClassStartdatePCRNDetail = row["ClassStartDate"].ToString().GetDate();
                //model.ClassEnddatePCRNDetail = row["ClassEndDate"].ToString().GetDate();
                //====================================================
                model.TraineeCode = row.Field<string>("TraineeCode");
                model.TraineeCNIC = row.Field<string>("TraineeCNIC");
                model.FatherName = row.Field<string>("FatherName");
                model.ContactNumber1 = row.Field<string>("ContactNumber1");
                model.DistrictName = row.Field<string>("DistrictName");
            }
            return model;
        }
        public PCRNDetailsModel UpdatePCRNDetails(PCRNDetailsModel PCRN)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[12];
                param[0] = new SqlParameter("@PCRNID", PCRN.PCRNID);
                param[1] = new SqlParameter("@ReportId", PCRN.ReportId);
                //param[2] = new SqlParameter("@TraineeId", PCRN.TraineeId);
                //param[3] = new SqlParameter("@Amount", PCRN.Amount);
                param[4] = new SqlParameter("@TokenNumber", PCRN.TokenNumber);
                param[5] = new SqlParameter("@TransactionNumber", PCRN.TransactionNumber);
                param[6] = new SqlParameter("@Comments", PCRN.Comments);
                param[7] = new SqlParameter("@IsPaid", PCRN.IsPaid);
                //param[8] = new SqlParameter("@IsVarified", PCRN.IsVarified);
                //param[9] = new SqlParameter("@Month", PCRN.Month);
                //param[10] = new SqlParameter("@NumberOfMonths", PCRN.NumberOfMonths);
                param[11] = new SqlParameter("@CurUserID", PCRN.CurUserID);
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[U_PCRNDetails]", param);
                return PCRN;
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }
    }
}
