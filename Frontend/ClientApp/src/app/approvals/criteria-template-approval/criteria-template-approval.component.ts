import { filter } from 'rxjs/operators';
import { Component, OnInit, ViewChild } from "@angular/core";
import { CommonSrvService } from "../../common-srv.service";
import { ActivatedRoute } from "@angular/router";
import { MatTableDataSource } from "@angular/material/table";
import { MatPaginator } from "@angular/material/paginator";
import { MatSort } from "@angular/material/sort";
import { MatTabGroup } from "@angular/material/tabs";
import { FormArray, FormBuilder, FormControl, FormGroup, Validators, } from "@angular/forms";
import { environment } from "../../../environments/environment";
import { EnumApprovalProcess } from "src/app/shared/Enumerations";
import { DialogueService } from "src/app/shared/dialogue.service";
@Component({
  selector: 'app-criteria-template-approval',
  templateUrl: './criteria-template-approval.component.html',
  styleUrls: ['./criteria-template-approval.component.scss']
})
export class CriteriaTemplateApprovalComponent implements OnInit {
  environment = environment;
  readonly = false
  fileArray: any = []
  TapIndex: any;
  error: any;
  GetDataObject: any = {};
  currentUser: any;
  savebtn: string = "Save ";
  TrainerProfile: any;
  TrainerTableColumns: any;
  trainerEditRecord = [];
  MainCateTotalMarks: number;
  SubCateTotalMarks: any;
  marksFlag: boolean = false
  mainCategoryMarks: any;
  mainCategoryMark: number;
  subCategoryMark: any;
  CategoryFlag: boolean = false
  Isread: boolean = false
  criteriaHeader: any=[]
  criteriaMainCategory: any;
  criteriaSubCategory: any;
  AttachedProgramCount: any=0
  isChecked: boolean = false
  constructor(
    private ComSrv: CommonSrvService,
    private AcitveRoute: ActivatedRoute,
    private fb: FormBuilder,
    private dialogue:DialogueService
  ) { }
  SpacerTitle: string;
  SearchCtr = new FormControl("");
  TableColumns = [];
  maxDate: Date;
  SaleGender: string = "Sales Tax Evidence";
  TablesData: MatTableDataSource<any>;
  TrainerTablesData: MatTableDataSource<any>;
  @ViewChild("tabGroup") tabGroup: MatTabGroup;
  @ViewChild("Paginator") Paginator: MatPaginator;
  @ViewChild("Sort") Sort: MatSort;
  ngOnInit(): void {
    this.currentUser = this.ComSrv.getUserDetails();
    this.TapIndex = 0;
    this.TrainerTablesData = new MatTableDataSource([]);
    this.TrainerTablesData = new MatTableDataSource([]);
    this.PageTitle();
    this.GetData();
    this.initCriteriaTemplateForm();
  }
  PageTitle(): void {
    this.ComSrv.setTitle(this.AcitveRoute.snapshot.data.title);
    this.SpacerTitle = this.AcitveRoute.snapshot.data.title;
  }
  IsSubmitted:boolean=false
  Edit(CriteriaTemplateID, EditFlag) {
    debugger;
    this.isChecked = true
    const row = this.TablesData.filteredData.filter(d => d.CriteriaTemplateID == CriteriaTemplateID)[0]
    this.IsSubmitted=row.IsSubmitted
    this.AttachedProgramCount=row.AttachedProgramCount
    this.readonly = true
    this.fileArray = []
    this.savebtn = 'Update'
    this.initCriteriaTemplateForm()
    this.removeMainCategory(0)
    this.tabGroup.selectedIndex = 1
    const mainCategory = this.GetDataObject.criteriaMainCategory.filter(d => d.CriteriaTemplateID == row.CriteriaTemplateID)
    const mainCategoryArray = []
    const formData = { ...row, mainCategory: mainCategoryArray }
    let i = 0
    mainCategory.forEach(item => {
      this.addMainCategory();
      mainCategoryArray.push({ ...item })
      const subCategory = this.GetDataObject.criteriaSubCategory.filter(d => d.CriteriaMainCategoryID == item.CriteriaMainCategoryID)
      const subCategoryArray = []
      const subCategoryFile = []
      subCategory.forEach(itemChild => {
        subCategoryFile.push(itemChild.CriteriaAttachment)
        subCategoryArray.push(itemChild)
      });
      this.fileArray.push(subCategoryFile)
      mainCategoryArray[i]['subCategory'] = subCategoryArray
      i++
    });
    this.CriteriaTemplateForm.get('mainCategory')['controls'].forEach(mainCategory => {
      const subCategoryA = mainCategory.get('subCategory') as FormArray;
      subCategoryA.push(this.createSubCategory());
    });
    this.CriteriaTemplateForm.patchValue(formData)
    this.CriteriaTemplateForm.get('mainCategory')['controls'].forEach(mainCategory => {
      const subCategoryA = mainCategory.get('subCategory') as FormArray;
      subCategoryA.controls.forEach((element, i) => {
        if (element.value.CriteriaSubCategoryID == 0) {
          this.removeSubCategory(mainCategory, i)
        }
      });
    });
  }
  CriteriaTemplateForm: FormGroup;
  initCriteriaTemplateForm() {
    this.CriteriaTemplateForm = this.fb.group({
      UserID: [0],
      IsSubmitted: [0],
      CriteriaTemplateID: [0],
      CriteriaTemplateTitle: ["", [Validators.required]],
      MarkingRequired: ["Yes", [Validators.required]],
      MaximumMarks: [0, [Validators.required]],
      Description: ["", [Validators.required]],
      mainCategory: this.fb.array([]),
    });
    this.MarkingRequiredListener();
    this.addMainCategory();
  }
  MarkingRequiredListener() {
    this.CriteriaTemplateForm.get("MarkingRequired").valueChanges.subscribe((value) => {
      const control = this.CriteriaTemplateForm.get("MaximumMarks");
      if (value === "Yes") {
        this.Isread = false;
      } else {
        this.Isread = true;
      }
      this.updateMarks()
    });
  }
  async MainCategoryListener() {
    debugger;
    if (this.CriteriaTemplateForm.get("MarkingRequired").value == 'No') {
      this.CategoryFlag = true
    } else {
      // this.CriteriaTemplateForm.get("mainCategory").valueChanges.subscribe((mainCategories) => {
      const mainCategories = this.CriteriaTemplateForm.get("mainCategory").value
      this.mainCategoryMarks = mainCategories.map((category) => Number(category.TotalMarks)).reduce((a, b) => a + b, 0);
      for (let i = 0; i < mainCategories.length; i++) {
        this.mainCategoryMark = Number(mainCategories[i].TotalMarks);
        this.subCategoryMark = mainCategories[i].subCategory.map((subCategory) => Number(subCategory.MaxMarks)).reduce((a, b) => a + b, 0);
        if (this.mainCategoryMark === this.subCategoryMark && this.mainCategoryMark != 0) {
          this.ComSrv.openSnackBar(`Main Category ${i + 1} is equal to Sub Category`)
          this.CategoryFlag = true
        } else {
          this.ComSrv.ShowError(`Main Category ${i + 1} is  not equal to Sub Category`)
          this.CategoryFlag = false
        }
      }
      // });
    }
  }
  ResetTemplate() {
    this.IsSubmitted=false;
    this.savebtn = 'Save'
    this.initCriteriaTemplateForm()
  }
  addMainCategory() {
    const mainCategory = this.CriteriaTemplateForm.get('mainCategory') as FormArray;
    mainCategory.push(this.createMainCategory());
    // this.fileArray.push([])
  }
  createMainCategory() {
    return this.fb.group({
      UserID: [0],
      CriteriaTemplateID: [0],
      CriteriaMainCategoryID: [0],
      CategoryTitle: ["", [Validators.required]],
      Description: ["", [Validators.required]],
      TotalMarks: [0, [Validators.required]],
      subCategory: this.fb.array([this.createSubCategory()]),
    });
  }
  addSubCategory(mainCategory: FormGroup) {
    const subCategory = mainCategory.get('subCategory') as FormArray;
    subCategory.push(this.createSubCategory());
  }
  removedMainCategoryIds = []
  removedSubCategoryIds = []
  removeMainCategory(index: number) {
    const mainCategory = this.CriteriaTemplateForm.get('mainCategory') as FormArray;
    if (mainCategory.at(index).value.CriteriaMainCategoryID > 0) {
      this.removedMainCategoryIds.push(mainCategory.at(index).value)
    }
    mainCategory.removeAt(index);
  }
  RemovedMainCategory(value) {
    this.ComSrv.postJSON("api/CriteriaTemplate/removeMainCategory", value).subscribe(
      (response) => {
        this.GetData()
      },
      (error) => {
        this.ComSrv.ShowError(`${error.message}`, "Close", 500000);
      }
    );
  }
  RemovedSubCategory(value) {
    this.ComSrv.postJSON("api/CriteriaTemplate/removeSubCategory", value).subscribe(
      (response) => {
        this.GetData()
      },
      (error) => {
        this.ComSrv.ShowError(`${error.message}`, "Close", 500000);
      }
    );
  }
  removeSubCategory(mainCategory: FormGroup, index: number) {
    const subCategory = mainCategory.get('subCategory') as FormArray;
    if (subCategory.at(index).value.CriteriaSubCategoryID > 0) {
      this.removedSubCategoryIds.push(subCategory.at(index).value)
    }
    subCategory.removeAt(index);
  }
  createSubCategory() {
    return this.fb.group({
      UserID: [0],
      CriteriaMainCategoryID: [0],
      CriteriaSubCategoryID: [0],
      SubCategoryTitle: ["", [Validators.required]],
      Description: ["", [Validators.required]],
      Criteria: ["", [Validators.required]],
      MarkedCriteria: ["", [Validators.required]],
      Mandatory: ["", [Validators.required]],
      MaxMarks: [0, [Validators.required]],
      Attachment: [""],
    });
  }
  updateMarks() {
    if (this.Isread == true) {
      this.CriteriaTemplateForm.get('MaximumMarks').setValue(0);
      const mainCategoryControls = this.CriteriaTemplateForm.get('mainCategory')['controls'];
      mainCategoryControls.forEach((mainCategoryControl) => {
        mainCategoryControl.get('TotalMarks').setValue(0);
        const subCategoryControls = mainCategoryControl.get('subCategory')['controls'];
        subCategoryControls.forEach((subCategoryControl) => {
          subCategoryControl.get('MaxMarks').setValue(0);
        });
      });
      return;
    }
  }
  Submit(){
    debugger;
    if (this.CriteriaTemplateForm.valid)
      {
        if (confirm('Data cannot be modified after submission. Are you sure you want to proceed?') == true) {
          this.CriteriaTemplateForm.get('IsSubmitted').setValue(1)
        this.CreateTemplate()
      } 
    }
  }
  async CreateTemplate() {
    debugger
    await this.MainCategoryListener();
    if (this.CriteriaTemplateForm.valid) {
      if (this.CriteriaTemplateForm.get("MarkingRequired").value != 'No') {
        if (!this.CriteriaTemplateForm.touched && this.CriteriaTemplateForm.get('IsSubmitted').value !=1) {
          this.ComSrv.ShowError("At least one change in the main category section is required.")
          return;
        }
        if (this.mainCategoryMarks != this.CriteriaTemplateForm.get("MaximumMarks").value) {
          this.ComSrv.ShowError("Main category total marks is not equal to template marks")
          return
        }
        if (this.CategoryFlag !== true) {
          this.ComSrv.ShowError(`Main Category is not equal to Sub Category`);
          return;
        }
      }
      this.ComSrv.postJSON("api/CriteriaTemplate/Save", this.CriteriaTemplateForm.value).subscribe(
        (response) => {
          this.IsSubmitted=false;
          for (let m = 0; m < this.removedMainCategoryIds.length; m++) {
            this.RemovedMainCategory(this.removedMainCategoryIds[m])
          }
          for (let s = 0; s < this.removedSubCategoryIds.length; s++) {
            this.RemovedSubCategory(this.removedSubCategoryIds[s])
          }
          this.GetData()
          this.initCriteriaTemplateForm()
        },
        (error) => {
          this.ComSrv.ShowError(`${error.message}`, "Close", 500000);
        }
      );
    } else {
      this.ComSrv.ShowError(`All * filed is mandatory.`);
    }
  }
  get trainerDetails() {
    return this.CriteriaTemplateForm.get("trainerDetails") as FormArray;
  }
  GetData() {
    this.FetchRecord();
  }
  EmptyCtrl(ev: any) {
    this.SearchCtr.setValue("");
  }
  ResetFrom() {
    this.savebtn = "Save";
  }
  camelCaseToWords(input: string): string {
    return input.replace(/([a-z])([A-Z])/g, "$1 $2");
  }
  getErrorMessage(errorKey: string, errorValue: any): string {
    const error =
      errorValue.requiredLength == 15
        ? errorValue.requiredLength - 2
        : errorValue.requiredLength - 1;
    const errorMessages = {
      required: "This field is required.",
      minlength: `This field must be at least ${error} characters long.`,
      maxlength: `This field's text exceeds the specified maximum length.  (maxLength: ${errorValue.requiredLength} characters)`,
      email: "Invalid email address.",
      pattern: "This field is only required text",
      customError: errorValue,
    };
    return errorMessages[errorKey];
  }
  getSelectedTabData() {
    switch (this.tabGroup.selectedIndex) {
      case 0:
        this.SpacerTitle = "Criteria Template";
        break;
      default:
    }
  }
  ShowPreview(fileName: string) {
    this.ComSrv.PreviewDocument(fileName);
  }
  FetchRecord() {
    this.ComSrv.postJSON("api/CriteriaTemplate/FetchCriteriaTemplate", {
      UserID: this.currentUser.UserID,
    }).subscribe(
      (response) => {
        this.GetDataObject = response

        this.criteriaHeader = this.GetDataObject.criteriaHeader.filter(d=>d.IsSubmitted==true && d.IsRejected!=true)
        this.criteriaMainCategory = this.GetDataObject.criteriaMainCategory
        this.criteriaSubCategory = this.GetDataObject.criteriaSubCategory
        this.LoadMatTable(this.criteriaHeader, "TemplateHeader");
      },
      (error) => {
        this.ComSrv.ShowError(`${error.message}`, "Close", 500000);
      }
    );
  }
  LoadMatTable(tableData: any[], tableName: string) {
    if(tableData.length>0){

      const excludeColumnArray=["AttachedProgramCount","IsSubmitted"]
        this.TableColumns = Object.keys(tableData[0]).filter(key => !key.includes('ID') && ! excludeColumnArray.includes(key));
      this.TableColumns.unshift("Action")
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
  DataExcelExport(data: any, title) {
    this.ComSrv.ExcelExporWithForm(data, title);
  }
  openApprovalDialogue(CriteriaHeaderID: any): void {
    let processKey = EnumApprovalProcess.CRTEM_APP;
    this.dialogue.openApprovalDialogue(processKey,CriteriaHeaderID).subscribe(result => {
      console.log(result);
      this.FetchRecord();
    });
  }
  ngAfterViewInit() {
    if (this.tabGroup) {
      this.tabGroup.selectedTabChange.subscribe((event) => {
        this.TapIndex = event.index
        if (this.TapIndex == 0) {
          this.isChecked = false
        }
        if (this.TapIndex == 1) {
          this.isChecked = true
        }
      });
    }
  }
}
