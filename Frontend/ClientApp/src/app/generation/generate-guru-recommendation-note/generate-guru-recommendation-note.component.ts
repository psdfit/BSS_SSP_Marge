import { Component, OnInit, AfterViewInit } from '@angular/core';
import { CommonSrvService } from '../../common-srv.service';
import { FormBuilder, FormControl } from '@angular/forms';
import { MatDatepicker } from '@angular/material/datepicker';
import * as _moment from 'moment';
import { Moment } from 'moment';
import { DateAdapter, MAT_DATE_LOCALE, MAT_DATE_FORMATS } from '@angular/material/core';
import { MomentDateAdapter, MAT_MOMENT_DATE_ADAPTER_OPTIONS } from '@angular/material-moment-adapter';

const moment = _moment;
import { DatePipe } from '@angular/common';
import { DialogueService } from 'src/app/shared/dialogue.service';
// See the Moment.js docs for the meaning of these formats:
// https://momentjs.com/docs/#/displaying/format/
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
  selector: 'app-generate-guru-recommendation-note',
  templateUrl: './generate-guru-recommendation-note.component.html',
  styleUrls: ['./generate-guru-recommendation-note.component.scss'],
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
    DatePipe
  ],
})

export class GenerateGuruRecommendationNoteComponent implements OnInit, AfterViewInit {


  Scheme = [];
  Kam = [];
  TspDetail = [];
  PrnClasses = [];
  ClassIDsArray = [];
  TotalCompletedClasses = 0;
  CompletedClassesWithResult = 0;
  IsGenerated : boolean;
  message: string;

  filters: IFilters = { SchemeID: 0, TspID: 0, UserID: 0, KamID: 0, Month: '', ClassIDs: '' };

  DateMonth = new FormControl(moment());
  SearchSch = new FormControl('');
  SearchKam = new FormControl('');
  SearchTsp = new FormControl('');
  error = '';

  currentUser: any;

  kamFilter = new FormControl(0);
  schemeFilter = new FormControl(0);
  tspFilter = new FormControl(0);

  displayedColumnsClass = ['Sr', 'ClassCode', 'Duration', 'ClassStatusName', 'StartDate', 'EndDate', 'TrainingAddressLocation', 'TradeName', 'GenderName', 'TraineesPerClass', 'CertAuthName'];

  result: any;
  isValid = false;
  infoAlert: boolean;


  constructor(private http: CommonSrvService, private fb: FormBuilder, private dialogue: DialogueService) {
    this.http.setTitle('Generate PRN Completion');
  }

  ngOnInit(): void {
    this.currentUser = this.http.getUserDetails();
    this.http.OID.subscribe(() => {
      this.filters.SchemeID = 0;
      this.filters.TspID = 0;
      this.filters.UserID = this.currentUser.UserID;
      this.filters.Month = '';
      this.filters.ClassIDs = '';
      this.getTspClassesForPRNCompletion(this.filters.TspID);
    })
  }
  ngAfterViewInit(): void {
    this.getSchemes();
    this.getKam();
    this.schemeFilter.valueChanges.subscribe(value => {this.filters.SchemeID = value; this.getTSPDetailByScheme(); });
    this.kamFilter.valueChanges.subscribe(value => {this.filters.KamID = value; this.getTSPDetailByScheme(); });
  }

  getSchemes() {
    this.http.getJSON('api/Scheme/FetchSchemeForFilter').subscribe(
      (d: any) => {
        this.error = '';
        this.Scheme = d;
      },
      error => {
          this.error = error;
      }
    );
  }
  getKam(){
    this.http.getJSON('api/KAMAssignment/RD_KAMAssignmentForFilters').subscribe(
      (d: any) => {
        this.error = '';
        this.Kam = d;
      },
      error => {
        this.error = error;
      }
    );
  }
  getTSPDetailByScheme() {
    this.TspDetail = [];
    this.tspFilter.setValue('');
    this.PrnClasses = [];
    this.TotalCompletedClasses = 0;
    this.CompletedClassesWithResult = 0;
    this.IsGenerated = false;
    this.filters.TspID = 0;
    this.ClassIDsArray = [];
    this.isValid = false;
    this.http.postJSON(`api/TSPDetail/GetTSPDetailForFilters`, this.filters) .subscribe(
      (data: any) => {
        this.error = '';
        this.TspDetail = data;
      },
      error => {
        this.error = error;
      })
  }

  getTspClassesForPRNCompletion(value) {
    this.filters.TspID = value;
    this.filters.Month = moment(this.DateMonth.value).format('YYYY-MM-DD');
    this.ClassIDsArray = [];
    this.isValid = false;
    this.IsGenerated = false;
    this.http.postJSON(`api/GenerateGuruRecommendationNote/RD_GuruRecommendationNote`, this.filters) .subscribe(
      (data: any) => {
        this.error = '';
        this.PrnClasses = data[0];
        this.TotalCompletedClasses = data[1];
        this.CompletedClassesWithResult = data[2];
        this.IsGenerated = data[3];
        if(this.IsGenerated)
        {
          this.PrnClasses = [];
          this.isValid = false;
          this.message = 'Already Generated.'
        }
        else
        {
          if(this.TotalCompletedClasses === this.CompletedClassesWithResult)
            this.isValid = true;
          else
            this.isValid = false;
        }
      },
      error => {
        this.error = error;
      })
  }
  generatePrnCompletion() {
    this.delay(1000);
    // this.filters.ClassIDs = this.PrnClasses.map(x=>x.ClassID).join(',');
    this.ClassIDsArray = this.PrnClasses.map((o: { ClassID: any; }) => o.ClassID);
    this.filters.ClassIDs = this.ClassIDsArray.join(',');
    this.http.confirm('PRN Completion Generation','Are You Sure?').subscribe(result => {
      if(result){
        this.http.postJSON(`api/Generation/GeneratePRNCompletion`, this.filters) .subscribe(
          (data: any) => {
            if(data){
              this.PrnClasses = [];
              this.TotalCompletedClasses = 0;
              this.CompletedClassesWithResult = 0;
              this.message = 'Generated Successfully.';
              this.IsGenerated = true;
              // this.http.openSnackBar('Generated successfully','',2000);
              // this.getTspClassesForPRNCompletion(this.filters.TspID);
            }
          },(error) => {
            this.error = error.error;
            this.http.ShowError(error.error);
          }
        )
      }
      else {
      }
    });
  }

  EmptyCtrl() {
    this.SearchKam.setValue('');
    this.SearchTsp.setValue('');
    this.SearchSch.setValue('');
  }

  chosenYearHandler(normalizedYear: Moment) {
    const ctrlValue = this.DateMonth.value;
    ctrlValue.year(normalizedYear.year());
    this.DateMonth.setValue(ctrlValue);
  }
  chosenMonthHandler(normalizedMonth: Moment,datepicker: MatDatepicker<Moment>) {
    const ctrlValue = this.DateMonth.value;
    ctrlValue.month(normalizedMonth.month());
    this.DateMonth.setValue(ctrlValue);
    if(this.filters.TspID !== 0)
      this.getTspClassesForPRNCompletion(this.filters.TspID);
    datepicker.close();
  }

  delay(ms: number) {
    return new Promise( resolve => setTimeout(resolve, ms) );
  }

  openClassJourneyDialogue(data: any): void
  {
		this.dialogue.openClassJourneyDialogue(data);
  }
  displayHideAlert() {
    if (this.infoAlert) {
      this.infoAlert = false;
    }
    else {
      this.infoAlert = true;
    }
  }
}

export interface IFilters {
  SchemeID?: number;
  TspID?: number;
  UserID?: number;
  KamID?: number;
  Month?: string;
  ClassIDs: string;
}
