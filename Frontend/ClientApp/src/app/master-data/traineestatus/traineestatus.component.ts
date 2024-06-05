/* **** Aamer Rehman Malik *****/
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
  selector: 'app-traineestatus',
  templateUrl: './traineestatus.component.html',
  styleUrls: ['./traineestatus.component.scss']
})
export class TraineeStatusComponent implements OnInit {
  traineestatusform: FormGroup;
  title: string; savebtn: string;
  displayedColumns = ['StatusName', 'InActive'];
  //displayedColumns = ['StatusName', 'InActive', "Action"];
  traineestatustypes: MatTableDataSource<any>;

  formrights: UserRightsModel;
  EnText: string = "Trainee Status Type";
  error: String;
  query = {
    order: 'TraineeStatusTypeID',
    limit: 5,
    page: 1
  };
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  working: boolean;
  constructor(private fb: FormBuilder, private http: CommonSrvService) {
    this.traineestatusform = this.fb.group({
      TraineeStatusTypeID: 0,
      StatusName: ["", Validators.required],
      InActive: ""
    }, { updateOn: "blur" });
    this.traineestatustypes = new MatTableDataSource([]);
    this.formrights = http.getFormRights();
    this.TraineeStatusTypeID.valueChanges.subscribe(val => val == null ? this.TraineeStatusTypeID.setValue(0) : val);
  }

  GetData() {
    this.http.getJSON('api/TraineeStatusTypes/GetTraineeStatusTypes').subscribe((d: any) => {
      this.traineestatustypes = new MatTableDataSource(d);
      this.traineestatustypes.paginator = this.paginator;
      this.traineestatustypes.sort = this.sort;
    }, error => this.error = error // error path
    );
  }
  ngOnInit() {
    this.http.setTitle("TraineeStatusTypes");
    this.title = "Add New ";
    this.savebtn = "Save ";
    this.GetData();
  }

  Submit() {
    if (!this.traineestatusform.valid)
      return;
    this.working = true;
    this.http.postJSON('api/TraineeStatusTypes/Save', this.traineestatusform.value)
      .subscribe((d: any) => {
        this.traineestatustypes = new MatTableDataSource(d);
        this.traineestatustypes.paginator = this.paginator;
        this.traineestatustypes.sort = this.sort;
        this.http.openSnackBar(this.TraineeStatusTypeID.value > 0 ? environment.UpdateMSG.replace("${Name}", this.EnText) : environment.SaveMSG.replace("${Name}", this.EnText));
        this.reset();
        this.title = "Add New ";
        this.savebtn = "Save ";
        this.working = false;
      },
      (error) => {
        this.http.ShowError(error.error);
        
      });
  }
  ChkName() {
    if (this.StatusName.value) {
      this.http.postJSON('api/TraineeStatusTypes/CheckName', { TraineeStatusTypeID: this.TraineeStatusTypeID.value, StatusName: this.StatusName.value }).subscribe((d: any) => {
        //this.users = d;
       
        if (d) {
          const err = this.StatusName.errors; // get control errors
          if (err) {
            delete err['duplicate']; // delete your own error
            if (!Object.keys(err).length) { // if no errors left
              this.StatusName.setErrors(null); // set control errors to null making it VALID
            } else {
              this.StatusName.setErrors(err); // controls got other errors so set them back
            }
          }
        }
        else {
          this.StatusName.setErrors({ 'duplicate': true });
          this.http.ShowWarning(this.StatusName.value + ' already exists.');
        }
      }, error => this.error = error // error path
      );
    }
  };
  reset() {
    this.traineestatusform.reset();
    this.TraineeStatusTypeID.setValue(0);
    this.title = "Add New ";
    this.savebtn = "Save ";
  }
  toggleEdit(row) {
    this.TraineeStatusTypeID.setValue(row.TraineeStatusTypeID);
    this.StatusName.setValue(row.StatusName);
    this.InActive.setValue(row.InActive);

    this.title = "Update ";
    this.savebtn = "Save ";
  }
  toggleActive(row) {
    this.http.confirm().subscribe(result => {
      if (result) {
        this.http.postJSON('api/TraineeStatusTypes/ActiveInActive', { 'TraineeStatusTypeID': row.TraineeStatusTypeID, 'InActive': row.InActive })
          .subscribe((d: any) => {
            this.http.openSnackBar(row.InActive == true ? environment.InActiveMSG.replace("${Name}", this.EnText) : environment.ActiveMSG.replace("${Name}", this.EnText));
            // this.traineestatustypes =new MatTableDataSource(d);
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
    this.traineestatustypes.filter = filterValue;
  }

  get TraineeStatusTypeID() { return this.traineestatusform.get("TraineeStatusTypeID"); }
  get StatusName() { return this.traineestatusform.get("StatusName"); }
  get InActive() { return this.traineestatusform.get("InActive"); }
}
export class TraineeStatusTypesModel extends ModelBase {
  TraineeStatusTypeID: number;
  StatusName: string;
}
