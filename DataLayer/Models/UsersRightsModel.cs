using System;

namespace DataLayer.Models
{
    [Serializable]
    public class UsersRightsModel : ModelBase
    {
        public UsersRightsModel() : base()
        {
        }

        public UsersRightsModel(bool InActive) : base(InActive)
        {
        }

        public int UserRightID { get; set; }
        public int FormID { get; set; }
        public string FormName { get; set; }
        public int UserID { get; set; }
        public int? RoleID { get; set; }
        public string UserName { get; set; }
        public string? CanCheck { get; set; }
        public bool CanAdd { get; set; }
        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }
        public bool CanView { get; set; }        public string ModuleTitle { get; set; }
        public int ModSortOrder { get; set; }
        public string Path { get; set; }
        public string Controller { get; set; }
        public int SortOrder { get; set; }
        public string modpath { get; set; }
        public bool? IsAddible { get; set; }
        public bool? IsEditable { get; set; }
        public bool? IsDeletable { get; set; }
        public bool? IsViewable { get; set; }
    }}