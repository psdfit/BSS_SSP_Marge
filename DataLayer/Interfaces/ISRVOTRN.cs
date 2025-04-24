using DataLayer.Models;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace DataLayer.Interfaces
{

    public interface ISRVOTRN
    {
        OTRNModel GetByOTRNId(int OTRNID);
        List<OTRNModel> FetchOTRN(OTRNModel mod);
        List<OTRNModel> FetchVRN(OTRNModel mod);
        List<OTRNModel> FetchOTRN();
        List<OTRNModel> FetchOTRN(bool InActive);
        void ActiveInActive(int OTRNId, bool? InActive, int CurUserID);
        bool OTRNApproveReject(OTRNModel model, SqlTransaction transaction = null);
        bool TRNApproveReject(TRNMasterModel model, SqlTransaction transaction = null);

        bool PO_TRNApproveReject(POHeaderModel model, SqlTransaction transaction = null);
        void GenerateInvoiceHeader_OTRN(int PoHeaderID, SqlTransaction _transaction, string ProcessKey);
    }
}
