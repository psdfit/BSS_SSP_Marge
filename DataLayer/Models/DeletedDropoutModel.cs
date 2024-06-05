using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
    [Serializable]
    public class DeletedDropoutModel: ModelBase
    {

        public DeletedDropoutModel() : base()
        {
        }
        public string SchemeName { get; set; }
        public string DetailString { get; set; }
        public string TSPName { get; set; }
        public string ClassCode { get; set; }
        public string TraineeName { get; set; }
        public string FatherName { get; set; }
        public string TraineeCNIC { get; set; }
        public string PreviousMonth { get; set; }
        public string SelectedMonth { get; set; }
        public string TickStatusPreviousMonth { get; set; }
        public string TickStatusSelectedMonth { get; set; }
        public string PreviousMonthDeleted { get; set; }
        public string SelectedMonthDeleted { get; set; }
        public string Remarks { get; set; }
        public DateTime StartDate { get; set; }
        public string StatusInMIS { get; set; }
        public string ActionToBeTaken { get; set; }

    }
}
