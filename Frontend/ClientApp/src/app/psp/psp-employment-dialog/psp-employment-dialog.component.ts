/* **** Aamer Rehman Malik *****/
import { Component, Inject, OnInit, ElementRef } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, FormGroupDirective, Validators, ValidatorFn } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { CommonSrvService } from 'src/app/common-srv.service';
import * as moment from 'moment';

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
  selector: 'app-psp-employment-dialog',
  templateUrl: './psp-employment-dialog.component.html',
  styleUrls: ['./psp-employment-dialog.component.scss']
})
export class PSPEmploymentDialogComponent implements OnInit {
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
  constructor(private fb: FormBuilder, private el: ElementRef, private ComSrv: CommonSrvService, private dialogRef: MatDialogRef<PSPEmploymentDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: Array<PspTrainees>) {
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
  GenerateForm(itm: PspTrainees) {
    let form: FormGroup = this.fb.group({
      PlacementID: 0,//this.FilterFindvalue(this.EmploymentData, itm.TraineeID, 'TraineeID', 'PlacementID') ?? 0,

      TraineeID: [itm.TraineeID, [Validators.required]],
      TradeName: ["NA", [Validators.required]],
      //ClassID: [{ value: itm.ClassID, disabled: true }, [Validators.required]],
      Designation: [itm.Designation, [Validators.required]],
      Department: [itm.Department, [Validators.required]],
      EmploymentDuration: [itm.EmploymentDuration, [Validators.required]],
      Salary: [itm.Salary, [Validators.required]],
      SupervisorName: [itm.SupervisorName, [Validators.required]],
      SupervisorContact: [itm.SupervisorContact, [Validators.required]],
      EmploymentStartDate: [moment(itm.EmploymentStartDate), [Validators.required]],
      EmploymentStatus: [null, [Validators.required]],
      //     EmploymentType: [itm.EmploymentType, [Validators.required]],
      EmployerName: [itm.EmployerName, [Validators.required]],
      EmployerBusinessType: [itm.EmployerBusinessType, [Validators.required]],
      EmploymentAddress: [itm.EmploymentAddress, [Validators.required]],
      District: [this.FilterFindvalue(this.DistrictDrp, itm.District, 'DistrictName', 'DistrictID'), [Validators.required]],
      EmploymentTehsil: [this.FilterFindvalue(this.TehsilDrp, itm.EmploymentTehsil, 'TehsilName', 'TehsilID'), [Validators.required]],
      TimeFrom: [moment(itm.TimeFrom == "0" ? null : itm.TimeFrom, "HH:mm").format("hh:mm A"), [Validators.required]],
      TimeTo: [moment(itm.TimeTo == "0" ? null : itm.TimeTo, "HH:mm").format("hh:mm A"), [Validators.required]],
      EmploymentTiming: [moment(itm.TimeFrom, "HH:mm").format("hh:mm A") + ' to ' + moment(itm.TimeTo, "HH:mm").format("hh:mm A")],

      OfficeContactNo: [itm.OfficeContactNo, [Validators.required]],

      TraineeName: [{ value: itm.TraineeName, disabled: true }, [Validators.required]],
      PlacementTypeID: [this.FilterFindvalue(this.PlacementTypeDrp, itm.PlacementType, 'PlacementType', 'PlacementTypeID'), [Validators.required]],
      VerificationMethodId: [this.FilterFindvalue(this.VerificationMethodsDrp, itm.VerificationMethod, 'VerificationMethodType', 'VerificationMethodID'), [Validators.required]],
      EOBI: [itm.EOBI],
      FileType: [''],
      FileName: [''],
      FilePath: [''],
      //Attachment: [''],
      //IsVerified: ["", Validators.required],
      //Comments: ["", Validators.required]
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
        form.get('District').enable();
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

    this.ComSrv.postJSON('api/TSPEmployment/SaveExcelFile', this.Detail.getRawValue())// { 'json': JSON.stringify(dataObj) })
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
    //const firstInvalidControl: HTMLElement = this.el.nativeElement.querySelector(
    //  "form .ng-invalid input"
    //);

    // firstInvalidControl.focus(); //without smooth behavior
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
    this.ComSrv.postJSON('api/PSPEmployment/GetData', { "TraineeID": this.data[0].TraineeID }).subscribe((d: any) => {
      console.log(d);
      this.ClassDrp = d.Class;

      this.PlacementTypeDrp = d.PlacementTypes;
      this.PlacementStatusDrp = d.PlacementStatus;
      this.VerificationMethods = d.VerificationMethods;
      this.VerificationMethodsDrp = d.VerificationMethods;
      this.DistrictDrp = d.District;
      this.TehsilDrp = d.Tehsil;
      this.EmploymentData = d.EmploymentData;
      this.OrgConfig = d.OrgConfig;
      if (d.DeadlineStatus == "Date Passed") {
        this.ComSrv.ShowError("Can't submit the request. Deadline end", "Error");
        this.DeadlineEnd = true;
      }
      this.data.forEach((trainee) =>
        this.Detail.push(this.GenerateForm(trainee)));
      this.traineeEmploymentForm.updateValueAndValidity();
    }, error => this.ComSrv.ShowError(error) // error path
    );
  }

  get ID() { return this.traineeEmploymentForm.get("ID"); }
  get Detail() { return this.traineeEmploymentForm.get("Detail") as FormArray; }
  get Attachment() { return this.traineeEmploymentForm.get("Attachment"); }

  onVerificarionMethodChange(event, form: FormGroup) {
    //if (form.get('PlacementTypeID').value == 2 && form.get('VerificationMethodId').value == 7)
    //  form.get('FilePath').clearValidators();
    //else
    //  form.get('FilePath').setValidators(Validators.required);


    //form.get('FilePath').updateValueAndValidity();
    //this.traineeEmploymentForm.updateValueAndValidity();
  }

}
export class PspTrainees {

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
  PlacementType:string
  PlacementTypeID: string;
  VerificationMethod: string;
  IsVerified: boolean
  Comments: string
 
}


