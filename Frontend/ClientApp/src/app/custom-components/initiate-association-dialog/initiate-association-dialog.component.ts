import { filter } from 'rxjs/operators';
import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatDialog } from '@angular/material/dialog';
import { CommonSrvService } from 'src/app/common-srv.service';
@Component({
  selector: 'app-initiate-association-dialog',
  templateUrl: './initiate-association-dialog.component.html',
  styleUrls: ['./initiate-association-dialog.component.scss']
})
export class InitiateAssociationDialogComponent implements OnInit {
  currentUser: any;
  Status: any = []
  check: boolean = false
  tradeManageIds: any;
  Criteria: any;
  attachedCriteria: any;
  error: any;
  constructor(
    private fb: FormBuilder,
    public ComSrv: CommonSrvService,
    public dialog: MatDialog,
    public dialogRef: MatDialogRef<InitiateAssociationDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any) {
    dialogRef.disableClose = true;
  }
  async ngOnInit() {
    this.currentUser = this.ComSrv.getUserDetails()
    this.InitCriteriaForm();
    await this.GetData();
    await this.GetProgramAttachedCriteria()
  }
  CriteriaForm: FormGroup;
  InitCriteriaForm() {
    this.CriteriaForm = this.fb.group({
      ID: [0],
      UserID: [this.currentUser.UserID],
      ProgramID: [this.data[0].ProgramID],
      CriteriaID: ['', [Validators.required]],
      StartDate: ['', [Validators.required]],
      EndDate: ['', [Validators.required]],
      Remarks: ['', [Validators.required]],
    });
  }
  async GetProgramAttachedCriteria() {
    this.SPName = "RD_SSPActiveProgram"
    this.paramObject = {}
    this.attachedCriteria = []
    this.attachedCriteria = await this.FetchData(this.SPName, this.paramObject)
    if (this.attachedCriteria) {
      this.attachedCriteria = this.attachedCriteria.filter(d => d.ProgramID == this.data[0].ProgramID)
    }
    if (this.attachedCriteria != undefined) {
      this.CriteriaForm.get("CriteriaID").setValue(this.attachedCriteria[0].CriteriaID)
      this.CriteriaForm.get("StartDate").setValue(this.attachedCriteria[0].AssociationStartDate)
      this.CriteriaForm.get("EndDate").setValue(this.attachedCriteria[0].AssociationEndDate)
      this.CriteriaForm.get("Remarks").setValue(this.attachedCriteria[0].Detail)
      const CriteriaData = this.Criteria.filter(d => d.CriteriaTemplateID == this.attachedCriteria[0].CriteriaID)
      this.Criteria = CriteriaData
    }
  }
  GetParamString(SPName: string, paramObject: any) {
    let ParamString = SPName;
    for (const key in paramObject) {
      if (Object.hasOwnProperty.call(paramObject, key)) {
        ParamString += `/${key}=${paramObject[key]}`;
      }
    }
    return ParamString;
  }
  paramObject: any = {}
  ExportReportName: string = ""
  SPName: string = ""
  async FetchData(SPName: string, paramObject: any) {
    try {
      const Param = this.GetParamString(SPName, paramObject);
      const data: any = await this.ComSrv.getJSON(`api/BSSReports/FetchReportData?Param=${Param}`).toPromise();
      if (data.length > 0) {
        return data;
      } else {
        this.ComSrv.ShowWarning(' No Record Found', 'Close');
      }
    } catch (error) {
      this.error = error;
    }
  }
  GetData() {
    this.ComSrv.postJSON("api/CriteriaTemplate/LoadCriteria", { UserID: this.currentUser.UserID }).subscribe(
      (response) => {
        this.Criteria = response;
      },
      (error) => {
        this.ComSrv.ShowError(`${error.message}`, "Close", 500000);
      }
    );
  }
  Save() {
    debugger;
    const StartDate = new Date(this.CriteriaForm.value.StartDate);
    const EndDate = new Date(this.CriteriaForm.value.EndDate);
    if (EndDate >= StartDate) {
      this.CriteriaForm.value["tradeManageIds"] = this.tradeManageIds
      if (this.CriteriaForm.valid) {
        this.ComSrv.postJSON("api/ProgramDesign/SaveProgramCriteriaHistory", this.CriteriaForm.value).subscribe(
          (response) => {
            this.check = true
            this.dialogRef.close(true);
            this.ComSrv.openSnackBar("Record update successfully.");
            this.CriteriaForm.reset()
          },
          (error) => {
            this.ComSrv.ShowError(`${error.message}`, "Close", 500000);
          }
        );
      } else {
        this.ComSrv.ShowError("Required form fields are missing");
      }
    } else {
      this.ComSrv.ShowError("End Date must be greater than from  Start Date.");
      return
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
