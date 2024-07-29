/* **** Aamer Rehman Malik *****/
import { Component, OnInit, Inject } from '@angular/core';
import { CommonSrvService } from '../../common-srv.service';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { EnumApprovalStatus, EnumApprovalProcess } from '../../shared/Enumerations';
import { UsersModel } from '../../master-data/users/users.component';
import { environment } from '../../../environments/environment';

@Component({
  selector: 'app-approval-dialog',
  templateUrl: './approval-dialogue.component.html',
  styleUrls: ['./approval-dialogue.component.scss'],
})
export class ApprovalDialogueComponent implements OnInit {
  environment = environment;
  approvalHistory: IApprovalHistory[] = [];
  TradeTargetDetail: ITradeDetail[] = [];
  approvalForm: FormGroup;
  // canSendBack: boolean = false;
  isValidApprover = false;
  isAlreadyApproved = false;
  RejectedButtonHideBit = true;
  latest: IApprovalHistory;
  latestid: ITradeDetail;
  TradeTargetValid: boolean = true;
  currentUserDetails: UsersModel;
  enumApprovalStatus = EnumApprovalStatus;
  alert: { Type: string, Title?: string, Message?: string; } = null;
  constructor(
    private http: CommonSrvService,
    public dialogRef: MatDialogRef<ApprovalDialogueComponent>,
    @Inject(MAT_DIALOG_DATA) public data: IApprovalHistory
  ) {
    this.RejectedButtonHide(data.ProcessKey);
    this.approvalForm = new FormGroup({
      Remarks: new FormControl('', [Validators.required]),
      TradeTarget: new FormControl(''),
    });
  }

  ngOnInit(): void {
    this.currentUserDetails = this.http.getUserDetails();
    this.getApprovalHistory();
    this.getTradeData();
  }

  RejectedButtonHide(ProcessKey: any) {
    if (ProcessKey === 'PO_SRN'
      || ProcessKey === 'PO_TRN'
      || ProcessKey === 'PO_VRN'
      || ProcessKey === 'PO_TSP'
      || ProcessKey === 'PRN_C'
      || ProcessKey === 'PRN_F'
      || ProcessKey === 'PRN_R'
      || ProcessKey === 'PRN_T'
      || ProcessKey === 'SRN'
      || ProcessKey === 'VRN'
      || ProcessKey === 'INV_1ST'
      || ProcessKey === 'INV_2ND'
      || ProcessKey === 'INV_C'
      || ProcessKey === 'INV_F'
      || ProcessKey === 'INV_R'
      || ProcessKey === 'INV_SRN'
      || ProcessKey === 'INV_VRN'
      || ProcessKey === 'INV_TRN'
    )
      this.RejectedButtonHideBit = false;
  }

  onApprove(approvalStatusID: number) {
    const isPostInSAP = (
      EnumApprovalProcess.AP_BD === this.latest.ProcessKey
      || EnumApprovalProcess.AP_PD === this.latest.ProcessKey
      || EnumApprovalProcess.PO_SRN === this.latest.ProcessKey
      || EnumApprovalProcess.PO_VRN === this.latest.ProcessKey
      || EnumApprovalProcess.PO_TRN === this.latest.ProcessKey
      || EnumApprovalProcess.PO_TSP === this.latest.ProcessKey
      || EnumApprovalProcess.INV_R === this.latest.ProcessKey
      || EnumApprovalProcess.INV_SRN === this.latest.ProcessKey
      || EnumApprovalProcess.INV_VRN === this.latest.ProcessKey
      || EnumApprovalProcess.INV_F === this.latest.ProcessKey
      || EnumApprovalProcess.INV_TRN === this.latest.ProcessKey
      || EnumApprovalProcess.INV_C === this.latest.ProcessKey
      || EnumApprovalProcess.TRD === this.latest.ProcessKey

    );
    if (this.latest.IsFinalStep && isPostInSAP && EnumApprovalProcess.AP_PD === this.latest.ProcessKey) {
      if (this.latestid.PTypeID == 7) {
        let obj = this.TradeTargetDetail;

        for (var i = 0; i < obj.length; i++) {
          if (!obj[i].TradeTarget) {
            this.alert = { Type: 'danger', Title: 'Error', Message: "Trade Target or Remarks input fields can not to be left blank!" };
            this.TradeTargetValid = false;
            break;
          }
          else {
            this.TradeTargetValid = true;
          }
        }
      }
    }

    this.approvalForm.markAllAsTouched();

    if (this.approvalForm.valid && this.TradeTargetValid) {
      if (this.latest.IsFinalStep && isPostInSAP && this.TradeTargetValid) {
        const titleConfirm = 'Are you sure? This operation will take some time';
        const messageConfirm = 'On your decision, document will be posted in SAP.';
        this.http.confirm(titleConfirm, messageConfirm).subscribe(
          (isConfirm: boolean) => {
            console.log(isConfirm);
            if (isConfirm === true) {
              this.save(approvalStatusID, isPostInSAP);

            }
          });
      } else {
        this.save(approvalStatusID, isPostInSAP);
      }

    }
  }
  save(approvalStatusID: number, isPostInSAP: boolean = false) {
    const obj: IApprovalHistory = {
      ApprovalHistoryID: this.latest.ApprovalHistoryID,
      ProcessKey: this.data.ProcessKey,
      FormID: this.data.FormID,
      Step: this.latest.Step,
      ApproverID: this.currentUserDetails.UserID,
      ApprovalStatusID: approvalStatusID,
      Comments: this.approvalForm.getRawValue().Remarks,
    };
    debugger;
    this.http.postJSON('api/Approval/SaveApprovalHistory', obj).subscribe(
      (_response: boolean) => {
        debugger;
        if (_response === true) {
          this.ngOnInit();
          if (!this.latest.IsFinalStep && isPostInSAP && EnumApprovalProcess.AP_PD === this.latest.ProcessKey) {
            if (this.latestid.PTypeID == 7) {
              this.saveTradeTarget();
            }
          }
          if (this.latest.IsFinalStep && isPostInSAP && EnumApprovalProcess.AP_PD === this.latest.ProcessKey) {
            if (this.latestid.PTypeID == 7) {
              this.UpdateTradeTarget();
            }
          }
          const message = this.latest.IsFinalStep && isPostInSAP
            ? 'Your decision saved successfully & document posted in SAP also.'
            : 'Your decision saved successfully.';
          this.alert = { Type: 'success', Title: 'Success', Message: message };

          ///Saving the TradeTarget

        } else {
          this.alert = { Type: 'danger', Title: 'Error', Message: 'Something went wrong in your decision saving.' };
        }
        // this.onNoClick();
      },
      (error) => {
        this.alert = { Type: 'danger', Title: 'Error', Message: error.error };
      }
    );
  }

  //Save TradeTarget
  saveTradeTarget() {
    let obj = this.TradeTargetDetail;

    this.http.postJSON('api/Approval/SaveTradeTarget', obj).subscribe(
      (_response: boolean) => {
        if (_response === true) {
          this.ngOnInit();
        } else {
          this.alert = { Type: 'danger', Title: 'Error', Message: 'Something went wrong in your decision saving. Trade Target not saved!' };
        }
        // this.onNoClick();
      },
      (error) => {
        this.alert = { Type: 'danger', Title: 'Error', Message: error.error };
      }
    );
  }

  //Update TradeTarget
  UpdateTradeTarget() {
    let obj = this.TradeTargetDetail;

    this.http.postJSON('api/Approval/UpdateTradeTarget', obj).subscribe(
      (_response: boolean) => {
        if (_response === true) {
          this.ngOnInit();
        } else {
          this.alert = { Type: 'danger', Title: 'Error', Message: 'Something went wrong in your decision saving. Trade Target not saved!' };
        }
        // this.onNoClick();
      },
      (error) => {
        this.alert = { Type: 'danger', Title: 'Error', Message: error.error };
      }
    );
  }

  getApprovalHistory() {
    /// data object must have ProcessKey & FormID
    this.http.postJSON('api/Approval/GetApprovalHistory', this.data).subscribe(
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

  getTradeData() { ///Getting trade date
    this.http.postJSON('api/Approval/GetTradeDate', this.data).subscribe(
      (responseData: ITradeDetail[]) => {
        console.log(responseData);
        if (responseData.length > 0) {
          this.latestid = responseData[0];
          this.TradeTargetDetail = responseData;
        }
      },
      (error) => {
        this.http.ShowError(error.error + '\n' + error.message);
      }
    );
  }

  onNoClick(): void {
    this.dialogRef.close(false);
  }

  onBlurEvent(event: any, id: any) {
    this.latestid.TradeTarget[id] = event.target.value;

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

  ApproverName?: string;
  ApproverIDs?: string;
  ApproverNames?: string;
  StatusDisplayName?: string;
  ModifiedDate?: Date;
  CreatedDate?: Date;
  IsFinalStep?: boolean;
}

export interface ITradeDetail {
  SchemeID?: number;
  TradeID?: number;
  PTypeID?: number;
  TradeTarget?: number;
  TradeName?: string;
}


