/* **** Aamer Rehman Malik *****/

using DataLayer.Interfaces;
using DataLayer.Models;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DataLayer.Services
{
    public class SRVClassInvoiceMap : ISRVClassInvoiceMap
    {
        public int BatchInsert(List<ClassInvoiceMapModel> list, SqlTransaction transaction = null)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[3];

                param[0] = new SqlParameter("@Json", JsonConvert.SerializeObject(list));
                return transaction != null
                                    ? SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, "[BAU_ClassInvoiceMap]", param)
                                    : SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[BAU_ClassInvoiceMap]", param);
            }
            catch (Exception e)
            { throw new Exception(e.Message); }
        }

        

       
    }
}