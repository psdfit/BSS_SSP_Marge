import { Component, Inject, OnInit } from '@angular/core';
import { CommonSrvService } from 'src/app/common-srv.service';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';


@Component({
  selector: 'app-TSPColorHistory-dialogue.component',//TSPColorHistorydialogueComponent
  templateUrl: './TSPColorHistory-dialogue.component.html',
  styleUrls: ['./TSPColorHistory-dialogue.component.scss']
})
export class TSPColorHistorydialogueComponent implements OnInit {
  TSPColorHistory: any[]=[];
  constructor(private http: CommonSrvService, public dialogRef: MatDialogRef<TSPColorHistorydialogueComponent>,
  @Inject(MAT_DIALOG_DATA) public data: any) { }
  ngOnInit(): void {
    this.getHistory();
  }
  getHistory() {
    this.http.getJSON("api/TSPColor/GetTSPColorHistory/" + this.data.TSPMasterID).subscribe(
      (data: any) => {
        debugger;
        this.TSPColorHistory = data;
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

