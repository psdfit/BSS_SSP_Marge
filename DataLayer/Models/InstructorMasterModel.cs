using System;

namespace DataLayer.Models
{
	[Serializable]

	public class InstructorMasterModel : ModelBase
	{
		public InstructorMasterModel() : base() { }
		public InstructorMasterModel(bool InActive) : base(InActive) { }

		public int InstrMasterID { get; set; }
		public string InstructorName { get; set; }
		public string CNICofInstructor { get; set; }
		public string QualificationHighest { get; set; }
		public string TotalExperience { get; set; }
		public int GenderID { get; set; }
		public string GenderName { get; set; }
		public int TradeID { get; set; }
		public string TradeName { get; set; }
		public string LocationAddress { get; set; }
		public string NameOfOrganization { get; set; }
		public string PicturePath { get; set; }

	}
}

