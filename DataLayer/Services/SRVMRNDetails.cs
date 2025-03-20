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
    public class SRVMRNDetails : SRVBase, ISRVMRNDetails
    {
        
        public List<MRNDetailsModel> FetchMRNDetails(MRNDetailsModel model)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@MRNID", model.MRNID));//temp
                //param.AddRange(Common.GetPagingParams(model));

                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_MRNDetails", param.ToArray()).Tables[0];
                return LoopinMRNDetails(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<MRNDetailsModel> GetMRNExcelExportByIDs(string ids)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_MRNDetails_By_IDs", new SqlParameter("@MRNMasterIDs", ids)).Tables[0];
            return LoopinMRNDetails(dt);
        }

        public List<MRNDetailsModel> FetchMRNDetailsFiltered(MRNDetailsModel model)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@MRNID", model.MRNID));//temp
                //param.AddRange(Common.GetPagingParams(model));

                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_MRNDetailsFiltered", param.ToArray()).Tables[0];
                return LoopinMRNDetails(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        private List<MRNDetailsModel> LoopinMRNDetails(DataTable dt, bool ForExcel = false)
        {
            List<MRNDetailsModel> MRNModel = new List<MRNDetailsModel>();
            foreach (DataRow r in dt.Rows)
            {
                MRNModel.Add(RowOfMRNDetails(r, ForExcel));
            }

            return MRNModel;
        }

        public List<MRNDetailsModel> GetMRNExcelExport(MRNDetailsModel model)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();

                param.Add(new SqlParameter("@MRNID", model.MRNID));
                param.Add(new SqlParameter("@month", model.Month));

                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "MRN_Excel_Export", param.ToArray()).Tables[0];
                return LoopinMRNDetails(dt, true); // ForExcel = true
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public MRNDetailsModel RowOfMRNDetails(DataRow row, bool ForExcel = false)
        {
            MRNDetailsModel model = new MRNDetailsModel();
            model.MRNID = row.Field<int>("MRNID");
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
                    model.TSPNameMRNDetail = row.Field<string>("TSP");
                    model.ClassCodeMRNDetail = row.Field<string>("Classcode");
                    model.ClassStartdateMRNDetail = row.Field<string>("Classstartdate");
                    model.ClassEnddateMRNDetail = row.Field<string>("Classenddate");
                }
                //model.ClassStartdateMRNDetail = row["Classstartdate"].ToString().GetDate();
                //model.ClassEnddateMRNDetail = row["Classenddate"].ToString().GetDate();

                //model.ClassStartdateMRNDetail = row["ClassStartDate"].ToString().GetDate();
                //model.ClassEnddateMRNDetail = row["ClassEndDate"].ToString().GetDate();
                //====================================================

                model.TraineeCode = row.Field<string>("TraineeCode");
                model.TraineeCNIC = row.Field<string>("TraineeCNIC");
                model.FatherName = row.Field<string>("FatherName");
                model.ContactNumber1 = row.Field<string>("ContactNumber1");
                model.DistrictName = row.Field<string>("DistrictName");

            }

            return model;
        }

        public MRNDetailsModel UpdateMRNDetails(MRNDetailsModel MRN)
        {
            try
            {

                SqlParameter[] param = new SqlParameter[12];
                param[0] = new SqlParameter("@MRNID", MRN.MRNID);
                param[1] = new SqlParameter("@ReportId", MRN.ReportId);
                //param[2] = new SqlParameter("@TraineeId", MRN.TraineeId);
                //param[3] = new SqlParameter("@Amount", MRN.Amount);
                param[4] = new SqlParameter("@TokenNumber", MRN.TokenNumber);
                param[5] = new SqlParameter("@TransactionNumber", MRN.TransactionNumber);
                param[6] = new SqlParameter("@Comments", MRN.Comments);
                param[7] = new SqlParameter("@IsPaid", MRN.IsPaid);
                //param[8] = new SqlParameter("@IsVarified", MRN.IsVarified);
                //param[9] = new SqlParameter("@Month", MRN.Month);
                //param[10] = new SqlParameter("@NumberOfMonths", MRN.NumberOfMonths);

                param[11] = new SqlParameter("@CurUserID", MRN.CurUserID);
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[U_MRNDetails]", param);
                return MRN;
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }
    }
}
