//import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Component, OnInit, ViewChild, Inject, HostListener } from '@angular/core';
import { environment } from '../../../environments/environment';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { FormGroup, FormBuilder, Validators, FormGroupDirective, NgForm, FormControl } from '@angular/forms';
import { CommonSrvService } from '../../common-srv.service';
import { ModelBase } from '../../shared/ModelBase';
import { UserRightsModel, UsersModel } from '../../master-data/users/users.component';
import { ActivatedRoute } from '@angular/router';
import * as moment from 'moment';
import { DateTime } from 'luxon';
import { EnumProgramCategory, EnumApprovalProcess } from '../../shared/Enumerations';
import { merge } from 'rxjs';
import { startWith, switchMap } from 'rxjs/operators';
import { DialogueService } from 'src/app/shared/dialogue.service';

@Component({
  selector: 'app-change-request',
  templateUrl: './change-request.component.html',
  styleUrls: ['./change-request.component.scss']
})
export class ChangeRequestDialogComponent implements OnInit {
  environment = environment;
  // @HostListener('document:keydown.enter', ['$event'])

  TraineeDisplayImg: any = "";
  SearchSch = new FormControl('',);
  SearchCls = new FormControl('',);
  SearchTrainee = new FormControl('',);
  SearchTradeList = new FormControl('',);
  SearchDistrict = new FormControl('',);
  SearchTehsil = new FormControl('',);

  changerequestSchemeform: FormGroup;
  changerequestTSPform: FormGroup;
  changerequestClassform: FormGroup;
  changerequestClassDatesform: FormGroup;
  changerequestTraineeform: FormGroup;
  changerequestInstructorform: FormGroup;
  changerequestInceptionReportform: FormGroup;
  changerequestReplaceInstructorform: FormGroup;
  addInstructorform: FormGroup;
  title: string;
  savebtn: string;
  alert: string;
  displayedColumnsScheme = ["Action", 'SchemeName', 'SchemeCode', 'BusinessRuleType', 'Stipend', 'StipendMode', 'UniformAndBag'];
  displayedColumnsTSP = ['Sr#', 'TSPName', 'Address', 'HeadName', 'HeadDesignation', 'HeadLandline', 'HeadEmail', 'CPName', 'CPDesignation', 'CPLandline', 'CPEmail',
    //'BankName',
    'BankAccountNumber', 'AccountTitle',
    //'BankBranch',
    "CrTSPStatus"];

  displayedColumnsClass = ['ClassCode', 'TrainingAddressLocation', 'CrClassLocationStatus', "Action"];
  displayedColumnsClassDates = ['ClassCode', 'StartDate', 'EndDate', 'Duration', 'CrClassDatesStatus', "Action"];

  displayedColumnsTrainee = ['TraineeName', 'FatherName', 'TraineeCNIC', 'TraineeEmail', 'DateOfBirth', 'VerificationStatus'
    , "CrStatus"
    , "Action"];
  displayedColumnsInstructor = ['InstructorName', 'TradeName', 'InstructorCNIC'
    , 'Status'
    , "Action"];

  displayedColumnsNewInstructor = ['Sr#', 'InstructorName', 'TradeName', 'InstructorCNIC', 'ClassCode', 'NewInstructorCRComments', 'NewInstructorCrStatus'
  ];
  displayedColumnsInceptionReport = ['ClassCode', 'TradeName', 'ClassStartTime', 'ClassEndTime'
    , 'InceptionCrStatus'
    , "Action"];

  scheme: MatTableDataSource<any>;
  tsp: MatTableDataSource<any>;
  tspChangeRequests: MatTableDataSource<any>;
  class: MatTableDataSource<any>;
  classArray: MatTableDataSource<any>;
  classArrayForDateChange: MatTableDataSource<any>;
  trainee: MatTableDataSource<any>;
  instructor: MatTableDataSource<any>;
  inceptionreport: MatTableDataSource<any>;
  classesForTimeChange: MatTableDataSource<any>;
  newInstructorsCRs: MatTableDataSource<any>;
  classObj: any;
  oldInstr: any;
  oldInstrCNICs: any;

  classDuration: any;

  filters: IChangeRequestTraineeListFilter = { SchemeID: 0, ClassID: 0, TraineeID: 0, TradeID: 0 };


  formrights: UserRightsModel;
  currentUser: UsersModel;

  classTehsils: []; classDistricts: []; tspDistricts: [];
  trades: []; tradedetailmap: []; education: []; certificationAgency: []; clusters: [];
  districts: []; tehsils: []; traineetehsils: []; genders: []; Instructors: []; trade: []; tradesArray: [];
  SchemeFilter: [];
  TSPDetailFilter: any[];
  classesArrayFilter: any[];
  classesArrayForNewTrainer: any[];
  activeClassesArrayFilter: any[];
  activeClassesArrayForNewTrainer: any[];
  traineeFilter: any[];
  ActiveClassTiming: any;
  doOverlap: Boolean = true;
  GetStartDateI: string;
  GetEndDateI: string;
  GetCurrentStartDateI: Date;
  GetCurrentEndDateI: Date;
  EnTextScheme: string = "Scheme Change Request";
  EnTextTSP: string = "TSP Change Request";
  EnTextClass: string = "Class Change Request";
  EnTextTrainee: string = "Trainee Change Request";
  EnTextInstructor: string = "Instructor Change Request";
  EnTextNewInstructor: string = "Instructor Request";
  EnTextInceptionReport: string = "Inception Report Change Request";
  EnTextInstructorReplacement: string = "Instructor Replacement Change Request";
  userid: number;
  tradeid: number;
  newInstructorAddress: string;
  MinClassHoursPerMonth: number;
  error: String;
  add: String;
  PreTraineeEmail: String;
  TimeDifference: number;
  Verified: boolean;
  InvalidInstructorFile: boolean;
  CheckTraineecCnic: boolean = false;
  query = {
    order: 'ChangeRequestID',
    limit: 5,
    page: 1
  };
  @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: false }) sort: MatSort;
  @ViewChild('SortScheme') SortScheme: MatSort;
  @ViewChild('PageScheme') PageScheme: MatPaginator;
  @ViewChild('SortTSP') SortTSP: MatSort;
  @ViewChild('PageTSP') PageTSP: MatPaginator;
  @ViewChild('SortClass') SortClass: MatSort;
  @ViewChild('PageClass') PageClass: MatPaginator;
  @ViewChild('SortTrainee') SortTrainee: MatSort;
  @ViewChild('PageTrainee') PageTrainee: MatPaginator;
  @ViewChild('SortInstructor') SortInstructor: MatSort;
  @ViewChild('PageInstructor') PageInstructor: MatPaginator;
  @ViewChild('SortClassesForTimeChange') SortClassesForTimeChange: MatSort;
  @ViewChild('PageClassesForTimeChange') PageClassesForTimeChange: MatPaginator;
  @ViewChild('SortClassesForDatesChange') SortClassesForDatesChange: MatSort;
  @ViewChild('PageClassesForDatesChange') PageClassesForDatesChange: MatPaginator;
  @ViewChild('SortInceptionReport') SortInceptionReport: MatSort;
  @ViewChild('PageInceptionReport') PageInceptionReport: MatPaginator;



  @ViewChild('tabGroup') tabGroup;
  working: boolean;

  cnicExists: boolean;
  maskCNIC = {
    mask: '00000-0000000-0'
  }

  constructor(private fb: FormBuilder,
    private ComSrv: CommonSrvService, public dialogueService: DialogueService
    //public dialogRef: MatDialogRef<ChangeRequestDialogComponent>,
    //@Inject(MAT_DIALOG_DATA) public data: any

  ) {

    this.changerequestSchemeform = this.fb.group({
      SchemeChangeRequestID: 0,
      SchemeID: "",
      SchemeName: "",
      SchemeCode: "",
      BusinessRuleType: "",
      Stipend: "",
      StipendMode: "",
      UniformAndBag: "",
    }, { updateOn: "blur" });
    this.scheme = new MatTableDataSource([]);

    this.changerequestTSPform = this.fb.group({
      TSPChangeRequestID: 0,
      TSPID: "",
      TSPName: ['', Validators.required],
      Type: "",
      PNTN: "",
      NTN: ['', Validators.required],
      GST: "",
      HeadName: ['', Validators.required],
      HeadDesignation: ['', Validators.required],
      HeadLandline: ['', Validators.required],
      HeadEmail: ['', [Validators.required, Validators.email]],
      CPName: ['', Validators.required],
      CPDesignation: ['', Validators.required],
      CPLandline: ['', { validators: [Validators.required, Validators.maxLength(12)], updateOn: "blur" }],
      CPEmail: ['', [Validators.required, Validators.email]],
      //BankName: ['', Validators.required],
      BankAccountNumber: ['', Validators.required],
      AccountTitle: ['', Validators.required],
      //BankBranch: ['', Validators.required],
      Address: ['', Validators.required],
    }, { updateOn: "blur" });
    this.tsp = new MatTableDataSource([]);

    this.changerequestClassform = this.fb.group({
      ClassChangeRequestID: 0,
      ClassID: "",
      ClassCode: ['', [Validators.required]],
      TrainingAddressLocation: ['', [Validators.required]],
      TehsilID: ['', [Validators.required]],
      DistrictID: ['', [Validators.required]],
      //StartDate: "",
      //EndDate: "",
    }, { updateOn: "blur" });
    this.classArray = new MatTableDataSource([]);

    this.changerequestClassDatesform = this.fb.group({
      ClassDatesChangeRequestID: 0,
      ClassID: "",
      ClassCode: ['', [Validators.required]],
      StartDate: ['', [Validators.required]],
      EndDate: ['', [Validators.required]],
      Duration: ['', [Validators.required]],
    }, { updateOn: "blur" });
    this.classArrayForDateChange = new MatTableDataSource([]);

    this.changerequestTraineeform = this.fb.group({
      TraineeChangeRequestID: 0,
      TraineeID: "",
      TraineeName: ['', [Validators.required, Validators.pattern('^[a-zA-Z \u0600-\u06FF]+')]],
      TraineeCNIC: ['', [Validators.required, Validators.minLength(15), Validators.maxLength(15)]],
      TraineeEmail: ['', [Validators.email, Validators.required]],
      FatherName: ['', [Validators.required, Validators.pattern('^[a-zA-Z \u0600-\u06FF]+')]],
      CNICIssueDate: ['', [Validators.required]],
      ContactNumber1: ['', [Validators.required, Validators.minLength(12), Validators.maxLength(12)]],
      TraineeHouseNumber: ['', [Validators.required]],
      TraineeStreetMohalla: ['', [Validators.required]],
      TraineeMauzaTown: ['', [Validators.required]],
      TraineeDistrictID: ['', [Validators.required]],
      TraineeTehsilID: ['', [Validators.required]],
      VerificationStatus: '',
      TraineechangeImage: "",
      TraineePreImage: "",
      TraineeClassCode: "",
      TraineeCode: ""
      //DateOfBirth: "",

    }, { updateOn: "change" });
    this.trainee = new MatTableDataSource([]);

    this.changerequestInstructorform = this.fb.group({
      InstructorChangeRequestID: 0,
      InstrID: "",
      InstructorName: ["", Validators.required],
      CNICofInstructor: ["", [Validators.required, Validators.minLength(15), Validators.maxLength(15)]],
      QualificationHighest: ["", Validators.required],
      TotalExperience: ["", Validators.required],
      PicturePath: "",
      InstructorCRComments: "",
      //GenderID: "",
    }, { updateOn: "blur" });
    this.instructor = new MatTableDataSource([]);

    this.changerequestInceptionReportform = this.fb.group({
      InceptionReportChangeRequestID: 0,
      IncepReportID: "",
      ClassID: "",
      ClassCode: "",
      ClassStartTime: ["", Validators.required],
      ClassEndTime: ["", Validators.required],
      ClassTotalHours: "",
      Shift: "",
      Monday: 0,
      Tuesday: 0,
      Wednesday: 0,
      Thursday: 0,
      Friday: 0,
      Saturday: 0,
      Sunday: 0
      //InstrIDs: ["", Validators.required]
    }, { updateOn: "blur" });
    this.inceptionreport = new MatTableDataSource([]);

    this.changerequestReplaceInstructorform = this.fb.group({
      InstructorReplaceChangeRequestID: 0,
      IncepReportID: "",
      ClassID: "",
      ClassCode: "",
      InstrIDs: ["", Validators.required]

    }, { updateOn: "blur" });
    this.classesForTimeChange = new MatTableDataSource([]);

    this.addInstructorform = this.fb.group({
      CRNewInstructorID: 0,
      InstructorName: ["", Validators.required],
      CNICofInstructor: ["", [Validators.required, Validators.minLength(15), Validators.maxLength(15)]],
      GenderID: ["", Validators.required],
      QualificationHighest: ["", Validators.required],
      TotalExperience: ["", Validators.required],
      PicturePath: "",
      TradeID: ["", Validators.required],
      SchemeID: ["", Validators.required],
      ClassID: ["", Validators.required],
      LocationAddress: ["", Validators.required],
      FilePath: ["", Validators.required],
      NewInstructorCRComments: ["", Validators.required],
      //GenderID: "",
    }, { updateOn: "blur" });
    this.instructor = new MatTableDataSource([]);

    this.formrights = ComSrv.getFormRights();
  }



  EmptyCtrl() {
    this.SearchCls.setValue('');
    this.SearchSch.setValue('');
    this.SearchTrainee.setValue('');
    this.SearchDistrict.setValue('');
    this.SearchTehsil.setValue('');
  }


  GetData() {
    //this.getSchemeData();
    this.getInstructorData();
  }
  ngOnInit() {

    this.ComSrv.setTitle("Change Request");
    this.title = "Add New ";
    this.savebtn = "Save ";
    this.currentUser = this.ComSrv.getUserDetails();
    this.userid = this.currentUser.UserID;
    this.GetData();
    this.getSchemes();
    //this.getSelectedTabData();

  }

  ngAfterViewInit() {
    this.ComSrv.OID.subscribe(OID => {
      this.filters.SchemeID = 0;
      this.filters.TraineeID = 0;
      this.filters.ClassID = 0;
      this.schemeFilter.setValue(0);
      this.traineeFilters.setValue(0);
      this.classFilter.setValue(0);
    })
  }

  getSelectedTabData() {   // for demo
    switch (this.tabGroup.selectedIndex) {
      case 0:
        this.getInstructorData();
        break;
      case 1:
        this.getNewInstructorFormData();
        break;
      //case 2:
      //  this.getInceptionReportData();
      //  break;
      case 2:
        this.getTraineeData();
        break;
      case 3:
        this.getInceptionReportClassesData();
        break;
      case 4:
        this.getClassDataForLocationChange();
        break;
      case 5:
        this.getClassData();
        break;
      case 6:
        this.getTSPData();
        break;
        //case 3:
        //    this.getTraineeData();
        break;
      default:
    }
  }

  getTSPData() {
    this.ComSrv.getJSON('api/TSPDetail/GetTSPForCRByUser/' + this.userid).subscribe((d: any) => {
      this.tsp = new MatTableDataSource(d[0]);
      this.tspDistricts = d[1];
      this.tspChangeRequests = new MatTableDataSource(d[2]);

      this.editTSPData(this.tsp.filteredData[0]);

      //this.TSPID.setValue(this.tsp.filteredData['TSPID'])
      //this.TSPName.setValue(this.tsp.filteredData['TSPName'])
      //this.HeadName.setValue(this.tsp.filteredData['HeadName'])
      //this.CPName.setValue(this.tsp.filteredData['CPName'])
      //this.Address.setValue(this.tsp.filteredData['Address'])
      this.tsp.paginator = this.paginator;
      this.tsp.sort = this.sort;

      this.tspChangeRequests.paginator = this.PageTSP;
      this.tspChangeRequests.sort = this.SortTSP;

    }, error => this.error = error // error path
    );
  }

  getTSPCRData() {
    this.ComSrv.getJSON('api/TSPDetail/GetTSPCRDataByUser/' + this.userid).subscribe((d: any) => {
      this.tsp = new MatTableDataSource(d[0]);
      this.tspDistricts = d[1];

      this.editTSPData(this.tsp.filteredData[0]);

      //this.TSPID.setValue(this.tsp.filteredData['TSPID'])
      //this.TSPName.setValue(this.tsp.filteredData['TSPName'])
      //this.HeadName.setValue(this.tsp.filteredData['HeadName'])
      //this.CPName.setValue(this.tsp.filteredData['CPName'])
      //this.Address.setValue(this.tsp.filteredData['Address'])
      this.tsp.paginator = this.paginator;
      this.tsp.sort = this.sort;

    }, error => this.error = error // error path
    );
  }

  getSchemeData() {
    this.ComSrv.getJSON('api/Scheme/GetSchemeByUser/' + this.userid).subscribe((d: any) => {
      this.scheme = new MatTableDataSource(d);

      this.editSchemeData(d[0]);
      //this.class.paginator = this.paginator;
      //this.class.sort = this.sort;

    }, error => this.error = error // error path
    );

  }
  getClassData() {
    this.ComSrv.getJSON('api/ClassChangeRequest/GetClassesByUsers/' + this.userid + '/' + this.filters.SchemeID).subscribe((d: any) => {
      this.classArray = new MatTableDataSource(d[0]);
      this.classArrayForDateChange = new MatTableDataSource(d[0]);
      this.classTehsils = d[1];
      this.classDistricts = d[2];

      this.classArray.paginator = this.PageClass;
      this.classArray.sort = this.SortClass;

      this.classArrayForDateChange.paginator = this.PageClassesForDatesChange;
      this.classArrayForDateChange.sort = this.SortClassesForDatesChange;

    }, error => this.error = error // error path
    );
  }

  getClassDataForLocationChange() {
    this.ComSrv.getJSON('api/ClassChangeRequest/GetClassesForLocationChangeByUser/' + this.userid).subscribe((d: any) => {
      this.classArray = new MatTableDataSource(d[0]);
      //this.classArrayForDateChange = new MatTableDataSource(d[0]);
      this.classTehsils = d[1];
      this.classDistricts = d[2];

      this.classArray.paginator = this.PageClass;
      this.classArray.sort = this.SortClass;

      //this.classArrayForDateChange.paginator = this.PageClassesForDatesChange;
      //this.classArrayForDateChange.sort = this.SortClassesForDatesChange;

    }, error => this.error = error // error path
    );
  }

  getSchemes() {
    this.filters.SchemeID = 0;
    this.filters.ClassID = 0;
    this.filters.TraineeID = 0;
    this.ComSrv.postJSON('api/Scheme/FetchSchemeByUser', this.filters).subscribe(
      (d: any) => {
        this.SchemeFilter = d;
      }, error => this.error = error
    );
  }

  //getTSPDetailByScheme(schemeId: number) {
  //  this.classesArrayFilter = [];
  //  this.ComSrv.getJSON(`api/TSPDetail/GetTSPDetailByScheme/` + schemeId)
  //    .subscribe(data => {
  //      this.TSPDetailFilter = <any[]>data;
  //    }, error => {
  //      this.error = error;
  //    })
  //}
  getClassesByFilters() {
    this.filters.ClassID = 0;
    this.filters.TraineeID = 0;
    this.ComSrv.postJSON(`api/Class/FetchClassesByUser/`, { UserID: this.userid, OID: this.ComSrv.OID.value, SchemeID: this.filters.SchemeID })
      .subscribe(data => {
        this.classesArrayFilter = <any[]>data;
        this.activeClassesArrayFilter = this.classesArrayFilter.filter(x => x.ClassStatusID == 3);
      }, error => {
        this.error = error;
      })
  }

  getClassesByInstructorSchemes(scheme: number) {
    this.ComSrv.postJSON(`api/Class/FetchClassesByUser/`, { UserID: this.userid, OID: this.ComSrv.OID.value, SchemeID: scheme })
      .subscribe(data => {
        this.classesArrayForNewTrainer = <any[]>data;
        this.activeClassesArrayForNewTrainer = this.classesArrayForNewTrainer.filter(x => (x.ClassStatusID == 3));
      }, error => {
        this.error = error;
      })
  }

  getTraineeData() {
    /*this.ComSrv.postJSON('api/TraineeProfile/FetchTraineesForCRFilterByUser/', { UserID: this.userid, OID: this.ComSrv.OID.value, SchemeID: this.filters.SchemeID, TraineeID: this.filters.TraineeID, ClassID: this.filters.ClassID }).subscribe((d: any) => {
      this.trainee = new MatTableDataSource(d[0]);
      this.traineeFilter = d[0];
      this.districts = d[1];
      this.traineetehsils = d[2];
      this.trainee.paginator = this.PageTrainee;
      this.trainee.sort = this.SortTrainee;

    }, error => this.error = error // error path
    );*/
    this.initTraineePagedData();
  }

  getInstructorData() {
    this.ComSrv.postJSON('api/Instructor/GetCRInstructorByUser/', { UserID: this.userid, TradeID: this.filters.TradeID }).subscribe((d: any) => {
      this.instructor = new MatTableDataSource(d[0]);
      this.oldInstr = new MatTableDataSource(d[0]);
      this.tradesArray = d[1];
      //this.genders = (d[1]);

      this.instructor.paginator = this.PageInstructor;
      this.instructor.sort = this.SortInstructor;

      //this.getInstrcutorsForDuplicateCheck();
      //this.tsp.paginator = this.paginator;
      //this.tsp.sort = this.sort;

    }, error => this.error = error // error path
    );
  }

  getInstrcutorsForDuplicateCheck() {
    this.ComSrv.getJSON('api/Instructor/GetInstructor').subscribe((d: any) => {
      this.oldInstrCNICs = d[3];
    }, error => this.error = error
    );
  }

  getNewInstructorFormData() {
    this.ComSrv.getJSON('api/Instructor/GetInstructorByUser/' + this.userid).subscribe((d: any) => {
      this.oldInstr = new MatTableDataSource(d[0]);
      this.genders = (d[1]);
      this.trade = (d[2]);
      this.newInstructorsCRs = new MatTableDataSource(d[3]);

      //this.instructor.paginator = this.PageInstructor;
      //this.instructor.sort = this.SortInstructor;

      //this.tsp.paginator = this.paginator;
      //this.tsp.sort = this.sort;

    }, error => this.error = error // error path
    );
  }

  checkInstructorExists(cnic) {
    if (!this.addInstructorform.controls.CNICofInstructor.dirty) {
      return;
    }

    for (let r of this.oldInstrCNICs) {
      if (this.addInstructorform.controls.CNICofInstructor.value == r.CNICofInstructor) {
        //this.addInstructorform.patchValue(r);
        this.addInstructorform.controls.CNICofInstructor.setValue('');
        this.cnicExists = true;

        break;
      }
      else {
        this.cnicExists = false;
      }
    }
  }
  checkCRInstructorExists(cnic) {
    if (!this.changerequestInstructorform.controls.CNICofInstructor.dirty) {
      return;
    }

    for (let r of this.oldInstrCNICs) {
      if (this.changerequestInstructorform.controls.CNICofInstructor.value == r.CNICofInstructor) {
        this.changerequestInstructorform.patchValue(r);
        //this.changerequestInstructorform.controls.CNICofInstructor.setValue('');
        this.cnicExists = true;

        break;
      }
      else {
        this.cnicExists = false;
      }
    }
  }

  //getInceptionReportData() {
  //  this.ComSrv.getJSON('api/InceptionReport/GetInceptionReportByUser/' + this.userid).subscribe((d: any) => {
  //    this.inceptionreport = new MatTableDataSource(d[0]);
  //    this.Instructors = d[1];

  //    this.inceptionreport.paginator = this.PageInceptionReport;
  //    this.inceptionreport.sort = this.SortInceptionReport;

  //  }, error => this.error = error // error path
  //  );
  //}

  getInceptionReportClassesData() {
    this.ComSrv.getJSON('api/InceptionReport/GetInceptionReportByUser/' + this.userid).subscribe((d: any) => {
      this.classesForTimeChange = new MatTableDataSource(d[0]);
      this.Instructors = d[1];

      this.classesForTimeChange.paginator = this.PageClassesForTimeChange;
      this.classesForTimeChange.sort = this.SortClassesForTimeChange;

      //this.tsp.paginator = this.paginator;
      //this.tsp.sort = this.sort;

    }, error => this.error = error // error path
    );
  }

  isEligibleTrainee(): void {
    let values = this.changerequestTraineeform.getRawValue();
    //if (values.TraineeCNIC.length == 15) {
    let filter = `?traineeId=${values.TraineeID}&cnic=${values.TraineeCNIC}&classId=${values.ClassID}`
    this.ComSrv.getJSON(`api/TraineeProfile/isEligibleTrainee` + filter).subscribe(
      (data: any) => {
        //BR (Business Rule)
        if (!data.isValid) {
          this.CheckTraineecCnic = false;
          this.TraineeCNIC.setErrors({ isValid: data.isValid, message: data.errMsg });
        }
        else {
          this.CheckTraineecCnic = true;
          this.TraineeCNIC.setErrors(null);
          this.TraineeCNIC.setValidators([Validators.required, Validators.minLength(15), Validators.maxLength(15)]);
          this.TraineeCNIC.updateValueAndValidity();
        }
      }, (error) => {
        this.error = error // error path
      }
    );
    //}
  }

  GetDurationType(duration: number): boolean {
    return duration % Math.round(duration) == 0 ? false : true;
  }

  GetEndDate(isFloat: boolean, date: Date, duration: any): Date {
    if (!isFloat)
      return new Date(date.getFullYear(), date.getMonth() + duration, date.getDate());
    else
      return new Date(date.getFullYear(), date.getMonth(), date.getDate() + (duration * 30));
  }

  SetEndDate() {
    if (this.changerequestClassDatesform.controls.Duration.value == '' || this.changerequestClassDatesform.controls.StartDate.value == '')
      return

    let duration = parseFloat(this.changerequestClassDatesform.controls.Duration.value);

    let isFloat: boolean = this.GetDurationType(duration);
    let date: Date = new Date(this.changerequestClassDatesform.controls.StartDate.value._i.year, this.changerequestClassDatesform.controls.StartDate.value._i.month, this.changerequestClassDatesform.controls.StartDate.value._i.date);
    let endDate: Date = this.GetEndDate(isFloat, date, duration);
    this.EndDate.setValue(endDate);
  }

  GenerateShift() {
    var Stime = this.ClassStartTime.value.split(":");

    var IStime = Stime[0];
    var FStime = Number(IStime);
    if (FStime < 12) {
      console.log(this.ClassStartTime.value);
      this.Shift.setValue('1st');

    }
    else {
      this.Shift.setValue('2nd');

    }

  }

  GenerateHours() {
    if (this.ClassStartTime.value != "" && this.ClassEndTime.value != "") {
      var Stime = this.ClassStartTime.value.split(":");
      var SMtime = this.ClassStartTime.value.split(" ");
      var SM = SMtime[1];
      var IStime = Stime[0];
      var SMinTime = Number(Stime[1]);
      var FStime = Number(IStime);

      console.log(Stime)
      console.log(IStime)
      console.log(FStime);

      var Etime = this.ClassEndTime.value.split(":");
      var EMtime = this.ClassEndTime.value.split(" ");
      var EM = EMtime[1];
      var IEtime = Etime[0];
      var EMinTime = Number(Etime[1]);
      var FEtime = Number(IEtime);

      console.log(Etime)
      console.log(IEtime);
      console.log(FEtime);

      var StartTimeMinutes = (FStime * 60) + SMinTime
      var EndTimeMinutes = (FEtime * 60) + EMinTime

      this.TimeDifference = EndTimeMinutes - StartTimeMinutes
      var num = this.TimeDifference;
      var hours = (num / 60);
      var rhours = Math.floor(hours);
      var minutes = (hours - rhours) * 60;
      var rminutes = Math.round(minutes);

      var finalTime = rhours + ":" + rminutes;
      var finalTime1 = parseFloat(rhours + "." + rminutes).toFixed(2);
      var finalTime2 = parseFloat(parseFloat(rhours + "." + rminutes).toFixed(2));

      FStime = FStime == 24 ? 0 : FStime;

      if (FStime > FEtime) {
        //this.ClassEndTime.reset();
        this.error = "Class End Time Should be Greater than Start Time";
        this.ComSrv.ShowError(this.error.toString(), "Error");
        return false;
      }
      else {
        var TotalHours = FEtime - FStime;
        this.ClassTotalHours.setValue(finalTime);
      }
    }


    if (IStime == '24') {
      this.ClassStartTime.setValue('00' + ":" + SMinTime)
    }
    if (IEtime == '24') {
      this.ClassEndTime.setValue('00' + ":" + EMinTime)
      //this.ClassEndTime.value = '00'
    }
  }


  Submit() {
    if (!this.changerequestTSPform.valid)
      return;
    this.working = true;
    this.ComSrv.postJSON('api/TSPDetail/Update_TSPDetail', this.changerequestTSPform.value)
      .subscribe((d: any) => {
        //this.tsp = new MatTableDataSource(d);
        //this.tsp.paginator = this.paginator;
        //this.tsp.sort = this.sort;
        //this.ComSrv.openSnackBar(this.RTPID.value > 0 ? environment.UpdateMSG.replace("${Name}", this.EnText) : environment.SaveMSG.replace("${Name}", this.EnText));
        //this.reset(myform);
        this.title = "Add New ";
        this.savebtn = "Save ";
      },
        error => this.error = error // error path
        , () => {
          this.working = false;

        });
  }

  SubmitScheme() {
    if (!this.changerequestSchemeform.valid)
      return;
    this.working = true;

    let processKey = EnumApprovalProcess.CR_SCHEME;
    this.changerequestSchemeform.value["ProcessKey"] = processKey;


    this.ComSrv.postJSON('api/SchemeChangeRequest/Save', this.changerequestSchemeform.value)
      .subscribe((d: any) => {
        //this.scheme = new MatTableDataSource(d);
        //this.scheme.paginator = this.paginator;
        //this.scheme.sort = this.sort;
        this.ComSrv.openSnackBar(this.SchemeChangeRequestID.value > 0 ? environment.UpdateMSG.replace("${Name}", this.EnTextScheme) : environment.SaveMSG.replace("${Name}", this.EnTextScheme));
        //this.reset(myform);
        this.title = "Add New ";
        this.savebtn = "Save ";
      },
        error => this.error = error // error path
        , () => {
          this.working = false;

        });
  }
  SubmitTSP() {
    if (!this.changerequestTSPform.valid)
      return;
    this.working = true;

    let processKey = EnumApprovalProcess.CR_TSP;
    this.changerequestClassform.value["ProcessKey"] = processKey;

    this.ComSrv.postJSON('api/TSPChangeRequest/Save', this.changerequestTSPform.value)
      .subscribe((d: any) => {
        //this.scheme = new MatTableDataSource(d);
        //this.scheme.paginator = this.paginator;
        //this.scheme.sort = this.sort;
        this.ComSrv.openSnackBar(this.TSPChangeRequestID.value > 0 ? environment.UpdateMSG.replace("${Name}", this.EnTextTSP) : environment.SaveMSG.replace("${Name}", this.EnTextTSP));
        this.title = "Add New ";
        this.working = false;
        this.savebtn = "Save ";
      },
        (error) => {
          this.error = error.error;
          this.ComSrv.ShowError(error.error);

        });
  }

  SubmitClass() {
    if (!this.changerequestClassform.valid)
      return;
    this.working = true;

    //if (this.ClassID.value == '') {
    //  this.error = "Only Change Request for Class Timings is allowed";
    //  this.ComSrv.ShowError(this.error.toString(), "Error");
    //  return;
    //}


    let processKey = EnumApprovalProcess.CR_CLASS_LOCATION;
    this.changerequestClassform.value["ProcessKey"] = processKey;

    this.ComSrv.postJSON('api/ClassChangeRequest/Save', this.changerequestClassform.value)
      .subscribe((d: any) => {
        //this.class = new MatTableDataSource(d);
        //this.class.paginator = this.paginator;
        //this.class.sort = this.sort;
        this.ComSrv.openSnackBar(this.ClassChangeRequestID.value > 0 ? environment.UpdateMSG.replace("${Name}", this.EnTextClass) : environment.SaveMSG.replace("${Name}", this.EnTextClass));
        this.resetClassLocationForm();
        this.title = "Add New ";
        this.savebtn = "Save ";
        this.working = false;
      },
        (error) => {
          this.error = error.error;
          this.ComSrv.ShowError(error.error);

        });
  }

  SubmitClassDates() {
    if (!this.changerequestClassDatesform.valid)
      return;
    this.working = true;

    //if (this.ClassID.value == '') {
    //  this.error = "Only Change Request for Class Timings is allowed";
    //  this.ComSrv.ShowError(this.error.toString(), "Error");
    //  return;
    //}


    let processKey = EnumApprovalProcess.CR_CLASS_DATES;
    this.changerequestClassform.value["ProcessKey"] = processKey;

    this.ComSrv.postJSON('api/ClassChangeRequest/SaveClassDates', this.changerequestClassDatesform.value)
      .subscribe((d: any) => {
        //this.class = new MatTableDataSource(d);
        //this.class.paginator = this.paginator;
        //this.class.sort = this.sort;
        this.ComSrv.openSnackBar(this.ClassDatesChangeRequestID.value > 0 ? environment.UpdateMSG.replace("${Name}", this.EnTextClass) : environment.SaveMSG.replace("${Name}", this.EnTextClass));
        this.resetClassDatesForm();
        this.title = "Add New ";
        this.working = false;
        this.savebtn = "Save ";
      },
        (error) => {
          this.error = error.error;
          this.ComSrv.ShowError(error.error);

        });
  }
  onEnterKey(event: KeyboardEvent): void {
    event.preventDefault();
  }

  SubmitTrainee() {

    // if(this.TraineeEmail.value ==this.PreTraineeEmail){
    //   this.ComSrv.ShowError('No Change detected.');
    //   return;
    // }

    this.isEligibleTrainee()

    if (!this.changerequestTraineeform.valid || this.CheckTraineecCnic == false) {
      return;
    }

    this.working = true;
    this.TraineeDisplayImg = '';

    if (this.changerequestTraineeform.value['VerificationStatus'] === 'Yes') {
      const processKey = EnumApprovalProcess.CR_TRAINEE_VERIFIED;
      this.changerequestTraineeform.value['ProcessKey'] = processKey;

      this.ComSrv.postJSON('api/TraineeChangeRequest/SaveVerifiedTrainee', this.changerequestTraineeform.getRawValue())
        .subscribe(
          (response: any) => {
            this.ComSrv.openSnackBar(
              this.TraineeChangeRequestID.value > 0
                ? environment.UpdateMSG.replace('${Name}', this.EnTextTrainee)
                : environment.SaveMSG.replace('${Name}', this.EnTextTrainee)
            );
            this.title = 'Add New';
            this.working = false;
            this.savebtn = 'Save';
          },
          (error) => {
            this.error = error.error;
            this.ComSrv.ShowError(error.error);
            this.working = false;
          }
        );
    } else {
      const processKey = EnumApprovalProcess.CR_TRAINEE_UNVERIFIED;
      this.changerequestTraineeform.value['ProcessKey'] = processKey;

      this.ComSrv.postJSON('api/TraineeChangeRequest/Save', this.changerequestTraineeform.getRawValue())
        .subscribe(
          (response: any) => {
            this.ComSrv.openSnackBar(
              this.TraineeChangeRequestID.value > 0
                ? environment.UpdateMSG.replace('${Name}', this.EnTextTrainee)
                : environment.SaveMSG.replace('${Name}', this.EnTextTrainee)
            );
            this.resetTraineeForm();
            this.title = 'Add New';
            this.working = false;
            this.savebtn = 'Save';
          },
          (error) => {
            this.error = error.error;
            this.ComSrv.ShowError(error.error);
            this.working = false;
          }
        );
    }
  }

  SubmitInstructor() {
    if (!this.changerequestInstructorform.valid)
      return;
    this.working = true;

    if (this.InstrID.value == '') {
      this.error = "Only Instructor Profile Change Request is allowed";
      this.ComSrv.ShowError(this.error.toString(), "Error");
      return;
    }

    let processKey = EnumApprovalProcess.CR_INSTRUCTOR;
    this.changerequestInstructorform.value["ProcessKey"] = processKey;

    this.ComSrv.postJSON('api/InstructorChangeRequest/Save', this.changerequestInstructorform.value)
      .subscribe((d: any) => {
        //this.instructor = new MatTableDataSource(d);
        //this.instructor.paginator = this.paginator;
        //this.instructor.sort = this.sort;
        if (d) {
          this.alert = 'yes';
        }
        this.ComSrv.openSnackBar(this.InstructorChangeRequestID.value > 0 ? environment.UpdateMSG.replace("${Name}", this.EnTextNewInstructor) : environment.SaveMSG.replace("${Name}", this.EnTextNewInstructor));
        this.resetInstructorForm();
        this.title = "Add New ";
        this.savebtn = "Save ";
        this.working = false;
      },
        (error) => {
          this.error = error.error;
          this.ComSrv.ShowError(error.error);


        });
  }

  SubmitNewInstructor() {
    if (!this.addInstructorform.valid)
      return;
    this.working = true;

    if (this.InvalidInstructorFile) {
      this.error = "Please upload only zip file";
      this.ComSrv.ShowError(this.error.toString(), "Error");
      return;
    }

    let processKey = EnumApprovalProcess.CR_NEW_INSTRUCTOR;
    this.addInstructorform.value["ProcessKey"] = processKey;

    this.ComSrv.postJSON('api/InstructorChangeRequest/SaveNewInstructor', this.addInstructorform.value)
      .subscribe((d: any) => {
        if (d) {
          this.newInstructorsCRs = d;
          this.add = "New Instructor successfully added and sent for approval";
          this.ComSrv.openSnackBar(this.add.toString(), "success");

          this.ComSrv.openSnackBar(this.CRNewInstructorID.value > 0 ? environment.UpdateMSG.replace("${Name}", this.EnTextInstructor) : environment.SaveMSG.replace("${Name}", this.EnTextInstructor));
        }
        this.resetaddInstructorForm();
        this.title = "Add New ";
        this.working = false;
        this.savebtn = "Save ";
      },
        (error) => {
          this.error = error.error;
          this.ComSrv.ShowError(error.error);


        });
  }



  SubmitInceptionReport() {
    if (!this.changerequestInceptionReportform.valid)
      return;
    this.working = true;
    if (this.IncepReportID.value == "") {
      this.error = "Only Change Request for Class Timings is allowed";
      this.ComSrv.ShowError(this.error.toString(), "Error");
      return;
    }

    //--- Getting other class timing of selected instructor of the active class.
    const classCode = this.changerequestInceptionReportform.get('ClassCode').value;
    //this.ComSrv.getJSON('api/InceptionReportChangeRequest/GetActiveClassTimingCR/' + this.ClassID.value)
    this.ComSrv.getJSON('api/InceptionReport/GetActiveClassTimingCR/' + classCode).subscribe((d: any) => {
      this.ActiveClassTiming = d[0];
      let ActiveClassTimingList = d[0];

      if (ActiveClassTimingList.length > 0) {
        this.GetStartDateI = this.ActiveClassTiming[0].StartDateTime;
        this.GetEndDateI = this.ActiveClassTiming[0].EndDateTime;
        this.GetCurrentStartDateI = this.ActiveClassTiming[0].ActualStartDate;
        this.GetCurrentEndDateI = this.ActiveClassTiming[0].ActualEndDate;

        //const currentClassStartDate = new Date(this.ActualStartDate.value);
        //const currentClassEndDate = new Date(this.ActualEndDate.value);
        const currentClassStartTime = this.ClassStartTime.value;
        const currentClassEndTime = this.ClassEndTime.value;

        //const otherClassStartDate = new Date(this.GetCurrentStartDate);
        //const otherClassEndDate = new Date(this.GetCurrentEndDate);
        const otherClassStartTime = this.GetStartDateI;
        const otherClassEndTime = this.GetEndDateI;

        //Class funcation overlap result
        //this.doOverlap = doClassTimesOverlap(currentClassTime, otherClassTime);
        this.doOverlap = doClassTimesOverlap(
          currentClassStartTime,
          currentClassEndTime,
          otherClassStartTime,
          otherClassEndTime
        );
        if (this.doOverlap) {
          //console.log("Instructor already assigned to another class in the same time bracket!");
          this.error = "Instructor already assigned to another class in the same time bracket!";
          this.ComSrv.ShowError(this.error.toString(), "Error");
          this.FinalSubmitted.setValue(false);
          return;
        }
        else {

          let selectedDaysCount = 0;
          if (this.Monday.value == 1) {
            selectedDaysCount++;
          }
          if (this.Tuesday.value == 1) {
            selectedDaysCount++;
          }
          if (this.Wednesday.value == 1) {
            selectedDaysCount++;
          }
          if (this.Thursday.value == 1) {
            selectedDaysCount++;
          }
          if (this.Friday.value == 1) {
            selectedDaysCount++;
          }
          if (this.Saturday.value == 1) {
            selectedDaysCount++;
          }
          if (this.Sunday.value == 1) {
            selectedDaysCount++;
          }
          if (selectedDaysCount * Number(this.TimeDifference) * 4 < (this.MinClassHoursPerMonth * 60)) {  // we are calculating & checking minutes per month instead of hours in this formula
            this.error = "Total Class Hours are less than class hours in Appendix";
            this.ComSrv.ShowError(this.error.toString(), "Error");
            return false;
          }

          let processKey = EnumApprovalProcess.CR_INCEPTION;
          this.changerequestInceptionReportform.value["ProcessKey"] = processKey;
          // this.changerequestInceptionReportform.value['InstrIDs'] = this.InstrIDs.value.join(',');

          this.ComSrv.postJSON('api/InceptionReportChangeRequest/Save', this.changerequestInceptionReportform.value)
            .subscribe((d: any) => {
              //this.instructor = new MatTableDataSource(d);
              //this.instructor.paginator = this.paginator;
              //this.instructor.sort = this.sort;
              this.ComSrv.openSnackBar(this.IncepReportID.value > 0 ? environment.UpdateMSG.replace("${Name}", this.EnTextInceptionReport) : environment.SaveMSG.replace("${Name}", this.EnTextInceptionReport));
              this.resetInceptionReportForm();
              this.title = "Add New ";
              this.savebtn = "Save ";
              this.working = false;
            },
              (error) => {
                this.error = error.error;
                this.ComSrv.ShowError(error.error);

              });
        }
      }
      else {

        let selectedDaysCount = 0;
        if (this.Monday.value == 1) {
          selectedDaysCount++;
        }
        if (this.Tuesday.value == 1) {
          selectedDaysCount++;
        }
        if (this.Wednesday.value == 1) {
          selectedDaysCount++;
        }
        if (this.Thursday.value == 1) {
          selectedDaysCount++;
        }
        if (this.Friday.value == 1) {
          selectedDaysCount++;
        }
        if (this.Saturday.value == 1) {
          selectedDaysCount++;
        }
        if (this.Sunday.value == 1) {
          selectedDaysCount++;
        }
        if (selectedDaysCount * Number(this.TimeDifference) * 4 < (this.MinClassHoursPerMonth * 60)) {  // we are calculating & checking minutes per month instead of hours in this formula
          this.error = "Total Class Hours are less than class hours in Appendix";
          this.ComSrv.ShowError(this.error.toString(), "Error");
          return false;
        }

        let processKey = EnumApprovalProcess.CR_INCEPTION;
        this.changerequestInceptionReportform.value["ProcessKey"] = processKey;
        // this.changerequestInceptionReportform.value['InstrIDs'] = this.InstrIDs.value.join(',');

        this.ComSrv.postJSON('api/InceptionReportChangeRequest/Save', this.changerequestInceptionReportform.value)
          .subscribe((d: any) => {
            //this.instructor = new MatTableDataSource(d);
            //this.instructor.paginator = this.paginator;
            //this.instructor.sort = this.sort;
            this.ComSrv.openSnackBar(this.IncepReportID.value > 0 ? environment.UpdateMSG.replace("${Name}", this.EnTextInceptionReport) : environment.SaveMSG.replace("${Name}", this.EnTextInceptionReport));
            this.resetInceptionReportForm();
            this.title = "Add New ";
            this.savebtn = "Save ";
            this.working = false;
          },
            (error) => {
              this.error = error.error;
              this.ComSrv.ShowError(error.error);

            });
      }
    }, error => this.error = error// error path
    );


  }




  // To replace or submit instructor

  //SubmitReplaceInstructor() {
  //  if (!this.changerequestReplaceInstructorform.valid)
  //    return;
  //  this.working = true;
  //  if (this.InstructorReplaceIncepReportID.value == "") {
  //    this.error = "Only Inception Report Change Request is allowed";
  //    this.ComSrv.ShowError(this.error.toString(), "Error");
  //    return;
  //  }

  //  let processKey = EnumApprovalProcess.CR_INSTRUCTOR_REPLACE;
  //  this.changerequestReplaceInstructorform.value["ProcessKey"] = processKey;
  //  this.changerequestReplaceInstructorform.value['InstrIDs'] = this.InstrIDs.value.join(',');

  //  this.ComSrv.postJSON('api/InstructorReplaceChangeRequest/Save', this.changerequestReplaceInstructorform.value)
  //    .subscribe((d: any) => {
  //      //this.instructor = new MatTableDataSource(d);
  //      //this.instructor.paginator = this.paginator;
  //      //this.instructor.sort = this.sort;
  //      this.ComSrv.openSnackBar(this.IncepReportID.value > 0 ? environment.UpdateMSG.replace("${Name}", this.EnTextInceptionReport) : environment.SaveMSG.replace("${Name}", this.EnTextInstructorReplacement));
  //      this.resetInstructorReplaceForm();
  //      this.title = "Add New ";
  //      this.savebtn = "Save ";
  //    },
  //      (error) => {
  //        this.error = error.error;
  //        this.ComSrv.ShowError(error.error);

  //      });
  //}


  reset() {
    this.changerequestTSPform.reset();
    //myform.resetFrom();
    this.TSPID.setValue(0);
    this.title = "Add New ";
    this.savebtn = "Save ";
  }
  resetTraineeForm() {
    this.changerequestTraineeform.reset();
    //myform.resetFrom();
    this.TraineeID.setValue(0);
    this.title = "Add New ";
    this.savebtn = "Save ";
  }
  resetInstructorForm() {
    this.changerequestInstructorform.reset();
    //myform.resetFrom();
    this.InstrID.setValue(0);
    this.title = "Add New ";
    this.savebtn = "Save ";
  }
  resetaddInstructorForm() {
    this.addInstructorform.reset();
    //myform.resetFrom();
    this.CRNewInstructorID.setValue(0);
    this.title = "Add New ";
    this.savebtn = "Save ";
  }
  resetInceptionReportForm() {
    this.changerequestInceptionReportform.reset();
    //myform.resetFrom();
    this.IncepReportID.setValue(0);
    this.title = "Add New ";
    this.savebtn = "Save ";
  }
  resetInstructorReplaceForm() {
    this.changerequestReplaceInstructorform.reset();
    //myform.resetFrom();
    this.InstructorReplaceIncepReportID.setValue(0);
    this.title = "Add New ";
    this.savebtn = "Save ";
  }
  resetClassLocationForm() {
    this.changerequestClassform.reset();
    //myform.resetFrom();
    this.ClassChangeRequestID.setValue(0);
    this.title = "Add New ";
    this.savebtn = "Save ";
  }
  resetClassDatesForm() {
    this.changerequestClassDatesform.reset();
    //myform.resetFrom();
    this.ClassDatesChangeRequestID.setValue(0);
    this.title = "Add New ";
    this.savebtn = "Save ";
  }

  editSchemeData(row: any) {
    this.SchemeID.setValue(row.SchemeID);
    this.SchemeCode.setValue(row.SchemeCode);
    this.BusinessRuleType.setValue(row.BusinessRuleType);
    this.Stipend.setValue(row.Stipend);
    this.StipendMode.setValue(row.StipendMode);
    this.UniformAndBag.setValue(row.UniformAndBag);
  }

  //Type: "",
  //PNTN: "",
  //GST: "",

  editTSPData(row: any) {
    this.TSPID.setValue(row.TSPID);
    this.TSPName.setValue(row.TSPName);
    this.Type.setValue(row.Type);
    this.NTN.setValue(row.NTN);
    this.PNTN.setValue(row.PNTN);
    this.GST.setValue(row.GST);
    this.HeadName.setValue(row.HeadName);
    this.HeadDesignation.setValue(row.HeadDesignation);
    this.HeadLandline.setValue(row.HeadLandline);
    this.HeadEmail.setValue(row.HeadEmail);
    this.CPName.setValue(row.CPName);
    this.CPDesignation.setValue(row.CPDesignation);
    this.CPLandline.setValue(row.CPLandline);
    this.CPEmail.setValue(row.CPEmail);
    this.Address.setValue(row.Address);
    //this.BankName.setValue(row.BankName);
    this.BankAccountNumber.setValue(row.BankAccountNumber);
    this.AccountTitle.setValue(row.AccountTitle);
    //  this.BankBranch.setValue(row.BankBranch);
  }

  editClassData(row: any) {
    this.ClassID.setValue(row.ClassID);
    this.ClassCode.setValue(row.ClassCode);
    this.TrainingAddressLocation.setValue(row.TrainingAddressLocation);
    this.ClassLocationDistrict.setValue(row.DistrictID);
    this.ClassLocationTehsil.setValue(row.TehsilID);
  }

  editClassDatesData(row: any) {
    this.ClassIDforDate.setValue(row.ClassID);
    this.ClassCodeForDate.setValue(row.ClassCode);
    this.StartDate.setValue(row.StartDate);
    this.EndDate.setValue(row.EndDate);

    this.Duration.setValue(row.Duration);

    //this.SetEndDate();
  }

  editTraineeData(row: any) {

    this.TraineechangeImage.setValue("");
    if (row.VerificationStatus == 'Yes') {

      this.changerequestTraineeform.controls.TraineeCNIC.disable({ onlySelf: true });
      this.changerequestTraineeform.controls.CNICIssueDate.disable({ onlySelf: true });
      this.changerequestTraineeform.controls.TraineeName.enable({ onlySelf: true });
      this.changerequestTraineeform.controls.FatherName.enable({ onlySelf: true });
      this.Verified = true;
    }
    else {
      this.Verified = false;
      this.changerequestTraineeform.controls.TraineeCNIC.enable({ onlySelf: true });
      this.changerequestTraineeform.controls.CNICIssueDate.enable({ onlySelf: true });
      this.changerequestTraineeform.controls.TraineeName.disable({ onlySelf: true });
      this.changerequestTraineeform.controls.FatherName.disable({ onlySelf: true });
    }
    this.PreTraineeEmail = row.TraineeEmail;
    this.TraineeID.setValue(row.TraineeID);
    this.TraineeName.setValue(row.TraineeName);
    this.FatherName.setValue(row.FatherName);
    this.TraineeCNIC.setValue(row.TraineeCNIC);
    this.TraineeEmail.setValue(row.TraineeEmail);
    this.CNICIssueDate.setValue(row.CNICIssueDate);
    this.ContactNumber1.setValue(row.ContactNumber1);
    this.TraineeHouseNumber.setValue(row.TraineeHouseNumber);
    this.TraineeStreetMohalla.setValue(row.TraineeStreetMohalla);
    this.TraineeMauzaTown.setValue(row.TraineeMauzaTown);
    this.TraineeDistrictID.setValue(row.TraineeDistrictID);
    this.TraineeTehsilID.setValue(row.TraineeTehsilID);
    this.VerificationStatus.setValue(row.VerificationStatus);

    this.TraineePreImage.setValue(row.TraineeImg);
    this.TraineeCode.setValue(row.TraineeCode);
    this.TraineeClassCode.setValue(row.ClassCode);

    this.TraineeDisplayImg = row.TraineeImg



  }

  editInstructorData(row: any) {
    this.InstrID.setValue(row.InstrID);
    this.InstructorName.setValue(row.InstructorName);
    this.CNICofInstructor.setValue(row.CNICofInstructor);
    this.QualificationHighest.setValue(row.QualificationHighest);
    this.TotalExperience.setValue(row.TotalExperience);
    this.PicturePath.setValue(row.PicturePath);
    this.InstructorCRComments.setValue(row.InstructorCRComments);
    //this.GenderID.setValue(row.GenderID);
  }

  editInceptionReportData(row: any) {
    this.IncepReportID.setValue(row.IncepReportID);
    this.InceptionReportClassCode.setValue(row.ClassCode);
    this.InceptionReportClassID.setValue(row.ClassID);
    this.ClassStartTime.setValue(moment(row.ClassStartTime).format('HH:mm').toString());
    this.ClassEndTime.setValue(moment(row.ClassEndTime).format('HH:mm').toString());
    //this.ClassStartTime.setValue(row.ClassStartTime);
    //this.ClassEndTime.setValue(row.ClassEndTime);
    this.Shift.setValue(row.Shift);
    this.ClassTotalHours.setValue(row.ClassTotalHours);
    this.Monday.setValue(row.Monday);
    this.Tuesday.setValue(row.Tuesday);
    this.Wednesday.setValue(row.Wednesday);
    this.Thursday.setValue(row.Thursday);
    this.Friday.setValue(row.Friday);
    this.Saturday.setValue(row.Saturday);
    this.Sunday.setValue(row.Sunday);
    this.Sunday.setValue(row.Sunday);

    this.ComSrv.getJSON('api/InceptionReport/GetInceptionReport/' + row.ClassID).subscribe((d: any) => {
      //this.inceptionreport = new MatTableDataSource(d[0]);
      //this.Instructors = d[2];
      this.classObj = d[3];

      //this.instructor = new MatTableDataSource(this.Instructors);

      this.MinClassHoursPerMonth = this.classObj.MinHoursPerMonth;

    }, error => this.error = error// error path
    );


    //this.InstrIDs.setValue(row.InstrIDs.split(',').map(Number));
  }
  editReplaceInstructorData(row: any) {
    this.InstructorReplaceIncepReportID.setValue(row.IncepReportID);
    this.InstructorReplaceClassCode.setValue(row.ClassCode);
    this.InstructorReplaceClassID.setValue(row.ClassID);


    this.ComSrv.getJSON('api/InceptionReport/GetInceptionReport/' + row.ClassID).subscribe((d: any) => {
      //this.inceptionreport = new MatTableDataSource(d[0]);
      this.Instructors = d[2];
      this.classObj = d[3];

      this.instructor = new MatTableDataSource(this.Instructors);

    }, error => this.error = error// error path
    );


    this.InstrIDs.setValue(row.InstrIDs.split(',').map(Number));
  }

  toggleEdit(row) {
    this.Address.setValue(row.Address);

    this.title = "Update ";
    this.savebtn = "Save ";
  }

  applyFilter(filterValue: string) {
    filterValue = filterValue.trim(); // Remove whitespace
    filterValue = filterValue.toLowerCase(); // Datasource defaults to lowercase matches
    this.tsp.filter = filterValue;
  }

  dateFilter = (d: Date | null): boolean => {
    const date = (d || new Date());
    // Prevent after current date selection .
    return date <= new Date();
  }

  GetPopulateClassTrade(classid: number) {
    var trade = this.activeClassesArrayForNewTrainer.filter(x => x.ClassID == classid).map(y => y.TradeID);
    var location = this.activeClassesArrayForNewTrainer.filter(x => x.ClassID == classid).map(y => y.TrainingAddressLocation);
    this.tradeid = trade[0];
    this.newInstructorAddress = location[0];
    this.addInstructorform.controls.TradeID.setValue(this.tradeid);
    //this.addInstructorform.controls.LocationAddress.setValue(location[0]);
    this.addInstructorform.controls.LocationAddress.setValue(this.newInstructorAddress);
  }
  //Pagination\\
  resultsTraineeLength: number;
  schemeFilter = new FormControl(0);
  classFilter = new FormControl(0);
  traineeFilters = new FormControl(0);

  initTraineePagedData() {
    this.SortTrainee.sortChange.subscribe(() => this.PageTrainee.pageIndex = 0);
    this.PageTrainee.pageSize = 5;
    merge(this.SortTrainee.sortChange, this.PageTrainee.page, this.schemeFilter.valueChanges, this.classFilter.valueChanges, this.traineeFilters.valueChanges).pipe(
      startWith({}),
      switchMap(() => {
        let pagedModel = {
          PageNo: this.PageTrainee.pageIndex + 1
          , PageSize: this.PageTrainee.pageSize
          , SortColumn: this.SortTrainee.active
          , SortOrder: this.SortTrainee.direction
          , SearchColumn: ''
          , SearchValue: ''
        };
        let filter = {
          UserID: this.userid,
          OID: this.ComSrv.OID.value,
          SchemeID: this.filters.SchemeID,
          ClassID: this.filters.ClassID,
          TraineeID: this.filters.TraineeID
        };
        return this.getTraineePagedData(pagedModel, filter);
      })).subscribe(data => {
        this.trainee = new MatTableDataSource(data[0]);
        this.districts = data[1];
        this.traineetehsils = data[2];
        this.resultsTraineeLength = data[3].TotalCount;
      }, error => this.error = error
      );
  }
  getTraineePagedData(pagingModel, filterModel) {
    return this.ComSrv.postJSON('api/TraineeProfile/FetchTraineesForCRFilterByUserPaged', { pagingModel, filterModel });
  }
  getTraineesByFilters() {
    this.filters.TraineeID = 0;
    this.traineeFilters.setValue(0);
    this.ComSrv.postJSON('api/TraineeProfile/FetchTraineesForCRFilterByUser/', { UserID: this.userid, OID: this.ComSrv.OID.value, SchemeID: this.filters.SchemeID, TraineeID: this.filters.TraineeID, ClassID: this.filters.ClassID })
      .subscribe((d: any) => {
        this.traineeFilter = d[0];
      }, error => this.error = error // error path
      );
  }

  checkOnTraineeEmail() {
    this.isEligibleTraineeEmail()
  }

  isEligibleTraineeEmail(): void {
    let values = this.changerequestTraineeform.getRawValue();
    let filter = `?traineeId=${values.TraineeID}&email=${values.TraineeEmail}&classId=${values.ClassID}`
    this.ComSrv.getJSON(`api/TraineeProfile/isEligibleTraineeEmail` + filter).subscribe(
      (data: any) => {
        //BR (Business Rule)
        if (!data.isValid) {
          this.TraineeEmail.setErrors({ isValid: data.isValid, message: data.errMsg });
        }
        else {
          this.TraineeEmail.setErrors(null);
          this.TraineeEmail.setValidators([Validators.email, Validators.required]);
          this.TraineeEmail.updateValueAndValidity();
        }
      }, (error) => {
        this.error = error // error path
      }
    );
    //}
  }

  checkZipFile(ev: Event) {
    let file = (ev.target as HTMLInputElement).files[0];
    var filename = file.name;
    console.log(file.name)
    var regexp;
    var extension = filename.substr(filename.lastIndexOf('.'));
    if (extension.toLowerCase() != ".zip") {
      //alert("Could not allow to upload files other than zip");
      this.addInstructorform.controls.FilePath.setErrors({ invalid: true });
      this.InvalidInstructorFile = true;
      //this.addInstructorform.controls.FilePath.value.invalid=true;
      //this.FilePath.errors.invalid = true;
      //this.FilePath.setErrors({ wrongtype: true });
      //this.FilePath.setValue(null);
      //this.FilePath.setErrors({ isValid: false, message: "Trainee's age is not valid for this class. " })
      //this.FilePath.value.nativeElement = "";
      //this.FilePath.reset();
      //this.FilePath.status({invalid) = true;
    }
    else {
      this.InvalidInstructorFile = false;
    }
  }
  openTraineeJourneyDialogue(data: any): void {
    debugger;
    this.dialogueService.openTraineeJourneyDialogue(data);
  }

  openClassJourneyDialogue(data: any): void {
    debugger;
    this.dialogueService.openClassJourneyDialogue(data);
  }
  get TSPChangeRequestID() { return this.changerequestTSPform.get("TSPChangeRequestID"); }
  get TSPID() { return this.changerequestTSPform.get("TSPID"); }
  get Type() { return this.changerequestTSPform.get("Type"); }
  get PNTN() { return this.changerequestTSPform.get("PNTN"); }
  get NTN() { return this.changerequestTSPform.get("NTN"); }
  get GST() { return this.changerequestTSPform.get("GST"); }
  get HeadName() { return this.changerequestTSPform.get("HeadName"); }
  get CPName() { return this.changerequestTSPform.get("CPName"); }
  get TSPName() { return this.changerequestTSPform.get("TSPName"); }
  get Address() { return this.changerequestTSPform.get("Address"); }
  get HeadDesignation() { return this.changerequestTSPform.get("HeadDesignation"); }
  get HeadEmail() { return this.changerequestTSPform.get("HeadEmail"); }
  get HeadLandline() { return this.changerequestTSPform.get("HeadLandline"); }
  get CPDesignation() { return this.changerequestTSPform.get("CPDesignation"); }
  get CPLandline() { return this.changerequestTSPform.get("CPLandline"); }
  get CPEmail() { return this.changerequestTSPform.get("CPEmail"); }
  //get BankName() { return this.changerequestTSPform.get("BankName"); }
  get BankAccountNumber() { return this.changerequestTSPform.get("BankAccountNumber"); }
  get AccountTitle() { return this.changerequestTSPform.get("AccountTitle"); }
  //get BankBranch() { return this.changerequestTSPform.get("BankBranch"); }

  get InActive() { return this.changerequestTSPform.get("InActive"); }
  get Comments() { return this.changerequestTSPform.get("Comments"); }
  get IsApproved() { return this.changerequestTSPform.get("IsApproved"); }
  get IsRejected() { return this.changerequestTSPform.get("IsRejected"); }

  get ClassChangeRequestID() { return this.changerequestClassform.get("ClassChangeRequestID"); }
  get ClassID() { return this.changerequestClassform.get("ClassID"); }
  get ClassCode() { return this.changerequestClassform.get("ClassCode"); }
  get TrainingAddressLocation() { return this.changerequestClassform.get("TrainingAddressLocation"); }
  get ClassLocationDistrict() { return this.changerequestClassform.get("DistrictID"); }
  get ClassLocationTehsil() { return this.changerequestClassform.get("TehsilID"); }

  get ClassDatesChangeRequestID() { return this.changerequestClassDatesform.get("ClassDatesChangeRequestID"); }
  get ClassIDforDate() { return this.changerequestClassDatesform.get("ClassID"); }
  get ClassCodeForDate() { return this.changerequestClassDatesform.get("ClassCode"); }
  get StartDate() { return this.changerequestClassDatesform.get("StartDate"); }
  get EndDate() { return this.changerequestClassDatesform.get("EndDate"); }
  get Duration() { return this.changerequestClassDatesform.get("Duration"); }

  get InstructorChangeRequestID() { return this.changerequestInstructorform.get("InstructorChangeRequestID"); }
  get InstrID() { return this.changerequestInstructorform.get("InstrID"); }
  get InstructorName() { return this.changerequestInstructorform.get("InstructorName"); }
  get CNICofInstructor() { return this.changerequestInstructorform.get("CNICofInstructor"); }
  get QualificationHighest() { return this.changerequestInstructorform.get("QualificationHighest"); }
  get TotalExperience() { return this.changerequestInstructorform.get("TotalExperience"); }
  get PicturePath() { return this.changerequestInstructorform.get("PicturePath"); }
  get InstructorCRComments() { return this.changerequestInstructorform.get("InstructorCRComments"); }




  get SchemeChangeRequestID() { return this.changerequestSchemeform.get("SchemeChangeRequestID"); }
  get SchemeID() { return this.changerequestSchemeform.get("SchemeID"); }
  get SchemeName() { return this.changerequestSchemeform.get("SchemeName"); }
  get SchemeCode() { return this.changerequestSchemeform.get("SchemeCode"); }
  get BusinessRuleType() { return this.changerequestSchemeform.get("BusinessRuleType"); }
  get Stipend() { return this.changerequestSchemeform.get("Stipend"); }
  get StipendMode() { return this.changerequestSchemeform.get("StipendMode"); }
  get UniformAndBag() { return this.changerequestSchemeform.get("UniformAndBag"); }



  get TraineeChangeRequestID() { return this.changerequestTraineeform.get("TraineeChangeRequestID"); }
  get TraineeID() { return this.changerequestTraineeform.get("TraineeID"); }
  get TraineeName() { return this.changerequestTraineeform.get("TraineeName"); }
  get FatherName() { return this.changerequestTraineeform.get("FatherName"); }
  get TraineeCNIC() { return this.changerequestTraineeform.get("TraineeCNIC"); }
  get TraineeEmail() { return this.changerequestTraineeform.get("TraineeEmail"); }
  get CNICIssueDate() { return this.changerequestTraineeform.get("CNICIssueDate"); }
  get ContactNumber1() { return this.changerequestTraineeform.get("ContactNumber1"); }

  get TraineechangeImage() { return this.changerequestTraineeform.get("TraineechangeImage"); }
  get TraineePreImage() { return this.changerequestTraineeform.get("TraineePreImage"); }
  get TraineeClassCode() { return this.changerequestTraineeform.get("TraineeClassCode"); }
  get TraineeCode() { return this.changerequestTraineeform.get("TraineeCode"); }

  get TraineeHouseNumber() { return this.changerequestTraineeform.get("TraineeHouseNumber"); }
  get TraineeStreetMohalla() { return this.changerequestTraineeform.get("TraineeStreetMohalla"); }
  get TraineeMauzaTown() { return this.changerequestTraineeform.get("TraineeMauzaTown"); }
  get TraineeDistrictID() { return this.changerequestTraineeform.get("TraineeDistrictID"); }
  get TraineeTehsilID() { return this.changerequestTraineeform.get("TraineeTehsilID"); }
  get VerificationStatus() { return this.changerequestTraineeform.get("VerificationStatus"); }

  get IncepReportID() { return this.changerequestInceptionReportform.get("IncepReportID"); }
  get InceptionReportClassCode() { return this.changerequestInceptionReportform.get("ClassCode"); }
  get InceptionReportClassID() { return this.changerequestInceptionReportform.get("ClassID"); }
  get ClassStartTime() { return this.changerequestInceptionReportform.get("ClassStartTime"); }
  get ClassEndTime() { return this.changerequestInceptionReportform.get("ClassEndTime"); }
  get ClassTotalHours() { return this.changerequestInceptionReportform.get("ClassTotalHours"); }
  get Shift() { return this.changerequestInceptionReportform.get("Shift"); }
  //get CenterLocation() { return this.inceptionreportform.get("CenterLocation"); }
  get Monday() { return this.changerequestInceptionReportform.get("Monday"); }
  get Tuesday() { return this.changerequestInceptionReportform.get("Tuesday"); }
  get Wednesday() { return this.changerequestInceptionReportform.get("Wednesday"); }
  get Thursday() { return this.changerequestInceptionReportform.get("Thursday"); }
  get Friday() { return this.changerequestInceptionReportform.get("Friday"); }
  get Saturday() { return this.changerequestInceptionReportform.get("Saturday"); }
  get Sunday() { return this.changerequestInceptionReportform.get("Sunday"); }
  get FinalSubmitted() { return this.changerequestInceptionReportform.get("FinalSubmitted"); }

  get InstructorReplaceIncepReportID() { return this.changerequestReplaceInstructorform.get("IncepReportID"); }
  get InstructorReplaceClassCode() { return this.changerequestReplaceInstructorform.get("ClassCode"); }
  get InstructorReplaceClassID() { return this.changerequestReplaceInstructorform.get("ClassID"); }
  get InstrIDs() { return this.changerequestReplaceInstructorform.get("InstrIDs"); }

  get CRNewInstructorID() { return this.addInstructorform.get("CRNewInstructorID"); }
  get NewInstructorName() { return this.addInstructorform.get("InstructorName"); }
  get NewCNICofInstructor() { return this.addInstructorform.get("CNICofInstructor"); }
  get NewInstructorQualificationHighest() { return this.addInstructorform.get("QualificationHighest"); }
  get NewInstructorTotalExperience() { return this.addInstructorform.get("TotalExperience"); }
  get NewInstructorPicturePath() { return this.addInstructorform.get("PicturePath"); }
  get NewInstructorGenderID() { return this.addInstructorform.get("GenderID"); }
  get NewInstructorTradeID() { return this.addInstructorform.get("TradeID"); }
  get NewInstructorSchemeID() { return this.addInstructorform.get("SchemeID"); }
  get NewInstructorClassID() { return this.addInstructorform.get("ClassID"); }
  get NewInstructorLocationAddress() { return this.addInstructorform.get("LocationAddress"); }
  get FilePath() { return this.addInstructorform.get("FilePath"); }
  get NewInstructorCRComments() { return this.addInstructorform.get("NewInstructorCRComments"); }

}

export class ChangeRequestModel extends ModelBase {
  TSPDetailID: number;
  ClassID: number;
  ClassCode: string;
  AddressOfTrainingLocation: string;
  StartDate: string;
  EndDate: string;
  InstructorName: string;
  InstructorCNIC: string;
  Qualification: string;
  TotalExperience: number;

}

export interface IChangeRequestTraineeListFilter {
  SchemeID: number;
  ClassID: number;
  TraineeID: number;
  TradeID: number;

  //UserID: number;
}
//function doClassTimesOverlap(
//  currentClassStartTime: string, // Time in "HH:mm" format
//  currentClassEndTime: string,   // Time in "HH:mm" format
//  otherClassStartTime: string,   // Time in "HH:mm" format
//  otherClassEndTime: string      // Time in "HH:mm" format
//): boolean {
//  // Check for time overlap
//  const timeOverlap =
//    currentClassStartTime <= otherClassEndTime &&
//    currentClassEndTime >= otherClassStartTime;

//  // Return true if both date and time overlap
//  return  timeOverlap;
//}
function doClassTimesOverlap(currentStartTime: string, currentEndTime: string, otherStartTime: string, otherEndTime: string): boolean {
  // Convert string times to Date objects for comparison
  const currentStart = new Date('2000-01-01T' + currentStartTime); // Assuming the date part is not relevant
  const currentEnd = new Date('2000-01-01T' + currentEndTime);
  const otherStart = new Date('2000-01-01T' + otherStartTime);
  const otherEnd = new Date('2000-01-01T' + otherEndTime);

  // Check if the current class overlaps with another class
  const overlapWithOtherClass = (currentStart < otherEnd && currentEnd > otherStart);

  // Check if the current class falls within another class (i.e., instructor already assigned to another class)
  const fallsWithinOtherClass = (currentStart >= otherStart && currentEnd <= otherEnd);

  // If there's an overlap with another class or if the current class falls within another class, return true
  return overlapWithOtherClass || fallsWithinOtherClass;
}


