import { filter } from 'rxjs/operators';
/* **** Aamer Rehman Malik *****/
import { Component, OnInit } from '@angular/core';
import { Workbook } from 'exceljs';
import * as fs from 'file-saver';
import * as XLSX from 'xlsx';
import { CommonSrvService } from '../../common-srv.service';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { EmploymentDailogComponent } from '../employment-dailog/employment-dailog.component';
import { trigger, transition, animate, query, stagger, style, state } from '@angular/animations';
import * as moment from 'moment';
import { Stream } from 'stream';
import { FormControl } from '@angular/forms';
import { UsersModel } from '../../master-data/users/users.component';
import { EnumUserLevel } from '../../shared/Enumerations';
import { DialogueService } from 'src/app/shared/dialogue.service';
import { ExportExcel } from '../../shared/Interfaces';
import { EnumExcelReportType } from '../../shared/Enumerations';
import { DatePipe } from '@angular/common';


@Component({
  selector: 'app-onjob-trainee-placement',
  templateUrl: './onjob-trainee-placement.component.html',
  styleUrls: ['./onjob-trainee-placement.component.scss'],
  providers: [DatePipe],
  animations: [
    trigger("listAnimation", [
      transition("* => *", [
        // each time the binding value changes
        query(
          ":leave",
          [stagger(100, [animate("0.5s", style({ opacity: 0 }))])],
          { optional: true }
        ),
        query(
          ":enter",
          [
            style({ opacity: 0 }),
            stagger(100, [animate("0.5s", style({ opacity: 1 }))])
          ],
          { optional: true }
        )
      ])
    ]),
    trigger(
      'enterAnimation', [
      transition(':enter', [
        style({ transform: 'translateX(100%)', opacity: 0 }),
        animate('500ms', style({ transform: 'translateX(0)', opacity: 1, 'overflow-x': 'hidden' }))
      ]),
      transition(':leave', [
        style({ transform: 'translateX(0)', opacity: 1 }),
        animate('500ms', style({ transform: 'translateX(100%)', opacity: 0 }))
      ])
    ]
    ),
    trigger('slideIn', [
      state('*', style({ 'overflow-y': 'hidden' })),
      state('void', style({ 'overflow-y': 'hidden' })),
      transition('* => void', [
        style({ height: '*' }),
        animate(250, style({ height: 0 }))
      ]),
      transition('void => *', [
        style({ height: '0' }),
        animate(250, style({ height: '*' }))
      ])
    ])
  ]
})

export class OnjobTraineePlacementComponent implements OnInit {
  TSPDetail = [];
  classesArray: any[];

  Scheme: any[];
  ClassList: any=[];
  //TraineeList: [];
  ApprovalData: any;
  ReportedEmploymentList: any;
  VerifiedEmploymentList: any;
  error: any;
  working: boolean;
  filters: IClassListFilter = { SchemeID: 0, ClassID: 0, TSPID: 0, UserID: 0 };
  SearchSchemeList = new FormControl('',);
  SearchTSPList = new FormControl('',);
  SearchClassList = new FormControl('',);
  currentUser: UsersModel;
  userid: number;
  enumUserLevel = EnumUserLevel;

  constructor(private ComSrv: CommonSrvService, private dialog: MatDialog, public dialogueService: DialogueService, private _date: DatePipe) { }

  ngOnInit(): void {
    this.currentUser = this.ComSrv.getUserDetails();
    if (this.currentUser.UserLevel == this.enumUserLevel.TSP) {
      this.ComSrv.setTitle("On Job Trainee");
    }
    else {
      this.ComSrv.setTitle("Employment");
    }
    this.userid = this.currentUser.UserID;
    this.GetClasses();
  }

  EmptyCtrl() {
    this.SearchClassList.setValue('');
    this.SearchTSPList.setValue('');
    this.SearchSchemeList.setValue('');
  }

  getTSPDetailByScheme(schemeId: number) {
    this.classesArray = [];
    this.ComSrv.getJSON(`api/TSPDetail/GetTSPDetailByScheme/` + schemeId)
      .subscribe(data => {
        this.TSPDetail = <any[]>data;
      }, error => {
        this.error = error;
      })
  }
  getClassesByTsp(tspId: number) {
    this.ComSrv.getJSON(`api/Class/GetClassesByTsp/` + tspId)
      .subscribe(data => {
        this.classesArray = <any[]>data;
      }, error => {
        this.error = error;
      })
  }

  getClassesBySchemes(schemeId: number) {
    this.ComSrv.postJSON(`api/Class/FetchClassesByUser/`, { UserID: this.userid, OID: this.ComSrv.OID.value, SchemeID: schemeId })
      .subscribe(data => {
        this.classesArray = <any[]>data;
      }, error => {
        this.error = error;
      })
  }


classData:any=[]
  GetClasses() {
    if (this.currentUser.UserLevel == EnumUserLevel.TSP) {
      this.ComSrv.getJSON(`api/TSPEmployment/GetFilteredClass/filter?filter=${this.filters.SchemeID}&filter=${this.filters.TSPID}&filter=${this.filters.ClassID}&filter=${this.userid}&filter=${this.ComSrv.OID.value}`).subscribe(
        (d: any) => {
          this.ClassList = d[0]
          this.classData = d[0];

          if (d[0]?.length > 0) {
            const startIndex = 0;
            const endIndex = Math.min(this.PAGE_SIZE, d[0].length);
            this.ClassList = d[0].slice(startIndex, endIndex);
            this.currentPage = 0;
            this.totalPages = Math.ceil(d[0].length / this.PAGE_SIZE);
          }
          this.Scheme = d[1];
          console.log(d);
        }
        , (error) => {
          console.error(JSON.stringify(error));
        }
      );
    }
    else {
      this.ComSrv.getJSON(`api/TSPEmployment/GetFilteredClass/filter?filter=${this.filters.SchemeID}&filter=${this.filters.TSPID}&filter=${this.filters.ClassID}&filter=${0}&filter=${this.ComSrv.OID.value}`).subscribe(
        (d: any) => {
          this.ClassList = d[0];
          this.Scheme = d[1];
          console.log(d);
        }
        , (error) => {
          console.error(JSON.stringify(error));
        }
      );
    }
  }


  PAGE_SIZE: number = 5;
  currentPage: number = 0;
  totalPages: number = 0;
  Math = Math;
  
  loadData(param: string = 'next'): void {
    if (!this.classData?.length) {
      this.ClassList = [];
      return;
    }
  
    this.totalPages = Math.ceil(this.classData.length / this.PAGE_SIZE);
  
    // Handle pagination direction
    if (param === 'next' && this.currentPage < this.totalPages - 1) {
      this.currentPage++;
    } else if (param === 'pre' && this.currentPage > 0) {
      this.currentPage--;
    }
  
    // Calculate indices
    const startIndex = this.currentPage * this.PAGE_SIZE;
    const endIndex = Math.min(startIndex + this.PAGE_SIZE, this.classData.length);
  
    // Update displayed data
    this.ClassList = this.classData.slice(startIndex, endIndex);
  }


  GetTraineeOfClass(ClassID: any, r: any) {
    r.HasTrainees = !r.HasTrainees;
    if (r.Trainees) {
      return;
    }
    this.ComSrv.getJSON("api/TSPEmployment/GetCompletedTraineesOfClassForEmployment/" + ClassID).subscribe(
      (d: any) => {
        r.Trainees = d.TraineeList;
        r.HasTrainees = true;
        console.log(d);
      }
      , (error) => {
        console.error(error);
      }
    );
  }
  SubmitClassData(ClassID: any, r: any) {
    debugger;
    this.ComSrv.getJSON("api/TSPEmployment/GetEmploymentReportedTraineesOfClass/" + ClassID).subscribe(
      (d: any) => {
        let Trainees: [] = d.TraineeList;
        let PlacementData: [] = d.PlacementData;
        let Dif = Trainees.length - PlacementData.length;
        if (Dif > 0) {
          this.ComSrv.confirmTrinee('Class Employment submit Confirmation', Dif > 0 ? Dif + ' Trainee data not saved.' : 'Are you sure to submit?').subscribe((res) => {

          });
        }
        else {
          this.ComSrv.confirm('Class Employment submit Confirmation', Dif > 0 ? Dif + ' Trainee data not saved Are you sure to submit?' : 'Are you sure to submit?').subscribe((res) => {
            if (res == true) {
              this.ComSrv.postJSON("api/TSPEmployment/SubmitClassEmployment", { 'ClassID': ClassID }).subscribe((d: any) => {
                this.ComSrv.openSnackBar("Class Employment submited successfuly.");
              });
            }
          });

        }

      }
      , (error) => {
        console.error(error);
      }
    );
  }
  GetTrainee(ClassID: any, ClassCode: string) {
    //if (r.Trainees) {
    //  r.Trainees = null;
    //  return;
    //}
    this.ComSrv.getJSON("api/TSPEmployment/GetTraineeOfClass/" + ClassID).subscribe(
      (d: any) => {
        let PlacementStatus = ['Employed', 'Unemployed', 'Not Submitted', 'Not Interested'];// d.PlacementStatus.map((o) => { return o.EmploymentStatusName; });
        let PlacementTypes: [] = d.PlacementTypes.map((o) => { return o.PlacementType; });
        let District: [] = d.District.map((o) => { return o.DistrictName; });
        let Tehsil: [] = d.Tehsil.map((o) => { return o.TehsilName; });
        let VerificationMethods: [] = d.VerificationMethods.map((o) => { return o.VerificationMethodType; });

        let Trainees = d.TraineeList.map((o) => {
          var el = {
            //'Trainee ID': o.TraineeID, //A    
            'Trainee Code': o.TraineeCode,//B    //A          //A -- Later alphatical headers are given to remove TraineeID and ClassID columns
            'Trainee Name': o.TraineeName,//C     //B         //B
            'Trainee Father Name': o.FatherName,//D    //C    //C
            'Trainee CNIC': o.TraineeCNIC,//E   //D           //D
            'Trainee Contact Number': o.ContactNumber,//E   //D           //E
            //'ClassID': o.ClassID,         //F            
            'Class Code': ClassCode,      //G    //E          //F 
            'Employment Status': '',      //H    //F          //G
            'Designation': '',            //I    //G          //H
            'Department': '',             //J    //H          //I
            'Employment Duration': 0,    //K     //I          //J
            'Salary': 0,                  //L    //J          //K
            'Supervisor Name': '',        //M    /K           //L
            'Supervisor Contact': '',     //N    //L          //M
            'Employment Start Date': '',  //O    //M          //N
            'Employer Name': '',          //P    //N          //O
            'Employer Business Type': '', //Q    //O          //P
            'Employment Address': '',     //R    //P          //Q
            'District': '',               //S    //Q          //R
            'Employment Tehsil': '',      //T    //R          //S
            'Time From': '0',             //U    //S          //T
            'Time To': '0',               //V    //T          //U
            'Office Contact No': '',      //W    //U          //V 
            'Placement Type': '',         //X    //V          //W
            'Verification Method': '',    //Y    //W          //X 
            'EOBI': '',
            'EmployerNTN': '',//Z    //X          //Y
            //'Trainee ID': o.TraineeID, //Z    
          };
          // = Object.assign({}, el);            
          return el;
        });

        //  const wbb: XLSX.WorkBook = XLSX.utils.book_new();
        //  const wss: XLSX.WorkSheet = XLSX.utils.json_to_sheet(Trainees);
        //  XLSX.utils.book_append_sheet(wbb, wss, 'PlacementForm');
        //XLSX.writeFile(wbb, 'Fact Sheet.xlsx');
        let wb = new Workbook();

        const ws = wb.addWorksheet('PlacementForm');
        let font = {
          name: 'Calibri',
          family: 4,
          size: 11,
          //underline: true,
          bold: true
        };
        let Combined = [...PlacementStatus, ...PlacementTypes, ...District, ...Tehsil, ...VerificationMethods];
        const hidden = wb.addWorksheet("hidden");
        for (var i = 0, length = Combined.length; i < length; i++) {
          let name = Combined[i];
          let row = hidden.addRow([name]);
        }
        hidden.state = 'hidden';
        ws.properties.defaultRowHeight = 18;
        //ws.addRow({ id: 1, name: 'John Doe', dob: new Date(1970, 1, 1) });
        let headerRow = ws.addRow(Object.keys(Trainees[0]));
        headerRow.eachCell((cell, number) => {
          cell.fill = {
            type: 'pattern',
            pattern: 'solid',
            fgColor: { argb: 'FFFFFF00' },
            bgColor: { argb: '80C0C0C0' }
          };
          cell.border = { top: { style: 'thin' }, left: { style: 'thin' }, bottom: { style: 'thin' }, right: { style: 'thin' } };
        });
        //ws.addRows(Object.values(Trainees));
        Trainees.forEach((el) => {
          let r = ws.addRow(Object.values(el));
          r.getCell('T').numFmt = "h:mm AM/PM";
          r.getCell('U').numFmt = "h:mm AM/PM";
          r.getCell('N').numFmt = "dd/MM/yyyy";
          r.getCell('M').numFmt = "0##########";
          r.getCell('V').numFmt = "0##########";

          r.getCell('G').dataValidation = {
            type: 'list',
            allowBlank: false,
            formulae: ["hidden!$A$1:$A$" + PlacementStatus.length],
            error: 'Select From List'
          };
          r.getCell('W').dataValidation = {
            type: 'list',
            allowBlank: false,
            formulae: ["hidden!$A$" + (PlacementStatus.length + 1) + ":$A$" + (PlacementStatus.length + PlacementTypes.length)],
            error: 'Select From List'
          };
          r.getCell('R').dataValidation = {
            type: 'list',
            allowBlank: false,
            formulae: ["hidden!$A$" + (PlacementStatus.length + PlacementTypes.length + 1) + ":$A$" + (PlacementStatus.length + PlacementTypes.length + District.length)],
            error: 'Select From List'
          };
          r.getCell('S').dataValidation = {
            type: 'list',
            allowBlank: false,
            formulae: ["hidden!$A$" + (PlacementStatus.length + PlacementTypes.length + District.length + 1) + ":$A$" + (PlacementStatus.length + PlacementTypes.length + District.length + Tehsil.length)],
            error: 'Select From List'
          };
          r.getCell('X').dataValidation = {
            type: 'list',
            allowBlank: false,
            formulae: ["hidden!$A$" + (PlacementStatus.length + PlacementTypes.length + District.length + Tehsil.length + 1) + ":$A$" + (PlacementStatus.length + PlacementTypes.length + District.length + Tehsil.length + VerificationMethods.length)],
            error: 'Select From List'
          };
          r.commit();
        });
        //let Z = ws.getColumn(26);
        //Z.style.protection = {
        //  locked: true
        //};
        //Z.hidden = true;
        //Z.style.protection {

        //}
        let F = ws.getColumn(6);
        F.style.protection = {
          locked: true
        };
        F.hidden = true;

        let RowH = ws.getRow(1);
        //RowH.fill == {
        //  type: 'pattern',
        //  pattern: 'solid',
        //  fgColor: { argb: 'FFFFFF00' },
        //  bgColor: { argb: '80C0C0C0' }
        //};
        RowH.font = font;

        wb.xlsx.writeBuffer().then((data) => {
          let blob = new Blob([data], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
          fs.saveAs(blob, 'Placement For Class ' + ClassCode + '.XLSX');
          wb = undefined;
        });

        console.log(Trainees);
      }

      , (error) => {
        console.error(error);
      }
    );
  }
  isNumber = (n: string | number): boolean =>
    !isNaN(parseFloat(String(n))) && isFinite(Number(n));
  onFileChange(ev: any, row: any) {
    let workBook = null;
    let jsonData = null;
    const reader = new FileReader();
    const file = ev.target.files[0];
    reader.onload = (event) => {
      const data = reader.result;
      workBook = XLSX.read(data, { type: 'binary' });
      var workbookX = new Workbook();
      //workbookX.xlsx.load(data)
      //  .then(function () {
      //    var worksheet = workbookX.getWorksheet('PlacementForm');
      //    worksheet.
      //    //worksheet.eachRow({ includeEmpty: true }, function (row, rowNumber) {
      //    //  console.log("Row " + rowNumber + " = " + JSON.stringify(row.values));
      //    //});
      //  });

      jsonData = workBook.SheetNames.reduce((initial, name) => {
        const sheet = workBook.Sheets[name];

        initial[name] = XLSX.utils.sheet_to_json(sheet, { raw: false });
        return initial;
      }, {});

      const dataString = JSON.stringify(jsonData);
      let dataObj = JSON.parse(dataString)?.PlacementForm;
      if (!dataObj) {
        this.ComSrv.ShowError("Invalid Excel file.");
        return;
      }

      if (row.EmploymentSubmited) {
        this.ComSrv.ShowError("Employemnt already submitted for this class");
        return;
      }

      var classCodeofFile = dataObj[0]["Class Code"];
      if (classCodeofFile === row.ClassCode) {
        dataObj = this.ComSrv.RemSpaceFields(dataObj ?? []);
        for (let i of dataObj) {
          //dataObj[i]["EmploymentStartDate"] = new Date(dataObj[i]["EmploymentStartDate"]);
          i.EmploymentStartDate = moment(i.EmploymentStartDate, 'DD/MM/YYYY').format('MM/DD/YYYY');
          //i.TimeFrom = new Date(i.TimeFrom);
          //i.TimeTo = new Date(i.TimeTo);
          i.Salary = this.isNumber(i.Salary) ? i.Salary : null
          i.EmploymentDuration = this.isNumber(i.EmploymentDuration) ? i.EmploymentDuration : null
          i.ClassID = row.ClassID;
        }
        //  const finalObj = JSON.stringify(dataObj);
        let dialogRef: MatDialogRef<EmploymentDailogComponent>;

        dialogRef = this.dialog.open(EmploymentDailogComponent, {
          //data: dataObj,
          data: { 'data': dataObj, 'EndDate': row.EndDate },
          minWidth: '90%',
          height: '95%',
          disableClose: true
        });

      }
      else {
        this.ComSrv.ShowError("Please upload file with same Class Code");
        return;
      }
      //dataObj = this.ComSrv.RemSpaceFields(dataObj ?? []);
      //for (let i of dataObj) {
      //  //dataObj[i]["EmploymentStartDate"] = new Date(dataObj[i]["EmploymentStartDate"]);
      //  i.EmploymentStartDate = moment(i.EmploymentStartDate, 'DD/MM/YYYY').format('MM/DD/YYYY');
      //  //i.TimeFrom = new Date(i.TimeFrom);
      //  //i.TimeTo = new Date(i.TimeTo);
      //  i.Salary = this.isNumber(i.Salary) ? i.Salary: null
      //  i.EmploymentDuration = this.isNumber(i.EmploymentDuration) ? i.EmploymentDuration: null
      //  i.ClassID = row.ClassID;
      //}
      ////  const finalObj = JSON.stringify(dataObj);
      //let dialogRef: MatDialogRef<EmploymentDailogComponent>;

      //dialogRef = this.dialog.open(EmploymentDailogComponent, {
      //  data: dataObj,
      //  minWidth: '90%',
      //  height: '95%',
      //  disableClose: true
      //});
    };
    //  this.ComSrv.postJSON('api/TSPEmployment/SaveExcelFile', finalObj)// { 'json': JSON.stringify(dataObj) })
    //    .subscribe((d: any) => {
    //      //this.ComSrv.openSnackBar(row.InActive == true ? environment.InActiveMSG.replace("${Name}", this.EnText) : environment.ActiveMSG.replace("${Name}", this.EnText));
    //      // this.placementforme =new MatTableDataSource(d);
    //    },
    //      error => this.ComSrv.ShowError(this.error)); // error path

    //  //this.PopulateFieldsFromFile(dataObj);
    //};
    reader.readAsBinaryString(file);
    ev.target.value = '';
  }

  openClassJourneyDialogue(data: any): void {
    debugger;
    this.dialogueService.openClassJourneyDialogue(data);
  }

  getDependantFilters() {
    if (this.currentUser.UserLevel == this.enumUserLevel.TSP) {
      this.getClassesBySchemes(this.filters.SchemeID);
    }
    else {
      this.getTSPDetailByScheme(this.filters.SchemeID);
    }
  }

  exportToExcelReportedEmploymentData() {
    this.ComSrv.postJSON('api/TSPEmployment/ReportedEmploymentExportExcel', this.filters).subscribe(
      (d: any) => {
        this.ReportedEmploymentList = d;
        this.exportToExcelReportedEmployment();
      }
      , (error) => {
        console.error(error);
      }
    );
  }

  exportToExcelVerifiedEmploymentData() {
    this.ComSrv.postJSON('api/TSPEmployment/VerifiedEmploymentExportExcel', this.filters).subscribe(
      (d: any) => {
        this.VerifiedEmploymentList = d;
        this.exportToExcelVerifiedEmployment();
      }
      , (error) => {
        console.error(error);
      }
    );
  }

  exportToExcelReportedEmployment() {
    let filteredData = [...this.ReportedEmploymentList]
    let data = {
    };

    let exportExcel: ExportExcel = {
      Title: 'Reported Employment Report',
      Author: '',
      Type: EnumExcelReportType.ReportedEmployment,
      Data: data,
      List1: this.populateDataReportedEmployment(filteredData),
    };
    this.dialogueService.openExportConfirmDialogue(exportExcel).subscribe();
  }

  exportToExcelVerifiedEmployment() {
    let filteredData = [...this.VerifiedEmploymentList]
    let data = {
    };

    let exportExcel: ExportExcel = {
      Title: 'Verified Employment Report',
      Author: '',
      Type: EnumExcelReportType.VerifiedEmployment,
      Data: data,
      List1: this.populateDataVerifiedEmployment(filteredData),
    };
    this.dialogueService.openExportConfirmDialogue(exportExcel).subscribe();
  }

  populateDataReportedEmployment(data: any) {
    return data.map(item => {
      return {
        "Scheme": item.SchemeName
        , "Trade Name": item.TradeName
        , "TSP Name": item.TSPName
        , "Class Code": item.ClassCode
        , "Class Start Date": this._date.transform(item.StartDate, 'MM/dd/yyyy')
        , "Class End Date": this._date.transform(item.EndDate, 'MM/dd/yyyy')
        , "TraineeID": item.TraineeCode
        , "Trainee CNIC": item.TraineeCNIC
        , "Trainee Contact Number": item.ContactNumber
        , "Designation": item.Designation
        , "Department": item.Department
        , "Employer Name": item.EmployerName
        //, "Employer Business Type": item.EmployerBusinessType
        , "Employment Address": item.EmploymentAddress
        , "Supervisor Name": item.SupervisorName
        , "SupervisorContactNumber": item.SupervisorContact
        , "PlacementType": item.PlacementType
        , "Verification Source": item.VerificationMethodType
        , "Document Verification": item.DEOVerification
        , "Telephonic Verification": item.TelephonicVerification
        , "Final Verification": item.FinalVerification
        , "Employment Status": item.EmploymentStatus
        , "Comments": item.Comments

        //, "Permanent Address": item.PermanentAddress
        //, "Permanent District": item.PermanentDistrict
        //, "Permanent Tehsil": item.PermanentTehsil
      }
    })
  }

  populateDataVerifiedEmployment(data: any) {
    return data.map(item => {
      return {
        "Class Code": item.ClassCode
        , "Scheme": item.SchemeName
        , "TSP": item.TSPName
        , "Class Status": item.ClassStatusName
        //, "Verification Source": item.VerificationMethodType
        //, "Completion Date": this._date.transform(item.StartDate, 'MM/dd/yyyy')
        , "Completion Date": this._date.transform(item.EndDate, 'MM/dd/yyyy')
        , "Contractual Trainees Per Class": item.TraineesPerClass
        , "Completed Trainees": item.CompletedTrainees
        , "Employment Commitment in percentage": item.OverallEmploymentCommitment
        , "Employment Commitment Trainees": item.EmploymentCommittedTrainees
        , "Employment Reported": item.EmploymentReported
        , "Verified": item.VerifiedEmployment
        , "Verified to Commitment": item.VerifiedToContractualCommitment + '%'
        , "Source Of Verification": item.SourceOfVerification
        , "KAM": item.KAM
        //, "Employment Address": item.EmploymentAddress
        //, "Supervisor Name": item.SupervisorName
        //, "SupervisorContactNumber": item.SupervisorContact

      }
    })
  }

}

export interface IClassListFilter {
  SchemeID: number;
  TSPID: number;
  ClassID: number;
  UserID: number;
}
