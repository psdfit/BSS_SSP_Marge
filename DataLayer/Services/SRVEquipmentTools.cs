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
    public class SRVEquipmentTools : SRVBase, ISRVEquipmentTools
    {
        public SRVEquipmentTools() { }
        public EquipmentToolsModel GetByEquipmentToolID(int EquipmentToolID)
        {
            try
            {
                SqlParameter param = new SqlParameter("@EquipmentToolID", EquipmentToolID);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_EquipmentTools", param).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    return RowOfEquipmentTools(dt.Rows[0]);
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public List<EquipmentToolsModel> SaveEquipmentTools(EquipmentToolsModel EquipmentTools)
        {
            try
            {

                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@EquipmentToolID", EquipmentTools.EquipmentToolID);
                param[1] = new SqlParameter("@EquipmentName", EquipmentTools.EquipmentName);
                param[2] = new SqlParameter("@EquipmentQuantity", EquipmentTools.EquipmentQuantity);
                //param[3] = new SqlParameter("@CertAuthID", EquipmentTools.CertAuthID);

                param[3] = new SqlParameter("@CurUserID", EquipmentTools.CurUserID);
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_EquipmentTools]", param);
                return FetchEquipmentTools();
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }
        private List<EquipmentToolsModel> LoopinData(DataTable dt)
        {
            List<EquipmentToolsModel> EquipmentToolsL = new List<EquipmentToolsModel>();

            foreach (DataRow r in dt.Rows)
            {
                EquipmentToolsL.Add(RowOfEquipmentTools(r));

            }
            return EquipmentToolsL;
        }
        public List<EquipmentToolsModel> FetchEquipmentTools(EquipmentToolsModel mod)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_EquipmentTools", Common.GetParams(mod)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<EquipmentToolsModel> FetchEquipmentTools()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_EquipmentTools").Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public List<EquipmentToolsModel> FetchEquipmentTools(bool InActive)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_EquipmentTools", new SqlParameter("@InActive", InActive)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public void ActiveInActive(int EquipmentToolID, bool? InActive, int CurUserID)
        {

            SqlParameter[] PLead = new SqlParameter[3];
            PLead[0] = new SqlParameter("@EquipmentToolID", EquipmentToolID);
            PLead[1] = new SqlParameter("@InActive", InActive);
            PLead[2] = new SqlParameter("@CurUserID", CurUserID);
            SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AI_EquipmentTools]", PLead);
        }
        private EquipmentToolsModel RowOfEquipmentTools(DataRow r)
        {
            EquipmentToolsModel EquipmentTools = new EquipmentToolsModel();
            EquipmentTools.EquipmentToolID = Convert.ToInt32(r["EquipmentToolID"]);
            EquipmentTools.EquipmentName = r["EquipmentName"].ToString();
            EquipmentTools.EquipmentQuantity = Convert.ToInt32(r["EquipmentQuantity"]);
            //EquipmentTools.CertAuthID = Convert.ToInt32(r["CertAuthID"]);
            //EquipmentTools.CertAuthName = r["CertAuthName"].ToString();
            EquipmentTools.InActive = Convert.ToBoolean(r["InActive"]);
            EquipmentTools.CreatedUserID = Convert.ToInt32(r["CreatedUserID"]);
            EquipmentTools.ModifiedUserID = Convert.ToInt32(r["ModifiedUserID"]);
            EquipmentTools.CreatedDate = r["CreatedDate"].ToString().GetDate();
            EquipmentTools.ModifiedDate = r["ModifiedDate"].ToString().GetDate();

            return EquipmentTools;
        }
    }
}
