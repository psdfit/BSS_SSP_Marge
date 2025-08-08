import { ChangeDetectorRef, Component, OnInit, ViewChild } from "@angular/core";
import { CommonSrvService } from "../../common-srv.service";
import { ActivatedRoute } from "@angular/router";
import { MatTableDataSource } from "@angular/material/table";
import { MatPaginator, PageEvent } from "@angular/material/paginator";
import { MatSort } from "@angular/material/sort";
import { MatTabGroup } from "@angular/material/tabs";
import { MatDialog, MatDialogRef } from "@angular/material/dialog";
import {
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from "@angular/forms";
import { environment } from "../../../environments/environment";
import { EnumApprovalProcess } from "src/app/shared/Enumerations";
import { DialogueService } from "src/app/shared/dialogue.service";
import { ProgramReviewComponent } from "src/app/custom-components/program-review/program-review.component";
@Component({
  selector: "app-annual-plan-approval",
  templateUrl: "./annual-plan-approval.component.html",
  styleUrls: ["./annual-plan-approval.component.scss"],
})
export class AnnualPlanApprovalComponent implements OnInit {
  [x: string]: any;
  isChecked: boolean = true;
  BSearchCtr = new FormControl("");
  TspName: any;
  rowData: any;
  tspName: string;
  ResponseData: any = [];
  tradeWiseTarget: any;
  lotWiseTarget: any;
  programDesign: any;
  programData: any;
  programId: string;
  constructor(
    private ComSrv: CommonSrvService,
    private AcitveRoute: ActivatedRoute,
    private fb: FormBuilder,
    private cdr: ChangeDetectorRef,
    private dialogue: DialogueService,
    private Dialog: MatDialog
  ) {}
  environment = environment;
  error: any;
  GetDataObject: any = {};
  SpacerTitle: string;
  TSearchCtr = new FormControl("");
  TapTTitle: string = "Profile";
  @ViewChild("tabGroup") tabGroup: MatTabGroup;
  TablesData: MatTableDataSource<any>;
  @ViewChild("Paginator") Paginator: MatPaginator;
  @ViewChild("Sort") Sort: MatSort;
  TableColumns = [];
  tradeTablesData: MatTableDataSource<any>;
  @ViewChild("tPaginator") tPaginator: MatPaginator;
  @ViewChild("tSort") tSort: MatSort;
  tradeTableColumns = [];
  lotTablesData: MatTableDataSource<any>;
  @ViewChild("lPaginator") lPaginator: MatPaginator;
  @ViewChild("lSort") lSort: MatSort;
  lotTableColumns = [];
  TSPsArray: any[];
  tspUserIDsArray: any[];
  currentUser: any;
  TapIndex = 0;
  ngOnInit(): void {
    this.currentUser = this.ComSrv.getUserDetails();
    this.TablesData = new MatTableDataSource([]);
    this.PageTitle();
    this.LoadData();
    this.InitAnnualPlanInfo();
  }
  AnnualPlanInfoForm: FormGroup;
  InitAnnualPlanInfo() {
    this.AnnualPlanInfoForm = this.fb.group({
      ProgramID: [""],
      Program: [{ value: "", disabled: true }],
      PlaningTypeID: [""],
      SelectionMethodID: [""],
    });
  }
  IsDisabled = false;
  SaveAnnualPlanInfo() {
    console.log(this.AnnualPlanInfoForm);
    if (this.AnnualPlanInfoForm.valid) {
      this.IsDisabled = true;
      this.ComSrv.postJSON(
        "api/ProgramDesign/UpdateProgramDesign",
        this.AnnualPlanInfoForm.getRawValue()
      ).subscribe(
        (response: any[]) => {
          this.AnnualPlanInfoForm.reset();
          this.selectedRow = {};
          this.IsDisabled = false;
          this.ComSrv.openSnackBar("Saved data");
          this.LoadData();
        },
        (error) => {
          this.error = error.error;
          this.ComSrv.ShowError(error.error, "error", 5000);
        }
      );
    } else {
      this.ComSrv.ShowError("please enter valid data");
    }
  }
  selectedRow: any = {};
  updatePlanningType(row: any) {
    this.selectedRow = {};

    if (row.IsFinalApproved == true) {
      this.ComSrv.ShowError(
        "This record is locked because the selected program has received final approval."
      );
      return;
    }
    this.selectedRow = row;
    this.AnnualPlanInfoForm.get("Program").setValue(row.ProgramName);
    this.AnnualPlanInfoForm.get("ProgramID").setValue(row.ProgramID);
    this.AnnualPlanInfoForm.get("PlaningTypeID").setValue(
      this.PlaningType.find((x) => row.PlaningType == x.PlaningType)
        ?.PlaningTypeID || ""
    );
    this.AnnualPlanInfoForm.get("SelectionMethodID").setValue(
      this.SelectionMethods.find((x) => row.SelectionMethod == x.MethodName)
        ?.ID || ""
    );
  }
  PlaningType: any[];
  SelectionMethods: any[];
  ShowDetail(row: any, tspName: string) {
    this.isChecked = true;
    this.tradeTablesData = new MatTableDataSource([]);
    this.lotTablesData = new MatTableDataSource([]);
    this.rowData = [];
    this.tspName = "";
    this.rowData = row;
    this.tspName = tspName;
    this.tabGroup.selectedIndex = 1;
    this.TspName = tspName + " Detail";

    this.tradeWiseTarget = this.GetDataObject.tradeWiseTarget.filter(
      (d) => d.ProgramName == this.rowData.ProgramName
    );
    this.lotWiseTarget = this.GetDataObject.lotWiseTarget.filter(
      (d) => d.ProgramID == this.rowData.ProgramID
    );
    this.LoadMatTable(this.tradeWiseTarget, "TradeDetail");
    this.LoadMatTable(this.lotWiseTarget, "TradeLotDetail");
    // } else {
    //   this.ComSrv.ShowError("Required form fields are missing");
    // }
  }
  LoadData() {
    this.ComSrv.postJSON("api/ProgramDesign/LoadSchemeData", {
      UserID: this.currentUser.UserID,
    }).subscribe(
      (response) => {
        this.GetDataObject = response;
        this.SelectionMethods = this.GetDataObject.selectionMethods;
        this.PlaningType = this.GetDataObject.planingType.filter(
          (d) => d.PlaningTypeID != 3
        );
        const ProgramTablesData =
          this.GetDataObject.programDesignSummary.filter(
            (d) =>
              d.IsInitiated == true &&
              (d.IsFinalApproved == true || d.IsFinalApproved == false)
          );
        this.LoadMatTable(ProgramTablesData, "Program");
      },
      (error) => {
        this.ComSrv.ShowError(`${error.message}`, "Close", 500000);
      }
    );
  }
  EmptyCtrl() {
    this.BSearchCtr.setValue("");
  }
  PageTitle(): void {
    this.ComSrv.setTitle(this.AcitveRoute.snapshot.data.title);
    this.SpacerTitle = this.AcitveRoute.snapshot.data.title;
  }
  LoadMatTable(tableData: any, dataType: string) {
    console.log(this.currentUser.RoleTitle);
    this.cdr.detectChanges();
    switch (dataType) {
      case "Program":
        let excludeColumnArray = [
          "WorkflowRemarks",
          "SSPWorkflow",
          "IsSubmitted",
          "ProcessStatus",
          "AssociationStartDate",
          "AssociationEndDate",
          "IsWorkflowAttached",
          "Workflow",
          "IsCriteriaAttached",
          "Criteria",
          "CriteriaRemarks",
          "StartDate",
          "EndDate",
          "StatusRemarks",
          "IsFinalApproved",
          "IsInitiated",
        ];

        if (this.currentUser.RoleTitle == "Program Development") {
          excludeColumnArray = [
            ...excludeColumnArray,
            "PlaningType",
            "SelectionMethod",
          ];
        }

        this.TableColumns = Object.keys(tableData[0]).filter(
          (key) => !key.includes("ID") && !excludeColumnArray.includes(key)
        );

        this.TableColumns.unshift("Action");
        this.TablesData = new MatTableDataSource(tableData);
        this.TablesData.paginator = this.Paginator;
        this.TablesData.sort = this.Sort;
        break;
      case "TradeDetail":
        this.tradeTableColumns = Object.keys(tableData[0]).filter(
          (key) => !key.includes("ID")
        );
        this.tradeTablesData = new MatTableDataSource(tableData);
        this.tradeTablesData.paginator = this.tPaginator;
        this.tradeTablesData.sort = this.tSort;
        break;
      case "TradeLotDetail":
        this.lotTableColumns = Object.keys(tableData[0]).filter(
          (key) => !key.includes("ID")
        );
        this.lotTablesData = new MatTableDataSource(tableData);
        this.lotTablesData.paginator = this.lPaginator;
        this.lotTablesData.sort = this.lSort;
        break;
      default:
        console.warn(`Unhandled dataType: ${dataType}`);
        break;
    }
  }
  camelCaseToWords(input: string): string {
    return input.replace(/([a-z])([A-Z])/g, "$1 $2");
  }
  applyFilter(data: MatTableDataSource<any>, event: any) {
    data.filter = event.target.value.trim().toLowerCase();
    if (data.paginator) {
      data.paginator.firstPage();
    }
  }
  DataExcelExport(data: any, title) {
    this.ComSrv.ExcelExporWithForm(data, title);
  }
  openApprovalDialogue(row: any): void {
    let processKey = EnumApprovalProcess.PROG_APP;
    this.dialogue
      .openApprovalDialogue(processKey, row.ProgramID)
      .subscribe((result) => {
        console.log(result);
        this.LoadData();
      });
  }

  openProgramReviewDialogue(row: any = {}): void {
    // Prepare data for ProgramPreviewComponent dialog
    const tradeWiseTarget = this.GetDataObject.tradeWiseTarget.filter(
      (d) => d.ProgramName == row.ProgramName
    );
    const lotWiseTarget = this.GetDataObject.lotWiseTarget.filter(
      (d) => d.ProgramID == row.ProgramID
    );

    const data = {
      programData: row,
      tradeData: tradeWiseTarget,
      lotData: lotWiseTarget,
    };
    const dialogRef = this.Dialog.open(ProgramReviewComponent, {
      width: "97%",
      data: data,
      disableClose: true,
    });
    dialogRef.afterClosed().subscribe((result) => {
      // if (result === true) {
      //   this.GetData()
      // }
    });
  }
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  onPageChange(event: PageEvent): void {
    this.currentPage = event.pageIndex + 1;
    this.pageSize = event.pageSize;
  }
  //   TablesData: any[] = []; // Your data array
  // TableColumns: string[] = []; // Your column definitions
  // currentUser: any = {}; // Your user object
  pageSize = 10;
  currentPage = 1;
  totalItems = 0;
  // environment: any = { DateFormat: 'MM/dd/yyyy' }; // Your environment config

  getErrorMessage(errorKey: string, errorValue: any): string {
    const errorMessages = {
      required: "This field is required.",
      minlength: `This field must be at least ${
        errorValue.requiredLength - 1
      } characters long.`,
      maxlength: `This field's text exceeds the specified maximum length.  (maxLength: ${errorValue.requiredLength} characters)`,
      email: "Invalid email address.",
      pattern: "This field is only required text",
      customError: errorValue,
    };
    return errorMessages[errorKey];
  }
  ngAfterViewInit() {
    if (this.tabGroup) {
      this.tabGroup.selectedTabChange.subscribe((event) => {
        console.log(event);
        this.TapIndex = event.index;
        if (this.TapIndex == 0) {
          this.isChecked = false;
        }
        if (this.TapIndex == 1) {
          this.isChecked = true;
        }
      });
    }
  }
}
