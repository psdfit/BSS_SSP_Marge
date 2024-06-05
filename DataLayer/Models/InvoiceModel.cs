using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
    [Serializable]
    public class InvoiceModel : ModelBase
    {
        public int ID { get; set; }
        public int SchemeID { get; set; }
        public int ClassID { get; set; }
        public int TradeID { get; set; }
        public int ProgCategory { get; set; }
        public int UnverifiedCNICDeductions { get; set; }
        public int DeductionTraineeDroput { get; set; }
        public int DeductionTraineeAttendance { get; set; }
        public int MiscDeductionNo { get; set; }
        public int ResultDeduction { get; set; }
        public int Batch { get; set; }
        public int BatchDuration { get; set; }
        public int DeductionTraineeUnVCnic { get; set; }
        public int ClassDays { get; set; }
        public int TraineePerClass { get; set; }
        public int ClaimTrainees { get; set; }
        public int MiscProfileDeductionCount { get; set; }

        public string Description { get; set; }
        public string PCategoryName { get; set; }
        public string GLCode { get; set; }
        public string GLName { get; set; }
        public string TrainingServicesSaleTax { get; set; }
        public string FundingSource { get; set; }
        public string TaxCode { get; set; }
        public string InvoiceType { get; set; }
        public string SchemeName { get; set; }
        public string ClassCode { get; set; }
        public string TradeName { get; set; }
        public string SchemeCode { get; set; }
        public string WTaxLiable { get; set; }
        public string ProcessKey { get; set; }
        public string CnicDeductionType { get; set; }
        public string DropOutDeductionType { get; set; }
        public string MiscDeductionType { get; set; }
        public string OcrCode { get; set; }
        public string OcrCode2 { get; set; }
        public string OcrCode3 { get; set; }
        public string LineStatus { get; set; }
        public double Stipend { get; set; }
        public double UniformBag { get; set; }
        public double BoardingOrLodging { get; set; }
        public double TestingFee { get; set; }
        public double TotalCostPerTrainee { get; set; }
        public double CnicDeductionAmount { get; set; }
        public double AttendanceDeductionAmount { get; set; }
        public double MiscDeductionAmount { get; set; }
        public double DropOutDeductionAmount { get; set; }
        public double PenaltyPercentage { get; set; }
        public double PenaltyAmount { get; set; }
        public double NetPayableAmount { get; set; }
        public double NetTrainingCost { get; set; }
        public double TotalMonthlyPayment { get; set; }
        public double GrossPayable { get; set; }
        public double MiscProfileDeductionAmount { get; set; }


        public bool IsApproved { get; set; }
        public bool IsRejected { get; set; }
        public bool IsPostedToSAP { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime ActualStartDate { get; set; }
        public DateTime ActualEndDate { get; set; }
        public DateTime FunctionalDate { get; set; }
        public int InvoiceNumber { get;  set; }
        public double LineTotal { get;  set; }
        public double TotalLC { get;  set; }
        public string MyProperty { get; set; }
        public string BaseEntry { get; set; }
        public string BaseType { get; set; }
        public string BaseLine { get; set; }
        public int InvoiceHeaderID { get; set; }
        public int PaymentToBeReleasedTrainees { get; set; }
    }
}
