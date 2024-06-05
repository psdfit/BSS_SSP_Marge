using System;

namespace DataLayer.Models
{
    [Serializable]
    public class SectorModel : ModelBase
    {
        public SectorModel() : base()
        {
        }

        public SectorModel(bool InActive) : base(InActive)
        {
        }

        public int SectorID { get; set; }
        public string SectorName { get; set; }
    }}