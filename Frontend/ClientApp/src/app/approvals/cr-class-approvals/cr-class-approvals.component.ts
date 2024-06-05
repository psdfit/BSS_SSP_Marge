import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { CommonSrvService } from '../../common-srv.service';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { UserRightsModel, UsersModel } from '../../master-data/users/users.component';
import { DialogueService } from '../../shared/dialogue.service';
import { EnumApprovalProcess, EnumProgramCategory } from '../../shared/Enumerations';
import { FormControl } from '@angular/forms';

@Component({
  selector: 'app-cr-class-approvals',
  templateUrl: './cr-class-approvals.component.html',
  styleUrls: ['./cr-class-approvals.component.scss']
})
export class ClassChangeRequestApprovalsComponent implements OnInit {
  displayedColumnsClass = ['ClassCode',
    'TradeName', 'SourceOfCurriculumName', 'EntryQualification', 'CertAuthName',
    'StartDate', 'EndDate', "Action"];

  classes: [];
  currentClassLocation: [];


  ActiveFormApprovalID: number;
  ChosenTradeID: number;
  title: string;
  savebtn: string;
  formrights: UserRightsModel;
  EnText: string = "";
  error: String;
  query = {
    order: 'ClassChangeRequestID',
    limit: 5,
    page: 1
  };
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  working: boolean;
  //Filters
  kamUsers: [];
  SearchSchemeList = new FormControl('',);
  SearchTSPList = new FormControl('',);
  SearchClassList = new FormControl('',);
  SearchKAM = new FormControl('',);
  TSPDetail = [];
  classesArray: any[];
  Scheme: any[];
  filters: IClassLocationChangeRequestFilter = { SchemeID: 0, TSPID: 0, ClassID: 0, KAMID: 0 };


  constructor(private http: CommonSrvService, private dialogue: DialogueService) {
    //this.schemes = new MatTableDataSource([]);
    this.formrights = http.getFormRights();
  }

  ngOnInit(): void {
    this.http.setTitle("Class Location Change Request");
    this.title = "";
    this.savebtn = "Approve";
  }
  ngAfterViewInit() {
    this.filters.SchemeID = 0;
    this.filters.TSPID = 0;
    this.filters.ClassID = 0;
    //this.getSchemes();
    //this.getKAMAssignment();
    this.GetSubmittedClasses();
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
  GetSubmittedClasses() {
    //this.http.getJSON('api/ClassChangeRequest/GetClassChangeRequest').subscribe((d: any) => {
    //  this.classes = d[0];
    //  //this.tsps.paginator = this.paginator;
    //  //this.tsps.sort = this.sort;
    //},
    //  error => this.error = error // error path
    //  , () => {
    //    this.working = false;
    //  });
    this.http.postJSON('api/ClassChangeRequest/GetClassChangeRequestByFilter', this.filters).subscribe((d: any) => {
      this.classes = d;
      //this.tsps.paginator = this.paginator;
      //this.tsps.sort = this.sort;
    },
      error => this.error = error // error path
      , () => {
        this.working = false;
      });
  }

  GetCurrentClassLocationByID(r) {
    if (r.currentClassLocation) {
      r.currentClassLocation = null;

      return;
    }
    this.http.postJSON('api/Class/RD_ClassBy/', { ClassID: r.ClassID }).subscribe((d: any) => {
      r.currentClassLocation = d;
    });
  }

  openApprovalDialogue(row: any): void {
    let processKey = EnumApprovalProcess.CR_CLASS_LOCATION;

    this.dialogue.openApprovalDialogue(processKey, row.ClassChangeRequestID).subscribe(result => {
      console.log(result);
      //location.reload();
      this.GetSubmittedClasses();
    });
  }

  //Filter
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
  openClassJourneyDialogue(data: any): void 
  {
		debugger;
		this.dialogue.openClassJourneyDialogue(data);
  }
}

export interface IClassLocationChangeRequestFilter {
  SchemeID: number;
  TSPID: number;
  KAMID: number;
  ClassID: number;
}
