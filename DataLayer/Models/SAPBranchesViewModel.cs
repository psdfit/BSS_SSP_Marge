using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
    public class SAPBranchesViewModel
    {
        public string Success { get; set; }
        public string Message { get; set; }
        public string BranchesList { get; set; }

    }
    public class BranchesItems
    {
        public int BranchId { get; set; }
        public string BranchName { get; set; }
    }
}
