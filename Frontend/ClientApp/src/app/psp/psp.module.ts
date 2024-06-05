import { CommonModule } from '@angular/common';
import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { MatTableExporterModule } from 'mat-table-exporter';
import { SharedModule } from '../shared/shared.module';

import { PSPEmpComponent } from './psp-employment/psp-employment.component';
import { PSPEmpRoutingModule } from './psp-routing';
import { PSPTraineeListComponent } from './psp-trainees/psp-trainees-list.component';
import { TraineePSPAssignmentComponent } from './trainee-psp-assignment/trainee-psp-assignment.component';
import { PSPBatchDialogueComponent } from './psp-batch-dialogue/psp-batch-dialogue.component';
import { PSPAssignedTraineesListComponent } from './psp-assigned-trainees-list/psp-assigned-trainees-list.component';
import { PSPEmploymentDialogComponent } from './psp-employment-dialog/psp-employment-dialog.component';


@NgModule({
  declarations: [
    PSPEmpComponent, PSPTraineeListComponent, TraineePSPAssignmentComponent, PSPAssignedTraineesListComponent, PSPEmploymentDialogComponent],
  imports: [
    CommonModule,
    SharedModule,
    PSPEmpRoutingModule,
    MatTableExporterModule
  ], schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class PSPEmpModule { }
