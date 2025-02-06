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
    public class SRVTPRNDetails : SRVBase, ISRVTPRNDetails
    {
        
        public List<TPRNDetailsModel> FetchTPRNDetails(TPRNDetailsModel model)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@TPRNID", model.TPRNID));//temp
                //param.AddRange(Common.GetPagingParams(model));

                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TPRNDetails", param.ToArray()).Tables[0];
                return LoopinTPRNDetails(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<TPRNDetailsModel> GetTPRNExcelExportByIDs(string ids)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TPRNDetails_By_IDs", new SqlParameter("@TPRNMasterIDs", ids)).Tables[0];
            return LoopinTPRNDetails(dt);
        }

        public List<TPRNDetailsModel> FetchTPRNDetailsFiltered(TPRNDetailsModel model)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@TPRNID", model.TPRNID));//temp
                //param.AddRange(Common.GetPagingParams(model));

                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TPRNDetailsFiltered", param.ToArray()).Tables[0];
                return LoopinTPRNDetails(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        private List<TPRNDetailsModel> LoopinTPRNDetails(DataTable dt, bool ForExcel = false)
        {
            List<TPRNDetailsModel> TPRNModel = new List<TPRNDetailsModel>();
            foreach (DataRow r in dt.Rows)
            {
                TPRNModel.Add(RowOfTPRNDetails(r, ForExcel));
            }

            return TPRNModel;
        }

        public List<TPRNDetailsModel> GetTPRNExcelExport(TPRNDetailsModel model)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();

                param.Add(new SqlParameter("@TPRNID", model.TPRNID));
                param.Add(new SqlParameter("@month", model.Month));

                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "TPRN_Excel_Export", param.ToArray()).Tables[0];
                return LoopinTPRNDetails(dt, true); // ForExcel = true
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public TPRNDetailsModel RowOfTPRNDetails(DataRow row, bool ForExcel = false)
        {
            TPRNDetailsModel model = new TPRNDetailsModel();
            model.TPRNID = row.Field<int>("TPRNID");
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
                    model.TSPNameTPRNDetail = row.Field<string>("TSP");
                    model.ClassCodeTPRNDetail = row.Field<string>("Classcode");
                    model.ClassStartdateTPRNDetail = row.Field<string>("Classstartdate");
                    model.ClassEnddateTPRNDetail = row.Field<string>("Classenddate");
                }
                //model.ClassStartdateTPRNDetail = row["Classstartdate"].ToString().GetDate();
                //model.ClassEnddateTPRNDetail = row["Classenddate"].ToString().GetDate();

                //model.ClassStartdateTPRNDetail = row["ClassStartDate"].ToString().GetDate();
                //model.ClassEnddateTPRNDetail = row["ClassEndDate"].ToString().GetDate();
                //====================================================

                model.TraineeCode = row.Field<string>("TraineeCode");
                model.TraineeCNIC = row.Field<string>("TraineeCNIC");
                model.FatherName = row.Field<string>("FatherName");
                model.ContactNumber1 = row.Field<string>("ContactNumber1");
                model.DistrictName = row.Field<string>("DistrictName");

            }

            return model;
        }

        public TPRNDetailsModel UpdateTPRNDetails(TPRNDetailsModel TPRN)
        {
            try
            {

                SqlParameter[] param = new SqlParameter[12];
                param[0] = new SqlParameter("@TPRNID", TPRN.TPRNID);
                param[1] = new SqlParameter("@ReportId", TPRN.ReportId);
                //param[2] = new SqlParameter("@TraineeId", TPRN.TraineeId);
                //param[3] = new SqlParameter("@Amount", TPRN.Amount);
                param[4] = new SqlParameter("@TokenNumber", TPRN.TokenNumber);
                param[5] = new SqlParameter("@TransactionNumber", TPRN.TransactionNumber);
                param[6] = new SqlParameter("@Comments", TPRN.Comments);
                param[7] = new SqlParameter("@IsPaid", TPRN.IsPaid);
                //param[8] = new SqlParameter("@IsVarified", TPRN.IsVarified);
                //param[9] = new SqlParameter("@Month", TPRN.Month);
                //param[10] = new SqlParameter("@NumberOfMonths", TPRN.NumberOfMonths);

                param[11] = new SqlParameter("@CurUserID", TPRN.CurUserID);
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[U_TPRNDetails]", param);
                return TPRN;
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }
    }
}
