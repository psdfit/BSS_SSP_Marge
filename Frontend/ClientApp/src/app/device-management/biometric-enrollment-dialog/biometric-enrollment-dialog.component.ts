import { Component, Inject, OnInit } from "@angular/core";
import { HttpClient, HttpHeaders, HttpParams } from "@angular/common/http";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";
import { MatDialog } from "@angular/material/dialog";
import { interval, Subscription } from "rxjs";
import { CommonSrvService } from "src/app/common-srv.service";
import { FormBuilder, FormControl, FormGroup } from "@angular/forms";
@Component({
  selector: "app-biometric-enrollment-dialog",
  templateUrl: "./biometric-enrollment-dialog.component.html",
  styleUrls: ["./biometric-enrollment-dialog.component.scss"],
})
export class BiometricEnrollmentDialogComponent implements OnInit {
  RightMiddleFinger = false;
  // RightIndexFinger = false;
  RightIndexFinger = new FormControl("");
  LeftIndexFinger = false;
  LeftMiddleFinger = false;
  Status: any[] = [];
  check = false;
  CNIC: any;
  CNICIssueDate: any;
  ClassID: any;
  CNICNumber: string;
  error: any;
  screenWidth: any;
  requestData: { Fingerprint: any; Index: number; CNIC: string };
  imgUrl = "";
  TemplateImgData: any;
  Token: any;
  urlStr = "https://localhost";
  deviceInfos: any;
  strBuffer: any;
  pageID = 0;
  flag: any;
  pLoopflag: any;
  aLoopflag: any;
  ddb_Timeout = -1;
  cb_DetectFakeAdvancedMode = 0;
  flagFingerOn: any;
  gSensorValid = "false";
  gIsCapturing = "false";
  gSensorOn = "false";
  gIsFingeOn = false;
  gPreviewFaileCount = 0;
  gLfdScore = 0;
  gIsCaptureEnd = false;
  gToastTimeout = 3000;
  printLfdFlag = false;
  printFlag = false;
  gGetLfdValueFlag = false;
  LoanId: any;
  myInterval: any;
  modalBio: any;
  IsDeviceConnected = false;
  selectedIndex: any = 0;
  selectedConnection: any;
  statusLoopSubscription: Subscription | undefined;
  ImgArray: any[] = [];
  traineeBiometricData: any[] = [];
  fingerprintForm: FormGroup;
  isProcessing = false; // Tracks whether a request is ongoing
  Name: any;
  SchemeName: any;
  ClassCode: any;
  TraineeCode: any;
  constructor(
    private http: HttpClient,
    public dialog: MatDialog,
    public dialogRef: MatDialogRef<BiometricEnrollmentDialogComponent>,
    public ComSrv: CommonSrvService,
    private fb: FormBuilder,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    dialogRef.disableClose = true;
    this.CNIC = this.data[0].TraineeCNIC;
    this.Name = this.data[0].TraineeName;
    this.TraineeCode = this.data[0].TraineeCode;
    this.CNICIssueDate = this.data[0].CNICIssueDate;
    this.ClassID = this.data[0].ClassID;
    this.ClassCode = this.data[0].ClassCode;
    this.SchemeName = this.data[0].SchemeName
    this.CNICNumber = "CNIC#:" + this.CNIC;
    console.log("dialog data");
    console.log(data);
    this.fingerprintForm = this.fb.group({
      RightIndexFinger: [false],
      RightMiddleFinger: [false],
      LeftIndexFinger: [false],
      LeftMiddleFinger: [false],
    });
  }
  ngOnInit() {
    this.disableAllCheckboxes();
    this.Init()
    // if(this.ComSrv.IsDeviceConnected==false)
    
  }



   async InitPage() {
    try {
      this.pageID = Math.random();
      const url = `${this.urlStr}/api/createSessionID`;
      const params = new HttpParams().set("dummy", Math.random().toString());
      const headers = new HttpHeaders({"Content-Type": "application/json; charset=utf-8", });
      const msg: any = await this.http.get(url, { headers, params }).toPromise();
      if (msg && msg.sessionId) {
        const expires = new Date(Date.now() + 60 * 60 * 1000);
        document.cookie = `username=${msg.sessionId}; expires=${expires.toUTCString()}`;
        this.Init();
      }

    } catch (error) {
      this.CheckDeviceConnection("Down");
    }
  }


   async Init() {
     try {
    const url = `${this.urlStr}/api/initDevice`;
    const params = new HttpParams().set("dummy", Math.random().toString());
    const headers = new HttpHeaders({ "Content-Type": "application/json" });
    const requestOptions = {
      headers,
      withCredentials: true,
      crossDomain: true,
    };
      const msg: any = await this.http
        .get(`${url}?dummy=${params}`, requestOptions)
        .toPromise();
      if (msg.retValue == 0) {
        this.CheckDeviceConnection("Up");
        if (msg.ScannerInfos) {
          this.deviceInfos = msg.ScannerInfos;
          this.AddScannerList(this.deviceInfos);
        }
        this.enableAllCheckboxes();
        this.CheckStatusLoop();
        this.SendParameter();
      } else {
        this.CheckDeviceConnection("Down");
      }
    } catch (error) {
      this.CheckDeviceConnection("Down");
    }
  }


  

  CheckDeviceConnection(connection: string) {
    
    this.IsDeviceConnected = connection === "Up";
    console.log("Device Connection:" + connection);
    if (connection === "Down") {
      this.ComSrv.ShowError("Please plug suprema device and start web-BioMini Agent ", "Close", 5000);
      this.closeDialog();
    }
  }


   SendParameter() {
    if (!this.isExistScannerHandle()) {
      this.ComSrv.ShowError("Scanner Init First");
      return;
    }
    const url = `${this.urlStr}/api/setParameters`;
    const params = new HttpParams()
      .set("dummy", Math.random().toString())
      .set("sHandle", this.deviceInfos[0]?.DeviceHandle)
      .set("brightness", "100")
      .set("fastmode", "1")
      .set("securitylevel", "4")
      .set("sensitivity", "7")
      .set("timeout", "3")
      .set("templateType","2002")
      .set("fakeLevel", "0")
      .set("detectFakeAdvancedMode", "0");
    const headers = new HttpHeaders().set("Content-Type", "application/json");
    const requestOptions = { params, headers };
    try {
      const response: any = this.http
        .get(url, requestOptions)
        .toPromise();
      console.log("set parameters response");
      console.log(response);
    } catch (error) {
      console.error("Error setting parameters:", error);
    }
  }

  async CaptureSingle(fingerPrintIndex: string) {
    try {
    if (!this.isExistScannerHandle()) {
      return;
    }
    this.imgUrl ="https://miro.medium.com/v2/resize:fit:679/1*9EBHIOzhE1XfMYoKz1JcsQ.gif";
    this.gIsCaptureEnd = false;
    this.printFlag = false;
    clearTimeout(this.pLoopflag);
    clearTimeout(this.aLoopflag);
    this.gPreviewFaileCount = 0;
    this.printLfdFlag = false;
    this.ComSrv.ShowError("Placed your Finger on Camera", "Close", 3000);
    const delayVal = 30000;
      const url = `${this.urlStr}/api/captureSingle`;
      const queryParams = new HttpParams()
        .set("dummy", Math.random().toString())
        .set("sHandle", this.deviceInfos[0]?.DeviceHandle)
        .set("id", this.pageID.toString())
        .set("resetTimer", delayVal.toString());
      const headers = new HttpHeaders().set("Content-Type", "application/json");
      const requestOptions = {
        params: queryParams,
        headers,
        withCredentials: true,
      };
      const response: any = await this.http.get(url, requestOptions).toPromise();
        debugger;
      if (response.retValue == 0) {
        // Successful response, proceed to next steps
        this.gGetLfdValueFlag = false;
         this.SingleImageLoop(fingerPrintIndex);

         
      } else {
        // Handle other unexpected cases (you may need to log or show a general error message)
        this.fingerprintForm.controls[fingerPrintIndex].setValue(false);
        this.ComSrv.ShowWarning("Fingerprint Capture Failed. Please try again","Close",5000);
        return;
      }
    } catch (error) {
      this.fingerprintForm.controls[fingerPrintIndex].setValue(false);
      this.ComSrv.ShowError("CaptureSingle HTTP request error:", error);
    }
  }


  async onCheckboxChange(event: any, fingerPrintIndex: string) {
    if (this.isProcessing) {
      return; // Prevent further interaction while processing
    }
    if (event.checked) {
      this.disableAllCheckboxes();
      this.isProcessing = true; // Mark processing as true
      // Simulate async capture
      await this.CaptureSingle(fingerPrintIndex);
      setTimeout(() => {
        this.isProcessing = false; // Mark processing as false
        this.enableAllCheckboxes();
      }, 1000);
    } else {
      this.traineeBiometricData = this.traineeBiometricData.filter(
        (object) => object.fingerPrintIndex !== fingerPrintIndex
      );
    }
    console.log("Selected Checkbox Data:", this.traineeBiometricData);
  }

  disableAllCheckboxes() {
    Object.keys(this.fingerprintForm.controls).forEach((key) => {
      this.fingerprintForm.controls[key].disable();
    });
  }
  enableAllCheckboxes() {
    Object.keys(this.fingerprintForm.controls).forEach((key) => {
      this.fingerprintForm.controls[key].enable();
    });
  }
  camelCaseToWords(input: string): string {
    return input.replace(/([a-z])([A-Z])/g, "$1 $2");
  }
  saveBiometricData() {
    if (this.traineeBiometricData.length < 4) {
      this.ComSrv.ShowError("Please Capture all FingerPrints");
      return;
    }
    const traineeData: { [key: string]: string } = {
      TraineeID: this.data[0].TraineeID,
      ClassID: this.data[0].ClassID,
    };
    this.traineeBiometricData.forEach((object) => {
      traineeData[object.fingerPrintIndex] = object.Template;
    });
    console.log("trainee biometric data on save");
    console.log(traineeData);
    this.ComSrv.postJSON(
      "api/TraineeProfile/SaveBiometricData",
      traineeData
    ).subscribe((response: any) => {
      if (response) {
        this.ComSrv.openSnackBar("Biometric Data Saved Successfully");
        this.closeDialog();
      } else {
        this.ComSrv.ShowError("Error Saving Biometric Data");
      }
    });
  }

 


  async AddScannerList(ScannerInfos: any[]) {
    let count = -1;
    ScannerInfos.forEach((scannerInfo) => {
      count = scannerInfo.key;
    });
    count++;
    try {
      if (ScannerInfos.length > 0) {
        const firstScannerHandle = ScannerInfos[0].DeviceHandle;
        const url = `${this.urlStr}/api/getParameters`;
        const queryParams = {
          dummy: Math.random().toString(),
          sHandle: firstScannerHandle,
        };
        const msg: any = await this.http
          .get(url, { params: queryParams })
          .toPromise();
        if (msg) {
          const devinfotxt = "";
        }
      }
    } catch (error) {
      this.CheckDeviceConnection("Down");
      this.ComSrv.ShowError(error.message, "Close", 5000);
    }
  }
  async CheckStatusLoop() {
    if (this.statusLoopSubscription && !this.statusLoopSubscription.closed) {
      return;
    }
    this.statusLoopSubscription = interval(1000).subscribe(async () => {
      try {
        const url = `${this.urlStr}/api/getScannerStatus`;
        const queryParams = {
          dummy: Math.random().toString(),
          sHandle: this.deviceInfos[0]?.DeviceHandle || "",
        };
        const msg: any = await this.http
          .get(url, { params: queryParams })
          .toPromise();
        if (msg.retValue == 0) {
          this.CheckDeviceConnection("Up");
          this.gSensorValid = msg.SensorValid;
          this.gIsCapturing = msg.IsCapturing;
          this.gSensorOn = msg.SensorOn;
          this.gIsFingeOn = msg.IsFingerOn;
        } else {
          this.CheckDeviceConnection("Down");
        }
      } catch (error) {
        this.CheckDeviceConnection("Down");
      }
    });
  }
  isExistScannerHandle() {
    return this.deviceInfos[0].DeviceHandle != 0;
  }
 
  async getCaptureEnd(fingerPrintIndex: string) {
    try {
      const url = `${this.urlStr}/api/getCaptureEnd`;
      const queryParams = new HttpParams()
        .set("dummy", Math.random().toString())
        .set("sHandle", this.deviceInfos[0]?.DeviceHandle)
        .set("id", this.pageID.toString());
      const headers = new HttpHeaders().set("Content-Type", "application/json");
      const requestOptions = { params: queryParams, headers };
      const response: any = await this.http
        .get(url, requestOptions)
        .toPromise();
      if (response) {
        this.gIsFingeOn = response.IsFingerOn;
        this.gIsCaptureEnd = response.captureEnd;
        this.gLfdScore = response.lfdScore;
      }
      if (this.gIsCaptureEnd) {
        await this.GetTemplateData(fingerPrintIndex);
      }
    } catch (error) {
      this.ComSrv.ShowError("getCaptureEnd HTTP request error:", error);
    }
  }
  async SingleImageLoop(fingerPrintIndex: string) {
    await this.getCaptureEnd(fingerPrintIndex);
    if (this.IsIEbrowser()) {
      if (this.gIsCaptureEnd) {
        this.pLoopflag = setTimeout(
          () => this.SingleImageLoop(fingerPrintIndex),
          100
        );
        this.gPreviewFaileCount = 0;
      } else if (this.gPreviewFaileCount < 60) {
        this.pLoopflag = setTimeout(
          () => this.SingleImageLoop(fingerPrintIndex),
          1000
        );
        if (this.printFlag) {
          this.gPreviewFaileCount++;
        }
      } else {
        this.gPreviewFaileCount = 0;
      }
    } else {
      if (this.gIsCaptureEnd) {
        const sessionData = `&shandle=${this.deviceInfos[0]?.DeviceHandle}&id=${this.pageID}`;
        this.imgUrl = `${this.urlStr
          }/img/CaptureImg.bmp?dummy=${Math.random()}${sessionData}`;
        if (!this.printFlag) {
          this.printFlag = true;
        }
        if (this.gLfdScore > 0 && !this.printLfdFlag) {
          this.printLfdFlag = true;
        }
        this.gPreviewFaileCount = 0;
      } else if (this.gPreviewFaileCount < 60) {
        this.pLoopflag = setTimeout(
          () => this.SingleImageLoop(fingerPrintIndex),
          1000
        );
        if (this.printFlag) {
          this.gPreviewFaileCount++;
        }
      } else {
        this.gPreviewFaileCount = 0;
      }
    }
  }
  async GetTemplateData(fingerPrintIndex: string) {
    if (!this.isExistScannerHandle()) {
      this.ComSrv.ShowError("Scanner Init First");
      return;
    }
    const url = `${this.urlStr}/api/getTemplateData`;
    const queryParams = new HttpParams()
      .set("dummy", Math.random().toString())
      .set("sHandle", this.deviceInfos[0]?.DeviceHandle)
      .set("id", this.pageID.toString())
      .set("encrypt", "0")
      .set("encryptKey", "")
      .set("extractEx", "0")
      .set("qualityLevel", "1");
    const headers = new HttpHeaders().set("Content-Type", "application/json");
    const requestOptions = { params: queryParams, headers };
    try {
      const response: any = await this.http
        .get(url, requestOptions)
        .toPromise();
      if (response.retString !== "") {
        if (response.templateBase64) {
          this.TemplateImgData = response.templateBase64;
        }
        const sessionData = `&shandle=${this.deviceInfos[0]?.DeviceHandle}&id=${this.pageID}`;
        if (this.gIsCaptureEnd) {
          this.imgUrl = `${this.urlStr}/img/CaptureImg.bmp?dummy=${Math.random()}${sessionData}`;
          // if (fingerPrintIndex && this.TemplateImgData && this.imgUrl) {
          if (fingerPrintIndex && this.TemplateImgData) {
            // Check if an object with the same `fingerPrintIndex` exists
            const existingIndex = this.traineeBiometricData.findIndex(
              (item) => item.fingerPrintIndex === fingerPrintIndex
            );
            if (existingIndex !== -1) {
              // If it exists, replace the object at the found index
              this.traineeBiometricData[existingIndex] = {
                fingerPrintIndex,
                Template: this.TemplateImgData,
                // ImgUrl: '../../../assets/fingerprint.jpg',
                ImgUrl: this.imgUrl,
              };
            } else {
              // If it doesn't exist, push the new object into the array
              this.traineeBiometricData.push({
                fingerPrintIndex,
                Template: this.TemplateImgData,
                // ImgUrl: '../../../assets/fingerprint.jpg',
                ImgUrl: this.imgUrl,
              });
            }
          } else {
            this.fingerprintForm.controls[fingerPrintIndex].setValue(false);
            this.ComSrv.ShowWarning(
              "FingerPrint Capture Failed please try again",
              "Close",
              5000
            );
          }
          console.log("trainee biometric data");
          console.log(this.traineeBiometricData);
        }
      } else {
        this.ComSrv.ShowError("No Template Data Found");
      }
    } catch (error) {
      this.ComSrv.ShowError("Error fetching image data:", error);
    }
  }
 
  IsIEbrowser(): boolean {
    const browser = window.navigator.userAgent.toLowerCase();
    return (
      browser.indexOf("msie") !== -1 ||
      (window.navigator.appName === "Netscape" &&
        window.navigator.userAgent.search("Trident") !== -1) ||
      browser.indexOf("edge") !== -1
    );
  }

  ngOnDestroy(): void {
    if (this.statusLoopSubscription) {
      this.statusLoopSubscription.unsubscribe();
    }
    
    }

  closeDialog() {
    if (this.statusLoopSubscription) {
      this.statusLoopSubscription.unsubscribe();
    }
    this.dialogRef.close();
  }
}
