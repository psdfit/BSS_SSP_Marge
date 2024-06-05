import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { DualEnrollmentCheckComponent } from './dual-enrollment-check/dual-enrollment-check.component';

import { AuthGuardService } from '../../app/security/auth-guard.service';




const routes: Routes = [{
  
    path: "dual-enrollment-check", component: DualEnrollmentCheckComponent, canActivate: [AuthGuardService], data: { icon: "verified_user", inMenu: true, title: "Dual Enrollment Check" }
}
];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class DualEnrollmentCheckRoutingModule { }
