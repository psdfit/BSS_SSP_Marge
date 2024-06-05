import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PBTERoutingModule } from './pbte-routing';
import { PBTEComponent } from './pbte/pbte.component';

import { SharedModule } from '../shared/shared.module';
import { MatTableExporterModule } from 'mat-table-exporter';

@NgModule({
    declarations: [PBTEComponent],
  imports: [
    CommonModule,
    SharedModule,
      PBTERoutingModule,
      MatTableExporterModule
  ], schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class PBTEModule { }
