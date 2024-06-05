using System;
using System.Collections.Generic;
using System.Text;
using DataLayer.Models;
using Microsoft.Data.SqlClient;

namespace DataLayer.Interfaces
{
    public interface ISRVPRN
    {
        List<PRNModel> GetPRNForApproval(int id);
        List<PRNModel> GetPRNForApproval(PRNModel model);
        List<PRNModel> GetPRNExcelExport(PRNMasterModel model);
        PRNModel GetPRNByID(PRNModel model, SqlTransaction _transaction);
        bool PRNApproveReject(PRNModel model, SqlTransaction transaction = null);
        List<PRNModel> GetPTBRTrainees(string classCode, DateTime month);
        public List<PRNModel> GetPRNExcelExportByIDs(string ids);
        public bool GeneratePRNCompletion(QueryFilters model);
        public bool GeneratePRNFinal(QueryFilters model);
        public bool PenaltyImposedByME_DeductionUniformBag(PRNModel model);
    }
}
