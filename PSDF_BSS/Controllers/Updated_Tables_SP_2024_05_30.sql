--
-- Create table [dbo].[SSPWorkflowTask]
--
PRINT (N'Create table [dbo].[SSPWorkflowTask]')
GO
CREATE TABLE dbo.SSPWorkflowTask (
	TaskID INT IDENTITY
   ,WorkflowID INT NULL
   ,TaskName NVARCHAR(100) NULL
   ,TaskDays INT NULL
   ,TaskApproval NVARCHAR(500) NULL
   ,TaskStatus NVARCHAR(500) NULL
   ,InActive BIT NULL
   ,CreatedUserID INT NULL
   ,ModifiedUserID INT NULL
   ,CreatedDate DATETIME NULL
   ,ModifiedDate DATETIME NULL
   ,CONSTRAINT PK_SSPWorkflowTask PRIMARY KEY CLUSTERED (TaskID)
) ON [PRIMARY]
GO

--
-- Create table [dbo].[SSPWorkflow]
--
PRINT (N'Create table [dbo].[SSPWorkflow]')
GO
CREATE TABLE dbo.SSPWorkflow (
	WorkflowID INT IDENTITY
   ,WorkflowTitle NVARCHAR(100) NULL
   ,SourcingTypeID INT NULL
   ,Description NVARCHAR(500) NULL
   ,TotalDays NVARCHAR(500) NULL
   ,TotalTaskDays NVARCHAR(500) NULL
   ,InActive BIT NULL
   ,CreatedUserID INT NULL
   ,ModifiedUserID INT NULL
   ,CreatedDate DATETIME NULL
   ,ModifiedDate DATETIME NULL
   ,CONSTRAINT PK_SSPWorkflow PRIMARY KEY CLUSTERED (WorkflowID)
) ON [PRIMARY]
GO

--
-- Create table [dbo].[SSPUsersPwd]
--
PRINT (N'Create table [dbo].[SSPUsersPwd]')
GO
CREATE TABLE dbo.SSPUsersPwd (
	pwdID INT IDENTITY
   ,UserPassword NVARCHAR(30) NOT NULL
   ,UserID INT NOT NULL
   ,InActive BIT NULL
   ,CreatedUserID INT NOT NULL
   ,ModifiedUserID INT NULL
   ,CreatedDate DATETIME NULL
   ,ModifiedDate DATETIME NULL
   ,FailedLoginAttempt INT NULL
   ,CONSTRAINT PK_SSPUsersPwd PRIMARY KEY CLUSTERED (pwdID)
) ON [PRIMARY]
GO

--
-- Create table [dbo].[SSPUsers]
--
PRINT (N'Create table [dbo].[SSPUsers]')
GO
CREATE TABLE dbo.SSPUsers (
	UserID INT IDENTITY
   ,UserName NVARCHAR(150) NULL
   ,Fname NVARCHAR(150) NULL
   ,lname NVARCHAR(150) NULL
   ,FullName NVARCHAR(150) NULL
   ,Email NVARCHAR(150) NULL
   ,RoleID INT NULL
   ,LoginDT DATETIME NULL
   ,InActive BIT NULL
   ,CreatedUserID INT NULL
   ,ModifiedUserID INT NULL
   ,CreatedDate DATETIME NULL
   ,ModifiedDate DATETIME NULL
   ,UserLevel INT NULL
   ,UserImage NVARCHAR(MAX) NULL
   ,EMEI NVARCHAR(50) NULL
   ,ContactNo NVARCHAR(15) NULL
   ,MISUID VARCHAR(100) NULL
   ,CONSTRAINT PK_SSPUsers PRIMARY KEY CLUSTERED (UserID)
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

--
-- Create table [dbo].[SSPTSPTrainingLocation]
--
PRINT (N'Create table [dbo].[SSPTSPTrainingLocation]')
GO
CREATE TABLE dbo.SSPTSPTrainingLocation (
	TrainingLocationID INT IDENTITY
   ,TrainingLocationName NVARCHAR(100) NULL
   ,TspProfileID INT NULL
   ,Province INT NULL
   ,Cluster INT NULL
   ,District INT NULL
   ,Tehsil INT NULL
   ,TrainingLocationAddress VARCHAR(500) NULL
   ,GeoTagging VARCHAR(500) NULL
   ,FrontMainEntrancePhoto NVARCHAR(500) NULL
   ,ClassroomPhoto NVARCHAR(500) NULL
   ,PracticalAreaPhoto NVARCHAR(500) NULL
   ,ComputerLabPhoto NVARCHAR(500) NULL
   ,ToolsAndEquipmentsPhoto NVARCHAR(500) NULL
   ,RegistrationAuthority INT NULL
   ,InActive BIT NULL
   ,CreatedUserID INT NULL
   ,ModifiedUserID INT NULL
   ,CreatedDate DATETIME NULL
   ,ModifiedDate DATETIME NULL
   ,CONSTRAINT PK_SSPTSPTrainingLocation PRIMARY KEY CLUSTERED (TrainingLocationID)
) ON [PRIMARY]
GO

--
-- Create table [dbo].[SSPTSPTradeStatusHistory]
--
PRINT (N'Create table [dbo].[SSPTSPTradeStatusHistory]')
GO
CREATE TABLE dbo.SSPTSPTradeStatusHistory (
	TspTradeStatusHistoryID INT IDENTITY
   ,StatusID INT NULL
   ,ProcurementRemarks NVARCHAR(2000) NULL
   ,Step INT NULL
   ,ApprovalLevel INT NULL
   ,TradeManageID INT NULL
   ,InActive BIT NULL
   ,CreatedUserID INT NULL
   ,CreatedDate DATETIME NULL
   ,CONSTRAINT PK_SSPTSPTradeStatusHistory PRIMARY KEY CLUSTERED (TspTradeStatusHistoryID)
) ON [PRIMARY]
GO

--
-- Create table [dbo].[SSPTSPTradeManage]
--
PRINT (N'Create table [dbo].[SSPTSPTradeManage]')
GO
CREATE TABLE dbo.SSPTSPTradeManage (
	TradeManageID INT IDENTITY
   ,TrainingLocationID INT NULL
   ,CertificateID INT NULL
   ,TradeID INT NULL
   ,TradeAsPerCer VARCHAR(250) NULL
   ,TrainingDuration NVARCHAR(250) NULL
   ,NoOfClassMor INT NULL
   ,ClassCapacityMor INT NULL
   ,NoOfClassEve INT NULL
   ,ClassCapacityEve INT NULL
   ,TspProfileID INT NULL
   ,ApprovalLevel INT NULL
   ,InActive BIT NULL
   ,CreatedUserID INT NULL
   ,ModifiedUserID INT NULL
   ,CreatedDate DATETIME NULL
   ,ModifiedDate DATETIME NULL
   ,CONSTRAINT PK_SSPTSPTradeManage PRIMARY KEY CLUSTERED (TradeManageID)
) ON [PRIMARY]
GO

--
-- Create table [dbo].[SSPTSPRegistrationPaymentDetail]
--
PRINT (N'Create table [dbo].[SSPTSPRegistrationPaymentDetail]')
GO
CREATE TABLE dbo.SSPTSPRegistrationPaymentDetail (
	TSPRegistrationPaymentID INT IDENTITY
   ,TSPID INT NULL
   ,TrainingLocationID INT NULL
   ,TrainingLocationFee DECIMAL(9, 2) NULL
   ,PayProPaymentTableID INT NULL
   ,PayProPaymentCode NVARCHAR(500) NULL
   ,IsApproved BIT NULL
   ,IsRejected BIT NULL
   ,Inactive BIT NULL
   ,CreatedUserID INT NULL
   ,CreatedDate DATETIME NULL
   ,ModifiedUserID INT NULL
   ,ModifiedDate DATETIME NULL
   ,CONSTRAINT PK_SSPTSPRegistrationPaymentDetail PRIMARY KEY CLUSTERED (TSPRegistrationPaymentID)
) ON [PRIMARY]
GO

--
-- Create table [dbo].[SSPTSPProfile]
--
PRINT (N'Create table [dbo].[SSPTSPProfile]')
GO
CREATE TABLE dbo.SSPTSPProfile (
	TspID INT IDENTITY
   ,BusinessName NVARCHAR(250) NULL
   ,RegistrationDate DATE NULL
   ,NTN NVARCHAR(9) NULL
   ,NTNEvidence NVARCHAR(MAX) NULL
   ,TspEmail NVARCHAR(150) NULL
   ,IsEmailVerify BIT NULL
   ,OTPCode NVARCHAR(10) NULL
   ,TspContact NVARCHAR(12) NULL
   ,Office NVARCHAR(250) NULL
   ,SalesTaxType NVARCHAR(50) NULL
   ,GST NVARCHAR(500) NULL
   ,GSTEvidence NVARCHAR(MAX) NULL
   ,PRA NVARCHAR(500) NULL
   ,PRAEvidence NVARCHAR(MAX) NULL
   ,LegalStatusID INT NULL
   ,LegalStatusEvidence NVARCHAR(MAX) NULL
   ,ProvinceID INT NULL
   ,ClusterID INT NULL
   ,DistrictID INT NULL
   ,TehsilID INT NULL
   ,GeoTagging NVARCHAR(MAX) NULL
   ,Address NVARCHAR(MAX) NULL
   ,ProgramTypeID INT NULL
   ,Website NVARCHAR(500) NULL
   ,HeadName NVARCHAR(500) NULL
   ,HeadDesignation NVARCHAR(100) NULL
   ,HeadCnicNum NVARCHAR(20) NULL
   ,HeadCnicFrontImg NVARCHAR(500) NULL
   ,HeadCnicBackImg NVARCHAR(500) NULL
   ,HeadEmail NVARCHAR(100) NULL
   ,HeadMobile NVARCHAR(12) NULL
   ,OrgLandline NVARCHAR(20) NULL
   ,POCName NVARCHAR(500) NULL
   ,POCDesignation NVARCHAR(100) NULL
   ,POCEmail NVARCHAR(100) NULL
   ,POCMobile NVARCHAR(12) NULL
   ,UserID INT NULL
   ,FinalSubmitted BIT NULL
   ,InActive BIT NULL
   ,CreatedUserID INT NULL
   ,ModifiedUserID INT NULL
   ,CreatedDate DATETIME NULL
   ,ModifiedDate DATETIME NULL
   ,CONSTRAINT PK_SSPTSPProfile PRIMARY KEY CLUSTERED (TspID)
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

--
-- Create table [dbo].[SSPTSPBankDetail]
--
PRINT (N'Create table [dbo].[SSPTSPBankDetail]')
GO
CREATE TABLE dbo.SSPTSPBankDetail (
	BankDetailID INT IDENTITY
   ,TspProfileID INT NULL
   ,BankID INT NULL
   ,OtherBank NVARCHAR(100) NULL
   ,AccountTitle NVARCHAR(200) NULL
   ,AccountNumber NVARCHAR(100) NULL
   ,BranchAddress NVARCHAR(500) NULL
   ,BranchCode NVARCHAR(100) NULL
   ,InActive BIT NULL
   ,CreatedUserID INT NULL
   ,ModifiedUserID INT NULL
   ,CreatedDate DATETIME NULL
   ,ModifiedDate DATETIME NULL
   ,CONSTRAINT PK_SSPTSPBankDetail PRIMARY KEY CLUSTERED (BankDetailID)
) ON [PRIMARY]
GO

--
-- Create table [dbo].[SSPTSPAssociationPaymentHistory]
--
PRINT (N'Create table [dbo].[SSPTSPAssociationPaymentHistory]')
GO
CREATE TABLE dbo.SSPTSPAssociationPaymentHistory (
	AssociationPaymentHistoryID INT IDENTITY
   ,TSPAssociationPaymentID INT NULL
   ,StatusID INT NULL
   ,Remarks NVARCHAR(2000) NULL
   ,Inactive BIT NULL
   ,CreatedUserID INT NULL
   ,CreatedDate DATETIME NULL
   ,ModifiedUserID INT NULL
   ,ModifiedDate DATETIME NULL
   ,CONSTRAINT PK_SSPTSPAssociationPaymentHistory PRIMARY KEY CLUSTERED (AssociationPaymentHistoryID)
) ON [PRIMARY]
GO

--
-- Create table [dbo].[SSPTSPAssociationPaymentDetail]
--
PRINT (N'Create table [dbo].[SSPTSPAssociationPaymentDetail]')
GO
CREATE TABLE dbo.SSPTSPAssociationPaymentDetail (
	TSPAssociationPaymentID INT IDENTITY
   ,TSPID INT NULL
   ,TradeLotID INT NULL
   ,TrainingLocationID INT NULL
   ,NoOfClasses VARCHAR(50) NULL
   ,TradeLotFee DECIMAL(9, 2) NULL
   ,PayProPaymentTableID INT NULL
   ,PayProPaymentCode NVARCHAR(500) NULL
   ,IsApproved BIT NULL
   ,IsRejected BIT NULL
   ,Inactive BIT NULL
   ,CreatedUserID INT NULL
   ,CreatedDate DATETIME NULL
   ,ModifiedUserID INT NULL
   ,ModifiedDate DATETIME NULL
   ,CONSTRAINT PK_SSPTSPAssociationPaymentDetail PRIMARY KEY CLUSTERED (TSPAssociationPaymentID)
) ON [PRIMARY]
GO

--
-- Create table [dbo].[SSPTspAssociationMaster]
--
PRINT (N'Create table [dbo].[SSPTspAssociationMaster]')
GO
CREATE TABLE dbo.SSPTspAssociationMaster (
	TspAssociationMasterID INT IDENTITY
   ,ProgramDesignID INT NULL
   ,TrainingLocationID INT NULL
   ,TradeLotID INT NULL
   ,TrainerDetailID INT NULL
   ,TradeLotTitle NVARCHAR(500) NULL
   ,InActive BIT NULL
   ,CreatedUserID INT NULL
   ,ModifiedUserID INT NULL
   ,CreatedDate DATETIME NULL
   ,ModifiedDate DATETIME NULL
   ,CONSTRAINT PK_SSPTspAssociationMaster PRIMARY KEY CLUSTERED (TspAssociationMasterID)
) ON [PRIMARY]
GO

--
-- Create table [dbo].[SSPTspAssociationEvaluation]
--
PRINT (N'Create table [dbo].[SSPTspAssociationEvaluation]')
GO
CREATE TABLE dbo.SSPTspAssociationEvaluation (
	TspAssociationEvaluationID INT IDENTITY
   ,TspAssociationMasterID INT NULL
   ,VerifiedCapacityMorning INT NULL
   ,VerifiedCapacityEvening INT NULL
   ,TotalCapacity INT NULL
   ,MarksBasedOnEvaluation INT NULL
   ,CategoryBasedOnEvaluation NVARCHAR(200) NULL
   ,EvaluationStatus INT NULL
  ,[Attachment] [nvarchar](500) NULL
 ,[Remarks] [nvarchar](max) NULL
   ,InActive BIT NULL
   ,CreatedUserID INT NULL
   ,ModifiedUserID INT NULL
   ,CreatedDate DATETIME NULL
   ,ModifiedDate DATETIME NULL
   ,CONSTRAINT PK_SSPTspAssociationEvaluation PRIMARY KEY CLUSTERED (TspAssociationEvaluationID)
) ON [PRIMARY]
GO

--
-- Create table [dbo].[SSPTspAssociationDetail]
--
PRINT (N'Create table [dbo].[SSPTspAssociationDetail]')
GO
CREATE TABLE dbo.SSPTspAssociationDetail (
	TspAssociationDetailID INT IDENTITY
   ,TspAssociationMasterID INT NULL
   ,CriteriaMainCategoryID INT NULL
   ,Attachment NVARCHAR(500) NULL
   ,Remarks NVARCHAR(500) NULL
   ,InActive BIT NULL
   ,CreatedUserID INT NULL
   ,ModifiedUserID INT NULL
   ,CreatedDate DATETIME NULL
   ,ModifiedDate DATETIME NULL
   ,CONSTRAINT PK_SSPTspAssociationDetail PRIMARY KEY CLUSTERED (TspAssociationDetailID)
) ON [PRIMARY]
GO

--
-- Create table [dbo].[SSPTrainingCertification]
--
PRINT (N'Create table [dbo].[SSPTrainingCertification]')
GO
CREATE TABLE dbo.SSPTrainingCertification (
	TrainingCertificationID INT IDENTITY
   ,TrainingLocationID INT NULL
   ,TspProfileID INT NULL
   ,RegistrationAuthority INT NULL
   ,RegistrationStatus INT NULL
   ,RegistrationCerNum VARCHAR(250) NULL
   ,IssuanceDate DATE NULL
   ,ExpiryDate VARCHAR(250) NULL
   ,RegistrationCerEvidence NVARCHAR(500) NULL
   ,InActive BIT NULL
   ,CreatedUserID INT NULL
   ,ModifiedUserID INT NULL
   ,CreatedDate DATETIME NULL
   ,ModifiedDate DATETIME NULL
   ,CONSTRAINT PK_SSPTrainingCertification PRIMARY KEY CLUSTERED (TrainingCertificationID)
) ON [PRIMARY]
GO

--
-- Create table [dbo].[SSPTrainerProfileDetail]
--
PRINT (N'Create table [dbo].[SSPTrainerProfileDetail]')
GO
CREATE TABLE dbo.SSPTrainerProfileDetail (
	TrainerDetailID INT IDENTITY
   ,TrainerProfileID INT NULL
   ,TrainerTradeID INT NULL
   ,ProfQualEvidence NVARCHAR(500) NULL
   ,ProfQualification NVARCHAR(100) NULL
   ,CertificateBody NVARCHAR(100) NULL
   ,RelExpYear [decimal](10, 2) NULL
   ,RelExpLetter NVARCHAR(500) NULL
   ,TspProfileID INT NULL
   ,InActive BIT NULL
   ,CreatedUserID INT NULL
   ,ModifiedUserID INT NULL
   ,CreatedDate DATETIME NULL
   ,ModifiedDate DATETIME NULL
   ,CONSTRAINT PK_SSPTrainerProfileDetail PRIMARY KEY CLUSTERED (TrainerDetailID)
) ON [PRIMARY]
GO

--
-- Create table [dbo].[SSPTrainerProfile]
--
PRINT (N'Create table [dbo].[SSPTrainerProfile]')
GO
CREATE TABLE dbo.SSPTrainerProfile (
	TrainerID INT IDENTITY
   ,TrainerName NVARCHAR(500) NULL
   ,TrainerMobile NVARCHAR(12) NULL
   ,TrainerEmail NVARCHAR(500) NULL
   ,Gender INT NULL
   ,TrainerCNIC NVARCHAR(20) NULL
   ,CnicFrontPhoto NVARCHAR(500) NULL
   ,CnicBackPhoto NVARCHAR(500) NULL
   ,Qualification INT NULL
   ,QualEvidence NVARCHAR(500) NULL
   ,TrainerCV NVARCHAR(500) NULL
   ,TspProfileID INT NULL
   ,InActive BIT NULL
   ,CreatedUserID INT NULL
   ,ModifiedUserID INT NULL
   ,CreatedDate DATETIME NULL
   ,ModifiedDate DATETIME NULL
   ,CONSTRAINT PK_SSPTrainerProfile PRIMARY KEY CLUSTERED (TrainerID)
) ON [PRIMARY]
GO

--
-- Create table [dbo].[SSPTraineeSupportItems]
--
PRINT (N'Create table [dbo].[SSPTraineeSupportItems]')
GO
CREATE TABLE dbo.SSPTraineeSupportItems (
	ID INT IDENTITY
   ,Applicability NVARCHAR(50) NULL
   ,InActive BIT NULL
   ,CreatedUserID INT NULL
   ,ModifiedUserID INT NULL
   ,CreatedDate DATETIME NULL
   ,ModifiedDate DATETIME NULL
   ,CONSTRAINT PK_SSPTraineeSupportItems PRIMARY KEY CLUSTERED (ID)
) ON [PRIMARY]
GO
--
-- Create table [dbo].[SSPTradeLot]
--
PRINT (N'Create table [dbo].[SSPTradeLot]')
GO
CREATE TABLE dbo.SSPTradeLot (
	TradeLotID INT IDENTITY
   ,TradeDesignID INT NULL
   ,TradeID INT NULL
   ,TradeLotNo INT NULL CONSTRAINT DF_SSPTradeLot_TradeLotNo DEFAULT (0)
   ,TradeDetailMapID INT NULL
   ,ProgramDesignOn NVARCHAR(200) NULL
   ,ProvinceID INT NULL
   ,ClusterID INT NULL
   ,DistrictID INT NULL
   ,Duration FLOAT NULL
   ,TraineeSelectedContTarget INT NULL
   ,CTM DECIMAL(10, 2) NULL
   ,TrainingCost DECIMAL(10, 2) NULL
   ,Stipend DECIMAL(10, 2) NULL
   ,BagAndBadge DECIMAL(10, 2) NULL
   ,ExamCost DECIMAL(10, 2) NULL
   ,TotalCost DECIMAL(10, 2) NULL
   ,Inactive BIT NULL
   ,CreatedUserID INT NULL
   ,CreatedDate DATETIME NULL
   ,ModifiedUserID INT NULL
   ,ModifiedDate DATETIME NULL
   ,CONSTRAINT PK_SSPTradeLot PRIMARY KEY CLUSTERED (TradeLotID)
) ON [PRIMARY]
GO

--
-- Create table [dbo].[SSPTradeDesign]
--
PRINT (N'Create table [dbo].[SSPTradeDesign]')
GO
CREATE TABLE dbo.SSPTradeDesign (
	TradeDesignID INT IDENTITY
   ,ProgramDesignID INT NULL
   ,GenderID INT NULL
   ,ProvinceID NVARCHAR(MAX) NULL
   ,ClusterID NVARCHAR(MAX) NULL
   ,DistrictID NVARCHAR(MAX) NULL
   ,ProgramDesignOn NVARCHAR(200) NULL
   ,SelectedCount INT NULL
   ,SelectedShortList NVARCHAR(MAX) NULL
   ,ProgramFocusID INT NULL
   ,TradeID INT NULL
   ,TradeDetailMapID INT NULL
   ,CTM INT NULL
   ,DropOutPerAge INT NULL
   ,ExamCost INT NULL
   ,TraineeContractedTarget INT NULL
   ,TraineeCompTarget INT NULL
   ,PerSelectedContraTarget INT NULL
   ,PerSelectedCompTarget INT NULL
   ,Inactive BIT NULL
   ,CreatedUserID INT NULL
   ,CreatedDate DATETIME NULL
   ,ModifiedUserID INT NULL
   ,ModifiedDate DATETIME NULL
   ,CONSTRAINT PK_SSPTradeDesign_1 PRIMARY KEY CLUSTERED (TradeDesignID)
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

--
-- Create table [dbo].[SSPSelectionMethods]
--
PRINT (N'Create table [dbo].[SSPSelectionMethods]')
GO
CREATE TABLE dbo.SSPSelectionMethods (
	ID INT IDENTITY
   ,MethodName NVARCHAR(50) NULL
   ,InActive BIT NULL
   ,CreatedUserID INT NULL
   ,ModifiedUserID INT NULL
   ,CreatedDate DATETIME NULL
   ,ModifiedDate DATETIME NULL
) ON [PRIMARY]
GO

--
-- Create table [dbo].[SSPSalesTaxType]
--
PRINT (N'Create table [dbo].[SSPSalesTaxType]')
GO
CREATE TABLE dbo.SSPSalesTaxType (
	SalesTaxID INT IDENTITY
   ,SalesTaxType NVARCHAR(250) NULL
   ,InActive BIT NULL
   ,CreatedUserID INT NULL
   ,CreatedDate DATETIME NULL
   ,ModifiedUserID INT NULL
   ,ModifiedDate DATETIME NULL
   ,CONSTRAINT PK_SSPSalesTaxType PRIMARY KEY CLUSTERED (SalesTaxID)
) ON [PRIMARY]
GO

--
-- Create table [dbo].[SSPRegistrationStatus]
--
PRINT (N'Create table [dbo].[SSPRegistrationStatus]')
GO
CREATE TABLE dbo.SSPRegistrationStatus (
	RegistrationStatusID INT IDENTITY
   ,RegistrationStatus NVARCHAR(100) NULL
   ,InActive BIT NULL
   ,CreatedUserID INT NULL
   ,ModifiedUserID INT NULL
   ,CreatedDate DATETIME NULL
   ,ModifiedDate DATETIME NULL
   ,CONSTRAINT PK_SSPRegistrationStatus PRIMARY KEY CLUSTERED (RegistrationStatusID)
) ON [PRIMARY]
GO

--
-- Create table [dbo].[SSPProgramWorkflowHistory]
--
PRINT (N'Create table [dbo].[SSPProgramWorkflowHistory]')
GO
CREATE TABLE dbo.SSPProgramWorkflowHistory (
	ID INT IDENTITY
   ,ProgramID INT NULL
   ,WorkflowID INT NULL
   ,Remarks NVARCHAR(2000) NULL
   ,IsInactive BIT NULL
   ,CreatedUserID INT NULL
   ,CreatedDate DATETIME NULL
   ,ModifiedUserID INT NULL
   ,ModifiedDate DATETIME NULL
   ,CONSTRAINT PK_SSPProgramWorkflowHistory PRIMARY KEY CLUSTERED (ID)
) ON [PRIMARY]
GO

--
-- Create table [dbo].[SSPProgramStatusHistory]
--
PRINT (N'Create table [dbo].[SSPProgramStatusHistory]')
GO
CREATE TABLE dbo.SSPProgramStatusHistory (
	ID INT IDENTITY
   ,ProgramID INT NULL
   ,StatusID INT NULL
   ,Remarks NVARCHAR(2000) NULL
   ,IsInactive BIT NULL
   ,CreatedUserID INT NULL
   ,CreatedDate DATETIME NULL
   ,ModifiedUserID INT NULL
   ,ModifiedDate DATETIME NULL
   ,CONSTRAINT PK_SSPProgramStatusHistory PRIMARY KEY CLUSTERED (ID)
) ON [PRIMARY]
GO

--
-- Create table [dbo].[SSPProgramDesign]
--
PRINT (N'Create table [dbo].[SSPProgramDesign]')
GO
CREATE TABLE dbo.SSPProgramDesign (
	ProgramID INT IDENTITY
   ,ProgramName NVARCHAR(MAX) NULL
   ,ProgramCode NVARCHAR(50) NULL
   ,ProgramTypeID INT NULL
   ,PCategoryID INT NULL
   ,FundingSourceID INT NULL
   ,FundingCategoryID INT NULL
   ,PaymentSchedule INT NULL
   ,ProgramDescription NVARCHAR(MAX) NULL
   ,Stipend FLOAT NULL
   ,StipendMode NVARCHAR(100) NULL
   ,ApplicabilityID NVARCHAR(50) NULL
   ,MinimumEducation INT NULL
   ,MaximumEducation INT NULL
   ,MinAge INT NULL
   ,MaxAge INT NULL
   ,GenderID INT NULL
   ,ContractAwardDate DATETIME NULL
   ,BusinessRuleType NVARCHAR(MAX) NULL
   ,CreatedUserID INT NULL
   ,ModifiedUserID INT NULL
   ,InActive BIT NULL
   ,FinalSubmitted BIT NULL
   ,PlanningType NVARCHAR(50) NULL
   ,TentativeProcessStart DATETIME NULL
   ,ClassStartDate DATETIME NULL
   ,SelectionMethod NVARCHAR(50) NULL
   ,EmploymentCommitment NVARCHAR(50) NULL
   ,SchemeDesignOn NVARCHAR(50) NULL
   ,Province NVARCHAR(500) NULL
   ,Cluster NVARCHAR(500) NULL
   ,District NVARCHAR(MAX) NULL
   ,ApprovalDescription NVARCHAR(MAX) NULL
   ,ApprovalAttachment NVARCHAR(MAX) NULL
   ,TORsAttachment NVARCHAR(MAX) NULL
   ,CriteriaAttachment NVARCHAR(MAX) NULL
   ,FinancialYear INT NULL
   ,bagBadgeCost FLOAT NULL
   ,IsInitiate BIT NULL
   ,IsFinalApproved BIT NULL
   ,WorkflowID INT NULL
   ,CriteriaID INT NULL
   ,ProgramStatusID INT NULL
   ,IsSubmitted BIT NULL
   ,IsApproved INT NULL
   ,IsRejected INT NULL
   ,ModifiedDate DATETIME NULL
   ,CreatedDate DATETIME NULL
   ,CONSTRAINT PK_SSPProgramDesign PRIMARY KEY CLUSTERED (ProgramID)
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

--
-- Create table [dbo].[SSPProgramCriteriaHistory]
--
PRINT (N'Create table [dbo].[SSPProgramCriteriaHistory]')
GO
CREATE TABLE dbo.SSPProgramCriteriaHistory (
	ID INT IDENTITY
   ,ProgramID INT NULL
   ,CriteriaID INT NULL
   ,Remarks NVARCHAR(2000) NULL
   ,StartDate DATETIME NULL
   ,EndDate DATETIME NULL
   ,IsInactive BIT NULL
   ,CreatedUserID INT NULL
   ,CreatedDate DATETIME NULL
   ,ModifiedUserID INT NULL
   ,ModifiedDate DATETIME NULL
   ,CONSTRAINT PK_SSPProgramCriteriaHistory PRIMARY KEY CLUSTERED (ID)
) ON [PRIMARY]
GO

--
-- Create table [dbo].[SSPProcessScheduleDetail]
--
PRINT (N'Create table [dbo].[SSPProcessScheduleDetail]')
GO
CREATE TABLE dbo.SSPProcessScheduleDetail (
	ProcessScheduleDetailID INT IDENTITY
   ,ProcessScheduleMasterID INT NULL
   ,ProcessID INT NULL
   ,ProcessStartDate DATETIME NULL
   ,ProcessEndDate DATETIME NULL
   ,ProcessDays NVARCHAR(500) NULL
   ,InActive BIT NULL
   ,CreatedUserID INT NULL
   ,ModifiedUserID INT NULL
   ,CreatedDate DATETIME NULL
   ,ModifiedDate DATETIME NULL
   ,CONSTRAINT PK_SSPProcessScheduleDetail PRIMARY KEY CLUSTERED (ProcessScheduleDetailID)
) ON [PRIMARY]
GO

--
-- Create table [dbo].[SSPProcessSchedule]
--
PRINT (N'Create table [dbo].[SSPProcessSchedule]')
GO
CREATE TABLE dbo.SSPProcessSchedule (
	ProcessID INT IDENTITY
   ,ProcessName VARCHAR(500) NULL
   ,ProcessStartDate DATETIME NULL
   ,ProcessEndDate DATETIME NULL
   ,DurationDays INT NULL
   ,ProgramName NVARCHAR(500) NULL
   ,programStartDate DATETIME NULL
   ,Inactive BIT NULL
   ,CreatedUserID INT NULL
   ,CreatedDate DATETIME NULL
   ,ModifiedUserID INT NULL
   ,ModifiedDate DATETIME NULL
   ,CONSTRAINT PK_SSPProcessSchedule PRIMARY KEY CLUSTERED (ProcessID)
) ON [PRIMARY]
GO

--
-- Create table [dbo].[SSPPlaningType]
--
PRINT (N'Create table [dbo].[SSPPlaningType]')
GO
CREATE TABLE dbo.SSPPlaningType (
	PlaningTypeID INT IDENTITY
   ,PlaningType NVARCHAR(100) NULL
   ,InActive BIT NULL
   ,CreatedUserID INT NULL
   ,ModifiedUserID INT NULL
   ,CreatedDate DATETIME NULL
   ,ModifiedDate DATETIME NULL
   ,CONSTRAINT PK_SSPPlaningType PRIMARY KEY CLUSTERED (PlaningTypeID)
) ON [PRIMARY]
GO

--
-- Create table [dbo].[SSPLegalStatus]
--
PRINT (N'Create table [dbo].[SSPLegalStatus]')
GO
CREATE TABLE dbo.SSPLegalStatus (
	LegalStatusID INT IDENTITY
   ,LegalStatusName VARCHAR(350) NOT NULL
   ,InActive BIT NULL
   ,CreatedUserID INT NULL
   ,ModifiedUserID INT NULL
   ,CreatedDate DATETIME NULL
   ,ModifiedDate DATETIME NULL
   ,CONSTRAINT PK_SSPLegalStatus PRIMARY KEY CLUSTERED (LegalStatusID)
) ON [PRIMARY]
GO

--
-- Create table [dbo].[SSPCriteriaSubCategory]
--
PRINT (N'Create table [dbo].[SSPCriteriaSubCategory]')
GO
CREATE TABLE dbo.SSPCriteriaSubCategory (
	CriteriaSubCategoryID INT IDENTITY
   ,CriteriaHeaderID INT NULL
   ,CriteriaMainCategoryID INT NULL
   ,SubCategoryTitle NVARCHAR(150) NULL
   ,SubCategoryDesc NVARCHAR(150) NULL
   ,Criteria NVARCHAR(150) NULL
   ,MarkedCriteria NVARCHAR(150) NULL
   ,IsMandatory NVARCHAR(150) NULL
   ,MaxMarks NVARCHAR(150) NULL
   ,Attachment NVARCHAR(500) NULL
   ,InActive BIT NULL
   ,CreatedUserID INT NULL
   ,CreatedDate DATETIME NULL
   ,ModifiedUserID INT NULL
   ,ModifiedDate DATETIME NULL
   ,CONSTRAINT PK_SSPCriteriaSubCategory PRIMARY KEY CLUSTERED (CriteriaSubCategoryID)
) ON [PRIMARY]
GO

--
-- Create table [dbo].[SSPCriteriaMainCategory]
--
PRINT (N'Create table [dbo].[SSPCriteriaMainCategory]')
GO
CREATE TABLE dbo.SSPCriteriaMainCategory (
	CriteriaMainCategoryID INT IDENTITY
   ,CriteriaHeaderID INT NULL
   ,MainCategoryTitle NVARCHAR(150) NULL
   ,MainCategoryDesc NVARCHAR(150) NULL
   ,TotalMarks NVARCHAR(150) NULL
   ,InActive BIT NULL
   ,CreatedUserID INT NULL
   ,CreatedDate DATETIME NULL
   ,ModifiedUserID INT NULL
   ,ModifiedDate DATETIME NULL
   ,CONSTRAINT PK_SSPCriteriaMainCategory PRIMARY KEY CLUSTERED (CriteriaMainCategoryID)
) ON [PRIMARY]
GO

--
-- Create table [dbo].[SSPCriteriaHeader]
--
PRINT (N'Create table [dbo].[SSPCriteriaHeader]')
GO
CREATE TABLE dbo.SSPCriteriaHeader (
	CriteriaHeaderID INT IDENTITY
   ,HeaderTitle NVARCHAR(150) NULL
   ,HeaderDesc NVARCHAR(150) NULL
   ,IsMarking NVARCHAR(150) NULL
   ,MaxMarks NVARCHAR(150) NULL
   ,[IsSubmitted] [bit] NULL
   ,[IsApproved] [bit] NULL
   ,[IsRejected] [bit] NULL
   ,InActive BIT NULL
   ,CreatedUserID INT NULL
   ,CreatedDate DATETIME NULL
   ,ModifiedUserID INT NULL
   ,ModifiedDate DATETIME NULL
   ,CONSTRAINT PK_SSPCriteriaHeader PRIMARY KEY CLUSTERED (CriteriaHeaderID)
) ON [PRIMARY]
GO

--
-- Create table [dbo].[SSPBank]
--
PRINT (N'Create table [dbo].[SSPBank]')
GO
CREATE TABLE dbo.SSPBank (
	BankID INT IDENTITY
   ,BankName VARCHAR(350) NOT NULL
   ,InActive BIT NULL
   ,CreatedUserID INT NULL
   ,ModifiedUserID INT NULL
   ,CreatedDate DATETIME NULL
   ,ModifiedDate DATETIME NULL
   ,CONSTRAINT PK_SSPBank PRIMARY KEY CLUSTERED (BankID)
) ON [PRIMARY]
GO

--
-- Create table [dbo].[SSPApprovalStatus]
--
PRINT (N'Create table [dbo].[SSPApprovalStatus]')
GO
CREATE TABLE dbo.SSPApprovalStatus (
	TspTradeStatusID INT IDENTITY
   ,Status NVARCHAR(100) NULL
   ,InActive BIT NULL
   ,CreatedUserID INT NULL
   ,CreatedDate DATETIME NULL
   ,CONSTRAINT PK_SSPApprovalStatus PRIMARY KEY CLUSTERED (TspTradeStatusID)
) ON [PRIMARY]
GO

--
-- Create table [dbo].[SSPTSPRegistrationFee]
--
PRINT (N'Create table [dbo].[SSPTSPRegistrationFee]')
GO
CREATE TABLE dbo.SSPTSPRegistrationFee (
	RegistrationFeeID INT IDENTITY
   ,RegistrationFee DECIMAL(9, 2) NULL
   ,Inactive BIT NULL
   ,CreatedUserID INT NULL
   ,CreatedDate DATETIME NULL
   ,ModifiedUserID INT NULL
   ,ModifiedDate DATETIME NULL
   ,CONSTRAINT PK_SSPTSPRegistrationFee PRIMARY KEY CLUSTERED (RegistrationFeeID)
) ON [PRIMARY]
GO

--
-- Create table [dbo].[SSPTSPAssociationFee]
--
PRINT (N'Create table [dbo].[SSPTSPAssociationFee]')
GO
CREATE TABLE dbo.SSPTSPAssociationFee (
	AssociationFeeID INT IDENTITY
   ,AssociationFee DECIMAL(9, 2) NULL
   ,Inactive BIT NULL
   ,CreatedUserID INT NULL
   ,CreatedDate DATETIME NULL
   ,ModifiedUserID INT NULL
   ,ModifiedDate DATETIME NULL
   ,CONSTRAINT PK_SSPTSPAssociationFee PRIMARY KEY CLUSTERED (AssociationFeeID)
) ON [PRIMARY]
GO

--
-- Create table [dbo].[SSPTSPAssignment]
--
PRINT (N'Create table [dbo].[SSPTSPAssignment]')
GO
CREATE TABLE dbo.SSPTSPAssignment (
	TSPAssignmentID INT IDENTITY
   ,ProgramID INT NULL
   ,TradeLotID INT NULL
   ,AssociationEvaluationID INT NULL
   ,TrainingLocationID INT NULL
   ,TSPID INT NULL
   ,IsFinal bit NULL DEFAULT (0)
   ,InActive BIT NULL
   ,CreatedUserID INT NULL
   ,ModifiedUserID INT NULL
   ,CreatedDate DATETIME NULL
   ,ModifiedDate DATETIME NULL
   ,CONSTRAINT PK_SSPTSPAssignment PRIMARY KEY CLUSTERED (TSPAssignmentID)
) ON [PRIMARY]
GO

--
-- Create table [dbo].[SSPProcessScheduleMaster]
--
PRINT (N'Create table [dbo].[SSPProcessScheduleMaster]')
GO
CREATE TABLE dbo.SSPProcessScheduleMaster (
	ProcessScheduleMasterID INT IDENTITY
   ,ProgramID INT NULL
   ,ProgramStartDate DATETIME NULL
   ,TotalDays INT NULL
   ,TotalProcess INT NULL
   ,InActive BIT NULL
   ,CreatedUserID INT NULL
   ,ModifiedUserID INT NULL
   ,CreatedDate DATETIME NULL
   ,ModifiedDate DATETIME NULL
   ,CONSTRAINT PK_SSPProcessScheduleMaster PRIMARY KEY CLUSTERED (ProcessScheduleMasterID)
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[SSPTraineeInterestProfile] (
    [TraineeIntrestID] [int] IDENTITY (1, 1) NOT NULL
   ,[TraineeRegistrationID] [int] NULL
   ,[TraineeName] [nvarchar](250) NULL
   ,[TraineeCNIC] [nvarchar](50) NULL
   ,[CNICIssueDate] [date] NULL
   ,[FatherName] [nvarchar](250) NULL
   ,[GenderID] [int] NULL
   ,[TradeID] [int] NULL
   ,[TrainingLocationID] [int] NULL
   ,[TradeLotID] [int] NULL
   ,[DateOfBirth] [date] NULL
   ,[SchemeID] [int] NULL
   ,[TspID] [int] NULL
   ,[DistrictID] [int] NULL
   ,[EducationID] [int] NOT NULL
   ,[ContactNumber1] [nvarchar](50) NULL
   ,[TraineeAge] [int] NULL
   ,[ReligionID] [int] NULL
   ,[TraineeDistrictID] [int] NULL
   ,[TraineeTehsilID] [int] NULL
   ,[TraineeDisabilityID] [int] NULL
   ,[TraineeEmail] [nvarchar](50) NULL
   ,[Shift] [nvarchar](50) NULL
   ,[HouseHoldIncomeID] [int] NULL
   ,[CreatedDate] [datetime] NULL
   ,[CreatedUserID] [int] NULL
   ,[ModifiedUserID] [int] NULL
   ,[ModifiedDate] [datetime] NULL
   ,[InActive] [bit] NULL
   ,[IsSubmitted] [bit] NULL
   ,[IsFinalSubmitted] [bit] NULL
   ,[ApprovalStatus] [nvarchar](50) NULL
   ,[StatusReason] [nvarchar](MAX) NULL
   ,CONSTRAINT [PK_SSPTraineeInterestProfile] PRIMARY KEY CLUSTERED
	(
	[TraineeIntrestID] ASC
	) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dbo].[SSPNotificationDetail] (
	[NotificationDetailID] [int] IDENTITY (1, 1) NOT NULL PRIMARY KEY
   ,[Subject] [nvarchar](400) NULL
   ,[CustomComments] [nvarchar](MAX) NULL
   ,[IsRead] [bit] NULL DEFAULT (0)
   ,[UserID] [int] NULL
   ,[IsSent] [bit] DEFAULT (0) NULL
   ,[InActive] [bit] DEFAULT (0) NULL
   ,[NotificationTypeId] [int] DEFAULT (2) NOT NULL
   ,[CreatedUserID] [int] NULL
   ,[ModifiedUserID] [int] NULL
   ,[CreatedDate] [datetime] NULL
   ,[ModifiedDate] [datetime] NULL
)

GO



INSERT INTO Modules (ModuleTitle, modpath, SortOrder, UserLevel, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate)
	VALUES (N'Profile Management', N'profile-manage', 1, DEFAULT, 0, 0, 0, GETDATE(), GETDATE());

INSERT AppForms (FormName, Path, Icon, Controller, ModuleID, SortOrder, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate, IsAddible, IsEditable, IsDeletable, IsViewable)
	VALUES (N'Profile', N'profile', N'', N'BusinessProfile', (SELECT MAX(ModuleID) FROM Modules), 0, 0, NULL, 0, GETDATE(), NULL, 1, 1, 1, 1)
INSERT AppForms (FormName, Path, Icon, Controller, ModuleID, SortOrder, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate, IsAddible, IsEditable, IsDeletable, IsViewable)
	VALUES (N'Base Data', N'base-data', N'', N'BusinessProfile', (SELECT MAX(ModuleID) FROM Modules), 0, 0, NULL, 0, GETDATE(), NULL, 1, 1, 1, 1)
INSERT AppForms (FormName, Path, Icon, Controller, ModuleID, SortOrder, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate, IsAddible, IsEditable, IsDeletable, IsViewable)
	VALUES (N'Registration Evaluation', N'evaluation', N'', N'BusinessProfile', (SELECT MAX(ModuleID) FROM Modules), 0, 0, 0, 0, GETDATE(), GETDATE(), 1, 1, 1, 1)


INSERT INTO Modules (ModuleTitle, modpath, SortOrder, UserLevel, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate)
	VALUES (N'Annual Plan', N'annual-planning', 1, DEFAULT, 0, 0, 0, GETDATE(), GETDATE());
INSERT AppForms (FormName, Path, Icon, Controller, ModuleID, SortOrder, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate, IsAddible, IsEditable, IsDeletable, IsViewable)
	VALUES (N'Calculate CTM', N'calculate-ctm', N'', N'AnnualPlan', (SELECT MAX(ModuleID) FROM Modules), 0, 0, 0, 0, GETDATE(), GETDATE(), 1, 1, 1, 1)
INSERT AppForms (FormName, Path, Icon, Controller, ModuleID, SortOrder, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate, IsAddible, IsEditable, IsDeletable, IsViewable)
	VALUES (N'Program Plan', N'program-plan', N'', N'AnnualPlan', (SELECT MAX(ModuleID) FROM Modules), 0, 0, 0, 0, GETDATE(), GETDATE(), 1, 1, 1, 1)
INSERT AppForms (FormName, Path, Icon, Controller, ModuleID, SortOrder, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate, IsAddible, IsEditable, IsDeletable, IsViewable)
	VALUES (N'Trade Plan', N'trade-plan', N'', N'AnnualPlan', (SELECT MAX(ModuleID) FROM Modules), 0, 0, 0, 0, GETDATE(), GETDATE(), 1, 1, 1, 1)
INSERT AppForms (FormName, Path, Icon, Controller, ModuleID, SortOrder, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate, IsAddible, IsEditable, IsDeletable, IsViewable)
	VALUES (N'Program Initiate', N'program-initiate', N'', N'AnnualPlan', (SELECT MAX(ModuleID) FROM Modules), 0, 0, 0, 0, GETDATE(), GETDATE(), 1, 1, 1, 1)
INSERT AppForms (FormName, Path, Icon, Controller, ModuleID, SortOrder, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate, IsAddible, IsEditable, IsDeletable, IsViewable)
	VALUES (N'Validate Business Plan', N'validate-business-plan', N'', N'AnnualPlan', (SELECT MAX(ModuleID) FROM Modules), 0, 0, 0, 0, GETDATE(), GETDATE(), 1, 1, 1, 1)
INSERT AppForms (FormName, Path, Icon, Controller, ModuleID, SortOrder, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate, IsAddible, IsEditable, IsDeletable, IsViewable)
	VALUES (N'Process Approved Plan', N'process-approved-plan', N'', N'AnnualPlan', (SELECT MAX(ModuleID) FROM Modules), 0, 0, 0, 0, GETDATE(), GETDATE(), 1, 1, 1, 1)
INSERT AppForms (FormName, Path, Icon, Controller, ModuleID, SortOrder, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate, IsAddible, IsEditable, IsDeletable, IsViewable)
	VALUES (N'Process Status Update', N'process-status-update', N'', N'AnnualPlan', (SELECT MAX(ModuleID) FROM Modules), 0, 0, 0, 0, GETDATE(), GETDATE(), 1, 1, 1, 1)
INSERT AppForms (FormName, Path, Icon, Controller, ModuleID, SortOrder, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate, IsAddible, IsEditable, IsDeletable, IsViewable)
	VALUES (N'Analysis | Historical Report', N'historical-report', N'', N'ProgramDesign', (SELECT MAX(ModuleID) FROM Modules), 0, 0, 0, 0, GETDATE(), GETDATE(), 1, 1, 1, 1)
INSERT AppForms (FormName, Path, Icon, Controller, ModuleID, SortOrder, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate, IsAddible, IsEditable, IsDeletable, IsViewable)
	VALUES (N'Analysis Report', N'registration-analysis-report', N'', N'AnnualPlan', (SELECT MAX(ModuleID) FROM Modules), 0, 0, 0, 0, GETDATE(), GETDATE(), 1, 1, 1, 1)





INSERT INTO Modules (ModuleTitle, modpath, SortOrder, UserLevel, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate)
	VALUES (N'Association Management', N'association-management', 2, DEFAULT, 0, 0, 0, GETDATE(), GETDATE());
INSERT AppForms (FormName, Path, Icon, Controller, ModuleID, SortOrder, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate, IsAddible, IsEditable, IsDeletable, IsViewable)
	VALUES (N'Initiate Association', N'initiate-association', N'', N'Association', (SELECT MAX(ModuleID) FROM Modules), 0, 0, 0, 0, GETDATE(), GETDATE(), 1, 1, 1, 1)
INSERT AppForms (FormName, Path, Icon, Controller, ModuleID, SortOrder, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate, IsAddible, IsEditable, IsDeletable, IsViewable)
	VALUES (N'Association Evaluation', N'association-evaluation', N'', N'Association', (SELECT MAX(ModuleID) FROM Modules), 0, 0, 0, 0, GETDATE(), GETDATE(), 1, 1, 1, 1)
INSERT AppForms (FormName, Path, Icon, Controller, ModuleID, SortOrder, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate, IsAddible, IsEditable, IsDeletable, IsViewable)
	VALUES (N'TSP Assignment', N'tsp-assignment', N'', N'Association', (SELECT MAX(ModuleID) FROM Modules), 0, 0, 0, 0, GETDATE(), GETDATE(), 1, 1, 1, 1)
INSERT AppForms (FormName, Path, Icon, Controller, ModuleID, SortOrder, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate, IsAddible, IsEditable, IsDeletable, IsViewable)
	VALUES (N'Association Submission', N'association-submission', N'', N'Association', (SELECT MAX(ModuleID) FROM Modules), 0, 0, 0, 0, GETDATE(), GETDATE(), 1, 1, 1, 1)
INSERT AppForms (FormName, Path, Icon, Controller, ModuleID, SortOrder, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate, IsAddible, IsEditable, IsDeletable, IsViewable)
	VALUES (N'Association Detail', N'association-detail', N'', N'Association', (SELECT MAX(ModuleID) FROM Modules), 0, 0, 0, 0, GETDATE(), GETDATE(), 1, 1, 1, 1)

INSERT SSPRegistrationStatus (RegistrationStatus, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate)
	VALUES (N'Renewal / Application inprocess.', CONVERT(BIT, 'False'), 0, 0, '2024-03-06 16:09:35.070', '2024-03-06 16:09:35.070')
INSERT SSPRegistrationStatus (RegistrationStatus, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate)
	VALUES (N'Valid Registration Certificate', CONVERT(BIT, 'False'), 0, 0, '2024-03-06 16:09:35.070', '2024-03-06 16:09:35.070')
INSERT SSPRegistrationStatus (RegistrationStatus, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate)
	VALUES (N'New Application', CONVERT(BIT, 'False'), 0, 0, '2024-05-31 12:27:11.533', '2024-05-31 12:27:11.533')

INSERT INTO Modules (ModuleTitle, modpath, SortOrder, UserLevel, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate)
	VALUES (N'Criteria Management', N'criteria-management', 2, DEFAULT, 0, 0, 0, GETDATE(), GETDATE());
INSERT AppForms (FormName, Path, Icon, Controller, ModuleID, SortOrder, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate, IsAddible, IsEditable, IsDeletable, IsViewable)
	VALUES (N'Criteria Template', N'criteria-template', N'', N'CriteriaTemplate', (SELECT MAX(ModuleID) FROM Modules), 0, 0, 0, 0, GETDATE(), GETDATE(), 1, 1, 1, 1)

INSERT INTO Modules (ModuleTitle, modpath, SortOrder, UserLevel, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate)
	VALUES (N'Workflow Management', N'workflow-management', 2, DEFAULT, 0, 0, 0, GETDATE(), GETDATE());
INSERT AppForms (FormName, Path, Icon, Controller, ModuleID, SortOrder, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate, IsAddible, IsEditable, IsDeletable, IsViewable)
	VALUES (N'Workflow', N'workflow', N'', N'WorkFlow', (SELECT MAX(ModuleID) FROM Modules), 0, 0, 0, 0, GETDATE(), GETDATE(), 1, 1, 1, 1)



INSERT INTO Modules (ModuleTitle, modpath, SortOrder, UserLevel, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate)
	VALUES (N'Payment', N'payment', 2, DEFAULT, 0, 0, 0, GETDATE(), GETDATE());
INSERT AppForms (FormName, Path, Icon, Controller, ModuleID, SortOrder, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate, IsAddible, IsEditable, IsDeletable, IsViewable)
	VALUES (N'Registration Payment', N'registration-payment', N'', N'Payment', (SELECT MAX(ModuleID) FROM Modules), 0, 0, 0, 0, GETDATE(), GETDATE(), 1, 1, 1, 1)
INSERT AppForms (FormName, Path, Icon, Controller, ModuleID, SortOrder, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate, IsAddible, IsEditable, IsDeletable, IsViewable)
	VALUES (N'Association Payment', N'association-payment', N'', N'Payment', (SELECT MAX(ModuleID) FROM Modules), 0, 0, 0, 0, GETDATE(), GETDATE(), 1, 1, 1, 1)

INSERT INTO Modules (ModuleTitle, modpath, SortOrder, UserLevel, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate)
	VALUES (N'Process Configuration', N'process-configuration', 2, DEFAULT, 0, 0, 0, GETDATE(), GETDATE());
INSERT AppForms (FormName, Path, Icon, Controller, ModuleID, SortOrder, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate, IsAddible, IsEditable, IsDeletable, IsViewable)
	VALUES (N'Process Schedule', N'process-schedule', N'', N'ProcessConfiguration', (SELECT MAX(ModuleID) FROM Modules), 0, 0, 0, 0, GETDATE(), GETDATE(), 1, 1, 1, 1)



INSERT INTO Modules (ModuleTitle, modpath, SortOrder, UserLevel, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate)
	VALUES (N'TSP Trainee Portal', N'tsp-trainee-portal', 2, DEFAULT, 0, 0, 0, GETDATE(), GETDATE());
INSERT AppForms (FormName, Path, Icon, Controller, ModuleID, SortOrder, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate, IsAddible, IsEditable, IsDeletable, IsViewable)
	VALUES (N'TSP Trainee Portal', N'tsp-trainee-portal', N'', N'TSPTraineePortal', (SELECT MAX(ModuleID) FROM Modules), 0, 0, 0, 0, GETDATE(), GETDATE(), 1, 1, 1, 1)

INSERT SSPSalesTaxType (SalesTaxType, InActive, CreatedUserID, CreatedDate, ModifiedUserID, ModifiedDate)
	VALUES (N'GST', CONVERT(BIT, 'True'), 0, '2024-01-20 14:20:34.403', 0, NULL)
INSERT SSPSalesTaxType (SalesTaxType, InActive, CreatedUserID, CreatedDate, ModifiedUserID, ModifiedDate)
	VALUES (N'PRA', CONVERT(BIT, 'True'), 0, '2024-01-20 14:20:34.403', 0, NULL)
INSERT SSPSalesTaxType (SalesTaxType, InActive, CreatedUserID, CreatedDate, ModifiedUserID, ModifiedDate)
	VALUES (N'None', CONVERT(BIT, 'False'), 0, '2024-03-16 20:43:23.370', 0, '2024-03-16 20:43:23.370')

INSERT SSPLegalStatus (LegalStatusName, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate)
	VALUES ('NGO registered under The Trusts Act', CONVERT(BIT, 'False'), NULL, NULL, NULL, NULL)
INSERT SSPLegalStatus (LegalStatusName, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate)
	VALUES ('Sole Proprietor', CONVERT(BIT, 'False'), NULL, NULL, NULL, NULL)
INSERT SSPLegalStatus (LegalStatusName, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate)
	VALUES ('NGO registered with Social Welfare Department', CONVERT(BIT, 'False'), NULL, NULL, NULL, NULL)
INSERT SSPLegalStatus (LegalStatusName, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate)
	VALUES ('NGO registered under Societies Registration Act', CONVERT(BIT, 'False'), NULL, NULL, NULL, NULL)
INSERT SSPLegalStatus (LegalStatusName, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate)
	VALUES ('Partnership Firm', CONVERT(BIT, 'False'), NULL, NULL, NULL, NULL)
INSERT SSPLegalStatus (LegalStatusName, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate)
	VALUES ('Private Ltd. Company', CONVERT(BIT, 'False'), NULL, NULL, NULL, NULL)
INSERT SSPLegalStatus (LegalStatusName, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate)
	VALUES ('Public Ltd. Company', CONVERT(BIT, 'False'), NULL, NULL, NULL, NULL)
INSERT SSPLegalStatus (LegalStatusName, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate)
	VALUES ('other', CONVERT(BIT, 'False'), NULL, NULL, NULL, NULL)

INSERT AppForms (FormName, Path, Icon, Controller, ModuleID, SortOrder, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate, IsAddible, IsEditable, IsDeletable, IsViewable)
	VALUES (N'TSP Registration', N'registration-approval', N'', N'Approval', 5, 0, 0, 0, 0, GETDATE(), GETDATE(), 1, 1, 1, 1)

INSERT SSPBank (BankName, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate)
	VALUES ('National Bank of Pakistan', CONVERT(BIT, 'False'), NULL, NULL, '2023-06-15 21:57:47.043', NULL)
INSERT SSPBank (BankName, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate)
	VALUES ('The Bank of Punjab', CONVERT(BIT, 'False'), NULL, NULL, '2023-06-15 21:57:47.043', NULL)
INSERT SSPBank (BankName, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate)
	VALUES ('Habib Bank Limited', CONVERT(BIT, 'False'), NULL, NULL, '2023-06-15 21:57:47.043', NULL)
INSERT SSPBank (BankName, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate)
	VALUES ('United Bank Limited', CONVERT(BIT, 'False'), NULL, NULL, '2023-06-15 21:57:47.043', NULL)
INSERT SSPBank (BankName, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate)
	VALUES ('MCB Bank Limited', CONVERT(BIT, 'False'), NULL, NULL, '2023-06-15 21:57:47.043', NULL)
INSERT SSPBank (BankName, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate)
	VALUES ('Meezan Bank Limited', CONVERT(BIT, 'False'), NULL, NULL, '2023-06-15 21:57:47.043', NULL)
INSERT SSPBank (BankName, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate)
	VALUES ('Allied Bank Limited', CONVERT(BIT, 'False'), NULL, NULL, '2023-06-15 21:57:47.043', NULL)
INSERT SSPBank (BankName, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate)
	VALUES ('Bank Al-Habib Limited', CONVERT(BIT, 'False'), NULL, NULL, '2023-06-15 21:57:47.043', NULL)
INSERT SSPBank (BankName, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate)
	VALUES ('Bank Al-Falah Limited', CONVERT(BIT, 'False'), NULL, NULL, '2023-06-15 21:57:47.043', NULL)
INSERT SSPBank (BankName, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate)
	VALUES ('Faysal Bank Limited', CONVERT(BIT, 'False'), NULL, NULL, '2023-06-15 21:57:47.043', NULL)
INSERT SSPBank (BankName, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate)
	VALUES ('Standard Chartered Bank (Pakistan) Limited', CONVERT(BIT, 'False'), NULL, NULL, '2023-06-15 21:57:47.043', NULL)
INSERT SSPBank (BankName, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate)
	VALUES ('Askari Bank Limited', CONVERT(BIT, 'False'), NULL, NULL, '2023-06-15 21:57:47.043', NULL)
INSERT SSPBank (BankName, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate)
	VALUES ('Al-Baraka Bank Limited', CONVERT(BIT, 'False'), NULL, NULL, '2023-06-15 21:57:47.043', NULL)
INSERT SSPBank (BankName, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate)
	VALUES ('MCB Islamic Bank Limited', CONVERT(BIT, 'False'), NULL, NULL, '2023-06-15 21:57:47.043', NULL)
INSERT SSPBank (BankName, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate)
	VALUES ('Habib Metropolitan Bank Limited', CONVERT(BIT, 'False'), NULL, NULL, '2023-06-15 21:57:47.043', NULL)
INSERT SSPBank (BankName, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate)
	VALUES ('SilkBank Limited', CONVERT(BIT, 'False'), NULL, NULL, '2023-06-15 21:57:47.043', NULL)
INSERT SSPBank (BankName, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate)
	VALUES ('Summit Bank Limited', CONVERT(BIT, 'False'), NULL, NULL, '2023-06-15 21:57:47.043', NULL)
INSERT SSPBank (BankName, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate)
	VALUES ('Soneri Bank Limited', CONVERT(BIT, 'False'), NULL, NULL, '2023-06-15 21:57:47.043', NULL)
INSERT SSPBank (BankName, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate)
	VALUES ('Other', CONVERT(BIT, 'False'), NULL, NULL, '2023-06-15 21:57:47.043', NULL)
INSERT SSPBank (BankName, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate)
	VALUES ('National Bank of Pakistan', CONVERT(BIT, 'False'), NULL, NULL, '2023-06-15 21:57:47.043', NULL)
INSERT SSPBank (BankName, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate)
	VALUES ('The Bank of Punjab', CONVERT(BIT, 'False'), NULL, NULL, '2023-06-15 21:57:47.043', NULL)
INSERT SSPBank (BankName, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate)
	VALUES ('Habib Bank Limited', CONVERT(BIT, 'False'), NULL, NULL, '2023-06-15 21:57:47.043', NULL)
INSERT SSPBank (BankName, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate)
	VALUES ('United Bank Limited', CONVERT(BIT, 'False'), NULL, NULL, '2023-06-15 21:57:47.043', NULL)
INSERT SSPBank (BankName, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate)
	VALUES ('MCB Bank Limited', CONVERT(BIT, 'False'), NULL, NULL, '2023-06-15 21:57:47.043', NULL)
INSERT SSPBank (BankName, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate)
	VALUES ('Meezan Bank Limited', CONVERT(BIT, 'False'), NULL, NULL, '2023-06-15 21:57:47.043', NULL)
INSERT SSPBank (BankName, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate)
	VALUES ('Allied Bank Limited', CONVERT(BIT, 'False'), NULL, NULL, '2023-06-15 21:57:47.043', NULL)
INSERT SSPBank (BankName, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate)
	VALUES ('Bank Al-Habib Limited', CONVERT(BIT, 'False'), NULL, NULL, '2023-06-15 21:57:47.043', NULL)
INSERT SSPBank (BankName, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate)
	VALUES ('Bank Al-Falah Limited', CONVERT(BIT, 'False'), NULL, NULL, '2023-06-15 21:57:47.043', NULL)
INSERT SSPBank (BankName, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate)
	VALUES ('Faysal Bank Limited', CONVERT(BIT, 'False'), NULL, NULL, '2023-06-15 21:57:47.043', NULL)
INSERT SSPBank (BankName, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate)
	VALUES ('Standard Chartered Bank (Pakistan) Limited', CONVERT(BIT, 'False'), NULL, NULL, '2023-06-15 21:57:47.043', NULL)
INSERT SSPBank (BankName, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate)
	VALUES ('Askari Bank Limited', CONVERT(BIT, 'False'), NULL, NULL, '2023-06-15 21:57:47.043', NULL)
INSERT SSPBank (BankName, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate)
	VALUES ('Al-Baraka Bank Limited', CONVERT(BIT, 'False'), NULL, NULL, '2023-06-15 21:57:47.043', NULL)
INSERT SSPBank (BankName, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate)
	VALUES ('MCB Islamic Bank Limited', CONVERT(BIT, 'False'), NULL, NULL, '2023-06-15 21:57:47.043', NULL)
INSERT SSPBank (BankName, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate)
	VALUES ('Habib Metropolitan Bank Limited', CONVERT(BIT, 'False'), NULL, NULL, '2023-06-15 21:57:47.043', NULL)
INSERT SSPBank (BankName, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate)
	VALUES ('SilkBank Limited', CONVERT(BIT, 'False'), NULL, NULL, '2023-06-15 21:57:47.043', NULL)
INSERT SSPBank (BankName, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate)
	VALUES ('Summit Bank Limited', CONVERT(BIT, 'False'), NULL, NULL, '2023-06-15 21:57:47.043', NULL)
INSERT SSPBank (BankName, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate)
	VALUES ('Soneri Bank Limited', CONVERT(BIT, 'False'), NULL, NULL, '2023-06-15 21:57:47.043', NULL)
INSERT SSPBank (BankName, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate)
	VALUES ('Other', CONVERT(BIT, 'False'), NULL, NULL, '2023-06-15 21:57:47.043', NULL)

INSERT AppForms (FormName, Path, Icon, Controller, ModuleID, SortOrder, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate, IsAddible, IsEditable, IsDeletable, IsViewable)
	VALUES (N'Annual Plan', N'annual-plan-approval', N'', N'Approval', 5, 0, 0, 0, 0, GETDATE(), GETDATE(), 1, 1, 1, 1)
INSERT INTO ApprovalProcess (ProcessKey, ApprovalProcessName, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate, HasAutoApproval, DisplayName)
	VALUES (N'PROG_APP', N'Program Approved', DEFAULT, 0, DEFAULT, GETDATE(), GETDATE(), DEFAULT, N'Program Approved');

INSERT SSPTSPRegistrationFee (RegistrationFee, Inactive, CreatedUserID, CreatedDate, ModifiedUserID, ModifiedDate)
	VALUES (5000.00, CONVERT(BIT, 'False'), 0, '2024-04-17 12:32:35.677', 0, '2024-04-17 12:32:35.677')
INSERT SSPTSPAssociationFee (AssociationFee, Inactive, CreatedUserID, CreatedDate, ModifiedUserID, ModifiedDate)
	VALUES (2000.00, CONVERT(BIT, 'False'), 0, '2024-04-17 12:32:35.680', 0, '2024-04-17 12:32:35.680')

INSERT Roles (RoleTitle, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate)
	VALUES (N'SSPRegistration', CONVERT(BIT, 'False'), 0, 0, '2024-04-25 11:47:12.953', '2024-04-25 11:47:12.953')


INSERT SSPPlaningType (PlaningType, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate)
	VALUES (N'SSP', CONVERT(BIT, 'False'), NULL, NULL, NULL, NULL)
INSERT SSPPlaningType (PlaningType, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate)
	VALUES (N'Procurement', CONVERT(BIT, 'False'), NULL, NULL, NULL, NULL)
INSERT SSPPlaningType (PlaningType, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate)
	VALUES (N'Other', CONVERT(BIT, 'False'), NULL, NULL, NULL, NULL)



INSERT SSPTraineeSupportItems (Applicability, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate)
	VALUES (N'Bags', CONVERT(BIT, 'False'), NULL, NULL, NULL, NULL)
INSERT SSPTraineeSupportItems (Applicability, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate)
	VALUES (N'Bagde', CONVERT(BIT, 'False'), NULL, NULL, NULL, NULL)
INSERT SSPTraineeSupportItems (Applicability, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate)
	VALUES (N'Uniform', CONVERT(BIT, 'False'), NULL, NULL, NULL, NULL)

INSERT ProgramType (PTypeName, AMSName, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate)
	VALUES (N'Skills Scholarship Program', N'Skills Scholarship Program', CONVERT(BIT, 'False'), 2, 0, '2024-05-07 11:29:48.533', NULL)

INSERT SSPSelectionMethods (MethodName, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate)
	VALUES (N'Quality & Cost', CONVERT(BIT, 'False'), NULL, NULL, NULL, NULL)
INSERT SSPSelectionMethods (MethodName, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate)
	VALUES (N'Least Cost', CONVERT(BIT, 'False'), NULL, NULL, NULL, NULL)
INSERT SSPSelectionMethods (MethodName, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate)
	VALUES (N'Association', CONVERT(BIT, 'False'), NULL, NULL, NULL, NULL)

INSERT Approval (ProcessKey, Step, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate, UserIDs, IsAutoApproval)
	VALUES (N'PROG_APP', 1, CONVERT(BIT, 'False'), 80, 0, '2024-04-25 16:15:11.577', NULL, N'81', CONVERT(BIT, 'False'))
INSERT Approval (ProcessKey, Step, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate, UserIDs, IsAutoApproval)
	VALUES (N'PROG_APP', 2, CONVERT(BIT, 'False'), 80, 0, '2024-04-25 16:15:11.577', NULL, N'670', CONVERT(BIT, 'False'))
INSERT Approval (ProcessKey, Step, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate, UserIDs, IsAutoApproval)
	VALUES (N'PROG_APP', 3, CONVERT(BIT, 'False'), 80, 0, '2024-04-25 16:15:11.577', NULL, N'396', CONVERT(BIT, 'False'))

INSERT INTO AppForms (FormName, Path, Icon, Controller, ModuleID, Sortorder, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate, IsAddible, IsEditable, IsDeletable, IsViewable)
	VALUES (N'Criteria Template', N'criteria-template-approval', N'', N'Approval', 5, 0, DEFAULT, 0, DEFAULT, GETDATE(), GETDATE(),1,1,1,1);

INSERT INTO  ApprovalProcess (ProcessKey, ApprovalProcessName, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate, HasAutoApproval, DisplayName)
	VALUES (N'CRTEM_APP', N'Criteria Template', DEFAULT, 0, DEFAULT, GETDATE(), GETDATE(), DEFAULT, N'Criteria Template Approval');


INSERT SSPApprovalStatus(Status, InActive, CreatedUserID, CreatedDate) VALUES (N'Submitted', CONVERT(bit, 'False'), NULL, NULL)
INSERT SSPApprovalStatus(Status, InActive, CreatedUserID, CreatedDate) VALUES (N'Pending', CONVERT(bit, 'False'), NULL, NULL)
INSERT SSPApprovalStatus(Status, InActive, CreatedUserID, CreatedDate) VALUES (N'In-Progress', CONVERT(bit, 'False'), NULL, NULL)
INSERT SSPApprovalStatus(Status, InActive, CreatedUserID, CreatedDate) VALUES (N'Accepted', CONVERT(bit, 'False'), NULL, NULL)
INSERT SSPApprovalStatus(Status, InActive, CreatedUserID, CreatedDate) VALUES (N'Rejected', CONVERT(bit, 'False'), NULL, NULL)
INSERT SSPApprovalStatus(Status, InActive, CreatedUserID, CreatedDate) VALUES (N'On-Hold', CONVERT(bit, 'False'), NULL, NULL)
INSERT SSPApprovalStatus(Status, InActive, CreatedUserID, CreatedDate) VALUES (N'Send-Back', CONVERT(bit, 'False'), 0, '2024-02-20 15:33:14.863')

INSERT SSPProcessSchedule(ProcessName, ProcessStartDate, ProcessEndDate, DurationDays, ProgramName, programStartDate, Inactive, CreatedUserID, CreatedDate, ModifiedUserID, ModifiedDate) VALUES ('Registration of TSP', '2024-04-01 00:00:00.000', '2024-04-16 00:00:00.000', 15, NULL, NULL, CONVERT(bit, 'False'), NULL, '2024-04-16 12:29:04.293', NULL, NULL)
INSERT SSPProcessSchedule(ProcessName, ProcessStartDate, ProcessEndDate, DurationDays, ProgramName, programStartDate, Inactive, CreatedUserID, CreatedDate, ModifiedUserID, ModifiedDate) VALUES ('Technical evaluation of registered TSPs', '2024-04-17 00:00:00.000', '2024-05-08 00:00:00.000', 21, NULL, NULL, CONVERT(bit, 'False'), NULL, '2024-04-16 12:29:04.293', NULL, NULL)
INSERT SSPProcessSchedule(ProcessName, ProcessStartDate, ProcessEndDate, DurationDays, ProgramName, programStartDate, Inactive, CreatedUserID, CreatedDate, ModifiedUserID, ModifiedDate) VALUES ('Data analysis and scheme design', '2024-05-09 00:00:00.000', '2024-05-11 00:00:00.000', 2, NULL, NULL, CONVERT(bit, 'False'), NULL, '2024-04-16 12:29:04.293', NULL, NULL)
INSERT SSPProcessSchedule(ProcessName, ProcessStartDate, ProcessEndDate, DurationDays, ProgramName, programStartDate, Inactive, CreatedUserID, CreatedDate, ModifiedUserID, ModifiedDate) VALUES ('Announcement of Schemes', '2024-05-12 00:00:00.000', '2024-05-13 00:00:00.000', 1, NULL, NULL, CONVERT(bit, 'False'), NULL, '2024-04-16 12:29:04.293', NULL, NULL)
INSERT SSPProcessSchedule(ProcessName, ProcessStartDate, ProcessEndDate, DurationDays, ProgramName, programStartDate, Inactive, CreatedUserID, CreatedDate, ModifiedUserID, ModifiedDate) VALUES ('Association submission by TSPs', '2024-05-14 00:00:00.000', '2024-05-17 00:00:00.000', 3, NULL, NULL, CONVERT(bit, 'False'), NULL, '2024-04-16 12:29:04.293', NULL, NULL)
INSERT SSPProcessSchedule(ProcessName, ProcessStartDate, ProcessEndDate, DurationDays, ProgramName, programStartDate, Inactive, CreatedUserID, CreatedDate, ModifiedUserID, ModifiedDate) VALUES ('Evaluation of TSPs & Announcement of associated TSPs', '2024-05-18 00:00:00.000', '2024-05-19 00:00:00.000', 1, NULL, NULL, CONVERT(bit, 'False'), NULL, '2024-04-16 12:29:04.293', NULL, NULL)
INSERT SSPProcessSchedule(ProcessName, ProcessStartDate, ProcessEndDate, DurationDays, ProgramName, programStartDate, Inactive, CreatedUserID, CreatedDate, ModifiedUserID, ModifiedDate) VALUES ('Uploading of Scheme Master Data on Trainee Portal', '2024-05-20 00:00:00.000', '2024-05-21 00:00:00.000', 1, NULL, NULL, CONVERT(bit, 'False'), NULL, '2024-04-16 12:29:04.293', NULL, NULL)
INSERT SSPProcessSchedule(ProcessName, ProcessStartDate, ProcessEndDate, DurationDays, ProgramName, programStartDate, Inactive, CreatedUserID, CreatedDate, ModifiedUserID, ModifiedDate) VALUES ('Registration of Trainee', '2024-05-22 00:00:00.000', '2024-06-06 00:00:00.000', 15, NULL, NULL, CONVERT(bit, 'False'), NULL, '2024-04-16 12:29:04.293', NULL, NULL)
INSERT SSPProcessSchedule(ProcessName, ProcessStartDate, ProcessEndDate, DurationDays, ProgramName, programStartDate, Inactive, CreatedUserID, CreatedDate, ModifiedUserID, ModifiedDate) VALUES ('Verification by TSP (Registered Trainee,Trainee Interview,Final list of Selected Trainees) and (NADRA Verisys of the Selected Trainees)', '2024-06-07 00:00:00.000', '2024-06-17 00:00:00.000', 10, NULL, NULL, CONVERT(bit, 'False'), NULL, '2024-04-16 12:29:04.293', NULL, NULL)
INSERT SSPProcessSchedule(ProcessName, ProcessStartDate, ProcessEndDate, DurationDays, ProgramName, programStartDate, Inactive, CreatedUserID, CreatedDate, ModifiedUserID, ModifiedDate) VALUES ('BSS & SAP Data Integration with (Class Creation, POs, Selected Trainees) , Information to TSPs about final classes and Trainees and Inception Report Generation', '2024-06-18 00:00:00.000', '2024-06-23 00:00:00.000', 5, NULL, NULL, CONVERT(bit, 'False'), NULL, '2024-04-16 12:29:04.293', NULL, NULL)

	
