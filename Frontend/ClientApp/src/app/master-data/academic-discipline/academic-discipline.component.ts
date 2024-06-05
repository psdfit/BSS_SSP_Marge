import { Component, OnInit, ViewChild, Input } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from '../../../environments/environment';
import { FormGroup, FormBuilder, Validators, FormGroupDirective } from '@angular/forms';
import { CommonSrvService } from '../../common-srv.service';
import { ModelBase } from '../../shared/ModelBase';
import { UserRightsModel } from '../../master-data/users/users.component';

@Component({
  selector: 'app-academic-discipline',
    templateUrl: './academic-discipline.component.html',
    styleUrls: ['./academic-discipline.component.scss']
})
export class AcademicDisciplineComponent implements OnInit {
    academicdisciplineform: FormGroup;
    title: string; savebtn: string;
    displayedColumns = ['AcademicDisciplineName', 'InActive', "Action"];
    academicdiscipline: MatTableDataSource<any>;

    formrights: UserRightsModel;
    EnText: string = "Academic Discipline";
    error: String;
    query = {
        order: 'AcademicDisciplineID',
        limit: 5,
        page: 1
    };
    @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
    @ViewChild(MatSort, { static: false }) sort: MatSort;
    working: boolean;
    constructor(private fb: FormBuilder, private ComSrv: CommonSrvService) {
        this.academicdisciplineform = this.fb.group({
            AcademicDisciplineID: 0,
            AcademicDisciplineName: ["", Validators.required],
            InActive: ""
        }, { updateOn: "blur" });
        this.academicdiscipline = new MatTableDataSource([]);
        this.formrights = ComSrv.getFormRights();
    }


    GetData() {
        this.ComSrv.getJSON('api/AcademicDiscipline/GetAcademicDiscipline').subscribe((d: any) => {
            this.academicdiscipline = new MatTableDataSource(d);
            this.academicdiscipline.paginator = this.paginator;
            this.academicdiscipline.sort = this.sort;
        }, error => this.error = error // error path
        );
    }
    ngOnInit() {
        this.ComSrv.setTitle("Academic Discipline");
        this.title = "Add New ";
        this.savebtn = "Save ";
        this.GetData();
    }

    Submit(myform: FormGroupDirective) {
        if (!this.academicdisciplineform.valid)
            return;
        this.working = true;
        this.ComSrv.postJSON('api/AcademicDiscipline/Save', this.academicdisciplineform.value)
            .subscribe((d: any) => {
                this.academicdiscipline = new MatTableDataSource(d);
                this.academicdiscipline.paginator = this.paginator;
                this.academicdiscipline.sort = this.sort;
                this.ComSrv.openSnackBar(this.AcademicDisciplineID.value > 0 ? environment.UpdateMSG.replace("${Name}", this.EnText) : environment.SaveMSG.replace("${Name}", this.EnText));
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
        this.academicdisciplineform.reset();
        //myform.resetFrom();
        this.AcademicDisciplineID.setValue(0);
        this.title = "Add New ";
        this.savebtn = "Save ";
    }
    toggleEdit(row) {
        this.AcademicDisciplineID.setValue(row.AcademicDisciplineID);
        this.AcademicDisciplineName.setValue(row.AcademicDisciplineName);
        this.InActive.setValue(row.InActive);

        this.title = "Update ";
        this.savebtn = "Save ";
    }
    toggleActive(row) {
        this.ComSrv.confirm().subscribe(result => {
            if (result) {
                this.ComSrv.postJSON('api/AcademicDiscipline/ActiveInActive', { 'AcademicDisciplineID': row.AcademicDisciplineID, 'InActive': row.InActive })
                    .subscribe((d: any) => {
                        this.ComSrv.openSnackBar(row.InActive == true ? environment.InActiveMSG.replace("${Name}", this.EnText) : environment.ActiveMSG.replace("${Name}", this.EnText));
                        // this.academicdiscipline =new MatTableDataSource(d);
                    },
                    (error) => {
                        this.error = error.error;
                        row.InActive = !row.InActive;
                        this.ComSrv.ShowError(error.error);
                        
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
    this.academicdiscipline.filter = filterValue;
}

get AcademicDisciplineID() { return this.academicdisciplineform.get("AcademicDisciplineID"); }
get AcademicDisciplineName() { return this.academicdisciplineform.get("AcademicDisciplineName"); }
get InActive() { return this.academicdisciplineform.get("InActive"); }
}
export class AcademicDisciplineModel extends ModelBase {
    AcademicDisciplineID: number;
    AcademicDisciplineName: string;

}
