import { Component, OnInit } from "@angular/core";
import { CommonSrvService } from "../../common-srv.service";

@Component({
  selector: "app-performance-analysis-report",
  templateUrl: "./performance-analysis-report.component.html",
  styleUrls: ["./performance-analysis-report.component.scss"],
})
export class PerformanceAnalysisReportComponent implements OnInit {
  error: String;
  report: [];
  working: boolean;
  constructor(private http: CommonSrvService) {}
  ngOnInit(): void {
    this.GetPerformanceAnalysisReport();
  }
  GetPerformanceAnalysisReport() {
    this.http.getJSON("api/Tier/GetPerformanceAnalysisReport").subscribe(
      (d: any) => {
        this.report = d;
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
