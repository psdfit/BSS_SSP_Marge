




--Start TSP Registration Tables
SELECT * INTO [SSP_Testing].dbo.[SSPTSPProfile]                   FROM [SSP_Testing].dbo.[SSPTSPProfile]
SELECT * INTO [SSP_Testing].dbo.[SSPUsers]                        FROM [SSP_Testing].dbo.[SSPUsers]
SELECT * INTO [SSP_Testing].dbo.[SSPUsersPwd]                     FROM [SSP_Testing].dbo.[SSPUsersPwd]
SELECT * INTO [SSP_Testing].dbo.[SSPApprovalStatus]               FROM [SSP_Testing].dbo.[SSPApprovalStatus]
SELECT * INTO [SSP_Testing].dbo.[SSPTSPTradeManage]               FROM [SSP_Testing].dbo.[SSPTSPTradeManage]
SELECT * INTO [SSP_Testing].dbo.[SSPTSPTradeStatusHistory]        FROM [SSP_Testing].dbo.[SSPTSPTradeStatusHistory]
SELECT * INTO [SSP_Testing].dbo.[SSPTSPTrainingLocation]          FROM [SSP_Testing].dbo.[SSPTSPTrainingLocation]
SELECT * INTO [SSP_Testing].dbo.[SSPBank]                         FROM [SSP_Testing].dbo.[SSPBank]
SELECT * INTO [SSP_Testing].dbo.[SSPLegalStatus]                  FROM [SSP_Testing].dbo.[SSPLegalStatus]
SELECT * INTO [SSP_Testing].dbo.[SSPRegistrationStatus]           FROM [SSP_Testing].dbo.[RegistrationStatus]
SELECT * INTO [SSP_Testing].dbo.[SSPSalesTaxType]                 FROM [SSP_Testing].dbo.[SalesTaxType]
SELECT * INTO [SSP_Testing].dbo.[SSPTSPBankDetail]                FROM [SSP_Testing].dbo.[TspBankDetail]
SELECT * INTO [SSP_Testing].dbo.[SSPTrainerProfile]               FROM [SSP_Testing].dbo.[TrainerProfile]
SELECT * INTO [SSP_Testing].dbo.[SSPTrainerProfileDetail]         FROM [SSP_Testing].dbo.[TrainerProfileDetail]
SELECT * INTO [SSP_Testing].dbo.[SSPTrainingCertification]        FROM [SSP_Testing].dbo.[TrainingCertification]
SELECT * INTO [SSP_Testing].dbo.[SSPTSPRegistrationFee]           FROM [SSP_Testing].dbo.[TSPRegistrationFee]
--SELECT * INTO [SSP_Testing].dbo.[SSPTSPRegistrationPaymentMaster] FROM [SSP_Testing].dbo.[TSPRegistrationPaymentMaster]
SELECT * INTO [SSP_Testing].dbo.[SSPTSPRegistrationPaymentDetail] FROM [SSP_Testing].dbo.[TSPRegistrationPaymentDetail]
--SELECT * INTO [SSP_Testing].dbo.[SSPTSPRegistrationPaymentHistory]FROM [SSP_Testing].dbo.[TSPRegistrationPaymentHistory]
SELECT * INTO [SSP_Testing].dbo.[SSPRegistrationAuthority]FROM [SSP_Testing].dbo.[RegistrationAuthority]
--End TSP Registration Tables

--Start Criteria Template Tables
SELECT * INTO [SSP_Testing].dbo.[SSPCriteriaHeader]         FROM [SSP_Testing].dbo.[CriteriaHeader]
SELECT * INTO [SSP_Testing].dbo.[SSPCriteriaMainCategory]   FROM [SSP_Testing].dbo.[CriteriaMianCategory]
SELECT * INTO [SSP_Testing].dbo.[SSPCriteriaSubCategory]    FROM [SSP_Testing].dbo.[CriteriaSubCategory]
SELECT * INTO [SSP_Testing].dbo.[SSPProgramCriteriaHistory] FROM [SSP_Testing].dbo.[ProgramCriteriaHistory]
--End Criteria Tables

--Start Process WorkFlow Tables
SELECT * INTO [SSP_Testing].dbo.[SSPWorkflow]               FROM [SSP_Testing].dbo.[Workflow]
SELECT * INTO [SSP_Testing].dbo.[SSPWorkflowTask]           FROM [SSP_Testing].dbo.[WorkflowTask]
SELECT * INTO [SSP_Testing].dbo.[SSPProgramWorkflowHistory] FROM [SSP_Testing].dbo.[ProgramWorkflowHistory]
--End Process WorkFlow Tables

--Start Process Configuration Tables
SELECT * INTO [SSP_Testing].dbo.[SSPProcessSchedule]       FROM [SSP_Testing].dbo.[ProcessSchedule]
SELECT * INTO [SSP_Testing].dbo.[SSPProcessScheduleDetail] FROM [SSP_Testing].dbo.[ProcessScheduleDetail]
SELECT * INTO [SSP_Testing].dbo.[SSPProcessScheduleMaster] FROM [SSP_Testing].dbo.[ProcessScheduleMaster]
--End Process Configuration Tables

--Start Annual ProgramDesign Tables
SELECT * INTO [SSP_Testing].dbo.[SSPPlaningType]          FROM [SSP_Testing].dbo.[PlaningType]
SELECT * INTO [SSP_Testing].dbo.[SSPSelectionMethods]     FROM [SSP_Testing].dbo.[SelectionMethods]
SELECT * INTO [SSP_Testing].dbo.[SSPTraineeSupportItems]  FROM [SSP_Testing].dbo.[TraineeSupportItems]
SELECT * INTO [SSP_Testing].dbo.[SSPProgramDesign]        FROM [SSP_Testing].dbo.[ProgramDesign]
SELECT * INTO [SSP_Testing].dbo.[SSPProgramStatusHistory] FROM [SSP_Testing].dbo.[ProgramStatusHistory]
SELECT * INTO [SSP_Testing].dbo.[SSPTradeDesign]          FROM [SSP_Testing].dbo.[TradeDesign]
SELECT * INTO [SSP_Testing].dbo.[SSPTradeLot]             FROM [SSP_Testing].dbo.[TradeLot]
--End Annual ProgramDesign Tables


--Start  Association Tables
SELECT * INTO [SSP_Testing].dbo.[SSPTspAssociationMaster]         FROM [SSP_Testing].dbo.[TspAssociationMaster]
SELECT * INTO [SSP_Testing].dbo.[SSPTspAssociationDetail]         FROM [SSP_Testing].dbo.[TspAssociationDetail]
SELECT * INTO [SSP_Testing].dbo.[SSPTspAssociationEvaluation]     FROM [SSP_Testing].dbo.[TspAssociationEvaluation]
SELECT * INTO [SSP_Testing].dbo.[SSPTSPAssociationFee]            FROM [SSP_Testing].dbo.[TSPAssociationFee]
SELECT * INTO [SSP_Testing].dbo.[SSPTSPAssociationPaymentDetail]  FROM [SSP_Testing].dbo.[TSPAssociationPaymentDetail]
SELECT * INTO [SSP_Testing].dbo.[SSPTSPAssociationPaymentHistory] FROM [SSP_Testing].dbo.[TSPAssociationPaymentHistory]
SELECT * INTO [SSP_Testing].dbo.[SSPTSPAssignment]                FROM [SSP_Testing].dbo.[TSPAssignment]
--End   Association  Tables



--Start TSP Trainee Portal Tables
SELECT * INTO [SSP_Testing].dbo.[SSPTraineeIntrestProfile] FROM [SSP_Testing].dbo.[SSPTraineeIntrestProfile]
--End TSP Trainee Portal Tables

--RoleID Must be 23
INSERT INTO Roles (RoleTitle, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate)
	VALUES (N'SSPRegistration', 0, DEFAULT, DEFAULT, GETDATE(), GETDATE());


--SELECT * FROM Modules m
--SELECT * FROM  AppForms af

INSERT INTO Modules (ModuleTitle, modpath, SortOrder, UserLevel, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate)
VALUES (N'Profile Management', N'profile-manage', 1, DEFAULT, 0, 0, 0, GETDATE(), GETDATE());

INSERT AppForms (FormName, Path, Icon, Controller, ModuleID, Sortorder, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate, IsAddible, IsEditable, IsDeletable, IsViewable)
VALUES (N'Profile', N'profile', N'', N'BusinessProfile',(SELECT MAX(ModuleID) FROM Modules), 0, 0, NULL, 0, GETDATE(), NULL,1,1,1,1)
INSERT AppForms (FormName, Path, Icon, Controller, ModuleID, Sortorder, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate, IsAddible, IsEditable, IsDeletable, IsViewable)
VALUES (N'Base Data', N'base-data', N'', N'BusinessProfile',(SELECT MAX(ModuleID) FROM Modules), 0, 0, NULL, 0, GETDATE(), NULL,1,1,1,1)
INSERT AppForms (FormName, Path, Icon, Controller, ModuleID, Sortorder, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate, IsAddible, IsEditable, IsDeletable, IsViewable)
VALUES (N'Registration Evaluation', N'evaluation', N'', N'BusinessProfile',(SELECT MAX(ModuleID) FROM Modules), 0, 0, 0, 0, GETDATE(), GETDATE(),1,1,1,1)


INSERT INTO Modules (ModuleTitle, modpath, SortOrder, UserLevel, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate)
VALUES (N'Annual Plan', N'annual-planning', 1, DEFAULT, 0, 0, 0, GETDATE(), GETDATE());
INSERT AppForms (FormName, Path, Icon, Controller, ModuleID, Sortorder, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate, IsAddible, IsEditable, IsDeletable, IsViewable)
VALUES (N'Calculate CTM', N'calculate-ctm', N'', N'AnnualPlan', (SELECT MAX(ModuleID) FROM Modules), 0, 0, 0, 0, GETDATE(), GETDATE(),1,1,1,1)
INSERT AppForms (FormName, Path, Icon, Controller, ModuleID, Sortorder, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate, IsAddible, IsEditable, IsDeletable, IsViewable)
VALUES (N'Program Plan', N'program-plan', N'', N'AnnualPlan', (SELECT MAX(ModuleID) FROM Modules), 0, 0, 0, 0, GETDATE(), GETDATE(),1,1,1,1)
INSERT AppForms (FormName, Path, Icon, Controller, ModuleID, Sortorder, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate, IsAddible, IsEditable, IsDeletable, IsViewable)
VALUES (N'Trade Plan', N'trade-plan', N'', N'AnnualPlan', (SELECT MAX(ModuleID) FROM Modules), 0, 0, 0, 0, GETDATE(), GETDATE(),1,1,1,1)
INSERT AppForms (FormName, Path, Icon, Controller, ModuleID, Sortorder, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate, IsAddible, IsEditable, IsDeletable, IsViewable)
VALUES (N'Program Initiate', N'program-initiate', N'', N'AnnualPlan', (SELECT MAX(ModuleID) FROM Modules), 0, 0, 0, 0, GETDATE(), GETDATE(),1,1,1,1)
INSERT AppForms (FormName, Path, Icon, Controller, ModuleID, Sortorder, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate, IsAddible, IsEditable, IsDeletable, IsViewable)
VALUES (N'Validate Business Plan', N'validate-business-plan', N'', N'AnnualPlan', (SELECT MAX(ModuleID) FROM Modules), 0, 0, 0, 0, GETDATE(), GETDATE(),1,1,1,1)
INSERT AppForms (FormName, Path, Icon, Controller, ModuleID, Sortorder, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate, IsAddible, IsEditable, IsDeletable, IsViewable)
VALUES (N'Process Approved Plan', N'process-approved-plan', N'', N'AnnualPlan', (SELECT MAX(ModuleID) FROM Modules), 0, 0, 0, 0, GETDATE(), GETDATE(),1,1,1,1)
INSERT AppForms (FormName, Path, Icon, Controller, ModuleID, Sortorder, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate, IsAddible, IsEditable, IsDeletable, IsViewable)
VALUES (N'Process Status Update', N'process-status-update', N'', N'AnnualPlan', (SELECT MAX(ModuleID) FROM Modules), 0, 0, 0, 0, GETDATE(), GETDATE(),1,1,1,1)
INSERT AppForms (FormName, Path, Icon, Controller, ModuleID, Sortorder, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate, IsAddible, IsEditable, IsDeletable, IsViewable)
VALUES (N'Analysis | Historical Report', N'historical-report', N'', N'ProgramDesign', (SELECT MAX(ModuleID) FROM Modules), 0, 0, 0, 0, GETDATE(), GETDATE(),1,1,1,1)
INSERT AppForms (FormName, Path, Icon, Controller, ModuleID, Sortorder, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate, IsAddible, IsEditable, IsDeletable, IsViewable)
VALUES (N'Analysis Report', N'registration-analysis-report', N'', N'AnnualPlan', (SELECT MAX(ModuleID) FROM Modules), 0, 0, 0, 0, GETDATE(), GETDATE(),1,1,1,1)





INSERT INTO Modules (ModuleTitle, modpath, SortOrder, UserLevel, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate)
VALUES (N'Association Management', N'association-management', 2, DEFAULT, 0, 0, 0, GETDATE(), GETDATE());
INSERT AppForms (FormName, Path, Icon, Controller, ModuleID, Sortorder, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate, IsAddible, IsEditable, IsDeletable, IsViewable)
VALUES (N'Initiate Association', N'initiate-association', N'', N'Association', (SELECT MAX(ModuleID) FROM Modules), 0, 0, 0, 0, GETDATE(), GETDATE(),1,1,1,1)
INSERT AppForms (FormName, Path, Icon, Controller, ModuleID, Sortorder, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate, IsAddible, IsEditable, IsDeletable, IsViewable)
VALUES (N'Association Evaluation', N'association-evaluation', N'', N'Association', (SELECT MAX(ModuleID) FROM Modules), 0, 0, 0, 0, GETDATE(), GETDATE(),1,1,1,1)
INSERT AppForms (FormName, Path, Icon, Controller, ModuleID, Sortorder, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate, IsAddible, IsEditable, IsDeletable, IsViewable)
VALUES (N'TSP Assignment', N'tsp-assignment', N'', N'Association', (SELECT MAX(ModuleID) FROM Modules), 0, 0, 0, 0, GETDATE(), GETDATE(),1,1,1,1)
INSERT AppForms (FormName, Path, Icon, Controller, ModuleID, Sortorder, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate, IsAddible, IsEditable, IsDeletable, IsViewable)
VALUES (N'Association Submission', N'association-submission', N'', N'Association', (SELECT MAX(ModuleID) FROM Modules), 0, 0, 0, 0, GETDATE(), GETDATE(),1,1,1,1)
INSERT AppForms (FormName, Path, Icon, Controller, ModuleID, Sortorder, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate, IsAddible, IsEditable, IsDeletable, IsViewable)
VALUES (N'Association Detail', N'association-detail', N'', N'Association', (SELECT MAX(ModuleID) FROM Modules), 0, 0, 0, 0, GETDATE(), GETDATE(),1,1,1,1)


INSERT INTO Modules (ModuleTitle, modpath, SortOrder, UserLevel, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate)
VALUES (N'Criteria Management', N'criteria-management', 2, DEFAULT, 0, 0, 0, GETDATE(), GETDATE());
INSERT AppForms (FormName, Path, Icon, Controller, ModuleID, Sortorder, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate, IsAddible, IsEditable, IsDeletable, IsViewable)
VALUES (N'Criteria Template', N'criteria-template', N'', N'CriteriaTemplate', (SELECT MAX(ModuleID) FROM Modules), 0, 0, 0, 0, GETDATE(), GETDATE(),1,1,1,1)

INSERT INTO Modules (ModuleTitle, modpath, SortOrder, UserLevel, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate)
VALUES (N'Workflow Management', N'workflow-management', 2, DEFAULT, 0, 0, 0, GETDATE(), GETDATE());
INSERT AppForms (FormName, Path, Icon, Controller, ModuleID, Sortorder, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate, IsAddible, IsEditable, IsDeletable, IsViewable)
VALUES (N'Workflow', N'workflow', N'', N'WorkFlow', (SELECT MAX(ModuleID) FROM Modules), 0, 0, 0, 0, GETDATE(), GETDATE(),1,1,1,1)



INSERT INTO Modules (ModuleTitle, modpath, SortOrder, UserLevel, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate)
VALUES (N'Payment', N'payment', 2, DEFAULT, 0, 0, 0, GETDATE(), GETDATE());
INSERT AppForms (FormName, Path, Icon, Controller, ModuleID, Sortorder, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate, IsAddible, IsEditable, IsDeletable, IsViewable)
VALUES (N'Registration Payment', N'registration-payment', N'', N'Payment', (SELECT MAX(ModuleID) FROM Modules), 0, 0, 0, 0, GETDATE(), GETDATE(),1,1,1,1)
INSERT AppForms (FormName, Path, Icon, Controller, ModuleID, Sortorder, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate, IsAddible, IsEditable, IsDeletable, IsViewable)
VALUES (N'Association Payment', N'association-payment', N'', N'Payment', (SELECT MAX(ModuleID) FROM Modules), 0, 0, 0, 0, GETDATE(), GETDATE(),1,1,1,1)

INSERT INTO Modules (ModuleTitle, modpath, SortOrder, UserLevel, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate)
VALUES (N'Process Configuration', N'process-configuration', 2, DEFAULT, 0, 0, 0, GETDATE(), GETDATE());
INSERT AppForms (FormName, Path, Icon, Controller, ModuleID, Sortorder, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate, IsAddible, IsEditable, IsDeletable, IsViewable)
VALUES (N'Process Schedule', N'process-schedule', N'', N'ProcessConfiguration', (SELECT MAX(ModuleID) FROM Modules), 0, 0, 0, 0, GETDATE(), GETDATE(),1,1,1,1)



INSERT INTO Modules (ModuleTitle, modpath, SortOrder, UserLevel, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate)
VALUES (N'TSP Trainee Portal', N'tsp-trainee-portal', 2, DEFAULT, 0, 0, 0, GETDATE(), GETDATE());
INSERT AppForms (FormName, Path, Icon, Controller, ModuleID, Sortorder, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate, IsAddible, IsEditable, IsDeletable, IsViewable)
VALUES (N'TSP Trainee Portal', N'tsp-trainee-portal', N'', N'TSPTraineePortal', (SELECT MAX(ModuleID) FROM Modules), 0, 0, 0, 0, GETDATE(), GETDATE(),1,1,1,1)


INSERT AppForms (FormName, Path, Icon, Controller, ModuleID, Sortorder, InActive, CreatedUserID, ModifiedUserID, CreatedDate, ModifiedDate, IsAddible, IsEditable, IsDeletable, IsViewable)
VALUES (N'TSP Registration', N'registration-approval', N'', N'Approval', 5, 0, 0, 0, 0, GETDATE(), GETDATE(),1,1,1,1)
