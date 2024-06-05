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
    public class SRVAcademicDiscipline : SRVBase, ISRVAcademicDiscipline
    {
        public SRVAcademicDiscipline() { }
        public AcademicDisciplineModel GetByAcademicDisciplineID(int AcademicDisciplineID)
        {
            try
            {
                SqlParameter param = new SqlParameter("@AcademicDisciplineID", AcademicDisciplineID);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_AcademicDiscipline", param).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    return RowOfAcademicDiscipline(dt.Rows[0]);
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public List<AcademicDisciplineModel> SaveAcademicDiscipline(AcademicDisciplineModel AcademicDiscipline)
        {
            try
            {

                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@AcademicDisciplineID", AcademicDiscipline.AcademicDisciplineID);
                param[1] = new SqlParameter("@AcademicDisciplineName", AcademicDiscipline.AcademicDisciplineName);

                param[2] = new SqlParameter("@CurUserID", AcademicDiscipline.CurUserID);
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_AcademicDiscipline]", param);
                return FetchAcademicDiscipline();
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }
        private List<AcademicDisciplineModel> LoopinData(DataTable dt)
        {
            List<AcademicDisciplineModel> AcademicDisciplineL = new List<AcademicDisciplineModel>();

            foreach (DataRow r in dt.Rows)
            {
                AcademicDisciplineL.Add(RowOfAcademicDiscipline(r));

            }
            return AcademicDisciplineL;
        }
        public List<AcademicDisciplineModel> FetchAcademicDiscipline(AcademicDisciplineModel mod)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_AcademicDiscipline", Common.GetParams(mod)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<AcademicDisciplineModel> FetchAcademicDiscipline()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_AcademicDiscipline").Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public List<AcademicDisciplineModel> FetchAcademicDiscipline(bool InActive)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_AcademicDiscipline", new SqlParameter("@InActive", InActive)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public void ActiveInActive(int AcademicDisciplineID, bool? InActive, int CurUserID)
        {

            SqlParameter[] PLead = new SqlParameter[3];
            PLead[0] = new SqlParameter("@AcademicDisciplineID", AcademicDisciplineID);
            PLead[1] = new SqlParameter("@InActive", InActive);
            PLead[2] = new SqlParameter("@CurUserID", CurUserID);
            SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AI_AcademicDiscipline]", PLead);
        }
        private AcademicDisciplineModel RowOfAcademicDiscipline(DataRow r)
        {
            AcademicDisciplineModel AcademicDiscipline = new AcademicDisciplineModel();
            AcademicDiscipline.AcademicDisciplineID = Convert.ToInt32(r["AcademicDisciplineID"]);
            AcademicDiscipline.AcademicDisciplineName = r["AcademicDisciplineName"].ToString();
            AcademicDiscipline.InActive = Convert.ToBoolean(r["InActive"]);
            AcademicDiscipline.CreatedUserID = Convert.ToInt32(r["CreatedUserID"]);
            AcademicDiscipline.ModifiedUserID = Convert.ToInt32(r["ModifiedUserID"]);
            AcademicDiscipline.CreatedDate = r["CreatedDate"].ToString().GetDate();
            AcademicDiscipline.ModifiedDate = r["ModifiedDate"].ToString().GetDate();

            return AcademicDiscipline;
        }
    }
}
