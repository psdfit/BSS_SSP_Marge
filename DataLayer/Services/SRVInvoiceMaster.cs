using System;
using System.Collections.Generic;
using System.Text;
using DataLayer.Models;
using DataLayer.Interfaces;
using DataLayer.Classes;
using DataLayer.Models;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;

namespace DataLayer.Services
{
    public class SRVInvoiceMaster : ISRVInvoiceMaster
    {
        public List<InvoiceMasterModel> GetInvoiceDetails(InvoiceMasterModel model, SqlTransaction _transaction)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@InvoiceHeaderID", model.InvoiceHeaderID));
                param.Add(new SqlParameter("@ProcessKey", model.ProcessKey));
                param.Add(new SqlParameter("@OID", model.OID));
                param.Add(new SqlParameter("@KAMID", model.KAMID));
                param.Add(new SqlParameter("@SchemeID", model.SchemeID));
                //param.Add(new SqlParameter("@TSPID", model.TSPID));
                param.Add(new SqlParameter("@TSPMasterID", model.TSPMasterID));
                param.Add(new SqlParameter("@U_Month", model.U_Month.HasValue ? model.U_Month.Value : model.U_Month));
                DataTable dt = new DataTable();
                if (_transaction != null)
                {
                    dt = SqlHelper.ExecuteDataset(_transaction, CommandType.StoredProcedure, "RD_InvoiceDetails", param.ToArray()).Tables[0];
                }
                else
                {
                    dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_InvoiceDetails", param.ToArray()).Tables[0];
                }
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<InvoiceMasterModel> GetInvoicesForApproval(InvoiceMasterModel model, SqlTransaction _transaction)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@InvoiceHeaderID", model.InvoiceHeaderID));
                param.Add(new SqlParameter("@ProcessKey", model.ProcessKey));
                param.Add(new SqlParameter("@OID", model.OID));
                param.Add(new SqlParameter("@KAMID", model.KAMID));
                param.Add(new SqlParameter("@SchemeID", model.SchemeID));
                //param.Add(new SqlParameter("@TSPID", model.TSPID));
                param.Add(new SqlParameter("@TSPMasterID", model.TSPMasterID));
                param.Add(new SqlParameter("@U_Month", model.U_Month.HasValue ? model.U_Month.Value : model.U_Month));
                DataTable dt = new DataTable();
                if (_transaction != null)
                {
                    dt = SqlHelper.ExecuteDataset(_transaction, CommandType.StoredProcedure, "RD_InvoiceHeader", param.ToArray()).Tables[0];
                }
                else
                {
                    dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_InvoiceHeader", param.ToArray()).Tables[0];
                }
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        
        public List<InvoiceMasterModel> GetInvoicesForTSP(InvoiceMasterModel model, SqlTransaction _transaction)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                //param.Add(new SqlParameter("@InvoiceHeaderID", model.InvoiceHeaderID));
                //param.Add(new SqlParameter("@ProcessKey", model.ProcessKey));
                param.Add(new SqlParameter("@OID", model.OID));
                //param.Add(new SqlParameter("@KAMID", model.KAMID));
                param.Add(new SqlParameter("@SchemeID", model.SchemeID));
                param.Add(new SqlParameter("@TSPID", model.TSPID));
                param.Add(new SqlParameter("@U_Month", model.U_Month.HasValue ? model.U_Month.Value : model.U_Month));
                DataTable dt = new DataTable();
                if (_transaction != null)
                {
                    dt = SqlHelper.ExecuteDataset(_transaction, CommandType.StoredProcedure, "RD_InvoiceHeaderForTSP", param.ToArray()).Tables[0];
                }
                else
                {
                    dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_InvoiceHeaderForTSP", param.ToArray()).Tables[0];
                }
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public DataTable GetInvoiceHeaderForTSP(InvoiceMasterModel model, SqlTransaction _transaction)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                //param.Add(new SqlParameter("@InvoiceHeaderID", model.InvoiceHeaderID));
                //param.Add(new SqlParameter("@ProcessKey", model.ProcessKey));
                param.Add(new SqlParameter("@OID", model.OID));
                //param.Add(new SqlParameter("@KAMID", model.KAMID));
                param.Add(new SqlParameter("@SchemeID", model.SchemeID));
                param.Add(new SqlParameter("@TSPID", model.TSPID));
                param.Add(new SqlParameter("@U_Month", model.U_Month.HasValue ? model.U_Month.Value : model.U_Month));
                param.Add(new SqlParameter("@ClassID", model.ClassID));

                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_InvoiceHeaderForTSP", param.ToArray()).Tables[0];
                return dt;
                //return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        
        public DataTable GetInvoiceHeaderForInternalUser(InvoiceMasterModel model, SqlTransaction _transaction)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                //param.Add(new SqlParameter("@InvoiceHeaderID", model.InvoiceHeaderID));
                //param.Add(new SqlParameter("@ProcessKey", model.ProcessKey));
                param.Add(new SqlParameter("@OID", model.OID));
                //param.Add(new SqlParameter("@KAMID", model.KAMID));
                param.Add(new SqlParameter("@SchemeID", model.SchemeID));
                param.Add(new SqlParameter("@TSPID", model.TSPID));
                param.Add(new SqlParameter("@U_Month", model.U_Month.HasValue ? model.U_Month.Value : model.U_Month));
                param.Add(new SqlParameter("@ClassID", model.ClassID));

                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_InvoiceHeaderForInternalUser", param.ToArray()).Tables[0];
                return dt;
                //return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public DataTable GetInvoiceHeaderForKAM(InvoiceMasterModel model, SqlTransaction _transaction)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                //param.Add(new SqlParameter("@InvoiceHeaderID", model.InvoiceHeaderID));
                //param.Add(new SqlParameter("@ProcessKey", model.ProcessKey));
                param.Add(new SqlParameter("@OID", model.OID));
                param.Add(new SqlParameter("@KAMID", model.KAMID));
                param.Add(new SqlParameter("@SchemeID", model.SchemeID));
                //param.Add(new SqlParameter("@TSPID", model.TSPID));
                param.Add(new SqlParameter("@U_Month", model.U_Month.HasValue ? model.U_Month.Value : model.U_Month));
                param.Add(new SqlParameter("@ClassID", model.ClassID));

                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_InvoiceHeaderForKAM", param.ToArray()).Tables[0];
                return dt;
                //return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public void GenerateInvoiceHeader(int PRNMasterID, SqlTransaction _transaction, string ProcessKey)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@PRNMasterID", PRNMasterID);
                param[1] = new SqlParameter("@ProcessKey", ProcessKey);

                if (_transaction != null)
                    SqlHelper.ExecuteNonQuery(_transaction, CommandType.StoredProcedure, "GenerateInvoiceHeader", param);
                else
                    SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "GenerateInvoiceHeader", param);

            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public void GenerateINVCompletion(int PRNMasterID, string ProcessKey, SqlTransaction _transaction = null)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@PRNMasterID", PRNMasterID);
                param[1] = new SqlParameter("@ProcessKey", ProcessKey);

                if (_transaction != null)
                    SqlHelper.ExecuteNonQuery(_transaction, CommandType.StoredProcedure, "GenerateINVCompletion", param);
                else
                    SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "GenerateINVCompletion", param);

            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public void GenerateINVEmployment(int PRNMasterID, string ProcessKey, SqlTransaction _transaction = null)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@PRNMasterID", PRNMasterID);
                param[1] = new SqlParameter("@ProcessKey", ProcessKey);

                if (_transaction != null)
                    SqlHelper.ExecuteNonQuery(_transaction, CommandType.StoredProcedure, "GenerateINVEmployment", param);
                else
                    SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "GenerateINVEmployment", param);

            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        private List<InvoiceMasterModel> LoopinData(DataTable dt)
        {
            List<InvoiceMasterModel> inv = new List<InvoiceMasterModel>();

            foreach (DataRow r in dt.Rows)
            {
                inv.Add(RowOfInvoice(r));
            }
            return inv;
        }
        private InvoiceMasterModel RowOfInvoice(DataRow row)
        {
            InvoiceMasterModel Invoice = new InvoiceMasterModel();
            if (row.Table.Columns.Contains("NetPayableAmount"))
            {
                Invoice.U_IPS = row.Field<int>("U_IPS");
                Invoice.ClassCode = row.Field<string>("ClassCode");
                Invoice.SchemeName = row.Field<string>("SchemeName");
                Invoice.SchemeCode = row.Field<string>("SchemeCode");
                Invoice.TSPName = row.Field<string>("TSPName");
                Invoice.TSPSAPCode = row.Field<string>("TSPSAPCode");
                Invoice.InvoiceMonth = row.Field<DateTime>("InvoiceMonth");
                Invoice.InvoiceStatus = row.Field<string>("InvoiceStatus");
                Invoice.NetPayableAmount = row.Field<decimal>("NetPayableAmount");
                Invoice.InvoiceType = row.Field<string>("InvoiceType");
            }
            else
            {
                Invoice.InvoiceHeaderID = row.Field<int>("InvoiceHeaderID");
                Invoice.DocNum = row.Field<int?>("DocNum");
                Invoice.BPL_IDAssignedToInvoice = row.Field<int>("BPL_IDAssignedToInvoice");
                Invoice.IsApproved = row.Field<bool>("IsApproved");
                Invoice.IsRejected = row.Field<bool>("IsRejected");
                Invoice.DocType = row.Field<string>("DocType");
                Invoice.Printed = row.Field<string>("Printed");
                Invoice.CardCode = row.Field<string>("CardCode");
                Invoice.CardName = row.Field<string>("CardName");
                Invoice.JournalMemo = row.Field<string>("JournalMemo");
                Invoice.CtlAccount = row.Field<string>("CtlAccount");
                Invoice.U_SCHEME = row.Field<string>("U_SCHEME");
                Invoice.U_SCH_Code = row.Field<string>("U_SCH_Code");
                Invoice.SAPCODE = row.Field<string>("SAPCODE");
                Invoice.POSAPCODE = row.Field<string>("POSAPCODE");
                Invoice.ProcessKey = row.Field<string>("ProcessKey");
                Invoice.TSPName = row.Field<string>("TSPName");
                Invoice.TSPID = row.Field<int>("TSPID");
                Invoice.Comments = row.Field<string>("Comments");
                Invoice.TSPColorCode = row.Field<string>("TSPColorCode");
                Invoice.TSPColorName = row.Field<string>("TSPColorName");
                Invoice.U_Month = row.Field<DateTime>("U_Month");
            }

            return Invoice;
        }

        public bool InvoiceHeaderApproveReject(InvoiceMasterModel model, SqlTransaction transaction = null)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@InvoiceHeaderID", model.InvoiceHeaderID));
                param.Add(new SqlParameter("@IsApproved", model.IsApproved));
                param.Add(new SqlParameter("@IsRejected", model.IsRejected));

                if (transaction != null)
                {
                    SqlHelper.ExecuteScalar(transaction, CommandType.StoredProcedure, "U_InvoiceHeaderApproveReject", param.ToArray());
                }
                else
                {
                    SqlHelper.ExecuteScalar(SqlHelper.GetCon(), CommandType.StoredProcedure, "U_InvoiceHeaderApproveReject", param.ToArray());
                }
                return true;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        ///////////////////////////// Generate Invoice for TRN --- (RISKY)  //////////////////////////////////////

        public void GenerateInvoiceHeader_TRN(int PoHeaderID, SqlTransaction _transaction, string ProcessKey)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@POHeaderID", PoHeaderID);
                param[1] = new SqlParameter("@ProcessKey", ProcessKey);

                if (_transaction != null)
                    SqlHelper.ExecuteNonQuery(_transaction, CommandType.StoredProcedure, "GenerateInvoiceHeader_TRN", param);
                else
                    SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "GenerateInvoiceHeader_TRN", param);

            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public bool UpdateSAPObjIdInInvoiceHeader(string sapObjIdINV, int invHeader, SqlTransaction transaction = null)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[10];
                param[0] = new SqlParameter("@InvoiceHeaderID", invHeader);
                param[1] = new SqlParameter("@SAPID", sapObjIdINV);
                if (transaction != null)
                {
                    SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, "[AU_InvoiceHeaderSAPID]", param);
                }
                else
                {
                    SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_InvoiceHeaderSAPID]", param);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public InvoiceMasterModel GetInvoicesForApproval_Notification(int InvoiceHeaderID, SqlTransaction transaction = null)
        {
            try
            {
                SqlParameter param = new SqlParameter("@InvoiceHeaderID", InvoiceHeaderID);
                DataTable dt = new DataTable();
                List<InvoiceMasterModel> InvoiceMasterModel = new List<InvoiceMasterModel>();
               // dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_InvoiceHeader_Notification", param).Tables[0];
                if (transaction != null)
                {
                    dt = SqlHelper.ExecuteDataset(transaction, CommandType.StoredProcedure, "RD_InvoiceHeader_Notification", param).Tables[0];
                }
                else
                {
                    dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_InvoiceHeader_Notification", param).Tables[0];

                }
                if (dt.Rows.Count > 0)
                {
                    InvoiceMasterModel = Helper.ConvertDataTableToModel<InvoiceMasterModel>(dt);

                    return InvoiceMasterModel[0];
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public InvoiceHeaderModel GetInvoiceHeader(int TSPID, DateTime? Month, SqlTransaction transaction = null)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@TSPID", TSPID);
                param[1] = new SqlParameter("@Month", Month);
                DataTable dt = new DataTable();
                List<InvoiceHeaderModel> InvoiceHeaderModel = new List<InvoiceHeaderModel>();
                if (transaction != null)
                {
                    dt = SqlHelper.ExecuteDataset(transaction, CommandType.StoredProcedure, "RD_GetInvoiceHeader", param).Tables[0];
                }
                else
                {
                    dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_GetInvoiceHeader", param).Tables[0];
                }
                if (dt.Rows.Count > 0)
                {
                    InvoiceHeaderModel = Helper.ConvertDataTableToModel<InvoiceHeaderModel>(dt);
                    return InvoiceHeaderModel[0];
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
    }
}
