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
  selector: 'app-classStatus',
  templateUrl: './classStatus.component.html',
  styleUrls: ['./classStatus.component.scss']
})
export class ClassStatusComponent implements OnInit {
  classstatusform: FormGroup;
  title: string; savebtn: string;
  displayedColumns = ['ClassStatusName', 'InActive'];
  //displayedColumns = ['ClassStatusName', 'InActive', "Action"];
  classstatustype: MatTableDataSource<any>;

  formrights: UserRightsModel;
  EnText: string = "Class Status";
  error: String;
  query = {
    order: 'ClassStatusID',
    limit: 5,
    page: 1
  };
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  working: boolean;
  constructor(private fb: FormBuilder, private ComSrv: CommonSrvService) {
    this.classstatusform = this.fb.group({
      ClassStatusID: 0,      ClassStatusName: ["", Validators.required],      InActive: ""
    }, { updateOn: "blur" });
    this.classstatustype = new MatTableDataSource([]);
    this.formrights = ComSrv.getFormRights();
    this.ClassStatusID.valueChanges.subscribe((val) => {
      if (val == null)
        this.ClassStatusID.setValue(0);
    });
  }

  GetData() {
    this.ComSrv.getJSON('api/ClassStatus/GetClassStatus').subscribe((d: any) => {
      this.classstatustype = new MatTableDataSource(d);
      this.classstatustype.paginator = this.paginator;
      this.classstatustype.sort = this.sort;
    }, error => this.error = error // error path
    );
  }
  ngOnInit() {
    this.ComSrv.setTitle("Class Status");
    this.title = "Add New ";
    this.savebtn = "Save ";
    this.GetData();
  }
  Submit() {
    if (!this.classstatusform.valid)
      return;
    this.working = true;
    this.ComSrv.postJSON('api/ClassStatus/Save', this.classstatusform.value)
      .subscribe((d: any) => {
        this.classstatustype = new MatTableDataSource(d);
        this.classstatustype.paginator = this.paginator;
        this.classstatustype.sort = this.sort;
        this.ComSrv.openSnackBar(this.ClassStatusID.value > 0 ? environment.UpdateMSG.replace("${Name}", this.EnText) : environment.SaveMSG.replace("${Name}", this.EnText));
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
    this.classstatusform.reset();
    this.ClassStatusID.setValue(0);
    this.title = "Add New ";
    this.savebtn = "Save ";
  }
  toggleEdit(row) {
    this.ClassStatusID.setValue(row.ClassStatusID);    this.ClassStatusName.setValue(row.ClassStatusName);    this.InActive.setValue(row.InActive);
    this.title = "Update ";
    this.savebtn = "Save ";
  }
  toggleActive(row) {
    this.ComSrv.confirm().subscribe(result => {
      if (result) {
        this.ComSrv.postJSON('api/ClassStatus/ActiveInActive', { 'ClassStatusID': row.ClassStatusID, 'InActive': row.InActive })
          .subscribe((d: any) => {
            this.ComSrv.openSnackBar(row.InActive == true ? environment.InActiveMSG.replace("${Name}", this.EnText) : environment.ActiveMSG.replace("${Name}", this.EnText));
            // this.classstatustype =new MatTableDataSource(d);
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
    this.classstatustype.filter = filterValue;
  }

  get ClassStatusID() { return this.classstatusform.get("ClassStatusID"); }  get ClassStatusName() { return this.classstatusform.get("ClassStatusName"); }  get InActive() { return this.classstatusform.get("InActive"); }
}
export class TraineeStatusTypesModel extends ModelBase {
  StatusID: number;  StatusName: string;
}
