using System;
using System.Data;
using DataLayer.Interfaces;
using DataLayer.Models.SSP;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using DataLayer.Classes;

namespace DataLayer.Services
{
    public class SRVProcessConfiguration : ISRVProcessConfiguration
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

            SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "Remove_WorkflowTask", parameters.ToArray());
            return true;
        }

        public DataTable Save(ProcessScheduleModel data)
        {
            List<SqlParameter> param = new List<SqlParameter>();

            param.Add(new SqlParameter("@UserID", data.UserID));
            param.Add(new SqlParameter("@ProcessScheduleMasterID", data.ProcessScheduleMasterID));
            param.Add(new SqlParameter("@ProgramID", data.ProgramID));
            param.Add(new SqlParameter("@ProgramStartDate", data.ProgramStartDate));
            param.Add(new SqlParameter("@TotalDays", data.TotalDays));
            param.Add(new SqlParameter("@TotalProcess", data.TotalProcess));
            param.Add(new SqlParameter("@InActive",0));



            DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "AU_SSPProcessScheduleMaster", param.ToArray()).Tables[0];

      

            var ProcessScheduleMasterID = 0;

            if (data.ProcessScheduleMasterID > 0)
            {
                ProcessScheduleMasterID = data.ProcessScheduleMasterID;
            }
            else
            {
                ProcessScheduleMasterID = Convert.ToInt32(dt.Rows[dt.Rows.Count - 1]["ProcessScheduleMasterID"]);
            }

            BatchInsert(data.processDetails, ProcessScheduleMasterID, data.UserID);

            return dt;
        }

        public int BatchInsert(List<ProcessDetail> ls, int BatchFkey, int CurUserID)
        {
            int rowsAffected = 0;
            foreach (var item in ls)
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@UserID", CurUserID));
                param.Add(new SqlParameter("@ProcessScheduleDetailID", item.ProcessScheduleDetailID));
                param.Add(new SqlParameter("@ProcessScheduleMasterID", BatchFkey));
                param.Add(new SqlParameter("@ProcessID", item.ProcessID));
                param.Add(new SqlParameter("@ProcessStartDate", item.ProcessStartDate));
                param.Add(new SqlParameter("@ProcessEndDate", item.ProcessEndDate));
                param.Add(new SqlParameter("@ProcessDays", item.ProcessDays));
                param.Add(new SqlParameter("@InActive", item.IsLocked==0 ? false : true));



                rowsAffected += SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "AU_SSPProcessScheduleDetail", param.ToArray());
            }
            return rowsAffected;
        }
    }
}
