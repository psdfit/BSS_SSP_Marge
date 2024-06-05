using System;

namespace DataLayer.Models
{
    [Serializable]
    public class OrganizationModel : ModelBase
    {
        public OrganizationModel() : base()
        {
        }

        public OrganizationModel(bool InActive) : base(InActive)
        {
        }

        public int OID { get; set; }
        public string OName { get; set; }
    }}