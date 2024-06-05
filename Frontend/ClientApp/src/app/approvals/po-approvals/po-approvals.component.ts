import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import * as XLSX from 'xlsx';
import { DialogueService } from '../../shared/dialogue.service';
import { DateAdapter, MAT_DATE_FORMATS, MAT_DATE_LOCALE } from '@angular/material/core';
import { MomentDateAdapter, MAT_MOMENT_DATE_ADAPTER_OPTIONS } from '@angular/material-moment-adapter';
import { Moment } from 'moment';
import * as _moment from 'moment';
import { MatDatepicker } from '@angular/material/datepicker';
import { FormControl } from '@angular/forms';
import { EnumApprovalProcess, EnumExcelReportType } from '../../shared/Enumerations';
import { CommonSrvService } from '../../common-srv.service';
const moment = _moment;
import { SearchFilter,ExportExcel } from '../../shared/Interfaces';
import { GroupByPipe } from 'angular-pipes'
import { DatePipe } from '@angular/common';
import { environment } from '../../../environments/environment';


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
  selector: 'app-po-approvals',
  templateUrl: './po-approvals.component.html',
  styleUrls: ['./po-approvals.component.scss'],
    providers: [DatePipe,
    // `MomentDateAdapter` can be automatically provided by importing `MomentDateModule` in your
    // application's root module. We provide it at the component level here, due to limitations of
    // our example generation script.
    {
      provide: DateAdapter,
      useClass: MomentDateAdapter,
      deps: [MAT_DATE_LOCALE, MAT_MOMENT_DATE_ADAPTER_OPTIONS]
    },

    { provide: MAT_DATE_FORMATS, useValue: MY_FORMATS },
  ],
})
export class PoApprovalsComponent implements OnInit {

  constructor(private http: CommonSrvService, private dialogue: DialogueService,
      private ComSrv: CommonSrvService, public dialogueService: DialogueService, private _date: DatePipe
  ) { }
  environment = environment;
  POHeaders: any[];
  ApprovalData: any;
  error: any;
  working: boolean;
  enumApprovalProcess = EnumApprovalProcess
  processKey = '';
  month = new FormControl(moment());

  @ViewChild('TABLE', { static: false }) TABLE: ElementRef;
  title = 'Excel';
  currentUser: any;


  schemeArray = [];
  tspDetailArray = [];
  SearchSch = new FormControl('');
  SearchTSP = new FormControl('');

  filters: SearchFilter = { SchemeID: 0, TSPID: 0};
  EmptyCtrl(Ev: any) {
    this.SearchTSP.setValue('');
    this.SearchSch.setValue('');
  }
  ngOnInit(): void {
    this.http.setTitle('Purchase Order Approval Requests');
    this.GetSchemes();
    // this.GetPOHeaderByFiltersByFilters();

    this.currentUser = this.ComSrv.getUserDetails();
  }


  // GetPOHeadersForApproval() {
  //  this.http.getJSON("api/PurchaseOrder/GetPOForApproval").subscribe((d: any) => {
  //    this.POHeaders = d;
  //  });
  // }

  GetSchemes() {
    this.http.postJSON('api/PurchaseOrder/GetPOHeaderSchemes', null).subscribe(
      (d: any) => {
        this.schemeArray = d.scheme;
        this.schemeArray.unshift({ SchemeID: 0, SchemeName: '-- All --' });
      }
    );
  }

  GetPOHeader() {
    this.http.postJSON(`api/PurchaseOrder/GetPOHeader`, { ProcessKey: this.processKey, Month: this.month.value }).subscribe(
      (d: any) => {
        this.POHeaders = d.poheader;
        this.schemeArray = d.scheme;
        // if (!this.isTSPUser) {
        this.schemeArray.unshift({ SchemeID: 0, SchemeName: '-- All --' });
        console.log(this.schemeArray);
      }
      , (error) => {
        console.error(JSON.stringify(error));
      }
    );
  }

  GetPOHeaderByFiltersByFilters() {
    this.http.postJSON(`api/PurchaseOrder/GetPOHeaderByFilters`,
     { ProcessKey: this.processKey, SchemeID: this.filters.SchemeID, TSPID: this.filters.TSPID, Month: this.month.value })
     .subscribe(
      (d: any) => {
        this.POHeaders = d.poheader;
        this.tspDetailArray = d.tspdetail;
        // this.tspDetailArray.unshift({ TSPID: 0, TSPNAME: '-- All --' });
        console.log(this.tspDetailArray);
      }
      , (error) => {
        console.error(JSON.stringify(error));
      }
    );
  }

  GetPoLinesForHeader(POHeaderID: any, r: any) {
    if (r.POLines) {
      r.POLines = null;
      return;
    }
    this.http.getJSON('api/PurchaseOrder/GetPOLinesByPOHeaderID/' + POHeaderID).subscribe(
      (d: any) => {
        r.POLines = d;
      }
      , (error) => {
        console.error(error);
      }
    );
  }
  openApprovalDialogue(row: any): void {
    this.dialogue.openApprovalDialogue(row.ProcessKey, row.POHeaderID).subscribe(result => {
      console.log(result);
      // location.reload();
    });
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
    // this.GetPOHeader();
    this.GetPOHeaderByFiltersByFilters();
    datepicker.close();
  }

  clearMonth() {
   // this.month.setValue(null);
   this.month = new FormControl(moment(null));
    // this.GetPOHeader();
    this.GetPOHeaderByFiltersByFilters();
  }

  // ExportToExcel() {

  //    this.http.postJSON("api/PurchaseOrder/GetPOSummary", { Month: this.month.value }).subscribe((d: any) => {

  //        //const ws: XLSX.WorkSheet = XLSX.utils.table_to_sheet(this.TABLE.nativeElement);
  //        const ws: XLSX.WorkSheet = XLSX.utils.json_to_sheet(d);
  //        const wb: XLSX.WorkBook = XLSX.utils.book_new();
  //        XLSX.utils.book_append_sheet(wb, ws, 'Sheet1');
  //        XLSX.writeFile(wb, 'PO_Summary.xlsx');

  //    });

  // }

  ExportToExcel(name?: string) {
    let filteredData;

    this.http.postJSON('api/PurchaseOrder/GetPOSummary',
    {Month: this.month.value, ProcessKey: this.processKey, SchemeID: this.filters.SchemeID, TSPID: this.filters.TSPID})
    .subscribe((d: any) => {
      filteredData = d;

      const data = {
        Month: this.month,
      };

      const exportExcel: ExportExcel = {
        Title: 'PO Summary Report',
        Author: this.currentUser.FullName,
        Type: EnumExcelReportType.PO,
        // Data: data,
        List1: this.populateData(filteredData),
      };
      this.dialogueService.openExportConfirmDialogue(exportExcel).subscribe();
    });
  }


  populateData(data: any) {
    return data.map(item => {
      return {
        Scheme: item.SchemeName
          , TSP: item.TSPName
          // , "Class Code": item.ClassCode
        , Trades: item.Trades
        , 'No. of Classes': item.ClassCount
          // , "Trade Name": item.TradeName
        , 'No. of Trainees': item.TraineesPerClass
          // , "Start Date": this._date.transform(item.StartDate, 'dd/MM/yyyy')
          // , "End Date": this._date.transform(item.EndDate, 'dd/MM/yyyy')


        , 'Scheme Cost': item.SchemeCost
        , 'Tax Rate': item.TaxRate
        , 'GST(PKR)': item.TotalCostPerClassInTax * item.TaxRate
        , 'PST(PKR)': item.PST
        , 'Final Amount': item.SchemeCost - (item.TotalCostPerClassInTax * item.TaxRate)
        // , "Month": item.Month,
      }
    })
  }


}

