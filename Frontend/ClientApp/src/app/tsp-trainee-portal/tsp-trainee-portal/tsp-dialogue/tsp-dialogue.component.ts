import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CommonSrvService } from '../../../common-srv.service';
import { UsersModel } from '../../../master-data/users/users.component';

@Component({
  selector: 'app-tsp-dialogue',
  templateUrl: './tsp-dialogue.component.html',
  styleUrls: ['./tsp-dialogue.component.scss']
})
export class TspDialogueComponent implements OnInit {
  tsrForm: FormGroup;
  currentUser: UsersModel;
  statusData: any[];
  reasonData: any[];
  error: String;

  constructor(
    private ComSrv: CommonSrvService,
    private fb: FormBuilder,
    public dialogRef: MatDialogRef<TspDialogueComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    this.tsrForm = this.fb.group({
      TraineeName: this.fb.control({ value: data.TraineeName, disabled: true }),
      ProgramName: this.fb.control({ value: data.ProgramName, disabled: true }),
      DistrictName: this.fb.control({ value: data.DistrictName, disabled: true }),
      TraineeReason: this.fb.control(data.TraineeStatusChangeReason, Validators.required),
      Rejectionreason: this.fb.control('', Validators.required)
    });
  }

  ngOnInit() {
    this.currentUser = this.ComSrv.getUserDetails();
    this.GetData();
    this.GetDataTraineeReason();

    // Bind the change event to the method
    this.tsrForm.get('TraineeReason').valueChanges.subscribe(value => {
      this.onTraineeReasonChange({ value });
    });

    // Initialize the Rejectionreason field based on initial value
    this.onTraineeReasonChange({ value: this.tsrForm.get('TraineeReason').value });
  }

  onTraineeReasonChange(event: any) {
    if (event.value === 'Release' || event.value === 'Reject') {
      this.tsrForm.get('Rejectionreason').setValidators([Validators.required]);
      this.tsrForm.get('Rejectionreason').enable();
    } else {
      this.tsrForm.get('Rejectionreason').clearValidators();
      this.tsrForm.get('Rejectionreason').disable();
    }
    this.tsrForm.get('Rejectionreason').updateValueAndValidity();
  }

  async save() {
    debugger;
    if (this.tsrForm.invalid) {
      this.tsrForm.markAllAsTouched();
      return;
    }

    let titleConfirm = "Confirmation";
    let messageConfirm = "";

    const traineeStatus = this.tsrForm.get('TraineeReason').value;

    // Determine the message and action based on traineeStatus
    switch (traineeStatus) {
      case 'Accept':
        messageConfirm = "Are you sure you want to submit this Trainee?";
        break;
      case 'Release':
        messageConfirm = "Are you sure you want to release this trainee?";
        break;
      case 'Reject':
        messageConfirm = "Are you sure you want to reject this trainee?";
        break;
      default:
        // Handle default case or throw an error if needed
        break;
    }

    const isConfirm = await this.ComSrv.confirm(titleConfirm, messageConfirm).toPromise();
    if (isConfirm) {
      // Construct the payload with TraineeID property
      const payload = {
        TraineeID: this.data.TraineeID,
        TraineeIntrestStatus: this.tsrForm.get('TraineeReason').value,
        TraineeStatusChangeReason: this.tsrForm.get('Rejectionreason').value
      };
      try {
        await this.ComSrv.postJSON('api/TraineeProfile/SaveInterview', payload).toPromise();
        this.ComSrv.openSnackBar('Data submitted for final submission successfully:');
        this.dialogRef.close(false);
      } catch (error) {
        this.ComSrv.ShowError('Error submitting data:', error);
      }
    }
  }

  GetData() {
    this.ComSrv.getJSON('api/TraineeStatusTypes/GetTraineeStatusTypes').subscribe((d: any) => {
      this.statusData = d;
    }, error => this.error = error // error path
    );
  }

  GetDataTraineeReason() {
    this.ComSrv.getJSON('api/TraineeStatusTypes/RD_TraineeStatusReason').subscribe((d: any) => {
      this.reasonData = d;
    }, error => this.error = error // error path
    );
  }

  onNoClick(): void {
    this.dialogRef.close(false);
  }

  get TraineeReason() { return this.tsrForm.get("TraineeReason"); }
  get Rejectionreason() { return this.tsrForm.get("Rejectionreason"); }
}
