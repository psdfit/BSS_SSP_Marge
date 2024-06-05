import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from '../../../environments/environment';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { CommonSrvService } from '../../common-srv.service';

import { ModelBase } from '../../shared/ModelBase';
import { UserRightsModel } from '../../master-data/users/users.component';

@Component({
  selector: 'app-notifications',
  templateUrl: './notifications.component.html',
  styleUrls: ['./notifications.component.scss']
})
export class NotificationsComponent implements OnInit {
  notificationsform: FormGroup;
  title: string; savebtn: string;
  displayedColumns = ['NotificationName', 'EventAction', 'Subject', 'Body', 'InActive', "Action"];
  notifications: MatTableDataSource<any>;

  formrights: UserRightsModel;
  EnText: string = "Notification";
  error: String;
  usernotifications = []; filteredusers = []; appforms = [];
  query = {
    order: 'NotificationID',
    limit: 5,
    page: 1
  };
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  working: boolean;
  constructor(private fb: FormBuilder, private ComSrv: CommonSrvService) {
    this.notificationsform = this.fb.group({
      NotificationID: 0,
      NotificationName: ["", Validators.required],
      EventAction: ["", Validators.required],
      Subject: ["", Validators.required],
      Body: ["", Validators.required],
      InActive: ""
    }, { updateOn: "blur" });
    this.notifications = new MatTableDataSource([]);
    this.formrights = ComSrv.getFormRights();
  }


  GetData() {
    this.ComSrv.getJSON('api/Notifications/GetNotifications').subscribe((d: any) => {
      this.notifications = new MatTableDataSource(d[0]);
      //this.usernotifications = d[1];
      this.appforms = d[1];
      this.notifications.paginator = this.paginator;
      this.notifications.sort = this.sort;
    }, error => this.error = error // error path
    );
  }
  ngOnInit() {
    this.ComSrv.setTitle("Notifications");
    this.title = "Add New ";
    this.savebtn = "Save ";
    this.GetData();
  }

  Submit() {
    if (!this.notificationsform.valid)
      return;
    this.working = true;
   /*  this.filteredusers = this.usernotifications.filter(subsec => subsec.IsSelected === true);
    console.log(this.filteredusers)
    this.notificationsform.value["UserNotifications"] = this.filteredusers; */
    this.ComSrv.postJSON('api/Notifications/Save', this.notificationsform.value)
      .subscribe((d: any) => {
        this.notifications = new MatTableDataSource(d);
        this.notifications.paginator = this.paginator;
        this.notifications.sort = this.sort;
        this.ComSrv.openSnackBar(this.NotificationID.value > 0 ? environment.UpdateMSG.replace("${Name}", this.EnText) : environment.SaveMSG.replace("${Name}", this.EnText));
        this.reset();
        this.title = "Add New ";
        this.savebtn = "Save ";
      },
        error => this.error = error // error path
        , () => {
          this.working = false;

        });


  }
  reset() {
    this.notificationsform.reset();
    this.NotificationID.setValue(0);
    this.usernotifications = this.appforms;

    this.title = "Add New ";
    this.savebtn = "Save ";
  }
  toggleEdit(row) {
    this.NotificationID.setValue(row.NotificationID);
    this.NotificationName.setValue(row.NotificationName);
    this.EventAction.setValue(row.EventAction);
    this.Subject.setValue(row.Subject);
    this.Body.setValue(row.Body);
    this.InActive.setValue(row.InActive);
    this.ComSrv.getJSON('api/Notifications/GetUserNotifications/' + row.NotificationID).subscribe((REs: any) => {
      this.usernotifications = REs;
    }, error => this.error = error // error path
    )

    this.title = "Update ";
    this.savebtn = "Save ";
  }
  toggleActive(row) {
    this.ComSrv.confirm().subscribe(result => {
      if (result) {
        this.ComSrv.postJSON('api/Notifications/ActiveInActive', { 'NotificationID': row.NotificationID, 'InActive': row.InActive })
          .subscribe((d: any) => {
            this.ComSrv.openSnackBar(row.InActive == true ? environment.InActiveMSG.replace("${Name}", this.EnText) : environment.ActiveMSG.replace("${Name}", this.EnText));
            // this.notifications =new MatTableDataSource(d);
          },
            error => this.error = error // error path
          );
      }
      else {
        row.InActive = !row.InActive;
      }
    });
  }
  applyFilter(filterValue: string) {
    filterValue = filterValue.trim(); // Remove whitespace
    filterValue = filterValue.toLowerCase(); // Datasource defaults to lowercase matches
    this.notifications.filter = filterValue;
  }

  get NotificationID() { return this.notificationsform.get("NotificationID"); }
  get NotificationName() { return this.notificationsform.get("NotificationName"); }
  get EventAction() { return this.notificationsform.get("EventAction"); }
  get Subject() { return this.notificationsform.get("Subject"); }
  get Body() { return this.notificationsform.get("Body"); }
  get InActive() { return this.notificationsform.get("InActive"); }
}
export class NotificationsModel extends ModelBase {
  NotificationID: number;
  NotificationName: string;
  EventAction: string;
  Subject: string;
  Body: string;

}
