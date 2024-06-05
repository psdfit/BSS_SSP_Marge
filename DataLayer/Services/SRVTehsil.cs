using DataLayer.Classes;
using DataLayer.Models;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;

namespace DataLayer.Services
{
    public class SRVTehsil : SRVBase, DataLayer.Interfaces.ISRVTehsil
    {
        public SRVTehsil()
        {
        }

        public TehsilModel GetByTehsilID(int TehsilID)
        {
            try
            {
                SqlParameter param = new SqlParameter("@TehsilID", TehsilID);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Tehsil", param).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    return RowOfTehsil(dt.Rows[0]);
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<TehsilModel> SaveTehsil(TehsilModel Tehsil)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@TehsilID", Tehsil.TehsilID);
                param[1] = new SqlParameter("@TehsilName", Tehsil.TehsilName);
                param[2] = new SqlParameter("@DistrictID", Tehsil.DistrictID);

                param[3] = new SqlParameter("@CurUserID", Tehsil.CurUserID);
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_Tehsil]", param);
                return FetchTehsil();
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }

        private List<TehsilModel> LoopinData(DataTable dt)
        {
            List<TehsilModel> TehsilL = new List<TehsilModel>();

            foreach (DataRow r in dt.Rows)
            {
                TehsilL.Add(RowOfTehsil(r));
            }
            return TehsilL;
        }
        private List<TehsilModel> LoopinAllPakistanTehsilData(DataTable dt)
        {
            List<TehsilModel> TehsilL = new List<TehsilModel>();

            foreach (DataRow r in dt.Rows)
            {
                TehsilL.Add(RowOfAllPakistanTehsil(r));
            }
            return TehsilL;
        }

        public List<TehsilModel> FetchTehsil(TehsilModel mod)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Tehsil", Common.GetParams(mod)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<TehsilModel> FetchTehsil()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Tehsil").Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<TehsilModel> FetchTehsil(bool InActive)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Tehsil", new SqlParameter("@InActive", InActive)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        
        public List<TehsilModel> FetchAllPakistanTehsil(bool InActive)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_AllPakistan_Tehsil", new SqlParameter("@InActive", InActive)).Tables[0];
                return LoopinAllPakistanTehsilData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public int BatchInsert(List<TehsilModel> ls, int @BatchFkey, int CurUserID)
        {
            SqlParameter[] param = new SqlParameter[3];

            param[0] = new SqlParameter("@Json", JsonConvert.SerializeObject(ls));
            param[1] = new SqlParameter("@", @BatchFkey);
            param[2] = new SqlParameter("@CurUserID", CurUserID);
            return SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[BAU_Tehsil]", param);
        }

        public List<TehsilModel> GetByDistrictID(int DistrictID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Tehsil", new SqlParameter("@DistrictID", DistrictID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<TehsilModel> GetByTehsilName(string TehsilName)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Tehsil", new SqlParameter("@TehsilName", TehsilName)).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    List<TehsilModel> tehsil = LoopinData(dt);

                    return tehsil;
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<TehsilModel> GetByDistrictIDApi(int DistrictID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TehsilApi", new SqlParameter("@DistrictID", DistrictID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public void ActiveInActive(int TehsilID, bool? InActive, int CurUserID)
        {
            SqlParameter[] PLead = new SqlParameter[3];
            PLead[0] = new SqlParameter("@TehsilID", TehsilID);
            PLead[1] = new SqlParameter("@InActive", InActive);
            PLead[2] = new SqlParameter("@CurUserID", CurUserID);
            SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AI_Tehsil]", PLead);
        }

        private TehsilModel RowOfTehsil(DataRow r)
        {
            TehsilModel Tehsil = new TehsilModel();
            Tehsil.TehsilID = Convert.ToInt32(r["TehsilID"]);
            Tehsil.TehsilName = r["TehsilName"].ToString();
            Tehsil.DistrictID = Convert.ToInt32(r["DistrictID"]);
            Tehsil.DistrictNameUrdu = r["DistrictNameUrdu"].ToString();
            Tehsil.DistrictName = r["DistrictName"].ToString();
            Tehsil.InActive = Convert.ToBoolean(r["InActive"]);
            Tehsil.CreatedUserID = Convert.ToInt32(r["CreatedUserID"]);
            Tehsil.ModifiedUserID = Convert.ToInt32(r["ModifiedUserID"]);
            Tehsil.CreatedDate = r["CreatedDate"].ToString().GetDate();
            Tehsil.ModifiedDate = r["ModifiedDate"].ToString().GetDate();

            return Tehsil;
        }
        
        private TehsilModel RowOfAllPakistanTehsil(DataRow r)
        {
            TehsilModel Tehsil = new TehsilModel();
            Tehsil.TehsilID = Convert.ToInt32(r["TehsilID"]);
            Tehsil.TehsilName = r["TehsilName"].ToString();
            if (r.Table.Columns.Contains("DistrictID"))
            {
                Tehsil.DistrictID = Convert.ToInt32(r["DistrictID"]);
            }
            if (r.Table.Columns.Contains("DistrictName"))
            {
                Tehsil.DistrictName = r["DistrictName"].ToString();
            }
            Tehsil.InActive = Convert.ToBoolean(r["InActive"]);
            Tehsil.CreatedUserID = Convert.ToInt32(r["CreatedUserID"]);
            Tehsil.ModifiedUserID = Convert.ToInt32(r["ModifiedUserID"]);
            Tehsil.CreatedDate = r["CreatedDate"].ToString().GetDate();
            Tehsil.ModifiedDate = r["ModifiedDate"].ToString().GetDate();

            return Tehsil;
        }
    }
}