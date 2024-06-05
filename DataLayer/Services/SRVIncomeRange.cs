using DataLayer.Interfaces;
using DataLayer.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DataLayer.Services
{
    public class SRVIncomeRange : ISRVIncomeRange
    {
        public SRVIncomeRange()
        {

        }
        private List<IncomeRangeModel> LoopinData(DataTable dt)
        {
            List<IncomeRangeModel> list = new List<IncomeRangeModel>();

            foreach (DataRow r in dt.Rows)
            {
                list.Add(RowOfIncomeRanges(r));
            }
            return list;
        }
        public List<IncomeRangeModel> FetchIncomeRanges(IncomeRangeModel mod)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@IncomeRangeID", mod.IncomeRangeID));
                param.Add(new SqlParameter("@InActive", mod.InActive));
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "GetIncomeRanges", param.ToArray()).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        private IncomeRangeModel RowOfIncomeRanges(DataRow row)
        {
            IncomeRangeModel obj = new IncomeRangeModel();
            obj.IncomeRangeID = row.Field<int>("IncomeRangeID");
            obj.RangeName = row.Field<string>("RangeName"); ;
            obj.InActive = row.Field<bool>("InActive");
            obj.CreatedUserID = row.Field<int>("CreatedUserID");
            obj.ModifiedUserID = row.Field<int?>("ModifiedUserID") ?? 0;
            obj.CreatedDate = row.Field<DateTime?>("CreatedDate");
            obj.ModifiedDate = row.Field<DateTime?>("ModifiedDate");

            return obj;
        }

    }
}
