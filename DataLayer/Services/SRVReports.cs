using DataLayer.Interfaces;
using DataLayer.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DataLayer.Services
{
    public class SRVReports : ISRVReports
    {
        public List<ReportsModel> FetchReports()
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@InActive", false));
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Reports", param.ToArray()).Tables[0];
                List<ReportsModel> reportsModel = new List<ReportsModel>();
                reportsModel = Helper.ConvertDataTableToModel<ReportsModel>(dt);

                return (reportsModel);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<ReportsModel> FetchReportsByRoleID(int? RoleID)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@RoleID", RoleID));
                param.Add(new SqlParameter("@InActive", false));
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Reports", param.ToArray()).Tables[0];
                List<ReportsModel> reportsModel = new List<ReportsModel>();
                reportsModel = Helper.ConvertDataTableToModel<ReportsModel>(dt);

                return (reportsModel);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<ReportsModel> FetchSubReports(int? ReportID)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@ReportID", ReportID);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_SubReports", param).Tables[0];
                List<ReportsModel> reportsModel = new List<ReportsModel>();
                reportsModel = Helper.ConvertDataTableToModel<ReportsModel>(dt);

                return (reportsModel);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<ReportsModel> FetchSubReportsFilters(int? SubReportID)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@SubReportID", SubReportID);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_SubReportsFilters", param).Tables[0];
                List<ReportsModel> reportsModel = new List<ReportsModel>();
                reportsModel = Helper.ConvertDataTableToModel<ReportsModel>(dt);

                return (reportsModel);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
    }
}
