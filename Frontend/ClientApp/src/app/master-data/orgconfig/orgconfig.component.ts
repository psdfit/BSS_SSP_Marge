/* **** Aamer Rehman Malik *****/
import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from '../../../environments/environment';
import { FormGroup, FormBuilder, Validators, FormGroupDirective, NgForm } from '@angular/forms';
import { CommonSrvService } from '../../common-srv.service';
import { UserRightsModel } from '../users/users.component';
import { ModelBase } from '../../shared/ModelBase';
import { ExportExcel } from '../../shared/Interfaces';
import { EnumUserLevel, EnumExcelReportType } from '../../shared/Enumerations';
import { DialogueService } from '../../shared/dialogue.service';
import { UsersModel } from '../../master-data/users/users.component';
@Component({
  selector: 'hrapp-orgconfig',
  templateUrl: './orgconfig.component.html',
  styleUrls: ['./orgconfig.component.scss']
})
export class OrgConfigComponent implements OnInit {
  title: string; savebtn: string;
  rights: UserRightsModel;
  objectKeys = Object.keys;
  EnText: string = "Organizations Configuration";
  error: String;
  OrgConfigs: OrgConfigModel[] = [];
  OrgCon: OrgConfigModel =new OrgConfigModel();
  Genders = [];
  Educations = [];
  programs = [];
  Organizations = [];
  OrganizationID: number;
  SchemeName = [];
  SID: any;
  TSPName = [];
  TID: any;
  ClassArray = [];
  CID: number;
  excelsheetArray: any[];
  BusinessRuleType: string;
  currentUser: UsersModel;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  working: boolean = false;
  constructor(private fb: FormBuilder, private http: CommonSrvService, public dialogueService: DialogueService) {
    
   this.rights = http.getFormRights();
  }

  GetData() {
    this.SID = "0";
    this.TID = "0";
    this.http.getJSON('api/OrgConfig/GetOrgConfig').subscribe((d: any) => {
      this.Genders = d[1];
      this.Organizations = d[0];
      this.Educations = d[2];
      this.programs = d[3];

      //this.SchemeName = d[4];
      //this.TSPName = d[4];
      //this.ClassArray = d[4];

    }, error => this.error = error // error path
    );

  }

  EmptyCtrl() {
    this.TID = 0;
    }

  GetDataScheme() {
    this.TID = "0";
    this.http.getJSON('api/OrgConfig/GetOrgConfigScheme/' + this.BusinessRuleType + '').subscribe((d: any) => {
      this.SchemeName = d[0];
      //this.GetConfig()
    }, error => this.error = error // error path
    );

  }

  GetDataTSP() {
    
    this.http.getJSON('api/OrgConfig/GetOrgConfigTSP/' + this.SID + '').subscribe((d: any) => {
      this.TSPName = d[0];
      this.TID = "0";
      this.GetConfig();
    }, error => this.error = error // error path
    );

  }

  GetDataClass() {
    this.http.getJSON('api/OrgConfig/GetOrgConfigClass/' + this.TID + '').subscribe((d: any) => {
      this.ClassArray = d[0];

    }, error => this.error = error // error path
    );

  }

  ngOnInit() {
    this.http.setTitle("Manage Organizations Configuration");
    this.title = "Add New ";
    this.savebtn = "Save ";
    this.GetData();
   }

  Submit(mform: NgForm) {
    if (!mform.valid)
      return;
    if (this.OrgConfigs.length == 0) {
      this.http.ShowError("No data to submit " + this.BusinessRuleType);
      return;
    }
    this.working = true;
    this.OrgConfigs[0].BusinessRuleTypeForGetPreviousList=this.BusinessRuleType
    this.http.postJSON('api/OrgConfig/Save', this.OrgConfigs)
      .subscribe((d: any) => {
        this.http.openSnackBar(environment.UpdateMSG.replace("${Name}", this.EnText));
        this.reset(mform);
        this.title = "Add New ";
        this.savebtn = "Save ";
        this.working = false;
      },
      (error) => {
        this.error = error.error;
        this.http.ShowError(error.error);
        
      });
  }
  reset(mform: NgForm) {
    mform.resetForm();
    this.title = "Add New ";
    this.savebtn = "Save ";
  }
  GetConfig() {
    if (this.OrganizationID > 0 && this.BusinessRuleType) {
      //this.http.getJSON('api/OrgConfig/GetOrgConfig/' + this.OrganizationID + "/" + this.BusinessRuleType + "/" + this.SID + "/" + this.TID + "/" + this.CID).subscribe((REs: any) => {
      this.http.getJSON('api/OrgConfig/GetOrgConfig/' + this.OrganizationID + "/" + this.BusinessRuleType + "/" + this.SID + "/" + this.TID).subscribe((REs: any) => {
      this.OrgConfigs = REs;
        if (this.OrgConfigs.length == 0)
          this.http.ShowWarning("No data found for " + this.BusinessRuleType);
      }, error => this.error = error // error path
      );
    }
    this.title = "Update ";
    this.savebtn = "Save ";
  }

 // ==============Excel Export============================
  getExcelExportData() {
    console.log('1');
    if (this.OrganizationID > 0 && this.BusinessRuleType) {
      console.log('2');
      this.http.getJSON('api/OrgConfig/GetOrgConfig/' + this.OrganizationID + "/" + this.BusinessRuleType + "/" + this.SID + "/" + this.TID).subscribe((REs: any) => {
        this.excelsheetArray = REs;

        const exportExcel: ExportExcel = {
          Title: 'Organizations Configuration',
            Author: "",
            Type: EnumExcelReportType.orgconfigration,
            Data: {},
          List1: this.populateOrgData(this.excelsheetArray)
          };
        this.dialogueService.openExportConfirmDialogue(exportExcel).subscribe();
        });
    }
  }
  populateOrgData(data: any) {
    return data
      .filter((item) => item.ClassID != "0")
      .map((item, index) => ({
        'Sr#': index + 1,
        'Scheme Name': item.SchemeName,
        'TSP Name': item.TSPName,
        'Class Code': item.ClassCode,
        'Dual Registration': item.DualRegistration,
        'Trainee Registration Bracket Days Before': item.BracketDaysBefore,
        'Trainee Registration Bracket Days After': item.BracketDaysAfter,
        'Eligible Gender': item.GenderName,
        'Minimum Age': item.MinAge,
        'Max Age': item.MaxAge,
        'Min Education': item.Education,
        'Inception Report Bracket Days Before': item.ReportBracketBefore,
        'Inception Report Bracket Days After': item.ReportBracketAfter,
        'Stipend Payment method': item.StipendPayMethod,
        'Class Start Day rule From Day': item.ClassStartFrom1,
        'Class Start Day rule To Day': item.ClassStartTo1,
        'Min.Attendance % & deduction(% Percentage)': item.MinAttendPercent,
        'Min.Attendance % & deduction(Amount)': item.StipendDeductAmount,
        '% Physical Count Min': item.PhyCountDeductPercent,
        '% Drop Out Deduction': item.DeductDropOutPercent,
        'Stipend Note Generation (greater than 1 month)': item.StipNoteGenGTMonth,
        'Stipend Note Generation (1 month or less)': item.StipNoteGenLTMonth,
        'Deduction Failed Trainees %': item.DeductionFailedTraineesPercent,
        'TCR OPENING DAYS': item.TSROpeningDays,
        'Employment Deadline': item.EmploymentDeadline,
        'Extra Trainees % age': item.TraineesPerClassThershold,
        'DVV Class': item.ISDVV
      }));
  }


  
  //==========================================================
  GetOrgRow(item: OrgConfigModel, BusinessRuleType) {
    return (item.OID > 0 && item.BusinessRuleType == BusinessRuleType  && item.SchemeID==0 && item.TSPID==0)
  }
  GetSchemeRow(item: OrgConfigModel) {
    return (item.SchemeID>0 && item.TSPID==0)
  }
  GetNotSchemeRow(item: OrgConfigModel) {
    return (item.SchemeID>0 && item.TSPID>0)
  }
  GetTSPRow(item: OrgConfigModel) {
    return (item.SchemeID > 0 && item.TSPID > 0 && item.ClassID==0);
  }
  GetNotTSPRow(item: OrgConfigModel) {
    return (item.SchemeID > 0 && item.TSPID > 0 && item.ClassID > 0);
  }
  SelProp(cur: any, All: OrgConfigModel[], Prop: string) {
    All.forEach((m: OrgConfigModel) => {
      m[Prop] = cur;
    });
  };
}
export class OrgConfigModel {
  ConfigID: number;
  OID: number;
  BusinessRuleType: string;
  SchemeID: number;
  TSPID: number;
  ClassID: number;
  DualRegistration: boolean;
  ISDVV: boolean;
  BracketDaysBefore: number;
  BracketDaysAfter: number;
  EligibleGenderID: number;
  MinAge: number;
  MaxAge: number;
  MinEducation: number;
  ReportBracketBefore: number;
  ReportBracketAfter: number;
  StipendPayMethod: string;
  ClassStartFrom1: number;
  ClassStartTo1: number;
  ClassStartFrom2: number;
  ClassStartTo2: number;
  BISPIndexFrom: number;
  BISPIndexTo: number;
  MinAttendPercent: number;
  StipendDeductAmount: number;
  PhyCountDeductPercent: number;
  DeductDropOutPercent: number;
  StipNoteGenGTMonth: number;
  StipNoteGenLTMonth: number;
  MPRDenerationDay: number;
  BusinessRuleTypeForGetPreviousList:string;

}
