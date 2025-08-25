using DataLayer.Classes;
using DataLayer.Models;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;

namespace DataLayer.Services
{
    public class SRVCovidMaskViolation : SRVBase, DataLayer.Interfaces.ISRVCovidMaskViolation
    {
        public List<CovidMaskViolationModel> GetCovidMaskViolationList(AMSReportsParamModel model)
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
                param[4] = new SqlParameter("@KAMID", model.KAMID ?? (object)DBNull.Value);
                param[5] = new SqlParameter("@FundingCategoryID", model.FundingCategoryID ?? (object)DBNull.Value);
                param[6] = new SqlParameter("@Schemes", string.IsNullOrEmpty(model.Schemes) ? DBNull.Value : (object)model.Schemes);
                param[7] = new SqlParameter("@TSPs", string.IsNullOrEmpty(model.TSPs) ? DBNull.Value : (object)model.TSPs);
                param[8] = new SqlParameter("@Classes", string.IsNullOrEmpty(model.Classes) ? DBNull.Value : (object)model.Classes);

                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "dbo.RD_CovidNonMask", param).Tables[0];
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

        private List<CovidMaskViolationModel> LoopinData(DataTable dt)
        {
            List<CovidMaskViolationModel> List = new List<CovidMaskViolationModel>();

            foreach (DataRow r in dt.Rows)
            {
                List.Add(RowOfCovidMaskViolation(r));
            }
            return List;
        }

        private CovidMaskViolationModel RowOfCovidMaskViolation(DataRow r)
        {
            CovidMaskViolationModel covidMask = new CovidMaskViolationModel();
            covidMask.DetailString = r["DetailString"].ToString();
            covidMask.SchemeName = r["SchemeName"].ToString();
			covidMask.TSPName = r["TSPName"].ToString();
            covidMask.ClassCode = r["ClassCode"].ToString();
            covidMask.Name = r["Name"].ToString();
            covidMask.FatherName = r["FatherName"].ToString();
            covidMask.Cnic = r["Cnic"].ToString();
            covidMask.Duration = Convert.ToInt32(r["Duration"]);
            covidMask.TraineeDoesNotWearMask = r["TraineeDoesNotWearMask"].ToString();
            covidMask.Action = r["Action"].ToString();
            return covidMask;

        }
    }
}
