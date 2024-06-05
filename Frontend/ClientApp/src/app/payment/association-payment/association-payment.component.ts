import { Component, OnInit, ViewChild, ChangeDetectorRef } from "@angular/core";
import { CommonSrvService } from "../../common-srv.service";
import { ActivatedRoute, Router } from "@angular/router";
import { MatTableDataSource } from "@angular/material/table";
import { MatPaginator } from "@angular/material/paginator";
import { MatSort } from "@angular/material/sort";
import { MatTabGroup } from "@angular/material/tabs";
import { FormBuilder, FormControl, FormGroup, Validators } from "@angular/forms";
import { MatDialog, MatDialogRef } from "@angular/material/dialog";
import { environment } from '../../../environments/environment';
import { TspStatusUpdateComponent } from "src/app/custom-components/tsp-status-update/tsp-status-update.component";
import { SelectionModel } from '@angular/cdk/collections';
@Component({
  selector: 'app-association-payment',
  templateUrl: './association-payment.component.html',
  styleUrls: ['./association-payment.component.scss']
})
export class AssociationPaymentComponent implements OnInit {
  isChecked: boolean = false
  isEdit: number;
  TotalClasses: any = 0
  constructor(
    private ComSrv: CommonSrvService,
    private AcitveRoute: ActivatedRoute,
    private router: Router,
    private fb: FormBuilder,
    private Cdr: ChangeDetectorRef,
    private Dialog: MatDialog,
  ) { }
  ChangeNoOfClass(row) {
    const TotalNoOfClassApplied = this.DTablesData.filteredData.reduce((sum, row) => Number(sum) + Number(row.NoOfClass), 0);
    this.TotalClasses = TotalNoOfClassApplied
    const TotalAssociationFee = this.AssociationFee[0].AssociationFee * parseInt(TotalNoOfClassApplied)
    this.TotalAssociationFee = TotalAssociationFee
  }
  NoOfClass: any;
  environment = environment;
  error: any;
  GetDataObject: any = {}
  SpacerTitle: string;
  TSearchCtr = new FormControl('');
  TapTTitle: string = "Profile"
  TableColumns = [];
  DTableColumns = [];
  @ViewChild("tabGroup") tabGroup: MatTabGroup;
  TablesData: MatTableDataSource<any>;
  @ViewChild("Paginator") Paginator: MatPaginator;
  @ViewChild("Sort") Sort: MatSort;
  DTablesData: MatTableDataSource<any>;
  @ViewChild("DPaginator") DPaginator: MatPaginator;
  @ViewChild("DSort") DSort: MatSort;
  selection = new SelectionModel<any>(true, []);
  TSPsArray: any[];
  tspUserIDsArray: any[];
  isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.DTablesData.data.length;
    return numSelected === numRows;
  }
  masterToggle() {
    this.isAllSelected() ?
      this.selection.clear() :
      this.DTablesData.data.forEach(row => this.selection.select(row));
  }
  currentUser: any;
  TapIndex: any;
  ngOnInit(): void {
    this.currentUser = this.ComSrv.getUserDetails();
    this.TapIndex = 0
    this.TablesData = new MatTableDataSource([]);
    this.DTablesData = new MatTableDataSource([]);
    this.PageTitle();
    this.GetAssociationFee();
    this.GetTSPTradelot();
  }
  PageTitle(): void {
    this.ComSrv.setTitle(this.AcitveRoute.snapshot.data.title);
    this.SpacerTitle = this.AcitveRoute.snapshot.data.title;
  }
  TotalSelectedLocation: number = 0;
  TotalAssociationFee: number = 0;
  AssociationFee: any[] = []
  async GetAssociationFee() {
    this.SPName = "RD_SSPAssociationFee"
    this.paramObject = {}
    this.AssociationFee = []
    this.AssociationFee = await this.FetchData(this.SPName, this.paramObject)
    if (this.AssociationFee.length > 0) {
    } else {
      this.ComSrv.ShowError("No Record found.Association Fee insertion is required");
    }
  }
  onCheckboxChange() {
    // const AssociationFee = this.AssociationFee[0].AssociationFee
    // setTimeout(() => {
    //   this.TotalAssociationFee = this.selection.selected.length * AssociationFee
    // }, 0);
  }
  TSPTradelot = []
  async GetTSPTradelot() {
    const Program: any = this.ComSrv.getMessage()
    if (Program.ProgramID == undefined) {
      this.router.navigateByUrl('/association-management/association-submission');
      return
    }
    this.SPName = "RD_SSPTSPAssociationSubmission"
    this.paramObject = {
      UserID: this.currentUser.UserID,
      ProgramID: Program.ProgramID
    }
    this.TSPTradelot = []
    this.TSPTradelot = await this.FetchData(this.SPName, this.paramObject)
    if (this.TSPTradelot.length > 0) {
      this.LoadMatTable(this.TSPTradelot.filter(d => d.PaymentStatus == 'Pending'), "PendingPaymentLocation")
      this.LoadMatTable(this.TSPTradelot.filter(d => d.PaymentStatus != 'Pending'), "PaidPaymentLocation")
    } else {
      this.ComSrv.ShowError("No Record found.Please Map Trade with TrainingLocation");
    }
  }
  GetParamString(SPName: string, paramObject: any) {
    let ParamString = SPName;
    for (const key in paramObject) {
      if (Object.hasOwnProperty.call(paramObject, key)) {
        ParamString += `/${key}=${paramObject[key]}`;
      }
    }
    return ParamString;
  }
  paramObject: any = {}
  ExportReportName: string = ""
  SPName: string = ""
  async FetchData(SPName: string, paramObject: any) {
    try {
      const Param = this.GetParamString(SPName, paramObject);
      const data: any = await this.ComSrv.getJSON(`api/BSSReports/FetchReportData?Param=${Param}`).toPromise();
      if (data.length > 0) {
        return data;
      } else {
        this.ComSrv.ShowWarning(' No Record Found', 'Close');
      }
    } catch (error) {
      this.error = error;
    }
  }
  ProceedPayment() {
    if (this.selection.selected.length === 0) {
      this.ComSrv.ShowError("At least one TradeLot must be selected to proceed the payment.", "Close", 5000);
      return;
    }
    for (const selected of this.selection.selected) {
      if (!selected.NoOfClass || selected.NoOfClass <= 0) {
        this.ComSrv.ShowError("The Number of Class for the selected 'TradeLot' must be greater than 0 to proceed the payment.", "Close", 5000);
        return;
      }
    }
    const data = {
      UserID: this.currentUser.UserID,
      NoOFClasses: this.TotalClasses,
      PerLocationAssociationFee: this.AssociationFee[0].AssociationFee,
      TotalAssociationFee: this.TotalAssociationFee,
      AssociatedTradeLot: [...this.selection.selected]
    }
    
    console.log(data)
    
    if (this.selection.selected.length > 0) {
      this.ComSrv.postJSON("api/Payment/SaveAssociationPayment", data).subscribe(
        (response) => {
          this.GetTSPTradelot();
        },
        (error) => {
          console.log(error.error[1].Description)
          this.ComSrv.ShowError(`TSP ${error.error[1].Description}`, "Close", 500000);
        }
      )
    } else {
      this.ComSrv.ShowError("At least one TradeLot must be selected to proceed the payment.", "Close", 5000);
    }
  }
  OpenDialogue(row) {
    const data = [row, [1]];
    const dialogRef = this.Dialog.open(TspStatusUpdateComponent, {
      width: '40%',
      data: data,
      disableClose: true,
    });
    dialogRef.afterClosed().subscribe(result => {
      if (result === true) {
        // this.ShowDetail(this.rowData, this.tspName, 0);
        // this.FetchRecord()
      }
    });
  }
  EmptyCtrl(ev: any) {
    this.TSearchCtr.setValue('');
  }
  camelCaseToWords(input: string): string {
    return input.replace(/([a-z])([A-Z])/g, '$1 $2');
  }
  LoadMatTable(tableData: any[], ReportName: string) {
    if (ReportName == "PendingPaymentLocation" && tableData.length == 0) {
      this.DTableColumns = []
      this.DTablesData = new MatTableDataSource([]);
    }
    if (ReportName == "PaidPaymentLocation" && tableData.length == 0) {
      this.TableColumns = []
      this.TablesData = new MatTableDataSource([]);
    }
    if (ReportName == "PendingPaymentLocation" && tableData.length > 0) {
      const excludeColumnArray = ["TrainingLocation", "IsChecked", "TotalNoOfClass"]
      this.DTableColumns = Object.keys(tableData[0]).filter(key => !key.includes('ID') && !excludeColumnArray.includes(key));
      this.DTablesData = new MatTableDataSource(tableData);
      this.DTablesData.paginator = this.DPaginator;
      this.DTablesData.sort = this.DSort;
    }
    if (ReportName == "PaidPaymentLocation" && tableData.length > 0) {
      const excludeColumnArray = ["TrainingLocation", "IsChecked", "TradeLot", "NoOfClass"]
      this.TableColumns = Object.keys(tableData[0]).filter(key => !key.includes('ID') && !excludeColumnArray.includes(key));
      this.TablesData = new MatTableDataSource(tableData);
      this.TablesData.paginator = this.Paginator;
      this.TablesData.sort = this.Sort;
    }
  }
  applyFilter(data: MatTableDataSource<any>, event: any) {
    data.filter = event.target.value.trim().toLowerCase();
    if (data.paginator) {
      data.paginator.firstPage();
    }
  }
  DataExcelExport(Data: any, ReportName: string) {
    this.ComSrv.ExcelExporWithForm(Data, ReportName);
  }
  ShowPreview(fileName: string) {
    this.ComSrv.PreviewDocument(fileName)
  }
  ngAfterViewInit() {
    if (this.tabGroup) {
      this.tabGroup.selectedTabChange.subscribe((event) => {
        this.TapIndex = event.index
        if (this.TapIndex == 0) {
          this.isChecked = false
        }
      });
    }
  }
}
