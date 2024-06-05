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
  selector: 'app-testing-agency',
  templateUrl: './testing-agency.component.html',
  styleUrls: ['./testing-agency.component.scss']
})
export class TestingAgencyComponent implements OnInit {
  testingagencyform: FormGroup;
  title: string; savebtn: string;
  displayedColumns = ['CertAuthName', 'CertificationCategoryID', 'InActive', "Action"];
  testingagency: MatTableDataSource<any>;
  CertificationCategory = [];
  formrights: UserRightsModel;
  EnText: string = "Testing Agency";
  error: String;
  query = {
    order: 'TestingAgencyID',
    limit: 5,
    page: 1
  };
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  working: boolean;
  constructor(private fb: FormBuilder, private ComSrv: CommonSrvService) {
    this.testingagencyform = this.fb.group({
      CertAuthID: 0,
      CertAuthName: ["", Validators.required],
      CertificationCategoryID: ["", Validators.required],
      InActive: ""
    }, { updateOn: "blur" });
    this.testingagency = new MatTableDataSource([]);
    this.formrights = ComSrv.getFormRights();
  }


  GetData() {
    this.ComSrv.getJSON('api/CertificationAuthority/GetCertificationAuthority').subscribe((d: any) => {
      this.testingagency = new MatTableDataSource(d[0])
      this.CertificationCategory = d[1];

      this.testingagency.paginator = this.paginator;
      this.testingagency.sort = this.sort;
    }, error => this.error = error // error path
    );
  }
  ngOnInit() {
    this.ComSrv.setTitle("TestingAgency");
    this.title = "Add New ";
    this.savebtn = "Save ";
    this.GetData();
  }

  Submit() {
    if (!this.testingagencyform.valid)
      return;
    this.working = true;
    this.ComSrv.postJSON('api/CertificationAuthority/Save', this.testingagencyform.value)
      .subscribe((d: any) => {
        this.testingagency = new MatTableDataSource(d);
        this.testingagency.paginator = this.paginator;
        this.testingagency.sort = this.sort;
        this.ComSrv.openSnackBar(this.CertAuthID.value > 0 ? environment.UpdateMSG.replace("${Name}", this.EnText) : environment.SaveMSG.replace("${Name}", this.EnText));
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
    this.testingagencyform.reset();
    this.CertAuthID.setValue(0);
    this.title = "Add New ";
    this.savebtn = "Save ";
  }
  toggleEdit(row) {
    this.CertAuthID.setValue(row.CertAuthID);
    this.CertAuthName.setValue(row.CertAuthName);
    this.CertificationCategoryID.setValue(row.CertificationCategoryID);
    this.InActive.setValue(row.InActive);

    this.title = "Update ";
    this.savebtn = "Save ";
  }
  toggleActive(row) {
    this.ComSrv.confirm().subscribe(result => {
      if (result) {
        this.ComSrv.postJSON('api/CertificationAuthority/ActiveInActive', { 'CertAuthID': row.CertAuthID, 'InActive': row.InActive })
          .subscribe((d: any) => {
            this.ComSrv.openSnackBar(row.InActive == true ? environment.InActiveMSG.replace("${Name}", this.EnText) : environment.ActiveMSG.replace("${Name}", this.EnText));
            // this.testingagency =new MatTableDataSource(d);
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
    this.testingagency.filter = filterValue;
  }

  get CertAuthID() { return this.testingagencyform.get("CertAuthID"); }
  get CertAuthName() { return this.testingagencyform.get("CertAuthName"); }
  get CertificationCategoryID() { return this.testingagencyform.get("CertificationCategoryID"); }
  get InActive() { return this.testingagencyform.get("InActive"); }
}
export class TestingAgencyModel extends ModelBase {
  CertAuthID: number;
  CertAuthName: string;
  CertificationCategoryID: number;

}
