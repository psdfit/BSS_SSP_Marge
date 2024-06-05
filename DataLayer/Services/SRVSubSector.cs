using DataLayer.Classes;
using DataLayer.Models;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;

namespace DataLayer.Services
{
    public class SRVSubSector : SRVBase, DataLayer.Interfaces.ISRVSubSector
    {
        public SRVSubSector()
        {
        }

        public SubSectorModel GetBySubSectorID(int SubSectorID)
        {
            try
            {
                SqlParameter param = new SqlParameter("@SubSectorID", SubSectorID);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_SubSector", param).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    return RowOfSubSector(dt.Rows[0]);
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<SubSectorModel> SaveSubSector(SubSectorModel SubSector)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@SubSectorID", SubSector.SubSectorID);
                param[1] = new SqlParameter("@SubSectorName", SubSector.SubSectorName);
                param[2] = new SqlParameter("@SectorID", SubSector.SectorID);

                param[3] = new SqlParameter("@CurUserID", SubSector.CurUserID);
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_SubSector]", param);
                return FetchSubSector();
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }

        private List<SubSectorModel> LoopinData(DataTable dt)
        {
            List<SubSectorModel> SubSectorL = new List<SubSectorModel>();

            foreach (DataRow r in dt.Rows)
            {
                SubSectorL.Add(RowOfSubSector(r));
            }
            return SubSectorL;
        }

        public List<SubSectorModel> FetchSubSector(SubSectorModel mod)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_SubSector", Common.GetParams(mod)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<SubSectorModel> FetchSubSector()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_SubSector").Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<SubSectorModel> FetchSubSector(bool InActive)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_SubSector", new SqlParameter("@InActive", InActive)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public int BatchInsert(List<SubSectorModel> ls, int @BatchFkey, int CurUserID)
        {
            SqlParameter[] param = new SqlParameter[3];

            param[0] = new SqlParameter("@Json", JsonConvert.SerializeObject(ls));
            param[1] = new SqlParameter("@", @BatchFkey);
            param[2] = new SqlParameter("@CurUserID", CurUserID);
            return SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[BAU_SubSector]", param);
        }

        public List<SubSectorModel> GetBySectorID(int SectorID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_SubSector", new SqlParameter("@SectorID", SectorID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public void ActiveInActive(int SubSectorID, bool? InActive, int CurUserID)
        {
            SqlParameter[] PLead = new SqlParameter[3];
            PLead[0] = new SqlParameter("@SubSectorID", SubSectorID);
            PLead[1] = new SqlParameter("@InActive", InActive);
            PLead[2] = new SqlParameter("@CurUserID", CurUserID);
            SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AI_SubSector]", PLead);
        }

        private SubSectorModel RowOfSubSector(DataRow r)
        {
            SubSectorModel SubSector = new SubSectorModel();
            SubSector.SubSectorID = Convert.ToInt32(r["SubSectorID"]);
            SubSector.SubSectorName = r["SubSectorName"].ToString();
            SubSector.SectorID = Convert.ToInt32(r["SectorID"]);
            SubSector.SectorName = r["SectorName"].ToString();
            SubSector.InActive = Convert.ToBoolean(r["InActive"]);
            SubSector.CreatedUserID = Convert.ToInt32(r["CreatedUserID"]);
            SubSector.ModifiedUserID = Convert.ToInt32(r["ModifiedUserID"]);
            SubSector.CreatedDate = r["CreatedDate"].ToString().GetDate();
            SubSector.ModifiedDate = r["ModifiedDate"].ToString().GetDate();

            return SubSector;
        }
    }
}