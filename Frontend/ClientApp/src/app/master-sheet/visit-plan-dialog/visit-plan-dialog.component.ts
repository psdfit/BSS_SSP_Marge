import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { IVisitPlan } from '../Interface/IVisitPlan';
import { Component, OnInit, ViewChild, Inject, ChangeDetectorRef } from '@angular/core';
import { environment } from '../../../environments/environment';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { FormGroup, FormBuilder, Validators, FormGroupDirective, AbstractControl, NgForm, FormControl } from '@angular/forms';
import { CommonSrvService } from '../../common-srv.service';
import { ModelBase } from '../../shared/ModelBase';
import { UserRightsModel, UsersModel } from '../../master-data/users/users.component';
import * as moment from 'moment';

import { EnumCertificationAuthority, EnumTraineeResultStatusTypes, EnumUserLevel, ExportType, EnumTraineeStatusType, EnumClassStatus, EnumExcelReportType } from '../../shared/Enumerations';

import { DialogueService } from '../../shared/dialogue.service';
import { ExportExcel } from '../../shared/Interfaces';
import { GroupByPipe } from 'angular-pipes';


@Component({
    selector: 'visit-plan-dialog',
    templateUrl: './visit-plan-dialog.component.html',
    styleUrls: ['./visit-plan-dialog.component.scss'],
    providers: [GroupByPipe]

})
export class VisitPlanDialogComponent implements OnInit {
    visitplanform: FormGroup;
    formrights: UserRightsModel;
    title: string; savebtn: string;
    classObj: any;
    error: String;
    success: String;
    EnText: string = "Visit Plan";
    checked: boolean = false;
    checked2: boolean = true;
    isOpenChange: boolean = false;
    IsMasterSheet: boolean = false;
    enableRichText: boolean = false;
    RegionFilter: boolean = false;
    TSPUnAccess: boolean = false;
    minEndDate: Date;
    currentUser: UsersModel;
    userid: number;

    isOpenChangeMessage: string = "";
    working = false;
    viewing = false;
    disableClassFlag: boolean = false;
    disableClusterFlag: boolean = false;
    disableDistrictFlag: boolean = false;
    disableRadioFlag: boolean = false;
    enableSchemes: boolean = false;
    classestoshow = [];
    //filters: ICalendarFilter = { VisitStartDate: "H-M-Y", ClassID: 0, TSPID: 0, TraineeID: 0 };

    displayedColumns = [
        'VisitTypeName',
        //'UserID',
        //'ClassID',
        'VisitStartDate',
        'VisitEndDate',
        'VisitStartTime',
        'VisitEndTime',
        'UserStatus',
        //'InActive',
        "Action"];
    visitplan: MatTableDataSource<any>;
    visitplanreportdata: MatTableDataSource<any>;
    Users; UserSelected = []; classes; clusters; districts; schemes; regions; eventusers = [];
    eventclasses = []; eventschemes = []; filteredusers = []; filteredclasses = [];
    query = {
        order: 'VisitPlanID',
        limit: 5,
        page: 1
    };

    @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
    @ViewChild(MatSort, { static: false }) sort: MatSort;
    constructor(
        private fb: FormBuilder,
        private ComSrv: CommonSrvService,
        public dialogueService: DialogueService,
        private groupByPipe: GroupByPipe,
        public dialogRef: MatDialogRef<VisitPlanDialogComponent>,
        @Inject(MAT_DIALOG_DATA) public data: any, private cdr: ChangeDetectorRef) {
        this.visitplanform = this.fb.group({
            VisitPlanID: 0,
            VisitType: ["", Validators.required],
            UserIDs: ["", Validators.required],
            RegionID: [0, Validators.required],
            ClusterID: ["", Validators.required],
            DistrictID: ["", Validators.required],
            VisitStartDate: ["", Validators.required],
            VisitEndDate: ["", Validators.required],
            VisitStartTime: ["", Validators.required],
            VisitEndTime: ["", Validators.required],
            Attachment: ["", Validators.required],
            Comments: "",
            Venue: "",
            LinkWithCRM: 0,
            ClassIDs: "",
            ClassCode: "",
            SchemeIDs: [0, Validators.required],
            IsVisited: 0,
            editor: new FormControl(null),
            //editor: null,
            CreatedUserID: 0
            //InActive: ""
        });
        this.visitplan = new MatTableDataSource([]);
        this.formrights = ComSrv.getFormRights();
    }
    triggerChangeDetection() {
        this.cdr.detectChanges()
    }
    ngOnInit() {
        let classId = this.data.ClassID;
        let userlevel = this.data.level;
        let dt = this.data.VisitStartDate;
        let user = this.data.UserID;
        let visiplanid = this.data.VisitPlanID;

        this.currentUser = this.ComSrv.getUserDetails();
        this.userid = this.currentUser.UserID;

        if (this.currentUser.UserLevel == 4) {
            this.TSPUnAccess = true
            this.visitplanform.disable({ onlySelf: true })
        } if (this.currentUser.UserLevel == 1 || this.currentUser.UserLevel == 2) {
            this.displayedColumns = [
                'GenerateReport',
                'VisitTypeName',
                'VisitStartDate',
                'VisitEndDate',
                'VisitStartTime',
                'VisitEndTime',
                'InActive',
                "Action"
            ]
        }

        this.savebtn = "Save ";
        //if (this.data.ClassID) {
        //    this.ClassID.setValue(this.data.ClassID);
        //}


        if (userlevel == 3) {
            this.GetTPMVisitPlanData(userlevel);
        }
        if (classId) {
            this.GetVisitPlanData(classId);
        }
        if (!this.data.IsMasterSheet && userlevel != 3) {
            this.GetVisitPlanCalendarData(dt, user);
        }


        if (this.data.ClassID) {
            this.ClassIDs.setValue(this.data.ClassID);

            this.IsMasterSheet = this.data.IsMasterSheet;
            this.VisitStartDate.setValue(this.data.VisitStartDate);
        }
        this.IsMasterSheet = this.data.IsMasterSheet;
        this.VisitStartDate.setValue(this.data.VisitStartDate);
        this.minEndDate = this.data.VisitStartDate;

    }

    onNoClick(): void {
        this.dialogRef.close(false);
    }

    updateVisiting(event: any, id: number) {
        if (event) {
            this.ComSrv.postJSON('api/VisitPlan/UpdateUserEventStatus', { 'VisitPlanID': id, "UserStatus": event, "UserID": this.data.UserID })
                .subscribe((d: any) => {


                    this.success = "Your Visiting Status has been saved";
                    this.ComSrv.openSnackBar(this.success.toString(), "Success");
                    //this.ComSrv.openSnackBar(row.InActive == true ? environment.InActiveMSG.replace("${Name}", this.EnText) : environment.ActiveMSG.replace("${Name}", this.EnText));
                    // this.visitplan =new MatTableDataSource(d);
                },
                    error => this.error = error // error path
                );
        }
        console.log("YEs")
    }
    getUserReportDate(id: number) {
        this.ComSrv.getJSON('api/VisitPlan/GetUserEventReportData/' + id)
            .subscribe((d: any) => {
                this.visitplanreportdata = d
                this.exportToExcel();
            },
                error => this.error = error // error path
            );
    }


    exportToExcel() {

        let data = {
            //"Filters:": '',
            //"Scheme(s)": '',//this.groupByPipe.transform(filteredData, 'Scheme').map(x => x.key).join(','),
            //"TSP(s)": '',//this.groupByPipe.transform(filteredData, 'TSP').map(x => x.key).join(','),
            //"TraineeImagesAdded": true
        };



        let exportExcel: ExportExcel = {
            Title: 'Visit Plan Report',
            Author: this.currentUser.FullName,
            Type: EnumExcelReportType.VisitPlan,
            Data: data,
            List1: this.populateData(this.visitplanreportdata),
        };
        this.dialogueService.openExportConfirmDialogue(exportExcel).subscribe();
    }


    populateData(data: any) {
        if (data[0].ClusterName) {
            return data.map(item => {
                return {
                    //"Scheme Code": item.SchemeCode

                    "TSP": item.TSPName
                    , "District": item.DistrictName
                    , "Cluster": item.ClusterName
                    , "User Visitng Status": item.UserStatus
                    , "TSP Participating Status By Call Center Agent": item.UserStatusByCallCenter
                    , "Contact Number": item.CPLandline
                    , "Name of Head Admissions": item.HeadName
                    , "Contact Person Admissions Name": item.CPAdmissionsName
                    , "Nominated Person Name": item.NominatedPersonName
                    , "Nominated Person Contact Number": item.NominatedPersonContactNumber
                    //, "Trade Code": item.TradeCode


                }
            })

        }
        else {
            return data.map(item => {
                return {
                    //"Scheme Code": item.SchemeCode

                    "TSP": item.TSPName
                    , "District": item.DistrictName
                    , "Region": item.RegionName
                    , "User Visitng Status": item.UserStatus
                    , "TSP Participating Status By Call Center Agent": item.UserStatusByCallCenter
                    , "Contact Number": item.CPLandline
                    , "Name of Head Admissions": item.HeadName
                    , "Contact Person Admissions Name": item.CPAdmissionsName
                    , "Nominated Person Name": item.NominatedPersonName
                    , "Nominated Person Contact Number": item.NominatedPersonContactNumber
                    //, "Trade Code": item.TradeCode


                }
            })



        }




    }

    radioChange(event) {
        if (event.value == "1") {
            this.checked = true;
            this.checked2 = false;
            this.ClusterID.setValue(0);
            this.RegionFilter = true
            //this.TradeCode.reset();
        }
        if (event.value == "2") {
            this.checked2 = true;
            this.checked = false;
            this.RegionID.setValue(0);
            this.RegionFilter = false

            //this.TradeCode.reset();
        }
    }

    GetTSPUsersByScheme(event: any) {

        if (event.length > 1) {
            event = event.value.join(',')
        }
        if (this.VisitType.value == "4" || this.VisitType.value == "5") {

            this.ComSrv.getJSON('api/VisitPlan/GetTSPUsersByScheme/', event.value).subscribe((response: any) => {
                //let VisitPlanData = <VisitPlanModel>response[0];
                this.UserSelected = response[0];
            });
        } else {
            return;

        }


    }

    GetVisitPlanData(ClassID) {
        this.ComSrv.getJSON('api/VisitPlan/GetVisitPlanBy/', ClassID).subscribe((response: any) => {
            //let VisitPlanData = <VisitPlanModel>response[0];
            this.visitplan = new MatTableDataSource(response[0]);
            //this.classObj = response[1];
            this.Users = response[2];
            this.classes = response[3];
            this.eventusers = response[4];
            this.eventclasses = response[5];
            this.clusters = response[6];
            this.districts = response[7];
            this.schemes = response[8];
            this.regions = response[9];
            this.eventschemes = response[10];

            this.visitplan.paginator = this.paginator;
            this.visitplan.sort = this.sort;
        });
    }

    GetVisitPlanCalendarData(dt: Date, user) {
        this.ComSrv.postJSON('api/VisitPlan/GetVisitPlanByDate/', { "VisitStartDate": dt.toISOString(), "UserID": user }).subscribe((response: any) => {
            //let VisitPlanData = <VisitPlanModel>response[0];
            this.visitplan = new MatTableDataSource(response[0]);
            //this.classObj = response[1];
            this.Users = response[1];
            this.classes = response[2];
            this.eventusers = response[3];
            this.eventclasses = response[4];
            this.clusters = response[5];
            this.districts = response[6];
            this.schemes = response[7];
            this.regions = response[8];
            this.eventschemes = response[9];
            this.visitplan.paginator = this.paginator;
            this.visitplan.sort = this.sort;
        });

  }


  GetVisitsByUser(dt: Date, user) {
    this.ComSrv.postJSON('api/VisitPlan/GetVisitsByUser/', { "VisitStartDate": dt, "UserID": user }).subscribe((response: any) => {
      this.visitplan = new MatTableDataSource(response[0]);

      this.visitplan.paginator = this.paginator;
      this.visitplan.sort = this.sort;
    });

  }


    GetTPMVisitPlanData(level) {
        this.ComSrv.getJSON('api/VisitPlan/GetTPMVisitPlanBy/', level).subscribe((response: any) => {
            //let VisitPlanData = <VisitPlanModel>response[0];
            this.visitplan = new MatTableDataSource(response[0]);
            console.log(this.visitplan.data);
            this.classes = response[1];
            this.Users = response[2];
            this.clusters = response[3];
            this.districts = response[4];

            this.visitplanform.disable({ onlySelf: true })
        });
    }

    updateSchemeControl(control: AbstractControl) {
        control.setValidators([Validators.required]);
        control.updateValueAndValidity();

    }

    FilterUsers(event: any) {
        if (event.value) {
            if (Number(event.value) == 5) {
                this.UserSelected = this.Users.filter(subsec => subsec.UserLevel === 4);
                this.enableRichText = true;
                this.enableSchemes = false;
                this.disableClassFlag = false;
                this.disableClusterFlag = false;
                this.disableDistrictFlag = false;
                this.disableRadioFlag = false;
                this.ClassIDs.setValue([0]);
                //this.updateSchemeControl(this.SchemeIDs);

            }
            else {
                var userlevel = Number(event.value);
                this.UserSelected = this.Users.filter(subsec => subsec.UserLevel === userlevel);
                this.enableRichText = false;
                //this.classestoshow = this.classes;
                if (event.value == 4) {
                    //this.Users = this.Users.filter(subsec => subsec.UserLevel === 4);
                    this.ClassIDs.setValue([0]);
                    this.data.ClassID = 0;
                    this.disableClassFlag = true;
                    //this.disableClusterFlag = true;
                    //this.disableDistrictFlag = true;
                    //this.disableRadioFlag = true;
                    this.enableSchemes = true;
                }
                else {
                    this.enableSchemes = false;
                    this.disableClassFlag = false;
                    this.disableClusterFlag = false;
                    this.disableDistrictFlag = false;
                    this.disableRadioFlag = false;
                    this.SchemeIDs.reset();



                }
                //this.Users = this.UserSelected;
                console.log(this.Users)
            }

        }

    }


    ChekTimeDifference() {
        if (this.VisitStartTime.invalid || this.VisitEndTime.invalid || this.VisitStartTime.value == null || this.VisitEndTime.value == null) {
            return;
        }

        var Stime = this.VisitStartTime.value.split(":");
        var SMtime = this.VisitStartTime.value.split(" ");
        var SM = SMtime[1];
        var IStime = Stime[0];
        var FStime = Number(IStime);



        var Etime = this.VisitEndTime.value.split(":");
        var EMtime = this.VisitEndTime.value.split(" ");
        var EM = EMtime[1];
        var IEtime = Etime[0];
        var FEtime = Number(IEtime);


        if (FStime > FEtime) {
            this.VisitEndTime.reset();
            this.error = "Class End Time Should be Greater than Start Time";
            this.ComSrv.ShowError(this.error.toString(), "Error");
            return false;
        }
        else {
            return;
        }

    }



    Submit() {
        this.visitplanform.value["CreatedUserID"] = this.currentUser.UserID;
        //CreatedUserID.setValue(this.currentUser.UserID);
        //if (!this.visitplanform.valid)
        //  return;
        //this.visitplanform.patchValue({ ClassID: this.data.ClassID })
        //this.filteredusers = this.eventusers.filter(subsec => subsec.UserID === this.UserID.value);
        this.filteredusers = this.eventusers.filter(x => this.UserIDs.value.map(y => y).includes(x.UserID));
        this.visitplanform.value["EventUsers"] = this.filteredusers;
        if (this.data.ClassID) {
            this.filteredclasses = this.eventclasses.filter(c => c.ClassID === this.data.ClassID)
            //if (this.SchemeIDs.value == null) {
            //    this.SchemeIDs.setValue(0);
            //}
        } else {
            this.filteredclasses = this.eventclasses.filter(a => this.ClassIDs.value.map(b => b).includes(a.ClassID));
        }

        this.visitplanform.value["EventSchemes"] = this.eventschemes.filter(a => this.SchemeIDs.value.map(b => b).includes(a.SchemeID));
        this.visitplanform.value["EventClasses"] = this.filteredclasses;
        //this.visitplanform.value["EventUsers"] = this.UserID.value;

        if (!this.visitplanform.valid)
            return;

        this.ComSrv.postJSON('api/VisitPlan/Save', this.visitplanform.value)
            .subscribe((d: any) => {
                this.visitplan = new MatTableDataSource(d);
                this.visitplan.paginator = this.paginator;
                this.visitplan.sort = this.sort;
                this.ComSrv.openSnackBar(this.VisitPlanID.value > 0 ? environment.UpdateMSG.replace("${Name}", this.EnText) : environment.SaveMSG.replace("${Name}", this.EnText));
                this.reset();
                //this.title = "Add New ";
                //this.savebtn = "Save ";
            },
            (error) => {
                this.error = error.error;
                this.working = false;
                this.ComSrv.ShowError(error.error);
                
              });
    }
    setTime(value) {
        let time = value;
    }
    reset() {
        //this.visitplanform.reset('UserID');
        //myform.resetFrom();
        this.VisitPlanID.setValue(0);
        //this.VisitPlanID.reset();
        this.VisitType.reset();
        this.UserIDs.reset();
        this.disableClassFlag = false;
        if (this.IsMasterSheet) {
            this.VisitStartDate.reset();
        }
        //this.VisitStartDate.setValue(this.data.VisitStartDate);
        this.VisitEndDate.reset();
        this.ClassIDs.reset();
        this.VisitStartTime.reset();
        this.VisitEndTime.reset();
        this.Attachment.reset();
        this.Comments.reset();
        this.Venue.reset();
        this.RegionID.setValue(0);
        this.ClusterID.setValue(0);
        this.DistrictID.setValue(0);
        this.SchemeIDs.setValue(0);
        this.LinkWithCRM.setValue(0);
        this.editor.reset();

        this.title = "Add New ";
        this.savebtn = "Save ";
    }



    toggleEdit(row) {
        //this.disableClassFlag = true;
        this.viewing = true;

        if (row.IsVisited == true) {
            this.visitplanform.disable({ onlySelf: true })
            this.isOpenChange = true;
            this.isOpenChangeMessage = "(Already Visited)"
        }
        this.VisitPlanID.setValue(row.VisitPlanID);
        this.VisitType.setValue(row.VisitType);
        //this.UserIDs.setValue(row.UserID);
        //this.ClassIDs.setValue(row.ClassID);
        this.VisitEndTime.setValue(moment(row.VisitEndTime).format('HH:mm').toString());
        this.VisitStartTime.setValue(moment(row.VisitStartTime).format('HH:mm').toString());

        this.VisitStartDate.setValue(row.VisitStartDate);
        this.VisitEndDate.setValue(row.VisitEndDate);
        this.Venue.setValue(row.Venue);
        this.Attachment.setValue(row.Attachment);
        this.Comments.setValue(row.Comments);
        this.Venue.setValue(row.Venue);
        //this.InActive.setValue(row.InActive);
        this.RegionID.setValue(row.RegionID);
        this.ClusterID.setValue(row.ClusterID);
        this.DistrictID.setValue(row.DistrictID);
        //this.SchemeIDs.setValue(row.SchemeIDs);
        this.LinkWithCRM.setValue(row.LinkWithCRM);

        if (row.ClusterID == 0) {
            this.checked = true;
            this.checked2 = false
            this.RegionFilter = true
        }
        else {
            this.checked2 = true;
            this.checked = false;
            this.RegionFilter = false

        }


        this.UserSelected = this.Users.filter(subsec => subsec.UserLevel === Number(row.VisitType));


        this.ComSrv.getJSON('api/VisitPlan/GetEventMappedData/' + row.VisitPlanID).subscribe((REs: any) => {
            //this.UserIDs.setValue(REs[0].UserID);
            let du = REs[0].map(function (val) {
                return val.UserID;
            });

            let Cu = REs[1].map(function (val) {
                return val.ClassID;
            });

            let Su = REs[2].map(function (val) {
                return val.SchemeID;
            });

            this.UserIDs.setValue(du);
            this.ClassIDs.setValue(Cu);
            this.SchemeIDs.setValue(Su);
            //this.UserIDs = REs;
            //this.UserIDs = REs;

            //let Ue = REs[0].map(function (val) {
            //    this.UserIDs.setValue(REs[0]);

            //    return val.UserID;


            //});

        }, error => this.error = error // error path
        )

        this.title = "Update ";
        this.savebtn = "Save ";
    }
    toggleActive(row) {
      this.ComSrv.confirmEventCancelStatus().subscribe(result => {
            if (result) {
                this.ComSrv.postJSON('api/VisitPlan/ActiveInActive', { 'VisitPlanID': row.VisitPlanID, 'InActive': row.InActive })
                    .subscribe((d: any) => {
                      if (d) {
                        this.GetVisitsByUser(row.VisitStartDate, this.data.UserID);
                      }
                      this.ComSrv.openSnackBar(row.InActive == true ? environment.CancelMSG.replace("${Name}", this.EnText) : environment.ActiveMSG.replace("${Name}", this.EnText));


                        // this.visitplan =new MatTableDataSource(d);
                    },
                        error => this.error = error // error path
                    );
            }
            else {
                row.InActive = !row.InActive;
            }
        });
    }
    applyFilter(filterValue: string) {
        filterValue = filterValue.trim(); // Remove whitespace
        filterValue = filterValue.toLowerCase(); // Datasource defaults to lowercase matches
        this.visitplan.filter = filterValue;
    }

    get VisitPlanID() { return this.visitplanform.get("VisitPlanID"); }
    get VisitType() { return this.visitplanform.get("VisitType"); }
    get UserIDs() { return this.visitplanform.get("UserIDs"); }
    get VisitStartDate() { return this.visitplanform.get("VisitStartDate"); }
    get VisitEndDate() { return this.visitplanform.get("VisitEndDate"); }
    get VisitStartTime() { return this.visitplanform.get("VisitStartTime"); }
    get VisitEndTime() { return this.visitplanform.get("VisitEndTime"); }
    get Attachment() { return this.visitplanform.get("Attachment"); }
    get Comments() { return this.visitplanform.get("Comments"); }
    get Venue() { return this.visitplanform.get("Venue"); }
    get LinkWithCRM() { return this.visitplanform.get("LinkWithCRM"); }
    get RegionID() { return this.visitplanform.get("RegionID"); }
    get ClusterID() { return this.visitplanform.get("ClusterID"); }
    get DistrictID() { return this.visitplanform.get("DistrictID"); }
    get SchemeIDs() { return this.visitplanform.get("SchemeIDs"); }
    get InActive() { return this.visitplanform.get("InActive"); }
    get ClassIDs() { return this.visitplanform.get("ClassIDs"); }
    get ClassCode() { return this.visitplanform.get("ClassCode"); }
    get IsVisited() { return this.visitplanform.get("IsVisited"); }
    get editor() { return this.visitplanform.get("editor"); }
    //get UserStatus() { return this.visitplanform.get("UserStatus"); }
}

export class VisitPlanModel extends ModelBase {
    VisitPlanID: number;
    VisitType: string;
    UserID: number;
    VisitStartDate: Date;
    VisitStartTime: string;
    VisitEndTime: string;
    Attachment: string;
    Comments: string;
    Venue: string;
    //UserStatus: string;
    ClassID: number;
    Classcode: string;
    editor: string;
    IsVisited: boolean;
    CreatedUserID: number;
}
