import { Component, OnInit, Inject } from '@angular/core';
import { CommonSrvService } from '../../common-srv.service';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-cr-trianee-history-dialogue',
  templateUrl: './cr-trianee-history-dialogue.component.html',
  styleUrls: ['./cr-trianee-history-dialogue.component.scss']
})
export class CRTraineeHistoryDialogueComponent implements OnInit {
  traineehistory: any[]=[];
  constructor(private http: CommonSrvService, public dialogRef: MatDialogRef<CRTraineeHistoryDialogueComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any) { }

  ngOnInit(): void {
    this.getHistory();
  }
  getHistory() {
    ///data object must be a traineeID
    this.http.getJSON("api/TraineeProfile/GetTraineeHistory/" + this.data.TraineeID).subscribe(
      (data: any[]) => {
        //let currentUser = this.http.getUserDetails();
        this.traineehistory = data;
      },
      error => {
        this.http.ShowError(error, "Error");
      }
    );
  }
  onNoClick(): void {
    this.dialogRef.close(false);
  }
}
