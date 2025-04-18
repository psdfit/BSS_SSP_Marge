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
import { OtrnApprovalsDialogueComponent } from "../otrn-approvals-dialogue/otrn-approvals-dialogue.component";

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
  selector: "app-otrn-approvals",
  templateUrl: "./otrn-approvals.component.html",
  styleUrls: ["./otrn-approvals.component.scss"],

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
export class OtrnApprovalsComponent implements OnInit {
  environment = environment;

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  //resultsLength: any;
  //dtOTRNDataDisplayedColumns = ['TSPName', 'TradeName', 'ClassCode', 'TraineeCode', 'TraineeName', 'FatherName', 'TraineeCNIC', 'DistrictName', 'ContactNumber1', 'TrainingAddressLocation', 'Comments', 'Batch', 'TokenNumber', 'TransactionNumber', 'Status', 'Month', 'NumberOfMonths', 'Action'];
  //dtOTRNData: MatTableDataSource<any>;
  OTRN: any[] = [];
  OTRNApproved: any[] = [];
  OTRNPending: any[] = [];
  OTRNGrouped: any[] = [];
  OTRNGroupedByBatch: any[] = [];
  varProcessKey: string;
  //OTRNGroupList: any[] = [];
  // OTRNDetails: any[];
  errorHTTP: any;
  month = new FormControl(moment());
  currentUser: any;
  kamusers: [];
  schemes: [];
  tsps: [];
  tspMasters: [];
  OTRNDetailsArray: any[];
  OTRNDetailsBulkArray: any[];

  OTRNMasterArray: any[];
  OTRNMasterIDs: string;

  SearchSch = new FormControl("");
  SearchKAM = new FormControl("");
  SearchTSP = new FormControl("");

  //filters: IOTRNApprovalFilter = { SchemeID: 0, TSPID: 0, KAMID: 0 };
  filters: IOTRNApprovalFilter = { SchemeID: 0, TSPMasterID: 0, KAMID: 0 };

  constructor(
    private datePipe: DatePipe,
    private http: CommonSrvService,
    public dialog: MatDialog,
    private overlay: Overlay,
    private dialogue: DialogueService,
    private groupByPipe: GroupByPipe
  ) {}

  ngOnInit(): void {
    this.http.setTitle("OTRN");
    this.currentUser = this.http.getUserDetails();
    this.OTRNDetailsArray = [];
    this.http.OID.subscribe((OID) => {
      this.GetOTRN();
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

  GetOTRN() {
    //let month = new Date('2020-03-01');
    //this.http.postJSON(`api/OTRN/GetOTRN`, { Month: this.month.value, KAMID: this.filters.KAMID, SchemeID: this.filters.SchemeID, TSPID: this.filters.TSPID }).subscribe(
    this.http
      .postJSON(`api/OTRN/GetOTRN`, {
        Month: this.month.value,
        KAMID: this.filters.KAMID,
        SchemeID: this.filters.SchemeID,
        TSPMasterID: this.filters.TSPMasterID,
      })
      .subscribe(
        (data: any) => {
          console.log(data);
          this.OTRNMasterArray = data.map((o) => o.OTRNID);
          this.OTRNMasterIDs = this.OTRNMasterArray.join(",");
          this.OTRN = [];
          this.OTRNGroupedByBatch = this.groupByPipe.transform(
            data,
            "ApprovalBatchNo"
          );
          let indexOTRN = 0;
          this.OTRNGroupedByBatch.forEach((key) => {
            var number = indexOTRN; //Number(key.key) - 1;
            //this.OTRNGrouped = this.groupByPipe.transform(this.OTRNGroupedByBatch[key], "IsApproved")

            this.OTRNGrouped = this.groupByPipe
              .transform(this.OTRNGroupedByBatch[number].value, "SchemeName")
              .map((x) => {
                return {
                  ...x,
                  toggleValue: true,
                  IsApproved: x.value[0]["IsApproved"],
                };
              });

            this.OTRN.push(this.OTRNGrouped);
            indexOTRN++;
          });
          this.OTRN = this.OTRN.reduce(
            (accumulator, value) => accumulator.concat(value),
            []
          );
          //this.OTRNGrouped = this.groupByPipe.transform(data, "IsApproved")
          //this.OTRNMasterArray = data.map(o => o.OTRNID);
          //this.OTRNMasterIDs = this.OTRNMasterArray.join(',');
          //this.OTRNPending = this.groupByPipe.transform(this.OTRNGrouped[1].value, "SchemeName")
          //  .map(x => {
          //    return { ...x, toggleValue: true, IsApproved: this.OTRNGrouped[1].key }
          //  });
          //this.OTRN.push(this.OTRNPending);
          //this.OTRNApproved = this.groupByPipe.transform(this.OTRNGrouped[0].value, "SchemeName")
          //  .map(x => {
          //    return { ...x, toggleValue: true, IsApproved: this.OTRNGrouped[0].key }
          //  });
          //this.OTRN.push(this.OTRNApproved);

          //console.log(this.OTRN);
          //this.OTRN = this.OTRN.reduce((accumulator, value) => accumulator.concat(value), []);

          //this.OTRNGrouped = this.groupByPipe.transform(data, "IsApproved")
          //this.OTRNMasterArray = data.map(o => o.OTRNID);
          //this.OTRNMasterIDs = this.OTRNMasterArray.join(',');
          //this.OTRNPending = this.groupByPipe.transform(this.OTRNGrouped[1].value, "SchemeName")
          //  .map(x => {
          //    return { ...x, toggleValue: true, IsApproved: this.OTRNGrouped[1].key }
          //  });
          //this.OTRN.push(this.OTRNPending);
          //this.OTRNApproved = this.groupByPipe.transform(this.OTRNGrouped[0].value, "SchemeName")
          //  .map(x => {
          //    return { ...x, toggleValue: true, IsApproved: this.OTRNGrouped[0].key }
          //  });
          //this.OTRN.push(this.OTRNApproved);

          //console.log(this.OTRN);
          //this.OTRN = this.OTRN.reduce((accumulator, value) => accumulator.concat(value), []);
        },

        (error) => {
          this.http.ShowError(error.error + "\n" + error.message);
        }
      );
  }
  GetOTRNDetails(r: any) {
    if (r.OTRNDetails) {
      r.OTRNDetails = null;
      this.OTRNDetailsArray = this.OTRNDetailsArray.filter(
        (s) => s.OTRNID != r.OTRNID
      );

      return;
    }
    this.http.getJSON("api/OTRN/GetOTRNDetails/" + r.OTRNID).subscribe(
      (data: any) => {
        r.OTRNDetails = data[0];
        this.OTRNDetailsArray.push(data[0]);
        this.OTRNDetailsArray = this.OTRNDetailsArray.reduce(
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
  //  //{ ProcessKey: 'AP', FormID:  row.OTRNId }
  //  //let datas: IApprovalHistory = { ProcessKey: 'AP', FormID: 292 };
  //  this.dialogue.openApprovalDialogue(EnumApprovalProcess.OTRN, row.OTRNID).subscribe(result => { console.log(result); });
  //}

  public openOTRNApprovalDialogue(OTRNGroup: any[]): void {
    console.log(OTRNGroup);
    debugger;
    //let datas: IApprovalHistory = { ProcessKey: processKey, FormID: formID };
    var processk = OTRNGroup.map((x) => x.ProcessKey);

    const dialogRef = this.dialog.open(OtrnApprovalsDialogueComponent, {
      width: "60%",
      data: {
        ProcessKey: EnumApprovalProcess.OTRN,
        FormIDs: OTRNGroup.map((x) => x.OTRNID),
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
    this.GetOTRN();
    datepicker.close();
  }

  clearMonth() {
    this.month = new FormControl(moment(null));
    //  this.month.setValue(null);
    this.GetOTRN();
  }

  GetByOTRNMasterIDs() {
    this.http
      .postJSON("api/OTRN/GetOTRNExcelExportByIDs", this.OTRNMasterIDs)
      .subscribe((d: any) => {
        this.OTRNDetailsBulkArray = d;
        this.ExportToExcelBulkOTRN();
      });
  }

  ExportToExcel(OTRNID: number) {
    let filteredData = this.OTRNDetailsBulkArray;

    this.http
      .postJSON("api/OTRN/GetOTRNExcelExport/", {
        OTRNID: OTRNID,
        Month: this.month.value,
      })
      .subscribe((d: any) => {
        filteredData = d;

        let exportExcel: ExportExcel = {
          Title: "OTRN_Excel_Export",
          Author: this.currentUser.FullName,
          Type: EnumExcelReportType.OTRN,
          //Data: data,
          List1: this.populateData(filteredData),
        };
        this.dialogue.openExportConfirmDialogue(exportExcel).subscribe();
      });
  }
  ExportToExcelBulkOTRN() {
    //if (this.OTRNDetailsArray.length == 0) {
    //    this.http.ShowError("Please check OTRN Details to export data")
    //    return;
    //}
    let fileName = "OTRN";
    let filteredData = [...this.OTRNDetailsBulkArray];

    const result = filteredData.reduce(
      (accumulator, value) => accumulator.concat(value),
      []
    );
    //console.log(result);

    let exportExcel: ExportExcel = {
      Title: "OTRN Approval Report",
      Author: this.currentUser.FullName,
      Type: EnumExcelReportType.OTRN,
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
      r.traineesAmountWise[0].value[0].OTRNID == r.OTRNID
    ) {
      //r.traineesAmountWise = null;
      return;
    }
    this.http.getJSON("api/OTRN/GetOTRNDetails/" + r.OTRNID).subscribe(
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

export interface IOTRNApprovalFilter {
  SchemeID: number;
  //TSPID: number;
  TSPMasterID: number;
  KAMID: number;
}
