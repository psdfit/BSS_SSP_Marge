import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { TSPNTPsComponent } from './tsp-ntps/tsp-ntps.component';

import { AuthGuardService } from '../../app/security/auth-guard.service';




const routes: Routes = [{
  
    path: "ntps", component: TSPNTPsComponent, canActivate: [AuthGuardService], data: { icon: "verified_user", inMenu: true, title: "NTPs" }
}
];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class TSPNTPsRoutingModule { }
