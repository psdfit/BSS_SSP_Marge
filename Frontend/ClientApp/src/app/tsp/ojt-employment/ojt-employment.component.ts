/* **** Aamer Rehman Malik *****/
import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, FormGroupDirective, NgForm, ValidationErrors, Validators, ValidatorFn } from '@angular/forms';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ActivatedRoute } from '@angular/router';
import { EmploymentStatuses } from 'src/app/shared/Enumerations';
import { environment } from '../../../environments/environment';
import { CommonSrvService } from '../../common-srv.service';
import { UserRightsModel } from '../../master-data/users/users.component';
import { DomSanitizer } from '@angular/platform-browser';
import { animate, trigger, state, transition, style } from '@angular/animations';
import { FormControl } from '@angular/forms';

const MyValidator: ValidatorFn = (fg: FormGroup) => {
  const PlacementTypeID = fg.get('PlacementTypeID').value;
  const VerificationMethodId = fg.get('VerificationMethodId').value;
  if (fg.get('EmploymentStatus').value == 'Employed') {
    if (PlacementTypeID == 1 && VerificationMethodId != 9) {
      fg.get('PlatformName').clearValidators();
      fg.get('NameofTraineeStorePage').clearValidators();
      fg.get('LinkofTraineeStorePage').clearValidators();
    }
    else if (PlacementTypeID == 2 && VerificationMethodId == 7) {
      fg.get('FilePath').clearValidators();
      fg.get('PlatformName').clearValidators();
      fg.get('NameofTraineeStorePage').clearValidators();
      fg.get('LinkofTraineeStorePage').clearValidators();
    }
    else {
      if (PlacementTypeID == 2 && VerificationMethodId == 6) {
        return fg.get('FilePath').value != "" || fg.get('EOBI').value != "" ? null : { fileoeobi: true };
      }
      else
        return fg.get('FilePath').value != "" ? null : { file: true };
    }

  }
  else
    fg.get('FilePath').clearValidators();
};
@Component({
  selector: 'hrapp-ojt',
  templateUrl: './ojt-employment.component.html',
  styleUrls: ['./ojt-employment.component.scss'],
  animations: [
    trigger('detailExpand', [
      state('collapsed', style({ height: '0px', minHeight: '0' })),
      state('expanded', style({ height: '*' })),
      transition('expanded <=> collapsed', animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)')),
    ]),
  ],
})

export class OJTEmpComponent implements OnInit {
  showField: boolean = false;
  trineeProfile: any;
  traineeEmploymentForm: FormGroup;
  ExcelFileForm: FormGroup;
  traineeEmployment: any;
  SchmeFundingSource: any;
  ProgramType: any;
  displayedColumns = ['TraineeID', 'TraineeCode', 'EmploymentStatus',
    //'Designation', 'Department', 'EmploymentDuration',
    'OfficeContactNo', 'EmploymentAddress', 'UploadedFile',
    'Salary',
    'ViewFile',
    //'expandedDetail',
    //'SupervisorName',
    'Action'];//, 'SupervisorContact', 'EmploymentStartDate', 'EmploymentStatus', 'EmploymentType', 'EmployerName', 'EmployerBusinessType', 'EmploymentAddress', 'District', 'EmploymentTehsil', 'EmploymentTiming', 'IsTSP', 'OldID', 'EmploymentTehsilOld', 'OfficeContactNo', 'IsMigrated', 'TraineeName', "Action"];
  ClassDrp: any;
  TraineeProfileDrp: any;
  employmentFile: any;
  PlacementTypeDrp: any;
  PlacementStatusDrp: any;
  VerificationMethods: any;
  VerificationMethodsDrp: any;
  DistrictDrp: any;
  TehsilDrp: any;
  paramClassId: "0";
  paramTraineeId: "0";
  traineeID: number;
  classID: number;
  traineeCode: string;
  contactNumber: string;
  designation: string;
  department: string;
  employmentDuration: number;
  salary: number;
  //NameofTraineeStorePage: any;
  //PlatformName: any;
  //LinkofTraineeStorePage: any;
  supervisorName: string;
  supervisorContact: string;
  employmentStartDate: Date;
  employmentStartLimit: Date;
  employmentStatus: number;
  employmentType: number;
  employerName: string;
  employerBusinessType: string;
  employmentAddress: string;
  district: number;
  employmentTehsil: number;
  timeFrom: string;
  timeTo: string;
  employmentTiming: string;
  isTSP: Boolean;
  oldID: string;
  employmentTehsilOld: string;
  officeContactNo: string;
  employerNTN: string;
  isMigrated: Boolean;
  traineeName: string;
  formrights: UserRightsModel;
  EnText: string = " On Job Trainee Employment Form";
  title: string; savebtn: string = "Save"; working: boolean; error: string;
  EmploymentData: any;
  OrgConfig: any;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild("ngForm") ngFrom: NgForm;
  constructor(private fb: FormBuilder, public domSanitizer: DomSanitizer, private ComSrv: CommonSrvService, private route: ActivatedRoute) {
    this.GenerateForm();
    this.ExcelFileForm = this.fb.group({
      ExcelField: ['', Validators.required]
    });
    route.params.subscribe(
      params => {
        //console.log(params);
        this.paramClassId = params["classid"];
        this.paramTraineeId = params["traineeid"];
        //console.log(this.paramClassId + "," + this.paramTraineeId)
        if (this.paramClassId && this.paramTraineeId) {
          this.TraineeID.setValue(this.paramTraineeId);
          this.ClassID.setValue(this.paramClassId);
          //this.traineeEmploymentForm.controls['TraineeID'].disable();
          //this.traineeEmploymentForm.controls['ClassID'].disable();
        }
      });
    this.PlacementTypeID.valueChanges.subscribe((v) => {
      this.setFileType();
      if (v == 1) {
        this.Designation.disable();
        this.Department.disable();
        this.EmploymentDuration.disable();
        this.SupervisorName.disable();
        this.SupervisorContact.disable();
        this.EmploymentStartDate.disable();
        this.EmployerName.disable();
        this.EmployerNTN.disable();
        this.EmployerBusinessType.disable();
        this.TimeFrom.disable();
        this.TimeTo.disable();
      }
      if (v == 2) {
        this.Designation.enable();
        this.Department.enable();
        this.EmploymentDuration.enable();
        this.SupervisorName.enable();
        this.SupervisorContact.enable();
        this.EmploymentStartDate.enable();
        this.EmployerName.enable();
        this.EmployerNTN.enable();
        this.EmployerBusinessType.enable();
        this.TimeFrom.enable();
        this.TimeTo.enable();
      }
      //else {
      //  this.Designation.enable();
      //  this.Department.enable();
      //  this.EmploymentDuration.enable();
      //  this.SupervisorName.enable();
      //  this.SupervisorContact.enable();
      //  this.EmploymentStartDate.enable();
      //  this.EmployerName.enable();
      //  this.EmployerBusinessType.enable();
      //  this.TimeFrom.enable();
      //  this.TimeTo.enable();
      //}
    });
    this.VerificationMethodId.valueChanges.subscribe((v) => {
      this.setFileType();
    });
    this.EmploymentStatus.valueChanges.subscribe((v) => {
      if (v == 'Employed') {
        //this.getData();
        this.radioChange(2);
        this.PlatformName.enable();
        this.NameofTraineeStorePage.enable();
        this.LinkofTraineeStorePage.enable();
        this.Department.enable();
        this.EmploymentDuration.enable();
        this.Salary.enable();
        this.SupervisorName.enable();
        this.SupervisorContact.enable();
        this.EmploymentStartDate.enable();
        this.TimeTo.enable();
        this.TimeFrom.enable();
        this.VerificationMethodId.enable();
        this.PlacementTypeID.enable();
        this.EmployerName.enable();
        this.EmployerNTN.enable();
        this.EmployerBusinessType.enable();
        this.EmploymentAddress.enable();
        this.District.enable();
        this.EmploymentTehsil.enable();
        this.EmploymentTiming.enable();
        this.OfficeContactNo.enable();
        this.Designation.enable();

        this.FilePath.enable();

      }
      else {

        this.PlacementTypeID.setValue('');

        this.PlatformName.disable();
        this.NameofTraineeStorePage.disable();
        this.LinkofTraineeStorePage.disable();

        this.Designation.disable();
        this.Department.disable();
        this.EmploymentDuration.disable();
        this.Salary.disable();
        this.SupervisorName.disable();
        this.SupervisorContact.disable();
        this.EmploymentStartDate.disable();
        this.TimeTo.disable();
        this.TimeFrom.disable();
        this.VerificationMethodId.disable();
        this.PlacementTypeID.disable();
        this.FilePath.disable();
        this.EmployerName.disable();
        this.EmployerNTN.disable();
        this.EmployerBusinessType.disable();
        this.EmploymentAddress.disable();
        this.District.disable();
        this.EmploymentTehsil.disable();
        this.EmploymentTiming.disable();
        this.OfficeContactNo.disable();

        //Null Fields
        this.PlacementTypeID.setValue('');
        this.PlatformName.setValue('');
        this.NameofTraineeStorePage.setValue('');
        this.LinkofTraineeStorePage.setValue('');

        this.Designation.setValue('');
        this.Department.setValue('');
        this.EmploymentDuration.setValue('');
        this.Salary.setValue('');
        this.SupervisorName.setValue('');
        this.SupervisorContact.setValue('');
        this.EmploymentStartDate.setValue('');
        this.TimeTo.setValue('');
        this.TimeFrom.setValue('');
        this.VerificationMethodId.setValue('');
        this.PlacementTypeID.setValue('');
        this.FilePath.setValue('');
        this.EmployerName.setValue('');
        this.EmployerNTN.setValue('');
        this.EmployerBusinessType.setValue('');
        this.EmploymentAddress.setValue('');
        this.District.setValue('');
        this.EmploymentTehsil.setValue('');
        this.EmploymentTiming.setValue('');
        this.OfficeContactNo.setValue('');

      }
      for (const field in this.traineeEmploymentForm.controls) { // 'field' is a string
        if (field != 'EmploymentStatus')
          this.traineeEmploymentForm.get(field).updateValueAndValidity(); // 'control' is a FormControl  

      }

      this.traineeEmploymentForm.updateValueAndValidity();
    });

  }
  ngOnInit(): void {
    this.ComSrv.setTitle("Employment Form");
    this.traineeEmployment = new MatTableDataSource([]);
    this.formrights = this.ComSrv.getFormRights();
    this.getData();
  }

  GenerateForm() {
    this.traineeEmploymentForm = this.fb.group({
      PlacementID: 0,
      VerificationMethodId: ['', [Validators.required]],
      TraineeID: [0, [Validators.required]],
      ClassID: [{ value: '', disabled: true }, [Validators.required]],
      Designation: ['', [Validators.required]],
      Department: ['', [Validators.required]],
      EmploymentDuration: ['', [Validators.required]],
      Salary: ['', [Validators.required]],
      SupervisorName: ['', [Validators.required]],
      SupervisorContact: ['', [Validators.required]],
      EmploymentStartDate: ['', [Validators.required]],
      EmploymentStatus: ['', [Validators.required]],

      EmployerName: ['', [Validators.required]],
      EmployerNTN: ['', [Validators.required]],
      EmployerBusinessType: [''],
      EmploymentAddress: ['', [Validators.required]],
      District: ['', [Validators.required]],
      EmploymentTehsil: ['', [Validators.required]],
      TimeFrom: [''],
      TimeTo: ['', [Validators.required]],
      EmploymentTiming: ['', [Validators.required]],
      IsTSP: false,
      OldID: "",
      EmploymentTehsilOld: [''],
      OfficeContactNo: ['', [Validators.required]],
      IsMigrated: false,
      TraineeName: [{ value: '', disabled: true }, [Validators.required]],
      TraineeCode: [{ value: '', disabled: true }, [Validators.required]],
      ClassCode: [{ value: '', disabled: true }, [Validators.required]],
      ContactNumber: [{ value: '', disabled: true }, [Validators.required]],
      PlacementTypeID: ['2', [Validators.required]],
      PlatformName: ['', [Validators.required]],
      NameofTraineeStorePage: ['', [Validators.required]],
      LinkofTraineeStorePage: ['', [Validators.required]],
      EOBI: [''],
      FileType: [''],
      FileName: [''],
      FilePath: ['', Validators.required]
    }, { updateOn: "change", validators: MyValidator });


  }

  reset(myform: FormGroupDirective) {
    this.traineeEmploymentForm.reset();
    myform.reset();
    this.PlacementID.setValue(0);
    this.title = "Add New";
    this.savebtn = "Save";
  }
  DeadlineEnd: boolean = false;
  EmploymentSubmited: boolean = false;
  getData() {
    this.ComSrv.postJSON('api/TSPEmployment/GetDataForEmploymentOJT', { "ClassID": this.paramClassId, "TraineeID": this.paramTraineeId }).subscribe((d: any) => {
      console.log(d);
      this.ClassDrp = d.Class;
      this.TraineeProfileDrp = d.TraineeProfile;
      this.PlacementTypeDrp = d.PlacementTypes;
      this.PlacementStatusDrp = d.PlacementStatus;
      this.VerificationMethods = d.VerificationMethods;
      this.DistrictDrp = d.District;
      this.TehsilDrp = d.Tehsil;
      this.EmploymentData = d.EmploymentData;
      this.OrgConfig = d.OrgConfig;
      this.EmploymentSubmited = d.EmploymentSubmited;
      if (d.DeadlineStatus == "Date Passed") {
        this.ComSrv.ShowError("Can't submit the request. Deadline end", "Error");
        this.DeadlineEnd = true;
      }

      this.SchmeFundingSource = this.ClassDrp.FundingCategoryID;
      this.ProgramType = this.ClassDrp.ProgramTypeID;
      if (this.SchmeFundingSource == 14 && this.ProgramType == 1) {
        this.showField = true;
      }

      if (d.EmploymentData) {
        this.traineeEmployment = d.EmploymentData;
      }
      this.trineeProfile = d.TraineeProfile;
      if (this.trineeProfile) {
        this.traineeEmploymentForm.patchValue({
          TraineeID: this.trineeProfile.TraineeID,
          TraineeName: this.trineeProfile.TraineeName,
          TraineeCode: this.trineeProfile.TraineeCode,
          ClassCode: this.trineeProfile.ClassCode,
          ContactNumber: this.trineeProfile.ContactNumber,
          ClassID: this.trineeProfile.ClassID,
          PlacementID: this.trineeProfile.PlacementID,
          VerificationMethodId: this.trineeProfile.VerificationMethodId,
          PlatformName: this.trineeProfile.PlatformName,
          NameofTraineeStorePage: this.trineeProfile.NameofTraineeStorePage,
          LinkofTraineeStorePage: this.trineeProfile.LinkofTraineeStorePage,
          Designation: this.trineeProfile.Designation,
          Department: this.trineeProfile.Department,
          EmploymentDuration: this.trineeProfile.EmploymentDuration,
          Salary: this.trineeProfile.Salary,
          SupervisorName: this.trineeProfile.SupervisorName,
          SupervisorContact: this.trineeProfile.SupervisorContact,
          EmploymentStartDate: this.trineeProfile.EmploymentStartDate,
          EmploymentStatus: this.trineeProfile.EmploymentStatus,
          EmployerName: this.trineeProfile.EmployerName,
          EmployerNTN: this.trineeProfile.EmployerNTN,
          EmployerBusinessType: this.trineeProfile.EmployerBusinessType,
          EmploymentAddress: this.trineeProfile.EmploymentAddress,
          District: this.trineeProfile.District,
          EmploymentTehsil: this.trineeProfile.EmploymentTehsil,
          TimeFrom: this.trineeProfile.EmploymentTiming ? this.trineeProfile.EmploymentTiming.split(" to ")[0] : "",
          TimeTo: this.trineeProfile.EmploymentTiming ? this.trineeProfile.EmploymentTiming.split(" to ")[1] : "",
          //EmploymentTiming: ['', [Validators.required]],
          IsTSP: this.trineeProfile.IsTSP,
          OfficeContactNo: this.trineeProfile.OfficeContactNo,
          IsMigrated: this.trineeProfile.IsMigrated,
          PlacementTypeID: this.trineeProfile.PlacementTypeID,
          EOBI: this.trineeProfile.EOBI,
          FileType: this.trineeProfile.FileType,
          FileName: this.trineeProfile.FileName,
          FilePath: this.trineeProfile.FilePath
        });
        this.fileType = this.trineeProfile.FileType;
        const endDate = new Date(this.trineeProfile.EndDate);
        this.employmentStartLimit = new Date(endDate.setDate(endDate.getDate() - 9));

        if (this.trineeProfile.PlacementTypeID) {
          this.VerificationMethodsDrp = this.VerificationMethods.filter(x => x.PlacementTypeID == this.trineeProfile.PlacementTypeID);

        }
      }
    }, error => this.error = error // error path
    );
  }


  getTraineesEmploymentFile(row) {
    var traineeId = row.TraineeID;
    var classId = row.ClassID;
    this.ComSrv.postJSON('api/TSPEmployment/GetEmploymentDataByTraineeIDOJT', { "ClassID": classId, "TraineeID": traineeId }).subscribe((d: any) => {
      row.employmentFile = d[0];
      this.employmentFile = d[0];
      row.FPath = this.employmentFile['FilePath'];
      row.FPath = this.employmentFile.FilePath;
      row.TName = d[0].TraineeName;
      row.VMType = d[0].VerificationMethodType;
      row.VMId = 7;
    }, error => this.error = error // error path
    );
  }

  toggleEdit(row) {
    this.VerificationMethodsDrp = this.VerificationMethods;
    if (row.PlacementTypeID == 1) {
      this.PlacementID.setValue(row.PlacementID);
      this.TraineeID.setValue(row.TraineeID);
      this.ClassID.setValue(row.ClassID);
      this.Salary.setValue(row.Salary);
      this.EmploymentStatus.setValue(row.EmploymentStatus);
      this.EmploymentAddress.setValue(row.EmploymentAddress);
      this.District.setValue(row.District);
      this.EmploymentTehsil.setValue(row.EmploymentTehsil);
      this.EmploymentTiming.setValue(row.EmploymentTiming);
      this.TimeFrom.setValue(row.EmploymentTiming.split(" to ")[0]);
      this.TimeTo.setValue(row.EmploymentTiming.split(" to ")[1]);

      this.PlatformName.setValue(row.PlatformName);
      this.NameofTraineeStorePage.setValue(row.NameofTraineeStorePage);
      this.LinkofTraineeStorePage.setValue(row.LinkofTraineeStorePage);

      this.OfficeContactNo.setValue(row.OfficeContactNo);
      this.TraineeName.setValue(row.TraineeName);
      this.TraineeCode.setValue(row.TraineeCode);
      this.ClassCode.setValue(row.ClassCode);
      this.ContactNumber.setValue(row.ContactNumber);
      this.PlacementTypeID.setValue(row.PlacementTypeID);
      this.VerificationMethodId.setValue(row.VerificationMethodId);
      this.Designation.reset();
      this.Department.reset();
      this.EmploymentDuration.reset();
      this.SupervisorName.reset();
      this.SupervisorContact.reset();
      this.EmploymentStartDate.reset();
      this.EmployerName.reset();
      this.EmployerNTN.reset();
      this.EmployerBusinessType.reset();
      this.TimeFrom.reset();
      this.TimeTo.reset();
      this.TimeFrom.disable();
      this.TimeTo.disable();
      this.Designation.disable();
      this.Department.disable();
      this.EmploymentDuration.disable();
      this.SupervisorName.disable();
      this.SupervisorContact.disable();
      this.EmploymentStartDate.disable();
      this.EmployerName.disable();
      this.EmployerNTN.disable();
      this.EmployerBusinessType.disable();
    }
    else {
      this.Designation.reset();
      this.Department.reset();
      this.EmploymentDuration.reset();
      this.SupervisorName.reset();
      this.SupervisorContact.reset();
      this.EmploymentStartDate.reset();
      this.EmployerName.reset();
      this.EmployerNTN.reset();
      this.EmployerBusinessType.reset();
      this.TimeFrom.reset();
      this.TimeTo.reset();

      this.PlatformName.reset();
      this.NameofTraineeStorePage.reset();
      this.LinkofTraineeStorePage.reset();

      this.PlacementID.setValue(row.PlacementID);
      this.TraineeID.setValue(row.TraineeID);
      this.ClassID.setValue(row.ClassID);
      this.Designation.setValue(row.Designation);
      this.Department.setValue(row.Department);
      this.EmploymentDuration.setValue(row.EmploymentDuration);
      this.Salary.setValue(row.Salary);
      this.SupervisorName.setValue(row.SupervisorName);
      this.SupervisorContact.setValue(row.SupervisorContact);
      this.EmploymentStartDate.setValue(row.EmploymentStartDate);
      this.EmploymentStatus.setValue(row.EmploymentStatus);
      // this.EmploymentType.setValue(row.EmploymentType);
      this.EmployerName.setValue(row.EmployerName);
      this.EmployerNTN.setValue(row.EmployerNTN);
      this.EmployerBusinessType.setValue(row.EmployerBusinessType);
      this.EmploymentAddress.setValue(row.EmploymentAddress);
      this.District.setValue(row.District);
      this.EmploymentTehsil.setValue(row.EmploymentTehsil);
      this.EmploymentTiming.setValue(row.EmploymentTiming);
      this.TimeFrom.setValue(row.EmploymentTiming.split(" to ")[0]);
      this.TimeTo.setValue(row.EmploymentTiming.split(" to ")[1]);
      this.PlacementTypeID.setValue(row.PlacementTypeID);
      this.VerificationMethodId.setValue(row.VerificationMethodId);

      this.OfficeContactNo.setValue(row.OfficeContactNo);

      this.TraineeName.setValue(row.TraineeName);
      this.TraineeCode.setValue(row.TraineeCode);
      this.ClassCode.setValue(row.ClassCode);
      this.ContactNumber.setValue(row.ContactNumber);

      this.Designation.enable();
      this.Department.enable();
      this.EmploymentDuration.enable();
      this.SupervisorName.enable();
      this.SupervisorContact.enable();
      this.EmploymentStartDate.enable();
      this.EmployerName.enable();
      this.EmployerNTN.enable();
      this.EmployerBusinessType.enable();
      this.TimeFrom.enable();
      this.TimeTo.enable();
    }

    this.title = "Update ";
    this.savebtn = "Save ";
  }
  toggleActive(row) {
    this.ComSrv.confirm().subscribe(result => {
      if (result) {
        this.ComSrv.postJSON('api/PlacementFormE/ActiveInActive', { 'PlacementID': row.PlacementID, 'InActive': row.InActive })
          .subscribe((d: any) => {
            this.ComSrv.openSnackBar(row.InActive == true ? environment.InActiveMSG.replace("${Name}", this.EnText) : environment.ActiveMSG.replace("${Name}", this.EnText));
            // this.placementforme =new MatTableDataSource(d);
          },
            error => this.ComSrv.ShowError(this.error, this.EnText)); // error path
      }

      else {
        row.InActive = !row.InActive;
      }
    }),
      error => this.ComSrv.ShowError(this.error, this.EnText); // error path
  }

  get PlacementID() { return this.traineeEmploymentForm.get("PlacementID"); }
  get TraineeID() { return this.traineeEmploymentForm.get("TraineeID"); }
  get ClassID() { return this.traineeEmploymentForm.get("ClassID"); }
  get Designation() { return this.traineeEmploymentForm.get("Designation"); }
  get PlatformName() { return this.traineeEmploymentForm.get("PlatformName"); }
  get NameofTraineeStorePage() { return this.traineeEmploymentForm.get("NameofTraineeStorePage"); }
  get LinkofTraineeStorePage() { return this.traineeEmploymentForm.get("LinkofTraineeStorePage"); }
  get Department() { return this.traineeEmploymentForm.get("Department"); }
  get EmploymentDuration() { return this.traineeEmploymentForm.get("EmploymentDuration"); }
  get Salary() { return this.traineeEmploymentForm.get("Salary"); }
  get SupervisorName() { return this.traineeEmploymentForm.get("SupervisorName"); }
  get SupervisorContact() { return this.traineeEmploymentForm.get("SupervisorContact"); }
  get EmploymentStartDate() { return this.traineeEmploymentForm.get("EmploymentStartDate"); }
  get EmploymentStatus() { return this.traineeEmploymentForm.get("EmploymentStatus"); }
  get EmploymentType() { return this.traineeEmploymentForm.get("EmploymentType"); }
  get EmployerName() { return this.traineeEmploymentForm.get("EmployerName"); }
  get EmployerNTN() { return this.traineeEmploymentForm.get("EmployerNTN"); }
  get EmployerBusinessType() { return this.traineeEmploymentForm.get("EmployerBusinessType"); }
  get EmploymentAddress() { return this.traineeEmploymentForm.get("EmploymentAddress"); }
  get District() { return this.traineeEmploymentForm.get("District"); }
  get EmploymentTehsil() { return this.traineeEmploymentForm.get("EmploymentTehsil"); }
  get TimeFrom() { return this.traineeEmploymentForm.get("TimeFrom"); }
  get TimeTo() { return this.traineeEmploymentForm.get("TimeTo"); }
  get EmploymentTiming() { return this.traineeEmploymentForm.get("EmploymentTiming"); }
  get IsTSP() { return this.traineeEmploymentForm.get("IsTSP"); }
  get OldID() { return this.traineeEmploymentForm.get("OldID"); }
  get EmploymentTehsilOld() { return this.traineeEmploymentForm.get("EmploymentTehsilOld"); }
  get OfficeContactNo() { return this.traineeEmploymentForm.get("OfficeContactNo"); }
  get IsMigrated() { return this.traineeEmploymentForm.get("IsMigrated"); }
  get TraineeName() { return this.traineeEmploymentForm.get("TraineeName"); }
  get ContactNumber() { return this.traineeEmploymentForm.get("ContactNumber"); }
  get TraineeCode() { return this.traineeEmploymentForm.get("TraineeCode"); }
  get ClassCode() { return this.traineeEmploymentForm.get("ClassCode"); }
  get VerificationMethodId() { return this.traineeEmploymentForm.get("VerificationMethodId"); }
  get PlacementTypeID() { return this.traineeEmploymentForm.get("PlacementTypeID"); }
  get FileType() { return this.traineeEmploymentForm.get("FileType"); }
  get FileName() { return this.traineeEmploymentForm.get("FileName"); }
  get FilePath() { return this.traineeEmploymentForm.get("FilePath"); }
  //get PlatformName() { return this.traineeEmploymentForm.get("PlatformName"); }
  //////////////////// My

  radioChange(typePlacement: number) {
    this.VerificationMethodId.setValue(null);
    //this.VerificationMethodsDrp = this.VerificationMethods.filter(x => x.PlacementTypeID == typePlacement);
    if (typePlacement === 2) {
      this.VerificationMethodsDrp = this.VerificationMethods.filter(
        x => x.PlacementTypeID === 2 &&
          (x.VerificationMethodType === 'Documents' || x.VerificationMethodType === 'Telephonic')
      );
    } else {
      this.VerificationMethodsDrp = this.VerificationMethods.filter(
        x => x.PlacementTypeID === typePlacement
      );
    }
    if (typePlacement == 1) {
      debugger;
      this.Designation.disable();
      this.Department.disable();
      this.EmploymentDuration.disable();
      this.SupervisorName.disable();
      this.TimeFrom.disable();
      this.TimeTo.disable();
      this.SupervisorContact.disable();
      this.Designation.setValue("");
      this.Department.setValue("");
      this.EmployerName.setValue("");
      this.EmployerNTN.setValue("");
      this.EmploymentStartDate.setValue("");
      this.EmploymentDuration.setValue("");
      this.SupervisorName.setValue("");
      this.SupervisorContact.setValue("");
      this.TimeFrom.setValue("");
      this.TimeTo.setValue("");

      //this.EmploymentStartDate.disable();
      //this.EmployerName.disable();
      //this.EmployerBusinessType.disable();

    }
    else {
      this.Designation.enable();
      this.Department.enable();
      this.EmploymentDuration.enable();
      this.SupervisorName.enable();
      this.SupervisorContact.enable();
      //this.Designation.setValue("");
      //this.Department.setValue("");
      //this.EmploymentDuration.setValue("");
      //this.SupervisorName.setValue("");
      //this.SupervisorContact.setValue("");
      //this.EmploymentStartDate.enable();
      //this.EmployerName.enable();
      //this.EmployerBusinessType.enable();
    }

  }
  selectedVerificationMethodId: string = null;
  onVerificarionMethodChange(event) {
    this.selectedVerificationMethodId = event.value;
    if (this.PlacementTypeID.value == 1 && event.value != 9) {
      this.PlatformName.clearValidators();
      this.NameofTraineeStorePage.clearValidators();
      this.LinkofTraineeStorePage.clearValidators();
    }
    else if (this.PlacementTypeID.value == 2 && event.value != 6) {
      this.FilePath.clearValidators();
      this.PlatformName.clearValidators();
      this.NameofTraineeStorePage.clearValidators();
      this.LinkofTraineeStorePage.clearValidators();
    }
    else {
      this.FilePath.setValidators([Validators.required]);
      this.FilePath.updateValueAndValidity();
    }

  }

  uploadFile: File;
  fileType: string;
  onUploadFileChange(files: FileList, type: string) {
    const file = files[0];
    this.uploadFile = file;
    this.fileType = type;
  }

  Submit(myform: FormGroupDirective) {

    //if (this.PlacementTypeID.value == 1 && this.VerificationMethodId.value != 9) {
    //  this.PlatformName.disable();
    //  this.NameofTraineeStorePage.disable();
    //  this.LinkofTraineeStorePage.disable();
    //  this.PlatformName.setValue("");
    //  this.NameofTraineeStorePage.setValue("");
    //  this.LinkofTraineeStorePage.setValue("");
    //}
    //else if (this.PlacementTypeID.value == 2) {
    this.PlacementTypeID.setValue(2);
    this.PlacementTypeID.clearValidators();

      this.PlatformName.disable();
      this.NameofTraineeStorePage.disable();
      this.LinkofTraineeStorePage.disable();
      this.PlatformName.setValue("");
      this.NameofTraineeStorePage.setValue("");
      this.LinkofTraineeStorePage.setValue("");
    //}
    if (this.SchmeFundingSource == 14 && this.ProgramType == 1) {

    }
    else {
      this.EmployerNTN.setValidators(null);
      this.EmployerNTN.updateValueAndValidity();
    }

    this.traineeEmploymentForm.patchValue({ EmploymentTiming: this.TimeFrom.value + " to " + this.TimeTo.value });
    this.traineeEmploymentForm.patchValue({ IsTSP: 1 });
    // this.traineeEmploymentForm.patchValue({ FileType: this.fileType });
    this.getFormValidationErrors();
    if (!this.traineeEmploymentForm.valid)
      return;

    let formValues = this.traineeEmploymentForm.getRawValue();
    //if (formValues.PlacementTypeID == "2" && formValues.VerificationMethodId == '6') {
    //  if ((!this.fileType || this.fileType != "SalarySlip") && !this.traineeEmploymentForm.get('EOBI').value) {
    //    this.ComSrv.ShowError("Kindly select any file or enter EOBI # to proceed.");
    //    return;
    //  }
    //}
    //if (formValues.PlacementTypeID == "1") {
    //  if (!this.fileType || this.fileType != "Self") {
    //    this.ComSrv.ShowError("Kindly select any file to proceed.");
    //    return;
    //  }
    //}
    this.working = true;
    //const formData: FormData = new FormData();
    //if (this.uploadFile) {
    //  formData.append("docFile", this.uploadFile, this.uploadFile.name);
    //}
    //formData.append("data", JSON.stringify(formValues));
    this.ComSrv.postFile('api/TSPEmployment/SaveOJT', formValues)
      .subscribe((result: any) => {
        if (result) {
          this.getData();
          this.ComSrv.openSnackBar("Saved Successfully");
        } else {
          this.ComSrv.ShowError("Data is not saved");
        }

        this.working = false;
      },
        (error) => {
          this.error = error.error;
          //this.ComSrv.ShowError(this.error.toString(), "Error");
          this.ComSrv.ShowError(error.error)
          this.working = false;
        });
  }
  setFileType() {
    if (this.EmploymentStatus.value == EmploymentStatuses.Employed) {
      this.FileType.setValue(this.PlacementTypeID.value == '2' && this.VerificationMethodId.value == '6' ? 'Employment Letter | Salary Slip' : 'Self' && this.VerificationMethodId.value == '9');
      if (this.VerificationMethodId.value == '7')
        this.FilePath.disable();
      else
        this.FilePath.enable();
      this.FilePath.updateValueAndValidity();
    }
  }
  getFormValidationErrors() {
    Object.keys(this.traineeEmploymentForm.controls).forEach(key => {
      const controlErrors: ValidationErrors = this.traineeEmploymentForm.get(key).errors;
      if (controlErrors != null) {
        Object.keys(controlErrors).forEach(keyError => {
          console.log('Key control: ' + key + ', keyError: ' + keyError + ', err value: ', controlErrors[keyError]);
        });
      }
    });
  }
}
