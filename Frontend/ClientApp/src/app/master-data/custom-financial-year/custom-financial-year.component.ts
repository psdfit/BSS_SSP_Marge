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
  selector: 'app-custom-financial-year',
  templateUrl: './custom-financial-year.component.html',
  styleUrls: ['./custom-financial-year.component.scss']
})
export class CustomFinancialYearComponent implements OnInit {
  csyform: FormGroup;
  title: string; savebtn: string;
  displayedColumns = ['OrgName', 'FromDate', 'ToDate', "InActive", "Action"];
  csy: MatTableDataSource<any>;
  orgs: any;
  formrights: UserRightsModel;
  EnText: string = "Financial Year";
  error: String;
  query = {
    order: 'Id',
    limit: 5,
    page: 1
  };

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  working: boolean;

  constructor(private fb: FormBuilder, private http: CommonSrvService) {
    this.csyform = this.fb.group({
      Id: 0,
      FromDate: ["", Validators.required],
      ToDate: ["", Validators.required],
      OrgID: ["", Validators.required],
      InActive: "",
    }, { updateOn: "blur" });
    this.csy = new MatTableDataSource([]);
    this.formrights = http.getFormRights();
    this.Id.valueChanges.subscribe((val) => {
      if (val == null)
        this.Id.setValue(0);
    });
  }

  ngOnInit() {
    this.http.setTitle("Financial Years");
    this.title = "Add New ";
    this.savebtn = "Save ";
    this.GetData();
  }

  GetData() {
    this.http.getJSON('api/CustomFinancialYear/GetCustomFinancialYear').subscribe((d: any) => {
      this.csy = new MatTableDataSource(d[0])
      this.orgs = d[1];

      this.csy.paginator = this.paginator;
      this.csy.sort = this.sort;
    }, error => this.error = error // error path
    );
  }

  Submit() {
    if (!this.csyform.valid)
      return;
    this.working = true;
    this.http.postJSON('api/CustomFinancialYear/Save', this.csyform.value)
      .subscribe((d: any) => {
        this.csy = new MatTableDataSource(d);
        this.csy.paginator = this.paginator;
        this.csy.sort = this.sort;
        this.http.openSnackBar(this.Id.value > 0 ? environment.UpdateMSG.replace("${Name}", this.EnText) : environment.SaveMSG.replace("${Name}", this.EnText));
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
    this.csyform.reset();
    this.title = "Add New ";
    this.savebtn = "Save ";
  }

  toggleEdit(row) {
    this.Id.setValue(row.Id);
    this.FromDate.setValue(row.FromDate);
    this.ToDate.setValue(row.ToDate);
    this.InActive.setValue(row.InActive);
    this.OrgID.setValue(row.OrgID);

    this.title = "Update ";
    this.savebtn = "Save ";
  }
  toggleActive(row) {
    this.http.confirm().subscribe(result => {
      if (result) {
        this.http.postJSON('api/CustomFinancialYear/ActiveInActive', { 'Id': row.Id, 'InActive': row.InActive })
          .subscribe((d: any) => {
            this.http.openSnackBar(row.InActive == true ? environment.InActiveMSG.replace("${Name}", this.EnText) : environment.ActiveMSG.replace("${Name}", this.EnText));
            // this.customfinancialyears =new MatTableDataSource(d);
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
    this.csy.filter = filterValue;
  }

  get Id() { return this.csyform.get("Id"); }
  get FromDate() { return this.csyform.get("FromDate"); }
  get ToDate() { return this.csyform.get("ToDate"); }
  get InActive() { return this.csyform.get("InActive"); }
  get OrgID() { return this.csyform.get("OrgID"); }

}

export class DistrictModel extends ModelBase {
  Id: number;
  OrgID: number;
  FromDate: Date;
  ToDate: Date;
}
