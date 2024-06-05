import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from '../../../environments/environment';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { CommonSrvService } from '../../common-srv.service';

import { ModelBase } from '../../shared/ModelBase';
import { UserRightsModel } from '../../master-data/users/users.component';

@Component({
    selector: 'app-employment-status',
    templateUrl: './employment-status.component.html',
    styleUrls: ['./employment-status.component.scss']
})
export class EmploymentStatusComponent implements OnInit {
    employmentstatusform: FormGroup;
    title: string; savebtn: string;
    displayedColumns = ['EmploymentStatusName', 'InActive'];
    //displayedColumns = ['EmploymentStatusName', 'InActive', "Action"];
    employmentstatus: MatTableDataSource<any>;

    formrights: UserRightsModel;
    EnText: string = "Employment Status";
    error: String;
    query = {
        order: 'EmploymentStatusID',
        limit: 5,
        page: 1
    };
    @ViewChild(MatPaginator) paginator: MatPaginator;
    @ViewChild(MatSort) sort: MatSort;
    working: boolean;
    constructor(private fb: FormBuilder, private ComSrv: CommonSrvService) {
        this.employmentstatusform = this.fb.group({
            EmploymentStatusID: 0,
            EmploymentStatusName: ["", Validators.required],
            InActive: ""
        }, { updateOn: "blur" });
        this.employmentstatus = new MatTableDataSource([]);
        this.formrights = ComSrv.getFormRights();
    }


    GetData() {
        this.ComSrv.getJSON('api/EmploymentStatus/GetEmploymentStatus').subscribe((d: any) => {
            this.employmentstatus = new MatTableDataSource(d);
            this.employmentstatus.paginator = this.paginator;
            this.employmentstatus.sort = this.sort;
        }, error => this.error = error // error path
        );
    }
    ngOnInit() {
        this.ComSrv.setTitle("Employment Status");
        this.title = "Add New ";
        this.savebtn = "Save ";
        this.GetData();
    }

    Submit() {
        if (!this.employmentstatusform.valid)
            return;
        this.working = true;
        this.ComSrv.postJSON('api/EmploymentStatus/Save', this.employmentstatusform.value)
            .subscribe((d: any) => {
                this.employmentstatus = new MatTableDataSource(d);
                this.employmentstatus.paginator = this.paginator;
                this.employmentstatus.sort = this.sort;
                this.ComSrv.openSnackBar(this.EmploymentStatusID.value > 0 ? environment.UpdateMSG.replace("${Name}", this.EnText) : environment.SaveMSG.replace("${Name}", this.EnText));
                this.reset();
                this.title = "Add New ";
                this.savebtn = "Save ";
            },
                error => this.error = error // error path
                , () => {
                    this.working = false;

                });


    }
    reset() {
        this.employmentstatusform.reset();
        this.EmploymentStatusID.setValue(0);
        this.title = "Add New ";
        this.savebtn = "Save ";
    }
    toggleEdit(row) {
        this.EmploymentStatusID.setValue(row.EmploymentStatusID);
        this.EmploymentStatusName.setValue(row.EmploymentStatusName);
        this.InActive.setValue(row.InActive);

        this.title = "Update ";
        this.savebtn = "Save ";
    }
    toggleActive(row) {
        this.ComSrv.confirm().subscribe(result => {
            if (result) {
                this.ComSrv.postJSON('api/EmploymentStatus/ActiveInActive', { 'EmploymentStatusID': row.EmploymentStatusID, 'InActive': row.InActive })
                    .subscribe((d: any) => {
                        this.ComSrv.openSnackBar(row.InActive == true ? environment.InActiveMSG.replace("${Name}", this.EnText) : environment.ActiveMSG.replace("${Name}", this.EnText));
                        // this.employmentstatus =new MatTableDataSource(d);
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
        this.employmentstatus.filter = filterValue;
    }

    get EmploymentStatusID() { return this.employmentstatusform.get("EmploymentStatusID"); }
    get EmploymentStatusName() { return this.employmentstatusform.get("EmploymentStatusName"); }
    get InActive() { return this.employmentstatusform.get("InActive"); }

}
export class EmploymentStatusModel extends ModelBase {
    EmploymentStatusID: number;
    EmploymentStatusName: string;

}
