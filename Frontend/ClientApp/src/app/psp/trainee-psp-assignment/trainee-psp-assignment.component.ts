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
  selector: 'app-trainee-psp-assignment',
  templateUrl: './trainee-psp-assignment.component.html',
  styleUrls: ['./trainee-psp-assignment.component.scss'],
  providers: [GroupByPipe, DatePipe]

})
export class TraineePSPAssignmentComponent implements OnInit {
  title: string; savebtn: string;
  displayedColumns = ['select', 'TraineeName', 'FatherName', 'TraineeCNIC',
    'ContactNumber'
  ];
  filters: ITraineePSPAssignmentListFilter = { UserIDs: [], PSPBatchID: 0 };

  selection = new SelectionModel<any>(true, []);

  PSPTraineesList: MatTableDataSource<any>;
  PSPTraineesListArray: any[];
  formrights: UserRightsModel;
  EnText: string = "Trainee PSP Assignment";
  error: String;
  update: String;
  currentUser: UsersModel;
  userid: number;

  SearchUsr = new FormControl('',);
  SearchBatchList = new FormControl('',);

  PSPAssignedTraineeIDs = [];
  pspUserIDsArray = [];
  tradesArray = [];
  users = [];
  pspbatches = [];
  pspBatchInterestedTrainees: MatTableDataSource<any>;
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
    this.pspBatchInterestedTrainees = new MatTableDataSource([]);
    this.formrights = http.getFormRights();
  }

  ngOnInit() {
    this.http.setTitle("Trainees List for PSP Assignment");
    this.title = "Add New ";
    this.savebtn = "Save ";
    this.currentUser = this.http.getUserDetails();
    this.userid = this.currentUser.UserID;

    this.getData();
    //this.getPSPTraineesList();

    //this.GetData();
  }

  EmptyCtrl() {
    this.SearchUsr.setValue('');

  }

  isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.pspBatchInterestedTrainees.data.length;
    return numSelected === numRows;
  }

  masterToggle() {
    this.isAllSelected() ?
      this.selection.clear() :
      this.pspBatchInterestedTrainees.data.forEach(row => this.selection.select(row));
  }


  getData() {
    this.http.getJSON(`api/PSPEmployment/GetPSPTraineeAssignment/` + this.filters.PSPBatchID)
      .subscribe(data => {
        this.users = data[0];
        this.pspbatches = data[1];
        this.pspBatchInterestedTrainees = new MatTableDataSource(data[2]);

        this.pspBatchInterestedTrainees.paginator = this.paginator;
        this.pspBatchInterestedTrainees.sort = this.sort;
      }, error => {
        this.error = error;
      })
  }


  Submit() {
    if (this.selection.selected.length == 0 || this.filters.UserIDs.length == 0) {
      this.http.ShowError("Please select Trainees or PSP");
      return;
    }
    this.PSPAssignedTraineeIDs = this.selection.selected.map(x => x.TraineeID);
    var selectedTraineeIDs = this.PSPAssignedTraineeIDs.join(',');

    this.pspUserIDsArray = this.filters.UserIDs;
    var pspUserIDs = this.pspUserIDsArray.join(',');

    //var traineesobj = {
    //  PSPAssignedTrainees: PSPAssignedTraineeIDs,
    //  PSPUserID: this.filters.UserID
    //}
    //var myjson = JSON.stringify(traineesobj);
    this.http.confirmTraineePSP().subscribe(result => {
      if (result) {
        //this.pspbatchform.value['CompletedTrainees'] = this.selection.selected;
        this.http.postJSON('api/PSPEmployment/SubmitPSPAssignedTrainees/', { PSPAssignedTraineeIDs: selectedTraineeIDs, PSPUserIDs: pspUserIDs })
          .subscribe((d: any) => {
            if (d) {
              this.update = "Successfully assigned PSP to selected trainees";
              this.http.openSnackBar(this.update.toString(), "Updated");
            }
            //this.reset();
            this.title = "Add New ";
            this.savebtn = "Save ";
          },
          (error) => {
            this.error = error.error;
            this.working = false;
            this.http.ShowError(error.error);
            
          });
      }
      else {

      }
    });



  }



}

export interface ITraineePSPAssignmentListFilter {

  UserIDs: any[];
  PSPBatchID: number;
  //ClassID: number;
  //UserID: number;
}
