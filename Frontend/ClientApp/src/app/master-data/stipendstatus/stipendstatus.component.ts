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
    selector: 'app-stipendstatus',
    templateUrl: './stipendstatus.component.html',
    styleUrls: ['./stipendstatus.component.scss']
})
export class StipendStatusComponent implements OnInit {
    stipendstatusform: FormGroup;
    title: string; savebtn: string;
    displayedColumns = ['StipendStatusName', 'InActive', "Action"];
    stipendstatus: MatTableDataSource<any>;

    formrights: UserRightsModel;
    EnText: string = "Stipend Status";
    error: String;
    query = {
        order: 'StipendStatusID',
        limit: 5,
        page: 1
    };
    @ViewChild(MatPaginator) paginator: MatPaginator;
    @ViewChild(MatSort) sort: MatSort;
    working: boolean;
    constructor(private fb: FormBuilder, private ComSrv: CommonSrvService) {
        this.stipendstatusform = this.fb.group({
            StipendStatusID: 0,
            StipendStatusName: ["", Validators.required],
            InActive: ""
        }, { updateOn: "blur" });
        this.stipendstatus = new MatTableDataSource([]);
        this.formrights = ComSrv.getFormRights();
    }


    GetData() {
        return this.ComSrv.getJSON('api/StipendStatus/GetStipendStatus');
    }

    ngOnInit() {
        this.ComSrv.setTitle("Stipend Status");
        this.title = "Add New ";
        this.savebtn = "Save ";
        this.GetData().subscribe((d: any) => {
            this.PopulateLists(d);
        }, error => this.error = error // error path
        );
    }

    CheckExists() {
        if (this.StipendStatusName.value.trim() == "")
            return;

        this.GetData().subscribe((d: any) => {
            this.PopulateLists(d);

            let exist: any = d.find(x => x.StipendStatusName.trim().toLowerCase() == this.StipendStatusName.value.trim().toLowerCase());

            if (exist) {
                this.StipendStatusName.setErrors({ exists: true });
            }
            else {
                this.StipendStatusName.updateValueAndValidity();
            }
        });
    }

    PopulateLists(d: any) {
        this.stipendstatus = new MatTableDataSource(d);
        this.stipendstatus.paginator = this.paginator;
        this.stipendstatus.sort = this.sort;
    }

    Submit() {
        if (!this.stipendstatusform.valid)
            return;
        this.working = true;
        this.ComSrv.postJSON('api/StipendStatus/Save', this.stipendstatusform.value)
            .subscribe((d: any) => {
                this.PopulateLists(d);
                this.ComSrv.openSnackBar(this.StipendStatusID.value > 0 ? environment.UpdateMSG.replace("${Name}", this.EnText) : environment.SaveMSG.replace("${Name}", this.EnText));
                this.reset();
                this.title = "Add New ";
                this.working = false;
                this.savebtn = "Save ";
            },
            (error) => {
                this.error = error.error;
                this.ComSrv.ShowError(error.error);
                
              });
    }

    reset() {
        this.stipendstatusform.reset();
        this.StipendStatusID.setValue(0);
        this.title = "Add New ";
        this.savebtn = "Save ";
    }

    toggleEdit(row) {
        this.StipendStatusID.setValue(row.StipendStatusID);
        this.StipendStatusName.setValue(row.StipendStatusName);
        this.InActive.setValue(row.InActive);

        this.title = "Update ";
        this.savebtn = "Save ";
    }

    toggleActive(row) {
        this.ComSrv.confirm().subscribe(result => {
            if (result) {
                this.ComSrv.postJSON('api/StipendStatus/ActiveInActive', { 'StipendStatusID': row.StipendStatusID, 'InActive': row.InActive })
                    .subscribe((d: any) => {
                        this.ComSrv.openSnackBar(row.InActive == true ? environment.InActiveMSG.replace("${Name}", this.EnText) : environment.ActiveMSG.replace("${Name}", this.EnText));
                        // this.stipendstatus =new MatTableDataSource(d);
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
        this.stipendstatus.filter = filterValue;
    }

    get StipendStatusID() { return this.stipendstatusform.get("StipendStatusID"); }
    get StipendStatusName() { return this.stipendstatusform.get("StipendStatusName"); }
    get InActive() { return this.stipendstatusform.get("InActive"); }
}
export class StipendStatusModel extends ModelBase {
    StipendStatusID: number;
    StipendStatusName: string;

}
