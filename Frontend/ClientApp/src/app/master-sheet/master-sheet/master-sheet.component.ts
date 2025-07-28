import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { CommonSrvService } from '../../common-srv.service';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { FormGroup, FormBuilder, Validators, FormControl } from '@angular/forms';
import { environment } from '../../../environments/environment';
import { ModelBase } from '../../shared/ModelBase';
import { UserRightsModel } from '../../master-data/users/users.component';
import { IMasterSheetFilter } from '../Interface/IMasterSheetFilter';
import { VisitPlanDialogComponent } from '../visit-plan-dialog/visit-plan-dialog.component';
import { UsersModel } from '../../master-data/users/users.component';
import { EnumUserLevel, EnumExcelReportType } from '../../shared/Enumerations';
import { DialogueService } from '../../shared/dialogue.service';
import { ExportExcel } from '../../shared/Interfaces';
import { GroupByPipe } from 'angular-pipes';
import { DatePipe } from '@angular/common';
import { merge } from 'rxjs';
import { startWith, switchMap } from 'rxjs/operators';
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
  selector: 'app-master-sheet',
  templateUrl: './master-sheet.component.html',
  styleUrls: ['./master-sheet.component.scss'],
  providers: [GroupByPipe, DatePipe,
    {
      provide: DateAdapter,
      useClass: MomentDateAdapter,
      deps: [MAT_DATE_LOCALE, MAT_MOMENT_DATE_ADAPTER_OPTIONS]
    },

    { provide: MAT_DATE_FORMATS, useValue: MY_FORMATS },
  ]
})
export class MasterSheetComponent implements OnInit, AfterViewInit {
  environment = environment;
  SearchSch = new FormControl('',);
  SearchCls = new FormControl('',);
  SearchTSP = new FormControl('',);
  TpmID = 17;
  LoggedInUserLevel: number;
  collection = [];
  p = 1;
  mastersheetform: FormGroup;
  title: string; savebtn: string;
  displayedColumns = ['SchemeCode',
    'Scheme', 'Batch', 'TSP',
    'Class', 'TradeGroup', 'Trade',
    'FundingSourceName', 'TrainingAddressLocation',
    'Province'
    , 'Tehsil', 'District', 'WhoIsDeliveringTraining',
    'Certification_Authority', 'RegistrationAuthorityName', 'ProgramFocusName', 'TraineesPerClass', 'Gender',
    'Duration', 'TotalTrainingHours', 'StartDate', 'EndDate', 'ClassStartTime', 'ClassEndTime',
    'InceptionReportDueOn', 'StudentProfileOverDueOn', 'CompletionReportDue',
    'InceptionReportReceived', 'InceptionReportDeliveredToTPM',
    'DateOfDeliveryToTPM', 'EnrolledTrainees',
    'TraineeProfilesReceived', 'TraineeProfileReceivedDate', 'TotalTraineeProfilesReceived',
    'RTP', 'ClassStatusName', 'CompletionReportStatus', 'Remarks',
    'ClassID_U', 'SchemeID_U', 'TSPID_U',
    'SchemeType', 'MinHoursPerMonth',
    'EmploymentInvoiceStatus',
    'Shift', 'Section',
    'Sector', 'OverallEmploymentCommitment', 'MinimumEducation', 'Organization', 'TradeCode',
    'Cluster', 'UserName', 'InstructorName', 'InstructorCNIC', 'TSPNTN', 'FundingCategoryName', 'IsDVV', 'TotalClassDays', 'DayNames', 'SourceOfCurriculum', 'PaymentSchedule',
    'Action'];
  displayedTPMColumns = ['Batch', 'ClassID', 'TrainingAddressLocation', 'TradeID', 'ProvinceID', 'TehsilID', 'DistrictID', 'Duration', 'StartDate', 'EndDate', 'SectorID', 'OverallEmploymentCommitment', 'MinimumEducation', 'OID',
    'UserID',
    'ClusterID', 'Action'];
  enumUserLevel = EnumUserLevel;
  mastersheet: MatTableDataSource<any>;
  // mastersheetList: MatTableDataSource<any>;
  mastersheetList: any[];
  District = [];
  Scheme = [];
  TSPDetail = [];
  Trade = [];
  Tehsil = [];
  Gender = [];
  Sector = [];
  Cluster = [];
  master = [];
  mastersheetArray = [];
  classesArray: any[];
  visitPlan: MatTableDataSource<any>;
  formrights: UserRightsModel;
  EnText = 'Master Sheet';
  error: string;
  query = {
    order: 'ID',
    limit: 10,
    page: 1
  };
  public employeedata = [];
  count = 5;
  filters: IMasterSheetFilter = {
    SchemeID: 0, ClassID: 0, TSPID: 0, UserID: 0, ClassStatusID: 0, // Class Status
    StartDate: '', // Start Date
    EndDate: '', // End Date
    FundingCategoryID: 0, // Project 
    KamID: 0 // KAM
  };
  @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: false }) sort: MatSort;
  working: boolean;
  currentUser: UsersModel;
  userid: number;
  isInternalUser = false;
  isTSPUser = false;
  // Pagination\\
  resultsLength: number;
  schemeFilter = new FormControl(0);
  tspFilter = new FormControl(0);
  classFilter = new FormControl(0);

  kamFilter = new FormControl(0);
  SearchKam = new FormControl('');
  Kam = [];

  fundingCategoryFilter = new FormControl();
  SearchFundingCategory = new FormControl('');
  Project: any[] = [];

  startDate = new FormControl(null);
  endDate = new FormControl(null);

  ClassStatus = [];
  SearchClassStatus = new FormControl('');
  ClassStatusFilter = new FormControl(0);



  constructor(private fb: FormBuilder, private ComSrv: CommonSrvService, public dialog: MatDialog,
    public dialogueService: DialogueService
    , private groupByPipe: GroupByPipe
    , private _date: DatePipe,) {
    // for (let i = 1; i <= 100; i++) {
    //  this.collection.push(`item ${i}`);
    // }
    this.mastersheetform = this.fb.group({
      ID: 0,
      DistrictID: '',
      DistrictName: '',
      ProvinceID: '',
      ProvinceName: '',
      SchemeID: ['', Validators.required],
      SchemeCode: ['', Validators.required],
      ClassID: '',
      ClassCode: ['', Validators.required],
      ClassStatusID: '',
      ClassStatusName: ['', Validators.required],
      Shift: ['', Validators.required],
      MinHoursPerMonth: ['', Validators.required],
      Duration: ['', Validators.required],
      TSPID: ['', Validators.required],
      TradeID: ['', Validators.required],
      TradeName: ['', Validators.required],
      TradeCode: ['', Validators.required],
      Batch: ['', Validators.required],
      TrainingAddressLocation: ['', Validators.required],
      TehsilID: ['', Validators.required],
      CertAuthID: ['', Validators.required],
      CertAuthName: ['', Validators.required],
      GenderID: ['', Validators.required],
      StartDate: ['', Validators.required],
      EndDate: ['', Validators.required],
      RTP: ['', Validators.required],
      ContractualClassHours: ['', Validators.required],
      EmploymentInvoiceStatus: ['', Validators.required],
      SectorID: ['', Validators.required],
      SectorName: ['', Validators.required],
      OverallEmploymentCommitment: ['', Validators.required],
      MinimumEducation: ['', Validators.required],
      OID: ['', Validators.required],
      OName: ['', Validators.required],
      ClusterID: ['', Validators.required],
      KamID: ['', Validators.required],
      UserID: ['', Validators.required],
      RoleID: ['', Validators.required],
      UserName: ['', Validators.required],
      TrainerName: ['', Validators.required],
      TrainerCNIC: ['', Validators.required],
      InActive: ''
    }, { updateOn: 'blur' });
    this.mastersheet = new MatTableDataSource([]);
    this.formrights = ComSrv.getFormRights();
  }
  EmptyCtrl() {
    this.SearchCls.setValue('');
    this.SearchTSP.setValue('');
    this.SearchSch.setValue('');
    this.SearchFundingCategory.setValue('');

  }

  openDialog(id: number): void {
    const dialogRef = this.dialog.open(VisitPlanDialogComponent, {
      height: '90%',
      data: { ClassID: id, IsMasterSheet: true, VisitDate: null }
    })
    dialogRef.afterClosed().subscribe(result => {
      this.visitPlan = result;
    })
  }
  openTPMDialog(level: number): void {
    const dialogRef = this.dialog.open(VisitPlanDialogComponent, {
      height: '90%',
      data: { level }
    })
    dialogRef.afterClosed().subscribe(result => {
      this.visitPlan = result;
    })
  }
  GetVisitPlanData(id: number) {
    this.ComSrv.postJSON('api/VisitPlan/RD_VisitPlanBy', { ClassID: id }).subscribe((response: any) => {
      // let VisitPlanData = <VisitPlanModel>response[0];
      this.visitPlan = new MatTableDataSource(response[0]);
      // this.classObj = response[1];
    });
  }
  getTSPDetailByScheme(schemeId: number) {
    this.classesArray = [];
    this.ComSrv.getJSON(`api/Dashboard/FetchTSPsByScheme?SchemeID=` + schemeId)
      .subscribe(data => {
        this.TSPDetail = (data as any[]);
      }, error => {
        this.error = error;
      })
  }
  getClassesByTsp(tspId: number) {
    this.ComSrv.getJSON(`api/Dashboard/FetchClassesByTSP?TspID=${tspId}`)
      .subscribe(data => {
        this.classesArray = (data as any[]);
      }, error => {
        this.error = error;
      })
  }
  // added KAM api endpoint:
  getKam() {
    this.ComSrv.getJSON('api/KAMAssignment/RD_KAMAssignmentForFilters').subscribe(
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
    this.ComSrv.getJSON(`api/Scheme/GetScheme?OID=${this.ComSrv.OID.value}`).subscribe((d: any) => {
      this.Project = d[4];
    },
      (error) => {
        this.error = error.error;
        this.ComSrv.ShowError(error.error + '\n' + error.message);
      } // error path
    );
  }

  GetClassStatus() {
    this.ComSrv.getJSON('api/ClassStatus/RD_ClassStatus').subscribe(
      (cs: any) => {
        this.ClassStatus = cs;
      },
      error => {
        this.error = error; // Error handling
        console.error('Error fetching ClassStatus:', error);
      }
    );
  }

  getMasterSheet() {

    const startDate = this.filters.StartDate ? this.filters.StartDate : '';
    const endDate = this.filters.EndDate ? this.filters.EndDate : '';

    const url = `api/MasterSheet/GetFilteredMasterSheet?schemeId=${this.filters.SchemeID}&tspId=${this.filters.TSPID}&classId=${this.filters.ClassID}&userId=${this.filters.UserID}&oId=${this.ComSrv.OID.value}&classStatusId=${this.filters.ClassStatusID}&fundingCategoryId=${this.filters.FundingCategoryID}&startDate=${encodeURIComponent(startDate)}&endDate=${encodeURIComponent(endDate)}&kamId=${this.filters.KamID}`;

    if (this.isTSPUser) {
      this.ComSrv.getJSON(url)
        .subscribe((data: any) => {
          this.mastersheetArray = data[0];
          this.Scheme = data[1];
          this.exportToExcel();
        },
          error => {
            this.error = error;
            console.error('Error fetching master sheet:', error);
          });
    } else {
      this.ComSrv.getJSON(url)
        .subscribe((data: any) => {
          this.mastersheetArray = data[0];
          this.Scheme = data[1];
          this.exportToExcel();
        },
          error => {
            this.error = error;
            console.error('Error fetching master sheet:', error);
          });
    }
  }
  ngOnInit() {
    this.ComSrv.setTitle('MasterSheet');
    this.title = 'Add New ';
    this.savebtn = 'Save ';
    this.currentUser = this.ComSrv.getUserDetails();
    this.userid = this.currentUser.UserID;
    if (this.currentUser.UserLevel === EnumUserLevel.TSP) {
      this.isTSPUser = true;
      this.filters.UserID = this.userid;
      this.displayedColumns = ['SchemeCode',
        'Scheme', 'Batch', 'TSP',
        'Class', 'TradeGroup', 'Trade',
        'TrainingAddressLocation', 'Province',
        'Tehsil', 'District', 'WhoIsDeliveringTraining',
        'Certification_Authority', 'TraineesPerClass', 'Gender',
        'Duration', 'TotalTrainingHours', 'StartDate', 'EndDate', 'ClassStartTime', 'ClassEndTime',
        'InceptionReportDueOn', 'StudentProfileOverDueOn', 'CompletionReportDue',
        'InceptionReportReceived',
        'EnrolledTrainees',
        'TraineeProfilesReceived', 'TraineeProfileReceivedDate', 'TotalTraineeProfilesReceived',
        'RTP', 'ClassStatusName', 'CompletionReportStatus', 'Remarks',
        'SchemeType', 'MinHoursPerMonth',
        'EmploymentInvoiceStatus',
        'Shift', 'Section',
        'Sector', 'OverallEmploymentCommitment', 'MinimumEducation', 'TradeCode',
        'Cluster', 'UserName', 'InstructorName', 'InstructorCNIC', 'Action'];
    } else if (this.currentUser.UserLevel === EnumUserLevel.AdminGroup || this.currentUser.UserLevel === EnumUserLevel.OrganizationGroup) {
      this.isInternalUser = true;

    }
    this.ComSrv.OID.subscribe(OID => {
      this.filters.SchemeID = 0;
      this.filters.TSPID = 0;
      this.filters.ClassID = 0;
      this.filters.UserID = 0;
      this.filters.ClassStatusID = 0;
      this.filters.StartDate = '';
      this.filters.FundingCategoryID = 0;
      this.filters.EndDate = '';
    });

    this.getKam();
    this.getFundingCategories();
    this.GetClassStatus();
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

  exportToExcel() {
    const filteredData = [...this.mastersheetArray]
    const data = {
      'Filters:': '',
      'Scheme(s)': this.groupByPipe.transform(filteredData, 'Scheme').map(x => x.key).join(','),
      'TSP(s)': this.groupByPipe.transform(filteredData, 'TSP').map(x => x.key).join(','),
      'KAM(s)': this.groupByPipe.transform(filteredData, 'UserName').map(x => x.key).join(','),
      'Project(s)': this.groupByPipe.transform(filteredData, 'FundingCategoryName').map(x => x.key).join(','),
      'Class Status Filter': this.groupByPipe.transform(filteredData, 'ClassStatusName').map(x => x.key).join(','),
      'Start Date': this.filters.StartDate || 'All',
      'End Date': this.filters.EndDate || 'All',
      Batch: 'All',
      Trade: 'All',
      'Certification Agency': 'All',
      Gender: 'All',
      'District of Training Location': 'All',
      'Class Status': 'All',
    };
    const exportExcel: ExportExcel = {
      Title: 'Master Sheet Report',
      Author: this.currentUser.FullName,
      Type: EnumExcelReportType.MasterSheet,
      Data: data,
      List1: this.populateData(filteredData),
    };
    this.dialogueService.openExportConfirmDialogue(exportExcel).subscribe();
  }
  populateData(data: any) {
    const dataWithLatestInstructorName = [...data];
    //dataWithLatestInstructorName.forEach(entry => {
    //  if (entry.InstructorName) {
    //    // Split the names by comma and take the last element
    //    const names = entry.InstructorName.split(',');
    //    entry.InstructorName = names[names.length - 1].trim();
    //  }
    //});
    return dataWithLatestInstructorName.map(item => {
      return {
        'Scheme Code': item.SchemeCode
        , Scheme: item.Scheme
        , Batch: item.Batch
        , TSP: item.TSP
        , 'Class Code': item.Class
        , 'Trade Group': item.TradeGroup
        , Trade: item.Trade
        , 'Funding Source': item.FundingSourceName
        , 'Training Location': item.TrainingAddressLocation
        , 'Tehsil of Training Center': item.Tehsil
        , 'District of Training Center': item.District
        , 'Province of Training Center': item.ProvinceName
        , 'Who is Delivering Training?': item.WhoIsDeliveringTraining
        , 'Testing/Certify Authority': item.Certification_Authority
        , 'Contractual Trainees per Class': item.TraineesPerClass
        , 'Class Gender Male/Female/Mix': item.Gender
        , 'Training Duration (Months)': item.Duration
        , 'Total Training Hours': item.TotalTrainingHours
        , 'Start Date': this._date.transform(item.StartDate, 'dd/MM/yyyy')
        , 'Completion Date': this._date.transform(item.EndDate, 'dd/MM/yyyy')
        , 'Class Start Time': this._date.transform(item.ClassStartTime, 'h:mm a')
        , 'Class End Time': this._date.transform(item.ClassEndTime, 'h:mm a')
        , 'Inception Report Due On': this._date.transform(item.InceptionReportDueOn, 'dd/MM/yyyy')
        , 'Student Profile Overdue on': this._date.transform(item.StudentProfileOverDueOn, 'dd/MM/yyyy')
        , 'Completion Report Due': this._date.transform(item.CompletionReportDue, 'dd/MM/yyyy')
        , 'Inception Report Received': item.InceptionReportReceived
        , 'Inception Report Delivered To TPM': item.InceptionReportDeliveredToTPM
        , 'Date of Delivery To TPM': item.DateOfDeliveryToTPM
        , 'Enrolled Trainees': item.EnrolledTrainees
        , 'Trainee Profiles Received': item.TraineeProfilesReceived
        , 'Trainee Profile Received Date': item.TraineeProfileReceivedDate
        , 'Total Trainee Profiles Received': item.TotalTraineeProfilesReceived
        , 'RTP Received': item.RTP
        , 'Class Status': item.ClassStatusName
        , 'Completion Report Status': item.CompletionReportStatus
        , Remarks: item.Remarks
        , ClassID: item.ClassID_U
        , 'Scheme ID': item.SchemeID_U
        , 'TSP ID': item.TSPID_U
        , 'Scheme Type': item.SchemeType
        , 'Contractual Class Hours': item.MinHoursPerMonth
        , EmploymentInvoiceStatus: item.EmploymentInvoiceStatus
        , Shift: item.Shift
        , Section: item.Section
        , Sector: item.Sector
        , 'Overall Employment Commitment': item.OverallEmploymentCommitment
        , 'Minimum Education': item.MinimumEducation
        , Organization: item.Organization
        , 'Trade Code': item.TradeCode
        , Cluster: item.Cluster
        , 'KAM': item.UserName
        , 'Instructor Name': item.InstructorName
        , 'Instructor CNIC': item.InstructorCNIC
        , 'TSP NTN': item.TSPNTN
        , 'Project': item.FundingCategoryName
        , 'Is DVV': item.IsDVV
        , 'No of Class Days': item.TotalClassDays
        , 'Days Name': item.DayNames
        , 'Source Of Curriculum': item.SourceOfCurriculum
        , 'Payment Schedule': item.PaymentSchedule
        , 'Registration Authority': item.RegistrationAuthorityName == "" ? '---' : item.RegistrationAuthorityName
        , 'Program Focus': item.ProgramFocusName
        , 'SAPID': item.SAPID
      }
    })
  }
  ngAfterViewInit() {
    this.ComSrv.OID.subscribe(
      OID => {
        this.initPagedData();
      });
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
            PageNo: this.paginator.pageIndex + 1
            , PageSize: this.paginator.pageSize
            , SortColumn: this.sort.active
            , SortOrder: this.sort.direction
            , SearchColumn: ''
            , SearchValue: ''
          };
          this.filters.SchemeID = this.schemeFilter.value || 0;
          this.filters.TSPID = this.tspFilter.value || 0;
          this.filters.ClassID = this.classFilter.value || 0;
          this.filters.KamID = this.kamFilter.value || 0;
          this.filters.ClassStatusID = this.ClassStatusFilter.value || 0;
          this.filters.FundingCategoryID = this.fundingCategoryFilter.value || 0;
          this.filters.StartDate = this.startDate.value ? this._date.transform(this.startDate.value, 'yyyy-MM-dd') : '';
          this.filters.EndDate = this.endDate.value ? this._date.transform(this.endDate.value, 'yyyy-MM-dd') : '';
          return this.getPagedData(pagedModel, this.filters);
        })).subscribe((data: any) => {
          const dataWithLatestInstructorName = [...data];
          //dataWithLatestInstructorName[0].forEach((entry: any) => {
          //  if (entry.InstructorName) {
          //    // Split the names by comma and take the last element
          //    const names = entry.InstructorName.split(',');
          //    entry.InstructorName = names[names.length - 1].trim();
          //  }
          //});
          this.mastersheet = new MatTableDataSource(dataWithLatestInstructorName[0]);

          this.Scheme = data[1];
          this.resultsLength = data[2].TotalCount;
        });
  }
  getPagedData(pagingModel, filterModel) {
    return this.ComSrv.postJSON('api/MasterSheet/GetFilteredMasterSheetPaged', { pagingModel, filterModel });
  }
  getDependantFilters() {
    if (this.currentUser.UserLevel === this.enumUserLevel.TSP) {
      this.getClassesBySchemeFilter();
    }
    else {
      this.getTSPDetailByScheme(this.filters.SchemeID);
    }
  }
  getClassesBySchemeFilter() {
    this.filters.ClassID = 0;
    this.ComSrv.getJSON(`api/Dashboard/FetchClassesBySchemeUser?SchemeID=${this.filters.SchemeID}&UserID=${this.userid}`)
      .subscribe(data => {
        this.classesArray = (data as any[]);
      }, error => {
        this.error = error;
      })
  }
  openClassJourneyDialogue(data: any): void {
    this.dialogueService.openClassJourneyDialogue(data);
  }
  get ID() { return this.mastersheetform.get('ID'); }
  get DistrictID() { return this.mastersheetform.get('DistrictID'); }
  get ProvinceID() { return this.mastersheetform.get('ProvinceID'); }
  get SchemeName() { return this.mastersheetform.get('SchemeName'); }
  get SchemeCode() { return this.mastersheetform.get('SchemeCode'); }
  get ClassCode() { return this.mastersheetform.get('ClassCode'); }
  get SchemeID() { return this.mastersheetform.get('SchemeID'); }
  get ClassID() { return this.mastersheetform.get('ClassID'); }
  get MinHoursPerMonth() { return this.mastersheetform.get('MinHoursPerMonth'); }
  get TSPID() { return this.mastersheetform.get('TSPID'); }
  get TradeID() { return this.mastersheetform.get('TradeID'); }
  get TradeCode() { return this.mastersheetform.get('TradeCode'); }
  get Batch() { return this.mastersheetform.get('Batch'); }
  get TrainingAddressLocation() { return this.mastersheetform.get('TrainingAddressLocation'); }
  get TehsilID() { return this.mastersheetform.get('TehsilID'); }
  get CertAuthID() { return this.mastersheetform.get('CertAuthID'); }
  get CertAuthName() { return this.mastersheetform.get('CertAuthName'); }
  get DeliveringTrainer() { return this.mastersheetform.get('DeliveringTrainer'); }
  get TestingCertifyAuthority() { return this.mastersheetform.get('TestingCertifyAuthority'); }
  get ContractualTrainees() { return this.mastersheetform.get('ContractualTrainees'); }
  get GenderID() { return this.mastersheetform.get('GenderID'); }
  get Duration() { return this.mastersheetform.get('Duration'); }
  get TotalTrainingHours() { return this.mastersheetform.get('TotalTrainingHours'); }
  get StartDate() { return this.mastersheetform.get('StartDate'); }
  get CompletionDate() { return this.mastersheetform.get('CompletionDate'); }
  get TotalTraineeProfilesReceived() { return this.mastersheetform.get('TotalTraineeProfilesReceived'); }
  get RTPReceived() { return this.mastersheetform.get('RTPReceived'); }
  get CompletionReportStatus() { return this.mastersheetform.get('CompletionReportStatus'); }
  get Remarks() { return this.mastersheetform.get('Remarks'); }
  get SchemeType() { return this.mastersheetform.get('SchemeType'); }
  get ContractualClassHours() { return this.mastersheetform.get('ContractualClassHours'); }
  get EmploymentInvoiceStatus() { return this.mastersheetform.get('EmploymentInvoiceStatus'); }
  get EmploymentCommitment() { return this.mastersheetform.get('EmploymentCommitment'); }
  get MinimumEducation() { return this.mastersheetform.get('MinimumEducation'); }
  get OID() { return this.mastersheetform.get('OID'); }
  get OName() { return this.mastersheetform.get('OName'); }
  get SectorID() { return this.mastersheetform.get('SectorID'); }
  get SectorName() { return this.mastersheetform.get('SectorName'); }
  get RoleID() { return this.mastersheetform.get('RoleID'); }
  get UserID() { return this.mastersheetform.get('UserID'); }
  get UserLevel() { return this.mastersheetform.get('UserLevel'); }
  get UserName() { return this.mastersheetform.get('UserName'); }
  get ClusterID() { return this.mastersheetform.get('ClusterID'); }
  get KamID() { return this.mastersheetform.get('KamID'); }
  get ClassStatusID() { return this.mastersheetform.get('ClassStatusID'); }
  get ClassStatusName() { return this.mastersheetform.get('ClassStatusName'); }
  get Shift() { return this.mastersheetform.get('Shift'); }

  get InActive() { return this.mastersheetform.get('InActive'); }
}
export class MasterSheetModel extends ModelBase {
  ID: number;
  DistrictID: number;
  DistrictName: string;
  ProvinceID: number;
  ProvinceName: string;
  SchemeID: number;
  SchemeName: string;
  SchemeCode: string;
  ClassCode: string;
  ClassID: number;
  ClassStatusName: string;
  ClassStatusID: number;
  Duration: number;
  TraineesPerClass: number;
  MinHoursPerMonth: number;
  CertAuthID: number;
  CertAuthName: string;
  RegistrationAuthorityName: string;
  ProgramFocusName: string;
  TSPID: number;
  TSPName: string;
  TradeID: number;
  TradeName: string;
  TradeCode: string;
  Batch: string;
  TrainingAddressLocation: string;
  TehsilID: number;
  TehsilName: string;
  GenderID: number;
  StartDate: string;
  EndDate: string;
  RTP: boolean;
  CompletionReportStatus: boolean;
  Remarks: string;
  SchemeType: number;
  ContractualClassHours: number;
  SectorID: number;
  SectorName: string;
  OverallEmploymentCommitment: string;
  MinimumEducation: string;
  OID: number;
  OName: string;
  ClusterID: number;
  ClusterName: string;
  UserID: number;
  RoleID: number;
  UserLevel: number;
  UserName: string;
  KamID: number;
  TSPNTN: string;
  FundingCategoryName: string;
  IsDVV: string;
  TotalClassDays: string;
  DayNames: string;
  SourceOfCurriculum: string;
  PaymentSchedule: string;
  SAPID: string;
}
