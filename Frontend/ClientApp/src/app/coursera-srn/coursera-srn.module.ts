
import { CommonModule } from '@angular/common';
import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from "@angular/core";
import { SharedModule } from "../shared/shared.module";
import { CourseraSrnRoutingModule } from './coursera-srn-routing.module';
import { CourseraSrnComponent } from './coursera-srn/coursera-srn.component';

@NgModule({
  //declarations: [CourseraReportComponent],
  imports: [
    CommonModule, SharedModule,
    CourseraSrnRoutingModule
  ],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  
})
export class CourseraSrnModule { }
