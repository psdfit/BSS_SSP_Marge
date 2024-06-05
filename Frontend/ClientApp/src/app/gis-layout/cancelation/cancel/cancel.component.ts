/* **** Aamer Rehman Malik *****/
import { Component, OnInit } from "@angular/core";
import { FormControl } from "@angular/forms";
import * as moment from 'moment';
import { CommonSrvService } from "src/app/common-srv.service";
import { IQueryFilters } from "src/app/home/home.component";
import { DialogueService } from 'src/app/shared/dialogue.service';

@Component({
  selector: "cancel",
  templateUrl: "./cancel.component.html",
  styleUrls: ["./cancel.component.scss"],
})
export class CancelComponent implements OnInit {
  Schemes: [];
  TSP: [];
  Classes: [];
  SearchSch = new FormControl("");
  SearchCls = new FormControl("");
  SearchTSP = new FormControl("");
  filters: IQueryFilters = {
    SchemeID: 0,
    TSPID: 0,
    ClassID: 0,
    TraineeID: 0,
    UserID: 0,
  };
  ClassData: ClassInvoiceMapExtModel[];
  CancelConfirmMsg: any;
  working: boolean;
  error: string;

  constructor(private ComSrv: CommonSrvService, private dailog: DialogueService) { }

  ngOnInit(): void {
    //this.Init();
    this.getDataByFilters();
    this.ComSrv.setTitle("Cancelation");
  }

  Init() {
    this.ComSrv.getJSON("api/Cancelation/GetData").subscribe((res) => {
      this.Schemes = res["Schemes"];
      this.TSP = res["TSP"];
      this.Classes = res["Classes"];
    });
  }
  getDataByFilters() {
    this.ComSrv.getJSON(
      "api/Cancelation/getCancelationData/",
      29540
    ).subscribe((res: any) => {
      this.ClassData = res;
      console.log(res);
    });

    //if (this.filters.ClassID > 0) {
    //  this.ComSrv.getJSON(
    //    "api/Cancelation/getCancelationData/",
    //    this.filters.ClassID
    //  ).subscribe((res: any) => {
    //    this.ClassData = res;
    //  });
    //}
  }
  Cancel(row: any, type: any, classid) {
    if (type == 'Inv') { this.CancelConfirmMsg = "Are you sure you want to cancel this invoice header? \nNote: All invoices against this header will be cancelled. Please view details for more information."; }
    this.ComSrv.confirm("Cancelation", this.CancelConfirmMsg).subscribe((result) => {
      if (result) {
        //console.log(row, type, classid);
        this.ComSrv.getJSON("api/Cancelation/Cancelation?FormID=" + row + "&Type=" + type + "&ClassID=" + classid).subscribe(
          (res: any) => {
            this.ComSrv.openSnackBar("Data has been canceled from the system.");
            this.ClassData = res;
          },
          (error) => {
            this.error = error.error;
            this.working = false;
            this.ComSrv.ShowError(this.error);
          });
      }
    });
  }
  Regenrate(row: any, index: number) {
    if (index > 0) {
      let b = false;
      for (var i = 0; i < index; i++) {
        if (this.ClassData[i].RegenratePRN || this.ClassData[i].RegenrateMPR)
          b = true;
      }
      if (b) {
        this.ComSrv.ShowWarning("Please Genrate previous Invoice(s)");
        return;
      }
    }
    this.ComSrv.confirm("Regenrate", "Are you sure?").subscribe((result) => {
      if (result) {
        row.Month = moment.utc(row.Month);
        this.ComSrv.postJSON("api/Cancelation/Regenrate", row).subscribe(
          (res: any) => {
            this.ClassData = res;
          }
        );
      }
    });
  };
  GetMPR(ID: number) {
    this.dailog.openDocumentDialogue(ID, 'MPR');
  }
  GetSRN(ID: number) {
    this.dailog.openDocumentDialogue(ID, 'SRN');
  }
  GetPRN(ID: number) {
    this.dailog.openDocumentDialogue(ID, 'PRN');
  }
  GetPO(ID: number) {
    this.dailog.openDocumentDialogue(ID, 'PO');
  }
  GetInv(ID: number) {
    this.dailog.openDocumentDialogue(ID, 'Inv');
  }
  EmptyCtrl(ev: any) {
    this.SearchCls.setValue("");
    this.SearchTSP.setValue("");
    this.SearchSch.setValue("");
  }
}
export class ClassInvoiceMapExtModel {
  MPRID: number;
  PRNID: number;
  Invoices: number;
  InvoicesHeader: number;
  MPRGenrated: Boolean;
  RegenrateMPR: Boolean;
  RegenratePRN: Boolean;

  IsGenerated: Boolean;
  InCancelation: Boolean;
  SRNID: number;
  POLineID: number;
  InvSapID: string;
  POSapID: string;
  SRNInvSapID: string;
  SRNInvoice: number;
  SRNInvHeader: number;
  InvIsCanceled: Boolean;
  InvSRNIsCanceled: Boolean;
}
