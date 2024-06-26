﻿using DataLayer.Classes;
using DataLayer.Models;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;

namespace DataLayer.Services
{
    public class SRVDeletedDropout : SRVBase, DataLayer.Interfaces.ISRVDeletedDropout
    {
        public List<DeletedDropoutModel> GetDeletedDropoutList(AMSReportsParamModel model)
        {
            try
            {
                String sDate = model.Month.ToString();
                DateTime datevalue = (Convert.ToDateTime(sDate.ToString()));

                String Month = datevalue.Day.ToString(); //Converting Day to Month
                String Day = datevalue.Month.ToString();  // Converting Month to Day
                String yy = datevalue.Year.ToString();
                string strMonth = yy + "-" + Month + "-" + Day;

                SqlParameter[] param = new SqlParameter[5];
                param[0] = new SqlParameter("@ClassID", model.ClassID);
                param[1] = new SqlParameter("@SchemeID", model.SchemeID);
                param[2] = new SqlParameter("@TSPID", model.TSPID);
                param[3] = new SqlParameter("@Month", model.Month);
                param[4] = new SqlParameter("@UserID", model.UserID);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "dbo.RD_DeletedDropout", param).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    return LoopinData(dt);
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private List<DeletedDropoutModel> LoopinData(DataTable dt)
        {
            List<DeletedDropoutModel> List = new List<DeletedDropoutModel>();

            foreach (DataRow r in dt.Rows)
            {
                List.Add(RowOfDeletedDropout(r));
            }
            return List;
        }

        private DeletedDropoutModel RowOfDeletedDropout(DataRow r)
        {
            DeletedDropoutModel deletedDropout = new DeletedDropoutModel();
            deletedDropout.DetailString = r["DetailString"].ToString();
            deletedDropout.TSPName = r["TSPName"].ToString();
            deletedDropout.SchemeName = r["SchemeName"].ToString();
            deletedDropout.ClassCode = r["ClassCode"].ToString();
            deletedDropout.TraineeName = r["TraineeName"].ToString();
            deletedDropout.FatherName = r["FatherName"].ToString();
            deletedDropout.TraineeCNIC = r["TraineeCNIC"].ToString();
            deletedDropout.PreviousMonth = r["PreviousMonth"].ToString();
            deletedDropout.SelectedMonth = r["SelectedMonth"].ToString();
            deletedDropout.TickStatusSelectedMonth = r["TickStatusSelectMonth"].ToString();
            deletedDropout.TickStatusPreviousMonth = r["TickStatusPreviousMonth"].ToString();
            //string month = r["Month"].ToString();
            //if (deletedDropout.PreviousMonth.Contains(month))
            //{
            //    deletedDropout.PreviousMonth = r["TickStatus"].ToString();
            //   // deletedDropout.SelectedMonthDeleted = "";
            //}
            //if (deletedDropout.SelectedMonth.Contains(month))
            //{
            //    //deletedDropout.PreviousMonthDeleted = r["TickStatus"].ToString();
            //     deletedDropout.SelectedMonth = r["TickStatus"].ToString();
            //}
            deletedDropout.Remarks = r["Remarks"].ToString();
            deletedDropout.StartDate = r["StartDate"].ToString().GetDate();
            deletedDropout.StatusInMIS = r["StatusInMIS"].ToString();
            //deletedDropout.ActionToBeTaken = r["ActionToBeTaken"].ToString();
            return deletedDropout;

        }
    }
}
