import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuardService } from '../../app/security/auth-guard.service';
import { ViewReportsComponent } from './view-reports/view-reports.component';
import { ViewAmsReportsComponent } from './view-ams-reports/view-ams-reports.component';
import { ViewSspReportsComponent } from './view-ssp-reports/view-ssp-reports.component';

const routes: Routes = [
  {
    path: 'view-reports',
    component: ViewReportsComponent,
    canActivate: [AuthGuardService],
    data: {
      icon: 'verified_user',
      inMenu: true,
      title: 'View Reports',
    },
  },
  {
    path: 'view-ams-report',
    component: ViewAmsReportsComponent,
    canActivate: [AuthGuardService],
    data: {
      icon: 'verified_user',
      inMenu: true,
      title: 'View Reports',
    },
  },
  {
    path: 'view-ssp-reports',
    component: ViewSspReportsComponent,
    canActivate: [AuthGuardService],
    data: {
      icon: 'verified_user',
      inMenu: true,
      title: 'SSP Reports',
    },
  },
];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ReportRouting { }
