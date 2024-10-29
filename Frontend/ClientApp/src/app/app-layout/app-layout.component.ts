import { Component, ChangeDetectorRef, OnDestroy, OnInit, HostListener, AfterViewInit } from '@angular/core';
import { MediaMatcher } from '@angular/cdk/layout';
// import { NavigationService, MenuItem } from "../navigation.service";
import { Observable } from 'rxjs';
import { AuthService } from '../security/auth-service.service';
import { fadeAnimation } from '../animations';
import { environment } from 'src/environments/environment';
import { MatDialog } from '@angular/material/dialog';
import { CommonSrvService, MenuItem } from '../common-srv.service';
import { ChangePasswordComponent } from '../master-data/change-password/change-password.component';
import { UsersModel, UserOrganizationModel } from '../master-data/users/users.component';
import { DialogueService } from '../shared/dialogue.service';
import { HubConnection } from '@aspnet/signalr';
import * as signalR from '@aspnet/signalr';
import { AppConfigService } from '../app-config.service';
import { Router, ActivatedRoute } from '@angular/router';
import { EnumUserRoles } from '../shared/Enumerations';
//import $ from "jquery";
import * as $ from 'jquery';
@Component({
  selector: 'app-app-layout',
  templateUrl: './app-layout.component.html',
  styleUrls: ['./app-layout.component.scss'],
  animations: [fadeAnimation],
})
export class AppLayoutComponent implements OnDestroy, OnInit {
  private _mobileQueryListener: () => void;
  mobileQuery: MediaQueryList;
  isExpanded = true;
  user: UsersModel;
  userSessionDetail: UsersModel;
  kamAssignedUsers: any;
  userObj: any[];
  kamUser: number;
  kamRoleId: number;
  kamUserFlag = false;
  appConfig: any;
  title = environment.PrjTitle;
  titleShort = environment.PrjTitleShort;
  mainMenuItems = {};
  Notifications = [];
  animals = ['pigs', 'goats', 'sheep'];
  // NotificationslIST = [];
  NotificationslIST = [];
  KAMInfo: any[];
  activeMenuItem$: Observable<MenuItem>;
  isLoggedIn$: Observable<boolean>;
  OID: number = null;
  NotificationCount = 0;
  OName = '';
  working = false;
  error = '';
  img = '';
  PageNo = 1;
  notificationcount = 0;
  NotificationCountHideAndShow = false;
  ViewAllButtonHideAndShow = false;
  RecordParPage = 5;
  userOrgs: UserOrganizationModel[] = [];
  userOrgsArray: UserOrganizationModel[] = [];
  open = {};
  sopen = false;
  Loading: Observable<boolean>;
  PageTitle: Observable<string>;
  processing = false;
  IsPbteUser = false;
  IsDeoUser = false;
  IsTspUser = false;
  IsKamUser = false;
  pTitle = '';
  public messages: string[];
  private _hubConnection: HubConnection;
  constructor(private http: CommonSrvService, private router: Router, private ActiveRoute: ActivatedRoute, private appConfigService: AppConfigService, private ComSrv: CommonSrvService, public dialogueService: DialogueService, changeDetectorRef: ChangeDetectorRef, media: MediaMatcher, public dialog: MatDialog, private auth: AuthService, private common: CommonSrvService) {
    this.mobileQuery = media.matchMedia('(max-width: 600px)');
    this._mobileQueryListener = () => changeDetectorRef.detectChanges();
    this.mobileQuery.addListener(this._mobileQueryListener);
    this.user = common.getUserDetails()
    this.img = common.getUserImg()
    this.activeMenuItem$ = this.common.activeMenuItem$;
    this.isLoggedIn$ = this.auth.isLoggedIn;
    this.messages = [];
    this.userOrgs = this.common.getUserOrgs();
    this.checkForKAMUser();
    this.isLoggedIn$.subscribe(v => {
      if (v) {
        if (this.user.RoleID == EnumUserRoles.PBTE) {
          this.IsPbteUser = true;
        }
        if (this.user.RoleID == EnumUserRoles.DataEntryOperator) {
          this.IsDeoUser = true;
        }
        if (this.user.RoleID == EnumUserRoles.TSP) {
          this.IsTspUser = true;
        }
        
        this.mainMenuItems = this.common.getMenuItems();
        console.log(this.mainMenuItems);
        this.userOrgs = this.common.getUserOrgs();
      }
      else
        this.mainMenuItems = {};
    });
    this.PageTitle = common.pageTitle;
    this.PageTitle.subscribe((n) => {
      if (n != '')
        this.pTitle = n == null ? '' : n
    });
    // this.PageTitle.subscribe((n) => this.pTitle = n);
    this.appConfig = this.appConfigService.getAppConfig();
  }
  //Author: Ali Haider
  //Date: 20-Jun-2023
  //Specification: Logout triggred on closing of browser or tab.
  // @HostListener('window:beforeunload', ['$event'])
  // unloadHandler(event: Event) {
  //   var inFormOrLink;
  //   $('a').on('click', function () { inFormOrLink = true; });
  //   $('form').bind('submit', function () { inFormOrLink = true; });
  //   // Remove the event listeners for 'click' on 'a' elements and 'submit' on 'form' elements
  //   $('a').off('click');
  //   $('form').off('submit');
  //   if (this.IsTspUser) {
  //     if (!inFormOrLink) {
  //       this.logout();
  //     }
  //   }
  // }
  // -----------------//Checking login for every click.
  // @HostListener('document:click', ['$event'])
  // documentClick(event: MouseEvent) {
  //   //Checking session is logout or not
  //   if (this.IsTspUser) {
  //     if (this.router.url !== "/profile-manage/profile" && this.router.url !== "/profile-manage/base-data") {
  //       this.common.getJSON('api/Users/GetSession?SessionID=' + sessionStorage.getItem("SessionID") + '&UserID=' + sessionStorage.getItem("UserID")).subscribe((d: any) => {
  //         this.userSessionDetail = d;
  //         if (this.userSessionDetail === null) {
  //           if (this.IsTspUser) {
  //             this.logout();
  //           }
  //         }
  //       }, error => this.error = error);
  //     }

  //   }
  // }
  ngOnInit(): void {
    // debugger;
    this.Notification();
    this.GetNotificationsDetail();
    this.GetOrgnations();
    this.OName = this.userOrgs[0].OName
  }
  GetOrgnations() {
    this.common.getJSON('api/Organization/GetOrganization').subscribe((d: any) => {
      this.userOrgsArray = d;
    }, error => this.error = error // error path
    );
  }
  setOrg(Org: UserOrganizationModel) {
    this.common.OID.next(Org.OID);
    this.OName = Org.OName;
  }
  // openDialogForNotificationDetail
  openDialogForNotificationDetail(data: any): void {
    this.dialogueService.openDialogForNotificationDetail(data);
  }
  openDialog(): void {
    // let dialogRef: MatDialogRef<ChangePasswordComponent>;
    const dialogRef = this.dialog.open(ChangePasswordComponent, {
      width: '350px'
    });
    dialogRef.afterClosed().subscribe(result => {
      if (result != undefined) {
        const a = result.value.NewPassword;
        this.common.postJSON('api/Users/ChangePassword', { UserID: this.common.getUserDetails().UserID, UserPassword: a, InActive: false })
          .subscribe((d: any) => {
            if (d == true) {
              this.common.openSnackBar(environment.UpdateMSG.replace('${Name}', 'Change Password'));
            } else {
              this.common.ShowError('Given Password is Already existed.Please use another password', 'Close');
            }
            // this.reset(r);
          },
            error => this.error = error // error path
            , () => {
              this.working = false;
            });
      }
    });
  }
  checkForKAMUser() {
    this.user = this.common.getUserDetails();
    const userid = this.user.UserID;
    this.common.getJSON('api/KAMAssignment/RD_KAMAssignmentForFilters').subscribe((d: any) => {
      this.kamAssignedUsers = d;
      this.userObj = this.kamAssignedUsers.filter(y => y.UserID == userid);
      if (this.userObj.length > 0) {
        this.kamRoleId = this.userObj.map(x => x.RoleID)[0];
        this.IsKamUser = true;
        if (this.user.RoleID == this.kamRoleId) {
          this.IsKamUser = true;
        }
        else {
          this.IsKamUser = false;
        }
      }
      // x.UserID, y => y.RoleID)
    },
    );
  }
  openKAMInfoDialog(): void {
    this.common.getJSON('api/KAMAssignment/RD_KAMInforForTSP').subscribe((response: any) => {
      // response = response.reduce((accumulator, value) => accumulator.concat(value), []);
      this.KAMInfo = response;
      // const dialogRef = this.dialog.open(KAMInformationDialogComponent, {
      //  minWidth: '500px',
      //  minHeight: '50px',
      //  //data: JSON.parse(JSON.stringify(row))
      //  data: this.KAMInfo
      // });
      // dialogRef.afterClosed().subscribe(result => {
      // });
    });
  }
  logout() {
    //this.auth.logout();
    this.common.getJSON('api/Users/Logout?SessionID=' + sessionStorage.getItem("SessionID") + '&UserID=' + sessionStorage.getItem("UserID")).subscribe((d: any) => {
      this.PageNo++;
    }, error => this.error = error // error path
    );
    sessionStorage.removeItem(environment.AuthToken);
    sessionStorage.removeItem(environment.RightsToken);
    sessionStorage.removeItem("UserImage");
    localStorage.removeItem('RememberMe');
    //this.isLoggedIn$.next(false);
    this.router.navigate(["/login"]);
  }
  ngOnDestroy(): void {
    this.mobileQuery.removeListener(this._mobileQueryListener);
  }
  onDeactivate() {
    document.body.scrollTop = 0;
    window.scrollTo(0, 0)
  }
  GetNotificationsDetail() {
    this.common.getJSON('api/Notifications/GetNotificationsDetail?PageNo=' + this.PageNo + '&RecordParPage=' + this.RecordParPage).subscribe((d: any) => {
      for (let index = 0; index < d.length; index++) {
        this.NotificationslIST.push(d[index]);
      }
      if (d[0] != null) {
        if (d[0].ReadNotificationcount > 0) {
          this.NotificationCount = d[0].ReadNotificationcount;
          if (this.NotificationCount > 3) { this.ViewAllButtonHideAndShow = true; }
          if (this.NotificationCount > 0) { this.NotificationCountHideAndShow = true; }
        }
      }
      this.PageNo++;
    }, error => this.error = error // error path
    );
    const f = ['d']
  }
  Notification() {
    this._hubConnection = new signalR.HubConnectionBuilder()
      .configureLogging(signalR.LogLevel.Debug)
      .withUrl(this.appConfig.UsersAPIURL + 'notifications', {
        skipNegotiation: true,
        transport: signalR.HttpTransportType.WebSockets
      })
      .build();
    this._hubConnection.serverTimeoutInMilliseconds = 100000; // 100 seconds
    Object.defineProperty(WebSocket, 'OPEN', { value: 1, });
    this._hubConnection
      .start()
      .then(() => this._hubConnection.invoke('GetConnectionId', this.ComSrv.getUserDetails().UserID.toString()).then(function (connectionId) { console.log(connectionId) }))
      .catch(err => console.log(err));
    this._hubConnection.on('SendNotification', (message: any) => {
      this.NotificationslIST.unshift({ EventAction: message.eventAction, NotificationName: message.notificationName, Body: message.body, DateDiff: 'Recently', Subject: message.subject, NotificationDetailID: message.notificationDetailID });
      // this.notificationcount=   this.NotificationCount;
      // this.NotificationCount=null;
      this.NotificationCount = this.NotificationCount + 1;
      this.NotificationCountHideAndShow = true;
    });
  }
  NotificationCountHideAndShowFunction() {
    this.NotificationCountHideAndShow = false;
  }
  onScroll() {
    this.GetNotificationsDetail();
  }
  CurrentopenNotification(data: any) {
    this.router.navigateByUrl(`/notification/viewallnotification/${data.NotificationDetailID}`);
    return true;
    // this.router.navigate(['/notification/viewallnotification',data]);
  }
}
