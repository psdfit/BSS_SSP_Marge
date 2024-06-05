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
    public class SRVTradeConsumableMaterialMap : SRVBase, ISRVTradeConsumableMaterialMap
    {
        public SRVTradeConsumableMaterialMap() { }
        public TradeConsumableMaterialMapModel GetByTradeConsumableMaterialMapID(int TradeConsumableMaterialMapID)
        {
            try
            {
                SqlParameter param = new SqlParameter("@TradeConsumableMaterialMapID", TradeConsumableMaterialMapID);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TradeConsumableMaterialMap", param).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    return RowOfTradeConsumableMaterialMap(dt.Rows[0]);
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public List<TradeConsumableMaterialMapModel> SaveTradeConsumableMaterialMap(TradeConsumableMaterialMapModel TradeConsumableMaterialMap)
        {
            try
            {

                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@TradeConsumableMaterialMapID", TradeConsumableMaterialMap.TradeConsumableMaterialMapID);
                param[1] = new SqlParameter("@TradeDurationMapID", TradeConsumableMaterialMap.TradeDurationMapID);
                param[2] = new SqlParameter("@ConsumableMaterialID", TradeConsumableMaterialMap.ConsumableMaterialID);

                param[3] = new SqlParameter("@CurUserID", TradeConsumableMaterialMap.CurUserID);
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_TradeConsumableMaterialMap]", param);
                return FetchTradeConsumableMaterialMap();
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }
        private List<TradeConsumableMaterialMapModel> LoopinData(DataTable dt)
        {
            List<TradeConsumableMaterialMapModel> TradeConsumableMaterialMapL = new List<TradeConsumableMaterialMapModel>();

            foreach (DataRow r in dt.Rows)
            {
                TradeConsumableMaterialMapL.Add(RowOfTradeConsumableMaterialMap(r));

            }
            return TradeConsumableMaterialMapL;
        }
        public List<TradeConsumableMaterialMapModel> FetchTradeConsumableMaterialMap(TradeConsumableMaterialMapModel mod)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TradeConsumableMaterialMap", Common.GetParams(mod)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<TradeConsumableMaterialMapModel> FetchTradeConsumableMaterialMap()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TradeConsumableMaterialMap").Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public List<TradeConsumableMaterialMapModel> FetchTradeConsumableMaterialMap(bool InActive)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TradeConsumableMaterialMap", new SqlParameter("@InActive", InActive)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }

        public List<TradeConsumableMaterialMapModel> FetchTradeConsumableMaterialMapAll(int TradeID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TradeConsumableMaterialMapAll", new SqlParameter("@TradeID", TradeID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<TradeConsumableMaterialMapModel> GetByTradeID(int TradeID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TradeConsumableMaterialMap", new SqlParameter("@TradeID", TradeID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<TradeConsumableMaterialMapModel> GetByConsumableMaterialID(int ConsumableMaterialID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TradeConsumableMaterialMap", new SqlParameter("@ConsumableMaterialID", ConsumableMaterialID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public int BatchInsert(List<TradeConsumableMaterialMapModel> ls, int @BatchFkey, int CurUserID)
        {
            SqlParameter[] param = new SqlParameter[3];

            param[0] = new SqlParameter("@Json", JsonConvert.SerializeObject(ls));
            param[1] = new SqlParameter("@TradeDurationMapID", @BatchFkey);
            param[2] = new SqlParameter("@CurUserID", CurUserID);
            return SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[BAU_TradeConsumableMaterialMap]", param);
        }


        public void ActiveInActive(int TradeConsumableMaterialMapID, bool? InActive, int CurUserID)
        {

            SqlParameter[] PLead = new SqlParameter[3];
            PLead[0] = new SqlParameter("@TradeConsumableMaterialMapID", TradeConsumableMaterialMapID);
            PLead[1] = new SqlParameter("@InActive", InActive);
            PLead[2] = new SqlParameter("@CurUserID", CurUserID);
            SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AI_TradeConsumableMaterialMap]", PLead);
        }
        private TradeConsumableMaterialMapModel RowOfTradeConsumableMaterialMap(DataRow r)
        {
            TradeConsumableMaterialMapModel TradeConsumableMaterialMap = new TradeConsumableMaterialMapModel();
            TradeConsumableMaterialMap.TradeConsumableMaterialMapID = Convert.ToInt32(r["TradeConsumableMaterialMapID"]);
            TradeConsumableMaterialMap.TradeDurationMapID = Convert.ToInt32(r["TradeDurationMapID"]);
            //TradeConsumableMaterialMap.TradeName = r["TradeName"].ToString();
            TradeConsumableMaterialMap.ConsumableMaterialID = Convert.ToInt32(r["ConsumableMaterialID"]);
            TradeConsumableMaterialMap.ItemName = r["ItemName"].ToString();
            TradeConsumableMaterialMap.InActive = Convert.ToBoolean(r["InActive"]);
            TradeConsumableMaterialMap.CreatedUserID = Convert.ToInt32(r["CreatedUserID"]);
            TradeConsumableMaterialMap.ModifiedUserID = Convert.ToInt32(r["ModifiedUserID"]);
            TradeConsumableMaterialMap.CreatedDate = r["CreatedDate"].ToString().GetDate();
            TradeConsumableMaterialMap.ModifiedDate = r["ModifiedDate"].ToString().GetDate();

            return TradeConsumableMaterialMap;
        }
    }
}
