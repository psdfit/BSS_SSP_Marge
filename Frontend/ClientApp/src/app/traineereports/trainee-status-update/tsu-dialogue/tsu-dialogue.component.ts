import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CommonSrvService } from '../../../common-srv.service';
import { environment } from '../../../../environments/environment';
import { UsersModel } from '../../../master-data/users/users.component';
import { EnumTraineeStatusType, EnumUserLevel } from '../../../shared/Enumerations';

@Component({
  selector: 'app-tsu-dialogue',
  templateUrl: './tsu-dialogue.component.html',
  styleUrls: ['./tsu-dialogue.component.scss']
})
export class TsuDialogueComponent implements OnInit {
  error: String;
  statusData: any[];
  reasonData: any[];
  //RowTraineeID: any;
  kamAssignmentTSPs: any[] = [];
  currentUser: UsersModel;
  tsrForm: FormGroup;
  EnText: string = "Trainee Status Update";
  constructor(private ComSrv: CommonSrvService,
    private fb: FormBuilder,
    public dialogRef: MatDialogRef<TsuDialogueComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any) {

    //this.RowTraineeID = data.row.TraineeID;
    this.tsrForm = this.fb.group({
      TraineeID: this.fb.control(data.TraineeID),
      TraineeCode: this.fb.control({ value: data.TraineeCode, disabled: true }),
      TraineeName: this.fb.control({ value: data.TraineeName, disabled: true }),
      TraineeCNIC: this.fb.control({ value: data.TraineeCNIC, disabled: true }),
      //TraineeCNIC: data.TraineeCNIC,
      //ContactNumber1: this.fb.control(data.ContactNumber1),
      TraineeStatusTypeID: this.fb.control(data.TraineeStatusTypeID, Validators.required),
      TraineeReason: this.fb.control(data.TraineeStatusChangeReason, Validators.required),
      ContactNumber1: this.fb.control('', Validators.required),
    })
    console.log(data)

    //this.TraineeID.setValue(data.row.TraineeID);
     this.statusData = this.data.statusTypeData;
        if (data.TraineeStatusTypeID == 1 || data.TraineeStatusTypeID == 2) {
          this.TraineeReason.setValue('');
          this.TraineeReason.clearValidators();
          this.TraineeReason.disable();
        }
        else {
          this.TraineeReason.setValidators([Validators.required]);
          this.TraineeReason.updateValueAndValidity();
          this.TraineeReason.enable();
    }
    if (data.TraineeStatusChangeReason == 'Others') {
        this.ContactNumber1.setValue('');
        this.ContactNumber1.setValidators([Validators.required]);
        this.ContactNumber1.updateValueAndValidity();
        this.ContactNumber1.enable();
      }
      else {
        this.ContactNumber1.setValue(' ');
        this.ContactNumber1.clearValidators();
        this.ContactNumber1.disable();
      }
  }

  ngOnInit() {
    this.currentUser = this.ComSrv.getUserDetails();
    this.getKAMAssignment();
    this.GetData();
    this.GetDataTraineeReason();
  }

  onTraineeStatusChange(event) {
    if (event.value == 1 || event.value == 2) {
      this.TraineeReason.setValue('');
      this.TraineeReason.clearValidators();
      this.TraineeReason.disable();
    }
    else {
      this.TraineeReason.setValue('')
      this.TraineeReason.setValidators([Validators.required]);
      this.TraineeReason.updateValueAndValidity();
      this.TraineeReason.enable();
    }
  }


  onTraineeReasonChange(event) {
    if (event.value == 'Others') {
      this.ContactNumber1.setValue('');
      this.ContactNumber1.setValidators([Validators.required]);
      this.ContactNumber1.updateValueAndValidity();
      this.ContactNumber1.enable();
    }
    else {
      this.ContactNumber1.setValue(' ');
      this.ContactNumber1.clearValidators();
      this.ContactNumber1.disable();
    }
  }

  GetData() {
    this.ComSrv.getJSON('api/TraineeStatusTypes/GetTraineeStatusTypes').subscribe((d: any) => {
      this.statusData = d;
      console.log('data is  :   ' + d);
    }, error => this.error = error // error path
    );
  }

  GetDataTraineeReason() {
    this.ComSrv.getJSON('api/TraineeStatusTypes/RD_TraineeStatusReason').subscribe((d: any) => {
      this.reasonData = d;
      console.log('data is  :   ' + d);
    }, error => this.error = error // error path
    );
  }

  onNoClick(): void {
    this.dialogRef.close(false);
  }
  onSaveClick(): void {
    console.log('test')
    if (this.tsrForm.valid) {
    console.log('test2')

      if (this.TraineeReason.value == 'Others') {
        if (this.ContactNumber1.value != "") {
          this.TraineeReason.setValue(this.ContactNumber1.value);
        }
        else {
          return;
        }
      }
      let statusName = this.statusData.find(x => x.TraineeStatusTypeID == this.TraineeStatusTypeID.value).StatusName;
      this.dialogRef.close({ ...(this.tsrForm.getRawValue()), StatusName: statusName });
    }
  }

  //Submit() {
  //  this.ComSrv.postJSON('api/TSRLiveData/UpdateTraineeStatus', this.tsrForm.value)
  //    .subscribe((d: any) => {
  //      this.ComSrv.openSnackBar(this.TraineeID.value > 0 ? environment.UpdateMSG.replace("${Name}", this.EnText) : environment.SaveMSG.replace("${Name}", this.EnText));
  //      this.reset();
  //      //this.title = "Add New ";
  //      //this.savebtn = "Save ";
  //    },
  //    );
  //}


  get TraineeStatusTypeID() { return this.tsrForm.get("TraineeStatusTypeID"); }
  get TraineeReason() { return this.tsrForm.get("TraineeReason"); }
  get TraineeID() { return this.tsrForm.get("TraineeID"); }
  get ContactNumber1() { return this.tsrForm.get("ContactNumber1");}
  //get TraineeReasonOther() { return this.tsrForm.get("TraineeReason"); }
  //get TraineeCNIC() { return this.tsrForm.get("TraineeCNIC"); }
  //get TraineeName() { return this.tsrForm.get("TraineeName"); }
  //get TraineeCode() { return this.tsrForm.get("TraineeCode"); }
  //get ContactNumber1() { return this.tsrForm.get("ContactNumber1"); }
  getKAMAssignment() {
    this.ComSrv.getJSON('api/KAMAssignment/RD_KAMAssignmentBy').subscribe(
      (data: any) => {
        this.kamAssignmentTSPs = data.filter(x => x.InActive == false).map(x => x.TspID);
        //this.kamAssignmentTSPs.includes(1)
        //debugger;
      }
      , (error) => this.error = error
    );
  }
  checkStatusTypesSelection(option: any) {
    console.log(option)
    let isInternalUser = this.currentUser.UserLevel == EnumUserLevel.AdminGroup || this.currentUser.UserLevel == EnumUserLevel.OrganizationGroup;
    let isKAMUser = isInternalUser && this.kamAssignmentTSPs.includes(this.data.TSPID);
    let isTSPUser = this.currentUser.UserLevel == EnumUserLevel.TSP;

    if (isTSPUser) {
      if (option == EnumTraineeStatusType.Expelled) {
        if (this.TraineeStatusTypeID.value == EnumTraineeStatusType.Completed || this.TraineeStatusTypeID.value == EnumTraineeStatusType.OnRoll) {
          ///make enabled
          return false;
        } else {
          //make disabled
          return true;
        }
      } else {
        //make disabled
        return true;
      }
    } else if (isKAMUser) {//|| option == EnumTraineeStatusType.DropOut || option == EnumTraineeStatusType.OnRoll
      if (option == EnumTraineeStatusType.Expelled) {
    console.log('kam')

        ///make enabled
        return false;
      } else {
        //make disabled
        return true;
      }
    }
    else if (isInternalUser) {
    console.log('internal')

      if (option == EnumTraineeStatusType.Expelled) {

        if (this.TraineeStatusTypeID.value == EnumTraineeStatusType.EnRoll || this.TraineeStatusTypeID.value == EnumTraineeStatusType.Completed || this.TraineeStatusTypeID.value == EnumTraineeStatusType.OnRoll) {
          ///make enabled
          return false;
        } else {
          //make disabled
          return true;
        }

      } else {
        //make disabled
        return true;
      }
    }
  }
}

