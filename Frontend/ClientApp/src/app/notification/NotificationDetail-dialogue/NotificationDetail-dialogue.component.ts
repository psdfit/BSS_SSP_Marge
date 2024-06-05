import { Component, Inject, OnInit, Pipe } from '@angular/core';
import { CommonSrvService } from 'src/app/common-srv.service';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';


@Component({
  selector: 'app-NotificationDetail-dialogue.component',//TSPColorHistorydialogueComponent
  templateUrl: './NotificationDetail-dialogue.component.html',
  styleUrls: ['./NotificationDetail-dialogue.component.scss']
})
export class NotificationDetaildialoguecomponent implements OnInit {
  NotificationDetail: any;
  CheckIfTable:boolean=false;
  @Pipe({name: 'safeHtml'})
  Body:string;
  constructor(private http: CommonSrvService, public dialogRef: MatDialogRef<NotificationDetaildialoguecomponent>,
  @Inject(MAT_DIALOG_DATA) public data: any) { }
  ngOnInit(): void {
    debugger;
    this.NotificationDetail=this.data.data;
    if(this.NotificationDetail.Body.indexOf('<table><thead>') !== -1){
      this.CheckIfTable=true
    }
    this.ReadNotification()
  }
  ReadNotification() {
    this.http.postJSON('api/NotificationMap/ReadNotification', { 'NotificationDetailID': this.data.data.NotificationDetailID })
              .subscribe((d: any) => {

              },
              
      error => {
      }
    );
  }
  onNoClick(): void {
    this.dialogRef.close(false);
  }
}

