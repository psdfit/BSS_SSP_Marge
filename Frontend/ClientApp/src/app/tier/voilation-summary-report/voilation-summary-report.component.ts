import { Component, OnInit } from "@angular/core";
import { CommonSrvService } from "../../common-srv.service";

@Component({
  selector: "app-voilation-summary-report",
  templateUrl: "./voilation-summary-report.component.html",
  styleUrls: ["./voilation-summary-report.component.scss"],
})
export class VoilationSummaryReportComponent implements OnInit {
  constructor(private http: CommonSrvService) {}
  error: String;
  Summary: [];
  monthsList: [];
  working: boolean;
  ngOnInit(): void {
    this.GetSummary();
  }
  GetSummary() {
    this.http.getJSON("api/Tier/GetVoilationSummaryReport").subscribe(
      (d: any) => {
        console.log(d);
        this.monthsList = d[0].MonthsList;
        this.Summary = d;
        //this.Summary = d;
        //this.schemes.paginator = this.paginator;
        //this.schemes.sort = this.sort;
      },
      (error) => (this.error = error), // error path
      () => {
        this.working = false;
      }
    );
  }
}
