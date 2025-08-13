import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { merge } from 'rxjs';
import { startWith, switchMap } from 'rxjs/operators';
import { CommonSrvService } from '../../common-srv.service';
import { SrnEditDialogComponent } from './srn-edit-dialog/srn-edit-dialog.component';
import { UserRightsModel, UsersModel } from '../../master-data/users/users.component';
import { MatDialog } from '@angular/material/dialog';
import { Overlay } from '@angular/cdk/overlay';
import { DialogueService } from '../../shared/dialogue.service';
import { EnumApprovalProcess } from '../../shared/Enumerations';
import { Moment } from 'moment';
import { MatDatepicker } from '@angular/material/datepicker';
import { FormControl } from '@angular/forms';
import { SearchFilter, ExportExcel } from '../../shared/Interfaces';
import { EnumExcelReportType, EnumUserLevel } from '../../shared/Enumerations';
import { DatePipe } from '@angular/common';

import * as _moment from 'moment';
import { DateAdapter, MAT_DATE_FORMATS, MAT_DATE_LOCALE } from '@angular/material/core';
import { MomentDateAdapter, MAT_MOMENT_DATE_ADAPTER_OPTIONS } from '@angular/material-moment-adapter';
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
    selector: 'app-srn',
    templateUrl: './srn.component.html',
    styleUrls: ['./srn.component.scss'],
    providers: [
        // `MomentDateAdapter` can be automatically provided by importing `MomentDateModule` in your
        // application's root module. We provide it at the component level here, due to limitations of
        // our example generation script.
        DatePipe,
        {
            provide: DateAdapter,
            useClass: MomentDateAdapter,
            deps: [MAT_DATE_LOCALE, MAT_MOMENT_DATE_ADAPTER_OPTIONS]
        },

        { provide: MAT_DATE_FORMATS, useValue: MY_FORMATS },
    ],
})
export class SrnComponent implements OnInit {
    environment = environment;

    @ViewChild(MatPaginator) paginator: MatPaginator;
    @ViewChild(MatSort) sort: MatSort;
    // resultsLength: any;
    // dtSRNDataDisplayedColumns = ['TSPName', 'TradeName', 'ClassCode', 'TraineeCode', 'TraineeName', 'FatherName', 'TraineeCNIC', 'DistrictName', 'ContactNumber1', 'TrainingAddressLocation', 'Comments', 'Batch', 'TokenNumber', 'TransactionNumber', 'Status', 'Month', 'NumberOfMonths', 'Action'];
    // dtSRNData: MatTableDataSource<any>;
    srn: any[];
    srnDetails: any[];
    srnDetailsFiltered: any[];
    errorHTTP: any;
    month = new FormControl(moment());
    SearchSch = new FormControl('');
    SearchKAM = new FormControl('');
    SearchTSP = new FormControl('');
    //filters: ISRNReportFilter = { SchemeID: 0, TSPID: 0, KAMID: 0 };
    filters: ISRNReportFilterMultiSelect = {
        Schemes: [],
        TSPMasterIDs: [],
        KAMID: 0,
        FundingCategoryID: 0,
        ClassStatusID: 0
    };
    kamusers: []; schemes: []; tsps: []; tspMasters: []; srnDetailsArray: any[];
    currentUser: UsersModel;
    enumUserLevel = EnumUserLevel;
    error = '';
    ClassStatus = [];
    SearchClassStatus = new FormControl('');
    ClassStatusFilter = new FormControl(0);
    fundingCategoryFilter = new FormControl();
    SearchFundingCategory = new FormControl('');
    Project: any[] = [];
    startDate = new FormControl();
    endDate = new FormControl();

    constructor(private http: CommonSrvService, public dialog: MatDialog, private dialogue: DialogueService, private _date: DatePipe) { }


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

    ngOnInit(): void {
        this.http.setTitle('Stipend Recommendation Note');
        this.currentUser = this.http.getUserDetails();
        this.srnDetailsArray = [];
        this.http.OID.subscribe(OID => {
            this.GetSRN();
            this.GetFiltersData();
        })
        this.getFundingCategories();
        this.GetClassStatus();

    }

    EmptyCtrl() {
        this.SearchKAM.setValue('');
        this.SearchTSP.setValue('');
        this.SearchSch.setValue('');
        this.SearchClassStatus.setValue('')
        this.SearchFundingCategory.setValue('')
    }

    GetFiltersData() {
        this.http.getJSON(`api/PRNMaster/GetFiltersData`).subscribe(
            (response: any) => {
                this.kamusers = response[0];
                this.schemes = response[1];
                //this.tsps = response[2];
                this.tspMasters = response[2];
                // r.PRN = data;
                // r.HasPRN = true;
            }
            , (error) => {
                console.error(JSON.stringify(error));
            }
        );
    }

    GetSRN() {
        // Modified: Convert array filters to comma-separated strings
        const modFilters = {
            Month: this.month.value,
            OID: this.http.OID.value,
            KAMID: this.filters.KAMID,
            Schemes: Array.isArray(this.filters.Schemes) ? this.filters.Schemes.join(',') : '0',
            TSPMasters: Array.isArray(this.filters.TSPMasterIDs) ? this.filters.TSPMasterIDs.join(',') : '0',
            ClassStatusID: this.filters.ClassStatusID,
            FundingCategoryID: this.filters.FundingCategoryID
        };
        this.http.postJSON(`api/SRN/GetSRN`, modFilters).subscribe(
            (data: any) => {
                this.srn = data;
            }
        );
    }


    GetSrnDetails(r: any) {
        if (r.srnDetails) {
            r.srnDetails = null;
            this.srnDetailsArray = this.srnDetailsArray.filter(s => s.SRNID != r.SRNID);
            return;
        }
        this.http.getJSON('api/SRN/GetSRNDetails/' + r.SRNID).subscribe((data: any) => {
            r.srnDetails = data[0];
            this.srnDetailsArray.push(data[0]);
            this.srnDetailsArray = this.srnDetailsArray.reduce((accumulator, value) => accumulator.concat(value), []);

        });
    }

    GetSrnDetailsFitered(r: any) {
        if (r.srnDetails) {
            r.srnDetails = null;
            this.srnDetailsArray = this.srnDetailsArray.filter(s => s.SRNID != r.SRNID);

            return;
        }
        this.http.getJSON('api/SRN/GetSRNDetailsFiltered/' + r.SRNID).subscribe((data: any) => {
            r.srnDetails = data[0];
            this.srnDetailsArray.push(data[0]);
            this.srnDetailsArray = this.srnDetailsArray.reduce((accumulator, value) => accumulator.concat(value), []);

        });
    }
    /// ---Invoke Dialog---S--////
    openDialog_SRNEdit(row: any): void {
        const dialogRef = this.dialog.open(SrnEditDialogComponent, {
            width: '600px',
            minHeight: '400px',
            data: { ...row }
        })
        dialogRef.afterClosed().subscribe(result => {
            console.log(result);
            this.UpdateSRNDetails(result);
        })
    }
    openApprovalDialogue(row: any): void {
        // { ProcessKey: 'AP', FormID:  row.SrnId }
        // let datas: IApprovalHistory = { ProcessKey: 'AP', FormID: 292 };
        this.dialogue.openApprovalDialogue(EnumApprovalProcess.SRN, row.SRNID).subscribe(result => { console.log(result); });
    }
    /// ---Invoke  Dialog---E--////

    UpdateSRNDetails(srnDetails: any): void {
        if (srnDetails) {
            this.http.postJSON('api/SRN/UpdateSRNDetails/', srnDetails).subscribe(data => {
                const updatedObj = data as any;
                this.srn.forEach(x => {
                    x.srnDetails.forEach(y => {
                        if (y.ReportId === updatedObj.ReportId) {
                            y.TokenNumber = updatedObj.TokenNumber,
                                y.TransactionNumber = updatedObj.TransactionNumber,
                                y.IsPaid = updatedObj.IsPaid
                        }
                    })
                });
                // this.srn = this.srn.map(x => x.SRNID != updatedObj.SRNID ? x
                //  : x.srnDetails.map(y => y.ReportId != updatedObj.ReportId ? y
                //    : {
                //      ...y
                //      , TokenNumber: updatedObj.TokenNumber
                //      , TransactionNumber: updatedObj.TransactionNumber
                //      , IsPaid: updatedObj.IsPaid
                //    }));


                // this.dtSRNData = new MatTableDataSource<any>(this.srn);
                // this.dtSRNData.paginator = this.paginator;
                // this.dtSRNData.sort = this.sort;
                // console.log(data);
            }, error => {
                this.errorHTTP = error;
            });
        }
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
        this.GetSRN();
        datepicker.close();
    }

    clearMonth() {
        this.month = new FormControl(moment(null));
        // this.month.setValue(null);
        this.GetSRN();
    }

    exportToExcel() {

        if (this.srnDetailsArray.length === 0) {
            this.http.ShowError('Please check SRN Details to export data')
            return;
        }
        const fileName = 'SRN'
        const filteredData = [...this.srnDetailsArray]
        // let removeKeys = Object.keys(filteredData[0]).filter(x => !this.displayedColumns.includes(x));
        // let data = [];//filteredData.map(x => { removeKeys.forEach(key => delete x[key]); return x });
        // filteredData.forEach(item => {
        //  let obj = {};
        //    this.displayedMPRDetailColumns.forEach(key => {
        //        obj[key] = item[key] || item;
        //        filteredData.concat();
        //        //obj = item

        // });
        //  data.push(obj)

        // });

        const result = filteredData.reduce((accumulator, value) => accumulator.concat(value), []);
        // console.log(result);

        const exportExcel: ExportExcel = {
            Title: 'SRN Report',
            Author: this.currentUser.FullName,
            Type: EnumExcelReportType.SRN,
            Month: this.month.value,
            Data: {},
            // List1: data
            List1: this.populateData(result)
        };
        this.dialogue.openExportConfirmDialogue(exportExcel).subscribe();
    }

    populateData(data: any) {
        return data.map(item => {
            return {

                'Project': item.ProjectName,
                'Scheme': item.SchemeName,
                'TSP': item.TSPNameSRNDetail,
                'Class code': item.ClassCodeSRNDetail,
                'Class start date': item.ClassStartdateSRNDetail,
                'Class end date': item.ClassEnddateSRNDetail,

                'Trainee Code': item.TraineeCode,
                'Trainee Name': item.TraineeName,
                'Father Name': item.FatherName,
                'Trainee CNIC': item.TraineeCNIC,
                'Contact Number 1': item.ContactNumber1,
                'Report Id': item.ReportId,
                Amount: item.Amount,
                'Token Number': item.TokenNumber,
                'Transaction Number': item.TransactionNumber,
                Comments: item.Comments
                // "IsPaid": item.IsPaid,
            }
        })
    }

    // added Project api endpoint:
    getFundingCategories() {
        this.http.getJSON(`api/Scheme/GetScheme?OID=${this.http.OID.value}`).subscribe((d: any) => {
            this.Project = d[4];
        },
            (error) => {
                this.error = error.error;
                this.http.ShowError(error.error + '\n' + error.message);
            } // error path
        );
    }

    // added classstatus api endpoint:
    GetClassStatus() {
        this.http.getJSON('api/ClassStatus/RD_ClassStatus').subscribe(
            (cs: any) => {
                this.ClassStatus = cs;
                console.log('Class Status Data:', cs); // Improved logging
            },
            error => {
                this.error = error; // Error handling
                console.error('Error fetching ClassStatus:', error);
            }
        );
    }
    openTraineeJourneyDialogue(data: any): void {
        // debugger;
        this.dialogue.openTraineeJourneyDialogue(data);
    }

    openClassJourneyDialogue(data: any): void {
        // debugger;
        this.dialogue.openClassJourneyDialogue(data);
    }
}

export interface ISRNReportFilter {
    SchemeID: number;
    TSPMasterID: number;
    //TSPID: number;
    KAMID: number;
    FundingCategoryID?: number;
    ClassStatusID?: number;
}

export interface ISRNReportFilterMultiSelect {
    Schemes: [];
    TSPMasterIDs: [];
    //TSPID: number;
    KAMID: number;
    FundingCategoryID?: number;
    ClassStatusID?: number;
}
