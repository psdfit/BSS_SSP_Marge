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
  selector: 'app-education-type',
  templateUrl: './education-type.component.html',
  styleUrls: ['./education-type.component.scss']
})
export class EducationTypeComponent implements OnInit {
  educationtypesform: FormGroup;
  title: string; savebtn: string;
  displayedColumns = ['Education', 'InActive', "Action"];
  educationtypes: MatTableDataSource<any>;

  formrights: UserRightsModel;
  EnText: string = "EducationTypes";
  error: String;
  query = {
      order: 'EducationTypeID',
    limit: 5,
    page: 1
  };
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  working: boolean;
  constructor(private fb: FormBuilder, private ComSrv: CommonSrvService) {
    this.educationtypesform = this.fb.group({
        EducationTypeID: 0,
      Education: ["", Validators.required],
      InActive: ""
    }, { updateOn: "blur" });
    this.educationtypes = new MatTableDataSource([]);
    this.formrights = ComSrv.getFormRights();
  }


  GetData() {
    this.ComSrv.getJSON('api/EducationTypes/GetEducationTypes').subscribe((d: any) => {
      this.educationtypes = new MatTableDataSource(d);
      this.educationtypes.paginator = this.paginator;
      this.educationtypes.sort = this.sort;
    }, error => this.error = error // error path
    );
  }
  ngOnInit() {
    this.ComSrv.setTitle("Education Types");
    this.title = "Add New ";
    this.savebtn = "Save ";
    this.GetData();
  }

  Submit() {
    if (!this.educationtypesform.valid)
      return;
    this.working = true;
    this.ComSrv.postJSON('api/EducationTypes/Save', this.educationtypesform.value)
      .subscribe((d: any) => {
        this.educationtypes = new MatTableDataSource(d);
        this.educationtypes.paginator = this.paginator;
        this.educationtypes.sort = this.sort;
          this.ComSrv.openSnackBar(this.EducationTypeID.value > 0 ? environment.UpdateMSG.replace("${Name}", this.EnText) : environment.SaveMSG.replace("${Name}", this.EnText));
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
    this.educationtypesform.reset();
      this.EducationTypeID.setValue(0);
    this.title = "Add New ";
    this.savebtn = "Save ";
  }
  toggleEdit(row) {
      this.EducationTypeID.setValue(row.EducationTypeID);
    this.Education.setValue(row.Education);
    this.InActive.setValue(row.InActive);

    this.title = "Update ";
    this.savebtn = "Save ";
  }
  toggleActive(row) {
    this.ComSrv.confirm().subscribe(result => {
      if (result) {
          this.ComSrv.postJSON('api/EducationTypes/ActiveInActive', { 'EducationTypeID': row.EducationTypeID, 'InActive': row.InActive })
          .subscribe((d: any) => {
            this.ComSrv.openSnackBar(row.InActive == true ? environment.InActiveMSG.replace("${Name}", this.EnText) : environment.ActiveMSG.replace("${Name}", this.EnText));
            // this.educationtypes =new MatTableDataSource(d);
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
    this.educationtypes.filter = filterValue;
  }

    get EducationTypeID() { return this.educationtypesform.get("EducationTypeID"); }
  get Education() { return this.educationtypesform.get("Education"); }
  get InActive() { return this.educationtypesform.get("InActive"); }
}
export class EducationTypesModel extends ModelBase {
    EducationTypeID: number;
  Education: string;

}
