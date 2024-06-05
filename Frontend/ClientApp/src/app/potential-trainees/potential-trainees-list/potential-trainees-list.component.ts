import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from '../../../environments/environment';
import { FormGroup, FormBuilder, Validators, FormControl } from '@angular/forms';
import { CommonSrvService } from '../../common-srv.service';
import { ModelBase } from '../../shared/ModelBase';
import { UserRightsModel } from '../../master-data/users/users.component';
import { Observable } from 'rxjs';
import { UsersModel } from '../../master-data/users/users.component';
import { EnumUserLevel, EnumExcelReportType } from '../../shared/Enumerations';
import { ExportExcel } from '../../shared/Interfaces';
import { GroupByPipe } from 'angular-pipes';
import { DatePipe } from '@angular/common';
import { DialogueService } from '../../shared/dialogue.service';
import { merge } from 'rxjs';
import { startWith, switchMap } from 'rxjs/operators';

@Component({
  selector: 'app-potential-trainees-list',
  templateUrl: './potential-trainees-list.component.html',
  styleUrls: ['./potential-trainees-list.component.scss'],
  providers: [GroupByPipe, DatePipe]

})
export class PotentialTraineesListComponent implements OnInit, AfterViewInit {
  title: string; savebtn: string;
  displayedColumns = ['ClassCode',
    'TradeName', 'TraineeName', 'TraineeCNIC',
    'TraineeEmail', 'TraineePhone', 'GenderName', 'DistrictName','TehsilName'
  ];
  filters: IPotentialTraineesListFilter = { TradeID: 0, DistrictID: 0, TehsilID: 0, UserID: 0 };

  filtersList: any;
  districtsList: any;
  tehsilsList: any;

  potentialTraineesList: MatTableDataSource<any>;
  formrights: UserRightsModel;
  EnText = 'Potential Trainees List';
  error: string;
  currentUser: UsersModel;
  userid: number;

  SearchTradeList = new FormControl('',);
  SearchDistrictList = new FormControl('',);
  SearchTehsilList = new FormControl('',);

  TSPDetail = [];
  classesArray: any[];

  Scheme: any[];

  query = {
    order: 'PotentialTraineeID',
    limit: 5,
    page: 1
  };
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  working: boolean;

  // Pagination\\
  resultsLength: number;
  schemeFilter = new FormControl(0);
  tspFilter = new FormControl(0);
  classFilter = new FormControl(0);
  tradeFilter = new FormControl(0);
  districtFilter = new FormControl(0);
  tehsilFilter = new FormControl(0);

  constructor(private fb: FormBuilder, private http: CommonSrvService, private groupByPipe: GroupByPipe,
    public dialogueService: DialogueService, private _date: DatePipe,) {
    this.potentialTraineesList = new MatTableDataSource([]);
  }

  ngOnInit() {
    this.http.setTitle('Potential Trainees List');
    this.title = 'Add New ';
    this.savebtn = 'Save ';
    this.currentUser = this.http.getUserDetails();
    this.userid = this.currentUser.UserID;
    this.filtersList = [];
    this.getFiltersData();
    // this.GetData();
  }
  ngAfterViewInit() {
    this.getPotentialTraineesList();

    this.filtersList = this.potentialTraineesList.filteredData;
  }
  EmptyCtrl() {
    this.SearchTradeList.setValue('');
    this.SearchDistrictList.setValue('');
    this.SearchTehsilList.setValue('');
  }
  //getSchemes() {
  //  this.http.getJSON(`api/Dashboard/FetchSchemes`)
  //  .subscribe(
  //    (d: any) => {
  //      this.Scheme = d;
  //    }, error => this.error = error
  //  );
  //}
  //getTSPDetailByScheme(schemeId: number) {
  //  this.tspFilter.setValue(0);
  //  this.classFilter.setValue(0);
  //  this.classesArray = [];
  //  this.http.getJSON(`api/Dashboard/FetchTSPsByScheme?SchemeID=` + schemeId)
  //    .subscribe(data => {
  //      this.TSPDetail = (data as any[]);
  //    }, error => {
  //      this.error = error;
  //    })
  //}
  getClassesByTsp(tspId: number) {
    this.classFilter.setValue(0);
    this.http.getJSON(`api/Dashboard/FetchClassesByTSP?TspID=${tspId}`)
      .subscribe(data => {
        this.classesArray = (data as any[]);
      }, error => {
        this.error = error;
      })
  }

  getFiltersData() {
    if (this.currentUser.UserLevel === EnumUserLevel.TSP) {
      var currentUserID = this.userid;
      this.http.getJSON(`api/PotentialTrainees/GetPotentialTraineesFiltersByUser/` + this.userid)
        .subscribe(data => {
          this.filtersList = data[0];
        }, error => {
          this.error = error;
        })
    }
    else {
      var currentUserID = 0;
      this.http.getJSON(`api/PotentialTrainees/GetPotentialTraineesFiltersByUser/` + currentUserID)
        .subscribe(data => {
          this.filtersList = data[0];
        }, error => {
          this.error = error;
        })
    }
  }

  getPotentialTraineesList() {
    this.initPagedData();
  }

  initPagedData() {
    this.sort.sortChange.subscribe(() => this.paginator.pageIndex = 0);
    this.paginator.pageSize = 5;
    merge(this.sort.sortChange, this.paginator.page, this.schemeFilter.valueChanges, this.tspFilter.valueChanges, this.classFilter.valueChanges).pipe(
      startWith({}),
      switchMap(() => {
        const pagedModel = {
          PageNo: this.paginator.pageIndex + 1
          , PageSize: this.paginator.pageSize
          , SortColumn: this.sort.active
          , SortOrder: this.sort.direction
          , SearchColumn: ''
          , SearchValue: ''
        };
        this.filters.TradeID = this.tradeFilter.value
        this.filters.DistrictID = this.districtFilter.value
        this.filters.TehsilID = this.tehsilFilter.value
        return this.getPagedData(pagedModel, this.filters);
      })).subscribe(data => {
        this.potentialTraineesList = new MatTableDataSource(data[0]);
        this.resultsLength = data[1].TotalCount;
      });
  }
  getPagedData(pagingModel, filterModel) {
    return this.http.postJSON('api/PotentialTrainees/GetFilteredPotentialTraineesPaged', { pagingModel, filterModel });
  }
  openClassJourneyDialogue(data: any): void
  {
		this.dialogueService.openClassJourneyDialogue(data);
  }
}

export interface IPotentialTraineesListFilter {
  TradeID: number;
  DistrictID: number;
  TehsilID: number;
  UserID: number;
}
