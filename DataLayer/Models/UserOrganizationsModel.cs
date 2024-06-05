using System;

namespace DataLayer.Models
{
    [Serializable]
    public class UserOrganizationsModel : ModelBase
    {
        public UserOrganizationsModel() : base()
        {
        }

        public UserOrganizationsModel(bool InActive) : base(InActive)
        {
        }

        public int Srno { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }
        public int OID { get; set; }
        public string OName { get; set; }
    }
}