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
  selector: 'app-registration-payment',
  templateUrl: './registration-payment.component.html',
  styleUrls: ['./registration-payment.component.scss']
})

export class RegistrationPaymentComponent implements OnInit {
  isChecked: boolean = false
  isEdit: number;
  constructor(
    private ComSrv: CommonSrvService,
    private AcitveRoute: ActivatedRoute,
    private fb: FormBuilder,
    private Cdr: ChangeDetectorRef,
    private Dialog: MatDialog,
  ) { }

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
    this.GetRegistrationFee();
    this.GetTSPTrainingLocationWithTradeCount();
  }

  PageTitle(): void {
    this.ComSrv.setTitle(this.AcitveRoute.snapshot.data.title);
    this.SpacerTitle = this.AcitveRoute.snapshot.data.title;
  }


  RegistrationFee: any[] = []
  TotalSelectedLocation: number = 0;
  TotalRegistrationFee: number = 0;

  async GetRegistrationFee() {
    this.SPName = "RD_SSPRegistrationFee"
    this.paramObject = {}
    this.RegistrationFee = []
    this.RegistrationFee = await this.FetchData(this.SPName, this.paramObject)

    if(this.RegistrationFee==undefined){
      this.ComSrv.ShowError("No Record found.Registration Fee insertion is required");
    }
   
  }

  onCheckboxChange() {
    const RegistrationFee = this.RegistrationFee[0].RegistrationFee
    setTimeout(() => {
      this.TotalRegistrationFee = this.selection.selected.length * RegistrationFee
    }, 0);
  }

  TSPTrainingLocationWithTradeCount = []
  async GetTSPTrainingLocationWithTradeCount() {
    this.SPName = "RD_SSPTSPTrainingLocationWithTradeCount"
    this.paramObject = {
      UserID: this.currentUser.UserID
    }
    this.TSPTrainingLocationWithTradeCount = []
    this.TSPTrainingLocationWithTradeCount = await this.FetchData(this.SPName, this.paramObject)
    if (this.TSPTrainingLocationWithTradeCount.length > 0) {
      this.LoadMatTable(this.TSPTrainingLocationWithTradeCount.filter(d => d.PaymentStatus == 'Pending'), "PendingPaymentLocation")
      this.LoadMatTable(this.TSPTrainingLocationWithTradeCount.filter(d => d.PaymentStatus != 'Pending'), "PaidPaymentLocation")
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

      if (data != undefined) {
        return data;
      } else {
        this.ComSrv.ShowWarning(' No Record Found', 'Close');
      }
    } catch (error) {
      this.error = error;
    }
  }

  ProceedPayment() {
    const data = {
      UserID: this.currentUser.UserID,
      NoOFLocation: this.selection.selected.length,
      PerLocationRegistrationFee: this.RegistrationFee[0].RegistrationFee,
      TotalRegistrationFee: this.TotalRegistrationFee,
      RegisteredLocations: [...this.selection.selected]
    }
    if (this.selection.selected.length > 0) {
      this.ComSrv.postJSON("api/Payment/SaveRegistrationPayment", data).subscribe(
        (response) => {
          this.GetTSPTrainingLocationWithTradeCount();
          
        },
        (error) => {
          console.log(error.error[1].Description)
          this.ComSrv.ShowError(`TSP ${error.error[1].Description}`, "Close", 500000);
        }
      )
    } else {
      this.ComSrv.ShowError("At least one location must be selected to proceed the payment.", "Close", 5000);
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
  
  
    
    if (ReportName == "PendingPaymentLocation" && tableData.length == 0){
      this.DTableColumns=[]
      this.DTablesData = new MatTableDataSource([]);
    }

    if (ReportName == "PaidPaymentLocation" && tableData.length == 0){
      this.TableColumns=[]
      this.TablesData = new MatTableDataSource([]);
    }

    if (ReportName == "PendingPaymentLocation" && tableData.length > 0) {
      const excludeColumnArray = ["PaymentStatus", "InvoiceNo", "PaymentID"]
      this.DTableColumns = Object.keys(tableData[0]).filter(key => !key.includes('UserID') && !excludeColumnArray.includes(key));
      this.DTablesData = new MatTableDataSource(tableData);
      this.DTablesData.paginator = this.DPaginator;
      this.DTablesData.sort = this.DSort;
    }
    if (ReportName == "PaidPaymentLocation" && tableData.length > 0) {
      const excludeColumnArray = ["TrainingLocationID"]
      this.TableColumns = Object.keys(tableData[0]).filter(key => !key.includes('UserID') && !excludeColumnArray.includes(key));
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
