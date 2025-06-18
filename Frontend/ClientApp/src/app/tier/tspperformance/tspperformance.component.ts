
import { FormControl } from '@angular/forms';
import { Router } from '@angular/router';
import { CommonSrvService } from 'src/app/common-srv.service';
import { AfterViewInit, Component, ElementRef, OnInit, QueryList, ViewChild, ViewChildren } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import * as XLSX from 'xlsx';
import * as FileSaver from 'file-saver';
@Component({
  selector: 'app-tspperformance',
  templateUrl: './tspperformance.component.html',
  styleUrls: ['./tspperformance.component.scss']
})
export class TSPPerformanceComponent implements OnInit {
  Schemes: any[];
  TradeLookup: any[];
  TSPPerformanceArray:any[];
  DatePeriod: any[];
   startDate:any;
   endDate:any;
  classesArray: any[];
  error: String;
  TSPDetail = [];
  Scheme: any[];
  filters: IQueryFilters = { SchemeID: 0, TSPID: 0, Date: '',TradeID: 0};
  constructor(private ComSRV: CommonSrvService, public dialog: MatDialog, private router: Router) { }

  ngOnInit(): void {
    this.ComSRV.setTitle("TSP Performance");
    this.getSchemes();
    this.TSPPerformanceDatePeriod();
  }
  getDataByFilters() {
    this.TSPPerformance();
  }
  TSPPerformance() {
    this.ComSRV.postJSON('api/Tier/GetTSPPerformance',this.filters).subscribe(
      (d: any) => {
        this.TSPPerformanceArray = d;
      }, error => this.error = error
    );
  }
  TSPPerformanceDatePeriod() {
    this.ComSRV.getJSON('api/Tier/TSPPerformanceDatePeriod').subscribe(
      (d: any) => {
        this.DatePeriod = d;
      }, error => this.error = error
    );
  }
  getSchemes() {
    this.ComSRV.postJSON('api/Scheme/FetchSchemeByUser', this.filters).subscribe(
      (d: any) => {
        this.Schemes = d;
      }, error => this.error = error
    );
  }
  FetchTradeDetailByTSP()
  {
    this.TradeLookup = [];
    this.TradeFilter.setValue(0);
    this.ComSRV.postJSON('api/Tier/FetchTradeDetailByTSP', this.filters).subscribe(
      (d: any) => {
        this.TradeLookup = d;
      }, error => this.error = error
    );

  }
  getSchemeData() {
    let interval = setInterval(() => {
      if (interval && this.Schemes) {
        let newSchemes = this.filters.SchemeID > 0 ? this.Schemes.filter(x => x.SchemeID == this.filters.SchemeID) : this.PageSchemeData; // this.Schemes;
        this.Scheme = newSchemes;
        
        
        interval = null;
        clearInterval(interval);
      }
    }, 1000);
  }
  getTSPDetailByScheme(schemeId: number) {
    this.classesArray = [];
    this.tspFilter.setValue(0);
    this.TradeLookup = [];
    this.TradeFilter.setValue(0);
    this.ComSRV.getJSON(`api/TSPDetail/GetTSPDetailByScheme/` + schemeId)
      .subscribe(data => {
        this.TSPDetail = <any[]>data;
      }, error => {
        this.error = error;
      });
  }
  SearchSch = new FormControl('');
  SearchCls = new FormControl('');
  SearchTSP = new FormControl('');
  SearchTrade   =new FormControl('');
  EmptyCtrl(ev: any) {
    this.SearchCls.setValue('');
    this.SearchTSP.setValue('');
    this.SearchSch.setValue('');
    this.SearchTrade.setValue('');
  }
  PageSchemeData: any[];
  PageClassData: any[];
  schemeFilter = new FormControl(0);
  tspFilter = new FormControl(0);
  Date = new FormControl(0);
  TradeFilter = new FormControl(0);
  ngAfterViewInit() {
    this.ComSRV.OID.subscribe(OID => {
      this.filters.SchemeID = 0;
      this.filters.TSPID = 0;
      this.schemeFilter.setValue(0);
      this.tspFilter.setValue(0);
      this.Date.setValue('');
      this.filters.TradeID=0;
      this.TradeFilter.setValue(0);
    })
  }

  downloadExcel(): void {
    const worksheet: XLSX.WorkSheet = XLSX.utils.json_to_sheet(this.TSPPerformanceArray);
    const workbook: XLSX.WorkBook = {
      Sheets: { 'TSP Performance': worksheet },
      SheetNames: ['TSP Performance']
    };
    const excelBuffer: any = XLSX.write(workbook, { bookType: 'xlsx', type: 'array' });
    this.saveAsExcelFile(excelBuffer, 'TSP_Performance');
  }

  private saveAsExcelFile(buffer: any, fileName: string): void {
    const data: Blob = new Blob([buffer], { type: 'application/octet-stream' });
    FileSaver.saveAs(data, `${fileName}_export_${new Date().getTime()}.xlsx`);
  }
}
 export interface IQueryFilters {
  SchemeID: number;
  TSPID: number;
  TradeID: number;
  Date: string;
}
