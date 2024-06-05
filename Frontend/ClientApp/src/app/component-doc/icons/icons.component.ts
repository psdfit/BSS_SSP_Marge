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
import { startWith, switchMap } from "rxjs/operators";
import { ActivatedRoute, Router } from "@angular/router";

@Component({
  selector: 'app-icons',
  templateUrl: './icons.component.html',
  styleUrls: ['./icons.component.scss']
})
export class IconsComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
  }

}
