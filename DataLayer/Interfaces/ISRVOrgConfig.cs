/* **** Aamer Rehman Malik *****/

using DataLayer.Models;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace DataLayer.Interfaces
{
    public interface ISRVOrgConfig
    {
        OrgConfigModel GetByConfigID(int ConfigID);

        List<OrgConfigModel> SaveOrgConfig(OrgConfigModel OrgConfig);

        List<OrgConfigModel> FetchOrgConfig(OrgConfigModel mod);

        List<OrgConfigModel> FetchOrgConfig();

        List<OrgConfigModel> FetchOrgConfig(bool InActive);
        OrgConfigModel GetByClassID(int ClassID, SqlTransaction transaction = null);
        int BatchInsert(List<OrgConfigModel> ls, int @BatchFkey, int CurUserID);
        void ActiveInActive(int ConfigID, bool? InActive, int CurUserID);

        //List<OrgConfigModel> FetchOrgConfig(int OID, string ruleType,int sid,int tid, int cid);
        List<OrgConfigModel> FetchOrgConfig(int OID, string ruleType, int sid,int tid);
        bool ComparreNewAndPreviousListOfOrgConfig(List<OrgConfigModel> D, List<OrgConfigModel> PreviousOrgConfigList,int UserID);
        int OrgConfigLog(List<OrgConfigModel> ls, int @BatchFkey, int CurUserID);
    }
}