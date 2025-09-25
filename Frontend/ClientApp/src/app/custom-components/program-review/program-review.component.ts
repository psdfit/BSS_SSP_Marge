import { Component, Inject, OnInit, ViewChild } from "@angular/core";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";
import { MatDialog } from "@angular/material/dialog";
import { CommonSrvService } from "src/app/common-srv.service";
import {
  FormArray,
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from "@angular/forms";
import { ActivatedRoute } from "@angular/router";
import { MatTableDataSource } from "@angular/material/table";
import { MatPaginator } from "@angular/material/paginator";
import { MatSort } from "@angular/material/sort";
import { environment } from "../../../environments/environment";
import * as Highcharts from "highcharts";
import { P } from "@angular/cdk/keycodes";
import { parse } from "path";
import { MatMenuTrigger } from "@angular/material/menu";
@Component({
  selector: "app-program-review",
  templateUrl: "./program-review.component.html",
  styleUrls: ["./program-review.component.scss"],
})
export class ProgramReviewComponent implements OnInit {
  Status: any = [];
  check: boolean = false;
  programBudgetHead: any[] = [];
  keys: string[];
  constructor(
    private comSrv: CommonSrvService,
    private activeRoute: ActivatedRoute,
    private fb: FormBuilder,
    public ComSrv: CommonSrvService,
    public dialog: MatDialog,
    public dialogRef: MatDialogRef<ProgramReviewComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    // this.TablesData = new MatTableDataSource(this.programBudgetHead);
    this.GetAll();
    dialogRef.disableClose = false;
  }
  TablesData: MatTableDataSource<any>;
  @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: false }) sort: MatSort;
  dataObject: any = [];
  programSummaryData: any = {};
  programOverviewData: any = {};
  programBudgetData: any = {};
  programUsedBudgetSegregationData: any = {};
  tradeDesignData: any = [];
  tradeLotData: any = [];
  ngOnInit() {
    console.log(this.data);
    this.TablesData = new MatTableDataSource([]);
    this.currentUser = this.ComSrv.getUserDetails();
    this.setPageTitle();
    // this.GetAll();
    // this.renderBudgetUtilizationChart(500000, 200000);
  }
  GetDataObject: any = {};
  GetAll() {
    try {
      // Use the correct structure for accessing ProgramID
      // console.log(this.data.programData.ProgramID);
      // const response: any =  this.comSrv
      //   .postJSON(`api/ProgramDesign/GetAllProgramDataById`,  this.data.programData.ProgramID )
      //   .toPromise();
      this.ComSrv.postJSON(
        "api/ProgramDesign/GetAllProgramDataById",
        this.data.programData.ProgramID
      ).subscribe((response) => {
        this.GetDataObject=response;
        // this.programBudgetHead = response.programBudgetHead;
        this.LoadMatTable(this.GetDataObject.programBudgetHead);
        
        // Assign the arrays from the response to component properties
        this.programBudgetHead = this.GetDataObject.programBudgetHead;
        // this.TablesData = new MatTableDataSource(this.programBudgetHead);
        // if (this.programBudgetHead.length > 0) {
        //   this.keys = Object.keys(this.programBudgetHead[0]);
        // }
        this.programOverviewData = this.GetDataObject.Overview;
        // debugger;
        this.programBudgetData = this.GetDataObject.ProgramBudget;
        this.tradeDesignData = this.GetDataObject.TradeBudget;
        this.tradeLotData = this.GetDataObject.TradeLotBudget;
        this.renderBudgetUtilizationChart(
          this.programBudgetData[0].AllocatedBudget,
          this.programBudgetData[0].PlannedBudget
        );
      });
    } catch (error) {
      this.comSrv.ShowError("Failed to fetch program data.");
    }
  }
  TableColumns = [];
    LoadMatTable(tableData: any[]) {
    const excludeColumnArray = [];
    debugger;
    if (tableData.length > 0) {
      this.TableColumns = Object.keys(tableData[0]).filter(
        (key) => !key.includes("ID") && !excludeColumnArray.includes(key)
      );
      // this.TableColumns.unshift("Actions")
      this.TablesData = new MatTableDataSource(tableData);
      this.TablesData.paginator = this.paginator;
      this.TablesData.sort = this.sort;
    }
  }
  applyFilter(filterValue: string) {
    this.TablesData.filter = filterValue.trim().toLowerCase();
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
  paramObject: any = {};
  ExportReportName: string = "";
  SPName: string = "";
  FetchData(SPName: string, paramObject: any) {
    try {
      const Param = this.GetParamString(SPName, paramObject);
      const data: any = this.ComSrv.getJSON(
        `api/BSSReports/FetchReportData?Param=${Param}`
      ).toPromise();
      if (data.length > 0) {
        return data;
      } else {
        this.ComSrv.ShowWarning(" No Record Found", "Close");
      }
    } catch (error) {
      this.error = error;
    }
  }
  environment = environment;
  error: string | null = null;
  currentUser: any;
  saveBtn: "Save" | "Update" = "Save";
  spacerTitle: string;
  searchCtrl = new FormControl("");
  tapIndex = 0;
  tableColumns: string[] = [
    "Actions",
    "Project",
    "WorkflowTitle",
    "Status",
    "StartDate",
    "EndDate",
    "CompletedDate",
    "TotalDays",
    "PendingDays",
    "EfficiencyDays",
  ];
  // tablesData: MatTableDataSource<any>;
  workflowForm: FormGroup;
  // @ViewChild(MatTabGroup) tabGroup: MatTabGroup;
  //  @ViewChild(MatSort) sort: MatSort;
  // @ViewChild(MatPaginator) paginator: MatPaginator;
  private setPageTitle(): void {
    const title =
      this.activeRoute.snapshot.data.title || "Workflow Request Mapping";
    this.comSrv.setTitle(title);
    this.spacerTitle = title;
  }
  dataExcelExport(data: any[], title: string): void {
    this.comSrv.ExportToExcel(data, title);
  }
  camelCaseToWords(input: string): string {
    return input.replace(/([a-z])([A-Z])/g, "$1 $2");
  }
  timeoutId: any;
  closeMenuLater(trigger: MatMenuTrigger): void {
    this.timeoutId = setTimeout(() => {
      trigger.closeMenu();
    }, 300); // Delay before closing
  }
  cancelCloseMenu(): void {
    clearTimeout(this.timeoutId);
  }
  clustersTooltipMenuClosed(trigger: MatMenuTrigger): void {
    trigger.closeMenu();
  }
  tooltipMenuClosed(trigger: any): void {
    trigger.closeMenu();
  }
  renderBudgetUtilizationChart(totalBudget: number, usedBudget: number): void {
    // Convert values to millions for display
    const totalBudgetM = totalBudget / 1_000_000;
    const usedBudgetM = usedBudget / 1_000_000;
    const remainingBudgetM = Math.max(totalBudgetM - usedBudgetM, 0);
    const totalInM = `${totalBudgetM.toFixed(1)}M`;
    Highcharts.chart("budgetUtilizationContainer", {
      chart: {
        type: "pie",
        backgroundColor: null,
        borderWidth: 0,
        shadow: false,
      },
      title: {
        text: `<span style="font-size:1.3em;font-weight:bold;">${totalInM}</span>`,
        align: "center",
        verticalAlign: "middle",
        y: 60,
      },
      tooltip: {
        pointFormat:
          "{series.name}: <b>{point.y:.2f}M ({point.percentage:.1f}%)</b>",
      },
      accessibility: {
        point: { valueSuffix: "M" },
      },
      legend: {
        enabled: true,
        align: "center",
        verticalAlign: "bottom",
        layout: "horizontal",
      },
      plotOptions: {
        pie: {
          dataLabels: {
            enabled: true,
            distance: -50,
            style: { fontWeight: "bold", color: "white" },
            format: "{point.name}: {point.y:.1f}M",
          },
          startAngle: -90,
          endAngle: 90,
          center: ["50%", "70%"],
          size: "110%",
        },
      },
      series: [
        {
          type: "pie",
          name: "Budget",
          innerSize: "50%",
          data: [
            { name: "Used", y: usedBudgetM, color: "#024f92" },
            { name: "Left", y: remainingBudgetM, color: "#024d8f17" },
          ],
          showInLegend: true,
        },
      ],
    } as Highcharts.Options);
  }
  // Keys to exclude (case-insensitive optional)
  getKeys(obj: any): string[] {
    const excludedKeys = [
      "DistrictName",
      "Duration",
      "CertAuthName",
      "Name",
      "OfOverAll",
      "ProgramID",
      "TradeLotID",
      "ProgramName",
      "TraineeSelectedContTarget",
      "PerSelectedContraTarget",
      "PerSelectedCompTarget",
      "TradeName",
      "ProgramFocusName",
      "BudgetPercentage",
    ];
    return Object.keys(obj).filter((key) => !excludedKeys.includes(key));
  }
  // Currency formatting for numeric financial fields
  isCurrencyField(key: string): boolean {
    const currencyKeys = [
      "ProgramBudget",
      "TrainingCost",
      "LotStipend",
      "BagAndBadge",
      "ExamCost",
      "OJTPayment",
      "GuruPayment",
      "TransportationCost",
      "MedicalCost",
      "PrometricCost",
      "ProtectorateCost",
      "OtherTrainingCost",
      "TotalCost",
      "CTM",
    ];
    return currencyKeys.includes(key);
  }
  formatNumber(value: any) {
    const numValue = parseFloat(value);
    if (numValue >= 1000000) {
      return (numValue / 1000000).toFixed(1) + "M";
    } else if (numValue >= 1000) {
      return (numValue / 1000).toFixed(0) + "K";
    } else {
      return numValue.toString();
    }
  }
  // keys: string[] = [];
  displayedColumns: string[] = [];
  formRights = { CanEdit: true }; // Mock form rights, adjust as needed
  ngAfterViewInit() {
    this.TablesData.sort = this.sort;
    this.TablesData.paginator = this.paginator;
  }
  // ngAfterViewInit() {
  //   setTimeout(() => {
  //     this.renderBudgetUtilizationChart(this.programBudgetData[0].ProgramBudget, this.programBudgetData[0].TotalCost);
  //   }, 0);
  // }
}
