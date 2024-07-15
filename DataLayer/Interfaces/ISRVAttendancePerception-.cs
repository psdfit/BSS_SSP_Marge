using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using DataLayer.Models;
using Microsoft.Data.SqlClient;

namespace DataLayer.Interfaces
{
    public interface ISRVAttendancePerception
    {
        List<AttendancePerceptionModel> GetAttendancePerceptionList(AMSReportsParamModel model);
        public DataTable FetchAttendancePerceptionList(AMSReportsParamModel model);

    }
}
