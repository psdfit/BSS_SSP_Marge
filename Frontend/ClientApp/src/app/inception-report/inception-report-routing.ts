import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { InceptionReportComponent } from './inception-report/inception-report.component';

import { AuthGuardService } from '../../app/security/auth-guard.service';




const routes: Routes = [{
  
  path: "inception/:id", component: InceptionReportComponent, canActivate: [AuthGuardService], data: { icon: "verified_user", inMenu: true, title: "Inception Report" }
}
];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class InceptionReportRoutingModule { }
