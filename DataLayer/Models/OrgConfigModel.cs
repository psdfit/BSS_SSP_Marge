/* **** Aamer Rehman Malik *****/
/* ****Aamer Rehman Malik *****/

using System;

namespace DataLayer.Models
{
    [Serializable]
    public class OrgConfigModel : ModelBase
    {
        public OrgConfigModel() :
            base()
        {
        }

        public OrgConfigModel(bool InActive) : base(InActive)
        {
        }

        public int ConfigID { get; set; }
        public int OID { get; set; }
        public string OName { get; set; }
        public string BusinessRuleType { get; set; }
        public int SchemeID { get; set; }
        public string SchemeName { get; set; }
        public int TSPID { get; set; }
        public int TSPMasterID { get; set; }
        public string TSPName { get; set; }
        public string ClassCode { get; set; }
        public int ClassID { get; set; }
        public bool? DualRegistration { get; set; }
        public int BracketDaysBefore { get; set; }
        public int BracketDaysAfter { get; set; }
        public int EligibleGenderID { get; set; }
        public int MinAge { get; set; }
        public int MaxAge { get; set; }
        public int MinEducation { get; set; }
        public int ReportBracketBefore { get; set; }
        public int ReportBracketAfter { get; set; }
        public string StipendPayMethod { get; set; }
        public int ClassStartFrom1 { get; set; }
        public int ClassStartTo1 { get; set; }
        public int ClassStartFrom2 { get; set; }
        public int ClassStartTo2 { get; set; }
        public int RTPFrom { get; set; }
        public int RTPTo { get; set; }
        public int BISPIndexFrom { get; set; }
        public int BISPIndexTo { get; set; }
        public int MinAttendPercent { get; set; }
        public int StipendDeductAmount { get; set; }
        public int PhyCountDeductPercent { get; set; }
        public int DeductDropOutPercent { get; set; }
        public int StipNoteGenGTMonth { get; set; }
        public int StipNoteGenLTMonth { get; set; }
        public int MPRDenerationDay { get; set; }
        public int EmploymentDeadline { get; set; }
        public int OJTDeadline { get; set; }
        public int DeductionSinceInceptionPercent { get; set; }
        public int DeductionFailedTraineesPercent { get; set; }

        public int TSROpeningDays { get; set; }
        public int TraineesPerClassThershold { get; set; }

        public bool IsCheckBISP { get; set; }
        public bool IsCheckPBTE { get; set; }
        public string BusinessRuleTypeForGetPreviousList { get; set; }
        public bool? ISDVV { get; set; }

        public string GenderName { get; set; }
        public string Education { get; set; }
        public int TSPConfigurationCounter { get; set; }
    }}