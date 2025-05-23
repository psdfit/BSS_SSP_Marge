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
import { MrnApprovalsDialogueComponent } from "../mrn-approvals-dialogue/mrn-approvals-dialogue.component";

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
  selector: "app-mrn-approvals",
  templateUrl: "./mrn-approvals.component.html",
  styleUrls: ["./mrn-approvals.component.scss"],

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
export class MrnApprovalsComponent implements OnInit {
  environment = environment;

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  //resultsLength: any;
  //dtMRNDataDisplayedColumns = ['TSPName', 'TradeName', 'ClassCode', 'TraineeCode', 'TraineeName', 'FatherName', 'TraineeCNIC', 'DistrictName', 'ContactNumber1', 'TrainingAddressLocation', 'Comments', 'Batch', 'TokenNumber', 'TransactionNumber', 'Status', 'Month', 'NumberOfMonths', 'Action'];
  //dtMRNData: MatTableDataSource<any>;
  MRN: any[] = [];
  MRNApproved: any[] = [];
  MRNPending: any[] = [];
  MRNGrouped: any[] = [];
  MRNGroupedByBatch: any[] = [];
  varProcessKey: string;
  //MRNGroupList: any[] = [];
  // MRNDetails: any[];
  errorHTTP: any;
  month = new FormControl(moment());
  currentUser: any;
  kamusers: [];
  schemes: [];
  tsps: [];
  tspMasters: [];
  MRNDetailsArray: any[];
  MRNDetailsBulkArray: any[];

  MRNMasterArray: any[];
  MRNMasterIDs: string;

  SearchSch = new FormControl("");
  SearchKAM = new FormControl("");
  SearchTSP = new FormControl("");

  //filters: IMRNApprovalFilter = { SchemeID: 0, TSPID: 0, KAMID: 0 };
  filters: IMRNApprovalFilter = { SchemeID: 0, TSPMasterID: 0, KAMID: 0 };

  constructor(
    private datePipe: DatePipe,
    private http: CommonSrvService,
    public dialog: MatDialog,
    private overlay: Overlay,
    private dialogue: DialogueService,
    private groupByPipe: GroupByPipe
  ) {}

  ngOnInit(): void {
    this.http.setTitle("MRN");
    this.currentUser = this.http.getUserDetails();
    this.MRNDetailsArray = [];
    this.http.OID.subscribe((OID) => {
      this.GetMRN();
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

  GetMRN() {
    //let month = new Date('2020-03-01');
    //this.http.postJSON(`api/MRN/GetMRN`, { Month: this.month.value, KAMID: this.filters.KAMID, SchemeID: this.filters.SchemeID, TSPID: this.filters.TSPID }).subscribe(
    this.http
      .postJSON(`api/MRN/GetMRN`, {
        Month: this.month.value,
        KAMID: this.filters.KAMID,
        SchemeID: this.filters.SchemeID,
        TSPMasterID: this.filters.TSPMasterID,
      })
      .subscribe(
        (data: any) => {
          console.log(data);
          this.MRNMasterArray = data.map((o) => o.MRNID);
          this.MRNMasterIDs = this.MRNMasterArray.join(",");
          this.MRN = [];
          this.MRNGroupedByBatch = this.groupByPipe.transform(
            data,
            "ApprovalBatchNo"
          );
          let indexMRN = 0;
          this.MRNGroupedByBatch.forEach((key) => {
            var number = indexMRN; //Number(key.key) - 1;
            //this.MRNGrouped = this.groupByPipe.transform(this.MRNGroupedByBatch[key], "IsApproved")

            this.MRNGrouped = this.groupByPipe
              .transform(this.MRNGroupedByBatch[number].value, "SchemeName")
              .map((x) => {
                return {
                  ...x,
                  toggleValue: true,
                  IsApproved: x.value[0]["IsApproved"],
                };
              });

            this.MRN.push(this.MRNGrouped);
            indexMRN++;
          });
          this.MRN = this.MRN.reduce(
            (accumulator, value) => accumulator.concat(value),
            []
          );
          //this.MRNGrouped = this.groupByPipe.transform(data, "IsApproved")
          //this.MRNMasterArray = data.map(o => o.MRNID);
          //this.MRNMasterIDs = this.MRNMasterArray.join(',');
          //this.MRNPending = this.groupByPipe.transform(this.MRNGrouped[1].value, "SchemeName")
          //  .map(x => {
          //    return { ...x, toggleValue: true, IsApproved: this.MRNGrouped[1].key }
          //  });
          //this.MRN.push(this.MRNPending);
          //this.MRNApproved = this.groupByPipe.transform(this.MRNGrouped[0].value, "SchemeName")
          //  .map(x => {
          //    return { ...x, toggleValue: true, IsApproved: this.MRNGrouped[0].key }
          //  });
          //this.MRN.push(this.MRNApproved);

          //console.log(this.MRN);
          //this.MRN = this.MRN.reduce((accumulator, value) => accumulator.concat(value), []);

          //this.MRNGrouped = this.groupByPipe.transform(data, "IsApproved")
          //this.MRNMasterArray = data.map(o => o.MRNID);
          //this.MRNMasterIDs = this.MRNMasterArray.join(',');
          //this.MRNPending = this.groupByPipe.transform(this.MRNGrouped[1].value, "SchemeName")
          //  .map(x => {
          //    return { ...x, toggleValue: true, IsApproved: this.MRNGrouped[1].key }
          //  });
          //this.MRN.push(this.MRNPending);
          //this.MRNApproved = this.groupByPipe.transform(this.MRNGrouped[0].value, "SchemeName")
          //  .map(x => {
          //    return { ...x, toggleValue: true, IsApproved: this.MRNGrouped[0].key }
          //  });
          //this.MRN.push(this.MRNApproved);

          //console.log(this.MRN);
          //this.MRN = this.MRN.reduce((accumulator, value) => accumulator.concat(value), []);
        },

        (error) => {
          this.http.ShowError(error.error + "\n" + error.message);
        }
      );
  }
  GetMRNDetails(r: any) {
    if (r.MRNDetails) {
      r.MRNDetails = null;
      this.MRNDetailsArray = this.MRNDetailsArray.filter(
        (s) => s.MRNID != r.MRNID
      );

      return;
    }
    this.http.getJSON("api/MRN/GetMRNDetails/" + r.MRNID).subscribe(
      (data: any) => {
        r.MRNDetails = data[0];
        this.MRNDetailsArray.push(data[0]);
        this.MRNDetailsArray = this.MRNDetailsArray.reduce(
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
  //  //{ ProcessKey: 'AP', FormID:  row.MRNId }
  //  //let datas: IApprovalHistory = { ProcessKey: 'AP', FormID: 292 };
  //  this.dialogue.openApprovalDialogue(EnumApprovalProcess.MRN, row.MRNID).subscribe(result => { console.log(result); });
  //}

  public openMRNApprovalDialogue(MRNGroup: any[]): void {
    console.log(MRNGroup);
    debugger;
    //let datas: IApprovalHistory = { ProcessKey: processKey, FormID: formID };
    var processk = MRNGroup.map((x) => x.ProcessKey);

    const dialogRef = this.dialog.open(MrnApprovalsDialogueComponent, {
      width: "60%",
      data: {
        ProcessKey: EnumApprovalProcess.MRN,
        FormIDs: MRNGroup.map((x) => x.MRNID),
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
    this.GetMRN();
    datepicker.close();
  }

  clearMonth() {
    this.month = new FormControl(moment(null));
    //  this.month.setValue(null);
    this.GetMRN();
  }

  GetByMRNMasterIDs() {
    this.http
      .postJSON("api/MRN/GetMRNExcelExportByIDs", this.MRNMasterIDs)
      .subscribe((d: any) => {
        this.MRNDetailsBulkArray = d;
        this.ExportToExcelBulkMRN();
      });
  }

  ExportToExcel(MRNID: number) {
    let filteredData = this.MRNDetailsBulkArray;

    this.http
      .postJSON("api/MRN/GetMRNExcelExport/", {
        MRNID: MRNID,
        Month: this.month.value,
      })
      .subscribe((d: any) => {
        filteredData = d;

        let exportExcel: ExportExcel = {
          Title: "MRN_Excel_Export",
          Author: this.currentUser.FullName,
          Type: EnumExcelReportType.MRN,
          //Data: data,
          List1: this.populateData(filteredData),
        };
        this.dialogue.openExportConfirmDialogue(exportExcel).subscribe();
      });
  }
  ExportToExcelBulkMRN() {
    //if (this.MRNDetailsArray.length == 0) {
    //    this.http.ShowError("Please check MRN Details to export data")
    //    return;
    //}
    let fileName = "MRN";
    let filteredData = [...this.MRNDetailsBulkArray];

    const result = filteredData.reduce(
      (accumulator, value) => accumulator.concat(value),
      []
    );
    //console.log(result);

    let exportExcel: ExportExcel = {
      Title: "MRN Approval Report",
      Author: this.currentUser.FullName,
      Type: EnumExcelReportType.MRN,
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
      r.traineesAmountWise[0].value[0].MRNID == r.MRNID
    ) {
      //r.traineesAmountWise = null;
      return;
    }
    this.http.getJSON("api/MRN/GetMRNDetails/" + r.MRNID).subscribe(
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

export interface IMRNApprovalFilter {
  SchemeID: number;
  //TSPID: number;
  TSPMasterID: number;
  KAMID: number;
}
