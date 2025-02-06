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
    public class SRVClassInvoiceMapExt : ISRVClassInvoiceExtMap
    {
        public bool Cancellation(int FormID, string Type, int ClassID)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>() {
                 new SqlParameter("@FormID", FormID)
                , new SqlParameter("@Type", Type)
                , new SqlParameter("@ClassID", ClassID)
                };
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "CXL_Cancelation", param.ToArray());
                return true;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public bool Regenerate(int ClassID, DateTime Month, string Type)
        {
            try
            {
                int ParamYear = Month.Year;
                int ParamMonth = Month.Month;
                int ParamDate = Month.Day;
                List<SqlParameter> param = new List<SqlParameter>() {
                 new SqlParameter("@ClassID", ClassID)
                , new SqlParameter("@Month", ParamYear+"-"+ParamMonth+"-"+ParamDate)
                , new SqlParameter("@Type", Type)
                };

                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "CXL_Regenerate", param.ToArray());

                return true;
            }
            catch (Exception ex)
            {
                string[] tokens = ex.Message.Split("##");
                throw new Exception(tokens[0]);
            }
        }

        //public bool Cancellation(int ClassID, DateTime Month, string InvoiceType, int InvoiceID)
        //{
        //    try
        //    {
        //        List<SqlParameter> param = new List<SqlParameter>() {
        //         new SqlParameter("@ClassID", ClassID)
        //        , new SqlParameter("@Month", Month)
        //        , new SqlParameter("@InvoiceType", InvoiceType)
        //        , new SqlParameter("@InvoiceID", InvoiceID)
        //        };
        //        SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "Sp_Cancelation", param.ToArray());
        //        return true;
        //    }
        //    catch (Exception ex) { throw new Exception(ex.Message); }
        //}


        //public bool Regenrate(int ClassID, DateTime Month, string InvoiceType, int InvoiceID)
        //{
        //    try
        //    {
        //        int ParamYear = Month.Year;
        //        int ParamMonth = Month.Month;
        //        int ParamDate = Month.Day;
        //        if (InvoiceType == "Regular")
        //        {
        //            List<SqlParameter> param = new List<SqlParameter>() {
        //         new SqlParameter("@Class", ClassID)
        //        , new SqlParameter("@Month", ParamYear+"-"+ParamMonth+"-"+ParamDate)
        //        };
        //            SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "GenrateMPR_N", param.ToArray());
        //        }
        //        else if (InvoiceType == "2nd Last")
        //        {
        //            SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "GeneratePRNCompletion");
        //        }
        //        else if (InvoiceType == "Final")
        //        {
        //            SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "GenrateEmploymentPRN");
        //        }
        //        return true;
        //    }
        //    catch (Exception ex) { throw new Exception(ex.Message); }
        //}

        public bool CancellationApproval(int ClassID, DateTime Month, string InvoiceType, int InvoiceID, bool approved, SqlTransaction _trans)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>() {
                 new SqlParameter("@ClassID", ClassID)
                , new SqlParameter("@Month", Month)
                , new SqlParameter("@InvoiceType", InvoiceType)
                , new SqlParameter("@InvoiceID", InvoiceID)
                , new SqlParameter("@approved", @approved)
                };
                if (_trans == null)
                    SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[Sp_Cancelation_Approval]", param.ToArray());
                else
                    SqlHelper.ExecuteNonQuery(_trans, CommandType.StoredProcedure, "[Sp_Cancelation_Approval]", param.ToArray());
                return true;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<ClassInvoiceMapExtModel> GetForApproval()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_ClassInvoiceMap").Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public DataTable GetMPRPRNDetails(int ClassID = 0, bool Running = false)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>() {
                new SqlParameter("@ClassID", ClassID)
                };
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "CXL_GetClassView", param.ToArray()).Tables[0];
                return dt;
                //return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public DataTable GetTSPInvoiceStatusDetails(int ClassID = 0, bool Running = false)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>() {
                new SqlParameter("@ClassID", ClassID)
                };
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "GetClassesForTspInvoiceStatus", param.ToArray()).Tables[0];
                return dt;
                //return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public DataTable GetTSPInvoiceStatusDetailsMonthWise(DateTime? month, int CurUserID, int InvoiceHeaderID, int ClassID = 0)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>() {
                new SqlParameter("@ClassID", ClassID),
                new SqlParameter("@Month", month),
                new SqlParameter("@UserID", CurUserID),
                new SqlParameter("@InvoiceHeaderID", InvoiceHeaderID)
                };
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "GetClassesForTspInvoiceDetail", param.ToArray()).Tables[0];
                return dt;
                //return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        
        public DataTable GetTSPInvoiceStatusDetailsMonthWiseByInternalUser(DateTime? month, int CurUserID, int InvoiceHeaderID, int ClassID = 0)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>() {
                new SqlParameter("@ClassID", ClassID),
                new SqlParameter("@Month", month),
                new SqlParameter("@UserID", CurUserID),
                new SqlParameter("@InvoiceHeaderID", InvoiceHeaderID)
                };
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "GetClassesForTspInvoiceDetailByInternalUser", param.ToArray()).Tables[0];
                return dt;
                //return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public DataTable getTSPInvoiceStatusDataMonthWiseForKAM(DateTime? month, int CurUserID, int ClassID = 0)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>() {
                new SqlParameter("@ClassID", ClassID),
                new SqlParameter("@Month", month),
                new SqlParameter("@UserID", CurUserID)
                };
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "GetClassesForKAMWiseTspInvoiceStatus", param.ToArray()).Tables[0];
                return dt;
                //return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public DataTable getTSPInvoiceStatusDetailMonthWiseForKAM(DateTime? month, int CurUserID, int InvoiceHeaderID, int ClassID = 0)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>() {
                new SqlParameter("@ClassID", ClassID),
                new SqlParameter("@Month", month),
                new SqlParameter("@UserID", CurUserID),
                new SqlParameter("@InvoiceHeaderID", InvoiceHeaderID)

                };
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "GetClassesForKAMWiseTspInvoiceDetail", param.ToArray()).Tables[0];
                return dt;
                //return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }


        //public DataTable GetTSPInvoiceStatusDetails(int ClassID = 0, bool Running = false)
        //{
        //    try
        //    {
        //        List<SqlParameter> param = new List<SqlParameter>() {
        //        new SqlParameter("@ClassID", ClassID)
        //        };
        //        DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "GetClassesForTspInvoiceStatus", param.ToArray()).Tables[0];
        //        return dt;
        //        //return LoopinData(dt);
        //    }
        //    catch (Exception ex) { throw new Exception(ex.Message); }
        //}

        //public DataTable GetTSPInvoiceStatusDetailsMonthWise(DateTime? month, int CurUserID,int InvoiceHeaderID, int ClassID = 0)
        //{
        //    try
        //    {
        //        List<SqlParameter> param = new List<SqlParameter>() {
        //        new SqlParameter("@ClassID", ClassID),
        //        new SqlParameter("@Month", month),
        //        new SqlParameter("@UserID", CurUserID),
        //        new SqlParameter("@InvoiceHeaderID", InvoiceHeaderID)
        //        };
        //        DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "GetClassesForTspInvoiceDetail", param.ToArray()).Tables[0];
        //        return dt;
        //        //return LoopinData(dt);
        //    }
        //    catch (Exception ex) { throw new Exception(ex.Message); }
        //}
        public DataTable getTSPInvoiceStatusDataMonthWiseForKAM(DateTime? month, int CurUserID, int InvoiceHeaderID, int ClassID = 0)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>() {
                new SqlParameter("@ClassID", ClassID),
                new SqlParameter("@Month", month),
                new SqlParameter("@UserID", CurUserID),
                new SqlParameter("@InvoiceHeaderID", InvoiceHeaderID)
                };
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "GetClassesForKAMWiseTspInvoiceDetail", param.ToArray()).Tables[0];
                return dt;
                //return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }


        private List<ClassInvoiceMapExtModel> LoopinData(DataTable dt)
        {
            List<ClassInvoiceMapExtModel> ClassInvoiceMapL = new List<ClassInvoiceMapExtModel>();

            foreach (DataRow r in dt.Rows)
            {
                ClassInvoiceMapL.Add(RowOf(r));
            }
            return ClassInvoiceMapL;
        }

        private List<ClassInvoiceMapModel> LoopinInvData(DataTable dt)
        {
            List<ClassInvoiceMapModel> List = new List<ClassInvoiceMapModel>();

            foreach (DataRow r in dt.Rows)
            {
                List.Add(InvRowOf(r));
            }
            return List;
        }

        public ClassInvoiceMapModel GetInvoices(int InvoiceID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_ClassInvoiceMap", new SqlParameter("@InvoiceID", InvoiceID)).Tables[0];
                return InvRowOf(dt.Rows?[0]);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        private ClassInvoiceMapModel InvRowOf(DataRow r)
        {
            ClassInvoiceMapModel ClassInvoiceMap = new ClassInvoiceMapModel();
            ClassInvoiceMap.InvoiceID = Convert.ToInt32(r["InvoiceID"]);
            ClassInvoiceMap.ClassID = Convert.ToInt32(r["ClassID"]);
            if (r.Table.Columns.Contains("ClassCode"))
                ClassInvoiceMap.ClassCode = r["ClassCode"].ToString();
            ClassInvoiceMap.Amount = Convert.ToDecimal(r["Amount"]);
            ClassInvoiceMap.InvoiceNo = Convert.ToInt32(r["InvoiceNo"]);
            ClassInvoiceMap.InvoiceType = r["InvoiceType"].ToString();
            ClassInvoiceMap.Month = Convert.ToDateTime(r["Month"].ToString());

            return ClassInvoiceMap;
        }

        private ClassInvoiceMapExtModel RowOf(DataRow r)
        {
            ClassInvoiceMapExtModel ClassInvoiceMap = new ClassInvoiceMapExtModel();
            ClassInvoiceMap.InvoiceID = Convert.ToInt32(r["InvoiceID"]);
            ClassInvoiceMap.ClassID = Convert.ToInt32(r["ClassID"]);
            if (r.Table.Columns.Contains("ClassCode"))
                ClassInvoiceMap.ClassCode = r["ClassCode"].ToString();
            ClassInvoiceMap.InvSapID = r["InvSapID"].ToString();
            ClassInvoiceMap.SRNInvSapID = r["SRNInvSapID"].ToString();
            ClassInvoiceMap.POSapID = r["POSapID"].ToString();
            ClassInvoiceMap.Amount = Convert.ToDecimal(r["Amount"]);
            ClassInvoiceMap.InvoiceNo = Convert.ToInt32(r["InvoiceNo"]);
            ClassInvoiceMap.InvoiceType = r["InvoiceType"].ToString();
            ClassInvoiceMap.Month = Convert.ToDateTime(r["Month"].ToString());
            ClassInvoiceMap.MPRGenrated = Convert.ToBoolean(r["MPRGenrated"]);
            ClassInvoiceMap.RegenrateMPR = Convert.ToBoolean(r["RegenrateMPR"]);
            ClassInvoiceMap.InCancelation = Convert.ToBoolean(string.IsNullOrEmpty(r["InCancelation"].ToString()) ? 0 : r["InCancelation"]);
            ClassInvoiceMap.IsGenerated = Convert.ToBoolean(r["IsGenerated"]);
            ClassInvoiceMap.MPRID = Convert.ToInt32(r["MPRID"]);
            ClassInvoiceMap.PRNID = Convert.ToInt32(r["PRNID"]);
            ClassInvoiceMap.Invoices = Convert.ToInt32(r["Invoices"]);
            ClassInvoiceMap.SRNID = Convert.ToInt32(r["SRNID"]);
            ClassInvoiceMap.POLineID = Convert.ToInt32(r["POLineID"]);
            ClassInvoiceMap.SRNInvoice = Convert.ToInt32(r["SRNInvoice"]);
            return ClassInvoiceMap;
        }

        public DataTable GetMPR(int ID)
        {
            try
            {
                return SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "GetMPR", new SqlParameter("@ID", ID)).Tables[0];

            }
            catch (Exception ex) { throw ex; }
        }

        public DataTable GetSRN(int ID)
        {
            try
            {
                return SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "GetSRN", new SqlParameter("@ID", ID)).Tables[0];

            }
            catch (Exception ex) { throw ex; }
        }

        public DataTable GetTPRN(int ID)
        {
            try
            {
                return SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "GetTPRN", new SqlParameter("@ID", ID)).Tables[0];

            }
            catch (Exception ex) { throw ex; }
        }

        public DataTable GetPRN(int ID)
        {
            try
            {
                return SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "GetPRN", new SqlParameter("@ID", ID)).Tables[0];

            }
            catch (Exception ex) { throw ex; }
        }

        public DataTable GetPRNMaster(int ID)
        {
            try
            {
                return SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "CXL_GetPRN", new SqlParameter("@ID", ID)).Tables[0];

            }
            catch (Exception ex) { throw ex; }
        }

        public DataTable GetPO(int ID)
        {
            try
            {
                return SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "GetPO", new SqlParameter("@ID", ID)).Tables[0];

            }
            catch (Exception ex) { throw ex; }
        }

        public DataTable GetPOHeader(int ID)
        {
            try
            {
                return SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "CXL_GetPO", new SqlParameter("@ID", ID)).Tables[0];

            }
            catch (Exception ex) { throw ex; }
        }

        public DataTable GetInv(int ID)
        {
            try
            {
                return SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "GetInv", new SqlParameter("@ID", ID)).Tables[0];

            }
            catch (Exception ex) { throw ex; }
        }

        public DataTable GetInvHeader(int ID)
        {
            try
            {
                return SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "CXL_GetInv", new SqlParameter("@ID", ID)).Tables[0];

            }
            catch (Exception ex) { throw ex; }
        }
    }
}