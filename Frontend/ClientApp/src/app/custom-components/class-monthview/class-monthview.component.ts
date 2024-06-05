/* **** Aamer Rehman Malik *****/
import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { CommonSrvService } from 'src/app/common-srv.service';
import { environment } from '../../../environments/environment';

@Component({
  selector: 'app-class-monthview',
  templateUrl: './class-monthview.component.html',
  styleUrls: ['./class-monthview.component.scss']
})
export class ClassMonthviewComponent implements OnInit {
  environment = environment;
  mpr: any[] = [];
  srn: any[] = [];
  PRNMaster: any[] = [];
  constructor(private http: CommonSrvService, public dialogRef: MatDialogRef<ClassMonthviewComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any) { }

  ngOnInit(): void {
    this.getHistory();
  };
  getHistory() {
    ///data object must be a traineeID
    this.http.postJSON("api/MPR/GetClassMonthview/", this.data).subscribe(
      (data: []) => {
        //let currentUser = this.http.getUserDetails();
        let d = data;
        this.mpr = d['Table'];
        this.mpr.forEach((it) => {
          if (it.mprDetails)
          it.mprDetails = JSON.parse(it.mprDetails);

        });
        this.srn = d['Table1'];
        this.srn.forEach((it) => {
          if (it.srnDetails)
          it.srnDetails = JSON.parse(it.srnDetails);

        });
        this.PRNMaster = d['Table2'];
        this.PRNMaster.forEach((it) => {
          if (it.PRN)
          it.PRN = JSON.parse(it.PRN);

        });
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
