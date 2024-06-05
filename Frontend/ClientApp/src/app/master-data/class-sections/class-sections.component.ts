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
    selector: 'app-class-sections',
    templateUrl: './class-sections.component.html',
    styleUrls: ['./class-sections.component.scss']
})
export class ClassSectionsComponent implements OnInit {
    classsectionsform: FormGroup;
    title: string; savebtn: string;
    displayedColumns = ['SectionName', 'InActive', "Action"];
    classsections: MatTableDataSource<any>;

    formrights: UserRightsModel;
    EnText: string = "Class Section";
    error: string;
    query = {
        order: 'SectionID',
        limit: 5,
        page: 1
    };
    @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
    @ViewChild(MatSort, { static: false }) sort: MatSort;
    working: boolean;
    constructor(private fb: FormBuilder, private ComSrv: CommonSrvService) {
        this.classsectionsform = this.fb.group({
            SectionID: 0,
            SectionName: ["", Validators.required],
            InActive: ""
        }, { updateOn: "blur" });
        this.classsections = new MatTableDataSource([]);
      this.formrights = ComSrv.getFormRights();
      this.SectionID.valueChanges.subscribe((val) => {
        if (val == null)
          this.SectionID.setValue(0);
      });
    }


    GetData() {
        this.ComSrv.getJSON('api/Sections/GetSections').subscribe((d: any) => {
            this.classsections = new MatTableDataSource(d);
            this.classsections.paginator = this.paginator;
            this.classsections.sort = this.sort;
        }, error => this.error = error // error path
        );
    }
    ngOnInit() {
        this.ComSrv.setTitle("Class Sections");
        this.title = "Add New ";
        this.savebtn = "Save ";
        this.GetData();
    }

    Submit(myform: FormGroupDirective) {
        if (!this.classsectionsform.valid)
            return;
        this.working = true;
        this.ComSrv.postJSON('api/Sections/Save', this.classsectionsform.value)
            .subscribe((d: any) => {
                this.classsections = new MatTableDataSource(d);
                this.classsections.paginator = this.paginator;
                this.classsections.sort = this.sort;
                this.ComSrv.openSnackBar(this.SectionID.value > 0 ? environment.UpdateMSG.replace("${Name}", this.EnText) : environment.SaveMSG.replace("${Name}", this.EnText));
                this.reset(myform);
                this.SectionID.setValue(0);
                this.working = false;
                this.title = "Add New ";
                this.savebtn = "Save ";
            },
            (error) => {
                this.error = error.error;
                this.ComSrv.ShowError(error.error);
                
              });


    }
    reset(myform: FormGroupDirective) {
        this.classsectionsform.reset();
        myform.resetForm();
        this.SectionID.setValue(0);
        this.title = "Add New ";
        this.savebtn = "Save ";
    }
    toggleEdit(row) {
        this.SectionID.setValue(row.SectionID);
        this.SectionName.setValue(row.SectionName);
        this.InActive.setValue(row.InActive);

        this.title = "Update ";
        this.savebtn = "Save ";
    }
    toggleActive(row) {
        this.ComSrv.confirm().subscribe(result => {
            if (result) {
                this.ComSrv.postJSON('api/Sections/ActiveInActive', { 'SectionID': row.SectionID, 'InActive': row.InActive })
                    .subscribe((d: any) => {
                        this.ComSrv.openSnackBar(row.InActive == true ? environment.InActiveMSG.replace("${Name}", this.EnText) : environment.ActiveMSG.replace("${Name}", this.EnText));
                        // this.classsections =new MatTableDataSource(d);
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
        this.classsections.filter = filterValue;
    }

    get SectionID() { return this.classsectionsform.get("SectionID"); }
    get SectionName() { return this.classsectionsform.get("SectionName"); }
    get InActive() { return this.classsectionsform.get("InActive"); }

}
export class ClassSectionsModel extends ModelBase {
    SectionID: number;
    SectionName: string;

}
