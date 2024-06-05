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
    selector: 'app-cluster',
    templateUrl: './cluster.component.html',
    styleUrls: ['./cluster.component.scss']
})
export class ClusterComponent implements OnInit {
    clusterform: FormGroup;
    title: string; savebtn: string;
    displayedColumns = ['ClusterName', 'InActive', "Action"];
    cluster: MatTableDataSource<any>;

    formrights: UserRightsModel;
    EnText: string = "Cluster Name";
    error: String;
    query = {
        order: 'ClusterID',
        limit: 5,
        page: 1
    };
    @ViewChild(MatPaginator) paginator: MatPaginator;
    @ViewChild(MatSort) sort: MatSort;
    working: boolean;
    constructor(private fb: FormBuilder, private http: CommonSrvService) {
        this.clusterform = this.fb.group({
            ClusterID: 0,
            ClusterName: ["", Validators.required],
            InActive: ""
        }, { updateOn: "blur" });
        this.cluster = new MatTableDataSource([]);
      this.formrights = http.getFormRights();
      this.ClusterID.valueChanges.subscribe((val) => {
        if (val == null)
          this.ClusterID.setValue(0);
      });
    }


    GetData() {
        this.http.getJSON('api/cluster/Getcluster').subscribe((d: any) => {
            this.cluster = new MatTableDataSource(d);
            this.cluster.paginator = this.paginator;
            this.cluster.sort = this.sort;
        }, error => this.error = error // error path
        );
    }
    ngOnInit() {
        this.http.setTitle("cluster");
        this.title = "Add New ";
        this.savebtn = "Save ";
        this.GetData();
    }

    Submit() {
        if (!this.clusterform.valid)
            return;
      this.working = true;
      if (this.ClusterID.value == null) this.ClusterID.setValue(0);
        this.http.postJSON('api/cluster/Save', this.clusterform.value)
            .subscribe((d: any) => {
                this.cluster = new MatTableDataSource(d);
                this.cluster.paginator = this.paginator;
                this.cluster.sort = this.sort;
                this.http.openSnackBar(this.ClusterID.value > 0 ? environment.UpdateMSG.replace("${Name}", this.EnText) : environment.SaveMSG.replace("${Name}", this.EnText));
                this.reset();
                this.ClusterID.setValue(0);

                this.title = "Add New ";
                this.working = false;
                this.savebtn = "Save ";
            },
            (error) => {
                this.error = error.error;
                this.http.ShowError(error.error);
                
              });
    }
    reset() {
        this.clusterform.reset();
        this.title = "Add New ";
        this.savebtn = "Save ";
    }
    toggleEdit(row) {
        this.ClusterID.setValue(row.ClusterID);
        this.ClusterName.setValue(row.ClusterName);
        this.InActive.setValue(row.InActive);

        this.title = "Update ";
        this.savebtn = "Save ";
  };
  ChkName() {
    if (this.ClusterName.value) {
      this.http.postJSON('api/cluster/CheckName', { ClusterID: this.ClusterID.value, ClusterName: this.ClusterName.value }).subscribe((d: any) => {
        //this.users = d;
        if (d)
          this.ClusterName.setErrors(null);
        else {
          this.ClusterName.setErrors({ 'duplicate': true });
          this.http.ShowWarning(this.ClusterName.value + ' already exists.');
        }
      }, error => this.error = error // error path
      );
    }
  };
    toggleActive(row) {
        this.http.confirm().subscribe(result => {
            if (result) {
                this.http.postJSON('api/cluster/ActiveInActive', { 'ClusterID': row.ClusterID, 'InActive': row.InActive })
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
        this.cluster.filter = filterValue;
    }

    get ClusterID() { return this.clusterform.get("ClusterID"); }
    get ClusterName() { return this.clusterform.get("ClusterName"); }
    get InActive() { return this.clusterform.get("InActive"); }
}
export class clusterModel extends ModelBase {
    ClusterID: number;
    ClusterName: string;
}
