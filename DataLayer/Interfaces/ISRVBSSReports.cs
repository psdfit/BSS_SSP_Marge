using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace DataLayer.Interfaces
{
    public interface ISRVBSSReports
    {
        public DataTable FetchList(String Param);
        public DataTable FetchReport(String Param);

        public DataTable FetchDropDownList(String Param);
    }
}