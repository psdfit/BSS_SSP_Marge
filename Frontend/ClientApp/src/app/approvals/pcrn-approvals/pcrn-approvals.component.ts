import { Component, OnInit, ViewChild } from "@angular/core";
import { MatPaginator } from "@angular/material/paginator";
import { MatSort } from "@angular/material/sort";
import { CommonSrvService } from "../../common-srv.service";
import { MatDialog } from "@angular/material/dialog";
import { Overlay } from "@angular/cdk/overlay";
import { DialogueService } from "../../shared/dialogue.service";
import {
  EnumApprovalProcess,
  EnumExcelReportType,
} from "../../shared/Enumerations";
import {
  DateAdapter,
  MAT_DATE_FORMATS,
  MAT_DATE_LOCALE,
} from "@angular/material/core";
import {
  MomentDateAdapter,
  MAT_MOMENT_DATE_ADAPTER_OPTIONS,
} from "@angular/material-moment-adapter";
import { Moment } from "moment";
import * as _moment from "moment";
import { MatDatepicker } from "@angular/material/datepicker";
import { FormControl } from "@angular/forms";

import { Observable } from "rxjs";
import { ExportExcel } from "../../shared/Interfaces";
import { GroupByPipe } from "angular-pipes";
import { environment } from "../../../environments/environment";
import { DatePipe } from "@angular/common";
import { PcrnApprovalsDialogueComponent } from "../pcrn-approvals-dialogue/pcrn-approvals-dialogue.component";

const moment = _moment;
// See the Moment.js docs for the meaning of these formats:
// https://momentjs.com/docs/#/displaying/format/
export const MY_FORMATS = {
  parse: {
    dateInput: "MM/YYYY",
  },
  display: {
    dateInput: "MM/YYYY",
    monthYearLabel: "MMM YYYY",
    dateA11yLabel: "LL",
    monthYearA11yLabel: "MMMM YYYY",
  },
};

@Component({
  selector: "app-pcrn-approvals",
  templateUrl: "./pcrn-approvals.component.html",
  styleUrls: ["./pcrn-approvals.component.scss"],

  providers: [
    // `MomentDateAdapter` can be automatically provided by importing `MomentDateModule` in your
    // application's root module. We provide it at the component level here, due to limitations of
    // our example generation script.
    {
      provide: DateAdapter,
      useClass: MomentDateAdapter,
      deps: [MAT_DATE_LOCALE, MAT_MOMENT_DATE_ADAPTER_OPTIONS],
    },

    { provide: MAT_DATE_FORMATS, useValue: MY_FORMATS },
    GroupByPipe,
    DatePipe,
  ],
})
export class PcrnApprovalsComponent implements OnInit {
  environment = environment;

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  //resultsLength: any;
  //dtPCRNDataDisplayedColumns = ['TSPName', 'TradeName', 'ClassCode', 'TraineeCode', 'TraineeName', 'FatherName', 'TraineeCNIC', 'DistrictName', 'ContactNumber1', 'TrainingAddressLocation', 'Comments', 'Batch', 'TokenNumber', 'TransactionNumber', 'Status', 'Month', 'NumberOfMonths', 'Action'];
  //dtPCRNData: MatTableDataSource<any>;
  PCRN: any[] = [];
  PCRNApproved: any[] = [];
  PCRNPending: any[] = [];
  PCRNGrouped: any[] = [];
  PCRNGroupedByBatch: any[] = [];
  varProcessKey: string;
  //PCRNGroupList: any[] = [];
  // PCRNDetails: any[];
  errorHTTP: any;
  month = new FormControl(moment());
  currentUser: any;
  kamusers: [];
  schemes: [];
  tsps: [];
  tspMasters: [];
  PCRNDetailsArray: any[];
  PCRNDetailsBulkArray: any[];

  PCRNMasterArray: any[];
  PCRNMasterIDs: string;

  SearchSch = new FormControl("");
  SearchKAM = new FormControl("");
  SearchTSP = new FormControl("");

  //filters: IPCRNApprovalFilter = { SchemeID: 0, TSPID: 0, KAMID: 0 };
  filters: IPCRNApprovalFilter = { SchemeID: 0, TSPMasterID: 0, KAMID: 0 };

  constructor(
    private datePipe: DatePipe,
    private http: CommonSrvService,
    public dialog: MatDialog,
    private overlay: Overlay,
    private dialogue: DialogueService,
    private groupByPipe: GroupByPipe
  ) {}

  ngOnInit(): void {
    this.http.setTitle("PCRN");
    this.currentUser = this.http.getUserDetails();
    this.PCRNDetailsArray = [];
    this.http.OID.subscribe((OID) => {
      this.GetPCRN();
      this.GetFiltersData();
    });
  }
  EmptyCtrl() {
    this.SearchKAM.setValue("");
    this.SearchTSP.setValue("");
    this.SearchSch.setValue("");
  }

  GetFiltersData() {
    this.http.getJSON(`api/PRNMaster/GetFiltersData`).subscribe(
      (response: any) => {
        this.kamusers = response[0];
        this.schemes = response[1];
        //this.tsps = response[2];
        this.tspMasters = response[2];
        //r.PRN = data;
        //r.HasPRN = true;
      },
      (error) => {
        console.error(JSON.stringify(error));
      }
    );
  }

  GetPCRN() {
    //let month = new Date('2020-03-01');
    //this.http.postJSON(`api/PCRN/GetPCRN`, { Month: this.month.value, KAMID: this.filters.KAMID, SchemeID: this.filters.SchemeID, TSPID: this.filters.TSPID }).subscribe(
    this.http
      .postJSON(`api/PCRN/GetPCRN`, {
        Month: this.month.value,
        KAMID: this.filters.KAMID,
        SchemeID: this.filters.SchemeID,
        TSPMasterID: this.filters.TSPMasterID,
      })
      .subscribe(
        (data: any) => {
          console.log(data);
          this.PCRNMasterArray = data.map((o) => o.PCRNID);
          this.PCRNMasterIDs = this.PCRNMasterArray.join(",");
          this.PCRN = [];
          this.PCRNGroupedByBatch = this.groupByPipe.transform(
            data,
            "ApprovalBatchNo"
          );
          let indexPCRN = 0;
          this.PCRNGroupedByBatch.forEach((key) => {
            var number = indexPCRN; //Number(key.key) - 1;
            //this.PCRNGrouped = this.groupByPipe.transform(this.PCRNGroupedByBatch[key], "IsApproved")

            this.PCRNGrouped = this.groupByPipe
              .transform(this.PCRNGroupedByBatch[number].value, "SchemeName")
              .map((x) => {
                return {
                  ...x,
                  toggleValue: true,
                  IsApproved: x.value[0]["IsApproved"],
                };
              });

            this.PCRN.push(this.PCRNGrouped);
            indexPCRN++;
          });
          this.PCRN = this.PCRN.reduce(
            (accumulator, value) => accumulator.concat(value),
            []
          );
          //this.PCRNGrouped = this.groupByPipe.transform(data, "IsApproved")
          //this.PCRNMasterArray = data.map(o => o.PCRNID);
          //this.PCRNMasterIDs = this.PCRNMasterArray.join(',');
          //this.PCRNPending = this.groupByPipe.transform(this.PCRNGrouped[1].value, "SchemeName")
          //  .map(x => {
          //    return { ...x, toggleValue: true, IsApproved: this.PCRNGrouped[1].key }
          //  });
          //this.PCRN.push(this.PCRNPending);
          //this.PCRNApproved = this.groupByPipe.transform(this.PCRNGrouped[0].value, "SchemeName")
          //  .map(x => {
          //    return { ...x, toggleValue: true, IsApproved: this.PCRNGrouped[0].key }
          //  });
          //this.PCRN.push(this.PCRNApproved);

          //console.log(this.PCRN);
          //this.PCRN = this.PCRN.reduce((accumulator, value) => accumulator.concat(value), []);

          //this.PCRNGrouped = this.groupByPipe.transform(data, "IsApproved")
          //this.PCRNMasterArray = data.map(o => o.PCRNID);
          //this.PCRNMasterIDs = this.PCRNMasterArray.join(',');
          //this.PCRNPending = this.groupByPipe.transform(this.PCRNGrouped[1].value, "SchemeName")
          //  .map(x => {
          //    return { ...x, toggleValue: true, IsApproved: this.PCRNGrouped[1].key }
          //  });
          //this.PCRN.push(this.PCRNPending);
          //this.PCRNApproved = this.groupByPipe.transform(this.PCRNGrouped[0].value, "SchemeName")
          //  .map(x => {
          //    return { ...x, toggleValue: true, IsApproved: this.PCRNGrouped[0].key }
          //  });
          //this.PCRN.push(this.PCRNApproved);

          //console.log(this.PCRN);
          //this.PCRN = this.PCRN.reduce((accumulator, value) => accumulator.concat(value), []);
        },

        (error) => {
          this.http.ShowError(error.error + "\n" + error.message);
        }
      );
  }
  GetPCRNDetails(r: any) {
    if (r.PCRNDetails) {
      r.PCRNDetails = null;
      this.PCRNDetailsArray = this.PCRNDetailsArray.filter(
        (s) => s.PCRNID != r.PCRNID
      );

      return;
    }
    this.http.getJSON("api/PCRN/GetPCRNDetails/" + r.PCRNID).subscribe(
      (data: any) => {
        r.PCRNDetails = data[0];
        this.PCRNDetailsArray.push(data[0]);
        this.PCRNDetailsArray = this.PCRNDetailsArray.reduce(
          (accumulator, value) => accumulator.concat(value),
          []
        );
      },
      (error) => {
        this.http.ShowError(error.error + "\n" + error.message);
      }
    );
  }
  ///---Invoke Dialog---S--////
  //openApprovalDialogue(row: any): void {
  //  //{ ProcessKey: 'AP', FormID:  row.PCRNId }
  //  //let datas: IApprovalHistory = { ProcessKey: 'AP', FormID: 292 };
  //  this.dialogue.openApprovalDialogue(EnumApprovalProcess.PCRN, row.PCRNID).subscribe(result => { console.log(result); });
  //}

  public openPCRNApprovalDialogue(PCRNGroup: any[]): void {
    console.log(PCRNGroup);
    debugger;
    //let datas: IApprovalHistory = { ProcessKey: processKey, FormID: formID };
    var processk = PCRNGroup.map((x) => x.ProcessKey);

    const dialogRef = this.dialog.open(PcrnApprovalsDialogueComponent, {
      width: "60%",
      data: {
        ProcessKey: EnumApprovalProcess.PCRN,
        FormIDs: PCRNGroup.map((x) => x.PCRNID),
      },
    });
  }
  ///---Invoke  Dialog---E--////
  chosenYearHandler(normalizedYear: Moment) {
    this.month = new FormControl(moment());
    const ctrlValue = this.month.value;
    ctrlValue.year(normalizedYear.year());
    this.month.setValue(ctrlValue);
  }

  chosenMonthHandler(
    normalizedMonth: Moment,
    datepicker: MatDatepicker<Moment>
  ) {
    const ctrlValue = this.month.value;
    ctrlValue.month(normalizedMonth.month());
    this.month.setValue(ctrlValue);
    this.GetPCRN();
    datepicker.close();
  }

  clearMonth() {
    this.month = new FormControl(moment(null));
    //  this.month.setValue(null);
    this.GetPCRN();
  }

  GetByPCRNMasterIDs() {
    this.http
      .postJSON("api/PCRN/GetPCRNExcelExportByIDs", this.PCRNMasterIDs)
      .subscribe((d: any) => {
        this.PCRNDetailsBulkArray = d;
        this.ExportToExcelBulkPCRN();
      });
  }

  ExportToExcel(PCRNID: number) {
    let filteredData = this.PCRNDetailsBulkArray;

    this.http
      .postJSON("api/PCRN/GetPCRNExcelExport/", {
        PCRNID: PCRNID,
        Month: this.month.value,
      })
      .subscribe((d: any) => {
        filteredData = d;

        let exportExcel: ExportExcel = {
          Title: "PCRN_Excel_Export",
          Author: this.currentUser.FullName,
          Type: EnumExcelReportType.PCRN,
          //Data: data,
          List1: this.populateData(filteredData),
        };
        this.dialogue.openExportConfirmDialogue(exportExcel).subscribe();
      });
  }
  ExportToExcelBulkPCRN() {
    //if (this.PCRNDetailsArray.length == 0) {
    //    this.http.ShowError("Please check PCRN Details to export data")
    //    return;
    //}
    let fileName = "PCRN";
    let filteredData = [...this.PCRNDetailsBulkArray];

    const result = filteredData.reduce(
      (accumulator, value) => accumulator.concat(value),
      []
    );
    //console.log(result);

    let exportExcel: ExportExcel = {
      Title: "PCRN Approval Report",
      Author: this.currentUser.FullName,
      Type: EnumExcelReportType.PCRN,
      Month: this.month.value,
      Data: {},
      //List1: data
      List1: this.populateData(result),
    };
    this.dialogue.openExportConfirmDialogue(exportExcel).subscribe();
  }

  populateData(data: any) {
    return data.map((item, index) => {
      return {
        "SR #": index + 1,
        Project: item.FundingCategory,
        Scheme: item.SchemeName,
        TSP: item.TSPName,
        "Class Start Date": this.datePipe.transform(
          item.ClassStartDate,
          "dd/MM/yyyy"
        ),
        "Class End Date": this.datePipe.transform(
          item.ClassEndDate,
          "dd/MM/yyyy"
        ),
        "Trainee Code": item.TraineeCode,
        "Trainee Name": item.TraineeName,
        "Father Name": item.FatherName,
        "Trainee CNIC": item.TraineeCNIC,
        "Contact Number": item.ContactNumber1,
        "Token Number": "",
        "Transaction Number": "",
        Amount: item.Amount,
        Comments: item.Comments,
      };
    });
  }
  getTraineesAmountWise(r: any) {
    if (
      r.traineesAmountWise &&
      r.traineesAmountWise[0].value[0].PCRNID == r.PCRNID
    ) {
      //r.traineesAmountWise = null;
      return;
    }
    this.http.getJSON("api/PCRN/GetPCRNDetails/" + r.PCRNID).subscribe(
      (data: any) => {
        r.traineesAmountWise = this.groupByPipe.transform(data[0], "Amount");
      },
      (error) => {
        this.http.ShowError(error.error + "\n" + error.message);
      }
    );
    console.log(r.traineesAmountWise);
  }
  openTraineeJourneyDialogue(data: any): void {
    debugger;
    this.dialogue.openTraineeJourneyDialogue(data);
  }

  openClassJourneyDialogue(data: any): void {
    debugger;
    this.dialogue.openClassJourneyDialogue(data);
  }
}

export interface IPCRNApprovalFilter {
  SchemeID: number;
  //TSPID: number;
  TSPMasterID: number;
  KAMID: number;
}
