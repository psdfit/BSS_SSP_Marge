
import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { AuthGuardService } from "../../app/security/auth-guard.service";
import { CompletionReportComponent } from "./completion-report/completion-report.component";
import { PerformanceAnalysisReportComponent } from "./performance-analysis-report/performance-analysis-report.component";
import { TierInfoComponent } from "./tier-info/tier-info.component";
import { TSPPerformanceComponent } from "./tspperformance/tspperformance.component";
import { VoilationSummaryReportComponent } from "./voilation-summary-report/voilation-summary-report.component";

const routes: Routes = [
  {
    path: "tier-info",
    component: TierInfoComponent,
    canActivate: [AuthGuardService],
    // data: { icon: "verified_user", inMenu: true, title: "ROSI" },
  },
  {
    path: "violation-smmary",
    component: VoilationSummaryReportComponent,
    canActivate: [AuthGuardService],
    // data: { icon: "verified_user", inMenu: true, title: "ROSI" },
  },
  {
    path: "completion-report",
    component: CompletionReportComponent,
    canActivate: [AuthGuardService],
    // data: { icon: "verified_user", inMenu: true, title: "ROSI" },
  },
  {
    path: "performance-analysis-report",
    component: PerformanceAnalysisReportComponent,
    canActivate: [AuthGuardService],
    // data: { icon: "verified_user", inMenu: true, title: "ROSI" },
  },
  {
    path: "tsp-performance",
    component: TSPPerformanceComponent,
    canActivate: [AuthGuardService],
    // data: { icon: "verified_user", inMenu: true, title: "ROSI" },
  },
];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  exports: [RouterModule],
})
export class TierRouting {}
