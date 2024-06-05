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
    selector: 'app-duration',
    templateUrl: './duration.component.html',
    styleUrls: ['./duration.component.scss']
})
export class DurationComponent implements OnInit {
    durationform: FormGroup;
    title: string; savebtn: string;
    displayedColumns = ['Duration', 'InActive', "Action"];
    duration: MatTableDataSource<any>;

    formrights: UserRightsModel;
    EnText: string = "Duration";
    error: String;
    query = {
        order: 'DurationID',
        limit: 5,
        page: 1
    };
    @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
    @ViewChild(MatSort, { static: false }) sort: MatSort;
    working: boolean;
    constructor(private fb: FormBuilder, private ComSrv: CommonSrvService) {
        this.durationform = this.fb.group({
            DurationID: 0,
            Duration: ["", Validators.required],
            InActive: ""
        }, { updateOn: "blur" });
        this.duration = new MatTableDataSource([]);
        this.formrights = ComSrv.getFormRights();
    }


    GetData() {
        this.ComSrv.getJSON('api/Duration/GetDuration').subscribe((d: any) => {
            this.duration = new MatTableDataSource(d);
            this.duration.paginator = this.paginator;
            this.duration.sort = this.sort;
        }, error => this.error = error // error path
        );
    }
    ngOnInit() {
        this.ComSrv.setTitle("Duration");
        this.title = "Add New ";
        this.savebtn = "Save ";
      this.GetData();
      this.DurationID.valueChanges.subscribe((val) => {
        if (val == null)
          this.DurationID.setValue(0);
      });
    }

    Submit(myform: FormGroupDirective) {
        if (!this.durationform.valid)
            return;
        this.working = true;
        this.ComSrv.postJSON('api/Duration/Save', this.durationform.value)
            .subscribe((d: any) => {
                this.duration = new MatTableDataSource(d);
                this.duration.paginator = this.paginator;
                this.duration.sort = this.sort;
                this.ComSrv.openSnackBar(this.DurationID.value > 0 ? environment.UpdateMSG.replace("${Name}", this.EnText) : environment.SaveMSG.replace("${Name}", this.EnText));
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
        this.durationform.reset();
        //myform.resetFrom();
        this.DurationID.setValue(0);
        this.title = "Add New ";
        this.savebtn = "Save ";
    }
    toggleEdit(row) {
        this.DurationID.setValue(row.DurationID);
        this.Duration.setValue(row.Duration);
        this.InActive.setValue(row.InActive);

        this.title = "Update ";
        this.savebtn = "Save ";
    }
    toggleActive(row) {
        this.ComSrv.confirm().subscribe(result => {
            if (result) {
                this.ComSrv.postJSON('api/Duration/ActiveInActive', { 'DurationID': row.DurationID, 'InActive': row.InActive })
                    .subscribe((d: any) => {
                        this.ComSrv.openSnackBar(row.InActive == true ? environment.InActiveMSG.replace("${Name}", this.EnText) : environment.ActiveMSG.replace("${Name}", this.EnText));
                        // this.duration =new MatTableDataSource(d);
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
    this.duration.filter = filterValue;
}

get DurationID() { return this.durationform.get("DurationID"); }
get Duration() { return this.durationform.get("Duration"); }
get InActive() { return this.durationform.get("InActive"); }

}
export class DurationModel extends ModelBase {
    DurationID: number;
    Duration: number;

}
