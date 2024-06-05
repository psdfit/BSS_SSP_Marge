using System;

namespace DataLayer.Models
{
    [Serializable]
    public class TraineeResultStatusTypesModel : ModelBase
    {
        public TraineeResultStatusTypesModel() : base()
        {
        }

        public TraineeResultStatusTypesModel(bool InActive) : base(InActive)
        {
        }

        public int ResultStatusID { get; set; }
        public string ResultStatusName { get; set; }
    }}