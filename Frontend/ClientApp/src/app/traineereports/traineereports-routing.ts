import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuardService } from '../../app/security/auth-guard.service';
import { SrnComponent } from './srn/srn.component';
import { MPRComponent } from './mpr/mpr.component';
import { TraineeStatusUpdateComponent } from './trainee-status-update/trainee-status-update.component';
import { TraineeCompletionReportComponent } from './trainee-completion-report/trainee-completion-report.component';
import { TraineeStatusReportComponent } from './trainee-status-report/trainee-status-report.component';
import { PrnReportComponent } from './prn-report/prn-report.component';
import { ClassStatusUpdateComponent } from './class-status-update/class-status-update/class-status-update.component';
import { GuruRportsComponent } from './gsr/gsr.component';

const routes: Routes = [
  {
    // path: 'administration', component: AppLayoutComponent, children: [{
    path: 'tsr'
    , component: TraineeStatusReportComponent
    , canActivate: [AuthGuardService]
    , data: {
      icon: 'verified_user'
      , inMenu: true
      , title: 'Trainee Status Report'
    }
  }, {
    path: 'srn'
    , component: SrnComponent
    , canActivate: [AuthGuardService]
    , data: {
      icon: 'verified_user'
      , inMenu: true
      , title: 'Stipend Recomendation Report (SRN)'
    }
  }, {
    path: 'mpr'
    , component: MPRComponent
    , canActivate: [AuthGuardService]
    , data: {
      icon: 'verified_user'
      , inMenu: true
      , title: 'Stipend Recomendation Report (SRN)'
    }
  }
  , {
    path: 'tcr'
    , component: TraineeCompletionReportComponent
    , canActivate: [AuthGuardService]
    , data: {
      icon: 'verified_user'
      , inMenu: true
      , title: 'Trainee Competion Report'
    }
  }
  , {
    path: 'tsu'
    , component: TraineeStatusUpdateComponent
    , canActivate: [AuthGuardService]
    , data: {
      icon: 'verified_user'
      , inMenu: true
      , title: 'Trainee Status Update'
    }
  }
  , {
    path: 'prn-report'
    , component: PrnReportComponent
    , canActivate: [AuthGuardService]
    , data: {
      icon: 'verified_user'
      , inMenu: true
      , title: 'PRN Approval Requests'
    }
  }
  , {
    path: 'class-status-update'
    , component: ClassStatusUpdateComponent
    , canActivate: [AuthGuardService]
    , data: {
      icon: 'verified_user'
      , inMenu: true
      , title: 'Class Status Update'
    }
  },
  {
    // path: 'administration', component: AppLayoutComponent, children: [{
    path: 'gsr'
    , component: GuruRportsComponent
    , canActivate: [AuthGuardService]
    , data: {
      icon: 'verified_user'
      , inMenu: true
      , title: 'Guru Status Report'
    }
  }

];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class TraineeReportsRoutingModule { }
