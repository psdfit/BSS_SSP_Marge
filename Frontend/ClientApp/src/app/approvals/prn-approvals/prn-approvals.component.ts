/* **** Aamer Rehman Malik *****/
import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { CommonSrvService } from '../../common-srv.service';
import { DateAdapter, MAT_DATE_FORMATS, MAT_DATE_LOCALE } from '@angular/material/core';
import { MomentDateAdapter, MAT_MOMENT_DATE_ADAPTER_OPTIONS } from '@angular/material-moment-adapter';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { EnumApprovalProcess, EnumExcelReportType, EnumTSPColorType } from '../../shared/Enumerations';
import { FormControl } from '@angular/forms';
import { Moment } from 'moment';
import * as _moment from 'moment';
import { DialogueService } from '../../shared/dialogue.service';
import { MatDatepicker } from '@angular/material/datepicker';
import { ExportExcel } from '../../shared/Interfaces';
import { trigger, transition, query, stagger, animate, style } from '@angular/animations';
import { environment } from '../../../environments/environment';
import { DatePipe } from '@angular/common';
import { MatDialog } from '@angular/material/dialog';
import { PrnApprovalPenaltyComponent } from 'src/app/master-data/prn-approval-penalty/prn-approval-penalty.component';
const moment = _moment


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
  selector: 'app-prn-approvals',
  templateUrl: './prn-approvals.component.html',
  styleUrls: ['./prn-approvals.component.scss'],
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
  animations: [
    trigger('listAnimation', [
      transition('* => *', [
        // each time the binding value changes
        query(
          ':leave',
          [stagger(100, [animate('0.5s', style({ opacity: 0 }))])],
          { optional: true }
        ),
        query(
          ':enter',
          [
            style({ opacity: 0 }),
            stagger(100, [animate('0.5s', style({ opacity: 1 }))])
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
    )]
})
export class PrnApprovalsComponent implements OnInit {
  // displayColumns: string[] = ['Actions', 'InvoiceNumber', 'ClaimedTrainees', 'EnrolledTrainees', 'DropoutsVerified', 'DropoutsUnverified', 'CNICVerified', 'CNICVExcesses', 'CNICUnverified', 'CNICUnVExcesses', 'MaxAttendance', 'DeductionMarginal', 'Duration', 'ContractualTrainees', 'ExpelledVerified', 'ExpelledUnverified', 'PassVerified', 'PassUnverified', 'FailedVerified', 'FailedUnverified', 'AbsentVerified', 'AbsentUnverified', 'DropOut', 'DeductionExtraRegisteredForExam', 'DeductionFailedTrainees', 'PenaltyImposedByME', 'DeductionUniformBagReceiving', 'CompletedTrainees', 'GraduatedCommitmentTrainees', 'EmploymentReported', 'VerifiedTrainees', 'VerifiedToCompletedCommitment', 'TraineesFoundInVISIT1', 'TraineesFoundInVISIT2', 'DeductionSinIncepDropout', 'PaymentWithheldSinIncepUnVCNIC', 'PenaltyTPMReports', 'ReimbursementUnVTrainees', 'ReimbursementAttandance', 'PaymentToBeReleasedTrainees', 'Payment100p', 'Payment50p', 'TradeName', 'ClassCode', 'ClassStatus', 'NonFunctionalVisit1', 'NonFunctionalVisit2', 'EmploymentCommitmentPercentage', 'NonFunctionalVisit1Date', 'NonFunctionalVisit2Date', 'NonFunctionalVisit3Date', 'ClassStartDate', 'ClassEndDate', 'IsApproved']
  displayColumns: string[] = ['Actions', 'TSPName', 'Month', 'Invoice Number', 'Created Date', 'Approved'];
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  environment = environment;
  SearchSch = new FormControl('');
  SearchKAM = new FormControl('');
  SearchTSP = new FormControl('');
  //===============Azhar Iqbal=================
  SearchStatus = new FormControl(0);
  //===========================================

  @ViewChild('TABLE', { static: false }) TABLE: ElementRef;
  title = 'Excel';
  currentUser: any;
  PRNMasterIDs: string;

  PRNMaster: any;
  PRNMasterIDsArray: any;
  error = '';
  PRNMonth:any;
  matTableData: MatTableDataSource<any>;
  enumApprovalProcess = EnumApprovalProcess;
  processKey = '';
  processTypes = [
    { value: EnumApprovalProcess.PRN_R, text: 'Regular' }
    , { value: EnumApprovalProcess.PRN_C, text: 'Completion' }
    , { value: EnumApprovalProcess.PRN_F, text: 'Final' }
  ]
  //filters: IPRNApprovalFilter = { SchemeID: 0, TSPID: 0, KAMID: 0 };
  
  filters: IPRNApprovalFilter = { SchemeID: 0, TSPMasterID: 0, KAMID: 0, StatusID: 0 };

  kamusers: [];
  schemes: any[];
  tsps: any[];
  tspMasters: [];
  prnDetailsArray: any[];
  PRNDetailsBulkArray: any[];
  PenaltyBtn=false;
  month = new FormControl(moment());

  constructor(private http: CommonSrvService, private dialogue: DialogueService, private datePipe: DatePipe,public dialog: MatDialog) {
    this.http.setTitle('Payment Recommendation Note');
  }

  EmptyCtrl() {
    this.SearchKAM.setValue('');
    this.SearchTSP.setValue('');
    this.SearchSch.setValue('');
    this.SearchStatus.setValue('');
  }

  ngOnInit(): void {
    this.currentUser = this.http.getUserDetails();
    this.prnDetailsArray = []

    this.http.OID.subscribe(OID => {
      this.GetPRNMasterForApproval();
      this.GetFiltersData();
     
    });

   }

  GetClassMonthview(ClassID: number, Month: Date, processkey: string): void {
    this.dialogue.openClassMonthviewDialogue(ClassID, Month, processkey).subscribe(result => {
      console.log(result);
      // location.reload();
    });
  }
  // GetPRNForApproval() {
  //    this.http.postJSON(`api/PRN/GetPRNForApproval`, { ProcessKey: this.processKey, Month: this.month.value }).subscribe(
  //        (data: any[]) => {
  //            this.matTableData = new MatTableDataSource<any[]>(data);
  //            this.matTableData.paginator = this.paginator;
  //            this.matTableData.sort = this.sort;
  //        }
  //        , (error) => {
  //            console.error(JSON.stringify(error));
  //        }
  //    );
  // }

  GetPRNMasterForApproval() {
    //this.http.postJSON(`api/PRNMaster/GetPRNMasterForApproval`, { ProcessKey: this.processKey, Month: this.month.value, OID: this.http.OID.value, KAMID: this.filters.KAMID, SchemeID: this.filters.SchemeID, TSPID: this.filters.TSPID }).subscribe(
    this.http.postJSON(`api/PRNMaster/GetPRNMasterForApproval`, { ProcessKey: this.processKey, Month: this.month.value, OID: this.http.OID.value, KAMID: this.filters.KAMID, SchemeID: this.filters.SchemeID, TSPMasterID: this.filters.TSPMasterID, StatusID: this.filters.StatusID}).subscribe(
      (data: any) => {
        this.PRNMaster = data;
        this.PRNMasterIDsArray = this.PRNMaster.map(o => o.PRNMasterID);
        this.PRNMasterIDs = this.PRNMasterIDsArray.join(',');

        // {
        //    return { PRNMasterID: o.PRNMasterID };
        // });


        // this.PRNMaster.values['PRNMasterID'];
      }
      , (error) => {
        console.error(JSON.stringify(error));
      }
    );
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

  GetPRN(r: any) {
    //this.PenaltyBtn=false;
    this.getApprovalHistory(r,r.PRNMasterID,r.ProcessKey,r.Month);
    r.HasPRN = !r.HasPRN;

    if (r.PRN) {
      this.prnDetailsArray = this.prnDetailsArray.filter(s => s.PRNMasterID !== r.PRNMasterID);

      return;
    }
    this.http.getJSON(`api/PRN/GetPRNForApproval/`, r.PRNMasterID).subscribe(
      (data: any) => {
        r.PRN = data;
        r.HasPRN = true;
        r.HasPRN = true;
        this.prnDetailsArray.push(data);
        this.prnDetailsArray = this.prnDetailsArray.reduce((accumulator, value) => accumulator.concat(value), []);
        console.log(this.prnDetailsArray);
      }
      , (error) => {
        console.error(JSON.stringify(error));
      }
    );
  }

  applyFilter(filterValue: string) {
    filterValue = filterValue.trim(); // Remove whitespace
    filterValue = filterValue.toLowerCase(); // Datasource defaults to lowercase matches
    this.matTableData.filter = filterValue;
  }

  checkTSPColor(row: any) {
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


  openApprovalDialogue(row: any): void {
    this.dialogue.openApprovalDialogue(row.ProcessKey, row.PRNMasterID).subscribe(result => {
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
    this.GetPRNMasterForApproval();
    datepicker.close();
  }

  clearMonth() {
    this.month = new FormControl(moment(null));
   // this.month.setValue(null);
    this.GetPRNMasterForApproval();
  }

  ExportToExcel(PRNMasterID: number, ProcessKey: string, month: string) {
    let filteredData;
    // tslint:disable-next-line: max-line-length
    this.http.postJSON('api/PRN/GetPRNExcelExport/', { PRNMasterID, Month: this.datePipe.transform(month, 'yyyy-MM-dd') }).subscribe((d: any) => {
      filteredData = d;

      const headerData = {
        'Scheme Name ': filteredData[0]?.SchemeName,
        'Tsp Name ': filteredData[0]?.TSPName,
        'Class Code(s) ': filteredData.map(x => x.ClassCode).join(','),
        'Invoice Type ': this.processTypes.find(x => x.value === ProcessKey).text,
        'Invoice Month ': this.datePipe.transform(month, 'MMMM yyyy'),
      }
      const exportExcel: ExportExcel = {
        Title: 'Payment Recommendation Note',
        Author: this.currentUser.FullName,
        // tslint:disable-next-line: max-line-length
        Type: (ProcessKey === EnumApprovalProcess.PRN_F ? EnumExcelReportType.PRN_F : ProcessKey === EnumApprovalProcess.PRN_C ? EnumExcelReportType.PRN_C : EnumExcelReportType.PRN_R),
        Data: headerData,
        List1: this.populateData(filteredData, ProcessKey),
      };
      this.dialogue.openExportConfirmDialogue(exportExcel).subscribe();
    });
  }


  // GetByPRNMasterIDs() {
  //  this.http.getJSON('api/PRN/GetPRNExcelExportByIDs/' + this.PRNMasterIDs).subscribe((d: any) => {
  //    this.PRNDetailsBulkArray = d;
  //    this.ExportToExcelBulkPRN();
  //  });
  // }

  GetByPRNMasterIDs() {
    this.http.postJSON('api/PRN/GetPRNBulkExcelExportByIDs', this.PRNMasterIDs).subscribe((d: any) => {
      this.PRNDetailsBulkArray = d;
      this.ExportToExcelBulkPRN();
    });

  }

  ExportToExcelBulkPRN() {

    // if (this.prnDetailsArray.length === 0) {
    //    this.http.ShowError("Please check PRN Details to export data")
    //    return;
    // }
    const filteredData = this.PRNDetailsBulkArray;


    // const result = filteredData.reduce((accumulator, value) => accumulator.concat(value), []);
    // console.log(result);

    const exportExcel: ExportExcel = {
      Title: 'PRN Approval Report',
      Author: this.currentUser.FullName,
      Type: EnumExcelReportType.PRN,
      Data: {},
      // List1: data
      List1: this.populatePRNByIDsData(filteredData, '')
    };
    this.dialogue.openExportConfirmDialogue(exportExcel).subscribe();

  }

  populatePRNByIDsData(data: any, _ProcessKey: string) {
    return data.map(item => {
      return {
          'Project': item.FundingCategory
         ,'Scheme': item.SchemeName
         ,'TSP': item.TSPName
         ,'KAM': item.kam
         ,'PRN Month': this.datePipe.transform(item.Month, 'MM/yyyy')
         ,'Class Code': item.ClassCode
        , 'Invoice No': item.InvoiceNumber
        , Trade: item.TradeName
        , 'Certification Authority':item.CertAuthName
        , Duration: item.Duration
        , 'Class Start Date': this.datePipe.transform(item.ClassStartDate, 'dd/MM/yyyy')
        , 'Class End Date': this.datePipe.transform(item.ClassEndDate, 'dd/MM/yyyy')
        , 'Class Status': item.ClassStatus
        , 'Contractual Trainees': item.ContractualTrainees
        , 'Claimed Trainees': item.ClaimedTrainees
        , 'Enrolled Trainees': item.EnrolledTrainees
        , 'CNIC Verified': item.CNICVerified
        , 'CNIC Verified Excesses': item.CNICVExcesses
        , 'Dropouts Verified': item.DropoutsVerified
        , 'Expelled Verified': item.ExpelledVerified
        , 'Pass Verified': item.PassVerified
        , 'Failed Verified': item.FailedVerified
        , 'Absent Verified': item.AbsentVerified
        , 'CNIC Unverified': item.CNICUnverified
        , 'CNIC UnVerified Excesses': item.CNICUnVExcesses
        , 'Dropouts Unverified': item.DropoutsUnverified
        , 'Expelled UnVerified': item.ExpelledUnverified
        , 'Pass Unverified': item.PassUnverified
        , 'Failed Unverified': item.FailedUnverified
        , 'Absent Unverified': item.AbsentUnverified
        , 'Dropout (Pass/Fail/Absent)': item.DropoutPassFailAbsent
        , 'Expelled (Pass/Fail/Absent)': item.ExpelledPassFailAbsent
        , 'Deduction Since Inception Dropout': item.DeductionSinIncepDropout
        , 'Max Attendance': item.MaxAttendance
        , 'Payment Withheld Physical Count': item.PaymentWithheldPhysicalCount
        , 'Deduction Marginal': item.DeductionMarginal
        , 'Deduction Extra Registered For Exam': item.DeductionExtraRegisteredForExam
        , 'Deduction Failed Trainees': item.DeductionFailedTrainees
        , 'Deduction Uniform Bag Receiving': item.DeductionUniformBagReceiving
        , 'Payment Withheld Since Inception UnV CNIC': item.PaymentWithheldSinIncepUnVCNIC
        , 'Penalty TPM Reports': item.PenaltyTPMReports
        , 'Penalty Imposed By MnE': item.PenaltyImposedByME
        , 'Reimbursement UnV Trainees': item.ReimbursementUnVTrainees
        , 'Reimbursement Attandance': item.ReimbursementAttandance
        , 'Employment Commitment Percentage': item.EmploymentCommitmentPercentage
        , 'Completed Trainees': item.CompletedTrainees
        , 'Employment Commitment Trainees': item.GraduatedCommitmentTrainees
        , 'Employment Reported': item.EmploymentReported
        , 'Verified Trainees': item.VerifiedTrainees
        , 'Verified to Commitment': (item.VerifiedToCompletedCommitment) + '%'
        // tslint:disable-next-line: max-line-length
        , 'Expelled Regular Verified For The Month': (item.ExpelledRegularVerifiedForTheMonth)
        , 'Certification Cost Deduction (All Types)': item.ExtraTraineeDeductCompletion + item.UnVDeductCompletion + item.DropOutDeductCompletion + item.AbsentDeductCompletion
        , 'Payment To Be Released Trainees': item.PaymentToBeReleasedTrainees
        , 'Status': item.StatusApproved
        , 'Approval Date': this.datePipe.transform(item.FinalApprovalDate, 'dd/MM/yyyy')
        , 'Creation Date': this.datePipe.transform(item.CreationDate, 'dd/MM/yyyy')

      };
    });
  }


  populateData(data: any, ProcessKey: string) {
    console.log(ProcessKey)
    if (ProcessKey === EnumApprovalProcess.PRN_F) {
      return data.map(item => {
        return {
          'Project': item.FundingCategory
          ,'Scheme': item.SchemeName
          ,'TSP': item.TSPName
          ,'KAM': item.kam
          ,'PRN Month': this.datePipe.transform(item.Month, 'MM/yyyy')
           , 'Class Code': item.ClassCode
          , 'Invoice No': item.InvoiceNumber
          , Trade: item.TradeName
          , 'Certification Authority':item.CertAuthName
          , Duration: item.Duration
          , 'Class Start Date': this.datePipe.transform(item.ClassStartDate, 'dd/MM/yyyy')
          , 'Class End Date': this.datePipe.transform(item.ClassEndDate, 'dd/MM/yyyy')
          , 'Class Status': item.ClassStatus
          , 'Contractual Trainees': item.ContractualTrainees
          , 'Claimed Trainees': item.ClaimedTrainees
          , 'Enrolled Trainees': item.EnrolledTrainees
          , 'CNIC Verified': item.CNICVerified
          , 'CNIC Verified Excesses': item.CNICVExcesses
          , 'Dropouts Verified': item.DropoutsVerified
          , 'Expelled Verified': item.ExpelledVerified
          , 'Pass Verified': item.PassVerified
          , 'Failed Verified': item.FailedVerified
          , 'Absent Verified': item.AbsentVerified
          , 'CNIC Unverified': item.CNICUnverified
          , 'CNIC UnVerified Excesses': item.CNICUnVExcesses
          , 'Dropouts Unverified': item.DropoutsUnverified
          , 'Expelled UnVerified': item.ExpelledUnverified
          , 'Pass Unverified': item.PassUnverified
          , 'Failed Unverified': item.FailedUnverified
          , 'Absent Unverified': item.AbsentUnverified
          , 'Dropout (Pass/Fail/Absent)': item.DropoutPassFailAbsent
          , 'Expelled (Pass/Fail/Absent)': item.ExpelledPassFailAbsent
          , 'Deduction Since Inception Dropout': item.DeductionSinIncepDropout
          , 'Max Attendance': item.MaxAttendance
          , 'Payment Withheld Physical Count': item.PaymentWithheldPhysicalCount
          , 'Deduction Marginal': item.DeductionMarginal
          , 'Deduction Extra Registered For Exam': item.DeductionExtraRegisteredForExam
          , 'Deduction Failed Trainees': item.DeductionFailedTrainees
          , 'Deduction Uniform Bag Receiving': item.DeductionUniformBagReceiving
          , 'Payment Withheld Since Inception UnV CNIC': item.PaymentWithheldSinIncepUnVCNIC
          , 'Penalty TPM Reports': item.PenaltyTPMReports
          , 'Penalty Imposed By MnE': item.PenaltyImposedByME
          , 'Reimbursement UnV Trainees': item.ReimbursementUnVTrainees
          , 'Reimbursement Attandance': item.ReimbursementAttandance
          , 'Employment Commitment Percentage': item.EmploymentCommitmentPercentage
          , 'Completed Trainees': item.CompletedTrainees
          , 'Employment Commitment Trainees': item.GraduatedCommitmentTrainees
          , 'Employment Reported': item.EmploymentReported
          , 'Verified Trainees': item.VerifiedTrainees
          , 'Verified to Commitment': (item.VerifiedToCompletedCommitment) + '%'
          , 'Payment To Be Released Trainees': item.PaymentToBeReleasedTrainees
          , 'Status': item.StatusApproved
          , 'Approval Date': this.datePipe.transform(item.FinalApprovalDate, 'dd/MM/yyyy')
          , 'Creation Date': this.datePipe.transform(item.CreationDate, 'dd/MM/yyyy')
        };
      });
    }
    else if (ProcessKey === EnumApprovalProcess.PRN_C) {
      return data.map(item => {
        return {
          'Project': item.FundingCategory
          ,'Scheme': item.SchemeName
          ,'TSP': item.TSPName
          ,'KAM': item.kam
          ,'PRN Month': this.datePipe.transform(item.Month, 'MM/yyyy')
           , 'Class Code': item.ClassCode
          , 'Invoice No': item.InvoiceNumber
          , Trade: item.TradeName
          , 'Certification Authority':item.CertAuthName
          , Duration: item.Duration
          , 'Class Start Date': this.datePipe.transform(item.ClassStartDate, 'dd/MM/yyyy')
          , 'Class End Date': this.datePipe.transform(item.ClassEndDate, 'dd/MM/yyyy')
          , 'Class Status': item.ClassStatus
          , 'Contractual Trainees': item.ContractualTrainees
          , 'Claimed Trainees': item.ClaimedTrainees
          , 'Enrolled Trainees': item.EnrolledTrainees
          , 'CNIC Verified': item.CNICVerified
          , 'CNIC Verified Excesses': item.CNICVExcesses
          , 'Dropouts Verified': item.DropoutsVerified
          , 'Expelled Verified': item.ExpelledVerified
          , 'Pass Verified': item.PassVerified
          , 'Failed Verified': item.FailedVerified
          , 'Absent Verified': item.AbsentVerified
          , 'CNIC Unverified': item.CNICUnverified
          , 'CNIC UnVerified Excesses': item.CNICUnVExcesses
          , 'Dropouts Unverified': item.DropoutsUnverified
          , 'Expelled UnVerified': item.ExpelledUnverified
          , 'Pass Unverified': item.PassUnverified
          , 'Failed Unverified': item.FailedUnverified
          , 'Absent Unverified': item.AbsentUnverified
          , 'Dropout (Pass/Fail/Absent)': item.DropoutPassFailAbsent
          , 'Expelled (Pass/Fail/Absent)': item.ExpelledPassFailAbsent
          , 'Deduction Since Inception Dropout': item.DeductionSinIncepDropout
          , 'Max Attendance': item.MaxAttendance
          , 'Payment Withheld Physical Count': item.PaymentWithheldPhysicalCount
          , 'Deduction Marginal': item.DeductionMarginal
          , 'Deduction Extra Registered For Exam': item.DeductionExtraRegisteredForExam
          , 'Deduction Failed Trainees': item.DeductionFailedTrainees
          , 'Deduction Uniform Bag Receiving': item.DeductionUniformBagReceiving
          , 'Payment Withheld Since Inception UnV CNIC': item.PaymentWithheldSinIncepUnVCNIC
          , 'Penalty TPM Reports': item.PenaltyTPMReports
          , 'Penalty Imposed By MnE': item.PenaltyImposedByME
          , 'Reimbursement UnV Trainees': item.ReimbursementUnVTrainees
          , 'Reimbursement Attandance': item.ReimbursementAttandance
          // tslint:disable-next-line: max-line-length
          , 'Certification Cost Deduction (All Types)': item.ExtraTraineeDeductCompletion + item.UnVDeductCompletion + item.DropOutDeductCompletion + item.AbsentDeductCompletion
          , 'Payment To Be Released Trainees': item.PaymentToBeReleasedTrainees
          , 'Status': item.StatusApproved
          , 'Approval Date': this.datePipe.transform(item.FinalApprovalDate, 'dd/MM/yyyy')
          , 'Creation Date': this.datePipe.transform(item.CreationDate, 'dd/MM/yyyy')
        };
      });
    }
    else if (ProcessKey === EnumApprovalProcess.PRN_R) {
      return data.map(item => {
        return {
          'Project': item.FundingCategory
          ,'Scheme': item.SchemeName
          ,'TSP': item.TSPName
          ,'KAM': item.kam
          ,'PRN Month': this.datePipe.transform(item.Month, 'MM/yyyy')
           , 'Class Code': item.ClassCode
          , 'Invoice No': item.InvoiceNumber
          , Trade: item.TradeName
          , 'Certification Authority':item.CertAuthName
          , Duration: item.Duration
          , 'Class Start Date': this.datePipe.transform(item.ClassStartDate, 'dd/MM/yyyy')
          , 'Class End Date': this.datePipe.transform(item.ClassEndDate, 'dd/MM/yyyy')
          , 'Class Status': item.ClassStatus
          , 'Contractual Trainees': item.ContractualTrainees
          , 'Claimed Trainees': item.ClaimedTrainees
          , 'Enrolled Trainees': item.EnrolledTrainees
          , 'CNIC Verified': item.CNICVerified
          , 'CNIC Verified Excesses': item.CNICVExcesses
          , 'Dropouts Verified': item.DropoutsVerified
          , 'Expelled Verified': item.ExpelledVerified
          , 'CNIC Unverified': item.CNICUnverified
          , 'CNIC UnVerified Excesses': item.CNICUnVExcesses
          , 'Dropouts Unverified': item.DropoutsUnverified
          , 'Expelled UnVerified': item.ExpelledUnverified
          , 'NonFunctional Visit 1': item.NonFunctionalVisit1
          , 'NonFunctional Visit 2': item.NonFunctionalVisit2
          , 'NonFunctional Visit 3': item.NonFunctionalVisit3
          , 'NonFunctional Visit 1 Date': this.datePipe.transform(item.NonFunctionalVisit1Date, 'dd/MM/yyyy')
          , 'NonFunctional Visit 2 Date': this.datePipe.transform(item.NonFunctionalVisit2Date, 'dd/MM/yyyy')
          , 'NonFunctional Visit 3 Date': this.datePipe.transform(item.NonFunctionalVisit3Date, 'dd/MM/yyyy')
          , 'Deduction Since Inception Dropout': item.DeductionSinIncepDropout
          , 'Max Attendance': item.MaxAttendance
          , 'Payment Withheld Physical Count': item.PaymentWithheldPhysicalCount
          , 'Deduction Marginal': item.DeductionMarginal
          , 'Payment Withheld Since Inception UnV CNIC': item.PaymentWithheldSinIncepUnVCNIC
          , 'Penalty TPM Reports': item.PenaltyTPMReports
          , 'Penalty Imposed By MnE': item.PenaltyImposedByME
          , 'Reimbursement UnV Trainees': item.ReimbursementUnVTrainees
          , 'Reimbursement Attandance': item.ReimbursementAttandance
          , 'Expelled Regular Verified For The Month': (item.ExpelledRegularVerifiedForTheMonth)
          , 'Payment To Be Released Trainees': item.PaymentToBeReleasedTrainees
          , 'Status': item.StatusApproved
          , 'Approval Date': this.datePipe.transform(item.FinalApprovalDate, 'dd/MM/yyyy')
          , 'Creation Date': this.datePipe.transform(item.CreationDate, 'dd/MM/yyyy')
        };
      });
    }
  }

  getPTBRTrainees(c: any) {
    this.http.postJSON(`api/PRN/GetPTBRTrainees`, { ClassCode: c.ClassCode, Month: c.Month }).subscribe(
      (data: any) => {
        c.previousPTBRTrainees = data;
      }
      , (error) => {
        console.error(JSON.stringify(error));
      }
    );
  }

  openClassJourneyDialogue(data: any): void
  {
		this.dialogue.openClassJourneyDialogue(data);
  }

  getApprovalHistory(row,PRNMasterID,ProcessKey,Month) {
    this.PRNMonth=null;
    this.PRNMonth=Month;
    /// data object must have ProcessKey & FormID
    this.http.postJSON('api/Approval/GetApprovalHistory', {ProcessKey, FormID: PRNMasterID}).subscribe(
      (responseData: any) => {
        // debugger;
        if (responseData.length === 1 && responseData[0].ApproverIDs.indexOf(this.currentUser.UserID) !== -1) {
          row.PenaltyBtn = true;
          //this.PenaltyBtn=true
        }
      },
      (error) => {
        this.http.ShowError(error.error + '\n' + error.message);
      }
    );
  }


  openDialogPenaltyImposedByME(data:any): void {
    data.PRNMonth=this.PRNMonth;
    const dialogRef = this.dialog.open(PrnApprovalPenaltyComponent, {
      width: '400px',
      data: { data }
    });
    dialogRef.afterClosed().subscribe(result => {
      // debugger;
      if (result != undefined) {
        const PenaltyImposedByME = result.value.PenaltyImposedByME;
        const PenaltyAndUniBagRecvInputRemarks = result.value.PenaltyAndUniBagRecvInputRemarks;
        const DeductionUniformBagReceiving = result.value.DeductionUniformBagReceiving;

        this.http.postJSON('api/PRN/PenaltyImposedByMEAndDeductionUniformBag', { ClassID : data.ClassID,
          PRNID: data.PRNID, PenaltyImposedByME, PenaltyAndUniBagRecvInputRemarks,DeductionUniformBagReceiving})
          .subscribe((d: any) => {
            this.http.openSnackBar(environment.UpdateMSG.replace('${Name}', 'Penalty Imposed By ME'));
            this.GetPRNMasterForApproval();
            this.GetFiltersData();
          },
            error => this.error = error // error path
            , () => {
            }
        );
      }
    });
  }


}

export interface IPRNApprovalFilter {
  SchemeID: number;
  //TSPID: number;
  TSPMasterID: number;
  KAMID: number;
  StatusID: number;
}
