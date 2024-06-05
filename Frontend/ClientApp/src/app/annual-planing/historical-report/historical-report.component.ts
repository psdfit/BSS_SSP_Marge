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
  selector: 'app-historical-report',
  templateUrl: './historical-report.component.html',
  styleUrls: ['./historical-report.component.scss']
})
export class HistoricalReportComponent implements OnInit {

  @ViewChild("tpaginator", { static: false }) tpaginator: MatPaginator;
  @ViewChild("tsort", { static: false }) tsort: MatSort;
  TablesData: MatTableDataSource<any>;
  TableColumns = [];
  @ViewChild("tabGroup") tabGroup: MatTabGroup;

  @ViewChild("hpaginator", { static: false }) hpaginator: MatPaginator;
  @ViewChild("hsort", { static: false }) hsort: MatSort;
  hTableData: MatTableDataSource<any>;
  hTableColumns = [];

  BSearchCtr = new FormControl('');
  DSearchCtr = new FormControl('');
  environment = environment;
  uniqueTSPName: any = [];
  uniqueTrade: any = [];
  uniqueProvinces: any = [];
  uniqueCluster: any = [];
  uniqueDistrct: any = [];
  filters: IAnalysisReportFilter = { TspID: 0, ProvinceID: 0, ClusterID: 0, DistrictID: 0, TradeID: 0 };
  // BSearchCtr = new FormControl('');
  // DSearchCtr = new FormControl('');
  // environment = environment;

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
  ProgramFocus: any;
  TSPMaster: any;
  SubSector: any;
  constructor(
    private ComSrv: CommonSrvService,
    private AcitveRoute: ActivatedRoute,
    private fb: FormBuilder,
  ) { }
  ngOnInit(): void {
    this.TapIndex = 0
    this.currentUser = this.ComSrv.getUserDetails();
    this.TablesData = new MatTableDataSource([]);
    this.PageTitle();
    this.InitFilterInfo()
    this.InitAnnualPlanInfo()
    this.LoadHistoricalData()
    this.LoadRegistrationAnalysisData()
  }
  filterForm: FormGroup;
  InitFilterInfo() {
    this.filterForm = this.fb.group({
      ClassStartDate: [''],
      ClassEndDate: [''],
      FundingSource: [''],
      ProgramType: [''],
      ProgramFocus: [''],
      Sector: [''],
      SubSector: [''],
      Trade: [''],
      TSPMaster: [''],
      Cluster: [''],
      District: [''],
    });

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
  LoadRegistrationAnalysisData() {
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
  LoadHistoricalData() {
    this.ComSrv.postJSON("api/ProgramDesign/LoadHistoricalReport", { UserID: this.currentUser.UserID }).subscribe(
      (response) => {
        this.GetDataObject = response
        // this.LoadMatTable(this.GetDataObject.HistoricalReport, "HistoricalReport")
        this.FundingSource = this.GetDataObject.FundingSource;
        this.ProgramFocus = this.GetDataObject.ProgramFocus;
        this.ProgramType = this.GetDataObject.ProgramType;
        this.Sector = this.GetDataObject.Sector;
        this.Trade = this.GetDataObject.Trade;
        this.TSPMaster = this.GetDataObject.TSPMaster;
        this.Province = this.GetDataObject.Province;
        this.Cluster = this.GetDataObject.Cluster;
        // this.District = this.GetDataObject.District;


      },
      (error) => {
        this.ComSrv.ShowError(`${error.message}`, "Close", 500000);
      }
    );
  }
  selectedProgram: any = {}

  OnChangeFilterValue(val) {
    const Sector=this.filterForm.getRawValue().Sector
    const Cluster=this.filterForm.getRawValue().Cluster
    this.SubSector = this.GetDataObject.SubSector.filter(d=>Sector.includes(d.SectorID));
    this.District = this.GetDataObject.District.filter(d=>Cluster.includes(d.ClusterID));

  }

  EmptyCtrl() {
    this.DSearchCtr.setValue('');
    this.BSearchCtr.setValue('');
  }
  Save() {
    const fData = this.filterForm.getRawValue()
    const mfData = {
      ClassStartDate: fData.ClassStartDate !== "" ? fData.ClassStartDate : null,
      ClassEndDate: fData.ClassEndDate !== "" ? fData.ClassEndDate : null,
      FundingSource: fData.FundingSource.length > 0 ? fData.FundingSource.join(",") : null,
      ProgramType: fData.ProgramType.length > 0 ? fData.ProgramType.join(",") : null,
      ProgramFocus: fData.ProgramFocus.length > 0 ? fData.FundingSource.join(",") : null,
      Sector: fData.Sector.length > 0 ? fData.Sector.join(",") : null,
      SubSector: fData.SubSector.length > 0 ? fData.SubSector.join(",") : null,
      Trade: fData.Trade.length > 0 ? fData.Trade.join(",") : null,
      TSPMaster: fData.TSPMaster.length > 0 ? fData.TSPMaster.join(",") : null,
      Cluster: fData.Cluster.length > 0 ? fData.Cluster.join(",") : null,
      District: fData.District.length > 0 ? fData.District.join(",") : null,
    };



    // if (this.filterForm.valid) {
    this.ComSrv.ShowWarning("Request Inprogress...");

    this.ComSrv.postJSON("api/ProgramDesign/LoadHistoricalReportByFilter", mfData).subscribe(
      (response: any[]) => {
        if (response.length > 0) {
          this.ComSrv.openSnackBar("Request Completed...");
          this.LoadMatTable(response, "HistoricalReport")

        } else {
          this.TableData.data=[];
          this.ComSrv.ShowError("No record found.", "Close", 500000);
        }

      },
      (error) => {
        this.ComSrv.ShowError(`${error.message}`, "Close", 500000);
      }
    )
    // } else {
    //   this.ComSrv.ShowError("please enter valid data");
    // }
  }



  ResetFrom() {
    this.filterForm.reset()
  }
  PageTitle(): void {
    this.ComSrv.setTitle(this.AcitveRoute.snapshot.data.title);
    this.SpacerTitle = this.AcitveRoute.snapshot.data.title;
  }
  LoadMatTable(tableData: any, dataType: string) {
    if (dataType == "HistoricalReport") {
     if(tableData.length>0){
      this.TableColumns = Object.keys(tableData[0]).filter(key =>
        !key.toLowerCase().includes('id')
      );
     }
      this.TableData = new MatTableDataSource(tableData);
      this.TableData.paginator = this.tpaginator;
      this.TableData.sort = this.tsort;
    }
    if (dataType == "AnalysisReport") {
      if(tableData.length>0){
       this.hTableColumns = Object.keys(tableData[0]).filter(key =>
         !key.toLowerCase().includes('id')
       );
      }
       this.hTableData = new MatTableDataSource(tableData);
       this.hTableData.paginator = this.hpaginator;
       this.hTableData.sort = this.hsort;
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
