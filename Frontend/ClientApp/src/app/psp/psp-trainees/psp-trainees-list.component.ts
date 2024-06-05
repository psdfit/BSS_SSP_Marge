import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from '../../../environments/environment';
import { FormGroup, FormBuilder, Validators, FormControl } from '@angular/forms';
import { CommonSrvService } from '../../common-srv.service';
import { ModelBase } from '../../shared/ModelBase';
import { UserRightsModel } from '../../master-data/users/users.component';
import { Observable } from 'rxjs';
import { UsersModel } from '../../master-data/users/users.component';
import { EnumUserLevel, EnumExcelReportType } from '../../shared/Enumerations';
import { ExportExcel } from '../../shared/Interfaces';
import { GroupByPipe } from 'angular-pipes';
import { DatePipe } from '@angular/common';
import { DialogueService } from '../../shared/dialogue.service';
import { SelectionModel } from '@angular/cdk/collections';
import { PSPBatchDialogueComponent } from '../psp-batch-dialogue/psp-batch-dialogue.component';
import { MatDialog } from '@angular/material/dialog';


@Component({
  selector: 'app-psp-trainees-list',
  templateUrl: './psp-trainees-list.component.html',
  styleUrls: ['./psp-trainees-list.component.scss'],
  providers: [GroupByPipe, DatePipe]

})
export class PSPTraineeListComponent implements OnInit {
  title: string; savebtn: string;
  displayedColumns = ['select','TraineeName', 'FatherName', 'TraineeCNIC',
    'ContactNumber', 'ResultStatusName'
  ];
  filters: IPSPTraineesListFilter = { TradeID: 0, ClassID: 0, UserID: 0 };

  selection = new SelectionModel <any> (true, []);

  PSPTraineesList: MatTableDataSource<any>;
  PSPTraineesListArray: any[];
  formrights: UserRightsModel;
  EnText: string = "Inception Report List";
  error: String;
  currentUser: UsersModel;
  userid: number;

  SearchTradeList = new FormControl('',);
  SearchClassList = new FormControl('',);

  tradesArray = [];  
  classesArray: any[];

  Scheme: any[];

  query = {
    order: 'PSPTraineeID',
    limit: 5,
    page: 1
  };
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  working: boolean;
  constructor(private fb: FormBuilder, private http: CommonSrvService, private groupByPipe: GroupByPipe,
    public dialogueService: DialogueService, private _date: DatePipe, private dialog: MatDialog,) {
    this.PSPTraineesList = new MatTableDataSource([]);
    this.formrights = http.getFormRights();
  }

  ngOnInit() {
    this.http.setTitle("PSP Trainees List");
    this.title = "Add New ";
    this.savebtn = "Save ";
    this.currentUser = this.http.getUserDetails();
    this.userid = this.currentUser.UserID;

    this.getData();
    //this.getPSPTraineesList();

    //this.GetData();
  }

  EmptyCtrl() {
    this.SearchTradeList.setValue('');
    this.SearchClassList.setValue('');
    //this.SearchTSPList.setValue('');
    //this.SearchSchemeList.setValue('');
  }

  isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.PSPTraineesList.data.length;
    return numSelected === numRows;
  }

  masterToggle() {
    this.isAllSelected() ?
      this.selection.clear() :
      this.PSPTraineesList.data.forEach(row => this.selection.select(row));
  }

  exportToExcel() {

  }
 
  getData() {
    this.http.postJSON(`api/PSPEmployment/GetTraineesFiltersData/`, this.filters)
      .subscribe(data => {
        this.tradesArray = data[0];
        this.classesArray = data[1];
      }, error => {
        this.error = error;
      })
  }


  openDialog(): void {
    const dialogRef = this.dialog.open(PSPBatchDialogueComponent, {
      //minWidth: '1000px',
      //minHeight: '600px',
      height: '40%',
      width: '40%',

      //data: JSON.parse(JSON.stringify(row))
      data: { "TraineeData": this.selection.selected, "TradeID": this.filters.TradeID }
      //data: this.selection.selected
      //this.GetVisitPlanData(data)
    })
    dialogRef.afterClosed().subscribe(result => {
      //console.log(result);
      //this.visitPlan = result;
      //this.submitVisitPlan(result);
    })
  }


  getPSPTraineesList() {
    if (this.currentUser.UserLevel == EnumUserLevel.TSP) {
      this.filters.UserID = this.userid;
      this.http.postJSON(`api/PSPEmployment/GetFilteredPSPTrainees/`, this.filters)
        .subscribe((data: any) => {
          this.PSPTraineesList = new MatTableDataSource(data[0]);
          this.PSPTraineesListArray = data[0];
          this.Scheme = data[1];

          //this.Scheme = this.Scheme.filter(x => this.mastersheetArray.map(y => y.SchemeID).includes(x.SchemeID))

          this.PSPTraineesList.paginator = this.paginator;
          this.PSPTraineesList.sort = this.sort;
          //    //
        },
          error => {
            this.error = error;
          })
    } else {
      this.http.postJSON(`api/PSPEmployment/GetFilteredPSPTrainees/`, this.filters)
        .subscribe((data: any) => {
          this.PSPTraineesList = new MatTableDataSource(data[0]);
          this.Scheme = data[1];

          this.PSPTraineesList.paginator = this.paginator;
          this.PSPTraineesList.sort = this.sort;
          //    //
        },
          error => {
            this.error = error;
          })
    }
  }

}

export interface IPSPTraineesListFilter {
  TradeID: number;
  ClassID: number;
  UserID: number;
  //ClassID: number;
  //UserID: number;
}
