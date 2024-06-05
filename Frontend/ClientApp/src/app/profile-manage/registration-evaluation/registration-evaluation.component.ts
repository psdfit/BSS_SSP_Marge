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
import { event } from "jquery";
import { GeoTaggingComponent } from "src/app/custom-components/geo-tagging/geo-tagging.component";
import { TspStatusUpdateComponent } from "src/app/custom-components/tsp-status-update/tsp-status-update.component";
// import { DialogueService } from "src/app/shared/dialogue.service";
// import { DatePipe, TitleCasePipe, formatCurrency } from "@angular/common";
// import { GroupByPipe } from "angular-pipes";
import { SelectionModel } from '@angular/cdk/collections';
@Component({
  selector: 'app-registration-evaluation',
  templateUrl: './registration-evaluation.component.html',
  styleUrls: ['./registration-evaluation.component.scss']
})
export class RegistrationEvaluationComponent implements OnInit {
  isChecked: boolean = false
  TspName: any;
  rowData: any;
  tspName: string;
  ResponseData: any = []
  isEdit: number;
  ResponsD: any=[]
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
  @ViewChild("Paginator") Paginator: MatPaginator;
  @ViewChild("Sort") Sort: MatSort;
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
    // console.log(numRows)
  }
  currentUser: any;
  TapIndex: any;
  TablesData: MatTableDataSource<any>;
  DTablesData: MatTableDataSource<any>;
  ngOnInit(): void {
    this.currentUser = this.ComSrv.getUserDetails();
    this.TapIndex = 0
    this.TablesData = new MatTableDataSource([]);
    this.DTablesData = new MatTableDataSource([]);
    this.PageTitle();
    this.GetData();
    this.InitFilterForm();
  }
  PageTitle(): void {
    this.ComSrv.setTitle(this.AcitveRoute.snapshot.data.title);
    this.SpacerTitle = this.AcitveRoute.snapshot.data.title;
  }
  CurrentDate: Date = new Date()
  FilterForm: FormGroup;
  InitFilterForm() {
    this.FilterForm = this.fb.group({
      UserID: [this.currentUser.UserID],
      StartDate: [this.CurrentDate, Validators.required],
      EndDate: [this.CurrentDate, Validators.required],
    });
  }
  ShowDetail(row: any, tspName: string, isEdit:number=0) {
    this.selection.clear()
    this.rowData = []
    this.tspName = ""
    this.rowData = row
    this.tspName = tspName
    this.tabGroup.selectedIndex = 1;
    this.isChecked = true
    this.TspName = tspName + " Detail"
    this.isEdit = isEdit == 0 ? 3 : 0
    if (this.rowData.UserID) {
      this.ComSrv.postJSON("api/BusinessProfile/FetchTspDetail", { UserID: this.rowData.UserID, ApprovalLevel: this.isEdit }).subscribe(
        (response) => {
          this.ResponseData = response
          if (this.ResponseData.length > 0) {
            this.LoadMatTable(response, "TspDetail")
          } else {
            this.tabGroup.selectedIndex = 0;
            this.ComSrv.ShowError("No record found");
          }
        },
        (error) => {
          this.ComSrv.ShowError(`${error.message}`, "Close", 500000);
        }
      );
    } else {
      this.ComSrv.ShowError("Required form fields are missing");
    }
  }
  Decision() {

    if (this.selection.selected.length > 0) {
      this.OpenDialogue(this.selection.selected)
    } else {
      this.ComSrv.ShowError("Minimum One record is required.");
    }
  }
  OpenDialogue(row) {
    const data = [row, [1]];
    const dialogRef = this.Dialog.open(TspStatusUpdateComponent, {
      // height: '45%',
      width: '35%',
      data: data,
      disableClose: true,
    });
    dialogRef.afterClosed().subscribe(result => {
      this.selection.clear()
      if (result === true) {
        this.ShowDetail(this.rowData, this.tspName, 0);
        this.FetchRecord()
      }
    });
  }
  GetData() {
    this.FetchRecord()
  }
  FetchRecord() {
    this.ComSrv.postJSON("api/BusinessProfile/FetchTspRegistration", { UserID: this.currentUser.UserID }).subscribe(
      (response) => {
        this.ResponsD=response
        console.log(this.ResponsD)
        if(this.ResponsD.length>0){
          
          this.LoadMatTable(response, "TspMaster");
        }
      },
      (error) => {
        this.ComSrv.ShowError(`${error.message}`, "Close", 500000);
      }
    );
  }
  EmptyCtrl(ev: any) {
    this.TSearchCtr.setValue('');
  }
  camelCaseToWords(input: string): string {
    return input.replace(/([a-z])([A-Z])/g, '$1 $2');
  }
  LoadMatTable(tableData: any, tableName: string) {
    if (tableName == "TspDetail") {
      this.DTableColumns = Object.keys(tableData[0])
      this.DTablesData = new MatTableDataSource(tableData);
      this.DTablesData.paginator = this.DPaginator;
      this.DTablesData.sort = this.DSort;
    } else {
      this.TableColumns = Object.keys(tableData[0]).filter(key => !key.includes('ID'));
      this.TableColumns.unshift("Action")
      this.TablesData = new MatTableDataSource(tableData)
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
