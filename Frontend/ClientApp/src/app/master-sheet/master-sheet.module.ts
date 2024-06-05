import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MasterSheetRoutingModule } from './master-sheet-routing';
import { MasterSheetComponent } from './master-sheet/master-sheet.component';
import { InceptionReportListComponent } from './inception-report-list/inception-report-list.component';

import { SharedModule } from '../shared/shared.module';
import { TradeLayerComponent } from "../master-data/trade-layer/trade-layer.component";


//import { VisitPlanDialogComponent } from './visit-plan-dialog/visit-plan-dialog.component';

@NgModule({
  declarations: [MasterSheetComponent, InceptionReportListComponent,TradeLayerComponent],
  imports: [
    CommonModule
    , SharedModule
    , MasterSheetRoutingModule
  ], schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class MasterSheetModule { }
