using DataLayer.Classes;
using DataLayer.Models;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;

namespace DataLayer.Services
{
    public class SRVEducationTypes : SRVBase, DataLayer.Interfaces.ISRVEducationTypes
    {
        public SRVEducationTypes()
        {
        }

        public EducationTypesModel GetByID(int EducationTypeID)
        {
            try
            {
                SqlParameter param = new SqlParameter("@EducationTypeID", EducationTypeID);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_EducationTypes", param).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    return RowOfEducationTypes(dt.Rows[0]);
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<EducationTypesModel> SaveEducationTypes(EducationTypesModel EducationTypes)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@EducationTypeID", EducationTypes.EducationTypeID);
                param[1] = new SqlParameter("@Education", EducationTypes.Education);

                param[2] = new SqlParameter("@CurUserID", EducationTypes.CurUserID);
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_EducationTypes]", param);
                return FetchEducationTypes();
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }

        private List<EducationTypesModel> LoopinData(DataTable dt)
        {
            List<EducationTypesModel> EducationTypesL = new List<EducationTypesModel>();

            foreach (DataRow r in dt.Rows)
            {
                EducationTypesL.Add(RowOfEducationTypes(r));
            }
            return EducationTypesL;
        }

        public List<EducationTypesModel> FetchEducationTypes(EducationTypesModel mod)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_EducationTypes", Common.GetParams(mod)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<EducationTypesModel> FetchEducationTypes()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_EducationTypes").Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<EducationTypesModel> FetchEducationTypes(bool InActive)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_EducationTypes", new SqlParameter("@InActive", InActive)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public DataTable FetchEducationTypesSSP(bool InActive)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_EducationTypes", new SqlParameter("@InActive", InActive)).Tables[0];
                return dt;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public DataTable FetchPaymentStructure()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_SAP_Scheme").Tables[0];
                return dt;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public DataTable FetchTraineeSupportItems()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TraineeSupportItems").Tables[0];
                return dt;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public DataTable FetchApplicabilityData(bool InActive)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Applicability", new SqlParameter("@InActive", InActive)).Tables[0];
                return dt;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }


        public int BatchInsert(List<EducationTypesModel> ls, int @BatchFkey, int CurUserID)
        {
            SqlParameter[] param = new SqlParameter[3];

            param[0] = new SqlParameter("@Json", JsonConvert.SerializeObject(ls));
            param[1] = new SqlParameter("@", @BatchFkey);
            param[2] = new SqlParameter("@CurUserID", CurUserID);
            return SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[BAU_EducationTypes]", param);
        }

        public void ActiveInActive(int EducationTypeID, bool? InActive, int CurUserID)
        {
            SqlParameter[] PLead = new SqlParameter[3];
            PLead[0] = new SqlParameter("@EducationTypeID", EducationTypeID);
            PLead[1] = new SqlParameter("@InActive", InActive);
            PLead[2] = new SqlParameter("@CurUserID", CurUserID);
            SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AI_EducationTypes]", PLead);
        }

        private EducationTypesModel RowOfEducationTypes(DataRow r)
        {
            EducationTypesModel EducationTypes = new EducationTypesModel();
            EducationTypes.EducationTypeID = Convert.ToInt32(r["EducationTypeID"]);
            EducationTypes.Education = r["Education"].ToString();
            EducationTypes.InActive = Convert.ToBoolean(r["InActive"]);
            EducationTypes.CreatedUserID = Convert.ToInt32(r["CreatedUserID"]);
            EducationTypes.ModifiedUserID = Convert.ToInt32(r["ModifiedUserID"]);
            EducationTypes.CreatedDate = r["CreatedDate"].ToString().GetDate();
            EducationTypes.ModifiedDate = r["ModifiedDate"].ToString().GetDate();

            return EducationTypes;
        }
    }
}