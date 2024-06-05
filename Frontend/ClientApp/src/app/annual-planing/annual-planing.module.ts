import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from "@angular/core";
import { SharedModule } from "../shared/shared.module";

import { AnnualPlaningRoutingModule } from "./annual-planing-routing.module";
import { TradePlanComponent } from "./trade-plan/trade-plan.component";
import { CalculateCtmComponent } from "./calculate-ctm/calculate-ctm.component";
import { ValidateBusinessPlanComponent } from "./validate-business-plan/validate-business-plan.component";
import { ProcessApprovedPlanComponent } from "./process-approved-plan/process-approved-plan.component";
import { ProcessStatusUpdateComponent } from "./process-status-update/process-status-update.component";
import { RegistrationAnalysisReportComponent } from "./registration-analysis-report/registration-analysis-report.component";
import { HistoricalReportComponent } from './historical-report/historical-report.component';
import { ProgramPlanComponent } from './program-plan/program-plan.component';
import { ProgramInitiateComponent } from './program-initiate/program-initiate.component';

@NgModule({
  declarations: [
    TradePlanComponent,
    CalculateCtmComponent,
    ValidateBusinessPlanComponent,
    ProcessApprovedPlanComponent,
    ProcessStatusUpdateComponent,
    RegistrationAnalysisReportComponent,
    HistoricalReportComponent,
    ProgramPlanComponent,
    ProgramInitiateComponent

  ],
  imports: [SharedModule, AnnualPlaningRoutingModule],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
})
export class AnnualPlaningModule {}
