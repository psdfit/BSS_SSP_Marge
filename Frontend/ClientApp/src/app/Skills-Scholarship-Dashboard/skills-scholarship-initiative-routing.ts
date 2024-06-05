import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { SkillsScholarshipComponent } from './skills-scholarship-initiative/skills-scholarship-initiative.component';
import { SkillsScholarshipReportComponent } from './skills-scholarship-report/skills-scholarship-report.component';
import { AuthGuardService } from '../security/auth-guard.service';




const routes: Routes = [{
  // path: 'administration', component: AppLayoutComponent, children: [{
  path: "SSIB2C", component: SkillsScholarshipComponent, canActivate: [AuthGuardService],
  data:
  {
    icon: "verified_user",
    inMenu: true,
    title: "Skills Scholarship Initiative"
  },
},
{
  path: "SSIB2CReport", component: SkillsScholarshipReportComponent, canActivate: [AuthGuardService],
  data:
  {
    icon: "verified_user",
    inMenu: true,
    title: "Skills Scholarship Report"
  },
}  
];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})

export class SkillsScholarshipRoutingModule { }
