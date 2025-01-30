using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;
using DataLayer.Models;

namespace DataLayer.Interfaces
{
    public interface ISRVPurchaseOrder
    {
        bool CreatePO(int schemeID, string processKey, int curuserID, SqlTransaction transaction = null);
        bool CreatePOForSRN(string srnIDs, string processKey, int curuserID, SqlTransaction transaction = null);
        bool CreatePOForGURN(string gurnIDs, string processKey, int curuserID, SqlTransaction transaction = null);
        bool CreatePOForTRN(int srnID, int curuserID, SqlTransaction transaction = null);
        bool POHeaderApproveReject(POHeaderModel model, SqlTransaction transaction = null);
        List<POHeaderModel> GetPOHeaderByID(int POHeaderID, SqlTransaction transaction = null);
    }
}
