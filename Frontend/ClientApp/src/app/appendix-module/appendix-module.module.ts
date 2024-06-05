/* **** Aamer Rehman Malik *****/
import { CommonModule } from '@angular/common';
import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { SharedModule } from '../shared/shared.module';
import { AppendixRoutingModule } from './appendix-module-routing';
import { AppendixComponent } from './appendix/appendix.component';
import { ClassComponent } from './class/class.component';
import { InstructorComponent } from './Instructor/Instructor.component';
import { TSPComponent } from './tsp/tsp.component';

@NgModule({
  declarations: [
    AppendixComponent
    , TSPComponent
    , ClassComponent
    , InstructorComponent
  ]
  , imports: [
    CommonModule
    , SharedModule
    , AppendixRoutingModule
  ]
  , schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class AppendixModuleModule { }
