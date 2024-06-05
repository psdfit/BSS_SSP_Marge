using DataLayer.Models;
using DataLayer.Models.SSP;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;

namespace DataLayer.Interfaces
{
    public interface ISRVProcessConfiguration
    {
        public DataTable Save(ProcessScheduleModel data);
        public bool RemoveTaskDetail(TaskDetail taskDetail);
        public DataTable FetchDataListBySPName(string spName);
        public DataTable LoopinData(DataTable dt, string[] attachmentColumns);
    }
}
