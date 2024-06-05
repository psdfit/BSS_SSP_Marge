using System;

namespace DataLayer.Models
{
    [Serializable]
    public class TraineeDisabilityModel : ModelBase
    {
        public TraineeDisabilityModel() : base()
        {
        }

        public TraineeDisabilityModel(bool InActive) : base(InActive)
        {
        }

        public int Id { get; set; }
        public string Disability { get; set; }
    }}