
using System;

namespace DataLayer.Models
{
    [Serializable]

    public class InstructorChangeRequestModel : ModelBase
    {
        public InstructorChangeRequestModel() : base() { }
        public InstructorChangeRequestModel(bool InActive) : base(InActive) { }

        public int InstructorChangeRequestID { get; set; }
        public int InstrID { get; set; }
        public string NameOfOrganization { get; set; }
        public string InstructorName { get; set; }
        public string CNICofInstructor { get; set; }
        public string QualificationHighest { get; set; }
        public string TotalExperience { get; set; }
        public int GenderID { get; set; }
        public int ClassID { get; set; }
        public string GenderName { get; set; }
        public bool IsApproved { get; set; }
        public bool IsRejected { get; set; }
        public string PicturePath { get; set; }
        public string InstructorCRComments { get; set; }
        public string NewInstructorCRComments { get; set; }

        public int CRNewInstructorID { get; set; }

        public int TradeID { get; set; }
        public string TradeName { get; set; }
        public string LocationAddress { get; set; }
        public string ClassCode { get; set; }
        public int SchemeID { get; set; }

        public string FileType { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public int TSPID { get; set; }
        public string TSPName { get; set; }
        public string SchemeName { get; set; }
        public string ClassCodeBYIncClassId { get; set; }
        public bool CrNewInstructorIsApproved { get; set; }
        public bool CrNewInstructorIsRejected { get; set; }



    }
    public class InstructorCRFiltersModel
    {
        public int UserID { get; set; }
        public int TradeID { get; set; }

    }}
