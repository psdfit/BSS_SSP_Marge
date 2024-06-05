
import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from "@angular/core";
import { SharedModule } from "../shared/shared.module";
import { CourseraReportRoutingModule } from './coursera-report-routing.module';
import { CourseraReportComponent } from './coursera-report/coursera-report.component';

@NgModule({
  declarations: [CourseraReportComponent],
  imports: [
    SharedModule,
    CourseraReportRoutingModule
  ],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],

})
export class CourseraReportModule { }
