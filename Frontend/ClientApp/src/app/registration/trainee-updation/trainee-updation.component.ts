import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { CommonSrvService } from '../../common-srv.service';
import { Element } from '@angular/compiler/src/render3/r3_ast';
import { MatProgressBar } from '@angular/material/progress-bar';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { FormGroup, FormBuilder, Validators, FormGroupDirective, FormArray, AbstractControl } from '@angular/forms';
import { environment } from '../../../environments/environment';
import { ModelBase } from '../../shared/ModelBase';
import { UserRightsModel, UsersModel } from '../../master-data/users/users.component';
import { MatDialog } from '@angular/material/dialog';
import { EnumCertificationAuthority, EnumTraineeResultStatusTypes, EnumUserLevel, ExportType, EnumTraineeStatusType, EnumClassStatus, EnumExcelReportType } from '../../shared/Enumerations';
import { DialogueService } from '../../shared/dialogue.service';
import * as XLSX from 'xlsx';
import { SearchFilter, ExportExcel } from '../../shared/Interfaces';
import { SelectionModel } from '@angular/cdk/collections';
import { async } from '@angular/core/testing';
import { delay } from 'rxjs/operators';

@Component({
  selector: 'app-trainee-updation',
  templateUrl: './trainee-updation.component.html',
  styleUrls: ['./trainee-updation.component.scss']
})
export class TraineeUpdationComponent implements OnInit {
  title: string;
  savebtn: string;
  //displayedColumns = ['Action', 'TraineeCode', 'TraineeName', 'TraineeCNIC', "GenderName", "TraineeStatus", "ResultStatusID", "ResultStatusChangeReason", "ResultDocument"];
  displayedColumns = ['Sr', 'Check', 'Action', 'SchemeName', 'SchemeCode', 'TSPName', 'ClassCode', 'TraineeCode', 'TraineeName', 'FatherName', 'TraineeCNIC', 'CNICIssueDate', 'GenderName', 'DateOfBirth', 'TraineeImg', "StatusName", "CNICImg"];

  schemeArray = [];
  tspDetailArray = [];
  classesArray: any[];
  formrights: UserRightsModel;
  error: String;
  errorHTTP: any;
  filters: SearchFilter = { SchemeID: 0, TSPID: 0, ClassID: 0, TraineeID: 0 };
  //traineeResultStatusTypeArray: any;
  tblData: any[];
  tblDatasource: MatTableDataSource<any[]> = new MatTableDataSource([]);
  tblFormGroup: FormGroup;
  isInternalUser: boolean = false;
  isTSPUser: boolean = false;
  currentUser: UsersModel;
  kamAssignmentTSPs: any[] = [];
  @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: false }) sort: MatSort;
  differSelectionCount: number = 0;

  constructor(private fb: FormBuilder, private commonService: CommonSrvService, public dialog: MatDialog, public dialogueService: DialogueService) {
    this.tblFormGroup = this.fb.group({
      formArray: this.fb.array([])
    });
    this.formrights = commonService.getFormRights();
  }
  ngOnInit() {
    this.commonService.setTitle("Update Trainee's CNIC Image");
    this.title = "Add New ";
    this.savebtn = "Save ";

    this.currentUser = this.commonService.getUserDetails();
    if (this.currentUser.UserLevel == EnumUserLevel.TSP) {
      this.isTSPUser = true;
    } else if (this.currentUser.UserLevel == EnumUserLevel.AdminGroup || this.currentUser.UserLevel == EnumUserLevel.OrganizationGroup) {
      this.isInternalUser = true;
    }


    this.getData();
  }
  ///Mat-Table checkbox Config - Start
  selection = new SelectionModel<any>(true, []);
  /** Whether the number of selected elements matches the total number of rows. */
  isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.tblDatasource.data.length;
    return numSelected === numRows;
  }
  /** Selects all rows if they are not all selected; otherwise clear selection. */
  masterToggle() {
    this.isAllSelected() ?
      this.selection.clear() :
      this.tblDatasource.data.forEach(row => this.selection.select(row));
  }
  /** The label for the checkbox on the passed row */
  checkboxLabel(row?: any): string {
    if (!row) {
      return `${this.isAllSelected() ? 'select' : 'deselect'} all`;
    }
    return `${this.selection.isSelected(row) ? 'deselect' : 'select'} row ${1}`;
  }
  ///Mat-Table checkbox Config - End

  getData() {
    this.commonService.getJSON('api/TraineeProfile/GetDataForTraineeUpdation').subscribe(
      (response: any) => {
        this.schemeArray = response.Schemes;
        this.fillTableForm(response.Data)
      }
      , error => this.error = error // error path
    );
  }
  getFilteredData() {
    let filters = "filters?" + Object.entries(this.filters).map(([key, value]) => `filters=${value}`).join('&');
    this.commonService.getJSON(`api/TraineeProfile/GetFilteredTrainees/${filters}`)
      .subscribe((data: any) => {
        this.fillTableForm(data);
        //debugger;
      },
        error => {
          this.fillTableForm([]);
          this.error = error;
        })
  }

  get formArray() { return this.tblFormGroup.get('formArray') as FormArray }

  fillTableForm(data: any[]) {
    this.formArray.clear();
    data.forEach(x => {
      let form = this.getNewForm(x);
      form.patchValue(x);
      this.formArray.push(form);
    });
    data=data.map((item, index) => { return { ...item, Sr: index + 1 } })
    this.tblDatasource = new MatTableDataSource(data);
    this.tblDatasource.paginator = this.paginator;
    this.tblDatasource.sort = this.sort;
    this.tblData = data;
  }
  getNewForm(data: any): FormGroup {
    let form = this.fb.group({
      TraineeID: [0],
      TraineeCode: [''],
      CNICImg: [''],
      //ControlDisabled:[false]
    }, { updateOn: "change" });

    form.get("CNICImg").valueChanges.pipe(delay(500)).subscribe(
      value => {
        this.getSetDifferSelection()
      })

    return form;
  }

  getTSPDetailByScheme() {
    this.filters.TSPID = 0;
    this.filters.ClassID = 0;
    this.classesArray = [];
    this.commonService.getJSON(`api/TSPDetail/GetTSPDetailByScheme/` + this.filters.SchemeID)
      .subscribe(data => {
        this.tspDetailArray = <any[]>data;
      }, error => {
        this.error = error;
      })
  }
  getClassesByTsp() {
    this.filters.ClassID = 0;
    this.commonService.getJSON(`api/Class/GetClassesByTsp/` + this.filters.TSPID)
      .subscribe(data => {
        this.classesArray = <any[]>data;
      }, error => {
        this.error = error;
      })
  }
  openHistoryDialogue(data: any): void {
    this.dialogueService.openTraineeStatusHistoryDialogue(data.TraineeID);
  }
  checkRowError(row) {
    return this.tblFormGroup.value.formArray.find(x => x.TraineeID == row.TraineeID).CNICImg == "";
  }
  saveAllSelected() {
    let selected = this.getSetDifferSelection();
    if (selected.length > 0) {
      this.commonService.postJSON('api/TraineeProfile/UpdateTraineeCNICImg', JSON.stringify(selected)).subscribe(
        (data: any) => {
          if (data == true) {
            this.commonService.openSnackBar("Saved Successfully");
            this.selection.clear();
            this.getFilteredData();
          }
        }
        , (error) => this.commonService.ShowError(error.error + '\n' + error.message)
      );
    }
    //console.log(selected);
  }

  getSetDifferSelection() {
    let selections = this.tblFormGroup.value.formArray.filter(row => this.selection.selected.map(x => x.TraineeID).includes(row.TraineeID));
    // console.log(selections);
    let differ = selections.filter(x => this.tblData.find(item => item.TraineeID == x.TraineeID).CNICImg != x.CNICImg);
    // console.log(differ);
    this.differSelectionCount = differ.length;
    return differ
  }

  applyFilter(filterValue: string) {
    filterValue = filterValue.trim(); // Remove whitespace
    filterValue = filterValue.toLowerCase(); // Datasource defaults to lowercase matches
    this.tblDatasource.filter = filterValue;
  }
  onFileChange(ev: any) {
    let files = Array.from(ev.target.files);
    files.forEach((file: File) => {
      let form = this.formArray.controls.find(x => x.value.TraineeCode == file.name.replace(/\.[^/.]+$/, "")) as FormGroup;
      if (form != undefined && file.type.match('image.*')) {
        this.toBase64(file).then(x => { form.controls.CNICImg.setValue(x) });

      }
    });

    //let files = Array.from(ev.target.files);
    //let newDataList = this.tblData.map(item => {
    //  let newObj = {};
    //  files.forEach( (file: File) => {
    //    if (file.name.replace(/\.[^/.]+$/, "") == item.TraineeCode.trim()) {
    //      newObj = {
    //        ...item
    //        , CNICImg: this.toBase64(file).then(x => { console.log(x); return  x})
    //      }
    //    } else {
    //      newObj = item;
    //    }
    //  })
    //  return newObj;
    //});
    //console.log(this.tblData);
    //this.fillTableForm(newDataList);
    //this.commonService.openSnackBar("Files binded Successfully.");

  }

  toBase64 = file => new Promise((resolve, reject) => {
    const reader = new FileReader();
    reader.readAsDataURL(file);
    reader.onloadend = () => resolve(reader.result);
    reader.onerror = error => reject(error);
  });

  //populateFieldsFromFile(data: any) {
  //  if (data.length > 0) {
  //    //let enbaledRows = this.tblFormGroup.value.tsrFormArray.filter(x => x.TraineeID);
  //    let newDataList = this.tblData.map(item => {
  //      let foundItem = data.find(x => x.TraineeCode.trim() == item.TraineeCode.trim());
  //      if (foundItem
  //        //&& enbaledRows.find(x => x.TraineeID == foundItem.TraineeID) != undefined
  //      ) {
  //        return {
  //          ...item
  //          //, ResultStatusID: this.traineeResultStatusTypeArray.find(x => x.ResultStatusName.toLowerCase() == foundItem.ResultStatus.toLowerCase()).ResultStatusID
  //          , ResultStatusChangeReason: foundItem.Comments
  //        }
  //      } else {
  //        return item;
  //      }
  //    });
  //    this.fillTableForm(newDataList);
  //    this.commonService.openSnackBar("Upload excel data Successfully");
  //  }
  //}
  exportToExcel() {
    let filteredData = [...this.tblDatasource.filteredData]
    //let removeKeys = Object.keys(filteredData[0]).filter(x => !this.displayedColumns.includes(x));
    let data = [];//filteredData.map(x => { removeKeys.forEach(key => delete x[key]); return x });
    filteredData.forEach(item => {
      let obj = {};
      this.displayedColumns.forEach(key => {
        obj[key] = item[key] || "";
      });
      data.push(obj)
    })
    let exportExcel: ExportExcel = {
      Title: 'Trainee Updation',
      Author: this.currentUser.FullName,
      Type: EnumExcelReportType.TraineeUpdation,
      Data: {},
      List1: data,
      ImageFieldNames: ["TraineeImg", "CNICImg"]
    };
    this.dialogueService.openExportConfirmDialogue(exportExcel).subscribe();
  }
}
