import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from '../../../environments/environment';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { CommonSrvService } from '../../common-srv.service';

import { ModelBase } from '../../shared/ModelBase';
import { UserRightsModel } from '../../master-data/users/users.component';
import * as moment from 'moment';

@Component({
  selector: 'app-year-wise-inflation-rate',
  templateUrl: './year-wise-inflation-rate.component.html',
  styleUrls: ['./year-wise-inflation-rate.component.scss']
})
export class InflationRateComponent implements OnInit {
  inflationRateform: FormGroup;
  title: string; savebtn: string;
  displayedColumns = ['FinancialYear', 'Month', 'Inflation', 'InActive', "Action"];
    yearwiseinflationrate: MatTableDataSource<any>;

    years = []

  formrights: UserRightsModel;
  EnText: string = "Inflation Rate";
  error: String;
  query = {
    order: 'IRID',
    limit: 5,
    page: 1
  };
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  working: boolean;
  constructor(private fb: FormBuilder, private ComSrv: CommonSrvService) {
    this.inflationRateform = this.fb.group({
      IRID: 0,
      FinancialYear: ["", Validators.required],
      Month: ["", Validators.required],
      Inflation: ["", Validators.required],
      InActive: ""
    }, { updateOn: "blur" });
    this.yearwiseinflationrate = new MatTableDataSource([]);
    this.formrights = ComSrv.getFormRights();
  }


  GetData() {
    this.ComSrv.getJSON('api/YearWiseInflationRate/GetYearWiseInflationRate').subscribe((d: any) => {
      this.yearwiseinflationrate = new MatTableDataSource(d);
      this.yearwiseinflationrate.paginator = this.paginator;
        this.yearwiseinflationrate.sort = this.sort;

    }, error => this.error = error // error path
    );
    }

    getFinancialYears() {

        let today = new Date().getFullYear();
        console.log(today);
        let min = today - 20
        let max = today + 20
        for (let i = min; i < max; i++) {
            let ai = i + 1;
            let yearvalue = i+"-"+ai;
            this.years.push(yearvalue);
        }
    }

  ngOnInit() {
    this.ComSrv.setTitle("Year Wise Inflation Rate");
    this.title = "Add New ";
    this.savebtn = "Save ";
      this.GetData();
      this.getFinancialYears();
  }

  Submit() {
    if (!this.inflationRateform.valid)
      return;
    this.working = true;
    this.ComSrv.postJSON('api/YearWiseInflationRate/Save', this.inflationRateform.value)
      .subscribe((d: any) => {
        this.yearwiseinflationrate = new MatTableDataSource(d);
        this.yearwiseinflationrate.paginator = this.paginator;
        this.yearwiseinflationrate.sort = this.sort;
        this.ComSrv.openSnackBar(this.IRID.value > 0 ? environment.UpdateMSG.replace("${Name}", this.EnText) : environment.SaveMSG.replace("${Name}", this.EnText));
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
    this.inflationRateform.reset();
    this.IRID.setValue(0);
    this.title = "Add New ";
    this.savebtn = "Save ";
  }
  toggleEdit(row) {
    this.IRID.setValue(row.IRID);
    this.FinancialYear.setValue(row.FinancialYear);
    this.Month.setValue(row.Month);
    this.Inflation.setValue(row.Inflation);
    this.InActive.setValue(row.InActive);

    this.title = "Update ";
    this.savebtn = "Save ";
  }
  toggleActive(row) {
    this.ComSrv.confirm().subscribe(result => {
      if (result) {
        this.ComSrv.postJSON('api/YearWiseInflationRate/ActiveInActive', { 'IRID': row.IRID, 'InActive': row.InActive })
          .subscribe((d: any) => {
            this.ComSrv.openSnackBar(row.InActive == true ? environment.InActiveMSG.replace("${Name}", this.EnText) : environment.ActiveMSG.replace("${Name}", this.EnText));
            // this.yearwiseinflationrate =new MatTableDataSource(d);
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
    this.yearwiseinflationrate.filter = filterValue;
  }

  get IRID() { return this.inflationRateform.get("IRID"); }
  get FinancialYear() { return this.inflationRateform.get("FinancialYear"); }
  get Month() { return this.inflationRateform.get("Month"); }
  get Inflation() { return this.inflationRateform.get("Inflation"); }
  get InActive() { return this.inflationRateform.get("InActive"); }
}
export class YearWiseInflationRateModel extends ModelBase {
  IRID: number;
  FinancialYear: string;
  Month: string;
  Inflation: number;

}
