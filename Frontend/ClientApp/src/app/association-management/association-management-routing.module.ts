import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuardService } from '../security/auth-guard.service';
import { InitiateAssociationComponent } from './initiate-association/initiate-association.component';
import { AssociationEvaluationComponent } from './association-evaluation/association-evaluation.component';
import { TspAssignmentComponent } from './tsp-assignment/tsp-assignment.component';
import { AssociationDetailComponent } from './association-detail/association-detail.component';
import { AssociationSubmissionComponent } from './association-submission/association-submission.component';


const routes: Routes = [
  {
    path: "initiate-association",
    component: InitiateAssociationComponent,
    canActivate: [AuthGuardService],
    data: {
      icon: "verified_user",
      inMenu: true,
      title: "Initiate Association",
    },
  },
  {
    path: "association-evaluation",
    component: AssociationEvaluationComponent,
    canActivate: [AuthGuardService],
    data: {
      icon: "verified_user",
      inMenu: true,
      title: "Association Evaluation",
    },
  },

  {
    path: "tsp-assignment",
    component: TspAssignmentComponent,
    canActivate: [AuthGuardService],
    data: {
      icon: "verified_user",
      inMenu: true,
      title: "TSP Assignment",
    },
  },
  {
    path: "association-detail",
    component: AssociationDetailComponent,
    canActivate: [AuthGuardService],
    data: {
      icon: "verified_user",
      inMenu: true,
      title: "Association Detail",
    },
  },
  {
    path: "association-submission",
    component: AssociationSubmissionComponent,
    canActivate: [AuthGuardService],
    data: {
      icon: "verified_user",
      inMenu: true,
      title: "Association Submission",
    },
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AssociationManagementRoutingModule { }
