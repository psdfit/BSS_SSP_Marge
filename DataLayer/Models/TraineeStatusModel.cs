using System;

namespace DataLayer.Models
{
    [Serializable]
    public class TraineeStatusModel : ModelBase
    {
        public TraineeStatusModel() : base()
        {
        }

        public TraineeStatusModel(bool InActive) : base(InActive)
        {
        }

        public int TraineeStatusID { get; set; }
        public int TraineeProfileID { get; set; }
        public int TraineeStatusTypeID { get; set; }
        public string Comments { get; set; }
        public string StatusName { get; internal set; }
        public string UserName { get; internal set; }
    }
}