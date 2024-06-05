import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuardService } from '../../security/auth-guard.service';
import { CancelComponent } from './cancel/cancel.component';
//import { AuthGuardService } from '../security/auth-guard.service';


const routes: Routes = [{ path: "cancel", component: CancelComponent, canActivate: [AuthGuardService], data: { icon: "verified_user", inMenu: true, title: "Event Calender" } }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CancelationRoutingModule { }
