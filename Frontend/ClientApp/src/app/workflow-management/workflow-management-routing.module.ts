import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { WorkflowComponent } from './workflow/workflow.component';
import { AuthGuardService } from '../security/auth-guard.service';


const routes: Routes = [
  {
    path: "workflow",
    component: WorkflowComponent,
    canActivate: [AuthGuardService],
    data: {
      icon: "verified_user",
      inMenu: true,
      title: "Workflow",
    },
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class WorkflowManagementRoutingModule { }
