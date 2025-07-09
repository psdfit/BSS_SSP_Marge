import { Component, Inject, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, FormGroupDirective, NgForm, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { DomSanitizer } from '@angular/platform-browser';
import { ActivatedRoute } from '@angular/router';
import { environment } from '../../../environments/environment';
import { CommonSrvService } from '../../common-srv.service';
import { UserRightsModel } from '../../master-data/users/users.component';



@Component({
  selector: 'hrapp-telephonic-employment-verification-ojt-dialog',
  templateUrl: './telephonic-employment-verification-ojt-dialog.component.html',
  styleUrls: ['./telephonic-employment-verification-ojt-dialog.component.scss']
})

export class TelephonicEmploymentVerificationOJT implements OnInit {
  EmploymentVerificationForm: FormGroup;

  CallCenterVerificationTraineeList: [];
  CallCenterVerificationSupervisorList: [];
  CallCenterVerificationCommentsList: [];


  placementverification: [];
  VerificationMethodList: [];
  id: number;
  placementID: number;
  traineeName: string;
  payrollVerification: Boolean;
  payrollVerificationStatus: Boolean;
  physicalVerification: Boolean;
  telephonicVerification: Boolean;
  physicalVerificationStatus: Boolean;
  telephonicVerificationStatus: Boolean;
  verificationMethodID: number;
  placementTypeID: number;
  isVerified: Boolean;
  comments: string;
  attachment: string;
  formrights: UserRightsModel;
  EnText: string = " Employment Verification Form";
  title: string; savebtn: string = "Save"; working: boolean; error: string;
  EmploymentData: any;
  EmploymentTypeID: number;
  OrgConfig: any;

  @ViewChild("ngForm") ngFrom: NgForm;
  DistrictDrp: any;
  TehsilDrp: any;
  constructor(private fb: FormBuilder, public domSanitizer: DomSanitizer, private ComSrv: CommonSrvService, private route: ActivatedRoute,
    public dialogRef: MatDialogRef<TelephonicEmploymentVerificationOJT>,
    @Inject(MAT_DIALOG_DATA) public data: any) {
    debugger;
    this.getData();
    this.GenerateForm();

  }

  fileType: string = "";
  fileName: string = "";
  filePath: string = "";
  verificationMethodType: string = "";
  //siteUrl: string = this.ComSrv.appSettings.UsersAPIURL;

  ngOnInit(): void {
    this.GetCallCenterVerification();
    //this.EmploymentTypeID = this.data['TraineeRow'].EmploymentTypeID;
    //this.placementID = this.data['TraineeRow'].PlacementTypeID;
    //this.verificationMethodID = this.data['TraineeRow'].VerificationMethodId;
    //this.EmploymentVerificationForm.patchValue({
    //  PlacementID: this.data['TraineeRow'].PlacementID,
    //  TraineeName: this.data['TraineeRow'].TraineeName,
    //  IsVerified: this.data['TraineeRow'].IsVerified,
    //  Comments: this.data['TraineeRow'].Comments,

    //  OfficeContactNo: this.data['TraineeRow'].OfficeContactNo,
    //  Designation: this.data['TraineeRow'].Designation,
    //  Department: this.data['TraineeRow'].Department,
    //  EmploymentDuration: this.data['TraineeRow'].EmploymentDuration,
    //  Salary: this.data['TraineeRow'].Salary,
    //  SupervisorName: this.data['TraineeRow'].SupervisorName,
    //  SupervisorContact: this.data['TraineeRow'].SupervisorContact,
    //  EmploymentStartDate: this.data['TraineeRow'].EmploymentStartDate,
    //  EmploymentStatus: this.data['TraineeRow'].EmploymentStatus,

    //  EmployerName: this.data['TraineeRow'].EmployerName,
    //  EmployerBusinessType: this.data['TraineeRow'].EmployerBusinessType,
    //  EmploymentAddress: this.data['TraineeRow'].EmploymentAddress,
    //  District: this.data['TraineeRow'].District,
    //  EmploymentTehsil: this.data['TraineeRow'].EmploymentTehsil,
    //  EmploymentTiming: this.data['TraineeRow'].EmploymentTiming,
    //  TimeFrom: this.data['TraineeRow'].EmploymentTiming.split(" to ")[0],
    //  TimeTo: this.data['TraineeRow'].EmploymentTiming.split(" to ")[1],
    //  EOBI: this.data['TraineeRow'].EOBI,
    this.EmploymentTypeID = this.data.EmploymentTypeID;
    this.placementID = this.data.PlacementTypeID;
    this.verificationMethodID = this.data.VerificationMethodId;
    this.EmploymentVerificationForm.patchValue({
      CallCenterVerificationTraineeID: this.data.CallCenterVerificationTraineeID,
      CallCenterVerificationSupervisorID: this.data.CallCenterVerificationSupervisorID,
      CallCenterVerificationCommentsID: this.data.CallCenterVerificationCommentsID,

      PlacementID: this.data.PlacementID,
      TraineeName: this.data.TraineeName,
      ContactNumber: this.data.ContactNumber,
      IsVerified: this.data.IsVerified,
      Comments: this.data.Comments,

      OfficeContactNo: this.data.OfficeContactNo,
      Designation: this.data.Designation,
      Department: this.data.Department,
      EmploymentDuration: this.data.EmploymentDuration,
      Salary: this.data.Salary,
      SupervisorName: this.data.SupervisorName,
      SupervisorContact: this.data.SupervisorContact,
      EmploymentStartDate: this.data.EmploymentStartDate,
      EmploymentStatus: this.data.EmploymentStatus,

      EmployerName: this.data.EmployerName,
      EmployerBusinessType: this.data.EmployerBusinessType,
      EmploymentAddress: this.data.EmploymentAddress,
      District: this.data.District,
      EmploymentTehsil: this.data.EmploymentTehsil,
      EmploymentTiming: this.data.EmploymentTiming,
      TimeFrom: this.data.EmploymentTiming.split(" to ")[0],
      TimeTo: this.data.EmploymentTiming.split(" to ")[1],
      EOBI: this.data.EOBI,
      ClassID: this.data.ClassID,

    });
    //this.fileName = this.data['TraineeRow'].FileName;
    //this.filePath = this.data['TraineeRow'].FilePath;
    //this.fileType = this.data['TraineeRow'].FileType;
    //this.verificationMethodType = this.data['TraineeRow'].VerificationMethodType;

    //if (this.data['TraineeRow'].PlacementTypeID == 1) {
    this.fileName = this.data.FileName;
    this.filePath = this.data.FilePath;
    this.fileType = this.data.FileType;
    this.verificationMethodType = this.data.VerificationMethodType;

    if (this.data.PlacementTypeID == 1) {
      this.Designation.disable();
      this.Department.disable();
      this.EmploymentDuration.disable();
      this.SupervisorName.disable();
      this.SupervisorContact.disable();
      this.EmploymentStartDate.disable();
      this.EmployerName.disable();
      this.EmployerBusinessType.disable();
      this.TimeFrom.disable();
      this.TimeTo.disable();
    }
    if (this.data.PlacementTypeID == 2) {
      this.Designation.disable();
      this.Department.disable();
      this.EmploymentDuration.disable();
      this.SupervisorName.disable();
      this.SupervisorContact.disable();
      this.EmploymentStartDate.disable();
      this.EmployerName.disable();
      this.EmployerBusinessType.disable();
      this.TimeFrom.disable();
      this.TimeTo.disable();
      this.EmploymentAddress.disable();
    }

    this.formrights = this.ComSrv.getFormRights();
    //this.getData();
  }



  GenerateForm() {
    this.EmploymentVerificationForm = this.fb.group({
      PlacementID: ["", Validators.required],
      TraineeName: "",
      ContactNumber: "",
      IsVerified: [],
      Comments: [""],


      CallCenterVerificationTraineeID: ["", Validators.required],
      CallCenterVerificationSupervisorID: ["", Validators.required],
      CallCenterVerificationCommentsID: ["", Validators.required],

      Designation: ['', [Validators.required]],
      Department: ['', [Validators.required]],
      EmploymentDuration: ['', [Validators.required]],
      Salary: ['', [Validators.required]],
      SupervisorName: ['', [Validators.required]],
      SupervisorContact: ['', [Validators.required]],
      EmploymentStartDate: ['', [Validators.required]],
      EmploymentStatus: ['', [Validators.required]],

      EmployerName: ['', [Validators.required]],
      EmployerBusinessType: [''],
      EmploymentAddress: ['', [Validators.required]],
      District: ['', [Validators.required]],
      EmploymentTehsil: ['', [Validators.required]],
      TimeFrom: [''],
      TimeTo: ['', [Validators.required]],
      EmploymentTiming: ['', [Validators.required]],
      Attachment: '',
      EOBI: [''],
      OfficeContactNo: ['', [Validators.required]],
      ClassID: ['', [Validators.required]],
    }, { updateOn: "blur" });
    //if (this.paramClassId != "" && this.paramTraineeId != "") {
    //  this.ClassID.setValue(this.paramClassId);
    //  this.TraineeID.setValue(this.paramTraineeId);
    //}
  }
  getData() {
    this.ComSrv.postJSON('api/TSPEmployment/GetEmploymentDataForVerificationOJT', { "ClassID": this.data.ClassID, "TraineeID": this.data.TraineeID }).subscribe((d: any) => {
      this.EmploymentData = d.EmploymentData;
    });
  }
  GetCallCenterVerification() {
    debugger;
    this.ComSrv.getJSON('api/EmploymentVerification/GetCallCenterVerification').subscribe((d: any) => {
      debugger;
      this.CallCenterVerificationTraineeList = d[0]
      this.CallCenterVerificationSupervisorList = d[1]
      this.CallCenterVerificationCommentsList = d[2]
      // this.ComplaintTypeddl = d;
    }, error => this.error = error // error path
    );
  }
  Submit(myform: FormGroupDirective) {
    debugger;
    if (!this.EmploymentVerificationForm.valid)
      return;
    if (this.EmploymentVerificationForm.value.CallCenterVerificationTraineeID == 0 || this.EmploymentVerificationForm.value.CallCenterVerificationSupervisorID == 0 || this.EmploymentVerificationForm.value.CallCenterVerificationCommentsID == 0) {
      this.ComSrv.ShowWarning("Required All Fields of Call Center Verification");
      return;
    }
    this.EmploymentVerificationForm["ClassID"] = this.data.ClassID
    this,this.EOBI["EOBI"] = '';
    this.EmploymentVerificationForm.patchValue({ EmploymentTiming: this.TimeFrom.value + " to " + this.TimeTo.value });
    this.working = true;
    this.ComSrv.postJSON('api/EmploymentVerification/UpdateTelephonicEmploymentVerificationOJT', this.EmploymentVerificationForm.value)
      .subscribe((d: any) => {


        //this.reset(myform);
        this.title = "Add New";
        this.savebtn = "Save";
        this.ComSrv.openSnackBar("Saved Successfully");
        this.working = false;
        //this.dialogRef.close(this.data['ClassRow'][0]);
        this.dialogRef.close(this.EmploymentVerificationForm.getRawValue());
      },
        (error) => {

          this.error = error.error;
          this.ComSrv.ShowError(this.error.toString(), "Error");
          this.working = false;
        });


  }
  toggleVerify(value) {
    if (this.IsVerified)
      this.IsVerified.setValue(true);
    else
      this.IsVerified.setValue(false);

  }
  reset(myform: FormGroupDirective) {
    this.EmploymentVerificationForm.reset();
    myform.reset();
    this.PlacementID.setValue(0);
    this.title = "Add New";
    this.savebtn = "Save";
  }


  get ID() { return this.EmploymentVerificationForm.get("ID"); }
  get TraineeName() { return this.EmploymentVerificationForm.get("TraineeName"); }
  get PlacementID() { return this.EmploymentVerificationForm.get("PlacementID"); }
  get PayrollVerification() { return this.EmploymentVerificationForm.get("PayrollVerification"); }
  get PayrollVerificationStatus() { return this.EmploymentVerificationForm.get("PayrollVerificationStatus"); }
  get PhysicalVerification() { return this.EmploymentVerificationForm.get("PhysicalVerification"); }
  get TelephonicVerification() { return this.EmploymentVerificationForm.get("TelephonicVerification"); }
  get PhysicalVerificationStatus() { return this.EmploymentVerificationForm.get("PhysicalVerificationStatus"); }
  get TelephonicVerificationStatus() { return this.EmploymentVerificationForm.get("TelephonicVerificationStatus"); }
  get VerificationMethodID() { return this.EmploymentVerificationForm.get("VerificationMethodID"); }
  get IsVerified() { return this.EmploymentVerificationForm.get("IsVerified"); }
  get Comments() { return this.EmploymentVerificationForm.get("Comments"); }
  get Attachment() { return this.EmploymentVerificationForm.get("Attachment"); }
  get InActive() { return this.EmploymentVerificationForm.get("InActive"); }
  get OfficeContactNo() { return this.EmploymentVerificationForm.get("OfficeContactNo"); }

  get Designation() { return this.EmploymentVerificationForm.get("Designation"); }
  get Department() { return this.EmploymentVerificationForm.get("Department"); }
  get EmploymentDuration() { return this.EmploymentVerificationForm.get("EmploymentDuration"); }
  get Salary() { return this.EmploymentVerificationForm.get("Salary"); }
  get SupervisorName() { return this.EmploymentVerificationForm.get("SupervisorName"); }
  get SupervisorContact() { return this.EmploymentVerificationForm.get("SupervisorContact"); }
  get EmploymentStartDate() { return this.EmploymentVerificationForm.get("EmploymentStartDate"); }
  get EmploymentStatus() { return this.EmploymentVerificationForm.get("EmploymentStatus"); }
  get EmploymentType() { return this.EmploymentVerificationForm.get("EmploymentType"); }
  get EmployerName() { return this.EmploymentVerificationForm.get("EmployerName"); }
  get EmployerBusinessType() { return this.EmploymentVerificationForm.get("EmployerBusinessType"); }
  get EmploymentAddress() { return this.EmploymentVerificationForm.get("EmploymentAddress"); }
  get District() { return this.EmploymentVerificationForm.get("District"); }
  get EmploymentTehsil() { return this.EmploymentVerificationForm.get("EmploymentTehsil"); }
  get TimeFrom() { return this.EmploymentVerificationForm.get("TimeFrom"); }
  get TimeTo() { return this.EmploymentVerificationForm.get("TimeTo"); }
  get EOBI() { return this.EmploymentVerificationForm.get("EOBI"); }
  get ClassID() { return this.EmploymentVerificationForm.get("ClassID"); }
  get CallCenterVerificationTraineeID() { return this.EmploymentVerificationForm.get("CallCenterVerificationTraineeID"); }
  get CallCenterVerificationSupervisorID() { return this.EmploymentVerificationForm.get("CallCenterVerificationSupervisorID"); }
  get CallCenterVerificationCommentsID() { return this.EmploymentVerificationForm.get("CallCenterVerificationCommentsID"); }


  closeDialog() {
    this.dialogRef.close();
  }

}
