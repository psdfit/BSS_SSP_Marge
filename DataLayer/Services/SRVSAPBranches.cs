using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using DataLayer.Classes;
using DataLayer.Models;
using Newtonsoft.Json;
using DataLayer.Interfaces;
namespace DataLayer.Services
{
    public class SRVSAPBranches : SRVBase, ISRVSAPBranches
    {
        public SRVSAPBranches() { }
        public SAPBranchesModel GetById(int Id)
        {
            try
            {
                SqlParameter param = new SqlParameter("@Id", Id);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_SAPBranches", param).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    return RowOfSAPBranches(dt.Rows[0]);
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public List<SAPBranchesModel> SaveSAPBranches(SAPBranchesModel SAPBranches)
        {
            try
            {

                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@Id", SAPBranches.Id);
                param[1] = new SqlParameter("@BranchId", SAPBranches.BranchId);
                param[2] = new SqlParameter("@BranchName", SAPBranches.BranchName);

                param[3] = new SqlParameter("@CurUserID", SAPBranches.CurUserID);
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_SAPBranches]", param);
                return FetchSAPBranches();
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }
        private List<SAPBranchesModel> LoopinData(DataTable dt)
        {
            List<SAPBranchesModel> SAPBranchesL = new List<SAPBranchesModel>();

            foreach (DataRow r in dt.Rows)
            {
                SAPBranchesL.Add(RowOfSAPBranches(r));

            }
            return SAPBranchesL;
        }
        public List<SAPBranchesModel> FetchSAPBranches(SAPBranchesModel mod)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_SAPBranches", Common.GetParams(mod)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<SAPBranchesModel> FetchSAPBranches()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_SAPBranches").Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public List<SAPBranchesModel> FetchSAPBranches(bool InActive)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_SAPBranches", new SqlParameter("@InActive", InActive)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }

        public void ActiveInActive(int Id, bool? InActive, int CurUserID)
        {

            SqlParameter[] PLead = new SqlParameter[3];
            PLead[0] = new SqlParameter("@Id", Id);
            PLead[1] = new SqlParameter("@InActive", InActive);
            PLead[2] = new SqlParameter("@CurUserID", CurUserID);
            SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AI_SAPBranches]", PLead);
        }
        private SAPBranchesModel RowOfSAPBranches(DataRow r)
        {
            SAPBranchesModel SAPBranches = new SAPBranchesModel();
            SAPBranches.Id = Convert.ToInt32(r["Id"]);
            SAPBranches.BranchId = Convert.ToInt32(r["BranchId"]);
            SAPBranches.BranchName = r["BranchName"].ToString();
            SAPBranches.InActive = Convert.ToBoolean(r["InActive"]);
            SAPBranches.CreatedUserID = Convert.ToInt32(r["CreatedUserID"]);
            SAPBranches.ModifiedUserID = Convert.ToInt32(r["ModifiedUserID"]);
            SAPBranches.CreatedDate = r["CreatedDate"].ToString().GetDate();
            SAPBranches.ModifiedDate = r["ModifiedDate"].ToString().GetDate();

            return SAPBranches;
        }
    }
}
