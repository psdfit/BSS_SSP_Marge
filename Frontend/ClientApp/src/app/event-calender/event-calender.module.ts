import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { CommonModule } from '@angular/common';
import { EventCalenderRoutingModule } from './event-calender-routing';
import { EventCalenderComponent } from './event-calender/event-calender.component';
import { CalendarModule, DateAdapter } from 'angular-calendar';
import { adapterFactory } from 'angular-calendar/date-adapters/date-fns';
import { SharedModule } from '../shared/shared.module';

import { NgbModalModule } from '@ng-bootstrap/ng-bootstrap';

import { CallCenterComponent } from './call-center/call-center.component';
import { CallCetnerAgentDialogComponent } from './call-center-agent-dialog/call-center-agent-dialog.component';


//import * as moment from 'moment';
//import { adapterFactory } from 'angular-calendar/date-adapters/date-fns';
//export function momentAdapterFactory() {
//    return adapterFactory(moment);
//}

@NgModule({
    declarations: [EventCalenderComponent, CallCenterComponent, CallCetnerAgentDialogComponent],
  imports: [
    CommonModule

    , EventCalenderRoutingModule,
    NgbModalModule,
    CalendarModule.forRoot({
      provide: DateAdapter,
      useFactory: adapterFactory
    }), SharedModule
  ], schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class EventCalenderModule { }
