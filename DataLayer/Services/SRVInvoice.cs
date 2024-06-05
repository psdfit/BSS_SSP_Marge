using System;
using System.Collections.Generic;
using System.Text;
using DataLayer.Models;
using DataLayer.Interfaces;
using DataLayer.Classes;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Data;

namespace DataLayer.Services
{
    public class SRVInvoice : ISRVInvoice
    {
        public List<InvoiceModel> GetInvoicesForApproval(int id, SqlTransaction _transaction = null)
        {
            try
            {
                DataTable dt = new DataTable();
                if (_transaction != null)
                {
                    dt = SqlHelper.ExecuteDataset(_transaction, CommandType.StoredProcedure, "RD_Invoice", new SqlParameter("@InvoiceHeaderID", id)).Tables[0];
                }
                else
                {
                    dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Invoice", new SqlParameter("@InvoiceHeaderID", id)).Tables[0];
                }
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public void GenerateInvoice(string ClassCode, int GLAccountID, int InvoiceNumber, SqlTransaction _transaction, string ProcessKey)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@ClassCode", ClassCode);
                param[1] = new SqlParameter("@GLAccountID", GLAccountID);
                param[2] = new SqlParameter("@InvoiceNumber", InvoiceNumber);
                param[3] = new SqlParameter("@ProcessKey", ProcessKey);

                if (_transaction != null)
                    SqlHelper.ExecuteNonQuery(_transaction, CommandType.StoredProcedure, "GenerateInvoiceRegular", param);
                else
                    SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "GenerateInvoiceRegular", param);

            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        private List<InvoiceModel> LoopinData(DataTable dt)
        {
            List<InvoiceModel> inv = new List<InvoiceModel>();

            foreach (DataRow r in dt.Rows)
            {
                inv.Add(RowOfInvoice(r));
            }
            return inv;
        }

        private InvoiceModel RowOfInvoice(DataRow r)
        {
            InvoiceModel Invoice = new InvoiceModel();

            Invoice.ID = Convert.ToInt32(r["ID"]);
            Invoice.SchemeID = Convert.ToInt32(r["SchemeID"]);
            Invoice.ClassID = Convert.ToInt32(r["ClassID"]);
            Invoice.TradeID = Convert.ToInt32(r["TradeID"]);
            Invoice.ProgCategory = Convert.ToInt32(r["ProgCategory"]);
            Invoice.UnverifiedCNICDeductions = Convert.ToInt32(r["UnverifiedCNICDeductions"]);
            Invoice.DeductionTraineeDroput = Convert.ToInt32(r["DeductionTraineeDroput"]);
            Invoice.DeductionTraineeAttendance = Convert.ToInt32(r["DeductionTraineeAttendance"]);
            Invoice.MiscDeductionNo = Convert.ToInt32(r["MiscDeductionNo"]);
            Invoice.ResultDeduction = Convert.ToInt32(r["ResultDeduction"]);
            Invoice.Batch = Convert.ToInt32(r["Batch"]);
            Invoice.BatchDuration = Convert.ToInt32(r["BatchDuration"]);
            Invoice.DeductionTraineeUnVCnic = Convert.ToInt32(r["DeductionTraineeUnVCnic"]);
            Invoice.ClassDays = Convert.ToInt32(r["ClassDays"]);
            Invoice.TraineePerClass = Convert.ToInt32(r["TraineePerClass"]);
            Invoice.ClaimTrainees = Convert.ToInt32(r["ClaimTrainees"]);

            Invoice.Description = r["Description"].ToString();
            Invoice.OcrCode = r["OcrCode"].ToString();
            Invoice.OcrCode2 = r["OcrCode2"].ToString();
            Invoice.OcrCode3 = r["OcrCode3"].ToString();
            Invoice.LineStatus = r["LineStatus"].ToString();
            Invoice.PCategoryName = r["PCategoryName"].ToString();
            Invoice.GLCode = r["GLCode"].ToString();
            Invoice.GLName = r["GLName"].ToString();
            Invoice.WTaxLiable = r["WTaxLiable"].ToString();
            Invoice.TrainingServicesSaleTax = r["TrainingServicesSaleTax"].ToString();
            Invoice.FundingSource = r["FundingSource"].ToString();
            Invoice.TaxCode = r["TaxCode"].ToString();
            Invoice.InvoiceType = r["InvoiceType"].ToString();
            Invoice.CnicDeductionType = r["CnicDeductionType"].ToString();
            Invoice.DropOutDeductionType = r["DropOutDeductionType"].ToString();
            Invoice.MiscDeductionType = r["MiscDeductionType"].ToString();
            Invoice.SchemeName = r["SchemeName"].ToString();
            Invoice.SchemeCode = r["SchemeCode"].ToString();
            Invoice.TradeName = r["TradeName"].ToString();
            Invoice.ClassCode = r["ClassCode"].ToString();
            Invoice.ProcessKey = r["ProcessKey"].ToString();
            Invoice.InvoiceNumber = Convert.ToInt32(r["InvoiceNumber"]);
            Invoice.MiscProfileDeductionCount = Convert.ToInt32(r["MiscProfileDeductionCount"]);
            Invoice.LineTotal = Convert.ToDouble(r["LineTotal"]);
            Invoice.TotalLC = Convert.ToDouble(r["TotalLC"]);
            Invoice.MiscProfileDeductionAmount = Convert.ToDouble(r["MiscProfileDeductionAmount"]);
            Invoice.TotalCostPerTrainee = Convert.ToDouble(r["TotalCostPerTrainee"]);
            Invoice.CnicDeductionAmount = Convert.ToDouble(r["CnicDeductionAmount"]);
            Invoice.DropOutDeductionAmount = Convert.ToDouble(r["DropOutDeductionAmount"]);
            Invoice.AttendanceDeductionAmount = Convert.ToDouble(r["AttendanceDeductionAmount"]);
            Invoice.PenaltyPercentage = Convert.ToDouble(r["PenaltyPercentage"]);
            Invoice.MiscDeductionAmount = Convert.ToDouble(r["MiscDeductionAmount"]);
            Invoice.PenaltyAmount = Convert.ToDouble(r["PenaltyAmount"]);
            Invoice.NetPayableAmount = Convert.ToDouble(r["NetPayableAmount"]);
            Invoice.NetTrainingCost = Convert.ToDouble(r["NetTrainingCost"]);
            Invoice.TotalMonthlyPayment = Convert.ToDouble(r["TotalMonthlyPayment"]);
            Invoice.GrossPayable = Convert.ToDouble(r["GrossPayable"]);
            Invoice.TestingFee = Convert.ToDouble(r["TestingFee"]);
            Invoice.Stipend = Convert.ToDouble(r["Stipend"]);
            Invoice.UniformBag = Convert.ToDouble(r["UniformBag"]);
            Invoice.BoardingOrLodging = Convert.ToDouble(r["BoardingOrLodging"]);

            Invoice.InActive = Convert.ToBoolean(r["InActive"]);
            Invoice.IsApproved = Convert.ToBoolean(r["IsApproved"]);
            Invoice.IsRejected = Convert.ToBoolean(r["IsRejected"]);

            Invoice.CreatedUserID = Convert.ToInt32(r["CreatedUserID"]);
            Invoice.ModifiedUserID = Convert.ToInt32(r["ModifiedUserID"]);
            Invoice.CreatedDate = r["CreatedDate"].ToString().GetDate();
            Invoice.ModifiedDate = r["ModifiedDate"].ToString().GetDate();
            Invoice.StartDate = r["StartDate"].ToString().GetDate();
            Invoice.EndDate = r["EndDate"].ToString().GetDate();
            Invoice.ActualStartDate = r["ActualStartDate"].ToString().GetDate();
            Invoice.ActualEndDate = r["ActualEndDate"].ToString().GetDate();
            Invoice.FunctionalDate = r["FunctionalDate"].ToString().GetDate();
            Invoice.BaseEntry = r["BaseEntry"].ToString();
            Invoice.BaseType = r["BaseType"].ToString();
            Invoice.BaseLine = r["BaseLine"].ToString();
            if (r.Table.Columns.Contains("InvoiceHeaderID"))
            {
                Invoice.InvoiceHeaderID = Convert.ToInt32(r["InvoiceHeaderID"]);
            }  
            
            if (r.Table.Columns.Contains("PaymentToBeReleasedTrainees"))
            {
                Invoice.PaymentToBeReleasedTrainees = Convert.ToInt32(string.IsNullOrEmpty(r["PaymentToBeReleasedTrainees"].ToString()) ? 0 : r["PaymentToBeReleasedTrainees"]);

            }
            //Payment TO be released added on 18-03-2022
            //Added by Rao Ali Haider

            return Invoice;
        }

        public bool UpdateSAPObjIdInInvoices(string sapObjId, int invHeaderId, SqlTransaction _transaction)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[10];
                param[0] = new SqlParameter("@InvoiceHeaderID", invHeaderId);
                param[1] = new SqlParameter("@SAPID", sapObjId);
                if (_transaction != null)
                {
                    SqlHelper.ExecuteNonQuery(_transaction, CommandType.StoredProcedure, "[AU_InvoiceSAPID]", param);
                }
                else
                {
                    SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_InvoiceSAPID]", param);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public List<InvoiceModel> GetInvoiceExcelExportByIDs(string ids)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Invoice_By_IDs", new SqlParameter("@InvoiceHeaderIDs", ids)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
    }
}
