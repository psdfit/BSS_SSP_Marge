import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { CommonSrvService } from '../../common-srv.service';
import { UserRightsModel } from '../../master-data/users/users.component';
import { ITraineeProfile } from '../Interface/ITraineeProfile';
import { TraineeVerificationDialogComponent } from '../trainee-verification-dialog/trainee-verification-dialog.component';
import { FormControl } from '@angular/forms';
import { merge } from 'rxjs';
import { startWith, switchMap } from 'rxjs/operators';
import { SearchFilter } from '../../shared/Interfaces';
import { DialogueService } from 'src/app/shared/dialogue.service';

@Component({
  selector: 'app-trainee-varification',
  templateUrl: './trainee-verification.component.html',
  styleUrls: ['./trainee-verification.component.scss']
})
export class TraineeVarificationComponent implements OnInit, AfterViewInit {
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  matTableDataTraineeProfile: MatTableDataSource<ITraineeProfile>; // = new MatTableDataSource<ITraineeProfile>([]);
  displayedColumnsTraineeProfileList = ['TraineeCode', 'TraineeCNIC', 'ClassCode', 'TraineeVerified', 'Action'];
  traineeProfileArray: any[];
  schemeArray: any[];
  tspDetailArray: any[];
  classesArray: any[];
  formRights: UserRightsModel;
  errorHTTP: string;
  filters: SearchFilter = { SchemeID: 0, ClassID: 0, TSPID: 0, OID: this.http.OID.value };
  traineeProfile: ITraineeProfile;
  schemeFilter = new FormControl(0);
  tspFilter = new FormControl(0);
  classFilter = new FormControl(0);
  resultsLength = 0;
  error = '';
  constructor(private http: CommonSrvService, public dialog: MatDialog, public dialogueService: DialogueService) {
    // this.filters.TspId = 0;
    // this.filters.ClassId = 0;
  }

  ngOnInit() {
    this.http.setTitle('Trainee\'s Verification');
    this.formRights = this.http.getFormRights();
    this.schemeFilter.valueChanges.subscribe(value => { this.getTSPDetailByScheme(value); });
    this.tspFilter.valueChanges.subscribe(value => { this.getClassesByTsp(value) })
  }

  ngAfterViewInit() {
    this.http.OID.subscribe(
      OID => {
        this.schemeFilter.setValue(0);
        this.tspFilter.setValue(0);
        this.classFilter.setValue(0);
        this.filters.OID = OID;

        this.getSchemesByOrg(OID);
        this.initPagedData();
      });
  }
  /// ---Invoke Varification Dialog---S--////
  openDialog(row: ITraineeProfile): void {
    const dialogRef = this.dialog.open(TraineeVerificationDialogComponent, {
      minWidth: '200px',
      minHeight: '400px',
      // data: JSON.parse(JSON.stringify(row))
      data: { ...row }
    })
    dialogRef.afterClosed().subscribe(result => {
      // console.log(result);
      this.traineeProfile = result;
      this.updateTraineeProfile();
    })
  }
  /// ---Invoke Varification Dialog---E--////

  // getTraineeProfile() {
  //  this.http.getJSON(`api/TraineeVerification/GetTraineeProfile/filter?filter=${this.filters.SchemeID}&filter=${this.filters.TSPID}&filter=${this.filters.ClassID}&filter=${this.http.OID.value}`)
  //    .subscribe(data => {
  //      this.traineeProfileArray = <ITraineeProfile[]>data;
  //      this.matTableDataTraineeProfile = new MatTableDataSource<ITraineeProfile>(this.traineeProfileArray);
  //      this.matTableDataTraineeProfile.paginator = this.paginator;
  //      this.matTableDataTraineeProfile.sort = this.sort;
  //    }, error => {
  //      this.errorHTTP = error;
  //    })
  // }
  applyFilter(filterValue: string) {
    filterValue = filterValue.trim(); // Remove whitespace
    filterValue = filterValue.toLowerCase(); // Datasource defaults to lowercase matches
    this.matTableDataTraineeProfile.filter = filterValue;
  }
  updateTraineeProfile(): void {
    if (this.traineeProfile) {
      this.http.postJSON('api/TraineeVerification/ManualVerify/', this.traineeProfile).subscribe(data => {
        const updatedObj = data as ITraineeProfile;
        if (updatedObj.TraineeID) {
          // let list = new Array();
          this.traineeProfileArray = this.traineeProfileArray.map(x => { return x.TraineeID == updatedObj.TraineeID ? updatedObj : x; })
          // this.traineeProfileArray = list;

          // this.matTableDataTraineeProfile = new MatTableDataSource<ITraineeProfile>(this.traineeProfileArray);
          // this.matTableDataTraineeProfile.paginator = this.paginator;
          // this.matTableDataTraineeProfile.sort = this.sort;
        }
        console.log(data);
      },
        (error) => {
          this.error = error.error;
          this.http.ShowError(error.error);
        });
      /*   error => {
          this.errorHTTP = error;
        }); */
    }
  }
  // getData() {
  //  this.http.getJSON(`api/TraineeVerification/GetData?OID=${this.http.OID.value}`).subscribe(
  //    data => {
  //      let schemes = data[0];
  //      let trainees = data[1]

  //      this.schemeArray = schemes;
  //      this.traineeProfileArray = trainees;
  //      this.matTableDataTraineeProfile = new MatTableDataSource<ITraineeProfile>(trainees);
  //      this.matTableDataTraineeProfile.paginator = this.paginator;
  //      this.matTableDataTraineeProfile.sort = this.sort;
  //      console.log(trainees)
  //    }, error => {
  //      this.errorHTTP = error;
  //    })
  // }
  getSchemesByOrg(oid: number) {
    // this.schemeArray = [];
    this.http.getJSON(`api/Scheme/RD_SchemeByOrg?OID=${oid}`)
      .subscribe(data => {
        this.schemeArray = (data as any[]);
      }, error => {
        this.error = error;
      })
  }
  getTSPDetailByScheme(schemeId: number) {
    this.tspFilter.setValue(0);
    this.classFilter.setValue(0);
    // this.tspDetailArray = [];
    // this.classesArray = [];
    this.http.getJSON(`api/TSPDetail/GetTSPDetailByScheme/` + schemeId)
      .subscribe(data => {
        this.tspDetailArray = (data as any[]);
      }, error => {
        this.error = error;
      })
  }
  getClassesByTsp(tspId: number) {
    this.classFilter.setValue(0);
    // this.classesArray = [];
    this.http.getJSON(`api/Class/GetClassesByTsp/` + tspId)
      .subscribe(data => {
        this.classesArray = (data as any[]);
      }, error => {
        this.error = error;
      })
  }
  initPagedData() {
    this.sort.sortChange.subscribe(() => this.paginator.pageIndex = 0);
    this.paginator.pageSize = 10;
    merge(this.sort.sortChange, this.paginator.page, this.schemeFilter.valueChanges, this.tspFilter.valueChanges, this.classFilter.valueChanges).pipe(
      startWith({}),
      switchMap(() => {
        const pagedModel = {
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
        return this.getPagedData(pagedModel, this.filters);
      })).subscribe(data => {
        this.traineeProfileArray = data[0];
        this.resultsLength = data[1].TotalCount;
      });
  }
  getPagedData(pagingModel, filterModel) {
    return this.http.postJSON('api/TraineeVerification/RD_TraineeProfilePaged', { pagingModel, filterModel });
  }

  openTraineeJourneyDialogue(data: any): void {
    // debugger;
    this.dialogueService.openTraineeJourneyDialogue(data);
  }

  openClassJourneyDialogue(data: any): void {
    // debugger;
    this.dialogueService.openClassJourneyDialogue(data);
  }
}
