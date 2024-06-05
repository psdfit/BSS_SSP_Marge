using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.IO;

namespace DataLayer.Classes
{
    public static class GenrateCode
    {
        public static bool GenrateClasses(string tblName, string Prj)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[GenRateSp]", new SqlParameter("@ObjectName", tblName));
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "[GenerateEntityClass]", new SqlParameter("@ObjectName", tblName)).Tables[0];
                string docPath =
                AppContext.BaseDirectory.Substring(0, AppContext.BaseDirectory.IndexOf("bin") - 1);
                string appRoot = docPath.Substring(0, docPath.LastIndexOf("\\") + 1);
                using (StreamWriter outputFile = new StreamWriter(Path.Combine(appRoot + "\\DataLayer\\Models\\", tblName + "Model.cs")))
                {
                    outputFile.WriteLine(dt.Rows[0][0].ToString());
                    outputFile.Close();
                }
                using (StreamWriter outputFile = new StreamWriter(Path.Combine(appRoot + "\\DataLayer\\Interfaces\\", "ISRV" + tblName + ".cs")))
                {
                    outputFile.WriteLine(dt.Rows[0][1].ToString());
                    outputFile.Close();
                }
                using (StreamWriter outputFile = new StreamWriter(Path.Combine(appRoot + "\\DataLayer\\Services\\", "SRV" + tblName + ".cs")))
                {
                    outputFile.WriteLine(dt.Rows[0][2].ToString());
                    outputFile.Close();
                }
                dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "[GenerateControllerAPI]", new SqlParameter[] { new SqlParameter("@ObjectName", tblName), new SqlParameter("@NameSpace", "PSDF_BSS" + Prj) }).Tables[0];
                using (StreamWriter outputFile = new StreamWriter(Path.Combine(appRoot + "\\" + "PSDF_BSS" + "\\Controllers\\", tblName + "Controller.cs")))
                {
                    outputFile.WriteLine(dt.Rows[0][0].ToString());
                    outputFile.Close();
                }
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }

            return true;
        }
    }
}