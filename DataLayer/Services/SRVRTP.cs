using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using DataLayer.Classes;
using DataLayer.Models;
using Newtonsoft.Json;
using DataLayer.Interfaces;
using System.Drawing;
using System.IO;
using System.Linq;

namespace DataLayer.Services
{
    public class SRVRTP : SRVBase, ISRVRTP
    {
        private readonly ISRVSendEmail srvSendEmail;
         private readonly ISRVUsers srvUsers;
         private readonly ISRVTSPMaster srvTSPMaster;
        public SRVRTP(ISRVSendEmail srvSendEmail, ISRVTSPMaster srvTSPMaster, ISRVUsers srvUsers) 
        {
            this.srvSendEmail = srvSendEmail; 
            this.srvUsers = srvUsers;
            this.srvTSPMaster = srvTSPMaster;
        }
        //public const string SignatureImageDirectory = @"c:\inetpub\wwwroot\BssAMSApi\Uploads\centre";
        public const string SignatureImageDirectory = @"C:\amsdata\centre";
        public List<RTPModel> GetByRTPID(int RTPID)
        {
            try
            {
                SqlParameter param = new SqlParameter("@RTPID", RTPID);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_RTP", param).Tables[0];
                return LoopinData(dt);
                //if (dt.Rows.Count > 0)
                //{
                //    return RowOfRTP(dt.Rows[0]);
                //}
                //else
                //    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public void SaveRTP(RTPModel R)
        {
            try
            {

                SqlParameter[] param = new SqlParameter[15];
                param[0] = new SqlParameter("@RTPID", R.RTPID);
                param[1] = new SqlParameter("@RTPValue", R.RTPValue);
                param[2] = new SqlParameter("@ClassID", R.ClassID);
                param[3] = new SqlParameter("@ClassCode", R.ClassCode);
                param[4] = new SqlParameter("@AddressOfTrainingLocation", R.AddressOfTrainingLocation);
                param[5] = new SqlParameter("@Comments", R.Comments);
                param[6] = new SqlParameter("@IsApproved", R.IsApproved);
                param[7] = new SqlParameter("@IsRejected", R.IsRejected);
                param[8] = new SqlParameter("@NTP", R.NTP);
                param[9] = new SqlParameter("@CenterInspection", R.CenterInspection);
                param[10] = new SqlParameter("@DistrictID", R.DistrictID);
                param[11] = new SqlParameter("@DistrictName", R.DistrictName);
                param[12] = new SqlParameter("@TehsilID", R.TehsilID);
                param[13] = new SqlParameter("@TehsilName", R.TehsilName);
                param[14] = new SqlParameter("@CurUserID", R.CurUserID);
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_RTP]", param);

                GetKAMUserByClassID(R.ClassID, R.ClassCode, R.CurUserID);

                //return FetchRTP();
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }

        public void GetKAMUserByClassID(int ClassID,string ClassCode, int CurUserID)
        {
            try
            {
                List<TSPMasterModel> model = new List<TSPMasterModel>();
                SqlParameter[] param = new SqlParameter[3];

                param[0] = new SqlParameter("@ClassID",ClassID);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_GetKAMUserByClassID", param).Tables[0];
                model = Helper.ConvertDataTableToModel<TSPMasterModel>(dt);

                ApprovalModel approvalModel = new ApprovalModel();
                ApprovalHistoryModel approvalHistoryModel = new ApprovalHistoryModel();
                approvalModel.ProcessKey = EnumApprovalProcess.RTP;
                approvalModel.CurUserID = Convert.ToInt32(CurUserID);
                approvalModel.UserIDs = model[0].UserID.ToString()+","+ CurUserID;
                approvalModel.CustomComments = "RTP against class code (" + ClassCode + ") has been submitted";
                srvSendEmail.GenerateEmailAndSendNotification(approvalModel, approvalHistoryModel);

            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }

        public void ApproveRTPRequest(RTPModel R)
        {
            try
            {
               
                SqlParameter[] param = new SqlParameter[5];

                param[0] = new SqlParameter("@ClassID", R.ClassID);
                param[1] = new SqlParameter("@Comments", R.Comments);
                param[2] = new SqlParameter("@IsApproved", R.IsApproved);
                param[3] = new SqlParameter("@IsRejected", R.IsRejected);

                param[4] = new SqlParameter("@CurUserID", R.CurUserID);
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_RTP_Approved]", param);
                SenedNotificationApprovedAndRejectRTP(R);

                //return FetchRTP();
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }

        public void SenedNotificationApprovedAndRejectRTP(RTPModel R)
        {
            try
            {
                //Send notification to KAM ,TSP , TPM
                ApprovalModel approvalModel = new ApprovalModel();
                ApprovalHistoryModel approvalHistoryModel = new ApprovalHistoryModel();
                UsersModel usermodal = new UsersModel();
                //Get TSP User for notification
                TSPMasterModel tspmodel = srvTSPMaster.GetTSPUserByClassID(R.ClassID);
                //Get KAM User for notification
                TSPMasterModel KAMmodel = srvTSPMaster.GetKAMUserByClassID(R.ClassID);
                if (R.IsRejected == false && R.IsApproved == true)
                {
                    usermodal.UserLevel = 3;
                    List<UsersModel> users = srvUsers.FetchUsers(usermodal);
                    foreach (var item in users)
                    {
                        approvalModel.UserIDs += item.UserID.ToString() + ",";
                    }
                    approvalModel.UserIDs+= tspmodel.CreatedUserID.ToString()+","+ KAMmodel.UserID.ToString();
                    approvalModel.ProcessKey = EnumApprovalProcess.RTP;
                    approvalModel.CurUserID = Convert.ToInt32(R.CurUserID);
                    approvalModel.CustomComments = " RTP for class code (" + R.ClassCode + ") has been approved";
                    srvSendEmail.GenerateEmailAndSendNotification(approvalModel, approvalHistoryModel);
                }
                else if (R.IsRejected == true && R.IsApproved == false)
                {
                    approvalModel.ProcessKey = EnumApprovalProcess.RTP;
                    approvalModel.CurUserID = Convert.ToInt32(R.CurUserID);
                    approvalModel.UserIDs = tspmodel.CreatedUserID.ToString() + "," + KAMmodel.UserID.ToString();
                    approvalModel.CustomComments = " RTP for class code (" + R.ClassCode + ") has been Rejected";
                    srvSendEmail.GenerateEmailAndSendNotification(approvalModel, approvalHistoryModel);
                }
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }

        private List<RTPModel> LoopinData(DataTable dt)
        {
            List<RTPModel> RTPL = new List<RTPModel>();

            foreach (DataRow r in dt.Rows)
            {
                RTPL.Add(RowOfRTP(r));

            }
            return RTPL;
        }
        private List<RTPModel> LoopinRTPClassData(DataTable dt)
        {
            List<RTPModel> RTPL = new List<RTPModel>();

            foreach (DataRow r in dt.Rows)
            {
                RTPL.Add(RowOfRTPClassData(r));

            }
            return RTPL;
        }
        private List<RTPModel> LoopinNTPData(DataTable dt)
        {
            List<RTPModel> RTPL = new List<RTPModel>();

            foreach (DataRow r in dt.Rows)
            {
                RTPL.Add(RowOfRTP(r));

            }
            return RTPL;
        }
        private List<CenterInspectionModel> LoopinCenterInspectionData(DataTable dt)
        {
            List<CenterInspectionModel> RTPL = new List<CenterInspectionModel>();

            foreach (DataRow r in dt.Rows)
            {
                RTPL.Add(RowOfCenterInspection(r));

            }
            return RTPL;
        }
        private List<CenterInspectionModel> LoopinCenterInspectionDataRequest(DataTable dt)
        {
            List<CenterInspectionModel> RTPL = new List<CenterInspectionModel>();

            foreach (DataRow r in dt.Rows)
            {
                RTPL.Add(RowOfCenterInspectionData(r));

            }
            return RTPL;
        }
        private List<CenterInspectionModel> LoopinCenterInspectionDataRequestSecurity(DataTable dt)
        {
            List<CenterInspectionModel> RTPL = new List<CenterInspectionModel>();

            foreach (DataRow r in dt.Rows)
            {
                RTPL.Add(RowOfCenterInspectionDataSecurity(r));

            }
            return RTPL;
        }

        private List<CenterInspectionComplianceModel> LoopinCenterInspectionAdditionalCompliance(DataTable dt)
        {
            List<CenterInspectionComplianceModel> RTPL = new List<CenterInspectionComplianceModel>();

            foreach (DataRow r in dt.Rows)
            {
                RTPL.Add(RowOfCenterInspectionAdditionalCompliance(r));

            }
            return RTPL;
        }

        private List<CenterInspectionTradeDetailModel> LoopinCenterInspectionTradeDetail(DataTable dt)
        {
            List<CenterInspectionTradeDetailModel> RTPL = new List<CenterInspectionTradeDetailModel>();

            foreach (DataRow r in dt.Rows)
            {
                RTPL.Add(RowOfCenterInspectionTradeDetail(r));

            }
            return RTPL;
        }      
        private List<CenterInspectionClassDetailModel> LoopinCenterInspectionClassDetail(DataTable dt)
        {
            List<CenterInspectionClassDetailModel> RTPL = new List<CenterInspectionClassDetailModel>();

            foreach (DataRow r in dt.Rows)
            {
                RTPL.Add(RowOfCenterInspectionClassDetail(r));

            }
            return RTPL;
        }
        private List<CenterInspectionNecessaryFcilitiesModel> LoopinCenterInspectionNecessaryFacilities(DataTable dt)
        {
            List<CenterInspectionNecessaryFcilitiesModel> RTPL = new List<CenterInspectionNecessaryFcilitiesModel>();

            foreach (DataRow r in dt.Rows)
            {
                RTPL.Add(RowOfCenterInspectionNecessaryFacilities(r));

            }
            return RTPL;
        }
        private List<CenterInspectionTradeToolModel> LoopinCenterInspectionyTradeTool(DataTable dt)
        {
            List<CenterInspectionTradeToolModel> RTPL = new List<CenterInspectionTradeToolModel>();

            foreach (DataRow r in dt.Rows)
            {
                RTPL.Add(RowOfCenterInspectionTradeTool(r));

            }
            return RTPL;
        }

        private List<CenterInspectionModel> LoopinCenterInspectionDataRequestIntegrity(DataTable dt)
        {
            List<CenterInspectionModel> RTPL = new List<CenterInspectionModel>();

            foreach (DataRow r in dt.Rows)
            {
                RTPL.Add(RowOfCenterInspectionDataIntegrity(r));

            }
            return RTPL;
        }
        private List<CenterInspectionModel> LoopinCenterInspectionDataRequestIncharge(DataTable dt)
        {
            List<CenterInspectionModel> RTPL = new List<CenterInspectionModel>();

            foreach (DataRow r in dt.Rows)
            {
                RTPL.Add(RowOfCenterInspectionDataIncharge(r));

            }
            return RTPL;
        }
        public List<RTPModel> FetchRTP(RTPModel mod)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_RTP", Common.GetParams(mod)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<RTPModel> FetchRTPByTPM()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_RTP_By_TPM").Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public List<RTPModel> FetchRTPByTSP(NTPByUserModel ntp)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@UserID", ntp.UserID));
                param.Add(new SqlParameter("@SchemeID", ntp.SchemeID));
                param.Add(new SqlParameter("@TSPID", ntp.TSPID));
                param.Add(new SqlParameter("@ClassID", ntp.ClassID));
                param.Add(new SqlParameter("@StatusID", ntp.StatusID));
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_RTP_By_TSP", Common.GetParams(ntp)).Tables[0];


                //DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_RTP_By_TSP", new SqlParameter("@UserID", userid), new SqlParameter("@SchemeID", schemeid), new SqlParameter("@TSPID", tspid), new SqlParameter("@ClassID", classid)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public List<RTPModel> FetchRTP()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_RTP").Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public List<RTPModel> FetchRTPByKAM(int userid,int OID =0)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@UserID", userid));
                param.Add(new SqlParameter("@OID", userid));
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_RTPs").Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        } 
        public List<RTPModel> FetchRTPByKAMUser(RTPByKAMModel rtp)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@UserID",rtp.UserID));
                param.Add(new SqlParameter("@OID",rtp.OID));
                param.Add(new SqlParameter("@SchemeID",rtp.SchemeID));
                param.Add(new SqlParameter("@TSPID",rtp.TSPID));
                param.Add(new SqlParameter("@ClassID",rtp.ClassID));
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_RTPs", Common.GetParams(rtp)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<CenterInspectionModel> GetCenterInspection(int classid)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_RTP_Center_Inspection", new SqlParameter("@ClassID", classid)).Tables[0];
                return LoopinCenterInspectionData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<CenterInspectionModel> GetCenterInspectionData(int classid)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_RTP_Center_Inspection_Data", new SqlParameter("@ClassID", classid)).Tables[0];
                return LoopinCenterInspectionDataRequest(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<CenterInspectionModel> GetCenterInspectionDataSecurity(int classid)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_RTP_Center_Inspection_Data", new SqlParameter("@ClassID", classid)).Tables[0];
                return LoopinCenterInspectionDataRequestSecurity(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<CenterInspectionComplianceModel> GetCenterInspectionAdditionalCompliance(int classid)
        {
            try{
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_RTP_Center_Inspection_AdditionalData", new SqlParameter("@ClassID", classid)).Tables[0];
                return LoopinCenterInspectionAdditionalCompliance(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }


        public List<CenterInspectionTradeDetailModel> GetCenterInspectionTradeDetail(int classid)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_RTP_Center_Inspection_TradeDetail", new SqlParameter("@ClassID", classid)).Tables[0];
                return LoopinCenterInspectionTradeDetail(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<CenterInspectionClassDetailModel> GetCenterInspectionClassDetail(int classid)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_RTP_Center_Inspection_ClassDetail", new SqlParameter("@ClassID", classid)).Tables[0];
                return LoopinCenterInspectionClassDetail(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<CenterInspectionNecessaryFcilitiesModel> GetCenterInspectionNecessaryFacilities(int classid)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_RTP_Center_Inspection_NecessaryFacilities", new SqlParameter("@ClassID", classid)).Tables[0];
                return LoopinCenterInspectionNecessaryFacilities(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<CenterInspectionTradeToolModel> GetCenterInspectionTradeTools(int classid)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_RTP_Center_Inspection_TradeTools", new SqlParameter("@ClassID", classid)).Tables[0];
                return LoopinCenterInspectionyTradeTool(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<CenterInspectionModel> GetCenterInspectionDataIntegrity(int classid)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_RTP_Center_Inspection_Data", new SqlParameter("@ClassID", classid)).Tables[0];
                return LoopinCenterInspectionDataRequestIntegrity(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<CenterInspectionModel> GetCenterInspectionDataIncharge(int classid)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_RTP_Center_Inspection_Data", new SqlParameter("@ClassID", classid)).Tables[0];
                return LoopinCenterInspectionDataRequestIncharge(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public void UpdateNTP(RTPModel R)
        {
            try
            {
                ApprovalModel approvalModel = new ApprovalModel();
                ApprovalHistoryModel approvalHistoryModel = new ApprovalHistoryModel();
                List<TSPMasterModel> model = new List<TSPMasterModel>();
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@RTPID", R.RTPID);
                param[1] = new SqlParameter("@ClassID", R.ClassID);

                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "Update_RTPs_NTP", param);

                TSPMasterModel tspmodel = srvTSPMaster.GetTSPUserByClassID(R.ClassID);
                approvalModel.ProcessKey = EnumApprovalProcess.NTP;
                approvalModel.CurUserID = Convert.ToInt32(R.CurUserID);
                approvalModel.UserIDs = tspmodel.CreatedUserID.ToString();
                approvalModel.CustomComments = " NTP for Class Code (" + R.ClassCode + ") has been issued";
                srvSendEmail.GenerateEmailAndSendNotification(approvalModel, approvalHistoryModel);


                //DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "Update_RTPs_NTP", new SqlParameter("@RTPID", rtpid)).Tables[0];
                //return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public void UpdateCenterInspection(int classid)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "Update_RTPs_CenterInspection", new SqlParameter("@ClassID", classid));

            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<RTPModel> FetchRTP(bool InActive)

        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_RTP", new SqlParameter("@InActive", InActive)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public List<RTPModel> GetByClassID(int ClassID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_RTP_ClassData", new SqlParameter("@ClassID", ClassID)).Tables[0];
                return LoopinRTPClassData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public void ActiveInActive(int RTPID, bool? InActive, int CurUserID)
        {

            SqlParameter[] PLead = new SqlParameter[3];
            PLead[0] = new SqlParameter("@RTPID", RTPID);
            PLead[1] = new SqlParameter("@InActive", InActive);
            PLead[2] = new SqlParameter("@CurUserID", CurUserID);
            SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AI_RTP]", PLead);
        }
        private RTPModel RowOfRTP(DataRow r)
        {
            RTPModel RTP = new RTPModel();
            RTP.RTPID = Convert.ToInt32(r["RTPID"]);
            //RTP.RTPValue = Convert.ToBoolean(r["RTPValue"]);
            RTP.ClassID = Convert.ToInt32(r["ClassID"]);
            //RTP.TradeName = r["TradeName"].ToString();
            RTP.ClassCode = r["ClassCode"].ToString();
            RTP.SchemeName = r["SchemeName"].ToString();
            RTP.TSPName = r["TSPName"].ToString();
            RTP.AddressOfTrainingLocation = Convert.ToString(string.IsNullOrEmpty(r["AddressOfTrainingLocation"].ToString()) ? "" : r["AddressOfTrainingLocation"]);
            //RTP.AddressOfTrainingLocation = r["AddressOfTrainingLocation"].ToString();
            RTP.TradeName = r["TradeName"].ToString();
            RTP.TraineesPerClass = Convert.ToInt32(r["TraineesPerClass"]);
            RTP.Duration = Convert.ToDouble(r["Duration"]);
            RTP.Comments = r["Comments"].ToString();
            RTP.IsApproved = Convert.ToBoolean(r["IsApproved"]);
            RTP.IsRejected = Convert.ToBoolean(r["IsApproved"]);
            RTP.NTP = Convert.ToBoolean(r["NTP"]);
            RTP.CenterInspection = Convert.ToBoolean(r["CenterInspection"]);
            RTP.CenterInspectionValue = r["CenterInspectionValue"].ToString();
            RTP.DistrictName = r["DistrictName"].ToString();
            RTP.TehsilName = r["TehsilName"].ToString();
            RTP.CPName = r["CPName"].ToString();
            RTP.CPLandline = r["CPLandline"].ToString();
            RTP.Name = r["Name"].ToString();
            RTP.TehsilName = r["TehsilName"].ToString();
            RTP.InActive = Convert.ToBoolean(r["InActive"]);
            RTP.CreatedUserID = Convert.ToInt32(r["CreatedUserID"]);
            RTP.ModifiedUserID = Convert.ToInt32(r["ModifiedUserID"]);
            RTP.CreatedDate = r["CreatedDate"].ToString().GetDate();
            RTP.ModifiedDate = r["ModifiedDate"].ToString().GetDate();
            RTP.StartDate = r["StartDate"].ToString().GetDate();
            
            if (r.Table.Columns.Contains("KAMID"))    // KAMID is the UserID joined with KAM UserID
            {
                RTP.KAMID = Convert.ToInt32(r["KAMID"]);
            }

            return RTP;
        }
        private RTPModel RowOfRTPClassData(DataRow r)
        {
            RTPModel RTP = new RTPModel();
            //RTP.RTPValue = Convert.ToBoolean(r["RTPValue"]);
            //RTP.TradeName = r["TradeName"].ToString();
            RTP.ClassCode = r["ClassCode"].ToString();
            RTP.SchemeName = r["SchemeName"].ToString();
            RTP.TSPName = r["TSPName"].ToString();
            RTP.AddressOfTrainingLocation = r["AddressOfTrainingLocation"].ToString();
            RTP.TradeName = r["TradeName"].ToString();
            RTP.TraineesPerClass = Convert.ToInt32(r["TraineesPerClass"]);
            RTP.Duration = Convert.ToDouble(r["Duration"]);

            RTP.DistrictName = r["DistrictName"].ToString();
            RTP.TehsilName = r["TehsilName"].ToString();
            RTP.CPName = r["CPName"].ToString();
            RTP.CPLandline = r["CPLandline"].ToString();
            RTP.Name = r["Name"].ToString();
            RTP.TehsilName = r["TehsilName"].ToString();
            RTP.InActive = Convert.ToBoolean(r["InActive"]);
            RTP.CreatedUserID = Convert.ToInt32(r["CreatedUserID"]);
            RTP.ModifiedUserID = Convert.ToInt32(r["ModifiedUserID"]);
            RTP.CreatedDate = r["CreatedDate"].ToString().GetDate();
            RTP.ModifiedDate = r["ModifiedDate"].ToString().GetDate();
            RTP.StartDate = r["StartDate"].ToString().GetDate();
            RTP.EndDate = r["EndDate"].ToString().GetDate();


            return RTP;
        }
        private CenterInspectionModel RowOfCenterInspection(DataRow r)
        {
            CenterInspectionModel CP = new CenterInspectionModel();

            //CP.ClassID = Convert.ToInt32(r["ClassID"]);
            CP.ClassesInspectedCount = Convert.ToInt32(r["ClassesInspectedCount"]);
            //CP.ClassCode = r["ClassCode"].ToString();
            //CP.TradeName = r["TradeName"].ToString();
            CP.TSPName = r["TSPName"].ToString();
            CP.TrainingCentreName = r["TrainingCentreName"].ToString();
            CP.TrainingCentreAddress = r["TrainingCentreAddress"].ToString();
            CP.CentreInchargeName = r["CentreInchargeName"].ToString();
            CP.CentreInchargeMob = r["CentreInchargeMob"].ToString();
            //CP.Parameter = r["Parameter"].ToString();
            //CP.Compliance = r["Compliance"].ToString();
            //CP.ObservatoryRemarks = r["ObservatoryRemarks"].ToString();
            //CP.PSDFCompliance = r["PSDFCompliance"].ToString();
            //CP.RecommendationRemarks = r["RecommendationRemarks"].ToString();

            CP.VisitDateTime = r["VisitDateTime"].ToString().GetDate();
            //CP.ExpectedStartDate = r["ExpectedStartDate"].ToString().GetDate();

            return CP;
        }
        private CenterInspectionModel RowOfCenterInspectionData(DataRow r)
        {
            CenterInspectionModel CP = new CenterInspectionModel();

            CP.LocationAccessSuitablity = r["LocationAccessSuitablity"].ToString();
            CP.LocAccessSuitabilityValue = r["LocAccessSuitabilityValue"].ToString();
            CP.LocAccessSuitabilityObservRemarks = r["LocAccessSuitabilityObservRemarks"].ToString();
            CP.LocAccessSuitabilityRecomRemarks = r["LocAccessSuitabilityRecomRemarks"].ToString();
            //CP.ExpectedStartDate = r["ExpectedStartDate"].ToString().GetDate();

            return CP;
        }

        private CenterInspectionModel RowOfCenterInspectionDataSecurity(DataRow r)
        {
            CenterInspectionModel CP = new CenterInspectionModel();



            CP.SecurityPremises = r["SecurityPremises"].ToString();
            CP.SecurityPremValue = r["SecurityPremValue"].ToString();
            CP.SecurityPremObservRemarks = r["SecurityPremObservRemarks"].ToString();
            CP.SecurityPremRecomRemarks = r["SecurityPremRecomRemarks"].ToString();
            //CP.ExpectedStartDate = r["ExpectedStartDate"].ToString().GetDate();

            return CP;
        }
        private CenterInspectionComplianceModel RowOfCenterInspectionAdditionalCompliance(DataRow r)
        {
            CenterInspectionComplianceModel CP = new CenterInspectionComplianceModel();

            CP.parameter = r["parameter"].ToString();
            CP.psdfStandard = r["psdfStandard"].ToString();
            CP.observatoryRemarks = r["observatoryRemarks"].ToString();
            CP.recommendationRemarks = r["recommendationRemarks"].ToString();
            CP.complaince = r["complaince"].ToString();
            //CP.ExpectedStartDate = r["ExpectedStartDate"].ToString().GetDate();

            return CP;
        }
        private CenterInspectionTradeDetailModel RowOfCenterInspectionTradeDetail(DataRow r)
        {
            CenterInspectionTradeDetailModel CP = new CenterInspectionTradeDetailModel();

            CP.tradeName = r["TradeName"].ToString();
            CP.classesPerBatch = r["ClassesPrBatch"].ToString();
            CP.quantitySufficient = r["SufficientQty"].ToString();
            CP.totalContractrualTraineesPerClass = r["ContrTraineePrClas"].ToString();
            CP.noOfItemsMissing = r["ItemsMissing"].ToString();
            CP.noOfRoomsForLab = r["LabRoomCount"].ToString();
            CP.isSpaceSufficient = r["SufficientSpace"].ToString();
            CP.powerBackupAvailability = r["PowerBackupAval"].ToString();
            return CP;
        }
        private CenterInspectionClassDetailModel RowOfCenterInspectionClassDetail(DataRow r)
        {
            CenterInspectionClassDetailModel CP = new CenterInspectionClassDetailModel();
            CP.ClassCode = r["ClassCode"].ToString();
            CP.TradeName = r["TradeName"].ToString();
            CP.ExpectedStartDate = DateTime.Parse(r["ExpectedStartDate"].ToString()).ToString("dd-MMM-yy");
            CP.BoardAval = r["BoardAval"].ToString();
            CP.SufficientFurniture = r["SufficientFurniture"].ToString();
            CP.LightAval = r["LightAval"].ToString();
            CP.VentFanAval = r["VentFanAval"].ToString();
            CP.ClassSpaceSufficient = r["ClassSpaceSufficient"].ToString();
            return CP;
        }
        private CenterInspectionNecessaryFcilitiesModel RowOfCenterInspectionNecessaryFacilities(DataRow r)
        {
            CenterInspectionNecessaryFcilitiesModel CP = new CenterInspectionNecessaryFcilitiesModel();

            CP.MonitoringID = r["MonitoringID"].ToString();
            CP.StructIntegValue = r["StructIntegValue"].ToString();
            CP.FMSubmissionDateTime = DateTime.Parse(r["FMSubmissionDateTime"].ToString()).ToString("dd-MMM-yy");
            CP.KeyFacBuildingStructInteg = r["KeyFacBuildingStructInteg"].ToString();
            CP.KeyFacMissing = r["KeyFacMissing"].ToString();
            CP.KeyFacElecBackup = r["KeyFacElecBackup"].ToString();
            CP.KeyFacEquipAval = r["KeyFacEquipAval"].ToString();
            CP.KeyFacFurnitureAval = r["KeyFacFurnitureAval"].ToString();
            CP.TspSignatureImgPath = r["TspSignatureImgPath"].ToString();
            CP.ReqID = r["ReqID"].ToString();
            if (!string.IsNullOrEmpty(CP.TspSignatureImgPath))
            {

                CP.TspSignatureImgPath = generateBase64(CP.TspSignatureImgPath, CP.ReqID);
            }
            CP.FMSignatureImgPath = r["FMSignatureImgPath"].ToString();
            if (!string.IsNullOrEmpty(CP.FMSignatureImgPath))
            {

                CP.FMSignatureImgPath = generateBase64(CP.FMSignatureImgPath, CP.ReqID);
            }
            CP.TspRepImgPath = r["TspRepImgPath"].ToString();
            if (!string.IsNullOrEmpty(CP.TspRepImgPath))
            {

                CP.TspRepImgPath = generateBase64(CP.TspRepImgPath, CP.ReqID);
            }
            CP.DistrictInchargeName = r["DistrictInchargeName"].ToString();
            CP.FMName = r["FMName"].ToString();
            CP.SignOffTspName = r["SignOffTspName"].ToString();
            CP.SignOffFmRemarks = r["SignOffFmRemarks"].ToString();
            CP.SignOffTspRemarks = r["SignOffTspRemarks"].ToString();
            return CP;
        }

        private string generateBase64(string tspSignatureImgPath, string monitoringID)
        {

            var fmSignImg = Path.Combine(SignatureImageDirectory, monitoringID, tspSignatureImgPath);
            //var fmSignImg = SignatureImageDirectory+"/"+monitoringID+"/"+tspSignatureImgPath;
            if (File.Exists(fmSignImg))
            {
                byte[] imageArray = System.IO.File.ReadAllBytes(fmSignImg);
                string base64ImageRepresentation = Convert.ToBase64String(imageArray);
                return base64ImageRepresentation;
            }
            return "";
        }

        private CenterInspectionTradeToolModel RowOfCenterInspectionTradeTool(DataRow r)
        {
            CenterInspectionTradeToolModel CP = new CenterInspectionTradeToolModel();

            CP.TradeID = r["TradeID"].ToString();
            CP.CentreMonitoringID = r["CentreMonitoringID"].ToString();
            CP.TradeName = r["TradeName"].ToString();
            CP.TradeDuration = r["TradeDuration"].ToString();
            CP.headCount = r["headCount"].ToString();
            CP.ToolName = r["ToolName"].ToString();
            CP.QuantityTotal = r["QuantityTotal"].ToString();
            CP.QuantityFound = r["QuantityFound"].ToString();
            return CP;
        }


        private CenterInspectionModel RowOfCenterInspectionDataIntegrity(DataRow r)
        {
            CenterInspectionModel CP = new CenterInspectionModel();

            CP.StructuralIntegrityCompliance = r["StructuralIntegrityCompliance"].ToString();
            CP.StructIntegValue = r["StructIntegValue"].ToString();
            CP.StructIntegObservRemarks = r["StructIntegObservRemarks"].ToString();
            CP.StructIntegRecomRemarks = r["StructIntegRecomRemarks"].ToString();
            //CP.ExpectedStartDate = r["ExpectedStartDate"].ToString().GetDate();

            return CP;
        }
        private CenterInspectionModel RowOfCenterInspectionDataIncharge(DataRow r)
        {
            CenterInspectionModel CP = new CenterInspectionModel();

            CP.CentreInchargeRoom = r["CentreInchargeRoom"].ToString();
            CP.CentreInchrgRoomValue = r["CentreInchrgRoomValue"].ToString();
            CP.CentreInchrgRoomRecomRemarks = r["CentreInchrgRoomRecomRemarks"].ToString();
            CP.CentreInchrgRoomObservRemarks = r["CentreInchrgRoomObservRemarks"].ToString();
            //CP.ExpectedStartDate = r["ExpectedStartDate"].ToString().GetDate();

            return CP;
        }

        public bool saveCentreMonitoringClassRecordNotification(List<RTPModel> model, int? CurUserID)
        {
            try
            {

                var ls = SRVRTP.FetchCentreMonitoringClassRecordNotification();
                foreach (var item in model)
                {
                    var list = ls.Where(f => f.ClassCode == item.ClassCode).Select(n => n.ClassCode).ToList();
                    if (list.Count == 0)
                    {
                        SqlParameter[] PLead = new SqlParameter[3];
                        PLead[0] = new SqlParameter("@ClassCode", item.ClassCode);
                        PLead[1] = new SqlParameter("@CurUserID", CurUserID);
                        SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_CentreMonitoringClassRecordNotification]", PLead);
                        TSPMasterModel KAMmodel = srvTSPMaster.GetKAMUserByClassID(item.ClassID);
                        ApprovalModel approvalModel = new ApprovalModel();
                        ApprovalHistoryModel approvalHistoryModel = new ApprovalHistoryModel();
                        approvalModel.CustomComments = "Inspection report ("+item.ClassCode+")  for the Class Code has been submitted ";
                        approvalModel.ProcessKey = EnumApprovalProcess.RTP;
                        approvalModel.UserIDs = KAMmodel.UserID.ToString();
                        srvSendEmail.GenerateEmailAndSendNotification(approvalModel, approvalHistoryModel);
                    }

                }

                return true;
            }
            catch (Exception ex) { throw new Exception(ex.Message); return false; }
        }
        public static List<RTPModel> FetchCentreMonitoringClassRecordNotification()
        {
            try
            {
                List<RTPModel> List = new List<RTPModel>();
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "[RD_CentreMonitoringClassRecordNotification]").Tables[0];
                List = Helper.ConvertDataTableToModel<RTPModel>(dt);
                return List;
            }
            catch (Exception ex) { throw new Exception(ex.Message); return null; }
        }
    }
}
