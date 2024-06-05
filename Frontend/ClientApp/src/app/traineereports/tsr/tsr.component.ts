import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
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
import { TSRDialogComponent } from './tsr-dialog/tsr-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { EnumCertificationAuthority, EnumTraineeResultStatusTypes, EnumUserLevel, ExportType, EnumTraineeStatusType, EnumClassStatus, EnumExcelReportType } from '../../shared/Enumerations';
import { DialogueService } from '../../shared/dialogue.service';
import * as XLSX from 'xlsx';
import { SearchFilter, ExportExcel } from '../../shared/Interfaces';
import { GroupByPipe } from 'angular-pipes';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'traineereports',
  templateUrl: './tsr.component.html',
  styleUrls: ['./tsr.component.scss'],
  providers: [GroupByPipe, DatePipe]
})
export class TraineereportsComponent implements OnInit {
  environment = environment;
  title: string;
  savebtn: string;
  //displayedColumns = ['Action', 'TraineeCode', 'TraineeName', 'TraineeCNIC', "GenderName", "TraineeStatus", "ResultStatusID", "ResultStatusChangeReason", "ResultDocument"];
  //displayedColumns = ['Sr','Action', 'SchemeName', 'SchemeCode', 'TSPName', 'ClassCode', 'TraineeCode', 'TraineeName', 'FatherName', 'TraineeCNIC', 'CNICIssueDate', 'GenderName', 'DateOfBirth', 'TraineeRollNumber', 'Batch', 'SectionName', 'CNICVerified', 'Education', 'ContactNumber1', 'IsExtra', 'TradeName', 'VoucherHolder', 'TraineeAge', 'TraineeImg', 'ClassStartDate', 'ClassEndDate', 'ClassStatusName', 'CertAuthName', "TraineeStatusName", "TraineeEmploymentStatus", "TraineeEmploymentVerificationStatus", "ClusterName", "SectorName", "KAM", "ResultStatusID", "ResultStatusChangeReason", "ResultDocument"];

  displayedColumns = ['Sr', 'TraineeCode', 'TraineeName', 'FatherName', 'TraineeCNIC', 'GenderName', "ContactNumber1", 'TraineeAge', "TraineeDistrictName", "GuardianNextToKinName", "TraineeStatusName"];

  schemeArray = [];
  tspDetailArray = [];
  classesArray: any[];
  formrights: UserRightsModel;
  error: String;
  errorHTTP: any;
  query = {
    order: 'TraineeID',
    limit: 5,
    page: 1
  };
  filters: SearchFilter = { SchemeID: 0, TSPID: 0, ClassID: 0, TraineeID: 0, OID: this.commonService.OID.value };
  traineeResultStatusTypeArray: any;
  tsrData: any[];
  tsrDatasource: MatTableDataSource<any[]> = new MatTableDataSource([]);
  tsrTableForm: FormGroup;
  isInternalUser: boolean = false;
  isTSPUser: boolean = false;
  currentUser: UsersModel;
  kamAssignmentTSPs: any[] = [];
  @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: false }) sort: MatSort;
  constructor(private fb: FormBuilder, private commonService: CommonSrvService, public dialog: MatDialog, public dialogueService: DialogueService, private groupByPipe: GroupByPipe, private datePipe: DatePipe) {
    this.tsrTableForm = this.fb.group({
      tsrFormArray: this.fb.array([])
    });

    //this.tsrDatasource = new MatTableDataSource([]);
    this.formrights = commonService.getFormRights();
  }
  SearchSch = new FormControl('');
  SearchCls = new FormControl('');
  SearchTSP = new FormControl('');
  EmptyCtrl(Ev: any) {
    this.SearchCls.setValue('');
    this.SearchTSP.setValue('');
    this.SearchSch.setValue('');
  }
  ngOnInit() {
    this.commonService.setTitle("Trainee Status Report");
    this.title = "Add New ";
    this.savebtn = "Save ";

    this.currentUser = this.commonService.getUserDetails();
    if (this.currentUser.UserLevel == EnumUserLevel.TSP) {
      this.isTSPUser = true;
    } else if (this.currentUser.UserLevel == EnumUserLevel.AdminGroup || this.currentUser.UserLevel == EnumUserLevel.OrganizationGroup) {
      this.isInternalUser = true;
    }

    this.commonService.OID.subscribe(OID => {
      this.filters.SchemeID = 0;
      this.filters.TSPID = 0;
      this.filters.ClassID = 0;

      this.getTraineeResultStatusTypes();
      this.getKAMAssignment();
      this.getData();
    })
  }

  fillTableForm(data: any[]) {
    this.tsrFormArray.clear();
    data.forEach(x => {
      let form = this.getNewForm(x);
      form.patchValue(x);
      this.tsrFormArray.push(form);
    });

    data = data.map((item, index) => { return { ...item, Sr: index + 1 } })

    this.tsrDatasource = new MatTableDataSource(data);
    this.tsrDatasource.paginator = this.paginator;
    this.tsrDatasource.sort = this.sort;
    this.tsrData = data;
  }
  getNewForm(data: any): FormGroup {
    let form = this.fb.group({
      TraineeID: [0],
      ResultStatusID: [0, Validators.required],
      ResultStatusChangeReason: ['', Validators.required],
      ResultDocument: [''],
      //ControlDisabled:[false]
    }, { updateOn: "change" });
    form.get("ResultStatusID").valueChanges.subscribe(
      value => {
        let control = form.get("ResultDocument");
        let contrTraineeID = form.get("TraineeID");
        if (value == EnumTraineeResultStatusTypes.Pass && contrTraineeID.enabled) {
          control.enable();
          control.setValidators(Validators.required);
          control.markAllAsTouched();
          control.updateValueAndValidity();
        } else {
          control.disable();
        }
      })
    ///BR
    let noOfDays = data.TSROpeningDays;
    let today = new Date();
    let classEndDate = typeof (data.ClassEndDate) == "string" ? new Date(data.ClassEndDate) : data.ClassEndDate;
    let deadline = new Date(classEndDate);
    deadline.setDate(classEndDate.getDate() + noOfDays);

    if (data.CertAuthID == EnumCertificationAuthority.PBTE
      || data.CertAuthID == EnumCertificationAuthority.NAVTEC
      || data.TraineeStatusTypeID != EnumTraineeStatusType.Completed
      || data.ClassStatusID != EnumClassStatus.Completed
      || data.IsCreatedPRNCompletion == true
      || !(today >= classEndDate && today <= deadline)
    ) {
      form.get("TraineeID").disable();
      form.get("ResultStatusID").disable();
      form.get("ResultStatusChangeReason").disable();
      form.get("ResultDocument").disable();
      //form.get("ControlDisabled").patchValue(true);
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
        this.kamAssignmentTSPs = data.filter(x => x.InActive == false).map(x => x.TspID);
        //this.kamAssignmentTSPs.includes(1)
        //debugger;
      }
      , (error) => this.error = error
    );
  }
  getData() {
    this.commonService.getJSON(`api/TSRLiveData/GetTSRLiveData?OID=${this.commonService.OID.value}`).subscribe(
      (d: any) => {
        //this.fillTableForm(d.TSRData);
        this.schemeArray = d.Schemes;
        //if (!this.isTSPUser) {
        this.schemeArray.unshift({ SchemeID: 0, SchemeName: "-- All --" });
        console.log(this.schemeArray);
        //}
        //this.tspDetailArray = d.TSPDetail;
        ///Set Default First value Selected
        //this.filters.SchemeID = this.schemeArray[0].SchemeID;
        this.fillTableForm(d.TSRLiveData)
      }
      , error => this.error = error // error path
    );
  }
  getFilteredTSRLiveData() {
    let filters = "filters?" + Object.entries(this.filters).map(([key, value]) => `filters=${value}`).join('&');
    this.commonService.getJSON(`api/TSRLiveData/GetFilteredTSRLiveData/${filters}`)
      .subscribe((data: any) => {
        this.fillTableForm(data);
        //debugger;
      },
        error => {
          this.fillTableForm([]);
          this.error = error;
        })
  }
  getTSPDetailByScheme(schemeId: number) {
    this.filters.TSPID = 0;
    this.filters.ClassID = 0;
    this.classesArray = [];
    this.commonService.getJSON(`api/TSPDetail/GetTSPDetailByScheme/` + schemeId)
      .subscribe(data => {
        this.tspDetailArray = <any[]>data;
      }, error => {
        this.error = error;
      })
  }
  getClassesByTsp(tspId: number) {
    this.filters.ClassID = 0;
    this.commonService.getJSON(`api/Class/GetClassesByTsp/` + tspId)
      .subscribe(data => {
        this.classesArray = <any[]>data;
      }, error => {
        this.error = error;
      })
  }

  openDialog(row: any): void {
    const dialogRef = this.dialog.open(TSRDialogComponent, {
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
    let values = this.tsrTableForm.value.tsrFormArray.filter(x => x.TraineeID)
    if (values.filter(x => x.ResultStatusID == EnumTraineeResultStatusTypes.None).length > 0) {
      this.commonService.openSnackBar("You should not mark as None 'ResultStatus'.");
      return;
    } else {
      this.commonService.postJSON('api/TraineeProfile/UpdateTraineeResult', JSON.stringify(values)).subscribe(
        (data: any) => {
          let newDataList = this.tsrData.map(item => {
            let foundItem = data.find(x => x.TraineeID == item.TraineeID);
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
          this.commonService.openSnackBar("Saved Successfully");
        }
        , (error) => this.commonService.ShowError(error.error + '\n' + error.message)
      );
    }
  }

  updateTraineeStatus(traineeData: any): void {
    if (traineeData) {
      this.commonService.getJSON(`api/TraineeProfile/UpdateTraineeStatus?TraineeID=${traineeData.TraineeID}&TraineeStatusTypeID=${traineeData.TraineeStatusTypeID}`).subscribe(
        data => {
          if (data == true) {
            this.tsrData = this.tsrData.map(x =>
              x.TraineeID == traineeData.TraineeID
                ? {
                  ...x
                  , TraineeStatusTypeID: traineeData.TraineeStatusTypeID
                  , TraineeStatusName: traineeData.StatusName
                }
                : x);
            this.fillTableForm(this.tsrData);
            this.commonService.openSnackBar("Saved Successfully");
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
    this.tsrDatasource.filter = filterValue;
  }

  onFileChange(ev: any) {
    let sheetName = 'TCR';
    let workBook = null;
    let jsonData = null;
    const reader = new FileReader();
    const file = ev.target.files[0];
    reader.onload = (event) => {
      const data = reader.result;
      workBook = XLSX.read(data, { type: 'binary' });
      jsonData = workBook.SheetNames.reduce((initial, name) => {
        const sheet = workBook.Sheets[name];
        initial[name] = XLSX.utils.sheet_to_json(sheet);
        return initial;
      }, {});

      const jData = JSON.parse(JSON.stringify(jsonData));
      this.populateFieldsFromFile(jData[sheetName]);

    }
    reader.readAsBinaryString(file);
    //ev.target.value = '';
  }
  populateFieldsFromFile(data: any) {
    if (data.length > 0) {
      //let enbaledRows = this.tsrTableForm.value.tsrFormArray.filter(x => x.TraineeID);
      let newDataList = this.tsrData.map(item => {
        let foundItem = data.find(x => x.TraineeCode.trim() == item.TraineeCode.trim());
        if (foundItem
          //&& enbaledRows.find(x => x.TraineeID == foundItem.TraineeID) != undefined
        ) {
          return {
            ...item
            , ResultStatusID: this.traineeResultStatusTypeArray.find(x => x.ResultStatusName.toLowerCase() == foundItem.ResultStatus.toLowerCase()).ResultStatusID
            , ResultStatusChangeReason: foundItem.Comments
          }
        } else {
          return item;
        }
      });
      this.fillTableForm(newDataList);
      this.commonService.openSnackBar("Upload excel data Successfully");
    }
  }

  exportToExcel() {

    let filteredData = [...this.tsrDatasource.filteredData];
    let data = {
      "Filters:": '',
      "TraineeStatus": 'All',
      "Scheme(s)": this.groupByPipe.transform(filteredData, 'SchemeName').map(x => x.key).join(','),
      "TSP(s)": this.groupByPipe.transform(filteredData, 'TSPName').map(x => x.key).join(','),
      "TraineeImagesAdded": true
    };
    //let removeKeys = Object.keys(filteredData[0]).filter(x => !this.displayedColumns.includes(x));
    //let dataList = [];//filteredData.map(x => { removeKeys.forEach(key => delete x[key]); return x });
    //filteredData.forEach(item => {
    //  let obj = {};
    //  this.displayedColumns.forEach(key => {
    //    obj[key] = item[key] || "";
    //  });
    //  dataList.push(obj)
    //})
    let exportExcel: ExportExcel = {
      Title: 'Trainee Status Report',
      Author: this.currentUser.FullName,
      Type: EnumExcelReportType.TSR,
      Data: data,
      List1: this.populateData(filteredData),
      ImageFieldNames: ["Trainee Img", "CNIC Img"]
    };
    this.dialogueService.openExportConfirmDialogue(exportExcel).subscribe();
  }
  populateData(data: any) {
    return data.map((item, index) => {
      return {
        "Sr#": ++index
        , "Scheme": item.SchemeName
        , "Training Service Provider": item.TSPName
        , "Trade Group": item.SectorName
        , "Trade": item.TradeName
        , "ClassCode": item.ClassCode
        , "Trainee ID": item.TraineeCode
        , "Trainee Name": item.TraineeName
        , "Father's Name": item.FatherName
        , "CNIC Issue Date": this.datePipe.transform(item.CNICIssueDate, 'dd/MM/yyyy')
        , "CNIC": item.TraineeCNIC
        , "Date Of Birth": this.datePipe.transform(item.DateOfBirth, 'dd/MM/yyyy')
        , "Roll #": item.TraineeRollNumber
        , "Batch": item.Batch
        , "Section": item.SectionName
        , "Shift": item.Shift == '1st' ? 'Morning' : 'Evening'
        , "Trainee Address": `${item.TraineeHouseNumber} ,${item.TraineeStreetMohalla} , ${item.TraineeMauzaTown} , ${item.TraineeTehsilName} , ${item.TraineeDistrictName}`
        , "Residence Tehsil": item.TraineeTehsilName
        , "District of Residence": item.TraineeDistrictName
        , "Gender": item.GenderName
        , "Education": item.Education
        , "Contact Number": item.ContactNumber1
        , "Training Location": `${item.TrainingAddressLocation}`
        , "District of Training Location": item.ClassDistrictName
        , "CNIC Verified": item.CNICVerified ? 'Yes' : 'No'
        , "Trainee Status": item.TraineeStatusName
        , "Is Dual": item.IsDual ? 'Yes' : 'No'
        , "Trainee Status Update Date": this.datePipe.transform(item.TraineeStatusChangeDate, 'dd/MM/yyyy')
        , "Examination Assesment": item.ResultStatusName
        , "Voucher Holder": item.VoucherHolder ? 'Yes' : 'No'
        , "Reason": item.TraineeStatusChangeReason
        , "Class ID": item.ClassUID
        , "Trainee Profile ID": item.TraineeUID
        , "CNIC IMG Status": item.IsVarifiedCNIC ? 1 : 0
        , "Trainee Img": item.TraineeImg
        //, "Trainee Age": item.TraineeAge
        //, "Class Start Date": this.datePipe.transform(item.ClassStartDate, 'dd/MM/yyyy')
        // , "Class End Date": this.datePipe.transform(item.ClassStartDate, 'dd/MM/yyyy')
        //, "Class Status": item.ClassStatusName
        , "CNIC Img": item.CNICImgNADRA
        // , "Certification Body": item.CertAuthName
        , "Sector": item.SectorName
        , "Cluster": item.ClusterName
        , "KAM": item.KAM
        , "Trainee Employment Status": item.TraineeEmploymentStatus ? 'Yes' : 'No'
        , "Trainee Employment Verification Status": item.TraineeEmploymentVerificationStatus ? 'Yes' : 'No'
      }
    })
  }
}
