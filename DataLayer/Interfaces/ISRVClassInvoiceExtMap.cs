/* **** Aamer Rehman Malik *****/

using DataLayer.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DataLayer.Interfaces
{
    public interface ISRVClassInvoiceExtMap
    {
        DataTable GetMPRPRNDetails(int ClassID, bool Running);

        List<ClassInvoiceMapExtModel> GetForApproval();

        bool Cancellation(int FormID, string Type, int ClassID);
        bool Regenerate(int ClassID, DateTime Month, string Type);

        bool CancellationApproval(int ClassID, DateTime Month, string InvoiceType, int InvoiceID, bool approved, SqlTransaction _Trans = null);
        DataTable GetTSPInvoiceStatusDetails(int ClassID, bool Running);

        ClassInvoiceMapModel GetInvoices(int InvoiceID);
        System.Data.DataTable GetMPR(int ID);
        System.Data.DataTable GetTPRN(int ID);
        System.Data.DataTable GetSRN(int ID);
        System.Data.DataTable GetPRN(int ID);
        System.Data.DataTable GetPRNMaster(int ID);
        System.Data.DataTable GetPO(int ID);
        System.Data.DataTable GetInv(int ID);

        System.Data.DataTable GetPOHeader(int ID);
        System.Data.DataTable GetInvHeader(int ID);
        public DataTable GetTSPInvoiceStatusDetailsMonthWise(DateTime? month, int CurUserID,int InvoiceHeaderID, int ClassID = 0);
        public DataTable getTSPInvoiceStatusDataMonthWiseForKAM(DateTime? month, int CurUserID, int ClassID = 0);
        public DataTable getTSPInvoiceStatusDetailMonthWiseForKAM(DateTime? month, int CurUserID, int InvoiceHeaderID, int ClassID = 0);
        public DataTable GetTSPInvoiceStatusDetailsMonthWiseByInternalUser(DateTime? month, int CurUserID, int InvoiceHeaderID, int ClassID = 0);

    }
}