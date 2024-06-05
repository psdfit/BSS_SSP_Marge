import { AfterViewInit, Component, OnInit, ViewChild } from "@angular/core";
import { CommonSrvService } from "../../common-srv.service";
import { MatDialog } from "@angular/material/dialog";
import { MatPaginator } from "@angular/material/paginator";
import { MatSort } from "@angular/material/sort";
import { MatTableDataSource } from "@angular/material/table";
import {
  FormGroup,
  FormBuilder,
  Validators,
  FormControl,
  FormArray,
  AbstractControl,
} from "@angular/forms";
import { environment } from "../../../environments/environment";
import { ModelBase } from "../../shared/ModelBase";
import { UserRightsModel } from "../../master-data/users/users.component";
import { UsersModel } from "../../master-data/users/users.component";
import { EnumUserLevel, EnumExcelReportType } from "../../shared/Enumerations";
import { DialogueService } from "../../shared/dialogue.service";
import { ExportExcel } from "../../shared/Interfaces";
import { GroupByPipe } from "angular-pipes";
import { DatePipe } from "@angular/common";
import { merge } from "rxjs";
import { startWith, switchMap } from "rxjs/operators";
import { ActivatedRoute, Router } from "@angular/router";

@Component({
  selector: 'app-tables',
  templateUrl: './tables.component.html',
  styleUrls: ['./tables.component.scss'],
  providers: [GroupByPipe, DatePipe]
})
export class TablesComponent implements OnInit {

  displayedColumns = ['AcademicDisciplineName', "Action"];
  academicdiscipline: MatTableDataSource<any>;
  EnText: string = "Academic Discipline";
  error: String;

  @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: false }) sort: MatSort;
  working: boolean;


  constructor(private ComSrv: CommonSrvService) {
    this.academicdiscipline = new MatTableDataSource([]);
  }


  GetData() {
    this.ComSrv.getJSON('api/AcademicDiscipline/GetAcademicDiscipline').subscribe((d: any) => {
      this.academicdiscipline = new MatTableDataSource(d);
      this.academicdiscipline.paginator = this.paginator;
      this.academicdiscipline.sort = this.sort;
    }, error => this.error = error 
    );
  }

  ngOnInit() {
    this.ComSrv.setTitle("Academic Discipline");
    this.GetData();
  }


  applyFilter(event: any) {
    this.academicdiscipline.filter = event.target.value.trim().toLowerCase();
    if (this.academicdiscipline.paginator) {
      this.academicdiscipline.paginator.firstPage();
    }
  }

  DataExcelExport(){
    console.log(this.academicdiscipline)
    this.ComSrv.ExcelExporWithForm(this.academicdiscipline.filteredData, 'Academic Discipline')
    
  }


}
