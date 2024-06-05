using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataLayer.Models
{
    public class AMSFormThreeViewModel
    {
        public AMSTSPDetailsModel TSPDetails { get; set; }
        public int? VisitCount { get; set; }

        public AMSVisitModel Visit1 { get; set; }
        public AMSVisitModel Visit2 { get; set; }
        public AMSVisitModel Visit3 { get; set; }
        public AMSVisitModel Visit4 { get; set; }
        public List<AMSVisitListModel> VisitDetails { get; set; }
        public List<AMSFakeGhostTraineesModel> FakeGhostTrainees { get; set; }
        public List<AMSMarginalTraineesModel> MarginalTrainees { get; set; }
        public AMSClassViolationModel ClassViolations { get; set; }

        public AMSCMTraineesCountModel CMVisit1 { get; set; }
        public AMSCMTraineesCountModel CMVisit2 { get; set; }
        public AMSCMTraineesCountModel CMVisit3 { get; set; }
        public AMSCMTraineesCountModel CMVisit4 { get; set; }

        public List<AMSTraineesPreceptionModel> TraineesPreceptions { get; set; }
        public List<AMSViolationTypeModel> ViolationTypeList { get; set; }
        public List<AMSFakeGhostTraineesModel> DeleteDropOutTrainees { get; set; }

        public List<AMSVisitModel> VisitsList { get; set; }

    }
    public class AMSTSPDetailsModel
    {
        public string TSPName { get; set; }
        public int? VisitInMonths { get; set; }
        public string CentreName { get; set; }
        public string SchemeName { get; set; }
        public string ClassCode { get; set; }
        public string TradeName { get; set; }
        public string InstructorsInfo { get; set; }
        public string ReportingMonth { get; set; }

    }
    public class AMSVisitModel
    {
        public string IsLock { get; set; }
        public string IsRelocated { get; set; }
        public string EquipmentAvailable { get; set; }
        public string SameInstructor { get; set; }
        public string IsLockRemarks { get; set; }
        public string IsRelocatedRemarks { get; set; }
        public string EquipmentAvailableRemarks { get; set; }
        public string SameInstructorRemarks { get; set; }

        public string VisitDate { get; set; }
        public string VisitTime { get; set; }
        public int? TraineesPresent { get; set; }

        public string FMSignImage{ get; set; }
        public string TSPSignImage { get; set; }

        public string FMSignOffRemarks { get; set; }
        public string TspSignOffRemarks { get; set; }

    }
    public class AMSVisitListModel
    {
        public int? SchemeID { get; set; }
        public string ClassID { get; set; }
        public string ClassCode { get; set; }
        public string TradeName { get; set; }
        public string SchemeName { get; set; }
        public string SchemeType { get; set; }
        public string TSPName { get; set; }
        public string InstructorsInfo { get; set; }
        public string CentreName { get; set; }
        public int? ClMrID { get; set; }
        public int? TraineesImported { get; set; }
        public DateTime? VisitDateTime { get; set; }
        public int? VisitNo { get; set; }
        public string IsLock { get; set; }
        public string IsLockRemarks { get; set; }
        public string IsRelocated { get; set; }
        public string IsRelocatedRemarks { get; set; }
        public string IsEquipmentAvailable { get; set; }
        public string IsEquipmentAvailableRemarks { get; set; }
        public string InstructorChanged { get; set; }
        public string IsAllocatedTrainerRemarks { get; set; }
        public string AttendRegisterAval { get; set; }
        public int? TraineeCount { get; set; }
        public int? TraineeAttendCountOne { get; set; }
        public int? TraineeAttendCountTwo { get; set; }
        public int? TraineeAttendCountThree { get; set; }
        public string SignOffTspRemarks { get; set; }
        public string SignOffFmRemarks { get; set; }
        public string TraineeProb1 { get; set; }
        public string TraineeProb2 { get; set; }
        public string FMRemarks { get; set; }
        public string FMSignatureImgPath { get; set; }
        public string TspSignatureImgPath { get; set; }
        public string UID { get; set; }
    }
    public class AMSFakeGhostTraineesModel
    {
        public int? SrNo { get; set; }
        public int? ClMrTraineeID { get; set; }
        public int? TraineeID { get; set; }
        public string IsPresent { get; set; }
        public string AttendanceStatus { get; set; }
        public string VerificationStatus { get; set; }
        public string IsMarginal { get; set; }
        public string IsGhost { get; set; }
        public string DisplayName { get; set; }
        public string FatherName { get; set; }
        public string TraineeCNIC { get; set; }
        public string Remarks { get; set; }
    }
    public class AMSMarginalTraineesModel
    {
        public int? SrNo { get; set; }
        public int? ClMrTraineeID { get; set; }
        public int? ClassInspectionRequestID { get; set; }
        public DateTime? VisitMonth { get; set; }
        public string TraineeName { get; set; }
        public string FatherName { get; set; }
        public string Cnic { get; set; }
        public string Remarks { get; set; }
    }
    public class AMSClassViolationModel
    {
        public int? major_count { get; set; }
        public int? minor_count { get; set; }
        public int? serious_count { get; set; }
        public int? observation_count { get; set; }
    }
    public class AMSCMTraineesCountModel
    {
        public DateTime? VisitDateTime { get; set; }
        public int? VisitNo { get; set; }
        public int? TraineesPresentCount { get; set; }
        public int? TraineesCountOne { get; set; }
        public int? TraineesCountTwo { get; set; }
        public int? TraineesCountThree { get; set; }
        public string VDate { get; set; }
        public string VTime { get; set; }
    }
    public class AMSTraineesPreceptionModel 
    {
        public int? SrNo { get; set; }
        public int? ID { get; set; }
        public int? ClassMonitoringID { get; set; }
        public int? VisitNo { get; set; }
        public int? ViolationID { get; set; }
        public string IsViolation { get; set; }
        public int? PercentageSatisfied { get; set; }
        public string VioDesc { get; set; }
        public string VioName { get; set; }
        public string Type { get; set; }
        public string TypeFti { get; set; }
        public string TypeCommunity { get; set; }
        public string TypeIndustrial { get; set; }
        public string ClassID { get; set; }
        public int? VioID { get; set; }
    }
    public class AMSViolationTypeModel
    {
        public int? VioID { get; set; }
        public int? SrNo { get; set; }
        public int? ID { get; set; }
        public int? ClassMonitoringID { get; set; }
        public int? VisitNo { get; set; }
        public int? ViolationID { get; set; }
        public int? PercentageSatisfied { get; set; }
        public string VioDesc { get; set; }
        public string VioName { get; set; }
        public string Type { get; set; }
        public string TypeFti { get; set; }
        public string TypeCommunity { get; set; }
        public string TypeIndustrial { get; set; }
        public int? ClassID { get; set; }
        public string ViolationType { get; set; }
        public string IsViolation { get; set; }

    }

}

