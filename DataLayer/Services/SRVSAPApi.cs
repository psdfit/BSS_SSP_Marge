using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using DataLayer.Interfaces;
using DataLayer.Models;
using Newtonsoft.Json.Schema;
using System.Linq;
using Microsoft.Extensions.Configuration;
using System.Data;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Globalization;
using Newtonsoft.Json.Linq;

namespace DataLayer.Services
{
    public class SRVSAPApi : SRVBase, DataLayer.Interfaces.ISRVSAPApi
    {
        private readonly IConfiguration configuration;
        private readonly string sapHost;
        private long requestTimeoutInMiliSeconds;

        public SRVSAPApi(IConfiguration configuration)
        {
            this.configuration = configuration;
            sapHost = configuration.GetValue<string>("AppSettings:SAP:Host");
            requestTimeoutInMiliSeconds = configuration.GetValue<long>("AppSettings:SAP:RequestTimeOut"); ;
        }
        public async Task<SAPResponseModel> SaveSAPCostCenterForScheme(SchemeModel scheme, SqlTransaction transaction = null)
        {
            try
            {
                if (scheme == null)
                {
                    throw new Exception("Scheme's details not found.");
                }
                var token = GetTokenAsync().Result;
                if (!string.IsNullOrEmpty(token))
                {

                    //var url = string.Format("{0}/api/CostCenter", sapHost);
                    var model = new CostCenterModel();
                    model.U_BSS_Id = scheme.SchemeID.ToString();
                    model.PrcName = (string.IsNullOrEmpty(scheme.SchemeName)) ? string.Empty : scheme.SchemeName;
                    model.GrpCode = string.Empty;
                    model.CCTypeCode = string.Empty;
                    model.ValidFrom = ConvertAndFormatDate(DateTime.Now.ToString());
                    model.ValidTo = string.Empty;
                    model.CCOwner = string.Empty;
                    model.Active = "Y";
                    model.U_Cost_Cntr_Name = string.IsNullOrEmpty(scheme.Description) ? string.Empty : scheme.Description;
                    model.Type = "Scheme";
                    model.TradeType = string.Empty;
                    var myJson = JsonConvert.SerializeObject(model);
                    using (var httpClient = new HttpClient())
                    {
                    
                        //var httpClient = new HttpClient();
                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                        httpClient.Timeout = TimeSpan.FromMilliseconds(requestTimeoutInMiliSeconds);
                        var request = await httpClient.PostAsync($@"{sapHost}/api/CostCenter", new StringContent(myJson, Encoding.UTF8, "application/json"));
                        string resultContent = await request.Content.ReadAsStringAsync();
                        SAPResponseModel response = JsonConvert.DeserializeObject<SAPResponseModel>(resultContent);
                        //response.StatusBit = response.Status == "0" ? false : response.Status == "1" ? true : false;
                        if (string.IsNullOrEmpty(response?.SapObjId))
                        {
                            throw new Exception($"SAP : {response?.Message ?? "Invalid response."}");
                        }
                        return response;
                    }
                }
                else
                {
                    throw new Exception("SAP User Login Failed");
                }
            }
            catch (Exception e)
            {
                throw new Exception($"SAP Service : {e.Message}", e);
            }
        }
        public async Task<SAPResponseModel> SaveSAPCostCenterForTrade(TradeCategoryModel tradeDetails, SqlTransaction transaction = null)
        {
            try
            {
                if (tradeDetails == null)
                {
                    throw new Exception("Trade's details not found");
                }
                var tradeType = string.Empty;
                if (!string.IsNullOrEmpty(tradeDetails.CertificationCategoryName))
                {
                    if (tradeDetails.CertificationCategoryName == "Local")
                    {
                        tradeType = "National";
                    }
                    if (tradeDetails.CertificationCategoryName == "International")
                    {
                        tradeType = "International";
                    }
                }
                var token = GetTokenAsync().Result;
                if (!string.IsNullOrEmpty(token))
                {

                    // var url = string.Format("{0}/api/CostCenter", sapHost);
                    var model = new CostCenterModel();
                    model.U_BSS_Id = tradeDetails.TradeID.ToString();
                    model.PrcName = string.IsNullOrEmpty(tradeDetails.TradeName) ? string.Empty : tradeDetails.TradeName.Substring(0, tradeDetails.TradeName.Length >= 30 ? 30 : tradeDetails.TradeName.Length);
                    model.GrpCode = string.Empty;
                    model.CCTypeCode = string.Empty;
                    model.ValidFrom = ConvertAndFormatDate(DateTime.Now.ToString());
                    model.ValidTo = string.Empty;
                    model.CCOwner = string.Empty;
                    model.Active = "Y";
                    model.U_Cost_Cntr_Name = string.IsNullOrEmpty(tradeDetails.TradeName) ? string.Empty : tradeDetails.TradeName;
                    model.Type = "Trade";
                    model.TradeType = tradeType;
                    var myJson = JsonConvert.SerializeObject(model);
                    using (var httpClient = new HttpClient())
                    {
                        //var httpClient = new HttpClient();
                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                        httpClient.Timeout = TimeSpan.FromMilliseconds(requestTimeoutInMiliSeconds);
                        var request = await httpClient.PostAsync($@"{sapHost}/api/CostCenter", new StringContent(myJson, Encoding.UTF8, "application/json"));
                        string resultContent = await request.Content.ReadAsStringAsync();
                        SAPResponseModel response = JsonConvert.DeserializeObject<SAPResponseModel>(resultContent);
                        if (string.IsNullOrEmpty(response?.SapObjId))
                        {
                            throw new Exception($"SAP : {response?.Message ?? "Invalid response."}");
                        }
                        return response;
                    }
                }
                else
                {
                    throw new Exception("SAP User Login Failed");
                    //transaction.Rollback();
                    //return new SAPResponseModel()
                    //{
                    //    Message = "Unable to login user",
                    //    Status = false,
                    //    SapObjId = null,
                    //};
                }

            }
            catch (Exception e)
            {
                throw new Exception($"SAP Service : {e.Message}", e);
            }
        }
        public async Task<POSAPResponseModel> SaveSAPPurchaseOrder(POHeaderModel poHeader, List<POLinesModel> poLines, SqlTransaction transaction = null)
        {
            try
            {
                var valid = false;
                var token = GetTokenAsync().Result;
                if (!string.IsNullOrEmpty(token))
                {
                    if (poHeader.DocDate <= DateTime.Now || poHeader.DocDueDate >= DateTime.Now)
                    {
                        valid = true;
                    }
                    else
                    {
                        valid = false;
                    }
                    if (valid)
                    {
                        var model = new PurchaseOrderSAPModel();
                        DateTime MonthDate = (DateTime)poHeader.Month;

                        model.U_IPS = poHeader.POHeaderID;
                        model.Printed = string.IsNullOrEmpty(poHeader.Printed) ? string.Empty : poHeader.Printed;
                        model.DocDate = ConvertAndFormatDate(poHeader.DocDate == null ? string.Empty : poHeader.DocDate.ToString());
                        model.DocDueDate = ConvertAndFormatDate(poHeader.DocDueDate == null ? string.Empty : poHeader.DocDueDate.ToString());
                        // model.CardCode = string.IsNullOrEmpty(poHeader.CardCode) ? string.Empty : poHeader.CardCode;
                        model.CardCode = !string.IsNullOrEmpty(poHeader.CardCode) ? poHeader.CardCode.Substring(0, poHeader.CardCode.Length > 15 ? 15 : poHeader.CardCode.Length) : string.Empty;
                        //model.CardName = string.IsNullOrEmpty(poHeader.CardName) ? string.Empty : poHeader.CardName;
                        model.CardName = !string.IsNullOrEmpty(poHeader.CardName) ? poHeader.CardName.Substring(0, poHeader.CardName.Length > 100 ? 100 : poHeader.CardName.Length) : string.Empty;
                        // model.JournalMemo = string.IsNullOrEmpty(poHeader.JournalMemo) ? string.Empty : poHeader.JournalMemo;
                        model.JournalMemo = !string.IsNullOrEmpty(poHeader.JournalMemo) ? poHeader.JournalMemo.Substring(0, poHeader.JournalMemo.Length > 50 ? 50 : poHeader.JournalMemo.Length) : string.Empty;

                        //model.Comments = string.IsNullOrEmpty(poHeader.Comments) ? string.Empty : poHeader.Comments;
                        model.Comments = !string.IsNullOrEmpty(poHeader.Comments) ? poHeader.Comments.Substring(0, poHeader.Comments.Length > 254 ? 254 : poHeader.Comments.Length) : string.Empty;

                        //model.U_Month = poHeader.Month;
                        model.U_Month = MonthDate.ToString("MMMM", CultureInfo.InvariantCulture);
                        //model.U_SCH_Code = string.IsNullOrEmpty(poHeader.U_Sch_Code) ? string.Empty : poHeader.U_Sch_Code;
                        model.U_SCH_Code = !string.IsNullOrEmpty(poHeader.U_Sch_Code) ? poHeader.U_Sch_Code.Substring(0, poHeader.U_Sch_Code.Length > 30 ? 30 : poHeader.U_Sch_Code.Length) : string.Empty;
                        //model.U_SCHEME = string.IsNullOrEmpty(poHeader.U_SCHEME) ? string.Empty : poHeader.U_SCHEME.Trim();
                        model.U_SCHEME = !string.IsNullOrEmpty(poHeader.U_SCHEME) ? poHeader.U_SCHEME.Substring(0, poHeader.U_SCHEME.Length > 15 ? 15 : poHeader.U_SCHEME.Length) : string.Empty;
                        model.BranchID = poHeader.BPLId.Value;
                        model.PODetail = poLines.Select(x => new POLinesSAP()
                        {

                            //Description = string.IsNullOrEmpty(x.Dscription) ? string.Empty : x.Dscription,
                            Description = !string.IsNullOrEmpty(x.Dscription) ? x.Dscription.Substring(0, x.Dscription.Length > 100 ? 100 : x.Dscription.Length) : string.Empty,
                            //AcctCode = string.IsNullOrEmpty(x.AcctCode) ? string.Empty : x.AcctCode,
                            AcctCode = !string.IsNullOrEmpty(x.AcctCode) ? x.AcctCode.Substring(0, x.AcctCode.Length > 15 ? 15 : x.AcctCode.Length) : string.Empty,
                            //OcrCode = string.IsNullOrEmpty(x.OcrCode) ? string.Empty : x.OcrCode,
                            OcrCode = !string.IsNullOrEmpty(x.OcrCode) ? x.OcrCode.Substring(0, x.OcrCode.Length > 8 ? 8 : x.OcrCode.Length) : string.Empty,
                            //TaxCode = string.IsNullOrEmpty(x.TaxCode) ? string.Empty : x.TaxCode,
                            TaxCode = !string.IsNullOrEmpty(x.TaxCode) ? x.TaxCode.Substring(0, x.TaxCode.Length > 8 ? 8 : x.TaxCode.Length) : string.Empty,
                            //WTLiable = string.IsNullOrEmpty(x.WtLiable) ? string.Empty : x.WtLiable,
                            WTLiable = !string.IsNullOrEmpty(x.WtLiable) ? x.WtLiable.Substring(0, x.WtLiable.Length > 1 ? 1 : x.WtLiable.Length) : string.Empty,

                            LineTotal = x.LineTotal == null ? string.Empty : x.LineTotal.ToString(),
                            LineStatus = string.IsNullOrEmpty(x.LineStatus) ? string.Empty : x.LineStatus,
                            U_Class_Code = x.U_Class_Code,
                            U_Batch = string.IsNullOrEmpty(x.U_Batch) ? string.Empty : x.U_Batch,
                            U_Batch_Duration = string.IsNullOrEmpty(x.U_Batch_Duration) ? string.Empty : x.U_Batch_Duration,
                            U_Training_Cost = x.U_Training_Cost == null ? string.Empty : x.U_Training_Cost.ToString(),
                            U_Stipend = x.U_Stipend == null ? string.Empty : x.U_Stipend.ToString(),
                            U_Uniform_Bag = x.U_Uniform_Bag == null ? string.Empty : x.U_Uniform_Bag.ToString(),
                            U_Trainee_Per_Class = x.U_Trainee_Per_Class.ToString(),
                            U_Testing_Fee = x.U_Testing_Fee == null ? string.Empty : x.U_Testing_Fee.ToString(),
                            U_Cost_Trainee_LMont = x.U_Cost_Trainee_LMont == null ? string.Empty : x.U_Cost_Trainee_LMont.ToString(),
                            U_Class_Start_Date = ConvertAndFormatDate(x.U_Class_Start_Date.ToString()), //Changed By Rao Ali Haider
                            U_Class_End_Date = ConvertAndFormatDate(x.U_Class_End_Date.ToString()),//Changed By Rao Ali Haider
                            U_Cost_Trainee_2Mont = x.U_Cost_Trainee_2Mont == null ? string.Empty : x.U_Cost_Trainee_2Mont.ToString(),
                            OcrCode2 = x.OcrCode2 == null ? string.Empty : x.OcrCode2.ToString(),
                            OcrCode3 = x.OcrCode3 == null ? string.Empty : x.OcrCode3.ToString(),
                            U_Cost_Trainee_FMont = x.U_Cost_Trainee_FMont == null ? string.Empty : x.U_Cost_Trainee_FMont.ToString(),
                            U_Cost_Trai_2nd_Last = x.U_Cost_Trai_2nd_Last == null ? string.Empty : x.U_Cost_Trai_2nd_Last.ToString(),
                            U_Cost_Trainee_Month = x.U_Cost_Trainee_Month == null ? string.Empty : x.U_Cost_Trainee_Month.ToString(),

                        }).ToList();
                        using (var httpClient = new HttpClient())
                        {
                            //var httpClient = new HttpClient();
                            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                            httpClient.Timeout = TimeSpan.FromMilliseconds(requestTimeoutInMiliSeconds);
                            //var url = string.Format("{0}/api/PurchaseOrder", sapHost);
                            var myJson = JsonConvert.SerializeObject(model);
                            var jsonString = JToken.Parse(myJson);
                            var request = await httpClient.PostAsync($@"{sapHost}/api/PurchaseOrder", new StringContent(myJson, Encoding.UTF8, "application/json"));
                            string resultContent = await request.Content.ReadAsStringAsync();
                            POSAPResponseModel response = JsonConvert.DeserializeObject<POSAPResponseModel>(resultContent);
                            if (response.Status == "1")
                            {
                                return response;
                            }
                            else
                            {
                                throw new Exception(response.Message);
                            }
                        }
                    }
                    else
                    {
                        throw new Exception("Purchase order data provided is invalid state");
                    }

                }
                else
                {
                    throw new Exception("SAP User Login Failed");
                }

            }
            catch (Exception e)
            {
                throw new Exception($"SAP Service : {e.Message}", e);
            }
        }

        /// <summary>
        /// Created By Rao Ali Haider
        /// 27-Dec-2023
        /// SAP Date Update
        /// </summary>
        /// <param name="inputDateString"></param>
        /// <returns></returns>

        static string ConvertAndFormatDate(string inputDateString)
        {
            // Parse the input date
            if (DateTime.TryParseExact(inputDateString, "dd/MM/yyyy h:mm:ss tt", null, System.Globalization.DateTimeStyles.None, out DateTime inputDate))
            {
                string formattedDateString = inputDate.ToString("MM/dd/yyyy h:mm:ss tt");

                return formattedDateString;
            }
            else
            {
                return inputDateString;
            }
        }

        public async Task<SAPResponseModel> SaveSAPAPInvoice(InvoiceMasterModel invoiceHeader, List<InvoiceModel> invoices, SqlTransaction transaction = null)
        {
            try
            {
                var token = GetTokenAsync().Result;
                if (!string.IsNullOrEmpty(token))
                {

                    var model = new InvoiceSAPModel();
                    DateTime MonthDate = invoiceHeader.U_Month.Value;

                    model.U_IPS = invoiceHeader.InvoiceHeaderID;
                    model.CardCode = string.IsNullOrEmpty(invoiceHeader.CardCode) ? string.Empty : invoiceHeader.CardCode;
                    model.CardName = string.IsNullOrEmpty(invoiceHeader.CardName) ? string.Empty : invoiceHeader.CardName;
                    model.DocDate = ConvertAndFormatDate(invoiceHeader.DocDate == null ? string.Empty : invoiceHeader.DocDate.ToString());
                    model.DocDueDate = ConvertAndFormatDate(invoiceHeader.DocDueDate == null ? string.Empty : invoiceHeader.DocDueDate.ToString());
                    model.TaxDate = ConvertAndFormatDate(DateTime.Now.ToString());
                    model.U_Scheme = string.IsNullOrEmpty(invoiceHeader.U_SCHEME.Trim()) ? string.Empty : invoiceHeader.U_SCHEME.Trim();
                    model.U_sch_code = string.IsNullOrEmpty(invoiceHeader.U_SCH_Code) ? string.Empty : invoiceHeader.U_SCH_Code;
                    model.U_Month = MonthDate.ToString("MMMM", CultureInfo.InvariantCulture); //January
                    model.BranchID = invoiceHeader.BPL_IDAssignedToInvoice;
                    string ProcessKeyTemp = invoiceHeader.ProcessKey;
                    if (invoiceHeader.ProcessKey == EnumApprovalProcess.INV_C)
                    {
                        model.U_Month = "Final 1";
                    }
                    else if (invoiceHeader.ProcessKey == EnumApprovalProcess.INV_F)
                    {
                        model.U_Month = "Final 2";
                    }
                    else if (invoiceHeader.ProcessKey == EnumApprovalProcess.INV_TRN)
                    {
                        model.U_Month = "Testing & Certification";
                    }

                    model.APInvoiceDetail = invoices.Select(x => new ApinvoiceDetail()
                    {
                        U_Class_Code = string.IsNullOrEmpty(x.ClassCode) ? string.Empty : x.ClassCode,
                        U_Invoice_Number = x.InvoiceType,
                        AcctCode = x.GLCode,
                        OcrCode = x.OcrCode,
                        OcrCode2 = x.OcrCode2,
                        U_Batch_Duration = x.BatchDuration.ToString(),//Changed by Rao ALi Haider 01-Jan-2024
                        U_Class_Start_Date = ConvertAndFormatDate(x.StartDate.ToString()),
                        U_Class_End_Date = ConvertAndFormatDate(x.EndDate.ToString()),
                        U_Class_AStart_Date = ConvertAndFormatDate(x.ActualStartDate.ToString()),
                        U_Class_AEnd_Date = ConvertAndFormatDate(x.ActualEndDate.ToString()),
                        U_Tranee_Per_Class = x.TraineePerClass,
                        U_Batch = x.Batch.ToString(),
                        U_nettrainingcost = invoiceHeader.ProcessKey == EnumApprovalProcess.INV_SRN ? "0" : x.NetTrainingCost.ToString(),
                        U_Stipend_Per_Traine = invoiceHeader.ProcessKey == EnumApprovalProcess.INV_SRN ? x.TotalMonthlyPayment.ToString() : x.Stipend.ToString(),
                        U_Uniform_Bag = x.UniformBag.ToString(),
                        U_Claimed_Trainee = x.ClaimTrainees.ToString(),
                        U_No_Unverified_CNIC = x.UnverifiedCNICDeductions.ToString(),
                        U_CNIC_Cat = x.CnicDeductionType.ToString(),
                        U_Unverified_CNIC = x.CnicDeductionAmount.ToString(),
                        U_No_Dropout = x.DeductionTraineeDroput.ToString(),
                        U_Drpoutded = x.DropOutDeductionType, //dropout amout
                        U_Dropout = x.DropOutDeductionAmount,
                        U_No_Attend_Shortfal = x.DeductionTraineeAttendance,
                        U_Attend_Shortfall = x.AttendanceDeductionAmount,
                        U_No_Misc_Deduction = x.MiscDeductionNo,
                        U_MiscDedCat = x.MiscDeductionType,
                        U_Misc_Deduction = x.MiscDeductionAmount,
                        U_Penalty = x.PenaltyAmount,
                        U_Percapld = x.PenaltyPercentage,
                        U_Result_Deduction = x.ResultDeduction,
                        U_GrossPayable = x.GrossPayable,
                        OcrCode3 = x.OcrCode3,
                        OcrCode4 = Convert.ToString(invoiceHeader.BPL_IDAssignedToInvoice),
                        Description = !string.IsNullOrEmpty(x.Description) ? x.Description.Substring(0, x.Description.Length > 100 ? 100 : x.Description.Length) : string.Empty,
                        U_Claimed_Trainees = x.ClaimTrainees.ToString(),
                        WtLiable = x.WTaxLiable,
                        U_Total_Monthly_Pay = x.TotalMonthlyPayment,
                        U_Testing_Fee = x.TestingFee,
                        U_Boarding_Loadging = x.BoardingOrLodging,
                        U_T_Cost_Per_Trainee = x.TotalCostPerTrainee,
                        U_Net_Invoice_Pay = x.NetPayableAmount,
                        U_NoProfileDed = x.MiscProfileDeductionCount,
                        U_Stipend = x.Stipend,
                        U_Location = "NoLocation",
                        LineTotal = x.TotalLC,
                        U_Linetotal = x.LineTotal,
                        VatGroup = x.TaxCode,
                        LineStatus = x.LineStatus,
                        U_AP_Ref = string.Empty,
                        U_Remarks = "AP Invoice Draft",
                        U_Trainee_Per_Class = x.TraineePerClass,
                        //PODocEntry = x.BaseEntry+string.Empty,
                        BaseEntry = x.BaseEntry.ToString(),
                        BaseType = x.BaseType.ToString(),
                        BaseLine = x.BaseLine
                    }).ToList();

                    using (var httpClient = new HttpClient())
                    {
                        //var httpClient = new HttpClient();

                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                        httpClient.Timeout = TimeSpan.FromMilliseconds(requestTimeoutInMiliSeconds);
                        //var url = string.Format("{0}/api/APInvoice", sapHost);
                        var myJson = JsonConvert.SerializeObject(model);
                        var jsonString = JToken.Parse(myJson);
                        var request = await httpClient.PostAsync($@"{sapHost}/api/APInvoice", new StringContent(myJson, Encoding.UTF8, "application/json"));
                        string resultContent = await request.Content.ReadAsStringAsync();
                        SAPResponseModel response = JsonConvert.DeserializeObject<SAPResponseModel>(resultContent);
                        //response.StatusBit = response?.Status == "0" ? false : response?.Status == "1" ? true : false;
                        if (string.IsNullOrEmpty(response?.SapObjId))
                        {
                            throw new Exception($"SAP : {response?.Message ?? "Invalid response."}");
                        }
                        return response;
                    }
                }
                else
                {
                    throw new Exception("SAP User Login Failed");
                }

            }
            catch (Exception e)
            {
                throw new Exception($"SAP Service : {e.Message}", e);
            }
        }
        public async Task<List<BranchesItems>> FetchSAPBranches()
        {
            try
            {
                var token = GetTokenAsync().Result;
                if (!string.IsNullOrEmpty(token))
                {
                    //var url = string.Format("{0}/api/Branch/Get", sapHost);
                    using (var httpClient = new HttpClient())
                    {
                        //var httpClient = new HttpClient();
                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                        httpClient.Timeout = TimeSpan.FromMilliseconds(requestTimeoutInMiliSeconds);
                        var response = await httpClient.GetAsync($@"{sapHost}/api/Branch/Get");
                        string resultContent = await response.Content.ReadAsStringAsync();
                        var converted = JsonConvert.DeserializeObject<SAPBranchesViewModel>(resultContent);
                        var bList = JsonConvert.DeserializeObject<List<BranchesItems>>(converted.BranchesList);

                        return bList.OrderBy(x => x.BranchId).ToList();
                    }
                }
                else
                {
                    return new List<BranchesItems>();
                }
            }
            catch (Exception e)
            {
                throw new Exception($"SAP Service : {e.Message}", e);
            }
        }
        private async Task<string> GetTokenAsync()
        {
            var res = string.Empty;
            var sap = configuration.GetSection("AppSettings").GetSection("SAP");
            try
            {
                using (var client = new HttpClient())
                {
                    string userName = sap.GetSection("UserName").Value;
                    string password = sap.GetSection("Password").Value;

                    var url = $@"{sapHost}/api/auth/login?username={userName}&password={password}";
                    //var httpClient = new HttpClient();
                    var response = await client.PostAsync(url, new StringContent(string.Empty));
                    string resultContent = await response.Content.ReadAsStringAsync();
                    dynamic d = JsonConvert.DeserializeObject<dynamic>(resultContent);
                    res = d;
                }
            }
            catch (Exception e)
            {
                throw new Exception($"SAP Service : {e.Message}", e);
            }
            return res;
        }
        public async Task<bool> SynceBranches()
        {
            try
            {
                var token = GetTokenAsync().Result;
                if (!string.IsNullOrEmpty(token))
                {
                    var url = string.Format("{0}/api/Branch/Get", sapHost);
                    using (var httpClient = new HttpClient())
                    {
                        //var httpClient = new HttpClient();
                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                        httpClient.Timeout = TimeSpan.FromMilliseconds(requestTimeoutInMiliSeconds);
                        var response = await httpClient.GetAsync($@"{sapHost}/api/Branch/Get");
                        string resultContent = await response.Content.ReadAsStringAsync();
                        var converted = JsonConvert.DeserializeObject<SAPBranchesViewModel>(resultContent);
                        var bList = JsonConvert.DeserializeObject<List<BranchesItems>>(converted.BranchesList);
                        var list = bList.Select(x => new SAPBranchesModel
                        {
                            BranchId = x.BranchId,
                            BranchName = x.BranchName

                        }).ToList();
                        var id = BatchInsertBranches(list);
                        return true;
                    }
                }
                else
                {

                    return false;
                }

            }
            catch (Exception e)
            {
                return false;
            }
        }
        public int BatchInsertBranches(List<SAPBranchesModel> ls)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@Json", JsonConvert.SerializeObject(ls));
                return SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[BAU_SAPBranches]", param);
            }
            catch (Exception e)
            {
                throw new Exception($"SAP Service : {e.Message}", e);
            }
        }
        public async Task<SAPResponseModel> SaveSAPBusinessPartnerForTSP(TSPDetailModel tsp, SqlTransaction transaction = null)
        {
            try
            {

                var token = GetTokenAsync().Result;
                if (!string.IsNullOrEmpty(token))
                {

                    //var url = string.Format("{0}/api/BusinessPartner", sapHost);
                    var model = new BusinessPartnerModel();
                    //model.CardName = string.IsNullOrEmpty(tsp.TSPName) ? string.Empty : tsp.TSPName;
                    //model.HouseBankAccount = "5435-3";
                    //model.Currency = "PKR";
                    //model.Email = string.IsNullOrEmpty(tsp.CPAccountsEmail) ? string.Empty : tsp.CPAccountsEmail;
                    //model.Address = string.IsNullOrEmpty(tsp.Address) ? string.Empty : tsp.Address;
                    //model.Phone = string.IsNullOrEmpty(tsp.CPLandline) ? string.Empty : tsp.CPLandline;
                    //model.ZipCode = "11111";
                    //model.City = string.Empty;
                    //model.CountryCode = "PK";

                    //model.CardName = tsp.TSPName ?? string.Empty;
                    model.AddUpdate = "1";                          // 1 for update
                    model.CardName = !string.IsNullOrEmpty(tsp.TSPName) ? tsp.TSPName.Substring(0, tsp.TSPName.Length > 100 ? 100 : tsp.TSPName.Length) : string.Empty;
                    model.HouseBankAccount = "5435-3";//hardcode
                    model.Currency = "PKR";//hardcode
                    // model.VatIdUnCmp = tsp.NTN ?? string.Empty;
                    model.VatIdUnCmp = !string.IsNullOrEmpty(tsp.NTN) ? tsp.NTN.Substring(0, tsp.NTN.Length > 32 ? 32 : tsp.NTN.Length) : string.Empty;

                    model.Phone = string.Empty;
                    //model.AddID = tsp.GST ?? string.Empty;
                    model.AddID = !string.IsNullOrEmpty(tsp.GST) ? tsp.GST.Substring(0, tsp.GST.Length > 64 ? 64 : tsp.GST.Length) : string.Empty;
                    model.TaxPayerName = string.Empty;
                    model.PNTN = $"PNTN: {tsp.PNTN ?? string.Empty}";
                    //model.e_mail = tsp.HeadEmail ?? string.Empty;
                    model.e_mail = !string.IsNullOrEmpty(tsp.HeadEmail) ? tsp.HeadEmail.Substring(0, tsp.HeadEmail.Length > 100 ? 100 : tsp.HeadEmail.Length) : string.Empty;

                    // model.Cellular = tsp.HeadLandline ?? string.Empty;
                    model.Cellular = !string.IsNullOrEmpty(tsp.HeadLandline) ? tsp.HeadLandline.Substring(0, tsp.HeadLandline.Length > 20 ? 20 : tsp.HeadLandline.Length) : string.Empty;
                    //model.Phone2 = tsp.OrgLandline ?? string.Empty;
                    model.Phone2 = !string.IsNullOrEmpty(tsp.OrgLandline) ? tsp.OrgLandline.Substring(0, tsp.OrgLandline.Length > 20 ? 20 : tsp.OrgLandline.Length) : string.Empty;
                    //model.IntrntSite = tsp.Website ?? string.Empty;
                    model.IntrntSite = !string.IsNullOrEmpty(tsp.Website) ? tsp.Website.Substring(0, tsp.Website.Length > 100 ? 100 : tsp.Website.Length) : string.Empty;
                    //model.U_Account = tsp.BankAccountNumber ?? string.Empty;
                    model.U_Account = !string.IsNullOrEmpty(tsp.BankAccountNumber) ? tsp.BankAccountNumber.Substring(0, tsp.BankAccountNumber.Length > 50 ? 50 : tsp.BankAccountNumber.Length) : string.Empty;

                    //model.U_AT = tsp.AccountTitle ?? string.Empty;
                    model.U_AT = !string.IsNullOrEmpty(tsp.AccountTitle) ? tsp.AccountTitle.Substring(0, tsp.AccountTitle.Length > 254 ? 254 : tsp.AccountTitle.Length) : string.Empty;
                    //model.BankName = !string.IsNullOrEmpty(tsp.BankName) ? tsp.BankName.Substring(0, tsp.BankName.Length > 254 ? 254 : tsp.BankName.Length) : string.Empty;
                    //model.BankBranch = !string.IsNullOrEmpty(tsp.BankBranch) ? tsp.BankBranch.Substring(0, tsp.BankBranch.Length > 254 ? 254 : tsp.BankBranch.Length) : string.Empty;
                    model.AddressList = new List<dynamic>()
                    {
                        new {
                            //Address=tsp.Address?? string.Empty,
                            Address=!string.IsNullOrEmpty(tsp.Address)?tsp.Address.Substring(0,tsp.Address.Length>50?50:tsp.Address.Length):string.Empty,
                            ADDRESS2=string.Empty,
                            CITY=tsp.DistrictName?? string.Empty,
                            ADRESTYPE=1
                        }
                    };
                    model.ContactPersons = new List<dynamic>()
                    {
                        new {
                           // Name=tsp.HeadName?? string.Empty,
                            Name=!string.IsNullOrEmpty(tsp.HeadName)?tsp.HeadName.Substring(0,tsp.HeadName.Length>50?50:tsp.HeadName.Length):string.Empty,
                            //FirstName=tsp.HeadName?? string.Empty,
                            FirstName=!string.IsNullOrEmpty(tsp.HeadName)?tsp.HeadName.Substring(0,tsp.HeadName.Length>50?50:tsp.HeadName.Length):string.Empty,
                            Title=!string.IsNullOrEmpty(tsp.HeadDesignation)?tsp.HeadDesignation.Substring(0,tsp.HeadDesignation.Length>10?10:tsp.HeadDesignation.Length):string.Empty,
                            //Cellolar=tsp.HeadLandline?? string.Empty,
                            Cellolar=!string.IsNullOrEmpty(tsp.HeadLandline)?tsp.HeadLandline.Substring(0,tsp.HeadLandline.Length>50?50:tsp.HeadLandline.Length):string.Empty,
                           // E_MailL=tsp.HeadEmail ?? string.Empty,
                            E_MailL=!string.IsNullOrEmpty(tsp.HeadEmail)?tsp.HeadEmail.Substring(0,tsp.HeadEmail.Length>100?100:tsp.HeadEmail.Length):string.Empty,
                        },
                         new {
                            //Name=tsp.CPName?? string.Empty,
                            Name=!string.IsNullOrEmpty(tsp.CPName)?tsp.CPName.Substring(0,tsp.CPName.Length>50?50:tsp.CPName.Length):string.Empty,
                            //FirstName=tsp.CPName?? string.Empty,
                            FirstName=!string.IsNullOrEmpty(tsp.CPName)?tsp.CPName.Substring(0,tsp.CPName.Length>50?50:tsp.CPName.Length):string.Empty,
                            Title=!string.IsNullOrEmpty(tsp.CPDesignation)?tsp.CPDesignation.Substring(0,tsp.CPDesignation.Length>10?10:tsp.CPDesignation.Length):string.Empty,
                            //Cellolar=tsp.CPLandline?? string.Empty,
                            Cellolar=!string.IsNullOrEmpty(tsp.CPLandline)?tsp.CPLandline.Substring(0,tsp.CPLandline.Length>50?50:tsp.CPLandline.Length):string.Empty,
                            //E_MailL=tsp.CPEmail ?? string.Empty,
                            E_MailL=!string.IsNullOrEmpty(tsp.CPEmail)?tsp.CPEmail.Substring(0,tsp.CPEmail.Length>100?100:tsp.CPEmail.Length):string.Empty,
                        }
                    };

                    var myJson = JsonConvert.SerializeObject(model);
                    using (var httpClient = new HttpClient())
                    {
                        //var httpClient = new HttpClient();
                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                        httpClient.Timeout = TimeSpan.FromMilliseconds(requestTimeoutInMiliSeconds);
                        var request = await httpClient.PostAsync($@"{sapHost}/api/BusinessPartner", new StringContent(myJson, Encoding.UTF8, "application/json"));
                        string resultContent = await request.Content.ReadAsStringAsync();

                        SAPResponseModel response = JsonConvert.DeserializeObject<SAPResponseModel>(resultContent);
                        //response.StatusBit = response.Status == "0" ? false : response.Status == "1" ? true : false;
                        if (string.IsNullOrEmpty(response?.SapObjId))
                        {
                            throw new Exception($"SAP : {response?.Message ?? "Invalid response."} {", TSP Name: " + tsp.TSPName }");
                        }
                        return response;
                    }
                }
                else
                {
                    throw new Exception("SAP User Login Failed");
                }
            }
            catch (Exception e)
            {
                throw new Exception($"SAP Service : {e.Message}", e);
            }
        }
        public async Task<SAPResponseModel> SaveSAPBusinessPartnerForTSPUpdate(TSPChangeRequestModel tsp, SqlTransaction transaction = null)
        {
            try
            {

                var token = GetTokenAsync().Result;
                if (!string.IsNullOrEmpty(token))
                {

                    var model = new BusinessPartnerModel();


                    model.AddUpdate = tsp.AddUpdate;                 // 2 for update
                    model.CardCode = !string.IsNullOrEmpty(tsp.SAPID) ? tsp.SAPID.Substring(0, tsp.SAPID.Length > 100 ? 100 : tsp.SAPID.Length) : string.Empty;
                    model.CardName = !string.IsNullOrEmpty(tsp.TSPName) ? tsp.TSPName.Substring(0, tsp.TSPName.Length > 100 ? 100 : tsp.TSPName.Length) : string.Empty;
                    model.HouseBankAccount = "5435-3";//hardcode
                    model.Currency = "PKR";//hardcode                    
                    //model.VatIdUnCmp = !string.IsNullOrEmpty(tsp.NTN) ? tsp.NTN.Substring(0, tsp.NTN.Length > 32 ? 32 : tsp.NTN.Length) : string.Empty;

                    model.Phone = string.Empty;
                    //model.AddID = tsp.GST ?? string.Empty;
                    // model.AddID = !string.IsNullOrEmpty(tsp.GST) ? tsp.GST.Substring(0, tsp.GST.Length > 64 ? 64 : tsp.GST.Length) : string.Empty;
                    //model.TaxPayerName = string.Empty;
                    //  model.PNTN = $"PNTN: {tsp.PNTN ?? string.Empty}";
                    model.e_mail = !string.IsNullOrEmpty(tsp.HeadEmail) ? tsp.HeadEmail.Substring(0, tsp.HeadEmail.Length > 100 ? 100 : tsp.HeadEmail.Length) : string.Empty;

                    model.Cellular = !string.IsNullOrEmpty(tsp.HeadLandline) ? tsp.HeadLandline.Substring(0, tsp.HeadLandline.Length > 20 ? 20 : tsp.HeadLandline.Length) : string.Empty;

                    model.U_Account = !string.IsNullOrEmpty(tsp.BankAccountNumber) ? tsp.BankAccountNumber.Substring(0, tsp.BankAccountNumber.Length > 50 ? 50 : tsp.BankAccountNumber.Length) : string.Empty;

                    model.U_AT = !string.IsNullOrEmpty(tsp.AccountTitle) ? tsp.AccountTitle.Substring(0, tsp.AccountTitle.Length > 254 ? 254 : tsp.AccountTitle.Length) : string.Empty;
                    //model.BankName = !string.IsNullOrEmpty(tsp.BankName) ? tsp.BankName.Substring(0, tsp.BankName.Length > 254 ? 254 : tsp.BankName.Length) : string.Empty;
                    //model.BankBranch = !string.IsNullOrEmpty(tsp.BankBranch) ? tsp.BankBranch.Substring(0, tsp.BankBranch.Length > 254 ? 254 : tsp.BankBranch.Length) : string.Empty;
                    model.AddressList = new List<dynamic>()
                    {
                        new {
                            Address=!string.IsNullOrEmpty(tsp.Address)?tsp.Address.Substring(0,tsp.Address.Length>50?50:tsp.Address.Length):string.Empty,
                            ADDRESS2=string.Empty,
                            ADRESTYPE=1
                        }
                    };
                    model.ContactPersons = new List<dynamic>()
                    {
                        new {
                            Name=!string.IsNullOrEmpty(tsp.HeadName)?tsp.HeadName.Substring(0,tsp.HeadName.Length>50?50:tsp.HeadName.Length):string.Empty,
                            //FirstName=tsp.HeadName?? string.Empty,
                            FirstName=!string.IsNullOrEmpty(tsp.HeadName)?tsp.HeadName.Substring(0,tsp.HeadName.Length>50?50:tsp.HeadName.Length):string.Empty,
                            Title=!string.IsNullOrEmpty(tsp.HeadDesignation)?tsp.HeadDesignation.Substring(0,tsp.HeadDesignation.Length>10?10:tsp.HeadDesignation.Length):string.Empty,
                            Cellolar=!string.IsNullOrEmpty(tsp.HeadLandline)?tsp.HeadLandline.Substring(0,tsp.HeadLandline.Length>50?50:tsp.HeadLandline.Length):string.Empty,
                            E_MailL=!string.IsNullOrEmpty(tsp.HeadEmail)?tsp.HeadEmail.Substring(0,tsp.HeadEmail.Length>100?100:tsp.HeadEmail.Length):string.Empty,
                        },
                         new {
                            Name=!string.IsNullOrEmpty(tsp.CPName)?tsp.CPName.Substring(0,tsp.CPName.Length>50?50:tsp.CPName.Length):string.Empty,
                            FirstName=!string.IsNullOrEmpty(tsp.CPName)?tsp.CPName.Substring(0,tsp.CPName.Length>50?50:tsp.CPName.Length):string.Empty,
                            Title=!string.IsNullOrEmpty(tsp.CPDesignation)?tsp.CPDesignation.Substring(0,tsp.CPDesignation.Length>10?10:tsp.CPDesignation.Length):string.Empty,
                            Cellolar=!string.IsNullOrEmpty(tsp.CPLandline)?tsp.CPLandline.Substring(0,tsp.CPLandline.Length>50?50:tsp.CPLandline.Length):string.Empty,
                            E_MailL=!string.IsNullOrEmpty(tsp.CPEmail)?tsp.CPEmail.Substring(0,tsp.CPEmail.Length>100?100:tsp.CPEmail.Length):string.Empty,
                        }
                    };

                    var myJson = JsonConvert.SerializeObject(model);
                    using (var httpClient = new HttpClient())
                    {
                        //var httpClient = new HttpClient();
                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                        httpClient.Timeout = TimeSpan.FromMilliseconds(requestTimeoutInMiliSeconds);
                        var request = await httpClient.PostAsync($@"{sapHost}/api/BusinessPartner", new StringContent(myJson, Encoding.UTF8, "application/json"));
                        string resultContent = await request.Content.ReadAsStringAsync();

                        SAPResponseModel response = JsonConvert.DeserializeObject<SAPResponseModel>(resultContent);
                        //response.StatusBit = response.Status == "0" ? false : response.Status == "1" ? true : false;
                        if (string.IsNullOrEmpty(response?.SapObjId))
                        {
                            throw new Exception($"SAP : {response?.Message ?? "Invalid response."} {", TSP Name: " + tsp.TSPName }");
                        }
                        return response;
                    }
                }
                else
                {
                    throw new Exception("SAP User Login Failed");
                }
            }
            catch (Exception e)
            {
                throw new Exception($"SAP Service : {e.Message}", e);
            }
        }
    }
}
