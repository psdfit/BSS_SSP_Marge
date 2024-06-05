import { Component, OnInit, ViewChild, ElementRef, AfterViewInit } from '@angular/core';
import { CommonSrvService } from '../../common-srv.service';
import { Element } from '@angular/compiler/src/render3/r3_ast';
import { MatProgressBar } from '@angular/material/progress-bar';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { FormGroup, FormBuilder, Validators, FormGroupDirective, FormArray, FormControl } from '@angular/forms';
import { environment } from '../../../environments/environment';
import { ModelBase } from '../../shared/ModelBase';
import { UserRightsModel, UsersModel } from '../../master-data/users/users.component';
import { MatDialog } from '@angular/material/dialog';
import { EnumCertificationAuthority, EnumTraineeResultStatusTypes, EnumUserLevel, ExportType, EnumTraineeStatusType, EnumClassStatus, EnumExcelReportType } from '../../shared/Enumerations';
import { DialogueService } from '../../shared/dialogue.service';
import * as XLSX from 'xlsx';
import { SearchFilter, ExportExcel } from '../../shared/Interfaces';
import { GroupByPipe } from 'angular-pipes';
import { DatePipe } from '@angular/common';
import { merge } from 'rxjs';
import { startWith, switchMap } from 'rxjs/operators';
import { SelectionModel } from '@angular/cdk/collections';
import { forkJoin,Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';
import { TspDialogueComponent } from './tsp-dialogue/tsp-dialogue.component';

@Component({
  selector: 'app-tsp-trainee-portal',
  templateUrl: './tsp-trainee-portal.component.html',
  styleUrls: ['./tsp-trainee-portal.component.scss'],
  providers: [GroupByPipe, DatePipe]
})
export class TspTraineePortalComponent implements OnInit {

  environment = environment;

  TraineeDatasource: MatTableDataSource<any>;
  displayedColumns = ['Sr', 'Select', 'TraineeName', 'FatherName', 'TraineeCNIC','GenderName', 'ReligionName', 'DateOfBirth','TraineeEmail','ContactNumber1', 'DistrictName','TrainingAddressLocation', 'Disability'  ];

  filters: SearchFilter = { SchemeID: 0, TSPID: 0, ClassID: 0, TraineeID: 0, OID: this.commonService.OID.value, SelectedColumns: [] };
  @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: false }) sort: MatSort;
  error = '';
  selectedOption: number;
  selectedProgramName: string;
  selectedProgram: any;
  showButtons: boolean = false;
  isSubmitButtonDisabled: boolean = false;
  selectedTrainingLocationID: number | null = null;
  selectedTradeID: number | null = null;
  buttonText: string = 'Submit for Interview';
  currentUser: UsersModel;
  enumUserLevel = EnumUserLevel;
  resultsLength: number;
  isInternalUser = false;
  isTSPUser = false;
  programArray = [];
  districtArray = [];
  tradeArray= [];
  genderArray = [];
  submitInProgress: boolean = false;
  programFilter = new FormControl(0);
  districtFilter = new FormControl(0);
  tradeFilter = new FormControl(0);
  genderFilter = new FormControl(0);
  traineelistFilter = new FormControl(0);
  SearchPro = new FormControl('');
  SearchSch = new FormControl('');
  SearchDis = new FormControl('');
  SearchTRD = new FormControl('');
  SearchGen= new FormControl('');
  selectAll: boolean = false;
  selection = new SelectionModel<any>(true, []);
  constructor(private httpClient: HttpClient,private fb: FormBuilder, private commonService: CommonSrvService, public dialog: MatDialog, public dialogueService: DialogueService, private groupByPipe: GroupByPipe, private datePipe: DatePipe) {
  }

  ngOnInit(): void {
    this.commonService.setTitle('TSP Trainee Portal');
    this.currentUser = this.commonService.getUserDetails();
    if (this.currentUser.UserLevel === EnumUserLevel.TSP) {
      this.isTSPUser = true;
    } else if (this.currentUser.UserLevel === EnumUserLevel.AdminGroup || this.currentUser.UserLevel === EnumUserLevel.OrganizationGroup) {
      this.isInternalUser = true;
    }
    this.traineelistFilter = new FormControl('1');
    this.getprogrambyTSP();
    // Initialize pagination
    this.TraineeDatasource = new MatTableDataSource<any>([]);
    this.initializePaginator();
  }
  initializePaginator() {
    // If paginator is available and TraineeDatasource is initialized
    if (this.paginator && this.TraineeDatasource) {
      // Set the paginator property of MatTableDataSource
      this.TraineeDatasource.paginator = this.paginator;
    }
  }

  openDialog(row: any): void {
    if (this.isSubmitButtonDisabled) {
      return;
    }
    const dialogRef = this.dialog.open(TspDialogueComponent, {
      width: '60%',
      data: { ...row, ProgramName: this.selectedProgramName }
    })
    dialogRef.afterClosed().subscribe(result => {
      this.getTraineeProfileTSP();
      console.log(result);
      // this.updateTraineeStatus(result);
    })
  }

  onProgramSelectionChange(programId: number) {
    // Update the value of programFilter
    this.selectedProgram = this.programArray.find(p => p.ProgramID === programId);
    this.selectedProgramName = this.programArray.find(program => program.ProgramID === programId)?.ProgramName;
    this.programFilter.setValue(programId);
    this.getdistrictbyTSP();
    this.isSubmitButtonDisabled = this.selectedProgram?.isLocked ?? false;
  }
  onDistrictSelectionChange(district: number) {
    // Update the value of programFilter
    this.districtFilter.setValue(district);
    this.getTradebyTSP();
  }
  onTradeSelectionChange(value: string) {
    debugger;
    const [tradeID, locationID] = value.split('-').map(Number);
    // Set the selected trade IDs
    this.selectedTradeID = tradeID;
    this.selectedTrainingLocationID = locationID;
    this.getTraineeProfileTSP();
  }
  getprogrambyTSP() {
    this.commonService.getJSON(`api/Scheme/SSPFetchSchemeByUser`)
      .subscribe(data => {
        console.log(data);
        this.programArray = (data as any[]).map(program => {
        // Set isLocked based on the IsLocked value from the database
        program.isLocked = program.IsLocked === 'Closed';
          return program;
        });
      }, error => {
        this.commonService.ShowError(error.error + '\n' + error.message);
      })
  }

  getdistrictbyTSP() {
    const programid = this.programFilter.value;
    console.log("ProgramID" + programid);
    this.commonService.getJSON(`api/District/SSPRD_DistrictTSP/` + programid)
      .subscribe(data => {
        this.districtArray = (data as any[]);
      }, error => {
        this.commonService.ShowError(error.error + '\n' + error.message);
      })
  }



  getTradebyTSP() {
    const programid = this.programFilter.value;
    const districtid = this.districtFilter.value;
    this.commonService.getJSON(`api/Trade/SSPRD_TradebyTSP/` + programid + '/' + districtid )
      .subscribe(data => {
        this.tradeArray = (data as any[]);
      }, error => {
        this.commonService.ShowError(error.error + '\n' + error.message);
      })
  }
  getGenderbyTSP() {
    this.commonService.getJSON(`api/Gender/RD_Gender`)
      .subscribe(data => {
        this.genderArray = (data as any[]);
      }, error => {
        this.commonService.ShowError(error.error + '\n' + error.message);
      })
  }
  getTraineeProfileTSP() {
      const programid = this.programFilter.value;
      const districtid = this.districtFilter.value;
      const tradeid = this.tradeFilter.value;
      const submitButton = document.getElementById('btnSubmit') as HTMLButtonElement;
    
      if (this.traineelistFilter.value == 1)
      {
        this.buttonText = 'Submit for Interview';
        this.showButtons = false;
        submitButton.hidden = false;
      }
      else if (this.traineelistFilter.value ==2)
      {
        this.buttonText = 'Accept';
        this.showButtons = false;
        submitButton.hidden = true;
      }
      else if (this.traineelistFilter.value == 3)
      {
        this.buttonText = 'Final Submission of Trainees';
        this.showButtons = false;
        submitButton.hidden = true;
       
    }
    debugger;
    this.commonService.getJSON(`api/TraineeProfile/GetSubmittedTraineesByTsp/` + this.traineelistFilter.value + '/' + this.programFilter.value + '/' + this.districtFilter.value + '/' + this.selectedTradeID + '/' + this.selectedTrainingLocationID  )
        .subscribe((data: any[]) =>{
      if (data && data.length > 0) {
        this.TraineeDatasource.data = (data as any[]);
      } else {
        this.TraineeDatasource.data = []; // Set the datasource to an empty array
      }
    }, error => {
      this.commonService.ShowError(error.error + '\n' + error.message);
    })
  }
  initPagedData()
  {
    this.getTraineeProfileTSP();
  }

  onSelectAllChange(): void {
    // Toggle selection for all rows
    this.TraineeDatasource.data.forEach(row => this.selection.toggle(row));

    // Update selectAll based on whether all rows are selected
    this.selectAll = this.TraineeDatasource.data.length > 0 && this.TraineeDatasource.data.every(row => this.selection.isSelected(row));
  }
  selectionChanged(): void {
    // Check if TraineeDatasource is not null and has length greater than 0
    if (this.TraineeDatasource.data && this.TraineeDatasource.data &&  this.TraineeDatasource.data.length > 0) {
      // Update selectAll based on whether all rows are selected
      this.selectAll = this.TraineeDatasource.data.every(row => this.selection.isSelected(row));
    } else {
      // If TraineeDatasource is null or empty, set selectAll to false
      this.selectAll = false;
    }
  }


  onCheckboxChange(checkedItem: any): void {
    // Toggle selection for the clicked row
    this.selection.toggle(checkedItem);
    // Update selectAll based on whether all rows are selected
    this.selectAll = this.TraineeDatasource.data.length > 0 && this.TraineeDatasource.data.every(row => this.selection.isSelected(row));
    this.selectionChanged();
  }
  clearSelection(): void {
    // Clear the selection
    this.selection.clear();
    // Update selectAll based on whether all rows are selected
    this.selectionChanged();
  }

 
  onSubmit(traineeStatus: string) {
   
    switch (traineeStatus) {
      case 'Submit':
        const submitButton = document.getElementById('btnSubmit') as HTMLButtonElement;
        submitButton.disabled = false;
        break;
      default:
        // Handle default case or throw an error if needed
        break;
    }

    this.save(traineeStatus);
  }

  async save(traineeStatus: string) {
    let titleConfirm = "Confirmation";
    let messageConfirm = "";
    let list = this.selection.selected;

    if (!list || list.length <= 0) {
      this.commonService.ShowError("There is nothing selected");
      // Re-enable the buttons when there are no selected items to submit
      this.enableButtons(traineeStatus);
      return;
    }

    // Check if submission is already in progress
    if (this.submitInProgress) {
      return; // Prevent further submissions
    }

    // Set submission in progress flag
    this.submitInProgress = true;

    // Save Trainee For Interview
    if (this.traineelistFilter.value == 1) {
      messageConfirm = "Are you sure you want to submit for Interview!";

      const isConfirm = await this.commonService.confirm(titleConfirm, messageConfirm).toPromise();
      if (isConfirm) {
        // Get every selected row IDs
        for (const row of list) {
          const id = row.TraineeID;
          // Construct the payload with TraineeID property
          const payload = { TraineeID: id };
          try {
            debugger;
            await this.commonService.postJSON('api/TraineeProfile/SaveSubmitted', payload).toPromise();
            this.commonService.openSnackBar('Data submitted for Interview successfully!');
            //this.TraineeDatasource = null;
            this.getTraineeProfileTSP();
            //this.clearSelection();
          } catch (error) {
            console.error('Error submitting data:', error);
            this.commonService.ShowError('Error submitting data:', error);
          }
        }
        // Repopulate the TraineeDatasource
       
        this.clearSelection(); // Call clearSelection to update selectAll state
      } else {
        // Enable the buttons if user cancels the confirmation
        this.enableButtons(traineeStatus);
      }
    } 

    // Reset the submitInProgress flag after all operations are completed
    this.submitInProgress = false;
  }



  enableButtons(traineeStatus: string) {
    switch (traineeStatus) {
      case 'Submit':
        const submitButton = document.getElementById('btnSubmit') as HTMLButtonElement;
        submitButton.disabled = false;
        break;
      case 'Release':
        const releaseButton = document.getElementById('btnRelease') as HTMLButtonElement;
        releaseButton.disabled = false;
        break;
      case 'Reject':
        const rejectButton = document.getElementById('btnReject') as HTMLButtonElement;
        rejectButton.disabled = false;
        break;
      default:
        // Handle default case or throw an error if needed
        break;
    }
  }
  applyFilter(data: MatTableDataSource<any>, event: any) {
    data.filter = event.target.value.trim().toLowerCase();
    if (data.paginator) {
      data.paginator.firstPage();
    }
  }
  DataExcelExport(data: any, title) {
    this.commonService.ExcelExporWithForm(data, title);
  }
  ExcelExporWithForm(ExportDataObject, ReportName) {


    const Data = ExportDataObject.map(obj => {
      const newObj = {};
      for (const key in obj) {
        if (!key.toLowerCase().includes('id')) {
          newObj[key] = obj[key];
        }
      }
      return newObj;
    });

    if (ExportDataObject.length > 0) {
      let exportExcel: ExportExcel = {
        Title: ReportName,
        Author: this.currentUser.FullName,
        Type: EnumExcelReportType.SRN,
        Month: new Date(),
        Data: {},
        List1: Data
      };
      this.dialogueService.openExportConfirmDialogue(exportExcel).subscribe();
    } else {
      this.commonService.ShowWarning(' No Record Found', 'Close');
    }


  }

}
export interface TraineeProfile {
  TraineeID : number;
  }
