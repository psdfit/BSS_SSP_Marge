import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { CommonSrvService } from '../../common-srv.service';

import { Element } from '@angular/compiler/src/render3/r3_ast';
import { MatProgressBar } from '@angular/material/progress-bar';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { FormGroup, FormBuilder, FormControl, Validators, FormGroupDirective } from '@angular/forms';
import { environment } from '../../../environments/environment';
import { ModelBase } from '../../shared/ModelBase';
import { UserRightsModel } from '../../master-data/users/users.component';
import { ActivatedRoute } from '@angular/router';
import * as moment from 'moment';
import { IOrgConfig } from '../../registration/Interface/IOrgConfig';
import * as XLSX from 'xlsx';
import { ITraineeProfile } from '../../registration/Interface/ITraineeProfile';
import { EnumCertificationAuthority, EnumTraineeResultStatusTypes, EnumUserLevel, ExportType, EnumTraineeStatusType, EnumClassStatus, EnumExcelReportType } from '../../shared/Enumerations';

import { DialogueService } from '../../shared/dialogue.service';
import { ExportExcel } from '../../shared/Interfaces';
import { GroupByPipe } from 'angular-pipes'
import { MatDialog } from '@angular/material/dialog';
import { UsersModel } from '../../master-data/users/users.component';
import { DatePipe } from '@angular/common';


@Component({
    selector: 'hrapp-tsp-ntps',
    templateUrl: './tsp-ntps.component.html',
    styleUrls: ['./tsp-ntps.component.scss'],
    providers: [GroupByPipe, DatePipe]

})

export class TSPNTPsComponent implements OnInit {
    pbteform: FormGroup;
    title: string; savebtn: string;
  //=====================Az=============================
  SearchSch = new FormControl('',);
  SearchCls = new FormControl('',);
  SearchTSP = new FormControl('',);
  SearchStatus = new FormControl('',);
  //====================================================
    displayedColumnsClasses = ['Training_Scheme', 'TSPName', 'Date_Of_Issuance', 'ClassCode', 'Trade',
        'Number_Of_Trainees', 'Duration','Curriculum', 'Address_Of_Training_Location'
      , 'Comments'];

    tspntps: MatTableDataSource<any>;
    pbteTSPs: MatTableDataSource<any>;
    pbteTrainees: MatTableDataSource<any>;
    pbteDropOutTrainees: MatTableDataSource<any>;

    selectedClasses: any;
    selectedTrainees: any;
    selectedTSPs: any;
  ParaUserID: any;
    data: any;

    traineeResultStatusTypes: any;

    update: String;
    userid: Number;

    disableExport: boolean;

  //=========================Az==========================
  filters: INTPApprovalFilter = { SchemeID: 0, ClassID: 0, TSPID: 0, StatusID:0 };


  schemes: [];

  SchemeFilter = [];
  TSPDetailFilter = [];
  classesArrayFilter: any[];
  StatusFilter = [];
  isInternalUser = false;
  isTSPUser = false;
  //========================================================

    isOpenRegistration: boolean = false;
    isOpenRegistrationMessage: string = "";
    formrights: UserRightsModel;
    currentUser: UsersModel;
    EnText: string = "TPMRTP";
    error: String;
    query = {
        order: 'IncepReportID',
        limit: 5,
        page: 1
    };
    @ViewChild(MatPaginator) paginator: MatPaginator;
    @ViewChild(MatSort) sort: MatSort;


    @ViewChild('SortTrainee') SortTrainee: MatSort;
    @ViewChild('PageTrainee') PageTrainee: MatPaginator;
    @ViewChild('SortDropOutTrainee') SortDropOutTrainee: MatSort;
    @ViewChild('PageDropOutTrainee') PageDropOutTrainee: MatPaginator;
    @ViewChild('SortTSP') SortTSP: MatSort;
    @ViewChild('PageTSP') PageTSP: MatPaginator;
    @ViewChild('SortClass') SortClass: MatSort;
    @ViewChild('PageClass') PageClass: MatPaginator;

    working: boolean;
    constructor(private fb: FormBuilder, private ComSrv: CommonSrvService, private route: ActivatedRoute,
        public dialog: MatDialog,
        public dialogueService: DialogueService,
        private groupByPipe: GroupByPipe,
        private _date: DatePipe) {
        this.pbteform = this.fb.group({
            IncepReportID: 0,
            FinalSubmitted: 0,
            SectionID: ["", Validators.required],
            InActive: ""
        }, { updateOn: "blur" });
        this.pbteTrainees = new MatTableDataSource([]);
        this.formrights = ComSrv.getFormRights("/pbte");
    }

  //======================Az==================================
  getbyTSPSchemes() {
    this.ComSrv.getJSON('api/Scheme/GetSchemeByUser/', this.userid).subscribe(
      (d: any) => {
        this.SchemeFilter = d;
        //if (this.Schemes[0].PTypeName == "Community") {
        //  this.hideTilesForCommmunityTSP = true;
        //}
      }, error => this.error = error
    );
  }

  getSchemes() {
    this.ComSrv.getJSON('api/Scheme/RD_Scheme').subscribe(
      (d: any) => {
        this.SchemeFilter = d;
        //if (this.Schemes[0].PTypeName == "Community") {
        //  this.hideTilesForCommmunityTSP = true;
        //}
      }, error => this.error = error
    );
  }
  getTSPDetailByScheme(schemeId: number) {
    this.classesArrayFilter = [];
    this.ComSrv.getJSON(`api/TSPDetail/GetTSPDetailByScheme/` + schemeId)
      .subscribe(data => {
        this.TSPDetailFilter = <any[]>data;
      }, error => {
        this.error = error;
      })
  }
  getClassesByTsp(tspId: number) {
    this.ComSrv.getJSON(`api/Class/GetClassesByTsp/` + tspId)
      .subscribe(data => {
        this.classesArrayFilter = <any[]>data;
      }, error => {
        this.error = error;
      })
  }
  //==========================================================
  GetData() {
    
    if (this.isTSPUser == true) {
      this.ParaUserID = this.userid;
    }
    else { this.ParaUserID =0}
    this.ComSrv.postJSON('api/RTP/GetRTPByTSP', { UserID: this.ParaUserID, SchemeID: this.filters.SchemeID, TSPID: this.filters.TSPID, ClassID: this.filters.ClassID, StatusID:this.filters.StatusID}).subscribe((d: any) => {

      this.tspntps = new MatTableDataSource(d[0]);
      //this.SchemeFilter = d[1];
      this.tspntps.paginator = this.PageClass;
      this.tspntps.sort = this.SortClass;
    },
      error => this.error = error // error path
      , () => {
        this.working = false;
      });
  }
    //GetData() {
    //  this.ComSrv.getJSON('api/RTP/GetRTPByTSP', { UserID: this.userid, SchemeID: this.filters.SchemeID, TSPID: this.filters.TSPID, ClassID: this.filters.ClassID }).subscribe((d: any) => {
    //        this.tspntps = new MatTableDataSource(d[0]);
    //        if (this.tspntps.filteredData.length == 0) {
    //            this.disableExport = true;
    //        }
    //        this.tspntps.paginator = this.PageClass;
    //        this.tspntps.sort = this.SortClass;

    //    }, error => this.error = error// error path
    //    );
    //};

    ngOnInit() {
        this.ComSrv.setTitle("Notice to Proceed");
        this.title = "Add New ";
        this.savebtn = "Save ";
        this.currentUser = this.ComSrv.getUserDetails();
        this.userid = this.currentUser.UserID;
      if (this.currentUser.UserLevel === EnumUserLevel.TSP) {
        this.isTSPUser = true;
        this.getbyTSPSchemes();
      } else if (this.currentUser.UserLevel === EnumUserLevel.AdminGroup || this.currentUser.UserLevel === EnumUserLevel.OrganizationGroup) {
        this.isInternalUser = true;
        this.getSchemes();
      }

      
      this.GetData();
  }

  EmptyCtrl() {
    this.SearchCls.setValue('');
    this.SearchTSP.setValue('');
    this.SearchSch.setValue('');
    this.SearchStatus.setValue('');
  }

    exportToExcel(name?: string) {
        let filteredData = [...this.tspntps.filteredData]
        //let removeKeys = Object.keys(filteredData[0]).filter(x => !this.displayedColumns.includes(x));
        //let data = [];//filteredData.map(x => { removeKeys.forEach(key => delete x[key]); return x });
        //filteredData.forEach(item => {
        //    let obj = {};
        //    this.displayedColumns.forEach(key => {
        //        obj[key] = item[key] || "";
        //    });
        //    data.push(obj)
        //})

        let data = {
            "Training Scheme(s)": this.groupByPipe.transform(filteredData, 'SchemeName').map(x => x.key).join(','),
            "Name of Training Service Provider(s)": this.groupByPipe.transform(filteredData, 'TSPName').map(x => x.key).join(','),
            
            //"TraineeImagesAdded": true
        };



        let exportExcel: ExportExcel = {
            Title: 'NTP Report',
            Author: this.currentUser.FullName,
            Type: EnumExcelReportType.NTP,
            Data: data,
            List1: this.populateData(filteredData),
        };
        this.dialogueService.openExportConfirmDialogue(exportExcel).subscribe();
    }


    populateData(data: any) {
        return data.map(item => {
            return {
                "Class Code": item.ClassCode
                , "Trade": item.TradeName
                , "Date of Issuance(dd-mm-yyyy)": this._date.transform(item.ModifiedDate, 'dd-MM-yyyy')
                , "Number Of Trainees": item.TraineesPerClass
                , "Duration (In Months)": item.Duration
                , "Curriculum Followed": item.Name
                , "Address of Training Location": item.AddressOfTrainingLocation
              , "Comments": item.Comments
                            

            }
        })
    }






    //toggleActive(row) {
    //    this.ComSrv.confirm().subscribe(result => {
    //        if (result) {
    //            this.ComSrv.postJSON('api/InceptionReport/ActiveInActive', { 'IncepReportID': row.IncepReportID, 'InActive': row.InActive })
    //                .subscribe((d: any) => {
    //                    this.ComSrv.openSnackBar(row.InActive == true ? environment.InActiveMSG.replace("${Name}", this.EnText) : environment.ActiveMSG.replace("${Name}", this.EnText));
    //                    // this.inceptionreport =new MatTableDataSource(d);
    //                },
    //                    error => this.error = error // error path
    //                );
    //        }
    //        else {
    //            row.InActive = !row.InActive;
    //        }
    //    });
    //}
    //applyFilter(filterValue: string) {
    //    filterValue = filterValue.trim(); // Remove whitespace
    //    filterValue = filterValue.toLowerCase(); // Datasource defaults to lowercase matches
    //}


    get InActive() { return this.pbteform.get("InActive"); }

}
//==========================Az==============================

export interface INTPApprovalFilter {
  SchemeID: number;
  TSPID: number;
  ClassID: number;
  StatusID: number;
}

//===========================================================
