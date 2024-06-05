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
    public class SRVConsumableMaterial : SRVBase, ISRVConsumableMaterial
    {
        public SRVConsumableMaterial() { }
        public ConsumableMaterialModel GetByConsumableMaterialID(int ConsumableMaterialID)
        {
            try
            {
                SqlParameter param = new SqlParameter("@ConsumableMaterialID", ConsumableMaterialID);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_ConsumableMaterial", param).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    return RowOfConsumableMaterial(dt.Rows[0]);
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public List<ConsumableMaterialModel> SaveConsumableMaterial(ConsumableMaterialModel ConsumableMaterial)
        {
            try
            {

                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@ConsumableMaterialID", ConsumableMaterial.ConsumableMaterialID);
                param[1] = new SqlParameter("@ItemName", ConsumableMaterial.ItemName);

                param[2] = new SqlParameter("@CurUserID", ConsumableMaterial.CurUserID);
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_ConsumableMaterial]", param);
                return FetchConsumableMaterial();
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }
        private List<ConsumableMaterialModel> LoopinData(DataTable dt)
        {
            List<ConsumableMaterialModel> ConsumableMaterialL = new List<ConsumableMaterialModel>();

            foreach (DataRow r in dt.Rows)
            {
                ConsumableMaterialL.Add(RowOfConsumableMaterial(r));

            }
            return ConsumableMaterialL;
        }
        public List<ConsumableMaterialModel> FetchConsumableMaterial(ConsumableMaterialModel mod)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_ConsumableMaterial", Common.GetParams(mod)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<ConsumableMaterialModel> FetchConsumableMaterial()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_ConsumableMaterial").Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public List<ConsumableMaterialModel> FetchConsumableMaterial(bool InActive)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_ConsumableMaterial", new SqlParameter("@InActive", InActive)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public void ActiveInActive(int ConsumableMaterialID, bool? InActive, int CurUserID)
        {

            SqlParameter[] PLead = new SqlParameter[3];
            PLead[0] = new SqlParameter("@ConsumableMaterialID", ConsumableMaterialID);
            PLead[1] = new SqlParameter("@InActive", InActive);
            PLead[2] = new SqlParameter("@CurUserID", CurUserID);
            SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AI_ConsumableMaterial]", PLead);
        }
        private ConsumableMaterialModel RowOfConsumableMaterial(DataRow r)
        {
            ConsumableMaterialModel ConsumableMaterial = new ConsumableMaterialModel();
            ConsumableMaterial.ConsumableMaterialID = Convert.ToInt32(r["ConsumableMaterialID"]);
            ConsumableMaterial.ItemName = r["ItemName"].ToString();
            ConsumableMaterial.InActive = Convert.ToBoolean(r["InActive"]);
            ConsumableMaterial.CreatedUserID = Convert.ToInt32(r["CreatedUserID"]);
            ConsumableMaterial.ModifiedUserID = Convert.ToInt32(r["ModifiedUserID"]);
            ConsumableMaterial.CreatedDate = r["CreatedDate"].ToString().GetDate();
            ConsumableMaterial.ModifiedDate = r["ModifiedDate"].ToString().GetDate();

            return ConsumableMaterial;
        }
    }
}
