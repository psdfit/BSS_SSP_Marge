import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CommonSrvService } from '../../../common-srv.service';
import { environment } from '../../../../environments/environment';
import { UsersModel } from '../../../master-data/users/users.component';
import { EnumClassStatus, EnumUserLevel, EnumUserRoles } from '../../../shared/Enumerations';
@Component({
  selector: 'app-class-status-dialouge',
  templateUrl: './class-status-dialouge.component.html',
  styleUrls: ['./class-status-dialouge.component.scss']
})
export class ClassStatusDialougeComponent implements OnInit {
  error: String;
  statusData: any = [];
  classReason: any[];
  //RowTraineeID: any;
  kamAssignmentTSPs: any[] = [];
  currentUser: UsersModel;
  csuForm: FormGroup;
  EnText: string = "Class Status Update";
  constructor(private ComSrv: CommonSrvService,
    private fb: FormBuilder,
    public dialogRef: MatDialogRef<ClassStatusDialougeComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any) {
    //this.RowTraineeID = data.row.TraineeID;
    this.csuForm = this.fb.group({
      ClassID: this.fb.control(data.ClassID),
      ClassCode: this.fb.control({ value: data.ClassCode, disabled: true }),
      ClassStatusID: this.fb.control(data.ClassStatusID, Validators.required),
      ClassReason: this.fb.control(data.Comments, Validators.required),
      SchemeCode: this.fb.control('', Validators.required),
    })
    //this.TraineeID.setValue(data.row.TraineeID);
    this.statusData = this.data.ClassStatusID;
    if (data.ClassStatusID !== 6 && data.ClassStatusID !== 5) {
      this.ClassReason.setValue('');
      this.ClassReason.clearValidators();
      this.ClassReason.disable();
    }
    else {
      this.ClassReason.setValidators([Validators.required]);
      this.ClassReason.updateValueAndValidity();
      this.ClassReason.enable();
    }
    if (data.Comments == 'Others') {
      this.SchemeCode.setValue('');
      this.SchemeCode.setValidators([Validators.required]);
      this.SchemeCode.updateValueAndValidity();
      this.SchemeCode.enable();
    }
    else {
      this.SchemeCode.setValue(' ');
      this.SchemeCode.clearValidators();
      this.SchemeCode.disable();
    }
  }
  ngOnInit() {
    this.currentUser = this.ComSrv.getUserDetails();
    this.GetData();
    this.GetClassReason();
  }
  onClassStatusChange(event) {
    if (event.value !== 6 && event.value !== 5) {
      this.ClassReason.setValue('');
      this.ClassReason.clearValidators();
      this.ClassReason.disable();
    }
    else {
      this.ClassReason.setValue('');
      this.ClassReason.setValidators([Validators.required]);
      this.ClassReason.updateValueAndValidity();
      this.ClassReason.enable();
    }
  }
  onClassReason(event) {
    if (event.value == 'Others') {
      this.SchemeCode.setValue('');
      this.SchemeCode.setValidators([Validators.required]);
      this.SchemeCode.updateValueAndValidity();
      this.SchemeCode.enable();
    }
    else {
      this.SchemeCode.setValue(' ');
      this.SchemeCode.clearValidators();
      this.SchemeCode.disable();
    }
  }
  GetData() {
    this.ComSrv.getJSON('api/ClassStatus/GetClassStatus').subscribe((d: any) => {
      this.statusData = d;
      console.log(d);
    }, error => this.error = error // error path
    );
  }
  GetClassReason() {
    this.ComSrv.getJSON('api/ClassStatus/GetClassReason').subscribe((d: any) => {
      this.classReason = d;
      console.log('data is  :   ' + d);
    }, error => this.error = error // error path
    );
  }
  onNoClick(): void {
    this.dialogRef.close(false);
  }
  onSaveClick(): void {
    if (this.csuForm.valid) {
      if (this.ClassReason.value == 'Others') {
        if (this.SchemeCode.value != "") {
          this.ClassReason.setValue(this.SchemeCode.value);
        }
        else {
          return;
        }
      }
      let statusName = this.statusData.find(x => x.ClassStatusID == this.ClassStatusID.value).ClassStatusName;
      this.dialogRef.close({ ...(this.csuForm.getRawValue()), StatusName: statusName });
    }
  }
  get ClassStatusID() { return this.csuForm.get("ClassStatusID"); }
  get ClassID() { return this.csuForm.get("ClassID"); }
  get ClassReason() { return this.csuForm.get("ClassReason"); }
  get SchemeCode() { return this.csuForm.get("SchemeCode"); }
  checkStatusTypesSelection(option: any) {
    let isInternalUser = this.currentUser.UserLevel == EnumUserLevel.AdminGroup || this.currentUser.UserLevel == EnumUserLevel.OrganizationGroup;
    if (this.currentUser.RoleID != 1) {
      if (isInternalUser) {
        if (option == EnumClassStatus.Cancelled) {
          if (this.ClassStatusID.value == EnumClassStatus.Active || this.ClassStatusID.value == EnumClassStatus.Ready) {
            ///make enabled
            return false;
          } else {
            //make disabled
            return true;
          }
        }
        else if (option == EnumClassStatus.Abandoned) {
          if (this.ClassStatusID.value == EnumClassStatus.Planned) {
            ///make enabled
            return false;
          } else {
            //make disabled
            return true;
          }
        }
        else {
          //make disabled
          return true;
        }
      }
    } else {
      if (this.currentUser.UserLevel == EnumUserLevel.AdminGroup) {
        if (option == EnumClassStatus.Cancelled || option == EnumClassStatus.Abandoned || option == EnumClassStatus.Active || option == EnumClassStatus.Planned || option == EnumClassStatus.Ready ) {
          return true;
        } else {
          return false;
        }
      }
    }
  }
}
