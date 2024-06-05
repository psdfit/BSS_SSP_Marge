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
import { SrnApprovalsDialogueComponent } from '../srn-approvals-dialogue/srn-approvals-dialogue.component';
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
    selector: 'app-srn-approvals',
    templateUrl: './srn-approvals.component.html',
    styleUrls: ['./srn-approvals.component.scss'],
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
        , GroupByPipe,DatePipe
    ],
    
})
export class SrnApprovalsComponent implements OnInit {
    environment = environment;

    @ViewChild(MatPaginator) paginator: MatPaginator;
    @ViewChild(MatSort) sort: MatSort;
    //resultsLength: any;
    //dtSRNDataDisplayedColumns = ['TSPName', 'TradeName', 'ClassCode', 'TraineeCode', 'TraineeName', 'FatherName', 'TraineeCNIC', 'DistrictName', 'ContactNumber1', 'TrainingAddressLocation', 'Comments', 'Batch', 'TokenNumber', 'TransactionNumber', 'Status', 'Month', 'NumberOfMonths', 'Action'];
    //dtSRNData: MatTableDataSource<any>;
    srn: any[] = [];
    srnApproved: any[] = [];
    srnPending: any[] = [];
    srnGrouped: any[] = [];
    srnGroupedByBatch: any[] = [];
    //srnGroupList: any[] = [];
    // srnDetails: any[];
    errorHTTP: any;
    month = new FormControl(moment());
    currentUser: any;
  kamusers: []; schemes: []; tsps: []; tspMasters: [];
  srnDetailsArray: any[];
  SRNDetailsBulkArray: any[];

  srnMasterArray: any[];
  SRNMasterIDs: string

    SearchSch = new FormControl('');
    SearchKAM = new FormControl('');
    SearchTSP = new FormControl('');

    //filters: ISRNApprovalFilter = { SchemeID: 0, TSPID: 0, KAMID: 0 };
    filters: ISRNApprovalFilter = { SchemeID: 0, TSPMasterID: 0, KAMID: 0 };


    constructor(private datePipe: DatePipe,private http: CommonSrvService, public dialog: MatDialog, private overlay: Overlay, private dialogue: DialogueService, private groupByPipe: GroupByPipe) { }

    ngOnInit(): void {
        this.http.setTitle("Stipend Recommendation Note");
        this.currentUser = this.http.getUserDetails();
      this.srnDetailsArray = []
        this.http.OID.subscribe(OID => {
            this.GetSRN();
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

    GetSRN() {
        //let month = new Date('2020-03-01');
        //this.http.postJSON(`api/SRN/GetSRN`, { Month: this.month.value, KAMID: this.filters.KAMID, SchemeID: this.filters.SchemeID, TSPID: this.filters.TSPID }).subscribe(
      this.http.postJSON(`api/SRN/GetSRN`, { Month: this.month.value, KAMID: this.filters.KAMID, SchemeID: this.filters.SchemeID, TSPMasterID: this.filters.TSPMasterID }).subscribe(
          (data: any) => {
            console.log(data)
            this.srnMasterArray = data.map(o => o.SRNID);
            this.SRNMasterIDs = this.srnMasterArray.join(',');

            this.srn = [];
            this.srnGroupedByBatch = this.groupByPipe.transform(data, "ApprovalBatchNo")
            let indexSRN = 0;
            this.srnGroupedByBatch.forEach(key => {
              var number = indexSRN; //Number(key.key) - 1;
              //this.srnGrouped = this.groupByPipe.transform(this.srnGroupedByBatch[key], "IsApproved")

              this.srnGrouped = this.groupByPipe.transform(this.srnGroupedByBatch[number].value, "SchemeName")
                .map(x => {
                  return { ...x, toggleValue: true , IsApproved : x.value[0]['IsApproved'] }
                });

              this.srn.push(this.srnGrouped);
              indexSRN++;
            }
            );
            this.srn = this.srn.reduce((accumulator, value) => accumulator.concat(value), []);
            //this.srnGrouped = this.groupByPipe.transform(data, "IsApproved")
            //this.srnMasterArray = data.map(o => o.SRNID);
            //this.SRNMasterIDs = this.srnMasterArray.join(',');
            //this.srnPending = this.groupByPipe.transform(this.srnGrouped[1].value, "SchemeName")
            //  .map(x => {
            //    return { ...x, toggleValue: true, IsApproved: this.srnGrouped[1].key }
            //  });
            //this.srn.push(this.srnPending);
            //this.srnApproved = this.groupByPipe.transform(this.srnGrouped[0].value, "SchemeName")
            //  .map(x => {
            //    return { ...x, toggleValue: true, IsApproved: this.srnGrouped[0].key }
            //  });
            //this.srn.push(this.srnApproved);

            //console.log(this.srn);
            //this.srn = this.srn.reduce((accumulator, value) => accumulator.concat(value), []);


            //this.srnGrouped = this.groupByPipe.transform(data, "IsApproved")
            //this.srnMasterArray = data.map(o => o.SRNID);
            //this.SRNMasterIDs = this.srnMasterArray.join(',');
            //this.srnPending = this.groupByPipe.transform(this.srnGrouped[1].value, "SchemeName")
            //  .map(x => {
            //    return { ...x, toggleValue: true, IsApproved: this.srnGrouped[1].key }
            //  });
            //this.srn.push(this.srnPending);
            //this.srnApproved = this.groupByPipe.transform(this.srnGrouped[0].value, "SchemeName")
            //  .map(x => {
            //    return { ...x, toggleValue: true, IsApproved: this.srnGrouped[0].key }
            //  });
            //this.srn.push(this.srnApproved);

            //console.log(this.srn);
            //this.srn = this.srn.reduce((accumulator, value) => accumulator.concat(value), []);

          },

            (error) => {
                this.http.ShowError(error.error + '\n' + error.message);
            }
        );
    }
    GetSrnDetails(r: any) {
        if (r.srnDetails) {
            r.srnDetails = null;
            this.srnDetailsArray = this.srnDetailsArray.filter(s => s.SRNID != r.SRNID);

            return;
        }
        this.http.getJSON("api/SRN/GetSRNDetails/" + r.SRNID).subscribe(
            (data: any) => {
                r.srnDetails = data[0];
                this.srnDetailsArray.push(data[0]);
                this.srnDetailsArray = this.srnDetailsArray.reduce((accumulator, value) => accumulator.concat(value), []);

            },
            (error) => {
                this.http.ShowError(error.error + '\n' + error.message);
            }
        );
    }
    ///---Invoke Dialog---S--////
    //openApprovalDialogue(row: any): void {
    //  //{ ProcessKey: 'AP', FormID:  row.SrnId }
    //  //let datas: IApprovalHistory = { ProcessKey: 'AP', FormID: 292 };
    //  this.dialogue.openApprovalDialogue(EnumApprovalProcess.SRN, row.SRNID).subscribe(result => { console.log(result); });
    //}

    public openSRNApprovalDialogue(srnGroup: any[]): void {
        console.log(srnGroup)
        //let datas: IApprovalHistory = { ProcessKey: processKey, FormID: formID };
        const dialogRef = this.dialog.open(SrnApprovalsDialogueComponent, {
            width: '60%',
            data: { ProcessKey: EnumApprovalProcess.SRN, FormIDs: srnGroup.map(x => x.SRNID) }
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
        this.GetSRN();
        datepicker.close();
    }

    clearMonth() {
        this.month = new FormControl(moment(null));
      //  this.month.setValue(null);
        this.GetSRN();
  }

  GetBySRNMasterIDs() {
    this.http.postJSON('api/SRN/GetSRNExcelExportByIDs', this.SRNMasterIDs).subscribe((d: any) => {
      this.SRNDetailsBulkArray = d;
      this.ExportToExcelBulkSRN();
    });

  }


  ExportToExcel(SRNID: number) {
    let filteredData = this.SRNDetailsBulkArray;

        this.http.postJSON('api/SRN/GetSRNExcelExport/', { SRNID: SRNID, Month: this.month.value }).subscribe((d: any) => {
            filteredData = d;

            let exportExcel: ExportExcel = {
                Title: 'SRN_Excel_Export',
                Author: this.currentUser.FullName,
                Type: EnumExcelReportType.SRN,
                //Data: data,
                List1: this.populateData(filteredData),
            };
            this.dialogue.openExportConfirmDialogue(exportExcel).subscribe();

        });

    }
    ExportToExcelBulkSRN() {

        //if (this.srnDetailsArray.length == 0) {
        //    this.http.ShowError("Please check SRN Details to export data")
        //    return;
        //}
        let fileName = 'SRN'
      let filteredData = [...this.SRNDetailsBulkArray]


        const result = filteredData.reduce((accumulator, value) => accumulator.concat(value), []);
        //console.log(result);

        let exportExcel: ExportExcel = {
            Title: 'SRN Approval Report',
            Author: this.currentUser.FullName,
            Type: EnumExcelReportType.SRN,
            Month: this.month.value,
            Data: {},
            //List1: data
            List1: this.populateData(result)
        };
        this.dialogue.openExportConfirmDialogue(exportExcel).subscribe();

    }

    populateData(data: any) {
        return data.map((item,index) => {
            return {
                "SR #":index+1,
                "Project": item.FundingCategory
                ,"Scheme": item.SchemeName
                ,"TSP": item.TSPName
                ,"Class Start Date": this.datePipe.transform(item.ClassStartDate,'dd/MM/yyyy')
                ,"Class End Date": this.datePipe.transform(item.ClassEndDate,'dd/MM/yyyy')
                ,"Trainee Code": item.TraineeCode
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
        if (r.traineesAmountWise && r.traineesAmountWise[0].value[0].SRNID == r.SRNID) {
            //r.traineesAmountWise = null;
            return;
        }
        this.http.getJSON("api/SRN/GetSRNDetails/" + r.SRNID).subscribe(
            (data: any) => {
                r.traineesAmountWise = this.groupByPipe.transform(data[0], "Amount")
            },
            (error) => {
                this.http.ShowError(error.error + '\n' + error.message);
            }
        );
        console.log(r.traineesAmountWise)
    }
    openTraineeJourneyDialogue(data: any): void 
    {
        debugger;
        this.dialogue.openTraineeJourneyDialogue(data);
    }

    openClassJourneyDialogue(data: any): void 
    {
        debugger;
        this.dialogue.openClassJourneyDialogue(data);
    }


}

export interface ISRNApprovalFilter {
    SchemeID: number;
    //TSPID: number;
  TSPMasterID: number;
    KAMID: number;
}
