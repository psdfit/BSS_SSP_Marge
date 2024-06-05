import { CommonModule } from '@angular/common';
import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { SharedModule } from '../shared/shared.module';
import { CancelationRoutingModule } from './cancelation-routing.module';
import { CancelComponent } from './cancel/cancel.component';




@NgModule({
  declarations: [CancelComponent],
  
  imports: [
    CommonModule,
    SharedModule,
    CancelationRoutingModule,
  ], schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class CancelationModule { }
