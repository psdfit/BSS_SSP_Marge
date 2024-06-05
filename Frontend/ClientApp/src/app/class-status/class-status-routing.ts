import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ClassStatusComponent } from './class-status/class-status.component';
import { AuthGuardService } from '../../app/security/auth-guard.service';
import { ClassProceedingStatusComponent } from './class-proceeding-status/class-proceeding-status.component';

const routes: Routes = [{
  path: "class-status"
  , component: ClassStatusComponent
  , canActivate: [AuthGuardService]
  , data: { icon: "verified_user", inMenu: true, title: "Class Status" }
},
  {
    path: "class-proceeding-status"
    , component: ClassProceedingStatusComponent
    , canActivate: [AuthGuardService]
    , data: { icon: "verified_user", inMenu: true, title: "Class Proceeding Status" }
  }];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ClassStatusRoutingModule { }
