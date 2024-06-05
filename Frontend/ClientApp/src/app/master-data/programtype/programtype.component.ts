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
    selector: 'app-programtype',
    templateUrl: './programtype.component.html',
    styleUrls: ['./programtype.component.scss']
})
export class ProgramTypeComponent implements OnInit {
    programtypeform: FormGroup;
    title: string; savebtn: string;
    displayedColumns = ['PTypeName', 'InActive'];
    //displayedColumns = ['PTypeName', 'InActive', "Action"];
    programtype: MatTableDataSource<any>;

    formrights: UserRightsModel;
    EnText: string = "Program Type";
    error: String;
    query = {
        order: 'PTypeID',
        limit: 5,
        page: 1
    };
    @ViewChild(MatPaginator) paginator: MatPaginator;
    @ViewChild(MatSort) sort: MatSort;
    working: boolean;
    constructor(private fb: FormBuilder, private http: CommonSrvService) {
        this.programtypeform = this.fb.group({
            PTypeID: 0,
            PTypeName: ["", Validators.required],
            InActive: ""
        }, { updateOn: "blur" });
        this.programtype = new MatTableDataSource([]);
      this.formrights = http.getFormRights();
      this.PTypeID.valueChanges.subscribe((val) => {
        if (val == null)
          this.PTypeID.setValue(0);
      });
    }


    GetData() {
        this.http.getJSON('api/ProgramType/GetProgramType').subscribe((d: any) => {
            this.programtype = new MatTableDataSource(d);
            this.programtype.paginator = this.paginator;
            this.programtype.sort = this.sort;
        }, error => this.error = error // error path
        );
    }
    ngOnInit() {
        this.http.setTitle("Program Type");
        this.title = "Add New ";
        this.savebtn = "Save ";
        this.GetData();
    }

    Submit() {
        if (!this.programtypeform.valid)
            return;
      this.working = true;
      if (this.PTypeID.value == null)
        this.PTypeID.setValue(0);
        this.http.postJSON('api/ProgramType/Save', this.programtypeform.value)
            .subscribe((d: any) => {
                this.programtype = new MatTableDataSource(d);
                this.programtype.paginator = this.paginator;
                this.programtype.sort = this.sort;
                this.http.openSnackBar(this.PTypeID.value > 0 ? environment.UpdateMSG.replace("${Name}", this.EnText) : environment.SaveMSG.replace("${Name}", this.EnText));
                this.reset();
                this.PTypeID.setValue(0);

                this.title = "Add New ";
                this.savebtn = "Save ";
            },
                error => this.error = error // error path
                , () => {
                    this.working = false;

                });


    }
    reset() {
        this.programtypeform.reset();
        this.PTypeID.setValue(0);
        this.title = "Add New ";
        this.savebtn = "Save ";
    }
    toggleEdit(row) {
        this.PTypeID.setValue(row.PTypeID);
        this.PTypeName.setValue(row.PTypeName);
        this.InActive.setValue(row.InActive);

        this.title = "Update ";
        this.savebtn = "Save ";
    }
    toggleActive(row) {
        this.http.confirm().subscribe(result => {
            if (result) {
                this.http.postJSON('api/ProgramType/ActiveInActive', { 'PTypeID': row.PTypeID, 'InActive': row.InActive })
                    .subscribe((d: any) => {
                        this.http.openSnackBar(row.InActive == true ? environment.InActiveMSG.replace("${Name}", this.EnText) : environment.ActiveMSG.replace("${Name}", this.EnText));
                        // this.programtype =new MatTableDataSource(d);
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
        this.programtype.filter = filterValue;
    }

    get PTypeID() { return this.programtypeform.get("PTypeID"); }
    get PTypeName() { return this.programtypeform.get("PTypeName"); }
    get InActive() { return this.programtypeform.get("InActive"); }
}
export class ProgramTypeModel extends ModelBase {
    PTypeID: number;
    PTypeName: string;

}
