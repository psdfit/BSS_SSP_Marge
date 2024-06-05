using DataLayer.Interfaces;
using DataLayer.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DataLayer.Services
{
  public  class SRVTSPColor: ISRVTSPColor
    {
        public List<TSPColorModel> FetchTSPMasterData()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "[RD_TSPMasterForTSPColor]").Tables[0];
                List<TSPColorModel> TSPColorModel = new List<TSPColorModel>();
                TSPColorModel = Helper.ConvertDataTableToModel<TSPColorModel>(dt);
                return (TSPColorModel);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<TSPColorModel> FetchTSPColor()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "[RD_TSPColor]").Tables[0];
                List<TSPColorModel> TSPColorModel = new List<TSPColorModel>();
                TSPColorModel = Helper.ConvertDataTableToModel<TSPColorModel>(dt);
                return (TSPColorModel);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public bool saveTSPColor(TSPColorModel model)
        {
            bool bit;
            try
            {
                SqlParameter[] param = new SqlParameter[6];
                param[0] = new SqlParameter("@TSPMasterID", model.TSPMasterID);
                param[1] = new SqlParameter("@CurUserID", model.CurUserID);
                param[3] = new SqlParameter("@TSPColorID", model.TSPColorID);
                param[4] = new SqlParameter("@TSPColorCode", model.TSPColorCode);
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_TSPColor]", param);
                return true;

            }
            catch (Exception ex) { throw new Exception(ex.Message); return false; }
        }
        public List<TSPColorModel> FetchTSPColorHistory(int? TSPMasterID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "[RD_TSPColorHistory]", new SqlParameter("@TSPMasterID", TSPMasterID)).Tables[0];
                List<TSPColorModel> TSPColorModel = new List<TSPColorModel>();
                TSPColorModel = Helper.ConvertDataTableToModel<TSPColorModel>(dt);
                return (TSPColorModel);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public List<TSPColorModel> FetchTSPColorByID(int userid)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "[RD_TSPColorByID]", new SqlParameter("@UserID", userid)).Tables[0];
                List<TSPColorModel> TSPColorModel = new List<TSPColorModel>();
                TSPColorModel = Helper.ConvertDataTableToModel<TSPColorModel>(dt);
                return (TSPColorModel);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public List<TSPColorModel> FetchTSPColorByFilters(TSPColorFiltersModel model)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@TSPID", model.TSPID);
                param[1] = new SqlParameter("@ClassID", model.ClassID);
                param[2] = new SqlParameter("@CurUserID", model.CurUserID);

                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "[RD_TSPColorByFilters]", param).Tables[0];
                List<TSPColorModel> TSPColorModel = new List<TSPColorModel>();
                TSPColorModel = Helper.ConvertDataTableToModel<TSPColorModel>(dt);
                return (TSPColorModel);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public List<BlackListCriteriaModel> CheckBlacklistingCriteriaCriteria(TSPColorFiltersModel model)

        {
            try
            {
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@TSPID", model.TSPID);
                param[1] = new SqlParameter("@ClassID", model.ClassID);
                //param[2] = new SqlParameter("@CurUserID", model.CurUserID);

                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TSPColorByFilters", param).Tables[0];
                List<BlackListCriteriaModel> BlacklistModel = new List<BlackListCriteriaModel>();
                BlacklistModel = Helper.ConvertDataTableToModel<BlackListCriteriaModel>(dt);
                return (BlacklistModel);

                //DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TSPColorByFilters", param).Tables[0];
                //if (dt.Rows.Count > 0)
                //{
                //    return LoopinCheckBlacklistingCriteria(dt);
                //}
                //else
                //    return new List<BlackListCriteriaModel>();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        
        }
        private List<BlackListCriteriaModel> LoopinCheckBlacklistingCriteria(DataTable dt)
        {
            List<BlackListCriteriaModel> list = new List<BlackListCriteriaModel>();

            foreach (DataRow r in dt.Rows)
            {
                list.Add(RowOfheckBlacklistingCriteria(r));
            }
            return list;
        }
        private BlackListCriteriaModel RowOfheckBlacklistingCriteria(DataRow r)
        {
            BlackListCriteriaModel model = new BlackListCriteriaModel();
            model.ErrorMessage = r.Field<string>("ErrorMessage");
            model.ErrorTypeID = r.Field<int>("ErrorTypeID");
            model.ErrorTypeName = r.Field<string>("ErrorTypeName");
            return model;
        }

    }
}
