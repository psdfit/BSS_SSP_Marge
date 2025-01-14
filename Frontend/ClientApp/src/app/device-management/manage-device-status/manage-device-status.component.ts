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
import { DeviceStatusUpdateDialogComponent } from "../device-status-update-dialog/device-status-update-dialog.component";
import { SearchFilter } from "src/app/shared/Interfaces";
import { EnumUserLevel } from "src/app/shared/Enumerations";
@Component({
  selector: 'app-manage-device-status',
  templateUrl: './manage-device-status.component.html',
  styleUrls: ['./manage-device-status.component.scss']
})
export class ManageDeviceStatusComponent implements OnInit {
  matSelectArray: MatSelect[] = [];
  @ViewChild('Applicability') Applicability: MatSelect;
  SelectedAll_Applicability: string;
  @ViewChild('Province') Province: MatSelect;
  SelectedAll_Province: string;
  @ViewChild('Cluster') Cluster: MatSelect;
  SelectedAll_Cluster: string;
  @ViewChild('District') District: MatSelect;
  SelectedAll_District: string;
  @ViewChild('activePaginator') activePaginator: MatPaginator;
  @ViewChild('inactivePaginator') inactivePaginator: MatPaginator;
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
  TraineeSupportItems: any[];
  PlaningType: any[];
  GetDataObject: any = {}
  SpacerTitle: string;
  SearchCtr = new FormControl('');
  PSearchCtr = new FormControl('');
  CSearchCtr = new FormControl('');
  DSearchCtr = new FormControl('');
  TSearchCtr = new FormControl('');
  BSearchCtr = new FormControl('');
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
  SaleGender: string = "Sales Tax Evidence"
  enumUserLevel = EnumUserLevel;
  noRecords: boolean;

  SearchTSP = new FormControl('');
  SearchCls = new FormControl('');
  SearchSch = new FormControl('');
  schemeFilter = new FormControl(0);
  tspFilter = new FormControl(0);
  classFilter = new FormControl(0);

  selectedSchemeID: number = 0;
  selectedTSPID: number = 0;
  selectedClassID: number = 0;


  filters: SearchFilter = { SchemeID: 0, TSPID: 0, ClassID: 0, TraineeID: 0, OID: this.ComSrv.OID.value, SelectedColumns: [] };


  schemeArray: any;
  tspDetailArray: any;
  classesArray: any;

  activeDevicesData: MatTableDataSource<any>;
  inactiveDevicesData: MatTableDataSource<any>;

  ngOnInit(): void {
    this.currentUser = this.ComSrv.getUserDetails();
    console.log(this.currentUser)
    this.TablesData = new MatTableDataSource([]);
    this.activeDevicesData = new MatTableDataSource([]);
    this.inactiveDevicesData = new MatTableDataSource([]);
    this.PageTitle();
    this.InitDeviceRegistrationForm()
    this.GetDeviceRegistration()


    this.schemeFilter.valueChanges.subscribe(value => {
      if (value == '0') {
        this.selectedSchemeID = null
      } else {
        this.selectedSchemeID = value;
      }
      this.GetDeviceRegistration();
      this.getTSPDetailByScheme(value);

    });
    // this.tspFilter.valueChanges.subscribe(value => { this.getClassesByTsp(value) });
    this.tspFilter.valueChanges.subscribe(value => {
      if (value == '0') {
        this.selectedTSPID = null
      } else {
        this.selectedTSPID = value;
      }
      this.getClassesByTsp(value);
      this.GetDeviceRegistration();
    });

    this.classFilter.valueChanges.subscribe(value => {
      if (value == '0') {
        this.selectedClassID = null
      } else {
        this.selectedClassID = value;
      }
      this.GetDeviceRegistration();
    });
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

  // fetchTSPLocationsByClass(classId: number): void {
  //   this.ComSrv.getJSON(`api/DeviceManagement/GetTSPDetailsByClassID?ClassID=${classId}`)
  //     .subscribe(
  //       (locations: string[]) => {
  //         this.TSPLocations = locations; // Populate TSP locations
  //       },
  //       error => {
  //         console.error('Error fetching TSP locations:', error);
  //       }
  //     );
  // }


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
      this.TableColumns = ['Action', 'Sr#', ...Object.keys(tableData[0]).filter(key => !key.includes('ID') && !excludeColumnArray.includes(key))];
      const activeDevices = tableData.filter(device => device.DeviceStatus === true);
      const inactiveDevices = tableData.filter(device => device.DeviceStatus === false);
      // Set the filtered data
      this.activeDevicesData = new MatTableDataSource(activeDevices);

      this.inactiveDevicesData = new MatTableDataSource(inactiveDevices);
      this.TablesData = new MatTableDataSource(tableData);

      this.activeDevicesData.paginator = this.activePaginator;
      this.activeDevicesData.sort = this.sort;

      this.inactiveDevicesData.paginator = this.inactivePaginator;
      this.inactiveDevicesData.sort = this.sort;
    }
  }

  onTabChange(index: number) {
    if (index === 0) {
      // Active Devices Tab
      this.activeDevicesData.paginator = this.activePaginator;
      this.activeDevicesData.sort = this.sort;
    } else if (index === 1) {
      // Inactive Devices Tab
      this.inactiveDevicesData.paginator = this.inactivePaginator;
      this.inactiveDevicesData.sort = this.sort;
    }
  }

  EmptyCtrl() {
    this.PSearchCtr.setValue('');
    this.CSearchCtr.setValue('');
    this.DSearchCtr.setValue('');
    this.BSearchCtr.setValue('');
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

    this.paramObject.SchemeID = this.selectedSchemeID;
    this.paramObject.TSPID = this.selectedTSPID;
    this.paramObject.ClassID = this.selectedClassID;

    const params = new URLSearchParams();

    // Dynamically append parameters to the URL
    Object.keys(this.paramObject).forEach(key => {
      if (this.paramObject[key]) {
        params.append(key, this.paramObject[key].toString());
      }
    });

    try {
      const deviceData: any = await this.ComSrv.getJSON(
        `api/DeviceManagement/GetDeviceRegistration?${params.toString()}`
      ).toPromise();
      console.log(deviceData, 'Fetched Device Data');
      if (deviceData && deviceData.length > 0) {
        this.LoadMatTable(deviceData); // Load the fetched data into the table
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

    // const dialogRef = this.Dialog.open(BiometricAttendanceDialogComponent, {
    const dialogRef = this.Dialog.open(DeviceStatusUpdateDialogComponent, {
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
    this.matSelectArray = [this.Applicability, this.Province, this.Cluster, this.District];
    if (this.tabGroup) {
      // Handle tab change events
      this.tabGroup.selectedTabChange.subscribe((event) => {
        this.onTabChange(event.index);
      });
    }
    // Set paginator and sort for active devices on initial load
    this.activeDevicesData.paginator = this.activePaginator;
    this.activeDevicesData.sort = this.sort;
    this.ComSrv.OID.subscribe(OID => {
      this.schemeFilter.setValue(0);
      this.tspFilter.setValue(0);
      this.classFilter.setValue(0);
      this.filters.OID = OID;
      this.getSchemesData();
    });
  }
}
