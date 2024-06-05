import { ChangeDetectorRef, Component, Inject, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatDialog } from '@angular/material/dialog';
import { CommonSrvService } from 'src/app/common-srv.service';
import { MatTableDataSource } from "@angular/material/table";
import { MatPaginator } from "@angular/material/paginator";
import { MatSort } from "@angular/material/sort";
@Component({
  selector: 'app-error-log-table',
  templateUrl: './error-log-table.component.html',
  styleUrls: ['./error-log-table.component.scss']
})
export class ErrorLogTableComponent implements OnInit {
  currentUser: any;
  Status: any = []
  check: boolean = false
  tradeManageIds: any;
  Criteria: Object;
  TablesData: MatTableDataSource<any>;
  @ViewChild("paginator") paginator: MatPaginator;
  @ViewChild("sort") sort: MatSort;
  TableColumns = [];
  constructor(
    private fb: FormBuilder,
    public ComSrv: CommonSrvService,
    public dialog: MatDialog,
    private cdr: ChangeDetectorRef,
    public dialogRef: MatDialogRef<ErrorLogTableComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any) {
    dialogRef.disableClose = true;
  }
  ngOnInit(): void {
    this.TablesData = new MatTableDataSource([]);
    this.LoadMatTable(this.data)
  }
  camelCaseToWords(input: string): string {
    return input.replace(/([a-z])([A-Z])/g, '$1 $2');
  }
  LoadMatTable(tableData: any[]) {
    this.cdr.detectChanges()
    if (tableData.length > 0) {
      const excludeColumnArray = []
      this.TableColumns = Object.keys(tableData[0]).filter(key => !key.includes('ID') && !excludeColumnArray.includes(key));
      this.TablesData = new MatTableDataSource(tableData)
      this.TablesData.paginator = this.paginator;
      this.TablesData.sort = this.sort;
    }
  }
  ShowPreview(fileName: string) {
    this.ComSrv.PreviewDocument(fileName)
  }
}
