import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { CommonModule } from '@angular/common';
import { InceptionReportRoutingModule } from './inception-report-routing';
import { InceptionReportComponent } from './inception-report/inception-report.component';

import { SharedModule } from '../shared/shared.module';
import { RTPDialogComponent } from './rtp/rtp.component';
import { RTPApprovalDialogueComponent } from './rtp-approval-dialogue/rtp-approval-dialogue.component';

@NgModule({
    declarations: [InceptionReportComponent, RTPDialogComponent, RTPApprovalDialogueComponent],
  imports: [
    CommonModule,
    SharedModule,
    InceptionReportRoutingModule
  ], schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class InceptionReportModule { }
