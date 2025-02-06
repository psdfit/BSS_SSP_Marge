using DataLayer.Classes;
using DataLayer.Interfaces;
using DataLayer.Models;
using DataLayer.Models.DVV;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace DataLayer.Services
{
    public class SRVGRN : ISRVGRN
    {
        public DataTable FetchClassesForGRNCompletion(GuruClassModel model)
        {
            
                List<SqlParameter> param = new List<SqlParameter>
                {

                };
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_DVVBiometricAttandanceTrainees", param.ToArray()).Tables[0];
                return dt;
          

        }
    }
}
