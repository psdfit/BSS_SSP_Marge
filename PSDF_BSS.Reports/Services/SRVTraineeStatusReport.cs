using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using PSDF_BSS.Reports.Models;
using PSDF_BSS.Reports.Interfaces;

namespace PSDF_BSS.Reports.Services
{
    public class SRVTraineeStatusReport : ISRVTraineeStatusReport
    {
        public List<TraineeStatusReportModel> GetTraineeStatusReportList(int? schemeId, int? tspId, int? classId, string dateTime, string reportType)
        {
            try
            {
                DateTime? date = null;
                if (!string.IsNullOrEmpty(dateTime))
                    date = DateTime.ParseExact(dateTime, "dd/MM/yyyy", null);
                schemeId = schemeId > 0 ? schemeId : 0;
                tspId = tspId > 0 ? tspId : 0;
                classId = classId > 0 ? classId : 0;
                SqlParameter[] param = new SqlParameter[5];
                param[0] = new SqlParameter("@SchemeId", schemeId);
                param[1] = new SqlParameter("@TspId", tspId);
                param[2] = new SqlParameter("@ClassId", classId);
                param[3] = new SqlParameter("@Month", date);
                param[4] = new SqlParameter("@ReportType", reportType);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "dbo.RD_TraineeStatusReport", param).Tables[0];
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
        private List<TraineeStatusReportModel> LoopinData(DataTable dt)
        {
            List<TraineeStatusReportModel> List = new List<TraineeStatusReportModel>();

            foreach (DataRow r in dt.Rows)
            {
                List.Add(RowOfTraineeStatusReport(r));
            }
            return List;
        }
        private TraineeStatusReportModel RowOfTraineeStatusReport(DataRow r)
        {
            TraineeStatusReportModel traineeStatusReport = new TraineeStatusReportModel();
            {
                traineeStatusReport.ReportName = r["ReportName"].ToString();
                traineeStatusReport.SchemeName = r["SchemeName"].ToString();
                traineeStatusReport.StartDate = r["StartDate"].ToString();
                traineeStatusReport.EndDate = r["EndDate"].ToString();
                traineeStatusReport.TotalContractedTrainees = r["Enrolled"] != DBNull.Value ? Convert.ToInt32(r["Enrolled"]) : 0;
                traineeStatusReport.GenderWiseBifurcationInPer = Math.Round(Convert.ToDouble(r["GenderWiseBifurcationInPer"]), 2);
                traineeStatusReport.Enrolled = r["Enrolled"] != DBNull.Value ? Convert.ToInt32(r["Enrolled"]) : 0;
                traineeStatusReport.Completed = r["Enrolled"] != DBNull.Value ? Convert.ToInt32(r["Enrolled"]) : 0;
                traineeStatusReport.SectorWiseContracted = r["SectorWiseContracted"].ToString();
                traineeStatusReport.ClusterWiseContracted = r["ClusterWiseContracted"].ToString();
                traineeStatusReport.FundingSourceName = r["FundingSourceName"].ToString();
            }
            return traineeStatusReport;
        }
    }
}