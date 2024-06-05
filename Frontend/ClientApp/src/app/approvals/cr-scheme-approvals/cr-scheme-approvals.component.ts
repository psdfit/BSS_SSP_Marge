import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { CommonSrvService } from '../../common-srv.service';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { UserRightsModel } from '../../master-data/users/users.component';
import { DialogueService } from '../../shared/dialogue.service';
import { EnumApprovalProcess, EnumProgramCategory } from '../../shared/Enumerations';

@Component({
    selector: 'app-cr-scheme-approvals',
    templateUrl: './cr-scheme-approvals.component.html',
    styleUrls: ['./cr-scheme-approvals.component.scss']
})
export class SchemeChangeRequestApprovalsComponent implements OnInit {
    displayedColumnsScheme = ['SchemeName', 'SchemeCode',
        //'Description', 'CreatedDate', 'UserName', 'PTypeName', 'PCategoryName', 'FundingSourceName', 'FundingCategoryName', 'PaymentSchedule',
        'Stipend', 'StipendMode', 'UniformAndBag',
        //'MinimumEducation', 'MaximumEducation', 'MinAge', 'MaxAge', 'GenderName', 'DualEnrollment', 'ContractAwardDate',
        'BusinessRuleType',
        //'OName',
        'Action'];
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

    ngOnInit(): void {
        this.http.setTitle("Scheme Change Request Approvals");
        this.title = "";
        this.savebtn = "Approve";
        this.GetSubmittedTradesForMyID();
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
        this.http.getJSON('api/SchemeChangeRequest/GetSchemeChangeRequest').subscribe((d: any) => {
            this.schemes = d[0];
            //this.schemes.paginator = this.paginator;
            //this.schemes.sort = this.sort;
        },
            error => this.error = error // error path
            , () => {
                this.working = false;
            });
    }

    openApprovalDialogue(row: any): void {
        let processKey = EnumApprovalProcess.CR_SCHEME;
        //if (row.PCategoryID == EnumProgramCategory.BusinessDevelopmentAndPartnerships) {
        //  processKey = EnumApprovalProcess.AP_BD;
        //}
        this.dialogue.openApprovalDialogue(processKey, row.SchemeChangeRequestID).subscribe(result => {
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
