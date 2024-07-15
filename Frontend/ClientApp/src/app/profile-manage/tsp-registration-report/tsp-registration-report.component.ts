import { Component, OnInit, ViewChild, ChangeDetectorRef } from "@angular/core";
import { CommonSrvService } from "../../common-srv.service";
import { ActivatedRoute, Router } from "@angular/router";
import { MatTableDataSource } from "@angular/material/table";
import { MatPaginator } from "@angular/material/paginator";
import { MatSort } from "@angular/material/sort";
import { MatTabGroup } from "@angular/material/tabs";
import { FormBuilder, FormControl, FormGroup, Validators } from "@angular/forms";
import { MatDialog, MatDialogRef } from "@angular/material/dialog";
import { environment } from '../../../environments/environment';
import * as Highcharts from "highcharts";
import { SelectionModel } from "@angular/cdk/collections";
@Component({
  selector: 'app-tsp-registration-report',
  templateUrl: './tsp-registration-report.component.html',
  styleUrls: ['./tsp-registration-report.component.scss']
})
export class TspRegistrationReportComponent implements OnInit {
  isChecked: boolean = false
  TspName: any;
  rowData: any;
  tspName: string;
  ResponseData: any = []
  isEdit: number;
  Response: any = []
  SelectedRow: any;
  constructor(
    private ComSrv: CommonSrvService,
    private ActiveRoute: ActivatedRoute,
    private fb: FormBuilder,
    private Cdr: ChangeDetectorRef,
    private Dialog: MatDialog,
  ) { }
  environment = environment;
  error: any;
  GetDataObject: any = {}
  SpacerTitle: string;
  TSearchCtr = new FormControl('');
  TapTTitle: string = "Profile"
  TableColumns = [];
  DTableColumns = [];
  @ViewChild("tabGroup") tabGroup: MatTabGroup;
  @ViewChild("Paginator") Paginator: MatPaginator;
  @ViewChild("Sort") Sort: MatSort;
  @ViewChild("DPaginator") DPaginator: MatPaginator;
  @ViewChild("DSort") DSort: MatSort;
  selection = new SelectionModel<any>(true, []);
  TSPsArray: any[];
  tspUserIDsArray: any[];
  isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.DTablesData.data.length;
    return numSelected === numRows;
  }
  masterToggle() {
    this.isAllSelected() ?
      this.selection.clear() :
      this.DTablesData.data.forEach(row => this.selection.select(row));
  }
  currentUser: any;
  TapIndex: any;
  TablesData: MatTableDataSource<any>;
  DTablesData: MatTableDataSource<any>;
  ngOnInit(): void {
    this.currentUser = this.ComSrv.getUserDetails();
    this.TapIndex = 0
    this.TablesData = new MatTableDataSource([]);
    this.DTablesData = new MatTableDataSource([]);
    this.PageTitle();
    this.GetData();
    this.InitFilterForm();
    this.ApplyFilter()
  }
  PageTitle(): void {
    this.ComSrv.setTitle(this.ActiveRoute.snapshot.data.title);
    this.SpacerTitle = this.ActiveRoute.snapshot.data.title;
  }
  CurrentDate: Date = new Date()
  FilterForm: FormGroup;
  SDate: Date = new Date()
  InitFilterForm() {
    this.FilterForm = this.fb.group({
      StartDate: [new Date('2024-04-01'), Validators.required],
      EndDate: [this.CurrentDate, Validators.required],
      SchemeDesignOn: [1, Validators.required],
    });
  }
  GetData() {
    this.ComSrv.postJSON("api/BusinessProfile/FetchTspRegistration", { UserID: this.currentUser.UserID }).subscribe(
      (response) => {
        this.Response = response
        if (this.Response.length > 0) {
          // this.LoadMatTable(response, "TspMaster");
        }
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
  LoadMatTable(tableData: any, tableName: string) {
    this.TableColumns = Object.keys(tableData[0]).filter(key => !key.includes('ID'));
    this.TablesData = new MatTableDataSource(tableData)
    this.TablesData.paginator = this.Paginator;
    this.TablesData.sort = this.Sort;
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
  _formsData: any = []
  async getFormsDetail() {
    this.SPName = "RD_SSPFormWiseTSPCount"
    this.paramObject = { StartDate: this.FilterForm.get("StartDate").value, EndDate: this.FilterForm.get("EndDate").value }
    this._formsData = []
    this._formsData = await this.FetchData(this.SPName, this.paramObject)
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
  
  public async generateBarChart(data) {
    if (data.registrationDetail.length > 0) {
      this.LoadMatTable(data.registrationDetail, "TspMaster");
    }
    const formWiseTSPCount: any[] = data.formWiseTSPCount
    const programWiseRegisteredTSP: any[] = data.programWiseRegisteredTSP
    const registeredTSPCount: any[] = data.registeredTSPCount
    this.optionsClassStatus.series = []
    this.programWiseRegChart.series[0].data = []
    this.tspWiseRegChart.series[0].data = []
    this.regCompletionStatusWiseChart.series[0].data = []
    formWiseTSPCount.forEach(element => {
      this.optionsClassStatus.series.push({
        name: this.camelCaseToWords(element.Forms),
        data: [element.Count],
      });
    });
    programWiseRegisteredTSP.forEach(element => {
      this.programWiseRegChart.series[0].data.push({
        name: element.PTypeName,
        y: element.Count,
      });
    });
    registeredTSPCount.forEach(element => {
      this.tspWiseRegChart.series[0].data.push({
        name: element.TSPType,
        y: element.Count,
      });
    });
    const filteredCounts = formWiseTSPCount.filter(element => element.Forms === 'TSPProfile' || element.Forms === 'RegistrationPayment');
    this.regCompletionStatusWiseChart.series[0].data.push(
      {
        name: 'Pending',
        y: Number(filteredCounts[0].Count) - Number(filteredCounts[1].Count),
      },
      {
        name: 'Completion',
        y: Number(filteredCounts[1].Count),
      }
    );
    Highcharts.chart("containerPassed", this.optionsClassStatus);
    Highcharts.chart("programRegChart", this.programWiseRegChart);
    Highcharts.chart("tspRegChart", this.tspWiseRegChart);
    Highcharts.chart("regCompletionStatusChart", this.regCompletionStatusWiseChart);
  }
  optionsClassStatus: any = {
    chart: {
      type: "column",
      height: "300px",
    },
    title: {
      text: "",
    },
    colors: ['#005b9e', '#7cb5ec', '#f15c80', '#fdb813', '#e4d354', '#2b908f', '#f45b5b', '#91e8e1'],
    credits: {
      enabled: false,
    },
    legend: {
      enabled: true,
      labelFormatter: function () {
        return this.name + " (" + this.yData[0] + ")";
      },
    },
    xAxis: {
      categories: ["Registration Forms"],
      crosshair: true,
    },
    yAxis: {
      min: 0,
      title: {
        text: "TSP Count",
      },
    },
    tooltip: {
      headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
      pointFormat:
        '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
        '<td style="padding:0"><b>{point.y:.1f}</b></td></tr>',
      footerFormat: "</table>",
      shared: true,
      useHTML: true,
    },
    plotOptions: {
      column: {
        pointPadding: 0.2,
        borderWidth: 0,
        // showInLegend: true,
        dataLabels: {
          enabled: true,
          format: '{point.y}', // Format to display the data label
        },
      },
    },
    series: [],
  };
  programWiseRegChart: any = {
    chart: {
      plotBackgroundColor: null,
      plotBorderWidth: null,
      plotShadow: false,
      type: "pie",
      height: "300px",
    },
    title: {
      text: "",
    },
    colors: ['#005b9e', '#7cb5ec', '#fdb813', '#f15c80', '#e4d354', '#2b908f', '#f45b5b', '#91e8e1'],
    credits: {
      enabled: false,
    },
    tooltip: {
      pointFormat:
        '<br>{point.name}: {point.y}<br>Total: {point.total}<br>{point.percentage:.1f} %',
    },
    accessibility: {
      point: {
        valueSuffix: "%",
      },
    },
    plotOptions: {
      pie: {
        innerSize: '50%',
        allowPointSelect: true,
        cursor: "pointer",
        dataLabels: {
          enabled: true,
          format: '{point.name}:{point.percentage:.0f}%',
          // distance: -45, // Adjust the distance to place the labels inside
          style: {
            color: 'black', // Color of the text inside the pie slices
            textOutline: 'none', // Remove the text outline
            //  rotation: 75 // Rotate the text by 45 degrees
          }
        },
        // showInLegend: true,
      },
    },
    series: [
      {
        // name: "Forms",
        colorByPoint: true,
        data: [],
      },
    ],
  };
  tspWiseRegChart: any = {
    chart: {
      plotBackgroundColor: null,
      plotBorderWidth: null,
      plotShadow: false,
      type: "pie",
      height: "300px",
    },
    title: {
      text: "",
    },
    colors: ['#005b9e', '#7cb5ec', '#fdb813', '#f15c80', '#e4d354', '#2b908f', '#f45b5b', '#91e8e1'],
    credits: {
      enabled: false,
    },
    tooltip: {
      pointFormat:
        '<br>{point.name}: {point.y}<br>Total: {point.total}<br>{point.percentage:.1f} %',
    },
    accessibility: {
      point: {
        valueSuffix: "%",
      },
    },
    plotOptions: {
      pie: {
        innerSize: '50%',
        allowPointSelect: true,
        cursor: "pointer",
        dataLabels: {
          enabled: true,
          format: '{point.name}:{point.percentage:.0f}%',
          // distance: -45, // Adjust the distance to place the labels inside
          style: {
            color: 'black', // Color of the text inside the pie slices
            textOutline: 'none', // Remove the text outline
            //  rotation: 75 // Rotate the text by 45 degrees
          }
        },
        // showInLegend: true,
      },
    },
    series: [
      {
        // name: "Forms",
        colorByPoint: true,
        data: [],
      },
    ],
  };
  regCompletionStatusWiseChart: any = {
    chart: {
      plotBackgroundColor: null,
      plotBorderWidth: null,
      plotShadow: false,
      type: "pie",
      height: "300px",
    },
    title: {
      text: "",
    },
    colors: ['#f45b5b', '#2b908f'],
    credits: {
      enabled: false,
    },
    tooltip: {
      pointFormat: '<br>{point.name}: {point.y}<br>Total: {point.total}<br>{point.percentage:.1f} %',
    },
    accessibility: {
      point: {
        valueSuffix: "%",
      },
    },
    plotOptions: {
      pie: {
        innerSize: '50%',
        allowPointSelect: true,
        cursor: "pointer",
        dataLabels: {
          enabled: true,
          format: '{point.name}: {point.percentage:.0f}%',
          style: {
            color: 'black', // Color of the text inside the pie slices
            textOutline: 'none', // Remove the text outline
            rotation: 75 // Rotate the text by 75 degrees
          }
        }
      },
    },
    series: [
      {
        colorByPoint: true,
        data: [],
      },
    ],
  };
  
  ApplyFilter() {
    if (this.FilterForm.valid) {
      this.ComSrv.postJSON("api/BusinessProfile/GetDashboardData", this.FilterForm.value).subscribe(
        (response) => {
          this.generateBarChart(response)
        },
        (error) => {
          this.ComSrv.ShowError(`${error.message}`, "Close", 500000);
        }
      );
    } else {
      this.ComSrv.ShowError("Required form fields are missing");
    }
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
        this.TapIndex = event.index
        if (this.TapIndex == 0) {
          this.isChecked = false
        }
      });
    }
  }
}
