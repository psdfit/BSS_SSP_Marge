import { filter } from 'rxjs/operators';
import { ChangeDetectorRef, Component, OnInit, ViewChild } from "@angular/core";
import { CommonSrvService } from "../../common-srv.service";
import { ActivatedRoute, Router } from "@angular/router";
import { MatTableDataSource } from "@angular/material/table";
import { MatPaginator } from "@angular/material/paginator";
import { MatSort } from "@angular/material/sort";
import { MatTabGroup } from "@angular/material/tabs";
import { FormBuilder, FormControl, FormGroup, Validators } from "@angular/forms";
import { MatDialog, MatDialogRef } from "@angular/material/dialog";
import { environment } from '../../../environments/environment';
import { TspStatusUpdateComponent } from "src/app/custom-components/tsp-status-update/tsp-status-update.component";
// import { DialogueService } from "src/app/shared/dialogue.service";
// import { DatePipe, TitleCasePipe, formatCurrency } from "@angular/common";
// import { GroupByPipe } from "angular-pipes";
import { SelectionModel } from '@angular/cdk/collections';
import { DialogueService } from '../../shared/dialogue.service';
import { EnumApprovalProcess, EnumProgramCategory } from '../../shared/Enumerations';

@Component({
  selector: 'app-validate-business-plan',
  templateUrl: './validate-business-plan.component.html',
  styleUrls: ['./validate-business-plan.component.scss']
})
export class ValidateBusinessPlanComponent implements OnInit {

  isChecked: boolean = false
  TspName: any;
  rowData: any;
  tspName: string;
  ResponseData: any = []
  tradeWiseTarget: any;
  lotWiseTarget: any;
  constructor(
    private ComSrv: CommonSrvService,
    private AcitveRoute: ActivatedRoute,
    private fb: FormBuilder,
    private Dialog: MatDialog,
    private dialogue: DialogueService,
    private cdr: ChangeDetectorRef

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

  tradeTablesData: MatTableDataSource<any>;
  @ViewChild("tPaginator") tPaginator: MatPaginator;
  @ViewChild("tSort") tSort: MatSort;
  tradeTableColumns = [];

  lotTablesData: MatTableDataSource<any>;
  @ViewChild("lPaginator") lPaginator: MatPaginator;
  @ViewChild("lSort") lSort: MatSort;
  lotTableColumns = [];


  selection = new SelectionModel<any>(true, []);
  TSPsArray: any[];
  tspUserIDsArray: any[];











  currentUser: any;
  TapIndex: any;
  ngOnInit(): void {
    this.currentUser = this.ComSrv.getUserDetails();
    this.TapIndex = 0
    this.TablesData = new MatTableDataSource([]);
    this.tradeTablesData = new MatTableDataSource([]);
    this.lotTablesData = new MatTableDataSource([]);
    this.PageTitle();
    this.GetData();
    this.InitFilterForm();
  }

  PageTitle(): void {
    this.ComSrv.setTitle(this.AcitveRoute.snapshot.data.title);
    this.SpacerTitle = this.AcitveRoute.snapshot.data.title;
  }

  CurrentDate: Date = new Date()
  FilterForm: FormGroup;
  InitFilterForm() {
    this.FilterForm = this.fb.group({
      UserID: [this.currentUser.UserID],
      StartDate: [this.CurrentDate, Validators.required],
      EndDate: [this.CurrentDate, Validators.required],
    });
  }


  ShowDetail(row: any, tspName: string) {
    this.rowData = []
    this.tspName = ""
    this.rowData = row
    this.tspName = tspName
    this.tabGroup.selectedIndex = 1;
    this.isChecked = true
    this.TspName = tspName + " Detail"
    // if (this.rowData.UserID) {

      this.tradeWiseTarget=this.GetDataObject.tradeWiseTarget.filter(d=>d.ProgramName==this.rowData.ProgramName)
      this.lotWiseTarget=this.GetDataObject.lotWiseTarget.filter(d=>d.ProgramID==this.rowData.ProgramID)
      this.LoadMatTable(this.tradeWiseTarget,"TradeDetail")
      this.LoadMatTable(this.lotWiseTarget,"TradeLotDetail")
    // } else {
    //   this.ComSrv.ShowError("Required form fields are missing");
    // }

  }



  GetData() {
    this.ComSrv.postJSON("api/ProgramDesign/LoadSchemeData", { UserID: this.currentUser.UserID }).subscribe(
      (response:any) => {
        this.GetDataObject=response

        this.LoadMatTable(response.programDesignSummary.filter(d=>d.IsInitiated ==true && d.IsFinalApproved ==false), "ProgramData");
      },
      (error) => {
        this.ComSrv.ShowError(`${error.message}`, "Close", 500000);
      }
    );
  }



  EmptyCtrl(ev: any) {
    this.TSearchCtr.setValue('');
  }

  camelCaseToWords(input: string): string {
    return input.replace(/([a-z])([A-Z])/g, '$1 $2');
  }

  LoadMatTable(tableData: any[], ReportName: string) {
    this.cdr.detectChanges()


    if(tableData.length>0){
      if(ReportName=='ProgramData') {
        const excludeColumnArray=["WorkflowRemarks","IsWorkflowAttached","Workflow","IsCriteriaAttached", "Criteria","CriteriaRemarks", "StartDate", "EndDate", "StatusRemarks","IsFinalApproved"]
        this.TableColumns = Object.keys(tableData[0])
        .filter(key => !key.includes('ID') && ! excludeColumnArray.includes(key));



          this.TableColumns.unshift('Action')
          this.TablesData = new MatTableDataSource(tableData)
          this.TablesData.paginator = this.Paginator;
          this.TablesData.sort = this.Sort;
        }
       if(ReportName=='TradeDetail') {
        console.log(ReportName)
          this.tradeTableColumns = Object.keys(tableData[0]).filter(key => !key.includes('ID'));
          this.tradeTablesData = new MatTableDataSource(tableData)
          this.tradeTablesData.paginator = this.tPaginator;
          this.tradeTablesData.sort = this.tSort;
        }
       if(ReportName=='TradeLotDetail') {
          this.lotTableColumns = Object.keys(tableData[0]).filter(key => !key.includes('ID'));
          this.lotTablesData = new MatTableDataSource(tableData)
          this.lotTablesData.paginator = this.lPaginator;
          this.lotTablesData.sort = this.lSort;
        }
    }else{
      this.ComSrv.ShowError("No Record Found")
    }





  }

  openApprovalDialogue(row: any): void {
    let processKey = EnumApprovalProcess.REG_EVAL;

    this.dialogue.openApprovalDialogue(processKey, row.ProgramID).subscribe(result => {
      console.log(result);
      //location.reload();
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
