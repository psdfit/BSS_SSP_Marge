import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ROSIRoutingModule } from './rosi-routing';
import { ROSIComponent } from './rosi/rosi.component';

import { SharedModule } from '../shared/shared.module';
import { MatTableExporterModule } from 'mat-table-exporter';

@NgModule({
    declarations: [ROSIComponent],
  imports: [
    CommonModule,
    SharedModule,
      ROSIRoutingModule,
      MatTableExporterModule
  ], schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class ROSIModule { }
