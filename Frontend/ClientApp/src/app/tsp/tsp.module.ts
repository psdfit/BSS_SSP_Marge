import { CommonModule } from '@angular/common';
import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { MatTableExporterModule } from 'mat-table-exporter';
import { SharedModule } from '../shared/shared.module';
import { DeoVerificationComponent } from './deo-verification/deo-verification.component';
import { EmpVerificationComponent } from './employment-verification/employment-verification.component';
import { FormalEmploymentVerificationComponent } from './formal-employment-verification/formal-employment-verification.component';
import { SelfEmploymentVerificationComponent } from './self-employment-verification/self-employment-verification.component';
import { TelephonicComponent } from './telephonic/telephonic.component';
import { TSPEmpComponent } from './tsp-employment/tsp-employment.component';
import { TSPEmpRoutingModule } from './tsp-routing';
import { TSPTraineeListComponent } from './tsp-trainee-list/tsp-trainee-list.component';
import { EmploymentDailogComponent } from './employment-dailog/employment-dailog.component';
import { DEOEmploymentVerificationDialogComponent } from './deo-employment-verification-dialog/deo-employment-verification-dialog.component';
import { TelephonicEmploymentverification } from './TelephonicEmployment-verification/TelephonicEmployment-verification.component';
import { telephonicemploymentverificationComponent } from './telephonic-employment-verification-dialog/telephonic-employment-verification-dialog.component';
import { OnjobTraineePlacementComponent } from './onjob-trainee-placement/onjob-trainee-placement.component';
import { OJTEmpComponent } from './ojt-employment/ojt-employment.component';

@NgModule({
  declarations: [
    TSPEmpComponent,
    TSPTraineeListComponent,
    EmpVerificationComponent,
    SelfEmploymentVerificationComponent,
    FormalEmploymentVerificationComponent,
    TelephonicComponent,
    DeoVerificationComponent,
    EmploymentDailogComponent,
    TelephonicEmploymentverification,
    DEOEmploymentVerificationDialogComponent,
    telephonicemploymentverificationComponent,
    OnjobTraineePlacementComponent,
    OJTEmpComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    TSPEmpRoutingModule,
    MatTableExporterModule
  ], schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class TSPEmpModule { }
