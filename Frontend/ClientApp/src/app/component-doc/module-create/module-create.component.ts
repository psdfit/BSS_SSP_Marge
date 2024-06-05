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
import { DraftTraineeDialogueComponent } from "src/app/custom-components/draft-trainee-dialogue/draft-trainee-dialogue.component";
// import { TestComponent } from "src/app/approvals/test/test.component";
import { TStatusHistoryDialogueComponent } from "src/app/custom-components/t-status-history-dialogue/t-status-history-dialogue.component";
import { CertificationAuthorityComponent } from "src/app/master-data/certification-authority/certification-authority.component";
import {STEPPER_GLOBAL_OPTIONS} from '@angular/cdk/stepper';
@Component({
  selector: 'app-module-create',
  templateUrl: './module-create.component.html',
  styleUrls: ['./module-create.component.scss'],
  providers:[GroupByPipe,DatePipe]
  
})
export class ModuleCreateComponent implements OnInit,AfterViewInit {

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private _formBuilder: FormBuilder,
    private ComSrv: CommonSrvService,
    public dialog: MatDialog,
    public dialogueService: DialogueService,
    private groupByPipe: GroupByPipe,
    private _date: DatePipe
  ) { }
 

  firstFormGroup = this._formBuilder.group({
    firstCtrl: ['', Validators.required],
  });
  secondFormGroup = this._formBuilder.group({
    secondCtrl: ['', Validators.required],
  });
  
  thirdFormGroup = this._formBuilder.group({
    secondCtrl: ['', Validators.required],
  });
  forthFormGroup = this._formBuilder.group({
    secondCtrl: ['', Validators.required],
  });
  fifthFormGroup = this._formBuilder.group({
    secondCtrl: ['', Validators.required],
  });
  isLinear = false;

  HtmlCodeBasicTempl:any;
  TSCodeBasicTempl:any;
 

  ngOnInit(): void {
  }


  ngAfterViewInit(){
    this.HtmlCodeBasicTempl=`<div class="comp-main-div" id="AcademicDiscipline-page">
    <mat-card>
       <mat-card-content>
       <mat-toolbar class="mat-elevation-z2 slim" color="accent">
       <mat-icon class="material-icons">add_circle</mat-icon>
       <span class="subheading-1">Create Module</span>
        </mat-toolbar>
          <div>
            <div class="row">
               <div class="col-sm-12">
                  .....
               </div>
            </div>
          </div>
       </mat-card-content>
    </mat-card>
    <mat-divider></mat-divider>
  </div>`;
  this.TSCodeBasicTempl=`import { AfterViewInit, Component, OnInit, ViewChild } from "@angular/core";
  import { CommonSrvService, IQueryFilters } from "../../common-srv.service";
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
  import { DraftTraineeDialogueComponent } from "src/app/custom-components/draft-trainee-dialogue/draft-trainee-dialogue.component";
  import { TestComponent } from "src/app/approvals/test/test.component";
  import { TStatusHistoryDialogueComponent } from "src/app/custom-components/t-status-history-dialogue/t-status-history-dialogue.component";
  import { CertificationAuthorityComponent } from "src/app/master-data/certification-authority/certification-authority.component";
 --providers:[GroupByPipe,DatePipe] add in @component {}
 constructor(
  private route: ActivatedRoute,
  private router: Router,
  private _formBuilder: FormBuilder,
  private ComSrv: CommonSrvService,
  public dialog: MatDialog,
  public dialogueService: DialogueService,
  private groupByPipe: GroupByPipe,
  private _date: DatePipe
) { }`
    
  }

}
