using DataLayer.Models;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace DataLayer.Interfaces
{

    public interface ISRVSRN
    {
        SRNModel GetBySrnId(int SRNID);
        List<SRNModel> FetchSRN(SRNModel mod);
        List<SRNModel> FetchVRN(SRNModel mod);
        List<SRNModel> FetchSRN();
        List<SRNModel> FetchSRN(bool InActive);
        void ActiveInActive(int SrnId, bool? InActive, int CurUserID);
        bool SRNApproveReject(SRNModel model, SqlTransaction transaction = null);
        bool TRNApproveReject(TRNMasterModel model, SqlTransaction transaction = null);

        bool PO_TRNApproveReject(POHeaderModel model, SqlTransaction transaction = null);
        void GenerateInvoiceHeader_SRN(int PoHeaderID, SqlTransaction _transaction, string ProcessKey);
    }
}
