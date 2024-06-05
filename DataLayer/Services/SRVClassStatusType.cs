using DataLayer.Classes;
using DataLayer.Models;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;

namespace DataLayer.Services
{
    public class SRVClassStatusType : SRVBase, DataLayer.Interfaces.ISRVClassStatusType
    {
        public SRVClassStatusType()
        {
        }

        public ClassStatusTypeModel GetByStatusTypeID(int StatusTypeID)
        {
            try
            {
                SqlParameter param = new SqlParameter("@StatusTypeID", StatusTypeID);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_ClassStatusType", param).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    return RowOfClassStatusType(dt.Rows[0]);
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<ClassStatusTypeModel> SaveClassStatusType(ClassStatusTypeModel ClassStatusType)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@StatusTypeID", ClassStatusType.StatusTypeID);
                param[1] = new SqlParameter("@StatusTypeName", ClassStatusType.StatusTypeName);

                param[2] = new SqlParameter("@CurUserID", ClassStatusType.CurUserID);
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_ClassStatusType]", param);
                return FetchClassStatusType();
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }

        private List<ClassStatusTypeModel> LoopinData(DataTable dt)
        {
            List<ClassStatusTypeModel> ClassStatusTypeL = new List<ClassStatusTypeModel>();

            foreach (DataRow r in dt.Rows)
            {
                ClassStatusTypeL.Add(RowOfClassStatusType(r));
            }
            return ClassStatusTypeL;
        }

        public List<ClassStatusTypeModel> FetchClassStatusType(ClassStatusTypeModel mod)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_ClassStatusType", Common.GetParams(mod)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<ClassStatusTypeModel> FetchClassStatusType()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_ClassStatusType").Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<ClassStatusTypeModel> FetchClassStatusType(bool InActive)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_ClassStatusType", new SqlParameter("@InActive", InActive)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public int BatchInsert(List<ClassStatusTypeModel> ls, int @BatchFkey, int CurUserID)
        {
            SqlParameter[] param = new SqlParameter[3];

            param[0] = new SqlParameter("@Json", JsonConvert.SerializeObject(ls));
            param[1] = new SqlParameter("@", @BatchFkey);
            param[2] = new SqlParameter("@CurUserID", CurUserID);
            return SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[BAU_ClassStatusType]", param);
        }

        public void ActiveInActive(int StatusTypeID, bool? InActive, int CurUserID)
        {
            SqlParameter[] PLead = new SqlParameter[3];
            PLead[0] = new SqlParameter("@StatusTypeID", StatusTypeID);
            PLead[1] = new SqlParameter("@InActive", InActive);
            PLead[2] = new SqlParameter("@CurUserID", CurUserID);
            SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AI_ClassStatusType]", PLead);
        }

        private ClassStatusTypeModel RowOfClassStatusType(DataRow r)
        {
            ClassStatusTypeModel ClassStatusType = new ClassStatusTypeModel();
            ClassStatusType.StatusTypeID = Convert.ToInt32(r["StatusTypeID"]);
            ClassStatusType.StatusTypeName = r["StatusTypeName"].ToString();
            ClassStatusType.InActive = Convert.ToBoolean(r["InActive"]);
            ClassStatusType.CreatedUserID = Convert.ToInt32(r["CreatedUserID"]);
            ClassStatusType.ModifiedUserID = Convert.ToInt32(r["ModifiedUserID"]);
            ClassStatusType.CreatedDate = r["CreatedDate"].ToString().GetDate();
            ClassStatusType.ModifiedDate = r["ModifiedDate"].ToString().GetDate();

            return ClassStatusType;
        }
    }
}