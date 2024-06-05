import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Component, OnInit, ViewChild, Inject } from '@angular/core';
import { environment } from '../../../environments/environment';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { FormGroup, FormBuilder, Validators, FormGroupDirective, NgForm } from '@angular/forms';
import { CommonSrvService } from '../../common-srv.service';
import { ModelBase } from '../../shared/ModelBase';
import { UserRightsModel } from '../../master-data/users/users.component';
import * as moment from 'moment';
import { DateTime } from 'luxon';

@Component({
    selector: 'app-inception-report-rtp',
    templateUrl: './rtp.component.html',
    styleUrls: ['./rtp.component.scss']
})
export class RTPDialogComponent implements OnInit {
    rtpform: FormGroup;
    title: string; savebtn: string;
    displayedColumns = ['Scheme', 'TSP', 'DateOfRequest', 'ClassCode', 'Trade', 'Trainees', 'Duration', 'AddressOfTrainingLocation', "Action"];
    rtp: MatTableDataSource<any>;

    formrights: UserRightsModel;
    EnText: string = "Request To Proceed";
    error: String;
    query = {
        order: 'RTPID',
        limit: 5,
        page: 1
    };
    @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
    @ViewChild(MatSort, { static: false }) sort: MatSort;
    working: boolean;
 

    constructor(private fb: FormBuilder,
        private ComSrv: CommonSrvService,
        public dialogRef: MatDialogRef < RTPDialogComponent >,
        @Inject(MAT_DIALOG_DATA) public data: any) {
        this.rtpform = this.fb.group({
            RTPID: 0,
            RTPValue:"",
            //Scheme: "",
            //TSP: "",
            //DateOfRequest: "",
            //DateOfRequest: "",
            ClassID:"",
            ClassCode: "",
            //Trade: "",
            //Trainees: "",
            //Duration: "",
            SchemeName: ["", Validators.required],
            TSPName: ["", Validators.required],
            TradeName: ["", Validators.required],
            TraineesPerClass: ["", Validators.required],
            Duration: ["", Validators.required],
            Name: ["", Validators.required],    
            CPName: ["", Validators.required],   
            CPLandline: ["", Validators.required],
            StartDate: ["", Validators.required],

            DistrictID: ["", Validators.required],
            DistrictName: ["", Validators.required],
            TehsilID: ["", Validators.required],
            TehsilName: ["", Validators.required],
            AddressOfTrainingLocation: ["", Validators.required],
            Comments: "",
            IsApproved: 0,
            ISRejected: 0,
            NTP: 0,
            CenterInspection: 0,
            InActive: ""

        }, { updateOn: "blur" });
        this.rtp = new MatTableDataSource([]);
        this.formrights = ComSrv.getFormRights();
    }


    GetData() {
        this.ComSrv.getJSON('api/RTP/GetRTP').subscribe((d: any) => {
            this.rtp = new MatTableDataSource(d);
            this.rtp.paginator = this.paginator;
            this.rtp.sort = this.sort;
        }, error => this.error = error // error path
        );
    }
    ngOnInit() {
        this.ComSrv.setTitle("RTP");
        this.title = "Add New ";
        this.savebtn = "Save ";


        this.ClassCode.setValue(this.data.ClassCode)
        this.DistrictID.setValue(this.data.DistrictID)
        this.DistrictName.setValue(this.data.DistrictName)
        this.TehsilID.setValue(this.data.TehsilID)
        this.TehsilName.setValue(this.data.TehsilName)
        this.ClassID.setValue(this.data.ClassID)
        this.RTPValue.setValue(this.data.RTPValue)
        this.SchemeName.setValue(this.data.SchemeName)
        this.TSPName.setValue(this.data.TSPName)
        this.TradeName.setValue(this.data.TradeName)
        this.TraineesPerClass.setValue(this.data.TraineesPerClass)
        this.Duration.setValue(this.data.Duration)
        this.Name.setValue(this.data.Name)
        this.CPName.setValue(this.data.CPName)
        this.CPLandline.setValue(this.data.CPLandline)
        this.StartDate.setValue(this.data.StartDate)
        //this.GetData();
    }

    onNoClick(): void {
        this.dialogRef.close(false);
    }

    Submit() {
        if (!this.rtpform.valid)
            return;
        this.working = true;
        this.ComSrv.postJSON('api/RTP/Save', this.rtpform.value)
            .subscribe((d: any) => {
                this.rtp = new MatTableDataSource(d);
                this.rtp.paginator = this.paginator;
                this.rtp.sort = this.sort;
                this.ComSrv.openSnackBar(this.RTPID.value > 0 ? environment.UpdateMSG.replace("${Name}", this.EnText) : environment.SaveMSG.replace("${Name}", this.EnText));
                //this.reset(myform);
                this.title = "Add New ";
                this.savebtn = "Save ";
                this.dialogRef.close(true);

            },
                error => this.error = error // error path
                , () => {
                    this.working = false;

                });


    }
    reset() {
        this.rtpform.reset();
        //myform.resetFrom();
        this.RTPID.setValue(0);
        this.title = "Add New ";
        this.savebtn = "Save ";
    }
    toggleEdit(row) {
        this.RTPID.setValue(row.RTPID);  
        this.ClassCode.setValue(row.ClassCode);
        this.AddressOfTrainingLocation.setValue(row.AddressOfTrainingLocation);

        this.title = "Update ";
        this.savebtn = "Save ";
    }
    toggleActive(row) {
        this.ComSrv.confirm().subscribe(result => {
            if (result) {
                this.ComSrv.postJSON('api/RTP/ActiveInActive', { 'RTPID': row.RTPID, 'InActive': row.InActive })
                    .subscribe((d: any) => {
                        this.ComSrv.openSnackBar(row.InActive == true ? environment.InActiveMSG.replace("${Name}", this.EnText) : environment.ActiveMSG.replace("${Name}", this.EnText));
                        // this.rtp =new MatTableDataSource(d);
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
        this.rtp.filter = filterValue;
    }

    get RTPID() { return this.rtpform.get("RTPID"); }
    get RTPValue() { return this.rtpform.get("RTPValue"); }
    get ClassID() { return this.rtpform.get("ClassID"); }
    get ClassCode() { return this.rtpform.get("ClassCode"); }
    get DistrictID() { return this.rtpform.get("DistrictID"); }
    get DistrictName() { return this.rtpform.get("DistrictName"); }
    get TehsilID() { return this.rtpform.get("TehsilID"); }
    get TehsilName() { return this.rtpform.get("TehsilName"); }
    get SchemeName() { return this.rtpform.get("SchemeName"); }
    get TSPName() { return this.rtpform.get("TSPName"); }
    get TradeName() { return this.rtpform.get("TradeName"); }
    get TraineesPerClass() { return this.rtpform.get("TraineesPerClass"); }
    get Duration() { return this.rtpform.get("Duration"); }
    get Name() { return this.rtpform.get("Name"); }
    get CPName() { return this.rtpform.get("CPName"); }
    get CPLandline() { return this.rtpform.get("CPLandline"); }
    get StartDate() { return this.rtpform.get("StartDate"); }
    get AddressOfTrainingLocation() { return this.rtpform.get("AddressOfTrainingLocation"); }
    get InActive() { return this.rtpform.get("InActive"); }
    get Comments() { return this.rtpform.get("Comments"); }
    get IsApproved() { return this.rtpform.get("IsApproved"); }
    get IsRejected() { return this.rtpform.get("IsRejected"); }

}

export class RTPModel extends ModelBase {
    RTPID: number;
    RTPValue: boolean;
    ClassID: number;
    ClassCode: string;
    AddressOfTrainingLocation: string;

}
