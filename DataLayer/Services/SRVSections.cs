using DataLayer.Classes;
using DataLayer.Models;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;

namespace DataLayer.Services
{
    public class SRVSections : SRVBase, DataLayer.Interfaces.ISRVSections
    {
        public SRVSections()
        {
        }

        public SectionsModel GetBySectionID(int SectionID)
        {
            try
            {
                SqlParameter param = new SqlParameter("@SectionID", SectionID);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Sections", param).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    return RowOfSections(dt.Rows[0]);
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<SectionsModel> SaveSections(SectionsModel Sections)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@SectionID", Sections.SectionID);
                param[1] = new SqlParameter("@SectionName", Sections.SectionName);

                param[2] = new SqlParameter("@CurUserID", Sections.CurUserID);
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_Sections]", param);
                return FetchSections();
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }

        private List<SectionsModel> LoopinData(DataTable dt)
        {
            List<SectionsModel> SectionsL = new List<SectionsModel>();

            foreach (DataRow r in dt.Rows)
            {
                SectionsL.Add(RowOfSections(r));
            }
            return SectionsL;
        }

        public List<SectionsModel> FetchSections(SectionsModel mod)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Sections", Common.GetParams(mod)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<SectionsModel> FetchSections()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Sections").Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<SectionsModel> FetchSections(bool InActive)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Sections", new SqlParameter("@InActive", InActive)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public int BatchInsert(List<SectionsModel> ls, int @BatchFkey, int CurUserID)
        {
            SqlParameter[] param = new SqlParameter[3];

            param[0] = new SqlParameter("@Json", JsonConvert.SerializeObject(ls));
            param[1] = new SqlParameter("@", @BatchFkey);
            param[2] = new SqlParameter("@CurUserID", CurUserID);
            return SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[BAU_Sections]", param);
        }

        public void ActiveInActive(int SectionID, bool? InActive, int CurUserID)
        {
            SqlParameter[] PLead = new SqlParameter[3];
            PLead[0] = new SqlParameter("@SectionID", SectionID);
            PLead[1] = new SqlParameter("@InActive", InActive);
            PLead[2] = new SqlParameter("@CurUserID", CurUserID);
            SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AI_Sections]", PLead);
        }

        private SectionsModel RowOfSections(DataRow r)
        {
            SectionsModel Sections = new SectionsModel();
            Sections.SectionID = Convert.ToInt32(r["SectionID"]);
            Sections.SectionName = r["SectionName"].ToString();
            Sections.CreatedUserID = Convert.ToInt32(r["CreatedUserID"]);
            Sections.ModifiedUserID = Convert.ToInt32(r["ModifiedUserID"]);
            Sections.CreatedDate = r["CreatedDate"].ToString().GetDate();
            Sections.ModifiedDate = r["ModifiedDate"].ToString().GetDate();
            Sections.InActive = Convert.ToBoolean(r["InActive"]);

            return Sections;
        }
    }
}