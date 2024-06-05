import { Component, OnInit, Inject } from '@angular/core';
import { CommonSrvService } from '../../common-srv.service';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-t-status-history-dialogue',
  templateUrl: './t-status-history-dialogue.component.html',
  styleUrls: ['./t-status-history-dialogue.component.scss']
})
export class TStatusHistoryDialogueComponent implements OnInit {
  history: any[]=[];
  constructor(private http: CommonSrvService, public dialogRef: MatDialogRef<TStatusHistoryDialogueComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any) { }

  ngOnInit(): void {
    this.getHistory();
  }
  getHistory() {
    ///data object must be a traineeID
    this.http.getJSON("api/TraineeProfile/GetTraineeStatusHistory/" + this.data.TraineeID).subscribe(
      (data: any[]) => {
        //let currentUser = this.http.getUserDetails();
        this.history = data;
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
