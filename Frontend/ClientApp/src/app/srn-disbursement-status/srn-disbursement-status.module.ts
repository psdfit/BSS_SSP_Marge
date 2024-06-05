import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SrnDisbursementStatusRoutingModule } from './srn-disbursement-status-routing';
import { SrnDisbursementStatusComponent } from './srn-disbursement-status/srn-disbursement-status.component';

import { SharedModule } from '../shared/shared.module';
import { MatTableExporterModule } from 'mat-table-exporter';

@NgModule({
  declarations: [SrnDisbursementStatusComponent],
  imports: [
    CommonModule,
    SharedModule,
    SrnDisbursementStatusRoutingModule,
      MatTableExporterModule
  ], schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class SrnDisbursementStatusModule { }
