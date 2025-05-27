import { ChangeDetectorRef, Component, Inject, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatDialog } from '@angular/material/dialog';
import { CommonSrvService } from 'src/app/common-srv.service';
import { MatTableDataSource } from "@angular/material/table";
import { MatPaginator } from "@angular/material/paginator";
import { MatSort } from "@angular/material/sort";
@Component({
  selector: 'app-trainee-guru-dialog',
  templateUrl: './trainee-guru-dialog.component.html',
  styleUrls: ['./trainee-guru-dialog.component.scss']
})
export class TraineeGuruDialogComponent implements OnInit {
  currentUser: any;

  TablesData: MatTableDataSource<any>;
  @ViewChild("paginator") paginator: MatPaginator;
  @ViewChild("sort") sort: MatSort;
  TableColumns = [];
  isEditable: boolean=false
  traineeGuruList:any;

  constructor(
    private fb: FormBuilder,
    public comSrv: CommonSrvService,
    public dialog: MatDialog,
    private cdr: ChangeDetectorRef,
    public dialogRef: MatDialogRef<TraineeGuruDialogComponent>,
    @Inject(MAT_DIALOG_DATA) 
    public data: any) {
    dialogRef.disableClose = true;
  }
  dateFilter = (d: Date | null): boolean => {
    // Prevent after current date selection .
    const date = (d || new Date());
    return date <= new Date();
  }
  ngOnInit() {
    this.GetData();
    this.currentUser = this.comSrv.getUserDetails()
    this.TablesData = new MatTableDataSource([]);
    this.InitTraineeGuruForm();
  }
  
  
  GetData() {
    this.getTraineeGuru()
  }

  getTraineeGuru() {
    const userID = this.comSrv.getUserDetails().UserID;
    this.comSrv.getJSON(`api/TraineeGuruProfile/GetTraineeGuru?UserID=${userID}`).subscribe(
      (response) => {
        this.traineeGuruList = response;
        this.LoadMatTable(this.traineeGuruList)
      },
      (error) => {
       this.comSrv.ShowError(error.message);
      }
    );
  }


  traineeGuruForm: FormGroup;
  InitTraineeGuruForm() {
    this.traineeGuruForm = this.fb.group({
      GuruProfileID: [null],
      FullName:['',[Validators.required]],
      ContactNumber:['',[Validators.required, Validators.minLength(12), Validators.maxLength(12)]],
      CNIC: ['',[Validators.required, Validators.minLength(15), Validators.maxLength(15)]],
      CNICIssuedDate: ['',[Validators.required]],
      CurUserID : [this.currentUser.UserID],
    });
  }
  get TraineeGuruProfileID() { return this.traineeGuruForm.get("TraineeGuruProfileID"); }
  get FullName() { return this.traineeGuruForm.get("FullName"); }
  get ContactNumber() { return this.traineeGuruForm.get("ContactNumber"); }
  get CNIC() { return this.traineeGuruForm.get("CNIC"); }
  get CNICIssuedDate() { return this.traineeGuruForm.get("CNICIssuedDate"); }
  get UserID() { return this.traineeGuruForm.get("CreatedByUserID"); }

  camelCaseToWords(input: string): string {
    return input.replace(/([a-z])([A-Z])/g, '$1 $2');
  }

 
  
  Save() {
    // Check for duplicate CNIC
    if (this.guruCNICCheck()) {
      return;
    }
    // Validate and save the form
    if (this.traineeGuruForm.valid) {
      this.comSrv.postJSON("api/TraineeGuruProfile/SaveTraineeGuruProfile", this.traineeGuruForm.value)
        .subscribe(
          (response) => {
            this.dialogRef.close(true);
            this.isEditable=false
            this.traineeGuruForm.reset();
            this.comSrv.openSnackBar("Record  updated successfully.");
          },
          (error) => {
            this.comSrv.ShowError(`${error.error}`, "Close", 5000);
          }
        );
    } else {
      this.comSrv.ShowError("Required form fields are missing");
    }
  }

  editRecord(guruProfile: any) {
    this.isEditable=true
    this.traineeGuruForm.patchValue(guruProfile);
  }

  guruCNICCheck(): boolean {
    debugger;
    const isExisted = this.isEditable
      ? this.traineeGuruList.find(
        g => g.CNIC === this.CNIC.value && g.GuruProfileID != this.traineeGuruForm.get("GuruProfileID").value
          && g.CurUserID === this.traineeGuruForm.get("CreatedByUserID").value
        )
      : this.traineeGuruList.find(g => g.CNIC === this.CNIC.value);
    if (isExisted) {
      this.comSrv.ShowError("Duplicate CNIC is not allowed");
      return true;
    }
    return false;
  }
  LoadMatTable(tableData: any[]) {
    this.cdr.detectChanges()
    if (tableData.length > 0) {
      const excludeColumnArray = ["CreatedDate"]
      this.TableColumns = Object.keys(tableData[0]).filter(key => !key.includes('ID') && !excludeColumnArray.includes(key));
      this.TableColumns.unshift("Action")
      this.TablesData = new MatTableDataSource(tableData)
      this.TablesData.paginator = this.paginator;
      this.TablesData.sort = this.sort;
    }
  }

  ShowPreview(fileName: string) {
    this.comSrv.PreviewDocument(fileName)
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
