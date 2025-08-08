import { filter } from "rxjs/operators";
import { AfterViewInit, Component, OnInit, QueryList, ViewChild, ViewChildren } from "@angular/core";
import { CommonSrvService } from "../../common-srv.service";
import { ActivatedRoute } from "@angular/router";
import { MatTableDataSource } from "@angular/material/table";
import { MatPaginator } from "@angular/material/paginator";
import { MatSort } from "@angular/material/sort";
import { MatTabChangeEvent, MatTabGroup } from "@angular/material/tabs";
import {
  FormArray,
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from "@angular/forms";
import { MatOption } from "@angular/material/core";
import { MatSelect, MatSelectChange } from "@angular/material/select";
import { environment } from "../../../environments/environment";
import {MatAccordion, MatExpansionPanel} from '@angular/material/expansion';
import { numberFormat } from "highcharts";
import { ProgramReviewComponent } from "src/app/custom-components/program-review/program-review.component";
import { MatDialog } from "@angular/material/dialog";

@Component({
  selector: "app-program-plan",
  templateUrl: "./program-plan.component.html",
  styleUrls: ["./program-plan.component.scss"],
})
export class ProgramPlanComponent implements OnInit, AfterViewInit {
  @ViewChild(MatAccordion) accordion: MatAccordion;
  @ViewChildren(MatExpansionPanel) panels!: QueryList<MatExpansionPanel>;
   step:number=0
  currentIndex: number=0
  programBudget: any=[]
  programHeadBudget: any=[]

  setStep(index: number) {
    this.step = index;
  }

  nextStep() {
    this.step++;
  }

  prevStep() {
    this.step--;
  }

    openAll() {
      // debugger;
      
    this.accordion.openAll();
  }

  closeAll() {
    this.accordion.closeAll();
    this.step = -1;
  }

  closeFirstPanel() {
    this.panels.toArray()[0]?.close();
    if (this.step === 0) {
      this.step = -1; // Reset index if first panel was active
    }
  }
  matSelectArray: MatSelect[] = [];
  @ViewChild("Applicability") Applicability: MatSelect;
  SelectedAll_Applicability: string;
  @ViewChild("Province") Province: MatSelect;
  SelectedAll_Province: string;
  @ViewChild("Cluster") Cluster: MatSelect;
  SelectedAll_Cluster: string;
  @ViewChild("District") District: MatSelect;
  SelectedAll_District: string;
  ProgramCategory: any[];
  FundingCategory: any;
  BusinessRuleType: any = [];
  EditCheck: boolean = false;
  selectedProgramData: any;
  ProvinceLength: number = 0;
  ClusterLength: number = 0;
  DistrictLength: number = 0;
  SelectAll(event: any, dropDownNo, controlName, formGroup) {
    const matSelect = this.matSelectArray[dropDownNo - 1];
    if (event.checked) {
      matSelect.options.forEach((item: MatOption) => item.select());
      if (this[formGroup].get(controlName).value) {
        const uniqueArray = Array.from(
          new Set(this[formGroup].get(controlName).value)
        );
        this[formGroup].get(controlName).setValue(uniqueArray);
      }
    } else {
      matSelect.options.forEach((item: MatOption) => item.deselect());
    }
  }
  optionClick(event, controlName) {
    this.EmptyCtrl();
    let newStatus = true;
    event.source.options.forEach((item: MatOption) => {
      if (!item.selected && !item.disabled) {
        newStatus = false;
      }
    });
    if (event.source.ngControl.name === controlName) {
      this["SelectedAll_" + controlName] = newStatus;
    } else {
      this["SelectedAll_" + controlName] = newStatus;
    }
  }
  constructor(
    private ComSrv: CommonSrvService,
    private AcitveRoute: ActivatedRoute,
    private fb: FormBuilder,
    private http: CommonSrvService,
    private Dialog: MatDialog
  ) {}
  TablesData: MatTableDataSource<any>;
  @ViewChild("tabGroup") tabGroup: MatTabGroup;
  @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: false }) sort: MatSort;
  CountZeroToHun = [];
  TapIndex: any;
  TORAndSOWEvidence: any = "";
  CriteriaEvidence: any = "";
  readOnly = false;
  PreadOnly = true;
  CreadOnly = true;
  DreadOnly = true;
  required = false;
  error: any;
  displayedColumns: string[] = [];
  SelectionMethods: any[];
  TraineeSupportItems: any[];
  PlaningType: any[];
  GetDataObject: any = {};
  SpacerTitle: string;
  SearchCtr = new FormControl("");
  PSearchCtr = new FormControl("");
  CSearchCtr = new FormControl("");
  DSearchCtr = new FormControl("");
  TSearchCtr = new FormControl("");
  BSearchCtr = new FormControl("");
  TapTTitle: string = "Profile";
  Data: any = [];
  Gender: any = [];
  GenderData: any = [];
  ProvinceData: any = [];
  FinancialYearData: any = [];
  ProgramTypeData: any = [];
  FundingSourceData: any = [];
  EducationData: any = [];
  ApplicabilityData: any = [];
  PaymentStructureData: any = [];
  TraineeSupportItemsData: any = [];
  ClusterData: any = [];
  DistrictData: any = [];
  TehsilData: any = [];
  TableColumns = [];
  maxDate: Date;
  SaleGender: string = "Sales Tax Evidence";
  ngOnInit(): void {
    for (let index = 0; index <= 100; index = index + 5) {
      this.CountZeroToHun.push({ CountId: index, CountValue: index + "%" });
    }
    this.TapIndex = 0;
    this.TablesData = new MatTableDataSource([]);
    this.PageTitle();
    this.GetData();
    this.InitAnnualPlanInfo();
  }
  AnnualPlanInfoForm: FormGroup;
  InitAnnualPlanInfo() {
    this.AnnualPlanInfoForm = this.fb.group({
      ProgramID: [""],
      FinancialYearID: ["", Validators.required],
      PlaningTypeID: [""],
      Program: ["", Validators.required],
      ProgramBudget: ["", Validators.required],
      ProgramCode: ["", Validators.required],
      Description: ["", Validators.required],
      ProgramTypeID: ["", Validators.required],
      ProgramCategoryID: [2, Validators.required],
      FundingSourceID: ["", Validators.required],
      FundingCategoryID: ["", Validators.required],
      BusinessRuleType: ["", Validators.required],
      GenderID: ["", Validators.required],
      Stipend: ["0", Validators.required],
      StipendMode: ["Digital", Validators.required],
      MinEducationID: ["", Validators.required],
      MaxEducationID: ["", Validators.required],
      MinAge: ["18", Validators.required],
      MaxAge: ["35", Validators.required],
      ApplicabilityID: [[1,2], Validators.required],
      TentativeProcessSDate: [new Date(), Validators.required],
      ClassStartDate: [new Date(), Validators.required],
      PaymentStructureID: ["", Validators.required],
      SelectionMethodID: [""],
      ApprovalRecDetail: [" "],
      ApprovalAttachment: [" "],
      SchemeDesignOn: ["", Validators.required],
      ProvinceID: [{ value: "", disabled: true }],
      ClusterID: [{ value: "", disabled: true }],
      DistrictID: [{ value: "", disabled: true }],
      EmploymentCommitment: ["", Validators.required],
      AttachmentTORs: ["", Validators.required],
      AttachmentCriteria: ["", Validators.required],
      TraineeSupportCost: ["0", Validators.required],
      IsSubmitted: [false],
      SAPProgram: ["", Validators.required],
    });

    const provinceCtrl = this.AnnualPlanInfoForm.get("ProvinceID");
    const clusterCtrl = this.AnnualPlanInfoForm.get("ClusterID");
    const districtCtrl = this.AnnualPlanInfoForm.get("DistrictID");

    const updateLength = (control, lengthProperty) => {
      const value = control.value;
      if (Array.isArray(value)) {
        this[lengthProperty] = value.length;
      }
    };

    updateLength(provinceCtrl, "ProvinceLength");
    updateLength(clusterCtrl, "ClusterLength");
    updateLength(districtCtrl, "DistrictLength");

    this.checkDuplicate(
      "Program",
      "Program name is already in use. Please choose another name.",
      "Program"
    );
    this.checkDuplicate(
      "ProgramCode",
      "Program Code is already in use. Please choose another code.",
      "ProgramCode"
    );


    this.AnnualPlanInfoForm.get('SAPProgram').valueChanges.subscribe(d => {
      debugger;
     const selectedProgram= this.programBudget.find(p=>p.U_Program==d)
     const selectedProgramHead= this.programHeadBudget.find(p=>p.U_Program==d)
     const selectedYear= this.FinancialYearData.find(p=>p.Year==selectedProgram.U_Year)

  

this.AnnualPlanInfoForm.get('ProgramBudget')?.setValue(selectedProgram.U_Amount.replace(/,/g, '').trim());
this.AnnualPlanInfoForm.get('FinancialYearID')?.setValue(selectedYear.Id);
this.AnnualPlanInfoForm.get('Program')?.setValue(selectedProgram.U_Program);

    })
    // this.AnnualPlanInfoForm.get('PlaningTypeID').valueChanges.subscribe(d => {
    //   if (d === 1) {
    //     this.ProgramTypeData = this.GetDataObject.GetProgramType.filter(item => item.PTypeID === 10);
    //     this.BusinessRuleType = this.GetDataObject.BusinessRuleType.filter(item => item.ID === 1);
    //     this.SelectionMethods = this.GetDataObject.SelectionMethods.filter(item => item.ID === 3);
    //     this.FundingSourceData = this.GetDataObject.FundingSource.filter(item => item.FundingSourceID !== 8 && item.FundingSourceID !== 9);
    //     this.AnnualPlanInfoForm.patchValue({
    //       ProgramTypeID: 10,
    //       BusinessRuleType: 1,
    //       SelectionMethodID: 3,
    //     });
    //   } else {
    //     this.ProgramTypeData = this.GetDataObject.GetProgramType.filter(item => item.PTypeID !== 10 && item.PTypeID !== 7);
    //     this.BusinessRuleType = this.GetDataObject.BusinessRuleType.filter(item => item.PlaningTypeID !== 3);
    //     this.SelectionMethods = this.GetDataObject.SelectionMethods.filter(item => item.ID !== 3);
    //     this.FundingSourceData = this.GetDataObject.FundingSource;

    //     this.AnnualPlanInfoForm.patchValue({
    //       ProgramTypeID: '',
    //       BusinessRuleType: '',
    //       SelectionMethodID: '',
    //       FundingSourceID: '',
    //     });
    //   }
    // });

    this.AnnualPlanInfoForm.get("FundingSourceID").valueChanges.subscribe(
      (fId) => {
        if (fId) {
          this.AnnualPlanInfoForm.get("FundingCategoryID").setValue("");
          this.FundingCategory = this.GetDataObject.FundingCategory.filter(
            (item) => item.FundingSourceID === fId
          );
        }
      }
    );
  }

  checkDuplicate(controlName, errorMessage, field) {
    this.AnnualPlanInfoForm.get(controlName).valueChanges.subscribe((value) => {
      if (value) {
        const formattedValue = value.trim().replace(/\s+/g, " ").toLowerCase();
        const selectedProgramID = this.selectedProgramData?.ProgramID;
        const data = this.GetDataObject.ProgramDesign.filter(
          (d) =>
            d.ProgramID !== selectedProgramID &&
            d[field].toLowerCase() === formattedValue
        );

        if (data.length > 0) {
          this.ComSrv.ShowError(errorMessage);
          this.AnnualPlanInfoForm.get(controlName).setErrors({
            customError: errorMessage,
          });
        } else {
          this.AnnualPlanInfoForm.get(controlName).setErrors(null);
        }
      }
    });
  }

  GetData() {
    this.ComSrv.getJSON("api/ProgramDesign/GetProgramDesign").subscribe(
      (response) => {
        this.GetDataObject = response;
        this.programBudget = this.GetDataObject.programBudget;
        this.programHeadBudget = this.GetDataObject.programHeadBudget;
        this.FinancialYearData = this.GetDataObject.GetFinancialYear;

        this.ProgramTypeData = this.GetDataObject.GetProgramType;
        this.FundingSourceData = this.GetDataObject.FundingSource;
        this.GenderData = this.GetDataObject.Gender.filter(
          (g) => g.GenderID != 8
        );
        this.EducationData = this.GetDataObject.EducationTypes;
        this.PaymentStructureData = this.GetDataObject.PaymentStructure;
        this.TraineeSupportItems = this.GetDataObject.TraineeSupportItems;
        this.SelectionMethods = this.GetDataObject.SelectionMethods;
        this.PlaningType = this.GetDataObject.PlaningType.filter(
          (d) => d.PlaningTypeID != 3
        );
        this.ProgramCategory = this.GetDataObject.ProgramCategory.filter(
          (d) => d.PCategoryID == 2
        );
        this.FundingCategory = this.GetDataObject.FundingCategory;
        this.BusinessRuleType = this.GetDataObject.BusinessRuleType;

        if (this.GetDataObject.ProgramDesign.length > 0) {
          this.LoadMatTable(this.GetDataObject.ProgramDesign);
        }
      },
      (error) => {
        this.ComSrv.ShowError(`${error.message}`, "Close", 500000);
      }
    );
  }
  ChangeSchemeDesignOn(value: string) {
    const provinceControl = this.AnnualPlanInfoForm.get("ProvinceID");
    const clusterControl = this.AnnualPlanInfoForm.get("ClusterID");
    const districtControl = this.AnnualPlanInfoForm.get("DistrictID");
    provinceControl.clearValidators();
    clusterControl.clearValidators();
    districtControl.clearValidators();
    provinceControl.disable();
    clusterControl.disable();
    districtControl.disable();
    this.PreadOnly = true;
    this.CreadOnly = true;
    this.DreadOnly = true;
    if (value === "Province") {
      this.AnnualPlanInfoForm.get("ClusterID").setValue("");
      this.AnnualPlanInfoForm.get("DistrictID").setValue("");
      this.ClusterData = [];
      this.DistrictData = [];
      this.GetProvince();
      provinceControl.setValidators(Validators.required);
      provinceControl.enable();
      this.PreadOnly = false;
      this.CreadOnly = false;
      this.DreadOnly = false;
    } else if (value === "Cluster") {
      this.AnnualPlanInfoForm.get("DistrictID").setValue("");
      this.DistrictData = [];
      this.GetProvince();
      clusterControl.setValidators(Validators.required);
      provinceControl.enable();
      clusterControl.enable();
      provinceControl.setValue("");
      this.CreadOnly = false;
      this.DreadOnly = false;
    } else if (value === "District") {
      this.GetProvince();
      districtControl.setValidators(Validators.required);
      provinceControl.enable();
      clusterControl.enable();
      districtControl.enable();
      provinceControl.setValue("");
      clusterControl.setValue("");
      this.DreadOnly = false;
    }
    provinceControl.updateValueAndValidity();
    clusterControl.updateValueAndValidity();
    districtControl.updateValueAndValidity();
  }
  GetProvince() {
    // this.ProvinceData = this.GetDataObject.Province.filter(p => this.GetDataObject.Province);
    this.ProvinceData = this.GetDataObject.Province.filter(
      (p) => p.ProvinceID != 4
    );
  }
  GetCluster(ProvinceId) {
    if (ProvinceId != "" && ProvinceId != null) {
      this.ClusterData = [];
      const cluster = this.GetDataObject.Cluster.filter((c) =>
        ProvinceId.includes(c.ProvinceID)
      );
      this.ClusterData = cluster;
    }
  }
  GetDistrict(ClusterId) {
    if (ClusterId != "" && ClusterId != null) {
      this.DistrictData = [];
      const district = this.GetDataObject.District.filter((d) =>
        ClusterId.includes(d.ClusterID)
      );
      this.DistrictData = district;
    }
  }
  IsSubmitted: boolean = false;
  IsFinalSubmit() {
    if (this.AnnualPlanInfoForm.valid) {
      this.IsSubmitted = true;
      this.AnnualPlanInfoForm.get("IsSubmitted").setValue(this.IsSubmitted);
      this.SaveAnnualPlanInfo();
    }
  }
  IsDisabled = false;
  SaveAnnualPlanInfo() {
    debugger;
    console.log(this.AnnualPlanInfoForm);

    if (
      this.AnnualPlanInfoForm.get("BusinessRuleType").value == "FTI" &&
      this.AnnualPlanInfoForm.get("ProgramTypeID").value == 10
    ) {
      this.AnnualPlanInfoForm.get("PlaningTypeID").setValue(1);
      this.AnnualPlanInfoForm.get("SelectionMethodID").setValue(3);
    } else {
      this.AnnualPlanInfoForm.get("PlaningTypeID").setValue(2);
      this.AnnualPlanInfoForm.get("SelectionMethodID").setValue(2);
    }

    debugger;
 this.AnnualPlanInfoForm.get("ProgramCategoryID").setValue(2);
    if (this.AnnualPlanInfoForm.valid) {
      this.IsDisabled = true;
      this.http
        .postJSON(
          "api/ProgramDesign/SaveProgramDesign",
          this.AnnualPlanInfoForm.getRawValue()
        )
        .subscribe(
          (response: any[]) => {
            this.AnnualPlanInfoForm.reset();
            if (response.length > 0) {
              this.LoadMatTable(response);
            }
            this.IsDisabled = false;

           this.ComSrv.openSnackBar("Scheme Plan saved successfully!", "Close", 3000);

          },
          (error) => {
            this.error = error.error;
            this.http.ShowError(error.error, "error", 5000);
          }
        );
    } else {
        this.ComSrv.ShowError("Please fill all required fields.");
    }
  }
  onTabChange(event: MatTabChangeEvent) {
  this.TapIndex = event.index;
}
  FinalSubmit: boolean = false;
  UpdateRecord(row: any) {

    this.readOnly = true;
    this.CriteriaEvidence = row.AttachmentCriteriaEvidence;
    this.TORAndSOWEvidence = row.AttachmentTORsEvidence;
    this.selectedProgramData = row;
    this.EditCheck = true;
    this.tabGroup.selectedIndex = 0;
    this.AnnualPlanInfoForm.patchValue({
      ...row,
      ApplicabilityID: row.ApplicabilityIDs.split(",").map(Number),
      ProvinceID: row.ProvinceIDs.split(",").map(Number),
      ClusterID: row.ClusterIDs.split(",").map(Number),
      DistrictID: row.DistrictIDs.split(",").map(Number),
    });
    setTimeout(() => {
      this.AnnualPlanInfoForm.get("PlaningTypeID").setValue(
        Number(row.PlaningTypeID)
      );
      this.AnnualPlanInfoForm.get("BusinessRuleType").setValue(
        row.BusinessRuleType
      );
      this.AnnualPlanInfoForm.get("FundingSourceID").setValue(
        Number(row.FundingSourceID)
      );
      this.AnnualPlanInfoForm.get("FundingCategoryID").setValue(
        Number(row.FundingCategoryID)
      );
      this.AnnualPlanInfoForm.get("ProgramTypeID").setValue(
        Number(row.ProgramTypeID)
      );
      this.AnnualPlanInfoForm.get("SelectionMethodID").setValue(
        Number(row.SelectionMethodIDs)
      );
      this.AnnualPlanInfoForm.get("ProgramCategoryID").setValue(2);
    }, 0);
    this.FinalSubmit = row.IsSubmitted;
    if (this.FinalSubmit) {
      this.AnnualPlanInfoForm.disable();
    } else {
      this.AnnualPlanInfoForm.enable();
    }
    this.accordion.closeAll();
setTimeout(() => {
  this.accordion.openAll();
}, 0);

  }
  ResetForm() {
    this.AnnualPlanInfoForm.get("Program").setValue(null);
    this.AnnualPlanInfoForm.get("ProgramCode").setValue(null);
    this.IsDisabled = false;
    this.readOnly = false;
    this.EditCheck = false;
    this.FinalSubmit = false;
    this.selectedProgramData = undefined;
    this.AnnualPlanInfoForm.enable();
    this.AnnualPlanInfoForm.reset();
  }
  panelOpenState = false;
  LoadMatTable(tableData: any[]) {
    const excludeColumnArray = [
      "AttachmentTORs",
      "IsSubmitted",
      "AttachmentCriteria",
      "ApprovalAttachment",
      "ApprovalEvidence",
      "ApprovalRecDetail","ClassStartDate","TentativeProcessSDate","TraineeSupportCost","Stipend","StipendMode","ProposedDistrict",'ProposedCluster','ProposedProvince','Description'
    ];
    if (tableData.length > 0) {
      this.TableColumns = Object.keys(tableData[0]).filter(
        (key) => !key.includes("ID") && !excludeColumnArray.includes(key)
      );
      this.TableColumns.unshift("Actions")
      this.TablesData = new MatTableDataSource(tableData);
      this.TablesData.paginator = this.paginator;
      this.TablesData.sort = this.sort;
    }
  }
  EmptyCtrl() {
    this.PSearchCtr.setValue("");
    this.CSearchCtr.setValue("");
    this.DSearchCtr.setValue("");
    this.BSearchCtr.setValue("");
  }

   openProgramReviewDialogue(row: any = {}): void {
    // Prepare data for ProgramPreviewComponent dialog
   

    const data = {
      programData: row
    };
    const dialogRef = this.Dialog.open(ProgramReviewComponent, {
      width: "90%",
      data: data,
      disableClose: true,
    });
    dialogRef.afterClosed().subscribe((result) => {
      // if (result === true) {
      //   this.GetData()
      // }
    });
  }
  ShowPreview(fileName: string) {
    this.ComSrv.PreviewDocument(fileName);
  }
  camelCaseToWords(input: string): string {
    return input.replace(/([a-z])([A-Z])/g, "$1 $2");
  }
  applyFilter(event: any) {
    this.TablesData.filter = event.target.value.trim().toLowerCase();
    if (this.TablesData.paginator) {
      this.TablesData.paginator.firstPage();
    }
  }
  DataExcelExport() {
    this.ComSrv.ExcelExporWithForm(
      this.TablesData.filteredData,
      this.SpacerTitle
    );
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
  PageTitle(): void {
    this.ComSrv.setTitle(this.AcitveRoute.snapshot.data.title);
    this.SpacerTitle = this.AcitveRoute.snapshot.data.title;
  }
  ngAfterViewInit() {
    this.matSelectArray = [
      this.Applicability,
      this.Province,
      this.Cluster,
      this.District,
    ];
    if (this.tabGroup) {
      this.tabGroup.selectedTabChange.subscribe((event) => {
        this.TapIndex = event.index;
      });
    }
  }
}
