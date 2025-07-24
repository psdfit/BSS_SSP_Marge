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
@Component({
  selector: "app-program-preview",
  templateUrl: "./program-preview.component.html",
  styleUrls: ["./program-preview.component.scss"],
})
export class ProgramPreviewComponent implements OnInit {
  Status: any = [];
  check: boolean = false;
  constructor(
    private comSrv: CommonSrvService,
    private activeRoute: ActivatedRoute,
    private fb: FormBuilder,
    public ComSrv: CommonSrvService,
    public dialog: MatDialog,
    public dialogRef: MatDialogRef<ProgramPreviewComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    dialogRef.disableClose = false;
  }
  dataObject: any = [];
   ngOnInit() {
    console.log(this.data);
    
    this.currentUser = this.ComSrv.getUserDetails();
    this.setPageTitle();
    this.GetAll();
    this.renderBudgetUtilizationChart(500000, 200000);
  }
    private async GetAll(): Promise<void> {
    try {
      // Use the correct structure for accessing ProgramID
      // console.log(this.data.programData.ProgramID);
      const response: any = await this.comSrv
        .postJSON(`api/ProgramDesign/GetAllProgramDataById`,  this.data.programData.ProgramID )
        .toPromise();
      // Assign the arrays from the response to component properties
      this.programOverviewData = response.Overview;
      this.programBudgetData = response.ProgramBudget;
      this.tradeDesignData = response.TradeBudget;
      this.tradeLotData = response.TradeLotBudget;
      this.renderBudgetUtilizationChart(this.programBudgetData[0].ProgramBudget, this.programBudgetData[0].TotalCost);
    } catch (error) {
      this.comSrv.ShowError("Failed to fetch program data.");
    }
  }
  
  programSummaryData: any = {};
  programOverviewData: any = {};
  programBudgetData: any = {};
  programUsedBudgetSegregationData: any = {};
  tradeDesignData: any = {};
  tradeLotData: any = {};
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
  async FetchData(SPName: string, paramObject: any) {
    try {
      const Param = this.GetParamString(SPName, paramObject);
      const data: any = await this.ComSrv.getJSON(
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
  tablesData: MatTableDataSource<any>;
  workflowForm: FormGroup;
  // @ViewChild(MatTabGroup) tabGroup: MatTabGroup;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
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

  renderBudgetUtilizationChart(totalBudget: number, usedBudget: number): void {
    // Convert values to millions for display
    const totalBudgetM = totalBudget / 1_000_000;
    const usedBudgetM = usedBudget / 1_000_000;
    const remainingBudgetM = Math.max(totalBudgetM - usedBudgetM, 0);
    const totalInM = `${totalBudgetM.toFixed(2)}M`;
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
            format: "{point.name}: {point.y:.2f}M",
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
  const excludedKeys = ['ProgramName', 'TraineeSelectedContTarget','PerSelectedContraTarget','PerSelectedCompTarget','TradeName','ProgramFocusName','BudgetPercentage'];
  return Object.keys(obj).filter(key => !excludedKeys.includes(key));
}
// Currency formatting for numeric financial fields
isCurrencyField(key: string): boolean {
  const currencyKeys = [
    'ProgramBudget',
    'TrainingCost',
    'LotStipend',
    'BagAndBadge',
    'ExamCost',
    'OJTPayment',
    'GuruPayment',
    'TransportationCost',
    'MedicalCost',
    'PrometricCost',
    'ProtectorateCost',
    'OtherTrainingCost',
    'TotalCost',
    'CTM'
  ];
  return currencyKeys.includes(key);
}


formatNumber(value: any) {
 const numValue = parseFloat(value);

  if (numValue >= 1000000) {
    return (numValue / 1000000).toFixed(1) + 'M';
  } else if (numValue >= 1000) {
    return (numValue / 1000).toFixed(1) + 'K';
  } else {
    return numValue.toString();
  }
}

  ngAfterViewInit() {
    setTimeout(() => {
      this.renderBudgetUtilizationChart(this.programBudgetData[0].ProgramBudget, this.programBudgetData[0].TotalCost);
    }, 0);
  }
}
