using DataLayer.Classes;
using DataLayer.Models;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;

namespace DataLayer.Services
{
    public class SRVViolationSummary : SRVBase, DataLayer.Interfaces.ISRVViolationSummary
    {
        public List<ViolationSummaryModel> GetViolationSummaryList(AMSReportsParamModel model)
        {
            try
            {
                String sDate = model.Month.ToString();
                DateTime datevalue = (Convert.ToDateTime(sDate.ToString()));

                String Month = datevalue.Day.ToString(); //Converting Day to Month
                String Day = datevalue.Month.ToString();  // Converting Month to Day
                String yy = datevalue.Year.ToString();
                string strMonth = yy + "-" + Month + "-" + Day;

                SqlParameter[] param = new SqlParameter[10];
                param[0] = new SqlParameter("@ClassID", model.ClassID);
                param[1] = new SqlParameter("@SchemeID", model.SchemeID);
                param[2] = new SqlParameter("@TSPID", model.TSPID);
                param[3] = new SqlParameter("@Month", model.Month);
                param[4] = new SqlParameter("@UserID", model.UserID);
                param[5] = new SqlParameter("@KAMID", model.KAMID ?? (object)DBNull.Value);
                param[6] = new SqlParameter("@FundingCategoryID", model.FundingCategoryID ?? (object)DBNull.Value);
                param[7] = new SqlParameter("@Schemes", string.IsNullOrEmpty(model.Schemes) ? DBNull.Value : (object)model.Schemes);
                param[8] = new SqlParameter("@TSPs", string.IsNullOrEmpty(model.TSPs) ? DBNull.Value : (object)model.TSPs);
                param[9] = new SqlParameter("@Classes", string.IsNullOrEmpty(model.Classes) ? DBNull.Value : (object)model.Classes);

                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_ViolationSummaryTrainees", param)?.Tables[0];
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
       
        private List<ViolationSummaryModel> LoopinData(DataTable dt)
        {
            List<ViolationSummaryModel> List = new List<ViolationSummaryModel>();

            foreach (DataRow r in dt.Rows)
            {
                List.Add(RowOfViolationSummary(r));
            }
            return List;
        }

        private ViolationSummaryModel RowOfViolationSummary(DataRow r)
        {
            ViolationSummaryModel violationSummary = new ViolationSummaryModel();
            //violationSummary.SchemeName = r["SchemeName"].ToString();
            violationSummary.SchemeName = r["SchemeName"].ToString();
            violationSummary.TSPName = r["TSPName"].ToString();
            violationSummary.ClassCode = r["ClassCode"].ToString();
            violationSummary.Minor = Convert.ToInt32(r["Minor"]);
            violationSummary.Major = Convert.ToInt32(r["Major"]);
            violationSummary.Serious = Convert.ToInt32(r["Serious"]);
            violationSummary.Total = Convert.ToInt32(r["Total"]);
            violationSummary.Observation = Convert.ToInt32(r["Observation"]); 
            violationSummary.Remarks = r["Remarks"].ToString();
            violationSummary.TotalClasses = Convert.ToInt32(r["TotalClasses"]);
            violationSummary.MonthForReport =r["MonthForReport"].ToString().GetDate();
            string Remarks = r["Remarks"].ToString();
            violationSummary.Remarks = Remarks;

            return violationSummary;
        }
    }
}
