using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
    [Serializable]
    public class FormApprovalModel : ModelBase
    {
        public int ID { get; set; }
        public string ProcessKey { get; set; }
        public int FormID { get; set; }
        public string UserName { get; set; }
        public int UserID { get; set; }
        public string Comment { get; set; }
        public bool IsApproved { get; set; }
        public bool SendBack { get; set; }
        public bool IsRejected { get; set; }
    }
}
