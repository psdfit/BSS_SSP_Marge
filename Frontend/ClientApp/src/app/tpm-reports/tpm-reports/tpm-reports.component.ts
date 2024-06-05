import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormGroupDirective, Validators } from '@angular/forms';
import { CommonSrvService } from '../../common-srv.service';
import { UserRightsModel } from '../../master-data/users/users.component';
import { ModelBase } from '../../shared/ModelBase';
import { environment } from '../../../environments/environment';
import { FormControl } from '@angular/forms';
import { MomentDateAdapter, MAT_MOMENT_DATE_ADAPTER_OPTIONS } from '@angular/material-moment-adapter';
import { DateAdapter, MAT_DATE_FORMATS, MAT_DATE_LOCALE } from '@angular/material/core';
import { MatDatepicker } from '@angular/material/datepicker';
import * as _moment from 'moment';
//import { default as _rollupMoment, Moment } from 'moment';
import { Moment } from 'moment';
import { EnumTPMReports } from '../../shared/Enumerations';

//const moment = _rollupMoment || _moment;
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
    selector: 'app-tpm-reports',
    templateUrl: './tpm-reports.component.html',
    styleUrls: ['./tpm-reports.component.scss'],
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
export class TPMReportsComponent implements OnInit {
    Schemes: any;
    TSPs: any;
    FilteredTSPs: any;
    ReportTypes = [];
    ControllerFunction;

    TPMForm: FormGroup

    date = new FormControl(moment());

    constructor(private fb: FormBuilder, private http: CommonSrvService) { }

    ngOnInit(): void {
        this.GetData();

        this.ReportTypes = Object.values(EnumTPMReports);

        this.TPMForm = this.fb.group({
            ReportType: ['', Validators.required],
            Month: new FormControl(moment()),
            SchemeUID: [, Validators.required],
            TSPUID: [],
        });
    }

    GetData() {
        this.http.getJSON('api/TPMReports').subscribe(d => {
            this.Schemes = d[0];
            this.TSPs = d[1];
            this.FilteredTSPs = d[1];
        });
    }

    filterTSPs(event) {

        const schemeid = this.Schemes.filter(p => p.UID === event.value)[0].SchemeID
        this.FilteredTSPs = this.TSPs.filter(x => x.SchemeID === schemeid);
    }

    Submit(nform: FormGroupDirective) {
        if (this.TPMForm.invalid)
            return;

        this.http.postJSON('api/TPMReports/', this.TPMForm.value).subscribe((d: any) => {

        });
    }

    HideShowFields(reportType) {
        if (reportType.value === EnumTPMReports.SchemeViolationReport ||
            reportType.value === EnumTPMReports.ConfirmedMarginal ||
            reportType.value === EnumTPMReports.AdditionalTrainees ||
            reportType.value === EnumTPMReports.DeletedOrDropoutTrainees ||
            reportType.value === EnumTPMReports.AttendanceAndPerception ||
            reportType.value === EnumTPMReports.InstructorDetails ||
            reportType.value === EnumTPMReports.OnJobTraining ||
            reportType.value === EnumTPMReports.EmploymentVerification) {

            this.DisableTSPField();
        }
        else if (reportType.value === EnumTPMReports.CenterInspection ||
            reportType.value === EnumTPMReports.ClassFormIII ||
            reportType.value === EnumTPMReports.TSPSummaryReportFormIV ||
            reportType.value === EnumTPMReports.ProfileVerificationPV) {

            this.EnableBothFields();
        }
        else if (reportType.value === EnumTPMReports.ProfileVerificationSummary) {

            this.DisableTSPField();
            this.DisableSchemeField();
        }
    }

    chosenYearHandler(normalizedYear: Moment) {
        const ctrlValue = this.Month.value;
        ctrlValue.year(normalizedYear.year());
        this.Month.setValue(ctrlValue);
    }

    chosenMonthHandler(normalizedMonth: Moment, datepicker: MatDatepicker<Moment>) {
        const ctrlValue = this.Month.value;
        ctrlValue.month(normalizedMonth.month());
        this.Month.setValue(ctrlValue);
        datepicker.close();
    }

    clearMonth() {
        this.Month.setValue(null);
    }

    DisableTSPField() {
        if (this.TPMForm.controls['TSPUID'].enabled)
            this.TPMForm.controls['TSPUID'].disable();
    }

    DisableSchemeField() {
        if (this.TPMForm.controls['SchemeUID'].enabled)
            this.TPMForm.controls['SchemeUID'].disable();
    }

    EnableBothFields() {
        if (this.TPMForm.controls['SchemeUID'].disabled)
            this.TPMForm.controls['SchemeUID'].enable();

        if (this.TPMForm.controls['TSPUID'].disabled)
            this.TPMForm.controls['TSPUID'].enable();
    }

    get Month() { return this.TPMForm.get('Month') }

}
