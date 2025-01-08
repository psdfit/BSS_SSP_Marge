import { Component, OnInit, ViewChild } from "@angular/core";
import { CommonSrvService } from "../../common-srv.service";
import { ActivatedRoute } from "@angular/router";
import { MatTableDataSource } from "@angular/material/table";
import { MatPaginator } from "@angular/material/paginator";
import { MatSort } from "@angular/material/sort";
import { MatTabGroup } from "@angular/material/tabs";
import { FormBuilder, FormControl, FormGroup, Validators } from "@angular/forms";
import { MatOption } from "@angular/material/core";
import { MatSelect } from "@angular/material/select";
import { MatDialog } from "@angular/material/dialog";
import { BiometricEnrollmentDialogComponent } from "../biometric-enrollment-dialog/biometric-enrollment-dialog.component";
import { EnumUserLevel } from "src/app/shared/Enumerations";
import { SearchFilter, ExportExcel } from '../../shared/Interfaces';

@Component({
  selector: 'app-device-registration',
  templateUrl: './device-registration.component.html',
  styleUrls: ['./device-registration.component.scss']
})
export class DeviceRegistrationComponent implements OnInit {
  matSelectArray: MatSelect[] = [];
  // @ViewChild('Applicability') Applicability: MatSelect;
  // SelectedAll_Applicability: string;
  // @ViewChild('Province') Province: MatSelect;
  // SelectedAll_Province: string;
  // @ViewChild('Cluster') Cluster: MatSelect;
  // SelectedAll_Cluster: string;
  // @ViewChild('District') District: MatSelect;
  // SelectedAll_District: string;
  currentUser: any = {}
  DeviceRegistration: any[];
  SelectAll(event: any, dropDownNo, controlName, formGroup) {
    const matSelect = this.matSelectArray[(dropDownNo - 1)];
    if (event.checked) {
      matSelect.options.forEach((item: MatOption) => item.select());
      if (this[formGroup].get(controlName).value) {
        const uniqueArray = Array.from(new Set(this[formGroup].get(controlName).value));
        this[formGroup].get(controlName).setValue(uniqueArray)
      }
    } else {
      matSelect.options.forEach((item: MatOption) => item.deselect());
    }
  }
  optionClick(event, controlName) {
    this.EmptyCtrl()
    let newStatus = true;
    event.source.options.forEach((item: MatOption) => {
      if (!item.selected && !item.disabled) {
        newStatus = false;
      }
    });
    if (event.source.ngControl.name === controlName) {
      this['SelectedAll_' + controlName] = newStatus;
    } else {
      this['SelectedAll_' + controlName] = newStatus;
    }
  }
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
  GetDataObject: any = {}
  SpacerTitle: string;
  SearchCtr = new FormControl('');
  PSearchCtr = new FormControl('');
  CSearchCtr = new FormControl('');
  DSearchCtr = new FormControl('');
  TSearchCtr = new FormControl('');
  BSearchCtr = new FormControl('');
  SearchTSP = new FormControl('');
  SearchCls = new FormControl('');
  SearchSch = new FormControl('');
  schemeFilter = new FormControl(0);
  tspFilter = new FormControl(0);
  classFilter = new FormControl(0);

  TapTTitle: string = "Profile"
  Data: any = []
  TableColumns = [];
  maxDate: Date;
  SaleGender: string = "Sales Tax Evidence"
  TSPNames: string[];
  TSPLocations: any[];
  DeviceTypes: string[];

  schemeArray: any;
  tspDetailArray: any;
  classesArray: any;

  enumUserLevel = EnumUserLevel;

  filters: SearchFilter = { SchemeID: 0, TSPID: 0, ClassID: 0, TraineeID: 0, OID: this.ComSrv.OID.value, SelectedColumns: [] };

  ngOnInit(): void {
    this.currentUser = this.ComSrv.getUserDetails();
    console.log(this.currentUser)
    this.TablesData = new MatTableDataSource([]);
    this.PageTitle();
    this.InitDeviceRegistrationForm()
    this.GetDeviceRegistration()

    // Subscribe to classFilter changes
    this.classFilter.valueChanges.subscribe(classId => {
      if (classId && classId > 0) {
        this.fetchTSPLocationsByClass(classId);
      } else {
        this.TSPLocations = []; // Reset if no class is selected
      }
    });
  }

  DeviceRegistrationForm: FormGroup;
  InitDeviceRegistrationForm() {
    this.DeviceRegistrationForm = this.fb.group({
      RegistrationID: [0],
      UserID: [this.currentUser.UserID],
      Brand: ['Suprema', Validators.required],
      Model: ['BioMini Slim 2', Validators.required],
      // TSPName: ['', Validators.required],
      TSPLocation: ['', Validators.required],
      // DeviceType: ['', Validators.required],
      SerialNumber: ['', Validators.required]
    });
    this.schemeFilter.valueChanges.subscribe(value => {
      if (this.currentUser.UserLevel === this.enumUserLevel.TSP) {
        this.getDependantFilters();
      } else {
        this.getTSPDetailByScheme(value);
      }
    });
    this.tspFilter.valueChanges.subscribe(value => { this.getClassesByTsp(value) });
  }


  IsDisabled = false;
  SaveFormData() {
    this.IsDisabled = true
    if (this.DeviceRegistrationForm.valid) {
      // Prepare payload with additional IDs
      const payload = {
        ...this.DeviceRegistrationForm.value,
        SchemeID: this.schemeFilter.value,
        ClassID: this.classFilter.value,
        TSPID: this.TSPLocations[0].TSPID,
      };

      this.http.postJSON('api/DeviceManagement/Save', payload).subscribe(
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
    const excludeColumnArray: string[] = ['TspName'];
    if (tableData.length > 0) {
      this.TableColumns = ['Sr#', ...Object.keys(tableData[0]).filter(key => !key.includes('ID') && !excludeColumnArray.includes(key))];
      this.TablesData = new MatTableDataSource(tableData);
      this.TablesData.paginator = this.paginator;
      this.TablesData.sort = this.sort;
    }
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

  fetchTSPLocationsByClass(classId: number): void {
    this.ComSrv.getJSON(`api/DeviceManagement/GetTSPDetailsByClassID?ClassID=${classId}`)
      .subscribe(
        (locations: string[]) => {
          this.TSPLocations = locations; // Populate TSP locations
        },
        error => {
          console.error('Error fetching TSP locations:', error);
        }
      );
  }


  getDependantFilters() {
    if (this.currentUser.UserLevel === this.enumUserLevel.TSP) {
      this.getClassesBySchemeFilter();
    } else {
      this.getTSPDetailByScheme(this.filters.SchemeID);
    }
  }


  EmptyCtrl() {
    this.PSearchCtr.setValue('');
    this.CSearchCtr.setValue('');
    this.DSearchCtr.setValue('');
    this.BSearchCtr.setValue('');
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


  paramObject: any = {}
  ExportReportName: string = ""
  SPName: string = ""
  async GetDeviceRegistration() {
    this.SPName = "RD_DVVDeviceRegistration";
    this.paramObject = {};
    if (this.currentUser.UserLevel === this.enumUserLevel.TSP) {
      this.paramObject.UserID = this.currentUser.UserID;
    }

    const params = new URLSearchParams();
    if (this.paramObject.UserID) {
      params.append('userID', this.paramObject.UserID.toString());
    }
    // Add registrationID if needed (currently not passed).

    try {
      const deviceData: any = await this.ComSrv.getJSON(
        `api/DeviceManagement/GetDeviceRegistration?${params.toString()}`
      ).toPromise();
      console.log(deviceData, 'Fetched Device Data');
      if (deviceData && deviceData.length > 0) {
        this.LoadMatTable(deviceData); // Load the fetched data into the table
      } else {
        this.ComSrv.ShowWarning('No records found', 'Close');
      }
    } catch (error) {
      console.error('Error fetching device registration data:', error);
      this.ComSrv.ShowError('Error fetching data', 'error', 5000);
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

    const dialogRef = this.Dialog.open(BiometricEnrollmentDialogComponent, {
      width: '40%',
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
  ngAfterViewInit() {
    // this.matSelectArray = [this.Applicability, this.Province, this.Cluster, this.District];
    if (this.tabGroup) {
      this.tabGroup.selectedTabChange.subscribe((event) => {
        this.TapIndex = event.index
      });
    }
    this.ComSrv.OID.subscribe(OID => {
      this.schemeFilter.setValue(0);
      this.tspFilter.setValue(0);
      this.classFilter.setValue(0);
      this.filters.OID = OID;
      this.getSchemesData();
    });
  }
}
