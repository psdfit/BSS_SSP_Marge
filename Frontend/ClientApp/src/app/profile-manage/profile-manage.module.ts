import { NgModule ,CUSTOM_ELEMENTS_SCHEMA} from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from "../shared/shared.module";

import { ProfileManageRoutingModule } from './profile-manage-routing.module';
import { ProfileComponent } from './profile/profile.component';
import {MatStepperModule} from '@angular/material/stepper';
import { BaseDataComponent } from './base-data/base-data.component';
import { RegistrationEvaluationComponent } from './registration-evaluation/registration-evaluation.component';
import { TspRegistrationReportComponent } from './tsp-registration-report/tsp-registration-report.component';



@NgModule({
  declarations: [ProfileComponent, BaseDataComponent, RegistrationEvaluationComponent, TspRegistrationReportComponent],
  imports: [
    SharedModule,
    ProfileManageRoutingModule,MatStepperModule,CommonModule
  ], schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class ProfileManageModule { }
