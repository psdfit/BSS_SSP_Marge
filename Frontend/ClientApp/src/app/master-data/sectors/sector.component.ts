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
  selector: 'app-sector',
  templateUrl: './sector.component.html',
  styleUrls: ['./sector.component.scss']
})

export class SectorComponent implements OnInit {
  sectorform: FormGroup;
  title: string; savebtn: string;
  displayedColumns = ['SectorName', 'InActive', "Action"];
  sector: MatTableDataSource<any>;

  formrights: UserRightsModel;
  EnText: string = "Sector";
  error: String;
  query = {
    order: 'SectorID',
    limit: 5,
    page: 1
  };
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  working: boolean;
  constructor(private fb: FormBuilder, private http: CommonSrvService) {
    this.sectorform = this.fb.group({
      SectorID: 0,
      SectorName: ["", Validators.required],
      InActive: ""
    }, { updateOn: "blur" });
    this.sector = new MatTableDataSource([]);
    this.formrights = http.getFormRights();
    this.SectorID.valueChanges.subscribe((val) => {
      if (val == null)
        this.SectorID.setValue(0);
    });
  }


  GetData() {
    this.http.getJSON('api/Sector/GetSector').subscribe((d: any) => {
      this.sector = new MatTableDataSource(d);
      this.sector.paginator = this.paginator;
      this.sector.sort = this.sort;
    }, error => this.error = error // error path
    );
  }
  ngOnInit() {
    this.http.setTitle("Sector");
    this.title = "Add New ";
    this.savebtn = "Save ";
    this.GetData();
  }

  Submit() {
    if (!this.sectorform.valid)
      return;
    this.working = true;
    this.http.postJSON('api/Sector/Save', this.sectorform.value)
      .subscribe((d: any) => {
        this.sector = new MatTableDataSource(d);
        this.sector.paginator = this.paginator;
        this.sector.sort = this.sort;
        this.http.openSnackBar(this.SectorID.value > 0 ? environment.UpdateMSG.replace("${Name}", this.EnText) : environment.SaveMSG.replace("${Name}", this.EnText));
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
    this.sectorform.reset();
    this.SectorID.setValue(0);
    this.title = "Add New ";
    this.savebtn = "Save ";
  }
  toggleEdit(row) {
    this.SectorID.setValue(row.SectorID);
    this.SectorName.setValue(row.SectorName);
    this.InActive.setValue(row.InActive);

    this.title = "Update ";
    this.savebtn = "Save ";
  }
  toggleActive(row) {
    this.http.confirm().subscribe(result => {
      if (result) {
        this.http.postJSON('api/Sector/ActiveInActive', { 'SectorID': row.SectorID, 'InActive': row.InActive })
          .subscribe((d: any) => {
            this.http.openSnackBar(row.InActive == true ? environment.InActiveMSG.replace("${Name}", this.EnText) : environment.ActiveMSG.replace("${Name}", this.EnText));
            // this.sector =new MatTableDataSource(d);
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
    this.sector.filter = filterValue;
  }

  get SectorID() { return this.sectorform.get("SectorID"); }
  get SectorName() { return this.sectorform.get("SectorName"); }
  get InActive() { return this.sectorform.get("InActive"); }
}

export class sectorModel extends ModelBase {
  SectorID: number;
  SectorName: string;
}
