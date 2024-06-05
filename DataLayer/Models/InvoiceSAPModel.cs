using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
    public class InvoiceSAPModel
    {
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public string DocDate { get; set; }
        public string DocDueDate { get; set; }
        public string TaxDate { get; set; }
        public string U_Scheme { get; set; }
        public string U_sch_code { get; set; }
        public string U_Month { get; set; }
        public int U_IPS { get; set; }
        public int BranchID { get; set; }
        public List<ApinvoiceDetail> APInvoiceDetail { get; set; }
    }

    public class ApinvoiceDetail
    {
        public string U_Class_Code { get; set; }
        public string U_Invoice_Number { get; set; }
        public string OcrCode { get; set; }
        public string OcrCode2 { get; set; }
        public string U_Batch_Duration { get; set; }
        public string U_Class_Start_Date { get; set; }
        public string U_Class_End_Date { get; set; }
        public string U_Class_AStart_Date { get; set; }
        public string U_Class_AEnd_Date { get; set; }
        public int U_Tranee_Per_Class { get; set; }
        public string U_Batch { get; set; }
        public string U_nettrainingcost { get; set; }
        public string U_Stipend_Per_Traine { get; set; }
        public string U_Uniform_Bag { get; set; }
        public string U_Claimed_Trainee { get; set; }
        public string U_No_Unverified_CNIC { get; set; }
        public string U_CNIC_Cat { get; set; }
        public string U_Unverified_CNIC { get; set; }
        public string U_No_Dropout { get; set; }
        public string U_Drpoutded { get; set; }
        public double U_Dropout { get; set; }
        public int U_No_Attend_Shortfal { get; set; }
        public double U_Attend_Shortfall { get; set; }
        public int U_No_Misc_Deduction { get; set; }
        public string U_MiscDedCat { get; set; }
        public double U_Misc_Deduction { get; set; }
        public double U_Penalty { get; set; }
        public double U_Percapld { get; set; }
        public double U_Result_Deduction { get; set; }
        public double U_GrossPayable { get; set; }
        public string AcctCode { get; set; }
        public string OcrCode3 { get; set; }
        public string OcrCode4 { get; set; }
        public string Description { get; set; }
        public string U_Claimed_Trainees { get; set; }
        public double LineTotal { get; set; }
        public string WtLiable { get; set; }
        public double U_Total_Monthly_Pay { get; set; }
        public double U_Testing_Fee { get; set; }
        public double U_Boarding_Loadging { get; set; }
        public double U_T_Cost_Per_Trainee { get; set; }
        public double U_Net_Invoice_Pay { get; set; }
        public string U_AP_Ref { get; set; }
        public double U_STValue { get; set; }
        public double U_ST_Amount { get; set; }
        public int U_NoProfileDed { get; set; }
        public double U_Stipend { get; set; }
        public double U_Linetotal { get; set; }
        public int U_Trainee_Per_Class { get; set; }
        public string U_Remarks { get; set; }
        public string U_Location { get; set; }
        public string VatGroup { get; set; }
        public string LineStatus { get; set; }
        public string BaseEntry { get; set; }
        public string BaseType { get; set; }
        public string BaseLine { get; set; }
        public string PODocEntry { get; set; }

        //public string U_No_Unverified_CNIC { get; internal set; }
    }

}
