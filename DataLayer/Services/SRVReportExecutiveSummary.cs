using DataLayer.Classes;
using DataLayer.Models;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;

namespace DataLayer.Services
{
    public class SRVReportExecutiveSummary : SRVBase, DataLayer.Interfaces.ISRVReportExecutiveSummary
    {
        public List<ReportExecutiveSummaryModel> GetReportExecutiveSummaryList(AMSReportsParamModel model)
        {
            try
            {
                String sDate = model.Month.ToString();
                DateTime datevalue = (Convert.ToDateTime(sDate.ToString()));

                String Month = datevalue.Day.ToString(); //Converting Day to Month
                String Day = datevalue.Month.ToString();  // Converting Month to Day
                String yy = datevalue.Year.ToString();
                string strMonth = yy + "-" + Month + "-" + Day;

                SqlParameter[] param = new SqlParameter[6];
                param[0] = new SqlParameter("@ClassID", model.ClassID);
                param[1] = new SqlParameter("@SchemeID", model.SchemeID);
                param[2] = new SqlParameter("@TSPID", model.TSPID);
                param[3] = new SqlParameter("@Month", model.Month);
                param[4] = new SqlParameter("@KAMID", model.KAMID ?? (object)DBNull.Value);
                param[5] = new SqlParameter("@FundingCategoryID", model.FundingCategoryID ?? (object)DBNull.Value);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "dbo.RD_ReportExecutiveSummary", param).Tables[0];
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

        private List<ReportExecutiveSummaryModel> LoopinData(DataTable dt)
        {
            List<ReportExecutiveSummaryModel> List = new List<ReportExecutiveSummaryModel>();

            foreach (DataRow r in dt.Rows)
            {
                List.Add(RowOfReportExecutiveSummary(r));
            }
            return List;
        }

        private ReportExecutiveSummaryModel RowOfReportExecutiveSummary(DataRow r)
        {
            ReportExecutiveSummaryModel ReportExecutiveSummary = new ReportExecutiveSummaryModel();
            ReportExecutiveSummary.SchemeName = r["SchemeName"].ToString();
            ReportExecutiveSummary.TSPName = r["TSPName"].ToString();
            ReportExecutiveSummary.ClassCode = r["ClassCode"].ToString();
            ReportExecutiveSummary.TotalNumberOfRegisteredTrainees = r["TotalNumberOfRegisteredTrainees"].ToString();
            ReportExecutiveSummary.TotalTraineesPresentInClass = r["TotalTraineesPresentInClass"].ToString();
            return ReportExecutiveSummary;

        }
    }
}
