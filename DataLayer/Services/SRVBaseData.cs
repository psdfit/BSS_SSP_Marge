using DataLayer.Classes;
using DataLayer.Interfaces;
using DataLayer.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using DataLayer.Models.SSP;
using System.IO;
namespace DataLayer.Services
{
    public class SRVBaseData : ISRVBaseData
    {
        public SRVBaseData()
        {
        }
        public DataTable FetchBankDetailList(int UserID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@UserID", UserID));
            DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_SSPBankDetail", param.ToArray()).Tables[0];
            return dt;
        }
        public DataTable FetchTrainingLocationList(int UserID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@UserID", UserID));
            DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_SSPTSPTrainingLocation", param.ToArray()).Tables[0];

            string[] Attachment = { "FrontMainEntrancePhoto", "ClassroomPhoto", "ComputerLabPhoto", "PracticalAreaPhoto", "ToolsAndEquipmentsPhoto" };
            return LoopinData(dt, Attachment);
        }
        public DataTable FetchCertificationList(int UserID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@UserID", UserID));
            DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_SSPTrainingCertification", param.ToArray()).Tables[0];
            string[] attachmentColumns = { "RegistrationCerEvidence" };
            return LoopinData(dt, attachmentColumns);
        }
        public DataTable FetchTradeMapList(int UserID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@UserID", UserID));
            DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_SSPTSPTradeManage", param.ToArray()).Tables[0];
            return dt;
        }
        public DataTable FetchTrainerDetail(int TrainerID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@TrainerProfileID", TrainerID));
            DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_SSPTrainerProfileDetail", param.ToArray()).Tables[0];
            string[] attachmentColumns = { "RelExpLetter", "ProfQualEvidence" };
            return LoopinData(dt, attachmentColumns);
        }
        public DataTable FetchTrainerProfileList(int UserID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@UserID", UserID));
            DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_SSPTrainerProfile", param.ToArray()).Tables[0];
            string[] attachmentColumns = { "QualEvidence", "CnicFrontPhoto", "CnicBackPhoto", "TrainerCV" };
            return LoopinData(dt, attachmentColumns);
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
            }
        }
        public DataTable SaveBankDetail(BankModel bank)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@UserID", bank.UserID));
            param.Add(new SqlParameter("@BankDetailID", bank.BankDetailID));
            param.Add(new SqlParameter("@BankID", bank.BankName));
            param.Add(new SqlParameter("@OtherBank", bank.OtherBank));
            param.Add(new SqlParameter("@AccountTitle", bank.AccountTitle));
            param.Add(new SqlParameter("@AccountNumber", bank.AccountNumber));
            param.Add(new SqlParameter("@BranchAddress", bank.BranchAddress));
            param.Add(new SqlParameter("@BranchCode", bank.BranchCode));
            DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "AU_SSPBankDetail", param.ToArray()).Tables[0];
            return dt;
        }
        public DataTable SaveCertification(CertificateModel data)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            string RegistrationCerEvidence = SaveAttachment("RegistrationCerEvidence", data.RegistrationCerEvidence, "", "Certificaton");
            param.Add(new SqlParameter("@UserID", data.UserID));
            param.Add(new SqlParameter("@TrainingCertificationID", data.TrainingCertificationID));
            param.Add(new SqlParameter("@TrainingLocationID", data.TrainingLocationID));
            param.Add(new SqlParameter("@RegistrationAuthority", data.RegistrationAuthority));
            param.Add(new SqlParameter("@RegistrationStatus", data.RegistrationStatus));
            param.Add(new SqlParameter("@RegistrationCerNum", data.RegistrationCerNum));
            param.Add(new SqlParameter("@IssuanceDate", data.IssuanceDate));
            param.Add(new SqlParameter("@ExpiryDate", data.ExpiryDate));
            param.Add(new SqlParameter("@RegistrationCerEvidence", RegistrationCerEvidence));
            DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "AU_SSPTrainingCertification", param.ToArray()).Tables[0];
            string[] Attachment = { "RegistrationCerEvidence" };
            return LoopinData(dt, Attachment);
        }
        public DataTable SaveTradeMapping(TradeMapModel data)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@UserID", data.UserID));
            param.Add(new SqlParameter("@TradeManageID", data.TradeManageID));
            param.Add(new SqlParameter("@TrainingLocationID", data.TrainingLocationID));
            param.Add(new SqlParameter("@CertificateID", data.CertificateID));
            param.Add(new SqlParameter("@TradeID", data.TradeID));
            param.Add(new SqlParameter("@TradeAsPerCer", data.TradeAsPerCer));
            param.Add(new SqlParameter("@NoOfClassMor", data.NoOfClassMor));
            param.Add(new SqlParameter("@ClassCapacityMor", data.ClassCapacityMor));
            param.Add(new SqlParameter("@NoOfClassEve", data.NoOfClassEve));
            param.Add(new SqlParameter("@ClassCapacityEve", data.ClassCapacityEve));
            param.Add(new SqlParameter("@TrainingDuration", data.TrainingDuration));
            DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "AU_SSPTSPTradeManage", param.ToArray()).Tables[0];
            return dt;
        }
        public DataTable SaveTrainerProfile(TrainerProfileModel data)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            string CnicFrontPhoto = SaveAttachment("TrainerProfile", data.CnicFrontPhoto, "", "CNIC");
            string CnicBackPhoto = SaveAttachment("TrainerProfile", data.CnicBackPhoto, "", "CNIC");
            string QualEvidence = SaveAttachment("TrainerProfile", data.QualEvidence, "", "Document");
            string TrainerCV = SaveAttachment("TrainerProfile", data.TrainerCV, "", "Document");
            param.Add(new SqlParameter("@UserID", data.UserID));
            param.Add(new SqlParameter("@TrainerID", data.TrainerID));
            param.Add(new SqlParameter("@TrainerName", data.TrainerName));
            param.Add(new SqlParameter("@TrainerMobile", data.TrainerMobile));
            param.Add(new SqlParameter("@TrainerEmail", data.TrainerEmail));
            param.Add(new SqlParameter("@Gender", data.Gender));
            param.Add(new SqlParameter("@TrainerCNIC", data.TrainerCNIC));
            param.Add(new SqlParameter("@Qualification", data.Qualification));
            param.Add(new SqlParameter("@CnicFrontPhoto", CnicFrontPhoto));
            param.Add(new SqlParameter("@CnicBackPhoto", CnicBackPhoto));
            param.Add(new SqlParameter("@QualEvidence", QualEvidence));
            param.Add(new SqlParameter("@TrainerCV", TrainerCV));
            DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "AU_SSPTrainerProfile", param.ToArray()).Tables[0];
            var TrainerProfileID = 0;
            if (data.TrainerID > 0)
            {
                TrainerProfileID = data.TrainerID;
            }
            else
            {
                TrainerProfileID = Convert.ToInt32(dt.Rows[dt.Rows.Count - 1]["TrainerID"]);
            }
            BatchInsert(data.trainerDetails, TrainerProfileID, data.UserID);
            string[] Attachment = { "CnicFrontPhoto", "CnicBackPhoto", "QualEvidence", "TrainerCV" };
            return LoopinData(dt, Attachment);
        }
        public int BatchInsert(List<TrainerProfileDetailModel> ls, int BatchFkey, int CurUserID)
        {
            int rowsAffected = 0;
            foreach (var item in ls)
            {
                string ProfQualEvidence = SaveAttachment("TrainerProfile", item.ProfQualEvidence, "Attached", "Document");
                string RelExpLetter = SaveAttachment("TrainerProfile", item.RelExpLetter, "Attached", "Document");
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@UserID", CurUserID));
                param.Add(new SqlParameter("@TrainerDetailID", item.TrainerDetailID == null ? 0 : item.TrainerDetailID));
                param.Add(new SqlParameter("@TrainerProfileID", BatchFkey));
                param.Add(new SqlParameter("@TrainerTradeID", item.TrainerTradeID));
                param.Add(new SqlParameter("@ProfQualification", item.ProfQualification));
                param.Add(new SqlParameter("@CertificateBody", item.CertificateBody));
                param.Add(new SqlParameter("@ProfQualEvidence", ProfQualEvidence));
                param.Add(new SqlParameter("@RelExpYear", item.RelExpYear));
                param.Add(new SqlParameter("@RelExpLetter", RelExpLetter));
                rowsAffected += SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "AU_SSPTrainerProfileDetail", param.ToArray());
            }
            return rowsAffected;
        }
        public DataTable SaveTrainerProfileDetail(TrainerProfileDetailModel param)
        {
            throw new NotImplementedException();
        }
        public DataTable SaveTrainingLocation(TrainingLocationModel data)
        {
            string FrontMainEntrancePhoto  = SaveAttachment("TrainingLocationPhoto", data.FrontMainEntrancePhoto  ,"", "FrontMainEntrancePhoto");
            string ClassroomPhoto          = SaveAttachment("TrainingLocationPhoto", data.ClassroomPhoto          ,"", "TrainingLocationPhoto");
            string PracticalAreaPhoto      = SaveAttachment("TrainingLocationPhoto", data.PracticalAreaPhoto      ,"", "PracticalAreaPhoto");

            string ComputerLabPhoto        = SaveAttachment("TrainingLocationPhoto", data.ComputerLabPhoto        ,"", "ComputerLabPhoto");

            string ToolsAndEquipmentsPhoto = SaveAttachment("TrainingLocationPhoto", data.ToolsAndEquipmentsPhoto ,"", "ToolsAndEquipmentsPhoto");

            List<SqlParameter> param = new List<SqlParameter>();
             param.Add(new SqlParameter("@UserID", data.UserID));
            param.Add(new SqlParameter("@TrainingLocationID", data.TrainingLocationID));
            param.Add(new SqlParameter("@TrainingLocationName", data.TrainingLocationName));
            param.Add(new SqlParameter("@Province", data.Province));
            param.Add(new SqlParameter("@Cluster", data.Cluster));
            param.Add(new SqlParameter("@District", data.District));
            param.Add(new SqlParameter("@Tehsil", data.Tehsil));
            param.Add(new SqlParameter("@TrainingLocationAddress", data.TrainingLocationAddress));
            param.Add(new SqlParameter("@GeoTagging", data.GeoTagging));
            param.Add(new SqlParameter("@RegistrationAuthority", data.RegistrationAuthority));

            param.Add(new SqlParameter("@FrontMainEntrancePhoto", FrontMainEntrancePhoto));
            param.Add(new SqlParameter("@ClassroomPhoto",ClassroomPhoto));
            param.Add(new SqlParameter("@PracticalAreaPhoto",PracticalAreaPhoto));
            param.Add(new SqlParameter("@ComputerLabPhoto",ComputerLabPhoto));
            param.Add(new SqlParameter("@ToolsAndEquipmentsPhoto",ToolsAndEquipmentsPhoto));
            DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "AU_SSPTspTrainingLocation", param.ToArray()).Tables[0];
            string[] Attachment = { "FrontMainEntrancePhoto", "ClassroomPhoto", "ComputerLabPhoto", "PracticalAreaPhoto", "ToolsAndEquipmentsPhoto" };
            return LoopinData(dt, Attachment);
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
        public DataTable DeleteTrainerDetail(int TrainerDetailID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@TrainerDetailID", TrainerDetailID));
            DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "Delete_TrainerDetail", param.ToArray()).Tables[0];
            return dt;
        }
    }
}