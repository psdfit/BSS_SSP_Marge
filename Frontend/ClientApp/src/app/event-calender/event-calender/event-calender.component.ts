/* **** Aamer Rehman Malik *****/
import { Component, TemplateRef, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { CalendarEvent, CalendarEventAction, CalendarEventTimesChangedEvent, CalendarView } from 'angular-calendar';
import { isSameDay, isSameMonth } from 'date-fns';
import { Subject } from 'rxjs';
import { CommonSrvService } from '../../common-srv.service';
//import { EventCalenderComponent } from '../event-calender/event-calender.component';
import { VisitPlanDialogComponent, VisitPlanModel } from '../../master-sheet/visit-plan-dialog/visit-plan-dialog.component';

const colors: any = {
  red: {
    primary: '#ad2121',
    secondary: '#FAE3E3'
  },
  blue: {
    primary: '#1e90ff',
    secondary: '#D1E8FF'
  },
  yellow: {
    primary: '#e3bc08',
    secondary: '#FDF1BA'
  }
};

@Component({
  selector: 'hrapp-event-calender',
  // changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './event-calender.component.html',
  styleUrls: ['./event-calender.component.scss'],
  //providers: [CalendarMomentDateFormatter: MO]
})

export class EventCalenderComponent {
  @ViewChild('modalContent', { static: true }) modalContent: TemplateRef<any>;

  view: CalendarView = CalendarView.Month;

  CalendarView = CalendarView;

  viewDate: Date = new Date();

    visitPlan: MatTableDataSource<any>;

    userevents = [];

  modalData: {
    action: string;
    event: CalendarEvent;
  };

  actions: CalendarEventAction[] = [
    {
      label: '<i class="fa fa-fw fa-pencil"></i>',
      a11yLabel: 'Edit',
      onClick: ({ event }: { event: CalendarEvent }): void => {
        this.handleEvent('Edited', event);
      }
    },
    {
      label: '<i class="fa fa-fw fa-times"></i>',
      a11yLabel: 'Delete',
      onClick: ({ event }: { event: CalendarEvent }): void => {
        this.events = this.events.filter(iEvent => iEvent !== event);
        this.handleEvent('Deleted', event);
      }
    }
  ];

  refresh: Subject<any> = new Subject();

  events: CalendarEvent[] = [
    //  {
    //  start: subDays(startOfDay(new Date()), 1),
    //  end: addDays(new Date(), 1),
    //  title: 'A 3 day event',
    //  color: colors.red,
    //  actions: this.actions,
    //  allDay: true,
    //  resizable: {
    //    beforeStart: true,
    //    afterEnd: true
    //  },
    //  draggable: true
    //},
    //{
    //  start: startOfDay(new Date()),
    //  title: 'An event with no end date',
    //  color: colors.yellow,
    //  actions: this.actions
    //},
    //{
    //  start: subDays(endOfMonth(new Date()), 3),
    //  end: addDays(endOfMonth(new Date()), 3),
    //  title: 'A long event that spans 2 months',
    //  color: colors.blue,
    //  allDay: true
    //},
    //{
    //  start: addHours(startOfDay(new Date()), 2),
    //  end: addHours(new Date(), 2),
    //  title: 'A draggable and resizable event',
    //  color: colors.yellow,
    //  actions: this.actions,
    //  resizable: {
    //    beforeStart: true,
    //    afterEnd: true
    ////  },
    //  draggable: true
    //}

  ];

  activeDayIsOpen: boolean = true;

  constructor(private modal: NgbModal, private ComSrv: CommonSrvService, public dialog: MatDialog) {
      this.ComSrv.setTitle("Event Calendar");
      this.GetVisitPlanData(this.ComSrv.getUserDetails().UserID);
      //this.GetVisitPlanByUser(this.ComSrv.getUserDetails().UserID);
    }

    GetVisitPlanByUser(UserID) {
        this.ComSrv.postJSON('api/UserEventMap/RD_UserEventMapBy', { "UserID": UserID }).subscribe((d: any) => {
            this.userevents = d;
            //this.filteredevents = this.userevents.filter(subsec => subsec.UserID === this.UserID.value);

            //this.GetVisitPlanData()
        }, // error path
        );
    }
    

  GetVisitPlanData(UserID) {
    this.ComSrv.postJSON('api/VisitPlan/GetPlanBy', { "UserID": UserID }).subscribe((res: VisitPlanModel[]) => {
      res.forEach((el: VisitPlanModel) => {
        this.events = [...this.events, {
          start: new Date(el.VisitStartDate),
            end: new Date(el.VisitStartDate),
          title: el.Comments,
          color: colors.blue,
          allDay: true
        }];
      });
    });
  }

  openDialog(): void {
    let viewDate = new Date();

    let time = this.viewDate.getTime();
    //Check if timezoneOffset is positive or negative
    if (this.viewDate.getTimezoneOffset() <= 0) {
      //Convert timezoneOffset to hours and add to Date value in milliseconds
      let final = time + (Math.abs(this.viewDate.getTimezoneOffset() * 60000));
      //Convert from milliseconds to date and convert date back to ISO string
      this.viewDate = new Date(final);
      //this.viewDate.setDate(final);
      //this.viewDate =final;
    } else {
      let final = time + (-Math.abs(this.viewDate.getTimezoneOffset() * 60000));
      //this.viewDate.setDate( new Date(final).toISOString();
      //this.viewDate.setDate(final);
      this.viewDate = new Date(final);
    }

    const dialogRef = this.dialog.open(VisitPlanDialogComponent, {
      //minWidth: '1000px',
      //  minHeight: '100%',
     height:'90%',
      //data: JSON.parse(JSON.stringify(row))

        data: { "ClassID": null, "IsMasterSheet": false, "VisitStartDate": this.viewDate, "UserID": this.ComSrv.getUserDetails().UserID }
      //this.GetVisitPlanData(data)
    })
    dialogRef.afterClosed().subscribe(result => {
      //console.log(result);
      this.visitPlan = result;
      //this.submitVisitPlan(result);
    })
  }

  dayClicked({ date, events }: { date: Date; events: CalendarEvent[] }): void {
    if (isSameMonth(date, this.viewDate)) {
      if (
        (isSameDay(this.viewDate, date) && this.activeDayIsOpen === true) ||
        events.length === 0
      ) {
        this.activeDayIsOpen = false;
      } else {
        this.activeDayIsOpen = true;
      }
      this.viewDate = date;
    }
  }

  eventTimesChanged({
    event,
    newStart,
    newEnd
  }: CalendarEventTimesChangedEvent): void {
    this.events = this.events.map(iEvent => {
      if (iEvent === event) {
        return {
          ...event,
          start: newStart,
          end: newEnd
        };
      }
      return iEvent;
    });
    this.handleEvent('Dropped or resized', event);
  }

  handleEvent(action: string, event: CalendarEvent): void {
    this.modalData = { event, action };
    this.modal.open(this.modalContent, { size: 'lg' });
  }

  //addEvent(): void {
  //  this.events = [
  //    ...this.events,
  //    {
  //      title: 'New event',
  //      start: startOfDay(new Date()),
  //        end: endOfDay(new Date()),
  //        user: el.UserID,
  //        class: el.ClassID,
  //      color: colors.red,
  //      draggable: true,
  //      resizable: {
  //        beforeStart: true,
  //        afterEnd: true
  //      }
  //    }
  //  ];
  //}

  //deleteEvent(eventToDelete: CalendarEvent) {
  //  this.events = this.events.filter(event => event !== eventToDelete);
  //}

  setView(view: CalendarView) {
    this.view = view;
  }

  closeOpenMonthViewDay() {
    this.activeDayIsOpen = false;
  }
}
