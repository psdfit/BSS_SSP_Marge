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
@Component({
  selector: 'app-association-submission',
  templateUrl: './association-submission.component.html',
  styleUrls: ['./association-submission.component.scss']
})
export class AssociationSubmissionComponent implements OnInit {
  [x: string]: any;
  isChecked: boolean = false
  tspName: string;
  trainingLocation: any = [];
  constructor(
    private ComSrv: CommonSrvService,
    private ActiveRoute: ActivatedRoute,
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
  AssociationTablesData: MatTableDataSource<any>;
  @ViewChild("AssociationPaginator") AssociationPaginator: MatPaginator;
  @ViewChild("AssociationSort") AssociationSort: MatSort;
  AssociationTableColumns = [];
  selection = new SelectionModel<any>(true, []);
  TSPsArray: any[];
  tspUserIDsArray: any[];
  currentUser: any;
  TapIndex: any;
  selectedTradeLot: number = 0
  ngOnInit(): void {
    this.currentUser = this.ComSrv.getUserDetails();
    this.TapIndex = 0
    this.TablesData = new MatTableDataSource([]);
    this.AssociationTablesData = new MatTableDataSource([]);
    this.PageTitle();
    this.GetData();
    this.getTrainerDetail()
    this.GetTspTrade()
    this.InitAssociationForm()
  }
  PageTitle(): void {
    this.ComSrv.setTitle(this.ActiveRoute.snapshot.data.title);
    this.SpacerTitle = this.ActiveRoute.snapshot.data.title;
  }
  IsExistedTradeLot: boolean = false
  AssociationForm: FormGroup;
  LotNo: any = ""
  InitAssociationForm() {
    this.AssociationForm = this.fb.group({
      TspAssociationMasterID: [0],
      UserID: [this.currentUser.UserID],
      TSPName: [this.currentUser.FullName],
      TrainingLocation: ['', [Validators.required]],
      TradeLot: ['', [Validators.required]],
      TrainerDetailID: ['', [Validators.required]],
      TradeLotTitle: [''],
      ProgramID: [0],
      associationDetail: this.fb.array([])
    });
    this.AssociationForm.get('TrainingLocation').valueChanges.subscribe((Id) => {
      this.AssociationForm.get('TradeLot').setValue('')
      this.AssociationForm.get('TrainerDetailID').setValue('')
      this.TrainerProfile = []
      const TrainingDistrict = this.trainingLocation.filter(d => d.TrainingLocationID === Id);
      if (TrainingDistrict.length > 0) {
        this.GetTradeLot(TrainingDistrict[0].District, this.ProgramID);
      }
    });
    this.AssociationForm.get('TradeLot').valueChanges.subscribe(Id => {
      this.TrainerProfile = []
      this.AssociationForm.get('TrainerDetailID').setValue('')
      if (this.TradeLot != undefined) {
        const TradeLot = this.TradeLot.filter(d => d.TradeLotID == Id)
        const TSPAssociationSubmission: any[] = this.TSPAssociationSubmission.filter(d => d.TradeLot == Id && d.TradeLot != this.selectedTradeLot && d.TrainingLocation == this.AssociationForm.value.TrainingLocation)
        if (TSPAssociationSubmission.length > 0) {
          this.IsExistedTradeLot = true
        } else {
          this.IsExistedTradeLot = false
        }
        console.log(this.IsExistedTradeLot)
        if (TradeLot.length > 0) {
          this.LotNo = TradeLot[0].LotNo
          console.log(this.LotNo)
          this.updateTrainerData(TradeLot[0].TradeID)
        }
      }
    })
  }
  async updateTrainerData(TradeId: any) {
    this.TrainerProfile = this.TrainerDetailByUserID.filter(t => t.TrainerTradeID == TradeId)
    if (this.TrainerProfile.length == 0) {
      this.ComSrv.ShowError("system couldn't find a trainer for selected trade lot.")
    }
  }
  Save() {
    if (this.IsExistedTradeLot == true) {
      this.ComSrv.ShowError('TradeLot is already existed with selected location')
      return
    }
    if (this.associationDetail.length > 0) {
      if (this.AssociationForm.valid && this.associationDetail.valid) {
        this.AssociationForm.get("ProgramID").setValue(this.ProgramID)
        this.AssociationForm.get("TradeLotTitle").setValue(this.LotNo)
        this.ComSrv.postJSON("api/Association/SaveAssociationSubmission", this.AssociationForm.getRawValue()).subscribe(
          (response) => {
            this.savebtn = ' Save '
            this.ComSrv.openSnackBar("Record saved successfully.");
            this.InitAssociationDetail()
            this.TradeLot = []
            this.EditCheck = false
          },
          (error) => {
            this.ComSrv.ShowError(`${error.error}`, "Close", 5000);
          }
        );
      } else {
        this.ComSrv.ShowError("Required form fields are missing");
      }
    } else {
      this.ComSrv.ShowError("click to reset form.");
    }
  }
  RowGenerator() {
    return this.fb.group({
      TspAssociationDetailID: [0],
      TspAssociationMasterID: [0],
      UserID: [this.currentUser.UserID],
      CriteriaMainCategoryID: [""],
      CategoryTitle: ['', [Validators.required]],
      Evidence: ['', [Validators.required]],
      Remarks: ['', [Validators.required]],
    });
  }
  get associationDetail() {
    return this.AssociationForm.get("associationDetail") as FormArray;
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
  ResetFrom() {
    this.AssociationDetail(this.activeProgram)
    this.TradeLot = []
    this.savebtn = " Save ";
    this.AssociationForm.get('TradeLot').setValue('')
    this.EditCheck = false
  }
  CurrentDate: Date = new Date()
  FilterForm: FormGroup;
  ProgramStatus: string;
  activeProgram: any = {}
  AssociationDetail(row: any) {
    if (row.ProgramStatus != 'Active') {
      this.ComSrv.ShowError('The association submission is locked because the selected program is closed.')
    }
    this.activeProgram = {}
    this.activeProgram = row
    this.ProgramStatus = row.ProgramStatus
    this.CriteriaID = row.CriteriaID
    this.ProgramID = row.ProgramID
    this.tabGroup.selectedIndex = 1;
    this.isChecked = true
    this.InitAssociationDetail()
  }
  EditCheck: any = false;
  InitAssociationDetail() {
    this.GetTrainingLocation()
    this.InitAssociationForm()
    this.GetCriteriaMainCategory(this.CriteriaID)
    this.GetCriteriaSubCategory(this.CriteriaID)
    this.GetAssociationSubmission()
  }
  GetData() {
    this.GetActiveProgram()
  }
  TSPAssociationSubmission: any = []
  async GetAssociationSubmission() {
    this.AssociationTablesData = new MatTableDataSource([])
    this.l
    this.ComSrv.setMessage({ ProgramID: this.ProgramID })
    this.SPName = "RD_SSPTSPAssociationSubmission"
    this.paramObject = {
      UserID: this.currentUser.UserID,
      ProgramID: this.ProgramID
    }
    this.TSPAssociationSubmission = await this.FetchData(this.SPName, this.paramObject)
    if (this.TSPAssociationSubmission != undefined) {
      this.LoadMatTable(this.TSPAssociationSubmission, 'AssociationSubmission')
    }
  }
  CriteriaMainCategory: any = []
  async GetCriteriaMainCategory(CriteriaHeaderID) {
    this.SPName = "RD_SSPCriteriaMainCategory"
    this.paramObject = {
      CriteriaHeaderID: CriteriaHeaderID
    }
    this.CriteriaMainCategory = await this.FetchData(this.SPName, this.paramObject)
    this.PopulateAssociationDetail(this.CriteriaMainCategory)
  }
  PopulateAssociationDetail(data) {
    this.associationDetail.clear()
    data.forEach(detail => {
      this.associationDetail.push(this.fb.group(detail));
    });
    this.cdr.detectChanges()
  }
  CriteriaSubCategory: any = []
  async GetCriteriaSubCategory(CriteriaHeaderID) {
    this.SPName = "RD_SSPCriteriaSubCategory"
    this.paramObject = {
      CriteriaHeaderID: CriteriaHeaderID
    }
    this.CriteriaSubCategory = await this.FetchData(this.SPName, this.paramObject)
    this.PopulateAssociationDetail(this.CriteriaMainCategory)
  }
  async GetActiveProgram() {
    this.SPName = "RD_SSPActiveProgram"
    const programData = await this.FetchData(this.SPName, this.paramObject)
    this.LoadMatTable(programData, 'ProgramData')
  }
  AssociationData: any = []
  async TSPAssociationDetail(TspAssociationMasterID) {
    this.SPName = "RD_SSPTSPAssociationDetail"
    this.paramObject = {
      TspAssociationMaster: TspAssociationMasterID
    }
    this.AssociationData = await this.FetchData(this.SPName, this.paramObject)
  }
  async GetTrainingLocation() {
    this.SPName = "RD_SSPTSPTrainingLocation"
    this.paramObject = {
      UserID: this.currentUser.UserID
    }
    this.trainingLocation = await this.FetchData(this.SPName, this.paramObject)
  }
  TspTrade: any = []
  async GetTspTrade() {
    this.SPName = "RD_SSPTSPTradeManage"
    this.paramObject = {
      UserID: this.currentUser.UserID
    }
    this.TspTrade = await this.FetchData(this.SPName, this.paramObject)
  }
  TrainerDetailByUserID: any = []
  async getTrainerDetail() {
    this.SPName = "RD_SSPTrainerProfileDetail"
    this.paramObject = {
      UserID: this.currentUser.UserID,
      TrainerProfileID: 0
    }
    this.TrainerDetailByUserID = []
    this.TrainerDetailByUserID = await this.FetchData(this.SPName, this.paramObject)
  }
  async GetTradeLot(DistrictID, ProgramID) {
    this.SPName = "RD_SSPProgramLot";
    this.paramObject = {
      ProgramID: ProgramID,
      DistrictID: DistrictID,
      UserID: this.currentUser.UserID,
      TrainingLocationID: this.AssociationForm.get('TrainingLocation').value,
      IsEdit: this.EditCheck
    };
    try {
      this.TradeLot = await this.FetchData(this.SPName, this.paramObject);
      if (this.TradeLot.length > 0) {
        this.updateTradeLotStatus(this.TradeLot);
      } else {
        // Handle case where TradeLot array is empty
        this.ComSrv.ShowError('No Trade Lots found for the specified criteria.');
      }
    } catch (error) {
      this.ComSrv.ShowError('Error fetching Trade Lots:', error);
    }
  }
  updateTradeLotStatus(TradeLot) {
    const updatedTradeLot = TradeLot.map(item => {
      const matchedTrade = this.TspTrade.find(trade => trade.TradeID === item.TradeID && trade.ApprovalStatus === "Accepted" && trade.TrainingLocationID === this.AssociationForm.get('TrainingLocation').value);
      const tradeLotDisabled = matchedTrade ? false : true;
      return {
        ...item,
        tradeLotDisabled: tradeLotDisabled
      };
    });
    this.TradeLot = updatedTradeLot;
    const selectedTradeLot = this.AssociationForm.get('TradeLot').value;
    const selectedTradeLotData = TradeLot.find(d => d.TradeLotID === selectedTradeLot);
    if (selectedTradeLotData) {
      this.updateTrainerData(selectedTradeLotData.TradeID);
    }
  }
  TrainerProfile: any = []
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
      const data = await this.ComSrv.getJSON(`api/BSSReports/FetchReportData?Param=${Param}`).toPromise();
      if (data != undefined) {
        return data;
      } else {
        if (SPName != 'RD_SSPTSPAssociationSubmission') {
          this.ComSrv.ShowWarning(' No Record Found', 'Close');
        }
      }
    } catch (error) {
      this.error = error;
      this.ComSrv.ShowError(error.error)
    }
  }
  EmptyCtrl(ev: any) {
    this.TSearchCtr.setValue('');
  }
  camelCaseToWords(input: string): string {
    return input.replace(/([a-z])([A-Z])/g, '$1 $2');
  }
  LoadMatTable(tableData: any, ReportName: string) {
    this.cdr.detectChanges()
    if (tableData != undefined && tableData.length > 0) {
      if (ReportName == 'ProgramData') {
        const excludeColumnArray = [""]
        this.TableColumns = Object.keys(tableData[0])
          .filter(key => !key.includes('ID') && !excludeColumnArray.includes(key));
        this.TableColumns.unshift('Action')
        this.TablesData = new MatTableDataSource(tableData)
        this.TablesData.paginator = this.Paginator;
        this.TablesData.sort = this.Sort;
      } else {
        const excludeColumnArray = ["TrainingLocation", "IsChecked", "TotalNoOfClass", "NoOfClass", "TradeLot"]
        this.AssociationTableColumns = Object.keys(tableData[0])
          .filter(key => !key.includes('ID') && !excludeColumnArray.includes(key));
        this.AssociationTablesData = new MatTableDataSource(tableData)
        this.AssociationTablesData.paginator = this.AssociationPaginator;
        this.AssociationTablesData.sort = this.AssociationSort;
      }
    }
  }
  get mainCategories(): string[] {
    const categories = new Set<string>();
    this.associationDetail.controls.forEach((control: FormGroup) => {
      const categoryTitle = control.get('CategoryTitle').value;
      categories.add(categoryTitle);
    });
    return Array.from(categories);
  }
  getItemsByCategory(category: string): any[] {
    return this.associationDetail.controls.filter((control: FormGroup) => {
      return control.get('CategoryTitle').value === category;
    }).map((control: FormGroup) => control.value);
  }
  async Edit(row: any) {
    this.selectedTradeLot = row.TradeLot
    this.savebtn = ' Update '
    this.EditCheck = true
    this.AssociationForm.get('TrainingLocation').setValue(row.TrainingLocation)
    this.AssociationForm.get('TradeLot').setValue(row.TradeLot)
    this.AssociationForm.get('TspAssociationMasterID').setValue(row.TspAssociationMasterID)
    await this.TSPAssociationDetail(row.TspAssociationMasterID)
    await this.PopulateAssociationDetail(this.AssociationData)
    await this.AssociationForm.get('TrainerDetailID').setValue(row.TrainerDetailID)
  }
  isOptionDisabled(item: any): boolean {
    if (this.EditCheck) {
      const selectedValue = this.AssociationForm.get('TrainingLocation').value;
      const isOptionActive = selectedValue == item.TrainingLocationID;
      return !isOptionActive;
    }
  }
  OpenDialogue(row) {
    const data = [row, [1]];
    const dialogRef = this.Dialog.open(InitiateAssociationDialogComponent, {
      width: '40%',
      data: data,
      disableClose: true,
    });
    dialogRef.afterClosed().subscribe(result => {
      if (result === true) {
        this.GetData()
      }
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
  proceedAssociationPayment() {
    this.router.navigateByUrl('/payment/association-payment');
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
