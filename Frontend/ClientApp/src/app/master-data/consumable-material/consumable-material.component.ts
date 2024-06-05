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
  selector: 'app-consumable-material',
  templateUrl: './consumable-material.component.html',
  styleUrls: ['./consumable-material.component.scss']
})
export class ConsumableMaterialComponent implements OnInit {
    consumablematerialform: FormGroup;
    title: string; savebtn: string;
    displayedColumns = ['ItemName', 'InActive', "Action"];
    consumablematerial: MatTableDataSource<any>;

    formrights: UserRightsModel;
    EnText: string = "Consumable Material";
    error: String;
    query = {
        order: 'ConsumableMaterialID',
        limit: 5,
        page: 1
    };
    @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
    @ViewChild(MatSort, { static: false }) sort: MatSort;
    working: boolean;
    constructor(private fb: FormBuilder, private ComSrv: CommonSrvService) {
        this.consumablematerialform = this.fb.group({
            ConsumableMaterialID: 0,
            ItemName: ["", Validators.required],
            InActive: ""
        }, { updateOn: "blur" });
        this.consumablematerial = new MatTableDataSource([]);
        this.formrights = ComSrv.getFormRights();
    }


    GetData() {
        this.ComSrv.getJSON('api/ConsumableMaterial/GetConsumableMaterial').subscribe((d: any) => {
            this.consumablematerial = new MatTableDataSource(d);
            this.consumablematerial.paginator = this.paginator;
            this.consumablematerial.sort = this.sort;
        }, error => this.error = error // error path
        );
    }
    ngOnInit() {
        this.ComSrv.setTitle("Consumable Material");
        this.title = "Add New ";
        this.savebtn = "Save ";
        this.GetData();
    }

    Submit(myform: FormGroupDirective) {
        if (!this.consumablematerialform.valid)
            return;
        this.working = true;
        this.ComSrv.postJSON('api/ConsumableMaterial/Save', this.consumablematerialform.value)
            .subscribe((d: any) => {
                this.consumablematerial = new MatTableDataSource(d);
                this.consumablematerial.paginator = this.paginator;
                this.consumablematerial.sort = this.sort;
                this.ComSrv.openSnackBar(this.ConsumableMaterialID.value > 0 ? environment.UpdateMSG.replace("${Name}", this.EnText) : environment.SaveMSG.replace("${Name}", this.EnText));
                this.reset(myform);
                this.title = "Add New ";
                this.working = false;
                this.savebtn = "Save ";
            },
            (error) => {
                this.error = error.error;
                this.ComSrv.ShowError(error.error);
                
              });


    }
    reset(myform: FormGroupDirective) {
        this.consumablematerialform.reset();
        //myform.resetFrom();
        this.ConsumableMaterialID.setValue(0);
        this.title = "Add New ";
        this.savebtn = "Save ";
    }
    toggleEdit(row) {
        this.ConsumableMaterialID.setValue(row.ConsumableMaterialID);
        this.ItemName.setValue(row.ItemName);
        this.InActive.setValue(row.InActive);

        this.title = "Update ";
        this.savebtn = "Save ";
    }
    toggleActive(row) {
        this.ComSrv.confirm().subscribe(result => {
            if (result) {
                this.ComSrv.postJSON('api/ConsumableMaterial/ActiveInActive', { 'ConsumableMaterialID': row.ConsumableMaterialID, 'InActive': row.InActive })
                    .subscribe((d: any) => {
                        this.ComSrv.openSnackBar(row.InActive == true ? environment.InActiveMSG.replace("${Name}", this.EnText) : environment.ActiveMSG.replace("${Name}", this.EnText));
                        // this.consumablematerial =new MatTableDataSource(d);
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
    this.consumablematerial.filter = filterValue;
}

get ConsumableMaterialID() { return this.consumablematerialform.get("ConsumableMaterialID"); }
get ItemName() { return this.consumablematerialform.get("ItemName"); }
get InActive() { return this.consumablematerialform.get("InActive"); }
}
export class ConsumableMaterialModel extends ModelBase {
    ConsumableMaterialID: number;
    ItemName: string;

}
