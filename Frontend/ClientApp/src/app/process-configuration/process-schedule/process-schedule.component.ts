import { filter } from 'rxjs/operators';
import { Component, OnInit, ViewChild, ChangeDetectorRef } from "@angular/core";
import { CommonSrvService } from "../../common-srv.service";
import { ActivatedRoute } from "@angular/router";
import { MatTableDataSource } from "@angular/material/table";
import { MatPaginator } from "@angular/material/paginator";
import { MatSort } from "@angular/material/sort";
import { MatTabGroup } from "@angular/material/tabs";
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from "@angular/forms";
import { MatDialog } from "@angular/material/dialog";
import { environment } from '../../../environments/environment';
@Component({
  selector: 'app-process-schedule',
  templateUrl: './process-schedule.component.html',
  styleUrls: ['./process-schedule.component.scss']
})
export class ProcessScheduleComponent implements OnInit {
  environment = environment;
  TapIndex: any;
  currentUser: any;
  savebtn: string = "Save "
  GetDataObject: any = {}
  ProcessSchedule: any;
  ProcessScheduleTableColumns: any;
  RegistrationStatus: any;
  Trade: any;
  EducationTypes: any;
  ProcessScheduleEditRecord = []
  //  Start veriables
  SpacerTitle: string;
  PSearchCtr = new FormControl('');
  BSearchCtr = new FormControl('');
  TapTTitle: string = "Profile"
  Data: any = []
  Gender: any = []
  TableColumns = [];
  ProcessScheduleTask: any;
  error: any;
  processScheduleMaster: any = []
  processScheduleDetail: any = []
  ProcessScheduleMasterID: any;
  programData: any;
  ProcessScheduleData: any;
  isDisabled: boolean = false
  isEdit: boolean = false
  constructor(
    private ComSrv: CommonSrvService,
    private ActiveRoute: ActivatedRoute,
    private fb: FormBuilder,
    private Cdr: ChangeDetectorRef,
    private Dialog: MatDialog,
  ) { }
  maxDate: Date;
  @ViewChild("tabGroup") tabGroup: MatTabGroup;
  ProcessScheduleTablesData: MatTableDataSource<any>;
  @ViewChild("ProcessSchedulePaginator") ProcessSchedulePaginator: MatPaginator;
  @ViewChild("ProcessScheduleSort") ProcessScheduleSort: MatSort;
  ngOnInit() {
    this.currentUser = this.ComSrv.getUserDetails();
    this.TapIndex = 0
    this.ProcessScheduleTablesData = new MatTableDataSource([]);
    this.InitProcessScheduleForm();
    this.GetActiveProgram()
    this.GetProcessSchedule()
    this.PageTitle();
    this.GetData();
  }
  PageTitle(): void {
    this.ComSrv.setTitle(this.ActiveRoute.snapshot.data.title);
    this.SpacerTitle = this.ActiveRoute.snapshot.data.title;
  }
  ProcessScheduleForm: FormGroup;
  ProcessScheduleDetails: FormArray;
  InitProcessScheduleForm() {
    this.ProcessScheduleForm = this.fb.group({
      ProcessScheduleMasterID: [0],
      ProgramID: ['', [Validators.required]],
      ProgramStartDate: ['', [Validators.required]],
      TotalDays: [0, [Validators.required]],
      TotalProcess: [0, [Validators.required]],
      InActive: [false],
      UserID: [this.currentUser.UserID],
      processDetails: this.fb.array([])
    });
    this.ProcessScheduleForm.get('processDetails').valueChanges.subscribe(d => {
      this.ProcessScheduleForm.get('TotalDays').setValue(d.map((processDetails) => Number(processDetails.ProcessDays)).reduce((a, b) => a + b, 0))
      this.ProcessScheduleForm.get('TotalProcess').setValue(d.length)
      
 
     
    })
    this.ProcessScheduleForm.get('ProgramID').valueChanges.subscribe(pd => {
      const programExisted: any = this.processScheduleMaster.filter(d => d.ProgramID == pd)
      if (programExisted.length > 0 && this.isEdit != true) {
        this.isDisabled = true
        this.ComSrv.ShowError('The process is already associated with the selected program. If you want to add an extension, please modify the record.')
      } else {
        this.isDisabled = false
      }
      const sProgramData = this.programData.filter(p => p.ProgramID == pd)
      if (sProgramData.length > 0) {
        this.ProcessScheduleForm.get('ProgramStartDate').setValue(sProgramData[0].TentativeProcessStart)
      }
    });
    this.AddProcessScheduleDetails()
  }
  GetData() {
    this.ComSrv.postJSON("api/ProcessConfiguration/LoadData", { UserID: this.currentUser.UserID }).subscribe(
      (response) => {
        this.GetDataObject = response
        this.processScheduleMaster = this.GetDataObject.processScheduleMaster
        this.processScheduleDetail = this.GetDataObject.processScheduleDetail
        if (this.processScheduleMaster.length > 0) {
          this.ProcessScheduleMasterID = this.GetDataObject.processScheduleMaster[0].ProcessScheduleMasterID
          // this.Edit(this.ProcessScheduleMasterID)
          this.LoadMatTable(this.GetDataObject.processScheduleMaster, "ProcessSchedule");
        }
      },
      (error) => {
        this.ComSrv.ShowError(`${error.message}`, "Close", 500000);
      }
    );
  }
  GetProcessDays(rowIndex: any) {
    this.GetProcessDate(rowIndex)
  }
  GetProcessDate(rowIndex: any) {
    const { ProcessStartDate, ProcessEndDate, ProcessDays } = this.processDetails.value[rowIndex];
    const pStartDate = new Date(ProcessStartDate);
    const pEndDate = new Date(ProcessEndDate);
    const differenceMs: number = pEndDate.getTime() - pStartDate.getTime();
    const daysDifference: number = Math.ceil(differenceMs / (1000 * 60 * 60 * 24));
    const endDate = new Date(pStartDate);
    endDate.setDate(pStartDate.getDate() - 1 + parseInt(ProcessDays));
    this.processDetails.controls[rowIndex].get("ProcessEndDate").setValue(endDate);
  }
  GetProcessID(ProcessID: any, rowIndex: any) {
    if (ProcessID) {
      const selectedProcess = this.processDetails.value.map(d => d.ProcessID)
      const ProcessData = this.ProcessScheduleData.filter(d => d.ProcessID == ProcessID)
      this.processDetails.controls[rowIndex].get("ProcessDays").setValue(ProcessData[0].DurationDays);
      this.processDetails.controls[rowIndex].get("ProcessStartDate").setValue('');
      this.processDetails.controls[rowIndex].get("ProcessEndDate").setValue('');
    }
    //  this.ProcessScheduleData=this.ProcessScheduleData.filter(d=>!selectedProcess.includes(d.ProcessID)) 
  }
  CreateProcessDate(pd: any) {
    const ProcessScheduleDetails = this.ProcessScheduleForm.get("processDetails") as FormArray;
    ProcessScheduleDetails.controls.forEach((control: FormGroup, index: number) => {
      const processDays = parseInt(control.get('ProcessDays').value);
      if (index == 0) {
        const startDate = new Date(pd);
        const endDate = new Date(startDate);
        endDate.setDate(startDate.getDate() - 1 + processDays);
        control.controls.ProcessStartDate.setValue(startDate);
        control.controls.ProcessEndDate.setValue(endDate);
      } else {
        const preProcessEndDate = new Date(this.processDetails.controls[index - 1].get('ProcessEndDate').value);
        const startDate = new Date(preProcessEndDate);
        const endDate = new Date(startDate);
        startDate.setDate(preProcessEndDate.getDate() + 1)
        endDate.setDate(startDate.getDate() + processDays);
        control.controls.ProcessStartDate.setValue(startDate);
        control.controls.ProcessEndDate.setValue(endDate);
      }
    });
  }
  async GetActiveProgram() {
    this.SPName = "RD_SSPProgramDesignSummary"
    this.paramObject = {}
    this.programData = await this.FetchData(this.SPName, this.paramObject)
    console.log(this.programData)
  }
  async GetProcessSchedule() {
    this.SPName = "RD_SSPProcessSchedule"
    this.paramObject = {}
    this.ProcessScheduleData = await this.FetchData(this.SPName, this.paramObject)
    console.log(this.ProcessScheduleData)
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
  Save() {
    //  const processDetails= this.ProcessScheduleForm.get('processDetails').value
    //   if (this.ProcessScheduleForm.get('ProcessDays').value != this.ProcessScheduleForm.get('TotalDays').value) {
    //     this.ComSrv.ShowError("Total days must be equal to Process days.");
    //     return
    //   }
    // const FormData={...processDetails,
    //   ProgramID:this.ProcessScheduleForm.get('ProgramID').value,
    //   ProgramStartDate:this.ProcessScheduleForm.get('ProgramStartDate').value,
    //   TotalDays:this.ProcessScheduleForm.get('TotalDays').value
    //  }

    if(this.processDetails.value.length==2){
      const checkProcessIsExisted=this.processDetails.value[0].ProcessID==this.processDetails.value[1].ProcessID
      if (checkProcessIsExisted) {
       return this.ComSrv.ShowError('The process is already associated with the selected program.please choose other process from list.')
      }
    }

    if (this.processDetails.length == 0 || this.processDetails.length<2){
       return  this.ComSrv.ShowError("Both trainee registration and TSP verification are required to proceed");
    }

    if (this.processDetails.length > 0) {
      if (this.ProcessScheduleForm.valid && this.processDetails.valid) {
        this.ComSrv.postJSON("api/ProcessConfiguration/Save", this.ProcessScheduleForm.value).subscribe(
          (response) => {
            this.ComSrv.openSnackBar("Record saved successfully.");
            this.isEdit=false
            // console.log(this.removedTaskIds)
            // for (let index = 0; index < this.removedTaskIds.length; index++) {
            //     this.DeleteProcessScheduleDetail(this.removedTaskIds[index])
            // }
            this.GetActiveProgram()
            this.InitProcessScheduleForm()
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
  AddProcessScheduleDetails() {
    this.ProcessScheduleDetails = this.ProcessScheduleForm.get("processDetails") as FormArray;
    this.ProcessScheduleDetails.push(this.RowGenerator());
  }
  Date = new Date()
  currentDate = new Date();
  RowGenerator() {
    const fb = new FormBuilder();
    return fb.group({
      ProcessScheduleDetailID: [0],
      UserID: [this.currentUser.UserID],
      ProcessID: ['', [Validators.required]],
      ProcessStartDate: ['', [Validators.required]],
      ProcessEndDate: ['', [Validators.required]],
      ProcessDays: ['', [Validators.required]],
      IsLocked: ['0', [Validators.required]],
    });
  }
  get processDetails() {
    return this.ProcessScheduleForm.get("processDetails") as FormArray;
  }
  removedTaskIds: any = []
  RemoveDetail(index: any, row: FormArray) {
    if (confirm('Do you want to remove this details?')) {
      if (row.value[index].TaskID > 0) {
        this.removedTaskIds.push(row.value[index])
      }
      this.ProcessScheduleDetails = this.ProcessScheduleForm.get("processDetails") as FormArray;
      this.ProcessScheduleDetails.removeAt(index)
    }
  }
  async Edit(ProcessScheduleMasterID) {
   await this.GetActiveProgram()
    this.isEdit = true
    this.savebtn = "...  ";
    const processScheduleMaster = this.GetDataObject.processScheduleMaster.filter(d => d.ProcessScheduleMasterID == ProcessScheduleMasterID)
    const processScheduleDetail = this.GetDataObject.processScheduleDetail.filter(d => d.ProcessScheduleMasterID == ProcessScheduleMasterID)
    this.programData = this.programData.filter(p => p.ProgramID == processScheduleMaster[0].ProgramID)
   
    if (processScheduleMaster[0].InActive == true) {
      this.ProcessScheduleForm.get("InActive").setValue(true)
      this.ProcessScheduleForm.disable();
    }
    else {
      this.ProcessScheduleForm.get("InActive").setValue(false)
      this.ProcessScheduleForm.enable();
    }
    this.ProcessScheduleForm.patchValue(processScheduleMaster[0]);
    this.PopulateTaskDetail(processScheduleDetail)
  }
  PopulateTaskDetail(data) {
    this.processDetails.clear()
    this.ProcessScheduleEditRecord = data
    data.forEach(detail => {
      this.processDetails.push(this.fb.group(detail));
    });
    setTimeout(() => {
      this.savebtn = "Save ";
    }, 1000);
  }
  DeleteProcessScheduleDetail(row) {
    if (row.TaskID > 0) {
      this.ComSrv.postJSON("api/ProcessSchedule/removeTaskDetail", row).subscribe(
        (response) => {
          this.ComSrv.openSnackBar("ProcessSchedule's task has been removed.");
        },
        (error) => {
          this.ComSrv.ShowError(`${error.message}`, "Close", 500000);
        }
      );
    }
  }
  EmptyCtrl(ev: any) {
    this.BSearchCtr.setValue('');
    this.PSearchCtr.setValue('');
  }
  camelCaseToWords(input: string): string {
    return input.replace(/([a-z])([A-Z])/g, '$1 $2');
  }
  LoadMatTable(tableData: any, tableName: string) {
    switch (tableName) {
      case "ProcessSchedule":
        this.ProcessScheduleTableColumns = Object.keys(tableData[0]).filter(key =>
          !key.includes('ID')
        );
        this.ProcessScheduleTablesData = new MatTableDataSource(tableData);
        this.ProcessScheduleTablesData.paginator = this.ProcessSchedulePaginator;
        this.ProcessScheduleTablesData.sort = this.ProcessScheduleSort;
        break;
    }
  }
  ResetFrom() {
    this.isEdit=false
    this.GetActiveProgram()
    this.InitProcessScheduleForm()
    this.savebtn = "Save";
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
      if (this.ComSrv.getMessage().ProcessScheduleID != undefined) {
        this.Edit(this.ComSrv.getMessage().ProcessScheduleID)
      }
    }, 1000);
    if (this.tabGroup) {
      this.tabGroup.selectedTabChange.subscribe((event) => {
        this.TapIndex = event.index
      });
    }
  }
}
