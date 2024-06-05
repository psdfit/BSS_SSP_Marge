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
  selector: 'app-tehsil',
  templateUrl: './tehsil.component.html',
  styleUrls: ['./tehsil.component.scss']
})
export class TehsilComponent implements OnInit {
  tehsilform: FormGroup;
  title: string; savebtn: string;
  displayedColumns = ['TehsilName', 'DistrictID', 'InActive', "Action"];
  tehsil: MatTableDataSource<any>;
  District: any;
  formrights: UserRightsModel;
  EnText: string = "Tehsil";
  error: String;
  query = {
    order: 'TehsilID',
    limit: 5,
    page: 1
  };
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  working: boolean;
  constructor(private fb: FormBuilder, private http: CommonSrvService) {
    this.tehsilform = this.fb.group({
      TehsilID: 0,
      TehsilName: ["", Validators.required],
      DistrictID: ["", Validators.required],
      InActive: ""
    }, { updateOn: "blur" });
    this.tehsil = new MatTableDataSource([]);
    this.formrights = http.getFormRights();
    this.TehsilID.valueChanges.subscribe((val) => {
      if (val == null)
        this.TehsilID.setValue(0);
    });
  }


  GetData() {
    this.http.getJSON('api/Tehsil/GetTehsil').subscribe((d: any) => {
      this.tehsil = new MatTableDataSource(d[0])
      this.District = d[1];

      this.tehsil.paginator = this.paginator;
      this.tehsil.sort = this.sort;
    }, error => this.error = error // error path
    );
  }
  ngOnInit() {
    this.http.setTitle("Tehsil");
    this.title = "Add New ";
    this.savebtn = "Save ";
    this.GetData();
    }

    ChkTehsilName() {
        if (this.TehsilName.value) {
            this.http.postJSON('api/Tehsil/CheckTehsilName', { TehsilID: this.TehsilID.value, TehsilName: this.TehsilName.value }).subscribe((d: any) => {
                //this.users = d;
                if (d)
                    this.TehsilName.setErrors(null);
                else
                    this.TehsilName.setErrors({ 'duplicate': true });
            }, error => this.error = error // error path
            );
        }
    };


  Submit(nform:FormGroupDirective) {
    if (!this.tehsilform.valid)
      return;
    this.working = true;
    if (this.TehsilID.value == null) this.TehsilID.setValue(0);
    this.http.postJSON('api/Tehsil/Save', this.tehsilform.value)
      .subscribe((d: any) => {
        this.tehsil = new MatTableDataSource(d);
        this.tehsil.paginator = this.paginator;
        this.tehsil.sort = this.sort;
        this.http.openSnackBar(this.TehsilID.value > 0 ? environment.UpdateMSG.replace("${Name}", this.EnText) : environment.SaveMSG.replace("${Name}", this.EnText));
          this.reset(nform);
          this.TehsilID.setValue(0);
          this.working = false;
        this.title = "Add New ";
        this.savebtn = "Save ";
      },
      (error) => {
        this.error = error.error;
        this.http.ShowError(error.error);
        
      });
  }
  reset(nform: FormGroupDirective) {
    nform.resetForm();
    this.tehsilform.reset();
    this.title = "Add New ";
    this.savebtn = "Save ";
  }
  toggleEdit(row) {
    this.TehsilID.setValue(row.TehsilID);
    this.TehsilName.setValue(row.TehsilName);
    this.DistrictID.setValue(row.DistrictID);
    this.InActive.setValue(row.InActive);

    this.title = "Update ";
    this.savebtn = "Save ";
  }
  toggleActive(row) {
    this.http.confirm().subscribe(result => {
      if (result) {
        this.http.postJSON('api/Tehsil/ActiveInActive', { 'TehsilID': row.TehsilID, 'InActive': row.InActive })
          .subscribe((d: any) => {
            this.http.openSnackBar(row.InActive == true ? environment.InActiveMSG.replace("${Name}", this.EnText) : environment.ActiveMSG.replace("${Name}", this.EnText));
            // this.tehsil =new MatTableDataSource(d);
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
    this.tehsil.filter = filterValue;
  }

  get TehsilID() { return this.tehsilform.get("TehsilID"); }
  get TehsilName() { return this.tehsilform.get("TehsilName"); }
  get DistrictID() { return this.tehsilform.get("DistrictID"); }
  get InActive() {
    return this.tehsilform.get("InActive");
  }
}
export class TehsilModel extends ModelBase {
  TehsilID: number;
  TehsilName: string;
  DistrictID: number;

}
