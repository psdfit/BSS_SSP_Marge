import { DateTime } from 'luxon';
import { Component, OnInit, AfterViewInit } from '@angular/core';
import { CommonSrvService } from '../../common-srv.service';
import { MatDialog } from '@angular/material/dialog';
import { Overlay } from '@angular/cdk/overlay';
import { DialogueService } from '../../shared/dialogue.service';
import { EnumApprovalProcess, EnumExcelReportType } from '../../shared/Enumerations';
import { DateAdapter, MAT_DATE_FORMATS, MAT_DATE_LOCALE } from '@angular/material/core';
import { MomentDateAdapter, MAT_MOMENT_DATE_ADAPTER_OPTIONS } from '@angular/material-moment-adapter';
import { Moment } from 'moment';
import * as _moment from 'moment';
import { MatDatepicker } from '@angular/material/datepicker';
import { FormControl } from '@angular/forms';
import { environment } from '../../../environments/environment';
import { trigger, transition, query, stagger, animate, style } from '@angular/animations';
import { ExportExcel } from 'src/app/shared/Interfaces';
import { DatePipe } from '@angular/common';

const moment = _moment;

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
  selector: 'app-trn-approvals',
  templateUrl: './trn-approvals.component.html',
  styleUrls: ['./trn-approvals.component.scss'],
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

export class TrnApprovalsComponent implements OnInit, AfterViewInit {
  environment = environment;
  // resultsLength: any;
  // tslint:disable-next-line: max-line-length
  // dtSRNDataDisplayedColumns = ['TSPName', 'TradeName', 'ClassCode', 'TraineeCode', 'TraineeName', 'FatherName', 'TraineeCNIC', 'DistrictName', 'ContactNumber1', 'TrainingAddressLocation', 'Comments', 'Batch', 'TokenNumber', 'TransactionNumber', 'Status', 'Month', 'NumberOfMonths', 'Action'];
  // dtSRNData: MatTableDataSource<any>;
  trn: any[];
  month = new FormControl(moment());
  Scheme = [];
  CertAuthDetail = [];

  filters: IFilters = { SchemeID: 0, CertAuthID: 0, UserID: 0, Month: ''};

  SearchSch = new FormControl('');
  SearchCertAuth = new FormControl('');
  error = '';

  currentUser: any;

  schemeFilter = new FormControl(0);
  certAuthFilter = new FormControl(0);
  TRNDetailsBulkArray: any;
  TRNMasterIDsArray: any[];
  TRNMasterIDs: string;

  constructor(private http: CommonSrvService, public dialog: MatDialog, private overlay: Overlay, private dialogue: DialogueService,
              private datePipe: DatePipe) {
    this.http.setTitle('Testing Recommendation Note');
  }

  ngOnInit(): void {
    this.currentUser = this.http.getUserDetails();
    this.http.OID.subscribe(() => {
      this.filters.SchemeID = 0;
      this.filters.CertAuthID = 0;
      this.filters.UserID = this.currentUser.UserID;
      this.filters.Month = moment(this.month.value).format('YYYY-MM-DD');
    })
    this.getSchemes();
    this.getCertificationAuthority();
  }
  ngAfterViewInit(): void {
    this.GetTRN();
    this.schemeFilter.valueChanges.subscribe(value => {
      this.filters.SchemeID = value;
      this.GetTRN();
    });
    this.certAuthFilter.valueChanges.subscribe(value => {
      this.filters.CertAuthID = value;
      this.GetTRN();
    });
  }

  AddTrn() {
    this.http.postJSON(`api/trn/add`, {})
      .subscribe(
        (d: any) => {
          console.log(d);
        }
        , (error) => {
          console.error(JSON.stringify(error));
        }
      );
  }

  GetTRN() {
    this.http.postJSON(`api/TRN/GetTRN/`,this.filters).subscribe(
      (data: any) => {
        this.trn = data;
        this.TRNMasterIDsArray = this.trn.map(o => o.TRNMasterID);
        this.TRNMasterIDs = this.TRNMasterIDsArray.join(',');
        console.table(this.trn);
      }
      , (error) => {
        console.error(JSON.stringify(error));
      }
    );
  }

  GetTrnDetails(r: any) {
    r.HasTRN = !r.HasTRN;
    if (r.trnDetails) {
      r.trnDetails = null;
      return;
    }

    this.http.getJSON('api/TRN/GetTRNDetails/' + r.TRNMasterID).subscribe(
      (data: any) => {
        r.trnDetails = data[0];
        r.HasTRN = true;
      }
      , (error) => {
        console.error(JSON.stringify(error));
      }
    );
  }
  /// ---Invoke Dialog---S--////
  openApprovalDialogue(row: any): void {
    // { ProcessKey: 'AP', FormID:  row.SrnId }
    // let datas: IApprovalHistory = { ProcessKey: 'AP', FormID: 292 };
    this.dialogue.openApprovalDialogue(EnumApprovalProcess.PRN_T, row.TRNMasterID).subscribe(result => { console.log(result); });
  }
  /// ---Invoke  Dialog---E--////
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
    this.filters.Month = moment(this.month.value).format('YYYY-MM-DD');
    this.GetTRN();
    datepicker.close();
  }

  clearMonth() {
    this.month = new FormControl(moment(null));
   // this.month.setValue(null);
   this.filters.Month = '';
    this.GetTRN();
  }

  openClassJourneyDialogue(data: any): void
  {
		this.dialogue.openClassJourneyDialogue(data);
  }

  getSchemes() {
    this.Scheme = [];
    this.filters.SchemeID = 0;
    this.filters.CertAuthID = 0;
    this.http.postJSON('api/Scheme/FetchSchemeByUser', this.filters).subscribe(
        (d: any) => {
          this.error = '';
          this.Scheme = d;
        }
        , (error) => {
          console.error(JSON.stringify(error));
        }
      );
  }
  getCertificationAuthority() {
    this.CertAuthDetail = [];
    this.http.getJSON(`api/CertificationAuthority/RD_CertificationAuthority`) .subscribe(
      (data: any) => {
        this.error = '';
        this.CertAuthDetail = data;
      }
      , (error) => {
        console.error(JSON.stringify(error));
      })
  }
  EmptyCtrl() {
    this.SearchCertAuth.setValue('');
    this.SearchSch.setValue('');
  }

  ExportToExcel(TRNMasterID: number, ProcessKey: string, month: DateTime) {
    let filteredData;
    this.http.postJSON('api/TRN/GetTRNExcelExport/', { TRNMasterID, month }).subscribe((d: any) => {
      filteredData = d;

      const headerData = {
        'Scheme Name ': filteredData[0]?.SchemeName,
        'Class Code(s) ': filteredData.map(x => x.ClassCode).join(','),
        'Invoice Type ': 'Testing',
        'Invoice Month ': this.datePipe.transform(month, 'dd/MM/yyyy'),
      }
      const exportExcel: ExportExcel = {
        Title: 'Testing Recommendation Note',
        Author: this.currentUser.FullName,
        Type: EnumExcelReportType.PRN_T,
        Data: headerData,
        List1: this.populateData(filteredData, ProcessKey),
      };
      this.dialogue.openExportConfirmDialogue(exportExcel).subscribe();
    });
  }

  GetByTRNMasterIDs() {
    this.http.postJSON('api/TRN/GetTRNBulkExcelExportByIDs', this.TRNMasterIDs).subscribe((d: any) => {
      this.TRNDetailsBulkArray = d;
      this.ExportToExcelBulkTRN();
    });

  }

  ExportToExcelBulkTRN() {

    const filteredData = this.TRNDetailsBulkArray;
    const exportExcel: ExportExcel = {
      Title: 'TRN Approval Report',
      Author: this.currentUser.FullName,
      Type: EnumExcelReportType.PRN_T,
      Data: {},
      List1: this.populateData(filteredData, '')
    };
    this.dialogue.openExportConfirmDialogue(exportExcel).subscribe();

  }

  populateData(data: any, _ProcessKey: string) {
    return data.map(item => {
      return {
        'Class Code': item.ClassCode
        , 'Class Duration' : item.Duration
        , 'Trade Name' : item.TradeName
        , 'Start Date' : this.datePipe.transform(item.ClassStartDate, 'dd/MM/yyyy')
        , 'End Date' : this.datePipe.transform(item.ClassEndDate, 'dd/MM/yyyy')
        , 'Contractual Trainees' : item.ContractualTrainees
        , 'Enrolled Trainees' : (item.EnrolledTrainees as number)
        , 'Trainees Registered for Exam' : (item.TraineesRegisteredForExam as number)
        , Pass : item.PassVerified + item.PassUnverified
        , Fail : item.FailedVerified + item.FailedUnverified
        , Absent : item.AbsentVerified + item.AbsentUnverified
        , 'Payment Released' : item.PaymentToBeReleased
      };
    });
  }

}
export interface IFilters {
  SchemeID?: number;
  CertAuthID?: number;
  UserID?: number;
  Month?: string;
}
