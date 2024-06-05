using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using DataLayer.Classes;
using DataLayer.Models;
using DataLayer.Interfaces;
using Newtonsoft.Json;
namespace DataLayer.Services
{
    public class SRVPotentialTrainees : SRVBase, ISRVPotentialTrainees
    {
        public SRVPotentialTrainees() { }
        public PotentialTraineesModel GetByID(int ID)
        {
            try
            {
                SqlParameter param = new SqlParameter("@ID", ID);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_PotentialTrainees", param).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    return RowOfPotentialTrainees(dt.Rows[0]);
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public List<PotentialTraineesModel> SavePotentialTrainees(PotentialTraineesModel PotentialTrainees)
        {
            try
            {

                SqlParameter[] param = new SqlParameter[4];
                //param[0] = new SqlParameter("@ID", PotentialTrainees.ID);
                param[1] = new SqlParameter("@DistrictID", PotentialTrainees.DistrictID);            
                param[5] = new SqlParameter("@TradeID", PotentialTrainees.TradeID);             
                param[8] = new SqlParameter("@TehsilID", PotentialTrainees.TehsilID);

                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_PotentialTrainees]", param);
                return FetchPotentialTrainees();
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }

        public List<PotentialTraineesModel> FetchPotentialTraineesByPaged(PagingModel pagingModel, SearchFilter filterModel, out int totalCount)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@TradeID", filterModel.TradeID));
                param.Add(new SqlParameter("@DistrictID", filterModel.DistrictID));
                param.Add(new SqlParameter("@TehsilID", filterModel.TehsilID));
                param.Add(new SqlParameter("@ClassID", filterModel.ClassID));
                param.Add(new SqlParameter("@UserID", filterModel.UserID));

                param.AddRange(Common.GetPagingParams(pagingModel));

                DataTable dt = new DataTable();
                dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_PotentialTrainees_Paged", param.ToArray()).Tables[0];
                if (dt.Rows.Count > 0)
                    totalCount = Convert.ToInt32(dt.Rows[0]["TotalRows"]);
                else
                    totalCount = 0;
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }


        private List<PotentialTraineesModel> LoopinData(DataTable dt)
        {
            List<PotentialTraineesModel> PotentialTraineesL = new List<PotentialTraineesModel>();

            foreach (DataRow r in dt.Rows)
            {
                PotentialTraineesL.Add(RowOfPotentialTrainees(r));

            }
            return PotentialTraineesL;
        }
        
        private List<PotentialTraineesModel> LoopinFiltersData(DataTable dt)
        {
            List<PotentialTraineesModel> PotentialTraineesL = new List<PotentialTraineesModel>();

            foreach (DataRow r in dt.Rows)
            {
                PotentialTraineesL.Add(RowOfPotentialTraineesFilters(r));

            }
            return PotentialTraineesL;
        }

        public List<PotentialTraineesModel> FetchPotentialTraineesFiltersData(int userID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_PotentialTrainees_FiltersData", new SqlParameter("@UserID", userID)).Tables[0];
                return LoopinFiltersData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<PotentialTraineesModel> FetchPotentialTrainees()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_PotentialTraineess").Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        
        public List<PotentialTraineesModel> GetByDistrictID(int DistrictID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_PotentialTrainees", new SqlParameter("@DistrictID", DistrictID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<PotentialTraineesModel> GetBySchemeID(int SchemeID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_PotentialTrainees", new SqlParameter("@SchemeID", SchemeID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<PotentialTraineesModel> GetByTSPID(int TSPID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_PotentialTrainees", new SqlParameter("@TSPID", TSPID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<PotentialTraineesModel> GetByTradeID(int TradeID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_PotentialTrainees", new SqlParameter("@TradeID", TradeID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<PotentialTraineesModel> GetByTehsilID(int TehsilID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_PotentialTrainees", new SqlParameter("@TehsilID", TehsilID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<PotentialTraineesModel> GetByGenderID(int GenderID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_PotentialTrainees", new SqlParameter("@GenderID", GenderID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<PotentialTraineesModel> GetBySectorID(int SectorID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_PotentialTrainees", new SqlParameter("@SectorID", SectorID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<PotentialTraineesModel> GetByClusterID(int ClusterID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_PotentialTrainees", new SqlParameter("@ClusterID", ClusterID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public void ActiveInActive(int ID, bool? InActive, int CurUserID)
        {

            SqlParameter[] PLead = new SqlParameter[3];
            PLead[0] = new SqlParameter("@ID", ID);
            PLead[1] = new SqlParameter("@InActive", InActive);
            PLead[2] = new SqlParameter("@CurUserID", CurUserID);
            SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AI_PotentialTrainees]", PLead);
        }

        public List<PotentialTraineesModel> FetchPotentialTraineesByFilters(int[] filters)
        {
            List<PotentialTraineesModel> list = new List<PotentialTraineesModel>();
            if (filters.Length > 0)
            {
                int schemeId = filters[0];
                int tspId = filters[1];
                int classId = filters[2];
                int userId = filters[3];
                int oID = filters[4];
                try
                {
                    SqlParameter[] param = new SqlParameter[10];
                    param[0] = new SqlParameter("@SchemeID", schemeId);
                    param[1] = new SqlParameter("@TSPID", tspId);
                    param[2] = new SqlParameter("@ClassID", classId);
                    param[3] = new SqlParameter("@UserID", userId);
                    param[4] = new SqlParameter("@OID", oID);
                    DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_PotentialTraineess", param).Tables[0];
                    list = LoopinData(dt);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return list;
        }


        private PotentialTraineesModel RowOfPotentialTrainees(DataRow r)
        {
            PotentialTraineesModel PotentialTrainees = new PotentialTraineesModel();
            PotentialTrainees.TraineeName = r["TraineeName"].ToString();
            PotentialTrainees.TraineeCNIC = r["TraineeCNIC"].ToString();
            PotentialTrainees.TraineeEmail = r["TraineeEmail"].ToString();
            PotentialTrainees.TraineePhone = r["TraineePhone"].ToString();
            PotentialTrainees.GenderName = r["GenderName"].ToString();
            PotentialTrainees.DistrictID = Convert.ToInt32(r["DistrictID"]);
            PotentialTrainees.DistrictName = r["DistrictName"].ToString();
            PotentialTrainees.TehsilName = r["TehsilName"].ToString();
            PotentialTrainees.TradeName = r["TradeName"].ToString();
            PotentialTrainees.ClassCode = r["ClassCode"].ToString();

            PotentialTrainees.ClassID = Convert.ToInt32(r["ClassID"]);
            PotentialTrainees.TehsilID = Convert.ToInt32(r["TehsilID"]);
            PotentialTrainees.GenderID = Convert.ToInt32(r["GenderID"]);

 
            return PotentialTrainees;
        }

        private PotentialTraineesModel RowOfPotentialTraineesFilters(DataRow r)
        {
            PotentialTraineesModel PotentialTrainees = new PotentialTraineesModel();

            PotentialTrainees.GenderID = Convert.ToInt32(r["GenderID"]);
            PotentialTrainees.GenderName = r["GenderName"].ToString();
            PotentialTrainees.DistrictID = Convert.ToInt32(r["DistrictID"]);
            PotentialTrainees.DistrictName = r["DistrictName"].ToString();
            PotentialTrainees.TehsilID = Convert.ToInt32(r["TehsilID"]);
            PotentialTrainees.TehsilName = r["TehsilName"].ToString();
            PotentialTrainees.TradeID = Convert.ToInt32(r["TradeID"]);
            PotentialTrainees.TradeName = r["TradeName"].ToString();
            PotentialTrainees.ClassID = Convert.ToInt32(r["ClassID"]);
            PotentialTrainees.ClassCode = r["ClassCode"].ToString();

            return PotentialTrainees;
        }
    }
}
