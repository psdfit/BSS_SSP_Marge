import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuardService } from '../security/auth-guard.service';
import { ComplaintHandlingComponent } from './complaint-handling/complaint-handling.component';
import { ComplaintComponent } from './complaint/complaint.component';
import { complaintUserComponent } from './complaintUser/complaintUser.component';


const routes: Routes = [
 // { path: 'complaint', component: ComplaintComponent },
 // { path: 'complainthandling', component:ComplaintHandlingComponent },
 // { path: 'complaintuser', component:complaintUserComponent },



  {
    path: "complaint",
    component: ComplaintComponent,
    canActivate: [AuthGuardService],
    data: {
      icon: "verified_user",
      inMenu: true,
      title: "Certification Authority",
    },
  },
  {
    path: "complainthandling",
    component: ComplaintHandlingComponent,
    canActivate: [AuthGuardService],
    data: {
      icon: "verified_user",
      inMenu: true,
      title: "Certification Authority",
    },
  },
  {
    path: "complaintuser",
    component: complaintUserComponent,
    canActivate: [AuthGuardService],
    data: {
      icon: "verified_user",
      inMenu: true,
      title: "Certification Authority",
    },
  },

];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ComplaintModuleRoutingModule { }
