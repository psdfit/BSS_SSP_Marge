import { filter } from 'rxjs/operators';
import { ChangeDetectorRef, Component, OnInit, ViewChild } from "@angular/core";
import { CommonSrvService } from "../../common-srv.service";
import { ActivatedRoute, Router } from "@angular/router";
import { MatTableDataSource } from "@angular/material/table";
import { MatPaginator } from "@angular/material/paginator";
import { MatSort } from "@angular/material/sort";
import { MatTabGroup } from "@angular/material/tabs";
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from "@angular/forms";
import { MatDialog, MatDialogRef } from "@angular/material/dialog";
import { environment } from '../../../environments/environment';
import { SelectionModel } from '@angular/cdk/collections';
import { InitiateAssociationDialogComponent } from "src/app/custom-components/initiate-association-dialog/initiate-association-dialog.component";
import { TspAssociationEvaluationDialogueComponent } from 'src/app/custom-components/tsp-association-evaluation-dialogue/tsp-association-evaluation-dialogue.component';
@Component({
  selector: 'app-association-evaluation',
  templateUrl: './association-evaluation.component.html',
  styleUrls: ['./association-evaluation.component.scss']
})
export class AssociationEvaluationComponent implements OnInit {
  [x: string]: any;
  isChecked: boolean = false
  TspName: any;
  rowData: any;
  tspName: string;
  ResponseData: any = []
  tradeWiseTarget: any;
  lotWiseTarget: any;
  trainingLocation: any = [];
  constructor(
    private ComSrv: CommonSrvService,
    private AcitveRoute: ActivatedRoute,
    private fb: FormBuilder,
    private Dialog: MatDialog,
    private cdr: ChangeDetectorRef,
    private router: Router
  ) { }
  environment = environment;
  error: any;
  GetDataObject: any = {}
  SpacerTitle: string;
  TSearchCtr = new FormControl('');
  TapTTitle: string = "Profile"
  @ViewChild("tabGroup") tabGroup: MatTabGroup;
  TablesData: MatTableDataSource<any>;
  @ViewChild("Paginator") Paginator: MatPaginator;
  @ViewChild("Sort") Sort: MatSort;
  TableColumns = [];
  hTablesData: MatTableDataSource<any>;
  @ViewChild("hpaginator") hpaginator: MatPaginator;
  @ViewChild("hsort") hsort: MatSort;
  hTableColumns = [];
  selection = new SelectionModel<any>(true, []);
  TSPsArray: any[];
  tspUserIDsArray: any[];
  currentUser: any;
  TapIndex: any;
  ngOnInit(): void {
    this.currentUser = this.ComSrv.getUserDetails();
    this.TapIndex = 0
    this.TablesData = new MatTableDataSource([]);
    this.hTablesData = new MatTableDataSource([]);
    this.PageTitle();
    this.GetData();
    // this.InitFilterForm();
    this.InitAssociationForm()
  }
  PageTitle(): void {
    this.ComSrv.setTitle(this.AcitveRoute.snapshot.data.title);
    this.SpacerTitle = this.AcitveRoute.snapshot.data.title;
  }
  AssociationForm: FormGroup;
  WorkflowDetails: FormArray;
  LotNo: any = ""
  ProgramID: any;
  InitAssociationForm() {
    this.AssociationForm = this.fb.group({
      TspAssociationMasterID: [0],
      UserID: [this.currentUser.UserID],
      TrainingLocation: ['', [Validators.required]],
      TradeLot: ['', [Validators.required]],
      TradeLotTitle: [this.LotNo],
      ProgramID: [this.ProgramID],
      associationDetail: this.fb.array([])
    });
    this.AssociationForm.get('TrainingLocation').valueChanges.subscribe(Id => {
      const TrainingDistrict = this.trainingLocation.filter(d => d.TrainingLocationID == Id)
      this.GetTradeLot(TrainingDistrict[0].District, this.ProgramID)
    })
    this.AssociationForm.get('TradeLot').valueChanges.subscribe(Id => {
      const TradeLot = this.TradeLot.filter(d => d.TradeLotID == Id)
      this.LotNo = TradeLot[0].LotNo
    })
  }
  LoadDetail() {
    this.PopulateTaskDetail(this.CriteriaMainCategory)
  }
  removedTaskIds: any = []
  RemoveDetail(index: any, row: FormArray) {
    if (confirm('Do you want to remove this details?')) {
      if (row.value[index].TaskID > 0) {
        this.removedTaskIds.push(row.value[index])
      }
      this.WorkflowDetails = this.AssociationForm.get("associationDetail") as FormArray;
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
  ShowSubCatgoryDetail() {
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
  savebtn: string = "Save "
  CurrentDate: Date = new Date()
  FilterForm: FormGroup;
  SelectedRow: any;
  CheckAssociation: any = true
  ShowDetail(row: any) {
    this.hTablesData = new MatTableDataSource([])
    this.SelectedRow = row;
    this.CheckAssociation = new Date(row.AssociationEndDate) < new Date()
    this.rowData = []
    this.tspName = ""
    this.rowData = row
    this.tabGroup.selectedIndex = 1;
    this.isChecked = true
    this.TspName = "Association"
    this.ProgramID = this.rowData.ProgramID
    setTimeout(() => {
      this.GetTspAssociationEvaluation()
    }, 100);
  }
  GetData() {
    this.GetActiveProgram()
  }
  TspAssociationDetail: any = []
  async GetTspAssociationDetail(TspAssociationMaterID) {
    this.SPName = "RD_SSPTspAssociationDetail"
    this.paramObject = {
      TspAssociationMaster: TspAssociationMaterID
    }
    this.TspAssociationDetail = await this.FetchData(this.SPName, this.paramObject)
  }
  AssociationEvaluationData: any = []
  AssociationEvaluationTrailData: any = []
  async GetTspAssociationEvaluation() {
    this.SPName = "RD_SSPTspAssociationEvaluation"
    this.paramObject = {
      ProgramID: this.ProgramID,
      TradeID: 0
    }
    this.AssociationEvaluationData = await this.FetchData(this.SPName, this.paramObject)
    if (this.AssociationEvaluationData == undefined) {
      this.ComSrv.ShowError("No Record Found ")
      return;
    }
    this.AssociationEvaluationTrailData = this.AssociationEvaluationData.filter(d => d.ProgramID == this.ProgramID)
    this.AssociationEvaluationData = this.AssociationEvaluationData.filter(d => d.ProgramID == this.ProgramID && d.InActive == false)
    if (this.AssociationEvaluationData.length > 0) {
      this.LoadMatTable(this.AssociationEvaluationData, 'AssociationEvaluation')
    } else {
      this.hTablesData = new MatTableDataSource([])
      this.ComSrv.ShowError("No Record Found ")
    }
  }
  CriteriaMainCategory: any = []
  async GetCriteriaMainCategory(CriteriaHeaderID) {
    this.SPName = "RD_SSPCriteriaMainCategory"
    this.paramObject = {
      CriteriaHeaderID: CriteriaHeaderID
    }
    this.CriteriaMainCategory = await this.FetchData(this.SPName, this.paramObject)
    this.PopulateTaskDetail(this.CriteriaMainCategory)
  }
  async GetActiveProgram() {
    this.SPName = "RD_SSPActiveProgram"
    this.paramObject = {}
    const programData = await this.FetchData(this.SPName, this.paramObject)
    this.LoadMatTable(programData, 'ProgramData')
  }
  async GetTrainingLocation() {
    this.SPName = "RD_SSPTspTrainingLocation"
    this.paramObject = {
      UserID: this.currentUser.UserID
    }
    this.trainingLocation = await this.FetchData(this.SPName, this.paramObject)
  }
  async GetTradeLot(DistrictID, ProgramID) {
    this.SPName = "RD_SSPProgramLot"
    this.paramObject = {
      ProgramID: ProgramID,
      DistrictID: DistrictID,
    }
    this.TradeLot = []
    this.TradeLot = await this.FetchData(this.SPName, this.paramObject)
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
  ViewWorkflow(row) {
    if (row.WorkflowID != 0) {
      this.ComSrv.setMessage(row)
      this.router.navigateByUrl('/workflow-management/workflow');
    }
  }
  ViewCriteria(row) {
    if (row.CriteriaID != 0) {
      this.ComSrv.setMessage(row)
      this.router.navigateByUrl('/criteria-management/criteria-template');
    }
  }
  EmptyCtrl(ev: any) {
    this.TSearchCtr.setValue('');
  }
  camelCaseToWords(input: string): string {
    return input.replace(/([a-z])([A-Z])/g, '$1 $2');
  }
  LoadMatTable(tableData: any, tableName: string) {
    this.cdr.detectChanges()
    if (tableName == 'ProgramData') {
      const excludeColumnArray = [""]
      this.TableColumns = Object.keys(tableData[0]).filter(key => !key.includes('ID') && !excludeColumnArray.includes(key));
      this.TableColumns.unshift('Action')
      this.TablesData = new MatTableDataSource(tableData)
      this.TablesData.paginator = this.Paginator;
      this.TablesData.sort = this.Sort;
    }
    if (tableName == 'AssociationEvaluation') {
      const excludeColumnArray = ["Status"]
      this.hTableColumns = Object.keys(tableData[0]).filter(key => !key.includes('ID') && !excludeColumnArray.includes(key));
      this.hTableColumns.unshift('Action')
      this.hTablesData = new MatTableDataSource(tableData)
      this.hTablesData.paginator = this.hpaginator;
      this.hTablesData.sort = this.hsort;
    }
  }
  async OpenDialogue(row) {

    await this.GetTspAssociationDetail(row.TspAssociationMasterID)
    const EvaluationTrail = this.AssociationEvaluationTrailData.filter(d => d.TspAssociationMasterID == row.TspAssociationMasterID)
    const data = [row, this.TspAssociationDetail, EvaluationTrail];
    const dialogRef = this.Dialog.open(TspAssociationEvaluationDialogueComponent, {
      width: '85%',
      // height: '90%',
      data: data,
      disableClose: true,
    });
    dialogRef.afterClosed().subscribe(result => {
      this.tabGroup.selectedIndex = 0
      this.ShowDetail(this.SelectedRow)
    });
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
  ShowPreview(fileName: string) {
    this.ComSrv.PreviewDocument(fileName)
  }
  ngAfterViewInit() {
    if (this.tabGroup) {
      this.tabGroup.selectedTabChange.subscribe((event) => {
        this.TapIndex = event.index
        if (this.TapIndex == 0) {
          this.isChecked = false
        }
      });
    }
  }
}
