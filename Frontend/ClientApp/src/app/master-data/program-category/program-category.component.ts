import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from '../../../environments/environment';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { CommonSrvService } from '../../common-srv.service';
import { ModelBase } from '../../shared/ModelBase';
import { UserRightsModel } from '../../master-data/users/users.component';
import { error } from 'protractor';

@Component({
    selector: 'app-program-category',
    templateUrl: './program-category.component.html',
    styleUrls: ['./program-category.component.scss']
})

export class ProgramCategoryComponent implements OnInit {
    programcategoryform: FormGroup;
    title: string; savebtn: string;
    displayedColumns = ['PCategoryName', 'PTypeID', 'InActive'];
    //displayedColumns = ['PCategoryName', 'PTypeID', 'InActive', "Action"];
    programcategory: MatTableDataSource<any>;
    ProgramType = [];
    formrights: UserRightsModel;
    EnText: string = "Program Category";
    error: String;
    query = {
        order: 'PCategoryID',
        limit: 5,
        page: 1
    };
    @ViewChild(MatPaginator) paginator: MatPaginator;
    @ViewChild(MatSort) sort: MatSort;
    working: boolean;
    constructor(private fb: FormBuilder, private ComSrv: CommonSrvService) {
        this.programcategoryform = this.fb.group({
            PCategoryID: 0,
            PCategoryName: ["", Validators.required],
            PTypeID: ["", Validators.required],
            InActive: ""
        }, { updateOn: "blur" });
        this.programcategory = new MatTableDataSource([]);
        this.formrights = ComSrv.getFormRights();
    }

    GetData() {
        this.GetProgramCatergoryData().subscribe((d: any) => {
            this.programcategory = new MatTableDataSource(d[0])
            this.ProgramType = d[1];

            this.programcategory.paginator = this.paginator;
            this.programcategory.sort = this.sort;
        }, error => this.error = error // error path
        );
    }
    ngOnInit() {
        this.ComSrv.setTitle("Program Category");
        this.title = "Add New ";
        this.savebtn = "Save ";
        this.GetData();
    }

    Submit() {
        if (!this.programcategoryform.valid)
            return;
        this.working = true;
        
        this.GetProgramCatergoryData().subscribe((d: any) => {
                this.programcategory = new MatTableDataSource(d);
                this.programcategory.paginator = this.paginator;
                this.programcategory.sort = this.sort;
                this.ComSrv.openSnackBar(this.PCategoryID.value > 0 ? environment.UpdateMSG.replace("${Name}", this.EnText) : environment.SaveMSG.replace("${Name}", this.EnText));
                this.reset();
                this.title = "Add New ";
                this.savebtn = "Save ";
            },
                error => this.error = error // error path
                , () => {
                    this.working = false;

                });
    }

    GetProgramCatergoryData() {
        return this.ComSrv.getJSON('api/ProgramCategory/GetProgramCategory');
    }

    CheckExists() {
        if (this.PCategoryName.value.trim() == "")
            return;

        this.GetProgramCatergoryData().subscribe((d: any) => {

            let exist: any = d[0].find(x => x.PCategoryName.trim().toLowerCase() == this.PCategoryName.value.trim().toLowerCase());

            if (exist) {
                this.PCategoryName.setErrors({ exists: true });
            }
            else {
                this.PCategoryName.updateValueAndValidity();
            }
        });
    }

    reset() {
        this.programcategoryform.reset();
        this.PCategoryID.setValue(0);
        this.title = "Add New ";
        this.savebtn = "Save ";
    }
    toggleEdit(row) {
        this.PCategoryID.setValue(row.PCategoryID);
        this.PCategoryName.setValue(row.PCategoryName);
        this.PTypeID.setValue(row.PTypeID);
        this.InActive.setValue(row.InActive);

        this.title = "Update ";
        this.savebtn = "Save ";
    }
    toggleActive(row) {
        this.ComSrv.confirm().subscribe(result => {
            if (result) {
                this.ComSrv.postJSON('api/ProgramCategory/ActiveInActive', { 'PCategoryID': row.PCategoryID, 'InActive': row.InActive })
                    .subscribe((d: any) => {
                        this.ComSrv.openSnackBar(row.InActive == true ? environment.InActiveMSG.replace("${Name}", this.EnText) : environment.ActiveMSG.replace("${Name}", this.EnText));
                        // this.programcategory =new MatTableDataSource(d);
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
        this.programcategory.filter = filterValue;
    }

    get PCategoryID() { return this.programcategoryform.get("PCategoryID"); }
    get PCategoryName() { return this.programcategoryform.get("PCategoryName"); }
    get PTypeID() { return this.programcategoryform.get("PTypeID"); }
    get InActive() { return this.programcategoryform.get("InActive"); }
}
export class ProgramCategoryModel extends ModelBase {
    PCategoryID: number;
    PCategoryName: string;
    PTypeID: number;

}

