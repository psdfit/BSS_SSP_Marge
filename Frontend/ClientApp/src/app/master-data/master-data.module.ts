/* **** Aamer Rehman Malik *****/
import { CommonModule } from '@angular/common';
import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { SharedModule } from '../shared/shared.module';
import { AcademicDisciplineComponent } from './academic-discipline/academic-discipline.component';
import { AppFormsComponent } from './appforms/app-forms.component';
import { ApprovalComponent } from './approval/approval.component';
import { CenterComponent } from './centres/centers.component';
import { CertificationAuthorityComponent } from './certification-authority/certification-authority.component';
import { CertificationCategoryComponent } from './certification-category/certification-category.component';
import { ClassSectionsComponent } from './class-sections/class-sections.component';
import { ClassStatusComponent } from './classStatus/classStatus.component';
import { ClusterComponent } from './clusters/cluster.component';
import { ConsumableMaterialComponent } from './consumable-material/consumable-material.component';
import { CustomFinancialYearComponent } from './custom-financial-year/custom-financial-year.component';
import { DistrictComponent } from './district/district.component';
import { DurationComponent } from './duration/duration.component';
import { EducationTypeComponent } from './education-type/education-type.component';
import { EmploymentStatusComponent } from './employment-status/employment-status.component';
import { EquipmentToolComponent } from './equipment-tool/equipment-tool.component';
import { FundingCategoryComponent } from './funding-category/funding-category.component';
import { FundingSourceComponent } from './funding-source/funding-source.component';
import { GenderComponent } from './genders/gender.component';
import { InfrastructureComponent } from './infrastructure/infrastructure.component';
import { KAMComponent } from './KAM/KAM.component';
import { KAMInformationDialogComponent } from './kam-information-dialog/kam-information-dialog.component';
import { MasterDataRoutingModule } from './master-data-routing';
import { NotificationsComponent } from './notifications/notifications.component';
import { OrgConfigComponent } from './orgconfig/orgconfig.component';
import { OrgnizationsComponent } from './orgnizations/orgnizations.component';
import { TSPColorChangedialogueComponent } from './TSPColorChange-dialogue/TSPColorChange-dialogue.component';
import { tspcolorComponent } from './tspcolor/tspcolor.component';
import { TSPColorHistorydialogueComponent } from './TSPColorHistory-dialogue/TSPColorHistory-dialogue.component';
import { rolerightsdialogueComponent } from './rolerights-dialogue/rolerights-dialogue.component';
import { PrnApprovalPenaltyComponent } from './prn-approval-penalty/prn-approval-penalty.component';
// import { PBTEDataSharingTimelinesComponent } from "./pbte-datasharing-timelines/pbte-datasharing-timelines.component";NotificationDetaildialoguecomponent
// import { ProgramCategoryComponent } from "./program-category/program-category.component";
// import { ProgramTypeComponent } from "./programtype/programtype.component";
// import { RegionComponent } from "./regions/region.component";
// import { ReligionComponent } from "./religion/religion.component";
// import { ResultStatusComponent } from "./resultstatus/resultstatus.component";
//// import { ChangePasswordComponent } from './change-password/change-password.component';
// import { RolesComponent } from "./roles/roles.component";
// import { SapBranchesComponent } from "./sap-branches/sap-branches.component";
// import { SectorComponent } from "./sectors/sector.component";
// import { SourceOfCurriculumComponent } from "./source-of-curriculum/source-of-curriculum.component";
// import { StipendStatusComponent } from "./stipendstatus/stipendstatus.component";
// import { SubSectorComponent } from "./subsector/subsector.component";
// import { TehsilComponent } from "./tehsil/tehsil.component";
// import { TestingAgencyComponent } from "./testing-agency/testing-agency.component";
// import { TierComponent } from "./tier/tier.component";
// import { TradeComponent } from "./trade/trade.component";
// import { TraineeDisabilityComponent } from "./trainee-disability/trainee-disability.component";
// import { TraineeStatusComponent } from "./traineestatus/traineestatus.component";
// import { TSPListComponent } from "./tsplist/tsplist.component";
// import { UsersComponent } from "./users/users.component";
// import { InflationRateComponent } from "./year-wise-inflation-rate/year-wise-inflation-rate.component";

@NgModule({
  declarations: [
    AppFormsComponent,
    // UsersComponent,
    // RolesComponent,
    OrgnizationsComponent,
    GenderComponent,
    // TraineeDisabilityComponent,
    ClusterComponent,
    // RegionComponent,
    // ReligionComponent,
    DistrictComponent,
    // TehsilComponent,
    OrgConfigComponent,
    // SectorComponent,
    EquipmentToolComponent,
    // SourceOfCurriculumComponent,
    ConsumableMaterialComponent,
    CustomFinancialYearComponent,
    // prnApproelPenaltyComponent,
    // SubSectorComponent,
    // ProgramTypeComponent,
    // StipendStatusComponent,
    // TraineeStatusComponent,
    // ResultStatusComponent,
    ClassStatusComponent,
    EducationTypeComponent,
    CertificationCategoryComponent,
    // TestingAgencyComponent,
    // TradeComponent,
    FundingSourceComponent,
    FundingCategoryComponent,
    // ProgramCategoryComponent,
    // InflationRateComponent,
    NotificationsComponent,
    EmploymentStatusComponent,
    // TierComponent,
    ApprovalComponent,
    rolerightsdialogueComponent,
    // TSPListComponent,
    ClassSectionsComponent,
    KAMComponent,
    KAMInformationDialogComponent,
    CertificationAuthorityComponent,
    DurationComponent,
    AcademicDisciplineComponent,
    // PBTEDataSharingTimelinesComponent,
    // SubSectorComponent,
    // ProgramTypeComponent,
    // StipendStatusComponent,
    // TraineeStatusComponent,
    // ResultStatusComponent,
    ClassStatusComponent,
    EducationTypeComponent,
    CertificationCategoryComponent,
    // TestingAgencyComponent,
    // TradeComponent,
    FundingSourceComponent,
    FundingCategoryComponent,
    // ProgramCategoryComponent,
    // InflationRateComponent,
    NotificationsComponent,
    EmploymentStatusComponent,
    // TierComponent,
    ApprovalComponent,
    // TSPListComponent,
    ClassSectionsComponent,
    KAMComponent,
    CertificationAuthorityComponent,
    DurationComponent,
    // SapBranchesComponent,
    // SubSectorComponent,
    // ProgramTypeComponent,
    // StipendStatusComponent,
    // TraineeStatusComponent,
    // ResultStatusComponent,
    ClassStatusComponent,
    EducationTypeComponent,
    CertificationCategoryComponent,
    // TestingAgencyComponent,
    // TradeComponent,
    FundingSourceComponent,
    FundingCategoryComponent,
    // ProgramCategoryComponent,
    // InflationRateComponent,
    NotificationsComponent,
    EmploymentStatusComponent,
    // TierComponent,
    ApprovalComponent,
    // TSPListComponent,
    ClassSectionsComponent,
    KAMComponent,
    CertificationAuthorityComponent,
    DurationComponent,
    AcademicDisciplineComponent,
    // PBTEDataSharingTimelinesComponent,
    //    SapBranchesComponent,
    CenterComponent,
    InfrastructureComponent,
    tspcolorComponent,
    TSPColorChangedialogueComponent,
    TSPColorHistorydialogueComponent,
    PrnApprovalPenaltyComponent
  ],
  imports: [CommonModule, SharedModule, MasterDataRoutingModule],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
})
export class MasterDataModuleModule {}
