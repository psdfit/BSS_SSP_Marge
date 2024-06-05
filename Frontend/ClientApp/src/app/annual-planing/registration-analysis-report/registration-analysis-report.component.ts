import { Component, OnInit, ViewChild } from "@angular/core";
import { CommonSrvService } from "../../common-srv.service";
import { ActivatedRoute } from "@angular/router";
import { MatTableDataSource } from "@angular/material/table";
import { MatPaginator } from "@angular/material/paginator";
import { MatSort } from "@angular/material/sort";
import { MatTabGroup } from "@angular/material/tabs";
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from "@angular/forms";
import { MatOption } from "@angular/material/core";
import { MatSelect, MatSelectChange } from "@angular/material/select";
import { environment } from '../../../environments/environment';

@Component({
  selector: 'app-registration-analysis-report',
  templateUrl: './registration-analysis-report.component.html',
  styleUrls: ['./registration-analysis-report.component.scss']
})
export class RegistrationAnalysisReportComponent implements OnInit {
  @ViewChild("tabGroup") tabGroup: MatTabGroup;
  @ViewChild("tpaginator", { static: false }) tpaginator: MatPaginator;
  @ViewChild("tsort", { static: false }) tsort: MatSort;
  TablesData: MatTableDataSource<any>;
  TableColumns = [];
  BSearchCtr = new FormControl('');
  DSearchCtr = new FormControl('');
  environment = environment;
  uniqueTSPName: any = [];
  uniqueTrade: any = [];
  uniqueProvinces: any = [];
  uniqueCluster: any = [];
  uniqueDistrct: any = [];
  filters: IAnalysisReportFilter = { TspID: 0, ProvinceID: 0, ClusterID: 0, DistrictID: 0, TradeID: 0 };
  TapIndex = 0

  error: any;
  currentUser: any;
  GetDataObject: any = {}

  SpacerTitle: string;
  TableData: any;
  FundingSource: any;
  Sector: any;
  Trade: any;
  Duration: any;
  Province: any;
  Cluster: any;
  District: any;
  ProgramType: any;
  constructor(
    private ComSrv: CommonSrvService,
    private AcitveRoute: ActivatedRoute,
    private fb: FormBuilder,
  ) { }
  ngOnInit(): void {
    this.LoadData()
    this.TapIndex = 0
    this.currentUser = this.ComSrv.getUserDetails();
    this.TablesData = new MatTableDataSource([]);
    this.PageTitle();
    this.InitAnnualPlanInfo()
  }
  AnnualPlanInfoForm: FormGroup;
  InitAnnualPlanInfo() {
    this.AnnualPlanInfoForm = this.fb.group({
      TSPName: [''],
      Trade: [''],
      Province: [''],
      Cluster: [''],
      Distrct: [''],
    });
  }
  LoadData() {
    this.ComSrv.getJSON("api/ProgramDesign/LoadAnalysisReport").subscribe(
      (response) => {
        this.GetDataObject = response
        this.LoadMatTable(response, "AnalysisReport")
        this.LoadMatTableFiltered(response)
      },
      (error) => {
        this.ComSrv.ShowError(`${error.message}`, "Close", 500000);
      }
    );
  }

  getDependantFilters() {
    const tspNameValue = this.AnnualPlanInfoForm.get('TSPName').value || 0;
    // Similarly, get values from other form controls
    const provinceValue = this.AnnualPlanInfoForm.get('Province').value || 0;
    const clusterValue = this.AnnualPlanInfoForm.get('Cluster').value || 0;
    const districtValue = this.AnnualPlanInfoForm.get('Distrct').value || 0;
    const tradeValue = this.AnnualPlanInfoForm.get('Trade').value || 0;

    this.filters = {
      TspID: tspNameValue,
      ProvinceID: provinceValue,
      ClusterID: clusterValue,
      DistrictID: districtValue,
      TradeID: tradeValue
    };
      this.GetData();
  }
  GetData() {
    this.ComSrv.getJSON(`api/ProgramDesign/LoadAnalysisReport/filter?filter=${this.filters.TspID}&filter=${this.filters.ProvinceID}&filter=${this.filters.ClusterID}&filter=${this.filters.DistrictID}&filter=${this.filters.TradeID}`).subscribe(
      (response) => {
        this.LoadMatTable(response,'AnalysisReport')
      },
      (error) => {
        this.ComSrv.ShowError(`${error.message}`, "Close", 500000);
      }
    );
  }
  LoadMatTableFiltered(tableData){

    // this.TableData = new MatTableDataSource(tableData);

    this.uniqueProvinces = Array.from(new Set(tableData.map(d => d.ProvinceID)))
      .map(provinceID => ({
        ProvinceID: provinceID,
        Province: tableData.find(d => d.ProvinceID === provinceID).Province
      }));

      this.uniqueTSPName = Array.from(new Set(tableData.map(d => d.TspID)))
      .map(tspID => ({
        TspID: tspID,
        TspName: tableData.find(d => d.TspID === tspID).TspName
      }));

      this.uniqueTrade = Array.from(new Set(tableData
        .filter(d => d.TradeID !== undefined)
        .map(d => d.TradeID)))
        .map(tradeID => ({
          TradeID: tradeID,
          TradeName: tableData.find(d => d.TradeID === tradeID).TradeName
        }));
      
      
      console.log(this.uniqueTrade)
      
      this.uniqueCluster = Array.from(new Set(tableData.map(d => d.ClusterID)))
      .map(clusterID => ({
        ClusterID: clusterID,
        Cluster: tableData.find(d => d.ClusterID === clusterID).Cluster
      }));

      this.uniqueDistrct = Array.from(new Set(tableData.map(d => d.DistrictID)))
      .map(districtID => ({
        DistrictID: districtID,
        District: tableData.find(d => d.DistrictID === districtID).District
      }));


      // this.LoadMatTable(this.TableData,'AnalysisReport')

}
  selectedProgram: any = {}
  OnChangeFilterValue(val) {
    console.log(this.AnnualPlanInfoForm.getRawValue())
  }
  EmptyCtrl() {
    this.DSearchCtr.setValue('');
    this.BSearchCtr.setValue('');
  }
 



  ResetFrom() {
    this.AnnualPlanInfoForm.reset()
  }
  PageTitle(): void {
    this.ComSrv.setTitle(this.AcitveRoute.snapshot.data.title);
    this.SpacerTitle = this.AcitveRoute.snapshot.data.title;
  }
  LoadMatTable(tableData: any, dataType: string) {
    if (dataType == "AnalysisReport") {
     if(tableData.length>0){
      this.TableColumns = Object.keys(tableData[0]).filter(key =>
        !key.toLowerCase().includes('id')
      );
     }
      this.TableData = new MatTableDataSource(tableData);
      this.TableData.paginator = this.tpaginator;
      this.TableData.sort = this.tsort;
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
    if (this.tabGroup) {
      this.tabGroup.selectedTabChange.subscribe((event) => {
        this.TapIndex = event.index
      });
    }
  }
}

export interface IAnalysisReportFilter {
  TspID: number;
  TradeID: number;
  ProvinceID: number;
  ClusterID: number;
  DistrictID: number;
}
