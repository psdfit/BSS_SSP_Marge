using DataLayer.Models;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace DataLayer.Interfaces
{

    public interface ISRVPVRN
    {
        PVRNModel GetByPVRNId(int PVRNID);
        List<PVRNModel> FetchPVRN(PVRNModel mod);
        List<PVRNModel> FetchVRN(PVRNModel mod);
        List<PVRNModel> FetchPVRN();
        List<PVRNModel> FetchPVRN(bool InActive);
        void ActiveInActive(int PVRNId, bool? InActive, int CurUserID);
        bool PVRNApproveReject(PVRNModel model, SqlTransaction transaction = null);
        bool TRNApproveReject(TRNMasterModel model, SqlTransaction transaction = null);

        bool PO_TRNApproveReject(POHeaderModel model, SqlTransaction transaction = null);
        void GenerateInvoiceHeader_PVRN(int PoHeaderID, SqlTransaction _transaction, string ProcessKey);
    }
}
