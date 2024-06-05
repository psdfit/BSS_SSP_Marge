import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from '../../../environments/environment';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { CommonSrvService } from '../../common-srv.service';
import { ModelBase } from '../../shared/ModelBase';
import { UserRightsModel } from '../../master-data/users/users.component';
import { error } from 'protractor';

@Component({
  selector: 'app-subsector',
  templateUrl: './subsector.component.html',
  styleUrls: ['./subsector.component.scss']
})

export class SubSectorComponent implements OnInit {
  subsectorform: FormGroup;
  title: string; savebtn: string;
  displayedColumns = ['SubSectorName', 'SectorID', 'InActive', 'Action'];
  EnText: string = "SubSector";
  subsector: MatTableDataSource<any>;
  Sector: any;
  error: String;
  formrights: UserRightsModel;
  query = {
    order: 'SubSectorID',
    limit: 5,
    page: 1
  };
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  working: boolean;
  constructor(private fb: FormBuilder, private http: CommonSrvService) {
    this.subsectorform = this.fb.group({
      SubSectorID:0,
      SubSectorName: ["", Validators.required],
      SectorID: ["", Validators.required],
      InActive:"",
    }, { updateOn: "blur" });
    this.subsector = new MatTableDataSource([]);
    this.formrights = this.http.getFormRights();
    this.SubSectorID.valueChanges.subscribe((val) => {
      if (val == null)
        this.SubSectorID.setValue(0);
    });
  }

  GetData() {
    this.http.getJSON('api/SubSector/GetSubSector').subscribe((d: any) => {
      this.subsector = new MatTableDataSource(d[0]);
      this.Sector = d[1];

      this.subsector.paginator = this.paginator;
      this.subsector.sort = this.sort;
    }, error => this.error = error // error path
    );
  }

  Submit() {
    if (!this.subsectorform.valid)
      return;
    this.working = true;
    if (this.SubSectorID.value == null) this.SubSectorID.setValue(0);
    this.http.postJSON('api/SubSector/Save', this.subsectorform.value)
      .subscribe((d: any) => {
        this.subsector = new MatTableDataSource(d);
        this.subsector.paginator = this.paginator;
        this.subsector.sort = this.sort;
        this.http.openSnackBar(this.SubSectorID.value > 0 ? environment.UpdateMSG.replace("${Name}", this.EnText) : environment.SaveMSG.replace("${Name}", this.EnText));
          this.reset();
          this.SubSectorID.setValue(0);
          this.working = false;
        this.title = "Add New ";
        this.savebtn = "Save ";
      },
      (error) => {
        this.error = error.error;
        this.http.ShowError(error.error);
        
      });
  }

  ngOnInit() {
    this.http.setTitle("SubSector");
    this.title = "Add New ";
    this.savebtn = "Save ";
    this.GetData();

  }

  reset() {
    this.subsectorform.reset();
    this.title = "Add New ";
    this.savebtn = "Save ";
  }

  toggleEdit(row) {
    this.SubSectorID.setValue(row.SubSectorID);
    this.SubSectorName.setValue(row.SubSectorName);
    this.SectorID.setValue(row.SectorID);
    this.InActive.setValue(row.InActive);

    this.title = "Update ";
    this.savebtn = "Save ";
  }

  toggleActive(row) {
    this.http.confirm().subscribe(result => {
      if (result) {
        this.http.postJSON('api/SubSector/ActiveInActive', { 'SubSectorID': row.SubSectorID, 'InActive': row.InActive })
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
    this.subsector.filter = filterValue;
  }


  get SubSectorID() { return this.subsectorform.get("SubSectorID"); }
  get SubSectorName() { return this.subsectorform.get("SubSectorName"); }
  get SectorID() { return this.subsectorform.get("SectorID"); }
  get InActive() {
    return this.subsectorform.get("InActive");
  }
}
export class SubSectorModel extends ModelBase {
  SubSectorID: number;
  SubSectorName: string;
  SectorID: number;

}

