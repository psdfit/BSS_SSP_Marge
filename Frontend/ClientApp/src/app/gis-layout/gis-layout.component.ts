import { Component, ChangeDetectorRef, OnDestroy, OnInit } from '@angular/core';
import { MediaMatcher } from '@angular/cdk/layout';
//import { NavigationService, MenuItem } from "../navigation.service";
import { title } from 'process';
import { Observable } from 'rxjs';
import { AuthService } from '../security/auth-service.service';
import { fadeAnimation } from '../animations';
import { trigger, transition, style, animate, query, stagger } from '@angular/animations';
import { environment } from 'src/environments/environment';
import { MatDialog } from '@angular/material/dialog';
import { CommonSrvService, MenuItem } from '../common-srv.service';
import { FormGroupDirective } from '@angular/forms';
import { ChangePasswordComponent } from '../master-data/change-password/change-password.component';
import { delay } from 'rxjs/operators';
import { UsersModel, UserOrganizationModel } from '../master-data/users/users.component';
import { DialogueService } from '../shared/dialogue.service';
import { HttpParams } from '@angular/common/http';

@Component({
  selector: 'app-gis-layout',
  templateUrl: './gis-layout.component.html',
  styleUrls: ['./gis-layout.component.scss']
})
export class GisLayoutComponent implements OnInit {

  private _mobileQueryListener: () => void;
  mobileQuery: MediaQueryList;
  isExpanded = true;
  user: UsersModel;
  title = environment.PrjTitle;
  titleShort = environment.PrjTitleShort;
  mainMenuItems = {};
  Notifications = [];
   animals = ['pigs', 'goats', 'sheep'];
 // NotificationslIST = [];
  NotificationslIST= [];
  activeMenuItem$: Observable<MenuItem>;
  isLoggedIn$: Observable<boolean>;
  OID: number=null;
  NotificationCount:null=null;
  OName: string='';
  working = false;
  error = '';
  img = '';
  PageNo:number=1;
  RecordParPage:number=5;
  userOrgs: UserOrganizationModel[]=[];
  open = {};
  sopen = true;
  Loading: Observable<boolean>;
  PageTitle: Observable<string>;
  processing: boolean = false;
  pTitle = "";
  constructor(public dialogueService: DialogueService,changeDetectorRef: ChangeDetectorRef, media: MediaMatcher, public dialog: MatDialog, private auth: AuthService, private common: CommonSrvService) {
    this.mobileQuery = media.matchMedia('(max-width: 600px)');
    this._mobileQueryListener = () => changeDetectorRef.detectChanges();
    this.mobileQuery.addListener(this._mobileQueryListener);
    this.user = common.getUserDetails()
    this.img = common.getUserImg()
    this.activeMenuItem$ = this.common.activeMenuItem$;
    this.isLoggedIn$ = this.auth.isLoggedIn;
    
    this.userOrgs = this.common.getUserOrgs();
    this.isLoggedIn$.subscribe(v => {
      if (v) {
        this.mainMenuItems = this.common.getMenuItems();
        this.userOrgs = this.common.getUserOrgs();
      }
      else
        this.mainMenuItems = {};
    });
    this.PageTitle = common.pageTitle;
    this.PageTitle.subscribe((n) => {
      if (n != "")
        this.pTitle = n == null ? "" : n
    });
    // this.PageTitle.subscribe((n) => this.pTitle = n);
  }
  ngOnInit(): void {
   // debugger;
    this.GetNotificationsDetail();
  }
  setOrg(Org:UserOrganizationModel) {
    this.common.OID.next(Org.OID);
    this.OName = Org.OName;
  }
  //openDialogForNotificationDetail
  openDialogForNotificationDetail(data: any): void {
    debugger;
    this.dialogueService.openDialogForNotificationDetail(data);
  }
  openDialog(): void {
    // let dialogRef: MatDialogRef<ChangePasswordComponent>;
    let dialogRef = this.dialog.open(ChangePasswordComponent, {
      width: '350px'
    });
    dialogRef.afterClosed().subscribe(result => {
      if (result != undefined) {
        let a = result.value.NewPassword;

        this.common.postJSON('api/Users/ChangePassword', { UserID: this.common.getUserDetails().UserID, UserPassword: a, InActive: false })
          .subscribe((d: any) => {
            this.common.openSnackBar(environment.UpdateMSG.replace("${Name}", "Change Password"));
            // this.reset(r);
          },
            error => this.error = error // error path
            , () => {
              this.working = false;
            });
      }
    });
  }
  logout() {
    this.auth.logout();
  }
  ngOnDestroy(): void {
    this.mobileQuery.removeListener(this._mobileQueryListener);
  }
  onDeactivate() {
    document.body.scrollTop = 0;
    window.scrollTo(0, 0)
  }
  GetNotificationsDetail() {
    debugger;
    this.common.getJSON('api/Notifications/GetNotificationsDetail?PageNo='+this.PageNo+"&RecordParPage="+this.RecordParPage).subscribe((d: any) => {
      debugger;
   
     for (let index = 0; index < d.length; index++) {
      debugger;
       this.NotificationslIST.push(d[index]);
       
     }
     
     this.NotificationCount+= d.length;
        this.PageNo++;
    }, error => this.error = error // error path
    );
    let f=["d"]
   
  }

  onScroll()  
  { 
    this.GetNotificationsDetail();
    debugger;

  }  

}
