/* **** Aamer Rehman Malik *****/
import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { CommonSrvService } from 'src/app/common-srv.service';
import { environment } from '../../../environments/environment';

@Component({
  selector: 'document-dialog',
  templateUrl: './document-dialog.component.html',
  styleUrls: ['./document-dialog.component.scss']
})
export class DocumentDialogComponent implements OnInit {
  environment = environment;
  mpr: any[] = [];
  srn: any[] = [];
  gurn: any = [];
  PRNMaster: any[] = [];
  PO: any[] = [];
  Inv: any[] = [];
  constructor(private http: CommonSrvService, public dialogRef: MatDialogRef<DocumentDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any) { }

  ngOnInit(): void {
    this.getHistory();
  };
  getHistory() {
    if (this.data.Col == 'MPR')
      this.GetMPR();
    else if (this.data.Col == 'SRN')
      this.GetSRN();
    else if (this.data.Col == 'GURN')
      this.GetGURN();
    else if (this.data.Col == 'PRN')
      this.GetPRN();
    else if (this.data.Col == 'PO')
      this.GetPO();
    else if (this.data.Col == 'Inv')
      this.GetInv();
    else if (this.data.Col == 'Inv')
      this.GetInv();
  }
  GetMPR() {
    this.http.getJSON("api/Cancelation/getMPRByID/", this.data.ID).subscribe(
      (data: any) => {
        //let currentUser = this.http.getUserDetails();
        let d = data;
        this.mpr = d;
        this.mpr.forEach((it) => {
          if (it.mprDetails)
            it.mprDetails = JSON.parse(it.mprDetails);

        });

      },
      error => {
        this.http.ShowError(error, "Error");
      }
    );
  }
  GetSRN() {
    this.http.getJSON("api/Cancelation/getSRN/", this.data.ID).subscribe(
      (data: any) => {
        //let currentUser = this.http.getUserDetails();
        let d = data;

        this.srn = d;
        this.srn.forEach((it) => {
          if (it.srnDetails)
            it.srnDetails = JSON.parse('[' + it.srnDetails + ']');

        });

      },
      error => {
        this.http.ShowError(error, "Error");
      }
    );
  }

  GetGURN() {
    this.http.getJSON("api/GURN/GetGURNDetails/", this.data.ID).subscribe(
      (data: any) => {
        if (data && data.length > 0) {
          const firstRow = data[0]; // Use the first record for parent data
          this.gurn = [{
            Month: firstRow[0].ClassStartdateGURNDetail,
            ReportDate: firstRow[0].ClassStartdateGURNDetail,
            NumberOfMonths: this.getNumberOfMonths(firstRow[0].ClassStartdateGURNDetail, firstRow[0].ClassEnddateGURNDetail),
            TSPName: firstRow[0].TSPNameGURNDetail,
            TradeName: firstRow[0].SchemeName,
            ClassCode: firstRow[0].ClassCodeGURNDetail,
            Batch: firstRow[0]?.TraineeCode?.split('-')[3],
            TrainingDistrict: firstRow[0].DistrictName,
            gurnDetails: this.getGurnDetails(firstRow), // Pass as an array if the function expects it
          }];
        } else {
          this.gurn = null; // Handle no data scenario
        }
      },
      (error) => {
        this.http.ShowError(error, "Error");
      }
    );
  }

  // Helper function to calculate months
  getNumberOfMonths(startDate: string, endDate: string): number {
    const start = new Date(startDate);
    const end = new Date(endDate);
    const months = (end.getFullYear() - start.getFullYear()) * 12;
    return months + end.getMonth() - start.getMonth();
  }

  // Helper function to process all rows for the nested table
  getGurnDetails(data: any[]) {
    return data.map((item) => ({
      ProjectName: item.ProjectName,
      ClassStartDate: item.ClassStartdateGURNDetail,
      ClassEndDate: item.ClassEnddateGURNDetail,
      GuruName: item.GuruName,
      GURUCNIC: item.GURUCNIC,
      GURUContactNumber: item.GURUContactNumber,
      TraineeCode: item.TraineeCode,
      TraineeName: item.TraineeName,
      FatherName: item.FatherName,
      TraineeCNIC: item.TraineeCNIC,
      ContactNumber1: item.ContactNumber1,
      ReportId: item.GURNID,
      Amount: item.Amount,
      TokenNumber: "", // Adjust as needed
      TransactionNumber: "", // Adjust as needed
      Comments: "", // Adjust as needed
      IsPaid: false, // Adjust as needed
    }));
  }

  GetPRN() {
    this.http.getJSON("api/Cancelation/getPRN/", this.data.ID).subscribe(
      (data: any) => {
        //let currentUser = this.http.getUserDetails();
        let d = data;

        //console.log(data);

        this.PRNMaster = d;
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
  GetPO() {
    this.http.getJSON("api/Cancelation/getPO/", this.data.ID).subscribe(
      (data: any) => {
        //let currentUser = this.http.getUserDetails();
        let d = data;

        //console.log(d);
        this.PO = d;
        this.PO.forEach((it) => {
          if (it.POLines)
            it.POLines = JSON.parse(it.POLines);

        });
      },
      error => {
        this.http.ShowError(error, "Error");
      }
    );
  }
  GetInv() {
    this.http.getJSON("api/Cancelation/getInv/", this.data.ID).subscribe(
      (data: any) => {
        //let currentUser = this.http.getUserDetails();
        let d = data;

        this.Inv = d;
        this.Inv.forEach((it) => {
          if (it.InvDetail)
            it.InvDetail = JSON.parse(it.InvDetail);
          //console.log(it.InvDetail);
          //console.log(it.InvDetail[0].s[0].t[0].cls);
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
