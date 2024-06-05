import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PotentialTraineesRoutingModule } from './potential-trainees-routing';
import { PotentialTraineesListComponent } from './potential-trainees-list/potential-trainees-list.component';
import { PotentialTraineesEnrollmentComponent } from './potential-trainees-enrollment/potential-trainees-enrollment.component';

import { SharedModule } from '../shared/shared.module';

//import { VisitPlanDialogComponent } from './visit-plan-dialog/visit-plan-dialog.component';

@NgModule({
  declarations: [PotentialTraineesListComponent, PotentialTraineesEnrollmentComponent],
  imports: [
    CommonModule
    , SharedModule
    , PotentialTraineesRoutingModule
  ], schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class PotentialTraineesModule { }
