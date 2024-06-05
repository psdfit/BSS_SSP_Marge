import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TraineeReportsRoutingModule } from './traineereports-routing';
import { TraineereportsComponent } from './tsr/tsr.component';
import { TSRDialogComponent } from './tsr/tsr-dialog/tsr-dialog.component';

import { SharedModule } from '../shared/shared.module';
import { SrnComponent } from './srn/srn.component';
import { MPRComponent } from './mpr/mpr.component';
import { SrnEditDialogComponent } from './srn/srn-edit-dialog/srn-edit-dialog.component';
import { MatTableExporterModule } from 'mat-table-exporter';
import { TraineeStatusUpdateComponent } from './trainee-status-update/trainee-status-update.component';
import { TraineeCompletionReportComponent } from './trainee-completion-report/trainee-completion-report.component';
import { TsuDialogueComponent } from './trainee-status-update/tsu-dialogue/tsu-dialogue.component';
import { TraineeStatusReportComponent } from './trainee-status-report/trainee-status-report.component';
import { PrnReportComponent } from './prn-report/prn-report.component';
import { ClassStatusUpdateComponent } from './class-status-update/class-status-update/class-status-update.component';
import { ClassStatusDialougeComponent } from './class-status-update/class-status-dialouge/class-status-dialouge.component';

@NgModule({
  declarations: [
    TraineereportsComponent,
    SrnComponent,
    SrnEditDialogComponent,
    TSRDialogComponent,
    MPRComponent,
    TraineeStatusUpdateComponent,
    TraineeCompletionReportComponent,
    TsuDialogueComponent,
    TraineeStatusReportComponent,
    PrnReportComponent,
    ClassStatusUpdateComponent,
    ClassStatusDialougeComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    TraineeReportsRoutingModule,
    MatTableExporterModule
  ],
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class TraineeReportsModule { }
