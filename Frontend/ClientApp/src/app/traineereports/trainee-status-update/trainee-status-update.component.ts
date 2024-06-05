import { Component, OnInit, ViewChild, ElementRef, AfterViewInit } from '@angular/core';
import { CommonSrvService } from '../../common-srv.service';
import { Element } from '@angular/compiler/src/render3/r3_ast';
import { MatProgressBar } from '@angular/material/progress-bar';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { FormGroup, FormBuilder, Validators, FormGroupDirective, FormArray, FormControl } from '@angular/forms';
import { environment } from '../../../environments/environment';
import { ModelBase } from '../../shared/ModelBase';
import { UserRightsModel, UsersModel } from '../../master-data/users/users.component';
import { MatDialog } from '@angular/material/dialog';
import { EnumCertificationAuthority, EnumTraineeResultStatusTypes, EnumUserLevel, ExportType, EnumTraineeStatusType, EnumClassStatus, EnumExcelReportType } from '../../shared/Enumerations';
import { DialogueService } from '../../shared/dialogue.service';
import * as XLSX from 'xlsx';
import { SearchFilter, ExportExcel } from '../../shared/Interfaces';
import { GroupByPipe } from 'angular-pipes';
import { DatePipe } from '@angular/common';
import { TsuDialogueComponent } from './tsu-dialogue/tsu-dialogue.component';
import { merge } from 'rxjs';
import { startWith, switchMap } from 'rxjs/operators';

@Component({
  selector: 'app-trainee-status-update',
  templateUrl: './trainee-status-update.component.html',
  styleUrls: ['./trainee-status-update.component.scss'],
  providers: [GroupByPipe, DatePipe]
})
export class TraineeStatusUpdateComponent implements OnInit, AfterViewInit {
  environment = environment;
  tsrDatasource: any[];
  displayedColumns = ['Sr', 'Action', 'TraineeCode', 'TraineeName', 'FatherName', 'TraineeCNIC', 'TraineeStatusName', 'TraineeStatusChangeReason'];
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

    // this.tsrDatasource = new MatTableDataSource([]);
    this.formrights = commonService.getFormRights();
  }

  ngOnInit() {
    this.commonService.setTitle('Trainee Status Update');
    this.currentUser = this.commonService.getUserDetails();
    if (this.currentUser.UserLevel === EnumUserLevel.TSP) {
      this.isTSPUser = true;
    } else if (this.currentUser.UserLevel === EnumUserLevel.AdminGroup || this.currentUser.UserLevel === EnumUserLevel.OrganizationGroup) {
      this.isInternalUser = true;
    }
    this.schemeFilter.valueChanges.subscribe(value => {
      if (this.currentUser.UserLevel === this.enumUserLevel.TSP) {
        this.getDependantFilters();
      }
      else {
        this.getTSPDetailByScheme(value);
      }
    });
    this.tspFilter.valueChanges.subscribe(value => { this.getClassesByTsp(value);})
  }
  ngAfterViewInit() {
    this.commonService.OID.subscribe(OID => {
      this.filters.SchemeID = 0;
      this.filters.TSPID = 0;
      this.filters.ClassID = 0;
      this.filters.OID = OID;

      this.getData();
      this.initPagedData();
      this.getTraineeResultStatusTypes();
      this.getKAMAssignment();
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
    this.tsrDatasource = data;
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
  getTraineeResultStatusTypes() {
    this.commonService.getJSON('api/TraineeResultStatusTypes/GetTraineeResultStatusTypes').subscribe(
      (data: any) => {
        this.traineeResultStatusTypeArray = data.filter(x => x.ResultStatusID != EnumTraineeResultStatusTypes.TestnotApplicable);
      }
      , (error) => this.error = error
    );
  }
  getKAMAssignment() {
    this.commonService.getJSON('api/KAMAssignment/RD_KAMAssignmentBy').subscribe(
      (data: any) => {
        this.kamAssignmentTSPs = data.filter(x => x.InActive === false).map(x => x.TspID);
        // this.kamAssignmentTSPs.includes(1)
        // debugger;
      }
      , (error) => this.error = error
    );
  }
  //getData() {
  //  this.commonService.getJSON(`api/TSRLiveData/GetTSRLiveData?OID=${this.commonService.OID.value}`).subscribe(
  //    (d: any) => {
  //      this.schemeArray = d.Schemes;
  //      this.fillTableForm(d.TSRLiveData)
  //    }
  //    , error => this.error = error // error path
  //  );
  //}

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
    this.commonService.getJSON(`api/TSRLiveData/GetFilteredTSRLiveData/${filters}`)
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
    const dialogRef = this.dialog.open(TsuDialogueComponent, {
      width: '600px',
      minHeight: '400px',
      data: { ...row }
    })
    dialogRef.afterClosed().subscribe(result => {
      console.log(result);
      this.updateTraineeStatus(result);
    })
  }
  openHistoryDialogue(data: any): void {
    this.dialogueService.openTraineeStatusHistoryDialogue(data.TraineeID);
  }
  updateTraineeResult() {
    if (!this.tsrTableForm.valid)
      return;
    /// when form control is disabled then control's value is undefined
    /// get data that have only traineeID is not undefined
    const values = this.tsrTableForm.value.tsrFormArray.filter(x => x.TraineeID)
    if (values.filter(x => x.ResultStatusID === EnumTraineeResultStatusTypes.None).length > 0) {
      this.commonService.openSnackBar('You should not mark as None \'ResultStatus\'.');
      return;
    } else {
      this.commonService.postJSON('api/TraineeProfile/UpdateTraineeResult', JSON.stringify(values)).subscribe(
        (data: any) => {
          const newDataList = this.tsrData.map(item => {
            const foundItem = data.find(x => x.TraineeID === item.TraineeID);
            if (foundItem) {
              return {
                ...item
                , ResultStatusID: foundItem.ResultStatusID
                , ResultStatusChangeReason: foundItem.ResultStatusChangeReason
                , ResultDocument: foundItem.ResultDocument
              }
            } else {
              return item;
            }
          });
          this.fillTableForm(newDataList);
          this.commonService.openSnackBar('Saved Successfully');
        }
        , (error) => this.commonService.ShowError(error.error + '\n' + error.message)
      );
    }
  }
  updateTraineeStatus(traineeData: any): void {
    if (traineeData) {
      this.commonService.getJSON(`api/TraineeProfile/UpdateTraineeStatus?TraineeID=${traineeData.TraineeID}&TraineeStatusTypeID=${traineeData.TraineeStatusTypeID}&TraineeStatusReason=${traineeData.TraineeReason}`).subscribe(
        data => {
          if (data === true) {
            this.tsrData = this.tsrData.map(x =>
              x.TraineeID === traineeData.TraineeID
                ? {
                  ...x
                  , TraineeStatusTypeID: traineeData.TraineeStatusTypeID
                  , TraineeStatusName: traineeData.StatusName
                  , TraineeStatusChangeReason: traineeData.TraineeReason
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
    // this.tsrDatasource.filter = filterValue;
  }
  // Pagination\\
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
        this.tsrDatasource = data[0];
        this.tsrData = data[0];
        this.resultsLength = data[1].TotalCount;
      }, error => {
        this.tsrDatasource = [];
        this.tsrData = [];
        this.resultsLength = 0;
        this.error = error;
      });
  }
  getPagedData(pagingModel, filterModel) {
    return this.commonService.postJSON('api/TSRLiveData/RD_TSUPaged', { pagingModel, filterModel });
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

  openTraineeJourneyDialogue(data: any): void
  {
    // debugger;
    this.dialogueService.openTraineeJourneyDialogue(data);
  }

  // onFileChange(ev: any) {
  //  let sheetName = 'TCR';
  //  let workBook = null;
  //  let jsonData = null;
  //  const reader = new FileReader();
  //  const file = ev.target.files[0];
  //  reader.onload = (event) => {
  //    const data = reader.result;
  //    workBook = XLSX.read(data, { type: 'binary' });
  //    jsonData = workBook.SheetNames.reduce((initial, name) => {
  //      const sheet = workBook.Sheets[name];
  //      initial[name] = XLSX.utils.sheet_to_json(sheet);
  //      return initial;
  //    }, {});

  //    const jData = JSON.parse(JSON.stringify(jsonData));
  //    this.populateFieldsFromFile(jData[sheetName]);

  //  }
  //  reader.readAsBinaryString(file);
  //  //ev.target.value = '';
  // }
  // populateFieldsFromFile(data: any) {
  //  if (data.length > 0) {
  //    //let enbaledRows = this.tsrTableForm.value.tsrFormArray.filter(x => x.TraineeID);
  //    let newDataList = this.tsrData.map(item => {
  //      let foundItem = data.find(x => x.TraineeCode.trim() == item.TraineeCode.trim());
  //      if (foundItem
  //        //&& enbaledRows.find(x => x.TraineeID == foundItem.TraineeID) != undefined
  //      ) {
  //        return {
  //          ...item
  // tslint:disable-next-line: max-line-length
  //          , ResultStatusID: this.traineeResultStatusTypeArray.find(x => x.ResultStatusName.toLowerCase() == foundItem.ResultStatus.toLowerCase()).ResultStatusID
  //          , ResultStatusChangeReason: foundItem.Comments
  //        }
  //      } else {
  //        return item;
  //      }
  //    });
  //    this.fillTableForm(newDataList);
  //    this.commonService.openSnackBar("Upload excel data Successfully");
  //  }
  // }

  // exportToExcel() {

  //  let filteredData = [...this.tsrDatasource.filteredData];
  //  let data = {
  //    "Filters:": '',
  //    "TraineeStatus": 'All',
  //    "Scheme(s)": this.groupByPipe.transform(filteredData, 'SchemeName').map(x => x.key).join(','),
  //    "TSP(s)": this.groupByPipe.transform(filteredData, 'TSPName').map(x => x.key).join(','),
  //    "TraineeImagesAdded": true
  //  };
  //  //let removeKeys = Object.keys(filteredData[0]).filter(x => !this.displayedColumns.includes(x));
  //  //let dataList = [];//filteredData.map(x => { removeKeys.forEach(key => delete x[key]); return x });
  //  //filteredData.forEach(item => {
  //  //  let obj = {};
  //  //  this.displayedColumns.forEach(key => {
  //  //    obj[key] = item[key] || "";
  //  //  });
  //  //  dataList.push(obj)
  //  //})
  //  let exportExcel: ExportExcel = {
  //    Title: 'Trainee Status Report',
  //    Author: this.currentUser.FullName,
  //    Type: EnumExcelReportType.TSR,
  //    Data: data,
  //    List1: this.populateData(filteredData),
  //    ImageFieldNames: ["Trainee Img", "CNIC Img"]
  //  };
  //  this.dialogueService.openExportConfirmDialogue(exportExcel).subscribe();
  // }
  // populateData(data: any) {
  //  return data.map((item, index) => {
  //    return {
  //      "Sr#": ++index
  //      , "Scheme": item.SchemeName
  //      , "Training Service Provider": item.TSPName
  //      , "Trade Group": item.SectorName
  //      , "Trade": item.TradeName
  //      , "ClassCode": item.ClassCode
  //      , "Trainee ID": item.TraineeCode
  //      , "Trainee Name": item.TraineeName
  //      , "Father's Name": item.FatherName
  //      , "CNIC Issue Date": this.datePipe.transform(item.CNICIssueDate, 'dd/MM/yyyy')
  //      , "CNIC": item.TraineeCNIC
  //      , "Date Of Birth": this.datePipe.transform(item.DateOfBirth, 'dd/MM/yyyy')
  //      , "Roll #": item.TraineeRollNumber
  //      , "Batch": item.Batch
  //      , "Section": item.SectionName
  //      , "Shift": item.Shift == '1st' ? 'Morning' : 'Evening'
  // tslint:disable-next-line: max-line-length
  //      , "Trainee Address": `${item.TraineeHouseNumber} ,${item.TraineeStreetMohalla} , ${item.TraineeMauzaTown} , ${item.TraineeTehsilName} , ${item.TraineeDistrictName}`
  //      , "Residence Tehsil": item.TraineeTehsilName
  //      , "District of Residence": item.TraineeDistrictName
  //      , "Gender": item.GenderName
  //      , "Education": item.Education
  //      , "Contact Number": item.ContactNumber1
  //      , "Training Location": `${item.TrainingAddressLocation}`
  //      , "District of Training Location": item.ClassDistrictName
  //      , "CNIC Verified": item.CNICVerified ? 'Yes' : 'No'
  //      , "Trainee Status": item.TraineeStatusName
  //      , "Is Dual": item.IsDual ? 'Yes' : 'No'
  //      , "Trainee Status Update Date": this.datePipe.transform(item.TraineeStatusChangeDate, 'dd/MM/yyyy')
  //      , "Examination Assesment": item.ResultStatusName
  //      , "Voucher Holder": item.VoucherHolder ? 'Yes' : 'No'
  //      , "Reason": item.TraineeStatusChangeReason
  //      , "Class ID": item.ClassUID
  //      , "Trainee Profile ID": item.TraineeUID
  //      , "CNIC IMG Status": item.IsVarifiedCNIC ? 1 : 0
  //      , "Trainee Img": item.TraineeImg
  //      //, "Trainee Age": item.TraineeAge
  //      //, "Class Start Date": this.datePipe.transform(item.ClassStartDate, 'dd/MM/yyyy')
  //      // , "Class End Date": this.datePipe.transform(item.ClassStartDate, 'dd/MM/yyyy')
  //      //, "Class Status": item.ClassStatusName
  //      , "CNIC Img": item.CNICImg
  //      // , "Certification Body": item.CertAuthName
  //      , "Sector": item.SectorName
  //      , "Cluster": item.ClusterName
  //      , "KAM": item.KAM
  //      , "Trainee Employment Status": item.TraineeEmploymentStatus ? 'Yes' : 'No'
  //      , "Trainee Employment Verification Status": item.TraineeEmploymentVerificationStatus ? 'Yes' : 'No'
  //    }
  //  })
  // }
}

