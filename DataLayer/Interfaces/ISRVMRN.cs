using DataLayer.Models;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace DataLayer.Interfaces
{

    public interface ISRVMRN
    {
        MRNModel GetByMRNId(int MRNID);
        List<MRNModel> FetchMRN(MRNModel mod);
        List<MRNModel> FetchVRN(MRNModel mod);
        List<MRNModel> FetchMRN();
        List<MRNModel> FetchMRN(bool InActive);
        void ActiveInActive(int MRNId, bool? InActive, int CurUserID);
        bool MRNApproveReject(MRNModel model, SqlTransaction transaction = null);
        bool TRNApproveReject(TRNMasterModel model, SqlTransaction transaction = null);

        bool PO_TRNApproveReject(POHeaderModel model, SqlTransaction transaction = null);
        void GenerateInvoiceHeader_MRN(int PoHeaderID, SqlTransaction _transaction, string ProcessKey);
    }
}
