/* ****Aamer Rehman Malik *****/

using DataLayer;
using DataLayer.Dapper;
using DataLayer.Interfaces;
using DataLayer.JobScheduler.Jobs;
using DataLayer.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using PSDF_BSS.Logging.Interface;
using PSDF_BSS.Logging.Service;
using System.Text;

namespace PSDF_BSS
{
    public static class IServiceSinglton
    {
        public static IServiceCollection AddSingletonClasses(this IServiceCollection services)
        {
            services.AddSingleton(typeof(ISRVUsers), typeof(SRVUsers));
            services.AddSingleton(typeof(ISRVAppForms), typeof(SRVAppForms));
            services.AddSingleton(typeof(ISRVRoles), typeof(SRVRoles));
            services.AddSingleton(typeof(ISRVOrganization), typeof(SRVOrganization));
            services.AddSingleton(typeof(ISRVTehsil), typeof(SRVTehsil));
            services.AddSingleton(typeof(ISRVGender), typeof(SRVGender));
            services.AddSingleton(typeof(ISRVEducationTypes), typeof(SRVEducationTypes));
            services.AddSingleton(typeof(ISRVCluster), typeof(SRVCluster));
            services.AddSingleton(typeof(ISRVAcademicDiscipline), typeof(SRVAcademicDiscipline));
            services.AddSingleton(typeof(ISRVSector), typeof(SRVSector));
            services.AddSingleton(typeof(ISRVRegion), typeof(SRVRegion));
            services.AddSingleton(typeof(ISRVReligion), typeof(SRVReligion));
            services.AddSingleton(typeof(ISRVTrade), typeof(SRVTrade));
            services.AddSingleton(typeof(ISRVTraineeDisability), typeof(SRVTraineeDisability));
            services.AddSingleton(typeof(ISRVDistrict), typeof(SRVDistrict));
            services.AddSingleton(typeof(ISRVSubSector), typeof(SRVSubSector));
            services.AddSingleton(typeof(ISRVCustomFinancialYear), typeof(SRVCustomFinancialYear));
            services.AddSingleton(typeof(ISRVProgramType), typeof(SRVProgramType));
            services.AddSingleton(typeof(ISRVTraineeStatusTypes), typeof(SRVTraineeStatusTypes));
            services.AddSingleton(typeof(ISRVClassStatusType), typeof(SRVClassStatusType));
            services.AddSingleton(typeof(ISRVClassStatus), typeof(SRVClassStatus));
            services.AddSingleton(typeof(ISRVCertificationCategory), typeof(SRVCertificationCategory));
            services.AddSingleton(typeof(ISRVCertificationAuthority), typeof(SRVCertificationAuthority));
            services.AddSingleton(typeof(ISRVTraineeResultStatusTypes), typeof(SRVTraineeResultStatusTypes));
            services.AddSingleton(typeof(ISRVTestingAgency), typeof(SRVTestingAgency));
            services.AddSingleton(typeof(ISRVTraineeProfile), typeof(SRVTraineeProfile));
            services.AddSingleton(typeof(ISRVFundingSource), typeof(SRVFundingSource));
            services.AddSingleton(typeof(ISRVFundingCategory), typeof(SRVFundingCategory));
            services.AddSingleton(typeof(ISRVProgramCategory), typeof(SRVProgramCategory));
            services.AddSingleton(typeof(ISRVYearWiseInflationRate), typeof(SRVYearWiseInflationRate));
            services.AddSingleton(typeof(ISRVNotifications), typeof(SRVNotifications));
            services.AddSingleton(typeof(ISRVStipendStatus), typeof(SRVStipendStatus));
            services.AddSingleton(typeof(ISRVEmploymentStatus), typeof(SRVEmploymentStatus));
            services.AddSingleton(typeof(ISRVTier), typeof(SRVTier));
            services.AddSingleton(typeof(ISRVTSPDetail), typeof(SRVTSPDetail));
            services.AddSingleton(typeof(ISRVApproval), typeof(SRVApproval));
            services.AddSingleton(typeof(ISRVApprovalProcess), typeof(SRVApprovalProcess));
            services.AddSingleton(typeof(ISRVClass), typeof(SRVClass));
            services.AddSingleton(typeof(ISRVInstructor), typeof(SRVInstructor));
            services.AddSingleton(typeof(ISRVInceptionReport), typeof(SRVInceptionReport));
            services.AddSingleton(typeof(ISRVTSPMaster), typeof(SRVTSPMaster));
            services.AddSingleton(typeof(ISRVContactPerson), typeof(SRVContactPerson));
            services.AddSingleton(typeof(ISRVInstructorMaster), typeof(SRVInstructorMaster));
            services.AddSingleton(typeof(ISRVOrgConfig), typeof(SRVOrgConfig));
            services.AddSingleton(typeof(ISRVClassSections), typeof(SRVClassSections));
            services.AddSingleton(typeof(ISRVSections), typeof(SRVSections));
            services.AddSingleton(typeof(ISRVKAMAssignment), typeof(SRVKAMAssignment));
            services.AddSingleton(typeof(ISRVTraineeStatus), typeof(SRVTraineeStatus));
            services.AddSingleton(typeof(ISRVMasterSheet), typeof(SRVMasterSheet));
            services.AddSingleton(typeof(ISRVVisitPlan), typeof(SRVVisitPlan));
            services.AddSingleton(typeof(ISRVTraineeAttendance), typeof(SRVTraineeAttendance));
            services.AddSingleton(typeof(INLogManager), typeof(NLogManager));
            services.AddSingleton(typeof(ISRVTSRLiveData), typeof(SRVTSRLiveData));
            services.AddSingleton(typeof(ISRVHomeStats), typeof(SRVHomeStats));
            services.AddSingleton(typeof(ISRVUsersRights), typeof(SRVUsersRights));
            services.AddSingleton(typeof(ISRVUserOrganizations), typeof(SRVUserOrganizations));
            services.AddSingleton(typeof(ISRVPurchaseOrder), typeof(SRVPurchaseOrder));
            services.AddSingleton(typeof(ISRVPOLines), typeof(SRVPOLines));
            services.AddSingleton(typeof(ISRVPOHeader), typeof(SRVPOHeader));
            services.AddSingleton(typeof(ISRVSRN), typeof(SRVSRN));
            services.AddSingleton(typeof(ISRVSRNDetails), typeof(SRVSRNDetails));
            services.AddSingleton(typeof(ISRVGURN), typeof(SRVGURN));
            services.AddSingleton(typeof(ISRVGURNDetails), typeof(SRVGURNDetails));
            services.AddSingleton(typeof(ISRVEquipmentTools), typeof(SRVEquipmentTools));
            services.AddSingleton(typeof(ISRVConsumableMaterial), typeof(SRVConsumableMaterial));
            services.AddSingleton(typeof(ISRVSourceOfCurriculum), typeof(SRVSourceOfCurriculum));
            services.AddSingleton(typeof(ISRVApprovalHistory), typeof(SRVApprovalHistory));
            services.AddSingleton(typeof(ISRVBenchmarking), typeof(SRVBenchmarking));
            services.AddSingleton(typeof(ISRVClassInvoiceMap), typeof(SRVClassInvoiceMap));
            services.AddSingleton(typeof(ISRVUserEventMap), typeof(SRVUserEventMap));
            services.AddSingleton(typeof(ISRVClassEventMap), typeof(SRVClassEventMap));
            services.AddSingleton(typeof(ISRVDuration), typeof(SRVDuration));
            services.AddSingleton(typeof(ISRVTradeDetailMap), typeof(SRVTradeDetailMap));
            services.AddSingleton(typeof(ISRVTradeEquipmentToolsMap), typeof(SRVTradeEquipmentToolsMap));
            services.AddSingleton(typeof(ISRVTradeConsumableMaterialMap), typeof(SRVTradeConsumableMaterialMap));
            services.AddSingleton(typeof(ISRVTradeSourceOfCurriculumMap), typeof(SRVTradeSourceOfCurriculumMap));
            services.AddSingleton(typeof(ISRVPBTE), typeof(SRVPBTE));
            services.AddSingleton(typeof(ISRVMPR), typeof(SRVMPR));
            services.AddSingleton(typeof(ISRVMPRTraineeDetail), typeof(SRVMPRTraineeDetail));
            services.AddSingleton(typeof(ISRVTSPEmployment), typeof(SRVTSPEmployment));
            services.AddSingleton(typeof(ISRVPlacementType), typeof(SRVPlacementType));
            services.AddSingleton(typeof(ISRVROSI), typeof(SRVROSI));
            services.AddSingleton(typeof(ISRVEmploymentVerification), typeof(SRVEmploymentVerification));
            services.AddSingleton(typeof(ISRVIncomeRange), typeof(SRVIncomeRange));
            services.AddSingleton(typeof(ISRVRTP), typeof(SRVRTP));
            services.AddSingleton(typeof(ISRVPRN), typeof(SRVPRN));
            services.AddSingleton(typeof(ISRVGRN), typeof(SRVGRN));
            services.AddSingleton(typeof(ISRVInvoice), typeof(SRVInvoice));
            services.AddSingleton(typeof(ISRVInvoiceMaster), typeof(SRVInvoiceMaster));
            services.AddSingleton(typeof(ISRVUser_Pwd), typeof(SRVUser_Pwd));
            services.AddSingleton(typeof(ISRVPRNMaster), typeof(SRVPRNMaster));
            services.AddSingleton(typeof(ISRVSAPApi), typeof(SRVSAPApi));
            services.AddSingleton(typeof(ISRVInfrastructure), typeof(SRVInfrastructure));
            services.AddSingleton(typeof(ISRVVerificationMethod), typeof(SRVVerificationMethod));
            services.AddSingleton(typeof(IDapperConfig), typeof(DapperConfig));
            services.AddSingleton(typeof(ISRVTrn), typeof(SRVTrn));
            services.AddSingleton(typeof(ISRVReferralSource), typeof(SRVReferralSource));
            services.AddSingleton(typeof(ISRVPBTEDataSharingTimelines), typeof(SRVPBTEDataSharingTimelines));
            services.AddSingleton(typeof(ISRVCenters), typeof(SRVCenters));
            services.AddSingleton(typeof(ISRVClassInvoiceExtMap), typeof(SRVClassInvoiceMapExt));
            services.AddSingleton(typeof(ISRVGenerateInvoice), typeof(SRVGenerateInvoice));
            services.AddSingleton(typeof(ISRVPOSummary), typeof(SRVPOSummary));
            services.AddSingleton(typeof(ISRVModules), typeof(SRVModules));
            services.AddSingleton(typeof(ISRVPaymentSchedule), typeof(SRVPaymentSchedule));
            services.AddSingleton(typeof(ISRVProvinces), typeof(SRVProvinces));
            services.AddSingleton(typeof(ISRVSAPBranches), typeof(SRVSAPBranches));
            services.AddSingleton(typeof(ISRVTSPDetailSchemeMap), typeof(SRVTSPDetailSchemeMap));
            services.AddSingleton(typeof(ISRVUserNotificationMap), typeof(SRVUserNotificationMap));
            services.AddSingleton(typeof(ISRVScheme), typeof(SRVScheme));
            services.AddSingleton(typeof(ISRVSendEmail), typeof(SRVSendEmail));
            services.AddSingleton(typeof(ISRVSchemeChangeRequest), typeof(SRVSchemeChangeRequest));
            services.AddSingleton(typeof(ISRVTSPChangeRequest), typeof(SRVTSPChangeRequest));
            services.AddSingleton(typeof(ISRVClassChangeRequest), typeof(SRVClassChangeRequest));
            services.AddSingleton(typeof(ISRVTraineeChangeRequest), typeof(SRVTraineeChangeRequest));
            services.AddSingleton(typeof(ISRVInstructorChangeRequest), typeof(SRVInstructorChangeRequest));
            services.AddSingleton(typeof(ISRVInceptionReportChangeRequest), typeof(SRVInceptionReportChangeRequest));
            services.AddSingleton(typeof(ISRVInstructorReplaceChangeRequest), typeof(SRVInstructorReplaceChangeRequest));
            services.AddSingleton(typeof(ISRVConfirmedMarginal), typeof(SRVConfirmedMarginal));
            services.AddSingleton(typeof(ISRVViolationSummary), typeof(SRVViolationSummary));
            services.AddSingleton(typeof(ISRVDeletedDropout), typeof(SRVDeletedDropout));
            services.AddSingleton(typeof(ISRVAttendancePerception), typeof(SRVAttendancePerception));
            services.AddSingleton(typeof(ISRVAMSReportService), typeof(SRVAMSReportService));
            services.AddSingleton(typeof(ISRVReportExecutiveSummary), typeof(SRVReportExecutiveSummary));
            services.AddSingleton(typeof(ISRVAdvanceSearch), typeof(SRVAAdvanceSearch));
            services.AddSingleton(typeof(ISRVAdditionalTrainees), typeof(SRVAdditionalTrainees));
            services.AddSingleton(typeof(ISRVFakeGhostTrainee), typeof(SRVFakeGhostTrainee));
            services.AddSingleton(typeof(ISRVCovidMaskViolation), typeof(SRVCovidMaskViolation));
            services.AddSingleton(typeof(ISRVPSPEmployment), typeof(SRVPSPEmployment));
            services.AddSingleton(typeof(ISRVEmploymentVerificationReport), typeof(SRVEmploymentVerificationReport));
            services.AddSingleton(typeof(ISRVComplaint), typeof(SRVComplaint));
            services.AddSingleton(typeof(ISRVTSPColor), typeof(SRVTSPColor));
            services.AddSingleton(typeof(ISRVReports), typeof(SRVReports));
            services.AddSingleton(typeof(ISRVDashboard), typeof(SRVDashboard));
            services.AddSingleton(typeof(ISRVComplaintUser), typeof(SRVComplaintUser));
            services.AddSingleton(typeof(ISRVNotificationMap), typeof(SRVNotificationMap));
            services.AddSingleton(typeof(INotificationsHub), typeof(NotificationsHub));
            services.AddSingleton(typeof(ISRVKAMDashboard), typeof(SRVKAMDashboard));
            services.AddSingleton(typeof(ISRVGIS), typeof(SRVGIS));
            services.AddSingleton(typeof(ISRVSrnDisbursementStatus), typeof(SRVSrnDisbursementStatus));
            services.AddSingleton(typeof(ISRVSkillsScholarshipInitiative), typeof(SRVSkillsScholarshipInitiative));
            services.AddSingleton(typeof(ISRVNotificationDetail), typeof(SRVNotificationDetail));
            services.AddSingleton(typeof(IJobSendEmail), typeof(JobSendEmail));
            services.AddSingleton(typeof(ISRVPotentialTrainees), typeof(SRVPotentialTrainees));
            //services.AddSingletonClassesDataLayer();
            services.AddSingleton(typeof(ISRVBSSReports), typeof(SRVBSSReports));
            //DVV Services
            services.AddSingleton(typeof(ISRVDeviceManagement), typeof(SRVDeviceManagement));
            services.AddSingleton(typeof(ISRVIPDocsVerification), typeof(SRVIPDocsVerification));


            //SSP Service Include
            services.AddSingleton(typeof(ISRVBusinessProfile), typeof(SRVBusinessProfile));
            services.AddSingleton(typeof(ISRVBaseData), typeof(SRVBaseData));
            services.AddSingleton(typeof(ISRVProgramDesign), typeof(SRVProgramDesign));
            services.AddSingleton(typeof(ISRVCriteriaTemplate), typeof(SRVCriteriaTemplate));
            services.AddSingleton(typeof(ISRVWorkflow), typeof(SRVWorkflow));
            services.AddSingleton(typeof(ISRVAssociation), typeof(SRVAssociation));
            services.AddSingleton(typeof(ISRVPayment), typeof(SRVPayment));
            services.AddSingleton(typeof(ISRVProcessConfiguration), typeof(SRVProcessConfiguration));

            //SRN Coursera --Added by  samiullah 17-01-2025
            services.AddSingleton(typeof(ISRVTPRN), typeof(SRVTPRN));
            services.AddSingleton(typeof(ISRVPVRN), typeof(SRVPVRN));
            services.AddSingleton(typeof(ISRVMRN), typeof(SRVMRN));

            services.AddSingleton(typeof(ISRVPCRN), typeof(SRVPCRN));
            services.AddSingleton(typeof(ISRVPCRNDetails), typeof(SRVPCRNDetails));

            services.AddSingleton(typeof(ISRVOTRN), typeof(SRVOTRN));
            services.AddSingleton(typeof(ISRVOTRNDetails), typeof(SRVOTRNDetails));

            services.AddSingleton(typeof(ISRVTPRNDetails), typeof(SRVTPRNDetails));
            services.AddSingleton(typeof(ISRVPVRNDetails), typeof(SRVPVRNDetails));
            services.AddSingleton(typeof(ISRVMRNDetails), typeof(SRVMRNDetails));
            services.AddSingleton(typeof(ISRVManualNoteGeneration), typeof(SRVManualNoteGeneration));

            //SRN Coursera --Added by  Rao Ali Haider 04-July-2024
            services.AddSingleton(typeof(ISRVSRNCoursera), typeof(SRVSRNCoursera));
            services.AddSingleton(typeof(ISRVTraineeGuruProfile), typeof(SRVTraineeGuruProfile));


            //SRN Coursera --Added by  Umair Nadeem 17-Feb-2025
            services.AddSingleton(typeof(ISRVMobileAppDownload), typeof(SRVMobileAppDownload));


            return services;
        }

        public static IServiceCollection AddMyCors(this IServiceCollection services, string MyAllowSpecificOrigins = "_myAllowSpecificOrigins", string CorsAllowOrigins = "*")
        {
            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,
                builder =>
                {
                    builder.WithOrigins(CorsAllowOrigins)
                                        .AllowAnyHeader()
                                        .AllowAnyMethod();
                });
            });
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            }
            );
            return services;
        }

        public static IServiceCollection AddMyAuthentication(this IServiceCollection services, string key)
        {
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
            return services;
        }
    }
}