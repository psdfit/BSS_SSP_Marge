/// Develop by Rao Ali Haider 20-Nov-2023
import { Component, OnInit, ViewChild } from '@angular/core';
import { FormGroup, FormBuilder, FormControl, Validators } from '@angular/forms';
import { CommonSrvService } from '../../common-srv.service';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { UserRightsModel } from '../../master-data/users/users.component';
import { DialogueService } from '../../shared/dialogue.service';
import { EnumApprovalStatus, EnumApprovalProcess, EnumProgramCategory } from '../../shared/Enumerations';
import { environment } from '../../../environments/environment';
import { SelectionModel } from '@angular/cdk/collections';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { UsersModel } from '../../master-data/users/users.component';
import { forkJoin } from 'rxjs';
import { map, switchMap } from 'rxjs/operators';
import { isNull } from 'angular-pipes/utils/utils';
import { DateFormat } from '../../shared/shared.module';
@Component({
  selector: 'app-approval-dialogue-batch',
  templateUrl: './approval-dialogue-batch.component.html',
  styleUrls: ['./approval-dialogue-batch.component.scss']
})
export class ApprovalDialogueBatchComponent implements OnInit {
  environment = environment;
  approvalHistory: IApprovalHistory[] = [];
  approvalForm: FormGroup;
  // canSendBack: boolean = false;
  isValidApprover = false;
  isAlreadyApproved = false;
  RejectedButtonHideBit = true;
  latest: IApprovalHistory;
  TradeTargetValid: boolean = true;
  currentUserDetails: UsersModel;
  enumApprovalStatus = EnumApprovalStatus;
  alert: { Type: string, Title?: string, Message?: string; } = null;
  currentDate: Date;

  displayedColumnsClass = ['ClassCode',
    'TradeName', 'SourceOfCurriculumName', 'EntryQualification', 'CertAuthName',
    'StartDate', 'EndDate'];
  SearchSch = new FormControl('',);
  SearchStatus = new FormControl('',);
  SearchBatch = new FormControl('',);
  classes: ClassType[] = [];
  classesList = [];
  classesBatchList = [];
  ClassRecommendation = [];
  currentClassDates: [];
  selectAll: boolean = false;
  data: IApprovalHistory;
  Scheme = [];
  BatchNos = [];

  schemeFilter = new FormControl(0);
  searchFilter = new FormControl(0);
  BatchNoFilter = new FormControl(0);
  ActiveFormApprovalID: number;
  ChosenTradeID: number;
  title: string;
  savebtn: string;
  formrights: UserRightsModel;
  EnText: string = "";
  error: String;
  query = {
    order: 'ClassDatesChangeRequestID',
    limit: 5,
    page: 1
  };
  working: boolean;
  selection = new SelectionModel<any>(true, []);
  constructor(private http: CommonSrvService, private dialogue: DialogueService, public dialogRef: MatDialogRef<ApprovalDialogueBatchComponent>) {
    //this.schemes = new MatTableDataSource([]);
    this.formrights = http.getFormRights();
    this.approvalForm = new FormGroup({
      Remarks: new FormControl('', [Validators.required]),
      BatchNo: new FormControl('', [Validators.required]),
    });
    this.currentDate = new Date();

  }

  ngOnInit(): void {
    this.http.setTitle("Class Dates Change Request");
    this.title = "";
    this.savebtn = "Approve";
    this.GetSchemes();
    this.GetClassDatesCRs();
    //this.getApprovalHistory();
    this.currentUserDetails = this.http.getUserDetails();
  }

  GetClassDatesCRs() {
    this.http.getJSON('api/ClassChangeRequest/GetClassDatesChangeRequest/' + this.schemeFilter.value + '/' + this.searchFilter.value + '/' + this.BatchNoFilter.value).subscribe((d: any) => {
      this.classesList = d[0];
      this.classesBatchList = d[1];
      this.ClassRecommendation = d[2];
      if (this.classesList.length > 0) {
        this.getApprovalHistory(this.classesList[0].ClassDatesChangeRequestID);
      }
      //const distinctBatchNos = [...new Set(this.classesList.map(item => item.BatchNo))];
      //const columnName = 'BatchNo';
      const uniqueBatchNos = [];
      const seenBatchNos = new Set();

      if (this.classesBatchList.length > 0) {
        if (this.BatchNos.length == 0) {
          for (const item of this.classesBatchList) {
            const trimmedBatchNo = item.BatchNo.trim(); // Trim to handle whitespace
            if (trimmedBatchNo !== '' && !seenBatchNos.has(trimmedBatchNo)) {
              seenBatchNos.add(trimmedBatchNo);
              uniqueBatchNos.push({
                BatchNo: trimmedBatchNo,
                DisplayBatchNo: trimmedBatchNo, // Add the property you want here
              });
            }
          }
          this.BatchNos = uniqueBatchNos;


        }
        const batchNoValue = this.BatchNoFilter.value === 0 ? null : this.BatchNoFilter.value;
        const batchNoFormControl = this.approvalForm.get('BatchNo');
        batchNoFormControl.setValue(batchNoValue);
        const randomNumber = Math.floor(Math.random() * 100000).toString().padStart(5, '0');
        const rendomnumber1 = Math.floor(Math.random() * 100000).toString().padStart(3, '0');
        if (batchNoValue == 0 || batchNoValue == null) {
          batchNoFormControl.setValue('Batch-' + randomNumber + '-B-' + rendomnumber1);
          batchNoFormControl.disable();
        }
        // Disable the BatchNo field if the value is not null
        if (batchNoValue !== null) {
          batchNoFormControl.disable();
        } else {
          batchNoFormControl.setValue('Batch-' + randomNumber + '-B-' + rendomnumber1);
          batchNoFormControl.disable();
        }
      }
      //this.tsps.paginator = this.paginator;
      //this.tsps.sort = this.sort;
    },
      error => this.error = error // error path
      , () => {
        this.working = false;
      });
  }
  GetSchemes() {
    this.http.getJSON('api/ClassChangeRequest/GetClassScheme').subscribe((d: any) => {
      this.Scheme = d[0];
      //this.tsps.paginator = this.paginator;
      //this.tsps.sort = this.sort;
    },
      error => this.error = error // error path
      , () => {
        this.working = false;
      });
  }

  GetCurrentClassDatesByID(r) {
    if (r.currentClassDates) {
      r.currentClassDates = null;

      return;
    }
    this.http.postJSON('api/Class/RD_ClassBy/', { ClassID: r.ClassID }).subscribe((d: any) => {
      r.currentClassDates = d;
    });
  }


  getApprovalHistory(FormID: number) {
    let data: IApprovalHistory = {
      ApprovalHistoryID: null,
      ProcessKey: 'CR_CLASS_DATES',
      Step: null,
      FormID: FormID,
      ApproverID: null,
      ApprovalStatusID: null,
      Comments: null,
      ApproverName: null,
      ApproverIDs: null,
      ApproverNames: null,
      StatusDisplayName: null,
      ModifiedDate: null,
      CreatedDate: null,
      IsFinalStep: null,
    };
    /// data object must have ProcessKey & FormID
    this.isAlreadyApproved = false;
    this.isValidApprover = false;
    this.http.postJSON('api/Approval/GetApprovalHistory', data).subscribe(
      (responseData: IApprovalHistory[]) => {
        console.log(responseData);
        if (responseData.length > 0) {
          // let currentUser = this.http.getUserDetails();
          this.latest = responseData[0];
          if (this.latest.ApprovalStatusID != EnumApprovalStatus.Approved) {
            responseData[0].ModifiedDate = responseData[0].CreatedDate;
            responseData[0].ApproverName = responseData[0].ApproverNames;
            /// checks current user is valid for current approval
            if (
              this.latest.ApproverIDs.split(',')
                .map(Number)
                .includes(this.currentUserDetails.UserID)
            ) {
              this.isValidApprover = true;
            } else {
              this.isValidApprover = false;
            }
          } else {
            // data.unshift(latest);
            // this.approvalHistory = data;
            this.isAlreadyApproved = true;
          }
          this.approvalHistory = responseData;
        }
      },
      (error) => {
        this.http.ShowError(error.error + '\n' + error.message);
      }
    );
  }

  onApprove(approvalStatusID: number) {
    let processKey = EnumApprovalProcess.CR_CLASS_DATES;
    this.approvalForm.markAllAsTouched();

    this.save(approvalStatusID, false);
  }

  getApprovalHistoryForSave(FormID: number) {
    let data: IApprovalHistory = {
      ApprovalHistoryID: null,
      ProcessKey: 'CR_CLASS_DATES',
      Step: null,
      FormID: FormID,
      ApproverID: null,
      ApprovalStatusID: null,
      Comments: null,
      ApproverName: null,
      ApproverIDs: null,
      ApproverNames: null,
      StatusDisplayName: null,
      ModifiedDate: null,
      CreatedDate: null,
      IsFinalStep: null,
    };

    // Return the observable with responseData
    return this.http.postJSON('api/Approval/GetApprovalHistory', data)
      .pipe(map((responseData: IApprovalHistory[]) => {
        if (responseData.length > 0) {
          let currentUser = this.http.getUserDetails();
          this.latest = responseData[0];
        }
        return responseData; // Return responseData to be used in the next operator or subscription
      }));
  }

  save(approvalStatusID: number, isPostInSAP: boolean = false) {
    // Get every selected row ids
    let list = this.selection.selected;

    if (!list || list.length <= 0) {
      this.http.ShowError("There is nothing selected");
      return;
    }
    if (this.approvalForm.getRawValue().Remarks == null || this.approvalForm.getRawValue().Remarks == '') {
      this.http.ShowError("Please add Remarks");
      return;
    }
    const requests = list.map(row => {
      const id = row.ClassDatesChangeRequestID;

      // Ensure getApprovalHistoryForSave completes before moving on
      return this.getApprovalHistoryForSave(id).pipe(
        switchMap((responseData: IApprovalHistory[]) => {
          const obj: IApprovalHistory = {
            ApprovalHistoryID: this.latest.ApprovalHistoryID,
            ProcessKey: 'CR_CLASS_DATES',
            FormID: id,
            Step: this.latest.Step,
            ApproverID: this.currentUserDetails.UserID,
            ApprovalStatusID: approvalStatusID,
            Comments: this.approvalForm.getRawValue().Remarks,
            BatchNo: this.approvalForm.getRawValue().BatchNo
          };

          // Return the HTTP post request observable
          return this.http.postJSON('api/Approval/SaveApprovalHistory', obj);
        })
      );
    });

    // Use forkJoin to execute all requests in parallel
    forkJoin(requests).subscribe(
      (responses: any[]) => {
        // All asynchronous operations completed successfully
        this.ngOnInit();
        const message = isPostInSAP
          ? 'Your decision saved successfully & document posted in SAP also.'
          : 'Your decision saved successfully.';
        this.alert = { Type: 'success', Title: 'Success', Message: message };
      },
      (error) => {
        // Handle errors during asynchronous operations
        this.alert = { Type: 'danger', Title: 'Error', Message: error.message };
      }
    );
  }



  //save(approvalStatusID: number, isPostInSAP: boolean = false) {
  //  //Get every selected row ids

  //  let list = this.selection.selected;
  //  if (!list || list.length <= 0) {
  //    this.http.ShowError("There is nothing selected");
  //    return;
  //  }
  //  var ids = list.map(x => x.ClassDatesChangeRequestID);
  //  ids.forEach(id => {
  //    this.getApprovalHistoryForSave(id);    
  //  const obj: IApprovalHistory = {
  //    ApprovalHistoryID: this.latest.ApprovalHistoryID,
  //    ProcessKey: 'CR_CLASS_DATES',
  //    FormID: id,
  //    Step: this.latest.Step,
  //    ApproverID: this.currentUserDetails.UserID,
  //    ApprovalStatusID: approvalStatusID,
  //    Comments: this.approvalForm.getRawValue().Remarks +" -BatchNo:- "+ this.approvalForm.getRawValue().BatchNo,
  //  };
  //  this.http.postJSON('api/Approval/SaveApprovalHistory', obj).subscribe(
  //    (_response: boolean) => {
  //      if (_response === true) {
  //        this.ngOnInit();
  //        const message = this.latest.IsFinalStep && isPostInSAP
  //          ? 'Your decision saved successfully & document posted in SAP also.'
  //          : 'Your decision saved successfully.';
  //        this.alert = { Type: 'success', Title: 'Success', Message: message };

  //        ///Saving the TradeTarget

  //      } else {
  //        this.alert = { Type: 'danger', Title: 'Error', Message: 'Something went wrong in your decision saving.' };
  //      }
  //      // this.onNoClick();
  //    },
  //    (error) => {
  //      this.alert = { Type: 'danger', Title: 'Error', Message: error.error };
  //    }
  //  );
  //  });
  //}

  onSelectAllChange(): void {
    // Handle "Select All" checkbox change logic here
    //this.classesList.forEach(item => item.isChecked = this.selectAll);
    this.classesList.forEach(item => {
      item.isChecked = this.selectAll;
      this.selection.toggle(item); // Manually update selection
    });
  }

  onCheckboxChange(checkedItem: any): void {
    this.selectAll = this.classesList.every(item => item.isChecked);
    this.selection.toggle(checkedItem);
  }

  openApprovalDialogue(row: any): void {
    let processKey = EnumApprovalProcess.CR_CLASS_DATES;

    this.dialogue.openApprovalDialogue(processKey, row.ClassDatesChangeRequestID).subscribe(result => {
      console.log(result);
      //location.reload();
      this.GetClassDatesCRs();
    });
  }
  openClassJourneyDialogue(data: any): void {
    debugger;
    this.dialogue.openClassJourneyDialogue(data);
  }
  openApprovalDialogueBatch(): void {
    debugger;
    this.dialogue.openApprovalDialogueBatch();
  }
  onNoClick(): void {
    this.dialogRef.close(false);

  }

}
export interface IApprovalHistory {
  ApprovalHistoryID?: number;
  ProcessKey: string;
  Step?: number;
  FormID: number;
  ApproverID?: number;
  ApprovalStatusID?: number;
  Comments?: string;
  BatchNo?: string;
  ApproverName?: string;
  ApproverIDs?: string;
  ApproverNames?: string;
  StatusDisplayName?: string;
  ModifiedDate?: Date;
  CreatedDate?: Date;
  IsFinalStep?: boolean;
}
interface ClassType {
  IsChecked: boolean;
  ClassChangeRequestID: number;
  ClassDatesChangeRequestID: number;
  ClassID: number;
  TradeID: number;
  TradeName: string;
  SourceOfCurriculumID: number;
  Name: string;
  CertAuthID: number;
  CertAuthName: string;
  ClusterID: number;
  ClusterName: string;
  TehsilID: number;
  TehsilName: string;
  DistrictID: number;
  DistrictName: string;
  ClassCode: string;
  TrainingAddressLocation: string;
  StartDate: Date;
  EndDate: Date;
  Duration: number;
  IsApproved: boolean;
  IsRejected: boolean;
  CrClassLocationIsApproved: boolean;
  CrClassLocationIsRejected: boolean;
  CrClassLocationID: number;

  CrClassDatesIsApproved: boolean;
  CrClassDatesIsRejected: boolean;
  CrClassDatesID: number;
  BatchNo: string;
}

