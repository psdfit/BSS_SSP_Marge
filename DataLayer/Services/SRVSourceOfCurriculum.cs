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
    public class SRVSourceOfCurriculum : SRVBase, ISRVSourceOfCurriculum
    {
        public SRVSourceOfCurriculum() { }
        public SourceOfCurriculumModel GetBySourceOfCurriculumID(int SourceOfCurriculumID)
        {
            try
            {
                SqlParameter param = new SqlParameter("@SourceOfCurriculumID", SourceOfCurriculumID);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_SourceOfCurriculum", param).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    return RowOfSourceOfCurriculum(dt.Rows[0]);
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public List<SourceOfCurriculumModel> SaveSourceOfCurriculum(SourceOfCurriculumModel SourceOfCurriculum)
        {
            try
            {

                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@SourceOfCurriculumID", SourceOfCurriculum.SourceOfCurriculumID);
                param[1] = new SqlParameter("@Name", SourceOfCurriculum.Name);
                //param[2] = new SqlParameter("@CertAuthID", SourceOfCurriculum.CertAuthID);
                param[2] = new SqlParameter("@CurUserID", SourceOfCurriculum.CurUserID);
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_SourceOfCurriculum]", param);
                return FetchSourceOfCurriculum();
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }
        private List<SourceOfCurriculumModel> LoopinData(DataTable dt)
        {
            List<SourceOfCurriculumModel> SourceOfCurriculumL = new List<SourceOfCurriculumModel>();

            foreach (DataRow r in dt.Rows)
            {
                SourceOfCurriculumL.Add(RowOfSourceOfCurriculum(r));

            }
            return SourceOfCurriculumL;
        }
        public List<SourceOfCurriculumModel> FetchSourceOfCurriculum(SourceOfCurriculumModel mod)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_SourceOfCurriculum", Common.GetParams(mod)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<SourceOfCurriculumModel> FetchSourceOfCurriculum()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_SourceOfCurriculum").Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public List<SourceOfCurriculumModel> FetchSourceOfCurriculum(bool InActive)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_SourceOfCurriculum", new SqlParameter("@InActive", InActive)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }

        public void ActiveInActive(int SourceOfCurriculumID, bool? InActive, int CurUserID)
        {

            SqlParameter[] PLead = new SqlParameter[3];
            PLead[0] = new SqlParameter("@SourceOfCurriculumID", SourceOfCurriculumID);
            PLead[1] = new SqlParameter("@InActive", InActive);
            PLead[2] = new SqlParameter("@CurUserID", CurUserID);
            SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AI_SourceOfCurriculum]", PLead);
        }
        private SourceOfCurriculumModel RowOfSourceOfCurriculum(DataRow r)
        {
            SourceOfCurriculumModel SourceOfCurriculum = new SourceOfCurriculumModel();
            SourceOfCurriculum.SourceOfCurriculumID = Convert.ToInt32(r["SourceOfCurriculumID"]);
            SourceOfCurriculum.Name = r["Name"].ToString();
            //SourceOfCurriculum.CertAuthID = Convert.ToInt32(r["CertAuthID"]);
            //SourceOfCurriculum.CertAuthName = r["CertAuthName"].ToString();
            SourceOfCurriculum.InActive = Convert.ToBoolean(r["InActive"]);
            SourceOfCurriculum.CreatedUserID = Convert.ToInt32(r["CreatedUserID"]);
            SourceOfCurriculum.ModifiedUserID = Convert.ToInt32(r["ModifiedUserID"]);
            SourceOfCurriculum.CreatedDate = r["CreatedDate"].ToString().GetDate();
            SourceOfCurriculum.ModifiedDate = r["ModifiedDate"].ToString().GetDate();
            return SourceOfCurriculum;
        }
    }
}
