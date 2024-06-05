import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { CriteriaManagementRoutingModule } from './criteria-management-routing.module';
import { CriteriaTemplateComponent } from './criteria-template/criteria-template.component';
import { SharedModule } from '../shared/shared.module';


@NgModule({
  declarations: [CriteriaTemplateComponent],
   imports: [SharedModule, CriteriaManagementRoutingModule],
   schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class CriteriaManagementModule { }
