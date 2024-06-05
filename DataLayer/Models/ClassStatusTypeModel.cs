using System;

namespace DataLayer.Models
{
    [Serializable]
    public class ClassStatusTypeModel : ModelBase
    {
        public ClassStatusTypeModel() : base()
        {
        }

        public ClassStatusTypeModel(bool InActive) : base(InActive)
        {
        }

        public int StatusTypeID { get; set; }
        public string StatusTypeName { get; set; }
    }}