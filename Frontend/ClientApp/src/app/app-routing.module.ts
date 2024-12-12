/* **** Aamer Rehman Malik *****/
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { AuthGuardService } from './security/auth-guard.service';
import { AppLayoutComponent } from './app-layout/app-layout.component';
import { HomeComponent } from './home/home.component';
import { ForgotPasswordComponent } from './forgot-password/forgot-password.component';
import { HomeComponent as HomeComponent1 } from './gis/home/home.component';
import { GisLayoutComponent } from './gis-layout/gis-layout.component';
import { TspSignUpComponent } from './tsp-sign-up/tsp-sign-up.component';
import { SendOTPComponent } from './send-otp/send-otp.component';

const routes: Routes = [
  {
    path: '',
    redirectTo: 'home',
    pathMatch: 'full',
  },
  {
    path: 'login',
    component: LoginComponent,
    data: { inMenu: false, title: 'User Login' },
  },
  {
    path: 'sign-up',
    component: TspSignUpComponent,
    data: { inMenu: false, title: 'Sign Up' },
  },
  {
    path: 'verify-otp',
    component: SendOTPComponent,
    data: { inMenu: false, title: 'verify OTP' },
  },
  {
    path: 'forgot-password',
    component: ForgotPasswordComponent,
    data: { inMenu: false, title: 'Forgot Password' },
  },
  {
    path: 'forgotpassword',
    component: ForgotPasswordComponent,
    data: { inMenu: false, title: 'Forgot Password' },
  },
  {
    path: 'gis',
    component: AppLayoutComponent,
    children: [
      {
        path: 'map',
        component: HomeComponent1
      }
    ]
  },
  {
    path: '',
    component: AppLayoutComponent,
    children: [
      {
        path: 'home',
        component: HomeComponent,
        canActivate: [AuthGuardService],
      },
      // Master Module
      {
        path: 'master',
        loadChildren: () =>
          import('./master-data/master-data.module').then(
            (m) => m.MasterDataModuleModule
          ),
        canLoad: [AuthGuardService],
      },
      {
        path: 'map',
        loadChildren: () =>
          import('./gis/home/home.module').then(
            (m) => m.HomeModule
          ),
      },
      {
        path: 'master',
        loadChildren: () =>
          import('./master-data/master-sub.module').then(
            (m) => m.MasterSubModuleModule
          ),
        canLoad: [AuthGuardService],
      },
      // Appendix Module
      {
        path: 'appendix',
        loadChildren: () =>
          import('./appendix-module/appendix-module.module').then(
            (m) => m.AppendixModuleModule
          ),
        canLoad: [AuthGuardService],
      },
      // Inception Report Module
      {
        path: 'inception-report',
        loadChildren: () =>
          import('./inception-report/inception-report.module').then(
            (m) => m.InceptionReportModule
          ),
        canLoad: [AuthGuardService],
      },
      // PBTE Module
      {
        path: 'pbte',
        loadChildren: () =>
          import('./pbte/pbte.module').then((m) => m.PBTEModule),
        canLoad: [AuthGuardService],
      },
      // TSP import Trainees
      {
        path: 'dual-enrollment-check',
        loadChildren: () =>
          import('./dual-enrollment-check/dual-enrollment-check.module').then(
            (m) => m.DualEnrollmentCheckModule
          ),
        canLoad: [AuthGuardService],
      },
      // SRN Disbursement Trainees
      {
        path: 'srn-disbursement-status',
        loadChildren: () =>
          import('./srn-disbursement-status/srn-disbursement-status.module').then(
            (m) => m.SrnDisbursementStatusModule
          ),
        canLoad: [AuthGuardService],
      },
      // ROSI Module
      {
        path: 'rosi',
        loadChildren: () =>
          import('./rosi/rosi.module').then((m) => m.ROSIModule),
        canLoad: [AuthGuardService],
      },
      // MasterSheet Module
      {
        path: 'mastersheet',
        loadChildren: () =>
          import('./master-sheet/master-sheet.module').then(
            (m) => m.MasterSheetModule
          ),
        canLoad: [AuthGuardService],
      },

      // Skills Scholarship Initiative
      {
        path: 'B2C',
        loadChildren: () =>
          import('./Skills-Scholarship-Dashboard/skills-scholarship-initiative.module').then(
            (m) => m.SkillsScholarshipModule
          ),
        canLoad: [AuthGuardService],
      },

      //// Class Status Update
      //{
      //  path: 'CSU',
      //  loadChildren: () =>
      //    import('./traineereports/traineereports.module').then(
      //      (m) => m.TraineeReportsModule
      //    ),
      //  canLoad: [AuthGuardService],
      //},

      // TPM Reports Module
      {
        path: 'tpm-reports',
        loadChildren: () =>
          import('./tpm-reports/tpm-reports.module').then(
            (m) => m.TPMReportsModule
          ),
        canLoad: [AuthGuardService],
      },
      // TPM RTPs Module
      {
        path: 'tpm-rtps',
        loadChildren: () =>
          import('./tpm-rtps/tpm-rtps.module').then((m) => m.TPMRTPsModule),
        canLoad: [AuthGuardService],
      },
      // NTPS Module
      {
        path: 'ntps',
        loadChildren: () =>
          import('./tsp-ntps/tsp-ntps.module').then((m) => m.TSPNTPsModule),
        canLoad: [AuthGuardService],
      },
      // Trade Benchmarking Module
      {
        path: 'trade-benchmarking',
        loadChildren: () =>
          import('./benchmarking/benchmarking.module').then(
            (m) => m.BenchmarkingModule
          ),
        canLoad: [AuthGuardService],
      },
      // ChangeRequest Module
      {
        path: 'change-request',
        loadChildren: () =>
          import('./change-request/change-request.module').then(
            (m) => m.ChangeRequestModule
          ),
        canLoad: [AuthGuardService],
      },
      // Registration Module
      {
        path: 'registration',
        loadChildren: () =>
          import('./registration/registration.module').then(
            (m) => m.RegistrationModule
          ),
        canLoad: [AuthGuardService],
      },
      // Approvals Module
      {
        path: 'approvals',
        loadChildren: () =>
          import('./approvals/approvals.module').then((m) => m.ApprovalsModule),
        canLoad: [AuthGuardService],
      },
      // Class Status Module
      {
        path: 'class-status',
        loadChildren: () =>
          import('./class-status/class-status.module').then(
            (m) => m.ClassStatusModule
          ),
        canLoad: [AuthGuardService],
      },
      // Event Calender Module
      {
        path: 'eventcalender',
        loadChildren: () =>
          import('./event-calender/event-calender.module').then(
            (m) => m.EventCalenderModule
          ),
        canLoad: [AuthGuardService],
      },
      // Trainee Reports Module
      {
        path: 'traineereports',
        loadChildren: () =>
          import('./traineereports/traineereports.module').then(
            (m) => m.TraineeReportsModule
          ),
        canLoad: [AuthGuardService],
      },
      // TSP Module
      {
        path: 'placement',
        loadChildren: () =>
          import('./tsp/tsp.module').then((m) => m.TSPEmpModule),
        canLoad: [AuthGuardService],
      },
      // PSP Module
      {
        path: 'psp-employment',
        loadChildren: () =>
          import('./psp/psp.module').then((m) => m.PSPEmpModule),
        canLoad: [AuthGuardService],
      },
      // Cancelation Module
      {
        path: 'cancelation',
        loadChildren: () =>
          import('./cancelation/cancelation.module').then((m) => m.CancelationModule),
        canLoad: [AuthGuardService],
      },
      // TSP Invoice Status Module
      {
        path: 'tsp-invoice-status',
        loadChildren: () =>
          import('./tsp-invoice-status/tsp-invoice-status.module').then((m) => m.TSPInvoiceStatusModule),
        canLoad: [AuthGuardService],
      },
      // Advanced search Module
      {
        path: 'advanced-search',
        loadChildren: () =>
          import('./advanced-search/search-module.module').then(
            (m) => m.AdvanceSearchModule
          ),
        canLoad: [AuthGuardService],
      },
      // Reports
      {
        path: 'reports',
        loadChildren: () =>
          import('./reports/reports.module').then(
            (m) => m.ReportsModule
          ),
        canLoad: [AuthGuardService],
      },
      // Complaint module
      {
        path: 'complaint',
        loadChildren: () =>
          import('./complaint-module/complaint-module.module').then(
            (m) => m.ComplaintModuleModule
          ),
        canLoad: [AuthGuardService],
      },
      {
        path: 'tier',
        loadChildren: () =>
          import('./tier/tier.module').then((m) => m.TierModule),
        canLoad: [AuthGuardService],
      },
      // Infrastructure Module

      {
        path: 'error',
        loadChildren: () =>
          import('./error-pages/error-pages.module').then(
            (m) => m.ErrorPagesModule
          ),
        // ,canLoad: [AuthGuardService],
      },
      // Dashboard > Trainee Journey Module
      {
        path: 'dashboard',
        loadChildren: () =>
          import('./dashboard/dashboard.module').then(
            (m) => m.DashboardModule
          ),
        canLoad: [AuthGuardService],
      },
      {
        path: 'notification',
        loadChildren: () =>
          import('./notification/notification.module').then(
            (m) => m.NotificationModule
          ),
        canLoad: [AuthGuardService],
      },
      {
        path: 'generation',
        loadChildren: () =>
          import('./generation/generation.module').then(
            (m) => m.GenerationModule
          ),
        canLoad: [AuthGuardService],
      }, {
        path: 'potential-trainees',
        loadChildren: () =>
          import('./potential-trainees/potential-trainees.module').then(
            (m) => m.PotentialTraineesModule
          ),
        canLoad: [AuthGuardService],
      },
      {
        path: 'coursera-reports',
        loadChildren: () =>
          import('./Coursera Reports/coursera-report.module').then(
            (m) => m.CourseraReportModule
          ),
        canLoad: [AuthGuardService],
      },
      {
        path: 'profile-manage',
        loadChildren: () =>
          import('./profile-manage/profile-manage.module').then(
            (m) => m.ProfileManageModule
          ),
        canLoad: [AuthGuardService],
      },
      {
        path: 'program-design',
        loadChildren: () =>
          import('./annual-planing/annual-planing.module').then(
            (m) => m.AnnualPlaningModule
          ),
        canLoad: [AuthGuardService],
      },
      {
        path: 'association-management',
        loadChildren: () =>
          import('./association-management/association-management.module').then(
            (m) => m.AssociationManagementModule
          ),
        canLoad: [AuthGuardService],
      },
      {
        path: 'criteria-management',
        loadChildren: () =>
          import('./criteria-management/criteria-management.module').then(
            (m) => m.CriteriaManagementModule
          ),
        canLoad: [AuthGuardService],
      },
      {
        path: 'workflow-management',
        loadChildren: () =>
          import('./workflow-management/workflow-management.module').then(
            (m) => m.WorkflowManagementModule
          ),
        canLoad: [AuthGuardService],
      },
      {
        path: 'payment',
        loadChildren: () =>
          import('./payment/payment.module').then(
            (m) => m.PaymentModule
          ),
        canLoad: [AuthGuardService],
      },
      {
        path: 'process-configuration',
        loadChildren: () =>
          import('./process-configuration/process-configuration.module').then(
            (m) => m.ProcessConfigurationModule
          ),
        canLoad: [AuthGuardService],
      },
      {
        path: 'tsp-trainee-portal',
        loadChildren: () =>
          import('./tsp-trainee-portal/tsp-trainee-portal.module').then(
            (m) => m.TSPTraineePortalModule
          ),
        canLoad: [AuthGuardService],
      },
      {
        path: 'device-management',
        loadChildren: () =>
          import('./device-management/device-management.module').then(
            (m) => m.DeviceManagementModule
          ),
        canLoad: [AuthGuardService],
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule { }
