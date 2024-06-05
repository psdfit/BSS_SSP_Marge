import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { FormGroup, FormBuilder, Validators, FormGroupDirective, FormControl, FormArray, ValidatorFn, AbstractControl, NgForm } from '@angular/forms';
import { CommonSrvService } from '../../common-srv.service';
import { ModelBase } from '../../shared/ModelBase';
import { UserRightsModel } from '../users/users.component';
import { MatTabGroup } from '@angular/material/tabs';

import { __await } from 'tslib';
import { AppConfigService } from '../../app-config.service';
@Component({
  selector: 'app-trade-layer',
  templateUrl: './trade-layer.component.html',
  styleUrls: ['./trade-layer.component.scss']
})
export class TradeLayerComponent implements OnInit {
  tradeform: FormGroup;
  title: string; savebtn: string;
  displayedColumns = ['Action', 'TradeName', 'TradeCode', 'SAPID', 'SectorID', 'SubSectorID'];
  trade: MatTableDataSource<any>;
  Sector = [];
  SubSector = [];
  SubSectorList = [];
  SubSectors = [];
  SubSectorSelected = [];
  CertificationCategory = [];
  CertificationAuthority = [];
  ConsumableMaterialList = [];
  EquipmentToolsList = [];
  SourceOfCurriculumList = [];
  TrainerQualificationList = [];
  DurationList = [];
  AcademicDisciplineList = [];
  TradeLayer = [];
  tradeDurations = [];
  tradeConsumableMaterials = [];
  tradeEquipmenttools = [];
  tradeSourceOfCurriculums = [];
  selectedDurations = [];
  selectedEquipmentTools = [];
  selectedConsumableMaterials = [];
  selectedSourceOfCurriculums = [];
  existingtrades = [];
  row = [];
  update: String;
  currentUser: any;
  TradeDetails = [];
  checked = false
  checked2 = true
  formrights: UserRightsModel;
  EnText: string = "Trade";
  sequence: any;
  error: String;
  AllowFinalSubmit: boolean = false;
  EditFlag: boolean = false;
  isOpenSubmissionMessage: string = "";
  isOpenSubmission: boolean = true;
  tradeLayerAttachment = []
  DocFile = ""
  DocArr = []
  query = {
    order: 'TradeID',
    limit: 10,
    page: 1
  };
  isEdit: boolean = false;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild("tabGroup") tabGroup: MatTabGroup;
  @ViewChild('NForm') NForm: NgForm;
  working: boolean;
  ApiUrl: string;
  CurriculaAttachmentsArr: FormGroup;
  constructor(private apiUrl: AppConfigService, private fb: FormBuilder, private ComSrv: CommonSrvService) {
    this.ApiUrl = this.apiUrl.getAppConfig().UsersAPIURL
    this.CurriculaAttachmentsArr = this.fb.group({
      CurriculaAttachments: this.fb.array([]),
    });
    this.tradeform = this.fb.group({
      TradeID: 0,
      TradeName: ["", Validators.required],
      TradeCode: ["", Validators.required],
      SectorID: ["", Validators.required],
      SubSectorID: ["", Validators.required],
      IsApproved: 0,
      IsRejected: 0,
      InActive: ""
    }, { updateOn: "blur" });
    this.trade = new MatTableDataSource([]);
    this.formrights = ComSrv.getFormRights();
  }

  downloadDocument(fileName: string) {
    if (fileName == '') {
      this.ComSrv.ShowError('There is no Attachment');
      return
    }
    this.ComSrv.downloadDocument('api/Users/GetDocument/' + fileName).subscribe(
      (blob: Blob) => {
        const url = window.URL.createObjectURL(blob);
        window.open(url);
      },
      (error: string) => {
        console.error('Error:', error);
      }
    );
  }

  GetTradeDetailData(id) {
    this.ComSrv.getJSON('api/Trade/GetTradeMapDetails/' + id).subscribe((REs: any) => {
      this.TradeDetails = REs.filter(x => x.EquipmentToolID = x.EquipmentToolID.split(',').map(Number));
      this.TradeDetails = REs.filter(x => x.ConsumableMaterialID = x.ConsumableMaterialID.split(',').map(Number));
      this.CurriculaAttachmentsArr = this.fb.group({
        CurriculaAttachments: this.fb.array([]),
      });
      // debugger
      this.tradeLayerAttachment = []
      this.TradeDetails.forEach(row => {
        this.DocFile = ""
        this.DocArr = []
        if (row.CurriculaAttachmentPre != "") {
          this.DocArr = row.CurriculaAttachmentPre.split("\\")
          this.DocFile = this.TradeCode.value + '||' + this.DocArr[5]
        } else {
          this.DocFile = row.CurriculaAttachmentPre
        }
        this.tradeLayerAttachment.push(this.DocFile)
        this.curriculaAttachments.push(this.fb.group({
          CurriculaAttachment: row.CurriculaAttachment,
        }));
        if (row.MappedWithClass) {
          row.disable();
        }
      }
      )
    }, error => this.error = error // error path
    )
  }
  RemoveTradeDetails(c) {
    this.TradeDetails.splice(this.TradeDetails.indexOf(c), 1);
    this.curriculaAttachments.removeAt(c);
  }
  GetData() {
    this.ComSrv.getJSON('api/Trade/GetTrade').subscribe((d: any) => {
      this.trade = new MatTableDataSource(d[0])
      this.existingtrades = d[0];
      this.Sector = d[1];
      this.SubSector = d[2];
      this.SubSectorList = d[2];
      this.CertificationCategory = d[3];
      this.CertificationAuthority = d[4];
      this.EquipmentToolsList = d[5];
      this.ConsumableMaterialList = d[6];
      this.TrainerQualificationList = d[7];
      this.SourceOfCurriculumList = d[8];
      this.DurationList = d[9];
      this.AcademicDisciplineList = d[10];
      this.TradeLayer = d[11];
      if (this.AllowFinalSubmit) {
        this.update = "Trade submitted";
        this.ComSrv.openSnackBar(this.update.toString(), "Submitted");
      }
      this.trade.paginator = this.paginator;
      this.trade.sort = this.sort;
    }, error => this.error = error 
    );
  }
  ngOnInit() {
    this.currentUser = this.ComSrv.getUserDetails();
    this.ComSrv.setTitle("Trade");
    this.title = "Add New ";
    this.savebtn = "Save ";
    this.GetData();
  }
  toggleEdit(row) {
    this.tabGroup.selectedIndex = 1;
    this.EditFlag = true;
    this.TradeID.setValue(row.TradeID);
    this.TradeName.setValue(row.TradeName);
    this.TradeCode.setValue(row.TradeCode);
    this.isEdit = true
    this.SectorID.setValue(row.SectorID);
    this.SubSectorID.setValue(row.SubSectorID);
    this.InActive.setValue(row.InActive);
    this.GetTradeDetailData(row.TradeID);
    this.tradeform.disable({ onlySelf: true });
    this.title = "Update ";
    this.savebtn = "Save ";
  }
  applyFilter(filterValue: string) {
    filterValue = filterValue.trim();
    filterValue = filterValue.toLowerCase();
    this.trade.filter = filterValue;
  }
  TradeLayerExcelExport() {
    this.ComSrv.ExcelExporWithForm(this.TradeLayer, 'Trade Layer Report')
  }
  get curriculaAttachments(): FormArray {
    return this.CurriculaAttachmentsArr.get('CurriculaAttachments') as FormArray;
  }
  get TradeID() { return this.tradeform.get("TradeID"); }
  get TradeName() { return this.tradeform.get("TradeName"); }
  get TradeCode() { return this.tradeform.get("TradeCode"); }
  get SectorID() { return this.tradeform.get("SectorID"); }
  get SubSectorID() { return this.tradeform.get("SubSectorID"); }
  get TraineeEducationTypeID() { return this.tradeform.get("TraineeEducationTypeID"); }
  get DurationID() { return this.tradeform.get("DurationID"); }
  get TotalTrainingHours() { return this.tradeform.get("TotalTrainingHours"); }
  get DailyTrainingHours() { return this.tradeform.get("DailyTrainingHours"); }
  get WeeklyTrainingHours() { return this.tradeform.get("WeeklyTrainingHours"); }
  get PracticalPercentage() { return this.tradeform.get("PracticalPercentage"); }
  get TheoryPercentage() { return this.tradeform.get("TheoryPercentage"); }
  get TheoryPercentage1() { return this.tradeform.get("TheoryPercentage1"); }
  get CertificationCategoryID() { return this.tradeform.get("CertificationCategoryID"); }
  get CertAuthID() { return this.tradeform.get("CertAuthID"); }
  get EquipmentTools() { return this.tradeform.get("EquipmentTools"); }
  get ConsumableMaterials() { return this.tradeform.get("ConsumableMaterials"); }
  get TrainerEducationTypeID() { return this.tradeform.get("TrainerEducationTypeID"); }
  get SourceOfCurriculumID() { return this.tradeform.get("SourceOfCurriculumID"); }
  get IsApproved() { return this.tradeform.get("IsApproved"); }
  get IsRejected() { return this.tradeform.get("IsRejected"); }
  get InActive() { return this.tradeform.get("InActive"); }
}
export class TradeModel extends ModelBase {
  TradeID: number;
  TradeName: string;
  CurriculaAttachment: string;
  TradeCode: string;
  SectorID: number;
  SubSectorID: number;
  TraineeEducationTypeID: number;
  DurationID: number;
  TotalTrainingHours: number;
  DailyTrainingHours: number;
  WeeklyTrainingHours: number;
  PracticalPercentage: number;
  TheoryPercentage: number;
  TheoryPercentage1: any;
  CertificationCategoryID: number;
  CertAuthID: number;
  EquipmentToolID: number;
  ConsumableMaterialID: number;
  TrainerEducationTypeID: number;
  SourceOfCurriculumID: number;
}
