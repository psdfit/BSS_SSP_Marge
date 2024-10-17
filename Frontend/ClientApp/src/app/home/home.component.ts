import {
  AfterViewInit,
  Component,
  ElementRef,
  OnInit,
  QueryList,
  ViewChild,
  ViewChildren,
} from "@angular/core";
import { MatDialog } from "@angular/material/dialog";
import { MatPaginator } from "@angular/material/paginator";
import { MatSort } from "@angular/material/sort";
import { MatTableDataSource } from "@angular/material/table";
import { Router } from "@angular/router";
//import { Chart,Animation } from 'chart.js';
import { environment } from "../../environments/environment";
import { ChangeRequestDialogComponent } from "../change-request/change-request/change-request.component";
import { TSPPendingClassesDialogueComponent } from "../custom-components/tsp-pending-classes-dialogue/tsp-pending-classes-dialogue.component";
import { DraftTraineeDialogueComponent } from "../custom-components/draft-trainee-dialogue/draft-trainee-dialogue.component";
import { CommonSrvService } from "../common-srv.service";
import { RTPDialogComponent } from "../inception-report/rtp/rtp.component";
import { UsersModel } from "../master-data/users/users.component";
import { IOrgConfig } from "../registration/Interface/IOrgConfig";
import { EnumUserLevel, EnumBusinessRuleTypes } from "../shared/Enumerations";
import { FormControl } from "@angular/forms";
import { EnumClassStatus, EnumTSPColorType } from "../shared/Enumerations";
import { merge } from "rxjs";
import { startWith, switchMap } from "rxjs/operators";
import * as Highcharts from "highcharts";
import { DialogueService } from "../shared/dialogue.service";
// GIS Component
import * as L from "leaflet";
import { BehaviorSubject } from "rxjs";
import { geoJSON } from "leaflet";
import { FeatureCollection } from "geojson";
import { ChangePasswordComponent } from "../master-data/change-password/change-password.component";
import { SelectionModel } from "@angular/cdk/collections";
var map, greenIcon, markers, districtLayer, provinceLayer, tehsilLayer;
declare const provinceGeojson: FeatureCollection;
declare const districtGeojson: FeatureCollection;
declare const tehsilsGeojson: FeatureCollection;
@Component({
  selector: "hrapp-home",
  templateUrl: "./home.component.html",
  styleUrls: ["./home.component.scss"],
})
export class HomeComponent implements OnInit, AfterViewInit {
  selection = new SelectionModel<any>(true, []);
  getTraineeProfileRegistration_Report() {
    
    debugger;
    let ids = this.selection.selected.map((x) => x.TraineeID).join(",");
    this.ComSRV.getReportJSON(
      "TraineeProfile/GetRegistrationReport/" + ids
    ).subscribe(
      (data: any) => {
        let file = this.ComSRV.base64ToFile(data.Response);
        const fileURL = window.URL.createObjectURL(file);
        this.selection.clear()
        // if (window.navigator.msSaveOrOpenBlob) {
        //   window.navigator.msSaveOrOpenBlob(file, fileURL.split(':')[1] + '.pdf');
        // } else {
        //   window.open(fileURL);
        // }
        window.open(fileURL);
      },
      (error) => {
        this.error = error; // error path
        this.ComSRV.ShowError(error.error + "\n" + error.message, "", 5000);
      }
    );
  }
  error: String;
  Role: String;
  UnverifiedTraineeEmail: any;
  Dashboard: any;
  orgConfig: IOrgConfig;
  classObj: any;
  classStartDate: Date;
  classEndDate: Date;
  StartDate: Date;
  EndDate: Date;
  Invoices: [];
  Inprocess: [];
  EnText: string = "RTP";
  Classes: [];
  Class: MatTableDataSource<any>;
  Scheme: any[];
  Schemes: any[];
  TSPs: MatTableDataSource<any>;
  Trainees: MatTableDataSource<any>;
  Stats: MatTableDataSource<any>;
  resultsSchemeLength: number;
  resultsTspLength: number;
  resultsClassLength: number;
  resultsTraineeLength: number;
  blacklistedTSPwithRed: boolean = false;
  blacklistedTSPwithBlack: boolean = false;
  hideTilesForCommmunityTSP: boolean = false;
  base64 = [];
  Planned: number;
  Active: number;
  Completed: number;
  Cancelled: number;
  Abandoned: number;
  Ready: number;
  Suspended: number;
  classesArray: any[];
  TSPDetail = [];
  //UserLevel;
  homeStates: any[];
  homePendingStates: any[];
  filters: IQueryFilters = {
    SchemeID: 0,
    TSPID: 0,
    ClassID: 0,
    TraineeID: 0,
    UserID: 0,
    OID: 0,
  };
  displayedColumnsClass = [
    "sn",
    "InternalUserAction",
    "ClassCode",
    "Duration",
    "ClassStatusName",
    "StartDate",
    "EndDate",
    "TrainingAddressLocation",
    "TradeName",
    "GenderName",
    "TraineesPerClass",
    "TehsilName",
    "CertAuthName",
    "InceptionReportDueOn",
    "TraineeRegistrationDueOn",
  ];
  displayedColumnsScheme = [
    "SchemeName",
    "SchemeCode",
    "PTypeName",
    "PCategoryName",
    "FundingSourceName",
    "GenderName",
    "MinAge",
    "MaxAge",
    "StipendMode",
    "BusinessRuleType",
  ];
  displayedColumnsTSP = [
    "TSPName",
    "Address",
    "TSPCode",
    "SchemeName",
    "NTN",
    "PNTN",
    "GST",
    "FTN",
    "HeadName",
    "CPName",
    "CPLandline",
  ];
  displayedColumnsTrainee = [
    "sn",
    "ClassCode",
    "TraineeName",
    "TraineeCode",
    "ClassBatch",
    "StatusName",
    "TraineeCNIC",
    "FatherName",
    "GuardianName",
    "GenderName",
    "TraineeAge",
    "ContactNumber1",
    "TraineeEmail",
    "PermanentResidence",
    "PermanentDistrictName",
    "PermanentTehsilName",
    "TraineeDoc",
  ];
  displayedColumnsStats = ["Schemes", "TSPs", "Classes", "Trainees"];
  Summary: any = {};
  @ViewChild("dailySalesChart") chartElementRef: ElementRef;
  @ViewChild("TopCust") TopCust: ElementRef;
  @ViewChild(MatPaginator, { static: false })
  paginator: QueryList<MatPaginator>;
  @ViewChild(MatSort, { static: false }) sort: QueryList<MatSort>;
  @ViewChild("SortScheme") SortScheme: MatSort;
  @ViewChild("PageScheme") PageScheme: MatPaginator;
  @ViewChild("SortTSP") SortTSP: MatSort;
  @ViewChild("PageTSP") PageTSP: MatPaginator;
  @ViewChild("SortClass") SortClass: MatSort;
  @ViewChild("PageClass") PageClass: MatPaginator;
  @ViewChild("SortTrainee") SortTrainee: MatSort;
  @ViewChild("PageTrainee") PageTrainee: MatPaginator;
  @ViewChild("tabGroup") tabGroup;
  SaleData: {};
  currentUser: UsersModel;
  enumUserLevel = EnumUserLevel;
  enumBusinessRuleTypes = EnumBusinessRuleTypes;
  reducedGroups = [];
  District: any;
  MainDistrict: any;
  Tehsil: any;
  MainTehsil: any;
  Sector: any;
  Clustor: any;
  selectedDistrict: any;
  markerObject: Array<any>;
  makeMarker = new BehaviorSubject([]);
  constructor(
    private ComSRV: CommonSrvService,
    public dialog: MatDialog,
    private router: Router,
    public dialogueService: DialogueService
  ) {
    ComSRV.setTitle("Dashboard");
    this.Class = new MatTableDataSource([]);
    //console.log(ComSRV.appSettings);
  }
  ngOnInit() {
    this.ComSRV.OID.subscribe((OID) => {
      this.filters.SchemeID = 0;
      this.filters.TSPID = 0;
      this.filters.ClassID = 0;
      this.filters.OID = OID;
      //console.log(OID);
      this.currentUser = this.ComSRV.getUserDetails();
      // console.log(this.currentUser);
      if (this.currentUser.UserLevel == this.enumUserLevel.TSP) {
        this.filters.UserID = this.currentUser.UserID;
        this.displayedColumnsClass = [
          "sn",
          "Action",
          "RTP",
          "ClassCode",
          "Duration",
          "ClassStatusName",
          "StartDate",
          "EndDate",
          "TrainingAddressLocation",
          "TradeName",
          "GenderName",
          "TraineesPerClass",
          "TehsilName",
          "CertAuthName",
          "Batch",
          "DistrictName",
          "InceptionReportDueOn",
          "TraineeRegistrationDueOn",
          "NTPStatus",
        ];
        //this.getFilteredStatsDataPendingClasses();
        //this.displayedColumnsClass.push("NTPStatus");
        //this.displayedColumnsClass.push("RTP");
        //this.displayedColumnsClass.push("Action");
      }
      this.GetTraineeDraftData();
      this.getSchemes();
      this.getDataByFilters();
      this.getUnverifiedTraineeEmail(this.currentUser.UserID);
    });
  }
  GetCurrentComplaintAttachements(r) {
    //this.base64 = (r.TraineeDoc)
    this.base64ToBlob(r.TraineeDoc, r.TraineeCode);
  }
  public base64ToBlob(b64Data, ComplaintNo) {
    var elem = window.document.createElement("a");
    elem.href = b64Data;
    elem.download = ComplaintNo;
    document.body.appendChild(elem);
    elem.click();
    document.body.removeChild(elem);
  }
  ngAfterViewInit() {
    this.ComSRV.OID.subscribe((OID) => {
      this.filters.SchemeID = 0;
      this.filters.TSPID = 0;
      this.filters.ClassID = 0;
      this.schemeFilter.setValue(0);
      this.tspFilter.setValue(0);
      this.classFilter.setValue(0);
      this.filters.OID = OID;
      this.initSchemePagedData();
      // if (this.currentUser.UserLevel == this.enumUserLevel.TSP) {
      this.CheckPasswordAge();
      // }
    });
    //this.getDataByFilters();
    //console.log('afterViewInit => ', this.tabGroup.selectedIndex);
    this.initializeMap();
  }
  user: any = {};
  working: any = true;
  CheckPasswordAge() {
    this.user = this.ComSRV.getUserDetails();
    this.ComSRV.postJSON("api/Users/CheckPasswordAge", this.user).subscribe(
      (response: any) => {
        if (response == "1") {
          this.ComSRV.ShowError(
            "Your session will expire in 180 Seconds.Please change your password",
            "Close",
            15000
          );
          this.openDialogChangePass();
          setTimeout(() => {
            this.logout();
            this.router.navigate(["/login"]);
          }, 180000);
        }
      },
      (error) => {
        this.ComSRV.ShowError(error.error, "Close", 30000);
      }
    );
  }
  logout() {
    this.ComSRV.getJSON(
      "api/Users/Logout?SessionID=" +
        sessionStorage.getItem("SessionID") +
        "&UserID=" +
        sessionStorage.getItem("UserID")
    ).subscribe(
      (d: any) => {},
      (error) => (this.error = error) // error path
    );
    sessionStorage.removeItem(environment.AuthToken);
    sessionStorage.removeItem(environment.RightsToken);
    sessionStorage.removeItem("UserImage");
    localStorage.removeItem("RememberMe");
    this.router.navigate(["/login"]);
  }
  openDialogChangePass(): void {
    // let dialogRef: MatDialogRef<ChangePasswordComponent>;
    const dialogRef = this.dialog.open(ChangePasswordComponent, {
      width: "350px",
    });
    dialogRef.afterClosed().subscribe((result) => {
      if (result != undefined) {
        const a = result.value.NewPassword;
        this.ComSRV.postJSON("api/Users/ChangePassword", {
          UserID: this.ComSRV.getUserDetails().UserID,
          UserPassword: a,
          InActive: false,
        }).subscribe(
          (d: any) => {
            if (d == true) {
              this.ComSRV.openSnackBar(
                environment.UpdateMSG.replace("${Name}", "Change Password")
              );
            } else {
              this.ComSRV.ShowError(
                "Given Password is Already existed.Please use another password",
                "Close"
              );
            }
            // this.reset(r);
          },
          (error) => (this.error = error), // error path
          () => {
            this.working = false;
          }
        );
      }
    });
  }
  getDataByFilters() {
    this.getFilteredStatsData();
    this.getSelectedTabData();
    if (this.currentUser.UserLevel == this.enumUserLevel.TSP) {
      this.getFilteredStatsDataPendingClasses();
    }
  }
  getSelectedTabData() {
    switch (this.tabGroup?.selectedIndex ?? 0) {
      case 0:
        this.getSchemeData();
        break;
      case 1:
        this.getTSPData();
        break;
      case 2:
        this.getClassData();
        break;
      case 3:
        this.getTraineeData();
        break;
      default:
    }
  }
  getUnverifiedTraineeEmail(UserId: number) {
    this.Role = this.currentUser.RoleTitle;
    let Url;
    Url = `GetUnverifiedTraineeEmail?TspID=${UserId}`;
    this.ComSRV.getJSON(`api/Scheme/${Url}`).subscribe(
      (data) => {
        this.UnverifiedTraineeEmail = data;
      },
      (error) => {
        this.error = error;
      }
    );
  }
  getSchemes() {
    this.ComSRV.postJSON(
      "api/Scheme/FetchSchemeByUser",
      this.filters
    ).subscribe(
      (d: any) => {
        this.Schemes = d;
        if (this.Schemes[0].PTypeName == "Community") {
          this.hideTilesForCommmunityTSP = true;
        }
      },
      (error) => (this.error = error)
    );
  }
  getSchemeData() {
    let interval = setInterval(() => {
      if (interval && this.Schemes) {
        let newSchemes =
          this.filters.SchemeID > 0
            ? this.Schemes.filter((x) => x.SchemeID == this.filters.SchemeID)
            : this.PageSchemeData; // this.Schemes;
        this.Scheme = newSchemes;
        interval = null;
        clearInterval(interval);
      }
    }, 1000);
  }
  getTSPData() {
    //let filter = "filter?" + Object.entries(this.filters).map(([key, value]) => `filter=${value}`).join('&');
    //this.ComSRV.postJSON(`api/TSPDetail/FetchTSPByUser`, this.filters)
    //  .subscribe(
    //    (d: any) => {
    //      this.TSPs = new MatTableDataSource(d);
    //      this.TSPs.paginator = this.PageTSP;
    //      this.TSPs.sort = this.SortTSP;
    //    }
    //    , error => this.error = error
    //  );
    this.initTSPPagedData();
  }
  SearchSch = new FormControl("");
  SearchCls = new FormControl("");
  SearchTSP = new FormControl("");
  EmptyCtrl(ev: any) {
    this.SearchCls.setValue("");
    this.SearchTSP.setValue("");
    this.SearchSch.setValue("");
  }
  getClassData() {
    //let filter = "filter?" + Object.entries(this.filters).map(([key, value]) => `filter=${value}`).join('&');
    //this.ComSRV.postJSON(`api/Class/FetchClassesByUser`, this.filters)
    //  .subscribe(
    //    (d: any) => {
    //      //  this.Class = new MatTableDataSource(d);
    //      this.classesArray = (d);
    //      this.buildDataSource();
    //    }
    //    , error => this.error = error
    //    // error path
    //  );
    this.initClassPagedData();
  }
  getTraineeData() {
    ////let filter = "filter?" + Object.entries(this.filters).map(([key, value]) => `filter=${value}`).join('&');
    //this.ComSRV.postJSON(`api/TraineeProfile/FetchTraineesByUser`, this.filters)
    //  .subscribe((d: any) => {
    //    this.Trainees = new MatTableDataSource(d);
    //    this.Trainees.paginator = this.PageTrainee;
    //    this.Trainees.sort = this.SortTrainee;
    //  }, error => this.error = error
    //    // error path
    //  );
    this.initTraineePagedData();
  }
  getTSPDetailByScheme(schemeId: number) {
    this.classesArray = [];
    this.tspFilter.setValue(0);
    this.classFilter.setValue(0);
    this.ComSRV.getJSON(
      `api/TSPDetail/GetTSPDetailByScheme/` + schemeId
    ).subscribe(
      (data) => {
        this.TSPDetail = <any[]>data;
      },
      (error) => {
        this.error = error;
      }
    );
  }
  getClassesByTsp(tspId: number) {
    this.classFilter.setValue(0);
    this.ComSRV.getJSON(`api/Class/GetClassesByTsp/` + tspId).subscribe(
      (data) => {
        this.classesArray = <any[]>data;
      },
      (error) => {
        this.error = error;
      }
    );
  }
  getFilteredStatsData() {
    this.ComSRV.postJSON(
      `api/HomeStats/FetchHomeStatsByUser`,
      this.filters
    ).subscribe(
      (data: any) => {
        console.log(data);
        this.homeStates = data;
      },
      (error) => {
        this.error = error;
      }
    );
  }
  getFilteredStatsDataPendingClasses() {
    this.ComSRV.postJSON(
      `api/HomeStats/FetchPendingHomeStatsByUser`,
      this.filters
    ).subscribe(
      (data: any) => {
        this.homePendingStates = data;
        this.Planned = this.homePendingStates[0].Planned;
        this.Active = this.homePendingStates[0].Active;
        this.Completed = this.homePendingStates[0].Completed;
        this.Cancelled = this.homePendingStates[0].Cancelled;
        this.Abandoned = this.homePendingStates[0].Abandoned;
        this.Ready = this.homePendingStates[0].Ready;
        this.Suspended = this.homePendingStates[0].Suspended;
        this.optionsClassStatus.series = [
          {
            name: "Planned",
            data: [this.Planned],
          },
          {
            name: "Active",
            data: [this.Active],
          },
          {
            name: "Completed",
            data: [this.Completed],
          },
          {
            name: "Cancelled",
            data: [this.Cancelled],
          },
          {
            name: "Abandoned",
            data: [this.Abandoned],
          },
          {
            name: "Ready",
            data: [this.Ready],
          },
          {
            name: "Suspended",
            data: [this.Suspended],
          },
        ];
        Highcharts.chart("containerPassed", this.optionsClassStatus);
      },
      (error) => {
        this.error = error;
      }
    );
  }
  toggleRTP(row) {
    this.ComSRV.confirm().subscribe((result) => {
      if (result) {
        this.ComSRV.postJSON("api/Class/RTP", {
          ClassID: row.ClassID,
          RTP: row.RTP,
        }).subscribe(
          (d: any) => {
            this.ComSRV.openSnackBar(
              row.RTP == true
                ? environment.ActiveRTP.replace("${Name}", this.EnText)
                : environment.RemoveRTP.replace("${Name}", this.EnText)
            );
            // this.organization =new MatTableDataSource(d);
          },
          (error) => (this.error = error) // error path
        );
      } else {
        row.RTP = !row.RTP;
      }
    });
  }
  openChangeRequestDialog(): void {
    const dialogRef = this.dialog.open(ChangeRequestDialogComponent, {
      //minWidth: '1000px',
      //minHeight: '600px',
      height: "95%",
      //data: JSON.parse(JSON.stringify(row))
      //data: { "ClassID": id, "IsMasterSheet": true, "VisitDate": null }
      //this.GetVisitPlanData(data)
    });
    dialogRef.afterClosed().subscribe((result) => {
      //console.log(result);
      //this.visitPlan = result;
      //this.submitVisitPlan(result);
    });
  }
  async UnverifiedTraineeEmailAddress() {
    // const Param = this.GetParamString('RD_TspUnverifiedTraineeEmail',  {tspId: this.currentUser.UserID});
    const Param = this.GetParamString("RD_UnverifiedTraineeEmail", {
      tspId: this.currentUser.UserID,
      KamId: 0,
    });
    const data: any = await this.ComSRV.getJSON(
      `api/BSSReports/FetchReportData?Param=${Param}`
    ).toPromise();
    if (data.length > 0) {
      this.ComSRV.ExportToExcel(data, "Unverifed_Trainee_Email");
    } else {
      this.ComSRV.ShowWarning(" No Record Found", "Close");
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
  DraftTrainee: any;
  GetTraineeDraftData() {
    let Url;
    // Url = 'GetTraineeDraftDataByTsp'
    Url = `GetTraineeDraftDataByTsp?TspID=${this.currentUser.UserID}`;
    this.ComSRV.getJSON(`api/TraineeProfile/${Url}`).subscribe(
      (response: any) => {
        let traineeProfileList = response.ListTraineeProfile;
        this.DraftTrainee = traineeProfileList.length;
      }
    );
  }
  openDraftTraineeDialog(tile: string): void {
    const dialogRef = this.dialog.open(DraftTraineeDialogueComponent, {
      height: "55%",
      width: "75%",
      data: { TileName: tile, UserID: this.filters.UserID },
    });
    dialogRef.afterClosed().subscribe((result) => {});
  }
  openPendingClassesDialog(tile: string): void {
    const dialogRef = this.dialog.open(TSPPendingClassesDialogueComponent, {
      //minWidth: '1000px',
      //minHeight: '600px',
      height: "55%",
      width: "60%",
      //data: JSON.parse(JSON.stringify(row))
      data: { TileName: tile, UserID: this.filters.UserID },
      //this.GetVisitPlanData(data)
    });
    dialogRef.afterClosed().subscribe((result) => {
      //console.log(result);
      //this.visitPlan = result;
      //this.submitVisitPlan(result);
    });
  }
  openDialog(row): void {
    this.ComSRV.postJSON("api/RTP/RD_RTPClassDataBy", {
      ClassID: row.ClassID,
    }).subscribe((response: any) => {
      //response = response.reduce((accumulator, value) => accumulator.concat(value), []);
      this.classObj = response[0];
      this.orgConfig = response[1];
      let orgConfigList = response[2];
      //BR Business Rule
      let classStartDate = this.classObj[0].StartDate;
      this.classStartDate = this.classObj[0].StartDate;
      let classEndDate = this.classObj.EndDate;
      this.classEndDate = this.classObj.EndDate;
      let classStart =
        typeof classStartDate == "string"
          ? new Date(classStartDate)
          : classStartDate;
      let today = new Date();
      let openeing = new Date(classStart);
      let closing = new Date(classStart);
      openeing.setDate(classStart.getDate() - this.orgConfig[0].RTPFrom);
      closing.setDate(classStart.getDate() + this.orgConfig[0].RTPTo);
      if (today < openeing || today > closing) {
        this.error = "RTP creation date has been passed";
        this.ComSRV.ShowError(this.error.toString(), null, 6000);
        row.RTP = !row.RTP;
        return;
      }
      const dialogRef = this.dialog.open(RTPDialogComponent, {
        minWidth: "700px",
        minHeight: "350px",
        //data: JSON.parse(JSON.stringify(row))
        data: {
          ClassID: row.ClassID,
          ClassCode: row.ClassCode,
          RTPValue: row.RTP,
          DistrictID: row.DistrictID,
          DistrictName: row.DistrictName,
          TehsilID: row.TehsilID,
          TehsilName: row.TehsilName,
          SchemeName: this.classObj[0].SchemeName,
          TSPName: this.classObj[0].TSPName,
          TradeName: this.classObj[0].TradeName,
          TraineesPerClass: this.classObj[0].TraineesPerClass,
          Duration: this.classObj[0].Duration,
          Name: this.classObj[0].Name,
          CPName: this.classObj[0].CPName,
          CPLandline: this.classObj[0].CPLandline,
          StartDate: this.classObj[0].StartDate,
        },
      });
      dialogRef.afterClosed().subscribe((result) => {
        if (result == undefined || result == false) {
          row.RTP = !row.RTP;
        }
        if (result == true) {
          row.RTP = row.RTP;
        }
      });
    });
  }
  routeToRegistration(row: any) {
    if (this.currentUser.UserLevel == EnumUserLevel.TSP) {
      this.checkTSPColor(row.ClassStatusID);
    }
    this.ComSRV.getJSON(
      `api/InceptionReport/GetInceptionReportByClass?classID=${row.ClassID}`
    ).subscribe(
      (data) => {
        if (this.blacklistedTSPwithRed || this.blacklistedTSPwithBlack) {
          return;
        }
        if (data[0].length > 0) {
          this.router.navigateByUrl(`/registration/trainee/${row.ClassID}`);
        } else {
          this.ComSRV.ShowError(
            "Please submit inception report first, before registration."
          );
        }
      },
      (error) => {
        this.error = error;
      }
    );
  }
  checkTSPColor(classStatusid: number) {
    this.ComSRV.postJSON("api/TSPColor/RD_TSPMasterColorByID", {
      CurUserID: this.currentUser.UserID,
    }).subscribe((response: any) => {
      if (
        response[0].TSPColorID == EnumTSPColorType.Red &&
        classStatusid != EnumClassStatus.Active
      ) {
        this.ComSRV.ShowError(
          "TSPs with color red can only handle active classes"
        );
        this.blacklistedTSPwithRed = true;
        return;
      }
      if (response[0].TSPColorID == EnumTSPColorType.Black) {
        this.ComSRV.ShowError(
          "BlackListed TSPs are not allowed to perform any action"
        );
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
    });
  }
  routeToInceptionReport(row: any) {
    if (this.currentUser.UserLevel == EnumUserLevel.TSP) {
      this.checkTSPColor(row.ClassStatusID);
    }
    this.ComSRV.postJSON("api/InceptionReport/RD_InceptionReportBy", {
      ClassID: row.ClassID,
    }).subscribe((response: any) => {
      let reportSubmissionBracket = <any[]>response[0];
      let InceptionReportData = <any[]>response[1];
      this.classObj = response[2];
      this.orgConfig = response[3];
      let orgConfigList = response[3];
      debugger;
      var data = this.Class.filteredData.filter(
        (a) => a.ClassID === row.ClassID
      );
      if (this.blacklistedTSPwithRed || this.blacklistedTSPwithBlack) {
        return;
      }
      if (orgConfigList.length <= 0) {
        this.error = "Please set 'Rules' of this class before creating report.";
        //this.inceptionreportform.disable({ onlySelf: true });
        this.ComSRV.ShowError(this.error.toString(), "Error");
        return;
      }
      if (
        data[0].ClassStatusID != EnumClassStatus.Ready &&
        InceptionReportData.length == 0
      ) {
        this.error = "Class status must be 'Ready' to submit inception report.";
        //this.inceptionreportform.disable({ onlySelf: true });
        this.ComSRV.ShowError(this.error.toString(), null, 6000);
        return false;
      }
      if (
        data[0].NTP == false &&
        data[0].PTypeName == "Community" &&
        data[0].TrainingAddressLocation == ""
      ) {
        this.ComSRV.ShowError("NTP must be issued");
      } else {
        this.router.navigateByUrl(`/inception-report/inception/${row.ClassID}`);
      }
    });
    //var data = this.Class.filteredData.filter(a => a.ClassID === row.ClassID);
    //console.log(data[0].NTP);
    //console.log(data[0]);
    //if (data[0].ClassStatusID != EnumClassStatus.Ready) {
    //    this.error = "Class status must be 'Ready' to submit inception report.";
    //    //this.inceptionreportform.disable({ onlySelf: true });
    //    this.ComSRV.ShowError(this.error.toString(), null, 6000);
    //    return;
    //}
    //if (data[0].NTP == false && (data[0].PTypeName == 'Community' || data[0].PTypeName == 'Cost Sharing')) {
    //    this.ComSRV.ShowError("NTP must be issued");
    //} else {
    //    this.router.navigateByUrl(`/inception-report/inception/${row.ClassID}`);
    //}
  }
  buildDataSource() {
    //let classData = this.groupBy('ClassStatusName', this.classesArray, this.reducedGroups)
    //let index = 0;
    //classData = classData.map((item) => { if (!item.isGroup) { return { ...item, Sr: ++index } } else { return item } })
    //this.Class = new MatTableDataSource(classData);
    //this.Class.paginator = this.PageClass;
    //this.Class.sort = this.SortClass;
    let classData = this.groupBy(
      "ClassStatusName",
      this.PageClassData,
      this.reducedGroups
    );
    let index = 0;
    classData = classData.map((item) => {
      if (!item.isGroup) {
        return { ...item, Sr: ++index };
      } else {
        return item;
      }
    });
    this.Class = new MatTableDataSource(classData);
  }
  reduceGroup(row) {
    row.reduced = !row.reduced;
    if (row.reduced) this.reducedGroups.push(row);
    else
      this.reducedGroups = this.reducedGroups.filter(
        (el) => el.value != row.value
      );
    this.buildDataSource();
  }
  isGroup(index, item): boolean {
    return item.isGroup;
  }
  groupBy(column: string, data: any[], reducedGroups?: any[]) {
    if (!column) return data;
    let collapsedGroups = reducedGroups;
    if (!reducedGroups) collapsedGroups = [];
    const customReducer = (accumulator, currentValue) => {
      let currentGroup = currentValue[column];
      if (!accumulator[currentGroup])
        accumulator[currentGroup] = [
          {
            groupName: `${currentValue[column]}`,
            value: currentValue[column],
            isGroup: true,
            reduced: collapsedGroups.some(
              (group) => group.value == currentValue[column]
            ),
          },
        ];
      accumulator[currentGroup].push(currentValue);
      return accumulator;
    };
    let groups = data.reduce(customReducer, {});
    let groupArray = Object.keys(groups).map((key) => groups[key]);
    let flatList = groupArray.reduce((a, c) => {
      return a.concat(c);
    }, []);
    return flatList.filter((rawLine) => {
      return (
        rawLine.isGroup ||
        collapsedGroups.every((group) => rawLine[column] != group.value)
      );
    });
  }
  //Pagination\\
  PageSchemeData: any[];
  PageClassData: any[];
  schemeFilter = new FormControl(0);
  tspFilter = new FormControl(0);
  classFilter = new FormControl(0);
  initSchemePagedData() {
    this.SortScheme.sortChange.subscribe(() => (this.PageScheme.pageIndex = 0));
    this.PageScheme.pageSize = 10;
    merge(
      this.SortScheme.sortChange,
      this.PageScheme.page,
      this.schemeFilter.valueChanges,
      this.tspFilter.valueChanges,
      this.classFilter.valueChanges
    )
      .pipe(
        startWith({}),
        switchMap(() => {
          let pagedModel = {
            PageNo: this.PageScheme.pageIndex + 1,
            PageSize: this.PageScheme.pageSize,
            SortColumn: this.SortScheme.active,
            SortOrder: this.SortScheme.direction,
            SearchColumn: "",
            SearchValue: "",
          };
          this.filters.SchemeID = this.schemeFilter.value;
          this.filters.TSPID = this.tspFilter.value;
          this.filters.ClassID = this.classFilter.value;
          return this.getSchemePagedData(pagedModel, this.filters);
        })
      )
      .subscribe(
        (data) => {
          this.PageSchemeData = data[0];
          this.resultsSchemeLength = data[1].TotalCount;
          this.getSchemeData();
        },
        (error) => (this.error = error)
      );
  }
  getSchemePagedData(pagingModel, filterModel) {
    return this.ComSRV.postJSON("api/Scheme/FetchSchemeByUserPaged", {
      pagingModel,
      filterModel,
    });
  }
  initTSPPagedData() {
    this.SortTSP.sortChange.subscribe(() => (this.PageTSP.pageIndex = 0));
    this.PageTSP.pageSize = 10;
    merge(
      this.SortTSP.sortChange,
      this.PageTSP.page,
      this.schemeFilter.valueChanges,
      this.tspFilter.valueChanges,
      this.classFilter.valueChanges
    )
      .pipe(
        startWith({}),
        switchMap(() => {
          let pagedModel = {
            PageNo: this.PageTSP.pageIndex + 1,
            PageSize: this.PageTSP.pageSize,
            SortColumn: this.SortTSP.active,
            SortOrder: this.SortTSP.direction,
            SearchColumn: "",
            SearchValue: "",
          };
          this.filters.SchemeID = this.schemeFilter.value;
          this.filters.TSPID = this.tspFilter.value;
          this.filters.ClassID = this.classFilter.value;
          return this.getTSPPagedData(pagedModel, this.filters);
        })
      )
      .subscribe(
        (data) => {
          this.TSPs = data[0];
          this.resultsTspLength = data[1].TotalCount;
        },
        (error) => (this.error = error)
      );
  }
  getTSPPagedData(pagingModel, filterModel) {
    return this.ComSRV.postJSON("api/TSPDetail/FetchTSPByUserPaged", {
      pagingModel,
      filterModel,
    });
  }
  initClassPagedData() {
    this.SortClass.sortChange.subscribe(() => (this.PageClass.pageIndex = 0));
    this.PageClass.pageSize = 10;
    merge(
      this.SortClass.sortChange,
      this.PageClass.page,
      this.schemeFilter.valueChanges,
      this.tspFilter.valueChanges,
      this.classFilter.valueChanges
    )
      .pipe(
        startWith({}),
        switchMap(() => {
          let pagedModel = {
            PageNo: this.PageClass.pageIndex + 1,
            PageSize: this.PageClass.pageSize,
            SortColumn: this.SortClass.active,
            SortOrder: this.SortClass.direction,
            SearchColumn: "",
            SearchValue: "",
          };
          this.filters.SchemeID = this.schemeFilter.value;
          this.filters.TSPID = this.tspFilter.value;
          this.filters.ClassID = this.classFilter.value;
          return this.getClassPagedData(pagedModel, this.filters);
        })
      )
      .subscribe(
        (data) => {
          this.PageClassData = data[0];
          this.resultsClassLength = data[1].TotalCount;
          this.buildDataSource();
        },
        (error) => (this.error = error)
      );
  }
  getClassPagedData(pagingModel, filterModel) {
    return this.ComSRV.postJSON("api/Class/FetchClassesByUserPaged", {
      pagingModel,
      filterModel,
    });
  }
  initTraineePagedData() {
    this.SortTrainee.sortChange.subscribe(
      () => (this.PageTrainee.pageIndex = 0)
    );
    this.PageTrainee.pageSize = 10;
    merge(
      this.SortTrainee.sortChange,
      this.PageTrainee.page,
      this.schemeFilter.valueChanges,
      this.tspFilter.valueChanges,
      this.classFilter.valueChanges
    )
      .pipe(
        startWith({}),
        switchMap(() => {
          let pagedModel = {
            PageNo: this.PageTrainee.pageIndex + 1,
            PageSize: this.PageTrainee.pageSize,
            SortColumn: this.SortTrainee.active,
            SortOrder: this.SortTrainee.direction,
            SearchColumn: "",
            SearchValue: "",
          };
          this.filters.SchemeID = this.schemeFilter.value;
          this.filters.TSPID = this.tspFilter.value;
          this.filters.ClassID = this.classFilter.value;
          return this.getTraineePagedData(pagedModel, this.filters);
        })
      )
      .subscribe(
        (data) => {
          this.Trainees = data[0];
          this.resultsTraineeLength = data[1].TotalCount;
        },
        (error) => (this.error = error)
      );
  }
  getTraineePagedData(pagingModel, filterModel) {
    return this.ComSRV.postJSON("api/TraineeProfile/FetchTraineesByUserPaged", {
      pagingModel,
      filterModel,
    });
  }
  public optionsClassStatus: any = {
    chart: {
      type: "column",
      height: "300px",
    },
    title: {
      text: "",
    },
    credits: {
      enabled: false,
    },
    legend: {
      enabled: true,
      labelFormatter: function () {
        return this.name + " (" + this.yData[0] + ")";
      },
    },
    xAxis: {
      categories: ["Class Status"],
      crosshair: true,
    },
    yAxis: {
      min: 0,
      title: {
        text: "No of Classes",
      },
    },
    tooltip: {
      headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
      pointFormat:
        '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
        '<td style="padding:0"><b>{point.y:.1f} </b></td></tr>',
      footerFormat: "</table>",
      shared: true,
      useHTML: true,
    },
    plotOptions: {
      column: {
        pointPadding: 0.2,
        borderWidth: 0,
        showInLegend: true,
      },
    },
    series: [],
  };
  getDependantFilters() {
    if (this.currentUser.UserLevel == this.enumUserLevel.TSP) {
      this.getClassesBySchemeFilter();
    } else {
      this.getTSPDetailByScheme(this.filters.SchemeID);
    }
  }
  getClassesBySchemeFilter() {
    this.filters.ClassID = 0;
    this.filters.TraineeID = 0;
    this.ComSRV.postJSON(`api/Class/FetchClassesByUser/`, {
      UserID: this.currentUser.UserID,
      OID: this.ComSRV.OID.value,
      SchemeID: this.filters.SchemeID,
    }).subscribe(
      (data) => {
        this.classesArray = <any[]>data;
        //this.activeClassesArrayFilter = this.classesArrayFilter.filter(x => x.ClassStatusID == 3);
      },
      (error) => {
        this.error = error;
      }
    );
  }
  openTraineeJourneyDialogue(data: any): void {
    debugger;
    this.dialogueService.openTraineeJourneyDialogue(data);
  }
  openClassJourneyDialogue(data: any): void {
    debugger;
    this.dialogueService.openClassJourneyDialogue(data);
  }
  // gis functions
  initializeMap() {
    var mbAttr =
        'Map data &copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors, ' +
        'Imagery © <a href="https://www.mapbox.com/">Mapbox</a>',
      mbUrl =
        "https://api.mapbox.com/styles/v1/{id}/tiles/{z}/{x}/{y}?access_token=pk.eyJ1IjoibWFwYm94IiwiYSI6ImNpejY4NXVycTA2emYycXBndHRqcmZ3N3gifQ.rJcFIG214AriISLbB6B5aw";
    var grayscale = L.tileLayer(mbUrl, {
        id: "mapbox/light-v9",
        tileSize: 512,
        zoomOffset: -1,
        attribution: mbAttr,
      }),
      streets = L.tileLayer(mbUrl, {
        id: "mapbox/streets-v11",
        tileSize: 512,
        zoomOffset: -1,
        attribution: mbAttr,
      }),
      googleStreets = L.tileLayer(
        "http://{s}.google.com/vt/lyrs=m&x={x}&y={y}&z={z}",
        {
          maxZoom: 20,
          subdomains: ["mt0", "mt1", "mt2", "mt3"],
        }
      ),
      googleHybrid = L.tileLayer(
        "http://{s}.google.com/vt/lyrs=s,h&x={x}&y={y}&z={z}",
        {
          maxZoom: 20,
          subdomains: ["mt0", "mt1", "mt2", "mt3"],
        }
      ),
      googleSat = L.tileLayer(
        "http://{s}.google.com/vt/lyrs=s&x={x}&y={y}&z={z}",
        {
          maxZoom: 20,
          subdomains: ["mt0", "mt1", "mt2", "mt3"],
        }
      ),
      googleTerrain = L.tileLayer(
        "http://{s}.google.com/vt/lyrs=p&x={x}&y={y}&z={z}",
        {
          maxZoom: 20,
          subdomains: ["mt0", "mt1", "mt2", "mt3"],
        }
      );
    map = L.map("map", {
      center: [30.5, 70.0],
      zoom: 6,
      layers: [googleStreets],
    });
    provinceLayer = L.geoJSON(provinceGeojson, {
      style: {
        color: "#FF8E23",
        weight: 3,
        opacity: 0.65,
      },
      // onEachFeature: onEachFeature,
      pointToLayer: function (feature, latlng) {
        return L.circleMarker(latlng, {
          radius: 8,
          fillColor: "#FF8E23",
          color: "#000",
          weight: 1,
          opacity: 1,
          fillOpacity: 0.8,
        });
      },
    }).addTo(map);
    districtLayer = L.geoJSON(districtGeojson, {
      style: {
        color: "#009AC4",
        weight: 3,
        opacity: 0.65,
      },
      // onEachFeature: onEachFeature,
      pointToLayer: function (feature, latlng) {
        return L.circleMarker(latlng, {
          radius: 8,
          fillColor: "#ff7800",
          color: "#000",
          weight: 1,
          opacity: 1,
          fillOpacity: 0.8,
        });
      },
    });
    tehsilLayer = L.geoJSON(tehsilsGeojson, {
      style: {
        color: "#00C43E",
        weight: 3,
        opacity: 0.65,
      },
      // onEachFeature: onEachFeature,
      pointToLayer: function (feature, latlng) {
        return L.circleMarker(latlng, {
          radius: 8,
          fillColor: "#00C43E",
          color: "#000",
          weight: 1,
          opacity: 1,
          fillOpacity: 0.8,
        });
      },
    });
    let baseLayers: any = {
      "Google Street": googleStreets,
      "Google Hybrid": googleHybrid,
      "Google Satellite": googleSat,
      "Google Terrain": googleTerrain,
    };
    var overlays = {
      "Province Boundry": provinceLayer,
      "District Boundry": districtLayer,
      "Tehsil Boundry": tehsilLayer,
    };
    L.control.layers(baseLayers, overlays, { collapsed: false }).addTo(map);
    greenIcon = L.icon({
      iconUrl: "./assets/marker-red.png",
      iconSize: [38, 38], // size of the icon
    });
    this.GetData();
    this.getCluster();
    this.getSector();
    this.getClassDataGis();
    this.setMarkersToMapInit();
  }
  GetData() {
    this.ComSRV.getJSON("api/Tehsil/GetTehsil").subscribe(
      (d: any) => {
        this.District = d[1];
        this.MainDistrict = d[1];
        this.Tehsil = d[0];
        this.MainTehsil = d[0];
      },
      (error) => (this.error = error) // error path
    );
  }
  getCluster() {
    this.ComSRV.getJSON("api/Cluster/GetCluster").subscribe(
      (d: any) => {
        this.Clustor = d;
      },
      (error) => (this.error = error) // error path
    );
  }
  getSector() {
    this.ComSRV.getJSON("api/Sector/GetSector").subscribe(
      (d: any) => {
        this.Sector = d;
      },
      (error) => (this.error = error) // error path
    );
  }
  addingMarkers(markerArray) {
    var cmale = 0;
    var cfemale = 0;
    var cboth = 0;
    var cclass = 0;
    markers = L.layerGroup();
    var customOptions = {
      className: "popupCustom",
    };
    console.log(markerArray[0]);
    for (var i = 0; i < markerArray.length; i++) {
      let lon: any = markerArray[i].Longitude;
      let lat: any = markerArray[i].Latitude;
      var newTest: any = Object.values(markers._layers);
      var mk = newTest.find((f) => {
        if (
          f._latlng.lat == markerArray[i].Latitude &&
          f._latlng.lng == markerArray[i].Longitude
        ) {
          return true;
        } else {
          return false;
        }
      });
      if (mk) {
        if (typeof markerArray[i].ClassCode != "undefined") {
          if (markerArray[i].GenderName == "Female") {
            cfemale += 1;
          } else if (markerArray[i].GenderName == "Male") {
            cmale += 1;
          } else if (markerArray[i].GenderName == "Both") {
            cboth += 1;
          }
          cclass += 1;
          mk._popup._content +=
            "<div>Total Classes: " +
            cclass +
            "</div><div>Male: " +
            cmale +
            ", Female: " +
            cfemale +
            ", Both: " +
            cboth +
            "</div><table class='table' style='border-bottom:2px solid black;'>" +
            "<tr><td>Class Code</td><td>" +
            markerArray[i].ClassCode +
            "</td></tr>" +
            "<tr><td>TSP Name</td><td>" +
            markerArray[i].TSPName +
            "</td></tr>" +
            "<tr><td>Cluster Name</td><td>" +
            markerArray[i].ClusterName +
            "</td></tr>" +
            // + "<tr><td>Gender Name</td><td>"+markerArray[i].GenderName+"</td></tr>"
            "<tr><td>Sector Name</td><td>" +
            markerArray[i].SectorName +
            "</td></tr>" +
            // + "<tr><td>Trade Name</td><td>"+markerArray[i].TradeName+"</td></tr>"
            "<tr><td>Training Address Location</td><td>" +
            markerArray[i].TrainingAddressLocation +
            "</td></tr>" +
            "</table>";
        }
      } else {
        if (markerArray[i].GenderName == "Female") {
          cfemale = 1;
        } else if (markerArray[i].GenderName == "Male") {
          cmale = 1;
        } else if (markerArray[i].GenderName == "Both") {
          cboth = 1;
        }
        cclass = 1;
        let popupText =
          "<div>Total Classes: " +
          cclass +
          "</div><div>Male: " +
          cmale +
          ", Female: " +
          cfemale +
          ", Both: " +
          cboth +
          "</div><table class='table' style='border-bottom:2px solid black;'>" +
          "<tr><td>Class Code</td><td>" +
          markerArray[i].ClassCode +
          "</td></tr>" +
          "<tr><td>TSP Name</td><td>" +
          markerArray[i].TSPName +
          "</td></tr>" +
          "<tr><td>Cluster Name</td><td>" +
          markerArray[i].ClusterName +
          "</td></tr>" +
          // + "<tr><td>Gender Name</td><td>"+markerArray[i].GenderName+"</td></tr>"
          "<tr><td>Sector Name</td><td>" +
          markerArray[i].SectorName +
          "</td></tr>" +
          // + "<tr><td>Trade Name</td><td>"+markerArray[i].TradeName+"</td></tr>"
          "<tr><td>Training Address Location</td><td>" +
          markerArray[i].TrainingAddressLocation +
          "</td></tr>" +
          "</table>";
        var markerLocation = new L.LatLng(lat, lon);
        var marker = new L.Marker(markerLocation, { icon: greenIcon });
        marker.on("click", (e: any) => {
          map.setView(e.latlng, 10);
        });
        markers.addLayer(marker);
        marker.bindPopup(popupText, customOptions);
      }
    }
    map.addLayer(markers);
  }
  setMarkersToMapInit() {
    this.makeMarker.subscribe((data: any) => {
      this.markerObject = data;
      this.addingMarkers(data);
    });
  }
  getClassDataGis() {
    this.ComSRV.getJSON("api/Class/GetClass").subscribe(
      (d: any) => {
        this.makeMarker.next(d[0]);
      },
      (error) => (this.error = error) // error path
    );
  }
  selectDistrict(district) {
    if (district == "%") {
      map.removeLayer(districtLayer);
      map.removeLayer(tehsilLayer);
      map.addLayer(provinceLayer);
      this.Tehsil = "";
      districtLayer.eachLayer(function (layer) {
        layer.setStyle({ color: "#009AC4" });
      });
      map.setView(new L.LatLng(30.5, 70.0), 6);
      this.removeMarkers();
      this.setMarkersForDistrict(district);
    } else {
      this.selectedDistrict = {
        DistrictID: district.DistrictID,
        DistrictName: district.DistrictName,
      };
      console.log(district);
      map.removeLayer(provinceLayer);
      map.removeLayer(tehsilLayer);
      map.addLayer(districtLayer);
      this.Tehsil = this.MainTehsil.filter(
        (tehs) => tehs.DistrictID == district.DistrictID
      );
      districtLayer.eachLayer(function (layer) {
        layer.setStyle({ color: "#009AC4" });
        if (layer.feature.properties.ADM2_EN == district.DistrictName) {
          map.fitBounds(layer.getBounds());
          layer.setStyle({ color: "cyan" });
        }
      });
      this.removeMarkers();
      this.setMarkersForDistrict(district.DistrictID);
    }
  }
  selectTehsil(tehsil) {
    if (tehsil == "%") {
      // console.log(this.selectedDistrict);
      // this.removeMarkers();
      // this.setMarkersForTehsil(tehsil);
      // map.removeLayer(districtLayer);
      // map.removeLayer(tehsilLayer);
      // map.addLayer(provinceLayer);
      // this.Tehsil = this.MainTehsil;
      // tehsilLayer.eachLayer(function(layer){
      //   layer.setStyle({color :'#00C43E'});
      // });
      // map.setView(new L.LatLng(30.5, 70.0), 6);
      map.removeLayer(provinceLayer);
      map.removeLayer(tehsilLayer);
      map.addLayer(districtLayer);
      this.Tehsil = this.MainTehsil.filter(
        (tehs) => tehs.DistrictID == this.selectedDistrict.DistrictID
      );
      var ddn = this.selectedDistrict.DistrictName;
      districtLayer.eachLayer(function (layer) {
        layer.setStyle({ color: "#009AC4" });
        if (layer.feature.properties.ADM2_EN == ddn) {
          map.fitBounds(layer.getBounds());
          layer.setStyle({ color: "cyan" });
        }
      });
      this.removeMarkers();
      this.setMarkersForDistrict(this.selectedDistrict.DistrictID);
    } else {
      this.removeMarkers();
      this.setMarkersForTehsil(tehsil.TehsilID);
      map.removeLayer(provinceLayer);
      map.removeLayer(districtLayer);
      map.addLayer(tehsilLayer);
      tehsilLayer.eachLayer(function (layer) {
        layer.setStyle({ color: "#00C43E" });
        if (layer.feature.properties.ADM3_EN == tehsil.TehsilName) {
          map.fitBounds(layer.getBounds());
          layer.setStyle({ color: "cyan" });
        }
      });
    }
  }
  selectSector(sector) {
    if (sector == "%") {
      this.removeMarkers();
      this.setMarkersForSector(sector);
      map.setView(new L.LatLng(30.5, 70.0), 6);
    } else {
      this.removeMarkers();
      this.setMarkersForSector(sector.SectorID);
    }
  }
  selectCluster(clustor) {
    console.log(clustor);
    if (clustor == "%") {
      this.removeMarkers();
      this.setMarkersForClustor(clustor);
    } else {
      this.District = this.MainDistrict.filter(
        (clus) => clus.ClusterID == clustor.ClusterID
      );
      this.removeMarkers();
      this.setMarkersForClustor(clustor.ClusterID);
    }
  }
  removeMarkers() {
    map.removeLayer(markers);
  }
  setMarkersForDistrict(id) {
    if (id == "%") {
      var newMarkers = this.markerObject;
      this.addingMarkers(newMarkers);
    } else {
      var newMarkers = this.markerObject.filter((d) => d.DistrictID === id);
      console.log(newMarkers);
      this.addingMarkers(newMarkers);
    }
  }
  setMarkersForClustor(id) {
    if (id == "%") {
      var newMarkers = this.markerObject;
      this.addingMarkers(newMarkers);
    } else {
      var newMarkers = this.markerObject.filter((d) => d.ClusterID === id);
      this.addingMarkers(newMarkers);
    }
  }
  setMarkersForSector(id) {
    if (id == "%") {
      var newMarkers = this.markerObject;
      this.addingMarkers(newMarkers);
    } else {
      var newMarkers = this.markerObject.filter((d) => d.SectorID === id);
      console.log(newMarkers);
      this.addingMarkers(newMarkers);
    }
  }
  setMarkersForTehsil(id) {
    if (id == "%") {
      var newMarkers = this.markerObject;
      this.addingMarkers(newMarkers);
    } else {
      var newMarkers = this.markerObject.filter((d) => d.TehsilID === id);
      console.log(newMarkers);
      this.addingMarkers(newMarkers);
    }
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
