import { ChangeDetectorRef, Component, Inject, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatDialog } from '@angular/material/dialog';
import { CommonSrvService } from 'src/app/common-srv.service';
import { MatTableDataSource } from "@angular/material/table";
import { MatPaginator } from "@angular/material/paginator";
import { MatSort } from "@angular/material/sort";
@Component({
  selector: 'app-tsp-association-evaluation-dialogue',
  templateUrl: './tsp-association-evaluation-dialogue.component.html',
  styleUrls: ['./tsp-association-evaluation-dialogue.component.scss']
})
export class TspAssociationEvaluationDialogueComponent implements OnInit {
  currentUser: any;
  Status: any = []
  check: boolean = false
  tradeManageIds: any;
  Criteria: Object;
  TablesData: MatTableDataSource<any>;
  @ViewChild("paginator") paginator: MatPaginator;
  @ViewChild("sort") sort: MatSort;
  TableColumns = [];

  tTablesData: MatTableDataSource<any>;
  @ViewChild("tpaginator") tpaginator: MatPaginator;
  @ViewChild("tsort") tsort: MatSort;
  tTableColumns = [];

  constructor(
    private fb: FormBuilder,
    public ComSrv: CommonSrvService,
    public dialog: MatDialog,
    private cdr: ChangeDetectorRef,
    public dialogRef: MatDialogRef<TspAssociationEvaluationDialogueComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any) {
    dialogRef.disableClose = true;
  }
  ngOnInit(): void {
    this.currentUser = this.ComSrv.getUserDetails()
    this.GetData();
    this.InitAssociationEvaluationForm();
    this.AssociationEvaluationForm.patchValue(this.data[0])
    this.TablesData = new MatTableDataSource([]);
    this.LoadMatTable(this.data[1])
    this.TrailLoadMatTable(this.data[2])
  }
  AssociationEvaluationForm: FormGroup;
  InitAssociationEvaluationForm() {
    this.AssociationEvaluationForm = this.fb.group({
      ID: [0],
      UserID: [this.currentUser.UserID],
      ProgramID: [""],
      TspAssociationEvaluationID: [""],
      TspAssociationMasterID: [""],
      Program: [""],
      TspName: [""],
      TradeLot: [""],
      Status: ['', [Validators.required]],
      VerifiedCapacityMorning: ['', [Validators.required]],
      VerifiedCapacityEvening: ['', [Validators.required]],
      MarksBasedOnEvaluation: ['', [Validators.required]],
      CategoryBasedOnEvaluation: ['', [Validators.required]],
      Remarks: ['', [Validators.required]],
      Attachment: [''],
    });
  }
  camelCaseToWords(input: string): string {
    return input.replace(/([a-z])([A-Z])/g, '$1 $2');
  }
  GetData() {
    this.ComSrv.postJSON("api/BusinessProfile/GetStatus", { UserID: this.currentUser.UserID }).subscribe(
      (response) => {
        this.Status = response;
        const status = this.Status.filter(g => g.Status == this.data[0].EvaluationStatus).map(g => g.TspTradeStatusID)
        this.AssociationEvaluationForm.get("Status").setValue(status[0])
      },
      (error) => {
        this.ComSrv.ShowError(`${error.message}`, "Close", 500000);
      }
    );
  }
  Save() {
    this.AssociationEvaluationForm.value["tradeManageIds"] = this.tradeManageIds
    if (this.AssociationEvaluationForm.valid) {
      this.ComSrv.postJSON("api/Association/SaveAssociationEvaluation", this.AssociationEvaluationForm.value).subscribe(
        (response) => {
          this.check = true
          this.dialogRef.close(true);
          this.ComSrv.openSnackBar("Record update successfully.");
          this.AssociationEvaluationForm.reset()
        },
        (error) => {
          this.ComSrv.ShowError(`${error.message}`, "Close", 500000);
        }
      );
    } else {
      this.ComSrv.ShowError("Required form fields are missing");
    }
  }
  LoadMatTable(tableData: any[]) {
    this.cdr.detectChanges()
    if (tableData.length > 0) {
      const excludeColumnArray = ["TspAssociationDetailID", "CriteriaMainCategoryID", "CriteriaTemplateID"]
      this.TableColumns = Object.keys(tableData[0]).filter(key => !key.includes('ID') && !excludeColumnArray.includes(key));
      this.TablesData = new MatTableDataSource(tableData)
      this.TablesData.paginator = this.paginator;
      this.TablesData.sort = this.sort;
    }
  }
  TrailLoadMatTable(tableData: any[]) {
    this.cdr.detectChanges()
    if (tableData.length > 0) {
      const excludeColumnArray = ["Program", "TspName", "TradeLot"]
      this.tTableColumns = Object.keys(tableData[0]).filter(key => !key.includes('ID') && !excludeColumnArray.includes(key));
      this.tTablesData = new MatTableDataSource(tableData)
      this.tTablesData.paginator = this.tpaginator;
      this.tTablesData.sort = this.tsort;
    }
  }
  ShowPreview(fileName: string) {
    this.ComSrv.PreviewDocument(fileName)
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
