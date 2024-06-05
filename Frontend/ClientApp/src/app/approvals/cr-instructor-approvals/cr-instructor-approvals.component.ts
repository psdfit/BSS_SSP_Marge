import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { CommonSrvService } from '../../common-srv.service';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { UserRightsModel } from '../../master-data/users/users.component';
import { DialogueService } from '../../shared/dialogue.service';
import { EnumApprovalProcess, EnumProgramCategory } from '../../shared/Enumerations';
import { FormControl } from '@angular/forms';

@Component({
  selector: 'app-cr-instructor-approvals',
  templateUrl: './cr-instructor-approvals.component.html',
  styleUrls: ['./cr-instructor-approvals.component.scss']
})
export class InstructorChangeRequestApprovalsComponent implements OnInit {
  displayedColumnsTSP = ["Action", 'TSPName', 'Address', 'HeadName', 'HeadDesignation', 'HeadLandline', 'HeadEmail', 'CPName', 'CPDesignation', 'CPLandline', 'CPEmail',
    'BankName', 'BankAccountNumber', 'AccountTitle', 'BankBranch'];
  //schemes: MatTableDataSource<any>;

  instructors: [];
  currentInstructor: [];
  SearchSchemeList = new FormControl('',);
  SearchTSPList = new FormControl('',);
  SearchClassList = new FormControl('',);
  SearchKAM = new FormControl('',);
  TSPDetail = [];
  classesArray: any[];
  Scheme: any[];


  filters: IInstructorChangeRequestFilter = { SchemeID: 0, TSPID: 0, ClassID: 0, KAMID: 0 };


  ActiveFormApprovalID: number;
  ChosenTradeID: number;
  title: string;
  savebtn: string;
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

  constructor(private http: CommonSrvService, private dialogue: DialogueService) {
    //this.schemes = new MatTableDataSource([]);
    this.formrights = http.getFormRights();
  }

  ngOnInit(): void {
    this.http.setTitle("Instructor Change Request");
    this.title = "";
    this.savebtn = "Approve";
    this.GetSubmittedInstructors();
    this.getSchemes();
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
      });
  }

  getClassesByTsp(tspId: number) {
    this.http.getJSON(`api/Class/GetClassesByTsp/` + tspId)
      .subscribe(data => {
        this.classesArray = <any[]>data;
      }, error => {
        this.error = error;
      });
  }

  EmptyCtrl() {
    this.SearchClassList.setValue('');
    this.SearchTSPList.setValue('');
    this.SearchSchemeList.setValue('');
    this.SearchKAM.setValue('');
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
  GetSubmittedInstructors() {
    this.http.postJSON('api/InstructorChangeRequest/GeFilteredtInstructorChangeRequest', this.filters).subscribe((d: any) => {
      this.instructors = d[0];
      //this.tsps.paginator = this.paginator;
      //this.tsps.sort = this.sort;
    },
      error => this.error = error // error path
      , () => {
        this.working = false;
      });
  }

  GetCurrentInstructorByID(r) {
    if (r.currentInstructor) {
      r.currentInstructor = null;

      return;
    }
    this.http.getJSON('api/Instructor/RD_InstructorBy_ID/' + r.InstrID).subscribe((d: any) => {
      r.currentInstructor = d;
    });
  }

  openApprovalDialogue(row: any): void {
    let processKey = EnumApprovalProcess.CR_INSTRUCTOR;

    this.dialogue.openApprovalDialogue(processKey, row.InstructorChangeRequestID).subscribe(result => {
      console.log(result);
      //location.reload();
    });
  }



}

export interface IInstructorChangeRequestFilter {
  SchemeID: number;
  TSPID: number;
  KAMID: number;
  ClassID: number;
}
