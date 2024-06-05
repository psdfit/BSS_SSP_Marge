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
    filters: ICRTraineeListFilter = { SchemeID: 0, ClassID: 0, TSPID: 0, UserID: 0 };

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
    }


    EmptyCtrl() {
      this.SearchClassList.setValue('');
      this.SearchTSPList.setValue('');
      this.SearchSchemeList.setValue('');
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

    getTSPDetailByScheme(schemeId: number) {
      this.classesArray = [];
      this.http.getJSON(`api/TSPDetail/GetTSPDetailByScheme/` + schemeId)
        .subscribe(data => {
          this.TSPDetail = <any[]>data;
        }, error => {
          this.error = error;
        })
    }
    getClassesByTsp(tspId: number) {
      this.http.getJSON(`api/Class/GetClassesByTsp/` + tspId)
        .subscribe(data => {
          this.classesArray = <any[]>data;
        }, error => {
          this.error = error;
        })
    }

    GetSubmittedTrainees() {
      this.http.postJSON('api/TraineeChangeRequest/GetTraineeChangeRequest/', { Process_Key: "CR_TRAINEE_UNVERIFIED", UserID: this.userid, OID: this.http.OID.value, SchemeID: this.filters.SchemeID, TSPID: this.filters.TSPID, ClassID: this.filters.ClassID }).subscribe((d: any) => {
            this.trainees = d[0];
            //this.tsps.paginator = this.paginator;
            //this.tsps.sort = this.sort;
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
    this.http.postJSON('api/TraineeProfile/RD_TraineeProfileBy/', { TraineeID: r.TraineeID} ).subscribe((d: any) => {
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
        , "CNIC Issue Date": this._date.transform(item.CNICIssueDate, 'MM/dd/yyyy')
        , "Trainee CNIC": item.TraineeCNIC
        , "Date Of Birth": this._date.transform(item.DateOfBirth, 'MM/dd/yyyy')
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
      }
    })
  }


  openHistoryDialogue(data: any): void {
    this.dialogue.openCrTraineeHistoryDialogue(data.TraineeID);
  }

  openApprovalDialogue(row: any): void {
      let processKey = EnumApprovalProcess.CR_TRAINEE_UNVERIFIED;
        
        this.dialogue.openApprovalDialogue(processKey, row.TraineeChangeRequestID).subscribe(result => {
          console.log(result);
          this.GetSubmittedTrainees();
            //location.reload();
        });
    }

}

export interface ICRTraineeListFilter {
  SchemeID: number;
  TSPID: number;
  ClassID: number;
  UserID: number;
}
