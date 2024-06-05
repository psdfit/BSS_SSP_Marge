import { Component, OnInit, ViewChild } from '@angular/core';
//import { IEmpVarificationFilter } from '../Interface/IVarificationFilter';
import { CommonSrvService } from '../../common-srv.service';
import { DialogueService } from '../../shared/dialogue.service';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { UsersModel } from '../../master-data/users/users.component';
import { MatTableDataSource } from '@angular/material/table';
import { SelectionModel } from '@angular/cdk/collections';
import { PSPEmploymentDialogComponent } from '../psp-employment-dialog/psp-employment-dialog.component';
import { Workbook } from 'exceljs';
import * as fs from 'file-saver';
//import { EmpVerificationComponent } from '../employment-verification/employment-verification.component';
//import { DEOEmploymentVerificationDialogComponent } from '../deo-employment-verification-dialog/deo-employment-verification-dialog.component';
import * as moment from 'moment';
import * as XLSX from 'xlsx';

@Component({
  selector: 'app-psp-assigned-trainees-list',
  templateUrl: './psp-assigned-trainees-list.component.html',
  styleUrls: ['./psp-assigned-trainees-list.component.scss']
})
export class PSPAssignedTraineesListComponent implements OnInit {

  specificTraineeListForVerification: any[];
  placementTypes: any[];
  placementTypeID: any;
  verificationMethods: any[];
  verificationMethodsDrp: any[];
  verificationMethodID: any;
  ClassList: [];
  PSPAssignedTraineesList: MatTableDataSource<any>;
  TraineeList: [];
  BatchTraineeList: [];
  pspbatches: [];
  ApprovalData: any;
  error: any;
  working: boolean;
  //filters: IEmpVarificationFilter = { ClassID: 0, PlacementTypeID: 2, VerificationMethodID: 7};
  pspfilters: IPSPAssignedTraineeListFilter = { PSPBatchID: 0};
  EmploymentTypeID: number;
  currentUser: UsersModel;
  userid: number;
  TraineeIDsForExport: string;
  displayedColumns = ['select','TraineeName', 'FatherName', 'TraineeCNIC',
    'ContactNumber','Batch', 'Action'
  ];

  selection = new SelectionModel<any>(true, []);

  update: String;
  query = {
    order: 'PSPTraineeID',
    limit: 5,
    page: 1
  };

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  constructor(private http: CommonSrvService, private dialogue: DialogueService, public dialog: MatDialog) {
    this.PSPAssignedTraineesList = new MatTableDataSource([]);

  }

  ngOnInit(): void {
    this.http.setTitle("Employment By PSP");

    this.currentUser = this.http.getUserDetails();
    this.userid = this.currentUser.UserID;
    this.GetBatches();
    this.GetPSPAssignedTraineesByID();

  }


  isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.PSPAssignedTraineesList.data.length;
    return numSelected === numRows;
  }

  masterToggle() {
    this.isAllSelected() ?
      this.selection.clear() :
      this.PSPAssignedTraineesList.data.forEach(row => this.selection.select(row));
  }


  GetPSPAssignedTraineesByID() {
    this.http.postJSON(`api/PSPEmployment/PSPAssignedTraineesByID`, { 'PSPUserID': this.userid, 'PSPBatchID': this.pspfilters.PSPBatchID }).subscribe(
      (d: any) => {
        this.PSPAssignedTraineesList = new MatTableDataSource(d[0]);
        //console.log(d);
        this.PSPAssignedTraineesList.paginator = this.paginator;
        this.PSPAssignedTraineesList.sort = this.sort;
      }
      , (error) => {
        console.error(JSON.stringify(error));
      }
    );
  }

  GetBatches() {
    this.http.getJSON(`api/PSPEmployment/GetPSPBatches`).subscribe(
      (d: any) => {
        this.pspbatches = d[0];
        console.log(d);
      }
      , (error) => {
        console.error(JSON.stringify(error));
      }
    );
  }

  GetPSPBatchTraineeList() {
    this.http.getJSON(`api/PSPEmployment/GetPSPBatchTraineeList?pspBatchId=${this.pspfilters.PSPBatchID}`).subscribe(
      (d: any) => {
        this.BatchTraineeList = d;
      }
      , (error) => {
        console.error(error);
      }
    );
  }

  ExportSelectedTraineesList() {
    this.TraineeIDsForExport = this.selection.selected.map(x => x.TraineeID).join(',');
    if (this.TraineeIDsForExport == '') {
      this.TraineeIDsForExport = this.PSPAssignedTraineesList.filteredData.map(x => x.TraineeID).join(',');
    }
    else {
      console.log(this.TraineeIDsForExport);
    }
    //return;
    this.http.postJSON("api/PSPEmployment/GetPSPBatchTraineesForExport", this.TraineeIDsForExport).subscribe(
      (d: any) => {
        let PlacementStatus = ['Employed', 'Unemployed', 'Not Submitted', 'Not Interested'];// d.PlacementStatus.map((o) => { return o.EmploymentStatusName; });
        let PlacementTypes: [] = d.PlacementTypes.map((o) => { return o.PlacementType; });
        let District: [] = d.District.map((o) => { return o.DistrictName; });
        let Tehsil: [] = d.Tehsil.map((o) => { return o.TehsilName; });
        let VerificationMethods: [] = d.VerificationMethods.map((o) => { return o.VerificationMethodType; });

        let Trainees = d.TraineeList.map((o) => {
          var el = {
            //'Trainee ID': o.TraineeID, //A    
            'Trainee Code': o.TraineeCode,//B    //A        -- Later alphatical headers are given to remove TraineeID and ClassID columns
            'Trainee Name': o.TraineeName,//C     //B
            'Trainee Father Name': o.FatherName,//D    //C
            'Trainee CNIC': o.TraineeCNIC,//E   //D
            //'ClassID': o.ClassID,         //F
            'Batch Name': o.BatchName,      //G    //E
            'Employment Status': '',      //H    //F
            'Designation': '',            //I    //G
            'Department': '',             //J    //H
            'Employment Duration': 0,    //K     //I
            'Salary': 0,                  //L    //J
            'Supervisor Name': '',        //M    /K
            'Supervisor Contact': '',     //N    //L
            'Employment Start Date': '',  //O    //M
            'Employer Name': '',          //P    //N
            'Employer Business Type': '', //Q    //O
            'Employment Address': '',     //R    //P
            'District': '',               //S    //Q
            'Employment Tehsil': '',      //T    //R
            'Time From': '0',             //U    //S
            'Time To': '0',               //V    //T
            'Office Contact No': '',      //W    //U
            'Placement Type': '',         //X    //V
            'Verification Method': '',    //Y    //W
            'EOBI': ''                    //Z    //X
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
          r.getCell('S').numFmt = "h:mm AM/PM";
          r.getCell('T').numFmt = "h:mm AM/PM";
          r.getCell('M').numFmt = "dd/MM/yyyy";
          r.getCell('L').numFmt = "0##########";
          r.getCell('U').numFmt = "0##########";

          r.getCell('F').dataValidation = {
            type: 'list',
            allowBlank: false,
            formulae: ["hidden!$A$1:$A$" + PlacementStatus.length],
            error: 'Select From List'
          };
          r.getCell('V').dataValidation = {
            type: 'list',
            allowBlank: false,
            formulae: ["hidden!$A$" + (PlacementStatus.length + 1) + ":$A$" + (PlacementStatus.length + PlacementTypes.length)],
            error: 'Select From List'
          };
          r.getCell('Q').dataValidation = {
            type: 'list',
            allowBlank: false,
            formulae: ["hidden!$A$" + (PlacementStatus.length + PlacementTypes.length + 1) + ":$A$" + (PlacementStatus.length + PlacementTypes.length + District.length)],
            error: 'Select From List'
          };
          r.getCell('R').dataValidation = {
            type: 'list',
            allowBlank: false,
            formulae: ["hidden!$A$" + (PlacementStatus.length + PlacementTypes.length + District.length + 1) + ":$A$" + (PlacementStatus.length + PlacementTypes.length + District.length + Tehsil.length)],
            error: 'Select From List'
          };
          r.getCell('W').dataValidation = {
            type: 'list',
            allowBlank: false,
            formulae: ["hidden!$A$" + (PlacementStatus.length + PlacementTypes.length + District.length + Tehsil.length + 1) + ":$A$" + (PlacementStatus.length + PlacementTypes.length + District.length + Tehsil.length + VerificationMethods.length)],
            error: 'Select From List'
          };
          r.commit();
        });
        let A = ws.getColumn(1);
        A.style.protection = {
          locked: true
        };
        A.hidden = true;
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
          fs.saveAs(blob, 'Placement For Class ' + '.XLSX');
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

  onFileChange(ev: any) {
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
        this.http.ShowError("Invalid Excel file.");
        return;
      }
      dataObj = this.http.RemSpaceFields(dataObj ?? []);
      for (let i of dataObj) {
        //dataObj[i]["EmploymentStartDate"] = new Date(dataObj[i]["EmploymentStartDate"]);
        i.EmploymentStartDate = moment(i.EmploymentStartDate, 'DD/MM/YYYY').format('MM/DD/YYYY');
        //i.TimeFrom = new Date(i.TimeFrom);
        //i.TimeTo = new Date(i.TimeTo);
        i.Salary = this.isNumber(i.Salary) ? i.Salary : null
        i.EmploymentDuration = this.isNumber(i.EmploymentDuration) ? i.EmploymentDuration : null
       // i.ClassID = ClassID;
      }
      //  const finalObj = JSON.stringify(dataObj);
      let dialogRef: MatDialogRef<PSPEmploymentDialogComponent>;

      dialogRef = this.dialog.open(PSPEmploymentDialogComponent, {
        data: dataObj,
        minWidth: '90%',
        height: '95%',
        disableClose: true
      });
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




  SubmitPSPemploymentList() {
    var list = this.BatchTraineeList;

    this.http.postJSON("api/PSPEmployment/SavePSPBatchTrainees", this.BatchTraineeList).subscribe(
      (d: any) => {
        //this.TraineeList = d.FormalList;
        //this.GetSelfEmploymentList();
        console.log(d);
        if (d) {
          this.update = "Trainee Status Updated Successfully";
          this.http.openSnackBar(this.update.toString(), "Updated"); 
        }
      }
      , (error) => {
        console.error(error);
      }
    );
  }

  openEmploymentDialog(): void {
    const dialogRef = this.dialog.open(PSPEmploymentDialogComponent, {
      //minWidth: '1000px',
      //minHeight: '600px',
      minWidth: '90%',
      height: '95%',

      //data: JSON.parse(JSON.stringify(row))
      data: this.PSPAssignedTraineesList
      //data: this.selection.selected
      //this.GetVisitPlanData(data)
    })
    dialogRef.afterClosed().subscribe(result => {
      //console.log(result);
      //this.visitPlan = result;
      //this.submitVisitPlan(result);
    })
  }




}


export interface IPSPAssignedTraineeListFilter {
  PSPBatchID: number
}
