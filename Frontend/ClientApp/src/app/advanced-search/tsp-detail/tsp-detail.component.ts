import { Component, OnInit, Input, ViewChild } from "@angular/core";
import { CommonSrvService } from "../../common-srv.service";
import { environment } from '../../../environments/environment';
import { EnumApprovalProcess, EnumExcelReportType } from '../../shared/Enumerations';
import { DialogueService } from '../../shared/dialogue.service';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { animate, trigger, state, transition, style } from '@angular/animations';

@Component({
  selector: 'tsp-detail',
  templateUrl: './tsp-detail.component.html',
  styleUrls: ['./tsp-detail.component.scss'],
  animations: [
    trigger('detailExpand', [
      state('collapsed', style({ height: '0px', minHeight: '0' })),
      state('expanded', style({ height: '*' })),
      transition('expanded <=> collapsed', animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)')),
    ]),
  ],
})


export class TSPDetailComponent implements OnInit{

  displayedColumnsClass = ['ClassCode' ,'Shift' ,'EnrolledTrainees' ,'ClassStartTime' ,'SourceOfCurriculum' ,'SchemeName' ,'ClassStatusName' ];
  dataSourceClass: MatTableDataSource<any>;
  displayedColumnsInstructor = ['InstrID', 'InstructorName', 'CNICofInstructor', 'GenderName'];
  dataSourceInstructor: MatTableDataSource<any>;
  displayedColumnsPRN = ['Sr', 'ApprovalProcessName', 'TSPName', 'Month', 'InvoiceNumber', 'Approved', 'CreatedDate'];
  dataSourcePRN: MatTableDataSource<any>;
  //displayedColumnsINV = ['Sr', 'Approval', 'TSPName', 'Month', 'InvoiceNumber', 'Approved', 'CreatedDate'];
  //dataSourceINV: MatTableDataSource<any>;

  @ViewChild('paginatorClass') paginatorClass: MatPaginator;
  @ViewChild('sortClass') sortClass: MatSort;

  @ViewChild('paginatorInstructor') paginatorInstructor: MatPaginator;
  @ViewChild('sortInstructor') sortInstructor: MatSort;

  @ViewChild('paginatorPRN') paginatorPRN: MatPaginator;
  @ViewChild('sortPRN') sortPRN: MatSort;

  //@ViewChild('paginatorINV') paginatorINV: MatPaginator;
  //@ViewChild('sortINV') sortINV: MatSort;

  isExpansionDetailRow = (i: number, row: Object) => row.hasOwnProperty('detailRow');
  expandedElement: any;

  constructor(private commonService: CommonSrvService, private dialogue: DialogueService) {
  }

  environment = environment;

  @Input() ID: any;

  enumApprovalProcess = EnumApprovalProcess;
  processKey: string = '';
  processTypes = [
    { value: EnumApprovalProcess.PRN_R, text: "Regular" }
    , { value: EnumApprovalProcess.PRN_C, text: "Completion" }
    , { value: EnumApprovalProcess.PRN_T, text: "PRN_T" }
    , { value: EnumApprovalProcess.PRN_F, text: "Final" }
  ]

  TSPDetail: any;
  TSPSchemes: any[];
  TSPClasses: any[];
  TSPInstructors: any[];
  TSPPRN: any[];
  TSPPRNDetail: any[];
  TSPPRNDetailMonthly: any[];
  TSPINV: any;
  TSPINVDetail: any[];

  getData() {
    this.commonService.getJSON("api/AdvancedSearch/GetTSPDetail?TSPMasterID=" + this.ID).subscribe((data: any[]) => {
      //console.log(data);
      this.TSPDetail = data["TSPDetail"][0];
      this.TSPSchemes = data["TSPSchemes"];
      this.TSPClasses = data["TSPClasses"];
      this.TSPInstructors = data["TSPInstructors"];
      this.TSPPRN = data["TSPPRN"];
      this.TSPINV = data["TSPINV"];

      this.dataSourceClass = new MatTableDataSource(this.TSPClasses);      
      this.dataSourceClass.paginator = this.paginatorClass;
      this.dataSourceClass.sort = this.sortClass;

      this.dataSourceInstructor = new MatTableDataSource(this.TSPInstructors);
      this.dataSourceInstructor.paginator = this.paginatorInstructor;
      this.dataSourceInstructor.sort = this.sortInstructor;

      this.dataSourcePRN = new MatTableDataSource(this.TSPPRN);
      this.dataSourcePRN.paginator = this.paginatorPRN;
      this.dataSourcePRN.sort = this.sortPRN;

      //this.dataSourceINV = new MatTableDataSource(this.TSPINV);
      //this.dataSourceINV.paginator = this.paginatorINV;
      //this.dataSourceINV.sort = this.sortINV;
    });
  }

  getPRNDetail(r: any) {
    r.HasPRN = !r.HasPRN;

    if (r.PRN) {
      this.TSPPRNDetail = this.TSPPRNDetail.filter(s => s.PRNMasterID != r.PRNMasterID);
      this.expandedElement = null;
      return;
    }
    this.commonService.getJSON(`api/PRN/GetPRNForApproval/`, r.PRNMasterID).subscribe(
      (data: any) => {
        r.PRN = data;
        r.HasPRN = true;
        this.TSPPRNDetail.push(data);
        this.TSPPRNDetail = this.TSPPRNDetail.reduce((accumulator, value) => accumulator.concat(value), []);
        this.expandedElement = r;
        //console.log(this.prnDetailsArray);
      }
      , (error) => {
        console.error(JSON.stringify(error));
      }
    );
  }

  getPTBRTrainees(c: any) {
    this.commonService.postJSON(`api/PRN/GetPTBRTrainees`, { ClassCode: c.ClassCode, Month: c.Month }).subscribe(
      (data: any) => {
        c.previousPTBRTrainees = data;
      }
      , (error) => {
        console.error(JSON.stringify(error));
      }
    );
  }

  getClassMonthview(ClassID: number, Month: Date, processkey: string): void {
    this.dialogue.openClassMonthviewDialogue(ClassID, Month, processkey).subscribe(result => {
      console.log(result);
      //location.reload();
    });
  }

  GetInvoiceLines(r) {
    if (r.InvoiceLines) {
      r.InvoiceLines = null;
      this.TSPINVDetail = this.TSPINVDetail.filter(inv => inv.InvoiceHeaderID !== r.InvoiceHeaderID);

      return;
    }

    this.commonService.getJSON('api/Invoice/GetInvoiceLines/', r.InvoiceHeaderID).subscribe((d: any) => {
      r.InvoiceLines = d;
      this.TSPINVDetail.push(d);
      console.log(this.TSPINVDetail);
      this.TSPINVDetail = this.TSPINVDetail.reduce((accumulator, value) => accumulator.concat(value), []);
    });
  }

  ngOnInit(): void {
    this.getData();
    this.TSPPRNDetail = [];
    this.TSPINVDetail = [];
  }
}
