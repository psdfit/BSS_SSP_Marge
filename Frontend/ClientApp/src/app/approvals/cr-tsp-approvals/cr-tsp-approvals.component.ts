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
  selector: 'app-cr-tsp-approvals',
  templateUrl: './cr-tsp-approvals.component.html',
  styleUrls: ['./cr-tsp-approvals.component.scss']
})
export class TSPChangeRequestApprovalsComponent implements OnInit {
  displayedColumnsTSP = ["Action", 'TSPName', 'Address', 'HeadName', 'HeadDesignation', 'HeadLandline', 'HeadEmail', 'CPName', 'CPDesignation', 'CPLandline', 'CPEmail',
    'BankName', 'BankAccountNumber', 'AccountTitle', 'BankBranch'];
  //schemes: MatTableDataSource<any>;

  tsps: [];
  trades: [];
  SearchStatus = new FormControl();
  ActiveFormApprovalID: number;
  ChosenTradeID: number;
  title: string;
  savebtn: string;
  formrights: UserRightsModel;
  EnText: string = "";
  error: String;
  query = {
    order: 'SchemeChangeRequestID',
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
  EmptyCtrl() {
    this.SearchStatus.setValue('');
  }
  ngOnInit(): void {
    this.http.setTitle("TSP Profile Change Request");
    this.title = "";
    this.savebtn = "Approve";
    this.GetSubmittedTSPs();
    //this.GetPRNMasterForApproval();
  }
  //GetPRNMasterForApproval() {
  //  this.http.postJSON(`api/PRNMaster/GetPRNMasterForApproval`, { ProcessKey: this.processKey, Month: this.month.value, OID: this.http.OID.value, KAMID: this.filters.KAMID, SchemeID: this.filters.SchemeID, TSPMasterID: this.filters.TSPMasterID, StatusID: this.filters.StatusID }).subscribe(
  //    (data: any) => {
  //      this.PRNMaster = data;
  //      this.PRNMasterIDsArray = this.PRNMaster.map(o => o.PRNMasterID);
  //      this.PRNMasterIDs = this.PRNMasterIDsArray.join(',');

  //    }
  //    , (error) => {
  //      console.error(JSON.stringify(error));
  //    }
  //  );
  //}
 
  GetSubmittedTSPs() {
    this.http.getJSON('api/TSPChangeRequest/GetTSPChangeRequest').subscribe((d: any) => {
      this.tsps = d[0];
      //console.log(this.tsps)
      //this.tsps.paginator = this.paginator;
      //this.tsps.sort = this.sort;
    },
      error => this.error = error // error path
      , () => {
        this.working = false;
      });
  }

  GetCurrentTspByID(r) {
    if (r.currentTsp) {
      r.currentTsp = null;

      return;
    }
    this.http.postJSON('api/TSPDetail/RD_TSPDetailBy/', { TSPID: r.TSPID }).subscribe((d: any) => {
      r.currentTsp = d;
    });
  }


  openApprovalDialogue(row: any): void {
    let processKey = EnumApprovalProcess.CR_TSP;

    this.dialogue.openApprovalDialogue(processKey, row.TSPChangeRequestID).subscribe(result => {
      console.log(result);
      //location.reload();
    });
  }



}
