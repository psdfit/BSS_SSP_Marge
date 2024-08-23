import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort'; import { FormBuilder, FormGroup, FormGroupDirective, Validators } from '@angular/forms';
import * as XLSX from 'xlsx';
import { environment } from '../../../environments/environment';
import { CommonSrvService } from '../../common-srv.service';
import { UserRightsModel } from '../../master-data/users/users.component';
import { ModelBase } from '../../shared/ModelBase';
import { ClassModel, ClassComponent } from '../class/class.component';
import { InstructorModel, InstructorComponent } from '../Instructor/Instructor.component';
import { TSPModel, TSPComponent } from '../tsp/tsp.component';
import { MatStepper } from '@angular/material/stepper';
import { MatTableDataSource } from '@angular/material/table';
import { AppendixImportSheetNames, EnumProgramCategory, EnumApprovalProcess } from '../../shared/Enumerations';
import { Observable } from 'rxjs';
import { MatTabGroup } from '@angular/material/tabs';
import { IMaskPipe } from 'angular-imask';
import { Route, Router } from '@angular/router';
import { DatePipe } from '@angular/common';
import { Workbook } from 'exceljs';
import * as fs from 'file-saver';
import { isNull, isNullOrUndefined } from 'util';

@Component({
  selector: 'app-appendix',
  templateUrl: './appendix.component.html',
  styleUrls: ['./appendix.component.scss'],
  providers: [IMaskPipe, DatePipe]
})
export class AppendixComponent implements OnInit {
  searchFilter: string = '';
  searchFilterSSP: string = '';
  schemeForm: FormGroup;
  schemeFileForm: FormGroup;
  isLoadedAppendixForm: boolean = false;
  paymentSchedule: any;
  programs: any;
  programCategories: any;
  //programCategoriesFiltered: any;
  fundingSources: any;
  fundingCategories: any;
  //fundingCategoriesFiltered: any;
  genders: any;
  organizations: any;
  educationTypes: any;
  age: any[] = [];
  minAge = 18;
  maxAge = 45;
  formrights: UserRightsModel;
  enText: string = "Scheme";
  error: String;
  sequence: any;
  insertedScheme: SchemeModel[] = [];
  tspsInserted: Array<TSPModel> = [];
  classesInserted: Array<ClassModel> = [];
  instructorsInserted: Array<InstructorModel> = [];
  //newScheme: boolean = true;
  tspDataExcel: any;
  classDataExcel: any;
  instructorDataExcel: any;
  displayedColumns = ['Scheme Name', 'Scheme Code', 'Description', 'Organization', 'PaymentSchedule', 'Action'];
  displayedColumns1 = ['Scheme Name', 'Scheme Code', 'Description', 'PaymentSchedule', 'ProcessStartDate','ProcessEndDate', 'Action'];
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(TSPComponent) childTSP: TSPComponent;
  @ViewChild(ClassComponent) childClass: ClassComponent;
  @ViewChild(InstructorComponent) childInstructor: InstructorComponent;
  @ViewChild("tabGroup") tabGroup: MatTabGroup;
  @ViewChild("stepper") stepper: MatStepper;
  schemes: SchemeModel[];
  businessRuleType = [
    { Name: "FTI" }
    , { Name: "Community" }
    , { Name: "Industry" }
    , { Name: "Cost Sharing" }
  ]
  schemesDataSource: MatTableDataSource<SchemeModel>;
  schemesDataSourceSSP: MatTableDataSource<SchemeModel>;
  constructor(private _formBuilder: FormBuilder, private http: CommonSrvService, private router: Router, private datePipe: DatePipe) {
    http.setTitle("Appendix");
    this.schemesDataSource = new MatTableDataSource([]);
    this.schemesDataSourceSSP = new MatTableDataSource([]);
    this.formrights = http.getFormRights();
  }
  currentUser:any;
  ngOnInit() {
    this.currentUser = this.http.getUserDetails();

    this.http.OID.subscribe(OID => {
      this.loadAppendix()

      //this.childTSP.ngOnInit();
    })
  }
  loadAppendix() {
    this.searchFilter = ''
    this.searchFilterSSP = ''
    this.isLoadedAppendixForm = false;
    this.insertedScheme = [];
    this.tspsInserted = [];
    this.classesInserted = [];
    this.instructorsInserted = [];

    this.initForm();
    this.getData();
    this.getDataSSPScheme();
    this.schemeForm.controls.ContractAwardDate.patchValue(new Date());
  }
  reLoadAppendix() {
    this.loadAppendix();
    this.childTSP.ngOnInit();
    this.childClass.ngOnInit();
    this.childInstructor.ngOnInit();
  }
  initForm() {
    this.schemeForm = this._formBuilder.group({
      SchemeID: [0],
      SchemeName: ['', { validators: [Validators.required, Validators.maxLength(30)], updateOn: "blur" }],
      SchemeCode: ['', { validators: [Validators.required, Validators.maxLength(4)], updateOn: "blur" }],
      Description: '',
      PaymentSchedule: ['', Validators.required],
      ProgramTypeID: ['', Validators.required],
      PCategoryID: ['', Validators.required],
      FundingSourceID: ['', Validators.required],
      FundingCategoryID: ['', Validators.required],
      BusinessRuleType: ['', Validators.required],
      Stipend: ['', Validators.required],
      StipendMode: ['', Validators.required],
      UniformAndBag: ['', Validators.required],
      MinimumEducation: ['', Validators.required],
      MaximumEducation: ['', Validators.required],
      MinAge: ['', Validators.required],
      MaxAge: ['', Validators.required],
      GenderID: ['', Validators.required],
      OrganizationID: ['', Validators.required],
      //DualEnrollment: ['', Validators.required],
      ContractAwardDate: ['', Validators.required]
    }
      //, { updateOn: "blur" }
    );

    //let schemeIDControl = this.schemeForm.get('SchemeID');
    let schemeNameControl = this.schemeForm.get('SchemeName');
    let schemeCodeControl = this.schemeForm.get('SchemeCode');
    let programTypeIDControl = this.schemeForm.get('ProgramTypeID');
    let maximumEducationControl = this.schemeForm.get('MaximumEducation');
    let stipendControl = this.schemeForm.get('Stipend');
    let uniformAndBagControl = this.schemeForm.get('UniformAndBag');
    debugger;
    schemeNameControl.valueChanges.subscribe(
      value => {
        if (value) {
          if (!this.insertedScheme[0]?.FinalSubmitted) {
            this.getAllSchemes().subscribe(schemes => {
              let duplicate =  schemes.find(x => (x.SchemeName.trim().toLowerCase() == value.trim().toLowerCase()) && (x.FinalSubmitted == true));
              if (duplicate) {
                schemeNameControl.setErrors({ exists: true });
                schemeNameControl.markAsDirty();
              }
            })
          }
        }
      }
    );
    schemeCodeControl.valueChanges.subscribe(
      value => {
        if (value) {
          if (!this.insertedScheme[0]?.FinalSubmitted) {
            this.getAllSchemes().subscribe(schemes => {
              let duplicate =  schemes.find(x => (x.SchemeCode.trim().toLowerCase() == value.trim().toLowerCase()) && (x.FinalSubmitted == true));
              if (duplicate) {
                schemeCodeControl.setErrors({ exists: true });
                schemeCodeControl.markAsDirty();
              }
            });
          }
        }
      }
    );
    programTypeIDControl.valueChanges.subscribe(
      value => {
        if (value && value > 0) {
          this.childClass.onChangeProgramType(value)
        }
      }
    );
    //maximumEducationControl.valueChanges.subscribe(
    //  value => {
    //    if (value && value > 0) {
    //      this.childClass.onChangeEducation(value);
    //    }
    //  }
    //);
    stipendControl.valueChanges.subscribe(value => {
      this.childClass.onChangeStipend(value);
    })
    uniformAndBagControl.valueChanges.subscribe(value => {
      this.childClass.onChangeUniformBag(value);
    })
    this.schemeFileForm = this._formBuilder.group({
      SchemeExcel: ['', Validators.required]
    });
  }
  fillForm() {
    this.schemeForm.patchValue(this.insertedScheme[0]);
    this.schemeForm.markAllAsTouched();
    this.isLoadedAppendixForm = true;
  }

  onFileChange(ev: any) {
    
    let workBook = null;
    let jsonData = null;
    const reader = new FileReader();
    const file = ev.target.files[0];

    //if (file.type == "") {
    //  this.http.ShowError('Please upload excel file');

    //  return;
    //}

    reader.onload = (event) => {
      const data = reader.result;
      workBook = XLSX.read(data, { type: 'binary' });
      //workBook.Sheets[Object.keys(workBook.Sheets).find(x => x.toLowerCase() == AppendixImportSheetNames.Scheme.toLowerCase())]
      jsonData = workBook.SheetNames.reduce((initial, name) => {
        const sheet = workBook.Sheets[name];
        initial[name] = XLSX.utils.sheet_to_json(sheet);
        return initial;
      }, {});
      const dataString = JSON.parse(JSON.stringify(jsonData));
      ///FIND FILES WITH IGNORE CASE SENSITIVITY
      const schemeData = dataString[Object.keys(workBook.Sheets).find(x => x.toLowerCase() == AppendixImportSheetNames.Scheme.toLowerCase())];
      this.tspDataExcel = dataString[Object.keys(workBook.Sheets).find(x => x.toLowerCase() == AppendixImportSheetNames.TSP.toLowerCase())];
      this.classDataExcel = dataString[Object.keys(workBook.Sheets).find(x => x.toLowerCase() == AppendixImportSheetNames.Class.toLowerCase())];
      this.instructorDataExcel = dataString[Object.keys(workBook.Sheets).find(x => x.toLowerCase() == AppendixImportSheetNames.Instructor.toLowerCase())];

      try {
        if (!schemeData || schemeData.length == 0) {
          this.http.ShowError("Sheet with the name '" + AppendixImportSheetNames.Scheme + "' not found in Excel file");
        } else {
          let schemeCode = this.schemeForm.value.SchemeCode.trim();
          if (schemeCode != '' && schemeCode != schemeData[0]['Scheme Code'].trim()) {
            this.http.ShowError("Imported scheme is not same as already loaded scheme.");
          } else {
            this.populateFieldsFromFile(schemeData[0]);
          }
        }
        //
        if (!this.tspDataExcel || this.tspDataExcel.length == 0) {
          this.http.ShowError("Sheet with the name '" + AppendixImportSheetNames.TSP + "' not found in Excel file.");
        } else {
          //this.childTSP.getTSPSequence(this.tspDataExcel.length).subscribe(seq => {
          if (this.childTSP.populateFieldsFromFile(this.tspDataExcel)) {
            if (!this.classDataExcel || this.classDataExcel.length == 0) {
              this.http.ShowError("Sheet with the name '" + AppendixImportSheetNames.Class + "' not found in Excel file.");
            } else {
              this.childClass.populateFieldsFromFile(this.classDataExcel, this.stepper);
            }
            if (!this.instructorDataExcel || this.instructorDataExcel.length == 0) {
              this.http.ShowError("Sheet with the name '" + AppendixImportSheetNames.Instructor + "' not found in Excel file.");
            } else {
              this.childInstructor.populateFieldsFromFile(this.instructorDataExcel);
            }
          } else {
            return false;
          }
          // })
        }

      } catch (e) {
        console.error(e)
      }
    }
    reader.readAsBinaryString(file);
    ev.target.value = '';
  }
  populateFieldsFromFile(_schemData: any) {
    if (!_schemData) {
      return;
    }
    _schemData = this.http.TrimFields(_schemData);
    //let dates = _schemData['Contract Award Date'].split("/");
    let program = this.programs.find(x => _schemData['Program Type'].toLowerCase() == x.PTypeName.toLowerCase())?.PTypeID || ''
    //if (program != '') {
    //  this.programCategories = this.programCategories.filter(a => a.PTypeID === program);
    //}
    let fundingSrc = this.fundingSources.find(x => _schemData['Funding Source'].toLowerCase() == x.FundingSourceName.toLowerCase())?.FundingSourceID || ''
    if (fundingSrc != '') {
      this.fundingCategories = this.fundingCategories.filter(a => a.FundingSourceID === fundingSrc);
    }
    this.schemeForm.patchValue({
      SchemeName: _schemData['Scheme Name'],
      SchemeCode: _schemData['Scheme Code'],
      Description: _schemData['Description'],
      PaymentSchedule: this.paymentSchedule.find(x => _schemData['Payment Schedule'].toLowerCase() == x.PaymentStructure.toLowerCase())?.PaymentStructure || '',
      OrganizationID: this.organizations .find(x => _schemData['Organization'].toLowerCase() == x.OName.toLowerCase())?.OID || '',
      ProgramTypeID: program,
      PCategoryID: this.programCategories.find(x => _schemData['Program Category'].toLowerCase() == x.PCategoryName.toLowerCase())?.PCategoryID || '',
      FundingSourceID: fundingSrc,
      FundingCategoryID: this.fundingCategories.find(x => _schemData['Funding Category'].toLowerCase() == x.FundingCategoryName.toLowerCase())?.FundingCategoryID || '',
      BusinessRuleType: this.businessRuleType.find(x => _schemData['Business Rules'].toLowerCase() == x.Name.toLowerCase())?.Name || '',
      //BusinessRuleType: _schemData['Business Rules'],
      Stipend: _schemData['Stipend'],
      StipendMode: _schemData['Stipend Mode'],
      UniformAndBag: _schemData['Uniform and Bag'],
      MinimumEducation: this.educationTypes.find(x => _schemData['Minimum Education'].toLowerCase() == x.Education.toLowerCase())?.EducationTypeID || '',
      MaximumEducation: this.educationTypes.find(x => _schemData['Maximum Education'].toLowerCase() == x.Education.toLowerCase())?.EducationTypeID || '',
      MinAge: parseInt(_schemData['Minimum Age(Years)']),
      MaxAge: parseInt(_schemData['Maximum Age']),
      GenderID: this.genders.find(x => _schemData['Gender'].toLowerCase() == x.GenderName.toLowerCase())?.GenderID || '',
      //DualEnrollment: _schemData['Dual Enrollment'].toLowerCase() == 'yes' ? true : false,
      //ContractAwardDate: new Date(parseInt(dates[2]), parseInt(dates[1]) - 1, parseInt(dates[0]) + 1),
      ContractAwardDate: new Date()
    });
    this.schemeForm.markAllAsTouched();
    this.isLoadedAppendixForm = true;
  }
  getData() {
    this.http.getJSON(`api/Scheme/GetScheme?OID=${this.http.OID.value}`).subscribe((d: any) => {
      this.schemes = d[0];
      this.programs = d[1];
      this.programCategories = d[2];
      //this.programCategoriesFiltered = d[2];
      this.fundingSources = d[3];
      this.fundingCategories = d[4];
      //this.fundingCategoriesFiltered = d[4];
      this.genders = d[5];
      this.educationTypes = d[6];
      this.organizations = d[7];
      this.paymentSchedule = d[8];

      this.schemesDataSource = new MatTableDataSource(this.schemes);
      this.schemesDataSource.paginator = this.paginator;
      this.schemesDataSource.sort = this.sort
    },
      (error) => {
        this.error = error.error;
        this.http.ShowError(error.error + '\n' + error.message);
      } // error path
    );

    let k = this.minAge;
    while (k <= this.maxAge) {
      this.age.push(k);
      k++;
    }
  }

 
  submit(nform: FormGroupDirective, stepper: MatStepper) {
    if (!this.schemeForm.valid)
      return;
    let titleConfirm = 'Confirmation';
    let messageConfirm = "Save only Scheme's data as a draft, You should save other components(TSPs, Classes & Instructors) individually.";
    this.http.confirm(titleConfirm, messageConfirm).subscribe(
      (isConfirm: Boolean) => {
        if (isConfirm == true) {
          let tempSchemeID = this.schemeForm.controls.SchemeID.value;
          this.http.postJSON('api/Scheme/Save', JSON.stringify(this.schemeForm.value)).subscribe(
            (d: any) => {
              this.insertedScheme = d ?? [];
              this.reset(nform);
              //if (isNullOrUndefined(this.schemeForm.value.SchemeID)) {
              //  this.schemesDataSource.filteredData.unshift(d[0]);
              //}
              this.schemeForm.patchValue(this.insertedScheme[0]);
              this.childClass.onSaveSchemeData_OnClassGrid(this.insertedScheme);

              //if (this.schemeForm.value.SchemeID) {
              //  var currentSchemeIndex = this.schemesDataSource.filteredData.indexOf(
              //    this.schemesDataSource.filteredData.find(x => {
              //      x.SchemeID = this.schemeForm.value.SchemeID;
              //    })
              //  );
              //  this.schemesDataSource.filteredData[currentSchemeIndex] = d[0];
              //}

              ////this.childClass.onChangeSchemeCode(this.insertedScheme[0].SchemeCode);

              this.loadSchemes().subscribe((d: any) => {
                this.schemesDataSource = new MatTableDataSource(d);
                this.schemesDataSource.paginator = this.paginator;
                this.schemesDataSource.sort = this.sort;
              });
              this.applyFilter();
            },
            (error) => {
              this.error = error.error;
              this.http.ShowError(error.error + '\n' + error.message);
            } // error path
            , () => {
              this.http.openSnackBar(tempSchemeID > 0 ? environment.UpdateMSG.replace("${Name}", this.enText) : environment.SaveMSG.replace("${Name}", this.enText));
              stepper.next();
            });
        }
      })
  }
  relaodAppendixForm() {
    let titleConfirm = "Do you want to relaod this appendix's form ?";
    let messageConfirm = "Changes that you made, may not be saved.";
    this.http.confirm(titleConfirm, messageConfirm).subscribe(
      (isConfirm: Boolean) => {
        if (isConfirm) {
          this.reloadPage();
        }
      })
  }
  reloadPage() {
    //this.router.navigateByUrl('/appendix/appendix', { skipLocationChange: true }).then(() => {
    //  this.router.navigate([AppendixComponent]);
    //});
    location.reload();
  }
  finalSubmit() {
    let titleConfirm = "Confirmation";
    let messageConfirm = "Do you want to Submit Appendix's Form ? After submission this form is not available for further changes.";
    this.http.confirm(titleConfirm, messageConfirm).subscribe(
      (isConfirm: Boolean) => {
        if (isConfirm) {
          let processKey = EnumApprovalProcess.AP_PD;
          if (this.schemeForm.controls.PCategoryID.value == EnumProgramCategory.BusinessDevelopmentAndPartnershipsFTI
            || this.schemeForm.controls.PCategoryID.value == EnumProgramCategory.BusinessDevelopmentAndPartnershipsIndustry
            || this.schemeForm.controls.PCategoryID.value == EnumProgramCategory.BusinessDevelopmentAndPartnershipsCommunity) {
            processKey = EnumApprovalProcess.AP_BD;
          }
          this.http.getJSON(`api/Scheme/FinalSubmit?SchemeID=${this.schemeForm.controls.SchemeID.value}&ProcessKey=${processKey}`)
            .subscribe((d: any) => {
              this.reloadPage();
            },
              (error) => {
                this.error = error.error;
                this.http.ShowError(error.error + '\n' + error.message);
              } // error path
            );
        }
      })
  }
  reset(nform: FormGroupDirective) {
    nform.resetForm();
    this.schemeForm.reset();
  }
  bindTSPs(event) {
    this.tspsInserted = event;
  }
  bindClasses(event) {
    this.classesInserted = event;
  }
  moveForward(event, stepper: MatStepper) {
    if (event)
      stepper.next();
  }
  loadSchemes(): Observable<any> {
    return this.http.getJSON(`api/Scheme/GetSchemesForAppendix?OID=${this.http.OID.value}`);
  }
  getAllSchemes(): Observable<any> {
    return this.http.getJSON(`api/Scheme/GetSubmittedSchemes`);
  }
  deleteDraftAppendix(schemeID: number) {
    let titleConfirm = "Confirmation";
    let messageConfirm = "Do you want to forever delete this draft Appendix?";
    this.http.confirm(titleConfirm, messageConfirm).subscribe(
      (isConfirm: Boolean) => {
        if (isConfirm) {
          this.http.getJSON(`api/Scheme/DeleteDraftAppendix?SchemeID=${schemeID}`).subscribe(
            (d: any) => {
              if (d) {
                this.reloadPage();
              }
            },
            (error) => {
              this.error = error.error;
              this.http.ShowError(error.error + '\n' + error.message, '', 5000);
            } // error path
          );
        }
      }
    );
  }
  
  editAppendix(schemeID: number) {
    this.insertedScheme = [];
    this.tspsInserted = [];
    this.classesInserted = [];
    this.instructorsInserted = [];
    this.tabGroup.selectedIndex = 0
    this.getAppendix(schemeID);
  }
  getAppendix(schemeID) { // SchemeID = 0 brings most recent incomplete scheme
    this.http.getJSON("api/Scheme/GetAppendix/" + schemeID).subscribe(
      (d: any) => {
        this.insertedScheme = d[0];
        this.tspsInserted = d[1];
        this.classesInserted = d[2];
        this.instructorsInserted = d[3];

        if (this.insertedScheme.length > 0) {
          this.fillForm();
          this.childTSP.fillForm(this.tspsInserted);
          this.childClass.fillForm(this.classesInserted);
          this.childInstructor.fillForm(this.instructorsInserted);
        }
      }),
      (error) => {
        this.error = error.error;
        this.http.ShowError(error.error + '\n' + error.message);
      } // error path
  }
  applyFilter() {
    if (this.searchFilter != '') {
      this.schemesDataSource.filter = this.searchFilter.trim().toLowerCase();
    }
  }

  ///Developed by Rao Ali Haider on 22-05-2024
  getDataSSPScheme() {
    this.http.getJSON(`api/Scheme/SSPFetchScheme`).subscribe((d: any) => {
      this.schemes = d[0];
      this.schemesDataSourceSSP = new MatTableDataSource(this.schemes);
      this.schemesDataSourceSSP.paginator = this.paginator;
      this.schemesDataSourceSSP.sort = this.sort
    },
      (error) => {
        this.error = error.error;
        this.http.ShowError(error.error + '\n' + error.message);
      } // error path
    );
  }
  applyFilterSSP() {
    if (this.searchFilterSSP != '') {
      this.schemesDataSourceSSP.filter = this.searchFilterSSP.trim().toLowerCase();
    }
  }
  GenerateAppendix(schemeID: number) {
    let titleConfirm = "Confirmation";
    let messageConfirm = "Do you want to generate this Appendix?";
    this.http.confirm(titleConfirm, messageConfirm).subscribe(
      (isConfirm: Boolean) => {
        if (isConfirm) {
          this.http.getJSON(`api/Scheme/GenerateAutoAppendix?SchemeID=${schemeID}`).subscribe(
            (d: any) => {
              if (d) {
                this.reloadPage();
              }
            },
            (error) => {
              this.error = error.error;
              this.http.ShowError(error.error + '\n' + error.message, '', 5000);
            } // error path
          );
        }
      }
    );
  }
  //-----------------------END Changes -------------------------
  get SchemeID() { return this.schemeForm.get("SchemeID"); }
  get FundingSourceID() { return this.schemeForm.get("FundingSourceID"); }

  exportAppendix(schemeID: any) {
    let scheme: any;
    let tsps: any;
    let classes: any;
    let instructors: any;

    this.http.postJSON('api/Appendix/GetAppendix', schemeID).subscribe((d: any) => {
      scheme = d[0];
      tsps = d[1];
      classes = d[2];
      instructors = d[3];

      //console.log(scheme, tsps, classes, instructors);
      scheme = {
        "Scheme Code": scheme.SchemeCode
        , "Scheme Name": scheme.SchemeName
        , "Payment Schedule": scheme.PaymentSchedule
        , "Description": scheme.Description
        //, "Scheme Duraton": x.SchemeCode
        , "Business Rules": scheme.BusinessRuleType
        , "Funding Source": scheme.FundingSourceName
        , "Funding Category": scheme.FundingCategoryName
        , "Program Category": scheme.PCategoryName
        , "Stipend": scheme.Stipend
        , "Stipend Mode": scheme.StipendMode
        , "Uniform and Bag": scheme.UniformAndBag
        , "Minimum Education": scheme.MinimumEducationName
        , "Maximum Education": scheme.MaximumEducationName
        , "Minimum Age(Years)": scheme.MinAge
        , "Maximum Age": scheme.MaxAge
        , "Gender": scheme.GenderName
        , "Program Type": scheme.PTypeName
      };
      tsps = tsps.map(x => {
        return {
          "TSP Name": x.TSPName
          , "TSP Code": x.TSPCode
          , "Organization": x.OrganizationName
          //,"Type": x.name
          , "TSP Color": x.TSPColor
          , "Tier": x.TierID
          //,"Type": x.name
          , "PNTN": x.PNTN
          , "GST": x.GST
          , "Address District": x.DistrictName
          , "Head of Organization": x.HeadName
          , "Designation of Head of Organization": x.HeadDesignation
          , "Email of Head of Organization": x.HeadEmail
          , "FTN": x.FTN
          , "NTN": x.NTN
          , "Mobile of Head of Organization": x.HeadLandline
          , "Landline Organization": x.HeadLandline
          , "Website	Name of Contact Person": x.Website
          , "Designation of Contact Person": x.CPDesignation
          , "Mobile / Landline of Contact Person": x.CPLandline
          , "Email of Contact Person": x.CPEmail
          , "Name of Contact Person Admissions": x.CPAdmissionsName
          , "Designation of Contact Person Admissions": x.CPAdmissionsDesignation
          , "Mobile / Landline of Contact Person Admissions": x.CPAdmissionsLandline
          , "Email of Contact Person Admissions": x.CPAdmissionsEmail
          , "Training District": x.Address
          , "Name of Contact Person Accounts": x.CPAccountsName
          , "Designation of Contact Person Accounts": x.CPAccountsDesignation
          , "Mobile / Landline of Contact Person Accounts": x.CPAccountsLandline
          , "Email of Contact Person Accounts": x.CPAccountsEmail
          , "Bank Name": x.BankName
          , "Bank Account / IBAN": x.BankAccountNumber
          , "Account Title": x.AccountTitle
          , "Bank Branch": x.BankBranch
        }
      });
      classes = classes.map(x => {
        return {
          "TSP Name": x.TSPName
          , "Sector": x.SectorName
          , "Trade Name": x.TradeName
          , "Class Code": x.ClassCode
          , "Duration in Months": x.Duration
          , "Source of Curriculum": x.SourceOfCurriculum
          , "Entry Qualification": x.EntryQualificationName
          , "Certification Authority": x.CertAuthName
          //,"Attendance Standard Percentage": x.
          //,"Total Trainees": x.
          , "Trainees per Class": x.TraineesPerClass
          //, "Number of Batches": x.Batch
          //,"Number of Classes": x.
          , "Minimum Training Hours Per Month": x.MinHoursPerMonth
          , "Start Date": this.datePipe.transform(x.StartDate, "dd-MM-yyyy")
          , "End Date": this.datePipe.transform(x.EndDate, "dd-MM-yyyy")
          , "Trainees Gender": x.GenderName
          , "Address of Training Location": x.TrainingAddressLocation
          , "Geo Tagging": x.Latitude != '' ? `${x.Latitude},${x.Longitude}` : ''
          , "Province": x.ProvinceName
          , "District": x.DistrictName
          , "Tehsil": x.TehsilName
          , "Cluster": x.ClusterName
          , "Total Trainee Bid Price": x.OfferedPrice
          , "Total Trainee BM Price": x.BMPrice
          //,"Total Trainee Cost	Sales Tax Rate": x.
          //,"Training Cost per Trainee per Month(Exclusive of Taxes)": x.
          //,"Sales Tax	Training Cost per Trainee per Month(Inclusive  of Taxes)": x.
          , "Uniform & Bag Cost per Trainee": x.UniformBagCost
          , "Testing & Certification Fee per Trainee": x.PerTraineeTestCertCost
          , "Boarding & Other Allowances per trainee": x.BoardingAllowancePerTrainee
          //,"Employment Commitment Self	Employment Commitment Formal": x.
          //,"Overall Employment Commitment": x.
          //,"Total Cost": x.
          , "Stipend": x.Stipend
          , "Training Cost Per Trainee Per Month Ex Tax": x.TrainingCostPerTraineePerMonthExTax
          , "Training Cost Per Trainee Per Month In Tax": x.TrainingCostPerTraineePerMonthInTax
          , "Total Cost Per Class": x.TotalCostPerClass
          , "Total Cost Per Class In Tax": x.TotalCostPerClassInTax
          , "Total Per Trainee Cost In Tax": x.TotalPerTraineeCostInTax
          //, "Total Testing Certification Of Class": x.TotalTestingCertificationOfClass
          , "SalesTax": x.SalesTax
          , "Sales Tax Rate": x.SalesTaxRate
          , "BM Price": x.BMPrice
          , "Bid Price": x.BidPrice
          , "Boarding Allowance Per Trainee": x.BoardingAllowancePerTrainee
        }
      });
      instructors = instructors.map(x => {
        return {
          "Name of Organization": x.NameOfOrganization
          , "Instructor Name": x.InstructorName
          , "Gender": x.GenderName
          //, "Profile Picture": x.
          , "Total Experience": x.TotalExperience
          , "Class Code": x.ClassCode
          , "Qualification Highest": x.QualificationHighest
          , "CNIC of Instructor": x.CNICofInstructor
          //, "CNIC Issue Date": x.
          , "Trade": x.TradeName
          , "Address of Training Location": x.LocationAddress

        }
      });

      let wb = new Workbook();

      const ws_scheme = wb.addWorksheet(AppendixImportSheetNames.Scheme);
      const ws_tsps = wb.addWorksheet(AppendixImportSheetNames.TSP);
      const ws_classes = wb.addWorksheet(AppendixImportSheetNames.Class);
      const ws_instructors = wb.addWorksheet(AppendixImportSheetNames.Instructor);

      ws_scheme.addRow(Object.keys(scheme));
      ws_scheme.addRow(Object.values(scheme));

      ws_tsps.addRow(Object.keys(tsps[0]));
      tsps.forEach(row => {
        ws_tsps.addRow(Object.values(row));
      });

      ws_classes.addRow(Object.keys(classes[0]));
      classes.forEach(row => {
        ws_classes.addRow(Object.values(row));
      });

      ws_instructors.addRow(Object.keys(instructors[0]));
      instructors.forEach(row => {
        ws_instructors.addRow(Object.values(row));
      });

      this.writeAndDownloadFile(wb, "Appendix");
    });
  }
  writeAndDownloadFile(wb: Workbook, name: string) {
    wb.xlsx.writeBuffer().then((data) => {
      let blob = new Blob([data], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
      fs.saveAs(blob, name + ".xlsx");

    }).catch(error => {
      console.error(error);
    });
  }
}
export class SchemeModel extends ModelBase {
  SchemeID: number;
  SchemeName: string;
  SchemeCode: string;
  Description: string;
  PaymentSchedule: number;
  ProgramTypeID: number;
  PCategoryID: number;
  FundingSourceID: number;
  FundingCategoryID: number;
  BusinessRuleType: string;
  Stipend: number;
  StipendMode: string;
  UniformAndBag: number;
  MinimumEducation: number;
  MaximumEducation: number;
  MinAge: number;
  MaxAge: number;
  Gender: number;
  OrganizationID: number;
  OName: string;
  // DualEnrollment: boolean;
  ContractAwardDate: Date;
  FinalSubmitted: boolean;
  ProgramName: string;
  ProcessStartDate: Date;
  ProcessEndDate: Date;
  IsLocked: boolean;
}
