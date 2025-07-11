using DataLayer.Classes;
using DataLayer.Interfaces;
using DataLayer.JobScheduler.Scheduler;
using DataLayer.Models;
using DataLayer.Models.SSP;
using Microsoft.Data.SqlClient;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Xml.Linq;

namespace DataLayer.Services
{
    public class SRVProgramDesign : ISRVProgramDesign
    {

        public SRVProgramDesign()
        { }

        public DataTable FetchProgramDesign(bool InActive)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_SSPProgramDesign", new SqlParameter("@InActive", InActive)).Tables[0];
                return dt;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public DataTable FetchAnalysisReportFilters(int[] filters)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[5];
                param[0] = new SqlParameter("@UserID", filters[0]);
                param[1] = new SqlParameter("@ProvinceID", filters[1]);
                param[2] = new SqlParameter("@ClusterID", filters[2]);
                param[3] = new SqlParameter("@DistrictID", filters[3]);
                param[4] = new SqlParameter("@TradeID", filters[4]);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_SSPTSPRegistrationDetailReport", param).Tables[0];
                return dt;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public DataTable FetchAnalysisReport()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "Rd_SSPTSPRegistrationDetailReportFiltered").Tables[0];
                return dt;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public void SaveProgramDesign(ProgramDesignModel programDesign)
        {
            try
            {

                string AttachmentTORspath = null;
                string AttachmentCriteriaPath = null;


                if (programDesign.AttachmentTORs != null || programDesign.AttachmentTORs != string.Empty)
                {
                    string path = FilePaths.PROGRAM_DESIGN_DOC + programDesign.Program;
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    string paths = path + "\\";
                    AttachmentTORspath = Common.AddFile(programDesign.AttachmentTORs, paths);
                    AttachmentCriteriaPath = Common.AddFile(programDesign.AttachmentCriteria, paths);
                }




                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@ProgramID", programDesign.ProgramID));
                param.Add(new SqlParameter("@ProgramName", programDesign.Program));
                param.Add(new SqlParameter("@ProgramBudget", programDesign.ProgramBudget));
                param.Add(new SqlParameter("@ProgramCode", programDesign.ProgramCode));
                param.Add(new SqlParameter("@ProgramTypeID", programDesign.ProgramTypeID));

                param.Add(new SqlParameter("@PCategoryID", programDesign.ProgramCategoryID));
                param.Add(new SqlParameter("@FundingCategoryID", programDesign.FundingCategoryID));
                param.Add(new SqlParameter("@StipendMode", programDesign.StipendMode));
                //param.Add(new SqlParameter("@ContractAwardDate", programDesign.ContractAwardDate));
                param.Add(new SqlParameter("@BusinessRuleType", programDesign.BusinessRuleType));

                param.Add(new SqlParameter("@FundingSourceID", programDesign.FundingSourceID));
                param.Add(new SqlParameter("@PaymentSchedule", programDesign.PaymentStructureID));
                param.Add(new SqlParameter("@ProgramDescription", programDesign.Description));
                param.Add(new SqlParameter("@Stipend", programDesign.Stipend));
                param.Add(new SqlParameter("@ApplicabilityID", string.Join(",", programDesign.ApplicabilityID)));
                param.Add(new SqlParameter("@MinimumEducation", programDesign.MinEducationID));
                param.Add(new SqlParameter("@MaximumEducation", programDesign.MaxEducationID));
                param.Add(new SqlParameter("@MinAge", programDesign.MinAge));
                param.Add(new SqlParameter("@MaxAge", programDesign.MaxAge));
                param.Add(new SqlParameter("@GenderID", programDesign.GenderID));
                param.Add(new SqlParameter("@CreatedUserID", programDesign.UserID));
                param.Add(new SqlParameter("@ModifiedUserID", programDesign.UserID));
                param.Add(new SqlParameter("@InActive", 0));
                param.Add(new SqlParameter("@FinalSubmitted", programDesign.FinalSubmitted));
                param.Add(new SqlParameter("@PlanningType", programDesign.PlaningTypeID));
                param.Add(new SqlParameter("@TentativeProcessStart", programDesign.TentativeProcessSDate));
                param.Add(new SqlParameter("@ClassStartDate", programDesign.ClassStartDate));
                param.Add(new SqlParameter("@SelectionMethod", programDesign.SelectionMethodID));
                param.Add(new SqlParameter("@EmploymentCommitment", programDesign.EmploymentCommitment));
                param.Add(new SqlParameter("@SchemeDesignOn", programDesign.SchemeDesignOn));
                param.Add(new SqlParameter("@Province", GetCsvOrEmpty(programDesign.ProvinceID)));
                param.Add(new SqlParameter("@Cluster", GetCsvOrEmpty(programDesign.ClusterID)));
                param.Add(new SqlParameter("@District", GetCsvOrEmpty(programDesign.DistrictID)));
                param.Add(new SqlParameter("@ApprovalDescription", programDesign.ApprovalRecDetail));
                param.Add(new SqlParameter("@ApprovalAttachment", programDesign.ApprovalAttachment));
                param.Add(new SqlParameter("@TORsAttachment", AttachmentTORspath));
                param.Add(new SqlParameter("@CriteriaAttachment", AttachmentCriteriaPath));
                param.Add(new SqlParameter("@FinancialYear", programDesign.FinancialYearID));
                param.Add(new SqlParameter("@bagBadgeCost", programDesign.TraineeSupportCost));
                param.Add(new SqlParameter("@IsSubmitted", programDesign.IsSubmitted));

                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_SSPProgramDesign]", param.ToArray());

            }
            catch (Exception ex)
            { throw ex; }
        }
        public void UpdateProgramDesign(ProgramDesignModel _data)
        {
            try
            {
                string query = $"UPDATE SSPProgramDesign SET PlanningType = {_data.PlaningTypeID},SelectionMethod ={_data.SelectionMethodID} WHERE ProgramID = {_data.ProgramID};";
                SqlHelper.ExecuteScalar(SqlHelper.GetCon(), CommandType.Text, query);

            }
            catch (Exception ex)
            { throw ex; }
        }


        private string GetCsvOrEmpty(int[]? values)
        {
            return values != null ? string.Join(",", values) : "";
        }


        public DataTable FetchDropDownList(string spName)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), spName);

                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }
        }



        public DataTable SaveTradeDesign(TradeLotDesignModel data)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@UserID", data.UserID));
            param.Add(new SqlParameter("@TradeDesignID", data.TradeDesignID));
            param.Add(new SqlParameter("@ProvinceID", GetCsvOrEmpty(data.Province)));
            param.Add(new SqlParameter("@ClusterID", GetCsvOrEmpty(data.Cluster)));
            param.Add(new SqlParameter("@DistrictID", GetCsvOrEmpty(data.District)));

            param.Add(new SqlParameter("@ProgramDesignOn", data.ProgramDesignOn));



            string selectedShortList = "";

            switch (data.ProgramDesignOn)
            {
                case "Province":
                    selectedShortList = GetCsvOrEmpty(data.Province);
                    break;
                case "Cluster":
                    selectedShortList = GetCsvOrEmpty(data.Cluster);
                    break;
                case "District":
                    selectedShortList = GetCsvOrEmpty(data.District);
                    break;
            }

            param.Add(new SqlParameter("@SelectedShortList", selectedShortList));


            param.Add(new SqlParameter("@SelectedShortListCount", data.SelectedCount));
            param.Add(new SqlParameter("@PerSelectedContraTarget", data.PerSelectedContraTarget));
            param.Add(new SqlParameter("@PerSelectedCompTarget", data.PerSelectedCompTarget));

            param.Add(new SqlParameter("@ProgramFocusID", data.ProgramFocus));
            param.Add(new SqlParameter("@ProgramDesignID", data.Scheme));
            param.Add(new SqlParameter("@TradeID", data.Trade));
            param.Add(new SqlParameter("@TradeDetailMapID", data.TradeLayer));
            param.Add(new SqlParameter("@CTM", data.CTM));

            param.Add(new SqlParameter("@OJTPayment", data.OJTPayment));
            param.Add(new SqlParameter("@GuruPayment", data.GuruPayment));
            param.Add(new SqlParameter("@TransportationCost", data.TransportationCost));
            param.Add(new SqlParameter("@MedicalCost", data.MedicalCost));
            param.Add(new SqlParameter("@PrometricCost", data.PrometricCost));
            param.Add(new SqlParameter("@ProtectorateCost", data.ProtectorateCost));
            param.Add(new SqlParameter("@OtherTrainingCost", data.OtherTrainingCost));

            param.Add(new SqlParameter("@ExamCost", data.ExamCost));
            param.Add(new SqlParameter("@DropOutPerAge", data.ContraTargetThreshold));
            param.Add(new SqlParameter("@TraineeContractedTarget", data.TraineeContraTarget));
            param.Add(new SqlParameter("@TraineeCompTarget", data.TraineeCompTarget));
            param.Add(new SqlParameter("@GenderID", data.GenderID));



            DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "AU_SSPTradeDesign", param.ToArray()).Tables[0];

            var TradeDesignID = 0;

            if (data.TradeDesignID > 0)
            {
                TradeDesignID = data.TradeDesignID;
            }
            else
            {
                TradeDesignID = Convert.ToInt32(dt.Rows[dt.Rows.Count - 1]["TradeDesignID"]);
            }

            BatchInsert(data.TradeLot, data.ProgramDesignOn, data.Province, data.Cluster, data.District, TradeDesignID, data.UserID, data.Trade, data.TradeLayer);
            return dt;
        }



        public int BatchInsert(List<TradeLot> ls, string ProgramDesignOn, int[] Province, int[] Cluster, int[] District, int BatchFkey, int CurUserID, int TradeID, int TradeDetailID)
        {
            int rowsAffected = 0;
            foreach (var item in ls)
            {
                List<SqlParameter> param = new List<SqlParameter>();

                param.Add(new SqlParameter("@UserID", CurUserID));
                param.Add(new SqlParameter("@TradeDesignID", BatchFkey));
                param.Add(new SqlParameter("@TradeID", TradeID));
                param.Add(new SqlParameter("@TradeDetailMapID", TradeDetailID));
                param.Add(new SqlParameter("@TradeLotID", item.TradeLotID));

                param.Add(new SqlParameter("@ProgramDesignOn", ProgramDesignOn));

                switch (ProgramDesignOn)
                {
                    case "Province":
                        param.Add(new SqlParameter("@ProvinceID", Province[rowsAffected]));
                        break;
                    case "Cluster":
                        param.Add(new SqlParameter("@ClusterID", Cluster[rowsAffected]));
                        break;
                    case "District":
                        param.Add(new SqlParameter("@DistrictID", District[rowsAffected]));
                        break;
                }


                param.Add(new SqlParameter("@TraineeSelectedContTarget", Convert.ToInt32(item.TraineeContTarget)));

                param.Add(new SqlParameter("@CTM", item.CTM));
                param.Add(new SqlParameter("@Duration", item.Duration));
                param.Add(new SqlParameter("@TrainingCost", item.TrainingCost));
                param.Add(new SqlParameter("@Stipend", item.Stipend));
                param.Add(new SqlParameter("@BagAndBadge", item.BagAndBadge));
                param.Add(new SqlParameter("@ExamCost", item.ExamCost));
                param.Add(new SqlParameter("@TotalCost", item.TotalCost));



                rowsAffected += SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "AU_SSPTradeLot", param.ToArray());
            }
            return rowsAffected;
        }


        public DataTable UpdateSchemeInitialization(ProgramDesignModel data)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@ProgramID", data.ProgramID));
            param.Add(new SqlParameter("@UserID", data.UserID));
            DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "AU_SSPUpdateSchemeInitialization", param.ToArray()).Tables[0];

            return dt;
        }
        public DataTable FetchCTMTradeWise(CTMCalculationModel data)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@FundingSourceName", data.FundingSource));
            param.Add(new SqlParameter("@FundingCategory", data.FundingCategory));
            param.Add(new SqlParameter("@ContractAwardStartDate", data.ContractAwardStartDate));
            param.Add(new SqlParameter("@ContractAwardEndDate", data.ContractAwardEndDate));
            param.Add(new SqlParameter("@SchemeType", data.SchemeType));
            param.Add(new SqlParameter("@Sector", data.Sector));
            param.Add(new SqlParameter("@Trade", data.Trade));
            if (data.Duration != "0")
            {
                param.Add(new SqlParameter("@Duration", data.Duration));

            }
            param.Add(new SqlParameter("@Cluster", data.Cluster));
            param.Add(new SqlParameter("@District", data.District));

            DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "CTMCalculationReport", param.ToArray()).Tables[0];
            return dt;
        }

        public DataTable FetchCTMBulkReport(CTMCalculationModel data)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@FundingSourceName", data.FundingSource));
            param.Add(new SqlParameter("@FundingCategory", data.FundingCategory));
            param.Add(new SqlParameter("@ContractAwardStartDate", data.ContractAwardStartDate));
            param.Add(new SqlParameter("@ContractAwardEndDate", data.ContractAwardEndDate));
            param.Add(new SqlParameter("@SchemeType", data.SchemeType));
            param.Add(new SqlParameter("@Sector", data.Sector));
            param.Add(new SqlParameter("@Trade", data.Trade));
            if (data.Duration != "0")
            {
                param.Add(new SqlParameter("@Duration", data.Duration));

            }
            param.Add(new SqlParameter("@Cluster", data.Cluster));
            param.Add(new SqlParameter("@District", data.District));

            DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "CTMCalculationReport1", param.ToArray()).Tables[0];
            return dt;
        }



        public DataTable FetchHistoryReport(HistoricalReportModel data)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@FundingSourceID", data.FundingSource));
            param.Add(new SqlParameter("@ProgramTypeID", data.ProgramType));
            param.Add(new SqlParameter("@StartDate", data.ClassStartDate));
            param.Add(new SqlParameter("@EndDate", data.ClassEndDate));
            param.Add(new SqlParameter("@ProgramFocusID", data.ProgramFocus));
            param.Add(new SqlParameter("@SectorID", data.Sector));
            param.Add(new SqlParameter("@SubSectorID", data.SubSector));
            param.Add(new SqlParameter("@TradeID", data.Trade));
            param.Add(new SqlParameter("@TSPID", data.TSPMaster));
            param.Add(new SqlParameter("@ClusterID", data.Cluster));
            param.Add(new SqlParameter("@DistrictID", data.District));





            DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "HistoricalReport", param.ToArray()).Tables[0];
            return dt;
        }

        public bool SaveProgramWorkflowHistory(ProgramWorkflowHistoryModel data)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@ID", data.ID));
            param.Add(new SqlParameter("@ProgramID", data.ProgramID));
            param.Add(new SqlParameter("@WorkflowID", data.WorkflowID));
            param.Add(new SqlParameter("@Remarks", data.Remarks));
            param.Add(new SqlParameter("@UserID", data.UserID));
            SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "AU_SSPProgramWorkflowHistory", param.ToArray());
            return true;
        }


        public bool SaveProgramStatusHistory(ProgramStatusHistoryModel data)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@ID", data.ID));
            param.Add(new SqlParameter("@ProgramID", data.ProgramID));
            param.Add(new SqlParameter("@StatusID", data.StatusID));
            param.Add(new SqlParameter("@Remarks", data.Remarks));
            param.Add(new SqlParameter("@UserID", data.UserID));

            SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "AU_SSPProgramStatusHistory", param.ToArray());
            return true;
        }


        public bool SaveProgramCriteriaHistory(ProgramCriteriaHistoryModel data)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@ID", data.ID));
            param.Add(new SqlParameter("@ProgramID", data.ProgramID));
            param.Add(new SqlParameter("@CriteriaID", data.CriteriaID));
            param.Add(new SqlParameter("@StartDate", data.StartDate));
            param.Add(new SqlParameter("@EndDate", data.EndDate));
            param.Add(new SqlParameter("@Remarks", data.Remarks));
            param.Add(new SqlParameter("@UserID", data.UserID));


            SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "AU_SSPProgramCriteriaHistory", param.ToArray());
            return true;
        }

        public bool ProgramApproveReject(ProgramDesignModel model, SqlTransaction transaction = null)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@ProgramID", model.ProgramID));
                param.Add(new SqlParameter("@IsApproved", model.IsApproved));
                param.Add(new SqlParameter("@IsRejected", model.IsRejected));
                param.Add(new SqlParameter("@CurUserID", model.CurUserID));

                if (transaction != null)
                {
                    SqlHelper.ExecuteScalar(transaction, CommandType.StoredProcedure, "AU_SSPProgramApproveReject", param.ToArray());
                }
                else
                {
                    SqlHelper.ExecuteScalar(SqlHelper.GetCon(), CommandType.StoredProcedure, "AU_SSPProgramApproveReject", param.ToArray());
                }
                return true;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public bool ProgramDesignFinalApproval(int FormId, SqlTransaction transaction = null)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@ProgramID", FormId));

                if (transaction != null)
                {
                    SqlHelper.ExecuteScalar(transaction, CommandType.StoredProcedure, "AU_SSPProgramDesignFinalApproval", param.ToArray());
                }
                else
                {
                    SqlHelper.ExecuteScalar(SqlHelper.GetCon(), CommandType.StoredProcedure, "AU_SSPProgramDesignFinalApproval", param.ToArray());
                }
                return true;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public DataTable LoopinData(DataTable dt, string[] attachmentColumns)
        {
            DataTable modifiedDataTable = dt.Clone();

            foreach (DataRow row in dt.Rows)
            {
                // Update all attachments in one go using the passed attachment columns array
                foreach (string attachmentColumn in attachmentColumns)
                {
                    UpdateAttachment(row, attachmentColumn);
                }

                modifiedDataTable.ImportRow(row);
            }

            return modifiedDataTable;
        }

        private void UpdateAttachment(DataRow row, string columnName)
        {
            string attachment = row[columnName].ToString();

            if (string.IsNullOrEmpty(attachment))
            {
                row[columnName] = "";
            }
            else
            {
                row[columnName] = Common.GetFileBase64(attachment);
                var test = Common.GetFileBase64(attachment);
            }
        }


        public DataTable GetTSPData(int TSPID)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@UserID", TSPID));

                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_SSPTSPProfile", param.ToArray()).Tables[0];
                return dt;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        private void SaveTSPRegistrationPayment(Object data)
        {
            // Calling PayPro API
            DataTable dt = FetchDropDownList("RD_SSPPayProMaxOrderNumber");
            DataTable TSP = GetTSPData(422);

            var phoneNumber = "0311-4358506".Replace("-", "");
            var orderNumber = Convert.ToInt32(dt.Rows[0]["OrderNumber"]);
            var orderAmount = "5000";
            DateTime issueDate = DateTime.Now.Date;
            DateTime dueDate = DateTime.Today.AddDays(7).Date;

            var order = new
            {
                MerchantId = "PSDF",
                MerchantPassword = "Live@psdf21",
                OrderNumber = orderNumber,
                OrderAmount = orderAmount,
                OrderDueDate = dueDate.ToString("dd/MM/yyyy"),
                OrderAmountWithinDueDate = orderAmount,
                OrderAmountAfterDueDate = orderAmount,
                OrderType = "Service",
                IssueDate = issueDate.ToString("dd/MM/yyyy"),
                OrderExpireAfterSeconds = 0,
                CustomerName = TSP.Rows[0]["InstituteName"],
                CustomerMobile = TSP.Rows[0]["HeadMobile"],
                CustomerEmail = TSP.Rows[0]["HeadEmail"],
                CustomerAddress = TSP.Rows[0]["Address"]
            };

            var jsonOrder = JsonConvert.SerializeObject(new[] { order });

            string baseUrl = "https://api.paypro.com.pk/";
            string reqUri = baseUrl + "cpay/co?oJson=" + jsonOrder;
            using (WebClient client = new WebClient())
            {
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                var response = client.UploadString(reqUri, jsonOrder);
                var model = JsonConvert.DeserializeObject<IList<PayProResponseModel>>(response);

                if (model[0].Status == "00")
                {
                    var OrderNumber = model[1].OrderNumber;
                    var Status = model[0].Status;
                    var Description = model[1].Description;
                    var OrderType = "Service";
                    var OrderExpireAfterSeconds = 0;
                    var ConnectPayId = model[1].ConnectPayId.ToString();
                    var IsFeeApplied = model[1].IsFeeApplied;
                    var Click2Pay = model[1].Click2Pay.ToString();
                }
            }

        }





    }
}
