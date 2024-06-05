import { CommonModule } from '@angular/common';
import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { SharedModule } from '../shared/shared.module';
import { TSPInvoiceStatusRoutingModule } from './tsp-invoice-status-routing.module';
import { TSPInvoiceStatusComponent } from './tsp-invoice-status/tsp-invoice-status.component';




@NgModule({
  declarations: [TSPInvoiceStatusComponent],
  
  imports: [
    CommonModule,
    SharedModule,
    TSPInvoiceStatusRoutingModule,
  ], schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class TSPInvoiceStatusModule { }
