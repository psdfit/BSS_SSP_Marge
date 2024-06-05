import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
//import { PotentialTraineesListComponent } from './master-sheet/master-sheet.component';
import { PotentialTraineesListComponent } from './potential-trainees-list/potential-trainees-list.component';
import { PotentialTraineesEnrollmentComponent } from './potential-trainees-enrollment/potential-trainees-enrollment.component';

import { AuthGuardService } from '../security/auth-guard.service';




const routes: Routes = [{
  // path: 'administration', component: AppLayoutComponent, children: [{
  path: "potential-trainees-list", component: PotentialTraineesListComponent, canActivate: [AuthGuardService], data: { icon: "verified_user", inMenu: true, title: "Potential Trainees List" }
},
  {
    path: "potential-trainees-enrollment", component: PotentialTraineesEnrollmentComponent, canActivate: [AuthGuardService], data: { icon: "verified_user", inMenu: true, title: "Potential Trainees Enrollment" }
}
];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PotentialTraineesRoutingModule { }
