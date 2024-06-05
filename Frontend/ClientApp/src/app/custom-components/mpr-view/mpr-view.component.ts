/* **** Aamer Rehman Malik *****/
import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { CommonSrvService } from 'src/app/common-srv.service';
import { environment } from '../../../environments/environment';

@Component({
  selector: 'app-mpr-view',
  templateUrl: './mpr-view.component.html',
  styleUrls: ['./mpr-view.component.scss']
})
export class MPRviewComponent implements OnInit {
  environment = environment;
  mpr: any[] = [];

  constructor(private http: CommonSrvService, public dialogRef: MatDialogRef<MPRviewComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any) { }

  ngOnInit(): void {
    //this.getHistory();
    this.mpr = this.data.mpr;
  };
  getHistory() {
    ///data object must be a traineeID
    this.http.postJSON("api/MPR/GetClassMonthview/", this.data).subscribe(
      (data: []) => {
        //let currentUser = this.http.getUserDetails();
        let d = data;
        this.mpr = d['Table'];
        this.mpr.forEach((it) => {
          it.mprDetails = JSON.parse(it.mprDetails);
        });
        //this.srn = d['Table1'];
        //this.srn.forEach((it) => {
        //  it.srnDetails = JSON.parse(it.srnDetails);
        //});
        //this.PRNMaster = d['Table2'];
        //this.PRNMaster.forEach((it) => {
        //  it.PRN = JSON.parse(it.PRN);
        //});
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
