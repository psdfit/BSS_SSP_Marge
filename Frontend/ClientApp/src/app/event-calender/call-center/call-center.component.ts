import { Component, OnInit, ViewChild, ElementRef, AfterViewInit, ViewChildren, QueryList } from '@angular/core';
import { CommonSrvService } from '../../common-srv.service';

import { Element } from '@angular/compiler/src/render3/r3_ast';
import { MatProgressBar } from '@angular/material/progress-bar';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { FormGroup, FormBuilder, Validators, FormGroupDirective } from '@angular/forms';
import { environment } from '../../../environments/environment';
import { ModelBase } from '../../shared/ModelBase';
import { UserRightsModel } from '../../master-data/users/users.component';
import { ActivatedRoute } from '@angular/router';
import * as moment from 'moment';
import { IOrgConfig } from '../../registration/Interface/IOrgConfig';

import { MatDialog } from '@angular/material/dialog';


import { UsersModel } from '../../master-data/users/users.component';
import { EnumCertificationAuthority, EnumTraineeResultStatusTypes, EnumUserLevel, ExportType, EnumTraineeStatusType, EnumClassStatus, EnumExcelReportType } from '../../shared/Enumerations';

import { DialogueService } from '../../shared/dialogue.service';
import { ExportExcel } from '../../shared/Interfaces';
import { GroupByPipe } from 'angular-pipes';
import { DatePipe } from '@angular/common';
import { CallCetnerAgentDialogComponent } from '../call-center-agent-dialog/call-center-agent-dialog.component';
import { VisitPlanModel } from '../../master-sheet/visit-plan-dialog/visit-plan-dialog.component';
import { animate, trigger, state, transition, style } from '@angular/animations';


@Component({
    selector: 'hrapp-call-center',
    templateUrl: './call-center.component.html',
    styleUrls: ['./call-center.component.scss'],
    animations: [
        trigger('detailExpand', [
            state('collapsed', style({ height: '0px', minHeight: '0' })),
            state('expanded', style({ height: '*' })),
            transition('expanded <=> collapsed', animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)')),
        ]),
    ],
})

export class CallCenterComponent implements OnInit {
    callcenterform: FormGroup;
    title: string; savebtn: string;

    displayedColumns = [
        'VisitTypeName',
        //'UserID',
        //'ClassID',
        'VisitStartDate',
        'VisitEndDate',
        'VisitStartTime',
        'VisitEndTime',
        //'CallCenterAgentStatus',
        //'UserStatus',
        //'InActive',
        "Action"
    ];

 
    pbteClasses: MatTableDataSource<any>;
    pbteTSPs: MatTableDataSource<any>;
    visitplan: MatTableDataSource<any>;
    visitPlan: MatTableDataSource<any>;

    expandedElement: VisitPlanModel | null;;

    filters: IQueryFilters = { SchemeID: 0, TSPID: 0, ClassID: 0 };

    users: any; shownestedtable: boolean;
    isOpenRegistration: boolean = false;
    isOpenRegistrationMessage: string = "";
    formrights: UserRightsModel;
    EnText: string = "Call Center";
    error: String;
    success: String;
    query = {
        order: 'RosiID',
        limit: 5,
        page: 1
    };
    @ViewChild(MatPaginator) paginator: MatPaginator;
    @ViewChild(MatSort) sort: MatSort;


    @ViewChild('SortTrainee') SortTrainee: MatSort;
    @ViewChild('PageTrainee') PageTrainee: MatPaginator;
    @ViewChild('SortDropOutTrainee') SortDropOutTrainee: MatSort;
    @ViewChild('PageDropOutTrainee') PageDropOutTrainee: MatPaginator;
    @ViewChild('SortTSP') SortTSP: MatSort;
    @ViewChild('PageTSP') PageTSP: MatPaginator;
    @ViewChild('SortClass') SortClass: MatSort;
    @ViewChild('PageClass') PageClass: MatPaginator;

    working: boolean;
    constructor(private fb: FormBuilder, private ComSrv: CommonSrvService, private route: ActivatedRoute, public dialog: MatDialog) {
        this.callcenterform = this.fb.group({
            RosiID: 0,
            ROSI: '',
            SchemeID: '',
            ClassID: '',
            TSPID: '',

        }, { updateOn: "blur" });
        this.visitplan = new MatTableDataSource([]);
        this.formrights = ComSrv.getFormRights("/Call Center");
    }

    primaryClick(element) {
        this.visitplan.filteredData.values['CallCenterAgentStatus'] = element;
        //console.log(this.primaryContact.name)
    }

    GetData() {
        this.ComSrv.getJSON('api/VisitPlan/GetCallCenterVisitPlan').subscribe((d: any) => {
            this.visitplan = d[0];
            //this.TSPs = d[1];
            //this.Classes = d[2];
            //this.rosi = d[3];

        }, error => this.error = error// error path
        );
    };

   

    ngOnInit() {
        this.ComSrv.setTitle("Call Center");
        this.title = "Add New ";
        this.savebtn = "Save ";
        this.GetData();
    }




    openDialog(id: any, userid:number): void {
        const dialogRef = this.dialog.open(CallCetnerAgentDialogComponent, {
            minWidth: '800px',
            minHeight: '350px',
            //data: JSON.parse(JSON.stringify(row))
            data: { "VisitPlanID": id , "UserID": userid}
            //this.GetVisitPlanData(data)
        })
        dialogRef.afterClosed().subscribe(result => {
            //console.log(result);
            this.visitPlan = result;
            //this.submitVisitPlan(result);
        })
    }

    updateVisitingStatusByCallCenterAgent(event: any, visitplanid: number, userid: number) {
        if (event) {
            this.ComSrv.postJSON('api/VisitPlan/UpdateCallCenterAgentEventStatus', { 'VisitPlanID': visitplanid, "UserStatusByCallCenter": event.target.value , "UserID": userid })
                .subscribe((d: any) => {
                    this.success = "Your Visiting Status has been saved";
                    this.ComSrv.openSnackBar(this.success.toString(), "Success");
                    //this.ComSrv.openSnackBar(row.InActive == true ? environment.InActiveMSG.replace("${Name}", this.EnText) : environment.ActiveMSG.replace("${Name}", this.EnText));
                    // this.visitplan =new MatTableDataSource(d);
                },
                    error => this.error = error // error path
                );
        }
        console.log("YEs")
    }

    GetRelevantUsers(row) {

        this.ComSrv.getJSON('api/VisitPlan/GetEventUsers/' + row.VisitPlanID)
            .subscribe((d: any) => {
                this.users = d[0];
                if (this.users) {
                    this.shownestedtable = true
                    this.expandedElement = row;
                }
                //this.benchmarkingClasses.paginator = this.paginator;
                //this.benchmarkingClasses.sort = this.sort;
                //this.ComSrv.openSnackBar(this.BenchmarkingID.value > 0 ? environment.UpdateMSG.replace("${Name}", this.EnText) : environment.SaveMSG.replace("${Name}", this.EnText));
                //this.title = "Add New ";
                //this.savebtn = "Save ";
            });
    }


    updateVisiting(event: any, visitplanid : number, userid: number) {
        if (event) {
            this.ComSrv.postJSON('api/VisitPlan/UpdateUserEventStatus', { 'VisitPlanID': visitplanid, "UserStatus": event.target.value, "UserID": userid })
                .subscribe((d: any) => {


                    this.success = "Your Visiting Status has been saved";
                    this.ComSrv.openSnackBar(this.success.toString(), "Success");
                    //this.ComSrv.openSnackBar(row.InActive == true ? environment.InActiveMSG.replace("${Name}", this.EnText) : environment.ActiveMSG.replace("${Name}", this.EnText));
                    // this.visitplan =new MatTableDataSource(d);
                },
                    error => this.error = error // error path
                );
        }
        console.log("YEs")
    }



    reset() {
        this.callcenterform.reset();
        this.RosiID.setValue(0);
        this.title = "Add New ";
        this.savebtn = "Save ";
    }
    toggleEdit(row) {

        this.RosiID.setValue(row.RosiID);
        this.ClassID.setValue(row.ClassID);

        this.title = "Update ";
        this.savebtn = "Save ";
    }
    toggleActive(row) {
        this.ComSrv.confirm().subscribe(result => {
            if (result) {
                this.ComSrv.postJSON('api/InceptionReport/ActiveInActive', { 'RosiID': row.RosiID, 'InActive': row.InActive })
                    .subscribe((d: any) => {
                        this.ComSrv.openSnackBar(row.InActive == true ? environment.InActiveMSG.replace("${Name}", this.EnText) : environment.ActiveMSG.replace("${Name}", this.EnText));
                        // this.inceptionreport =new MatTableDataSource(d);
                    },
                        error => this.error = error // error path
                    );
            }
            else {
                row.InActive = !row.InActive;
            }
        });
    }
    applyFilter(filterValue: string) {
        filterValue = filterValue.trim(); // Remove whitespace
        filterValue = filterValue.toLowerCase(); // Datasource defaults to lowercase matches
        this.pbteClasses.filter = filterValue;
    }

    get RosiID() { return this.callcenterform.get("RosiID"); }
    get ClassID() { return this.callcenterform.get("ClassID"); }
    get SchemeID() { return this.callcenterform.get("SchemeID"); }
    get TSPID() { return this.callcenterform.get("TSPID"); }

}
export class InceptionReportModel extends ModelBase {
    RosiID: number;
}

export interface IQueryFilters {
    SchemeID: number;
    TSPID: number;
    ClassID: number;
}
