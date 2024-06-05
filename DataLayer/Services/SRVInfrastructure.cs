using DataLayer.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DataLayer.Services
{
    public class SRVInfrastructure : SRVBase, DataLayer.Interfaces.ISRVInfrastructure
    {
        public bool SaveInfrastructure(InfrastructureSaveModel list)
        {
            try
            {
                foreach (var item in list.InfrastructureArray)
                {
                    SqlParameter[] param = new SqlParameter[11];
                    param[0] = new SqlParameter("@InfrastructureID", null);
                    param[1] = new SqlParameter("@Stream", item.Stream);
                    param[2] = new SqlParameter("@Scheme", item.Scheme);
                    param[3] = new SqlParameter("@TSP", item.TrainingServiceProvider);
                    param[4] = new SqlParameter("@Trade", item.Trade);
                    param[5] = new SqlParameter("@Building", item.Building);
                    param[6] = new SqlParameter("@Furniture", item.Furniture);
                    param[7] = new SqlParameter("@BackupSourceOfElectricity", item.BackupSourceOfElectricity);
                    param[8] = new SqlParameter("@ToolsAndequipment", item.ToolsAndequipment);
                    param[9] = new SqlParameter("@TotalAm", item.TotalAm);
                    param[10] = new SqlParameter("@ScoreOnScaleOf5", item.ScoreOnScaleOf5);
                    SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_Infrastructure]", param);
                }
                return true;
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }
        public List<InfrastructureModel> GetInfrastructures()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Infrastructure").Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }
        private List<InfrastructureModel> LoopinData(DataTable dt)
        {
            List<InfrastructureModel> list = new List<InfrastructureModel>();

            foreach (DataRow r in dt.Rows)
            {
                list.Add(RowOfInfrastructures(r));
            }
            return list;
        }
        private InfrastructureModel RowOfInfrastructures(DataRow r)
        {
            InfrastructureModel obj = new InfrastructureModel();
            obj.Stream = r["Stream"].ToString();
            obj.Scheme = r["Scheme"].ToString();
            obj.TrainingServiceProvider = r["TSP"].ToString();
            obj.Trade = r["Trade"].ToString();
            obj.Building = r["Building"].ToString();
            obj.Furniture = r["Furniture"].ToString();
            obj.BackupSourceOfElectricity = r["BackupSourceOfElectricity"].ToString();
            obj.ToolsAndequipment = r["ToolsAndequipment"].ToString();
            obj.TotalAm = r["TotalAm"].ToString();
            obj.ScoreOnScaleOf5 = r["ScoreOnScaleOf5"].ToString();
            return obj;
        }
    }
}
