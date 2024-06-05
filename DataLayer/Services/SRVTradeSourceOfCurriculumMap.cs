/* **** Aamer Rehman Malik *****/

using DataLayer.Classes;
using DataLayer.Interfaces;
using DataLayer.Models;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;

namespace DataLayer.Services
{
    public class SRVTradeSourceOfCurriculumMap : SRVBase, ISRVTradeSourceOfCurriculumMap
    {
        public SRVTradeSourceOfCurriculumMap()
        {
        }

        public TradeSourceOfCurriculumMapModel GetByTradeSourceOfCurriculumMapID(int TradeSourceOfCurriculumMapID)
        {
            try
            {
                SqlParameter param = new SqlParameter("@TradeSourceOfCurriculumMapID", TradeSourceOfCurriculumMapID);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TradeSourceOfCurriculumMap", param).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    return RowOfTradeSourceOfCurriculumMap(dt.Rows[0]);
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<TradeSourceOfCurriculumMapModel> SaveTradeSourceOfCurriculumMap(TradeSourceOfCurriculumMapModel TradeSourceOfCurriculumMap)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@TradeSourceOfCurriculumMapID", TradeSourceOfCurriculumMap.TradeSourceOfCurriculumMapID);
                param[1] = new SqlParameter("@TradeID", TradeSourceOfCurriculumMap.TradeID);
                param[2] = new SqlParameter("@SourceOfCurriculumID", TradeSourceOfCurriculumMap.SourceOfCurriculumID);

                param[3] = new SqlParameter("@CurUserID", TradeSourceOfCurriculumMap.CurUserID);
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_TradeSourceOfCurriculumMap]", param);
                return FetchTradeSourceOfCurriculumMap();
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }

        private List<TradeSourceOfCurriculumMapModel> LoopinData(DataTable dt)
        {
            List<TradeSourceOfCurriculumMapModel> TradeSourceOfCurriculumMapL = new List<TradeSourceOfCurriculumMapModel>();

            foreach (DataRow r in dt.Rows)
            {
                TradeSourceOfCurriculumMapL.Add(RowOfTradeSourceOfCurriculumMap(r));
            }
            return TradeSourceOfCurriculumMapL;
        }

        public List<TradeSourceOfCurriculumMapModel> FetchTradeSourceOfCurriculumMap(TradeSourceOfCurriculumMapModel mod)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TradeSourceOfCurriculumMap", Common.GetParams(mod)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<TradeSourceOfCurriculumMapModel> FetchTradeSourceOfCurriculumMap()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TradeSourceOfCurriculumMap").Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<TradeSourceOfCurriculumMapModel> FetchTradeSourceOfCurriculumMap(bool InActive)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TradeSourceOfCurriculumMap", new SqlParameter("@InActive", InActive)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<TradeSourceOfCurriculumMapModel> FetchTradeSourceOfCurriculumMapAll(int TradeID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TradeSourceOfCurriculumMapAll", new SqlParameter("@TradeID", TradeID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<TradeSourceOfCurriculumMapModel> GetByTradeID(int TradeID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TradeSourceOfCurriculumMap", new SqlParameter("@TradeID", TradeID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<TradeSourceOfCurriculumMapModel> GetBySourceOfCurriculumID(int SourceOfCurriculumID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TradeSourceOfCurriculumMap", new SqlParameter("@SourceOfCurriculumID", SourceOfCurriculumID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public int BatchInsert(List<TradeSourceOfCurriculumMapModel> ls, int @BatchFkey, int CurUserID)
        {
            SqlParameter[] param = new SqlParameter[3];

            param[0] = new SqlParameter("@Json", JsonConvert.SerializeObject(ls));
            param[1] = new SqlParameter("@TradeID", @BatchFkey);
            param[2] = new SqlParameter("@CurUserID", CurUserID);
            return SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[BAU_TradeSourceOfCurriculumMap]", param);
        }

        public void ActiveInActive(int TradeSourceOfCurriculumMapID, bool? InActive, int CurUserID)
        {
            SqlParameter[] PLead = new SqlParameter[3];
            PLead[0] = new SqlParameter("@TradeSourceOfCurriculumMapID", TradeSourceOfCurriculumMapID);
            PLead[1] = new SqlParameter("@InActive", InActive);
            PLead[2] = new SqlParameter("@CurUserID", CurUserID);
            SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AI_TradeSourceOfCurriculumMap]", PLead);
        }

        private TradeSourceOfCurriculumMapModel RowOfTradeSourceOfCurriculumMap(DataRow r)
        {
            TradeSourceOfCurriculumMapModel TradeSourceOfCurriculumMap = new TradeSourceOfCurriculumMapModel();
            TradeSourceOfCurriculumMap.TradeSourceOfCurriculumMapID = Convert.ToInt32(r["TradeSourceOfCurriculumMapID"]);
            TradeSourceOfCurriculumMap.TradeID = Convert.ToInt32(r["TradeID"]);
            //TradeSourceOfCurriculumMap.TradeName = r["TradeName"].ToString();
            TradeSourceOfCurriculumMap.SourceOfCurriculumID = Convert.ToInt32(r["SourceOfCurriculumID"]);
            TradeSourceOfCurriculumMap.Name = r["Name"].ToString();
            TradeSourceOfCurriculumMap.InActive = Convert.ToBoolean(r["InActive"]);
            TradeSourceOfCurriculumMap.CreatedUserID = Convert.ToInt32(r["CreatedUserID"]);
            TradeSourceOfCurriculumMap.ModifiedUserID = Convert.ToInt32(r["ModifiedUserID"]);
            TradeSourceOfCurriculumMap.CreatedDate = r["CreatedDate"].ToString().GetDate();
            TradeSourceOfCurriculumMap.ModifiedDate = r["ModifiedDate"].ToString().GetDate();

            return TradeSourceOfCurriculumMap;
        }
    }
}