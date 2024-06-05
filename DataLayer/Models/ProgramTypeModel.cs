using System;

namespace DataLayer.Models
{
    [Serializable]
    public class ProgramTypeModel : ModelBase
    {
        public ProgramTypeModel() : base()
        {
        }

        public ProgramTypeModel(bool InActive) : base(InActive)
        {
        }

        public int PTypeID { get; set; }
        public string PTypeName { get; set; }
    }}