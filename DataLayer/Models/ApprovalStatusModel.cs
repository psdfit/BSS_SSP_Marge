using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
   public class ApprovalStatusModel
    {
        public Nullable<int> ApprovalStatusID { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
    }
}
