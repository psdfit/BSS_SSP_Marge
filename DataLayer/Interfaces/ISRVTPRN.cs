using DataLayer.Models;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace DataLayer.Interfaces
{

    public interface ISRVTPRN
    {
        TPRNModel GetByTPRNId(int TPRNID);
        List<TPRNModel> FetchTPRN(TPRNModel mod);
        List<TPRNModel> FetchVRN(TPRNModel mod);
        List<TPRNModel> FetchTPRN();
        List<TPRNModel> FetchTPRN(bool InActive);
        void ActiveInActive(int TPRNId, bool? InActive, int CurUserID);
        bool TPRNApproveReject(TPRNModel model, SqlTransaction transaction = null);
        bool TRNApproveReject(TRNMasterModel model, SqlTransaction transaction = null);

        bool PO_TRNApproveReject(POHeaderModel model, SqlTransaction transaction = null);
        void GenerateInvoiceHeader_TPRN(int PoHeaderID, SqlTransaction _transaction, string ProcessKey);
    }
}
