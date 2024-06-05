import {NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { EventCalenderComponent } from './event-calender/event-calender.component';

import { AuthGuardService } from '../../app/security/auth-guard.service';
import { CallCenterComponent } from './call-center/call-center.component';



const routes: Routes = [
    {
  // path: 'administration', component: AppLayoutComponent, children: [{
    path: "eventcalender", component: EventCalenderComponent, canActivate: [AuthGuardService], data: { icon: "verified_user", inMenu: true, title: "Event Calender" }

    },
    {
  // path: 'administration', component: AppLayoutComponent, children: [{
    path: "call-center", component: CallCenterComponent, canActivate: [AuthGuardService], data: { icon: "verified_user", inMenu: true, title: "Call Center" },

    }

];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class EventCalenderRoutingModule { }
