using DataLayer.Classes;
using DataLayer.Models;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;

namespace DataLayer.Services
{
    public class SRVTestingAgency : SRVBase, DataLayer.Interfaces.ISRVTestingAgency
    {
        public SRVTestingAgency()
        {
        }

        public TestingAgencyModel GetByTestingAgencyID(int TestingAgencyID)
        {
            try
            {
                SqlParameter param = new SqlParameter("@TestingAgencyID", TestingAgencyID);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TestingAgency", param).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    return RowOfTestingAgency(dt.Rows[0]);
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<TestingAgencyModel> SaveTestingAgency(TestingAgencyModel TestingAgency)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@TestingAgencyID", TestingAgency.TestingAgencyID);
                param[1] = new SqlParameter("@TestingAgencyName", TestingAgency.TestingAgencyName);
                param[2] = new SqlParameter("@CertificationCategoryID", TestingAgency.CertificationCategoryID);

                param[3] = new SqlParameter("@CurUserID", TestingAgency.CurUserID);
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_TestingAgency]", param);
                return FetchTestingAgency();
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }

        private List<TestingAgencyModel> LoopinData(DataTable dt)
        {
            List<TestingAgencyModel> TestingAgencyL = new List<TestingAgencyModel>();

            foreach (DataRow r in dt.Rows)
            {
                TestingAgencyL.Add(RowOfTestingAgency(r));
            }
            return TestingAgencyL;
        }

        public List<TestingAgencyModel> FetchTestingAgency(TestingAgencyModel mod)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TestingAgency", Common.GetParams(mod)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<TestingAgencyModel> FetchTestingAgency()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TestingAgency").Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<TestingAgencyModel> FetchTestingAgency(bool InActive)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TestingAgency", new SqlParameter("@InActive", InActive)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public int BatchInsert(List<TestingAgencyModel> ls, int @BatchFkey, int CurUserID)
        {
            SqlParameter[] param = new SqlParameter[3];

            param[0] = new SqlParameter("@Json", JsonConvert.SerializeObject(ls));
            param[1] = new SqlParameter("@", @BatchFkey);
            param[2] = new SqlParameter("@CurUserID", CurUserID);
            return SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[BAU_TestingAgency]", param);
        }

        public List<TestingAgencyModel> GetByCertificationCategoryID(int CertificationCategoryID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TestingAgency", new SqlParameter("@CertificationCategoryID", CertificationCategoryID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public void ActiveInActive(int TestingAgencyID, bool? InActive, int CurUserID)
        {
            SqlParameter[] PLead = new SqlParameter[3];
            PLead[0] = new SqlParameter("@TestingAgencyID", TestingAgencyID);
            PLead[1] = new SqlParameter("@InActive", InActive);
            PLead[2] = new SqlParameter("@CurUserID", CurUserID);
            SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AI_TestingAgency]", PLead);
        }

        private TestingAgencyModel RowOfTestingAgency(DataRow r)
        {
            TestingAgencyModel TestingAgency = new TestingAgencyModel();
            TestingAgency.TestingAgencyID = Convert.ToInt32(r["TestingAgencyID"]);
            TestingAgency.TestingAgencyName = r["TestingAgencyName"].ToString();
            TestingAgency.CertificationCategoryID = Convert.ToInt32(r["CertificationCategoryID"]);
            TestingAgency.CertificationCategoryName = r["CertificationCategoryName"].ToString();
            TestingAgency.InActive = Convert.ToBoolean(r["InActive"]);
            TestingAgency.CreatedUserID = Convert.ToInt32(r["CreatedUserID"]);
            TestingAgency.ModifiedUserID = Convert.ToInt32(r["ModifiedUserID"]);
            TestingAgency.CreatedDate = r["CreatedDate"].ToString().GetDate();
            TestingAgency.ModifiedDate = r["ModifiedDate"].ToString().GetDate();

            return TestingAgency;
        }
    }
}