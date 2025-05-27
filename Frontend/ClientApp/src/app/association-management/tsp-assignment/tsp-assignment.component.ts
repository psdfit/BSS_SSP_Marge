import { error } from 'protractor';
import { filter } from 'rxjs/operators';
import { ChangeDetectorRef, Component, OnInit, ViewChild } from "@angular/core";
import { CommonSrvService } from "../../common-srv.service";
import { ActivatedRoute } from "@angular/router";
import { MatTableDataSource } from "@angular/material/table";
import { MatPaginator } from "@angular/material/paginator";
import { MatSort } from "@angular/material/sort";
import { MatTabGroup } from "@angular/material/tabs";
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from "@angular/forms";
import { environment } from '../../../environments/environment';
import { MatSelect } from '@angular/material/select';
import { MatOption } from '@angular/material/core';
@Component({
  selector: 'app-tsp-assignment',
  templateUrl: './tsp-assignment.component.html',
  styleUrls: ['./tsp-assignment.component.scss']
})
export class TspAssignmentComponent implements OnInit {
  isChecked: boolean = true
  BSearchCtr = new FormControl('');
  tSearchCtr = new FormControl('');
  TSearchCtr = new FormControl('');
  LSearchCtr = new FormControl('');
  TLSearchCtr = new FormControl('');
  matSelectArray: MatSelect[] = [];
  @ViewChild('TSPDropDown') TSPDropDown: MatSelect;
  SelectedAll_TSPDropDown: boolean = false
  SelectAll(event: any, dropDownNo, controlName, formGroup) {
    const matSelect = this.matSelectArray[(dropDownNo - 1)];
    if (event.checked) {
      matSelect.options.forEach((item: MatOption) => item.select());
      if (this[formGroup].get(controlName).value) {
        const uniqueArray = Array.from(new Set(this[formGroup].get(controlName).value));
        this[formGroup].get(controlName).setValue(uniqueArray)
      }
    } else {
      matSelect.options.forEach((item: MatOption) => item.deselect());
    }
  }
  optionClick(event, controlName) {
    this.EmptyCtrl()
    let newStatus = true;
    event.source.options.forEach((item: MatOption) => {
      if (!item.selected && !item.disabled) {
        newStatus = false;
      }
    });
    if (event.source.ngControl.name === controlName) {
      this['SelectedAll_' + controlName] = newStatus;
    } else {
      this['SelectedAll_' + controlName] = newStatus;
    }
  }
  TspName: any;
  rowData: any;
  tspName: string;
  ResponseData: any = []
  tradeWiseTarget: any;
  lotWiseTarget: any;
  programDesign: any;
  programData: any;
  programId: string;
  Province: any[];
  Cluster: any[];
  District: any[];
  programProvince: any[] = []
  programCluster: any[] = []
  programDistrict: any[] = []
  constructor(
    private ComSrv: CommonSrvService,
    private AcitveRoute: ActivatedRoute,
    private fb: FormBuilder,
    private cdr: ChangeDetectorRef
  ) { }
  environment = environment;
  error: any;
  GetDataObject: any = {}
  SpacerTitle: string;
  TapTTitle: string = "Profile"
  @ViewChild("tabGroup") tabGroup: MatTabGroup;
  TablesData: MatTableDataSource<any>;
  @ViewChild("Paginator") Paginator: MatPaginator;
  @ViewChild("Sort") Sort: MatSort;
  TableColumns = [];
  tradeTablesData: MatTableDataSource<any>;
  @ViewChild("tPaginator") tPaginator: MatPaginator;
  @ViewChild("tSort") tSort: MatSort;
  tradeTableColumns = [];
  lotTablesData: MatTableDataSource<any>;
  @ViewChild("lPaginator") lPaginator: MatPaginator;
  @ViewChild("lSort") lSort: MatSort;
  lotTableColumns = [];
  TSPsArray: any[];
  tspUserIDsArray: any[];
  currentUser: any;
  TapIndex = 0
  ngOnInit(): void {
    this.currentUser = this.ComSrv.getUserDetails();
    this.TablesData = new MatTableDataSource([]);
    this.PageTitle();
    this.InitSchemeInfo()
    this.InitTradeDetailInfo()
    this.InitTspAssignmentInfo()
    this.LoadData()
    this.GetProvince()
    this.GetCluster()
    this.GetDistrict()
  }
  SchemeDesignOnID = ""
  ProgramID = 0
  LocationID = 0
  TspAssignmentInfoForm: FormGroup;
  InitTspAssignmentInfo() {
    this.TspAssignmentInfoForm = this.fb.group({
      UserID: [this.currentUser.UserID],
      ProgramID: ['', Validators.required],
      TSPAssignmentID: [0],
      DistrictID: [''],
      ClusterID: [''],
      ProvinceID: [''],
      TspTrainingLocationID: [''],
      TspAssociationEvaluationID: ['',Validators.required],
      TradeLotID: ['', Validators.required],
      TSP: ['']
    });
    // console.log(this.programDistrict.length)
    this.TspAssignmentInfoForm.get("ProgramID").valueChanges.subscribe(program => {
      if(program !=null && program !=""){

      const selectedProgram = this.GetDataObject.programDesignSummary.filter(d => d.ProgramID == program);
      this.programProvince = []
      this.programCluster = []
      this.programDistrict = []
      this.Trade = []
      this.TSP = []
      this.SchemeDesignOnID = selectedProgram[0].SchemeDesignOnID
      this.ProgramID = selectedProgram[0].ProgramID

      this.TspAssignmentInfoForm.get("DistrictID").setValue('')
      this.TspAssignmentInfoForm.get("ClusterID").setValue('')
      this.TspAssignmentInfoForm.get("ProvinceID").setValue('')
      this.TspAssignmentInfoForm.get("TradeLotID").setValue('')
      this.TspAssignmentInfoForm.get("TSP").setValue('')
      
      if (this.SchemeDesignOnID == "Province") {
        const selectedProvince = selectedProgram[0].ProvinceID.split(",").map(Number)
        this.programProvince = this.Province.filter(p => selectedProvince.includes(p.ProvinceID))
      }
      if (this.SchemeDesignOnID == "Cluster") {
        const selectedCluster = selectedProgram[0].ClusterID.split(",").map(Number)
        this.programCluster = this.Cluster.filter(p => selectedCluster.includes(p.ClusterID))
      }
      if (this.SchemeDesignOnID == "District") {
        const selectedDistrict = selectedProgram[0].DistrictID.split(",").map(Number)
        this.programDistrict = this.District.filter(p => selectedDistrict.includes(p.DistrictID))
      }
    }
    })

    this.TspAssignmentInfoForm.get("ProvinceID").valueChanges.subscribe(province => {
      if(province !='' && province !=null){
        this.Trade = []
        this.TSP = []
        this.LocationID = province
        this.GetTradeDetailByProgram(province)
      }
    })
    this.TspAssignmentInfoForm.get("ClusterID").valueChanges.subscribe(cluster => {
      if(cluster !='' && cluster !=null){
      this.Trade = []
      this.TSP = []
      this.LocationID = cluster
      this.GetTradeDetailByProgram(cluster)
      }
    })

    this.TspAssignmentInfoForm.get("DistrictID").valueChanges.subscribe(district => {
      if(district !='' && district !=null){
      this.Trade = []
      this.TSP = []
      this.LocationID = district
      this.GetTradeDetailByProgram(district)
      }
    })
    this.TspAssignmentInfoForm.get("TradeLotID").valueChanges.subscribe(tradeLotId => {
      // this.TspAssignmentInfoForm.get("TSP").setValue("")
      if(tradeLotId !='' && tradeLotId !=null){

      this.GetTSPDetailByProgram(tradeLotId)
      this.GetTradeDesignDetailByTradeLot(tradeLotId)
      }
    })
    this.TspAssignmentInfoForm.get("TspAssociationEvaluationID").valueChanges.subscribe(tspEvaluation => {
      if(tspEvaluation !='' && tspEvaluation !=null){

      const selectedTspTrainingLocationID = this.TSP.filter(t => tspEvaluation.includes(t.TspAssociationEvaluationID)).map(t => t.TrainingLocationID);
      this.TspAssignmentInfoForm.get("TspTrainingLocationID").setValue(selectedTspTrainingLocationID)

      const TSPID = this.TSP.filter(t => tspEvaluation.includes(t.TspAssociationEvaluationID)).map(t => t.TSPID);
      this.TspAssignmentInfoForm.get("TSP").setValue(TSPID)

      const selectedTspCapacitySum = this.TSP.filter(t => tspEvaluation.includes(t.TspAssociationEvaluationID)).reduce((acc, t) => acc + t.TSPCapacity, 0);
    
      this.TradeDetailInfoForm.get("TspCapacity").setValue(0)
      this.TradeDetailInfoForm.get("TspCapacity").setValue(selectedTspCapacitySum)
      }
      
    })
  }
  TradeDetailInfoForm: FormGroup;
  InitTradeDetailInfo() {
    this.TradeDetailInfoForm = this.fb.group({
      TspCapacity: ['', Validators.required],
      Duration: ['', Validators.required],
      TradeCode: ['', Validators.required],
      WeeklyTrainingHours: ['', Validators.required],
      CertificationAuthority: ['', Validators.required],
      SourceOfCurriculum: ['', Validators.required],
      TraineeQualification: ['', Validators.required],
      TrainerQualification: ['', Validators.required],
      ProgramFocus: ['', Validators.required],
      ContractedTarget: ['', Validators.required],
      CompletionTarget: ['', Validators.required],
      CTM: ['', Validators.required],
    });
  }
  InitSchemeInfo() {
    this.TspAssignmentInfoForm = this.fb.group({
      ProgramID: ['', Validators.required],
      TotalLots: ['', Validators.required],
      TotalCluster: ['', Validators.required],
      TotalDistrict: ['', Validators.required],
      ContractedTarget: ['', Validators.required],
      CompletionTarget: ['', Validators.required],
      TotalCost: ['', Validators.required],
    });
  }

  TradeLotDetail = [];
  async GetTradeDesignDetailByTradeLot(TradeLotID) {
    this.SPName = "RD_SSPTradeDesignByTradeLot"
    this.paramObject = {
      TradeLotID: TradeLotID
    }
    this.TradeLotDetail = []
    this.TradeLotDetail = await this.FetchData(this.SPName, this.paramObject)
    this.TradeDetailInfoForm.patchValue(this.TradeLotDetail[0])
  }
  TSP = [];
  async GetTSPDetailByProgram(TradeLotID) {
    this.SPName = "RD_SSPTSPByProgramDistrictAndTradeLot"
    this.paramObject = {
      ProgramDesignOn: this.SchemeDesignOnID,
      ProgramID: this.ProgramID,
      LocationID: this.LocationID,
      TradeLotID: TradeLotID
    }
    this.TSP = []
    this.TSP = await this.FetchData(this.SPName, this.paramObject)
  }
  Trade = [];
  async GetTradeDetailByProgram(LocationID) {
    this.SPName = "RD_SSPTradeByProgramAndDistrict"
    this.paramObject = {
      ProgramDesignOn: this.SchemeDesignOnID,
      ProgramID: this.ProgramID,
      LocationID: LocationID
    }
    this.Trade = []
    this.Trade = await this.FetchData(this.SPName, this.paramObject)
  }
  async GetProvince() {
    this.SPName = "RD_Province"
    this.Province = []
    this.Province = await this.FetchData(this.SPName, this.paramObject)
  }
  async GetCluster() {
    this.SPName = "RD_Cluster"
    this.Cluster = []
    this.Cluster = await this.FetchData(this.SPName, this.paramObject)
  }
  async GetDistrict() {
    this.SPName = "RD_District"
    this.District = []
    this.District = await this.FetchData(this.SPName, this.paramObject)
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
  async LoadData() {
    await this.ComSrv.postJSON("api/Association/LoadData", { UserID: this.currentUser.UserID }).subscribe(
      (response) => {
        this.GetDataObject = response
        this.programDesign = this.GetDataObject.programDesignSummary.filter(p => p.IsInitiated == true && p.IsFinalApproved == true);
        // const ProgramTablesData = this.GetDataObject.programDesignSummary;
        this.LoadMatTable(this.GetDataObject.TSPAssignment, "TSPAssignment");
      },
      (error) => {
        this.ComSrv.ShowError(`${error.message}`, "Close", 500000);
      }
    );
  }
  selectedProgram: any = {}
  LoadProgramData(programId) {
    this.programData = this.GetDataObject.programDesignSummary.filter(p => p.ProgramID === programId && p.IsInitiated === false);
    if (this.programData.length > 0) {
      this.TspAssignmentInfoForm.patchValue(this.programData[0])
    }
  }
  EmptyCtrl() {
    this.BSearchCtr.setValue('');
    this.tSearchCtr.setValue('');
    this.LSearchCtr.setValue('');
    this.TLSearchCtr.setValue('');
    this.TSearchCtr.setValue('');
  }
  Save() {
    // const errorKeys: string[] = Object.keys(this.TspAssignmentInfoForm.value);
    // const formError = [];
    
    // for (let index = 0; index < errorKeys.length; index++) {
    //   const key = errorKeys[index];
    //   const formControl = this.TspAssignmentInfoForm.get(key);
    
    //   if (formControl && formControl.errors != null) {
    //     // Determine error message based on the form control key
    //     let errorMessage = `${key} is required`; // Default error message
    
    //     // Customize error message based on key (e.g., replace "ID" with empty string)
    //     if (key.includes("ID")) {
    //       errorMessage = `${key.replace("ID", "")} is required`;
    //     }
    //     formError.push(errorMessage)
    //     // Populate formError object with error message
    //     formError[key] = errorMessage;
    //   }
    // }
    
    // // Log the formError object as JSON string
    // if (formError.length>0) {
    //   this.ComSrv.ShowError(formError.join(','));
    //   return;
    // }
    // console.log(JSON.stringify(formError));

  

    if (this.TspAssignmentInfoForm.valid) {
      this.ComSrv.postJSON("api/Association/SaveTSPAssignment", this.TspAssignmentInfoForm.getRawValue()).subscribe(
        (response) => {
          this.SelectedAll_TSPDropDown=false;
          this.TspAssignmentInfoForm.reset()
          this.LoadData()
          this.InitTspAssignmentInfo()
          this.ComSrv.openSnackBar("Saved data");
        },
        (error) => {
          this.ComSrv.ShowError(`${error.message}`, "Close", 500000);
        }
      )
    } else {
      this.ComSrv.ShowError("please enter valid data");
    }
  }
  ResetFrom() {
    this.TspAssignmentInfoForm.reset()
    this.InitTspAssignmentInfo()
    this.programId = ""
  }
  PageTitle(): void {
    this.ComSrv.setTitle(this.AcitveRoute.snapshot.data.title);
    this.SpacerTitle = this.AcitveRoute.snapshot.data.title;
  }
  LoadMatTable(tableData: any[], ReportName: string) {
    this.cdr.detectChanges()
    if (tableData.length > 0) {
      switch (ReportName) {
        case 'TSPAssignment':
          const excludeColumnArray = ["WorkflowRemarks", "ProcessStatus", "AssociationStartDate", "AssociationEndDate", "IsWorkflowAttached", "Workflow", "IsCriteriaAttached", "Criteria", "CriteriaRemarks", "StartDate", "EndDate", "StatusRemarks", "IsFinalApproved"]
          this.TableColumns = Object.keys(tableData[0]).filter(key => !key.includes('ID') && !excludeColumnArray.includes(key));
          // this.TableColumns.unshift('Action');
          this.TablesData = new MatTableDataSource(tableData);
          this.TablesData.paginator = this.Paginator;
          this.TablesData.sort = this.Sort;
          break;
        case 'TradeDetail':
          this.tradeTableColumns = Object.keys(tableData[0]).filter(key => !key.includes('ID'));
          this.tradeTablesData = new MatTableDataSource(tableData);
          this.tradeTablesData.paginator = this.tPaginator;
          this.tradeTablesData.sort = this.tSort;
          break;
        case 'TradeLotDetail':
          this.lotTableColumns = Object.keys(tableData[0]).filter(key => !key.includes('ID'));
          this.lotTablesData = new MatTableDataSource(tableData);
          this.lotTablesData.paginator = this.lPaginator;
          this.lotTablesData.sort = this.lSort;
          break;
        default:
          console.warn(`Unhandled ReportName: ${ReportName}`);
          break;
      }
    }
  }
  camelCaseToWords(input: string): string {
    return input.replace(/([a-z])([A-Z])/g, '$1 $2');
  }
  applyFilter(data: MatTableDataSource<any>, event: any) {
    data.filter = event.target.value.trim().toLowerCase();
    if (data.paginator) {
      data.paginator.firstPage();
    }
  }
  DataExcelExport(data: any, title) {
    this.ComSrv.ExcelExporWithForm(data, title);
  }
  getErrorMessage(errorKey: string, errorValue: any): string {
    const errorMessages = {
      required: 'This field is required.',
      minlength: `This field must be at least ${errorValue.requiredLength - 1} characters long.`,
      maxlength: `This field's text exceeds the specified maximum length.  (maxLength: ${errorValue.requiredLength} characters)`,
      email: 'Invalid email address.',
      pattern: 'This field is only required text',
      customError: errorValue
    };
    return errorMessages[errorKey];
  }
  ngAfterViewInit() {
    this.matSelectArray = [this.TSPDropDown];
    if (this.tabGroup) {
      this.tabGroup.selectedTabChange.subscribe((event) => {
        this.TapIndex = event.index
        if (this.TapIndex == 0) {
          this.isChecked = false
        }
        if (this.TapIndex == 1) {
          this.isChecked = true
        }
      });
    }
  }
}
