/* **** Aamer Rehman Malik *****/
import { Component, Inject, OnInit, ElementRef } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, FormGroupDirective, Validators, ValidatorFn } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { CommonSrvService } from 'src/app/common-srv.service';
import * as moment from 'moment';
import { DomSanitizer } from '@angular/platform-browser';

//const MyValidator: ValidatorFn = (fg: FormGroup) => {
//  const PlacementTypeID = fg.get('PlacementTypeID').value;
//  const VerificationMethodId = fg.get('VerificationMethodId').value;
//  if (fg.get('EmploymentStatus').value == 'Employed') {
//    if (PlacementTypeID == 2 && VerificationMethodId == 7)
//      fg.get('FilePath').clearValidators();
//    else {
//      if (PlacementTypeID == 2 && VerificationMethodId == 6) {
//        return fg.get('FilePath').value != "" || fg.get('EOBI').value != "" ? null : { fileoeobi: true };
//      }
//      else
//        return fg.get('FilePath').value != "" ? null : { file: true };


//    }
//  }
//  else
//    fg.get('FilePath').clearValidators();
//};
@Component({
  selector: 'app-deo-employment-verification-dialog',
  templateUrl: './deo-employment-verification-dialog.component.html',
  styleUrls: ['./deo-employment-verification-dialog.component.scss']
})
export class DEOEmploymentVerificationDialogComponent implements OnInit {
  ExcelFileForm: any;
  traineeEmploymentForm: FormGroup;
  ClassDrp: any;
  PlacementStatusDrp: any;
  PlacementTypeDrp: any;
  VerificationMethods: any;
  DistrictDrp: any;
  TehsilDrp: any;
  EmploymentData: any;
  OrgConfig: any;
  DeadlineEnd: boolean;
  paramClassId: any;
  working = false;
  EnText = '';
  update: String;

  savebtn = 'Upload';
  VerificationMethodsDrp: any;
  constructor(private fb: FormBuilder,
    private el: ElementRef, private ComSrv: CommonSrvService, private dialogRef: MatDialogRef<DEOEmploymentVerificationDialogComponent>, public domSanitizer: DomSanitizer,
    @Inject(MAT_DIALOG_DATA) public data: Array<TspTrainees>) {
    this.traineeEmploymentForm = this.fb.group({
      ClassID: '',
      Detail: new FormArray([])
    }, { updateOn: "blur" });
    this.getData();

  }
  ngOnInit(): void {

  }
  FilterFindvalue(arr: Array<any>, filter: any, filfield: string, value: any) {
    let rArr = arr.filter((i) => i[filfield] == filter);
    return rArr.length > 0 ? rArr[0][value] : null;
  }
  GenerateForm(itm: any) {
    let form: FormGroup = this.fb.group({
      PlacementID: this.FilterFindvalue(this.EmploymentData, itm.TraineeID, 'TraineeID', 'PlacementID') ?? 0,

      TraineeID: [itm.TraineeID],
      TradeName: ["NA"],
      //ClassID: [{ value: itm.ClassID, disabled: true }, [Validators.required]],
      Designation: [itm.Designation],
      Department: [itm.Department],
      EmploymentDuration: [itm.EmploymentDuration],
      Salary: [itm.Salary],
      SupervisorName: [itm.SupervisorName],
      SupervisorContact: [itm.SupervisorContact],
      EmploymentStartDate: [moment(itm.EmploymentStartDate)],
      EmploymentStatus: [null],
      //     EmploymentType: [itm.EmploymentType, [Validators.required]],
      EmployerName: [itm.EmployerName],
      EmployerBusinessType: [itm.EmployerBusinessType],
      EmploymentAddress: [itm.EmploymentAddress],
      //District: [this.FilterFindvalue(this.DistrictDrp, itm.District, 'DistrictName', 'DistrictID'), [Validators.required]],
      //EmploymentTehsil: [this.FilterFindvalue(this.TehsilDrp, itm.EmploymentTehsil, 'TehsilName', 'TehsilID'), [Validators.required]],
      //TimeFrom: [moment(itm.TimeFrom == "0" ? null : itm.TimeFrom, "HH:mm").format("hh:mm A"), [Validators.required]],
      //TimeTo: [moment(itm.TimeTo == "0" ? null : itm.TimeTo, "HH:mm").format("hh:mm A"), [Validators.required]],
      EmploymentTiming: [moment(itm.TimeFrom, "HH:mm").format("hh:mm A") + ' to ' + moment(itm.TimeTo, "HH:mm").format("hh:mm A")],
      TimeFrom: [itm.EmploymentTiming.split(" to ")[0]],
      TimeTo: [itm.EmploymentTiming.split(" to ")[1]],
      OfficeContactNo: [itm.OfficeContactNo],

      TraineeName: [{ value: itm.TraineeName, disabled: true }],
      //PlacementTypeID: [this.FilterFindvalue(this.PlacementTypeDrp, itm.PlacementType, 'PlacementType', 'PlacementTypeID'), [Validators.required]],
      //VerificationMethodId: [this.FilterFindvalue(this.VerificationMethodsDrp, itm.VerificationMethod, 'VerificationMethodType', 'VerificationMethodID'), [Validators.required]],
      //PlacementTypeID: [this.FilterFindvalue(this.PlacementTypeDrp, itm.PlacementType, 'PlacementType', 'PlacementTypeID'), [Validators.required]],
      //VerificationMethodId: [this.FilterFindvalue(this.VerificationMethodsDrp, itm.VerificationMethod, 'VerificationMethodType', 'VerificationMethodID'), [Validators.required]],
      EOBI: [itm.EOBI],
      FileType: [{ value: itm.FileType }],
      FileName: [{ value: itm.FileName }],
      FilePath: [''],
      VerificationMethodType: [''],
      Attachment: [''],
      //this.fileName = this.data.FileName;
      //this.filePath = this.data.FilePath;
      //this.fileType = this.data.FileType;
      IsVerified: [itm.IsVerified, Validators.required],
      Comments: [itm.Comments, Validators.required]
    }, { updateOn: "blur" });

    form.get('EmploymentStatus').valueChanges.subscribe((v) => {
      if (v == 'Employed') {
        form.get('Designation').enable();
        form.get('Department').enable();
        form.get('EmploymentDuration').enable();
        form.get('Salary').enable();
        form.get('SupervisorName').enable();
        form.get('SupervisorContact').enable();
        form.get('EmploymentStartDate').enable();
        form.get('TimeTo').enable();
        form.get('TimeFrom').enable();
        form.get('VerificationMethodId').enable();
        form.get('PlacementTypeID').enable();
        form.get('EmployerName').enable();
        form.get('EmployerBusinessType').enable();
        form.get('EmploymentAddress').enable();
        //form.get('District').enable();
        form.get('EmploymentTehsil').enable();
        form.get('EmploymentTiming').enable();
        form.get('OfficeContactNo').enable();
        form.get('EOBI').enable();
        //form.get('EmploymentStatus').enable();
        form.get('FileName').enable();
        form.get('FilePath').enable();
        form.get('FileType').enable();
        form.get('TradeName').enable();
        form.get('TraineeID').enable();


      }
      else {
        form.get('Designation').disable();
        form.get('Department').disable();
        form.get('EmploymentDuration').disable();
        form.get('Salary').disable();
        form.get('SupervisorName').disable();
        form.get('SupervisorContact').disable();
        form.get('EmploymentStartDate').disable();
        form.get('TimeTo').disable();
        form.get('TimeFrom').disable();
        form.get('VerificationMethodId').disable();
        form.get('PlacementTypeID').disable();
        form.get('EmployerName').disable();
        form.get('EmployerBusinessType').disable();
        form.get('EmploymentAddress').disable();
        form.get('District').disable();
        form.get('EmploymentTehsil').disable();
        form.get('EmploymentTiming').disable();
        form.get('OfficeContactNo').disable();
        form.get('EOBI').disable();
        //form.get('EmploymentStatus').disable();
        form.get('FileName').disable();
        form.get('FilePath').disable();
        form.get('FileType').disable();
        form.get('TradeName').disable();
        form.get('TraineeID').disable();
      }
      for (const field in form.controls) { // 'field' is a string

        form.get(field).updateValueAndValidity({ emitEvent: false }); // 'control' is a FormControl  
      }
      form.updateValueAndValidity({ emitEvent: false });
      //  this.traineeEmploymentForm.updateValueAndValidity({ emitEvent: false });
    });
    form.get('EmploymentStatus').setValue(itm.EmploymentStatus);
    form.get('FilePath').setValue(itm.FilePath);
    form.get('TraineeName').setValue(itm.TraineeName);
    form.get('VerificationMethodType').setValue(itm.VerificationMethodType);
    return form
  }
  onNoClick(): void {
    this.dialogRef.close(false);
  }
  Submit(nform: FormGroupDirective) {
    if (this.traineeEmploymentForm.invalid) {
      this.scrollToFirstInvalidControl();
      return;
    }
    this.ComSrv.postJSON('api/EmploymentVerification/SaveBulk', this.Detail.getRawValue())// { 'json': JSON.stringify(dataObj) })
      .subscribe((d: any) => {
        nform.resetForm();
        this.update = "Data Submitted successfully";
        this.ComSrv.openSnackBar(this.update.toString(), "Updated");
        this.dialogRef.close(true);
      },
        error => this.ComSrv.ShowError(error, this.EnText) // error path
      );
  }
  private scrollToFirstInvalidControl() {
    for (const key of Object.keys(this.Detail.controls)) {
      for (const key1 of Object.keys(this.Detail.controls[key].controls)) {
        if (this.Detail.controls[key].controls[key1].invalid) {
          const invalidControl = this.el.nativeElement.querySelector('[formcontrolname="' + key1 + '"]');
          invalidControl.focus();
          break;
        }
      }
    }
  }
  reset(nform: FormGroupDirective) {
    nform.resetForm();
  }
  getData() {
    if (this.data[0].ClassID) {
      this.ComSrv.postJSON('api/TSPEmployment/GetEmploymentDataForVerification', { "ClassID": this.data[0].ClassID }).subscribe((d: any) => {
        console.log(d);
        this.EmploymentData = d.EmploymentData;
        this.data.forEach((trainee) =>
          this.Detail.push(this.GenerateForm(trainee)));
        this.traineeEmploymentForm.updateValueAndValidity();
      }, error => this.ComSrv.ShowError(error) // error path
      );
    }
    else {
      this.ComSrv.postJSON('api/PSPEmployment/GetEmploymentDataForVerification', { "PSPBatchID": this.data[0].PSPBatchID }).subscribe((d: any) => {
        console.log(d);
        this.EmploymentData = d.EmploymentData;
        this.data.forEach((trainee) =>
          this.Detail.push(this.GenerateForm(trainee)));
        this.traineeEmploymentForm.updateValueAndValidity();
      }, error => this.ComSrv.ShowError(error) // error path
      );

    }
  }

  get ID() { return this.traineeEmploymentForm.get("ID"); }
  get Detail() { return this.traineeEmploymentForm.get("Detail") as FormArray; }
  get Attachment() { return this.traineeEmploymentForm.get("Attachment"); }
  get VerificationMethodType() { return this.traineeEmploymentForm.get("VerificationMethodType"); }
  get FilePath() { return this.traineeEmploymentForm.get("FilePath"); }

  onVerificarionMethodChange(event, form: FormGroup) {
    //if (form.get('PlacementTypeID').value == 2 && form.get('VerificationMethodId').value == 7)
    //  form.get('FilePath').clearValidators();
    //else
    //  form.get('FilePath').setValidators(Validators.required);


    //form.get('FilePath').updateValueAndValidity();
    //this.traineeEmploymentForm.updateValueAndValidity();
  }

}
export class TspTrainees {

  PlacementID: number;
  TraineeID: number;
  TraineeName: string;
  ClassName: string;
  ClassID: number;
  TradeName: string;
  Designation: string;
  Department: string;
  EmploymentDuration: number;
  Salary: number;
  SupervisorName: string;
  SupervisorContact: string;
  EmploymentStartDate: Date;
  TimeFrom: string;
  TimeTo: string;
  EmploymentStatus: string;
  EmploymentType: number;
  EmploymentStatusName: string;
  EmploymentTypeName: string;
  EmployerName: string;
  EmployerBusinessType: string;
  EmploymentAddress: string;
  District: number;

  DistrictName: string;
  EmploymentTehsil: string;
  EmploymentTiming: string;
  EmploymentTehsilOld: string;
  OfficeContactNo: string;
  VerificationMethodId: number;
  EOBI: string;
  PlacementType: string
  PlacementTypeID: string;
  VerificationMethod: string;
  IsVerified: boolean
  Comments: string;
  PSPBatchID: number;


}


