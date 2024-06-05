using DataLayer.Classes;
using DataLayer.Models;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;

namespace DataLayer.Services
{
    public class SRVProvinces : SRVBase, DataLayer.Interfaces.ISRVProvinces
    {
        public SRVProvinces()
        {
        }

        public ProvincesModel GetById(int Id)
        {
            try
            {
                SqlParameter param = new SqlParameter("@Id", Id);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Provinces", param).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    return RowOfProvinces(dt.Rows[0]);
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<ProvincesModel> SaveProvinces(ProvincesModel Provinces)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@Id", Provinces.Id);
                param[1] = new SqlParameter("@ProvinceName", Provinces.ProvinceName);

                param[2] = new SqlParameter("@CurUserID", Provinces.CurUserID);
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_Provinces]", param);
                return FetchProvinces();
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }

        private List<ProvincesModel> LoopinData(DataTable dt)
        {
            List<ProvincesModel> ProvincesL = new List<ProvincesModel>();

            foreach (DataRow r in dt.Rows)
            {
                ProvincesL.Add(RowOfProvinces(r));
            }
            return ProvincesL;
        }

        public List<ProvincesModel> FetchProvinces(ProvincesModel mod)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Provinces", Common.GetParams(mod)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<ProvincesModel> FetchProvinces()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Provinces").Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<ProvincesModel> FetchProvinces(bool InActive)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Provinces", new SqlParameter("@InActive", InActive)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<ProvincesModel> FetchProvince(bool InActive)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Province", new SqlParameter("@InActive", InActive)).Tables[0];
                return LoopinDataProvince(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        private List<ProvincesModel> LoopinDataProvince(DataTable dt)
        {
            List<ProvincesModel> ProvincesL = new List<ProvincesModel>();

            foreach (DataRow r in dt.Rows)
            {
                ProvincesL.Add(RowOfProvince(r));
            }
            return ProvincesL;
        }

        private ProvincesModel RowOfProvince(DataRow r)
        {
            ProvincesModel Provinces = new ProvincesModel();
            Provinces.ProvinceID = Convert.ToInt32(r["ProvinceID"]);
            Provinces.ProvinceName = r["ProvinceName"].ToString();
            Provinces.InActive = Convert.ToBoolean(r["InActive"]);

            return Provinces;
        }

        public int BatchInsert(List<ProvincesModel> ls, int @BatchFkey, int CurUserID)
        {
            SqlParameter[] param = new SqlParameter[3];

            param[0] = new SqlParameter("@Json", JsonConvert.SerializeObject(ls));
            param[1] = new SqlParameter("@", @BatchFkey);
            param[2] = new SqlParameter("@CurUserID", CurUserID);
            return SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[BAU_Provinces]", param);
        }

        public void ActiveInActive(int Id, bool? InActive, int CurUserID)
        {
            SqlParameter[] PLead = new SqlParameter[3];
            PLead[0] = new SqlParameter("@Id", Id);
            PLead[1] = new SqlParameter("@InActive", InActive);
            PLead[2] = new SqlParameter("@CurUserID", CurUserID);
            SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AI_Provinces]", PLead);
        }

        private ProvincesModel RowOfProvinces(DataRow r)
        {
            ProvincesModel Provinces = new ProvincesModel();
            Provinces.Id = Convert.ToInt32(r["Id"]);
            Provinces.ProvinceName = r["ProvinceName"].ToString();
            Provinces.InActive = Convert.ToBoolean(r["InActive"]);
            Provinces.CreatedUserID = Convert.ToInt32(r["CreatedUserID"]);
            Provinces.ModifiedUserID = Convert.ToInt32(r["ModifiedUserID"]);
            Provinces.CreatedDate = r["CreatedDate"].ToString().GetDate();
            Provinces.ModifiedDate = r["ModifiedDate"].ToString().GetDate();

            return Provinces;
        }
    }
}