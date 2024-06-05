using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models.SSP
{
    public class WorkflowModel : ModelBase
    {
        public WorkflowModel() { }

        public int UserID { get; set; }
        public int WorkflowID { get; set; } = 0; 
        public string WorkflowTitle { get; set; }
        public int SourcingTypeID { get; set; }
        public string Description { get; set; }
        public string TotalDays { get; set; } 
        public string TotalTaskDays { get; set; } 
        public List<TaskDetail> taskDetails { get; set; } = new List<TaskDetail>(); 

    }

    public class TaskDetail
    {
        public int WorkflowID { get; set; } = 0; 
        public int UserID { get; set; }
        public int TaskID { get; set; } = 0;
        public string TaskName { get; set; }
        public string TaskDays { get; set; }
        public string TaskApproval { get; set; }
        public string TaskStatus { get; set; }
    }
}
