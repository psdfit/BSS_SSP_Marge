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
    public class SRVGURNDetails : SRVBase, ISRVGURNDetails
    {
        
        public List<GURNDetailsModel> FetchGURNDetails(GURNDetailsModel model)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@GURNID", model.GURNID));//temp
                //param.AddRange(Common.GetPagingParams(model));

                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_GURNDetails", param.ToArray()).Tables[0];
                return LoopinGURNDetails(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<GURNDetailsModel> GetGURNExcelExportByIDs(string ids)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_GURNDetails_By_IDs", new SqlParameter("@GURNMasterIDs", ids)).Tables[0];
            return LoopinGURNDetails(dt);
        }

        public List<GURNDetailsModel> FetchGURNDetailsFiltered(GURNDetailsModel model)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@GURNID", model.GURNID));//temp
                //param.AddRange(Common.GetPagingParams(model));

                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_GURNDetailsFiltered", param.ToArray()).Tables[0];
                return LoopinGURNDetails(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        private List<GURNDetailsModel> LoopinGURNDetails(DataTable dt, bool ForExcel = false)
        {
            List<GURNDetailsModel> GURNModel = new List<GURNDetailsModel>();
            foreach (DataRow r in dt.Rows)
            {
                GURNModel.Add(RowOfGURNDetails(r, ForExcel));
            }

            return GURNModel;
        }

        public List<GURNDetailsModel> GetGURNExcelExport(GURNDetailsModel model)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();

                param.Add(new SqlParameter("@GURNID", model.GURNID));
                param.Add(new SqlParameter("@month", model.Month));

                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "GURN_Excel_Export", param.ToArray()).Tables[0];
                return LoopinGURNDetails(dt, true); // ForExcel = true
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public GURNDetailsModel RowOfGURNDetails(DataRow row, bool ForExcel = false)
        {
            GURNDetailsModel model = new GURNDetailsModel();
            model.GURNID = row.Field<int>("GURNID");
            model.ReportId = row.Field<string>("ReportId");
            model.Amount = row.Field<decimal?>("Amount");
            model.TokenNumber = row.Field<string>("TokenNumber");
            model.TransactionNumber = row.Field<string>("TransactionNumber");
            model.Comments = row.Field<string>("Comments");
            model.IsPaid = row.Field<bool?>("IsPaid");
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
                    model.TSPNameGURNDetail = row.Field<string>("TSP");
                    model.ClassCodeGURNDetail = row.Field<string>("Classcode");
                    model.ClassStartdateGURNDetail = row.Field<DateTime?>("Classstartdate")?.ToString("yyyy-MM-dd");
                    model.ClassEnddateGURNDetail = row.Field<DateTime?>("Classenddate")?.ToString("yyyy-MM-dd");

                }
                //model.ClassStartdateGURNDetail = row["Classstartdate"].ToString().GetDate();
                //model.ClassEnddateGURNDetail = row["Classenddate"].ToString().GetDate();

                //model.ClassStartdateGURNDetail = row["ClassStartDate"].ToString().GetDate();
                //model.ClassEnddateGURNDetail = row["ClassEndDate"].ToString().GetDate();
                //====================================================

                model.TraineeCode = row.Field<string>("TraineeCode");
                model.TraineeCNIC = row.Field<string>("TraineeCNIC");
                model.FatherName = row.Field<string>("FatherName");
                model.ContactNumber1 = row.Field<string>("ContactNumber");
                model.DistrictName = row.Field<string>("DistrictName");
                model.GURUName = row.Field<string>("GURUName");
                model.GURUCNIC = row.Field<string>("GURUCNIC");
                model.GURUContactNumber = row.Field<string>("GURUContactNumber");


            }

            return model;
        }

        public GURNDetailsModel UpdateGURNDetails(GURNDetailsModel GURN)
        {
            try
            {

                SqlParameter[] param = new SqlParameter[12];
                param[0] = new SqlParameter("@GURNID", GURN.GURNID);
                param[1] = new SqlParameter("@ReportId", GURN.ReportId);
                //param[2] = new SqlParameter("@TraineeId", GURN.TraineeId);
                //param[3] = new SqlParameter("@Amount", GURN.Amount);
                param[4] = new SqlParameter("@TokenNumber", GURN.TokenNumber);
                param[5] = new SqlParameter("@TransactionNumber", GURN.TransactionNumber);
                param[6] = new SqlParameter("@Comments", GURN.Comments);
                param[7] = new SqlParameter("@IsPaid", GURN.IsPaid);
                //param[8] = new SqlParameter("@IsVarified", GURN.IsVarified);
                //param[9] = new SqlParameter("@Month", GURN.Month);
                //param[10] = new SqlParameter("@NumberOfMonths", GURN.NumberOfMonths);

                param[11] = new SqlParameter("@CurUserID", GURN.CurUserID);
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[U_GURNDetails]", param);
                return GURN;
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }
    }
}
