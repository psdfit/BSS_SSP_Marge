using DataLayer.Models;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace DataLayer.Interfaces
{
    public interface ISRVPCRN
    {
        PCRNModel GetByPCRNId(int PCRNID);
        List<PCRNModel> FetchPCRN(PCRNModel mod);
        List<PCRNModel> FetchVRN(PCRNModel mod);
        List<PCRNModel> FetchPCRN();
        List<PCRNModel> FetchPCRN(bool InActive);
        void ActiveInActive(int PCRNId, bool? InActive, int CurUserID);
        bool PCRNApproveReject(PCRNModel model, SqlTransaction transaction = null);
        bool TRNApproveReject(TRNMasterModel model, SqlTransaction transaction = null);
        bool PO_TRNApproveReject(POHeaderModel model, SqlTransaction transaction = null);
        void GenerateInvoiceHeader_PCRN(int PoHeaderID, SqlTransaction _transaction, string ProcessKey);
    }
}
