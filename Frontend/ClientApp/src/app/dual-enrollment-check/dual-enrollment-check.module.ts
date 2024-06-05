import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DualEnrollmentCheckRoutingModule } from './dual-enrollment-check-routing';
import { DualEnrollmentCheckComponent } from './dual-enrollment-check/dual-enrollment-check.component';

import { SharedModule } from '../shared/shared.module';
import { MatTableExporterModule } from 'mat-table-exporter';

@NgModule({
    declarations: [DualEnrollmentCheckComponent],
  imports: [
    CommonModule,
    SharedModule,
      DualEnrollmentCheckRoutingModule,
      MatTableExporterModule
  ], schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class DualEnrollmentCheckModule { }
