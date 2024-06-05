import { Component, OnInit, ViewChild, ElementRef, AfterViewInit } from '@angular/core';
import { CommonSrvService } from '../../../common-srv.service';
import { Element } from '@angular/compiler/src/render3/r3_ast';
import { MatProgressBar } from '@angular/material/progress-bar';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { FormGroup, FormBuilder, Validators, FormGroupDirective, FormArray, FormControl } from '@angular/forms';
import { environment } from '../../../../environments/environment';
import { ModelBase } from '../../../shared/ModelBase';
import { UserRightsModel, UsersModel } from '../../../master-data/users/users.component';
import { MatDialog } from '@angular/material/dialog';
import { EnumCertificationAuthority, EnumTraineeResultStatusTypes, EnumUserLevel, ExportType, EnumTraineeStatusType, EnumClassStatus, EnumExcelReportType } from '../../../shared/Enumerations';
import { DialogueService } from '../../../shared/dialogue.service';
import * as XLSX from 'xlsx';
import { SearchFilter, ExportExcel } from '../../../shared/Interfaces';
import { GroupByPipe } from 'angular-pipes';
import { DatePipe } from '@angular/common';
import { ClassStatusDialougeComponent } from '../class-status-dialouge/class-status-dialouge.component';
import { merge } from 'rxjs';
import { startWith, switchMap } from 'rxjs/operators';

@Component({
  selector: 'app-class-status-update',
  templateUrl: './class-status-update.component.html',
  styleUrls: ['./class-status-update.component.scss'],
  providers: [GroupByPipe, DatePipe]
})

export class ClassStatusUpdateComponent implements OnInit, AfterViewInit {
  environment = environment;
  ClassDatasource: any[];
  displayedColumns = ['Sr', 'Action', 'ClassCode', 'ClassStatusName', 'Duration', 'StartDate', 'EndDate'];
  schemeArray = [];
  tspDetailArray = [];
  classesArray: any[];
  filters: SearchFilter = { SchemeID: 0, TSPID: 0, ClassID: 0, TraineeID: 0, OID: this.commonService.OID.value, SelectedColumns: [] };
  @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: false }) sort: MatSort;
  error = '';
  schemeFilter = new FormControl(0);
  tspFilter = new FormControl(0);
  classFilter = new FormControl(0);
  SearchSch = new FormControl('');
  SearchTSP = new FormControl('');
  SearchCls = new FormControl('');
  resultsLength: number;
  currentUser: UsersModel;
  enumUserLevel = EnumUserLevel;

  formrights: UserRightsModel;
  errorHTTP: any;
  traineeResultStatusTypeArray: any;
  tsrData: any[];
  tsrTableForm: FormGroup;
  isInternalUser = false;
  isTSPUser = false;
  kamAssignmentTSPs: any[] = [];
  constructor(private fb: FormBuilder, private commonService: CommonSrvService, public dialog: MatDialog, public dialogueService: DialogueService, private groupByPipe: GroupByPipe, private datePipe: DatePipe) {
    this.tsrTableForm = this.fb.group({
      tsrFormArray: this.fb.array([])
    });

    // this.ClassDatasource = new MatTableDataSource([]);
    this.formrights = commonService.getFormRights();
  }

  ngOnInit() {
    this.commonService.setTitle('Class Status Update');
    this.currentUser = this.commonService.getUserDetails();
    if (this.currentUser.UserLevel === EnumUserLevel.TSP) {
      this.isTSPUser = true;
    } else if (this.currentUser.UserLevel === EnumUserLevel.AdminGroup || this.currentUser.UserLevel === EnumUserLevel.OrganizationGroup) {
      this.isInternalUser = true;
    }
    this.schemeFilter.valueChanges.subscribe(value => {
      if (this.currentUser.UserLevel === this.enumUserLevel.TSP) {
        //this.getDependantFilters();
      }
      else {
        this.getTSPDetailByScheme(value);
      }
    });
    this.tspFilter.valueChanges.subscribe(value => { this.getClassesByTsp(value); })
  }
  ngAfterViewInit() {
    this.commonService.OID.subscribe(OID => {
      this.filters.SchemeID = 0;
      this.filters.TSPID = 0;
      this.filters.ClassID = 0;
      this.filters.OID = OID;

      this.getData();
      this.initPagedData();
    })
  }
  EmptyCtrl(Ev: any) {
    this.SearchCls.setValue('');
    this.SearchTSP.setValue('');
    this.SearchSch.setValue('');
  }
  getSchemesByOrg(oid: number) {
    // this.schemeArray = [];
    this.commonService.getJSON(`api/Dashboard/FetchSchemes`)
      .subscribe(data => {
        this.schemeArray = (data as any[]);
      }, error => {
        this.error = error;
      })
  }
  getTSPDetailByScheme(schemeId: number) {
    this.tspFilter.setValue(0);
    this.classFilter.setValue(0);
    // this.tspDetailArray = [];
    // this.classesArray = [];
    this.commonService.getJSON(`api/Dashboard/FetchTSPsByScheme?SchemeID=` + schemeId)
      .subscribe(data => {
        this.tspDetailArray = (data as any[]);
      }, error => {
        this.error = error;
      })
  }
  getClassesByTsp(tspId: number) {
    this.classFilter.setValue(0);
    // this.classesArray = [];
    this.commonService.getJSON(`api/Dashboard/FetchClassesByTSP?TspID=${tspId}`)
      .subscribe(data => {
        this.classesArray = (data as any[]);
      }, error => {
        this.error = error;
      })
  }

  fillTableForm(data: any[]) {
    this.tsrFormArray.clear();
    data.forEach(x => {
      const form = this.getNewForm(x);
      form.patchValue(x);
      this.tsrFormArray.push(form);
    });
    data = data.map((item, index) => { return { ...item, Sr: index + 1 } })
    console.log(data);
    this.ClassDatasource = data;
    this.tsrData = data;
  }
  getNewForm(data: any): FormGroup {
    const form = this.fb.group({
      TraineeID: [0],
      ResultStatusID: [0, Validators.required],
      ResultStatusChangeReason: ['', Validators.required],
      ResultDocument: [''],
      // ControlDisabled:[false]
    }, { updateOn: 'change' });
    form.get('ResultStatusID').valueChanges.subscribe(
      value => {
        const control = form.get('ResultDocument');
        const contrTraineeID = form.get('TraineeID');
        if (value === EnumTraineeResultStatusTypes.Pass && contrTraineeID.enabled) {
          control.enable();
          control.setValidators(Validators.required);
          control.markAllAsTouched();
          control.updateValueAndValidity();
        } else {
          control.disable();
        }
      })
    /// BR
    const noOfDays = data.TSROpeningDays;
    const today = new Date();
    const classEndDate = typeof (data.ClassEndDate) === 'string' ? new Date(data.ClassEndDate) : data.ClassEndDate;
    const deadline = new Date(classEndDate);
    deadline.setDate(classEndDate.getDate() + noOfDays);

    if (data.CertAuthID === EnumCertificationAuthority.PBTE
      || data.CertAuthID === EnumCertificationAuthority.NAVTEC
      || data.TraineeStatusTypeID != EnumTraineeStatusType.Completed
      || data.ClassStatusID != EnumClassStatus.Completed
      || data.IsCreatedPRNCompletion === true
      || !(today >= classEndDate && today <= deadline)
    ) {
      form.get('TraineeID').disable();
      form.get('ResultStatusID').disable();
      form.get('ResultStatusChangeReason').disable();
      form.get('ResultDocument').disable();
      // form.get("ControlDisabled").patchValue(true);
    }
    return form;
  }
  get tsrFormArray() { return this.tsrTableForm.get('tsrFormArray') as FormArray }
  
  getData() {
    //this.commonService.getJSON(`api/TSRLiveData/GetTSRLiveData?OID=${this.commonService.OID.value}`).subscribe(
    this.commonService.getJSON(`api/TSRLiveData/GetSchemesForTSR?OID=${this.commonService.OID.value}`).subscribe(
      (d: any) => {
        this.schemeArray = d.Schemes;
        console.log(this.schemeArray);
        // this.fillTableForm(d.TSRLiveData)
      }
      , error => this.error = error // error path
    );
  }

  getFilteredTSRLiveData() {
    const filters = 'filters?' + Object.entries(this.filters).map(([key, value]) => `filters=${value}`).join('&');
    this.commonService.getJSON(`api/ClassStatusUpdate/GetFilteredClassData/${filters}`)
      .subscribe((data: any) => {
        this.fillTableForm(data);
        // debugger;
      },
        error => {
          this.fillTableForm([]);
          this.error = error;
        })
  }

  openDialog(row: any): void {
    const dialogRef = this.dialog.open(ClassStatusDialougeComponent, {
      width: '600px',
      minHeight: '400px',
      data: { ...row }
    })
    dialogRef.afterClosed().subscribe(result => {
      console.log(result);
      this.updateTraineeStatus(result);
    })
  }
  updateTraineeStatus(ClassData: any): void {
    if (ClassData) {
      
      this.commonService.getJSON(`api/ClassStatusUpdate/CSUpdate?ClassID=${ClassData.ClassID}&ClassStatusID=${ClassData.ClassStatusID}&ClassReason=${ClassData.ClassReason}`).subscribe(
        data => {
          if (data === true) {
            this.tsrData = this.tsrData.map(x =>
              x.ClassID === ClassData.ClassID
                ? {
                  ...x
                  , ClassStatusID: ClassData.ClassStatusID
                  , ClassStatusName: ClassData.StatusName
                }
                : x);
            this.fillTableForm(this.tsrData);
            this.commonService.openSnackBar('Saved Successfully');
          }
        }
        , error => {
          this.commonService.ShowError(error.error + '\n' + error.message);
        });
    }
  }

  applyFilter(filterValue: string) {
    filterValue = filterValue.trim(); // Remove whitespace
    filterValue = filterValue.toLowerCase(); // Datasource defaults to lowercase matches
    // this.ClassDatasource.filter = filterValue;
  }
  //|Pagination|\\
  initPagedData() {
    // debugger;
    this.sort.sortChange.subscribe(() => this.paginator.pageIndex = 0);
    this.paginator.pageSize = 5;
    // tslint:disable-next-line: max-line-length
    merge(this.sort.sortChange, this.paginator.page, this.schemeFilter.valueChanges.pipe(), this.tspFilter.valueChanges, this.classFilter.valueChanges).pipe(
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
        this.filters.SchemeID = this.schemeFilter.value
        this.filters.TSPID = this.tspFilter.value
        this.filters.ClassID = this.classFilter.value
        return this.getPagedData(pagedModel, this.filters);
      })).subscribe(data => {
        // debugger;
        this.ClassDatasource = data[0];
        this.tsrData = data[0];
        this.resultsLength = data[1].TotalCount;
      }, error => {
        this.ClassDatasource = [];
        this.tsrData = [];
        this.resultsLength = 0;
        this.error = error;
      });
  }
  getPagedData(pagingModel, filterModel) {
    return this.commonService.postJSON('api/ClassStatusUpdate/RD_GetFilteredClassData', { pagingModel, filterModel });
  }

  getDependantFilters() {
    if (this.currentUser.UserLevel === this.enumUserLevel.TSP) {
      this.getClassesBySchemeFilter();
    }
    else {
      this.getTSPDetailByScheme(this.schemeFilter.value);
    }
  }

  getClassesBySchemeFilter() {
    this.filters.ClassID = 0;
    this.filters.TraineeID = 0;
    this.commonService.getJSON(`api/Dashboard/FetchClassesBySchemeUser?SchemeID=${this.schemeFilter.value}&UserID=${this.currentUser.UserID}`)
      .subscribe(data => {
        this.classesArray = (data as any[]);
        // this.activeClassesArrayFilter = this.classesArrayFilter.filter(x => x.ClassStatusID == 3);
      }, error => {
        this.error = error;
      })
  }

  openTraineeJourneyDialogue(data: any): void {
    // debugger;
    this.dialogueService.openTraineeJourneyDialogue(data);
  }

}
