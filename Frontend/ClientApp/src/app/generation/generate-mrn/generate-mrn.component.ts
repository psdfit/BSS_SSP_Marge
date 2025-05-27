import { filter } from 'rxjs/operators';
import { Component, OnInit, AfterViewInit } from "@angular/core";
import { CommonSrvService } from "../../common-srv.service";
import { FormBuilder, FormControl } from "@angular/forms";
import { MatDatepicker } from "@angular/material/datepicker";
import * as _moment from "moment";
import { Moment } from "moment";
import {
  DateAdapter,
  MAT_DATE_LOCALE,
  MAT_DATE_FORMATS,
} from "@angular/material/core";
import {
  MomentDateAdapter,
  MAT_MOMENT_DATE_ADAPTER_OPTIONS,
} from "@angular/material-moment-adapter";
import { ExportExcel } from "../../shared/Interfaces";
import { GroupByPipe } from "angular-pipes";
const moment = _moment;
import { DatePipe } from "@angular/common";
import { DialogueService } from "src/app/shared/dialogue.service";
import { EnumExcelReportType } from "../../shared/Enumerations";
// Date formats
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
  selector: "app-generate-mrn",
  templateUrl: "./generate-mrn.component.html",
  styleUrls: ["./generate-mrn.component.scss"],
  providers: [
    {
      provide: DateAdapter,
      useClass: MomentDateAdapter,
      deps: [MAT_DATE_LOCALE, MAT_MOMENT_DATE_ADAPTER_OPTIONS],
    },
    { provide: MAT_DATE_FORMATS, useValue: MY_FORMATS },
    DatePipe,
    GroupByPipe,
  ],
})
export class GenerateMrnComponent implements OnInit {
  Scheme = [];
  Kam = [];
  TspDetail = [];
  PrnClasses: any = [];
  IsGenerated: boolean;
  message: string;
  filters: IFilters = { SchemeID: 0, Month: "" };
  DateMonth = new FormControl(moment());
  SearchSch = new FormControl("");
  SearchKam = new FormControl("");
  SearchTsp = new FormControl("");
  error = "";
  currentUser: any;
  kamFilter = new FormControl(0);
  schemeFilter = new FormControl(0);
  tspFilter = new FormControl(0);
  displayedColumnsClass = [
    "Sr",
    "SchemeName",
    "TSPName",
    "TradeName",
    "ClassCode",
    "StartDate",
    "EndDate",
    "ClassStatusName",
    "MedicalCost",
    "TotalEligibleTrainees",
  ];
  result: any;
  isValid = false;
  infoAlert: boolean;
  maxDate: Moment;
  today = new Date();
  constructor(
    private http: CommonSrvService,
    private fb: FormBuilder,
    private dialogue: DialogueService,
    private groupByPipe: GroupByPipe,
    private _date: DatePipe
  ) {
    this.http.setTitle("Generate MRN");
    this.maxDate = moment(); // Set maxDate to today's date as a Moment object
    console.log(this.maxDate.format("YYYY-MM-DD")); // Debug maxDate value
  }
  ngOnInit(): void {
    this.getSchemes();
    this.currentUser = this.http.getUserDetails();
    this.http.OID.subscribe(() => {
      this.filters.Month = "";
    });
  }
  // ngAfterViewInit(): void {
  //   this.getSchemes();
  // }
  getSchemes() {
    this.http.getJSON("api/Scheme/FetchSchemeForFilter").subscribe(
      (d: any) => {
        this.error = "";
        this.Scheme = d.filter((x) => this._date.transform(x.CreatedDate, "yyyy")>'2023');
      },
      (error) => {
        this.error = error;
      }
    );
  }
  //getTspClassesForVRNCompletion(value) {
  //  this.filters.Month = moment(this.DateMonth.value).format('YYYY-MM-DD');
  //  this.filters.SchemeID = value;
  //  this.isValid = false;
  //  this.IsGenerated = false;
  //  this.http.postJSON(`api/SRNCoursera/GetVRNReport`, this.filters).subscribe(
  //    (data: any) => {
  //      this.error = '';
  //      this.PrnClasses = data;
  //      debugger;
  //      this.IsGenerated = data.length > 0 && data[0] ? data[0].IsStipend : false;
  //      this.isValid = true;
  //      this.message = 'Already Generated.'
  //    },
  //    error => {
  //      this.error = error;
  //    })
  //}
  getTspClassesForVRNCompletion(value) {
    this.filters.Month = moment(this.DateMonth.value).format("YYYY-MM-DD");
    this.filters.SchemeID = value;
    this.isValid = false;
    this.IsGenerated = false;
    this.http
      .postJSON(`api/ManualNoteGeneration/GetMRN`, this.filters)
      .subscribe(
        (data: any) => {
          this.error = "";
          debugger;
          if (data && data.length > 0 && data[0].Message) {
            // SRN has already been generated for this month
            this.message = data[0].Message; // This will be "Already generated of this month"
            this.isValid = false;
            this.IsGenerated = true;
          } else {
            // Process the data as usual
            this.PrnClasses = data;
            this.IsGenerated =
              data.length > 0 && data[0] ? data[0].IsStipend : false;
            this.isValid = true;
            this.IsGenerated = false;
            this.message = "Already Generated.";
            
          }
        },
        (error) => {
          this.error = error;
        }
      );
  }
  generate() {
    this.delay(1000);
    this.http
      .confirm("MRN Generation", "Are You Sure you want to generate MRN?")
      .subscribe((result) => {
        if (result) {
          this.http
            .postJSON(`api/ManualNoteGeneration/GenerateMRN`, this.filters)
            .subscribe(
              (data: any) => {
                if (data) {
                  this.PrnClasses = [];
                  this.message = "Generated Successfully.";
                  this.IsGenerated = true;
                }
              },
              (error) => {
                this.error = error.error;
                this.http.ShowError(error.error);
              }
            );
        }
      });
  }
  exportToExcel() {
    const filteredData = [...this.PrnClasses];
    const data = {
      "Filters:": "",
      "Scheme(s)": this.groupByPipe
        .transform(filteredData, "Scheme")
        .map((x) => x.key)
        .join(","),
      "TSP(s)": this.groupByPipe
        .transform(filteredData, "TSP")
        .map((x) => x.key)
        .join(","),
      Batch: "All",
      Trade: "All",
      "Certification Agency": "All",
      Gender: "All",
      "District of Training Location": "All",
      "Class Status": "All",
    };
    const exportExcel: ExportExcel = {
      Title: "MRN Generation Report",
      Author: this.currentUser.FullName,
      Type: EnumExcelReportType.MasterSheet,
      Data: data,
      List1: this.populateData(filteredData),
    };
    this.dialogue.openExportConfirmDialogue(exportExcel).subscribe();
  }
  populateData(data: any) {
    return data.map((item) => {
      return {
        Scheme: item.SchemeName,
        "TSP Name": item.TSPName,
        "Trade Name": item.TradeName,
        "Class Code": item.ClassCode,
        "Class Start Date": this._date.transform(item.StartDate, "dd/MM/yyyy"),
        "Class End Date": this._date.transform(item.EndDate, "dd/MM/yyyy"),
        "Class Status": item.ClassStatusName,
        "Completion Report": item.ClassCode,
        "Total Eligible Trainees": item.TotalEligibleTrainees,
      };
    });
  }
  EmptyCtrl() {
    this.SearchKam.setValue("");
    this.SearchTsp.setValue("");
    this.SearchSch.setValue("");
  }
  chosenYearHandler(normalizedYear: Moment) {
    const ctrlValue = this.DateMonth.value;
    ctrlValue.year(normalizedYear.year());
    this.DateMonth.setValue(ctrlValue);
  }
  chosenMonthHandler(
    normalizedMonth: Moment,
    datepicker: MatDatepicker<Moment>
  ) {
    const ctrlValue = this.DateMonth.value;
    ctrlValue.month(normalizedMonth.month());
    this.DateMonth.setValue(ctrlValue);
    debugger;
    if (this.schemeFilter.value !== 0) {
      this.getTspClassesForVRNCompletion(this.schemeFilter.value);
    }
    datepicker.close();
  }
  delay(ms: number) {
    return new Promise((resolve) => setTimeout(resolve, ms));
  }
  openClassJourneyDialogue(data: any): void {
    this.dialogue.openClassJourneyDialogue(data);
  }
  displayHideAlert() {
    this.infoAlert = !this.infoAlert;
  }
}
export interface IFilters {
  SchemeID?: number;
  Month?: string;
}
