import { ChangeDetectorRef,Component, OnInit, ViewChild } from "@angular/core";
import { CommonSrvService } from "../../common-srv.service";
import { ActivatedRoute } from "@angular/router";
import { MatTableDataSource } from "@angular/material/table";
import { MatPaginator } from "@angular/material/paginator";
import { MatSort } from "@angular/material/sort";
import { MatTabGroup } from "@angular/material/tabs";
import { FormControl } from "@angular/forms";
import { MatDialog, MatDialogRef } from "@angular/material/dialog";
import { environment } from '../../../environments/environment';

import { TspStatusUpdateComponent } from "src/app/custom-components/tsp-status-update/tsp-status-update.component";
// import { DialogueService } from "src/app/shared/dialogue.service";
// import { DatePipe, TitleCasePipe, formatCurrency } from "@angular/common";
// import { GroupByPipe } from "angular-pipes";
import { SelectionModel } from '@angular/cdk/collections';

@Component({
  selector: 'app-registration-approval',
  templateUrl: './registration-approval.component.html',
  styleUrls: ['./registration-approval.component.scss']
})
export class RegistrationApprovalComponent implements OnInit {


isChecked: boolean=false
TspName: any;
  rowData: any;
  tspName: string;
  ResponseData:any=[]
  constructor(
    private ComSrv: CommonSrvService,
    private AcitveRoute: ActivatedRoute,
    private Dialog: MatDialog,
    private cdr: ChangeDetectorRef,
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
  DTablesData: MatTableDataSource<any>;
  ngOnInit(): void {
    this.currentUser = this.ComSrv.getUserDetails();
    this.TapIndex = 0
    this.DTablesData = new MatTableDataSource([]);
    this.PageTitle();
  }

  PageTitle(): void {
    this.ComSrv.setTitle(this.AcitveRoute.snapshot.data.title);
    this.SpacerTitle = this.AcitveRoute.snapshot.data.title;
  }




  ShowDetail() {
      this.ComSrv.postJSON("api/BusinessProfile/FetchTspDetail",{UserID:0,ApprovalLevel:1}).subscribe(
        (response) => {
          this.ResponseData=response
          if(this.ResponseData.length>0){
            this.LoadMatTable(response)
          }else{
            this.ComSrv.ShowError("No record found");
          }
        },
        (error) => {
          this.ComSrv.ShowError(`${error.message}`, "Close", 500000);
        }
      );

  }

  Decision() {
    console.log(this.selection.selected)

    if (this.selection.selected.length>0) {
      this.OpenDialogue(this.selection.selected)
    } else {
      this.ComSrv.ShowError("Minimum One record is required.");
    }
    }

    OpenDialogue(row) {
      const data = [row, [2]];

      const dialogRef = this.Dialog.open(TspStatusUpdateComponent, {
        height: '55%',
        width: '40%',
        data: data,
        disableClose: true,
      });

      dialogRef.afterClosed().subscribe(result => {
        if (result === true) {
          this.ShowDetail();
        }
      });
    }






  EmptyCtrl(ev: any) {
    this.TSearchCtr.setValue('');
  }

  camelCaseToWords(input: string): string {
    return input.replace(/([a-z])([A-Z])/g, '$1 $2');
  }

  LoadMatTable(tableData: any) {

    this.DTableColumns = Object.keys(tableData[0])
    // .filter(key =>!key.includes('ID') && !["Name", "Other"].includes(key));
    this.DTablesData = new MatTableDataSource(tableData);
    this.DTablesData.paginator = this.DPaginator;
    this.DTablesData.sort = this.DSort;

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
        if(this.TapIndex==0){
          this.isChecked=false
        }
      });

    }
  }
}
