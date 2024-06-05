import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ProcessScheduleComponent } from './process-schedule/process-schedule.component';
import { AuthGuardService } from '../security/auth-guard.service';


const routes: Routes = [
  {
    path: 'process-schedule',
    component: ProcessScheduleComponent,
    canActivate: [AuthGuardService],
    data: {
      icon: 'verified_user',
      inMenu: true,
      title: 'Process Schedule'
    }
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ProcessConfigurationRoutingModule { }
