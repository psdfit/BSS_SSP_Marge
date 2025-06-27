import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { TSPEmpComponent } from './tsp-employment/tsp-employment.component';

import { AuthGuardService } from '../../app/security/auth-guard.service';
import { TSPTraineeListComponent } from './tsp-trainee-list/tsp-trainee-list.component';
import { EmpVerificationComponent } from './employment-verification/employment-verification.component';
import { SelfEmploymentVerificationComponent } from './self-employment-verification/self-employment-verification.component';
import { FormalEmploymentVerificationComponent } from './formal-employment-verification/formal-employment-verification.component';
import { TelephonicComponent } from './telephonic/telephonic.component';
import { DeoVerificationComponent } from './deo-verification/deo-verification.component';
import { OnjobTraineePlacementComponent } from './onjob-trainee-placement/onjob-trainee-placement.component';
import { OJTEmpComponent } from './ojt-employment/ojt-employment.component';



const routes: Routes = [
  {
    path: 'tsp-employment',
    component: TSPEmpComponent,
    canActivate: [AuthGuardService],
    data: { icon: 'verified_user', inMenu: true, title: 'TSP' }
  },
  {
    path: 'tsp-employment/:classid/:traineeid',
    component: TSPEmpComponent,
    canActivate: [AuthGuardService],
    data: { icon: 'verified_user', inMenu: false, title: 'TSP' }
  },
  //{
  //  path: 'ojt-employment',
  //  component: OJTEmpComponent,
  //  canActivate: [AuthGuardService],
  //  data: { icon: 'verified_user', inMenu: true, title: 'TSP' }
  //},
  {
    path: 'ojt-employment/:classid/:traineeid',
    component: OJTEmpComponent,
    canActivate: [AuthGuardService],
    data: { icon: 'verified_user', inMenu: false, title: 'TSP' }
  },
  {
    path: 'trainee-list',
    component: TSPTraineeListComponent,
    canActivate: [AuthGuardService],
    data: { icon: 'verified_user', inMenu: true, title: 'Trainee List' }
  },
  {
    path: 'on-job-trainee',
    component: OnjobTraineePlacementComponent,
    canActivate: [AuthGuardService],
    data: { icon: 'verified_user', inMenu: true, title: 'OnJob Trainee Placement' }
  },
  {
    path: 'employment-verification',
    component: EmpVerificationComponent,
    canActivate: [AuthGuardService],
    data: { icon: 'verified_user', inMenu: true, title: 'Trainee List' }
  },
  {
    path: 'formal-telephonic-employment-verification',
    component: TelephonicComponent,
    canActivate: [AuthGuardService],
    data: { icon: 'verified_user', inMenu: true, title: 'Trainee List' }
  },
  {
    path: 'self-employment-verification',
    component: DeoVerificationComponent,
    canActivate: [AuthGuardService],
    data: { icon: 'verified_user', inMenu: true, title: 'Trainee List' }
  }
];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class TSPEmpRoutingModule { }
