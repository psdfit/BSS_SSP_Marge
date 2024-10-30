using Dapper;
using DataLayer.Classes;
using DataLayer.Interfaces;
using DataLayer.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Xml.Linq;

namespace DataLayer.Services
{
    public class SRVBSSReports : SRVBase, ISRVBSSReports
    {
        public SRVBSSReports()
        { }

        public DataTable FetchReport(string Param)
        {
            try
            {
                var parts = Param.Split('/');
                string SpName = parts[0];
                DataSet ds;

                if (parts.Length > 1)
                {
                    var parameters = new List<SqlParameter>();
                    for (int i = 1; i < parts.Length; i++)
                    {
                        var SepSpKeyVal = parts[i].Split('=');
                        string SpParamName = SepSpKeyVal[0];
                        string SpParamVal = SepSpKeyVal[1];
                        parameters.Add(new SqlParameter(SpParamName, SpParamVal));
                    }
                    ds = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), SpName, parameters.ToArray());
                }
                else
                {
                    ds = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), SpName);
                }

                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching Data " + ex.Message);
            }
        }

        public DataTable FetchDropDownList(string Param)
        {
            try
            {
                var SpDesc = JsonConvert.DeserializeObject<SpDescModel>(Param);
                string SpName = SpDesc.SpName;
                string SpParamValue = SpDesc.SpParamValue;
                var parts = SpParamValue.Split('/');
                SqlParameter[] parameters = new SqlParameter[parts.Length];
                for (int i = 0; i < parts.Length; i++)
                {
                    var SepSpKeyVal = parts[i].Split('=');

                    string SpParamName = SepSpKeyVal[0];
                    string SpParamVal = SepSpKeyVal[1];

                    parameters[i] = new SqlParameter(SpParamName, SpParamVal);
                }

                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), SpName, parameters);

                if (ds.Tables.Count > 0)
                {
                    return ds.Tables[0];
                }
                else
                {
                    throw new Exception("No data found.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error Desc' attendance: " + ex.Message);
            }
        }


        public DataTable FetchList(string Param)
        {
            try
            {
                var parts = Param.Split('/');
                string SpName = parts[0];
                SqlParameter[] parameters = new SqlParameter[parts.Length - 1];
                if (parts.Length > 1)
                {
                    for (int i = 0; i < parts.Length; i++)
                    {
                        if (i > 0)
                        {
                            var SepSpKeyVal = parts[i].Split('=');

                            string SpParamName = SepSpKeyVal[0];
                            string SpParamVal = SepSpKeyVal[1];

                            parameters[i - 1] = new SqlParameter(SpParamName, SpParamVal);
                        }
                    }
                }


                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), SpName, parameters);

                if (ds.Tables.Count > 0)
                {
                    return ds.Tables[0];
                }
                else
                {
                    throw new Exception("No data found.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching trainees' attendance: " + ex.Message);
            }
        }

        //#region filter

        //public DataTable FetchTraineesAttendanceByMonth(string month)
        //{
        //    try
        //    {
        //        SqlParameter[] parameters = new SqlParameter[1];
        //        parameters[0] = new SqlParameter("@Month", month);
        //        DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), "RD_TraineesAttendanceReport", parameters);

        // if (ds.Tables.Count > 0) { return ds.Tables[0]; } else { throw new Exception("No data
        // found."); } } catch (Exception ex) { throw new Exception("An error occurred while
        // fetching trainees' attendance: " + ex.Message); }

        //}
        //public DataTable FetchTrainerChangeLogsByMonth(string month)
        //{
        //    try
        //    {
        // var parts = month.Split('/'); string startMonth = parts[0]; string endMonth = parts[1];
        // SqlParameter[] parameters = { new SqlParameter("@StartMonth", startMonth), new
        // SqlParameter("@EndMonth", endMonth) };

        // DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), "Rd_TrainerChangeLogsReport", parameters);

        // if (ds.Tables.Count > 0) { return ds.Tables[0]; // Return the first table if it exists }
        // else { throw new Exception("No data found."); } } catch (Exception ex) { throw new
        // Exception("An error occurred while fetching trainees' attendance: " + ex.Message); }

        //}

        //#endregion Dashborad


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

    }
}