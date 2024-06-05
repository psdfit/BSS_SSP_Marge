import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { CommonSrvService } from '../../common-srv.service';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { UserRightsModel } from '../../master-data/users/users.component';
import { DialogueService } from '../../shared/dialogue.service';
import { EnumApprovalProcess, EnumProgramCategory } from '../../shared/Enumerations';

@Component({
  selector: 'app-trade-approvals',
  templateUrl: './trade-approvals.component.html',
  styleUrls: ['./trade-approvals.component.scss']
})
export class TradeApprovalsComponent implements OnInit {
  displayedColumnsScheme = ['SchemeName', 'SchemeCode', 'Description', 'CreatedDate', 'UserName', 'PTypeName', 'PCategoryName', 'FundingSourceName', 'FundingCategoryName', 'PaymentSchedule', 'Stipend', 'StipendMode', 'UniformAndBag', 'MinimumEducation', 'MaximumEducation', 'MinAge', 'MaxAge', 'GenderName', 'DualEnrollment', 'ContractAwardDate', 'BusinessRuleType', 'OName', 'Action'];
  displayedColumnsTSPs = ['TSPName', 'TSPCode', 'Address', 'TSPColor', 'Tier', 'NTN', 'PNTN', 'GST', 'FTN', 'DistrictName', 'HeadName', 'HeadDesignation', 'HeadEmail', 'HeadLandline', 'OrgLandline', 'CPName', 'CPDesignation', 'CPLandline', 'CPEmail', 'Website', 'CPAdmissionsName', 'CPAdmissionsDesignation', 'CPAdmissionsLandline', 'CPAdmissionsEmail', 'CPAccountsName', 'CPAccountsDesignation', 'CPAccountsLandline', 'CPAccountsEmail', 'BankName', 'BankAccountNumber', 'AccountTitle', 'BankBranch', 'Organization', 'Action'];
  displayedColumns = ['TradeName', 'TradeCode', 'SectorName', 'SubSectorName', 'Duration', 'CertificationCategoryName', 'CertAuthName', 'Name', "Action"];
  //schemes: MatTableDataSource<any>;

  schemes: [];
  trades: [];

  ActiveFormApprovalID: number;
  ChosenTradeID: number;
  title: string;
  savebtn: string;
  formrights: UserRightsModel;
  EnText: string = "";
  error: String;
  query = {
    order: 'TradeID',
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
    this.http.setTitle("Trade Approvals");
    this.title = "";
    this.savebtn = "Approve";
    this.http.OID.subscribe(OID => {
      this.GetSubmittedTradesForMyID();
    });
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
  GetSubmittedTradesForMyID() {
    this.http.getJSON(`api/Trade/GetSubmittedTrades?OID=${this.http.OID.value}`).subscribe((d: any) => {
      this.trades = d;
      //this.schemes.paginator = this.paginator;
      //this.schemes.sort = this.sort;
    },
      error => this.error = error // error path
      , () => {
        this.working = false;
      });
  }

  openApprovalDialogue(row: any): void {
    let processKey = EnumApprovalProcess.TRD;
    //if (row.PCategoryID == EnumProgramCategory.BusinessDevelopmentAndPartnerships) {
    //  processKey = EnumApprovalProcess.AP_BD;
    //}
    this.dialogue.openApprovalDialogue(processKey, row.TradeID).subscribe(result => {
      console.log(result);
      //location.reload();
    });
  }

  //OK() { //this method is just for testing invoices generation, pls ignore this
  //    this.http.getJSON('api/Scheme/GenerateInvoice').subscribe((d: any) => {
  //        this.classes = d;
  //    });
  //}

  //ApproveOrReject(ApprovalData: IApproval) {
  //  let params = []
  //  let ProcessKey = 'AP'; // AP = Appendix, (ProcessKey)

  //  params.push(ApprovalData.SchemeID);
  //  params.push(ApprovalData.Comment);
  //  params.push(ApprovalData.Decision);
  //  params.push(ProcessKey);
  //  params.push(ApprovalData.FormApprovalID);

  //  this.http.postJSON('api/FormApproval/ApproveOrRejectScheme', JSON.stringify(params)).subscribe((d: any) => {
  //  },
  //    error => this.error = error // error path
  //    , () => {
  //      this.working = false;
  //      location.reload();
  //    });
  //}

  //openDialog(r: IApproval): void {
  //  const dialogRef = this.dialog.open(ApprovalsDialogueComponent, {
  //    minWidth: '400px',
  //    minHeight: '400px',

  //    data: { ...r }
  //  })
  //  dialogRef.afterClosed().subscribe(result => {
  //    console.log(result);
  //    this.ApprovalData = result;

  //    if (this.ApprovalData != null)
  //      this.ApproveOrReject(this.ApprovalData);
  //  })
  //}

}
