import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ActivatedRoute } from '@angular/router';
import { CommonSrvService } from 'src/app/common-srv.service';
import { UserRightsModel } from 'src/app/master-data/users/users.component';
import { DialogueService } from 'src/app/shared/dialogue.service';
import { merge } from 'rxjs';
import { startWith, switchMap } from 'rxjs/operators';
import { SearchFilter } from '../../shared/Interfaces';


@Component({
  selector: 'app-view-all-notification',
  templateUrl: './view-all-notification.component.html',
  styleUrls: ['./view-all-notification.component.scss']
})
export class ViewAllNotificationComponent implements OnInit, AfterViewInit {
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild('PageNotification') PageNotification: MatPaginator;
  ViewAllNotification: MatTableDataSource<any>;
  ViewOpenNotification: MatTableDataSource<any>;
  ViewAllNotificationform: FormGroup;
  formrights: UserRightsModel;
  ViewAllNotificationsArray: any;
  resultsNotificationLength = 50;
  ViewAllOpennotificationDiv = false;
  EnText = 'View All Notification';
  error: string;
  filters: SearchFilter = { OID: this.http.OID.value };
  ComplaintTypeIDFilter = new FormControl(0);
  displayedColumns = ['NotificationName', 'Subject', 'PushNotificationDate', 'History'];
  Notifications = [];
  selectedFile: string;
  ext: string;
  resultsLength = 0;
  base64 = [];
  NotificationDetailID: number;
  PageNo = 1;
  RecordParPage = 5;
  constructor(private route: ActivatedRoute, private http: CommonSrvService, public dialogueService: DialogueService) {
    this.formrights = http.getFormRights();
    route.params.subscribe(
      params => { this.NotificationDetailID = params.id; });
  }

  ngOnInit(): void {
    // debugger;
    if (this.NotificationDetailID > 0) {
      this.GetNotificationDetasilByID(this.NotificationDetailID)
      this.ReadNotification(this.NotificationDetailID)
    }
    this.http.setTitle('View All Notification');
    // this.GetNotificationsDetail();

  }

  ngAfterViewInit() {
    // debugger;
    this.http.OID.subscribe(
      OID => {
        this.filters.OID = OID;
        this.initPagedData();
      });
  }









  initPagedData() {
    // debugger;
    this.sort.sortChange.subscribe(() => this.paginator.pageIndex = 0);
    this.paginator.pageSize = 5;
    merge(this.sort.sortChange, this.paginator.page).pipe(
      startWith({}),
      switchMap(() => {
        const pagedModel = {
          PageNo: this.paginator.pageIndex + 1
          , PageSize: this.paginator.pageSize
          , SortColumn: this.sort.active
          , SortOrder: this.sort.direction
          , SearchColumn: ''
          , SearchValue: ''
        };
        return this.getPagedData(pagedModel);
      })).subscribe(data => {
        // debugger;
        this.ViewAllNotificationsArray = data;
        this.resultsLength = data[0].TotleCount;
      });
  }





  getPagedData(pagingModel) {
    // tslint:disable-next-line: max-line-length
    return this.http.getJSON('api/Notifications/GetNotificationsDetail?PageNo=' + pagingModel.PageNo + '&RecordParPage=' + pagingModel.PageSize);
  }


  ReadNotification(NotificationDetailID) {
    this.http.postJSON('api/NotificationMap/ReadNotification', { NotificationDetailID })
      .subscribe((d: any) => {

      },

        error => {
        }
      );
  }
  GetNotificationDetasilByID(NotificationDetailID) {
    // debugger;
    this.http.getJSON('api/NotificationMap/GetNotificationDetasilByID?NotificationDetailID=' + NotificationDetailID).subscribe((d: any) => {
      // debugger;
      this.populateOpenNotification(d);
      if (d.length > 0) {
        this.ViewAllOpennotificationDiv = true;
      }
    }, error => this.error = error // error path
    );
  }
  GetNotificationsDetail() {
    // debugger;
    this.http.getJSON('api/Notifications/GetNotificationsDetail?PageNo=' + this.PageNo + '&RecordParPage=' + this.RecordParPage)
      .subscribe((d: any) => {
        // debugger;
        this.populateViewAllNotification(d);
      }, error => this.error = error // error path
      );
  }
  // openDialogForNotificationDetail
  openDialogForNotificationDetail(data: any): void {
    // debugger;
    this.dialogueService.openDialogForNotificationDetail(data);
  }
  applyFilter(filterValue: string) {
    filterValue = filterValue.trim(); // Remove whitespace
    filterValue = filterValue.toLowerCase(); // Datasource defaults to lowercase matches
    this.ViewAllNotification.filter = filterValue;
    // this.ViewOpenNotification.filter = filterValue;
  }
  populateViewAllNotification(d: any) {
    this.ViewAllNotification = new MatTableDataSource(d);
    this.ViewAllNotification.paginator = this.paginator;
    this.ViewAllNotification.sort = this.sort;
  }
  populateOpenNotification(d: any) {
    this.ViewOpenNotification = new MatTableDataSource(d);
    /*   this.ViewOpenNotification.paginator = this.paginator;
      this.ViewOpenNotification.sort = this.sort; */
  }
  openHistoryDialogue(data: any): void {
    // debugger;
    this.dialogueService.openComplaintHistoryDialogue(data);
  }
}
