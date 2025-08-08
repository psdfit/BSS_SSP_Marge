import { ChangeDetectorRef, Component, OnInit, ViewChild } from "@angular/core";
import { CommonSrvService } from "../../common-srv.service";
import { ActivatedRoute } from "@angular/router";
import { MatTableDataSource } from "@angular/material/table";
import { MatPaginator } from "@angular/material/paginator";
import { MatSort } from "@angular/material/sort";
import { MatTabGroup } from "@angular/material/tabs";
import { FormBuilder, FormControl, FormGroup, Validators } from "@angular/forms";

import { environment } from '../../../environments/environment';
@Component({
  selector: 'app-program-initiate',
  templateUrl: './program-initiate.component.html',
  styleUrls: ['./program-initiate.component.scss']
})
export class ProgramInitiateComponent implements OnInit {
  isChecked: boolean = true
  BSearchCtr = new FormControl('');

  // TapTitle: any;
  rowData: any;
  TapTitle: string;
  ResponseData: any = []
  tradeWiseTarget: any;
  lotWiseTarget: any;
  programDesign: any;
  programData: any;
  programId: string;
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
  TSearchCtr = new FormControl('');


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
  TapIndex=0
  ngOnInit(): void {
    this.currentUser = this.ComSrv.getUserDetails();
    this.TablesData = new MatTableDataSource([]);
    this.PageTitle();
    this.InitSchemeInfo()
    this.LoadData()
  }
  SchemeInfoForm: FormGroup;
  InitSchemeInfo() {
    this.SchemeInfoForm = this.fb.group({
      ProgramID: ['', Validators.required],
      TotalLots: ['', Validators.required],
      TotalProvince: ['', Validators.required],
      TotalCluster: ['', Validators.required],
      TotalDistrict: ['', Validators.required],
      TotalTrade: ['', Validators.required],
      ContractedTarget: ['', Validators.required],
      CompletionTarget: ['', Validators.required],
      TotalCost: ['', Validators.required],
    });
  }

  ShowDetail(row: any, TapTitle: string) {

    this.isChecked = true
    this.tradeTablesData = new MatTableDataSource([]);
    this.lotTablesData = new MatTableDataSource([]);
    this.rowData = []
    this.TapTitle = ""
    this.rowData = row
    this.TapTitle = TapTitle
    this.tabGroup.selectedIndex = 1;
    this.TapTitle = TapTitle + " Detail"
    // if (this.rowData.UserID) {
      console.log(this.rowData)
      console.log(this.rowData.ProgramID)
      this.tradeWiseTarget=this.GetDataObject.tradeWiseTarget.filter(d=>d.ProgramName==this.rowData.ProgramName)
      this.lotWiseTarget=this.GetDataObject.lotWiseTarget.filter(d=>d.ProgramID==this.rowData.ProgramID)
      this.LoadMatTable(this.tradeWiseTarget,"TradeDetail")
      this.LoadMatTable(this.lotWiseTarget,"TradeLotDetail")
    // } else {
    //   this.ComSrv.ShowError("Required form fields are missing");
    // }

  }

  LoadData() {
    this.ComSrv.postJSON("api/ProgramDesign/LoadSchemeData", { UserID: this.currentUser.UserID }).subscribe(
      (response) => {
        this.GetDataObject = response
        this.programDesign = this.GetDataObject.programDesignSummary.filter(p => p.IsInitiated == false && p.IsSubmitted == true);
        const program = this.GetDataObject.programDesignSummary.filter(p=> p.IsSubmitted == true);
        const ProgramTablesData = this.GetDataObject.programDesignSummary;
        this.LoadMatTable(program, "Program");

      },
      (error) => {
        this.ComSrv.ShowError(`${error.message}`, "Close", 500000);
      }
    );
  }
  selectedProgram: any = {}
  LoadProgramData(programId) {
    this.programData = this.GetDataObject.programDesignSummary.filter(p => p.ProgramID === programId && p.IsInitiated === false);

    console.log(this.programData[0])


    if(this.programData[0].ProgramDesignOn=='Province'){
      this.programData[0].TotalCluster=0
    }else if(this.programData[0].ProgramDesignOn=='Cluster'){
      this.programData[0].TotalDistrict=0
    }else{
      this.programData = this.GetDataObject.programDesignSummary.filter(p => p.ProgramID === programId && p.IsInitiated === false);
    }

    if (this.programData.length > 0) {
      this.SchemeInfoForm.patchValue(this.programData[0])
    }
  }
  EmptyCtrl() {
    this.BSearchCtr.setValue('');
  }
  IsDisabled=false

  Save() {
    if (this.SchemeInfoForm.valid) {
      this.IsDisabled=true

      this.ComSrv.postJSON("api/ProgramDesign/UpdateSchemeInitialization", this.SchemeInfoForm.getRawValue()).subscribe(
        (response) => {
          console.log(response)
          this.LoadData()
          this.ResetFrom()
          this.ComSrv.openSnackBar("Program Initialized for Approval.", "Close", 5000);
      this.IsDisabled=false

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
    this.SchemeInfoForm.reset()
    this.programId = ""
  }
  PageTitle(): void {
    this.ComSrv.setTitle(this.AcitveRoute.snapshot.data.title);
    this.SpacerTitle = this.AcitveRoute.snapshot.data.title;
  }
  LoadMatTable(tableData: any[], ReportName: string) {
    this.cdr.detectChanges()

    if(tableData.length>0){
      switch (ReportName) {
        case 'Program':
          const excludeColumnArray=["WorkflowRemarks","SSPWorkflow","ProcessStatus","IsSubmitted", "AssociationStartDate", "AssociationEndDate","IsWorkflowAttached","Workflow","IsCriteriaAttached", "Criteria","CriteriaRemarks", "StartDate", "EndDate", "StatusRemarks","IsFinalApproved"]
          this.TableColumns = Object.keys(tableData[0]).filter(key => !key.includes('ID') && ! excludeColumnArray.includes(key));
          this.TableColumns.unshift('Action');
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
          console.warn(`Unhandled dataType: ${ReportName}`);
          break;
      }
    }else{
      this.ComSrv.ShowError("No Record Found")
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
    console.log(data)
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
    if (this.tabGroup) {
      this.tabGroup.selectedTabChange.subscribe((event) => {

       console.log(event)
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
