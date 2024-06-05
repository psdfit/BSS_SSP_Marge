using System;

namespace DataLayer.Models
{
    [Serializable]
    public class StipendStatusModel : ModelBase
    {
        public StipendStatusModel() : base()
        {
        }

        public StipendStatusModel(bool InActive) : base(InActive)
        {
        }

        public int StipendStatusID { get; set; }
        public string StipendStatusName { get; set; }
    }}