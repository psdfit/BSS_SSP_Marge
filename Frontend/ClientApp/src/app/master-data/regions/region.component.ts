/* **** Aamer Rehman Malik *****/
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
  selector: 'app-region',
  templateUrl: './region.component.html',
  styleUrls: ['./region.component.scss']
})
export class RegionComponent implements OnInit {
  regionform: FormGroup;
  title: string; savebtn: string;
  displayedColumns = ['RegionName', 'InActive', "Action"];
  region: MatTableDataSource<any>;

  formrights: UserRightsModel;
  EnText: string = "Region Name";
  error: String;
  query = {
    order: 'RegionID',
    limit: 5,
    page: 1
  };
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  working: boolean;
  constructor(private fb: FormBuilder, private http: CommonSrvService) {
    this.regionform = this.fb.group({
      RegionID: 0,
      RegionName: ["", Validators.required],
      InActive: ""
    }, { updateOn: "blur" });
    this.region = new MatTableDataSource([]);
    this.formrights = http.getFormRights();
    this.RegionID.valueChanges.subscribe((val) => {
      if (val == null)
        this.RegionID.setValue(0);
    });
  }

  GetData() {
    this.http.getJSON('api/Region/GetRegion').subscribe((d: any) => {
      this.region = new MatTableDataSource(d);
      this.region.paginator = this.paginator;
      this.region.sort = this.sort;
    }, error => this.error = error // error path
    );
  }
  ngOnInit() {
    this.http.setTitle("Region");
    this.title = "Add New ";
    this.savebtn = "Save ";
    this.GetData();
  }

  Submit() {
    if (!this.regionform.valid)
      return;
    this.working = true;
    if (this.RegionID.value == null) this.RegionID.setValue(0);
    this.http.postJSON('api/Region/Save', this.regionform.value)
      .subscribe((d: any) => {
        this.region = new MatTableDataSource(d);
        this.region.paginator = this.paginator;
        this.region.sort = this.sort;
        this.http.openSnackBar(this.RegionID.value > 0 ? environment.UpdateMSG.replace("${Name}", this.EnText) : environment.SaveMSG.replace("${Name}", this.EnText));
        this.reset();
        this.RegionID.setValue(0);
        this.working = false;
        this.title = "Add New ";
        this.savebtn = "Save ";
      },
      (error) => {
        this.error = error.error;
        this.http.ShowError(error.error);
        
      });
  }
  reset() {
    this.regionform.reset();
    this.title = "Add New ";
    this.savebtn = "Save ";
  }
  toggleEdit(row) {
    this.RegionID.setValue(row.RegionID);
    this.RegionName.setValue(row.RegionName);
    this.InActive.setValue(row.InActive);

    this.title = "Update ";
    this.savebtn = "Save ";
  }
  toggleActive(row) {
    this.http.confirm().subscribe(result => {
      if (result) {
        this.http.postJSON('api/Region/ActiveInActive', { 'RegionID': row.RegionID, 'InActive': row.InActive })
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
    this.region.filter = filterValue;
  }
  ChkName() {
    if (this.RegionName.value) {
      this.http.postJSON('api/Region/CheckName', { RegionID: this.RegionID.value, RegionName: this.RegionName.value }).subscribe((d: any) => {
        //this.users = d;
        if (d)
          this.RegionName.setErrors(null);
        else {
          this.RegionName.setErrors({ 'duplicate': true });
          this.http.ShowWarning(this.RegionName.value + ' already exists.');
        }
      }, error => this.error = error // error path
      );
    }
  };
  get RegionID() { return this.regionform.get("RegionID"); }
  get RegionName() { return this.regionform.get("RegionName"); }
  get InActive() { return this.regionform.get("InActive"); }
}
export class regionModel extends ModelBase {
  RegionID: number;
  RegionName: string;
}
