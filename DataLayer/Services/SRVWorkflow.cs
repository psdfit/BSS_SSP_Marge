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
using System.Net.Mail;
using System.Transactions;
using System.Xml.Linq;
using System.Data.Common;
using OfficeOpenXml.Packaging.Ionic.Zlib;
using System.Threading.Tasks;


namespace DataLayer.Services
{
    public class SRVWorkflow : ISRVWorkflow
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

        public bool RemoveTaskDetail(TaskDetail taskDetail)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@TaskID", taskDetail.TaskID));

            SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "Remove_SSPWorkflowTask", parameters.ToArray());
            return true;
        }

        public DataTable Save(WorkflowModel data)
        {
            List<SqlParameter> param = new List<SqlParameter>();
          
            param.Add(new SqlParameter("@UserID", data.UserID));
            param.Add(new SqlParameter("@WorkflowID", data.@WorkflowID));
            param.Add(new SqlParameter("@WorkflowTitle", data.@WorkflowTitle));
            param.Add(new SqlParameter("@SourcingTypeID", data.@SourcingTypeID));
            param.Add(new SqlParameter("@Description", data.@Description));
            param.Add(new SqlParameter("@TotalDays", data.@TotalDays));
            param.Add(new SqlParameter("@TotalTaskDays", data.@TotalTaskDays));

            DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "AU_SSPWorkflow", param.ToArray()).Tables[0];

            var WorkflowID = 0;

            if (data.WorkflowID > 0)
            {
                WorkflowID = data.WorkflowID;
            }
            else
            {
                WorkflowID = Convert.ToInt32(dt.Rows[dt.Rows.Count - 1]["WorkflowID"]);
            }

            BatchInsert(data.taskDetails, WorkflowID, data.UserID);



            return dt;
        }


        public int BatchInsert(List<TaskDetail> ls, int BatchFkey, int CurUserID)
        {
            int rowsAffected = 0;
            foreach (var item in ls)
            {
              
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@UserID", CurUserID));
                param.Add(new SqlParameter("@TaskID", item.TaskID));
                param.Add(new SqlParameter("@WorkflowID", BatchFkey));
                param.Add(new SqlParameter("@TaskName", item.TaskName));
                param.Add(new SqlParameter("@TaskDays", item.@TaskDays));
                param.Add(new SqlParameter("@TaskApproval", item.TaskApproval));
                param.Add(new SqlParameter("@TaskStatus", item.TaskStatus));

                rowsAffected += SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "AU_SSPWorkflowTask", param.ToArray());
            }
            return rowsAffected;
        }
    }
}
