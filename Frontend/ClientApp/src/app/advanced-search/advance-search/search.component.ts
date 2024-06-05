import { Component, OnInit, ViewChild } from "@angular/core";
import { FormBuilder, FormControl, FormGroup, Validators } from "@angular/forms";
import { CommonSrvService } from "../../common-srv.service";
import { SearchFilter } from '../../shared/Interfaces';
import { MatTableDataSource } from '@angular/material/table';
import { ModelBase } from '../../shared/ModelBase';
import { HttpClient } from '@angular/common/http';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { GroupByPipe } from 'angular-pipes';
import { MatTabGroup } from '@angular/material/tabs';
import { DialogueService } from "src/app/shared/dialogue.service";

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.scss'],
  providers: [GroupByPipe]
})

export class SearchComponent implements OnInit {
  @ViewChild("tabGroup") tabGroup: MatTabGroup;
  @ViewChild("paginatorScheme") paginatorScheme: MatPaginator;
  @ViewChild("sortScheme") sortScheme: MatSort;
  @ViewChild("paginatorTSP") paginatorTSP: MatPaginator;
  @ViewChild("sortTSP") sortTSP: MatSort;
  @ViewChild("paginatorClass") paginatorClass: MatPaginator;
  @ViewChild("sortClass") sortClass: MatSort;
  @ViewChild("paginatorTrainee") paginatorTrainee: MatPaginator;
  @ViewChild("sortTrainee") sortTrainee: MatSort;
  @ViewChild("paginatorInstructor") paginatorInstructor: MatPaginator;
  @ViewChild("sortInstructor") sortInstructor: MatSort;
  @ViewChild("paginatorMPR") paginatorMPR: MatPaginator;
  @ViewChild("sortMPR") sortMPR: MatSort;
  @ViewChild("paginatorPRN") paginatorPRN: MatPaginator;
  @ViewChild("sortPRN") sortPRN: MatSort;
  @ViewChild("paginatorSRN") paginatorSRN: MatPaginator;
  @ViewChild("sortSRN") sortSRN: MatSort;
  @ViewChild("paginatorInvoice") paginatorInvoice: MatPaginator;
  @ViewChild("sortTrainee") sortInvoice: MatSort;
  searchForm: FormGroup;
  tableNamesList = [
      { value: "TraineeProfile", text: "Trainee Profile" }
    , { value: "Scheme", text: "Scheme" }
    , { value: "TSPMaster", text: "TSP" }
    , { value: "Class", text: "Class" }
    , { value: "Instructor", text: "Instructor" }
    , { value: "MPR", text: "MPR" }
    , { value: "PRNMaster", text: "PRN" }
    , { value: "SRN", text: "SRN" }
    , { value: "Invoice", text: "Invoice" }
  ]
  error: String;
  schemeList: MatTableDataSource<any> = new MatTableDataSource([]);
  schemeListColumn = ['SchemeCode', 'SchemeName', 'Description', 'MatchedColumn', 'Action'];
  tspList: MatTableDataSource<any> = new MatTableDataSource([]);
  tspListColumn = ['TSPName', 'TSPCode', 'Address', 'MatchedColumn', 'Action'];
  classList: MatTableDataSource<any> = new MatTableDataSource([]);
  classListColumn = ['ClassCode', 'ClassStatus', 'TrainingAddressLocation', 'TSPName', 'MatchedColumn', 'Action'];
  traineeList: MatTableDataSource<any> = new MatTableDataSource([]);
  traineeListColumn = ['TraineeCode', 'TraineeName', 'TraineeCNIC', 'MatchedColumn', 'Action'];

  instructorList: MatTableDataSource<any> = new MatTableDataSource([]);
  instructorListColumn = ['InstructorName', 'CNICofInstructor', 'NameOfOrganization', 'MatchedColumn', 'Action'];
  mprList: MatTableDataSource<any> = new MatTableDataSource([]);
  mprListColumn = ['MPRName', 'SchemeName', 'ClassCode', 'MatchedColumn', 'Action'];
  prnList: MatTableDataSource<any> = new MatTableDataSource([]);
  prnListColumn = ['ProcessKey', 'Month', 'InvoiceNumber', 'MatchedColumn', 'Action'];
  srnList: MatTableDataSource<any> = new MatTableDataSource([]);
  srnListColumn = ['SRNID', 'ClassCode', 'Month', 'MatchedColumn', 'Action'];
  invoiceList: MatTableDataSource<any> = new MatTableDataSource([]);
  invoiceListColumn = ['ProcessKey', 'Month', 'InvoiceNumber',
    //'TSPCode', 'TSPName',
    'Description', 'InvoiceType', 'MatchedColumn', 'Action'];

  selectedValue: any;
  selectedType: any;

  constructor(private commonService: CommonSrvService, private fb: FormBuilder, private groupBy: GroupByPipe,public dialogueService: DialogueService) {
    this.searchForm = this.fb.group(
      {
        SearchString: ['', [Validators.required]]
        , TableNames: ['']
      }
    )

    this.selectedType = null;
    this.selectedValue = null;
  }
  ngOnInit(): void {
    this.commonService.setTitle("Advance Search");
  }

  search() {
    if (this.searchForm.invalid) return;
    let filterData = {
      SearchString: this.SearchString.value
      , SearchTables: this.TableNames.value != "" ? this.TableNames.value.join(',') : this.tableNamesList.map(x => x.value).join(',')
    }

    this.commonService.postJSON("api/AdvancedSearch/Search", filterData).subscribe(
      (data: any[]) => {
        //console.log(data);
        let schemesArray = data.filter(x => x.TableName == "Scheme").map(x => { return { ...x, ...JSON.parse(x.JsonRow || null) } });
        let tspArray = data.filter(x => x.TableName == "TSPMaster").map(x => { return { ...x, ...JSON.parse(x.JsonRow || null) } });
        let classArray = data.filter(x => x.TableName == "Class").map(x => { return { ...x, ...JSON.parse(x.JsonRow || null) } });
        let traineeArray = data.filter(x => x.TableName == "TraineeProfile").map(x => { return { ...x, ...JSON.parse(x.JsonRow || null) } });
        let instructorArray = data.filter(x => x.TableName == "Instructor").map(x => { return { ...x, ...JSON.parse(x.JsonRow || null) } });

        let mprArray = data.filter(x => x.TableName == "MPR").map(x => { return { ...x, ...JSON.parse(x.JsonRow || null) } });
        let prnArray = data.filter(x => x.TableName == "PRNMaster").map(x => { return { ...x, ...JSON.parse(x.JsonRow || null) } });
        let srnArray = data.filter(x => x.TableName == "SRN").map(x => { return { ...x, ...JSON.parse(x.JsonRow || null) } });
        let invoiceArray = data.filter(x => x.TableName == "Invoice").map(x => { return { ...x, ...JSON.parse(x.JsonRow || null) } });
        
        this.schemeList = new MatTableDataSource(schemesArray);
        this.schemeList.paginator = this.paginatorScheme;
        this.schemeList.sort = this.sortScheme;

        this.tspList = new MatTableDataSource(tspArray);
        this.tspList.paginator = this.paginatorTSP;
        this.tspList.sort = this.sortTSP;

        this.classList = new MatTableDataSource(classArray);
        this.classList.paginator = this.paginatorClass;
        this.classList.sort = this.sortClass;

        this.traineeList = new MatTableDataSource(traineeArray);
        this.traineeList.paginator = this.paginatorTrainee;
        this.traineeList.sort = this.sortTrainee;

        this.instructorList = new MatTableDataSource(instructorArray);
        this.instructorList.paginator = this.paginatorInstructor;
        this.instructorList.sort = this.sortInstructor;

        this.mprList = new MatTableDataSource(mprArray);
        this.mprList.paginator = this.paginatorMPR;
        this.mprList.sort = this.sortMPR;

        this.prnList = new MatTableDataSource(prnArray);
        this.prnList.paginator = this.paginatorPRN;
        this.prnList.sort = this.sortPRN;

        this.srnList = new MatTableDataSource(srnArray);
        this.srnList.paginator = this.paginatorSRN;
        this.srnList.sort = this.sortSRN;

        this.invoiceList = new MatTableDataSource(invoiceArray);
        this.invoiceList.paginator = this.paginatorInvoice;
        this.invoiceList.sort = this.sortInvoice

        //console.log(this.schemeList);
        this.tabGroup.selectedIndex = 0;
      }
    );

  }

  onClickDetail(data) {
    this.selectedType = data.type;
    this.selectedValue = data.value;
    this.tabGroup.selectedIndex = 1;
    //console.log(data.type , data.value);
  }

  onSelectedTabChange(event) {
    if (event.index == 0) { this.selectedType = null; }
    //console.log(event);
  }
  
  openTraineeJourneyDialogue(data: any): void 
  {
    debugger;
    this.dialogueService.openTraineeJourneyDialogue(data);
    }

    openClassJourneyDialogue(data: any): void 
    {
      debugger;
      this.dialogueService.openClassJourneyDialogue(data);
    }


  ////getter
  get SearchString() { return this.searchForm.get("SearchString"); }
  get TableNames() { return this.searchForm.get("TableNames"); }

}


