import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from '../../../environments/environment';
import { FormGroup, FormBuilder, Validators, FormGroupDirective } from '@angular/forms';
import { CommonSrvService } from '../../common-srv.service';

import { ModelBase } from '../../shared/ModelBase';
import { UserRightsModel } from '../../master-data/users/users.component';

@Component({
    selector: 'app-pbte-datasharing-timelines',
    templateUrl: './pbte-datasharing-timelines.component.html',
    styleUrls: ['./pbte-datasharing-timelines.component.scss']
})
export class PBTEDataSharingTimelinesComponent implements OnInit {
    pbtedatasharingtimelinesform: FormGroup;
    title: string; savebtn: string;
    displayedColumns = ['ClassTimeline', 'TSPTimeline', 'TraineeTimeline', 'DropOutTraineeTimeline', 'InActive', "Action"];
    pbtedatasharingtimelines: MatTableDataSource<any>;

    formrights: UserRightsModel;
    EnText: string = "PBTE Data-Sharing Timeline";
    error: String;
    query = {
        order: 'ID',
        limit: 5,
        page: 1
    };
    @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
    @ViewChild(MatSort, { static: false }) sort: MatSort;
    working: boolean;
    constructor(private fb: FormBuilder, private ComSrv: CommonSrvService) {
        this.pbtedatasharingtimelinesform = this.fb.group({
            ID: 0,
            ClassTimeline: ["", Validators.required],
            TSPTimeline: ["", Validators.required],
            TraineeTimeline: ["", Validators.required],
            DropOutTraineeTimeline: ["", Validators.required],
            InActive: ""
        }, { updateOn: "blur" });
        this.pbtedatasharingtimelines = new MatTableDataSource([]);
        this.formrights = ComSrv.getFormRights();
    }


    GetData() {
        this.ComSrv.getJSON('api/PBTEDataSharingTimelines/GetPBTEDataSharingTimelines').subscribe((d: any) => {
            this.pbtedatasharingtimelines = new MatTableDataSource(d);
            this.pbtedatasharingtimelines.paginator = this.paginator;
            this.pbtedatasharingtimelines.sort = this.sort;
        }, error => this.error = error // error path

        );
    }
    ngOnInit() {
        this.ComSrv.setTitle("PBTE Data Sharing Timelines");
        this.title = "Add New ";
        this.savebtn = "Save ";
        this.GetData();
    }

    Submit(myform: FormGroupDirective) {
        if (!this.pbtedatasharingtimelinesform.valid)
            return;
        this.working = true;
        this.ComSrv.postJSON('api/PBTEDataSharingTimelines/Save', this.pbtedatasharingtimelinesform.value)
            .subscribe((d: any) => {
                this.pbtedatasharingtimelines = new MatTableDataSource(d);
                this.pbtedatasharingtimelines.paginator = this.paginator;
                this.pbtedatasharingtimelines.sort = this.sort;
                this.ComSrv.openSnackBar(this.ID.value > 0 ? environment.UpdateMSG.replace("${Name}", this.EnText) : environment.SaveMSG.replace("${Name}", this.EnText));
                this.reset(myform);
                this.title = "Add New ";
                this.savebtn = "Save ";
                this.working = false;
            },
            (error) => {
                this.error = error.error;
                this.ComSrv.ShowError(error.error);
                
              });


    }
    reset(myform: FormGroupDirective) {
        this.pbtedatasharingtimelinesform.reset();
        //myform.resetFrom();
        this.ID.setValue(0);
        this.title = "Add New ";
        this.savebtn = "Save ";
    }
    toggleEdit(row) {
        this.ID.setValue(row.ID);
        this.ClassTimeline.setValue(row.ClassTimeline);
        this.TSPTimeline.setValue(row.TSPTimeline);
        this.TraineeTimeline.setValue(row.TraineeTimeline);
        this.DropOutTraineeTimeline.setValue(row.DropOutTraineeTimeline);
        this.InActive.setValue(row.InActive);

        this.title = "Update ";
        this.savebtn = "Save ";
    }
    toggleActive(row) {
        this.ComSrv.confirm().subscribe(result => {
            if (result) {
                this.ComSrv.postJSON('api/PBTEDataSharingTimelines/ActiveInActive', { 'ID': row.ID, 'InActive': row.InActive })
                    .subscribe((d: any) => {
                        this.ComSrv.openSnackBar(row.InActive == true ? environment.InActiveMSG.replace("${Name}", this.EnText) : environment.ActiveMSG.replace("${Name}", this.EnText));
                        // this.pbtedatasharingtimelines =new MatTableDataSource(d);
                    },
                    (error) => {
                        this.error = error.error;
                        this.ComSrv.ShowError(error.error);
                        row.InActive = !row.InActive;
                      });

            }

            else {
                row.InActive = !row.InActive;
            }
        });
    }
    applyFilter(filterValue: string) {
        filterValue = filterValue.trim(); // Remove whitespace
        filterValue = filterValue.toLowerCase(); // Datasource defaults to lowercase matches
        this.pbtedatasharingtimelines.filter = filterValue;
    }

    get ID() { return this.pbtedatasharingtimelinesform.get("ID"); }
    get ClassTimeline() { return this.pbtedatasharingtimelinesform.get("ClassTimeline"); }
    get TSPTimeline() { return this.pbtedatasharingtimelinesform.get("TSPTimeline"); }
    get TraineeTimeline() { return this.pbtedatasharingtimelinesform.get("TraineeTimeline"); }
    get DropOutTraineeTimeline() { return this.pbtedatasharingtimelinesform.get("DropOutTraineeTimeline"); }
    get InActive() { return this.pbtedatasharingtimelinesform.get("InActive"); }



}
export class PBTEDataSharingTimelinesModel extends ModelBase {
    ID: number;
    ClassTimeline: number;
    TSPTimeline: number;
    TraineeTimeline: number;
    DropOutTraineeTimeline: number;

}
