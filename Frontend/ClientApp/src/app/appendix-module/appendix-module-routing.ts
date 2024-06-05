import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AppendixComponent } from './appendix/appendix.component';
import { AuthGuardService } from '../../app/security/auth-guard.service';


const routes: Routes = [
  {
    path: "appendix"
    , component: AppendixComponent
    , canActivate: [AuthGuardService]
    , data: {
      icon: "verified_user"
      , inMenu: true
      , title: "Create Appendix"
    }
}
];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AppendixRoutingModule { }
