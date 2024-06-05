import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CommonSrvService } from '../../../common-srv.service';
import { environment } from '../../../../environments/environment';
import { UsersModel } from '../../../master-data/users/users.component';
import { EnumTraineeStatusType, EnumUserLevel } from '../../../shared/Enumerations';

@Component({
  selector: 'app-csu-dialogue',
  templateUrl: './csu-dialogue.component.html',
  styleUrls: ['./csu-dialogue.component.scss']
})
export class CsuDialogueComponent implements OnInit {
  error: String;
  statusData: any[];
  kamAssignmentTSPs: any[] = [];
  currentUser: UsersModel;
  csrForm: FormGroup;
  EnText: string = "Class Status Update";
  constructor(private ComSrv: CommonSrvService,
    private fb: FormBuilder,
    public dialogRef: MatDialogRef<CsuDialogueComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any) {

    this.csrForm = this.fb.group({
      ClassID: this.fb.control(data.ClassID),
      ClassCode: this.fb.control({ value: data.ClassCode, disabled: true }),
      SchemeName: this.fb.control({ value: data.SchemeName, disabled: true }),
      TSPName: this.fb.control({ value: data.TSPName, disabled: true }),
      ClassStatusID: this.fb.control(data.ClassStatusID, Validators.required),
    })

    this.statusData = this.data.statusTypeData;
  }

  ngOnInit() {
    this.currentUser = this.ComSrv.getUserDetails();
    //this.getKAMAssignment();
    this.GetData();
  }

  GetData() {
    this.ComSrv.getJSON('api/ClassStatus/RD_ClassStatus').subscribe((d: any) => {
      this.statusData = d;
      console.log('data is  :   ' + d);
    }, error => this.error = error // error path
    );
  }

  onNoClick(): void {
    this.dialogRef.close(false);
  }
  onSaveClick(): void {
    if (this.csrForm.valid) {
      let statusName = this.statusData.find(x => x.ClassStatusID == this.ClassStatusID.value).ClassStatusName;
      this.dialogRef.close({ ...(this.csrForm.getRawValue()), StatusName: statusName });
    }
  }


  get ClassStatusID() { return this.csrForm.get("ClassStatusID"); }
  get ClassID() { return this.csrForm.get("ClassID"); }
  //getKAMAssignment() {
  //  this.ComSrv.getJSON('api/KAMAssignment/RD_KAMAssignmentBy').subscribe(
  //    (data: any) => {
  //      this.kamAssignmentTSPs = data.filter(x => x.InActive == false).map(x => x.TspID);
  //      //this.kamAssignmentTSPs.includes(1)
  //      //debugger;
  //    }
  //    , (error) => this.error = error
  //  );
  //}

  checkStatusTypesSelection(option: any) {
    return false;
    //let isInternalUser = this.currentUser.UserLevel == EnumUserLevel.AdminGroup || this.currentUser.UserLevel == EnumUserLevel.OrganizationGroup;
    //let isKAMUser = isInternalUser && this.kamAssignmentTSPs.includes(this.data.TSPID);
    //let isTSPUser = this.currentUser.UserLevel == EnumUserLevel.TSP;

    //if (isTSPUser) {
    //  if (option == EnumTraineeStatusType.Expelled) {
    //    ///make enabled
    //    return false;
    //  } else {
    //    //make disabled
    //    return true;
    //  }
    //} else if (isKAMUser) {
    //  if (option == EnumTraineeStatusType.Expelled || option == EnumTraineeStatusType.DropOut || option == EnumTraineeStatusType.OnRoll) {
    //    ///make enabled
    //    return false;
    //  } else {
    //    //make disabled
    //    return true;
    //  }
    //}
  }
}
