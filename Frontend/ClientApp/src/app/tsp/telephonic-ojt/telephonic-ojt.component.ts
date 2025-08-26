import { Component, OnInit, ViewChild } from '@angular/core';
import { IEmpVarificationFilter } from '../Interface/IVarificationFilter';
import { CommonSrvService } from '../../common-srv.service';
import { DialogueService } from '../../shared/dialogue.service';
import { MatDialog } from '@angular/material/dialog';
import { EmpVerificationComponent } from '../employment-verification/employment-verification.component';
import { DEOEmploymentVerificationDialogComponent } from '../deo-employment-verification-dialog/deo-employment-verification-dialog.component';
import { ExportExcel } from '../../shared/Interfaces';
import { GroupByPipe } from 'angular-pipes';
import { DatePipe } from '@angular/common';
import { EnumExcelReportType } from '../../shared/Enumerations';
import { FormArray, FormControl, FormBuilder, FormGroup, FormGroupDirective, NgForm, Validators } from '@angular/forms';
import * as IMask from 'imask';
import { TelephonicEmploymentVerificationOJTBulkComponent } from '../telephonic-employment-verification-ojt-bulk-dialog/telephonic-employment-verification-ojt-bulk-dialog.component';
import { TelephonicEmploymentVerificationOJT } from '../telephonic-employment-verification-ojt-dialog/telephonic-employment-verification-ojt-dialog.component';

@Component({
  selector: 'app-telephonic-ojt',
  templateUrl: './telephonic-ojt.component.html',
  styleUrls: ['./telephonic-ojt.component.scss'],
  providers: [GroupByPipe, DatePipe]

})
export class TelephonicOJTComponent implements OnInit {

  specificTraineeListForVerification: any[];
  specificTraineeListForVerificationPSP: any[];
  placementTypes: any[];
  placementTypeID: any;
  verificationMethods: any[];
  verificationMethodsDrp: any[];
  verificationMethodID: any;
  ClassList: [];
  ClassListFiltered: [];
  TSPList: [];
  BatchList: [];
  BatchListForPSPVerification: [];
  TraineeList: any[] = [];
  TraineeListForExcel: [];
  TraineeListPSP: [];
  BatchTraineeList: [];
  ApprovalData: any;
  error: any;
  working: boolean;
  filters: IEmpVarificationFilter = { ClassID: 0, TSPID: 0, PlacementTypeID: 2, VerificationMethodID: 7 };
  pspfilters: IPSPTraineesFilter = { PSPBatchID: 0 };
  filtersPSPVerification: IPSPEmpVarificationFilter = { PSPBatchID: 0, PlacementTypeID: 2, VerificationMethodID: 7 };
  EmploymentTypeID: number;
  SearchTSPList = new FormControl('',);
  SearchCls = new FormControl('',);

  update: String;

  @ViewChild('tabGroup') tabGroup;

  constructor(private http: CommonSrvService, private dialogue: DialogueService, public dialog: MatDialog, private _date: DatePipe) { }

  ngOnInit(): void {
    this.http.setTitle("Call Center Employment Verification");
    //this.getVerificationDropdowns();
    this.GetTSPs();
    // this.GetClasses();
    //this.GetBatches();
    this.GetTelephonicEmploymentClassesList();
  }

  getSelectedTabData() {   // for demo
    switch (this.tabGroup.selectedIndex) {
      case 0:
        //  this.GetClasses();
        this.GetTelephonicEmploymentClassesList();
        break;
      case 1:
        this.GetBatches();
        break;
      case 2:
        this.GetBatchesForVerification();
        this.GetSelfEmploymentListPSP();
        break;
      default:
    }
  }


  getVerificationDropdowns() {
    this.http.getJSON(`api/TSPEmployment/EmployeeVerificationDropdownOJT`).subscribe(
      (d: any) => {
        this.placementTypes = d.placementTypes;
        this.verificationMethods = d.verificationMethods;
        let pId = this.placementTypes.length > 0 ? this.placementTypes[0].PlacementTypeID : 1;
        this.filters.PlacementTypeID = pId;
        this.verificationMethodsDrp = this.verificationMethods.filter(x => x.PlacementTypeID == pId);
        this.GetTelephonicEmploymentClassesList();
      }
      , (error) => {
        console.error(JSON.stringify(error));
      }
    );
  }

  placementTypeChange(e) {
    this.filters.VerificationMethodID = 0;
    this.verificationMethodsDrp = this.verificationMethods.filter(x => x.PlacementTypeID == e.value);
    this.GetTelephonicEmploymentClassesList();
  }

  GetTSPs() {
    this.http.getJSON(`api/TSPEmployment/GetTspForEmploymentVerificationOJT?placementTypeId=${this.filters.PlacementTypeID}&verificationMethodId=${this.filters.VerificationMethodID}`).subscribe(
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
    this.http.getJSON(`api/TSPEmployment/GetTelephonicEmploymentVerificationClassesOJT?pId=${this.filters.PlacementTypeID}&vmId=${this.filters.VerificationMethodID}&tspId=${this.filters.TSPID}`).subscribe(
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

  /*  GetClasses() {
     this.http.getJSON(`api/TSPEmployment/GetEmploymentVerificationByCallCentreClasses`).subscribe(
       (d: any) => {
         this.ClassList = d.ClassList;
         console.log(d);
       }
       , (error) => {
         console.error(JSON.stringify(error));
       }
     );
   } */

  GetBatches() {
    this.http.getJSON(`api/PSPEmployment/GetPSPBatches`).subscribe(
      (d: any) => {
        this.BatchList = d[0];
        console.log(d);
      }
      , (error) => {
        console.error(JSON.stringify(error));
      }
    );
  }

  GetBatchesForVerification() {
    this.http.getJSON(`api/PSPEmployment/GetPSPBatches`).subscribe(
      (d: any) => {
        this.BatchListForPSPVerification = d[0];
        console.log(d);
      }
      , (error) => {
        console.error(JSON.stringify(error));
      }
    );
  }


  toggleVerify(Row: any) {
    this.http.postJSON("api/EmploymentVerification/UpdateVerifyStatusOJT/", { PlacementID: Row.PlacementID, IsVerified: true }).subscribe(
      (d: any) => {
        //this.TraineeList = d.FormalList;
        this.GetTelephonicEmploymentClassesList();
        console.log(d);
      }
      , (error) => {
        console.error(error);
      }
    );
  };

  GetPSPBatchTraineeList() {
    this.http.getJSON(`api/PSPEmployment/GetPSPBatchTraineeList?pspBatchId=${this.pspfilters.PSPBatchID}`).subscribe(
      (d: any) => {
        this.BatchTraineeList = d;
      }
      , (error) => {
        console.error(error);
      }
    );
  }

  SubmitPSPemploymentList() {
    var list = this.BatchTraineeList;

    this.http.postJSON("api/PSPEmployment/SavePSPBatchTrainees", this.BatchTraineeList).subscribe(
      (d: any) => {
        //this.TraineeList = d.FormalList;
        //this.GetSelfEmploymentList();
        console.log(d);
        if (d) {
          this.update = "Trainee Status Updated Successfully";
          this.http.openSnackBar(this.update.toString(), "Updated");
        }
      }
      , (error) => {
        console.error(error);
      }
    );
  }

  GetTelephonicEmploymentClassesList() {
    this.http.getJSON(`api/TSPEmployment/GetTelephonicEmploymentVerificationClassesOJT?pId=${this.filters.PlacementTypeID}&vmId=${this.filters.VerificationMethodID}&tspId=${this.filters.TSPID}&cId=${this.filters.ClassID}`).subscribe(
      (d: any) => {
        this.ClassListFiltered = d.ClassList;
        //  this.EmploymentTypeID = d.TypeID;
      }
      , (error) => {
        console.error(error);
      }
    );
  }
  GetSelfEmploymentListPSP() {
    this.http.getJSON(`api/PSPEmployment/GetPSPSelfEmploymentList?pId=${this.filtersPSPVerification.PlacementTypeID}&vmId=${this.filtersPSPVerification.VerificationMethodID}&bId=${this.filtersPSPVerification.PSPBatchID}`).subscribe(
      (d: any) => {
        this.TraineeListPSP = d.SelfList;
        this.EmploymentTypeID = d.TypeID;
      }
      , (error) => {
        console.error(error);
      }
    );
  }

  mask = IMask.createMask({ mask: '0000 00000000' });


  GetTraineeOfClass(ClassID: any, r: any) {
    r.HasTrainees = !r.HasTrainees;
    if (!r.HasTrainees) {
      //this.specificTraineeListForVerification = this.allTraineeListForVerification.filter(i => i.ClassID != ClassID);
      return;
    }

    this.http.getJSON(`api/TSPEmployment/GetTelephonicTraineesForEmploymentVerificationOJT?pId=${this.filters.PlacementTypeID}&vmId=${this.filters.VerificationMethodID}&cId=${ClassID}`).subscribe(
      (d: any) => {
        r.TraineeList = d.PlacementData;
        //this.TraineeList.push(r.TraineeList);
        //this.TraineeList = this.TraineeList.reduce((accumulator, value) => accumulator.concat(value), []);

        r.HasTrainees = true;
        //this.allTraineeListForVerification.push(r.TraineeList)
      }
      , (error) => {
        console.error(error);
      }
    );

  };

  SubmitClassData(ClassID: any, ClassCode: string) {

    this.http.getJSON(`api/TSPEmployment/GetTelephonicTraineesForEmploymentVerificationOJT?pId=${this.filters.PlacementTypeID}&vmId=${this.filters.VerificationMethodID}&cId=${ClassID}`).subscribe(
      (d: any) => {
        this.TraineeList = d.PlacementData;

        let list = this.TraineeList.filter(x => x.IsVerified == null);

        let Dif = list.length;
        //this.http.confirm('Class Employment verification submit Confirmation', Dif > 0 ? Dif + ' Trainee data not saved Are you sure to submit?' : 'Are you sure to submit?').subscribe((res) => {
        //  if (res == true) {
        //    this.http.postJSON("api/EmploymentVerification/SubmitClassVerificationByCallCenter", { 'ClassID': ClassID }).subscribe((d: any) => {
        //      this.http.openSnackBar("Class Employment verification submited successfuly.");
        //    });
        //  }
        //});

        if (Dif > 0) {
          this.http.confirmTrinee('Class Employment submit Confirmation', Dif > 0 ? Dif + ' Trainee data not saved.' : 'Are you sure to submit?').subscribe((res) => {

          });
        }
        else {
          this.http.confirm('Class Employment submit Confirmation', Dif > 0 ? Dif + ' Trainee data not saved Are you sure to submit?' : 'Are you sure to submit?').subscribe((res) => {
            if (res === true) {
              this.http.postJSON('api/EmploymentVerification/SubmitClassVerificationByCallCenterOJT', { ClassID }).subscribe((d: any) => {
                this.http.openSnackBar('Class Employment submited successfuly.');
              });
            }
          });

        }

      });

    //let list = this.TraineeList.filter(x => x.IsVerified == null);

    //let Dif = list.length;
    ////this.http.confirm('Class Employment verification submit Confirmation', Dif > 0 ? Dif + ' Trainee data not saved Are you sure to submit?' : 'Are you sure to submit?').subscribe((res) => {
    ////  if (res == true) {
    ////    this.http.postJSON("api/EmploymentVerification/SubmitClassVerificationByCallCenter", { 'ClassID': ClassID }).subscribe((d: any) => {
    ////      this.http.openSnackBar("Class Employment verification submited successfuly.");
    ////    });
    ////  }
    ////});

    //if (Dif > 0) {
    //  this.http.confirmTrinee('Class Employment submit Confirmation', Dif > 0 ? Dif + ' Trainee data not saved.' : 'Are you sure to submit?').subscribe((res) => {

    //  });
    //}
    //else {
    //  this.http.confirm('Class Employment submit Confirmation', Dif > 0 ? Dif + ' Trainee data not saved Are you sure to submit?' : 'Are you sure to submit?').subscribe((res) => {
    //    if (res === true) {
    //      this.http.postJSON('api/TSPEmployment/SubmitClassEmployment', { ClassID }).subscribe((d: any) => {
    //        this.http.openSnackBar('Class Employment submited successfuly.');
    //      });
    //    }
    //  });

    //}

  };
  openDialog(item): void {
    const dialogRef = this.dialog.open(TelephonicEmploymentVerificationOJT, {
      minWidth: '1000px',
      minHeight: '600px',
      //data: JSON.parse(JSON.stringify(row))
      data: item
    })
    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.GetTelephonicEmploymentClassesList();
      }
    })
  }

  exportToExcelTraineesData() {
    this.http.getJSON(`api/TSPEmployment/TraineeForVerificationExportExcelOJT?pId=${this.filters.PlacementTypeID}&vmId=${this.filters.VerificationMethodID}&cId=${this.filters.ClassID}`).subscribe(
      (d: any) => {
        this.TraineeListForExcel = d.SelfList;
        this.EmploymentTypeID = d.TypeID;
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
      Title: 'Call Centre Employment Verification Report',
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
        "CC Decision": item.VerificationStatus,
        "CC Supervisor Response": item.CCSupervisorResponse,
        "CC Trainee Response": item.CCTraineeResponse,
        "Submission Date": this._date.transform(item.EmploymentSubmitedDate, 'MM/dd/yyyy'),
        //, "Comments": item.Comments,
        "Scheme": item.SchemeName,
        "TSP Name": item.TSPName
        //  , "TradeName": item.TradeName
        , "Class Code": item.ClassCode
        , "TraineeID": item.TraineeCode
        // , "Class Start Date": this._date.transform(item.StartDate, 'MM/dd/yyyy')
        , "Class End Date": this._date.transform(item.EndDate, 'MM/dd/yyyy')
        , "Trainee Name": item.TraineeName
        , "Trainee CNIC": item.TraineeCNIC
        , "Gender": item.GenderName
        , "Designation": item.Designation
        , "Department": item.Department
        , "NameOfEmployer": item.EmployerName
        , "Employment Address": item.EmploymentAddress
        //, "Employment Duration": item.EmploymentDuration
        , "Employment Timing": item.EmploymentTiming
        , "OfficeContactNo": item.OfficeContactNo
        , "Salary": item.Salary
        // , "EmploymentCommitment": item.EmploymentCommitment
        , "Employment Start Date": this._date.transform(item.EmploymentStartDate, 'MM/dd/yyyy')
        , "Trainee Contact Number": item.ContactNumber
        , "Supervisor Name": item.SupervisorName
        , "Employment Status": item.EmploymentStatus
        , "Employer Business Type": item.EmployerBusinessType
        , "EmploymentDistrict": item.DistrictName
        , "EmploymentTehsil": item.TehsilName

        //, "Permanent Address": item.PermanentAddress
        //, "Permanent District": item.PermanentDistrict
        //, "Permanent Tehsil": item.PermanentTehsil
      }
    })
  }

  getTSPemploymentList(item: any, TSPID: any) {
    debugger;
    //this.specificTraineeListForVerification = this.allTraineeListForVerification.filter(i => i.ClassID == item.value);
    this.http.getJSON(`api/TSPEmployment/GetTelephonicEmploymentListOJT?pId=${this.filters.PlacementTypeID}&vmId=${this.filters.VerificationMethodID}&cId=${item}&TSPID=${TSPID}`).subscribe(
      (d: any) => {
        this.specificTraineeListForVerification = d.SelfList;
        this.EmploymentTypeID = d.TypeID;
        if (this.specificTraineeListForVerification.length == 0) {
          this.http.ShowError("No record found");
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
    this.http.getJSON(`api/PSPEmployment/GetPSPSelfEmploymentList?pId=${this.filters.PlacementTypeID}&vmId=${this.filters.VerificationMethodID}&bId=${item}`).subscribe(
      (d: any) => {
        this.specificTraineeListForVerificationPSP = d.SelfList;
        this.EmploymentTypeID = d.TypeID;
        if (this.specificTraineeListForVerificationPSP.length == 0) {
          this.http.ShowError("No record found");
          return;
        }
        this.openVerificationDialog(this.specificTraineeListForVerificationPSP)
        console.log(d);
      }
      , (error) => {
        console.error(error);
      }
    );
  }

  openVerificationDialog(item: any): void {
    debugger;
    const dialogRef = this.dialog.open(TelephonicEmploymentVerificationOJTBulkComponent, {
      minWidth: '1000px',
      minHeight: '600px',
      //data: JSON.parse(JSON.stringify(row))
      data: item

    })
    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.GetTelephonicEmploymentClassesList();
      }
    })
  }
  openClassJourneyDialogue(data: any): void {
    debugger;
    this.dialogue.openClassJourneyDialogue(data);
  }
  openTraineeJourneyDialogue(data: any): void {
    debugger;
    this.dialogue.openTraineeJourneyDialogue(data);
  }
  //selectAll(e: any) { //.checked
  //  this.TraineeList.filter(x => x.FileType == 'Self').map(function (x) {
  //    x.IsTSP = e.checked;
  //    return x
  //  });
  //};



}


export interface IPSPEmpVarificationFilter {
  PSPBatchID: number,
  PlacementTypeID: number,
  VerificationMethodID: number
}


export interface IPSPTraineesFilter {
  PSPBatchID: number
}
