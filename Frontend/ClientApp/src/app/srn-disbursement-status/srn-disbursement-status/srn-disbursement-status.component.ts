import { Component, OnInit } from '@angular/core';
import { ViewChild, ElementRef } from '@angular/core';
import { CommonSrvService } from '../../common-srv.service';
import { FormControl } from '@angular/forms';

import { Element } from '@angular/compiler/src/render3/r3_ast';
import { MatProgressBar } from '@angular/material/progress-bar';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { FormGroup, FormBuilder, Validators, FormGroupDirective } from '@angular/forms';
import { environment } from '../../../environments/environment';
import { ModelBase } from '../../shared/ModelBase';
import { UserRightsModel, UsersModel } from '../../master-data/users/users.component';
import { ActivatedRoute } from '@angular/router';
import * as moment from 'moment';
import { Moment } from 'moment';
import { MatDatepicker } from '@angular/material/datepicker';
import * as fs from 'file-saver';

import { DialogueService } from 'src/app/shared/dialogue.service';
import { DatePipe } from '@angular/common';
import { DateAdapter, MAT_DATE_LOCALE, MAT_DATE_FORMATS } from '@angular/material/core';
import { MomentDateAdapter, MAT_MOMENT_DATE_ADAPTER_OPTIONS } from '@angular/material-moment-adapter';
import * as XLSX from 'xlsx';
import { ExportExcel } from '../../shared/Interfaces';
import { ExportType,EnumExcelReportType } from '../../shared/Enumerations';
import { Workbook } from 'exceljs';



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
  selector: 'app-srn-disbursement-status',
  templateUrl: './srn-disbursement-status.component.html',
  styleUrls: ['./srn-disbursement-status.component.scss'],
  providers: [
    // `MomentDateAdapter` can be automatically provided by importing `MomentDateModule` in your
    // application's root module. We provide it at the component level here, due to limitations of
    // our example generation script.
    DatePipe,
    {
      provide: DateAdapter,
      useClass: MomentDateAdapter,
      deps: [MAT_DATE_LOCALE, MAT_MOMENT_DATE_ADAPTER_OPTIONS],

    },

    { provide: MAT_DATE_FORMATS, useValue: MY_FORMATS },
  ],
})
export class SrnDisbursementStatusComponent implements OnInit {
  environment = environment;
  srnDisbursementTrainees: MatTableDataSource<any>;
  error: string;
  update: string;
  month = new FormControl(moment());
  selectedTrainees: any;
  disbursementUpdatedTrainees: any;
  data: any;
  currentUser: UsersModel;

  Schemes: any[];
  Classes: any[];
  TSPDetail : any[];


  filters: IQueryFilters = { SchemeID: 0, TSPID: 0, ClassID: 0 };
  tk: any;

  displayedColumnsTrainees = ['SchemeName','TSPName','TradeName','ClassCode',
    //'TradeID',
    'TraineeName', 'TraineeCode', 'FatherName', 'TraineeCNIC', 'DistrictName', 'ContactNumber', 'Comments', 'Amount',
    'Batch',
    'TokenNumber', 'TransactionNumber', 'Redeem', 'Month','NumberOfMonths'
  ];

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  constructor(private fb: FormBuilder, private ComSrv: CommonSrvService, public dialogueService: DialogueService, private _date: DatePipe) { }

  ngOnInit(): void {
    this.ComSrv.setTitle("Stipend Disbursement Status");
    this.currentUser = this.ComSrv.getUserDetails();

    this.getSchemes();
    this.getSRNDisbursementTraineeData();
  }

  SearchSch = new FormControl('');
  SearchCls = new FormControl('');
  SearchTSP = new FormControl('');

  schemeFilter = new FormControl(0);
  tspFilter = new FormControl(0);
  classFilter = new FormControl(0);

  getSRNDisbursementTraineeData() {
    this.ComSrv.postJSON(`api/SrnDisbursementStatus/GetTraineeForSrnDisbursementByFilters`, {  Month: this.month.value, SchemeID: this.filters.SchemeID, TSPID: this.filters.TSPID, ClassID: this.filters.ClassID }).subscribe((d: any) => {
      this.srnDisbursementTrainees = new MatTableDataSource(d[0]);

      this.srnDisbursementTrainees.paginator = this.paginator;
      this.srnDisbursementTrainees.sort = this.sort;
    },
      error => this.error = error// error path
    );
  }

  onTraineeFileChange(ev: any) {
    let workBook = null;
    let jsonData = null;
    const reader = new FileReader();
    const file = ev.target.files[0];
    reader.onload = (event) => {
      const data = reader.result;
      workBook = XLSX.read(data, { type: 'binary' });
      jsonData = workBook.SheetNames.reduce((initial, name) => {
        const sheet = workBook.Sheets[name];
        initial[name] = XLSX.utils.sheet_to_json(sheet);
        return initial;
      }, {});

      const dataString = JSON.stringify(jsonData);
      this.data = JSON.parse(dataString);
      console.log(this.srnDisbursementTrainees.filteredData);
      console.log(this.data.Srn_Disbursement_Status);

      if (!this.data.Srn_Disbursement_Status) {
        this.ComSrv.ShowError("Sheet with the name 'Srn_Disbursement_Status' not found in Excel file");
        return false;
      }

      this.selectedTrainees = this.data.Srn_Disbursement_Status.filter(x => this.srnDisbursementTrainees.filteredData.map(y => y.TraineeCode).includes(x.TraineeCode));
      console.log(this.selectedTrainees);

      if (this.selectedTrainees.length == 0) {
        this.error = "No matched trainee to update disbursement information";

        this.ComSrv.ShowError(this.error.toString(), "Error");
        return false;
      }

      this.selectedTrainees = this.srnDisbursementTrainees.filteredData.filter(x => this.data.Srn_Disbursement_Status.map(y => y.TraineeCode).includes(x.TraineeCode));
      console.log(this.selectedTrainees);

      //this.selectedTrainees = this.selectedTrainees.map(x => x.TokenNumber == this.data.Srn_Disbursement_Status.map(y => y.TraineeCode).includes(x.TraineeCode));

      //this.disbursementUpdatedTrainees =
        this.selectedTrainees.forEach((item) => {
        item.TokenNumber = this.data.Srn_Disbursement_Status.filter(x => x.TraineeCode === item.TraineeCode).map(y => y.TokenNumber).toString();
          item.TransactionNumber = this.data.Srn_Disbursement_Status.filter(x => x.TraineeCode === item.TraineeCode).map(y => y.TransactionNumber).toString();
          item.Redeem = this.data.Srn_Disbursement_Status.filter(x => x.TraineeCode === item.TraineeCode).map(y => y.Status).toString();
        //item.TokenNumber = this.tk[0].TokenNumber;
          //.map(y => y.TokenNumber.toString());

      })

      //this.srnDisbursementTrainees = new MatTableDataSource(this.data.Srn_Disbursement_Status)
      return;
      
    }
    reader.readAsBinaryString(file);
    ev.target.value = '';
  }

  generateExcel() {
    let timeSpan = new Date().toISOString();
    let fileName = `Stipend Disbursement Status`;

    let dataForExport = this.populateData(this.srnDisbursementTrainees.filteredData); // this.srnDisbursementTrainees.filteredData;

    let workbook = new Workbook();
    let workSheet = workbook.addWorksheet('Srn_Disbursement_Status');

   
    dataForExport.forEach((item, index) => {
      let keys = Object.keys(item);
      let values = Object.values(item);


      if (index == 0) {
        ///SET HEADER
        let headerRow = workSheet.addRow(keys);
        headerRow.height = 25;
        headerRow.eachCell((cell, number) => {
          cell.fill = {
            type: 'pattern',
            pattern: 'solid',
            fgColor: { argb: 'cdcdcd' },
            //bgColor: { argb: 'cdcdcd' }
          }
          cell.border = { top: { style: 'thin' }, left: { style: 'thin' }, bottom: { style: 'thin' }, right: { style: 'thin' } }
          cell.font = { bold: true }
          cell.alignment = { vertical: 'middle', horizontal: 'center', readingOrder: "ltr" }
        });
      };

      ///SET COLUMN VALUES
      let row = workSheet.addRow(values);
      row.height = 30;
      row.eachCell((cell, number) => {
        cell.border = { top: { style: 'thin' }, left: { style: 'thin' }, bottom: { style: 'thin' }, right: { style: 'thin' } };
        cell.alignment = { vertical: 'middle', horizontal: 'center', readingOrder: "ltr" }
        workSheet.getColumn(number).width = 20;
      })

    });

    workbook.xlsx.writeBuffer().then((data) => {
      let blob = new Blob([data], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
      fs.saveAs(blob, `${fileName}.${ExportType.XLSX}`);
      //this.onNoClick();
    }).catch(error => {
      console.error(error);
      //this.onNoClick();
    });

  }


  SubmitDisbursement() {
    if (this.selectedTrainees.length == 0) {
      this.error = "No matched trainee to update disbursement information";

      this.ComSrv.ShowError(this.error.toString(), "Error");
      return false;
    }

    this.selectedTrainees['Month'] = this.month.value;
    this.ComSrv.postJSON('api/SrnDisbursementStatus/UpdateSrnDisbursementTrainees', this.selectedTrainees)
      .subscribe((d: any) => {
        this.update = "Disbursement Information for Trainees saved successfully";
        this.ComSrv.openSnackBar(this.update.toString(), "Updated");
      },
        error => this.error = error // error path
      );
  }


  generateTraineeExcel(str : string) {

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
    this.getSRNDisbursementTraineeData();
    datepicker.close();
  }

  clearMonth() {
    // this.month.setValue(null);
    this.month = new FormControl(moment(null));
    this.getSRNDisbursementTraineeData();
  }

  openTraineeJourneyDialogue(data: any): void {
    debugger;
    this.dialogueService.openTraineeJourneyDialogue(data);
  }

  openClassJourneyDialogue(data: any): void {
    debugger;
    this.dialogueService.openClassJourneyDialogue(data);
  }

  getSchemes() {
    this.ComSrv.postJSON('api/Scheme/FetchSchemeByUser', this.filters).subscribe(
      (d: any) => {
        this.Schemes = d;

      }, error => this.error = error
    );
  }
  getTSPDetailByScheme(schemeId: number) {
    this.filters.TSPID = 0;
    this.filters.ClassID = 0;
    this.ComSrv.getJSON(`api/TSPDetail/GetTSPDetailByScheme/` + schemeId)
      .subscribe(data => {
        this.TSPDetail = <any[]>data;
      }, error => {
        this.error = error;
      });
  }
  getClassesByTsp(tspId: number) {
    this.filters.ClassID = 0;
    this.ComSrv.getJSON(`api/Class/GetClassesByTsp/` + tspId)
      .subscribe(data => {
        this.Classes = <any[]>data;
      }, error => {
        this.error = error;
      });
  }
  EmptyCtrl(ev: any) {
    this.SearchCls.setValue('');
    this.SearchTSP.setValue('');
    this.SearchSch.setValue('');
  }

  populateData(data: any) {
    return data.map(item => {
      return {
        'Scheme Name': item.SchemeName,
        'ClassCode': item.ClassCode,
        'TSP Name': item.TSPName,
        'Trainee Name': item.TraineeName,
        'TraineeCode': item.TraineeCode,
        'Father Name': item.FatherName,
        'Trainee CNIC': item.TraineeCNIC,
        //'Class Start Date': this._date.transform(item.StartDate, 'dd/MM/yyyy'),
        'District': item.DistrictName,
        'Contact Number': item.ContactNumber,
        'Comments': item.Comments,
        'Amount': item.Amount,
        'Batch': item.Batch,
        'TokenNumber': item.TokenNumber,
        'TransactionNumber': item.TransactionNumber,
        'Status': item.Redeem,
        'Month': this._date.transform(item.Month, 'MM/yyyy'),
        'NumberOfMonths': item.NumberOfMonths,

      }
    })
  }

}


export interface IQueryFilters {
  SchemeID: number;
  TSPID: number;
  ClassID: number;
  
}
