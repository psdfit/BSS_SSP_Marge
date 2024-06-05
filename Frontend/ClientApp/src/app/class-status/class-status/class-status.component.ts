import { Component, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { CommonSrvService } from '../../common-srv.service';
import { environment } from '../../../environments/environment';
import { SearchFilter } from '../../shared/Interfaces';
import { FormControl } from '@angular/forms';
import { UserRightsModel, UsersModel } from '../../master-data/users/users.component';
import { MatDialog } from '@angular/material/dialog';
import { DialogueService } from '../../shared/dialogue.service';
import { CsuDialogueComponent } from './csu-dialogue/csu-dialogue.component';
import { merge } from 'rxjs';
import { startWith, switchMap } from 'rxjs/operators';

@Component({
  selector: 'app-class-status',
  templateUrl: './class-status.component.html',
  styleUrls: ['./class-status.component.scss']
})
export class ClassStatusComponent implements OnInit {
  environment = environment;
  schemeArray = [];
  tspDetailArray = [];
  classesArray: any[];
  formrights: UserRightsModel;
  filters: SearchFilter = { SchemeID: 0, TSPID: 0, ClassID: 0, TraineeID: 0, OID: this.commonService.OID.value, SelectedColumns: [] };
  @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: false }) sort: MatSort;
  error: string = "";
  schemeFilter = new FormControl(0);
  tspFilter = new FormControl(0);
  classFilter = new FormControl(0);
  SearchSch = new FormControl('');
  SearchTSP = new FormControl('');
  SearchCls = new FormControl('');
  currentUser: UsersModel;
  resultsLength: number;
  tsrData: any[];

  Class: MatTableDataSource<any>;
  ClassStatuses: any;
  displayedColumnsClass = ['Action', 'ClassCode', 'Duration', 'ClassStatusName', 'StartDate', 'EndDate', 'TrainingAddressLocation', 'TradeName', 'GenderName', 'TraineesPerClass', 'TehsilName', 'CertAuthName'];
  working: boolean;

  constructor(private commonService: CommonSrvService, public dialog: MatDialog, public dialogueService: DialogueService) {  this.formrights = commonService.getFormRights();}

  ngOnInit(): void {
    this.commonService.setTitle("Class Statuses Update");
    this.currentUser = this.commonService.getUserDetails();
    this.schemeFilter.valueChanges.subscribe(value => { this.getTSPDetailByScheme(value); });
    this.tspFilter.valueChanges.subscribe(value => { this.getClassesByTsp(value) })
  }
  ngAfterViewInit() {
    this.commonService.OID.subscribe(
      OID => {
        this.schemeFilter.setValue(0);
        this.tspFilter.setValue(0);
        this.classFilter.setValue(0);
        this.filters.OID = OID;

        this.getSchemesByOrg(OID);
        this.GetData();
      });
  }

  EmptyCtrl(Ev: any) {
    this.SearchCls.setValue('');
    this.SearchTSP.setValue('');
    this.SearchSch.setValue('');
  }
  getSchemesByOrg(oid: number) {
    //this.schemeArray = [];
    this.commonService.getJSON(`api/Scheme/RD_SchemeByOrg?OID=${oid}`)
      .subscribe(data => {
        this.schemeArray = <any[]>data;
      }, error => {
        this.error = error;
      })
  }
  getTSPDetailByScheme(schemeId: number) {
    this.tspFilter.setValue(0);
    this.classFilter.setValue(0);
    //this.tspDetailArray = [];
    //this.classesArray = [];
    this.commonService.getJSON(`api/TSPDetail/GetTSPDetailByScheme/` + schemeId)
      .subscribe(data => {
        this.tspDetailArray = <any[]>data;
      }, error => {
        this.error = error;
      })
  }
  getClassesByTsp(tspId: number) {
    this.classFilter.setValue(0);
    //this.classesArray = [];
    this.commonService.getJSON(`api/Class/GetClassesByTsp/` + tspId)
      .subscribe(data => {
        this.classesArray = <any[]>data;
      }, error => {
        this.error = error;
      })
  }

  GetData() {
    //this.commonService.getJSON('api/Class/RD_ClassByStatus').subscribe((d: any) => {
    //  this.Class = new MatTableDataSource(d[0]);

    //  this.Class.paginator = this.paginator;
    //  this.Class.sort = this.sort;

    //  this.ClassStatuses = d[1];
    //});
    this.initClassPagedData();
  }

  UpdateClassStatus(event, ClassID) {
    this.commonService.postJSON(`api/Class/UpdateClassStatus`, { ClassStatusID: event.value, ClassID: ClassID }).subscribe((d: any) => {
      this.GetData();
    },
      error => this.error = error // error path
      , () => {
        this.working = false;
        this.commonService.openSnackBar(environment.UpdateMSG.replace("${Name}", 'Class Status'));
      });
     
  }

  applyFilter(filterValue: string) {
    filterValue = filterValue.trim(); // Remove whitespace
    filterValue = filterValue.toLowerCase(); // Datasource defaults to lowercase matches
    this.Class.filter = filterValue;
  }

  updateClassStatus(classData: any): void {
    if (classData) {
      this.commonService.postJSON(`api/Class/UpdateClassStatus`, { ClassStatusID: classData.ClassStatusID, ClassID: classData.ClassID }).subscribe((d: any) => {
        this.GetData();
        this.commonService.openSnackBar(environment.UpdateMSG.replace("${Name}", 'Class Status'));
      },
       /*  error => this.error = error // error path
        , () => {
          this.working = false;
          this.commonService.openSnackBar(environment.UpdateMSG.replace("${Name}", 'Class Status'));
        }); */
        (error) => {
          this.error = error.error;
          this.working = false;
          this.commonService.ShowError(error.error);
          
        });
    }
  }

  openDialog(row: any): void {
    const dialogRef = this.dialog.open(CsuDialogueComponent, {
      width: '600px',
      minHeight: '400px',
      data: { ...row }
    })
    dialogRef.afterClosed().subscribe(result => {
      console.log(result);
      this.updateClassStatus(result);
    })
  }
  //openHistoryDialogue(data: any): void {
  //  this.dialogueService.openTraineeStatusHistoryDialogue(data.TraineeID);
  //}
  initClassPagedData() {
    this.sort.sortChange.subscribe(() => this.paginator.pageIndex = 0);
    this.paginator.pageSize = 10;
    merge(this.sort.sortChange, this.paginator.page, this.schemeFilter.valueChanges, this.tspFilter.valueChanges, this.classFilter.valueChanges).pipe(
      startWith({}),
      switchMap(() => {
        let pagedModel = {
          PageNo: this.paginator.pageIndex + 1
          , PageSize: this.paginator.pageSize
          , SortColumn: this.sort.active
          , SortOrder: this.sort.direction
          , SearchColumn: ''
          , SearchValue: ''
        };
        this.filters.SchemeID = this.schemeFilter.value
        this.filters.TSPID = this.tspFilter.value
        this.filters.ClassID = this.classFilter.value
        return this.getClassPagedData(pagedModel, this.filters);
      })).subscribe(data => {
        this.Class = new MatTableDataSource(data[0]);
        this.tsrData= data[0];
        this.Class.paginator = this.paginator;
        this.Class.sort = this.sort;
        this.resultsLength = data[1].TotalCount;
      }, error => this.error = error
      );
  }
  getClassPagedData(pagingModel, filterModel) {
    return this.commonService.postJSON('api/Class/FetchClassesByUserPaged', { pagingModel, filterModel });
  }
  openClassJourneyDialogue(data: any): void 
  {
    debugger;
    this.dialogueService.openClassJourneyDialogue(data);
  }
}
