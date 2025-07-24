/* **** Aamer Rehman Malik *****/
/* ****Aamer Rehman Malik *****/
import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { ConfirmDailogComponent } from './custom-components/confirm-dailog/confirm-dailog.component';
import { MatDialogRef, MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { environment } from '../environments/environment';
import { Title, DomSanitizer } from '@angular/platform-browser';
import { Observable, BehaviorSubject } from 'rxjs';
import { Router, NavigationEnd, ActivatedRoute } from '@angular/router';
import { UserRightsModel, UsersModel, UserOrganizationModel } from './master-data/users/users.component';
import { PrintpreviewComponent } from './custom-components/printpreview/printpreview.component';
import { SnackBarComponent } from './custom-components/snack-bar/snack-bar.component';
import { filter, map } from 'rxjs/operators';
import { Key } from 'protractor';
import { AppConfigService } from './app-config.service';
import { EnumExcelReportType, EnumUserLevel } from './shared/Enumerations';
import { confirmdailogTraineeComponent } from './custom-components/confirm-dailogTrainee/confirm-dailogTrainee.component';
import * as XLSX from 'xlsx';
import { ExportExcel } from './shared/Interfaces';
import { DialogueService } from './shared/dialogue.service';
import { throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { PreviewFileComponent } from './custom-components/preview-file/preview-file.component';
export class MenuItem {
  path: string;
  title: string;
  parent: string;
  icon?: string;
}
@Injectable({
  providedIn: 'root'
})




export class CommonSrvService {


  base64ToFile(res: string, mimeType: string = 'application/pdf') {
    let byteCharacters = atob(res);

    let byteNumbers = new Array(byteCharacters.length);

    for (var i = 0; i < byteCharacters.length; i++)
      byteNumbers[i] = byteCharacters.charCodeAt(i);

    let byteArray = new Uint8Array(byteNumbers);
    let file = new Blob([byteArray], { type: 'application/pdf' });
    //return this.domSanitizer.bypassSecurityTrustResourceUrl(URL.createObjectURL(file));
    return file;
  }
  dateFilter = (d: Date | null): boolean => {
    const date = (d || new Date());
    // Prevent after current date selection .
    return date <= new Date();
  }

  // appSettings: any = (appSettings as any).default;
  appConfig: any;
  userrights: any;
  error = '';
  FormIDRemoveForTSP = environment.FormIDRemoveForTSPUser;
  FormIDRemoveForAll = environment.FormIDRemoveForAllUsers;
  currentUser: UsersModel;
  activeMenuItem$: Observable<MenuItem>;
  pageTitle = new BehaviorSubject<string>('');
  OID = new BehaviorSubject<number>(0);
  constructor(private dialogue: DialogueService, private titleService: Title, private http: HttpClient, private snackBar: MatSnackBar, private dialog: MatDialog, private route: Router, private domSanitizer: DomSanitizer, private router: Router, private appConfigService: AppConfigService) {
    this.activeMenuItem$ = this.router.events
      .pipe(
        filter(event => event instanceof NavigationEnd)
      ).pipe(
        map(_ => this.router.routerState.root))
      .pipe(map(route => {
        const active = this.lastRouteWithMenuItem(route.root);
        this.titleService.setTitle(active.title);
        return active;
      }));
    this.userrights = JSON.parse(sessionStorage.getItem(environment.RightsToken));
    this.appConfig = this.appConfigService.getAppConfig();
    // this.pageTitle.next(this.titleService.getTitle());
  }


  sharedDataObj: any = {}

  setMessage(sharedData: any) {
    this.sharedDataObj = {}
    this.sharedDataObj = sharedData
  }

  getMessage() {
    return this.sharedDataObj
  }

  PreviewDocument(fileName: string) {
    if (fileName == '' || fileName == undefined) {
      this.ShowError('There is no Attachment');
      return
    }


    const filePath = fileName;
    const updatedPath = filePath.slice(1);
    const updatedDocPath = updatedPath.replace(/[\/\\]/g, '||');
    this.downloadDocument('api/Users/Document/' + updatedDocPath).subscribe(
      (blob: Blob) => {
        if (['pdf', 'jpeg', 'png'].some(ext => fileName.includes(ext))) {
          const dialogRef = this.dialog.open(PreviewFileComponent, {
            data: { blobData: blob },
            height: "92%",
            width: "60%",
            disableClose: true,
            autoFocus: true
          });
        } else {
          const lastIndex = fileName.lastIndexOf('\\');
          const sanitizedFileName = lastIndex !== -1 ? fileName.slice(lastIndex + 1) : fileName;
          const filePath = sanitizedFileName.slice(1).replace(/[\/\\]/g, '||');
          const link = document.createElement('a');
          link.href = window.URL.createObjectURL(blob);
          link.download = filePath;
          document.body.appendChild(link);
          link.click();
          document.body.removeChild(link);
        }



      },
      (error: string) => {
        this.ShowError(`${error}`, "Close", 2000);
      }
    )
  }

  downloadDocument(fileName: string): Observable<Blob> {
    const URL = fileName;

    return this.http.get(this.appConfig.UsersAPIURL + fileName, { responseType: 'blob' }).pipe(
      catchError((error: HttpErrorResponse) => {
        return throwError('Something went wrong while downloading the document.');
      })
    );
  }

  GETDocument(URL: string, data: any) {

    return this.http.post(this.appConfig.UsersAPIURL + URL, JSON.stringify(data), { headers: { responseType: 'blob', 'Content-Type': 'application/json', Authorization: `Bearer ${this.getUserDetails().Token}` } });
  }


  postNoAuth(URL: string, data: any) {
    return this.http.post(this.appConfig.UsersAPIURL + URL, data, { headers: { 'Content-Type': 'application/json', 'No-Auth': 'True' } });
  }
  getNoAuth(URL: string, id?: any | null) {
    return this.http.get(this.appConfig.UsersAPIURL + URL + (id == null ? '' : id), { headers: { 'Content-Type': 'application/json', 'No-Auth': 'True' } });
  }
  postJSON(URL: string, data: any) {
    return this.http.post(this.appConfig.UsersAPIURL + URL, JSON.stringify(data), { headers: { 'Content-Type': 'application/json', Authorization: `Bearer ${this.getUserDetails().Token}` } });
  }
  getJSON(URL: string, id?: any | null) {
    return this.http.get(this.appConfig.UsersAPIURL + URL + (id == null ? '' : id), { headers: { Authorization: `Bearer ${this.getUserDetails().Token}` } });
  }
  get(URL: string, options?: any) {
    return this.http.get(URL, options);
  }
  getRpt(URL: string, id?: any | null) {
    return this.http.get(this.appConfig.UsersAPIURL + URL + (id == null ? '' : id), { headers: { responseType: 'blob', 'Content-Type': 'application/octet-stream', Authorization: sessionStorage.getItem(environment.AuthToken) == null ? '' : sessionStorage.getItem(environment.AuthToken) } });
  }
  postRpt(URL: string, data: any) {
    return this.http.post(this.appConfig.UsersAPIURL + URL, JSON.stringify(data), { headers: { responseType: 'blob', 'Content-Type': 'application/json', Authorization: sessionStorage.getItem(environment.AuthToken) == null ? '' : sessionStorage.getItem(environment.AuthToken) } });
  }
  getReportJSON(URL: string, id?: any | null) {
    return this.http.get(this.appConfig.ReportsAPIURL + URL + (id == null ? '' : id));
  }
  postFile(URL: string, data: any) {
    return this.http.post(this.appConfig.UsersAPIURL + URL, data, { headers: { Authorization: `Bearer ${this.getUserDetails().Token}` } });
  }
  postJSONPromisis(URL: string, data: any) {
    return this.http.post(this.appConfig.UsersAPIURL + URL, JSON.stringify(data), { headers: { 'Content-Type': 'application/json', Authorization: `Bearer ${this.getUserDetails().Token}` } });
  }
  getMobileApp(apiUrl: string, options: any): Observable<Blob> {
    return this.http.get(this.appConfig.UsersAPIURL + apiUrl, { ...options, responseType: 'blob' }).pipe(
      map((response: Blob | ArrayBuffer) => {
        if (response instanceof ArrayBuffer) {
          return new Blob([response]);
        }
        return response;
      }),
      catchError((error: HttpErrorResponse) => {
        console.error('Error while fetching mobile app:', error);
        return throwError(() => new Error('Error while fetching the mobile app.'));
      })
    );
  }




  openSnackBar(message: string, action?: string | null, Duration?: number | 3000) {
    // return this.snackBar.open(message, action, {
    return this.snackBar.openFromComponent(SnackBarComponent, {
      duration: (Duration == null ? 3000 : Duration),
      verticalPosition: 'top',
      horizontalPosition: 'right',
      panelClass: ['alert-success'],
      politeness: 'polite',
      data: {
        message, snackType: 'Success'
      }
    });
  }
  ShowWarning(message: string, action?: string | null, Duration?: number | 3000) {
    // return this.snackBar.open(message, action, {
    return this.snackBar.openFromComponent(SnackBarComponent, {
      duration: (Duration == null ? 3000 : Duration),
      verticalPosition: 'top',
      horizontalPosition: 'right',
      panelClass: ['alert-warning'],
      politeness: 'polite',
      data: {
        message, snackType: 'Warn', action
      }
    });
  }
  ShowError(message: string, action?: string | null, Duration?: number | 3000) {

    // return this.snackBar.open(message, action, {
    return this.snackBar.openFromComponent(SnackBarComponent, {
      duration: (Duration == null ? 3000 : Duration),
      verticalPosition: 'top',
      horizontalPosition: 'right',
      panelClass: ['alert-danger'],
      politeness: 'polite',
      data: {
        message, snackType: 'Error', action
      },
    });
  }
  setTitle(newTitle: string) {
    setTimeout(() => {
      const title = (this.route.url.split('/')[1] ? this.route.url.split('/')[1].toUpperCase() + ' / ' : '') + newTitle;
      this.pageTitle.next(title);
    }, 200);
    // this.pageTitle =  new Observable(observer => { observer.next(newTitle) });
    this.titleService.setTitle(newTitle + ': ' + environment.PrjTitle);
  }
  getUserDetails(): UsersModel {
    return JSON.parse(atob(sessionStorage.getItem(environment.AuthToken)));
  }
  getUserImg(): string {
    return sessionStorage.getItem('UserImage');
  }
  getUserOrgs(): UserOrganizationModel[] {
    return JSON.parse(sessionStorage.getItem('UserOrgs'));
  }
  Base64ToFile(res: string, mimeType = 'application/pdf') {
    const byteCharacters = atob(res);
    const byteNumbers = new Array(byteCharacters.length);
    for (let i = 0; i < byteCharacters.length; i++)
      byteNumbers[i] = byteCharacters.charCodeAt(i);
    const byteArray = new Uint8Array(byteNumbers);
    const file = new Blob([byteArray], { type: mimeType });
    return this.domSanitizer.bypassSecurityTrustResourceUrl(URL.createObjectURL(file));
  }
  public ShowPreview(file: string): Observable<boolean> {
    let dialogRef: MatDialogRef<PrintpreviewComponent>;
    dialogRef = this.dialog.open(PrintpreviewComponent, {
      data: this.Base64ToFile(file),
      minWidth: '90%',
      height: '700px'
    });
    return dialogRef.afterClosed();
  }
  public confirmTrinee(title?: string, message?: string): Observable<boolean> {
    if (title == null) title = environment.InActiveConfirmBoxTitle;
    if (message == null) message = "Are you Sure?";
    let dialogRef: MatDialogRef<confirmdailogTraineeComponent>;
    dialogRef = this.dialog.open(confirmdailogTraineeComponent, {
      minHeight: '30%',
      minWidth: '40%'
    });
    dialogRef.componentInstance.title = title;
    dialogRef.componentInstance.message = message;
    return dialogRef.afterClosed();
  }
  public confirm(title?: string, message?: string): Observable<boolean> {
    if (title == null) title = environment.InActiveConfirmBoxTitle;
    if (message == null) message = 'Are you Sure?';
    let dialogRef: MatDialogRef<ConfirmDailogComponent>;
    dialogRef = this.dialog.open(ConfirmDailogComponent, {
      minHeight: '30%',
      minWidth: '40%',
      disableClose: true,
    });
    dialogRef.componentInstance.title = title;
    dialogRef.componentInstance.message = message;
    return dialogRef.afterClosed();
  }
  public confirmComplaintStatus(title?: string, message?: string): Observable<boolean> {
    if (title == null) title = environment.StatusConfirmBoxTitle;
    if (message == null) message = 'Are you Sure?';
    let dialogRef: MatDialogRef<ConfirmDailogComponent>;
    dialogRef = this.dialog.open(ConfirmDailogComponent, {
      minHeight: '30%',
      minWidth: '40%'
    });
    dialogRef.componentInstance.title = title;
    dialogRef.componentInstance.message = message;
    return dialogRef.afterClosed();
  }
  public confirmEventCancelStatus(title?: string, message?: string): Observable<boolean> {
    if (title == null) title = 'Event Cancellation';
    if (message == null) message = 'Are you Sure?';
    let dialogRef: MatDialogRef<ConfirmDailogComponent>;
    dialogRef = this.dialog.open(ConfirmDailogComponent, {
      minHeight: '30%',
      minWidth: '40%'
    });
    dialogRef.componentInstance.title = title;
    dialogRef.componentInstance.message = message;
    return dialogRef.afterClosed();
  }
  public confirmKAM(title?: string, message?: string): Observable<boolean> {
    if (title == null) title = 'Update KAM';
    if (message == null) message = 'Are you sure you want to update KAM for this TSP?';
    let dialogRef: MatDialogRef<ConfirmDailogComponent>;
    dialogRef = this.dialog.open(ConfirmDailogComponent, {
      height: '30%',
      width: '40%'
    });
    dialogRef.componentInstance.title = title;
    dialogRef.componentInstance.message = message;
    return dialogRef.afterClosed();
  }
  public confirmNTP(title?: string, message?: string): Observable<boolean> {
    if (title == null) title = 'Notice To Proceed';
    if (message == null) message = 'Are you sure you want to create NTP?';
    let dialogRef: MatDialogRef<ConfirmDailogComponent>;
    dialogRef = this.dialog.open(ConfirmDailogComponent, {
      height: '30%',
      width: '40%'
    });
    dialogRef.componentInstance.title = title;
    dialogRef.componentInstance.message = message;
    return dialogRef.afterClosed();
  }
  public confirmTraineePSP(title?: string, message?: string): Observable<boolean> {
    if (title == null) title = 'Assign PSP to Trainees';
    if (message == null) message = 'Once Submitted then cannot be edited?';
    let dialogRef: MatDialogRef<ConfirmDailogComponent>;
    dialogRef = this.dialog.open(ConfirmDailogComponent, {
      height: '30%',
      width: '40%'
    });
    dialogRef.componentInstance.title = title;
    dialogRef.componentInstance.message = message;
    return dialogRef.afterClosed();
  }
  public TrimFields(obj: any): any {
    if (obj === null && !Array.isArray(obj) && typeof obj != 'object') return obj;
    return Object.keys(obj).reduce((acc, key) => {
      acc[key.trim()] = typeof obj[key] === 'string' ?
        obj[key].trim() : typeof obj[key] === 'object' ? this.TrimFields(obj[key]) : obj[key];
      return acc;
    }, Array.isArray(obj) ? [] : {});
  }
  public RemSpaceFields(obj: any): any {
    if (obj === null && !Array.isArray(obj) && typeof obj != 'object') return obj;
    return Object.keys(obj).reduce((acc, key) => {
      acc[key.trim().replace(/\s/g, '')] = typeof obj[key] === 'string' ? obj[key].trim() : typeof obj[key] === 'object' ? this.RemSpaceFields(obj[key]) : obj[key];
      return acc;
    }, Array.isArray(obj) ? [] : {});
  }
  userAuthentication(URL: string, data: any) {
    const reqHeader = new HttpHeaders({ 'Content-Type': 'application/json', 'No-Auth': 'True' });
    return this.http.post(this.appConfig.UsersAPIURL + URL, data, { headers: reqHeader });
  }
  getFormRights(path: string = ''): UserRightsModel {
    const rights = JSON.parse(sessionStorage.getItem(environment.RightsToken));
    if (path.length > 0)
      return rights.filter(s => s.Path == path)[0];
    else
      return rights.filter(s => '/' + s.Path == this.route.url.substring(this.route.url.lastIndexOf('/')))[0];
    // return rights.filter(s => s.Path == this.route.url.split('/')[2].split('?')[0])[0];
  }

  // fetch and validate top level domain:
  fetchAndValidateTLD(email: string): Observable<boolean> {
    return new Observable<boolean>((observer) => {
      this.http.get('https://data.iana.org/TLD/tlds-alpha-by-domain.txt', { responseType: 'text' })
        .subscribe(
          (data: any) => {
            const validTLDs = data.split('\n').map(tld => tld.trim().toLowerCase());
            const emailDomain = email.split('@')[1]?.toLowerCase();
            const emailTLD = emailDomain?.split('.').pop();

            if (!validTLDs.includes(emailTLD)) {
              observer.next(false);
              observer.complete();
              return;
            }

            observer.next(true);
            observer.complete();
          },
          (error) => {
            this.error = 'Failed to fetch the TLD list';
            observer.error('Failed to fetch TLD list');
          }
        );
    });
  }


  private initHeaders(): HttpHeaders {
    const token = sessionStorage.getItem(environment.AuthToken);
    let headers;
    if (token !== null) {
      headers = new HttpHeaders({
        'Content-Type': 'application/json',
        Pragma: 'no-cache', Authorization: btoa(token)
      });
    }
    else {
      headers = new HttpHeaders({
        'Content-Type': 'application/json',
        Pragma: 'no-cache'
      });
    }
    return headers;
  }
  getMenuItems(): MenuItem[] {
    // debugger;
    this.userrights = JSON.parse(sessionStorage.getItem(environment.RightsToken));
    // console.log(this.userrights)
    this.currentUser = this.getUserDetails();
    // console.log(this.userrights);
    // debugger;
    if (this.currentUser.UserLevel == EnumUserLevel.TSP) {
      this.FormIDRemoveForTSP.forEach(element => {
        this.userrights = this.userrights.filter(item => item.FormID != element);
      });
    }
    else {
      this.FormIDRemoveForAll.forEach(element => {
        this.userrights = this.userrights.filter(item => item.FormID != element);
      });
    }
    if (this.userrights != null)
      return this.userrights
        .filter(route => route.CanView == 1) // only add a menu item for routes with a title set.
        .map(route => {
          return {
            path: '/' + route.modpath + '/' + route.Path,
            parent: route.ModuleTitle,
            title: route.FormName,
            ModSortOrder: route.ModSortOrder,
            SortOrder: route.SortOrder,
            icon: route.Icon ? route.Icon : 'dashboard'
          };
        });
    else
      return [];


  }
  private lastRouteWithMenuItem(route: ActivatedRoute): MenuItem {
    let lastMenu = undefined;
    do { lastMenu = this.extractMenu(route) || lastMenu; }
    while ((route = route.firstChild));
    return lastMenu;
  } 
  private extractMenu(route: ActivatedRoute): MenuItem {
    const cfg = route.routeConfig;
    return cfg && cfg.data && cfg.data.title
      ? { path: cfg.path, title: cfg.data.title, parent: this.userrights.filter(a => a.path == cfg.path)[0].ModuleTitle, icon: (cfg.data.icon ? cfg.data.icon : 'dashboard') }
      : undefined
  }
  FetchReport(SPName: string, ReportName: string, paramObject: any) {
    try {
      const Param = this.GetParamString(SPName, paramObject);
      const data: any = this.getJSON(`api/BSSReports/FetchList?Param=${Param}`).toPromise();
      if (data.length > 0) {
        this.ExportToExcel(data, ReportName);
      } else {
        this.ShowWarning(' No Record Found', 'Close');
      }
    } catch (error) {
      this.error = error;
    }
  }
  GetParamString(SPName: string, paramObject: any) {
    let ParamString = SPName;
    for (const key in paramObject) {
      if (paramObject.length > 0) {
        if (Object.hasOwnProperty.call(paramObject, key)) {
          ParamString += `/${key}=${paramObject[key]}`;
        }
      }
    }
    return ParamString;
  }
  ExportToExcel(data: any, FileName: any) {
    const DateTime = new Date().toLocaleString(undefined, {
      year: 'numeric',
      month: '2-digit',
      day: '2-digit',
      hour: '2-digit',
      minute: '2-digit',
      second: '2-digit',
      hour12: true
    });

    const ws: XLSX.WorkSheet = XLSX.utils.json_to_sheet(data);

    const wb: XLSX.WorkBook = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, FileName);
    const fileName = `${FileName}_${DateTime}.xlsx`;
    XLSX.writeFile(wb, fileName);
  }





  ExcelExportWithForm(data, ReportName) {

debugger;
    const Data = data[0].map(obj => {
      debugger;
      const newObj = {};
      for (const key in obj) {
        if (!key.toLowerCase().includes('id')) {
          newObj[key] = obj[key];
        }
      }
      return newObj;
    });

    if (data[0].length > 0) {
      let exportExcel: ExportExcel = {
        Title: ReportName,
        Author: this.currentUser.FullName,
        Type: EnumExcelReportType.SRN,
        Month: new Date(),
        Data: data[1],
        List1: Data
      };
      this.dialogue.openExportConfirmDialogue(exportExcel).subscribe();
    } else {
      this.ShowWarning(' No Record Found', 'Close');
    }




  }


  ExcelExporWithForm(ExportDataObject, ReportName) {


    const Data = ExportDataObject.map(obj => {
      const newObj = {};
      for (const key in obj) {
        if (!key.toLowerCase().includes('id') &&
          !key.toLowerCase().includes('attachment') &&
          !key.toLowerCase().includes('evidence')) {
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
      this.dialogue.openExportConfirmDialogue(exportExcel).subscribe();
    } else {
      this.ShowWarning(' No Record Found', 'Close');
    }


  }

}


export interface IQueryFilters {
  SchemeID: number;
  TSPID: number;
  ClassID: number;
  TraineeID: number;
  UserID: number;
  OID?: number;
}
