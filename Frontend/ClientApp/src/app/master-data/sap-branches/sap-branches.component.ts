import { Component, OnInit } from "@angular/core";
import { CommonSrvService } from "../../common-srv.service";
import { MatTableDataSource } from "@angular/material/table";

@Component({
  selector: "app-sap-branches",
  templateUrl: "./sap-branches.component.html",
  styleUrls: ["./sap-branches.component.scss"],
})
export class SapBranchesComponent implements OnInit {
  displayedColumns = ["BranchId", "BranchName"];
  sapBranches: any[];
  error: string;
  dtProcess: MatTableDataSource<any> = new MatTableDataSource([]);
  constructor(private ComSrv: CommonSrvService) {}

  ngOnInit(): void {
    this.GetData();
  }
  GetData() {
    this.ComSrv.getJSON("api/SAP/GetSAPBraches").subscribe(
      (d: any) => {
        this.dtProcess = new MatTableDataSource(d);
      },
      (error) => (this.error = error) // error path
    );
  }
  syncBranches() {
    this.ComSrv.postJSON("api/SAP/SynceBranches", null).subscribe(
      (d: any) => {
        alert("Synced Successfully");
      },
      (error) => (this.error = error)
    );
  }
}
