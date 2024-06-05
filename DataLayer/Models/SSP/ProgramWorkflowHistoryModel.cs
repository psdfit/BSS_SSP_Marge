using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models.SSP
{
    public class ProgramWorkflowHistoryModel:ModelBase
    {
        public int ID { get; set; }
        public int ProgramID { get; set; }
        public int WorkflowID { get; set; }
        public string Remarks { get; set; }
        public int UserID { get; set; }
    }
}
