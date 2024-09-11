import { filter } from 'rxjs/operators';
import { Component, OnInit, ViewChild, ElementRef } from "@angular/core";
import { CommonSrvService } from "../../common-srv.service";
import { MatPaginator } from "@angular/material/paginator";
import { MatSort } from "@angular/material/sort";
import { MatTableDataSource } from "@angular/material/table";
import {
  FormGroup,
  FormBuilder,
  Validators,
  FormGroupDirective,
} from "@angular/forms";
import { environment } from "../../../environments/environment";
import { UserRightsModel } from "../../master-data/users/users.component";
import { ActivatedRoute } from "@angular/router";
import * as XLSX from "xlsx";
import * as fileSaver from "file-saver";
import {
  ExportType,
  EnumExcelReportType,
  PBTESheetNames,
} from "../../shared/Enumerations";
import { Workbook } from "exceljs";
import * as fs from "file-saver";
import { ExportExcel } from "../../shared/Interfaces";
import { FormControl } from "@angular/forms";
import { GroupByPipe } from "angular-pipes";
import { DialogueService } from "src/app/shared/dialogue.service";
import { SelectionModel } from "@angular/cdk/collections";
import * as _moment from "moment";
import { Moment } from "moment";
const moment = _moment;
import { DatePipe } from "@angular/common";
import { MatDatepicker } from "@angular/material/datepicker";
import { debug } from 'console';
// See the Moment.js docs for the meaning of these formats:
// https://momentjs.com/docs/#/displaying/format/
export const MY_FORMATS = {
  parse: {
    dateInput: "MM/YYYY",
  },
  display: {
    dateInput: "MM/YYYY",
    monthYearLabel: "MMM YYYY",
    dateA11yLabel: "LL",
    monthYearA11yLabel: "MMMM YYYY",
  },
};
@Component({
  selector: "hrapp-pbte",
  templateUrl: "./pbte.component.html",
  styleUrls: ["./pbte.component.scss"],
  providers: [DatePipe],
})
export class PBTEComponent implements OnInit {
  selection = new SelectionModel<any>(true, []);
  ParentSchemeName: any = "";
  PbteDBFile: any;
  _examDataArray: any = [];
  _tsrDataArray: any = [];
  isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.hTablesData.filteredData.length;
    return numSelected === numRows;
  }
  masterToggle() {
    this.isAllSelected()
      ? this.selection.clear()
      : this.hTablesData.filteredData.forEach((row) =>
        this.selection.select(row)
      );
    // console.log(numRows)
  }
  _fileUploadForm: FormGroup;
  initForm() {
    this._fileUploadForm = this.fb.group({
      excelFile: ["", Validators.required],
    });
  }
  environment = environment;
  pbteform: FormGroup;
  title: string;
  savebtn: string;
  filters: IQueryFilters = {
    SchemeID: 0,
    TSPID: 0,
    ClassID: 0,
    TradeID: 0,
    DistrictID: 0,
  };
  navttcfilters: IQueryFilters = {
    SchemeID: 0,
    TSPID: 0,
    ClassID: 0,
    TradeID: 0,
    DistrictID: 0,
  };

  //exportAsConfig: ExportAsConfig
  displayedColumnsClasses = [
    "SchemeName",
    "PBTESchemeName",
    "Batch",
    "TSPName",
    "ClassCode",
    "TradeName",
    "TrainingAddressLocation",
    "PBTEAddress",
    "TehsilName",
    "DistrictName",
    "CertAuthName",
    "TraineesPerClass",
    "GenderName",
    "Duration",
    "StartDate",
    "EndDate",
    "ClassStatusName",
  ];
  displayedColumnsTsps = [
    //'TSPID',
    "TSPName",
    "TSPCode",
    //'ClassID',
    "Address",
    "HeadName",
    "HeadDesignation",
    "HeadEmail",
    "HeadLandline",
    "OrgLandline",
    "Website",
    "CPName",
    "CPDesignation",
    "CPEmail",
    "CPLandline",
  ];
  displayedColumnsTrainees = [
    "ClassCode",
    "Batch",
    "TradeName",
    //'TradeID',
    "TraineeName",
    "TraineeCode",
    "FatherName",
    "TraineeCNIC",
    "DateOfBirth",
    "Education",
    "TraineeImg",
    "TraineeID",
    "PBTEID",
    "CollegeID",
  ];
  displayedColumnsExamResultTrainees = [
    "ClassCode",
    "Batch",
    "TradeName",
    //'TradeID',
    "TraineeName",
    "TraineeCode",
    "StatusName",
    "ResultStatusName",
    "FatherName",
    "TraineeCNIC",
    "DateOfBirth",
    "Education",
    //'TraineeImg',
    "TraineeID",
    "PBTEID",
    "CollegeID",
  ];
  displayedColumnsDropOutTrainees = [
    "ClassCode",
    "Batch",
    "TradeName",
    //'TradeID',
    "TraineeName",
    "FatherName",
    "TraineeCNIC",
    "DateOfBirth",
    "Education",
    "TraineeID",
    "PBTEID",
    "CollegeID",
    "StatusName",
  ];
  displayedColumnsTrades = ["PBTETradeName", "PBTETradeDuration"];
  hTablesData: MatTableDataSource<any>;
  @ViewChild("hpaginator") hpaginator: MatPaginator;
  @ViewChild("hsort") hsort: MatSort;
  hTableColumns = [];
  tTablesData: MatTableDataSource<any>;
  @ViewChild("tpaginator") tpaginator: MatPaginator;
  @ViewChild("tsort") tsort: MatSort;
  tTableColumns = [];
  cTablesData: MatTableDataSource<any>;
  @ViewChild("cpaginator") cpaginator: MatPaginator;
  @ViewChild("csort") csort: MatSort;
  cTableColumns = [];
  traineeTablesData: MatTableDataSource<any>;
  @ViewChild("traineepaginator") traineepaginator: MatPaginator;
  @ViewChild("traineesort") traineesort: MatSort;
  traineeTableColumns = [];

  examTablesData: MatTableDataSource<any>;
  @ViewChild("exampaginator") exampaginator: MatPaginator;
  @ViewChild("examsort") examsort: MatSort;
  examTableColumns = [];

  tradeTablesData: MatTableDataSource<any>;
  @ViewChild("tradepaginator") tradepaginator: MatPaginator;
  @ViewChild("tradesort") tradesort: MatSort;
  tradeTableColumns = [];
  date = new FormControl(moment());
  chosenYearHandler(normalizedYear: Moment) {
    const ctrlValue = this.date.value;
    ctrlValue.year(normalizedYear.year());
    this.date.setValue(ctrlValue);
  }
  chosenMonthHandler(
    normalizedMonth: Moment,
    datepicker: MatDatepicker<Moment>
  ) {
    const ctrlValue = this.date.value;
    ctrlValue.month(normalizedMonth.month());
    this.date.setValue(ctrlValue);
    datepicker.close();
  }
  groupedPbteTSPs: MatTableDataSource<any>;
  groupedTSPsArray: any;
  groupednavttcTSPsArray: any;
  pbteClasses: MatTableDataSource<any>;
  pbteTSPs: MatTableDataSource<any>;
  pbteTrainees: MatTableDataSource<any>;
  pbteExamResultTrainees: MatTableDataSource<any>;
  pbteExamResultTraineesAfterReset: MatTableDataSource<any>;
  navttcExamResultTrainees: MatTableDataSource<any>;
  navttcExamResultTraineesAfterReset: MatTableDataSource<any>;
  pbteDropOutTrainees: MatTableDataSource<any>;
  pbteTrades: MatTableDataSource<any>;
  pbteTradesAfterReset: MatTableDataSource<any>;
  pbteClassesAfterReset: MatTableDataSource<any>;
  pbteTSPsAfterReset: MatTableDataSource<any>;
  pbteTraineesAfterReset: MatTableDataSource<any>;
  pbteDropOutTraineesAfterReset: MatTableDataSource<any>;
  navttcClasses: MatTableDataSource<any>;
  navttcTSPs: MatTableDataSource<any>;
  navttcTrainees: MatTableDataSource<any>;
  navttcDropOutTrainees: MatTableDataSource<any>;
  navttcClassesAfterReset: MatTableDataSource<any>;
  navttcTSPsAfterReset: MatTableDataSource<any>;
  navttcTraineesAfterReset: MatTableDataSource<any>;
  navttcDropOutTraineesAfterReset: MatTableDataSource<any>;
  groupedTSPsArrayForData = [];
  groupednavttcTSPsArrayForData = [];
  selectedClasses: any;
  pbteClassesArray: any;
  pbteTSPsArray: any;
  pbteTSPsArrayToFilter: any;
  pbteTraineesArray: any;
  pbteExamResultTraineesArray: any;
  navttcExamResultTraineesArray: any;
  pbteTradesArray: any;
  navttcClassesArray: any;
  navttcTSPsArray: any;
  navttcTSPsArrayToFilter: any;
  navttcTraineesArray: any;
  selectedTrainees: any;
  selectedTSPs: any;
  selectedTrades: any;
  dropoutTrainees: any;
  navttcdropouts: any;
  Schemes: any;
  navttcSchemes: any;
  TSPDetail: any;
  navttcTSPDetail: any;
  classesArray: any;
  navttcclassesArray: any;
  Trade: any;
  navttcTrade: any;
  District: any;
  navttcDistrict: any;
  pbteStats: any;
  pbteStatsAfterReset: any;
  navttcStats: any;
  navttcStatsAfterReset: any;
  schemeForExport: any;
  pbteTraineesExamScriptArray: any;
  navttcTraineesSqlScriptArray: any;
  navttcTraineesRegisterSqlScriptArray: any;
  data: any;
  traineeResultStatusTypes: any;
  update: String;
  isOpenRegistration: boolean = false;
  isOpenRegistrationMessage: string = "";
  formrights: UserRightsModel;
  EnText: string = "PBTE";
  error: String;
  query = {
    order: "IncepReportID",
    limit: 5,
    page: 1,
  };
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild("SortTrainee") SortTrainee: MatSort;
  @ViewChild("PageTrainee") PageTrainee: MatPaginator;
  @ViewChild("SortExamResultTrainee") SortExamResultTrainee: MatSort;
  @ViewChild("PageExamResultTrainee") PageExamResultTrainee: MatPaginator;
  @ViewChild("SortDropOutTrainee") SortDropOutTrainee: MatSort;
  @ViewChild("PageDropOutTrainee") PageDropOutTrainee: MatPaginator;
  @ViewChild("SortTSP") SortTSP: MatSort;
  @ViewChild("PageTSP") PageTSP: MatPaginator;
  @ViewChild("SortClass") SortClass: MatSort;
  @ViewChild("PageClass") PageClass: MatPaginator;
  @ViewChild("SortTrade") SortTrade: MatSort;
  @ViewChild("PageTrade") PageTrade: MatPaginator;
  @ViewChild("SortNavttcTrainee") SortNavttcTrainee: MatSort;
  @ViewChild("PageNavttcTrainee") PageNavttcTrainee: MatPaginator;
  @ViewChild("SortNavttcDropOutTrainee") SortNavttcDropOutTrainee: MatSort;
  @ViewChild("PageNavttcDropOutTrainee") PageNavttcDropOutTrainee: MatPaginator;
  @ViewChild("SortNavttcTSP") SortNavttcTSP: MatSort;
  @ViewChild("PageNavttcTSP") PageNavttcTSP: MatPaginator;
  @ViewChild("SortNavttcClass") SortNavttcClass: MatSort;
  @ViewChild("PageNavttcClass") PageNavttcClass: MatPaginator;
  @ViewChild("SortNAVTTCExamResultTrainee")
  SortNAVTTCExamResultTrainee: MatSort;
  @ViewChild("PageNAVTTCExamResultTrainee")
  PageNAVTTCExamResultTrainee: MatPaginator;
  @ViewChild("tabGroup") tabGroup;
  @ViewChild("tabGroupS") tabGroupS;
  @ViewChild("tabGroupN") tabGroupN;
  working: boolean;
  constructor(
    private fb: FormBuilder,
    private ComSrv: CommonSrvService,
    private route: ActivatedRoute,
    public dialogueService: DialogueService,
    private _date: DatePipe
  ) {
    this.pbteform = this.fb.group(
      {
        PBTEID: 0,
        InActive: "",
      },
      { updateOn: "blur" }
    );
    this.pbteTrainees = new MatTableDataSource([]);
    this.formrights = ComSrv.getFormRights("/pbte");
  }
  SearchSch = new FormControl("");
  SearchCls = new FormControl("");
  SearchTSP = new FormControl("");
  SearchTrade = new FormControl("");
  SearchDistrict = new FormControl("");
  EmptyCtrl(ev: any = "") {
    this.BSearchCtr.setValue("");
    this.SearchCls.setValue("");
    this.SearchTSP.setValue("");
    this.SearchSch.setValue("");
    this.SearchCtr.setValue("");
    this.SearchTrade.setValue("");
    this.SearchDistrict.setValue("");
  }
  ngOnInit() {
    this.cTablesData = new MatTableDataSource([]);
    this.hTablesData = new MatTableDataSource([]);
    this.tTablesData = new MatTableDataSource([]);
    this.examTablesData = new MatTableDataSource([]);
    this.traineeTablesData = new MatTableDataSource([]);

    this.ComSrv.setTitle("PBTE");
    this.title = "Add New ";
    this.savebtn = "Save ";
    //this.GetData();
    //this.getPBTEStatsData();
    //this.GetPBTEFiltersData();
    this.GetPBTEFiltersData();
    this.getSelectedTabData();
    this.initForm();

    this.getPBTEClassData();
    // this.GetPbteData("Scheme")
    // this.GetPbteData("Class")
    // this.GetPbteData("CenterLocation")
  }
  getSelectedTabData() {
    switch (this.tabGroup?.selectedIndex ?? 0) {
      case 0:
        this.GetPbteData("Scheme");
        break;
      case 1:
        this.getPBTESelectedSubTabData();
        this.getPBTEStatsData();
        //this.GetPBTEFiltersData();
        break;
      case 1:
        this.getNAVTTCStatsData();
        //this.GetPBTEFiltersData();
        this.getNAVTTCSelectedSubTabData();
        break;
      default:
    }
  }
  getPBTEStatsData() {
    this.ComSrv.postJSON("api/PBTE/GetPBTEStatsData", this.filters).subscribe(
      (d: any) => {
        this.pbteStats = d[0];
        this.pbteStatsAfterReset = d[0];
        //this.getPBTEClassData();
      },
      (error) => (this.error = error) // error path
    );
  }
  getNAVTTCStatsData() {
    this.ComSrv.postJSON(
      "api/PBTE/GetNAVTTCStatsData",
      this.navttcfilters
    ).subscribe(
      (d: any) => {
        this.navttcStats = d[0];
        this.navttcStatsAfterReset = d[0];
        //this.getNAVTTCClassData();
      },
      (error) => (this.error = error) // error path
    );
  }
  getPBTESelectedSubTabData() {
    switch (this.tabGroupS?.selectedIndex ?? 0) {
      case 0:
        this.getPBTEClassData();
        break;
      case 1:
        this.getPBTETSPData();
        break;
      case 2:
        this.getPBTETraineeData();
        break;
      case 3:
        this.getPBTEDropOutTraineeData();
        break;
      //case 4:
      //  this.getPBTETradeData();
      //  break;
      case 4:
        this.getPBTEExamResultTraineeData();
        break;
      default:
    }
  }
  getNAVTTCSelectedSubTabData() {
    switch (this.tabGroupN?.selectedIndex ?? 0) {
      case 0:
        this.getNAVTTCClassData();
        break;
      case 1:
        this.getNAVTTCTSPData();
        break;
      case 2:
        this.getNAVTTCTraineeData();
        break;
      case 3:
        this.getNAVTTCDropOutTraineeData();
        break;
      //case 4:
      //  this.getNAVTTCExamResultTraineeData();
      //  break;
      default:
    }
  }
  selectedPbteCenter: any = 0;
  selectedPbteTrade: any = 0;
  SearchCtr = new FormControl("");
  onSelectionChange(row: any) {
    debugger;
    let titleConfirm = "Confirmation";
    let messageConfirm = "Are you sure you want to map the selected data?";
    this.ComSrv.confirm(titleConfirm, messageConfirm).subscribe(
      (isConfirm: Boolean) => {
        if (isConfirm == true) {
          this.SavePBTECenterMapping(row);
        } else {
          row.selectedPbteCenter = "";
        }
      }
    );
  }

  onTradeSelectionChange(row: any) {
    let titleConfirm = "Confirmation";
    let messageConfirm = "Are you sure you want to map the selected data?";
    this.ComSrv.confirm(titleConfirm, messageConfirm).subscribe(
      (isConfirm: Boolean) => {
        if (isConfirm == true) {
          this.SavePBTETradeMapping(row);
        } else {
          row.selectedPbteTrade = "";
        }
      }
    );
  }

  BSearchCtr = new FormControl("");
  pbteTSPLocation: any;
  pbtecourse: any;
  GetDataObject: any = {};
  _dataObject: any;
  getPBTEClassData() {
    this.ComSrv.postJSON("api/PBTE/GetPBTEClasses", this.filters).subscribe(
      (d: any) => {
        this.pbteClasses = new MatTableDataSource(d[0]);
        this.pbteClassesAfterReset = new MatTableDataSource(d[0]);
        this.pbteClassesArray = d[0];
        this.pbteClasses.paginator = this.PageClass;
        this.pbteClasses.sort = this.SortClass;
        // this.GetScheme(d[0], d[1]);
        // this.GetTrade(d[0],d[2]);
        // this.LoadMatTable(d[2], "CenterLocationMapping");
        this.GetDataObject["pbte"] = d[3];
        this.pbtecourse = [];
        this.GetDataObject["trade"] = d[4];
      },
      (error) => (this.error = error) // error path
    );
  }
  pbteScheme: any = [];
  selectedPbteScheme: string = "";
  onSchemeChange() {
    if (this.selectedPbteScheme == "New Scheme") {
      this.ParentSchemeName = this.selectedPbteScheme;
    } else {
      this.ParentSchemeName = "";
    }
  }
  pbteTrade: any = [];
  TraineeUrduData: any = [];
  async GetPbteData(reportName: string = "Scheme") {
    this.hTablesData = new MatTableDataSource([]);
    this.cTablesData = new MatTableDataSource([]);
    this.tTablesData = new MatTableDataSource([]);
    this.tradeTablesData = new MatTableDataSource([]);
    this.examTablesData = new MatTableDataSource([]);
    this.traineeTablesData = new MatTableDataSource([]);
    const _month = moment(this.date.value).format("YYYY-MM");
    const data = { month: _month, report: reportName };
    const response = await this.ComSrv.postJSON(
      "api/PBTE/GetPbteData",
      data
    ).toPromise();
    this._dataObject = response;

    if (response && this._dataObject.data.length > 0) {
      if (reportName == "Scheme") {
        this.pbteScheme = this._dataObject.mappedScheme;
        this.LoadMatTable(this._dataObject.data, "SchemeMapping");
      }

      if (reportName == "Trade") {
        this.LoadMatTable(this._dataObject.data, "Trade");
      }
      if (reportName == "CenterLocation") {
        this.LoadMatTable(this._dataObject.data, "CenterLocationMapping");
      }
      if (reportName == "Class") {
        this.LoadMatTable(this._dataObject.data, "Class");
      }
      if (reportName == "Exam") {
        this.LoadMatTable(this._dataObject.data, "ExamData");
      }
      if (reportName == "Trainee") {
        this.LoadMatTable(this._dataObject.data, "TraineeData");
        // this.TraineeUrduData = this._dataObject.data.filter(
        //   (x) => this.containsUrdu(x.TraineeName) == true
        // );
      }

      if (reportName == "ExaminationSqlScript") {
        let textarray: any = [];
        this._dataObject.data.forEach((item) => {
          var text = `insert into dbo.Examination (ExamID,ExamSessionUrdu,ExamSessionenglish,maincategoryid,BatchNo,ExamYear,StartDate,EndDate,Active,ExamSession,Userid,Entrydate) values(${item.examId
            },'${item.ExamSessionUrdu}','${item.ExamSessionenglish}',${item.maincategoryid
            },${item.BatchNo},${item.ExamYear},'${item.StartDate}','${item.EndDate
            }',${item.Active == true ? 1 : 0},'${item.ExamSession}',44,'${this._date.transform(new Date().toISOString(), "yyyy-MM-dd HH:mm")}');`;
          textarray.push(text);
        });
        var blob = new Blob([textarray.join("\r\n")], {
          type: "text/plain;charset=utf-8, endings: 'native'",
        });
        fileSaver.saveAs(blob, "PBTE-Examination-Sql-Script.sql");
      }

      if (reportName == "TraineeSqlScript") {
        let textarray: any = [];
        this._dataObject.data.forEach((item) => {
          let obj = {};
          this.displayedColumnsTrainees.forEach((key) => {
            obj[key] = item[key] || item;
            console.log(item.Batch);
          });
          var text = `insert into dbo.Student (ExamId,CollegeId,course_categoryId,Lock,ResultLocked,StudentName,FathersName,DateofBirth,Qualification,Gender,active,Class_Code,NIC,Trainee_ID,Shift,Shift_Time_From,Shift_Time_To,Userid,Entrydate) values(${item.ExamId
            },${item.CollegeId},${item.course_categoryId},1,1,'${item.StudentName.trim()
            }','${item.FathersName.trim()}','${this._date.transform(item.DateofBirth, "dd-MM-yyyy")}','${item.Qualification
            }',${item.Gender},${1},'${item.Class_Code}','${item.NIC}','${item.Trainee_ID
            }','${item.Shift}','${item.Shift_Time_From}','${item.Shift_Time_To
            }',44,'${this._date.transform(new Date().toISOString(), "yyyy-MM-dd HH:mm")}');`;
          textarray.push(text);
        });
        var blob = new Blob([textarray.join("\r\n")], {
          type: "text/plain;charset=utf-8, endings: 'native'",
        });
        fileSaver.saveAs(blob, "PBTE-Trainee-Sql-Script.sql");
      }
    } else {
      this.ComSrv.ShowError("No data found for the selected month");
    }
  }

  containsUrdu(text: any) {
    const urduRegex = /[\u0600-\u06FF\u0750-\u077F]/;
    return urduRegex.test(text);
  }

  mappedPBTEScheme: any = [];
  saveMappedScheme() {
    let _schemeName;
    if (this.selectedPbteScheme == "New Scheme") {
      _schemeName = this.ParentSchemeName.trim();
    } else {
      _schemeName = this.selectedPbteScheme;
    }
    const _schemeList = this.selection.selected;

    if (_schemeList.length == 0) {
      this.ComSrv.ShowError("Please select at least one row");
      return;
    }
    if (_schemeName == "" || _schemeName == null) {
      this.ComSrv.ShowError("Please Enter a scheme");
      return;
    }
    _schemeList.forEach((obj) => {
      obj.PBTESchemeName = _schemeName;
    });

    this.ComSrv.postJSON("api/PBTE/SaveSchemeMapping", _schemeList).subscribe(
      (data: any) => {
        this.mappedPBTEScheme = data;
        this.selection.clear();
        this.search = "";
        this.selectedPbteScheme = "";
        this.GetPbteData("Scheme");
        this.ParentSchemeName = "";
      },
      (error) => (this.error = error) // error path
    );
  }
  getSelectedFile() {
    // debugger
    if (this.PbteDBFile) {
      const _pbteDBFile = this.PbteDBFile;
      this.ComSrv.postJSON("api/PBTE/SavePbteDBFile", {
        pbteFile: _pbteDBFile,
      }).subscribe(
        (data: any) => {
          this.PbteDBFile = "";
          if (data == true) {
            this.ComSrv.openSnackBar("PBTE DB File saved successfully.");
          } else {
            this.ComSrv.ShowError("Failed to save");
          }
        },
        (error) => {
          this.ComSrv.ShowError(`${error.message}`, "Close", 5000);
        }
      );
    } else {
      this.ComSrv.ShowError("Please select a PBTE DB file");
    }
  }
  SavePBTECenterMapping(row: any) {
    const data: any = {
      PBTECollegeID: row.selectedPbteCenter,
      TSPName: row.BSSTSP,
      TSPCenterLocation: row.BSSCenter,
      TSPCenterDistrict: row.BSSDistrict,
    };
    this.ComSrv.postJSON("api/PBTE/SavePBTECenterMapping", data).subscribe(
      (d: any) => {
        this.GetPbteData("CenterLocation");
      },
      (error) => (this.error = error) // error path
    );
  }
  SavePBTETradeMapping(row: any): void {
    debugger;
    const data = {
      PBTETradeID: row.selectedPbteTrade,
      TradeName: row.BSSTrade,
      TradeID: row.TradeID,
      Duration: row.Duration,
    };

    this.ComSrv.postJSON("api/PBTE/SavePBTETradeMapping", data).subscribe(
      () => {
        this.GetPbteData("Trade");
      },
      (error) => {
        this.error = error;
        // Optionally, handle the error (e.g., show a message to the user)
      }
    );
  }
  openedSelection(row: any) {
    this.pbtecourse = [];
    const data = this.GetDataObject.pbte.filter(
      (x) => x.College_Name == row.BSSTSP && x.District_Name == row.BSSDistrict
    );
    this.pbtecourse = data;
    if (this.pbtecourse.length == 0) {
      this.ComSrv.ShowError("No TSP found in the PBTE Database");
    }
    console.log(data);
  }

  openedTradeSelection(row: any) {
    this.pbteTrade = [];
    const data = this.GetDataObject.trade.filter(
      (x) => x.CategoryName == row.PBTESchemeName
    );
    this.pbteTrade = data;
    if (this.pbteTrade.length == 0) {
      this.ComSrv.ShowError(
        "No trade found with the selected scheme in the PBTE database."
      );
    }
    console.log(data);
  }

  camelCaseToWords(input: string): string {
    return input.replace(/([a-z])([A-Z])/g, "$1 $2");
  }
  DataExcelExport(Data: any, ReportName: string) {
    if (ReportName === "PBTE_MSR") {
      const Error = Data.find((element) =>
        element.PBTESchemeName.includes("Required")
      );

      if (Error) {
        this.ComSrv.ShowError("Mapping required for " + Error.SchemeName);
        return;
      }
    }

    this.ComSrv.ExcelExporWithForm(Data, ReportName);
  }

  LoadMatTable(tableData: any, tableName: string) {
    //  debugger;
    if (tableName == "SchemeMapping") {
      this.hTableColumns = Object.keys(tableData[0]).filter(
        (key) => !key.includes("ID")
      );
      this.hTablesData = new MatTableDataSource(tableData);
      this.hTablesData.paginator = this.hpaginator;
      this.hTablesData.sort = this.hsort;
    }

    if (tableName == "Trade") {
      this.tradeTableColumns = Object.keys(tableData[0]).filter(
        (key) => !key.includes("ID")
      );
      this.tradeTableColumns.push("PBTE Trade");
      this.tradeTablesData = new MatTableDataSource(tableData);
      this.tradeTablesData.paginator = this.tradepaginator;
      this.tradeTablesData.sort = this.tradesort;
    }

    if (tableName == "Class") {
      this.cTableColumns = Object.keys(tableData[0]).filter(
        (key) => !key.includes("ID")
      );
      this.cTablesData = new MatTableDataSource(tableData);
      this.cTablesData.paginator = this.cpaginator;
      this.cTablesData.sort = this.csort;
    }
    if (tableName == "CenterLocationMapping") {
      // debugger
      const excludeColumnArray = [];
      this.tTableColumns = Object.keys(tableData[0]).filter(
        (key) => !excludeColumnArray.includes(key)
      );
      this.tTableColumns.push("PBTE Center Location");
      this.tTablesData = new MatTableDataSource(tableData);
      this.tTablesData.paginator = this.tpaginator;
      this.tTablesData.sort = this.tsort;
    }
    if (tableName == "ExamData") {
      // debugger
      const excludeColumnArray = [];
      this.examTableColumns = Object.keys(tableData[0]).filter(
        (key) => !excludeColumnArray.includes(key)
      );
      this.examTablesData = new MatTableDataSource(tableData);
      this.examTablesData.paginator = this.exampaginator;
      this.examTablesData.sort = this.examsort;
    }

    if (tableName == "TraineeData") {
      debugger
      this.TraineeUrduData = tableData.filter(
        (x) => this.containsUrdu(x.TraineeName) == true
      );

      const excludeColumnArray = [];
      this.traineeTableColumns = Object.keys(tableData[0]).filter(
        (key) => !excludeColumnArray.includes(key)
      );
      this.traineeTablesData = new MatTableDataSource(tableData);
      this.traineeTablesData.paginator = this.traineepaginator;
      this.traineeTablesData.sort = this.traineesort;


    }
  }
  getPBTETSPData() {
    this.ComSrv.postJSON("api/PBTE/GetPBTETSPs", this.filters).subscribe(
      (d: any) => {
        this.pbteTSPs = new MatTableDataSource(d[0]);
        this.pbteTSPsAfterReset = new MatTableDataSource(d[0]);
        this.pbteTSPsArray = d[0];
        this.pbteTSPsArrayToFilter = d[0];
        this.GroupTSPs(this.pbteTSPs);
        this.pbteTSPs.paginator = this.PageTSP;
        this.pbteTSPs.sort = this.SortTSP;
      },
      (error) => (this.error = error) // error path
    );
  }
  getPBTETraineeData() {
    this.ComSrv.postJSON("api/PBTE/GetPBTETrainees", this.filters).subscribe(
      (d: any) => {
        this.pbteTrainees = new MatTableDataSource(d[0]);
        this.pbteTraineesAfterReset = new MatTableDataSource(d[0]);
        this.pbteTraineesArray = d[0];
        this.pbteTrainees.paginator = this.PageTrainee;
        this.pbteTrainees.sort = this.SortTrainee;
      },
      (error) => (this.error = error) // error path
    );
  }
  getPBTETraineeExamScriptData() {
    this.ComSrv.postJSON(
      "api/PBTE/GetPBTETraineesExamScriptData",
      this.filters
    ).subscribe(
      (d: any) => {
        this.pbteTraineesExamScriptArray = d[0];
        this.GetSql("TraineesExamination");
      },
      (error) => (this.error = error) // error path
    );
  }
  getPBTEDropOutTraineeData() {
    this.ComSrv.postJSON(
      "api/PBTE/GetPBTEDropOutTrainees",
      this.filters
    ).subscribe(
      (d: any) => {
        this.pbteDropOutTrainees = new MatTableDataSource(d[0]);
        this.pbteDropOutTraineesAfterReset = new MatTableDataSource(d[0]);
        //this.pbteDropOutTraineesArray = d[0];
        this.pbteDropOutTrainees.paginator = this.PageDropOutTrainee;
        this.pbteDropOutTrainees.sort = this.SortDropOutTrainee;
      },
      (error) => (this.error = error) // error path
    );
  }
  getPBTETradeData() {
    this.ComSrv.postJSON("api/PBTE/GetPBTETrades", this.filters).subscribe(
      (d: any) => {
        this.pbteTrades = new MatTableDataSource(d[0]);
        this.pbteTradesAfterReset = new MatTableDataSource(d[0]);
        this.pbteTradesArray = d[0];
        this.pbteTrades.paginator = this.PageTrade;
        this.pbteTrades.sort = this.SortTrade;
      },
      (error) => (this.error = error) // error path
    );
  }
  getPBTEExamResultTraineeData() {
    this.ComSrv.postJSON(
      "api/PBTE/GetPBTETraineesExamResult",
      this.filters
    ).subscribe(
      (d: any) => {
        this.pbteExamResultTrainees = new MatTableDataSource(d[0]);
        this.pbteExamResultTraineesAfterReset = new MatTableDataSource(d[0]);
        this.pbteExamResultTraineesArray = d[0];
        this.traineeResultStatusTypes = d[1];
        this.pbteExamResultTrainees.paginator = this.PageExamResultTrainee;
        this.pbteExamResultTrainees.sort = this.SortExamResultTrainee;
      },
      (error) => (this.error = error) // error path
    );
  }
  getNAVTTCClassData() {
    this.ComSrv.postJSON(
      "api/PBTE/GetNAVTTCClasses",
      this.navttcfilters
    ).subscribe(
      (d: any) => {
        this.navttcClasses = new MatTableDataSource(d[0]);
        this.navttcClassesAfterReset = new MatTableDataSource(d[0]);
        this.navttcClassesArray = d[0];
        this.navttcClasses.paginator = this.PageNavttcClass;
        this.navttcClasses.sort = this.SortNavttcClass;
      },
      (error) => (this.error = error) // error path
    );
  }
  getNAVTTCTSPData() {
    this.ComSrv.postJSON(
      "api/PBTE/GetNAVTTCTSPs",
      this.navttcfilters
    ).subscribe(
      (d: any) => {
        this.navttcTSPs = new MatTableDataSource(d[0]);
        this.navttcTSPsAfterReset = new MatTableDataSource(d[0]);
        this.navttcTSPsArray = d[0];
        this.navttcTSPsArrayToFilter = d[0];
        this.GroupNavttcTSPs(this.navttcTSPs);
        this.navttcTSPs.paginator = this.PageNavttcTSP;
        this.navttcTSPs.sort = this.SortNavttcTSP;
      },
      (error) => (this.error = error) // error path
    );
  }
  getNAVTTCTraineeData() {
    this.ComSrv.postJSON(
      "api/PBTE/GetNAVTTCTrainees",
      this.navttcfilters
    ).subscribe(
      (d: any) => {
        this.navttcTrainees = new MatTableDataSource(d[0]);
        this.navttcTraineesAfterReset = new MatTableDataSource(d[0]);
        this.navttcTraineesArray = d[0];
        this.navttcTrainees.paginator = this.PageNavttcTrainee;
        this.navttcTrainees.sort = this.SortNavttcTrainee;
      },
      (error) => (this.error = error) // error path
    );
  }
  getNAVTTCTraineeSqlScriptData() {
    this.ComSrv.postJSON(
      "api/PBTE/GetNAVTTCTraineesSqlScriptData",
      this.navttcfilters
    ).subscribe(
      (d: any) => {
        //this.navttcTrainees = new MatTableDataSource(d[0]);
        //this.navttcTraineesAfterReset = new MatTableDataSource(d[0]);
        this.navttcTraineesSqlScriptArray = d[0];
        this.GetNAVTTCSql("Trainees");
        //this.navttcTrainees.paginator = this.PageNavttcTrainee;
        //this.navttcTrainees.sort = this.SortNavttcTrainee;
      },
      (error) => (this.error = error) // error path
    );
  }
  getNAVTTCTraineeRegisterSqlScriptData() {
    this.ComSrv.postJSON(
      "api/PBTE/GetNAVTTCTraineesRegisterSqlScriptData",
      this.navttcfilters
    ).subscribe(
      (d: any) => {
        //this.navttcTrainees = new MatTableDataSource(d[0]);
        //this.navttcTraineesAfterReset = new MatTableDataSource(d[0]);
        this.navttcTraineesRegisterSqlScriptArray = d[0];
        this.GetNAVTTCSql("TraineesRegisteration");
        //this.navttcTrainees.paginator = this.PageNavttcTrainee;
        //this.navttcTrainees.sort = this.SortNavttcTrainee;
      },
      (error) => (this.error = error) // error path
    );
  }
  getNAVTTCDropOutTraineeData() {
    this.ComSrv.postJSON(
      "api/PBTE/GetNAVTTCDropOutTrainees",
      this.navttcfilters
    ).subscribe(
      (d: any) => {
        this.navttcDropOutTrainees = new MatTableDataSource(d[0]);
        this.navttcDropOutTraineesAfterReset = new MatTableDataSource(d[0]);
        //this.pbteDropOutTraineesArray = d[0];
        this.navttcDropOutTrainees.paginator = this.PageNavttcDropOutTrainee;
        this.navttcDropOutTrainees.sort = this.SortNavttcDropOutTrainee;
      },
      (error) => (this.error = error) // error path
    );
  }
  getNAVTTCExamResultTraineeData() {
    this.ComSrv.postJSON(
      "api/PBTE/GetNAVTTCTraineesExamResult",
      this.navttcfilters
    ).subscribe(
      (d: any) => {
        this.navttcExamResultTrainees = new MatTableDataSource(d[0]);
        this.navttcExamResultTraineesAfterReset = new MatTableDataSource(d[0]);
        this.navttcExamResultTraineesArray = d[0];
        this.traineeResultStatusTypes = d[1];
        this.navttcExamResultTrainees.paginator =
          this.PageNAVTTCExamResultTrainee;
        this.navttcExamResultTrainees.sort = this.SortNAVTTCExamResultTrainee;
      },
      (error) => (this.error = error) // error path
    );
  }
  //FilteredData(moduleName: string) {
  //    if (moduleName == "PBTE") {
  //        if (this.filters.SchemeID != 0) {
  //            this.pbteClasses = new MatTableDataSource(this.pbteClassesArray.filter(cl => cl.SchemeID == this.filters.SchemeID));
  //            this.pbteTSPs = new MatTableDataSource(this.pbteTSPsArray.filter(cl => cl.SchemeID == this.filters.SchemeID));
  //            this.pbteTrainees = new MatTableDataSource(this.pbteTraineesArray.filter(cl => cl.SchemeID == this.filters.SchemeID));
  //            this.pbteExamResultTrainees = new MatTableDataSource(this.pbteExamResultTraineesArray.filter(cl => cl.SchemeID == this.filters.SchemeID));
  //            this.GroupTSPs(this.pbteTSPs);
  //        }
  //        if (this.filters.TSPID != 0) {
  //            this.pbteClasses = new MatTableDataSource(this.pbteClassesArray.filter(cl => cl.TSPID == this.filters.TSPID));
  //            this.pbteTSPs = new MatTableDataSource(this.pbteTSPsArray.filter(cl => cl.TSPID == this.filters.TSPID));
  //            this.pbteTrainees = new MatTableDataSource(this.pbteTraineesArray.filter(cl => cl.TSPID == this.filters.TSPID));
  //            this.pbteExamResultTrainees = new MatTableDataSource(this.pbteExamResultTraineesArray.filter(cl => cl.TSPID == this.filters.TSPID));
  //            this.GroupTSPs(this.pbteTSPs);
  //        }
  //        if (this.filters.ClassID != 0) {
  //            this.pbteClasses = new MatTableDataSource(this.pbteClassesArray.filter(cl => cl.ClassID == this.filters.ClassID));
  //            this.pbteTSPs = new MatTableDataSource(this.pbteTSPsArray.filter(cl => cl.ClassID == this.filters.ClassID));
  //            this.pbteTrainees = new MatTableDataSource(this.pbteTraineesArray.filter(cl => cl.ClassID == this.filters.ClassID));
  //            this.pbteExamResultTrainees = new MatTableDataSource(this.pbteExamResultTraineesArray.filter(cl => cl.ClassID == this.filters.ClassID));
  //        }
  //        if (this.filters.TradeID != 0) {
  //            this.pbteClasses = new MatTableDataSource(this.pbteClassesArray.filter(cl => cl.TradeID == this.filters.TradeID));
  //            this.pbteTSPs = new MatTableDataSource(this.pbteTSPsArray.filter(cl => cl.TradeID == this.filters.TradeID));
  //            this.pbteTrainees = new MatTableDataSource(this.pbteTraineesArray.filter(cl => cl.TradeID == this.filters.TradeID));
  //            this.pbteExamResultTrainees = new MatTableDataSource(this.pbteExamResultTraineesArray.filter(cl => cl.TradeID == this.filters.TradeID));
  //            this.pbteTrades = new MatTableDataSource(this.pbteTradesArray.filter(cl => cl.TradeID == this.filters.TradeID));
  //            this.GroupTSPs(this.pbteTSPs);
  //        }
  //        if (this.filters.DistrictID != 0) {
  //            this.pbteClasses = new MatTableDataSource(this.pbteClassesArray.filter(cl => cl.DistrictID == this.filters.DistrictID));
  //            this.pbteTSPs = new MatTableDataSource(this.pbteTSPsArray.filter(cl => cl.DistrictID == this.filters.DistrictID));
  //            this.pbteTrainees = new MatTableDataSource(this.pbteTraineesArray.filter(cl => cl.DistrictID == this.filters.DistrictID));
  //            this.pbteExamResultTrainees = new MatTableDataSource(this.pbteExamResultTraineesArray.filter(cl => cl.DistrictID == this.filters.DistrictID));
  //            this.GroupTSPs(this.pbteTSPs);
  //        }
  //        this.pbteStats = [
  //            { "classes": this.pbteClasses.data.length, "tsps": this.pbteTSPs.data.length, "trainees": this.pbteTrainees.data.length, "drpout": this.pbteDropOutTrainees.data.length },
  //        ];
  //        this.SortANDPaginate();
  //    }
  //    if (moduleName == "NAVTTC") {
  //        if (this.filters.SchemeID != 0) {
  //            this.navttcClasses = new MatTableDataSource(this.navttcClassesArray.filter(cl => cl.SchemeID == this.filters.SchemeID));
  //            this.navttcTSPs = new MatTableDataSource(this.navttcTSPsArray.filter(cl => cl.SchemeID == this.filters.SchemeID));
  //            this.navttcTrainees = new MatTableDataSource(this.navttcTraineesArray.filter(cl => cl.SchemeID == this.filters.SchemeID));
  //            this.navttcExamResultTrainees = new MatTableDataSource(this.navttcExamResultTraineesArray.filter(cl => cl.SchemeID == this.filters.SchemeID));
  //            this.GroupNavttcTSPs(this.navttcTSPs);
  //        }
  //        if (this.filters.TSPID != 0) {
  //            this.navttcClasses = new MatTableDataSource(this.navttcClassesArray.filter(cl => cl.TSPID == this.filters.TSPID));
  //            this.navttcTSPs = new MatTableDataSource(this.navttcTSPsArray.filter(cl => cl.TSPID == this.filters.TSPID));
  //            this.navttcTrainees = new MatTableDataSource(this.navttcTraineesArray.filter(cl => cl.TSPID == this.filters.TSPID));
  //            this.navttcExamResultTrainees = new MatTableDataSource(this.navttcExamResultTraineesArray.filter(cl => cl.TSPID == this.filters.TSPID));
  //            this.GroupNavttcTSPs(this.navttcTSPs);
  //        }
  //        if (this.filters.ClassID != 0) {
  //            this.navttcClasses = new MatTableDataSource(this.navttcClassesArray.filter(cl => cl.ClassID == this.filters.ClassID));
  //            this.navttcTSPs = new MatTableDataSource(this.navttcTSPsArray.filter(cl => cl.ClassID == this.filters.ClassID));
  //            this.navttcTrainees = new MatTableDataSource(this.navttcTraineesArray.filter(cl => cl.ClassID == this.filters.ClassID));
  //            this.navttcExamResultTrainees = new MatTableDataSource(this.navttcExamResultTraineesArray.filter(cl => cl.ClassID == this.filters.ClassID));
  //        }
  //        if (this.filters.TradeID != 0) {
  //            this.navttcClasses = new MatTableDataSource(this.navttcClassesArray.filter(cl => cl.TradeID == this.filters.TradeID));
  //            this.navttcTSPs = new MatTableDataSource(this.navttcTSPsArray.filter(cl => cl.TradeID == this.filters.TradeID));
  //            this.navttcTrainees = new MatTableDataSource(this.navttcTraineesArray.filter(cl => cl.TradeID == this.filters.TradeID));
  //            this.navttcExamResultTrainees = new MatTableDataSource(this.navttcExamResultTraineesArray.filter(cl => cl.TradeID == this.filters.TradeID));
  //            this.GroupNavttcTSPs(this.navttcTSPs);
  //        }
  //        if (this.filters.DistrictID != 0) {
  //            this.navttcClasses = new MatTableDataSource(this.navttcClassesArray.filter(cl => cl.DistrictID == this.filters.DistrictID));
  //            this.navttcTSPs = new MatTableDataSource(this.navttcTSPsArray.filter(cl => cl.DistrictID == this.filters.DistrictID));
  //            this.navttcTrainees = new MatTableDataSource(this.navttcTraineesArray.filter(cl => cl.DistrictID == this.filters.DistrictID));
  //            this.navttcExamResultTrainees = new MatTableDataSource(this.navttcExamResultTraineesArray.filter(cl => cl.DistrictID == this.filters.DistrictID));
  //            this.GroupNavttcTSPs(this.navttcTSPs);
  //        }
  //        this.navttcStats = [
  //            { "classes": this.navttcClasses.data.length, "tsps": this.navttcTSPs.data.length, "trainees": this.navttcTrainees.data.length, "drpout": this.navttcDropOutTrainees.data.length },
  //        ];
  //        this.SortANDPaginate();
  //    }
  //}
  ResetFilters() {
    this.filters.SchemeID = 0;
    this.filters.TSPID = 0;
    this.filters.ClassID = 0;
    this.filters.TradeID = 0;
    this.filters.DistrictID = 0;
    this.navttcfilters.SchemeID = 0;
    this.navttcfilters.TSPID = 0;
    this.navttcfilters.ClassID = 0;
    this.navttcfilters.TradeID = 0;
    this.navttcfilters.DistrictID = 0;
    //this.getPBTEStatsData();
    this.getSelectedTabData();
    //this.getPBTESelectedSubTabData();
    //this.navttcStats = this.navttcStatsAfterReset;
    //this.getNAVTTCSelectedSubTabData();
    //this.pbteClasses = this.pbteClassesAfterReset;
    //this.pbteTSPs = this.pbteTSPsAfterReset;
    //this.pbteTrainees = this.pbteTraineesAfterReset;
    //this.pbteExamResultTrainees = this.pbteExamResultTraineesAfterReset;
    //this.pbteDropOutTrainees = this.pbteDropOutTraineesAfterReset;
    //this.pbteTrades = this.pbteTradesAfterReset;
    //this.navttcClasses = this.navttcClassesAfterReset;
    //this.navttcTSPs = this.navttcTSPsAfterReset;
    //this.navttcTrainees = this.navttcTraineesAfterReset;
    //this.navttcExamResultTrainees = this.navttcExamResultTraineesAfterReset;
    //this.navttcDropOutTrainees = this.navttcDropOutTraineesAfterReset;
    //this.pbteStats = this.pbteStatsAfterReset;
    //this.navttcStats = this.navttcStatsAfterReset;
    //this.SortANDPaginate();
  }
  GetPBTEFiltersData() {
    this.ComSrv.getJSON("api/PBTE/GetPBTEFiltersData").subscribe(
      (d: any) => {
        this.Schemes = d[0];
        this.TSPDetail = d[1];
        this.classesArray = d[2];
        this.Trade = d[3];
        this.District = d[4];
      },
      (error) => (this.error = error) // error path
    );
  }
  //GetData() {
  //    this.ComSrv.getJSON('api/PBTE/GetPBTE').subscribe((d: any) => {
  //        this.pbteClasses = new MatTableDataSource(d[0]);
  //        this.pbteClassesAfterReset = new MatTableDataSource(d[0]);
  //        this.pbteClassesArray = d[0];
  //        this.pbteTSPs = new MatTableDataSource(d[1]);
  //        this.pbteTSPsArray = d[1];
  //        this.pbteTSPsArrayToFilter = d[1];
  //        this.pbteTrainees = new MatTableDataSource(d[2]);
  //        this.pbteTraineesAfterReset = new MatTableDataSource(d[2]);
  //        this.pbteTraineesArray = d[2];
  //        this.pbteDropOutTrainees = new MatTableDataSource(d[3]);
  //        this.pbteDropOutTraineesAfterReset = new MatTableDataSource(d[3]);
  //        this.dropoutTrainees = d[3];
  //        this.traineeResultStatusTypes = d[4];
  //        this.Schemes = d[5];
  //        this.navttcSchemes = d[5];
  //        this.TSPDetail = d[6];
  //        this.navttcTSPDetail = d[6];
  //        this.classesArray = d[7];
  //        this.navttcclassesArray = d[7];
  //        this.Trade = d[8];
  //        this.navttcTrade = d[8];
  //        this.District = d[9];
  //        this.navttcDistrict = d[9];
  //        this.navttcClasses = new MatTableDataSource(d[10]);
  //        this.navttcClassesAfterReset = new MatTableDataSource(d[10]);
  //        this.navttcClassesArray = d[10];
  //        this.navttcTSPs = new MatTableDataSource(d[11]);
  //        this.navttcTSPsAfterReset = new MatTableDataSource(d[11]);
  //        this.navttcTSPsArray = d[11];
  //        this.navttcTSPsArrayToFilter = d[11];
  //        this.navttcTrainees = new MatTableDataSource(d[12]);
  //        this.navttcTraineesAfterReset = new MatTableDataSource(d[12]);
  //        this.navttcTraineesArray = d[12];
  //        this.navttcDropOutTrainees = new MatTableDataSource(d[13]);
  //        this.navttcDropOutTraineesAfterReset = new MatTableDataSource(d[13]);
  //        this.navttcdropouts = d[13];
  //        this.pbteTrades = new MatTableDataSource(d[14]);
  //        this.pbteTradesAfterReset = new MatTableDataSource(d[14]);
  //        this.pbteTradesArray = d[14];
  //        this.pbteExamResultTrainees = new MatTableDataSource(d[15]);
  //        this.pbteExamResultTraineesAfterReset = new MatTableDataSource(d[15]);
  //        this.pbteExamResultTraineesArray = d[15];
  //        this.navttcExamResultTrainees = new MatTableDataSource(d[16]);
  //        this.navttcExamResultTraineesAfterReset = new MatTableDataSource(d[16]);
  //        this.navttcExamResultTraineesArray = d[16];
  //        //console.log(this.pbteTradesArray)
  //        //this.Schemes = this.Schemes.filter(x => this.pbteClassesArray.map(y => y.SchemeID).includes(x.SchemeID))
  //        //this.TSPDetail = this.TSPDetail.filter(x => this.pbteTSPsArray.map(y => y.TSPID).includes(x.TSPID))
  //        //this.classesArray = this.classesArray.filter(x => this.pbteClassesArray.map(y => y.ClassID).includes(x.ClassID))
  //        //this.Trade = this.Trade.filter(x => this.pbteClassesArray.map(y => y.TradeID).includes(x.TradeID))
  //        //this.District = this.District.filter(x => this.pbteClassesArray.map(y => y.DistrictID).includes(x.DistrictID))
  //        //this.navttcSchemes = this.navttcSchemes.filter(x => this.navttcClassesArray.map(y => y.SchemeID).includes(x.SchemeID))
  //        //this.navttcTSPDetail = this.navttcTSPDetail.filter(x => this.navttcTSPsArray.map(y => y.TSPID).includes(x.TSPID))
  //        //this.navttcclassesArray = this.navttcclassesArray.filter(x => this.navttcClassesArray.map(y => y.ClassID).includes(x.ClassID))
  //        //this.groupArr = new MatTableDataSource(this.pbteTSPsArray.reduce((r, { TSPID }) => {
  //        //    if (!r(o => o.TSPID == TSPID)) {
  //        //        r.push({ TSPID, groupItem: this.pbteTSPsArray.filter(v => v.TSPID == TSPID) });
  //        //    }
  //        //}));
  //        this.pbteTSPs = new MatTableDataSource(new GroupByPipe().transform(this.pbteTSPsArrayToFilter, 'TSPID'));
  //        //this.groupedTSPsArray = (new GroupByPipe().transform(this.pbteTSPsArray, 'TSPID'));
  //        this.pbteTSPs.filteredData.forEach(key => {
  //            let obj = [];
  //            this.groupedTSPsArray = this.pbteTSPsArrayToFilter.filter(t => t.TSPID === Number(key.key));
  //            //this.groupedTSPsArray.map(x=> x.ClassID = this.groupedTSPsArray.map(t => t.ClassID));
  //            this.groupedTSPsArrayForData.push(this.groupedTSPsArray[0]);
  //        });
  //        this.pbteTSPs = new MatTableDataSource(this.groupedTSPsArrayForData)
  //        this.pbteTSPsAfterReset = new MatTableDataSource(this.groupedTSPsArrayForData);
  //        console.log(this.groupedTSPsArrayForData);
  //        this.navttcTSPs = new MatTableDataSource(new GroupByPipe().transform(this.navttcTSPsArrayToFilter, 'TSPID'));
  //        //this.groupedTSPsArray = (new GroupByPipe().transform(this.pbteTSPsArray, 'TSPID'));
  //        this.navttcTSPs.filteredData.forEach(key => {
  //            let obj = [];
  //            this.groupednavttcTSPsArray = this.navttcTSPsArrayToFilter.filter(t => t.TSPID === Number(key.key));
  //            //this.groupedTSPsArray.map(x=> x.ClassID = this.groupedTSPsArray.map(t => t.ClassID));
  //            this.groupednavttcTSPsArrayForData.push(this.groupednavttcTSPsArray[0]);
  //        });
  //        this.navttcTSPs = new MatTableDataSource(this.groupednavttcTSPsArrayForData)
  //        this.navttcTSPsAfterReset = new MatTableDataSource(this.groupednavttcTSPsArrayForData);
  //        console.log(this.groupednavttcTSPsArrayForData);
  //        this.pbteStats = [
  //            { "classes": this.pbteClasses.data.length, "tsps": this.pbteTSPs.data.length, "trainees": this.pbteTrainees.data.length, "drpout": this.pbteDropOutTrainees.data.length },
  //        ];
  //        this.pbteStatsAfterReset = [
  //            { "classes": this.pbteClasses.data.length, "tsps": this.pbteTSPs.data.length, "trainees": this.pbteTrainees.data.length, "drpout": this.pbteDropOutTrainees.data.length },
  //        ];
  //        this.navttcStats = [
  //            { "classes": this.navttcClasses.data.length, "tsps": this.navttcTSPs.data.length, "trainees": this.navttcTrainees.data.length, "drpout": this.navttcDropOutTrainees.data.length },
  //        ];
  //        this.navttcStatsAfterReset = [
  //            { "classes": this.navttcClasses.data.length, "tsps": this.navttcTSPs.data.length, "trainees": this.navttcTrainees.data.length, "drpout": this.navttcDropOutTrainees.data.length },
  //        ];
  //        this.SortANDPaginate();
  //    }, error => this.error = error// error path
  //    );
  //};
  GetSql(tableName: string) {
    let data = [];
    let textarray = [];
    switch (tableName) {
      case (tableName = "DropOutTrainees"): {
        this.pbteDropOutTrainees.filteredData.forEach((item) => {
          let obj = {};
          this.displayedColumnsDropOutTrainees.forEach((key) => {
            obj[key] = item[key] || item;
            console.log(item.Batch);
          });
          var text = `update dbo.Student set ClassCode='${item.ClassCode
            }',Batch=${item.Batch},TraineeName='${item.TraineeName
            }',FatherName='${item.FatherName}',TraineeCNIC='${item.TraineeCNIC
            }',DateOfBirth='${this._date.transform(
              item.DateOfBirth,
              "dd/MM/yyyy"
            )}',Education='${item.Education}',StudentID=${item.PBTEID
            },CollegeID=${item.CollegeID},TraineeStatus='${item.StatusName
            }' where TraineeID=${item.TraineeID},`;
          //textarray = textarray + text;
          textarray.push(text);
        });
        //console.log(textarrady.join(" "));
        var blob = new Blob([textarray.join("\r\n")], {
          type: "text/plain;charset=utf-8, endings: 'native'",
        });
        fileSaver.saveAs(blob, "PBTE-DropOut-Trainee-Sql-Script.sql");
        break;
      }
      case (tableName = "Trainees"): {
        this.pbteTrainees.filteredData.forEach((item) => {
          let obj = {};
          this.displayedColumnsTrainees.forEach((key) => {
            obj[key] = item[key] || item;
            console.log(item.Batch);
          });
          var text = `insert into dbo.Student (ExamId,StudentName,FathersName,DateofBirth,Qualification,Gender,active,Class_Code,NIC,Trainee_ID,Shift,Shift_Time_From,Shift_Time_To) values(${item.ExamID
            },'${item.TraineeName}','${item.FatherName}','${this._date.transform(
              item.DateOfBirth,
              "dd/MM/yyyy"
            )}','${item.Education}',${item.GenderID},${1},'${item.TraineeCNIC
            }','${item.TraineeCode}','${item.TraineeShift
            }','${this._date.transform(
              item.ShiftFrom,
              "hh:mm a"
            )}','${this._date.transform(item.ShiftTo, "h:mm a")}');`;
          //textarray = textarray + text;
          textarray.push(text);
        });
        //console.log(textarrady.join(" "));
        var blob = new Blob([textarray.join("\r\n")], {
          type: "text/plain;charset=utf-8, endings: 'native'",
        });
        fileSaver.saveAs(blob, "PBTE-Trainee-Sql-Script.sql");
        break;
      }
      case (tableName = "TraineesExamination"): {
        this.pbteTraineesExamScriptArray.forEach((item) => {
          //let obj = {};
          //this.displayedColumnsTrainees.forEach(key => {
          //  obj[key] = item[key] || item;
          //  console.log(item.Batch);
          //});
          var text = `insert into dbo.[Examination] (ExamID,ExamSessionUrdu,ExamSessionenglish,maincategoryid,BatchNo,ExamYear,StartDate,EndDate,Description,Active,ExamSession) values(${item.ExamID},'${item.ExamSessionUrdu}','${item.ExamSessionenglish}',${item.maincategoryid},${item.Batch},${item.ExamYear},'${item.StartDate}','${item.EndDate}',${item.Active},'${item.ExamSession}');`;
          //textarray = textarray + text;
          textarray.push(text);
        });
        //console.log(textarrady.join(" "));
        var blob = new Blob([textarray.join("\r\n")], {
          type: "text/plain;charset=utf-8, endings: 'native'",
        });
        fileSaver.saveAs(blob, "PBTE-Trainee-Sql-Script.sql");
        break;
      }
      case (tableName = "TSPs"): {
        this.pbteTSPs.filteredData.forEach((item) => {
          let obj = {};
          this.displayedColumnsTsps.forEach((key) => {
            obj[key] = item[key] || item;
            console.log(item.Batch);
          });
          var text = `insert into table1 (TSPName,TSPCode,Address,HeadName,HeadDesignation,HeadEmail,HeadLandline,OrgLandline,Website,CPName,CPDesignation,CPEmail,CPLandline) values('${item.TSPName}','${item.TSPCode}','${item.Address}','${item.HeadName}','${item.HeadDesignation}','${item.HeadEmail}','${item.HeadLandline}','${item.OrgLandline}','${item.Website}','${item.CPName}','${item.CPDesignation}','${item.CPEmail}','${item.CPLandline}');`;
          //textarray = textarray + text;
          textarray.push(text);
        });
        //console.log(textarrady.join(" "));
        var blob = new Blob([textarray.join("\r\n")], {
          type: "text/plain;charset=utf-8, endings: 'native'",
        });
        fileSaver.saveAs(blob, "PBTE-TSPs-Sql-Script.sql");
        break;
      }
      case (tableName = "Classes"): {
        this.pbteClasses.filteredData.forEach((item) => {
          let obj = {};
          this.displayedColumnsClasses.forEach((key) => {
            obj[key] = item[key] || item;
            console.log(item.Batch);
          });
          var text = `insert into table1 (SchemeName,Batch,TSPName,ClassCode,TradeName,TrainingAddressLocation,TehsilName,DistrictName,CertAuthName,TraineesPerClass,GenderName,Duration,StartDate,EndDate,ClassStatusName) values('${item.SchemeName
            }',${item.Batch},'${item.TSPName}','${item.ClassCode}','${item.TradeName
            }','${item.TrainingAddressLocation}','${item.TehsilName}','${item.TehsilName
            }','${item.DistrictName}','${item.CertAuthName}',${item.TraineesPerClass
            },'${item.GenderName}',${item.Duration},${this._date.transform(
              item.StartDate,
              "dd/MM/yyyy"
            )}','${this._date.transform(item.EndDate, "dd/MM/yyyy")}','${item.ClassStatusName
            }');`;
          //textarray = textarray + text;
          textarray.push(text);
        });
        //console.log(textarrady.join(" "));
        var blob = new Blob([textarray.join("\r\n")], {
          type: "text/plain;charset=utf-8, endings: 'native'",
        });
        fileSaver.saveAs(blob, "PBTE-Classes-Sql-Script.sql");
        break;
      }
      default: {
      }
    }
  }
  GetNAVTTCSql(tableName: string) {
    let data = [];
    let textarray = [];
    switch (tableName) {
      case (tableName = "DropOutTrainees"): {
        this.navttcDropOutTrainees.filteredData.forEach((item) => {
          let obj = {};
          this.displayedColumnsDropOutTrainees.forEach((key) => {
            obj[key] = item[key] || item;
            console.log(item.Batch);
          });
          var text = `update dbo.Student set ClassCode='${item.ClassCode
            }',Batch=${item.Batch},TraineeName='${item.TraineeName
            }',FatherName='${item.FatherName}',TraineeCNIC='${item.TraineeCNIC
            }',DateOfBirth='${this._date.transform(
              item.DateOfBirth,
              "dd/MM/yyyy"
            )}',Education='${item.Education}',StudentID=${item.PBTEID
            },CollegeID=${item.CollegeID},TraineeStatus='${item.StatusName
            }' where TraineeID=${item.TraineeID},`;
          //textarray = textarray + text;
          textarray.push(text);
        });
        //console.log(textarrady.join(" "));
        var blob = new Blob([textarray.join("\r\n")], {
          type: "text/plain;charset=utf-8, endings: 'native'",
        });
        fileSaver.saveAs(blob, "NAVTTC-DropOut-Trainee-Sql-Script.sql");
        break;
      }
      case (tableName = "Trainees"): {
        var traineeid = 1;
        this.navttcTraineesSqlScriptArray.forEach((item) => {
          //console.log(item[key]);
          //console.log(item);
          //this.displayedColumnsTrainees.forEach(key => {
          //  obj[key] = item[key] || item;
          //  //console.log(item[key]);
          //});
          var text = `insert into tbl_trainee (traineeid,trainee_name,father_name,gender,disableId,dob,cnic,basic_qualification,pathWayId,mobile,email,address,districtId,instituteId,psdf_traineeId,psdf_ClassCode) values(${traineeid},'${item.TraineeName
            }','${item.FatherName}','${item.GenderID}','${item.disableId
            }','${this._date.transform(item.DOB, "dd/MM/yyyy")}','${item.TraineeCNIC
            }','${item.Education}','${item.pathWayId}','${item.mobile}','${item.email
            }','${item.address}','${item.instituteId}','${item.psdf_traineeId
            }','${item.psdf_ClassCode}');`;
          //textarray = textarray + text;
          traineeid++;
          textarray.push(text);
        });
        //console.log(textarrady.join(" "));
        var blob = new Blob([textarray.join("\r\n")], {
          type: "text/plain;charset=utf-8, endings: 'native'",
        });
        fileSaver.saveAs(blob, "NAVTTC-Trainee-Sql-Script.sql");
        break;
      }
      case (tableName = "TraineesRegisteration"): {
        var traineeid = 1;
        this.navttcTraineesRegisterSqlScriptArray.forEach((item) => {
          let obj = {};
          //console.log(item[key]);
          //console.log(item);
          //this.displayedColumnsTrainees.forEach(key => {
          //  obj[key] = item[key] || item;
          //  //console.log(item[key]);
          //});
          var text = `insert into tbl_traineeregistration (traineeid,instituteId,qabId,qualificationId,termId,sessionId,shiftId,psdf_ClassCode) values('${traineeid}','${item.instituteId}','${item.qabId}','${item.Education}',${item.termId},'${item.sessionId}','${item.shiftId}','${item.psdf_ClassCode}');`;
          //textarray = textarray + text;
          traineeid++;
          textarray.push(text);
        });
        //console.log(textarrady.join(" "));
        var blob = new Blob([textarray.join("\r\n")], {
          type: "text/plain;charset=utf-8, endings: 'native'",
        });
        fileSaver.saveAs(blob, "NAVTTC-Trainee-Registration-Sql-Script.sql");
        break;
      }
      case (tableName = "TSPs"): {
        this.navttcTSPs.filteredData.forEach((item) => {
          let obj = {};
          this.displayedColumnsTsps.forEach((key) => {
            obj[key] = item[key] || item;
            console.log(item.Batch);
          });
          var text = `insert into table1 (TSPName,TSPCode,Address,HeadName,HeadDesignation,HeadEmail,HeadLandline,OrgLandline,Website,CPName,CPDesignation,CPEmail,CPLandline) values('${item.TSPName}','${item.TSPCode}','${item.Address}','${item.HeadName}','${item.HeadDesignation}','${item.HeadEmail}','${item.HeadLandline}','${item.OrgLandline}','${item.Website}','${item.CPName}','${item.CPDesignation}','${item.CPEmail}','${item.CPLandline}');`;
          //textarray = textarray + text;
          textarray.push(text);
        });
        //console.log(textarrady.join(" "));
        var blob = new Blob([textarray.join("\r\n")], {
          type: "text/plain;charset=utf-8, endings: 'native'",
        });
        fileSaver.saveAs(blob, "NAVTTC-TSPs-Sql-Script.sql");
        break;
      }
      case (tableName = "Classes"): {
        this.navttcClasses.filteredData.forEach((item) => {
          let obj = {};
          this.displayedColumnsClasses.forEach((key) => {
            obj[key] = item[key] || item;
            console.log(item.Batch);
          });
          var text = `insert into table1 (SchemeName,Batch,TSPName,ClassCode,TradeName,TrainingAddressLocation,TehsilName,DistrictName,CertAuthName,TraineesPerClass,GenderName,Duration,StartDate,EndDate,ClassStatusName) values('${item.SchemeName
            }',${item.Batch},'${item.TSPName}','${item.ClassCode}','${item.TradeName
            }','${item.TrainingAddressLocation}','${item.TehsilName}','${item.TehsilName
            }','${item.DistrictName}','${item.CertAuthName}',${item.TraineesPerClass
            },'${item.GenderName}',${item.Duration},${this._date.transform(
              item.StartDate,
              "dd/MM/yyyy"
            )}','${this._date.transform(item.EndDate, "dd/MM/yyyy")}','${item.ClassStatusName
            }');`;
          //textarray = textarray + text;
          textarray.push(text);
        });
        //console.log(textarrady.join(" "));
        var blob = new Blob([textarray.join("\r\n")], {
          type: "text/plain;charset=utf-8, endings: 'native'",
        });
        fileSaver.saveAs(blob, "NAVTTC-Classes-Sql-Script.sql");
        break;
      }
      default: {
      }
    }
  }
  onTSRFileChange(ev: any) {
    const file = ev.target.files[0];
    if (!file) {
      this.ComSrv.ShowError("Please upload an Excel file.");
      return;
    }
    const reader = new FileReader();
    reader.onload = (event) => {
      let workBook: XLSX.WorkBook;
      let jsonData: any;
      try {
        const data = reader.result as string;
        workBook = XLSX.read(data, { type: "binary" });
        jsonData = workBook.SheetNames.reduce((initial, name) => {
          const sheet = workBook.Sheets[name];
          initial[name] = XLSX.utils.sheet_to_json(sheet);
          return initial;
        }, {});
        const dataString = JSON.parse(JSON.stringify(jsonData));
        const _exampData =
          dataString[
          Object.keys(workBook.Sheets).find(
            (x) =>
              x.toLowerCase() === PBTESheetNames.ExaminationData.toLowerCase()
          )
          ];
        const _traineeData =
          dataString[
          Object.keys(workBook.Sheets).find(
            (x) =>
              x.toLowerCase() === PBTESheetNames.TraineeData.toLowerCase()
          )
          ];
        if (!_exampData || _exampData.length === 0) {
          this.ComSrv.ShowError(
            "Sheet with the name '" +
            PBTESheetNames.ExaminationData +
            "' not found in Excel file."
          );
        } else {
          console.log(_exampData);
          // this.populateFieldsFromFile(_exampData[0]);
        }
        if (!_traineeData || _traineeData.length === 0) {
          this.ComSrv.ShowError(
            "Sheet with the name '" +
            PBTESheetNames.TraineeData +
            "' not found in Excel file."
          );
        } else {
          console.log(_traineeData);
          // Handle trainee data here
        }
        this._examDataArray = _exampData;
        this._tsrDataArray = _traineeData;
        this.LoadMatTable(_exampData, "ExamData");
        this.LoadMatTable(_traineeData, "TraineeData");
      } catch (error) {
        console.error("Error processing file:", error);
        this.ComSrv.ShowError("An error occurred while processing the file.");
      }
    };
    reader.readAsBinaryString(file);
    ev.target.value = "";
  }
  saveExamData() {
    const Error = this.examTablesData.filteredData.find((element) =>
      element.SchemeForPBTE.includes("required")
    );

    if (this.examTablesData.filteredData.length == 0) {
      this.ComSrv.ShowError("no data found");
      return;
    }
    if (Error) {
      this.ComSrv.ShowError(Error.SchemeForPBTE);
      return;
    }

    // const error = this.traineeTablesData.filteredData.find(element =>
    //   element.SchemeForPBTE.includes("required") ||
    //   element.PBTETrainingAddress.includes("required")
    // );

    // if (error) {
    //   const errorMessage = error.SchemeForPBTE.includes("required") ? "Scheme mapping required for " + error.Scheme : "Training location mapping required for " + error.TrainingLocation;

    //   this.ComSrv.ShowError(errorMessage);
    //   return;
    // }

    if (this.examTablesData) {
      this.ComSrv.postJSON(
        "api/PBTE/SavePBTEExam",
        this.examTablesData.filteredData
      ).subscribe(
        (d: any) => {
          if (d) {
            this.ComSrv.openSnackBar("PBTE Exam data saved successfully");
          } else {
            this.ComSrv.ShowError("Error saving PBTE Exam data");
          }
        },
        (error) => (this.error = error) // error path
      );
    }
  }

  saveTraineeData() {
    const data = this.traineeTablesData.filteredData.find(
      (e) =>
        e.ExamCode == 0 ||
        e.CollegeCode == 0 ||
        e.CourseCode == 0 ||
        e.MainCategoryCode == 0 ||
        e.CourseCategoryCode == 0
    );

    if (this.TraineeUrduData.length > 0) {
      this.ComSrv.ShowError(
        "conversion required for urdu trainee name into english for " +
        this.TraineeUrduData.length +
        " Trainee"
      );
      return;
    }

    if (data) {
      if (data.MainCategoryCode == 0) {
        this.ComSrv.ShowError("Scheme mapping required for " + data.Scheme);
        return;
      }
      if (data.ExamCode == 0) {
        this.ComSrv.ShowError("please save examination data");
        return;
      }
      if (data.CourseCode == 0) {
        this.ComSrv.ShowError("Trade Mapping required for " + data.Trade);
        return;
      }

      if (data.CollegeCode == 0) {
        this.ComSrv.ShowError(
          "Center Mapping required for " + data.TrainingLocation
        );
        return;
      }

      if (data.CourseCategoryCode == 0) {
        this.ComSrv.ShowError(
          "Course and MainCatogory are not found in PBTE Database" +
          data.SchemeForPBTE +
          " || Course: " +
          data.Trade
        );
        return;
      }
    }

    const error = this.traineeTablesData.filteredData.find(
      (element) =>
        element.SchemeForPBTE.includes("required") ||
        element.PBTETrainingAddress.includes("required")
    );
    if (error) {
      const errorMessage = error.SchemeForPBTE.includes("required")
        ? "Scheme mapping required for " + error.Scheme
        : "Training location mapping required for " + error.TrainingLocation;

      this.ComSrv.ShowError(errorMessage);
      return;
    }

    if (this.traineeTablesData) {
      this.ComSrv.postJSON(
        "api/PBTE/SavePBTEStudent",
        this.traineeTablesData.filteredData
      ).subscribe(
        (d: any) => {
          if (d) {
            this.ComSrv.openSnackBar("PBTE Exam data saved successfully");
          } else {
            this.ComSrv.ShowError("Error saving PBTE Exam data");
          }
        },
        (error) => {
          this.ComSrv.ShowError(error.message);
        }
      );
    } else {
      this.ComSrv.ShowError("No data found");
    }
  }

  traineeData: any = [];

  importTraineeData(ev: any) {
    debugger;
    this.traineeData = [];
    let workBook = null;
    debugger;
    let jsonData = null;
    const reader = new FileReader();
    const file = ev.target.files[0];
    reader.onload = (event) => {
      const data = reader.result;
      workBook = XLSX.read(data, { type: "binary" });
      jsonData = workBook.SheetNames.reduce((initial, name) => {
        const sheet = workBook.Sheets[name];
        initial[name] = XLSX.utils.sheet_to_json(sheet);
        return initial;
      }, {});
      debugger;
      const dataString = JSON.stringify(jsonData);
      this.traineeData = JSON.parse(dataString);
      // console.log(this.traineeData)
      // console.log(this.)
      this.urduToEnglishConversion(this.traineeData.Sheet1);
    };
    reader.readAsBinaryString(file);
    ev.target.value = "";
  }

  urduToEnglishConversion(uploadedData: any) {
    const _totalTrainee = this.traineeTablesData.filteredData;
    const _traineeUrduData = this.TraineeUrduData;
    const _uploadTraineeData = uploadedData;

    if (_traineeUrduData.length != _uploadTraineeData.length) {
      this.ComSrv.ShowError(
        "between Urdu trainee data count and imported data count must be same"
      );
      return;
    }
    debugger

    const _traineeData = _totalTrainee.map((r) => ({
      ...r,
      "TraineeName": this.containsUrdu(r.TraineeName) == false ? r.TraineeName : _uploadTraineeData.find(x => x.TraineeCode == r.TraineeCode)?.TraineeName || r.TraineeName,
      "FatherName": this.containsUrdu(r.FatherName) == false ? r.FatherName : _uploadTraineeData.find(x => x.TraineeCode == r.TraineeCode)?.FatherName || r.FatherName,
    }));
    this.LoadMatTable(_traineeData, 'TraineeData')


  }

  onFileChange(ev: any) {
    let workBook = null;
    let jsonData = null;
    const reader = new FileReader();
    const file = ev.target.files[0];
    reader.onload = (event) => {
      const data = reader.result;
      workBook = XLSX.read(data, { type: "binary" });
      jsonData = workBook.SheetNames.reduce((initial, name) => {
        const sheet = workBook.Sheets[name];
        initial[name] = XLSX.utils.sheet_to_json(sheet);
        return initial;
      }, {});
      const dataString = JSON.stringify(jsonData);
      this.data = JSON.parse(dataString);
      console.log(this.pbteClasses.filteredData);
      console.log(this.data.Class_PBTE);
      if (!this.data.Class_PBTE) {
        this.ComSrv.ShowError(
          "Sheet with the name 'Class_PBTE' not found in Excel file"
        );
        return false;
      }
      this.selectedClasses = this.pbteClasses.filteredData;
      this.selectedClasses = this.data.Class_PBTE.filter((x) =>
        this.pbteClasses.filteredData
          .map((y) => y.ClassCode)
          .includes(x.ClassCode)
      );
      console.log(this.selectedClasses);
      if (this.selectedClasses.length == 0) {
        this.error = "No matched class to update";
        this.ComSrv.ShowError(this.error.toString(), "Error");
        return false;
      }
      this.ComSrv.postJSON(
        "api/PBTE/UpdatePBTEClasses",
        this.selectedClasses
      ).subscribe(
        (d: any) => {
          this.update = "PBTE imported for Classes";
          this.ComSrv.openSnackBar(this.update.toString(), "Updated"); // this.inceptionreport =new MatTableDataSource(d);
        },
        (error) => (this.error = error) // error path
      );
    };
    reader.readAsBinaryString(file);
    ev.target.value = "";
  }
  onNAVTTCFileChange(ev: any) {
    let workBook = null;
    let jsonData = null;
    const reader = new FileReader();
    const file = ev.target.files[0];
    reader.onload = (event) => {
      const data = reader.result;
      workBook = XLSX.read(data, { type: "binary" });
      jsonData = workBook.SheetNames.reduce((initial, name) => {
        const sheet = workBook.Sheets[name];
        initial[name] = XLSX.utils.sheet_to_json(sheet);
        return initial;
      }, {});
      const dataString = JSON.stringify(jsonData);
      this.data = JSON.parse(dataString);
      console.log(this.navttcClasses.filteredData);
      console.log(this.data.Class_NAVTTC);
      if (!this.data.Class_NAVTTC) {
        this.ComSrv.ShowError(
          "Sheet with the name 'Class_NAVTTC' not found in Excel file"
        );
        return false;
      }
      this.selectedClasses = this.navttcClasses.filteredData;
      this.selectedClasses = this.data.Class_NAVTTC.filter((x) =>
        this.navttcClasses.filteredData
          .map((y) => y.ClassCode)
          .includes(x.ClassCode)
      );
      console.log(this.selectedClasses);
      if (this.selectedClasses.length == 0) {
        this.error = "No matched class to update";
        this.ComSrv.ShowError(this.error.toString(), "Error");
        return false;
      }
      this.ComSrv.postJSON(
        "api/PBTE/UpdateNAVTTCClasses",
        this.selectedClasses
      ).subscribe(
        (d: any) => {
          this.update = "CollegeID and TradeID updated for matched Classes";
          this.ComSrv.openSnackBar(this.update.toString(), "Updated"); // this.inceptionreport =new MatTableDataSource(d);
        },
        (error) => (this.error = error) // error path
      );
    };
    reader.readAsBinaryString(file);
    ev.target.value = "";
  }
  onTraineeFileChange(ev: any) {
    let workBook = null;
    let jsonData = null;
    const reader = new FileReader();
    const file = ev.target.files[0];
    reader.onload = (event) => {
      const data = reader.result;
      workBook = XLSX.read(data, { type: "binary" });
      jsonData = workBook.SheetNames.reduce((initial, name) => {
        const sheet = workBook.Sheets[name];
        initial[name] = XLSX.utils.sheet_to_json(sheet);
        return initial;
      }, {});
      const dataString = JSON.stringify(jsonData);
      this.data = JSON.parse(dataString);
      console.log(this.pbteTrainees.filteredData);
      console.log(this.data.Trainee_PBTE);
      if (!this.data.Trainee_PBTE) {
        this.ComSrv.ShowError(
          "Sheet with the name 'Trainee_PBTE' not found in Excel file"
        );
        return false;
      }
      this.selectedTrainees = this.pbteTrainees.filteredData;
      this.selectedTrainees = this.data.Trainee_PBTE.filter((x) =>
        this.pbteTrainees.filteredData
          .map((y) => y.TraineeCode)
          .includes(x.TraineeCode)
      );
      console.log(this.selectedTrainees);
      if (this.selectedTrainees.length == 0) {
        this.error = "No matched trainee to update result";
        this.ComSrv.ShowError(this.error.toString(), "Error");
        return false;
      }
      this.ComSrv.postJSON(
        "api/PBTE/UpdatePBTETrainees",
        this.selectedTrainees
      ).subscribe(
        (d: any) => {
          this.update = "PBTE imported for Trainees";
          this.ComSrv.openSnackBar(this.update.toString(), "Updated");
        },
        (error) => (this.error = error) // error path
      );
    };
    reader.readAsBinaryString(file);
    ev.target.value = "";
  }
  onNAVTTCTraineeFileChange(ev: any) {
    let workBook = null;
    let jsonData = null;
    const reader = new FileReader();
    const file = ev.target.files[0];
    reader.onload = (event) => {
      const data = reader.result;
      workBook = XLSX.read(data, { type: "binary" });
      jsonData = workBook.SheetNames.reduce((initial, name) => {
        const sheet = workBook.Sheets[name];
        initial[name] = XLSX.utils.sheet_to_json(sheet);
        return initial;
      }, {});
      const dataString = JSON.stringify(jsonData);
      this.data = JSON.parse(dataString);
      console.log(this.navttcTrainees.filteredData);
      console.log(this.data.Trainee_NAVTTC);
      if (!this.data.Trainee_NAVTTC) {
        this.ComSrv.ShowError(
          "Sheet with the name 'Trainee_NAVTTC' not found in Excel file"
        );
        return false;
      }
      this.selectedTrainees = this.navttcTrainees.filteredData;
      this.selectedTrainees = this.data.Trainee_NAVTTC.filter((x) =>
        this.navttcTrainees.filteredData
          .map((y) => y.TraineeCode)
          .includes(x.TraineeCode)
      );
      console.log(this.selectedTrainees);
      if (this.selectedTrainees.length == 0) {
        this.error = "No matched trainee to update result";
        this.ComSrv.ShowError(this.error.toString(), "Error");
        return false;
      }
      this.ComSrv.postJSON(
        "api/PBTE/UpdatePBTETrainees",
        this.selectedTrainees
      ).subscribe(
        (d: any) => {
          this.update = "NAVTTC imported for Trainees";
          this.ComSrv.openSnackBar(this.update.toString(), "Updated");
        },
        (error) => (this.error = error) // error path
      );
    };
    reader.readAsBinaryString(file);
    ev.target.value = "";
  }
  onTraineeResultFileChange(ev: any) {
    let workBook = null;
    let jsonData = null;
    const reader = new FileReader();
    const file = ev.target.files[0];
    reader.onload = (event) => {
      const data = reader.result;
      workBook = XLSX.read(data, { type: "binary" });
      jsonData = workBook.SheetNames.reduce((initial, name) => {
        const sheet = workBook.Sheets[name];
        initial[name] = XLSX.utils.sheet_to_json(sheet);
        return initial;
      }, {});
      const dataString = JSON.stringify(jsonData);
      this.data = JSON.parse(dataString);
      console.log(this.traineeResultStatusTypes);
      console.log(this.pbteTrainees.filteredData);
      console.log(this.data.PBTE_Results);
      if (!this.data.PBTE_Results) {
        this.ComSrv.ShowError(
          "Sheet with the name 'PBTE_Results' not found in Excel file"
        );
        return false;
      }
      this.selectedTrainees = this.pbteExamResultTrainees.filteredData;
      this.selectedTrainees = this.data.PBTE_Results.filter((x) =>
        this.pbteExamResultTrainees.filteredData
          .map((y) => y.TraineeCode)
          .includes(x.TraineeCode)
      );
      console.log(this.selectedTrainees);
      if (this.selectedTrainees.length == 0) {
        this.error = "No matched trainee to update result";
        this.ComSrv.ShowError(this.error.toString(), "Error");
        return false;
      }
      this.selectedTrainees = this.selectedTrainees.filter((x) =>
        this.traineeResultStatusTypes
          .map((y) => y.ResultStatusName)
          .includes(x.ResultStatusName)
      );
      console.log(this.selectedTrainees);
      if (this.selectedTrainees.length == 0) {
        this.error = "No matched trainee to update result";
        this.ComSrv.ShowError(this.error.toString(), "Error");
      }
      this.ComSrv.postJSON(
        "api/PBTE/UpdatePBTETraineesResult",
        this.selectedTrainees
      ).subscribe(
        (d: any) => {
          this.update = "Result Status imported for Trainees";
          this.ComSrv.openSnackBar(this.update.toString(), "Updated");
        },
        (error) => (this.error = error) // error path
      );
    };
    reader.readAsBinaryString(file);
    ev.target.value = "";
  }
  onNAVTTCTraineeResultFileChange(ev: any) {
    let workBook = null;
    let jsonData = null;
    const reader = new FileReader();
    const file = ev.target.files[0];
    reader.onload = (event) => {
      const data = reader.result;
      workBook = XLSX.read(data, { type: "binary" });
      jsonData = workBook.SheetNames.reduce((initial, name) => {
        const sheet = workBook.Sheets[name];
        initial[name] = XLSX.utils.sheet_to_json(sheet);
        return initial;
      }, {});
      const dataString = JSON.stringify(jsonData);
      this.data = JSON.parse(dataString);
      console.log(this.traineeResultStatusTypes);
      console.log(this.navttcExamResultTrainees.filteredData);
      console.log(this.data.NAVTTC_Results);
      if (!this.data.NAVTTC_Results) {
        this.ComSrv.ShowError(
          "Sheet with the name 'NAVTTC_Results' not found in Excel file"
        );
        return false;
      }
      this.selectedTrainees = this.pbteTrainees.filteredData;
      this.selectedTrainees = this.data.NAVTTC_Results.filter((x) =>
        this.navttcExamResultTrainees.filteredData
          .map((y) => y.TraineeCode)
          .includes(x.TraineeCode)
      );
      console.log(this.selectedTrainees);
      if (this.selectedTrainees.length == 0) {
        this.error = "No matched trainee to update result";
        this.ComSrv.ShowError(this.error.toString(), "Error");
        return false;
      }
      this.selectedTrainees = this.selectedTrainees.filter((x) =>
        this.traineeResultStatusTypes
          .map((y) => y.ResultStatusName)
          .includes(x.ResultStatusName)
      );
      console.log(this.selectedTrainees);
      this.ComSrv.postJSON(
        "api/PBTE/UpdatePBTETraineesResult",
        this.selectedTrainees
      ).subscribe(
        (d: any) => {
          this.update = "Result Status imported for Trainees";
          this.ComSrv.openSnackBar(this.update.toString(), "Updated");
        },
        (error) => (this.error = error) // error path
      );
    };
    reader.readAsBinaryString(file);
    ev.target.value = "";
  }
  onTSPFileChange(ev: any) {
    let workBook = null;
    let jsonData = null;
    const reader = new FileReader();
    const file = ev.target.files[0];
    reader.onload = (event) => {
      const data = reader.result;
      workBook = XLSX.read(data, { type: "binary" });
      jsonData = workBook.SheetNames.reduce((initial, name) => {
        const sheet = workBook.Sheets[name];
        initial[name] = XLSX.utils.sheet_to_json(sheet);
        return initial;
      }, {});
      const dataString = JSON.stringify(jsonData);
      this.data = JSON.parse(dataString);
      console.log(this.pbteTSPs.filteredData);
      console.log(this.data.TSP_PBTE);
      if (!this.data.TSP_PBTE) {
        this.ComSrv.ShowError(
          "Sheet with the name 'TSP_PBTE' not found in Excel file"
        );
        return false;
      }
      this.selectedTSPs = this.pbteTSPs.filteredData;
      this.selectedTSPs = this.data.TSP_PBTE.filter((x) =>
        this.pbteTSPs.filteredData
          .map((y) => y.TSPCode)
          .includes(x.TSPCode.toString())
      );
      console.log(this.selectedTSPs);
      if (this.selectedTSPs.length == 0) {
        this.error = "No matched TSP to update";
        this.ComSrv.ShowError(this.error.toString(), "Error");
        return false;
      }
      this.ComSrv.postJSON(
        "api/PBTE/UpdatePBTETSPs",
        this.selectedTSPs
      ).subscribe(
        (d: any) => {
          this.update = "PBTE imported for TSPs";
          this.ComSrv.openSnackBar(this.update.toString(), "Updated");
        },
        (error) => (this.error = error) // error path
      );
    };
    reader.readAsBinaryString(file);
    ev.target.value = "";
  }
  onNAVTTCTSPFileChange(ev: any) {
    let workBook = null;
    let jsonData = null;
    const reader = new FileReader();
    const file = ev.target.files[0];
    reader.onload = (event) => {
      const data = reader.result;
      workBook = XLSX.read(data, { type: "binary" });
      jsonData = workBook.SheetNames.reduce((initial, name) => {
        const sheet = workBook.Sheets[name];
        initial[name] = XLSX.utils.sheet_to_json(sheet);
        return initial;
      }, {});
      const dataString = JSON.stringify(jsonData);
      this.data = JSON.parse(dataString);
      console.log(this.navttcTSPs.filteredData);
      console.log(this.data.TSP_NAVTTC);
      if (!this.data.TSP_NAVTTC) {
        this.ComSrv.ShowError(
          "Sheet with the name 'TSP_NAVTTC' not found in Excel file"
        );
        return false;
      }
      this.selectedTSPs = this.navttcTSPs.filteredData;
      this.selectedTSPs = this.data.TSP_NAVTTC.filter((x) =>
        this.navttcTSPs.filteredData
          .map((y) => y.TSPCode)
          .includes(x.TSPCode.toString())
      );
      console.log(this.selectedTSPs);
      if (this.selectedTSPs.length == 0) {
        this.error = "No matched TSP to update";
        this.ComSrv.ShowError(this.error.toString(), "Error");
        return false;
      }
      this.ComSrv.postJSON(
        "api/PBTE/UpdatePBTETSPs",
        this.selectedTSPs
      ).subscribe(
        (d: any) => {
          this.update = "NAVTTC imported for TSPs";
          this.ComSrv.openSnackBar(this.update.toString(), "Updated");
        },
        (error) => (this.error = error) // error path
      );
    };
    reader.readAsBinaryString(file);
    ev.target.value = "";
  }
  onTradeFileChange(ev: any) {
    let workBook = null;
    let jsonData = null;
    const reader = new FileReader();
    const file = ev.target.files[0];
    reader.onload = (event) => {
      const data = reader.result;
      workBook = XLSX.read(data, { type: "binary" });
      jsonData = workBook.SheetNames.reduce((initial, name) => {
        const sheet = workBook.Sheets[name];
        initial[name] = XLSX.utils.sheet_to_json(sheet);
        return initial;
      }, {});
      const dataString = JSON.stringify(jsonData);
      this.data = JSON.parse(dataString);
      console.log(this.pbteTrades.filteredData);
      console.log(this.data.Trade_PBTE);
      if (!this.data.Trade_PBTE) {
        this.ComSrv.ShowError(
          "Sheet with the name 'Trade_PBTE' not found in Excel file"
        );
        return false;
      }
      this.selectedTrades = this.pbteTrades.filteredData;
      this.selectedTrades = this.data.Trade_PBTE.filter((x) =>
        this.pbteTrades.filteredData
          .map((y) => y.TradeCode)
          .includes(x.TradeCode)
      );
      console.log(this.selectedTrades);
      if (this.selectedTrades.length == 0) {
        this.error = "No matched Trade to update";
        this.ComSrv.ShowError(this.error.toString(), "Error");
        return false;
      }
      this.ComSrv.postJSON(
        "api/PBTE/UpdatePBTETrades",
        this.selectedTrades
      ).subscribe(
        (d: any) => {
          this.update = "PBTE imported for Trades";
          this.ComSrv.openSnackBar(this.update.toString(), "Updated"); // this.inceptionreport =new MatTableDataSource(d);
        },
        (error) => (this.error = error) // error path
      );
    };
    reader.readAsBinaryString(file);
    ev.target.value = "";
  }
  applyFilter(filterValue: string) {
    filterValue = filterValue.trim(); // Remove whitespace
    filterValue = filterValue.toLowerCase(); // Datasource defaults to lowercase matches
    this.pbteClasses.filter = filterValue;
  }
  search: any = "";

  applyTraineefilter(filterValue: string) {
    console.log(this.search);
    filterValue = filterValue.trim(); // Remove whitespace
    filterValue = filterValue.toLowerCase(); // Datasource defaults to lowercase matches
    this.traineeTablesData.filter = filterValue;
  }
  applyFilter1(filterValue: string) {
    console.log(this.search);
    filterValue = filterValue.trim(); // Remove whitespace
    filterValue = filterValue.toLowerCase(); // Datasource defaults to lowercase matches
    this.tTablesData.filter = filterValue;
  }

  applyFilter2(filterValue: string) {
    console.log(this.search);
    filterValue = filterValue.trim(); // Remove whitespace
    filterValue = filterValue.toLowerCase(); // Datasource defaults to lowercase matches
    this.cTablesData.filter = filterValue;
  }
  generateExcel() {
    let timeSpan = new Date().toISOString();
    //let prefix = this.input.Title || "Exported";
    let fileName = `Trades List`;
    //let uncheckedKeys = this.columns.controls.filter(x => !x.value).map(x => this.getControlName(x));
    //let dataForExport = this.pbteTradesArray.values('TradeName', 'Duration');
    //let dataForExport = this.pbteTradesArray.map(x => { TradeName: x.TradeName, DurationName: x.DurationName });
    this.pbteTradesArray = this.pbteTrades.filteredData;
    let dataForExport = this.pbteTradesArray.map((x) => {
      return { TradeName: x.TradeName, Duration: x.Duration };
    });
    //let dataForExport = this.pbteTrades.data.values;
    if (this.filters.SchemeID != 0) {
      let Scheme = [];
      this.schemeForExport = this.Schemes.filter(
        (x) => x.SchemeID == this.filters.SchemeID
      );
      console.log(this.schemeForExport);
      console.log(this.schemeForExport[0]["SchemeName"]);
    }
    let workbook = new Workbook();
    let workSheet = workbook.addWorksheet();
    workSheet.mergeCells("A1:C2");
    ///SET TITLE
    //if (this.filters.SchemeID != 0) {
    //    let titleRow = workSheet.addRow(['Scheme & Trade']);
    //}
    //else {
    //    let titleRow = workSheet.addRow(['Unique Trade List & Its Duration (in Months):']);
    //}
    //let titleRow = workSheet.addRow(['Unique Trade List & Its Duration (in Months):']);
    if (this.filters.SchemeID != 0) {
      let schemeName = this.schemeForExport[0]["SchemeName"];
      console.log(schemeName);
      workSheet.getCell("A1:C2").value = "Scheme & Trade";
      let titleRow = workSheet.getCell("A1:C2");
      titleRow.font = { bold: true, size: 20 };
      titleRow.fill = {
        type: "pattern",
        pattern: "solid",
        fgColor: { argb: "04FFC5" },
      };
      titleRow.border = {
        top: { style: "thin" },
        left: { style: "thin" },
        bottom: { style: "thin" },
        right: { style: "thin" },
      };
      titleRow.font = { bold: true };
      titleRow.alignment = {
        vertical: "middle",
        horizontal: "center",
        readingOrder: "ltr",
      };
      workSheet.mergeCells("A3:C4");
      workSheet.getCell("A3:C4").value = schemeName;
      let schemeRow = workSheet.getCell("A3:C4");
      schemeRow.font = { bold: true, size: 20 };
      schemeRow.fill = {
        type: "pattern",
        pattern: "solid",
        fgColor: { argb: "FFAC04" },
      };
      schemeRow.border = {
        top: { style: "thin" },
        left: { style: "thin" },
        bottom: { style: "thin" },
        right: { style: "thin" },
      };
      schemeRow.font = { bold: true };
      schemeRow.alignment = {
        vertical: "middle",
        horizontal: "center",
        readingOrder: "ltr",
      };
      //let titleRow = workSheet.addRow(['Scheme Wise Trade List']);
      //let schemeRow = workSheet.addRow([schemeName]);
      //titleRow.font = { bold: true, size: 14 }
      //titleRow.height = 45;
      //titleRow.eachCell((cell, number) => {
      //    cell.fill = {
      //        type: 'pattern',
      //        pattern: 'solid',
      //        fgColor: { argb: 'cdcdcd' },
      //    }
      //    cell.border = { top: { style: 'thin' }, left: { style: 'thin' }, bottom: { style: 'thin' }, right: { style: 'thin' } }
      //    cell.font = { bold: true }
      //    cell.alignment = { vertical: 'middle', horizontal: 'center', readingOrder: "ltr" }
      //});
      //schemeRow.font = { bold: true, size: 14 }
      //schemeRow.height = 45;
      //schemeRow.eachCell((cell, number) => {
      //    cell.fill = {
      //        type: 'pattern',
      //        pattern: 'solid',
      //        fgColor: { argb: 'FF8000' },
      //    }
      //    cell.border = { top: { style: 'thin' }, left: { style: 'thin' }, bottom: { style: 'thin' }, right: { style: 'thin' } }
      //    cell.font = { bold: true }
      //    cell.alignment = { vertical: 'middle', horizontal: 'center', readingOrder: "ltr" }
      //});
    } else {
      //let titleRow = workSheet.addRow(['Unique Trade List & Its Duration (in Months):']);
      workSheet.getCell("A1:C2").value =
        "Unique Trade List & Its Duration (in Months):";
      let titleRow = workSheet.getCell("A1:C2");
      titleRow.font = { bold: true, size: 20 };
      titleRow.fill = {
        type: "pattern",
        pattern: "solid",
        fgColor: { argb: "04FFC5" },
      };
      titleRow.border = {
        top: { style: "thin" },
        left: { style: "thin" },
        bottom: { style: "thin" },
        right: { style: "thin" },
      };
      titleRow.font = { bold: true };
      titleRow.alignment = {
        vertical: "middle",
        horizontal: "center",
        readingOrder: "ltr",
      };
      //titleRow.height = 45;
      //titleRow.eachCell((cell, number) => {
      //    cell.fill = {
      //        type: 'pattern',
      //        pattern: 'solid',
      //        fgColor: { argb: 'cdcdcd' },
      //    }
    }
    dataForExport.forEach((item, index) => {
      let keys = Object.keys(item);
      let values = Object.values(item);
      //SET SERIAL NUMBER
      keys.unshift("Sr#");
      values.unshift(++index);
      index--;
      if (index == 0) {
        ///SET HEADER
        let headerRow = workSheet.addRow(keys);
        headerRow.height = 25;
        headerRow.eachCell((cell, number) => {
          cell.fill = {
            type: "pattern",
            pattern: "solid",
            fgColor: { argb: "cdcdcd" },
            //bgColor: { argb: 'cdcdcd' }
          };
          cell.border = {
            top: { style: "thin" },
            left: { style: "thin" },
            bottom: { style: "thin" },
            right: { style: "thin" },
          };
          cell.font = { bold: true };
          cell.alignment = {
            vertical: "middle",
            horizontal: "center",
            readingOrder: "ltr",
          };
        });
      }
      ///SET COLUMN VALUES
      let row = workSheet.addRow(values);
      row.height = 80;
      row.eachCell((cell, number) => {
        cell.border = {
          top: { style: "thin" },
          left: { style: "thin" },
          bottom: { style: "thin" },
          right: { style: "thin" },
        };
        cell.alignment = {
          vertical: "middle",
          horizontal: "center",
          readingOrder: "ltr",
        };
        workSheet.getColumn(number).width = 30;
      });
    });
    workbook.xlsx
      .writeBuffer()
      .then((data) => {
        let blob = new Blob([data], {
          type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
        });
        fs.saveAs(blob, `${fileName}.${ExportType.XLSX}`);
        //this.onNoClick();
      })
      .catch((error) => {
        console.error(error);
        //this.onNoClick();
      });
  }
  generateTraineeExcel(tablename: string) {
    let timeSpan = new Date().toISOString();
    //let prefix = this.input.Title || "Exported";
    //let fileName = `${prefix} - ${timeSpan}`;
    //let uncheckedKeys = this.columns.controls.filter(x => !x.value).map(x => this.getControlName(x));
    let dataForExport = [];
    if (tablename == "PBTE-Trainees") {
      dataForExport = this.populateData(this.pbteTrainees.filteredData);
    }
    if (tablename == "NAVTTC-Trainees") {
      dataForExport = this.populateData(this.navttcTrainees.filteredData);
    }
    let workbookTrainee = new Workbook();
    let workSheetTrainee = workbookTrainee.addWorksheet();
    let exportExcel: ExportExcel = {
      Title: "Trainee PBTE Report",
      Author: "",
      Type: 0,
      Data: "",
      List1: [],
      ImageFieldNames: ["Trainee Img"],
    };
    dataForExport.forEach((item, index) => {
      let keys = Object.keys(item);
      //let values = Object.entries(item).map(([key, value]) => value);
      let values = Object.values(item);
      ///SET SERIAL NUMBER
      //keys.unshift("Sr#");
      //values.unshift(++index)
      //index--;
      if (index == 0) {
        ///SET HEADER
        let headerRow = workSheetTrainee.addRow(keys);
        headerRow.height = 25;
        headerRow.eachCell((cell, number) => {
          cell.fill = {
            type: "pattern",
            pattern: "solid",
            fgColor: { argb: "cdcdcd" },
            //bgColor: { argb: 'cdcdcd' }
          };
          cell.border = {
            top: { style: "thin" },
            left: { style: "thin" },
            bottom: { style: "thin" },
            right: { style: "thin" },
          };
          cell.font = { bold: true };
          cell.alignment = {
            vertical: "middle",
            horizontal: "center",
            readingOrder: "ltr",
          };
        });
      }
      ///SET COLUMN VALUES
      let row = workSheetTrainee.addRow(values);
      row.height = 80;
      row.eachCell((cell, number) => {
        cell.border = {
          top: { style: "thin" },
          left: { style: "thin" },
          bottom: { style: "thin" },
          right: { style: "thin" },
        };
        cell.alignment = { vertical: "middle", readingOrder: "ltr" };
        workSheetTrainee.getColumn(number).width = 20;
      });
      ///SET IMAGE
      keys.forEach((key, indx) => {
        if (exportExcel.ImageFieldNames.includes(key) && item[key] != "") {
          let image = workbookTrainee.addImage({
            base64: item[key],
            extension: "jpeg",
          });
          let cell = row.getCell(indx + 1);
          cell.value = "";
          cell.removeName;
          //workSheet.addImage(image, `${cell.address}:${cell.address}`);
          workSheetTrainee.addImage(image, {
            tl: { col: indx + 0.5, row: row.number - 1 + 0.2 },
            ext: { width: 80, height: 80 },
            editAs: "absolute",
          });
        }
      });
    });
    workbookTrainee.xlsx
      .writeBuffer()
      .then((data) => {
        let blob = new Blob([data], {
          type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
        });
        if (tablename == "NAVTTC-Trainees") {
          fs.saveAs(blob, `NAVTTC_Trainees.${ExportType.XLSX}`);
        } else {
          fs.saveAs(blob, `PBTE_Trainees.${ExportType.XLSX}`);
        }
        //this.onNoClick();
      })
      .catch((error) => {
        console.error(error);
        //this.onNoClick();
      });
  }
  populateData(data: any) {
    return data.map((item, index) => {
      return {
        //"Sr#": ++index
        "Class Code": item.ClassCode,
        Batch: item.Batch,
        Course: item.TradeName,
        Course_CatID: item.TradeID,
        "Trainee Name": item.TraineeName,
        //, "Trainee Code": item.TraineeCode
        "Father's Name": item.FatherName,
        CNIC: item.TraineeCNIC,
        DOB: this._date.transform(item.DateOfBirth, "dd/MM/yyyy"),
        //, "Batch": item.Batch
        Education: item.Education,
        "Trainee Img": item.TraineeImg,
        TraineeID: item.TraineeCode,
        "Student ID": item.PBTEID,
        "College ID": item.CollegeID,
      };
    });
  }
  GroupTSPs(tspArray: any) {
    this.groupedTSPsArrayForData = [];
    this.pbteTSPs = new MatTableDataSource(
      new GroupByPipe().transform(tspArray.filteredData, "TSPMasterID")
    );
    //this.groupedTSPsArray = (new GroupByPipe().transform(this.pbteTSPsArray, 'TSPID'));
    this.pbteTSPs.filteredData.forEach((key) => {
      let obj = [];
      this.groupedTSPsArray = this.pbteTSPsArrayToFilter.filter(
        (t) => t.TSPMasterID === Number(key.key)
      );
      //this.groupedTSPsArray.map(x=> x.ClassID = this.groupedTSPsArray.map(t => t.ClassID));
      this.groupedTSPsArrayForData.push(this.groupedTSPsArray[0]);
    });
    this.pbteTSPs = new MatTableDataSource(this.groupedTSPsArrayForData);
    //this.pbteTSPsAfterReset = new MatTableDataSource(this.groupedTSPsArrayForData);
    console.log(this.groupedTSPsArrayForData);
  }
  GroupNavttcTSPs(tspArray: any) {
    this.groupednavttcTSPsArrayForData = [];
    this.navttcTSPs = new MatTableDataSource(
      new GroupByPipe().transform(tspArray.filteredData, "TSPMasterID")
    );
    //this.groupedTSPsArray = (new GroupByPipe().transform(this.pbteTSPsArray, 'TSPID'));
    this.navttcTSPs.filteredData.forEach((key) => {
      let obj = [];
      this.groupednavttcTSPsArray = this.navttcTSPsArrayToFilter.filter(
        (t) => t.TSPMasterID === Number(key.key)
      );
      //this.groupedTSPsArray.map(x=> x.ClassID = this.groupedTSPsArray.map(t => t.ClassID));
      this.groupednavttcTSPsArrayForData.push(this.groupednavttcTSPsArray[0]);
    });
    this.navttcTSPs = new MatTableDataSource(
      this.groupednavttcTSPsArrayForData
    );
    //this.pbteTSPsAfterReset = new MatTableDataSource(this.groupedTSPsArrayForData);
    console.log(this.groupednavttcTSPsArrayForData);
  }
  SortANDPaginate() {
    this.pbteClasses.paginator = this.PageClass;
    this.pbteClasses.sort = this.SortClass;
    this.pbteTSPs.paginator = this.PageTSP;
    this.pbteTSPs.sort = this.SortTSP;
    this.pbteTrainees.paginator = this.PageTrainee;
    this.pbteTrainees.sort = this.SortTrainee;
    this.pbteExamResultTrainees.paginator = this.PageExamResultTrainee;
    this.pbteExamResultTrainees.sort = this.SortExamResultTrainee;
    this.pbteDropOutTrainees.paginator = this.PageDropOutTrainee;
    this.pbteDropOutTrainees.sort = this.SortDropOutTrainee;
    this.pbteTrades.paginator = this.PageTrade;
    this.pbteTrades.sort = this.SortTrade;
    this.navttcClasses.paginator = this.PageNavttcClass;
    this.navttcClasses.sort = this.SortNavttcClass;
    this.navttcTSPs.paginator = this.PageNavttcTSP;
    this.navttcTSPs.sort = this.SortNavttcTSP;
    this.navttcTrainees.paginator = this.PageNavttcTrainee;
    this.navttcTrainees.sort = this.SortNavttcTrainee;
    this.navttcExamResultTrainees.paginator = this.PageNAVTTCExamResultTrainee;
    this.navttcExamResultTrainees.sort = this.SortNAVTTCExamResultTrainee;
    this.navttcDropOutTrainees.paginator = this.PageNavttcDropOutTrainee;
    this.navttcDropOutTrainees.sort = this.SortNavttcDropOutTrainee;
  }
  get InActive() {
    return this.pbteform.get("InActive");
  }
  openTraineeJourneyDialogue(data: any): void {
    this.dialogueService.openTraineeJourneyDialogue(data);
  }
  openClassJourneyDialogue(data: any): void {
    this.dialogueService.openClassJourneyDialogue(data);
  }
}
export interface IQueryFilters {
  SchemeID: number;
  TSPID: number;
  ClassID: number;
  TradeID: number;
  DistrictID: number;
}
