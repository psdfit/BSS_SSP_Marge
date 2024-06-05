using System;

namespace DataLayer.Models
{
    [Serializable]
    public class TraineeStatusTypesModel : ModelBase
    {
        public TraineeStatusTypesModel() : base()
        {
        }

        public TraineeStatusTypesModel(bool InActive) : base(InActive)
        {
        }

        public int TraineeStatusTypeID { get; set; }
        public string StatusName { get; set; }

        public int TraineeStatusReasonID { get; set; }
        public string TraineeReason { get; set; }
    }}