import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuardService } from '../security/auth-guard.service';

import { VisaStampingComponent } from './visa-stamping/visa-stamping.component';
import { MedicalCostComponent } from './medical-cost/medial-cost.component';
import { PrometricCostComponent } from './prometric-cost/prometric-cost.component';
import { OtherTrainingCostComponent } from './other-training-cost/other-training-cost.component';



const routes: Routes = [
  {
    path: "visa-stamping",
    component: VisaStampingComponent,
    canActivate: [AuthGuardService],
    data: {
      icon: "verified_user",
      inMenu: true,
      title: "Visa Stamping",
    },
  },
  {
    path: "medical-cost",
    component: MedicalCostComponent,
    canActivate: [AuthGuardService],
    data: {
      icon: "verified_user",
      inMenu: true,
      title: "Medical Cost",
    },
  },
  {
    path: "prometric-cost",
    component: PrometricCostComponent,
    canActivate: [AuthGuardService],
    data: {
      icon: "verified_user",
      inMenu: true,
      title: "Prometric Cost",
    },
  },
  {
    path: "other-training-cost",
    component: OtherTrainingCostComponent,
    canActivate: [AuthGuardService],
    data: {
      icon: "verified_user",
      inMenu: true,
      title: "Other Training Cost",
    },
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class InternationalPlacementRoutingModule { }
