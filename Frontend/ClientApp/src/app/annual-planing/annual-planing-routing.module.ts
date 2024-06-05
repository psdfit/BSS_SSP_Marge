import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";

import { TradePlanComponent } from "./trade-plan/trade-plan.component";
import { AuthGuardService } from "../security/auth-guard.service";
import { CalculateCtmComponent } from "./calculate-ctm/calculate-ctm.component";
import { ValidateBusinessPlanComponent } from "./validate-business-plan/validate-business-plan.component";
import { ProcessApprovedPlanComponent } from "./process-approved-plan/process-approved-plan.component";
import { ProcessStatusUpdateComponent } from "./process-status-update/process-status-update.component";
import { RegistrationAnalysisReportComponent } from "./registration-analysis-report/registration-analysis-report.component";
import { HistoricalReportComponent } from "./historical-report/historical-report.component";
import { ProgramPlanComponent } from "./program-plan/program-plan.component";
import { ProgramInitiateComponent } from "./program-initiate/program-initiate.component";
const routes: Routes = [
  {
    path: "program-plan",
    component: ProgramPlanComponent,
    canActivate: [AuthGuardService],
    data: {
      icon: "verified_user",
      inMenu: true,
      title: "Program Plan",
    },
  },
  {
    path: "trade-plan",
    component: TradePlanComponent,
    canActivate: [AuthGuardService],
    data: {
      icon: "verified_user",
      inMenu: true,
      title: "Trade Plan",
    },
  },
  {
    path: "program-initiate",
    component: ProgramInitiateComponent,
    canActivate: [AuthGuardService],
    data: {
      icon: "verified_user",
      inMenu: true,
      title: "Program Initiate",
    },
  },
  {
    path: "calculate-ctm",
    component: CalculateCtmComponent,
    canActivate: [AuthGuardService],
    data: {
      icon: "verified_user",
      inMenu: true,
      title: "Calculate CTM",
    },
  },
  {
    path: "validate-business-plan",
    component: ValidateBusinessPlanComponent,
    canActivate: [AuthGuardService],
    data: {
      icon: "verified_user",
      inMenu: true,
      title: "Validate Business Plan",
    },
  },
  {
    path: "process-approved-plan",
    component: ProcessApprovedPlanComponent,
    canActivate: [AuthGuardService],
    data: {
      icon: "verified_user",
      inMenu: true,
      title: "Process Approved Plan",
    },
  },
  {
    path: "process-status-update",
    component: ProcessStatusUpdateComponent,
    canActivate: [AuthGuardService],
    data: {
      icon: "verified_user",
      inMenu: true,
      title: "Process Status Update",
    },
  },
  {
    path: "registration-analysis-report",
    component: RegistrationAnalysisReportComponent,
    canActivate: [AuthGuardService],
    data: {
      icon: "verified_user",
      inMenu: true,
      title: "Registration Analysis Report",
    },
  },
  {
    path: "historical-report",
    component:HistoricalReportComponent ,
    canActivate: [AuthGuardService],
    data: {
      icon: "verified_user",
      inMenu: true,
      title: "Registration Analysis | Historical Report",
    },
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AnnualPlaningRoutingModule {}
