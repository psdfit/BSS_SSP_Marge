import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TSPNTPsRoutingModule } from './tsp-ntps-routing';
import { TSPNTPsComponent } from './tsp-ntps/tsp-ntps.component';

import { SharedModule } from '../shared/shared.module';
import { MatTableExporterModule } from 'mat-table-exporter';

@NgModule({
    declarations: [TSPNTPsComponent],
  imports: [
    CommonModule,
    SharedModule,
      TSPNTPsRoutingModule,
      MatTableExporterModule
  ], schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class TSPNTPsModule { }
