import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from '../../../environments/environment';
import { MatPaginator } from '@angular/material/paginator';
import { SearchFilter, SearchFilterMultiSelect, ExportExcel } from '../../shared/Interfaces';
import { MatSort } from '@angular/material/sort';
import { CommonSrvService } from '../../common-srv.service';
import { FormControl } from '@angular/forms';
import { merge } from 'rxjs';
import { startWith, switchMap } from 'rxjs/operators';
import { DatePipe } from '@angular/common';
import { GroupByPipe } from '../../pipes/GroupBy.pipe';
import { UsersModel } from '../../master-data/users/users.component';
import { EnumExcelReportType, EnumUserLevel } from '../../shared/Enumerations';
import { DialogueService } from '../../shared/dialogue.service';
import { MatDatepicker } from '@angular/material/datepicker';
import * as _moment from 'moment';
import { Moment } from 'moment';
import { DateAdapter, MAT_DATE_LOCALE, MAT_DATE_FORMATS } from '@angular/material/core';
import { MomentDateAdapter, MAT_MOMENT_DATE_ADAPTER_OPTIONS } from '@angular/material-moment-adapter';

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
  selector: 'app-trainee-status-report',
  templateUrl: './trainee-status-report.component.html',
  styleUrls: ['./trainee-status-report.component.scss'],
  providers: [GroupByPipe, DatePipe,
    {
      provide: DateAdapter,
      useClass: MomentDateAdapter,
      deps: [MAT_DATE_LOCALE, MAT_MOMENT_DATE_ADAPTER_OPTIONS]
    },

    { provide: MAT_DATE_FORMATS, useValue: MY_FORMATS },
  ]
})
export class TraineeStatusReportComponent implements OnInit, AfterViewInit {
  environment = environment;
  tsrDatasource: any[];
  displayedColumns = ['Sr', 'TraineeCode', 'TraineeName', 'FatherName', 'TraineeCNIC', 'GenderName', 'ContactNumber1', 'TraineeAge', 'ProvinceName', 'TraineeDistrictName', 'GuardianNextToKinName', 'TraineeStatusName', "ClassStatusName", "StartDate", "EndDate", "CertAuthName", "ReligionName", "Disability", "Accounttitle", "BankName", "IBANNumber", "Dvv"];
  schemeArray = [];
  tspDetailArray = [];
  classesArray: any[];
  // filters: SearchFilter = {
  //   SchemeID: 0, TSPID: 0, ClassID: 0, TraineeID: 0, OID: this.commonService.OID.value, SelectedColumns: [], ClassStatusID: 0, // Class Status
  //   StartDate: '', // Start Date
  //   EndDate: '', // End Date
  //   FundingCategoryID: 0, // Project 
  //   KamID: 0
  // };
  // Modified: Changed SchemeID, TSPID, ClassID to arrays for multi-select support
  filters: SearchFilterMultiSelect = {
    Schemes: [],
    TSPs: [],
    Classes: [],
    TraineeID: 0,
    OID: this.commonService.OID.value,
    SelectedColumns: [],
    ClassStatusID: 0, // Class Status
    StartDate: '', // Start Date
    EndDate: '', // End Date
    FundingCategoryID: 0, // Project 
    KamID: 0
  };
  @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: false }) sort: MatSort;
  error = '';
  // Modified: Initialized as array for multi-select
  schemeFilter = new FormControl([]);
  tspFilter = new FormControl([]);
  classFilter = new FormControl([]);
  SearchSch = new FormControl('');
  SearchTSP = new FormControl('');
  SearchCls = new FormControl('');
  resultsLength: number;
  currentUser: UsersModel;
  enumUserLevel = EnumUserLevel;
  TSRDataModelKeys = Object.keys(new TSRDataModel());

  kamFilter = new FormControl(0);
  SearchKam = new FormControl('');
  Kam = [];

  fundingCategoryFilter = new FormControl();
  SearchFundingCategory = new FormControl('');
  Project: any[] = [];

  startDate = new FormControl();
  endDate = new FormControl();

  ClassStatus = [];
  SearchClassStatus = new FormControl('');
  ClassStatusFilter = new FormControl(0);


  constructor(
    private commonService: CommonSrvService,
    private datePipe: DatePipe,
    private groupByPipe: GroupByPipe,
    public dialogueService: DialogueService
  ) { }

  ngOnInit(): void {
    this.commonService.setTitle('Trainee Status Report');
    this.currentUser = this.commonService.getUserDetails();

    this.schemeFilter.valueChanges.subscribe(value => {
      if (this.currentUser.UserLevel === this.enumUserLevel.TSP) {
        this.getDependantFilters();
      } else {
        this.getTSPDetailByScheme(value);
      }
    });
    this.tspFilter.valueChanges.subscribe(value => { this.getClassesByTsp(value) });
  }

  ngAfterViewInit() {
    this.commonService.OID.subscribe(OID => {
      this.schemeFilter.setValue([]);
      this.tspFilter.setValue([]);
      this.classFilter.setValue([]);
      this.filters.OID = OID;
      this.getData();
      this.initPagedData();
      this.getKam();
      this.getFundingCategories();
      this.GetClassStatus();
    });
  }


  // added KAM api endpoint:
  getKam() {
    this.commonService.getJSON('api/KAMAssignment/RD_KAMAssignmentForFilters').subscribe(
      (d: any) => {
        this.error = '';
        this.Kam = d;
      },
      error => {
        this.error = error;
      }
    );
  }
  // added Project api endpoint:
  getFundingCategories() {
    this.commonService.getJSON(`api/Scheme/GetScheme?OID=${this.commonService.OID.value}`).subscribe((d: any) => {
      this.Project = d[4];
    },
      (error) => {
        this.error = error.error;
        this.commonService.ShowError(error.error + '\n' + error.message);
      } // error path
    );
  }

  // added classstatus api endpoint:
  GetClassStatus() {
    this.commonService.getJSON('api/ClassStatus/RD_ClassStatus').subscribe(
      (cs: any) => {
        this.ClassStatus = cs;
        console.log('Class Status Data:', cs); // Improved logging
      },
      error => {
        this.error = error; // Error handling
        console.error('Error fetching ClassStatus:', error);
      }
    );
  }

  chosenYearHandlerForStartDate(normalizedYear: Moment) {
    const ctrlValue = this.startDate.value;
    ctrlValue.year(normalizedYear.year());
    this.startDate.setValue(ctrlValue);
  }
  chosenMonthHandlerForStartDate(normalizedMonth: Moment, datepicker: MatDatepicker<Moment>) {
    const ctrlValue = this.startDate.value;
    ctrlValue.month(normalizedMonth.month());
    this.startDate.setValue(ctrlValue);

    datepicker.close();
  }

  chosenYearHandlerForEndDate(normalizedYear: Moment) {
    const ctrlValue = this.endDate.value;
    ctrlValue.year(normalizedYear.year());
    this.endDate.setValue(ctrlValue);
  }
  chosenMonthHandlerForEndDate(normalizedMonth: Moment, datepicker: MatDatepicker<Moment>) {
    const ctrlValue = this.endDate.value;
    ctrlValue.month(normalizedMonth.month());
    this.endDate.setValue(ctrlValue);
    datepicker.close();
  }


  EmptyCtrl(Ev: any) {
    this.SearchCls.setValue('');
    this.SearchTSP.setValue('');
    this.SearchSch.setValue('');
    this.SearchFundingCategory.setValue('');
  }

  getData() {
    this.commonService.getJSON(`api/TSRLiveData/GetSchemesForTSR?OID=${this.commonService.OID.value}`)
      .subscribe((d: any) => {
        this.schemeArray = d.Schemes;
      }, error => this.error = error);
  }

  getSchemesByOrg(oid: number) {
    this.commonService.getJSON(`api/Dashboard/FetchSchemes`)
      .subscribe(data => {
        this.schemeArray = (data as any[]);
      }, error => {
        this.error = error;
      });
  }

  // Modified: Updated to handle array of scheme IDs
  getTSPDetailByScheme(schemeIds: number[]) {
    this.tspFilter.setValue([]);
    this.classFilter.setValue([]);
    const schemeIdParam = schemeIds.length > 0 ? schemeIds.join(',') : '0';
    this.commonService.getJSON(`api/Dashboard/FetchTSPsByMultipleSchemes?SchemeID=${schemeIdParam}`)
      .subscribe(data => {
        this.tspDetailArray = (data as any[]);
      }, error => {
        this.error = error;
      });
  }

  // Modified: Updated to handle array of TSP IDs
  getClassesByTsp(tspIds: number[]) {
    this.classFilter.setValue([]);
    const tspIdParam = tspIds.length > 0 ? tspIds.join(',') : '0';
    this.commonService.getJSON(`api/Dashboard/FetchMultipleClassesByTSP?TspID=${tspIdParam}`)
      .subscribe(data => {
        this.classesArray = (data as any[]);
      }, error => {
        this.error = error;
      });
  }

  applyFilter(filterValue: string) {
    filterValue = filterValue.trim().toLowerCase();
    // this.tsrDatasource.filter = filterValue;
  }

  initPagedData() {
    this.sort.sortChange.subscribe(() => this.paginator.pageIndex = 0);
    this.paginator.pageSize = 10;
    merge(this.sort.sortChange, this.paginator.page, this.schemeFilter.valueChanges,
      this.tspFilter.valueChanges, this.classFilter.valueChanges, this.kamFilter.valueChanges,
      this.ClassStatusFilter.valueChanges, this.startDate.valueChanges,
      this.endDate.valueChanges, this.fundingCategoryFilter.valueChanges)
      .pipe(
        startWith({}),
        switchMap(() => {
          const pagedModel = {
            PageNo: this.paginator.pageIndex + 1,
            PageSize: this.paginator.pageSize,
            SortColumn: this.sort.active,
            SortOrder: this.sort.direction,
            SearchColumn: '',
            SearchValue: '',
          };
          // Modified: Assigned array values
          this.filters.Schemes = this.schemeFilter.value || [];
          this.filters.TSPs = this.tspFilter.value || [];
          this.filters.Classes = this.classFilter.value || [];
          this.filters.KamID = this.kamFilter.value || 0;
          this.filters.ClassStatusID = this.ClassStatusFilter.value || 0;
          this.filters.FundingCategoryID = this.fundingCategoryFilter.value || 0;
          this.filters.StartDate = this.startDate.value ? this.datePipe.transform(this.startDate.value, 'yyyy-MM-dd') : '';
          this.filters.EndDate = this.endDate.value ? this.datePipe.transform(this.endDate.value, 'yyyy-MM-dd') : '';
          return this.getPagedData(pagedModel, this.filters);
        })
      ).subscribe(data => {
        this.tsrDatasource = data[0];
        this.resultsLength = data[1].TotalCount;
      });
  }

  getPagedData(pagingModel, filterModel) {
    // Added: Convert array filters to comma-separated strings for backend compatibility
    const modFilter = {
      ...filterModel,
      Schemes: Array.isArray(filterModel.Schemes) ? filterModel.Schemes.join(',') : '0',
      TSPs: Array.isArray(filterModel.TSPs) ? filterModel.TSPs.join(',') : '0',
      Classes: Array.isArray(filterModel.Classes) ? filterModel.Classes.join(',') : '0'
    };
    return this.commonService.postJSON('api/TSRLiveData/RD_TSRPaged', { pagingModel, filterModel: modFilter });
  }

  exportToExcel() {
    // Modified: Assigned array values and converted to strings where needed
    this.filters.Schemes = this.schemeFilter.value || [];
    this.filters.TSPs = this.tspFilter.value || [];
    this.filters.Classes = this.classFilter.value || [];
    this.filters.KamID = this.kamFilter.value || 0;
    this.filters.ClassStatusID = this.ClassStatusFilter.value || 0;
    this.filters.FundingCategoryID = this.fundingCategoryFilter.value || 0;
    this.filters.StartDate = this.startDate.value ? this.datePipe.transform(this.startDate.value, 'yyyy-MM-dd') : '';
    this.filters.EndDate = this.endDate.value ? this.datePipe.transform(this.endDate.value, 'yyyy-MM-dd') : '';

    const exportExcel: ExportExcel = {
      Title: 'Trainee Status Report',
      Author: this.currentUser.FullName,
      Type: EnumExcelReportType.TSR,
      Data: {},
      List1: [],
      ImageFieldNames: ['Trainee Img', 'CNIC Img'],
      SearchFilters: this.filters,
      DataModel: new TSRDataModel(),
      LoadDataAsync: this.loadDataAsync
    };

    this.dialogueService.openExportConfirmDialogue(exportExcel).subscribe();
  }

  async loadDataAsync(that: any) {
    // Added: Convert array filters to comma-separated strings before API call
    const modFilters = {
      ...(that.input.SearchFilters || {}),
      Schemes: Array.isArray(that.input.SearchFilters?.Schemes)
        ? (that.input.SearchFilters.Schemes.length > 0
          ? that.input.SearchFilters.Schemes.join(',')
          : '0')
        : '0',
      TSPs: Array.isArray(that.input.SearchFilters?.TSPs)
        ? (that.input.SearchFilters.TSPs.length > 0
          ? that.input.SearchFilters.TSPs.join(',')
          : '0')
        : '0',
      Classes: Array.isArray(that.input.SearchFilters?.Classes)
        ? (that.input.SearchFilters.Classes.length > 0
          ? that.input.SearchFilters.Classes.join(',')
          : '0')
        : '0'
    };

    await that.commonService.postJSON(`api/TSRLiveData/GetFilteredTSRData`, modFilters)
      .toPromise()
      .then((responseData: any[]) => {
        that.input.Data = {
          'Filters:': '',
          TraineeStatus: 'All',
          'Scheme(s)': that.groupByPipe.transform(responseData, 'SchemeName').map(x => x.key).join(','),
          'TSP(s)': that.groupByPipe.transform(responseData, 'TSPName').map(x => x.key).join(','),
          TraineeImagesAdded: true
        };
        that.input.List1 = responseData.map((item, index) => {
          const obj = new TSRDataModel();
          obj['Sr#'] = ++index;
          obj.Scheme = item.SchemeName;
          obj['Training Service Provider'] = item.TSPName;
          obj['Trade Group'] = item.SectorName;
          obj.Trade = item.TradeName;
          obj.ClassCode = item.ClassCode;
          obj['Trainee ID'] = item.TraineeCode;
          obj['Trainee Name'] = item.TraineeName;
          obj['Father\'s Name'] = item.FatherName;
          obj['CNIC Issue Date'] = that.datePipe.transform(item.CNICIssueDate, 'dd/MM/yyyy');
          obj.CNIC = item.TraineeCNIC;
          obj['Date Of Birth'] = that.datePipe.transform(item.DateOfBirth, 'dd/MM/yyyy');
          obj['Roll #'] = item.TraineeRollNumber;
          obj.Batch = item.Batch;
          obj.Section = item.SectionName;
          obj.Shift = item.Shift === '1st' ? 'Morning' : 'Evening';
          obj['Trainee Address'] = `${item.TraineeHouseNumber}, ${item.TraineeStreetMohalla}, ${item.TraineeMauzaTown}, ${item.TraineeTehsilName}, ${item.TraineeDistrictName}`;
          obj['Residence Tehsil'] = item.TraineeTehsilName;
          obj['District of Residence'] = item.TraineeDistrictName;
          obj['Province of Residence'] = item.ProvinceName;
          obj.Gender = item.GenderName;
          obj.Education = item.Education;
          obj['Contact Number'] = item.ContactNumber1;
          obj['Training Location'] = `${item.TrainingAddressLocation}`;
          obj['District of Training Location'] = item.ClassDistrictName;

          obj['CNIC Verified'] = item.TraineeVerified ? 'Yes' : 'No';
          obj['Trainee Status'] = item.TraineeStatusName;
          obj['Is Dual'] = item.IsDual ? 'Yes' : 'No';
          obj['Trainee Status Update Date'] = that.datePipe.transform(item.TraineeStatusChangeDate, 'dd/MM/yyyy');
          obj['Examination Assesment'] = item.ResultStatusName;
          obj['Voucher Holder'] = item.VoucherHolder ? 'Yes' : 'No';
          obj.Reason = item.TraineeStatusChangeReason;
          obj['Class ID'] = item.ClassUID;
          obj['Trainee Profile ID'] = item.TraineeUID;
          obj['CNIC IMG Status'] = item.IsVarifiedCNIC ? 1 : 0;
          obj['Trainee Img'] = item.TraineeImg;
          obj['CNIC Img'] = item.CNICImgNADRA;
          obj.Sector = item.SectorName;

          obj.Cluster = item.ClusterName;
          obj.KAM = item.KAM;
          obj['Class Status'] = item.ClassStatusName;
          obj['Class Start Date'] = that.datePipe.transform(item.StartDate, 'dd/MM/yyyy');
          obj['Class End Date'] = that.datePipe.transform(item.EndDate, 'dd/MM/yyyy');
          obj['Certify Authority'] = item.CertAuthName;
          obj['Religion'] = item.ReligionName;
          obj['Disability'] = item.Disability;
          obj['Dvv'] = item.Dvv;
          obj['Trainee Employment Status'] = item.TraineeEmploymentStatus;
          obj['Trainee Employment Verification Status'] = item.TraineeEmploymentVerificationStatus;
          obj['Trainee Email'] = item.TraineeEmail;
          obj['AccountTitle'] = item.Accounttitle;
          obj['BankName'] = item.BankName;
          obj['IBANNumber'] = item.IBANNumber;
          return obj;

        });
      });
  }

  getDependantFilters() {
    if (this.currentUser.UserLevel === this.enumUserLevel.TSP) {
      this.getClassesBySchemeFilter();
    } else {
      this.getTSPDetailByScheme(this.filters.SchemeID);
    }
  }

  // Modified: Updated to handle array of scheme IDs
  getClassesBySchemeFilter() {
    // this.filters.ClassID = [];
    const schemeIdParam = this.schemeFilter.value.length > 0 ? this.schemeFilter.value.join(',') : '0';
    this.commonService.getJSON(`api/Dashboard/FetchClassesByMultipleSchemeUser?SchemeID=${schemeIdParam}&UserID=${this.currentUser.UserID}`)
      .subscribe(data => {
        this.classesArray = (data as any[]);
      }, error => {
        this.error = error;
      });
  }

  openTraineeJourneyDialogue(data: any): void {
    this.dialogueService.openTraineeJourneyDialogue(data);
  }
}

export class TSRDataModel {
  'Sr#': any = 'Sr';
  'Scheme': any = 'SchemeName';
  'Training Service Provider': any = 'TSPName';
  'Trade Group': any = 'SectorName';
  'Trade': any = 'TradeName';
  'ClassCode': any = 'ClassCode';
  'Trainee ID': any = 'TraineeCode';
  'Trainee Name': any = 'TraineeName';
  'Father\'s Name': any = 'FatherName';
  'CNIC Issue Date': any = 'CNICIssueDate'
  'CNIC': any = 'TraineeCNIC';
  'Date Of Birth': any = 'DateOfBirth'
  'Roll #': any = 'TraineeRollNumber';
  'Batch': any = 'Batch';
  'Section': any = 'SectionName';
  'Shift': any = 'Shift';
  'Trainee Address': any = 'TraineeHouseNumber,TraineeStreetMohalla,TraineeMauzaTown,TraineeTehsilName,TraineeDistrictName';
  'Residence Tehsil': any = 'TraineeTehsilName';
  'District of Residence': any = 'TraineeDistrictName';
  'Province of Residence': any = 'ProvinceName';
  'Gender': any = 'GenderName';
  'Education': any = 'Education';
  'Contact Number': any = 'ContactNumber1';
  'Training Location': any = 'TrainingAddressLocation';
  'District of Training Location': any = 'ClassDistrictName';
  'CNIC Verified': any = 'TraineeVerified';
  'Trainee Status': any = 'TraineeStatusName';
  'Is Dual': any = 'IsDual';
  'Trainee Status Update Date': any = 'TraineeStatusChangeDate';
  'Examination Assesment': any = 'ResultStatusName';
  'Voucher Holder': any = 'VoucherHolder';
  'Reason': any = 'TraineeStatusChangeReason';
  'Class ID': any = 'ClassUID';
  'Trainee Profile ID': any = 'TraineeUID';
  'CNIC IMG Status': any = 'IsVarifiedCNIC';
  'Trainee Img': any = 'TraineeImg';
  'CNIC Img': any = 'CNICImgNADRA';
  'Sector': any = 'SectorName';
  'Cluster': any = 'ClusterName';
  'KAM': any = 'KAM';
  'Trainee Employment Status': any = 'TraineeEmploymentStatus';
  'Trainee Employment Verification Status': any = 'TraineeEmploymentVerificationStatus';
  'Trainee Email': any = 'TraineeEmail';
  'Class Status': any = 'ClassStatusName';
  'Class Start Date': any = 'StartDate';
  'Class End Date': any = 'EndDate';
  'Certify Authority' = 'CertAuthName';
  'Religion' = 'ReligionName';
  'Disability': any = 'Disability';
  'AccountTitle': any = 'Accounttitle';
  'BankName': any = 'BankName';
  'IBANNumber': any = 'IBANNumber';
  'Dvv': any = 'Dvv';
}
