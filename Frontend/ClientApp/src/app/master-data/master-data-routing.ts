/* **** Aamer Rehman Malik *****/
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuardService } from '../../app/security/auth-guard.service';
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
import { KAMComponent } from './KAM/KAM.component';
import { NotificationsComponent } from './notifications/notifications.component';
import { OrgConfigComponent } from './orgconfig/orgconfig.component';
import { OrgnizationsComponent } from './orgnizations/orgnizations.component';
import { AcademicDisciplineComponent } from './academic-discipline/academic-discipline.component';
import { InfrastructureComponent } from './infrastructure/infrastructure.component';
import { tspcolorComponent } from './tspcolor/tspcolor.component';
// import { PBTEDataSharingTimelinesComponent } from "./pbte-datasharing-timelines/pbte-datasharing-timelines.component";
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

// import { YourInfoComponent } from './your-info/your-info.component';

const routes: Routes = [
  // {
  //  path: "users",
  //  component: UsersComponent,
  //  canActivate: [AuthGuardService],
  //  data: { icon: "verified_user", inMenu: true, title: "Manage Users" },
  // },
  {
    path: 'appforms',
    component: AppFormsComponent,
    data: { icon: 'verified_user', inMenu: true, title: 'Manage App Forms' },
  },
  // {
  //  path: "roles",
  //  component: RolesComponent,
  //  canActivate: [AuthGuardService],
  //  data: {
  //    icon: "supervised_user_circle",
  //    inMenu: true,
  //    title: "Manage Roles",
  //  },
  // },
  {
    path: 'organizations',
    component: OrgnizationsComponent,
    canActivate: [AuthGuardService],
    data: { icon: 'verified_user', inMenu: true, title: 'Manage Orgnizations' },
  },
  {
    path: 'genders',
    component: GenderComponent,
    canActivate: [AuthGuardService],
    data: { icon: 'verified_user', inMenu: true, title: 'Genders' },
  },
  {
    path: 'duration',
    component: DurationComponent,
    canActivate: [AuthGuardService],
    data: { icon: 'verified_user', inMenu: true, title: 'Duration' },
  },
  {
    path: 'KAM',
    component: KAMComponent,
    canActivate: [AuthGuardService],
    data: { icon: 'verified_user', inMenu: true, title: 'KAM' },
  },
  // {
  //  path: "source-of-curriculum",
  //  component: SourceOfCurriculumComponent,
  //  canActivate: [AuthGuardService],
  //  data: {
  //    icon: "verified_user",
  //    inMenu: true,
  //    title: "Source Of Curriculum",
  //  },
  // },
  {
    path: 'equipment-tool',
    component: EquipmentToolComponent,
    canActivate: [AuthGuardService],
    data: { icon: 'verified_user', inMenu: true, title: 'Equipment Tool' },
  },
  {
    path: 'consumable-material',
    component: ConsumableMaterialComponent,
    canActivate: [AuthGuardService],
    data: { icon: 'verified_user', inMenu: true, title: 'Consumable Material' },
  },
  {
    path: 'class-sections',
    component: ClassSectionsComponent,
    canActivate: [AuthGuardService],
    data: { icon: 'verified_user', inMenu: true, title: 'Class Sections' },
  },
  // {
  //  path: "pbte-datasharing-timelines",
  //  component: PBTEDataSharingTimelinesComponent,
  //  canActivate: [AuthGuardService],
  //  data: { icon: "verified_user", inMenu: true, title: "Class Sections" },
  // },
  {
    path: 'education-type',
    component: EducationTypeComponent,
    canActivate: [AuthGuardService],
    data: { icon: 'verified_user', inMenu: true, title: 'Education Type' },
  },
  {
    path: 'clusters',
    component: ClusterComponent,
    canActivate: [AuthGuardService],
    data: { icon: 'verified_user', inMenu: true, title: 'Clusters' },
  },
  {
    path: 'centers',
    component: CenterComponent,
    canActivate: [AuthGuardService],
    data: { icon: 'verified_user', inMenu: true, title: 'Centers' },
  },
  // {
  //  path: "trade",
  //  component: TradeComponent,
  //  canActivate: [AuthGuardService],
  //  data: { icon: "verified_user", inMenu: true, title: "Trades" },
  // },
  {
    path: 'approval',
    component: ApprovalComponent,
    canActivate: [AuthGuardService],
    data: { icon: 'verified_user', inMenu: true, title: 'Approval' },
  },
  // {
  //  path: "tier",
  //  component: TierComponent,
  //  canActivate: [AuthGuardService],
  //  data: { icon: "verified_user", inMenu: true, title: "Tier" },
  // },
  // {
  //  path: "tsplist",
  //  component: TSPListComponent,
  //  canActivate: [AuthGuardService],
  //  data: { icon: "verified_user", inMenu: true, title: "TSPList" },
  // },
  // {
  //  path: "regions",
  //  component: RegionComponent,
  //  canActivate: [AuthGuardService],
  //  data: { icon: "verified_user", inMenu: true, title: "Regions" },
  // },
  // {
  //  path: "religion",
  //  component: ReligionComponent,
  //  canActivate: [AuthGuardService],
  //  data: { icon: "verified_user", inMenu: true, title: "Religion" },
  // },
  // {
  //  path: "trainee-disability",
  //  component: TraineeDisabilityComponent,
  //  canActivate: [AuthGuardService],
  //  data: { icon: "verified_user", inMenu: true, title: "Trainee Disability" },
  // },
  {
    path: 'district',
    component: DistrictComponent,
    canActivate: [AuthGuardService],
    data: { icon: 'verified_user', inMenu: true, title: 'Districts' },
  },
  // {
  //  path: "tehsil",
  //  component: TehsilComponent,
  //  canActivate: [AuthGuardService],
  //  data: { icon: "verified_user", inMenu: true, title: "Tehsils" },
  // },
  // {
  //  path: "sector",
  //  component: SectorComponent,
  //  canActivate: [AuthGuardService],
  //  data: { icon: "verified_user", inMenu: true, title: "Sectors" },
  // },
  // {
  //  path: "subsector",
  //  component: SubSectorComponent,
  //  canActivate: [AuthGuardService],
  //  data: { icon: "verified_user", inMenu: true, title: "subsectors" },
  // },
  {
    path: 'funding-source',
    component: FundingSourceComponent,
    canActivate: [AuthGuardService],
    data: {
      icon: 'verified_user',
      inMenu: true,
      title: 'Funding Source Component',
    },
  },
  {
    path: 'funding-category',
    component: FundingCategoryComponent,
    canActivate: [AuthGuardService],
    data: {
      icon: 'verified_user',
      inMenu: true,
      title: 'Funding Category Component',
    },
  },
  {
    path: 'academic-discipline',
    component: AcademicDisciplineComponent,
    canActivate: [AuthGuardService],
    data: {
      icon: 'verified_user',
      inMenu: true,
      title: 'Academic Discipline Component',
    },
  },
  {
    path: 'custom-financial-year',
    component: CustomFinancialYearComponent,
    canActivate: [AuthGuardService],
    data: {
      icon: 'verified_user',
      inMenu: true,
      title: 'Custom Financial Year',
    },
  },
  // {
  //  path: "programtype",
  //  component: ProgramTypeComponent,
  //  canActivate: [AuthGuardService],
  //  data: { icon: "verified_user", inMenu: true, title: "Program Type" },
  // },
  // {
  //  path: "program-category",
  //  component: ProgramCategoryComponent,
  //  canActivate: [AuthGuardService],
  //  data: { icon: "verified_user", inMenu: true, title: "Program Category" },
  // },
  // {
  //  path: "stipendstatus",
  //  component: StipendStatusComponent,
  //  canActivate: [AuthGuardService],
  //  data: { icon: "verified_user", inMenu: true, title: "Stipend Status" },
  // },
  // {
  //  path: "resultstatus",
  //  component: ResultStatusComponent,
  //  canActivate: [AuthGuardService],
  //  data: { icon: "verified_user", inMenu: true, title: "Result Status" },
  // },
  {
    path: 'ClassStatus',
    component: ClassStatusComponent,
    canActivate: [AuthGuardService],
    data: { icon: 'verified_user', inMenu: true, title: 'Class Status' },
  },
  // {
  //  path: "traineestatus",
  //  component: TraineeStatusComponent,
  //  canActivate: [AuthGuardService],
  //  data: { icon: "verified_user", inMenu: true, title: "Trainee Status" },
  // },
  {
    path: 'employment-status',
    component: EmploymentStatusComponent,
    canActivate: [AuthGuardService],
    data: { icon: 'verified_user', inMenu: true, title: 'Employment Status' },
  },
  {
    path: 'certification-category',
    component: CertificationCategoryComponent,
    canActivate: [AuthGuardService],
    data: {
      icon: 'verified_user',
      inMenu: true,
      title: 'Certification Category',
    },
  },
  {
    path: 'certification-authority',
    component: CertificationAuthorityComponent,
    canActivate: [AuthGuardService],
    data: {
      icon: 'verified_user',
      inMenu: true,
      title: 'Certification Authority',
    },
  },
  // {
  //  path: "testing-agency",
  //  component: TestingAgencyComponent,
  //  canActivate: [AuthGuardService],
  //  data: { icon: "verified_user", inMenu: true, title: "Testing Agency" },
  // },
  // {
  //  path: "inflation-rate",
  //  component: InflationRateComponent,
  //  canActivate: [AuthGuardService],
  //  data: { icon: "verified_user", inMenu: true, title: "Inflation Rate" },
  // },notifications-handling
  {
    path: 'notifications',
    component: NotificationsComponent,
    canActivate: [AuthGuardService],
    data: { icon: 'verified_user', inMenu: true, title: 'Notifications' },
  },
  {
    path: 'configurations',
    component: OrgConfigComponent,
    canActivate: [AuthGuardService],
    data: {
      icon: 'verified_user',
      inMenu: true,
      title: 'Organizations Configuration',
    },
  },
  {
    path: 'infrastructure',
    component: InfrastructureComponent,
    canActivate: [AuthGuardService],
    data: { icon: 'verified_user', inMenu: true, title: 'Infrastructure' },
  },
  {
    path: 'TSPColor',
    component:tspcolorComponent,
   // canActivate: [AuthGuardService],
    data: { icon: 'verified_user', inMenu: true, title: 'TSPColor' },
  },
  // {
  //  path: "sap-branches",
  //  component: SapBranchesComponent,
  //  canActivate: [AuthGuardService],
  //  data: {
  //    icon: "verified_user",
  //    inMenu: true,
  //    title: "SAP Branches",
  //  },
  // },
];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class MasterDataRoutingModule { }
