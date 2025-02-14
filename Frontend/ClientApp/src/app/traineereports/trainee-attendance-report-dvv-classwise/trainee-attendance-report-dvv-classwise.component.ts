import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from '../../../environments/environment';
import { MatPaginator } from '@angular/material/paginator';
import { SearchFilterTAR, ExportExcel } from '../../shared/Interfaces';
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
import * as moment from 'moment';
import { Moment } from 'moment';
import { MatDatepicker } from '@angular/material/datepicker';
import { MomentDateAdapter, MAT_MOMENT_DATE_ADAPTER_OPTIONS } from '@angular/material-moment-adapter';
import { DateAdapter, MAT_DATE_FORMATS, MAT_DATE_LOCALE } from '@angular/material/core';


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
  selector: 'app-trainee-attendance-report-dvv-classwise',
  templateUrl: './trainee-attendance-report-dvv-classwise.component.html',
  styleUrls: ['./trainee-attendance-report-dvv-classwise.component.scss'],
  providers: [GroupByPipe, DatePipe,
    {
      provide: DateAdapter,
      useClass: MomentDateAdapter,
      deps: [MAT_DATE_LOCALE, MAT_MOMENT_DATE_ADAPTER_OPTIONS]
    },

    { provide: MAT_DATE_FORMATS, useValue: MY_FORMATS },
  ]
})
export class TraineeAttendanceClassWiseComponent implements OnInit, AfterViewInit {
  environment = environment;
  tsrDatasource: any[];
  // displayedColumns = ['Sr', 'TraineeCode', 'TraineeName', 'FatherName', 'TraineeCNIC', 'GenderName', 'ContactNumber1', 'TraineeAge', 'ProvinceName', 'TraineeDistrictName', 'GuardianNextToKinName', 'TraineeStatusName', "ClassStatusName", "StartDate", "EndDate", "CertAuthName", "ReligionName", "Disability", "Dvv"];
  // displayedColumns = ['TraineeCode', 'TraineeName', 'TraineeCNIC', 'FatherName', 'SchemeName', 'ClassCode', 'CheckIn', 'CheckOut']; 
  displayedColumns = [
    'Scheme', 'TSP', 'ClassCode', 'District', 'ClassStartDate', 'ClassEndDate',
    'AttendanceDate', 'OnRollCompletedTraineesPresent', 'OnRollCompletedTraineesAbsent',
    'TotalOnRollCompletedTrainees', 'OnRollCompletedTraineesRatio',
  ];


  schemeArray = [];
  tspDetailArray = [];
  classesArray: any[];
  // filters: SearchFilter = { SchemeID: 0, TSPID: 0, ClassID: 0, TraineeID: 0, OID: this.commonService.OID.value, SelectedColumns: [] };
  filters: SearchFilterTAR = { SchemeID: 0, TSPID: 0, ClassID: 0, TraineeID: 0, Month: null, Year: null, OID: this.commonService.OID.value, SelectedColumns: [] };
  @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: false }) sort: MatSort;
  error = '';
  schemeFilter = new FormControl(0);
  tspFilter = new FormControl(0);
  classFilter = new FormControl(0);
  SearchSch = new FormControl('');
  SearchTSP = new FormControl('');
  SearchCls = new FormControl('');
  month = new FormControl(moment());
  resultsLength: number;
  currentUser: UsersModel;
  enumUserLevel = EnumUserLevel;
  TARDataModelKeys = Object.keys(new TARDataModel());

  constructor(
    private commonService: CommonSrvService,
    private datePipe: DatePipe,
    private groupByPipe: GroupByPipe,
    public dialogueService: DialogueService
  ) { }

  ngOnInit(): void {
    this.commonService.setTitle('Trainee Attendance Report Classwise');
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
      this.schemeFilter.setValue(0);
      this.tspFilter.setValue(0);
      this.classFilter.setValue(0);
      this.filters.OID = OID;
      this.getSchemes();
      this.initPagedData();
    });
  }

  EmptyCtrl(Ev: any) {
    this.SearchCls.setValue('');
    this.SearchTSP.setValue('');
    this.SearchSch.setValue('');
  }


  getSchemes() {
    this.commonService.postJSON('api/Scheme/FetchSchemeByUser', this.filters).subscribe(
      (d: any) => {
        this.schemeArray = d;
      }, error => this.error = error
    );
  }

  getData() {
    this.commonService.getJSON(`api/Scheme/FetchSchemeByUser`)
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

  getTSPDetailByScheme(schemeId: number) {
    this.tspFilter.setValue(0);
    this.classFilter.setValue(0);
    this.commonService.getJSON(`api/Dashboard/FetchTSPsByScheme?SchemeID=${schemeId}`)
      .subscribe(data => {
        this.tspDetailArray = (data as any[]);
      }, error => {
        this.error = error;
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
    this.initPagedData();
    datepicker.close();
  }
  clearMonth() {
    this.month = new FormControl(moment(null));
    // this.month.setValue(null);
    this.initPagedData();
  }
  getClassesByTsp(tspId: number) {
    this.classFilter.setValue(0);
    this.commonService.getJSON(`api/Dashboard/FetchClassesByTSP?TspID=${tspId}`)
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

    merge(
      this.sort.sortChange,
      this.paginator.page,
      this.schemeFilter.valueChanges,
      this.tspFilter.valueChanges,
      this.classFilter.valueChanges,
      this.month.valueChanges // Capture month filter changes
    ).pipe(
      startWith({}),
      switchMap(() => {
        const pagedModel = {
          PageNo: this.paginator.pageIndex + 1,
          PageSize: this.paginator.pageSize,
          SortColumn: this.sort.active,
          SortOrder: this.sort.direction,
          SearchColumn: '',
          SearchValue: ''
        };
        this.filters.SchemeID = this.schemeFilter.value;
        this.filters.TSPID = this.tspFilter.value;
        this.filters.ClassID = this.classFilter.value;

        // Include the month filter in the request
        const selectedMonth = this.month.value;
        if (selectedMonth) {
          this.filters.Month = selectedMonth.month() + 1; // JavaScript months are 0-based
          this.filters.Year = selectedMonth.year();
        } else {
          this.filters.Month = null;
          this.filters.Year = null;
        }

        return this.getPagedData(pagedModel, this.filters);
      })
    ).subscribe(data => {
      this.tsrDatasource = data[0];
      this.resultsLength = data[1].TotalCount; // Update the total count
    });
  }

  getPagedData(pagingModel, filterModel) {
    return this.commonService.postJSON('api/TSRLiveData/RD_TARPagedClasswise', { pagingModel, filterModel });
  }

  exportToExcel() {
    this.filters.SchemeID = this.schemeFilter.value;
    this.filters.TSPID = this.tspFilter.value;
    this.filters.ClassID = this.classFilter.value;

    const exportExcel: ExportExcel = {
      Title: 'Trainee Attendance Report Classwise',
      Author: this.currentUser.FullName,
      Type: EnumExcelReportType.TSR,
      Data: {},
      List1: [],
      ImageFieldNames: ['Trainee Img', 'CNIC Img'],
      SearchFilters: this.filters,
      DataModel: new TARDataModel(),
      LoadDataAsync: this.loadDataAsync
    };

    this.dialogueService.openExportConfirmDialogue(exportExcel).subscribe();
  }

  async loadDataAsync(that: any) {
    const payload = {
      pagingModel: { PageNumber: 0, PageSize: 0 }, // Add Paging Model if required
      filterModel: that.input.SearchFilters, // Ensure it matches the backend expectation
    };
    await that.commonService.postJSON(`api/TSRLiveData/RD_TARPagedClasswise`, payload)
      .toPromise()
      .then((responseData: any[]) => {
        that.input.Data = {
          'Filters:': '',
          TraineeStatus: 'All',
          'Scheme(s)': that.groupByPipe.transform(responseData[0], 'Scheme').map(x => x.key).join(','),
          'TSP(s)': that.groupByPipe.transform(responseData[0], 'TSP').map(x => x.key).join(','),
        };
        that.input.List1 = responseData[0].map((item, index) => {
          const obj = new TARDataModel();
          obj['Sr#'] = ++index;
          obj['Scheme'] = item.Scheme;
          obj['TSP'] = item.TSP;
          obj['Class Code'] = item.ClassCode;
          obj['District'] = item.District;
          obj['Class Start Date'] = that.datePipe.transform(item.ClassStartDate, 'dd/MM/yyyy');
          obj['Class End Date'] = that.datePipe.transform(item.ClassEndDate, 'dd/MM/yyyy');
          obj['Attendance Date'] = that.datePipe.transform(item.AttendanceDate, 'dd/MM/yyyy');
          obj['Total Trainees Per Class'] = item.TotalTraineesPerClass;
          obj['On-Roll Completed Trainees Present'] = item.OnRollCompletedTraineesPresent;
          obj['On-Roll Completed Trainees Absent'] = item.OnRollCompletedTraineesAbsent;
          obj['Total On-Roll Completed Trainees'] = item.TotalOnRollCompletedTrainees;
          obj['On-Roll Completed Trainees Ratio'] = item.OnRollCompletedTraineesRatio;
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

  getClassesBySchemeFilter() {
    this.filters.ClassID = 0;
    this.filters.TraineeID = 0;
    this.commonService.getJSON(`api/Dashboard/FetchClassesBySchemeUser?SchemeID=${this.schemeFilter.value}&UserID=${this.currentUser.UserID}`)
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

export class TARDataModel {
  'Sr#': any = 'Sr#';
  'Scheme': any = 'Scheme';
  'TSP': any = 'TSP';
  'Class Code': any = 'ClassCode';
  'District': any = 'District';
  'Class Start Date': any = 'ClassStartDate';
  'Class End Date': any = 'ClassEndDate';
  'Attendance Date': any = 'AttendanceDate';
  'Total Trainees Per Class': any = 'TotalTraineesPerClass';
  'On-Roll Completed Trainees Present': any = 'OnRollCompletedTraineesPresent';
  'On-Roll Completed Trainees Absent': any = 'OnRollCompletedTraineesAbsent';
  'Total On-Roll Completed Trainees': any = 'TotalOnRollCompletedTrainees';
  'On-Roll Completed Trainees Ratio': any = 'OnRollCompletedTraineesRatio';
}


