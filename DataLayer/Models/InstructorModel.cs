
using System;

namespace DataLayer.Models
{
	[Serializable]
	public class InstructorModel : ModelBase
	{
		public InstructorModel() : base() { }
		public InstructorModel(bool InActive) : base(InActive) { }
		public int InstrID { get; set; }
		public string InstructorName { get; set; }
		public string CNICofInstructor { get; set; }
		//public int? InstrClassID { get; set; }
		public string? ClassCode { get; set; }
		public string QualificationHighest { get; set; }
		public string TotalExperience { get; set; }
		public string NameOfOrganization { get; set; }
		public int GenderID { get; set; }
		public string GenderName { get; set; }
		//public int? District_OLD { get; set; }
		//public int? Tehsil_OLD { get; set; }
		public string PicturePath { get; set; }
		public int TSPID { get; set; }
		public int TradeID { get; set; }
		public string TradeName { get; set; }
		public string LocationAddress { get; set; }
		public int ClassID { get; set; }   // does not exist in table, just sending this parameter to AU_Instructor 
											// which also inserts in ClassInstructorMap table

		public int? InstrMasterID { get; set; }
		public int SchemeID { get; set; }
		public string BiometricData1 { get; set; }
		public string BiometricData2 { get; set; }
		public string BiometricData3 { get; set; }
		public string BiometricData4 { get; set; }
		public decimal Latitude { get; set; }
		public decimal Longitude { get; set; }
		public DateTime? TimeStampOfVerification { get; set; }
		public bool IsVerifiedByDVV { get; set; }
		public string SchemeName { get; set; }
		public string TSPName { get; set; }
		public int InstrCrID { get; set; }
		public bool CrIsApproved { get; set; }
		public bool CrIsRejected { get; set; }
		public string InstructorCRComments { get; set; }

	}


	public class CheckInstructorCNICModel 
	{
		//public int InstrID { get; set; }
		public string CNICofInstructor { get; set; }

	}
	
	public class InstructorInceptionReportCRModel 
	{
        public int InstrID { get; set; }             
        public string InstructorName { get; set; }
		public int GenderID { get; set; }            
		public string GenderName { get; set; }     
        public int TradeID { get; set; }    
        public string TradeName { get; set; }

    }
	
	
	public class ClassCodeByInstrID 
	{
        public string ClassCode { get; set; }

    }}
