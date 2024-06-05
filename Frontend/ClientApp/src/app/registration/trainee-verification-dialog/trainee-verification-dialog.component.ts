import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ITraineeProfile } from '../Interface/ITraineeProfile';
import { FormGroup, FormBuilder, FormControl } from '@angular/forms';
import { EnumTraineeUnVerifiedReason } from '../../shared/Enumerations';
import { CommonSrvService } from '../../common-srv.service';

@Component({
  selector: 'trainee-varification-dialog',
  templateUrl: './trainee-verification-dialog.component.html',
  styleUrls: ['./trainee-verification-dialog.component.scss']
})
export class TraineeVerificationDialogComponent implements OnInit {
  verificationForm: FormGroup;
  unverfiedReason = EnumTraineeUnVerifiedReason;
  constructor(private http: CommonSrvService, public dialogRef: MatDialogRef<TraineeVerificationDialogComponent>, private fb: FormBuilder, @Inject(MAT_DIALOG_DATA) public data: ITraineeProfile) {
    //console.log(data);
    this.verificationForm = this.fb.group(
      {
        TraineeID: this.fb.control({ value: '' }),
        TraineeCode: this.fb.control({ value: '', disabled: true }),
        TraineeName: this.fb.control({ value: '', disabled: true }),
        TraineeCNIC: this.fb.control({ value: '', disabled: true }),
        DateOfBirth: this.fb.control({ value: '', disabled: true}),
        AgeVerified: this.fb.control({ value: false }),
        AgeUnVerifiedReason: this.fb.control({ value: '' }),
        DistrictVerified: this.fb.control({ value: false }),
        ResidenceUnVerifiedReason: this.fb.control({ value: '' }),
        CNICVerified: this.fb.control({ value: false }),
        CNICUnVerifiedReason: this.fb.control({ value: '' }),
      });
    this.AgeVerified.valueChanges.subscribe(
      value => {
        this.onChangeAgeVerified(value);
      });
    this.DistrictVerified.valueChanges.subscribe(
      value => {
        this.onChangeDistrictVerified(value);
      });
    this.CNICVerified.valueChanges.subscribe(
      value => {
        this.onChangeCNICVerified(value);
      });
  }

  ngOnInit() {
    this.verificationForm.patchValue(this.data);
    this.onChangeAgeVerified(this.data.AgeVerified);
    this.onChangeDistrictVerified(this.data.DistrictVerified);
    this.onChangeCNICVerified(this.data.CNICVerified);
  }

  onNoClick(): void {
    this.dialogRef.close(false);
  }
  onSaveClick(): void {
    if (this.verificationForm.valid) {
      if (!this.AgeVerified.value || !this.DistrictVerified.value) {
        let titleConfirm = "Are you sure?";
        let messageConfirm = "Trainee will be Expelled, if either 'age is not verified' or 'district is not verified'.";
        this.http.confirm(titleConfirm, messageConfirm).subscribe(
          (isConfirmed: Boolean) => {
            if (isConfirmed) { this.save(); }
          });
      } else {
        this.save();
      }
    }
  }
  save() {
    this.AgeUnVerifiedReason.patchValue(this.AgeVerified ? this.AgeUnVerifiedReason.value : this.unverfiedReason.Age);
    this.ResidenceUnVerifiedReason.patchValue(this.DistrictVerified ? this.ResidenceUnVerifiedReason.value : this.unverfiedReason.District);
    this.CNICUnVerifiedReason.patchValue(this.CNICVerified ? this.CNICUnVerifiedReason.value : this.unverfiedReason.CNIC);
    this.dialogRef.close({ ...(this.verificationForm.getRawValue()) });
  }
  onChangeAgeVerified(isChecked: boolean) {
    if (isChecked) {
      this.AgeUnVerifiedReason.setValue(this.data.AgeUnVerifiedReason == this.unverfiedReason.Age ? '' : this.data.AgeUnVerifiedReason)
      this.AgeUnVerifiedReason.enable();
    } else {
      this.AgeUnVerifiedReason.setValue(this.unverfiedReason.Age);
      this.AgeUnVerifiedReason.disable();
    }
  }
  onChangeDistrictVerified(isChecked: boolean) {
    if (isChecked) {
      this.ResidenceUnVerifiedReason.setValue(this.data.ResidenceUnVerifiedReason == this.unverfiedReason.District ? '' : this.data.ResidenceUnVerifiedReason);
      this.ResidenceUnVerifiedReason.enable();
    } else {
      this.ResidenceUnVerifiedReason.setValue(this.unverfiedReason.District);
      this.ResidenceUnVerifiedReason.disable();
    }
  }
  onChangeCNICVerified(isChecked: boolean) {
    if (isChecked) {
      this.CNICUnVerifiedReason.setValue(this.data.CNICUnVerifiedReason == this.unverfiedReason.CNIC ? '' : this.data.CNICUnVerifiedReason);
      this.CNICUnVerifiedReason.enable();
    } else {
      this.CNICUnVerifiedReason.setValue(this.unverfiedReason.CNIC);
      this.CNICUnVerifiedReason.disable();
    }
  }
  ///getter
  get AgeVerified() { return this.verificationForm.get('AgeVerified') };
  get AgeUnVerifiedReason() { return this.verificationForm.get('AgeUnVerifiedReason') };
  get DistrictVerified() { return this.verificationForm.get('DistrictVerified') };
  get ResidenceUnVerifiedReason() { return this.verificationForm.get('ResidenceUnVerifiedReason') };
  get CNICVerified() { return this.verificationForm.get('CNICVerified') };
  get CNICUnVerifiedReason() { return this.verificationForm.get('CNICUnVerifiedReason') };
}
