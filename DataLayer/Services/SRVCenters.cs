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
    public class SRVCenters : SRVBase, ISRVCenters
    {
        public SRVCenters() { }
        public CentersModel GetByCenterID(int CenterID)
        {
            try
            {
                SqlParameter param = new SqlParameter("@CenterID", CenterID);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Centers", param).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    return RowOfCenters(dt.Rows[0]);
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public List<CentersModel> SaveCenters(CentersModel Centers)
        {
            try
            {

                SqlParameter[] param = new SqlParameter[11];
                param[0] = new SqlParameter("@CenterID", Centers.CenterID);
                param[1] = new SqlParameter("@CenterName", Centers.CenterName);
                param[2] = new SqlParameter("@CenterAddress", Centers.CenterAddress);
                //param[3] = new SqlParameter("@CenterGeoLocation", Centers.CenterGeoLocation);
                param[4] = new SqlParameter("@CenterDistrict", Centers.CenterDistrict);
                param[5] = new SqlParameter("@CenterTehsil", Centers.CenterTehsil);
                param[6] = new SqlParameter("@CenterInchargeName", Centers.CenterInchargeName);
                param[7] = new SqlParameter("@CenterInchargeMobile", Centers.CenterInchargeMobile);
                //param[8] = new SqlParameter("@UID", Centers.UID);
                //param[9] = new SqlParameter("@IsMigrated", Centers.IsMigrated);

                param[10] = new SqlParameter("@CurUserID", Centers.CurUserID);
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_Centers]", param);
                return FetchCenters();
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }
        private List<CentersModel> LoopinData(DataTable dt)
        {
            List<CentersModel> CentersL = new List<CentersModel>();

            foreach (DataRow r in dt.Rows)
            {
                CentersL.Add(RowOfCenters(r));

            }
            return CentersL;
        }
        public List<CentersModel> FetchCenters(CentersModel mod)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Centers", Common.GetParams(mod)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<CentersModel> FetchCenters()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Centers").Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public List<CentersModel> FetchCenters(bool InActive)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Centers", new SqlParameter("@InActive", InActive)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public void ActiveInActive(int CenterID, bool? InActive, int CurUserID)
        {

            SqlParameter[] PLead = new SqlParameter[3];
            PLead[0] = new SqlParameter("@CenterID", CenterID);
            PLead[1] = new SqlParameter("@InActive", InActive);
            PLead[2] = new SqlParameter("@CurUserID", CurUserID);
            SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AI_Centers]", PLead);
        }
        private CentersModel RowOfCenters(DataRow r)
        {
            CentersModel Centers = new CentersModel();
            Centers.CenterID = Convert.ToInt32(r["CenterID"]);
            Centers.CenterName = r["CenterName"].ToString();
            Centers.CenterAddress = r["CenterAddress"].ToString();
            Centers.CenterGeoLocation = r["CenterGeoLocation"].ToString();
            Centers.DistrictName = r["DistrictName"].ToString();
            Centers.TehsilName = r["TehsilName"].ToString();
            Centers.CenterDistrict = Convert.ToInt32(r["CenterDistrict"]);
            Centers.CenterTehsil = Convert.ToInt32(r["CenterTehsil"]);
            Centers.CenterInchargeName = r["CenterInchargeName"].ToString();
            Centers.CenterInchargeMobile = r["CenterInchargeMobile"].ToString();
            Centers.InActive = Convert.ToBoolean(r["InActive"]);
            Centers.CreatedUserID = Convert.ToInt32(r["CreatedUserID"]);
            Centers.ModifiedUserID = Convert.ToInt32(r["ModifiedUserID"]);
            Centers.CreatedDate = r["CreatedDate"].ToString().GetDate();
            Centers.ModifiedDate = r["ModifiedDate"].ToString().GetDate();
            Centers.UID = r["UID"].ToString();
            Centers.IsMigrated = Convert.ToBoolean(r["IsMigrated"]);

            return Centers;
        }
    }
}
