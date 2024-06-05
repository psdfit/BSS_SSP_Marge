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
  selector: 'app-orgnizations',
  templateUrl: './orgnizations.component.html',
  styleUrls: ['./orgnizations.component.scss']
})
export class OrgnizationsComponent implements OnInit {
  organizationform: FormGroup;
  title: string; savebtn: string;
  displayedColumns = ['OName', 'InActive', "Action"];
  organization: MatTableDataSource<any>;

  formrights: UserRightsModel;
  EnText: string = "Organization";
  error: String;
  query = {
    order: 'OID',
    limit: 5,
    page: 1
  };
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  working: boolean;
  constructor(private fb: FormBuilder, private http: CommonSrvService) {
    this.organizationform = this.fb.group({
      OID: 0,
      OName: ["", Validators.required],
      InActive: ""
    }, { updateOn: "blur" });
    this.organization = new MatTableDataSource([]);
    this.formrights = http.getFormRights();
  }

  GetData() {
    this.http.getJSON('api/Organization/GetOrganization').subscribe((d: any) => {
      this.organization = new MatTableDataSource(d);
      this.organization.paginator = this.paginator;
      this.organization.sort = this.sort;
    }, error => this.error = error // error path
    );
  }
  ngOnInit() {
    this.http.setTitle("Organization");
    this.title = "Add New ";
    this.savebtn = "Save ";
    this.GetData();
  }

  Submit() {
    if (!this.organizationform.valid)
      return;
    this.working = true;
    this.http.postJSON('api/Organization/Save', this.organizationform.value)
      .subscribe((d: any) => {
        this.organization = new MatTableDataSource(d);
        this.organization.paginator = this.paginator;
        this.organization.sort = this.sort;
        this.http.openSnackBar(this.OID.value > 0 ? environment.UpdateMSG.replace("${Name}", this.EnText) : environment.SaveMSG.replace("${Name}", this.EnText));
        this.reset();
        this.title = "Add New ";
        this.savebtn = "Save ";
        this.working = false;
      },
      (error) => {
        this.error = error.error;
        this.http.ShowError(error.error);
        
      });
  }
  reset() {
    this.organizationform.reset();
    this.OID.setValue(0);
    this.title = "Add New ";
    this.savebtn = "Save ";
  }
  toggleEdit(row) {
    this.OID.setValue(row.OID);
    this.OName.setValue(row.OName);
    this.InActive.setValue(row.InActive);

    this.title = "Update ";
    this.savebtn = "Save ";
  }
  toggleActive(row) {
    this.http.confirm().subscribe(result => {
      if (result) {
        this.http.postJSON('api/Organization/ActiveInActive', { 'OID': row.OID, 'InActive': row.InActive })
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
    this.organization.filter = filterValue;
  }

  get OID() { return this.organizationform.get("OID"); }
  get OName() { return this.organizationform.get("OName"); }
  get InActive() { return this.organizationform.get("InActive"); }
}
export class organizationModel extends ModelBase {
  OID: number;
  OName: string;
}
