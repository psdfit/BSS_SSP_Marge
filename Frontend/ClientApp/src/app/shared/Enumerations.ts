export enum EnumApprovalStatus {
  Pending = 1
  , Approved = 2
  , Rejected = 3
  , SendBack = 4
}
export enum EnumApprovalProcess {
  AP_PD = 'AP_PD'
  , AP_BD = 'AP_BD'
  , PO_TSP = 'PO_TSP'
  , PO_TRN = 'PO_TRN'
  , PO_SRN = 'PO_SRN'
  , PO_GURN = 'PO_GURN'
  , PO_GRN = 'PO_GRN'
  , PRN_C = 'PRN_C'
  , PRN_R = 'PRN_R'
  , PRN_F = 'PRN_F' // this is actually PRN Employment (Final = Employment)
  , PRN_T = 'PRN_T'
  , INV_R = 'INV_R'
  , INV_1ST = 'INV_1ST'
  , INV_2ND = 'INV_2ND'
  , INV_SRN = 'INV_SRN'
  , INV_GURN = 'INV_GURN'
  , INV_TPRN = 'INV_TPRN'
  , INV_PVRN = 'INV_PVRN'
  , INV_PCRN = 'INV_PCRN'
  , INV_OTRN = 'INV_OTRN'
  , INV_MRN = 'INV_MRN'
  , INV_F = 'INV_F'
  , INV_TRN = 'INV_TRN'
  , INV_C = 'INV_C'
  , TRD = 'TRD'
  , CR_SCHEME = 'CR_SCHEME'
  , CR_TSP = 'CR_TSP'
  , CR_CLASS_LOCATION = 'CR_CLASS_LOCATION'
  , CR_CLASS_DATES = 'CR_CLASS_DATES'
  , CR_TRAINEE_VERIFIED = 'CR_TRAINEE_VERIFIED'
  , CR_TRAINEE_UNVERIFIED = 'CR_TRAINEE_UNVERIFIED'
  , CR_INSTRUCTOR = 'CR_INSTRUCTOR'
  , CR_NEW_INSTRUCTOR = 'CR_NEW_INSTRUCTOR'
  , CR_INCEPTION = 'CR_INCEPTION'
  , CR_INSTRUCTOR_REPLACE = 'CR_INSTRUCTOR_REPLACE'
  , SRN = 'SRN'
  , GURN = 'GURN'
  , TPRN = 'TPRN'
  , PVRN = 'PVRN'
  , PCRN = 'PCRN'
  , OTRN = 'OTRN'
  , MRN = 'MRN'
  , PO_TPRN = 'PO_TPRN'
  , PO_PVRN = 'PO_PVRN'
  , PO_PCRN = 'PO_PCRN'
  , PO_OTRN = 'PO_OTRN'
  , PO_MRN = 'PO_MRN'
  , VRN = 'VRN'
  , INV_VRN = 'INV_VRN'
  , PO_VRN = 'PO_VRN'
  , CANCELATION = 'Cancel'
  , REG_EVAL = 'REG_EVAL'
  , PROG_APP = 'PROG_APP'
  , CRTEM_APP = 'CRTEM_APP'
  , IPPC = 'IPPC'
  , IPMC = 'IPMC'
  , IPOT = 'IPOT'
  , IPVS = 'IPVS'

}
export enum EnumCertificationAuthority {
  PBTE = 1
  , NAVTEC = 2
  , TEVTA = 3
  , CityAndGuilds = 4
}
export enum EnumTraineeStatusType {
  EnRoll = 1
  , OnRoll = 2
  , DropOut = 3
  , Expelled = 4
  , ResultAwaited = 5
  , Completed = 6
  , TrainingCancelled = 7
  , Absent = 8
  , Marginal = 8

}
export enum EnumClassStatus {
  Planned = 1
  , Active = 3
  , Completed = 4
  , Abandoned = 5
  , Cancelled = 6
  , Ready = 7
  , OnHold = 8
}
export enum EnumTraineeResultStatusTypes {
  Pass = 1
  , Fail = 2
  , None = 3
  , Absent = 4
  , TestnotApplicable = 5
  , AttendedbutnotTested = 6
}
export enum EnumUserLevel {
  AdminGroup = 1
  , OrganizationGroup = 2
  , TPM = 3
  , TSP = 4
}
export enum EnumUserRoles {
  Administrator = 1
  , FinanceAccount = 2
  , Procurement = 3
  , MonitoringEvaluation = 4
  , InformationTechnology = 5
  , ProgramDevelopment = 6
  , BusinessDevelopmentPartnerships = 7
  , TSP = 12
  , super = 13
  , TPM = 14
  , MarketingCommunication = 15
  , DataEntryOperator = 16
  , ExecutiveManagement = 17
  , HumanResources = 18
  , PBTE = 19
  , DEO = 20
  , KAM = 22
  , SSPRegistration = 23
}
export enum ExportType {
  CSV = 'csv',
  XLS = 'xls',
  XLSX = 'xlsx',
  TXT = 'txt',
  JSON = 'json',
  PDF = 'pdf',
  OTHER = 'other'
}
export enum EnumExcelReportType {
  TSR = 1,
  MasterSheet = 2,
  MPR = 3,
  NTP = 4,
  VisitPlan = 5,
  PO = 6,
  PRN = 7,
  SRN = 8,
  Invoice = 9,
  RTP = 10,
  TMP_RTP = 11,
  TraineeUpdation = 12,
  InceptionReport = 13,
  UnVerifiedTraineesChangeRequestApproval = 14,
  PRN_C = 15,
  PRN_R = 16,
  PRN_F = 17,
  PRN_T = 18,
  ROSI = 19,
  ReportedEmployment = 20,
  VerifiedEmployment = 21,
  PendingClassesinKAMDashboard = 22,
  StipendDisbursementStatusReport = 23,
  orgconfigration = 24,
  TPRN = 25,
  PO_TPRN = 26,
  GURN = 27,
  PVRN=28,
  MRN=29,
  PCRN=30,
  OTRN=31,
  TAR=32

}


export enum EnumTSPColorType {
  White = 1,
  Yellow = 2,
  Red = 3,
  Black = 4

}
export enum EnumProgramCategory {
  ProgramDevelopmentFTI = 2,// 5
  ProgramDevelopmentIndustry = 8,// 5
  ProgramDevelopmentCommunity = 11,// 5
  CostSharing = 6,
  BusinessDevelopmentAndPartnershipsFTI = 1,// 7
  BusinessDevelopmentAndPartnershipsIndustry = 9,// 7
  BusinessDevelopmentAndPartnershipsCommunity = 10,// 7
}
export enum EnumProgramType {
  FTI = 1,
  Community = 2,
  Industry = 3,
  CostSharing = 6,
}
export enum EnumGender {
  Male = 3,
  Female = 5,
  Transgender = 6,
  Both = 7,
  _3way = 8

}

export enum EnumAppForms {
  AppendixWithFinancialInformation = 1147,
  InceptionReport = 1119
}

export enum EnumAppendixModules {
  Scheme = 'Scheme',
  TSP = 'TSP',
  Class = 'Class',
  Instructor = 'Instructor'
}
export enum AppendixImportSheetNames {
  Scheme = 'Scheme',
  TSP = 'TSP',
  Class = 'Class',
  Instructor = 'Instructor'
}

export enum PBTESheetNames {
  ExaminationData = 'ExaminationData',
  TraineeData = 'TraineeData'
}

export enum EnumTPMReports {
  CenterInspection = 'Center Inspection',
  ClassFormIII = 'Class (Form III)',
  TSPSummaryReportFormIV = 'TSP Summary Report (Form IV)',
  SchemeViolationReport = 'Scheme Violation Report',
  ConfirmedMarginal = 'Confirmed Marginal',
  AdditionalTrainees = 'Additional Trainees',
  DeletedOrDropoutTrainees = 'Deleted/Dropout Trainees',
  AttendanceAndPerception = 'Attendance and Perception',
  InstructorDetails = 'Instructor Details',
  ProfileVerificationPV = 'Profile Verification (PV)',
  ProfileVerificationSummary = 'Profile Verification Summary',
  OnJobTraining = 'On Job Training',
  EmploymentVerification = 'Employment Verification'
}
export enum EnumTraineeUnVerifiedReason {
  Age = 'Under/Over age',
  District = 'Out of district',
  CNIC = 'CNIC not of trainee'
}
export enum EmploymentStatuses {
  Employed = 'Employed',
  Unemployed = 'Unemployed',
  NotSubmitted = 'Not Submitted',
  NotInterested = 'Not Interested'
}

export enum EnumBusinessRuleTypes {
  Community = 'Community',
  CostSharing = 'Cost Sharing'
}
export enum EnumAmsReports {
  AttendancePerception = 'Attendance & Perception',
  ConfirmedMarginal = 'Confirmed Marginal',
  DeletedDropout = 'Deleted Dropout',
  ViolationSummary = 'Violation Summary',
  ReportExecutiveSummary = 'Report Executive Summary',
  AdditionalTrainees = 'Additional Trainees',
  FakeGhostTrainee = 'Fake Ghost Trainee',
  CovidMaskViolation = 'Covid Mask Violation',
  EmploymentVerification = 'Employment Verification',
  FormIII = 'Form III',
  CenterInspection = 'Center Inspection',
  FormIV = 'Form IV',
  // UnverifiedTrainee = 'Unverified Trainee',
  ProfileVerification = 'Profile Verification'
}

export enum EnumReports {
  'TSP Master Data' = 1,
  'Trade Data' = 2,
  'Procurement - Trainee Status Report' = 3,
  'TSP Curriculum Report' = 4,
  'Procurement - Placement Report' = 5,
  'Sector Wise Trainee Report' = 6,
  'Financial Comparison of Schemes' = 7,
  'Total Locations Report' = 8,
  'List of new Trades, TSPs and Industry in a Scheme' = 9,
  'Change of Instructor' = 10,
  'Fact Sheet' = 11,
  'Change in Appendix Report' = 12,
  'Invoice Status Report' = 13,
  'Stipend Disbursement Report' = 14,
  'Trainee Complaints Report' = 15,
  'TPM Summaries - Violations' = 16,
  'TPM Summaries - Trainees' = 17,
  'Management Report' = 18,
  'SDP Monthly Report' = 19,
  'BSS - Confirmed Marginal' = 20,
  'Unverified Trainees Report' = 21,
  'Weighted Average Cost per Trainee per Month' = 22,
  'Number of Unique TSPs engaged with PSDF' = 23,
  'PD - Trainee Status Report' = 24,
  'Income of Graduate' = 25,
  'Trainee Type Percentage' = 26,
  'Top 10 Trades' = 27,
  'PD - Placement Report' = 28,
  'BDP - Trainee Status Report' = 29,
  'Players under Training for placement' = 30,
  'Cost Saving Report' = 31,
  'Number of Cost Sharing Partners' = 32,
  'Business Partner Invoice Status Report' = 33,
  'Tsp Invoice Status Report' = 34,
  'Trainees Attendance Report' = 35,
  'Trainer change Logs' = 36,
  'Bulk Trainees Status Report' = 37,
  'TSP Details Report' = 38,
  'AMS Missing Classes Data Report' = 39,
  // 'TSP Change Request Report' = ,  -- id removed - no record in DB
  'Class-wise Payment Report' = 41,

}
export enum EnumSubReports {
  'Master Data of TSPs – Trainee Wise' = 'TraineeWise'
  , 'Master Data of TSPs – Cost Wise' = 'CostWise'
  , 'Top 20 TSPs – Trainee' = 'Top20TraineeWise'
  , 'Top 20 TSPs – Cost' = 'Top20CostWise'
  , 'Total Trades – Trainee and Cost' = 'TraineeAndCost'
  , 'Top 10 Trades – Cost' = 'Top10CostWise'
  , 'Top 10 Trades – Trainee' = 'Top10TraineeWise'
  , 'Trainees Contracted - Sector' = 'Sector'
  , 'Trainees Contracted - Clustor' = 'Cluster'
  , 'Contracted/Completed' = 'Completed'
  , 'Geographic placement – cluster & district' = 'Cluster'
  , 'Trade wise placement' = 'Trade'
  , 'Sector Wise placement' = 'Sector'
  , 'Financial Comparisons of Schemes (Trade wise)' = 'Trade'
  , 'Financial Comparisons of Schemes (Sector wise)' = 'Sector'
  , 'Financial Comparisons of Schemes (Cluster wise)' = 'Cluster'
  , 'Top 10 Trades - Scheme' = 'Scheme'
  , 'Top 10 Trades - Sector' = 'Sector'
  , 'PD - Placement Report - Scheme' = 'Scheme'
  , 'PD - Placement Report - Trade' = 'Trade'
  , 'PD - Placement Report - Sector' = 'Scheme'
  , 'Trainee Complaints Report - A' = 'A'
  , 'Trainee Complaints Report - B' = 'B'
  , 'Management Report (A)' = 'A'
  , 'Management Report (B)' = 'B'
}
export enum EnumReportsFilters {
  'Quarter' = 'Quarter'
  , 'Examination Body' = 'ExaminationBody'
  , 'Sector' = 'Sector'
  , 'Duration' = 'Duration'
  , 'Trade' = 'Trade'
  , 'Scheme' = 'Scheme'
  , 'Gender' = 'Gender'
  , 'Scheme Type' = 'ProgramType'
  , 'Program Type' = 'ProgramType'
  , 'Month' = 'Calendar'
  , 'Cluster' = 'Cluster'
  , 'Year' = 'Calendar'
  , 'Curriculum' = 'Curriculum'
  , 'District' = 'District'
  , 'Entry Qualification' = 'EntryQualification'
  , 'Financial Year' = 'FinancialYear'
  , 'Tsp' = 'Tsp'
  , 'Kam' = 'User'
  , 'Start Date' = 'StartDate'
  , 'End Date' = 'EndDate'
  , 'Class' = 'Class'
  , 'Funding' = 'Funding'
  , 'Employment Status' = 'EmploymentStatus'
  , 'Program Category' = 'ProgramCategory'
  , 'FundingCategory' = 'FundingCategory'
}
