import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';


import { ProcessConfigurationRoutingModule } from './process-configuration-routing.module';
import { ProcessScheduleComponent } from './process-schedule/process-schedule.component';
import { SharedModule } from '../shared/shared.module';


@NgModule({
  declarations: [ProcessScheduleComponent],
  imports: [
    SharedModule,
    ProcessConfigurationRoutingModule
  ],schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class ProcessConfigurationModule { }
