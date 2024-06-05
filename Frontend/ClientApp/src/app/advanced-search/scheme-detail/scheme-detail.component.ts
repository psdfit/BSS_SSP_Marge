import { Component, OnInit, Input, ViewChild } from "@angular/core";
import { CommonSrvService } from "../../common-srv.service";
import { environment } from '../../../environments/environment';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';

@Component({
  selector: 'scheme-detail',
  templateUrl: './scheme-detail.component.html',
  styleUrls: ['./scheme-detail.component.scss']
})


export class SchemeDetailComponent implements OnInit{

  displayedColumnsClass = ['ClassCode', 'TrainingAddressLocation', 'TradeName', 'SectorName', 'ClusterName', 'Batch', 'StartDate', 'EndDate', 'ClassStatusName'];
  dataSourceClass: MatTableDataSource<any>;

  @ViewChild('paginatorClass') paginatorClass: MatPaginator;
  @ViewChild('sortClass') sortClass: MatSort;

  constructor(private commonService: CommonSrvService) {
  }

  environment = environment;

  @Input() ID: any;

  SchemeDetail: any;
  SchemeTSPs: any[];
  SchemeClasses: any[];

  getData() {
    this.commonService.getJSON("api/AdvancedSearch/GetSchemeDetail?SchemeID=" + this.ID).subscribe((data: any[]) => {
      this.SchemeDetail = data["SchemeDetail"][0];
      this.SchemeTSPs = data["SchemeTSPs"];
      this.SchemeClasses = data["SchemeClasses"];

      this.dataSourceClass = new MatTableDataSource(this.SchemeClasses);
      this.dataSourceClass.paginator = this.paginatorClass;
      this.dataSourceClass.sort = this.sortClass;
    });
  }

  ngOnInit(): void {
    this.getData();
  }
}
