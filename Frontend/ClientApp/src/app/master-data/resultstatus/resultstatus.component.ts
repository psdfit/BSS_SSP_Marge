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
  selector: 'app-resultstatus ',
  templateUrl: './resultstatus.component.html',
  styleUrls: ['./resultstatus.component.scss']
})
export class ResultStatusComponent implements OnInit {
  resultstatusform: FormGroup;
  title: string; savebtn: string;
  displayedColumns = ['ResultStatusName', 'InActive'];
  //displayedColumns = ['ResultStatusName', 'InActive', "Action"];
  traineeresultstatustypes: MatTableDataSource<any>;

  formrights: UserRightsModel;
  EnText: string = "Trainee Result Status";
  error: String;
  query = {
    order: 'ResultStatusID',
    limit: 5,
    page: 1
  };
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  working: boolean;
  constructor(private fb: FormBuilder, private ComSrv: CommonSrvService) {
    this.resultstatusform = this.fb.group({
      ResultStatusID: 0,
      ResultStatusName: ["", Validators.required],
      InActive: ""
    }, { updateOn: "blur" });
    this.traineeresultstatustypes = new MatTableDataSource([]);
    this.formrights = ComSrv.getFormRights();
  }


  GetData() {
    this.ComSrv.getJSON('api/TraineeResultStatusTypes/GetTraineeResultStatusTypes').subscribe((d: any) => {
      this.traineeresultstatustypes = new MatTableDataSource(d);
      this.traineeresultstatustypes.paginator = this.paginator;
      this.traineeresultstatustypes.sort = this.sort;
    }, error => this.error = error // error path
    );
  }
  ngOnInit() {
    this.ComSrv.setTitle("Trainee Result Status Types");
    this.title = "Add New ";
    this.savebtn = "Save ";
    this.GetData();
  }

  Submit() {
    if (!this.resultstatusform.valid)
      return;
    this.working = true;
    this.ComSrv.postJSON('api/TraineeResultStatusTypes/Save', this.resultstatusform.value)
      .subscribe((d: any) => {
        this.traineeresultstatustypes = new MatTableDataSource(d);
        this.traineeresultstatustypes.paginator = this.paginator;
        this.traineeresultstatustypes.sort = this.sort;
        this.ComSrv.openSnackBar(this.ResultStatusID.value > 0 ? environment.UpdateMSG.replace("${Name}", this.EnText) : environment.SaveMSG.replace("${Name}", this.EnText));
        this.reset();
        this.title = "Add New ";
        this.savebtn = "Save ";
      },
        error => this.error = error // error path
        , () => {
          this.working = false;

        });


  }
  reset() {
    this.resultstatusform.reset();
    this.ResultStatusID.setValue(0);
    this.title = "Add New ";
    this.savebtn = "Save ";
  }
  toggleEdit(row) {
    this.ResultStatusID.setValue(row.ResultStatusID);
    this.ResultStatusName.setValue(row.ResultStatusName);
    this.InActive.setValue(row.InActive);

    this.title = "Update ";
    this.savebtn = "Save ";
  }
  toggleActive(row) {
    this.ComSrv.confirm().subscribe(result => {
      if (result) {
        this.ComSrv.postJSON('api/TraineeResultStatusTypes/ActiveInActive', { 'ResultStatusID': row.ResultStatusID, 'InActive': row.InActive })
          .subscribe((d: any) => {
            this.ComSrv.openSnackBar(row.InActive == true ? environment.InActiveMSG.replace("${Name}", this.EnText) : environment.ActiveMSG.replace("${Name}", this.EnText));
            // this.traineeresultstatustypes =new MatTableDataSource(d);
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
    this.traineeresultstatustypes.filter = filterValue;
  }

  get ResultStatusID() { return this.resultstatusform.get("ResultStatusID"); }
  get ResultStatusName() { return this.resultstatusform.get("ResultStatusName"); }
  get InActive() { return this.resultstatusform.get("InActive"); }

}
export class TraineeResultStatusTypesModel extends ModelBase {
  ResultStatusID: number;
  ResultStatusName: string;

}
