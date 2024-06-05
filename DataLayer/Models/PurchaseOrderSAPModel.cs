using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
    public class PurchaseOrderSAPModel
    {
        public string Printed { get; set; }
        public string DocDate { get; set; }
        public string DocDueDate { get; set; }
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public string JournalMemo { get; set; }
        public string Comments { get; set; }
        public string U_SCH_Code { get; set; }
        public string U_SCHEME { get; set; }
        public string U_Month { get; set; }
        public int U_IPS { get; set; }
        public List<POLinesSAP> PODetail { get; set; }
        public int BranchID { get; set; }

    }
    public class POLinesSAP
    {
        public string Description { get; set; }
        public string AcctCode { get; set; }
        public string OcrCode { get; set; }
        public string TaxCode { get; set; }
        public string WTLiable { get; set; }
        public string LineTotal { get; set; }
        public string LineStatus { get; set; }
        public string U_Class_Code { get; set; }
        public string U_Batch { get; set; }
        public string U_Batch_Duration { get; set; }
        public string U_Training_Cost { get; set; }
        public string U_Stipend { get; set; }
        public string U_Uniform_Bag { get; set; }
        public string U_Trainee_Per_Class { get; set; }
        public string U_Testing_Fee { get; set; }
        public string U_Cost_Trainee_LMont { get; set; }
        public string U_Cost_Trainee_FMont { get; set; }
        public string U_Cost_Trai_2nd_Last { get; set; }
        public string OcrCode2 { get; set; }
        public string OcrCode3 { get; set; }
        public string U_Class_Start_Date { get; set; }
        public string U_Class_End_Date { get; set; }
        public string U_Cost_Trainee_2Mont { get; set; }
        public string U_Cost_Trainee_Month { get; set; }
    }


}
