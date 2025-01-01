import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { TraineeComponent } from './trainee/trainee.component';

import { AuthGuardService } from '../../app/security/auth-guard.service';
import { TraineeVarificationComponent } from './trainee-verification/trainee-verification.component';
import { TraineeUpdationComponent } from './trainee-updation/trainee-updation.component';




const routes: Routes = [
  {
    path: "trainee",
    component: TraineeComponent,
    canActivate: [AuthGuardService],
    data: {
      icon: "verified_user",
      inMenu: true,
      title: "Trainee Registration"
    }
  },
  {
    path: "trainee/:id",
    component: TraineeComponent,
    canActivate: [AuthGuardService],
    data: {
      icon: "verified_user",
      inMenu: false,
      title: "Trainee Registration"
    }
  },
  {
    path: "trainee-verification",
    component: TraineeVarificationComponent,
    canActivate: [AuthGuardService],
    data: {
      icon: "verified_user",
      inMenu: true,
      title: "verification Trainee"
    }
  },
  {
    path: "trainee-updation",
    component: TraineeUpdationComponent,
    canActivate: [AuthGuardService],
    data: {
      icon: "verified_user",
      inMenu: true,
      title: "Trainee Update"
    }
  }
];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class RegistrationRoutingModule { }
