import { Component, OnInit, ViewChild, ChangeDetectorRef } from "@angular/core";
import { CommonSrvService } from "../../common-srv.service";
import { ActivatedRoute, Router } from "@angular/router";
import { FormBuilder, FormControl, FormGroup, Validators } from "@angular/forms";
import { MatDialog } from "@angular/material/dialog";
import { GeoTaggingComponent } from "src/app/custom-components/geo-tagging/geo-tagging.component";
import { MatTabGroup } from "@angular/material/tabs";
@Component({
  selector: "app-profile",
  templateUrl: "./profile.component.html",
  styleUrls: ["./profile.component.scss"],
})
export class ProfileComponent implements OnInit {
  TapIndex: any;
  error: any;
  profile: any;
  programType: any;
  NTNEvidence: any;
  PRAEvidence: any;
  GSTEvidence: any;
  diaLogueReadOnly = false
  currentUser: any;
  GetDataObject: any;
  BusinessType: any;
  CurrentUserID: any;
  LegalStatusEvidence: any;
  OrgHeadCNICFrontImgUrl: any;
  OrgHeadCNICBackImgUrl: any;
  readonly: boolean = false
  ProFileReadonly: boolean = false
  COPreadonly: boolean = false
  legalStatus: any;
  incompleteForms: any;
  PendingForm: any[] = []
  constructor(
    private Route: Router,
    private ComSrv: CommonSrvService,
    private ActiveRoute: ActivatedRoute,
    private fb: FormBuilder,
    private Dialog: MatDialog,
  ) { }
  SpacerTitle: string;
  SearchCtr = new FormControl('');
  PSearchCtr = new FormControl('');
  CSearchCtr = new FormControl('');
  DSearchCtr = new FormControl('');
  TSearchCtr = new FormControl('');
  TapTTitle: string = "Business Profile"
  Data: any = []
  STaxType: any = []
  ProvinceData: any = []
  ClusterData: any = []
  DistrictData: any = []
  TehsilData: any = []
  TableColumns = [];
  maxDate: Date;
  SalesTaxType: string = "Sales Tax Evidence"
  ProfileForm: FormGroup;
  ContactInfoForm: FormGroup;
  @ViewChild("tabGroup") tabGroup: MatTabGroup;
  ngOnInit(): void {
    this.TapIndex = 0
    this.PageTitle();
    this.currentUser = this.ComSrv.getUserDetails();
    this.GetData();
    this.InitProfileForm();
    this.InitContactInfoForm();
  }
  PageTitle(): void {
    this.ComSrv.setTitle(this.ActiveRoute.snapshot.data.title);
    this.SpacerTitle = this.ActiveRoute.snapshot.data.title;
  }
  InitProfileForm() {
    this.ProfileForm = this.fb.group({
      TspID: [''],
      TaxTypeID: [''],
      InstituteName: ['', [Validators.required]],
      RegistrationDate: [{ value: '' }, [Validators.required]],
      InstituteNTN: ['', [Validators.required]],
      NTNAttachment: ['', [Validators.required]],
      TaxType: ['', [Validators.required]],
      GSTNumber: [{ value: '', disabled: true }],
      GSTAttachment: [{ value: '', disabled: true }],
      PRANumber: [{ value: '', disabled: true }],
      PRAAttachment: [{ value: '', disabled: true }],
      LegalStatus: ['', [Validators.required]],
      LegalStatusAttachment: ['', [Validators.required]],
      Province: ['', [Validators.required]],
      Cluster: ['', [Validators.required]],
      District: ['', [Validators.required]],
      Tehsil: ['', [Validators.required]],
      LatitudeAndLongitude: ['', [Validators.required]],
      HeadOfficeAddress: ['', [Validators.required]],
      BusinessType: ['', [Validators.required]],
      Website: [''],
    });
    this.ProfileForm.get("TaxType").valueChanges.subscribe((TaxValue) => {
      this.UpdateTaxType(TaxValue)
    });
  }
  UpdateTaxType(TaxValue) {
    const GSTNumber = this.ProfileForm.get('GSTNumber');
    const GSTAttachment = this.ProfileForm.get("GSTAttachment");
    const PRANumber = this.ProfileForm.get('PRANumber');
    const PRAAttachment = this.ProfileForm.get("PRAAttachment");
    GSTNumber.disable();
    GSTAttachment.disable();
    PRANumber.disable();
    PRAAttachment.disable();
    if (TaxValue[0] == 1 || TaxValue[0] == 2) {
      this.STaxType = this.GetDataObject.salesTax.filter(d => d.SalesTaxID != 3)
    } else {
      this.STaxType = this.GetDataObject.salesTax
    }
    if (TaxValue.length == 1 && TaxValue[0] == 1) {
      GSTNumber.setValidators([Validators.required]);
      GSTAttachment.setValidators([Validators.required]);
      PRANumber.setValue("");
      PRAAttachment.setValue("");
      GSTNumber.enable();
      GSTAttachment.enable();
    } else if (TaxValue.length == 1 && TaxValue[0] == 2) {
      PRANumber.setValidators([Validators.required]);
      PRAAttachment.setValidators([Validators.required]);
      GSTNumber.setValue("");
      GSTAttachment.setValue("");
      PRANumber.enable();
      PRAAttachment.enable();
    } else if (TaxValue.length == 1 && TaxValue[0] == 3) {
      GSTNumber.setValue("");
      GSTAttachment.setValue("");
      PRANumber.setValue("");
      PRAAttachment.setValue("");
    } else {
      // Reset validators for all fields
      GSTNumber.setValidators([Validators.required]);
      GSTAttachment.setValidators([Validators.required]);
      PRANumber.setValidators([Validators.required]);
      PRAAttachment.setValidators([Validators.required]);
      // Enable all fields
      GSTNumber.enable();
      GSTAttachment.enable();
      PRANumber.enable();
      PRAAttachment.enable();
    }
    // Update form validity
    this.ProfileForm.updateValueAndValidity();
  }
  FormValueComparison(formValue: any, firstControl: any, secondControl: any) {
    const firstValue = formValue[firstControl];
    const secondValue = formValue[secondControl];
    if (firstValue === secondValue) {
      this.ContactInfoForm.controls[secondControl].setErrors({
        customError: `${firstControl.charAt(0).toUpperCase() + firstControl.slice(1)} and ${secondControl.charAt(0).toUpperCase() + secondControl.slice(1)} must not be the same`
      });
    }
  }
  // GetProvince() {
  //   ['Cluster', 'District', 'Tehsil'].forEach(controlName => this.ProfileForm.get(controlName).setValue(''));
  // }
  // GetCluster(ProvinceId) {
  //   this.GetProvince();
  //   ['District', 'Tehsil'].forEach(controlName => this.ProfileForm.get(controlName).setValue(''));
  //   this.ClusterData = this.GetDataObject.cluster.filter(c => c.ProvinceID === ProvinceId);
  // }
  // GetDistrict(ClusterId) {
  //   ['Tehsil'].forEach(controlName => this.ProfileForm.get(controlName).setValue(''));
  //   this.DistrictData = this.GetDataObject.district.filter(d => d.ClusterID === ClusterId);
  // }
  // GetTehsil(DistrictId) {
  //   this.TehsilData = this.GetDataObject.tehsil.filter(t => t.DistrictID === DistrictId);
  // }
  GetTehsil(tehsilID) {
    const tehsilData = this.GetDataObject.tehsil.filter(t => t.TehsilID == tehsilID);
    const districtData = this.GetDataObject.district.find(t => t.DistrictID == tehsilData[0].DistrictID);
    // this.TehsilData = this.GetDataObject.tehsil.filter(d=>d.TehsilID==tehsilID)
    this.DistrictData = this.GetDataObject.district.filter(d => d.DistrictID == districtData.DistrictID)
    this.ClusterData = this.GetDataObject.cluster.filter(d => d.ClusterID == districtData.ClusterID)
    this.ProvinceData = this.GetDataObject.province.filter(d => d.ProvinceID == districtData.ProvinceID)
    this.ProfileForm.get("District").setValue(districtData.DistrictID)
    this.ProfileForm.get("Cluster").setValue(districtData.ClusterID)
    this.ProfileForm.get("Province").setValue(districtData.ProvinceID)
  }
  NextTap(): void {
    this.tabGroup.selectedIndex += 1;
  }
  PreviousTap(): void {
    this.tabGroup.selectedIndex -= 1;
  }
  NextTapForBaseData() {
    this.Route.navigateByUrl("profile-manage/base-data")
  }
  totalScore: number = 100;
  currentScore: number = 0; // Initialize the current score
  calculateScore() {
    this.currentScore = this.TSPProfileScore.map(d => d.Score).reduce((accumulator, currentValue) => accumulator + currentValue, 0);
    this.PendingForm = this.TSPProfileScore.filter(d => d.Score == 0).map(d => d.FormName)
  }
  get progressPercentage(): number {
    return (this.currentScore / this.totalScore) * 100;
  }
  async GetData() {
    this.GetTSPProfileScore()
    this.ComSrv.postJSON("api/BusinessProfile/GetData", { UserID: this.currentUser.UserID }).subscribe(
      (response) => {
        this.GetDataObject = response
        this.TehsilData = this.GetDataObject.tehsil
        this.DistrictData = this.GetDataObject.district
        this.ClusterData = this.GetDataObject.cluster
        this.ProvinceData = this.GetDataObject.province
        this.BusinessType = this.GetDataObject.programType.filter(d => [1, 2, 3].includes(d.PTypeID));
        this.STaxType = this.GetDataObject.salesTax
        this.legalStatus = this.GetDataObject.legalStatus
        if (this.GetDataObject.profile[0]) {
          this.ProfileEdit(this.GetDataObject.profile[0])
          this.POCEdit(this.GetDataObject.profile[0])
        } else {
          const control = this.ProfileForm.controls
          control.HeadOfficeAddress.setValue(this.GetDataObject.masterDetail[0].Address)
          control.InstituteNTN.setValue(this.GetDataObject.masterDetail[0].NTN)
          control.InstituteName.setValue(this.GetDataObject.masterDetail[0].TSPName)
        }
      },
      (error) => {
        let url = error.url.split("/")
        this.ComSrv.ShowError(`${error.error} in ${url[4] + "/" + url[5]}`, "Close", 50000);
      }
    );
  }
  ProfileEdit(profile: any): void {
    if (profile) {
      if (profile.NTNAttachment) {
        this.readonly = true
      }
      const formControls = this.ProfileForm.controls;
      this.NTNEvidence = profile.NTNEvidence
      this.PRAEvidence = profile.PRAEvidence
      this.GSTEvidence = profile.GSTEvidence
      this.LegalStatusEvidence = profile.LegalStatusEvidence
      if (profile.TaxTypeID) {
        formControls.TaxType.setValue(profile.TaxTypeID.trim().split(',').map(Number));
      }
      formControls.InstituteName.setValue(profile.InstituteName);
      formControls.InstituteNTN.setValue(profile.InstituteNTN);
      formControls.RegistrationDate.setValue(profile.RegistrationDate);
      formControls.NTNAttachment.setValue(profile.NTNAttachment);
      formControls.BusinessType.setValue(profile.ProgramTypeID);
      formControls.GSTNumber.setValue(profile.GSTNumber);
      formControls.GSTAttachment.setValue(profile.GSTAttachment);
      formControls.PRANumber.setValue(profile.PRANumber);
      formControls.PRAAttachment.setValue(profile.PRAAttachment);
      formControls.LegalStatus.setValue(profile.LegalStatusID);
      formControls.LegalStatusAttachment.setValue(profile.LegalStatusAttachment);
      formControls.Province.setValue(profile.ProvinceID);
      formControls.Cluster.setValue(profile.ClusterID);
      formControls.District.setValue(profile.DistrictID);
      formControls.Tehsil.setValue(profile.TehsilID);
      formControls.LatitudeAndLongitude.setValue(profile.GeoTagging);
      formControls.Website.setValue(profile.Website);
      formControls.HeadOfficeAddress.setValue(profile.Address);
    }
  }
  SaveProfile() {
    debugger;
    this.ProfileForm.get("TspID").setValue(this.currentUser.UserID)


    if (this.GetDataObject.profile[0]) {
      this.ProfileForm.controls.InstituteName.setValue(this.GetDataObject.profile[0].InstituteName);
      this.ProfileForm.controls.InstituteNTN.setValue(this.GetDataObject.profile[0].InstituteNTN);
    } else {
      const control = this.ProfileForm.controls
      control.InstituteNTN.setValue(this.GetDataObject.masterDetail[0].NTN)
      control.InstituteName.setValue(this.GetDataObject.masterDetail[0].TSPName)
    }

    const salesTax = this.ProfileForm.get("TaxType").value;
    if (salesTax && Array.isArray(salesTax)) {
      this.ProfileForm.get("TaxTypeID").setValue(salesTax.join(","));
    }

    if (this.ProfileForm.valid) {
      this.ComSrv.postJSON("api/BusinessProfile/Save", this.ProfileForm.value).subscribe(
        (response) => {
          if (response[0] > 0) {
            this.readonly = false
          }
          this.GetTSPProfileScore()
          this.ComSrv.openSnackBar("Profile has been updated.");
          this.ProfileEdit(response[0])
        },
        (error) => {
          let url = error.url.split("/")
          this.ComSrv.ShowError(`${error.error} in ${url[4] + "/" + url[5]}`, "Close", 50000);
        }
      );
    } else {
      this.ComSrv.ShowError("please enter valid data");
    }
  }
  InitContactInfoForm() {
    this.ContactInfoForm = this.fb.group({
      TspID: [''],
      InstituteName: [''],
      InstituteNTN: [''],
      HeadofOrgName: ['', [Validators.required]],
      HeadofOrgDesi: ['', [Validators.required]],
      CNICofHeadofOrg: ['', [Validators.required]],
      HeadofOrgCNICFrontPhoto: ['', [Validators.required]],
      HeadofOrgCNICBackPhoto: ['', [Validators.required]],
      HeadofOrgEmail: ['', [Validators.required, Validators.email]],
      HeadofOrgMobile: ['', [Validators.required, Validators.minLength(12)]],
      ORGLandline: ['', [Validators.required, Validators.maxLength(15)]],
      POCName: ['', [Validators.required]],
      POCDesignation: ['', [Validators.required]],
      POCEmail: ['', [Validators.required, Validators.email]],
      POCMobile: ['', [Validators.required, Validators.minLength(12)]],
    });
    this.ContactInfoForm.valueChanges.subscribe((formValues) => {
      this.FormValueComparison(formValues, "HeadofOrgName", "POCName");
      this.FormValueComparison(formValues, "HeadofOrgDesi", "POCDesignation");
      this.FormValueComparison(formValues, "HeadofOrgEmail", "POCEmail");
      this.FormValueComparison(formValues, "HeadofOrgMobile", "POCMobile");
    });
  }
  SaveContactPersonInfo() {
    const check = this.PendingForm.includes("BusinessProfile");

    if (!check) {
      this.ContactInfoForm.get("TspID").setValue(this.currentUser.UserID)
      this.ContactInfoForm.get("InstituteName").setValue(this.ProfileForm.get("InstituteName").value)
      this.ContactInfoForm.get("InstituteNTN").setValue(this.ProfileForm.get("InstituteNTN").value)
      if (this.ContactInfoForm.valid) {
        this.ComSrv.postJSON("api/BusinessProfile/SavePOC", this.ContactInfoForm.value).subscribe(
          (response) => {
            if (response[0] > 0) {
              this.COPreadonly = false
            }
            this.GetTSPProfileScore()
            this.ComSrv.openSnackBar("Profile data has been modified.");
            this.POCEdit(response[0])
          },
          (error) => {
            let url = error.url.split("/")
            this.ComSrv.ShowError(`${error.error} in ${url[4] + "/" + url[5]}`, "Close", 50000);
          }
        );
      } else {
        this.ComSrv.ShowError("please enter valid data");
      }
    }
    else {
      this.ComSrv.ShowError("Business profile is not completed. Please complete the business profile!");
    }
  }
  POCEdit(profile: any): void {
    if (profile.OrgHeadCNICFrontImgUrl) {
      this.COPreadonly = true
      const formControls = this.ContactInfoForm.controls;
      formControls.HeadofOrgName.setValue(profile.HeadName);
      formControls.CNICofHeadofOrg.setValue(profile.HeadCnicNum);
      formControls.HeadofOrgCNICFrontPhoto.setValue(profile.HeadCnicFrontImg);
      formControls.HeadofOrgCNICBackPhoto.setValue(profile.HeadCnicBackImg);
      formControls.HeadofOrgDesi.setValue(profile.HeadDesignation);
      formControls.HeadofOrgEmail.setValue(profile.HeadEmail);
      formControls.HeadofOrgMobile.setValue(profile.HeadMobile);
      formControls.ORGLandline.setValue(profile.OrgLandline);
      formControls.POCName.setValue(profile.POCName);
      formControls.POCDesignation.setValue(profile.POCDesignation);
      formControls.POCMobile.setValue(profile.POCMobile);
      formControls.POCEmail.setValue(profile.POCEmail);
      this.OrgHeadCNICFrontImgUrl = profile.OrgHeadCNICFrontImgUrl
      this.OrgHeadCNICBackImgUrl = profile.OrgHeadCNICBackImgUrl
    }
  }
  EmptyCtrl(ev: any) {
    this.PSearchCtr.setValue('');
    this.CSearchCtr.setValue('');
    this.DSearchCtr.setValue('');
    this.TSearchCtr.setValue('');
  }
  OpenMapDialogue() {
    // let dialogRef: MatDialogRef<GeoTaggingComponent>;
    let dialogRef = this.Dialog.open(GeoTaggingComponent, {
      height: '70%',
      width: '60%',
      // disableClose: true,
    });
    dialogRef.afterClosed().subscribe(result => {
      this.ProfileForm.get('LatitudeAndLongitude').setValue(this.ComSrv.sharedDataObj.join(','))
      this.diaLogueReadOnly = false;
    });
  }
  resetFromData() {
    if (confirm('do you want to reset form data')) {
      this.ProfileForm.reset()
    }
  }
  TSPProfileScore = []
  async GetTSPProfileScore() {
    this.SPName = "RD_SSPProfileScore"
    this.paramObject = {
      UserID: this.currentUser.UserID
    }
    this.TSPProfileScore = []
    this.TSPProfileScore = await this.FetchData(this.SPName, this.paramObject)
    this.calculateScore();
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
      const data: any = await this.ComSrv.postJSON('api/BSSReports/FetchReport', Param).toPromise();
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
  getErrorMessage(errorKey: string, errorValue: any): string {
    const errorMessages = {
      required: 'This field is required.',
      minlength: `This field must be at least ${errorValue.requiredLength - 1} characters long.`,
      maxlength: `This field's text exceeds the specified maximum length.  (maxLength: ${errorValue.requiredLength} characters)`,
      email: 'Invalid email address.',
      pattern: 'This field is only required text',
      customError: errorValue
    };
    return errorMessages[errorKey];
  }
  ShowPreview(fileName: string) {
    this.ComSrv.PreviewDocument(fileName)
  }
  ngAfterViewInit() {
    if (this.tabGroup) {
      this.tabGroup.selectedTabChange.subscribe((event) => {
        this.TapIndex = event.index
      });
    }
  }
}
