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
    selector: 'app-centers',
    templateUrl: './centers.component.html',
    styleUrls: ['./centers.component.scss']
})
export class CenterComponent implements OnInit {
    centersform: FormGroup;
    title: string; savebtn: string;
    displayedColumns = ['CenterName', 'CenterAddress',
 //'CenterGeoLocation',
        'CenterDistrict', 'CenterTehsil', 'CenterInchargeName', 'CenterInchargeMobile', 'InActive'
        //'UID', 'IsMigrated', 'HouseNo', 'Street', 'Town'
        , "Action"];
    centers: MatTableDataSource<any>;

    formrights: UserRightsModel;
    EnText: string = "Center";
    District: []; Tehsil: [];
    error: String;
    query = {
        order: 'CenterID',
        limit: 5,
        page: 1
    };
    @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
    @ViewChild(MatSort, { static: false }) sort: MatSort;
    working: boolean;
    constructor(private fb: FormBuilder, private ComSrv: CommonSrvService) {
        this.centersform = this.fb.group({
            CenterID: 0,
        }, { updateOn: "blur" });
        this.centers = new MatTableDataSource([]);
        this.formrights = ComSrv.getFormRights();
    }

    GetData() {
        this.ComSrv.getJSON('api/Centers/GetCenters').subscribe((d: any) => {
            this.centers = new MatTableDataSource(d[0]);
            this.District = d[1];
            this.Tehsil = d[2];
            this.centers.paginator = this.paginator;
            this.centers.sort = this.sort;
        }, error => this.error = error // error path
        );
    }
    ngOnInit() {
        this.ComSrv.setTitle("Centers");
        this.title = "Add New ";
        this.savebtn = "Save ";
        this.GetData();
    }
    Submit() {
        if (!this.centersform.valid)
            return;
        this.working = true;
        this.ComSrv.postJSON('api/Centers/Save', this.centersform.value)
            .subscribe((d: any) => {
                this.centers = new MatTableDataSource(d);
                this.centers.paginator = this.paginator;
                this.centers.sort = this.sort;
                this.ComSrv.openSnackBar(this.CenterID.value > 0 ? environment.UpdateMSG.replace("${Name}", this.EnText) : environment.SaveMSG.replace("${Name}", this.EnText));
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
        this.centersform.reset();
        //myform.resetFrom();
        this.CenterID.setValue(0);
        this.title = "Add New ";
        this.savebtn = "Save ";
    }
    toggleEdit(row) {
        this.CenterID.setValue(row.CenterID);
        this.title = "Update ";
        this.savebtn = "Save ";
    }
    toggleActive(row) {
        this.ComSrv.confirm().subscribe(result => {
            if (result) {
                this.ComSrv.postJSON('api/Centers/ActiveInActive', { 'CenterID': row.CenterID, 'InActive': row.InActive })
                    .subscribe((d: any) => {
                        this.ComSrv.openSnackBar(row.InActive == true ? environment.InActiveMSG.replace("${Name}", this.EnText) : environment.ActiveMSG.replace("${Name}", this.EnText));
                        // this.centers =new MatTableDataSource(d);
                    },
                        error => this.error = error // error path
			  );
    }
      else {
    row.InActive = !row.InActive;
}
    });
        }
applyFilter(filterValue: string) {
    filterValue = filterValue.trim(); // Remove whitespace
    filterValue = filterValue.toLowerCase(); // Datasource defaults to lowercase matches
    this.centers.filter = filterValue;
}

get CenterID() { return this.centersform.get("CenterID"); }
}
export class CentersModel extends ModelBase {
    CenterID: number;
}