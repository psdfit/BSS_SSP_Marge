/* **** Aamer Rehman Malik *****/

using System;
using System.Collections.Generic;
using System.Text;
using DataLayer.Models;
using DataLayer.Interfaces;
using System.Data;
using Microsoft.Data.SqlClient;
using DataLayer.Classes;
using System.Linq;

namespace DataLayer.Services
{
    public class SRVPRN : ISRVPRN
    {
        public List<PRNModel> GetPRNForApproval(int id)
        {
            DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_PRN", new SqlParameter("@PRNMasterID", id)).Tables[0];

            return LoopinPRN(dt);
        }
        public List<PRNModel> GetPRNForApproval(PRNModel model)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@ProcessKey", model.ProcessKey));
            param.Add(new SqlParameter("@Month", model.Month));

            DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_PRN", param.ToArray()).Tables[0];

            return LoopinPRN(dt);
        }
        public List<PRNModel> GetPRNExcelExport(PRNMasterModel model)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Month", model.Month));
            param.Add(new SqlParameter("@PRNMasterID", model.PRNMasterID));
            DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "PRN_Excel_Export", param.ToArray()).Tables[0];

            return LoopinPRN(dt);
        }
        public List<PRNModel> GetPRNExcelExportByIDs(string ids)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_PRN_By_IDs", new SqlParameter("@PRNMasterIDs", ids)).Tables[0];
            return LoopinPRN(dt);
        }
        public PRNModel GetPRNByID(PRNModel model, SqlTransaction _transaction = null)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@PRNID", model.PRNID));
            DataTable dt = new DataTable();

            if (_transaction != null)
                dt = SqlHelper.ExecuteDataset(_transaction, CommandType.StoredProcedure, "RD_PRN", param.ToArray()).Tables[0];
            else
                dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_PRN", param.ToArray()).Tables[0];

            if (dt.Rows.Count > 0)
            {
                return RowOfPRN(dt.Rows[0]);
            }
            else
                return null;
        }
        private List<PRNModel> LoopinPRN(DataTable dt)
        {
            List<PRNModel> prn = new List<PRNModel>();
            foreach (DataRow r in dt.Rows)
            {
                prn.Add(RowOfPRN(r));
            }

            return prn;
        }
        private PRNModel RowOfPRN(DataRow r)
        {
            PRNModel model = new PRNModel();

            model.PRNID = r.Field<int>("PRNID");
            if (r.Table.Columns.Contains("TSPName"))
            {
                model.TSPName = r["TSPName"].ToString();
            }
            model.ClassID = r.Field<int>("ClassID");
            model.InvoiceNumber = r.Field<int>("InvoiceNumber");
            model.TradeID = r.Field<int>("TradeID");
            model.ClaimedTrainees = r.Field<int>("ClaimedTrainees");
            model.EnrolledTrainees = r.Field<int>("EnrolledTrainees");
            model.DropoutsVerified = r.Field<int>("DropoutsVerified");
            model.DropoutsUnverified = r.Field<int>("DropoutsUnverified");
            model.CNICVerified = r.Field<int>("CNICVerified");
            model.CNICVExcesses = r.Field<int>("CNICVExcesses");
            model.CNICUnverified = r.Field<int>("CNICUnverified");
            model.CNICUnVExcesses = r.Field<int>("CNICUnVExcesses");
            model.MaxAttendance = r.Field<int>("MaxAttendance");
            model.DeductionMarginal = r.Field<int>("DeductionMarginal");
            model.Duration = Convert.ToDouble(r["Duration"]);
            model.ContractualTrainees = r.Field<int>("ContractualTrainees");
            model.ExpelledVerified = r.Field<int>("ExpelledVerified");
            model.ExpelledUnverified = r.Field<int>("ExpelledUnverified");
            model.PassVerified = r.Field<int>("PassVerified");
            model.PassUnverified = r.Field<int>("PassUnverified");
            model.FailedVerified = r.Field<int>("FailedVerified");
            model.FailedUnverified = r.Field<int>("FailedUnverified");
            model.AbsentVerified = r.Field<int>("AbsentVerified");
            model.AbsentUnverified = r.Field<int>("AbsentUnverified");
            model.DropOut = r.Field<int>("DropOut");
            model.DeductionExtraRegisteredForExam = r.Field<int>("DeductionExtraRegisteredForExam");
            model.DeductionFailedTrainees = r.Field<int>("DeductionFailedTrainees");
            model.PenaltyImposedByME = r.Field<int>("PenaltyImposedByME");
            model.DeductionUniformBagReceiving = r.Field<int>("DeductionUniformBagReceiving");
            model.PaymentWithheldPhysicalCount = Convert.ToDouble(r["PaymentWithheldPhysicalCount"]);
            model.CompletedTrainees = r.Field<int>("CompletedTrainees");
            model.GraduatedCommitmentTrainees = r.Field<int>("GraduatedCommitmentTrainees");
            model.EmploymentReported = r.Field<int>("EmploymentReported");
            model.VerifiedTrainees = r.Field<int>("VerifiedTrainees");
            model.VerifiedToCompletedCommitment = r.Field<int>("VerifiedToCompletedCommitment");
            model.TraineesFoundInVISIT1 = r.Field<int>("TraineesFoundInVISIT1");
            model.TraineesFoundInVISIT2 = r.Field<int>("TraineesFoundInVISIT2");
            model.PRNMasterID = r.Field<int>("PRNMasterID");
            model.ExtraTraineeDeductCompletion = r.Field<int>("ExtraTraineeDeductCompletion");
            model.UnVDeductCompletion = r.Field<int>("UnVDeductCompletion");
            model.DropOutDeductCompletion = r.Field<int>("DropOutDeductCompletion");
            model.DeductionSinIncepDropout = Convert.ToDouble(r["DeductionSinIncepDropout"]);
            model.PaymentWithheldSinIncepUnVCNIC = Convert.ToDouble(r["PaymentWithheldSinIncepUnVCNIC"]);
            model.PenaltyTPMReports = Convert.ToDouble(r["PenaltyTPMReports"]);
            model.ReimbursementUnVTrainees = Convert.ToDouble(r["ReimbursementUnVTrainees"]);
            model.ReimbursementAttandance = Convert.ToDouble(r["ReimbursementAttandance"]);
            model.PaymentToBeReleasedTrainees = Convert.ToDouble(r["PaymentToBeReleasedTrainees"]);
            model.Payment100p = Convert.ToDouble(r["Payment100p"]);
            model.Payment50p = Convert.ToDouble(r["Payment50p"]);
            model.TradeName = r["TradeName"].ToString();
            model.ClassCode = r["ClassCode"].ToString();
            model.ClassStatus = r["ClassStatus"].ToString();
            model.NonFunctionalVisit1 = r["NonFunctionalVisit1"].ToString();
            model.NonFunctionalVisit2 = r["NonFunctionalVisit2"].ToString();
            model.NonFunctionalVisit3 = r["NonFunctionalVisit3"].ToString();
            model.EmploymentCommitmentPercentage = r["EmploymentCommitmentPercentage"].ToString();
            //if (r["NonFunctionalVisit1Date"] != "")
            model.NonFunctionalVisit1Date = r.Field<DateTime?>("NonFunctionalVisit1Date");
            //if (r["NonFunctionalVisit2Date"] != "")
            model.NonFunctionalVisit2Date = r.Field<DateTime?>("NonFunctionalVisit2Date");
            //if (r["NonFunctionalVisit3Date"] != "")
            model.NonFunctionalVisit3Date = r.Field<DateTime?>("NonFunctionalVisit3Date");
            model.ClassStartDate = r["ClassStartDate"].ToString().GetDate();
            model.ClassEndDate = r["ClassEndDate"].ToString().GetDate();
            if (r.Table.Columns.Contains("Month"))
            {
                model.Month = r["Month"].ToString().GetDate();
            }
            model.IsApproved = Convert.ToBoolean(r["IsApproved"]);
            model.ProcessKey = r["ProcessKey"].ToString();
            model.IsRejected = r.Field<bool?>("IsRejected");
            model.InCancel = r.Field<bool?>("InCancel");
            model.InActive = r.Field<bool?>("InActive");
            if (r.Table.Columns.Contains("SchemeName"))
            {
                model.SchemeName = r["SchemeName"].ToString(); 
            }
            if (r.Table.Columns.Contains("AbsentDeductCompletion"))
            {
                model.AbsentDeductCompletion = r.Field<int?>("AbsentDeductCompletion");
            }
            if (r.Table.Columns.Contains("DropoutPassFailAbsent"))
            {
                model.DropoutPassFailAbsent = r.Field<int?>("DropoutPassFailAbsent");
            }
            if (r.Table.Columns.Contains("ExpelledPassFailAbsent"))
            {
                model.ExpelledPassFailAbsent = r.Field<int?>("ExpelledPassFailAbsent");
            }
            if (r.Table.Columns.Contains("PenaltyAndUniBagRecvInputRemarks"))
            {
                model.PenaltyAndUniBagRecvInputRemarks = r["PenaltyAndUniBagRecvInputRemarks"].ToString();
            }
            if (r.Table.Columns.Contains("CertAuthName"))
            {
                model.CertAuthName = r["CertAuthName"].ToString();
            }
            if (r.Table.Columns.Contains("ExpelledRegularVerifiedForTheMonth"))
            {
                model.ExpelledRegularVerifiedForTheMonth = r.Field<int?>("ExpelledRegularVerifiedForTheMonth");
            }

            if (r.Table.Columns.Contains("status")) 
            {
                model.StatusApproved = r["status"].ToString();
            }

            if (r.Table.Columns.Contains("finalApprovalDate"))
            {
                model.FinalApprovalDate = r["finalApprovalDate"].ToString().GetDate();
            }

            if (r.Table.Columns.Contains("CreatedDate"))
            {
                model.CreationDate = r["CreatedDate"].ToString().GetDate();
            }

            if (r.Table.Columns.Contains("FundingCategory"))
            {
                model.FundingCategory = r["FundingCategory"].ToString();
            }

            if (r.Table.Columns.Contains("kam"))
            {
                model.kam = r["kam"].ToString();
            }


            return model;
        }
        public bool PRNApproveReject(PRNModel model, SqlTransaction transaction = null)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@PRNID", model.PRNID));
                param.Add(new SqlParameter("@IsApproved", model.IsApproved));
                param.Add(new SqlParameter("@IsRejected", model.IsRejected));
                param.Add(new SqlParameter("@CurUserID", model.CurUserID));

                if (transaction != null)
                {
                    SqlHelper.ExecuteScalar(transaction, CommandType.StoredProcedure, "U_PRNApproveReject", param.ToArray());
                }
                else
                {
                    SqlHelper.ExecuteScalar(SqlHelper.GetCon(), CommandType.StoredProcedure, "U_PRNApproveReject", param.ToArray());
                }
                return true;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<PRNModel> GetPTBRTrainees(string classCode, DateTime month)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@ClassCode", classCode));
            DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_PRN", param.ToArray()).Tables[0];
            var list = LoopinPRN(dt);
            list = list.Where(x => x.Month < month && x.InActive == false).ToList();
            return list;
        }
        public bool GeneratePRNCompletion(QueryFilters model)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@TSPID", model.TSPID));
                param.Add(new SqlParameter("@ClassIDs", model.ClassIDs));
                param.Add(new SqlParameter("@CurUserID", model.UserID));
                param.Add(new SqlParameter("@Month", model.Month));
                SqlHelper.ExecuteScalar(SqlHelper.GetCon(), CommandType.StoredProcedure, "dbo.GeneratePRNCompletion", param.ToArray());
                return true;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public bool GeneratePRNFinal(QueryFilters model)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@TSPID", model.TSPID));
                param.Add(new SqlParameter("@ClassIDs", model.ClassIDs));
                param.Add(new SqlParameter("@CurUserID", model.UserID));
                param.Add(new SqlParameter("@Month", model.Month));
                SqlHelper.ExecuteScalar(SqlHelper.GetCon(), CommandType.StoredProcedure, "dbo.GenrateEmploymentPRN", param.ToArray());
                return true;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public bool PenaltyImposedByME_DeductionUniformBag(PRNModel model)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@PRNID", model.PRNID));
                param.Add(new SqlParameter("@ClassID", model.ClassID));
                param.Add(new SqlParameter("@PenaltyImposedByME", model.PenaltyImposedByME));
                param.Add(new SqlParameter("@DeductionUniformBagReceiving", model.DeductionUniformBagReceiving));
                param.Add(new SqlParameter("@PenaltyAndUniBagRecvInputRemarks", model.PenaltyAndUniBagRecvInputRemarks));
                    SqlHelper.ExecuteScalar(SqlHelper.GetCon(), CommandType.StoredProcedure, "U_PenaltyImposedByME_DeductionUniformBag", param.ToArray());
                return true;
            }
            catch (Exception ex) { throw new Exception(ex.Message); return false; }
        }
    }
}