import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ClassStatusRoutingModule } from './class-status-routing';
import { ClassStatusComponent } from './class-status/class-status.component';
import { SharedModule } from '../shared/shared.module';
import { ClassProceedingStatusComponent } from './class-proceeding-status/class-proceeding-status.component';
import { CsuDialogueComponent } from './class-status/csu-dialogue/csu-dialogue.component';

@NgModule({
  declarations: [ClassStatusComponent, ClassProceedingStatusComponent, CsuDialogueComponent],
  imports: [
      CommonModule,
      SharedModule,
      ClassStatusRoutingModule

    ], schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class ClassStatusModule { }
