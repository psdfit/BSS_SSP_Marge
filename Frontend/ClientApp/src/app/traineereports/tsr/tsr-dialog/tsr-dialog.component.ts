import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CommonSrvService } from '../../../common-srv.service';
import { environment } from '../../../../environments/environment';

@Component({
  selector: 'app-tsr-dialog',
  templateUrl: './tsr-dialog.component.html',
  styleUrls: ['./tsr-dialog.component.scss']
})
export class TSRDialogComponent implements OnInit {
  error: String;
  statusData: any[];
  //RowTraineeID: any;
  tsrForm: FormGroup;
  EnText: string = "Trainee Status Update";
  constructor(private ComSrv: CommonSrvService,
    private fb: FormBuilder,
    public dialogRef: MatDialogRef<TSRDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any) {

    //this.RowTraineeID = data.row.TraineeID;
    this.tsrForm = this.fb.group({
      TraineeID: this.fb.control(data.TraineeID),
      TraineeCode: this.fb.control({ value: data.TraineeCode, disabled: true }),
      TraineeName: this.fb.control({ value: data.TraineeName, disabled: true }),
      TraineeCNIC: this.fb.control({ value: data.TraineeCNIC, disabled: true }),
      //TraineeCNIC: data.TraineeCNIC,
      //ContactNumber1: data.ContactNumber1,
      TraineeStatusTypeID: this.fb.control(data.TraineeStatusTypeID , Validators.required),
    })

    //this.TraineeID.setValue(data.row.TraineeID);
  }
 
  ngOnInit() {
    this.GetData();
  }

  GetData() {
    this.ComSrv.getJSON('api/TraineeStatusTypes/GetTraineeStatusTypes').subscribe((d: any) => {
      this.statusData = d;
    
    }, error => this.error = error // error path
    );
  }

  onNoClick(): void {
    this.dialogRef.close(false);
  }
  onSaveClick(): void {
    if (this.tsrForm.valid) {
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
  //get TraineeStatus() { return this.tsrForm.get("TraineeStatus"); }
  get TraineeID() { return this.tsrForm.get("TraineeID"); }
  //get TraineeCNIC() { return this.tsrForm.get("TraineeCNIC"); }
  //get TraineeName() { return this.tsrForm.get("TraineeName"); }
  //get TraineeCode() { return this.tsrForm.get("TraineeCode"); }
  //get ContactNumber1() { return this.tsrForm.get("ContactNumber1"); }

}
