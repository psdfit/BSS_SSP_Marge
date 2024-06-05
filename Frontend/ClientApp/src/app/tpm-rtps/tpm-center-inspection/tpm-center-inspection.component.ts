import { Component, OnInit, ViewChild, ElementRef, Inject } from '@angular/core';
import { CommonSrvService } from '../../common-srv.service';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

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
import * as XLSX from 'xlsx';
import { ITraineeProfile } from '../../registration/Interface/ITraineeProfile';



@Component({
    selector: 'hrapp-tpm-center-inspection',
    templateUrl: './tpm-center-inspection.component.html',
    styleUrls: ['./tpm-center-inspection.component.scss']
})

export class TPMCenterInspectionComponent implements OnInit {
    centerinspectionform: FormGroup;
    title: string; savebtn: string;

    displayedColumnsClasses = ['ClassID','ClassCode', 'ExpectedStartDate',
        //'TraineeID',
        'TradeName'];

    tpmrtps: MatTableDataSource<any>;
    pbteTSPs: MatTableDataSource<any>;
    pbteTrainees: MatTableDataSource<any>;
    pbteDropOutTrainees: MatTableDataSource<any>;

    selectedClasses: any;
    selectedTrainees: any;
    selectedTSPs: any;

    //data: any;

    traineeResultStatusTypes: any;

    update: String;



    isOpenRegistration: boolean = false;
    isOpenRegistrationMessage: string = "";
    formrights: UserRightsModel;
    EnText: string = "Center Inspection Report";
    error: String;
    query = {
        order: 'IncepReportID',
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
    constructor(private fb: FormBuilder, private ComSrv: CommonSrvService, private route: ActivatedRoute,
        public dialogRef: MatDialogRef<TPMCenterInspectionComponent>,
        @Inject(MAT_DIALOG_DATA) public data: any) {
        this.centerinspectionform = this.fb.group({
            CenterInspectionID: 0,
            ClassID: "",
            ClassCode:"",
            ExpectedStartDate:"",
            TradeName:"",
            InActive: ""
        }, { updateOn: "blur" });
        this.pbteTrainees = new MatTableDataSource([]);
        this.formrights = ComSrv.getFormRights("/pbte");
    }



    GetData(id:string) {
        this.ComSrv.getJSON('api/RTP/GetCenterInspectionByClass/' + id).subscribe((d: any) => {
            this.tpmrtps = new MatTableDataSource(d[0]);
            this.TradeName.setValue(d[0].TradeName);
            this.ExpectedStartDate.setValue(d[0].ExpectedStartDate);
            this.tpmrtps.paginator = this.PageClass;
            this.tpmrtps.sort = this.SortClass;

        }, error => this.error = error// error path
        );
    };



    ngOnInit() {
        this.ComSrv.setTitle("TPM-RTPs");
        this.title = "Add New ";
        this.savebtn = "Save ";
        this.GetData(this.data.ClassID);
        this.ClassID.setValue(this.data.ClassID);
        this.ClassCode.setValue(this.data.ClassCode);
    }

    Submit() {
        //if (!this.durationform.valid)
        //    return;
        //this.working = true;
        this.ComSrv.getJSON('api/RTP/UpdateCenterInspection/', + this.ClassID.value)
            .subscribe((d: any) => {
                //this.duration = new MatTableDataSource(d);
                //this.duration.paginator = this.paginator;
                //this.duration.sort = this.sort;
                this.ComSrv.openSnackBar(this.CenterInspectionID.value > 0 ? environment.UpdateMSG.replace("${Name}", this.EnText) : environment.SaveMSG.replace("${Name}", this.EnText));
                //this.reset(myform);
                //this.title = "Add New ";
                //this.savebtn = "Save ";
            },
                error => this.error = error // error path
                , () => {
                    this.working = false;

                });


    }



    toggleActive(row) {
        this.ComSrv.confirm().subscribe(result => {
            if (result) {
                this.ComSrv.postJSON('api/InceptionReport/ActiveInActive', { 'IncepReportID': row.IncepReportID, 'InActive': row.InActive })
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
        //this.pbteClasses.filter = filterValue;
    }


    get CenterInspectionID() { return this.centerinspectionform.get("CenterInspectionID"); }
    get InActive() { return this.centerinspectionform.get("InActive"); }
    get ClassID() { return this.centerinspectionform.get("ClassID"); }
    get ClassCode() { return this.centerinspectionform.get("ClassCode"); }
    get ExpectedStartDate() { return this.centerinspectionform.get("ExpectedStartDate"); }
    get TradeName() { return this.centerinspectionform.get("TradeName"); }

}

