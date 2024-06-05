using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
    public class KAMAssignedTSPsModel
    {
		public KAMAssignedTSPsModel() : base() { }
		//public KAMAssignedTSPsModel(bool InActive) : base(InActive) { }

		public int TSPID { get; set; }
		public string TSPName { get; set; }
		public Boolean IsSelected { get; set; }
		public int SchemeID { get; set; }
		public string SchemeName { get; set; }

	}
}
