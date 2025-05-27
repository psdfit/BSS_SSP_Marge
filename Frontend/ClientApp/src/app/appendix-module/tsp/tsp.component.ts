import { Component, OnInit, ViewChild, Input, Output, EventEmitter, ElementRef } from '@angular/core';
import { environment } from '../../../environments/environment';
import { FormGroup, FormBuilder, Validators, FormGroupDirective, FormArray } from '@angular/forms';
import { CommonSrvService } from '../../common-srv.service';
import { ModelBase } from '../../shared/ModelBase';
import { SchemeModel } from '../appendix/appendix.component';
import { Observable } from 'rxjs';
import { ENTER, COMMA, X } from '@angular/cdk/keycodes';
import { EnumAppendixModules } from '../../shared/Enumerations';
import { ClassComponent } from '../class/class.component';
import { IMaskPipe } from 'angular-imask';
import * as IMask from 'imask';
import { InstructorComponent } from '../Instructor/Instructor.component';
import { delay } from 'rxjs/operators';
import { UserRightsModel } from 'src/app/master-data/users/users.component';

@Component({
  selector: 'app-tsp',
  templateUrl: './tsp.component.html',
  styleUrls: ['./tsp.component.scss']
})
export class TSPComponent implements OnInit {
  environment = environment;
  get HeadEmail() { return this.notForm.get("HeadEmail"); }
  get CPEmail() { return this.notForm.get("CPEmail"); }
  tableList: any[] = [];
  populatedTableList: any[] = [];
  inlineForm: FormGroup = this.getTSPInlineForm();

  tspsForm: FormGroup;
  notForm: FormGroup; // this form is not for submitting. Just adds entries to the TSPsForm.
  enText: string = "TSP";
  error: String;
  inst: FormGroup;
  districts: any;
  oldTSPs: any;
  selectable = true;
  removable = true;
  separatorKeysCodes: number[] = [ENTER, COMMA];
  isObject = false;
  manualTSP: any;
  readOnly = false;
  formrights: UserRightsModel;
  @ViewChild('InstrInput') tspInput: ElementRef<HTMLInputElement>;
  insertedTSPs: any[] = [];
  @Input() scheme: SchemeModel[] = [];
  @Input() incompTSP: TSPModel[];
  @Input() childClassComponent: ClassComponent
  @Input() childInstructorComponent: InstructorComponent
  @Output() tsp: EventEmitter<any> = new EventEmitter<any>();
  @Output() next: EventEmitter<any> = new EventEmitter<any>();
  schemeColumns = ['Scheme Name', 'Scheme Code', 'Description', 'Organization', 'PaymentSchedule'];
  constructor(private _formBuilder: FormBuilder, private http: CommonSrvService, private iMask: IMaskPipe) {
    this.formrights = http.getFormRights();
  }
  maskNTN = {
    mask: '0000000-0'
    //,lazy: false
  }
  maskPNTN = {
    mask: '0000000-0'
    //,lazy: false
  }
  maskGST = {
    mask: '0000000-0'
    //,lazy: false
  }
  maskFTN = {
    mask: '000000-0'
    //,lazy: false
  }
  ngOnInit() {
    this.http.OID.subscribe(OID => {
      this.tspsForm = this._formBuilder.group({
        Tsps: new FormArray([])
      });
      this.notForm = this.getNewRow();
      // this.notForm.controls.NTN.enable({ onlySelf: true });
      // this.notForm.controls.NTN.updateValueAndValidity();
      this.getData();
      this.tableList = [];
      this.populatedTableList = [];
    })
  }
  fillTSPs() {
    if (this.incompTSP.length > 0) {
      this.fillForm(this.incompTSP);
    }
  }
  fillForm(TSPS: any) {
    this.Tsps.clear();
    for (let i of TSPS) {
      let form = this.getNewRow();
      form.patchValue(i);
      //***Manual trigger OnChange Event on NTN***////
      this.checkDupplicateTSP(form);
      //Add in TSP array
      this.Tsps.push(form);
      form.markAllAsTouched();
    }
  }
  getData() {
    this.http.getJSON('api/TSPMaster/GetTSPMaster').subscribe((d: any) => {
      this.oldTSPs = d[0];
      this.districts = d[1];
    }, error => this.error = error
    );
  }
  getAlreadySavedTSPMaster(): Observable<any> {
    return this.http.getJSON('api/TSPMaster/RD_TSPMaster');
  }
  checkDupplicateTSP(formGroup: FormGroup) {
    //debugger;
    let ntnControl = formGroup.get('NTN');
    let tspnameControl = formGroup.get('TSPName');
    let isNewControl = formGroup.get('IsNew');

    if (ntnControl.value && ntnControl.value != '' && tspnameControl.value && tspnameControl.value != '') {
      //debugger;
      let duplicateInSameScheme = this.Tsps.controls.find(x => (x.value.NTN == ntnControl.value && x.value.TSPName == tspnameControl.value));
      if (duplicateInSameScheme) {
        /*debugger*/;
        ntnControl.setErrors({ duplicateInSameScheme: true });
        tspnameControl.setErrors({ duplicate: true });
      } else {
        ntnControl.setErrors(null);
        tspnameControl.setErrors(null);
        ntnControl.setValidators([Validators.required]);
        tspnameControl.setValidators([Validators.required]);
        this.checkTspIsNew(ntnControl.value, tspnameControl.value).subscribe(isNew => isNewControl.setValue(isNew));
      }
    }
    formGroup.updateValueAndValidity();
    return formGroup;
  }
  checkTspIsNew(ntn, tspname): Observable<any> {
    if (!ntn && ntn == '' && !tspname && tspname == '') return;
    return this.http.postJSON('api/TSPMaster/CheckTspIsNew', { NTN: ntn, TSPName: tspname });
  }
  submitTSP(nform: FormGroupDirective) {
    debugger;
    if (!this.checkTspGridValidity()) {
      return;
    }
    if (!this.tspsForm.valid)
      return;
    //let tsps = this.tspsForm.getRawValue().Tsps
    let tsps = this.populatedTableList;
    debugger
    let _errorRowNo=0;
    
    const error = tsps.find((tsp, index) => {
      _errorRowNo = index;
      return (
        tsp.HeadDesignation.toLowerCase() === tsp.CPDesignation.toLowerCase() ||
        tsp.HeadName.toLowerCase() === tsp.CPName.toLowerCase() ||
        tsp.HeadLandline === tsp.CPLandline ||
        tsp.HeadEmail.toLowerCase() === tsp.CPEmail.toLowerCase()
      );
    });
    
  
  if (error) {
    const headDesignationError =
      error.HeadDesignation.toLowerCase() === error.CPDesignation.toLowerCase();
    const headNameError =
      error.HeadName.toLowerCase() === error.CPName.toLowerCase();
    const headLandlineError =
      error.HeadLandline === error.CPLandline;
    const headEmailError =
      error.HeadEmail.toLowerCase() === error.CPEmail.toLowerCase();

    if (headDesignationError) {
      this.http.ShowError("Head and POC Designation are the same at row " + (_errorRowNo + 1) + ".");
      return;
    }

    if (headNameError) {
      this.http.ShowError("Head and POC names are the same at row " + (_errorRowNo + 1) + ".");
      return;
    }

    if (headLandlineError) {
      this.http.ShowError("Head and POC landlines are the same at row " + (_errorRowNo + 1) + ".");
      return;
    }

    if (headEmailError) {
      this.http.ShowError("Head and POC emails are the same at row " + (_errorRowNo + 1) + ".");
      return;
    }
  } 
  
    if (this.scheme.length == 0) {
      this.http.ShowError("Save scheme firstly.");
      return;
    } else {
      tsps = tsps.map((x: any) => {
        return {
          ...x
          , SchemeID: this.scheme[0].SchemeID
          , OrganizationID: this.scheme[0].OrganizationID
        }
      })
    }
    //console.log(tsps);
    this.http.postJSON('api/TSPDetail/Save', JSON.stringify(tsps)).subscribe(
      (d: TSPModel[]) => {
        if (d.length > 0) {

          this.insertedTSPs = d;

          this.tsp.emit(this.insertedTSPs);
          this.next.emit(true);

          this.emptyTSPs();
          this.fillForm(this.insertedTSPs);
          this.childClassComponent.onSaveTspData_OnClassGrid(this.insertedTSPs);
          this.childInstructorComponent.applyOnSaveTspData_OnInstructorGrid(this.insertedTSPs);
        }
      },
      error => this.error = error // error path
      , () => {
        this.http.openSnackBar(environment.SaveMSG.replace("${Name}", this.enText));
        this.remove();
      });
  }
  reset(nform: FormGroupDirective) {
    nform.resetForm();
  }

  checkOnHeadEmail() {
    let values = this.notForm.getRawValue();
    this.http.fetchAndValidateTLD(values.HeadEmail)
      .subscribe(
        (isValidTLD: boolean) => {
          if (!isValidTLD) {
            this.HeadEmail.setErrors({ isValid: false, message: 'Invalid email Address' });
            return;
          }
        }, (error) => {
          this.error = error // error path
        }
      );

  }

  checkOnCPEmail() {
    let values = this.notForm.getRawValue();
    this.http.fetchAndValidateTLD(values.CPEmail)
      .subscribe(
        (isValidTLD: boolean) => {
          if (!isValidTLD) {
            this.CPEmail.setErrors({ isValid: false, message: 'Invalid email Address' });
            return;
          }
        }, (error) => {
          this.error = error // error path
        }
      );

  }

  getNewRow(): FormGroup {
    //debugger;
    let form = this._formBuilder.group({
      IsNew: 0,
      TSPID: 0,
      TSPMasterID: 0,
      SchemeID: '',
      OrganizationID: '',
      TSPName: ['', Validators.required],
      TSPCode: ['0'],
      TSPColor: '',
      Address: ['', Validators.required],
      TierID: 0,
      //NTN: [{ value: '', disabled: true }, [Validators.required, Validators.minLength(9), Validators.maxLength(9)]],
      NTN: ['', [Validators.required]],
      PNTN: "",
      GST: "",
      FTN: "",
      DistrictID: ['', Validators.required],
      HeadName: ['', Validators.required],
      HeadDesignation: ['', Validators.required],
      HeadEmail: ['', [Validators.required, Validators.email]],
      HeadLandline: ['', Validators.required],
      OrgLandline: ['', Validators.required],
      Website: "",
      CPName: ['', Validators.required],
      CPDesignation: ['', Validators.required],
      CPLandline: ['', { validators: [Validators.required, Validators.maxLength(12)], updateOn: "blur" }],
      CPEmail: ['', [Validators.required, Validators.email]],
      CPAdmissionsName: [''],
      CPAdmissionsDesignation: [''],
      CPAdmissionsLandline: [''],
      CPAdmissionsEmail: ['', [Validators.email]],
      CPAccountsName: [''],
      CPAccountsDesignation: [''],
      CPAccountsLandline: [''],
      CPAccountsEmail: ['', [Validators.email]],
      BankName: ['', Validators.required],
      BankAccountNumber: ['', Validators.required],
      AccountTitle: ['', Validators.required],
      BankBranch: ['', Validators.required]
    }, { updateOn: "blur" });

    let tspNameControl = form.get('TSPName');
    let ntnControl = form.get('NTN');

    tspNameControl.valueChanges.subscribe(
      value => {
        if (value) {
          //debugger;
          this.checkDupplicateTSP(form);
        }
      }
    );
    ntnControl.valueChanges.subscribe(
      value => {
        if (value && value != '') {
          this.checkDupplicateTSP(form);
        }
      }
    );
    // tspNameControl.valueChanges.subscribe(
    //   value => {
    //     if (value) {
    //       debugger;
    //       let duplicate = this.Tsps.controls.find(x => x.value.TSPName.trim().toLowerCase() == value.trim().toLowerCase());
    //       if (duplicate) {
    //         debugger;
    //         tspNameControl.setErrors({ duplicate: true });
    //       }
    //     }
    //     form.updateValueAndValidity();
    //   }
    // );
    // ntnControl.valueChanges.subscribe(
    //   value => {
    //     if (value && value != '') {
    //      // this.checkDupplicateTSP(form);
    //      let duplicateNTN = this.Tsps.controls.find(x => x.value.NTN.trim().toLowerCase() == value.trim().toLowerCase());
    //       if (duplicateNTN) {
    //         debugger;
    //         ntnControl.setErrors({ duplicateInSameScheme: true });
    //       }
    //     }
    //   }
    // );
    return form;
  }

  get HeadEmail() { return this.notForm.get("HeadEmail"); }
  get CPEmail() { return this.notForm.get("CPEmail"); }

  checkOnHeadEmail() {
    let values = this.notForm.getRawValue();
    this.http.fetchAndValidateTLD(values.HeadEmail)
    .subscribe(
      (isValidTLD: boolean) => {
        if (!isValidTLD) {
          this.HeadEmail.setErrors({ isValid: false, message: 'Invalid email Address' });
          return;
        }
      }, (error) => {
        this.error = error // error path
      }
    );

    }

    checkOnCPEmail() {
      let values = this.notForm.getRawValue();
      this.http.fetchAndValidateTLD(values.CPEmail)
      .subscribe(
        (isValidTLD: boolean) => {
          if (!isValidTLD) {
            this.CPEmail.setErrors({ isValid: false, message: 'Invalid email Address' });
            return;
          }
        }, (error) => {
          this.error = error // error path
        }
      );
  
      }


  removeTsp(ind, r: any) {
    this.Tsps.removeAt(ind);
    //let id = r.controls.TSPID.value
    let id = r.TSPID;
    //console.log(r.controls.TSPID.value);
    this.populatedTableList.splice(ind, 1);
    if (id != 0) {
      this.http.getJSON(`api/Appendix/RemoveFromAppendix?formID=${id}&form=${EnumAppendixModules.TSP}`)
        .subscribe((d: any) => {
        }, error => this.error = error // error path
          , () => {
            //this.working = false;
          });
    }
  }
  entry() {
    debugger;
    if (!this.notForm.valid)
      return;
    let form = this.getNewRow();
    let TSPNameText: any;
    let tspMasterID: any;
    if (this.isObject == false) {
      TSPNameText = this.manualTSP;
      tspMasterID = 0;
    }
    // else {
    //   for (let s of this.oldTSPs) {
    //     if (this.manualTSP.TSPMasterID == s.TSPMasterID) { // TSPName here is actually TSPMasterID
    //       TSPNameText = s.TSPName;
    //       tspMasterID = this.manualTSP.TSPMasterID

    //       break;
    //     }
    //   }
    // }

    //this.getTSPSequence(1).subscribe(d => {

    this.tableList['SchemeID'] ='';
     this.tableList['OrganizationID'] ='';
     this.tableList['TSPMasterID'] =tspMasterID;
     this.tableList['TSPName'] =this.notForm.controls.TSPName.value;
     this.tableList['TSPCode'] =0;//this.getTSPCode(d[0]);
     this.tableList['TSPColor'] =this.notForm.controls.TSPColor.value;
     this.tableList['Address'] =this.notForm.controls.Address.value;
     this.tableList['TierID'] =this.notForm.controls.TierID.value;
     this.tableList['NTN'] =this.notForm.controls.NTN.value;
     this.tableList['PNTN'] =this.notForm.controls.PNTN.value;
     this.tableList['GST'] =this.notForm.controls.GST.value;
     this.tableList['FTN'] =this.notForm.controls.FTN.value;
    this.tableList['DistrictID'] = this.notForm.controls.DistrictID.value;
    this.tableList['DistrictName'] = this.districts.find(x => this.notForm.controls.DistrictID.value == x.DistrictID)?.DistrictName || '';
     this.tableList['HeadName'] =this.notForm.controls.HeadName.value;
     this.tableList['HeadDesignation'] =this.notForm.controls.HeadDesignation.value;
     this.tableList['HeadEmail'] =this.notForm.controls.HeadEmail.value;
     this.tableList['HeadLandline'] =this.notForm.controls.HeadLandline.value;
     this.tableList['OrgLandline'] =this.notForm.controls.OrgLandline.value;
     this.tableList['Website'] =this.notForm.controls.Website.value;
     this.tableList['CPName'] =this.notForm.controls.CPName.value;
     this.tableList['CPDesignation'] =this.notForm.controls.CPDesignation.value;
     this.tableList['CPLandline'] =this.notForm.controls.CPLandline.value;
     this.tableList['CPEmail'] =this.notForm.controls.CPEmail.value;
     this.tableList['CPAdmissionsName'] =this.notForm.controls.CPAdmissionsName.value;
     this.tableList['CPAdmissionsDesignation'] =this.notForm.controls.CPAdmissionsDesignation.value;
     this.tableList['CPAdmissionsLandline'] =this.notForm.controls.CPAdmissionsLandline.value;
     this.tableList['CPAdmissionsEmail'] =this.notForm.controls.CPAdmissionsEmail.value;
     this.tableList['CPAccountsName'] =this.notForm.controls.CPAccountsName.value;
     this.tableList['CPAccountsDesignation'] =this.notForm.controls.CPAccountsDesignation.value;
     this.tableList['CPAccountsLandline'] =this.notForm.controls.CPAccountsLandline.value;
     this.tableList['CPAccountsEmail'] =this.notForm.controls.CPAccountsEmail.value;
     this.tableList['BankName'] =this.notForm.controls.BankName.value;
     this.tableList['BankAccountNumber'] =this.notForm.controls.BankAccountNumber.value;
     this.tableList['AccountTitle'] =this.notForm.controls.AccountTitle.value;
    this.tableList['BankBranch'] = this.notForm.controls.BankBranch.value;

    this.tableList = this.checkDupplicateTSPInlineForm(this.tableList);
    this.populatedTableList.push(this.tableList);
    this.tableList = [];

    form.patchValue({
      SchemeID: '',
      OrganizationID: '',
      TSPMasterID: tspMasterID,
      TSPName: this.notForm.controls.TSPName.value,
      TSPCode: 0,//this.getTSPCode(d[0]),
      TSPColor: this.notForm.controls.TSPColor.value,
      Address: this.notForm.controls.Address.value,
      TierID: this.notForm.controls.TierID.value,
      NTN: this.notForm.controls.NTN.value,
      PNTN: this.notForm.controls.PNTN.value,
      GST: this.notForm.controls.GST.value,
      FTN: this.notForm.controls.FTN.value,
      DistrictID: this.notForm.controls.DistrictID.value,
      HeadName: this.notForm.controls.HeadName.value,
      HeadDesignation: this.notForm.controls.HeadDesignation.value,
      HeadEmail: this.notForm.controls.HeadEmail.value,
      HeadLandline: this.notForm.controls.HeadLandline.value,
      OrgLandline: this.notForm.controls.OrgLandline.value,
      Website: this.notForm.controls.Website.value,
      CPName: this.notForm.controls.CPName.value,
      CPDesignation: this.notForm.controls.CPDesignation.value,
      CPLandline: this.notForm.controls.CPLandline.value,
      CPEmail: this.notForm.controls.CPEmail.value,
      CPAdmissionsName: this.notForm.controls.CPAdmissionsName.value,
      CPAdmissionsDesignation: this.notForm.controls.CPAdmissionsDesignation.value,
      CPAdmissionsLandline: this.notForm.controls.CPAdmissionsLandline.value,
      CPAdmissionsEmail: this.notForm.controls.CPAdmissionsEmail.value,
      CPAccountsName: this.notForm.controls.CPAccountsName.value,
      CPAccountsDesignation: this.notForm.controls.CPAccountsDesignation.value,
      CPAccountsLandline: this.notForm.controls.CPAccountsLandline.value,
      CPAccountsEmail: this.notForm.controls.CPAccountsEmail.value,
      BankName: this.notForm.controls.BankName.value,
      BankAccountNumber: this.notForm.controls.BankAccountNumber.value,
      AccountTitle: this.notForm.controls.AccountTitle.value,
      BankBranch: this.notForm.controls.BankBranch.value,
    });
    //});
    //***Manual trigger OnChange Event on NTN***////
    //local form check
    this.checkDupplicateTSP(form);
    //Add in TSP array
    this.Tsps.push(form);
    form.markAllAsTouched();
  }
  getTSPSequence(noOfSeqs: any) {
    return this.http.getJSON("api/TSPDetail/GetTSPSequence/" + noOfSeqs);
  }
  getTSPCode(tspcode: any): string {
    //return this.Scheme[0].SchemeCode + "-" + tspcode;
    return tspcode;
  }
  populateFieldsFromFile(_tspData: any): boolean {

    if (!_tspData || _tspData.length == 0) {
      return false;
    }
    ///HANDLES DUPLICATION( ignore duplicates)
    // _tspData = _tspData.filter(x => !this.Tsps.controls.find(control => control.value.TSPName.trim().toLowerCase() == x['TSP Name'].trim().toLowerCase()))


    //console.log(seq);
    let errFound = false;
    for (var i = 0; i < _tspData.length; i++) {
      let item = _tspData[i];

      item = this.http.TrimFields(item);

      ///TWO WAYS TO TRANSFORM VALUE USING IMASK
      ///let maskedValue = IMask.pipe(item['NTN'].toString(), new IMask.MaskedPattern(this.maskNTN));
      ///let maskedValue1 = this.iMask.transform(item['NTN'].toString(), new IMask.MaskedPattern(this.maskNTN));

      //let regNTN = new RegExp(`^[0-9]{${this.maskNTN.mask.split('-')[0].length}}-[0-9]{${this.maskNTN.mask.split('-')[1].length}}$`);
      //let regPNTN = new RegExp(`^[0-9]{${this.maskPNTN.mask.split('-')[0].length}}-[0-9]{${this.maskPNTN.mask.split('-')[1].length}}$`);
      //let regGST = new RegExp(`^[0-9]{${this.maskGST.mask.split('-')[0].length}}-[0-9]{${this.maskGST.mask.split('-')[1].length}}$`);
      //let regFTN = new RegExp(`^[0-9]{${this.maskFTN.mask.split('-')[0].length}}-[0-9]{${this.maskFTN.mask.split('-')[1].length}}$`);
      //if (!regNTN.test(item['NTN'])) {
      //  alert(`NTN :${item['NTN']} is not in required format of TSP : ${item['TSP Name']}. Please reload excel.`);
      //  //throw new Error('Exception message');
      //  errFound = true;
      //  break;
      //}
      //if (item['PNTN'] != undefined && !regPNTN.test(item['PNTN'])) {
      //  alert(`PNTN :${item['PNTN']} is not in required format of TSP : ${item['TSP Name']}. Please reload excel.`);
      //  //throw new Error('Exception message');
      //  //return false
      //  errFound = true;
      //  break;
      //}
      //if (item['GST'] != undefined) {
      //  if (!regGST.test(item['GST'])) {
      //    alert(`GST :${item['GST']} is not in required format of TSP : ${item['TSP Name']}. Please reload excel.`);
      //    errFound = true;
      //    break;
      //  }
      //  //throw new Error('Exception message');
      //  //return false

      //}
      //if (item['FTN'] != undefined && !regFTN.test(item['FTN'])) {
      //  alert(`FTN :${item['FTN']} is not in required format of TSP : ${item['TSP Name']}. Please reload excel.`);
      //  //throw new Error('Exception message');
      //  //return false
      //  errFound = true;
      //  break;
      //}

      this.tableList['TSPID'] = 0;
      this.tableList['TSPMasterID'] = 0;
      this.tableList['TSPName'] = item['TSP Name'];
      this.tableList['Address'] = item['Address'];
      //this.tableList['//NTN'] = this.iMask.transform(item['NTN'] ?? '', new IMask.MaskedPattern(this.maskNTN));
      //this.tableList['//PNTN'] = this.iMask.transform(item['PNTN'] ?? '', new IMask.MaskedPattern(this.maskPNTN));
      //this.tableList['//GST'] = this.iMask.transform(item['GST'] ?? '', new IMask.MaskedPattern(this.maskGST));
      //this.tableList['//FTN'] = this.iMask.transform(item['FTN'] ?? '', new IMask.MaskedPattern(this.maskFTN));
      this.tableList['NTN'] = item['NTN'];
      this.tableList['PNTN'] = item['PNTN'];
      this.tableList['GST'] = item['GST'];
      this.tableList['FTN'] = item['FTN'];
      this.tableList['TSPCode'] = 0;
      this.tableList['DistrictID'] = this.districts.find(x => item['District'].toLowerCase() == x.DistrictName.toLowerCase())?.DistrictID || '';
      this.tableList['DistrictName'] = this.districts.find(x => item['District'].toLowerCase() == x.DistrictName.toLowerCase())?.DistrictName || '';
      this.tableList['HeadName'] = item['Head of Organization'];
      this.tableList['HeadDesignation'] = item['Designation of Head of Organization'];
      this.tableList['HeadEmail'] = item['Email of Head of Organization'];
      this.tableList['HeadLandline'] = item['Mobile of Head of Organization'];
      this.tableList['OrgLandline'] = item['Landline Organization'];
      this.tableList['Website'] = item['Website'];
      this.tableList['CPName'] = item['Name of Contact Person'];
      this.tableList['CPDesignation'] = item['Designation of Contact Person'];
      this.tableList['CPLandline'] = item['Mobile/ Landline of Contact Person'];
      this.tableList['CPEmail'] = item['Email of Contact Person'];
      this.tableList['BankName'] = item['Bank Name'];
      this.tableList['BankAccountNumber'] = item['Bank Account / IBAN'];
      this.tableList['AccountTitle'] = item['Account Title'];
      this.tableList['BankBranch'] = item['Bank Branch'];
      this.tableList['DuplicateNTN'] = false;
      this.tableList['DuplicateTSP'] = false;
      this.tableList['nullNTN'] = false;
      this.tableList['nullTSP'] = false;


      this.tableList = this.checkDupplicateTSPInlineForm(this.tableList);
      this.highlightInvalidPopulatedList(this.tableList);
      this.populatedTableList.push(this.tableList);
      this.tableList = [];

      let row = this.getNewRow();
      row.patchValue({
        TSPID: 0,
        TSPMasterID: 0,
        TSPName: item['TSP Name'],
        Address: item['Address'],
        //NTN: this.iMask.transform(item['NTN'] ?? '', new IMask.MaskedPattern(this.maskNTN)),
        //PNTN: this.iMask.transform(item['PNTN'] ?? '', new IMask.MaskedPattern(this.maskPNTN)),
        //GST: this.iMask.transform(item['GST'] ?? '', new IMask.MaskedPattern(this.maskGST)),
        //FTN: this.iMask.transform(item['FTN'] ?? '', new IMask.MaskedPattern(this.maskFTN)),
        NTN: item['NTN'],
        PNTN: item['PNTN'],
        GST: item['GST'],
        FTN: item['FTN'],
        TSPCode: 0,
        DistrictID: this.districts.find(x => item['District'].toLowerCase() == x.DistrictName.toLowerCase())?.DistrictID || '',
        HeadName: item['Head of Organization'],
        HeadDesignation: item['Designation of Head of Organization'],
        HeadEmail: item['Email of Head of Organization'],
        HeadLandline: item['Mobile of Head of Organization'],
        OrgLandline: item['Landline Organization'],
        Website: item['Website'],
        CPName: item['Name of Contact Person'],
        CPDesignation: item['Designation of Contact Person'],
        CPLandline: item['Mobile/ Landline of Contact Person'],
        CPEmail: item['Email of Contact Person'],
        //CPAdmissionsName: item['Name of Contact Person Admissions'],
        //CPAdmissionsDesignation: item['Designation of Contact Person Admissions'],
        //CPAdmissionsLandline: item['Mobile/ Landline of Contact Person Admissions'],
        //CPAdmissionsEmail: item['Email of Contact Person Admissions'],
        //CPAccountsName: item['Name of Contact Person Accounts'],
        //CPAccountsDesignation: item['Designation of Contact Person Accounts'],
        //CPAccountsLandline: item['Mobile/ Landline of Contact Person Accounts'],
        //CPAccountsEmail: item['Email of Contact Person Accounts'],
        BankName: item['Bank Name'],
        BankAccountNumber: item['Bank Account / IBAN'],
        AccountTitle: item['Account Title'],
        BankBranch: item['Bank Branch'],
      });
      //***Manual trigger OnChange Event on NTN***////
      //db

      this.checkDupplicateTSP(row);
      //Add in TSP array
      this.Tsps.push(row);
      row.markAllAsTouched();
    }
    if (errFound) {
      return false;
    }
    return true;
  }
  emptyTSPs() {
    this.Tsps.clear();
  }
  add(event: any): void { // runs when typed manually
    //debugger;
    if (event.value == "")
      return;

    this.isObject = false;

    const input = event.input;
    const value = event.value;

    // Add our TSP
    if ((value || '').trim()) {
      this.manualTSP = value.trim();
    }

    // Reset the input value
    if (input) {
      input.value = '';
    }

    this.clearTSPMasterDataFields();

    this.notForm.patchValue({
      TSPName: this.manualTSP,
    });
  }
  clearTSPMasterDataFields() {
    this.notForm.controls.NTN.setValue('');
    this.notForm.controls.PNTN.setValue('');
    this.notForm.controls.FTN.setValue('');
    this.notForm.controls.GST.setValue('');
    this.notForm.controls.Address.setValue('');
    this.readOnly = false;
  }
  remove(): void {
    if (this.isObject)
      this.manualTSP = null
    else
      this.manualTSP = '';

    this.clearTSPMasterDataFields();
  }
  selected(event: any): void { // when seleted from list
    this.isObject = true;

    this.manualTSP = event.option.value;
    this.tspInput.nativeElement.value = '';


    this.notForm.patchValue(this.manualTSP);
    this.notForm.patchValue({
      TSPName: this.manualTSP.TSPName,
    });

    this.readOnly = true;
  }

  getColor(Value) {
    (2)
    switch (Value) {
      case 1://Already
        return 'white';
      case 2://New
        return 'white';
      case 3://NTN Already Exist 
        return '#fcf8e3';
      case 4://TSP Name Already Exist
        return '#fcf8e3';
    }
  }

  edit(index: any, row: any) {
    //this.educationDDL = [...this.educationList]
    //this.populateFields
    //this.populatedTableList.forEach(item => {
    //  item.IsEditable

    //});

    this.inlineForm = this.getTSPInlineForm();

    this.populatedTableList.forEach(x => {
      if (x.IsEditable = true) {
        x.IsEditable = false;
      }
    }
    );
    //this.populatedTableList.find(x => x.IsEditable = true).IsEditable=false;

    this.populatedTableList[index]['IsEditable'] = true;

    //this.inlineForm.controls.TSPID.setValue(row.TSPID);
    //  = this.populatedTableList.map(x =>
    //  x.IsEditable = false
    //)
    //this.showControl(index);
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
    this.inlineForm.controls.DistrictName.setValue(this.districts.find(x => x.DistrictID == this.inlineForm.controls.DistrictID.value)?.DistrictName || '');
    //this.inlineForm.controls.TSPName.setValue(row.TSPName);
    ////this.inlineForm.controls.Duration.setValue(row.Duration);
    //this.inlineForm.controls.DurationID.setValue(row.DurationID);
    //this.inlineForm.controls.GenderName.setValue(row.GenderName);
    //this.inlineForm.controls.TradeName.setValue(row.TradeName);
    //this.inlineForm.controls.DistrictName.setValue(row.DistrictName);
    //this.inlineForm.controls.TehsilName.setValue(row.TehsilName);
    //this.inlineForm.controls.ClusterName.setValue(row.ClusterName);
    //this.inlineForm.controls.CertAuthName.setValue(row.CertAuthName);
    //this.inlineForm.controls.Education.setValue(row.Education);
    //this.inlineForm.controls.EducationTypeID.setValue(row.EntryQualification);
    //this.inlineForm.controls.RequiredLocationGeoTag.setValue(row.RequiredLocationGeoTag);
    //this.inlineForm.controls.TradeDetailMapID.setValue(row.TradeDetailMapID);
    //this.inlineForm.controls.TradeDetailMapID.setValue(this.populatedTableList[index].TradeDetailMapID)
    row.NotValid = this.highlightInvalidPopulatedList(this.inlineForm.value).NotValid;
    //if (!this.inlineForm.controls.TradeDetailMapID.value) {
    //  row.TradeDetailMapID = undefined;
    //}


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
    console.log(row)
    //console.log(rowData)
  }
  updateRow(index, row) {
    //let rowData = this.traineeList[index];
    //console.log(row)
    //console.log(rowData)
  }
  showControl(index) {
    //  this.inlineRowIndex = index + 1;
    this.populatedTableList[index].IsEditable = true;
  }
  hideControl(index) {
    this.populatedTableList[index].IsEditable = false;
    //  this.inlineRowIndex = 0
  }

  checkTspIsNewInlineForm(ntn, tspname): Observable<any> {
    if (!ntn && ntn == '' && !tspname && tspname == '') return;
    return this.http.postJSON('api/TSPMaster/CheckTspIsNew', { NTN: ntn, TSPName: tspname });
  }

  checkDupplicateTSPInlineForm(row:any) {
    //debugger;
    let ntnControl = row.NTN;
    let tspnameControl = row.TSPName;
    let isNewControl = row.IsNew;

    if (ntnControl && ntnControl != '' && tspnameControl && tspnameControl != '') {
      //debugger;
      let duplicateInSameScheme = this.Tsps.controls.find(x => (x.value.NTN == ntnControl && x.value.TSPName == tspnameControl));
      if (duplicateInSameScheme) {
        /*debugger*/;
        //ntnControl.setErrors({ duplicateInSameScheme: true });
        row['DuplicateNTN'] = true;
        //tspnameControl.setErrors({ duplicate: true });
        row['DuplicateTSP'] = true;
      } else {
        //ntnControl.setErrors(null);

        //row['nullNTN'] = true;

        //tspnameControl.setErrors(null);

        //row['nullTSP'] = true;
        //ntnControl.setValidators([Validators.required]);
        //tspnameControl.setValidators([Validators.required]);
        this.checkTspIsNew(ntnControl, tspnameControl).subscribe(isNew => row.IsNew = isNew);
      }
    }
    return row;
  }

  highlightInvalidPopulatedList(row: any) {
    //this.populatedTableList.forEach(x => {
    if (
      //!this.tableList['TSPID'] ||
      row['TSPName'] == "" ||
      row['Address'] == "" ||
      row['HeadName'] == "" ||
      row['HeadDesignation'] == "" ||
      row['HeadEmail'] == "" ||
      row['HeadLandline'] == "" ||
      row['HeadLandline'].length > 20 ||
      row['OrgLandline'] == "" ||
      row['OrgLandline'].length > 20 ||
      row['CPName'] == "" ||
      row['CPDesignation'] == "" ||
      row['CPLandline'] == "" ||
      row['CPLandline'].length > 12 ||
      row['CPEmail'] == "" ||
      //row['CPAdmissionsEmail'] == "" ||
      //row['CPAccountsEmail'] == "" ||
      row['BankName'] == "" ||
      row['BankAccountNumber'] == "" ||
      row['AccountTitle'] == "" ||
      row['BankBranch'] == "" ||
      //row!x['TotalTrainees'] ||
     
      !row['DistrictID']
      
    ) {
      row['NotValid'] = true;
    }
    else {
      row['NotValid'] = false;
    }

    return row;

  }

  checkTspGridValidity() {
    let notValidRowsCount = 0;
    let unsavedRowsCount = 0;
    this.populatedTableList.forEach(x => {
      if (x.NotValid) {
        notValidRowsCount++;
      }
      if (x.IsEditable) {
        unsavedRowsCount++
      }

    });
    if (unsavedRowsCount > 0) {
      this.http.ShowError("Please save all individual rows before submitting inserted TSPs");
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

  getTSPInlineForm() {
    //debugger;
    return this._formBuilder.group({
      IsNew: 0,
      TSPID: 0,
      TSPMasterID: 0,
      SchemeID: '',
      OrganizationID: '',
      TSPName: ['', Validators.required],
      TSPCode: ['0'],
      TSPColor: '',
      Address: ['', Validators.required],
      TierID: 0,
      //NTN: [{ value: '', disabled: true }, [Validators.required, Validators.minLength(9), Validators.maxLength(9)]],
      NTN: ['', [Validators.required]],
      PNTN: "",
      GST: "",
      FTN: "",
      DistrictID: ['', Validators.required],
      DistrictName: ['', Validators.required],
      HeadName: ['', Validators.required],
      HeadDesignation: ['', Validators.required],
      HeadEmail: ['', [Validators.required, Validators.email]],
      HeadLandline: ['', { validators: [Validators.required, Validators.maxLength(20)], updateOn: "blur" }],
      OrgLandline: ['', { validators: [Validators.required, Validators.maxLength(20)], updateOn: "blur" }],
      Website: "",
      CPName: ['', Validators.required],
      CPDesignation: ['', Validators.required],
      CPLandline: ['', { validators: [Validators.required, Validators.maxLength(20)], updateOn: "blur" }],
      CPEmail: ['', [Validators.required, Validators.email]],
      CPAdmissionsName: [''],
      CPAdmissionsDesignation: [''],
      CPAdmissionsLandline: [''],
      CPAdmissionsEmail: ['', [Validators.email]],
      CPAccountsName: [''],
      CPAccountsDesignation: [''],
      CPAccountsLandline: [''],
      CPAccountsEmail: ['', [Validators.email]],
      BankName: ['', Validators.required],
      BankAccountNumber: ['', Validators.required],
      AccountTitle: ['', Validators.required],
      BankBranch: ['', Validators.required]
    }, { updateOn: "blur" });

    //let tspNameControl = form.get('TSPName');
    //let ntnControl = form.get('NTN');

    //tspNameControl.valueChanges.subscribe(
    //  value => {
    //    if (value) {
    //      //debugger;
    //      this.checkDupplicateTSP(form);
    //    }
    //  }
    //);
    //ntnControl.valueChanges.subscribe(
    //  value => {
    //    if (value && value != '') {
    //      this.checkDupplicateTSP(form);
    //    }
    //  }
    //);

  }


  get TSPID() { return this.tspsForm.get("TSPID"); }
  get Tsps() { return this.tspsForm.get("Tsps") as FormArray; }
}
export class TSPModel extends ModelBase {
  SchemeID: number;
  MinAge: number;
  MaxAge: number;
  TSPID: number;
  TSPMasterID: number;
  TSPName: string;
  TSPCode: string;
  OrganizationID: number;
  TSPColor: string;
  Address: string;
  TierID: number;
  NTN: string;
  PNTN: string;
  GST: string;
  FTN: string;
  DistrictID: number;
  HeadName: string;
  HeadDesignation: string;
  HeadEmail: string;
  HeadLandline: string;
  OrgLandline: string;
  Website: string;
  CPName: string;
  CPDesignation: string;
  CPLandline: string;
  CPEmail: string;
  CPAdmissionsName: string;
  CPAdmissionsDesignation: string;
  CPAdmissionsLandline: string;
  CPAdmissionsEmail: string;
  CPAccountsName: string;
  CPAccountsDesignation: string;
  CPAccountsLandline: string;
  CPAccountsEmail: string;
  BankName: string;
  BankAccountNumber: string;
  AccountTitle: string;
  BankBranch: string;
}
