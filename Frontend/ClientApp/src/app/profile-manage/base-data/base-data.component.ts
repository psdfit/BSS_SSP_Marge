import { Component, OnInit, ViewChild, ChangeDetectorRef } from "@angular/core";
import { CommonSrvService } from "../../common-srv.service";
import { ActivatedRoute, Router } from "@angular/router";
import { MatTableDataSource } from "@angular/material/table";
import { MatPaginator } from "@angular/material/paginator";
import { MatSort } from "@angular/material/sort";
import { MatTabGroup } from "@angular/material/tabs";
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from "@angular/forms";
import { MatDialog } from "@angular/material/dialog";
import { takeUntil } from "rxjs/operators";
import { Subject } from "rxjs";
import { environment } from '../../../environments/environment';
import { GeoTaggingComponent } from "src/app/custom-components/geo-tagging/geo-tagging.component";
@Component({
  selector: 'app-base-data',
  templateUrl: './base-data.component.html',
  styleUrls: ['./base-data.component.scss']
})
export class BaseDataComponent implements OnInit {
  environment = environment;
  TapIndex: any;
  isLinear = false;
  HtmlCodeBasicTempl: any;
  TSCodeBasicTempl: any;
  error: any;
  GetDataObject: any = {}
  currentUser: any;
  bank: any;
  BankTableColumns: string[];
  BankDetail: any;
  savebtn: string = "Add "
  registrationAuthority: any;
  TrainingLocation: any = []
  Certification: any;
  TradeMapping: any;
  TrainerProfile: any;
  TrainingTableColumns: string[];
  CertificateTableColumns: string[];
  TradeMapTableColumns: string[];
  TrainerTableColumns: any;
  RegistrationStatus: any;
  Trade: any;
  EducationTypes: any;
  trainerEditRecord = []
  EditCheck: boolean = false
  TrainerEditCheck: boolean = false
  TspTrades: any[] = []
  TspMappedTrades = []
  PendingForm: any[]=[]
  fieldSetDisabled: boolean;

  constructor(
    private ComSrv: CommonSrvService,
    private ActivateRoute: ActivatedRoute,
    private fb: FormBuilder,
    private Cdr: ChangeDetectorRef,
    private router: Router,
    private Dialog: MatDialog,
  ) { }
  SpacerTitle: string;
  SearchCtr = new FormControl('');
  PSearchCtr = new FormControl('');
  CSearchCtr = new FormControl('');
  DSearchCtr = new FormControl('');
  TSearchCtr = new FormControl('');
  TTSearchCtr = new FormControl('');
  BSearchCtr = new FormControl('');
  TapTTitle: string = "Profile"
  Data: any = []
  Gender: any = []
  ProvinceData: any = []
  ClusterData: any = []
  DistrictData: any = []
  TehsilData: any = []
  TableColumns = [];
  maxDate: Date;
  SaleGender: string = "Sales Tax Evidence"
  BankTablesData: MatTableDataSource<any>;
  @ViewChild("tabGroup") tabGroup: MatTabGroup;
  @ViewChild("BankPaginator") BankPaginator: MatPaginator;
  @ViewChild("BankSort") BankSort: MatSort;
  TrainingTablesData: MatTableDataSource<any>;
  @ViewChild("TrainingPaginator") TrainingPaginator: MatPaginator;
  @ViewChild("TrainingSort") TrainingSort: MatSort;
  TrainerTablesData: MatTableDataSource<any>;
  CertificateTablesData: MatTableDataSource<any>;
  @ViewChild("CertificatePaginator") CertificatePaginator: MatPaginator;
  @ViewChild("CertificateSort") CertificateSort: MatSort;
  TradeTablesData: MatTableDataSource<any>;
  @ViewChild("TradePaginator") TradePaginator: MatPaginator;
  @ViewChild("TradeSort") TradeSort: MatSort;
  @ViewChild("TrainerPaginator") TrainerPaginator: MatPaginator;
  @ViewChild("TrainerSort") TrainerSort: MatSort;
  private ngUnsubscribe = new Subject();
  ngOnInit(): void {
    this.currentUser = this.ComSrv.getUserDetails();
    this.TapIndex = 0
    this.BankTablesData = new MatTableDataSource([]);
    this.TrainerTablesData = new MatTableDataSource([]);
    this.CertificateTablesData = new MatTableDataSource([]);
    this.TradeTablesData = new MatTableDataSource([]);
    this.TrainerTablesData = new MatTableDataSource([]);
    this.PageTitle();
    this.GetData();
    this.InitBankInfoForm();
    this.InitTrainingForm();
    this.InitTrainerForm();
    this.InitCertificate();
    this.InitTradeForm();
  }
  PageTitle(): void {
    this.ComSrv.setTitle(this.ActivateRoute.snapshot.data.title);
    this.SpacerTitle = this.ActivateRoute.snapshot.data.title;
  }
  BankInfoForm: FormGroup;
  InitBankInfoForm() {
    this.BankInfoForm = this.fb.group({
      UserID: [this.currentUser.UserID],
      BankDetailID: [0],
      BankName: ['', [Validators.required]],
      OtherBank: [''],
      AccountTitle: ['', [Validators.required]],
      AccountNumber: ['', [Validators.required]],
      BranchAddress: ['', [Validators.required]],
      BranchCode: ['', [Validators.required]],
    });
    this.BankInfoForm.get('BankName').valueChanges.pipe(
      takeUntil(this.ngUnsubscribe)
    ).subscribe((bankId) => {
      if (bankId != null) {
        const selectedOption = this.bank.find(s => s.BankID === bankId).BankName;
        if (selectedOption == "Other") {
          this.BankInfoForm.get('OtherBank').setValidators(Validators.required);
          this.readOnly = false;
          this.required = true;
        } else {
          this.BankInfoForm.get('OtherBank').setValue("")
          this.BankInfoForm.get('OtherBank').clearValidators();
          this.readOnly = true;
          this.required = false;
        }
        this.BankInfoForm.get('OtherBank').updateValueAndValidity();
      }
    });
  }
  ngOnDestroy() {
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }
  BankEdit(data: any) {
    this.savebtn = "Update "
    this.BankInfoForm.get("BankDetailID").setValue(data.BankDetailID)
    this.BankInfoForm.patchValue(data)
  }
  EditTrainingLocation(data: any) {
    this.EditCheck = true;
    this.savebtn = "Update "
    this.TrainingForm.get("TrainingLocationID").setValue(data.TrainingLocationID)
    this.TrainingForm.patchValue(data)
  }
  EditCertificationDetail(data: any) {
    this.savebtn = "Update "
    this.Certificate.get("TrainingCertificationID").setValue(data.TrainingCertificationID)
    this.Certificate.patchValue(data)
  }
  EditTradeMapping(data: any) {
    this.savebtn = "Update "
    this.TradeForm.get("TradeManageID").setValue(data.TradeManageID)
    this.TradeForm.patchValue(data)
  }
  EditRecord(row: any, formId: string, formIdval: number, formName: FormGroup) {
    this.savebtn = "Update ";
    formName.get(formId).setValue(formIdval);
    formName.patchValue(row);
    this.GetTrainerDetail(formIdval)
    this.TrainerEditCheck = true;
  }
  PopulateTrainerDetail(data) {
    this.trainerDetails.clear()
    this.trainerEditRecord = data
    data.forEach(detail => {
      this.trainerDetails.push(this.fb.group(detail));
    });
  }
  GetTrainerDetail(Id: number) {
    if (Id != null && Id != 0) {
      this.ComSrv.postJSON("api/BaseData/GetTrainerDetail", this.TrainerForm.value).subscribe(
        (response) => {
          this.PopulateTrainerDetail(response)
        },
        (error) => {
          let url = error.url.split("/")
          this.ComSrv.ShowError(`${error.error} in ${url[4] + "/" + url[5]}`, "Close", 50000);
        }
      );
    } else {
      this.ComSrv.ShowError("Required form fields are missing");
    }
  }
  SaveBankInfo() {
    const control = this.BankInfoForm.controls
    if (this.BankInfoForm.valid) {
      this.ComSrv.postJSON("api/BaseData/SaveBank", this.BankInfoForm.value).subscribe(
        (response) => {
          this.LoadMatTable(response, "BankDetail")
          this.ComSrv.openSnackBar("Record saved successfully.");
          this.savebtn = "Add";
          this.BankInfoForm.reset()
          this.GetData();
        },
        (error) => {
          let url = error.url.split("/")
          this.ComSrv.ShowError(`${error.error} in ${url[4] + "/" + url[5]}`, "Close", 50000);
        }
      );
    } else {
      this.ComSrv.ShowError("Required form fields are missing");
    }
  }
  TrainingForm: FormGroup;
  InitTrainingForm() {
    this.TrainingForm = this.fb.group({
      UserID: [this.currentUser.UserID],
      TrainingLocationID: [0],
      TrainingLocationName: ['', [Validators.required]],
      Province: ['', [Validators.required]],
      Cluster: ['', [Validators.required]],
      District: ['', [Validators.required]],
      Tehsil: ['', [Validators.required]],
      TrainingLocationAddress: ['', [Validators.required]],
      GeoTagging: ['', [Validators.required]],
      FrontMainEntrancePhoto: ['', [Validators.required]],
      ClassroomPhoto: ['', [Validators.required]],
      ComputerLabPhoto: [''],
      PracticalAreaPhoto: ['', [Validators.required]],
      ToolsAndEquipmentsPhoto: ['', [Validators.required]],
      RegistrationAuthority: [4, [Validators.required]],
    });
  }
  SaveTrainingInfo() {
    const TrainingLocation = this.TrainingForm.get("TrainingLocationName").value
    if (this.TrainingLocation.length > 0 && this.EditCheck != true) {
      const IsExisted: any[] = this.TrainingLocation.filter(d => d.TrainingLocationName.trim().toLowerCase() == TrainingLocation.trim().toLowerCase())
      if (IsExisted.length > 0) {
        this.ComSrv.ShowError('The training location already exists. Please choose a different training location name')
        return
      }
    }
    if (this.TrainingForm.valid) {
      this.ComSrv.postJSON("api/BaseData/SaveTrainingLocation", this.TrainingForm.value).subscribe(
        (response) => {
          this.LoadMatTable(response, "TrainingLocation")
          this.ComSrv.openSnackBar("Record saved successfully.");
          this.savebtn = "Add";
          this.EditCheck = false;
          this.TrainingForm.reset()
          this.GetData();
        },
        (error) => {
          let url = error.url.split("/")
          this.ComSrv.ShowError(`${error.error} in ${url[4] + "/" + url[5]}`, "Close", 50000);
        }
      );
    } else {
      this.ComSrv.ShowError("Required form fields are missing");
    }
  }
  Certificate: FormGroup;
  InitCertificate() {
    this.Certificate = this.fb.group({
      UserID: [this.currentUser.UserID],
      TrainingCertificationID: [0],
      TrainingLocationID: ['', [Validators.required]],
      RegistrationAuthority: ['', [Validators.required]],
      RegistrationStatus: ['', [Validators.required]],
      RegistrationCerNum: ['', [Validators.required]],
      IssuanceDate: ['', [Validators.required]],
      ExpiryDate: ['', [Validators.required]],
      RegistrationCerEvidence: ['', [Validators.required]],
    });
    this.Certificate.get("TrainingLocationID").valueChanges.subscribe((TrainingLocationID) => {
      // if (TrainingLocationID != null && TrainingLocationID != "") {
      //   const selectedTraining = this.TrainingLocation.filter(t => t.TrainingLocationID == TrainingLocationID)[0].RegistrationAuthority
      //   // this.Certificate.controls.RegistrationAuthority.setValue(selectedTraining)
      // }
    })
  }
  SaveCertificateInfo() {
    if (this.Certificate.valid) {
      this.ComSrv.postJSON("api/BaseData/SaveCertification", this.Certificate.value).subscribe(
        (response) => {
          this.LoadMatTable(response, "Certification")
          this.ComSrv.openSnackBar("Record saved successfully.");
          this.savebtn = "Add";
          this.Certificate.reset()
          this.GetData();
        },
        (error) => {
          let url = error.url.split("/")
          this.ComSrv.ShowError(`${error.error} in ${url[4] + "/" + url[5]}`, "Close", 50000);
        }
      );
    } else {
      this.ComSrv.ShowError("Required form fields are missing");
    }
  }
  async SaveTradeInfo() {
    if (this.TradeForm.valid) {
      await this.ComSrv.postJSON("api/BaseData/SaveTradeMapping", this.TradeForm.value).subscribe(
        (response) => {
          this.TradeMapping = response
          if (this.TradeMapping.length > 0) {
            this.getTspTrade(this.TradeMapping)
          }
          this.LoadMatTable(response, "TradeMapping")
          this.ComSrv.openSnackBar("Record saved successfully.");
          this.savebtn = "Add";
          this.TradeForm.reset()
          this.GetData();
        },
        (error) => {
          let url = error.url.split("/")
          this.ComSrv.ShowError(`${error.error} in ${url[4] + "/" + url[5]}`, "Close", 50000);
        }
      );
    } else {
      this.ComSrv.ShowError("Required form fields are missing");
    }
  }
  TradeForm: FormGroup;
  InitTradeForm() {
    this.TradeForm = this.fb.group({
      UserID: [this.currentUser.UserID],
      TradeManageID: [0],
      TrainingLocationID: ['', [Validators.required]],
      CertificateID: ['', [Validators.required]],
      TradeID: ['', [Validators.required]],
      TradeAsPerCer: ['', [Validators.required]],
      TrainingDuration: ['', [Validators.required]],
      NoOfClassMor: ['', [Validators.required]],
      ClassCapacityMor: ['', [Validators.required]],
      NoOfClassEve: ['', [Validators.required]],
      ClassCapacityEve: ['', [Validators.required]],
    });
  }
  TrainerForm: FormGroup;
  TrainerDetails: FormArray;
  InitTrainerForm() {
    this.TrainerForm = this.fb.group({
      UserID: [this.currentUser.UserID],
      TrainerID: [0],
      TrainerName: ['', [Validators.required]],
      TrainerMobile: ['', [Validators.required, Validators.minLength(12)]],
      TrainerEmail: ['', [Validators.required, Validators.email]],
      Gender: ['', [Validators.required]],
      TrainerCNIC: ['', [Validators.required, Validators.minLength(15)]],
      CnicFrontPhoto: ['', [Validators.required]],
      CnicBackPhoto: ['', [Validators.required]],
      Qualification: ['', [Validators.required]],
      QualEvidence: ['', [Validators.required]],
      TrainerCV: ['', [Validators.required]],
      trainerDetails: this.fb.array([])
    });
    this.AddTrainerDetails()
  }
  SaveTrainerInfo() {
    const TrainerCNIC = this.TrainerForm.get('TrainerCNIC').value
    // return;
    if (TrainerCNIC != '' && this.TrainerEditCheck != true) {
      const IsExisted: any[] = this.TrainerProfile.filter(d => d.TrainerCNIC == TrainerCNIC)
      if (IsExisted.length > 0) {
        this.ComSrv.ShowError('Trainer CNIC already exists. Please choose a different CNIC')
        return
      }
    }
    if (this.trainerDetails.length > 0) {
      if (this.TrainerForm.valid && this.trainerDetails.valid) {
        this.ComSrv.postJSON("api/BaseData/SaveTrainerProfile", this.TrainerForm.value).subscribe(
          (response) => {
            this.TrainerProfile = response
            this.LoadMatTable(response, "TrainerProfile")
            this.ComSrv.openSnackBar("Record saved successfully.");
            this.savebtn = "Add";
            this.trainerEditRecord = []
            this.trainerDetails.clear()
            this.AddTrainerDetails()
            this.TrainerForm.reset()
            this.TrainerEditCheck = false
            this.GetData();
          },
          (error) => {
            let url = error.url.split("/")
            this.ComSrv.ShowError(`${error.error} in ${url[4] + "/" + url[5]}`, "Close", 50000);
          }
        );
      } else {
        this.ComSrv.ShowError("Required form fields are missing");
      }
    } else {
      this.ComSrv.ShowError("Trainer detail is required");
    }
  }
  AddTrainerDetails() {
    this.TrainerDetails = this.TrainerForm.get("trainerDetails") as FormArray;
    this.TrainerDetails.push(this.RowGenerator());
  }
  RowGenerator() {
    return this.fb.group({
      TrainerProfileID: [''],
      UserID: [this.currentUser.UserID],
      TrainerDetailID: [0],
      TrainerTradeID: ['', [Validators.required]],
      ProfQualification: ['', [Validators.required]],
      CertificateBody: ['', [Validators.required]],
      ProfQualEvidence: ['', [Validators.required]],
      RelExpYear: ['', [Validators.required]],
      RelExpLetter: ['', [Validators.required]],
    });
  }
  get trainerDetails() {
    return this.TrainerForm.get("trainerDetails") as FormArray;
  }
  RemoveDetail(index: any, row: FormArray) {
    if (confirm('Do you want to remove this details?')) {
      if (row.value[index].TrainerDetailID > 0) {
        this.DeleteTrainerDetail(row.value[index])
      }
      this.TrainerDetails = this.TrainerForm.get("trainerDetails") as FormArray;
      this.TrainerDetails.removeAt(index)
    }
  }
  DeleteTrainerDetail(row) {
    if (row.TrainerDetailID > 0) {
      this.ComSrv.postJSON("api/BaseData/DeleteTrainerDetail", row).subscribe(
        (response) => {
          this.PopulateTrainerDetail(response)
        },
        (error) => {
          let url = error.url.split("/")
          this.ComSrv.ShowError(`${error.error} in ${url[4] + "/" + url[5]}`, "Close", 50000);
        }
      );
    }
  }
  FormValueComparison(formValue: any, firstControl: any, secondControl: any) {
    const firstValue = formValue[firstControl];
    const secondValue = formValue[secondControl];
    if (firstValue && secondValue && firstValue === secondValue) {
      this.BankInfoForm.controls[secondControl].setErrors({
        customError: `${firstControl.charAt(0).toUpperCase() + firstControl.slice(1)} and ${secondControl.charAt(0).toUpperCase() + secondControl.slice(1)} must not be the same`
      });
    }
  }
  // GetProvince() {
  //   ['Cluster', 'District', 'Tehsil'].forEach(controlName => this.TrainingForm.get(controlName).setValue(''));
  // }
  // GetCluster(ProvinceId) {
  //   this.GetProvince();
  //   ['District', 'Tehsil'].forEach(controlName => this.TrainingForm.get(controlName).setValue(''));
  //   this.ClusterData = this.GetDataObject.cluster.filter(c => c.ProvinceID === ProvinceId);
  // }
  // GetDistrict(ClusterId) {
  //   ['Tehsil'].forEach(controlName => this.TrainingForm.get(controlName).setValue(''));
  //   this.DistrictData = this.GetDataObject.district.filter(d => d.ClusterID === ClusterId);
  // }
  // GetTehsil(DistrictId) {
  //   this.TehsilData = this.GetDataObject.tehsil.filter(t => t.DistrictID === DistrictId);
  // }

  GetTehsil(tehsilID) {
    
    if(tehsilID>0){
      const tehsilData = this.GetDataObject.tehsil.find(t => t.TehsilID == tehsilID);
      const districtData = this.GetDataObject.district.find(t => t.DistrictID == tehsilData.DistrictID);
      this.DistrictData = this.GetDataObject.district.filter(d => d.DistrictID == districtData.DistrictID)
      this.ClusterData = this.GetDataObject.cluster.filter(d => d.ClusterID == districtData.ClusterID)
      this.ProvinceData = this.GetDataObject.province.filter(d => d.ProvinceID == districtData.ProvinceID)
      this.TrainingForm.get("District").setValue(districtData.DistrictID)
      this.TrainingForm.get("Cluster").setValue(districtData.ClusterID)
      this.TrainingForm.get("Province").setValue(districtData.ProvinceID)
    }
   
  }

  GetData() {
    this.GetTSPProfileScore()
    this.ComSrv.postJSON("api/BaseData/GetData", { UserID: this.currentUser.UserID }).subscribe(
      (response) => {
        this.GetDataObject = response
        this.ProvinceData = this.GetDataObject.province
        this.bank = this.GetDataObject.Bank
        this.Gender = this.GetDataObject.Gender.filter(d => d.GenderID != 7 && d.GenderID != 8)
        this.EducationTypes = this.GetDataObject.EducationTypes
        this.RegistrationStatus = this.GetDataObject.RegistrationStatus
        this.registrationAuthority = this.GetDataObject.RegistrationAuthority
        this.Certification = this.GetDataObject.Certification
        this.Trade = this.GetDataObject.Trade
        this.BankDetail = this.GetDataObject.BankDetail
        this.TrainingLocation = this.GetDataObject.TrainingLocation
        this.TradeMapping = this.GetDataObject.TradeMapping
        this.TrainerProfile = this.GetDataObject.TrainerProfile

        this.TehsilData = this.GetDataObject.tehsil
        
        if (this.BankDetail.length > 0) {
          this.LoadMatTable(this.BankDetail, "BankDetail");
        }
        if (this.TrainingLocation.length > 0) {
          this.LoadMatTable(this.TrainingLocation, "TrainingLocation");
        }
        if (this.Certification.length > 0) {
          this.LoadMatTable(this.Certification, "Certification");
        }
        if (this.TradeMapping.length > 0) {
          this.LoadMatTable(this.TradeMapping, "TradeMapping");
          this.getTspTrade(this.TradeMapping)
        }
        if (this.TrainerProfile.length > 0) {
          this.LoadMatTable(this.TrainerProfile, "TrainerProfile");
        }
      },
      (error) => {
        let url = error.url.split("/")
        this.ComSrv.ShowError(`${error.error} in ${url[4] + "/" + url[5]}`, "Close", 50000);
      }
    );
  }
  readOnly = true
  required = false
  OpenMapDialogue() {
    let dialogRef = this.Dialog.open(GeoTaggingComponent, {
      width: '60%',
      disableClose: true,
    });
    dialogRef.afterClosed().subscribe(result => {
      this.TrainingForm.get('GeoTagging').setValue(this.ComSrv.sharedDataObj.join(','))
      this.readOnly = false;
    });
  }
  EmptyCtrl(ev: any) {
    this.PSearchCtr.setValue('');
    this.CSearchCtr.setValue('');
    this.DSearchCtr.setValue('');
    this.DSearchCtr.setValue('');
    this.BSearchCtr.setValue('');
    this.TSearchCtr.setValue('');
    this.TTSearchCtr.setValue('');
  }
  ResetFrom() {
    this.EditCheck = false
    this.TrainerEditCheck = false
    this.savebtn = "Add";
  }
  camelCaseToWords(input: string): string {
    return input.replace(/([a-z])([A-Z])/g, '$1 $2');
  }
  LoadMatTable(tableData: any, ReportName: string) {
    if (tableData != undefined) {
      switch (ReportName) {
        case "BankDetail":
          this.BankTableColumns = Object.keys(tableData[0]).filter(key =>
            !key.toLowerCase().includes('id') &&
            !["BankName", "OtherBank"].includes(key)
          );
          this.BankTablesData = new MatTableDataSource(tableData);
          this.BankTablesData.paginator = this.BankPaginator;
          this.BankTablesData.sort = this.BankSort;
          break;
        case "TrainingLocation":
          this.TrainingTableColumns = Object.keys(tableData[0])
          .filter(key => 
              !key.toLowerCase().includes('id') &&
              !key.toLowerCase().includes('photo') &&
              !["Province", "Cluster", "District", "Tehsil", "RegistrationAuthorityName", "RegistrationAuthority"].includes(key)
          );
      
          this.TrainingTablesData = new MatTableDataSource(tableData);
          this.TrainingTablesData.paginator = this.TrainingPaginator;
          this.TrainingTablesData.sort = this.TrainingSort;
          break;
        case "Certification":
          this.CertificateTableColumns = Object.keys(tableData[0]).filter(key =>
            !key.includes('ID') &&
            !["RegistrationCerEvidence", "RegistrationStatus", "RegistrationAuthority"].includes(key)
          );
          this.CertificateTablesData = new MatTableDataSource(tableData);
          this.CertificateTablesData.paginator = this.CertificatePaginator;
          this.CertificateTablesData.sort = this.CertificateSort;
          break;
        case "TradeMapping":
          this.TradeMapTableColumns = Object.keys(tableData[0]).filter(key =>
            !key.includes('ID') &&
            ![""].includes(key)
          );
          this.TradeTablesData = new MatTableDataSource(tableData);
          this.TradeTablesData.paginator = this.TradePaginator;
          this.TradeTablesData.sort = this.TradeSort;
          break;
        case "TrainerProfile":
          this.TrainerTableColumns = Object.keys(tableData[0]).filter(key =>
            !key.includes('ID') &&
            !["TrainerCV", "QualEvidence", "CnicFrontPhoto", "CnicBackPhoto", "Qualification", "Gender"].includes(key)
          );
          this.TrainerTablesData = new MatTableDataSource(tableData);
          this.TrainerTablesData.paginator = this.TrainerPaginator;
          this.TrainerTablesData.sort = this.TrainerSort;
          break;
      }
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
  getErrorMessage(errorKey: string, errorValue: any): string {
    const error = errorValue.requiredLength == 15 ? errorValue.requiredLength - 2 : errorValue.requiredLength - 1
    const errorMessages = {
      required: 'This field is required.',
      minlength: `This field must be at least ${error} characters long.`,
      maxlength: `This field's text exceeds the specified maximum length.  (maxLength: ${errorValue.requiredLength} characters)`,
      email: 'Invalid email address.',
      pattern: 'This field is only required text',
      customError: errorValue
    };
    return errorMessages[errorKey];
  }
  getSelectedTabData() {
    switch (this.tabGroup.selectedIndex) {
      case 0:
        this.SpacerTitle = "Training Location"
        break;
      case 1:
        this.SpacerTitle = "Training Certificate"
        break;
      case 2:
        this.SpacerTitle = "Training Certificate Mapping"
        break;
      case 3:
        this.SpacerTitle = "Trainer Profile"
        break;
      case 4:
        this.SpacerTitle = "Bank Detail"
        break;
      default:
    }
  }
  ShowPreview(fileName: string) {
    this.ComSrv.PreviewDocument(fileName)
  }
  async proceedRegistrationPayment() {
    
    const bankDetail = this.TSPProfileScore.some(score => score.FormName === 'BankDetail' && score.Score === 0);
    if(bankDetail){
      this.ComSrv.ShowError('Minimum One Bank is required to proceedPayment.')
      return;
    }
    
    const check = await this.checkTradeTrainerMapped();
    if (check === 1) {
      this.router.navigateByUrl('/payment/registration-payment');
    }
  }
  async SetDefault(row) {
    this.SPName = "AU_SetDefault"
    this.paramObject = {
      BankDetailID: row.BankDetailID,
      CurrentUserID: this.currentUser.UserID
    }
    this.BankDetail = []
    this.BankDetail = await this.FetchData(this.SPName, this.paramObject)
    if (this.BankDetail.length > 0) {
      this.LoadMatTable(this.BankDetail, "BankDetail");
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
      // if (data.length > 0) {
      return data;
      // } else {
      //   // this.ComSrv.ShowWarning(' No Record Found', 'Close');
      // }
    } catch (error) {
      this.error = error;
    }
  }
  getTspTrade(TradeMapData: any) {
    this.TspMappedTrades = [];

    const TspTradeMap = new Map();

    TradeMapData.forEach(trade => {
      TspTradeMap.set(trade.TradeName, trade.TradeID);
    });
    
    TspTradeMap.forEach((tradeID, tradeName) => {
      this.TspMappedTrades.push({ TradeName: tradeName, TradeID: tradeID });
    });
  }

  TrainerDetailByUserID: any[] = []
  async getTrainerDetail() {
    this.SPName = "RD_SSPTrainerProfileDetail"
    this.paramObject = {
      UserID: this.currentUser.UserID,
      TrainerProfileID: 0
    }
    this.TrainerDetailByUserID = []
    this.TrainerDetailByUserID = await this.FetchData(this.SPName, this.paramObject)
  }

  async checkTradeTrainerMapped() {
    await this.getTrainerDetail();
    if (this.TspMappedTrades.length != 0) {
      const trainerTradeIDs = this.TrainerDetailByUserID.map(item => item.TrainerTradeID);
      
      const tradeNotMappedTrainer: any[] = this.TspMappedTrades
        .filter(item => !trainerTradeIDs.includes(item.TradeID))
        .map(item => item.TradeName);
      
      if (tradeNotMappedTrainer.length === 0) {
        return 1;
      } else {
        const msg = `Please map trainer with the following trade(s): ${tradeNotMappedTrainer.join(',')}`;
        this.ComSrv.ShowError(msg, 'Close', 15000);
        return 0;
      }
    } else {
      this.ComSrv.ShowError("Trade mapping with training location is necessary to set up the trainer profile.", 'Close', 10000)
    }
  }

  NextTap(): void {
   
    this.tabGroup.selectedIndex = this.TapIndex+1;
  }
  PreviousTap(): void {
    this.tabGroup.selectedIndex =this.TapIndex-1;
  }

  totalScore: number = 100; 

  currentScore: number = 0; 

  calculateScore() {
    this.currentScore = this.TSPProfileScore.map(d=>d.Score).reduce((accumulator, currentValue) => accumulator + currentValue, 0);
    this.PendingForm=this.TSPProfileScore.filter(d=>d.Score==0).map(d=>d.FormName)
  }

  get progressPercentage(): number {
    return (this.currentScore / this.totalScore) * 100;
  }
  
  TSPProfileScore = []
  async GetTSPProfileScore() {
    this.SPName = "RD_SSPProfileScore"
    this.paramObject = {
      UserID: this.currentUser.UserID
    }
    this.TSPProfileScore = []
    this.TSPProfileScore = await this.FetchReport(this.SPName, this.paramObject)
    this.calculateScore();

 this.checkProfileScores()

  }

  checkProfileScores() {
    const businessProfileIncomplete = this.TSPProfileScore.some(score => score.FormName === 'BusinessProfile' && score.Score === 0);
    const contactPersonIncomplete = this.TSPProfileScore.some(score => score.FormName === 'ContactPerson' && score.Score === 0);
    
    if (businessProfileIncomplete || contactPersonIncomplete) {
      this.fieldSetDisabled = true;
      this.ComSrv.ShowError('Please complete the Business Profile and Contact Person Information before completing the Base Data.');
      this.TrainingForm.disable();
      this.Certificate.disable();
      this.TradeForm.disable();
      this.TrainerForm.disable();
    } else {
      this.fieldSetDisabled = false;
      this.TrainingForm.enable();
      this.Certificate.enable();
      this.TradeForm.enable();
      this.TrainerForm.enable();
    }
  }
  
  async FetchReport(SPName: string, paramObject: any) {
    try {
      const Param = this.GetParamString(SPName, paramObject);
      const data: any  = await this.ComSrv.postJSON('api/BSSReports/FetchReport',Param).toPromise();
      if (data != undefined) {
        return data;
      } else {
        if (SPName != 'RD_SSPTSPAssociationSubmission') {
          this.ComSrv.ShowWarning(' No Record Found', 'Close');
        }
      }
    } catch (error) {
      this.error = error;
      this.ComSrv.ShowError(error.error)
    }
  }

  ngAfterViewInit() {
    if (this.tabGroup) {
      this.tabGroup.selectedTabChange.subscribe((event) => {
        this.TapIndex = event.index
      });
      if (this.TapIndex == 3 && this.TspMappedTrades.length == 0) {
        this.ComSrv.ShowError("Trade mapping with training location is necessary to set up the trainer profile.", 'Close', 10000)
      }
    }
  }
}
