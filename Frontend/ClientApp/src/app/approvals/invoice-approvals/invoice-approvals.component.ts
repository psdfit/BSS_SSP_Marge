/* **** Aamer Rehman Malik *****/
import { Component, OnInit } from '@angular/core';
import { CommonSrvService } from '../../common-srv.service';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { UserRightsModel, UsersModel } from '../../master-data/users/users.component';
import { DialogueService } from '../../shared/dialogue.service';
import { EnumApprovalProcess } from '../../shared/Enumerations';
import { Moment } from 'moment';
import { MatDatepicker } from '@angular/material/datepicker';
import { FormControl } from '@angular/forms';
import * as _moment from 'moment';
import { DateAdapter, MAT_DATE_LOCALE, MAT_DATE_FORMATS } from '@angular/material/core';
import { MomentDateAdapter, MAT_MOMENT_DATE_ADAPTER_OPTIONS } from '@angular/material-moment-adapter';
import { MatTableDataSource } from '@angular/material/table';
import { SearchFilter, ExportExcel } from '../../shared/Interfaces';
import { EnumExcelReportType, EnumTSPColorType } from '../../shared/Enumerations';
import { DatePipe } from '@angular/common';

const moment = _moment;
import { environment } from '../../../environments/environment';

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
  selector: 'app-invoice-approvals',
  templateUrl: './invoice-approvals.component.html',
  styleUrls: ['./invoice-approvals.component.scss'],
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
export class InvoiceApprovalsComponent implements OnInit {
  environment = environment;
  InvoiceHeaders: any;
  ApprovalData: any;
  InvoiceDetails: any[] = [];
  matTableData: MatTableDataSource<any>;
  enumApprovalProcess = EnumApprovalProcess
  processKey = '';
  error: any;
  working: boolean;
  blacklistedTSP: boolean;
  blacklistedTSPwithRed: boolean;
  blacklistedTSPwithYellow: boolean;
  month = new FormControl(moment());
  kamusers: []; schemes: []; tsps: []; tspMasters: []; filteredInvoice: any[];
  SearchSch = new FormControl('');
  SearchKAM = new FormControl('');
  SearchTSP = new FormControl('');
  //filters: IInvoiceApprovalFilter = { SchemeID: 0, TSPID: 0, KAMID: 0 };
  filters: IInvoiceApprovalFilter = { SchemeID: 0, TSPMasterID: 0, KAMID: 0 };
  currentUser: UsersModel;
  InvoiceHeadersIDsArray: any;
  InvoiceHeadersIDs: string;


  constructor(private http: CommonSrvService, private dialogue: DialogueService, private _date: DatePipe) {
    this.http.setTitle('Invoice');
  }

  ngOnInit(): void {
    this.http.OID.subscribe(OID => {
      this.GetInvoicesForApproval();

    })
    this.currentUser = this.http.getUserDetails();
    this.GetFiltersData();
    this.filteredInvoice = []

  }

  EmptyCtrl() {
    this.SearchKAM.setValue('');
    this.SearchTSP.setValue('');
    this.SearchSch.setValue('');
  }

  GetFiltersData() {
    this.http.getJSON(`api/PRNMaster/GetFiltersData`).subscribe(
      (response: any) => {
        this.kamusers = response[0];
        this.schemes = response[1];
        //this.tsps = response[2];
        this.tspMasters = response[2];
        // r.PRN = data;
        // r.HasPRN = true;
      }
      , (error) => {
        console.error(JSON.stringify(error));
      }
    );
  }

  GetInvoicesForApproval() {
    //this.http.postJSON('api/Invoice/GetInvoicesForApproval', { ProcessKey: this.processKey, U_Month: this.month.value, OID: this.http.OID.value, KAMID: this.filters.KAMID, SchemeID: this.filters.SchemeID, TSPID: this.filters.TSPID }).subscribe((d: any) => {
    this.http.postJSON('api/Invoice/GetInvoicesForApproval', { ProcessKey: this.processKey, U_Month: this.month.value, OID: this.http.OID.value, KAMID: this.filters.KAMID, SchemeID: this.filters.SchemeID, TSPMasterID: this.filters.TSPMasterID }).subscribe((d: any) => {
      this.InvoiceHeaders = d;
      this.InvoiceHeadersIDsArray = this.InvoiceHeaders.map((o: { InvoiceHeaderID: any; }) => o.InvoiceHeaderID);
      this.InvoiceHeadersIDs = this.InvoiceHeadersIDsArray.join(',');
    });
  }

  GetInvoiceLines(r) {
    if (r.InvoiceLines) {
      r.InvoiceLines = null;
      this.filteredInvoice = this.filteredInvoice.filter(inv => inv.InvoiceHeaderID !== r.InvoiceHeaderID);

      return;
    }

    this.http.getJSON('api/Invoice/GetInvoiceLines/', r.InvoiceHeaderID).subscribe((d: any) => {
      r.InvoiceLines = d;
      this.filteredInvoice.push(d);
      console.log(this.filteredInvoice);
      this.filteredInvoice = this.filteredInvoice.reduce((accumulator, value) => accumulator.concat(value), []);
    });
  }

  checkTSPColor(row: any) {
    if (row.ProcessKey === EnumApprovalProcess.INV_SRN || row.ProcessKey === EnumApprovalProcess.INV_TRN || row.ProcessKey === EnumApprovalProcess.INV_TPRN) {
      this.openApprovalDialogue(row);
    }
    else {
    this.http.postJSON('api/TSPColor/RD_TSPMasterColorByFilters', { TSPID: row.TSPID }).subscribe((response: any) => {
      console.log(response);
      if (response[0].ErrorTypeID === EnumTSPColorType.Red || response[0].ErrorTypeID === EnumTSPColorType.Black) {
        this.http.ShowError(response[0].ErrorMessage);
        // this.blacklistedTSPwithRed = true;
        // this.blacklistedTSP = true;
        return;
      }
      if (response[0].ErrorTypeID === EnumTSPColorType.Yellow) {
        // this.http.ShowError("Decision can be made against Yellowlist TSP");
        this.openApprovalDialogue(row);
      }
      if (response[0].ErrorTypeID === EnumTSPColorType.White) {
        // this.http.ShowError("Decision can be made against Yellowlist TSP");
        this.openApprovalDialogue(row);
      }
      // if (response[0].TSPColorID === EnumTSPColorType.Yellow) {
      //  this.http.ShowError("TSPs with color yellow can only perform day to days operations");
      //  this.blacklistedTSPwithYellow = true;
      //  this.blacklistedTSPwithRed = false;
      //  this.blacklistedTSP = false;
      // }
      // else {
      //  this.openApprovalDialogue(row);
      //  //this.blacklistedTSPwithRed = false;
      //  //this.blacklistedTSP = false;
      //  //this.blacklistedTSPwithYellow = true;
      // }
    }
      );
    }

  }

  CheckTSPForBlacklisting(row: any) {
    this.checkTSPColor(row.TSPID);
    if (this.blacklistedTSPwithRed === undefined || this.blacklistedTSP === undefined) {
      return;
    }
    else {
      if (this.blacklistedTSPwithRed || this.blacklistedTSP) {
        return;
      }
      else {
        this.openApprovalDialogue(row);
      }
    }

  }


  openApprovalDialogue(row: any): void {
    debugger;
    this.dialogue.openApprovalDialogue(row.ProcessKey, row.InvoiceHeaderID).subscribe(result => {
      console.log(result);
      // location.reload();
    });
  }

  GetClassMonthview(ClassID: number, Month: Date, Type: string): void {
    this.dialogue.openClassMonthviewDialogue(ClassID, Month, Type).subscribe(result => {
      // location.reload();        console.log(result);

    });
  }

  // applyFilter(filterValue: string) {
  //    filterValue = filterValue.trim(); // Remove whitespace
  //    filterValue = filterValue.toLowerCase(); // Datasource defaults to lowercase matches
  //    this.matTableData.filter = filterValue;
  // }

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
    this.GetInvoicesForApproval();
    datepicker.close();
  }

  clearMonth() {
   // this.month.setValue(null);
   this.month = new FormControl(moment(null));
    this.GetInvoicesForApproval();
  }

  exportToExcel() {
    this.http.postJSON('api/Invoice/GetInvoiceBulkExcelExportByIDs', this.InvoiceHeadersIDs).subscribe((d: any) => {
      const filteredData = d;

      // const result = filteredData.reduce((accumulator, value) => accumulator.concat(value), []);
      // console.log(result);

      const exportExcel: ExportExcel = {
        Title: 'Invoice Report',
        Author: this.currentUser.FullName,
        Type: EnumExcelReportType.Invoice,
        Data: {},
        // List1: data
        List1: this.populateData(filteredData)
      };
      this.dialogue.openExportConfirmDialogue(exportExcel).subscribe();
    });
  }

  ExportToExcel(InvoiceHeaderID: number) {
    this.http.getJSON('api/Invoice/GetInvoiceLines/', InvoiceHeaderID).subscribe((d: any) => {
      const filteredData = d;

      const exportExcel: ExportExcel = {
        Title: 'Invoice Report',
        Author: this.currentUser.FullName,
        Type: EnumExcelReportType.Invoice,
        Data: {},
        // List1: data
        List1: this.populateData(filteredData)
      };
      this.dialogue.openExportConfirmDialogue(exportExcel).subscribe();
    });
  }
    ExportInvoiceDetailsToExcel() {
    this.http.postJSON('api/Invoice/GetInvoiceDetails', 
    { ProcessKey: this.processKey, 
      U_Month: this.month.value, 
      OID: this.http.OID.value, 
      KAMID: this.filters.KAMID, 
      SchemeID: this.filters.SchemeID, 
      TSPMasterID: this.filters.TSPMasterID 
    }).subscribe((d: any) => {
      this.InvoiceDetails = d;

      const exportExcel: ExportExcel = {
        Title: 'Invoice Details Report',
        Author: this.currentUser.FullName,
        Type: EnumExcelReportType.Invoice,
        Data: {},
        List1: this.populateInvoiceDetailData(this.InvoiceDetails)
      };
      this.dialogue.openExportConfirmDialogue(exportExcel).subscribe();
    });
  }

  populateInvoiceDetailData(data: any) {
    return data.map((item, index) => {
      return {
        'Sr#': index + 1,
        'U_IPS': item.U_IPS,
        'Class Code': item.ClassCode,
        'TSP Name': item.TSPName,
        'TSP SAP Code': item.TSPSAPCode,
        'Scheme Name': item.SchemeName,
        'Scheme Code': item.SchemeCode,
        'Invoice Status': item.InvoiceStatus,
        'Invoice Month': this._date.transform(item.InvoiceMonth, 'dd/MM/yyyy'),
        'Net Payable': item.NetPayableAmount,
        'Invoice Type': item.InvoiceType,
      };
    });
  }
  
  populateData(data: any) {
    return data.map(item => {
      return {
        // "Scheme Code": item.SchemeCode

        'Invoice Type': item.InvoiceType,
        'Class Code': item.ClassCode,
        Description: item.Description,
        'G/L Account': item.GLCode,
        'GL Name': item.GLName,
        Scheme: item.SchemeName,
        Trade: item.TradeName,
        Department: item.PCategoryName,
        'Tax Code': item.TaxCode,
        'W Tax Liable': item.WTaxLiable,
        'Class Start Date': this._date.transform(item.StartDate, 'dd/MM/yyyy'),
        'Class End Date': this._date.transform(item.EndDate, 'dd/MM/yyyy'),
        'Actual Start Date': this._date.transform(item.ActualStartDate, 'dd/MM/yyyy'),
        'Actual End Date': this._date.transform(item.ActualEndDate, 'dd/MM/yyyy'),
        Batch: item.Batch,
        'Batch Duration': item.BatchDuration,
        'Invoice Number': item.InvoiceNumber,
        'Trainee Per Class': item.TraineePerClass,
        'Class Days': item.ClassDays,
        'Claim Trainees': item.ClaimTrainees,
        'Total Monthly Payment': item.TotalMonthlyPayment,
        Stipend: item.Stipend,
        'Uniform and Bag': item.UniformBag,
        'Total Cost Per Trainee': item.TotalCostPerTrainee,
        'Unverified CNIC Deductions': item.UnverifiedCNICDeductions,
        'Cnic Deduction Type': item.CnicDeductionType,
        'Cnic Deduction Amount': item.CnicDeductionAmount,
        'Deduction Trainee Droput': item.DeductionTraineeDroput,
        'DropOut Deduction Type': item.DropOutDeductionType,
        'DropOut Deduction Amount': item.DropOutDeductionAmount,
        'Deduction Trainee Attendance': item.DeductionTraineeAttendance,
        'Attendance Deduction Amount': item.AttendanceDeductionAmount,
        'Misc Deduction No': item.MiscDeductionNo,
        'Misc Deduction Type': item.MiscDeductionType,
        'Misc Deduction Amount': item.MiscDeductionAmount,
        'Penalty Percentage': item.PenaltyPercentage,
        'Penalty Amount': item.PenaltyAmount,
        'Result Deduction': item.ResultDeduction,
        'Net Payable Amount': item.NetPayableAmount,
        'Gross Payable': item.GrossPayable,
        'Net Training Cost': item.NetTrainingCost,
        'Total LC': item.TotalLC,
        'Line Total': item.LineTotal
      }
    })
  }

}

export interface IInvoiceApprovalFilter {
  SchemeID: number;
  //TSPID: number;
  TSPMasterID: number;
  KAMID: number;
}
