import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormControl } from '@angular/forms';
import { CommonSrvService } from '../../common-srv.service';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { UserRightsModel, UsersModel } from '../../master-data/users/users.component';
import { DialogueService } from '../../shared/dialogue.service';
import { EnumApprovalProcess, EnumProgramCategory } from '../../shared/Enumerations';

@Component({
  selector: 'app-cr-inception-report-approvals',
  templateUrl: './cr-inception-report-approvals.component.html',
  styleUrls: ['./cr-inception-report-approvals.component.scss']
})
export class InceptionReportChangeRequestApprovalsComponent implements OnInit {
  displayedColumnsTSP = ["Action", 'TSPName', 'Address', 'HeadName', 'HeadDesignation', 'HeadLandline', 'HeadEmail', 'CPName', 'CPDesignation', 'CPLandline', 'CPEmail',
    'BankName', 'BankAccountNumber', 'AccountTitle', 'BankBranch'];
  //schemes: MatTableDataSource<any>;

  filters: ICRInceptionReportListFilter = { SchemeID: 0, ClassID: 0, TSPID: 0, KAMID: 0 };

  inceptionreports: [];
  currentInceptionReport: [];
  kamUsers: [];
  SearchSchemeList = new FormControl('',);
  SearchTSPList = new FormControl('',);
  SearchClassList = new FormControl('',);
  SearchKAM = new FormControl('',);
  TSPDetail = [];
  classesArray: any[];

  Scheme: any[];

  currentUser: UsersModel;
  trainees: [];
  currentTrainee: [];

  kamUsersId: number;
  userid: number;

  ActiveFormApprovalID: number;
  ChosenTradeID: number;
  title: string;
  savebtn: string;
  formrights: UserRightsModel;
  EnText: string = "";
  error: String;
  query = {
    order: 'InceptionReportChangeRequestID',
    limit: 5,
    page: 1
  };
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  working: boolean;

  constructor(private http: CommonSrvService, private dialogue: DialogueService) {
    //this.schemes = new MatTableDataSource([]);
    this.formrights = http.getFormRights();
  }

  ngOnInit(): void {
    this.http.setTitle("Class Timings Change Request");
    this.title = "";
    this.savebtn = "Approve";
    this.currentUser = this.http.getUserDetails();
    this.userid = this.currentUser.UserID;
  }
  ngAfterViewInit() {
    this.filters.SchemeID = 0;
    this.filters.TSPID = 0;
    this.filters.ClassID = 0;
    this.filters.KAMID = 0;
    this.getSchemes();
    this.getKAMAssignment();
    this.GetSubmittedInceptionReports();
  }
  EmptyCtrl() {
    this.SearchClassList.setValue('');
    this.SearchTSPList.setValue('');
    this.SearchSchemeList.setValue('');
    this.SearchKAM.setValue('');
  }
  getKAMAssignment() {
    this.http.getJSON('api/KAMAssignment/GetKAMAssignment').subscribe(
      (data: any) => {
        this.kamUsers = data[0];
      }
      , (error) => this.error = error
    );
  }
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
  GetSubmittedInceptionReports() {
    this.http.postJSON('api/InceptionReportChangeRequest/GetFilteredInceptionReportChangeRequest', { Process_Key: EnumApprovalProcess.CR_INCEPTION, KamID: this.filters.KAMID, OID: this.http.OID.value, SchemeID: this.filters.SchemeID, TSPID: this.filters.TSPID, ClassID: this.filters.ClassID }).subscribe((d: any) => {
      this.inceptionreports = d;
      //this.tsps.paginator = this.paginator;
      //this.tsps.sort = this.sort;
    },
      error => this.error = error // error path
      , () => {
        this.working = false;
      });
  }

  GetCurrentInceptionReportByID(r) {
    if (r.currentInceptionReport) {
      r.currentInceptionReport = null;

      return;
    }
    this.http.getJSON('api/InceptionReport/RD_InceptionReportBy_ID/', + r.IncepReportID).subscribe((d: any) => {
      r.currentInceptionReport = d;
    });
  }

  openApprovalDialogue(row: any): void {
    let processKey = EnumApprovalProcess.CR_INCEPTION;

    this.dialogue.openApprovalDialogue(processKey, row.InceptionReportChangeRequestID).subscribe(result => {
      console.log(result);
      //location.reload();
      this.GetSubmittedInceptionReports();
    });
  }

  openClassJourneyDialogue(data: any): void 
  {
		debugger;
		this.dialogue.openClassJourneyDialogue(data);
  }

}

export interface ICRInceptionReportListFilter {
  SchemeID: number;
  TSPID: number;
  ClassID: number;
  KAMID: number;
}
