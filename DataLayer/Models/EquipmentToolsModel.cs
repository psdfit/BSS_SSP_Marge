
using System;

namespace DataLayer.Models
{
    [Serializable]

    public class EquipmentToolsModel : ModelBase
    {
        public EquipmentToolsModel() : base() { }
        public EquipmentToolsModel(bool InActive) : base(InActive) { }

        public int EquipmentToolID { get; set; }
        public string EquipmentName { get; set; }
        public int EquipmentQuantity { get; set; }

        //public int CertAuthID { get; set; }
        //public string CertAuthName { get; set; }


    }}
