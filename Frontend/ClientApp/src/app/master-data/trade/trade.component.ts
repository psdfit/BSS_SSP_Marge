import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from '../../../environments/environment';
import { FormGroup, FormBuilder, Validators, FormGroupDirective, FormControl, FormArray, ValidatorFn, AbstractControl, NgForm } from '@angular/forms';
import { CommonSrvService } from '../../common-srv.service';
import { ModelBase } from '../../shared/ModelBase';
import { UserRightsModel } from '../../master-data/users/users.component';
import { EnumProgramCategory, EnumApprovalProcess, EnumExcelReportType } from '../../shared/Enumerations';
import { MatTabGroup } from '@angular/material/tabs';
import { ITradeDetail } from './ITradeDetail';
import { I } from '@angular/cdk/keycodes';
import { __await } from 'tslib';
import { AppConfigService } from '../../app-config.service';
import { MatSelect } from '@angular/material/select';
import { MatOption } from '@angular/material/core';
// import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
// import { Observable, throwError } from 'rxjs';
// import { catchError } from 'rxjs/operators';
@Component({
  selector: 'app-trade',
  templateUrl: './trade.component.html',
  styleUrls: ['./trade.component.scss']
})
export class TradeComponent implements OnInit {
  tradeform: FormGroup;
  title: string; savebtn: string;
  displayedColumns = ['Action', 'InActive', 'TradeName', 'TradeCode', 'SAPID', 'SectorID', 'SubSectorID'];
  trade: MatTableDataSource<any>;
  Sector = [];
  SearchSch = new FormControl('',);
  Equipment = new FormControl('',);
  schemeFilter = new FormControl(0);
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
    limit: 5,
    page: 1
  };
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild("tabGroup") tabGroup: MatTabGroup;
  @ViewChild('NForm') NForm: NgForm;
  @ViewChild('EquipmentTools1') EquipmentTools1: MatSelect;
  working: boolean;
  ApiUrl: string;
  CurriculaAttachmentsArr: FormGroup;
  constructor(private apiUrl: AppConfigService, private fb: FormBuilder, private ComSrv: CommonSrvService) {
    this.ApiUrl = this.apiUrl.getAppConfig().UsersAPIURL
    // Initialize CurriculaAttachmentsArr
    this.CurriculaAttachmentsArr = this.fb.group({
      CurriculaAttachments: this.fb.array([]),
    });
    // Initialize tradeform with CurriculaAttachmentsArr as a nested form group
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
  // isCurriculaAttachmentEmpty(i: number): boolean {
  //   const curriculaAttachmentControl = this.curriculaAttachments.at(i).get('CurriculaAttachment').value;
  //   return curriculaAttachmentControl.value;
  // }
  fsAllSelected: any;
  toggleAllSelectionFundingS() {
    if (this.fsAllSelected) {
      //this.IsAllSelected = true;
      this.EquipmentTools1.options.forEach((item: MatOption) => item.select());
    } else {
      //this.IsAllSelected = false;
      this.EquipmentTools1.options.forEach((item: MatOption) => item.deselect());
    }
  }
  optionFSourceClick() {
    let newStatus = true;
    this.EquipmentTools1.options.forEach((item: MatOption) => {
      if (!item.selected) {
        newStatus = false;
      }
    });
    this.fsAllSelected = newStatus;
    // this.UpdateFiltersIDs();
  }
  EmptyCtrl() {
    this.SearchSch.setValue('');
    this.Equipment.setValue('');
  }
  downloadDocument(fileName: string) {
    if (fileName == '') {
      this.ComSrv.ShowError('There is no Attachment');
      return
    }
    this.ComSrv.downloadDocument('api/Users/GetDocument/' + fileName).subscribe(
      (blob: Blob) => {
        // Handle the downloaded blob, for example, save it or open it
        const url = window.URL.createObjectURL(blob);
        window.open(url);
      },
      (error: string) => {
        console.error('Error:', error);
        // Handle the error appropriately
      }
    );
  }
  AddTradeDetail() {
    debugger;
    this.TradeDetails.push({
      DurationID: '',
      TotalTrainingHours: '',
      DailyTrainingHours: '',
      WeeklyTrainingHours: '',
      CertificationCategoryID: '',
      CertAuthID: '',
      SourceOfCurriculumID: '',
      TraineeEducationTypeID: '',
      TraineeAcademicDisciplineID: 0,
      PracticalPercentage: '',
      TheoryPercentage: '',
      TheoryPercentage1: '',
      TrainerEducationTypeID: '',
      TrainerAcademicDisciplineID: 0,
      EquipmentToolID: '',
      ConsumableMaterialID: '',
      curriculaAttachments: ''
    });
    this.tradeLayerAttachment.push('')
    this.curriculaAttachments.push(this.fb.group({ CurriculaAttachment: ['', Validators.required], }));
  }
  GetTradeDetailData(id) {
    this.ComSrv.getJSON('api/Trade/GetTradeMapDetails/' + id).subscribe((REs: any) => {
      this.TradeDetails = REs.filter(x => x.EquipmentToolID = x.EquipmentToolID.split(',').map(Number));
      this.TradeDetails = REs.filter(x => x.ConsumableMaterialID = x.ConsumableMaterialID.split(',').map(Number));
      this.CurriculaAttachmentsArr = this.fb.group({
        CurriculaAttachments: this.fb.array([]),
      });
      debugger
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
        // this.editAttachment(index,row.CurriculaAttachment)
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
      //this.tradeDurations = d[10];
      //this.tradeEquipmenttools = d[11];
      //this.tradeConsumableMaterials = d[12];
      //this.tradeSourceOfCurriculums = d[13];
      this.trade.paginator = this.paginator;
      this.trade.sort = this.sort;
    }, error => this.error = error // error path
    );
  }
  ngOnInit() {
    this.currentUser = this.ComSrv.getUserDetails();
    this.GetTradeCode();
    this.ComSrv.setTitle("Trade");
    this.title = "Add New ";
    this.savebtn = "Save ";
    this.GetData();
  }
  ChkTradeName() {
    if (this.TradeName.value) {
      this.ComSrv.postJSON('api/Trade/CheckTradeName', { TradeID: this.TradeID.value, TradeName: this.TradeName.value }).subscribe((d: any) => {
        //this.users = d;
        if (d) {
          this.TradeName.setErrors(null);
        }
        else {
          this.TradeName.setErrors({ 'duplicate': true });
          this.TradeCode.setValue(null);
        }
      }, error => this.error = error // error path
      );
    }
  };
  ChkTradeCode() {
    if (this.TradeCode.value) {
      this.ComSrv.postJSON('api/Trade/CheckTradeCode', { TradeID: this.TradeID.value, TradeCode: this.TradeCode.value }).subscribe((d: any) => {
        //this.users = d;
        if (d)
          this.TradeCode.setErrors(null);
        else
          this.TradeCode.setErrors({ 'duplicate': true });
      }, error => this.error = error // error path
      );
    }
  };
  radioChange(event) {
    if (event.value == "1") {
      this.checked = true;
      this.checked2 = false;
      this.TradeName.reset();
      this.TradeCode.reset();
    }
    if (event.value == "2") {
      this.checked2 = true;
      this.checked = false;
      this.TradeName.reset();
      this.TradeCode.reset();
    }
  }
  GetTradeCode() {
    this.ComSrv.getJSON("api/Trade/CheckTradeCodeScheme").subscribe((d: any) => {
      this.sequence = d;
    },
    )
  }
  getTradeData(event) {
    var id = Number(event.value)
    this.ComSrv.getJSON("api/Trade/RD_TradeBy" + id).subscribe((d: any) => {
      this.row = d;
    },
    )
  }
  SetTradeCode(event) {
    if (event == null || event.length < 1) {
      this.tradeform.controls.TradeCode.setValue('');
      return;
    }
    let name = event;
    let n = name.substr(0, 3);
    n = n.toUpperCase();
    let code = n + "-" + this.sequence;
    this.tradeform.controls.TradeCode.setValue(code);
  }
  omit_special_char(event) {
    var k;
    k = event.charCode;  //         k = event.keyCode;  (Both can be used)
    return ((k > 64 && k < 91) || (k > 96 && k < 123) || k == 8 || k == 32 || (k >= 48 && k <= 57));
  }
  OnSectorSelected(event: any) {
    this.FilterSubSector(event.value)
  }
  TradeLayerExcelExport() {
    this.ComSrv.ExcelExporWithForm(this.TradeLayer, 'Trade Layer Report')
  }
  FilterSubSector(event: any) {
    if (event.value) {
      this.SubSectorSelected = this.SubSectorList.filter(subsec => subsec.SectorID === event.value);
      this.SubSector = this.SubSectorSelected;
    }
  }
  SubmitAgainTrade() {
    var strArray = this.TradeDetails.map(item => this.concatObjectAsString(item));
    var dup = this.getDuplicates(strArray);
    var dupIndices = Object.values(dup);
    var dupIndicesArray = dupIndices[0];
    var newArr: any[];
    newArr = [];
    var indexCounter = 0;
    this.TradeDetails.forEach(item => {
      item['Dirty'] = false;
    })
    dupIndices.forEach((x: any) => {
      x.forEach(ind => {
        var arrIndVal = this.TradeDetails[ind];
        if (indexCounter == 0) {
          this.TradeDetails[ind]['Dirty'] = true;
        }
        newArr.push(arrIndVal);
      })
      indexCounter++;
    }
    )
    if (newArr.length > 0) {
      this.error = "Duplicate Trade Details Exist in Red highlighted rows";
      this.ComSrv.ShowError(this.error.toString(), "Error");
      return;
    }
    else {
      this.SubmitTradeDetails();
    }
    this.Submit();
  }
  private concatObjectAsString(obj: ITradeDetail) {
    return obj.DurationID.toString()
      + obj.CertAuthID.toString()
      + obj.SourceOfCurriculumID.toString()
  }
  getDuplicates(array: any[]) {
    var duplicates = {};
    for (var i = 0; i < array.length; i++) {
      if (duplicates.hasOwnProperty(array[i])) {
        duplicates[array[i]].push(i);
      } else if (array.lastIndexOf(array[i]) !== i) {
        duplicates[array[i]] = [i];
      }
    }
    return duplicates;
  };
  SubmitTrade(SubmitType: string) {
    if (this.tradeform.value.TradeID == 0) {
      this.error = "Please save Trade First";
      this.ComSrv.ShowError(this.error.toString(), "Error");
      return false;
    }
    var strArray = this.TradeDetails.map(item => this.concatObjectAsString(item));
    var dup = this.getDuplicates(strArray);
    var dupIndices = Object.values(dup);
    var dupIndicesArray = dupIndices[0];
    var newArr: any[];
    newArr = [];
    var indexCounter = 0;
    this.TradeDetails.forEach(item => {
      item['Dirty'] = false;
    })
    dupIndices.forEach((x: any) => {
      x.forEach(ind => {
        var arrIndVal = this.TradeDetails[ind];
        if (indexCounter == 0) {
          this.TradeDetails[ind]['Dirty'] = true;
        }
        newArr.push(arrIndVal);
      })
      indexCounter++;
    }
    )
    if (newArr.length > 0) {
      this.error = "Duplicate Trade Details Exist in Red highlighted rows";
      this.ComSrv.ShowError(this.error.toString(), "Error");
      return;
    }
    this.tradeform.value["FinalSubmitted"] = true;
    this.Submit();
    if (SubmitType == "Final") {
      let processKey = EnumApprovalProcess.TRD;
      this.ComSrv.getJSON(`api/Trade/FinalSubmitTrade?TradeID=${this.tradeform.value.TradeID}&ProcessKey=${processKey}`)
        .subscribe((d: any) => {
          this.AllowFinalSubmit = true;
          this.trade = new MatTableDataSource(d);
        }, error => this.error = error
          , () => {
            this.working = false;
          });
    }
  }
  Submit() {
    if (!this.tradeform.valid) {
      return;
    }
    if (this.TradeDetails.length == 0) {
      this.error = "Please add Trade Details";
      this.ComSrv.ShowError(this.error.toString(), "Error");
      return;
    }
    const CurriCulaAttachmentsArr = this.curriculaAttachments.value;
    for (let index = 0; index < CurriCulaAttachmentsArr.length; index++) {
      const CurriculaAttachmentVal = CurriCulaAttachmentsArr[index].CurriculaAttachment;
      // if (CurriculaAttachmentVal=='') {
      //   this.ComSrv.ShowError(`Missing Curriculum Attachment at row ${index + 1}.`);
      //     return
      // }
      if (CurriculaAttachmentVal != '') {
        this.TradeDetails[index]['CurriculaAttachment'] = CurriculaAttachmentVal
      }
    }
    let Details = JSON.parse(JSON.stringify(this.TradeDetails));
    Details.forEach((x) => {
      x.EquipmentToolID = x.EquipmentToolID.join(',');
      x.ConsumableMaterialID = x.ConsumableMaterialID.join(',');
    }
    )
    this.tradeform.value["TradeDetails"] = Details;
    this.ComSrv.postJSON('api/Trade/Save', this.tradeform.value)
      .subscribe((d: any) => {
        this.trade = new MatTableDataSource(d);
        this.trade.paginator = this.paginator;
        this.trade.sort = this.sort;
        if (this.TradeID.value == 0) {
          this.GetTradeCode();
        }
        this.ComSrv.openSnackBar(this.TradeID.value > 0 ? environment.UpdateMSG.replace("${Name}", this.EnText) : environment.SaveMSG.replace("${Name}", this.EnText));
        this.reset();
        if (this.AllowFinalSubmit) {
          this.update = "Trade submitted";
          this.ComSrv.openSnackBar(this.update.toString(), "Submitted");
          this.AllowFinalSubmit = false;
          this.GetTradeCode();
        }
        this.title = "Add New ";
        this.savebtn = "Save ";
      },
        (error) => {
          this.error = error.error;
          this.working = false;
          this.ComSrv.ShowError(error.error);
        });
  }
  SubmitTradeDetails() {
    if (this.TradeDetails.length == 0) {
      this.error = "Please add Trade Details";
      this.ComSrv.ShowError(this.error.toString(), "Error");
      return false;
    }
    const CurriCulaAttachmentsArr = this.curriculaAttachments.value;
    for (let index = 0; index < CurriCulaAttachmentsArr.length; index++) {
      const CurriculaAttachmentVal = CurriCulaAttachmentsArr[index].CurriculaAttachment;
      if (CurriculaAttachmentVal == '') {
        this.ComSrv.ShowError(`Missing Curriculum Attachment at row ${index + 1}.`);
        return
      }
      this.TradeDetails[index]['CurriculaAttachment'] = CurriculaAttachmentVal
    }
    let Details = JSON.parse(JSON.stringify(this.TradeDetails));
    Details.forEach((x) => {
      x.EquipmentToolID = x.EquipmentToolID.join(',');
      x.ConsumableMaterialID = x.ConsumableMaterialID.join(',');
    }
    )
    this.tradeform.value["TradeDetails"] = Details;
    this.ComSrv.postJSON('api/Trade/SaveTradeDetail', this.tradeform.value)
      .subscribe((d: any) => {
        this.trade = new MatTableDataSource(d);
        this.trade.paginator = this.paginator;
        this.trade.sort = this.sort;
        this.ComSrv.openSnackBar(this.TradeID.value > 0 ? environment.UpdateMSG.replace("${Name}", this.EnText) : environment.SaveMSG.replace("${Name}", this.EnText));
        this.update = "Trade Detail submitted";
        this.ComSrv.openSnackBar(this.update.toString(), "Submitted");
        this.title = "Add New ";
        this.savebtn = "Save ";
      },
        (error) => {
          this.error = error.error;
          this.working = false;
          this.ComSrv.ShowError(error.error);
        });
  }
  reset() {
    this.tradeform.reset();
    (this.CurriculaAttachmentsArr.get('CurriculaAttachments') as FormArray).clear();
    this.tradeLayerAttachment = []
    this.CurriculaAttachmentsArr.reset();
    this.NForm.resetForm();
    this.TradeID.setValue(0);
    this.IsApproved.setValue(0);
    this.IsRejected.setValue(0);
    this.TradeDetails = [];
    this.isOpenSubmission = true;
    this.tradeform.enable({ onlySelf: true });
    this.title = "Add New ";
    this.savebtn = "Save ";
  }
  toggleEdit(row) {
    // debugger;
    this.tabGroup.selectedIndex = 0;
    this.EditFlag = true;
    this.TradeID.setValue(row.TradeID);
    this.TradeName.setValue(row.TradeName);
    if (row.TradeCode == "") {
      this.SetTradeCode(row.TradeName)
    }
    else {
      this.TradeCode.setValue(row.TradeCode);
    }
    this.SectorID.setValue(row.SectorID);
    this.SubSectorID.setValue(row.SubSectorID);
    this.InActive.setValue(row.InActive);
    this.GetTradeDetailData(row.TradeID);
    if (row.FinalSubmitted) {
      this.tradeform.disable({ onlySelf: true });
      this.isOpenSubmission = false;
      this.isOpenSubmissionMessage = "(Submitted)"
    }
    else {
      this.isOpenSubmission = true;
      this.tradeform.enable({ onlySelf: true });
    }
    this.title = "Update ";
    this.savebtn = "Save ";
  }
  toggleActive(row) {
    this.ComSrv.confirm().subscribe(result => {
      if (result) {
        this.ComSrv.postJSON('api/Trade/ActiveInActive', { 'TradeID': row.TradeID, 'InActive': row.InActive })
          .subscribe((d: any) => {
            this.ComSrv.openSnackBar(row.InActive == true ? environment.InActiveMSG.replace("${Name}", this.EnText) : environment.ActiveMSG.replace("${Name}", this.EnText));
          },
            (error) => {
              this.error = error.error;
              this.ComSrv.ShowError(error.error);
              row.InActive = !row.InActive;
            });
      }
      else {
        row.InActive = !row.InActive;
      }
    });
  }
  applyFilter(filterValue: string) {
    filterValue = filterValue.trim();
    filterValue = filterValue.toLowerCase();
    this.trade.filter = filterValue;
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
