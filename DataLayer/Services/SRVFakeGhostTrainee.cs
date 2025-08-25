using DataLayer.Classes;
using DataLayer.Models;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;

namespace DataLayer.Services
{
    public class SRVFakeGhostTrainee : SRVBase, DataLayer.Interfaces.ISRVFakeGhostTrainee
    {
        public List<FakeGhostTraineeModel> GetFakeGhostTraineeList(AMSReportsParamModel model)
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

                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "dbo.RD_FakeGhostTrainee", param).Tables[0];
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

        private List<FakeGhostTraineeModel> LoopinData(DataTable dt)
        {
            List<FakeGhostTraineeModel> List = new List<FakeGhostTraineeModel>();

            foreach (DataRow r in dt.Rows)
            {
                List.Add(RowOfFakeGhostTrainee(r));
            }
            return List;
        }

        private FakeGhostTraineeModel RowOfFakeGhostTrainee(DataRow r)
        {
            FakeGhostTraineeModel fakeGhostTrainee = new FakeGhostTraineeModel();
            fakeGhostTrainee.DetailString = r["DetailString"].ToString();
            fakeGhostTrainee.SchemeName = r["SchemeName"].ToString();
            fakeGhostTrainee.TSPName = r["TSPName"].ToString();
            fakeGhostTrainee.ClassCode = r["ClassCode"].ToString();
            fakeGhostTrainee.Name = r["Name"].ToString();
            fakeGhostTrainee.FatherName = r["FatherName"].ToString();
            fakeGhostTrainee.Cnic = r["Cnic"].ToString();
            fakeGhostTrainee.PreviousMonth = r["PreviousMonth"].ToString();
            fakeGhostTrainee.SelectedMonth = r["SelectedMonth"].ToString();
            fakeGhostTrainee.Visit1SelectMonth = r["Visit1SelectMonth"].ToString();
            fakeGhostTrainee.Visit2SelectMonth = r["Visit2SelectMonth"].ToString();
            fakeGhostTrainee.Visit1PreviousMonth = r["Visit1PreviousMonth"].ToString();
            fakeGhostTrainee.Visit2PreviousMonth = r["Visit2PreviousMonth"].ToString();
            return fakeGhostTrainee;

        }
    }
}
