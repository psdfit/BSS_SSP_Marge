import { AfterViewInit, Component, OnInit, ViewChild } from "@angular/core";
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
// import { TestComponent } from "src/app/approvals/test/test.component";
import { TStatusHistoryDialogueComponent } from "src/app/custom-components/t-status-history-dialogue/t-status-history-dialogue.component";
import { CertificationAuthorityComponent } from "src/app/master-data/certification-authority/certification-authority.component";
import { TestComponent } from "src/app/approvals/test/test.component";

@Component({
  selector: 'app-dialogue',
  templateUrl: './dialogue.component.html',
  styleUrls: ['./dialogue.component.scss'],
  providers: [GroupByPipe, DatePipe]
})
export class DialogueComponent implements OnInit {
  error: string;
  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private builder: FormBuilder,
    private ComSrv: CommonSrvService,
    public dialog: MatDialog,
    public dialogueService: DialogueService,
    private groupByPipe: GroupByPipe,
    private _date: DatePipe
  ) { }

  ngOnInit(): void {
  }



  OpenEditDialogue(data: any) {
    console.log(data)
    const dialogRef = this.dialog.open(TestComponent, {
      height: '70%',
      width: '85%',
      data: { "titleName": 'draft trainee', "UserID": data ,"TraineeID":547726}
    });
    dialogRef.afterClosed().subscribe(result => {
        // this.GetTraineeDraftData()
    });
  }

  OpenForm(data: any) {
    console.log(data)
    const dialogRef = this.dialog.open(CertificationAuthorityComponent, {
      height: '70%',
      width: '85%',
      data: { "titleName": 'draft trainee', "UserID": data ,"TraineeID":547726}
    });
    dialogRef.afterClosed().subscribe(result => {
        // this.GetTraineeDraftData()
    });
  }
  
    OpenTableGrid(data: any) {
      console.log(data)
      const dialogRef = this.dialog.open(TStatusHistoryDialogueComponent, {
        height: '55%',
        width: '75%',
        data: { "titleName": 'draft trainee', "UserID": data ,"TraineeID":547726}
      });
      dialogRef.afterClosed().subscribe(result => {
          // this.GetTraineeDraftData()
      });
    }
  toggleActive() {
  const title="Active or Inactive this record";
  const massage="Are you sure?";

    this.ComSrv.confirm(title,massage).subscribe(result => {
      if (result) {
        this.ComSrv.postJSON('api/AcademicDiscipline/ActiveInActive', { 'AcademicDisciplineID': 2, 'InActive': 0 })
          .subscribe((d: any) => {
            console.log(d)
            this.ComSrv.openSnackBar('Record has saved');
          },
            (error) => {
              this.error = error.error;
              this.ComSrv.ShowError(error.error);
            })
      }

    })
  }
  

}
