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
import { BiometricEnrollmentDialogComponent } from "../biometric-enrollment-dialog/biometric-enrollment-dialog.component";
import { SearchFilter } from "src/app/shared/Interfaces";
@Component({
  selector: 'app-trainee-enrollment',
  templateUrl: './trainee-enrollment.component.html',
  styleUrls: ['./trainee-enrollment.component.scss']
})
export class TraineeEnrollmentComponent implements OnInit {
  currentUser: any = {}
  DeviceRegistration: any[];
  schemeArray: any;
  tspDetailArray: any;
  classesArray: any;
  noRecords: boolean;

  constructor(
    private Dialog: MatDialog,
    private ComSrv: CommonSrvService,
    private AcitveRoute: ActivatedRoute,
    private fb: FormBuilder,
    private http: CommonSrvService,
  ) { }
  TablesData: MatTableDataSource<any>;
  @ViewChild("tabGroup") tabGroup: MatTabGroup;
  @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: false }) sort: MatSort;
  CountZeroToHun = [];
  TapIndex: any;
  readOnly = true
  PreadOnly = true
  CreadOnly = true
  DreadOnly = true
  required = false
  error: any;
  displayedColumns: string[] = []
  SelectionMethods: any[];
  TraineeSupportItems: any[];
  PlaningType: any[];
  GetDataObject: any = {}
  SpacerTitle: string;
  SearchCls = new FormControl('');
  SearchSch = new FormControl('');
  SearchTSP = new FormControl('');
  schemeFilter = new FormControl(0);
  tspFilter = new FormControl(0);
  classFilter = new FormControl(0);


  TapTTitle: string = "Profile"
  Data: any = []
  Gender: any = []
  GenderData: any = []
  ProvinceData: any = []
  FinancialYearData: any = []
  ProgramTypeData: any = []
  FundingSourceData: any = []
  EducationData: any = []
  ApplicabilityData: any = []
  PaymentStructureData: any = []
  TraineeSupportItemsData: any = []
  ClusterData: any = []
  DistrictData: any = []
  TehsilData: any = []
  TableColumns = [];
  maxDate: Date;
  enumUserLevel = EnumUserLevel;

  filters: SearchFilter = { SchemeID: 0, TSPID: 0, ClassID: 0, TraineeID: 0, OID: this.ComSrv.OID.value, SelectedColumns: [] };

  SaleGender: string = "Sales Tax Evidence"
  ngOnInit(): void {
    this.currentUser = this.ComSrv.getUserDetails();
    console.log(this.currentUser);
    this.TablesData = new MatTableDataSource([]);
    this.InitDeviceRegistrationForm();
    this.GetDeviceRegistration();
    this.getSchemesData(); // Fetch schemes on component load
    this.PageTitle();
    // Update class dropdown based on selected scheme
    this.schemeFilter.valueChanges.subscribe(value => {
      if (this.currentUser.UserLevel === this.enumUserLevel.TSP) {
        this.getDependantFilters();
        this.GetDeviceRegistration();
      } else {
        this.getTSPDetailByScheme(value);
        this.GetDeviceRegistration();
      }
    });

    this.classFilter.valueChanges.subscribe(value => {
      this.GetDeviceRegistration();
    });


    // Optionally update tspFilter based on user level if needed
    if (this.currentUser.UserLevel !== this.enumUserLevel.TSP) {
      this.getTspDetails();
    }
  }

  DeviceRegistrationForm: FormGroup;
  InitDeviceRegistrationForm() {
    this.DeviceRegistrationForm = this.fb.group({
      RegistrationID: [0],
      UserID: [this.currentUser.UserID],
      Brand: ['', Validators.required],
      Model: ['', Validators.required],
      SerialNumber: ['', Validators.required],
    });
  }

  IsDisabled = false;


  SaveFormData() {
    this.IsDisabled = true
    if (this.DeviceRegistrationForm.valid) {
      this.http.postJSON('api/DeviceManagement/Save', this.DeviceRegistrationForm.getRawValue()).subscribe(
        (response: any[]) => {
          this.DeviceRegistrationForm.reset()
          this.InitDeviceRegistrationForm()
          this.GetDeviceRegistration()
          this.ComSrv.openSnackBar("Saved data");
          this.IsDisabled = false
        },
        (error) => {
          this.error = error.error;
          this.http.ShowError(error.error, 'error', 5000);
        });
    } else {
      this.ComSrv.ShowError("All fields marked with * are required. Please complete these fields before submitting the form.");
    }
  }

  activationRequest(row: any) {
    console.log(row)
    this.OpenDialogue(row, 'Activate')
  }

  deActivationRequest(row: any) {
    console.log(row)
    this.OpenDialogue(row, 'DeActivate')

  }

  FinalSubmit: boolean = false;
  UpdateRecord(row: any) {
    this.tabGroup.selectedIndex = 0;
    this.DeviceRegistrationForm.patchValue({
      ...row,
      ApplicabilityID: row.ApplicabilityIDs.split(',').map(Number),
      SelectionMethodID: row.SelectionMethodIDs,
      ProvinceID: row.ProvinceIDs.split(',').map(Number),
      ClusterID: row.ClusterIDs.split(',').map(Number),
      DistrictID: row.DistrictIDs.split(',').map(Number),
    });
    this.FinalSubmit = row.IsSubmitted
    if (this.FinalSubmit) {
      this.DeviceRegistrationForm.disable()
    } else {
      this.DeviceRegistrationForm.enable()
    }
  }
  ResetFrom() {
    if (confirm('do you want to reset form data')) {
      this.DeviceRegistrationForm.reset
    }
  }
  
  LoadMatTable(tableData: any[]) {
    const excludeColumnArray: string[] = [];
    if (tableData.length > 0) {
      // Set table columns and data source
      this.TableColumns = ['Sr#', ...Object.keys(tableData[0]).filter(key => !key.includes('ID') && !excludeColumnArray.includes(key))];
      this.TablesData = new MatTableDataSource(tableData);
      this.TablesData.paginator = this.paginator;
      this.TablesData.sort = this.sort;
    } else {
      // Clear the table when no data
      this.TablesData = new MatTableDataSource([]);
    }
  }
  

  EmptyCtrl() {
    this.SearchCls.setValue('');
    this.SearchTSP.setValue('');
    this.SearchSch.setValue('');
  }
  ShowPreview(fileName: string) {
    this.ComSrv.PreviewDocument(fileName)
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
    this.ComSrv.ExcelExporWithForm(
      this.TablesData.filteredData,
      this.SpacerTitle
    );
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
    this.ComSrv.getJSON(`api/TSRLiveData/GetSchemesForGSR?OID=${this.ComSrv.OID.value}`)
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
    if (this.currentUser.UserLevel === this.enumUserLevel.TSP) {
      this.getClassesBySchemeFilter();
    } else {
      this.getTSPDetailByScheme(this.filters.SchemeID);
    }
  }

  paramObject: any = {}
  ExportReportName: string = ""
  SPName: string = ""

  async GetDeviceRegistration() {
    this.SPName = "RD_DVVDeviceRegistration";
    this.paramObject = {
      UserID: this.currentUser.UserID,
      SchemeID: this.schemeFilter.value || 0,
      ClassID: this.classFilter.value || 0,
    };
    this.DeviceRegistration = [];

    try {
      this.IsDisabled = true; // Disable UI elements during API call
      const endpoint = 'api/DeviceManagement/GetBiometricAttendanceOnRollTrainees'; // Replace with your API endpoint
      const params = this.paramObject;

      const response: any = await this.ComSrv.postJSON(endpoint, params).toPromise();

      if (response && response.length > 0) {
        this.DeviceRegistration = response;

        const draftTrainee = this.DeviceRegistration.filter(x => x.BiometricEnrollment == "Pending");
        if (draftTrainee.length > 0) {
          this.LoadMatTable(draftTrainee);
          this.noRecords = false;
        } else {
          this.ComSrv.ShowWarning('No records found.', 'Close');
          this.noRecords = true;
        }
      } else {
        this.ComSrv.ShowWarning('No Trainees available for Enrollment.', 'Close');
        this.noRecords = true;
      }
    } catch (error) {
      this.ComSrv.ShowError('Failed to fetch device data. Please try again later.', 'error', 5000);
      console.error('API call error:', error);
      this.noRecords = true; // Ensure fallback in case of error
      this.LoadMatTable([]); // Empty table for errors
    } finally {
      this.IsDisabled = false; // Re-enable UI elements
    }
  }

  async FetchData(SPName: string, paramObject: any) {
    try {
      const Param = this.GetParamString(SPName, paramObject);
      const data: any = await this.ComSrv.postJSON('api/BSSReports/FetchReport', Param).toPromise();
      if (data.length > 0) {
        return data;
      } else {
        if (SPName != 'RD_SSPTSPAssociationSubmission') {
          this.ComSrv.ShowWarning(' No Record Found', 'Close');
        }
      }
    } catch (error) {
      this.error = error;
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

  OpenDialogue(row, DeviceStatus) {
    const data = [row, DeviceStatus];
    // const dialogRef = this.Dialog.open(BiometricEnrollmentDialogComponent, {
    const dialogRef = this.Dialog.open(BiometricEnrollmentDialogComponent, {
      width: '50%',
      data: data,
      disableClose: true,
    });

    dialogRef.afterClosed().subscribe(result => {
      this.GetDeviceRegistration()
      if (result === true) {
      }
    });
  }

  getErrorMessage(errorKey: string, errorValue: any): string {
    const errorMessages = {
      required: 'This field is required.',
      minlength: `This field must be at least ${errorValue.requiredLength - 1} characters long.`,
      maxlength: `This field's text exceeds the specified maximum length.  (maxLength: ${errorValue.requiredLength} characters)`,
      email: 'Invalid email address.',
      pattern: 'This field is only required text',
      customError: errorValue
    };
    return errorMessages[errorKey];
  }

  PageTitle(): void {
    this.ComSrv.setTitle(this.AcitveRoute.snapshot.data.title);
    this.SpacerTitle = this.AcitveRoute.snapshot.data.title;
  }
}