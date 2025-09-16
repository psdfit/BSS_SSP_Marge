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
import { TakamolRecommendationNoteApprovalsDialogueComponent } from "../takamol-recommendation-note-approvals-dialogue/takamol-recommendation-note-approvals-dialogue.component";

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
  selector: "app-takamol-recommendation-note-approvals",
  templateUrl: "./takamol-recommendation-note-approvals.component.html",
  styleUrls: ["./takamol-recommendation-note-approvals.component.scss"],

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
export class TakamolRecommendationNoteApprovalsComponent implements OnInit {
  environment = environment;

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  //resultsLength: any;
  //dtTakamolRecommendationNoteDataDisplayedColumns = ['TSPName', 'TradeName', 'ClassCode', 'TraineeCode', 'TraineeName', 'FatherName', 'TraineeCNIC', 'DistrictName', 'ContactNumber1', 'TrainingAddressLocation', 'Comments', 'Batch', 'TokenNumber', 'TransactionNumber', 'Status', 'Month', 'NumberOfMonths', 'Action'];
  //dtTakamolRecommendationNoteData: MatTableDataSource<any>;
  TakamolRecommendationNote: any[] = [];
  TakamolRecommendationNoteApproved: any[] = [];
  TakamolRecommendationNotePending: any[] = [];
  TakamolRecommendationNoteGrouped: any[] = [];
  TakamolRecommendationNoteGroupedByBatch: any[] = [];
  varProcessKey: string;
  //TakamolRecommendationNoteGroupList: any[] = [];
  // TakamolRecommendationNoteDetails: any[];
  errorHTTP: any;
  month = new FormControl(moment());
  currentUser: any;
  kamusers: [];
  schemes: [];
  tsps: [];
  tspMasters: [];
  TakamolRecommendationNoteDetailsArray: any[];
  TakamolRecommendationNoteDetailsBulkArray: any[];

  TakamolRecommendationNoteMasterArray: any[];
  TakamolRecommendationNoteMasterIDs: string;

  SearchSch = new FormControl("");
  SearchKAM = new FormControl("");
  SearchTSP = new FormControl("");

  //filters: ITakamolRecommendationNoteApprovalFilter = { SchemeID: 0, TSPID: 0, KAMID: 0 };
  filters: ITakamolRecommendationNoteApprovalFilter = { SchemeID: 0, TSPMasterID: 0, KAMID: 0 };

  constructor(
    private datePipe: DatePipe,
    private http: CommonSrvService,
    public dialog: MatDialog,
    private overlay: Overlay,
    private dialogue: DialogueService,
    private groupByPipe: GroupByPipe
  ) {}

  ngOnInit(): void {
    this.http.setTitle("TakamolRecommendationNote");
    this.currentUser = this.http.getUserDetails();
    this.TakamolRecommendationNoteDetailsArray = [];
    this.http.OID.subscribe((OID) => {
      this.GetTakamolRecommendationNote();
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

  GetTakamolRecommendationNote() {
    //let month = new Date('2020-03-01');
    //this.http.postJSON(`api/TakamolRecommendationNote/GetTakamolRecommendationNote`, { Month: this.month.value, KAMID: this.filters.KAMID, SchemeID: this.filters.SchemeID, TSPID: this.filters.TSPID }).subscribe(
    this.http
      .postJSON(`api/TakamolRecommendationNote/GetTakamolRecommendationNote`, {
        Month: this.month.value,
        KAMID: this.filters.KAMID,
        SchemeID: this.filters.SchemeID,
        TSPMasterID: this.filters.TSPMasterID,
      })
      .subscribe(
        (data: any) => {
          console.log(data);
          this.TakamolRecommendationNoteMasterArray = data.map((o) => o.TakamolRecommendationNoteID);
          this.TakamolRecommendationNoteMasterIDs = this.TakamolRecommendationNoteMasterArray.join(",");
          this.TakamolRecommendationNote = [];
          this.TakamolRecommendationNoteGroupedByBatch = this.groupByPipe.transform(
            data,
            "ApprovalBatchNo"
          );
          let indexTakamolRecommendationNote = 0;
          this.TakamolRecommendationNoteGroupedByBatch.forEach((key) => {
            var number = indexTakamolRecommendationNote; //Number(key.key) - 1;
            //this.TakamolRecommendationNoteGrouped = this.groupByPipe.transform(this.TakamolRecommendationNoteGroupedByBatch[key], "IsApproved")

            this.TakamolRecommendationNoteGrouped = this.groupByPipe
              .transform(this.TakamolRecommendationNoteGroupedByBatch[number].value, "SchemeName")
              .map((x) => {
                return {
                  ...x,
                  toggleValue: true,
                  IsApproved: x.value[0]["IsApproved"],
                };
              });

            this.TakamolRecommendationNote.push(this.TakamolRecommendationNoteGrouped);
            indexTakamolRecommendationNote++;
          });
          this.TakamolRecommendationNote = this.TakamolRecommendationNote.reduce(
            (accumulator, value) => accumulator.concat(value),
            []
          );
          //this.TakamolRecommendationNoteGrouped = this.groupByPipe.transform(data, "IsApproved")
          //this.TakamolRecommendationNoteMasterArray = data.map(o => o.TakamolRecommendationNoteID);
          //this.TakamolRecommendationNoteMasterIDs = this.TakamolRecommendationNoteMasterArray.join(',');
          //this.TakamolRecommendationNotePending = this.groupByPipe.transform(this.TakamolRecommendationNoteGrouped[1].value, "SchemeName")
          //  .map(x => {
          //    return { ...x, toggleValue: true, IsApproved: this.TakamolRecommendationNoteGrouped[1].key }
          //  });
          //this.TakamolRecommendationNote.push(this.TakamolRecommendationNotePending);
          //this.TakamolRecommendationNoteApproved = this.groupByPipe.transform(this.TakamolRecommendationNoteGrouped[0].value, "SchemeName")
          //  .map(x => {
          //    return { ...x, toggleValue: true, IsApproved: this.TakamolRecommendationNoteGrouped[0].key }
          //  });
          //this.TakamolRecommendationNote.push(this.TakamolRecommendationNoteApproved);

          //console.log(this.TakamolRecommendationNote);
          //this.TakamolRecommendationNote = this.TakamolRecommendationNote.reduce((accumulator, value) => accumulator.concat(value), []);

          //this.TakamolRecommendationNoteGrouped = this.groupByPipe.transform(data, "IsApproved")
          //this.TakamolRecommendationNoteMasterArray = data.map(o => o.TakamolRecommendationNoteID);
          //this.TakamolRecommendationNoteMasterIDs = this.TakamolRecommendationNoteMasterArray.join(',');
          //this.TakamolRecommendationNotePending = this.groupByPipe.transform(this.TakamolRecommendationNoteGrouped[1].value, "SchemeName")
          //  .map(x => {
          //    return { ...x, toggleValue: true, IsApproved: this.TakamolRecommendationNoteGrouped[1].key }
          //  });
          //this.TakamolRecommendationNote.push(this.TakamolRecommendationNotePending);
          //this.TakamolRecommendationNoteApproved = this.groupByPipe.transform(this.TakamolRecommendationNoteGrouped[0].value, "SchemeName")
          //  .map(x => {
          //    return { ...x, toggleValue: true, IsApproved: this.TakamolRecommendationNoteGrouped[0].key }
          //  });
          //this.TakamolRecommendationNote.push(this.TakamolRecommendationNoteApproved);

          //console.log(this.TakamolRecommendationNote);
          //this.TakamolRecommendationNote = this.TakamolRecommendationNote.reduce((accumulator, value) => accumulator.concat(value), []);
        },

        (error) => {
          this.http.ShowError(error.error + "\n" + error.message);
        }
      );
  }
  GetTakamolRecommendationNoteDetails(r: any) {
    if (r.TakamolRecommendationNoteDetails) {
      r.TakamolRecommendationNoteDetails = null;
      this.TakamolRecommendationNoteDetailsArray = this.TakamolRecommendationNoteDetailsArray.filter(
        (s) => s.TakamolRecommendationNoteID != r.TakamolRecommendationNoteID
      );

      return;
    }
    this.http.getJSON("api/TakamolRecommendationNote/GetTakamolRecommendationNoteDetails/" + r.TakamolRecommendationNoteID).subscribe(
      (data: any) => {
        r.TakamolRecommendationNoteDetails = data[0];
        this.TakamolRecommendationNoteDetailsArray.push(data[0]);
        this.TakamolRecommendationNoteDetailsArray = this.TakamolRecommendationNoteDetailsArray.reduce(
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
  //  //{ ProcessKey: 'AP', FormID:  row.TakamolRecommendationNoteId }
  //  //let datas: IApprovalHistory = { ProcessKey: 'AP', FormID: 292 };
  //  this.dialogue.openApprovalDialogue(EnumApprovalProcess.TakamolRecommendationNote, row.TakamolRecommendationNoteID).subscribe(result => { console.log(result); });
  //}

  public openTakamolRecommendationNoteApprovalDialogue(TakamolRecommendationNoteGroup: any[]): void {
    console.log(TakamolRecommendationNoteGroup);
    debugger;
    //let datas: IApprovalHistory = { ProcessKey: processKey, FormID: formID };
    var processk = TakamolRecommendationNoteGroup.map((x) => x.ProcessKey);

    const dialogRef = this.dialog.open(TakamolRecommendationNoteApprovalsDialogueComponent, {
      width: "60%",
      data: {
        ProcessKey: EnumApprovalProcess.TakamolRecommendationNote,
        FormIDs: TakamolRecommendationNoteGroup.map((x) => x.TakamolRecommendationNoteID),
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
    this.GetTakamolRecommendationNote();
    datepicker.close();
  }

  clearMonth() {
    this.month = new FormControl(moment(null));
    //  this.month.setValue(null);
    this.GetTakamolRecommendationNote();
  }

  GetByTakamolRecommendationNoteMasterIDs() {
    this.http
      .postJSON("api/TakamolRecommendationNote/GetTakamolRecommendationNoteExcelExportByIDs", this.TakamolRecommendationNoteMasterIDs)
      .subscribe((d: any) => {
        this.TakamolRecommendationNoteDetailsBulkArray = d;
        this.ExportToExcelBulkTakamolRecommendationNote();
      });
  }

  ExportToExcel(TakamolRecommendationNoteID: number) {
    let filteredData = this.TakamolRecommendationNoteDetailsBulkArray;

    this.http
      .postJSON("api/TakamolRecommendationNote/GetTakamolRecommendationNoteExcelExport/", {
        TakamolRecommendationNoteID: TakamolRecommendationNoteID,
        Month: this.month.value,
      })
      .subscribe((d: any) => {
        filteredData = d;

        let exportExcel: ExportExcel = {
          Title: "TakamolRecommendationNote_Excel_Export",
          Author: this.currentUser.FullName,
          Type: EnumExcelReportType.TakamolRecommendationNote,
          //Data: data,
          List1: this.populateData(filteredData),
        };
        this.dialogue.openExportConfirmDialogue(exportExcel).subscribe();
      });
  }
  ExportToExcelBulkTakamolRecommendationNote() {
    //if (this.TakamolRecommendationNoteDetailsArray.length == 0) {
    //    this.http.ShowError("Please check TakamolRecommendationNote Details to export data")
    //    return;
    //}
    let fileName = "TakamolRecommendationNote";
    let filteredData = [...this.TakamolRecommendationNoteDetailsBulkArray];

    const result = filteredData.reduce(
      (accumulator, value) => accumulator.concat(value),
      []
    );
    //console.log(result);

    let exportExcel: ExportExcel = {
      Title: "TakamolRecommendationNote Approval Report",
      Author: this.currentUser.FullName,
      Type: EnumExcelReportType.TakamolRecommendationNote,
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
      r.traineesAmountWise[0].value[0].TakamolRecommendationNoteID == r.TakamolRecommendationNoteID
    ) {
      //r.traineesAmountWise = null;
      return;
    }
    this.http.getJSON("api/TakamolRecommendationNote/GetTakamolRecommendationNoteDetails/" + r.TakamolRecommendationNoteID).subscribe(
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

export interface ITakamolRecommendationNoteApprovalFilter {
  SchemeID: number;
  //TSPID: number;
  TSPMasterID: number;
  KAMID: number;
}
