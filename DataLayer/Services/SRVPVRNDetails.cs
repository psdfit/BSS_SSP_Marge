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
    public class SRVPVRNDetails : SRVBase, ISRVPVRNDetails
    {
        
        public List<PVRNDetailsModel> FetchPVRNDetails(PVRNDetailsModel model)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@PVRNID", model.PVRNID));//temp
                //param.AddRange(Common.GetPagingParams(model));

                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_PVRNDetails", param.ToArray()).Tables[0];
                return LoopinPVRNDetails(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<PVRNDetailsModel> GetPVRNExcelExportByIDs(string ids)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_PVRNDetails_By_IDs", new SqlParameter("@PVRNMasterIDs", ids)).Tables[0];
            return LoopinPVRNDetails(dt);
        }

        public List<PVRNDetailsModel> FetchPVRNDetailsFiltered(PVRNDetailsModel model)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@PVRNID", model.PVRNID));//temp
                //param.AddRange(Common.GetPagingParams(model));

                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_PVRNDetailsFiltered", param.ToArray()).Tables[0];
                return LoopinPVRNDetails(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        private List<PVRNDetailsModel> LoopinPVRNDetails(DataTable dt, bool ForExcel = false)
        {
            List<PVRNDetailsModel> PVRNModel = new List<PVRNDetailsModel>();
            foreach (DataRow r in dt.Rows)
            {
                PVRNModel.Add(RowOfPVRNDetails(r, ForExcel));
            }

            return PVRNModel;
        }

        public List<PVRNDetailsModel> GetPVRNExcelExport(PVRNDetailsModel model)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();

                param.Add(new SqlParameter("@PVRNID", model.PVRNID));
                param.Add(new SqlParameter("@month", model.Month));

                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "PVRN_Excel_Export", param.ToArray()).Tables[0];
                return LoopinPVRNDetails(dt, true); // ForExcel = true
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public PVRNDetailsModel RowOfPVRNDetails(DataRow row, bool ForExcel = false)
        {
            PVRNDetailsModel model = new PVRNDetailsModel();
            model.PVRNID = row.Field<int>("PVRNID");
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
                    model.TSPNamePVRNDetail = row.Field<string>("TSP");
                    model.ClassCodePVRNDetail = row.Field<string>("Classcode");
                    model.ClassStartdatePVRNDetail = row.Field<string>("Classstartdate");
                    model.ClassEnddatePVRNDetail = row.Field<string>("Classenddate");
                }
                //model.ClassStartdatePVRNDetail = row["Classstartdate"].ToString().GetDate();
                //model.ClassEnddatePVRNDetail = row["Classenddate"].ToString().GetDate();

                //model.ClassStartdatePVRNDetail = row["ClassStartDate"].ToString().GetDate();
                //model.ClassEnddatePVRNDetail = row["ClassEndDate"].ToString().GetDate();
                //====================================================

                model.TraineeCode = row.Field<string>("TraineeCode");
                model.TraineeCNIC = row.Field<string>("TraineeCNIC");
                model.FatherName = row.Field<string>("FatherName");
                model.ContactNumber1 = row.Field<string>("ContactNumber1");
                model.DistrictName = row.Field<string>("DistrictName");

            }

            return model;
        }

        public PVRNDetailsModel UpdatePVRNDetails(PVRNDetailsModel PVRN)
        {
            try
            {

                SqlParameter[] param = new SqlParameter[12];
                param[0] = new SqlParameter("@PVRNID", PVRN.PVRNID);
                param[1] = new SqlParameter("@ReportId", PVRN.ReportId);
                //param[2] = new SqlParameter("@TraineeId", PVRN.TraineeId);
                //param[3] = new SqlParameter("@Amount", PVRN.Amount);
                param[4] = new SqlParameter("@TokenNumber", PVRN.TokenNumber);
                param[5] = new SqlParameter("@TransactionNumber", PVRN.TransactionNumber);
                param[6] = new SqlParameter("@Comments", PVRN.Comments);
                param[7] = new SqlParameter("@IsPaid", PVRN.IsPaid);
                //param[8] = new SqlParameter("@IsVarified", PVRN.IsVarified);
                //param[9] = new SqlParameter("@Month", PVRN.Month);
                //param[10] = new SqlParameter("@NumberOfMonths", PVRN.NumberOfMonths);

                param[11] = new SqlParameter("@CurUserID", PVRN.CurUserID);
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[U_PVRNDetails]", param);
                return PVRN;
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }
    }
}
