import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatDialog } from '@angular/material/dialog';
import { CommonSrvService } from 'src/app/common-srv.service';
@Component({
  selector: 'app-device-status-update-dialog',
  templateUrl: './device-status-update-dialog.component.html',
  styleUrls: ['./device-status-update-dialog.component.scss']
})
export class DeviceStatusUpdateDialogComponent implements OnInit {
  currentUser: any;
  Status: any = []
  check: boolean = false
  tradeManageIds: any;
  spacerTitle = ''
  updateStatus: string=''
  constructor(
    private fb: FormBuilder,
    public ComSrv: CommonSrvService,
    public dialog: MatDialog,
    public dialogRef: MatDialogRef<DeviceStatusUpdateDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any) {
      console.log(this.currentUser)
    dialogRef.disableClose = true;
  }
  ngOnInit(): void {
    this.currentUser = this.ComSrv.getUserDetails()
    console.log(this.data)
    if(this.data[1] == 'Activate'){
      this.spacerTitle ='Device Activation Request'
      this.updateStatus='Activation'
    }else{
      this.spacerTitle ='Device DeActivation Request'
      this.updateStatus='DeActivation'

    }
    this.InitStatusForm();
  }
  StatusForm: FormGroup;
  InitStatusForm() {
    
    this.StatusForm = this.fb.group({
      ActivationLogID: [0],
      RegistrationID: [this.data[0].RegistrationID],
      DeviceStatusRequest: [this.updateStatus],
      UserID: [this.currentUser.UserID],
      Remarks: ['', [Validators.required]],
    });
  }

  Save() {
    if (this.StatusForm.valid) {
debugger
    const data={...this.data[0],...this.StatusForm.value}
      this.ComSrv.postJSON("api/DeviceManagement/UpdateDeviceStatus",data).subscribe(
        (response) => {
          console.log(response[0])
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
