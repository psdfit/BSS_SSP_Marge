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
  selector: "app-buttons",
  templateUrl: "./buttons.component.html",
  styleUrls: ["./buttons.component.scss"],
  providers: [GroupByPipe, DatePipe]

})
export class ButtonsComponent implements OnInit {
  CurrentUser: UsersModel;
  filters: IQueryFilters = { SchemeID: 0, TSPID: 0, ClassID: 0, TraineeID: 0, UserID: 0, OID: 0 };

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





  ngOnInit() {
    this.CurrentUser = this.ComSrv.getUserDetails();

    this.ComSrv.OID.subscribe(OID => {
      this.filters.SchemeID = 0;
      this.filters.TSPID = 0;
      this.filters.ClassID = 0;
      this.filters.OID = OID;

    })


    this.route.data.subscribe((data) => {
      console.log(data);
    });

  }



}
