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
    selector: 'app-equipment-tool',
    templateUrl: './equipment-tool.component.html',
    styleUrls: ['./equipment-tool.component.scss']
})
export class EquipmentToolComponent implements OnInit {
    equipmenttoolsform: FormGroup;
    title: string; savebtn: string;
    displayedColumns = ['EquipmentName', 'EquipmentQuantity', 'InActive', "Action"];
    equipmenttools: MatTableDataSource<any>;

    formrights: UserRightsModel;
    EnText: string = "Equipment Tool";
    error: String;
    CertificationAuthority: [];
    query = {
        order: 'EquipmentToolID',
        limit: 5,
        page: 1
    };
    @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
    @ViewChild(MatSort, { static: false }) sort: MatSort;
    working: boolean;
    constructor(private fb: FormBuilder, private ComSrv: CommonSrvService) {
        this.equipmenttoolsform = this.fb.group({
            EquipmentToolID: 0,
            EquipmentName: ["", Validators.required],
            EquipmentQuantity: ["", Validators.required],
            //CertAuthID: "",
            InActive: ""
        }, { updateOn: "blur" });
        this.equipmenttools = new MatTableDataSource([]);
        this.formrights = ComSrv.getFormRights();
    }


    GetData() {
        this.ComSrv.getJSON('api/EquipmentTools/GetEquipmentTools').subscribe((d: any) => {
            this.equipmenttools = new MatTableDataSource(d[0]);
            //this.CertificationAuthority = d[1];
            this.equipmenttools.paginator = this.paginator;
            this.equipmenttools.sort = this.sort;
        }, error => this.error = error // error path
        );
    }
    ngOnInit() {
        this.ComSrv.setTitle("Equipment Tools");
        this.title = "Add New ";
        this.savebtn = "Save ";
        this.GetData();
    }

    Submit(myform: FormGroupDirective) {
        if (!this.equipmenttoolsform.valid)
            return;
        this.working = true;
        this.ComSrv.postJSON('api/EquipmentTools/Save', this.equipmenttoolsform.value)
            .subscribe((d: any) => {
                this.equipmenttools = new MatTableDataSource(d);
                this.equipmenttools.paginator = this.paginator;
                this.equipmenttools.sort = this.sort;
                this.ComSrv.openSnackBar(this.EquipmentToolID.value > 0 ? environment.UpdateMSG.replace("${Name}", this.EnText) : environment.SaveMSG.replace("${Name}", this.EnText));
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
        this.equipmenttoolsform.reset();
        //myform.resetFrom();
        this.EquipmentToolID.setValue(0);
        this.title = "Add New ";
        this.savebtn = "Save ";
    }
    toggleEdit(row) {
        this.EquipmentToolID.setValue(row.EquipmentToolID);
        this.EquipmentName.setValue(row.EquipmentName);
        this.EquipmentQuantity.setValue(row.EquipmentQuantity);
        //this.CertAuthID.setValue(row.CertAuthID);
        this.InActive.setValue(row.InActive);

        this.title = "Update ";
        this.savebtn = "Save ";
    }
    toggleActive(row) {
        this.ComSrv.confirm().subscribe(result => {
            if (result) {
                this.ComSrv.postJSON('api/EquipmentTools/ActiveInActive', { 'EquipmentToolID': row.EquipmentToolID, 'InActive': row.InActive })
                    .subscribe((d: any) => {
                        this.ComSrv.openSnackBar(row.InActive == true ? environment.InActiveMSG.replace("${Name}", this.EnText) : environment.ActiveMSG.replace("${Name}", this.EnText));
                        // this.equipmenttools =new MatTableDataSource(d);
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
        this.equipmenttools.filter = filterValue;
    }

    get EquipmentToolID() { return this.equipmenttoolsform.get("EquipmentToolID"); }
    get EquipmentName() { return this.equipmenttoolsform.get("EquipmentName"); }
    get EquipmentQuantity() { return this.equipmenttoolsform.get("EquipmentQuantity"); }
    //get CertAuthID() { return this.equipmenttoolsform.get("CertAuthID"); }
    get InActive() { return this.equipmenttoolsform.get("InActive"); }
}
export class EquipmentToolsModel extends ModelBase {
    EquipmentToolID: number;
    EquipmentName: string;
    EquipmentQuantity: number;
    //CertAuthID: number;


}
