import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from '../../../environments/environment';
import { MatPaginator } from '@angular/material/paginator';
import { SearchFilter, ExportExcel } from '../../shared/Interfaces';
import { MatSort } from '@angular/material/sort';
import { CommonSrvService } from '../../common-srv.service';
import { FormControl } from '@angular/forms';
import { merge } from 'rxjs';
import { startWith, switchMap } from 'rxjs/operators';
import { DatePipe } from '@angular/common';
import { GroupByPipe } from '../../pipes/GroupBy.pipe';
import { UsersModel } from '../../master-data/users/users.component';
import { EnumExcelReportType, EnumUserLevel } from '../../shared/Enumerations';
import { DialogueService } from '../../shared/dialogue.service';

@Component({
  selector: 'traineereports',
  templateUrl: './gsr.component.html',
  styleUrls: ['./gsr.component.scss'],
  providers: [GroupByPipe, DatePipe]
})
export class GuruRportsComponent implements OnInit {
  environment = environment;
  gsrDatasource: any[];
  displayedColumns = ['Sr', 'TraineeCode', 'TraineeName', 'FatherName', 'TraineeCNIC', 'TSPName', 'ClassCode', 'GuruName', 'GuruCnic', 'GuruContactNumber'];
  schemeArray = [];
  tspDetailArray = [];
  classesArray: any[];
  filters: SearchFilter = { SchemeID: 0, TSPID: 0, ClassID: 0, TraineeID: 0, OID: this.commonService.OID.value, SelectedColumns: [] };
  @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: false }) sort: MatSort;
  error = '';
  schemeFilter = new FormControl(0);
  tspFilter = new FormControl(0);
  classFilter = new FormControl(0);
  SearchSch = new FormControl('');
  SearchTSP = new FormControl('');
  SearchCls = new FormControl('');
  resultsLength: number;
  currentUser: UsersModel;
  enumUserLevel = EnumUserLevel;
  GSRDataModelKeys = Object.keys(new GSRDataModel());

  constructor(
    private commonService: CommonSrvService,
    private datePipe: DatePipe,
    private groupByPipe: GroupByPipe,
    public dialogueService: DialogueService
  ) { }

  ngOnInit(): void {
    this.commonService.setTitle('Trainee Status Report');
    this.currentUser = this.commonService.getUserDetails();
    this.schemeFilter.valueChanges.subscribe(value => {
      if (this.currentUser.UserLevel === this.enumUserLevel.TSP) {
        this.getDependantFilters();
      } else {
        this.getTSPDetailByScheme(value);
      }
    });
    this.tspFilter.valueChanges.subscribe(value => { this.getClassesByTsp(value) });
  }

  ngAfterViewInit() {
    this.commonService.OID.subscribe(OID => {
      this.schemeFilter.setValue(0);
      this.tspFilter.setValue(0);
      this.classFilter.setValue(0);
      this.filters.OID = OID;
      this.getData();
      this.initPagedData();
    });
  }

  EmptyCtrl(Ev: any) {
    this.SearchCls.setValue('');
    this.SearchTSP.setValue('');
    this.SearchSch.setValue('');
  }

  getData() {
    this.commonService.getJSON(`api/TSRLiveData/GetSchemesForGSR?OID=${this.commonService.OID.value}`)
      .subscribe((d: any) => {
        this.schemeArray = d.Schemes;
      }, error => this.error = error);
  }

  getSchemesByOrg(oid: number) {
    this.commonService.getJSON(`api/Dashboard/FetchSchemes`)
      .subscribe(data => {
        this.schemeArray = (data as any[]);
      }, error => {
        this.error = error;
      });
  }

  getTSPDetailByScheme(schemeId: number) {
    this.tspFilter.setValue(0);
    this.classFilter.setValue(0);
    this.commonService.getJSON(`api/Dashboard/FetchTSPsByScheme?SchemeID=${schemeId}`)
      .subscribe(data => {
        this.tspDetailArray = (data as any[]);
      }, error => {
        this.error = error;
      });
  }

  getClassesByTsp(tspId: number) {
    this.classFilter.setValue(0);
    this.commonService.getJSON(`api/Dashboard/FetchClassesByTSP?TspID=${tspId}`)
      .subscribe(data => {
        this.classesArray = (data as any[]);
      }, error => {
        this.error = error;
      });
  }

  applyFilter(filterValue: string) {
    filterValue = filterValue.trim().toLowerCase();
    // this.gsrDatasource.filter = filterValue;
  }

  initPagedData() {
    this.sort.sortChange.subscribe(() => this.paginator.pageIndex = 0);
    this.paginator.pageSize = 10;
    merge(this.sort.sortChange, this.paginator.page, this.schemeFilter.valueChanges, this.tspFilter.valueChanges, this.classFilter.valueChanges)
      .pipe(
        startWith({}),
        switchMap(() => {
          const pagedModel = {
            PageNo: this.paginator.pageIndex + 1,
            PageSize: this.paginator.pageSize,
            SortColumn: this.sort.active,
            SortOrder: this.sort.direction,
            SearchColumn: '',
            SearchValue: '',
          };
          this.filters.SchemeID = this.schemeFilter.value;
          this.filters.TSPID = this.tspFilter.value;
          this.filters.ClassID = this.classFilter.value;
          return this.getPagedData(pagedModel, this.filters);
        })
      ).subscribe(data => {
        this.gsrDatasource = data[0];
        this.resultsLength = data[1].TotalCount;
      });
  }

  // getPagedData(pagingModel, filterModel) {
  //   return this.commonService.postJSON('api/TSRLiveData/RD_TSRPaged', { pagingModel, filterModel });
  // }

  getPagedData(pagingModel, filterModel) {
    return this.commonService.postJSON('api/TSRLiveData/RD_GSRPaged', { pagingModel, filterModel });
  }

  exportToExcel() {
    this.filters.SchemeID = this.schemeFilter.value;
    this.filters.TSPID = this.tspFilter.value;
    this.filters.ClassID = this.classFilter.value;

    const exportExcel: ExportExcel = {
      Title: 'Guru Status Report',
      Author: this.currentUser.FullName,
      Type: EnumExcelReportType.TSR,
      Data: {},
      List1: [],
      SearchFilters: this.filters,
      DataModel: new GSRDataModel(),
      LoadDataAsync: this.loadDataAsync
    };

    this.dialogueService.openExportConfirmDialogue(exportExcel).subscribe();
  }

  async loadDataAsync(that: any) {
    await that.commonService.postJSON(`api/TSRLiveData/GetFilteredGSRData`, that.input.SearchFilters)
      .toPromise()
      .then((responseData: any[]) => {
        that.input.Data = {
          'Filters:': '',
          TraineeStatus: 'All',
          // 'Scheme(s)': that.groupByPipe.transform(responseData, 'SchemeName').map(x => x.key).join(','),
          'TSP(s)': that.groupByPipe.transform(responseData, 'TSPName').map(x => x.key).join(','),
          TraineeImagesAdded: true
        };
        that.input.List1 = responseData.map((item, index) => {
          const obj = new GSRDataModel();
          obj['Sr#'] = ++index;
          obj['Training Service Provider'] = item.TSPName;
          obj['Trainee ID'] = item.TraineeCode;
          obj['Trainee Name'] = item.TraineeName;
          obj['Father\'s Name'] = item.FatherName;
          obj['Trainee CNIC'] = item.TraineeCNIC;
          obj.ClassCode = item.ClassCode;
          obj['Guru Name'] = item.GuruName;
          obj['Guru CNIC'] = item.GuruCNIC;
          obj['Guru Contact Number'] = item.GuruContactNumber;
          return obj;
        });
      });
  }

  getDependantFilters() {
    if (this.currentUser.UserLevel === this.enumUserLevel.TSP) {
      this.getClassesBySchemeFilter();
    } else {
      this.getTSPDetailByScheme(this.filters.SchemeID);
    }
  }

  getClassesBySchemeFilter() {
    this.filters.ClassID = 0;
    this.filters.TraineeID = 0;
    this.commonService.getJSON(`api/Dashboard/FetchClassesBySchemeUser?SchemeID=${this.schemeFilter.value}&UserID=${this.currentUser.UserID}`)
      .subscribe(data => {
        this.classesArray = (data as any[]);
      }, error => {
        this.error = error;
      });
  }

  openTraineeJourneyDialogue(data: any): void {
    this.dialogueService.openTraineeJourneyDialogue(data);
  }
}

export class GSRDataModel {
  'Sr#': any = 'Sr';
  'Training Service Provider': any = 'TSPName';
  'Trainee ID': any = 'TraineeCode';
  'Trainee Name': any = 'TraineeName';
  'Father\'s Name': any = 'FatherName';
  'Trainee CNIC': any = 'TraineeCNIC';
  'ClassCode': any = 'ClassCode';
  'Guru Name': any = 'GuruName';
  'Guru CNIC': any = 'GuruCNIC';
  'Guru Contact Number': any = 'GuruContactNumber';
}
