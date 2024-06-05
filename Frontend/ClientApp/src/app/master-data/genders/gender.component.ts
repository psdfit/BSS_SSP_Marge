import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from '../../../environments/environment';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { CommonSrvService } from '../../common-srv.service';
import { ModelBase } from '../../shared/ModelBase';
import { UserRightsModel } from '../../master-data/users/users.component';
import { Observable } from 'rxjs';

@Component({
    selector: 'app-gender',
    templateUrl: './gender.component.html',
    styleUrls: ['./gender.component.scss']
})
export class GenderComponent implements OnInit {
    genderform: FormGroup;
    title: string; savebtn: string;
    displayedColumns = ['GenderName', 'InActive', "Action"];
    gender: MatTableDataSource<any>;
    formrights: UserRightsModel;
    EnText: string = "Gender";
    error: String;
    query = {
        order: 'GenderID',
        limit: 5,
        page: 1
    };
    @ViewChild(MatPaginator) paginator: MatPaginator;
    @ViewChild(MatSort) sort: MatSort;
    working: boolean;
    constructor(private fb: FormBuilder, private http: CommonSrvService) {
        this.genderform = this.fb.group({
            GenderID: 0,
            GenderName: ["", Validators.required],
            InActive: ""
        }, { updateOn: "blur" });
        this.gender = new MatTableDataSource([]);
        this.formrights = http.getFormRights();
    }


    GetData() {
        this.GetGenderData().subscribe((d: any) => {
            this.populateGenderList(d);
        }, error => this.error = error // error path
        );
    }
    ngOnInit() {
        this.http.setTitle("Gender");
        this.title = "Add New ";
        this.savebtn = "Save ";
        this.GetData();
    }

    Submit() {
        if (!this.genderform.valid)
            return;
        this.working = true;
        this.http.postJSON('api/Gender/Save', this.genderform.value).subscribe((d: any) => {
                this.populateGenderList(d);
                this.http.openSnackBar(this.GenderID.value > 0 ? environment.UpdateMSG.replace("${Name}", this.EnText) : environment.SaveMSG.replace("${Name}", this.EnText));
                this.reset();
                this.GenderID.setValue(0);
                this.title = "Add New ";
                this.savebtn = "Save ";
                this.working = false;
            },
            (error) => {
                this.error = error.error;
               
                this.http.ShowError(error.error);
                
              });
               /*  error => this.error = error // error path
                , () => {
                    this.working = false;
                    this.http.ShowError(error.error);
                    this.working = false;
                }); */
    }

    GetGenderData() {
        return this.http.getJSON('api/Gender/GetGender');
    }

    CheckExists() {
        if (this.GenderName.value.trim() == "")
            return;

        this.GetGenderData().subscribe((d: any) => {
            this.populateGenderList(d);

            let exist: any = d.find(x => x.GenderName.trim().toLowerCase() == this.GenderName.value.trim().toLowerCase());

            if (exist) {
                this.GenderName.setErrors({ exists: true });
            }
            else {
                this.GenderName.updateValueAndValidity();
            }
        });
    }

    populateGenderList(d: any) {
        this.gender = new MatTableDataSource(d);
        this.gender.paginator = this.paginator;
        this.gender.sort = this.sort;
    }

    reset() {
        this.genderform.reset();
        this.title = "Add New ";
        this.savebtn = "Save ";
    }
    toggleEdit(row) {
        this.GenderID.setValue(row.GenderID);
        this.GenderName.setValue(row.GenderName);
        this.InActive.setValue(row.InActive);

        this.title = "Update ";
        this.savebtn = "Save ";
    }
    toggleActive(row) {
        this.http.confirm().subscribe(result => {
            if (result) {
                this.http.postJSON('api/Gender/ActiveInActive', { 'GenderID': row.GenderID, 'InActive': row.InActive })
                    .subscribe((d: any) => {
                        this.http.openSnackBar(row.InActive == true ? environment.InActiveMSG.replace("${Name}", this.EnText) : environment.ActiveMSG.replace("${Name}", this.EnText));
                        // this.organization =new MatTableDataSource(d);
                    },
                       // error => this.error = error // error path
                     
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
        this.gender.filter = filterValue;
    }

    get GenderID() { return this.genderform.get("GenderID"); }
    get GenderName() { return this.genderform.get("GenderName"); }
    get InActive() { return this.genderform.get("InActive"); }
}
export class genderModel extends ModelBase {
    GenderID: number;
    GenderName: string;
}
