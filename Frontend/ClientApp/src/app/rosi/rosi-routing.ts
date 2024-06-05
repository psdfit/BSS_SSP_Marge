import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ROSIComponent } from './rosi/rosi.component';

import { AuthGuardService } from '../../app/security/auth-guard.service';




const routes: Routes = [{
  
    path: "rosi", component: ROSIComponent, canActivate: [AuthGuardService], data: { icon: "verified_user", inMenu: true, title: "ROSI" }
}
];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ROSIRoutingModule { }
