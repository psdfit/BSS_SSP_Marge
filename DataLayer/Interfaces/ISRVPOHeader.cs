using System;
using System.Collections.Generic;
using DataLayer.Models;
using Microsoft.Data.SqlClient;

namespace DataLayer.Interfaces
{
    public interface ISRVPOHeader
    {
        List<POHeaderModel> FetchPOHeader(POHeaderModel model, SqlTransaction transaction = null);
        List<SubmitedPOsModel> GetPOForApproval(int UserID);
        List<POHeaderModel> FetchPOHeaderByFilters(DateTime? date, int schemeID, int tSPID, string processKey);
    }
}
