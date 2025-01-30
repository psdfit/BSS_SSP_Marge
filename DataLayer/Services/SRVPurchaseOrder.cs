using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using DataLayer.Interfaces;
using DataLayer.Models;
using System.Diagnostics;

namespace DataLayer.Services
{
    public class SRVPurchaseOrder : ISRVPurchaseOrder
    {
        public SRVPurchaseOrder()
        {

        }

        public List<POHeaderModel> GetPOHeaderByID(int POHeaderID, SqlTransaction transaction = null)
        {
            List<POHeaderModel> POHeaderModel = new List<POHeaderModel>();
            DataTable dt = new DataTable();
           // DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "[RD_GetPOHeaderByID]", new SqlParameter("@POHeaderID", POHeaderID)).Tables[0];

            if (transaction != null)
            {
                dt = SqlHelper.ExecuteDataset(transaction, CommandType.StoredProcedure, "[RD_GetPOHeaderByID]", new SqlParameter("@POHeaderID", POHeaderID)).Tables[0];
            }
            else
            {
                dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "[RD_GetPOHeaderByID]", new SqlParameter("@POHeaderID", POHeaderID)).Tables[0];

            }
            if (dt.Rows.Count > 0)
            {
                POHeaderModel = Helper.ConvertDataTableToModel<POHeaderModel>(dt);
                return POHeaderModel;
            }
            else { 
            return null;

            }
        }

        public bool CreatePO(int schemeID,string processKey, int curuserID, SqlTransaction transaction = null)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@SchemeID", schemeID));
                param.Add(new SqlParameter("@CurUserID", curuserID));
                param.Add(new SqlParameter("@ProcessKey", processKey));

                if (transaction != null)
                {
                    SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, "POLinesAndHeader", param.ToArray());
                }
                else
                {
                    SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "POLinesAndHeader", param.ToArray());
                }
                return true;
            }
            catch (Exception e)
            { throw new Exception(e.Message); }
        }
        public bool CreatePOForSRN(string srnIDs,string processKey, int curuserID, SqlTransaction transaction = null)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@SRNIDs", srnIDs));
                param.Add(new SqlParameter("@CurUserID", curuserID));
                param.Add(new SqlParameter("@ProcessKey", processKey));

                if (transaction != null)
                {
                    SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, "POForSRN", param.ToArray());
                }
                else
                {
                    SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "POForSRN", param.ToArray());
                }
                return true;
            }
            catch (Exception e)
            { throw new Exception(e.Message); }
        }
public bool CreatePOForGURN(string gurnIDs,string processKey, int curuserID, SqlTransaction transaction = null)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@GURNIDs", gurnIDs));
                param.Add(new SqlParameter("@CurUserID", curuserID));
                param.Add(new SqlParameter("@ProcessKey", processKey));

                if (transaction != null)
                {
                    SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, "POForGURN", param.ToArray());
                }
                else
                {
                    SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "POForGURN", param.ToArray());
                }
                return true;
            }
            catch (Exception e)
            { throw new Exception(e.Message); }
        }
        public bool CreatePOForTRN(int srnID, int curuserID, SqlTransaction transaction = null)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@TRNMasterID", srnID));
                param.Add(new SqlParameter("@CurUserID", curuserID));
                param.Add(new SqlParameter("@ProcessKey", EnumApprovalProcess.PO_TRN));

                if (transaction != null)
                {
                    SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, "POForTRN", param.ToArray());
                }
                else
                {
                    SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "POForTRN", param.ToArray());
                }
                return true;
            }
            catch (Exception e)
            { throw new Exception(e.Message); }
        }
        public bool POHeaderApproveReject(POHeaderModel model, SqlTransaction transaction = null)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@POHeaderID", model.POHeaderID));
                param.Add(new SqlParameter("@IsApproved", model.IsApproved));
                param.Add(new SqlParameter("@IsRejected", model.IsRejected));
                param.Add(new SqlParameter("@CurUserID", model.ModifiedUserID));

                if (transaction != null)
                {
                    SqlHelper.ExecuteScalar(transaction, CommandType.StoredProcedure, "U_POHeaderApproveReject", param.ToArray());
                }
                else
                {
                    SqlHelper.ExecuteScalar(SqlHelper.GetCon(), CommandType.StoredProcedure, "U_POHeaderApproveReject", param.ToArray());
                }
                return true;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
    }
}
