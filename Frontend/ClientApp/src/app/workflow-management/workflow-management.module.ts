import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { WorkflowManagementRoutingModule } from './workflow-management-routing.module';
import { WorkflowComponent } from './workflow/workflow.component';
import { SharedModule } from '../shared/shared.module';


@NgModule({
  declarations: [WorkflowComponent],
  imports: [ SharedModule,WorkflowManagementRoutingModule]
  ,schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class WorkflowManagementModule { }
