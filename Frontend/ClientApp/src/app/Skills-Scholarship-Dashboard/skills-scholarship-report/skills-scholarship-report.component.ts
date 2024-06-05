import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { CommonSrvService } from '../../common-srv.service';
import { MatDialog } from '@angular/material/dialog';

import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { FormGroup, FormBuilder, Validators, FormControl } from '@angular/forms';
import { environment } from '../../../environments/environment';
//import { ModelBase } from '../../shared/ModelBase';
import { UserRightsModel } from '../../master-data/users/users.component';
import { UsersModel } from '../../master-data/users/users.component';
import { EnumUserLevel, EnumExcelReportType } from '../../shared/Enumerations';

import { DialogueService } from '../../shared/dialogue.service';
//import { ExportExcel } from '../../shared/Interfaces';
//import { GroupByPipe } from 'angular-pipes';
import { DatePipe } from '@angular/common';
//import { merge } from 'rxjs';
//import { startWith, switchMap } from 'rxjs/operators';

@Component({
  selector: 'app-skills-scholarship-report',
  templateUrl: './skills-scholarship-report.component.html',
  styleUrls: ['./skills-scholarship-report.component.scss']
})
export class SkillsScholarshipReportComponent implements OnInit {
  environment = environment;
  SearchSch = new FormControl('',);
  title: string;
  SessionControl: boolean = true;
  buttonTitle: string = 'Session Control View';


  hideColumn = true;
  displayedColumns: string[] = ['TradeName', 'TradeTarget', 'EnrolmentsCompleted', 'OverallEnrolments', 'RemainingSeats'];
  
  updateColumnVisibility() {
    // Update hideColumn based on your condition
   
      this.hideColumn = false;

    this.displayedColumns = ['Trade', 'NoOfAssociate', 'Target', 'Enrollments', 'Remaining', 'Completed', 'RaceStatus'];
  }

  enumUserLevel = EnumUserLevel;
  SkillsScholarshipArray: MatTableDataSource<any>;
  SkillsScholarshipReport: MatTableDataSource<any>;
  UsersSessionDate: MatTableDataSource<any>;
  Scheme = [];
  TSP = [];
  Cluster = [];
  selectedClusterName: string;

  formrights: UserRightsModel;
  EnText = 'Skills-Scholarship-Initiative Report';
  error: string;
  query = {
    order: 'ID',
    limit: 10,
    page: 1
  };
  // Some array of things.
  filters: ISkillscholarshipFilter = { SchemeID: 0, TSPID: 0, Locality: 0, Cluster: 0 };
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
  LocalityFilter = new FormControl(1);
  ClusterFilter = new FormControl(2);
  constructor(
    private fb: FormBuilder,
    private http: CommonSrvService,
    private ComSrv: CommonSrvService,
    public dialog: MatDialog,
    public dialogueService: DialogueService
    //private _date: DatePipe
  )
  {
    console.log("ok");
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



  getSkillsScholarshipData() {
    if (this.isTSPUser) {
      this.ComSrv.getJSON(`api/SkillsScholarshipInitiative/GetFilteredSkillsScholarshipInitiativeReport/` + this.filters.SchemeID + `/` + this.filters.TSPID + `/` + this.filters.Locality + `/` + this.filters.Cluster)
        .subscribe((data: any) => {
          // this.mastersheet = new MatTableDataSource(data[0]); 
          this.Scheme = data[0];
          this.SkillsScholarshipArray = new MatTableDataSource(data[1]);
          //this.ComSrv.ExportToExcel(data, 'Name'); For Export to Excel
        },
          error => {
            this.error = error;
          })
     
    } else {
      this.ComSrv.getJSON(`api/SkillsScholarshipInitiative/GetFilteredSkillsScholarshipInitiativeReport/` + this.filters.SchemeID + `/` + this.filters.TSPID + `/` + this.filters.Locality + `/` + this.filters.Cluster)
        .subscribe((data: any) => {
          // this.mastersheet = new MatTableDataSource(data[0]);
          this.Scheme = data[1];
          this.SkillsScholarshipArray = new MatTableDataSource(data[2]);
        },
          error => {
            this.error = error;
          })
    }
  }

  getReport() {
    this.ComSrv.ExportToExcel(this.populateOrgData(this.SkillsScholarshipArray.filteredData), "Skills Scholarship Report");
  }

  populateOrgData(data: any) {
    return data
      //.filter((item) => item.ClassID != "0") for filter data
      .map((item, index) => ({
        'Sr#': index + 1,       
        'Trade': item.TradeName,
        'No. of Associate': item.NoOfAssociate,
        'Target': item.TradeTarget,        
        'Enrolments': item.OverallEnrolments,
        'Completed': item.EnrolmentsCompleted,
        'Remaining': item.RemainingSeats,
        '%age Completed': ((item.EnrolmentsCompleted / item.TradeTarget) * 100).toFixed(1), // Calculate and round to 2 decimal places
        'Race Status': item.RaceStopped == 1 && item.HasRaceStarted == 1 ? 'FCFS enrollment completed' : (item.HasRaceStarted == 1 && item.RaceStopped == 0 ? 'FCFS Enrollment in progress' : 'FCFS Enrollment not started'),
        'Cluster Name': item.ClusterName
      }));
  }

  getSelectedClusterName(): string {
    const selectedCluster = this.Cluster.find(c => c.ClusterID === this.filters.Cluster);
    return selectedCluster ? selectedCluster.ClusterName : '';
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
      this.filters.Locality = 0;
      this.filters.Cluster = 0;
      // this.getMasterSheet();
      // this.initPagedData();
    })
  }
}

export interface ISkillscholarshipFilter {
  SchemeID: number;
  TSPID: number;
  Locality: number;
  Cluster: number;
}
