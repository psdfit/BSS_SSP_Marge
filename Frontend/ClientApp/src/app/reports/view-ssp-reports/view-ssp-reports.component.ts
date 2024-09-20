
import { AfterViewInit, Component, OnInit, ViewChild } from "@angular/core";
import { MatPaginator } from "@angular/material/paginator";
import { MatSort } from "@angular/material/sort";
import { MatTableDataSource } from "@angular/material/table";

import { DatePipe } from "@angular/common";
import { DecimalPipe } from "@angular/common";

import { CommonSrvService } from "../../common-srv.service";
import { ActivatedRoute } from "@angular/router";
import {
  Form,
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from "@angular/forms";
import { deflate } from "zlib";

@Component({
  selector: "app-view-ssp-reports",
  templateUrl: "./view-ssp-reports.component.html",
  styleUrls: ["./view-ssp-reports.component.scss"],
  providers: [DatePipe, DecimalPipe],
})
export class ViewSspReportsComponent implements OnInit,AfterViewInit  {
  currentUser: any;
  TableData: MatTableDataSource<[any]>;
  TableColumns: string[] = [];
  SpacerTitle: any;
  dataObject: any;
  error: string;
  subReportsArray: any[];

  reportsArray: any;
  roleID: any;
  genForm: FormGroup;

  SearchRName = new FormControl("");
  SearchSRName = new FormControl("");

  paramObject: any = {};
  ExportReportName: string = "";
  SPName: string = "";

  maxDate: Date = new Date();

  constructor(
    private ComSrv: CommonSrvService,
    public DatePipe: DatePipe,
    private ActiveRoute: ActivatedRoute,
    private fb: FormBuilder
  ) {}

 
  ngOnInit() {
    this.ComSrv.setTitle("Application Log");
    this.TableData = new MatTableDataSource([]);
    this.currentUser = this.ComSrv.getUserDetails();
    this.roleID = this.currentUser.RoleID;
    this.PageTitle();
    // this.loadData();
    this.getReportsName();
    this.getSubReportsFilters();
    this.formGen();
  }

  formGen() {
    this.genForm = this.fb.group({
      ReportName: [40, Validators.required],
      SubReportName: ["", Validators.required],
      StartDate: [this.maxDate, Validators.required],
      EndDate: [this.maxDate, Validators.required],
    });
  }


  PageTitle(): void {
    this.ComSrv.setTitle(this.ActiveRoute.snapshot.data.title);
    this.SpacerTitle = this.ActiveRoute.snapshot.data.title;
  }

  @ViewChild("Paginator") Paginator: MatPaginator;
  @ViewChild("Sort") Sort: MatSort;



  attachmentColumnArray: string[] = [
    "QualEvidence",
    "TrainerCV",
    "ProfQualEvidence",
    "RelExpLetter",
    "NTNAttachment",
    "PRAAttachment",
    "GSTAttachment",
    "LegalStatusAttachment",
    "HeadCnicFrontImg",
    "HeadCnicBackImg",
  ];
  LoadMatTable(reportData: any[], reportTitle: string): void {
    if (reportData.length > 0) {
      const excludeColumnArray: string[] = [
        "NTNEvidence","RegistrationCertificateEvidence",
        "PRAEvidence",
        "GSTEvidence",
        "legalStatusEvidence",
        "OrgHeadCNICFrontImgUrl",
        "OrgHeadCNICBackImgUrl",
        "LegalStatusEvidence",
      ];

      this.TableColumns = Object.keys(reportData[0]).filter(
        (key) => !key.includes("ID") && !excludeColumnArray.includes(key)
      );
      this.TableColumns = ["SrNo", ...this.TableColumns];
      this.TableData = new MatTableDataSource(reportData);
      this.TableData.paginator = this.Paginator;
      this.TableData.sort = this.Sort;
    }
  }
  camelCaseToWords(input: string): string {
    return input.replace(/([a-z])([A-Z])/g, "$1 $2");
  }

  applyFilter(data: MatTableDataSource<any>, event: any) {
    data.filter = event.trim().toLowerCase();
    if (data.paginator) {
      data.paginator.firstPage();
    }
  }

  DataExcelExport(data: any[], title) {
    if (data.length > 0) {
      this.ComSrv.ExcelExporWithForm(data, title);
    } else {
      this.ComSrv.ShowError("No Record Found");
    }
  }

  async getReportsName() {
    this.ComSrv.getJSON(`api/Reports/RD_Reports/${this.roleID}`).subscribe(
      (data: any) => {
        this.error = "";
        
        // Clear previous report arrays
        this.subReportsArray = [];
        
        // Filter reports by "Registration Reports"
        this.reportsArray = data.filter((x: any) => x.ReportName === "Registration Reports");
        
        // Set the value of "ReportName" field
        this.genForm.get("ReportName")?.setValue(40);
      },
      (error) => {
        this.error = `${error.name}, ${error.statusText}`;
      }
    );
  }
  
  
  getSubReportsName(ReportID: any=40) {
    debugger;
    this.ComSrv.getJSON(
      `api/Reports/RD_SubReports?ReportID=` + ReportID.ReportID
    ).subscribe(
      (data: any) => {
        this.error = "";
        this.subReportsArray = data;
        this.genForm.get("SubReportName").setValue(56);
      },
      (error) => {
        this.error = `${error.name} , ${error.statusText}`;
      }
    );
  }

  async getSubReportsFilters(SubReport: any = "") {
    debugger;
    switch (SubReport.SubReportName) {
      case "TSP Profile Report":
        this.GetTSPProfile();
        break;

      case "Trainer Report":
        this.SPName = "RD_SSPTrainerRPT";
        this.paramObject = {};
        this.DataTable = await this.FetchData(this.SPName, this.paramObject);
        this.LoadMatTable(this.DataTable, "Trainer Report");
        break;

      case "TSP POC Report":
        this.SPName = "RD_SSPTSPContactPersonRPT";
        this.paramObject = {};
        this.DataTable = await this.FetchData(this.SPName, this.paramObject);
        this.LoadMatTable(this.DataTable, "Trainer Report");
        break;

      case "TSP Bank Report":
        this.SPName = "RD_SSPBankDetailRPT";
        this.paramObject = {};
        this.DataTable = await this.FetchData(this.SPName, this.paramObject);
        this.LoadMatTable(this.DataTable, "Bank Detail");
        break;

      case "Registration Evaluation Report":
        this.SPName = "RD_SSPTSPRegistrationDetail";
        this.paramObject = {};
        this.DataTable = await this.FetchData(this.SPName, this.paramObject);
        this.LoadMatTable(this.DataTable, "Registration Evaluation Report");
        break;

      default:
        this.GetTSPProfile();

        break;
    }
  }

  EmptyCtrl() {
    this.SearchRName.setValue("");
    this.SearchSRName.setValue("");
  }

  DataTable: any = [];

  async GetTSPProfile() {
    debugger;
    this.SPName = "RD_SSPTSPProfileRPT";
    this.paramObject = {};
    this.DataTable = await this.FetchData(this.SPName, this.paramObject);
    debugger;
    this.LoadMatTable(
      this.DataTable.filter((x) => x.HeadName != ""),
      "TSP Profile"
    );
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

  async FetchData(SPName: string, paramObject: any) {
    try {
      const Param = this.GetParamString(SPName, paramObject);
      const data = await this.ComSrv.postJSON(
        `api/BSSReports/FetchReport`,
        Param
      ).toPromise();
      if (data != undefined) {
        return data;
      } else {
        if (SPName != "RD_SSPTSPAssociationSubmission") {
          this.ComSrv.ShowWarning(" No Record Found", "Close");
        }
      }
    } catch (error) {
      this.error = error;
      this.ComSrv.ShowError(error.error);
    }
  }

  ShowPreview(fileName: string) {
    this.ComSrv.PreviewDocument(fileName);
  }

  isDocumentColumn(column: string): boolean {
    const documentColumns = [
      "QualEvidence",
      "TrainerCV",
      "ProfQualificationEvidence",
      "RelExpLetter",
    ];
    return documentColumns.includes(column);
  }

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

  ngAfterViewInit(): void {
    debugger;
    // this.genForm.get("ReportName")?.setValue(40);
    // this.genForm.get("ReportName")?.setValue("40");
    console.log(this.genForm.value)

  }
}
