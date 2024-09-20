import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormControl } from '@angular/forms';
import { CommonSrvService } from '../../common-srv.service';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { UserRightsModel } from '../../master-data/users/users.component';
import { DialogueService } from '../../shared/dialogue.service';
import { EnumApprovalProcess, EnumProgramCategory } from '../../shared/Enumerations';
import { environment } from '../../../environments/environment';

@Component({
  selector: 'app-cr-class-dates-approvals',
  templateUrl: './cr-class-dates-approvals.component.html',
  styleUrls: ['./cr-class-dates-approvals.component.scss']
})
export class ClassDatesChangeRequestApprovalsComponent implements OnInit {
    environment = environment;

    displayedColumnsClass = ['ClassCode',
        'TradeName', 'SourceOfCurriculumName', 'EntryQualification', 'CertAuthName',
        'StartDate', 'EndDate', "Action"];
  SearchSch = new FormControl('',);
  SearchStatus = new FormControl('',);
    classes: [];
    currentClassDates: [];
    Scheme = [];
    schemeFilter = new FormControl(0);
    searchFilter = new FormControl(0);
    ActiveFormApprovalID: number;
    ChosenTradeID: number;
    title: string;
    savebtn: string;
    formrights: UserRightsModel;
    EnText: string = "";
    error: String;
    query = {
        order: 'ClassDatesChangeRequestID',
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
        this.http.setTitle("Class Dates Change Request");
        this.title = "";
      this.savebtn = "Approve";
      this.GetSchemes();
      this.GetClassDatesCRs();
    }

    GetClassDatesCRs() {
        this.http.getJSON('api/ClassChangeRequest/GetClassDatesChangeRequest/' + this.schemeFilter.value + '/' + this.searchFilter.value + '/' + null).subscribe((d: any) => {
          this.classes = d[0];
            //this.tsps.paginator = this.paginator;
            //this.tsps.sort = this.sort;
        },
            error => this.error = error // error path
            , () => {
                this.working = false;
            });
  }
  GetSchemes() {
    this.http.getJSON('api/ClassChangeRequest/GetClassScheme').subscribe((d: any) => {
     this.Scheme = d[0];
      //this.tsps.paginator = this.paginator;
      //this.tsps.sort = this.sort;
    },
      error => this.error = error // error path
      , () => {
        this.working = false;
      });
  }
  
  GetCurrentClassDatesByID(r) {
    if (r.currentClassDates) {
      r.currentClassDates = null;

      return;
    }
    this.http.postJSON('api/Class/RD_ClassBy/', { ClassID: r.ClassID }).subscribe((d: any) => {
      r.currentClassDates = d;
    });
  }
  /// Develop by Rao Ali Haider 20-Nov-2023

  EmptyCtrl() {
    this.SearchSch.setValue('');
    this.SearchStatus.setValue('');
  }
    openApprovalDialogue(row: any): void {
        let processKey = EnumApprovalProcess.CR_CLASS_DATES;
        
        this.dialogue.openApprovalDialogue(processKey, row.ClassDatesChangeRequestID).subscribe(result => {
            console.log(result);
            //location.reload();
          this.GetClassDatesCRs();
        });
  }
    openClassJourneyDialogue(data: any): void 
    {
      debugger;
      this.dialogue.openClassJourneyDialogue(data);
    }
  openApprovalDialogueBatch(): void {
    debugger;
    this.dialogue.openApprovalDialogueBatch().subscribe(result => {
      this.GetClassDatesCRs();
    });
  }

}
