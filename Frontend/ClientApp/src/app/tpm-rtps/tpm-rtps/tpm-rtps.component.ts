import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ActivatedRoute } from '@angular/router';
import { environment } from '../../../environments/environment';
import { CommonSrvService } from '../../common-srv.service';
import { UserRightsModel, UsersModel } from '../../master-data/users/users.component';
import { TPMCenterInspectionComponent } from '../tpm-center-inspection/tpm-center-inspection.component';
import { ExportExcel } from '../../shared/Interfaces';
import { EnumExcelReportType } from '../../shared/Enumerations';
import { DialogueService } from '../../shared/dialogue.service';




@Component({
  selector: 'app-tpm-rtps',
  templateUrl: './tpm-rtps.component.html',
  styleUrls: ['./tpm-rtps.component.scss']
})

export class TPMRTPsComponent implements OnInit {
  environment = environment;
  pbteform: FormGroup;
  title: string; savebtn: string;
  currentUser: UsersModel;
  @ViewChild('matTable') table: ElementRef;

  displayedColumns = ['SchemeName', 'TSPName', 'DistrictName', 'TehsilName',
    'CreatedDate', 'ClassCode', 'TradeName',
    'TraineesPerClass', 'Duration', 'Curriculum',
    // 'RTPID',
    // 'ClassID',
    'AddressOfTrainingLocation',
    'CPName', 'CPLandline', 'StartDate',
    // 'TraineeID',
    'Comments',
    // 'Action'
  ];

  tpmrtps: MatTableDataSource<any>;
  pbteTSPs: MatTableDataSource<any>;
  pbteTrainees: MatTableDataSource<any>;
  pbteDropOutTrainees: MatTableDataSource<any>;

  selectedClasses: any;
  selectedTrainees: any;
  selectedTSPs: any;

  data: any;

  traineeResultStatusTypes: any;

  update: string;



  isOpenRegistration = false;
  isOpenRegistrationMessage = '';
  formrights: UserRightsModel;
  EnText = 'TPMRTP';
  error: string;
  query = {
    order: 'IncepReportID',
    limit: 5,
    page: 1
  };
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;


  @ViewChild('SortTrainee') SortTrainee: MatSort;
  @ViewChild('PageTrainee') PageTrainee: MatPaginator;
  @ViewChild('SortDropOutTrainee') SortDropOutTrainee: MatSort;
  @ViewChild('PageDropOutTrainee') PageDropOutTrainee: MatPaginator;
  @ViewChild('SortTSP') SortTSP: MatSort;
  @ViewChild('PageTSP') PageTSP: MatPaginator;
  @ViewChild('SortClass') SortClass: MatSort;
  @ViewChild('PageClass') PageClass: MatPaginator;

  working: boolean;
  constructor(private fb: FormBuilder, private ComSrv: CommonSrvService, private route: ActivatedRoute,
    public dialog: MatDialog, public dialogueService: DialogueService) {
    this.pbteform = this.fb.group({
      IncepReportID: 0,
      FinalSubmitted: 0,
      SectionID: ['', Validators.required],
      InActive: ''
    }, { updateOn: 'blur' });
    this.tpmrtps = new MatTableDataSource([]);
    this.pbteTrainees = new MatTableDataSource([]);
    this.formrights = ComSrv.getFormRights('/pbte');
    this.currentUser = this.ComSrv.getUserDetails();
  }



  GetData() {
    this.ComSrv.getJSON('api/RTP/GetRTPByTPM').subscribe((d: any) => {
      this.tpmrtps = new MatTableDataSource(d[0]);
      this.tpmrtps.paginator = this.PageClass;
      this.tpmrtps.sort = this.SortClass;
    }, error => this.error = error// error path
    );
  };



  ngOnInit() {
    this.ComSrv.setTitle('TPM-RTPs');
    this.title = 'Add New ';
    this.savebtn = 'Save ';
    this.GetData();
  }

  openCenterInspectionDialog(row): void {
    const dialogRef = this.dialog.open(TPMCenterInspectionComponent, {
      minWidth: '1000px',
      minHeight: '600px',
      // data: JSON.parse(JSON.stringify(row))
      data: { ClassID: row.ClassID, ClassCode: row.ClassCode }
      // this.GetVisitPlanData(data)
    })
    dialogRef.afterClosed().subscribe(result => {
      // console.log(result);
      // this.visitPlan = result;
      // this.submitVisitPlan(result);
    })
  }



  toggleActive(row) {
    this.ComSrv.confirm().subscribe(result => {
      if (result) {
        this.ComSrv.postJSON('api/InceptionReport/ActiveInActive', { IncepReportID: row.IncepReportID, InActive: row.InActive })
          .subscribe((d: any) => {
            // tslint:disable-next-line: max-line-length
            this.ComSrv.openSnackBar(row.InActive === true ? environment.InActiveMSG.replace('${Name}', this.EnText) : environment.ActiveMSG.replace('${Name}', this.EnText));
            // this.inceptionreport =new MatTableDataSource(d);
          },
            error => this.error = error // error path
          );
      }
      else {
        row.InActive = !row.InActive;
      }
    });
  }
  applyFilter(filterValue: string) {
    filterValue = filterValue.trim(); // Remove whitespace
    filterValue = filterValue.toLowerCase(); // Datasource defaults to lowercase matches
    // this.pbteClasses.filter = filterValue;
  }
  exportToExcel() {
    console.log(this.table);

    const exportExcel: ExportExcel = {
      Title: EnumExcelReportType[EnumExcelReportType.TMP_RTP],
      Author: this.currentUser.FullName,
      Type: EnumExcelReportType.TMP_RTP,
      List1: this.getDisplayColumnsData(this.tpmrtps.filteredData, this.displayedColumns)
    };
    this.dialogueService.openExportConfirmDialogue(exportExcel).subscribe();
  }

  getDisplayColumnsData(data: any[],displayColulmn:any[]) {
    const filteredData = JSON.parse(JSON.stringify(data))
    // let removeKeys = Object.keys(filteredData[0]).filter(x => !displayColulmn.includes(x));
    const dataList = [];// filteredData.map(x => { removeKeys.forEach(key => delete x[key]); return x });
    filteredData.forEach(item => {
      const obj = {};
      this.displayedColumns.forEach(key => {
        obj[key] = item[key] || '';
      });
      dataList.push(obj)
    })
    return dataList;
  }

  openClassJourneyDialogue(data: any): void
  {
		this.dialogueService.openClassJourneyDialogue(data);
  }

  get InActive() { return this.pbteform.get('InActive'); }

}

