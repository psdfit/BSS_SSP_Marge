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
    public class SRVBenchmarking : SRVBase, ISRVBenchmarking
    {
        public SRVBenchmarking() { }
        public BenchmarkingModel GetByBenchmarkingID(int BenchmarkingID)
        {
            try
            {
                SqlParameter param = new SqlParameter("@BenchmarkingID", BenchmarkingID);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Benchmarking", param).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    return RowOfBenchmarking(dt.Rows[0]);
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public List<BenchmarkingModel> SaveBenchmarking(BenchmarkingModel Benchmarking)
        {
            try
            {

                SqlParameter[] param = new SqlParameter[16];
                param[0] = new SqlParameter("@BenchmarkingID", Benchmarking.BenchmarkingID);
                param[1] = new SqlParameter("@TradeName", Benchmarking.TradeName);
                param[2] = new SqlParameter("@ProposedAmount", Benchmarking.ProposedAmount);
                //     param[3] = new SqlParameter("@ClassFrom", Benchmarking.ClassFrom);
                //param[4] = new SqlParameter("@ClassTo", Benchmarking.ClassTo);
                param[5] = new SqlParameter("@CalculatedAmount", Benchmarking.CalculatedAmount);
                param[6] = new SqlParameter("@CalculatedAmount70", Benchmarking.CalculatedAmount70);
                param[7] = new SqlParameter("@ProposedAmount50", Benchmarking.ProposedAmount50);
                param[8] = new SqlParameter("@OfferedAmount", Benchmarking.OfferedAmount);
                //param[9] = new SqlParameter("@DistrictID", Benchmarking.DistrictID);
                //param[10] = new SqlParameter("@ClusterID", Benchmarking.ClusterID);
                //param[11] = new SqlParameter("@RegionID", Benchmarking.RegionID);
                param[12] = new SqlParameter("@TotalClasses", Benchmarking.TotalClasses);
                //param[13] = new SqlParameter("@Inflation", Benchmarking.Inflation);
                //param[14] = new SqlParameter("@InRecentSchemes", Benchmarking.InRecentSchemes);

                param[15] = new SqlParameter("@CurUserID", Benchmarking.CurUserID);
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_Benchmarking]", param);
                return FetchBenchmarking();
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }
        private List<BenchmarkingModel> LoopinData(DataTable dt)
        {
            List<BenchmarkingModel> BenchmarkingL = new List<BenchmarkingModel>();

            foreach (DataRow r in dt.Rows)
            {
                BenchmarkingL.Add(RowOfBenchmarking(r));

            }
            return BenchmarkingL;
        }
        public List<BenchmarkingModel> FetchBenchmarking(BenchmarkingModel mod)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[6];
                param[0] = new SqlParameter("@TradeName", mod.TradeName);
                param[1] = new SqlParameter("@ProgramTypeID", mod.ProgramTypeID);
                param[2] = new SqlParameter("@CostSharing", mod.CostSharing);
                param[3] = new SqlParameter("@TSPID", mod.TSPID);
                //param[3] = new SqlParameter("@Json", JsonConvert.SerializeObject(mod));
                param[4] = new SqlParameter("@ProposedAmount", mod.ProposedAmount);

                
                
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "Get_Benchmarking_Updated1", param).Tables[0];
                //return LoopinData(dt);

                if (dt.Rows.Count.Equals(0))
                {
                    DataTable dt1 = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "Get_Benchmarking_No_Relevant_Trades", param).Tables[0];
                    return LoopinData(dt1);

                }
                else
                {
                    return LoopinData(dt);
                }

            }

            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public DataTable FetchBenchmarkingClasses(BenchmarkingModel mod)

        {
            try
            {
                List<SqlParameter> prams = new List<SqlParameter>()
                {
                    new SqlParameter("@TradeName",mod.TradeName),
                    new SqlParameter("@ProgramTypeID",mod.ProgramTypeID),
                    new SqlParameter("@TSPID",mod.TSPID),
                    new SqlParameter("@SchemeDate",mod.SchemeDate),
                    new SqlParameter("@PTypeName",mod.PTypeName),
                    //new SqlParameter("@InRecentSchemes",mod.InRecentSchemes),
                    ////new SqlParameter("@ClusterID",mod.ClusterID),
                    ////new SqlParameter("@DistrictID",mod.DistrictID),
                    ////new SqlParameter("@RegionID",mod.RegionID),
               };
                return SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "Get_Benchmarking_Classes", prams.ToArray()).Tables[0];
                // return LoopinClassesData(ct);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<BenchmarkingModel> FetchBenchmarking()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Benchmarking").Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public List<BenchmarkingModel> FetchBenchmarking(bool InActive)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Benchmarking", new SqlParameter("@InActive", InActive)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public void ActiveInActive(int BenchmarkingID, bool? InActive, int CurUserID)
        {

            SqlParameter[] PLead = new SqlParameter[3];
            PLead[0] = new SqlParameter("@BenchmarkingID", BenchmarkingID);
            PLead[1] = new SqlParameter("@InActive", InActive);
            PLead[2] = new SqlParameter("@CurUserID", CurUserID);
            SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AI_Benchmarking]", PLead);
        }
        private BenchmarkingModel RowOfBenchmarking(DataRow r)
        {
            BenchmarkingModel Benchmarking = new BenchmarkingModel();
            //Benchmarking.BenchmarkingID = Convert.ToInt32(r["BenchmarkingID"]);
            Benchmarking.TradeName = r["TradeName"].ToString();
            Benchmarking.RecentSchemes = r["RecentSchemes"].ToString();
            Benchmarking.CostSharing = r["CostSharing"].ToString();
            Benchmarking.ProposedAmount = Convert.ToInt32(r["ProposedAmount"]);
            Benchmarking.ProgramTypeID = Convert.ToInt32(r["ProgramTypeID"]);
            Benchmarking.TSPID = r["TSPID"].ToString();
            Benchmarking.TSPName = r["TSPName"].ToString();
            //Benchmarking.ClassFrom =r["ClassFrom"].ToString().GetDateSTR();
            //Benchmarking.ClassTo =r["ClassTo"].ToString().GetDateSTR();
            Benchmarking.OfferedAmount = Convert.ToInt32(r["OfferedAmount"]);
            Benchmarking.CalculatedAmount = Convert.ToInt32(r["CalculatedAmount"]);
            Benchmarking.CalculatedAmount70 = Convert.ToInt32(r["CalculatedAmount70"]);
            Benchmarking.ProposedAmount50 = Convert.ToInt32(r["ProposedAmount50"]);
            Benchmarking.TotalClasses = Convert.ToInt32(r["TotalClasses"]);
            Benchmarking.SchemeDate = r["SchemeDate"].ToString().GetDate();
            Benchmarking.PTypeName = r["PTypeName"].ToString();

            //Benchmarking.DistrictID = Convert.ToInt32(r["DistrictID"]);
            //Benchmarking.RegionID = Convert.ToInt32(r["RegionID"]);
            //Benchmarking.ClusterID = Convert.ToInt32(r["ClusterID"]);
            //Benchmarking.InRecentSchemes = Convert.ToBoolean(r["InRecentSchemes"]);
            //Benchmarking.InflationRate = Convert.ToInt32(r["InflationRate"]);


            return Benchmarking;
        }
    }
}
