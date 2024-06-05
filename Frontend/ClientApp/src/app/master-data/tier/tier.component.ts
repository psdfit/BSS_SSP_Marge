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
  selector: 'app-tier',
  templateUrl: './tier.component.html',
  styleUrls: ['./tier.component.scss']
})
export class TierComponent implements OnInit {
  tierform: FormGroup;
  title: string; savebtn: string;
  displayedColumns = ['TierName', 'InActive', "Action"];
  tier: MatTableDataSource<any>;

  formrights: UserRightsModel;
  EnText: string = "Tier";
  error: String;
  query = {
    order: 'TierID',
    limit: 5,
    page: 1
  };
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  working: boolean;
  constructor(private fb: FormBuilder, private ComSrv: CommonSrvService) {
    this.tierform = this.fb.group({
      TierID: 0,
      TierName: ["", Validators.required],
      InActive: ""
    }, { updateOn: "blur" });
    this.tier = new MatTableDataSource([]);
    this.formrights = ComSrv.getFormRights();
  }


  GetData() {
    this.ComSrv.getJSON('api/Tier/GetTier').subscribe((d: any) => {
      this.tier = new MatTableDataSource(d);
      this.tier.paginator = this.paginator;
      this.tier.sort = this.sort;
    }, error => this.error = error // error path
    );
  }
  ngOnInit() {
    this.ComSrv.setTitle("Tier");
    this.title = "Add New ";
    this.savebtn = "Save ";
    this.GetData();
  }

  Submit() {
    if (!this.tierform.valid)
      return;
    this.working = true;
    this.ComSrv.postJSON('api/Tier/Save', this.tierform.value)
      .subscribe((d: any) => {
        this.tier = new MatTableDataSource(d);
        this.tier.paginator = this.paginator;
        this.tier.sort = this.sort;
        this.ComSrv.openSnackBar(this.TierID.value > 0 ? environment.UpdateMSG.replace("${Name}", this.EnText) : environment.SaveMSG.replace("${Name}", this.EnText));
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
    this.tierform.reset();
    this.TierID.setValue(0);
    this.title = "Add New ";
    this.savebtn = "Save ";
  }
  toggleEdit(row) {
    this.TierID.setValue(row.TierID);
    this.TierName.setValue(row.TierName);
    this.InActive.setValue(row.InActive);

    this.title = "Update ";
    this.savebtn = "Save ";
  }
  toggleActive(row) {
    this.ComSrv.confirm().subscribe(result => {
      if (result) {
        this.ComSrv.postJSON('api/Tier/ActiveInActive', { 'TierID': row.TierID, 'InActive': row.InActive })
          .subscribe((d: any) => {
            this.ComSrv.openSnackBar(row.InActive == true ? environment.InActiveMSG.replace("${Name}", this.EnText) : environment.ActiveMSG.replace("${Name}", this.EnText));
            // this.tier =new MatTableDataSource(d);
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
    this.tier.filter = filterValue;
  }

  get TierID() { return this.tierform.get("TierID"); }
  get TierName() { return this.tierform.get("TierName"); }
  get InActive() { return this.tierform.get("InActive"); }
}
export class TierModel extends ModelBase {
  TierID: number;
  TierName: string;

}
