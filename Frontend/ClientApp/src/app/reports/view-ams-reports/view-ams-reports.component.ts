import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators, AbstractControl } from '@angular/forms';
import { MatDatepicker } from '@angular/material/datepicker';
import * as _moment from 'moment';
import { Moment } from 'moment';
import { CommonSrvService } from '../../common-srv.service';
import { IAMSFilters } from '../Interfaces/IAMSFilters';
import { MatTableDataSource } from '@angular/material/table';
import { ExportType, EnumExcelReportType, EnumAmsReports, EnumUserLevel } from '../../shared/Enumerations';
import { Workbook } from 'exceljs';
import * as fs from 'file-saver';
import { ExportExcel } from '../../shared/Interfaces';
import { DateAdapter, MAT_DATE_LOCALE, MAT_DATE_FORMATS } from '@angular/material/core';
import { MomentDateAdapter, MAT_MOMENT_DATE_ADAPTER_OPTIONS } from '@angular/material-moment-adapter';
const moment = _moment;
import { GroupByPipe } from 'angular-pipes';
import { DatePipe } from '@angular/common';
import { UsersModel } from '../../master-data/users/users.component';


// See the Moment.js docs for the meaning of these formats:
// https://momentjs.com/docs/#/displaying/format/
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

@Component({
  selector: 'app-view-ams-reports',
  templateUrl: './view-ams-reports.component.html',
  styleUrls: ['./view-ams-reports.component.scss'],
  providers: [
    // `MomentDateAdapter` can be automatically provided by importing `MomentDateModule` in your
    // application's root module. We provide it at the component level here, due to limitations of
    // our example generation script.
    {
      provide: DateAdapter,
      useClass: MomentDateAdapter,
      deps: [MAT_DATE_LOCALE, MAT_MOMENT_DATE_ADAPTER_OPTIONS]
    },

    { provide: MAT_DATE_FORMATS, useValue: MY_FORMATS },
    DatePipe
  ],
})
export class ViewAmsReportsComponent implements OnInit {
  filters: IAMSFilters = {
    SchemeID: 0,
    ClassID: 0,
    TSPID: 0,
    UserID: 0,
    ReportName: '',
    DateMonth: '',
  };
  SearchSch = new FormControl('');
  SearchCls = new FormControl('');
  SearchTSP = new FormControl('');
  SearchRpt = new FormControl('');
  classesArray: any[];
  kamAssignedUsers: any;
  Scheme = [];
  TSPDetail = [];
  vsReportData = [];
  groupedByTSPsArray: any;

  currentUser: UsersModel;
  enumUserLevel = EnumUserLevel;
  userid: number;
  userObj: any[];
  kamRoleId: number;
  isInternalUser = false;
  isTSPUser = false;

  error = '';
  // VisitEndDate = new FormControl(moment());
  ReportArray: any[] = [];
  genForm = this.fb.group({
    ReportName: ['', [Validators.required]],
    SchemeID: ['0'],
    TSPID: ['', [Validators.required]],
    ClassID: ['', [Validators.required]],
    DateMonth: [moment(), [Validators.required]]
  });
  // cmReportData: any;
  working: boolean;
  customSr: number;

  constructor(private ComSrv: CommonSrvService, private fb: FormBuilder, private datePipe: DatePipe) { }

  ngOnInit(): void {
    this.ComSrv.setTitle("AMS Reports");

    this.checkForKAMUser();
    this.ReportArray = Object.keys(EnumAmsReports).map(key => ({ key, value: EnumAmsReports[key] }));
    this.currentUser = this.ComSrv.getUserDetails();

    if (this.currentUser.UserLevel === EnumUserLevel.TSP) {
      this.filters.UserID = this.currentUser.UserID;
      this.ReportArray = this.ReportArray.filter(x => x.value === EnumAmsReports.AttendancePerception
        || x.value === EnumAmsReports.ViolationSummary
        || x.value === EnumAmsReports.ProfileVerification
        || x.value === EnumAmsReports.ConfirmedMarginal
        || x.value === EnumAmsReports.DeletedDropout
        || x.value === EnumAmsReports.AdditionalTrainees
      )
    }

    this.ReportName.valueChanges.subscribe(value => {
      if (value === EnumAmsReports.CenterInspection || value === EnumAmsReports.FormIII
        // || value === EnumAmsReports.UnverifiedTrainee
        || value === EnumAmsReports.ProfileVerification) {
        this.updateRequiredValidator(this.TSPID, true);
        this.updateRequiredValidator(this.ClassID, true);
        this.updateRequiredValidator(this.DateMonth, false);
      } else {
        this.updateRequiredValidator(this.TSPID, false);
        this.updateRequiredValidator(this.ClassID, false);
        this.updateRequiredValidator(this.DateMonth, true);
      }
      if (value === EnumAmsReports.FormIV) {
        this.updateRequiredValidator(this.TSPID, true);
        this.updateRequiredValidator(this.DateMonth, false);
      } else {
        this.updateRequiredValidator(this.TSPID, false);
        this.updateRequiredValidator(this.DateMonth, true);
      }
    })
  }

  updateRequiredValidator(control: AbstractControl, isRequired: boolean) {
    if (isRequired) {
      control.setValidators([Validators.required]);
      control.updateValueAndValidity();
      control.markAsTouched();
    } else {
      control.clearValidators()
      control.updateValueAndValidity();
      control.markAsTouched();
    }
  }
  chosenYearHandler(normalizedYear: Moment) {
    const ctrlValue = this.DateMonth.value;
    ctrlValue.year(normalizedYear.year());
    this.DateMonth.setValue(ctrlValue);
  }
  chosenMonthHandler(
    normalizedMonth: Moment,
    datepicker: MatDatepicker<Moment>
  ) {
    const ctrlValue = this.DateMonth.value;
    ctrlValue.month(normalizedMonth.month());
    this.DateMonth.setValue(ctrlValue);
    //
    datepicker.close();
  }
  EmptyCtrl() {
    this.SearchCls.setValue('');
    this.SearchTSP.setValue('');
    this.SearchSch.setValue('');
    this.SearchRpt.setValue('');
  }

  checkForKAMUser() {
    this.currentUser = this.ComSrv.getUserDetails();
    this.userid = this.currentUser.UserID;

    this.ComSrv.getJSON('api/KAMAssignment/RD_KAMAssignmentForFilters').subscribe((d: any) => {
      this.kamAssignedUsers = d;
      this.userObj = this.kamAssignedUsers.filter(y => y.UserID === this.userid);
      if (this.userObj.length > 0) {
        this.kamRoleId = this.userObj.map(x => x.RoleID)[0];
      }
      if (this.kamRoleId) {
        this.getSchemesByKAM();
      }
      else {
        this.getSchemes();
      }

      // x.UserID, y => y.RoleID)
    },
    );
  }
  getSchemes() {
    this.Scheme = [];
    this.ComSrv.getJSON('api/Dashboard/FetchSchemesByUsers?UserId=' + this.userid).subscribe(
      (d: any) => {
        this.error = '';
        this.Scheme = d;
      },
      (error) => (this.error = `${error.name} , ${error.statusText}`)
    );
  }
  getSchemesByKAM() {
    this.Scheme = [];
    this.ComSrv.getJSON('api/Dashboard/FetchSchemesByKam?KamID=' + this.userid).subscribe(
      (d: any) => {
        this.error = '';
        this.Scheme = d;
      },
      (error) => (this.error = `${error.name} , ${error.statusText}`)
    );
  }
  getDependantFilters() {
    if (this.currentUser.UserLevel === this.enumUserLevel.TSP) {
      this.getClassesBySchemeFilter();
    }
    else {
      this.getUserRelevantTSPs();
    }
  }
  getClassesBySchemeFilter() {
    this.filters.ClassID = 0;
    this.ComSrv.getJSON(`api/Dashboard/FetchClassesBySchemeUser?SchemeID=${this.SchemeID.value}&UserID=${this.userid}`)
      .subscribe(data => {
        this.classesArray = (data as any[]);
      }, error => {
        this.error = error;
      })
  }
  getUserRelevantTSPs() {
    this.filters.SchemeID = this.SchemeID.value;
    if (this.kamRoleId) {
      this.KAMRelevantTSPsByScheme();
    }
    else {
      this.getTSPDetailByScheme(this.filters.SchemeID);
    }
  }
  KAMRelevantTSPsByScheme() {
    this.TSPDetail = [];
    this.classesArray = [];
    this.TSPID.setValue('');
    this.ClassID.setValue('');
    if (this.SchemeID.value != '') {
      this.ComSrv.getJSON(
        `api/Dashboard/FetchTSPsByKamScheme?SchemeID=${this.SchemeID.value}&KamID=${this.userid}`
      ).subscribe(
        (data) => {
          this.TSPDetail = (data as any[]);
        },
        (error) => {
          this.error = `${error.name} , ${error.statusText}`;
          console.log(error)
        }
      );
    }
  }
  getTSPDetailByScheme(schemeId: number) {
    this.TSPDetail = [];
    this.classesArray = [];
    this.TSPID.setValue('');
    this.ClassID.setValue('');
    if (this.SchemeID.value != '') {
      this.ComSrv.getJSON(`api/Dashboard/FetchTSPsByScheme?SchemeID=` + schemeId
      ).subscribe(
        (data) => {
          this.TSPDetail = (data as any[]);
        },
        (error) => {
          this.error = `${error.name} , ${error.statusText}`;
          console.log(error)
        }
      );
    }
  }
  getClassesByTsp(tspId: any) {
    if (tspId === '') return
    this.ClassID.setValue('');
    this.classesArray = [];
    this.ComSrv.getJSON(`api/Dashboard/FetchClassesByTSP?TspID=${tspId}`).subscribe(
      (data) => {
        this.error = '';
        this.classesArray = (data as any[]);
      },
      (error) => {
        // debugger
        this.error = `${error.name} , ${error.statusText}`;
      }
    );
  }

  generateReport() {
    let getFilter;
    if (!this.genForm.valid) return;
    if (this.genForm.valid) {
      if (this.SchemeID.value != '') {
        const filter = {
          SchemeID: this.SchemeID.value
          , TSPID: this.TSPID.value
          , ClassID: this.ClassID.value
          , Month: this.DateMonth.value
          , UserID: this.filters.UserID
        };
        switch (this.ReportName.value) {
          case EnumAmsReports.ConfirmedMarginal:
            this.ComSrv.postJSON('api/AMSReports/GetCMReport', filter).subscribe(

              (data: any) => {
                this.error = '';
                if (data) {
                  this.generateCMReportExcel(data);
                } else {
                  this.error = 'Not Found Data.'
                }
              }
              , (error) => {
                this.error = `${error.name} , ${error.statusText}`;
                console.error(this.error);
              })
            break;
          case EnumAmsReports.ViolationSummary:
            this.ComSrv.postJSON('api/AMSReports/GetVSReport', filter).subscribe(
              (data: any) => {
                this.error = '';
                this.vsReportData = data;
                if (data) {
                  this.generateVSReportExcel(data);
                } else {
                  this.error = 'Not Found Data.'
                }
              }
              , (error) => {
                this.error = `${error.name} , ${error.statusText}`;
                console.error(this.error);
              })
            break;
          case EnumAmsReports.DeletedDropout:
            this.ComSrv.postJSON('api/AMSReports/GetDDReport', filter).subscribe(
              (data: any) => {
                this.error = '';
                if (data) {
                  this.generateDDReportExcel(data);
                } else {
                  this.error = 'Not Found Data.'
                }
              }
              , (error) => {
                this.error = `${error.name} , ${error.statusText}`;
                console.error(this.error);
              })
            break;
          case EnumAmsReports.AttendancePerception:
            this.ComSrv.postJSON('api/AMSReports/GetAPReport', filter).subscribe(
              (data: any) => {
                this.error = '';
                if (data) {
                  this.generateAPReportExcel(data);
                } else {
                  this.error = 'Not Found Data.'
                }
              }
              , (error) => {
                this.error = `${error.name} , ${error.statusText}`;
                console.error(this.error);
              })
            break;
          case EnumAmsReports.ReportExecutiveSummary:
            this.ComSrv.postJSON('api/AMSReports/GetPESReport', filter).subscribe(
              (data: any) => {
                this.error = '';
                if (data) {
                  this.generatePESReportExcel(data);
                } else {
                  this.error = 'Not Found Data.'
                }
              }
              , (error) => {
                this.error = `${error.name} , ${error.statusText}`;
                console.error(this.error);
              })
            break;
          case EnumAmsReports.AdditionalTrainees:
            this.ComSrv.postJSON('api/AMSReports/GetATReport', filter).subscribe(
              (data: any) => {
                this.error = '';
                if (data) {
                  this.generateATReportExcel(data);
                } else {
                  this.error = 'Not Found Data.'
                }
              }
              , (error) => {
                this.error = `${error.name} , ${error.statusText}`;
                console.error(this.error);
              })
            break;
          case EnumAmsReports.FakeGhostTrainee:
            this.ComSrv.postJSON('api/AMSReports/GetFGTReport', filter).subscribe(
              (data: any) => {
                this.error = '';
                if (data) {
                  this.generateFGTReportExcel(data);
                } else {
                  this.error = 'Not Found Data.'
                }
              }
              , (error) => {
                this.error = `${error.name} , ${error.statusText}`;
                console.error(this.error);
              })
            break;
          case EnumAmsReports.CovidMaskViolation:
            this.ComSrv.postJSON('api/AMSReports/GetCMVReport', filter).subscribe(
              (data: any) => {
                this.error = '';
                if (data) {
                  this.generateCMVReportExcel(data);
                } else {
                  this.error = 'Not Found Data.'
                }
              }
              , (error) => {
                this.error = `${error.name} , ${error.statusText}`;
                console.error(this.error);
              })
            break;
          case EnumAmsReports.EmploymentVerification:
            // debugger;
            this.ComSrv.postJSON('api/AMSReports/GetEVRReport', filter).subscribe(
              (data: any) => {
                this.error = '';
                if (data) {
                  this.generateEVRReportExcel(data);
                } else {
                  this.error = 'Not Found Data.'
                }
              }
              , (error) => {
                this.error = `${error.name} , ${error.statusText}`;
                console.error(this.error);
              })
            break;
          case EnumAmsReports.FormIII:
            getFilter = `schemeId=${this.SchemeID.value}&tspId=${this.TSPID.value}&classId=${this.ClassID.value}&date=${moment(this.DateMonth.value).format('DD/MM/YYYY')}`;
            window.open(`${this.ComSrv.appConfig.UsersAPIURL}ams/AMSReports/FormIII?${getFilter}`, '_blank');
            break;
          case EnumAmsReports.CenterInspection:
            getFilter = `classId=${this.ClassID.value}`;
            window.open(`${this.ComSrv.appConfig.UsersAPIURL}ams/AMSReports/CenterInspection?${getFilter}`, '_blank');
            break;
          case EnumAmsReports.FormIV:
            getFilter = `schemeId=${this.SchemeID.value}&tspId=${this.TSPID.value}&classId=${this.ClassID.value}&date=${moment(this.DateMonth.value).format('DD/MM/YYYY')}`;
            window.open(`${this.ComSrv.appConfig.UsersAPIURL}ams/AMSReports/FormIV?${getFilter}`, '_blank');
            break;
          case EnumAmsReports.ProfileVerification:
            getFilter = `schemeId=${this.SchemeID.value}&tspId=${this.TSPID.value}&classId=${this.ClassID.value}&date=${moment(this.DateMonth.value).format('DD/MM/YYYY')}`;
            window.open(`${this.ComSrv.appConfig.UsersAPIURL}ams/AMSReports/ProfileVerification?${getFilter}`, '_blank');
            break;
          default:
            break;
        }
      }

    }
  }

  // Confirm Marginal /// start////
  generateCMReportExcel(data: any) {
    const dataForExport = this.populateData(data);
    const workbook = new Workbook();
    const workSheet = workbook.addWorksheet();
    const detailArray = data[0].DetailString.split(',', 4);
    workSheet.mergeCells('A1:M5');
    workSheet.getCell('A1:M5').value = `${detailArray[0]}
${detailArray[2]}
${detailArray[3]}`;

    const titleRow = workSheet.getCell('A1:M5');
    titleRow.style = { alignment: { wrapText: true, readingOrder: 'ltr', vertical: 'top' } };
    titleRow.font = { bold: true, };


    // workSheet.mergeCells('A1:A2:A3:A4');


    workSheet.addRow([]);

    dataForExport.forEach((item, index) => {
      const keys = Object.keys(item);
      // let values = Object.entries(item).map(([key, value]) => value);
      const values = Object.values(item);

      /// SET SERIAL NUMBER
      keys.unshift('Sr#');
      values.unshift(++index)
      index--;

      if (index == 0) {
        /// SET HEADER
        const headerRow = workSheet.addRow(keys);
        headerRow.height = 25;
        headerRow.eachCell((cell, number) => {
          cell.fill = {
            type: 'pattern',
            pattern: 'solid',
            fgColor: { argb: 'FFA500' },
            // bgColor: { argb: 'cdcdcd' }
          }
          cell.border = { top: { style: 'thin' }, left: { style: 'thin' }, bottom: { style: 'thin' }, right: { style: 'thin' } }
          cell.font = { bold: true }
          cell.alignment = { vertical: 'middle', horizontal: 'center', readingOrder: 'ltr' }
        });
      };

      /// SET COLUMN VALUES
      const row = workSheet.addRow(values);
      row.height = 30;
      row.eachCell((cell, number) => {
        cell.border = { top: { style: 'thin' }, left: { style: 'thin' }, bottom: { style: 'thin' }, right: { style: 'thin' } };
        cell.alignment = { vertical: 'middle', readingOrder: 'ltr' }
        if (cell.value.toString().toLowerCase() == 'p') {
          cell.font = { name: 'Wingdings 2' }
          // cell.value = String.fromCodePoint(10003)
          cell.alignment = { vertical: 'middle', horizontal: 'center', }
        }
        workSheet.getColumn(number).width = 20;
      })

    });

    workbook.xlsx.writeBuffer().then((data) => {
      const blob = new Blob([data], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
      fs.saveAs(blob, `${this.ReportName.value}.${ExportType.XLSX}`);

    }).catch(error => {
      this.error = `${error.name} , ${error.statusText}`;
      console.error(error);
    });

  }
  populateData(data: any) {
    return data.map((item, index) => {
      let actionColumnVal = '';
      if (item.FirstMonthMarginal == 'P' || item.SecondMonthMarginal == 'P') {
        actionColumnVal = 'Rs 200 Stipend Deduction'
      }
      if (item.BSSStatus == 'expelled' || item.BSSStatus == 'drop-out') {
        actionColumnVal = 'No Action'
      }
      return {
        // "Sr#": ++index
        'Scheme Name': item.SchemeName
        , 'Service Provider Name': item.TSPName
        , ClassCode: item.ClassCode
        , 'Course Duration': item.ClassDuration
        , 'Trainee Code': item.TraineeCode
        , 'Trainee Name': item.TraineeName
        , 'Father/Husband Name': item.FatherName
        , CNIC: item.TraineeCNIC
        , ['Confirmed Marginal ' + item.FirstMonth]: item.FirstMonthMarginal
        , ['Confirmed Marginal ' + item.SecondMonth]: item.SecondMonthMarginal
        , Remarks: ''
        , 'Start Date': this.datePipe.transform(item.StartDate, 'dd/MM/yyyy')
        , 'Status in BSS': item.BSSStatus
        , 'Action to be Taken': actionColumnVal
      }

    })
  }
  // Confirm Marginal /// end////

  // violation summary /// start////
  generateVSReportExcel(data: any) {
    this.customSr = 0;

    // let dataForExport = this.populateVSReportData(data);

    let tspMinorTotal = 0;
    let tspMajorTotal = 0;
    let tspSeriousTotal = 0;
    let tspViolationTotal = 0;
    let tspObservationTotal = 0;
    const violationReportGrandTotal: any = [];

    const workbook = new Workbook();
    const workSheet = workbook.addWorksheet();

    workSheet.mergeCells('A1:J8');

    const schemeNameForReport = this.vsReportData[0].SchemeName;
    const monthForReport = this.vsReportData[0].MonthForReport;


    workSheet.getCell('A1:J8').value = `Punjab Skills Development Fund
  CLASS MONITORING REPORTS SUMMARY
  For the Month of ${this.datePipe.transform(monthForReport, 'MMMM yyyy')}`;

    const titleRow = workSheet.getCell('A1:J8');
    titleRow.style = { alignment: { wrapText: true, readingOrder: 'ltr', vertical: 'top' } };
    titleRow.font = { bold: true, };

    const headerarray = ['', '', '', '', '', '', 'Violation', '', 'Observation', ''];
    const header = workSheet.addRow(headerarray);
    header.height = 35;
    header.eachCell((cell, number) => {
      cell.fill = {
        type: 'pattern',
        pattern: 'solid',
        fgColor: { argb: 'FFA500' },
        // bgColor: { argb: 'cdcdcd' }
      }
      cell.border = { top: { style: 'thin' }, left: { style: 'thin' }, bottom: { style: 'thin' }, right: { style: 'thin' } }
      cell.font = { bold: true }
      cell.alignment = { vertical: 'middle', horizontal: 'center', readingOrder: 'ltr' }
    });

    workSheet.mergeCells('E9:H9');
    workSheet.getCell('E9:H9').value = 'Violation';




    this.groupedByTSPsArray = (new GroupByPipe().transform(this.vsReportData, 'TSPName'));

    this.groupedByTSPsArray.forEach((item, index) => {
      const obj = {};
      const tspHeaderRow = [];                // array to create a row to add tsp grouped name and calculations

      tspHeaderRow.push('---');
      tspHeaderRow.push('---');

      const groupHeaderTSP = item.value;      // array for tsp grouped data
      const groupedtspname = item.key;        // tsp name

      tspHeaderRow.push(groupedtspname);     // array to create a row to add tsp grouped name and calculations

      // get respective tsp coulumn values

      const minorArray = groupHeaderTSP.map(o => o.Minor);
      const majorArray = groupHeaderTSP.map(o => o.Major);
      const seriousArray = groupHeaderTSP.map(o => o.Serious);
      const violationtotalArray = groupHeaderTSP.map(o => o.Total);
      const observationtotalArray = groupHeaderTSP.map(o => o.Observation);

      // sum of coulumn values for grouped tsp header row

      const sumTSPMinor = minorArray.reduce((a, b) => a + b, 0);
      const sumTSPMajor = majorArray.reduce((a, b) => a + b, 0);
      const sumTSPSerious = seriousArray.reduce((a, b) => a + b, 0);
      const sumTSPViolationTotal = violationtotalArray.reduce((a, b) => a + b, 0);
      const sumTSPObservationTotal = observationtotalArray.reduce((a, b) => a + b, 0);


      tspMinorTotal = tspMinorTotal + sumTSPMinor
      tspMajorTotal = tspMajorTotal + sumTSPMajor
      tspSeriousTotal = tspSeriousTotal + sumTSPSerious
      tspViolationTotal = tspViolationTotal + sumTSPViolationTotal
      tspObservationTotal = tspObservationTotal + sumTSPObservationTotal
      // push data for coulumn values for grouped tsp header row

      tspHeaderRow.push('---');
      tspHeaderRow.push(sumTSPMinor);
      tspHeaderRow.push(sumTSPMajor);
      tspHeaderRow.push(sumTSPSerious);
      tspHeaderRow.push(sumTSPViolationTotal);
      tspHeaderRow.push(sumTSPObservationTotal);
      tspHeaderRow.push('---');

      // populate data for respective tsp

      const dataForExport = this.populateVSReportData(item.value)

      if (index == 0) {
        let keys;
        /// SET HEADER
        dataForExport.forEach((item) => {
          keys = Object.keys(item);
        });
        const headerRow = workSheet.addRow(keys);
        headerRow.height = 55;
        headerRow.eachCell((cell, number) => {
          cell.fill = {
            type: 'pattern',
            pattern: 'solid',
            fgColor: { argb: 'FFA500' },
            // bgColor: { argb: 'cdcdcd' }
          }
          cell.border = { top: { style: 'thin' }, left: { style: 'thin' }, bottom: { style: 'thin' }, right: { style: 'thin' } }
          cell.font = { bold: true }
          cell.alignment = { vertical: 'middle', horizontal: 'center', readingOrder: 'ltr' }
        });
      };

      dataForExport.forEach((item) => {
        // let values = Object.entries(item).map(([key, value]) => value);
        const values = Object.values(item);


        /// SET COLUMN VALUES
        const row = workSheet.addRow(values);
        row.height = 30;
        row.eachCell((cell, number) => {
          cell.border = { top: { style: 'thin' }, left: { style: 'thin' }, bottom: { style: 'thin' }, right: { style: 'thin' } };
          cell.alignment = { vertical: 'middle', horizontal: 'center', readingOrder: 'ltr' }
          workSheet.getColumn(number).width = 25;

        })
      });

      const tspfooter = workSheet.addRow(tspHeaderRow);
      tspfooter.height = 25;
      tspfooter.eachCell((cell, number) => {
        cell.fill = {
          type: 'pattern',
          pattern: 'solid',
          fgColor: { argb: 'CCE5FF' },
          // bgColor: { argb: 'cdcdcd' }
        }
        cell.border = { top: { style: 'thin' }, left: { style: 'thin' }, bottom: { style: 'thin' }, right: { style: 'thin' } }
        cell.font = { bold: true }
        cell.alignment = { vertical: 'middle', horizontal: 'center', readingOrder: 'ltr' }
      });

    });

    violationReportGrandTotal.push('---');
    violationReportGrandTotal.push('---');
    violationReportGrandTotal.push('Grand Total');
    violationReportGrandTotal.push('---');
    violationReportGrandTotal.push(tspMinorTotal);
    violationReportGrandTotal.push(tspMajorTotal);
    violationReportGrandTotal.push(tspSeriousTotal);
    violationReportGrandTotal.push(tspViolationTotal);
    violationReportGrandTotal.push(tspObservationTotal);
    violationReportGrandTotal.push('--');


    const tspGrandTotalRow = workSheet.addRow(violationReportGrandTotal);
    tspGrandTotalRow.height = 25;
    tspGrandTotalRow.eachCell((cell, number) => {
      cell.fill = {
        type: 'pattern',
        pattern: 'solid',
        fgColor: { argb: 'FFA500' },
        // bgColor: { argb: 'cdcdcd' }
      }
      cell.border = { top: { style: 'thin' }, left: { style: 'thin' }, bottom: { style: 'thin' }, right: { style: 'thin' } }
      cell.font = { bold: true }
      cell.alignment = { vertical: 'middle', horizontal: 'center', readingOrder: 'ltr' }
    });

    workbook.xlsx.writeBuffer().then((data) => {
      const blob = new Blob([data], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
      fs.saveAs(blob, `ViolationSummaryReport.${ExportType.XLSX}`);

      // this.onNoClick();
    }).catch(error => {
      console.error(error);
      // this.onNoClick();
    });


  }
  populateVSReportData(data: any) {
    return data.map((item, index) => {
      return {
        'Sr. No': ++this.customSr
        , 'Scheme Name': item.SchemeName
        , 'TSP Name': item.TSPName
        , 'Class Code': item.ClassCode
        , Minor: item.Minor
        , Major: item.Major
        , Serious: item.Serious
        , ' Total': item.Total      // ViolationTotal
        , Total: item.Observation    // ObservationTotal
        , Remarks: item.Remarks

      }
    })
  }
  // violation summary /// end////

  // DeletedDropout /// start////
  generateDDReportExcel(data: any) {
    const dataForExport = this.populateDDData(data);
    const workbook = new Workbook();
    const workSheet = workbook.addWorksheet();
    workSheet.mergeCells('A1:L6');
    const detailArray = data[0].DetailString.split(',', 4);
    workSheet.getCell('A1:L6').value = `${detailArray[0]}
${detailArray[2]}
${detailArray[3]}`;

    const titleRow = workSheet.getCell('A1:L6');
    titleRow.style = { alignment: { wrapText: true, readingOrder: 'ltr', vertical: 'top' } };
    titleRow.font = { bold: true, };
    // workSheet.mergeCells('A1:A2:A3:A4');


    workSheet.addRow([]);

    dataForExport.forEach((item, index) => {
      const keys = Object.keys(item);
      // let values = Object.entries(item).map(([key, value]) => value);
      const values = Object.values(item);

      /// SET SERIAL NUMBER
      keys.unshift('Sr#');
      values.unshift(++index)
      index--;

      if (index == 0) {
        /// SET HEADER
        const headerRow = workSheet.addRow(keys);
        headerRow.height = 25;
        headerRow.eachCell((cell, number) => {
          cell.fill = {
            type: 'pattern',
            pattern: 'solid',
            fgColor: { argb: 'FFA500' },
            // bgColor: { argb: 'cdcdcd' }
          }
          cell.border = { top: { style: 'thin' }, left: { style: 'thin' }, bottom: { style: 'thin' }, right: { style: 'thin' } }
          cell.font = { bold: true }
          cell.alignment = { vertical: 'middle', horizontal: 'center', readingOrder: 'ltr' }
        });
      };

      /// SET COLUMN VALUES
      const row = workSheet.addRow(values);
      row.height = 30;
      row.eachCell((cell, number) => {
        cell.border = { top: { style: 'thin' }, left: { style: 'thin' }, bottom: { style: 'thin' }, right: { style: 'thin' } };
        cell.alignment = { vertical: 'middle', readingOrder: 'ltr' }
        if (cell.value.toString().toLowerCase() == 'p') {
          cell.font = { name: 'Wingdings 2' }
          // cell.value = String.fromCodePoint(10003)
          cell.alignment = { vertical: 'middle', horizontal: 'center', }
        }
        workSheet.getColumn(number).width = 20;
      })

    });

    workbook.xlsx.writeBuffer().then((data) => {
      const blob = new Blob([data], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
      fs.saveAs(blob, `${this.ReportName.value}.${ExportType.XLSX}`);

    }).catch(error => {
      this.error = `${error.name} , ${error.statusText}`;
      console.error(error);
    });

  }
  populateDDData(data: any) {
    return data.map((item, index) => {
      // debugger;
      const actionColumnVal = 'No Action';
      // if (item.PreviousMonthDeleted == 'P' && item.SelectedMonthDeleted == 'P') {
      //  actionColumnVal = 'Trainee Should be Dropout'
      // }
      // if (item.StatusInMIS == 'Expelled' || item.StatusInMIS == 'Dropout') {
      //  actionColumnVal = 'No Action'
      // }
      // let firstmonth = item.PreviousMonth;
      // let secondmonth = item.SelectedMonth;
      return {
        // "Sr#": ++index
        'Scheme Name': item.SchemeName
        , 'Service Provider Name': item.TSPName
        , ClassCode: item.ClassCode
        , 'Trainee Name': item.TraineeName
        , 'Father/Husband Name': item.FatherName
        , CNIC: item.TraineeCNIC
        , [item.PreviousMonth]: item.TickStatusPreviousMonth
        , [item.SelectedMonth]: item.TickStatusSelectedMonth
        , Remarks: item.Remarks
        , 'Start Date': this.datePipe.transform(item.StartDate, 'dd/MM/yyyy')
        , 'Status In BSS': item.StatusInMIS
        , 'Action to be Taken': actionColumnVal
      }
    })
  }
  // DeletedDropout /// end////

  // Attendance Perception/// start////
  generateAPReportExcel(data: any) {
    const dataForExport = this.populateAPData(data);
    const workbook = new Workbook();
    const workSheet = workbook.addWorksheet();

    workSheet.mergeCells('A1:AX5');

    // const schemeNameForReport = data[0].SchemeName;
    const monthForReport = this.datePipe.transform(data[0].VisitDate, 'MMMM yyyy');
    //${schemeNameForReport}
    workSheet.getCell('A1:AX5').value = `Punjab Skills Development Fund
Trainees Attendance & Perception - Summarized Reporting
For the Month of ${monthForReport}`;

    const titleRow = workSheet.getCell('A1:AX5');
    titleRow.style = { alignment: { wrapText: true, readingOrder: 'ltr', vertical: 'top' } };
    titleRow.font = { bold: true, };

    // let headerarray = ['', '', '', '', '', 'Violation', '', 'Observation', ''];
    const header1 = workSheet.addRow(['', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '']);
    const header2 = workSheet.addRow(['', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '']);



    header1.height = 35;
    header1.eachCell((cell, number) => {
      cell.fill = {
        type: 'pattern',
        pattern: 'solid',
        fgColor: { argb: '000000' },
        // bgColor: { argb: 'cdcdcd' }
      }
      cell.border = { top: { style: 'thin' }, left: { style: 'thin' }, bottom: { style: 'thin' }, right: { style: 'thin' } }
      cell.font = {
        bold: true, color: { argb: 'FFFFFF' }
      }
      cell.alignment = { vertical: 'middle', horizontal: 'center', readingOrder: 'ltr' }
    });
    header2.height = 35;
    header2.eachCell((cell, number) => {
      cell.fill = {
        type: 'pattern',
        pattern: 'solid',
        fgColor: { argb: 'FFA500' },
        // bgColor: { argb: 'cdcdcd' }
      }
      cell.border = { top: { style: 'thin' }, left: { style: 'thin' }, bottom: { style: 'thin' }, right: { style: 'thin' } }
      cell.font = { bold: true }
      cell.alignment = { vertical: 'middle', horizontal: 'center', readingOrder: 'ltr' }
    });

    workSheet.mergeCells('F6:P6');
    workSheet.getCell('F6:P6').value = 'Visit-1';

    workSheet.mergeCells('Q6:AA6');
    workSheet.getCell('Q6:AA6').value = 'Visit-2';

    workSheet.mergeCells('AB6:AL6');
    workSheet.getCell('AB6:AL6').value = 'Visit-3';

    workSheet.mergeCells('AM6:AW6');
    workSheet.getCell('AM6:AW6').value = 'Visit-4';


    workSheet.mergeCells('F7:I7');
    workSheet.getCell('F7:I7').value = 'Attendance';

    workSheet.mergeCells('J7:P7');
    workSheet.getCell('J7:P7').value = 'Perception of (Percentage (%) Satisfied)';

    workSheet.mergeCells('Q7:T7');
    workSheet.getCell('Q7:T7').value = 'Attendance';

    workSheet.mergeCells('U7:AA7');
    workSheet.getCell('U7:AA7').value = 'Perception of (Percentage (%) Satisfied)';

    workSheet.mergeCells('AB7:AE7');
    workSheet.getCell('AB7:AE7').value = 'Attendance';

    workSheet.mergeCells('AF7:AL7');
    workSheet.getCell('AF7:AL7').value = 'Perception of (Percentage (%) Satisfied)';

    workSheet.mergeCells('AM7:AP7');
    workSheet.getCell('AM7:AP7').value = 'Attendance';

    workSheet.mergeCells('AQ7:AW7');
    workSheet.getCell('AQ7:AW7').value = 'Perception of (Percentage (%) Satisfied)';


    dataForExport.forEach((item, index) => {
      const keys = Object.keys(item);
      // let values = Object.entries(item).map(([key, value]) => value);
      const values = Object.values(item);

      /// SET SERIAL NUMBER
      keys.unshift('Sr#');
      values.unshift(++index)
      index--;

      if (index == 0) {
        /// SET HEADER
        const headerRow = workSheet.addRow(keys);
        headerRow.height = 25;
        headerRow.eachCell((cell, number) => {
          cell.fill = {
            type: 'pattern',
            pattern: 'solid',
            fgColor: { argb: 'FFA500' },
            // bgColor: { argb: 'cdcdcd' }
          }
          cell.border = { top: { style: 'thin' }, left: { style: 'thin' }, bottom: { style: 'thin' }, right: { style: 'thin' } }
          cell.font = { bold: true }
          cell.alignment = { vertical: 'middle', horizontal: 'center', readingOrder: 'ltr' }
        });
      };

      /// SET COLUMN VALUES
      const row = workSheet.addRow(values);
      row.height = 30;
      row.eachCell((cell, number) => {
        cell.border = { top: { style: 'thin' }, left: { style: 'thin' }, bottom: { style: 'thin' }, right: { style: 'thin' } };
        cell.alignment = { vertical: 'middle', horizontal: 'center', readingOrder: 'ltr' }
        workSheet.getColumn(number).width = 25;
      })

    });

    workbook.xlsx.writeBuffer().then((data) => {
      const blob = new Blob([data], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
      fs.saveAs(blob, `${this.ReportName.value}.${ExportType.XLSX}`);

    }).catch(error => {
      this.error = `${error.name} , ${error.statusText}`;
      console.error(error);
    });
  }
  populateAPData(data: any) {
    return data.map((item, index) => {
      return {
        // "Sr#": ++index,
        'Schme Name': item.SchemeName
        , 'Service Provider Name': item.TSPName
        , Trade: item.TradeName
        , ClassCode: item.ClassCode
        , 'Contractual Trainees': item.TraineesPerClass
        , 'Visit Date Attendance': item.VisitDateAttendance1
        , '1 day before': item.OneDayBefore1
        , '2 day before': item.TwoDayBefore1
        , '3 day before': item.ThreeDayBefore1
        , 'Sufficient Consumables': item.SufficientConsumablesVisit1
        , 'Sufficient equipment/tools': item.SufficientEquipmentTools1
        , 'Quality of practical training': item.QualityOfPracticalTrainingVisit1
        , 'Quality of meals': item.QualityOfMealsVisit1
        , 'Quality of boarding facility': item.QualityOfBoardingFacilityVisit1
        , 'Training Usefulness': item.TrainingUsefulnessVisit1
        , 'Average daily hours': item.AverageDailyHoursVisit1

        , 'Visit Date Attendance ': item.VisitDateAttendance2
        , '1 day before ': item.OneDayBefore2
        , '2 day before ': item.TwoDayBefore2
        , '3 day before ': item.ThreeDayBefore2
        , 'Sufficient Consumables ': item.SufficientConsumablesVisit2
        , 'Sufficient equipment/tools ': item.SufficientEquipmentTools2
        , 'Quality of practical training ': item.QualityOfPracticalTrainingVisit2
        , 'Quality of meals ': item.QualityOfMealsVisit2
        , 'Quality of boarding facility ': item.QualityOfBoardingFacilityVisit2
        , 'Training Usefulness ': item.TrainingUsefulnessVisit2
        , 'Average daily hours ': item.AverageDailyHoursVisit2

        , 'Visit Date Attendance  ': item.VisitDateAttendance3
        , '1 day before  ': item.OneDayBefore3
        , '2 day before  ': item.TwoDayBefore3
        , '3 day before  ': item.ThreeDayBefore3
        , 'Sufficient Consumables  ': item.SufficientConsumablesVisit3
        , 'Sufficient equipment/tools  ': item.SufficientEquipmentTools3
        , 'Quality of practical training  ': item.QualityOfPracticalTrainingVisit3
        , 'Quality of meals  ': item.QualityOfMealsVisit3
        , 'Quality of boarding facility  ': item.QualityOfBoardingFacilityVisit3
        , 'Training Usefulness  ': item.TrainingUsefulnessVisit3
        , 'Average daily hours  ': item.AverageDailyHoursVisit3

        , 'Visit Date Attendance   ': item.VisitDateAttendance4
        , '1 day before   ': item.OneDayBefore4
        , '2 day before   ': item.TwoDayBefore4
        , '3 day before   ': item.ThreeDayBefore4
        , 'Sufficient Consumables   ': item.SufficientConsumablesVisit4
        , 'Sufficient equipment/tools   ': item.SufficientEquipmentTools4
        , 'Quality of practical training   ': item.QualityOfPracticalTrainingVisit4
        , 'Quality of meals   ': item.QualityOfMealsVisit4
        , 'Quality of boarding facility   ': item.QualityOfBoardingFacilityVisit4
        , 'Training Usefulness   ': item.TrainingUsefulnessVisit4
        , 'Average daily hours   ': item.AverageDailyHoursVisit4

        , 'Remarks/Comments (If Any)': item.Remarks
      }
    })
  }
  // Attendance Perception /// end////

  // Report Executive Summary /// start////
  generatePESReportExcel(data: any) {
    const dataForExport = this.populatePESData(data);
    const workbook = new Workbook();
    const workSheet = workbook.addWorksheet();

    dataForExport.forEach((item, index) => {
      const keys = Object.keys(item);
      // let values = Object.entries(item).map(([key, value]) => value);
      const values = Object.values(item);

      /// SET SERIAL NUMBER
      keys.unshift('Sr#');
      values.unshift(++index)
      index--;

      if (index == 0) {
        /// SET HEADER
        const headerRow = workSheet.addRow(keys);
        headerRow.height = 25;
        headerRow.eachCell((cell, number) => {
          cell.fill = {
            type: 'pattern',
            pattern: 'solid',
            fgColor: { argb: 'FFA500' },
            // bgColor: { argb: 'cdcdcd' }
          }
          cell.border = { top: { style: 'thin' }, left: { style: 'thin' }, bottom: { style: 'thin' }, right: { style: 'thin' } }
          cell.font = { bold: true }
          cell.alignment = { vertical: 'middle', horizontal: 'center', readingOrder: 'ltr' }
        });
      };

      /// SET COLUMN VALUES
      const row = workSheet.addRow(values);
      row.height = 30;
      row.eachCell((cell, number) => {
        cell.border = { top: { style: 'thin' }, left: { style: 'thin' }, bottom: { style: 'thin' }, right: { style: 'thin' } };
        cell.alignment = { vertical: 'middle', readingOrder: 'ltr' }
        workSheet.getColumn(number).width = 20;
      })

    });

    workbook.xlsx.writeBuffer().then((data) => {
      const blob = new Blob([data], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
      fs.saveAs(blob, `${this.ReportName.value}.${ExportType.XLSX}`);

    }).catch(error => {
      this.error = `${error.name} , ${error.statusText}`;
      console.error(error);
    });

  }
  populatePESData(data: any) {
    return data.map((item, index) => {
      return {
        // "Sr#": ++index
        Scheme: item.SchemeName
        , 'Service Provider Name': item.TSPName
        , ClassCode: item.ClassCode
        , 'Total number of registered trainees': item.TotalNumberOfRegisteredTrainees
        , 'Total trainees present in class': item.TotalTraineesPresentInClass
      }
    })
  }
  // Report Executive Summary /// end////

  // Additional Trainees /// start////
  generateATReportExcel(data: any) {
    const dataForExport = this.populateATData(data);
    const workbook = new Workbook();
    const workSheet = workbook.addWorksheet();
    workSheet.mergeCells('A1:L6');
    const detailArray = data[0].DetailString.split(',', 4);
    workSheet.getCell('A1:L6').value = `${detailArray[0]}
${detailArray[2]}
${detailArray[3]}`;

    const titleRow = workSheet.getCell('A1:L6');
    titleRow.style = { alignment: { wrapText: true, readingOrder: 'ltr', vertical: 'top' } };
    titleRow.font = { bold: true, };
    // workSheet.mergeCells('A1:A2:A3:A4');


    workSheet.addRow([]);

    dataForExport.forEach((item, index) => {
      const keys = Object.keys(item);
      // let values = Object.entries(item).map(([key, value]) => value);
      const values = Object.values(item);

      /// SET SERIAL NUMBER
      keys.unshift('Sr#');
      values.unshift(++index)
      index--;

      if (index == 0) {
        /// SET HEADER
        const headerRow = workSheet.addRow(keys);
        headerRow.height = 25;
        headerRow.eachCell((cell, number) => {
          cell.fill = {
            type: 'pattern',
            pattern: 'solid',
            fgColor: { argb: 'FFA500' },
            // bgColor: { argb: 'cdcdcd' }
          }
          cell.border = { top: { style: 'thin' }, left: { style: 'thin' }, bottom: { style: 'thin' }, right: { style: 'thin' } }
          cell.font = { bold: true }
          cell.alignment = { vertical: 'middle', horizontal: 'center', readingOrder: 'ltr' }
        });
      };

      /// SET COLUMN VALUES
      const row = workSheet.addRow(values);
      row.height = 30;
      row.eachCell((cell, number) => {
        cell.border = { top: { style: 'thin' }, left: { style: 'thin' }, bottom: { style: 'thin' }, right: { style: 'thin' } };
        cell.alignment = { vertical: 'middle', readingOrder: 'ltr' }
        if (cell.value.toString().toLowerCase() == 'p') {
          cell.font = { name: 'Wingdings 2' }
          // cell.value = String.fromCodePoint(10003)
          cell.alignment = { vertical: 'middle', horizontal: 'center', }
        }
        workSheet.getColumn(number).width = 20;
      })

    });

    workbook.xlsx.writeBuffer().then((data) => {
      const blob = new Blob([data], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
      fs.saveAs(blob, `${this.ReportName.value}.${ExportType.XLSX}`);

    }).catch(error => {
      this.error = `${error.name} , ${error.statusText}`;
      console.error(error);
    });

  }
  populateATData(data: any) {
    return data.map((item, index) => {
      return {
        // "Sr#": ++index
        'Scheme Name': item.SchemeName,
        'Service Provider Name': item.TSPName
        , ClassCode: item.ClassCode
        , 'Trainee Name': item.Name
        , 'Father/Husband Name': item.FatherName
        , CNIC: item.Cnic
        , [item.SelectedMonth]: item.TickStatusSelectedMonth
        , [item.PreviousMonth]: item.TickStatusPreviousMonth
        , Remarks: item.Remarks
      }
    })
  }
  // Additional Trainees /// end////

  // Fake Ghost Trainee /// start////
  generateFGTReportExcel(data: any) {
    const dataForExport = this.populateFGTData(data);
    const workbook = new Workbook();
    const workSheet = workbook.addWorksheet();
    workSheet.mergeCells('A1:E2');
    const detailArray = data[0].DetailString.split(',', 4);
    workSheet.getCell('A1:C2').value = `${detailArray[0]}
${detailArray[2]}
${detailArray[3]}`;
    workSheet.getCell('H3:I3').value = `${data[0].SelectedMonth}`;
    workSheet.getCell('J3:K3').value = `${data[0].PreviousMonth}`;
    const titleRow = workSheet.getCell('A1:I2');
    titleRow.style = { alignment: { wrapText: true, readingOrder: 'ltr', vertical: 'top' } };
    titleRow.font = { bold: true, };


    workSheet.addRow([]);

    dataForExport.forEach((item, index) => {
      const keys = Object.keys(item);
      // let values = Object.entries(item).map(([key, value]) => value);
      const values = Object.values(item);

      /// SET SERIAL NUMBER
      keys.unshift('Sr#');
      values.unshift(++index)
      index--;

      if (index == 0) {
        /// SET HEADER
        const headerRow = workSheet.addRow(keys);
        headerRow.height = 25;
        headerRow.eachCell((cell, number) => {
          cell.fill = {
            type: 'pattern',
            pattern: 'solid',
            fgColor: { argb: 'FFA500' },
            // bgColor: { argb: 'cdcdcd' }
          }
          cell.border = { top: { style: 'thin' }, left: { style: 'thin' }, bottom: { style: 'thin' }, right: { style: 'thin' } }
          cell.font = { bold: true }
          cell.alignment = { vertical: 'middle', horizontal: 'center', readingOrder: 'ltr' }
        });
      };

      /// SET COLUMN VALUES
      const row = workSheet.addRow(values);
      row.height = 30;
      row.eachCell((cell, number) => {
        cell.border = { top: { style: 'thin' }, left: { style: 'thin' }, bottom: { style: 'thin' }, right: { style: 'thin' } };
        cell.alignment = { vertical: 'middle', readingOrder: 'ltr' }
        if (cell.value.toString().toLowerCase() == 'p') {
          cell.font = { name: 'Wingdings 2' }
          // cell.value = String.fromCodePoint(10003)
          cell.alignment = { vertical: 'middle', horizontal: 'center', }
        }
        workSheet.getColumn(number).width = 20;
      })

    });

    workbook.xlsx.writeBuffer().then((data) => {
      const blob = new Blob([data], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
      fs.saveAs(blob, `${this.ReportName.value}.${ExportType.XLSX}`);

    }).catch(error => {
      this.error = `${error.name} , ${error.statusText}`;
      console.error(error);
    });

  }
  populateFGTData(data: any) {
    return data.map((item, index) => {
      return {
        // "Sr#": ++index
        'Scheme Name': item.SchemeName
        , 'TSP Name': item.TSPName
        , ClassCode: item.ClassCode
        , 'Trainee Name': item.Name
        , 'Father/Husband Name': item.FatherName
        , CNIC: item.Cnic
        , 'Visit 1': item.Visit1SelectMonth
        , 'Visit 2': item.Visit2SelectMonth
        , 'Visit 1 ': item.Visit1PreviousMonth
        , 'Visit 2 ': item.Visit2PreviousMonth
      }
    })
  }
  // Fake Ghost Trainee /// end////

  //  /// Covid Mask Violation////
  generateCMVReportExcel(data: any) {
    const dataForExport = this.populateCMVData(data);
    const workbook = new Workbook();
    const workSheet = workbook.addWorksheet();
    workSheet.mergeCells('A1:I6');
    const detailArray = data[0].DetailString.split(',', 4);
    workSheet.getCell('A1:I6').value = `${detailArray[0]}
${detailArray[1]}
${detailArray[2]}
${detailArray[3]}`;

    const titleRow = workSheet.getCell('A1:I6');
    titleRow.style = { alignment: { wrapText: true, readingOrder: 'ltr', vertical: 'top' } };
    titleRow.font = { bold: true, };
    // workSheet.mergeCells('A1:A2:A3:A4');


    workSheet.addRow([]);

    dataForExport.forEach((item, index) => {
      const keys = Object.keys(item);
      // let values = Object.entries(item).map(([key, value]) => value);
      const values = Object.values(item);

      /// SET SERIAL NUMBER
      keys.unshift('Sr#');
      values.unshift(++index)
      index--;

      if (index == 0) {
        /// SET HEADER
        const headerRow = workSheet.addRow(keys);
        headerRow.height = 25;
        headerRow.eachCell((cell, number) => {
          cell.fill = {
            type: 'pattern',
            pattern: 'solid',
            fgColor: { argb: 'FFA500' },
            // bgColor: { argb: 'cdcdcd' }
          }
          cell.border = { top: { style: 'thin' }, left: { style: 'thin' }, bottom: { style: 'thin' }, right: { style: 'thin' } }
          cell.font = { bold: true }
          cell.alignment = { vertical: 'middle', horizontal: 'center', readingOrder: 'ltr' }
        });
      };

      /// SET COLUMN VALUES
      const row = workSheet.addRow(values);
      row.height = 30;
      row.eachCell((cell, number) => {
        cell.border = { top: { style: 'thin' }, left: { style: 'thin' }, bottom: { style: 'thin' }, right: { style: 'thin' } };
        cell.alignment = { vertical: 'middle', readingOrder: 'ltr' }
        if (cell.value.toString().toLowerCase() == 'p') {
          cell.font = { name: 'Wingdings 2' }
          // cell.value = String.fromCodePoint(10003)
          cell.alignment = { vertical: 'middle', horizontal: 'center', }
        }
        workSheet.getColumn(number).width = 20;
      })

    });

    workbook.xlsx.writeBuffer().then((data) => {
      const blob = new Blob([data], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
      fs.saveAs(blob, `${this.ReportName.value}.${ExportType.XLSX}`);

    }).catch(error => {
      this.error = `${error.name} , ${error.statusText}`;
      console.error(error);
    });

  }
  populateCMVData(data: any) {
    return data.map((item, index) => {
      return {
        // "Sr#": ++index
        'Service Provider Name': item.TSPName
        , ClassCode: item.ClassCode
        , 'Trainee Name': item.Name
        , 'Father/Husband Name': item.FatherName
        , CNIC: item.Cnic
        , 'Course Duration': item.Duration
        , ['Trainee Does Not Wear Mask']: item.TraineeDoesNotWearMask
        , Action: item.Action
      }

    })
  }
  // Covid Mask Violation /// end////

  //  ///Employment Verification////
  generateEVRReportExcel(data: any) {
    const dataForExport = this.populateEVRData(data);
    const workbook = new Workbook();
    const workSheet = workbook.addWorksheet();
    workSheet.mergeCells('A1:I6');
    const detailArray = data[0].DetailString.split(',', 4);
    workSheet.getCell('A1:I6').value = `${detailArray[0]}
${detailArray[2]}
${detailArray[3]}`;

    const titleRow = workSheet.getCell('A1:I6');
    titleRow.style = { alignment: { wrapText: true, readingOrder: 'ltr', vertical: 'top' } };
    titleRow.font = { bold: true, };
    // workSheet.mergeCells('A1:A2:A3:A4');


    workSheet.addRow([]);

    dataForExport.forEach((item, index) => {
      const keys = Object.keys(item);
      // let values = Object.entries(item).map(([key, value]) => value);
      const values = Object.values(item);

      /// SET SERIAL NUMBER
      keys.unshift('Sr#');
      values.unshift(++index)
      index--;

      if (index == 0) {
        /// SET HEADER
        const headerRow = workSheet.addRow(keys);
        headerRow.height = 25;
        headerRow.eachCell((cell, number) => {
          cell.fill = {
            type: 'pattern',
            pattern: 'solid',
            fgColor: { argb: 'FFA500' },
            // bgColor: { argb: 'cdcdcd' }
          }
          cell.border = { top: { style: 'thin' }, left: { style: 'thin' }, bottom: { style: 'thin' }, right: { style: 'thin' } }
          cell.font = { bold: true }
          cell.alignment = { vertical: 'middle', horizontal: 'center', readingOrder: 'ltr' }
        });
      };

      /// SET COLUMN VALUES
      const row = workSheet.addRow(values);
      row.height = 30;
      row.eachCell((cell, number) => {
        cell.border = { top: { style: 'thin' }, left: { style: 'thin' }, bottom: { style: 'thin' }, right: { style: 'thin' } };
        cell.alignment = { vertical: 'middle', readingOrder: 'ltr' }
        workSheet.getColumn(number).width = 20;
      })

    });

    workbook.xlsx.writeBuffer().then((data) => {
      const blob = new Blob([data], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
      fs.saveAs(blob, `${this.ReportName.value}.${ExportType.XLSX}`);

    }).catch(error => {
      this.error = `${error.name} , ${error.statusText}`;
      console.error(error);
    });

  }
  populateEVRData(data: any) {
    return data.map((item, index) => {
      return {
        // "Sr#": ++index
        'Class Code': item.ClassCode
        , Scheme: item.SchemeName
        , TSP: item.TSPName
        , 'Class start Date': item.ClassStartDate
        , 'Class end Date': item.ClassEndDate
        , 'Employment Commitment': item.EmploymentCommitment
        , 'Contractual Trainee': item.CompletedTrainees
        , 'Completed Trainees': item.CompletedTrainees
        , 'Employment Commitment(Trainees)': item.EmploymentCommitmentTrainees
        , 'Employment Commitment(Trainees)(Floor/ Ceiling Formula)': item.EmploymentCommitmentFloor
        , Reported: item.Reported
        , Unverified: item.Unverified
        , Verified: item.Verified
        , 'Verified to Commitment': item.VerifiedtoCommitment
      }

    })
  }

  // Employment Verification /// end////

  // getter
  get ReportName() { return this.genForm.get('ReportName') }
  get SchemeID() { return this.genForm.get('SchemeID') }
  get TSPID() { return this.genForm.get('TSPID') }
  get ClassID() { return this.genForm.get('ClassID') }
  get DateMonth() { return this.genForm.get('DateMonth') }
}
