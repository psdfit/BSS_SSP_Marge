using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
    [Serializable]
    public class POLinesModel: ModelBase 
    {
        public int POLineID { get; set; }
        public int POHeaderID { get; set; }
        public int DocEntry { get; set; }
        public int DocNumber { get; set; }
        public int LineNum { get; set; }
        public string Dscription { get; set; }
        public string AcctCode { get; set; }
        public string OcrCode { get; set; }
        public string TaxCode { get; set; }
        public string WtLiable { get; set; }
        public string LineStatus { get; set; }
        public decimal? LineTotal { get; set; }
        public string OcrCode2 { get; set; }
        public string OcrCode3 { get; set; }
        public string U_Class_Code { get; set; }
        public string U_Batch { get; set; }
        public string U_Batch_Duration { get; set; }
        public decimal? U_Training_Cost { get; set; }
        public decimal? U_Stipend { get; set; }
        public decimal? U_Uniform_Bag { get; set; }
        public int U_Trainee_Per_Class { get; set; }
        public decimal? U_Testing_Fee { get; set; }
        public decimal? U_Cost_Trainee_Month { get; set; }
        public decimal? U_Cost_Trainee_LMont { get; set; }
        public decimal? U_Cost_Trainee_FMont { get; set; }
        public decimal? U_Cost_Trai_2nd_Last { get; set; }
        public decimal? U_Cost_Trainee_2Mont { get; set; }
        public DateTime U_Class_Start_Date { get; set; }
        public DateTime U_Class_End_Date { get; set; }
    }
}
