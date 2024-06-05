import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { PSPEmpComponent } from './psp-employment/psp-employment.component';
import { AuthGuardService } from '../../app/security/auth-guard.service';
import { PSPTraineeListComponent } from './psp-trainees/psp-trainees-list.component';
import { TraineePSPAssignmentComponent } from './trainee-psp-assignment/trainee-psp-assignment.component';
import { PSPBatchDialogueComponent } from './psp-batch-dialogue/psp-batch-dialogue.component';
import { PSPAssignedTraineesListComponent } from './psp-assigned-trainees-list/psp-assigned-trainees-list.component';




const routes: Routes = [
  {
        path: "psp-employment",
    component: PSPEmpComponent,
    canActivate: [AuthGuardService],
    data: { icon: "verified_user", inMenu: true, title: "PSP" }
  },
  {
    path: "psp-employment/:PSPBatchID/:traineeid",
    component: PSPEmpComponent,
    canActivate: [AuthGuardService],
    data: { icon: "verified_user", inMenu: false, title: "PSP" }
  },
  {
        path: "psp-trainees-list",
    component: PSPTraineeListComponent,
    canActivate: [AuthGuardService],
    data: { icon: "verified_user", inMenu: true, title: "PSP Trainees" }
  },
  {
    path: "trainee-psp-assignment",
    component: TraineePSPAssignmentComponent,
    canActivate: [AuthGuardService],
    data: { icon: "verified_user", inMenu: true, title: "Trainees PSP Assignment" }
  },
  {
    path: "psp-assigned-trainees",
    component: PSPAssignedTraineesListComponent,
    canActivate: [AuthGuardService],
    data: { icon: "verified_user", inMenu: true, title: "PSP Assigned Trainees" }
  }
 
];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PSPEmpRoutingModule { }
