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
import { Router } from '@angular/router';


@Component({
  selector: 'app-potential-trainees-enrollment',
  templateUrl: './potential-trainees-enrollment.component.html',
  styleUrls: ['./potential-trainees-enrollment.component.scss'],
  providers: [GroupByPipe, DatePipe]

})
export class PotentialTraineesEnrollmentComponent implements OnInit, AfterViewInit {
  title: string; savebtn: string;
  displayedColumns = ['Action','ClassCode',
    'TradeName', 'TraineeName', 'TraineeCNIC',
    'TraineeEmail', 'TraineePhone', 'GenderName', 'DistrictName','TehsilName'
  ];
  filters: IPotentialTraineesListFilter = { ClassID: 0, UserID: 0 };

  filtersList: any;
  districtsList: any;
  tehsilsList: any;

  potentialTraineesList: MatTableDataSource<any>;
  formrights: UserRightsModel;
  EnText = 'Potential Trainees Enrollment';
  error: string;
  currentUser: UsersModel;
  userid: number;

  SearchClassList = new FormControl('',);

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
  ClassFilter = new FormControl(0);


  constructor(private fb: FormBuilder, private http: CommonSrvService, private groupByPipe: GroupByPipe,
    public dialogueService: DialogueService, private router: Router, private _date: DatePipe,) {
    this.potentialTraineesList = new MatTableDataSource([]);
  }

  ngOnInit() {
    this.http.setTitle('Potential Trainees Enrollment');
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
    this.SearchClassList.setValue('');
  }
 
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
        this.filters.ClassID = this.classFilter.value
        this.filters.UserID = this.userid;
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


  routeToRegistration(row: any) {
    if (this.currentUser.UserLevel == EnumUserLevel.TSP) {
      //this.checkTSPColor(row.ClassStatusID);


      var potentialTraineeObject = JSON.stringify(row);
      var potentialTraineeObject1 = JSON.parse(potentialTraineeObject);

      sessionStorage.setItem('potentialTrainee', potentialTraineeObject);
    }
    this.http.getJSON(`api/InceptionReport/GetInceptionReportByClass?classID=${row.ClassID}`)
      .subscribe(data => {

        //if (this.blacklistedTSPwithRed || this.blacklistedTSPwithBlack) {
        //  return;
        //}

        if (data[0].length > 0) {
          this.router.navigateByUrl(`/registration/trainee/${row.ClassID}`);
        } else {
          this.http.ShowError("Please submit inception report first, before registration.");
        }
      }, error => {
        this.error = error;
      });

  }
}

export interface IPotentialTraineesListFilter {
  ClassID: number;
  UserID: number;
}
