using System;

namespace DataLayer.Models
{
    [Serializable]
    public class ProvincesModel : ModelBase
    {
        public ProvincesModel() : base()
        {
        }

        public ProvincesModel(bool InActive) : base(InActive)
        {
        }

        public int Id { get; set; }
        public string ProvinceName { get; set; }
        public int ProvinceID { get; set; }
    }}