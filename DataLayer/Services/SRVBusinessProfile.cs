using DataLayer.Classes;
using DataLayer.Interfaces;
using DataLayer.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.Caching;
using System.Security.Claims;
using System.Text;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using DataLayer.JobScheduler.Scheduler;
using DataLayer.Models.SSP;
using System.IO;
using System.Dynamic;
using System.Text.Json;
using System.Security.Cryptography.Xml;
namespace DataLayer.Services
{
    public class SRVBusinessProfile : ISRVBusinessProfile
    {
        public SRVBusinessProfile()
        {
        }
        public DataTable FetchProfile(int UserID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@UserID", UserID));
            DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_SSPTSPProfile", param.ToArray()).Tables[0];
            return ProfileLoopinData(dt);
        }
        public DataTable FetchMaster(int UserID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@UserID", UserID));
            DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_SSPTSPMasterByUSerID", param.ToArray()).Tables[0];
            return dt;
        }
        public DataTable FetchTspRegistration()
        {
            List<SqlParameter> param = new List<SqlParameter>();
            DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_SSPTSPRegistrationMaster", param.ToArray()).Tables[0];
            return dt;
        }
        public DataTable FetchTspRegistrationDetail(BusinessProfileModel user)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@UserID", user.UserID));
            param.Add(new SqlParameter("@ApprovalLevel", user.ApprovalLevel));
            DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_SSPTSPRegistrationDetail", param.ToArray()).Tables[0];
            return dt;
        }
        public DataTable UpdateTspTradeValidationStatus(TradeMapModel data)
        {
            string[] tradeIDs = data.tradeManageIds.Split(',');
            foreach (string item in tradeIDs)
            {
                int tradeManageID = Convert.ToInt32(item);
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@UserID", data.UserID));
                param.Add(new SqlParameter("@TradeManageID", tradeManageID));
                param.Add(new SqlParameter("@ProcurementRemarks", data.Remarks));
                param.Add(new SqlParameter("@StatusID", data.Status));
                param.Add(new SqlParameter("@ApprovalLevel", data.ApprovalLevel));
                SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "AU_SSPTSPTradeValidationStatus", param.ToArray());
                SendNotification(data);
            }
            return FetchTspRegistrationDetail(new BusinessProfileModel { UserID = 0, ApprovalLevel = data.ApprovalLevel });
        }

        static void SendNotification(TradeMapModel data)
        {
            string subject = "Trade Evaluation Notification";
            string body = $@"<!DOCTYPE html>
<html>
    <head> </head>
    <body>
    <p>Dear{data.TSPName},</p>
    <p>Trade ({data.TradeName}) has been {data.StatusName}.</p>
    <p><strong>Remarks:</strong> {data.Remarks}</p>
    <p>Best regards,<br />Punjab Skills Development Fund</p>
    </body>
</html>";

            var emailNotifiction = new UserNotificationMapModel
            {
                Subject = subject,
                CustomComments = body,
                UserID = data.TSPID,
                CurUserID = data.CurUserID
            };

            int NotificationId = SRVNotificationDetail.saveSSPSendNotification(emailNotifiction, data.CurUserID);
        }

        public DataTable ProfileLoopinData(DataTable dt)
        {
            DataTable modifiedDataTable = dt.Clone();
            foreach (DataRow row in dt.Rows)
            {
                UpdateAttachment(row, "NTNAttachment");
                UpdateAttachment(row, "PRAAttachment");
                UpdateAttachment(row, "GSTAttachment");
                UpdateAttachment(row, "LegalStatusAttachment");
                UpdateAttachment(row, "HeadCnicFrontImg");
                UpdateAttachment(row, "HeadCnicBackImg");
                modifiedDataTable.ImportRow(row);
            }
            return modifiedDataTable;
        }
        public DataTable SaveProfile(BusinessProfileModel user)
        {
            string tspNTNEvidence = SaveAttachment("NTNEvidence", user.NTNAttachment, user.InstituteName, user.InstituteNTN);
            string tspGSTEvidence = SaveAttachment("GSTEvidence", user.GSTAttachment, user.InstituteName, user.InstituteNTN);
            string tspPRAEvidence = SaveAttachment("PRAEvidence", user.PRAAttachment, user.InstituteName, user.InstituteNTN);
            string tspLegalStatusEvidence = SaveAttachment("LegalStatusEvidence", user.LegalStatusAttachment, user.InstituteName, user.InstituteNTN);
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@UserID", user.TspID));
            param.Add(new SqlParameter("@BusinessName", user.InstituteName));
            param.Add(new SqlParameter("@RegistrationDate", user.RegistrationDate));
            param.Add(new SqlParameter("@NTN", user.InstituteNTN));
            param.Add(new SqlParameter("@NTNEvidence", tspNTNEvidence));
            param.Add(new SqlParameter("@SalesTaxType", user.TaxTypeID));
            param.Add(new SqlParameter("@GST", user.GSTNumber));
            param.Add(new SqlParameter("@GSTEvidence", tspGSTEvidence));
            param.Add(new SqlParameter("@PRA", user.PRANumber));
            param.Add(new SqlParameter("@PRAEvidence", tspPRAEvidence));
            param.Add(new SqlParameter("@LegalStatusID", user.LegalStatus));
            param.Add(new SqlParameter("@LegalStatusEvidence", tspLegalStatusEvidence));
            param.Add(new SqlParameter("@ProvinceID", user.Province));
            param.Add(new SqlParameter("@ClusterID", user.Cluster));
            param.Add(new SqlParameter("@DistrictID", user.District));
            param.Add(new SqlParameter("@TehsilID", user.Tehsil));
            param.Add(new SqlParameter("@GeoTagging", user.LatitudeAndLongitude));
            param.Add(new SqlParameter("@Address", user.HeadOfficeAddress));
            param.Add(new SqlParameter("@ProgramTypeID", user.BusinessType));
            param.Add(new SqlParameter("@Website", user.Website));
            DataTable dt = SqlHelper.ExecuteDataset(
                SqlHelper.GetCon(),
                CommandType.StoredProcedure, "AU_SSPTSPInfo",
                param.ToArray()).Tables[0];
            return ProfileLoopinData(dt);
        }
        public DataTable SaveContactPersonProfile(BusinessProfileModel user)
        {
            string HeadofOrgCNICFrontPhoto = SaveAttachment("HeadOrgCNICDoc", user.HeadofOrgCNICFrontPhoto, user.InstituteName, user.InstituteNTN);
            string HeadofOrgCNICBackPhoto = SaveAttachment("HeadOrgCNICDoc", user.HeadofOrgCNICBackPhoto, user.InstituteName, user.InstituteNTN);
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@UserID", user.TspID));
            param.Add(new SqlParameter("@HeadName", user.HeadofOrgName));
            param.Add(new SqlParameter("@HeadDesignation", user.HeadofOrgDesi));
            param.Add(new SqlParameter("@HeadCnicNum", user.CNICofHeadofOrg));
            param.Add(new SqlParameter("@HeadCnicFrontImg", HeadofOrgCNICFrontPhoto));
            param.Add(new SqlParameter("@HeadCnicBackImg", HeadofOrgCNICBackPhoto));
            param.Add(new SqlParameter("@HeadEmail", user.HeadofOrgEmail));
            param.Add(new SqlParameter("@HeadMobile", user.HeadofOrgMobile));
            param.Add(new SqlParameter("@OrgLandline", user.ORGLandline));
            param.Add(new SqlParameter("@POCName", user.POCName));
            param.Add(new SqlParameter("@POCDesignation", user.POCDesignation));
            param.Add(new SqlParameter("@POCEmail", user.POCEmail));
            param.Add(new SqlParameter("@POCMobile", user.POCMobile));
            DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "AU_SSPContactPerson", param.ToArray()).Tables[0];
            return ProfileLoopinData(dt);
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
            }
        }
        private static string SaveAttachment(string fileType, string attachment, string instituteName, string instituteNTN)
        {
            if (!string.IsNullOrEmpty(attachment))
            {
                string path = FilePaths.TSP_FILE_DIR + fileType + "\\" + instituteName + "_" + instituteNTN;
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string paths = path + "\\";
                return Common.AddFile(attachment, paths);
                //return path+Common.AddFile(attachment, Path.Combine(path, "\\"));
            }
            return "";
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

        public DataTable FetchData(dynamic data, string SpName)
        {
            try
            {

                dynamic DataObject = JsonConvert.DeserializeObject<dynamic>(data.ToString());
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@StartDate", DataObject.StartDate.ToString()));
                param.Add(new SqlParameter("@EndDate", DataObject.EndDate.ToString()));

                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, SpName, param.ToArray()).Tables[0];
                return dt; ;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}