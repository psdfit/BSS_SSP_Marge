import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SkillsScholarshipComponent } from './skills-scholarship-initiative/skills-scholarship-initiative.component';
import { SkillsScholarshipRoutingModule } from './skills-scholarship-initiative-routing';
import { SharedModule } from '../shared/shared.module';
import { SkillsScholarshipReportComponent } from './skills-scholarship-report/skills-scholarship-report.component';
//import { VisitPlanDialogComponent } from './visit-plan-dialog/visit-plan-dialog.component';

@NgModule({
  declarations: [SkillsScholarshipComponent, SkillsScholarshipReportComponent],
  imports: [
    CommonModule
    , SharedModule
    , SkillsScholarshipRoutingModule
  ], schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class SkillsScholarshipModule { }
