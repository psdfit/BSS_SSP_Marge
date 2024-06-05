import { OnInit, Component, ViewChild, AfterViewInit } from '@angular/core';
import { CommonSrvService } from '../../common-srv.service';
import { UserRightsModel } from '../../master-data/users/users.component';
import { DialogueService } from '../../shared/dialogue.service';
import { EnumApprovalProcess } from '../../shared/Enumerations';
import { environment } from '../../../environments/environment';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { animate, trigger, state, transition, style } from '@angular/animations';

@Component({
  selector: 'app-test',
  templateUrl: './test.component.html',
  styleUrls: ['./test.component.scss'],
  animations: [
    trigger('detailExpand', [
      state('collapsed', style({ height: '0px', minHeight: '0' })),
      state('expanded', style({ height: '*' })),
      transition('expanded <=> collapsed', animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)')),
    ]),
  ],
})

export class TestComponent implements OnInit, AfterViewInit {
  environment = environment;

  displayedColumns: string[] = ['Action', 'ClassCode', 'StartDate', 'EndDate', 'Duration', 'Requested'];
  displayedSubColumns: string[] = ['ClassCode', 'StartDate', 'EndDate', 'Duration', 'Requested'];
  classes: MatTableDataSource<any>;
  currentClassDates: any;
  expandedElement: any;
  resultsLength: number;

  ActiveFormApprovalID: number;
  ChosenTradeID: number;
  title: string;
  savebtn: string;
  formrights: UserRightsModel;
  EnText = '';
  error: string;

  @ViewChild(MatPaginator) paginator: MatPaginator;

  working: boolean;

  constructor(private http: CommonSrvService, private dialogue: DialogueService) {
    // this.schemes = new MatTableDataSource([]);
    this.formrights = http.getFormRights();
  }
  ngOnInit(): void {
    this.http.setTitle('Class Dates Change Request Approvals');
    this.title = '';
    this.savebtn = 'Approve';
  }

  ngAfterViewInit() {
    this.GetClassDatesCRs();
  }

  GetClassDatesCRs() {
    this.http.getJSON('api/ClassChangeRequest/GetClassDatesChangeRequest').subscribe((d: any) => {
      this.classes = new MatTableDataSource(d[0]);
      this.classes.paginator = this.paginator;
      // this.tsps.paginator = this.paginator;
      // this.tsps.sort = this.sort;
    },
      error => this.error = error // error path
      , () => {
        this.working = false;
      });
  }
  applyFilter(filterValue: string) {
    filterValue = filterValue.trim(); // Remove whitespace
    filterValue = filterValue.toLowerCase(); // Datasource defaults to lowercase matches
    this.classes.filter = filterValue;
}
  GetCurrentClassDatesByID(r) {
    if (r.currentClassDates) {
      r.currentClassDates = null;
      this.currentClassDates = null;
      this.expandedElement = null;
    }
    else {
      this.http.postJSON('api/Class/RD_ClassBy/', { ClassID: r.ClassID }).subscribe((d: any) => {
        r.currentClassDates = d;
        this.currentClassDates = d;
        this.expandedElement = r;
      });
    }
  }

  openApprovalDialogue(row: any): void {
    const processKey = EnumApprovalProcess.CR_CLASS_DATES;

    this.dialogue.openApprovalDialogue(processKey, row.ClassDatesChangeRequestID).subscribe(result => {
      console.log(result);
      // location.reload();
    });
  }
}
