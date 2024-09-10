import { Component, OnInit, ViewChild } from "@angular/core";
import { MatPaginator } from "@angular/material/paginator";
import { MatSort } from "@angular/material/sort";
import { MatTableDataSource } from "@angular/material/table";
import { environment } from "../../../environments/environment";
import { DatePipe } from "@angular/common";
import { DecimalPipe } from "@angular/common";

import { CommonSrvService } from "../../common-srv.service";
import { ActivatedRoute } from "@angular/router";

@Component({
  selector: "app-application-log",
  templateUrl: "./application-log.component.html",
  styleUrls: ["./application-log.component.scss"],
  providers: [DatePipe, DecimalPipe],
})
export class ApplicationLogComponent implements OnInit {
  dataObject: any;
  currentUser: any;
  appLogTableData: MatTableDataSource<[any]>;
  appLogTableColumns: string[] = [];
  SpacerTitle: any;

    
    constructor(private ComSrv: CommonSrvService, public DatePipe: DatePipe,private ActiveRoute: ActivatedRoute) {}

  ngOnInit(): void {
    this.ComSrv.setTitle("Application Log");
    this.appLogTableData = new MatTableDataSource([]);
    this.currentUser = this.ComSrv.getUserDetails();
    this.PageTitle();
    this.loadData();
  }

  PageTitle(): void {
    this.ComSrv.setTitle(this.ActiveRoute.snapshot.data.title);
    this.SpacerTitle = this.ActiveRoute.snapshot.data.title;
  }

  @ViewChild("appLogPaginator") appLogPaginator: MatPaginator;
  @ViewChild("appLogSort") appLogSort: MatSort;

  loadData() {
    this.ComSrv.postJSON("api/Users/LoadData", {
      UserID: this.currentUser.UserID,
    }).subscribe(
      (response) => {
        this.dataObject = response;

        if (this.dataObject.hasOwnProperty("applicationLog")) {
          this.LoadMatTable(this.dataObject.applicationLog, "ApplicationLog");
        }
      },
      (error) => {
        this.ComSrv.ShowError(`${error.message}`, "Close", 500000);
      }
    );
  }

  LoadMatTable(reportData: any[], reportTitle: string): void {
    if (reportData.length > 0) {
      const excludeColumnArray: string[] = [];

      if (reportTitle === "ApplicationLog") {
        this.appLogTableColumns = Object.keys(reportData[0]).filter(
          (key) => !key.includes("ID") && !excludeColumnArray.includes(key)
        );
        this.appLogTableColumns = ["SrNo", ...this.appLogTableColumns];
        this.appLogTableData = new MatTableDataSource(reportData);
        this.appLogTableData.paginator = this.appLogPaginator;
        this.appLogTableData.sort = this.appLogSort;
      }
    }
  }
  camelCaseToWords(input: string): string {
    return input.replace(/([a-z])([A-Z])/g, "$1 $2");
  }

  applyFilter(data: MatTableDataSource<any>, event: any) {
    data.filter = event.trim().toLowerCase();
    if (data.paginator) {
      data.paginator.firstPage();
    }
  }

  DataExcelExport(data: any[], title) {
    if (data.length > 0) {
      this.ComSrv.ExcelExporWithForm(data, title);
    } else {
      this.ComSrv.ShowError("No Record Found");
    }
  }
  
}
