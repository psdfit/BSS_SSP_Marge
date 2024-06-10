import { filter } from 'rxjs/operators';
import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatDialog } from '@angular/material/dialog';
import { CommonSrvService } from 'src/app/common-srv.service';
// import { DatePipe } from '@angular/common';
@Component({
  selector: 'app-tsp-status-update',
  templateUrl: './tsp-status-update.component.html',
  styleUrls: ['./tsp-status-update.component.scss']
})
export class TspStatusUpdateComponent implements OnInit {
  currentUser: any;
  Status: any = []
  check: boolean = false
  tradeManageIds: any;
  constructor(
    private fb: FormBuilder,
    public ComSrv: CommonSrvService,
    public dialog: MatDialog,
    public dialogRef: MatDialogRef<TspStatusUpdateComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any) {
    dialogRef.disableClose = true;
  }
  ngOnInit(): void {
    this.currentUser = this.ComSrv.getUserDetails()
    this.GetData();
    this.InitStatusForm();
    console.log(this.data)
    this.tradeManageIds = []
    this.tradeManageIds = this.data[0].map(d => d.TradeManageID).join(",")
  }
  StatusForm: FormGroup;
  InitStatusForm() {
    this.StatusForm = this.fb.group({
      UserID: [this.currentUser.UserID],
      Status: ['', [Validators.required]],
      Remarks: ['', [Validators.required]],
    });
  }
  GetData() {
    this.ComSrv.postJSON("api/BusinessProfile/GetStatus", { UserID: this.currentUser.UserID }).subscribe(
      (response) => {
        this.Status = response;
        if (this.data[1][0] == 2) {
          this.Status = this.Status.filter(s => s.Status == "Send-Back" || s.Status == "Rejected" || s.Status == "Accepted");
        } else {
          this.Status = this.Status.filter(s => s.Status == "Accepted" || s.Status == "Rejected" || s.Status == "On-Hold");
        }
      },
      (error) => {
        this.ComSrv.ShowError(`${error.message}`, "Close", 500000);
      }
    );
  }
  UpdateStatus() {
    this.StatusForm.value["tradeManageIds"] = this.tradeManageIds

    const Status = this.Status.filter(d => d.TspTradeStatusID == this.StatusForm.get("Status").value)
    this.StatusForm.value["TSPID"] = this.data[2].UserID
    this.StatusForm.value["TSPName"] = this.data[2].TspName
    this.StatusForm.value["TradeName"] = this.data[0][0].TradeName
    this.StatusForm.value["StatusName"] = Status[0].Status

    if (this.StatusForm.valid) {
      if (this.StatusForm.get("Status").value == 4 || this.StatusForm.get("Status").value == 5) {
        this.StatusForm.value["ApprovalLevel"] = this.data[1][0]
      } else {
        this.StatusForm.value["ApprovalLevel"] = 0
      }
      console.log(this.StatusForm.value)
      this.ComSrv.postJSON("api/BusinessProfile/UpdateTradeStatus", this.StatusForm.value).subscribe(
        (response) => {

          this.check = true
          this.dialogRef.close(true);
          this.ComSrv.openSnackBar("Record update successfully.");
          this.StatusForm.reset()
        },
        (error) => {
          this.ComSrv.ShowError(`${error.message}`, "Close", 500000);
        }
      );
    } else {
      this.ComSrv.ShowError("Required form fields are missing");
    }
  }
  getErrorMessage(errorKey: string, errorValue: any): string {
    const error = errorValue.requiredLength == 15 ? errorValue.requiredLength - 2 : errorValue.requiredLength - 1
    const errorMessages = {
      required: 'This field is required.',
      minlength: `This field must be at least ${error} characters long.`,
      maxlength: `This field's text exceeds the specified maximum length.  (maxLength: ${errorValue.requiredLength} characters)`,
      email: 'Invalid email address.',
      pattern: 'This field is only required text',
      customError: errorValue
    };
    return errorMessages[errorKey];
  }
}
