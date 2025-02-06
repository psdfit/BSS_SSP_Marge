
//
import { Component, OnInit, Inject } from "@angular/core";
import { CommonSrvService } from "../../common-srv.service";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";
import { FormGroup, FormControl, Validators } from "@angular/forms";
import { EnumApprovalStatus } from "../../shared/Enumerations";
import { UsersModel } from "../../master-data/users/users.component";

@Component({
  selector: 'app-tprn-approvals-dialogue',
  templateUrl: './tprn-approvals-dialogue.component.html',
  styleUrls: ['./tprn-approvals-dialogue.component.scss']
})
export class TprnApprovalsDialogueComponent implements OnInit {

  approvalHistory: IApprovalHistory[] = [];
  approvalForm: FormGroup;
  //canSendBack: boolean = false;
  isValidApprover: boolean = false;
  isAlreadyApproved: boolean = false;
  latest: IApprovalHistory;
  currentUserDetails: UsersModel;
  enumApprovalStatus = EnumApprovalStatus;
  constructor(
    private http: CommonSrvService,
    public dialogRef: MatDialogRef<TprnApprovalsDialogueComponent>,
    @Inject(MAT_DIALOG_DATA) public data: IApprovalHistory
  ) {
    this.approvalForm = new FormGroup({
      Remarks: new FormControl("", [Validators.required]),
    });
  }

  ngOnInit(): void {
    this.currentUserDetails = this.http.getUserDetails();
    this.getApprovalHistory();
  }

  save(approvalStatusID: number) {
    this.approvalForm.markAllAsTouched();
    if (this.approvalForm.valid) {
      let obj: IApprovalHistory = {
        ApprovalHistoryID: this.latest.ApprovalHistoryID,
        ProcessKey: this.data.ProcessKey,
        FormID: this.data.FormID,
        FormIDs: this.data.FormIDs,
        Step: this.latest.Step,
        ApproverID: this.currentUserDetails.UserID,
        ApprovalStatusID: approvalStatusID,
        Comments: this.approvalForm.getRawValue().Remarks,
      };
      this.http.postJSON("api/Approval/SaveTPRNApprovalHistory", obj).subscribe(
        (_response: any) => {
          if (_response == true) {
            this.http.openSnackBar("Saved Successfully");
          } else {
            //this.http.ShowError(this.error.toString(), "Error");
          }
          this.onNoClick();
        },
        (error) => {
          this.http.ShowError(error.error + '\n' + error.message);
        }
      );
    }
  }
  getApprovalHistory() {
    ///data object must have ProcessKey & FormID
    this.data.FormID = this.data.FormIDs[0]
    this.http.postJSON("api/Approval/GetApprovalHistory", this.data).subscribe(
      (responseData: IApprovalHistory[]) => {
        if (responseData.length > 0) {
          //let currentUser = this.http.getUserDetails();
          this.latest = responseData[0];
          if (this.latest.ApprovalStatusID != EnumApprovalStatus.Approved) {
            responseData[0].ModifiedDate = responseData[0].CreatedDate;
            responseData[0].ApproverName = responseData[0].ApproverNames;
            ///checks current user is valid for current approval
            if (
              this.latest.ApproverIDs.split(",")
                .map(Number)
                .includes(this.currentUserDetails.UserID)
            ) {
              this.isValidApprover = true;
            } else {
              this.isValidApprover = false;
            }
          } else {
            //data.unshift(latest);
            //this.approvalHistory = data;
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
  onNoClick(): void {
    this.dialogRef.close(false);
  }
}
export interface IApprovalHistory {
  ApprovalHistoryID?: number;
  ProcessKey: string;
  Step?: number;
  FormID: number;
  FormIDs?: any[];
  ApproverID?: number;
  ApprovalStatusID?: number;
  Comments?: string;

  ApproverName?: string;
  ApproverIDs?: string;
  ApproverNames?: string;
  StatusDisplayName?: string;
  ModifiedDate?: Date;
  CreatedDate?: Date;
}

