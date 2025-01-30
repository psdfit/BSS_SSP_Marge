using DataLayer.Classes;
using DataLayer.Models;
using DataLayer.Interfaces;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DataLayer.Services
{
    public class SRVGURNCoursera : ISRVGURNCoursera
    {
        public SRVGURNCoursera()
        {
        }

        public DataTable FetchGURNCourseraTrainees(QueryFilters model)
        {
            SqlParameter[] param = new SqlParameter[5];

            param[0] = new SqlParameter("@SchemeID", model.SchemeID);
            param[1] = new SqlParameter("@Month", model.Month);
            DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_EligibleforGURNCoursera", param).Tables[0];
            return dt;
        }

        public DataTable GenerateGURNCoursera(QueryFilters model, out string IsGenerated)
        {
            SqlParameter[] param = new SqlParameter[5];
            IsGenerated = null;
            param[0] = new SqlParameter("@SchemeID", model.SchemeID);
            param[1] = new SqlParameter("@Month", model.Month);
            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "dbo.GenerateGURNCoursera", param.ToArray());
            IsGenerated = ds.Tables[0].Rows[0]["GURNGenerated"].ToString();
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0];
            return null;
        }
    }
}
