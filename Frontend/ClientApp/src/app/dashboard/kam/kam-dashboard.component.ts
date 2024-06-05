import { Component, OnInit, ViewChild, ViewChildren, ChangeDetectorRef } from "@angular/core";
import { FormGroup, FormBuilder, FormControl, Validators, FormGroupDirective } from '@angular/forms';
import { CommonSrvService } from "../../common-srv.service";
import { MatDatepicker } from "@angular/material/datepicker";
import * as _moment from 'moment';
import * as Highcharts from 'highcharts';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { UsersModel } from '../../master-data/users/users.component';
import { SelectionModel } from '@angular/cdk/collections';
import { ModelBase } from '../../shared/ModelBase';
import { KAMPendingClassesDialogueComponent } from '../../custom-components/kam-pending-classes-dialogue/kam-pending-classes-dialogue.component';
import { KAMDeadlinesDialogComponent } from '../../custom-components/kam-deadlines-dialog/kam-deadlines-dialog.component';
import { MatIconModule } from '@angular/material/icon';
import { GroupByPipe } from 'angular-pipes';

const moment = _moment
import { Moment } from 'moment';
import { DraftTraineeDialogueComponent } from "src/app/custom-components/draft-trainee-dialogue/draft-trainee-dialogue.component";
// import { IQueryFilters } from "src/app/rosi/rosi/rosi.component";
export const MY_FORMATS = {
  parse: {
    dateInput: 'MM/YYYY',
  },
  display: {
    dateInput: 'MM/YYYY',
    monthYearLabel: 'MMM YYYY',
    dateA11yLabel: 'LL',
    monthYearA11yLabel: 'MMMM YYYY',
  },
};

declare var require: any;


let Boost = require('highcharts/modules/boost');
let noData = require('highcharts/modules/no-data-to-display');
let More = require('highcharts/highcharts-more');
let SolidGuage = require('highcharts/modules/solid-gauge');
let Funnel = require('highcharts/modules/funnel');

Boost(Highcharts);
noData(Highcharts);
More(Highcharts);
SolidGuage(Highcharts);
Funnel(Highcharts);
noData(Highcharts);

@Component({
  selector: 'kam-dashboard',
  templateUrl: './kam-dashboard.component.html',
  styleUrls: ['./kam-dashboard.component.scss'],
  providers: [GroupByPipe]

})


export class KAMDashboardComponent implements OnInit {

  
  editorform: FormGroup;
  month = new FormControl(moment());
  modules = {}
  constructor(private commonService: CommonSrvService, public dialog: MatDialog, private fb: FormBuilder, private cdr: ChangeDetectorRef, private groupByPipe: GroupByPipe) {
    this.editorform = this.fb.group({
      subject: ["", Validators.required],
      EmailAttachmentFile:[""],
      editor: new FormControl(null),
    }, { updateOn: "change" });

    this.modules = {
      'toolbar': [
        ['bold', 'italic', 'underline', 'strike'],        // toggled buttons
        ['blockquote', 'code-block'],

        [{ 'header': 1 }, { 'header': 2 }],               // custom button values
        [{ 'list': 'ordered' }, { 'list': 'bullet' }],
        [{ 'script': 'sub' }, { 'script': 'super' }],      // superscript/subscript
        [{ 'indent': '-1' }, { 'indent': '+1' }],          // outdent/indent
        [{ 'direction': 'rtl' }],                         // text direction

        [{ 'size': ['small', false, 'large', 'huge'] }],  // custom dropdown
        [{ 'header': [1, 2, 3, 4, 5, 6, false] }],

        [{ 'color': [] }, { 'background': [] }],          // dropdown with defaults from theme
        [{ 'font': [] }],
        [{ 'align': [] }],

        ['clean'],                                         // remove formatting button
      ]
    }

  }

  triggerChangeDetection() {
    this.cdr.detectChanges()
  }

  displayedColumnsClass = ['sn', 'ClassCode', 'Duration', 'ClassStatusName', 'StartDate', 'EndDate', 'TrainingAddressLocation', 'TradeName', 'GenderName', 'TraineesPerClass', 'TehsilName', 'CertAuthName', 'InceptionReportDueOn', 'TraineeRegistrationDueOn'];
  displayedColumnsTSP = ['select','TSPName', 'TSPCode', 'TSPColorName'];

  Class: MatTableDataSource<any>;
  TSPs: MatTableDataSource<any>;
  TSPsArray: any[];
  tspUserIDsArray: any[];

  selection = new SelectionModel<any>(true, []);

  SearchTSP = new FormControl("");

  user: UsersModel; 
  selectedValueTSP: any;
  selectedValueStart: any = undefined;
  selectedValueEnd: any = undefined;

  tspUserIDs: string;
  Start: string;
  End: string;
  error: String;


  DashboardStats: any[];
  deadlines: any[];
  deadlinesGrouped: any[];
  deadlinesLoopWiseGroup: any[];
  deadlinesLoopWise: any[];
  deadlinesFinalResult: any[];

  ContractedTrainees: number;
  EnrolledTrainees: number;
  UnverifiedTrainees: number;
  TrainingProviders: number;
  ContractedClasses: number;
  Role: String;
  UnverifiedTraineeEmail: any;
  
  ContractualToEnrolled: string;
  PendingClassesForEmployment: string;

  Resolved: number;
  Pending: number;
  InProcess: number;
  Unresolved: number;
  TotalComplaints: number;
  TotalDeadlines: number;

  Planned: number;
  Active: number;
  Completed: number;
  Abandoned: number;
  Cancelled: number;
  Ready: number;
  Suspended: number;
  currentUser: UsersModel;

  LinkForCRM: string;

  @ViewChild('tabGroup') tabGroup;
  @ViewChild('SortTSP') SortTSP: MatSort;
  @ViewChild('PageTSP') PageTSP: MatPaginator;
  DraftTrainee:any;
  filters: IQueryFilters = { SchemeID: 0, TSPID: 0, ClassID: 0, TraineeID: 0, UserID: 0, OID: 0 };

  ngOnInit(): void {
    this.commonService.OID.subscribe(OID => {
      this.filters.SchemeID = 0;
      this.filters.TSPID = 0;
      this.filters.ClassID = 0;
      this.filters.OID = OID;
      this.user = this.commonService.getUserDetails();
      this.currentUser = this.commonService.getUserDetails();
      this.filters.UserID = this.user.UserID;
    });
    
    this.GetTraineeDraftData()
    this.commonService.setTitle("KAM Dashboard");
    this.getKAMDashboardData();
    this.getDeadlinesForKAM();
    this.getUnverifiedTraineeEmail(this.user.UserID)
  }



  
  getUnverifiedTraineeEmail(UserId: number) {
    // this.Role=this.currentUser.RoleTitle
    let Url;
    // if(this.Role=='TSP'){
    //    Url='GetUnverifiedTraineeEmail'
    // }else{
      Url='GetUnverifiedTraineeEmailByKam'

    // }
    
    this.commonService.getJSON(`api/Scheme/${Url}/` + UserId)
      .subscribe(data => {
       console.log(data)
       this.UnverifiedTraineeEmail=data
      }, error => {
        this.error = error;
      });
  }

  async UnverifiedTraineeEmailAddress(){
    const Param = this.GetParamString('RD_UnverifiedTraineeEmail',  {tspId:0,KamId:this.user.UserID});

    const data: any =await  this.commonService.getJSON(`api/BSSReports/FetchReportData?Param=${Param}`).toPromise();
    if (data.length > 0) {
      this.commonService.ExportToExcel(data,'Unverifed_Trainee_Email');
    } else {
      this.commonService.ShowWarning(' No Record Found', 'Close');
    }
  }

  
  GetParamString(SPName: string, paramObject: any) {

    let ParamString = SPName;
    for (const key in paramObject) {
      if (Object.hasOwnProperty.call(paramObject, key)) {
        ParamString += `/${key}=${paramObject[key]}`;
      }
    }

    return ParamString;

  }
  
 


  GetTraineeDraftData() {
    // this.Role=this.user.RoleTitle
    let Url;
   
      Url = `GetTraineeDraftDataByKam?KamId=${this.currentUser.UserID}`

    this.commonService.getJSON(`api/TraineeProfile/${Url}`)
      .subscribe(
        (response: any) => {
          let traineeProfileList = response.ListTraineeProfile;
        this.DraftTrainee=traineeProfileList.length
        }
      );
  }

  openDraftTraineeDialog(tile: string): void {
    console.log(tile)
    const dialogRef = this.dialog.open(DraftTraineeDialogueComponent, {
      height: '55%',
      width: '75%',
      data: { "TileName": tile, "UserID": this.currentUser.UserID }
    });
    dialogRef.afterClosed().subscribe(result => {
        this.GetTraineeDraftData()
    });
  }

  isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.TSPs.data.length;
    return numSelected === numRows;
  }

  masterToggle() {
    this.isAllSelected() ?
      this.selection.clear() :
      this.TSPs.data.forEach(row => this.selection.select(row));
  }

  sendEmailToSelectedUsers() {
    debugger;
    const numSelected = this.selection.selected;

    this.tspUserIDsArray = numSelected.map(x => x.UserID);
    //this.tspUserIDs = this.tspUserIDsArray.join(',');

    var subject = this.subject.value;

    var email = this.editor.value;
    
    var EmailAttachmentFile=this.EmailAttachmentFile.value;

    if (!this.editorform.valid)
      return;

    this.commonService.postJSON('api/KAMDashboard/SendEmailToTSPs', { 'Subject': subject,'EmailAttachmentFile': EmailAttachmentFile,'EmailToSent': email, 'UserIDs': this.tspUserIDsArray })
      .subscribe((d: any) => {
        if (d) {
          this.reset();
          var success = "Email sent successfully";
          this.commonService.openSnackBar(success.toString(), "Updated"); 

        }
      },
        (error) => {
          this.error = error.error;
          this.commonService.ShowError(error.error);
        });
  }
  reset(){
    this.editorform.reset();
  }

  getDeadlinesForKAM() {
    this.commonService.postJSON('api/KAMDashboard/GetDeadlinesForKAM/', { "UserID": this.user.UserID, 'TSPID': this.selectedValueTSP }).subscribe(
      (d: any) => {
        this.deadlines = d;
        this.TotalDeadlines = this.deadlines[0].length;
        //this.deadlinesFinalResult = [];
        //this.deadlinesGrouped = this.groupByPipe.transform(this.deadlines[0], "DeadlineType")

        //let indexSRN = 0;
        //this.deadlinesGrouped.forEach(key => {
        //  var number = indexSRN; //Number(key.key) - 1;
        //  this.deadlinesLoopWise = this.deadlinesGrouped[number].value;
        //  this.deadlinesLoopWiseGroup = this.groupByPipe.transform(this.deadlinesLoopWise, "DeadlineDate")

        //  var newarr = [];
        //  newarr = this.deadlinesLoopWiseGroup.map(x => [{ DeadlineDate: x.key, DeadlineType: x.value[0].DeadlineType }])
        //  newarr = newarr.reduce((accumulator, value) => accumulator.concat(value), []);
        //  this.deadlinesFinalResult.push(newarr);
        //  indexSRN++;
        //}
        //);
        //this.deadlinesFinalResult = this.deadlinesFinalResult.reduce((accumulator, value) => accumulator.concat(value), []);


      }, error => this.error = error
    );
  }

  clearMonth() {
    this.month = new FormControl(moment(null));
    //  this.month.setValue(null);
    this.getKAMDashboardData();
  }

  chosenYearHandler(normalizedYear: Moment) {
    this.month = new FormControl(moment());
    const ctrlValue = this.month.value;
    ctrlValue.year(normalizedYear.year());
    this.month.setValue(ctrlValue);
  }

  chosenMonthHandler(normalizedMonth: Moment, datepicker: MatDatepicker<Moment>) {
    const ctrlValue = this.month.value;
    ctrlValue.month(normalizedMonth.month());
    this.month.setValue(ctrlValue);
    this.getKAMDashboardData();
    datepicker.close();
  }

  getRelevantTSPs() {
    if (this.selectedValueTSP == 0) {
      this.TSPs = new MatTableDataSource(this.TSPsArray);
    }
    else {
      this.TSPs = new MatTableDataSource(this.TSPs.filteredData.filter(x => x.TSPID == this.selectedValueTSP));
    }
  }

  getKAMDashboardData() {
    var userid = this.user.UserID;
    this.commonService.postJSON("api/KAMDashboard/GetKAMDashboardTSPs", { "UserID": userid, 'TSPID': this.selectedValueTSP }).subscribe((data: any[]) => {
      this.TSPs = new MatTableDataSource(data[0]);
      this.TSPsArray = data[0];
      //this.getRelevantTSPs();
      this.TSPs.paginator = this.PageTSP;
      this.TSPs.sort = this.SortTSP;

      this.DashboardStats = data[1];
      this.ContractualToEnrolled = this.DashboardStats[0].ContractualToEnrolled;
      this.PendingClassesForEmployment = this.DashboardStats[0].PendingClassesForEmployment;
      this.Resolved = this.DashboardStats[0].Resolved;
      this.Pending = this.DashboardStats[0].Pending;
      this.InProcess = this.DashboardStats[0].InProcess;
      this.Unresolved = this.DashboardStats[0].Unresolved;
      this.TotalComplaints = this.DashboardStats[0].TotalComplaints;
      //this.TotalDeadlines = this.DashboardStats[0].TotalDeadlines;
      this.Planned = this.DashboardStats[0].Planned;
      this.Active = this.DashboardStats[0].Active;
      this.Completed = this.DashboardStats[0].Completed;
      this.Abandoned = this.DashboardStats[0].Abandoned;
      this.Cancelled = this.DashboardStats[0].Cancelled;
      this.Ready = this.DashboardStats[0].Ready;
      this.Suspended = this.DashboardStats[0].Suspended;

      this.LinkForCRM = this.DashboardStats[0].LinkForCRM;
      this.optionsSDPie.series[0].data = this.DashboardStats[0].SDPie;

      //Highcharts.chart('containerSDPie', this.optionsSDPie);

      this.optionsClassStatus.series = [{
        name: 'Planned',
        data: [this.Planned]

      }, {
        name: 'Active',
        data: [this.Active]

      }, {
        name: 'Completed',
        data: [this.Completed]

      }, {
        name: 'Abandoned',
        data: [this.Abandoned]

      }, {
        name: 'Cancelled',
        data: [this.Cancelled]

      }, {
        name: 'Ready',
        data: [this.Ready]

      }, {
        name: 'Suspended',
        data: [this.Suspended]

      }];

      //Highcharts.chart('containerPassed', this.optionsComplaintStatus);
      Highcharts.chart('containerClasses', this.optionsClassStatus);

      //this.optionsComplaintStatus.series = [{
      //  name: 'Pending',
      //  data: [this.Pending]

      //}, {
      //  name: 'InProcess',
      //  data: [this.InProcess]

      //}, {
      //  name: 'Resolved',
      //  data: [this.Resolved]

      //}, {
      //  name: 'Unresolved',
      //  data: [this.Unresolved]

      //}];

      //Highcharts.chart('containerPassed', this.optionsComplaintStatus);

      this.getDeadlinesForKAM();
    });
  }

  EmptyCtrl(type: any) {
    if (type == 'Trade') {
     
      this.selectedValueTSP = undefined;
      this.selectedValueStart = undefined;
      this.selectedValueEnd = undefined;
    } else if (type == 'Cluster') {
     ;
      this.selectedValueTSP = undefined;
      this.selectedValueStart = undefined;
      this.selectedValueEnd = undefined;
    } else if (type == 'District') {
     
      this.selectedValueStart = undefined;
      this.selectedValueEnd = undefined;
    } else if (type == 'Program') {
     
      this.selectedValueTSP = undefined;
      this.selectedValueStart = undefined;
      this.selectedValueEnd = undefined;
    } else if (type == 'Scheme') {

      this.selectedValueTSP = undefined;
      this.selectedValueStart = undefined;
      this.selectedValueEnd = undefined;
    } else if (type == 'TSP') {
      this.selectedValueStart = undefined;
      this.selectedValueEnd = undefined;
    }
  }
  private EmptyDopdowns() {
    
    this.selectedValueTSP = undefined;
  }



  getSelectedTabData() {
    switch (this.tabGroup?.selectedIndex ?? 0) {
      case 0:
        this.getTSPData();
        break;
      case 2:
        this.getClassData();
        break;
      default:
    }
  }

  public optionsSDPie: any = {
    chart: {
      plotBackgroundColor: null,
      plotBorderWidth: null,
      plotShadow: false,
      type: 'pie',
      height: '300px'
    },
    title: {
      text: ''
    },
    credits: {
      enabled: false
    },
    tooltip: {
      pointFormat: '<br>{point.name}: {point.y}<br>Total: {point.total}<br>{point.percentage:.1f} %'
    },
    accessibility: {
      point: {
        valueSuffix: '%'
      }
    },
    plotOptions: {
      pie: {
        allowPointSelect: true,
        cursor: 'pointer',
        dataLabels: {
          enabled: false
        },
        showInLegend: true
      }
    },
    series: [{
      name: 'Class Status',
      colorByPoint: true,
      data: []
    }]
  }

  public optionsClassStatus: any = {
    chart: {
      type: 'column',
      height: '300px'
    },
    title: {
      text: ''
    },
    credits: {
      enabled: false
    },
    legend: {
      enabled: true,
      labelFormatter: function () {
        return this.name + ' (' + this.yData[0] + ')';
      }
    },
    xAxis: {
      categories: [
        'Class Status'
      ],
      crosshair: true
    },
    yAxis: {
      min: 0,
      title: {
        text: 'No of Classes'
      }
    },
    tooltip: {
      headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
      pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
        '<td style="padding:0"><b>{point.y:.1f} </b></td></tr>',
      footerFormat: '</table>',
      shared: true,
      useHTML: true
    },
    plotOptions: {
      column: {
        pointPadding: 0.2,
        borderWidth: 0,
        showInLegend: true
      }
    },
    series: []
  }


  public optionsComplaintStatus: any = {
    chart: {
      type: 'column',
      height: '300px'
    },
    title: {
      text: ''
    },
    credits: {
      enabled: false
    },
    legend: {
      enabled: true,
      labelFormatter: function () {
        return this.name + ' (' + this.yData[0] + ')';
      }
    },
    xAxis: {
      categories: [
        'Complaints Status'
      ],
      crosshair: true
    },
    yAxis: {
      min: 0,
      title: {
        text: 'No of Complaints'
      }
    },
    tooltip: {
      headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
      pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
        '<td style="padding:0"><b>{point.y:.1f} </b></td></tr>',
      footerFormat: '</table>',
      shared: true,
      useHTML: true
    },
    plotOptions: {
      column: {
        pointPadding: 0.2,
        borderWidth: 0,
        showInLegend: true
      }
    },
    series: []
  }
  applyFilter(filterValue: string) {
    filterValue = filterValue.trim(); // Remove whitespace
    filterValue = filterValue.toLowerCase(); // Datasource defaults to lowercase matches
    this.TSPs.filter = filterValue;
}

  openPendingClassesDialog(tile: string): void {
    const dialogRef = this.dialog.open(KAMPendingClassesDialogueComponent, {
      //minWidth: '1000px',
      //minHeight: '600px',

      height: '65%',
      width: '60%',
      //data: JSON.parse(JSON.stringify(row))
      data: { "TileName": tile, "UserID": this.user.UserID, "TSPID": this.selectedValueTSP }
      //this.GetVisitPlanData(data)
    });
    dialogRef.afterClosed().subscribe(result => {


      //console.log(result);
      //this.visitPlan = result;
      //this.submitVisitPlan(result);
    });
  }

  openKamDeadlineDialog(): void {
    const dialogRef = this.dialog.open(KAMDeadlinesDialogComponent, {
      //minWidth: '1000px',
      //minHeight: '600px',

      height: '35%',
      width: '30%',
      //data: JSON.parse(JSON.stringify(row))
      data: this.deadlines[0]
      //this.GetVisitPlanData(data)
    });
    dialogRef.afterClosed().subscribe(result => {


      //console.log(result);
      //this.visitPlan = result;
      //this.submitVisitPlan(result);
    });
  }



  getTSPData() {

  }

  getClassData() {

  }


  get editor() { return this.editorform.get("editor"); }
  get subject() { return this.editorform.get("subject"); }
  get EmailAttachmentFile() { return this.editorform.get("EmailAttachmentFile"); }

}


export class editorModel extends ModelBase {
  
  editor: string;

}


export interface IQueryFilters {
  SchemeID: number;
  TSPID: number;
  ClassID: number;
  TraineeID: number;
  UserID: number;
  OID?: number;
}
