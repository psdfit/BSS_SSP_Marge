using DataLayer.Models;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace DataLayer.Interfaces
{

    public interface ISRVGURN
    {
        List<GURNModel> FetchGURN(GURNModel mod);
        bool GURNApproveReject(GURNModel model, SqlTransaction transaction = null);


        GURNModel GetByGurnId(int GURNID);
        List<GURNModel> FetchVRN(GURNModel mod);
        List<GURNModel> FetchGURN();
        List<GURNModel> FetchGURN(bool InActive);
        void ActiveInActive(int GurnId, bool? InActive, int CurUserID);
        bool TRNApproveReject(TRNMasterModel model, SqlTransaction transaction = null);

        bool PO_TRNApproveReject(POHeaderModel model, SqlTransaction transaction = null);
        void GenerateInvoiceHeader_GURN(int PoHeaderID, SqlTransaction _transaction, string ProcessKey);
    }
}
