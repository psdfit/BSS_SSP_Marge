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
  selector: 'app-inception-report-list',
  templateUrl: './inception-report-list.component.html',
  styleUrls: ['./inception-report-list.component.scss'],
  providers: [GroupByPipe, DatePipe]

})
export class InceptionReportListComponent implements OnInit, AfterViewInit {
  title: string; savebtn: string;
  displayedColumns = ['SchemeName', 'TSPName', 'ClassCode',
    'CenterName', 'AddressOfTrainingCenterTheoratical', 'InchargeNameTheoratical',
    'InchargeContactTheoratical', 'AddressOfTrainingCenterPractical', 'InchargeNamePractical',
    'InchargeContactPractical', 'NameOfAuthorizedPerson', 'MobileContactOfAuthorizedPerson',
    'EmailOfAuthorizedPerson', 'TehsilName', 'DistrictName',
    'TradeName', 'Batch', 'ClassStartTime','ClassEndTime',
    'StartDate', 'EndDate','EnrolledTrainees',
    'Shift', 'GenderName', 'TrainingDaysNo','TrainingDays',
    'InstructorInfo','ClassTotalHours'];
  filters: IInceptionReportListFilter = { SchemeID: 0, ClassID: 0, TSPID: 0, UserID: 0 };

  inceptionReportList: MatTableDataSource<any>;
  formrights: UserRightsModel;
  EnText = 'Inception Report List';
  error: string;
  currentUser: UsersModel;
  userid: number;

  SearchSchemeList = new FormControl('',);
  SearchTSPList = new FormControl('',);
  SearchClassList = new FormControl('',);

  TSPDetail = [];
  classesArray: any[];

  Scheme: any[];

  query = {
    order: 'IncepReportID',
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

  constructor(private fb: FormBuilder, private http: CommonSrvService, private groupByPipe: GroupByPipe,
    public dialogueService: DialogueService, private _date: DatePipe,) {
    this.inceptionReportList = new MatTableDataSource([]);
  }

  ngOnInit() {
    this.http.setTitle('Inception Report List');
    this.title = 'Add New ';
    this.savebtn = 'Save ';
    this.currentUser = this.http.getUserDetails();
    this.userid = this.currentUser.UserID;
    this.getSchemes();
    // this.GetData();
  }
  ngAfterViewInit() {
    this.getInceptionReportList();
  }
  EmptyCtrl() {
    this.SearchClassList.setValue('');
    this.SearchTSPList.setValue('');
    this.SearchSchemeList.setValue('');
  }
  getSchemes() {
    this.http.getJSON(`api/Dashboard/FetchSchemes`)
    .subscribe(
      (d: any) => {
        this.Scheme = d;
      }, error => this.error = error
    );
  }
  getTSPDetailByScheme(schemeId: number) {
    this.tspFilter.setValue(0);
    this.classFilter.setValue(0);
    this.classesArray = [];
    this.http.getJSON(`api/Dashboard/FetchTSPsByScheme?SchemeID=` + schemeId)
      .subscribe(data => {
        this.TSPDetail = (data as any[]);
      }, error => {
        this.error = error;
      })
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

  getInceptionReportList() {
    // if (this.currentUser.UserLevel == EnumUserLevel.TSP) {
    //  this.http.getJSON(`api/InceptionReport/GetFilteredInceptionReport/filter?filter=${this.filters.SchemeID}&filter=${this.filters.TSPID}&filter=${this.filters.ClassID}&filter=${this.userid}&filter=${this.http.OID.value}`)
    //    .subscribe((data: any) => {
    //      debugger;
    //      this.inceptionReportList = new MatTableDataSource(data[0]);
    //      //this.Scheme = this.Scheme.filter(x => this.mastersheetArray.map(y => y.SchemeID).includes(x.SchemeID))

    //      this.inceptionReportList.paginator = this.paginator;
    //      this.inceptionReportList.sort = this.sort;
    //      //    //
    //    },
    //      error => {
    //        this.error = error;
    //      })
    // } else {
    //  this.http.getJSON(`api/InceptionReport/GetFilteredInceptionReport/filter?filter=${this.filters.SchemeID}&filter=${this.filters.TSPID}&filter=${this.filters.ClassID}&filter=${this.filters.UserID}&filter=${this.http.OID.value}`)
    //    .subscribe((data: any) => {
    //      this.inceptionReportList = new MatTableDataSource(data[0]);

    //      this.inceptionReportList.paginator = this.paginator;
    //      this.inceptionReportList.sort = this.sort;
    //      //    //
    //    },
    //      error => {
    //        this.error = error;
    //      })
    // }
    this.initPagedData();
  }

  exportToExcel() {
    this.http.getJSON(`api/InceptionReport/GetFilteredInceptionReport/filter?filter=${this.filters.SchemeID}&filter=${this.filters.TSPID}&filter=${this.filters.ClassID}&filter=${this.filters.UserID}&filter=${this.http.OID.value}`)
      .subscribe((data1: any) => {
        const filteredData = [...data1[0]]

        const data = {
          'Filters:': '',
        };

        const exportExcel: ExportExcel = {
          Title: 'Training Inception Report',
          Author: this.currentUser.FullName,
          Type: EnumExcelReportType.InceptionReport,
          Data: data,
          List1: this.populateData(filteredData),
        };
        this.dialogueService.openExportConfirmDialogue(exportExcel).subscribe();
      },
        error => {
          this.error = error;
        })
  }

  populateData(data: any) {
    return data.map((item,index) => {
      return {
        'Sr#': ++index
        , Scheme: item.SchemeName
        , TSP: item.TSPName
        , 'Class Code': item.ClassCode
        , 'Center Name': item.CenterName
        , 'Address Training Center (Theoratical)': item.AddressOfTrainingCenterTheoratical
        , 'Incharge Name (Theoratical)': item.InchargeNameTheoratical
        , 'Incharge Name Contact (Theoratical)': item.InchargeContactTheoratical
        , 'Address Training Center (Practical)': item.AddressOfTrainingCenterPractical
        , 'Incharge Name (Practical)': item.InchargeNamePractical
        , 'Incharge Contact (Pratical)': item.InchargeContactPractical
        , 'Tehsil (Practical)': item.TehsilName
        , 'District (Pratical)': item.DistrictName
        , Trade: item.TradeName
        , Batch: item.Batch
        , 'Number of Trainees': item.EnrolledTrainees
        , Gender: item.GenderName
        , 'Expected Start Date': this._date.transform(item.StartDate, 'dd/MM/yyyy')
        , 'Expected End Date': this._date.transform(item.EndDate, 'dd/MM/yyyy')
        , Shift: item.Shift
        , 'Class Start Time': this._date.transform(item.ClassStartTime, 'h:mm a')
        , 'Class End Time': this._date.transform(item.ClassEndTime, 'h:mm a')
        , 'Name of Authorized Person': item.NameOfAuthorizedPerson
        , 'Mobile Contact of Authorized Person': item.MobileContactOfAuthorizedPerson
        , 'Email of Authorized Person': item.EmailOfAuthorizedPerson
        , 'Training Days No': item.TrainingDaysNo
        , 'Training Days': item.TrainingDays
        , 'Instructor Info': item.InstructorInfo
        , 'Min Study Hours': item.ClassTotalHours

      }
    })
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
        this.filters.SchemeID = this.schemeFilter.value
        this.filters.TSPID = this.tspFilter.value
        this.filters.ClassID = this.classFilter.value
        return this.getPagedData(pagedModel, this.filters);
      })).subscribe(data => {
        this.inceptionReportList = new MatTableDataSource(data[0]);
        this.resultsLength = data[1].TotalCount;
      });
  }
  getPagedData(pagingModel, filterModel) {
    return this.http.postJSON('api/InceptionReport/GetFilteredInceptionReportPaged', { pagingModel, filterModel });
  }
  openClassJourneyDialogue(data: any): void
  {
		this.dialogueService.openClassJourneyDialogue(data);
  }
}

export interface IInceptionReportListFilter {
  SchemeID: number;
  TSPID: number;
  ClassID: number;
  UserID: number;
}
