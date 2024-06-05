import { Component, OnInit, ViewChild } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { CommonSrvService } from '../../common-srv.service';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from '../../../environments/environment.prod';

@Component({
  selector: 'hrapp-app-forms',
  templateUrl: './app-forms.component.html',
  styleUrls: ['./app-forms.component.scss']
})
export class AppFormsComponent implements OnInit {
  appformsform: FormGroup;
  title: string; savebtn: string;
  displayedColumns = ['FormName', 'Path', 'Controller', 'ModuleID', 'InActive', "Action"];
  appforms: MatTableDataSource<any>;
  modules = [];
  EnText: string = "Form";
  error: String;
  query = {
    order: 'FormID',
    limit: 5,
    page: 1
  };
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  working: boolean;
  constructor(private fb: FormBuilder, private http: CommonSrvService) {
    this.appformsform = this.fb.group({
      FormID: 0,
      FormName: ["", Validators.required],
      Path: ["", Validators.required],
      Controller: ["", Validators.required],
      ModuleID: ["", Validators.required],
      InActive: ""
    }, { updateOn: "blur" });
    this.appforms = new MatTableDataSource([]);
  }

  GetData() {
    this.http.getJSON('api/AppForms/GetAppForms').subscribe((d: any) => {
      this.appforms = new MatTableDataSource(d[0])
      this.modules = d[1];

      this.appforms.paginator = this.paginator;
      this.appforms.sort = this.sort;
    }, error => this.error = error // error path
    );
  }
  ngOnInit() {
    this.http.setTitle("Manage AppForms");
    this.title = "Add New ";
    this.savebtn = "Save ";
    this.GetData();
  }

  Submit() {
    this.working = true;
    if (!this.appformsform.valid)
      return;
    this.http.postJSON('api/AppForms/Save', this.appformsform.value)
      .subscribe((d: any) => {
        this.appforms = new MatTableDataSource(d);
        this.appforms.paginator = this.paginator;
        this.appforms.sort = this.sort;
        this.http.openSnackBar(this.FormID.value > 0 ? environment.UpdateMSG.replace("${Name}", this.EnText) : environment.SaveMSG.replace("${Name}", this.EnText));
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
    this.appformsform.reset();
    this.title = "Add New ";
    this.savebtn = "Save ";
  }
  toggleEdit(row) {
    this.FormID.setValue(row.FormID);
    this.FormName.setValue(row.FormName);
    this.Path.setValue(row.Path);
    this.Controller.setValue(row.Controller);
    this.ModuleID.setValue(row.ModuleID);
    this.InActive.setValue(row.InActive);

    this.title = "Update ";
    this.savebtn = "Save ";
  }
  toggleActive(row) {
    this.http.confirm().subscribe(result => {
      if (result) {
        this.http.postJSON('api/AppForms/ActiveInActive', { 'FormID': row.FormID, 'InActive': (row.InActive ? false : true) })
          .subscribe((d: any) => {
            this.http.openSnackBar(row.InActive == false ? environment.InActiveMSG.replace("${Name}", this.EnText) : environment.ActiveMSG.replace("${Name}", this.EnText));
            // this.appforms =new MatTableDataSource(d);
          },
            error => this.error = error // error path
          );
      }
      else {
        this.appforms.filter = "1111";
        this.appforms.filter = "";
      }
    });
  }
  applyFilter(filterValue: string) {
    filterValue = filterValue.trim(); // Remove whitespace
    filterValue = filterValue.toLowerCase(); // Datasource defaults to lowercase matches
    this.appforms.filter = filterValue;
  }

  get FormID() { return this.appformsform.get("FormID"); }
  get FormName() { return this.appformsform.get("FormName"); }
  get Path() { return this.appformsform.get("Path"); }
  get Controller() { return this.appformsform.get("Controller"); }
  get ModuleID() { return this.appformsform.get("ModuleID"); }
  get InActive() { return this.appformsform.get("InActive"); }
}
