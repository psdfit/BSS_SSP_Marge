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
const MyValidator: ValidatorFn = (fg: FormGroup) => {
  const PlacementTypeID = fg.get('PlacementTypeID').value;
  const VerificationMethodId = fg.get('VerificationMethodId').value;
  if (fg.get('EmploymentStatus').value == 'Employed') {
    if (PlacementTypeID == 2 && VerificationMethodId == 7)
      fg.get('FilePath').clearValidators();
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
  selector: 'hrapp-psp',
    templateUrl: './psp-employment.component.html',
    styleUrls: ['./psp-employment.component.scss']
})

export class PSPEmpComponent implements OnInit {
  trineeProfile: any;
  traineeEmploymentForm: FormGroup;
  ExcelFileForm: FormGroup;
  traineeEmployment: MatTableDataSource<any>;
  displayedColumns = ['TraineeID', 'PSPBatchID', 'EmploymentStatus', 'Designation', 'Department', 'EmploymentDuration', 'Salary', 'SupervisorName','Action'];//, 'SupervisorContact', 'EmploymentStartDate', 'EmploymentStatus', 'EmploymentType', 'EmployerName', 'EmployerBusinessType', 'EmploymentAddress', 'District', 'EmploymentTehsil', 'EmploymentTiming', 'IsPSP', 'OldID', 'EmploymentTehsilOld', 'OfficeContactNo', 'IsMigrated', 'TraineeName', "Action"];
  //ClassDrp: any;
  BatchDrp: any;
  TraineeProfileDrp: any;
  PlacementTypeDrp: any;
  PlacementStatusDrp: any;
  VerificationMethods: any;
  VerificationMethodsDrp: any;
  DistrictDrp: any;
  TehsilDrp: any;
  paramPSPBatchID: number;
  paramTraineeId: number;
  traineeID: number;
  pspBatchID: number;
  designation: string;
  department: string;
  employmentDuration: number;
  salary: number;
  supervisorName: string;
  supervisorContact: string;
  employmentStartDate: Date;
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
  isPSP: Boolean;
  oldID: string;
  employmentTehsilOld: string;
  officeContactNo: string;
  isMigrated: Boolean;
  traineeName: string;
  formrights: UserRightsModel;
  EnText: string = " PSP Employment Form";
  title: string; savebtn: string = "Save"; working: boolean; error: string;
  EmploymentData: any;
  OrgConfig: any;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild("ngForm") ngFrom: NgForm;
  constructor(private fb: FormBuilder, private ComSrv: CommonSrvService, private route: ActivatedRoute) {
    this.GenerateForm();
    this.ExcelFileForm = this.fb.group({
      ExcelField: ['', Validators.required]
    });
    route.params.subscribe(
      params => {
        //console.log(params);
        this.paramPSPBatchID = Number(params["PSPBatchID"]);
        this.paramTraineeId = Number(params["traineeid"]);
        //console.log(this.paramPSPBatchID + "," + this.paramTraineeId)
        if (this.paramPSPBatchID && this.paramTraineeId) {
          this.TraineeID.setValue(this.paramTraineeId);
          this.PSPBatchID.setValue(this.paramPSPBatchID);
          //this.traineeEmploymentForm.controls['TraineeID'].disable();
          //this.traineeEmploymentForm.controls['PSPBatchID'].disable();
        }
      });
    this.PlacementTypeID.valueChanges.subscribe((v) => {
      this.setFileType();
    });
    this.VerificationMethodId.valueChanges.subscribe((v) => {
      this.setFileType();
    });
    this.EmploymentStatus.valueChanges.subscribe((v) => {
      if (v == 'Employed') {
        this.Designation.enable();
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
        this.EmployerBusinessType.enable();
        this.EmploymentAddress.enable();
        this.District.enable();
        this.EmploymentTehsil.enable();
        this.EmploymentTiming.enable();
        this.OfficeContactNo.enable();
       
        this.FilePath.enable();

      }
      else {
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
        this.EmployerBusinessType.disable();
        this.EmploymentAddress.disable();
        this.District.disable();
        this.EmploymentTehsil.disable();
        this.EmploymentTiming.disable();
        this.OfficeContactNo.disable();
        
      }
      for (const field in this.traineeEmploymentForm.controls) { // 'field' is a string
        if(field!='EmploymentStatus')
         this.traineeEmploymentForm.get(field).updateValueAndValidity(); // 'control' is a FormControl  

      }

      this.traineeEmploymentForm.updateValueAndValidity();
    });
  }
    ngOnInit(): void {
        this.ComSrv.setTitle("PSP Employment");
    this.traineeEmployment = new MatTableDataSource([]);
    this.formrights = this.ComSrv.getFormRights();
    this.getData();
  }

  GenerateForm() {
    this.traineeEmploymentForm = this.fb.group({
      PlacementID: 0,
      VerificationMethodId: ['', [Validators.required]],
      TraineeID: [0, [Validators.required]],
      PSPBatchID: [{ value: '', disabled: true }, [Validators.required]],
      Designation: ['', [Validators.required]],
      Department: ['', [Validators.required]],
      EmploymentDuration: ['', [Validators.required]],
      Salary: ['', [Validators.required]],
      SupervisorName: ['', [Validators.required]],
      SupervisorContact: ['', [Validators.required]],
      EmploymentStartDate: ['', [Validators.required]],
      EmploymentStatus: ['', [Validators.required]],
     
      EmployerName: ['', [Validators.required]],
      EmployerBusinessType: ['', [Validators.required]],
      EmploymentAddress: ['', [Validators.required]],
      District: ['', [Validators.required]],
      EmploymentTehsil: ['', [Validators.required]],
      TimeFrom: [''],
      TimeTo: ['', [Validators.required]],
      EmploymentTiming: ['', [Validators.required]],
      IsPSP: false,
      OldID: "",
      EmploymentTehsilOld: [''],
      OfficeContactNo: ['', [Validators.required]],
      IsMigrated: false,
      TraineeName: [{ value: '', disabled: true }, [Validators.required]],
      PlacementTypeID: ['', [Validators.required]],
      EOBI: [''],
      FileType: [''],
      FileName: [''],
      FilePath: ['',Validators.required]
    }, { updateOn: "blur", validators: MyValidator  });
   
   
  }                                    
                                    
  reset(myform: FormGroupDirective) {
    this.traineeEmploymentForm.reset();
    myform.reset();
    this.PlacementID.setValue(0);
    this.title = "Add New";
    this.savebtn = "Save";
  }
  DeadlineEnd: boolean = false;
  getData() {
    this.ComSrv.postJSON('api/PSPEmployment/GetData', { "PSPBatchID": this.paramPSPBatchID, "TraineeID": this.paramTraineeId }).subscribe((d: any) => {
      console.log(d);
      this.BatchDrp = d.PSPBatch;
      this.TraineeProfileDrp = d.TraineeProfile;
      this.PlacementTypeDrp = d.PlacementTypes;
      this.PlacementStatusDrp = d.PlacementStatus;
      this.VerificationMethods = d.VerificationMethods;
      this.DistrictDrp = d.District;
      this.TehsilDrp = d.Tehsil;
      this.EmploymentData = d.EmploymentData;
      this.OrgConfig = d.OrgConfig;
      if (d.DeadlineStatus == "Date Passed") {
        this.ComSrv.ShowError("Can't submit the request. Deadline end", "Error");
        this.DeadlineEnd = true;
      }
      if (d.EmploymentData) {
        this.traineeEmployment = new MatTableDataSource<any>(d.EmploymentData);
        this.traineeEmployment.paginator = this.paginator;
        this.traineeEmployment.sort = this.sort;
      }
      this.trineeProfile = d.TraineeProfile;
      if (this.trineeProfile) {
        this.traineeEmploymentForm.patchValue({
          TraineeID: this.trineeProfile.TraineeID,
          TraineeName: this.trineeProfile.TraineeName,
          //PSPBatchID: this.trineeProfile.PSPBatchID,
          PlacementID: this.trineeProfile.PlacementID,
          VerificationMethodId: this.trineeProfile.VerificationMethodId,
          Designation: this.trineeProfile.Designation,
          Department: this.trineeProfile.Department,
          EmploymentDuration: this.trineeProfile.EmploymentDuration,
          Salary: this.trineeProfile.Salary,
          SupervisorName: this.trineeProfile.SupervisorName,
          SupervisorContact: this.trineeProfile.SupervisorContact,
          EmploymentStartDate: this.trineeProfile.EmploymentStartDate,
          EmploymentStatus: this.trineeProfile.EmploymentStatus,
          EmployerName: this.trineeProfile.EmployerName,
          EmployerBusinessType: this.trineeProfile.EmployerBusinessType,
          EmploymentAddress: this.trineeProfile.EmploymentAddress,
          District: this.trineeProfile.District,
          EmploymentTehsil: this.trineeProfile.EmploymentTehsil,
          TimeFrom: this.trineeProfile.EmploymentTiming ?this.trineeProfile.EmploymentTiming.split(" to ")[0] : "",
          TimeTo: this.trineeProfile.EmploymentTiming ? this.trineeProfile.EmploymentTiming.split(" to ")[1] : "",
          //EmploymentTiming: ['', [Validators.required]],
          IsPSP: this.trineeProfile.IsPSP,
          OfficeContactNo: this.trineeProfile.OfficeContactNo,
          IsMigrated: this.trineeProfile.IsMigrated,
          PlacementTypeID: this.trineeProfile.PlacementTypeID,
          EOBI: this.trineeProfile.EOBI,
          FileType: this.trineeProfile.FileType,
          FileName: this.trineeProfile.FileName,
          FilePath: this.trineeProfile.FilePath
        });
        this.fileType = this.trineeProfile.FileType;

        if (this.trineeProfile.PlacementTypeID) {
          this.VerificationMethodsDrp = this.VerificationMethods.filter(x => x.PlacementTypeID == this.trineeProfile.PlacementTypeID);
        }
      }
    }, error => this.error = error // error path
    );
  }
  toggleEdit(row) {
    this.PlacementID.setValue(row.PlacementID);
    this.TraineeID.setValue(row.TraineeID);
    this.PSPBatchID.setValue(row.PSPBatchID);
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
    this.EmployerBusinessType.setValue(row.EmployerBusinessType);
    this.EmploymentAddress.setValue(row.EmploymentAddress);
    this.District.setValue(row.District);
    this.EmploymentTehsil.setValue(row.EmploymentTehsil);
    this.EmploymentTiming.setValue(row.EmploymentTiming);
    this.TimeFrom.setValue(row.EmploymentTiming.split(" to ")[0]);
    this.TimeTo.setValue(row.EmploymentTiming.split(" to ")[1]);
   
    this.OfficeContactNo.setValue(row.OfficeContactNo);
   
    this.TraineeName.setValue(row.TraineeName);

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
  get PSPBatchID() { return this.traineeEmploymentForm.get("PSPBatchID"); }
  get Designation() { return this.traineeEmploymentForm.get("Designation"); }
  get Department() { return this.traineeEmploymentForm.get("Department"); }
  get EmploymentDuration() { return this.traineeEmploymentForm.get("EmploymentDuration"); }
  get Salary() { return this.traineeEmploymentForm.get("Salary"); }
  get SupervisorName() { return this.traineeEmploymentForm.get("SupervisorName"); }
  get SupervisorContact() { return this.traineeEmploymentForm.get("SupervisorContact"); }
  get EmploymentStartDate() { return this.traineeEmploymentForm.get("EmploymentStartDate"); }
  get EmploymentStatus() { return this.traineeEmploymentForm.get("EmploymentStatus"); }
  get EmploymentType() { return this.traineeEmploymentForm.get("EmploymentType"); }
  get EmployerName() { return this.traineeEmploymentForm.get("EmployerName"); }
  get EmployerBusinessType() { return this.traineeEmploymentForm.get("EmployerBusinessType"); }
  get EmploymentAddress() { return this.traineeEmploymentForm.get("EmploymentAddress"); }
  get District() { return this.traineeEmploymentForm.get("District"); }
  get EmploymentTehsil() { return this.traineeEmploymentForm.get("EmploymentTehsil"); }
  get TimeFrom() { return this.traineeEmploymentForm.get("TimeFrom"); }
  get TimeTo() { return this.traineeEmploymentForm.get("TimeTo"); }
  get EmploymentTiming() { return this.traineeEmploymentForm.get("EmploymentTiming"); }
  get IsPSP() { return this.traineeEmploymentForm.get("IsPSP"); }
  get OldID() { return this.traineeEmploymentForm.get("OldID"); }
  get EmploymentTehsilOld() { return this.traineeEmploymentForm.get("EmploymentTehsilOld"); }
  get OfficeContactNo() { return this.traineeEmploymentForm.get("OfficeContactNo"); }
  get IsMigrated() { return this.traineeEmploymentForm.get("IsMigrated"); }
  get TraineeName() { return this.traineeEmploymentForm.get("TraineeName"); }
  get VerificationMethodId() { return this.traineeEmploymentForm.get("VerificationMethodId"); }
  get PlacementTypeID() { return this.traineeEmploymentForm.get("PlacementTypeID"); }
  get FileType() { return this.traineeEmploymentForm.get("FileType"); }
  get FileName() { return this.traineeEmploymentForm.get("FileName"); }
  get FilePath() { return this.traineeEmploymentForm.get("FilePath"); }

  //////////////////// My

  radioChange(event) {
    this.VerificationMethodsDrp = this.VerificationMethods.filter(x => x.PlacementTypeID == event.value);
  }
  selectedVerificationMethodId: string = null;
  onVerificarionMethodChange(event) {
    this.selectedVerificationMethodId = event.value;
  }

  uploadFile: File;
  fileType: string;
  onUploadFileChange(files: FileList, type: string) {
    const file = files[0];
    this.uploadFile = file;
    this.fileType = type;
  }

  Submit(myform: FormGroupDirective) {
    this.traineeEmploymentForm.patchValue({ EmploymentTiming: this.TimeFrom.value + " to " + this.TimeTo.value });
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

    this.ComSrv.postFile('api/PSPEmployment/Save', formValues)
      .subscribe((result: any) => {
        if (result) {
          this.ComSrv.openSnackBar("Saved Successfully");
        } else {
          this.ComSrv.ShowError("Data is not saved");
        }

        this.working = false;
      },
        (error) => {
          this.error = error.error;
          this.ComSrv.ShowError(this.error.toString(), "Error");
          this.working = false;
        });
  }
  setFileType() {
    if (this.EmploymentStatus.value == EmploymentStatuses.Employed) {
      this.FileType.setValue(this.PlacementTypeID.value == '2' && this.VerificationMethodId.value == '6' ? 'Employment Letter | Salary Slip' : 'Self');
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


  ChekTimeDifference() {
    if (this.traineeEmploymentForm.get('TimeFrom').invalid) {// || this.VisitEndTime.invalid || this.VisitStartTime.value == null || this.VisitEndTime.value == null) {
      return;
    }
    console.log(this.traineeEmploymentForm.get('TimeFrom'));
    return;

    //var Stime = this.VisitStartTime.value.split(":");
    //var SMtime = this.VisitStartTime.value.split(" ");
    //var SM = SMtime[1];
    //var IStime = Stime[0];
    //var FStime = Number(IStime);



    //var Etime = this.VisitEndTime.value.split(":");
    //var EMtime = this.VisitEndTime.value.split(" ");
    //var EM = EMtime[1];
    //var IEtime = Etime[0];
    //var FEtime = Number(IEtime);


    //if (FStime > FEtime) {
    //  this.VisitEndTime.reset();
    //  this.error = "Class End Time Should be Greater than Start Time";
    //  this.ComSrv.ShowError(this.error.toString(), "Error");
    //  return false;
    //}
    //else {
    //  return;
    //}

  }
}
