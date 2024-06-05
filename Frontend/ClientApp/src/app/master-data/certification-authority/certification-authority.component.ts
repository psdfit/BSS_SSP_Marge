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
  selector: 'app-certification-authority',
  templateUrl: './certification-authority.component.html',
  styleUrls: ['./certification-authority.component.scss']
})
export class CertificationAuthorityComponent implements OnInit {
  certificationauthorityform: FormGroup;
  title: string; savebtn: string;
  displayedColumns = ['CertAuthName', 'CertificationCategoryID', 'InActive', "Action"];
  certificationauthority: MatTableDataSource<any>;
  CertificationCategory = [];
  formrights: UserRightsModel;
  EnText: string = "Certification Authority";
  error: String;
  query = {
    order: 'CertAuthID',
    limit: 5,
    page: 1
  };
  @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: false }) sort: MatSort;
  working: boolean;
  constructor(private fb: FormBuilder, private ComSrv: CommonSrvService) {
    this.certificationauthorityform = this.fb.group({
      CertAuthID: 0,
      CertAuthName: ["", Validators.required],
      CertificationCategoryID: ["", Validators.required],
      InActive: ""
    }, { updateOn: "blur" });
    this.certificationauthority = new MatTableDataSource([]);
    this.formrights = ComSrv.getFormRights();
  }


  GetData() {
    this.ComSrv.getJSON('api/CertificationAuthority/GetCertificationAuthority').subscribe((d: any) => {
      this.certificationauthority = new MatTableDataSource(d[0])
      this.CertificationCategory = d[1];

      this.certificationauthority.paginator = this.paginator;
      this.certificationauthority.sort = this.sort;
    }, error => this.error = error // error path
    );
  }
  ngOnInit() {
    this.ComSrv.setTitle("Certification Authority");
    this.title = "Add New ";
    this.savebtn = "Save ";
    this.GetData();
  }

  Submit(myform: FormGroupDirective) {
    if (!this.certificationauthorityform.valid)
      return;
    this.working = true;
    this.ComSrv.postJSON('api/CertificationAuthority/Save', this.certificationauthorityform.value)
      .subscribe((d: any) => {
        this.certificationauthority = new MatTableDataSource(d);
        this.certificationauthority.paginator = this.paginator;
        this.certificationauthority.sort = this.sort;
        this.ComSrv.openSnackBar(this.CertAuthID.value > 0 ? environment.UpdateMSG.replace("${Name}", this.EnText) : environment.SaveMSG.replace("${Name}", this.EnText));
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
    this.certificationauthorityform.reset();
    //myform.resetFrom();
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
            // this.certificationauthority =new MatTableDataSource(d);
          },
          (error) => {
            this.error = error.error;
            this.ComSrv.ShowError(error.error);
            row.InActive = !row.InActive;
          });
			  
  }
      else {
  row.InActive = row.InActive;
}
    });
        }
applyFilter(filterValue: string) {
  filterValue = filterValue.trim(); // Remove whitespace
  filterValue = filterValue.toLowerCase(); // Datasource defaults to lowercase matches
  this.certificationauthority.filter = filterValue;
}

get CertAuthID() { return this.certificationauthorityform.get("CertAuthID"); }
get CertAuthName() { return this.certificationauthorityform.get("CertAuthName"); }
get CertificationCategoryID() { return this.certificationauthorityform.get("CertificationCategoryID"); }
get InActive() { return this.certificationauthorityform.get("InActive"); }

}
export class CertificationAuthorityModel extends ModelBase {
  CertAuthID: number;
  CertAuthName: string;
  CertificationCategoryID: number;

}
