using DataLayer.Interfaces;
using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Data;
using DataLayer.Classes;
using Microsoft.Data.SqlClient;
using System.Text;

namespace DataLayer.Services
{
    public class SRVSRNDetails : SRVBase, ISRVSRNDetails
    {
        
        public List<SRNDetailsModel> FetchSRNDetails(SRNDetailsModel model)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@SRNID", model.SRNID));//temp
                //param.AddRange(Common.GetPagingParams(model));

                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_SRNDetails", param.ToArray()).Tables[0];
                return LoopinSRNDetails(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<SRNDetailsModel> GetSRNExcelExportByIDs(string ids)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_SRNDetails_By_IDs", new SqlParameter("@SRNMasterIDs", ids)).Tables[0];
            return LoopinSRNDetails(dt);
        }

        public List<SRNDetailsModel> FetchSRNDetailsFiltered(SRNDetailsModel model)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@SRNID", model.SRNID));//temp
                //param.AddRange(Common.GetPagingParams(model));

                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_SRNDetailsFiltered", param.ToArray()).Tables[0];
                return LoopinSRNDetails(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        private List<SRNDetailsModel> LoopinSRNDetails(DataTable dt, bool ForExcel = false)
        {
            List<SRNDetailsModel> SRNModel = new List<SRNDetailsModel>();
            foreach (DataRow r in dt.Rows)
            {
                SRNModel.Add(RowOfSRNDetails(r, ForExcel));
            }

            return SRNModel;
        }

        public List<SRNDetailsModel> GetSRNExcelExport(SRNDetailsModel model)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();

                param.Add(new SqlParameter("@SRNID", model.SRNID));
                param.Add(new SqlParameter("@month", model.Month));

                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "SRN_Excel_Export", param.ToArray()).Tables[0];
                return LoopinSRNDetails(dt, true); // ForExcel = true
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public SRNDetailsModel RowOfSRNDetails(DataRow row, bool ForExcel = false)
        {
            SRNDetailsModel model = new SRNDetailsModel();
            model.SRNID = row.Field<int>("SRNID");
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
                    model.TSPNameSRNDetail = row.Field<string>("TSP");
                    model.ClassCodeSRNDetail = row.Field<string>("Classcode");
                    model.ClassStartdateSRNDetail = row.Field<string>("Classstartdate");
                    model.ClassEnddateSRNDetail = row.Field<string>("Classenddate");
                }
                //model.ClassStartdateSRNDetail = row["Classstartdate"].ToString().GetDate();
                //model.ClassEnddateSRNDetail = row["Classenddate"].ToString().GetDate();

                //model.ClassStartdateSRNDetail = row["ClassStartDate"].ToString().GetDate();
                //model.ClassEnddateSRNDetail = row["ClassEndDate"].ToString().GetDate();
                //====================================================

                model.TraineeCode = row.Field<string>("TraineeCode");
                model.TraineeCNIC = row.Field<string>("TraineeCNIC");
                model.FatherName = row.Field<string>("FatherName");
                model.ContactNumber1 = row.Field<string>("ContactNumber1");
                model.DistrictName = row.Field<string>("DistrictName");

            }

            return model;
        }

        public SRNDetailsModel UpdateSRNDetails(SRNDetailsModel SRN)
        {
            try
            {

                SqlParameter[] param = new SqlParameter[12];
                param[0] = new SqlParameter("@SRNID", SRN.SRNID);
                param[1] = new SqlParameter("@ReportId", SRN.ReportId);
                //param[2] = new SqlParameter("@TraineeId", SRN.TraineeId);
                //param[3] = new SqlParameter("@Amount", SRN.Amount);
                param[4] = new SqlParameter("@TokenNumber", SRN.TokenNumber);
                param[5] = new SqlParameter("@TransactionNumber", SRN.TransactionNumber);
                param[6] = new SqlParameter("@Comments", SRN.Comments);
                param[7] = new SqlParameter("@IsPaid", SRN.IsPaid);
                //param[8] = new SqlParameter("@IsVarified", SRN.IsVarified);
                //param[9] = new SqlParameter("@Month", SRN.Month);
                //param[10] = new SqlParameter("@NumberOfMonths", SRN.NumberOfMonths);

                param[11] = new SqlParameter("@CurUserID", SRN.CurUserID);
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[U_SRNDetails]", param);
                return SRN;
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }
    }
}
