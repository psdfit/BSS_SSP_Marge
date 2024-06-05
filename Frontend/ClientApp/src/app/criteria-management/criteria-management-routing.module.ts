
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuardService } from '../security/auth-guard.service';
import { CriteriaTemplateComponent } from './criteria-template/criteria-template.component';


const routes: Routes = [
  {
    path: "criteria-template",
    component: CriteriaTemplateComponent,
    canActivate: [AuthGuardService],
    data: {
      icon: "verified_user",
      inMenu: true,
      title: "Criteria Template",
    },
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CriteriaManagementRoutingModule { }
