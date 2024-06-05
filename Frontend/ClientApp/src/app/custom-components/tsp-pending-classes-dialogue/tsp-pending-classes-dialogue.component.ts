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
import { RTPDialogComponent } from '../../inception-report/rtp/rtp.component';
import { EnumClassStatus, EnumTSPColorType } from '../../shared/Enumerations';



@Component({
  selector: 'app-tsp-pending-classes-dialogue',
  templateUrl: './tsp-pending-classes-dialogue.component.html',
  styleUrls: ['./tsp-pending-classes-dialogue.component.scss']
})
export class TSPPendingClassesDialogueComponent implements OnInit {
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

  query = {
    order: 'ClassID',
    limit: 5,
    page: 1
  };

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  displayedColumnsClass = [
    'ClassCode',
    'RTP',
    'Action'
    //'Duration', 'ClassStatusName', 'StartDate', 'EndDate', 'TrainingAddressLocation', 'TradeName', 'GenderName', 'TraineesPerClass', 'TehsilName', 'CertAuthName', 'InceptionReportDueOn', 'TraineeRegistrationDueOn', 'NTPStatus'
  ];


  constructor(private http: CommonSrvService, public dialog: MatDialog, public dialogRef: MatDialogRef<TSPPendingClassesDialogueComponent>, private router: Router,
    @Inject(MAT_DIALOG_DATA) public data: any) {
    dialogRef.disableClose = true;
}

  ngOnInit(): void {
    this.filters.UserID = this.data.UserID;
    this.currentUser = this.http.getUserDetails();
    this.title = "Pending Classes";
    this.getPendingClasses();

  }
  getPendingClasses() {
    if (this.data.TileName == "PendingInceptionReports") {
      this.title = "Pending Classes for Inception Report";

      this.getPendingInceptionReportClassData();
      this.displayedColumnsClass = [
        'SchemeName',
        'ClassCode',
        'Duration',
        'StartDate', 'EndDate',
        'GenderName',
        'ClassStatusName',
        //'RTP',
        'Action'
        //'Duration', 'ClassStatusName', 'StartDate', 'EndDate', 'TrainingAddressLocation', 'TradeName', 'GenderName', 'TraineesPerClass', 'TehsilName', 'CertAuthName', 'InceptionReportDueOn', 'TraineeRegistrationDueOn', 'NTPStatus'
      ];

    }
    if (this.data.TileName == "PendingRegisterations") {
      this.title = "Pending Classes for Trainee Registeration";
      this.getPendingRegisterationsClassData();
      this.displayedColumnsClass = [
        'SchemeName',
        'ClassCode',
        'Duration',
        'StartDate', 'EndDate',
        'GenderName',
        'ClassStatusName',
        //'RTP',
        'ActionRegisteration'
        //'Duration', 'ClassStatusName', 'StartDate', 'EndDate', 'TrainingAddressLocation', 'TradeName', 'GenderName', 'TraineesPerClass', 'TehsilName', 'CertAuthName', 'InceptionReportDueOn', 'TraineeRegistrationDueOn', 'NTPStatus'
      ];
    }
    if (this.data.TileName == "PendingRTPs") {
      this.title = "Pending Classes for RTP";
      this.getPendingRTPsClassData();
      this.displayedColumnsClass = [
        'SchemeName',
        'ClassCode',
        'Duration',
        'StartDate', 'EndDate',
        'GenderName',
        'ClassStatusName',
        'RTP',
        //'Action'
        //'Duration', 'ClassStatusName', 'StartDate', 'EndDate', 'TrainingAddressLocation', 'TradeName', 'GenderName', 'TraineesPerClass', 'TehsilName', 'CertAuthName', 'InceptionReportDueOn', 'TraineeRegistrationDueOn', 'NTPStatus'
      ];
    }
    if (this.data.TileName == "Deadlines") {
      this.title = "Deadlines";
      this.getDeadlinesData();
      this.displayedColumnsClass = [
        'SchemeName',
        'ClassCode',
        'Duration',
        'StartDate', 'EndDate',
        'ClassStatusName',
        //'Action'
        //'Duration', 'ClassStatusName', 'StartDate', 'EndDate', 'TrainingAddressLocation', 'TradeName', 'GenderName', 'TraineesPerClass', 'TehsilName', 'CertAuthName', 'InceptionReportDueOn', 'TraineeRegistrationDueOn', 'NTPStatus'
      ];
    }
  }

  getDeadlinesData() {
    this.http.postJSON(`api/Class/FetchClassesByUser`, this.filters)
      .subscribe(
        (d: any) => {
          this.classesArray = new MatTableDataSource(d);
          this.classesArray.paginator = this.paginator;
          this.classesArray.sort = this.sort;

        }

      );
  }
  getPendingRTPsClassData() {
    this.http.postJSON(`api/Class/FetchPendingRTPClassesByUser`, this.filters)
      .subscribe(
        (d: any) => {
          //  this.Class = new MatTableDataSource(d);
          this.classesArray = new MatTableDataSource(d);
          this.classesArray.paginator = this.paginator;
          this.classesArray.sort = this.sort;
          //this.buildDataSource();

        }

      );

  }
  getPendingRegisterationsClassData() {
    this.http.postJSON(`api/Class/FetchPendingRegistertionClassesByUser`, this.filters)
      .subscribe(
        (d: any) => {
          //  this.Class = new MatTableDataSource(d);
          this.classesArray = new MatTableDataSource(d);
          this.classesArray.paginator = this.paginator;
          this.classesArray.sort = this.sort;
          //this.buildDataSource();

        }

      );

  }
  getPendingInceptionReportClassData() {
    this.http.postJSON(`api/Class/FetchPendingInceptionReportClassesByUser`, this.filters)
      .subscribe(
        (d: any) => {
          //  this.Class = new MatTableDataSource(d);
          this.classesArray = new MatTableDataSource(d);
          this.classesArray.paginator = this.paginator;
          this.classesArray.sort = this.sort;
          //this.buildDataSource();

        }

      );

  }

  routeToRegistration(row: any) {
    this.http.getJSON(`api/InceptionReport/GetInceptionReportByClass?classID=${row.ClassID}`)
      .subscribe(data => {
        if (data[0].length > 0) {
          this.onNoClick();
          this.router.navigateByUrl(`/registration/trainee/${row.ClassID}`);
        } else {
          this.http.ShowError("Please submit inception report first, before registration.");
        }
      }, error => {
        this.error = error;
      });

  }


  checkTSPColor(classStatusid: number) {
    this.http.postJSON('api/TSPColor/RD_TSPMasterColorByID', { 'CurUserID': this.currentUser.UserID }).subscribe((response: any) => {
      console.log(response);
      if (response[0].TSPColorID == EnumTSPColorType.Red && classStatusid != EnumClassStatus.Active) {
        this.http.ShowError("TSPs with color red can only handle active classes");
        this.blacklistedTSPwithRed = true;
        return;
      }
      if (response[0].TSPColorID == EnumTSPColorType.Black) {
        this.http.ShowError("BlackListed TSPs are not allowed to perform any action");
        this.blacklistedTSPwithBlack = true;
        return;
      }
      //if (response[0].TSPColorID == EnumTSPColorType.Yellow) {
      //  this.ComSRV.ShowError("TSPs with color yellow can only perform day to day operations");
      //  this.blacklistedTSPwithRed = true;
      //  return;
      //}
      else {
        this.blacklistedTSPwithRed = false;
        this.blacklistedTSPwithBlack = false;
      }
    }
    );
  }

  routeToInceptionReport(row: any) {
    if (this.currentUser.UserLevel == EnumUserLevel.TSP) {
      this.checkTSPColor(row.ClassStatusID);

    }

    this.http.postJSON('api/InceptionReport/RD_InceptionReportBy', { "ClassID": row.ClassID }).subscribe((response: any) => {
      let reportSubmissionBracket = <any[]>response[0];
      let InceptionReportData = <any[]>response[1];
      this.classObj = response[2];
      this.orgConfig = response[3];
      let orgConfigList = response[3];

      var data = this.classesArray.filteredData.filter(a => a.ClassID === row.ClassID);
      console.log(data[0].NTP);

      if (this.blacklistedTSPwithRed || this.blacklistedTSPwithBlack) {
        return;
      }

      if (orgConfigList.length <= 0) {
        this.error = "Please set 'Rules' of this class before creating report.";
        //this.inceptionreportform.disable({ onlySelf: true });
        this.http.ShowError(this.error.toString(), "Error");
        return;
      }

      if (data[0].ClassStatusID != EnumClassStatus.Ready && InceptionReportData.length == 0) {
        this.error = "Class status must be 'Ready' to submit inception report.";
        //this.inceptionreportform.disable({ onlySelf: true });
        this.http.ShowError(this.error.toString(), null, 6000);
        return false;
      }

      if (data[0].NTP == false && (data[0].PTypeName == 'Community' && data[0].TrainingAddressLocation == '')) {
        this.http.ShowError("NTP must be issued");
      } else {
        this.onNoClick();
        this.router.navigateByUrl(`/inception-report/inception/${row.ClassID}`);
      }
    },
    );

  }


  openDialog(row): void {

    this.http.postJSON('api/RTP/RD_RTPClassDataBy', { "ClassID": row.ClassID }).subscribe((response: any) => {
      //response = response.reduce((accumulator, value) => accumulator.concat(value), []);

      this.classObj = response[0];
      this.orgConfig = response[1];
      let orgConfigList = response[2];


      //BR Business Rule
      let classStartDate = this.classObj[0].StartDate;
      this.classStartDate = this.classObj[0].StartDate;
      let classEndDate = this.classObj.EndDate;
      this.classEndDate = this.classObj.EndDate;
      let classStart = typeof (classStartDate) == "string" ? new Date(classStartDate) : classStartDate;
      let today = new Date();
      let openeing = new Date(classStart);
      let closing = new Date(classStart);
      openeing.setDate((classStart.getDate() - this.orgConfig[0].RTPFrom));
      closing.setDate((classStart.getDate() + this.orgConfig[0].RTPTo));
      if (today < openeing || today > closing) {
        this.error = "RTP creation date has been passed";
        this.http.ShowError(this.error.toString(), null, 6000);
        row.RTP = !row.RTP;
        return;
      }

      const dialogRef = this.dialog.open(RTPDialogComponent, {
        minWidth: '700px',
        minHeight: '350px',
        //data: JSON.parse(JSON.stringify(row))
        data: {
          "ClassID": row.ClassID, "ClassCode": row.ClassCode, 'RTPValue': row.RTP,
          'DistrictID': row.DistrictID, 'DistrictName': row.DistrictName
          , 'TehsilID': row.TehsilID, 'TehsilName': row.TehsilName
          , 'SchemeName': this.classObj[0].SchemeName, 'TSPName': this.classObj[0].TSPName
          , 'TradeName': this.classObj[0].TradeName, 'TraineesPerClass': this.classObj[0].TraineesPerClass
          , 'Duration': this.classObj[0].Duration, 'Name': this.classObj[0].Name, 'CPName': this.classObj[0].CPName,
          'CPLandline': this.classObj[0].CPLandline, 'StartDate': this.classObj[0].StartDate

        }
      });
      dialogRef.afterClosed().subscribe(result => {

        if (result == undefined || result == false) {
          row.RTP = !row.RTP;
        }
        if (result == true) {
          row.RTP = row.RTP;
        }
      });

    });



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
