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
    selector: 'app-call-center-agent',
    templateUrl: './call-center-agent-dialog.component.html',
    styleUrls: ['./call-center-agent-dialog.component.scss']
})
export class CallCetnerAgentDialogComponent implements OnInit {
    callcenteragentform: FormGroup;
    title: string; savebtn: string;
    displayedColumns = ['Scheme', 'TSP', 'DateOfRequest', 'ClassCode', 'Trade', 'Trainees', 'Duration', 'AddressOfTrainingLocation', "Action"];
    agent: MatTableDataSource<any>;

    formrights: UserRightsModel;
    EnText: string = "Nominated Person";
    error: String;
    success: String;
    query = {
        order: 'CallCenterAgentID',
        limit: 5,
        page: 1
    };
    @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
    @ViewChild(MatSort, { static: false }) sort: MatSort;
    working: boolean;


    constructor(private fb: FormBuilder,
        private ComSrv: CommonSrvService,
        public dialogRef: MatDialogRef<CallCetnerAgentDialogComponent>,
        @Inject(MAT_DIALOG_DATA) public data: any) {
        this.callcenteragentform = this.fb.group({
            //CallCenterAgentID: 0,
            NominatedPersonName: ["", Validators.required],
            NominatedPersonContactNumber: ["", Validators.required],
            VisitPlanID: "",
            UserID:"",
            InActive: ""

        }, { updateOn: "blur" });
        this.agent = new MatTableDataSource([]);
        this.formrights = ComSrv.getFormRights();
    }


    GetData() {
        this.ComSrv.getJSON('api/agent/GetCallCenterAgent').subscribe((d: any) => {
            this.agent = new MatTableDataSource(d);
            this.agent.paginator = this.paginator;
            this.agent.sort = this.sort;
        }, error => this.error = error // error path
        );
    }
    ngOnInit() {
        this.ComSrv.setTitle("agent");
        this.title = "Add New ";
        this.savebtn = "Save ";

        this.VisitPlanID.setValue(this.data.VisitPlanID)
        this.UserID.setValue(this.data.UserID)



        //this.GetData();
    }

    onNoClick(): void {
        this.dialogRef.close(false);
    }

    Submit() {
        if (!this.callcenteragentform.valid)
            return;
        this.working = true;
        this.ComSrv.postJSON('api/VisitPlan/SaveNewCallCenterAgent', this.callcenteragentform.value)
            .subscribe((d: any) => {
                this.agent = new MatTableDataSource(d);
                this.agent.paginator = this.paginator;
                this.agent.sort = this.sort;
                this.success = "Nominated Person has been added";
                this.ComSrv.openSnackBar(this.success.toString(), "Success");                //this.reset(myform);
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
        this.callcenteragentform.reset();
        //myform.resetFrom();
        this.CallCenterAgentID.setValue(0);
        this.title = "Add New ";
        this.savebtn = "Save ";
    }
    toggleEdit(row) {
  

        this.title = "Update ";
        this.savebtn = "Save ";
    }
    toggleActive(row) {
        this.ComSrv.confirm().subscribe(result => {
            if (result) {
                this.ComSrv.postJSON('api/agent/ActiveInActive', { 'RTPID': row.RTPID, 'InActive': row.InActive })
                    .subscribe((d: any) => {
                        this.ComSrv.openSnackBar(row.InActive == true ? environment.InActiveMSG.replace("${Name}", this.EnText) : environment.ActiveMSG.replace("${Name}", this.EnText));
                        // this.agent =new MatTableDataSource(d);
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
        this.agent.filter = filterValue;
    }

    get CallCenterAgentID() { return this.callcenteragentform.get("CallCenterAgentID"); }
    get NominatedPersonName() { return this.callcenteragentform.get("NominatedPersonName"); }
    get NominatedPersonContactNumber() { return this.callcenteragentform.get("NominatedPersonContactNumber"); }
    get VisitPlanID() { return this.callcenteragentform.get("VisitPlanID"); }
    get UserID() { return this.callcenteragentform.get("UserID"); }
    get InActive() { return this.callcenteragentform.get("InActive"); }
 

}

export class RTPModel extends ModelBase {
    CallCenterAgentID: number;
    NominatedPersonName: string;
    VisitPlanID: number;
    UserID: number;
    NominatedPersonContactNumber: string;

}
