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
    selector: 'app-source-of-curriculum',
    templateUrl: './source-of-curriculum.component.html',
    styleUrls: ['./source-of-curriculum.component.scss']
})
export class SourceOfCurriculumComponent implements OnInit {
    sourceofcurriculumform: FormGroup;
    title: string; savebtn: string;
    displayedColumns = ['Name', 'InActive', "Action"];
    sourceofcurriculum: MatTableDataSource<any>;

    formrights: UserRightsModel;
    EnText: string = "Source Of Curriculum";
    error: String;
    CertificationAuthority: [];
    query = {
        order: 'SourceOfCurriculumID',
        limit: 5,
        page: 1
    };
    @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
    @ViewChild(MatSort, { static: false }) sort: MatSort;
    working: boolean;
    constructor(private fb: FormBuilder, private ComSrv: CommonSrvService) {
        this.sourceofcurriculumform = this.fb.group({
            SourceOfCurriculumID: 0,
            Name: ["", Validators.required],
            //CertAuthID: "",
            InActive: ""
        }, { updateOn: "blur" });
        this.sourceofcurriculum = new MatTableDataSource([]);
        this.formrights = ComSrv.getFormRights();
    }


    GetData() {
        this.ComSrv.getJSON('api/SourceOfCurriculum/GetSourceOfCurriculum').subscribe((d: any) => {
            this.sourceofcurriculum = new MatTableDataSource(d[0]);
            //this.CertificationAuthority = (d[1]);
            this.sourceofcurriculum.paginator = this.paginator;
            this.sourceofcurriculum.sort = this.sort;
        }, error => this.error = error // error path
        );
    }
    ngOnInit() {
        this.ComSrv.setTitle("Source Of Curriculum");
        this.title = "Add New ";
        this.savebtn = "Save ";
        this.GetData();
    }

    Submit(myform: FormGroupDirective) {
        if (!this.sourceofcurriculumform.valid)
            return;
        this.working = true;
        this.ComSrv.postJSON('api/SourceOfCurriculum/Save', this.sourceofcurriculumform.value)
            .subscribe((d: any) => {
                this.sourceofcurriculum = new MatTableDataSource(d);
                this.sourceofcurriculum.paginator = this.paginator;
                this.sourceofcurriculum.sort = this.sort;
                this.ComSrv.openSnackBar(this.SourceOfCurriculumID.value > 0 ? environment.UpdateMSG.replace("${Name}", this.EnText) : environment.SaveMSG.replace("${Name}", this.EnText));
                this.reset(myform);
                this.title = "Add New ";
                this.savebtn = "Save ";
                this.working = false;
            },
            (error) => {
                this.error = error.error;
                this.ComSrv.ShowError(error.error);
                
              });

    }
    reset(myform: FormGroupDirective) {
        this.sourceofcurriculumform.reset();
        //myform.resetFrom();
        this.SourceOfCurriculumID.setValue(0);
        this.title = "Add New ";
        this.savebtn = "Save ";
    }
    toggleEdit(row) {
        this.SourceOfCurriculumID.setValue(row.SourceOfCurriculumID);
        this.Name.setValue(row.Name);
        //this.CertAuthID.setValue(row.CertAuthID);
        this.InActive.setValue(row.InActive);

        this.title = "Update ";
        this.savebtn = "Save ";
    }
    toggleActive(row) {
        this.ComSrv.confirm().subscribe(result => {
            if (result) {
                this.ComSrv.postJSON('api/SourceOfCurriculum/ActiveInActive', { 'SourceOfCurriculumID': row.SourceOfCurriculumID, 'InActive': row.InActive })
                    .subscribe((d: any) => {
                        this.ComSrv.openSnackBar(row.InActive == true ? environment.InActiveMSG.replace("${Name}", this.EnText) : environment.ActiveMSG.replace("${Name}", this.EnText));
                        // this.sourceofcurriculum =new MatTableDataSource(d);
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
        this.sourceofcurriculum.filter = filterValue;
    }

    get SourceOfCurriculumID() { return this.sourceofcurriculumform.get("SourceOfCurriculumID"); }
    get Name() { return this.sourceofcurriculumform.get("Name"); }
    //get CertAuthID() { return this.sourceofcurriculumform.get("CertAuthID"); }
    get InActive() { return this.sourceofcurriculumform.get("InActive"); }
}
export class SourceOfCurriculumModel extends ModelBase {
    SourceOfCurriculumID: number;
    Name: string;
    //CertAuthID: number;

}
