import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { merge } from 'rxjs';
import { startWith, switchMap } from 'rxjs/operators';
import { CommonSrvService } from '../../common-srv.service';
import { MomentDateAdapter, MAT_MOMENT_DATE_ADAPTER_OPTIONS } from '@angular/material-moment-adapter';
import { DateAdapter, MAT_DATE_FORMATS, MAT_DATE_LOCALE } from '@angular/material/core';
import { MatDatepicker } from '@angular/material/datepicker';
import { FormControl } from '@angular/forms';
// Depending on whether rollup is used, moment needs to be imported differently.
// Since Moment.js doesn't have a default export, we normally need to import using the `* as`
// syntax. However, rollup creates a synthetic default module and we thus need to import it using
// the `default as` syntax.
import * as _moment from 'moment';
// tslint:disable-next-line:no-duplicate-imports
import { Moment } from 'moment';
import * as XLSX from 'xlsx';
import { DialogueService } from '../../shared/dialogue.service';
import { SearchFilter, ExportExcel, SearchFilterMultiSelect } from '../../shared/Interfaces';
import { EnumExcelReportType, EnumUserLevel, EnumUserRoles } from '../../shared/Enumerations';
import { UsersModel } from '../../master-data/users/users.component';
import { environment } from '../../../environments/environment';

const moment = _moment;

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
    selector: 'app-mpr',
    templateUrl: './mpr.component.html',
    styleUrls: ['./mpr.component.scss'],
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
export class MPRComponent implements OnInit {
    environment = environment;

    @ViewChild(MatPaginator) paginator: MatPaginator;
    @ViewChild(MatSort) sort: MatSort;
    // resultsLength: any;
    displayedColumns = ['Month', 'ReportDate', 'TSPName', 'TradeName', 'ClassCode', 'Batch'];
    displayedMPRDetailColumns = ['Trainee Code', 'TrianeeName', 'Class Code', 'TSP Name', 'Trainee CNIC', 'Is Extra',
        'CNIC Verified', 'Stipend Recommended', 'Trainee Status', 'Comments',

    ];
    mprDetails: any[];
    mpr: any[];
    MPRIDsArray: any[];
    MPRIDs: string;
    monthlympr: any[];
    errorHTTP: any;
    month = new FormControl(moment());
    // Modified: Updated to use SearchFilterMultiSelect with array-based filters
    filters: SearchFilterMultiSelect = {
        Schemes: [],
        TSPs: [],
        Classes: [],
    };
    schemeArray: any[];
    classesArray: any[];
    tspDetailArray: any[];
    currentUser: UsersModel;
    enumUserLevel = EnumUserLevel;
    enumRoleLevel = EnumUserRoles;
    mprDetailsArray: any[]; MPRDetailsBulkArray: any[];

    constructor(private ComSrv: CommonSrvService, public dialogueService: DialogueService) {

    }
    SearchSch = new FormControl('');
    SearchCls = new FormControl('');
    SearchTSP = new FormControl('');

    kamFilter = new FormControl(0);
    SearchKam = new FormControl('');
    Kam = [];

    fundingCategoryFilter = new FormControl();
    SearchFundingCategory = new FormControl('');
    Project: any[] = [];

    error = ''



    EmptyCtrl(Ev: any) {
        this.SearchCls.setValue('');
        this.SearchTSP.setValue('');
        this.SearchSch.setValue('');
        this.SearchFundingCategory.setValue('');
        this.SearchKam.setValue('');

    }
    chosenYearHandler(normalizedYear: Moment) {
        this.month = new FormControl(moment());
        const ctrlValue = this.month.value;
        ctrlValue.year(normalizedYear.year());
        this.month.setValue(ctrlValue);
    }

    chosenMonthHandler(normalizedMonth: Moment, datepicker: MatDatepicker<Moment>) {
        const ctrlValue = this.month.value;
        ctrlValue.month(normalizedMonth.month());
        this.month.setValue(ctrlValue);
        this.GetMPR();
        datepicker.close();
    }
    clearMonth() {
        this.month = new FormControl(moment(null));
        // this.month.setValue(null);
        this.GetMPR();
    }
    ngOnInit(): void {
        this.ComSrv.setTitle('Monthly Progress Report');
        this.currentUser = this.ComSrv.getUserDetails();
        this.GetMPR();
        this.monthlympr = []
        this.getKam();
        this.getFundingCategories();
    }

    GetMPR() {
        // Modified: Convert array filters to comma-separated strings
        const modFilters = {
            Month: this.month.value,
            Schemes: Array.isArray(this.filters.Schemes) ? (this.filters.Schemes.length > 0 ? this.filters.Schemes.join(',') : '0') : '0',
            TSPs: Array.isArray(this.filters.TSPs) ? (this.filters.TSPs.length > 0 ? this.filters.TSPs.join(',') : '0') : '0',
            Classes: Array.isArray(this.filters.Classes) ? (this.filters.Classes.length > 0 ? this.filters.Classes.join(',') : '0') : '0',
            KAMID: this.filters.KamID,
            FundingCategoryID: this.filters.FundingCategoryID
        };
        this.ComSrv.postJSON(`api/MPR/RD_MPRBy`, modFilters).subscribe(
            (data: any) => {
                this.mpr = data.MPR;
                this.schemeArray = data.Schemes;
                this.MPRIDsArray = this.mpr.map(o => o.MPRID);
                this.MPRIDs = this.MPRIDsArray.join(',');
            }
        );
    }

    // added KAM api endpoint:
    getKam() {
        this.ComSrv.getJSON('api/KAMAssignment/RD_KAMAssignmentForFilters').subscribe(
            (d: any) => {
                this.error = '';
                this.Kam = d;
            },
            error => {
                this.error = error;
            }
        );
    }
    // added Project api endpoint:
    getFundingCategories() {
        this.ComSrv.getJSON(`api/Scheme/GetScheme?OID=${this.ComSrv.OID.value}`).subscribe((d: any) => {
            this.Project = d[4];
            console.log(this.Project, 'projecttt')
        },
            (error) => {
                this.error = error.error;
                this.ComSrv.ShowError(error.error + '\n' + error.message);
            } // error path
        );
    }


    GetMPRDetails(r: any) {
        if (r.mprDetails) {
            r.mprDetails = null;
            this.monthlympr = this.monthlympr.filter(mpr => mpr.MPRID != r.MPRID);
            return;
        }
        this.ComSrv.getJSON('api/MPR/MPRTraineeDetail/' + r.MPRID).subscribe((data: any) => {
            r.mprDetails = data;
            // this.mprDetails = data;
            // this.mprDetails.push(data);
            this.monthlympr.push(data);
            console.log(this.monthlympr);
            this.monthlympr = this.monthlympr.reduce((accumulator, value) => accumulator.concat(value), []);

            // console.log(this.monthlympr);
        });
    }

    GetByMPRIDs() {
        this.ComSrv.postJSON('api/MPR/GetMPRExcelExportByIDs', this.MPRIDs).subscribe((d: any) => {
            this.MPRDetailsBulkArray = d;
            // console.log(this.MPRDetailsBulkArray);

            if (d.length === 0) {
                this.ComSrv.ShowError('No MPR Detail Record Found');
                return false;
            }
            this.ExportToExcelBulkMPRDetail();
        });

    }

    ExportToExcelBulkMPRDetail() {

        // if (this.prnDetailsArray.length == 0) {
        //    this.http.ShowError("Please check PRN Details to export data")
        //    return;
        // }
        const fileName = 'MPR Details'
        const filteredData = this.MPRDetailsBulkArray;


        // const result = filteredData.reduce((accumulator, value) => accumulator.concat(value), []);
        // console.log(result);

        const exportExcel: ExportExcel = {
            Title: 'MPR Details Report',
            Author: this.currentUser.FullName,
            Type: EnumExcelReportType.MPR,
            Month: this.month.value,
            Data: {},
            // List1: data
            List1: this.populateData(filteredData)
        };
        this.dialogueService.openExportConfirmDialogue(exportExcel).subscribe();

    }



    getTSPDetailByScheme() {
        // Modified: Handle array of scheme IDs
        this.filters.TSPs = [];
        this.filters.Classes = [];
        const schemeIdParam = this.filters.Schemes.length > 0 ? this.filters.Schemes.join(',') : '0';
        this.ComSrv.getJSON(`api/Dashboard/FetchTSPsByMultipleSchemes?SchemeID=${schemeIdParam}`)
            .subscribe(data => {
                this.tspDetailArray = (data as any[]);
            }, error => {
                this.error = error;
            });
    }

    getClassesByTsp() {
        // Modified: Handle array of TSP IDs
        this.filters.Classes = [];
        const tspIdParam = this.filters.TSPs.length > 0 ? this.filters.TSPs.join(',') : '0';
        this.ComSrv.getJSON(`api/Dashboard/FetchMultipleClassesByTSP?TspID=${tspIdParam}`)
            .subscribe(data => {
                this.classesArray = (data as any[]);
            }, error => {
                this.error = error;
            });
    }

    // exportToExcel() {

    //    if (this.monthlympr.length == 0) {
    //        this.ComSrv.ShowError("Please check MPR Details to export data")
    //        return;
    //    }
    //    let fileName = 'MPR'
    //    let filteredData = [...this.monthlympr]


    //    const result = filteredData.reduce((accumulator, value) => accumulator.concat(value), []);

    //    let exportExcel: ExportExcel = {
    //        Title: 'Monthly Progress Report',
    //        Author: this.currentUser.FullName,
    //        Type: EnumExcelReportType.MPR,
    //        Data: {},
    //        ////List1: data
    //        List1: this.populateData(result)
    //    };
    //    this.dialogueService.openExportConfirmDialogue(exportExcel).subscribe();
    // }


    populateData(data: any) {
        return data.map(item => {
            return {
                'Scheme Name': item.SchemeName
                , 'MPR Month': item.mprMonth
                , 'Trainee Code': item.TraineeCode
                , 'Trainee Name': item.TraineeName
                , 'Class Code': item.ClassCode
                , 'TSP Name': item.TSPName
                , 'Trainee CNIC': item.TraineeCNIC
                , 'Is Extra': item.ExtraStatus
                , 'Is Marginal': item.IsMarginal
                , 'CNIC Verified': item.CNICVerificationStatus
                , 'Stipend Recommended': item.StipendAmount
                , 'Trainee Status': item.TraineeStatusName
                , Comments: item.Reason
                // , "Trade Code": item.TradeCode
            }
        })
    }

    getDependantFilters() {
        if (this.currentUser.UserLevel === this.enumUserLevel.TSP) {
            this.getClassesBySchemeFilter();
        }
        else {
            this.getTSPDetailByScheme();
        }
    }
    openTraineeJourneyDialogue(data: any): void {
        // debugger;
        this.dialogueService.openTraineeJourneyDialogue(data);
    }
    openClassJourneyDialogue(data: any): void {
        // debugger;
        this.dialogueService.openClassJourneyDialogue(data);
    }
    getClassesBySchemeFilter() {
        // Modified: Handle array of scheme IDs
        this.filters.Classes = [];
        this.filters.TraineeID = 0;
        const schemeIdParam = this.filters.Schemes.length > 0 ? this.filters.Schemes.join(',') : '0';
        this.ComSrv.getJSON(`api/Dashboard/FetchClassesByMultipleSchemeUser?SchemeID=${schemeIdParam}&UserID=${this.currentUser.UserID}`)
            .subscribe(data => {
                this.classesArray = (data as any[]);
            }, error => {
                this.error = error;
            });
    }
}
