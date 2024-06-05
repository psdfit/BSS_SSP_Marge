import { filter, map } from 'rxjs/operators';
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
  selector: 'app-calculate-ctm',
  templateUrl: './calculate-ctm.component.html',
  styleUrls: ['./calculate-ctm.component.scss']
})
export class CalculateCtmComponent implements OnInit {


  matSelectArray: MatSelect[] = [];
  @ViewChild('FundingSourceOpt') FundingSourceOpt: MatSelect;
  SelectedAll_FundingSourceOpt: string;

  @ViewChild('FundingCategoryOpt') FundingCategoryOpt: MatSelect;
  SelectedAll_FundingCategoryOpt: string;

  @ViewChild('SchemeTypeOpt') SchemeTypeOpt: MatSelect;
  SelectedAll_SchemeTypeOpt: string;

  @ViewChild('SectorOpt') SectorOpt: MatSelect;
  SelectedAll_SectorOpt: string;

  @ViewChild('TradeData') TradeData: MatSelect;
  SelectedAll_TradeData: string;
  // @ViewChild('Cluster') Cluster: MatSelect;
  // SelectedAll_Cluster: string;
  // @ViewChild('District') District: MatSelect;
  // SelectedAll_District: string;

  @ViewChild("tabGroup") tabGroup: MatTabGroup;
  @ViewChild("tpaginator") tpaginator: MatPaginator;
  @ViewChild("tsort") tsort: MatSort;
  TablesData: MatTableDataSource<any>;
  TableColumns = [];
  STSearchCtr = new FormControl('');
  BSearchCtr = new FormControl('');
  DSearchCtr = new FormControl('');
  FSSearchCtr = new FormControl('');
  FCSearchCtr = new FormControl('');
  environment = environment;
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
  CTMBulkReport: any;
  Parameter: {};
  FundingCategory: any[]=[]
  constructor(
    private ComSrv: CommonSrvService,
    private AcitveRoute: ActivatedRoute,
    private fb: FormBuilder,
  ) { }


  SelectAll(event: any, dropDownNo, controlName, formGroup) {
    // debugger
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
  ngOnInit(): void {
    this.TapIndex = 0
    this.currentUser = this.ComSrv.getUserDetails();
    this.TablesData = new MatTableDataSource([]);
    this.PageTitle();
    this.InitFilterInfo()
    this.LoadData()
  }
  filterForm: FormGroup;
  
  InitFilterInfo() {
    this.filterForm = this.fb.group({
      ContractAwardStartDate: ['2013-07-30',Validators.required],
      ContractAwardEndDate: [new Date(),Validators.required],
      FundingSource: [''],
      FundingCategory: [''],
      SchemeType: [''],
      Sector: [''],
      Trade: [''],
      Duration: [''],
      Cluster: [''],
      District: [''],
    });
    this.filterForm.get('Sector').valueChanges.subscribe((d) => {
     if(d){
      if(!this.filterForm.get('Sector').value.length){
        this.Trade = this.GetDataObject.Trade;
      }
      if (this.filterForm.get('Sector').value.length) {
        this.Trade = this.GetDataObject.Trade.filter(t => d.includes(t.SectorID))
      }
     }
    
    })

    this.filterForm.get('Cluster').valueChanges.subscribe((d) => {
      if (d) {
        this.filterForm.get('District').setValue('')
        this.District = this.GetDataObject.District.filter(t =>d.includes(t.ClusterID))
      }
    })
    this.filterForm.get('FundingSource').valueChanges.subscribe(fId => {
      if (fId) {
        this.filterForm.get('FundingCategory').setValue('');
        this.FundingCategory = this.GetDataObject.FundingCategory.filter(t =>fId.includes(t.FundingSourceID))
      }
    });
  }
  LoadData() {
    this.ComSrv.postJSON("api/ProgramDesign/LoadCTMReport", { UserID: this.currentUser.UserID }).subscribe(
      (response) => {
        this.GetDataObject = response
        // const ProgramTablesData = this.GetDataObject.CTMCalculationReport;
        // this.LoadMatTable(ProgramTablesData, "CTM")
        // this.LoadMatTableFiltered(ProgramTablesData)
        this.FundingSource = this.GetDataObject.FundingSource;
        this.ProgramType = this.GetDataObject.ProgramType;
        this.Sector = this.GetDataObject.Sector;
        this.Trade = this.GetDataObject.Trade;
        this.Duration = this.GetDataObject.Duration;
        this.Province = this.GetDataObject.Province;
        this.Cluster = this.GetDataObject.Cluster;
        this.District = this.GetDataObject.District;
      },
      (error) => {
        this.ComSrv.ShowError(`${error.message}`, "Close", 500000);
      }
    );
  }


  // GetCTMBulkReport(BulkReport: any) {
  //   debugger;
  //   this.FundingSource = [];
  //   const FundingSource = new Map();

  //   BulkReport.forEach(data => {
  //     FundingSource.set(data.FundingSource, data.FundingSourceID);
  //   });

    
  //   FundingSource.forEach((ID,Name) => {
  //     this.FundingSource.push({ FundingSource:Name, FundingSourceID:ID });
  //   });

  // }


  selectedProgram: any = {}
 
  EmptyCtrl() {
    this.BSearchCtr.setValue('');
    this.FSSearchCtr.setValue('');
    this.FCSearchCtr.setValue('');
    this.STSearchCtr.setValue('');
  }

  async FetchRecord() {
    if (this.filterForm.valid) {
      const fData = this.filterForm.getRawValue();
      this.Parameter = {};
  
      const {
        FundingSource: fundingSourceIds =[0],
        SchemeType: schemeTypeIds =[0],
        Sector: sectorIds =[0],
        Trade: tradeIds =[0],
        District: districtIds =[0],
        Cluster: clusterIds =[0],
        Duration: duration,
        ContractAwardStartDate,
        ContractAwardEndDate,
        FundingCategory=[0],
      } = fData;
  
      const fs = this.FundingSource
        .filter(d => fundingSourceIds.includes(d.FundingSourceID))
        .map(d => d.FundingSourceName);
  
      const st = this.ProgramType
        .filter(d => schemeTypeIds.includes(d.PTypeID))
        .map(d => d.PTypeName);
  
      const s = this.Sector
        .filter(d => sectorIds.includes(d.SectorID))
        .map(d => d.SectorName);
  
      const t = this.Trade
        .filter(d => tradeIds.includes(d.TradeID))
        .map(d => d.TradeName);
  
      const d = this.Duration
        .filter(d => d.DurationID === duration);
  
      const district = this.District
        .filter(d => districtIds.includes(d.DistrictID))
        .map(d => d.DistrictName);
  
      const cluster = this.Cluster
        .filter(d => clusterIds.includes(d.ClusterID))
        .map(d => d.ClusterName);
  
      this.Parameter = {
        'Parameter': '-------',
        'FundingSource': fs.join(', '),
        'SchemeType': st.join(', '),
        'Sector': s.join(', '),
        'Trade': t.join(', '),
        'Duration': d.length > 0 ? d[0].Duration + " Month" : "",
        'District': district.join(', '),
        'Cluster': cluster.join(', '),
      };
  
      const mfData = {
        ContractAwardStartDate: ContractAwardStartDate || null,
        ContractAwardEndDate: ContractAwardEndDate || null,
        FundingSource: fundingSourceIds.length > 0 ? fundingSourceIds.join(",") : null,
        FundingCategory: FundingCategory.length > 0 ? FundingCategory.join(",") : null,
        SchemeType: schemeTypeIds.length > 0 ? schemeTypeIds.join(",") : null,
        Sector: sectorIds.length > 0 ? sectorIds.join(",") : null,
        Trade: tradeIds.length > 0 ? tradeIds.join(",") : null,
        Duration: duration || "0",
        Cluster: clusterIds.length > 0 ? clusterIds.join(",") : null,
        District: districtIds.length > 0 ? districtIds.join(",") : null,
      };
  
      await this.FetchCTMBulkReport(mfData);
      await this.FetchCTMTradeWise(mfData);
    }
  }
  
  FetchCTMTradeWise(formData) {
    // debugger;
    this.ComSrv.postJSON("api/ProgramDesign/FetchCTMTradeWise", formData).subscribe(
      (response) => {
        if (response) {
          this.LoadMatTable(response, "CTM")
        } else {
          this.TableData.data = [];
          this.ComSrv.ShowError("No record found.", "Close", 500000);
        }
      },
      (error) => {
        this.ComSrv.ShowError(`${error.message}`, "Close", 500000);
      }
    )
  }
  FetchCTMBulkReport(formData) {
    this.ComSrv.postJSON("api/ProgramDesign/FetchCTMBulkReport", formData).subscribe(
      (response) => {
        if (response) {
          this.CTMBulkReport = response
        } else {
          this.TableData.data = [];
          this.ComSrv.ShowError("No record found.", "Close", 500000);
        }
      },
      (error) => {
        this.ComSrv.ShowError(`${error.message}`, "Close", 500000);
      }
    )
  }
  LoadMatTableFiltered(tableData: any): void {
    const fundingSource = {};
    const trade = {};
    const cluster = {};
    const district = {};
    // tableData.forEach(item => {
    //   fundingSource[item.FundingSource] = item.FundingSourceID;
    // });
    // tableData.forEach(item => {
    //   trade[item.Trade] = item.TradeID;
    // });
    // this.schemeType = [...new Set(tableData.map(item => item.SchemeType))];
    // this.sector = [...new Set(tableData.map(item => item.Sector))];
  }
  ResetFrom() {
    this.InitFilterInfo()
    this.LoadMatTable([], "CTM")
  }
  PageTitle(): void {
    this.ComSrv.setTitle(this.AcitveRoute.snapshot.data.title);
    this.SpacerTitle = this.AcitveRoute.snapshot.data.title;
  }
  LoadMatTable(tableData: any, dataType: string) {
    if (dataType == "CTM") {
      if (tableData.length > 0) {
        this.TableColumns = Object.keys(tableData[0]).filter(key =>
          !key.toLowerCase().includes('id')
        );
        this.TableColumns.unshift('Sr#')
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
  DataExcelExport(data: any[], title) {
    const dataWithParameter: any[] = [data, this.Parameter]
    if (data.length > 0) {
      this.ComSrv.ExcelExportWithForm(dataWithParameter, title);
    } else {
      this.ComSrv.ShowError('No Record Found')
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
    this.matSelectArray = [this.TradeData,this.FundingSourceOpt,this.FundingCategoryOpt,this.SchemeTypeOpt,this.SectorOpt, this.Province, this.Cluster, this.District];
    if (this.tabGroup) {
      this.tabGroup.selectedTabChange.subscribe((event) => {
        this.TapIndex = event.index
      });
    }
  }
}
