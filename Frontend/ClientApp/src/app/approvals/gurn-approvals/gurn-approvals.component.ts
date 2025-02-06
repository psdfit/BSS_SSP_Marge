import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { CommonSrvService } from '../../common-srv.service';
import { MatDialog } from '@angular/material/dialog';
import { Overlay } from '@angular/cdk/overlay';
import { DialogueService } from '../../shared/dialogue.service';
import { EnumApprovalProcess, EnumExcelReportType } from '../../shared/Enumerations';
import { DateAdapter, MAT_DATE_FORMATS, MAT_DATE_LOCALE } from '@angular/material/core';
import { MomentDateAdapter, MAT_MOMENT_DATE_ADAPTER_OPTIONS } from '@angular/material-moment-adapter';
import { Moment } from 'moment';
import * as _moment from 'moment';
import { MatDatepicker } from '@angular/material/datepicker';
import { FormControl } from '@angular/forms';
import { GurnApprovalsDialogueComponent } from '../gurn-approvals-dialogue/gurn-approvals-dialogue.component';
import { Observable } from 'rxjs';
import { ExportExcel } from '../../shared/Interfaces';
import { GroupByPipe } from 'angular-pipes';
import { environment } from '../../../environments/environment';
import { DatePipe } from '@angular/common';

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
    selector: 'app-gurn-approvals',
    templateUrl: './gurn-approvals.component.html',
    styleUrls: ['./gurn-approvals.component.scss'],
    providers: [
        // `MomentDateAdapter` can be automatically provided by importing `MomentDateModule` in your
        // application's root module. We provide it at the component level here, due to limitations of
        // our example generation script.
        {
            provide: DateAdapter,
            useClass: MomentDateAdapter,
            deps: [MAT_DATE_LOCALE, MAT_MOMENT_DATE_ADAPTER_OPTIONS]
        },

        { provide: MAT_DATE_FORMATS, useValue: MY_FORMATS }
        , GroupByPipe, DatePipe
    ],

})
export class GurnApprovalsComponent implements OnInit {
    environment = environment;

    @ViewChild(MatPaginator) paginator: MatPaginator;
    @ViewChild(MatSort) sort: MatSort;
    //resultsLength: any;
    //dtGURNDataDisplayedColumns = ['TSPName', 'TradeName', 'ClassCode', 'TraineeCode', 'TraineeName', 'FatherName', 'TraineeCNIC', 'DistrictName', 'ContactNumber1', 'TrainingAddressLocation', 'Comments', 'Batch', 'TokenNumber', 'TransactionNumber', 'Status', 'Month', 'NumberOfMonths', 'Action'];
    //dtGURNData: MatTableDataSource<any>;
    gurn: any[] = [];
    gurnApproved: any[] = [];
    gurnPending: any[] = [];
    gurnGrouped: any[] = [];
    gurnGroupedByBatch: any[] = [];
    varProcessKey: string
    //gurnGroupList: any[] = [];
    // gurnDetails: any[];
    errorHTTP: any;
    month = new FormControl(moment());
    currentUser: any;
    kamusers: []; schemes: []; tsps: []; tspMasters: [];
    gurnDetailsArray: any[];
    GURNDetailsBulkArray: any[];

    gurnMasterArray: any[];
    GURNMasterIDs: string

    SearchSch = new FormControl('');
    SearchKAM = new FormControl('');
    SearchTSP = new FormControl('');

    //filters: IGURNApprovalFilter = { SchemeID: 0, TSPID: 0, KAMID: 0 };
    filters: IGURNApprovalFilter = { SchemeID: 0, TSPMasterID: 0, KAMID: 0 };


    constructor(private datePipe: DatePipe, private http: CommonSrvService, public dialog: MatDialog, private overlay: Overlay, private dialogue: DialogueService, private groupByPipe: GroupByPipe) { }

    ngOnInit(): void {
        this.http.setTitle("Guru Payment Note");
        this.currentUser = this.http.getUserDetails();
        this.gurnDetailsArray = []
        this.http.OID.subscribe(OID => {
            this.GetGURN();
            this.GetFiltersData();
        })
    }
    EmptyCtrl() {
        this.SearchKAM.setValue('');
        this.SearchTSP.setValue('');
        this.SearchSch.setValue('');
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
            }
            , (error) => {
                console.error(JSON.stringify(error));
            }
        );
    }

    GetGURN() {
        //let month = new Date('2020-03-01');
        //this.http.postJSON(`api/GURN/GetGURN`, { Month: this.month.value, KAMID: this.filters.KAMID, SchemeID: this.filters.SchemeID, TSPID: this.filters.TSPID }).subscribe(
        this.http.postJSON(`api/GURN/GetGURN`, { Month: this.month.value, KAMID: this.filters.KAMID, SchemeID: this.filters.SchemeID, TSPMasterID: this.filters.TSPMasterID }).subscribe(
            (data: any) => {
                this.gurnMasterArray = data.map(o => o.GURNID);
                this.GURNMasterIDs = this.gurnMasterArray.join(',');
                this.gurn = [];
                this.gurnGroupedByBatch = this.groupByPipe.transform(data, "ApprovalBatchNo")
                let indexGURN = 0;
                this.gurnGroupedByBatch.forEach(key => {
                    var number = indexGURN; //Number(key.key) - 1;
                    //this.gurnGrouped = this.groupByPipe.transform(this.gurnGroupedByBatch[key], "IsApproved")

                    this.gurnGrouped = this.groupByPipe.transform(this.gurnGroupedByBatch[number].value, "SchemeName")
                        .map(x => {
                            return { ...x, toggleValue: true, IsApproved: x.value[0]['IsApproved'] }
                        });

                    this.gurn.push(this.gurnGrouped);
                    indexGURN++;
                }
                );
                this.gurn = this.gurn.reduce((accumulator, value) => accumulator.concat(value), []);
                //this.gurnGrouped = this.groupByPipe.transform(data, "IsApproved")
                //this.gurnMasterArray = data.map(o => o.GURNID);
                //this.GURNMasterIDs = this.gurnMasterArray.join(',');
                //this.gurnPending = this.groupByPipe.transform(this.gurnGrouped[1].value, "SchemeName")
                //  .map(x => {
                //    return { ...x, toggleValue: true, IsApproved: this.gurnGrouped[1].key }
                //  });
                //this.gurn.push(this.gurnPending);
                //this.gurnApproved = this.groupByPipe.transform(this.gurnGrouped[0].value, "SchemeName")
                //  .map(x => {
                //    return { ...x, toggleValue: true, IsApproved: this.gurnGrouped[0].key }
                //  });
                //this.gurn.push(this.gurnApproved);

                //console.log(this.gurn);
                //this.gurn = this.gurn.reduce((accumulator, value) => accumulator.concat(value), []);


                //this.gurnGrouped = this.groupByPipe.transform(data, "IsApproved")
                //this.gurnMasterArray = data.map(o => o.GURNID);
                //this.GURNMasterIDs = this.gurnMasterArray.join(',');
                //this.gurnPending = this.groupByPipe.transform(this.gurnGrouped[1].value, "SchemeName")
                //  .map(x => {
                //    return { ...x, toggleValue: true, IsApproved: this.gurnGrouped[1].key }
                //  });
                //this.gurn.push(this.gurnPending);
                //this.gurnApproved = this.groupByPipe.transform(this.gurnGrouped[0].value, "SchemeName")
                //  .map(x => {
                //    return { ...x, toggleValue: true, IsApproved: this.gurnGrouped[0].key }
                //  });
                //this.gurn.push(this.gurnApproved);

                //console.log(this.gurn);
                //this.gurn = this.gurn.reduce((accumulator, value) => accumulator.concat(value), []);

            },

            (error) => {
                this.http.ShowError(error.error + '\n' + error.message);
            }
        );
    }
    GetGurnDetails(r: any) {
        if (r.gurnDetails) {
            r.gurnDetails = null;
            this.gurnDetailsArray = this.gurnDetailsArray.filter(s => s.GURNID != r.GURNID);

            return;
        }
        this.http.getJSON("api/GURN/GetGURNDetails/" + r.GURNID).subscribe(
            (data: any) => {
                r.gurnDetails = data[0];
                this.gurnDetailsArray.push(data[0]);
                this.gurnDetailsArray = this.gurnDetailsArray.reduce((accumulator, value) => accumulator.concat(value), []);

            },
            (error) => {
                this.http.ShowError(error.error + '\n' + error.message);
            }
        );
    }
    ///---Invoke Dialog---S--////
    //openApprovalDialogue(row: any): void {
    //  //{ ProcessKey: 'AP', FormID:  row.GurnId }
    //  //let datas: IApprovalHistory = { ProcessKey: 'AP', FormID: 292 };
    //  this.dialogue.openApprovalDialogue(EnumApprovalProcess.GURN, row.GURNID).subscribe(result => { console.log(result); });
    //}

    public openGURNApprovalDialogue(gurnGroup: any[]): void {
        console.log(gurnGroup)
        debugger;
        //let datas: IApprovalHistory = { ProcessKey: processKey, FormID: formID };
        var processk = gurnGroup.map(x => x.ProcessKey)

        const dialogRef = this.dialog.open(GurnApprovalsDialogueComponent, {
            width: '60%',
            data: { ProcessKey: EnumApprovalProcess.GURN, FormIDs: gurnGroup.map(x => x.GURNID) }
        })

    }
    ///---Invoke  Dialog---E--////
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
        this.GetGURN();
        datepicker.close();
    }

    clearMonth() {
        this.month = new FormControl(moment(null));
        //  this.month.setValue(null);
        this.GetGURN();
    }

    GetByGURNMasterIDs() {
        this.http.postJSON('api/GURN/GetGURNExcelExportByIDs', this.GURNMasterIDs).subscribe((d: any) => {
            this.GURNDetailsBulkArray = d;
            this.ExportToExcelBulkGURN();
        });

    }


    ExportToExcel(GURNID: number) {
        
        let filteredData = this.GURNDetailsBulkArray;

        this.http.postJSON('api/GURN/GetGURNExcelExport/', { GURNID: GURNID, Month: this.month.value }).subscribe((d: any) => {
            filteredData = d;

            let exportExcel: ExportExcel = {
                Title: 'GURN_Excel_Export',
                Author: this.currentUser.FullName,
                Type: EnumExcelReportType.GURN,
                //Data: data,
                List1: this.populateData(filteredData),
            };
            this.dialogue.openExportConfirmDialogue(exportExcel).subscribe();
        });

    }
    ExportToExcelBulkGURN() {

        //if (this.gurnDetailsArray.length == 0) {
        //    this.http.ShowError("Please check GURN Details to export data")
        //    return;
        //}
        let fileName = 'GURN'
        let filteredData = [...this.GURNDetailsBulkArray]


        const result = filteredData.reduce((accumulator, value) => accumulator.concat(value), []);
        //console.log(result);

        let exportExcel: ExportExcel = {
            Title: 'GURN Approval Report',
            Author: this.currentUser.FullName,
            Type: EnumExcelReportType.GURN,
            Month: this.month.value,
            Data: {},
            //List1: data
            List1: this.populateData(result)
        };
        this.dialogue.openExportConfirmDialogue(exportExcel).subscribe();

    }

    populateData(data: any) {
        return data.map((item, index) => {
            return {
                "SR #": index + 1,
                "Project": item.ProjectName
                , "Scheme": item.SchemeName
                , "TSP": item.TSPName
                , "Class Start Date": this.datePipe.transform(item.ClassStartDate, 'dd/MM/yyyy')
                , "Class End Date": this.datePipe.transform(item.ClassEndDate, 'dd/MM/yyyy')
                , "GURU Name": item.GURUName
                , "GURU CNIC": item.GURUCNIC
                , "GURU Contact Number": item.GURUContactNumber
                , "Trainee Code": item.TraineeCode
                , "Trainee Name": item.TraineeName
                , "Father Name": item.FatherName
                , "Trainee CNIC": item.TraineeCNIC
                , "Contact Number": item.ContactNumber1
                , "Token Number": ""
                , "Transaction Number": ""
                , "Amount": item.Amount
                , "Comments": item.Comments
            }
        })
    }
    getTraineesAmountWise(r: any) {
        if (r.traineesAmountWise && r.traineesAmountWise[0].value[0].GURNID == r.GURNID) {
            //r.traineesAmountWise = null;
            return;
        }
        this.http.getJSON("api/GURN/GetGURNDetails/" + r.GURNID).subscribe(
            (data: any) => {
                r.traineesAmountWise = this.groupByPipe.transform(data[0], "Amount")
            },
            (error) => {
                this.http.ShowError(error.error + '\n' + error.message);
            }
        );
        console.log(r.traineesAmountWise)
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

export interface IGURNApprovalFilter {
    SchemeID: number;
    //TSPID: number;
    TSPMasterID: number;
    KAMID: number;
}
