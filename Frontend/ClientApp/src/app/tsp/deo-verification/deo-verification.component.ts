import { Component, OnInit, ViewChild } from '@angular/core';
import { IEmpVarificationFilter } from '../Interface/IVarificationFilter';
import { EmpVerificationComponent } from '../employment-verification/employment-verification.component';
import { CommonSrvService } from '../../common-srv.service';
import { DialogueService } from '../../shared/dialogue.service';
import { MatDialog } from '@angular/material/dialog';
import * as IMask from 'imask';
import { fadeAnimation, enterAnimation } from 'src/app/animations';
import { FormArray, FormControl, FormBuilder, FormGroup, FormGroupDirective, NgForm, Validators } from '@angular/forms';
import { DEOEmploymentVerificationDialogComponent } from '../deo-employment-verification-dialog/deo-employment-verification-dialog.component';
import { ExportExcel } from '../../shared/Interfaces';
import { GroupByPipe } from 'angular-pipes';
import { DatePipe } from '@angular/common';
import { EnumExcelReportType } from '../../shared/Enumerations';
import { SelectionModel } from '@angular/cdk/collections';


@Component({
  selector: 'app-deo-verification',
  templateUrl: './deo-verification.component.html',
  styleUrls: ['./deo-verification.component.scss'],
  animations: [enterAnimation],
  providers: [GroupByPipe, DatePipe]

})
export class DeoVerificationComponent implements OnInit {
  EmploymentVerificationForm: FormGroup;
  notForm: FormGroup;

  placementTypes: any[];
  employmentRecordToVerify: any[];
  placementTypeID: any;
  verificationMethods: any[];
  verificationMethodsDrp: any[];
  placementTypesPSP: any[];
  verificationMethodsPSP: any[];
  verificationMethodsDrpPSP: any[];
  verificationMethodID: any;
  ClassList: any[];
  TSPList: any[];
  ClassListFiltered: any[];
  PSPBatches: any[];
  PSPBatchesFiltered: any[];
  PSPTraineeList: [];
  TraineeList: any[] = [];
  TraineeListForExcel: any[];
  DeoDashboardStats: any[];
  allTraineeListForVerification: any[];
  specificTraineeListForVerification: any[];
  ApprovalData: any;
  error: any;
  working: boolean;
  filters: IEmpVarificationFilter = { ClassID: 0, TSPID: 0, PlacementTypeID: 0, VerificationMethodID: 0 };
  filtersPSP: IPSPEmploymentFilters = { PSPBatchID: 0, PlacementTypeID: 0, VerificationMethodID: 0 };
  EmploymentTypeID: number;
  PendingEmploymentVerifications: number;
  PendingCNICVerifications: number;
  forwardedList: number[] = [];
  employmentVerificationFormGroup = {};
  SearchCls = new FormControl('',);
  SearchTSPList = new FormControl('',);
  SearchBatch = new FormControl('',);
  selection = new SelectionModel<any>(true, []);

  @ViewChild('tabGroup') tabGroup;

  constructor(private fb: FormBuilder, private dialogue: DialogueService, public dialog: MatDialog, private comSrv: CommonSrvService, private _date: DatePipe) { }

  ngOnInit(): void {
    this.comSrv.setTitle("DEO Employment Verification");
    this.getDeoDashboardStats();
    this.getVerificationDropdowns();
    this.filters.PlacementTypeID = 1;
    this.GetTSPs();
    this.GetClassesForDEOVerification();
    //this.GetClasses();
    this.initForm();

  }
  EmptyCtrl() {
    this.SearchCls.setValue('');
    this.SearchBatch.setValue('');
  }

  getDeoDashboardStats() {
    this.comSrv.getJSON(`api/TSPEmployment/GetDeoDashboardStats`).subscribe(
      (d: any) => {
        console.log(d);
        this.DeoDashboardStats = d
        this.PendingEmploymentVerifications = d.PendingEmploymentVerifications;
        this.PendingCNICVerifications = d.PendingCNICVerifications;
      }
      , (error) => {
        console.error(JSON.stringify(error));
      }
    );
  }

  initForm() {
    this.EmploymentVerificationForm = this.fb.group({
      employmentTrainees: new FormArray([])
    });
    this.employmentVerificationFormGroup = {
      PlacementID: ["", Validators.required],
      TraineeName: "",
      IsVerified: [false, Validators.required],
      Comments: ["", Validators.required],

      Designation: ['', [Validators.required]],
      Department: ['', [Validators.required]],
      EmploymentDuration: ['', [Validators.required]],
      Salary: ['', [Validators.required]],
      SupervisorName: ['', [Validators.required]],
      SupervisorContact: ['', [Validators.required]],
      EmploymentStartDate: ['', [Validators.required]],
      EmploymentStatus: ['', [Validators.required]],

      EmployerName: ['', [Validators.required]],
      EmployerBusinessType: ['', [Validators.required]],
      EmploymentAddress: ['', [Validators.required]],
      District: ['', [Validators.required]],
      EmploymentTehsil: ['', [Validators.required]],
      TimeFrom: [''],
      TimeTo: ['', [Validators.required]],
      EmploymentTiming: ['', [Validators.required]],
      Attachment: '',
      OfficeContactNo: ['', [Validators.required]],
    }, { updateOn: "blur" };
    this.notForm = this.fb.group({ ...this.employmentVerificationFormGroup, traineesforEmployment: [0, Validators.required] });

  }

  getSelectedTabData() {   // for demo
    switch (this.tabGroup.selectedIndex) {
      case 0:
        this.GetTSPs();
        this.GetClasses();
        break;
      case 1:
        this.getVerificationDropdownsPSP();
        this.GetPSPBatchesList();
        break;
      default:
    }
  }


  getVerificationDropdowns() {
    this.comSrv.getJSON(`api/TSPEmployment/EmployeeVerificationDropdown`).subscribe(
      (d: any) => {
        console.log(d);
        this.placementTypes = d.placementTypes;
        this.verificationMethods = d.verificationMethods;
        let pId = this.placementTypes.length > 0 ? this.placementTypes[0].PlacementTypeID : 1;
        this.filters.PlacementTypeID = pId;
        this.verificationMethodsDrp = this.verificationMethods.filter(x => x.PlacementTypeID == pId && x.VerificationMethodID != 7);
        //this.GetSelfEmploymentList();
      }
      , (error) => {
        console.error(JSON.stringify(error));
      }
    );
  }

  placementTypeChange(e) {
    this.filters.PlacementTypeID = e.value;
    this.filters.VerificationMethodID = 0;
    if (this.filters.PlacementTypeID == 2) {
      this.filters.VerificationMethodID = 6;
    }

    this.verificationMethodsDrp = this.verificationMethods.filter(x => x.PlacementTypeID == e.value && x.VerificationMethodID != 7);
    //this.GetSelfEmploymentList();
    this.GetTSPs();
    this.GetClassesForDEOVerification();
  }

  verficationMethodChange(e) {
    console.log(e.value);
    this.filters.VerificationMethodID = e.value;
    //this.GetSelfEmploymentList();
    this.GetClassesForDEOVerification();
  }

  //classChange(e) {
  //  console.log(e.value);
  //  this.filters.ClassID = e.value;
  //  if (this.filters.ClassID == 0) {
  //    this.ClassListFiltered = this.ClassList;
  //  }
  //  else {
  //    this.ClassListFiltered = this.ClassList.filter(x => x.ClassID == this.filters.ClassID);
  //  }
  //  //this.GetSelfEmploymentList();
  //  this.GetClassesForDEOVerification();

  //}

  getVerificationDropdownsPSP() {
    this.comSrv.getJSON(`api/TSPEmployment/EmployeeVerificationDropdown`).subscribe(
      (d: any) => {
        console.log(d);
        this.placementTypesPSP = d.placementTypes;
        this.verificationMethodsPSP = d.verificationMethods;
        let pIdPSP = this.placementTypes.length > 0 ? this.placementTypes[0].PlacementTypeID : 1;
        this.filtersPSP.PlacementTypeID = pIdPSP;
        this.verificationMethodsDrpPSP = this.verificationMethodsPSP.filter(x => x.PlacementTypeID == pIdPSP && x.VerificationMethodID != 7);
        this.GetPSPSelfEmploymentList();
      }
      , (error) => {
        console.error(JSON.stringify(error));
      }
    );
  }

  placementTypeChangePSP(e) {
    this.filtersPSP.PlacementTypeID = e.value;
    this.filtersPSP.VerificationMethodID = 0;
    if (this.filtersPSP.PlacementTypeID == 2) {
      this.filtersPSP.VerificationMethodID = 6;
    }

    this.verificationMethodsDrpPSP = this.verificationMethodsPSP.filter(x => x.PlacementTypeID == e.value && x.VerificationMethodID != 7);
    this.GetPSPSelfEmploymentList();
  }

  verficationMethodChangePSP(e) {
    console.log(e.value);
    this.filtersPSP.VerificationMethodID = e.value;
    this.GetPSPSelfEmploymentList();
  }

  pspBatchChange(e) {
    console.log(e.value);
    this.filtersPSP.PSPBatchID = e.value;
    if (this.filtersPSP.PSPBatchID == 0) {
      this.PSPBatchesFiltered = this.PSPBatches;
    }
    else {
      this.PSPBatchesFiltered = this.PSPBatches.filter(x => x.PSPBatchID == this.filtersPSP.PSPBatchID);

    }
    this.GetPSPSelfEmploymentList();
  }

  GetTSPs() {
    this.comSrv.getJSON(`api/TSPEmployment/GetTspForEmploymentVerification?placementTypeId=${this.filters.PlacementTypeID}&verificationMethodId=${this.filters.VerificationMethodID}`).subscribe(
      (d: any) => {
        this.TSPList = d.TSPsList;
        //this.ClassListFiltered = d.ClassList;
        console.log(d);
      }
      , (error) => {
        console.error(JSON.stringify(error));
      }
    );
  }

  GetEmploymentClassesByTSP() {
    this.comSrv.getJSON(`api/TSPEmployment/GetEmploymentVerificationClasses?pId=${this.filters.PlacementTypeID}&vmId=${this.filters.VerificationMethodID}&tspId=${this.filters.TSPID}`).subscribe(
      (d: any) => {
        if (this.filters.ClassID == 0) {
          this.ClassList = d.ClassList;
        }
        //this.ClassListFiltered = d.ClassList;
        //console.log(d);
      }
      , (error) => {
        console.error(JSON.stringify(error));
      }
    );
  }

  GetClasses() {
    this.comSrv.getJSON(`api/TSPEmployment/GetTspClassese`).subscribe(
      (d: any) => {
        this.ClassList = d.ClassList;
        this.ClassListFiltered = d.ClassList;
        console.log(d);
      }
      , (error) => {
        console.error(JSON.stringify(error));
      }
    );
  }

  GetClassesForDEOVerification() {
    this.comSrv.getJSON(`api/TSPEmployment/GetEmploymentVerificationClasses?pId=${this.filters.PlacementTypeID}&vmId=${this.filters.VerificationMethodID}&tspId=${this.filters.TSPID}&cId=${this.filters.ClassID}`).subscribe(
      (d: any) => {
        if (this.filters.ClassID == 0) {
          this.ClassList = d.ClassList;
        }
        this.ClassListFiltered = d.ClassList;
        console.log(d);
      }
      , (error) => {
        console.error(JSON.stringify(error));
      }
    );
  }

  GetPSPTrainees() {
    this.comSrv.postJSON(`api/PSPEmployment/GetPSPTraineeForDEOVerification`, this.filtersPSP).subscribe(
      (d: any) => {
        this.PSPTraineeList = d;
        console.log(d);
      }
      , (error) => {
        console.error(JSON.stringify(error));
      }
    );
  }

  GetPSPBatchesList() {
    this.comSrv.getJSON(`api/PSPEmployment/GetPSPBatches`).subscribe(
      (d: any) => {
        this.PSPBatches = d[0];
        this.PSPBatchesFiltered = d[0];
        console.log(d);
      }
      , (error) => {
        console.error(JSON.stringify(error));
      }
    );
  }


  toggleVerify(Row: any) {
    this.comSrv.postJSON("api/EmploymentVerification/UpdateVerifyStatus/", { PlacementID: Row.PlacementID, IsVerified: true }).subscribe(
      (d: any) => {
        //this.TraineeList = d.FormalList;
        this.GetSelfEmploymentList();
        console.log(d);
      }
      , (error) => {
        console.error(error);
      }
    );
  };
  GetSelfEmploymentList() {

    this.comSrv.getJSON(`api/TSPEmployment/GetSelfEmploymentList?pId=${this.filters.PlacementTypeID}&vmId=${this.filters.VerificationMethodID}&cId=${this.filters.ClassID}`).subscribe(
      (d: any) => {
        this.TraineeList = d.SelfList;
        this.EmploymentTypeID = d.TypeID;
        console.log(d);
      }
      , (error) => {
        console.error(error);
      }
    );
  }
  GetPSPSelfEmploymentList() {

    this.comSrv.getJSON(`api/PSPEmployment/GetPSPSelfEmploymentList?pId=${this.filtersPSP.PlacementTypeID}&vmId=${this.filtersPSP.VerificationMethodID}&bId=${this.filtersPSP.PSPBatchID}`).subscribe(
      (d: any) => {
        this.PSPTraineeList = d.SelfList;
        this.EmploymentTypeID = d.TypeID;
        console.log(d);
      }
      , (error) => {
        console.error(error);
      }
    );
  }
  openDialog(item: any): void {
    this.comSrv.getJSON(`api/TSPEmployment/GetTraineeForEmploymentVerification?traineeId=${item.TraineeID}`).subscribe(
      (d: any) => {
        this.employmentRecordToVerify = d[0];
        this.pushDataToDialog();
      }
      , (error) => {
        console.error(error);
      }
    );
  }

  pushDataToDialog() {

    const dialogRef = this.dialog.open(EmpVerificationComponent, {
      minWidth: '1000px',
      minHeight: '600px',
      //data: JSON.parse(JSON.stringify(row))
      //data: { 'TraineeRow': item, 'ClassRow': row },
      data: this.employmentRecordToVerify,
      //{
      //  "PlacementID": item.PlacementID,
      //  "TraineeName": item.TraineeName,
      //  "IsVerified": item.IsVerified,
      //  "Comments": item.Comments,
      //  "FileName": item.FileName,
      //  "FilePath": item.FilePath,
      //  "FileType": item.FileType,
      //  "SupervisorName": item.SupervisorName,
      //  "SupervisorContact": item.SupervisorContact,
      //  "OfficeContactNo": item.OfficeContactNo,
      //  "VerificationMethodType": item.VerificationMethodType,
      //  "EmploymentTypeID": item.EmploymentTypeID
      //}
      //this.GetVisitPlanData(data)
    })
    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        debugger;
        var newList = this.ClassListFiltered.map(header => {
          if (header.ClassID == result.ClassID) {
            let list = header.TraineeList.map((item: any) => {
              if (item.PlacementID == result.PlacementID) {

                return {
                  ...item
                  , IsVerified: result.IsVerified
                  , PhysicalVerificationStatus: result.IsVerified
                  , Comments: result.Comments
                }
              }
              else {
                return item;
              }
            });
            header.TraineeList = list
            return header;
          } else {
            return header;
          }

        })


      }
    })
  }

  getTSPemploymentList(item: any, TSPID: any) {

    //this.specificTraineeListForVerification = this.allTraineeListForVerification.filter(i => i.ClassID == item.value);
    this.comSrv.getJSON(`api/TSPEmployment/GetSelfEmploymentList?pId=${this.filters.PlacementTypeID}&vmId=${this.filters.VerificationMethodID}&cId=${item}&TSPID=${TSPID}`).subscribe(
      (d: any) => {
        this.specificTraineeListForVerification = d.SelfList;
        this.EmploymentTypeID = d.TypeID;
        if (this.specificTraineeListForVerification.length == 0) {
          this.comSrv.ShowError("No record found");
          return;
        }
        this.openVerificationDialog(this.specificTraineeListForVerification)
        console.log(d);
      }
      , (error) => {
        console.error(error);
      }
    );
  }

  getPSPemploymentList(item: any) {

    //this.specificTraineeListForVerification = this.allTraineeListForVerification.filter(i => i.ClassID == item.value);
    this.comSrv.getJSON(`api/PSPEmployment/GetPSPSelfEmploymentList?pId=${this.filtersPSP.PlacementTypeID}&vmId=${this.filtersPSP.VerificationMethodID}&bId=${item}`).subscribe(
      (d: any) => {
        this.specificTraineeListForVerification = d.SelfList;
        this.EmploymentTypeID = d.TypeID;
        if (this.specificTraineeListForVerification.length == 0) {
          this.comSrv.ShowError("No record found");
          return;
        }
        this.openVerificationDialog(this.specificTraineeListForVerification)
        console.log(d);
      }
      , (error) => {
        console.error(error);
      }
    );
  }

  exportToExcelTraineesData() {
    this.comSrv.getJSON(`api/TSPEmployment/TraineeForVerificationExportExcel?pId=${this.filters.PlacementTypeID}&vmId=${this.filters.VerificationMethodID}&tspId=${this.filters.TSPID}&cId=${this.filters.ClassID}`).subscribe(
      (d: any) => {
        this.TraineeListForExcel = d.SelfList;
        //this.EmploymentTypeID = d.TypeID;
        console.log(d);
        this.exportToExcel();
      }
      , (error) => {
        console.error(error);
      }
    );
  }

  exportToExcel() {
    let filteredData = [...this.TraineeListForExcel]
    let data = {
    };

    let exportExcel: ExportExcel = {
      Title: 'DEO Employment Verification Report',
      Author: '',
      Type: EnumExcelReportType.UnVerifiedTraineesChangeRequestApproval,
      Data: data,
      List1: this.populateData(filteredData),
    };
    this.dialogue.openExportConfirmDialogue(exportExcel).subscribe();
  }


  populateData(data: any) {
    return data.map(item => {
      return {
        //"Scheme": item.SchemeName
        "ME Verification": item.VerificationStatus
        , "Comments": item.Comments
        , "TSP Name": item.TSPName
        , "Class Code": item.ClassCode
        , "PlacementType": item.PlacementType
        , "Verification Source": item.VerificationMethodType
        , "Class Start Date": this._date.transform(item.StartDate, 'MM/dd/yyyy')
        , "Class End Date": this._date.transform(item.EndDate, 'MM/dd/yyyy')
        , "TraineeID": item.TraineeCode
        , "Trade Name": item.TradeName
        , "Trainee CNIC": item.TraineeCNIC
        , "Trainee Contact Number": item.ContactNumber
        , "Designation": item.Designation
        , "Department": item.Department
        , "Employer Name": item.EmployerName
        , "Employer Business Type": item.EmployerBusinessType
        , "Employment Address": item.EmploymentAddress
        , "Supervisor Name": item.SupervisorName
        , "SupervisorContactNumber": item.SupervisorContact
        //, "Permanent Address": item.PermanentAddress
        //, "Permanent District": item.PermanentDistrict
        //, "Permanent Tehsil": item.PermanentTehsil
      }
    })
  }

  openVerificationDialog(item: any): void {

    const dialogRef = this.dialog.open(DEOEmploymentVerificationDialogComponent, {
      minWidth: '1000px',
      minHeight: '600px',
      //data: JSON.parse(JSON.stringify(row))
      data: item

    })
    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.GetSelfEmploymentList();
        //this.GetTraineeOfClass(item.ClassID, item);
      }
    })
  }
  selectAll(e: any) { //.checked
    //this.TraineeList.filter(x => x.FileType == 'Self').map(function (x) {
    this.TraineeList.filter(x => x.FileType == 'Self').map(function (x) {
      x.IsChecked = e.checked;
      return x
    });
  };

  isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.ClassListFiltered.length;
    return numSelected === numRows;
  }

  masterToggle() {
    this.isAllSelected() ?
      this.selection.clear() :
      this.ClassListFiltered.forEach(row => this.selection.select(row));
  }

  GetTraineeOfClass(ClassID: any, r: any) {
    r.HasTrainees = !r.HasTrainees;
    if (!r.HasTrainees) {
      //this.specificTraineeListForVerification = this.allTraineeListForVerification.filter(i => i.ClassID != ClassID);
      return;
    }

    this.comSrv.getJSON(`api/TSPEmployment/GetTraineesForEmploymentVerification?pId=${this.filters.PlacementTypeID}&vmId=${this.filters.VerificationMethodID}&cId=${ClassID}`).subscribe(
      (d: any) => {
        debugger;
        r.TraineeList = d.PlacementData;
        this.TraineeList.push(r.TraineeList);
        this.TraineeList = this.TraineeList.reduce((accumulator, value) => accumulator.concat(value), []);

        r.HasTrainees = true;
        //this.allTraineeListForVerification.push(r.TraineeList)
      }
      , (error) => {
        console.error(error);
      }
    );

  };


  GetTraineeOfBatch(PSPBatchID: any, r: any) {
    r.HasTrainees = !r.HasTrainees;
    //this.specificTraineeListForVerification = this.allTraineeListForVerification.filter(i => i.ClassID != ClassID);
    return;
  };



  fillFormValues(data: any) {
    this.EmploymentVerificationForm.patchValue({
      PlacementID: data.PlacementID,
      TraineeName: data.TraineeName,
      IsVerified: data.IsVerified,
      Comments: data.Comments,
      OfficeContactNo: data.OfficeContactNo,
      Designation: data.Designation,
      Department: data.Department,

      SupervisorName: data.SupervisorName,
      SupervisorContact: data.SupervisorContact,
      EmploymentStartDate: data.EmploymentStartDate,
      EmploymentStatus: data.EmploymentStatus,

      EmployerName: data.EmployerName,
      EmployerBusinessType: data.EmployerBusinessType,
      EmploymentAddress: data.EmploymentAddress,
      District: data.District,
      EmploymentTehsil: data.EmploymentTehsil,
      EmploymentTiming: data.EmploymentTiming
    })
  }

  updateVisiting(event: any, id: number, comment: string) {
    if (event) {
      //this.comSrv.postJSON('api/VisitPlan/UpdateUserEventStatus', { 'VisitPlanID': id, "UserStatus": event, "UserID": this.data.UserID })
      //  .subscribe((d: any) => {


      //    //this.success = "Your Visiting Status has been saved";
      //    //this.comSrv.openSnackBar(this.success.toString(), "Success");
      //    //this.ComSrv.openSnackBar(row.InActive == true ? environment.InActiveMSG.replace("${Name}", this.EnText) : environment.ActiveMSG.replace("${Name}", this.EnText));
      //    // this.visitplan =new MatTableDataSource(d);
      //  },
      //    error => this.error = error // error path
      //  );
    }
    console.log("YEs")
  }

  openClassJourneyDialogue(data: any): void {
    debugger;
    this.dialogue.openClassJourneyDialogue(data);
  }

  SubmitClassData(ClassID: any, ClassCode: string) {

    let list = this.TraineeList.filter(x => x.IsVerified == null);

    let Dif = list.length;
    this.comSrv.confirm('Class Employment verification submit Confirmation', Dif > 0 ? Dif + ' Trainee data not saved Are you sure to submit?' : 'Are you sure to submit?').subscribe((res) => {
      if (res == true) {
        this.comSrv.postJSON("api/EmploymentVerification/SubmitClassVerification", { 'ClassID': ClassID }).subscribe((d: any) => {
          this.comSrv.openSnackBar("Class Employment verification submited successfuly.");
        });
      }
    });

  };
  mask = IMask.createMask({ mask: '0000 00000000' });
  forwardToTelephonic() {

    this.comSrv.confirm("Are you sure?", "You want to forward to Telephonic verification").subscribe(result => {
      if (!result) {
        return;
      }
      //let list = this.TraineeList.filter(x => x.PlacementTypeID == 'Self').map(function (x) { return x.PlacementID; });
      //let list = this.TraineeList.map(function (x) { return x.PlacementID; });
      let list = this.selection.selected;
      if (!list || list.length <= 0) {
        this.comSrv.ShowError("These is nothing to move");
        return;
      }
      var ids = list.map(x => x.ClassID);
      this.filters.PlacementTypeID;
      this.filters.VerificationMethodID;
      this.comSrv.postJSON('api/TSPEmployment/ForwardedToTelephonic', { ClassIDslist: ids, PlacementTypeID: this.filters.PlacementTypeID, VerificationMethodID: this.filters.VerificationMethodID })
        .subscribe((d: any) => {
          if (d) {
            this.comSrv.openSnackBar("data is moved");
            //this.ClassListFiltered = this.ClassList;
            //var newList = this.ClassListFiltered.map(header => {
            //  if (header.ClassID == list[0].ClassID) {
            //    let list = header.TraineeList.map((item: any) => {
            //      if (item.PlacementID == result.PlacementID) {
            //        return {
            //          ...item
            //          , IsVerified: result.IsVerified
            //          , Comments: result.Comments
            //        }
            //      }
            //      else {
            //        return item;
            //      }
            //    });
            //    header.TraineeList = list
            //    return header;
            //  } else {
            //    return header;
            //  }

            //})


            //this.GetSelfEmploymentList();
          } else {
            this.comSrv.ShowError("Some error has been occured.");
          }
        },
          (error) => {

            this.error = error.error;
            this.comSrv.ShowError(this.error.toString(), "Error");
            this.working = false;
          });



    });
  }

  //get


  get ID() { return this.EmploymentVerificationForm.get("ID"); }
  get TraineeName() { return this.EmploymentVerificationForm.get("TraineeName"); }
  get PlacementID() { return this.EmploymentVerificationForm.get("PlacementID"); }
  get PayrollVerification() { return this.EmploymentVerificationForm.get("PayrollVerification"); }
  get PayrollVerificationStatus() { return this.EmploymentVerificationForm.get("PayrollVerificationStatus"); }
  get PhysicalVerification() { return this.EmploymentVerificationForm.get("PhysicalVerification"); }
  get TelephonicVerification() { return this.EmploymentVerificationForm.get("TelephonicVerification"); }
  get PhysicalVerificationStatus() { return this.EmploymentVerificationForm.get("PhysicalVerificationStatus"); }
  get TelephonicVerificationStatus() { return this.EmploymentVerificationForm.get("TelephonicVerificationStatus"); }
  get VerificationMethodID() { return this.EmploymentVerificationForm.get("VerificationMethodID"); }
  get IsVerified() { return this.EmploymentVerificationForm.get("IsVerified"); }
  get Comments() { return this.EmploymentVerificationForm.get("Comments"); }
  get Attachment() { return this.EmploymentVerificationForm.get("Attachment"); }
  get InActive() { return this.EmploymentVerificationForm.get("InActive"); }
  get OfficeContactNo() { return this.EmploymentVerificationForm.get("OfficeContactNo"); }

  get Designation() { return this.EmploymentVerificationForm.get("Designation"); }
  get Department() { return this.EmploymentVerificationForm.get("Department"); }
  get EmploymentDuration() { return this.EmploymentVerificationForm.get("EmploymentDuration"); }
  get Salary() { return this.EmploymentVerificationForm.get("Salary"); }
  get SupervisorName() { return this.EmploymentVerificationForm.get("SupervisorName"); }
  get SupervisorContact() { return this.EmploymentVerificationForm.get("SupervisorContact"); }
  get EmploymentStartDate() { return this.EmploymentVerificationForm.get("EmploymentStartDate"); }
  get EmploymentStatus() { return this.EmploymentVerificationForm.get("EmploymentStatus"); }
  get EmploymentType() { return this.EmploymentVerificationForm.get("EmploymentType"); }
  get EmployerName() { return this.EmploymentVerificationForm.get("EmployerName"); }
  get EmployerBusinessType() { return this.EmploymentVerificationForm.get("EmployerBusinessType"); }
  get EmploymentAddress() { return this.EmploymentVerificationForm.get("EmploymentAddress"); }
  get District() { return this.EmploymentVerificationForm.get("District"); }
  get EmploymentTehsil() { return this.EmploymentVerificationForm.get("EmploymentTehsil"); }

  get employmentTrainees() { return this.EmploymentVerificationForm.get("employmentTrainees") as FormArray; }


}


export interface IPSPEmploymentFilters {
  PlacementTypeID: number,
  VerificationMethodID: number,
  PSPBatchID: number
}
