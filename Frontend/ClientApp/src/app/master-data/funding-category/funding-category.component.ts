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
    selector: 'app-funding-category',
    templateUrl: './funding-category.component.html',
    styleUrls: ['./funding-category.component.scss']
})

export class FundingCategoryComponent implements OnInit {
    fundingcategoryform: FormGroup;
    title: string; savebtn: string;
    displayedColumns = ['FundingCategoryName', 'FundingSourceID', 'InActive', "Action"];
    fundingcategory: MatTableDataSource<any>;
    FundingSource = [];
    formrights: UserRightsModel;
    EnText: string = "Funding Category";
    error: String;
    query = {
        order: 'FundingCategoryID',
        limit: 5,
        page: 1
    };
    @ViewChild(MatPaginator) paginator: MatPaginator;
    @ViewChild(MatSort) sort: MatSort;
    working: boolean;
    constructor(private fb: FormBuilder, private ComSrv: CommonSrvService) {
        this.fundingcategoryform = this.fb.group({
            FundingCategoryID: 0,
            FundingCategoryName: ["", Validators.required],
            FundingSourceID: ["", Validators.required],
            InActive: ""
        }, { updateOn: "blur" });
        this.fundingcategory = new MatTableDataSource([]);
        this.formrights = ComSrv.getFormRights();
        this.FundingCategoryID.valueChanges.subscribe(val => val = val ?? 0 )
    }

    GetData() {
        return this.ComSrv.getJSON('api/FundingCategory/GetFundingCategory');
    }
    ngOnInit() {
        this.ComSrv.setTitle("Funding Category");
        this.title = "Add New ";
        this.savebtn = "Save ";
        this.GetData().subscribe((d: any) => {
            this.PopulateLists(d);
        }, error => this.error = error // error path
        );
    }

    CheckExists() {
        if (this.FundingCategoryName.value.trim() == "")
            return;

        this.GetData().subscribe((d: any) => {
            this.PopulateLists(d);

            let exist: any = d[0].find(x => x.FundingCategoryName.trim().toLowerCase() == this.FundingCategoryName.value.trim().toLowerCase());

            if (exist) {
                this.FundingCategoryName.setErrors({ exists: true });
            }
            else {
                this.FundingCategoryName.updateValueAndValidity();
            }
        });
    }

    PopulateLists(d) {
        this.fundingcategory = new MatTableDataSource(d[0])
        this.FundingSource = d[1];

        this.fundingcategory.paginator = this.paginator;
        this.fundingcategory.sort = this.sort;
    }

    Submit() {
        if (!this.fundingcategoryform.valid)
            return;
        this.working = true;
        this.ComSrv.postJSON('api/FundingCategory/Save', this.fundingcategoryform.value)
            .subscribe((d: any) => {
                this.fundingcategory = new MatTableDataSource(d);
                this.fundingcategory.paginator = this.paginator;
                this.fundingcategory.sort = this.sort;
                this.ComSrv.openSnackBar(this.FundingCategoryID.value > 0 ? environment.UpdateMSG.replace("${Name}", this.EnText) : environment.SaveMSG.replace("${Name}", this.EnText));
                this.reset();
                this.FundingCategoryID.setValue(0);

                this.title = "Add New ";
                this.savebtn = "Save ";
                this.working = false;
            },
            (error) => {
                this.error = error.error;
                this.working = false;
                this.ComSrv.ShowError(error.error);
                
              });


    }
    reset() {
        this.fundingcategoryform.reset();
        this.FundingCategoryID.setValue(0);
        this.title = "Add New ";
        this.savebtn = "Save ";
    }
    toggleEdit(row) {
        this.FundingCategoryID.setValue(row.FundingCategoryID);
        this.FundingCategoryName.setValue(row.FundingCategoryName);
        this.FundingSourceID.setValue(row.FundingSourceID);
        this.InActive.setValue(row.InActive);

        this.title = "Update ";
        this.savebtn = "Save ";
    }
    toggleActive(row) {
        this.ComSrv.confirm().subscribe(result => {
            if (result) {
                this.ComSrv.postJSON('api/FundingCategory/ActiveInActive', { 'FundingCategoryID': row.FundingCategoryID, 'InActive': row.InActive })
                    .subscribe((d: any) => {
                        this.ComSrv.openSnackBar(row.InActive == true ? environment.InActiveMSG.replace("${Name}", this.EnText) : environment.ActiveMSG.replace("${Name}", this.EnText));
                        // this.fundingcategory =new MatTableDataSource(d);
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
        this.fundingcategory.filter = filterValue;
    }

    get FundingCategoryID() { return this.fundingcategoryform.get("FundingCategoryID"); }
    get FundingCategoryName() { return this.fundingcategoryform.get("FundingCategoryName"); }
    get FundingSourceID() { return this.fundingcategoryform.get("FundingSourceID"); }
    get InActive() { return this.fundingcategoryform.get("InActive"); }

}
export class FundingCategoryModel extends ModelBase {
    FundingCategoryID: number;
    FundingCategoryName: string;
    FundingSourceID: number;

}

