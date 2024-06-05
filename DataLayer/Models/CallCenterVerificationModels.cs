using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
  public  class CallCenterVerificationModels : ModelBase
    {
        public CallCenterVerificationModels() : base()
        {
        }

        public class CallCenterVerificationTraineeModel
        {
            public int CallCenterVerificationTraineeID { get; set; }
            public string Name { get; set; }
            public string DisplayName { get; set; }

        }
        public class CallCenterVerificationSupervisorModel
        {



            public int CallCenterVerificationSupervisorID { get; set; }
            public string Name { get; set; }
            public string DisplayName { get; set; }
        }
        public class CallCenterVerificationCommentsModel
        {

            public int CallCenterVerificationCommentsID { get; set; }
            public string Name { get; set; }
            public string DisplayName { get; set; }


        } 
        public class CallCenterVerificationHistoryModel
        {

            public int CallCenterVerificationHistoryID { get; set; }
            public int PlacementVerificationID { get; set; }
            public int CallCenterVerificationTraineeID { get; set; }
            public int CallCenterVerificationSupervisorID { get; set; }
            public int CallCenterVerificationCommentsID { get; set; }


        }


    }
}
