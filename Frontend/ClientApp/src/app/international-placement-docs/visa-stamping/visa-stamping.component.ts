import { Component, OnInit, ViewChild } from "@angular/core";
import { CommonSrvService } from "../../common-srv.service";
import { ActivatedRoute } from "@angular/router";
import { MatTableDataSource } from "@angular/material/table";
import { MatPaginator } from "@angular/material/paginator";
import { MatSort } from "@angular/material/sort";
import { MatTabGroup } from "@angular/material/tabs";
import { FormBuilder, FormControl, FormGroup, Validators } from "@angular/forms";
import { MatDialog } from "@angular/material/dialog";
import { EnumUserLevel } from "src/app/shared/Enumerations";
import { VisaStampingDocDialogComponent } from "../visa-stamping-doc-dialog/visa-stamping-doc-dialog.component";
import { SearchFilter } from "src/app/shared/Interfaces";
import { EnumApprovalProcess } from '../../shared/Enumerations';
import { DialogueService } from '../../shared/dialogue.service';

@Component({
  selector: 'app-visa-stamping',
  templateUrl: './visa-stamping.component.html',
  styleUrls: ['./visa-stamping.component.scss']
})
export class VisaStampingComponent implements OnInit {
  currentUser: any = {}
  schemeArray: any;
  tspDetailArray: any;
  classesArray: any;
  noRecords: boolean;

  constructor(
    private dialog: MatDialog,
    private ComSrv: CommonSrvService,
    private AcitveRoute: ActivatedRoute,
    private fb: FormBuilder,
    private http: CommonSrvService,
    private dialogue: DialogueService
  ) { }
  TablesData: MatTableDataSource<any>;
  @ViewChild("tabGroup") tabGroup: MatTabGroup;
  @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: false }) sort: MatSort;
  CountZeroToHun = [];
  TapIndex: any;
  readOnly = true
  required = false
  error: any;
  displayedColumns: string[] = []
  SpacerTitle: string;

  SearchCls = new FormControl('');
  SearchSch = new FormControl('');
  SearchTSP = new FormControl('');

  schemeFilter = new FormControl(0);
  tspFilter = new FormControl(0);
  classFilter = new FormControl(0);

  selectedSchemeID: number = 0;
  selectedTSPID: number = 0;
  selectedClassID: number = 0;

  TapTTitle: string = "Profile"

  Data: any = []
  TableColumns = [];

  enumUserLevel = EnumUserLevel;
  fetchedDocuments: any[] = []; // Store fetched documents


  filters: SearchFilter = { SchemeID: 0, TSPID: 0, ClassID: 0, TraineeID: 0, OID: this.ComSrv.OID.value, SelectedColumns: [] };

  ngOnInit(): void {
    this.currentUser = this.ComSrv.getUserDetails();
    this.TablesData = new MatTableDataSource([]);
    this.GetIPTrainees();
    this.getSchemesData(); // Fetch schemes on component load
    this.PageTitle();

    // Update class dropdown based on selected scheme
    this.schemeFilter.valueChanges.subscribe(value => {
      this.selectedSchemeID = value;
      if (this.currentUser.UserLevel === this.enumUserLevel.TSP) {
        this.getDependantFilters();
      } else {
        this.getTSPDetailByScheme(value);
        this.GetIPTrainees();
      }
    });

    this.tspFilter.valueChanges.subscribe(value => {
      if (value == '0') {
        this.selectedTSPID = null
      } else {
        this.selectedTSPID = value;
      }
      this.getClassesByTsp(value);
      this.GetIPTrainees();
    });

    this.classFilter.valueChanges.subscribe(value => {
      if (value == '0') {
        this.selectedClassID = null
      } else {
        this.selectedClassID = value;
      }
      this.GetIPTrainees();
    });
  }

  formatDate(dateString: string): string {
    if (!dateString) return ''; // Return empty string for null or undefined values
    // Check if the date is already in DD/MM/YYYY format
    if (dateString.includes('/')) {
      const [day, month, year] = dateString.split('/');
      // Reformat to YYYY-MM-DD for the Date constructor
      const isoDateString = `${year}-${month}-${day}`;
      const date = new Date(isoDateString);
      // Check if the date is valid
      if (isNaN(date.getTime())) {
        console.warn('Invalid date string:', dateString);
        return ''; // Return empty string for invalid dates
      }
      // Format back to DD/MM/YYYY
      const formattedDay = date.getDate().toString().padStart(2, '0');
      const formattedMonth = (date.getMonth() + 1).toString().padStart(2, '0');
      const formattedYear = date.getFullYear();
      return `${formattedDay}/${formattedMonth}/${formattedYear}`;
    }
    // Handle ISO format (e.g., "2023-09-30T00:00:00")
    const date = new Date(dateString);
    if (isNaN(date.getTime())) {
      console.warn('Invalid date string:', dateString);
      return ''; // Return empty string for invalid dates
    }
    const day = date.getDate().toString().padStart(2, '0');
    const month = (date.getMonth() + 1).toString().padStart(2, '0');
    const year = date.getFullYear();
    return `${day}/${month}/${year}`;
  }

  LoadMatTable(tableData: any[]) {
    const excludeColumnArray: string[] = [
      'IsIPPCDocUploaded',
      'PrometricCostApprovalStatus',
      'IsIPOTDocUploaded',
      'OtherTraineeCostApprovalStatus',
      'IsIPMCDocUploaded',
      'MedicalCostApprovalStatus'
    ];

    if (tableData.length > 0) {
      const processedData = tableData.map(row => {
        let newRow = { ...row };
        // Format dates
        newRow.ClassStartDate = this.formatDate(newRow.ClassStartDate);
        newRow.ClassEndDate = this.formatDate(newRow.ClassEndDate);
        newRow.TraineeDOB = this.formatDate(newRow.TraineeDOB);
        // Handle "Documents Uploaded" column (0/1 to No/Yes)
        if ('IsIPVSDocUploaded' in newRow) {
          newRow['DocumentsUploaded'] = newRow['IsIPVSDocUploaded'] === 1 ? 'Yes' : 'No';
          delete newRow['IsIPVSDocUploaded'];
        }
        return newRow;
      });
      this.TableColumns = [
        'Sr#',
        ...Object.keys(processedData[0]).filter(key =>
          !key.includes('ID') && !excludeColumnArray.includes(key)
        )
      ];
      this.TableColumns.push('Document');

      this.TablesData = new MatTableDataSource(processedData);
      this.TablesData.paginator = this.paginator;
      this.TablesData.sort = this.sort;
    } else {
      this.TablesData = new MatTableDataSource([]);
    }
  }

  EmptyCtrl() {
    this.SearchCls.setValue('');
    this.SearchTSP.setValue('');
    this.SearchSch.setValue('');
  }

  camelCaseToWords(input: string): string {
    return input.replace(/([a-z])([A-Z])/g, '$1 $2');
  }

  applyFilter(event: any) {
    this.TablesData.filter = event.target.value.trim().toLowerCase();
    if (this.TablesData.paginator) {
      this.TablesData.paginator.firstPage();
    }
  }

  DataExcelExport() {
    const excludeColumnArray: string[] = [
      'IsIPPCDocUploaded',
      'PrometricCostApprovalStatus',
      'IsIPOTDocUploaded',
      'OtherTraineeCostApprovalStatus',
      'IsIPMCDocUploaded',
      'MedicalCostApprovalStatus'
    ];
    const exportData = this.TablesData.filteredData.map((row: any) => {
      let newRow = { ...row };
      // Format dates
      newRow.ClassStartDate = this.formatDate(newRow.ClassStartDate);
      newRow.ClassEndDate = this.formatDate(newRow.ClassEndDate);
      newRow.TraineeDOB = this.formatDate(newRow.TraineeDOB);
      // Handle "Documents Uploaded" column (0/1 to No/Yes)
      if ('DocumentsUploaded' in newRow) {
        newRow['DocumentsUploaded'] = newRow['Documents Uploaded'] === 1 ? 'Yes' : 'No';
      }
      // Remove unwanted columns
      excludeColumnArray.forEach(col => delete newRow[col]);
      return newRow;
    });
    this.ComSrv.ExcelExporWithForm(exportData, this.SpacerTitle);
  }


  getClassesByTsp(tspId: number) {
    this.classFilter.setValue(0);
    this.ComSrv.getJSON(`api/Dashboard/FetchClassesByTSP?TspID=${tspId}`)
      .subscribe(data => {
        this.classesArray = (data as any[]);
      }, error => {
        this.error = error;
      });
  }

  getSchemesData() {
    this.ComSrv.getJSON(`api/TSRLiveData/GetSchemesForTSR?OID=${this.ComSrv.OID.value}`)
      .subscribe((d: any) => {
        this.schemeArray = d.Schemes;
      }, error => this.error = error);
  }

  getTspDetails() {
    this.ComSrv.getJSON(`api/Dashboard/FetchTSPDetails`)
      .subscribe(data => {
        this.tspDetailArray = data;
      }, error => {
        this.error = error;
      });
  }

  getClassesBySchemeFilter() {
    this.filters.ClassID = 0;
    this.filters.TraineeID = 0;
    this.ComSrv.getJSON(`api/Dashboard/FetchClassesBySchemeUser?SchemeID=${this.schemeFilter.value}&UserID=${this.currentUser.UserID}`)
      .subscribe(data => {
        this.classesArray = (data as any[]);
      }, error => {
        this.error = error;
      });
  }

  getTSPDetailByScheme(schemeId: number) {
    this.tspFilter.setValue(0);
    this.classFilter.setValue(0);
    this.ComSrv.getJSON(`api/Dashboard/FetchTSPsByScheme?SchemeID=${schemeId}`)
      .subscribe(data => {
        this.tspDetailArray = (data as any[]);
      }, error => {
        this.error = error;
      });
  }

  getDependantFilters() {
    this.getClassesBySchemeFilter();
    this.GetIPTrainees();
  }

  paramObject: any = {}
  ExportReportName: string = ""
  SPName: string = ""

  async GetIPTrainees() {
    this.SPName = "RD_IPTrainees";
    this.paramObject = {};
    if (this.currentUser.UserLevel === this.enumUserLevel.TSP) {
      this.paramObject.UserID = this.currentUser.UserID;
    }

    const params = new URLSearchParams();
    if (this.paramObject.UserID) {
      params.append('userID', this.paramObject.UserID.toString());
    }
    this.paramObject.SchemeID = this.selectedSchemeID;
    this.paramObject.TSPID = this.selectedTSPID;
    this.paramObject.ClassID = this.selectedClassID;


    // Dynamically append parameters to the URL
    Object.keys(this.paramObject).forEach(key => {
      if (this.paramObject[key]) {
        params.append(key, this.paramObject[key].toString());
      }
    });

    try {
      const IPTrainees: any = await this.ComSrv.getJSON(
        `api/IPDocsVerification/GetIPTrainees?${params.toString()}`
      ).toPromise();
      if (IPTrainees && IPTrainees.length > 0) {
        this.LoadMatTable(IPTrainees); // Load the fetched data into the table
        this.noRecords = false;
      } else {
        this.ComSrv.ShowWarning('No records found', 'Close');
        this.noRecords = true;
      }
    } catch (error) {
      console.error('Error fetching device registration data:', error);
      this.ComSrv.ShowError('Error fetching data', 'error', 5000);
      this.noRecords = true;
    }
  }

  GetParamString(SPName: string, paramObject: any) {
    let ParamString = SPName;
    for (const key in paramObject) {
      if (Object.hasOwnProperty.call(paramObject, key)) {
        ParamString += `/${key}=${paramObject[key]}`;
      }
    }
    return ParamString;
  }

  PageTitle(): void {
    this.ComSrv.setTitle(this.AcitveRoute.snapshot.data.title);
    this.SpacerTitle = this.AcitveRoute.snapshot.data.title;
  }

  openVisaStampingDialog(row: any): void {
    const dialogRef = this.dialog.open(VisaStampingDocDialogComponent, {
      width: '1100px',
      data: {
        traineeID: row.TraineeID,
        traineeName: row.TraineeName,
        traineeCode: row.TraineeCode,
        tspName: row.tspName,
        tsp: row.TSP,
        tspID: row.TSPID,
        classCode: row.ClassCode,
        VisaStampingStauts: row.VisaStampingApprovalStatus
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      this.GetIPTrainees();
      console.log('Dialog closed', result);
    });
  }

  fetchDocuments(traineeID: number) {
    this.ComSrv.getJSON(`api/IPDocsVerification/GetVisaStampingDocs/${traineeID}`).subscribe({
      next: (response: any) => {
        this.fetchedDocuments = response;
      },
      error: (error) => {
        console.error("Error fetching documents", error);
      },
    });
  }

  openApprovalDialogue(row: any): void {
    let processKey = EnumApprovalProcess.IPVS;
    this.ComSrv.getJSON(`api/IPDocsVerification/GetVisaStampingDocs/${row.TraineeID}`).subscribe({
      next: (response: any) => {
        this.fetchedDocuments = response;
        if (this.fetchedDocuments.length > 0) {
          this.dialogue.openApprovalDialogue(processKey, this.fetchedDocuments[0].VisaStampingTraineeDocumentsID)
            .subscribe(result => {
              console.log(result);
              // location.reload();
            });
        } else {
          console.warn("No documents found.");
        }
      },
      error: (error) => {
        console.error("Error fetching documents", error);
      },
    });
  }
}