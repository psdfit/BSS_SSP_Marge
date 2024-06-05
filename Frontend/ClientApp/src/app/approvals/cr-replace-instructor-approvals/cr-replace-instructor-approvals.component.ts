import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { CommonSrvService } from '../../common-srv.service';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { UserRightsModel } from '../../master-data/users/users.component';
import { DialogueService } from '../../shared/dialogue.service';
import { EnumApprovalProcess, EnumProgramCategory } from '../../shared/Enumerations';

@Component({
  selector: 'app-cr-replace-instructor-approvals',
  templateUrl: './cr-replace-instructor-approvals.component.html',
  styleUrls: ['./cr-replace-instructor-approvals.component.scss']
})
export class ReplaceInstructorChangeRequestApprovalsComponent implements OnInit {
    displayedColumnsTSP = ["Action", 'TSPName', 'Address', 'HeadName', 'HeadDesignation', 'HeadLandline', 'HeadEmail', 'CPName', 'CPDesignation', 'CPLandline', 'CPEmail',
        'BankName', 'BankAccountNumber', 'AccountTitle', 'BankBranch'];
    //schemes: MatTableDataSource<any>;

  instructorReplacementrequests: [];
  currentInceptionReport: [];

    ActiveFormApprovalID: number;
    ChosenTradeID: number;
    title: string;
    savebtn: string;
    formrights: UserRightsModel;
    EnText: string = "";
    error: String;
    query = {
        order: 'InstructorReplaceChangeRequestID',
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
        this.http.setTitle("Replace Instructor Change Request Approvals");
        this.title = "";
        this.savebtn = "Approve";
        this.GetSubmittedInstructorReplacements();
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
  GetSubmittedInstructorReplacements() {
        this.http.getJSON('api/InstructorReplaceChangeRequest/GetInstructorReplaceChangeRequest').subscribe((d: any) => {
            this.instructorReplacementrequests = d[0];
            //this.tsps.paginator = this.paginator;
            //this.tsps.sort = this.sort;
        },
            error => this.error = error // error path
            , () => {
                this.working = false;
            });
  }

  GetCurrentMappedInstructorByID(r) {
    if (r.currentMappedInstructor) {
      r.currentMappedInstructor = null;

      return;
    }
    this.http.getJSON('api/InceptionReport/RD_Instuctors_By_InceptionID/', + r.IncepReportID).subscribe((d: any) => {
      r.currentMappedInstructor = d;
    });
  }

    openApprovalDialogue(row: any): void {
        let processKey = EnumApprovalProcess.CR_INSTRUCTOR_REPLACE;
        
        this.dialogue.openApprovalDialogue(processKey, row.InstructorReplaceChangeRequestID).subscribe(result => {
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
