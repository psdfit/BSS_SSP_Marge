import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormControl } from '@angular/forms';
import { CommonSrvService } from '../../common-srv.service';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { DialogueService } from '../../shared/dialogue.service';
import { EnumApprovalProcess, EnumProgramCategory, EnumExcelReportType } from '../../shared/Enumerations';
import { UserRightsModel, UsersModel } from '../../master-data/users/users.component';
import { ExportExcel } from '../../shared/Interfaces';
import { GroupByPipe } from 'angular-pipes';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-cr-trainee-approvals',
  templateUrl: './cr-trainee-approvals.component.html',
  styleUrls: ['./cr-trainee-approvals.component.scss'],
  providers: [GroupByPipe, DatePipe]

})
export class TraineeChangeRequestApprovalsComponent implements OnInit {
  displayedColumnsTSP = ["Action", 'TSPName', 'Address', 'HeadName', 'HeadDesignation', 'HeadLandline', 'HeadEmail', 'CPName', 'CPDesignation', 'CPLandline', 'CPEmail',
    'BankName', 'BankAccountNumber', 'AccountTitle', 'BankBranch'];
  //schemes: MatTableDataSource<any>;
  // Modified: Changed SchemeID, TSPID, ClassID to arrays for multi-select support
  filters: ICRTraineeListFilter = {
    Schemes: [],
    TSPs: [],
    Classes: [],
    UserID: 0,
    KAMID: 0,
    FundingCategoryID: 0,
    startDate: null,
    endDate: null
  };

  SearchSchemeList = new FormControl('',);
  SearchTSPList = new FormControl('',);
  SearchClassList = new FormControl('',);

  TSPDetail = [];
  classesArray: any[];

  Scheme: any[];

  trainees: [];
  currentTrainee: [];
  currentUser: UsersModel;

  userid: number;
  KAMID: number;

  ActiveFormApprovalID: number;
  ChosenTradeID: number;
  title: string;
  savebtn: string;
  formrights: UserRightsModel;
  EnText: string = "";
  error: String;
  query = {
    order: 'TraineeChangeRequestID',
    limit: 5,
    page: 1
  };
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  working: boolean;

  kamFilter = new FormControl(0);
  SearchKam = new FormControl('');
  Kam = [];

  fundingCategoryFilter = new FormControl();
  SearchFundingCategory = new FormControl();
  Project: any[] = [];  // This should be populated by your API call




  constructor(private http: CommonSrvService, private dialogue: DialogueService, private _date: DatePipe) {
    //this.schemes = new MatTableDataSource([]);
    this.formrights = http.getFormRights();
  }

  ngOnInit(): void {
    this.http.setTitle("Un-Verified Trainee Change Request");
    this.title = "";
    this.savebtn = "Approve";
    this.GetSubmittedTrainees();
    this.currentUser = this.http.getUserDetails();
    this.userid = this.currentUser.UserID;
    //this.GetData();
    this.getSchemes();
    this.getKam();
    this.getFundingCategories();
  }

  ngAfterViewInit(): void {
    this.kamFilter.valueChanges.subscribe(value => { this.filters.KAMID = value; this.GetSubmittedTrainees(); });
    this.fundingCategoryFilter.valueChanges.subscribe(value => { this.filters.FundingCategoryID = value; this.GetSubmittedTrainees(); });
  }

  onDateChange() {
    this.GetSubmittedTrainees();
  }


  EmptyCtrl() {
    this.SearchClassList.setValue('');
    this.SearchTSPList.setValue('');
    this.SearchSchemeList.setValue('');
    this.SearchFundingCategory.setValue('');
  }
  //GetSubmittedSchemesForMyID() {
  //  this.http.getJSON('api/Scheme/GetSubmittedSchemes').subscribe((d: any) => {
  //    this.schemes = d;
  //    //this.schemes.paginator = this.paginator;
  //    //this.schemes.sort = this.sort;
  //  },
  //    error => this.error = error // error path
  //    , () => {
  //      this.working = false;
  //    });
  //  }
  getSchemes() {
    this.http.postJSON('api/Scheme/FetchSchemeByUser', this.filters).subscribe(
      (d: any) => {
        this.Scheme = d;
      }, error => this.error = error
    );
  }

  // Modified: Updated to handle array of scheme IDs
  getTSPDetailByScheme(schemeIds: number[]) {
    this.classesArray = [];
    // Modified: Join array of scheme IDs for API call
    const schemeIdParam = schemeIds.length > 0 ? schemeIds.join(',') : '0';
    this.http.getJSON(`api/Dashboard/FetchTSPsByMultipleSchemes?SchemeID=${schemeIdParam}`)
      .subscribe(data => {
        this.TSPDetail = (data as any[]);
      }, error => {
        this.error = error;
      });
  }

  // Modified: Updated to handle array of TSP IDs
  getClassesByTsp(tspIds: number[]) {
    // Modified: Join array of TSP IDs for API call
    const tspIdParam = tspIds.length > 0 ? tspIds.join(',') : '0';
    this.http.getJSON(`api/Dashboard/FetchMultipleClassesByTSP?TspID=${tspIdParam}`)
      .subscribe(data => {
        this.classesArray = (data as any[]);
      }, error => {
        this.error = error;
      });
  }


  // added KAM api endpoint:
  getKam() {
    this.http.getJSON('api/KAMAssignment/RD_KAMAssignmentForFilters').subscribe(
      (d: any) => {
        this.error = '';
        this.Kam = d;
      },
      error => {
        this.error = error;
      }
    );
  }

  getFundingCategories() {
    this.http.getJSON(`api/Scheme/GetScheme?OID=${this.http.OID.value}`).subscribe((d: any) => {
      this.Project = d[4];
    },
      (error) => {
        this.error = error.error;
        this.http.ShowError(error.error + '\n' + error.message);
      } // error path
    );
  }

  GetSubmittedTrainees() {
    // Modified: Convert array filters to comma-separated strings for backend compatibility
    const modFilters = {
      Process_Key: "CR_TRAINEE_UNVERIFIED",
      UserID: this.userid,
      OID: this.http.OID.value,
      Schemes: Array.isArray(this.filters.Schemes) ? this.filters.Schemes.join(',') : '0',
      TSPs: Array.isArray(this.filters.TSPs) ? this.filters.TSPs.join(',') : '0',
      Classes: Array.isArray(this.filters.Classes) ? this.filters.Classes.join(',') : '0',
      KAMID: this.filters.KAMID,
      StartDate: this.filters.startDate,
      EndDate: this.filters.endDate,
      FundingCategoryID: this.filters.FundingCategoryID
    };
    this.http.postJSON('api/TraineeChangeRequest/GetTraineeChangeRequest/', modFilters).subscribe((d: any) => {
      this.trainees = d[0];
    },
      error => this.error = error // error path
      , () => {
        this.working = false;
      });
  }

  GetCurrentTraineeByID(r) {
    if (r.currentTrainee) {
      r.currentTrainee = null;
      return;
    }
    this.http.postJSON('api/TraineeProfile/RD_TraineeProfileBy/', { TraineeID: r.TraineeID }).subscribe((d: any) => {
      r.currentTrainee = d;
    });
  }

  exportToExcel(name?: string) {
    let filteredData = [...this.trainees]


    let data = {
      //"Filters:": '',
      //"Scheme(s)": '',//this.groupByPipe.transform(filteredData, 'Scheme').map(x => x.key).join(','),
      //"TSP(s)": '',//this.groupByPipe.transform(filteredData, 'TSP').map(x => x.key).join(','),
      //"Batch": 'All',
      //"Trade": 'All',
      //"Certification Agency": 'All',
      //"Gender": 'All',
      //"District of Training Location": 'All',
      //"Class Status": 'All',
      //"TraineeImagesAdded": true
    };



    let exportExcel: ExportExcel = {
      Title: 'UnVerified Trainees Change Request Report',
      Author: this.currentUser.FullName,
      Type: EnumExcelReportType.UnVerifiedTraineesChangeRequestApproval,
      Data: data,
      List1: this.populateData(filteredData),
    };
    this.dialogue.openExportConfirmDialogue(exportExcel).subscribe();
  }


  populateData(data: any) {
    return data.map(item => {
      return {
        "Scheme": item.SchemeName
        , "TSP": item.TSPName
        , "Trade": item.TradeName
        , "Class Code": item.ClassCode
        , "Trainee Code": item.TraineeCode
        , "Trainee Name": item.TraineeName
        , "Father Name": item.FatherName
        // date format change by Umair Nadeem, date: 9 August 2024
        , "CNIC Issue Date": this._date.transform(item.CNICIssueDate, 'dd/MM/yyyy')
        , "Trainee CNIC": item.TraineeCNIC
        , "Date Of Birth": this._date.transform(item.DateOfBirth, 'dd/MM/yyyy')
        , "Trainee Address": item.TraineeHouseNumber + " " + item.TraineeStreetMohalla + " " + item.TraineeMauzaTown
        , "Trainee Tehsil": item.TehsilName
        , "Trainee District": item.DistrictName
        , "Gender Name": item.GenderName
        , "CNIC Verified": item.CNICVerified
        , "Trainee Status": item.TraineeStatus
        , "District Verication": item.DistrictVerified
        , "CNIC Unverification Reason": item.CNICUnVerifiedReason
        , "M& E Status": item.MEStatus
        , "BSS Status": item.BSSStatus
        , "Religion": item.Religion
        , "Permanent Address": item.PermanentAddress
        , "Permanent District": item.PermanentDistrict
        , "Permanent Tehsil": item.PermanentTehsil
        // newly added items by Umair Nadeem, date: 9 August 2024
        , "Class Start Date": this._date.transform(item.ClassStartDate, 'dd/MM/yyyy')
        , "Class End Date": this._date.transform(item.ClassEndDate, 'dd/MM/yyyy')
        , "Scheme Type": item.AMSName
        , "Project": item.ProjectName
        , "KAM Name": item.KAMName
        , "Created Date Time": this._date.transform(item.CreatedDate, 'dd/MM/yyyy h:mm:ss a')
        , "Ticket Number": item.TraineeChangeRequestID
      }
    })
  }


  openHistoryDialogue(data: any): void {
    this.dialogue.openCrTraineeHistoryDialogue(data.TraineeID);
  }

  openApprovalDialogue(row: any): void {
    let processKey = EnumApprovalProcess.CR_TRAINEE_UNVERIFIED;

    this.dialogue.openApprovalDialogue(processKey, row.TraineeChangeRequestID).subscribe(result => {
      this.GetSubmittedTrainees();
      //location.reload();
    });
  }
}

export interface ICRTraineeListFilter {
  Schemes: [];
  TSPs: [];
  Classes: [];
  UserID: number;
  KAMID: number;
  FundingCategoryID: number;  // Add this line
  startDate: Date | null;
  endDate: Date | null;
}
