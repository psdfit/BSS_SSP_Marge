import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { CommonSrvService } from '../../common-srv.service';
import { MatDialog } from '@angular/material/dialog';

import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { FormGroup, FormBuilder, Validators, FormControl } from '@angular/forms';
import { environment } from '../../../environments/environment';
import { ModelBase } from '../../shared/ModelBase';
import { UserRightsModel } from '../../master-data/users/users.component';
import { UsersModel } from '../../master-data/users/users.component';
import { EnumUserLevel, EnumExcelReportType } from '../../shared/Enumerations';

import { DialogueService } from '../../shared/dialogue.service';
import { ExportExcel } from '../../shared/Interfaces';
import { GroupByPipe } from 'angular-pipes';
import { DatePipe } from '@angular/common';
import { merge } from 'rxjs';
import { startWith, switchMap } from 'rxjs/operators';



@Component({
  selector: 'app-skills-scholarship-initiative',
  templateUrl: './skills-scholarship-initiative.component.html',
  styleUrls: ['./skills-scholarship-initiative.component.scss'],
  providers: [GroupByPipe, DatePipe]

})

export class SkillsScholarshipComponent implements OnInit {
  environment = environment;
  SearchSch = new FormControl('',);
  title: string;
  isDivHidden: boolean = false;
  SessionControl: boolean = true;
  buttonTitle: string = 'Session Control View';

  hideDiv() { //Hide div 
    this.isDivHidden = !this.isDivHidden;
    this.SessionControl = !this.SessionControl;
    this.buttonTitle = this.isDivHidden ? 'View Dashboard' : 'Session Control View';

    this.ComSrv.OID.subscribe(OID => {
      this.filters.TSPID = 0;
      this.filters.Locality = 0;
      this.filters.Cluster = 0;
      this.filters.District = 0;
      this.getSkillsScholarshipData();
      // this.initPagedData();
    })
    if (this.buttonTitle == 'View Dashboard') {
      this.getUsersSessionData();
    }
  }
  
  hideColumn = true;
  displayedColumns: string[] = this.hideColumn ? ['DistrictName', 'TradeName', 'TradeTarget', 'EnrolmentsCompleted', 'OverallEnrolments', 'RemainingSeats'] : ['Action', 'DistrictName', 'TradeName', 'NoOfAssociate', 'TradeTarget', 'EnrolmentsCompleted', 'RemainingSeats'];
  columnsForTable2: string[] = ['Action', 'TSPName', 'UserName', 'LoginDateTime', 'SessionID', 'IPAddress'];
  updateColumnVisibility() {
    // Update hideColumn based on your condition
    if (this.currentUser.UserLevel === EnumUserLevel.TSP) {
      this.hideColumn = true;
    }
    else {
      this.hideColumn = false;
    }

    this.displayedColumns = this.hideColumn ? ['DistrictName', 'TradeName', 'TradeTarget', 'EnrolmentsCompleted', 'OverallEnrolments', 'RemainingSeats'] : ['Action', 'DistrictName', 'TradeName', 'NoOfAssociate', 'TradeTarget', 'EnrolmentsCompleted', 'RemainingSeats'];
  }

  enumUserLevel = EnumUserLevel;
  SkillsScholarshipArray: MatTableDataSource<any>;
  UsersSessionDate: MatTableDataSource<any>;
  Scheme = [];
  TSP = [];
  Cluster = [];
  District = [];


  formrights: UserRightsModel;
  EnText = 'Skills-Scholarship-Initiative';
  error: string;
  query = {
    order: 'ID',
    limit: 10,
    page: 1
  };
  // Some array of things.
  filters: ISkillscholarshipFilter = { SchemeID: 0, TSPID: 0, Locality: 0, Cluster: 0, District: 0 };
  @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: false }) sort: MatSort;
  working: boolean;
  currentUser: UsersModel;
  userid: number;
  isInternalUser = false;
  isTSPUser = false;

  // Pagination\\
  resultsLength: number;
  schemeFilter = new FormControl(0);
  TSPFilter = new FormControl(1);
  LocalityFilter = new FormControl(2);
  ClusterFilter = new FormControl(3);
  DistrictFilter = new FormControl(4);
  constructor(private fb: FormBuilder, private http: CommonSrvService, private ComSrv: CommonSrvService, public dialog: MatDialog,
    public dialogueService: DialogueService
    , private groupByPipe: GroupByPipe
    , private _date: DatePipe,) {
   
  }
  EmptyCtrl() {
    this.SearchSch.setValue('');
  }

  getClusterDropdown() {
    this.ComSrv.getJSON(`api/SkillsScholarshipInitiative/GetFilteredClusters/` + this.filters.Locality)
      .subscribe((data: any) => { 
        this.Cluster = data[0];
      },
        error => {
          this.error = error;
        })
  }

  getDistrictDropdown() {
    this.ComSrv.getJSON(`api/SkillsScholarshipInitiative/GetFilteredDistricts/` + this.filters.Cluster)
      .subscribe((data: any) => {
        this.District = data[0];
      },
        error => {
          this.error = error;
        })
  }

 

  getStartRaceOfRow(r) {

    var Clusterid = r.ClusterID;
    var Tradeid = r.TradeID;
    var Schemeid = r.SchemeID;
    var Districtid = r.DistrictID;
    let titleConfirm = "Confirmation";
    let messageConfirm = "Do you want to Start the Race?";
    this.http.confirm(titleConfirm, messageConfirm).subscribe(
      (isConfirm: Boolean) => {
        if (isConfirm) {
          this.http.getJSON(`api/SkillsScholarshipInitiative/StartRace/` + Schemeid + `/` + Clusterid  + `/` + Districtid + `/` + Tradeid ).subscribe(
            (d: any) => {
              if (d) {
                this.getSkillsScholarshipData();
              }
            },
            (error) => {
              this.error = error.error;
              this.http.ShowError(error.error + '\n' + error.message, '', 5000);
            } // error path
          );
        }
      }
    );

  }
  //---Stop Race
  getStopRaceOfRow(r) {

    if (!r.HasRaceStarted) {
      this.http.ShowError('' + '\n' + 'Race not started for this Trade!', '', 5000);
      return;
    }
    var Clusterid = r.ClusterID;
    var Tradeid = r.TradeID;
    var Schemeid = r.SchemeID;
    var Districtid = r.DistrictID;
    let titleConfirm = "Confirmation";
    let messageConfirm = "Do you want to Stop the Race?";
    this.http.confirm(titleConfirm, messageConfirm).subscribe(
      (isConfirm: Boolean) => {
        if (isConfirm) {
          this.http.getJSON(`api/SkillsScholarshipInitiative/StopRace/` + Schemeid + `/` + Clusterid + `/` + Districtid + `/` + Tradeid).subscribe(
            (d: any) => {
              if (d) {
                this.getSkillsScholarshipData();
              }
            },
            (error) => {
              this.error = error.error;
              this.http.ShowError(error.error + '\n' + error.message, '', 5000);
            } // error path
          );
        }
      }
    );

  }

  getSkillsScholarshipData() {
    if (this.isTSPUser) {
      this.ComSrv.getJSON(`api/SkillsScholarshipInitiative/GetFilteredSkillsScholarshipInitiative/` + this.filters.SchemeID + `/` + this.filters.TSPID + `/` + this.filters.Locality + `/` + this.filters.Cluster + `/` + this.filters.District)
        .subscribe((data: any) => {
          // this.mastersheet = new MatTableDataSource(data[0]); 
          this.Scheme = data[0];
          this.SkillsScholarshipArray = new MatTableDataSource(data[1]);
          //this.filters.TSPID = data[0].map(x=> x.TSPID);
        
          // this.Scheme = this.Scheme.filter(x => this.mastersheetArray.map(y => y.SchemeID).includes(x.SchemeID))

          // this.mastersheet.paginator = this.paginator;
          // this.mastersheet.sort = this.sort;
          //    //
        },
          error => {
            this.error = error;
          })
    } else {
      this.ComSrv.getJSON(`api/SkillsScholarshipInitiative/GetFilteredSkillsScholarshipInitiative/` + this.filters.SchemeID + `/` + this.filters.TSPID + `/` + this.filters.Locality + `/` + this.filters.Cluster + `/` + this.filters.District)
        .subscribe((data: any) => {
          // this.mastersheet = new MatTableDataSource(data[0]);
          this.Scheme = data[1];
          this.SkillsScholarshipArray = new MatTableDataSource(data[2]);
          this.TSP = data[0];
          // this.mastersheet.paginator = this.paginator;
          // this.mastersheet.sort = this.sort;
          //    //
        },
          error => {
            this.error = error;
          })
    }
  }

 

  ngOnInit() {
    this.ComSrv.setTitle('Skills Scholarship Initiative');
    this.currentUser = this.ComSrv.getUserDetails();
    this.userid = this.currentUser.UserID;
    // this.schemeFilter.valueChanges.subscribe(value => { this.getTSPDetailByScheme(value); });
    // this.tspFilter.valueChanges.subscribe(value => { this.getClassesByTsp(value) })
    this.updateColumnVisibility();
    if (this.currentUser.UserLevel === EnumUserLevel.TSP) {
      this.isTSPUser = true;
      //this.getTSPDetailbyUseriD();
      this.getSkillsScholarshipData();
      
    } else if (this.currentUser.UserLevel === EnumUserLevel.AdminGroup || this.currentUser.UserLevel === EnumUserLevel.OrganizationGroup) {
      this.isInternalUser = true;
      this.getSkillsScholarshipData();
      
    }
    
    this.ComSrv.OID.subscribe(OID => {
      this.filters.SchemeID = 0;
      this.filters.TSPID = 0;
      this.filters.Locality = 0;
      this.filters.Cluster = 0;
      this.filters.District = 0;
      // this.getMasterSheet();
      // this.initPagedData();
    })
  }


  getTSPDetailbyUseriD() {
    if (this.isTSPUser) {
      this.http.getJSON('api/SkillsScholarshipInitiative/GetFilteredUserbyID/' + this.filters.SchemeID).subscribe(
        (d: any) => {
          this.filters.TSPID = d[0].map(x => x.TSPID);
          
        });
    }
  }


  getUsersSessionData() {
    this.ComSrv.getJSON(`api/SkillsScholarshipInitiative/GetFilteredSessionCount/` + this.filters.SchemeID + `/` + this.filters.TSPID)
      .subscribe((data: any) => {
        this.Scheme = data[1];
        this.TSP = data[0];
          this.UsersSessionDate = new MatTableDataSource(data[2]);
          
        },
          error => {
            this.error = error;
          })
  }

  DeleteSessionOfRow(r) {

    var Schemeid = r.SchemeID;
    var Tspid = r.TSPID;
    var SessionID = r.SessionID;
    let titleConfirm = "Confirmation";
    let messageConfirm = "Are you want to remove the active session?";
    this.http.confirm(titleConfirm, messageConfirm).subscribe(
      (isConfirm: Boolean) => {
        if (isConfirm) {
          this.http.getJSON(`api/SkillsScholarshipInitiative/DeleteSession/` + this.filters.SchemeID + `/` + Tspid + `/` + SessionID).subscribe(
            (d: any) => {
              if (d) {
                this.getUsersSessionData();
              }
            },
            (error) => {
              this.error = error.error;
              this.http.ShowError(error.error + '\n' + error.message, '', 5000);
            } // error path
          );
        }
      }
    );

  }

}

export interface ISkillscholarshipFilter {
  SchemeID: number;
  TSPID: number;
  Locality: number;
  Cluster: number;
  District: number;
 }
