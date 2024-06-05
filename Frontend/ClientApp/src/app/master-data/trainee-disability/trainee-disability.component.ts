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
    selector: 'app-trainee-disability',
    templateUrl: './trainee-disability.component.html',
    styleUrls: ['./trainee-disability.component.scss']
})
export class TraineeDisabilityComponent implements OnInit {
    traineedisabilityform: FormGroup;
    title: string; savebtn: string;
    displayedColumns = ['Disability', 'InActive', "Action"];
    traineedisability: MatTableDataSource<any>;

    formrights: UserRightsModel;
    EnText: string = "Disability";
    error: String;
    query = {
        order: 'Id',
        limit: 5,
        page: 1
    };
    @ViewChild(MatPaginator) paginator: MatPaginator;
    @ViewChild(MatSort) sort: MatSort;
    working: boolean;
    constructor(private fb: FormBuilder, private http: CommonSrvService) {
        this.traineedisabilityform = this.fb.group({
            Id: 0,
            Disability: "",
            InActive: ""
        }, { updateOn: "blur" });
        this.traineedisability = new MatTableDataSource([]);
        this.formrights = http.getFormRights();
        this.Id.valueChanges.subscribe((val) => {
            if (val == null)
                this.Id.setValue(0);
        });
    }

    ngOnInit() {
        this.http.setTitle("Trainee Disability");
        this.title = "Add New ";
        this.savebtn = "Save ";
        this.GetData().subscribe((d: any) => {
            this.PopulateList(d);
        }, error => this.error = error // error path
        );
    }

    GetData() {
        return this.http.getJSON('api/TraineeDisability/GetTraineeDisability');
    }

    PopulateList(d) {
        this.traineedisability = new MatTableDataSource(d);
        this.traineedisability.paginator = this.paginator;
        this.traineedisability.sort = this.sort;
    }

    Submit() {
        if (!this.traineedisabilityform.valid)
            return;
        this.working = true;
        if (this.Id.value == null)
            this.Id.setValue(0);
        this.http.postJSON('api/TraineeDisability/Save', this.traineedisabilityform.value)
            .subscribe((d: any) => {
                this.PopulateList(d);
                this.http.openSnackBar(this.Id.value > 0 ? environment.UpdateMSG.replace("${Name}", this.EnText) : environment.SaveMSG.replace("${Name}", this.EnText));
                this.reset();
                this.title = "Add New ";
                this.savebtn = "Save ";
                this.working = false;
            },
            (error) => {
                this.error = error.error;
                this.http.ShowError(error.error);
                
              });
    }

    CheckExists() {
        if (this.Disability.value.trim() == "")
            return;

        this.GetData().subscribe((d: any) => {
            this.PopulateList(d);

            let exist: any = d.find(x => x.Disability.trim().toLowerCase() == this.Disability.value.trim().toLowerCase());

            if (exist) {
                this.Disability.setErrors({ exists: true });
            }
            else {
                this.Disability.updateValueAndValidity();
            }
        });
    }

    reset() {
        this.traineedisabilityform.reset();
        this.title = "Add New ";
        this.savebtn = "Save ";
    }
    toggleEdit(row) {
        this.Id.setValue(row.Id);
        this.Disability.setValue(row.Disability);
        this.InActive.setValue(row.InActive);

        this.title = "Update ";
        this.savebtn = "Save ";
    }
    toggleActive(row) {
        this.http.confirm().subscribe(result => {
            if (result) {
                this.http.postJSON('api/TraineeDisability/ActiveInActive', { 'Id': row.Id, 'InActive': row.InActive })
                    .subscribe((d: any) => {
                        this.http.openSnackBar(row.InActive == true ? environment.InActiveMSG.replace("${Name}", this.EnText) : environment.ActiveMSG.replace("${Name}", this.EnText));
                        // this.organization =new MatTableDataSource(d);
                    },
                    (error) => {
                        this.error = error.error;
                        this.http.ShowError(error.error);
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
        this.traineedisability.filter = filterValue;
    }

    get Id() { return this.traineedisabilityform.get("Id"); }
    get Disability() { return this.traineedisabilityform.get("Disability"); }
    get InActive() { return this.traineedisabilityform.get("InActive"); }
}
