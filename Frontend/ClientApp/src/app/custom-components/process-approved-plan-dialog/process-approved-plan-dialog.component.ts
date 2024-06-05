
import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatDialog } from '@angular/material/dialog';
import { CommonSrvService } from 'src/app/common-srv.service';

// import { DatePipe } from '@angular/common';
@Component({
  selector: 'app-process-approved-plan-dialog',
  templateUrl: './process-approved-plan-dialog.component.html',
  styleUrls: ['./process-approved-plan-dialog.component.scss']
})
export class ProcessApprovedPlanDialogComponent implements OnInit {
  currentUser: any;
  Status: any=[]
  check: boolean=false
  tradeManageIds: any;
  workflow: any;



  constructor(
    private fb: FormBuilder,
    public ComSrv:CommonSrvService,
    public dialog: MatDialog,
    public dialogRef: MatDialogRef<ProcessApprovedPlanDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any) {

    dialogRef.disableClose = true;
  }
  ngOnInit(): void {
    this.currentUser= this.ComSrv.getUserDetails()
    this.GetData();
    this.InitWorkflowform();

  }

  Workflowform: FormGroup;
  InitWorkflowform() {
    this.Workflowform = this.fb.group({
      ID: [0],
      ProgramID: [this.data[0].ProgramID],
      UserID: [this.currentUser.UserID],
      WorkflowID: ['', [Validators.required]],
      Remarks: ['', [Validators.required]],
    });

  }


  GetData() {
    this.ComSrv.postJSON("api/Workflow/LoadWorkflow", { UserID: this.currentUser.UserID }).subscribe(
      (response:any) => {
        this.workflow=response
      },
      (error) => {
        this.ComSrv.ShowError(`${error.message}`, "Close", 500000);
      }
    );
  }


  Save() {

      this.Workflowform.value["tradeManageIds"]=this.tradeManageIds

       if (this.Workflowform.valid) {
        this.ComSrv.postJSON("api/ProgramDesign/SaveProgramWorkflowHistory", this.Workflowform.value).subscribe(
          (response) => {
            this.dialogRef.close(true);
            this.ComSrv.openSnackBar("Record saved successfully.");
            this.Workflowform.reset()
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
