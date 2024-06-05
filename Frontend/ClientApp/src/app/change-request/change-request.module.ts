import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SharedModule } from '../shared/shared.module';
import { ChangeRequestDialogComponent } from './change-request/change-request.component';
import { ChangeRequestRoutingModule } from './change-request-routing';
import { MatTableExporterModule } from 'mat-table-exporter';


@NgModule({
    declarations: [ChangeRequestDialogComponent],
  imports: [
    CommonModule,
      SharedModule,
      ChangeRequestRoutingModule,
      MatTableExporterModule
    //InceptionReportRoutingModule
  ], schemas: [CUSTOM_ELEMENTS_SCHEMA]
})


export class ChangeRequestModule { }
