import { COMMA, ENTER } from '@angular/cdk/keycodes';
import { Component, ElementRef, Input, OnInit, ViewChild } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { CommonSrvService } from '../../common-srv.service';
import { ModelBase } from '../../shared/ModelBase';
import { ClassModel } from '../class/class.component';
import { TSPModel } from '../tsp/tsp.component';
import { SchemeModel } from '../appendix/appendix.component';
import { EnumAppendixModules } from '../../shared/Enumerations';
import * as IMask from 'imask';
import { UserRightsModel } from 'src/app/master-data/users/users.component';

@Component({
  selector: 'app-instructor',
  templateUrl: './instructor.component.html',
  styleUrls: ['./instructor.component.scss']
})
export class InstructorComponent implements OnInit {

  tableList: any[] = [];
  populatedTableList: any[] = [];
  inlineForm: FormGroup = this.getInstructorInlineForm();

  env = environment;
  notForm: FormGroup;
  instructorForm: FormGroup;
  instance: FormGroup;
  genders: any;
  trades: any;
  oldInstr: any;
  formrights: UserRightsModel;
  enText: string = "Instructor";
  error: String;
  insertedInstructors: [] = [];
  allowFinalSubmit: boolean = false;
  @Input() scheme: SchemeModel[] = [];
  @Input() tsps: Array<TSPModel> = [];
  @Input() classes: Array<ClassModel> = [];
  @Input() incompInstr: InstructorModel[] = [];
  schemeColumns = ['Scheme Name', 'Scheme Code', 'Description', 'Organization', 'PaymentSchedule'];
  tspColumns = ['TSP Name', 'NTN', 'PNTN', 'GST', 'FTN'];
  classColumns = ['Trade', 'Duration', 'Total Cost', 'Start Date', 'End Date'];
  cnicExists = false;
  selectable = true;
  removable = true;
  separatorKeysCodes: number[] = [ENTER, COMMA];
  filteredInstrs: Observable<string[]>;
  isObject = false;
  cnicReadOnly = false;
  manualInstructor: any;
  @ViewChild('InstrInput') InstrInput: ElementRef<HTMLInputElement>;

  maskCNIC = {
    mask: '00000-0000000-0'
  }

  constructor(private _formBuilder: FormBuilder, private http: CommonSrvService) {
    this.formrights = http.getFormRights();
  }

  ngOnInit() {
    this.http.OID.subscribe(OID => {
      this.allowFinalSubmit = false;
      this.instructorForm = this._formBuilder.group({
        Instructors: new FormArray([])
      });
      this.notForm = this.getNewRow();
      this.getData();
      this.tableList = [];
      this.populatedTableList = [];
    })
  }
  getData() {
    this.http.getJSON('api/Instructor/GetInstructor').subscribe((d: any) => {
      this.genders = d[0];
      this.trades = d[1];
      this.oldInstr = d[2];
    }, error => this.error = error
    );
  }
  fillForm(InstrsList: any) {
    if (InstrsList.length > 0) {
      this.allowFinalSubmit = true;
      for (let i of InstrsList) {
        this.instance = this.getNewRow();
        this.Instructors.push(this.instance);
        this.instance.patchValue(i);
      }
    }
  }
  applyOnSaveTspData_OnInstructorGrid(tspList: TSPModel[]) {
    if (tspList.length > 0) {
      this.Instructors.controls.forEach((rowForm: FormGroup) => {
        let foundItem = tspList.find(x => x.TSPName.trim().toLowerCase() == rowForm.value.NameOfOrganization.trim().toLowerCase());
        if (foundItem) {
          rowForm.controls.TSPID.patchValue(foundItem.TSPID);
        }
      });

      this.populatedTableList.forEach(rowForm => {
        let foundItem = tspList.find(x => x.TSPName.trim().toLowerCase() == rowForm.NameOfOrganization.toLowerCase());
        if (foundItem) {
          //let seq = rowForm.value.ClassCode?.split('-');
          //seq = seq[seq.length - 1];
          rowForm.TSPID = foundItem.TSPID;
          rowForm.TSPName = foundItem.TSPName;

          this.highlightInvalidPopulatedList(rowForm);
          //rowForm.Stipend = this.scheme[0].Stipend;
          //rowForm.UniformBagCost = this.scheme[0].UniformAndBag;
          //rowForm.controls.ClassCode.patchValue(`${this.scheme[0]?.SchemeCode}-${foundItem.TSPCode ?? ''}-${seq}`)
        }
      })
    }
  }
  submitInstructor() {
    debugger;
    if (!this.checkInstructorGridValidity()) {
      return;
    }

    console.log(this.tsps)
    if (this.scheme.length == 0) {
      this.http.ShowError("Save scheme firstly.");
      return;
    } else if (this.tsps.length == 0) {
      this.http.ShowError("Save Tsps firstly.");
      return;
    } else if (this.classes.length == 0) {
      this.http.ShowError("Save Classes firstly.");
      return;
    } else {
      //let list = this.instructorForm.value.Instructors
      let list = this.populatedTableList;
      list = list.map((x: any) => {
        return {
          ...x
          , SchemeID: this.scheme[0].SchemeID
        }
      });
      
      this.http.postJSON('api/Instructor/Save', JSON.stringify(list))
        .subscribe((d: any) => {
          this.allowFinalSubmit = true;
          this.insertedInstructors = d;

          this.reset();
          this.emptyInstructors();
          this.fillForm(this.insertedInstructors);
        }, error => this.error = error // error path
          , () => {
            this.http.openSnackBar(environment.SaveMSG.replace("${Name}", this.enText));
            this.remove();
          });
      //}
    }

  }
  getNewRow(): FormGroup {
    let form = this._formBuilder.group({
      InstrID: 0,
      InstrMasterID: [0],
      SchemeID: '',
      TSPID: ['', Validators.required],
      InstructorName: ['', Validators.required], // this is actually InstrMasterID
      GenderID: ['', Validators.required],
      PicturePath: "",
      TotalExperience: ['', [Validators.required, Validators.max(99)]],
      QualificationHighest: ['', Validators.required],
      CNICofInstructor: ['', [Validators.required, Validators.minLength(15), Validators.maxLength(15)]],
      TradeID: ['', Validators.required],
      LocationAddress: ['', Validators.required],
      ClassCode: [''],//['', Validators.required],
      NameOfOrganization: "",
    });
    let tspIDControl = form.get('TSPID');
    let nameOfOrganizationControl = form.get('NameOfOrganization');
    tspIDControl.valueChanges.subscribe(
      value => {
        if (value) {
          let tsp = this.tsps.find(x => x.TSPID == value);
          if (tsp) {
            nameOfOrganizationControl.patchValue(tsp?.TSPName)
          }
        }
      }
    );
    return form;
  }
  removeInstructor(ind, r: any) {
    this.Instructors.removeAt(ind);

    this.populatedTableList.splice(ind, 1);

    let id = r.controls.InstrID.value
    console.log(r.controls.InstrID.value);

    if (id != 0) {
      this.http.getJSON(`api/Appendix/RemoveFromAppendix?formID=${id}&form=${EnumAppendixModules.Instructor}`)
        .subscribe((d: any) => {
        }, error => this.error = error // error path
          , () => {
            //this.working = false;
          });
    }
  }
  emptyInstructors() {
    this.Instructors.clear();
  }
  entry() {
    if (!this.notForm.valid || this.cnicExists)
      return;
    this.allowFinalSubmit = false;
    this.instance = this.getNewRow();
    this.Instructors.push(this.instance);
    let InstrNameText: string;
    let instrMID: number;
    if (this.isObject) {
      InstrNameText = this.manualInstructor.InstructorName;
      instrMID = this.manualInstructor.InstrMasterID;
    }
    else {
      InstrNameText = this.manualInstructor;
      instrMID = 0;
    }
    this.instance.patchValue({
      InstrID: 0,
      TSPID: this.notForm.controls.TSPID.value,
      //SchemeID: this.classes[0].SchemeID,
      SchemeID: this.scheme[0].SchemeID,
      InstrMasterID: instrMID,
      InstructorName: InstrNameText,
      GenderID: this.notForm.controls.GenderID.value,
      PicturePath: this.notForm.controls.PicturePath.value,
      TotalExperience: this.notForm.controls.TotalExperience.value,
      QualificationHighest: this.notForm.controls.QualificationHighest.value,
      CNICofInstructor: this.notForm.controls.CNICofInstructor.value,
      TradeID: this.notForm.controls.TradeID.value,
      LocationAddress: this.notForm.controls.LocationAddress.value,
      ClassCode: this.notForm.controls.ClassCode.value,
    });
    this.instance.markAllAsTouched();

    this.tableList['InstrID'] = 0;
    this.tableList['TSPID'] = this.notForm.controls.TSPID.value;
    this.tableList['TSPName'] = this.tsps?.find(x => x.TSPID == this.notForm.controls.TSPID.value).TSPName;
    this.tableList['SchemeID'] = this.scheme[0].SchemeID;
    this.tableList['InstrMasterID'] = instrMID;
    this.tableList['InstructorName'] = this.notForm.controls.InstructorName.value;
    this.tableList['GenderID'] = this.notForm.controls.GenderID.value;
    this.tableList['GenderName'] = this.genders?.find(x => x.GenderID == this.notForm.controls.GenderID.value).GenderName;
    this.tableList['PicturePath'] = this.notForm.controls.PicturePath.value;
    this.tableList['TotalExperience'] = this.notForm.controls.TotalExperience.value;
    this.tableList['QualificationHighest'] = this.notForm.controls.QualificationHighest.value;
    this.tableList['CNICofInstructor'] = this.notForm.controls.CNICofInstructor.value;
    this.tableList['TradeID'] = this.notForm.controls.TradeID.value;
    this.tableList['TradeName'] = this.trades?.find(x => x.TradeID == this.notForm.controls.TradeID.value).TradeName;
    this.tableList['LocationAddress'] = this.notForm.controls.LocationAddress.value;
    this.tableList['ClassCode'] = this.notForm.controls.ClassCode.value;

    this.populatedTableList.push(this.tableList);
    this.tableList = [];


  }
  populateFieldsFromFile(_instructorData: any) {
    _instructorData.forEach(
      (item: any, index: string | number) => {
        item = this.http.TrimFields(item);
        this.instance = this.getNewRow();
        this.Instructors.push(this.instance);
        let tsp: TSPModel = this.tsps?.find(x => item["Name of Organization"].toLowerCase() == x.TSPName.toLowerCase());

        this.tableList['TSPID'] = tsp?.TSPID ?? '';
        this.tableList['TSPName'] = tsp?.TSPName ?? '';
        this.tableList['NameOfOrganization'] = tsp?.TSPName ?? item["Name of Organization"];
        this.tableList['InstrMasterID'] = 0;
        this.tableList['InstructorName'] = item['Instructor Name'];
        this.tableList['GenderID'] = this.genders.find(x => item['Gender'].toLowerCase() == x.GenderName.toLowerCase())?.GenderID || '';
        this.tableList['GenderName'] = this.genders.find(x => item['Gender'].toLowerCase() == x.GenderName.toLowerCase())?.GenderName || '';
        //this.tableList['//PicturePath']=f['Profile Picture'];
        this.tableList['TotalExperience'] = item['Total Experience'];
        this.tableList['QualificationHighest'] = item['Qualification Highest'];
        this.tableList['CNICofInstructor'] = IMask.pipe(item['CNIC of Instructor'].toString(), new IMask.MaskedPattern(this.maskCNIC));
        this.tableList['TradeID'] = this.trades.find(x => item['Trade'].toLowerCase() == x.TradeName.toLowerCase())?.TradeID || '';
        this.tableList['TradeName'] = this.trades.find(x => item['Trade'].toLowerCase() == x.TradeName.toLowerCase())?.TradeName || '';
        this.tableList['LocationAddress'] = item['Address of Training Location']

        this.highlightInvalidPopulatedList(this.tableList);

        this.populatedTableList.push(this.tableList);
        this.tableList = [];

        this.instance.patchValue({
          TSPID: tsp?.TSPID ?? '',
          NameOfOrganization: tsp?.TSPName ?? item["Name of Organization"],
          InstrMasterID: 0,
          InstructorName: item['Instructor Name'],
          GenderID: this.genders.find(x => item['Gender'].toLowerCase() == x.GenderName.toLowerCase())?.GenderID || '',
          //PicturePath: f['Profile Picture'],
          TotalExperience: item['Total Experience'],
          QualificationHighest: item['Qualification Highest'],
          CNICofInstructor: IMask.pipe(item['CNIC of Instructor'].toString(), new IMask.MaskedPattern(this.maskCNIC)),
          TradeID: this.trades.find(x => item['Trade'].toLowerCase() == x.TradeName.toLowerCase())?.TradeID || '',
          LocationAddress: item['Address of Training Location'],
        });

        this.instance.markAllAsTouched();
      })
  }
  reset() {
    this.notForm.reset();
  }
  get Instructors() { return this.instructorForm.get("Instructors") as FormArray; }
  get InstrID() { return this.instructorForm.get("InstrID"); }

  add(event: any): void { // runs when typed manually
    if (event.value == "")
      return;
    this.isObject = false;

    const input = event.input;
    const value = event.value;

    // Add our Instructor
    if ((value || '').trim()) {
      this.manualInstructor = value.trim();
    }

    // Reset the input value
    if (input) {
      input.value = '';
    }

    //this.cnicReadOnly = false;
    this.notForm.controls.CNICofInstructor.setValue('');
    this.notForm.patchValue({
      InstructorName: this.manualInstructor,
    });
  }
  remove(): void {
    if (this.isObject)
      this.manualInstructor = null
    else
      this.manualInstructor = '';

    this.cnicReadOnly = false;
  }
  selected(event: any): void {
    this.isObject = true;

    this.manualInstructor = event.option.value;
    this.InstrInput.nativeElement.value = '';

    this.notForm.patchValue(this.manualInstructor);
    this.notForm.patchValue({
      InstructorName: this.manualInstructor.InstructorName,
      PicturePath: "",
      TradeID: ''
    });

    this.cnicReadOnly = true;
  }
  checkInstructorExists(cnic) {
    if (!this.notForm.controls.CNICofInstructor.dirty) {
      return;
    }

    for (let r of this.oldInstr) {
      if (this.notForm.controls.CNICofInstructor.value == r.CNICofInstructor) {
        this.notForm.patchValue(r);
        this.notForm.controls.CNICofInstructor.setValue('');
        this.cnicExists = true;

        break;
      }
      else {
        this.cnicExists = false;
      }
    }
  }

  //Inline table rows functions

  edit(index: any, row: any) {
    //this.educationDDL = [...this.educationList]
    //this.populateFields
    //this.populatedTableList.forEach(item => {
    //  item.IsEditable

    //});

    this.inlineForm = this.getInstructorInlineForm();

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

    row.NotValid = this.highlightInvalidPopulatedList(this.inlineForm.value).NotValid;

    if (row.NotValid) {
      this.http.ShowError("Inserted row is still invalid");
      return;
    }
    //this.inlineForm.controls.PicturePath.setValue(row.PicturePath);
    //this.inlineForm.controls.Duration.setValue(row.Duration);
    this.inlineForm.controls.GenderName.setValue(row.GenderName);
    //this.inlineForm.controls.TradeName.setValue(row.TradeName);
    //this.inlineForm.controls.PicturePath.setValue(row.PicturePath);
    this.inlineForm.controls.TSPName.setValue(this.tsps.find(x => x.TSPID == this.inlineForm.controls.TSPID.value).TSPName);
    this.inlineForm.controls.TradeName.setValue(this.trades.find(x => x.TradeID == this.inlineForm.controls.TradeID.value).TradeName);
    row.TradeName = this.inlineForm.controls.TradeName.value;
    row.TSPName = this.inlineForm.controls.TSPName.value;
    //this.inlineForm.controls.TradeDetailMapID.setValue(this.populatedTableList[index].TradeDetailMapID)
    //if (!this.inlineForm.controls.TradeDetailMapID.value) {
    //  row.TradeDetailMapID = undefined;
    //}



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


  highlightInvalidPopulatedList(row: any) {

    //this.populatedTableList.forEach(x => {
    if (
      //!this.tableList['TSPID'] ||
      !row['TSPID'] ||
      !row['TradeID'] ||
      //row['TSPName'] == "" ||
      //!row['InstructorID'] ||
      //row['InstructorName'] == "" ||
      //row!x['TotalTrainees'] ||
      !row['TotalExperience'] ||
      !row['QualificationHighest'] ||
      !row['CNICofInstructor'] ||
      //!row['TradeName'] ||
      !row['GenderID'] ||
      //!row['GenderName'] ||
      !row['LocationAddress']
    ) {
      row['NotValid'] = true;
    }
    else {
      row['NotValid'] = false;
    }

    return row;

  }

  checkInstructorGridValidity() {
    let notValidRowsCount = 0;
    let unsavedRowsCount = 0;
    this.populatedTableList.forEach(x => {
      if (x.NotValid) {
        notValidRowsCount++;
      }
      if (x.IsEditable) {
        unsavedRowsCount++
      }

    })
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


  getInstructorInlineForm() {
    return this._formBuilder.group({
      InstrID: 0,
      InstrMasterID: [0],
      SchemeID: '',
      TSPID: ['', Validators.required],
      TSPName: ['', Validators.required],
      InstructorName: ['', Validators.required], // this is actually InstrMasterID
      GenderID: ['', Validators.required],
      GenderName: ['', Validators.required],
      PicturePath: "",
      TotalExperience: ['', [Validators.required, Validators.max(99)]],
      QualificationHighest: ['', Validators.required],
      CNICofInstructor: ['', [Validators.required, Validators.minLength(15), Validators.maxLength(15)]],
      TradeID: ['', Validators.required],
      TradeName: ['', Validators.required],
      LocationAddress: ['', Validators.required],
      ClassCode: [''],//['', Validators.required],
      NameOfOrganization: "",
    },
      { updateOn: "change" }

    );
    //let tspIDControl = form.get('TSPID');
    //let nameOfOrganizationControl = form.get('NameOfOrganization');
    //tspIDControl.valueChanges.subscribe(
    //  value => {
    //    if (value) {
    //      let tsp = this.tsps.find(x => x.TSPID == value);
    //      if (tsp) {
    //        nameOfOrganizationControl.patchValue(tsp?.TSPName)
    //      }
    //    }
    //  }
    //);
  }

}
export class InstructorModel extends ModelBase {
  ClassCode: number;
  InstrID: number;
  InstructorName: string;
  CNICofInstructor: string;
  InstrClassID: number;
  QualificationHighest: string;
  TotalExperience: string;
  GenderID: number;
  PicturePath: string;
  NameOfOrganization: string;
  TSPID: string;
  TradeID: number;
  LocationAddress: string;
  InstrMasterID: number;
  SchemeID: number;
  ClassID: number;
}

