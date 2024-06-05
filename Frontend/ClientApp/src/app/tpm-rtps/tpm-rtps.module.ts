import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TPMRTPsRoutingModule } from './tpm-rtps-routing';
import { TPMRTPsComponent } from './tpm-rtps/tpm-rtps.component';

import { SharedModule } from '../shared/shared.module';
import { MatTableExporterModule } from 'mat-table-exporter';
import { TPMCenterInspectionComponent } from './tpm-center-inspection/tpm-center-inspection.component';


@NgModule({
    declarations: [TPMRTPsComponent, TPMCenterInspectionComponent],
  imports: [
    CommonModule,
    SharedModule,
      TPMRTPsRoutingModule,
      MatTableExporterModule
  ], schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class TPMRTPsModule { }
