import { Component, OnInit, AfterViewInit } from '@angular/core';
import { FormBuilder, FormControl, FormArray, FormGroup, Validators, AbstractControl } from '@angular/forms';
import { CommonSrvService } from '../../common-srv.service';
import { EnumReports, EnumReportsFilters, EnumSubReports } from '../../shared/Enumerations';
import { UsersModel } from '../../master-data/users/users.component';

import { MatDatepicker } from '@angular/material/datepicker';
import * as _moment from 'moment';
import { Moment } from 'moment';
import { DateAdapter, MAT_DATE_LOCALE, MAT_DATE_FORMATS } from '@angular/material/core';
import { MomentDateAdapter, MAT_MOMENT_DATE_ADAPTER_OPTIONS } from '@angular/material-moment-adapter';
// import { Select2OptionData } from 'ng-select2';
import * as XLSX from 'xlsx';

const moment = _moment;
import { DatePipe } from '@angular/common';
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
  selector: 'app-view-reports',
  templateUrl: './view-reports.component.html',
  styleUrls: ['./view-reports.component.scss'],
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

export class ViewReportsComponent implements OnInit, AfterViewInit {
  currentUser: UsersModel;
  roleID: number;
  titleName: string;
  error = '';
  reportsArray = [];
  subReportsArray = [];
  filtersData = [];
  filters = [];
  DateMonth: any;
  SearchRName = new FormControl('');
  SearchSRName = new FormControl('');
  date = new FormControl(moment());
  startDate = new FormControl(moment());
  endDate = new FormControl(moment());
  ID: number;
  SearchFilters: any;

  genForm: FormGroup;

  constructor(private ComSrv: CommonSrvService, private fb: FormBuilder) {
    this.ComSrv.setTitle('Reports');
  }

  ngOnInit(): void {
    this.currentUser = this.ComSrv.getUserDetails();
    this.roleID = this.currentUser.RoleID;
    this.titleName = this.currentUser.RoleTitle + ' ' + 'Reports';
    this.genForm = this.fb.group({
      ReportName: ['', Validators.required],
      SubReportName: ['', Validators.required]
    });

    this.ReportName.valueChanges.subscribe(() => {
      this.updateRequiredValidator(this.SubReportName, true);
    })
  }
  ngAfterViewInit(): void {
    this.getReportsName();
  }

  updateRequiredValidator(control: AbstractControl, isRequired: boolean) {
    if (isRequired) {
      control.setValidators([Validators.required]);
      control.updateValueAndValidity();
      control.markAsTouched();
    } else {
      control.clearValidators()
      control.updateValueAndValidity();
      control.markAsTouched();
    }
  }
  EmptyCtrl() {
    this.SearchRName.setValue('');
    this.SearchSRName.setValue('');
    // this.SearchFilters.clear();
  }

  getSchemeDataByProgramCategory(ID: number) {
    const itemIndex = this.filtersData.findIndex(item => item.First === 'Scheme')
    this.ID = ID;
    if (itemIndex > -1) {
      this.ComSrv.getJSON('api/Reports/RD_SchemeByProgramCategory?ID=' + this.ID).subscribe(
        (d: any) => {
          this.error = '';
          this.filtersData[itemIndex].Second = d;
        },
        (error) => (this.error = `${error.name} , ${error.statusText}`)
      );
    }
  }
  getTspDataByScheme(ID: number) {
    const itemIndex = this.filtersData.findIndex(item => item.First === 'Tsp')
    this.ID = ID;
    if (itemIndex > -1) {
      this.ComSrv.getJSON('api/Reports/RD_TSPByScheme?ID=' + this.ID).subscribe(
        (d: any) => {
          this.error = '';
          this.filtersData[itemIndex].Second = d;
          // console.table(this.filtersData);
        },
        (error) => (this.error = `${error.name} , ${error.statusText}`)
      );
    }
  }
  getReportsName() {
    this.ComSrv.getJSON(`api/Reports/RD_Reports/` + this.roleID).subscribe(
      (data: any) => {
        this.error = '';
        this.subReportsArray = [];
        this.filtersData = [];
        this.filters = [];
        this.SearchFilters = new FormArray([]);
        this.reportsArray = data;
      },
      (error) => {
        this.error = `${error.name} , ${error.statusText}`;
      }
    );
  }
  getSubReportsName(ReportID: any) {
    this.SubReportName.setValue('');
    this.ComSrv.getJSON(`api/Reports/RD_SubReports?ReportID=` + ReportID.ReportID).subscribe(
      (data: any) => {
        this.error = '';
        this.filtersData = [];
        this.filters = [];
        this.SearchFilters = new FormArray([]);
        this.subReportsArray = data;
      },
      (error) => {
        this.error = `${error.name} , ${error.statusText}`;
      }
    );
  }
  getSubReportsFilters(SubReportID: any) {
    if (SubReportID.SubReportID == 53) {
      this.startDate.setValue('')
      this.endDate.setValue('')
    } else {
      this.startDate = new FormControl(moment());
      this.endDate = new FormControl(moment());
    }
    this.filters = [];
    this.filtersData = [];
    this.ComSrv.getJSON(`api/Reports/RD_SubReportsFilters?SubReportID=` + SubReportID.SubReportID).subscribe(
      (data: any) => {
        this.error = '';
        this.SearchFilters = new FormArray([]);
        // console.log(data);
        for (const iterator of data) {
          const url = `api/Reports/GetFilterData?FilterName=${EnumReportsFilters[iterator.FiltersName]}&UserID=${this.currentUser.UserID}`;
          this.ComSrv.getJSON(url).subscribe(
            (d: any) => {
              this.error = '';
              this.filtersData.push(d);
              this.SearchFilters.push(new FormControl(''));
            },
            (error) => {
              this.error = `${error.name} , ${error.statusText}`;
            }
          );
        }
      },
      (error) => {
        this.error = `${error.name} , ${error.statusText}`;
      }
    );
  }
  saveFiltersData(index: any, filters: any, filterName: string) {
    if (filterName === 'Program Category')
      this.getSchemeDataByProgramCategory(filters.ID);
    if (filterName === 'Scheme')
      this.getTspDataByScheme(filters.ID);
    const object: any = { Index: index + 1, ID: filters.ID, FilterName: filterName, Value: filters.Name };
    const itemIndex = this.filters.findIndex(item => item.Index === object.Index);
    if (itemIndex > -1)
      this.filters[itemIndex] = object;
    else
      this.filters.push(object);
  }

  chosenYearHandler(normalizedYear: Moment) {
    const ctrlValue = this.date.value;
    ctrlValue.year(normalizedYear.year());
    this.date.setValue(ctrlValue);
  }
  chosenMonthHandler(normalizedMonth: Moment, datepicker: MatDatepicker<Moment>) {
    const ctrlValue = this.date.value;
    ctrlValue.month(normalizedMonth.month());
    this.date.setValue(ctrlValue);
    datepicker.close();
  }

  chosenYearHandlerForStartDate(normalizedYear: Moment) {
    const ctrlValue = this.startDate.value;
    ctrlValue.year(normalizedYear.year());
    this.startDate.setValue(ctrlValue);
  }
  chosenMonthHandlerForStartDate(normalizedMonth: Moment, datepicker: MatDatepicker<Moment>) {
    const ctrlValue = this.startDate.value;
    ctrlValue.month(normalizedMonth.month());
    this.startDate.setValue(ctrlValue);

    datepicker.close();
  }

  chosenYearHandlerForEndDate(normalizedYear: Moment) {
    const ctrlValue = this.endDate.value;
    ctrlValue.year(normalizedYear.year());
    this.endDate.setValue(ctrlValue);
  }
  chosenMonthHandlerForEndDate(normalizedMonth: Moment, datepicker: MatDatepicker<Moment>) {
    const ctrlValue = this.endDate.value;
    ctrlValue.month(normalizedMonth.month());
    this.endDate.setValue(ctrlValue);
    datepicker.close();
  }

  chosenFilter(Name: string) {
    let itemIndex: number;
    if (Name === 'Scheme') {
      itemIndex = this.filters.findIndex((item: { FilterName: string; }) => item.FilterName === 'Scheme');
      if (itemIndex > -1)
        return `schemeID=${this.filters[itemIndex].ID}`;
      else
        return `schemeID=${0}`;
    }
    else if (Name === 'Gender') {
      itemIndex = this.filters.findIndex((item: { FilterName: string; }) => item.FilterName === 'Gender');
      if (itemIndex > -1)
        return `genderID=${this.filters[itemIndex].ID}`;
      else
        return `genderID=${0}`;
    }
    else if (Name === 'Trade') {
      itemIndex = this.filters.findIndex((item: { FilterName: string; }) => item.FilterName === 'Trade');
      if (itemIndex > -1)
        return `tradeID=${this.filters[itemIndex].ID}`;
      else
        return `tradeID=${0}`;
    }
    else if (Name === 'Sector') {
      itemIndex = this.filters.findIndex((item: { FilterName: string; }) => item.FilterName === 'Sector');
      if (itemIndex > -1)
        return `sectorID=${this.filters[itemIndex].ID}`;
      else
        return `sectorID=${0}`;
    }
    else if (Name === 'Cluster') {
      itemIndex = this.filters.findIndex((item: { FilterName: string; }) => item.FilterName === 'Cluster');
      if (itemIndex > -1)
        return `clusterID=${this.filters[itemIndex].ID}`;
      else
        return `clusterID=${0}`;
    }
    else if (Name === 'Curriculum') {
      itemIndex = this.filters.findIndex((item: { FilterName: string; }) => item.FilterName === 'Curriculum');
      if (itemIndex > -1)
        return `sourceofCurriculumID=${this.filters[itemIndex].ID}`;
      else
        return `sourceofCurriculumID=${0}`;
    }
    else if (Name === 'Duration') {
      itemIndex = this.filters.findIndex((item: { FilterName: string; }) => item.FilterName === 'Duration');
      if (itemIndex > -1)
        return `durationID=${this.filters[itemIndex].ID}`;
      else
        return `durationID=${0}`;
    }
    else if (Name === 'Entry Qualification') {
      itemIndex = this.filters.findIndex((item: { FilterName: string; }) => item.FilterName === 'Entry Qualification');
      if (itemIndex > -1)
        return `educationTypeID=${this.filters[itemIndex].ID}`;
      else
        return `educationTypeID=${0}`;
    }
    else if (Name === 'Examination Body') {
      itemIndex = this.filters.findIndex((item: { FilterName: string; }) => item.FilterName === 'Examination Body');
      if (itemIndex > -1)
        return `certAuthID=${this.filters[itemIndex].ID}`;
      else
        return `certAuthID=${0}`;
    }
    else if (Name === 'Program Type') {
      itemIndex = this.filters.findIndex((item: { FilterName: string; }) => item.FilterName === 'Program Type');
      if (itemIndex > -1)
        return `programTypeID=${this.filters[itemIndex].ID}`;
      else
        return `programTypeID=${0}`;
    }
    else if (Name === 'Financial Year') {
      itemIndex = this.filters.findIndex((item: { FilterName: string; }) => item.FilterName === 'Financial Year');
      if (itemIndex > -1)
        return `financialYear=${this.filters[itemIndex].Value}`;
      else
        return `financialYear=${0}`;
    }
    else if (Name === 'Quarter') {
      itemIndex = this.filters.findIndex((item: { FilterName: string; }) => item.FilterName === 'Quarter');
      if (itemIndex > -1)
        return `Quarter=${this.filters[itemIndex].ID}`;
      else
        return `Quarter=${0}`;
    }
    else if (Name === 'KAM') {
      itemIndex = this.filters.findIndex((item: { FilterName: string; }) => item.FilterName === 'KAM');
      if (itemIndex > -1)
        return `kamID=${this.filters[itemIndex].ID}`;
      else
        return `kamID=${0}`;
    }
    else if (Name === 'Tsp') {
      itemIndex = this.filters.findIndex((item: { FilterName: string; }) => item.FilterName === 'Tsp');
      if (itemIndex > -1)
        return `tspID=${this.filters[itemIndex].ID}`;
      else
        return `tspID=${0}`;
    }
    else if (Name === 'Project Type') {
      itemIndex = this.filters.findIndex((item: { FilterName: string; }) => item.FilterName === 'Project Type');
      if (itemIndex > -1)
        return `projectTypeID=${this.filters[itemIndex].ID}`;
      else
        return `projectTypeID=${0}`;
    }
    else if (Name === 'Class') {
      itemIndex = this.filters.findIndex((item: { FilterName: string; }) => item.FilterName === 'Class');
      if (itemIndex > -1)
        return `classID=${this.filters[itemIndex].ID}`;
      else
        return `classID=${0}`;
    }
    else if (Name === 'Employment Status') {
      itemIndex = this.filters.findIndex((item: { FilterName: string; }) => item.FilterName === 'Employment Status');
      if (itemIndex > -1)
        return `EmploymentStatus=${this.filters[itemIndex].Value}`;
      else
        return `EmploymentStatus=`;
    }
    else if (Name === 'Program Category') {
      itemIndex = this.filters.findIndex((item: { FilterName: string; }) => item.FilterName === 'Program Category');
      if (itemIndex > -1)
        return `programCategoryID=${this.filters[itemIndex].ID}`;
      else
        return `programCategoryID=${0}`;
    }
    else if (Name === 'Financial Year') {
      itemIndex = this.filters.findIndex((item: { FilterName: string; }) => item.FilterName === 'Financial Year');
      if (itemIndex > -1)
        return `financialYear=${this.filters[itemIndex].Value}`;
      else
        return `financialYear=${0}`;
    }
    else if (Name === 'District') {
      itemIndex = this.filters.findIndex((item: { FilterName: string; }) => item.FilterName === 'District');
      if (itemIndex > -1)
        return `districtID=${this.filters[itemIndex].ID}`;
      else
        return `districtID=${0}`;
    }
    else if (Name === 'KAM') {
      itemIndex = this.filters.findIndex((item: { FilterName: string; }) => item.FilterName === 'KAM');
      if (itemIndex > -1)
        return `kamID=${this.filters[itemIndex].ID}`;
      else
        return `kamID=${0}`;
    }
    else if (Name === 'Tsp') {
      itemIndex = this.filters.findIndex((item: { FilterName: string; }) => item.FilterName === 'Tsp');
      if (itemIndex > -1)
        return `tspID=${this.filters[itemIndex].ID}`;
      else
        return `tspID=${0}`;
    }
    else if (Name === 'Project Type') {
      itemIndex = this.filters.findIndex((item: { FilterName: string; }) => item.FilterName === 'Project Type');
      if (itemIndex > -1)
        return `projectTypeID=${this.filters[itemIndex].ID}`;
      else
        return `projectTypeID=${0}`;
    }
    else if (Name === 'Funding Category') {
      debugger;
      itemIndex = this.filters.findIndex((item: { FilterName: string; }) => item.FilterName === 'Funding Category');
      if (itemIndex > -1)
        return `fundingCategoryID=${this.filters[itemIndex].ID}`;
      else
        return `fundingCategoryID=${0}`;
    }
  }

  generateReport(Report: any, subReport: any) {
    if (!this.genForm.valid) return;
    let getFilter = '';
    let itemIndex: number;
    switch (Report.ReportID) {
      case EnumReports['TSP Master Data']:
        getFilter = `reportType=${EnumSubReports[subReport.SubReportName]}`;
        getFilter = getFilter + '&' + this.chosenFilter('Scheme');
        getFilter = getFilter + '&' + this.chosenFilter('Gender');
        getFilter = getFilter + '&' + this.chosenFilter('Program Type');
        getFilter = getFilter + '&' + this.chosenFilter('Program Category') + `&downloadType=${''}`;
        window.open(`${this.ComSrv.appConfig.ReportsAPIURL}Reports/GetTSPMasterReport?${getFilter}`, '_blank');
        break;
      case EnumReports['Trade Data']:
        getFilter = `reportType=${EnumSubReports[subReport.SubReportName]}`;
        getFilter = getFilter + '&' + this.chosenFilter('Trade');
        getFilter = getFilter + '&' + this.chosenFilter('Gender');
        getFilter = getFilter + '&' + this.chosenFilter('Scheme');
        getFilter = getFilter + '&' + this.chosenFilter('Program Type');
        getFilter = getFilter + '&' + this.chosenFilter('Program Category') + `&downloadType=${''}`;
        window.open(`${this.ComSrv.appConfig.ReportsAPIURL}Reports/GetTradeDataReport?${getFilter}`, '_blank');
        break;
      case EnumReports['Procurement - Trainee Status Report']:
        getFilter = `reportType=${EnumSubReports[subReport.SubReportName]}`;
        getFilter = getFilter + '&' + this.chosenFilter('Sector');
        getFilter = getFilter + '&' + this.chosenFilter('Cluster');
        getFilter = getFilter + '&' + this.chosenFilter('Scheme');
        getFilter = getFilter + '&' + this.chosenFilter('Program Type');
        getFilter = getFilter + '&' + this.chosenFilter('Program Category') + `&downloadType=${''}`;
        window.open(`${this.ComSrv.appConfig.ReportsAPIURL}Reports/GetTraineeStatusReport?${getFilter}`, '_blank');
        break;
      case EnumReports['TSP Curriculum Report']:
        getFilter = `reportType=${'TSPCurriculum'}`;
        getFilter = getFilter + '&' + this.chosenFilter('Curriculum');
        getFilter = getFilter + '&' + this.chosenFilter('Duration');
        getFilter = getFilter + '&' + this.chosenFilter('Entry Qualification');
        getFilter = getFilter + '&' + this.chosenFilter('Examination Body');
        getFilter = getFilter + '&' + this.chosenFilter('Scheme');
        getFilter = getFilter + '&' + this.chosenFilter('Program Type');
        getFilter = getFilter + '&' + this.chosenFilter('Program Category') + `&downloadType=${''}`;
        window.open(`${this.ComSrv.appConfig.ReportsAPIURL}Reports/GetTSPCurriculumReport?${getFilter}`, '_blank');
        break;
      case EnumReports['Procurement - Placement Report']:
        getFilter = `reportType=${EnumSubReports[subReport.SubReportName]}`;
        getFilter = getFilter + '&' + this.chosenFilter('Cluster');
        getFilter = getFilter + '&' + this.chosenFilter('District');
        getFilter = getFilter + '&' + this.chosenFilter('Trade');
        getFilter = getFilter + '&' + this.chosenFilter('Sector');
        getFilter = getFilter + '&' + this.chosenFilter('Scheme');
        getFilter = getFilter + '&' + this.chosenFilter('Program Type');
        getFilter = getFilter + '&' + this.chosenFilter('Program Category') + `&downloadType=${''}`;
        window.open(`${this.ComSrv.appConfig.ReportsAPIURL}Reports/GetPlacement?${getFilter}`, '_blank');
        break;
      case EnumReports['Sector Wise Trainee Report']:
        getFilter = this.chosenFilter('Sector')
        getFilter = getFilter + '&' + this.chosenFilter('Scheme');
        getFilter = getFilter + '&' + this.chosenFilter('Program Type');
        getFilter = getFilter + '&' + this.chosenFilter('Program Category') + `&downloadType=${''}`;
        window.open(`${this.ComSrv.appConfig.ReportsAPIURL}Reports/GetSectorTrainee?${getFilter}`, '_blank');
        break;
      case EnumReports['Financial Comparison of Schemes']:
        getFilter = `reportType=${EnumSubReports[subReport.SubReportName]}`;
        const itemIndex1 = this.filters.findIndex((item: { FilterName: string }) => item.FilterName === 'Trade');
        const itemIndex2 = this.filters.findIndex((item: { FilterName: string }) => item.FilterName === 'Sector');
        const itemIndex3 = this.filters.findIndex((item: { FilterName: string }) => item.FilterName === 'Cluster');
        if (itemIndex1 > -1)
          getFilter = getFilter + `&ID=${this.filters[itemIndex1].ID}`;
        else if (itemIndex2 > -1)
          getFilter = getFilter + `&ID=${this.filters[itemIndex2].ID}`;
        else if (itemIndex3 > -1)
          getFilter = getFilter + `&ID=${this.filters[itemIndex3].ID}`;
        else
          getFilter = getFilter + `&ID=${0}`;

        // tslint:disable-next-line: max-line-length
        itemIndex = this.filters.findIndex((item: { FilterName: string; Index: number }) => item.FilterName === 'Scheme' && item.Index === 1);
        if (itemIndex > -1)
          getFilter = getFilter + `&scheme1ID=${this.filters[itemIndex].ID}`;
        else
          getFilter = getFilter + `&scheme1ID=${0}`;

        // tslint:disable-next-line: max-line-length
        itemIndex = this.filters.findIndex((item: { FilterName: string; Index: number }) => item.FilterName === 'Scheme' && item.Index === 2);
        if (itemIndex > -1)
          getFilter = getFilter + `&scheme2ID=${this.filters[itemIndex].ID}`;
        else
          getFilter = getFilter + `&scheme2ID=${0}`;

        // tslint:disable-next-line: max-line-length
        itemIndex = this.filters.findIndex((item: { FilterName: string; Index: number }) => item.FilterName === 'Scheme' && item.Index === 3);
        if (itemIndex > -1)
          getFilter = getFilter + `&scheme3ID=${this.filters[itemIndex].ID}`;
        else
          getFilter = getFilter + `&scheme3ID=${0}`;

        getFilter = getFilter + `&downloadType=${''}`;
        window.open(`${this.ComSrv.appConfig.ReportsAPIURL}Reports/GetFinancialComparisonOfSchemes?${getFilter}`, '_blank');
        break;
      case EnumReports['Total Locations Report']:
        getFilter = this.chosenFilter('Financial Year');
        getFilter = getFilter + '&' + this.chosenFilter('Scheme');
        getFilter = getFilter + '&' + this.chosenFilter('Program Type');
        getFilter = getFilter + '&' + this.chosenFilter('Program Category') + `&downloadType=${''}`;
        window.open(`${this.ComSrv.appConfig.ReportsAPIURL}Reports/GetLocations?${getFilter}`, '_blank');
        break;
      case EnumReports['List of new Trades, TSPs and Industry in a Scheme']:
        getFilter = this.chosenFilter('Scheme');
        getFilter = getFilter + `&StartDate=${moment(this.startDate.value).format('YYYY/MM/DD')}&EndDate=${moment(this.endDate.value).format('YYYY/MM/DD')}`;
        getFilter = getFilter + '&' + this.chosenFilter('Program Type');
        getFilter = getFilter + '&' + this.chosenFilter('Program Category') + `&downloadType=${''}`;
        window.open(`${this.ComSrv.appConfig.ReportsAPIURL}Reports/NewTradesTSPIndustryQuarterly?${getFilter}`, '_blank');
        break;
      case EnumReports['Change of Instructor']:
        getFilter = this.chosenFilter('Scheme');
        getFilter = getFilter + `&tspID=${0}`;
        getFilter = getFilter + '&' + this.chosenFilter('Program Type');
        getFilter = getFilter + '&' + this.chosenFilter('Program Category') + `&downloadType=${''}`;
        window.open(`${this.ComSrv.appConfig.ReportsAPIURL}Reports/GetChangeofInstructorReport?${getFilter}`, '_blank');
        break;
      case EnumReports['Fact Sheet']:
        getFilter = this.chosenFilter('Scheme');
        getFilter = getFilter + `&downloadType=${''}`;
        window.open(`${this.ComSrv.appConfig.ReportsAPIURL}Reports/GetFactSheetReport?${getFilter}`, '_blank');
        break;
      case EnumReports['Change in Appendix Report']:
        getFilter = this.chosenFilter('Scheme');
        getFilter = getFilter + `&downloadType=${''}`;
        window.open(`${this.ComSrv.appConfig.ReportsAPIURL}Reports/GetChangeinAppendixReport?${getFilter}`, '_blank');
        break;
      case EnumReports['Invoice Status Report']:
        getFilter = this.chosenFilter('KAM');
        getFilter = getFilter + '&' + this.chosenFilter('Scheme');
        getFilter = getFilter + '&' + this.chosenFilter('Tsp');
        getFilter = getFilter + `&Month=${moment(this.date.value).format('YYYY/MM/DD')}&downloadType=${''}`;
        window.open(`${this.ComSrv.appConfig.ReportsAPIURL}Reports/GetInvoiceStatusReport?${getFilter}`, '_blank');
        break;
      case EnumReports['Stipend Disbursement Report']:
        getFilter = this.chosenFilter('KAM');
        getFilter = getFilter + '&' + this.chosenFilter('Scheme');
        getFilter = getFilter + '&' + this.chosenFilter('Tsp');
        getFilter = getFilter + `&month=${moment(this.date.value).format('YYYY/MM/DD')}&downloadType=${''}`;
        window.open(`${this.ComSrv.appConfig.ReportsAPIURL}Reports/GetStipendDisbursementReport?${getFilter}`, '_blank');
        break;
      case EnumReports['Trainee Complaints Report']:
        getFilter = `reportType=${EnumSubReports[subReport.SubReportName]}`;
        getFilter = getFilter + '&' + this.chosenFilter('KAM');
        getFilter = getFilter + '&' + this.chosenFilter('Scheme');
        getFilter = getFilter + '&' + this.chosenFilter('Tsp');
        window.open(`${this.ComSrv.appConfig.ReportsAPIURL}Reports/GetTraineeComplaintsReport?${getFilter}`, '_blank');
        break;
      case EnumReports['TPM Summaries - Violations']:
        getFilter = this.chosenFilter('Scheme');
        getFilter = getFilter + '&' + this.chosenFilter('Tsp');
        getFilter = getFilter + `&month=${moment(this.date.value).format('YYYY/MM/DD')}&downloadType=${''}`;
        window.open(`${this.ComSrv.appConfig.ReportsAPIURL}Reports/GetTPMSummariesViolationsReport?${getFilter}`, '_blank');
        break;
      case EnumReports['TPM Summaries - Trainees']:
        getFilter = this.chosenFilter('Scheme');
        getFilter = getFilter + '&' + this.chosenFilter('Tsp');
        getFilter = getFilter + `&month=${moment(this.date.value).format('YYYY/MM/DD')}&downloadType=${''}`;
        window.open(`${this.ComSrv.appConfig.ReportsAPIURL}Reports/GetTPMSummariesTraineesReport?${getFilter}`, '_blank');
        break;
      case EnumReports['Management Report']:
        getFilter = `reportType=${EnumSubReports[subReport.SubReportName]}`;
        getFilter = getFilter + '&' + this.chosenFilter('Scheme');
        getFilter = getFilter + '&' + this.chosenFilter('Tsp');
        getFilter = getFilter + `&Month=${''}&downloadType=${''}`;
        window.open(`${this.ComSrv.appConfig.ReportsAPIURL}Reports/GetManagementReport?${getFilter}`, '_blank');
        break;
      case EnumReports['SDP Monthly Report']:
        getFilter = this.chosenFilter('Project Type');
        getFilter = getFilter + `&month=${moment(this.date.value).format('YYYY/MM/DD')}&downloadType=${''}`;
        window.open(`${this.ComSrv.appConfig.ReportsAPIURL}Reports/GetSDPMonthlyReport?${getFilter}`, '_blank');
        break;
      case EnumReports['BSS - Confirmed Marginal']:
        getFilter = `Month=${moment(this.date.value).format('YYYY/MM/DD')}&tspID=${this.currentUser.UserID}`;
        getFilter = getFilter + '&' + this.chosenFilter('Class');
        getFilter = getFilter + `&downloadType=${''}`;
        window.open(`${this.ComSrv.appConfig.ReportsAPIURL}Reports/GetTSPTraineeStatusReport?${getFilter}`, '_blank');
        break;
      case EnumReports['Unverified Trainees Report']:
        getFilter = `tspID=${this.currentUser.UserID}`;
        getFilter = getFilter + '&' + this.chosenFilter('Class');
        getFilter = getFilter + `&downloadType=${''}`;
        window.open(`${this.ComSrv.appConfig.ReportsAPIURL}Reports/GetUnverifiedTraineeReport?${getFilter}`, '_blank');
        break;
      case EnumReports['Weighted Average Cost per Trainee per Month']:
        itemIndex = this.filters.findIndex((item: { FilterName: string; }) => item.FilterName === 'Program Type');
        getFilter = this.chosenFilter('Program Type');
        getFilter = getFilter + '&' + this.chosenFilter('Scheme');
        getFilter = getFilter + '&' + this.chosenFilter('Class');
        getFilter = getFilter + `&downloadType=${''}`;
        window.open(`${this.ComSrv.appConfig.ReportsAPIURL}Reports/GetTraineeCostReport?${getFilter}`, '_blank');
        break;
      case EnumReports['Number of Unique TSPs engaged with PSDF']:
        getFilter = this.chosenFilter('Scheme');
        getFilter = getFilter + `&downloadType=${''}`;
        window.open(`${this.ComSrv.appConfig.ReportsAPIURL}Reports/GetSchemeUniqueTSPReport?${getFilter}`, '_blank');
        break;
      case EnumReports['PD - Trainee Status Report']:
        getFilter = this.chosenFilter('Program Type');
        getFilter = getFilter + '&' + this.chosenFilter('Trade');
        getFilter = getFilter + '&' + this.chosenFilter('Scheme');
        getFilter = getFilter + '&' + this.chosenFilter('Tsp');
        // tslint:disable-next-line: max-line-length
        getFilter = getFilter + `&quarter=${0}&year=${0}&Month=${moment(this.date.value).format('YYYY/MM/DD')}&downloadType=${''}`;
        window.open(`${this.ComSrv.appConfig.ReportsAPIURL}Reports/GetPDTraineeStatusReport?${getFilter}`, '_blank');
        break;
      case EnumReports['Income of Graduate']:
        getFilter = this.chosenFilter('Scheme')
        getFilter = getFilter + '&' + this.chosenFilter('Tsp')
        getFilter = getFilter + '&' + this.chosenFilter('Employment Status')
        getFilter = getFilter + `&TraineeID=${0}&downloadType=${''}`;
        window.open(`${this.ComSrv.appConfig.ReportsAPIURL}Reports/GetIncomeOfGraduateReport?${getFilter}`, '_blank');
        break;
      case EnumReports['Trainee Type Percentage']:
        getFilter = this.chosenFilter('Program Type')
        getFilter = getFilter + '&' + this.chosenFilter('Cluster');
        getFilter = getFilter + '&' + this.chosenFilter('Sector');
        getFilter = getFilter + `&downloadType=${''}`;
        window.open(`${this.ComSrv.appConfig.ReportsAPIURL}Reports/GetTraineeTypePercentageReport?${getFilter}`, '_blank');
        break;
      case EnumReports['Top 10 Trades']:
        getFilter = `Type=${EnumSubReports[subReport.SubReportName]}`;
        getFilter = getFilter + '&' + this.chosenFilter('Scheme');
        getFilter = getFilter + '&' + this.chosenFilter('Sector');
        getFilter = getFilter + `&downloadType=${''}`;
        window.open(`${this.ComSrv.appConfig.ReportsAPIURL}Reports/GetPDTop10TradesReport?${getFilter}`, '_blank');
        break;
      case EnumReports['PD - Placement Report']:
        getFilter = `Type=${EnumSubReports[subReport.SubReportName]}`;
        getFilter = getFilter + '&' + this.chosenFilter('Scheme');
        getFilter = getFilter + '&' + this.chosenFilter('Sector');
        getFilter = getFilter + '&' + this.chosenFilter('Trade');
        window.open(`${this.ComSrv.appConfig.ReportsAPIURL}Reports/GetPDPlacmentReport?${getFilter}`, '_blank');
        break;
      case EnumReports['BDP - Trainee Status Report']:
        getFilter = this.chosenFilter('Program Type');
        getFilter = getFilter + '&' + this.chosenFilter('Trade');
        getFilter = getFilter + '&' + this.chosenFilter('Scheme');
        getFilter = getFilter + '&' + this.chosenFilter('Tsp');
        // tslint:disable-next-line: max-line-length
        getFilter = getFilter + `&quarter=${0}&year=${0}&Month=${moment(this.date.value).format('YYYY/MM/DD')}&downloadType=${''}`;
        window.open(`${this.ComSrv.appConfig.ReportsAPIURL}Reports/GetPDTraineeStatusReport?${getFilter}`, '_blank');
        break;
      case EnumReports['Players under Training for placement']:
        getFilter = this.chosenFilter('Scheme');
        getFilter = getFilter + `&downloadType=${''}`;
        window.open(`${this.ComSrv.appConfig.ReportsAPIURL}Reports/GetPDPPlayersUnderTrainingReport?${getFilter}`, '_blank');
        break;
      case EnumReports['Cost Saving Report']:
        getFilter = `StartDate=${moment(this.startDate.value).format('YYYY/MM/DD')}&EndDate=${moment(this.endDate.value).format('YYYY/MM/DD')}&downloadType=${''}`;
        window.open(`${this.ComSrv.appConfig.ReportsAPIURL}Reports/GetPDPCostSavingReport?${getFilter}`, '_blank');
        break;
      case EnumReports['Number of Cost Sharing Partners']:
        getFilter = this.chosenFilter('Scheme');
        getFilter = getFilter + `&downloadType=${''}`;
        window.open(`${this.ComSrv.appConfig.ReportsAPIURL}Reports/GetCostSharingPartnersReport?${getFilter}`, '_blank');
        break;
      case EnumReports['Business Partner Invoice Status Report']:
        getFilter = `Month=${moment(this.date.value).format('YYYY/MM/DD')}&downloadType=${''}`;
        window.open(`${this.ComSrv.appConfig.ReportsAPIURL}Reports/GetBusinessPartnerInvoiceStatus?${getFilter}`, '_blank');
        break;
      case EnumReports['Tsp Invoice Status Report']:
        getFilter = this.chosenFilter('Scheme');
        getFilter = getFilter + '&' + this.chosenFilter('Tsp');
        getFilter = getFilter + `&downloadType=${''}`;
        window.open(`${this.ComSrv.appConfig.ReportsAPIURL}Reports/GeTspInvoiceStatus?${getFilter}`, '_blank');
        break;

      // excel  report start
      case EnumReports['Trainees Attendance Report']:
        var Month = moment(this.date.value).format('YYYY-MM');

        this.SPName = "RPT_TraineesAttendance"
        this.ExportReportName = "Trainees Attendance"

        this.paramObject = {
          Month: Month
        }

        this.FetchReportData(this.SPName, this.ExportReportName, this.paramObject)
        break;

      case EnumReports['Trainer change Logs']:
        var StartMonth = moment(this.startDate.value).format('YYYY-MM');
        var EndMonth = moment(this.endDate.value).format('YYYY-MM');

        this.SPName = "RPT_TrainerChangeLogs"
        this.ExportReportName = 'Trainer change Logs'

        this.paramObject = {
          StartMonth: StartMonth,
          EndMonth: EndMonth,
        }

        this.FetchReportData(this.SPName, this.ExportReportName, this.paramObject)
        break;

      case EnumReports['TSP Change Request Report']:
        var StartMonth = moment(this.startDate.value).format('YYYY-MM');
        var EndMonth = moment(this.endDate.value).format('YYYY-MM');

        this.SPName = "RPT_TSPChangeRequest"
        this.ExportReportName = 'TSP Change Request Report'

        this.paramObject = {
          StartMonth: StartMonth,
          EndMonth: EndMonth,
        }

        this.FetchReportData(this.SPName, this.ExportReportName, this.paramObject)
        break;

      case EnumReports['Bulk Trainees Status Report']:
        var FundingCategory = this.chosenFilter('Funding Category').split("=")[1];
        FundingCategory = FundingCategory == 'undefined' ? "0" : FundingCategory

        var StartMonth = moment(this.startDate.value).format('YYYY-MM');
        var EndMonth = moment(this.endDate.value).format('YYYY-MM');

        this.SPName = "RPT_TSRData"
        this.ExportReportName = 'TSR'

        this.paramObject = {
          FundingCategory: FundingCategory,
          StartMonth: StartMonth != 'Invalid date' ? StartMonth : '',
          EndMonth: StartMonth != 'Invalid date' ? EndMonth : '',
        }

        this.FetchReportData(this.SPName, this.ExportReportName, this.paramObject)
        break;


      case EnumReports['Class-wise Payment Report']:


        this.SPName = "RPT_ClasswisePayment"
        this.ExportReportName = 'Class-wise Payment Report'

        const schemeFilter = this.chosenFilter('Scheme') || '';
        const tspFilter = this.chosenFilter('Tsp') || '';

        this.paramObject = {
          Scheme: schemeFilter.includes('=') ? schemeFilter.split('=')[1] : '0',
          Tsp: tspFilter.includes('=') ? tspFilter.split('=')[1] : '0'
        };

        this.FetchReportData(this.SPName, this.ExportReportName, this.paramObject)
        break;

      case EnumReports['TSP Details Report']:
        var FundingSource = this.chosenFilter('Project Type').split("=")[1];
        FundingSource = FundingSource == 'undefined' ? "0" : FundingSource

        this.SPName = "RPT_TSPDetail"
        this.ExportReportName = 'TSP Details Report'

        this.paramObject = {
          FundingSource: FundingSource
        }

        this.FetchReportData(this.SPName, this.ExportReportName, this.paramObject)

        break;
      //======================Azhar iqbal==============================

      // excel  report start
      case EnumReports['AMS Missing Classes Data Report']:
        var Month = moment(this.date.value).format('YYYY-MM');

        this.SPName = "AMSMissingClassesReport"
        this.ExportReportName = "AMS Missing Classes Data Report"

        this.paramObject = {
          Month: Month
        }

        this.FetchReportData(this.SPName, this.ExportReportName, this.paramObject)
        break;
      default:
        // let hostURL = `https://localhost:44303/ProcurementReports/Index?`;
        // let url = hostURL + getFilter;
        // window.open(url, "_blank");
        break;
    }
  }

  paramObject: any = {}
  ExportReportName: string = ""
  SPName: string = ""

  // Start written by sami which is used to create excel and paramstring by js object

  GetParamString(SPName: string, paramObject: any) {
    let ParamString = SPName;
    for (const key in paramObject) {
      if (Object.hasOwnProperty.call(paramObject, key)) {
        ParamString += `/${key}=${paramObject[key]}`;
      }
    }

    return ParamString;
  }

  async FetchReportData(SPName: string, ReportName: string, paramObject: any) {
    try {
      const Param = this.GetParamString(SPName, paramObject);
      const data: any = await this.ComSrv.getJSON(`api/BSSReports/FetchReportData?Param=${Param}`).toPromise();
      if (data.length > 0) {
        this.ExportToExcel(data, ReportName);
      } else {
        this.ComSrv.ShowWarning(' No Record Found', 'Close');
      }
    } catch (error) {
      this.error = error;
    }
  }
  ExportToExcel(data: any, FileName: any) {
    debugger;
    // this.ComSrv.ExcelExportWithForm({data}, FileName);
    const DateTime = new Date().toLocaleString(undefined, {
      year: 'numeric',
      month: '2-digit',
      day: '2-digit',
      hour: '2-digit',
      minute: '2-digit',
      second: '2-digit',
      hour12: true
    });

    const ws: XLSX.WorkSheet = XLSX.utils.json_to_sheet(data);
    const wb: XLSX.WorkBook = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, FileName);
    const fileName = `${FileName}_${DateTime}.xlsx`;
    XLSX.writeFile(wb, fileName);
  }
  // end written by sami which is used to create excel and paramstring by js object

  get ReportName() { return this.genForm.get('ReportName') }
  get SubReportName() { return this.genForm.get('SubReportName') }
}
