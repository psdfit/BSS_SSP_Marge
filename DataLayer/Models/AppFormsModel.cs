using System;

namespace DataLayer.Models
{
    [Serializable]
    public class AppFormsModel : ModelBase
    {
        public AppFormsModel() : base()
        {
        }

        public AppFormsModel(bool InActive) : base(InActive)
        {
        }

        public int FormID { get; set; }
        public string FormName { get; set; }
        public string Path { get; set; }
        public string Icon { get; set; }
        public string Controller { get; set; }
        public int? ModuleID { get; set; }
        public string ModuleTitle { get; set; }
        public int? Sortorder { get; set; }
        public string RoleTitle { get; set; }
        public bool? CanAdd { get; set; }
        public bool? CanEdit { get; set; }
        public bool? CanDelete { get; set; }
        public bool? CanView { get; set; }

        public bool? IsAddible { get; set; }
        public bool? IsEditable { get; set; }
        public bool? IsDeletable { get; set; }
        public bool? IsViewable { get; set; }
    }}