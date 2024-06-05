/* **** Aamer Rehman Malik *****/

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataLayer.Models
{
    public class PSPEmploymentModel : ModelBase
    {
        public int? PlacementID { get; set; }
        public int TraineeID { get; set; }
        public string TraineeName { get; set; }
        public int? ClassID { get; set; }
        public int? PSPBatchID { get; set; }
        public string ClassCode { get; set; }

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
    }

    public class PSPEmploymentExcelModel
    {
        public int? PlacementID { get; set; }
        public int? TraineeID { get; set; }
        public string TraineeName { get; set; }
        public string ClassName { get; set; }
        public int? ClassID { get; set; }

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
        public string DistrictName { get; set; }
        public string EmploymentTehsilName { get; set; }
        public string EmploymentTiming { get; set; }
        public bool? IsTSP { get; set; }
        public string OldID { get; set; }
        public string EmploymentTehsilOld { get; set; }
        public string OfficeContactNo { get; set; }
        public bool? IsMigrated { get; set; }
        public bool? VerifyStatus { get; set; }
        public int? VerificationMethodId { get; set; }
        public string FilePath { get; set; }
        public int? PlacementTypeID { get; set; }
    }
}