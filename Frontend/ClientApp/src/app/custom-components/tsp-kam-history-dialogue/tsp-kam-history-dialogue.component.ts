import { Component, OnInit, Inject } from '@angular/core';
import { CommonSrvService } from '../../common-srv.service';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-tsp-kam-history-dialogue',
    templateUrl: './tsp-kam-history-dialogue.component.html',
    styleUrls: ['./tsp-kam-history-dialogue.component.scss']
})
export class TSPKAMHistoryDialogueComponent implements OnInit {
  tsphistory: any[]=[];
    constructor(private http: CommonSrvService, public dialogRef: MatDialogRef<TSPKAMHistoryDialogueComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any) { }

  ngOnInit(): void {
    this.getHistory();
  }
  getHistory() {
    ///data object must be a traineeID
      this.http.getJSON("api/KAMAssignment/GetTSPKAMHistory/" + this.data.TspID).subscribe(
      (data: any) => {
        //let currentUser = this.http.getUserDetails();
        this.tsphistory = data;
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
