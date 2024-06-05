/* **** Aamer Rehman Malik *****/

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataLayer.Models
{
    public class TSPEmploymentModel : ModelBase
    {
        public int? PlacementID { get; set; }
        public int? TraineeID { get; set; }
        public string TraineeName { get; set; }
        public int? ClassID { get; set; }
        public int? TSPID { get; set; }
        public int? PhysicalVerificationStatus { get; set; }
        public string ClassCode { get; set; }
        public DateTime? EndDate { get; set; }

        public string TradeName { get; set; }
        public string Designation { get; set; }
        public string Department { get; set; }
        public int? EmploymentDuration { get; set; }
        public double? Salary { get; set; }
        public string SupervisorName { get; set; }
        public string SupervisorContact { get; set; }
        public DateTime? EmploymentStartDate { get; set; }
        public string? EmploymentStatus { get; set; }
        public int? EmploymentType { get; set; }
        public string EmployerName { get; set; }
        public string EmployerBusinessType { get; set; }
        public string EmploymentAddress { get; set; }
        public int? District { get; set; }
        public int? EmploymentTehsil { get; set; }
        public string EmploymentTiming { get; set; }
        public bool? IsTSP { get; set; }
        public string OldID { get; set; }
        public string EmploymentTehsilOld { get; set; }
        public string OfficeContactNo { get; set; }
        public bool? IsMigrated { get; set; }
        public bool? VerifyStatus { get; set; }

        public int? PlacementTypeID { get; set; }
        public string EOBI { get; set; }

        public int? VerificationMethodId { get; set; }
        public string FileType { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }

        public bool? IsVerified { get; set; }
        public string Comments { get; set; }
        public string VerificationMethodType { get; set; }
        public string TraineeCode { get; set; }
        public string ContactNumber { get; set; }
        public int CallCenterVerificationTraineeID { get; set; }
        public int CallCenterVerificationSupervisorID { get; set; }
        public int CallCenterVerificationCommentsID { get; set; }
        public int FundingCategoryID { get; set; }

        public string CallCenterVerificationTraineeName { get; set; }
        public string CallCenterVerificationSupervisorName { get; set; }
        public string CallCenterVerificationTraineeComment { get; set; }

        public string PlatformName { get; set; }
        public string NameofTraineeStorePage { get; set; }
        public string LinkofTraineeStorePage { get; set; }

        public string EmployerNTN { get; set; }

    }

    public class TSPEmploymentExcelModel
    {
        public string DEOVerification { get; set; }
        public string TelephonicVerification { get; set; }
        public string FinalVerification { get; set; }
        public int? PlacementID { get; set; }
        public int? TraineeID { get; set; }
        public string TraineeName { get; set; }
        public string GenderName { get; set; }
        public string DistrictName { get; set; }
        public string TehsilName { get; set; }
        public string EmploymentTiming { get; set; }
        public string OfficeContactNo { get; set; }
        public string EmploymentCommitment { get; set; }
        public string SchemeName { get; set; }
        public string TSPName { get; set; }
        public string ClassCode { get; set; }
        public int? ClassID { get; set; }
        public int? TSPID { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string TradeName { get; set; }
        public string Designation { get; set; }
        public string Department { get; set; }
        public int? EmploymentDuration { get; set; }
        public double? Salary { get; set; }
        public string SupervisorName { get; set; }
        public string SupervisorContact { get; set; }
        public DateTime? EmploymentStartDate { get; set; }
        public string? EmploymentStatus { get; set; }
        public int? EmploymentType { get; set; }
        public string EmploymentStatusName { get; set; }
        public string EmploymentTypeName { get; set; }
        public string EmployerName { get; set; }
        public string EmployerBusinessType { get; set; }
        public string EmploymentAddress { get; set; }
        public int? District { get; set; }
        public int? EmploymentTehsil { get; set; }
        public string EmploymentTehsilName { get; set; }
        public bool? IsTSP { get; set; }
        public string OldID { get; set; }
        public string EmploymentTehsilOld { get; set; }
        public bool? IsMigrated { get; set; }
        public bool? VerifyStatus { get; set; }
        public int? VerificationMethodId { get; set; }
        public string FilePath { get; set; }
        public int? PlacementTypeID { get; set; }
        public string EOBI { get; set; }
        public string PlacementType { get; set; }
        public string VerificationMethodType { get; set; }
        public string TraineeCode { get; set; }
        public string TraineeCNIC { get; set; }
        public string ContactNumber { get; set; }
        public string VerificationStatus { get; set; }
        public string Comments { get; set; }
        public string CCTraineeResponse { get; set; }
        public string CCSupervisorResponse { get; set; }
        public DateTime? EmploymentSubmitedDate { get; set; }
        public string PlatformName { get; set; }
        public string NameofTraineeStorePage { get; set; }
        public string LinkofTraineeStorePage { get; set; }
        public string EmployerNTN { get; set; }
    }
    public class TSPReportedEmploymentExcelModel
    {
        public int? PlacementID { get; set; }
        public int? TraineeID { get; set; }
        public string TraineeName { get; set; }
        public string GenderName { get; set; }
        public string DistrictName { get; set; }
        public string TehsilName { get; set; }
        public string EmploymentTiming { get; set; }
        public string OfficeContactNo { get; set; }
        public string EmploymentCommitment { get; set; }
        public string SchemeName { get; set; }
        public string TSPName { get; set; }
        public string ClassCode { get; set; }
        public int? ClassID { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string TradeName { get; set; }
        public string Designation { get; set; }
        public string Department { get; set; }
        public int? EmploymentDuration { get; set; }
        public double? Salary { get; set; }
        public string SupervisorName { get; set; }
        public string SupervisorContact { get; set; }
        public DateTime? EmploymentStartDate { get; set; }
        public string? EmploymentStatus { get; set; }
        public int? EmploymentType { get; set; }
        public string EmploymentStatusName { get; set; }
        public string EmploymentTypeName { get; set; }
        public string EmployerName { get; set; }
        public string EmployerBusinessType { get; set; }
        public string EmploymentAddress { get; set; }
        public int? District { get; set; }
        public int? EmploymentTehsil { get; set; }
        public string EmploymentTehsilName { get; set; }
        public bool? IsTSP { get; set; }
        public string OldID { get; set; }
        public string EmploymentTehsilOld { get; set; }
        public bool? IsMigrated { get; set; }
        public bool? VerifyStatus { get; set; }
        public int? VerificationMethodId { get; set; }
        public string FilePath { get; set; }
        public int? PlacementTypeID { get; set; }
        public string EOBI { get; set; }
        public string PlacementType { get; set; }
        public string VerificationMethodType { get; set; }
        public string TraineeCode { get; set; }
        public string TraineeCNIC { get; set; }
        public string ContactNumber { get; set; }
        public string VerificationStatus { get; set; }
        public string Comments { get; set; }
        public string PlatformName { get; set; }
        public string NameofTraineeStorePage { get; set; }
        public string LinkofTraineeStorePage { get; set; }
        public string EmployerNTN { get; set; }

    }
    public class ForwardToTelephonicVerification
    {
        //int[] list,int? PlacementTypeID ,int? VerificationMethodID
        public int? PlacementTypeID { get; set; }
        public int? VerificationMethodID { get; set; }
        public int[] ClassIDslist { get; set; }
    }
}