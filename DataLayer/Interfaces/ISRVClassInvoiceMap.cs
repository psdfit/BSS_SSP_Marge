using DataLayer.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Interfaces
{
    public interface ISRVClassInvoiceMap
    {
        int BatchInsert(List<ClassInvoiceMapModel> list, SqlTransaction transaction = null);
        
    }
}
