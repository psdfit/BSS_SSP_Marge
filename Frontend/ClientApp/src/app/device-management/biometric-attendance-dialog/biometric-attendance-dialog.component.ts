import { Component, Inject, OnInit } from "@angular/core";
import { HttpClient, HttpHeaders, HttpParams } from "@angular/common/http";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";
import { MatDialog } from "@angular/material/dialog";
import { interval, Subscription } from "rxjs";
import { CommonSrvService } from "src/app/common-srv.service";
import { FormBuilder, FormControl, FormGroup } from "@angular/forms";
import { tr } from "date-fns/locale";
@Component({
  selector: "app-biometric-attendance-dialog",
  templateUrl: "./biometric-attendance-dialog.component.html",
  styleUrls: ["./biometric-attendance-dialog.component.scss"],
})
export class BiometricAttendanceDialogComponent implements OnInit {
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
  imgUrl =
    "https://miro.medium.com/v2/resize:fit:679/1*9EBHIOzhE1XfMYoKz1JcsQ.gif";
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
  IsMarked: boolean = false;
  templateData: any = "";
  IsVerified: boolean = false;
  Name: any;
  TraineeCode: any;
  ClassCode: any;
  SchemeName: any;
  isProcessingEnd: boolean = false
  constructor(
    private http: HttpClient,
    public dialog: MatDialog,
    public dialogRef: MatDialogRef<BiometricAttendanceDialogComponent>,
    public ComSrv: CommonSrvService,
    private fb: FormBuilder,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    dialogRef.disableClose = true;
    this.CNIC = this.data[0].TraineeCNIC;
    this.CNICIssueDate = this.data[0].CNICIssueDate;
    this.ClassID = this.data[0].ClassID;
    this.CNICNumber = "CNIC#:" + this.CNIC;
    this.CNIC = this.data[0].TraineeCNIC;
    this.Name = this.data[0].TraineeName;
    this.TraineeCode = this.data[0].TraineeCode;
    this.CNICIssueDate = this.data[0].CNICIssueDate;
    this.ClassID = this.data[0].ClassID;
    this.ClassCode = this.data[0].ClassCode;
    this.SchemeName = this.data[0].SchemeName;
    this.CNICNumber = "CNIC#:" + this.CNIC;
    console.log("dialog data");
    console.log(data);
    this.fingerprintForm = this.fb.group({
      RightIndexFinger: [false],
      RightMiddleFinger: [false],
      LeftIndexFinger: [false],
      LeftMiddleFinger: [false],
      AttendanceType: ["CheckIn"],
    });
    if (this.data[0].CheckedIn == "Not Checked In") {
      this.fingerprintForm.controls["AttendanceType"].setValue("CheckIn");
    } else {
      this.fingerprintForm.controls["AttendanceType"].setValue("CheckOut");
    }
  }
  async verifyImpression() {
    const fingerprintImpressions = [
      "RightIndexFinger",
      "RightMiddleFinger",
      "LeftIndexFinger",
      "LeftMiddleFinger",
    ];
    this.isProcessingEnd = true;
    this.ComSrv.ShowWarning("Placed your Finger on Camera.", "Close", 10000);
    for (const fingerprint of fingerprintImpressions) {
      const isVerified = await this.VerifyWithTemplate(fingerprint);
      if (isVerified) {
        console.log(`Fingerprint ${fingerprint} verified successfully.`);
        this.ComSrv.ShowWarning("Attendance is marked", "Close", 1000);
        break; // Exit the loop if verification is successful
      }
    }
    

    this.isProcessingEnd = false;

    this.ComSrv.ShowError(`Verification Failed. Please use an other registered finger.`, "Close", 5000);
    console.log("Fingerprint form value: ", this.fingerprintForm.value);
  }
  async onCheckboxChange(event:any,fingerPrintIndex: string): Promise<boolean> {
    console.timeStamp(fingerPrintIndex);
    try {
      const isVerified = await this.VerifyWithTemplate(fingerPrintIndex);
      this.isProcessingEnd = true;
      return isVerified; // Return verification status
    } catch (error) {
      console.error("Error during test for fingerprint:", fingerPrintIndex, error);
      return false;
    } finally {
      console.timeEnd(fingerPrintIndex);
    }
  }
  async VerifyWithTemplate(fingerPrintIndex: string): Promise<boolean> {
    if (!this.isExistScannerHandle()) {
      this.ComSrv.ShowError("Scanner Init First");
      return false;
    }
    this.templateData = this.data[0][fingerPrintIndex];
    const templateLength = this.templateData.length;
    const url = `${this.urlStr}/db/verifyTemplate`;
    const cb_EncryptOpt = 0;
    const txt_EncryptKey = "";
    const extractEx = 0;
    const qualityLevel = 1;
    const queryParams = new HttpParams()
      .set("dummy", Math.random().toString())
      .set("sHandle", this.deviceInfos[0]?.DeviceHandle)
      .set("id", this.pageID.toString())
      .set("tempLen", templateLength.toString())
      .set("tempData", this.templateData)
      .set("securitylevel", "4")
      .set("templateType", "2002")
      .set("encrypt", cb_EncryptOpt.toString())
      .set("encryptKey", txt_EncryptKey)
      .set("extractEx", extractEx.toString())
      .set("qualityLevel", qualityLevel.toString());
    const headers = new HttpHeaders().set("Content-Type", "application/json");
    const requestOptions = {
      params: queryParams,
      headers: headers,
    };
    try {
      const response: any = await this.http.get(url, requestOptions).toPromise();
      if (response.retVerify === "Success") {
        this.IsVerified = true;
        this.IsMarked = true;
        console.log(`Verification succeeded for ${fingerPrintIndex}`);
        await this.saveBiometricData();
        return true; // Verification successful
      } 
      // else {
      //   this.ComSrv.ShowError(
      //     `Verification Failed. Please use [${this.camelCaseToWords(
      //       fingerPrintIndex
      //     )}] index.`,
      //     "Close",
      //     7000
      //   );
      // }
      return false; // Verification failed
    } catch (error) {
      this.ComSrv.ShowError("Error verifying finger impression:");
      console.error(error);
      return false;
    }
  }
  // async verifyImpression() {
  //   const fingerprintImpressions = [
  //     "RightIndexFinger",
  //     "RightMiddleFinger",
  //     "LeftIndexFinger",
  //     "LeftMiddleFinger",
  //   ];
  //   for (const fingerprint of fingerprintImpressions) {
  //     await this.test(fingerprint);
  //   }
  //   console.log("Fingerprint form value: " + this.fingerprintForm.value);
  // }
  // async test(fingerPrintIndex) {
  //   console.timeStamp(fingerPrintIndex);
  //   await new Promise(resolve => this.VerifyWithTemplate(fingerPrintIndex));
  //    this.isProcessingEnd=true;
  //   console.timeEnd(fingerPrintIndex);
  // }
  // async VerifyWithTemplate(fingerPrintIndex: string) {
  //   if (this.isExistScannerHandle() == false) {
  //     this.ComSrv.ShowError("Scanner Init First");
  //     return;
  //   }
  //   this.templateData = this.data[0][fingerPrintIndex];
  //   var templateLength = this.templateData.length;
  //   const url = `${this.urlStr}/db/verifyTemplate`;
  //   const cb_EncryptOpt = 0;
  //   const txt_EncryptKey = "";
  //   const extractEx = 0;
  //   const qualityLevel = 1; //none
  //   const queryParams = new HttpParams()
  //     .set("dummy", Math.random().toString())
  //     .set("sHandle", this.deviceInfos[0]?.DeviceHandle)
  //     .set("id", this.pageID.toString())
  //     .set("tempLen", templateLength.toString())
  //     .set("tempData", this.templateData)
  //     .set("securitylevel", "2")
  //     .set("templateType", "2002")
  //     .set("encrypt", cb_EncryptOpt.toString())
  //     .set("encryptKey", txt_EncryptKey)
  //     .set("extractEx", extractEx.toString())
  //     .set("qualityLevel", qualityLevel.toString());
  //   const headers = new HttpHeaders().set("Content-Type", "application/json");
  //   const requestOptions = {
  //     params: queryParams,
  //     headers: headers,
  //   };
  //   try {
  //     const response: any = await this.http
  //       .get(url, requestOptions)
  //       .toPromise();
  //     this.IsVerified = response.retVerify == "Success" ? true : false;
  //     if (response.retVerify == "Success") {
  //       this.IsMarked = true;
  //       if (this.IsMarked) {
  //         this.saveBiometricData();
  //       }
  //     } else {
  //       this.ComSrv.ShowError(
  //         "Verification Failed.Please use [ " +
  //           this.camelCaseToWords(fingerPrintIndex) +
  //           " ] index",
  //         "Close",
  //         7000
  //       );
  //     }
  //   } catch (error) {
  //     this.ComSrv.ShowError("Error verifying finger impression:");
  //   }
  // }
  ngOnInit() {
    this.disableAllCheckboxes();
    this.InitPage();
  }
  async InitPage() {
    try {
      this.pageID = Math.random();
      const url = `${this.urlStr}/api/createSessionID`;
      const params = new HttpParams().set("dummy", Math.random().toString());
      const headers = new HttpHeaders({
        "Content-Type": "application/json; charset=utf-8",
      });
      const msg: any = await this.http
        .get(url, { headers, params })
        .toPromise();
      if (msg && msg.sessionId) {
        const expires = new Date(Date.now() + 60 * 60 * 1000);
        document.cookie = `username=${msg.sessionId
          }; expires=${expires.toUTCString()}`;
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
      this.ComSrv.ShowError(
        "Please start Suprema device and web-BioMini Agent ",
        "Close",
        7000
      );
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
      .set("templateType", "2002")
      .set("fakeLevel", "0")
      .set("detectFakeAdvancedMode", "0");
    const headers = new HttpHeaders().set("Content-Type", "application/json");
    const requestOptions = { params, headers };
    try {
      const response: any = this.http.get(url, requestOptions).toPromise();
      console.log("set parameters response");
      console.log(response);
    } catch (error) {
      console.error("Error setting parameters:", error);
    }
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
    const traineeData = {
      TraineeID: this.data[0].TraineeID,
      ClassID: this.data[0].ClassID,
      AttendanceType: this.data[0].CheckedIn == "Not Checked In" ? "CheckIn" : "CheckOut",
      FingerImpression: this.templateData,
    };
    console.log("trainee biometric attendance on save");
    console.log(traineeData);
    try {
      this.ComSrv.postJSON(
        "api/TraineeProfile/SaveBiometricAttendance",
        traineeData
      ).subscribe((response: any) => {
        if (response) {
          this.IsMarked = false;
          this.ComSrv.openSnackBar(
            "Attendance marked Successfully",
            "Close",
            5000
          );
          this.statusLoopSubscription.unsubscribe();
          setTimeout(() => {
            this.closeDialog();
          }, 500);
        } else {
          this.ComSrv.ShowError("Error Saving Biometric Data");
        }
      });
    } catch (error) {
      this.ComSrv.ShowError("Error Saving Biometric Data");
    }
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
  async CaptureSingle(fingerPrintIndex: string) {
    if (!this.isExistScannerHandle()) {
      return;
    }
    this.imgUrl =
      "https://miro.medium.com/v2/resize:fit:679/1*9EBHIOzhE1XfMYoKz1JcsQ.gif";
    this.gIsCaptureEnd = false;
    this.printFlag = false;
    clearTimeout(this.pLoopflag);
    clearTimeout(this.aLoopflag);
    this.gPreviewFaileCount = 0;
    this.printLfdFlag = false;
    this.ComSrv.ShowError("Placed your Finger on Camera", "Close", 5000);
    const delayVal = 30000;
    try {
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
      const response: any = await this.http
        .get(url, requestOptions)
        .toPromise();
      if (response.retValue == 0) {
        // Successful response, proceed to next steps
        this.gGetLfdValueFlag = false;
        await this.SingleImageLoop(fingerPrintIndex);
      } else {
        // Handle other unexpected cases (you may need to log or show a general error message)
        this.fingerprintForm.controls[fingerPrintIndex].setValue(false);
        this.ComSrv.ShowWarning(
          "Fingerprint Capture Failed. Please try again",
          "Close",
          5000
        );
        return;
      }
    } catch (error) {
      this.ComSrv.ShowError("CaptureSingle HTTP request error:", error);
    }
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
          this.imgUrl = `${this.urlStr
            }/img/CaptureImg.bmp?dummy=${Math.random()}${sessionData}`;
          if (fingerPrintIndex && this.TemplateImgData && this.imgUrl) {
            // Check if an object with the same `fingerPrintIndex` exists
            const existingIndex = this.traineeBiometricData.findIndex(
              (item) => item.fingerPrintIndex === fingerPrintIndex
            );
            if (existingIndex !== -1) {
              // If it exists, replace the object at the found index
              this.traineeBiometricData[existingIndex] = {
                fingerPrintIndex,
                Template: this.TemplateImgData,
                ImgUrl: this.imgUrl,
              };
            } else {
              // If it doesn't exist, push the new object into the array
              this.traineeBiometricData.push({
                fingerPrintIndex,
                Template: this.TemplateImgData,
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
