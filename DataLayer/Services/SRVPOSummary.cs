using System;
using System.Collections.Generic;
using System.Data;
using DataLayer.Classes;
using DataLayer.Interfaces;
using DataLayer.Models;
using Microsoft.Data.SqlClient;

namespace DataLayer.Services
{
    public class SRVPOSummary : ISRVPOSummary
    {
        public List<POSummaryModel> GetPOSummary(DateTime Month,int SchemeID, int TSPID, string ProcessKey)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>() {
                 new SqlParameter("@Month", Month),
                 new SqlParameter("@Scheme_ID", SchemeID),
                 new SqlParameter("@TSP_ID", TSPID),
                 new SqlParameter("@ProcessKey", ProcessKey)
                };
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "PO_Summary", param.ToArray()).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        //private List<POSummaryModel> LoopinData(DataTable dt)
        //{
        //    List<POSummaryModel> inv = new List<POSummaryModel>();

        //    foreach (DataRow r in dt.Rows)
        //    {
        //        inv.Add(Row(r));
        //    }
        //    return inv;
        //}
        private List<POSummaryModel> LoopinData(DataTable dt)
        {
            List<POSummaryModel> inv = new List<POSummaryModel>();

            foreach (DataRow r in dt.Rows)
            {
                inv.Add(Row(r));
            }
            return inv;
        }

        private POSummaryModel Row(DataRow r)
        {
            POSummaryModel po = new POSummaryModel();

            po.SchemeName = r["SchemeName"].ToString();
            po.TSPName = r["TSPName"].ToString();
            po.ClassCount = Convert.ToInt32(r["ClassCount"]);
            po.Trades = r["Trades"].ToString();
            po.SchemeCost = Convert.ToDouble(r["SchemeCost"]);
            po.TotalCostPerClassInTax = Convert.ToDouble(r["TotalCostPerClassInTax"]);
            po.TaxRate = Convert.ToDouble(r["TaxRate"]);
            po.SalesTax = Convert.ToDouble(r["SalesTax"]);
            po.PST = Convert.ToInt32(r["PST"]);
            po.FinalAmount = Convert.ToDouble(r["FinalAmount"]);
            po.Month = r["Month"].ToString().GetDate();

            return po;
        }
        //private POSummaryModel Row(DataRow r)
        //{
        //    POSummaryModel po = new POSummaryModel();

        //    po.SchemeName = r["SchemeName"].ToString();
        //    po.ClassCode = r["ClassCode"].ToString();
        //    po.TSPName = r["TSPName"].ToString();
        //    po.TradeName = r["TradeName"].ToString();
        //    po.TraineesPerClass = Convert.ToInt32(r["TraineesPerClass"]);
        //    po.LineTotal = Convert.ToInt32(r["LineTotal"]);
        //    po.StartDate = r["StartDate"].ToString().GetDate();
        //    po.EndDate = r["EndDate"].ToString().GetDate();

        //    return po;
        //}

    }
}
