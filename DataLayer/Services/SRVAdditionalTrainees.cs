using DataLayer.Classes;
using DataLayer.Models;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;

namespace DataLayer.Services
{
    public class SRVAdditionalTrainees : SRVBase, DataLayer.Interfaces.ISRVAdditionalTrainees
    {
        public List<AdditionalTraineesModel> GetAdditionalTraineesList(AMSReportsParamModel model)
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
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "dbo.RD_AdditionalTrainees", param).Tables[0];
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

        private List<AdditionalTraineesModel> LoopinData(DataTable dt)
        {
            List<AdditionalTraineesModel> List = new List<AdditionalTraineesModel>();

            foreach (DataRow r in dt.Rows)
            {
                List.Add(RowOfAdditionalTrainees(r));
            }
            return List;
        }

        private AdditionalTraineesModel RowOfAdditionalTrainees(DataRow r)
        {
            AdditionalTraineesModel additionalTrainees = new AdditionalTraineesModel();
            additionalTrainees.DetailString = r["DetailString"].ToString();
            additionalTrainees.TSPName = r["TSPName"].ToString();
            additionalTrainees.SchemeName = r["SchemeName"].ToString();
            additionalTrainees.ClassCode = r["ClassCode"].ToString();
            additionalTrainees.Name = r["Name"].ToString();
            additionalTrainees.FatherName = r["FatherName"].ToString();
            additionalTrainees.Cnic = r["Cnic"].ToString();
            additionalTrainees.PreviousMonth = r["PreviousMonth"].ToString();
            additionalTrainees.SelectedMonth = r["SelectedMonth"].ToString();
            additionalTrainees.TickStatusSelectedMonth = r["TickStatusSelectMonth"].ToString();
            additionalTrainees.TickStatusPreviousMonth = r["TickStatusPreviousMonth"].ToString();
            additionalTrainees.Remarks = r["Remarks"].ToString();
            return additionalTrainees;

        }
    }
}
