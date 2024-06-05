using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
    public class ClassProceeedingStatusData
    {
        public string ClassCode { get; set; }
        public DateTime? MonthOfMPR { get; set; }
        public bool IsGeneratedMPR { get; set; }
        public bool IsDataInsertedInAMS { get; set; }
        public bool IsGeneratedPRNRegular { get; set; }
        public bool IsGeneratedPRNRegularPO { get; set; }
        public bool IsGeneratedPRNRegularInvoice { get; set; }
        public bool IsGeneratedSRN { get; set; }
        public bool IsGeneratedSRNPO { get; set; }
        public bool IsGeneratedSRNInvoice { get; set; }
    }
}
