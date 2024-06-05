import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ChangeRequestDialogComponent } from './change-request/change-request.component';

import { AuthGuardService } from '../../app/security/auth-guard.service';


const routes: Routes = [{
  // path: 'administration', component: AppLayoutComponent, children: [{
    path: "change-request", component: ChangeRequestDialogComponent, canActivate: [AuthGuardService], data: { icon: "verified_user", inMenu: true, title: "Change Request" }
}
];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ChangeRequestRoutingModule { }
