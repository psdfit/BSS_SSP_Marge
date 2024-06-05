import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuardService } from '../../app/security/auth-guard.service';
import { TraineeJourneyAllComponent } from './trainee-journey-all/trainee-journey-all.component';
import { TraineeJourneyComponent } from './trainee-journey/trainee-journey.component';
import { ClassJourneyComponent } from './class-journey/class-journey.component';
import { ManagementComponent } from './management/management.component';
import { KAMDashboardComponent } from './kam/kam-dashboard.component';

const routes: Routes = [
  {
    path: 'trainee-journey-all',
    component: TraineeJourneyAllComponent,
    canActivate: [AuthGuardService],
    data: {
      icon: 'verified_user',
      inMenu: true,
      title: 'Trainee Journey All',
    },
  },
  {
    path: 'trainee-journey',
    component: TraineeJourneyComponent,
    canActivate: [AuthGuardService],
    data: {
      icon: 'verified_user',
      inMenu: true,
      title: 'Trainee Journey',
    },
  },
  {
    path: 'class-journey',
    component: ClassJourneyComponent,
    canActivate: [AuthGuardService],
    data: {
      icon: 'verified_user',
      inMenu: true,
      title: 'Class Journey',
    },
  },
  {
    path: 'management',
    component: ManagementComponent,
    canActivate: [AuthGuardService],
    data: {
      icon: 'verified_user',
      inMenu: true,
      title: 'Management Dashboard',
    },
  },
  {
    path: 'kam-dashboard',
    component: KAMDashboardComponent,
    canActivate: [AuthGuardService],
    data: {
      icon: 'verified_user',
      inMenu: true,
      title: 'KAM Dashboard',
    },
  }
];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class DashboardRouting {}
