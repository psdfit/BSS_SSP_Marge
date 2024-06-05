using DataLayer.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Interfaces
{
    public interface ISRVSAPApi
    {
        Task<SAPResponseModel> SaveSAPCostCenterForScheme(SchemeModel scheme,SqlTransaction transaction = null);
        Task<SAPResponseModel> SaveSAPCostCenterForTrade(TradeCategoryModel tradeDetails, SqlTransaction transaction = null);
        Task<SAPResponseModel> SaveSAPBusinessPartnerForTSP(TSPDetailModel tsp, SqlTransaction transaction = null);
        Task<POSAPResponseModel> SaveSAPPurchaseOrder(POHeaderModel poHeader,List<POLinesModel> poLines, SqlTransaction transaction = null);
        Task<SAPResponseModel> SaveSAPAPInvoice(InvoiceMasterModel invoiceHeader, List<InvoiceModel> invoices, SqlTransaction transaction = null);
        Task<List<BranchesItems>> FetchSAPBranches();
        Task<bool> SynceBranches();
        Task<SAPResponseModel> SaveSAPBusinessPartnerForTSPUpdate(TSPChangeRequestModel tsp, SqlTransaction transaction = null);

    }
}
