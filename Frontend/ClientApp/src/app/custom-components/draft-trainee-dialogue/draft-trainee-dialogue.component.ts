import { Component, OnInit, Inject, ViewChild } from '@angular/core';
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
import { TsuDialogueComponent } from 'src/app/traineereports/trainee-status-update/tsu-dialogue/tsu-dialogue.component';
// import { DatePipe } from '@angular/common';
@Component({
  selector: 'draft-trainee-dialogue',
  templateUrl: './draft-trainee-dialogue.component.html',
  styleUrls: ['./draft-trainee-dialogue.component.scss']
})

export class DraftTraineeDialogueComponent implements OnInit {
  classesArray: MatTableDataSource<any>;
  filters: IQueryFilters = { SchemeID: 0, TSPID: 0, ClassID: 0, TraineeID: 0, UserID: 0, OID: 0 };
  title: string; error: string;
  orgConfig: IOrgConfig;
  classObj: any;
  classStartDate: Date;
  classEndDate: Date;
  StartDate: Date;
  EndDate: Date;
  currentUser: UsersModel;
  enumUserLevel = EnumUserLevel;
  enumBusinessRuleTypes = EnumBusinessRuleTypes;
  blacklistedTSPwithRed: boolean = false;
  blacklistedTSPwithBlack: boolean = false;
  Role:string=""
  query = {
    order: 'ClassID',
    limit: 5,
    page: 1
  };
  CurrentDate: Date;
  EnrollmentDateC:Date;
  EnrollmentDate:string;

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  displayedColumnsClass = [];

  constructor(private http: CommonSrvService, public dialog: MatDialog, public dialogRef: MatDialogRef<DraftTraineeDialogueComponent>, private router: Router,
    @Inject(MAT_DIALOG_DATA) public data: any) {
    dialogRef.disableClose = true;
  }
 
  ngOnInit(): void {
    this.CurrentDate = new Date()
    
    this.filters.UserID = this.data.UserID;
    this.currentUser = this.http.getUserDetails();
    this.getPendingClasses();
    
  }
  getPendingClasses() {
    if (this.data.TileName == "Draft Trainee Report") {
      this.title = "Draft Trainee Report";
      this.GetTraineeDraftDataByUser();
      this.displayedColumnsClass = [
        'Status',
        'TSP Name',
        'Class Code',
        'Trainee Name',
        'Trainee CNIC',
        'Trainee Code',
      ];
    }
  }


  routeToRegistration(row: any) {
    this.onNoClick()
    this.router.navigateByUrl(`/registration/trainee/${row.ClassID}`);

  }


  GetTraineeDraftDataByUser() {
    this.Role=this.currentUser.RoleTitle
    let Url;
    if(this.Role=='TSP'){
       Url = `GetTraineeDraftDataByTsp?TspID=${this.currentUser.UserID}`
    }else{
      Url = `GetTraineeDraftDataByKam?KamID=${this.currentUser.UserID}`
    }
    this.http.getJSON(`api/TraineeProfile/${Url}`)
      .subscribe(
        (response: any) => {
          let traineeProfileList = response.ListTraineeProfile;
          this.classesArray = new MatTableDataSource(traineeProfileList);
          this.classesArray.paginator = this.paginator;
          this.classesArray.sort = this.sort;
        }
      );
  }


  openDialog(row: any): void {
    const dialogRef = this.dialog.open(TsuDialogueComponent, {
      width: '600px',
      minHeight: '400px',
      data: { ...row,TileName:'Draft Trainee' }
    })
    dialogRef.afterClosed().subscribe(result => {
      this.updateTraineeStatus(result);
      this.GetTraineeDraftDataByUser()

    })
  }



  updateTraineeStatus(traineeData: any): void {
    if (traineeData) {
      this.http.getJSON(`api/TraineeProfile/UpdateTraineeStatus?TraineeID=${traineeData.TraineeID}&TraineeStatusTypeID=${traineeData.TraineeStatusTypeID}&TraineeStatusReason=${traineeData.TraineeReason}`).subscribe(
        data => {
          if (data === true) {
            this.http.openSnackBar('Saved Successfully');
          }
        }
        , error => {
          this.http.ShowError(error.error + '\n' + error.message);
        });
    }
  }

  onNoClick(): void {
    this.dialogRef.close(false);
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
