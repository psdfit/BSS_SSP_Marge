using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Interfaces
{
    public interface ISRVReports
    {
        List<ReportsModel> FetchReports();
        List<ReportsModel> FetchReportsByRoleID(int? RoleID);
        List<ReportsModel> FetchSubReports(int? ReportID);
        List<ReportsModel> FetchSubReportsFilters(int? SubReportID);
    }
}
