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
    selector: 'app-certification-category',
    templateUrl: './certification-category.component.html',
    styleUrls: ['./certification-category.component.scss']
})
export class CertificationCategoryComponent implements OnInit {
    certificationcategoryform: FormGroup;
    title: string; savebtn: string;
    displayedColumns = ['CertificationCategoryName', 'InActive', "Action"];
    certificationcategory: MatTableDataSource<any>;

    formrights: UserRightsModel;
    EnText: string = "Certification Category";
    error: String;
    query = {
        order: 'CertificationCategoryID',
        limit: 5,
        page: 1
    };
    @ViewChild(MatPaginator) paginator: MatPaginator;
    @ViewChild(MatSort) sort: MatSort;
    working: boolean;
    constructor(private fb: FormBuilder, private ComSrv: CommonSrvService) {
        this.certificationcategoryform = this.fb.group({
            CertificationCategoryID: 0,
            CertificationCategoryName: ["", Validators.required],
            InActive: ""
        }, { updateOn: "blur" });
        this.certificationcategory = new MatTableDataSource([]);
        this.formrights = ComSrv.getFormRights();
    }


    GetData() {
        return this.ComSrv.getJSON('api/CertificationCategory/GetCertificationCategory');
    }
    ngOnInit() {
        this.ComSrv.setTitle("Certification Category");
        this.title = "Add New ";
        this.savebtn = "Save ";

        this.GetData().subscribe((d: any) => {
            this.populateList(d);
        }, error => this.error = error // error path
        );
    }

    Submit() {
        if (!this.certificationcategoryform.valid)
            return;
        this.working = true;
        this.ComSrv.postJSON('api/CertificationCategory/Save', this.certificationcategoryform.value)
            .subscribe((d: any) => {
                this.populateList(d);
                this.ComSrv.openSnackBar(this.CertificationCategoryID.value > 0 ? environment.UpdateMSG.replace("${Name}", this.EnText) : environment.SaveMSG.replace("${Name}", this.EnText));
                this.reset();
                this.title = "Add New ";
                this.savebtn = "Save ";
                this.working = false;
            },
            (error) => {
                this.error = error.error;
                this.ComSrv.ShowError(error.error);
                
              });
    }
    reset() {
        this.certificationcategoryform.reset();
        this.CertificationCategoryID.setValue(0);
        this.title = "Add New ";
        this.savebtn = "Save ";
    }
    toggleEdit(row) {
        this.CertificationCategoryID.setValue(row.CertificationCategoryID);
        this.CertificationCategoryName.setValue(row.CertificationCategoryName);
        this.InActive.setValue(row.InActive);

        this.title = "Update ";
        this.savebtn = "Save ";
    }
    toggleActive(row) {
        this.ComSrv.confirm().subscribe(result => {
            if (result) {
                this.ComSrv.postJSON('api/CertificationCategory/ActiveInActive', { 'CertificationCategoryID': row.CertificationCategoryID, 'InActive': row.InActive })
                    .subscribe((d: any) => {
                        this.ComSrv.openSnackBar(row.InActive == true ? environment.InActiveMSG.replace("${Name}", this.EnText) : environment.ActiveMSG.replace("${Name}", this.EnText));
                        // this.certificationcategory =new MatTableDataSource(d);
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
        this.certificationcategory.filter = filterValue;
    }

    CheckExists() {
        if (this.CertificationCategoryName.value.trim() == "")
            return;

        this.GetData().subscribe((d: any) => {
            this.populateList(d);

            let exist: any = d.find(x => x.CertificationCategoryName.trim().toLowerCase() == this.CertificationCategoryName.value.trim().toLowerCase());

            if (exist) {
                this.CertificationCategoryName.setErrors({ exists: true });
            }
            else {
                this.CertificationCategoryName.updateValueAndValidity();
            }
        });
    }

    populateList(d: any) {
        this.certificationcategory = new MatTableDataSource(d);
        this.certificationcategory.paginator = this.paginator;
        this.certificationcategory.sort = this.sort;
    }

    get CertificationCategoryID() { return this.certificationcategoryform.get("CertificationCategoryID"); }
    get CertificationCategoryName() { return this.certificationcategoryform.get("CertificationCategoryName"); }
    get InActive() { return this.certificationcategoryform.get("InActive"); }

}
export class CertificationCategoryModel extends ModelBase {
    CertificationCategoryID: number;
    CertificationCategoryName: string;

}
