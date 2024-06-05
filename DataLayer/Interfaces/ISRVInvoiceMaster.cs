/* **** Aamer Rehman Malik *****/

using DataLayer.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DataLayer.Interfaces
{
    public interface ISRVInvoiceMaster
    {
        List<InvoiceMasterModel> GetInvoicesForApproval(InvoiceMasterModel model, SqlTransaction t = null);
        List<InvoiceMasterModel> GetInvoiceDetails(InvoiceMasterModel model, SqlTransaction t = null);

        void GenerateInvoiceHeader(int PRNMasterID, SqlTransaction _transaction, string ProcessKey);

        void GenerateInvoiceHeader_TRN(int PoHeaderID, SqlTransaction _transaction, string ProcessKey);

        void GenerateINVCompletion(int PRNMasterID, string ProcessKey, SqlTransaction _transaction = null);

        void GenerateINVEmployment(int PRNMasterID, string ProcessKey, SqlTransaction _transaction = null);

        bool InvoiceHeaderApproveReject(InvoiceMasterModel model, SqlTransaction transaction = null);

        bool UpdateSAPObjIdInInvoiceHeader(string sapObjIdINV, int invHeader, SqlTransaction transaction = null);
        InvoiceMasterModel GetInvoicesForApproval_Notification(int InvoiceHeaderID, SqlTransaction transaction = null);
        InvoiceHeaderModel GetInvoiceHeader(int TSPID, DateTime? Month, SqlTransaction transaction = null);
        public List<InvoiceMasterModel> GetInvoicesForTSP(InvoiceMasterModel model, SqlTransaction _transaction);
        DataTable GetInvoiceHeaderForTSP(InvoiceMasterModel model, SqlTransaction _transaction);
        DataTable GetInvoiceHeaderForKAM(InvoiceMasterModel model, SqlTransaction _transaction);
        public DataTable GetInvoiceHeaderForInternalUser(InvoiceMasterModel model, SqlTransaction _transaction);

    }
}