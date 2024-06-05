import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TSPTraineePortalRoutingModule } from './tsp-trainee-portal-routing';
import { SharedModule } from '../shared/shared.module';

import { TspTraineePortalComponent } from './tsp-trainee-portal/tsp-trainee-portal.component';
import { TspDialogueComponent } from './tsp-trainee-portal/tsp-dialogue/tsp-dialogue.component';


//import { VisitPlanDialogComponent } from './visit-plan-dialog/visit-plan-dialog.component';

@NgModule({
  declarations: [TspTraineePortalComponent, TspDialogueComponent],
  imports: [
    CommonModule,
    SharedModule,
    TSPTraineePortalRoutingModule, 
  ], schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class TSPTraineePortalModule { }
