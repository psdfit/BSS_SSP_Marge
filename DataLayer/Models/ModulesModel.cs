using System;

namespace DataLayer.Models
{
    [Serializable]
    public class ModulesModel : ModelBase
    {
        public ModulesModel() : base()
        {
        }

        public ModulesModel(bool InActive) : base(InActive)
        {
        }

        public int ModuleID { get; set; }
        public string ModuleTitle { get; set; }
        public string modpath { get; set; }
        public int SortOrder { get; set; }
    }}