/* **** Aamer Rehman Malik *****/
import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import * as moment from 'moment';
import { CommonSrvService } from 'src/app/common-srv.service';
import { IQueryFilters } from 'src/app/home/home.component';
import { DialogueService } from 'src/app/shared/dialogue.service';

@Component({
  selector: 'app-cancel',
  templateUrl: './cancel.component.html',
  styleUrls: ['./cancel.component.scss'],
})
export class CancelComponent implements OnInit {
  Schemes: [];
  TSP: [];
  Classes: [];
  SearchSch = new FormControl('');
  SearchCls = new FormControl('');
  SearchTSP = new FormControl('');
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
    this.Init();
    // this.getDataByFilters();
    this.ComSrv.setTitle('Cancelation');
  }

  Init() {
    this.ComSrv.getJSON('api/Cancelation/GetData').subscribe((res) => {
      this.Schemes = res['Schemes'];
      this.TSP = res['TSP'];
      this.Classes = res['Classes'];
    });
  }
  getDataByFilters() {
    this.ComSrv.getJSON(
      'api/Cancelation/getCancelationData/',
      this.filters.ClassID
      // 29540
      // 29216
    ).subscribe((res: any) => {
      this.ClassData = res;
      // console.log(res);
    });
  }
  Cancel(row: any, type: any, classid, index) {
    let next : any;
    if (index < this.ClassData.length - 1) {
      console.log(this.ClassData[index + 1].Month, this.ClassData.length, index);
      next = this.ClassData[index + 1];
    }

    if (type === 'Inv_R') {
      if (next != undefined) {
        if (next.InvHeader > 0 && next.InvIsCanceled <= 0) { this.ComSrv.ShowError('Please cancel next month\'s invoice first to cancel this one.'); return; }
      }
      this.CancelConfirmMsg = 'Are you sure you want to cancel this invoice header? \nNote: All invoices against this header will be cancelled. Please view details for more information.';
    }
    else if (type === 'Inv_S') {
      if (next != undefined) {
        if (next.InvHeaderSRN > 0 && next.InvSRNIsCanceled <= 0) { this.ComSrv.ShowError('Please cancel next month\'s invoice first to cancel this one.'); return; }
      }
      this.CancelConfirmMsg = 'Are you sure you want to cancel this invoice header? \nNote: All invoices against this header will be cancelled. Please view details for more information.';
    }
    else if (type == 'PO_SRN') {
      if (next != undefined) {
        if (next.POHeaderIDSRN > 0 && next.POHeaderSRNIsCanceled <= 0) { this.ComSrv.ShowError("Please cancel next month's PO first to cancel this one."); return; }
      }
      this.CancelConfirmMsg = 'Are you sure you want to cancel this PO header? \nNote: All PO Lines against this header will be cancelled. Please view details for more information.';
    }
    else if (type === 'SRN') {
      if (next != undefined) {
        if (next.SRNID > 0 && next.SRNIsCanceled <= 0) { this.ComSrv.ShowError('Please cancel next month\'s SRN first to cancel this one.'); return; }
      }
      this.CancelConfirmMsg = 'Are you sure you want to cancel this SRN?';
    }
    else if (type === 'PRN') {
      if (next != undefined) {
        if (next.PRNID > 0 && next.PRNIsCanceled <= 0) { this.ComSrv.ShowError('Please cancel next month\'s PRN first to cancel this one.'); return; }
      }
      this.CancelConfirmMsg = 'Are you sure you want to cancel this PRN? \nNote: PRN is TSP wise, so all prn details will be cancelled for this month. Please view details for more information.';
    }
    else if (type === 'MPR') {
      if (next != undefined) {
        if (next.MPRID > 0 && next.MPRIsCanceled <= 0) { this.ComSrv.ShowError('Please cancel next month\'s MPR first to cancel this one.'); return; }
      }
      this.CancelConfirmMsg = 'Are you sure you want to cancel this MPR?';
    }
    else { this.ComSrv.ShowError('Cancelation not available for this document.'); return; }

    this.ComSrv.confirm('Cancelation', this.CancelConfirmMsg).subscribe((result) => {
      if (result) {
        // console.log(row, type, classid);
        this.ComSrv.getJSON('api/Cancelation/Cancelation?FormID=' + row + '&Type=' + type + '&ClassID=' + classid).subscribe(
          (res: any) => {
            this.ComSrv.openSnackBar('Data has been canceled from the system.');
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
  Generate(row: any, type: string, index: number) {
    // debugger;
    row.Type = type;
    row.Index = index;
    this.ComSrv.confirm('Regenerate', 'Are you sure you want to generate this ' + type + '?').subscribe((result) => {
      if (result) {
        row.Month = moment.utc(row.Month);

        this.ComSrv.postJSON('api/Cancelation/Regenerate', row).subscribe(
          (res: any) => {
            this.ComSrv.openSnackBar(type + ' has been generated.');
            this.ClassData = res;
          },
          (error) => {
            this.error = error.error;
            this.working = false;
            this.ComSrv.ShowError(this.error);
          }
        );
      }
    });
  }
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
    this.SearchCls.setValue('');
    this.SearchTSP.setValue('');
    this.SearchSch.setValue('');
  }
}
export class ClassInvoiceMapExtModel {
  MPRID: number;
  PRNID: number;
  Invoices: number;
  InvoicesHeader: number;
  MPRGenrated: boolean;
  RegenrateMPR: boolean;
  RegenratePRN: boolean;

  IsGenerated: boolean;
  InCancelation: boolean;
  SRNID: number;
  POLineID: number;
  InvSapID: string;
  POSapID: string;
  SRNInvSapID: string;
  SRNInvoice: number;
  SRNInvHeader: number;
  InvIsCanceled: boolean;
  InvSRNIsCanceled: boolean;
  Month: any;
  MPRIsCanceled: boolean;
}
