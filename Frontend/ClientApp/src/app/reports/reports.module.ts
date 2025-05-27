import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ViewReportsComponent } from './view-reports/view-reports.component';
import { ViewAmsReportsComponent } from './view-ams-reports/view-ams-reports.component';
import { SharedModule } from '../shared/shared.module';
import { ReportRouting } from './reports.routing';
import { ViewSspReportsComponent } from './view-ssp-reports/view-ssp-reports.component';

@NgModule({
  declarations: [ViewReportsComponent, ViewAmsReportsComponent, ViewSspReportsComponent],
  imports: [CommonModule, SharedModule, ReportRouting],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
})
export class ReportsModule {}
