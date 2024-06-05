using System;

namespace DataLayer.Models
{
    public class ModelBase
    {
        public ModelBase()
        {
        }

        public ModelBase(bool InActive)
        {
            this.InActive = InActive;
        }

        public int CreatedUserID { get; set; }
        public int CurUserID { get; set; }
        public int? ModifiedUserID { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public bool? InActive { get; set; }
    }
}