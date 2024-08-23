import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from '../../../environments/environment';
import { MatPaginator } from '@angular/material/paginator';
import { SearchFilter, ExportExcel } from '../../shared/Interfaces';
import { MatSort } from '@angular/material/sort';
import { CommonSrvService } from '../../common-srv.service';
import { FormControl } from '@angular/forms';
import { forkJoin, merge } from 'rxjs';
import { startWith, switchMap } from 'rxjs/operators';
import { DatePipe } from '@angular/common';
import { GroupByPipe } from '../../pipes/GroupBy.pipe';
import { UsersModel } from '../../master-data/users/users.component';
import { EnumExcelReportType, EnumUserLevel } from '../../shared/Enumerations';
import { DialogueService } from '../../shared/dialogue.service';
import { SelectionModel } from '@angular/cdk/collections';

@Component({
  selector: 'app-coursera-report',
  templateUrl: './coursera-report.component.html',
  styleUrls: ['./coursera-report.component.scss'],
  providers: [GroupByPipe, DatePipe]
})
export class CourseraReportComponent implements OnInit {
  environment = environment;
  tsrDatasource: any[];
  displayedColumns = ['Sr','Action', 'TraineeCode', 'TraineeName', 'FatherName', 'TraineeCNIC', 'GenderName', 'ContactNumber1', 'TraineeAge', 'TraineeEmail', 'InvitationDate'];
  schemeArray = [];
  tspDetailArray = [];
  classesArray: any[];
  filters: SearchFilter = { SchemeID: 0, TSPID: 0, ClassID: 0, TraineeID: 0, OID: this.commonService.OID.value, SelectedColumns: [] };
  @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: false }) sort: MatSort;
  error = '';
  schemeFilter = new FormControl(0);
  SearchSch = new FormControl('');
  SearchTSP = new FormControl('');
  SearchCls = new FormControl('');
  resultsLength: number;
  currentUser: UsersModel;
  enumUserLevel = EnumUserLevel;
  TSRDataModelKeys = Object.keys(new TSRDataModel());
  selectAll: boolean = false;
  selection = new SelectionModel<any>(true, []);
  constructor(
    private commonService: CommonSrvService,
    private datePipe: DatePipe,
    private groupByPipe: GroupByPipe,
    public dialogueService: DialogueService
  ) { }

  ngOnInit(): void {
    this.commonService.setTitle('Trainee Status Report');
    this.currentUser = this.commonService.getUserDetails();
  }

  ngAfterViewInit() {
    this.commonService.OID.subscribe(OID => {
      this.schemeFilter.setValue(0);
      this.filters.OID = OID;
      this.initPagedData();
    });

    // Set selectAll based on the initial state of selection
    this.selectAll = this.tsrDatasource.length > 0 && this.selection.selected.length === this.tsrDatasource.length;
  }


  EmptyCtrl(Ev: any) {
    this.SearchCls.setValue('');
    this.SearchTSP.setValue('');
    this.SearchSch.setValue('');
  }

  getData() {
    this.commonService.getJSON(`api/TSRLiveData/GetSchemesForTSR?OID=${this.commonService.OID.value}`)
      .subscribe((d: any) => {
        this.schemeArray = d.Schemes;
      }, error => this.error = error);
  }

  applyFilter(filterValue: string) {
    filterValue = filterValue.trim().toLowerCase();
    // this.tsrDatasource.filter = filterValue;
  }

  initPagedData() {
    this.sort.sortChange.subscribe(() => this.paginator.pageIndex = 0);
    this.paginator.pageSize = 10;
    merge(this.sort.sortChange, this.paginator.page, this.schemeFilter.valueChanges)
      .pipe(
        startWith({}),
        switchMap(() => {
          const pagedModel = {
            PageNo: this.paginator.pageIndex + 1,
            PageSize: this.paginator.pageSize,
            SortColumn: this.sort.active,
            SortOrder: this.sort.direction,
            SearchColumn: '',
            SearchValue: '',
          };
          this.filters.SchemeID = this.schemeFilter.value;
          return this.getPagedData(pagedModel, this.filters);
        })
      ).subscribe(data => {
        this.tsrDatasource = data[0];
        this.resultsLength = data[1].TotalCount;
      });
  }

  getPagedData(pagingModel, filterModel) {
    return this.commonService.postJSON('api/CourseraReport/RD_CourseraReportPaged', { pagingModel, filterModel });
  }

  openTraineeJourneyDialogue(data: any): void {
    this.dialogueService.openTraineeJourneyDialogue(data);
  }

  onSelectAllChange(): void {
    // Toggle selectAll property
    this.selectAll = !this.selectAll;

    // Update selection state for all checkboxes
    if (this.selectAll) {
      // Select all checkboxes
      this.tsrDatasource.forEach(item => {
        if (!item.isChecked) {
          item.isChecked = true;
          this.selection.select(item); // Manually update selection
        }
      });
    } else {
      // Deselect all checkboxes
      this.tsrDatasource.forEach(item => {
        if (item.isChecked) {
          item.isChecked = false;
          this.selection.deselect(item); // Manually update selection
        }
      });
    }
  }




  onCheckboxChange(checkedItem: any): void {
    this.selection.toggle(checkedItem); // Toggle selection for the clicked row
    this.selectAll = this.tsrDatasource.length > 0 && this.selection.selected.length === this.tsrDatasource.length;
  }

  onSubmit() {
    if (this.selection.isEmpty()) {
      this.commonService.ShowError("There is nothing selected");
      return;
    }
    this.save();
  }

  save() {
    let titleConfirm = "Confirmation";
    let messageConfirm = "";
    let list = this.selection.selected;

    if (!list || list.length <= 0) {
      this.commonService.ShowError("There is nothing selected");
      return;
    }
    // Save Trainee For Interview

    messageConfirm = "Are you sure you want to change trainee status";
    this.commonService.confirm(titleConfirm, messageConfirm).subscribe(
      (isConfirm: Boolean) => {
        if (isConfirm) {
          // Get every selected row IDs
          const ids = list.map(row => row.TraineeID); // Extracting IDs from selected rows

          // Determine the API endpoint based on the selected value from the dropdown
          let apiEndpoint;
          let comment;
          if (this.schemeFilter.value == 1) {
            apiEndpoint = 'api/CourseraReport/SaveExpell';
            comment = 'Expelled: Invitation Not Accepted';
          } else if (this.schemeFilter.value == 2) {
            apiEndpoint = 'api/CourseraReport/SaveExpell';
            comment = 'Dropout: Inactive for 14 days below 70%';
          } else if (this.schemeFilter.value == 3) {
            apiEndpoint = 'api/CourseraReport/SaveExpell';
            comment = 'Dropout: Inactive for 28 days above 70%';
          } else if (this.schemeFilter.value >= 4) {
            apiEndpoint = 'api/CourseraReport/SaveExpell';
            comment = 'Dropout: Inactive for More than 3 months';
          }
          else {
            apiEndpoint = 'api/CourseraReport/SaveExpell';
            comment = 'Dropout: Inactive for 28 days above 70%';
          }


          // Construct the payload with IDs and Comment based on schemeFilter value
          const payload = { CoursraTraineeIDs: ids.join(','), Comment: comment };

          // Send a single HTTP post request with the determined API endpoint and payload
          this.commonService.postJSON(apiEndpoint, payload).subscribe(
            response => {
              this.commonService.openSnackBar('Trainees status changed successfully!');
              this.initPagedData();
            },
            error => {
              this.commonService.ShowError('Error submitting data:', error);
            }
          );
        }
      }
    );
  } //end save


}

export class TSRDataModel {
  'Sr#': any = 'Sr';
  'Scheme': any = 'SchemeName';
  'Training Service Provider': any = 'TSPName';
  'Trade Group': any = 'SectorName';
  'Trade': any = 'TradeName';
  'ClassCode': any = 'ClassCode';
  'Trainee ID': any = 'TraineeCode';
  'Trainee Name': any = 'TraineeName';
  'Father\'s Name': any = 'FatherName';
  'CNIC Issue Date': any = 'CNICIssueDate'
  'CNIC': any = 'TraineeCNIC';
  'Date Of Birth': any = 'DateOfBirth'
  'Roll #': any = 'TraineeRollNumber';
  'Batch': any = 'Batch';
  'Section': any = 'SectionName';
  'Shift': any = 'Shift';
  'Trainee Address': any = 'TraineeHouseNumber,TraineeStreetMohalla,TraineeMauzaTown,TraineeTehsilName,TraineeDistrictName';
  'Residence Tehsil': any = 'TraineeTehsilName';
  'District of Residence': any = 'TraineeDistrictName';
  'Province of Residence': any = 'ProvinceName';
  'Gender': any = 'GenderName';
  'Education': any = 'Education';
  'Contact Number': any = 'ContactNumber1';
  'Training Location': any = 'TrainingAddressLocation';
  'District of Training Location': any = 'ClassDistrictName';
  'CNIC Verified': any = 'TraineeVerified';
  'Trainee Status': any = 'TraineeStatusName';
  'Is Dual': any = 'IsDual';
  'Trainee Status Update Date': any = 'TraineeStatusChangeDate';
  'Examination Assesment': any = 'ResultStatusName';
  'Voucher Holder': any = 'VoucherHolder';
  'Reason': any = 'TraineeStatusChangeReason';
  'Class ID': any = 'ClassUID';
  'Trainee Profile ID': any = 'TraineeUID';
  'CNIC IMG Status': any = 'IsVarifiedCNIC';
  'Trainee Img': any = 'TraineeImg';
  'CNIC Img': any = 'CNICImgNADRA';
  'Sector': any = 'SectorName';
  'Cluster': any = 'ClusterName';
  'KAM': any = 'KAM';
  'Trainee Employment Status': any = 'TraineeEmploymentStatus';
  'Trainee Employment Verification Status': any = 'TraineeEmploymentVerificationStatus';
  'Trainee Email': any = 'TraineeEmail';
  'Class Status': any = 'ClassStatusName';
  'Class Start Date': any = 'StartDate';
  'Class End Date': any = 'EndDate';
  'Certify Authority' = 'CertAuthName';
  'Religion' = 'ReligionName';
  'Disability': any = 'Disability';
  'Dvv': any = 'Dvv';
}
