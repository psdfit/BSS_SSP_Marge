﻿using Microsoft.Data.SqlClient;
using System.Data;
using DataLayer.Models.SSP;
using DataLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using DataLayer.Classes;
using System.Transactions;
using DataLayer.Models;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using Microsoft.AspNetCore.Components.Routing;
using System.Data.Common;
using System.Linq;


namespace DataLayer.Services
{
    public class SRVAssociation : ISRVAssociation
    {
        public DataTable FetchDataListBySPName(string spName)
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

        public DataTable LoopingData(DataTable dt, string[] attachmentColumns)
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

        private static string SaveAttachment(string TSPName, string fileType, string attachment)
        {
            if (!string.IsNullOrEmpty(attachment))
            {
                string path = FilePaths.DOCUMENTS_FILE_DIR + TSPName + "\\" + fileType;
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string paths = path + "\\";
                return Common.AddFile(attachment, paths);
            }
            return "";
        }

        public DataTable SaveAssociationSubmission(AssociationSubmissionModel data)
        {

            SqlConnection connection = new SqlConnection(SqlHelper.GetCon());
            connection.Open();
            var transaction = connection.BeginTransaction();

            try
            {
                List<SqlParameter> param = new List<SqlParameter>();


                param.Add(new SqlParameter("@UserID", data.UserID));
                param.Add(new SqlParameter("@TspAssociationMasterID", data.TspAssociationMasterID));
                param.Add(new SqlParameter("@ProgramDesignID", data.ProgramID));
                param.Add(new SqlParameter("@TrainingLocationID", data.TrainingLocation));
                param.Add(new SqlParameter("@TradeLotID", data.TradeLot));
                param.Add(new SqlParameter("@TrainerDetailID", data.TrainerDetailID));
                param.Add(new SqlParameter("@TradeLotTitle", data.TradeLotTitle));


                DataTable dt = SqlHelper.ExecuteDataset(transaction, CommandType.StoredProcedure, "AU_SSPTSPAssociationMaster", param.ToArray()).Tables[0];

                var TspAssociationMasterID = 0;

                if (data.TspAssociationMasterID > 0)
                {
                    TspAssociationMasterID = data.TspAssociationMasterID;
                }
                else
                {
                    TspAssociationMasterID = Convert.ToInt32(dt.Rows[0]["TspAssociationMasterID"]);
                }

                BatchInsert(data.associationDetail, TspAssociationMasterID, data.UserID, data.TSPName, transaction);

                transaction.Commit();

                return dt;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw ex;
            }


        }


        public int BatchInsert(List<AssociationDetail> ls, int BatchFkey, int CurUserID, string TSPName, SqlTransaction transaction = null)
        {
            try
            {
                int rowsAffected = 0;
                foreach (var item in ls)
                {
                    string AssociationEvidence = SaveAttachment("TSPName", "AssociationEvidence", item.Evidence);

                    List<SqlParameter> param = new List<SqlParameter>();
                    param.Add(new SqlParameter("@UserID", CurUserID));
                    param.Add(new SqlParameter("@TspAssociationDetailID", item.TspAssociationDetailID));
                    param.Add(new SqlParameter("@TspAssociationMasterID", BatchFkey));
                    param.Add(new SqlParameter("@CriteriaMainCategoryID", item.CriteriaMainCategoryID));
                    param.Add(new SqlParameter("@Attachment", AssociationEvidence));
                    param.Add(new SqlParameter("@Remarks", item.Remarks));

                    rowsAffected += SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, "AU_SSPTSPAssociationDetail", param.ToArray());
                }
                return rowsAffected;
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }

        public DataTable SaveAssociationEvaluation(AssociationEvaluationModel data)
        {
            try
            {

                string searchString = @"\Documents";

                bool containsPath = data.Attachment.Contains(searchString);
                string EvaluationAttachment = "";
                if (!containsPath)
                {
                    EvaluationAttachment = SaveAttachment("EvaluationAttachment", data.Attachment, "", "EvaluationAttachment");
                }
                else
                {
                    EvaluationAttachment = data.Attachment;
                }

                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@UserID", data.UserID));
                param.Add(new SqlParameter("@TspAssociationEvaluationID", data.TspAssociationEvaluationID));
                param.Add(new SqlParameter("@TspAssociationMasterID", data.TspAssociationMasterID));
                param.Add(new SqlParameter("@VerifiedCapacityMorning", data.VerifiedCapacityMorning));
                param.Add(new SqlParameter("@VerifiedCapacityEvening", data.VerifiedCapacityEvening));
                param.Add(new SqlParameter("@TotalCapacity", Convert.ToInt16(data.VerifiedCapacityMorning) + Convert.ToInt16(data.VerifiedCapacityEvening)));
                param.Add(new SqlParameter("@MarksBasedOnEvaluation", data.MarksBasedOnEvaluation));
                param.Add(new SqlParameter("@CategoryBasedOnEvaluation", data.CategoryBasedOnEvaluation));
                param.Add(new SqlParameter("@EvaluationStatus", data.Status));
                param.Add(new SqlParameter("@Remarks", data.Remarks));
                param.Add(new SqlParameter("@Attachment", EvaluationAttachment));

                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "AU_SSPTSPAssociationEvaluation", param.ToArray()).Tables[0];

                int[] _notificationStatuses = new int[] {4,5};
                if (_notificationStatuses.Contains(data.Status))
                {
                    SendNotification(data);
                }
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        static void SendNotification(AssociationEvaluationModel data)
        {
            string subject = "Association Evaluation Notification";
            string body = $@"<!DOCTYPE html>
<html>
  <head> </head>
  <body>
    <p>Dear {data.TspName},</p>
    <p>Trade Lot ({data.TradeLot}) has been {data.StatusName}.</p>
    <p><strong>Remarks:</strong> {data.Remarks}</p>
    <p>Best regards,<br />Punjab Skills Development Fund</p>
  </body>
</html>
";

            var emailNotifiction = new UserNotificationMapModel
            {
                Subject = subject,
                CustomComments = body,
                UserID = data.TSPID,
                CurUserID = data.CurUserID
            };

            int NotificationId = SRVNotificationDetail.saveSSPSendNotification(emailNotifiction, data.CurUserID);
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

        public void SaveTSPAssignment(TSPAssignmentModel data)
        {

            try
            {

                for (global::System.Int32 i = 0; i < data.TspAssociationEvaluationID.Count; i++)
                {
                    List<SqlParameter> param = new List<SqlParameter>();
                    param.Add(new SqlParameter("@UserID", data.UserID));
                    param.Add(new SqlParameter("@TSPAssignmentID", data.TSPAssignmentID));
                    param.Add(new SqlParameter("@ProgramID", data.ProgramID));
                    param.Add(new SqlParameter("@TradeLotID", data.TradeLotID));
                    param.Add(new SqlParameter("@TrainingLocationID", data.TspTrainingLocationID[i]));
                    param.Add(new SqlParameter("@TspAssociationEvaluationID", data.TspAssociationEvaluationID[i]));
                    param.Add(new SqlParameter("@TSPID", data.TSP[i]));
                    SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "AU_SSPTSPAssignment", param.ToArray());
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        public DataTable FetchReportBySPNameAndParam(string spName, string parameter, int value)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@" + parameter, value));
            DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, spName, param.ToArray()).Tables[0];
            return dt;
        }
    }
}
