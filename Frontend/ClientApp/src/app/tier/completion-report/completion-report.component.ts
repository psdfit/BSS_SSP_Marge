import { Component, OnInit } from "@angular/core";
import { FormBuilder } from "@angular/forms";
import { CommonSrvService } from "../../common-srv.service";

@Component({
  selector: "app-completion-report",
  templateUrl: "./completion-report.component.html",
  styleUrls: ["./completion-report.component.scss"],
})
export class CompletionReportComponent implements OnInit {
  error: String;
  report: [];
  working: boolean;
  constructor(
    private http: CommonSrvService,
    private _formBuilder: FormBuilder
  ) {}
  ngOnInit(): void {
    this.GetCompletionReport();
  }
  GetCompletionReport() {
    this.http.getJSON("api/Tier/GetCompletionReport").subscribe(
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
