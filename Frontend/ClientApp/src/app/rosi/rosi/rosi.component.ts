import { Component, OnInit, ViewChild, ElementRef, AfterViewInit, ViewChildren, QueryList, VERSION } from '@angular/core';
import { CommonSrvService } from '../../common-srv.service';

import { Element } from '@angular/compiler/src/render3/r3_ast';
import { MatProgressBar } from '@angular/material/progress-bar';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { FormGroup, FormBuilder, Validators, FormGroupDirective } from '@angular/forms';
import { environment } from '../../../environments/environment';
import { ModelBase } from '../../shared/ModelBase';
import { UserRightsModel } from '../../master-data/users/users.component';
import { ActivatedRoute } from '@angular/router';
import * as moment from 'moment';
import { IOrgConfig } from '../../registration/Interface/IOrgConfig';
import { invalid } from '@angular/compiler/src/render3/view/util';
import { FormControl } from '@angular/forms';
import { MatSelect } from '@angular/material/select';
import { MatOption } from '@angular/material/core';
import { ExportExcel } from '../../shared/Interfaces';
import { GroupByPipe } from 'angular-pipes';
import { DatePipe } from '@angular/common';
import { DialogueService } from '../../shared/dialogue.service';
import { EnumUserLevel, EnumExcelReportType } from '../../shared/Enumerations';

@Component({
  selector: 'hrapp-rosi',
  templateUrl: './rosi.component.html',
  styleUrls: ['./rosi.component.scss'],
  providers: [GroupByPipe, DatePipe]

})

export class ROSIComponent implements OnInit {
  @ViewChild('organization') organization: MatSelect;
  @ViewChild('fundingSource') fundingSource: MatSelect;
  @ViewChild('ptype') ptype: MatSelect;
  @ViewChild('scheme') scheme: MatSelect;
  @ViewChild('sector') sector: MatSelect;
  @ViewChild('cluster') cluster: MatSelect;
  @ViewChild('district') district: MatSelect;
  @ViewChild('tsp') tsp: MatSelect;
  @ViewChild('trade') trade: MatSelect;
  @ViewChild('gender') gender: MatSelect;
  @ViewChild('duration') duration: MatSelect;

  env = environment;
  rosiform: FormGroup;
  title: string; savebtn: string;
  fsAllSelected = false;
  ptAllSelected = false;
  schAllSelected = false;
  sectAllSelected = false;
  clstAllSelected = false;
  distAllSelected = false;
  tspAllSelected = false;
  trdAllSelected = false;
  durAllSelected = false;
  gendAllSelected = false;
  orgAllSelected = false;


  IsAllSelected = false;
  //AllSelected=false;
  //AllSelected=false;
  //AllSelected=false;
  displayedColumnsROSI = ['ROSI', 'AverageWageRate', 'OpportunityCost', 'VerifiedTrainees',
    'AverageTrainingCost', 'CompletedTrainees', 'MarginofEmployment'];
  displayedColumnsSchemeROSI = ['ROSI', 'SchemeName', 'SchemeCode', 'SchemeType', 'Contractual', 'CancelledClasses', 'NetContractual',
    'CompletedTrainees', 'ReportedEmployed', 'VerifiedTrainees',
    'CostPerAppendix', 'Appendix', 'Testing', 'Total',
    'AvergaeWageRate', 'AverageTrainingCost',
    'OpportunityCost', 'NetIncrease'];
  displayedColumnsTSPROSI = ['ROSI', 'TSPName', 'AverageTrainingCost', 'OpportunityCost', 'CompletedTrainees', 'VerifiedTrainees'];
  displayedColumnsClassROSI = ['ROSI', 'ClassCode', 'OpportunityCost', 'CompletedTrainees', 'VerifiedTrainees'];

  Schemes: [];
  TSPs: [];
  Classes: [];
  ProgramTypes: [];
  Sectors: [];
  Clusters: [];
  Districts: [];
  Trades: [];
  Organizations: [];
  FundingSources: [];
  Genders: any[];
  GendersFiltered: any[];
  Durations: [];
  calculatedROSI: [];
  calculatedROSIDataSet: [];


  pbteClasses: MatTableDataSource<any>;
  pbteTSPs: MatTableDataSource<any>;
  pbteTrainees: MatTableDataSource<any>;
  rosi: MatTableDataSource<any>;
  rosiArray: any;
  rosiByScheme: MatTableDataSource<any>;
  rosiByTSP: MatTableDataSource<any>;
  rosiByClass: MatTableDataSource<any>;

  //filters: IQueryFilters = {
  //  SchemeIDs: [], TSPIDs: [], PTypeIDs: [], SectorIDs: [], ClusterIDs: [],
  //  DistrictIDs: [], TradeIDs: [], OrganizationIDs: [], FundingSourceIDs: [], EmploymentFlag : true,
  //  StartDate: new Date(""), EndDate: new Date(""),

  //};

  filters: IQueryFilters = {
    SchemeIDs: []
    , TSPIDs: []
    , PTypeIDs: []
    , SectorIDs: []
    , ClusterIDs: []
    , DistrictIDs: []
    , TradeIDs: []
    , OrganizationIDs: []
    , FundingSourceIDs: []
    , GenderIDs: []
    , DurationIDs: []
    , EmploymentFlag: false
    , ActualContractualFlag: true
    , StartDate: ""
    , EndDate: ""
  };

  filtersValues: IDataQueryFilters = {
    SchemeIDs: '', TSPIDs: '', PTypeIDs: '', SectorIDs: '', ClusterIDs: '',
    DistrictIDs: '', TradeIDs: '', OrganizationIDs: '', FundingSourceIDs: '', GenderIDs: '', DurationIDs: '', EmploymentFlag: false, ActualContractualFlag: true,
    StartDate: "", EndDate: ""

  };

  //filters: IQueryFilters = {
  //  SchemeID: 0, TSPID: 0, PTypeID: 0, SectorID: 0, ClusterID: 0, DistrictID: 0, TradeID: 0, OrganizationID: 0, FundingSourceID: 0, GenderID:0,
  //  EmploymentFlag: false,StartDate: "", EndDate: "",

  //};

  isOpenRegistration: boolean = false;
  DateFilter: boolean = false;
  isOpenRegistrationMessage: string = "";
  formrights: UserRightsModel;
  EnText: string = "ROSI";
  checkedVerifiedEmployment: boolean;
  checkedEmployment: boolean;
  checkedReportedEmployment: boolean = false;
  error: String;
  minEndDate: Date;

  query = {
    order: 'RosiID',
    limit: 5,
    page: 1
  };
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;


  @ViewChild('SortTrainee') SortTrainee: MatSort;
  @ViewChild('PageTrainee') PageTrainee: MatPaginator;
  @ViewChild('SortDropOutTrainee') SortDropOutTrainee: MatSort;
  @ViewChild('PageDropOutTrainee') PageDropOutTrainee: MatPaginator;
  @ViewChild('SortTSP') SortTSP: MatSort;
  @ViewChild('PageTSP') PageTSP: MatPaginator;
  @ViewChild('SortClass') SortClass: MatSort;
  @ViewChild('PageClass') PageClass: MatPaginator;
  @ViewChild('tabGroup') tabGroup;

  working: boolean;
  constructor(private fb: FormBuilder, private ComSrv: CommonSrvService, private route: ActivatedRoute, public dialogueService: DialogueService, private _date: DatePipe) {
    this.rosiform = this.fb.group({
      RosiID: 0,
      ROSI: '',
      SchemeID: '',
      ClassID: '',
      TSPID: '',

    }, { updateOn: "blur" });
    this.pbteTrainees = new MatTableDataSource([]);
    this.formrights = ComSrv.getFormRights("/ROSI");
  }

  getSelectedTabData() {
    switch (this.tabGroup?.selectedIndex ?? 0) {
      case 0:
        //this.getSchemeData();
        break;
      case 1:
        //this.getTSPData();
        break;
      default:
    }
  }

  GetData() {
    this.ComSrv.getJSON('api/ROSI/GetROSI',).subscribe((d: any) => {
      this.Schemes = d[0];
      this.TSPs = d[1];
      this.Classes = d[2];
      this.ProgramTypes = d[3];
      this.Sectors = d[4];
      this.Clusters = d[5];
      this.Districts = d[6];
      this.Trades = d[7];
      this.Organizations = d[8];
      this.FundingSources = d[9];
      this.Genders = d[10];
      this.Durations = d[11];

      this.Genders = this.Genders.filter(g => g.GenderID != 6);

      //this.rosi = d[3];
      //this.rosiArray = d[3];

    }, error => this.error = error// error path
    );
  };

  onBlur() {
    // debugger;
    //(blur)="onBlur()"
    // this.filtersValues.TSPIDs = this.filters.TSPIDs.length == 0 ? '' : this.filters.TSPIDs.join(',')
    // this.GetFiltersDataByDependancy();
  }

  UpdateFiltersIDs() {
    this.filtersValues.SchemeIDs = this.filters.SchemeIDs.length == 0 ? '' : this.filters.SchemeIDs.join(',')
    this.filtersValues.TSPIDs = this.filters.TSPIDs.length == 0 ? '' : this.filters.TSPIDs.join(',')
    this.filtersValues.PTypeIDs = this.filters.PTypeIDs.length == 0 ? '' : this.filters.PTypeIDs.join(',')
    this.filtersValues.SectorIDs = this.filters.SectorIDs.length == 0 ? '' : this.filters.SectorIDs.join(',')
    this.filtersValues.ClusterIDs = this.filters.ClusterIDs.length == 0 ? '' : this.filters.ClusterIDs.join(',')
    this.filtersValues.DistrictIDs = this.filters.DistrictIDs.length == 0 ? '' : this.filters.DistrictIDs.join(',')
    this.filtersValues.TradeIDs = this.filters.TradeIDs.length == 0 ? '' : this.filters.TradeIDs.join(',')
    this.filtersValues.OrganizationIDs = this.filters.OrganizationIDs.length == 0 ? '' : this.filters.OrganizationIDs.join(',')
    this.filtersValues.FundingSourceIDs = this.filters.FundingSourceIDs.length == 0 ? '' : this.filters.FundingSourceIDs.join(',')
    this.filtersValues.GenderIDs = this.filters.GenderIDs.length == 0 ? '' : this.filters.GenderIDs.join(',')
    this.filtersValues.DurationIDs = this.filters.DurationIDs.length == 0 ? '' : this.filters.DurationIDs.join(',')

    if (this.IsAllSelected) {
    }
    else {
      this.GetFiltersDataByDependancy();
    }
  }

  GetFiltersDataByDependancy() {
    //if (this.filters.SchemeIDs.length != 0) {
    //  this.filters.SchemeIDs.toString();
    //  this.filtersValues.SchemeIDs = this.filters.SchemeIDs.join(',');
    //}
    this.ComSrv.postJSON('api/ROSI/GetROSIFiltersData', this.filtersValues).subscribe((d: any) => {
      if (this.filters.SchemeIDs.length == 0) {
        this.Schemes = d[0];
      }
      if (this.filters.TSPIDs.length == 0) {
        this.TSPs = d[1];
      }
      if (this.filters.PTypeIDs.length == 0) {
        this.ProgramTypes = d[2];
      }
      if (this.filters.SectorIDs.length == 0) {
        this.Sectors = d[3];
      }
      if (this.filters.ClusterIDs.length == 0) {
        this.Clusters = d[4];
      }
      if (this.filters.DistrictIDs.length == 0) {
        this.Districts = d[5];
      }
      if (this.filters.TradeIDs.length == 0) {
        this.Trades = d[6];
      }
      if (this.filters.OrganizationIDs.length == 0) {
        this.Organizations = d[7];
      }
      if (this.filters.FundingSourceIDs.length == 0) {
        this.FundingSources = d[8];
      }
      if (this.filters.GenderIDs.length == 0) {
        this.Genders = d[9];
        this.Genders = d[9].filter(g => g.GenderID != 6);

      }
      if (this.filters.DurationIDs.length == 0) {
        this.Durations = d[10];
      }

      //this.Classes = d[2];
      //this.ProgramTypes = d[3];
      //this.Sectors = d[4];
      //this.Clusters = d[5];
      //this.Districts = d[6];
      //this.Trades = d[7];
      //this.Organizations = d[8];
      //this.FundingSources = d[9];
      //this.Genders = d[10];
      //this.rosi = d[3];
      //this.rosiArray = d[3];

    }, error => this.error = error// error path
    );
  };

  FilteredRosiByScheme(SchemeID: number) {
    //var SchemeID = event.value;
    this.ComSrv.getJSON('api/ROSI/GetROSIByScheme/' + SchemeID).subscribe((d: any) => {
      this.rosiByScheme = d;
      this.rosiArray = d;

    }, error => this.error = error// error path
    );
  };


  FilteredRosiByTSP(TSPID: number) {
    this.ComSrv.getJSON('api/ROSI/GetROSIByTSP/', + TSPID).subscribe((d: any) => {
      this.rosiByTSP = d;
    }, error => this.error = error// error path
    );
  };


  FilteredRosiByClass(ClassID: number) {
    this.ComSrv.getJSON('api/ROSI/GetROSIByClass/' + ClassID).subscribe((d: any) => {
      this.rosiByClass = d;
    }, error => this.error = error// error path
    );
  };

  getROSIByFilters() {

    if (this.filters.StartDate == '' || this.filters.EndDate == '') {
      this.error = "Please select both Duration start and Duration End to calculate ROSI";
      //this.inceptionreportform.disable({ onlySelf: true });
      this.ComSrv.ShowError(this.error.toString(), "Error");
      return;
    }
    if (this.filters.SchemeIDs != [''] && this.filters.TSPIDs == ['']) {
      //if (this.filters.SchemeID != 0 && this.filters.TSPID == 0) {
      this.displayedColumnsROSI = ['ROSI', 'SchemeName', 'SchemeCode', 'SchemeType', 'Contractual', 'CancelledClasses', 'NetContractual',
        'CompletedTrainees', 'ReportedEmployed', 'VerifiedTrainees', 'AverageTrainingCost',
        //'CostPerAppendix',
        //'Appendix', 'Testing', 'Total',
        'AverageWageRate', 'OpportunityCost', 'NetIncrease', 'MarginofEmployment'];
    }
    if (this.filters.TSPIDs != ['']) {
      //if (this.filters.TSPID != 0) {
      this.displayedColumnsROSI = ['ROSI', 'TSPName',
        'CompletedTrainees', 'ReportedEmployed', 'VerifiedTrainees', 'AverageTrainingCost',
        //'CostPerAppendix', 'Appendix', 'Testing', 'Total',
        'AverageWageRate', 'OpportunityCost', 'NetIncrease'];
    }

    //var strSchemeIDs = this.filters.SchemeIDs.join(',');
    //var strTSPIDs = this.filters.TSPIDs.join(',');
    //var strPTypeIDs = this.filters.PTypeIDs.join(',');
    //var strClusterIDs = this.filters.ClusterIDs.join(',');
    //var strSectorIDs = this.filters.SectorIDs.join(',');
    //var strDistrictIDs = this.filters.DistrictIDs.join(',');
    //var strTradeIDs = this.filters.TradeIDs.join(',');
    //var strOrganizationIDs = this.filters.OrganizationIDs.join(',');
    //var strFundingSourceIDs = this.filters.FundingSourceIDs.join(',');
    //var strSchemeIDs = ''
    //var strTSPIDs = ''
    //var strPTypeIDs = ''
    //var strClusterIDs = ''
    //var strSectorIDs = ''
    //var strDistrictIDs = ''
    //var strTradeIDs = ''
    //var strOrganizationIDs = ''
    //var strFundingSourceIDs = ''


    this.ComSrv.postJSON(`api/ROSI/GetROSIByFilters`, {
      StartDate: this.filters.StartDate, EndDate: this.filters.EndDate, SchemeIDs: this.filtersValues.SchemeIDs
      , TSPIDs: this.filtersValues.TSPIDs, PTypeIDs: this.filtersValues.PTypeIDs, SectorIDs: this.filtersValues.SectorIDs
      , ClusterIDs: this.filtersValues.ClusterIDs, DistrictIDs: this.filtersValues.DistrictIDs, TradeIDs: this.filtersValues.TradeIDs,
      OrganizationIDs: this.filtersValues.OrganizationIDs, FundingSourceIDs: this.filtersValues.FundingSourceIDs, EmploymentFlag: this.filters.EmploymentFlag
    })
      //this.ComSrv.postJSON(`api/ROSI/GetROSIByFilters`, { StartDate: this.filters.StartDate, EndDate: this.filters.EndDate, SchemeID: this.filters.SchemeID, TSPID: this.filters.TSPID, PTypeID: this.filters.PTypeID, SectorID: this.filters.SectorID, ClusterID: this.filters.ClusterID, DistrictID: this.filters.DistrictID, TradeID: this.filters.TradeID, OrganizationID: this.filters.OrganizationID, FundingSourceID: this.filters.FundingSourceID, GenderID: this.filters.GenderID, EmploymentFlag: this.filters.EmploymentFlag })
      .subscribe((data: any) => {
        this.rosiByScheme = data[0];
        if (data[0].length != 0) {
          this.DateFilter = true;
        }
      },
        error => {
          this.error = error;
          this.error = "No suitable data found against selected filters to calculate ROSI";
          //this.inceptionreportform.disable({ onlySelf: true });
          this.ComSrv.ShowError(this.error.toString(), "Error");
        })
  }

  getCalculatedROSIDataSetByFilters() {

    if (this.filters.StartDate == '' || this.filters.EndDate == '') {
      this.error = "Please select both Duration start and Duration End to calculate ROSI";
      //this.inceptionreportform.disable({ onlySelf: true });
      this.ComSrv.ShowError(this.error.toString(), "Error");
      return;
    }


    this.ComSrv.postJSON(`api/ROSI/GetROSICalculationDataSetByFilters`, {
      StartDate: this.filters.StartDate, EndDate: this.filters.EndDate, SchemeIDs: this.filtersValues.SchemeIDs
      , TSPIDs: this.filtersValues.TSPIDs, PTypeIDs: this.filtersValues.PTypeIDs, SectorIDs: this.filtersValues.SectorIDs
      , ClusterIDs: this.filtersValues.ClusterIDs, DistrictIDs: this.filtersValues.DistrictIDs, TradeIDs: this.filtersValues.TradeIDs,
      OrganizationIDs: this.filtersValues.OrganizationIDs, FundingSourceIDs: this.filtersValues.FundingSourceIDs, GenderIDs: this.filtersValues.GenderIDs, DurationIDs: this.filtersValues.DurationIDs, EmploymentFlag: this.filters.EmploymentFlag
    })
      //this.ComSrv.postJSON(`api/ROSI/GetROSIByFilters`, { StartDate: this.filters.StartDate, EndDate: this.filters.EndDate, SchemeID: this.filters.SchemeID, TSPID: this.filters.TSPID, PTypeID: this.filters.PTypeID, SectorID: this.filters.SectorID, ClusterID: this.filters.ClusterID, DistrictID: this.filters.DistrictID, TradeID: this.filters.TradeID, OrganizationID: this.filters.OrganizationID, FundingSourceID: this.filters.FundingSourceID, GenderID: this.filters.GenderID, EmploymentFlag: this.filters.EmploymentFlag })
      .subscribe((data: any) => {
        this.calculatedROSIDataSet = data[0];
        if (data[0].length == 0) {
          this.error = "No suitable data found against selected filters to calculate ROSI";
          this.ComSrv.ShowError(this.error.toString(), "Error");
        }
        if (data[0].length != 0) {
          this.DateFilter = true;
        }
      },
        error => {
          this.error = error;
          this.error = "No suitable data found against selected filters to calculate ROSI";
          //this.inceptionreportform.disable({ onlySelf: true });
          this.ComSrv.ShowError(this.error.toString(), "Error");
        })
  }

  getCalculatedROSIByFilters() {

    if (this.filters.StartDate == '' || this.filters.EndDate == '') {
      this.error = "Please select both Duration start and Duration End to calculate ROSI";
      //this.inceptionreportform.disable({ onlySelf: true });
      this.ComSrv.ShowError(this.error.toString(), "Error");
      return;
    }


    this.ComSrv.postJSON(`api/ROSI/GetROSICalculationByFilters`, {
      StartDate: this.filters.StartDate, EndDate: this.filters.EndDate, SchemeIDs: this.filtersValues.SchemeIDs
      , TSPIDs: this.filtersValues.TSPIDs, PTypeIDs: this.filtersValues.PTypeIDs, SectorIDs: this.filtersValues.SectorIDs
      , ClusterIDs: this.filtersValues.ClusterIDs, DistrictIDs: this.filtersValues.DistrictIDs, TradeIDs: this.filtersValues.TradeIDs,
      OrganizationIDs: this.filtersValues.OrganizationIDs, FundingSourceIDs: this.filtersValues.FundingSourceIDs, GenderIDs: this.filtersValues.GenderIDs, DurationIDs: this.filtersValues.DurationIDs, EmploymentFlag: this.filters.EmploymentFlag
    })
      //this.ComSrv.postJSON(`api/ROSI/GetROSIByFilters`, { StartDate: this.filters.StartDate, EndDate: this.filters.EndDate, SchemeID: this.filters.SchemeID, TSPID: this.filters.TSPID, PTypeID: this.filters.PTypeID, SectorID: this.filters.SectorID, ClusterID: this.filters.ClusterID, DistrictID: this.filters.DistrictID, TradeID: this.filters.TradeID, OrganizationID: this.filters.OrganizationID, FundingSourceID: this.filters.FundingSourceID, GenderID: this.filters.GenderID, EmploymentFlag: this.filters.EmploymentFlag })
      .subscribe((data: any) => {
        this.calculatedROSI = data[0];
        if (data[0].length == 0) {
          this.error = "No suitable data found against selected filters to calculate ROSI";
          this.ComSrv.ShowError(this.error.toString(), "Error");
        }
        if (data[0].length != 0) {
          this.DateFilter = true;
        }
      },
        error => {
          this.error = error;
          this.error = "No suitable data found against selected filters to calculate ROSI";
          //this.inceptionreportform.disable({ onlySelf: true });
          this.ComSrv.ShowError(this.error.toString(), "Error");
        })
  }



  ngOnInit() {
    this.ComSrv.setTitle("ROSI");
    this.title = "Add New ";
    //this.savebtn = "Save ";
    //this.GetData();
    this.GetFiltersDataByDependancy();
    this.checkedReportedEmployment = false;
    //  this.filters.EmploymentFlag = false;
    //  this.filters.OrganizationIDs = ['1'];


  }

  startChange(event: any) {
    this.minEndDate = event.value;
  }


  SearchOrg = new FormControl('');
  SearchPType = new FormControl('');
  SearchFSource = new FormControl('');
  SearchSch = new FormControl('');
  SearchClus = new FormControl('');
  SearchSect = new FormControl('');
  SearchDist = new FormControl('');
  SearchTSP = new FormControl('');
  SearchTrd = new FormControl('');
  SearchDur = new FormControl('');
  SearchGen = new FormControl('');
  //EmptyCtrl(ev: any) {
  //  this.SearchCls.setValue('');
  //  this.SearchTSP.setValue('');
  //  this.SearchSch.setValue('');
  //}

  reset() {
    this.rosiform.reset();
    this.RosiID.setValue(0);
    this.title = "Add New ";
    this.savebtn = "Save ";
  }
  clearSchemeFilter(event: any) {
    this.filters.SchemeIDs = [''];
    this.filtersValues.SchemeIDs = '';
    this.schAllSelected = false;

  }
  clearSectorFilter(event: any) {
    this.filters.SectorIDs = [''];
    this.filtersValues.SectorIDs = '';
    this.sectAllSelected = false;

  }
  clearClusterFilter(event: any) {
    this.filters.ClusterIDs = [''];
    this.filtersValues.ClusterIDs = '';
    this.clstAllSelected = false;

  }
  clearDistrictFilter(event: any) {
    this.filters.DistrictIDs = [''];
    this.filtersValues.DistrictIDs = '';
    this.distAllSelected = false;

  }
  clearTSPFilter(event: any) {
    this.filters.TSPIDs = [''];
    this.filtersValues.TSPIDs = '';
    this.tspAllSelected = false;

  }
  clearTradeFilter(event: any) {
    this.filters.TradeIDs = [''];
    this.filtersValues.TradeIDs = '';
    this.trdAllSelected = false;

  }
  clearGenderFilter(event: any) {
    this.filters.GenderIDs = [''];
    this.filtersValues.GenderIDs = '';
    this.gendAllSelected = false;

  }
  clearFundingSourceFilter(event: any) {
    this.filters.FundingSourceIDs = [''];
    this.filtersValues.FundingSourceIDs = '';
    this.fsAllSelected = false;

  }
  clearProgramTypeFilter(event: any) {
    this.filters.PTypeIDs = [''];
    this.filtersValues.PTypeIDs = '';
    this.ptAllSelected = false;

  }
  clearDurationFilter(event: any) {
    this.filters.DurationIDs = [''];
    this.filtersValues.DurationIDs = '';
    this.durAllSelected = false;

  }
  clearOrganizationFilter(event: any) {
    this.filters.OrganizationIDs = [''];
    this.filtersValues.OrganizationIDs = '';
    //this.orgAllSelected = false;

  }

  ResetFilters() {
    //this.filters.SchemeIDs = [''];
    //this.filters.TSPIDs = [''];
    //this.filters.PTypeIDs = [''];
    //this.filters.SectorIDs = [''];
    //this.filters.ClusterIDs = [''];
    //this.filters.DistrictIDs = [''];
    //this.filters.TradeIDs = [''];

    this.filters.SchemeIDs = [];
    this.filters.TSPIDs = [];
    this.filters.PTypeIDs = [];
    this.filters.SectorIDs = [];
    this.filters.ClusterIDs = [];
    this.filters.DistrictIDs = [];
    this.filters.TradeIDs = [];
    this.filters.GenderIDs = [];
    this.filters.OrganizationIDs = [];
    this.filters.FundingSourceIDs = [];

    this.filters.StartDate = '';
    this.filters.EndDate = '';

    this.filtersValues.SchemeIDs = '';
    this.filtersValues.TSPIDs = '';
    this.filtersValues.PTypeIDs = '';
    this.filtersValues.SectorIDs = '';
    this.filtersValues.ClusterIDs = '';
    this.filtersValues.DistrictIDs = '';
    this.filtersValues.TradeIDs = '';
    this.filtersValues.GenderIDs = '';
    this.filtersValues.OrganizationIDs = '';
    this.filtersValues.FundingSourceIDs = '';

    this.filtersValues.StartDate = '';
    this.filtersValues.EndDate = '';
    this.DateFilter = false;

    this.orgAllSelected = false;
    this.fsAllSelected = false;
    this.ptAllSelected = false;
    this.schAllSelected = false;
    this.clstAllSelected = false;
    this.sectAllSelected = false;
    this.distAllSelected = false;
    this.tspAllSelected = false;
    this.trdAllSelected = false;
    this.gendAllSelected = false;
    this.durAllSelected = false;

    this.calculatedROSI = [];
    //this.rosiByScheme = null;
    this.UpdateFiltersIDs();
    //this.GetFiltersDataByDependancy();

    //this.displayedColumnsROSI = ['ROSI',
    //  'AverageWageRate', 'OpportunityCost', 'VerifiedTrainees',
    //  'AverageTrainingCost', 'CompletedTrainees' ];



  }
  radioChange(event) {
    if (event.value = true) {
      this.checkedVerifiedEmployment = true;
      this.checkedEmployment = false;
    }
    if (event.value = false) {
      this.checkedEmployment = true;
      this.checkedVerifiedEmployment = false;


      //this.TradeCode.reset();
    }
  }


  toggleEdit(row) {

    this.RosiID.setValue(row.RosiID);
    this.ClassID.setValue(row.ClassID);

    this.title = "Update ";
    this.savebtn = "Save ";
  }

  applyFilter(filterValue: string) {
    filterValue = filterValue.trim(); // Remove whitespace
    filterValue = filterValue.toLowerCase(); // Datasource defaults to lowercase matches
    this.pbteClasses.filter = filterValue;
  }

  ///Select All
  //toggleAllSelectionORG() {
  //  if (this.AllSelected) {
  //    this.organization.options.forEach((item: MatOption) => item.select());
  //  } else {
  //    this.organization.options.forEach((item: MatOption) => item.deselect());
  //  }
  //}
  toggleAllSelectionFundingS() {
    if (this.fsAllSelected) {
      //this.IsAllSelected = true;
      this.fundingSource.options.forEach((item: MatOption) => item.select());
    } else {
      //this.IsAllSelected = false;
      this.fundingSource.options.forEach((item: MatOption) => item.deselect());
    }
  }

  optionFSourceClick() {
    let newStatus = true;
    this.fundingSource.options.forEach((item: MatOption) => {
      if (!item.selected) {
        newStatus = false;
      }
    });
    this.fsAllSelected = newStatus;
    this.UpdateFiltersIDs();
  }


  toggleAllSelectionptype() {
    if (this.ptAllSelected) {
      //this.IsAllSelected = true;
      this.ptype.options.forEach((item: MatOption) => item.select());
    } else {
      this.ptype.options.forEach((item: MatOption) => item.deselect());
    }
  }
  optionPTypeClick() {
    let newStatus = true;
    this.ptype.options.forEach((item: MatOption) => {
      if (!item.selected) {
        newStatus = false;
      }
    });
    this.ptAllSelected = newStatus;
    this.UpdateFiltersIDs();
  }

  toggleAllSelectionscheme() {
    if (this.schAllSelected) {
      //this.IsAllSelected = true;
      this.scheme.options.forEach((item: MatOption) => item.select());
    }
    else {
      //this.IsAllSelected = false;
      //if (!this.IsAllSelected) {

      //}
      //else {
      this.scheme.options.forEach((item: MatOption) => item.deselect());
      /*}*/
    }
  }
  optionSchemeClick() {
    let newStatus = true;
    this.scheme.options.forEach((item: MatOption) => {
      if (!item.selected) {
        newStatus = false;
      }
    });
    this.schAllSelected = newStatus;
    this.UpdateFiltersIDs();
  }


  toggleAllSelectionssector() {
    if (this.sectAllSelected) {
      //this.IsAllSelected = true;
      this.sector.options.forEach((item: MatOption) => item.select());
    } else {
      this.sector.options.forEach((item: MatOption) => item.deselect());
    }
  }
  optionSectorClick() {
    let newStatus = true;
    this.sector.options.forEach((item: MatOption) => {
      if (!item.selected) {
        newStatus = false;
      }
    });
    this.sectAllSelected = newStatus;
    this.UpdateFiltersIDs();
  }

  toggleAllSelectionscluster() {
    if (this.clstAllSelected) {
      //this.IsAllSelected = true;
      this.cluster.options.forEach((item: MatOption) => item.select());
    } else {
      this.cluster.options.forEach((item: MatOption) => item.deselect());
    }
  }
  optionClusterClick() {
    let newStatus = true;
    this.cluster.options.forEach((item: MatOption) => {
      if (!item.selected) {
        newStatus = false;
      }
    });
    this.clstAllSelected = newStatus;
    this.UpdateFiltersIDs();
  }

  toggleAllSelectionsdistrict() {
    if (this.distAllSelected) {
      //this.IsAllSelected = true;
      this.district.options.forEach((item: MatOption) => item.select());
    } else {
      this.district.options.forEach((item: MatOption) => item.deselect());
    }
  }
  optionDistrictClick() {
    let newStatus = true;
    this.district.options.forEach((item: MatOption) => {
      if (!item.selected) {
        newStatus = false;
      }
    });
    this.distAllSelected = newStatus;
    this.UpdateFiltersIDs();
  }

  toggleAllSelectionstsp() {
    if (this.tspAllSelected) {
      //this.IsAllSelected = true;
      this.tsp.options.forEach((item: MatOption) => item.select());
    } else {
      this.tsp.options.forEach((item: MatOption) => item.deselect());
    }
  }

  optionTspClick() {
    let newStatus = true;
    this.tsp.options.forEach((item: MatOption) => {
      if (!item.selected) {
        newStatus = false;
      }
    });
    this.tspAllSelected = newStatus;
    this.UpdateFiltersIDs();
  }

  toggleAllSelectiontrade() {
    if (this.trdAllSelected) {
      //this.IsAllSelected = true;
      this.trade.options.forEach((item: MatOption) => item.select());
    } else {
      this.trade.options.forEach((item: MatOption) => item.deselect());
    }
  }

  optionTradeClick() {
    let newStatus = true;
    this.trade.options.forEach((item: MatOption) => {
      if (!item.selected) {
        newStatus = false;
      }
    });
    this.trdAllSelected = newStatus;
    this.UpdateFiltersIDs();
  }

  toggleAllSelectionduration() {
    if (this.durAllSelected) {
      //this.IsAllSelected = true;
      this.duration.options.forEach((item: MatOption) => item.select());
    } else {
      this.duration.options.forEach((item: MatOption) => item.deselect());
    }
  }

  optionDurationClick() {
    let newStatus = true;
    this.duration.options.forEach((item: MatOption) => {
      if (!item.selected) {
        newStatus = false;
      }
    });
    this.durAllSelected = newStatus;
    this.UpdateFiltersIDs();
  }

  toggleAllSelectiongender() {
    if (this.gendAllSelected) {
      //this.IsAllSelected = true;
      this.gender.options.forEach((item: MatOption) => item.select());
    } else {
      this.gender.options.forEach((item: MatOption) => item.deselect());
    }
  }

  optionGenderClick() {
    let newStatus = true;
    this.gender.options.forEach((item: MatOption) => {
      if (!item.selected) {
        newStatus = false;
      }
    });
    this.gendAllSelected = newStatus;
    this.UpdateFiltersIDs();
  }

  exportToExcelContractualROSI() {

    let filteredData = [...this.calculatedROSIDataSet]

    let data = {
      "Filters:": '',
    };

    let exportExcel: ExportExcel = {
      Title: 'Contractual ROSI Dataset Report',
      Author: '',
      Type: EnumExcelReportType.ROSI,
      Data: data,
      List1: this.populateDataContractualROSI(filteredData),
    };
    this.dialogueService.openExportConfirmDialogue(exportExcel).subscribe();
  }

  populateDataContractualROSI(data: any) {
    return data.map((item, index) => {
      return {
        "Class Code": item.ClassCode
        , "Duration (Months)": item.Duration
        , "Total Cost Per Class": item.TotalCostPerClass
        , "Employment Payout %": item.EmploymentPayout
        , "Employment Commitment %": item.OverallEmploymentCommitment
        , "Contractual Weighted Total": item.ContractualWeightedTotal
        //, "Actual Weighted Total": item.ActualWeightedTotal
        , "Verified Average Wage Rate": item.VerifiedAverageWageRate
        , "Minimum wage rate for unskilled labor": item.OpportunityCost
        , "Contractual Employment Commitment": item.ContractualEmploymentCommitment
        , "No Of Reported Trainees": item.NoOfReportedTrainees
        //, "No Of Verified Trainees": item.NoOfVerifiedTrainees
        , "Contractual CTM": item.ContractualCTM
        //, "Actual CTM": item.ActualCTM
        , "Contractual Average Duration": item.ContractualAverageDuration
        //, "Actual Average Duration": item.ActualAverageDuration
        , "No Of Contracted Trainees": item.NoOfContractedTrainees
        //, "No Of Actual Completed Trainees": item.NoOfActualCompletedTrainees
        , "Contractual ROSI Numerator": item.ContractualROSINumerator
        ////, "Contractual ROSI Denominator": this._date.transform(item.StartDate, 'dd/MM/yyyy')
        ////, "Reported Forecasted ROSI Numerator ": this._date.transform(item.EndDate, 'dd/MM/yyyy')
        , "Contractual ROSI Denominator": item.ContractualROSIDenominator
        //, "Reported Forecasted ROSI Numerator ": item.ReportedForecastedROSINumerator
        //, "Reported Forecasted ROSI Denominator ": item.ReportedForecastedROSIDenominator
        //, "Verified Forecasted ROSI Numerator ": item.VerifiedForecastedROSINumerator
        //, "Verified Forecasted ROSI Denominator": item.VerifiedForecastedROSIDenominator

        //, "Actual ROSI Numerator": item.ActualROSINumerator
        //, "Actual ROSIDenominator ": item.ActualROSIDenominator
        //, "Verified Actual ROSI Numerator": item.VerifiedActualROSINumerator
        //, "Verified Actual ROSI Denominator": item.VerifiedActualROSIDenominator
        , "Organization": item.OrganizationName
        , "FundingSource": item.FundingSourceName
        , "Program Type ": item.ProgramTypeName
        , "Scheme": item.SchemeName
        , "Sector": item.SectorName
        , "Cluster": item.ClusterName
        , "District": item.DistrictName
        , "TSP": item.TSPName
        , "Trade": item.TradeName
        , "Gender": item.GenderName

      }
    })
  }


  exportToExcelForecastedROSI() {

    let filteredData = [...this.calculatedROSIDataSet]

    let data = {
      "Filters:": '',
    };

    let exportExcel: ExportExcel = {
      Title: 'Forecasted ROSI Dataset Report',
      Author: '',
      Type: EnumExcelReportType.ROSI,
      Data: data,
      List1: this.populateDataForecastedROSI(filteredData),
    };
    this.dialogueService.openExportConfirmDialogue(exportExcel).subscribe();
  }

  populateDataForecastedROSI(data: any) {
    return data.map((item, index) => {
      return {
        "Class Code": item.ClassCode
        , "Duration (Months)": item.Duration
        , "Total Cost Per Class": item.TotalCostPerClass
        , "Actual Cost Per Class": item.ActualCostPerClass
        , "Employment Payout %": item.EmploymentPayout
        , "Employment Commitment %": item.OverallEmploymentCommitment
        , "Contractual Weighted Total": item.ContractualWeightedTotal
        , "Actual Weighted Total": item.ActualWeightedTotal
        , "Verified Average Wage Rate": item.VerifiedAverageWageRate
        , "Minimum wage rate for unskilled labor": item.OpportunityCost
        , "Contractual Employment Commitment": item.ContractualEmploymentCommitment
        , "No Of Reported Trainees": item.NoOfReportedTrainees
        , "No Of Verified Trainees": item.NoOfVerifiedTrainees
        , "Contractual CTM": item.ContractualCTM
        //, "Actual CTM": item.ActualCTM
        //, "Contractual Average Duration": item.ContractualAverageDuration
        , "Actual Average Duration": item.ActualAverageDuration
        , "No Of Contracted Trainees": item.NoOfContractedTrainees
        , "No Of Actual Completed Trainees": item.NoOfActualCompletedTrainees
        //, "Contractual ROSI Numerator": item.ContractualROSINumerator
        ////, "Contractual ROSI Denominator": this._date.transform(item.StartDate, 'dd/MM/yyyy')
        ////, "Reported Forecasted ROSI Numerator ": this._date.transform(item.EndDate, 'dd/MM/yyyy')
        //, "Contractual ROSI Denominator": item.ContractualROSIDenominator
        , "Forecasted ROSI Numerator (based on Reported Employment)": item.ReportedForecastedROSINumerator
        , "Forecasted ROSI Denominator (based on Reported Employment)": item.ReportedForecastedROSIDenominator
        , "Forecasted ROSI Numerator (based on Verified Employment)": item.VerifiedForecastedROSINumerator
        , "Forecasted ROSI Denominator (based on Verified Employment)": item.VerifiedForecastedROSIDenominator
        , "Organization": item.OrganizationName
        , "FundingSource": item.FundingSourceName
        , "Program Type ": item.ProgramTypeName
        , "Scheme": item.SchemeName
        , "Sector": item.SectorName
        , "Cluster": item.ClusterName
        , "District": item.DistrictName
        , "TSP": item.TSPName
        , "Trade": item.TradeName
        , "Gender": item.GenderName

      }
    })
  }

  exportToExcelActualROSI() {

    let filteredData = [...this.calculatedROSIDataSet]

    let data = {
      "Filters:": '',
    };

    let exportExcel: ExportExcel = {
      Title: 'Actual ROSI DataSet Report',
      Author: '',
      Type: EnumExcelReportType.ROSI,
      Data: data,
      List1: this.populateDataActualROSI(filteredData),
    };
    this.dialogueService.openExportConfirmDialogue(exportExcel).subscribe();
  }

  populateDataActualROSI(data: any) {
    return data.map((item, index) => {
      return {
        "Class Code": item.ClassCode
        , "Duration (Months)": item.Duration
        , "Total Cost Per Class": item.TotalCostPerClass
        , "Actual Cost Per Class": item.ActualCostPerClass
        , "Employment Payout %": item.EmploymentPayout
        , "Employment Commitment %": item.OverallEmploymentCommitment
        //, "Contractual Weighted Total": item.ContractualWeightedTotal
        , "Actual Weighted Total": item.ActualWeightedTotal
        , "Verified Average Wage Rate": item.VerifiedAverageWageRate
        , "Minimum wage rate for unskilled labor": item.OpportunityCost
        , "Contractual Employment Commitment": item.ContractualEmploymentCommitment
        , "No Of Reported Trainees": item.NoOfReportedTrainees
        , "No Of Verified Trainees": item.NoOfVerifiedTrainees
        , "Contractual CTM": item.ContractualCTM
        , "Actual CTM": item.ActualCTM
        , "Contractual Average Duration": item.ContractualAverageDuration
        , "Actual Average Duration": item.ActualAverageDuration
        , "No Of Contracted Trainees": item.NoOfContractedTrainees
        , "No Of Actual Completed Trainees": item.NoOfActualCompletedTrainees
        //, "Contractual ROSI Numerator": item.ContractualROSINumerator
        ////, "Contractual ROSI Denominator": this._date.transform(item.StartDate, 'dd/MM/yyyy')
        ////, "Reported Forecasted ROSI Numerator ": this._date.transform(item.EndDate, 'dd/MM/yyyy')
        //, "Contractual ROSI Denominator": item.ContractualROSIDenominator
        //, "Reported Forecasted ROSI Numerator ": item.ReportedForecastedROSINumerator
        //, "Reported Forecasted ROSI Denominator ": item.ReportedForecastedROSIDenominator
        //, "Verified Forecasted ROSI Numerator ": item.VerifiedForecastedROSINumerator
        //, "Verified Forecasted ROSI Denominator": item.VerifiedForecastedROSIDenominator

        , "Actual ROSI Numerator (based on Reported Employment)": item.ActualROSINumerator
        , "Actual ROSI Denominator (based on Reported Employment)": item.ActualROSIDenominator
        , "Actual ROSI Numerator (based on Verified Employment)": item.VerifiedActualROSINumerator
        , "Actual ROSI Denominator (based on Verified Employment)": item.VerifiedActualROSIDenominator
        , "Organization": item.OrganizationName
        , "FundingSource": item.FundingSourceName
        , "Program Type ": item.ProgramTypeName
        , "Scheme": item.SchemeName
        , "Sector": item.SectorName
        , "Cluster": item.ClusterName
        , "District": item.DistrictName
        , "TSP": item.TSPName
        , "Trade": item.TradeName
        , "Gender": item.GenderName

      }
    })
  }

  get RosiID() { return this.rosiform.get("RosiID"); }
  get ClassID() { return this.rosiform.get("ClassID"); }
  get SchemeID() { return this.rosiform.get("SchemeID"); }
  get TSPID() { return this.rosiform.get("TSPID"); }

}
export class ROSIModel extends ModelBase {
  RosiID: number;
}


//export interface IQueryFilters {
//  SchemeID: number;
//  TSPID: number;
//  //ClassID: number;
//  PTypeID: number;
//  SectorID: number;
//  ClusterID: number;
//  DistrictID: number;
//  TradeID: number;
//  OrganizationID: number;
//  FundingSourceID: number;
//  GenderID: number;
//  StartDate: string;
//  EndDate: string;
//  EmploymentFlag: boolean;
//}


export interface IQueryFilters {
  SchemeIDs: string[];
  TSPIDs: string[];
  //ClassID: string;
  PTypeIDs: string[];
  SectorIDs: string[];
  ClusterIDs: string[];
  DistrictIDs: string[];
  TradeIDs: string[];
  OrganizationIDs: string[];
  FundingSourceIDs: string[];
  GenderIDs: string[];
  DurationIDs: string[];
  StartDate: string;
  EndDate: string;
  EmploymentFlag: boolean;
  ActualContractualFlag: boolean;
}

export interface IDataQueryFilters {
  SchemeIDs: string;
  TSPIDs: string;
  //ClassID: string;
  PTypeIDs: string;
  SectorIDs: string;
  ClusterIDs: string;
  DistrictIDs: string;
  TradeIDs: string;
  OrganizationIDs: string;
  FundingSourceIDs: string;
  GenderIDs: string;
  DurationIDs: string;
  StartDate: string;
  EndDate: string;
  EmploymentFlag: boolean;
  ActualContractualFlag: boolean;
}

//export interface IQueryFilters {
//  SchemeIDs: any[];
//  TSPIDs: any[];
//  //ClassID: number;
//  PTypeIDs: any[];
//  SectorIDs: any[];
//  ClusterIDs: any[];
//  DistrictIDs: any[];
//  TradeIDs: any[];
//  OrganizationIDs: any[];
//  FundingSourceIDs: any[];
//  EmploymentFlag: boolean;
//  StartDate: Date;
//  EndDate: Date;

//}



