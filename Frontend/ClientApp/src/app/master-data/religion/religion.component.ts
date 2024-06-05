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
  selector: 'app-religion',
  templateUrl: './religion.component.html',
  styleUrls: ['./religion.component.scss']
})
export class ReligionComponent implements OnInit {
  religionform: FormGroup;
  title: string; savebtn: string;
  displayedColumns = ['ReligionName', 'InActive', "Action"];
  religion: MatTableDataSource<any>;

  formrights: UserRightsModel;
  EnText: string = "Religion";
  error: String;
  query = {
    order: 'ReligionID',
    limit: 5,
    page: 1
  };
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  working: boolean;
  constructor(private fb: FormBuilder, private ComSrv: CommonSrvService) {
    this.religionform = this.fb.group({
      ReligionID: 0,
      ReligionName: ["", Validators.required],
      InActive: ""
    }, { updateOn: "blur" });
    this.religion = new MatTableDataSource([]);
    this.formrights = ComSrv.getFormRights();
    this.ReligionID.valueChanges.subscribe((val) => {
      if (val == null)
        this.ReligionID.setValue(0);
    });
  }

  GetData() {
    this.ComSrv.getJSON('api/Religion/GetReligion').subscribe((d: any) => {
      this.religion = new MatTableDataSource(d);
      this.religion.paginator = this.paginator;
      this.religion.sort = this.sort;
    }, error => this.error = error // error path
    );
  }
  ngOnInit() {
    this.ComSrv.setTitle("Religion");
    this.title = "Add New ";
    this.savebtn = "Save ";
    this.GetData();
  }

  Submit() {
    if (!this.religionform.valid)
      return;
    this.working = true;
    this.ComSrv.postJSON('api/Religion/Save', this.religionform.value)
      .subscribe((d: any) => {
        this.religion = new MatTableDataSource(d);
        this.religion.paginator = this.paginator;
        this.religion.sort = this.sort;
        this.ComSrv.openSnackBar(this.ReligionID.value > 0 ? environment.UpdateMSG.replace("${Name}", this.EnText) : environment.SaveMSG.replace("${Name}", this.EnText));
          this.reset();
          this.ReligionID.setValue(0);
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
    this.religionform.reset();
    this.ReligionID.setValue(0);
    this.title = "Add New ";
    this.savebtn = "Save ";
  }
  toggleEdit(row) {
    this.ReligionID.setValue(row.ReligionID);
    this.ReligionName.setValue(row.ReligionName);
    this.InActive.setValue(row.InActive);

    this.title = "Update ";
    this.savebtn = "Save ";
  }
  toggleActive(row) {
    this.ComSrv.confirm().subscribe(result => {
      if (result) {
        this.ComSrv.postJSON('api/Religion/ActiveInActive', { 'ReligionID': row.ReligionID, 'InActive': row.InActive })
          .subscribe((d: any) => {
            this.ComSrv.openSnackBar(row.InActive == true ? environment.InActiveMSG.replace("${Name}", this.EnText) : environment.ActiveMSG.replace("${Name}", this.EnText));
            // this.religion =new MatTableDataSource(d);
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
    this.religion.filter = filterValue;
  }

  get ReligionID() { return this.religionform.get("ReligionID"); }
  get ReligionName() { return this.religionform.get("ReligionName"); }
  get InActive() { return this.religionform.get("InActive"); }
}
export class ReligionModel extends ModelBase {
  ReligionID: number;
  ReligionName: string;
}
