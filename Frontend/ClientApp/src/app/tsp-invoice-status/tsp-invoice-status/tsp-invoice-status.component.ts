/* **** Aamer Rehman Malik *****/
import { Component, OnInit } from "@angular/core";
import { FormControl } from "@angular/forms";
import * as moment from 'moment';
import { CommonSrvService } from "src/app/common-srv.service";
import { IQueryFilters } from "src/app/home/home.component";
import { DialogueService } from 'src/app/shared/dialogue.service';
import { EnumUserLevel } from '../../shared/Enumerations';
import { UsersModel } from '../../master-data/users/users.component';
import { Moment } from "moment";
import * as _moment from 'moment';
import { DateAdapter, MAT_DATE_LOCALE, MAT_DATE_FORMATS } from '@angular/material/core';
import { MomentDateAdapter, MAT_MOMENT_DATE_ADAPTER_OPTIONS } from '@angular/material-moment-adapter';
import { MatDatepicker } from "@angular/material/datepicker";
import { environment } from '../../../environments/environment';
import { DatePipe } from '@angular/common';


export const MY_FORMATS = {
  parse: {
    dateInput: 'MM/YYYY',
  },
  display: {
    dateInput: 'MM/YYYY',
    monthYearLabel: 'MMM YYYY',
    dateA11yLabel: 'LL',
    monthYearA11yLabel: 'MMMM YYYY',
  },
};

@Component({
  selector: "app-tsp-invoice-status",
  templateUrl: "./tsp-invoice-status.component.html",
  styleUrls: ["./tsp-invoice-status.component.scss"],
  providers: [
    // `MomentDateAdapter` can be automatically provided by importing `MomentDateModule` in your
    // application's root module. We provide it at the component level here, due to limitations of
    // our example generation script.
    DatePipe,
    {
      provide: DateAdapter,
      useClass: MomentDateAdapter,
      deps: [MAT_DATE_LOCALE, MAT_MOMENT_DATE_ADAPTER_OPTIONS],

    },

    { provide: MAT_DATE_FORMATS, useValue: MY_FORMATS },
  ],
})


export class TSPInvoiceStatusComponent implements OnInit {
  environment = environment;
  InvoiceHeaders: any;
  month = new FormControl(moment());
  Schemes: any[];
  TSP: any[];
  Classes: any[];
  kamAssignedUsers: any;

  userid: number;
  userObj: any[];
  kamRoleId: number;

  error = '';

  SearchSch = new FormControl("");
  SearchCls = new FormControl("");
  SearchTSP = new FormControl("");
  filters: IQueryFilters = {
    SchemeID: 0,
    TSPID: 0,
    ClassID: 0,
    TraineeID: 0,
    UserID: 0,
  };
  ClassData: ClassInvoiceMapExtModel[];
  currentUser: UsersModel;
  enumUserLevel = EnumUserLevel;
  constructor(private ComSrv: CommonSrvService, private dailog: DialogueService) { }

  ngOnInit(): void {
    this.Init();
  }

  Init() {
    this.ComSrv.setTitle("TSP Invoice Status");
    this.currentUser = this.ComSrv.getUserDetails();
    //var oid = this.ComSrv.OID;
    this.checkForKAMUser();
    //this.getSchemes();
    //this.ComSrv.postJSON("api/Cancelation/GetRelevantUserData/", this.filters).subscribe((res) => {
    //  this.Schemes = res["Schemes"];
    //  this.TSP = res["TSP"];
    //  this.Classes = res["Classes"];
    //});
  }
  getSchemes() {
    this.ComSrv.postJSON('api/Scheme/FetchSchemeByUser', this.filters).subscribe(
      (d: any) => {
        this.Schemes = d;
      },
      //error => this.error = error
    );
  }

  KAMRelevantTSPsByScheme(filters: IQueryFilters) {
    this.TSP = [];
    this.Classes = [];
    //this.TSPID.setValue('');
    //this.ClassID.setValue('');
    if (this.filters.SchemeID != 0) {
      this.ComSrv.postJSON(
        `api/TSPDetail/GetKamRelevantTSPDetailByScheme`, { SchemeID: this.filters.SchemeID, UserID: this.userid }
      ).subscribe(
        (data) => {
          this.TSP = <any[]>data;
        },
        (error) => {
          this.error = `${error.name} , ${error.statusText}`;
          console.log(error)
        }
      );
    }
  }

  getTSPDetailByScheme(schemeId: number) {
    //this.filters.setValue(0);
    //this.classFilter.setValue(0);
    this.filters.ClassID = 0;
    this.filters.TSPID = 0;
    //this.tspDetailArray = [];
    //this.classesArray = [];
    this.ComSrv.getJSON(`api/TSPDetail/GetTSPDetailByScheme/` + schemeId)
      .subscribe(data => {
        this.TSP = <any[]>data;
      }, error => {
        //this.error = error;
      })
  }
  getClassesByTsp(tspId: number) {
    this.filters.ClassID = 0;
    //this.classesArray = [];
    this.ComSrv.getJSON(`api/Class/GetClassesByTsp/` + tspId)
      .subscribe(data => {
        this.Classes = <any[]>data;
      }, error => {
        //this.error = error;
      })
  }


  getDataByFilters() {
    if (this.filters.ClassID > 0) {
      this.ComSrv.getJSON(
        "api/Cancelation/getTSPInvoiceStatusData/",
        this.filters.ClassID
      ).subscribe((res: any) => {
        this.ClassData = res;
      });
    }
  }

  getDataByFiltersForTSPInvoice(r) {
    //if (this.filters.ClassID > 0) {
    //this.ComSrv.getJSON("api/Cancelation/getTSPInvoiceStatusDataMonthWise/?ClassID=" + this.filters.ClassID +"&Month=" + this.month.value).subscribe((res: any) => {
    if (r.ClassData) {
      r.ClassData = null;

      return;
    }


    if (this.kamRoleId) {
      this.ComSrv.postJSON("api/Cancelation/getTSPInvoiceStatusDataMonthWiseForKAM", { "InvoiceHeaderID": r.InvoiceHeaderID, "ClassID": this.filters.ClassID, "Month": this.month.value }).subscribe((res: any) => {
        r.ClassData = res;
      });
    }
    if (this.currentUser.UserLevel != this.enumUserLevel.TSP && !this.kamRoleId) {
      this.ComSrv.postJSON("api/Cancelation/getTSPInvoiceStatusDataMonthWiseByInternalUser", { "InvoiceHeaderID": r.InvoiceHeaderID, "ClassID": this.filters.ClassID, "Month": this.month.value }).subscribe((res: any) => {
        r.ClassData = res;
      });
    }
    else {
      this.ComSrv.postJSON("api/Cancelation/getTSPInvoiceStatusDataMonthWise", { "InvoiceHeaderID": r.InvoiceHeaderID, "ClassID": this.filters.ClassID, "Month": this.month.value }).subscribe((res: any) => {
        r.ClassData = res;
      });
    }
  //  }
  }

  GetInvoicesForApproval() {
    if (this.kamRoleId) {
      this.ComSrv.postJSON('api/Invoice/GetInvoicesForKAM', { KAMID:this.filters.UserID,U_Month: this.month.value, OID: this.ComSrv.OID.value, SchemeID: this.filters.SchemeID, ClassID: this.filters.ClassID, TSPID: this.currentUser.UserID }).subscribe((d: any) => {
        this.InvoiceHeaders = d;
      });
    }
    if (this.currentUser.UserLevel != this.enumUserLevel.TSP && !this.kamRoleId) {
      this.ComSrv.postJSON('api/Invoice/GetInvoicesForInternalUser', { U_Month: this.month.value, OID: this.ComSrv.OID.value, SchemeID: this.filters.SchemeID, ClassID: this.filters.ClassID, TSPID: this.filters.TSPID }).subscribe((d: any) => {
        this.InvoiceHeaders = d;
      });
    }
    if (this.currentUser.UserLevel == this.enumUserLevel.TSP) {
      this.ComSrv.postJSON('api/Invoice/GetInvoicesForTSP', { U_Month: this.month.value, OID: this.ComSrv.OID.value, SchemeID: this.filters.SchemeID, ClassID: this.filters.ClassID, TSPID: this.currentUser.UserID }).subscribe((d: any) => {
        this.InvoiceHeaders = d;
      });
    }
  }

  Cancel(row: any) {
    this.ComSrv.confirm("Cancelation", "Are you sure?").subscribe((result) => {
      if (result) {
        row.Month = moment.utc(row.Month);
        this.ComSrv.postJSON("api/Cancelation/Cancelation", row).subscribe(
          (res:any) => {
            this.ClassData = res;
          }
        );
      }
    });
  }
  Regenrate(row: any, index: number) {
    if (index > 0) {
      let b = false;
      for (var i = 0; i < index; i++) {
        if (this.ClassData[i].RegenratePRN || this.ClassData[i].RegenrateMPR)
          b = true;
      }
      if (b) {
        this.ComSrv.ShowWarning("Please Genrate previous Invoice(s)");
        return;
      }
    }
    this.ComSrv.confirm("Regenrate", "Are you sure?").subscribe((result) => {
      if (result) {
        row.Month =  moment.utc(row.Month);
        this.ComSrv.postJSON("api/Cancelation/Regenrate", row).subscribe(
          (res: any) => {
            this.ClassData = res;
          }
        );
      }
    });
  };
  GetMPR(ID:number) {
    this.dailog.openDocumentDialogue(ID, 'MPR');
  }
  GetSRN(ID: number) {
    this.dailog.openDocumentDialogue(ID, 'SRN');
  }
  GetPRN(ID: number) {
    this.dailog.openDocumentDialogue(ID, 'PRN');
  }
  GetPO(ID: number) {
    this.dailog.openDocumentDialogue(ID, 'PO');
  }
  GetInv(ID: number) {
    this.dailog.openDocumentDialogue(ID, 'Inv');
  }
  EmptyCtrl(ev: any) {
    this.SearchCls.setValue("");
    this.SearchTSP.setValue("");
    this.SearchSch.setValue("");
  }

  getUserRelevantTSPs() {
    if (this.kamRoleId) {
      this.KAMRelevantTSPsByScheme(this.filters);
    }
    else {
      this.getTSPDetailByScheme(this.filters.SchemeID);
    }
  }

  getDependantFilters() {
    if (this.currentUser.UserLevel == this.enumUserLevel.TSP) {
      this.getClassesBySchemeFilter();
    }
    else {
      this.getUserRelevantTSPs();
    }
  }

  getClassesBySchemeFilter() {
    this.filters.ClassID = 0;
    this.filters.TraineeID = 0;
    this.ComSrv.postJSON(`api/Class/FetchClassesByUser/`, { UserID: this.currentUser.UserID, OID: this.ComSrv.OID.value, SchemeID: this.filters.SchemeID })
      .subscribe(data => {
        this.Classes = <any[]>data;
        //this.activeClassesArrayFilter = this.classesArrayFilter.filter(x => x.ClassStatusID == 3);
      }, error => {
        //this.error = error;
      })
  }

  checkForKAMUser() {
    this.currentUser = this.ComSrv.getUserDetails();
    this.userid = this.currentUser.UserID;

    this.ComSrv.getJSON('api/KAMAssignment/RD_KAMAssignmentForFilters').subscribe((d: any) => {
      this.kamAssignedUsers = d;
      this.userObj = this.kamAssignedUsers.filter(y => y.UserID == this.userid);
      if (this.userObj.length > 0) {
        this.kamRoleId = this.userObj.map(x => x.RoleID)[0];
      }
      if (this.kamRoleId) {
        this.getSchemesByKAM();
      }
      else {
        this.getSchemes();
      }

      // x.UserID, y => y.RoleID)
    },
    );
  }
  clearMonth() {
    this.month = new FormControl(moment(null));
    //this.month.setValue(null);
    this.GetInvoicesForApproval();

  }
  chosenYearHandler(normalizedYear: Moment) {
    this.month = new FormControl(moment());
    const ctrlValue = this.month.value;
    ctrlValue.year(normalizedYear.year());
    this.month.setValue(ctrlValue);
  }
  chosenMonthHandler(normalizedMonth: Moment, datepicker: MatDatepicker<Moment>) {
    const ctrlValue = this.month.value;
    ctrlValue.month(normalizedMonth.month());
    this.month.setValue(ctrlValue);
    this.GetInvoicesForApproval();
    //this.getDataByFiltersForTSPInvoice();
    datepicker.close();
  }
  getSchemesByKAM() {
    this.Schemes = [];
    this.ComSrv.getJSON(
      'api/Scheme/GetSchemeByKAM/' + this.userid).subscribe(
        (d: any) => {
          this.error = '';
          this.Schemes = d;
        },
        (error) => (this.error = `${error.name} , ${error.statusText}`)
      );
  }

}
export class ClassInvoiceMapExtModel
{
         MPRID         :number; 
         PRNID         :number; 
         Invoices      :number; 
          MPRGenrated  :Boolean
          RegenrateMPR :Boolean;
          RegenratePRN :Boolean;
                       
          IsGenerated  :Boolean;
          InCancelation:Boolean;
         SRNID         :number; 
         POLineID      :number; 
          InvSapID     :string;
          POSapID      :string;
          SRNInvSapID  :string;
        SRNInvoice: number; 
}
