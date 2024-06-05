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
    public class SRVPlacementType : SRVBase, ISRVPlacementType
    {
        public SRVPlacementType() { }
        public PlacementTypeModel GetByPlacementTypeID(int PlacementTypeID)
        {
            try
            {
                SqlParameter param = new SqlParameter("@PlacementTypeID", PlacementTypeID);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_PlacementType", param).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    return RowOfPlacementType(dt.Rows[0]);
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public List<PlacementTypeModel> SavePlacementType(PlacementTypeModel PlacementType)
        {
            try
            {

                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@PlacementTypeID", PlacementType.PlacementTypeID);
                param[1] = new SqlParameter("@PlacementType", PlacementType.PlacementType);

                param[2] = new SqlParameter("@CurUserID", PlacementType.CurUserID);
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_PlacementType]", param);
                return FetchPlacementType();
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }
        private List<PlacementTypeModel> LoopinData(DataTable dt)
        {
            List<PlacementTypeModel> PlacementTypeL = new List<PlacementTypeModel>();

            foreach (DataRow r in dt.Rows)
            {
                PlacementTypeL.Add(RowOfPlacementType(r));

            }
            return PlacementTypeL;
        }
        public List<PlacementTypeModel> FetchPlacementType(PlacementTypeModel mod)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_PlacementType", Common.GetParams(mod)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<PlacementTypeModel> FetchPlacementType()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_PlacementType").Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public List<PlacementTypeModel> FetchPlacementType(bool InActive)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_PlacementType", new SqlParameter("@InActive", InActive)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public void ActiveInActive(int PlacementTypeID, bool? InActive, int CurUserID)
        {

            SqlParameter[] PLead = new SqlParameter[3];
            PLead[0] = new SqlParameter("@PlacementTypeID", PlacementTypeID);
            PLead[1] = new SqlParameter("@InActive", InActive);
            PLead[2] = new SqlParameter("@CurUserID", CurUserID);
            SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AI_PlacementType]", PLead);
        }
        private PlacementTypeModel RowOfPlacementType(DataRow r)
        {
            PlacementTypeModel PlacementType = new PlacementTypeModel();
            PlacementType.PlacementTypeID = Convert.ToInt32(r["PlacementTypeID"]);
            PlacementType.PlacementType = r["PlacementType"].ToString();
            PlacementType.InActive = Convert.ToBoolean(r["InActive"]);
            PlacementType.CreatedUserID = Convert.ToInt32(r["CreatedUserID"]);
            PlacementType.ModifiedUserID = Convert.ToInt32(r["ModifiedUserID"]);
            PlacementType.CreatedDate = r["CreatedDate"].ToString().GetDate();
            PlacementType.ModifiedDate = r["ModifiedDate"].ToString().GetDate();

            return PlacementType;
        }
    }
}
