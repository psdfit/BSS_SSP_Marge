using System.Collections.Generic;
using DataLayer.Models;
using Microsoft.Data.SqlClient;

namespace DataLayer.Interfaces
{
    public interface ISRVPOLines
    {
        List<POLinesModel> FetchPOLines(int id);
        List<POLinesModel> GetPOLinesByPOHeaderID(int id, SqlTransaction transaction = null);
    }
}
