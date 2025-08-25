using DataLayer.Classes;
using DataLayer.Models;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;

namespace DataLayer.Services
{
    public class SRVConfirmedMarginal : SRVBase, DataLayer.Interfaces.ISRVConfirmedMarginal
    {
        public List<ConfirmedMarginalModel> GetConfirmedMarginalList(AMSReportsParamModel model)
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



                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_ConfirmedMarginalTrainees", param).Tables[0];
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

        private List<ConfirmedMarginalModel> LoopinData(DataTable dt)
        {
            List<ConfirmedMarginalModel> List = new List<ConfirmedMarginalModel>();

            foreach (DataRow r in dt.Rows)
            {
                List.Add(RowOfConfirmedMarginal(r));
            }
            return List;
        }

        private ConfirmedMarginalModel RowOfConfirmedMarginal(DataRow r)
        {
            ConfirmedMarginalModel confirmedMarginal = new ConfirmedMarginalModel();
            confirmedMarginal.SchemeName = r["SchemeName"].ToString();
            confirmedMarginal.TSPName = r["TSPName"].ToString();
            confirmedMarginal.ClassCode = r["ClassCode"].ToString();
            confirmedMarginal.ClassDuration = r["Duration"].ToString();
            confirmedMarginal.TraineeName = r["TraineeName"].ToString();
            confirmedMarginal.TraineeCode = r["TraineeCode"].ToString();
            confirmedMarginal.FatherName = r["FatherName"].ToString();
            confirmedMarginal.TraineeCNIC = r["TraineeCNIC"].ToString();
            confirmedMarginal.FirstMonthMarginal = r["FirstMonthMarginal"].ToString();
            confirmedMarginal.SecondMonthMarginal = r["SecondMonthMarginal"].ToString();
            //confirmedMarginal.Remarks = r["Remarks"].ToString();
            confirmedMarginal.StartDate = r["StartDate"].ToString().GetDate();
            confirmedMarginal.VisitMonth = r["VisitMonth"].ToString().GetDate();
            confirmedMarginal.BSSStatus = r["BSSStatus"].ToString();
            confirmedMarginal.SecondMonth = r["SecondMonth"].ToString();
            confirmedMarginal.FirstMonth = r["FirstMonth"].ToString();
            confirmedMarginal.DetailString = r["DetailString"].ToString();
            //confirmedMarginal.ActionToBeTaken = r["ActionToBeTaken"].ToString();
            return confirmedMarginal;

        }
    }
}
