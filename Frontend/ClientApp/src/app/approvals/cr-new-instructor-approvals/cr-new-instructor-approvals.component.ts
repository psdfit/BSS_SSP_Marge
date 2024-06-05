import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { CommonSrvService } from '../../common-srv.service';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { UserRightsModel } from '../../master-data/users/users.component';
import { DialogueService } from '../../shared/dialogue.service';
import { EnumApprovalProcess, EnumProgramCategory } from '../../shared/Enumerations';
import { DomSanitizer } from '@angular/platform-browser';
import { FormControl } from '@angular/forms';


@Component({
  selector: 'app-cr-new-instructor-approvals',
  templateUrl: './cr-new-instructor-approvals.component.html',
  styleUrls: ['./cr-new-instructor-approvals.component.scss']
})
export class NewInstructorRequestApprovalsComponent implements OnInit {
  displayedColumnsTSP = ["Action", 'TSPName', 'Address', 'HeadName', 'HeadDesignation', 'HeadLandline', 'HeadEmail', 'CPName', 'CPDesignation', 'CPLandline', 'CPEmail',
    'BankName', 'BankAccountNumber', 'AccountTitle', 'BankBranch'];
  //schemes: MatTableDataSource<any>;

  newinstructors: [];
  currentInstructor: [];
  currentInstructorAttachment: [];

  filters: INewInstructorChangeRequestFilter = { SchemeID: 0, TSPID: 0, ClassID: 0, KAMID: 0 };


  ActiveFormApprovalID: number;
  ChosenTradeID: number;
  title: string;
  savebtn: string;

  SearchSchemeList = new FormControl('',);
  SearchTSPList = new FormControl('',);
  SearchClassList = new FormControl('',);
  SearchKAM = new FormControl('',);
  TSPDetail = [];
  classesArray: any[];
  Scheme: any[];

  formrights: UserRightsModel;
  EnText: string = "";
  error: String;
  query = {
    order: 'InstructorChangeRequestID',
    limit: 5,
    page: 1
  };
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  working: boolean;

  constructor(private http: CommonSrvService, public domSanitizer: DomSanitizer, private dialogue: DialogueService) {
    //this.schemes = new MatTableDataSource([]);
    this.formrights = http.getFormRights();
  }

  ngOnInit(): void {
    this.http.setTitle("New Instructor Requests");
    this.title = "";
    this.savebtn = "Approve";
    this.GetNewInstructorsRequests();
    this.getSchemes();
  }

  //Filter
  EmptyCtrl() {
    this.SearchClassList.setValue('');
    this.SearchTSPList.setValue('');
    this.SearchSchemeList.setValue('');
    //this.SearchKAM.setValue('');
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
  GetNewInstructorsRequests() {
    this.http.postJSON('api/InstructorChangeRequest/GetFilteredNewInstructorRequest', this.filters).subscribe((d: any) => {
      this.newinstructors = d[0];
      //this.tsps.paginator = this.paginator;
      //this.tsps.sort = this.sort;
    },
      error => this.error = error // error path
      , () => {
        this.working = false;
      });
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


  GetCurrentInstructorAttachements(r) {
    if (r.currentInstructorAttachments) {
      r.currentInstructorAttachments = null;

      return;
    }
    this.http.postJSON('api/InstructorChangeRequest/GetNewInstructorRequestAttachments', { "CRNewInstructorID": r.CRNewInstructorID }).subscribe((d: any) => {
      console.log(d[0].FilePath);
      if (d[0].map(x=>x.FilePath) == "") {
        this.error = "File not found against this record";
        this.http.ShowError(this.error.toString(), "Error");
        return;
      }
      r.currentInstructorAttachments = d[0];
      //this.tsps.paginator = this.paginator;
      //this.tsps.sort = this.sort;
    });
  }

  openApprovalDialogue(row: any): void {
    let processKey = EnumApprovalProcess.CR_NEW_INSTRUCTOR;

    this.dialogue.openApprovalDialogue(processKey, row.CRNewInstructorID).subscribe(result => {
      console.log(result);
      //location.reload();
    });
  }


  openClassJourneyDialogue(data: any): void 
  {
		debugger;
		this.dialogue.openClassJourneyDialogue(data);
  }
}


export interface INewInstructorChangeRequestFilter {
  SchemeID: number;
  TSPID: number;
  KAMID: number;
  ClassID: number;
}
