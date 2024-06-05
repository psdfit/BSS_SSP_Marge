import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../shared/shared.module';
import { TPMReportsRoutingModule } from './tpm-reports-routing.module';
import { TPMReportsComponent } from './tpm-reports/tpm-reports.component';


@NgModule({
  declarations: [TPMReportsComponent],
  imports: [
      CommonModule,
      SharedModule,
    TPMReportsRoutingModule
    ], schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class TPMReportsModule { }
