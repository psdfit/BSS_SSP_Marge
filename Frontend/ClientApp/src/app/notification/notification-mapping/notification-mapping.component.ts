import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from '../../../environments/environment';
import { FormGroup, FormBuilder, Validators, FormControl } from '@angular/forms';
import { CommonSrvService } from '../../common-srv.service';
import { ModelBase } from '../../shared/ModelBase';
import { UserRightsModel } from '../../master-data/users/users.component';
import { MatTabGroup } from '@angular/material/tabs';
@Component({
  selector: 'app-notification-mapping',
  templateUrl: './notification-mapping.component.html',
  styleUrls: ['./notification-mapping.component.scss']
})
export class NotificationMappingComponent implements OnInit {
  @ViewChild("tabGroup") tabGroup: MatTabGroup;
  notificationsMappingform: FormGroup;
  title: string; savebtn: string;
  displayedColumns = ['NotificationName', 'ProcessKey', 'FullName',"Action"];
  notificationsHandling: MatTableDataSource<any>;

  formrights: UserRightsModel;
  EnText: string = "Notification Mapping";
  error: String;
  Users: any[] = [];
  ApprovalProcess:any[]=[];
  GetNotifications:any[]=[];
  GetApprovalStatus:any[]=[];
  Steps: any[] = [];
  UserNotificationsInfo:any;
  isShownUserDDL: boolean = true ;
  isShownApprovalStatusDDL:boolean=true;
  isShownNotification: boolean = false ;
  isShownProcess: boolean = false ;
  SearchCls = new FormControl("");
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
    this.notificationsMappingform = this.fb.group({
      NotificationMapID: 0,
      NotificationID: ["", Validators.required],
      UserIDs: [""],
      ProcessKey: ["", Validators.required],
      NotificationName: [""],
      EventAction: [""],
      Subject: [""],
      Body: [""],
      FullName:[""],
      ApprovalStatusID:0,
      InActive: ""
    }, { updateOn: "blur" });
    this.notificationsHandling = new MatTableDataSource([]);
    this.formrights = ComSrv.getFormRights();
  }


  GetData() {
    this.ComSrv.getJSON('api/NotificationMap/GetNotificationMap').subscribe((d: any) => {
      debugger;
      this.notificationsHandling = new MatTableDataSource(d);
      this.notificationsHandling.paginator = this.paginator;
      this.notificationsHandling.sort = this.sort;
    }, error => this.error = error // error path
    );
  }
  ngOnInit() {
    this.ComSrv.setTitle("Notifications Mapping");
    this.GFetchUsersForCRUD();
    this.GetNotificationsForCURD();
    this.GetApprovalProcessForCRUD();
    this.title = "Add New ";
    this.savebtn = "Save ";
    this.GetData();
  }
  GFetchUsersForCRUD() {
    debugger;
    this.ComSrv.getJSON('api/Complaint/FetchUsers').subscribe((d: any) => {
      debugger;
      this.Users = d;
    }, error => this.error = error // error path
    );
  }
  GetNotificationsForCURD() {
    this.ComSrv.getJSON('api/Notifications/GetNotifications').subscribe((d: any) => {
     this.GetNotifications=d[0];
     debugger;
    }, error => this.error = error // error path
    );
  }
  GetApprovalProcessForCRUD() {
    this.ComSrv.getJSON('api/ApprovalProcess/GetApprovalProcess').subscribe((d: any) => {
      this.ApprovalProcess=d;
    }, error => this.error = error // error path
    );
  }
  EmptyCtrl() {
    this.SearchCls.setValue("");
  }
  GetNotificationsInfo(NotificationID:any){
    debugger;
    this.ComSrv.getJSON(`api/Notifications/RD_NotificationsByID?NotificationID=` + NotificationID).subscribe((d: any) => {
      debugger;
      if(d[0]!=null){
        this.isShownNotification=true
        this.NotificationName.setValue(d[0].NotificationName);
        this.EventAction.setValue(d[0].EventAction);
        this.Subject.setValue(d[0].Subject);
        this.Body.setValue(d[0].Body);
        
      } 
     
    }, error => this.error = error // error path
    );
  }
  GetProcessInfoByProcessKey(ProcessKey:any){
    debugger;
    //this.UserDDLShowHide(ProcessKey);
    this.ComSrv.getJSON(`api/NotificationMap/RD_GetProcessInfoByProcessKey?ProcessKey=` + ProcessKey).subscribe((d: any) => {
      debugger;
      if(d[0]!=null){
        this.NotificationMapID.setValue(d[0].NotificationMapID);
        this.NotificationID.setValue(d[0].NotificationID);
        this.NotificationName.setValue(d[0].NotificationName);
        this.EventAction.setValue(d[0].EventAction);
        this.Subject.setValue(d[0].Subject);
        this.Body.setValue(d[0].Body);
        //this.FullName.setValue(d[0].FullName);
        this.Steps = Array.from(d[0].UserID.split(','),Number);
        this.isShownNotification=true;
        //this.isShownProcess = true ;
        this.title = "Update ";
        this.savebtn = "Update ";
      }
      else{
        /* this.NotificationMapID.setValue(0);
        this.UserIDs.setValue('');
        this.NotificationID.setValue(''); */
        this.reset();
        this.ProcessKey.setValue(ProcessKey);
        this.isShownNotification=false;
        this.title = "Add New ";
        this.savebtn = "Save ";
      }
    
    }, error => this.error = error // error path
    );
  }
  Submit() {
    debugger;
    if (!this.notificationsMappingform.valid)
      return;
      this.working = true;
    this.ComSrv.postJSON('api/NotificationMap/Save', this.notificationsMappingform.value)
      .subscribe((d: any) => {
        debugger;
        if(d==true)
        {
          this.GetData();
          this.ComSrv.openSnackBar(this.NotificationMapID.value > 0 ? environment.UpdateMSG.replace("${Name}", this.EnText) : environment.SaveMSG.replace("${Name}", this.EnText));
          this.reset();
          this.title = "Add New ";
          this.savebtn = "Save ";
          this.working = false;
        }else{ this.ComSrv.ShowError("Sorry record already exists");}

        
      },
      (error) => {
        this.error = error.error;
        this.ComSrv.ShowError(error.error);
        
      });


  }
  reset() {
    this.notificationsMappingform.reset();
    this.NotificationID.setValue(0);
    this.isShownNotification=false
    this.isShownProcess = false ;
    this.usernotifications = this.appforms;
    this.title = "Add New ";
    this.savebtn = "Save ";
    }

  toggleEdit(row) {
    debugger;
    this.tabGroup.selectedIndex = TabGroup.notificationsMappingform;
    this.NotificationMapID.setValue(row.NotificationMapID);
    this.NotificationID.setValue(row.NotificationID);
    this.ProcessKey.setValue(row.ProcessKey);
    this.GetProcessInfoByProcessKey(row.ProcessKey)
      this.Steps = Array.from(row.UserID.split(','),Number);
    this.title = "Update ";
    this.savebtn = "Update ";
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
    this.notificationsHandling.filter = filterValue;
  }

  get NotificationID() { return this.notificationsMappingform.get("NotificationID"); }
  get NotificationMapID() { return this.notificationsMappingform.get("NotificationMapID"); }
  get UserIDs() { return this.notificationsMappingform.get("UserIDs"); }
  //get UserID() { return this.notificationsMappingform.get("UserID"); }
  get ProcessKey() { return this.notificationsMappingform.get("ProcessKey"); }
  get NotificationName() { return this.notificationsMappingform.get("NotificationName"); }
  get EventAction() { return this.notificationsMappingform.get("EventAction"); }
  get Subject() { return this.notificationsMappingform.get("Subject"); }
  get Body() { return this.notificationsMappingform.get("Body"); }
  get FullName() { return this.notificationsMappingform.get("FullName"); }
  get ApprovalStatusID() { return this.notificationsMappingform.get("ApprovalStatusID"); }
}


export class NotificationsModel extends ModelBase {
  NotificationID: number;
  NotificationMapID: number;
  UserIDs: number;
  ProcessKey: string;
  //UserID:number;

}
enum TabGroup {
  notificationsMappingform = 0,
}