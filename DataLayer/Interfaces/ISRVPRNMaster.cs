using System;
using System.Collections.Generic;
using System.Text;
using DataLayer.Models;
using Microsoft.Data.SqlClient;

namespace DataLayer.Interfaces
{
    public interface ISRVPRNMaster
    {
        List<PRNMasterModel> GetPRNMasterForApproval(int id, SqlTransaction transaction = null);
        List<PRNMasterModel> GetPRNMasterForApproval(PRNMasterModel model);
        PRNMasterModel GetPRNMasterByID(PRNMasterModel model, SqlTransaction _transaction);
        bool PRNMasterApproveReject(PRNMasterModel model, SqlTransaction transaction = null);
    }
}
