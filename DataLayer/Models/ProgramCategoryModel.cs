
using System;

namespace DataLayer.Models
{
    [Serializable]

    public class ProgramCategoryModel : ModelBase
    {
        public ProgramCategoryModel() : base() { }
        public ProgramCategoryModel(bool InActive) : base(InActive) { }

        public int PCategoryID { get; set; }
        public string PCategoryName { get; set; }
        public string PCategoryCode { get; set; }
        public int PTypeID { get; set; }
        public string PTypeName { get; set; }

    }}
