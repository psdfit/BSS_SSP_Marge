using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
   public class AppFormEndPointModel:ModelBase
    {
        public AppFormEndPointModel() : base()
        {
        }

        public AppFormEndPointModel(bool InActive) : base(InActive)
        {
        }
        public string Controller { get; set; }
        public string Action { get; set; }
        public int? FormID { get; set; }
    }
}