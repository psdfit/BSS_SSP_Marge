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
    public class SRVClassStatus : SRVBase, ISRVClassStatus
    {
        public SRVClassStatus() { }
        public ClassStatusModel GetByClassStatusID(int ClassStatusID)
        {
            try
            {
                SqlParameter param = new SqlParameter("@ClassStatusID", ClassStatusID);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_ClassStatus", param).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    return RowOfClassStatus(dt.Rows[0]);
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public List<ClassStatusModel> SaveClassStatus(ClassStatusModel ClassStatus)
        {
            try
            {

                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@ClassStatusID", ClassStatus.ClassStatusID);
                param[1] = new SqlParameter("@ClassStatusName", ClassStatus.ClassStatusName);

                param[2] = new SqlParameter("@CurUserID", ClassStatus.CurUserID);
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_ClassStatus]", param);
                return FetchClassStatus();
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }
        private List<ClassStatusModel> LoopinData(DataTable dt)
        {
            List<ClassStatusModel> ClassStatusL = new List<ClassStatusModel>();

            foreach (DataRow r in dt.Rows)
            {
                ClassStatusL.Add(RowOfClassStatus(r));

            }
            return ClassStatusL;
        }

        private List<ClassStatusModel> LoopinDataClassReason(DataTable dt)
        {
            List<ClassStatusModel> ClassStatusL = new List<ClassStatusModel>();

            foreach (DataRow r in dt.Rows)
            {
                ClassStatusL.Add(RowOfClassStatusReason(r));

            }
            return ClassStatusL;
        }
        public List<ClassStatusModel> FetchClassStatus(ClassStatusModel mod)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_ClassStatus", Common.GetParams(mod)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<ClassStatusModel> FetchClassStatus()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_ClassStatus").Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public List<ClassStatusModel> FetchClassReason()
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.Text, "Select ClassStatusReasonID,Reason from ClassStatusReason where InActive = 0");
                DataTable dt = ds.Tables[0];
                return LoopinDataClassReason(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        
        public List<ClassStatusModel> FetchClassStatus(bool InActive)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_ClassStatus", new SqlParameter("@InActive", InActive)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public void ActiveInActive(int ClassStatusID, bool? InActive, int CurUserID)
        {

            SqlParameter[] PLead = new SqlParameter[3];
            PLead[0] = new SqlParameter("@ClassStatusID", ClassStatusID);
            PLead[1] = new SqlParameter("@InActive", InActive);
            PLead[2] = new SqlParameter("@CurUserID", CurUserID);
            SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AI_ClassStatus]", PLead);
        }
        private ClassStatusModel RowOfClassStatus(DataRow r)
        {
            ClassStatusModel ClassStatus = new ClassStatusModel();
            ClassStatus.ClassStatusID = Convert.ToInt32(r["ClassStatusID"]);
            ClassStatus.ClassStatusName = r["ClassStatusName"].ToString();
            ClassStatus.InActive = Convert.ToBoolean(r["InActive"]);
            ClassStatus.CreatedUserID = Convert.ToInt32(r["CreatedUserID"]);
            ClassStatus.ModifiedUserID = Convert.ToInt32(r["ModifiedUserID"]);
            ClassStatus.CreatedDate = r["CreatedDate"].ToString().GetDate();
            ClassStatus.ModifiedDate = r["ModifiedDate"].ToString().GetDate();

            return ClassStatus;
        }

        private ClassStatusModel RowOfClassStatusReason(DataRow r)
        {
            ClassStatusModel ClassStatus = new ClassStatusModel();
            ClassStatus.ClassStatusReasonID = Convert.ToInt32(r["ClassStatusReasonID"]);
            ClassStatus.ClassReason = r["Reason"].ToString();

            return ClassStatus;
        }
        
    }
}
