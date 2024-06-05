using System;

namespace DataLayer.Models
{
    [Serializable]
    public class SubSectorModel : ModelBase
    {
        public SubSectorModel() : base()
        {
        }

        public SubSectorModel(bool InActive) : base(InActive)
        {
        }

        public int SubSectorID { get; set; }
        public string SubSectorName { get; set; }
        public int SectorID { get; set; }
        public string SectorName { get; set; }
    }}