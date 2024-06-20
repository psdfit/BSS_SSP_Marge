-- Record N1
CREATE OR ALTER PROCEDURE dbo.RD_SSPTSPProfile @UserID INT = 0
AS
BEGIN
	SELECT
		ttp.UserID
	   ,ttp.TspID
	   ,ttp.TspEmail
	   ,ttp.TspContact
	   ,ttp.BusinessName AS InstituteName
	   ,ttp.RegistrationDate AS RegistrationDate
	   ,ttp.NTN AS InstituteNTN
	   ,ttp.NTNEvidence AS NTNAttachment
	   ,ttp.NTNEvidence
	   ,ttp.SalesTaxType AS TaxTypeID
	   ,ttp.GST AS GSTNumber
	   ,ttp.GSTEvidence AS GSTAttachment
	   ,ttp.PRA AS PRANumber
	   ,ttp.GSTEvidence
	   ,ttp.PRAEvidence AS PRAAttachment
	   ,ttp.PRAEvidence
	   ,ttp.LegalStatusID AS LegalStatusID
	   ,ls.LegalStatusName AS LegalStatus
	   ,ttp.LegalStatusEvidence AS LegalStatusAttachment
	   ,ttp.LegalStatusEvidence
	   ,ttp.Website
	   ,p.ProvinceID
	   ,p.ProvinceName
	   ,c.ClusterID
	   ,c.ClusterName
	   ,t.TehsilID
	   ,t.TehsilName
	   ,d.DistrictID
	   ,d.DistrictName
	   ,ttp.GeoTagging
	   ,ttp.Address
	   ,ISNULL(ttp.HeadName, '') AS HeadName
	   ,ttp.HeadCnicNum
	   ,ttp.HeadCnicFrontImg
	   ,ttp.HeadCnicBackImg
	   ,ttp.HeadCnicFrontImg AS OrgHeadCNICFrontImgUrl
	   ,ttp.HeadCnicBackImg AS OrgHeadCNICBackImgUrl
	   ,ttp.HeadDesignation
	   ,ttp.TspEmail AS HeadEmail
	   ,ttp.HeadMobile
	   ,ttp.OrgLandline
	   ,ttp.POCName
	   ,ttp.POCDesignation
	   ,ttp.POCEmail
	   ,ttp.POCMobile
	   ,FinalSubmitted
	   ,ttp.ProgramTypeID
	FROM SSPTSPProfile ttp
	LEFT JOIN ProgramType pt
		ON ttp.ProgramTypeID = pt.PTypeID
	LEFT JOIN SSPLegalStatus ls
		ON ls.LegalStatusID = ttp.LegalStatusID
	LEFT JOIN Province p
		ON ttp.ProvinceID = p.ProvinceID
	LEFT JOIN Cluster c
		ON ttp.ClusterID = c.ClusterID
	LEFT JOIN District d
		ON ttp.DistrictID = d.DistrictID
	LEFT JOIN Tehsil t
		ON ttp.TehsilID = t.TehsilID
	WHERE (
	@UserID = 0
	OR ttp.UserID = @UserID
	)
END
GO
ALTER PROCEDURE [dbo].[RD_CustomFinancialYears] @Id INT = 0,
@FromDate DATETIME = '',
@ToDate DATETIME = '',
@OrgID INT = 0,
@InActive BIT = NULL,
@CreatedUserID INT = 0,
@ModifiedUserID INT = 0,
@CreatedDate DATE = '',
@ModifiedDate DATE = ''
AS
BEGIN
	SELECT
		M.*
	   ,Organization.OName
	   ,CONCAT(
		CONVERT(DATE, FromDate),
		' - ',
		CONVERT(DATE, ToDate)
		) AS FinancialYearName
	FROM CustomFinancialYears M
	INNER JOIN Organization
		ON Organization.OID = M.OrgID
	WHERE (
	@Id = 0
	OR M.Id = @Id
	)
	AND (
	@FromDate = ''
	OR M.FromDate = @FromDate
	)
	AND (
	@ToDate = ''
	OR M.ToDate = @ToDate
	)
	AND (
	@OrgID = 0
	OR M.OrgID = @OrgID
	)
	AND (
	@InActive IS NULL
	OR M.InActive = @InActive
	)
	AND (
	@CreatedUserID = 0
	OR M.CreatedUserID = @CreatedUserID
	)
	AND (
	@ModifiedUserID = 0
	OR M.ModifiedUserID = @ModifiedUserID
	)
	AND (
	@CreatedDate = ''
	OR M.CreatedDate = @CreatedDate
	)
	AND (
	@ModifiedDate = ''
	OR M.ModifiedDate = @ModifiedDate
	)
END
GO
CREATE OR ALTER PROCEDURE [dbo].[RD_SSPProgramDesignForTradeLot]
AS
BEGIN
	SELECT
		pd.ProgramID
	   ,pd.ProgramName
	   ,pd.Stipend
	   ,ISNULL(pd.bagBadgeCost, 0) AS bagBadgeCost
	   ,pd.SchemeDesignOn
	   ,pd.Province
	   ,pd.Cluster
	   ,pd.District
	   ,CASE
			WHEN pd.IsInitiate IS NULL OR
				pd.IsInitiate = 0 THEN 0
			ELSE 1
		END IsInitiate
	   ,pd.IsInitiate
	   ,IsInitiate
	   ,pd.GenderID
	   ,g.GenderName AS Gender
	FROM SSPProgramDesign pd
	LEFT JOIN Gender g
		ON pd.GenderID = g.GenderID
	WHERE pd.InActive = 0
	AND pd.IsSubmitted = 1
END -- Record N3
GO
CREATE OR ALTER PROCEDURE [dbo].[RD_SSPTradeLayerForTradeLot]
AS
BEGIN
	SELECT
		td.TradeDetailMapID
	   ,t.TradeName
	   ,t.TradeID
	   ,s.SectorName
	   ,ss.SubSectorName
	   ,ca.CertAuthName
	   ,soc.Name AS SourceOfCurriculum
	   ,et.Education AS traineeEducation
	   ,d.Duration
	FROM Trade t
	INNER JOIN Sector s
		ON t.SectorID = s.SectorID
	INNER JOIN SubSector ss
		ON t.SubSectorID = ss.SubSectorID
	INNER JOIN TradeDetailMap td
		ON t.TradeID = td.TradeID
	INNER JOIN CertificationAuthority ca
		ON td.CertAuthID = ca.CertAuthID
	INNER JOIN EducationTypes et
		ON et.EducationTypeID = td.TraineeEducationTypeID
	INNER JOIN Duration d
		ON d.DurationID = td.DurationID
	LEFT JOIN SourceOfCurriculum soc
		ON td.SourceOfCurriculumID = soc.SourceOfCurriculumID
	WHERE t.InActive = 0
	AND t.SAPID IS NOT NULL
	AND t.TradeCode IS NOT NULL
END -- Record N3
GO
CREATE OR ALTER PROCEDURE [dbo].[SSPGetUsersRightsByUser] @UserID INT = 0
AS
BEGIN
	SELECT
		(SELECT
				tu.Username
			FROM SSPUsers tu
			WHERE tu.UserID = @UserID
			AND tu.InActive = 0)
		AS Username
	   ,Mo.ModuleTitle
	   ,Mo.modpath
	   ,Mo.SortOrder AS ModSortOrder
	   ,A.*
	   ,@UserID AS UserID
	   ,0 AS UserRightID
	   ,1 AS CanAdd
	   ,1 AS CanEdit
	   ,1 AS CanDelete
	   ,1 AS CanView
	   ,1 AS InActive
	FROM AppForms A
	LEFT JOIN Modules Mo
		ON Mo.ModuleID = A.ModuleID
	WHERE A.InActive = 0
	AND Mo.ModuleTitle IN (
	'Profile Management',
	'Association Management',
	'TSP Trainee Portal'
	)
	AND A.FormName IN (
	'Profile',
	'Base Data',
	'Association Submission',
	'TSP Trainee Portal'
	)
END -- Record N5
GO
CREATE OR ALTER PROCEDURE [dbo].[RD_SSPCheckTSPEmail] @TSPEmail NVARCHAR(MAX) = ''
AS
BEGIN
	SELECT
		(1)
	FROM TSPMaster t
	INNER JOIN Users u
		ON t.UserID = u.UserID
	WHERE u.Email = @TSPEmail
	UNION ALL
	SELECT
		(1) AS TSPCount
	FROM SSPUsers tu
	WHERE tu.Email = @TSPEmail
END -- Record N5
GO

CREATE OR ALTER PROCEDURE [dbo].[RD_SSPWorkflow]
AS
BEGIN
	SELECT
		w.WorkflowID
	   ,w.WorkflowTitle
	   ,(SELECT
				COUNT(1)
			FROM SSPWorkflowTask wt
			WHERE wt.WorkflowID = w.WorkflowID)
		AS TotalTask
	   ,w.SourcingTypeID
	   ,w.Description
	   ,w.TotalDays
	   ,w.TotalTaskDays
	   ,pt.PlaningType AS SourcingType
	FROM SSPWorkflow w
	LEFT JOIN SSPPlaningType pt
		ON w.SourcingTypeID = pt.PlaningTypeID
END -- Record N6
GO
CREATE OR ALTER PROCEDURE RD_SSPProcessScheduleDetail
AS
	SELECT
		CASE
			WHEN psd.InActive = 0 AND
				0 = 0 THEN '0'
			ELSE '1'
		END AS IsLocked
	   ,psd.*
	FROM SSPProcessScheduleDetail psd
	INNER JOIN SSPProcessSchedule ps
		ON ps.ProcessID = psd.ProcessID
	ORDER BY ps.ProcessID -- Record N8
GO
CREATE OR ALTER PROCEDURE [dbo].[RD_SSPTradeForTradeLot]
AS
BEGIN
	SELECT
		t.TradeName
	   ,t.TradeID
	   ,s.SectorName
	   ,ss.SubSectorName
	FROM Trade t
	INNER JOIN Sector s
		ON t.SectorID = s.SectorID
	INNER JOIN SubSector ss
		ON t.SubSectorID = ss.SubSectorID
	WHERE t.Inactive = 0
	AND t.SAPID IS NOT NULL
	AND t.TradeCode IS NOT NULL
END -- Record N9
GO
CREATE OR ALTER PROCEDURE [dbo].[RD_SSPUsers] @UserID INT = 0,
@UserName NVARCHAR(60) = '',
@Fname NVARCHAR(50) = '',
@lname NVARCHAR(50) = '',
@Email NVARCHAR(50) = '',
@RoleID INT = 0,
@UserLevel INT = 0,
@UserImage NVARCHAR(MAX) = '',
@LoginDT DATETIME = '',
@InActive BIT = NULL,
@CreatedUserID INT = 0,
@ModifiedUserID INT = 0,
@CreatedDate DATE = '',
@ModifiedDate DATE = ''
AS
BEGIN
	SELECT
		*
	   ,UP.UserPassword
	   ,Roles.RoleTitle
	FROM SSPUsers M
	INNER JOIN Roles
		ON Roles.RoleID = M.RoleID
	CROSS APPLY (SELECT TOP 1
			UserPassword
		FROM SSPUsersPwd
		WHERE UserID = M.UserID
		ORDER BY pwdID DESC) AS UP
	WHERE (
	@UserID = 0
	OR M.UserID = @UserID
	)
	AND (
	@UserName = ''
	OR M.UserName = @UserName
	)
	AND (
	@Fname = ''
	OR M.Fname = @Fname
	)
	AND (
	@lname = ''
	OR M.lname = @lname
	)
	AND (
	@Email = ''
	OR M.Email = @Email
	)
	AND (
	@RoleID = 0
	OR M.RoleID = @RoleID
	)
	AND (
	@UserImage = ''
	OR M.UserImage = @UserImage
	)
	AND (
	@UserLevel = 0
	OR M.UserLevel = @UserLevel
	)
	AND (
	@LoginDT = ''
	OR M.LoginDT = @LoginDT
	)
	AND (
	@InActive IS NULL
	OR M.InActive = @InActive
	)
	AND (
	@CreatedUserID = 0
	OR M.CreatedUserID = @CreatedUserID
	)
	AND (
	@ModifiedUserID = 0
	OR M.ModifiedUserID = @ModifiedUserID
	)
	AND (
	@CreatedDate = ''
	OR M.CreatedDate = @CreatedDate
	)
	AND (
	@ModifiedDate = ''
	OR M.ModifiedDate = @ModifiedDate
	)
END -- Record N10
GO
CREATE OR ALTER PROCEDURE [dbo].[RD_SSPProgramDistrict] @ProgramID INT = 0,
@UserID INT = 0
AS
BEGIN
	SELECT
		pd.ProgramID
	   ,pd.ProgramName
	   ,d.DistrictID
	   ,d.DistrictName
	FROM SSPTSPAssignment t
	INNER JOIN SSPProgramDesign pd
		ON pd.ProgramID = t.ProgramID
	INNER JOIN SSPTSPTrainingLocation ttl
		ON ttl.TrainingLocationID = t.TrainingLocationID
	INNER JOIN District d
		ON d.DistrictID = ttl.District
	WHERE pd.IsInitiate = 1
	AND pd.IsFinalApproved = 1
	AND pd.InActive = 0
	AND t.ProgramID = @ProgramID
	AND t.TSPID = @UserID
	GROUP BY pd.ProgramName
			,pd.ProgramID
			,d.DistrictID
			,d.DistrictName
	ORDER BY pd.ProgramID DESC
END -- Record N11
GO
CREATE OR ALTER PROCEDURE [dbo].[SSPLogin] @UserName NVARCHAR(50),
@Userpassword NVARCHAR(50),
@EMEI NVARCHAR(50) = '',
@SessionId NVARCHAR(50) = '',
@IPAddress NVARCHAR(50) = ''
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @CurrentState INT = NULL;
	DECLARE @UserID INT = 0
		   ,@AllowedSessions TINYINT = 131;
	SELECT
		@UserID = u.UserID
	FROM Users u
	WHERE LOWER(u.UserName) = LOWER(TRIM(@UserName))
	SELECT
		@CurrentState = COUNT(*)
	FROM SessionCount AS SC
	WHERE SC.UserID = @UserID
	GROUP BY SC.UserID
	SELECT
		@AllowedSessions = 100
	SELECT
		u.*
	   ,UP.UserPassword
	   ,ISNULL(@CurrentState, 0) AS CurrentState
	   ,@AllowedSessions AS AllowedSessions
	   ,@SessionId AS SessionId
	   ,Roles.RoleTitle
	FROM dbo.SSPUsers u
	LEFT JOIN dbo.Roles
		ON Roles.RoleID = u.RoleID
	LEFT JOIN SSPUsersPwd pwd
		ON u.UserID = pwd.UserID
			AND pwd.InActive = 0
	CROSS APPLY (SELECT TOP 1
			UserPassword
		FROM SSPUsersPwd sp
		WHERE UserID = u.UserID
		ORDER BY pwdID DESC) AS UP
	WHERE u.InActive = 0
	AND Roles.InActive = 0
	AND LOWER(u.UserName) = LOWER(TRIM(@UserName))
	AND (pwd.UserPassword = @Userpassword)
END -- Record N16  
GO
CREATE OR ALTER PROCEDURE [dbo].[AU_SSPProgramApproveReject] @ProgramID INT,
@CurUserID INT,
@IsApproved BIT,
@IsRejected BIT
AS
BEGIN
	UPDATE SSPProgramDesign
	SET IsInitiate =
		CASE
			WHEN @IsRejected = 1 THEN 0
			ELSE IsInitiate
		END
	   ,IsApproved = @IsApproved
	   ,IsRejected = @IsRejected
	   ,ModifiedUserID = @CurUserID
	   ,ModifiedDate = GETDATE()
	WHERE ProgramID = @ProgramID;
END -- Record N14
GO

CREATE OR ALTER PROCEDURE [dbo].[AU_SSPTraineeInterestProfile] @TraineeID INT,
@UserID INT = NULL
AS
BEGIN
	BEGIN TRANSACTION;
	INSERT INTO SSPTraineeInterestProfile (TraineeRegistrationID,
	TraineeName,
	TraineeCNIC,
	FatherName,
	GenderID,
	TradeID,
	TradeLotID,
	DistrictID,
	SchemeID,
	DateOfBirth,
	TspID,
	EducationID,
	ContactNumber1,
	TraineeAge,
	ReligionID,
	TraineeDistrictID,
	TraineeDisabilityID,
	TraineeEmail,
	CreatedDate,
	CreatedUserID,
	InActive,
	IsSubmitted)
		SELECT
			t.CandidateRegistrationID
		   ,c.FullName
		   ,c.CNIC
		   ,c.FatherName
		   ,c.GenderID
		   ,t.TradeID
		   ,t.TradeLotID
		   ,t.DistrictID
		   ,t.ProgramID
		   ,c.DateOfBirth
		   ,t.TspID
		   ,c.EducationTypeID
		   ,c.MobileNumber
		   ,DATEDIFF(YEAR, c.DateOfBirth, GETDATE()) AS TraineeAge
		   ,
			--Calculate age
			c.ReligionID
		   ,c.DistrictOfResidenceID
		   ,c.DisabilityID
		   ,c.Email
		   ,GETDATE() AS CreatedDate
		   ,@UserID
		   ,0 AS InActive
		   ,1 AS IsSubmitted
		FROM SkillsScholarshipProgram.dbo.TraineeInterest t
		INNER JOIN SkillsScholarshipProgram.dbo.CandidateDetails c
			ON c.CandidateRegistrationID = t.CandidateRegistrationID
		WHERE c.CandidateRegistrationID = @TraineeID
	UPDATE SkillsScholarshipProgram.dbo.TraineeInterest
	SET IsSubmitted = 1
	   ,UpdatedOn = GETDATE()
	WHERE CandidateRegistrationID = @TraineeID;
	COMMIT TRANSACTION;
--Commit the transaction
END -- Record N17
GO
CREATE OR ALTER PROCEDURE RD_SSPApplicability @InActive BIT = NULL
AS
BEGIN
	SELECT
		*
	FROM SSPApplicability
	WHERE InActive = @InActive
END -- Record N16
GO
CREATE
OR ALTER PROCEDURE RD_SSPLotWiseTarget
AS
	SELECT
		tl.TradeLotID
	   ,tl.TradeDesignID
	   ,pd.ProgramID
	   ,pd.ProgramName AS Program
	   ,CASE
			WHEN td.ProgramDesignOn = 'Province' THEN CONCAT(
				'(',
				(d1.Duration),
				' month | ' + soc.Name + ' | ' + p.ProvinceName + ') ',
				t.TradeName
				)
			WHEN td.ProgramDesignOn = 'Cluster' THEN CONCAT(
				'(',
				(d1.Duration),
				' month | ' + soc.Name + ' | ' + c.ClusterName + ') ',
				t.TradeName
				)
			WHEN td.ProgramDesignOn = 'District' THEN CONCAT(
				'(',
				(d1.Duration),
				' month | ' + soc.Name + ' | ' + d.DistrictName + ') ',
				t.TradeName
				)
		END TradeLot
	   ,td.ProgramDesignOn
	   ,tl.TraineeSelectedContTarget AS ContractedTarget
	   ,tl.Duration
	   ,FORMAT(tl.CTM, 'N2') AS CTM
	   ,FORMAT(tl.TrainingCost, 'N2') AS TrainingCost
	   ,FORMAT(tl.Stipend, 'N2') AS Stipend
	   ,FORMAT(tl.BagAndBadge, 'N2') AS [Bag&Badge]
	   ,FORMAT(tl.ExamCost, 'N2') AS ExamCost
	   ,FORMAT(
		tl.TrainingCost + tl.Stipend + tl.BagAndBadge + tl.ExamCost,
		'N2'
		) AS TotalCost
	   ,td.ProgramDesignID
	FROM SSPTradeLot tl
	LEFT JOIN Trade t
		ON tl.TradeID = t.TradeID
	LEFT JOIN Province p
		ON tl.ProvinceID = p.ProvinceID
	LEFT JOIN Cluster c
		ON tl.ClusterID = c.ClusterID
	LEFT JOIN District d
		ON d.DistrictID = tl.DistrictID
	LEFT JOIN SSPTradeDesign td
		ON tl.TradeDesignID = td.TradeDesignID
	LEFT JOIN SSPProgramDesign pd
		ON pd.ProgramID = td.ProgramDesignID
	INNER JOIN TradeDetailMap tdm
		ON tdm.TradeDetailMapID = td.TradeDetailMapID
	INNER JOIN Duration d1
		ON d1.DurationID = tdm.DurationID
	INNER JOIN SourceOfCurriculum soc
		ON tdm.SourceOfCurriculumID = soc.SourceOfCurriculumID
	INNER JOIN Gender g
		ON g.GenderID = td.GenderID
	WHERE tl.Inactive = 0
	AND td.Inactive = 0
	AND pd.Inactive = 0
	ORDER BY tl.TradeLotID -- Record N19  
-- Record N19
GO
CREATE OR ALTER PROCEDURE dbo.RD_SSPBank
AS
BEGIN
	SELECT
		*
	FROM SSPBank b
	WHERE b.InActive = 0
END -- Record N20
GO
CREATE OR ALTER PROCEDURE dbo.RD_SSPRegistrationStatus
AS
BEGIN
	SELECT
		*
	FROM SSPRegistrationStatus rs
	WHERE rs.InActive = 0
END -- Record N21
GO
CREATE OR ALTER PROCEDURE RD_SSPTradeDesign
AS
BEGIN
	SELECT
		td.TradeDesignID
	   ,td.ProgramDesignID AS Scheme
	   ,td.ProvinceID AS ProvinceID
	   ,td.ClusterID
	   ,td.DistrictID
	   ,td.ProgramDesignOn
	   ,td.SelectedCount
	   ,td.SelectedShortList
	   ,td.ProgramFocusID AS ProgramFocus
	   ,td.TradeID AS Trade
	   ,td.TradeDetailMapID AS TradeLayer
	   ,td.CTM
	   ,CAST(td.CTM AS INT) AS CTM
	   ,td.DropOutPerAge AS [DropOut %]
	   ,CAST(td.ExamCost AS INT) AS ExamCost
	   ,td.TraineeContractedTarget
	   ,td.TraineeCompTarget
	   ,td.PerSelectedContraTarget
	   ,td.PerSelectedCompTarget
	FROM SSPTradeDesign td
	WHERE td.InActive = 0
END -- Record N22  
GO
CREATE OR ALTER PROCEDURE dbo.RD_SSPBankDetail @UserID INT
AS
BEGIN
	SELECT
		tbd.BankDetailID
	   ,CASE
			WHEN b.BankName = 'Other' THEN tbd.OtherBank
			WHEN b.BankName <> 'Other' THEN b.BankName
		END AS Bank
	   ,CASE
			WHEN tbd.InActive = 0 THEN 'YES'
			WHEN tbd.InActive = 1 THEN 'NO'
		END AS IsDefault
	   ,tbd.OtherBank
	   ,tbd.BankID AS BankName
	   ,tbd.AccountTitle
	   ,tbd.AccountNumber
	   ,tbd.BranchAddress
	   ,tbd.BranchCode
	   ,tbd.CreatedDate
	FROM SSPTSPBankDetail tbd
	INNER JOIN SSPBank b
		ON tbd.BankID = b.BankID
	WHERE tbd.CreatedUserID = @UserID
END -- Record N23
GO
CREATE OR ALTER PROCEDURE AU_SSPProcessScheduleDetail @ProcessScheduleDetailID INT,
@ProcessScheduleMasterID INT,
@ProcessID NVARCHAR(200),
@ProcessStartDate DATETIME,
@ProcessEndDate DATETIME,
@ProcessDays NVARCHAR(500),
@InActive BIT = 0,
@UserID INT
AS
	IF @ProcessScheduleDetailID = 0
	BEGIN
		INSERT INTO dbo.SSPProcessScheduleDetail (ProcessScheduleMasterID,
		ProcessID,
		ProcessStartDate,
		ProcessEndDate,
		ProcessDays,
		InActive,
		CreatedUserID,
		CreatedDate)
			SELECT
				@ProcessScheduleMasterID
			   ,@ProcessID
			   ,@ProcessStartDate
			   ,@ProcessEndDate
			   ,@ProcessDays
			   ,@InActive
			   ,@UserID
			   ,GETDATE()
	END
	ELSE
	BEGIN
		UPDATE dbo.SSPProcessScheduleDetail
		SET ProcessScheduleMasterID = @ProcessScheduleMasterID
		   ,ProcessID = @ProcessID
		   ,ProcessStartDate = @ProcessStartDate
		   ,ProcessEndDate = @ProcessEndDate
		   ,ProcessDays = @ProcessDays
		   ,InActive = @InActive
		   ,ModifiedUserID = @UserID
		   ,ModifiedDate = GETDATE()
		WHERE ProcessScheduleDetailID = @ProcessScheduleDetailID
	END -- Record N24
GO
CREATE OR ALTER PROCEDURE dbo.RD_SSPTSPRegistrationDetail @UserID INT = 0,
@ApprovalLevel INT = 0
AS
BEGIN
	IF @ApprovalLevel = 3
	BEGIN
		SELECT
			ttm.TradeManageID
		   ,ttp.BusinessName AS TspName
		   ,CASE
				WHEN tts.Status IS NULL OR
					tts.Status = '' THEN 'Pending'
				ELSE tts.Status
			END Status
		   ,ttsh.ProcurementRemarks
		   ,ttl.TrainingLocationName AS TrainingLocation
		   ,ra.RegistrationAuthorityName AS RegistrationAuthority
		   ,p.ProvinceName AS Province
		   ,c.ClusterName AS Cluster
		   ,d.DistrictName AS District
		   ,t1.TehsilName AS Tehsil
		   ,ttl.TrainingLocationAddress AS TrainingAddress
		   ,tc.RegistrationStatus
		   ,tc.RegistrationCerNum AS CertificateNo
		   ,tc.IssuanceDate
		   ,tc.ExpiryDate
		   ,tc.RegistrationCerEvidence AS RegistrationCertificateEvidence
		   ,t.TradeName
		   ,t.TradeCode
		   ,ttm.TradeAsPerCer AS TradePerCertificate
		   ,ttm.NoOfClassMor AS MorningTotalClasses
		   ,ttm.ClassCapacityMor AS MorningClassCapacity
		   ,ttm.NoOfClassEve AS EveningTotalClasses
		   ,ttm.ClassCapacityEve AS EveningClassCapacity
		FROM SSPTSPProfile ttp
		LEFT JOIN SSPTSPTrainingLocation ttl
			ON ttl.CreatedUserID = ttp.UserID
		INNER JOIN SSPTSPTradeManage ttm
			ON ttm.TrainingLocationID = ttl.TrainingLocationID
		INNER JOIN SSPTSPRegistrationPaymentDetail tpd
			ON ttm.TrainingLocationID = tpd.TrainingLocationID
		INNER JOIN PayPro_PaymentDetail pppd
			ON pppd.ID = tpd.PayProPaymentTableID
		LEFT JOIN SSPTrainingCertification tc
			ON tc.TrainingCertificationID = ttm.CertificateID
		LEFT JOIN Trade t
			ON ttm.TradeID = t.TradeID
		LEFT JOIN Sector s
			ON t.SectorID = s.SectorID
		LEFT JOIN SubSector ss
			ON t.SubSectorID = ss.SubSectorID
		LEFT JOIN Province p
			ON ttl.Province = p.ProvinceID
		LEFT JOIN Cluster c
			ON ttl.Cluster = c.ClusterID
		LEFT JOIN District d
			ON ttl.District = d.DistrictID
		LEFT JOIN Tehsil t1
			ON ttl.Tehsil = t1.TehsilID
		LEFT JOIN SSPTSPTradeStatusHistory ttsh
			ON ttm.TradeManageID = ttsh.TradeManageID
				AND ttsh.InActive = 0
		LEFT JOIN SSPApprovalStatus tts
			ON tts.TspTradeStatusID = ttsh.StatusID
		LEFT JOIN RegistrationAuthority ra
			ON ra.RegistrationAuthorityID = ttl.RegistrationAuthority
		WHERE pppd.OrderStatus = 'PAID'
		AND ttp.IsEmailVerify = 1
		AND (
		@UserID = 0
		OR ttp.UserID = @UserID
		)
	END
	ELSE
	BEGIN
		SELECT
			ttm.TradeManageID
		   ,ttp.BusinessName AS TspName
		   ,CASE
				WHEN tts.Status IS NULL OR
					tts.Status = '' THEN 'Pending'
				ELSE tts.Status
			END Status
		   ,ttsh.ProcurementRemarks
		   ,ttl.TrainingLocationName AS TrainingLocation
		   ,ra.RegistrationAuthorityName AS RegistrationAuthority
		   ,p.ProvinceName AS Province
		   ,c.ClusterName AS Cluster
		   ,d.DistrictName AS District
		   ,t1.TehsilName AS Tehsil
		   ,ttl.TrainingLocationAddress AS TrainingAddress
		   ,tc.RegistrationStatus
		   ,tc.RegistrationCerNum AS CertificateNo
		   ,tc.IssuanceDate
		   ,tc.ExpiryDate
		   ,tc.RegistrationCerEvidence AS RegistrationCertificateEvidence
		   ,t.TradeName
		   ,t.TradeCode
		   ,ttm.TradeAsPerCer AS TradePerCertificate
		   ,ttm.NoOfClassMor AS MorningTotalClasses
		   ,ttm.ClassCapacityMor AS MorningClassCapacity
		   ,ttm.NoOfClassEve AS EveningTotalClasses
		   ,ttm.ClassCapacityEve AS EveningClassCapacity
		FROM SSPTSPProfile ttp
		LEFT JOIN SSPTSPTrainingLocation ttl
			ON ttl.CreatedUserID = ttp.UserID
		INNER JOIN SSPTSPTradeManage ttm
			ON ttm.TrainingLocationID = ttl.TrainingLocationID
		INNER JOIN SSPTSPRegistrationPaymentDetail tpd
			ON ttm.TrainingLocationID = tpd.TrainingLocationID
		INNER JOIN PayPro_PaymentDetail pppd
			ON pppd.ID = tpd.PayProPaymentTableID
		LEFT JOIN SSPTrainingCertification tc
			ON tc.TrainingCertificationID = ttm.CertificateID
		LEFT JOIN Trade t
			ON ttm.TradeID = t.TradeID
		LEFT JOIN Sector s
			ON t.SectorID = s.SectorID
		LEFT JOIN SubSector ss
			ON t.SubSectorID = ss.SubSectorID
		LEFT JOIN Province p
			ON ttl.Province = p.ProvinceID
		LEFT JOIN Cluster c
			ON ttl.Cluster = c.ClusterID
		LEFT JOIN District d
			ON ttl.District = d.DistrictID
		LEFT JOIN Tehsil t1
			ON ttl.Tehsil = t1.TehsilID
		LEFT JOIN SSPTSPTradeStatusHistory ttsh
			ON ttm.TradeManageID = ttsh.TradeManageID
				AND ttsh.InActive = 0
		LEFT JOIN SSPApprovalStatus tts
			ON tts.TspTradeStatusID = ttsh.StatusID
		LEFT JOIN RegistrationAuthority ra
			ON ra.RegistrationAuthorityID = ttl.RegistrationAuthority
		WHERE pppd.OrderStatus = 'PAID'
		AND --ttp.InActive = 0 AND        
		ttp.IsEmailVerify = 1
		AND (
		ttsh.StatusID <> 4
		OR ttsh.StatusID IS NULL
		) --AND ttm.ApprovalLevel = @ApprovalLevel
		AND (
		@UserID = 0
		OR ttp.UserID = @UserID
		)
	END
END -- Record N25
GO
CREATE OR ALTER PROCEDURE dbo.RD_SSPTSPTradeManage @UserID INT
AS
BEGIN
	SELECT
		CASE
			WHEN ttm.TradeID = 0 THEN 'Other'
			ELSE t.TradeName
		END AS TradeName
	   ,
		--CASE    
		--    WHEN (    
		--        tts.Status IS NULL    
		--        OR tts.Status = ''    
		--        OR tts.Status = 'Send-Back'    
		--    )    
		--    AND (ttm.ApprovalLevel = 0) THEN 'Pending'    
		--    WHEN (    
		--        tts.Status IS NULL    
		--        OR tts.Status = ''    
		--    )    
		--    AND (ttm.ApprovalLevel = 1) THEN 'In-Progress'    
		--    ELSE tts.Status    
		ISNULL(tts.Status, 'Pending') ApprovalStatus
	   ,CASE
			WHEN ttsh.ProcurementRemarks = '' OR
				ttsh.ProcurementRemarks IS NULL THEN 'Pending'
			ELSE ttsh.ProcurementRemarks
		END ProcurementRemarks
	   ,ttl.TrainingLocationName
	   ,ra.RegistrationAuthorityName AS RegistrationAuthority
	   ,tc.RegistrationCerNum
	   ,ttm.TradeManageID
	   ,ttm.TrainingLocationID
	   ,ttm.CertificateID
	   ,ttm.TradeID
	   ,ttm.TradeAsPerCer
	   ,ISNULL(ttm.TrainingDuration, '---') TrainingDuration
	   ,ttm.NoOfClassMor
	   ,ttm.ClassCapacityMor
	   ,ttm.NoOfClassEve
	   ,ttm.ClassCapacityEve
	   ,ISNULL(pppd.OrderStatus, 'UNPAID') AS PaymentStatus
	FROM SSPTSPTradeManage ttm
	INNER JOIN SSPTSPTrainingLocation ttl
		ON ttm.TrainingLocationID = ttl.TrainingLocationID
	LEFT JOIN SSPTSPRegistrationPaymentDetail spd
		ON ttm.TrainingLocationID = spd.TrainingLocationID
	LEFT JOIN PayPro_PaymentDetail pppd
		ON pppd.ID = spd.PayProPaymentTableID
			AND pppd.OrderStatus = 'PAID'
	INNER JOIN SSPTrainingCertification tc
		ON ttm.CertificateID = tc.TrainingCertificationID
	INNER JOIN RegistrationAuthority ra
		ON ra.RegistrationAuthorityID = tc.RegistrationAuthority
	LEFT JOIN Trade t
		ON ttm.TradeID = t.TradeID
	LEFT JOIN SSPTSPTradeStatusHistory ttsh
		ON ttm.TradeManageID = ttsh.TradeManageID
			AND ttsh.InActive = 0
	LEFT JOIN SSPApprovalStatus tts
		ON tts.TspTradeStatusID = ttsh.StatusID
	WHERE ttm.CreatedUserID = @UserID
END -- Record N26     
GO
CREATE OR ALTER PROCEDURE RD_SSPProcessScheduleMaster
AS
	SELECT
		psm.ProcessScheduleMasterID
	   ,pd.ProgramID
	   ,pd.ProgramName AS ProgramTitle
	   ,psm.programStartDate
	   ,psm.TotalDays
	   ,psm.TotalProcess
	   ,psm.CreatedDate
	FROM SSPProcessScheduleMaster psm
	INNER JOIN SSPProgramDesign pd
		ON psm.ProgramID = pd.ProgramID
	ORDER BY psm.ProcessScheduleMasterID DESC -- Record N27
GO
CREATE OR ALTER PROCEDURE RD_SSPProgramDesignSummary
AS
	WITH cte_TradePlan
	AS
	(SELECT
			td.ProgramDesignID
		   ,SUM(td.TraineeContractedTarget) AS TraineeContractedTarget
		   ,SUM(td.TraineeCompTarget) AS TraineeCompTarget
		   ,COUNT(DISTINCT td.TradeID) AS TotalTrade
		FROM SSPTradeDesign td
		GROUP BY td.ProgramDesignID),
	cte_TradeLot
	AS
	(SELECT
			td.ProgramDesignID
		   ,td.DropOutPerAge
		   ,SUM(tl.TrainingCost) AS TrainingCost
		   ,SUM(tl.Stipend) AS Stipend
		   ,SUM(tl.BagAndBadge) AS TraineeSupportCost
		   ,SUM(tl.ExamCost) AS ExamCost
		   ,SUM(tl.TotalCost) AS TotalCost
		   ,COUNT(tl.TradeLotID) AS TradeLot
		FROM SSPTradeDesign td
		INNER JOIN SSPTradeLot tl
			ON td.TradeDesignID = tl.TradeDesignID
		GROUP BY td.ProgramDesignID
				,td.DropOutPerAge),
	cte_TradeDistrict
	AS
	(SELECT
			td.ProgramDesignID
		   ,ISNULL(
			COUNT(
			DISTINCT CASE
				WHEN s.value <> '' THEN s.value
			END
			),
			0
			) AS TotalProvince
		   ,ISNULL(
			COUNT(
			DISTINCT CASE
				WHEN c.value <> '' THEN c.value
			END
			),
			0
			) AS TotalCluster
		   ,CASE
				WHEN td.ProgramDesignOn = 'Province' THEN 0
				WHEN td.ProgramDesignOn = 'Cluster' THEN 0
				ELSE ISNULL(
					COUNT(
					DISTINCT CASE
						WHEN d.value <> '' THEN d.value
					END
					),
					0
					)
			END AS TotalDistrict
		FROM SSPTradeDesign td
		CROSS APPLY STRING_SPLIT(
		ISNULL(CAST(td.ProvinceID AS NVARCHAR(MAX)), ''),
		','
		) s
		CROSS APPLY STRING_SPLIT(
		ISNULL(CAST(td.ClusterID AS NVARCHAR(MAX)), ''),
		','
		) c
		CROSS APPLY STRING_SPLIT(
		ISNULL(CAST(td.DistrictID AS NVARCHAR(MAX)), ''),
		','
		) d --CROSS APPLY STRING_SPLIT(td.DistrictID, ',') d        
		GROUP BY td.ProgramDesignID
				,td.ProgramDesignOn)
	SELECT
		pd.ProgramID
	   ,ISNULL(pd.IsSubmitted, 0) AS IsSubmitted
	   ,pd.SchemeDesignOn AS SchemeDesignOnID
	   ,pd.Province AS ProvinceID
	   ,pd.Cluster AS ClusterID
	   ,pd.District AS DistrictID
	   ,pd.ProgramName
	   ,ISNULL(pd.IsInitiate, 0) IsInitiated
	   ,CASE
			WHEN pd.IsApproved IS NULL AND
				pd.IsRejected = 0 THEN 'Pending'
			WHEN pd.IsFinalApproved = 0 AND
				pd.IsRejected = 0 THEN 'Rejected'
			WHEN pd.IsFinalApproved = 1 THEN 'Approved'
		END AS ApprovalStatus
	   ,ISNULL(pd.IsFinalApproved, 0) IsFinalApproved
	   ,ISNULL(tts.Status, 'Pending') AS ProcessStatus
	   ,ISNULL(pwh.WorkflowID, 0) AS IsWorkflowAttached
	   ,ISNULL(w.WorkflowTitle, '---') AS SSPWorkflow
	   ,
		--ISNULL(pch.CriteriaID,0) AS IsCriteriaAttached,
		CASE
			WHEN ch.HeaderTitle IS NULL THEN 0
			ELSE 1
		END IsCriteriaAttached
	   ,ISNULL(ch.HeaderTitle, '---') AS Criteria
	   ,pch.StartDate AS AssociationStartDate
	   ,pch.EndDate AS AssociationEndDate
	   ,pd.ClassStartDate
	   ,pd.TentativeProcessStart
	   ,pt.PlaningType
	   ,ISNULL(pwh.WorkflowID, 0) AS WorkflowID
	   ,ISNULL(pwh.Remarks, 0) AS WorkflowRemarks
	   ,ISNULL(pch.CriteriaID, 0) AS CriteriaID
	   ,ISNULL(pch.Remarks, 0) AS CriteriaRemarks
	   ,ISNULL(psh.StatusID, 0) AS StatusID
	   ,ISNULL(psh.Remarks, 0) AS StatusRemarks
	   ,(SELECT
				STRING_AGG(a.Applicability, ',')
			FROM SSPTraineeSupportItems a
			WHERE a.ID IN (SELECT
					*
				FROM STRING_SPLIT(pd.ApplicabilityID, ',')))
		AS SupportItems
	   ,pd.SchemeDesignOn AS ProgramDesignOn
	   ,ISNULL(ctd.TotalProvince, 0) AS TotalProvince
	   ,ISNULL(ctd.TotalCluster, 0) AS TotalCluster
	   ,ISNULL(ctd.TotalDistrict, 0) AS TotalDistrict
	   ,ISNULL(ctl.TradeLot, 0) AS TotalLots
	   ,ISNULL(ctp.TotalTrade, 0) AS TotalTrade
	   ,ctl.DropOutPerAge AS [DropOut%]
	   ,FORMAT(ISNULL(ctp.TraineeContractedTarget, 0), '#,0.00') AS ContractedTarget
	   ,FORMAT(ISNULL(ctp.TraineeCompTarget, 0), '#,0.00') AS CompletionTarget
	   ,FORMAT(ISNULL(ctl.ExamCost, 0), '#,0.00') AS ExamCost
	   ,FORMAT(ISNULL(ctl.Stipend, 0), '#,0.00') AS Stipend
	   ,FORMAT(ISNULL(ctl.TraineeSupportCost, 0), '#,0.00') AS TraineeSupportCost
	   ,FORMAT(ISNULL(ctl.TrainingCost, 0), '#,0.00') AS TrainingCost
	   ,FORMAT(ISNULL(ctl.TotalCost, 0), '#,0.00') AS TotalCost
	FROM SSPProgramDesign pd
	LEFT JOIN cte_TradePlan ctp
		ON ctp.ProgramDesignID = pd.ProgramID
	LEFT JOIN cte_TradeDistrict ctd
		ON ctd.ProgramDesignID = pd.ProgramID
	LEFT JOIN cte_TradeLot ctl
		ON ctl.ProgramDesignID = pd.ProgramID
	LEFT JOIN SSPProgramWorkflowHistory pwh
		ON pd.ProgramID = pwh.ProgramID
			AND pwh.IsInactive = 0
	LEFT JOIN SSPProgramCriteriaHistory pch
		ON pwh.ProgramID = pch.ProgramID
			AND pch.IsInactive = 0
	LEFT JOIN SSPProgramStatusHistory psh
		ON pd.ProgramID = psh.ProgramID
			AND psh.IsInactive = 0
	LEFT JOIN SSPWorkflow w
		ON pd.WorkflowID = w.WorkflowID
	LEFT JOIN SSPCriteriaHeader ch
		ON ch.CriteriaHeaderID = pd.CriteriaID
	LEFT JOIN SSPApprovalStatus tts
		ON tts.TspTradeStatusID = pd.ProgramStatusID
	LEFT JOIN SSPPlaningType pt
		ON pt.PlaningTypeID = pd.PlanningType
	WHERE ctl.TradeLot > 0 -- Record N7
GO
CREATE
OR ALTER PROCEDURE AU_SSPProgramDesign @ProgramID INT = NULL,
@ProgramName NVARCHAR(MAX) = NULL,
@ProgramCode NVARCHAR(10) = NULL,
@FundingCategoryID INT = NULL,
@PCategoryID INT = NULL,
@BusinessRuleType NVARCHAR(MAX) = NULL,
@StipendMode VARCHAR(100) = NULL,
@ContractAwardDate DATETIME = NULL,
@SelectionMethod NVARCHAR(MAX) = NULL,
@ProgramTypeID INT = NULL,
@FundingSourceID INT = NULL,
@PaymentSchedule NVARCHAR(MAX) = NULL,
@ProgramDescription NVARCHAR(MAX) = NULL,
@Stipend FLOAT = NULL,
@ApplicabilityID NVARCHAR(50) = NULL,
@MinimumEducation INT = NULL,
@MaximumEducation INT = NULL,
@MinAge INT = NULL,
@MaxAge INT = NULL,
@GenderID INT = NULL,
@CreatedUserID INT = NULL,
@ModifiedUserID INT = NULL,
@InActive BIT = 0,
@IsSubmitted BIT = 0,
@FinalSubmitted BIT = NULL,
@PlanningType NVARCHAR(50) = NULL,
@TentativeProcessStart DATETIME = NULL,
@ClassStartDate DATETIME = NULL,
@TraineeSupportItems NVARCHAR(50) = NULL,
@EmploymentCommitment NVARCHAR(50) = NULL,
@SchemeDesignOn NVARCHAR(50) = NULL,
@Province NVARCHAR(MAX) = NULL,
@Cluster NVARCHAR(MAX) = NULL,
@District NVARCHAR(MAX) = NULL,
@ApprovalDescription NVARCHAR(MAX) = NULL,
@ApprovalAttachment NVARCHAR(MAX) = NULL,
@TORsAttachment NVARCHAR(MAX) = NULL,
@CriteriaAttachment NVARCHAR(MAX) = NULL,
@FinancialYear NVARCHAR(50) = NULL,
@bagBadgeCost FLOAT = NULL
AS
BEGIN
	IF EXISTS (SELECT
				1
			FROM SSPProgramDesign
			WHERE ProgramID = @ProgramID)
	BEGIN
		UPDATE SSPProgramDesign
		SET ProgramName = @ProgramName
		   ,ProgramCode = @ProgramCode
		   ,ProgramTypeID = @ProgramTypeID
		   ,PCategoryID = @PCategoryID
		   ,FundingSourceID = @FundingSourceID
		   ,FundingCategoryID = @FundingCategoryID
		   ,PaymentSchedule = @PaymentSchedule
		   ,ProgramDescription = @ProgramDescription
		   ,Stipend = @Stipend
		   ,StipendMode = @StipendMode
		   ,ApplicabilityID = @ApplicabilityID
		   ,MinimumEducation = @MinimumEducation
		   ,MaximumEducation = @MaximumEducation
		   ,MinAge = @MinAge
		   ,MaxAge = @MaxAge
		   ,GenderID = @GenderID
		   ,
			--ContractAwardDate = @ContractAwardDate,      
			BusinessRuleType = @BusinessRuleType
		   ,ModifiedUserID = @ModifiedUserID
		   ,ModifiedDate = GETDATE()
		   ,InActive = 0
		   ,IsSubmitted = @IsSubmitted
		   ,FinalSubmitted = @FinalSubmitted
		   ,PlanningType = @PlanningType
		   ,TentativeProcessStart = @TentativeProcessStart
		   ,ClassStartDate = @ClassStartDate
		   ,SelectionMethod = @SelectionMethod
		   ,EmploymentCommitment = @EmploymentCommitment
		   ,SchemeDesignOn = @SchemeDesignOn
		   ,Province = @Province
		   ,Cluster = @Cluster
		   ,District = @District
		   ,ApprovalDescription = @ApprovalDescription
		   ,ApprovalAttachment = @ApprovalAttachment
		   ,TORsAttachment = @TORsAttachment
		   ,CriteriaAttachment = @CriteriaAttachment
		   ,bagBadgeCost = @bagBadgeCost
		   ,FinancialYear = @FinancialYear
		WHERE ProgramID = @ProgramID;
	END
	ELSE
	BEGIN
		INSERT INTO SSPProgramDesign (ProgramName,
		ProgramCode,
		ProgramTypeID,
		PCategoryID,
		FundingSourceID,
		FundingCategoryID,
		PaymentSchedule,
		ProgramDescription,
		Stipend,
		StipendMode,
		ApplicabilityID,
		MinimumEducation,
		MaximumEducation,
		MinAge,
		MaxAge,
		GenderID,
		ContractAwardDate,
		BusinessRuleType,
		CreatedUserID,
		CreatedDate,
		InActive,
		FinalSubmitted,
		PlanningType,
		TentativeProcessStart,
		ClassStartDate,
		SelectionMethod,
		EmploymentCommitment,
		SchemeDesignOn,
		Province,
		Cluster,
		District,
		ApprovalDescription,
		ApprovalAttachment,
		TORsAttachment,
		CriteriaAttachment,
		bagBadgeCost,
		FinancialYear,
		IsSubmitted,
		IsInitiate)
			VALUES (@ProgramName, @ProgramCode, @ProgramTypeID, @PCategoryID, @FundingSourceID, @FundingCategoryID, @PaymentSchedule, @ProgramDescription, @Stipend, @StipendMode, @ApplicabilityID, @MinimumEducation, @MaximumEducation, @MinAge, @MaxAge, @GenderID, GETDATE(), @BusinessRuleType, @CreatedUserID, GETDATE(), 0, @FinalSubmitted, @PlanningType, @TentativeProcessStart, @ClassStartDate, @SelectionMethod, @EmploymentCommitment, @SchemeDesignOn, @Province, @Cluster, @District, @ApprovalDescription, @ApprovalAttachment, @TORsAttachment, @CriteriaAttachment, @bagBadgeCost, @FinancialYear, @IsSubmitted, 0);
	END
END -- Record N27 
GO
CREATE OR ALTER PROCEDURE RD_SSPTSPAssociationEvaluation @ProgramID INT = 0,
@TradeID INT = 0
AS
	SELECT
		pd.ProgramName AS Program
	   ,pd.ProgramID
	   ,CASE
			WHEN tts.Status IS NULL THEN 'Pending'
			ELSE tts.Status
		END EvaluationStatus
	   ,tae.Remarks
	   ,tae.Attachment
	   ,ttp.BusinessName AS TspName
	   ,ttl.TrainingLocationName AS TrainingLocation
	   ,ttl.Province AS ProvinceID
	   ,ttl.Cluster AS ClusterID
	   ,ttl.District AS DistricID
	   ,tam.TradeLotTitle AS TradeLot
	   ,tl.TradeID
	   ,tam.TspAssociationMasterID
	   ,ISNULL(tae.VerifiedCapacityMorning, 0) AS VerifiedCapacityMorning
	   ,ISNULL(tae.VerifiedCapacityEvening, 0) AS VerifiedCapacityEvening
	   ,ISNULL(tae.TotalCapacity, 0) AS TotalCapacity
	   ,ISNULL(tae.MarksBasedOnEvaluation, 0) AS MarksBasedOnEvaluation
	   ,ISNULL(tae.CategoryBasedOnEvaluation, 0) AS CategoryBasedOnEvaluation --,tae.EvalutionStatus AS Status    
	   ,u.FullName AS CreatedBy
	  ,ISNULL(tae.InActive, 0) InActive
,tpd.TSPID
	FROM SSPTspAssociationMaster tam
	INNER JOIN SSPTSPAssociationPaymentDetail tpd
		ON tam.TradeLotID = tpd.TradeLotID
			AND tam.TrainingLocationID = tpd.TrainingLocationID
			AND tam.CreatedUserID = tpd.CreatedUserID
	INNER JOIN PayPro_PaymentDetail pppd
		ON pppd.ID = tpd.PayProPaymentTableID
			AND pppd.OrderStatus = 'PAID'
	LEFT JOIN SSPProgramDesign pd
		ON pd.ProgramID = tam.ProgramDesignID
	LEFT JOIN SSPTSPProfile ttp
		ON ttp.UserID = tam.CreatedUserID
	LEFT JOIN SSPTSPTrainingLocation ttl
		ON ttl.TrainingLocationID = tam.TrainingLocationID
	LEFT JOIN SSPTradeLot tl
		ON tam.TradeLotID = tl.TradeLotID
	LEFT JOIN SSPTspAssociationEvaluation tae
		ON tae.TspAssociationMasterID = tam.TspAssociationMasterID
	--AND tae.InActive = 0    
	LEFT JOIN SSPApprovalStatus tts
		ON tts.TspTradeStatusID = tae.EvaluationStatus
	LEFT JOIN Users u
		ON u.UserID = tae.CreatedUserID
	WHERE (
	@ProgramID = 0
	OR pd.ProgramID = @ProgramID
	)
	AND (
	@TradeID = 0
	OR tl.TradeID = @TradeID
	)

	ORDER BY tae.TspAssociationEvaluationID DESC

GO
CREATE OR ALTER PROCEDURE AU_SSPUpdateSchemeInitialization @ProgramID INT = 0,
@UserID INT = 0
AS
	UPDATE SSPProgramDesign
	SET IsInitiate = 1
	WHERE ProgramID = @ProgramID
	DECLARE @FormID INT = @ProgramID;
	DECLARE @ProcessKey NVARCHAR(250) = 'PROG_APP';
	INSERT INTO dbo.ApprovalHistory (ProcessKey,
	Step,
	FormID,
	ApproverID,
	Comments,
	ApprovalStatusID,
	CreatedUserID,
	ModifiedUserID,
	CreatedDate,
	ModifiedDate,
	InActive)
		VALUES (@ProcessKey --ProcessKey - nvarchar(100)      
		, 1 --Step - int      
		, @FormID --FormID - int      
		, NULL --ApproverID - int      
		, N'Pending' --Comments - nvarchar(4000)      
		, 1 --ApprovalStatusID - int      
		, @UserID --CreatedUserID - int      
		, NULL --ModifiedUserID - int      
		, GETDATE() --CreatedDate - datetime      
		, NULL --ModifiedDate - datetime      
		, 0 --InActive - bit      
		)
	EXEC RD_SSPProgramDesignSummary -- Record N30
GO
CREATE OR ALTER PROCEDURE dbo.AU_SSPContactPerson @UserID INT,
@HeadName NVARCHAR(500),
@HeadDesignation NVARCHAR(100),
@HeadCnicNum NVARCHAR(20),
@HeadCnicFrontImg NVARCHAR(500),
@HeadCnicBackImg NVARCHAR(500),
@HeadEmail NVARCHAR(100),
@HeadMobile NVARCHAR(12),
@OrgLandline NVARCHAR(20),
@POCName NVARCHAR(500),
@POCDesignation NVARCHAR(100),
@POCEmail NVARCHAR(100),
@POCMobile NVARCHAR(12)
AS
BEGIN
	IF EXISTS (SELECT
				*
			FROM SSPTSPProfile ttp
			WHERE ttp.UserID = @UserID)
	BEGIN
		UPDATE dbo.SSPTSPProfile
		SET HeadName = @HeadName
		   ,HeadDesignation = @HeadDesignation
		   ,HeadCnicNum = @HeadCnicNum
		   ,HeadCnicFrontImg = @HeadCnicFrontImg
		   ,HeadCnicBackImg = @HeadCnicBackImg
		   ,HeadEmail = @HeadEmail
		   ,TspEmail = @HeadEmail
		   ,HeadMobile = @HeadMobile
		   ,OrgLandline = @OrgLandline
		   ,POCName = @POCName
		   ,POCDesignation = @POCDesignation
		   ,POCEmail = @POCEmail
		   ,POCMobile = @POCMobile
		   ,ModifiedUserID = @UserID
		   ,ModifiedDate = GETDATE()
		WHERE UserID = @UserID
	END
	ELSE
	BEGIN
		INSERT INTO SSPTSPProfile (UserID,
		HeadName,
		HeadDesignation,
		HeadCnicNum,
		HeadCnicFrontImg,
		HeadCnicBackImg,
		HeadEmail,
		TspEmail,
		HeadMobile,
		OrgLandline,
		POCName,
		POCDesignation,
		POCEmail,
		POCMobile,
		CreatedUserID,
		CreatedDate)
			VALUES (@UserID, @HeadName, @HeadDesignation, @HeadCnicNum, @HeadCnicFrontImg, @HeadCnicBackImg, @HeadEmail, @HeadEmail, @HeadMobile, @OrgLandline, @POCName, @POCDesignation, @POCEmail, @POCMobile, @UserID, GETDATE());
	END
	EXEC RD_SSPTSPProfile @UserID = @UserID
END -- Record N30
GO
CREATE
OR ALTER PROCEDURE RD_SSPTradeWiseTarget
AS
	SELECT
		td.TradeDesignID
	   ,pd.ProgramName
	   ,CONCAT(
		'(',
		(d.Duration),
		' month | ' + soc.Name + ') ',
		t.TradeName
		) AS Trade
	   ,pf.ProgramFocusName AS ProgramFocus
	   ,g.GenderName AS Gender
	   ,td.ProgramDesignOn
	   ,td.SelectedCount AS TotalLot
	   ,d.Duration AS Duration
	   ,td.TraineeContractedTarget AS ContractedTarget
	   ,td.TraineeCompTarget AS CompletionTarget
	   ,CAST(td.DropOutPerAge AS NVARCHAR) + '%' AS [DropOut%]
	   ,FORMAT(
		td.TraineeContractedTarget - td.TraineeCompTarget,
		'N2'
		) AS [AnticipateTraineeDropOut]
	   ,td.CTM
	   ,FORMAT(
		ISNULL(
		(
		(td.CTM * d.Duration * td.TraineeCompTarget) + (pd.Stipend * d.Duration * td.TraineeCompTarget) + (pd.bagBadgeCost * td.TraineeCompTarget) + (td.ExamCost * td.TraineeCompTarget)
		) / TraineeCompTarget,
		0
		),
		'#,0.00'
		) AS TotalCostPerCompletedTrainee --  ,FORMAT((SELECT
		--		SUM(CTM)
		--	FROM SSPTradeLot
		--	WHERE TradeID = td.TradeID)
		--,
		--'N2'
		--) AS TotalCTM
	   ,FORMAT((SELECT
				SUM(TrainingCost)
			FROM SSPTradeLot
			WHERE TradeID = td.TradeID)
		,
		'N2'
		) AS ContractualTrainingCost
	   ,FORMAT(
		ISNULL(td.CTM * d.Duration * td.TraineeCompTarget, 0),
		'#,0.00'
		) AS CompletionTrainingCost
	   ,FORMAT((SELECT
				SUM(Stipend)
			FROM SSPTradeLot
			WHERE TradeID = td.TradeID)
		,
		'N2'
		) AS ContractualStipendCost
	   ,FORMAT(
		ISNULL(
		pd.Stipend * d.Duration * td.TraineeCompTarget,
		0
		),
		'#,0.00'
		) AS CompletionStipendCost
	   ,FORMAT((SELECT
				SUM(BagAndBadge)
			FROM SSPTradeLot
			WHERE TradeID = td.TradeID)
		,
		'N2'
		) AS ContractualBagAndBadgeCost
	   ,FORMAT(
		ISNULL(pd.bagBadgeCost * td.TraineeCompTarget, 0),
		'#,0.00'
		) AS CompletionBagAndBadgeCost
	   ,FORMAT((SELECT
				SUM(ExamCost)
			FROM SSPTradeLot
			WHERE TradeID = td.TradeID)
		,
		'N2'
		) AS ContractualExamCost
	   ,FORMAT(
		ISNULL(td.ExamCost * td.TraineeCompTarget, 0),
		'#,0.00'
		) AS CompletionExamCost --,FORMAT((SELECT
		--		SUM(TotalCost)
		--	FROM SSPTradeLot
		--	WHERE TradeID = td.TradeID)
		--,
		--'N2'
		--) AS ContractualTotalCost
	   ,FORMAT(
		ISNULL(
		(td.CTM * d.Duration * td.TraineeContractedTarget) + (
		pd.Stipend * d.Duration * td.TraineeContractedTarget
		) + (pd.bagBadgeCost * td.TraineeContractedTarget) + (td.ExamCost * td.TraineeContractedTarget),
		0
		),
		'#,0.00'
		) AS ContractualTotalCost
	   ,FORMAT(
		ISNULL(
		(td.CTM * d.Duration * td.TraineeCompTarget) + (pd.Stipend * d.Duration * td.TraineeCompTarget) + (pd.bagBadgeCost * td.TraineeCompTarget) + (td.ExamCost * td.TraineeCompTarget),
		0
		),
		'#,0.00'
		) AS CompletionTotalCost
	   ,ISNULL((SELECT
				STRING_AGG(ProvinceName, ',')
			FROM Province
			WHERE ProvinceID IN (SELECT
					value
				FROM STRING_SPLIT(td.ProvinceID, ',')))
		,
		'---'
		) AS ProposedProvince
	   ,ISNULL((SELECT
				STRING_AGG(ClusterName, ',')
			FROM Cluster
			WHERE ClusterID IN (SELECT
					value
				FROM STRING_SPLIT(td.ClusterID, ',')))
		,
		'---'
		) AS ProposedCluster
	   ,ISNULL((SELECT
				STRING_AGG(DistrictName, ',')
			FROM District
			WHERE DistrictID IN (SELECT
					value
				FROM STRING_SPLIT(td.DistrictID, ',')))
		,
		'---'
		) AS ProposedDistrict
	FROM SSPTradeDesign td
	LEFT JOIN Trade t
		ON td.TradeID = t.TradeID
	INNER JOIN ProgramFocus pf
		ON td.ProgramFocusID = pf.ProgramFocusID
	INNER JOIN SSPProgramDesign pd
		ON pd.ProgramID = td.ProgramDesignID
	INNER JOIN TradeDetailMap tdm
		ON tdm.TradeDetailMapID = td.TradeDetailMapID
	INNER JOIN Duration d
		ON d.DurationID = tdm.DurationID
	INNER JOIN SourceOfCurriculum soc
		ON tdm.SourceOfCurriculumID = soc.SourceOfCurriculumID
	INNER JOIN Gender g
		ON g.GenderID = td.GenderID
	WHERE td.InActive = 0 -- Record N32 
-- Record N32
GO
CREATE
OR ALTER PROCEDURE AU_SSPTradeLot @TradeLotID INT,
@UserID INT,
@TradeDesignID INT,
@TradeID INT,
@TradeDetailMapID INT,
@DistrictID INT = 0,
@ClusterID INT = 0,
@ProvinceID INT = 0,
@Duration FLOAT,
@TraineeSelectedContTarget INT,
@ProgramDesignOn VARCHAR(200),
@CTM DECIMAL(10, 2),
@TrainingCost DECIMAL(10, 2),
@Stipend DECIMAL(10, 2),
@BagAndBadge DECIMAL(10, 2),
@ExamCost DECIMAL(10, 2),
@TotalCost DECIMAL(10, 2)
AS
BEGIN
	DECLARE @ProgramID INT
		   ,@TradeLotNo INT;
	-- Retrieve ProgramDesignID based on TradeDesignID
	SELECT
		@ProgramID = sd.ProgramDesignID
	FROM SSPTradeDesign sd
	WHERE sd.TradeDesignID = @TradeDesignID;
	-- Calculate the next ProgramLotNo for the given ProgramDesignID for Prucrement 
	SELECT
		@TradeLotNo = COUNT(sl.TradeLotID) + 1
	FROM SSPTradeLot sl
	INNER JOIN SSPTradeDesign sd
		ON sd.TradeDesignID = sl.TradeDesignID
	WHERE sd.ProgramDesignID = @ProgramID;
	INSERT INTO dbo.SSPTradeLot (TradeDesignID,
	TradeID,
	TradeLotNo,
	TradeDetailMapID,
	ProgramDesignOn,
	ProvinceID,
	ClusterID,
	DistrictID,
	Duration,
	TraineeSelectedContTarget,
	CTM,
	TrainingCost,
	Stipend,
	BagAndBadge,
	ExamCost,
	TotalCost,
	InActive,
	CreatedUserID,
	CreatedDate)
		VALUES (@TradeDesignID, @TradeID, @TradeLotNo, @TradeDetailMapID, @ProgramDesignOn, @ProvinceID, @ClusterID, @DistrictID, @Duration, @TraineeSelectedContTarget, @CTM, @TrainingCost, @Stipend, @BagAndBadge, @ExamCost, @TotalCost, 0, @UserID, GETDATE())
END -- Record N33  
-- Record N33
GO
CREATE OR ALTER PROCEDURE dbo.AU_SSPTSPTradeManage @UserID INT,
@TradeManageID INT,
@TrainingLocationID INT,
@CertificateID INT,
@TradeID INT,
@TradeAsPerCer VARCHAR(250),
@TrainingDuration VARCHAR(250),
@NoOfClassMor INT,
@ClassCapacityMor INT,
@NoOfClassEve INT,
@ClassCapacityEve INT
AS
BEGIN
	IF @TradeManageID = 0
	BEGIN
		INSERT INTO SSPTSPTradeManage (TrainingLocationID,
		TrainingDuration,
		CertificateID,
		TradeID,
		TradeAsPerCer,
		NoOfClassMor,
		ClassCapacityMor,
		NoOfClassEve,
		ClassCapacityEve,
		TspProfileID,
		InActive,
		CreatedUserID,
		CreatedDate)
			VALUES (@TrainingLocationID, @TrainingDuration, @CertificateID, @TradeID, @TradeAsPerCer, @NoOfClassMor, @ClassCapacityMor, @NoOfClassEve, @ClassCapacityEve, @UserID, 0, @UserID, GETDATE())
	END
	IF @TradeManageID <> 0
	BEGIN
		UPDATE SSPTSPTradeManage
		SET TrainingLocationID = @TrainingLocationID
		   ,CertificateID = @CertificateID
		   ,TrainingDuration = @TrainingDuration
		   ,TradeID = @TradeID
		   ,TradeAsPerCer = @TradeAsPerCer
		   ,NoOfClassMor = @NoOfClassMor
		   ,ClassCapacityMor = @ClassCapacityMor
		   ,NoOfClassEve = @NoOfClassEve
		   ,ClassCapacityEve = @ClassCapacityEve
		   ,ModifiedUserID = @UserID
		   ,ModifiedDate = GETDATE()
		WHERE TradeManageID = @TradeManageID
		AND TspProfileID = @UserID
	END
	EXEC RD_SSPTSPTradeManage @UserID = @UserID
END -- Record N34
-- Record N35
GO
CREATE
OR ALTER PROCEDURE RD_SSPProgramLot @ProgramID INT = 0,
@DistrictID INT = 0,
@UserID INT = 0,
@TrainingLocationID INT = 0,
@IsEdit BIT
AS
	DECLARE @ProgramDesignOn NVARCHAR(200) = 'District'
	SELECT
		@ProgramDesignOn = pd.SchemeDesignOn
	FROM SSPProgramDesign pd
	WHERE pd.ProgramID = @ProgramID
	IF @IsEdit = 0
	BEGIN
		SELECT
			CONCAT(
			'Lot No.',
			CAST(tl.TradeLotNo AS VARCHAR(10)),
			' (',
			t.TradeName,
			'-',
			CASE @ProgramDesignOn
				WHEN 'District' THEN d.DistrictName
				WHEN 'Cluster' THEN c.ClusterName
				WHEN 'Province' THEN p.ProvinceName
				ELSE ''
			END,
			') (',
			pf.ProgramFocusName,
			')'
			) AS LotNo
		   ,tl.TradeLotID
		   ,td.ProgramDesignID
		   ,t.TradeID
		FROM SSPTradeLot tl
		LEFT JOIN Trade t
			ON tl.TradeID = t.TradeID
		LEFT JOIN Province p
			ON tl.ProvinceID = p.ProvinceID
		LEFT JOIN Cluster c
			ON tl.ClusterID = c.ClusterID
		LEFT JOIN District d
			ON tl.DistrictID = d.DistrictID
		LEFT JOIN SSPTradeDesign td
			ON tl.TradeDesignID = td.TradeDesignID
		LEFT JOIN ProgramFocus pf
			ON td.ProgramFocusID = pf.ProgramFocusID
		LEFT JOIN SSPProgramDesign pd
			ON pd.ProgramID = td.ProgramDesignID
		WHERE tl.InActive = 0 --AND d.InActive = 0        
		AND pd.InActive = 0
		AND (pd.ProgramID = @ProgramID)
		AND (
		(
		@ProgramDesignOn = 'Province'
		AND tl.ProvinceID = (SELECT
				d1.ProvinceID
			FROM District d1
			WHERE d1.DistrictID = @DistrictID)
		)
		OR (
		@ProgramDesignOn = 'Cluster'
		AND tl.ClusterID = (SELECT
				d1.ClusterID
			FROM District d1
			WHERE d1.DistrictID = @DistrictID)
		)
		OR (
		@ProgramDesignOn = 'District'
		AND tl.DistrictID = @DistrictID
		)
		)
		AND tl.TradeLotID NOT IN (SELECT
				tam.TradeLotID
			FROM SSPTspAssociationMaster tam
			WHERE tam.ProgramDesignID = @ProgramID
			AND tam.TrainingLocationID = @TrainingLocationID
			AND tam.CreatedUserID = @UserID)
		AND t.TradeID NOT IN (SELECT
				sm.TradeID
			FROM SSPTSPTradeManage sm
			INNER JOIN SSPTSPTradeStatusHistory ssh
				ON sm.TradeManageID = ssh.TradeManageID
				AND ssh.InActive = 0
			WHERE sm.TrainingLocationID = @TrainingLocationID
			AND ssh.StatusID <> 4)
		ORDER BY pd.ProgramID,
		tl.TradeLotID
	END
	ELSE
	IF @IsEdit = 1
	BEGIN
		SELECT
			CONCAT(
			'Lot No.',
			CAST(tl.TradeLotNo AS VARCHAR(10)),
			' (',
			t.TradeName,
			'-',
			CASE @ProgramDesignOn
				WHEN 'District' THEN d.DistrictName
				WHEN 'Cluster' THEN c.ClusterName
				WHEN 'Province' THEN p.ProvinceName
				ELSE ''
			END,
			') (',
			pf.ProgramFocusName,
			')'
			) AS LotNo
		   ,tl.TradeLotID
		   ,td.ProgramDesignID
		   ,t.TradeID
		FROM SSPTradeLot tl
		LEFT JOIN Trade t
			ON tl.TradeID = t.TradeID
		LEFT JOIN Province p
			ON tl.ProvinceID = p.ProvinceID
		LEFT JOIN Cluster c
			ON tl.ClusterID = c.ClusterID
		LEFT JOIN District d
			ON tl.DistrictID = d.DistrictID
		LEFT JOIN SSPTradeDesign td
			ON tl.TradeDesignID = td.TradeDesignID
		LEFT JOIN ProgramFocus pf
			ON td.ProgramFocusID = pf.ProgramFocusID
		LEFT JOIN SSPProgramDesign pd
			ON pd.ProgramID = td.ProgramDesignID
		WHERE tl.InActive = 0 --AND d.InActive = 0        
		AND pd.InActive = 0
		AND (pd.ProgramID = @ProgramID)
		AND (
		(
		@ProgramDesignOn = 'Province'
		AND tl.ProvinceID = (SELECT
				d1.ProvinceID
			FROM District d1
			WHERE d1.DistrictID = @DistrictID)
		)
		OR (
		@ProgramDesignOn = 'Cluster'
		AND tl.ClusterID = (SELECT
				d1.ClusterID
			FROM District d1
			WHERE d1.DistrictID = @DistrictID)
		)
		OR (
		@ProgramDesignOn = 'District'
		AND tl.DistrictID = @DistrictID
		)
		) --AND tl.TradeLotID NOT IN (SELECT        
		--        tam.TradeLotID        
		--       FROM SSPTspAssociationMaster tam        
		--       WHERE tam.ProgramDesignID = @ProgramID        
		--       AND tam.TrainingLocationID = @TrainingLocationID        
		--       AND tam.CreatedUserID = @UserID)     
		AND t.TradeID NOT IN (SELECT
				sm.TradeID
			FROM SSPTSPTradeManage sm
			INNER JOIN SSPTSPTradeStatusHistory ssh
				ON sm.TradeManageID = ssh.TradeManageID
				AND ssh.InActive = 0
			WHERE sm.TrainingLocationID = @TrainingLocationID
			AND ssh.StatusID <> 4)
		ORDER BY pd.ProgramID,
		tl.TradeLotID
	END -- Record N36  
GO
CREATE OR ALTER PROCEDURE AU_SSPProgramStatusHistory @ID INT = 0,
@ProgramID INT,
@StatusID INT,
@Remarks NVARCHAR(2000),
@IsInactive BIT = 0,
@UserID INT
AS
	UPDATE SSPProgramDesign
	SET ProgramStatusID = @StatusID
	WHERE ProgramID = @ProgramID
	AND IsFinalApproved = 1
	UPDATE SSPProgramStatusHistory
	SET IsInactive = 1
	WHERE ProgramID = @ProgramID
	IF @ID = 0
	BEGIN
		INSERT INTO dbo.SSPProgramStatusHistory (ProgramID,
		StatusID,
		Remarks,
		IsInactive,
		CreatedUserID,
		CreatedDate)
			SELECT
				@ProgramID
			   ,@StatusID
			   ,@Remarks
			   ,@IsInactive
			   ,@UserID
			   ,GETDATE()
	END
	ELSE
	BEGIN
		UPDATE dbo.SSPProgramStatusHistory
		SET ProgramID = @ProgramID
		   ,StatusID = @StatusID
		   ,Remarks = @Remarks
		   ,IsInactive = @IsInactive
		   ,ModifiedUserID = @UserID
		   ,ModifiedDate = GETDATE()
		WHERE ID = @ID
	END -- Record N37  
GO
CREATE OR ALTER PROCEDURE dbo.RD_SSPTSPProfile @UserID INT = 0
AS
BEGIN
	SELECT
		ttp.UserID
	   ,ttp.TSPID
	   ,ttp.TspEmail
	   ,ttp.TspContact
	   ,ttp.BusinessName AS InstituteName
	   ,ttp.RegistrationDate AS RegistrationDate
	   ,ttp.NTN AS InstituteNTN
	   ,ttp.NTNEvidence AS NTNAttachment
	   ,ttp.NTNEvidence
	   ,ttp.SalesTaxType AS TaxTypeID
	   ,ttp.GST AS GSTNumber
	   ,ttp.GSTEvidence AS GSTAttachment
	   ,ttp.PRA AS PRANumber
	   ,ttp.GSTEvidence
	   ,ttp.PRAEvidence AS PRAAttachment
	   ,ttp.PRAEvidence
	   ,ttp.LegalStatusID AS LegalStatusID
	   ,ls.LegalStatusName AS LegalStatus
	   ,ttp.LegalStatusEvidence AS LegalStatusAttachment
	   ,ttp.LegalStatusEvidence
	   ,ttp.Website
	   ,p.ProvinceID
	   ,p.ProvinceName
	   ,c.ClusterID
	   ,c.ClusterName
	   ,t.TehsilID
	   ,t.TehsilName
	   ,d.DistrictID
	   ,d.DistrictName
	   ,ttp.GeoTagging
	   ,ttp.Address
	   ,ISNULL(ttp.HeadName, '') AS HeadName
	   ,ttp.HeadCnicNum
	   ,ttp.HeadCnicFrontImg
	   ,ttp.HeadCnicBackImg
	   ,ttp.HeadCnicFrontImg AS OrgHeadCNICFrontImgUrl
	   ,ttp.HeadCnicBackImg AS OrgHeadCNICBackImgUrl
	   ,ttp.HeadDesignation
	   ,ttp.TspEmail AS HeadEmail
	   ,ttp.HeadMobile
	   ,ttp.OrgLandline
	   ,ttp.POCName
	   ,ttp.POCDesignation
	   ,ttp.POCEmail
	   ,ttp.POCMobile
	   ,FinalSubmitted
	   ,ttp.ProgramTypeID
	FROM SSPTSPProfile ttp
	LEFT JOIN ProgramType pt
		ON ttp.ProgramTypeID = pt.PTypeID
	LEFT JOIN SSPLegalStatus ls
		ON ls.LegalStatusID = ttp.LegalStatusID
	LEFT JOIN Province p
		ON ttp.ProvinceID = p.ProvinceID
	LEFT JOIN Cluster c
		ON ttp.ClusterID = c.ClusterID
	LEFT JOIN District d
		ON ttp.DistrictID = d.DistrictID
	LEFT JOIN Tehsil t
		ON ttp.TehsilID = t.TehsilID
	WHERE (
	@UserID = 0
	OR ttp.UserID = @UserID
	)
END -- Record N38
GO
CREATE OR ALTER PROCEDURE dbo.AU_SSPPayPro_PaymentDetail @OrderNumber NVARCHAR(256),
@OrderAmount FLOAT = NULL,
@OrderDueDate DATETIME = NULL,
@OrderType NVARCHAR(256) = NULL,
@IssueDate DATETIME = NULL,
@OrderExpireAfterSeconds INT = NULL,
@OrderAmountWithinDueDate FLOAT = NULL,
@OrderAmountAfterDueDate FLOAT = NULL,
@Status NVARCHAR(50) = NULL,
@IsFeeApplied BIT = NULL,
@ConnectPayId NVARCHAR(150) = NULL,
@ConnectPayFee NVARCHAR(50) = NULL,
@Description NVARCHAR(256) = NULL,
@DatePaid DATETIME = NULL,
@OrderStatus NVARCHAR(256) = NULL,
@OrderAmountPaid FLOAT = NULL,
@Click2Pay NVARCHAR(MAX) = NULL
AS
	INSERT INTO dbo.PayPro_PaymentDetail (OrderNumber,
	OrderAmount,
	OrderDueDate,
	OrderType,
	IssueDate,
	OrderExpireAfterSeconds,
	OrderAmountWithinDueDate,
	OrderAmountAfterDueDate,
	Status,
	IsFeeApplied,
	ConnectPayId,
	ConnectPayFee,
	Description,
	DatePaid,
	OrderStatus,
	OrderAmountPaid,
	Click2Pay)
		SELECT
			@OrderNumber
		   ,@OrderAmount
		   ,@OrderDueDate
		   ,@OrderType
		   ,@IssueDate
		   ,0
		   ,@OrderAmountWithinDueDate
		   ,@OrderAmountAfterDueDate
		   ,@Status
		   ,@IsFeeApplied
		   ,@ConnectPayId
		   ,''
		   ,@Description
		   ,@DatePaid
		   ,@OrderStatus
		   ,@OrderAmountPaid
		   ,@Click2Pay
	SELECT
		*
	FROM PayPro_PaymentDetail pppd
	WHERE pppd.ID = SCOPE_IDENTITY() -- Record N39
GO
CREATE OR ALTER PROCEDURE RD_SSPTradeLotByTradeDesign @TradeDesignID INT = 0
AS
	SELECT
		tl.TradeLotID
	   ,tl.DistrictID
	   ,tl.TraineeSelectedContTarget
	   ,tl.TotalCost
	FROM SSPTradeLot tl
	WHERE tl.TradeDesignID = @TradeDesignID -- Record N40  
GO
CREATE OR ALTER PROCEDURE RD_SSPTspAssociationDetail @TspAssociationMaster INT = 0
AS
BEGIN
	SELECT
		tad.TspAssociationDetailID
	   ,cmc.CriteriaMainCategoryID AS CriteriaMainCategoryID
	   ,cmc.CriteriaHeaderID AS CriteriaTemplateID
	   ,cmc.MainCategoryTitle AS CategoryTitle
	   ,tad.Attachment AS Evidence
	   ,tad.Remarks
	   ,cmc.TotalMarks
	FROM SSPTspAssociationDetail tad
	LEFT JOIN SSPTspAssociationMaster tam
		ON tad.TspAssociationMasterID = tam.TspAssociationMasterID
	LEFT JOIN SSPCriteriaMainCategory cmc
		ON cmc.CriteriaMainCategoryID = tad.CriteriaMainCategoryID
	WHERE (
	@TspAssociationMaster = 0
	OR tad.TspAssociationMasterID = @TspAssociationMaster
	)
	AND tam.InActive = 0
END -- Record N41  
GO
CREATE OR ALTER PROCEDURE RD_SSPTSPByProgramDistrictAndTradeLot @ProgramDesignOn NVARCHAR(200) = 'District',
@ProgramID INT = 0,
@LocationID INT = 0,
@TradeLotID INT = 0
AS
	SELECT
		tae.TspAssociationEvaluationID
	   ,tam.ProgramDesignID AS ProgramID
	   ,tam.TrainingLocationID
	   ,tam.CreatedUserID AS TSPID
	   ,ttp.BusinessName + ' (' + ttl.TrainingLocationName + ')' AS TSPName
	   ,tae.TotalCapacity AS TSPCapacity
	   ,tam.TradeLotID
	   ,tam.TradeLotTitle
	   ,ttl.Province AS ProvinceID
	   ,ttl.Cluster AS ClusterID
	   ,ttl.District AS DistrictID
	FROM SSPTspAssociationEvaluation tae
	LEFT JOIN SSPTspAssociationMaster tam
		ON tae.TspAssociationMasterID = tam.TspAssociationMasterID
	LEFT JOIN SSPTSPTrainingLocation ttl
		ON tam.TrainingLocationID = ttl.TrainingLocationID
	LEFT JOIN Province p
		ON ttl.Province = p.ProvinceID
	LEFT JOIN Cluster c
		ON ttl.Cluster = c.ClusterID
	LEFT JOIN District d
		ON ttl.District = d.DistrictID
	LEFT JOIN SSPTSPProfile ttp
		ON ttp.UserID = tam.CreatedUserID
	LEFT JOIN SSPTSPAssignment t
		ON t.AssociationEvaluationID = tae.TspAssociationEvaluationID
	WHERE tam.InActive = 0
	AND ttl.InActive = 0
	AND tae.InActive = 0
	AND t.TSPAssignmentID IS NULL
	AND tam.ProgramDesignID = @ProgramID
	AND (
	(
	@ProgramDesignOn = 'Province'
	AND ttl.Province = @LocationID
	)
	OR (
	@ProgramDesignOn = 'Cluster'
	AND ttl.Cluster = @LocationID
	)
	OR (
	@ProgramDesignOn = 'District'
	AND ttl.District = @LocationID
	)
	)
	AND tam.TradeLotID = @TradeLotID -- Record N42
GO
CREATE OR ALTER PROCEDURE [dbo].[AU_SSPOTPVerification] @TSPID INT,
@OTPCode INT
AS
BEGIN
	IF EXISTS (SELECT
				1
			FROM SSPTSPProfile
			WHERE TSPID = @TSPID
			AND OTPCode = @OTPCode)
	BEGIN
		UPDATE SSPTSPProfile
		SET IsEmailVerify = 1
		WHERE TSPID = @TSPID
		AND OTPCode = @OTPCode;
	END
	SELECT
		tu.UserName
	   ,tup.UserPassword
	   ,ttp.TspEmail
	   ,ttp.TspContact
	FROM SSPTSPProfile ttp
	INNER JOIN SSPUsers tu
		ON ttp.UserID = tu.UserID
	INNER JOIN SSPUsersPwd tup
		ON ttp.UserID = tup.UserID
	WHERE ttp.TSPID = @TSPID
	AND ttp.OTPCode = @OTPCode
	AND tu.InActive = 0
	AND tup.InActive = 0
END;
-- Record N43
GO
CREATE OR ALTER PROCEDURE [dbo].[CTMCalculationReport1] @FundingSourceName VARCHAR(MAX) = NULL,
@SchemeType VARCHAR(MAX) = NULL,
@FundingCategory VARCHAR(MAX) = NULL,
@ContractAwardStartDate DATE = NULL,
@ContractAwardEndDate DATE = NULL,
@Sector VARCHAR(MAX) = NULL,
@Trade VARCHAR(MAX) = NULL,
@Duration VARCHAR(MAX) = '1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24',
@District VARCHAR(MAX) = NULL,
@Cluster VARCHAR(MAX) = NULL
AS
BEGIN
	SELECT
		FS.FundingSourceName AS FundingSource
	   ,FS.FundingSourceID
	   ,S.SAPID AS SAPSchemeCode
	   ,S.SchemeName AS Scheme
	   ,PT.AMSName AS SchemeType
	   ,S.ContractAwardDate
	   ,T.TSPName AS TSP
	   ,C.ClassCode AS Class
	   ,CS.ClassStatusName AS ClassStatus
	   ,se.SectorName AS Sector
	   ,tr.TradeName AS Trade
	   ,tr.TradeID
	   ,C.Duration
	   ,C.TraineesPerClass
	   ,C.Batch
	   ,g.GenderName AS Gender
	   ,C.TrainingAddressLocation
	   ,d.DistrictName AS District
	   ,d.DistrictID
	   ,cl.ClusterName AS Cluster
	   ,cl.ClusterID
	   ,C.MinHoursPerMonth
	   ,C.StartDate
	   ,C.EndDate
	   ,C.SourceOfCurriculum
	   ,edt.Education AS EntryQualification
	   ,cer.CertAuthName AS CertificationAuthority
	   ,S.PaymentSchedule
	   ,C.TrainingCostPerTraineePerMonthExTax
	   ,C.SalesTax
	   ,C.TrainingCostPerTraineePerMonthInTax
	   ,C.UniformBagCost
	   ,C.PerTraineeTestCertCost
	   ,C.TotalCostPerClassInTax
	   ,C.OverallEmploymentCommitment AS EmploymentCommitmentSelf
	   ,C.Duration * C.TraineesPerClass AS 'Weight'
	   ,C.TrainingCostPerTraineePerMonthExTax * (C.Duration * C.TraineesPerClass) AS ExclusiveOfSalesTax
	   ,C.TrainingCostPerTraineePerMonthInTax * (C.Duration * C.TraineesPerClass) AS InclusiveOfSalesTax
	FROM Class AS C
	LEFT JOIN ClassStatus AS CS
		ON CS.ClassStatusID = C.ClassStatusID
	INNER JOIN Scheme AS S
		ON S.SchemeID = C.SchemeID
	INNER JOIN ProgramType AS PT
		ON PT.PTypeID = S.ProgramTypeID
	INNER JOIN TSPDetail AS T
		ON T.TSPID = C.TSPID
	LEFT JOIN dbo.TSPMaster AS TSPM
		ON T.TSPMasterID = TSPM.TSPMasterID
	INNER JOIN EducationTypes edt
		ON edt.EducationTypeID = C.EntryQualification
	INNER JOIN FundingSource FS
		ON FS.FundingSourceID = S.FundingSourceID
	INNER JOIN Trade tr
		ON tr.TradeID = C.TradeID
	INNER JOIN District d
		ON d.DistrictID = C.DistrictID
	INNER JOIN Gender g
		ON g.GenderID = C.GenderID
	INNER JOIN Cluster cl
		ON cl.ClusterID = C.ClusterID
	INNER JOIN Sector se
		ON se.SectorID = C.SectorID
	INNER JOIN CertificationAuthority cer
		ON cer.CertAuthID = C.CertAuthID
	INNER JOIN STRING_SPLIT(@Duration, ',')
		ON VALUE = C.Duration
	WHERE S.FinalSubmitted = 1
	AND S.IsApproved = 1
	AND S.IsMigrated = 0
	AND S.ProgramTypeID <> 9
	AND (S.FundingCategoryID IN (SELECT
			*
		FROM STRING_SPLIT(@FundingCategory, ','))
	OR @FundingCategory IS NULL)
	AND (S.ContractAwardDate BETWEEN @ContractAwardStartDate AND @ContractAwardEndDate)
	AND (
	S.FundingSourceID IN (SELECT
			*
		FROM STRING_SPLIT(@FundingSourceName, ','))
	OR @FundingSourceName IS NULL
	)
	AND (
	S.ProgramTypeID IN (SELECT
			*
		FROM STRING_SPLIT(@SchemeType, ','))
	OR @SchemeType IS NULL
	)
	--AND (
	--S.ContractAwardDate IN (SELECT value
	--	FROM STRING_SPLIT(@ContractAwardDate, ',')) OR @ContractAwardDate IS NULL
	--)
	AND (
	C.SectorID IN (SELECT
			*
		FROM STRING_SPLIT(@Sector, ','))
	OR @Sector IS NULL
	)
	AND (
	C.TradeID IN (SELECT
			*
		FROM STRING_SPLIT(@Trade, ','))
	OR @Trade IS NULL
	)
	AND (
	C.DistrictID IN (SELECT
			*
		FROM STRING_SPLIT(@District, ','))
	OR @District IS NULL
	)
	AND (
	cl.ClusterID IN (SELECT
			VALUE
		FROM STRING_SPLIT(@Cluster, ','))
	OR @Cluster IS NULL
	) --GROUP BY tr.TradeName    
	ORDER BY tr.TradeName
END
-- Record N44  

GO
CREATE OR ALTER PROCEDURE [dbo].[HistoricalReport] @FundingSourceID NVARCHAR(MAX) = NULL,
@ProgramTypeID NVARCHAR(MAX) = NULL,
@StartDate NVARCHAR(MAX) = NULL,
@EndDate NVARCHAR(MAX) = NULL,
@ProgramFocusID NVARCHAR(MAX) = NULL,
@SchemeID NVARCHAR(MAX) = NULL,
@SectorID NVARCHAR(MAX) = NULL,
@SubSectorID NVARCHAR(MAX) = NULL,
@TradeID NVARCHAR(MAX) = NULL,
@TSPID NVARCHAR(MAX) = NULL,
@ProvinceID NVARCHAR(MAX) = NULL,
@ClusterID NVARCHAR(MAX) = NULL,
@DistrictID NVARCHAR(MAX) = NULL
AS
BEGIN
	SELECT
		FS.FundingSourceName
	   ,pt.PTypeName AS ProgramType
	   ,c.StartDate
	   ,c.EndDate
	   ,PF.ProgramFocusName
	   ,s.SchemeName
	   ,se.SectorName
	   ,SS.SubSectorName
	   ,tr.TradeName
	   ,t.TSPName
	   ,P.ProvinceName
	   ,cl.ClusterName
	   ,d.DistrictName
	   ,SUM(
		CASE c.ClassStatusID
			WHEN 5 THEN 0
			ELSE c.TraineesPerClass
		END
		) AS ContractualTrainees
	   ,COUNT(TP.TraineeID) AS EnrolledTrainees
	   ,SUM(
		CASE TS.StatusName
			WHEN 'Completed' THEN 1
			ELSE 0
		END
		) AS CompletedTrainees
	   ,ROUND(
		SUM(
		c.TraineesPerClass * (
		CAST(c.OverallEmploymentCommitment AS NUMERIC) / 100
		)
		),
		0
		) AS EmploymentCommitment
	   ,SUM(
		CASE PFE.EmploymentStatus
			WHEN 'Employed' THEN 1
			ELSE 0
		END
		) AS EmploymentReported
	   ,SUM(
		CASE
			WHEN (
				(
				PFE.ForwardedToTelephonic = 0 AND
				pv.ID IS NOT NULL AND
				pv.IsVerified = 1
				) OR
				(
				PFE.ForwardedToTelephonic = 1 AND
				pv.ID IS NOT NULL AND
				pv.TelephonicVerificationStatus = 1
				)
				) THEN 1
			WHEN (
				(
				PFE.ForwardedToTelephonic = 0 AND
				pv.ID IS NULL AND
				pv.IsVerified IS NULL
				) OR
				(
				PFE.ForwardedToTelephonic = 1 AND
				pv.ID IS NULL AND
				pv.TelephonicVerificationStatus IS NULL
				) OR
				(
				PFE.ForwardedToTelephonic = 1 AND
				pv.ID IS NOT NULL AND
				pv.TelephonicVerificationStatus IS NULL
				) OR
				(
				PFE.ForwardedToTelephonic = 1 AND
				pv.ID IS NOT NULL AND
				pv.TelephonicVerificationStatus = 0 AND
				pv.IsVerified IS NULL
				)
				) THEN 0
			ELSE 0
		END
		) AS EmploymentVerified
	   ,SUM(
		CASE c.ClassStatusID
			WHEN 6 THEN c.TraineesPerClass
			ELSE 0
		END
		) AS CancelledClasses
	   ,SUM(
		CASE c.ClassStatusID
			WHEN 5 THEN c.TraineesPerClass
			ELSE 0
		END
		) AS AbandonedClasses
	   ,SUM(
		CASE TP.GenderID
			WHEN 3 THEN 1
			ELSE 0
		END
		) AS MaleTrainees
	   ,SUM(
		CASE TP.GenderID
			WHEN 5 THEN 1
			ELSE 0
		END
		) AS FemaleTrainees
	   ,SUM(
		CASE TP.GenderID
			WHEN 6 THEN 1
			ELSE 0
		END
		) AS TransgenderTrainees
	FROM Class c
	INNER JOIN ClassStatus cs
		ON cs.ClassStatusID = c.ClassStatusID
	INNER JOIN TraineeProfile AS TP
		ON c.ClassID = TP.ClassID
			AND (
				@StartDate IS NULL
				OR c.StartDate >= @StartDate
			)
			AND (
				@EndDate IS NULL
				OR c.EndDate <= @EndDate
			)
	OUTER APPLY GetLatestTraineeStatus(TP.TraineeID) AS TS
	INNER JOIN ProgramFocus AS PF
		ON c.ProgramFocusID = PF.ProgramFocusID
			AND (
				@ProgramFocusID IS NULL
				OR PF.ProgramFocusID IN (SELECT
						*
					FROM STRING_SPLIT(@ProgramFocusID, ','))
			)
	INNER JOIN TSPDetail t
		ON t.TspID = c.TspID
			AND (
				@TSPID IS NULL
				OR t.TspID IN (SELECT
						VALUE
					FROM STRING_SPLIT(@TSPID, ','))
			)
	INNER JOIN Scheme s
		ON s.SchemeID = c.SchemeID
			AND (
				@SchemeID IS NULL
				OR s.SchemeID IN (SELECT
						*
					FROM STRING_SPLIT(@SchemeID, ','))
			)
	INNER JOIN FundingSource FS
		ON FS.FundingSourceID = s.FundingSourceID
			AND (
				@FundingSourceID IS NULL
				OR FS.FundingSourceID IN (SELECT
						VALUE
					FROM STRING_SPLIT(@FundingSourceID, ','))
			)
	INNER JOIN Trade tr
		ON tr.TradeID = c.TradeID
			AND (
				@TradeID IS NULL
				OR tr.TradeID IN (SELECT
						*
					FROM STRING_SPLIT(@TradeID, ','))
			)
	INNER JOIN District d
		ON d.DistrictID = c.DistrictID
			AND (
				@DistrictID IS NULL
				OR d.DistrictID IN (SELECT
						*
					FROM STRING_SPLIT(@DistrictID, ','))
			)
	INNER JOIN Province AS P
		ON d.ProvinceID = P.ProvinceID
			AND (
				@ProvinceID IS NULL
				OR P.ProvinceID IN (SELECT
						VALUE
					FROM STRING_SPLIT(@ProvinceID, ','))
			)
	INNER JOIN Gender g
		ON g.GenderID = c.GenderID
	INNER JOIN Cluster cl
		ON cl.ClusterID = c.ClusterID
			AND (
				@ClusterID IS NULL
				OR cl.ClusterID IN (SELECT
						VALUE
					FROM STRING_SPLIT(@ClusterID, ','))
			)
	INNER JOIN Sector se
		ON se.SectorID = c.SectorID
			AND (
				@SectorID IS NULL
				OR se.SectorID IN (SELECT
						*
					FROM STRING_SPLIT(@SectorID, ','))
			)
	INNER JOIN SubSector SS
		ON se.SectorID = SS.SectorID
			AND (
				@SubSectorID IS NULL
				OR SS.SubSectorID IN (SELECT
						*
					FROM STRING_SPLIT(@SubSectorID, ','))
			)
	LEFT JOIN PlacementFormE AS PFE
		ON TP.TraineeID = PFE.TraineeID
	LEFT JOIN PlacementVerification pv
		ON pv.PlacementID = PFE.PlacementID
	INNER JOIN ProgramType pt
		ON s.ProgramTypeID = pt.PTypeID
			AND (
				@ProgramTypeID IS NULL
				OR pt.PTypeID IN (SELECT
						*
					FROM STRING_SPLIT(@ProgramTypeID, ','))
			)
	WHERE c.IsMigrated = 0
	AND TP.IsSubmitted = 1
	GROUP BY FS.FundingSourceName
			,pt.PTypeName
			,c.StartDate
			,c.EndDate
			,PF.ProgramFocusName
			,s.SchemeName
			,se.SectorName
			,SS.SubSectorName
			,tr.TradeName
			,t.TSPName
			,P.ProvinceName
			,cl.ClusterName
			,d.DistrictName;
END -- Record N45
GO
CREATE OR ALTER PROCEDURE [dbo].[RD_SSPTSPRegistrationDetailReport] @InActive BIT = 0,
@UserID INT = 0,
@TradeID INT = 0,
@ProvinceID INT = 0,
@ClusterID INT = 0,
@DistrictID INT = 0
AS
BEGIN
	SELECT
		ttp.BusinessName AS TspName
	   ,ttp.UserID AS TspID
	   ,t.TradeName
	   ,t.TradeID
	   ,p.ProvinceName AS Province
	   ,p.ProvinceID
	   ,c.ClusterName AS Cluster
	   ,c.ClusterID
	   ,d.DistrictName AS District
	   ,d.DistrictID
	   ,ra.RegistrationAuthorityName AS RegistrationAuthority
	   ,tc.ExpiryDate
	   ,ttm.TrainingDuration AS [TrainingDuration (Months)]
	   ,ttm.ClassCapacityMor AS 'ClassCapacityMorning'
	   ,ttm.ClassCapacityEve AS 'ClassCapacityEvening'
	   ,ttsh.ProcurementRemarks AS 'ProcurementRemarks'
	   ,tts.Status
	FROM SSPTSPProfile ttp
	LEFT JOIN SSPTSPTrainingLocation ttl
		ON ttl.CreatedUserID = ttp.UserID
	INNER JOIN SSPTSPTradeManage ttm
		ON ttm.TrainingLocationID = ttl.TrainingLocationID
	LEFT JOIN SSPTrainingCertification tc
		ON tc.TrainingCertificationID = ttm.CertificateID
	LEFT JOIN Trade t
		ON ttm.TradeID = t.TradeID
	LEFT JOIN Sector s
		ON t.SectorID = s.SectorID
	LEFT JOIN SubSector ss
		ON t.SubSectorID = ss.SubSectorID
	LEFT JOIN Province p
		ON ttl.Province = p.ProvinceID
	LEFT JOIN Cluster c
		ON ttl.Cluster = c.ClusterID
	LEFT JOIN District d
		ON ttl.District = d.DistrictID
	LEFT JOIN Tehsil t1
		ON ttl.Tehsil = t1.TehsilID
	LEFT JOIN SSPTSPTradeStatusHistory ttsh
		ON ttm.TradeManageID = ttsh.TradeManageID
			AND ttsh.InActive = 0
	LEFT JOIN SSPApprovalStatus tts
		ON tts.TspTradeStatusID = ttsh.StatusID
	LEFT JOIN RegistrationAuthority ra
		ON ra.RegistrationAuthorityID = ttl.RegistrationAuthority
	WHERE (
	@UserID = 0
	OR ttp.UserID = @UserID
	)
	AND (
	@TradeID = 0
	OR t.TradeID = @TradeID
	)
	AND (
	@ProvinceID = 0
	OR p.ProvinceID = @ProvinceID
	)
	AND (
	@ClusterID = 0
	OR c.ClusterID = @ClusterID
	)
	AND (
	@DistrictID = 0
	OR d.DistrictID = @DistrictID
	)
END -- Record N46
GO
CREATE OR ALTER PROCEDURE AU_SSPTspAssociationEvaluation @TspAssociationEvaluationID INT = NULL,
@TspAssociationMasterID INT,
@VerifiedCapacityMorning INT,
@VerifiedCapacityEvening INT,
@TotalCapacity INT,
@MarksBasedOnEvaluation INT,
@CategoryBasedOnEvaluation NVARCHAR(200),
@Remarks NVARCHAR(MAX),
@Attachment NVARCHAR(500),
@EvaluationStatus INT,
@UserID INT = NULL
AS
BEGIN
	UPDATE SSPTspAssociationEvaluation
	SET Inactive = 1
	WHERE TspAssociationMasterID = @TspAssociationMasterID

	INSERT INTO SSPTspAssociationEvaluation (TspAssociationMasterID,
	VerifiedCapacityMorning,
	VerifiedCapacityEvening,
	TotalCapacity,
	MarksBasedOnEvaluation,
	CategoryBasedOnEvaluation,
	EvaluationStatus,
	CreatedUserID,
	Remarks,
	Attachment,
	Inactive,
	CreatedDate)
		VALUES (@TspAssociationMasterID, @VerifiedCapacityMorning, @VerifiedCapacityEvening, @TotalCapacity, @MarksBasedOnEvaluation, @CategoryBasedOnEvaluation, @EvaluationStatus, @UserID, @Remarks, @Attachment, 0, GETDATE());
	SELECT
		*
	FROM SSPTspAssociationEvaluation
END 
GO
CREATE OR ALTER PROCEDURE dbo.AU_SSPBankDetail @UserID INT,
@BankDetailID INT,
@BankID INT,
@OtherBank NVARCHAR(100) = '',
@AccountTitle NVARCHAR(200),
@AccountNumber NVARCHAR(100),
@BranchAddress NVARCHAR(500),
@BranchCode NVARCHAR(100)
AS
BEGIN
	IF @BankDetailID = 0
	BEGIN
		UPDATE SSPTSPBankDetail
		SET InActive = 1
		WHERE CreatedUserID = @UserID
		INSERT INTO dbo.SSPTSPBankDetail (TspProfileID,
		BankID,
		OtherBank,
		AccountTitle,
		AccountNumber,
		BranchAddress,
		BranchCode,
		InActive,
		CreatedUserID,
		CreatedDate)
			VALUES (@UserID, @BankID, @OtherBank, @AccountTitle, @AccountNumber, @BranchAddress, @BranchCode, 0, @UserID, GETDATE())
	END
	IF @BankDetailID <> 0
	BEGIN
		UPDATE dbo.SSPTSPBankDetail
		SET BankID = @BankID
		   ,OtherBank = @OtherBank
		   ,AccountTitle = @AccountTitle
		   ,AccountNumber = @AccountNumber
		   ,BranchAddress = @BranchAddress
		   ,BranchCode = @BranchCode
		   ,ModifiedUserID = @UserID
		   ,ModifiedDate = GETDATE()
		WHERE BankDetailID = @BankDetailID
		AND TspProfileID = @UserID
	END
	EXEC RD_SSPBankDetail @UserID = @UserID
END -- Record N48
GO
CREATE OR ALTER PROCEDURE dbo.AU_SSPTrainingCertification @UserID INT,
@TrainingCertificationID INT,
@TrainingLocationID INT,
@RegistrationAuthority INT,
@RegistrationStatus INT,
@RegistrationCerNum VARCHAR(250),
@IssuanceDate DATE,
@ExpiryDate VARCHAR(250),
@RegistrationCerEvidence NVARCHAR(500)
AS
BEGIN
	IF @TrainingCertificationID = 0
	BEGIN
		INSERT INTO dbo.SSPTrainingCertification (TrainingLocationID,
		TspProfileID,
		RegistrationAuthority,
		RegistrationStatus,
		RegistrationCerNum,
		IssuanceDate,
		ExpiryDate,
		RegistrationCerEvidence,
		InActive,
		CreatedUserID,
		CreatedDate)
			VALUES (@TrainingLocationID, @UserID, @RegistrationAuthority, @RegistrationStatus, @RegistrationCerNum, @IssuanceDate, @ExpiryDate, @RegistrationCerEvidence, 0, @UserID, GETDATE())
	END
	IF @TrainingCertificationID <> 0
	BEGIN
		UPDATE dbo.SSPTrainingCertification
		SET TrainingLocationID = @TrainingLocationID
		   ,RegistrationAuthority = @RegistrationAuthority
		   ,RegistrationStatus = @RegistrationStatus
		   ,RegistrationCerNum = @RegistrationCerNum
		   ,IssuanceDate = @IssuanceDate
		   ,ExpiryDate = @ExpiryDate
		   ,RegistrationCerEvidence = @RegistrationCerEvidence
		   ,ModifiedUserID = @UserID
		   ,ModifiedDate = GETDATE()
		WHERE TrainingCertificationID = @TrainingCertificationID
		AND TspProfileID = @UserID
	END
	EXEC RD_SSPTrainingCertification @UserID = @UserID
END -- Record N50
GO
CREATE OR ALTER PROCEDURE RD_SSPProgramBudget
AS
	SELECT
		pd.ProgramID
	   ,pd.ProgramName AS Program
	   ,FORMAT(
		CAST(
		SUM(CAST(tl.TraineeSelectedContTarget AS INT)) AS DECIMAL(18, 2)
		),
		'N2'
		) AS ContractedTarget
	   ,FORMAT(SUM(DISTINCT td.TraineeCompTarget), 'N2') AS CompletionTarget
	   ,FORMAT(SUM(tl.CTM), 'N2') AS CTM
	   ,FORMAT(SUM(tl.TrainingCost), 'N2') AS TrainingCost
	   ,FORMAT(SUM(tl.Stipend), 'N2') AS Stipend
	   ,FORMAT(SUM(tl.BagAndBadge), 'N2') BagAndBadge
	   ,FORMAT(SUM(tl.ExamCost), 'N2') AS ExamCost
	   ,FORMAT(SUM(tl.TotalCost), 'N2') AS TotalCost
	FROM SSPTradeLot tl
	LEFT JOIN Trade t
		ON tl.TradeID = t.TradeID
	LEFT JOIN SSPTradeDesign td
		ON tl.TradeDesignID = td.TradeDesignID
	LEFT JOIN SSPProgramDesign pd
		ON pd.ProgramID = td.ProgramDesignID
	WHERE tl.InActive = 0
	GROUP BY pd.ProgramID
			,pd.ProgramName -- Record N49
GO
CREATE OR ALTER PROCEDURE dbo.AU_SSPTspInfo @UserID INT,
@BusinessName NVARCHAR(250),
@RegistrationDate DATE,
@NTN NVARCHAR(9),
@NTNEvidence NVARCHAR(500),
@SalesTaxType NVARCHAR(500),
@GST NVARCHAR(500),
@GSTEvidence NVARCHAR(500),
@PRA NVARCHAR(500),
@PRAEvidence NVARCHAR(500),
@LegalStatusID INT,
@LegalStatusEvidence NVARCHAR(500),
@ProvinceID INT,
@ClusterID INT,
@DistrictID INT,
@TehsilID INT,
@GeoTagging NVARCHAR(500),
@Address NVARCHAR(500),
@ProgramTypeID INT,
@Website NVARCHAR(500)
AS
BEGIN
	IF EXISTS (SELECT
				*
			FROM SSPTSPProfile ttp
			WHERE ttp.UserID = @UserID)
	BEGIN
		UPDATE dbo.SSPTSPProfile
		SET BusinessName = @BusinessName
		   ,RegistrationDate = @RegistrationDate
		   ,NTN = @NTN
		   ,NTNEvidence = @NTNEvidence
		   ,SalesTaxType = @SalesTaxType
		   ,GST = @GST
		   ,GSTEvidence = @GSTEvidence
		   ,PRA = @PRA
		   ,PRAEvidence = @PRAEvidence
		   ,LegalStatusID = @LegalStatusID
		   ,LegalStatusEvidence = @LegalStatusEvidence
		   ,ProvinceID = @ProvinceID
		   ,ClusterID = @ClusterID
		   ,DistrictID = @DistrictID
		   ,TehsilID = @TehsilID
		   ,GeoTagging = @GeoTagging
		   ,Address = @Address
		   ,ProgramTypeID = @ProgramTypeID
		   ,Website = @Website
		   ,ModifiedDate = GETDATE()
		   ,ModifiedUserID = @UserID
		WHERE UserID = @UserID
	END
	ELSE
	BEGIN
		INSERT INTO SSPTSPProfile (BusinessName,
		UserID,
		RegistrationDate,
		NTN,
		NTNEvidence,
		SalesTaxType,
		GST,
		GSTEvidence,
		PRA,
		PRAEvidence,
		LegalStatusID,
		LegalStatusEvidence,
		ProvinceID,
		ClusterID,
		DistrictID,
		TehsilID,
		GeoTagging,
		Address,
		ProgramTypeID,
		Website,
		IsEmailVerify,
		CreatedDate,
		CreatedUserID)
			VALUES (@BusinessName, @UserID, @RegistrationDate, @NTN, @NTNEvidence, @SalesTaxType, @GST, @GSTEvidence, @PRA, @PRAEvidence, @LegalStatusID, @LegalStatusEvidence, @ProvinceID, @ClusterID, @DistrictID, @TehsilID, @GeoTagging, @Address, @ProgramTypeID, @Website, 1, GETDATE(), @UserID);
	END
	EXEC RD_SSPTSPProfile @UserID = @UserID
END -- Record N51  
GO
CREATE OR ALTER PROCEDURE AU_SSPProcessScheduleMaster @ProcessScheduleMasterID INT,
@ProgramID NVARCHAR(100),
@ProgramStartDate DATETIME,
@TotalDays INT,
@TotalProcess INT,
@InActive INT = 0,
@UserID INT
AS
	IF @ProcessScheduleMasterID = 0
	BEGIN
		INSERT INTO SSPProcessScheduleMaster (ProgramID,
		ProgramStartDate,
		TotalDays,
		TotalProcess,
		InActive,
		CreatedUserID,
		CreatedDate)
			SELECT
				@ProgramID
			   ,@ProgramStartDate
			   ,@TotalDays
			   ,@TotalProcess
			   ,0
			   ,@UserID
			   ,GETDATE()
	END
	ELSE
	BEGIN
		UPDATE SSPProcessScheduleMaster
		SET ProgramID = @ProgramID
		   ,ProgramStartDate = @ProgramStartDate
		   ,TotalDays = @TotalDays
		   ,TotalProcess = @TotalProcess
		   ,InActive = 0
		   ,ModifiedUserID = @UserID
		   ,ModifiedDate = GETDATE()
		WHERE ProcessScheduleMasterID = @ProcessScheduleMasterID
	END
	SELECT
		MAX(psm.ProcessScheduleMasterID) AS ProcessScheduleMasterID
	FROM SSPProcessScheduleMaster psm -- Record N52
GO
CREATE
OR ALTER PROCEDURE dbo.AU_SSPTSPAssociationPaymentDetail @TradeLotID INT,
@TradeLotFee DECIMAL(9, 2),
@PayProPaymentTableID INT,
@PayProPaymentCode NVARCHAR(500),
@UserID INT,
@TrainingLocationID INT,
@NoOfClasses VARCHAR(50)
AS
	INSERT INTO dbo.SSPTSPAssociationPaymentDetail (TspID,
	TradeLotID,
	TrainingLocationID,
	TradeLotFee,
	PayProPaymentTableID,
	PayProPaymentCode,
	NoOfClasses,
	IsApproved,
	IsRejected,
	InActive,
	CreatedUserID,
	CreatedDate)
		SELECT
			@UserID
		   ,@TradeLotID
		   ,@TrainingLocationID
		   ,@TradeLotFee * @NoOfClasses
		   ,@PayProPaymentTableID
		   ,@PayProPaymentCode
		   ,@NoOfClasses
		   ,0
		   ,0
		   ,0
		   ,@UserID
		   ,GETDATE() -- Record N52 
-- Record N52
GO
CREATE OR ALTER PROCEDURE RD_SSPCriteriaSubCategory @CriteriaHeaderID INT = 0
AS
	SELECT
		csc.CriteriaMainCategoryID
	   ,csc.SubCategoryTitle
	   ,csc.SubCategoryDesc
	   ,csc.MaxMarks
	   ,csc.Attachment
	   ,csc.Criteria
	FROM SSPCriteriaSubCategory csc
	WHERE (
	@CriteriaHeaderID = 0
	OR csc.CriteriaHeaderID = @CriteriaHeaderID
	)
-- Record N53  
GO
CREATE
OR ALTER PROCEDURE RD_SSPTSPAssociationSubmission @UserID INT = 0,
@ProgramID INT = 0
AS
	SELECT
		tam.CreatedUserID
	   ,tam.TradeLotID AS TradeLot
	   ,tam.TrainingLocationID AS TrainingLocation
	   ,CASE
			WHEN tpd.NoOfClasses IS NULL OR
				tpd.NoOfClasses = '' THEN '0'
			ELSE tpd.NoOfClasses
		END AS NoOfClass
	   ,ttl.TrainingLocationName AS TrainingLocationName
	   ,tam.TradeLotTitle
	   ,sp.TrainerName + ' (' + sp.TrainerCNIC + ')' AS Trainer
	   ,tam.TrainerDetailID
	   ,tpd.TradeLotFee
	   ,CASE
			WHEN tpd.NoOfClasses IS NULL OR
				tpd.NoOfClasses = '' THEN '0'
			ELSE tpd.NoOfClasses
		END AS TotalNoOfClass
	   ,CASE
			WHEN pppd.OrderStatus = '' OR
				pppd.OrderStatus IS NULL THEN 'Pending'
			ELSE pppd.OrderStatus
		END AS PaymentStatus
	   ,pppd.OrderNumber AS InvoiceNo
	   ,pppd.ConnectPayId AS [PayProCode]
	   ,CAST('false' AS BIT) AS IsChecked
	   ,ISNULL(tts.Status, 'Pending') AS EvaluationStatus
	   ,pd.ProgramName
	   ,tam.TspAssociationMasterID
	FROM SSPTspAssociationMaster tam
	INNER JOIN SSPProgramDesign pd
		ON pd.ProgramID = tam.ProgramDesignID
	INNER JOIN SSPTrainerProfileDetail spd
		ON spd.TrainerDetailID = tam.TrainerDetailID
	INNER JOIN SSPTrainerProfile sp
		ON sp.TrainerID = spd.TrainerProfileID
	INNER JOIN SSPTSPTrainingLocation ttl
		ON tam.TrainingLocationID = ttl.TrainingLocationID
			AND ttl.CreatedUserID = tam.CreatedUserID
	LEFT JOIN SSPTradeLot tl
		ON tam.TradeLotID = tl.TradeLotID
	LEFT JOIN SSPTSPAssociationPaymentDetail tpd
		ON tam.TradeLotID = tpd.TradeLotID
			AND tam.TrainingLocationID = tpd.TrainingLocationID
			AND ttl.CreatedUserID = tpd.CreatedUserID
	LEFT JOIN PayPro_PaymentDetail pppd
		ON tpd.PayProPaymentTableID = pppd.ID
	LEFT JOIN SSPTspAssociationEvaluation tae
		ON tam.TspAssociationMasterID = tae.TspAssociationMasterID
			AND tae.InActive = 0
	LEFT JOIN SSPApprovalStatus tts
		ON tts.TspTradeStatusID = tae.EvaluationStatus
	WHERE (
	@UserID = 0
	OR tam.CreatedUserID = @UserID
	)
	AND (
	@ProgramID = 0
	OR pd.ProgramID = @ProgramID
	) --AND pd.ProgramID = @ProgramID        
	AND tam.InActive = 0 -- Record N57     
-- Record N57 
-- Record N57
GO

CREATE OR ALTER PROCEDURE [dbo].[CTMCalculationReport] @FundingSourceName VARCHAR(MAX) = NULL,
@SchemeType VARCHAR(MAX) = NULL,
@FundingCategory VARCHAR(MAX) = NULL,
@ContractAwardStartDate DATE = NULL,
@ContractAwardEndDate DATE = NULL,
@Sector VARCHAR(MAX) = NULL,
@Trade VARCHAR(MAX) = NULL,
@Duration VARCHAR(MAX) = '1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24',
@District VARCHAR(MAX) = NULL,
@Cluster VARCHAR(MAX) = NULL
AS
BEGIN
	SELECT
		tr.TradeName
	   ,tr.TradeCode
	   ,C.Duration AS [Duration (Months)]
	   ,ROUND(SUM(ISNULL(C.TrainingCostPerTraineePerMonthExTax, 0) * (C.Duration * C.TraineesPerClass)) / SUM(C.Duration * C.TraineesPerClass), 0) AS [CTM Exclusive SalesTax]
	   ,ROUND(SUM(ISNULL(C.TrainingCostPerTraineePerMonthInTax, 0) * (C.Duration * C.TraineesPerClass)) / SUM(C.Duration * C.TraineesPerClass), 0) AS [CTM Inclusive SalesTax]
	FROM Class AS C
	LEFT JOIN ClassStatus AS CS
		ON CS.ClassStatusID = C.ClassStatusID
	INNER JOIN Scheme AS S
		ON S.SchemeID = C.SchemeID
	INNER JOIN ProgramType AS PT
		ON PT.PTypeID = S.ProgramTypeID
	INNER JOIN TSPDetail AS T
		ON T.TspID = C.TspID
	LEFT JOIN dbo.TSPMaster AS TSPM
		ON T.TSPMasterID = TSPM.TSPMasterID
	INNER JOIN EducationTypes edt
		ON edt.EducationTypeID = C.EntryQualification
	INNER JOIN FundingSource FS
		ON FS.FundingSourceID = S.FundingSourceID
	INNER JOIN Trade tr
		ON tr.TradeID = C.TradeID
	INNER JOIN District d
		ON d.DistrictID = C.DistrictID
	INNER JOIN Gender g
		ON g.GenderID = C.GenderID
	INNER JOIN Cluster cl
		ON cl.ClusterID = C.ClusterID
	INNER JOIN Sector se
		ON se.SectorID = C.SectorID
	INNER JOIN CertificationAuthority cer
		ON cer.CertAuthID = C.CertAuthID
	INNER JOIN STRING_SPLIT(@Duration, ',')
		ON VALUE = C.Duration
	WHERE S.FinalSubmitted = 1
	AND S.IsApproved = 1
	AND S.IsMigrated = 0
	AND S.ProgramTypeID <> 9
	AND (S.ContractAwardDate BETWEEN @ContractAwardStartDate AND @ContractAwardEndDate)
	AND (S.FundingSourceID IN (SELECT
			*
		FROM STRING_SPLIT(@FundingSourceName, ','))
	OR @FundingSourceName IS NULL)
	AND (S.FundingCategoryID IN (SELECT
			*
		FROM STRING_SPLIT(@FundingCategory, ','))
	OR @FundingCategory IS NULL)
	AND (
	S.ProgramTypeID IN (SELECT
			*
		FROM STRING_SPLIT(@SchemeType, ','))
	OR @SchemeType IS NULL
	)
	--AND (S.ContractAwardDate IN (SELECT value
	--	FROM STRING_SPLIT(@ContractAwardDate, ',')) OR @ContractAwardDate IS NULL
	--)
	AND (
	C.SectorID IN (SELECT
			*
		FROM STRING_SPLIT(@Sector, ','))
	OR @Sector IS NULL
	)
	AND (
	C.TradeID IN (SELECT
			*
		FROM STRING_SPLIT(@Trade, ','))
	OR @Trade IS NULL
	)
	AND (
	C.DistrictID IN (SELECT
			*
		FROM STRING_SPLIT(@District, ','))
	OR @District IS NULL
	)
	AND (
	cl.ClusterID IN (SELECT
			VALUE
		FROM STRING_SPLIT(@Cluster, ','))
	OR @Cluster IS NULL
	)
	GROUP BY tr.TradeName
			,tr.TradeCode
			,C.Duration
	ORDER BY tr.TradeName, tr.TradeCode
END
-- Record N58  

GO
CREATE OR ALTER PROCEDURE [dbo].[RD_SSPCheckTSPNTNName] @TSPNTN NVARCHAR(MAX) = '',
@TSPName NVARCHAR(MAX) = ''
AS
BEGIN --@TSPCount = 1: NTN exists  
	--@TSPCount = 2: NTN and Name exist  
	DECLARE @TSPCount INT = 0
	IF EXISTS (SELECT
				1
			FROM TSPMaster t
			WHERE t.NTN = @TSPNTN
			UNION ALL
			SELECT
				1
			FROM SSPTSPProfile ttp
			WHERE ttp.NTN = @TSPNTN
			AND ttp.IsEmailVerify = 1)
	BEGIN
		SET @TSPCount = 1
	END
	IF EXISTS (SELECT
				1
			FROM TSPMaster t
			WHERE t.TSPName = @TSPName
			AND t.NTN = @TSPNTN
			UNION ALL
			SELECT
				1
			FROM SSPTSPProfile ttp
			WHERE ttp.BusinessName = @TSPName
			AND ttp.NTN = @TSPNTN
			AND ttp.IsEmailVerify = 1)
	BEGIN
		SET @TSPCount = @TSPCount + 1
	END
	SELECT
		@TSPCount AS Result
END -- Record N56
GO
CREATE OR ALTER PROCEDURE [dbo].[RD_SSPProgramDesign] (@InActive BIT = NULL, @ProgramID INT = NULL)
AS
BEGIN
	SELECT
		p.ProgramID
	   ,p.ProgramName AS Program
	   ,ISNULL(p.IsSubmitted, 0) IsSubmitted
	   ,fs.FundingSourceID
	   ,g.GenderID
	   ,e.EducationTypeID AS MinEducationID
	   ,et.EducationTypeID AS MaxEducationID
	   ,p.ProgramTypeID AS ProgramTypeID
	   ,p.FundingSourceID AS FundingSourceID
	   ,p.FundingCategoryID
	   ,p.BusinessRuleType
	   ,fc.FundingCategoryName
	   ,ss.SAP_SchemeID AS PaymentStructureID
	   ,p.ApplicabilityID AS ApplicabilityIDs
	   ,p.MinimumEducation AS MinEducationID
	   ,p.MaximumEducation AS MaxEducationID
	   ,cfy.ID AS FinancialYearID
	   ,CAST(p.PlanningType AS INT) AS PlaningTypeID
	   ,p.ProgramCode
	   ,pt1.PlaningType
	   ,CONVERT(VARCHAR(10), cfy.FromDate, 120) + ' TO ' + CONVERT(VARCHAR(10), cfy.ToDate, 120) AS FinancialYear
	   ,pt.PTypeName AS ProgramType
	   ,fs.FundingSourceName AS FundingSource
	   ,g.GenderName AS Gender
	   ,e.Education AS MinEducation
	   ,et.Education AS MaxEducation
	   ,p.MinAge
	   ,p.MaxAge
	   ,p.PaymentSchedule AS PaymentStructureID
	   ,ss.SchemeCode + '-' + ss.Description AS PaymentStructure
	   ,p.ProgramDescription AS Description
	   ,p.Stipend
	   ,p.StipendMode
	   ,p.SelectionMethod AS SelectionMethodIDs
	   ,(SELECT
				STRING_AGG(sm.MethodName, ',')
			FROM SSPSelectionMethods sm
			WHERE sm.ID IN (SELECT
					value
				FROM STRING_SPLIT(p.SelectionMethod, ',')))
		AS SelectionMethod
	   ,p.EmploymentCommitment
	   ,p.SchemeDesignOn
	   ,CASE
			WHEN p.Province IS NULL OR
				p.Province = '' THEN '---'
			ELSE (SELECT
						STRING_AGG(ProvinceName, ',')
					FROM Province p1
					WHERE p1.ProvinceID IN (SELECT
							value
						FROM STRING_SPLIT(p.Province, ',')))
		END ProposedProvince
	   ,CASE
			WHEN p.Cluster IS NULL OR
				p.Cluster = '' THEN '---'
			ELSE (SELECT
						STRING_AGG(ClusterName, ',')
					FROM Cluster c
					WHERE c.ClusterID IN (SELECT
							value
						FROM STRING_SPLIT(p.Cluster, ',')))
		END ProposedCluster
	   ,CASE
			WHEN p.District IS NULL OR
				p.District = '' THEN '---'
			ELSE (SELECT
						STRING_AGG(DistrictName, ',')
					FROM District d
					WHERE d.DistrictID IN (SELECT
							value
						FROM STRING_SPLIT(p.District, ',')))
		END ProposedDistrict
	   ,p.Province AS ProvinceIDs
	   ,p.Cluster AS ClusterIDs
	   ,p.District AS DistrictIDs
	   ,p.ApprovalAttachment AS ApprovalEvidence
	   ,p.TORsAttachment AS AttachmentTORsEvidence
	   ,p.CriteriaAttachment AS AttachmentCriteriaEvidence
	   ,p.ApprovalAttachment
	   ,p.TORsAttachment AS AttachmentTORs
	   ,p.CriteriaAttachment AS AttachmentCriteria
	   ,CAST(p.bagBadgeCost AS INT) AS TraineeSupportCost
	   ,p.TentativeProcessStart AS TentativeProcessSDate
	   ,p.ClassStartDate
	   ,p.FinancialYear AS FinancialYearID
	   ,p.ApprovalDescription AS ApprovalRecDetail
	FROM SSPProgramDesign p
	LEFT JOIN ProgramType pt
		ON pt.PTypeID = p.ProgramTypeID
	LEFT JOIN FundingSource fs
		ON fs.FundingSourceID = p.FundingSourceID
	INNER JOIN FundingCategory fc
		ON p.FundingCategoryID = fc.FundingCategoryID
	LEFT JOIN Gender g
		ON g.GenderID = p.GenderID
	LEFT JOIN EducationTypes e
		ON e.EducationTypeID = p.MinimumEducation
	LEFT JOIN EducationTypes et
		ON et.EducationTypeID = p.MaximumEducation
	LEFT JOIN SSPPlaningType pt1
		ON pt1.PlaningTypeID = p.PlanningType
	LEFT JOIN CustomFinancialYears cfy
		ON cfy.ID = p.FinancialYear
	LEFT JOIN SAP_Scheme ss
		ON ss.SAP_SchemeID = p.PaymentSchedule
	WHERE (
	@InActive IS NULL
	OR p.InActive = @InActive
	)
	ORDER BY p.ProgramID DESC
END
GO
CREATE OR ALTER PROCEDURE [dbo].[RD_SSPTSPRegistrationDetailReportFiltered] @InActive BIT = 0,
@UserID INT = 0,
@TradeID INT = 0,
@ProvinceID INT = 0,
@ClusterID INT = 0,
@DistrictID INT = 0
AS
BEGIN
	SELECT
		ttp.BusinessName AS TspName
	   ,ttp.UserID AS TspID
	   ,t.TradeName
	   ,t.TradeID
	   ,p.ProvinceName AS Province
	   ,p.ProvinceID
	   ,c.ClusterName AS Cluster
	   ,c.ClusterID
	   ,d.DistrictName AS District
	   ,d.DistrictID
	   ,ra.RegistrationAuthorityName AS RegistrationAuthority
	   ,tc.ExpiryDate
	   ,ttm.TrainingDuration AS [TrainingDuration (Months)]
	   ,ttm.ClassCapacityMor AS 'ClassCapacityMorning'
	   ,ttm.ClassCapacityEve AS 'ClassCapacityEvening'
	   ,ttsh.ProcurementRemarks AS 'ProcurementRemarks'
	   ,CASE
			WHEN (
				tts.Status IS NULL OR
				tts.Status = '' OR
				tts.Status = 'Send-Back'
				) AND
				(ttm.ApprovalLevel = 0) THEN 'Pending'
			WHEN (
				tts.Status IS NULL OR
				tts.Status = ''
				) AND
				(ttm.ApprovalLevel = 1) THEN 'In-Progress'
			ELSE tts.Status
		END Status
	FROM SSPTSPProfile ttp
	LEFT JOIN SSPTSPTrainingLocation ttl
		ON ttl.CreatedUserID = ttp.UserID
	INNER JOIN SSPTSPTradeManage ttm
		ON ttm.TrainingLocationID = ttl.TrainingLocationID
	LEFT JOIN SSPTrainingCertification tc
		ON tc.TrainingCertificationID = ttm.CertificateID
	LEFT JOIN Trade t
		ON ttm.TradeID = t.TradeID
	LEFT JOIN Sector s
		ON t.SectorID = s.SectorID
	LEFT JOIN SubSector ss
		ON t.SubSectorID = ss.SubSectorID
	LEFT JOIN Province p
		ON ttl.Province = p.ProvinceID
	LEFT JOIN Cluster c
		ON ttl.Cluster = c.ClusterID
	LEFT JOIN District d
		ON ttl.District = d.DistrictID
	LEFT JOIN Tehsil t1
		ON ttl.Tehsil = t1.TehsilID
	LEFT JOIN SSPTSPTradeStatusHistory ttsh
		ON ttm.TradeManageID = ttsh.TradeManageID
			AND ttsh.InActive = 0
	LEFT JOIN SSPApprovalStatus tts
		ON tts.TspTradeStatusID = ttsh.StatusID
	LEFT JOIN RegistrationAuthority ra
		ON ra.RegistrationAuthorityID = ttl.RegistrationAuthority
	WHERE --ttp.InActive = @InActive   AND   
	(
	@UserID = 0
	OR ttp.UserID = @UserID
	)
	AND (
	@TradeID = 0
	OR t.TradeID = @TradeID
	)
	AND (
	@ProvinceID = 0
	OR p.ProvinceID = @ProvinceID
	)
	AND (
	@ClusterID = 0
	OR c.ClusterID = @ClusterID
	)
	AND (
	@DistrictID = 0
	OR d.DistrictID = @DistrictID
	) -- ORDER BY ttm.ClassCapacityMor      
END -- Record N62  
GO
CREATE OR ALTER PROCEDURE AU_SSPTSPAssignment @TSPAssignmentID INT = 0,
@ProgramID INT,
@TradeLotID INT,
@TrainingLocationID INT = NULL,
@TspAssociationEvaluationID INT = NULL,
@TSPID INT,
@UserID INT
AS
BEGIN
	IF @TSPAssignmentID = 0
	BEGIN
		INSERT INTO SSPTSPAssignment (ProgramID,
		TradeLotID,
		TrainingLocationID,
		AssociationEvaluationID,
		TspID,
		InActive,
		CreatedUserID,
		CreatedDate)
			VALUES (@ProgramID, @TradeLotID, @TrainingLocationID, @TspAssociationEvaluationID, @TSPID, 0, @UserID, GETDATE());
	END
	ELSE
	BEGIN
		UPDATE SSPTSPAssignment
		SET ProgramID = @ProgramID
		   ,TradeLotID = @TradeLotID
		   ,TrainingLocationID = @TrainingLocationID
		   ,AssociationEvaluationID = @TspAssociationEvaluationID
		   ,TspID = @TSPID
		   ,[InActive] = 0
		   ,ModifiedUserID = @UserID
		   ,ModifiedDate = GETDATE()
		WHERE TSPAssignmentID = @TSPAssignmentID;
	END
END;
-- Record N63
GO
CREATE OR ALTER PROCEDURE dbo.AU_SSPTspTrainingLocation @UserID INT,
@TrainingLocationID INT,
@TrainingLocationName NVARCHAR(100),
@FrontMainEntrancePhoto NVARCHAR(500),
@ClassroomPhoto NVARCHAR(500),
@PracticalAreaPhoto NVARCHAR(500),
@ComputerLabPhoto NVARCHAR(500),
@ToolsAndEquipmentsPhoto NVARCHAR(500),
@Province INT,
@Cluster INT,
@District INT,
@Tehsil INT,
@TrainingLocationAddress VARCHAR(500),
@GeoTagging VARCHAR(500),
@RegistrationAuthority INT
AS
BEGIN
	IF @TrainingLocationID = 0
	BEGIN
		INSERT INTO dbo.SSPTSPTrainingLocation (TrainingLocationName,
		TspProfileID,
		Province,
		Cluster,
		District,
		Tehsil,
		TrainingLocationAddress,
		GeoTagging,
		RegistrationAuthority,
		FrontMainEntrancePhoto,
		ClassroomPhoto,
		PracticalAreaPhoto,
		ComputerLabPhoto,
		ToolsAndEquipmentsPhoto,
		InActive,
		CreatedUserID,
		CreatedDate)
			VALUES (@TrainingLocationName, @UserID, @Province, @Cluster, @District, @Tehsil, @TrainingLocationAddress, @GeoTagging, @RegistrationAuthority, @FrontMainEntrancePhoto, @ClassroomPhoto, @PracticalAreaPhoto, @ComputerLabPhoto, @ToolsAndEquipmentsPhoto, 0, @UserID, GETDATE())
	END
	IF @TrainingLocationID <> 0
	BEGIN
		UPDATE dbo.SSPTSPTrainingLocation
		SET TrainingLocationName = @TrainingLocationName
		   ,Province = @Province
		   ,Cluster = @Cluster
		   ,District = @District
		   ,Tehsil = @Tehsil
		   ,TrainingLocationAddress = @TrainingLocationAddress
		   ,GeoTagging = @GeoTagging
		   ,RegistrationAuthority = @RegistrationAuthority
		   ,FrontMainEntrancePhoto = @FrontMainEntrancePhoto
		   ,ClassroomPhoto = @ClassroomPhoto
		   ,PracticalAreaPhoto = @PracticalAreaPhoto
		   ,ComputerLabPhoto = @ComputerLabPhoto
		   ,ToolsAndEquipmentsPhoto = @ToolsAndEquipmentsPhoto
		   ,ModifiedUserID = @UserID
		   ,ModifiedDate = GETDATE()
		WHERE TrainingLocationID = @TrainingLocationID
		AND TspProfileID = @UserID
	END
	EXEC RD_SSPTspTrainingLocation @UserID = @UserID
END -- Record N64 
-- Record N64
GO
CREATE OR ALTER PROCEDURE dbo.RD_SSPTspTradeValidationStatus
AS
BEGIN
	SELECT
		*
	FROM SSPTspTradeStatus
	WHERE InActive = 0
END -- Record N65
GO
CREATE OR ALTER PROCEDURE RD_SSPCheckTradePlanExisted @ProgramDesignOn NVARCHAR(200) = 'District',
@ProgramID INT = 0,
@LocationID INT = 0,
@ProgramFocusID INT = 0,
@TradeDetailMapID INT = 0
AS
BEGIN
	SELECT
		tl.TradeLotID
	   ,pd.ProgramName AS Program
	   ,pd.ProgramName AS ProgramFocus
	   ,'(' + CAST(d1.Duration AS NVARCHAR(20)) + ' | ' + soc.Name + ')' + t.TradeName AS Trade
	   ,td.ProgramDesignOn
	   ,CASE
			WHEN td.ProgramDesignOn = 'Province' THEN p.ProvinceName
			WHEN td.ProgramDesignOn = 'Cluster' THEN c.ClusterName
			ELSE d.DistrictName
		END SelectedProgramDesignOn
	FROM SSPTradeLot tl
	LEFT JOIN SSPTradeDesign td
		ON tl.TradeDesignID = td.TradeDesignID
	LEFT JOIN Province p
		ON tl.ProvinceID = p.ProvinceID
	LEFT JOIN Cluster c
		ON tl.ClusterID = c.ClusterID
	LEFT JOIN District d
		ON tl.DistrictID = d.DistrictID
	LEFT JOIN ProgramFocus pf
		ON td.ProgramFocusID = pf.ProgramFocusID
	LEFT JOIN SSPProgramDesign pd
		ON pd.ProgramID = td.ProgramDesignID
	LEFT JOIN Trade t
		ON tl.TradeID = t.TradeID
	LEFT JOIN TradeDetailMap tdm
		ON td.TradeDetailMapID = tdm.TradeDetailMapID
	LEFT JOIN Duration d1
		ON tdm.DurationID = d1.DurationID
	LEFT JOIN SourceOfCurriculum soc
		ON tdm.SourceOfCurriculumID = soc.SourceOfCurriculumID
	WHERE td.ProgramDesignID = @ProgramID
	AND td.ProgramFocusID = @ProgramFocusID
	AND td.TradeDetailMapID = @TradeDetailMapID
	AND (
	(
	@ProgramDesignOn = 'Province'
	AND tl.ProvinceID = @LocationID
	)
	OR (
	@ProgramDesignOn = 'Cluster'
	AND tl.ClusterID = @LocationID
	)
	OR (
	@ProgramDesignOn = 'District'
	AND tl.DistrictID = @LocationID
	)
	)
	ORDER BY tl.TradeLotID ASC
END;
GO -- Record N62
CREATE OR ALTER PROCEDURE AU_SSPTradeDesign @TradeDesignID INT,
@ProgramDesignID INT,
@ProvinceID NVARCHAR(MAX) = 0,
@ClusterID NVARCHAR(MAX) = 0,
@DistrictID NVARCHAR(MAX) = 0,
@ProgramFocusID INT,
@TradeID INT,
@TradeDetailMapID INT,
@CTM INT,
@ExamCost INT,
@DropOutPerAge INT,
@TraineeContractedTarget INT,
@TraineeCompTarget INT,
@GenderID INT,
@ProgramDesignOn NVARCHAR(500) = 'District',
@SelectedShortList NVARCHAR(MAX),
@SelectedShortListCount INT,
@PerSelectedContraTarget INT,
@PerSelectedCompTarget INT,
@UserID INT
AS
BEGIN
	IF @TradeDesignID > 0
	BEGIN
		DELETE FROM SSPTradeLot
		WHERE TradeDesignID = @TradeDesignID
	END
	IF @TradeDesignID = 0
	BEGIN
		INSERT INTO dbo.SSPTradeDesign (ProgramDesignID,
		GenderID,
		ProvinceID,
		ClusterID,
		DistrictID,
		SelectedShortList,
		SelectedCount,
		ProgramDesignOn,
		ProgramFocusID,
		TradeID,
		TradeDetailMapID,
		CTM,
		ExamCost,
		DropOutPerAge,
		TraineeContractedTarget,
		TraineeCompTarget,
		PerSelectedContraTarget,
		PerSelectedCompTarget,
		InActive,
		CreatedUserID,
		CreatedDate)
			VALUES (@ProgramDesignID, @GenderID, @ProvinceID, @ClusterID, @DistrictID, @SelectedShortList, @SelectedShortListCount, @ProgramDesignOn, @ProgramFocusID, @TradeID, @TradeDetailMapID, @CTM, @ExamCost, @DropOutPerAge, @TraineeContractedTarget, @TraineeCompTarget, @PerSelectedContraTarget, @PerSelectedCompTarget, 0, @UserID, GETDATE())
	END
	IF @TradeDesignID > 0
	BEGIN
		UPDATE dbo.SSPTradeDesign
		SET ProgramDesignID = @ProgramDesignID
		   ,GenderID = @GenderID
		   ,ProvinceID = @ProvinceID
		   ,ClusterID = @ClusterID
		   ,DistrictID = @DistrictID
		   ,ProgramDesignOn = @ProgramDesignOn
		   ,SelectedCount = @SelectedShortListCount
		   ,SelectedShortList = @SelectedShortList
		   ,ProgramFocusID = @ProgramFocusID
		   ,TradeID = @TradeID
		   ,TradeDetailMapID = @TradeDetailMapID
		   ,CTM = @CTM
		   ,ExamCost = @ExamCost
		   ,DropOutPerAge = @DropOutPerAge
		   ,TraineeContractedTarget = @TraineeContractedTarget
		   ,TraineeCompTarget = @TraineeCompTarget
		   ,PerSelectedContraTarget = @PerSelectedContraTarget
		   ,PerSelectedCompTarget = @PerSelectedCompTarget
		   ,ModifiedUserID = @UserID
		   ,ModifiedDate = GETDATE()
		WHERE TradeDesignID = @TradeDesignID
	END
	EXEC RD_SSPTradeDesign
END -- Record N66
GO
CREATE
OR ALTER PROCEDURE RD_SSPProcessSchedule @ProcessKey NVARCHAR(MAX) = ''
AS
	SELECT
		ps.ProcessID
	   ,ps.ProcessName
	   ,ps.DurationDays
	   ,CASE
			WHEN CAST(ps.ProcessEndDate AS DATE) >= CAST(GETDATE() AS DATE) THEN 'NO'
			ELSE 'YES'
		END IsLocked
	FROM SSPProcessSchedule ps
	WHERE ps.InActive = 0
	AND ps.ProcessID IN (8, 9)
	AND (
	@ProcessKey = ''
	OR ps.ProcessName = @ProcessKey
	) -- Record N67
GO
CREATE OR ALTER PROCEDURE [dbo].[RD_SSPWorkflowTask]
AS
BEGIN
	SELECT
		wt.TaskID
	   ,wt.WorkflowID
	   ,wt.TaskName
	   ,wt.TaskDays
	   ,wt.TaskApproval
	   ,wt.TaskStatus
	FROM SSPWorkflowTask wt
END -- Record N68
GO
CREATE OR ALTER PROCEDURE dbo.RD_SSPTspTrainingLocation @UserID INT = 0
AS
BEGIN
	SELECT
		tl.TrainingLocationID
	   ,tl.TrainingLocationName
	   ,tl.Province
	   ,tl.Cluster
	   ,tl.District
	   ,tl.Tehsil
	   ,tl.TrainingLocationAddress
	   ,tl.GeoTagging
	   ,tl.RegistrationAuthority
	   ,p.ProvinceName
	   ,c.ClusterName
	   ,d.DistrictName
	   ,t.TehsilName
	   ,ra.RegistrationAuthorityName
	   ,tl.FrontMainEntrancePhoto
	   ,tl.ClassroomPhoto
	   ,tl.PracticalAreaPhoto
	   ,tl.ComputerLabPhoto
	   ,tl.ToolsAndEquipmentsPhoto
	   ,tl.FrontMainEntrancePhoto AS FrontMainEntrance
	   ,tl.ClassroomPhoto AS Classroom
	   ,tl.PracticalAreaPhoto AS PracticalArea
	   ,tl.ComputerLabPhoto AS ComputerLab
	   ,tl.ToolsAndEquipmentsPhoto AS ToolsAndEquipments
	   ,tl.CreatedUserID AS UserID
	   ,ISNULL(pppd.OrderStatus, 'UNPAID') AS PaymentStatus
	FROM SSPTSPTrainingLocation tl
	INNER JOIN Province p
		ON p.ProvinceID = tl.Province
	INNER JOIN Cluster c
		ON c.ClusterID = tl.Cluster
	INNER JOIN District d
		ON d.DistrictID = tl.District
	INNER JOIN Tehsil t
		ON t.TehsilID = tl.Tehsil
	INNER JOIN RegistrationAuthority ra
		ON ra.RegistrationAuthorityID = tl.RegistrationAuthority
	LEFT JOIN SSPTSPRegistrationPaymentDetail spd
		ON tl.TrainingLocationID = spd.TrainingLocationID
	LEFT JOIN PayPro_PaymentDetail pppd
		ON pppd.ID = spd.PayProPaymentTableID
			AND pppd.OrderStatus = 'PAID'
	WHERE (
	@UserID = 0
	OR tl.CreatedUserID = @UserID
	)
	ORDER BY FrontMainEntrancePhoto DESC
END -- Record N35          
-- Record N35    
GO
CREATE OR ALTER PROCEDURE AU_SSPProgramCriteriaHistory @ID INT,
@ProgramID INT,
@CriteriaID INT,
@Remarks NVARCHAR(2000),
@StartDate DATETIME,
@EndDate DATETIME,
@IsInactive BIT = 0,
@UserID INT
AS
	UPDATE SSPProgramDesign
	SET CriteriaID = @CriteriaID
	WHERE ProgramID = @ProgramID
	AND IsFinalApproved = 1
	AND IsFinalApproved = 1
	UPDATE SSPProgramCriteriaHistory
	SET IsInactive = 1
	WHERE ProgramID = @ProgramID
	IF @ID = 0
	BEGIN
		INSERT INTO dbo.SSPProgramCriteriaHistory (ProgramID,
		CriteriaID,
		Remarks,
		StartDate,
		EndDate,
		IsInactive,
		CreatedUserID,
		CreatedDate)
			SELECT
				@ProgramID
			   ,@CriteriaID
			   ,@Remarks
			   ,@StartDate
			   ,@EndDate
			   ,@IsInactive
			   ,@UserID
			   ,GETDATE()
	END
	ELSE
	BEGIN
		UPDATE dbo.SSPProgramCriteriaHistory
		SET ProgramID = @ProgramID
		   ,CriteriaID = @CriteriaID
		   ,Remarks = @Remarks
		   ,StartDate = @StartDate
		   ,EndDate = @EndDate
		   ,IsInactive = @IsInactive
		   ,ModifiedUserID = @UserID
		   ,ModifiedDate = GETDATE()
		WHERE ID = @ID
	END -- Record N70
GO
CREATE OR ALTER PROCEDURE AU_SSPProgramDesignFinalApproval @ProgramID INT = 0
AS
	UPDATE SSPProgramDesign
	SET IsFinalApproved = 1
	WHERE ProgramID = @ProgramID -- Record N71
GO
CREATE OR ALTER PROCEDURE AU_SSPProgramWorkflowHistory @ID INT = 0,
@ProgramID INT,
@WorkflowID INT,
@UserID INT,
@Remarks NVARCHAR(2000),
@IsInactive BIT = 0
AS
	UPDATE SSPProgramDesign
	SET WorkflowID = @WorkflowID
	WHERE ProgramID = @ProgramID
	AND IsFinalApproved = 1
	AND IsFinalApproved = 1
	UPDATE SSPProgramWorkflowHistory
	SET IsInactive = 1
	WHERE ProgramID = @ProgramID
	IF @ID = 0
	BEGIN
		INSERT INTO dbo.SSPProgramWorkflowHistory (ProgramID,
		WorkflowID,
		Remarks,
		IsInactive,
		CreatedUserID,
		CreatedDate)
			SELECT
				@ProgramID
			   ,@WorkflowID
			   ,@Remarks
			   ,@IsInactive
			   ,@UserID
			   ,GETDATE()
	END
	ELSE
	BEGIN
		UPDATE dbo.SSPProgramWorkflowHistory
		SET ProgramID = @ProgramID
		   ,WorkflowID = @WorkflowID
		   ,Remarks = @Remarks
		   ,IsInactive = @IsInactive
		   ,ModifiedUserID = @UserID
		   ,ModifiedDate = GETDATE()
		WHERE ID = @ID
	END -- Record N69
GO
CREATE
OR ALTER PROCEDURE AU_SSPTSPAssociationMaster @TspAssociationMasterID INT = 0,
@ProgramDesignID INT,
@TrainingLocationID INT,
@TradeLotID INT,
@TrainerDetailID INT,
@TradeLotTitle NVARCHAR(500),
@UserID INT
AS
	IF @TspAssociationMasterID = 0
	BEGIN
		INSERT INTO dbo.SSPTspAssociationMaster (ProgramDesignID,
		TrainingLocationID,
		TradeLotID,
		TrainerDetailID,
		TradeLotTitle,
		InActive,
		CreatedUserID,
		CreatedDate)
			SELECT
				@ProgramDesignID
			   ,@TrainingLocationID
			   ,@TradeLotID
			   ,@TrainerDetailID
			   ,@TradeLotTitle
			   ,0
			   ,@UserID
			   ,GETDATE()
	END
	ELSE
	BEGIN
		UPDATE dbo.SSPTspAssociationMaster
		SET ProgramDesignID = @ProgramDesignID
		   ,TrainingLocationID = @TrainingLocationID
		   ,TrainerDetailID = @TrainerDetailID
		   ,TradeLotID = @TradeLotID
		   ,TradeLotTitle = @TradeLotTitle
		   ,InActive = 0
		   ,ModifiedUserID = @UserID
		   ,ModifiedDate = GETDATE()
		WHERE TspAssociationMasterID = @TspAssociationMasterID
	END
	SELECT TOP 1
		tam.TspAssociationMasterID
	FROM SSPTspAssociationMaster tam
	ORDER BY tam.TspAssociationMasterID DESC -- Record N73 
-- Record N73
GO
CREATE OR ALTER PROCEDURE dbo.AU_SSPCriteriaHeader 
    @CriteriaHeaderID INT = 0,
    @HeaderTitle NVARCHAR(150) = NULL,
    @HeaderDesc NVARCHAR(150),
    @IsMarking NVARCHAR(150),
    @MaxMarks NVARCHAR(150),
    @UserID INT,
    @IsSubmitted BIT
AS
BEGIN
    SET NOCOUNT ON;

    IF @CriteriaHeaderID = 0
    BEGIN
        -- Insert new record into SSPCriteriaHeader
        INSERT INTO dbo.SSPCriteriaHeader 
        (
            HeaderTitle,
            HeaderDesc,
            IsMarking,
            MaxMarks,
            IsSubmitted,
            InActive,
            CreatedUserID,
            CreatedDate
        )
        VALUES 
        (
            @HeaderTitle, 
            @HeaderDesc, 
            @IsMarking, 
            @MaxMarks, 
            @IsSubmitted, 
            0, 
            @UserID, 
            GETDATE()
        );

        IF @IsSubmitted = 1  
        BEGIN  
            -- Insert into ApprovalHistory if submitted
            INSERT INTO dbo.ApprovalHistory 
            (
                ProcessKey,  
                Step,  
                FormID,  
                ApproverID,  
                Comments,  
                ApprovalStatusID,  
                CreatedUserID,  
                ModifiedUserID,  
                CreatedDate,  
                ModifiedDate,  
                InActive
            )  
            VALUES 
            (
                'CRTEM_APP',         -- ProcessKey
                1,                   -- Step
                SCOPE_IDENTITY(),    -- FormID
                NULL,                -- ApproverID
                N'Pending',          -- Comments
                1,                   -- ApprovalStatusID
                @UserID,             -- CreatedUserID
                NULL,                -- ModifiedUserID
                GETDATE(),           -- CreatedDate
                NULL,                -- ModifiedDate
                0                    -- InActive
            );  
        END
    END
    ELSE
    BEGIN
        -- Update existing record in SSPCriteriaHeader
        UPDATE dbo.SSPCriteriaHeader
        SET 
            HeaderTitle = @HeaderTitle,
            HeaderDesc = @HeaderDesc,
            IsMarking = @IsMarking,
            MaxMarks = @MaxMarks,
            IsSubmitted = @IsSubmitted,
            ModifiedUserID = @UserID,
            ModifiedDate = GETDATE()
        WHERE 
            CriteriaHeaderID = @CriteriaHeaderID;

        IF @IsSubmitted = 1  
        BEGIN  
            -- Insert into ApprovalHistory if submitted
            INSERT INTO dbo.ApprovalHistory 
            (
                ProcessKey,  
                Step,  
                FormID,  
                ApproverID,  
                Comments,  
                ApprovalStatusID,  
                CreatedUserID,  
                ModifiedUserID,  
                CreatedDate,  
                ModifiedDate,  
                InActive
            )  
            VALUES 
            (
                'CRTEM_APP',         -- ProcessKey
                1,                   -- Step
                @CriteriaHeaderID,   -- FormID
                NULL,                -- ApproverID
                N'Pending',          -- Comments
                1,                   -- ApprovalStatusID
                @UserID,             -- CreatedUserID
                NULL,                -- ModifiedUserID
                GETDATE(),           -- CreatedDate
                NULL,                -- ModifiedDate
                0                    -- InActive
            );  
        END
    END

    -- Return the last inserted or updated record
    SELECT TOP 1 *
    FROM dbo.SSPCriteriaHeader
    ORDER BY CriteriaHeaderID DESC;
END;


GO
CREATE OR ALTER PROCEDURE dbo.AU_SSPCriteriaSubCategory @CriteriaSubCategoryID INT,
@CriteriaHeaderID INT,
@CriteriaMainCategoryID INT,
@SubCategoryTitle NVARCHAR(150),
@SubCategoryDesc NVARCHAR(150),
@Criteria NVARCHAR(150),
@MarkedCriteria NVARCHAR(150),
@IsMandatory NVARCHAR(150),
@MaxMarks NVARCHAR(150),
@Attachment NVARCHAR(500),
@UserID INT
AS
	IF @CriteriaSubCategoryID = 0
	BEGIN
		INSERT INTO dbo.SSPCriteriaSubCategory (CriteriaHeaderID,
		CriteriaMainCategoryID,
		SubCategoryTitle,
		SubCategoryDesc,
		Criteria,
		MarkedCriteria,
		IsMandatory,
		MaxMarks,
		Attachment,
		InActive,
		CreatedUserID,
		CreatedDate)
			VALUES (@CriteriaHeaderID, @CriteriaMainCategoryID, @SubCategoryTitle, @SubCategoryDesc, @Criteria, @MarkedCriteria, @IsMandatory, @MaxMarks, @Attachment, 0, @UserID, GETDATE())
	END
	ELSE
	BEGIN
		UPDATE dbo.SSPCriteriaSubCategory
		SET CriteriaHeaderID = @CriteriaHeaderID
		   ,CriteriaMainCategoryID = @CriteriaMainCategoryID
		   ,SubCategoryTitle = @SubCategoryTitle
		   ,SubCategoryDesc = @SubCategoryDesc
		   ,Criteria = @Criteria
		   ,MarkedCriteria = @MarkedCriteria
		   ,IsMandatory = @IsMandatory
		   ,MaxMarks = @MaxMarks
		   ,Attachment = @Attachment
		   ,ModifiedUserID = @UserID
		   ,ModifiedDate = GETDATE()
		WHERE CriteriaSubCategoryID = @CriteriaSubCategoryID
	END
	SELECT
		*
	FROM SSPCriteriaSubCategory csc -- Record N75  
GO
CREATE
OR ALTER PROCEDURE dbo.AU_SSPSignUp @BusinessName NVARCHAR(250),
@NTN NVARCHAR(9),
@TspEmail NVARCHAR(150),
@TspContact NVARCHAR(12),
@TspPwd NVARCHAR(500),
@Office NVARCHAR(20),
@OTPCode NVARCHAR(10)
AS
BEGIN
	BEGIN TRY
		BEGIN TRANSACTION;
		DECLARE @UserID INT
			   ,@TspID INT
			   ,@UserName NVARCHAR(100);
		SELECT
			@UserName = LTRIM(RTRIM(LEFT(REPLACE(@BusinessName, ' ', ''), 5))) + '-' + RIGHT(
			'000' + CAST(ISNULL(MAX(ttp.UserID), 0) + 1 AS NVARCHAR(5)),
			4
			)
		FROM SSPUsers ttp;
		INSERT INTO SSPUsers (UserName,
		FullName,
		Email,
		RoleID,
		UserLevel,
		ContactNo,
		InActive,
		CreatedUserID,
		CreatedDate,
		ModifiedDate,
		UserImage)
			VALUES (@UserName, @BusinessName, @TspEmail, 23, 4, @TspContact, 0, 0, GETDATE(), NULL, N'');
		SET @UserID = SCOPE_IDENTITY();
		--Insert into SSPUsersPwd        
		INSERT INTO SSPUsersPwd (UserPassword,
		UserID,
		InActive,
		CreatedUserID,
		CreatedDate,
		FailedLoginAttempt)
			VALUES (@TspPwd, @UserID, 0, 0, GETDATE(), 0);
		--Insert into dbo.SSPTSPProfile        
		INSERT INTO dbo.SSPTSPProfile (BusinessName,
		NTN,
		TspEmail,
		IsEmailVerify,
		TspContact,
		UserID,
		Office,
		OTPCode,
		CreatedDate,
		CreatedUserID,
		InActive)
			VALUES (@BusinessName, @NTN, @TspEmail, 0, @TspContact, @UserID, @Office, @OTPCode, GETDATE(), @UserID, 0);
		SET @TspID = SCOPE_IDENTITY();
		COMMIT TRANSACTION;
		SELECT
			ttp.TspID
		   ,ttp.TspEmail
		FROM SSPTSPProfile ttp
		WHERE ttp.TspID = @TspID;
	END TRY
	BEGIN CATCH
		IF @@trancount > 0
			ROLLBACK TRANSACTION;
		--Throw a custom error message along with original error details        
		DECLARE @ErrorMessage NVARCHAR(2048) = 'An error occurred during the SignUp process. Error Details: ' + ERROR_MESSAGE() + ' (Error ' + CAST(ERROR_NUMBER() AS NVARCHAR) + ')';
		THROW 51000,
		@ErrorMessage,
		1;
	END CATCH;
END -- Record N76  
-- Record N76
GO
CREATE OR ALTER PROCEDURE dbo.AU_SSPTSPRegistrationPaymentDetail @TrainingLocationID INT,
@TrainingLocationFee DECIMAL(9, 2),
@PayProPaymentTableID INT,
@PayProPaymentCode NVARCHAR(500),
@UserID INT
AS
	INSERT INTO dbo.SSPTSPRegistrationPaymentDetail (TspID,
	TrainingLocationID,
	TrainingLocationFee,
	PayProPaymentTableID,
	PayProPaymentCode,
	IsApproved,
	IsRejected,
	InActive,
	CreatedUserID,
	CreatedDate)
		SELECT
			@UserID
		   ,@TrainingLocationID
		   ,@TrainingLocationFee
		   ,@PayProPaymentTableID
		   ,@PayProPaymentCode
		   ,0
		   ,0
		   ,0
		   ,@UserID
		   ,GETDATE() -- Record N77
GO
CREATE OR ALTER PROCEDURE dbo.AU_SSPWorkflow @WorkflowID INT,
@WorkflowTitle NVARCHAR(100),
@SourcingTypeID INT,
@Description NVARCHAR(500),
@TotalDays NVARCHAR(500),
@TotalTaskDays NVARCHAR(500),
@UserID INT
AS
	IF @WorkflowID = 0
	BEGIN
		INSERT INTO dbo.SSPWorkflow (WorkflowTitle,
		SourcingTypeID,
		Description,
		TotalDays,
		TotalTaskDays,
		InActive,
		CreatedUserID,
		CreatedDate)
			VALUES (@WorkflowTitle, @SourcingTypeID, @Description, @TotalDays, @TotalTaskDays, 0, @UserID, GETDATE())
	END
	ELSE
	BEGIN
		UPDATE dbo.SSPWorkflow
		SET WorkflowTitle = @WorkflowTitle
		   ,SourcingTypeID = @SourcingTypeID
		   ,Description = @Description
		   ,TotalDays = @TotalDays
		   ,TotalTaskDays = @TotalTaskDays
		   ,ModifiedUserID = @UserID
		   ,ModifiedDate = GETDATE()
		WHERE WorkflowID = @WorkflowID
	END
	SELECT
		*
	FROM SSPWorkflow w
	ORDER BY w.WorkflowID DESC -- Record N78  
GO
CREATE OR ALTER PROCEDURE dbo.AU_SSPWorkflowTask @TaskID INT,
@WorkflowID INT,
@TaskName NVARCHAR(100),
@TaskDays INT,
@TaskApproval NVARCHAR(500),
@TaskStatus NVARCHAR(500),
@InActive BIT = 0,
@UserID INT
AS
	IF @TaskID = 0
	BEGIN
		INSERT INTO dbo.SSPWorkflowTask (WorkflowID,
		TaskName,
		TaskDays,
		TaskApproval,
		TaskStatus,
		InActive,
		CreatedUserID,
		CreatedDate)
			VALUES (@WorkflowID, @TaskName, @TaskDays, @TaskApproval, @TaskStatus, 0, @UserID, GETDATE())
	END
	ELSE
	BEGIN
		UPDATE dbo.SSPWorkflowTask
		SET WorkflowID = @WorkflowID
		   ,TaskName = @TaskName
		   ,TaskDays = @TaskDays
		   ,TaskApproval = @TaskApproval
		   ,TaskStatus = @TaskStatus
		   ,ModifiedUserID = @UserID
		   ,ModifiedDate = GETDATE()
		WHERE TaskID = @TaskID
	END -- Record N79
GO
CREATE OR ALTER PROCEDURE dbo.RD_SSPSalesTax @InActive BIT = 0
AS
BEGIN
	SELECT
		stt.SalesTaxID
	   ,stt.SalesTaxType
	FROM SSPSalesTaxType stt
	WHERE (
	@InActive = 0
	OR stt.InActive = @InActive
	)
END -- Record N81
GO
CREATE OR ALTER PROCEDURE RD_SSPCriteriaMainCategory @CriteriaHeaderID INT = 0
AS
BEGIN
	SELECT
		cmc.CriteriaMainCategoryID AS CriteriaMainCategoryID
	   ,cmc.CriteriaHeaderID AS CriteriaTemplateID
	   ,cmc.MainCategoryTitle AS CategoryTitle
	   ,cmc.MainCategoryDesc AS Description
	   ,cmc.TotalMarks AS TotalMarks
	   ,'' AS Evidence
	   ,'' AS Remarks
	FROM SSPCriteriaMainCategory cmc
	WHERE (
	@CriteriaHeaderID = 0
	OR cmc.CriteriaHeaderID = @CriteriaHeaderID
	)
END -- Record N82
GO
CREATE OR ALTER PROCEDURE RD_SSPCriteriaTemplate
AS
BEGIN
	SELECT
		ch.CriteriaHeaderID AS CriteriaTemplateID
	   ,ch.HeaderTitle AS CriteriaTemplateTitle
	   ,CASE
			WHEN ch.IsApproved IS NULL AND
				ch.IsRejected IS NULL THEN 'Pending'
			WHEN ch.IsApproved = 1 THEN 'Approved'
			WHEN ch.IsRejected = 1 THEN 'Rejected'
		END ApprovalStatus
	   ,ch.HeaderDesc AS Description
	   ,ch.IsMarking AS MarkingRequired
	   ,ch.MaxMarks AS MaximumMarks
	   ,ISNULL(ch.IsSubmitted, 0) IsSubmitted
	   ,(SELECT
				COUNT(*)
			FROM SSPProgramDesign sd
			WHERE sd.CriteriaID = ch.CriteriaHeaderID)
		AS AttachedProgramCount
	FROM SSPCriteriaHeader ch
END


GO
CREATE OR ALTER PROCEDURE RD_SSPPayProMaxOrderNumber
AS
	SELECT
		MAX(OrderNumber) + 1 AS OrderNumber
	FROM PayPro_PaymentDetail
	WHERE ID = (SELECT
			MAX(ID)
		FROM PayPro_PaymentDetail pppd) -- Record N84
GO
CREATE OR ALTER PROCEDURE RD_SSPTradeByProgramAndDistrict @ProgramDesignOn NVARCHAR(200) = 'District',
@ProgramID INT = 0,
@LocationID INT = 0
AS
	SELECT
		tl.TradeLotID
	   ,td.TradeID
	   ,'(' + CAST(d1.Duration AS NVARCHAR(20)) + ' Month | ' + soc.Name + ') ' + t.TradeName AS Trade
	   ,td.ProgramDesignID AS ProgramID
	   ,td.TradeDetailMapID
	   ,p.ProvinceName
	   ,c.ClusterName
	   ,d.DistrictName
	   ,d.DistrictID
	FROM SSPTradeLot tl
	LEFT JOIN SSPTradeDesign td
		ON tl.TradeDesignID = td.TradeDesignID
	LEFT JOIN Trade t
		ON tl.TradeID = t.TradeID
			AND t.InActive = 0
	LEFT JOIN Province p
		ON tl.ProvinceID = p.ProvinceID
	LEFT JOIN Cluster c
		ON tl.ClusterID = c.ClusterID
	LEFT JOIN District d
		ON d.DistrictID = tl.DistrictID
	INNER JOIN TradeDetailMap tdm
		ON tdm.TradeDetailMapID = td.TradeDetailMapID
	INNER JOIN Duration d1
		ON d1.DurationID = tdm.DurationID
	INNER JOIN SourceOfCurriculum soc
		ON tdm.SourceOfCurriculumID = soc.SourceOfCurriculumID
	WHERE td.ProgramDesignID = @ProgramID
	AND (
	(
	@ProgramDesignOn = 'Province'
	AND tl.ProvinceID = @LocationID
	)
	OR (
	@ProgramDesignOn = 'Cluster'
	AND tl.ClusterID = @LocationID
	)
	OR (
	@ProgramDesignOn = 'District'
	AND tl.DistrictID = @LocationID
	)
	) -- Record N85  
GO
CREATE OR ALTER PROCEDURE RD_SSPTSPAssignment
AS
	SELECT
		pd.ProgramName AS Program
	   ,pd.SchemeDesignOn AS ProgramDesignOn
	   ,c.ClusterID
	   ,ttp.BusinessName + ' (' + ttl.TrainingLocationName + ')' AS TSP
	   ,'( ' + CAST(d1.Duration AS NVARCHAR(20)) + ' Month | ' + soc.Name + ' )' + t1.TradeName AS Trade
	   ,t1.TradeCode
	   ,tae.MarksBasedOnEvaluation
	   ,tae.CategoryBasedOnEvaluation
	   ,tae.TotalCapacity
	   ,tae.VerifiedCapacityMorning
	   ,tae.VerifiedCapacityEvening
	   ,p.ProvinceName
	   ,c.ClusterName
	   ,d.DistrictName
	FROM SSPTSPAssignment t
	LEFT JOIN SSPTspAssociationEvaluation tae
		ON t.AssociationEvaluationID = tae.TspAssociationEvaluationID
	LEFT JOIN SSPTspAssociationMaster tam
		ON tae.TspAssociationMasterID = tam.TspAssociationMasterID
	LEFT JOIN SSPTSPProfile ttp
		ON ttp.UserID = tam.CreatedUserID
	INNER JOIN SSPProgramDesign pd
		ON t.ProgramID = pd.ProgramID
	LEFT JOIN SSPTradeLot tl
		ON t.TradeLotID = tl.TradeLotID
	LEFT JOIN SSPTradeDesign td
		ON tl.TradeDesignID = td.TradeDesignID
	LEFT JOIN SSPTSPTrainingLocation ttl
		ON t.TrainingLocationID = ttl.TrainingLocationID
	LEFT JOIN Province p
		ON ttl.Province = p.ProvinceID
	LEFT JOIN Cluster c
		ON ttl.Cluster = c.ClusterID
	LEFT JOIN District d
		ON ttl.District = d.DistrictID
	LEFT JOIN Trade t1
		ON td.TradeID = t1.TradeID
	LEFT JOIN TradeDetailMap tdm
		ON td.TradeDetailMapID = tdm.TradeDetailMapID
	LEFT JOIN Duration d1
		ON tdm.DurationID = d1.DurationID
	LEFT JOIN SourceOfCurriculum soc
		ON tdm.SourceOfCurriculumID = soc.SourceOfCurriculumID
	WHERE tae.InActive = 0 -- Record N87
GO
CREATE OR ALTER PROCEDURE [dbo].[SSP_CheckUserPass] @UserPassword NVARCHAR(50),
@UserName NVARCHAR(50),
@PassCount INT OUTPUT
AS
BEGIN
	SELECT
		@PassCount = COUNT(u.UserID)
	FROM SSPUsersPwd up
	INNER JOIN SSPUsers u
		ON up.UserID = u.UserID
	WHERE u.UserName = @UserName
	AND up.UserPassword = @UserPassword;
END -- Record N88  
GO
CREATE OR ALTER PROCEDURE dbo.AU_SSPTrainerProfileDetail @UserID INT,
@TrainerDetailID INT,
@TrainerProfileID INT,
@TrainerTradeID INT,
@ProfQualification NVARCHAR(100),
@CertificateBody NVARCHAR(100),
@RelExpYear FLOAT,
@RelExpLetter NVARCHAR(500),
@ProfQualEvidence NVARCHAR(500)
AS
BEGIN
	IF @TrainerDetailID = 0
	BEGIN
		INSERT INTO dbo.SSPTrainerProfileDetail (TrainerProfileID,
		TrainerTradeID,
		ProfQualification,
		ProfQualEvidence,
		CertificateBody,
		RelExpYear,
		RelExpLetter,
		TspProfileID,
		InActive,
		CreatedUserID,
		CreatedDate)
			VALUES (@TrainerProfileID, @TrainerTradeID, @ProfQualification, @ProfQualEvidence, @CertificateBody, @RelExpYear, @RelExpLetter, @UserID, 0, @UserID, GETDATE())
	END
	IF @TrainerDetailID <> 0
	BEGIN
		UPDATE dbo.SSPTrainerProfileDetail
		SET TrainerProfileID = @TrainerProfileID
		   ,TrainerTradeID = @TrainerTradeID
		   ,ProfQualification = @ProfQualification
		   ,CertificateBody = @CertificateBody
		   ,RelExpYear = @RelExpYear
		   ,RelExpLetter = @RelExpLetter
		   ,ProfQualEvidence = @ProfQualEvidence
		   ,ModifiedUserID = @UserID
		   ,ModifiedDate = GETDATE()
		WHERE TrainerDetailID = @TrainerDetailID
	END
	EXEC RD_SSPTrainerProfileDetail @UserID = @UserID
END -- Record N89  
GO
CREATE OR ALTER PROCEDURE dbo.AU_SSPTspTradeValidationStatus @UserID INT,
@TradeManageID INT,
@ProcurementRemarks NVARCHAR(200) = NULL,
@StatusID INT,
@ApprovalLevel INT
AS
BEGIN
	DECLARE @TspUserID INT;
	UPDATE SSPTSPTradeStatusHistory
	SET InActive = 1
	WHERE TradeManageID = @TradeManageID
	UPDATE SSPTSPTradeManage
	SET ApprovalLevel = @ApprovalLevel
	WHERE TradeManageID = @TradeManageID
	INSERT INTO SSPTSPTradeStatusHistory (ProcurementRemarks,
	ApprovalLevel,
	TradeManageID,
	StatusID,
	InActive,
	Step,
	CreatedUserID,
	CreatedDate)
		VALUES (@ProcurementRemarks, @ApprovalLevel, @TradeManageID, @StatusID, 0, 0, @UserID, GETDATE());
	SELECT
		@TspUserID = ttm.CreatedUserID
	FROM SSPTSPTradeManage ttm
	WHERE ttm.TradeManageID = @TradeManageID
	EXEC RD_SSPTSPRegistrationDetail @UserID = @TspUserID
END -- Record N90
GO
CREATE OR ALTER PROCEDURE dbo.Delete_TrainerDetail @TrainerDetailID INT
AS
BEGIN
	DECLARE @TrainerProfileID INT = 0
	SELECT
		@TrainerProfileID = tpd.TrainerProfileID
	FROM SSPTrainerProfileDetail tpd
	WHERE tpd.TrainerDetailID = @TrainerDetailID
	DELETE FROM SSPTrainerProfileDetail
	WHERE TrainerDetailID = @TrainerDetailID
	EXEC RD_SSPTrainerProfileDetail @UserID = 0
								   ,@TrainerProfileID = @TrainerProfileID
END -- Record N91
GO
CREATE OR ALTER PROCEDURE dbo.RD_SSPTrainerDetail @TrainerProfileID INT
AS
BEGIN
	SELECT
		tpd.*
	   ,tp.TrainerName
	   ,t.TradeName
	FROM SSPTrainerProfileDetail tpd
	INNER JOIN SSPTrainerProfile tp
		ON tpd.TspProfileID = tp.TspProfileID
	INNER JOIN Trade t
		ON t.TradeID = tpd.TrainerTradeID
	WHERE tpd.TrainerProfileID = @TrainerProfileID
END -- Record N92
GO
CREATE OR ALTER PROCEDURE dbo.RD_SSPTrainerProfileDetail @UserID INT = 0,
@TrainerProfileID INT = 0
AS
BEGIN
	SELECT
		tpd.TrainerDetailID
	   ,tpd.TrainerTradeID
	   ,tpd.TrainerProfileID
	   ,tpd.ProfQualEvidence
	   ,tpd.ProfQualification
	   ,tpd.ProfQualEvidence AS QualificationEvidence
	   ,tpd.CertificateBody
	   ,tpd.RelExpYear
	   ,tpd.RelExpLetter
	   ,tpd.RelExpLetter AS RelationalExpLetter
	   ,t.TradeName
	   ,sp.TrainerName + ' (' + sp.TrainerCNIC + ')' AS TrainerName
	FROM SSPTrainerProfileDetail tpd
	INNER JOIN Trade t
		ON t.TradeID = tpd.TrainerTradeID
	INNER JOIN SSPTrainerProfile sp
		ON sp.TrainerID = tpd.TrainerProfileID
	WHERE (
	@TrainerProfileID = 0
	OR tpd.TrainerProfileID = @TrainerProfileID
	)
	AND (
	@UserID = 0
	OR tpd.CreatedUserID = @UserID
	)
END --EXEC RD_SSPTrainerProfileDetail @UserID =15    
-- Record N93 
-- Record N93
GO
CREATE OR ALTER PROCEDURE dbo.RD_SSPTrainingCertification @UserID INT
AS
BEGIN
	SELECT
		ttl.TrainingLocationName
	   ,ra.RegistrationAuthorityName
	   ,tc.TrainingCertificationID
	   ,tc.TrainingLocationID
	   ,tc.RegistrationAuthority
	   ,tc.RegistrationStatus
	   ,ss.RegistrationStatus AS RegistrationCertificateStatus
	   ,CASE
			WHEN tc.RegistrationCerNum IS NULL OR
				tc.RegistrationCerNum = '' THEN '---'
			ELSE tc.RegistrationCerNum
		END RegistrationCerNum
	   ,CASE
			WHEN tc.ExpiryDate IS NULL OR
				tc.ExpiryDate = '' THEN '---'
			ELSE tc.ExpiryDate
		END ExpiryDate
	   ,tc.IssuanceDate
	   ,tc.RegistrationCerEvidence
	   ,tc.RegistrationCerEvidence AS RegistrationCertificateEvidence
 ,(SELECT
				COUNT(1)
			FROM SSPTSPTradeManage sm
			LEFT JOIN SSPTSPTradeStatusHistory ssh
				ON sm.TradeManageID = ssh.TradeManageID
			WHERE sm.CertificateID = tc.TrainingCertificationID
			AND ssh.StatusID = 4
			AND ssh.InActive = 0)
		TotalMappedTrade
	FROM SSPTrainingCertification tc
	INNER JOIN SSPTSPTrainingLocation ttl
		ON tc.TrainingLocationID = ttl.TrainingLocationID
	INNER JOIN RegistrationAuthority ra
		ON ra.RegistrationAuthorityID = tc.RegistrationAuthority
	INNER JOIN SSPRegistrationStatus ss
		ON tc.RegistrationStatus = ss.RegistrationStatusID
	WHERE tc.CreatedUserID = @UserID
END

-- Record N94

GO
CREATE
OR ALTER PROCEDURE dbo.RD_SSPTspRegistrationMaster
AS
BEGIN
	SELECT
		ttp.UserID
	   ,ttp.BusinessName AS TspName
	   ,ttp.RegistrationDate
	   ,(SELECT
				COUNT(*)
			FROM SSPTSPTradeManage ttmmm
			INNER JOIN SSPTSPRegistrationPaymentDetail tpd1
				ON ttmmm.TrainingLocationID = tpd1.TrainingLocationID
			WHERE ttmmm.CreatedUserID = ttp.UserID
			AND (
			ttmmm.ApprovalLevel = 0
			OR ttmmm.ApprovalLevel IS NULL
			))
		AS PendingTrade
	   ,(SELECT
				COUNT(*)
			FROM SSPTSPTradeManage ttmm
			INNER JOIN SSPTSPRegistrationPaymentDetail tpd1
				ON ttmm.TrainingLocationID = tpd1.TrainingLocationID
			WHERE ttmm.CreatedUserID = ttp.UserID --AND ttm.ApprovalLevel = 0  
		)
		AS TotalTrade
	   ,COUNT(DISTINCT ttl.District) AS TotalDistrict
	   ,COUNT(DISTINCT ttl.TrainingLocationID) AS TrainingLocation
	   ,COUNT(DISTINCT tc.TrainingCertificationID) AS TotalCertificate
	FROM SSPTSPProfile ttp
	INNER JOIN SSPTSPTrainingLocation ttl
		ON ttl.CreatedUserID = ttp.UserID
	INNER JOIN SSPTSPRegistrationPaymentDetail tpd
		ON ttl.TrainingLocationID = tpd.TrainingLocationID
			AND ttp.UserID = tpd.CreatedUserID
	INNER JOIN PayPro_PaymentDetail pppd
		ON pppd.ID = tpd.PayProPaymentTableID
	INNER JOIN SSPTSPTradeManage ttm
		ON ttm.CreatedUserID = ttp.UserID
	INNER JOIN SSPTrainingCertification tc
		ON tc.CreatedUserID = ttp.UserID
	WHERE ttp.IsEmailVerify = 1
	AND ttp.RegistrationDate IS NOT NULL
	AND pppd.OrderStatus = 'PAID'
	GROUP BY ttp.BusinessName
			,ttp.RegistrationDate
			,ttp.UserID
END -- Record N95  
-- Record N95
GO
CREATE OR ALTER PROCEDURE dbo.RD_SSPTspRegistrationMaster_old
AS
BEGIN
	SELECT
		ttp.BusinessName AS TspName
	   ,ttp.UserID
	   ,(SELECT
				COUNT(ttm.ApprovalLevel)
			FROM SSPTSPTradeManage ttm
			WHERE ttm.CreatedUserID = ttp.UserID
			AND ttm.ApprovalLevel = 0)
		AS PendingTrade
	   ,ttp.RegistrationDate
	   ,COUNT(DISTINCT ttl.District) AS TotalDistrict
	   ,COUNT(DISTINCT ttl.TrainingLocationID) AS TrainingLocation
	   ,COUNT(DISTINCT ttm.TradeManageID) AS TotalTrades
	   ,COUNT(DISTINCT tc.TrainingCertificationID) AS TotalCertificate
	FROM SSPTSPProfile ttp
	LEFT JOIN SSPTSPTrainingLocation ttl
		ON ttl.CreatedUserID = ttp.UserID
	LEFT JOIN SSPTSPTradeManage ttm
		ON ttm.CreatedUserID = ttp.UserID
	LEFT JOIN SSPTrainingCertification tc
		ON tc.CreatedUserID = ttp.UserID
	WHERE ttp.IsEmailVerify = 1
	AND ttp.RegistrationDate IS NOT NULL --AND ttp.InActive = 0   
	GROUP BY ttp.BusinessName
			,ttp.RegistrationDate
			,ttp.UserID;
END -- Record N96
GO
CREATE OR ALTER PROCEDURE RD_SSPActiveProgram
AS
BEGIN
	SELECT
		pd.ProgramID AS ProgramID
	   ,pd.ProgramName AS Program
	   ,(SELECT
				COUNT(1)
			FROM SSPTspAssociationMaster sam
			WHERE sam.ProgramDesignID = pd.ProgramID)
		TotalAssociation
	   ,pd.SchemeDesignOn AS ProgramDesignOn
	   ,CASE
			WHEN pch.EndDate >= GETDATE() THEN 'Active'
			ELSE 'InActive'
		END AS ProgramStatus
	   ,FORMAT(pch.StartDate, 'yyyy-MM-dd HH:mm') AS AssociationStartDate
	   ,FORMAT(pch.EndDate, 'yyyy-MM-dd HH:mm') AS AssociationEndDate
	   ,DATEDIFF(DAY, pch.StartDate, pch.EndDate) + 1 AS TotalDays
	   ,ISNULL(pch.Remarks, '') AS Detail
	   ,pd.CriteriaID
	FROM SSPProgramDesign pd
	LEFT JOIN SSPProgramCriteriaHistory pch
		ON pd.ProgramID = pch.ProgramID
			AND pch.IsInactive = 0
	WHERE pd.IsFinalApproved = 1
	AND pch.CriteriaID <> 0
	AND pch.CriteriaID IS NOT NULL
	ORDER BY pch.ProgramID DESC
END;
-- Record N98 
-- Record N98
GO
CREATE OR ALTER PROCEDURE [dbo].[SSPLoginAttempt] @UserName NVARCHAR(50),
@UserPassword NVARCHAR(50),
@LoginAttempt INT OUTPUT
AS
BEGIN
	DECLARE @UserID INT;
	DECLARE @CheckFailedLoginAttempt INT;
	SELECT
		@UserID = u.UserID
	   ,@CheckFailedLoginAttempt = up.FailedLoginAttempt
	FROM SSPUsers AS u
	INNER JOIN SSPUsersPwd up
		ON u.UserID = up.UserID
			AND up.InActive = 0
	WHERE u.UserName = @UserName;
	--Check if the user exists and is active        
	IF @UserID IS NOT NULL
	BEGIN
		SET @LoginAttempt = 6;
		--Check user's password        
		IF EXISTS (SELECT
					1
				FROM SSPUsersPwd AS up
				WHERE up.UserID = @UserID
				AND up.UserPassword = @UserPassword
				AND up.InActive = 0)
		BEGIN --Successful login, reset failed login attempts       
			IF @CheckFailedLoginAttempt > 0
			BEGIN
				UPDATE SSPUsersPwd
				SET FailedLoginAttempt = 0
				   ,ModifiedDate = GETDATE()
				   ,ModifiedUserID = @UserID
				WHERE UserID = @UserID
				AND InActive = 0
				AND FailedLoginAttempt <> 0
			END
		END
		ELSE
		BEGIN --Failed login, increment the failed login attempt count        
			UPDATE SSPUsersPwd
			SET FailedLoginAttempt = ISNULL(FailedLoginAttempt, 0) + 1
			   ,ModifiedDate = GETDATE()
			   ,ModifiedUserID = @UserID
			WHERE UserID = @UserID
			AND InActive = 0;
		END --Get the updated failed login attempt count        
		SELECT
			@LoginAttempt = ISNULL(FailedLoginAttempt, 0)
		FROM SSPUsersPwd AS up
		WHERE up.UserID = @UserID
		AND up.InActive = 0;
	--Lock the account if the threshold is reached        
	--IF @LoginAttempt = 5        
	--BEGIN        
	--   UPDATE SSPUsersPwd        
	--   SET InActive = 1        
	--   WHERE UserID = @UserID        
	--   AND InActive = 0;        
	--END        
	END
	ELSE
	BEGIN
		SET @LoginAttempt = 7;
	END
END -- Record N99
GO
CREATE OR ALTER PROCEDURE dbo.RD_SSPTrainerProfile @UserID INT
AS
BEGIN
	SELECT
		tp.TrainerID
	   ,tp.TrainerName
	   ,tp.TrainerMobile
	   ,tp.TrainerEmail
	   ,tp.Gender
	   ,tp.TrainerCNIC
	   ,tp.CnicFrontPhoto
	   ,tp.CnicBackPhoto
	   ,tp.CnicFrontPhoto AS FrontCNIC
	   ,tp.CnicBackPhoto AS BackCNIC
	   ,tp.Qualification
	   ,tp.QualEvidence
	   ,tp.QualEvidence AS QualificationEvidence
	   ,tp.TrainerCV
	   ,tp.TrainerCV AS TrainerResume
	   ,tp.CreatedDate
	   ,et.Education
	   ,g.GenderName AS GenderName
	FROM SSPTrainerProfile tp
	INNER JOIN EducationTypes et
		ON et.EducationTypeID = tp.Qualification
	INNER JOIN Gender g
		ON tp.Gender = g.GenderID
	WHERE tp.CreatedUserID = @UserID
END -- Record N100
GO
CREATE OR ALTER PROCEDURE dbo.AU_SSPCriteriaMainCategory @CriteriaMainCategoryID INT,
@CriteriaHeaderID INT,
@MainCategoryTitle NVARCHAR(150),
@MainCategoryDesc NVARCHAR(150),
@TotalMarks NVARCHAR(150),
@UserID INT
AS
	IF @CriteriaMainCategoryID = 0
	BEGIN
		INSERT INTO dbo.SSPCriteriaMainCategory (CriteriaHeaderID,
		MainCategoryTitle,
		MainCategoryDesc,
		TotalMarks,
		InActive,
		CreatedUserID,
		CreatedDate)
			VALUES (@CriteriaHeaderID, @MainCategoryTitle, @MainCategoryDesc, @TotalMarks, 0, @UserID, GETDATE())
	END
	ELSE
	BEGIN
		UPDATE dbo.SSPCriteriaMainCategory
		SET CriteriaHeaderID = @CriteriaHeaderID
		   ,MainCategoryTitle = @MainCategoryTitle
		   ,MainCategoryDesc = @MainCategoryDesc
		   ,TotalMarks = @TotalMarks
		   ,ModifiedUserID = @UserID
		   ,ModifiedDate = GETDATE()
		WHERE CriteriaMainCategoryID = @CriteriaMainCategoryID
	END
	SELECT TOP 1
		CriteriaMainCategoryID
	FROM SSPCriteriaMainCategory
	ORDER BY CriteriaMainCategoryID DESC -- Record N102  
GO
CREATE OR ALTER PROCEDURE AU_SSPTspAssociationDetail @TspAssociationDetailID INT,
@TspAssociationMasterID INT,
@CriteriaMainCategoryID INT,
@Attachment NVARCHAR(500),
@Remarks NVARCHAR(500),
@InActive BIT = 0,
@UserID INT
AS
	IF @TspAssociationDetailID = 0
	BEGIN
		INSERT INTO dbo.SSPTspAssociationDetail (TspAssociationMasterID,
		CriteriaMainCategoryID,
		Attachment,
		Remarks,
		InActive,
		CreatedUserID,
		CreatedDate)
			SELECT
				@TspAssociationMasterID
			   ,@CriteriaMainCategoryID
			   ,@Attachment
			   ,@Remarks
			   ,@InActive
			   ,@UserID
			   ,GETDATE()
	END
	ELSE
	BEGIN
		UPDATE dbo.SSPTspAssociationDetail
		SET TspAssociationMasterID = @TspAssociationMasterID
		   ,CriteriaMainCategoryID = @CriteriaMainCategoryID
		   ,Attachment = @Attachment
		   ,Remarks = @Remarks
		   ,InActive = @InActive
		   ,ModifiedUserID = @UserID
		   ,ModifiedDate = GETDATE()
		WHERE TspAssociationDetailID = @TspAssociationDetailID
	END -- Record N103
GO
CREATE OR ALTER PROCEDURE dbo.AU_SSPProcessSchedule @ProcessName VARCHAR(500),
@ProcessStartDate DATETIME,
@ProcessEndDate DATETIME,
@ProcessDays INT,
@ProgramName NVARCHAR(500),
@programStartDate DATETIME,
@Inactive BIT = 0,
@UserID INT
AS
	INSERT INTO dbo.ProcessSchedule (ProcessName,
	ProcessStartDate,
	ProcessEndDate,
	DurationDays,
	ProgramName,
	ProgramStartDate,
	InActive,
	CreatedUserID,
	CreatedDate)
		SELECT
			@ProcessName
		   ,@ProcessStartDate
		   ,@ProcessEndDate
		   ,@ProcessDays
		   ,@ProgramName
		   ,@programStartDate
		   ,@Inactive
		   ,@UserID
		   ,GETDATE() -- Record N105
GO
CREATE OR ALTER PROCEDURE RD_SSPAssociationFee
AS
	SELECT TOP 1
		ta.AssociationFee
	   ,ta.AssociationFeeID
	FROM SSPTSPAssociationFee ta
	WHERE ta.InActive = 0 -- Record N106
GO
CREATE OR ALTER PROCEDURE RD_SSPRegistrationFee
AS
	SELECT TOP 1
		tf.RegistrationFee
	   ,tf.RegistrationFeeID
	FROM SSPTSPRegistrationFee tf
	WHERE tf.InActive = 0 -- Record N107
GO
CREATE OR ALTER PROCEDURE RD_SSPSourcingType
AS
	SELECT
		st.SourcingTypeID
	   ,st.SourcingType
	FROM SSPSourcingType st -- Record N108
GO
CREATE OR ALTER PROCEDURE RD_SSPTradeDesignByTradeLot @TradeLotID INT = 0
AS
	SELECT
		tl.TradeLotID
	   ,d1.Duration
	   ,tdm.WeeklyTrainingHours
	   ,ca.CertAuthName AS CertificationAuthority
	   ,soc.Name AS SourceOfCurriculum
	   ,et.Education AS TraineeQualification
	   ,et1.Education AS TrainerQualification
	   ,pf.ProgramFocusName AS ProgramFocus
	   ,td.TraineeContractedTarget AS ContractedTarget
	   ,td.TraineeCompTarget AS CompletionTarget
	   ,td.CTM
	FROM SSPTradeLot tl
	LEFT JOIN SSPTradeDesign td
		ON tl.TradeDesignID = td.TradeDesignID
	LEFT JOIN ProgramFocus pf
		ON td.ProgramFocusID = pf.ProgramFocusID
	LEFT JOIN Trade t
		ON tl.TradeID = t.TradeID
			AND t.InActive = 0
	LEFT JOIN Province p
		ON tl.ProvinceID = p.ProvinceID
	LEFT JOIN Cluster c
		ON tl.ClusterID = c.ClusterID
	LEFT JOIN District d
		ON d.DistrictID = tl.DistrictID
	INNER JOIN TradeDetailMap tdm
		ON tdm.TradeDetailMapID = td.TradeDetailMapID
	INNER JOIN Duration d1
		ON d1.DurationID = tdm.DurationID
	INNER JOIN SourceOfCurriculum soc
		ON tdm.SourceOfCurriculumID = soc.SourceOfCurriculumID
	INNER JOIN CertificationAuthority ca
		ON tdm.CertAuthID = ca.CertAuthID
	INNER JOIN EducationTypes et
		ON et.EducationTypeID = tdm.TraineeEducationTypeID
	INNER JOIN EducationTypes et1
		ON et1.EducationTypeID = tdm.TrainerEducationTypeID
	WHERE tl.TradeLotID = @TradeLotID -- Record N111
GO
CREATE OR ALTER PROCEDURE Remove_SSPWorkflowTask @TaskID INT
AS
	DELETE FROM SSPWorkflowTask
	WHERE TaskID = @TaskID -- Record N112

GO
CREATE OR ALTER PROCEDURE dbo.AU_SSPTrainerProfile @UserID INT,
@TrainerID INT,
@TrainerName NVARCHAR(500),
@TrainerMobile NVARCHAR(12),
@TrainerEmail NVARCHAR(500),
@Gender INT,
@TrainerCNIC NVARCHAR(20),
@CnicFrontPhoto NVARCHAR(500),
@CnicBackPhoto NVARCHAR(500),
@Qualification INT,
@QualEvidence NVARCHAR(500),
@TrainerCV NVARCHAR(500)
AS
BEGIN
	IF @TrainerID = 0
	BEGIN
		INSERT INTO dbo.SSPTrainerProfile (TrainerName,
		TrainerMobile,
		TrainerEmail,
		Gender,
		TrainerCNIC,
		CnicFrontPhoto,
		CnicBackPhoto,
		Qualification,
		QualEvidence,
		TrainerCV,
		TspProfileID,
		InActive,
		CreatedUserID,
		CreatedDate)
			VALUES (@TrainerName, @TrainerMobile, @TrainerEmail, @Gender, @TrainerCNIC, @CnicFrontPhoto, @CnicBackPhoto, @Qualification, @QualEvidence, @TrainerCV, @UserID, 0, @UserID, GETDATE())
	END
	IF @TrainerID <> 0
	BEGIN
		UPDATE dbo.SSPTrainerProfile
		SET TrainerName = @TrainerName
		   ,TrainerMobile = @TrainerMobile
		   ,TrainerEmail = @TrainerEmail
		   ,Gender = @Gender
		   ,TrainerCNIC = @TrainerCNIC
		   ,CnicFrontPhoto = @CnicFrontPhoto
		   ,CnicBackPhoto = @CnicBackPhoto
		   ,Qualification = @Qualification
		   ,QualEvidence = @QualEvidence
		   ,TrainerCV = @TrainerCV
		   ,ModifiedUserID = @UserID
		   ,ModifiedDate = GETDATE()
		WHERE TrainerID = @TrainerID
	END
	EXEC RD_SSPTrainerProfile @UserID = @UserID
END -- Record N116
GO
CREATE OR ALTER PROCEDURE RD_SSPProgramTSP @ProgramID INT,
@District INT,
@TradeLotID INT
AS
BEGIN
	SELECT
		t.TspID
	   ,ttp.BusinessName
	FROM SSPTSPAssignment t
	INNER JOIN SSPTSPProfile ttp
		ON ttp.TspID = t.TspID
	INNER JOIN SSPTSPTrainingLocation ttl
		ON ttl.TrainingLocationID = t.TrainingLocationID
	WHERE t.ProgramID = @ProgramID
	AND ttl.District = @District
	AND t.TradeLotID = @TradeLotID
END -- Record N117
GO
CREATE OR ALTER PROCEDURE RD_SSPTradeLotByDesignID @TradeDesignID INT
AS
BEGIN
	SELECT
		*
	FROM SSPTradeLot
	WHERE TradeDesignID = @TradeDesignID;
END;
-- Record N118
GO
CREATE OR ALTER PROCEDURE RD_SSPLegalStatus
AS
BEGIN
	SELECT
		*
	FROM SSPLegalStatus
	WHERE InActive = 0
END -- Record N119
GO
CREATE OR ALTER PROCEDURE RD_SSPPlaningType @InActive BIT = NULL
AS
BEGIN
	SELECT
		*
	FROM SSPPlaningType
	WHERE InActive = 0
END -- Record N120
GO
CREATE OR ALTER PROCEDURE RD_SSPSelectionMethods @InActive BIT = NULL
AS
BEGIN
	SELECT
		*
	FROM SSPSelectionMethods
	WHERE InActive = 0
END -- Record N121
GO
CREATE OR ALTER PROCEDURE RD_SSPTraineeSupportItems @InActive BIT = NULL
AS
BEGIN
	SELECT
		*
	FROM SSPTraineeSupportItems
END -- Record N122
GO
CREATE OR ALTER PROCEDURE RD_SSPTSPMasterByUSerID @UserID INT
AS
BEGIN
	SELECT
		t.TSPName
	   ,t.Address
	   ,t.NTN
	FROM TSPMaster t
	WHERE t.UserID = @UserID
END -- Record N111
GO
CREATE OR ALTER PROCEDURE [dbo].[RD_SSPUserOrganizations] @UserID INT = 0
AS
BEGIN
	SELECT TOP 1
		M.*
	   ,(SELECT
				s.UserName
			FROM SSPUsers s
			WHERE s.UserID = @UserID)
		AS UserName
	   ,(SELECT
				o.OName
			FROM Organization o
			WHERE o.OID = 1)
		OName
	FROM UserOrganizations M
END -- Record N112
GO
CREATE OR ALTER PROCEDURE [dbo].[RD_SSPRegistrationAuthority] @InActive NVARCHAR(MAX) = '0'
AS
BEGIN
	SELECT
		ra.RegistrationAuthorityID
	   ,ra.RegistrationAuthorityName
	FROM RegistrationAuthority ra
	WHERE (
	@InActive IS NULL
	OR ra.InActive = @InActive
	)
END -- Record N113
GO
CREATE OR ALTER PROCEDURE dbo.RD_SSPApprovalStatus
AS
BEGIN
	SELECT
		*
	FROM SSPApprovalStatus ss
	WHERE InActive = 0
END -- Record N115
GO
CREATE OR ALTER PROCEDURE SSPRemove_MainCategory @CriteriaMainCategoryID INT = 0
AS
BEGIN
	DELETE FROM SSPCriteriaMainCategory
	WHERE CriteriaMainCategoryID = @CriteriaMainCategoryID
	DELETE FROM SSPCriteriaSubCategory
	WHERE CriteriaMainCategoryID = @CriteriaMainCategoryID
END -- Record N116
GO
CREATE OR ALTER PROCEDURE SSPRemove_SubCategory @CriteriaSubCategoryID INT = 0
AS
BEGIN
	DELETE FROM SSPCriteriaSubCategory
	WHERE CriteriaSubCategoryID = @CriteriaSubCategoryID
END -- Record N117
GO
CREATE OR ALTER PROCEDURE [dbo].[AU_SSPTraineeInterstInterview] @TraineeID INT,
@ApprovalStatus NVARCHAR(50),
@UserID INT = NULL
AS
BEGIN
	BEGIN TRANSACTION;
	-- Update IsSubmitted flag in TraineeIntrest table
	UPDATE SSPTraineeInterestProfile
	SET IsFinalSubmitted = 1
	   ,ApprovalStatus = @ApprovalStatus
	   ,ModifiedUserID = @UserID
	   ,ModifiedDate = GETDATE()
	WHERE TraineeRegistrationID = @TraineeID;
	-----Update Trainee Intrest Portal --------
	UPDATE SkillsScholarshipProgram.dbo.TraineeInterest
	SET TraineeStatus = @ApprovalStatus
	   ,UpdatedOn = GETDATE()
	WHERE CandidateRegistrationID = @TraineeID;
	COMMIT TRANSACTION;
-- Commit the transaction
END -- Record N119
GO
CREATE OR ALTER PROCEDURE RD_SSPCriteriaSubCatgoryForTemplate @MainCategoryID INT = 0
AS
BEGIN
	SELECT
		csc.CriteriaSubCategoryID AS CriteriaSubCategoryID
	   ,csc.CriteriaHeaderID AS CriteriaHeaderID
	   ,csc.CriteriaMainCategoryID AS CriteriaMainCategoryID
	   ,csc.SubCategoryTitle AS SubCategoryTitle
	   ,csc.SubCategoryDesc AS Description
	   ,csc.Criteria AS Criteria
	   ,csc.MarkedCriteria AS MarkedCriteria
	   ,csc.IsMandatory AS Mandatory
	   ,csc.MaxMarks AS MaxMarks
	   ,csc.Attachment AS Attachment
	   ,csc.Attachment AS CriteriaAttachment
	FROM SSPCriteriaSubCategory csc
	WHERE (
	@MainCategoryID = 0
	OR csc.CriteriaMainCategoryID = @MainCategoryID
	)
END -- Record N120
GO
CREATE OR ALTER PROCEDURE [dbo].[RD_SSPProgramTrade] --1,8,6  
@ProgramID INT = 0,
@DistrictID INT = 0,
@UserID INT = 0
AS
BEGIN
	SELECT
		sl1.TradeLotID AS TradeID
	   ,t.TradeName + ' | ' + sl.TrainingLocationName AS TradeName
	   ,sl.TrainingLocationID
	   ,s.ProgramID
	   ,d.DistrictID
	FROM SSPTSPAssignment s
	INNER JOIN SSPTSPTrainingLocation sl
		ON sl.TrainingLocationID = s.TrainingLocationID
	LEFT JOIN Province p
		ON p.ProvinceID = sl.Province
	LEFT JOIN Cluster c
		ON c.ClusterID = sl.Cluster
	LEFT JOIN District d
		ON d.DistrictID = sl.District
	INNER JOIN SSPTradeLot sl1
		ON s.TradeLotID = sl1.TradeLotID
	INNER JOIN Trade t
		ON sl1.TradeID = t.TradeID

	WHERE s.ProgramID = @ProgramID
	AND s.TSPID = @UserID
	AND d.DistrictID = @DistrictID;

END
GO
CREATE OR ALTER PROCEDURE RD_SSPProgramStatus
AS
	SELECT
		sd.ProgramName
	   ,ssd.ProcessStartDate
	   ,ssd.ProcessEndDate
	   ,ssd.InActive
	   ,CASE
			WHEN CAST(ssd.ProcessEndDate AS DATE) >= CAST(GETDATE() AS DATE) THEN 'Open'
			ELSE 'Closed'
		END IsLocked
	FROM SSPProcessScheduleMaster ssm
	INNER JOIN SSPProcessScheduleDetail ssd
		ON ssd.ProcessScheduleMasterID = ssm.ProcessScheduleMasterID
	INNER JOIN SSPProgramDesign sd
		ON sd.ProgramID = ssm.ProgramID
	ORDER BY sd.ProgramID DESC
GO
CREATE OR ALTER PROCEDURE RD_BusinessRuleType
AS
	SELECT
		*
	FROM BusinessRules br
GO
CREATE
OR ALTER PROC AU_SetDefault @BankDetailID INT = 0,
@CurrentUserID INT = 0
AS
	UPDATE SSPTSPBankDetail
	SET InActive = 1
	WHERE CreatedUserID = @CurrentUserID
	UPDATE SSPTSPBankDetail
	SET InActive = 0
	WHERE BankDetailID = @BankDetailID
	EXEC RD_SSPBankDetail @UserID = @CurrentUserID
GO
CREATE
OR ALTER PROCEDURE [dbo].[GetUsersRightsByUser] @UserID INT = 0
AS
BEGIN
	DECLARE @RoleID INT
	SELECT
		@RoleID = r.RoleID
	FROM Users u
	INNER JOIN Roles r
		ON u.RoleID = r.RoleID
	WHERE u.UserID = @UserID --IF role title IS TSP THEN EXEC
	IF @RoleID = 12
	BEGIN
		SELECT
			Users.FullName AS Username
		   ,Mo.ModuleTitle
		   ,Mo.modpath
		   ,Mo.SortOrder AS ModSortOrder
		   ,A.*
		   ,@UserID AS UserID
		   ,ISNULL(M.UserRightID, 0) AS UserRightID
		   ,ISNULL(M.CanAdd, 0) AS CanAdd
		   ,ISNULL(M.CanEdit, 0) AS CanEdit
		   ,ISNULL(M.CanDelete, 0) AS CanDelete
		   ,ISNULL(M.CanView, 0) AS CanView
		   ,ISNULL(M.InActive, 0) AS InActive
		FROM AppForms A
		LEFT OUTER JOIN UsersRights M
			ON A.FormID = M.FormID
				AND M.UserID = @UserID
		LEFT JOIN Users
			ON Users.UserID = M.UserID
		LEFT JOIN Modules Mo
			ON Mo.ModuleID = A.ModuleID
		WHERE A.InActive != 1
		UNION ALL
		SELECT
			(SELECT
					tu.Username
				FROM SSPUsers tu
				WHERE tu.UserID = @UserID
				AND tu.InActive = 0)
			AS Username
		   ,Mo.ModuleTitle
		   ,Mo.modpath
		   ,Mo.SortOrder AS ModSortOrder
		   ,A.*
		   ,@UserID AS UserID
		   ,0 AS UserRightID
		   ,1 AS CanAdd
		   ,1 AS CanEdit
		   ,1 AS CanDelete
		   ,1 AS CanView
		   ,1 AS InActive
		FROM AppForms A
		LEFT JOIN Modules Mo
			ON Mo.ModuleID = A.ModuleID --INNER JOIN Users u ON u.UserID=@UserID AND u.RoleID=4    
		WHERE A.InActive = 0
		AND Mo.ModuleTitle IN (
		'Profile Management',
		'Association Management',
		'TSP Trainee Portal'
		)
		AND A.FormName IN (
		'Profile',
		'Base Data',
		'Association Submission',
		'TSP Trainee Portal'
		)
	END
	ELSE
	BEGIN
		SELECT
			Users.FullName AS Username
		   ,Mo.ModuleTitle
		   ,Mo.modpath
		   ,Mo.SortOrder AS ModSortOrder
		   ,A.*
		   ,@UserID AS UserID
		   ,ISNULL(M.UserRightID, 0) AS UserRightID
		   ,ISNULL(M.CanAdd, 0) AS CanAdd
		   ,ISNULL(M.CanEdit, 0) AS CanEdit
		   ,ISNULL(M.CanDelete, 0) AS CanDelete
		   ,ISNULL(M.CanView, 0) AS CanView
		   ,ISNULL(M.InActive, 0) AS InActive
		FROM AppForms A
		LEFT OUTER JOIN UsersRights M
			ON A.FormID = M.FormID
				AND M.UserID = @UserID
		LEFT JOIN Users
			ON Users.UserID = M.UserID
		LEFT JOIN Modules Mo
			ON Mo.ModuleID = A.ModuleID
		WHERE A.InActive != 1
	END
END
GO
CREATE OR ALTER PROCEDURE AU_SSPMergeUserAccounts @SchemeID INT = 0
AS
BEGIN
	SET NOCOUNT ON;
	-- Temp table to store necessary data  
	CREATE TABLE #UserMappings (
		BSSUserID INT
	   ,SSPUserID INT
	   ,SSPUserName NVARCHAR(500)
	   ,SSPUserPassword NVARCHAR(MAX)
	);
	-- Insert relevant data into the temp table  
	INSERT INTO #UserMappings (BSSUserID, SSPUserID, SSPUserName, SSPUserPassword)
		SELECT
			t.UserID AS BSSUserID
		   ,s.UserID AS SSPUserID
		   ,su.UserName AS SSPUserName
		   ,up.UserPassword AS SSPUserPassword
		FROM TSPMaster t
		INNER JOIN SSPTSPProfile s
			ON s.BusinessName = t.TSPName
				AND t.NTN = s.NTN
				AND t.UserID <> s.UserID
		--INNER JOIN TSPDetail t1  
		--  ON s.TspID = t1.TspID  
		--    AND t1.SchemeID = @SchemeID  
		INNER JOIN TSPDetail t1
			ON t.TSPMasterID = t1.TSPMasterID
				AND t1.SchemeID = @SchemeID
		INNER JOIN SSPUsers su
			ON su.UserID = s.UserID
		INNER JOIN SSPUsersPwd up
			ON su.UserID = up.UserID
		WHERE up.InActive = 0 --AND su.UserName='Apple-0005'  
	-- Update UserName of BSS newly created Users  in Users table  
	UPDATE u
	SET u.UserName = um.SSPUserName
	FROM Users u
	INNER JOIN #UserMappings um
		ON u.UserID = um.BSSUserID;
	-- Update UserPassword of BSS newly created Users in  User_pwd table  
	UPDATE up
	SET up.UserPassword = um.SSPUserPassword
	FROM User_pwd up
	INNER JOIN #UserMappings um
		ON up.UserID = um.BSSUserID
	WHERE up.InActive = 0;
	-- Update SSPTSPProfile table  
	UPDATE ssp
	SET ssp.UserID = um.BSSUserID
	FROM SSPTSPProfile ssp
	INNER JOIN #UserMappings um
		ON ssp.UserID = um.SSPUserID;
	-- Update CreatedUserID in various tables  
	UPDATE sst
	SET sst.CreatedUserID = um.BSSUserID
	FROM SSPTSPTradeManage sst
	INNER JOIN #UserMappings um
		ON sst.CreatedUserID = um.SSPUserID;
	UPDATE sstl
	SET sstl.CreatedUserID = um.BSSUserID
	FROM SSPTSPTrainingLocation sstl
	INNER JOIN #UserMappings um
		ON sstl.CreatedUserID = um.SSPUserID;
	UPDATE ssbd
	SET ssbd.CreatedUserID = um.BSSUserID
	FROM SSPTSPBankDetail ssbd
	INNER JOIN #UserMappings um
		ON ssbd.CreatedUserID = um.SSPUserID;
	UPDATE stp
	SET stp.CreatedUserID = um.BSSUserID
	FROM SSPTrainerProfile stp
	INNER JOIN #UserMappings um
		ON stp.CreatedUserID = um.SSPUserID;
	UPDATE stpd
	SET stpd.CreatedUserID = um.BSSUserID
	FROM SSPTrainerProfileDetail stpd
	INNER JOIN #UserMappings um
		ON stpd.CreatedUserID = um.SSPUserID;
	UPDATE stc
	SET stc.CreatedUserID = um.BSSUserID
	FROM SSPTrainingCertification stc
	INNER JOIN #UserMappings um
		ON stc.CreatedUserID = um.SSPUserID;
	UPDATE strpd
	SET strpd.CreatedUserID = um.BSSUserID
	FROM SSPTSPRegistrationPaymentDetail strpd
	INNER JOIN #UserMappings um
		ON strpd.CreatedUserID = um.SSPUserID;
	UPDATE stsam
	SET stsam.CreatedUserID = um.BSSUserID
	FROM SSPTspAssociationMaster stsam
	INNER JOIN #UserMappings um
		ON stsam.CreatedUserID = um.SSPUserID;
	UPDATE stsad
	SET stsad.CreatedUserID = um.BSSUserID
	FROM SSPTspAssociationDetail stsad
	INNER JOIN #UserMappings um
		ON stsad.CreatedUserID = um.SSPUserID;
	UPDATE ssapd
	SET ssapd.CreatedUserID = um.BSSUserID
	   ,ssapd.TspID = um.BSSUserID
	FROM SSPTSPAssociationPaymentDetail ssapd
	INNER JOIN #UserMappings um
		ON ssapd.CreatedUserID = um.SSPUserID;
	UPDATE ssta
	SET ssta.TspID = um.BSSUserID
	FROM SSPTSPAssignment ssta
	INNER JOIN #UserMappings um
		ON ssta.TspID = um.SSPUserID;
	UPDATE sip
	SET sip.TspID = um.BSSUserID
	FROM SSPTraineeInterestProfile sip
	INNER JOIN #UserMappings um
		ON sip.TspID = um.SSPUserID;
	UPDATE ti
	SET ti.TspID = um.BSSUserID
	FROM SkillsScholarshipProgram.dbo.TraineeInterest ti
	INNER JOIN #UserMappings um
		ON ti.TspID = um.SSPUserID;
	-- Drop the temp table  
	DROP TABLE #UserMappings;
END
GO

CREATE OR ALTER PROC RD_SSPProfileScore @UserID INT = 0
AS
	SELECT
		'BusinessProfile' AS FormName
	   ,CASE
			WHEN EXISTS (SELECT
						1
					FROM SSPTSPProfile
					WHERE UserID = @UserID
					AND LegalStatusID > 0) THEN 10
			ELSE 0
		END AS Score
	   ,CASE
			WHEN EXISTS (SELECT
						1
					FROM SSPTSPProfile
					WHERE UserID = @UserID
					AND LegalStatusID > 0) THEN 'Completed'
			ELSE 'Incomplete'
		END AS Status

	UNION ALL

	SELECT
		'ContactPerson' AS FormName
	   ,CASE
			WHEN EXISTS (SELECT
						1
					FROM SSPTSPProfile
					WHERE UserID = @UserID
					AND POCName IS NOT NULL
					AND POCName <> '') THEN 10
			ELSE 0
		END AS Score
	   ,CASE
			WHEN EXISTS (SELECT
						1
					FROM SSPTSPProfile
					WHERE UserID = @UserID
					AND POCName IS NOT NULL
					AND POCName <> '') THEN 'Completed'
			ELSE 'Incomplete'
		END AS Status

	UNION ALL

	SELECT
		'TrainingLocation' AS FormName
	   ,CASE
			WHEN EXISTS (SELECT
						1
					FROM SSPTSPTrainingLocation
					WHERE CreatedUserID = @UserID
					AND TrainingLocationName IS NOT NULL
					AND TrainingLocationName <> '') THEN 10
			ELSE 0
		END AS Score
	   ,CASE
			WHEN EXISTS (SELECT
						1
					FROM SSPTSPTrainingLocation
					WHERE CreatedUserID = @UserID
					AND TrainingLocationName IS NOT NULL
					AND TrainingLocationName <> '') THEN 'Completed'
			ELSE 'Incomplete'
		END AS Status

	UNION ALL

	SELECT
		'TrainingCertification' AS FormName
	   ,CASE
			WHEN EXISTS (SELECT
						1
					FROM SSPTrainingCertification
					WHERE CreatedUserID = @UserID
					AND TrainingCertificationID IS NOT NULL
					AND TrainingCertificationID <> '') THEN 10
			ELSE 0
		END AS Score
	   ,CASE
			WHEN EXISTS (SELECT
						1
					FROM SSPTrainingCertification
					WHERE CreatedUserID = @UserID
					AND TrainingCertificationID IS NOT NULL
					AND TrainingCertificationID <> '') THEN 'Completed'
			ELSE 'Incomplete'
		END AS Status

	UNION ALL

	SELECT
		'TrainingCertificateMapping' AS FormName
	   ,CASE
			WHEN EXISTS (SELECT
						1
					FROM SSPTSPTradeManage
					WHERE CreatedUserID = @UserID
					AND TradeManageID IS NOT NULL
					AND TradeManageID <> '') THEN 20
			ELSE 0
		END AS Score
	   ,CASE
			WHEN EXISTS (SELECT
						1
					FROM SSPTSPTradeManage
					WHERE CreatedUserID = @UserID
					AND TradeManageID IS NOT NULL
					AND TradeManageID <> '') THEN 'Completed'
			ELSE 'Incomplete'
		END AS Status

	UNION ALL

	SELECT
		'TrainerProfile' AS FormName
	   ,CASE
			WHEN EXISTS (SELECT
						1
					FROM SSPTrainerProfile
					WHERE CreatedUserID = @UserID
					AND TrainerID IS NOT NULL
					AND TrainerID <> '') THEN 20
			ELSE 0
		END AS Score
	   ,CASE
			WHEN EXISTS (SELECT
						1
					FROM SSPTrainerProfile
					WHERE CreatedUserID = @UserID
					AND TrainerID IS NOT NULL
					AND TrainerID <> '') THEN 'Completed'
			ELSE 'Incomplete'
		END AS Status

	UNION ALL
	SELECT
		'BankDetail' AS FormName
	   ,CASE
			WHEN EXISTS (SELECT
						1
					FROM SSPTSPBankDetail
					WHERE CreatedUserID = @UserID
					AND BankDetailID IS NOT NULL
					AND BankDetailID <> '') THEN 10
			ELSE 0
		END AS Score
	   ,CASE
			WHEN EXISTS (SELECT
						1
					FROM SSPTSPBankDetail
					WHERE CreatedUserID = @UserID
					AND BankDetailID IS NOT NULL
					AND BankDetailID <> '') THEN 'Completed'
			ELSE 'Incomplete'
		END AS Status

	UNION ALL

	SELECT
		'RegistrationPayment' AS FormName
	   ,CASE
			WHEN EXISTS (SELECT
						1
					FROM SSPTSPRegistrationPaymentDetail
					WHERE CreatedUserID = @UserID
					AND TSPRegistrationPaymentID IS NOT NULL
					AND TSPRegistrationPaymentID <> '') THEN 10
			ELSE 0
		END AS Score
	   ,CASE
			WHEN EXISTS (SELECT
						1
					FROM SSPTSPRegistrationPaymentDetail
					WHERE CreatedUserID = @UserID
					AND TSPRegistrationPaymentID IS NOT NULL
					AND TSPRegistrationPaymentID <> '') THEN 'Completed'
			ELSE 'Incomplete'
		END AS Status;


GO
CREATE OR ALTER PROCEDURE dbo.RD_SSPTSPTrainingLocationWithTradeCount @UserID INT = 0
AS
BEGIN
	SELECT
		ttm.TrainingLocationID
	   ,ttl.TrainingLocationName AS TrainingLocation
	   ,tpd.TrainingLocationFee
	   ,CASE
			WHEN pppd.OrderStatus = '' OR
				pppd.OrderStatus IS NULL THEN 'Pending'
			ELSE pppd.OrderStatus
		END AS PaymentStatus
	   ,pppd.OrderNumber AS InvoiceNo
	   ,pppd.ConnectPayId AS [PayPro ID]
	   ,ttl.TrainingLocationAddress
	   ,COUNT(ttm.TradeManageID) TotalTrade
	   ,ttm.CreatedUserID AS UserID
	   ,ttp.BusinessName AS [TSP Name]
	   ,ttp.TspEmail AS [TSP Email]
	   ,ttp.TspContact AS [TSP Contact]
	FROM SSPTSPTradeManage ttm
	INNER JOIN SSPTSPTrainingLocation ttl
		ON ttm.TrainingLocationID = ttl.TrainingLocationID
	LEFT JOIN SSPTSPTradeStatusHistory ttsh
		ON ttm.TradeManageID = ttsh.TradeManageID
			AND ttsh.InActive = 0
	LEFT JOIN SSPApprovalStatus tts
		ON tts.TspTradeStatusID = ttsh.StatusID
	LEFT JOIN SSPTSPProfile ttp
		ON ttp.UserID = ttm.CreatedUserID
	LEFT JOIN SSPTSPRegistrationPaymentDetail tpd
		ON tpd.TrainingLocationID = ttm.TrainingLocationID
	LEFT JOIN PayPro_PaymentDetail pppd
		ON tpd.PayProPaymentTableID = pppd.ID
	WHERE ttm.CreatedUserID = @UserID --AND tts.Status = 'Accepted'    
	GROUP BY ttm.TrainingLocationID
			,ttl.TrainingLocationName
			,ttl.TrainingLocationAddress
			,ttm.CreatedUserID
			,ttp.BusinessName
			,ttp.TspEmail
			,ttp.TspContact
			,tpd.TrainingLocationFee
			,pppd.OrderStatus
			,pppd.OrderNumber
			,pppd.ConnectPayId
END
GO

CREATE OR ALTER PROCEDURE [dbo].[AU_SSPTraineeInterestInterview] @TraineeID INT,
@ApprovalStatus NVARCHAR(50),
@UserID INT = NULL,
@StatusReason NVARCHAR(250) = NULL
AS
BEGIN
	BEGIN TRANSACTION;
	--Update IsSubmitted flag in TraineeInterest table
	UPDATE SSPTraineeInterestProfile
	SET IsFinalSubmitted = 1
	   ,ApprovalStatus = @ApprovalStatus
	   ,StatusReason = @StatusReason
	   ,ModifiedUserID = @UserID
	   ,ModifiedDate = GETDATE()
	WHERE TraineeRegistrationID = @TraineeID;
	-----Update Trainee Interest Portal --------
	UPDATE SkillsScholarshipProgram.dbo.TraineeInterest
	SET TraineeStatus = @ApprovalStatus
	   ,TraineeStatusReason = @StatusReason
	   ,UpdatedBy = @UserID
	   ,UpdatedOn = GETDATE()
	WHERE CandidateRegistrationID = @TraineeID;
	COMMIT TRANSACTION;
--Commit the transaction
END
------------------------
GO
CREATE OR ALTER PROCEDURE [dbo].[AU_SSPTraineeInterstProfile] @TraineeID INT,
@UserID INT = NULL
AS
BEGIN
	BEGIN TRANSACTION;
	INSERT INTO SSPTraineeInterestProfile (TraineeRegistrationID,
	TraineeName,
	TraineeCNIC,
	CNICIssueDate,
	HouseHoldIncomeID,
	FatherName,
	GenderID,
	TradeID,
	TradeLotID,
	DistrictID,
	SchemeID,
	TrainingLocationID,
	DateOfBirth,
	TspID,
	EducationID,
	ContactNumber1,
	TraineeAge,
	ReligionID,
	TraineeDistrictID,
	TraineeDisabilityID,
	TraineeEmail,
	Shift,
	CreatedDate,
	CreatedUserID,
	InActive,
	IsSubmitted,
	IsFinalSubmitted)
		SELECT
			t.CandidateRegistrationID
		   ,c.FullName
		   ,c.CNIC
		   ,c.CNICIssuanceDate
		   ,c.HouseHoldIncome
		   ,c.FatherName
		   ,c.GenderID
		   ,t.TradeID
		   ,t.TradeLotID
		   ,t.DistrictID
		   ,t.ProgramID
		   ,t.TrainingLocationID
		   ,c.DateOfBirth
		   ,t.TspID
		   ,c.EducationTypeID
		   ,c.MobileNumber
		   ,DATEDIFF(YEAR, c.DateOfBirth, GETDATE()) AS TraineeAge
		   , -- Calculate age
			c.ReligionID
		   ,c.DistrictOfResidenceID
		   ,c.DisabilityID
		   ,c.Email
		 ,t.Shift
		   ,GETDATE() AS CreatedDate
		   ,@UserID
		   , -- Specify the user ID who created the 
			0 AS InActive
		   , -- Assuming default value for InActive column
			1 AS IsSubmitted
		   , -- Assuming default value for IsSubmitted column
			0 ---IsFinalSubmitted
		FROM SkillsScholarshipProgram.dbo.TraineeInterest t
		INNER JOIN SkillsScholarshipProgram.dbo.CandidateDetails c
			ON c.CandidateRegistrationID = t.CandidateRegistrationID
		WHERE c.CandidateRegistrationID = @TraineeID
	-- Update IsSubmitted flag in TraineeIntrest table
	UPDATE SkillsScholarshipProgram.dbo.TraineeInterest
	SET IsSubmitted = 1
	   ,UpdatedOn = GETDATE()
	WHERE CandidateRegistrationID = @TraineeID;
	COMMIT TRANSACTION; -- Commit the transaction
END
------------------------------------------
GO
CREATE OR ALTER PROCEDURE [dbo].[RD_SSPSubmittedTrainee] --exec [RD_SSPSubmittedTrainee] @TraineeFilter = 2,@ProgramID = 1,@Districtid = 19,@TradeID = 2,@TSPID =4
@TspId INT = NULL,
@DistrictID INT = NULL,
@TradeID INT = NULL,
@GenderID INT = NULL,
@TraineeDIs INT = NULL,
@TraineeFilter INT = NULL,
@ProgramID INT = NULL,
@TrainingLocationID INT = NULL
AS
BEGIN
	IF @TraineeFilter = 1
	BEGIN
		SELECT
			t.CandidateRegistrationID AS 'TraineeID'
		   ,c.FullName
		   ,c.FatherName
		   ,g.GenderName
		   ,r.ReligionName
		   ,c.DateOfBirth
		   ,c.MobileNumber
		   ,c.Email
		   ,c.CNIC
		   ,d.DistrictName AS 'TraineeDistrict'
		   ,c.CurrentAddress
		   ,c.EducationTypeID AS 'LastDegree'
		   ,td.Disability
		   ,t.TradeLotID AS TradeID
		   ,t.DistrictID
		   ,t.TspID
		   ,t.TrainingLocationID
	           ,t.Shift
		FROM SkillsScholarshipProgram.dbo.TraineeInterest t
		INNER JOIN SkillsScholarshipProgram.dbo.CandidateDetails c
			ON c.CandidateRegistrationID = t.CandidateRegistrationID
		INNER JOIN Gender g
			ON g.GenderID = c.GenderID
		INNER JOIN Religion r
			ON r.ReligionID = c.ReligionID
		INNER JOIN TraineeDisability td
			ON td.Id = c.DisabilityID
		INNER JOIN District d
			ON d.DistrictID = c.DistrictOfResidenceID
		WHERE t.IsSubmitted = 0
		AND t.TspID = @TspId
		AND t.ProgramID = @ProgramID
		AND t.DistrictID = @DistrictID
		AND t.TradeLotID = @TradeID
		AND t.TrainingLocationID = @TrainingLocationID
	END
	ELSE
	IF @TraineeFilter = 2
	BEGIN
		SELECT
			t.TraineeRegistrationID AS 'TraineeID'
		   ,t.TraineeName AS FullName
		   ,t.FatherName
		   ,g.GenderName
		   ,r.ReligionName
		   ,t.DateOfBirth
		   ,t.ContactNumber1 AS MobileNumber
		   ,t.TraineeEmail AS Email
		   ,t.TraineeCNIC AS CNIC
		   ,d.DistrictName AS 'TraineeDistrict'
		   ,'' AS CurrentAddress
		   ,td.Disability
		   ,t.TradeLotID AS TradeID
		   ,t.TraineeDistrictID AS DistrictID
		   ,t.TspID
		   ,t.DistrictID
		   ,t.SchemeID
		   ,t.TrainingLocationID
		  ,t.Shift
		FROM SSPTraineeInterestProfile t
		INNER JOIN Gender g
			ON g.GenderID = t.GenderID
		INNER JOIN Religion r
			ON r.ReligionID = t.ReligionID
		INNER JOIN TraineeDisability td
			ON td.Id = t.TraineeDisabilityID
		INNER JOIN District d
			ON d.DistrictID = t.TraineeDistrictID
		WHERE t.IsSubmitted = 1
		AND (t.IsFinalSubmitted IS NULL
		OR t.IsFinalSubmitted = 0)
		AND t.TspID = @TspId
		AND t.SchemeID = @ProgramID
		AND t.DistrictID = @DistrictID
		AND t.TradeLotID = @TradeID
		AND t.TrainingLocationID = @TrainingLocationID
	END
	ELSE
	IF @TraineeFilter = 3
	BEGIN
		SELECT
			t.TraineeRegistrationID AS 'TraineeID'
		   ,t.TraineeName AS FullName
		   ,t.FatherName
		   ,g.GenderName
		   ,r.ReligionName
		   ,t.DateOfBirth
		   ,t.ContactNumber1 AS MobileNumber
		   ,t.TraineeEmail AS Email
		   ,t.TraineeCNIC AS CNIC
		   ,d.DistrictName AS 'TraineeDistrict'
		   ,'' AS CurrentAddress
		   ,td.Disability
		   ,t.TradeID
		   ,t.TraineeDistrictID AS DistrictID
		   ,t.TspID AS TSPMasterID
		   ,t.TrainingLocationID
			 ,t.Shift
		FROM SSPTraineeInterestProfile t
		INNER JOIN Gender g
			ON g.GenderID = t.GenderID
		INNER JOIN Religion r
			ON r.ReligionID = t.ReligionID
		INNER JOIN TraineeDisability td
			ON td.Id = t.TraineeDisabilityID
		INNER JOIN District d
			ON d.DistrictID = t.TraineeDistrictID
		WHERE t.IsFinalSubmitted = 1
		AND t.TspID = @TspId
		AND t.SchemeID = @ProgramID
		AND t.DistrictID = @DistrictID
		AND t.TradeLotID = @TradeID
		AND t.TrainingLocationID = @TrainingLocationID
		AND t.ApprovalStatus = 'Accept'
	END
END
-------------------
GO
CREATE OR ALTER PROCEDURE [dbo].[RD_SSPProgramByUser]  --1586
@UserID INT = 0
AS
BEGIN
	SELECT DISTINCT
		pd.ProgramID
	   ,pd.ProgramName
	   ,ssd.ProcessStartDate
	   ,ssd.ProcessEndDate
	   ,pd.ProgramCode
	   ,pd.ProgramDescription
	   ,sm.PaymentStructure
	   ,CASE
			WHEN CAST(ssd.ProcessEndDate AS DATE) >= CAST(GETDATE() AS DATE) THEN 'Open'
			ELSE 'Closed'
		END IsLocked
	FROM SSPTSPAssignment ts
	INNER JOIN SSPProgramDesign pd
		ON pd.ProgramID = ts.ProgramID
	INNER JOIN SSPProcessScheduleMaster ssm
		ON ssm.ProgramID = pd.ProgramID
	INNER JOIN SSP_Testing.dbo.SSPProcessScheduleDetail ssd
		ON ssd.ProcessScheduleMasterID = ssm.ProcessScheduleMasterID
			AND ssd.ProcessID = 8 --TSP Trainee Evaluation
	INNER JOIN SAP_Scheme sm
		ON sm.SAP_SchemeID = pd.PaymentSchedule
	WHERE pd.IsApproved = 1
	AND pd.IsFinalApproved = 1
	AND pd.InActive = 0
	AND (TspID = @UserID
	OR @UserID = 0)
	AND ProgramName NOT IN (SELECT
			SchemeName
		FROM Scheme
		WHERE SchemeName = pd.ProgramName)
	ORDER BY pd.ProgramID

END;
--------------------------------
GO

ALTER PROCEDURE [dbo].[POLinesAndHeader] @SchemeID INT
, @CurUserID INT
, @ProcessKey NVARCHAR(50)
AS
BEGIN

	DECLARE @TSPMasterID INT
			-- variables for POHeader  
		   ,@docEntry INT
		   ,@DocTotal FLOAT = 0
		   ,@Comments NVARCHAR(MAX)
		   ,@JournalMemo NVARCHAR(MAX)
		   ,@CtlAccount NVARCHAR(MAX)
		   ,@U_SCHEME NVARCHAR(MAX)
		   ,@U_SCH_Code NVARCHAR(MAX)
		   ,@tspName NVARCHAR(MAX)
		   ,@schemeName NVARCHAR(MAX)
		   ,@paymentStructure NVARCHAR(MAX)
			-- POLines  
		   ,@schemeCode NVARCHAR(MAX)
		   ,@cardCode NVARCHAR(MAX)
		   ,@tspid INT
		   ,@classCode NVARCHAR(MAX)
		   ,@classID NVARCHAR(MAX)
		   ,@batch NVARCHAR(MAX)
		   ,@tradeName NVARCHAR(MAX)
		   ,@pCatID NVARCHAR(MAX)
		   ,@perTraineeCostOfTheMonth FLOAT
		   ,@batchDuration NVARCHAR(MAX)
		   ,@traineesPerClass INT
		   ,@perTraineeCostLastMonth FLOAT
		   ,@UniformAndBagCost FLOAT
		   ,@perTraineeCost2ndLast FLOAT
		   ,@perTraineeStipend FLOAT
		   ,@totalCostPerClass FLOAT
		   ,@U_Testing_Fee FLOAT
		   ,@U_Class_Start_Date DATE
		   ,@U_Class_End_Date DATE
		   ,@lineNum INT
		   ,@deptCode NVARCHAR(MAX)
			-- variables for calculations  
		   ,@description NVARCHAR(MAX)
		   ,@ocrCode2 NVARCHAR(MAX)
		   ,@lineTotal FLOAT
		   ,@perTraineeCost FLOAT
		   ,@U_Cost_Trainee_Month FLOAT = 0
		   ,@U_Cost_Trainee_LMonth FLOAT = 0
		   ,@U_Cost_Trainee_2Month FLOAT = 0
		   ,@U_Cost_Trainee_2nd_Last FLOAT = 0
		   ,@U_Cost_Trainee_FMonth FLOAT = 0
			-- static fields for PO  
		   ,@acctCode NVARCHAR(MAX)
		   ,@taxCode NVARCHAR(MAX)
		   ,@wTLiable NVARCHAR(10)
		   ,@lineStatus NVARCHAR(10)
		   ,@ocrCode NVARCHAR(MAX)
		   ,@ocrCode3 NVARCHAR(MAX)
		   ,@SchemeNameBSS NVARCHAR(MAX)
		   ,@SchemeCodeBSS NVARCHAR(MAX)
		   ,@sapSchemeCode NVARCHAR(MAX)
		   ,@sapBPLId NVARCHAR(MAX)
		   ,@ClassDuration DECIMAL(18, 2)
		   ,@PerMonthPerTraineeCostExTax DECIMAL(18, 2)
		   ,@ProgramType INT;




	SELECT
		@SchemeNameBSS = s.SchemeName
	   ,@SchemeCodeBSS = s.SchemeCode
	   ,@schemeCode = s.SAPID
	   ,@paymentStructure = s.PaymentSchedule
	   ,@sapSchemeCode = sap.SchemeCode
	   ,@sapBPLId = sb.BranchID
	   ,@U_SCHEME = sap.SchemeCode
	   ,@ProgramType = s.ProgramTypeID
	FROM Scheme s
	INNER JOIN dbo.SAP_BPL AS sb
		ON sb.FundingCategoryID = s.FundingCategoryID
	INNER JOIN SAP_Scheme sap
		ON sap.PaymentStructure = s.PaymentSchedule
	WHERE s.SchemeID = @SchemeID;


	--Updated by Ali Haider on 22-Apl-2024  
	-----Get @GLAccountCode according to program type if SSP then update Scholarship Cost upon Training Cost  
	IF (@ProgramType = 10)
	BEGIN
		SELECT
			@acctCode = AcctCode
		   ,@taxCode = TaxCode
		   ,@wTLiable = WTLiable
		   ,@lineStatus = LineStatus
			--, @ocrCode3 = OcrCode3  
		   ,@ocrCode = OcrCode
		   ,@CtlAccount = CtlAccount
		   ,@U_SCH_Code = U_SCH_Code
		-- , @U_SCHEME = N'C1120'  
		FROM dbo.SAP_StaticField AS ssf
		WHERE ssf.SAP_StaticFileldID = 2;
	END
	ELSE
	BEGIN
		SELECT
			@acctCode = AcctCode
		   ,@taxCode = TaxCode
		   ,@wTLiable = WTLiable
		   ,@lineStatus = LineStatus
			--, @ocrCode3 = OcrCode3  
		   ,@ocrCode = OcrCode
		   ,@CtlAccount = CtlAccount
		   ,@U_SCH_Code = U_SCH_Code
		-- , @U_SCHEME = N'C1120'  
		FROM dbo.SAP_StaticField AS ssf
		WHERE ssf.SAP_StaticFileldID = 1;
	END

	-------------End updation------------  

	SELECT
		@docEntry = NEXT VALUE FOR DocEntrySequence;

	DECLARE tspMasterCursor CURSOR FOR SELECT DISTINCT
		(tm.TSPMasterID)
	FROM Class c
	INNER JOIN TSPDetail td
		ON c.TspID = td.TspID
	INNER JOIN TSPMaster tm
		ON td.TSPMasterID = tm.TSPMasterID
	WHERE c.SchemeID = @SchemeID;

	OPEN tspMasterCursor;
	FETCH NEXT FROM tspMasterCursor
	INTO @TSPMasterID;

	WHILE @@fetch_status = 0 -- TSPs FETCH  
	BEGIN

	-- POHeader Starting  
	DECLARE @ident INT;
	INSERT INTO POHeader (ProcessKey
	, DocEntry
	, DocNum
	, DocType
	, Printed
	, DocDate
	, DocDueDate
	, CardCode
	, CardName
	, DocTotal
	, Comments
	, JournalMemo
	, BPLId
	, CtlAccount
	, U_SCHEME
	, U_Sch_Code
	, month
	--, SAPID  
	, CreatedUserID
	, IsMigrated)
		VALUES (@ProcessKey, @docEntry, '0', 'S', 'N', GETDATE(), GETDATE(), (SELECT SAPID FROM TSPMaster WHERE TSPMasterID = @TSPMasterID), (SELECT TSPName FROM TSPMaster WHERE TSPMasterID = @TSPMasterID), @DocTotal, 'PO for ' + @SchemeNameBSS + '-' + @SchemeCodeBSS, 'PO for ' + @SchemeNameBSS + '-' + @SchemeCodeBSS, @sapBPLId, @CtlAccount, @U_SCHEME, @schemeCode, GETDATE(), @CurUserID, 0);

	SET @ident = SCOPE_IDENTITY();
	-- POHeader Ending  

	-- POLines Starting  
	DECLARE poLineCursor CURSOR FOR SELECT
		s.SAPID
	   ,s.SchemeName
	   ,s.PaymentSchedule
	   ,c.ClassCode
	   ,c.ClassID
	   ,td.SAPID
	   ,td.TspID
	   ,td.TSPName
	   ,c.Batch
	   ,trd.TradeName
	   ,s.PCategoryID
	   ,c.TrainingCostPerTraineePerMonthExTax
	   ,c.Duration
	   ,c.TraineesPerClass
	   ,c.UniformBagCost
	   ,c.Stipend
	   ,c.TotalCostPerClass
	   ,c.PerTraineeTestCertCost
	   ,c.StartDate
	   ,c.EndDate
	   ,c.TotalPerTraineeCostInTax
	FROM Class c
	INNER JOIN Scheme s
		ON s.SchemeID = c.SchemeID
	INNER JOIN TSPDetail td
		ON c.TspID = td.TspID
	INNER JOIN TSPMaster tm
		ON td.TSPMasterID = tm.TSPMasterID
	INNER JOIN Trade trd
		ON c.TradeID = trd.TradeID
	WHERE tm.TSPMasterID = @TSPMasterID
	AND c.SchemeID = @SchemeID;

	SET @lineNum = 0;

	OPEN poLineCursor;
	FETCH NEXT FROM poLineCursor
	INTO @schemeCode
	, @schemeName
	, @paymentStructure
	, @classCode
	, @classID
	, @cardCode
	, @tspid
	, @tspName
	, @batch
	, @tradeName
	, @pCatID
	, @perTraineeCostOfTheMonth
	, @batchDuration
	, @traineesPerClass
	, @UniformAndBagCost
	, @perTraineeStipend
	, @totalCostPerClass
	, @U_Testing_Fee
	, @U_Class_Start_Date
	, @U_Class_End_Date
	, @perTraineeCost;

	WHILE @@fetch_status = 0 -- Classes FETCH  
	BEGIN
	SET @description = CONCAT(@classCode, '  Batch ', @batch, '  ', @tradeName);
	SET @deptCode = (SELECT
			s.PCategoryID
		FROM Class
		INNER JOIN Scheme s
			ON s.SchemeID = Class.SchemeID
		WHERE Class.ClassID = @classID);

	DECLARE @StipendMethod NVARCHAR(100) = N'';
	SELECT
		@StipendMethod = StipendPayMethod
	FROM OrgConfig
	WHERE ClassID = @classID;
	IF (@StipendMethod = 'digital')
	BEGIN
		SET @perTraineeStipend = 0;
	END;

	DECLARE @tradeidsap NVARCHAR(100) = N'';
	SET @tradeidsap = (SELECT
			t.SAPID
		FROM Trade t
		INNER JOIN Class
			ON Class.TradeID = t.TradeID
		WHERE Class.ClassID = @classID);
	SET @ocrCode2 = @tradeidsap;
	IF (@deptCode = 1) --6 dev deb, Business Development  
	BEGIN
		SET @deptCode = N'01';
		SET @ocrCode3 = N'BD011';
	--SET @ocrCode2 = @tradeidsap+'-01';  
	END;
	ELSE
	IF (@deptCode = 2) --7 dev db, Program Development  
	BEGIN
		SET @deptCode = N'00';
		SET @ocrCode3 = N'PD002';
	--SET @ocrCode2 = @tradeidsap+'-00';  
	END;
	--      ELSE  
	--         SET @deptCode = N'';  
	--set @ocrCode3 = N'PD002';  
	-- select @perTraineeCostOfTheMonth=SUM(Amount) from classinvoicemap where InvoiceNo=1 and InvoiceType='Regular' and ClassID in (select classid from class where schemeid=@SchemeID)  





	--SET @perTraineeCostLastMonth = @totalCostPerClass * 0.2;  
	--SET @perTraineeCost2ndLast = @totalCostPerClass * 0.1;  


	--set @perTraineeCost = @totalCostPerClass / @traineesPerClass;  

	--SET @U_Cost_Trainee_FMonth = @perTraineeCost * 0.7;  
	--SELECT @U_Cost_Trainee_Month = @perTraineeCost * 0.7;  
	--SET @U_Cost_Trainee_LMonth = @perTraineeCost * @batchDuration * 0.2;  
	--SET @U_Cost_Trainee_2Month = @perTraineeCost * 0.7;  
	--SET @U_Cost_Trainee_2nd_Last = @perTraineeCost * 0.1;  

	SELECT
		@U_Cost_Trainee_FMonth = Amount
	FROM ClassInvoiceMap
	WHERE ClassID = @classID
	AND InvoiceNo = 1
	AND InvoiceType = 'Regular';

	--DECLARE @UniformBagCost FLOAT = 0;  
	--SELECT @UniformBagCost = UniformBagCost  
	--FROM Class  
	--WHERE ClassID = @classID;  

	SET @U_Cost_Trainee_FMonth = @U_Cost_Trainee_FMonth + @UniformAndBagCost;

	SELECT
		@U_Cost_Trainee_2Month = Amount
	FROM ClassInvoiceMap
	WHERE ClassID = @classID
	AND InvoiceNo = 2
	AND InvoiceType = 'Regular';

	SELECT
		@U_Cost_Trainee_Month = Amount
	FROM ClassInvoiceMap
	WHERE ClassID = @classID
	AND InvoiceNo = 3
	AND InvoiceType = 'Regular';

	SELECT
		@U_Cost_Trainee_2nd_Last = Amount
	FROM ClassInvoiceMap
	WHERE ClassID = @classID
	AND InvoiceType = '2nd Last';
	SELECT
		@U_Cost_Trainee_LMonth = Amount
	FROM ClassInvoiceMap
	WHERE ClassID = @classID
	AND InvoiceType = 'Final'; -- final = employment  

	--select * from class  
	SELECT
		@PerMonthPerTraineeCostExTax = TrainingCostPerTraineePerMonthExTax
	   ,@ClassDuration = Duration
	FROM Class
	WHERE ClassID = @classID;

	DECLARE @intDuration INT = 0;
	SET @intDuration = @ClassDuration;

	---s--updated by numan at 2021-11-15   
	--IF(@ClassDuration < 1)  
	--BEGIN  
	-- SET @lineTotal  
	--    = (@PerMonthPerTraineeCostExTax * @ClassDuration * @traineesPerClass)  
	--                   + (@UniformAndBagCost * @traineesPerClass);  
	--END  
	--ELSE  
	--BEGIN  
	-- IF ((@ClassDuration % @intDuration) > 0)  
	-- BEGIN  
	--  SET @lineTotal  
	--   = (@U_Cost_Trainee_FMonth + @U_Cost_Trainee_2Month + @U_Cost_Trainee_Month) * @traineesPerClass  
	--     + (@U_Cost_Trainee_LMonth * @traineesPerClass) --+ (@UniformAndBagCost * @traineesPerClass)  
	--     + (@U_Cost_Trainee_2nd_Last * @traineesPerClass);  
	-- END;  
	-- ELSE  
	-- BEGIN  
	--  SET @lineTotal  
	--   = (@PerMonthPerTraineeCostExTax * @ClassDuration * @traineesPerClass)  
	--     + (@UniformAndBagCost * @traineesPerClass);  
	-- END;  
	--END  
	IF (@ClassDuration < 1)
	BEGIN
		SET @lineTotal
		= (@PerMonthPerTraineeCostExTax * @traineesPerClass)
		+ (@UniformAndBagCost * @traineesPerClass);
	END
	ELSE
	BEGIN
		BEGIN
			SET @lineTotal
			= (@PerMonthPerTraineeCostExTax * @ClassDuration * @traineesPerClass)
			+ (@UniformAndBagCost * @traineesPerClass);
		END;
	END
	---e--updated by numan at 2021-11-15   

	--IF ((@ClassDuration % @intDuration) > 0)  
	--BEGIN  
	--    SET @lineTotal  
	--        = (@U_Cost_Trainee_FMonth + @U_Cost_Trainee_2Month + @U_Cost_Trainee_Month) * @traineesPerClass  
	--          + (@U_Cost_Trainee_LMonth * @traineesPerClass) --+ (@UniformAndBagCost * @traineesPerClass)  
	--          + (@U_Cost_Trainee_2nd_Last * @traineesPerClass);  
	--END;  
	--ELSE  
	--BEGIN  
	--    SET @lineTotal  
	--        = (@PerMonthPerTraineeCostExTax * @ClassDuration * @traineesPerClass)  
	--          + (@UniformAndBagCost * @traineesPerClass);  
	--END;  

	SET @DocTotal = @DocTotal + @lineTotal;

	----This check is added by Numan Bin Tariq at 2020-03-04 -----S---  
	--IF @batchDuration <= 1  
	--   AND  
	--   (  
	--       SELECT COUNT(*)  
	--       FROM ClassInvoiceMap  
	--       WHERE ClassID = @classID  
	--             AND InvoiceType = 'Regular'  
	--   ) > 1  
	--BEGIN  
	--    DECLARE @regularAmount FLOAT =  
	--            (  
	--                SELECT SUM(Amount)  
	--                FROM ClassInvoiceMap  
	--                WHERE ClassID = @classID  
	--                      AND InvoiceType = 'Regular'  
	--            );  

	--    SET @U_Cost_Trainee_FMonth = @regularAmount + @UniformAndBagCost;  
	--    SET @U_Cost_Trainee_2Month = @regularAmount;  
	--END;  
	----This check is added by Numan Bin Tariq at 2020-03-04 -----E---  

	SELECT TOP 1
		@U_Class_Start_Date = POStartDate
	   ,@U_Class_End_Date = POEndDate
	FROM ClassInvoiceMap
	WHERE ClassID = @classID
	ORDER BY InvoiceID

	INSERT INTO POLines (DocEntry
	, DocNumber
	, LineNum
	, Dscription
	, AcctCode
	, OcrCode
	, TaxCode
	, WtLiable
	, LineTotal
	, LineStatus
	, OcrCode2
	, OcrCode3
	, U_Class_Code
	, U_Batch
	, U_Batch_Duration
	, U_Training_Cost
	, U_Stipend
	, U_Uniform_Bag
	, U_Trainee_Per_Class
	, U_Testing_Fee
	, U_Cost_Trainee_Month
	, U_Cost_Trainee_LMont
	, U_Cost_Trainee_FMont
	, U_Cost_Trai_2nd_Last
	, U_Class_Start_Date
	, U_Class_End_Date
	, U_Cost_Trainee_2Mont
	, CreatedUserID
	, IsMigrated
	, POHeaderID)
		VALUES (@docEntry, 1, @lineNum, @description, @acctCode, @schemeCode, @taxCode, @wTLiable, @lineTotal, @lineStatus, @ocrCode2, @ocrCode3, @classCode, @batch, @batchDuration, @perTraineeCostOfTheMonth, @perTraineeStipend, @UniformAndBagCost, @traineesPerClass, 0, @U_Cost_Trainee_Month, @U_Cost_Trainee_LMonth, @U_Cost_Trainee_FMonth, @U_Cost_Trainee_2nd_Last, @U_Class_Start_Date, @U_Class_End_Date, @U_Cost_Trainee_2Month, @CurUserID, 0, @ident);

	SET @lineNum += 1;

	FETCH NEXT FROM poLineCursor
	INTO @schemeCode
	, @schemeName
	, @paymentStructure
	, @classCode
	, @classID
	, @cardCode
	, @tspid
	, @tspName
	, @batch
	, @tradeName
	, @pCatID
	, @perTraineeCostOfTheMonth
	, @batchDuration
	, @traineesPerClass
	, @UniformAndBagCost
	, @perTraineeStipend
	, @totalCostPerClass
	, @U_Testing_Fee
	, @U_Class_Start_Date
	, @U_Class_End_Date
	, @perTraineeCost;

	END;

	CLOSE poLineCursor;
	DEALLOCATE poLineCursor;
	-- POLines Ending  

	-- Updating POHeader  
	-- SET @Comments = CONCAT(@schemeName, @schemeCode);  
	-- SET @JournalMemo = CONCAT(@schemeName, @schemeCode);  

	UPDATE POHeader
	SET DocDate = GETDATE()
	   ,DocDueDate = GETDATE()
		-- , CardCode = @cardCode  
		--, CardName = @tspName  
	   ,TspID = @tspid
	   ,DocTotal = @DocTotal
	--, Comments = @Comments  
	--, JournalMemo = @JournalMemo  
	WHERE POHeaderID = @ident;
	--   

	SET @DocTotal = 0; -- Setting DocTOTAL back to 0 after every iteration at tsp level  

	-- Now send POHeader to First Approver  

	INSERT INTO dbo.ApprovalHistory (ProcessKey
	, Step
	, FormID
	, ApproverID
	, ApprovalStatusID
	, Comments
	, CreatedUserID
	, CreatedDate
	, ModifiedUserID
	, ModifiedDate
	, InActive)
		VALUES (@ProcessKey -- ProcessKey - nvarchar(100)  
		, 1           -- Step - int  
		, @ident      -- FormID - int  
		, NULL        -- ApproverID - int  
		, 1           -- ApprovalStatusID - int  
		, N'Pending'  -- Comments - nvarchar(4000)  
		, @CurUserID  -- CreatedUserID - int  
		, GETDATE()   -- CreatedDate - datetime  
		, NULL        -- ModifiedUserID - int  
		, NULL        -- ModifiedDate - datetime  
		, 0           -- InActive - bit  
		);

	FETCH NEXT FROM tspMasterCursor
	INTO @TSPMasterID;

	END;

	CLOSE tspMasterCursor;
	DEALLOCATE tspMasterCursor;

	--MERGE SSP UserAccounts IN SSP Scheme

	IF @ProgramType = 10
	BEGIN
		EXEC AU_SSPMergeUserAccounts @SchemeID = @SchemeID
	END


END;
------------------------------
GO
ALTER PROCEDURE [dbo].[GenerateInvoiceRegular] @InvoiceHeaderID INT
, @ClassCode NVARCHAR(100)
, @InvoiceNumber INT
, @ProcessKey NVARCHAR(50)
, @PRNID INT
, @POLineNum INT
, @PODocEntry INT
, @PODocNum INT
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @GLAccountCode NVARCHAR(100);
	DECLARE @GLName NVARCHAR(100);
	DECLARE @WTLiable NVARCHAR(10);
	DECLARE @TaxCode NVARCHAR(100);
	DECLARE @DaysInAMonth INT = 30; -- should be in config maybe?
	DECLARE @FirstNonFunctVisitPenalty DECIMAL(18, 2) = 5; -- (5%) should be in config maybe?

	DECLARE @seriousViolCounts INT = 0;
	DECLARE @majorViolCounts INT = 0;
	DECLARE @minorViolCounts INT = 0;

	DECLARE @ClassID INT;
	DECLARE @ClassUID NVARCHAR(250);
	DECLARE @SchemeID INT;
	DECLARE @TradeID INT;
	DECLARE @Month DATE;
	DECLARE @Duration DECIMAL(18, 2);
	DECLARE @Description NVARCHAR(100);
	DECLARE @PCatID INT;
	DECLARE @FSName NVARCHAR(150);
	DECLARE @Type NVARCHAR(100) = N'First';
	DECLARE @InvoiceType NVARCHAR(100) = N'First';
	DECLARE @TraineesPerClass INT;
	DECLARE @ClaimedTrainees INT;
	--DECLARE @Stipend INT;
	DECLARE @UnfirmBag FLOAT;
	DECLARE @UnfirmBag_Remb_SI FLOAT;
	DECLARE @Boarding INT;
	DECLARE @TestingFee FLOAT = 0;
	DECLARE @TotalCostPerTrainee FLOAT;
	DECLARE @TotalCostPerTrainee1 FLOAT;
	DECLARE @Batch INT;
	DECLARE @BatchDuration INT;
	DECLARE @StartDate DATETIME;
	DECLARE @EndDate DATETIME;
	DECLARE @InvoiceStartDate DATETIME;
	DECLARE @InvoiceEndDate DATETIME;
	DECLARE @ActualStartDate DATETIME;
	DECLARE @ActualEndDate DATETIME;
	DECLARE @ClassDays INT = 30;
	DECLARE @TotalMonthlyPayment FLOAT;
	DECLARE @TotalMonthlyPayment1 FLOAT;
	DECLARE @TotalMonthlyPaymentGross FLOAT;
	DECLARE @TotalMonthlyPaymentGross1 FLOAT;
	DECLARE @GrossPayable FLOAT;
	DECLARE @UnverifiedCNICDeductionsSinceInception INT = 0;
	DECLARE @CNICDeductionAmountSinceInception FLOAT = 0;
	DECLARE @UnVCNICDeductionCountForTheMonth INT = 0;
	DECLARE @UnVCNICDeductionAmountForTheMonth FLOAT = 0;
	DECLARE @CNICDeductionTypeForTheMonth NVARCHAR(100) = N'';
	DECLARE @DeductionTraineeDroput INT = 0;
	DECLARE @DropOutDeductionType NVARCHAR(100) = N'';
	DECLARE @DropOutDeductionAmount FLOAT = 0;
	DECLARE @DeductionTraineeAttendance FLOAT = 0;
	DECLARE @AttendanceDeductionAmount FLOAT = 0;
	DECLARE @DeductionTraineeUnVCnic INT = 0;
	DECLARE @MiscDeductionNo INT = 0;
	DECLARE @MiscDeductionType NVARCHAR(100) = N'';
	DECLARE @MiscDeductionAmount FLOAT = 0;
	DECLARE @FunctionalDate DATETIME;
	DECLARE @PenaltyPercentage FLOAT = 0;
	DECLARE @PenaltyAmount FLOAT;
	DECLARE @ResultDeduction FLOAT;
	DECLARE @NetPayableAmount FLOAT;
	DECLARE @NetTrainingCost FLOAT;
	DECLARE @PenaltyTPMReports FLOAT;
	DECLARE @PaymentToBeReleased FLOAT;
	DECLARE @EnrolledTrainees INT;
	DECLARE @UniformBagCost FLOAT;
	DECLARE @CNICVerifiedTrainees INT;
	DECLARE @DeductionFailedTrainees INT;
	DECLARE @DeducMarginalCount INT = 0;
	DECLARE @DeductMarginalAmount FLOAT = 0;
	DECLARE @DeductDropoutForTheMonthCount INT = 0;
	DECLARE @DeductDropoutForTheMonthAmount FLOAT = 0;
	DECLARE @OcrCode NVARCHAR(100) = N'';
	DECLARE @OcrCode2 NVARCHAR(100) = N'';
	DECLARE @OcrCode3 NVARCHAR(100) = N'';
	DECLARE @OcrCode4 NVARCHAR(100) = N'';
	DECLARE @lineStatus NVARCHAR(50) = N'';
	DECLARE @DropOut INT;
	DECLARE @PaymentWithheldPhysicalCount INT;
	DECLARE @CNICUnverified INT;
	DECLARE @ExpelledVerified INT;
	DECLARE @ExpelledUnverified INT;
	DECLARE @NonFunctionInAllVisits BIT = 'False';
	DECLARE @ProgramType INT;
	-- DECLARE @PCategoryID INT;




	--SELECT @TaxCode  = TaxCode
	--     , @WTLiable = WTLiable
	--FROM dbo.SAP_StaticField
	--WHERE SAP_StaticFileldID = 1;

	SELECT
		@TaxCode = TaxCode
	   ,@WTLiable = WTLiable
	   ,@lineStatus = LineStatus
	--, @ocrCode3 = OcrCode3
	--, @ocrCode = OcrCode
	-- , @CtlAccount = CtlAccount
	--, @U_SCH_Code = U_SCH_Code
	-- , @U_SCHEME = N'C1120'
	FROM dbo.SAP_StaticField AS ssf
	WHERE ssf.SAP_StaticFileldID = 1;

	SELECT
		@OcrCode = U_SCH_Code
	   ,@OcrCode4 = BPL_IDAssignedToInvoice
	FROM dbo.InvoiceHeader
	WHERE InvoiceHeaderID = @InvoiceHeaderID;

	SELECT
		@UniformBagCost = c.UniformBagCost
	   ,@SchemeID = c.SchemeID
	   ,@ClassID = c.ClassID
	   ,@ClassUID = c.UID
	   ,@Duration = c.Duration
	   ,@TraineesPerClass = c.TraineesPerClass
		-- , @Stipend                                = c.Stipend
	   ,@UnfirmBag = c.UniformBagCost
	   ,@UnfirmBag_Remb_SI = c.UniformBagCost
	   ,@Boarding = c.BoardingAllowancePerTrainee
	   ,@Batch = c.Batch
	   ,@BatchDuration = c.Duration
	   ,@StartDate = c.StartDate
	   ,@EndDate = c.EndDate
	   ,@TotalMonthlyPayment = cim.Amount
	   ,@TotalMonthlyPaymentGross = cim.Amount
	   ,@TotalCostPerTrainee = cim.Amount
	   ,@Month = cim.month
	   ,@ClassDays = ISNULL(cim.InvoiceDays, 0)
	   ,@InvoiceStartDate = cim.InvoiceStartDate
	   ,@InvoiceEndDate = cim.InvoiceEndDate
	   ,@ClaimedTrainees = prn.ClaimedTrainees
	   ,@EnrolledTrainees = prn.EnrolledTrainees
	   ,@UnverifiedCNICDeductionsSinceInception = prn.PaymentWithheldSinIncepUnVCNIC
	   ,@UnVCNICDeductionCountForTheMonth = prn.CNICUnverified
	   ,@DeductionTraineeDroput = prn.DeductionSinIncepDropout
	   ,@DeductionTraineeAttendance = prn.PaymentWithheldPhysicalCount
	   ,@FunctionalDate = prn.ClassStartDate
	   ,@PenaltyPercentage = prn.PenaltyTPMReports
	   ,@DeductionFailedTrainees = prn.DeductionFailedTrainees
	   ,@DeducMarginalCount = prn.DeductionMarginal
	   ,@CNICVerifiedTrainees = prn.CNICVerified
	   ,@DeductDropoutForTheMonthCount
		= (prn.DropoutsVerified + prn.DropoutsUnverified + prn.ExpelledVerified + prn.ExpelledUnverified)
	   ,@ExpelledVerified = prn.ExpelledVerified
	   ,@ExpelledUnverified = prn.ExpelledUnverified
	   ,@DropOut = prn.DropOut
	   ,@PaymentToBeReleased = prn.PaymentToBeReleasedTrainees
	   ,@PaymentWithheldPhysicalCount = prn.PaymentWithheldPhysicalCount
	   ,@CNICUnverified = prn.CNICUnverified
	   ,@ActualStartDate = ir.ActualStartDate
	   ,@ActualEndDate = ir.ActualEndDate
	   ,@TradeID = tr.TradeID
	   ,@OcrCode2 = tr.SAPID
	   ,@Description = CONCAT(c.ClassCode, ' - ', CAST(c.Batch AS NVARCHAR(10)), ' - ', tr.TradeName)
	   ,@PCatID = pc.PCategoryID
	   ,@FSName = fs.FundingSourceName
	   ,@TestingFee = 0
	   ,@OcrCode3 =
		CASE
			WHEN s.PCategoryID = 1 THEN 'BD011'
			ELSE CASE
					WHEN s.PCategoryID = 2 THEN 'PD002'
				END
		END
	   ,@ProgramType = s.ProgramTypeID
	FROM dbo.Class AS c
	INNER JOIN dbo.Trade AS tr
		ON c.TradeID = tr.TradeID
	INNER JOIN dbo.TSPDetail AS td
		ON td.TspID = c.TspID
	INNER JOIN dbo.Scheme AS s
		ON s.SchemeID = td.SchemeID
	INNER JOIN dbo.ProgramCategory AS pc
		ON pc.PCategoryID = s.PCategoryID
	INNER JOIN dbo.FundingSource AS fs
		ON fs.FundingSourceID = s.FundingSourceID
	INNER JOIN dbo.PRN AS prn
		ON prn.ClassCode = c.ClassCode
	INNER JOIN dbo.ClassInvoiceMap AS cim
		ON cim.ClassID = c.ClassID
	INNER JOIN dbo.InceptionReport AS ir
		ON ir.ClassID = c.ClassID
	WHERE c.ClassCode = @ClassCode
	AND prn.PRNID = @PRNID
	AND cim.InvoiceNo = @InvoiceNumber
	AND cim.InvoiceType = 'Regular';
	--c.ClassCode = 'PU132-87-206' AND cim.InvoiceNo = 1 AND prn.PRNID = 2 AND cim.InvoiceType = 'Regular';



	--Updated by Ali Haider on 22-Apl-2024
	-----Get @GLAccountCode according to program type if SSP then update Scholarship Cost upon Training Cost
	IF (@ProgramType = 10)
	BEGIN
		SELECT
			@GLAccountCode = GLAccountCode
		   ,@GLName = GLName
		FROM dbo.SAP_GLAccount
		WHERE GLAccountID = 7;
	END
	ELSE
	BEGIN
		SELECT
			@GLAccountCode = GLAccountCode
		   ,@GLName = GLName
		FROM dbo.SAP_GLAccount
		WHERE GLAccountID = 1;
	END

	-------------End updation------------


	SET @TotalMonthlyPaymentGross1 = @TotalMonthlyPaymentGross
	SET @TotalMonthlyPayment1 = @TotalMonthlyPayment
	SET @TotalCostPerTrainee1 = @TotalCostPerTrainee


	--DECLARE @deptCode INT =
	--        (
	--            SELECT s.PCategoryID
	--            FROM Class
	--                INNER JOIN Scheme s ON s.SchemeID = Class.SchemeID
	--            WHERE Class.ClassID = @ClassID
	--        );
	--DECLARE @tradeidsap NVARCHAR(100) = N'';
	--SET @tradeidsap =
	--(
	--    SELECT t.SAPID
	--    FROM Trade t
	--        INNER JOIN Class ON Class.TradeID = t.TradeID
	--    WHERE Class.ClassID = @ClassID
	--);
	--SET @OcrCode2 = @tradeidsap;

	--IF (@PCategoryID = 1) --6 dev db, Business Development
	--BEGIN
	--    SET @PCategoryID = N'01';
	--    SET @OcrCode3 = N'BD011';
	----SET @ocrCode2 = @tradeidsap+'-01';
	--END;
	--ELSE IF (@PCategoryID = 2) --7 dev db, Program Development
	--BEGIN
	--    SET @PCategoryID = N'00';
	--    SET @OcrCode3 = N'PD002';
	----SET @ocrCode2 = @tradeidsap+'-00';
	--END;
	IF @ClassDays <> 30
	BEGIN
		SET @TotalMonthlyPaymentGross = @TotalMonthlyPaymentGross / 2
		SET @TotalMonthlyPayment = @TotalMonthlyPayment / 2
		SET @TotalCostPerTrainee = @TotalCostPerTrainee / 2
		SET @DaysInAMonth = 15
	END;

	IF @EnrolledTrainees < @ClaimedTrainees
	BEGIN
		SET @ClaimedTrainees = @EnrolledTrainees;
	END;

	DECLARE @ExtraCountFromClass INT = 0;
	SET @ExtraCountFromClass = @EnrolledTrainees - @TraineesPerClass;

	IF @ExtraCountFromClass > 0
	BEGIN
		SET @DeductDropoutForTheMonthCount = @DeductDropoutForTheMonthCount - @ExtraCountFromClass;
	END;

	--/* Rule added for the Dropouts exclusion */

	--IF @ClaimedTrainees < @EnrolledTrainees
	--BEGIN
	--    SET @DeductDropoutForTheMonthCount = 0;
	--END;
	--/* Rule added for the Dropouts exclusion */


	IF (@DeductDropoutForTheMonthCount < 0)
	BEGIN
		SET @DeductDropoutForTheMonthCount = 0;
	END;

	IF (@InvoiceNumber = 1)
	BEGIN
		SET @TotalMonthlyPayment = @TotalMonthlyPayment + @UniformBagCost;
		SET @TotalMonthlyPaymentGross = @TotalMonthlyPaymentGross + @UniformBagCost;
		--SET @Stipend = 0;
		SET @Boarding = 0;
	END;

	IF (@InvoiceNumber = 2)
	BEGIN
		SET @InvoiceType = N'2ndInv';
		--SET @Stipend = 0;
		SET @UnfirmBag = 0;
		SET @Boarding = 0;
	END;

	IF (@InvoiceNumber > 2)
	BEGIN
		SET @InvoiceType = N'Regular';
		-- SET @Stipend = 0;
		SET @UnfirmBag = 0;
		SET @Boarding = 0;
	END;

	DECLARE @NonFunc INT
		   ,@StartDay INT
		   ,@CostPerDay FLOAT
		   ,@ClassStartDay INT = DAY(@StartDate)
		   ,@ClassEndDay INT = DAY(@EndDate);

	--SET @DaysInAMonth = 30;

	SET @PenaltyAmount = (@PenaltyPercentage / 100) * (@TotalCostPerTrainee * @ClaimedTrainees);
	IF (@PenaltyPercentage > 100)
	BEGIN
		SET @PenaltyAmount = (100 / 100) * (@TotalCostPerTrainee * @ClaimedTrainees);
	END;

	SET @NetTrainingCost
	= ((@PaymentToBeReleased - @DeductionTraineeAttendance) * @TotalCostPerTrainee) - @PenaltyAmount;

	---Non-Functional Calcualtions of Deduction---- S----
	----Get Current Months Visits---
	DECLARE @NonFuncDeduct FLOAT = 0
		   ,@FunctionalDay INT
		   ,@minVisit INT = 0;

	SELECT
		cm.VisitNo
	   ,cv.VisitDateTime
	   ,CASE
			WHEN cm.IsLock = 'y' THEN 0
			ELSE 1
		END IsFunctional INTO #currentVisits
	FROM dbo.AMSClassViolations cv
	INNER JOIN dbo.AMSClassMonitoring cm
		ON cv.ClassMonitoringID = cm.ID
	WHERE cv.ClassID = @ClassUID
	AND MONTH(cv.VisitDateTime) = MONTH(@Month)
	AND YEAR(cv.VisitDateTime) = YEAR(@Month)
	AND cv.ViolationID = 18;
	--Non Functional in all visits of current month
	IF (SELECT
				COUNT(*)
			FROM #currentVisits)
		> 0
		AND (SELECT
				COUNT(*)
			FROM #currentVisits)
		= (SELECT
				COUNT(*)
			FROM #currentVisits AS tv
			WHERE tv.IsFunctional = 0)
	BEGIN
		SET @NonFuncDeduct = @NetTrainingCost;
		SET @NonFunctionInAllVisits = 'True';
	END
	ELSE
	IF (@InvoiceNumber = 1) -- 1st Month
	BEGIN
		IF (SELECT
					COUNT(*)
				FROM #currentVisits)
			!= (SELECT
					COUNT(*)
				FROM #currentVisits AS tv
				WHERE tv.IsFunctional = 1)
		BEGIN
			PRINT 'found non functional in any 1 visit of current month';
			SET @minVisit = (SELECT
					ISNULL(MIN(tv.VisitNo), 0)
				FROM #currentVisits AS tv
				WHERE tv.IsFunctional = 1);
			IF @minVisit > 1
			BEGIN
				--if(@Duration = 0.5) BEGIN SET @DaysInAMonth = 15; END
				SET @CostPerDay = @NetTrainingCost / @DaysInAMonth;
				SET @FunctionalDay = (SELECT
						ISNULL(MIN(DAY(tv.VisitDateTime)), 0)
					FROM #currentVisits AS tv
					WHERE tv.IsFunctional = 1);

				IF @FunctionalDay > 0
				BEGIN
					SET @NonFuncDeduct = @CostPerDay * (@FunctionalDay - @ClassStartDay);
				END;
				ELSE
				BEGIN
					SET @NonFuncDeduct = @CostPerDay * @DaysInAMonth;
				END;
			END;
		END;
	END;

	IF @NonFuncDeduct > 0
	BEGIN
		SET @MiscDeductionNo = @ClaimedTrainees - (@CNICUnverified + @DropOut + @DeductionTraineeAttendance);
		--SET @MiscDeductionAmount = @NonFuncDeduct * @MiscDeductionNo;
		SET @MiscDeductionAmount = @NonFuncDeduct;
		SET @MiscDeductionType = N'Non Functional';
	END;

	---Non-Functional Calcualtions of Deduction---- E----

	IF (@UnVCNICDeductionCountForTheMonth > 0)
	BEGIN
		SET @UnVCNICDeductionAmountForTheMonth = @UnVCNICDeductionCountForTheMonth * @TotalMonthlyPayment;
		SET @CNICDeductionTypeForTheMonth = N'UNCNICFTM'; --UN-CNIC For The Month, UNCNICFSI for U-CNIC Since Inception
	END;

	DECLARE @dropcat NVARCHAR(100) = N'';
	IF @DeductDropoutForTheMonthCount > 0
	BEGIN
		SET @DeductDropoutForTheMonthAmount = @DeductDropoutForTheMonthCount * @TotalMonthlyPayment;
		SET @dropcat = N'DFM';
	END;
	SET @DeductMarginalAmount = @DeducMarginalCount * @TotalMonthlyPaymentGross;
	SET @GrossPayable = @TotalMonthlyPaymentGross * @ClaimedTrainees;
	SET @DropOutDeductionAmount = @TotalMonthlyPaymentGross * @DeductionTraineeDroput;
	SET @CNICDeductionAmountSinceInception = @TotalMonthlyPaymentGross * @TotalMonthlyPayment;
	--bookmark
	SET @AttendanceDeductionAmount = @DeductionTraineeAttendance * @TotalCostPerTrainee;
	--SET @PenaltyAmount = (@PenaltyPercentage / 100) * (@TotalMonthlyPayment * @PaymentToBeReleased)
	--SET @ResultDeduction = @TotalMonthlyPayment * @DeductionFailedTrainees;
	SET @ResultDeduction = 0;
	--SET @NetTrainingCost = @TotalMonthlyPayment * @PaymentToBeReleased;

	--SET @NetPayableAmount = @GrossPayable - (@AttendanceDeductionAmount + @PenaltyAmount + @ResultDeduction+@NonFuncDeduct
	--+@DeductDropoutForTheMonthAmount+@DeductMarginalAmount+@UnVCNICDeductionAmountForTheMonth);
	--SET @NetTrainingCost = @NetPayableAmount;
	--if(@InvoiceNumber = 1)
	--begin
	-- set @NetTrainingCost = @NetTrainingCost -(@TraineesPerClass*@UniformBagCost);
	--end
	--else
	--begin
	--  set @NetTrainingCost = @NetTrainingCost -((@DeductionTraineeDroput*@TotalCostPerTrainee) + @PenaltyAmount + (@DeductionFailedTrainees*@TotalCostPerTrainee)+@NonFuncDeduct+(@UnVCNICDeductionCountForTheMonth*@TotalCostPerTrainee));
	--end

	--fix on date 2020-01-18
	--SET @NetPayableAmount
	--    = (@ClaimedTrainees - (@DropOut + @CNICUnverified + @PaymentWithheldPhysicalCount)) * @TotalMonthlyPayment;

	SET @NetPayableAmount
	= (@ClaimedTrainees * @TotalMonthlyPayment)
	- (@UnVCNICDeductionAmountForTheMonth + @DeductDropoutForTheMonthAmount + @AttendanceDeductionAmount
	+ @MiscDeductionAmount + @PenaltyAmount + @ResultDeduction
	);
	--SET @NetPayableAmount=(@CNICUnverified*@TotalMonthlyPayment)
	--+(@DeductionTraineeDroput*@TotalMonthlyPayment) 
	--+(@DeductionTraineeAttendance*@TotalCostPerTrainee)
	--+(@MiscDeductionNo*@TotalCostPerTrainee)
	--+(CASE WHEN @ClaimedTrainees<@EnrolledTrainees THEN(@ClaimedTrainees *@TotalCostPerTrainee)*@PenaltyPercentage ELSE (@EnrolledTrainees *@TotalCostPerTrainee)*@PenaltyPercentage END )

	--SET @NetTrainingCost
	--    = (@ClaimedTrainees - (@DropOut + @CNICUnverified + @PaymentWithheldPhysicalCount)) * @TotalCostPerTrainee;

	--SET @NetTrainingCost = (@PaymentToBeReleased - @DeductionTraineeAttendance) * @TotalCostPerTrainee;


	--this check removed by numan at 2021-03-11--S-
	--IF @NetTrainingCost < 0
	--BEGIN
	--    SET @NetTrainingCost = 0;
	--END;
	--this check removed numan at 2021-03-11--E-

	---  second month onwards


	--IF (@DeductDropoutForTheMonthAmount > 0)
	--BEGIN
	--    SET @dropcat = N'DFM';
	--END;
	IF @NonFunctionInAllVisits = 'True'
	BEGIN
		SET @MiscDeductionType = N'Non Functional';
		SET @MiscDeductionAmount = @NetPayableAmount;
		SET @NetPayableAmount = 0;
	END
	--INSERT INTO BSS.DBO.MYTABLE (InvoiceHeaderID, ClassCode, InvoiceNumber, ProcessKey, PRNID, POLineNum, PODocEntry
	-- , PODocNum, NonFunctionInAllVisits, MiscDeductionAmount)
	--SELECT @InvoiceHeaderID, @ClassCode, @InvoiceNumber, @ProcessKey, @PRNID, @POLineNum, @PODocEntry, @PODocNum, 
	--		@NonFunctionInAllVisits, @MiscDeductionAmount


	INSERT INTO dbo.Invoice (SchemeID
	, ClassID
	, TradeID
	, Description
	, GLCode
	, GLName
	, TrainingServicesSaleTax
	, ProgCategory
	, FundingSource
	, TaxCode
	, WTaxLiable
	, InvoiceType
	, TraineePerClass
	, ClaimTrainees
	, Stipend
	, UniformBag
	, BoardingOrLodging
	, TestingFee
	, TotalCostPerTrainee
	, UnverifiedCNICDeductions
	, CnicDeductionType
	, CnicDeductionAmount
	, DeductionTraineeDroput
	, DropOutDeductionType
	, DropOutDeductionAmount
	, DeductionTraineeAttendance
	, AttendanceDeductionAmount
	, MiscDeductionNo
	, MiscDeductionType
	, MiscDeductionAmount
	, FunctionalDate
	, PenaltyPercentage
	, PenaltyAmount
	, ResultDeduction
	, NetPayableAmount
	, NetTrainingCost
	, InActive
	, CreatedUserID
	, ModifiedUserID
	, CreatedDate
	, ModifiedDate
	, StartDate
	, EndDate
	, ActualStartDate
	, ActualEndDate
	, Batch
	, BatchDuration
	, TotalMonthlyPayment
	, GrossPayable
	, DeductionTraineeUnVCnic
	, ClassDays
	, IsApproved
	, month
	, IsPostedToSAP
	, IsRejected
	, ProcessKey
	, InvoiceHeaderID
	, SAPID
	, InvoiceNumber
	, MiscMarginalTraineeCount
	, MiscMarginalTraineeAmount
	, MiscProfileDeductionCount
	, MiscProfileDeductionAmount
	, MiscFailedDeductionCount
	, MiscFailedDeductionAmount
	, TotalLC
	, MiscNonFunctionalAmount
	, LineTotal
	, OcrCode
	, OcrCode2
	, OcrCode3
	, LineStatus
	, OcrCode4
	, BaseEntry
	, BaseType
	, BaseLine
	, InCancel
	, PaymentToBeReleasedTrainees)
		VALUES (@SchemeID                          -- SchemeID - int
		, @ClassID                           -- ClassID - int
		, @TradeID                           -- TradeID - int
		, @Description                       -- Description - nvarchar(250)
		, @GLAccountCode                     -- GLCode - nvarchar(250)
		, @GLName                            -- GLName - nvarchar(250)
		, (SELECT GLAccountCode FROM SAP_GLAccount WHERE GLAccountID = 4)                                  -- TrainingServicesSaleTax - nvarchar(250)
		, @PCatID                            -- ProgCategory - int
		, @FSName                            -- FundingSource - nvarchar(250)
		, @TaxCode                           -- TaxCode - nvarchar(250)
		, @WTLiable                          -- WTaxLiable - nvarchar(10)
		, @InvoiceType                       -- InvoiceType - nvarchar(250)
		, @TraineesPerClass                  -- TraineePerClass - int
		, @ClaimedTrainees                   -- ClaimTrainees - int
		, 0                                  -- Stipend - decimal(19, 4)
		, @UnfirmBag                         -- UniformBag - decimal(19, 4)
		, 0                                  -- BoardingOrLodging - decimal(19, 4)
		, 0                                  -- TestingFee - decimal(19, 4)
		, @TotalCostPerTrainee               -- TotalCostPerTrainee - decimal(19, 4)
		, @UnVCNICDeductionCountForTheMonth  -- UnverifiedCNICDeductions - decimal(19, 4)
		, @CNICDeductionTypeForTheMonth      -- CnicDeductionType - nvarchar(250)
		, @UnVCNICDeductionAmountForTheMonth -- CnicDeductionAmount - decimal(19, 4)
		, @DeductDropoutForTheMonthCount     -- DeductionTraineeDroput - int
		, @dropcat                           -- DropOutDeductionType - nvarchar(250)
		, @DeductDropoutForTheMonthAmount    -- DropOutDeductionAmount - decimal(19, 4)
		, @DeductionTraineeAttendance        -- DeductionTraineeAttendance - decimal(19, 4)
		, @AttendanceDeductionAmount         -- AttendanceDeductionAmount - decimal(19, 4)
		, @MiscDeductionNo                   -- MiscDeductionNo - int
		, @MiscDeductionType                 -- MiscDeductionType - nvarchar(250)
		, @MiscDeductionAmount               -- MiscDeductionAmount - decimal(19, 4)
		, @FunctionalDate                    -- FunctionalDate - datetime
		, (@PenaltyPercentage / 100)         -- PenaltyPercentage - decimal(19, 4)
		, @PenaltyAmount                     -- PenaltyAmount - decimal(19, 4)
		, @ResultDeduction                   -- ResultDeduction - decimal(19, 4)
		, @NetPayableAmount                  -- NetPayableAmount - decimal(19, 4)
		, @NetTrainingCost                   -- NetTrainingCost - decimal(19, 4)
		, 0                                  -- InActive - bit
		, 15                                 -- CreatedUserID - int
		, 0                                  -- ModifiedUserID - int
		, GETDATE()                          -- CreatedDate - datetime
		, ''                                 -- ModifiedDate - datetime
		, @InvoiceStartDate                  -- StartDate - date
		, @InvoiceEndDate                    -- EndDate - date
		, @ActualStartDate                   -- ActualStartDate - date
		, @ActualEndDate                     -- ActualEndDate - date
		, @Batch                             -- Batch - int
		, @BatchDuration                     -- BatchDuration - int
		, @TotalMonthlyPayment               -- TotalMonthlyPayment - decimal(19, 4)
		, @GrossPayable                      -- GrossPayable - decimal(19, 4)
		, @DeductionTraineeUnVCnic           -- DeductionTraineeUnVCnic - int
		, @ClassDays                         -- ClassDays - int
		, 0                                  -- IsApproved - bit
		, @Month                             -- Month - date
		, 0                                  -- IsPostedToSAP - bit
		, 0                                  -- IsRejected - bit
		, @ProcessKey                        -- ProcessKey - nvarchar(50)
		, @InvoiceHeaderID                   -- InvoiceHeaderID - int
		, N''                                -- SAPID - nvarchar(max)
		, @InvoiceNumber                     -- InvoiceNumber - int
		, @DeducMarginalCount                -- MiscMarginalTraineeCount - int
		, @DeductMarginalAmount              -- MiscMarginalTraineeAmount - decimal(19, 4)
		, 0                                  -- MiscProfileDeductionCount - int -not inserted in prev
		, 0                                  -- MiscProfileDeductionAmount - decimal(19, 4) -not inserted in prev
		, 0                                  -- MiscFailedDeductionCount - int -not inserted in prev
		, 0                                  -- MiscFailedDeductionAmount - decimal(19, 4) -not inserted in prev
		, @NetPayableAmount                  -- TotalLC - decimal(19, 4)
		, @NonFuncDeduct                     -- MiscNonFunctionalAmount - decimal(19, 4)
		, @GrossPayable                      -- LineTotal - decimal(19, 4)
		, @OcrCode                           -- OcrCode - nvarchar(100)
		, @OcrCode2                          -- OcrCode2 - nvarchar(100)
		, @OcrCode3                          -- OcrCode3 - nvarchar(100)
		, @lineStatus                        -- LineStatus - nvarchar(50)
		, @OcrCode4                          -- OcrCode4 - nvarchar(100)  -not inserted in prev
		, @PODocEntry                        -- BaseEntry - nvarchar(250)
		, N'22'                              -- BaseType - nvarchar(250)
		, @POLineNum                         -- BaseLine - nvarchar(250)
		, 0                                  -- InCancel - bit -not inserted in prev
		, @PaymentToBeReleased);

	DECLARE @InvoiceID INT = SCOPE_IDENTITY();
	DECLARE @ReimUnvTraineees INT = 0
		   ,@ReimAttendance INT = 0
		   ,@PreviousInvoiceDays INT = 0
		   ,@SinceInceptionDropout INT = 0
		   ,@SinceInceptionUnvCNIC INT = 0
			--, @TotalMonthlyPayment_temp    FLOAT
		   ,@TotalCostPerTrainee_Reim_SI FLOAT;

	SELECT
		@ReimUnvTraineees = ReimbursementUnVTrainees
	   ,@ReimAttendance = ReimbursementAttandance
	FROM dbo.PRN
	WHERE PRNID = @PRNID;

	--SELECT * FROM PRN

	---THIS CHECK IS ADDED BY NUMAN AT 2021-03-19---S-
	-----GET PREVIOUS MONTH PRN
	--DECLARE @ExpelledVerifiedForTheMonth INT;
	-----TOTAL_EXPELLED_VERIFIED - PREV_EXPELLED_VERIFIED = FOR_THE_MONTH_EXPELLED_VERIFIED
	--SELECT @ExpelledVerifiedForTheMonth = @ExpelledVerified - ISNULL(p.ExpelledVerified, 0)
	----INTO #prevPRN
	--FROM dbo.PRN                 p
	--    INNER JOIN dbo.PRNMaster AS pm ON pm.PRNMasterID = p.PRNMasterID
	--                                      AND pm.InActive = 0
	--WHERE p.ClassID = @ClassID
	--      AND p.InvoiceNumber = (@InvoiceNumber - 1)
	--      AND p.InActive = 0;
	---THIS ISSUE IS FIXED BY NUMAN AT 2021-03-19---E-
	---THIS CHECK IS ADDED BY Mubeen AT 2021-07-01---S-
	DECLARE @ExpelledVerifiedForTheMonth_Regular INT;
	SELECT
		@ExpelledVerifiedForTheMonth_Regular = COUNT(mtd.TraineeID)
	FROM dbo.MPR m
	INNER JOIN dbo.MPRTraineeDetail mtd
		ON m.MPRID = mtd.MPRID
			AND CAST(FORMAT(m.month, 'yyyyMM') AS INT) = CAST(FORMAT(@Month, 'yyyyMM') AS INT) -- Current month
	WHERE m.ClassID = @ClassID
	AND mtd.IsExtra = 0 -- Regular
	AND mtd.TraineeStatus = 4 -- Expelled
	AND mtd.TraineeStatusID = 1 -- Verified
	AND mtd.TraineeID IN (SELECT
			mtd1.TraineeID
		FROM dbo.MPR m1
		INNER JOIN dbo.MPRTraineeDetail mtd1
			ON m1.MPRID = mtd1.MPRID
			AND CAST(FORMAT(m1.month, 'yyyyMM') AS INT) = CAST(FORMAT(DATEADD(MONTH, -1, @Month), 'yyyyMM') AS INT) -- previous month
		WHERE m1.ClassID = m.ClassID
		AND mtd1.IsExtra = 0 -- Regular
		AND mtd1.TraineeStatus = 2 --On Rolled
	);
	---THIS CHECK IS ADDED BY Mubeen AT 2021-07-01---E-

	IF (@InvoiceNumber > 1)
	BEGIN
		SELECT
			@SinceInceptionDropout = DeductionSinIncepDropout
		   ,@SinceInceptionUnvCNIC = PaymentWithheldSinIncepUnVCNIC
		FROM dbo.PRN
		WHERE PRNID = @PRNID;
		--Added by Mubeen at 2021-05-18
		SET @SinceInceptionDropout = @SinceInceptionDropout + @ExpelledVerifiedForTheMonth_Regular;
		--Added by Mubeen at 2021-05-18
		IF (@SinceInceptionDropout > 0
			OR @SinceInceptionUnvCNIC > 0)
		BEGIN

			DECLARE @TotalMonthlyPayment_SI FLOAT = @TotalMonthlyPayment1 * (@InvoiceNumber - 1);
			DECLARE @TotalMonthlyPayment_ForSI FLOAT = @TotalMonthlyPayment_SI + @UniformBagCost;
			DECLARE @FirstMonthDays TINYINT = 0;

			--This issue is fixed By Numan at 2021-03-05--edited at 2021-03-19-
			----SET @SinceInceptionDropout = @SinceInceptionDropout + @ExpelledVerified + @ExpelledUnverified;
			--SET @SinceInceptionDropout = @SinceInceptionDropout + @ExpelledVerified;
			--This commented by Mubeen at 2021-05-18---
			--SET @SinceInceptionDropout = @SinceInceptionDropout + @ExpelledVerifiedForTheMonth;
			--This commented by Mubeen at 2021-05-18---
			--This issue is fixed By Numan at 2021-03-05--edited at 2021-03-19-

			SELECT
				@FirstMonthDays = DAY(C.StartDate)
			FROM Class AS C
			INNER JOIN ClassStatus AS CS
				ON C.ClassStatusID = CS.ClassStatusID
			WHERE C.ClassCode = @ClassCode
			--IF @FirstMonthDays > 13
			--	BEGIN
			--		IF @InvoiceNumber = 2
			--		BEGIN
			--			SET @TotalMonthlyPayment_SI = (@TotalMonthlyPayment1 / 2);
			--			SET @TotalMonthlyPayment_ForSI = @TotalMonthlyPayment_SI + @UniformBagCost;
			--		END
			--		ELSE
			--		BEGIN
			--			SET @TotalMonthlyPayment_SI = (@TotalMonthlyPayment1 * (@InvoiceNumber - 2)) + (@TotalMonthlyPayment1 / 2);
			--			SET @TotalMonthlyPayment_ForSI = @TotalMonthlyPayment_SI + @UniformBagCost;
			--		END
			--	END

			SET @TotalCostPerTrainee_Reim_SI = @TotalCostPerTrainee * (@InvoiceNumber - 1);

			INSERT INTO dbo.Invoice (SchemeID
			, ClassID
			, TradeID
			, Description
			, GLCode
			, GLName
			, TrainingServicesSaleTax
			, ProgCategory
			, FundingSource
			, TaxCode
			, WTaxLiable
			, InvoiceType
			, TraineePerClass
			, ClaimTrainees
			, Stipend
			, UniformBag
			, BoardingOrLodging
			, TestingFee
			, TotalCostPerTrainee
			, UnverifiedCNICDeductions
			, CnicDeductionType
			, CnicDeductionAmount
			, DeductionTraineeDroput
			, DropOutDeductionType
			, DropOutDeductionAmount
			, DeductionTraineeAttendance
			, AttendanceDeductionAmount
			, MiscDeductionNo
			, MiscDeductionType
			, MiscDeductionAmount
			, FunctionalDate
			, PenaltyPercentage
			, PenaltyAmount
			, ResultDeduction
			, NetPayableAmount
			, NetTrainingCost
			, InActive
			, CreatedUserID
			, ModifiedUserID
			, CreatedDate
			, ModifiedDate
			, StartDate
			, EndDate
			, ActualStartDate
			, ActualEndDate
			, Batch
			, BatchDuration
			, TotalMonthlyPayment
			, GrossPayable
			, DeductionTraineeUnVCnic
			, ClassDays
			, IsApproved
			, month
			, IsPostedToSAP
			, IsRejected
			, ProcessKey
			, InvoiceHeaderID
			, SAPID
			, InvoiceNumber
			, MiscMarginalTraineeCount
			, MiscMarginalTraineeAmount
			, MiscProfileDeductionCount
			, MiscProfileDeductionAmount
			, MiscFailedDeductionCount
			, MiscFailedDeductionAmount
			, TotalLC
			, MiscNonFunctionalAmount
			, LineTotal
			, OcrCode
			, OcrCode2
			, OcrCode3
			, LineStatus
			, OcrCode4
			, BaseEntry
			, BaseType
			, BaseLine
			, InCancel
			, PaymentToBeReleasedTrainees)
				VALUES (@SchemeID                                                                            -- SchemeID - int
				, @ClassID                                                                             -- ClassID - int
				, @TradeID                                                                             -- TradeID - int
				, @Description                                                                         -- Description - nvarchar(250)
				, CASE WHEN @ProgramType = 10 THEN (SELECT GLAccountCode FROM dbo.SAP_GLAccount WHERE GLAccountID = 8) ELSE (SELECT GLAccountCode FROM dbo.SAP_GLAccount WHERE GLAccountID = 1) END                                                                       -- GLCode - nvarchar(250) update according to ProgramType to 
				, CASE WHEN @ProgramType = 10 THEN (SELECT GLName FROM dbo.SAP_GLAccount WHERE GLAccountID = 8) ELSE (SELECT GLName FROM dbo.SAP_GLAccount WHERE GLAccountID = 1) END                                                                             -- GLName - nvarchar(250) update according to ProgramType to 
				, (SELECT GLAccountCode FROM SAP_GLAccount WHERE GLAccountID = 4)                                                                                    -- TrainingServicesSaleTax - nvarchar(250)
				, @PCatID                                                                              -- ProgCategory - int
				, @FSName                                                                              -- FundingSource - nvarchar(250)
				, @TaxCode                                                                             -- TaxCode - nvarchar(250)
				, @WTLiable                                                                            -- WTaxLiable - nvarchar(10)
				, N'Since Inception'                                                                   -- InvoiceType - nvarchar(250)
				, 0                                                                                    -- TraineePerClass - int
				, 0                                                                                    -- ClaimTrainees - int
				, 0                                                                                    -- Stipend - decimal(19, 4)
				, 0                                                                                    -- UniformBag - decimal(19, 4)
				, 0                                                                                    -- BoardingOrLodging - decimal(19, 4)
				, 0                                                                                    -- TestingFee - decimal(19, 4)
				, 0                                                                                    -- TotalCostPerTrainee - decimal(19, 4)
				, @SinceInceptionUnvCNIC                                                               -- UnverifiedCNICDeductions - decimal(19, 4)
				, CASE WHEN @SinceInceptionUnvCNIC > 0 THEN 'UNCNICSI' ELSE '' END                                                                                  -- CnicDeductionType - nvarchar(250)
				, (@SinceInceptionUnvCNIC * @TotalMonthlyPayment_ForSI)                                -- CnicDeductionAmount - decimal(19, 4)
				, @SinceInceptionDropout                                                               -- DeductionTraineeDroput - int
				, CASE WHEN @SinceInceptionDropout > 0 THEN 'DSI' ELSE '' END                                                                                  -- DropOutDeductionType - nvarchar(250)
				, (@SinceInceptionDropout * @TotalMonthlyPayment_ForSI)                                --(@SinceInceptionDropout * @TotalCostPerTrainee)        -- DropOutDeductionAmount - decimal(19, 4)
				, 0                                                                                    -- DeductionTraineeAttendance - decimal(19, 4)
				, 0                                                                                    -- AttendanceDeductionAmount - decimal(19, 4)
				, 0                                                                                    -- MiscDeductionNo - int --@MiscDeductionNo
				, N''                                                                                  -- MiscDeductionType - nvarchar(250)
				, 0                                                                                    -- MiscDeductionAmount - decimal(19, 4)
				, @FunctionalDate                                                                      -- FunctionalDate - datetime
				, 0                                                                                    -- PenaltyPercentage - decimal(19, 4)
				, 0                                                                                    -- PenaltyAmount - decimal(19, 4)
				, 0                                                                                    -- ResultDeduction - decimal(19, 4)
				, -((@SinceInceptionDropout + @SinceInceptionUnvCNIC) * @TotalMonthlyPayment_ForSI)   -- NetPayableAmount - decimal(19, 4)
				, -((@SinceInceptionDropout + @SinceInceptionUnvCNIC) * @TotalCostPerTrainee_Reim_SI)   -- NetTrainingCost - decimal(19, 4)  - ((@SinceInceptionDropout + @SinceInceptionUnvCNIC) * @TotalCostPerTrainee)
				, 0                                                                                    -- InActive - bit
				, 15                                                                                   -- CreatedUserID - int
				, 0                                                                                    -- ModifiedUserID - int
				, GETDATE()                                                                            -- CreatedDate - datetime
				, ''                                                                                   -- ModifiedDate - datetime
				, @InvoiceStartDate                                                                    -- StartDate - date
				, @InvoiceEndDate                                                                      -- EndDate - date
				, @ActualStartDate                                                                     -- ActualStartDate - date
				, @ActualEndDate                                                                       -- ActualEndDate - date
				, @Batch                                                                               -- Batch - int
				, @BatchDuration                                                                       -- BatchDuration - int
				, 0                                                                                    -- TotalMonthlyPayment - decimal(19, 4)
				, 0                                                                                    -- - ((@SinceInceptionDropout + @SinceInceptionUnvCNIC) * @TotalMonthlyPayment) -- GrossPayable - decimal(19, 4)
				, 0                                                                                    -- DeductionTraineeUnVCnic - int -not inserted in prev
				, 0                                                                                    -- ClassDays - int -not inserted in prev
				, 0                                                                                    -- IsApproved - bit
				, @Month                                                                               -- Month - date
				, 0                                                                                    -- IsPostedToSAP - bit -not inserted in prev
				, 0                                                                                    -- IsRejected - bit
				, @ProcessKey                                                                          -- ProcessKey - nvarchar(50)
				, @InvoiceHeaderID                                                                     -- InvoiceHeaderID - int
				, N''                                                                                  -- SAPID - nvarchar(max) -not inserted in prev
				, @InvoiceNumber                                                                       -- InvoiceNumber - int
				, 0                                                                                    -- MiscMarginalTraineeCount - int -not inserted in prev
				, 0                                                                                    -- MiscMarginalTraineeAmount - decimal(19, 4) -not inserted in prev
				, 0                                                                                    -- MiscProfileDeductionCount - int -not inserted in prev
				, 0                                                                                    -- MiscProfileDeductionAmount - decimal(19, 4) -not inserted in prev
				, 0                                                                                    -- MiscFailedDeductionCount - int -not inserted in prev
				, 0                                                                                    -- MiscFailedDeductionAmount - decimal(19, 4) -not inserted in prev
				, -((@SinceInceptionDropout + @SinceInceptionUnvCNIC) * @TotalMonthlyPayment_ForSI)   -- TotalLC - decimal(19, 4)
				, 0                                                                                    -- MiscNonFunctionalAmount - decimal(19, 4) -not inserted in prev
				, 0                                                                                    --  -- LineTotal - decimal(19, 4)
				, @OcrCode                                                                             -- OcrCode - nvarchar(100)
				, @OcrCode2                                                                            -- OcrCode2 - nvarchar(100)
				, @OcrCode3                                                                            -- OcrCode3 - nvarchar(100)
				, @lineStatus                                                                          -- LineStatus - nvarchar(50)
				, @OcrCode4                                                                            -- OcrCode4 - nvarchar(100)  -not inserted in prev
				, ''                                                                                   -- BaseEntry - nvarchar(250)
				, N'-1'                                                                                -- BaseType - nvarchar(250)
				, @POLineNum                                                                           -- BaseLine - nvarchar(250)
				, 0                                                                                    -- InCancel - bit -not inserted in prev
				, @PaymentToBeReleased);
		END;

		IF (@ReimUnvTraineees > 0
			OR @ReimAttendance > 0)
		BEGIN
			DECLARE @TotalMonthlyPayment_Reim FLOAT = @TotalMonthlyPayment * (@InvoiceNumber - 1);
			--IF (@InvoiceNumber = 2)
			--BEGIN
			SELECT
				@PreviousInvoiceDays = cim.InvoiceDays
			FROM dbo.Class AS c
			INNER JOIN dbo.PRN AS prn
				ON prn.ClassCode = c.ClassCode
			INNER JOIN dbo.ClassInvoiceMap AS cim
				ON cim.ClassID = c.ClassID
			WHERE c.ClassCode = @ClassCode
			AND cim.InvoiceNo = (@InvoiceNumber - 1)
			AND cim.InvoiceType = 'Regular';
			IF (@PreviousInvoiceDays = 30)
			BEGIN
				SET @TotalMonthlyPaymentGross = @TotalMonthlyPaymentGross1
				SET @TotalMonthlyPayment = @TotalMonthlyPayment1
				SET @TotalCostPerTrainee = @TotalCostPerTrainee1
				SET @UnVCNICDeductionAmountForTheMonth = @UnVCNICDeductionCountForTheMonth * @TotalMonthlyPayment;
				SET @AttendanceDeductionAmount = @DeductionTraineeAttendance * @TotalCostPerTrainee;
				SET @TotalMonthlyPayment_Reim = @TotalMonthlyPayment * (@InvoiceNumber - 1);
			END
			ELSE
			BEGIN

				SET @UnVCNICDeductionAmountForTheMonth = @UnVCNICDeductionCountForTheMonth * @TotalMonthlyPayment;
				SET @AttendanceDeductionAmount = @DeductionTraineeAttendance * @TotalCostPerTrainee;

				IF (@ClassDays = 30)
				BEGIN
					SET @TotalMonthlyPayment = @TotalMonthlyPayment / 2
					SET @TotalCostPerTrainee = @TotalCostPerTrainee / 2
					SET @TotalMonthlyPaymentGross = @TotalMonthlyPaymentGross / 2
					SET @TotalMonthlyPayment_Reim = (@TotalMonthlyPayment) * (@InvoiceNumber - 1);
				END
				ELSE
					SET @TotalMonthlyPayment_Reim = @TotalMonthlyPayment * (@InvoiceNumber - 1);

			END

			SET @NetPayableAmount
			= (@ClaimedTrainees * @TotalMonthlyPayment)
			- (@UnVCNICDeductionAmountForTheMonth + @DeductDropoutForTheMonthAmount + @AttendanceDeductionAmount
			+ @MiscDeductionAmount + @PenaltyAmount + @ResultDeduction
			);
			--END


			DECLARE @TotalMonthlyPayment_ForReim FLOAT = (SELECT
					SUM(I.TotalCostPerTrainee)
				FROM Invoice AS I
				WHERE I.ClassID = @ClassID
				AND I.InvoiceHeaderID <> @InvoiceHeaderID)
			+ @UniformBagCost;

			SET @TotalCostPerTrainee_Reim_SI = @TotalCostPerTrainee * (@InvoiceNumber - 1);

			INSERT INTO dbo.Invoice (SchemeID
			, ClassID
			, TradeID
			, Description
			, GLCode
			, GLName
			, TrainingServicesSaleTax
			, ProgCategory
			, FundingSource
			, TaxCode
			, WTaxLiable
			, InvoiceType
			, TraineePerClass
			, ClaimTrainees
			, Stipend
			, UniformBag
			, BoardingOrLodging
			, TestingFee
			, TotalCostPerTrainee
			, UnverifiedCNICDeductions
			, CnicDeductionType
			, CnicDeductionAmount
			, DeductionTraineeDroput
			, DropOutDeductionType
			, DropOutDeductionAmount
			, DeductionTraineeAttendance
			, AttendanceDeductionAmount
			, MiscDeductionNo
			, MiscDeductionType
			, MiscDeductionAmount
			, FunctionalDate
			, PenaltyPercentage
			, PenaltyAmount
			, ResultDeduction
			, NetPayableAmount
			, NetTrainingCost
			, InActive
			, CreatedUserID
			, ModifiedUserID
			, CreatedDate
			, ModifiedDate
			, StartDate
			, EndDate
			, ActualStartDate
			, ActualEndDate
			, Batch
			, BatchDuration
			, TotalMonthlyPayment
			, GrossPayable
			, DeductionTraineeUnVCnic
			, ClassDays
			, IsApproved
			, month
			, IsPostedToSAP
			, IsRejected
			, ProcessKey
			, InvoiceHeaderID
			, SAPID
			, InvoiceNumber
			, MiscMarginalTraineeCount
			, MiscMarginalTraineeAmount
			, MiscProfileDeductionCount
			, MiscProfileDeductionAmount
			, MiscFailedDeductionCount
			, MiscFailedDeductionAmount
			, TotalLC
			, MiscNonFunctionalAmount
			, LineTotal
			, OcrCode
			, OcrCode2
			, OcrCode3
			, LineStatus
			, OcrCode4
			, BaseEntry
			, BaseType
			, BaseLine
			, InCancel
			, PaymentToBeReleasedTrainees)
				VALUES (@SchemeID                                                                                     -- SchemeID - int
				, @ClassID                                                                                      -- ClassID - int
				, @TradeID                                                                                      -- TradeID - int
				, @Description                                                                                  -- Description - nvarchar(250)
				, @GLAccountCode                                                                                -- GLCode - nvarchar(250)
				, @GLName                                                                                       -- GLName - nvarchar(250)
				, (SELECT GLAccountCode FROM SAP_GLAccount WHERE GLAccountID = 4)                                                                                             -- TrainingServicesSaleTax - nvarchar(250)
				, @PCatID                                                                                       -- ProgCategory - int
				, @FSName                                                                                       -- FundingSource - nvarchar(250)
				, @TaxCode                                                                                      -- TaxCode - nvarchar(250)
				, @WTLiable                                                                                     -- WTaxLiable - nvarchar(10)
				, N'Reimbursement'                                                                              -- InvoiceType - nvarchar(250)
				, 0                                                                                             -- TraineePerClass - int
				, 0                                                                                             -- ClaimTrainees - int
				, 0                                                                                             -- Stipend - decimal(19, 4)
				, 0                                                                                             -- UniformBag - decimal(19, 4)
				, 0                                                                                             -- BoardingOrLodging - decimal(19, 4)
				, 0                                                                                             -- TestingFee - decimal(19, 4)
				, 0                                                                                             -- TotalCostPerTrainee - decimal(19, 4)
				, -@ReimUnvTraineees                                                                            -- UnverifiedCNICDeductions - decimal(19, 4)
				, CASE WHEN @ReimUnvTraineees > 0 THEN 'UNCNICSI' ELSE '' END                                                                                           -- CnicDeductionType - nvarchar(250)
				, -(@ReimUnvTraineees * @TotalMonthlyPayment_ForReim)                                          -- CnicDeductionAmount - decimal(19, 4)
				, 0                                                                                             -- DeductionTraineeDroput - int
				, ''                                                                                            -- DropOutDeductionType - nvarchar(250)
				, 0                                                                                             -- DropOutDeductionAmount - decimal(19, 4)
				, -@ReimAttendance                                                                             -- DeductionTraineeAttendance - decimal(19, 4)
				, -(@ReimAttendance * @TotalCostPerTrainee)                                                    -- AttendanceDeductionAmount - decimal(19, 4)
				, 0                                                                                             -- MiscDeductionNo - int
				, N''                                                                                           -- MiscDeductionType - nvarchar(250)
				, 0                                                                                             -- MiscDeductionAmount - decimal(19, 4)
				, @FunctionalDate                                                                               -- FunctionalDate - datetime
				, 0                                                                                             -- PenaltyPercentage - decimal(19, 4)
				, 0                                                                                             -- PenaltyAmount - decimal(19, 4)
				, 0                                                                                             -- ResultDeduction - decimal(19, 4)
				, (@ReimUnvTraineees * @TotalMonthlyPayment_ForReim) + (@ReimAttendance * @TotalCostPerTrainee) -- NetPayableAmount - decimal(19, 4)  (@ReimUnvTraineees * @TotalMonthlyPayment) + (@ReimAttendance * @TotalCostPerTrainee)
				, ((@ReimUnvTraineees * @TotalMonthlyPayment_ForReim) + (@ReimAttendance * @TotalCostPerTrainee) - (@ReimUnvTraineees * @UnfirmBag_Remb_SI))                                                                                             -- NetTrainingCost - decimal(19, 4) ((@ReimUnvTraineees * @TotalMonthlyPayment) + (@ReimAttendance * @TotalCostPerTrainee) - (@ReimUnvTraineees * @UnfirmBag_Remb_SI) )      
				, 0                                                                                             -- InActive - bit
				, 15                                                                                            -- CreatedUserID - int
				, 0                                                                                             -- ModifiedUserID - int
				, GETDATE()                                                                                     -- CreatedDate - datetime
				, ''                                                                                            -- ModifiedDate - datetime
				, @InvoiceStartDate                                                                             -- StartDate - date
				, @InvoiceEndDate                                                                               -- EndDate - date
				, @ActualStartDate                                                                              -- ActualStartDate - date
				, @ActualEndDate                                                                                -- ActualEndDate - date
				, @Batch                                                                                        -- Batch - int
				, @BatchDuration                                                                                -- BatchDuration - int
				, 0                                                                                             -- TotalMonthlyPayment - decimal(19, 4)
				, 0                                                                                             -- (@ReimUnvTraineees * @TotalMonthlyPayment) + (@ReimAttendance * @TotalCostPerTrainee) -- GrossPayable - decimal(19, 4)
				, 0                                                                                             -- DeductionTraineeUnVCnic - int -not inserted in prev
				, 0                                                                                             -- ClassDays - int 
				, 0                                                                                             -- IsApproved - bit
				, @Month                                                                                        -- Month - date
				, 0                                                                                             -- IsPostedToSAP - bit -not inserted in prev
				, 0                                                                                             -- IsRejected - bit
				, @ProcessKey                                                                                   -- ProcessKey - nvarchar(50)
				, @InvoiceHeaderID                                                                              -- InvoiceHeaderID - int
				, N''                                                                                           -- SAPID - nvarchar(max) -not inserted in prev
				, @InvoiceNumber                                                                                -- InvoiceNumber - int
				, 0                                                                                             -- MiscMarginalTraineeCount - int -not inserted in prev
				, 0                                                                                             -- MiscMarginalTraineeAmount - decimal(19, 4) -not inserted in prev
				, 0                                                                                             -- MiscProfileDeductionCount - int -not inserted in prev
				, 0                                                                                             -- MiscProfileDeductionAmount - decimal(19, 4) -not inserted in prev
				, 0                                                                                             -- MiscFailedDeductionCount - int -not inserted in prev
				, 0                                                                                             -- MiscFailedDeductionAmount - decimal(19, 4) -not inserted in prev
				, (@ReimUnvTraineees * @TotalMonthlyPayment_ForReim) + (@ReimAttendance * @TotalCostPerTrainee) -- TotalLC - decimal(19, 4) (@ReimUnvTraineees * @TotalMonthlyPayment) + (@ReimAttendance * @TotalCostPerTrainee)
				, 0                                                                                             -- MiscNonFunctionalAmount - decimal(19, 4) -not inserted in prev
				, 0                                                                                             --(@ReimUnvTraineees * @TotalMonthlyPayment) + (@ReimAttendance * @TotalCostPerTrainee) -- LineTotal - decimal(19, 4)
				, @OcrCode                                                                                      -- OcrCode - nvarchar(100)
				, @OcrCode2                                                                                     -- OcrCode2 - nvarchar(100)
				, @OcrCode3                                                                                     -- OcrCode3 - nvarchar(100)
				, @lineStatus                                                                                   -- LineStatus - nvarchar(50)
				, @OcrCode4                                                                                     -- OcrCode4 - nvarchar(100)  -not inserted in prev
				, ''                                                                                            -- BaseEntry - nvarchar(250)
				, N'-1'                                                                                         -- BaseType - nvarchar(250)
				, @InvoiceID                                                                                    -- BaseLine - nvarchar(250) -- @POLineNum
				, 0                                                                                             -- InCancel - bit -not inserted in prev
				, @PaymentToBeReleased);

		END;
	END;

	UPDATE ClassInvoiceMap
	SET IsGenerated = 1
	WHERE ClassID = @ClassID
	AND InvoiceNo = @InvoiceNumber;

END;
-------------------------------------------------
GO
ALTER PROCEDURE [dbo].[GenerateINVCompletion] @PRNMasterID INT
, @ProcessKey NVARCHAR(50)
AS
BEGIN
	--DECLARE @ProcessKey NVARCHAR(50) = N'INV_C'

	--BEGIN TRANSACTION
	--BEGIN TRY
	DECLARE @TSPID INT;
	DECLARE @TSPCode NVARCHAR(200);
	DECLARE @TSPName NVARCHAR(200);
	DECLARE @CtlAccount NVARCHAR(50);
	DECLARE @U_SCHEME NVARCHAR(50);
	DECLARE @U_SCH_Code NVARCHAR(50);
	DECLARE @ClassCode NVARCHAR(100);
	DECLARE @PaymentToBeReleasedTrainees FLOAT;
	--DECLARE @ClaimedTraineesCount         INT
	DECLARE @GrossPayableAmount FLOAT;
	DECLARE @DeductionFailedTrainees FLOAT;
	DECLARE @TaxCode NVARCHAR(250);
	DECLARE @WTLiable NVARCHAR(10);
	DECLARE @ClaimedTrainees INT;
	DECLARE @EnrolledTrainees INT;
	DECLARE @ExtraTraineeDeductCompletion INT;
	DECLARE @AbsentDeductCompletion INT;
	DECLARE @UnVDeductCompletion INT;
	DECLARE @CNICUnverified INT;
	DECLARE @DropoutDeductCompletion INT;
	DECLARE @TotalCostPerTrainee DECIMAL(18, 2);
	DECLARE @TotalMonthlyPayment DECIMAL(18, 2);
	DECLARE @OcrCode NVARCHAR(100) = N'';
	DECLARE @OcrCode2 NVARCHAR(100) = N'';
	DECLARE @OcrCode3 NVARCHAR(100) = N'';
	DECLARE @OcrCode4 NVARCHAR(100) = N'';
	DECLARE @lineStatus NVARCHAR(50) = N'';
	DECLARE @GLAccountCode NVARCHAR(50) = N'';
	DECLARE @GLName NVARCHAR(50) = N'';
	DECLARE @PODocEntry INT = 0;
	DECLARE @PODocNum INT = 0;
	DECLARE @POLineNum INT = 0;
	DECLARE @CNICDeductionType NVARCHAR(50) = N'';
	DECLARE @CNICDeductionAmount FLOAT = 0;
	DECLARE @InvoiceType NVARCHAR(250) = N'2nd Last';
	DECLARE @ClassDays INT = 30;
	DECLARE @ProgramType INT; ---Include Program Type for Specific GLAccount 
	--SELECT @TSPID = pm.TSPID
	--FROM dbo.PRNMaster AS pm
	--WHERE pm.PRNMasterID = @PRNMasterID

	DECLARE @CardCode NVARCHAR(100);
	DECLARE @SchemeCode NVARCHAR(150);
	DECLARE @PaymentStructure NVARCHAR(150);
	DECLARE @Month DATE;
	DECLARE @sapBPLId INT;
	DECLARE @sapSchemeCode NVARCHAR(150);
	DECLARE @sapOcrCode NVARCHAR(150);
	DECLARE @POHeaderID INT;
	DECLARE @SchemeNameBSS NVARCHAR(250);
	DECLARE @SchemeCodeBSS NVARCHAR(10);
	DECLARE @ReimUnvTraineees INT = 0;
	DECLARE @ReimAttendance INT = 0;

	DECLARE @SinceInceptionDropout INT = 0;
	DECLARE @SinceInceptionUnvCNIC INT = 0;
	DECLARE @FunctionalDate DATE;
	DECLARE @InvoiceNumber INT;
	DECLARE @DeductDropoutForTheMonthCount INT = 0;
	DECLARE @PenaltyPercentage FLOAT = 0;
	DECLARE @DeductionTraineeAttendance FLOAT = 0;
	DECLARE @AttendanceDeductionAmount FLOAT = 0;
	DECLARE @DeductionUniformBagReceiving INT;

	SELECT
		@TSPID = t.TspID
	   ,@SchemeNameBSS = s.SchemeName
	   ,@SchemeCodeBSS = s.SchemeCode
		--, @SchemeCode       = s.SAPID
	   ,@U_SCH_Code = s.SAPID
	   ,@PaymentStructure = s.PaymentSchedule
	   ,@CardCode = tm.SAPID
	   ,@TSPName = tm.TSPName
	   ,@Month = pm.[month]
	   ,@POHeaderID = poh.POHeaderID
	   ,@PODocEntry = poh.DocEntry
	   ,@PODocNum = poh.DocNum
	   ,@sapBPLId = sb.BranchID
	   ,@U_SCHEME = sap.SchemeCode
	   ,@sapSchemeCode = sap.SchemeCode
	   ,@ProgramType = s.ProgramTypeID
	FROM dbo.TSPDetail t
	INNER JOIN dbo.TSPMaster tm
		ON tm.TSPMasterID = t.TSPMasterID
	INNER JOIN dbo.PRNMaster pm
		ON pm.TspID = t.TspID
	INNER JOIN dbo.Scheme s
		ON s.SchemeID = t.SchemeID
	INNER JOIN dbo.POHeader poh
		ON poh.TspID = t.TspID
	INNER JOIN dbo.SAP_Scheme sap
		ON sap.PaymentStructure = s.PaymentSchedule
	INNER JOIN dbo.SAP_BPL AS sb
		ON sb.FundingCategoryID = s.FundingCategoryID
	WHERE pm.PRNMasterID = @PRNMasterID;


	--Updated by Ali Haider on 22-Apl-2024
	-----Get @GLAccountCode according to program type if SSP than update Scholorship Cost upon Training Cost
	IF (@ProgramType = 10)
	BEGIN
		SELECT
			@GLAccountCode = GLAccountCode
		   ,@GLName = GLName
		FROM dbo.SAP_GLAccount
		WHERE GLAccountID = 7;
	END
	ELSE
	BEGIN
		SELECT
			@GLAccountCode = GLAccountCode
		   ,@GLName = GLName
		FROM dbo.SAP_GLAccount
		WHERE GLAccountID = 1;
	END

	-------------End updation------------

	SELECT
		@CtlAccount = CtlAccount
	   ,@lineStatus = LineStatus
	   ,@TaxCode = TaxCode
	   ,@WTLiable = WTLiable
	FROM dbo.SAP_StaticField
	WHERE SAP_StaticFileldID = 1;


	--SELECT @TSPCode    = td.TSPCode
	--     , @TSPName    = td.TSPName
	--     , @U_SCHEME   = ss.SchemeCode
	--     , @sapBPLId   = ss.SAP_SchemeID
	--     , @U_SCH_Code = s.SAPID
	--FROM dbo.TSPDetail            AS td
	--    INNER JOIN dbo.Scheme     AS s ON s.SchemeID = td.SchemeID
	--    INNER JOIN dbo.SAP_Scheme AS ss ON ss.PaymentStructure = s.PaymentSchedule
	--WHERE td.TSPID = @TSPID;

	INSERT INTO dbo.InvoiceHeader (DocNum
	, DocType
	, Printed
	, DocDate
	, DocDueDate
	, CardCode
	, CardName
	, DocTotal
	, Comments
	, JournalMemo
	, BPL_IDAssignedToInvoice
	, CtlAccount
	, U_SCHEME
	, U_Sch_Code
	, U_Month
	, InActive
	, CreatedUserID
	, ModifiedUserID
	, CreatedDate
	, ModifiedDate
	, TspID
	, ProcessKey
	, IsApproved
	, IsRejected
	, POHeaderID)
		VALUES (0            -- DocNum - int
		, N'S'         -- DocType - nvarchar(10)
		, N'N'         -- Printed - nvarchar(10)
		, GETDATE()    -- DocDate - date
		, GETDATE()    -- DocDueDate - date
		, @CardCode    -- CardCode - nvarchar(200)
		, @TSPName     -- CardName - nvarchar(200)
		, 0            -- DocTotal - decimal(18, 0)
		, @InvoiceType -- Comments - nvarchar(max)
		, @InvoiceType -- JournalMemo - nvarchar(200)
		, @sapBPLId    -- BPL_IDAssignedToInvoice - int
		, @CtlAccount  -- CtlAccount - nvarchar(50)
		, @U_SCHEME    -- U_SCHEME - nvarchar(50)
		, @U_SCH_Code  -- U_SCH_Code - nvarchar(50)
		, @Month       -- U_Month - date
		, 0            -- InActive - bit
		, 15           -- CreatedUserID - int
		, 0            -- ModifiedUserID - int
		, GETDATE()    -- CreatedDate  - datetime
		, NULL         -- ModifiedDate - datetime
		, @TSPID       -- TSPID - int
		, @ProcessKey  -- ProcessKey - nvarchar(50)
		, 0            -- IsApproved - bit
		, 0            -- IsRejected - bit
		, @POHeaderID  -- POHeaderID - int
		);

	DECLARE @InvoiceHeaderID INT = SCOPE_IDENTITY();
	--send to 1st approval
	INSERT INTO dbo.ApprovalHistory (Step
	, FormID
	, ApproverID
	, Comments
	, ApprovalStatusID
	, CreatedUserID
	, ModifiedUserID
	, CreatedDate
	, ModifiedDate
	, InActive
	, ProcessKey)
		VALUES (1                -- Step - int
		, @InvoiceHeaderID -- FormID - int
		, 0                -- ApproverID - int
		, N'Pending'       -- Comments - nvarchar(4000)
		, 1                -- ApprovalStatusID - int
		, 15               -- CreatedUserID - int
		, 0                -- ModifiedUserID - int
		, GETDATE()        -- CreatedDate - datetime
		, NULL             -- ModifiedDate - datetime
		, 0                -- InActive - bit
		, @ProcessKey      -- ProcessKey - nvarchar(100)
		);

	DECLARE _cursor CURSOR LOCAL FOR (SELECT
			p.ClassCode
		   ,p.PaymentToBeReleasedTrainees
		   ,p.DeductionFailedTrainees
		   ,p.ClaimedTrainees
		   ,p.EnrolledTrainees
		   ,p.ExtraTraineeDeductCompletion
		   ,p.UnVDeductCompletion
		   ,p.DropOutDeductCompletion
		   ,p.AbsentDeductCompletion
		   ,p.CNICUnverified
		   ,p.ReimbursementAttandance
			--, p.ReimbursementUnVTrainees
		   ,p.ClassStartDate
			--, (p.DropoutsVerified + p.DropoutsUnverified + p.ExpelledVerified + p.ExpelledUnverified) DeductDropoutForTheMonthCount
		   ,p.PenaltyTPMReports + p.PenaltyImposedByME PenaltyPercentage
		   ,p.PaymentWithheldPhysicalCount
		   ,p.DeductionUniformBagReceiving
		FROM dbo.PRN AS p
		WHERE p.PRNMasterID = @PRNMasterID);
	OPEN _cursor;
	FETCH NEXT FROM _cursor
	INTO @ClassCode
	, @PaymentToBeReleasedTrainees
	, @DeductionFailedTrainees
	, @ClaimedTrainees
	, @EnrolledTrainees
	, @ExtraTraineeDeductCompletion
	, @UnVDeductCompletion
	, @DropoutDeductCompletion
	, @AbsentDeductCompletion
	, @CNICUnverified
	, @ReimAttendance
	--2, @ReimUnvTraineees -- need clarity
	, @FunctionalDate
	--, @DeductDropoutForTheMonthCount
	, @PenaltyPercentage
	, @DeductionTraineeAttendance
	, @DeductionUniformBagReceiving;
	WHILE @@fetch_status = 0
	BEGIN
	DECLARE @SchemeID INT;
	DECLARE @ClassID INT;
	DECLARE @TradeID INT;
	DECLARE @TradeName NVARCHAR(250);
	--DECLARE, @Description             NVARCHAR(250)
	DECLARE @GLCode NVARCHAR(250);
	--DECLARE, @GLName                  NVARCHAR(250)
	DECLARE @TrainingServicesSaleTax NVARCHAR(250);
	DECLARE @PCategory INT;
	DECLARE @FundingSource NVARCHAR(250);
	DECLARE @TraineePerClass INT;
	--DECLARE , @Stipend                 DECIMAL(18, 0)
	DECLARE @UniformBagCost DECIMAL(18, 0);
	DECLARE @StartDate DATE;
	DECLARE @EndDate DATE;
	DECLARE @ActualStartDate DATE;
	DECLARE @ActualEndDate DATE;
	DECLARE @Batch INT;
	DECLARE @BatchDuration INT;
	DECLARE @ResultDeduction DECIMAL(18, 6);
	DECLARE @NetPayableAmount DECIMAL(18, 6);
	DECLARE @NetTrainingCost DECIMAL(18, 6);
	DECLARE @TraineesPerClass INT;
	DECLARE @dropcat NVARCHAR(100) = N'';
	DECLARE @DeductDropoutForTheMonthAmount FLOAT = 0;
	DECLARE @PenaltyAmount FLOAT;
	DECLARE @MiscDeductionNo INT = 0;
	DECLARE @MiscDeductionType NVARCHAR(100) = N'';
	DECLARE @MiscDeductionAmount FLOAT = 0;


	SELECT
		@SchemeID = s.SchemeID
	   ,@ClassID = c.ClassID
	   ,@TradeID = c.TradeID
	   ,@TradeName = t.TradeName
	   ,@PCategory = s.PCategoryID
	   ,@FundingSource = fs.FundingSourceName
	   ,@StartDate = c.StartDate
	   ,@EndDate = c.EndDate
	   ,@TraineePerClass = c.TraineesPerClass
		-- , @Stipend = c.Stipend
	   ,@UniformBagCost = c.UniformBagCost
	   ,@Batch = c.Batch
	   ,@BatchDuration = c.Duration
	   ,@ActualStartDate = ir.ActualStartDate
	   ,@ActualEndDate = ir.ActualEndDate
	   ,@TraineesPerClass = c.TraineesPerClass
	FROM dbo.Class AS c
	INNER JOIN dbo.InceptionReport AS ir
		ON ir.ClassID = c.ClassID
	INNER JOIN dbo.TSPDetail AS td
		ON td.TspID = c.TspID
	INNER JOIN dbo.Scheme AS s
		ON s.SchemeID = td.SchemeID
	INNER JOIN dbo.Trade AS t
		ON t.TradeID = c.TradeID
	INNER JOIN dbo.FundingSource AS fs
		ON fs.FundingSourceID = s.FundingSourceID
	WHERE c.ClassCode = @ClassCode;

	IF @EnrolledTrainees < @ClaimedTrainees
	BEGIN
		SET @ClaimedTrainees = @EnrolledTrainees;
	END;

	DECLARE @ExtraCountFromClass INT = 0;
	SET @ExtraCountFromClass = @EnrolledTrainees - @TraineesPerClass;

	--IF @ExtraCountFromClass > 0
	--BEGIN
	--    SET @DeductDropoutForTheMonthCount = @DeductDropoutForTheMonthCount - @ExtraCountFromClass;
	--END;
	--IF (@DeductDropoutForTheMonthCount < 0)
	--BEGIN
	--    SET @DeductDropoutForTheMonthCount = 0;
	--END;
	SET @DeductDropoutForTheMonthCount = @ClaimedTrainees - @CNICUnverified - @PaymentToBeReleasedTrainees

	SELECT
		@ResultDeduction
		= (c.PerTraineeTestCertCost
		* (@ExtraTraineeDeductCompletion + @UnVDeductCompletion + @DropoutDeductCompletion
		+ @AbsentDeductCompletion
		)
		)
	   ,@NetPayableAmount = cim.Amount * @PaymentToBeReleasedTrainees
	   ,@TotalCostPerTrainee = cim.Amount
	   ,@TotalMonthlyPayment = cim.Amount
	   ,@NetTrainingCost = cim.Amount * @PaymentToBeReleasedTrainees
	   ,@GrossPayableAmount = cim.Amount * @ClaimedTrainees
	   ,@CNICDeductionAmount = cim.Amount * @CNICUnverified
	   ,@ClassDays = cim.InvoiceDays
	   ,@InvoiceNumber = cim.InvoiceNo
	FROM dbo.ClassInvoiceMap AS cim
	INNER JOIN dbo.Class c
		ON c.ClassID = cim.ClassID
	WHERE cim.ClassID = @ClassID
	AND cim.InvoiceType = @InvoiceType;

	--SELECT
	--    TOP (1)
	--    @TotalMonthlyPayment = cim.Amount
	--FROM dbo.ClassInvoiceMap AS cim
	--    INNER JOIN dbo.Class c ON c.ClassID = cim.ClassID
	--WHERE cim.ClassID = @ClassID
	--      AND cim.InvoiceType = 'Regular'
	--ORDER BY cim.InvoiceNo;

	IF @DeductDropoutForTheMonthCount > 0
	BEGIN
		SET @DeductDropoutForTheMonthAmount = @DeductDropoutForTheMonthCount * @TotalMonthlyPayment; --need clarity of @TotalMonthlyPayment, 2ndlast amount or reguar amount
		SET @dropcat = N'DFM';
	END;
	SET @PenaltyAmount = (@PenaltyPercentage / 100) * (@TotalCostPerTrainee * @ClaimedTrainees);
	IF (@PenaltyPercentage > 100)
	BEGIN
		SET @PenaltyAmount = (100 / 100) * (@TotalCostPerTrainee * @ClaimedTrainees);
	END;
	IF (@CNICDeductionAmount > 0)
	BEGIN
		SET @CNICDeductionType = N'UNCNICFTM';
	END;

	SET @AttendanceDeductionAmount = @DeductionTraineeAttendance * @TotalCostPerTrainee;

	DECLARE @deptCode INT = (SELECT
			s.PCategoryID
		FROM dbo.Class c
		INNER JOIN Scheme s
			ON s.SchemeID = c.SchemeID
		WHERE c.ClassID = @ClassID);

	DECLARE @tradeidsap NVARCHAR(100) = (SELECT
			t.SAPID
		FROM dbo.Trade t
		INNER JOIN dbo.Class c
			ON c.TradeID = t.TradeID
		WHERE c.ClassID = @ClassID);

	SET @OcrCode2 = @tradeidsap;
	IF (@deptCode = 1) --6 dev db, Business Development
	BEGIN
		SET @deptCode = N'01';
		SET @OcrCode3 = N'BD011';
	--SET @ocrCode2 = @tradeidsap+'-01';
	END;
	ELSE
	IF (@deptCode = 2) --7 dev db, Program Development
	BEGIN
		SET @deptCode = N'00';
		SET @OcrCode3 = N'PD002';
	--SET @ocrCode2 = @tradeidsap+'-00';
	END;

	SELECT
		@OcrCode = U_Sch_Code
	   ,@OcrCode4 = BPL_IDAssignedToInvoice
	FROM dbo.InvoiceHeader
	WHERE InvoiceHeaderID = @InvoiceHeaderID;


	--Updated by Ali Haider on 22-Apl-2024
	-----Get @GLAccountCode according to program type if SSP than update Scholorship Cost upon Training Cost
	IF (@ProgramType = 10)
	BEGIN
		SELECT
			@GLCode = sga.GLAccountCode
		   ,@GLName = sga.GLName
		FROM dbo.SAP_GLAccount AS sga
		WHERE sga.GLAccountID = 7;
	END
	ELSE
	BEGIN
		SELECT
			@GLCode = sga.GLAccountCode
		   ,@GLName = sga.GLName
		FROM dbo.SAP_GLAccount AS sga
		WHERE sga.GLAccountID = 1;
	END


	SELECT
		@TrainingServicesSaleTax = sga.GLAccountCode
	FROM dbo.SAP_GLAccount AS sga
	WHERE sga.GLAccountID = 4;

	SELECT
	TOP (1)
		@POLineNum = pl.LineNum
	FROM dbo.POLines pl
	INNER JOIN dbo.PRN p
		ON p.ClassCode = pl.U_Class_Code
	INNER JOIN dbo.PRNMaster pm
		ON pm.PRNMasterID = p.PRNMasterID
	INNER JOIN dbo.POHeader PH
		ON PH.POHeaderID = pl.POHeaderID
	WHERE p.ClassID = @ClassID
	AND ISNULL(pl.InActive, 0) = 0
	AND pm.PRNMasterID = @PRNMasterID
	AND PH.ProcessKey = 'PO_TSP';

	SET @MiscDeductionNo = @DeductionFailedTrainees-- + @DeductionUniformBagReceiving
	IF (@DeductionFailedTrainees > 0)
	BEGIN
		SET @MiscDeductionType = 'Failed Deduction'
		--10 % of amount
		--SET @MiscDeductionAmount = (0.1* ( select sum(cim.Amount) from dbo.ClassInvoiceMap cim where cim.ClassID = @ClassID) * @DeductionFailedTrainees)
		SET @MiscDeductionAmount = ((SELECT
				cim.Amount
			FROM dbo.ClassInvoiceMap cim
			WHERE cim.ClassID = @ClassID
			AND cim.InvoiceType = '2nd Last')
		* @DeductionFailedTrainees)
	END;

	INSERT INTO dbo.Invoice (SchemeID
	, ClassID
	, TradeID
	, Description
	, GLCode
	, GLName
	, TrainingServicesSaleTax
	, ProgCategory
	, FundingSource
	, TaxCode
	, WTaxLiable
	, InvoiceType
	, TraineePerClass
	, ClaimTrainees
	, Stipend
	, UniformBag
	, BoardingOrLodging
	, TestingFee
	, TotalCostPerTrainee
	, UnverifiedCNICDeductions
	, CnicDeductionType
	, CnicDeductionAmount
	, DeductionTraineeDroput
	, DropOutDeductionType
	, DropOutDeductionAmount
	, DeductionTraineeAttendance
	, AttendanceDeductionAmount
	, MiscDeductionNo
	, MiscDeductionType
	, MiscDeductionAmount
	, FunctionalDate
	, PenaltyPercentage
	, PenaltyAmount
	, ResultDeduction
	, NetPayableAmount
	, NetTrainingCost
	, InActive
	, CreatedUserID
	, ModifiedUserID
	, CreatedDate
	, ModifiedDate
	, StartDate
	, EndDate
	, ActualStartDate
	, ActualEndDate
	, Batch
	, BatchDuration
	, TotalMonthlyPayment
	, GrossPayable
	, DeductionTraineeUnVCnic
	, ClassDays
	, IsApproved
	, month
	, IsPostedToSAP
	, IsRejected
	, ProcessKey
	, InvoiceHeaderID
	, SAPID
	, InvoiceNumber
	--, MiscMarginalTraineeCount
	--, MiscMarginalTraineeAmount
	--, MiscProfileDeductionCount
	--, MiscProfileDeductionAmount
	--, MiscFailedDeductionCount
	--, MiscFailedDeductionAmount
	, TotalLC
	--, MiscNonFunctionalAmount
	, LineTotal
	, OcrCode
	, OcrCode2
	, OcrCode3
	, LineStatus
	, OcrCode4
	, BaseEntry
	, BaseType
	, BaseLine
	, PaymentToBeReleasedTrainees
	--, InCancel
	)
		VALUES (@SchemeID                                            -- SchemeID - int
		, @ClassID                                             -- ClassID - int
		, @TradeID                                             -- TradeID - int
		, CONCAT(@ClassCode, ' - ', @Batch, ' - ', @TradeName) -- Description - nvarchar(250)
		, @GLCode                                              -- GLCode - nvarchar(250)
		, @GLName                                              -- GLName - nvarchar(250)
		, @TrainingServicesSaleTax                             -- TrainingServicesSaleTax - nvarchar(250)
		, @PCategory                                           -- ProgCategory - int
		, @FundingSource                                       -- FundingSource - nvarchar(250)
		, @TaxCode                                             -- TaxCode - nvarchar(250)
		, @WTLiable                                            -- WTaxLiable - nvarchar(10)
		, '2Last'                                         -- InvoiceType - nvarchar(250)
		, @TraineePerClass                                     -- TraineePerClass - int
		, @ClaimedTrainees                                     -- ClaimTrainees - int
		, 0                                                    -- Stipend - decimal(18, 0)
		, 0                                                    -- UniformBag - decimal(18, 0)
		, 0                                                    -- BoardingOrLodging - decimal(18, 0)
		, 0                                                    -- TestingFee - decimal(18, 0)
		, @TotalCostPerTrainee                                 -- TotalCostPerTrainee - decimal(18, 0)
		, @CNICUnverified                                      -- UnverifiedCNICDeductions - decimal(18, 0)
		, @CNICDeductionType                                   -- CnicDeductionType - nvarchar(250)
		, @CNICDeductionAmount                                 -- CnicDeductionAmount - decimal(18, 0)
		, @DeductDropoutForTheMonthCount                       -- DeductionTraineeDroput - int
		, @dropcat                                             -- DropOutDeductionType - nvarchar(250)
		, @DeductDropoutForTheMonthAmount                      -- DropOutDeductionAmount - decimal(18, 0)
		, @DeductionTraineeAttendance                          -- DeductionTraineeAttendance - decimal(18, 0)
		, 0                                                    -- AttendanceDeductionAmount - decimal(18, 0)
		, @MiscDeductionNo                                     -- MiscDeductionNo - int
		, @MiscDeductionType                                   -- MiscDeductionType - nvarchar(250)
		, @MiscDeductionAmount                                 -- MiscDeductionAmount - decimal(18, 0)
		, @FunctionalDate                                      -- FunctionalDate - datetime
		, @PenaltyPercentage                                   -- PenaltyPercentage - decimal(18, 0)
		, @PenaltyAmount                                       -- PenaltyAmount - decimal(18, 0)
		, @ResultDeduction                                     -- ResultDeduction - decimal(18, 0)
		, @NetPayableAmount - (@ResultDeduction) - @MiscDeductionAmount - @PenaltyAmount  -- NetPayableAmount - decimal(18, 0)
		, @NetPayableAmount - (@ResultDeduction) - @MiscDeductionAmount - @PenaltyAmount  -- NetTrainingCost - decimal(18, 0)
		, 0                                                    -- InActive - bit
		, 0                                                    -- CreatedUserID - int
		, 0                                                    -- ModifiedUserID - int
		, GETDATE()                                            -- CreatedDate - datetime
		, NULL                                                 -- ModifiedDate - datetime
		, @StartDate                                           -- StartDate - date
		, @EndDate                                             -- EndDate - date
		, @ActualStartDate                                     -- ActualStartDate - date
		, @ActualEndDate                                       -- ActualEndDate - date
		, @Batch                                               -- Batch - int
		, @BatchDuration                                       -- BatchDuration - int
		, @TotalCostPerTrainee                                 -- TotalMonthlyPayment - decimal(18, 0)
		, @GrossPayableAmount                                  -- GrossPayable - decimal(18, 0)
		, 0                                                    -- DeductionTraineeUnVCnic - int
		, @ClassDays                                           -- ClassDays - int
		, 0                                                    -- IsApproved - bit
		, @Month                                               -- Month - date
		, 0                                                    -- IsPostedToSAP - bit
		, 0                                                    -- IsRejected - bit
		, @ProcessKey                                          -- ProcessKey - nvarchar(50)
		, @InvoiceHeaderID                                     -- InvoiceHeaderID - int
		, N''                                                  -- SAPID - nvarchar(max)
		--s-- updated by numan at 2021-12-17
		--, @InvoiceNumber + @BatchDuration                      -- InvoiceNumber
		, @InvoiceNumber                      -- InvoiceNumber
		--e-- updated by numan at 2021-12-17
		, @NetPayableAmount - (@ResultDeduction) - @MiscDeductionAmount - @PenaltyAmount               -- Total LC DECIMAL
		, @GrossPayableAmount					                 -- LineTotal decimal
		, @OcrCode                                             -- OcrCode1 , sap scheme code
		, @OcrCode2                                            -- OcrCode2 , Trade code
		, @OcrCode3                                            -- OcrCode3 , Department Code
		, @lineStatus                                          -- Line Status
		, @OcrCode4                                            -- OcrCode4
		, @PODocEntry                                          -- BaseEntry
		, 22                                                   -- BaseType
		, @POLineNum                                           -- BaseLine
		, @PaymentToBeReleasedTrainees);

	DECLARE @InvoiceID INT = SCOPE_IDENTITY();

	IF (@DeductionUniformBagReceiving > 0)
	BEGIN
		SET @MiscDeductionNo = @DeductionUniformBagReceiving
		SET @MiscDeductionType = 'Others'
		SET @MiscDeductionAmount = ((SELECT
				cim.Amount
			FROM dbo.ClassInvoiceMap cim
			WHERE cim.ClassID = @ClassID
			AND cim.InvoiceType = '2nd Last')
		* @DeductionUniformBagReceiving)

		INSERT INTO dbo.Invoice (SchemeID
		, ClassID
		, TradeID
		, Description
		, GLCode
		, GLName
		, TrainingServicesSaleTax
		, ProgCategory
		, FundingSource
		, TaxCode
		, WTaxLiable
		, InvoiceType
		, TraineePerClass
		, ClaimTrainees
		, Stipend
		, UniformBag
		, BoardingOrLodging
		, TestingFee
		, TotalCostPerTrainee
		, UnverifiedCNICDeductions
		, CnicDeductionType
		, CnicDeductionAmount
		, DeductionTraineeDroput
		, DropOutDeductionType
		, DropOutDeductionAmount
		, DeductionTraineeAttendance
		, AttendanceDeductionAmount
		, MiscDeductionNo
		, MiscDeductionType
		, MiscDeductionAmount
		, FunctionalDate
		, PenaltyPercentage
		, PenaltyAmount
		, ResultDeduction
		, NetPayableAmount
		, NetTrainingCost
		, InActive
		, CreatedUserID
		, ModifiedUserID
		, CreatedDate
		, ModifiedDate
		, StartDate
		, EndDate
		, ActualStartDate
		, ActualEndDate
		, Batch
		, BatchDuration
		, TotalMonthlyPayment
		, GrossPayable
		, DeductionTraineeUnVCnic
		, ClassDays
		, IsApproved
		, month
		, IsPostedToSAP
		, IsRejected
		, ProcessKey
		, InvoiceHeaderID
		, SAPID
		, InvoiceNumber
		, MiscMarginalTraineeCount
		, MiscMarginalTraineeAmount
		, MiscProfileDeductionCount
		, MiscProfileDeductionAmount
		, MiscFailedDeductionCount
		, MiscFailedDeductionAmount
		, TotalLC
		, MiscNonFunctionalAmount
		, LineTotal
		, OcrCode
		, OcrCode2
		, OcrCode3
		, LineStatus
		, OcrCode4
		, BaseEntry
		, BaseType
		, BaseLine
		, InCancel
		, PaymentToBeReleasedTrainees)
			VALUES (@SchemeID                                                                                     -- SchemeID - int
			, @ClassID                                                                                      -- ClassID - int
			, @TradeID                                                                                      -- TradeID - int
			, CONCAT(@ClassCode, ' - ', @Batch, ' - ', @TradeName)                                          -- Description - nvarchar(250)
			, @GLAccountCode                                                                                -- GLCode - nvarchar(250)
			, @GLName                                                                                       -- GLName - nvarchar(250)
			, (SELECT GLAccountCode FROM SAP_GLAccount WHERE GLAccountID = 4)                                                                                             -- TrainingServicesSaleTax - nvarchar(250)
			, @PCategory                                                                                    -- ProgCategory - int
			, @FundingSource                                                                                -- FundingSource - nvarchar(250)
			, @TaxCode                                                                                      -- TaxCode - nvarchar(250)
			, @WTLiable                                                                                     -- WTaxLiable - nvarchar(10)
			, N'2Last'																					  -- InvoiceType - nvarchar(250)
			, 0                                                                                             -- TraineePerClass - int
			, 0                                                                                             -- ClaimTrainees - int
			, 0                                                                                             -- Stipend - decimal(19, 4)
			, 0                                                                                             -- UniformBag - decimal(19, 4)
			, 0                                                                                             -- BoardingOrLodging - decimal(19, 4)
			, 0                                                                                             -- TestingFee - decimal(19, 4)
			, 0                                                                                             -- TotalCostPerTrainee - decimal(19, 4)
			, 0				                                                                              -- UnverifiedCNICDeductions - decimal(19, 4)
			, N''														                                      -- CnicDeductionType - nvarchar(250)
			, 0																							  -- CnicDeductionAmount - decimal(19, 4)
			, 0                                                                                             -- DeductionTraineeDroput - int
			, ''                                                                                            -- DropOutDeductionType - nvarchar(250)
			, 0                                                                                             -- DropOutDeductionAmount - decimal(19, 4)
			, 0				                                                                              -- DeductionTraineeAttendance - decimal(19, 4)
			, 0						                                                                      -- AttendanceDeductionAmount - decimal(19, 4)
			, @MiscDeductionNo                                                                              -- MiscDeductionNo - int
			, @MiscDeductionType                                                                            -- MiscDeductionType - nvarchar(250)
			, @MiscDeductionAmount                                                                          -- MiscDeductionAmount - decimal(19, 4)
			, @FunctionalDate                                                                               -- FunctionalDate - datetime
			, 0                                                                                             -- PenaltyPercentage - decimal(19, 4)
			, 0                                                                                             -- PenaltyAmount - decimal(19, 4)
			, 0                                                                                             -- ResultDeduction - decimal(19, 4)
			, -@MiscDeductionAmount																		  -- NetPayableAmount - decimal(19, 4)
			, 0						                                                                      -- NetTrainingCost - decimal(19, 4)    
			, 0                                                                                             -- InActive - bit
			, 15                                                                                            -- CreatedUserID - int
			, 0                                                                                             -- ModifiedUserID - int
			, GETDATE()                                                                                     -- CreatedDate - datetime
			, ''                                                                                            -- ModifiedDate - datetime
			, @StartDate                                                                                    -- StartDate - date
			, @EndDate                                                                                      -- EndDate - date
			, @ActualStartDate                                                                              -- ActualStartDate - date
			, @ActualEndDate                                                                                -- ActualEndDate - date
			, @Batch                                                                                        -- Batch - int
			, @BatchDuration                                                                                -- BatchDuration - int
			, 0                                                                                             -- TotalMonthlyPayment - decimal(19, 4)
			, 0                                                                                             -- GrossPayable - decimal(19, 4)
			, 0                                                                                             -- DeductionTraineeUnVCnic - int -not inserted in prev
			, 0                                                                                             -- ClassDays - int 
			, 0                                                                                             -- IsApproved - bit
			, @Month                                                                                        -- Month - date
			, 0                                                                                             -- IsPostedToSAP - bit -not inserted in prev
			, 0                                                                                             -- IsRejected - bit
			, @ProcessKey                                                                                   -- ProcessKey - nvarchar(50)
			, @InvoiceHeaderID                                                                              -- InvoiceHeaderID - int
			, N''                                                                                           -- SAPID - nvarchar(max) -not inserted in prev
			--s-- updated by numan at 2021-12-17
			--, @InvoiceNumber + @BatchDuration                                                               -- InvoiceNumber - int
			, @InvoiceNumber                                                                -- InvoiceNumber - int
			--e-- updated by numan at 2021-12-17
			, 0                                                                                             -- MiscMarginalTraineeCount - int -not inserted in prev
			, 0                                                                                             -- MiscMarginalTraineeAmount - decimal(19, 4) -not inserted in prev
			, 0                                                                                             -- MiscProfileDeductionCount - int -not inserted in prev
			, 0                                                                                             -- MiscProfileDeductionAmount - decimal(19, 4) -not inserted in prev
			, 0                                                                                             -- MiscFailedDeductionCount - int -not inserted in prev
			, 0                                                                                             -- MiscFailedDeductionAmount - decimal(19, 4) -not inserted in prev
			, -@MiscDeductionAmount																		  -- TotalLC - decimal(19, 4) (@ReimUnvTraineees * @TotalMonthlyPayment) + (@ReimAttendance * @TotalCostPerTrainee)
			, 0                                                                                             -- MiscNonFunctionalAmount - decimal(19, 4) -not inserted in prev
			, 0																							  -- LineTotal - decimal(19, 4)
			, @OcrCode                                                                                      -- OcrCode - nvarchar(100)
			, @OcrCode2                                                                                     -- OcrCode2 - nvarchar(100)
			, @OcrCode3                                                                                     -- OcrCode3 - nvarchar(100)
			, @lineStatus                                                                                   -- LineStatus - nvarchar(50)
			, @OcrCode4                                                                                     -- OcrCode4 - nvarchar(100)  -not inserted in prev
			, ''                                                                                            -- BaseEntry - nvarchar(250)
			, N'-1'                                                                                         -- BaseType - nvarchar(250)
			, @InvoiceID                                                                                    -- BaseLine - nvarchar(250) -- @POLineNum
			, 0                                                                                             -- InCancel - bit -not inserted in prev
			, @PaymentToBeReleasedTrainees);

	END

	IF (@ReimAttendance > 0)
	BEGIN
		----temporary variables needs tobe removed--
		----DECLARE @TotalMonthlyPayment FLOAT;-- from class invoice map cim.Amount 
		----DECLARE @UniformBagCost FLOAT; -- from class UniformBagCost
		----DECLARE @TotalCostPerTrainee_Reim_SI FLOAT; -- TotalCostPerTrainee * (@InvoiceNumber - 1);
		--DECLARE @UnfirmBag_Remb_SI FLOAT = @UniformBagCost; -- from class UniformBagCost
		----temporary variables needs tobe removed--
		--DECLARE @TotalMonthlyPayment_Regular FLOAT =
		--        (
		--            SELECT SUM(Amount)
		--            FROM dbo.ClassInvoiceMap AS cim
		--            WHERE cim.ClassID = @ClassID
		--                  AND cim.InvoiceType = 'Regular'
		--        );
		----DECLARE @TotalMonthlyPayment_Reim FLOAT = @TotalMonthlyPayment * (@InvoiceNumber - 1);--need clarity multiply with duration instead of invoice
		--DECLARE @TotalMonthlyPayment_ForReim FLOAT = @TotalMonthlyPayment_Regular + @UniformBagCost;

		--SET @TotalCostPerTrainee_Reim_SI = @TotalCostPerTrainee * (@InvoiceNumber - 1);
		--SELECT TOP (1) AttendanceDeductionAmount FROM dbo.Invoice i WHERE i.InvoiceNumber = @BatchDuration AND i.InvoiceType = 'Regular' AND i.ClassID = @ClassID --Olde
		SET @AttendanceDeductionAmount = (SELECT TOP (1)
				AttendanceDeductionAmount
			FROM dbo.Invoice i
			WHERE i.InvoiceType = 'Regular'
			AND i.ClassID = @ClassID
			ORDER BY i.InvoiceNumber DESC)

		INSERT INTO dbo.Invoice (SchemeID
		, ClassID
		, TradeID
		, Description
		, GLCode
		, GLName
		, TrainingServicesSaleTax
		, ProgCategory
		, FundingSource
		, TaxCode
		, WTaxLiable
		, InvoiceType
		, TraineePerClass
		, ClaimTrainees
		, Stipend
		, UniformBag
		, BoardingOrLodging
		, TestingFee
		, TotalCostPerTrainee
		, UnverifiedCNICDeductions
		, CnicDeductionType
		, CnicDeductionAmount
		, DeductionTraineeDroput
		, DropOutDeductionType
		, DropOutDeductionAmount
		, DeductionTraineeAttendance
		, AttendanceDeductionAmount
		, MiscDeductionNo
		, MiscDeductionType
		, MiscDeductionAmount
		, FunctionalDate
		, PenaltyPercentage
		, PenaltyAmount
		, ResultDeduction
		, NetPayableAmount
		, NetTrainingCost
		, InActive
		, CreatedUserID
		, ModifiedUserID
		, CreatedDate
		, ModifiedDate
		, StartDate
		, EndDate
		, ActualStartDate
		, ActualEndDate
		, Batch
		, BatchDuration
		, TotalMonthlyPayment
		, GrossPayable
		, DeductionTraineeUnVCnic
		, ClassDays
		, IsApproved
		, month
		, IsPostedToSAP
		, IsRejected
		, ProcessKey
		, InvoiceHeaderID
		, SAPID
		, InvoiceNumber
		, MiscMarginalTraineeCount
		, MiscMarginalTraineeAmount
		, MiscProfileDeductionCount
		, MiscProfileDeductionAmount
		, MiscFailedDeductionCount
		, MiscFailedDeductionAmount
		, TotalLC
		, MiscNonFunctionalAmount
		, LineTotal
		, OcrCode
		, OcrCode2
		, OcrCode3
		, LineStatus
		, OcrCode4
		, BaseEntry
		, BaseType
		, BaseLine
		, InCancel
		, PaymentToBeReleasedTrainees)
			VALUES (@SchemeID                                                                                     -- SchemeID - int
			, @ClassID                                                                                      -- ClassID - int
			, @TradeID                                                                                      -- TradeID - int
			, CONCAT(@ClassCode, ' - ', @Batch, ' - ', @TradeName)                                          -- Description - nvarchar(250)
			, @GLAccountCode                                                                                -- GLCode - nvarchar(250)
			, @GLName                                                                                       -- GLName - nvarchar(250)
			, (SELECT GLAccountCode FROM SAP_GLAccount WHERE GLAccountID = 4)                                                                                             -- TrainingServicesSaleTax - nvarchar(250)
			, @PCategory                                                                                    -- ProgCategory - int
			, @FundingSource                                                                                -- FundingSource - nvarchar(250)
			, @TaxCode                                                                                      -- TaxCode - nvarchar(250)
			, @WTLiable                                                                                     -- WTaxLiable - nvarchar(10)
			, N'Reimbursement'                                                                              -- InvoiceType - nvarchar(250)
			, 0                                                                                             -- TraineePerClass - int
			, 0                                                                                             -- ClaimTrainees - int
			, 0                                                                                             -- Stipend - decimal(19, 4)
			, 0                                                                                             -- UniformBag - decimal(19, 4)
			, 0                                                                                             -- BoardingOrLodging - decimal(19, 4)
			, 0                                                                                             -- TestingFee - decimal(19, 4)
			, 0                                                                                             -- TotalCostPerTrainee - decimal(19, 4)
			, 0				                                                                              -- UnverifiedCNICDeductions - decimal(19, 4)
			, N''														                                      -- CnicDeductionType - nvarchar(250)
			, 0																							  -- CnicDeductionAmount - decimal(19, 4)
			, 0                                                                                             -- DeductionTraineeDroput - int
			, ''                                                                                            -- DropOutDeductionType - nvarchar(250)
			, 0                                                                                             -- DropOutDeductionAmount - decimal(19, 4)
			, -@ReimAttendance                                                                              -- DeductionTraineeAttendance - decimal(19, 4)
			, -@AttendanceDeductionAmount                                                                    -- AttendanceDeductionAmount - decimal(19, 4)
			, 0                                                                                             -- MiscDeductionNo - int
			, N''                                                                                           -- MiscDeductionType - nvarchar(250)
			, 0                                                                                             -- MiscDeductionAmount - decimal(19, 4)
			, @FunctionalDate                                                                               -- FunctionalDate - datetime
			, 0                                                                                             -- PenaltyPercentage - decimal(19, 4)
			, 0                                                                                             -- PenaltyAmount - decimal(19, 4)
			, 0                                                                                             -- ResultDeduction - decimal(19, 4)
			, @AttendanceDeductionAmount																	  -- NetPayableAmount - decimal(19, 4)
			, @AttendanceDeductionAmount                                                                    -- NetTrainingCost - decimal(19, 4)    
			, 0                                                                                             -- InActive - bit
			, 15                                                                                            -- CreatedUserID - int
			, 0                                                                                             -- ModifiedUserID - int
			, GETDATE()                                                                                     -- CreatedDate - datetime
			, ''                                                                                            -- ModifiedDate - datetime
			, @StartDate                                                                                    -- StartDate - date
			, @EndDate                                                                                      -- EndDate - date
			, @ActualStartDate                                                                              -- ActualStartDate - date
			, @ActualEndDate                                                                                -- ActualEndDate - date
			, @Batch                                                                                        -- Batch - int
			, @BatchDuration                                                                                -- BatchDuration - int
			, 0                                                                                             -- TotalMonthlyPayment - decimal(19, 4)
			, 0                                                                                             -- GrossPayable - decimal(19, 4)
			, 0                                                                                             -- DeductionTraineeUnVCnic - int -not inserted in prev
			, 0                                                                                             -- ClassDays - int 
			, 0                                                                                             -- IsApproved - bit
			, @Month                                                                                        -- Month - date
			, 0                                                                                             -- IsPostedToSAP - bit -not inserted in prev
			, 0                                                                                             -- IsRejected - bit
			, @ProcessKey                                                                                   -- ProcessKey - nvarchar(50)
			, @InvoiceHeaderID                                                                              -- InvoiceHeaderID - int
			, N''                                                                                           -- SAPID - nvarchar(max) -not inserted in prev
			--s-- updated by numan at 2021-12-17
			--, @InvoiceNumber + @BatchDuration                                                               -- InvoiceNumber - int
			, @InvoiceNumber                                                                -- InvoiceNumber - int
			--e-- updated by numan at 2021-12-17
			, 0                                                                                             -- MiscMarginalTraineeCount - int -not inserted in prev
			, 0                                                                                             -- MiscMarginalTraineeAmount - decimal(19, 4) -not inserted in prev
			, 0                                                                                             -- MiscProfileDeductionCount - int -not inserted in prev
			, 0                                                                                             -- MiscProfileDeductionAmount - decimal(19, 4) -not inserted in prev
			, 0                                                                                             -- MiscFailedDeductionCount - int -not inserted in prev
			, 0                                                                                             -- MiscFailedDeductionAmount - decimal(19, 4) -not inserted in prev
			, @AttendanceDeductionAmount																	  -- TotalLC - decimal(19, 4) (@ReimUnvTraineees * @TotalMonthlyPayment) + (@ReimAttendance * @TotalCostPerTrainee)
			, 0                                                                                             -- MiscNonFunctionalAmount - decimal(19, 4) -not inserted in prev
			, 0																							  -- LineTotal - decimal(19, 4)
			, @OcrCode                                                                                      -- OcrCode - nvarchar(100)
			, @OcrCode2                                                                                     -- OcrCode2 - nvarchar(100)
			, @OcrCode3                                                                                     -- OcrCode3 - nvarchar(100)
			, @lineStatus                                                                                   -- LineStatus - nvarchar(50)
			, @OcrCode4                                                                                     -- OcrCode4 - nvarchar(100)  -not inserted in prev
			, ''                                                                                            -- BaseEntry - nvarchar(250)
			, N'-1'                                                                                         -- BaseType - nvarchar(250)
			, @InvoiceID                                                                                    -- BaseLine - nvarchar(250) -- @POLineNum
			, 0                                                                                             -- InCancel - bit -not inserted in prev
			, @PaymentToBeReleasedTrainees);

	END;
	FETCH NEXT FROM _cursor
	INTO @ClassCode
	, @PaymentToBeReleasedTrainees
	, @DeductionFailedTrainees
	, @ClaimedTrainees
	, @EnrolledTrainees
	, @ExtraTraineeDeductCompletion
	, @UnVDeductCompletion
	, @DropoutDeductCompletion
	, @AbsentDeductCompletion
	, @CNICUnverified
	, @ReimAttendance
	--, @ReimUnvTraineees
	, @FunctionalDate
	--, @DeductDropoutForTheMonthCount
	, @PenaltyPercentage
	, @DeductionTraineeAttendance
	, @DeductionUniformBagReceiving;

	END;

	UPDATE dbo.InvoiceHeader
	SET DocTotal = (SELECT
			SUM(i.LineTotal)
		FROM dbo.Invoice AS i
		WHERE i.InvoiceHeaderID = @InvoiceHeaderID)
	WHERE InvoiceHeaderID = @InvoiceHeaderID;
	--COMMIT TRANSACTION
	CLOSE _cursor;
	DEALLOCATE _cursor;
--END TRY
--BEGIN CATCH
--   --ROLLBACK TRANSACTION;
--INSERT INTO dbo.JobErrorLog
--   (
--      ErrorNumber
--    , ErrorState
--    , ErrorSeverity
--    , ErrorProcedure
--    , ErrorLine
--    , ErrorMessage
--    , ErrorDateTime
--    , CustomRemarks
--   )
--   VALUES
--   (  ERROR_NUMBER()    -- ErrorNumber - int
--    , ERROR_STATE()     -- ErrorState - int
--    , ERROR_SEVERITY()  -- ErrorSeverity - int
--    , ERROR_PROCEDURE() -- ErrorProcedure - nvarchar(1000)
--    , ERROR_LINE()      -- ErrorLine - int
--    , ERROR_MESSAGE()   -- ErrorMessage - nvarchar(4000)
--    , GETDATE()         -- ErrorDateTime - datetime
--    , N''               -- CustomRemarks - nvarchar(1000)
--      )
--END CATCH

END;
-------------------------------------
GO
ALTER PROCEDURE [dbo].[GenerateINVEmployment]  --@PRNMasterID=4938, @ProcessKey='INV_F'
@PRNMasterID INT
, @ProcessKey NVARCHAR(50)
AS
BEGIN
	--DECLARE @ProcessKey NVARCHAR(50) = N'INV_C'

	--BEGIN TRANSACTION
	--BEGIN TRY
	DECLARE @TSPID INT
		   ,@TSPCode NVARCHAR(200)
		   ,@TSPName NVARCHAR(200)
		   ,@CtlAccount NVARCHAR(50)
		   ,@U_SCHEME NVARCHAR(50)
		   ,@U_SCH_Code NVARCHAR(50)
		   ,@ClassCode NVARCHAR(100)
		   ,@PaymentToBeReleasedTrainees FLOAT
		   ,@DeductionFailedTrainees FLOAT
		   ,@TaxCode NVARCHAR(250)
		   ,@WTLiable NVARCHAR(10)
		   ,@ClaimedTrainees INT
		   ,@VerifiedTrainees INT
		   ,@OcrCode NVARCHAR(100) = ''
		   ,@OcrCode2 NVARCHAR(100) = ''
		   ,@OcrCode3 NVARCHAR(100) = ''
		   ,@OcrCode4 NVARCHAR(100) = ''
		   ,@lineStatus NVARCHAR(50) = ''
		   ,@POLineNum INT
		   ,@PODocEntry INT
		   ,@PODocNum INT
		   ,@EnrolledTrainees INT = 0
		   ,@CNICUnverified INT = 0
		   ,@CNICVerified INT = 0
		   ,@GrossPayableAmount FLOAT
		   ,@DropoutsUnverified INT = 0
		   ,@DropoutsVerified INT = 0
		   ,@ExpelledUnverified INT = 0
		   ,@ExpelledVerified INT = 0
		   ,@CNICUnVExcesses INT = 0
		   ,@CNICVExcesses INT = 0
		   ,@ClassDays INT = 30;


	DECLARE @CardCode NVARCHAR(100)
		   ,@SchemeCode NVARCHAR(150)
		   ,@PaymentStructure NVARCHAR(150)
		   ,@Month DATE
		   ,@MonthPO DATE
		   ,@sapBPLId INT
		   ,@sapSchemeCode NVARCHAR(150)
		   ,@sapOcrCode NVARCHAR(150)
		   ,@POHeaderID INT
		   ,@SchemeNameBSS NVARCHAR(250)
		   ,@SchemeCodeBSS NVARCHAR(10)
			--s--added by numan at 2021-12-17
		   ,@InvoiceNumber INT
	--e--added by numan at 2021-12-17

	SELECT
		@TSPID = t.TspID
	   ,@SchemeNameBSS = s.SchemeName
	   ,@SchemeCodeBSS = s.SchemeCode
	   ,@CardCode = tm.SAPID
	   ,@TSPName = tm.TSPName
	   ,@SchemeCode = s.SAPID
	   ,@PaymentStructure = s.PaymentSchedule
	   ,@Month = pm.[month]
	   ,@MonthPO = poh.month
	   ,@POHeaderID = poh.POHeaderID
	   ,@sapSchemeCode = sap.SchemeCode
	   ,@sapBPLId = sb.BranchID
	   ,@PODocEntry = poh.DocEntry
	   ,@PODocNum = poh.DocNum
	   ,@U_SCH_Code = s.SAPID
	FROM TSPDetail t
	INNER JOIN TSPMaster tm
		ON tm.TSPMasterID = t.TSPMasterID
	INNER JOIN PRNMaster pm
		ON pm.TspID = t.TspID
	INNER JOIN Scheme s
		ON s.SchemeID = t.SchemeID
	INNER JOIN POHeader poh
		ON poh.TspID = t.TspID
	INNER JOIN SAP_Scheme sap
		ON sap.PaymentStructure = s.PaymentSchedule
	INNER JOIN dbo.SAP_BPL sb
		ON sb.FundingCategoryID = s.FundingCategoryID
	WHERE pm.PRNMasterID = @PRNMasterID;

	SELECT
		@CtlAccount = ssf.CtlAccount
	   ,@TaxCode = ssf.TaxCode
	   ,@WTLiable = ssf.WtLiable
	   ,@lineStatus = LineStatus
	FROM dbo.SAP_StaticField AS ssf
	WHERE ssf.SAP_StaticFileldID = 1
	--
	SELECT
		@TSPCode = tm.SAPID
	   ,@TSPName = td.TSPName
	FROM dbo.TSPDetail AS td
	INNER JOIN dbo.Scheme AS s
		ON s.SchemeID = td.SchemeID
	INNER JOIN dbo.SAP_Scheme AS ss
		ON ss.PaymentStructure = s.PaymentSchedule
	INNER JOIN TSPMaster tm
		ON tm.TSPMasterID = td.TSPMasterID
	WHERE td.TspID = @TSPID

	INSERT INTO dbo.InvoiceHeader (DocNum
	, DocType
	, Printed
	, DocDate
	, DocDueDate
	, CardCode
	, CardName
	, DocTotal
	, Comments
	, JournalMemo
	, BPL_IDAssignedToInvoice
	, CtlAccount
	, U_SCHEME
	, U_Sch_Code
	, U_Month
	, InActive
	, CreatedUserID
	, ModifiedUserID
	, CreatedDate
	, ModifiedDate
	, TspID
	, ProcessKey
	, IsApproved
	, IsRejected
	, POHeaderID)
		VALUES (0            -- DocNum - INT
		, N'S'         -- DocType - NVARCHAR(10)
		, N'N'         -- PrINTed - NVARCHAR(10)
		, GETDATE()    -- DocDate - date
		, GETDATE()    -- DocDueDate - date
		, @TSPCode     -- CardCode - NVARCHAR(200)
		, @TSPName     -- CardName - NVARCHAR(200)
		, 0            -- DocTotal - decimal(18, 0)
		, N'Last '     -- Comments - NVARCHAR(max)
		, N'Last '     -- JournalMemo - NVARCHAR(200)
		, @sapBPLId    -- BPL_IDAssignedToInvoice - INT
		, @CtlAccount  -- CtlAccount - NVARCHAR(50)
		, @sapSchemeCode -- U_SCHEME - NVARCHAR(50)
		, @U_SCH_Code  -- U_SCH_Code - NVARCHAR(50)
		, @Month       -- U_Month - date
		, 0            -- InActive - bit
		, 0            -- CreatedUserID - INT
		, 0            -- ModifiedUserID - INT
		, GETDATE()    -- CreatedDate - datetime
		, NULL         -- ModifiedDate - datetime
		, @TSPID       -- TSPID - INT
		, @ProcessKey  -- ProcessKey - NVARCHAR(50)
		, 0            -- IsApproved - bit
		, 0            -- IsRejected - bit
		, @POHeaderID  -- POHeaderID INT
		);
	DECLARE @InvoiceHeaderID INT = SCOPE_IDENTITY()
	--send to 1st approval
	INSERT INTO dbo.ApprovalHistory (Step
	, FormID
	, ApproverID
	, Comments
	, ApprovalStatusID
	, CreatedUserID
	, ModifiedUserID
	, CreatedDate
	, ModifiedDate
	, InActive
	, ProcessKey)
		VALUES (1                -- Step - INT
		, @InvoiceHeaderID -- FormID - INT
		, 0                -- ApproverID - INT
		, N'Pending'       -- Comments - NVARCHAR(4000)
		, 1                -- ApprovalStatusID - INT
		, 0                -- CreatedUserID - INT
		, 0                -- ModifiedUserID - INT
		, GETDATE()        -- CreatedDate - datetime
		, NULL             -- ModifiedDate - datetime
		, 0                -- InActive - bit
		, @ProcessKey      -- ProcessKey - NVARCHAR(100)
		)

	DECLARE _cursor CURSOR LOCAL FOR (SELECT
			p.ClassCode
		   ,p.PaymentToBeReleasedTrainees
		   ,p.ClaimedTrainees
		   ,p.EnrolledTrainees
		   ,p.VerifiedTrainees
		   ,p.CNICUnverified
		   ,p.CNICVerified
		   ,p.DropoutsUnverified
		   ,p.DropoutsVerified
		   ,p.ExpelledUnverified
		   ,p.ExpelledVerified
		   ,p.CNICUnVExcesses
		   ,p.CNICVExcesses
		FROM dbo.PRN AS p
		WHERE p.PRNMasterID = @PRNMasterID)
	OPEN _cursor
	FETCH NEXT FROM _cursor
	INTO @ClassCode
	, @PaymentToBeReleasedTrainees
	, @ClaimedTrainees
	, @EnrolledTrainees
	, @VerifiedTrainees
	, @CNICUnverified
	, @CNICVerified
	, @DropoutsUnverified
	, @DropoutsVerified
	, @ExpelledUnverified
	, @ExpelledVerified
	, @CNICUnVExcesses
	, @CNICVExcesses
	WHILE @@fetch_status = 0
	BEGIN
	DECLARE @SchemeID INT
		   ,@ClassID INT
		   ,@TradeID INT
		   ,@TradeName NVARCHAR(250)
		   ,@GLCode NVARCHAR(250)
		   ,@GLName NVARCHAR(250)
		   ,@TrainingServicesSaleTax NVARCHAR(250)
		   ,@PCategory INT
		   ,@FundingSource NVARCHAR(250)
		   ,@InvoiceType NVARCHAR(250) = N'Last'
		   ,@TraineePerClass INT
		   ,@TotalCostPerTrainee DECIMAL(18, 6)
		   ,@StartDate DATE
		   ,@EndDate DATE
		   ,@ActualStartDate DATE
		   ,@ActualEndDate DATE
		   ,@Batch INT
		   ,@BatchDuration INT
		   ,@ResultDeduction DECIMAL(18, 6)
		   ,@NetPayableAmount DECIMAL(18, 6)
		   ,@NetTrainingCost DECIMAL(18, 6)
		   ,@ProgramType INT  ---Include Program Type for Specific GLAccount 

	SELECT
		@SchemeID = s.SchemeID
	   ,@ClassID = c.ClassID
	   ,@TradeID = c.TradeID
	   ,@TradeName = t.TradeName
	   ,@PCategory = s.PCategoryID
	   ,@FundingSource = fs.FundingSourceName
	   ,@StartDate = c.StartDate
	   ,@EndDate = c.EndDate
	   ,@TraineePerClass = c.TraineesPerClass
	   ,@Batch = c.Batch
	   ,@BatchDuration = c.Duration
	   ,@ActualStartDate = ir.ActualStartDate
	   ,@ActualEndDate = ir.ActualEndDate
	   ,@ProgramType = s.ProgramTypeID
	FROM dbo.Class AS c
	INNER JOIN dbo.InceptionReport AS ir
		ON ir.ClassID = c.ClassID
	INNER JOIN dbo.TSPDetail AS td
		ON td.TspID = c.TspID
	INNER JOIN dbo.Scheme AS s
		ON s.SchemeID = td.SchemeID
	INNER JOIN dbo.Trade AS t
		ON t.TradeID = c.TradeID
	INNER JOIN dbo.FundingSource AS fs
		ON fs.FundingSourceID = s.FundingSourceID
	WHERE c.ClassCode = @ClassCode
	DECLARE @CnicUnverifiedAmount FLOAT;
	SELECT
		@TotalCostPerTrainee = cim.Amount
	   ,@ResultDeduction = 0
	   ,@NetTrainingCost = cim.Amount * @PaymentToBeReleasedTrainees
	   ,@CnicUnverifiedAmount = cim.Amount * @CNICUnverified
	   ,@GrossPayableAmount = cim.Amount * @TraineePerClass
	   ,@ClassDays = cim.InvoiceDays

		--s--added by numan at 2021-12-17
	   ,@InvoiceNumber = cim.InvoiceNo
	--e--added by numan at 2021-12-17
	FROM dbo.ClassInvoiceMap AS cim
	WHERE cim.ClassID = @ClassID
	AND cim.InvoiceType = 'Final'


	DECLARE @deptCode INT = (SELECT
			s.PCategoryID
		FROM Class
		INNER JOIN Scheme s
			ON s.SchemeID = Class.SchemeID
		WHERE Class.ClassID = @ClassID)
	DECLARE @tradeidsap NVARCHAR(100) = '';
	SET @tradeidsap = (SELECT
			t.SAPID
		FROM Trade t
		INNER JOIN Class
			ON Class.TradeID = t.TradeID
		WHERE Class.ClassID = @ClassID);
	SET @OcrCode2 = @tradeidsap;
	IF (@deptCode = 1)--6 dev db 1 DEP DB, Business Development
	BEGIN
		SET @deptCode = N'01';
		SET @OcrCode3 = N'BD011';
	--SET @ocrCode2 = @tradeidsap+'-01';
	END
	ELSE
	IF (@deptCode = 2) --7 dev db 2 DEP DB, Program Development
	BEGIN
		SET @deptCode = N'00';
		SET @OcrCode3 = N'PD002';
	--SET @ocrCode2 = @tradeidsap+'-00';
	END

	SELECT
		@OcrCode = U_Sch_Code
	   ,@OcrCode4 = BPL_IDAssignedToInvoice
	FROM InvoiceHeader
	WHERE InvoiceHeaderID = @InvoiceHeaderID;

	SELECT TOP 1
		@POLineNum = pl.LineNum
	FROM POLines pl
	INNER JOIN PRN
		ON PRN.ClassCode = pl.U_Class_Code
	INNER JOIN PRNMaster pm
		ON pm.PRNMasterID = PRN.PRNMasterID
			AND pm.ProcessKey = 'PRN_F'
	INNER JOIN POHeader PH
		ON PH.POHeaderID = pl.POHeaderID
	WHERE PRN.ClassID = @ClassID
	AND ISNULL(pl.InActive, 0) = 0
	AND CAST(FORMAT(PH.month, 'yyyyMM') AS INT) = CAST(FORMAT(CAST(@MonthPO AS DATE), 'yyyyMM') AS INT);



	--Updated by Ali Haider on 22-Apl-2024
	-----Get @GLAccountCode according to program type if SSP than update Scholorship Cost upon Training Cost
	IF (@ProgramType = 10)
	BEGIN
		SELECT
			@GLCode = sga.GLAccountCode
		   ,@GLName = sga.GLName
		FROM dbo.SAP_GLAccount AS sga
		WHERE sga.GLAccountID = 7
	END
	ELSE
	BEGIN
		SELECT
			@GLCode = sga.GLAccountCode
		   ,@GLName = sga.GLName
		FROM dbo.SAP_GLAccount AS sga
		WHERE sga.GLAccountID = 1
	END

	-------------End updation------------

	SELECT
		@TrainingServicesSaleTax = sga.GLAccountCode
	FROM dbo.SAP_GLAccount AS sga
	WHERE sga.GLAccountID = 4

	DECLARE @MiscDedNumber INT = 0
		   ,@MiscDedAmount FLOAT = 0
		   ,@MiscDedCat NVARCHAR(100) = '';
	SELECT
		@MiscDedNumber = @EnrolledTrainees - @CNICUnverified - @PaymentToBeReleasedTrainees - @DropoutsUnverified - @DropoutsVerified - @ExpelledUnverified - @ExpelledVerified - @CNICUnVExcesses - @CNICVExcesses
	IF (@MiscDedNumber > 0)
	BEGIN
		SET @MiscDedAmount = @MiscDedNumber * @TotalCostPerTrainee;
		SET @MiscDedCat = 'Others';
	END

	IF @EnrolledTrainees < @ClaimedTrainees
	BEGIN
		SET @ClaimedTrainees = @EnrolledTrainees;
	END;

	DECLARE @DeductionTraineeDroputNumber INT = 0
		   ,@DeductionTraineeDroputAmount FLOAT = 0
		   ,@DeductionTraineeDroputCat NVARCHAR(100) = N'';
	SELECT
		@DeductionTraineeDroputNumber = @ClaimedTrainees - @MiscDedNumber - @PaymentToBeReleasedTrainees - @CNICUnverified
	IF (@DeductionTraineeDroputNumber > 0)
	BEGIN
		SET @DeductionTraineeDroputAmount = @DeductionTraineeDroputNumber * @TotalCostPerTrainee;
		SET @DeductionTraineeDroputCat = 'DFM';
	END
	--Gross Payable-Cnic Deduction Amount-DropOut Deduction Amount-Misc Deduction Amount


	--added by numan at 2021-09-03
	SET @GrossPayableAmount = @ClaimedTrainees * @TotalCostPerTrainee
	--added by numan at 2021-09-03

	--below line moved from 338 to 344 at 2021-11-15
	SET @NetPayableAmount = @GrossPayableAmount - @CnicUnverifiedAmount - @DeductionTraineeDroputAmount - @MiscDedAmount;

	INSERT INTO dbo.Invoice (OcrCode
	, OcrCode2
	, OcrCode3
	, OcrCode4
	, LineStatus
	, SchemeID
	, ClassID
	, TradeID
	, Description
	, GLCode
	, GLName
	, TrainingServicesSaleTax
	, ProgCategory
	, FundingSource
	, TaxCode
	, WTaxLiable
	, InvoiceType
	, TraineePerClass
	, ClaimTrainees
	, Stipend
	, UniformBag
	, BoardingOrLodging
	, TestingFee
	, TotalCostPerTrainee
	, UnverifiedCNICDeductions
	, CnicDeductionType
	, CnicDeductionAmount
	, DeductionTraineeDroput
	, DropOutDeductionType
	, DropOutDeductionAmount
	, DeductionTraineeAttendance
	, AttendanceDeductionAmount
	, MiscDeductionNo
	, MiscDeductionType
	, MiscDeductionAmount
	, FunctionalDate
	, PenaltyPercentage
	, PenaltyAmount
	, ResultDeduction
	, NetPayableAmount
	, NetTrainingCost
	, InActive
	, CreatedUserID
	, ModifiedUserID
	, CreatedDate
	, ModifiedDate
	, StartDate
	, EndDate
	, ActualStartDate
	, ActualEndDate
	, Batch
	, BatchDuration
	, TotalMonthlyPayment
	, GrossPayable
	, DeductionTraineeUnVCnic
	, ClassDays
	, IsApproved
	, month
	, IsPostedToSAP
	, IsRejected
	, ProcessKey
	, InvoiceHeaderID
	, LineTotal
	, TotalLC
	, BaseEntry
	, BaseType
	, BaseLine
	--s--added by numan at 2021-12-17
	, InvoiceNumber
	, PaymentToBeReleasedTrainees
	--e--added by numan at 2021-12-17
	)
		VALUES (@OcrCode                                            -- OcrCode1 , sap scheme code
		, @OcrCode2                                          -- OcrCode2 , Trade code
		, @OcrCode3                                          -- OcrCode3 , Department Code
		, @OcrCode4                                          -- OcrCode4 , Branch Code
		, @lineStatus                                         -- Line Status
		, @SchemeID                                            -- SchemeID - INT
		, @ClassID                                             -- ClassID - INT
		, @TradeID                                             -- TradeID - INT
		, CONCAT(@ClassCode, ' - ', @Batch, ' - ', @TradeName) -- Description - NVARCHAR(250)
		, @GLCode                                              -- GLCode - NVARCHAR(250)
		, @GLName                                              -- GLName - NVARCHAR(250)
		, @TrainingServicesSaleTax                             -- TrainingServicesSaleTax - NVARCHAR(250)
		, @PCategory                                           -- ProgCategory - INT
		, @FundingSource                                       -- FundingSource - NVARCHAR(250)
		, @TaxCode                                             -- TaxCode - NVARCHAR(250)
		, @WTLiable                                            -- WTaxLiable - NVARCHAR(10)
		, @InvoiceType                                         -- InvoiceType - NVARCHAR(250)
		, @TraineePerClass                                     -- TraineePerClass - INT
		, @ClaimedTrainees                                     -- ClaimTrainees - INT
		, 0                                                    -- Stipend - decimal(18, 0)
		, 0                                                    -- UniformBag - decimal(18, 0)
		, 0                                                    -- BoardingOrLodging - decimal(18, 0)
		, 0                                                    -- TestingFee - decimal(18, 0)  SELECT * FROM INVOICE
		, @TotalCostPerTrainee                                 -- TotalCostPerTrainee - decimal(18, 0)
		, @CNICUnverified                                      -- UnverifiedCNICDeductions - decimal(18, 0)
		, CASE WHEN @CNICUnverified > 0 THEN N'UNCNICFTM' ELSE N'' END            -- CnicDeductionType - NVARCHAR(250)
		, CASE WHEN @CNICUnverified > 0 THEN @CnicUnverifiedAmount ELSE 0 END     -- CnicDeductionAmount - decimal(18, 0)
		, @DeductionTraineeDroputNumber                        -- DeductionTraineeDroput - INT
		, @DeductionTraineeDroputCat                           -- DropOutDeductionType - NVARCHAR(250)
		, @DeductionTraineeDroputAmount                           -- DropOutDeductionAmount - decimal(18, 0)
		, 0                                                    -- DeductionTraineeAttendance - decimal(18, 0)
		, 0                                                    -- AttendanceDeductionAmount - decimal(18, 0)
		, @MiscDedNumber                                       -- MiscDeductionNo - INT
		, @MiscDedCat                                          -- MiscDeductionType - NVARCHAR(250)
		, @MiscDedAmount                                       -- MiscDeductionAmount - decimal(18, 0)
		, GETDATE()                                            -- FunctionalDate - datetime
		, 0                                                    -- PenaltyPercentage - decimal(18, 0)
		, 0                                                    -- PenaltyAmount - decimal(18, 0)
		, @ResultDeduction                                     -- ResultDeduction - decimal(18, 0)
		, @NetPayableAmount                                    -- NetPayableAmount - decimal(18, 0)
		--s--updated by numan at 2021-12-08--
		--, CASE WHEN @PaymentToBeReleasedTrainees = 0 THEN @NetPayableAmount ELSE @NetPayableAmount + @MiscDedAmount END   -- NetTrainingCost - decimal(18, 0)
		, @NetPayableAmount
		--e--updated by numan at 2021-12-08--
		, 0                                                    -- InActive - bit
		, 0                                                    -- CreatedUserID - INT
		, 0                                                    -- ModifiedUserID - INT
		, GETDATE()                                            -- CreatedDate - datetime
		, NULL                                                 -- ModifiedDate - datetime
		, @StartDate                                           -- StartDate - date
		, @EndDate                                             -- EndDate - date
		, @ActualStartDate                                     -- ActualStartDate - date
		, @ActualEndDate                                       -- ActualEndDate - date
		, @Batch                                               -- Batch - INT
		, @BatchDuration                                       -- BatchDuration - INT
		, @TotalCostPerTrainee                                 -- TotalMonthlyPayment - decimal(18, 0)
		, @GrossPayableAmount                                  -- GrossPayable - decimal(18, 0)
		, 0                                                    -- DeductionTraineeUnVCnic - INT
		, @ClassDays                                           -- ClassDays - INT
		, 0                                                    -- IsApproved - bit
		, @Month                                               -- Month - date
		, 0                                                    -- IsPostedToSAP - bit
		, 0                                                    -- IsRejected - bit
		, @ProcessKey                                          -- ProcessKey - NVARCHAR(50)
		, @InvoiceHeaderID                                     -- InvoiceHeaderID - INT
		, @GrossPayableAmount                                  -- LineTotal INT
		, @NetPayableAmount									  -- TotalLC
		, @PODocEntry										  -- BaseEntry
		, 22													  -- BaSEType
		, @POLineNum											  -- BaseLine
		--s--added by numan at 2021-12-17
		, @InvoiceNumber										--InvoiceNumber
		, @PaymentToBeReleasedTrainees
		--e--added by numan at 2021-12-17
		)

	FETCH NEXT FROM _cursor
	INTO @ClassCode
	, @PaymentToBeReleasedTrainees
	, @ClaimedTrainees
	, @EnrolledTrainees
	, @VerifiedTrainees
	, @CNICUnverified
	, @CNICVerified
	, @DropoutsUnverified
	, @DropoutsVerified
	, @ExpelledUnverified
	, @ExpelledVerified
	, @CNICUnVExcesses
	, @CNICVExcesses
	END

	UPDATE dbo.InvoiceHeader
	SET DocTotal = (SELECT
			SUM(i.LineTotal)
		FROM dbo.Invoice AS i
		WHERE i.InvoiceHeaderID = @InvoiceHeaderID)
	WHERE InvoiceHeaderID = @InvoiceHeaderID
	--COMMIT TRANSACTION
	CLOSE _cursor;
	DEALLOCATE _cursor;
--END TRY
--BEGIN CATCH
--   --ROLLBACK TRANSACTION;
--INSERT INTO dbo.JobErrorLog
--   (
--      ErrorNumber
--    , ErrorState
--    , ErrorSeverity
--    , ErrorProcedure
--    , ErrorLine
--    , ErrorMessage
--    , ErrorDateTime
--    , CustomRemarks
--   )
--   VALUES
--   (  ERROR_NUMBER()    -- ErrorNumber - INT
--    , ERROR_STATE()     -- ErrorState - INT
--    , ERROR_SEVERITY()  -- ErrorSeverity - INT
--    , ERROR_PROCEDURE() -- ErrorProcedure - NVARCHAR(1000)
--    , ERROR_LINE()      -- ErrorLine - INT
--    , ERROR_MESSAGE()   -- ErrorMessage - NVARCHAR(4000)
--    , GETDATE()         -- ErrorDateTime - datetime
--    , N''               -- CustomRemarks - NVARCHAR(1000)
--      )
--END CATCH

END
-----------------------------------

GO

CREATE OR ALTER PROCEDURE AU_UpdateFlageFinalTSPAssignment @SchemeID INT = 0
AS
BEGIN
	UPDATE ta
	SET IsFinal = 1
	FROM SSPTSPAssignment ta
	INNER JOIN (SELECT
			sip.SchemeID
		   ,sip.TradeLotID
		   ,sip.TrainingLocationID
		   ,sip.TspID
		FROM SSPTraineeInterestProfile sip
		WHERE sip.ApprovalStatus = 'Accept'
		GROUP BY sip.SchemeID
				,sip.TradeLotID
				,sip.TrainingLocationID
				,sip.TspID
		HAVING COUNT(*) >= 15) AS filtered_interests
		ON filtered_interests.SchemeID = ta.ProgramID
		AND filtered_interests.TradeLotID = ta.TradeLotID
		AND filtered_interests.TrainingLocationID = ta.TrainingLocationID
		AND filtered_interests.TspID = ta.TSPID
	WHERE ta.ProgramID = @SchemeID;
END

GO
CREATE OR ALTER PROCEDURE [dbo].[AU_SSPNotificationDetail]  
    @Subject NVARCHAR(400) = ''  
  , @CustomComments NVARCHAR(MAX) = ''  
  , @UserID INT  
  , @CurUserID INT  

AS
BEGIN
	INSERT INTO [SSPNotificationDetail] (Subject
	, CustomComments
	, UserID
	, CreatedUserID
	, CreatedDate
	, NotificationTypeId)
	VALUES (@Subject, @CustomComments, @UserID, @CurUserID, GETDATE(),2)
	SELECT SCOPE_IDENTITY() AS id
END;  

GO

CREATE or alter  PROCEDURE [dbo].[RD_SSPNotificationsForEmail]    
AS    
BEGIN    
declare @SSPUsers int;
SELECT @SSPUsers=s.OTPCode FROM SSPTSPProfile s where s.UserID=577

IF @SSPUsers is null BEGIN  
	SELECT nd.NotificationDetailID      
         ,nd.Subject    
         , nd.CustomComments    
         , u.UserID    
         , u.UserName     
         , u.FullName    
         , u.Email     
    FROM dbo.SSPNotificationDetail        AS nd    
        INNER JOIN dbo.Users           AS u ON u.UserID = nd.UserID    
    WHERE nd.NotificationTypeId = 2  AND ISNULL(nd.IsSent, 0) = 0 and u.Email != '';     
END
else
BEGIN
	SELECT nd.NotificationDetailID      
         ,nd.Subject    
         , nd.CustomComments    
         , u.UserID    
         , u.UserName     
         , u.FullName    
         , u.Email     
    FROM dbo.SSPNotificationDetail        AS nd    
        INNER JOIN dbo.SSPUsers           AS u ON u.UserID = nd.UserID    
    WHERE nd.NotificationTypeId = 2  AND ISNULL(nd.IsSent, 0) = 0 and u.Email != '';     
END
    
END; 

GO
CREATE PROCEDURE [dbo].[AU_SSPCriteriaApproveReject] @CriteriaTemplateID INT,
@CurUserID INT,
@IsApproved BIT,
@IsRejected BIT
AS
BEGIN
	UPDATE SSPCriteriaHeader
	SET IsSubmitted =
		CASE
			WHEN @IsRejected = 1 THEN 0
			ELSE IsSubmitted
		END
	   ,IsApproved = @IsApproved
	   ,IsRejected = @IsRejected
	   ,ModifiedUserID = @CurUserID
	   ,ModifiedDate = GETDATE()
	WHERE CriteriaHeaderID = @CriteriaTemplateID;
END


GO

CREATE PROCEDURE AU_SSPCriteriaFinalApproval @CriteriaTemplateID INT = 0
AS
BEGIN
	UPDATE SSPCriteriaHeader
	SET IsApproved = 1
	WHERE CriteriaHeaderID = @CriteriaTemplateID
END
