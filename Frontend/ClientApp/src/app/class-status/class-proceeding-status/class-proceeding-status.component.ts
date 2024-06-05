import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonSrvService } from '../../common-srv.service';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { FormControl } from '@angular/forms';
import { SearchFilter } from '../../shared/Interfaces';
import { DialogueService } from 'src/app/shared/dialogue.service';


@Component({
  selector: 'app-class-proceeding-status',
  templateUrl: './class-proceeding-status.component.html',
  styleUrls: ['./class-proceeding-status.component.scss']
})
export class ClassProceedingStatusComponent implements OnInit {
  filters: SearchFilter = { SchemeID: 0, TSPID: 0, ClassID: 0 };
  displayedColumns: string[] = ['ClassCode', 'MonthOfMPR', 'IsGeneratedMPR', 'IsDataInsertedInAMS', 'IsGeneratedPRNRegular', 'IsGeneratedSRN', 'IsGeneratedSRNPO', 'IsGeneratedSRNInvoice']
  tableDataSource: MatTableDataSource<any[]>;
  tableData: Array<any> = new Array();
  schemeArray: Array<any> = new Array();
  tspArray: Array<any> = new Array();
  classesArray: Array<any> = new Array();
  error: any;
  SearchSch = new FormControl('');
  SearchCls = new FormControl('');
  SearchTSP = new FormControl('');
  @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: false }) sort: MatSort;
  constructor(private commonService: CommonSrvService,public dialogueService: DialogueService) {
    this.loadMatTable();
    this.getData();
  }

  ngOnInit(): void {

  }
  emptyCtrl() {
    this.SearchCls.setValue('');
    this.SearchTSP.setValue('');
    this.SearchSch.setValue('');
  }
  getData() {
    this.commonService.getJSON(`api/Class/GetClassProceeedingStatusData`).subscribe(
      data => {
        this.schemeArray = data[0];
        this.tableData = data[1];
        this.loadMatTable(this.tableData);
      },
      (error) => {
        this.error = error.error;
        this.commonService.ShowError(error.error + '\n' + error.message);
      } // error path
    )
  }
  getfilteredData() {
    let filters = "filters?" + Object.entries(this.filters).map(([key, value]) => `filters=${value}`).join('&');
    this.commonService.getJSON(`api/Class/GetFilteredClassProceeedingStatusData/${filters}`).subscribe(
      (data: any) => {
        this.loadMatTable(data);
        //debugger;
      },
      (error) => {
        this.loadMatTable();
        this.error = error.error;
        this.commonService.ShowError(error.error + '\n' + error.message);
      } // error path
    )
  }
  loadMatTable(data?: any[]) {
    this.tableDataSource = new MatTableDataSource(data)
    this.tableDataSource.paginator = this.paginator;
    this.tableDataSource.sort = this.sort;
  }
  getTSPDetailByScheme(schemeId: number) {
    this.classesArray = [];
    this.commonService.getJSON(`api/TSPDetail/GetTSPDetailByScheme/` + schemeId)
      .subscribe(data => {
        this.tspArray = <any[]>data;
      }, error => {
        this.error = error;
      })
  }
  getClassesByTsp(tspId: number) {
    this.commonService.getJSON(`api/Class/GetClassesByTsp/` + tspId)
      .subscribe(data => {
        this.classesArray = <any[]>data;
      }, error => {
        this.error = error;
      })
  }
  applyFilter(filterValue: string) {
    this.tableDataSource.filter = filterValue.trim().toLowerCase();
  }
  openClassJourneyDialogue(data: any): void 
  {
    debugger;
    this.dialogueService.openClassJourneyDialogue(data);
  }
}
