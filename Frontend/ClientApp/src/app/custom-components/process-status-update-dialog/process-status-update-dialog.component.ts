
import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatDialog } from '@angular/material/dialog';
import { CommonSrvService } from 'src/app/common-srv.service';

@Component({
  selector: 'app-process-status-update-dialog',
  templateUrl: './process-status-update-dialog.component.html',
  styleUrls: ['./process-status-update-dialog.component.scss']
})
export class ProcessStatusUpdateDialogComponent implements OnInit {
  currentUser: any;
  Status: any=[]
  check: boolean=false
  tradeManageIds: any;



  constructor(
    private fb: FormBuilder,
    public ComSrv:CommonSrvService,
    public dialog: MatDialog,
    public dialogRef: MatDialogRef<ProcessStatusUpdateDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any) {

    dialogRef.disableClose = true;
  }
  ngOnInit(): void {
    this.currentUser= this.ComSrv.getUserDetails()
    console.log(this.currentUser)
    this.GetData();
    this.InitStatusForm();
   console.log(this.data)

  }

  StatusForm: FormGroup;
  InitStatusForm() {


    this.StatusForm = this.fb.group({
      ID: [0],
      UserID: [this.currentUser.UserID],
      ProgramID: [this.data[0].ProgramID],
      StatusID: ['', [Validators.required]],
      Remarks: ['', [Validators.required]],
    });

  }

  GetData(){

      this.ComSrv.postJSON("api/BusinessProfile/GetStatus", {UserID:this.currentUser.UserID}).subscribe(
        (response) => {
          this.Status =response ;

            this.Status = this.Status


        },
        (error) => {
          this.ComSrv.ShowError(`${error.message}`, "Close", 500000);
        }
      );


  }

    Save() {


       if (this.StatusForm.valid) {



        this.ComSrv.postJSON("api/ProgramDesign/SaveProgramStatusHistory", this.StatusForm.value).subscribe(
          (response) => {
            console.log(response[0])
            this.check=true
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
      const error=errorValue.requiredLength==15?errorValue.requiredLength-2:errorValue.requiredLength-1
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
