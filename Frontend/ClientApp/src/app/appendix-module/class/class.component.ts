import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, FormGroupDirective, Validators, AbstractControl, FormControl } from '@angular/forms';
import { environment } from '../../../environments/environment';
import { CommonSrvService } from '../../common-srv.service';
import { UserRightsModel } from '../../master-data/users/users.component';
import { ModelBase } from '../../shared/ModelBase';
import { TSPModel } from '../tsp/tsp.component';
import { SchemeModel } from '../appendix/appendix.component';
import { EnumProgramCategory, EnumAppendixModules, EnumProgramType } from '../../shared/Enumerations';
import * as moment from 'moment';
import { Observable } from 'rxjs';
import { MatStepper } from '@angular/material/stepper';


@Component({
  selector: 'app-class',
  templateUrl: './class.component.html',
  styleUrls: ['./class.component.scss']
})

export class ClassComponent implements OnInit {
  environment = environment;

  tableList: any[] = [];
  populatedTableList: any[] = [];
  inlineForm: FormGroup = this.getInlineForm();

  classForm: FormGroup;
  notForm: FormGroup; // this form is not for submitting. Just adds entries to the ClassForm.
  formrights: UserRightsModel;
  EnText: string = "Class";
  error: String;
  districts: any;
  tspMasterList: any;
  tehsils: any;
  schemeData: any;
  sectors: any;
  trades: any;
  durations: any;
  sourceOfCurriculums: any;
  clusters: any;
  province: any;
  education: any;
  certificationAgency: any;
  genderOfTrainee: any;
  TradeDetailMap: any;
  formattedDate: any;
  classStartDateNextBatch: any;
  classEndDateNextBatch: any;
  classStartDateNextBatch1: any;
  classEndDateNextBatch1: any;
  send_date = new Date();

  registrationAuthority: any;
  programFocus: any;
  //classStartDateTemp = new Date();
  //classEndDateTemp = new Date();
  insertedClass: any[] = [];
  decimalPlaces: number = 6;
  @Input() scheme: SchemeModel[] = [];
  @Input() tsps: TSPModel[] = [];
  @Input() classes: ClassModel[] = [];
  @Output() classData: EventEmitter<any> = new EventEmitter<any>();
  @Output() next: EventEmitter<any> = new EventEmitter<any>();
  schemeColumns = ['Scheme Name', 'Scheme Code', 'Description', 'Organization', 'PaymentSchedule'];
  tspColumns = ['TSP Name', 'NTN', 'PNTN', 'GST', 'FTN'];
  // progress: number = 0;
  geoTagPattern: string = String.raw`^[+-]?((\d+(\.\d*)?)|(\.\d+))([ ]*)[,]([ ]*)[+-]?((\d+(\.\d*)?)|(\.\d+))$`;
  classFormGroup = {};
  stepper: MatStepper;
  constructor(private _formBuilder: FormBuilder, private http: CommonSrvService) {
    this.formrights = http.getFormRights();
    //this.rowForm = this.newRow();
  }
  ngOnInit() {
    this.http.OID.subscribe(OID => {
      this.send_date.setMonth(this.send_date.getMonth() + 8);
      this.formattedDate = this.send_date.toISOString().slice(0, 10);
      this.tableList = [];
      this.populatedTableList = [];
      this.initForm()
      this.getData();
    });
  }
  initForm() {
    this.classForm = this._formBuilder.group({
      Class: new FormArray([])
    });
    this.classFormGroup = {
      TSPID: ['', Validators.required],
      SchemeID: [''],
      OrganizationID: [''],
      ClassID: [0],
      TSPName: [''],
      MinAge: [''],
      MaxAge: [''],
      SectorID: ['', Validators.required],
      TradeID: ['', Validators.required],
      Duration: ['', Validators.required],
      SourceOfCurriculum: ['', Validators.required],
      EntryQualification: ['', Validators.required],
      CertAuthID: ['', Validators.required],

      RegistrationAuthorityID: ['', Validators.required],
      ProgramFocusID: ['', Validators.required],
      //AttendanceStandardPercentage: ['', Validators.required],
      TotalTrainees: [0, Validators.required],
      TraineesPerClass: [0, Validators.required],
      Batch: [0, Validators.required],
      //TotalClasses: [0, Validators.required],//extra field in main form
      //ClassCode: ['', Validators.required],//extra field in grid
      MinHoursPerMonth: [0, Validators.required],
      StartDate: ['', Validators.required],
      EndDate: ['', Validators.required],
      GenderID: ['', Validators.required],
      TrainingAddressLocation: ['', Validators.required],
      GeoTagging: ['', [Validators.required, Validators.pattern(this.geoTagPattern)]],
      DistrictID: ['', Validators.required],
      TehsilID: ['', Validators.required],
      ClusterID: ['', Validators.required],
      ProvinceID: ['', Validators.required],
      BidPrice: 0,
      BMPrice: 0,
      TotalPerTraineeCostInTax: [0, Validators.required],
      SalesTaxRate: [0, [Validators.required, Validators.max(0.99)]],
      TrainingCostPerTraineePerMonthExTax: [0, Validators.required],
      SalesTax: [0, Validators.required],
      TrainingCostPerTraineePerMonthInTax: [0, Validators.required],
      UniformBagCost: [0, Validators.required],
      PerTraineeTestCertCost: [0, Validators.required],
      BoardingAllowancePerTrainee: [0],
      EmploymentCommitmentSelf: [0, Validators.required],
      EmploymentCommitmentFormal: [0, Validators.required],
      OverallEmploymentCommitment: [0, Validators.required],
      Stipend: [0, Validators.required],
      //StipendMode: ['Digital', Validators.required],
      TotalCostPerClass: [0, Validators.required],
      //TradeDetailMapID: ['']
      balloonpayment: 0,
      GuruPayment: 0,
      Transportation: 0,
      ProtectorateandVisa: 0,  //Added by Rao Ali Haider for International Plac
      MedicalCost: 0,
      PrometricCost: 0,
      OtherTrainingCost: 0,
    }
    this.notForm = this._formBuilder.group({ ...this.classFormGroup, TotalClasses: [0, Validators.required] });

  }
  updateGeoTaggingControl(control: AbstractControl, isRequired: boolean) {
    if (isRequired) {
      control.setValidators([Validators.required, Validators.pattern(this.geoTagPattern)]);
      control.updateValueAndValidity();
    } else {
      control.clearValidators();
      control.setValidators([Validators.pattern(this.geoTagPattern)]);
      control.updateValueAndValidity();
    }
  }
  updateTrainingLocationControl(control: AbstractControl, isRequired: boolean) {
    if (isRequired) {
      control.setValidators([Validators.required]);
      control.updateValueAndValidity();
    } else {
      control.clearValidators();
      //control.setValidators([Validators.pattern(this.geoTagPattern)]);
      control.updateValueAndValidity();
    }
  }
  onChangeProgramType(value: number) {
    if (value == EnumProgramType.Community
      // || value == EnumProgramCategory.CostSharing
    ) {
      ///UPDATE MAIN FORM
      this.updateGeoTaggingControl(this.notForm.controls.GeoTagging, false);
      this.updateTrainingLocationControl(this.notForm.controls.TrainingAddressLocation, false);
      ///UPDATE GRID
      this.Class.controls.forEach((rowForm: FormGroup) => {
        this.updateGeoTaggingControl(rowForm.controls.GeoTagging, false);
        this.updateTrainingLocationControl(rowForm.controls.TrainingAddressLocation, false);
      })
      this.populatedTableList.forEach(obj => {
        obj['RequiredLocationGeoTag'] = false;
        this.highlightInvalidPopulatedList(obj);
      }
      )
    }
    else {
      ///UPDATE MAIN FORM
      this.updateGeoTaggingControl(this.notForm.controls.GeoTagging, true);
      this.updateTrainingLocationControl(this.notForm.controls.TrainingAddressLocation, true);
      ///UPDATE GRID 
      this.Class.controls.forEach((rowForm: FormGroup) => {
        this.updateGeoTaggingControl(rowForm.controls.GeoTagging, true);
        this.updateTrainingLocationControl(rowForm.controls.TrainingAddressLocation, true);
      })
      this.populatedTableList.forEach(obj => {
        obj['RequiredLocationGeoTag'] = true;
      }
      )
    }
  }
  onChangeEducation(maxEducationID: number) {
    this.education = this.education.filter(x => x.EducationTypeID <= maxEducationID);
  }
  //onChangeSchemeCode(schemeCode: string) {
  //  this.Class.controls.forEach((rowForm: FormGroup) => {
  //    let classCode = rowForm.controls.ClassCode.value.split('-');
  //    rowForm.controls.ClassCode.patchValue(`${schemeCode}-${classCode[1]}-${classCode[2]}`);
  //  })
  //}
  onSaveTspData_OnClassGrid(tspList: TSPModel[]) {
    if (tspList.length > 0) {
      this.Class.controls.forEach((rowForm: FormGroup) => {
        let foundItem = tspList.find(x => x.TSPName.trim().toLowerCase() == rowForm.value.TSPName.toLowerCase());
        if (foundItem) {
          //let seq = rowForm.value.ClassCode?.split('-');
          //seq = seq[seq.length - 1];
          rowForm.controls.TSPID.patchValue(foundItem.TSPID)
          //rowForm.controls.ClassCode.patchValue(`${this.scheme[0]?.SchemeCode}-${foundItem.TSPCode ?? ''}-${seq}`)
        }
      })

      this.populatedTableList.forEach(rowForm => {
        let foundItem = tspList.find(x => x.TSPName.trim().toLowerCase() == rowForm.TSPName.toLowerCase());
        if (foundItem) {
          //let seq = rowForm.value.ClassCode?.split('-');
          //seq = seq[seq.length - 1];
          rowForm.TSPID = foundItem.TSPID;
          //rowForm.Stipend = this.scheme[0].Stipend;
          //rowForm.UniformBagCost = this.scheme[0].UniformAndBag;
          //rowForm.controls.ClassCode.patchValue(`${this.scheme[0]?.SchemeCode}-${foundItem.TSPCode ?? ''}-${seq}`)
        }
      })
    }
    this.populatedTableList.forEach(x => {
      this.highlightInvalidPopulatedList(x);
    })

  }

  onSaveSchemeData_OnClassGrid(schemeList: SchemeModel[]) {

    this.populatedTableList.forEach(rowForm => {
      rowForm.Stipend = schemeList[0].Stipend;
      //rowForm.UniformBagCost = schemeList[0].UniformAndBag;
      //rowForm.controls.ClassCode.patchValue(`${this.scheme[0]?.SchemeCode}-${foundItem.TSPCode ?? ''}-${seq}`)
    }
    );

    this.populatedTableList.forEach(x => {
      this.highlightInvalidPopulatedList(x);
    })



  }




  onChangeStipend(value: any) {
    this.notForm.controls.Stipend.patchValue(value);
    this.populatedTableList.forEach(x => x.Stipend == value);
    this.Class.controls.forEach((rowForm: FormGroup) => {
      rowForm.controls.Stipend.patchValue(value);
    });
  }
  //onChangeUniformBag(value: any) {
  //  this.notForm.controls.UniformBagCost.patchValue(value);
  //  this.Class.controls.forEach((rowForm: FormGroup) => {
  //    rowForm.controls.UniformBagCost.patchValue(value);
  //  });
  //}
  getData() {
    this.http.getJSON('api/Class/GetClass').subscribe((d: any) => {
      // console.log(d);
      // after  optimization

      // this.sectors = d[0];
      // this.genderOfTrainee = d[1];
      // this.districts = d[2];
      // this.tehsils = d[3];
      // this.clusters = d[4];
      // this.education = d[5];
      // this.certificationAgency = d[6];
      // this.trades = d[7]
      // this.TradeDetailMap = d[8];
      // this.durations = d[9];
      // this.sourceOfCurriculums = d[10];
      // this.province = d[11];



      // before  optimization
      this.sectors = d[1];
      this.genderOfTrainee = d[2];
      this.districts = d[3];
      //this.DistrictsF = d[3];
      this.tehsils = d[4];
      //this.TehsilsF = d[4];
      this.clusters = d[5];
      this.education = d[6];
      this.certificationAgency = d[7];
      this.trades = d[8]
      this.TradeDetailMap = d[10];
      this.durations = d[11];
      this.sourceOfCurriculums = d[12];
      this.province = d[14];
      this.registrationAuthority = d[15].Value;
      this.programFocus = d[16].Value;
      // console.log(this.programFocus);
      
    }, error => this.error = error
    );
  }

  SearchTrade = new FormControl('');
  EmptyCtrl(ev: any) {
    this.SearchTrade.setValue('');

  }

  getNewRow() {
    let form = this._formBuilder.group({ ...this.classFormGroup, ClassCode: ['0', Validators.required] });
    //let tspControl = form.get('TSPID');
    //let classCodeControl = form.get('ClassCode');
    //tspControl.valueChanges.subscribe(
    //  value => {
    //    if (value) {
    //      let foundItem = this.tsps.find(x => x.TSPID == value);
    //      if (foundItem) {
    //        let seq = classCodeControl.value.split('-');
    //        seq = seq[seq.length - 1];
    //        classCodeControl.patchValue(`${this.scheme[0]?.SchemeCode}-${foundItem.TSPCode ?? ''}-${seq}`);
    //      }
    //    }
    //  }
    //);
    return form;
  }
  fillForm(Classes: any) {
    this.Class.clear();

    for (let i of Classes) {

      let form = this.getNewRow();

      //if (!this.isRequiredField(this.notForm, "GeoTagging")) {
      //  this.updateGeoTaggingControl(form.controls.GeoTagging, false);
      //}
      //if (!this.isRequiredField(this.notForm, "TrainingAfddressLocation")) {
      //  this.updateTrainingLocationControl(form.controls.TrainingAddressLocation, false);
      //}

      //if (i.Latitude != '' && i.Longitude != '') {
      //  form.controls.GeoTagging.patchValue(i.Latitude + " , " + i.Longitude);
      //}

      this.Class.push(form);
      form.patchValue(i);

      //form.controls.SalesTaxRate.patchValue(i.SalesTax / i.TrainingCostPerTraineePerMonthInTax);
      form.controls.EntryQualification.patchValue(parseInt(i.EntryQualification));
      form.markAllAsTouched();

      this.populatedTableList = Classes;
    }
  }
  getClassSequence(noOfSeqs: any): Observable<Object> {
    return this.http.getJSON("api/Class/GetClassSequence/" + noOfSeqs);
  }
  calculations() {
    //Below Code to generate classes from E-Tendering System Excel file
    let ET_TotalTrainees = parseInt(this.notForm.controls.TotalTrainees.value);
    let ET_TraineesPerClass = parseInt(this.notForm.controls.TraineesPerClass.value);
    let ET_NumberofBatches = parseInt(this.notForm.controls.Batch.value);
    let ET_NumberofClassesExcel = parseInt(this.notForm.controls.TotalClasses.value);
    let duration = parseFloat(this.notForm.controls.Duration.value);

    //let roundedClassCount1 = this.getClassCount(ET_TotalTrainees, ET_TraineesPerClass);

    if (ET_TraineesPerClass * ET_NumberofBatches * ET_NumberofClassesExcel !== ET_TotalTrainees) {
      this.http.ShowError("Total trainees and Number of Classes to be generated are not according to rule.");
      return false;
    }

    let dataArray = [];

    let date: Date = new Date(this.notForm.controls.StartDate.value._i.year, this.notForm.controls.StartDate.value._i.month, this.notForm.controls.StartDate.value._i.date);
    let isFloat: boolean = this.GetDurationType(duration);

    for (let j = 1; j <= ET_NumberofBatches; j++) {
      let allTrainees = ET_TotalTrainees;

      //let sd: any = "" + this.notForm.controls.StartDate.value._i.year + "-" + this.notForm.controls.StartDate.value._i.month + "-" + this.notForm.controls.StartDate.value._i.date
      //let ed: any = "" + this.notForm.controls.EndDate.value._i.year + "-" + this.notForm.controls.EndDate.value._i.month + "-" + this.notForm.controls.EndDate.value._i.date

      //this.classEndDateNextBatch = sd;
      //this.classStartDateNextBatch = ed;

      dataArray.push({
        'batch': j,
        'trainees': ET_TraineesPerClass,
        'classStartDate': this.GetStartDate(date),
        'classEndDate': this.GetEndDate(isFloat, date, duration)
      });

      date = this.GetEndDate(isFloat, date, duration);

      allTrainees = allTrainees - ET_TraineesPerClass;
    }


    //let classSeq = 0;
    this.getClassSequence(dataArray.length).subscribe(
      seq => {
        dataArray.forEach(
          (item: any, index: any) => {
            let TotalCost_temp = this.calculateTotalCost(
              this.notForm.controls.TrainingCostPerTraineePerMonthInTax.value,
              parseFloat(this.notForm.controls.Duration.value),
              parseInt(this.notForm.controls.BoardingAllowancePerTrainee.value),
              parseInt(this.notForm.controls.Stipend.value),
              parseInt(this.notForm.controls.PerTraineeTestCertCost.value),
              parseInt(this.notForm.controls.UniformBagCost.value),
              parseInt(this.notForm.controls.balloonpayment.value),
              parseInt(this.notForm.controls.GuruPayment.value),
              parseInt(this.notForm.controls.Transportation.value),
              parseInt(this.notForm.controls.ProtectorateandVisa.value),
              parseInt(this.notForm.controls.MedicalCost.value),
              parseInt(this.notForm.controls.PrometricCost.value),
              parseInt(this.notForm.controls.OtherTrainingCost.value),
              parseInt(item.trainees)
            );

            let form = this.getNewRow();
            //let validator = this.notForm.controls.GeoTagging.validator;
            if (!this.isRequiredField(this.notForm, "GeoTagging")) {
              this.updateGeoTaggingControl(form.controls.GeoTagging, false);
            }
            if (!this.isRequiredField(this.notForm, "TrainingAddressLocation")) {
              this.updateTrainingLocationControl(form.controls.TrainingAddressLocation, false);
            }
            this.Class.push(form);

            let tsp = this.tsps.find(x => x.TSPID == this.notForm.controls.TSPID.value)
            let tspcode = tsp?.TSPCode ?? '';
            let tspName = tsp?.TSPName ?? '';
            let tradeName = this.trades.find(x => x.TradeID == this.notForm.controls.TradeID.value).TradeName ?? '';
            let genderName = this.genderOfTrainee.find(x => x.GenderID == this.notForm.controls.GenderID.value).GenderName || '';
            let certAuthName = this.certificationAgency.find(x => x.CertAuthID == this.notForm.controls.CertAuthID.value)?.CertAuthName || '';
            
            let RegistrtionAuthorityName = this.registrationAuthority.find(x => x.RegistrationAuthorityID == this.notForm.controls.RegistrationAuthorityID.value)?.RegistrtionAuthorityName || '';
            let ProgramFocusName = this.programFocus.find(x => x.ProgramFocusID == this.notForm.controls.programFocusID.value)?.ProgramFocusName || '';

            let clusterName = this.clusters.find(x => x.ClusterID == this.notForm.controls.ClusterID.value)?.ClusterName || '';
            let ProvinceName = this.province.find(x => x.ProvinceID == this.notForm.controls.ProvinceID.value)?.ProvinceName || '';
            let districtName = this.districts.find(x => x.DistrictID == this.notForm.controls.DistrictID.value)?.DistrictName || '';
            let tehsilName = this.tehsils.find(x => x.TehsilID == this.notForm.controls.TehsilID.value)?.TehsilName || '';
            let sectorName = this.sectors.find(x => x.SectorID == this.notForm.controls.SectorID.value)?.SectorName || '';
            let education = this.education.find(x => x.EducationTypeID == this.notForm.controls.EntryQualification.value)?.Education || '';
            let durationID = this.durations.find(x => x.Duration == this.notForm.controls.Duration.value)?.DurationID || '';


            //Assigns trade detail map ID
            let tradeDetailID = this.checkAssignTradeDetailMapID(this.notForm.value);

            if (!tradeDetailID) {
              this.http.ShowError("No Trade Detail found against given specification for Trade");
              return;
            }


            this.tableList['TSPID'] = this.notForm.controls.TSPID ?? '',
              this.tableList['TSPName'] = tspName ?? '',
              this.tableList['SectorID'] = this.notForm.controls.SectorID,
              this.tableList['SectorName'] = sectorName,
              this.tableList['TradeID'] = this.notForm.controls.TradeID,
              this.tableList['TradeName'] = tradeName,
              this.tableList['ClassCode'] = 0, //classCode,
              this.tableList['Duration'] = this.notForm.controls.Duration.value,
              this.tableList['DurationID'] = durationID,
              this.tableList['SourceOfCurriculum'] = this.notForm.controls.SourceOfCurriculum.value,
              this.tableList['EducationTypeID'] = this.notForm.controls.EntryQualification.value,
              this.tableList['Education'] = education,
              this.tableList['CertAuthID'] = this.notForm.controls.certAuthID,
              this.tableList['CertAuthName'] = certAuthName,


              this.tableList['RegistrationAuthorityID'] = this.notForm.controls.RegistrationAuthorityID,
              this.tableList['RegistrtionAuthorityName'] = RegistrtionAuthorityName,


              this.tableList['ProgramFocusID'] = this.notForm.controls.ProgramFocusID,
              this.tableList['ProgramFocusName'] = ProgramFocusName,

              //this.tableList['//AttendanceStandardPercentage']= f['Attendance Standard Percentage'],
              //Below 2 Columns are calcutated from above 4 columns
              this.tableList['TraineesPerClass'] = this.notForm.controls.TraineesPerClass.value,
              this.tableList['Batch'] = this.notForm.controls.Batch.value,
              this.tableList['MinHoursPerMonth'] = this.notForm.controls.MinHoursPerMonth.value,
              this.tableList['StartDate'] = this.notForm.controls.StartDate.value,
              this.tableList['EndDate'] = this.notForm.controls.EndDate.value,
              this.tableList['GenderID'] = this.notForm.controls.genderID,
              this.tableList['GenderName'] = genderName,
              this.tableList['TrainingAddressLocation'] = this.notForm.controls.TrainingAddressLocation.value,
              this.tableList['GeoTagging'] = this.notForm.controls.GeoTagging.value,
              this.tableList['DistrictID'] = this.notForm.controls.DistrictID.value,
              this.tableList['DistrictName'] = districtName,
              this.tableList['TehsilID'] = this.notForm.controls.TehsilID.value,
              this.tableList['TehsilName'] = tehsilName,
              this.tableList['ClusterID'] = this.notForm.controls.ClusterID.value,
              this.tableList['ClusterName'] = clusterName,
              this.tableList['ProvinceID'] = this.notForm.controls.ProvinceID.value,
              this.tableList['ProvinceName'] = ProvinceName,
              this.tableList['BidPrice'] = this.notForm.controls.BidPrice.value,
              this.tableList['BMPrice'] = this.notForm.controls.BMPrice.value,
              this.tableList['TotalPerTraineeCostInTax'] = this.notForm.controls.TotalPerTraineeCostInTax.value, //(TrainingCostPerTraineePerMonthIncTaxes_temp * this.notForm.controls.Duration in Months']),
              this.tableList['SalesTaxRate'] = this.notForm.controls.SalesTaxRate,
              this.tableList['TrainingCostPerTraineePerMonthExTax'] = this.notForm.controls.TrainingCostPerTraineePerMonthExTax.value,
              this.tableList['SalesTax'] = this.notForm.controls.SalesTax.value,
              this.tableList['TrainingCostPerTraineePerMonthInTax'] = this.notForm.controls.TrainingCostPerTraineePerMonthInTax.value,
              this.tableList['UniformBagCost'] = this.notForm.controls.UniformBagCost.value,
              this.tableList['PerTraineeTestCertCost'] = this.notForm.controls.PerTraineeTestCertCost.value,
              this.tableList['BoardingAllowancePerTrainee'] = this.notForm.controls.BoardingAllowancePerTrainee.value,
              this.tableList['EmploymentCommitmentSelf'] = this.notForm.controls.EmploymentCommitmentSelf.value,
              this.tableList['EmploymentCommitmentFormal'] = this.notForm.controls.EmploymentCommitmentFormal.value,
              this.tableList['OverallEmploymentCommitment'] = this.notForm.controls.OverallEmploymentCommitment.value,
              this.tableList['Stipend'] = Math.round(this.notForm.controls.Stipend.value),
              this.tableList['TotalCostPerClass'] = this.notForm.controls.TotalCostPerClass.value,
              this.tableList['TradeDetailMapID'] = tradeDetailID,
              this.tableList['IsEditable'] = false,
              this.tableList['balloonpayment'] = this.notForm.controls.balloonpayment.value,
              this.tableList['GuruPayment'] = this.notForm.controls.GuruPayment.value,
              this.tableList['Transportation'] = this.notForm.controls.Transportation.value,
              this.tableList['MedicalCost'] = this.notForm.controls.MedicalCost.value,  //Added by Rao Ali Haider for International Plac
              this.tableList['PrometricCost'] = this.notForm.controls.PrometricCost.value,
              this.tableList['OtherTrainingCost'] = this.notForm.controls.OtherTrainingCost.value,
              this.tableList['ProtectorateandVisa'] = this.notForm.controls.ProtectorateandVisa.value
              this.populatedTableList.push(this.tableList);
            this.tableList = [];

            form.patchValue({
              TSPID: this.notForm.controls.TSPID.value,
              SectorID: this.notForm.controls.SectorID.value,
              TradeID: this.notForm.controls.TradeID.value,
              ClassCode: 0,// `${this.scheme[0]?.SchemeCode}-${tspcode}-${seq[index]}`,
              Duration: this.notForm.controls.Duration.value,
              SourceOfCurriculum: this.notForm.controls.SourceOfCurriculum.value,
              EntryQualification: this.notForm.controls.EntryQualification.value,
              CertAuthID: this.notForm.controls.CertAuthID.value,

              RegistrationAuthorityID: this.notForm.controls.RegistrationAuthorityID.value,
              ProgramFocusID: this.notForm.controls.ProgramFocusID.value,
              //AttendanceStandardPercentage: this.notForm.controls.AttendanceStandardPercentage.value,
              TraineesPerClass: item.trainees,
              Batch: item.batch,
              MinHoursPerMonth: this.notForm.controls.MinHoursPerMonth.value,
              StartDate: new Date(item.classStartDate),
              EndDate: new Date(item.classEndDate),
              GenderID: this.notForm.controls.GenderID.value,
              TrainingAddressLocation: this.notForm.controls.TrainingAddressLocation.value,
              GeoTagging: this.notForm.controls.GeoTagging.value,
              DistrictID: this.notForm.controls.DistrictID.value,
              TehsilID: this.notForm.controls.TehsilID.value,
              ClusterID: this.notForm.controls.ClusterID.value,
              ProvinceID: this.notForm.controls.ProvinceID.value,
              BidPrice: this.notForm.controls.BidPrice.value,
              BMPrice: this.notForm.controls.BMPrice.value == null ? '' : this.notForm.controls.BMPrice.value,
              TotalPerTraineeCostInTax: this.notForm.controls.TotalPerTraineeCostInTax.value,
              SalesTaxRate: this.notForm.controls.SalesTaxRate.value,
              TrainingCostPerTraineePerMonthExTax: this.notForm.controls.TrainingCostPerTraineePerMonthExTax.value,
              SalesTax: this.notForm.controls.SalesTax.value,
              TrainingCostPerTraineePerMonthInTax: this.notForm.controls.TrainingCostPerTraineePerMonthInTax.value,
              UniformBagCost: this.notForm.controls.UniformBagCost.value,
              PerTraineeTestCertCost: this.notForm.controls.PerTraineeTestCertCost.value,
              BoardingAllowancePerTrainee: this.notForm.controls.BoardingAllowancePerTrainee.value,
              EmploymentCommitmentSelf: this.notForm.controls.EmploymentCommitmentSelf.value,
              EmploymentCommitmentFormal: this.notForm.controls.EmploymentCommitmentFormal.value,
              OverallEmploymentCommitment: this.notForm.controls.OverallEmploymentCommitment.value,
              Stipend: this.notForm.controls.Stipend.value,
              TotalCostPerClass: TotalCost_temp,
              balloonpayment: this.notForm.controls.balloonpayment.value,
              GuruPayment: this.notForm.controls.GuruPayment.value,
              Transportation: this.notForm.controls.Transportation.value,
              ProtectorateandVisa: this.notForm.controls.ProtectorateandVisa.value,  //Added by Rao Ali Haider for International Plac
              MedicalCost: this.notForm.controls.MedicalCost.value,
              PrometricCost: this.notForm.controls.PrometricCost.value,
              OtherTrainingCost: this.notForm.controls.OtherTrainingCost.value
              
            }, { emitEvent: true });
          });
      });
  }
  GetDurationType(duration: number): boolean {
    return duration % Math.round(duration) == 0 ? false : true;
  }

  GetEndDate(isFloat: boolean, date: Date, duration: any): Date {
    if (!isFloat)
      return new Date(date.getFullYear(), date.getMonth() + duration, date.getDate());
    else
      return new Date(date.getFullYear(), date.getMonth(), date.getDate() + (duration * 30));
  }

  GetStartDate(date: Date): Date {
    return new Date(date.getFullYear(), date.getMonth(), date.getDate() + 1);
  }

  SetEndDate() {
    if (this.notForm.controls.Duration.value == '' || this.notForm.controls.StartDate.value == '')
      return

    let duration = parseFloat(this.notForm.controls.Duration.value);

    let isFloat: boolean = this.GetDurationType(duration);
    let date: Date = new Date(this.notForm.controls.StartDate.value._i.year, this.notForm.controls.StartDate.value._i.month, this.notForm.controls.StartDate.value._i.date);
    let endDate: Date = this.GetEndDate(isFloat, date, duration);
    this.notForm.controls.EndDate.setValue(endDate);
  }

  isRequiredField(form: FormGroup, field: string): boolean {
    const form_field = form.get(field);
    if (!form_field.validator) {
      return false;
    }

    const validator = form_field.validator({} as AbstractControl);
    return validator ? validator.required : false;
  }
  pushFormtoTable() {
    this.ZeroCheck();

    if (this.notForm.invalid)
      return;

    if (!this.calculations())
      return false;
  }
  ZeroCheck() {
    let ls: any[] = [];
    ls.push(this.notForm.controls.TotalTrainees);
    ls.push(this.notForm.controls.TotalClasses);
    ls.push(this.notForm.controls.TraineesPerClass);
    ls.push(this.notForm.controls.Batch);
    ls.push(this.notForm.controls.MinHoursPerMonth);

    ls.forEach(c => {
      if (parseFloat(c.value) == 0) {
        c.setErrors({ zero: true });
      }
      else {
        c.updateValueAndValidity();
      }
    });

    this.notForm.markAllAsTouched();
  }
  submitClass(nform: FormGroupDirective) {
    if (!this.classForm.valid)
      return;

    //this.working = true;
    if (this.scheme.length == 0) {
      this.http.ShowError("Save scheme first.");
      return;
    } else if (this.tsps.length == 0) {
      this.http.ShowError("Save Tsps first.");
      return;
    } else {
      let classes = this.classForm.value.Class
      
      classes = classes.map((x: any) => {
        let geoTagging = x.GeoTagging?.split(',') ?? [];
        return {
          ...x
          , SchemeID: this.scheme[0].SchemeID
          , OrganizationID: this.scheme[0].OrganizationID
          , MinAge: this.scheme[0].MinAge
          , MaxAge: this.scheme[0].MaxAge
          , Latitude: geoTagging.length > 0 ? geoTagging[0].toString().trim() : ''
          , Longitude: geoTagging.length > 1 ? geoTagging[1].toString().trim() : ''
        }
      });
      this.http.postJSON('api/Class/Save', JSON.stringify(classes))
        .subscribe((d: any) => {
          this.insertedClass = d;
          this.classData.emit(this.insertedClass);
          this.next.emit(true);
          this.reset(nform);

          this.emptyClass();
          this.fillForm(this.insertedClass);
          this.http.openSnackBar(environment.SaveMSG.replace("${Name}", this.EnText), this.EnText);
          this.stepper.next();

        },
          (error) => {
            this.error = error.error;
            this.http.ShowError(error.error);

          });

    }

  }
  reset(nform: FormGroupDirective) {
    nform.resetForm();
  }
  dateFilter = (d: Date | null): boolean => {
    if (!this.scheme[0]?.FinalSubmitted) {
      const date = (d || new Date());
      return date >= new Date();
      //return date >=new Date(date.getFullYear(), date.getMonth(), 1);
    }
    return true;
  }
  removeClass(ind, r: any) {
    //this.Class.removeAt(ind);

    //let id = r.controls.ClassID.value
    let id = r.ClassID
    this.populatedTableList.splice(ind, 1);

    if (id != 0) {
      this.http.getJSON(`api/Appendix/RemoveFromAppendix?formID=${id}&form=${EnumAppendixModules.Class}`)
        .subscribe((d: any) => {
        }, error => this.error = error // error path
          , () => {
            //this.working = false;
          });
    }
  }
  salesTaxCalculation() {
    let SaleTaxRate = parseFloat(this.notForm.controls.SalesTaxRate.value);

    if (this.notForm.controls.SalesTaxRate.value != '' && SaleTaxRate >= 1) {
      this.notForm.controls.SalesTaxRate.setErrors({ limit: true })

      return;
    }
    else
      this.notForm.controls.SalesTaxRate.updateValueAndValidity();

    if (this.notForm.controls.SalesTaxRate.value == '' ||
      this.notForm.controls.Duration.value == '' ||
      this.notForm.controls.TrainingCostPerTraineePerMonthInTax.value == '') {
      return;
    }

    let duration = parseFloat(this.notForm.controls.Duration.value);
    let TrainingCostPerTraineePerMonthInTax = this.notForm.controls.TrainingCostPerTraineePerMonthInTax.value;
    let SalesTaxAmount = this.calculateSalesTax(SaleTaxRate, TrainingCostPerTraineePerMonthInTax);
    let PerTraineePerMonthExTax = this.calculateTrainingCostPerTraineePerMonthExTaxes(TrainingCostPerTraineePerMonthInTax, SaleTaxRate);

    //let PerTraineeTotalCost = parseFloat((TrainingCostPerTraineePerMonthInTax * duration).toFixed(this.decimalPlaces));
    let PerTraineeTotalCost = 0;
    if (duration < 1) { PerTraineeTotalCost = parseFloat((TrainingCostPerTraineePerMonthInTax).toFixed(this.decimalPlaces)); }
    else { PerTraineeTotalCost = parseFloat((TrainingCostPerTraineePerMonthInTax * duration).toFixed(this.decimalPlaces)); }

    this.notForm.controls.TotalPerTraineeCostInTax.setValue(PerTraineeTotalCost);
    this.notForm.controls.SalesTax.setValue(SalesTaxAmount);
    this.notForm.controls.TrainingCostPerTraineePerMonthExTax.setValue(PerTraineePerMonthExTax);
    this.notForm.controls.TrainingCostPerTraineePerMonthInTax.setValue(TrainingCostPerTraineePerMonthInTax);
  }
  setOverallEmpComm() {
    let self = this.notForm.controls.EmploymentCommitmentSelf.value;
    let formal = this.notForm.controls.EmploymentCommitmentFormal.value;
    if (self == "" || formal == "") {
      return;
    }
    self = parseInt(self);
    formal = parseInt(formal);
    this.notForm.controls.OverallEmploymentCommitment.setValue(self + formal);
    if (self + formal > 100) {
      this.notForm.controls.OverallEmploymentCommitment.setErrors({ greater: true });
    }
    else {
      this.notForm.controls.OverallEmploymentCommitment.updateValueAndValidity();
    }
    this.notForm.controls.OverallEmploymentCommitment.markAsTouched();
  }
  checkDuration(cont) {
    if (cont.value <= 0) {
      cont.setErrors({ less: true });
    }
    else {
      cont.updateValueAndValidity();
    }
  }
  percentageCheck(cont) {
    if (cont.value > 100 || cont.value < 0) {
      cont.setErrors({ limit: true })
    }
    else {
      cont.updateValueAndValidity();
    }
  }
  populateFieldsFromFile(_classData: any, stepper: MatStepper = null) {
    debugger;
    this.stepper = stepper;
    this.Class.clear();
    if (_classData.length == 0) {
      return;
    }
    for (let f of _classData) {
      f = this.http.TrimFields(f);

      //Below code will be used to enter records in tableList Array
      //this.tableList['SectorID'] = this.sectors.find(x => f["Sector"]?.toLowerCase() == x.SectorName.toLowerCase())?.SectorID || '';
      //this.tableList['SectorName'] = this.sectors.find(x => f["Sector"]?.toLowerCase() == x.SectorName.toLowerCase())?.SectorName || '';



      //Below Code to generate classes from E-Tendering System Excel file
      let ET_TotalTrainees = parseInt(f['Total Trainees']);
      let ET_TraineesPerClassTemp = parseInt(f['Trainees per Class']);
      let ET_TraineesPerClass = parseInt(f['Trainees per Class']);
      let ET_NumberofBatches = parseInt(f['Number of Batches']);
      let ET_NumberofClassesExcel = parseInt(f['Number of Classes']);
      //let ET_NumberofClasses = parseInt((ET_TotalTrainees / ET_TraineesPerClass).toString());
      //let roundedClassCount1 = this.getClassCount(ET_TotalTrainees, ET_TraineesPerClass);

      if (ET_TraineesPerClassTemp * ET_NumberofBatches * ET_NumberofClassesExcel !== ET_TotalTrainees) {
        this.http.ShowError("Total trainees and Number of Classes to be generated are not according to rule.");
        return false;
      }
      /// class code should be tsp wise in sequence( minimum 4 digits )
      /// class start should be 1 to 5 or 15-16 date of the month , other wise show error alert

      let rows = [];
      let classStartDate = new Date(Math.round((f['Start Date'] - 25569) * 86400 * 1000));

      if (this.validateClassStartDate(classStartDate)) {
        alert(`Please Reload file. /n Error : InValid ClassStartDate ${classStartDate} , it must be (in-between 1 to 5 ) or (15 or 16) date of month.`)
      }
      for (var classNumber = 1; classNumber <= ET_NumberofClassesExcel; classNumber++) {
        let start = moment(classStartDate);
        for (var batch = 1; batch <= ET_NumberofBatches; batch++) {
          //debugger;
          let duration = f['Duration in Months'];
          let isFloat: boolean = duration % Math.round(duration) == 0 ? false : true;
          start = batch == 1 ? start : isFloat ? start.add(duration * 30, 'days') : start.toDate().getDate() > 1 ? start.add(duration, 'months') : start.add(duration, 'months');
          let end = moment(start.toDate());
          //end = isFloat ? end.add(duration * 30, 'days') : end.toDate().getDate() > 1 ? end.add(duration, 'months').subtract(1, 'days') : end.add(duration, 'months').subtract(1, 'days');
          end = isFloat ? end.add(duration * 30, 'days').subtract(1, 'days')
            : end.toDate().getDate() > 1 ? end.add(duration, 'months').subtract(1, 'days') : end.add(duration, 'months').subtract(1, 'days');

          rows.push({
            'batch': batch,
            'trainees': ET_TraineesPerClass,
            'classStartDate': start.toDate(),
            'classEndDate': end.toDate()
          });
        }
      }

      let regAuthorityID = this.registrationAuthority.find(x => f["Registration Authority"]?.toLowerCase() == x.RegistrationAuthorityName.toLowerCase())?.RegistrationAuthorityID || '';
      let regAuthorityName = this.registrationAuthority.find(x => f["Registration Authority"]?.toLowerCase() == x.RegistrationAuthorityName.toLowerCase())?.RegistrationAuthorityName || '';


      
      let programFocusID = this.programFocus.find(x => f['Program Focus']?.toLowerCase()   == x.ProgramFocusName.toLowerCase())?.ProgramFocusID || '';
      let ProgramFocusName = this.programFocus.find(x => f['Program Focus']?.toLowerCase() == x.ProgramFocusName.toLowerCase())?.ProgramFocusName || '';

      

      let sectorID = this.sectors.find(x => f["Sector"]?.toLowerCase() == x.SectorName.toLowerCase())?.SectorID || '';
      let sectorName = this.sectors.find(x => f["Sector"]?.toLowerCase() == x.SectorName.toLowerCase())?.SectorName || '';
      let tradeID = this.trades.find(x => f["Trade Name"]?.toLowerCase() == x.TradeName.toLowerCase())?.TradeID || '';
      let tradeName = this.trades.find(x => f["Trade Name"]?.toLowerCase() == x.TradeName.toLowerCase())?.TradeName || '';
      let educationTypeID = this.education.find(x => f["Entry Qualification"]?.toLowerCase() == x.Education.toLowerCase())?.EducationTypeID || '';
      let education = this.education.find(x => f["Entry Qualification"]?.toLowerCase() == x.Education.toLowerCase())?.Education || '';
      let certAuthID = this.certificationAgency.find(x => f["Certification Authority"]?.toLowerCase() == x.CertAuthName.toLowerCase())?.CertAuthID || '';
      let certAuthName = this.certificationAgency.find(x => f["Certification Authority"]?.toLowerCase() == x.CertAuthName.toLowerCase())?.CertAuthName || '';
      let genderID = this.genderOfTrainee.find(x => f["Trainees Gender"]?.toLowerCase() == x.GenderName.toLowerCase())?.GenderID || '';
      let genderName = this.genderOfTrainee.find(x => f["Trainees Gender"]?.toLowerCase() == x.GenderName.toLowerCase())?.GenderName || '';

      let ProvinceID = this.province.find(x => f["Province"]?.toLowerCase() == x.ProvinceName.toLowerCase())?.ProvinceID || '';
      let ProvinceName = this.province.find(x => f["Province"]?.toLowerCase() == x.ProvinceName.toLowerCase())?.ProvinceName || '';

      let clusterID = this.clusters.find(x => f["Cluster"]?.toLowerCase() == x.ClusterName.toLowerCase() && x.ProvinceID == ProvinceID)?.ClusterID || '';
      let clusterName = this.clusters.find(x => f["Cluster"]?.toLowerCase() == x.ClusterName.toLowerCase() && x.ProvinceID == ProvinceID)?.ClusterName || '';

      let districtID = this.districts.find(x => f["District"]?.toLowerCase() == x.DistrictName.toLowerCase() && x.ClusterID == clusterID)?.DistrictID || '';
      let districtName = this.districts.find(x => f["District"]?.toLowerCase() == x.DistrictName.toLowerCase() && x.ClusterID == clusterID)?.DistrictName || '';
      let tehsilID = this.tehsils.find(x => f["Tehsil"]?.toLowerCase() == x.TehsilName.toLowerCase() && x.DistrictID == districtID)?.TehsilID || '';
      let tehsilName = this.tehsils.find(x => f["Tehsil"]?.toLowerCase() == x.TehsilName.toLowerCase() && x.DistrictID == districtID)?.TehsilName || '';

      //Trade detail map with class
      let relevantTradeDetail: any[];
      relevantTradeDetail = this.TradeDetailMap.filter(x => x.TradeID == tradeID);
      let durationID = this.durations.find(x => f["Duration in Months"] == x.Duration).DurationID;

      //let tradeDetailID = relevantTradeDetail.find(x => x.DurationID = durationID && x.CertAuthID == certAuthID).TradeDetailMapID;
      let reltradeDetailID = relevantTradeDetail.filter(x => x.DurationID == durationID && x.CertAuthID == certAuthID).map(y => y.TradeDetailMapID);
      //let reltradeDetailID = relevantTradeDetail.map(x => x.DurationID = durationID && x.CertAuthID == certAuthID);

      //let tradeDetailID = reltradeDetailID[0];
      //let tradeDetailID = this.checkAssignTradeDetailMapID(f);

      //if (!tradeDetailID) {
      //  this.http.ShowError("No Trade Detail found against given specification in highlighted row.");
      //  console.log("this row should be highlighted");
      //  //return false;
      //}

      let tsp: TSPModel = this.tsps?.find(x => f["TSP Name"]?.toLowerCase() == x.TSPName.toLowerCase());

      //this.getClassSequence(rows.length).subscribe(
      //  seq => {
      rows.forEach(
        (item: any, index: any) => {
          let TotalTraineeCost_temp = parseInt(f['Total Trainee Cost']);
          let SalesTaxRate_temp = parseFloat(f['Sales Tax Rate']);
          let Duration_temp = parseFloat(f['Duration in Months']);
          let TrainingCostPerTraineePerMonthIncTaxes_temp = parseFloat(f['Training Cost per Trainee per Month (Inclusive  of Taxes)']?.toFixed(this.decimalPlaces));
          let SalesTax_temp = this.calculateSalesTax(SalesTaxRate_temp, TrainingCostPerTraineePerMonthIncTaxes_temp);
          let TrainingCostPerTraineePerMonthExTaxes_temp = this.calculateTrainingCostPerTraineePerMonthExTaxes(TrainingCostPerTraineePerMonthIncTaxes_temp, SalesTaxRate_temp);
          let TotalCost_temp = this.calculateTotalCost
            (
              TrainingCostPerTraineePerMonthIncTaxes_temp,
              Duration_temp,
              parseInt(f['Boarding & Other Allowances per trainee']),
              //parseInt(f['Stipend']),
              parseInt(this.notForm.controls.Stipend.value),
              parseInt(f['Testing & Certification Fee per Trainee']),
              parseInt(f['Uniform & Bag Cost per Trainee / Hostel']),
              //parseInt(this.notForm.controls.UniformBagCost.value),
              parseInt(f['On Job Training (OJT)']),
              parseInt(f['Guru Payment']),
              parseInt(f['Transportation']),
              parseInt(f['Protectorate and Visa Stamping']),
              parseInt(f['Medical cost']),
              parseInt(f['Prometric costs']),
              parseInt(f['Other Training and supporting cost']),
              item.trainees

              //this.notForm.controls.TrainingCostPerTraineePerMonthInTax.value,
              //parseFloat(this.notForm.controls.Duration.value),
              //parseInt(this.notForm.controls.BoardingAllowancePerTrainee.value),
              //parseInt(this.notForm.controls.Stipend.value),
              //parseInt(this.notForm.controls.PerTraineeTestCertCost.value),
              //parseInt(this.notForm.controls.UniformBagCost.value),
              //parseInt(item.trainees)

            );

          // let classCode = `${this.scheme[0]?.SchemeCode ?? ''}-${tsp?.TSPCode ?? ''}-${seq[index]}`;
          let form = this.getNewRow();
          //let validator = this.notForm.controls.GeoTagging.validator;
          //if (!(validator && validator({} as AbstractControl).required)) {
          //  this.updateGeoTaggingControl(form.controls.GeoTagging, false);
          //}
          if (!this.isRequiredField(this.notForm, "GeoTagging")) {
            this.updateGeoTaggingControl(form.controls.GeoTagging, false);
          }
          if (!this.isRequiredField(this.notForm, "TrainingAddressLocation")) {
            this.updateTrainingLocationControl(form.controls.TrainingAddressLocation, false);
          }
          this.Class.push(form);

          let durationF = f['Duration in Months'];
          let TotalPerTraineeCostInTaxF = TrainingCostPerTraineePerMonthIncTaxes_temp;
          if (durationF > 1) { TotalPerTraineeCostInTaxF = TrainingCostPerTraineePerMonthIncTaxes_temp * durationF; }

          this.tableList['TSPID'] = tsp?.TSPID ?? '',
            this.tableList['TSPName'] = tsp?.TSPName ?? f["TSP Name"],
            this.tableList['SectorID'] = sectorID,
            this.tableList['SectorName'] = f["Sector"],
            this.tableList['TradeID'] = tradeID,
            this.tableList['TradeName'] = tradeName,
            this.tableList['ClassCode'] = 0, //classCode,
            this.tableList['DurationID'] = durationID,
            this.tableList['Duration'] = f['Duration in Months'],
            this.tableList['SourceOfCurriculum'] = f['Source of Curriculum'],
            this.tableList['EntryQualification'] = educationTypeID,
            this.tableList['Education'] = education,
            this.tableList['CertAuthID'] = certAuthID,
            this.tableList['CertAuthName'] = certAuthName,
            this.tableList['ProgramFocusID'] = programFocusID,
            this.tableList['ProgramFocusName'] = ProgramFocusName,
            this.tableList['RegistrationAuthorityID'] = regAuthorityID,
            this.tableList['RegistrationAuthorityName'] = regAuthorityName,
            //this.tableList['//AttendanceStandardPercentage']= f['Attendance Standard Percentage'],
            //Below 2 Columns are calcutated from above 4 columns
            this.tableList['TraineesPerClass'] = item.trainees,
            this.tableList['Batch'] = item.batch,
            this.tableList['MinHoursPerMonth'] = parseInt(f['Minimum Training Hours Per Month']),
            this.tableList['StartDate'] = new Date(item.classStartDate),
            this.tableList['EndDate'] = new Date(item.classEndDate),
            this.tableList['GenderID'] = genderID,
            this.tableList['GenderName'] = genderName,
            this.tableList['TrainingAddressLocation'] = f['Address of Training Location'],
            this.tableList['GeoTagging'] = f['Geo Tagging'],
            this.tableList['DistrictID'] = districtID,
            this.tableList['DistrictName'] = districtName,
            this.tableList['TehsilID'] = tehsilID,
            this.tableList['TehsilName'] = tehsilName,
            this.tableList['ClusterID'] = clusterID,
            this.tableList['ClusterName'] = clusterName,
            this.tableList['ProvinceID'] = ProvinceID,
            this.tableList['ProvinceName'] = ProvinceName,
            this.tableList['BidPrice'] = f['Total Trainee Bid Price'],
            this.tableList['BMPrice'] = f['Total Trainee BM Price'],
            this.tableList['TotalPerTraineeCostInTax'] = TotalPerTraineeCostInTaxF, //(TrainingCostPerTraineePerMonthIncTaxes_temp * f['Duration in Months']),
            this.tableList['SalesTaxRate'] = f['Sales Tax Rate'],
            this.tableList['TrainingCostPerTraineePerMonthExTax'] = TrainingCostPerTraineePerMonthExTaxes_temp,
            this.tableList['SalesTax'] = Math.round(SalesTax_temp),
            this.tableList['TrainingCostPerTraineePerMonthInTax'] = TrainingCostPerTraineePerMonthIncTaxes_temp,
            this.tableList['UniformBagCost'] = f['Uniform & Bag Cost per Trainee / Hostel'],
            this.tableList['PerTraineeTestCertCost'] = Math.round(f['Testing & Certification Fee per Trainee']),
            this.tableList['BoardingAllowancePerTrainee'] = Math.round(f['Boarding & Other Allowances per trainee']),
            this.tableList['EmploymentCommitmentSelf'] = Math.round(f['Employment Commitment Self']),
            this.tableList['EmploymentCommitmentFormal'] = Math.round(f['Employment Commitment Formal']),
            this.tableList['OverallEmploymentCommitment'] = Math.round(f['Employment Commitment Self'] + f['Employment Commitment Formal']),
            this.tableList['Stipend'] = Math.round(f['Stipend']),
            this.tableList['TotalCostPerClass'] = Math.round(TotalCost_temp),
            this.tableList['balloonpayment'] = f['On Job Training (OJT)'] ?? 0,
            this.tableList['GuruPayment'] = f['Guru Payment'] ?? 0, 
            this.tableList['Transportation'] = f['Transportation'] ?? 0,
            this.tableList['ProtectorateandVisa'] = f['Protectorate and Visa Stamping'] ?? 0, //Added by Rao Ali Haider for International Placement
            this.tableList['MedicalCost'] = f['Medical cost'] ?? 0,
            this.tableList['PrometricCost'] = f['Prometric costs'] ?? 0,
            this.tableList['OtherTrainingCost'] = f['Other Training and supporting cost'] ?? 0,
            this.tableList['IsEditable'] = false
          
          ///

          let tradeDetailID = this.checkAssignTradeDetailMapID(this.tableList);
// console.log(programFocusID);
// console.log(ProgramFocusName);

          if (!tradeDetailID) {
            this.http.ShowError("No Trade Detail found against given specification in highlighted row.");
            console.log("this row should be highlighted");
            //return false;
          }

          this.tableList['TradeDetailMapID'] = tradeDetailID,

            ///

            this.highlightInvalidPopulatedList(this.tableList)
          this.populatedTableList.push(this.tableList);
// console.log(this.tableList);

          this.tableList = [];

          form.patchValue({
            TSPID: tsp?.TSPID ?? '',
            TSPName: tsp?.TSPName ?? f["TSP Name"],
            SectorID: sectorID,
            TradeID: tradeID,
            ClassCode: 0, //classCode,
            Duration: f['Duration in Months'],
            SourceOfCurriculum: f['Source of Curriculum'],
            EntryQualification: educationTypeID,
            CertAuthID: certAuthID,
            RegistrationAuthorityID: regAuthorityID,
            ProgramFocusID: programFocusID,
            //AttendanceStandardPercentage: f['Attendance Standard Percentage'],
            //Below 2 Columns are calcutated from above 4 columns
            TraineesPerClass: item.trainees,
            Batch: item.batch,
            MinHoursPerMonth: parseInt(f['Minimum Training Hours Per Month']),
            StartDate: new Date(item.classStartDate),
            EndDate: new Date(item.classEndDate),
            GenderID: genderID,
            TrainingAddressLocation: f['Address of Training Location'],
            GeoTagging: f['Geo Tagging'],
            DistrictID: districtID,
            TehsilID: tehsilID,
            ClusterID: clusterID,
            ProvinceID: ProvinceID,
            BidPrice: f['Total Trainee Bid Price'],
            BMPrice: f['Total Trainee BM Price'],
            TotalPerTraineeCostInTax: TotalPerTraineeCostInTaxF, //(TrainingCostPerTraineePerMonthIncTaxes_temp * f['Duration in Months']),
            SalesTaxRate: f['Sales Tax Rate'],
            TrainingCostPerTraineePerMonthExTax: TrainingCostPerTraineePerMonthExTaxes_temp,
            SalesTax: Math.round(SalesTax_temp),
            TrainingCostPerTraineePerMonthInTax: TrainingCostPerTraineePerMonthIncTaxes_temp,
            UniformBagCost: f['Uniform & Bag Cost per Trainee / Hostel'],
            PerTraineeTestCertCost: Math.round(f['Testing & Certification Fee per Trainee']),
            BoardingAllowancePerTrainee: Math.round(f['Boarding & Other Allowances per trainee']),
            EmploymentCommitmentSelf: Math.round(f['Employment Commitment Self']),
            EmploymentCommitmentFormal: Math.round(f['Employment Commitment Formal']),
            OverallEmploymentCommitment: Math.round(f['Employment Commitment Self'] + f['Employment Commitment Formal']),
            Stipend: Math.round(f['Stipend']),
            balloonpayment: Math.round(f['On Job Training (OJT)']), 
            GuruPayment: Math.round(f['Guru Payment']),
            Transportation: Math.round(f['Transportation']),
            ProtectorateandVisa: Math.round(f['Protectorate and Visa Stamping']),  //Added by Rao Ali Haider for International Plac
            MedicalCost: Math.round(f['Medical cost']),
            PrometricCost: Math.round(f['Prometric costs']),
            OtherTrainingCost: Math.round(f['Other Training and supporting cost']),
            TotalCostPerClass: Math.round(TotalCost_temp)
          }, { emitEvent: true });
          form.markAllAsTouched();
        });
      //  })

    }
  }
  validateClassStartDate(classStart: Date) {
    if (!classStart) {
      return false;
    }
    return (classStart.getDate() > 5 && classStart.getDate() < 15) || (classStart.getDate() > 16)
  }
  getClassCount(TotalTrainees, raineesPerClass) {
    return Math.ceil(TotalTrainees / raineesPerClass)
  }
  calculateSalesTax(SalesTaxRate, TrainingCostPerTraineePerMonthIncTaxes) {
    if (SalesTaxRate == 0.0) {
      //return Math.round(TrainingCostPerTraineePerMonthIncTaxes);
      return 0;
    }

    return Math.round((TrainingCostPerTraineePerMonthIncTaxes / (1 + SalesTaxRate)) * SalesTaxRate);
  }
  calculateTrainingCostPerTraineePerMonthExTaxes(TrainingCostPerTraineePerMonthIncTaxes, SalesTaxRate) {
    if (SalesTaxRate == 0.0) {
      return TrainingCostPerTraineePerMonthIncTaxes;
    }

    //return (TrainingCostPerTraineePerMonthIncTaxes / (1 + SalesTaxRate))?.toFixed(2);
    return parseFloat((TrainingCostPerTraineePerMonthIncTaxes / (1 + SalesTaxRate)).toFixed(this.decimalPlaces));
  }
  calculateTotalCost(trainingCostPerTraineePerMonthIncTaxes, duration, boarding, stipend, testingCert, uniformBag, ojt, Guru, transportation, ProtectorateandVisa, MedicalCost, PrometricCost, OtherTrainingCost, trainees) {
    let val = 0
    if (duration < 1) { val = Math.round(((trainingCostPerTraineePerMonthIncTaxes) + (duration * boarding) + (duration * stipend) + (duration * transportation) + testingCert + uniformBag + ojt + (duration * Guru) + ProtectorateandVisa + MedicalCost + PrometricCost +  OtherTrainingCost) * trainees); }
    else { val = Math.round(((trainingCostPerTraineePerMonthIncTaxes * duration) + (duration * boarding) + (duration * stipend) + (duration * transportation) + testingCert + (duration * uniformBag) + ojt + (duration * Guru)) * trainees); }
    return parseFloat(val.toFixed(this.decimalPlaces));
  }
  emptyClass() {
    this.Class.clear();
  }

  //Assigns trade detail map id to class when entered from form

  checkAssignTradeDetailMapID(row: any) {

    let tradeID = row.TradeID;
    let certAuthID = row.CertAuthID;


    //Trade detail map with class
    let relevantTradeDetail: any[];
    relevantTradeDetail = this.TradeDetailMap.filter(x => x.TradeID == tradeID);
    //let durationValue = this.durations.find(x => f["Duration in Months"] == x.Duration).DurationID;
    let durationID = this.durations.find(x => row.Duration == x.Duration).DurationID;
    let sourceOfCurriculumID = this.sourceOfCurriculums.find(x => row.SourceOfCurriculum == x.Name).SourceOfCurriculumID;

    //let tradeDetailID = relevantTradeDetail.find(x => x.DurationID = durationID && x.CertAuthID == certAuthID).TradeDetailMapID;
    let reltradeDetailID = relevantTradeDetail.filter(x => x.DurationID == durationID && x.CertAuthID == certAuthID && x.SourceOfCurriculumID == sourceOfCurriculumID).map(y => y.TradeDetailMapID);
    //let reltradeDetailID = relevantTradeDetail.map(x => x.DurationID = durationID && x.CertAuthID == certAuthID);

    //let tradeDetailID = reltradeDetailID[0];

    //if (tradeDetailID) {
    //  row['TradeDetailMapID'] = tradeDetailID;
    //  this.notForm.controls.TradeDetailMapID.setValue(tradeDetailID);
    //}
    //else {
    //  this.http.ShowError("No Trade Detail found against given specification for Trade");
    //}

    return reltradeDetailID[0];
  }


  //Inline table rows functions

  edit(index: any, row: any) {

    this.inlineForm = this.getInlineForm();
    this.populatedTableList.forEach(x => {
      if (x.IsEditable = true) {
        x.IsEditable = false;
      }
    }
    );

    
    this.populatedTableList[index].IsEditable = true;
  }
  save(index: any, row: any, form: any) {
    this.hideControl(index);
    this.updateRow(index, row);
    //this.educationDDL = [];
    //form.reset();
  }
  saveInlineForm(index: any, row: any) {
    this.updateInlineFrom(index, row);
    //row.reset();
  }
  updateInlineFrom(index, row) {
    this.inlineForm.controls.SectorName.setValue(row.SectorName);
    this.inlineForm.controls.TSPName.setValue(row.TSPName);
    //this.inlineForm.controls.Duration.setValue(row.Duration);
    this.inlineForm.controls.DurationID.setValue(row.DurationID);
    this.inlineForm.controls.GenderName.setValue(row.GenderName);
    this.inlineForm.controls.TradeName.setValue(row.TradeName);
    this.inlineForm.controls.DistrictName.setValue(row.DistrictName);
    this.inlineForm.controls.TehsilName.setValue(row.TehsilName);
    this.inlineForm.controls.ClusterName.setValue(row.ClusterName);
    this.inlineForm.controls.ProvinceName.setValue(row.ProvinceName);
    this.inlineForm.controls.CertAuthName.setValue(row.CertAuthName);
    this.inlineForm.controls.RegistrationAuthorityName.setValue(row.RegistrationAuthorityName);
    this.inlineForm.controls.ProgramFocusName.setValue(row.ProgramFocusName);
    this.inlineForm.controls.Education.setValue(row.Education);
    this.inlineForm.controls.EducationTypeID.setValue(row.EntryQualification);
    this.inlineForm.controls.RequiredLocationGeoTag.setValue(row.RequiredLocationGeoTag);
    this.inlineForm.controls.TradeDetailMapID.setValue(row.TradeDetailMapID);
    //this.inlineForm.controls.TradeDetailMapID.setValue(this.populatedTableList[index].TradeDetailMapID)
    this.inlineForm.controls.TradeDetailMapID.setValue(this.checkAssignTradeDetailMapID(this.inlineForm.value));
    row.NotValid = this.highlightInvalidPopulatedList(this.inlineForm.value).NotValid;
    if (!this.inlineForm.controls.TradeDetailMapID.value) {
      row.TradeDetailMapID = undefined;
    }


    if (row.NotValid) {
      this.http.ShowError("Inserted row is still invalid");
      return;
    }

    this.populatedTableList[index] = this.inlineForm.value;
    this.hideControl(index);
    //this.populatedTableList[index] = row;
    //let newDataList = this.traineeList.map(item => {
    //  let foundItem = data.find(x => x.TraineeID == item.TraineeID);
    //  if (foundItem) {
    //    return {
    //      ...item
    //      , ResultStatusID: foundItem.ResultStatusID
    //      , ResultStatusChangeReason: foundItem.ResultStatusChangeReason
    //      , ResultDocument: foundItem.ResultDocument
    //    }
    //  } else {
    //    return item;
    //  }
    //});
   
  }
  updateRow(index, row) {
    //let rowData = this.traineeList[index];
  }
  showControl(index) {
    //  this.inlineRowIndex = index + 1;
    this.populatedTableList[index].IsEditable = true;
  }
  hideControl(index) {
    this.populatedTableList[index].IsEditable = false;
    //  this.inlineRowIndex = 0
  }


  highlightInvalidPopulatedList(row: any) {

    var regEx = new RegExp(this.geoTagPattern);
    //row['InvalidGeoTagging'] = row.GeoTagging.length > 0 && !regEx.test(row.GeoTagging)
    if (!regEx.test(row.GeoTagging) && row.RequiredLocationGeoTag) {
      row['InvalidGeoTagging'] = true;
    }
    //this.populatedTableList.forEach(x => {
    if (
      //!this.tableList['TSPID'] ||
      !row['SectorID'] ||
      !row['TradeID'] ||
      row['Duration'] == "" ||
      row['SourceOfCurriculum'] == "" ||
      !row['EntryQualification'] ||
      !row['CertAuthID'] || !row['RegistrationAuthorityID'] || !row['ProgramFocusID'] ||
      //row!x['TotalTrainees'] ||
      !row.TraineesPerClass ||
      !row['Batch'] ||
      !row['MinHoursPerMonth'] ||
      !row['StartDate'] ||
      !row['EndDate'] ||
      !row['GenderID'] ||
      (row.RequiredLocationGeoTag && !row['TrainingAddressLocation']) ||
      (row.RequiredLocationGeoTag && !row['GeoTagging']) ||
      !row['DistrictID'] ||
      !row['TehsilID'] ||
      !row['ClusterID'] ||
      !row['ProvinceID'] ||
      (!row['TotalPerTraineeCostInTax'] && row['TotalPerTraineeCostInTax'] != 0) ||
      (!row['SalesTaxRate'] && row['SalesTaxRate'] != 0) ||
      (row['SalesTaxRate'] >= 1) ||
      (!row['SalesTax'] && row['SalesTax'] != 0) ||
      (!row['TrainingCostPerTraineePerMonthInTax'] && row['TrainingCostPerTraineePerMonthInTax'] != 0) ||
      (!row['UniformBagCost'] && row['UniformBagCost'] != 0) ||
      //(isNaN(row.UniformBagCost)) ||
      (isNaN(row.Stipend)) ||
      (!row['balloonpayment'] && row['balloonpayment'] != 0) ||
      (!row['GuruPayment'] && row['GuruPayment'] != 0) ||
      (!row['Transportation'] && row['Transportation'] != 0) ||
      (!row['PerTraineeTestCertCost'] && row['PerTraineeTestCertCost'] != 0) ||
      (!row['EmploymentCommitmentSelf'] && row['EmploymentCommitmentSelf'] != 0) ||
      (!row['EmploymentCommitmentFormal'] && row['EmploymentCommitmentFormal'] != 0) ||
      (!row['OverallEmploymentCommitment'] && row['OverallEmploymentCommitment'] != 0) ||
      (!row['TotalCostPerClass'] && row['TotalCostPerClass'] != 0) ||
      (!row['ProtectorateandVisa'] && row['ProtectorateandVisa'] != 0) || //Added by Rao Ali Haider for International Plac
      (!row['MedicalCost'] && row['MedicalCost'] != 0) ||
      (!row['PrometricCost'] && row['PrometricCost'] != 0) ||
      (!row['OtherTrainingCost'] && row['OtherTrainingCost'] != 0) ||
      (row.RequiredLocationGeoTag && !regEx.test(row.GeoTagging)) ||
      !row['TradeDetailMapID']
    ) {
      row['NotValid'] = true;
    }
    else {
      row['NotValid'] = false;
    }

    return row;

  }

  checkClassGridValidity() {
    let notValidRowsCount = 0;
    let unsavedRowsCount = 0;
    debugger;
    this.populatedTableList.forEach(x => {
      if (x.NotValid) {
        notValidRowsCount++;
      }
      if (x.IsEditable) {
        unsavedRowsCount++
      }

    });
    if (unsavedRowsCount > 0) {
      this.http.ShowError("Please save all individual rows before submitting class");
      return false;
    }
    if (notValidRowsCount > 0) {
      this.http.ShowError("Some inserted rows are still invalid");
      return false;
    }

    else {
      return true;
    }
  }

  submitPopulatedClasses() {

    if (!this.checkClassGridValidity()) {
      return;
    }
    //this.working = true;
    if (this.scheme.length == 0) {
      this.http.ShowError("Save scheme first.");
      return;
    } else if (this.tsps.length == 0) {
      this.http.ShowError("Save Tsps first.");
      return;
    } else {
      let classes = this.populatedTableList;
      classes = classes.map((x: any) => {
        let geoTagging = x.GeoTagging?.split(',') ?? [];
        return {
          ...x
          , SchemeID: this.scheme[0].SchemeID
          , OrganizationID: this.scheme[0].OrganizationID
          , MinAge: this.scheme[0].MinAge
          , MaxAge: this.scheme[0].MaxAge
          , Latitude: geoTagging.length > 0 ? geoTagging[0].toString().trim() : ''
          , Longitude: geoTagging.length > 1 ? geoTagging[1].toString().trim() : ''
        }
      });
      this.http.postJSON('api/Class/Save', JSON.stringify(classes))
        .subscribe((d: any) => {
          this.insertedClass = d;
          this.classData.emit(this.insertedClass);
          this.next.emit(true);
          //this.reset(nform);

          this.emptyClass();
          //this.fillForm(this.insertedClass);
          this.http.openSnackBar(environment.SaveMSG.replace("${Name}", this.EnText), this.EnText);
          this.stepper.next();

        },
          (error) => {
            this.error = error.error;
            this.http.ShowError(error.error);

          });

    }

  }

  getInlineForm() {
    return this._formBuilder.group({
      TSPID: ['', Validators.required],
      SchemeID: [''],
      OrganizationID: [''],
      ClassID: [0],
      TSPName: [''],
      MinAge: [''],
      MaxAge: [''],
      SectorID: ['', Validators.required],
      SectorName: ['', Validators.required],
      TradeID: ['', Validators.required],
      TradeName: ['', Validators.required],
      DurationID: ['', Validators.required],
      Duration: ['', Validators.required],
      SourceOfCurriculum: ['', Validators.required],
      EntryQualification: ['', Validators.required],
      EducationTypeID: ['', Validators.required],
      Education: ['', Validators.required],
      CertAuthID: ['', Validators.required],
      CertAuthName: ['', Validators.required],
      ProgramFocusName: ['', Validators.required],
      ProgramFocusID: ['', Validators.required],
      RegistrationAuthorityName: ['', Validators.required],
      RegistrationAuthorityID: ['', Validators.required],
      //AttendanceStandardPercentage: ['', Validators.required],
      TotalTrainees: [0, Validators.required],
      TraineesPerClass: [0, Validators.required],
      Batch: [0, Validators.required],
      //TotalClasses: [0, Validators.required],//extra field in main form
      //ClassCode: ['', Validators.required],//extra field in grid
      MinHoursPerMonth: [0, Validators.required],
      StartDate: ['', Validators.required],
      EndDate: ['', Validators.required],
      GenderID: ['', Validators.required],
      GenderName: ['', Validators.required],
      TrainingAddressLocation: ['', Validators.required],
      GeoTagging: ['', [Validators.required, Validators.pattern(this.geoTagPattern)]],
      DistrictID: ['', Validators.required],
      DistrictName: ['', Validators.required],
      TehsilID: ['', Validators.required],
      TehsilName: ['', Validators.required],
      ClusterID: ['', Validators.required],
      ClusterName: ['', Validators.required],
      ProvinceID: ['', Validators.required],
      ProvinceName: ['', Validators.required],
      BidPrice: 0,
      BMPrice: 0,
      TotalPerTraineeCostInTax: [0, Validators.required],
      SalesTaxRate: [0, [Validators.required, Validators.max(0.99)]],
      TrainingCostPerTraineePerMonthExTax: [0, Validators.required],
      SalesTax: [0, Validators.required],
      TrainingCostPerTraineePerMonthInTax: [0, Validators.required],
      UniformBagCost: [0, Validators.required],
      PerTraineeTestCertCost: [0, Validators.required],
      BoardingAllowancePerTrainee: [0],
      EmploymentCommitmentSelf: [0, Validators.required],
      EmploymentCommitmentFormal: [0, Validators.required],
      OverallEmploymentCommitment: [0, Validators.required],
      Stipend: [0, Validators.required],
      //StipendMode: ['Digital', Validators.required],
      TotalCostPerClass: [0, Validators.required],
      TradeDetailMapID: ['', Validators.required],
      RequiredLocationGeoTag: ['', Validators.required],
      balloonpayment: 0,
      GuruPayment: 0,
      Transportation: 0,      
      ProtectorateandVisa: 0,
      MedicalCost:0,
      PrometricCost: 0,
      OtherTrainingCost: 0
    },
      { updateOn: "change" }

    );

  }

  get ClassID() { return this.classForm.get("ClassID"); }
  get Class() { return this.classForm.get("Class") as FormArray; }
}
export class ClassModel extends ModelBase {
  ClassID: number;
  ClassName: string;
  ClassCode: string;
  SchemeID: number;
  TSPID: number;
  SectorID: number;
  TradeID: number;
  GenderID: number;
  DistrictID: number;
  TehsilID: number;
  ClusterID: number;
  ProvinceID: number;
  Duration: number;
  Latitude: string;
  Longitude: string;
  TraineesPerClass: number;
  Batch: number;
  TrainingAddressLocation: string;
  //AttendanceStandardPercentage: string;
  MinHoursPerMonth: number;
  StartDate: Date;
  EndDate: Date;
  SourceOfCurriculum: string;
  EntryQualification: string;
  CertAuthID: number;
  RegistrationAuthorityID: number;
  ProgramFocusID: number;
  EmploymentCommitmentSelf: number;
  EmploymentCommitmentFormal: number;
  OverallEmploymentCommitment: number;
  TrainingCostPerTraineePerMonthExTax: number;
  SalesTax: number;
  TrainingCostPerTraineePerMonthInTax: number;
  UniformBagCost: number;
  TotalPerTraineeCostInTax: number;
  PerTraineeTestCertCost: number;
  TotalCostPerClass: number;
  Stipend: number;
  //StipendMode: string;
  BoardingAllowancePerTrainee: number;
  BidPrice: number;
  BMPrice: number;
  CreatedUserID: number;
  IsDual: boolean;
  OrganizationID: number;
  MinAge: number;
  MaxAge: number;
  balloonpayment: number;
  GuruPayment: number;
  Transportation: number;
  ProtectorateandVisa: number;
  MedicalCost: number;
  PrometricCost: number;
  OtherTrainingCost: number;
}



