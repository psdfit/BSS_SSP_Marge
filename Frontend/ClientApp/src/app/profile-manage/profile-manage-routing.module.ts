import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ProfileComponent } from './profile/profile.component';
import { AuthGuardService } from '../security/auth-guard.service';

import { BaseDataComponent } from './base-data/base-data.component';
import { RegistrationEvaluationComponent } from './registration-evaluation/registration-evaluation.component';
import { TspRegistrationReportComponent } from './tsp-registration-report/tsp-registration-report.component';





const routes: Routes = [
  {
    path: "profile",
    component: ProfileComponent,
    canActivate: [AuthGuardService],
    data: {
      icon: "verified_user",
      inMenu: true,
      title: "Profile Management",
    },
  },
  {
    path: "base-data",
    component: BaseDataComponent,
    canActivate: [AuthGuardService],
    data: {
      icon: "verified_user",
      inMenu: true,
      title: "Base Data",
    },
  },
  {
    path: "evaluation",
    component: RegistrationEvaluationComponent,
    canActivate: [AuthGuardService],
    data: {
      icon: "verified_user",
      inMenu: true,
      title: "Registration Evaluation",
    },
  },
  {
    path: "registration-report",
    component: TspRegistrationReportComponent,
    canActivate: [AuthGuardService],
    data: {
      icon: "verified_user",
      inMenu: true,
      title: "TSP Registration Report",
    },
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ProfileManageRoutingModule { }
