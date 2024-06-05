import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { SrnDisbursementStatusComponent } from './srn-disbursement-status/srn-disbursement-status.component';

import { AuthGuardService } from '../../app/security/auth-guard.service';


const routes: Routes = [{
  
  path: "srn-disbursement-status", component: SrnDisbursementStatusComponent, canActivate: [AuthGuardService], data: { icon: "verified_user", inMenu: true, title: "SRN Disbursement Status" }
}
];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SrnDisbursementStatusRoutingModule { }
