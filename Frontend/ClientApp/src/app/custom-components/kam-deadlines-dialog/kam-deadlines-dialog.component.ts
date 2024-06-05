import { Component, OnInit, Inject, ViewChild, ChangeDetectorRef } from '@angular/core';
import { FormGroup, FormBuilder, FormControl, Validators, FormGroupDirective } from '@angular/forms';
import { CommonSrvService } from '../../common-srv.service';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { EnumUserLevel, EnumBusinessRuleTypes } from '../../shared/Enumerations';
import { UsersModel } from '../../master-data/users/users.component';
import { IOrgConfig } from '../../registration/Interface/IOrgConfig';
import { RTPDialogComponent } from '../../inception-report/rtp/rtp.component';
import { EnumClassStatus, EnumTSPColorType } from '../../shared/Enumerations';
import { SelectionModel } from '@angular/cdk/collections';
import * as _moment from 'moment';
import { environment } from '../../../environments/environment';

const moment = _moment
import { Moment } from 'moment';

@Component({
  selector: 'app-kam-deadlines-dialog',
  templateUrl: './kam-deadlines-dialog.component.html',
  styleUrls: ['./kam-deadlines-dialog.component.scss']
})
export class KAMDeadlinesDialogComponent implements OnInit {
  editorform: FormGroup;
  month = new FormControl(moment());
  currentUser: UsersModel;
  error: String;
  env = environment;

  deadlines: any[];
  deadlinesFinalResult: any[];
  
  selection = new SelectionModel<any>(true, []);


  query = {
    order: 'ClassID',
    limit: 5,
    page: 1
  };

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;



  constructor(private http: CommonSrvService, public dialog: MatDialog, private fb: FormBuilder, private cdr: ChangeDetectorRef, public dialogRef: MatDialogRef<KAMDeadlinesDialogComponent>, private router: Router,
    @Inject(MAT_DIALOG_DATA) public data: any) {
    this.editorform = this.fb.group({
      subject: ["", Validators.required],
      editor: new FormControl(null),
    }, { updateOn: "change" });

  }

  ngOnInit(): void {
    //this.filters.UserID = this.data.UserID;
    this.currentUser = this.http.getUserDetails();
    this.deadlinesFinalResult = this.data;
    //this.title = "Email To TSPs";

  }

  getDeadlinesForTSP() {
    this.http.getJSON('api/KAMDashboard/GetDeadlinesForKAM' + this.currentUser.UserID).subscribe(
      (d: any) => {
        this.deadlines = d;
      }, error => this.error = error
    );
  }
 
  triggerChangeDetection() {
    this.cdr.detectChanges()
  }

}


export interface IQueryFilters {
  SchemeID: number;
  TSPID: number;
  ClassID: number;
  TraineeID: number;
  UserID: number;
  OID?: number;
}
