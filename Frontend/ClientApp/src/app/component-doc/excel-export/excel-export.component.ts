import { AfterViewInit, Component, OnInit, ViewChild } from "@angular/core";
import { CommonSrvService ,IQueryFilters} from "../../common-srv.service";
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
import { groupBy, startWith, switchMap } from "rxjs/operators";
import { ActivatedRoute, Router } from "@angular/router";
import * as XLSX from 'xlsx';


@Component({
  selector: 'app-excel-export',
  templateUrl: './excel-export.component.html',
  styleUrls: ['./excel-export.component.scss'],
  providers:[GroupByPipe,DatePipe]
})
export class ExcelExportComponent implements OnInit {
  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private builder: FormBuilder,
    private ComSrv: CommonSrvService,
    public dialog: MatDialog,
    public dialogueService: DialogueService,
    private groupByPipe: GroupByPipe,
    private _date: DatePipe
  ) {}


  ngOnInit(): void {
  }

  JsObjectData: any = [
    { position: 1, name: 'Hydrogen Hydrogen Hydrogen', weight: 1.0079, symbol: 'H' },
    { position: 2, name: 'Helium', weight: 4.0026, symbol: 'He Hydrogen Hydrogen' },
  ];

  JsObjectData1: any = [
    { position: 1, name: 'Hydrogen', weight: 1.0079, symbol: 'H' },
    { position: 2, name: 'Helium', weight: 4.0026, symbol: 'He' },
    { position: 3, name: 'Lithium', weight: 6.941, symbol: 'Li' },
  ];

  exportToExcelinSheet(): void {
    const ws1: XLSX.WorkSheet = XLSX.utils.json_to_sheet(this.JsObjectData);
    const ws2: XLSX.WorkSheet = XLSX.utils.json_to_sheet(this.JsObjectData1);

    const wb: XLSX.WorkBook = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws1, 'Sheet1');
    XLSX.utils.book_append_sheet(wb, ws2, 'Sheet2');

    XLSX.writeFile(wb, 'appendix.xlsx')

  }

  ExportExcel(){
    this.ComSrv.ExportToExcel(this.JsObjectData,'ExcelReportWithOutFormating')
  }

  ExportExcelWithFormating(){
    this.ComSrv.ExcelExporWithForm(this.JsObjectData,'ExcelReportWithFormating')
  }

}
