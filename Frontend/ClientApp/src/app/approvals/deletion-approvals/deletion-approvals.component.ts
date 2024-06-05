/* **** Aamer Rehman Malik *****/
import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { CommonSrvService } from '../../common-srv.service';
import { MatDialog } from '@angular/material/dialog';
import { Overlay } from '@angular/cdk/overlay';
import { DialogueService } from '../../shared/dialogue.service';
import { EnumApprovalProcess } from '../../shared/Enumerations';
import { DateAdapter, MAT_DATE_FORMATS, MAT_DATE_LOCALE } from '@angular/material/core';
import { MomentDateAdapter, MAT_MOMENT_DATE_ADAPTER_OPTIONS } from '@angular/material-moment-adapter';
import { Moment } from 'moment';
import * as _moment from 'moment';
import { MatDatepicker } from '@angular/material/datepicker';
import { FormControl } from '@angular/forms';
import { environment } from '../../../environments/environment';

const moment = _moment;

export const MY_FORMATS = {
  parse: {
    dateInput: 'MM/YYYY',
  },
  display: {
    dateInput: 'MM/YYYY',
    monthYearLabel: 'MMM YYYY',
    dateA11yLabel: 'LL',
    monthYearA11yLabel: 'MMMM YYYY',
  },
};

@Component({
  selector: 'app-deletion-approvals',
  templateUrl: './deletion-approvals.component.html',
  styleUrls: ['./deletion-approvals.component.scss'],
  providers: [
    // `MomentDateAdapter` can be automatically provided by importing `MomentDateModule` in your
    // application's root module. We provide it at the component level here, due to limitations of
    // our example generation script.
    {
      provide: DateAdapter,
      useClass: MomentDateAdapter,
      deps: [MAT_DATE_LOCALE, MAT_MOMENT_DATE_ADAPTER_OPTIONS]
    },

    { provide: MAT_DATE_FORMATS, useValue: MY_FORMATS },
  ],
})

export class DeletionApprovalsComponent implements OnInit {
  environment = environment;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  ClassData: [];

  errorHTTP: any;
  month = new FormControl(moment());

  constructor(private ComSrv: CommonSrvService, public dialog: MatDialog, private overlay: Overlay, private dialogue: DialogueService) { }

  ngOnInit(): void {
    this.ComSrv.setTitle("Cancelation");
    this.GetInvoice()
  }

  GetInvoice() {
    this.ComSrv.getJSON('api/Cancelation/getInvoice/').subscribe((res: any) => {
      this.ClassData = res;
    });
  }

  ///---Invoke Dialog---S--////
  openApprovalDialogue(row: any): void {
    //{ ProcessKey: 'AP', FormID:  row.SrnId }
    //let datas: IApprovalHistory = { ProcessKey: 'AP', FormID: 292 };
    this.dialogue.openApprovalDialogue(EnumApprovalProcess.CANCELATION, row.InvoiceID).subscribe(result => { console.log(result); });
  }
  ///---Invoke  Dialog---E--////
 


}
