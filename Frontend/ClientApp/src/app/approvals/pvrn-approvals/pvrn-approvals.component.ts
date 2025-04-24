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
import { PvrnApprovalsDialogueComponent } from "../pvrn-approvals-dialogue/pvrn-approvals-dialogue.component";

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
  selector: "app-pvrn-approvals",
  templateUrl: "./pvrn-approvals.component.html",
  styleUrls: ["./pvrn-approvals.component.scss"],

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
export class PvrnApprovalsComponent implements OnInit {
  environment = environment;

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  //resultsLength: any;
  //dtPVRNDataDisplayedColumns = ['TSPName', 'TradeName', 'ClassCode', 'TraineeCode', 'TraineeName', 'FatherName', 'TraineeCNIC', 'DistrictName', 'ContactNumber1', 'TrainingAddressLocation', 'Comments', 'Batch', 'TokenNumber', 'TransactionNumber', 'Status', 'Month', 'NumberOfMonths', 'Action'];
  //dtPVRNData: MatTableDataSource<any>;
  PVRN: any[] = [];
  PVRNApproved: any[] = [];
  PVRNPending: any[] = [];
  PVRNGrouped: any[] = [];
  PVRNGroupedByBatch: any[] = [];
  varProcessKey: string;
  //PVRNGroupList: any[] = [];
  // PVRNDetails: any[];
  errorHTTP: any;
  month = new FormControl(moment());
  currentUser: any;
  kamusers: [];
  schemes: [];
  tsps: [];
  tspMasters: [];
  PVRNDetailsArray: any[];
  PVRNDetailsBulkArray: any[];

  PVRNMasterArray: any[];
  PVRNMasterIDs: string;

  SearchSch = new FormControl("");
  SearchKAM = new FormControl("");
  SearchTSP = new FormControl("");

  //filters: IPVRNApprovalFilter = { SchemeID: 0, TSPID: 0, KAMID: 0 };
  filters: IPVRNApprovalFilter = { SchemeID: 0, TSPMasterID: 0, KAMID: 0 };

  constructor(
    private datePipe: DatePipe,
    private http: CommonSrvService,
    public dialog: MatDialog,
    private overlay: Overlay,
    private dialogue: DialogueService,
    private groupByPipe: GroupByPipe
  ) {}

  ngOnInit(): void {
    this.http.setTitle("PVRN");
    this.currentUser = this.http.getUserDetails();
    this.PVRNDetailsArray = [];
    this.http.OID.subscribe((OID) => {
      this.GetPVRN();
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

  GetPVRN() {
    //let month = new Date('2020-03-01');
    //this.http.postJSON(`api/PVRN/GetPVRN`, { Month: this.month.value, KAMID: this.filters.KAMID, SchemeID: this.filters.SchemeID, TSPID: this.filters.TSPID }).subscribe(
    this.http
      .postJSON(`api/PVRN/GetPVRN`, {
        Month: this.month.value,
        KAMID: this.filters.KAMID,
        SchemeID: this.filters.SchemeID,
        TSPMasterID: this.filters.TSPMasterID,
      })
      .subscribe(
        (data: any) => {
          console.log(data);
          this.PVRNMasterArray = data.map((o) => o.PVRNID);
          this.PVRNMasterIDs = this.PVRNMasterArray.join(",");
          this.PVRN = [];
          this.PVRNGroupedByBatch = this.groupByPipe.transform(
            data,
            "ApprovalBatchNo"
          );
          let indexPVRN = 0;
          this.PVRNGroupedByBatch.forEach((key) => {
            var number = indexPVRN; //Number(key.key) - 1;
            //this.PVRNGrouped = this.groupByPipe.transform(this.PVRNGroupedByBatch[key], "IsApproved")

            this.PVRNGrouped = this.groupByPipe
              .transform(this.PVRNGroupedByBatch[number].value, "SchemeName")
              .map((x) => {
                return {
                  ...x,
                  toggleValue: true,
                  IsApproved: x.value[0]["IsApproved"],
                };
              });

            this.PVRN.push(this.PVRNGrouped);
            indexPVRN++;
          });
          this.PVRN = this.PVRN.reduce(
            (accumulator, value) => accumulator.concat(value),
            []
          );
          //this.PVRNGrouped = this.groupByPipe.transform(data, "IsApproved")
          //this.PVRNMasterArray = data.map(o => o.PVRNID);
          //this.PVRNMasterIDs = this.PVRNMasterArray.join(',');
          //this.PVRNPending = this.groupByPipe.transform(this.PVRNGrouped[1].value, "SchemeName")
          //  .map(x => {
          //    return { ...x, toggleValue: true, IsApproved: this.PVRNGrouped[1].key }
          //  });
          //this.PVRN.push(this.PVRNPending);
          //this.PVRNApproved = this.groupByPipe.transform(this.PVRNGrouped[0].value, "SchemeName")
          //  .map(x => {
          //    return { ...x, toggleValue: true, IsApproved: this.PVRNGrouped[0].key }
          //  });
          //this.PVRN.push(this.PVRNApproved);

          //console.log(this.PVRN);
          //this.PVRN = this.PVRN.reduce((accumulator, value) => accumulator.concat(value), []);

          //this.PVRNGrouped = this.groupByPipe.transform(data, "IsApproved")
          //this.PVRNMasterArray = data.map(o => o.PVRNID);
          //this.PVRNMasterIDs = this.PVRNMasterArray.join(',');
          //this.PVRNPending = this.groupByPipe.transform(this.PVRNGrouped[1].value, "SchemeName")
          //  .map(x => {
          //    return { ...x, toggleValue: true, IsApproved: this.PVRNGrouped[1].key }
          //  });
          //this.PVRN.push(this.PVRNPending);
          //this.PVRNApproved = this.groupByPipe.transform(this.PVRNGrouped[0].value, "SchemeName")
          //  .map(x => {
          //    return { ...x, toggleValue: true, IsApproved: this.PVRNGrouped[0].key }
          //  });
          //this.PVRN.push(this.PVRNApproved);

          //console.log(this.PVRN);
          //this.PVRN = this.PVRN.reduce((accumulator, value) => accumulator.concat(value), []);
        },

        (error) => {
          this.http.ShowError(error.error + "\n" + error.message);
        }
      );
  }
  GetPVRNDetails(r: any) {
    if (r.PVRNDetails) {
      r.PVRNDetails = null;
      this.PVRNDetailsArray = this.PVRNDetailsArray.filter(
        (s) => s.PVRNID != r.PVRNID
      );

      return;
    }
    this.http.getJSON("api/PVRN/GetPVRNDetails/" + r.PVRNID).subscribe(
      (data: any) => {
        r.PVRNDetails = data[0];
        this.PVRNDetailsArray.push(data[0]);
        this.PVRNDetailsArray = this.PVRNDetailsArray.reduce(
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
  //  //{ ProcessKey: 'AP', FormID:  row.PVRNId }
  //  //let datas: IApprovalHistory = { ProcessKey: 'AP', FormID: 292 };
  //  this.dialogue.openApprovalDialogue(EnumApprovalProcess.PVRN, row.PVRNID).subscribe(result => { console.log(result); });
  //}

  public openPVRNApprovalDialogue(PVRNGroup: any[]): void {
    console.log(PVRNGroup);
    debugger;
    //let datas: IApprovalHistory = { ProcessKey: processKey, FormID: formID };
    var processk = PVRNGroup.map((x) => x.ProcessKey);

    const dialogRef = this.dialog.open(PvrnApprovalsDialogueComponent, {
      width: "60%",
      data: {
        ProcessKey: EnumApprovalProcess.PVRN,
        FormIDs: PVRNGroup.map((x) => x.PVRNID),
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
    this.GetPVRN();
    datepicker.close();
  }

  clearMonth() {
    this.month = new FormControl(moment(null));
    //  this.month.setValue(null);
    this.GetPVRN();
  }

  GetByPVRNMasterIDs() {
    this.http
      .postJSON("api/PVRN/GetPVRNExcelExportByIDs", this.PVRNMasterIDs)
      .subscribe((d: any) => {
        this.PVRNDetailsBulkArray = d;
        this.ExportToExcelBulkPVRN();
      });
  }

  ExportToExcel(PVRNID: number) {
    let filteredData = this.PVRNDetailsBulkArray;

    this.http
      .postJSON("api/PVRN/GetPVRNExcelExport/", {
        PVRNID: PVRNID,
        Month: this.month.value,
      })
      .subscribe((d: any) => {
        filteredData = d;

        let exportExcel: ExportExcel = {
          Title: "PVRN_Excel_Export",
          Author: this.currentUser.FullName,
          Type: EnumExcelReportType.PVRN,
          //Data: data,
          List1: this.populateData(filteredData),
        };
        this.dialogue.openExportConfirmDialogue(exportExcel).subscribe();
      });
  }
  ExportToExcelBulkPVRN() {
    //if (this.PVRNDetailsArray.length == 0) {
    //    this.http.ShowError("Please check PVRN Details to export data")
    //    return;
    //}
    let fileName = "PVRN";
    let filteredData = [...this.PVRNDetailsBulkArray];

    const result = filteredData.reduce(
      (accumulator, value) => accumulator.concat(value),
      []
    );
    //console.log(result);

    let exportExcel: ExportExcel = {
      Title: "PVRN Approval Report",
      Author: this.currentUser.FullName,
      Type: EnumExcelReportType.PVRN,
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
      r.traineesAmountWise[0].value[0].PVRNID == r.PVRNID
    ) {
      //r.traineesAmountWise = null;
      return;
    }
    this.http.getJSON("api/PVRN/GetPVRNDetails/" + r.PVRNID).subscribe(
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

export interface IPVRNApprovalFilter {
  SchemeID: number;
  //TSPID: number;
  TSPMasterID: number;
  KAMID: number;
}
