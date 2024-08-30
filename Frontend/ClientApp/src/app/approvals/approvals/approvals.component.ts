import {
  AfterViewInit,
  ChangeDetectorRef,
  Component,
  OnInit,
  ViewChild,
} from "@angular/core";
import { MatPaginator } from "@angular/material/paginator";
import { MatSort } from "@angular/material/sort";
import { Workbook } from "exceljs";
import * as fs from "file-saver";
import { CommonSrvService } from "../../common-srv.service";
import { UserRightsModel } from "../../master-data/users/users.component";
import { DialogueService } from "../../shared/dialogue.service";
import {
  EnumApprovalProcess,
  EnumProgramCategory,
  AppendixImportSheetNames,
  EnumAppForms,
} from "../../shared/Enumerations";
import { DatePipe } from "@angular/common";
import { environment } from "../../../environments/environment";
import { MatTableDataSource } from "@angular/material/table";
import { DecimalPipe } from "@angular/common";
import { MatStepper } from "@angular/material/stepper";
import { StepperSelectionEvent } from "@angular/cdk/stepper";
@Component({
  selector: "app-approvals",
  templateUrl: "./approvals.component.html",
  styleUrls: ["./approvals.component.scss"],
  providers: [DatePipe, DecimalPipe],
})
export class ApprovalsComponent implements OnInit {
  // displayedColumnsScheme = [
  //   "SchemeName",
  //   "SchemeCode",
  //   "Description",
  //   "CreatedDate",
  //   "UserName",
  //   "PTypeName",
  //   "PCategoryName",
  //   "FundingSourceName",
  //   "FundingCategoryName",
  //   "PaymentSchedule",
  //   "Stipend",
  //   "StipendMode",
  //   "UniformAndBag",
  //   "MinimumEducation",
  //   "MaximumEducation",
  //   "MinAge",
  //   "MaxAge",
  //   "GenderName",
  //   "DualEnrollment",
  //   "ContractAwardDate",
  //   "BusinessRuleType",
  //   "OName",
  //   "Action",
  // ];
  // displayedColumnsTSPs = [
  //   "TSPName",
  //   "TSPCode",
  //   "Address",
  //   "TSPColor",
  //   "Tier",
  //   "NTN",
  //   "PNTN",
  //   "GST",
  //   "FTN",
  //   "DistrictName",
  //   "HeadName",
  //   "HeadDesignation",
  //   "HeadEmail",
  //   "HeadLandline",
  //   "OrgLandline",
  //   "CPName",
  //   "CPDesignation",
  //   "CPLandline",
  //   "CPEmail",
  //   "Website",
  //   "CPAdmissionsName",
  //   "CPAdmissionsDesignation",
  //   "CPAdmissionsLandline",
  //   "CPAdmissionsEmail",
  //   "CPAccountsName",
  //   "CPAccountsDesignation",
  //   "CPAccountsLandline",
  //   "CPAccountsEmail",
  //   "BankName",
  //   "BankAccountNumber",
  //   "AccountTitle",
  //   "BankBranch",
  //   "Organization",
  //   "Action",
  // ];

  @ViewChild(MatStepper) stepper: MatStepper;

  onStepChange(event: StepperSelectionEvent): void {
    const currentIndex = event.selectedIndex;
    if (this.ChosenSchemeID > 0) {
      if (currentIndex == 1) {
        this.GetTsps(this.ChosenSchemeID);
      }

      if (currentIndex == 2) {
        this.GetClasses();
      }

      if (currentIndex == 3) {
        this.GetInstructors();
      }
    } else {
      if(currentIndex !=0)
      this.http.ShowError("Please select a TSP from the scheme table");
    }

    // You can add any other logic you want to execute on step change here
  }
  schemeTableData: MatTableDataSource<[any]>;
  schemeTableColumns: string[] = [];
  @ViewChild("schemePaginator") schemePaginator: MatPaginator;
  @ViewChild("schemeSort") schemeSort: MatSort;

  tspTableData: MatTableDataSource<any>;
  tspTableColumns: string[] = [];
  @ViewChild("tspPaginator") tspPaginator: MatPaginator;
  @ViewChild("tspSort") tspSort: MatSort;

  classTableData: MatTableDataSource<any>;
  classTableColumns: string[] = [];
  @ViewChild("classPaginator") classPaginator: MatPaginator;
  @ViewChild("classSort") classSort: MatSort;

  instructorTableData: MatTableDataSource<any>;
  instructorTableColumns: string[] = [];
  @ViewChild("instructorPaginator") instructorPaginator: MatPaginator;
  @ViewChild("instructorSort") instructorSort: MatSort;

  LoadMatTable(reportData: any[], reportTitle: string): void {
    this.cdRef.detectChanges();
    if (reportData.length > 0) {
      const excludeColumnArray: string[] = [
        // "CreatedDate",
        "ModifiedDate",
        "InActive",
        "IsMigrated",
        "FinalSubmitted",
        "ContractAwardDate",
        "AssignedUser",
      ];
      if (reportTitle === "Scheme") {
        this.schemeTableColumns = Object.keys(reportData[0]).filter(
          (key) => !key.includes("ID") && !excludeColumnArray.includes(key)
        );
        this.schemeTableColumns = [
          "Actions",
          "Navigate",
          ...this.schemeTableColumns,
        ];
        this.schemeTableData = new MatTableDataSource(reportData);
        this.schemeTableData.paginator = this.schemePaginator;
        this.schemeTableData.sort = this.schemeSort;
      }

      if (reportTitle === "TSP") {
        this.tspTableColumns = Object.keys(reportData[0]).filter(
          (key) => !key.includes("ID") && !excludeColumnArray.includes(key)
        );
        this.tspTableColumns = ["No.", ...this.tspTableColumns];
        this.tspTableData = new MatTableDataSource(reportData);
        this.tspTableData.paginator = this.tspPaginator;
        this.tspTableData.sort = this.tspSort;
      }
      
      if (reportTitle === "Class") {
        this.classTableColumns = Object.keys(reportData[0]).filter(
          (key) => !key.includes("ID") && !excludeColumnArray.includes(key)
        );
        this.classTableColumns = ["No.", ...this.classTableColumns];
        this.classTableData = new MatTableDataSource(reportData);
        this.classTableData.paginator = this.classPaginator;
        this.classTableData.sort = this.classSort;
      }
      if (reportTitle === "Instructor") {
        this.instructorTableColumns = Object.keys(reportData[0]).filter(
          (key) => !key.includes("ID") && !excludeColumnArray.includes(key)
        );
        this.instructorTableColumns = ["No.", ...this.instructorTableColumns];
        this.instructorTableData = new MatTableDataSource(reportData);
        this.instructorTableData.paginator = this.instructorPaginator;
        this.instructorTableData.sort = this.instructorSort;
      }
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
  environment = environment;
  schemes: [];
  tsps: [];
  classes: [];
  instructors: any[];
  ActiveFormApprovalID: number;
  ChosenSchemeID: number;
  title: string;
  savebtn: string;
  formrights: UserRightsModel;
  EnText = "";
  error: string;
  query = {
    order: "SchemeID",
    limit: 5,
    page: 1,
  };
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  working: boolean;
  isFinancialUser: boolean = false;
  constructor(
    private decimalPipe: DecimalPipe,
    private http: CommonSrvService,
    private dialogue: DialogueService,
    private datePipe: DatePipe,
    private cdRef: ChangeDetectorRef
  ) {
    // this.schemes = new MatTableDataSource([]);
    this.formrights = http.getFormRights();
  }
  ngOnInit(): void {
    this.schemeTableData = new MatTableDataSource([]);
    this.tspTableData = new MatTableDataSource([]);
    this.classTableData = new MatTableDataSource([]);
    this.instructorTableData = new MatTableDataSource([]);
    this.http.setTitle("Appendix");
    this.title = "";
    this.savebtn = "Approve";
    this.http.OID.subscribe((OID) => {
      this.schemes = [];
      this.tsps = [];
      this.classes = [];
      this.instructors = [];
      this.GetSubmittedSchemesForMyID();
    });
  }
  GetSubmittedSchemesForMyID() {
    this.http
      .getJSON(`api/Scheme/GetSubmittedSchemes?OID=${this.http.OID.value}`)
      .subscribe(
        (d: any) => {
          // debugger;
          this.schemes = d;
          console.log(d[0]);
          const _schemeData = d.map((x) => ({
            IsApproved: x.IsApproved == true ? "YES" : "NO",
            Scheme: x.SchemeName,
            SchemeCode: x.SchemeCode,
            SchemeID: x.SchemeID,
            Description: x.Description,
            CreatedDate: this.datePipe.transform(x.CreatedDate , 'yyyy-MM-dd HH:mm:ss'),
            UserName: x.UserName,
            ProgramType: x.PTypeName,
            PRogramCategory: x.PCategoryName,
            FundingSource: x.FundingSourceName,
            FundingCategory: x.FundingCategoryName,
            PaymentSchedule: x.PaymentSchedule,
            Stipend: x.Stipend,
            StipendMode: x.StipendMode,
            UniformAndBag: x.UniformAndBag,
            MinimumEducation: x.MinimumEducationName,
            MaximumEducation: x.MaximumEducationName,
            MinAge: x.MinAge,
            MaxAge: x.MaxAge,
            Gender: x.GenderName,
            DualEnrollment: x.DualEnrollment == true ? "YES" : "NO",
            ContractAwardDate: x.ContractAwardDate,
            BusinessRuleType: x.BusinessRuleType,
            Organization: x.OName,
          }));

          this.LoadMatTable(_schemeData, "Scheme");
          // this.schemes.paginator = this.paginator;
          // this.schemes.sort = this.sort;
        },
        (error) => (this.error = error), // error path
        () => {
          this.working = false;
        }
      );
  }
  GetTsps(SchemeID) {
    this.ChosenSchemeID = SchemeID;
    this.http
      .getJSON("api/TSPDetail/GetTSPBySchemeID/" + this.ChosenSchemeID)
      .subscribe(
        (d: any) => {
          const _tspData = d.map((r) => ({
            "TSP Name": r.TSPName,
            "TSP Code": r.TSPCode,
            "TSP Color": r.TSPColor,
            Address: r.Address,
            Tier: r.TierName,
            NTN: r.NTN,
            PNTN: r.PNTN,
            GST: r.GST,
            FTN: r.FTN,
            District: r.DistrictName,
            "Head Name": r.HeadName,
            "Organization Head Designation": r.HeadDesignation,
            "Organization Head Email": r.HeadEmail,
            "Organization Head Landline/Mobile": r.HeadLandline,
            "Organization Landline": r.OrgLandline,
            'Website': r.Website,
            CreatedDate: this.datePipe.transform(r.CreatedDate , 'yyyy-MM-dd HH:mm:ss'),
            "Contact Person Name": r.CPName,
            "Contact Person Designation": r.CPDesignation,
            "Contact Person Landline/Mobile": r.CPLandline,
            "Contact Person Email": r.CPEmail,
            "Contact Person for Admission Name": r.CPAdmissionsName,
            "Contact Person for Admission Designation": r.CPAdmissionsDesignation,
            "Contact Person for Admission Landline/Mobile":r.CPAdmissionsLandline,
            "Contact Person for Admission Email": r.CPAdmissionsEmail,
            "Contact Person for Accounts Name": r.CPAccountsName,
            "Contact Person for Accounts Designation": r.CPAccountsDesignation,
            "Contact Person for Accounts Landline/Mobile": r.CPAccountsLandline,
            "Contact Person for Accounts Email": r.CPAccountsEmail,
            "Bank Name of TSP": r.BankName,
            "Account No./IBAN of TSP": r.BankAccountNumber,
            "Account Title": r.AccountTitle,
            "Bank Branch": r.BankBranch,
          }));

          this.tsps = d;
          this.LoadMatTable(_tspData, "TSP");

          this.stepper.selectedIndex = 1
          // this.schemes.paginator = this.paginator;
          // this.schemes.sort = this.sort;
        },
        (error) => (this.error = error), // error path
        () => {
          this.working = false;
        }
      );
  }
  GetClasses() {
    this.http
      .getJSON("api/Class/GetClassesBySchemeID/" + this.ChosenSchemeID)
      .subscribe(
        (d: any) => {
          // debugger;
          const _classesData = d.map((r, index) => ({
            "Class Code": r.ClassCode,
            "TSP Code": r.TSPCode,
            Sector: r.SectorName,
            Trade: r.TradeName,
            CreatedDate: this.datePipe.transform(r.CreatedDate , 'yyyy-MM-dd HH:mm:ss'),
            "Duration (Months)": r.Duration,
            "Source of Curriculum": r.SourceOfCurriculum,
            "Entry Qualification": r.EntryQualificationName,
            "Certification Authority": r.CertAuthName,
            "Registration Authority": r.RegistrationAuthorityName,
            "Program Focus": r.ProgramFocusName,
            "Contractual Trainees": r.TraineesPerClass,
            "Batch Number": r.Batch,
            "Min training hours/month": r.MinHoursPerMonth,
            "Start Date": r.StartDate,
            "End Date": r.EndDate,
            "Trainee Gender": r.GenderName,
            "Training Location Address": r.TrainingAddressLocation,
            "Geo Tagging (Lat,Long)": r.GeoTagging,
            Province: r.ProvinceName,
            District: r.DistrictName,
            Tehsil: r.TehsilName,
            Cluster: r.ClusterName,
            "Total Trainee Bid Price": this.decimalPipe.transform(r.BidPrice,"1.2-2"),
            "Total Trainee BM Price":  this.decimalPipe.transform(r.BMPrice,"1.2-2"),
            "Total Trainee Cost": this.decimalPipe.transform(r.TotalCostPerClass,"1.2-2"),
            "Sales Tax Rate": r.SalesTaxRate,
            "Training Cost per Trainee per Month (Exclusive of Taxes)":
              r.TrainingCostPerTraineePerMonthExTax,
            "Sales Tax": r.SalesTax,
            "Training Cost per Trainee per Month (Inclusive of Taxes)":
              r.TrainingCostPerTraineePerMonthInTax,
            "Uniform & Bag Cost per Trainee": r.UniformBagCost,
            "Testing & Certification Fee per Trainee": r.PerTraineeTestCertCost,
            "Boarding & Other Allowances per trainee":
              r.BoardingAllowancePerTrainee,
            "Employment Commitment Self": r.EmploymentCommitmentSelf,
            "Employment Commitment Formal": r.EmploymentCommitmentFormal,
            "Overall Employment Commitment": r.OverallEmploymentCommitment,
            Stipend: this.decimalPipe.transform(r.Stipend,"1.2-2"),
            "Balloon Payment": this.decimalPipe.transform(r.balloonpayment,"1.2-2"),
            "Total Cost": this.decimalPipe.transform(r.TotalCostPerClass,"1.2-2"),
          }));

          this.classes = d;
          this.LoadMatTable(_classesData, "Class");
        },
        (error) => (this.error = error), // error path
        () => {
          this.working = false;
        }
      );
  }
  GetInstructors() {
    this.http
      .getJSON("api/Instructor/GetInstructorsBySchemeID/" + this.ChosenSchemeID)
      .subscribe(
        (d: any) => {
          this.instructors = d;
          const instructorTableData = d.map((r, index) => ({
            Organization: r.NameOfOrganization,
            "Instructor Name": r.InstructorName,
            CreatedDate: this.datePipe.transform(r.CreatedDate , 'yyyy-MM-dd HH:mm:ss'),
            Gender: r.GenderName,
            "Profile Picture": r.PicturePath,
            "Total Experience": r.TotalExperience,
            "Highest Qualification": r.QualificationHighest,
            "Instructor CNIC": r.CNICofInstructor,
            Trade: r.TradeName,
            "Training Location": r.LocationAddress,
            "Class Code": r.ClassCode,
          }));

          this.LoadMatTable(instructorTableData, "Instructor");
        },
        (error) => (this.error = error), // error path
        () => {
          this.working = false;
        }
      );
  }
  exportExcel(schemeId, isClickedFinancial) {
    if (isClickedFinancial) {
      var userID = this.formrights.UserID;
      var formID = EnumAppForms.AppendixWithFinancialInformation;
      this.http
        .getJSON(`api/Users/CheckFinanceUserRights/${userID}/${formID}`)
        .subscribe(
          (d: any) => {
            var userFinancialRights = d[0];
            if (d[0].length > 0) {
              if (userFinancialRights[0].CanView) {
                this.ExportAppendix(schemeId, true);
              } else {
                this.error = "You do not have rights to download this appendix";
                this.http.ShowError(this.error.toString(), "Error");
                return false;
              }
            } else {
              this.error = "You do not have rights to download this appendix";
              this.http.ShowError(this.error.toString(), "Error");
              return false;
            }
          },
          (error) => (this.error = error), // error path
          () => {
            this.working = false;
          }
        );
    } else {
      this.ExportAppendix(schemeId, false);
    }
  }
  checkUserFinance(id) {
    var userID = this.formrights.UserID;
    var formID = 1147;
    this.http
      .getJSON(`api/Users/CheckFinanceUserRights/${userID}/${formID}`)
      .subscribe(
        (d: any) => {
          if (d.CanView) {
            this.isFinancialUser = true;
            this.ExportAppendix(id, d.CanView);
          } else {
            this.isFinancialUser = false;
            this.error = "You do not have rights to download this appendix";
            this.http.ShowError(this.error.toString(), "Error");
            return false;
          }
        },
        (error) => (this.error = error), // error path
        () => {
          this.working = false;
        }
      );
  }
  ExportToExcel(row) {
    this.http
      .postJSON("api/Approval/GetFactSheet", { SchemeID: row.SchemeID })
      .subscribe(
        (d: any) => {
          const data = d[0];
          // const ws: XLSX.WorkSheet = XLSX.utils.json_to_sheet(d);
          const wb = new Workbook();
          const ws = wb.addWorksheet("Fact Sheet");
          const font = {
            name: "Calibri",
            family: 4,
            size: 11,
            // underline: true,
            bold: true,
          };
          const Keys = ["", ""];
          ws.properties.defaultRowHeight = 18;
          Keys.concat(Object.keys(data));
          const Col4 = ws.getColumn(4);
          const Col5 = ws.getColumn(5);
          const Col3 = ws.getColumn(3);
          Col3.font = font;
          const RowH = ws.getRow(3);
          Col4.width = 40;
          Col5.width = 20;
          Col3.width = 28;
          Col3.alignment = {
            vertical: "middle",
            horizontal: "center",
            wrapText: true,
          };
          Col4.values = Object.keys(data);
          Col5.values = Object.values(data);
          ws.insertRow(1, "", ""); // Row 1
          ws.mergeCells("C2:E2");
          const title = ws.getCell("C2");
          title.alignment = { vertical: "middle", horizontal: "center" };
          title.value = data["SchemeName"]; // Row 2
          RowH.fill = {
            type: "pattern",
            pattern: "solid",
            fgColor: { argb: "FFFFFF00" },
            bgColor: { argb: "80C0C0C0" },
          };
          title.fill = {
            type: "pattern",
            pattern: "solid",
            fgColor: { argb: "FFFFFF00" },
            bgColor: { argb: "FF0000FF" },
          };
          ws.insertRow(3, ["", "", "Type", "Indicator", "Value"]); // Row 3
          ws.mergeCells("C4:C11");
          ws.getCell("C4").value = "Target Trainee";
          ws.mergeCells("C12:C16");
          ws.getCell("C13").value = "Training Provider (TP) Participation";
          ws.mergeCells("C17:C24");
          ws.getCell("C17").value = "CTM";
          ws.getCell("C25").value = "Course Duration";
          ws.getCell("C26").value = "Procurement Time ";
          ws.mergeCells("C27:C35");
          ws.getCell("C27").value = "Cost";
          ws.mergeCells("C36:C41");
          ws.getCell("C36").value = "Largest TPs";
          ws.getCell("C42").value = "Trade";
          ws.mergeCells("C43:C44");
          ws.getCell("C43").value = "Women ";
          ws.mergeCells("C45:C47");
          ws.getCell("C45").value = "Placement ";
          this.WriteAndDownloadFile(wb, "FactSheet");
        },
        (error) => (this.error = error), // error path
        () => {
          this.working = false;
        }
      );
  }
  openApprovalDialogue(row: any): void {
    let processKey = EnumApprovalProcess.AP_PD;
    if (
      row.PCategoryID ==
        EnumProgramCategory.BusinessDevelopmentAndPartnershipsFTI ||
      row.PCategoryID ==
        EnumProgramCategory.BusinessDevelopmentAndPartnershipsCommunity ||
      row.PCategoryID ==
        EnumProgramCategory.BusinessDevelopmentAndPartnershipsIndustry
    ) {
      processKey = EnumApprovalProcess.AP_BD;
    }
    this.dialogue
      .openApprovalDialogue(processKey, row.SchemeID)
      .subscribe((result) => {
        console.log(result);
        // location.reload();
      });
  }
  ExportAppendix(SchemeID: any, isAllowFinancial: boolean) {
    let scheme: any;
    let tsps: any;
    let classes: any;
    let instructors: any;
    this.http
      .postJSON("api/Appendix/GetAppendix", SchemeID)
      .subscribe((d: any) => {
        scheme = d[0];
        tsps = d[1];
        classes = d[2];
        instructors = d[3];
        // console.log(scheme)
        // console.log(tsps)
        // console.log(classes)
        // console.log(instructors)
        scheme = {
          "Scheme Code": scheme.SchemeCode,
          "Scheme Name": scheme.SchemeName,
          "Payment Schedule": scheme.PaymentSchedule,
          Description: scheme.Description,
          // , "Scheme Duraton": x.SchemeCode
          "Business Rules": scheme.BusinessRuleType,
          "Funding Source": scheme.FundingSourceName,
          "Funding Category": scheme.FundingCategoryName,
          "Program Category": scheme.PCategoryName,
          Stipend: scheme.Stipend,
          "Stipend Mode": scheme.StipendMode,
          "Uniform and Bag": scheme.UniformAndBag,
          "Minimum Education": scheme.MinimumEducationName,
          "Maximum Education": scheme.MaximumEducationName,
          "Minimum Age(Years)": scheme.MinAge,
          "Maximum Age": scheme.MaxAge,
          Gender: scheme.GenderName,
          "Program Type": scheme.PTypeName,
        };
        tsps = tsps.map((x) => {
          return {
            "TSP Name": x.TSPName,
            "TSP Code": x.TSPCode,
            "Organization	": x.OrganizationName,
            // ,"Type": x.name
            "TSP Color": x.TSPColor,
            Tier: x.TierID,
            // ,"Type": x.name
            PNTN: x.PNTN,
            GST: x.GST,
            "Address District": x.DistrictName,
            "Head of Organization": x.HeadName,
            "Designation of Head of Organization": x.HeadDesignation,
            "Email of Head of Organization": x.HeadEmail,
            FTN: x.FTN,
            NTN: x.NTN,
            "Mobile of Head of Organization": x.HeadLandline,
            "Landline Organization": x.HeadLandline,
            Website: x.Website,
            "Name of Contact Person": x.CPName,
            "Designation of Contact Person": x.CPDesignation,
            "Mobile / Landline of Contact Person": x.CPLandline,
            "Email of Contact Person": x.CPEmail,
            "Name of Contact Person Admissions": x.CPAdmissionsName,
            "Designation of Contact Person Admissions":
              x.CPAdmissionsDesignation,
            "Mobile / Landline of Contact Person Admissions":
              x.CPAdmissionsLandline,
            "Email of Contact Person Admissions": x.CPAdmissionsEmail,
            "Training District": x.Address,
            "Name of Contact Person Accounts": x.CPAccountsName,
            "Designation of Contact Person Accounts": x.CPAccountsDesignation,
            "Mobile / Landline of Contact Person Accounts":
              x.CPAccountsLandline,
            "Email of Contact Person Accounts": x.CPAccountsEmail,
            "Bank Name": x.BankName,
            "Bank Account / IBAN": x.BankAccountNumber,
            "Account Title": x.AccountTitle,
            "Bank Branch": x.BankBranch,
          };
        });
        classes = classes.map((x) => {
          var obj = {
            "TSP Name": x.TSPName,
            Sector: x.SectorName,
            "Trade Name": x.TradeName,
            "Class Code": x.ClassCode,
            "Duration in Months": x.Duration,
            "Source of Curriculum": x.SourceOfCurriculum,
            "Entry Qualification": x.EntryQualificationName,
            "Certification Authority": x.CertAuthName,
            "Registration Authority": x.RegistrationAuthorityName,
            "Program Focus": x.ProgramFocusName,
            // ,"Attendance Standard Percentage": x.
            // ,"Total Trainees": x.
            "Trainees per Class": x.TraineesPerClass,
            // , "Number of Batches": x.Batch
            // ,"Number of Classes": x.
            "Minimum Training Hours Per Month": x.MinHoursPerMonth,
            "Start Date": this.datePipe.transform(x.StartDate, "dd-MM-yyyy"),
            "End Date": this.datePipe.transform(x.EndDate, "dd-MM-yyyy"),
            "Trainees Gender": x.GenderName,
            "Address of Training Location": x.TrainingAddressLocation,
            "Geo Tagging":
              x.Latitude != "" ? `${x.Latitude},${x.Longitude}` : "",
            Province: x.ProvinceName,
            District: x.DistrictName,
            Tehsil: x.TehsilName,
            Cluster: x.ClusterName,
            "Total Trainee Bid Price": x.OfferedPrice,
            "Total Trainee BM Price": x.BMPrice,
            // ,"Total Trainee Cost	Sales Tax Rate": x.
            // ,"Training Cost per Trainee per Month(Exclusive of Taxes)": x.
            // ,"Sales Tax	Training Cost per Trainee per Month(Inclusive  of Taxes)": x.
            "Uniform & Bag Cost per Trainee": x.UniformBagCost,
            "Testing & Certification Fee per Trainee": x.PerTraineeTestCertCost,
            "Boarding & Other Allowances per trainee":
              x.BoardingAllowancePerTrainee,
            // ,"Employment Commitment Self	Employment Commitment Formal": x.
            // ,"Overall Employment Commitment": x.
            Stipend: x.Stipend,
            "Baloon Payment": x.balloonpayment,
            // ,"Total Cost": x.
            "Training Cost Per Trainee Per Month Ex Tax":
              x.TrainingCostPerTraineePerMonthExTax,
            "Training Cost Per Trainee Per Month In Tax":
              x.TrainingCostPerTraineePerMonthInTax,
            "Total Cost Per Class": x.TotalCostPerClass,
            "Total Cost Per Class In Tax": x.TotalCostPerClassInTax,
            "Total Per Trainee Cost In Tax": x.TotalPerTraineeCostInTax,
            // , "Total Testing Certification Of Class": x.TotalTestingCertificationOfClass
            SalesTax: x.SalesTax,
            "Sales Tax Rate": x.SalesTaxRate,
            "BM Price": x.BMPrice,
            "Bid Price": x.BidPrice,
            "Boarding Allowance Per Trainee": x.BoardingAllowancePerTrainee,
            "EmploymentCommitmentSelf": x.EmploymentCommitmentSelf,
            "EmploymentCommitmentFormal": x.EmploymentCommitmentFormal,
            "OverallEmploymentCommitment": x.OverallEmploymentCommitment,
          };
          if (!isAllowFinancial) {
            delete obj["Training Cost Per Trainee Per Month Ex Tax"];
            delete obj["Training Cost Per Trainee Per Month In Tax"];
            delete obj["Total Cost Per Class"];
            delete obj["Total Cost Per Class In Tax"];
            delete obj["Total Per Trainee Cost In Tax"];
            delete obj["SalesTax"];
            delete obj["Sales Tax Rate"];
            delete obj["BM Price"];
            delete obj["Bid Price"];
            delete obj["Boarding Allowance Per Trainee"];
          }
          return obj;
        });
        instructors = instructors.map((x) => {
          return {
            "Name of Organization": x.NameOfOrganization,
            "Instructor Name": x.InstructorName,
            Gender: x.GenderName,
            // , "Profile Picture": x.
            "Total Experience": x.TotalExperience,
            "Class Code": x.ClassCode,
            "Qualification Highest": x.QualificationHighest,
            "CNIC of Instructor": x.CNICofInstructor,
            // , "CNIC Issue Date": x.
            Trade: x.TradeName,
            "Address of Training Location": x.LocationAddress,
          };
        });
        const wb = new Workbook();
        const ws_scheme = wb.addWorksheet(AppendixImportSheetNames.Scheme);
        const ws_tsps = wb.addWorksheet(AppendixImportSheetNames.TSP);
        const ws_classes = wb.addWorksheet(AppendixImportSheetNames.Class);
        const ws_instructors = wb.addWorksheet(
          AppendixImportSheetNames.Instructor
        );
        ws_scheme.addRow(Object.keys(scheme));
        ws_scheme.addRow(Object.values(scheme));
        ws_tsps.addRow(Object.keys(tsps[0]));
        tsps.forEach((row) => {
          ws_tsps.addRow(Object.values(row));
        });
        ws_classes.addRow(Object.keys(classes[0]));
        classes.forEach((row) => {
          ws_classes.addRow(Object.values(row));
        });
        ws_instructors.addRow(Object.keys(instructors[0]));
        instructors.forEach((row) => {
          ws_instructors.addRow(Object.values(row));
        });
        this.WriteAndDownloadFile(wb, "Appendix");
      });
  }
  WriteAndDownloadFile(wb: Workbook, name: string) {
    wb.xlsx
      .writeBuffer()
      .then((data) => {
        const blob = new Blob([data], {
          type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
        });
        fs.saveAs(blob, name + ".xlsx");
      })
      .catch((error) => {
        console.error(error);
      });
  }
  // OK() { //this method is just for testing invoices generation, pls ignore this
  //    this.http.getJSON('api/Scheme/GenerateInvoice').subscribe((d: any) => {
  //        this.classes = d;
  //    });
  // }
}
