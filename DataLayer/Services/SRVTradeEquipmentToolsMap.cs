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
    public class SRVTradeEquipmentToolsMap : SRVBase, ISRVTradeEquipmentToolsMap
    {
        public SRVTradeEquipmentToolsMap() { }
        public TradeEquipmentToolsMapModel GetByTradeEquipmentToolsMap(int TradeEquipmentToolsMap)
        {
            try
            {
                SqlParameter param = new SqlParameter("@TradeEquipmentToolsMap", TradeEquipmentToolsMap);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TradeEquipmentToolsMap", param).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    return RowOfTradeEquipmentToolsMap(dt.Rows[0]);
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public List<TradeEquipmentToolsMapModel> SaveTradeEquipmentToolsMap(TradeEquipmentToolsMapModel TradeEquipmentToolsMap)
        {
            try
            {

                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@TradeEquipmentToolsMapID", TradeEquipmentToolsMap.TradeEquipmentToolsMapID);
                param[1] = new SqlParameter("@TradeDurationMapID", TradeEquipmentToolsMap.TradeDurationMapID);
                param[2] = new SqlParameter("@EquipmentToolID", TradeEquipmentToolsMap.EquipmentToolID);

                param[3] = new SqlParameter("@CurUserID", TradeEquipmentToolsMap.CurUserID);
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_TradeEquipmentToolsMap]", param);
                return FetchTradeEquipmentToolsMap();
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }
        private List<TradeEquipmentToolsMapModel> LoopinData(DataTable dt)
        {
            List<TradeEquipmentToolsMapModel> TradeEquipmentToolsMapL = new List<TradeEquipmentToolsMapModel>();

            foreach (DataRow r in dt.Rows)
            {
                TradeEquipmentToolsMapL.Add(RowOfTradeEquipmentToolsMap(r));

            }
            return TradeEquipmentToolsMapL;
        }
        public List<TradeEquipmentToolsMapModel> FetchTradeEquipmentToolsMap(TradeEquipmentToolsMapModel mod)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TradeEquipmentToolsMap", Common.GetParams(mod)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<TradeEquipmentToolsMapModel> FetchTradeEquipmentToolsMap()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TradeEquipmentToolsMap").Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public List<TradeEquipmentToolsMapModel> FetchTradeEquipmentToolsMap(bool InActive)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TradeEquipmentToolsMap", new SqlParameter("@InActive", InActive)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }

        public List<TradeEquipmentToolsMapModel> FetchTradeEquipmentToolsMapAll(int TradeID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TradeEquipmentToolsMapAll", new SqlParameter("@TradeID", TradeID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<TradeEquipmentToolsMapModel> GetByTradeID(int TradeID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TradeEquipmentToolsMap", new SqlParameter("@TradeID", TradeID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<TradeEquipmentToolsMapModel> GetByEquipmentToolID(int EquipmentToolID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TradeEquipmentToolsMap", new SqlParameter("@EquipmentToolID", EquipmentToolID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public int BatchInsert(List<TradeEquipmentToolsMapModel> ls, int @BatchFkey, int CurUserID)
        {
            SqlParameter[] param = new SqlParameter[3];

            param[0] = new SqlParameter("@Json", JsonConvert.SerializeObject(ls));
            param[1] = new SqlParameter("@TradeDurationMapID", @BatchFkey);
            param[2] = new SqlParameter("@CurUserID", CurUserID);
            return SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[BAU_TradeEquipmentToolsMap]", param);
        }

        public void ActiveInActive(int TradeEquipmentToolsMap, bool? InActive, int CurUserID)
        {

            SqlParameter[] PLead = new SqlParameter[3];
            PLead[0] = new SqlParameter("@TradeEquipmentToolsMap", TradeEquipmentToolsMap);
            PLead[1] = new SqlParameter("@InActive", InActive);
            PLead[2] = new SqlParameter("@CurUserID", CurUserID);
            SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AI_TradeEquipmentToolsMap]", PLead);
        }
        private TradeEquipmentToolsMapModel RowOfTradeEquipmentToolsMap(DataRow r)
        {
            TradeEquipmentToolsMapModel TradeEquipmentToolsMap = new TradeEquipmentToolsMapModel();
            TradeEquipmentToolsMap.TradeEquipmentToolsMapID = Convert.ToInt32(r["TradeEquipmentToolsMapID"]);
            TradeEquipmentToolsMap.TradeDurationMapID = Convert.ToInt32(r["TradeDurationMapID"]);
            //TradeEquipmentToolsMap.TradeName = r["TradeName"].ToString();
            TradeEquipmentToolsMap.EquipmentToolID = Convert.ToInt32(r["EquipmentToolID"]);
            TradeEquipmentToolsMap.EquipmentName = r["EquipmentName"].ToString();
            TradeEquipmentToolsMap.InActive = Convert.ToBoolean(r["InActive"]);
            TradeEquipmentToolsMap.CreatedUserID = Convert.ToInt32(r["CreatedUserID"]);
            TradeEquipmentToolsMap.ModifiedUserID = Convert.ToInt32(r["ModifiedUserID"]);
            TradeEquipmentToolsMap.CreatedDate = r["CreatedDate"].ToString().GetDate();
            TradeEquipmentToolsMap.ModifiedDate = r["ModifiedDate"].ToString().GetDate();

            return TradeEquipmentToolsMap;
        }
    }
}
