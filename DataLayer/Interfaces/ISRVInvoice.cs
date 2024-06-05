using System;
using System.Collections.Generic;
using DataLayer.Models;
using Microsoft.Data.SqlClient;

namespace DataLayer.Interfaces
{
    public interface ISRVInvoice
    {
        List<InvoiceModel> GetInvoicesForApproval(int id, SqlTransaction _transaction = null);
        void GenerateInvoice(string ClassCode, int GLAccountID, int InvoiceNumber, SqlTransaction _transaction, string ProcessKey);
        bool UpdateSAPObjIdInInvoices(string sapObjId, int invHeaderId, SqlTransaction _transaction);
        public List<InvoiceModel> GetInvoiceExcelExportByIDs(string ids);
    }
}
