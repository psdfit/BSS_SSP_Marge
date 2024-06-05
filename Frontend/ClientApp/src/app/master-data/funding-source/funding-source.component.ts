import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from '../../../environments/environment';
import { FormGroup, FormBuilder, Validators, FormControl, NgForm } from '@angular/forms';
import { CommonSrvService } from '../../common-srv.service';
import { ModelBase } from '../../shared/ModelBase';
import { UserRightsModel } from '../../master-data/users/users.component';

@Component({
  selector: 'app-funding-source',
  templateUrl: './funding-source.component.html',
  styleUrls: ['./funding-source.component.scss']
})
export class FundingSourceComponent implements OnInit {
  fundingsourceform: FormGroup;
  title: string; savebtn: string;
  displayedColumns = ['FundingSourceName', 'InActive', "Action"];
  fundingsource: MatTableDataSource<any>;

  formrights: UserRightsModel;
  EnText: string = "Funding Source";
  error: String;
  query = {
    order: 'FundingSourceID',
    limit: 5,
    page: 1
  };
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild('NForm') NForm: NgForm;
  working: boolean;
  constructor(private fb: FormBuilder, private ComSrv: CommonSrvService) {
    this.CreateForm();
    this.fundingsource = new MatTableDataSource([]);
    this.formrights = ComSrv.getFormRights();
    this.FundingSourceID.valueChanges.subscribe(val => val = val ?? 0);
  }
  ChkName() {
    if (this.FundingSourceName.value) {
      this.ComSrv.postJSON('api/FundingSource/CheckName', { FundingSourceID: this.FundingSourceID.value, FundingSourceName: this.FundingSourceName.value }).subscribe((d: any) => {
        //this.users = d;
        if (d)
          this.FundingSourceName.setErrors(null);
        else {
          this.FundingSourceName.setErrors({ 'duplicate': true });
          this.ComSrv.ShowWarning(this.FundingSourceName.value + ' already exists.');
        }
      }, error => this.error = error // error path
      );
    }
  };

  CreateForm() {
    this.fundingsourceform = undefined;
    this.fundingsourceform = this.fb.group({
      FundingSourceID: 0,
      FundingSourceName: new FormControl("", Validators.required),
      InActive: ""
    }, { updateOn: "blur" });
  }

  GetData() {
    this.ComSrv.getJSON('api/FundingSource/GetFundingSource').subscribe((d: any) => {
      this.fundingsource = new MatTableDataSource(d);
      this.fundingsource.paginator = this.paginator;
      this.fundingsource.sort = this.sort;
    }, error => this.error = error // error path
    );
  }
  ngOnInit() {
    this.ComSrv.setTitle("Funding Source");
    this.title = "Add New ";
    this.savebtn = "Save ";
    this.GetData();
  }

  Submit() {
    if (!this.fundingsourceform.valid)
      return;
    this.working = true;
    this.ComSrv.postJSON('api/FundingSource/Save', this.fundingsourceform.value)
      .subscribe((d: any) => {
        this.fundingsource = new MatTableDataSource(d);
        this.fundingsource.paginator = this.paginator;
        this.fundingsource.sort = this.sort;
        this.ComSrv.openSnackBar(this.FundingSourceID.value > 0 ? environment.UpdateMSG.replace("${Name}", this.EnText) : environment.SaveMSG.replace("${Name}", this.EnText));
          this.reset();
          this.FundingSourceID.setValue(0);
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
    this.fundingsourceform.reset();
    this.NForm.resetForm();
    //this.fundingsourceform.setValidators([]);
    this.CreateForm();
    //this.fundingsourceform.markAsUntouched();
    
   
    this.title = "Add New ";
    this.savebtn = "Save ";
  }
  toggleEdit(row) {
    this.FundingSourceID.setValue(row.FundingSourceID);
    this.FundingSourceName.setValue(row.FundingSourceName);
    this.InActive.setValue(row.InActive);

    this.title = "Update ";
    this.savebtn = "Save ";
  }
  toggleActive(row) {
    this.ComSrv.confirm().subscribe(result => {
      if (result) {
        this.ComSrv.postJSON('api/FundingSource/ActiveInActive', { 'FundingSourceID': row.FundingSourceID, 'InActive': row.InActive })
          .subscribe((d: any) => {
            this.ComSrv.openSnackBar(row.InActive == true ? environment.InActiveMSG.replace("${Name}", this.EnText) : environment.ActiveMSG.replace("${Name}", this.EnText));
            // this.fundingsource =new MatTableDataSource(d);
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
    this.fundingsource.filter = filterValue;
  }

  get FundingSourceID() { return this.fundingsourceform.get("FundingSourceID"); }
  get FundingSourceName() { return this.fundingsourceform.get("FundingSourceName"); }
  get InActive() { return this.fundingsourceform.get("InActive"); }
}
export class FundingSourceModel extends ModelBase {
  FundingSourceID: number;
  FundingSourceName: string;

}
