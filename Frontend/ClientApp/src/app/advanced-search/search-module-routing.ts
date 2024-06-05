import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { SearchComponent } from './advance-search/search.component';
import { TraineeProfileComponent } from './trainee-profile/trainee-profile.component';
import { InstructorProfileComponent } from './instructor-profile/instructor-profile.component';
import { ClassDetailComponent } from './class-detail/class-detail.component';
import { TSPDetailComponent } from './tsp-detail/tsp-detail.component';
import { SchemeDetailComponent } from './scheme-detail/scheme-detail.component';
import { MPRDetailComponent } from './mpr-detail/mpr-detail.component';
import { PRNDetailComponent } from './prn-detail/prn-detail.component';
import { SRNDetailComponent } from './srn-detail/srn-detail.component';
import { InvoiceDetailComponent } from './invoice-detail/invoice-detail.component';
import { AuthGuardService } from '../security/auth-guard.service';


const routes: Routes = [
  {
    path: "advanced-search",
    component: SearchComponent,
    canActivate: [AuthGuardService],
    data: {
      icon: "verified_user",
      inMenu: true,
      title: "Advance Search"
    }
  },
  {
    path: "trainee-profile",
    component: TraineeProfileComponent,
    canActivate: [AuthGuardService],
    data: {
      icon: "verified_user",
      inMenu: true,
      title: "Trainee Detail"
    }
  },
  {
    path: "instructor-profile",
    component: InstructorProfileComponent,
    canActivate: [AuthGuardService],
    data: {
      icon: "verified_user",
      inMenu: true,
      title: "Instructor Detail"
    }
  },
  {
    path: "class-detail",
    component: ClassDetailComponent,
    canActivate: [AuthGuardService],
    data: {
      icon: "verified_user",
      inMenu: true,
      title: "Class Detail"
    }
  },
  {
    path: "tsp-detail",
    component: TSPDetailComponent,
    canActivate: [AuthGuardService],
    data: {
      icon: "verified_user",
      inMenu: true,
      title: "TSP Detail"
    }
  },
  {
    path: "scheme-detail",
    component: SchemeDetailComponent,
    canActivate: [AuthGuardService],
    data: {
      icon: "verified_user",
      inMenu: true,
      title: "Scheme Detail"
    }
  },
  {
    path: "mpr-detail",
    component: MPRDetailComponent,
    canActivate: [AuthGuardService],
    data: {
      icon: "verified_user",
      inMenu: true,
      title: "MPR Detail"
    }
  },
  {
    path: "prn-detail",
    component: PRNDetailComponent,
    canActivate: [AuthGuardService],
    data: {
      icon: "verified_user",
      inMenu: true,
      title: "PRN Detail"
    }
  },
  {
    path: "srn-detail",
    component: SRNDetailComponent,
    canActivate: [AuthGuardService],
    data: {
      icon: "verified_user",
      inMenu: true,
      title: "SRN Detail"
    }
  },
  {
    path: "invoice-detail",
    component: InvoiceDetailComponent,
    canActivate: [AuthGuardService],
    data: {
      icon: "verified_user",
      inMenu: true,
      title: "Invoice Detail"
    }
  }
];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdvanceSearchRoutingModule { }
