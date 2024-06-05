using System;

namespace DataLayer.Models
{
    [Serializable]
    public class RolesRightsModel : ModelBase
    {
        public RolesRightsModel() : base()
        {
        }

        public RolesRightsModel(bool InActive) : base(InActive)
        {
        }

        public int RoleRightID { get; set; }
        public int FormID { get; set; }
        public string FormName { get; set; }
        public int RoleID { get; set; }
        public string RoleTitle { get; set; }
        public string ModuleTitle { get; set; }
        public bool CanAdd { get; set; }
        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }
        public bool CanView { get; set; }

        public bool? IsAddible { get; set; }
        public bool? IsEditable { get; set; }
        public bool? IsDeletable { get; set; }
        public bool? IsViewable { get; set; }
    }}