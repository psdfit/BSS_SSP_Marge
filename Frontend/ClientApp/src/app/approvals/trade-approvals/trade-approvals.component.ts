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
  // trades: [];
  trades: any[] = []; // Modified to any[] to allow adding showDetails property
  selectedTrade: any = null; // Track the currently selected trade for details
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
  // Reference data for mapping IDs to names
  CertificationCategory: any[] = [];
  CertificationAuthority: any[] = [];
  EquipmentToolsList: any[] = [];
  ConsumableMaterialList: any[] = [];
  TrainerQualificationList: any[] = [];
  SourceOfCurriculumList: any[] = [];
  DurationList: any[] = [];
  AcademicDisciplineList: any[] = [];
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  working: boolean;
  currentPage = 1;
  itemsPerPage = 10; // Or any number you want
  searchTerm: string = '';

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
    // Fetch reference data
    this.getReferenceData();
  }

  // Fetch reference data for mapping IDs to names
  getReferenceData(): void {
    this.http.getJSON('api/Trade/GetTrade').subscribe(
      (d: any) => {
        this.CertificationCategory = d[3];
        this.CertificationAuthority = d[4];
        this.EquipmentToolsList = d[5];
        this.ConsumableMaterialList = d[6];
        this.TrainerQualificationList = d[7];
        this.SourceOfCurriculumList = d[8];
        this.DurationList = d[9];
        this.AcademicDisciplineList = d[10];
      },
      error => this.error = error
    );
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

  // Toggle details view and fetch details if needed
  toggleDetails(trade: any): void {
    if (this.selectedTrade && this.selectedTrade.TradeID === trade.TradeID) {
      // Toggle visibility if the same trade is clicked
      this.selectedTrade.showDetails = !this.selectedTrade.showDetails;
      if (!this.selectedTrade.showDetails) {
        this.selectedTrade = null; // Clear selection when hiding
      }
    } else {
      // Reset previous trade's showDetails
      if (this.selectedTrade) {
        this.selectedTrade.showDetails = false;
      }
      // Set new selected trade and fetch details
      trade.showDetails = true;
      this.selectedTrade = trade;
      this.fetchTradeDetails(trade.TradeID);
    }
  }

  // Fetch trade details from API
  fetchTradeDetails(tradeID: number): void {
    this.http.getJSON(`api/Trade/GetTradeMapDetails/${tradeID}`).subscribe(
      (data: any[]) => {
        this.selectedTrade.details = data; // Store array of details
      },
      error => {
        this.error = error;
        this.selectedTrade.details = []; // Set empty array on error
      }
    );
  }

  // Helper methods to map IDs to names
  getDurationName(id: number): string {
    const item = this.DurationList.find(d => d.DurationID === id);
    return item ? item.Duration || 'N/A' : 'N/A';
  }

  getCertificationCategoryName(id: number): string {
    const item = this.CertificationCategory.find(c => c.CertificationCategoryID === id);
    return item ? item.CertificationCategoryName || 'N/A' : 'N/A';
  }

  getCertificationAuthorityName(id: number): string {
    const item = this.CertificationAuthority.find(c => c.CertAuthID === id);
    return item ? item.CertAuthName || 'N/A' : 'N/A';
  }

  getEducationTypeName(id: number): string {
    const item = this.TrainerQualificationList.find(t => t.EducationTypeID === id);
    return item ? item.Education || 'N/A' : 'N/A';
  }

  getAcademicDisciplineName(id: number): string {
    const item = this.AcademicDisciplineList.find(a => a.AcademicDisciplineID === id);
    return item ? item.AcademicDisciplineName || 'N/A' : 'N/A';
  }

  getEquipmentToolNames(ids: string): string {
    if (!ids) return 'N/A';
    const idArray = ids.split(',').map(id => id.trim());
    const names = idArray
      .map(id => {
        const item = this.EquipmentToolsList.find(e => e.EquipmentToolID === Number(id));
        return item ? item.EquipmentName || 'N/A' : 'N/A';
      })
      .filter(name => name !== 'N/A');
    return names.length > 0 ? names.join(', ') : 'N/A';
  }

  getConsumableMaterialNames(ids: string): string {
    if (!ids) return 'N/A';
    const idArray = ids.split(',').map(id => id.trim());
    const names = idArray
      .map(id => {
        const item = this.ConsumableMaterialList.find(c => c.ConsumableMaterialID === Number(id));
        return item ? item.ItemName || 'N/A' : 'N/A';
      })
      .filter(name => name !== 'N/A');
    return names.length > 0 ? names.join(', ') : 'N/A';
  }


  get filteredTrades() {
    if (!this.searchTerm) return this.trades;
    const term = this.searchTerm.toLowerCase();
    return this.trades.filter(trade =>
      trade.TradeName?.toLowerCase().includes(term) ||
      trade.TradeCode?.toLowerCase().includes(term) ||
      trade.SectorName?.toLowerCase().includes(term) ||
      trade.SubSectorName?.toLowerCase().includes(term)
    );
  }

  get paginatedTrades() {
    const filtered = this.filteredTrades; // apply search
    const start = (this.currentPage - 1) * this.itemsPerPage;
    const end = start + this.itemsPerPage;
    return filtered.slice(start, end);
  }
  get totalPages() {
    return Math.ceil(this.trades.length / this.itemsPerPage);
  }
  changePage(page: number) {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
    }
  }

  applyFilter(event: any): void {
    this.searchTerm = event.target.value.trim().toLowerCase();
    this.currentPage = 1; // Reset to first page on new search
  }

  DataExcelExport(data: any, title) {
    this.http.ExcelExporWithForm(data, title);
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
