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
// import { DialogueService } from "src/app/shared/dialogue.service";
// import { DatePipe, TitleCasePipe, formatCurrency } from "@angular/common";
// import { GroupByPipe } from "angular-pipes";
import { SelectionModel } from '@angular/cdk/collections';
import { DialogueService } from '../../shared/dialogue.service';
import { ProcessApprovedPlanDialogComponent } from "src/app/custom-components/process-approved-plan-dialog/process-approved-plan-dialog.component";


@Component({
  selector: 'app-process-approved-plan',
  templateUrl: './process-approved-plan.component.html',
  styleUrls: ['./process-approved-plan.component.scss']
})
export class ProcessApprovedPlanComponent implements OnInit {

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
    // this.InitFilterForm();
  }

  PageTitle(): void {
    this.ComSrv.setTitle(this.AcitveRoute.snapshot.data.title);
    this.SpacerTitle = this.AcitveRoute.snapshot.data.title;
  }

  CurrentDate: Date = new Date()
  FilterForm: FormGroup;



  ShowDetail(row: any, tspName: string) {
    this.rowData = []
    this.tspName = ""
    this.rowData = row
    this.tspName = tspName
    this.tabGroup.selectedIndex = 1;
    this.isChecked = true
    this.TspName = tspName + " Detail"
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



  GetData() {
    this.ComSrv.postJSON("api/ProgramDesign/LoadSchemeData", { UserID: this.currentUser.UserID }).subscribe(
      (response:any) => {
        this.GetDataObject=response

        // this.LoadMatTable(response.programDesignSummary, "ProgramData");
        this.LoadMatTable(response.programDesignSummary.filter(d=>d.IsFinalApproved ==true ), "ProgramData");
      },
      (error) => {
        this.ComSrv.ShowError(`${error.message}`, "Close", 500000);
      }
    );
  }

  ViewWorkflow(row){
    if(row.WorkflowID!=0){
      this.ComSrv.setMessage(row)
      this.router.navigateByUrl('/workflow-management/workflow');
    }

  }


  EmptyCtrl(ev: any) {
    this.TSearchCtr.setValue('');
  }

  camelCaseToWords(input: string): string {
    return input.replace(/([a-z])([A-Z])/g, '$1 $2');
  }

  LoadMatTable(tableData: any[], tableName: string) {
    this.cdr.detectChanges()

    if(tableData.length>0){
      if(tableName=='ProgramData') {
        const excludeColumnArray=["WorkflowRemarks","IsSubmitted","AssociationStartDate","AssociationEndDate","ProcessStatus","IsCriteriaAttached", "Criteria","CriteriaRemarks", "StartDate", "EndDate", "StatusRemarks","IsFinalApproved"]
       this.TableColumns = Object.keys(tableData[0])
       .filter(key => !key.includes('ID') && ! excludeColumnArray.includes(key));

       this.TableColumns.unshift('Action')
       this.TablesData = new MatTableDataSource(tableData)
       this.TablesData.paginator = this.Paginator;
       this.TablesData.sort = this.Sort;
     }
      if(tableName=='TradeDetail') {
      console.log(tableName)
        this.tradeTableColumns = Object.keys(tableData[0]).filter(key => !key.includes('ID'));
        this.tradeTablesData = new MatTableDataSource(tableData)
        this.tradeTablesData.paginator = this.tPaginator;
        this.tradeTablesData.sort = this.tSort;
      }
      if(tableName=='TradeLotDetail') {
        this.lotTableColumns = Object.keys(tableData[0]).filter(key => !key.includes('ID'));
        this.lotTablesData = new MatTableDataSource(tableData)
        this.lotTablesData.paginator = this.lPaginator;
        this.lotTablesData.sort = this.lSort;
      }

    }else{
      this.ComSrv.ShowError("No Record Found")
    }



  }


  OpenDialogue(row) {
    const data = [row, [1]];

    const dialogRef = this.Dialog.open(ProcessApprovedPlanDialogComponent, {
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
