import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from '../../../environments/environment';
import { FormGroup, Validators, FormGroupDirective, FormControl, FormBuilder, NgForm } from '@angular/forms';
import { CommonSrvService } from '../../common-srv.service';
import { UserRightsModel } from '../../master-data/users/users.component';
import { IOrgConfig } from '../Interface/IOrgConfig';
import { ITraineeProfile } from '../Interface/ITraineeProfile';
import { DomSanitizer } from '@angular/platform-browser';
import { SelectionModel } from '@angular/cdk/collections';
import { ActivatedRoute } from '@angular/router';
import { EnumGender, EnumUserLevel } from '../../shared/Enumerations';
import { MatTabGroup } from '@angular/material/tabs';
import { SchemeModel } from '../../appendix-module/appendix/appendix.component';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { saveAs } from 'file-saver';
import { MatDialog, MatDialogRef } from "@angular/material/dialog";
import * as JSZip from 'jszip';
import { TraineeGuruDialogComponent } from '../trainee-guru-dialog/trainee-guru-dialog.component';

@Component({
  selector: 'app-trainee',
  templateUrl: './trainee.component.html',
  styleUrls: ['./trainee.component.scss']
})
export class TraineeComponent implements OnInit {

  SearchCls = new FormControl('',);

  environment = environment;
  traineeProfileForm: FormGroup;
  ///Mat-Table checkbox Config - Start
  traineeProfile: MatTableDataSource<ITraineeProfile>;
  displayedColumns = ['Check', 'TraineeImg', 'TraineeCode', 'TraineeName', 'FatherName', 'TraineeCNIC', 'DateOfBirth', 'ContactNumber1', 'ClassCode', 'TradeName', 'SchemeName'];
  selection = new SelectionModel<ITraineeProfile>(true, []);
  submittedTrainees: number = 0;
  savedTrainees: number = 0;
  traineeThreshold: number = 0;
  registrationError: string = "";
  /** Whether the number of selected elements matches the total number of rows. */
  isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.traineeProfile.data.length;
    return numSelected === numRows;
  }
  /** Selects all rows if they are not all selected; otherwise clear selection. */
  masterToggle() {
    this.isAllSelected() ?
      this.selection.clear() :
      this.traineeProfile.data.forEach(row => this.selection.select(row));
  }
  /** The label for the checkbox on the passed row */
  checkboxLabel(row?: ITraineeProfile): string {
    if (!row) {
      return `${this.isAllSelected() ? 'select' : 'deselect'} all`;
    }
    return `${this.selection.isSelected(row) ? 'deselect' : 'select'} row ${1}`;
  }

  traineeProfileArray: any[] = [];
  orgConfig: IOrgConfig;
  traineeClass: any;
  Gender: any[];
  ddlGender: any[];
  Trade: any;
  EducationTypes: any;
  ddlEducationTypes: any;
  //Cluster: any;
  province: any;
  District: any;
  Tehsil: any;
  TemporaryProvince: any;
  TempDistrict: any;
  TempTehsil: any;
  TraineeDisability: any[];
  IncomeRange: any[];
  ReferralSource: any[];
  Sections: any;
  //Scheme: any;
  TSPDetail: any;
  Religion: any;
  EmploymentStatus: any[];
  ClassStatuses: any;
  TraineeProfile_seqNextVal: BigInteger;
  classStartDate: Date;
  classEndDate: Date;
  traineesPerClass: number = 0;
  totalAllowedTrineesPerClass: number;
  hiddenSaveExtra: boolean = false;
  enableSaveExtra: boolean = false;
  isFormDisabled: boolean = true;
  IsFieldDisabled: boolean = true;
  isProgramFieldDisabled: boolean = false;
  isClosedBracketDays: boolean = false;
  isCompletedAllowedTrainees: boolean = false;
  paramClassId: string;
  classId: number;
  classesArray: any;
  formrights: UserRightsModel;
  isTspUser: boolean;
  saveBtnTitle: string = "Save";
  EnText: string = "";
  error: string;
  EDFScheme: boolean = false;
  SearchPro = new FormControl('');
  IsSkillsScholrship: boolean = false;
  IsSkillsScholrshipProgram: boolean = false;
  IsInternationalPlacement: boolean = false;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild("ngForm") ngFrom: NgForm;
  @ViewChild("tabGroup") tabGroup: MatTabGroup;
  //working: boolean;
  isValidForm: boolean = true;
  registrationReport: any;
  constructor(    private Dialog: MatDialog, private fb: FormBuilder, private https: HttpClient, private http: CommonSrvService, private router: Router, private domSanitizer: DomSanitizer, private route: ActivatedRoute) {
    this.createTraineeProfileForm();
    route.params.subscribe(
      params => { this.paramClassId = params["id"]; });

  }

  downloadFolder() {
    const specificColumnValues: any[] = [];
    for (const traineeProfile of this.traineeProfileArray) {

      const columnValue = traineeProfile.TraineeDocURL; // Replace columnName with the actual column name in your array
      if (columnValue != "" || columnValue != '') {
        specificColumnValues.push('\$' + columnValue);
      }
    }

    const zip = new JSZip();
    const foldername = this.ClassCode.value + '.zip';
    //const folderUrl = '$/Documents/Traniee_Profiles/TraineeDocuments/J8E-2275-0001/J8E-2275-0001-31/54bb01f9-5396-4ff7-819d-a6db03551fff.jpeg';
    // Add files or directories to the ZIP. You can modify this part based on your specific folder structure.
    specificColumnValues.forEach((value, index) => {
      zip.file(value, 'File' + index);
    });

    // Generate the ZIP file asynchronously
    zip.generateAsync({ type: 'blob' }).then((content) => {
      // Use the FileSaver library to save the ZIP file
      saveAs(content, foldername);
    });
  }

  ngOnInit() {
    this.http.setTitle("Trainee Profile");
    this.formrights = this.http.getFormRights("trainee");
    this.isTspUser = this.http.getUserDetails().UserLevel == EnumUserLevel.TSP;

    this.http.OID.subscribe(OID => {
      this.traineeProfile = new MatTableDataSource([]);
      this.traineeProfileArray = [];
      this.hiddenSaveExtra = false;
      this.enableSaveExtra = false;
      //this.isFormDisabled = true;
      this.isClosedBracketDays = false;
      this.isCompletedAllowedTrainees = false;
      this.getData();
      this.getSetDataByClass();
      this.saveBtnTitle = "Save";
      this.createTraineeProfileForm();
      this.setFormIsDisabled(true);
      this.ControlProvince();
      this.ControlDocumentUploading();
      
    });

  }

  ControlDocumentUploading() {
    if (this.IsSkillsScholrship) {
      this.TraineeDoc.setValue('');
      this.TraineeDoc.setValidators([Validators.required]);
      this.TraineeDoc.updateValueAndValidity();
    } else if(this.SchemeCode=="STV"){
      this.TraineeDoc.setValue('');
      this.TraineeDoc.setValidators([Validators.required]);
      this.TraineeDoc.updateValueAndValidity();
    }
    else {
      this.TraineeDoc.setValue('');
      this.TraineeDoc.clearValidators();
      this.TraineeDoc.updateValueAndValidity();
    }
  }

  ControlProvince() {
    if (this.EDFScheme) {
      this.ProvinceID.setValue('');
      this.ProvinceID.enable();
      this.IsFieldDisabled = false;
    }
    else {
      this.ProvinceID.setValue(6);
      this.ProvinceID.disable();
      this.IsFieldDisabled = true;

    }
  }

  onEmploymentStatusBeforeTrainingChange(event) {
    if (event.value == 2 || event.value == 3) {
      this.TraineeIndividualIncomeID.setValue('');
      this.TraineeIndividualIncomeID.clearValidators();
      this.TraineeIndividualIncomeID.disable();
    }
    else {
      this.TraineeIndividualIncomeID.setValue('');
      this.TraineeIndividualIncomeID.setValidators([Validators.required]);
      this.TraineeIndividualIncomeID.updateValueAndValidity();
      this.TraineeIndividualIncomeID.enable();
    }
  }
  createTraineeProfileForm() {
    this.traineeProfileForm = this.fb.group({
      TraineeID: [0],
      TraineeCode: [{ value: '', disabled: true }, [Validators.required]],
      TraineeName: ['', [Validators.required, Validators.pattern('^[a-zA-Z \-\']+')]],
      TraineeCNIC: ['', [Validators.required, Validators.minLength(15), Validators.maxLength(15)]],
      FatherName: ['', [Validators.required, Validators.pattern('^[a-zA-Z \-\']+')]],
      GenderID: [{ value: '', disabled: true }, [Validators.required]],
      TradeID: [{ value: '', disabled: true }, [Validators.required]],
      SectionID: [{ value: '', disabled: true }, [Validators.required]],
      DateOfBirth: ['', [Validators.required]],
      CNICIssueDate: ['', [Validators.required]],
      IsDual: new FormControl({ value: false, disabled: true }),
      TSPID: new FormControl({ value: null, disabled: true }, [Validators.required]),
      ClassID: ['', [Validators.required]],
      ClassCode: new FormControl({ value: '', disabled: true }),
      TraineeDisabilityID: ['', [Validators.required]],
      EducationID: ['', [Validators.required]],
      ContactNumber1: ['', [Validators.required, Validators.minLength(12), Validators.maxLength(12)]],
      TraineeImg: ['', [Validators.required]],
      TraineeDoc: ['', [Validators.required]],
      TraineeEmail: ['', [Validators.email, Validators.required]],
      TraineeAge: ['', [Validators.required]],
      ReligionID: ['', [Validators.required]],
      VoucherHolder: new FormControl(false),
      // updated by sami 2025-01-03      
      // IsReferredByGuru: ['', [Validators.required]],
       IsReferredByGuru: [''],
      ReferralSourceID: ['', [Validators.required]],
      TraineeIndividualIncomeID: ['', [Validators.required]],
      HouseHoldIncomeID: ['', [Validators.required]],
      EmploymentStatusBeforeTrainingID: ['', [Validators.required]],
      GuardianNextToKinName: ['', [Validators.required, Validators.pattern('^[a-zA-Z \-\']+')]],
      GuardianNextToKinContactNo: ['', [Validators.required]],
      TraineeHouseNumber: ['', [Validators.required]],
      TraineeStreetMohalla: ['', [Validators.required]],
      TraineeMauzaTown: ['', [Validators.required]],
      ProvinceID: [{ value: '', disabled: true }, [Validators.required]],
      TraineeDistrictID: ['', [Validators.required]],
      TraineeTehsilID: ['', [Validators.required]],
      Undertaking: [false, Validators.requiredTrue],
      IsExtra: [false],
      IsSubmitted: [false],
      SchemeID: [0],
      //Temporary Address fields
      TemporaryResidence: [{ value: '', disabled: true }],
      TemporaryDistrict: [{ value: '', disabled: true }],
      TemporaryTehsil: [{ value: '', disabled: true }],
      //End
      AgeVerified: [false],
      DistrictVerified: [false],
      IsManual: [true],
      CNICImgNADRA: [''],
      CNICUnVerifiedReason: [''],
      AgeUnVerifiedReason: [''],
      ResidenceUnVerifiedReason: [''],
      CNICVerificationDate: [''],
      ResultStatusID: [3],
      IsMigrated: [false],
      ResultStatusChangeDate: [''],
      ResultStatusChangeReason: [''],
      /// Added By Rao ALi haider for International Placement
      Accounttitle: ['', [Validators.required]],
      BankName: ['', [Validators.required]],
      IBANNumber: ['', [Validators.required]]
    }, { updateOn: "change" });
    this.VoucherHolder.valueChanges.subscribe(checked => {
      if (checked) {
        const validators = [Validators.required];
        this.traineeProfileForm.addControl('VoucherNumber', this.fb.control('', validators));
        this.traineeProfileForm.addControl('VoucherOrganization', this.fb.control('', validators));
      } else {
        this.traineeProfileForm.removeControl('VoucherNumber');
        this.traineeProfileForm.removeControl('VoucherOrganization');
      }
      this.traineeProfileForm.updateValueAndValidity();
    })

    //this.IsReferredByGuru.valueChanges.subscribe(checked => {

    //  if (checked) {
    //    this.traineeProfileForm.addControl('GuruProfileID', this.fb.control('', [Validators.required]));
    //  } else {
    //    this.traineeProfileForm.removeControl('GuruProfileID');
    //  }
    //  this.traineeProfileForm.updateValueAndValidity();
    //})
    this.traineeProfileForm.get('IsReferredByGuru').valueChanges.subscribe((isReferred) => {
      if (isReferred) {
        this.traineeProfileForm.addControl(
          'GuruProfileID',
          this.fb.control('', [Validators.required])
        );
      } else {
        this.traineeProfileForm.removeControl('GuruProfileID');
      }
      this.traineeProfileForm.updateValueAndValidity();
    });
  }

 
  setFormIsDisabled(isDisabled: boolean, errMsg = '') {
    debugger;
    this.saveBtnTitle = "Save";
    //this.isFormDisabled = state == 'enabled' ? false : state == 'disabled' ? true : true;
    this.isFormDisabled = isDisabled;
    this.registrationError = errMsg;
    if (!this.isTspUser) {
      this.isFormDisabled = true;
    }
  }
  onSave() {
    this.save();
  }
  onSaveAndSubmit() {
    this.traineeProfileForm.patchValue({ IsSubmitted: true });
    this.save();
  }
  save() {
    
    // if (!this.traineeProfileForm.valid) {
    //   //debugger;
      
    //   this.http.ShowError("Something missing or invalid.", "Error");
    //   console.log(this.traineeProfileForm)
    //   return;
    // }

    if (!this.traineeProfileForm.valid) {
      //debugger;
      const invalidControls = [];
      for (const name in this.traineeProfileForm.controls) {
        if (this.traineeProfileForm.controls[name].invalid) {
          invalidControls.push(name);
        }
      }
      this.http.ShowError("Something missing or invalid in: " + invalidControls.join(', '), "Error");
      console.log(this.traineeProfileForm);
      return;
    }

    // this.working = true;
    //this.markAsExtraTrainee();
    if (this.TraineeID.value == 0) {
      ///Validate Allowed Trainee
      //let submittedTrainees = this.traineeProfileArray.filter(x => x.IsSubmitted == true).length;
      let savedTrainees = this.traineeProfileArray.length;
      if (savedTrainees >= this.totalAllowedTrineesPerClass) {
        this.http.ShowError(`Registraion Completed, Allowed only ${this.totalAllowedTrineesPerClass} trainee(s).`, "Error");
        return;
      }
      ///Mark As Extra
      if (savedTrainees >= this.traineesPerClass) {
        this.traineeProfileForm.patchValue({ IsExtra: true });
      }
    }
    ///Validate Age Of Trainee
    let item = this.traineeProfileForm.getRawValue();


    if(item.EmploymentStatusBeforeTrainingID ==2 || item.EmploymentStatusBeforeTrainingID==3){
      item.TraineeIndividualIncomeID=""
    }
    
    //item = this.trimStrings(item);
    //if (!this.isValidTraineeByAge(new Date(item.DateOfBirth))) {
    //  this.http.ShowError("Trainee's age is not valid for this class.", "Error");
    //  return;
    //}

    this.http.postJSON('api/TraineeProfile/Save', item).subscribe(
      (response: any) => {
        this.traineeProfileArray = JSON.parse(JSON.stringify(response));
        this.traineeProfile = new MatTableDataSource(response);
        this.traineeProfile.paginator = this.paginator;
        this.traineeProfile.sort = this.sort;
        //this.reset();
        //this.http.openSnackBar("Saved Successfully");
        let titleConfirm = 'Success';
        let messageConfirm = `Trainee profile ${item.IsSubmitted ? "submitted" : "saved"} successfully, Press 'Yes' for farther registration.`;

        this.http.confirm(titleConfirm, messageConfirm).subscribe(
          (isConfirm: Boolean) => {
            this.reset();
            if (!isConfirm) {
              if (sessionStorage.getItem('potentialTrainee')) {
                this.router.navigateByUrl(`/potential-trainees/potential-trainees-enrollment`);
              }
              else {
                this.tabGroup.selectedIndex = TabGroup.Trainees;
              }
            }
          })

        this.setDisplayedColumns();
      },
      (error) => {
        this.error = error.error;
        this.http.ShowError(error.error, 'error', 5000);
      });
  }
  
  trimStrings(obj) {
    return Object.keys(obj).reduce((acc, key) => {
      acc[key.trim()] = typeof obj[key] === 'string' ? obj[key]?.trim() : obj[key];
      return acc;
    }, Array.isArray(obj) ? [] : {});
  }
  reset() {
    this.saveBtnTitle = "Save";
    if (!this.traineeProfileForm.controls.IsManual.value) {
      Object.keys(this.traineeProfileForm.controls).forEach(key => {
        if (this.traineeProfileForm.get(key).disabled == true) {

        }
        else {
          this.traineeProfileForm.get(key).reset();
        }
        ;
      })
      console.log('No reset for diabled fields in case of DVV');
      return false;
    }
    else {
      this.createTraineeProfileForm();
      this.getSetDataByClass();
    }

    //this.traineeProfileForm.reset()
    //ngForm.resetForm();

    //this.createTraineeProfileForm();
    //this.getSetDataByClass();
  }
  toggleActive(row) {
    this.http.confirm().subscribe(result => {
      if (result) {
        this.http.postJSON('api/TraineeProfile/ActiveInActive', { 'TraineeID': row.TraineeID, 'InActive': row.InActive })
          .subscribe((d: any) => {
            this.http.openSnackBar(row.InActive == true ? environment.InActiveMSG.replace("${Name}", this.EnText) : environment.ActiveMSG.replace("${Name}", this.EnText));
          },
            (error) => {
              this.error = error.error;
              this.http.ShowError(error.error);
              row.InActive = !row.InActive;
            });
      }
      else {
        row.InActive = !row.InActive;
      }
    });
  }
  onEditTraineeClass() {
    this.saveBtnTitle = "Save";
    this.createTraineeProfileForm();

    this.getSetDataByClass();

  
  }
  onEditTrainee(row) {
    //this.registrationError = '';

    if (this.IsSkillsScholrship){
      this.getTraineeGuru(row.TraineeID)
    }
    
    // this.traineeGuruList.find(g=>g.TraineeID)
    if (sessionStorage.getItem('potentialTrainee')) {
      sessionStorage.removeItem('potentialTrainee')
    }

    this.CheckRegistrationCriteria(this.classId).subscribe(
      (response: any[]) => {
        this.traineeProfileForm.patchValue(row);
        
        if (this.IsSkillsScholrship && this.traineeGuruDetailList.length>0){
         const sTraineeGuru= this.traineeGuruDetailList.find(tg=>tg.TraineeID==row.TraineeID)
         if(sTraineeGuru){
          this.IsReferredByGuru.setValue(true)
          this.GuruProfileID.setValue(sTraineeGuru.GuruProfileID)
         }
          
        }
        if (!row.IsManual) {
          //this.traineeProfileForm.controls.IsManual.setValue(false);
          this.TraineeName.disable();
          this.FatherName.disable();
          this.GenderID.disable();
          this.TraineeCNIC.disable();
          this.DateOfBirth.disable();
          this.TraineeAge.disable();
          this.CNICIssueDate.disable();
        }

        this.tabGroup.selectedIndex = TabGroup.RegistrationForm;
        debugger;
        if (row.IsSubmitted) {
          this.setFormIsDisabled(true, 'This profile is Submitted so that not available for edit.');
        } else {
          if (response.length > 0) {
            if (response[0].ErrorTypeID == 5 && response[0].ErrorTypeName == 'DVV class') {
              this.setFormIsDisabled(false);
              this.saveBtnTitle = "Update";
            }
            else if (response[0].ErrorTypeID == 5 && response[0].ErrorTypeName == 'Skills Scholarship Program class') {
              this.setFormIsDisabled(false);
              this.TraineeCNIC.disable();
              this.TraineeName.disable();
              this.FatherName.disable();
              this.TraineeEmail.disable();
              this.DateOfBirth.disable();
              this.TraineeAge.disable();
              this.CNICIssueDate.disable();
              this.TraineeAge.disable();              
              this.ContactNumber1.disable();
              this.isProgramFieldDisabled = true;

              this.saveBtnTitle = "Update";
            }
            else
              this.setFormIsDisabled(true, response[0].Errormessage);
            // this.registrationError = response[0].Errormessage
          } else {
            this.setFormIsDisabled(false);
            this.saveBtnTitle = "Update";
          }
        }
      }
      , error => { })
    if (row.EmploymentStatusBeforeTrainingID == 2 || row.EmploymentStatusBeforeTrainingID == 3) {
      this.TraineeIndividualIncomeID.setValue('');
      this.TraineeIndividualIncomeID.clearValidators();
      this.TraineeIndividualIncomeID.disable();
    }
    else {
      this.TraineeIndividualIncomeID.setValue('');
      this.TraineeIndividualIncomeID.setValidators([Validators.required]);
      this.TraineeIndividualIncomeID.updateValueAndValidity();
      this.TraineeIndividualIncomeID.enable();
    }

    if (row.TraineeDistrictID == 0 && !row.IsManual) {
      row.ProvinceID = 6;
      row.TraineeDistrictID = row.TemporaryDistrict;
      row.TraineeTehsilID = row.TemporaryTehsil;
    }
    else {
      this.ControlProvince();
    }
    this.ControlDocumentUploading();

  
  }

  checkOnTraineeCNIC() {
    this.isEligibleTrainee()
  }

  checkOnTraineeEmail() {
    this.isEligibleTraineeEmail()
  }
  applyFilter(filterValue: string) {
    filterValue = filterValue.trim(); // Remove whitespace
    filterValue = filterValue.toLowerCase(); // Datasource defaults to lowercase matches
    this.traineeProfile.filter = filterValue;
  }
  //applyFilterClass(filterValue: string) {
  //  filterValue = filterValue.trim(); // Remove whitespace
  //  filterValue = filterValue.toLowerCase(); // Datasource defaults to lowercase matches
  //  this.Class.filter = filterValue;
  //}
  CheckRegistrationCriteria(classid: number) {
    return this.http.getJSON(`api/TraineeProfile/CheckRegistrationCriteria?classId=${classid}`);
  }
  traineeGuruList:any;
  getGuruProfiles() {
    const userID = this.http.getUserDetails().UserID;
    this.http.getJSON(`api/traineeGuruProfile/getTraineeGuru?UserID=${userID}`).subscribe(
      (response) => {
        this.traineeGuruList = response;
      },
      (error) => {
        console.error('Error fetching trainee guru data:', error);
      }
    );
  }
  traineeGuruDetailList:any=[]

  async getTraineeGuru(traineeID:number): Promise<void> {
    try {
      this.traineeGuruDetailList = await this.http.getJSON(`api/traineeGuruProfile/getTraineeGuruDetail?traineeID=${traineeID}`).toPromise();
    } catch (error) {
      console.error('Error fetching trainee guru data:', error);
    }
  }
  

  getGuruDetail(GuruDetail:any){
    if(GuruDetail==0){
      this.addTraineeGuru()
    }
  }
  

  addTraineeGuru() {
    const dialogRef = this.Dialog.open(TraineeGuruDialogComponent, {
      width: '80%',
      // height: '90%',
      data: [],
      disableClose: true,
    });
    dialogRef.afterClosed().subscribe(result => {
      this.GuruProfileID.setValue("")
      this.getGuruProfiles()
      if(this.traineeGuruList.length>0){
      this.GuruProfileID.setValue(this.traineeGuruList[0].GuruProfileID)
      }
    });
  }

  RemoveInternatonplacemntFields() { ///Hide the remove the validation of bank fields
    if (!this.IsInternationalPlacement) {
      this.BankName.setValue('');
      this.BankName.clearValidators();
      this.BankName.disable();

      this.IBANNumber.setValue('');
      this.IBANNumber.clearValidators();
      this.IBANNumber.disable();


      this.Accounttitle.setValue('');
      this.Accounttitle.clearValidators();
      this.Accounttitle.disable();
    }
  }

  getData() {
    this.http.getJSON(`api/TraineeProfile/GetData?OID=${this.http.OID.value}`).subscribe(
      (d: any) => {
        this.Gender = d.Genders;
        this.Trade = d.Trade;
        this.EducationTypes = d.EducationTypes;
        this.District = d.District;
        this.Tehsil = d.Tehsil;
        this.TempDistrict = d.TempDistrict;
        this.TempTehsil = d.TempTehsil;
        this.TraineeDisability = d.TraineeDisability;
        this.IncomeRange = d.IncomeRange;
        this.ReferralSource = d.ReferralSource;
        this.Sections = d.Sections;
        this.TSPDetail = d.TSPDetail;
        this.classesArray = d.Class;
        this.Religion = d.Religion;
        this.EmploymentStatus = d.EmploymentStatus;
        this.province = d.Province;
        this.TemporaryProvince = d.TemporaryProvince;
        if (this.paramClassId) {
          this.classId = parseInt(this.paramClassId);
          this.onEditTraineeClass();
        }
      }, error => this.error = error // error path
    );
  }
  SchemeCode:string=""
  getSetDataByClass() {
    if (this.isTspUser && !this.paramClassId) {
      this.isFormDisabled = true;
      return false;
    }

    //this.registrationError = '';
    this.http.getJSON('api/TraineeProfile/GetDataByClass/' + this.classId).subscribe(
      (response: any) => {
        let traineeProfileList = response.ListTraineeProfile;
        this.traineeProfileArray = JSON.parse(JSON.stringify(response.ListTraineeProfile));
        let checkRegistrationCriteria = response.CheckRegistrationCriteria;
debugger;
        if (checkRegistrationCriteria.length > 0) {
          //this.registrationError = checkRegistrationCriteria[0].ErrorMessage;
          this.setFormIsDisabled(true, checkRegistrationCriteria[0].ErrorMessage);
          //return;
        } else {
          this.setFormIsDisabled(false);
        }
        this.traineeClass = response.ClassModel;
        let orgConfigList = response.OrgConfigModel;

        //Object.assign(this.traineeProfileArray, response[2]);
        let inceptionReportList = response.InceptionReportModel || [];
        this.TraineeProfile_seqNextVal = response.NextTraineeCode;
        let scheme: SchemeModel = response.Schemes;

      

        ///
        if (inceptionReportList.length <= 0) {
          this.error = "Please submit inception report first, before registration.";
          //this.registrationError = this.error;
          //this.http.ShowError(this.error, "Error");
          this.setFormIsDisabled(true, this.error);// isFormDisabled = true;
          return;
        }
        ///
        if (orgConfigList.length <= 0) {
          this.error = "Please set 'Rules' of this class before registration.";
          //this.registrationError = this.error;
          // this.http.ShowError(this.error, "Error");
          this.setFormIsDisabled(true, this.error);//this.isFormDisabled = true;
          return;
        }
        ///
        if (!scheme) {
          this.error = "Not found scheme Data.";
          //this.registrationError = this.error;
          //this.http.ShowError(this.error, "Error");
          this.setFormIsDisabled(true, this.error);//this.isFormDisabled = true;
          return;
        }
        if (scheme.SchemeCode == 'STV' || scheme.SchemeCode == 'ST25' || scheme.SchemeCode == 'UNDP' || scheme.SchemeCode == 'SVN' || scheme.SchemeCode == 'SNV') {
          this.EDFScheme = true;
        }
        else if (scheme.FundingSourceID !== 12) //EDF Scheme
        {
          this.EDFScheme = false;
        }
        else {
          this.EDFScheme = true;
        }
        if (scheme.FundingCategoryID !== 20) {
          this.IsInternationalPlacement = false;
        }
        else {
          this.IsInternationalPlacement = true;
          this.RemoveInternatonplacemntFields();
        }
        if (scheme.ProgramTypeID !== 7) {  //Skills Scolarship
          this.IsSkillsScholrship = false;
        }
        else {
          this.getGuruProfiles();
          this.IsSkillsScholrship = true;
        }
        if (scheme.ProgramTypeID == 10) {  //Skills Scolarship Program
          this.IsSkillsScholrshipProgram = true;
        }
        else {
          this.IsSkillsScholrshipProgram = false;
        }

        this.TraineeCode.setValue(this.traineeClass.ClassCode + "-" + this.TraineeProfile_seqNextVal);
        this.TradeID.setValue(this.traineeClass.TradeID);
        this.TSPID.setValue(this.traineeClass.TSPID);
        this.ClassCode.setValue(this.traineeClass.ClassCode);
        this.ClassID.setValue(this.traineeClass.ClassID);
        this.SchemeID.setValue(this.traineeClass.SchemeID);
        this.SectionID.setValue(inceptionReportList[0].SectionID);
        this.traineesPerClass = this.traineeClass.TraineesPerClass;
        this.orgConfig = <IOrgConfig>orgConfigList[0];

        this.submittedTrainees = traineeProfileList.filter(x => x.IsSubmitted == true).length;
        this.savedTrainees = traineeProfileList.filter(x => x.IsSubmitted == false).length;

        let traineesPerClassThershold = this.orgConfig.TraineesPerClassThershold || 0;
        this.traineeThreshold = Math.ceil((this.traineesPerClass / 100) * traineesPerClassThershold);
        this.totalAllowedTrineesPerClass = this.traineesPerClass + this.traineeThreshold;

        this.classStartDate = this.traineeClass.StartDate;
        this.classEndDate = this.traineeClass.EndDate;

        //console.log(traineeProfileList)
        //Set Trainees
        this.traineeProfile.data = traineeProfileList.map(function (item, index) {
          item.Sr = index + 1;
          return item;
        });
        this.traineeProfile.paginator = this.paginator;
        this.traineeProfile.sort = this.sort;

        //Set OrgConfig & Its Related Form Controls
        this.toggleGenderState(this.orgConfig.EligibleGenderID)
        this.ControlProvince();
        this.ControlDocumentUploading();
        this.IsDual.setValue(this.orgConfig.DualRegistration);
        //BR Business Rule
        if (scheme.MinimumEducation == 25 || scheme.MinimumEducation == 26) {
          this.ddlEducationTypes = this.EducationTypes.filter(x => x.EducationTypeID <= scheme.MinimumEducation && x.EducationTypeID <= scheme.MaximumEducation || x.EducationTypeID == 25 || x.EducationTypeID == 26);
        }
        else {
          this.ddlEducationTypes = this.EducationTypes.filter(x => x.EducationTypeID >= scheme.MinimumEducation && x.EducationTypeID <= scheme.MaximumEducation);
        }
        //debugger;
        this.setDisplayedColumns();

        this.populatePotentialTraineeData();


        //BR Business Rule
        //let classStart = typeof (this.classStartDate) == "string" ? new Date(this.classStartDate) : this.classStartDate;
        //let today = new Date();
        //let openeing = new Date(classStart);
        //let closing = new Date(classStart);
        //openeing.setDate(classStart.getDate() - this.orgConfig.BracketDaysBefore);
        //closing.setDate(classStart.getDate() + this.orgConfig.BracketDaysAfter);
        //if (today >= openeing && today <= closing) {
        //  this.setFormIsDisabled(false);
        //  this.isClosedBracketDays = false;
        //} else {
        //  this.setFormIsDisabled(true);
        //  this.isClosedBracketDays = true;
        //  return;
        //}
        /////
        //let submittedTrainees = this.traineeProfileArray.filter(x => x.IsSubmitted == true).length;
        //if (submittedTrainees >= this.totalAllowedTrineesPerClass) {
        //  this.isCompletedAllowedTrainees = true;
        //  this.isFormDisabled = true;
        //  return;
        //} else {
        //  this.isCompletedAllowedTrainees = false;

        //}
      },
      (error) => this.http.ShowError(error.error + '\n' + error.message)
    );
  }
  toggleGenderState(genderID: Number) {
    switch (genderID) {
      case EnumGender.Both:
          if (this.IsSkillsScholrship) {
                this.ddlGender = this.Gender.filter(
                  x => x.GenderID == EnumGender.Male || x.GenderID == EnumGender.Female || x.GenderID == EnumGender.Transgender
                );
                this.GenderID.enable();
              } else {
                this.ddlGender = this.Gender.filter(
                  x => x.GenderID == EnumGender.Male || x.GenderID == EnumGender.Female
                );
                this.GenderID.enable();
        }
        break;
      case EnumGender._3way:
        this.ddlGender = this.Gender.filter(x => x.GenderID == EnumGender.Male || x.GenderID == EnumGender.Female || x.GenderID == EnumGender.Transgender);
        this.GenderID.enable()
        break;
      default:
        this.ddlGender = this.Gender.filter(x => x.GenderID == genderID);
        this.GenderID.disable()
        this.GenderID.setValue(this.orgConfig.EligibleGenderID);
        break;
    }
    if (this.isFormDisabled) {
      this.GenderID.disable();
    }
  }
  setDisplayedColumns() {
    //let anyExtra = this.traineeProfileArray.filter(x => x.IsSubmitted == true && x.IsExtra == true).length > 0 ? true : false;
    let anyExtra = this.traineeProfileArray.filter(x => x.IsExtra == true).length > 0 ? true : false;
    if (anyExtra) {
      this.hiddenSaveExtra = true;
      this.displayedColumns = ['Sr', 'Check', 'TraineeImg', 'TraineeCode', 'TraineeName', 'FatherName', 'TraineeCNIC', 'DateOfBirth', 'ContactNumber1', 'ClassCode', 'TradeName', 'SchemeName', 'TrainingAddressLocation', 'DistrictName', 'IsExtra', 'Action'];
    } else {
      this.hiddenSaveExtra = false;
      this.displayedColumns = ['Sr', 'Check', 'TraineeImg', 'TraineeCode', 'TraineeName', 'FatherName', 'TraineeCNIC', 'DateOfBirth', 'ContactNumber1', 'ClassCode', 'TradeName', 'SchemeName', 'TrainingAddressLocation', 'DistrictName', 'Action'];
    }
  }
  saveExtraTrainees() {
    //let submittedTrainees = this.traineeProfile.data.filter(x => x.IsSubmitted == true).length;
    //let extraTrainees = this.traineeProfile.data.filter(x => x.IsSubmitted == true && x.IsExtra == true).length;
    let submittedTrainees = this.traineeProfile.data.length;
    let extraTrainees = this.traineeProfile.data.filter(x => x.IsExtra == true).length;
    let allowedExtraTrainees = submittedTrainees - this.traineesPerClass;
    if (allowedExtraTrainees !== extraTrainees) {
      this.http.ShowError(`You must be mark as checked, ${allowedExtraTrainees} extra Trainee(s).`, null, 6000);
      return;
    }
    let differArray = this.arrDifference(this.traineeProfile.data, this.traineeProfileArray);
    this.http.postJSON('api/TraineeProfile/BatchUpdateExtraTrainees', JSON.stringify(differArray))
      .subscribe((d: any) => {
        this.traineeProfileArray = JSON.parse(JSON.stringify(this.traineeProfile.data));
        this.http.openSnackBar("Saved successfully");
      },
        error => this.http.ShowError(error.error + '\n' + error.message) // error path
      );
  }
  onChangeIsExtra() {
    if (this.arrDifference(this.traineeProfile.data, this.traineeProfileArray).length > 0) {
      this.enableSaveExtra = true;
    } else {
      this.enableSaveExtra = false;
    }
  }
  arrDifference(arrA, arrB) {
    return arrA.filter(item => arrB.find(x => x.TraineeID == item.TraineeID && x.IsExtra == item.IsExtra) == undefined)
  }
  isEligibleTrainee(): void {
    let values = this.traineeProfileForm.getRawValue();
    if (values.TraineeCNIC == '') {
      return;
    }
    //if (values.TraineeCNIC.length == 15) {
    let filter = `?traineeId=${values.TraineeID}&cnic=${values.TraineeCNIC}&classId=${values.ClassID}`
    this.http.getJSON(`api/TraineeProfile/isEligibleTrainee` + filter).subscribe(
      (data: any) => {
        //BR (Business Rule)
        if (!data.isValid) {
          this.TraineeCNIC.setErrors({ isValid: data.isValid, message: data.errMsg });
        }
        else {
          this.TraineeCNIC.setErrors(null);
          this.TraineeCNIC.setValidators([Validators.required, Validators.minLength(15), Validators.maxLength(15)]);
          this.TraineeCNIC.updateValueAndValidity();
        }
      }, (error) => {
        this.error = error // error path
      }
    );
    //}
  }
  //isEligibleTraineeEmail(): void {
  //  let values = this.traineeProfileForm.getRawValue();
  //  if (values.TraineeEmail == '') {
  //    return;
  //  }
  //  //if (values.TraineeCNIC.length == 15) {
  //  let filter = `?traineeId=${values.TraineeID}&email=${values.TraineeEmail}&classId=${values.ClassID}`
  //  this.http.getJSON(`api/TraineeProfile/isEligibleTraineeEmail` + filter).subscribe(
  //    (data: any) => {
  //      //BR (Business Rule)
  //      if (!data.isValid) {
  //        this.TraineeEmail.setErrors({ isValid: data.isValid, message: data.errMsg });
  //      }
  //      else {
  //        this.TraineeEmail.setErrors(null);
  //        this.TraineeEmail.setValidators([Validators.email, Validators.required]);
  //        this.TraineeEmail.updateValueAndValidity();
  //      }
  //    }, (error) => {
  //      this.error = error // error path
  //    }
  //  );
  //  //}
  //}
  isEligibleTraineeEmail(): void {
    let values = this.traineeProfileForm.getRawValue();

    if (values.TraineeEmail == '') {
      return;
    }

    const filter = `?traineeId=${values.TraineeID}&email=${values.TraineeEmail}&classId=${values.ClassID}`;

    this.http.fetchAndValidateTLD(values.TraineeEmail)
      .subscribe(
        (isValidTLD: boolean) => {
          if (!isValidTLD) {
            this.TraineeEmail.setErrors({ isValid: false, message: 'Invalid email address' });
            return;
          }

          // If TLD is valid, proceed with your existing validation
          this.http.getJSON(`api/TraineeProfile/isEligibleTraineeEmail` + filter).subscribe(
            (data: any) => {
              if (!data.isValid) {
                this.TraineeEmail.setErrors({ isValid: data.isValid, message: data.errMsg });
              } else {
                this.TraineeEmail.setErrors(null);
                this.TraineeEmail.setValidators([Validators.email, Validators.required]);
                this.TraineeEmail.updateValueAndValidity();
              }
            },
            (error) => {
              this.error = error; // Handle error
            }
          );
        },
        (error) => {
          this.error = 'Failed to fetch the TLD list'; // Handle fetch error
        }
      );
  }

  calculateAgeEligibility() {
    let dateOfBirth: Date = typeof (this.DateOfBirth.value) === 'string' ? new Date(this.DateOfBirth.value) : this.DateOfBirth.value;
    let classID: number = this.ClassID.value;
    this.http.postJSON('api/TraineeProfile/CalculateAgeEligibility', { DateOfBirth: dateOfBirth, ClassID: classID }).subscribe(
      (response: any) => {
        this.TraineeAge.setValue(response.age)
        if (response.errMsg != '') {
          this.TraineeAge.setErrors({ isValid: false, message: response.errMsg })
          this.TraineeAge.markAsTouched();
          //this.TraineeAge.updateValueAndValidity();
        } else {
          this.TraineeAge.setErrors(null);
          this.TraineeAge.setValidators([Validators.required]);
        }
      },
      (error) => {
        this.error = error.error;
        this.http.ShowError(error.error + '\n' + error.message);
      } // error path
    );
  }
  calculateAge() {
    const birthDate = new Date(this.DateOfBirth.value);
    if (!this.isValidTraineeByAge(birthDate)) {
      this.TraineeAge.setErrors({ isValid: false, message: "Trainee's age is not valid for this class. " })
      this.TraineeAge.markAsTouched();
    }
    else {
      this.TraineeAge.setErrors(null);
      this.TraineeAge.setValidators([Validators.required]);
      this.TraineeAge.updateValueAndValidity();
    }
  }
  isValidTraineeByAge(dateOfBirth: Date) {
    let birthDate: Date = typeof (dateOfBirth) === 'string' ? new Date(dateOfBirth) : dateOfBirth;
    let isValid: boolean = false;
    let classStartDate = typeof (this.classStartDate) === 'string' ? new Date(this.classStartDate) : this.classStartDate;
    let classEndDate = typeof (this.classEndDate) === 'string' ? new Date(this.classEndDate) : this.classEndDate;
    let ageOnClassStartDate = this.dateDifference(classStartDate, birthDate).years - 1;
    let ageOnClassEndDate = this.dateDifference(classEndDate, birthDate).years - 1;
    //BR (Business Rule)
    if (ageOnClassEndDate >= this.orgConfig.MinAge) {
      isValid = true;
      this.TraineeAge.setValue(ageOnClassEndDate)
      if (ageOnClassStartDate <= this.orgConfig.MaxAge) {
        isValid = true;
        this.TraineeAge.setValue(ageOnClassStartDate)
      } else {
        isValid = false;
        this.TraineeAge.setValue(ageOnClassStartDate)
      }
    } else {
      isValid = false;
      this.TraineeAge.setValue(ageOnClassEndDate)
    }
    return isValid;
  }
  dateDifference(maxDate: Date, minDate: Date) {
    //Convert to UTC
    let maxDateUTC = new Date(Date.UTC(maxDate.getUTCFullYear(), maxDate.getUTCMonth(), maxDate.getUTCDate()));
    let minDateUTC = new Date(Date.UTC(minDate.getUTCFullYear(), minDate.getUTCMonth(), minDate.getUTCDate()));
    ////---------Days------------////
    let days = maxDateUTC.getDate() - minDateUTC.getDate();
    if (days < 0) {
      minDateUTC.setMonth(minDateUTC.getMonth() - 1);
      days += this.daysInMonth(minDateUTC);
    }
    ////Exclude Start Day
    //--days;
    ////---------Months------------////
    let months = maxDateUTC.getMonth() - minDateUTC.getMonth();
    if (months < 0) {
      minDateUTC.setFullYear(minDateUTC.getFullYear() - 1);
      months += 12;
    }
    ////---------Years------------////
    let years = maxDateUTC.getFullYear() - minDateUTC.getFullYear();

    ////---------Appendix------------////
    //let yAppendix, mAppendix, dAppendix;
    //if (years > 1) yAppendix = "y(s) ";
    //else yAppendix = "y ";
    //if (months > 1) mAppendix = "m(s) ";
    //else mAppendix = "m ";
    //if (days > 1) dAppendix = "d ";
    //else dAppendix = "d(s) ";
    //return years + yAppendix + months + mAppendix + days + dAppendix;
    return { years: years, months: months, days: days }
  }
  daysInMonth(dateUTC: Date) {
    let monthStart: any = new Date(dateUTC.getFullYear(), dateUTC.getMonth(), 1);
    let monthEnd: any = new Date(dateUTC.getFullYear(), dateUTC.getMonth() + 1, 1);
    let monthLength = (monthEnd - monthStart) / (1000 * 60 * 60 * 24);
    return monthLength;
  }

  GetCurrentComplaintAttachements() {
    let ids = this.classId; //this.selection.selected.map(x => x.TraineeID).join(',')
    this.base64ToBlob(ids, ids);

  }
  public base64ToBlob(b64Data, ComplaintNo) {
    var elem = window.document.createElement('a');
    elem.href = b64Data
    elem.download = ComplaintNo;
    document.body.appendChild(elem);
    elem.click();
    document.body.removeChild(elem);
  }


  getTraineeProfileRegistration_Report() {
    let ids = this.selection.selected.map(x => x.TraineeID).join(',')
    this.http.getReportJSON('TraineeProfile/GetRegistrationReport/' + ids).subscribe(
      (data: any) => {
        let file = this.base64ToFile(data.Response);
        const fileURL = window.URL.createObjectURL(file);
        // if (window.navigator.msSaveOrOpenBlob) {
        //   window.navigator.msSaveOrOpenBlob(file, fileURL.split(':')[1] + '.pdf');
        // } else {
          //   window.open(fileURL);
          // }
            window.open(fileURL);

      },
      (error) => {
        this.error = error // error path
        this.http.ShowError(error.error + '\n' + error.message, '', 5000);
      })
  }
  base64ToFile(res: string, mimeType: string = 'application/pdf') {
    let byteCharacters = atob(res);

    let byteNumbers = new Array(byteCharacters.length);

    for (var i = 0; i < byteCharacters.length; i++)
      byteNumbers[i] = byteCharacters.charCodeAt(i);

    let byteArray = new Uint8Array(byteNumbers);
    let file = new Blob([byteArray], { type: 'application/pdf' });
    //return this.domSanitizer.bypassSecurityTrustResourceUrl(URL.createObjectURL(file));
    return file;
  }
  dateFilter = (d: Date | null): boolean => {
    const date = (d || new Date());
    // Prevent after current date selection .
    return date <= new Date();
  }
  deleteDraftTrainee(traineeID: number) {
    let titleConfirm = "Confirmation";
    let messageConfirm = "Do you want to forever delete this draft Trainee?";
    this.http.confirm(titleConfirm, messageConfirm).subscribe(
      (isConfirm: Boolean) => {
        if (isConfirm) {
          this.http.getJSON(`api/TraineeProfile/DeleteDraftTrainee?TraineeID=${traineeID}`).subscribe(
            (d: any) => {
              if (d) {
                this.reset()
                //let updatedList = this.traineeProfileArray.filter(x => x.TraineeID != traineeID)
                //this.traineeProfileArray = JSON.parse(JSON.stringify(updatedList));
                //this.traineeProfile = new MatTableDataSource(updatedList);
                //this.reloadPage();
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

  EmptyCtrl() {
    this.SearchCls.setValue('');
  }


  populatePotentialTraineeData() {
    if (sessionStorage.getItem('potentialTrainee')) {
      var potentialTraineeObj = JSON.parse(sessionStorage.getItem('potentialTrainee'));
      this.TraineeName.setValue(potentialTraineeObj.TraineeName);
      this.TraineeEmail.setValue(potentialTraineeObj.TraineeEmail);
      this.TraineeCNIC.setValue(potentialTraineeObj.TraineeCNIC);
      this.ContactNumber1.setValue(potentialTraineeObj.TraineePhone);
    }

  }

  ngOnDestroy() {
    if (sessionStorage.getItem('potentialTrainee')) {
      sessionStorage.removeItem('potentialTrainee')
    }
  }

  ////---------Getter-----S----////
  get TraineeID() { return this.traineeProfileForm.get("TraineeID"); }
  get TraineeCode() { return this.traineeProfileForm.get("TraineeCode"); }
  get TraineeName() { return this.traineeProfileForm.get("TraineeName"); }
  get TraineeCNIC() { return this.traineeProfileForm.get("TraineeCNIC"); }
  get FatherName() { return this.traineeProfileForm.get("FatherName"); }
  get GenderID() { return this.traineeProfileForm.get("GenderID"); }
  get TradeID() { return this.traineeProfileForm.get("TradeID"); }
  get SectionID() { return this.traineeProfileForm.get("SectionID"); }
  get DateOfBirth() { return this.traineeProfileForm.get("DateOfBirth"); }
  get CNICIssueDate() { return this.traineeProfileForm.get("CNICIssueDate"); }
  get IsDual() { return this.traineeProfileForm.get("IsDual"); }
  get SchemeID() { return this.traineeProfileForm.get("SchemeID"); }
  get TSPID() { return this.traineeProfileForm.get("TSPID"); }
  get ClassID() { return this.traineeProfileForm.get("ClassID"); }
  get ClassCode() { return this.traineeProfileForm.get("ClassCode"); }
  get TraineeDisabilityID() { return this.traineeProfileForm.get("TraineeDisabilityID"); }
  get EducationID() { return this.traineeProfileForm.get("EducationID"); }
  get ContactNumber1() { return this.traineeProfileForm.get("ContactNumber1"); }
  get CNICVerified() { return this.traineeProfileForm.get("CNICVerified"); }
  get TraineeImg() { return this.traineeProfileForm.get("TraineeImg"); }
  get CNICImgNADRA() { return this.traineeProfileForm.get("CNICImgNADRA"); }
  get TraineeDoc() { return this.traineeProfileForm.get("TraineeDoc"); } //Trainee Document required
  get TraineeAge() { return this.traineeProfileForm.get("TraineeAge"); }
  get ReligionID() { return this.traineeProfileForm.get("ReligionID"); }
  get VoucherHolder() { return this.traineeProfileForm.get("VoucherHolder"); }

  get IsReferredByGuru() { return this.traineeProfileForm.get("IsReferredByGuru"); }
  get GuruProfileID() { return this.traineeProfileForm.get("GuruProfileID"); }


  get VoucherNumber() { return this.traineeProfileForm.get("VoucherNumber"); }
  get VoucherOrganization() { return this.traineeProfileForm.get("VoucherOrganization"); }
  get ReferralSourceID() { return this.traineeProfileForm.get("ReferralSourceID"); }
  get TraineeIndividualIncomeID() { return this.traineeProfileForm.get("TraineeIndividualIncomeID"); }
  get HouseHoldIncomeID() { return this.traineeProfileForm.get("HouseHoldIncomeID"); }
  get EmploymentStatusBeforeTrainingID() { return this.traineeProfileForm.get("EmploymentStatusBeforeTrainingID"); }
  get Undertaking() { return this.traineeProfileForm.get("Undertaking"); }
  get GuardianNextToKinName() { return this.traineeProfileForm.get("GuardianNextToKinName"); }
  get GuardianNextToKinContactNo() { return this.traineeProfileForm.get("GuardianNextToKinContactNo"); }
  get TraineeHouseNumber() { return this.traineeProfileForm.get("TraineeHouseNumber"); }
  get TraineeStreetMohalla() { return this.traineeProfileForm.get("TraineeStreetMohalla"); }
  get TemporaryResidence() { return this.traineeProfileForm.get("TemporaryResidence"); }
  get TraineeMauzaTown() { return this.traineeProfileForm.get("TraineeMauzaTown"); }
  get ProvinceID() { return this.traineeProfileForm.get("ProvinceID"); }
  get TraineeDistrictID() { return this.traineeProfileForm.get("TraineeDistrictID"); }
  get TraineeTehsilID() { return this.traineeProfileForm.get("TraineeTehsilID"); }
  get TemporaryDistrict() { return this.traineeProfileForm.get("TemporaryDistrict"); }
  get TemporaryTehsil() { return this.traineeProfileForm.get("TemporaryTehsil"); }
  get AgeVerified() { return this.traineeProfileForm.get("AgeVerified"); }
  get DistrictVerified() { return this.traineeProfileForm.get("DistrictVerified"); }
  get TraineeVerified() { return this.traineeProfileForm.get("TraineeVerified"); }
  get TraineeEmail() { return this.traineeProfileForm.get("TraineeEmail"); }
  get Accounttitle() { return this.traineeProfileForm.get("Accounttitle"); }
  get BankName() { return this.traineeProfileForm.get("BankName"); }
  get IBANNumber() { return this.traineeProfileForm.get("IBANNumber"); }

  
  //get CNICImg() { return this.traineeProfileForm.get("CNICImg"); }
  ////----Getter----E-----////

}
enum TabGroup {
  RegistrationForm = 0,
  Trainees = 1
}


