import { Component, Inject, OnInit } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { FormControl } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatDialog } from '@angular/material/dialog';
import { interval, Subscription } from 'rxjs';
import { CommonSrvService } from 'src/app/common-srv.service';
@Component({
  selector: 'app-biometric-enrollment-dialog',
  templateUrl: './biometric-enrollment-dialog.component.html',
  styleUrls: ['./biometric-enrollment-dialog.component.scss']
})
export class BiometricEnrollmentDialogComponent implements OnInit {
  currentUser: any;
  Status: any = []
  check: boolean = false
  CNIC: any = this.data[0].TraineeCNIC
  CNICIssueDate: any = this.data[0].CNICIssueDate
  ClassID: any = this.data[0].ClassID
  CNICNumber: string = 'CNIC#:' + this.CNIC
  error: any;
  screenWidth: any;
  requestData: { Fingerprint: any; Index: number; CNIC: string; };
  imgUrl: string = ''
  TemplateImgData: any;
  Token: any;
  DistrictAndTehsilData: any = []
  constructor(
    private http: HttpClient,
    public dialog: MatDialog,
    public dialogRef: MatDialogRef<BiometricEnrollmentDialogComponent>,
    public ComSrv: CommonSrvService,
    @Inject(MAT_DIALOG_DATA) public data: any) {
    dialogRef.disableClose = true;
    this.CNICNumber = 'CNIC#:' + this.data[0].TraineeCNIC
  }
  fingerPrintArray: any[] = [
    { ID: "1", Value: "1-Right Thumb" },
    { ID: "2", Value: "2-Right Index Finger" },
    { ID: "3", Value: "3-Right Middle Finger" },
    { ID: "4", Value: "4-Right Ring Finger" },
    { ID: "5", Value: "5-Right Little Finger" },
    { ID: "6", Value: "6-Left Thumb" },
    { ID: "7", Value: "7-Left Index Finger" },
    { ID: "8", Value: "8-Left Middle Finger" },
    { ID: "9", Value: "9-Left Ring Finger" },
    { ID: "10", Value: "10-Left Little Finger" }
  ]
  async ngOnInit() {
    this.currentUser = this.ComSrv.getUserDetails()
    this.InitPage()
    this.Init()
    // await this.GetToken()
    // await this.GetDistrict()
  }

  urlStr = 'https://localhost';
  NadraAPIUrl = 'http://172.19.1.25:8081/nadra';
  //  BSSAPIUrl = 'http://localhost:63703/'; //for localhost
  BSSAPIUrl = 'http://172.19.1.19:51600/DVV_API/';//for test server
  // NadraAPIUrl = 'http://119.159.234.173:8081/nadra';
  deviceInfos: any;
  strBuffer: any;
  pageID = 0;
  flag: any;
  pLoopflag: any;
  aLoopflag: any;
  ddb_Timeout = -1;
  cb_DetectFakeAdvancedMode = 0;
  flagFingerOn: any;
  gSensorValid = 'false';
  gIsCapturing = 'false';
  gSensorOn = 'false';
  gIsFingeOn = false;
  gPreviewFaileCount = 0;
  gLfdScore = 0;
  gIsCaptureEnd = false;
  gToastTimeout = 3000;
  printLfdFlag = false;
  printFlag = false;
  gGetLfdValueFlag = false;
  StaffId = '<%=HttpContext.Current.Session("StaffId").toString()%>';
  APIURL = '<%= System.Configuration.ConfigurationManager.AppSettings("APIURL").ToString() %>';
  LoanId: any;
  _CNIC: any;
  _ContactNo: any;
  myInterval: any;
  modalBio: any;
  IsDeviceConnected: boolean = true
  selectedIndex: any = 0
  selectedConnection: any;
  // store session 
  async InitPage() {
    this.pageID = Math.random();
    try {
      const url = `${this.urlStr}/api/createSessionID`;
      const dummyParam = Math.random().toString();
      const params = new HttpParams().set('dummy', dummyParam);
      const headers = new HttpHeaders({
        'Content-Type': 'application/json; charset=utf-8'
      });
      const msg: any = await this.http.get(url, { headers, params }).toPromise();
      if (msg && msg.sessionId) {
        // Calculate cookie expiry time (1 hour from now)
        const expires = new Date(Date.now() + 60 * 60 * 1000);
        const cookieStr = `username=${msg.sessionId}; expires=${expires.toUTCString()}`;
        document.cookie = cookieStr;
      }
    } catch (error) {
      this.CheckDeviceConnection('Down');
      this.ComSrv.ShowError(error.message, 'Close', 5000);
    }
  }
  async Init() {
    const url = `${this.urlStr}/api/initDevice`;
    const dummyParam = Math.random().toString();
    const headers = new HttpHeaders({
      'Content-Type': 'application/json'
    });
    const requestOptions = {
      headers,
      withCredentials: true,
      crossDomain: true
    };
    try {
      const msg: any = await this.http.get(`${url}?dummy=${dummyParam}`, requestOptions).toPromise();
      // console.log(msg.retValue)
      if (msg.retValue == 0) {
        this.CheckDeviceConnection('Up');
        if (msg.ScannerInfos) {
          this.deviceInfos = msg.ScannerInfos;
          this.AddScannerList(this.deviceInfos);
        }
        // Start the status checking loop
        this.CheckStatusLoop();
      } else {
        this.CheckDeviceConnection('Down');
      }
    } catch (error) {
      this.CheckDeviceConnection('Down');
      this.ComSrv.ShowError(error.message, 'Close', 5000);
    }
  }
  CheckDeviceConnection(connection: string) {
    switch (connection) {
      case 'Up':
        this.IsDeviceConnected = true
        break;
      case 'Down':
        this.IsDeviceConnected = false
        break;
    }
    console.log("Device Connection:" + connection)
  }
  async AddScannerList(ScannerInfos: any[]) {
    let count = -1;
    ScannerInfos.forEach(scannerInfo => {
      const strBuffer = `[${scannerInfo.DeviceIndex}] ${scannerInfo.DeviceType} (${scannerInfo.ScannerName})`;
      count = scannerInfo.key;
    });
    count++;
    try {
      if (ScannerInfos.length > 0) {
        const firstScannerHandle = ScannerInfos[0].DeviceHandle;
        const url = `${this.urlStr}/api/getParameters`;
        const queryParams = {
          dummy: Math.random().toString(),
          sHandle: firstScannerHandle
        };
        const msg: any = await this.http.get(url, { params: queryParams }).toPromise();
        if (msg) {
          const devinfotxt = '';
        }
      }
    } catch (error) {
      this.CheckDeviceConnection('Down');
      this.ComSrv.ShowError(error.message, 'Close', 5000);
    }
  }
  statusLoopSubscription: Subscription | undefined;
  async CheckStatusLoop() {
    if (this.statusLoopSubscription && !this.statusLoopSubscription.closed) {
      return;
    }
    this.statusLoopSubscription = interval(1000).subscribe(async () => {
      try {
        const url = `${this.urlStr}/api/getScannerStatus`;
        const queryParams = {
          dummy: Math.random().toString(),
          sHandle: this.deviceInfos[0]?.DeviceHandle || ''
        };
        const msg: any = await this.http.get(url, { params: queryParams }).toPromise();
        if (msg.retValue == 0) {
          this.CheckDeviceConnection('Up');
          this.gSensorValid = msg.SensorValid;
          this.gIsCapturing = msg.IsCapturing;
          this.gSensorOn = msg.SensorOn;
          this.gIsFingeOn = msg.IsFingerOn;
        } else {
          this.CheckDeviceConnection('Down');
        }
      } catch (error) {
        this.CheckDeviceConnection('Down');
      }
    });
    // console.log(this.statusLoopSubscription);
  }
  isExistScannerHandle() {
    if (this.deviceInfos[0].DeviceHandle == 0) {
      return false;
    } else {
      return true;
    }
  }
  async CaptureSingle() {
    // const response = await this.NADRAverification('', 1, "33203-6898043-7".replace(/-/g, ""))
    if (!this.isExistScannerHandle()) {
      return;
    }
    if (this.selectedIndex == 0) {
      this.ComSrv.ShowError('Required Fingerprint index')
      return
    }
    // this.Init();
    // $('#Bt_GtImg').hide();
    this.imgUrl = ''
    this.gIsCaptureEnd = false;
    this.printFlag = false;
    if (this.pLoopflag !== undefined) {
      clearTimeout(this.pLoopflag);
    }
    if (this.aLoopflag !== undefined) {
      clearTimeout(this.aLoopflag);
    }
    this.gPreviewFaileCount = 0;
    this.printLfdFlag = false;
    // $('#Fpimg').attr('src', 'data:image/gif;base64,R0lGODlhAQABAIAAAAAAAP///ywAAAAAAQABAAACAUwAOw==');
    this.ComSrv.ShowError("Placed your Finger on Camera", "Close", 3000);
    const delayVal = 30000;
    try {
      const sessionCookie = document.cookie;
      const url = `${this.urlStr}/api/captureSingle`;
      const queryParams = new HttpParams()
        .set('dummy', Math.random().toString())
        .set('sHandle', this.deviceInfos[0]?.DeviceHandle)
        .set('id', this.pageID.toString())
        .set('resetTimer', delayVal.toString());
      const headers = new HttpHeaders().set('Content-Type', 'application/json');
      const requestOptions = {
        params: queryParams,
        headers: headers,
        withCredentials: true
      };
      const response: any = await this.http.get(url, requestOptions).toPromise();
      const sessionData = `&shandle=${this.deviceInfos[0]?.DeviceHandle}&id=${this.pageID}`;
      const imgUrl = `${this.urlStr}/img/CaptureImg.bmp?dummy=${Math.random()}${sessionData}`;
      if (response.retValue == 0) {
        this.gGetLfdValueFlag = false;
        await this.SingleImageLoop();
      }
    } catch (error) {
      this.ComSrv.ShowError('CaptureSingle HTTP request error:', error);
    }
  }
  async getCaptureEnd() {
    try {
      const url = `${this.urlStr}/api/getCaptureEnd`;
      const queryParams = new HttpParams()
        .set('dummy', Math.random().toString())
        .set('sHandle', this.deviceInfos[0]?.DeviceHandle)
        .set('id', this.pageID.toString())
      const headers = new HttpHeaders().set('Content-Type', 'application/json');
      const requestOptions = {
        params: queryParams,
        headers: headers
      };
      const response: any = await this.http.get(url, requestOptions).toPromise();
      if (response) {
        this.gIsFingeOn = response.IsFingerOn;
        this.gIsCaptureEnd = response.captureEnd;
        this.gLfdScore = response.lfdScore;
      }
      const sessionData = `&shandle=${this.deviceInfos[0]?.DeviceHandle}&id=${this.pageID}`;
      this.imgUrl = `${this.urlStr}/img/CaptureImg.bmp?dummy=${Math.random()}${sessionData}`;
      if (this.gIsCaptureEnd) {
        this.GetTemplateData()
      }
    } catch (error) {
      this.ComSrv.ShowError('getCaptureEnd HTTP request error:', error);
    }
  }
  GetSelectedIndex(param) {
    this.imgUrl = ''
  }
  async GetPassword(data: any) {
    const response: any = await this.ComSrv.postJSON('api/Users/GetPassword', data).toPromise()
    console.log(response.password)
    return response.password
  }
  async GetToken() {
    try {
      const userPassword = await this.GetPassword({ UserPassword: this.currentUser.UserPassword });
      const url = this.BSSAPIUrl+'api/auth/login';
      const body = {
        UserName: this.currentUser.UserName,
        UserPassword: userPassword,
        IMEI: ''
      };
      const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
      const response: any = await this.http.post(url, body, { headers }).toPromise();
      const token = response?.Data?.Token;
      if (token) {
        this.Token = token
      } else {
        this.ComSrv.ShowError('Invalid user');
      }
    } catch (error) {
      this.ComSrv.ShowError('Error occurred while getting token');
      console.error('Error in getToken:', error);
    }
  }
  async GetDistrict() {
    try {
      const url = this.BSSAPIUrl+'api/LookUp/ListDistrictsAndTehsils';
      const response: any = await this.http.get(url, { headers: { Authorization: `Bearer ${this.Token}` } }).toPromise();
      console.log(response.Data)
      if (response.Data.length != 0) {
        this.DistrictAndTehsilData = response.Data
      } else {
        this.ComSrv.ShowError('invalid user')
      }
    } catch (error) {
      this.ComSrv.ShowError(error)
    }
  }
  async verifyNadraRequest() {
    //  await this.GetToken();
    // try {
      const sessionData = `&shandle=${this.deviceInfos[0]?.DeviceHandle}&id=${this.pageID}`;
      const imgData = await this.GetImageBuffer();
      if (this.gIsCaptureEnd) {
        this.requestData = {
          "Fingerprint": imgData,
          "Index": this.selectedIndex,
          "CNIC": this.CNIC.replace(/-/g, "")
        };
        this.imgUrl = `${this.urlStr}/img/CaptureImg.bmp?dummy=${Math.random()}${sessionData}`;
      }
      const NadraData: any = await this.http.post(this.NadraAPIUrl, JSON.stringify(this.requestData)).toPromise();
      // const NadraData: any = {
      //   "StatusCode": 200,
      //   "Message": "successful",
      //   "Data": {
      //     "SESSION_ID": "5032100001759271441",
      //     "CITIZEN_NUMBER": "3320368980437",
      //     "NAME": "سمیع اللہ",
      //     "FATHER_HUSBAND_NAME": "آس محمد سلیم",
      //     "PRESENT_ADDRESS": "بھٹہ نمبر 2جامع مسجد عمر،محلہ گلشن عمر کالونی گرین ٹاؤن،لاہور",
      //     "PERMANANT_ADDRESS": "مسجد مکی والی لکی نو دوئم ڈاکخانہ لکی نو،شورکوٹ،ضلع جھنگ",
      //     "DATE_OF_BIRTH": "1998-06-01",
      //     "GENDER": "male"
      //   }
      // };
      // debugger;
      
      if (NadraData.StatusCode == 200) {
        const OtherDistrict = 29
        const OtherTehsil = 686
        const PermanentDistrictData =  this.getDistrictFromAddress(NadraData.Data.PERMANANT_ADDRESS)
        const PermanentDistrict = PermanentDistrictData != null && PermanentDistrictData.DistrictID != undefined ? PermanentDistrictData.DistrictID : OtherDistrict
        const PermanentTehsilData =  this.getTehsilFromAddressAndDistrict(PermanentDistrict, NadraData.Data.PERMANANT_ADDRESS)
        const PermanentTehsil = PermanentTehsilData != null && PermanentTehsilData.TehsilID != undefined ? PermanentTehsilData.TehsilID : OtherTehsil
        const TemporaryDistrictData =  this.getDistrictFromAddress(NadraData.Data.PRESENT_ADDRESS)
        const TemporaryDistrict = TemporaryDistrictData != null && TemporaryDistrictData.DistrictID != undefined ? TemporaryDistrictData.DistrictID : OtherDistrict
        const TemporaryTehsilData =  this.getTehsilFromAddressAndDistrict(TemporaryDistrict, NadraData.Data.PRESENT_ADDRESS)
        const TemporaryTehsil = TemporaryTehsilData != null && TemporaryTehsilData.TehsilID != undefined ? TemporaryTehsilData.TehsilID : OtherTehsil
      await  this.saveTraineeRegistration(NadraData.Data, PermanentDistrict, PermanentTehsil, TemporaryDistrict, TemporaryTehsil)
      } else {
        this.ComSrv.ShowError(NadraData.Message, 'close', 5000);
      }
    // } catch (error) {
    //   console.error(error.Message, error);
    //   this.ComSrv.ShowError(error.Message, 'close', 5000);
    // }
  }
   getDistrictFromAddress(address) {
    const matchingDistrict = this.DistrictAndTehsilData.find(d => address.includes(d.DistrictNameUrdu));
    return matchingDistrict ? matchingDistrict : null;
  }
   getTehsilFromAddressAndDistrict(district, address) {
    const matchingTehsil = this.DistrictAndTehsilData.find(d => address.includes(d.TehsilNameUrdu) && d.DistrictID == district);
    return matchingTehsil ? matchingTehsil : null;
  }
 
   async saveTraineeRegistration(responseData, PermanentDistrict, PermanentTehsil, TemporaryDistrict, TemporaryTehsil) {
    const body: any = {
      // SessionID: responseData.SESSION_ID,
      TraineeName: responseData.NAME,
      FatherName: responseData.FATHER_HUSBAND_NAME,
      TraineeCNIC: this.CNIC,
      CNICIssueDate: this.CNICIssueDate,
      DateOfBirth: responseData.DATE_OF_BIRTH,
      MobileNumber: '',
      TimeStampOfVerification: new Date(),
      Latitude: '123',
      Longitude: '123',
      BiometricData1: this.TemplateImgData,
      BiometricData2: this.TemplateImgData,
      BiometricData3: this.TemplateImgData,
      BiometricData4: this.TemplateImgData,
      ClassID: this.ClassID,
      GenderID: responseData.GENDER == 'male' ? 3 : (responseData.GENDER == 'female' ? 5 : 6),
      EducationID: 0,
      ReligionID: 0,
      PermanentResidence: responseData.PERMANANT_ADDRESS,
      PermanentDistrict: PermanentDistrict,
      PermanentTehsil: PermanentTehsil,
      TemporaryResidence: responseData.PRESENT_ADDRESS,
      TemporaryDistrict: TemporaryDistrict,
      TemporaryTehsil: TemporaryTehsil
    };
    
    try {
      const url = this.BSSAPIUrl + 'api/Trainee/Registration';
      const headers = new HttpHeaders({
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${this.Token}`
      });
      if (this.TemplateImgData === undefined || this.TemplateImgData === '') {
        this.ComSrv.ShowError('Please Scan finger again', 'close', 5000);
        return;
      } else {
        const response: any =await  this.http.post(url, body, { headers }).toPromise();
        if (response.StatusCode == 200) {
          this.ComSrv.openSnackBar(response.Message);
          this.dialogRef.close([true, body]);
          // this.closeDialog();
        } else {
          this.ComSrv.ShowError(JSON.stringify(response));
        }
      }
    } catch (error) {
      this.ComSrv.ShowError(error);
    }
  }
  
  closeDialog() {
    if (this.statusLoopSubscription) {
      this.statusLoopSubscription.unsubscribe();
    }
    this.dialogRef.close();
  }

  async GetImageBuffer() {
    if (!this.isExistScannerHandle()) {
      this.ComSrv.ShowError('Scanner Init First');
      return;
    }
    const url = `${this.urlStr}/api/getImageData`;
    const fileType = 1; // BMP type (assuming 1 represents BMP)
    const queryParams = new HttpParams()
      .set('dummy', Math.random().toString())
      .set('sHandle', this.deviceInfos[0]?.DeviceHandle)
      .set('id', this.pageID.toString())
      .set('fileType', fileType.toString());
    const headers = new HttpHeaders().set('Content-Type', 'application/json');
    const requestOptions = {
      params: queryParams,
      headers: headers
    };
    try {
      const response: any = await this.http.get(url, requestOptions).toPromise();
      if (response.imageBase64 !== '') {
        return response.imageBase64; // Return image data if it's not empty
      } else {
        return ''; // Return empty string if imageBase64 is empty
      }
    } catch (error) {
      this.ComSrv.ShowError('Error fetching image data:', error);
      return ''; // Return empty string on error
    }
  }
  IsIEbrowser(): any {
    const browser = window.navigator.userAgent.toLowerCase();
    if ((browser.indexOf('chrome') !== -1 || browser.indexOf('firefox') !== -1) && browser.indexOf('edge') === -1) {
      return 0; // Not IE
    } else {
      if (browser.indexOf('msie') !== -1 || (window.navigator.appName === 'Netscape' && window.navigator.userAgent.search('Trident') !== -1) || browser.indexOf('edge') !== -1) {
        return 1; // IE or Edge
      }
    }
    return -1;
  }
  async SingleImageLoop() {
    const sessionData = `&shandle=${this.deviceInfos[0]?.DeviceHandle}&id=${this.pageID}`;
    await this.getCaptureEnd();
    const isIE: any = this.IsIEbrowser() == 1;
    if (isIE) {
      if (this.gIsCaptureEnd) {
        const imgUrl = `${this.urlStr}/img/CaptureImg.bmp?dummy=${Math.random()}${sessionData}`;
        console.log(imgUrl)
        if (this.gLfdScore > 0 && !this.printLfdFlag) {
          this.printLfdFlag = true;
        }
        this.pLoopflag = setTimeout(() => this.SingleImageLoop(), 100);
        this.gPreviewFaileCount = 0;
      } else if (this.gPreviewFaileCount < 60) {
        this.pLoopflag = setTimeout(() => this.SingleImageLoop(), 1000);
        if (this.printFlag) {
          this.gPreviewFaileCount++;
        }
      } else {
        this.gPreviewFaileCount = 0;
        // this.resetFpimg();
        // this.hideBtGtImg();
      }
    } else {
      if (this.gIsCaptureEnd) {
        const imgUrl = `${this.urlStr}/img/CaptureImg.bmp?dummy=${Math.random()}${sessionData}`;
        // $('#Fpimg').removeAttr('src'); // Remove src attribute using native JavaScript
        // $('#Fpimg').attr('src', imgUrl); // Set new src attribute
        console.log(imgUrl)
        this.imgUrl=imgUrl
        if (!this.printFlag) {
          // AppendLog('captureSingle', 'Success');
          // $(".notify").toggleClass("active");
          // $("#notifyType").toggleClass("success");
          // setTimeout(() => {
          //   $(".notify").removeClass("active");
          //   $("#notifyType").removeClass("success");
          // }, 2000);
          // this.showBtGtImg();
          this.printFlag = true;
        }
        if (this.gLfdScore > 0 && !this.printLfdFlag) {
          // AppendLogData(`SDK LFD score: ${this.gLfdScore}`);
          this.printLfdFlag = true;
        }
        // return true;
        // this.pLoopflag = setTimeout(() => this.SingleImageLoop(), 100);
        this.gPreviewFaileCount = 0;
      } else if (this.gPreviewFaileCount < 60) {
        const ddb_Timeout = 5;
        this.pLoopflag = setTimeout(() => this.SingleImageLoop(), 1000);
        if (this.printFlag) {
          this.gPreviewFaileCount++;
        }
      } else {
        this.gPreviewFaileCount = 0;
        // this.resetFpimg();
        // this.hideBtGtImg();
        // AppendLogData('SingleImageLoop END');
      }
    }
  }

  async getSelectedRowBio(lnk: any) {
    const modalBio = document.getElementById('modalBio');
    modalBio.style.display = 'block';
    // $(".loader").hide();
    const row = lnk.parentNode.parentNode;
    const LoanId = row.cells[1].innerHTML;
    const _CNIC = row.cells[6].innerHTML.replace(/-/g, '');
    const _ContactNo = row.cells[7].innerHTML.replace(/-/g, '');
    // $('#spnCNIC').text(_CNIC);
    // Fetch remaining attempts from API
    this.http.get(this.APIURL + `MCOS/GetNADRARemainingAttempts/?CNIC=${_CNIC}`)
      .subscribe((jsonRes: any) => {
        if (jsonRes > 0) {
          // $("#spnRemainingAt").text(jsonRes);
        } else {
          // $("#spnRemainingAt").text("--");
        }
      });
    // Fetch remaining time from API
    this.http.get(this.APIURL + `MCOS/GetNADRARemainingTime/?CNIC=${_CNIC}`)
      .subscribe((jsonRes: any) => {
        if (jsonRes !== "NoTime") {
          // $("#timer").show();
          const currDate = new Date(jsonRes);
          currDate.setMinutes(currDate.getMinutes() + 10);
          const endTime = currDate;
          clearInterval(this.myInterval);
          this.myInterval = setInterval(() => { this.myTimer(endTime); }, 1000);
        }
      });
    // $('#ddlfingerIndexes').val("1");
    // $("#timer").hide();
    // $(".notify").removeClass("active failure").text("");
    //InitPage();
    this.Init();
    // $("#Bt_GtImg").hide();
    this.gPreviewFaileCount = 0;
    this.printLfdFlag = false;
    // $('#Fpimg').attr('src', 'data:image/gif;base64,R0lGODlhAQABAIAAAAAAAP///ywAAAAAAQABAAACAUwAOw==');
    return false;
  }
  myTimer(endTime: any) {
    const endTimeSeconds = Date.parse(endTime) / 1000;
    const nowSeconds = Date.now() / 1000;
    let timeLeft = endTimeSeconds - nowSeconds;
    const days = Math.floor(timeLeft / 86400);
    timeLeft -= days * 86400;
    const hours = Math.floor(timeLeft / 3600);
    timeLeft -= hours * 3600;
    const minutes = Math.floor(timeLeft / 60);
    const seconds = Math.floor(timeLeft % 60);
    const formattedHours = (hours < 10) ? `0${hours}` : `${hours}`;
    const formattedMinutes = (minutes < 10) ? `0${minutes}` : `${minutes}`;
    const formattedSeconds = (seconds < 10) ? `0${seconds}` : `${seconds}`;
    // const timeDisplay = `${formattedMinutes}<span>:</span>${formattedSeconds}`;
    // document.getElementById('time').innerHTML = timeDisplay;
    if (formattedMinutes === '00' && formattedSeconds === '00') {
      // const modalBio = document.getElementById('modalBio');
      // if (modalBio) {
      // modalBio.style.display = 'none';
      // }
    }
  }
  BSearchCtr = new FormControl('');
  emptyCtrl() {
    this.BSearchCtr.setValue('');
  }
  async SendParameter(TemplateType) {
    if (!this.isExistScannerHandle()) {
      this.ComSrv.ShowError('Scanner Init First');
      return;
    }
    const cb_DetectFakeAdvancedMode = 0;
    const tb_Sensitivity = 7;
    const tb_Brightness = 100;
    const ddb_Securitylevel = 4
    const ddb_Timeout = 5
    const ddb_TemplateType = TemplateType //2002 for ISO_19794_2,2003 for ANSI378,2001 for XPERIX
    const ddb_Fakelevel = 0
    const cb_FastMode = 0
    const url = `${this.urlStr}/api/setParameters`;
    const params = new HttpParams()
      .set('dummy', Math.random().toString())
      .set('sHandle', this.deviceInfos[0]?.DeviceHandle)
      .set('brightness', tb_Brightness.toString())
      .set('fastmode', cb_FastMode.toString())
      .set('securitylevel', ddb_Securitylevel.toString())
      .set('sensitivity', tb_Sensitivity.toString())
      .set('timeout', ddb_Timeout.toString())
      .set('templateType', ddb_TemplateType.toString())
      .set('fakeLevel', ddb_Fakelevel.toString())
      .set('detectFakeAdvancedMode', cb_DetectFakeAdvancedMode.toString());
    const header = new HttpHeaders().set('Content-Type', 'application/json');
    const requestOpt = {
      params: params,
      headers: header
    };
    try {
      const response: any = await this.http.get(url, requestOpt).toPromise();
      console.log(response.retString)
      if (response.retString !== '') {
      } else {
        return '';
      }
    } catch (error) {
      return '';
    }
  }
  async GetTemplateData() {
    if (!this.isExistScannerHandle()) {
      this.ComSrv.ShowError('Scanner Init First');
      return;
    }
    // 2001 _1. XPERIX*
    // 2002 _2. ISO_19794_2
    // 2003 _3. ANSI378
    await this.SendParameter(2002)
    const url = `${this.urlStr}/api/getTemplateData`;
    const cb_EncryptOpt = 0;
    const txt_EncryptKey = ""
    const extractEx = 0 //Use ExtractEx is Unchecked,
    const qualityLevel = 1 //none
    const queryParams = new HttpParams()
      .set('dummy', Math.random().toString())
      .set('sHandle', this.deviceInfos[0]?.DeviceHandle)
      .set('id', this.pageID.toString())
      .set('encrypt', cb_EncryptOpt.toString())
      .set('encryptKey', txt_EncryptKey)
      .set('extractEx', extractEx.toString())
      .set('qualityLevel', qualityLevel.toString());
    const headers = new HttpHeaders().set('Content-Type', 'application/json');
    const requestOptions = {
      params: queryParams,
      headers: headers
    };
    try {
   
      const response: any = await this.http.get(url, requestOptions).toPromise();
      console.log(response.retString)
      if (response.retString !== '') {
        if( response.templateBase64){
          this.TemplateImgData = response.templateBase64
        }
        console.log(response.templateBase64)
      } else {
        return ''; // Return empty string if imageBase64 is empty
      }
    } catch (error) {
      this.ComSrv.ShowError('Error fetching image data:', error);
      return ''; // Return empty string on error
    }
  }
}
