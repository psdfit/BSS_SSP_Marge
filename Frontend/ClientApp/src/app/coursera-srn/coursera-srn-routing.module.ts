import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuardService } from '../security/auth-guard.service';
import { CourseraSrnComponent } from './coursera-srn/coursera-srn.component';


const routes: Routes = [
  { path: "coursera-srn", component: CourseraSrnComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CourseraSrnRoutingModule { }
