import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { TierInfoComponent } from "./tier-info/tier-info.component";
import { SharedModule } from "../shared/shared.module";
import { TierRouting } from "./tier-routing";
import { VoilationSummaryReportComponent } from './voilation-summary-report/voilation-summary-report.component';
import { CompletionReportComponent } from './completion-report/completion-report.component';
import { PerformanceAnalysisReportComponent } from './performance-analysis-report/performance-analysis-report.component';
import { TSPPerformanceComponent } from './tspperformance/tspperformance.component';

@NgModule({
  declarations: [TierInfoComponent, VoilationSummaryReportComponent, CompletionReportComponent, PerformanceAnalysisReportComponent, TSPPerformanceComponent],
  imports: [CommonModule, SharedModule, TierRouting],
 
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  
})
export class TierModule {}
