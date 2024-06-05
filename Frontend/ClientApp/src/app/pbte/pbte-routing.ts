import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { PBTEComponent } from './pbte/pbte.component';

import { AuthGuardService } from '../../app/security/auth-guard.service';




const routes: Routes = [{
  
    path: "pbte", component: PBTEComponent, canActivate: [AuthGuardService], data: { icon: "verified_user", inMenu: true, title: "PBTE" }
}
];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PBTERoutingModule { }
