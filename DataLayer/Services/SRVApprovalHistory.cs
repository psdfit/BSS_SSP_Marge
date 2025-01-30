/* **** Aamer Rehman Malik *****/

using DataLayer.Dapper;
using DataLayer.Interfaces;
using DataLayer.Models;
using DataLayer.Models.SSP;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DataLayer.Services
{
    public class SRVApprovalHistory : SRVBase, ISRVApprovalHistory
    {
        private readonly ISRVScheme srvScheme;
        private readonly ISRVProgramDesign srvProgramDesign;
        private readonly ISRVCriteriaTemplate srvCriteria;
        private readonly ISRVPurchaseOrder srvPurchaseOrder;
        private readonly ISRVGenerateInvoice generateInvoice;
        private readonly ISRVSRN srvSRN;
        private readonly ISRVGURN srvGURN;
        private readonly ISRVTrade srvTrade;
        private readonly ISRVPRN srvPRN;
        private readonly ISRVSAPApi srvSAPApi;
        private readonly ISRVTrn srvTRN;
        private readonly ISRVPRNMaster srvPRNMaster;
        private readonly ISRVApproval srvApproval;
        private readonly ISRVInvoiceMaster srvInvoiceMaster;
        private readonly ISRVInvoice srvInvoice;
        private readonly ISRVUsers srvUsers;
        private readonly ISRVPOHeader srvPOHeader;
        private readonly ISRVPOLines srvPOLines;
        private readonly ISRVSendEmail srvSendEmail;
        private readonly ISRVSchemeChangeRequest srvCrScheme;
        private readonly ISRVTSPChangeRequest srvCrTSP;
        private readonly ISRVClassChangeRequest srvCrClass;
        private readonly ISRVTraineeChangeRequest srvCrTrainee;
        private readonly ISRVInstructorChangeRequest srvCrInstructor;
        private readonly ISRVInceptionReportChangeRequest srvCrInceptionReport;
        private readonly ISRVInstructorReplaceChangeRequest srvCrInstructorReplace;
        //private readonly ISRVSchemeChangeRequest srvCrScheme;
        //private readonly ISRVSchemeChangeRequest srvCrScheme;
        //private readonly ISRVSchemeChangeRequest srvCrScheme;
        private readonly ISRVClassInvoiceExtMap srvcancel;
        private readonly IDapperConfig _dapper;
        private readonly ISRVTSPMaster _srvTSPMaster;
        private readonly ISRVApprovalProcess _srvApprovalProcess;
        private readonly ISRVTSPDetail srvTSPDetail;
        //private readonly ISRVClassInvoiceMap srvInvMap;
        public SRVApprovalHistory(ISRVTSPDetail srv, ISRVProgramDesign srvProgramDesign, ISRVCriteriaTemplate srvCriteria, ISRVApprovalProcess srvApprovalProcess, ISRVTSPMaster srvTSPMaster, ISRVScheme srvScheme, ISRVPurchaseOrder srvPurchaseOrder, ISRVGenerateInvoice generateInvoice,
            ISRVSRN srvSRN, ISRVGURN srvGURN, ISRVTrade srvTrade, ISRVPRN srvPRN, ISRVSAPApi srvSAPApi, ISRVTrn srvTRN, ISRVPRNMaster srvPRNMaster,
            ISRVApproval srvApproval, ISRVInvoiceMaster srvInvoiceMaster, ISRVInvoice srvInvoice, ISRVUsers srvUsers,
            ISRVPOHeader srvPOHeader, ISRVPOLines srvPOLines, ISRVSendEmail srvSendEmail, ISRVClassInvoiceExtMap srvcancel, ISRVClassInvoiceMap srvInvMap,
            ISRVSchemeChangeRequest srvCrScheme, ISRVTSPChangeRequest srvCrTSP, ISRVClassChangeRequest srvCrClass, ISRVTraineeChangeRequest srvCrTrainee,
            ISRVInstructorChangeRequest srvCrInstructor, ISRVInceptionReportChangeRequest srvCrInceptionReport, ISRVInstructorReplaceChangeRequest srvCrInstructorReplace, IDapperConfig dapper)
        {
            this.srvTSPDetail = srv;
            this.srvProgramDesign = srvProgramDesign;
            this.srvCriteria = srvCriteria;
            this._srvApprovalProcess = srvApprovalProcess;
            this._srvTSPMaster = srvTSPMaster;
            this.srvScheme = srvScheme;
            this.srvPurchaseOrder = srvPurchaseOrder;
            this.generateInvoice = generateInvoice;
            this.srvSRN = srvSRN;
            this.srvGURN = srvGURN;
            this.srvTrade = srvTrade;
            this.srvPRN = srvPRN;
            this.srvSAPApi = srvSAPApi;
            this.srvInvoiceMaster = srvInvoiceMaster;
            this.srvPRNMaster = srvPRNMaster;
            this.srvApproval = srvApproval;
            this.srvInvoice = srvInvoice;
            this.srvUsers = srvUsers;
            this.srvPOHeader = srvPOHeader;
            this.srvPOLines = srvPOLines;
            this.srvSendEmail = srvSendEmail;
            this.srvCrScheme = srvCrScheme;
            this.srvCrTSP = srvCrTSP;
            this.srvCrClass = srvCrClass;
            this.srvCrTrainee = srvCrTrainee;
            this.srvCrInstructor = srvCrInstructor;
            this.srvCrInceptionReport = srvCrInceptionReport;
            this.srvCrInstructorReplace = srvCrInstructorReplace;
            this.srvcancel = srvcancel;
            _dapper = dapper;

            //this.srvInvMap = srvInvMap;
        }

        private List<ApprovalHistoryModel> LoopinData(DataTable dt)
        {
            List<ApprovalHistoryModel> ApprovalL = new List<ApprovalHistoryModel>();

            foreach (DataRow r in dt.Rows)
            {
                ApprovalL.Add(RowOfApprovalHistory(r));
            }
            return ApprovalL;
        }

        public List<ApprovalHistoryModel> FetchApprovalHistory(ApprovalHistoryModel model, SqlTransaction transaction = null)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@ApprovalHistoryID", model.ApprovalHistoryID));
                param.Add(new SqlParameter("@ProcessKey", model.ProcessKey));
                param.Add(new SqlParameter("@FormID", model.FormID));

                DataTable dt = new DataTable();
                if (transaction != null)
                {
                    dt = SqlHelper.ExecuteDataset(transaction, CommandType.StoredProcedure, "RD_ApprovalHistory", param.ToArray()).Tables[0];
                }
                else
                {
                    dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_ApprovalHistory", param.ToArray()).Tables[0];
                }
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        private ApprovalHistoryModel RowOfApprovalHistory(DataRow row)
        {
            ApprovalHistoryModel approvalhistory = new ApprovalHistoryModel();

            approvalhistory.ApprovalHistoryID = row.Field<int>("ApprovalHistoryID");
            approvalhistory.Step = row.Field<int>("Step");
            approvalhistory.FormID = row.Field<int>("FormID");
            approvalhistory.ApproverID = row.Field<int?>("ApproverID");
            approvalhistory.ApprovalStatusID = row.Field<int>("ApprovalStatusID");
            approvalhistory.Comments = row.Field<string>("Comments");
            approvalhistory.CreatedUserID = row.Field<int?>("CreatedUserID") ?? 0;
            approvalhistory.ModifiedUserID = row.Field<int?>("ModifiedUserID") ?? 0;
            approvalhistory.CreatedDate = row.Field<DateTime?>("CreatedDate");
            approvalhistory.ModifiedDate = row.Field<DateTime?>("ModifiedDate");
            approvalhistory.InActive = row.Field<bool?>("InActive");
            approvalhistory.ApproverName = row.Field<string>("ApproverName");
            approvalhistory.ProcessKey = row.Field<string>("ProcessKey");
            approvalhistory.ApproverIDs = row.Field<string>("ApproverIDs");
            approvalhistory.ApproverNames = row.Field<string>("ApproverNames");
            approvalhistory.StatusDisplayName = row.Field<string>("StatusDisplayName");
            approvalhistory.IsFinalStep = row.Field<bool>("IsFinalStep");
            if (row.Table.Columns.Contains("IsAutoApproval"))
                approvalhistory.IsAutoApproval = row.Field<bool>("IsAutoApproval");
            else
                approvalhistory.IsAutoApproval = false;
            return approvalhistory;
        }

        public bool SaveSRNApprovalHistory(ref ApprovalWrapperModel wrapperModel)
        {

            var model = wrapperModel.approvalHistoryModel;
            using (SqlConnection connection = new SqlConnection(SqlHelper.GetCon()))
            {
                connection.Open();
                bool result = false;
                var _transaction = connection.BeginTransaction();
                try
                {
                    List<ApprovalModel> approvals = srvApproval.FetchApproval(new ApprovalModel() { ProcessKey = model.ProcessKey }, _transaction);
                    ApprovalModel currentApproval = approvals.Where(x => x.Step == model.Step).FirstOrDefault();
                    bool isFinalApprover = currentApproval.Step == approvals.Count;

                    wrapperModel.approvals = approvals;
                    wrapperModel.currentApproval = currentApproval;
                    wrapperModel.isFinalApprover = isFinalApprover;

                    if (currentApproval.UserIDs.Split(',').Contains(model.CurUserID.ToString()) && model.FormIDs.Count() > 0)
                    {
                        foreach (var id in model.FormIDs)
                        {
                            var current = FetchApprovalHistory(new ApprovalHistoryModel()
                            {
                                FormID = id,
                                ProcessKey = model.ProcessKey,
                                ApprovalStatusID = (int)EnumApprovalStatus.Pending
                            }, _transaction).OrderByDescending(x => x.ApprovalHistoryID).FirstOrDefault();
                            current.ApprovalStatusID = model.ApprovalStatusID;
                            current.ApproverID = model.ApproverID;
                            current.Comments = model.Comments;
                            current.CurUserID = model.CurUserID;
                            //model.FormID = id;
                            if (AU_ApprovalHistory(current, _transaction))
                            {
                                ApprovalHistoryModel next = new ApprovalHistoryModel()
                                {
                                    ApprovalHistoryID = 0,
                                    ProcessKey = current.ProcessKey,
                                    FormID = current.FormID,
                                    ApprovalStatusID = (int)EnumApprovalStatus.Pending,
                                    Comments = "Pending",
                                    ApproverID = null,
                                    CurUserID = current.CurUserID,
                                    InActive = false
                                };
                                switch (current.ApprovalStatusID)
                                {
                                    case (int)EnumApprovalStatus.Approved:
                                        if (!isFinalApprover)
                                        {
                                            next.Step = currentApproval.Step + 1;
                                            AU_ApprovalHistory(next, _transaction);
                                        }
                                        else
                                        {
                                            srvSRN.SRNApproveReject(new SRNModel()
                                            {
                                                SRNID = next.FormID,
                                                IsApproved = true,
                                                IsRejected = false,
                                                CurUserID = next.CurUserID
                                            }, _transaction);
                                        }
                                        break;

                                    case (int)EnumApprovalStatus.SendBack:
                                        next.Step = currentApproval.Step - 1;
                                        AU_ApprovalHistory(next, _transaction);
                                        break;

                                    case (int)EnumApprovalStatus.Rejected:
                                        srvSRN.SRNApproveReject(new SRNModel()
                                        {
                                            SRNID = next.FormID,
                                            IsApproved = false,
                                            IsRejected = true,
                                            CurUserID = next.CurUserID
                                        }, _transaction);
                                        break;
                                }
                            }
                        }

                        if (isFinalApprover)
                        {
                            var ProcessKey = model.ProcessKey;
                            if (ProcessKey == "VRN") {
                                srvPurchaseOrder.CreatePOForSRN(string.Join(',', model.FormIDs), EnumApprovalProcess.PO_VRN, model.CurUserID, _transaction);
                            }
                            else {
                                srvPurchaseOrder.CreatePOForSRN(string.Join(',', model.FormIDs), EnumApprovalProcess.PO_SRN, model.CurUserID, _transaction);
                            }
                         }
                        _transaction.Commit();
                        result = true;
                    }
                }
                catch (Exception)
                {
                    _transaction.Rollback();
                    throw;
                }
                return result;
            }
        }
        public bool SaveGURNApprovalHistory(ref ApprovalWrapperModel wrapperModel)
        {
            var model = wrapperModel.approvalHistoryModel;
            using (SqlConnection connection = new SqlConnection(SqlHelper.GetCon()))
            {
                connection.Open();
                bool result = false;
                var _transaction = connection.BeginTransaction();
                try
                {
                    List<ApprovalModel> approvals = srvApproval.FetchApproval(new ApprovalModel() { ProcessKey = model.ProcessKey }, _transaction);
                    ApprovalModel currentApproval = approvals.Where(x => x.Step == model.Step).FirstOrDefault();
                    bool isFinalApprover = currentApproval.Step == approvals.Count;

                    wrapperModel.approvals = approvals;
                    wrapperModel.currentApproval = currentApproval;
                    wrapperModel.isFinalApprover = isFinalApprover;

                    if (currentApproval.UserIDs.Split(',').Contains(model.CurUserID.ToString()) && model.FormIDs.Count() > 0)
                    {
                        foreach (var id in model.FormIDs)
                        {
                            var current = FetchApprovalHistory(new ApprovalHistoryModel()
                            {
                                FormID = id,
                                ProcessKey = model.ProcessKey,
                                ApprovalStatusID = (int)EnumApprovalStatus.Pending
                            }, _transaction).OrderByDescending(x => x.ApprovalHistoryID).FirstOrDefault();
                            current.ApprovalStatusID = model.ApprovalStatusID;
                            current.ApproverID = model.ApproverID;
                            current.Comments = model.Comments;
                            current.CurUserID = model.CurUserID;
                            //model.FormID = id;
                            if (AU_ApprovalHistory(current, _transaction))
                            {
                                ApprovalHistoryModel next = new ApprovalHistoryModel()
                                {
                                    ApprovalHistoryID = 0,
                                    ProcessKey = current.ProcessKey,
                                    FormID = current.FormID,
                                    ApprovalStatusID = (int)EnumApprovalStatus.Pending,
                                    Comments = "Pending",
                                    ApproverID = null,
                                    CurUserID = current.CurUserID,
                                    InActive = false
                                };
                                switch (current.ApprovalStatusID)
                                {
                                    case (int)EnumApprovalStatus.Approved:
                                        if (!isFinalApprover)
                                        {
                                            next.Step = currentApproval.Step + 1;
                                            AU_ApprovalHistory(next, _transaction);
                                        }
                                        else
                                        {
                                            srvGURN.GURNApproveReject(new GURNModel()
                                            {
                                                GURNID = next.FormID,
                                                IsApproved = true,
                                                IsRejected = false,
                                                CurUserID = next.CurUserID
                                            }, _transaction);
                                        }
                                        break;

                                    case (int)EnumApprovalStatus.SendBack:
                                        next.Step = currentApproval.Step - 1;
                                        AU_ApprovalHistory(next, _transaction);
                                        break;

                                    case (int)EnumApprovalStatus.Rejected:
                                        srvGURN.GURNApproveReject(new GURNModel()
                                        {
                                            GURNID = next.FormID,
                                            IsApproved = false,
                                            IsRejected = true,
                                            CurUserID = next.CurUserID
                                        }, _transaction);
                                        break;
                                }
                            }
                        }

                        if (isFinalApprover)
                        {
                            var ProcessKey = model.ProcessKey;
                            if (ProcessKey == "GURN") {
                                srvPurchaseOrder.CreatePOForGURN(string.Join(',', model.FormIDs), EnumApprovalProcess.PO_GURN, model.CurUserID, _transaction);
                            }
                            else {
                                srvPurchaseOrder.CreatePOForGURN(string.Join(',', model.FormIDs), EnumApprovalProcess.PO_GURN, model.CurUserID, _transaction);
                            }
                         }
                        _transaction.Commit();
                        result = true;
                    }
                }
                catch (Exception)
                {
                    _transaction.Rollback();
                    throw;
                }
                return result;
            }
        }

        public void SendSRNApprovalNotification(ApprovalWrapperModel wrapperModel)
        {
            var model = wrapperModel.approvalHistoryModel;
            var approvals = wrapperModel.approvals;
            var currentApproval = wrapperModel.currentApproval;
            var isFinalApprover = wrapperModel.isFinalApprover;
            try
            {
                ApprovalModel approvalsModelForNotification = new ApprovalModel();
                if (currentApproval.UserIDs.Split(',').Contains(model.CurUserID.ToString()) && model.FormIDs.Count() > 0)
                {
                    if (!isFinalApprover)
                    {
                        switch (model.ApprovalStatusID)
                        {
                            case (int)EnumApprovalStatus.Approved:
                                var nextApproval = approvals.Where(x => x.Step == currentApproval.Step + 1).FirstOrDefault();
                                // srvSendEmail.GenerateEmailToApprovers(nextApproval, model);
                                approvalsModelForNotification.UserIDs = nextApproval.UserIDs;
                                approvalsModelForNotification.ProcessKey = model.ProcessKey;
                                srvSendEmail.GenerateEmailAndSendNotification(approvalsModelForNotification, model);

                                break;

                            case (int)EnumApprovalStatus.SendBack:
                                var previousApproval = approvals.Where(x => x.Step == currentApproval.Step - 1).FirstOrDefault();
                                //srvSendEmail.GenerateEmailToApprovers(previousApproval, model);
                                approvalsModelForNotification.UserIDs = previousApproval.UserIDs;
                                approvalsModelForNotification.ProcessKey = model.ProcessKey;
                                srvSendEmail.GenerateEmailAndSendNotification(approvalsModelForNotification, model);
                                break;

                            default:
                                break;
                        }
                    }
                    else
                    {
                        var firstApproval = srvApproval.FetchApproval(new ApprovalModel() { ProcessKey = EnumApprovalProcess.PO_SRN, Step = 1 }).FirstOrDefault();
                        srvSendEmail.GenerateEmailToApprovers(firstApproval, new ApprovalHistoryModel() { ApprovalStatusID = (int)EnumApprovalStatus.Pending });

                        // Notification send to TSP and KAM
                        var FormIDs = string.Join(",", model.FormIDs);
                        string KAMAndTspUserByFormIds = this._srvTSPMaster.GET_KAMAndTspUserBySRNIDs_Notification(FormIDs);
                        ApprovalHistoryModel ConcateClassescodebyFormIds = this._srvTSPMaster.GET_ConcateClassescodebySRNID_Notification(FormIDs);
                        approvalsModelForNotification.UserIDs = KAMAndTspUserByFormIds;
                        approvalsModelForNotification.ProcessKey = model.ProcessKey;
                        model.ApprovalStatusID = (int)EnumApprovalStatus.Pending;
                        approvalsModelForNotification.CustomComments = "Approved";
                        approvalsModelForNotification.isUserMapping = true;
                        srvSendEmail.GenerateEmailAndSendNotification(approvalsModelForNotification, model);

                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool SaveApprovalHistory(ref ApprovalWrapperModel wrapperModel)
        {
            var model = wrapperModel.approvalHistoryModel;
            List<ApprovalModel> approvals = srvApproval.FetchApproval(new ApprovalModel() { ProcessKey = model.ProcessKey });
            ApprovalModel currentApproval = approvals.Where(x => x.Step == model.Step).FirstOrDefault();
            bool isFinalApprover = currentApproval.Step == approvals.Count;
            bool result = false;

            wrapperModel.approvals = approvals;
            wrapperModel.currentApproval = currentApproval;
            wrapperModel.isFinalApprover = isFinalApprover;

            using (SqlConnection connection = new SqlConnection(SqlHelper.GetCon()))
            {
                connection.Open();
                var _transaction = connection.BeginTransaction();
                try
                {
                    if (currentApproval.UserIDs.Split(',').Contains(model.CurUserID.ToString()))
                    {
                        ///Update Current History Record
                        ///Add new History Record with latest ApprovalID & PendingStatus
                        if (AU_ApprovalHistory(model, _transaction)) // this updates the comments etc
                        {
                            ApprovalHistoryModel historyModel = new ApprovalHistoryModel()
                            {
                                ApprovalHistoryID = 0,
                                ProcessKey = model.ProcessKey,
                                FormID = model.FormID,
                                ApprovalStatusID = (int)EnumApprovalStatus.Pending,
                                Comments = "Pending",
                                ApproverID = null,
                                CurUserID = model.CurUserID,
                                InActive = false
                            };
                            switch (model.ApprovalStatusID)
                            {
                                case (int)EnumApprovalStatus.Approved:
                                    ///add next Approval History Record with pending status
                                    if (!isFinalApprover)
                                    {
                                        historyModel.Step = currentApproval.Step + 1;
                                        var nextApproval = approvals.Where(x => x.Step == currentApproval.Step + 1).FirstOrDefault();
                                        AU_ApprovalHistory(historyModel, _transaction); // this enters next approver
                                        _transaction.Commit();
                                        result = true;
                                    }
                                    else
                                    {
                                        switch (model.ProcessKey)
                                        {
                                            case EnumApprovalProcess.AP_PD:
                                            case EnumApprovalProcess.AP_BD:
                                                srvScheme.SchemeApproveReject(new SchemeModel()
                                                {
                                                    SchemeID = model.FormID,
                                                    IsApproved = true,
                                                    IsRejected = false,
                                                    CurUserID = model.CurUserID
                                                }, _transaction);

                                                generateInvoice.GenerateInvoices(model.FormID, _transaction);
                                                List<EmailUsersModel> users = generateInvoice.CreateTSPsAccounts(model.FormID, model.CurUserID, _transaction);

                                                //Fetch scheme details and enter scheme in SAP
                                                var scheme = srvScheme.GetBySchemeID(model.FormID, _transaction);
                                                if (string.IsNullOrEmpty(scheme?.SAPID))
                                                {
                                                    var sapResponse = srvSAPApi.SaveSAPCostCenterForScheme(scheme, _transaction).Result;
                                                    if (sapResponse.Status == "1")
                                                    {
                                                        srvScheme.UpdateSchemeSAPID(model.FormID, sapResponse.SapObjId, _transaction);
                                                    }
                                                }
                                                //Enter TSP in SAP
                                                srvPurchaseOrder.CreatePO(model.FormID, EnumApprovalProcess.PO_TSP, model.CurUserID, _transaction);
                                                _transaction.Commit();
                                                generateInvoice.GenerateEmailsToUsers(users);
                                                //if (scheme.ProgramTypeID == 7)
                                                //{
                                                //    generateInvoice.GenerateEmailsToUsersB2C(users);
                                                //}
                                                result = true;
                                                break;

                                            case EnumApprovalProcess.TRD:
                                                srvTrade.TradeApproveReject(new TradeModel()
                                                {
                                                    TradeID = model.FormID,
                                                    IsApproved = true,
                                                    IsRejected = false,
                                                    CurUserID = model.CurUserID
                                                }, _transaction);

                                                //Fetch Trade details and enter Trade in SAP
                                                var tradeDetails = srvTrade.GetTradeCertificationType(model.FormID, _transaction);
                                                if (string.IsNullOrEmpty(tradeDetails?.SAPID))
                                                {
                                                    var sapTRDResponse = srvSAPApi.SaveSAPCostCenterForTrade(tradeDetails, _transaction).Result;
                                                    if (sapTRDResponse.Status == "1")
                                                    {
                                                        srvTrade.UpdateTradeSAPID(model.FormID, sapTRDResponse.SapObjId, _transaction);
                                                    }
                                                }
                                                _transaction.Commit();
                                                result = true;
                                                break;



                                            case EnumApprovalProcess.PROG_APP:
                                                srvProgramDesign.ProgramApproveReject(new ProgramDesignModel()
                                                {
                                                    ProgramID = model.FormID,
                                                    IsApproved = true,
                                                    IsRejected = false,
                                                    CurUserID = model.CurUserID
                                                }, _transaction);

                                                srvProgramDesign.ProgramDesignFinalApproval(model.FormID, _transaction);
                                                _transaction.Commit();
                                                result = true;
                                                break;

                                            case EnumApprovalProcess.CRTEM_APP:
                                                srvCriteria.CriteriaApproveReject(new CriteriaTemplateModel()
                                                {
                                                    CriteriaTemplateID = model.FormID,
                                                    IsApproved = true,
                                                    IsRejected = false,
                                                    CurUserID = model.CurUserID
                                                }, _transaction);

                                                srvCriteria.CriteriaFinalApproval(model.FormID, _transaction);
                                                _transaction.Commit();
                                                result = true;
                                                break;


                                            case EnumApprovalProcess.CR_SCHEME:
                                                srvCrScheme.CrSchemeApproveReject(new SchemeChangeRequestModel()
                                                {
                                                    SchemeChangeRequestID = model.FormID,
                                                    IsApproved = true,
                                                    IsRejected = false,
                                                    CurUserID = model.CurUserID
                                                }, _transaction);
                                                _transaction.Commit();
                                                result = true;
                                                break;




                                            case EnumApprovalProcess.CR_TSP:

                                                var tspChangeRequest = srvCrTSP.GetByTSPChangeRequestID(model.FormID, _transaction);
                                                //if (string.IsNullOrEmpty(tspChangeRequest?.SAPID))
                                                //{
                                                //Fetch TSP Change Request details and update TSP in SAP
                                                tspChangeRequest.AddUpdate = string.IsNullOrEmpty(tspChangeRequest?.SAPID) ? "1" : "2";
                                                var sapTspResponse = srvSAPApi.SaveSAPBusinessPartnerForTSPUpdate(tspChangeRequest, _transaction).Result;
                                                if (sapTspResponse.Status == "1")
                                                {
                                                    srvCrTSP.CrTSPApproveReject(new TSPChangeRequestModel()
                                                    {
                                                        TSPChangeRequestID = model.FormID,
                                                        IsApproved = true,
                                                        IsRejected = false,
                                                        CurUserID = model.CurUserID
                                                    }, _transaction);
                                                }
                                                //}
                                                _transaction.Commit();
                                                result = true;
                                                break;

                                            case EnumApprovalProcess.CR_CLASS_LOCATION:
                                                srvCrClass.CrClassApproveReject(new ClassChangeRequestModel()
                                                {
                                                    ClassChangeRequestID = model.FormID,
                                                    IsApproved = true,
                                                    IsRejected = false,
                                                    CurUserID = model.CurUserID
                                                }, _transaction);
                                                _transaction.Commit();
                                                result = true;
                                                break;
                                            case EnumApprovalProcess.CR_CLASS_DATES:
                                                srvCrClass.CrClassDatesApproveReject(new ClassChangeRequestModel()
                                                {
                                                    ClassChangeRequestID = model.FormID,
                                                    IsApproved = true,
                                                    IsRejected = false,
                                                    CurUserID = model.CurUserID
                                                }, _transaction);
                                                _transaction.Commit();
                                                result = true;
                                                break;
                                            case EnumApprovalProcess.CR_TRAINEE_UNVERIFIED:
                                                srvCrTrainee.CrTraineeApproveReject(new TraineeChangeRequestModel()
                                                {
                                                    TraineeChangeRequestID = model.FormID,
                                                    IsApproved = true,
                                                    IsRejected = false,
                                                    CurUserID = model.CurUserID
                                                }, _transaction);
                                                _transaction.Commit();
                                                result = true;
                                                break;

                                            case EnumApprovalProcess.CR_TRAINEE_VERIFIED:
                                                srvCrTrainee.CrTraineeApproveReject(new TraineeChangeRequestModel()
                                                {
                                                    TraineeChangeRequestID = model.FormID,
                                                    IsApproved = true,
                                                    IsRejected = false,
                                                    CurUserID = model.CurUserID
                                                }, _transaction);
                                                _transaction.Commit();
                                                result = true;
                                                break;

                                            case EnumApprovalProcess.CR_INSTRUCTOR:
                                                InstructorChangeRequestModel InstructorChangeRequest = srvCrInstructor.GetByInstructorChangeRequestID_Notification(model.FormID, _transaction);
                                                srvCrInstructor.CrInstructorApproveReject(new InstructorChangeRequestModel()
                                                {
                                                    InstructorChangeRequestID = model.FormID,
                                                    IsApproved = true,
                                                    IsRejected = false,
                                                    CurUserID = model.CurUserID
                                                }, _transaction);
                                                _transaction.Commit();
                                                result = true;
                                                break;

                                            case EnumApprovalProcess.CR_NEW_INSTRUCTOR:
                                                srvCrInstructor.CrNewInstructorApproveReject(new InstructorChangeRequestModel()
                                                {
                                                    CRNewInstructorID = model.FormID,
                                                    IsApproved = true,
                                                    IsRejected = false,
                                                    CurUserID = model.CurUserID
                                                }, _transaction);
                                                InstructorChangeRequestModel NewInstructorChangeRequest = srvCrInstructor.GetByNewInstructorChangeRequestID_Notification(model.FormID, _transaction);
                                                _transaction.Commit();
                                                result = true;
                                                break;

                                            case EnumApprovalProcess.CR_INCEPTION:
                                                srvCrInceptionReport.CrInceptionReportApproveReject(new InceptionReportChangeRequestModel()
                                                {
                                                    InceptionReportChangeRequestID = model.FormID,
                                                    IsApproved = true,
                                                    IsRejected = false,
                                                    CurUserID = model.CurUserID
                                                }, _transaction);
                                                _transaction.Commit();
                                                result = true;
                                                break;

                                            case EnumApprovalProcess.CR_INSTRUCTOR_REPLACE:
                                                InstructorReplaceChangeRequestModel InstructorReplaceChangeRequest = srvCrInstructorReplace.GetByInstructorReplaceChangeRequestID(model.FormID, _transaction);
                                                srvCrInstructorReplace.CrInstructorReplaceApproveReject(new InstructorReplaceChangeRequestModel()
                                                {
                                                    InstructorReplaceChangeRequestID = model.FormID,
                                                    IsApproved = true,
                                                    IsRejected = false,
                                                    CurUserID = model.CurUserID
                                                }, _transaction);
                                                _transaction.Commit();
                                                result = true;
                                                break;

                                            case EnumApprovalProcess.PRN_R:
                                                List<PRNMasterModel> PRNMasterModel = srvPRNMaster.GetPRNMasterForApproval(model.FormID, _transaction);
                                                ApprovalProcessModel ApprovalProcessModelR = _srvApprovalProcess.GetByProcessKey(EnumApprovalProcess.INV_R);
                                                srvPRNMaster.PRNMasterApproveReject(new PRNMasterModel()
                                                {
                                                    PRNMasterID = model.FormID,
                                                    IsApproved = true,
                                                    IsRejected = false
                                                }, _transaction);
                                                srvInvoiceMaster.GenerateInvoiceHeader(model.FormID, _transaction, EnumApprovalProcess.INV_R);
                                                _transaction.Commit();
                                                result = true;
                                                break;

                                            case EnumApprovalProcess.PRN_C:
                                                List<PRNMasterModel> PRNMasterModelc = srvPRNMaster.GetPRNMasterForApproval(model.FormID, _transaction);
                                                ApprovalProcessModel ApprovalProcessModelC = _srvApprovalProcess.GetByProcessKey(EnumApprovalProcess.INV_C);
                                                srvPRNMaster.PRNMasterApproveReject(new PRNMasterModel()
                                                {
                                                    PRNMasterID = model.FormID,
                                                    IsApproved = true,
                                                    IsRejected = false
                                                }, _transaction);

                                                srvInvoiceMaster.GenerateINVCompletion(model.FormID, EnumApprovalProcess.INV_C, _transaction);
                                                _transaction.Commit();
                                                result = true;
                                                break;

                                            case EnumApprovalProcess.PRN_F:
                                                List<PRNMasterModel> PRNMasterModelF = srvPRNMaster.GetPRNMasterForApproval(model.FormID, _transaction);
                                                ApprovalProcessModel ApprovalProcessModelF = _srvApprovalProcess.GetByProcessKey(EnumApprovalProcess.INV_F);
                                                srvPRNMaster.PRNMasterApproveReject(new PRNMasterModel()
                                                {
                                                    PRNMasterID = model.FormID,
                                                    IsApproved = true,
                                                    IsRejected = false
                                                }, _transaction);

                                                srvInvoiceMaster.GenerateINVEmployment(model.FormID, EnumApprovalProcess.INV_F, _transaction);
                                                _transaction.Commit();
                                                result = true;
                                                break;

                                            case EnumApprovalProcess.PRN_T:
                                                List<PRNMasterModel> PRNMasterModelT = srvPRNMaster.GetPRNMasterForApproval(model.FormID, _transaction);
                                                srvSRN.TRNApproveReject(new TRNMasterModel()
                                                {
                                                    TRNMasterID = model.FormID,
                                                    IsApproved = true,
                                                    IsRejected = false,
                                                    ModifiedUserID = model.CurUserID
                                                }, _transaction);

                                                srvPurchaseOrder.CreatePOForTRN(model.FormID, model.CurUserID, _transaction);
                                                _transaction.Commit();
                                                result = true;
                                                break;

                                            case EnumApprovalProcess.PO_TRN:
                                                srvSRN.PO_TRNApproveReject(new POHeaderModel()
                                                {
                                                    POHeaderID = model.FormID,
                                                    IsApproved = true,
                                                    IsRejected = false,
                                                    ModifiedUserID = model.CurUserID
                                                }, _transaction);
                                                //Enter Purchase Order in SAP
                                                var md = new POHeaderModel { POHeaderID = model.FormID, ProcessKey = "", Month = null };
                                                var poHeader = srvPOHeader.FetchPOHeader(md, _transaction).First();
                                                var poLines = srvPOLines.GetPOLinesByPOHeaderID(model.FormID, _transaction);
                                                if (string.IsNullOrEmpty(poHeader?.DocNum) || poHeader?.DocNum == "0")
                                                {
                                                    var sapPO_TSPResponse = srvSAPApi.SaveSAPPurchaseOrder(poHeader, poLines, _transaction).Result;
                                                    if (sapPO_TSPResponse.Status == "1"
                                                       //|| (sapPO_TSPResponse.Status == "0" && sapPO_TSPResponse.POModel.DocEntry > 0 && !string.IsNullOrEmpty(sapPO_TSPResponse.POModel.DocNum))
                                                       )
                                                    {
                                                        new SRVPOHeader().UpdateSAPInPOHeader(model.FormID, sapPO_TSPResponse.POModel.DocEntry, sapPO_TSPResponse.POModel.DocNum, _transaction);
                                                        //new SRVPOLines().UpdateSAPInPOLines(model.FormID, sapPO_TSPResponse.POModel.PODetail, poLines, _transaction);
                                                    }
                                                }
                                                srvInvoiceMaster.GenerateInvoiceHeader_TRN(model.FormID, _transaction, EnumApprovalProcess.INV_TRN);
                                                _transaction.Commit();
                                                result = true;
                                                break;

                                            case EnumApprovalProcess.PO_SRN:
                                                srvPurchaseOrder.POHeaderApproveReject(new POHeaderModel()
                                                {
                                                    POHeaderID = model.FormID,
                                                    IsApproved = true,
                                                    IsRejected = false,
                                                    ModifiedUserID = model.CurUserID
                                                }, _transaction);

                                                //Enter Purchase Order in SAP
                                                var mdSRN = new POHeaderModel { POHeaderID = model.FormID, ProcessKey = "", Month = null };
                                                var poHeaderSRN = srvPOHeader.FetchPOHeader(mdSRN, _transaction).First();
                                                var poLinesSRN = srvPOLines.GetPOLinesByPOHeaderID(model.FormID, _transaction);
                                                if (string.IsNullOrEmpty(poHeaderSRN?.DocNum) || poHeaderSRN?.DocNum == "0")
                                                {
                                                    var sapSRNResponse = srvSAPApi.SaveSAPPurchaseOrder(poHeaderSRN, poLinesSRN, _transaction).Result;
                                                    if (sapSRNResponse.Status == "1"
                                                        //|| (sapSRNResponse.Status == "0" && sapSRNResponse.POModel.DocEntry > 0 && !string.IsNullOrEmpty(sapSRNResponse.POModel.DocNum))
                                                        )
                                                    {
                                                        new SRVPOHeader().UpdateSAPInPOHeader(model.FormID, sapSRNResponse.POModel.DocEntry, sapSRNResponse.POModel.DocNum, _transaction);
                                                        // new SRVPOLines().UpdateSAPInPOLines(model.FormID, sapSRNResponse.POModel.PODetail, poLinesSRN, _transaction);
                                                    }
                                                }
                                                srvSRN.GenerateInvoiceHeader_SRN(model.FormID, _transaction, EnumApprovalProcess.INV_SRN);
                                                _transaction.Commit();
                                                result = true;
                                                break;

                                            case EnumApprovalProcess.PO_GURN:
                                                srvPurchaseOrder.POHeaderApproveReject(new POHeaderModel()
                                                {
                                                    POHeaderID = model.FormID,
                                                    IsApproved = true,
                                                    IsRejected = false,
                                                    ModifiedUserID = model.CurUserID
                                                }, _transaction);

                                                //Enter Purchase Order in SAP
                                                var mdGURN = new POHeaderModel { POHeaderID = model.FormID, ProcessKey = "", Month = null };
                                                var poHeaderGURN = srvPOHeader.FetchPOHeader(mdGURN, _transaction).First();
                                                var poLinesGURN = srvPOLines.GetPOLinesByPOHeaderID(model.FormID, _transaction);
                                                if (string.IsNullOrEmpty(poHeaderGURN?.DocNum) || poHeaderGURN?.DocNum == "0")
                                                {
                                                    var sapGURNResponse = srvSAPApi.SaveSAPPurchaseOrder(poHeaderGURN, poLinesGURN, _transaction).Result;
                                                    if (sapGURNResponse.Status == "1"
                                                        //|| (sapSRNResponse.Status == "0" && sapSRNResponse.POModel.DocEntry > 0 && !string.IsNullOrEmpty(sapSRNResponse.POModel.DocNum))
                                                        )
                                                    {
                                                        new SRVPOHeader().UpdateSAPInPOHeader(model.FormID, sapGURNResponse.POModel.DocEntry, sapGURNResponse.POModel.DocNum, _transaction);
                                                        // new SRVPOLines().UpdateSAPInPOLines(model.FormID, sapSRNResponse.POModel.PODetail, poLinesSRN, _transaction);
                                                    }
                                                }
                                                srvGURN.GenerateInvoiceHeader_GURN(model.FormID, _transaction, EnumApprovalProcess.INV_GURN);
                                                _transaction.Commit();
                                                result = true;
                                                break;

                                            case EnumApprovalProcess.PO_VRN:
                                                srvPurchaseOrder.POHeaderApproveReject(new POHeaderModel()
                                                {
                                                    POHeaderID = model.FormID,
                                                    IsApproved = true,
                                                    IsRejected = false,
                                                    ModifiedUserID = model.CurUserID
                                                }, _transaction);

                                                //Enter Purchase Order in SAP
                                                var mdVRN = new POHeaderModel { POHeaderID = model.FormID, ProcessKey = "", Month = null };
                                                var poHeaderVRN = srvPOHeader.FetchPOHeader(mdVRN, _transaction).First();
                                                var poLinesVRN = srvPOLines.GetPOLinesByPOHeaderID(model.FormID, _transaction);
                                                if (string.IsNullOrEmpty(poHeaderVRN?.DocNum) || poHeaderVRN?.DocNum == "0")
                                                {
                                                    var sapSRNResponse = srvSAPApi.SaveSAPPurchaseOrder(poHeaderVRN, poLinesVRN, _transaction).Result;
                                                    if (sapSRNResponse.Status == "1"
                                                        //|| (sapSRNResponse.Status == "0" && sapSRNResponse.POModel.DocEntry > 0 && !string.IsNullOrEmpty(sapSRNResponse.POModel.DocNum))
                                                        )
                                                    {
                                                        new SRVPOHeader().UpdateSAPInPOHeader(model.FormID, sapSRNResponse.POModel.DocEntry, sapSRNResponse.POModel.DocNum, _transaction);
                                                        // new SRVPOLines().UpdateSAPInPOLines(model.FormID, sapSRNResponse.POModel.PODetail, poLinesSRN, _transaction);
                                                    }
                                                }
                                                srvSRN.GenerateInvoiceHeader_SRN(model.FormID, _transaction, EnumApprovalProcess.INV_VRN);
                                                _transaction.Commit();
                                                result = true;
                                                break;


                                            case EnumApprovalProcess.PO_TSP:
                                                List<POHeaderModel> POHeaderModel = srvPurchaseOrder.GetPOHeaderByID(model.FormID, _transaction);
                                                srvPurchaseOrder.POHeaderApproveReject(new POHeaderModel()
                                                {
                                                    POHeaderID = model.FormID,
                                                    IsApproved = true,
                                                    IsRejected = false,
                                                    ModifiedUserID = model.CurUserID
                                                }, _transaction);
                                                //Enter Purchase Order in SAP
                                                var mdTSP = new POHeaderModel { POHeaderID = model.FormID, ProcessKey = "", Month = null };
                                                var poHeaderTSP = srvPOHeader.FetchPOHeader(mdTSP, _transaction).First();
                                                var poLinesTSP = srvPOLines.GetPOLinesByPOHeaderID(model.FormID, _transaction);
                                                if (string.IsNullOrEmpty(poHeaderTSP?.DocNum) || poHeaderTSP?.DocNum == "0")
                                                {
                                                    var sapTSPResponse = srvSAPApi.SaveSAPPurchaseOrder(poHeaderTSP, poLinesTSP, _transaction).Result;
                                                    if (sapTSPResponse.Status == "1"
                                                        //|| (sapTSPResponse.Status == "0" && sapTSPResponse.POModel.DocEntry > 0 && !string.IsNullOrEmpty(sapTSPResponse.POModel.DocNum))
                                                        )
                                                    {
                                                        new SRVPOHeader().UpdateSAPInPOHeader(model.FormID, sapTSPResponse.POModel.DocEntry, sapTSPResponse.POModel.DocNum, _transaction);
                                                        // new SRVPOLines().UpdateSAPInPOLines(model.FormID, sapTSPResponse.POModel.PODetail, poLinesTSP, _transaction);
                                                    }
                                                }
                                                _transaction.Commit();
                                                result = true;
                                                break;

                                            case EnumApprovalProcess.INV_C:
                                            case EnumApprovalProcess.INV_SRN:
                                            case EnumApprovalProcess.INV_GURN:
                                            case EnumApprovalProcess.INV_VRN:
                                            case EnumApprovalProcess.INV_F:
                                                ApprovalProcessModel ApprovalProcessInvoiceF = _srvApprovalProcess.GetByProcessKey(model.ProcessKey);
                                                srvInvoiceMaster.InvoiceHeaderApproveReject(new InvoiceMasterModel()
                                                {
                                                    InvoiceHeaderID = model.FormID,
                                                    IsApproved = true,
                                                    IsRejected = false
                                                }, _transaction);

                                                //Enter Invoice in SAP
                                                var invoiceModelSRN = new InvoiceMasterModel { InvoiceHeaderID = model.FormID, ProcessKey = "" };
                                                var invoiceHeaderSRN = srvInvoiceMaster.GetInvoicesForApproval(invoiceModelSRN, _transaction).First();
                                                var invoicesSRN = srvInvoice.GetInvoicesForApproval(model.FormID, _transaction);
                                                if (string.IsNullOrEmpty(invoiceHeaderSRN?.SAPID))
                                                {
                                                    var sapINVSRN = srvSAPApi.SaveSAPAPInvoice(invoiceHeaderSRN, invoicesSRN, _transaction).Result;
                                                    if (sapINVSRN.Status == "1" || (sapINVSRN.Status == "0" && !string.IsNullOrEmpty(sapINVSRN.SapObjId)))
                                                    {
                                                        srvInvoiceMaster.UpdateSAPObjIdInInvoiceHeader(sapINVSRN.SapObjId, model.FormID, _transaction);
                                                        srvInvoice.UpdateSAPObjIdInInvoices(sapINVSRN.SapObjId, model.FormID, _transaction);
                                                    }
                                                }
                                                _transaction.Commit();
                                                result = true;
                                                break;

                                            case EnumApprovalProcess.INV_TRN:
                                                ApprovalProcessModel ApprovalProcessInvoiceTRN = _srvApprovalProcess.GetByProcessKey(model.ProcessKey);
                                                srvInvoiceMaster.InvoiceHeaderApproveReject(new InvoiceMasterModel()
                                                {
                                                    InvoiceHeaderID = model.FormID,
                                                    IsApproved = true,
                                                    IsRejected = false
                                                }, _transaction);

                                                //Enter Invoice in SAP
                                                var invoiceModelTRN = new InvoiceMasterModel { InvoiceHeaderID = model.FormID, ProcessKey = "" };
                                                var invoiceHeaderTRN = srvInvoiceMaster.GetInvoicesForApproval(invoiceModelTRN, _transaction).First();
                                                var invoicesTRN = srvInvoice.GetInvoicesForApproval(model.FormID, _transaction);
                                                if (string.IsNullOrEmpty(invoiceHeaderTRN?.SAPID))
                                                {
                                                    var sapINVTRN = srvSAPApi.SaveSAPAPInvoice(invoiceHeaderTRN, invoicesTRN, _transaction).Result;
                                                    if (sapINVTRN.Status == "1" || (sapINVTRN.Status == "0" && !string.IsNullOrEmpty(sapINVTRN.SapObjId)))
                                                    {
                                                        srvInvoiceMaster.UpdateSAPObjIdInInvoiceHeader(sapINVTRN.SapObjId, model.FormID, _transaction);
                                                        srvInvoice.UpdateSAPObjIdInInvoices(sapINVTRN.SapObjId, model.FormID, _transaction);
                                                    }
                                                }
                                                _transaction.Commit();
                                                result = true;
                                                break;

                                            case EnumApprovalProcess.INV_R:
                                            case EnumApprovalProcess.INV_1ST:
                                            case EnumApprovalProcess.INV_2ND:
                                                ApprovalProcessModel ApprovalProcessInvoiceR = _srvApprovalProcess.GetByProcessKey(model.ProcessKey);
                                                srvInvoiceMaster.InvoiceHeaderApproveReject(new InvoiceMasterModel()
                                                {
                                                    InvoiceHeaderID = model.FormID,
                                                    IsApproved = true,
                                                    IsRejected = false
                                                }, _transaction);

                                                var invoiceModel = new InvoiceMasterModel { InvoiceHeaderID = model.FormID, ProcessKey = "" };
                                                var invoiceHeader = srvInvoiceMaster.GetInvoicesForApproval(invoiceModel, _transaction).First();
                                                var invoices = srvInvoice.GetInvoicesForApproval(model.FormID, _transaction);

                                                if (string.IsNullOrEmpty(invoiceHeader?.SAPID))
                                                {
                                                    //Enter Invoice in SAP
                                                    var sapINV = srvSAPApi.SaveSAPAPInvoice(invoiceHeader, invoices, _transaction).Result;
                                                    if (sapINV.Status == "1" || (sapINV.Status == "0" && !string.IsNullOrEmpty(sapINV.SapObjId)))
                                                    {
                                                        srvInvoiceMaster.UpdateSAPObjIdInInvoiceHeader(sapINV.SapObjId, model.FormID, _transaction);
                                                        srvInvoice.UpdateSAPObjIdInInvoices(sapINV.SapObjId, model.FormID, _transaction);
                                                    }
                                                }
                                                _transaction.Commit();
                                                result = true;
                                                break;

                                            case EnumApprovalProcess.Calcelation:
                                                ClassInvoiceMapModel invMap = srvcancel.GetInvoices(model.FormID);
                                                srvcancel.CancellationApproval(invMap.ClassID, invMap.Month, invMap.InvoiceType, invMap.InvoiceID, true, _transaction);
                                                _transaction.Commit();
                                                result = true;
                                                break;

                                            default:
                                                break;
                                        }
                                    }
                                    break;

                                case (int)EnumApprovalStatus.SendBack:
                                    ///add previous Approval History Record with pending status
                                    historyModel.Step = currentApproval.Step - 1;
                                    AU_ApprovalHistory(historyModel, _transaction);
                                    _transaction.Commit();
                                    result = true;
                                    break;

                                case (int)EnumApprovalStatus.Rejected:
                                    ///add first Approval History Record with pending status
                                    switch (model.ProcessKey)
                                    {
                                        //case EnumApprovalProcess.AP_PD:
                                        case EnumApprovalProcess.AP_BD:
                                            ///Rejected scheme is now Editable for created user
                                            SchemeModel scheme = srvScheme.GetBySchemeID_Notification(model.FormID, _transaction);
                                            srvScheme.SchemeApproveReject(new SchemeModel()
                                            {
                                                SchemeID = model.FormID,
                                                IsApproved = false,
                                                IsRejected = true,
                                                CurUserID = model.CurUserID
                                            }, _transaction);
                                            _transaction.Commit();
                                            result = true;
                                            break;

                                        case EnumApprovalProcess.PROG_APP:
                                            //SchemeModel ProgramDesign = srvScheme.GetBySchemeID_Notification(model.FormID, _transaction);
                                            srvProgramDesign.ProgramApproveReject(new ProgramDesignModel()
                                            {
                                                ProgramID = model.FormID,
                                                IsApproved = false,
                                                IsRejected = true,
                                                CurUserID = model.CurUserID
                                            }, _transaction);
                                            _transaction.Commit();
                                            result = true;
                                            break;

                                        case EnumApprovalProcess.CRTEM_APP:
                                            //SchemeModel ProgramDesign = srvScheme.GetBySchemeID_Notification(model.FormID, _transaction);
                                            srvCriteria.CriteriaApproveReject(new CriteriaTemplateModel()
                                            {
                                                CriteriaTemplateID = model.FormID,
                                                IsApproved = false,
                                                IsRejected = true,
                                                CurUserID = model.CurUserID
                                            }, _transaction);
                                            _transaction.Commit();
                                            result = true;
                                            break;


                                        case EnumApprovalProcess.TRD:
                                            TradeModel Trade = srvTrade.GetByTradeID_Notification(model.FormID, _transaction);
                                            srvTrade.TradeApproveReject(new TradeModel()
                                            {
                                                TradeID = model.FormID,
                                                IsApproved = false,
                                                IsRejected = true,
                                                CurUserID = model.CurUserID
                                            }, _transaction);
                                            _transaction.Commit();
                                            result = true;
                                            break;

                                        case EnumApprovalProcess.CR_SCHEME:
                                            SchemeChangeRequestModel SchemeChangeRequest = srvCrScheme.GetBySchemeChangeRequestID(model.FormID, _transaction);
                                            srvCrScheme.CrSchemeApproveReject(new SchemeChangeRequestModel()
                                            {
                                                SchemeChangeRequestID = model.FormID,
                                                IsApproved = false,
                                                IsRejected = true,
                                                CurUserID = model.CurUserID
                                            }, _transaction);
                                            _transaction.Commit();
                                            result = true;
                                            break;

                                        case EnumApprovalProcess.CR_TSP:
                                            TSPChangeRequestModel TSPChangeRequest = srvCrTSP.GetByTSPChangeRequestID_Notification(model.FormID, _transaction);
                                            srvCrTSP.CrTSPApproveReject(new TSPChangeRequestModel()
                                            {
                                                TSPChangeRequestID = model.FormID,
                                                IsApproved = false,
                                                IsRejected = true,
                                                CurUserID = model.CurUserID
                                            }, _transaction);
                                            _transaction.Commit();
                                            result = true;
                                            break;

                                        case EnumApprovalProcess.CR_CLASS_LOCATION:
                                            ClassChangeRequestModel ClassChangeRequest = srvCrClass.GetByClassChangeRequestID_Notification(model.FormID, _transaction);
                                            srvCrClass.CrClassApproveReject(new ClassChangeRequestModel()
                                            {
                                                ClassChangeRequestID = model.FormID,
                                                IsApproved = false,
                                                IsRejected = true,
                                                CurUserID = model.CurUserID
                                            }, _transaction);
                                            _transaction.Commit();
                                            result = true;
                                            break;

                                        case EnumApprovalProcess.CR_CLASS_DATES:
                                            ClassChangeRequestModel ClassChangeRequestModel = srvCrClass.GetByClassChangeRequestID_Notification(model.FormID, _transaction);
                                            srvCrClass.CrClassDatesApproveReject(new ClassChangeRequestModel()
                                            {
                                                ClassChangeRequestID = model.FormID,
                                                IsApproved = false,
                                                IsRejected = true,
                                                CurUserID = model.CurUserID
                                            }, _transaction);
                                            _transaction.Commit();
                                            result = true;
                                            break;

                                        case EnumApprovalProcess.CR_TRAINEE_UNVERIFIED:
                                            TraineeChangeRequestModel TraineeChangeRequestModel = srvCrTrainee.GetByTraineeChangeRequestID_Notification(model.FormID, _transaction);
                                            srvCrTrainee.CrTraineeApproveReject(new TraineeChangeRequestModel()
                                            {
                                                TraineeChangeRequestID = model.FormID,
                                                IsApproved = false,
                                                IsRejected = true,
                                                CurUserID = model.CurUserID
                                            }, _transaction);
                                            _transaction.Commit();
                                            result = true;
                                            break;

                                        case EnumApprovalProcess.CR_TRAINEE_VERIFIED:
                                            TraineeChangeRequestModel TraineeChangeRequestModel2 = srvCrTrainee.GetByTraineeChangeRequestID_Notification(model.FormID, _transaction);
                                            srvCrTrainee.CrTraineeApproveReject(new TraineeChangeRequestModel()
                                            {
                                                TraineeChangeRequestID = model.FormID,
                                                IsApproved = false,
                                                IsRejected = true,
                                                CurUserID = model.CurUserID
                                            }, _transaction);
                                            _transaction.Commit();
                                            result = true;
                                            break;

                                        case EnumApprovalProcess.CR_INSTRUCTOR:

                                            InstructorChangeRequestModel InstructorChangeRequest = srvCrInstructor.GetByInstructorChangeRequestID_Notification(model.FormID, _transaction);
                                            srvCrInstructor.CrInstructorApproveReject(new InstructorChangeRequestModel()
                                            {
                                                InstructorChangeRequestID = model.FormID,
                                                IsApproved = false,
                                                IsRejected = true,
                                                CurUserID = model.CurUserID
                                            }, _transaction);
                                            _transaction.Commit();
                                            result = true;
                                            break;

                                        case EnumApprovalProcess.CR_NEW_INSTRUCTOR:
                                            InstructorChangeRequestModel NewInstructorChangeRequest = srvCrInstructor.GetByNewInstructorChangeRequestID_Notification(model.FormID, _transaction);
                                            srvCrInstructor.CrNewInstructorApproveReject(new InstructorChangeRequestModel()
                                            {
                                                CRNewInstructorID = model.FormID,
                                                IsApproved = false,
                                                IsRejected = true,
                                                CurUserID = model.CurUserID
                                            }, _transaction);
                                            _transaction.Commit();
                                            result = true;
                                            break;

                                        case EnumApprovalProcess.CR_INCEPTION:
                                            InceptionReportChangeRequestModel InceptionReportChangeRequest = srvCrInceptionReport.GetByInceptionReportChangeRequestID(model.FormID, _transaction);
                                            srvCrInceptionReport.CrInceptionReportApproveReject(new InceptionReportChangeRequestModel()
                                            {
                                                InceptionReportChangeRequestID = model.FormID,
                                                IsApproved = false,
                                                IsRejected = true,
                                                CurUserID = model.CurUserID
                                            }, _transaction);
                                            _transaction.Commit();
                                            result = true;
                                            break;

                                        case EnumApprovalProcess.CR_INSTRUCTOR_REPLACE:
                                            InstructorReplaceChangeRequestModel InstructorReplaceChangeRequest = srvCrInstructorReplace.GetByInstructorReplaceChangeRequestID(model.FormID, _transaction);
                                            srvCrInstructorReplace.CrInstructorReplaceApproveReject(new InstructorReplaceChangeRequestModel()
                                            {
                                                InstructorReplaceChangeRequestID = model.FormID,
                                                IsApproved = false,
                                                IsRejected = true,
                                                CurUserID = model.CurUserID
                                            }, _transaction);
                                            _transaction.Commit();
                                            result = true;
                                            break;

                                        case EnumApprovalProcess.PRN_R:
                                        case EnumApprovalProcess.PRN_C:
                                            List<PRNMasterModel> PRNMasterModel = srvPRNMaster.GetPRNMasterForApproval(model.FormID, _transaction);
                                            srvPRNMaster.PRNMasterApproveReject(new PRNMasterModel()
                                            {
                                                PRNMasterID = model.FormID,
                                                IsApproved = false,
                                                IsRejected = true,
                                                CurUserID = model.CurUserID
                                            }, _transaction);
                                            _transaction.Commit();
                                            result = true;
                                            break;

                                        case EnumApprovalProcess.PRN_T:
                                            srvSRN.TRNApproveReject(new TRNMasterModel()
                                            {
                                                TRNMasterID = model.FormID,
                                                IsApproved = false,
                                                IsRejected = true,
                                                ModifiedUserID = model.CurUserID
                                            }, _transaction);

                                            //created ID Null And created by system
                                            _transaction.Commit();
                                            result = true;
                                            break;

                                        case EnumApprovalProcess.PO_TRN:
                                        case EnumApprovalProcess.PO_SRN:
                                        case EnumApprovalProcess.PO_TSP:

                                            List<POHeaderModel> POHeaderModel = srvPurchaseOrder.GetPOHeaderByID(model.FormID, _transaction);
                                            srvPurchaseOrder.POHeaderApproveReject(new POHeaderModel()
                                            {
                                                POHeaderID = model.FormID,
                                                IsApproved = false,
                                                IsRejected = true,
                                                ModifiedUserID = model.CurUserID
                                            }, _transaction);
                                            _transaction.Commit();
                                            result = true;
                                            break;

                                        case EnumApprovalProcess.INV_C:
                                        case EnumApprovalProcess.INV_F:
                                        case EnumApprovalProcess.INV_SRN:
                                            List<InvoiceMasterModel> invoiceHeader = srvInvoiceMaster.GetInvoicesForApproval(new InvoiceMasterModel() { InvoiceHeaderID = model.FormID, ProcessKey = model.ProcessKey }, _transaction);
                                            srvInvoiceMaster.InvoiceHeaderApproveReject(new InvoiceMasterModel()
                                            {
                                                InvoiceHeaderID = model.FormID,
                                                IsApproved = false,
                                                IsRejected = true
                                            }, _transaction);
                                            //created ID =0 and created by system
                                            _transaction.Commit();
                                            result = true;
                                            break;

                                        case EnumApprovalProcess.INV_GURN:
                                            List<InvoiceMasterModel> invoiceHeaderGURN = srvInvoiceMaster.GetInvoicesForApproval(new InvoiceMasterModel() { InvoiceHeaderID = model.FormID, ProcessKey = model.ProcessKey }, _transaction);
                                            srvInvoiceMaster.InvoiceHeaderApproveReject(new InvoiceMasterModel()
                                            {
                                                InvoiceHeaderID = model.FormID,
                                                IsApproved = false,
                                                IsRejected = true
                                            }, _transaction);
                                            //created ID =0 and created by system
                                            _transaction.Commit();
                                            result = true;
                                            break;

                                        case EnumApprovalProcess.Calcelation:
                                            ClassInvoiceMapModel invMap = srvcancel.GetInvoices(model.FormID);
                                            srvcancel.CancellationApproval(invMap.ClassID, invMap.Month, invMap.InvoiceType, invMap.InvoiceID, false, _transaction);

                                            _transaction.Commit();
                                            result = true;
                                            break;

                                        default:
                                            break;
                                    }
                                    break;

                                default:
                                    break;
                            }
                        }
                        else
                        {
                            ///Record not updated successfully
                        }
                    }
                    else
                    {
                        ///Fake Approver
                        //result = false;
                    }
                }
                catch (Exception ex)
                {
                    _transaction.Rollback();
                    throw;
                }
            }

            return result;
        }

        public void SendApprovalNotification(ApprovalWrapperModel wrapperModel)
        {
            var model = wrapperModel.approvalHistoryModel;
            var approvals = wrapperModel.approvals;
            var currentApproval = wrapperModel.currentApproval;
            var isFinalApprover = wrapperModel.isFinalApprover;
            ApprovalModel approvalsModelForNotification = new ApprovalModel();

            try
            {
                if (currentApproval.UserIDs.Split(',').Contains(model.CurUserID.ToString()))
                {
                    // Update Current History Record
                    // Add new History Record with latest ApprovalID & PendingStatus
                    ApprovalHistoryModel historyModel = new ApprovalHistoryModel()
                    {
                        ApprovalHistoryID = 0,
                        ProcessKey = model.ProcessKey,
                        FormID = model.FormID,
                        ApprovalStatusID = (int)EnumApprovalStatus.Pending,
                        Comments = "Pending",
                        ApproverID = null,
                        CurUserID = model.CurUserID,
                        InActive = false
                    };
                    switch (model.ApprovalStatusID)
                    {
                        case (int)EnumApprovalStatus.Approved:
                            ///add next Approval History Record with pending status
                            if (!isFinalApprover)
                            {
                                historyModel.Step = currentApproval.Step + 1;
                                var nextApproval = approvals.Where(x => x.Step == currentApproval.Step + 1).FirstOrDefault();
                                if (model.ProcessKey == EnumApprovalProcess.CR_TSP)
                                {
                                    var tspChangeRequest = srvCrTSP.GetByTSPChangeRequestID(model.FormID);
                                    nextApproval.CustomComments = " Approved TSP Name:  (" + tspChangeRequest.TSPName + ")";
                                    nextApproval.UserIDs += "," + tspChangeRequest.CreatedUserID;
                                }
                                else if (model.ProcessKey == EnumApprovalProcess.CR_NEW_INSTRUCTOR)
                                {
                                    InstructorChangeRequestModel NewInstructorChangeRequest = srvCrInstructor.GetByNewInstructorChangeRequestID_Notification(model.FormID);
                                    nextApproval.CustomComments = "Approved .(NameOfOrganization: " + NewInstructorChangeRequest.NameOfOrganization + ",InstructorName: " + NewInstructorChangeRequest.InstructorName + ")";
                                }
                                else if (model.ProcessKey == EnumApprovalProcess.CR_INSTRUCTOR_REPLACE)
                                {
                                    InstructorReplaceChangeRequestModel InstructorReplaceChangeRequest = srvCrInstructorReplace.GetByInstructorReplaceChangeRequestID(model.FormID);

                                    nextApproval.CustomComments = "Approved .(" + "Instructor Name: " + InstructorReplaceChangeRequest.InstructorName + ",TSP Name:" + InstructorReplaceChangeRequest.TSPName + ",Scheme Name: " + InstructorReplaceChangeRequest.SchemeName + ") ";
                                }
                                else if (model.ProcessKey == EnumApprovalProcess.CR_INSTRUCTOR)
                                {
                                    InstructorChangeRequestModel InstructorChangeRequest = srvCrInstructor.GetByInstructorChangeRequestID_Notification(model.FormID);
                                    nextApproval.CustomComments = "Approved . (InstructorName: " + InstructorChangeRequest.InstructorName + ")";
                                }
                                else if (model.ProcessKey == EnumApprovalProcess.PO_TSP)
                                {
                                    List<POHeaderModel> POHeaderModel = srvPurchaseOrder.GetPOHeaderByID(model.FormID);
                                    nextApproval.CustomComments = "Approved . (Card Name: " + POHeaderModel[0].CardName + ",Document Number" + POHeaderModel[0].DocNum + ")";
                                }
                                else if (model.ProcessKey == EnumApprovalProcess.AP_BD || model.ProcessKey == EnumApprovalProcess.AP_PD)
                                {
                                    SchemeModel scheme = srvScheme.GetBySchemeID_Notification(model.FormID);
                                    nextApproval.CustomComments = "Approved . (Scheme Name: " + scheme.SchemeName + ")";
                                }
                                else if (model.ProcessKey == EnumApprovalProcess.PRN_C || model.ProcessKey == EnumApprovalProcess.PRN_F || model.ProcessKey == EnumApprovalProcess.PRN_R || model.ProcessKey == EnumApprovalProcess.PRN_T)
                                {
                                    List<PRNMasterModel> PRNMasterModel = srvPRNMaster.GetPRNMasterForApproval(model.FormID);
                                    nextApproval.CustomComments = "Approved .PRN for the month of (" + PRNMasterModel[0].Month + ") has been approved for Class Code (" + PRNMasterModel[0].ClassCode + ")";
                                }
                                else if (model.ProcessKey == EnumApprovalProcess.INV_1ST || model.ProcessKey == EnumApprovalProcess.INV_2ND || model.ProcessKey == EnumApprovalProcess.INV_C || model.ProcessKey == EnumApprovalProcess.INV_F || model.ProcessKey == EnumApprovalProcess.INV_R || model.ProcessKey == EnumApprovalProcess.INV_SRN || model.ProcessKey == EnumApprovalProcess.INV_GURN || model.ProcessKey == EnumApprovalProcess.INV_TRN)
                                {
                                    ApprovalProcessModel ApprovalProcessInvoice = _srvApprovalProcess.GetByProcessKey(model.ProcessKey);
                                    List<InvoiceMasterModel> invoiceHeader = srvInvoiceMaster.GetInvoicesForApproval(new InvoiceMasterModel() { InvoiceHeaderID = model.FormID, ProcessKey = "" });
                                    nextApproval.CustomComments = "Approved for the month of (" + invoiceHeader[0].U_Month + ") has been approved. Scheme Name: (" + invoiceHeader[0].SchemeName + ") , TSP Name: (" + invoiceHeader[0].TSPName + ") ,  Invoice type: (" + ApprovalProcessInvoice.ApprovalProcessName + ")";

                                }
                                else
                                {
                                    nextApproval.CustomComments = "Approved and  assigned to you for approval. Kindly login to your PSDF BSS account to view details and further actions";
                                }
                                srvSendEmail.GenerateEmailAndSendNotification(nextApproval, model);
                            }
                            else
                            {
                                switch (model.ProcessKey)
                                {
                                    case EnumApprovalProcess.AP_PD:
                                    case EnumApprovalProcess.AP_BD:
                                        var scheme = srvScheme.GetBySchemeID(model.FormID);
                                        List<EmailUsersModel> users = generateInvoice.CreateTSPsAccounts(model.FormID, model.CurUserID);
                                        generateInvoice.GenerateEmailsToUsers(users);
                                        approvalsModelForNotification.isUserMapping = true;
                                        approvalsModelForNotification.ProcessKey = model.ProcessKey;
                                        approvalsModelForNotification.UserIDs = currentApproval.UserIDs;
                                        approvalsModelForNotification.UserIDs += "," + scheme.CreatedUserID.ToString();
                                        approvalsModelForNotification.CustomComments = "Final Approved . (Scheme Name: " + scheme.SchemeName + ")";
                                        srvSendEmail.GenerateEmailAndSendNotification(approvalsModelForNotification, model);
                                        break;

                                    case EnumApprovalProcess.TRD:

                                        approvalsModelForNotification.isUserMapping = true;
                                        approvalsModelForNotification.UserIDs = currentApproval.UserIDs;
                                        approvalsModelForNotification.ProcessKey = model.ProcessKey;
                                        approvalsModelForNotification.CustomComments = "Final Approved";
                                        srvSendEmail.GenerateEmailAndSendNotification(approvalsModelForNotification, model);
                                        break;

                                    case EnumApprovalProcess.CR_SCHEME:
                                        approvalsModelForNotification.isUserMapping = true;
                                        approvalsModelForNotification.ProcessKey = model.ProcessKey;
                                        approvalsModelForNotification.CustomComments = " Final Approved";
                                        approvalsModelForNotification.UserIDs = currentApproval.UserIDs;
                                        srvSendEmail.GenerateEmailAndSendNotification(approvalsModelForNotification, model);
                                        break;

                                    case EnumApprovalProcess.CR_TSP:
                                        var tspChangeRequest = srvCrTSP.GetByTSPChangeRequestID(model.FormID);
                                        approvalsModelForNotification.isUserMapping = true;
                                        approvalsModelForNotification.ProcessKey = model.ProcessKey;
                                        approvalsModelForNotification.CustomComments = "Final Approved TSP Name:  (" + tspChangeRequest.TSPName + ")";
                                        approvalsModelForNotification.UserIDs = currentApproval.UserIDs;
                                        approvalsModelForNotification.UserIDs += "," + tspChangeRequest.CreatedUserID.ToString();
                                        srvSendEmail.GenerateEmailAndSendNotification(approvalsModelForNotification, model);
                                        break;

                                    case EnumApprovalProcess.CR_CLASS_LOCATION:
                                        approvalsModelForNotification.isUserMapping = true;
                                        approvalsModelForNotification.ProcessKey = model.ProcessKey;
                                        approvalsModelForNotification.CustomComments = "Final Approved";
                                        approvalsModelForNotification.UserIDs = currentApproval.UserIDs;
                                        srvSendEmail.GenerateEmailAndSendNotification(approvalsModelForNotification, model);
                                        break;

                                    case EnumApprovalProcess.CR_CLASS_DATES:
                                        approvalsModelForNotification.isUserMapping = true;
                                        approvalsModelForNotification.ProcessKey = model.ProcessKey;
                                        approvalsModelForNotification.CustomComments = "Final approved";
                                        approvalsModelForNotification.UserIDs = currentApproval.UserIDs;
                                        srvSendEmail.GenerateEmailAndSendNotification(approvalsModelForNotification, model);
                                        break;

                                    case EnumApprovalProcess.CR_TRAINEE_UNVERIFIED:
                                        approvalsModelForNotification.isUserMapping = true;
                                        approvalsModelForNotification.ProcessKey = model.ProcessKey;
                                        approvalsModelForNotification.CustomComments = "Final Approved";
                                        approvalsModelForNotification.UserIDs = currentApproval.UserIDs;
                                        srvSendEmail.GenerateEmailAndSendNotification(approvalsModelForNotification, model);
                                        break;

                                    case EnumApprovalProcess.CR_TRAINEE_VERIFIED:
                                        approvalsModelForNotification.isUserMapping = true;
                                        approvalsModelForNotification.ProcessKey = model.ProcessKey;
                                        approvalsModelForNotification.CustomComments = "Final Approved";
                                        approvalsModelForNotification.UserIDs = currentApproval.UserIDs;
                                        srvSendEmail.GenerateEmailAndSendNotification(approvalsModelForNotification, model);
                                        break;

                                    case EnumApprovalProcess.CR_INSTRUCTOR:
                                        InstructorChangeRequestModel InstructorChangeRequest = srvCrInstructor.GetByInstructorChangeRequestID_Notification(model.FormID);
                                        approvalsModelForNotification.isUserMapping = true;
                                        approvalsModelForNotification.ProcessKey = model.ProcessKey;
                                        approvalsModelForNotification.CustomComments = "Final Approved . (InstructorName: " + InstructorChangeRequest.InstructorName + ")";
                                        approvalsModelForNotification.UserIDs = currentApproval.UserIDs;
                                        approvalsModelForNotification.UserIDs += "," + InstructorChangeRequest.CreatedUserID.ToString(); ;
                                        srvSendEmail.GenerateEmailAndSendNotification(approvalsModelForNotification, model);
                                        break;

                                    case EnumApprovalProcess.CR_NEW_INSTRUCTOR:
                                        InstructorChangeRequestModel NewInstructorChangeRequest = srvCrInstructor.GetByNewInstructorChangeRequestID_Notification(model.FormID);
                                        approvalsModelForNotification.isUserMapping = true;
                                        approvalsModelForNotification.ProcessKey = model.ProcessKey;
                                        approvalsModelForNotification.CustomComments = "Final Approved .(Name Of Organization: " + NewInstructorChangeRequest.NameOfOrganization + ",Instructor Name: " + NewInstructorChangeRequest.InstructorName + ")";
                                        approvalsModelForNotification.UserIDs = currentApproval.UserIDs;
                                        approvalsModelForNotification.UserIDs += "," + NewInstructorChangeRequest.CreatedUserID;
                                        srvSendEmail.GenerateEmailAndSendNotification(approvalsModelForNotification, model);
                                        break;

                                    case EnumApprovalProcess.CR_INCEPTION:
                                        approvalsModelForNotification.isUserMapping = true;
                                        approvalsModelForNotification.ProcessKey = model.ProcessKey;
                                        approvalsModelForNotification.CustomComments = "Final Approved";
                                        approvalsModelForNotification.UserIDs = currentApproval.UserIDs;
                                        srvSendEmail.GenerateEmailAndSendNotification(approvalsModelForNotification, model);
                                        break;

                                    case EnumApprovalProcess.CR_INSTRUCTOR_REPLACE:
                                        InstructorReplaceChangeRequestModel InstructorReplaceChangeRequest = srvCrInstructorReplace.GetByInstructorReplaceChangeRequestID(model.FormID);
                                        approvalsModelForNotification.isUserMapping = true;
                                        approvalsModelForNotification.ProcessKey = model.ProcessKey;
                                        approvalsModelForNotification.CustomComments = "Final Approved .(" + "Instructor Name: " + InstructorReplaceChangeRequest.InstructorName + ",TSP Name:" + InstructorReplaceChangeRequest.TSPName + ",Scheme Name: " + InstructorReplaceChangeRequest.SchemeName + ") ";
                                        approvalsModelForNotification.UserIDs = currentApproval.UserIDs;
                                        approvalsModelForNotification.UserIDs += "," + InstructorReplaceChangeRequest.CreatedUserID.ToString();
                                        srvSendEmail.GenerateEmailAndSendNotification(approvalsModelForNotification, model);
                                        break;

                                    case EnumApprovalProcess.PRN_R:
                                        List<PRNMasterModel> PRNMasterModel = srvPRNMaster.GetPRNMasterForApproval(model.FormID);
                                        ApprovalProcessModel ApprovalProcessModelR = _srvApprovalProcess.GetByProcessKey(EnumApprovalProcess.INV_R);
                                        approvalsModelForNotification.isUserMapping = true;
                                        approvalsModelForNotification.ProcessKey = model.ProcessKey;
                                        approvalsModelForNotification.CustomComments = "Final Approved .PRN for the month of (" + PRNMasterModel[0].Month?.ToString("MM/yyyy") + ") has been approved. Class Code: (" + PRNMasterModel[0].ClassCode + "), Scheme Name: (" + PRNMasterModel[0].SchemeName + ") , TSP Name: (" + PRNMasterModel[0].TSPName + ") ,  Invoice type: (" + ApprovalProcessModelR.ApprovalProcessName + ")";
                                        approvalsModelForNotification.UserIDs = PRNMasterModel[0].KAMID.ToString();
                                        approvalsModelForNotification.UserIDs += "," + PRNMasterModel[0].UserID.ToString();
                                        srvSendEmail.GenerateEmailAndSendNotification(approvalsModelForNotification, model);
                                        break;

                                    case EnumApprovalProcess.PRN_C:
                                        List<PRNMasterModel> PRNMasterModelc = srvPRNMaster.GetPRNMasterForApproval(model.FormID);
                                        ApprovalProcessModel ApprovalProcessModelC = _srvApprovalProcess.GetByProcessKey(EnumApprovalProcess.INV_C);
                                        //string IDs= PRNMasterModelc[0].KAMID.ToString();
                                        // IDs+= PRNMasterModelc[0].UserID.ToString();
                                        //srvSendEmail.GenerateEmailAndSendNotification(
                                        //                          srvApproval.FetchApproval(new ApprovalModel() { ProcessKey = EnumApprovalProcess.INV_C, Step = 1,UserIDs= IDs, CustomComments = "Final Approved .PRN for the month of (" + PRNMasterModelc[0].Month + ") has been approved for Class Code (" + PRNMasterModelc[0].ClassCode + ")" }).FirstOrDefault()
                                        //                        , new ApprovalHistoryModel() { ApprovalStatusID = (int)EnumApprovalStatus.Pending }
                                        //                        );
                                        var ApprovalC = srvApproval.FetchApproval(new ApprovalModel() { ProcessKey = EnumApprovalProcess.PRN_C, Step = 1 }).FirstOrDefault();
                                        if (ApprovalC != null)
                                        {
                                            ApprovalC.UserIDs = PRNMasterModelc[0].KAMID.ToString();
                                            ApprovalC.UserIDs += "," + PRNMasterModelc[0].UserID.ToString();
                                            ApprovalC.isUserMapping = true;
                                            ApprovalC.CustomComments = "Final Approved .PRN for the month of (" + PRNMasterModelc[0].Month?.ToString("MM/yyyy") + ") has been approved. Class Code: (" + PRNMasterModelc[0].ClassCode + "), Scheme Name: (" + PRNMasterModelc[0].SchemeName + ") , TSP Name: (" + PRNMasterModelc[0].TSPName + ") ,  Invoice type: (" + ApprovalProcessModelC.ApprovalProcessName + ")";
                                            srvSendEmail.GenerateEmailAndSendNotification(ApprovalC, new ApprovalHistoryModel() { ApprovalStatusID = (int)EnumApprovalStatus.Pending });
                                        }
                                        break;

                                    case EnumApprovalProcess.PRN_F:
                                        List<PRNMasterModel> PRNMasterModelF = srvPRNMaster.GetPRNMasterForApproval(model.FormID);
                                        ApprovalProcessModel ApprovalProcessModelF = _srvApprovalProcess.GetByProcessKey(EnumApprovalProcess.INV_F);
                                        var ApprovalF = srvApproval.FetchApproval(new ApprovalModel() { ProcessKey = EnumApprovalProcess.PRN_F, Step = 1 }).FirstOrDefault();
                                        if (PRNMasterModelF.Count > 0)
                                        {
                                            ApprovalF.UserIDs = PRNMasterModelF[0].KAMID.ToString();
                                            ApprovalF.UserIDs += "," + PRNMasterModelF[0].UserID.ToString();
                                            ApprovalF.isUserMapping = true;
                                            ApprovalF.CustomComments = "Final Approved .PRN for the month of (" + PRNMasterModelF[0].Month?.ToString("MM/yyyy") + ") has been approved. Class Code: (" + PRNMasterModelF[0].ClassCode + "), Scheme Name: (" + PRNMasterModelF[0].SchemeName + ") , TSP Name: (" + PRNMasterModelF[0].TSPName + ") ,  Invoice type: (" + ApprovalProcessModelF.ApprovalProcessName + ")";
                                            srvSendEmail.GenerateEmailAndSendNotification(ApprovalF, new ApprovalHistoryModel() { ApprovalStatusID = (int)EnumApprovalStatus.Pending });
                                        }
                                        break;

                                    case EnumApprovalProcess.PRN_T:
                                        List<PRNMasterModel> PRNMasterModelT = srvPRNMaster.GetPRNMasterForApproval(model.FormID);
                                        //approvalsModelForNotification.isUserMapping = true;
                                        //approvalsModelForNotification.ProcessKey = model.ProcessKey;
                                        //if (PRNMasterModelT.Count > 0)
                                        //{
                                        //    approvalsModelForNotification.UserIDs += PRNMasterModelT[0].KAMID.ToString();
                                        //    approvalsModelForNotification.UserIDs += "," + PRNMasterModelT[0].UserID.ToString();
                                        //    approvalsModelForNotification.CustomComments = "Final Approved .TRN for the month of (" + PRNMasterModelT[0].Month + ") has been approved. Class Code: (" + PRNMasterModelT[0].ClassCode + "), Scheme Name: (" + PRNMasterModelT[0].SchemeName + ") , TSP Name: (" + PRNMasterModelT[0].TSPName + ")";
                                        //}
                                        //srvSendEmail.GenerateEmailAndSendNotification(approvalsModelForNotification, model);
                                        break;

                                    case EnumApprovalProcess.PO_TRN:
                                        approvalsModelForNotification.isUserMapping = true;
                                        approvalsModelForNotification.ProcessKey = model.ProcessKey;
                                        approvalsModelForNotification.UserIDs = currentApproval.UserIDs;
                                        approvalsModelForNotification.CustomComments = "Final Approved";
                                        srvSendEmail.GenerateEmailAndSendNotification(approvalsModelForNotification, model);
                                        break;

                                    case EnumApprovalProcess.PO_SRN:
                                        srvSendEmail.GenerateEmailToApprovers(srvApproval.FetchApproval(new ApprovalModel() { ProcessKey = EnumApprovalProcess.INV_SRN, Step = 1 }).FirstOrDefault(), new ApprovalHistoryModel() { ApprovalStatusID = (int)EnumApprovalStatus.Pending });
                                        break;
                                    case EnumApprovalProcess.PO_GURN:
                                        srvSendEmail.GenerateEmailToApprovers(srvApproval.FetchApproval(new ApprovalModel() { ProcessKey = EnumApprovalProcess.INV_GURN, Step = 1 }).FirstOrDefault(), new ApprovalHistoryModel() { ApprovalStatusID = (int)EnumApprovalStatus.Pending });
                                        break;

                                    case EnumApprovalProcess.PO_TSP:
                                        List<POHeaderModel> POHeaderModel = srvPurchaseOrder.GetPOHeaderByID(model.FormID);
                                        if (POHeaderModel.Count > 0)
                                        {
                                            approvalsModelForNotification.isUserMapping = true;
                                            approvalsModelForNotification.ProcessKey = model.ProcessKey;
                                            approvalsModelForNotification.CustomComments = "Final Approved . (Card Name: " + POHeaderModel[0].CardName + ",Document Number" + POHeaderModel[0].DocNum + ")";
                                            approvalsModelForNotification.UserIDs = currentApproval.UserIDs;
                                            approvalsModelForNotification.UserIDs += "," + POHeaderModel[0].CreatedUserID;
                                            srvSendEmail.GenerateEmailAndSendNotification(approvalsModelForNotification, model);
                                        }
                                        break;

                                    case EnumApprovalProcess.INV_C:
                                    case EnumApprovalProcess.INV_SRN:
                                    case EnumApprovalProcess.INV_GURN:
                                    case EnumApprovalProcess.INV_F:
                                        ApprovalProcessModel ApprovalProcessInvoiceF = _srvApprovalProcess.GetByProcessKey(model.ProcessKey);
                                        var invoiceModelSRN = new InvoiceMasterModel { InvoiceHeaderID = model.FormID, ProcessKey = "" };
                                        var invoiceHeaderSRN = srvInvoiceMaster.GetInvoicesForApproval(invoiceModelSRN).First();

                                        TSPDetailModel TSPAndKAMUserBYTSPID = srvTSPDetail.GetUserByTSPID(invoiceHeaderSRN.TSPID);
                                        if (TSPAndKAMUserBYTSPID != null)
                                        {
                                            approvalsModelForNotification.isUserMapping = true;
                                            approvalsModelForNotification.ProcessKey = model.ProcessKey;
                                            approvalsModelForNotification.CustomComments = "Final Approved for the month of (" + invoiceHeaderSRN.U_Month?.ToString("MM/yyyy") + ") has been approved. Scheme Name: (" + invoiceHeaderSRN.SchemeName + ") , TSP Name: (" + invoiceHeaderSRN.TSPName + ") ,  Invoice type: (" + ApprovalProcessInvoiceF.ApprovalProcessName + ") : Draft approved for prepayment verification";
                                            approvalsModelForNotification.UserIDs = currentApproval.UserIDs;
                                            approvalsModelForNotification.UserIDs += "," + TSPAndKAMUserBYTSPID.UserID;
                                            approvalsModelForNotification.UserIDs += "," + TSPAndKAMUserBYTSPID.KAMID;
                                            srvSendEmail.GenerateEmailAndSendNotification(approvalsModelForNotification, model);
                                        }
                                        break;

                                    case EnumApprovalProcess.INV_TRN:
                                        ApprovalProcessModel ApprovalProcessInvoiceTRN = _srvApprovalProcess.GetByProcessKey(model.ProcessKey);

                                        var invoiceModelTRN = new InvoiceMasterModel { InvoiceHeaderID = model.FormID, ProcessKey = "" };
                                        var invoiceHeaderTRN = srvInvoiceMaster.GetInvoicesForApproval(invoiceModelTRN).First();

                                        TSPDetailModel TSPAndKAMUserBYTSPIDTRN = srvTSPDetail.GetUserByTSPID(invoiceHeaderTRN.TSPID);
                                        approvalsModelForNotification.isUserMapping = true;
                                        approvalsModelForNotification.ProcessKey = model.ProcessKey;
                                        approvalsModelForNotification.UserIDs = currentApproval.UserIDs;
                                        approvalsModelForNotification.UserIDs += "," + TSPAndKAMUserBYTSPIDTRN.UserID;
                                        approvalsModelForNotification.UserIDs += "," + TSPAndKAMUserBYTSPIDTRN.KAMID;
                                        approvalsModelForNotification.CustomComments = "Final Approved for the month of(" + invoiceHeaderTRN.U_Month?.ToString("MM/yyyy") + ") has been approved.Scheme Name: (" + invoiceHeaderTRN.SchemeName + "), TSP Name: (" + invoiceHeaderTRN.TSPName + "), Invoice type: (" + ApprovalProcessInvoiceTRN.ApprovalProcessName + ") : Draft approved for prepayment verification";
                                        srvSendEmail.GenerateEmailAndSendNotification(approvalsModelForNotification, model);
                                        break;

                                    case EnumApprovalProcess.INV_R:
                                    case EnumApprovalProcess.INV_1ST:
                                    case EnumApprovalProcess.INV_2ND:
                                        ApprovalProcessModel ApprovalProcessInvoiceR = _srvApprovalProcess.GetByProcessKey(model.ProcessKey);
                                        var invoiceModel = new InvoiceMasterModel { InvoiceHeaderID = model.FormID, ProcessKey = "" };
                                        var invoiceHeader = srvInvoiceMaster.GetInvoicesForApproval(invoiceModel).First();

                                        TSPDetailModel TSPAndKAMUserBYTSPIDR = srvTSPDetail.GetUserByTSPID(invoiceHeader.TSPID);
                                        if (TSPAndKAMUserBYTSPIDR != null)
                                        {
                                            approvalsModelForNotification.UserIDs = currentApproval.UserIDs;
                                            approvalsModelForNotification.isUserMapping = true;
                                            approvalsModelForNotification.UserIDs += "," + TSPAndKAMUserBYTSPIDR.UserID;
                                            approvalsModelForNotification.UserIDs += "," + TSPAndKAMUserBYTSPIDR.KAMID;
                                            approvalsModelForNotification.ProcessKey = model.ProcessKey;
                                            approvalsModelForNotification.CustomComments = "Final Approved for the month of(" + invoiceHeader.U_Month?.ToString("MM/yyyy") + ") has been approved.Scheme Name: (" + invoiceHeader.SchemeName + "), TSP Name: (" + invoiceHeader.TSPName + "), Invoice type: (" + ApprovalProcessInvoiceR.ApprovalProcessName + ") : Draft approved for prepayment verification";
                                            srvSendEmail.GenerateEmailAndSendNotification(approvalsModelForNotification, model);
                                        }
                                        break;

                                    case EnumApprovalProcess.Calcelation:
                                        ClassInvoiceMapModel invMap = srvcancel.GetInvoices(model.FormID);
                                        srvcancel.CancellationApproval(invMap.ClassID, invMap.Month, invMap.InvoiceType, invMap.InvoiceID, true);
                                        approvalsModelForNotification.isUserMapping = true;
                                        approvalsModelForNotification.ProcessKey = model.ProcessKey;
                                        approvalsModelForNotification.CustomComments = "Final Approved";
                                        approvalsModelForNotification.UserIDs = currentApproval.UserIDs;
                                        srvSendEmail.GenerateEmailAndSendNotification(approvalsModelForNotification, model);
                                        break;

                                    default:
                                        break;
                                }
                            }
                            break;

                        case (int)EnumApprovalStatus.SendBack:
                            ///add previous Approval History Record with pending status
                            historyModel.Step = currentApproval.Step - 1;
                            var previousApproval = approvals.Where(x => x.Step == currentApproval.Step - 1).FirstOrDefault();
                            if (model.ProcessKey == EnumApprovalProcess.CR_TSP)
                            {
                                var tspChangeRequest = srvCrTSP.GetByTSPChangeRequestID(model.FormID);
                                previousApproval.CustomComments = " SendBack to you with Remarks. TSP Name:  (" + tspChangeRequest.TSPName + ")";
                            }
                            else if (model.ProcessKey == EnumApprovalProcess.CR_NEW_INSTRUCTOR)
                            {
                                InstructorChangeRequestModel NewInstructorChangeRequest = srvCrInstructor.GetByNewInstructorChangeRequestID_Notification(model.FormID);
                                previousApproval.CustomComments = "SendBack to you with Remarks .(NameOfOrganization: " + NewInstructorChangeRequest.NameOfOrganization + ",InstructorName: " + NewInstructorChangeRequest.InstructorName + ")";
                            }
                            else if (model.ProcessKey == EnumApprovalProcess.CR_INSTRUCTOR_REPLACE)
                            {
                                InstructorReplaceChangeRequestModel InstructorReplaceChangeRequest = srvCrInstructorReplace.GetByInstructorReplaceChangeRequestID(model.FormID);
                                previousApproval.CustomComments = "SendBack to you with Remarks .(" + "Instructor Name: " + InstructorReplaceChangeRequest.InstructorName + ",TSP Name:" + InstructorReplaceChangeRequest.TSPName + ",Scheme Name: " + InstructorReplaceChangeRequest.SchemeName + ") ";
                            }
                            else if (model.ProcessKey == EnumApprovalProcess.CR_INSTRUCTOR)
                            {
                                InstructorChangeRequestModel InstructorChangeRequest = srvCrInstructor.GetByInstructorChangeRequestID_Notification(model.FormID);
                                previousApproval.CustomComments = "SendBack to you with Remarks . (InstructorName: " + InstructorChangeRequest.InstructorName + ")";
                            }
                            else if (model.ProcessKey == EnumApprovalProcess.PO_TSP)
                            {
                                List<POHeaderModel> POHeaderModel = srvPurchaseOrder.GetPOHeaderByID(model.FormID);
                                previousApproval.CustomComments = "SendBack to you with Remarks . (Card Name: " + POHeaderModel[0].CardName + ",Document Number" + POHeaderModel[0].DocNum + ")";
                            }
                            else if (model.ProcessKey == EnumApprovalProcess.AP_BD || model.ProcessKey == EnumApprovalProcess.AP_PD)
                            {
                                SchemeModel scheme = srvScheme.GetBySchemeID_Notification(model.FormID);
                                previousApproval.CustomComments = "SendBack to you with Remarks . (Scheme Name: " + scheme.SchemeName + ")";
                            }
                            else if (model.ProcessKey == EnumApprovalProcess.PRN_C || model.ProcessKey == EnumApprovalProcess.PRN_F || model.ProcessKey == EnumApprovalProcess.PRN_R || model.ProcessKey == EnumApprovalProcess.PRN_T)
                            {
                                List<PRNMasterModel> PRNMasterModel = srvPRNMaster.GetPRNMasterForApproval(model.FormID);
                                previousApproval.CustomComments = "SendBack to you with Remarks .PRN for the month of (" + PRNMasterModel[0].Month + ") and  Class Code (" + PRNMasterModel[0].ClassCode + ")";
                            }
                            else if (model.ProcessKey == EnumApprovalProcess.INV_1ST || model.ProcessKey == EnumApprovalProcess.INV_2ND || model.ProcessKey == EnumApprovalProcess.INV_C || model.ProcessKey == EnumApprovalProcess.INV_F || model.ProcessKey == EnumApprovalProcess.INV_R || model.ProcessKey == EnumApprovalProcess.INV_SRN || model.ProcessKey == EnumApprovalProcess.INV_GURN || model.ProcessKey == EnumApprovalProcess.INV_TRN || model.ProcessKey == EnumApprovalProcess.INV_TRN)
                            {
                                ApprovalProcessModel ApprovalProcessInvoice = _srvApprovalProcess.GetByProcessKey(model.ProcessKey);
                                List<InvoiceMasterModel> invoiceHeaderINV = srvInvoiceMaster.GetInvoicesForApproval(new InvoiceMasterModel() { InvoiceHeaderID = model.FormID, ProcessKey = "" });
                                previousApproval.CustomComments = "SendBack to you with Remarks. for the month of (" + invoiceHeaderINV[0].U_Month?.ToString("MM/yyyy") + ") and Scheme Name: (" + invoiceHeaderINV[0].SchemeName + ") , TSP Name: (" + invoiceHeaderINV[0].TSPName + ") ,  Invoice type: (" + ApprovalProcessInvoice.ApprovalProcessName + ")";

                            }
                            else
                            {
                                previousApproval.CustomComments = "SendBack to you with Remarks . Kindly  login  to your PSDF BSS account to view details and further actions";
                            }
                            srvSendEmail.GenerateEmailAndSendNotification(previousApproval, model);
                            break;

                        case (int)EnumApprovalStatus.Rejected:
                            ///add first Approval History Record with pending status
                            switch (model.ProcessKey)
                            {
                                case EnumApprovalProcess.AP_PD:
                                case EnumApprovalProcess.AP_BD:
                                    ///Rejected scheme is now Editable for created user
                                    SchemeModel scheme = srvScheme.GetBySchemeID_Notification(model.FormID);
                                    if (scheme != null && scheme.CreatedUserID != null && scheme.CreatedUserID != 0)
                                    {
                                        approvalsModelForNotification.UserIDs = scheme.CreatedUserID.ToString();
                                        approvalsModelForNotification.ProcessKey = model.ProcessKey;
                                        approvalsModelForNotification.CustomComments = "Rejected . (Scheme Name: " + scheme.SchemeName + ")";
                                        srvSendEmail.GenerateEmailAndSendNotification(approvalsModelForNotification, model);
                                    }
                                    //End
                                    break;

                                case EnumApprovalProcess.TRD:
                                    TradeModel Trade = srvTrade.GetByTradeID_Notification(model.FormID);
                                    if (Trade != null && Trade.CreatedUserID != null && Trade.CreatedUserID != 0)
                                    {
                                        approvalsModelForNotification.UserIDs = Trade.CreatedUserID.ToString();
                                        approvalsModelForNotification.ProcessKey = model.ProcessKey;
                                        approvalsModelForNotification.CustomComments = "Rejected . Kindly  login  to your PSDF BSS account to view details and further actions";
                                        srvSendEmail.GenerateEmailAndSendNotification(approvalsModelForNotification, model);
                                    }
                                    break;

                                case EnumApprovalProcess.CR_SCHEME:
                                    SchemeChangeRequestModel SchemeChangeRequest = srvCrScheme.GetBySchemeChangeRequestID(model.FormID);
                                    if (SchemeChangeRequest != null && SchemeChangeRequest.CreatedUserID != null && SchemeChangeRequest.CreatedUserID != 0)
                                    {
                                        approvalsModelForNotification.UserIDs = SchemeChangeRequest.CreatedUserID.ToString();
                                        approvalsModelForNotification.ProcessKey = model.ProcessKey;
                                        approvalsModelForNotification.CustomComments = "Rejected . Kindly  login  to your PSDF BSS account to view details and further actions";
                                        srvSendEmail.GenerateEmailAndSendNotification(approvalsModelForNotification, model);
                                    }
                                    break;

                                case EnumApprovalProcess.CR_TSP:
                                    TSPChangeRequestModel TSPChangeRequest = srvCrTSP.GetByTSPChangeRequestID_Notification(model.FormID);
                                    if (TSPChangeRequest != null && TSPChangeRequest.CreatedUserID != null && TSPChangeRequest.CreatedUserID != 0)
                                    {
                                        approvalsModelForNotification.UserIDs = TSPChangeRequest.CreatedUserID.ToString();
                                        approvalsModelForNotification.ProcessKey = model.ProcessKey;
                                        approvalsModelForNotification.CustomComments = "Rejected. TSP Name: (" + TSPChangeRequest.TSPName + ")";
                                        srvSendEmail.GenerateEmailAndSendNotification(approvalsModelForNotification, model);
                                    }
                                    break;

                                case EnumApprovalProcess.CR_CLASS_LOCATION:
                                    ClassChangeRequestModel ClassChangeRequest = srvCrClass.GetByClassChangeRequestID_Notification(model.FormID);
                                    if (ClassChangeRequest != null && ClassChangeRequest.CreatedUserID != null && ClassChangeRequest.CreatedUserID != 0)
                                    {
                                        approvalsModelForNotification.UserIDs = ClassChangeRequest.CreatedUserID.ToString();
                                        approvalsModelForNotification.ProcessKey = model.ProcessKey;
                                        approvalsModelForNotification.CustomComments = "Rejected . Kindly  login  to your PSDF BSS account to view details and further actions";
                                        srvSendEmail.GenerateEmailAndSendNotification(approvalsModelForNotification, model);
                                    }
                                    break;

                                case EnumApprovalProcess.CR_CLASS_DATES:
                                    ClassChangeRequestModel ClassChangeRequestModel = srvCrClass.GetByClassChangeRequestID_Notification(model.FormID);
                                    if (ClassChangeRequestModel != null && ClassChangeRequestModel.CreatedUserID != null && ClassChangeRequestModel.CreatedUserID != 0)
                                    {
                                        approvalsModelForNotification.UserIDs = ClassChangeRequestModel.CreatedUserID.ToString();
                                        approvalsModelForNotification.ProcessKey = model.ProcessKey;
                                        approvalsModelForNotification.CustomComments = "Rejected . Kindly  login  to your PSDF BSS account to view details and further actions";
                                        srvSendEmail.GenerateEmailAndSendNotification(approvalsModelForNotification, model);
                                    }
                                    break;

                                case EnumApprovalProcess.CR_TRAINEE_UNVERIFIED:
                                    TraineeChangeRequestModel TraineeChangeRequestModel = srvCrTrainee.GetByTraineeChangeRequestID_Notification(model.FormID);
                                    if (TraineeChangeRequestModel != null && TraineeChangeRequestModel.CreatedUserID != null && TraineeChangeRequestModel.CreatedUserID != 0)
                                    {
                                        approvalsModelForNotification.UserIDs = TraineeChangeRequestModel.CreatedUserID.ToString();
                                        approvalsModelForNotification.ProcessKey = model.ProcessKey;
                                        approvalsModelForNotification.CustomComments = "Rejected . Kindly  login  to your PSDF BSS account to view details and further actions";
                                        srvSendEmail.GenerateEmailAndSendNotification(approvalsModelForNotification, model);
                                    }
                                    break;

                                case EnumApprovalProcess.CR_TRAINEE_VERIFIED:
                                    TraineeChangeRequestModel TraineeChangeRequestModel2 = srvCrTrainee.GetByTraineeChangeRequestID_Notification(model.FormID);
                                    if (TraineeChangeRequestModel2 != null && TraineeChangeRequestModel2.CreatedUserID != null && TraineeChangeRequestModel2.CreatedUserID != 0)
                                    {
                                        approvalsModelForNotification.UserIDs = TraineeChangeRequestModel2.CreatedUserID.ToString();
                                        approvalsModelForNotification.ProcessKey = model.ProcessKey;
                                        approvalsModelForNotification.CustomComments = "Rejected . Kindly  login  to your PSDF BSS account to view details and further actions";
                                        srvSendEmail.GenerateEmailAndSendNotification(approvalsModelForNotification, model);
                                    }
                                    break;

                                case EnumApprovalProcess.CR_INSTRUCTOR:
                                    InstructorChangeRequestModel InstructorChangeRequest = srvCrInstructor.GetByInstructorChangeRequestID_Notification(model.FormID);
                                    if (InstructorChangeRequest != null && InstructorChangeRequest.CreatedUserID != null && InstructorChangeRequest.CreatedUserID != 0)
                                    {
                                        approvalsModelForNotification.UserIDs = InstructorChangeRequest.CreatedUserID.ToString();
                                        approvalsModelForNotification.ProcessKey = model.ProcessKey;
                                        approvalsModelForNotification.CustomComments = "Rejected . (InstructorName: " + InstructorChangeRequest.InstructorName + ")";
                                        srvSendEmail.GenerateEmailAndSendNotification(approvalsModelForNotification, model);
                                    }
                                    break;

                                case EnumApprovalProcess.CR_NEW_INSTRUCTOR:
                                    InstructorChangeRequestModel NewInstructorChangeRequest = srvCrInstructor.GetByNewInstructorChangeRequestID_Notification(model.FormID);
                                    if (NewInstructorChangeRequest != null && NewInstructorChangeRequest.CreatedUserID != null && NewInstructorChangeRequest.CreatedUserID != 0)
                                    {
                                        approvalsModelForNotification.UserIDs = NewInstructorChangeRequest.CreatedUserID.ToString();
                                        approvalsModelForNotification.ProcessKey = model.ProcessKey;
                                        approvalsModelForNotification.ClassCodeBYIncClassId = NewInstructorChangeRequest.ClassCodeBYIncClassId;


                                        approvalsModelForNotification.CustomComments = "Rejected .(NameOfOrganization: " + NewInstructorChangeRequest.NameOfOrganization + ",InstructorName: " + NewInstructorChangeRequest.InstructorName + ")";
                                        srvSendEmail.GenerateEmailAndSendNotification(approvalsModelForNotification, model);
                                    }
                                    break;

                                case EnumApprovalProcess.CR_INCEPTION:
                                    InceptionReportChangeRequestModel InceptionReportChangeRequest = srvCrInceptionReport.GetByInceptionReportChangeRequestID(model.FormID);
                                    if (InceptionReportChangeRequest != null && InceptionReportChangeRequest.CreatedUserID != null && InceptionReportChangeRequest.CreatedUserID != 0)
                                    {
                                        approvalsModelForNotification.UserIDs = InceptionReportChangeRequest.CreatedUserID.ToString();
                                        approvalsModelForNotification.ProcessKey = model.ProcessKey;
                                        approvalsModelForNotification.CustomComments = "Rejected . Kindly  login  to your PSDF BSS account to view details and further actions";
                                        srvSendEmail.GenerateEmailAndSendNotification(approvalsModelForNotification, model);
                                    }
                                    break;

                                case EnumApprovalProcess.CR_INSTRUCTOR_REPLACE:
                                    InstructorReplaceChangeRequestModel InstructorReplaceChangeRequest = srvCrInstructorReplace.GetByInstructorReplaceChangeRequestID(model.FormID);
                                    if (InstructorReplaceChangeRequest != null && InstructorReplaceChangeRequest.CreatedUserID != null && InstructorReplaceChangeRequest.CreatedUserID != 0)
                                    {
                                        approvalsModelForNotification.UserIDs = InstructorReplaceChangeRequest.CreatedUserID.ToString();
                                        approvalsModelForNotification.ProcessKey = model.ProcessKey;
                                        approvalsModelForNotification.CustomComments = "Rejected .(" + "Instructor Name: " + InstructorReplaceChangeRequest.InstructorName + ",TSP Name:" + InstructorReplaceChangeRequest.TSPName + ",Scheme Name: " + InstructorReplaceChangeRequest.SchemeName + ") ";
                                        srvSendEmail.GenerateEmailAndSendNotification(approvalsModelForNotification, model);
                                    }
                                    break;

                                case EnumApprovalProcess.PRN_R:
                                case EnumApprovalProcess.PRN_C:
                                    List<PRNMasterModel> PRNMasterModel = srvPRNMaster.GetPRNMasterForApproval(model.FormID);
                                    if (PRNMasterModel[0] != null && PRNMasterModel[0].CreatedUserID != null && PRNMasterModel[0].CreatedUserID != 0)
                                    {
                                        approvalsModelForNotification.UserIDs = PRNMasterModel[0].CreatedUserID.ToString();
                                        approvalsModelForNotification.ProcessKey = model.ProcessKey;
                                        approvalsModelForNotification.CustomComments = "Rejected .PRN for the month of (" + PRNMasterModel[0].Month + ") has been approved for Class Code (" + PRNMasterModel[0].ClassCode + ")";
                                        srvSendEmail.GenerateEmailAndSendNotification(approvalsModelForNotification, model);
                                    }
                                    break;

                                case EnumApprovalProcess.PRN_T:
                                    break;

                                case EnumApprovalProcess.PO_TRN:
                                case EnumApprovalProcess.PO_SRN:
                                case EnumApprovalProcess.PO_TSP:
                                    List<POHeaderModel> POHeaderModel = srvPurchaseOrder.GetPOHeaderByID(model.FormID);
                                    if (POHeaderModel != null && POHeaderModel[0].CreatedUserID != null && POHeaderModel[0].CreatedUserID != 0)
                                    {
                                        approvalsModelForNotification.UserIDs = POHeaderModel[0].CreatedUserID.ToString();
                                        approvalsModelForNotification.CustomComments = "Rejected . (Card Name: " + POHeaderModel[0].CardName + ",Document Number" + POHeaderModel[0].DocNum + ")";
                                        approvalsModelForNotification.ProcessKey = model.ProcessKey;
                                        srvSendEmail.GenerateEmailAndSendNotification(approvalsModelForNotification, model);
                                    }
                                    break;

                                case EnumApprovalProcess.INV_C:
                                case EnumApprovalProcess.INV_F:
                                case EnumApprovalProcess.INV_SRN:
                                case EnumApprovalProcess.INV_GURN:
                                    List<InvoiceMasterModel> invoiceHeader = srvInvoiceMaster.GetInvoicesForApproval(new InvoiceMasterModel() { InvoiceHeaderID = model.FormID, ProcessKey = model.ProcessKey });
                                    if (invoiceHeader != null && invoiceHeader[0].CreatedUserID != null && invoiceHeader[0].CreatedUserID != 0)
                                    {
                                        approvalsModelForNotification.UserIDs = invoiceHeader[0].CreatedUserID.ToString();
                                        approvalsModelForNotification.ProcessKey = model.ProcessKey;
                                        approvalsModelForNotification.CustomComments = "Rejected . Kindly  login  to your PSDF BSS account to view details and further actions";
                                        srvSendEmail.GenerateEmailAndSendNotification(approvalsModelForNotification, model);
                                    }
                                    break;

                                case EnumApprovalProcess.Calcelation:
                                    break;

                                default:
                                    break;
                            }
                            break;

                        default:
                            break;
                    }
                }
                else
                {
                    // Fake Approver
                    // result = false;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private bool AU_ApprovalHistory(ApprovalHistoryModel model, SqlTransaction transaction)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@ApprovalHistoryID", model.ApprovalHistoryID));
                param.Add(new SqlParameter("@ProcessKey", model.ProcessKey));
                param.Add(new SqlParameter("@Step", model.Step));
                param.Add(new SqlParameter("@FormID", model.FormID));
                param.Add(new SqlParameter("@ApproverID", model.ApproverID));
                param.Add(new SqlParameter("@ApprovalStatusID", model.ApprovalStatusID));
                param.Add(new SqlParameter("@Comments", model.Comments));
                param.Add(new SqlParameter("@CurUserID", model.CurUserID));
                SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, "[AU_ApprovalHistory]", param.ToArray());
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private bool BeforeApprovalHistory(ApprovalHistoryModel model, ApprovalModel currentApproval, bool isFinalApprover)
        {
            try
            {
                switch (model.ProcessKey)
                {
                    default:
                        break;
                }
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private bool AfterApprovalHistory(ApprovalHistoryModel model, ApprovalModel currentApproval, bool isFinalApprover)
        {
            try
            {
                switch (model.ProcessKey)
                {
                    default:
                        break;
                }
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<ApprovalHistoryModel> NextApproval(ApprovalHistoryModel model)
        {
            try
            {
                var list = _dapper.Query<ApprovalHistoryModel>("dbo.RD_NextAutoProcess", new { @ProcessKey = model.ProcessKey, @FormID = model.FormID }, CommandType.StoredProcedure);
                if (list.Count != 0)
                    return list;
                return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }


        //Getting Trade Target detail
        public List<SchemeTradeMapping> FetchTradeTarget(ApprovalHistoryModel model, SqlTransaction transaction = null)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@SchemeID", model.FormID));

                DataTable dt = new DataTable();
                if (transaction != null)
                {
                    dt = SqlHelper.ExecuteDataset(transaction, CommandType.StoredProcedure, "RD_GetTradeTarget", param.ToArray()).Tables[0];
                }
                else
                {
                    dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_GetTradeTarget", param.ToArray()).Tables[0];
                }
                return LoopinDataTrade(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        private List<SchemeTradeMapping> LoopinDataTrade(DataTable dt)
        {
            List<SchemeTradeMapping> TradeTarget = new List<SchemeTradeMapping>();

            foreach (DataRow r in dt.Rows)
            {
                TradeTarget.Add(RowOfTradeTarget(r));
            }
            return TradeTarget;
        }
        private SchemeTradeMapping RowOfTradeTarget(DataRow row)
        {
            SchemeTradeMapping TradeTarget = new SchemeTradeMapping();

            TradeTarget.SchemeID = row.Field<int>("SchemeID");
            TradeTarget.TradeID = row.Field<int>("TradeID");
            TradeTarget.PTypeID = row.Field<int>("PTypeID");
            TradeTarget.TradeName = row.Field<string>("TradeName");
            TradeTarget.ClusterName = row.Field<string>("ClusterName");
            TradeTarget.ClusterID = row.Field<int>("ClusterID");
            TradeTarget.DistrictName = row.Field<string>("DistrictName");
            TradeTarget.DistrictID = row.Field<int>("DistrictID");
            if (row.Table.Columns.Contains("TradeTarget"))
            {
                int val = row.Field<int>("TradeTarget");
                if (val == 0)
                {
                    TradeTarget.TradeTarget = string.Empty;

                }
                else
                {
                    TradeTarget.TradeTarget = val.ToString();
                }
            }

            return TradeTarget;
        }

        public bool SaveTradeTarget(List<SchemeTradeMapping> model)
        {
            using (SqlConnection connection = new SqlConnection(SqlHelper.GetCon()))
            {
                connection.Open();
                var _transaction = connection.BeginTransaction();
                try
                {

                    foreach (var data in model)
                    {
                        List<SqlParameter> param = new List<SqlParameter>();
                        param.Add(new SqlParameter("@SchemeID", data.SchemeID));
                        param.Add(new SqlParameter("@TradeID", data.TradeID));
                        param.Add(new SqlParameter("@TradeTarget", data.TradeTarget));
                        param.Add(new SqlParameter("@ClusterID", data.ClusterID));
                        param.Add(new SqlParameter("@DistrictID", data.DistrictID));
                        SqlHelper.ExecuteNonQuery(_transaction, CommandType.StoredProcedure, "[Save_TradeTarget]", param.ToArray());
                    }
                    _transaction.Commit();
                    return true;
                }
                catch (Exception)
                {
                    _transaction.Rollback();
                    throw;

                }
            }
        }
        public bool UpdateTradeTarget(List<SchemeTradeMapping> model)
        {
            using (SqlConnection connection = new SqlConnection(SqlHelper.GetCon()))
            {
                connection.Open();
                var _transaction = connection.BeginTransaction();
                try
                {

                    foreach (var data in model)
                    {
                        List<SqlParameter> param = new List<SqlParameter>();
                        param.Add(new SqlParameter("@SchemeID", data.SchemeID));
                        param.Add(new SqlParameter("@TradeID", data.TradeID));
                        param.Add(new SqlParameter("@TradeTarget", data.TradeTarget));
                        param.Add(new SqlParameter("@ClusterID", data.ClusterID));
                        param.Add(new SqlParameter("@DistrictID", data.DistrictID));
                        SqlHelper.ExecuteNonQuery(_transaction, CommandType.StoredProcedure, "[Update_TradeTarget]", param.ToArray());
                    }
                    _transaction.Commit();
                    return true;
                }
                catch (Exception)
                {
                    _transaction.Rollback();
                    throw;

                }
            }
        }

    }
}