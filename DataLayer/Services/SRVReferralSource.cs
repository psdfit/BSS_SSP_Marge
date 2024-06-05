using DataLayer.Interfaces;
using DataLayer.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DataLayer.Services
{
    
    public class SRVReferralSource : ISRVReferralSource
    {
        public SRVReferralSource()
        {

        }
        private List<ReferralSourceModel> LoopinData(DataTable dt)
        {
            List<ReferralSourceModel> list = new List<ReferralSourceModel>();

            foreach (DataRow r in dt.Rows)
            {
                list.Add(RowOfReferralSource(r));
            }
            return list;
        }

        public List<ReferralSourceModel> FetchReferralSources(ReferralSourceModel mod)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@ReferralSourceID", mod.ReferralSourceID));
                param.Add(new SqlParameter("@InActive", mod.InActive));
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_ReferralSources", param.ToArray()).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        private ReferralSourceModel RowOfReferralSource(DataRow row)
        {
            ReferralSourceModel obj = new ReferralSourceModel();
            obj.ReferralSourceID = row.Field<int>("ReferralSourceID");
            obj.Name = row.Field<string>("Name"); ;
            obj.UrduName = row.Field<string>("UrduName");
            obj.Description = row.Field<string>("Description");
            obj.InActive = row.Field<bool>("InActive");

            return obj;
        }

    }
}
