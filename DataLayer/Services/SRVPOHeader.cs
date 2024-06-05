using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using DataLayer.Interfaces;
using DataLayer.Models;
using DataLayer.Classes;

namespace DataLayer.Services
{
    public class SRVPOHeader : ISRVPOHeader
    {
        public List<POHeaderModel> FetchPOHeader(POHeaderModel model, SqlTransaction transaction = null)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@POHeaderID", model.POHeaderID));
                param.Add(new SqlParameter("@ProcessKey", model.ProcessKey));
                param.Add(new SqlParameter("@Month", model.Month));
                DataTable dt = new DataTable();
                if (transaction != null)
                {
                    dt = SqlHelper.ExecuteDataset(transaction, CommandType.StoredProcedure, "RD_POHeaders", param.ToArray()).Tables[0];
                }
                else
                {
                    dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_POHeaders", param.ToArray()).Tables[0];
                }
                return LoopinData(dt);
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }

        public List<SubmitedPOsModel> GetPOForApproval(int UserID)
        {
            try
            {
                SqlParameter param = new SqlParameter("@UserID", UserID);

                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "GetPOForApproval", param).Tables[0];
                return LoopinSubmitedPOs(dt);
            }
            catch (Exception e)
            { throw new Exception(e.Message); }
        }
        
        private List<POHeaderModel> LoopinData(DataTable dt)
        {
            List<POHeaderModel> POHeader = new List<POHeaderModel>();

            foreach (DataRow r in dt.Rows)
            {
                POHeader.Add(RowOfPOHeader(r));
            }

            return POHeader;
        }

        private POHeaderModel RowOfPOHeader(DataRow row)
        {
            POHeaderModel POHeader = new POHeaderModel();

            POHeader.POHeaderID = row.Field<int>("POHeaderID");
            POHeader.DocEntry = row.Field<int?>("DocEntry");
            POHeader.BPLId = row.Field<int?>("BPLId");
            POHeader.ProcessKey = row.Field<string>("ProcessKey");
            POHeader.DocNum = row.Field<string>("DocNum");
            POHeader.DocType = row.Field<string>("DocType");
            POHeader.Printed = row.Field<string>("Printed");
            POHeader.CardCode = row.Field<string>("CardCode");
            POHeader.CardName = row.Field<string>("CardName");
            POHeader.Comments = row.Field<string>("Comments");
            POHeader.JournalMemo = row.Field<string>("JournalMemo");
            POHeader.DocTotal = row.Field<decimal?>("DocTotal");
            POHeader.CtlAccount = row.Field<string>("CtlAccount");
            POHeader.U_SCHEME = row.Field<string>("U_SCHEME");
            POHeader.U_Sch_Code = row.Field<string>("U_Sch_Code");
            POHeader.DocDate = row.Field<DateTime?>("DocDate");
            POHeader.DocDueDate =row.Field<DateTime?>("DocDueDate");
            POHeader.CreatedDate = row.Field<DateTime?>("CreatedDate");
            POHeader.ModifiedDate = row.Field<DateTime?>("ModifiedDate");
            POHeader.CreatedUserID = row.Field<int?>("CreatedUserID");
            POHeader.ModifiedUserID = row.Field<int?>("ModifiedUserID");
            POHeader.SAPID = row.Field<string?>("SAPID");
            POHeader.Month = row.Field<DateTime>("Month");

            return POHeader;
        }

        private List<SubmitedPOsModel> LoopinSubmitedPOs(DataTable dt)
        {
            List<SubmitedPOsModel> POHeader = new List<SubmitedPOsModel>();

            foreach (DataRow r in dt.Rows)
            {
                POHeader.Add(SubmitedPOs(r));
            }

            return POHeader;
        }

        private SubmitedPOsModel SubmitedPOs(DataRow r)
        {
            SubmitedPOsModel POHeader = new SubmitedPOsModel();

            POHeader.POHeaderID = Convert.ToInt32(r["POHeaderID"]);
            POHeader.DocEntry = Convert.ToInt32(r["DocEntry"]);
            POHeader.BPLId = Convert.ToInt32(r["BPLId"]);
            POHeader.ProcessKey = r["ProcessKey"].ToString();
            POHeader.DocNum = r["DocNum"].ToString();
            POHeader.DocType = r["DocType"].ToString();
            POHeader.Printed = r["Printed"].ToString();
            POHeader.CardCode = r["CardCode"].ToString();
            POHeader.CardName = r["CardName"].ToString();
            POHeader.Comments = r["Comments"].ToString();
            POHeader.JournalMemo = r["JournalMemo"].ToString();
            POHeader.DocTotal = Convert.ToDecimal(r["DocTotal"]);
            POHeader.CtlAccount = r["CtlAccount"].ToString();
            POHeader.U_SCHEME = r["U_SCHEME"].ToString();
            POHeader.U_Sch_Code = r["U_Sch_Code"].ToString();
            POHeader.DocDate = r["DocDate"].ToString().GetDate();
            POHeader.DocDueDate = r["DocDueDate"].ToString().GetDate();
            POHeader.CreatedDate = r["CreatedDate"].ToString().GetDate();
            POHeader.ModifiedDate = r["ModifiedDate"].ToString().GetDate();
            POHeader.CreatedUserID = Convert.ToInt32(r["CreatedUserID"]);
            POHeader.ModifiedUserID = Convert.ToInt32(r["ModifiedUserID"]);
            POHeader.InActive = Convert.ToBoolean(r["InActive"]);

            POHeader.SendBack = Convert.ToBoolean(r["SendBack"]);
            POHeader.FormApprovalID = Convert.ToInt32(r["FormApprovalID"]);
            POHeader.Comment = r["Comment"].ToString();
            //POHeader.Month = r.Field<DateTime>("Month");

            return POHeader;
        }
        public bool UpdateSAPInPOHeader(int poHeaderId, int docEntry, string docNum, SqlTransaction transaction = null)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[10];
                param[0] = new SqlParameter("@POHeaderID", poHeaderId);
                param[1] = new SqlParameter("@DocEntry", docEntry);
                param[2] = new SqlParameter("@DocNum", docNum);
                if (transaction != null)
                {
                    SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, "[AU_POHeaderSAPID]", param);
                }
                else
                {
                    SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_POHeaderSAPID]", param);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<POHeaderModel> FetchPOHeaderByFilters(DateTime? POMonth, int schemeID, int tSPID, string processKey)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                if (POMonth != null)
                {
                    param.Add(new SqlParameter("@POMonth", POMonth));
                }
                param.Add(new SqlParameter("@SCHEMEID", schemeID));
                param.Add(new SqlParameter("@TSPID",tSPID));
                param.Add(new SqlParameter("@PROCESSKEY",processKey));
                DataTable dt = new DataTable();
                dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_POHeadersByFilters", param.ToArray()).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }
    }
}
