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
    public class SRVClassSections : SRVBase, DataLayer.Interfaces.ISRVClassSections
    {
        public SRVClassSections() { }
        public ClassSectionsModel GetBySectionID(int SectionID)
        {
            try
            {
                SqlParameter param = new SqlParameter("@SectionID", SectionID);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_ClassSections", param).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    return RowOfClassSections(dt.Rows[0]);
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public List<ClassSectionsModel> SaveClassSections(ClassSectionsModel ClassSections)
        {
            try
            {

                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@SectionID", ClassSections.SectionID);
                param[1] = new SqlParameter("@SectionName", ClassSections.SectionName);

                param[2] = new SqlParameter("@CurUserID", ClassSections.CurUserID);
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_ClassSections]", param);
                return FetchClassSections();
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }
        private List<ClassSectionsModel> LoopinData(DataTable dt)
        {
            List<ClassSectionsModel> ClassSectionsL = new List<ClassSectionsModel>();

            foreach (DataRow r in dt.Rows)
            {
                ClassSectionsL.Add(RowOfClassSections(r));

            }
            return ClassSectionsL;
        }
        public List<ClassSectionsModel> FetchClassSections(ClassSectionsModel mod)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_ClassSections", Common.GetParams(mod)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<ClassSectionsModel> FetchClassSections()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_ClassSections").Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public List<ClassSectionsModel> FetchClassSections(bool InActive)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_ClassSections", new SqlParameter("@InActive", InActive)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }

        public void ActiveInActive(int SectionID, bool? InActive, int CurUserID)
        {

            SqlParameter[] PLead = new SqlParameter[3];
            PLead[0] = new SqlParameter("@SectionID", SectionID);
            PLead[1] = new SqlParameter("@InActive", InActive);
            PLead[2] = new SqlParameter("@CurUserID", CurUserID);
            SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AI_ClassSections]", PLead);
        }
        private ClassSectionsModel RowOfClassSections(DataRow r)
        {
            ClassSectionsModel ClassSections = new ClassSectionsModel();
            ClassSections.SectionID = Convert.ToInt32(r["SectionID"]);
            ClassSections.SectionName = r["SectionName"].ToString();
            ClassSections.InActive = Convert.ToBoolean(r["InActive"]);
            ClassSections.CreatedUserID = Convert.ToInt32(r["CreatedUserID"]);
            ClassSections.ModifiedUserID = Convert.ToInt32(r["ModifiedUserID"]);
            ClassSections.CreatedDate = r["CreatedDate"].ToString().GetDate();
            ClassSections.ModifiedDate = r["ModifiedDate"].ToString().GetDate();

            return ClassSections;
        }
    }
}
