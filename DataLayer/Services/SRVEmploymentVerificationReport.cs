using DataLayer.Classes;
using DataLayer.Models;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;

namespace DataLayer.Services
{
    public class SRVEmploymentVerificationReport : SRVBase, DataLayer.Interfaces.ISRVEmploymentVerificationReport
    {
        public List<EmploymentVerificationReportModel> GetEmploymentVerificationReportList(AMSReportsParamModel model)
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
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_EmploymentVerificationReport", param).Tables[0];
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

        private List<EmploymentVerificationReportModel> LoopinData(DataTable dt)
        {
            List<EmploymentVerificationReportModel> List = new List<EmploymentVerificationReportModel>();

            foreach (DataRow r in dt.Rows)
            {
                List.Add(RowOfEmploymentVerificationReport(r));
            }
            return List;
        }

        private EmploymentVerificationReportModel RowOfEmploymentVerificationReport(DataRow r)
        {
            EmploymentVerificationReportModel employmentVerification = new EmploymentVerificationReportModel();
            employmentVerification.DetailString = r["DetailString"].ToString();
            employmentVerification.SchemeName = r["SchemeName"].ToString();
            employmentVerification.TSPName = r["TSPName"].ToString();
            employmentVerification.ClassCode = r["ClassCode"].ToString();
            employmentVerification.ClassStartDate = r["ClassStartDate"].ToString();
            employmentVerification.ClassEndDate = r["ClassEndDate"].ToString();
            employmentVerification.EmploymentCommitment = r["EmploymentCommitment"].ToString();
            employmentVerification.CompletedTrainees = r["CompletedTrainees"].ToString();
            employmentVerification.EmploymentCommitmentTrainees = r["EmploymentCommitmentTrainees"].ToString();
            employmentVerification.EmploymentCommitmentFloor = r["EmploymentCommitmentFloor"].ToString();
            employmentVerification.Reported = r["Reported"].ToString();
            employmentVerification.Unverified = r["Unverified"].ToString();
            employmentVerification.Verified = r["Verified"].ToString();
            employmentVerification.VerifiedtoCommitment = r["VerifiedtoCommitment"].ToString();
            return employmentVerification;
        }
    }
}
