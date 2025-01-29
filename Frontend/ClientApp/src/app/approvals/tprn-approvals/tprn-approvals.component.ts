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
import { TprnApprovalsDialogueComponent } from "../tprn-approvals-dialogue/tprn-approvals-dialogue.component";

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
  selector: "app-tprn-approvals",
  templateUrl: "./tprn-approvals.component.html",
  styleUrls: ["./tprn-approvals.component.scss"],

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
export class TprnApprovalsComponent implements OnInit {
  environment = environment;

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  //resultsLength: any;
  //dtTPRNDataDisplayedColumns = ['TSPName', 'TradeName', 'ClassCode', 'TraineeCode', 'TraineeName', 'FatherName', 'TraineeCNIC', 'DistrictName', 'ContactNumber1', 'TrainingAddressLocation', 'Comments', 'Batch', 'TokenNumber', 'TransactionNumber', 'Status', 'Month', 'NumberOfMonths', 'Action'];
  //dtTPRNData: MatTableDataSource<any>;
  TPRN: any[] = [];
  TPRNApproved: any[] = [];
  TPRNPending: any[] = [];
  TPRNGrouped: any[] = [];
  TPRNGroupedByBatch: any[] = [];
  varProcessKey: string;
  //TPRNGroupList: any[] = [];
  // TPRNDetails: any[];
  errorHTTP: any;
  month = new FormControl(moment());
  currentUser: any;
  kamusers: [];
  schemes: [];
  tsps: [];
  tspMasters: [];
  TPRNDetailsArray: any[];
  TPRNDetailsBulkArray: any[];

  TPRNMasterArray: any[];
  TPRNMasterIDs: string;

  SearchSch = new FormControl("");
  SearchKAM = new FormControl("");
  SearchTSP = new FormControl("");

  //filters: ITPRNApprovalFilter = { SchemeID: 0, TSPID: 0, KAMID: 0 };
  filters: ITPRNApprovalFilter = { SchemeID: 0, TSPMasterID: 0, KAMID: 0 };

  constructor(
    private datePipe: DatePipe,
    private http: CommonSrvService,
    public dialog: MatDialog,
    private overlay: Overlay,
    private dialogue: DialogueService,
    private groupByPipe: GroupByPipe
  ) {}

  ngOnInit(): void {
    this.http.setTitle("TPRN");
    this.currentUser = this.http.getUserDetails();
    this.TPRNDetailsArray = [];
    this.http.OID.subscribe((OID) => {
      this.GetTPRN();
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

  GetTPRN() {
    //let month = new Date('2020-03-01');
    //this.http.postJSON(`api/TPRN/GetTPRN`, { Month: this.month.value, KAMID: this.filters.KAMID, SchemeID: this.filters.SchemeID, TSPID: this.filters.TSPID }).subscribe(
    this.http
      .postJSON(`api/TPRN/GetTPRN`, {
        Month: this.month.value,
        KAMID: this.filters.KAMID,
        SchemeID: this.filters.SchemeID,
        TSPMasterID: this.filters.TSPMasterID,
      })
      .subscribe(
        (data: any) => {
          console.log(data);
          this.TPRNMasterArray = data.map((o) => o.TPRNID);
          this.TPRNMasterIDs = this.TPRNMasterArray.join(",");
          this.TPRN = [];
          this.TPRNGroupedByBatch = this.groupByPipe.transform(
            data,
            "ApprovalBatchNo"
          );
          let indexTPRN = 0;
          this.TPRNGroupedByBatch.forEach((key) => {
            var number = indexTPRN; //Number(key.key) - 1;
            //this.TPRNGrouped = this.groupByPipe.transform(this.TPRNGroupedByBatch[key], "IsApproved")

            this.TPRNGrouped = this.groupByPipe
              .transform(this.TPRNGroupedByBatch[number].value, "SchemeName")
              .map((x) => {
                return {
                  ...x,
                  toggleValue: true,
                  IsApproved: x.value[0]["IsApproved"],
                };
              });

            this.TPRN.push(this.TPRNGrouped);
            indexTPRN++;
          });
          this.TPRN = this.TPRN.reduce(
            (accumulator, value) => accumulator.concat(value),
            []
          );
          //this.TPRNGrouped = this.groupByPipe.transform(data, "IsApproved")
          //this.TPRNMasterArray = data.map(o => o.TPRNID);
          //this.TPRNMasterIDs = this.TPRNMasterArray.join(',');
          //this.TPRNPending = this.groupByPipe.transform(this.TPRNGrouped[1].value, "SchemeName")
          //  .map(x => {
          //    return { ...x, toggleValue: true, IsApproved: this.TPRNGrouped[1].key }
          //  });
          //this.TPRN.push(this.TPRNPending);
          //this.TPRNApproved = this.groupByPipe.transform(this.TPRNGrouped[0].value, "SchemeName")
          //  .map(x => {
          //    return { ...x, toggleValue: true, IsApproved: this.TPRNGrouped[0].key }
          //  });
          //this.TPRN.push(this.TPRNApproved);

          //console.log(this.TPRN);
          //this.TPRN = this.TPRN.reduce((accumulator, value) => accumulator.concat(value), []);

          //this.TPRNGrouped = this.groupByPipe.transform(data, "IsApproved")
          //this.TPRNMasterArray = data.map(o => o.TPRNID);
          //this.TPRNMasterIDs = this.TPRNMasterArray.join(',');
          //this.TPRNPending = this.groupByPipe.transform(this.TPRNGrouped[1].value, "SchemeName")
          //  .map(x => {
          //    return { ...x, toggleValue: true, IsApproved: this.TPRNGrouped[1].key }
          //  });
          //this.TPRN.push(this.TPRNPending);
          //this.TPRNApproved = this.groupByPipe.transform(this.TPRNGrouped[0].value, "SchemeName")
          //  .map(x => {
          //    return { ...x, toggleValue: true, IsApproved: this.TPRNGrouped[0].key }
          //  });
          //this.TPRN.push(this.TPRNApproved);

          //console.log(this.TPRN);
          //this.TPRN = this.TPRN.reduce((accumulator, value) => accumulator.concat(value), []);
        },

        (error) => {
          this.http.ShowError(error.error + "\n" + error.message);
        }
      );
  }
  GetTPRNDetails(r: any) {
    if (r.TPRNDetails) {
      r.TPRNDetails = null;
      this.TPRNDetailsArray = this.TPRNDetailsArray.filter(
        (s) => s.TPRNID != r.TPRNID
      );

      return;
    }
    this.http.getJSON("api/TPRN/GetTPRNDetails/" + r.TPRNID).subscribe(
      (data: any) => {
        r.TPRNDetails = data[0];
        this.TPRNDetailsArray.push(data[0]);
        this.TPRNDetailsArray = this.TPRNDetailsArray.reduce(
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
  //  //{ ProcessKey: 'AP', FormID:  row.TPRNId }
  //  //let datas: IApprovalHistory = { ProcessKey: 'AP', FormID: 292 };
  //  this.dialogue.openApprovalDialogue(EnumApprovalProcess.TPRN, row.TPRNID).subscribe(result => { console.log(result); });
  //}

  public openTPRNApprovalDialogue(TPRNGroup: any[]): void {
    console.log(TPRNGroup);
    debugger;
    //let datas: IApprovalHistory = { ProcessKey: processKey, FormID: formID };
    var processk = TPRNGroup.map((x) => x.ProcessKey);

    const dialogRef = this.dialog.open(TprnApprovalsDialogueComponent, {
      width: "60%",
      data: {
        ProcessKey: EnumApprovalProcess.TPRN,
        FormIDs: TPRNGroup.map((x) => x.TPRNID),
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
    this.GetTPRN();
    datepicker.close();
  }

  clearMonth() {
    this.month = new FormControl(moment(null));
    //  this.month.setValue(null);
    this.GetTPRN();
  }

  GetByTPRNMasterIDs() {
    this.http
      .postJSON("api/TPRN/GetTPRNExcelExportByIDs", this.TPRNMasterIDs)
      .subscribe((d: any) => {
        this.TPRNDetailsBulkArray = d;
        this.ExportToExcelBulkTPRN();
      });
  }

  ExportToExcel(TPRNID: number) {
    let filteredData = this.TPRNDetailsBulkArray;

    this.http
      .postJSON("api/TPRN/GetTPRNExcelExport/", {
        TPRNID: TPRNID,
        Month: this.month.value,
      })
      .subscribe((d: any) => {
        filteredData = d;

        let exportExcel: ExportExcel = {
          Title: "TPRN_Excel_Export",
          Author: this.currentUser.FullName,
          Type: EnumExcelReportType.TPRN,
          //Data: data,
          List1: this.populateData(filteredData),
        };
        this.dialogue.openExportConfirmDialogue(exportExcel).subscribe();
      });
  }
  ExportToExcelBulkTPRN() {
    //if (this.TPRNDetailsArray.length == 0) {
    //    this.http.ShowError("Please check TPRN Details to export data")
    //    return;
    //}
    let fileName = "TPRN";
    let filteredData = [...this.TPRNDetailsBulkArray];

    const result = filteredData.reduce(
      (accumulator, value) => accumulator.concat(value),
      []
    );
    //console.log(result);

    let exportExcel: ExportExcel = {
      Title: "TPRN Approval Report",
      Author: this.currentUser.FullName,
      Type: EnumExcelReportType.TPRN,
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
      r.traineesAmountWise[0].value[0].TPRNID == r.TPRNID
    ) {
      //r.traineesAmountWise = null;
      return;
    }
    this.http.getJSON("api/TPRN/GetTPRNDetails/" + r.TPRNID).subscribe(
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

export interface ITPRNApprovalFilter {
  SchemeID: number;
  //TSPID: number;
  TSPMasterID: number;
  KAMID: number;
}
