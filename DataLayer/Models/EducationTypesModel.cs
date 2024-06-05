using System;

namespace DataLayer.Models
{
    [Serializable]
    public class EducationTypesModel : ModelBase
    {
        public EducationTypesModel() : base()
        {
        }

        public EducationTypesModel(bool InActive) : base(InActive)
        {
        }

        public int EducationTypeID { get; set; }
        public string Education { get; set; }
    }}