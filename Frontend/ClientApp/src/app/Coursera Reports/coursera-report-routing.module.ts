import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuardService } from '../security/auth-guard.service';
import { CourseraReportComponent } from './coursera-report/coursera-report.component';


const routes: Routes = [
  { path: "coursera-reports", component: CourseraReportComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CourseraReportRoutingModule { }
