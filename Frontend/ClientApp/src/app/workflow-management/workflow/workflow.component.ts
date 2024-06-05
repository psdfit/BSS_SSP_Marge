import { filter } from 'rxjs/operators';
import { Component, OnInit, ViewChild, ChangeDetectorRef } from "@angular/core";
import { CommonSrvService } from "../../common-srv.service";
import { ActivatedRoute, Router } from "@angular/router";
import { MatTableDataSource } from "@angular/material/table";
import { MatPaginator } from "@angular/material/paginator";
import { MatSort } from "@angular/material/sort";
import { MatTabGroup } from "@angular/material/tabs";
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from "@angular/forms";
import { MatDialog } from "@angular/material/dialog";
import { environment } from '../../../environments/environment';
@Component({
  selector: 'app-workflow',
  templateUrl: './workflow.component.html',
  styleUrls: ['./workflow.component.scss']
})

export class WorkflowComponent implements OnInit {
  environment = environment;
  TapIndex: any;
  currentUser: any;
  savebtn: string = "Save "
  GetDataObject: any = {}
  Workflow: any;
  WorkflowTableColumns: any;
  RegistrationStatus: any;
  Trade: any;
  EducationTypes: any;
  WorkflowEditRecord = []
  //  Start veriables
  SpacerTitle: string;
  TSearchCtr = new FormControl('');
  BSearchCtr = new FormControl('');
  TapTTitle: string = "Profile"
  Data: any = []
  Gender: any = []
  TableColumns = [];
  sourcingType: any;
  workflowTask: any;

  constructor(
    private ComSrv: CommonSrvService,
    private AcitveRoute: ActivatedRoute,
    private fb: FormBuilder,
    private Cdr: ChangeDetectorRef,
    private Dialog: MatDialog,
  ) { }

  maxDate: Date;
  @ViewChild("tabGroup") tabGroup: MatTabGroup;
  WorkflowTablesData: MatTableDataSource<any>;
  @ViewChild("WorkflowPaginator") WorkflowPaginator: MatPaginator;
  @ViewChild("WorkflowSort") WorkflowSort: MatSort;

  ngOnInit(): void {
    this.currentUser = this.ComSrv.getUserDetails();
    this.TapIndex = 0
    this.WorkflowTablesData = new MatTableDataSource([]);
    this.PageTitle();
    this.GetData();
    this.InitWorkflowForm();
  }

  PageTitle(): void {
    this.ComSrv.setTitle(this.AcitveRoute.snapshot.data.title);
    this.SpacerTitle = this.AcitveRoute.snapshot.data.title;
  }

  Edit(WorkflowID) {
    this.savebtn = "Update ";
    const WorkData = this.GetDataObject.workflow.filter(d => d.WorkflowID == WorkflowID)
    const taskData = this.GetDataObject.workflowTask.filter(d => d.WorkflowID == WorkflowID)
    this.WorkflowForm.patchValue(WorkData[0]);
    this.PopulateTaskDetail(taskData)
  }

  PopulateTaskDetail(data) {
    this.taskDetails.clear()
    this.WorkflowEditRecord = data
    data.forEach(detail => {
      this.taskDetails.push(this.fb.group(detail));
    });
    console.log(this.taskDetails.value)
  }


  WorkflowForm: FormGroup;
  WorkflowDetails: FormArray;
  InitWorkflowForm() {
    this.WorkflowForm = this.fb.group({
      UserID: [this.currentUser.UserID],
      WorkflowID: [0],
      WorkflowTitle: ['', [Validators.required]],
      SourcingTypeID: [1, [Validators.required]],
      Description: ['', [Validators.required]],
      TotalDays: ['', [Validators.required]],
      TotalTaskDays: [{ value: 0, disabled: true }, [Validators.required]],
      taskDetails: this.fb.array([])
    });

    this.WorkflowForm.get('taskDetails').valueChanges.subscribe(d => {
      this.WorkflowForm.get('TotalTaskDays').setValue(d.map((category) => Number(category.TaskDays)).reduce((a, b) => a + b, 0))


    })

    this.AddWorkflowDetails()
  }
  Save() {
    if (this.WorkflowForm.get('TotalTaskDays').value != this.WorkflowForm.get('TotalDays').value) {
      this.ComSrv.ShowError("Total days must be equal to task days.");

      return
    }

    if (this.taskDetails.length > 0) {
      if (this.WorkflowForm.valid && this.taskDetails.valid) {
        this.ComSrv.postJSON("api/Workflow/Save", this.WorkflowForm.getRawValue()).subscribe(
          (response) => {
            this.ComSrv.openSnackBar("Record saved successfully.");
            console.log(this.removedTaskIds)
            for (let index = 0; index < this.removedTaskIds.length; index++) {
                this.DeleteWorkflowDetail(this.removedTaskIds[index])
            }
            this.InitWorkflowForm()

            this.GetData();
          },
          (error) => {
            this.ComSrv.ShowError(`${error.message}`, "Close", 500000);
          }
        );
      } else {
        this.ComSrv.ShowError("Required form fields are missing");
      }
    } else {
      this.ComSrv.ShowError("Task detail is required");
    }
  }
  AddWorkflowDetails() {
    this.WorkflowDetails = this.WorkflowForm.get("taskDetails") as FormArray;
    this.WorkflowDetails.push(this.RowGenerator());
  }
  RowGenerator() {
    return this.fb.group({
      WorkflowID: [0],
      UserID: [this.currentUser.UserID],
      TaskID: [0],
      TaskName: ['', [Validators.required]],
      TaskDays: ['', [Validators.required]],
      TaskApproval: ['', [Validators.required]],
      TaskStatus: ['Pending', [Validators.required]],
    });
  }
  get taskDetails() {
    return this.WorkflowForm.get("taskDetails") as FormArray;
  }
  removedTaskIds:any=[]
  RemoveDetail(index: any, row: FormArray) {

    if (confirm('Do you want to remove this details?')) {
      if (row.value[index].TaskID > 0) {

        this.removedTaskIds.push(row.value[index])
      }
      this.WorkflowDetails = this.WorkflowForm.get("taskDetails") as FormArray;
      this.WorkflowDetails.removeAt(index)
    }
  }
  DeleteWorkflowDetail(row) {
    if (row.TaskID > 0) {
      this.ComSrv.postJSON("api/Workflow/removeTaskDetail", row).subscribe(
        (response) => {
          this.ComSrv.openSnackBar("Workflow's task has been removed.");

        },
        (error) => {
          this.ComSrv.ShowError(`${error.message}`, "Close", 500000);
        }
      );
    }
  }
  GetData() {
    this.ComSrv.postJSON("api/Workflow/FetchData", { UserID: this.currentUser.UserID }).subscribe(
      (response) => {
        this.GetDataObject = response
        this.Workflow = this.GetDataObject.workflow
        this.workflowTask = this.GetDataObject.workflowTask
        this.sourcingType = this.GetDataObject.sourcingType
        if (this.Workflow.length > 0) {
          this.LoadMatTable(this.GetDataObject.workflow, "Workflow");
        }
      },
      (error) => {
        this.ComSrv.ShowError(`${error.message}`, "Close", 500000);
      }
    );
  }
  EmptyCtrl(ev: any) {
    this.BSearchCtr.setValue('');
  }
  camelCaseToWords(input: string): string {
    return input.replace(/([a-z])([A-Z])/g, '$1 $2');
  }
  LoadMatTable(tableData: any, tableName: string) {
    switch (tableName) {
      case "Workflow":
        this.WorkflowTableColumns = Object.keys(tableData[0]).filter(key =>
          !key.includes('ID')
        );
        this.WorkflowTablesData = new MatTableDataSource(tableData);
        this.WorkflowTablesData.paginator = this.WorkflowPaginator;
        this.WorkflowTablesData.sort = this.WorkflowSort;
        break;
    }
  }
  ResetFrom() {
    this.InitWorkflowForm()
    this.savebtn = "Save";
    this.WorkflowForm.reset()
  }
  applyFilter(data: MatTableDataSource<any>, event: any) {
    data.filter = event.target.value.trim().toLowerCase();
    if (data.paginator) {
      data.paginator.firstPage();
    }
  }
  DataExcelExport(Data: any, ReportName: string) {
    this.ComSrv.ExcelExporWithForm(Data, ReportName);
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
  ShowPreview(fileName: string) {
    this.ComSrv.PreviewDocument(fileName)
  }

  ngAfterViewInit() {


   setTimeout(() => {
    if(this.ComSrv.getMessage().WorkflowID != undefined){
      this.Edit(this.ComSrv.getMessage().WorkflowID)
    }
   }, 1000);

    if (this.tabGroup) {
      this.tabGroup.selectedTabChange.subscribe((event) => {
        this.TapIndex = event.index
      });
    }
  }
}
