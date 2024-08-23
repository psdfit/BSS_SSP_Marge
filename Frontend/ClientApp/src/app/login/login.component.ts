/* ****Aamer Rehman Malik *****/
import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { CommonSrvService } from '../common-srv.service';
import { AuthService } from '../security/auth-service.service';
import { UsersModel } from '../master-data/users/users.component';
import { EnumUserRoles } from '../shared/Enumerations';
import { FormGroup } from '@angular/forms';

import { Router } from '@angular/router';
import { NgbCarouselConfig } from '@ng-bootstrap/ng-bootstrap';
@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  hide = true;
  user: any = {};
  kamAssignedUsers: any;
  userids: any[];
  rememberme = false;
  kamUser: number;
  kamRoleId: number;
  kamUserFlag = false;
  currentUser: UsersModel;
  UserID1: UsersModel;



  PassComp: boolean = false;
  password: string = '';
  passwordForm: FormGroup;
  // images: string[] = [
  //   '../../../../assets/images/LMS---Slider-04.jpg',
  //   '../../../../assets/images/LMS---Slider-04.jpg',
  //   '../../../../assets/images/LMS---Slider-04.jpg',
  //   // Add more image paths as needed
  // ];
	images = [1055, 194, 368].map((n) => `https://picsum.photos/id/${n}/900/500`);


  constructor(private Common: CommonSrvService, private auths: AuthService, private router: Router, config: NgbCarouselConfig) {
    Common.setTitle('User Login');
    config.showNavigationArrows = true;
		config.showNavigationIndicators = true;
  }

  onEnterKey(event: KeyboardEvent): void {
    event.preventDefault();
  }


  UserName = ''
  Userpassword = ''
  ngOnInit() {
    if (this.Common.sharedDataObj != undefined && this.Common.sharedDataObj.hasOwnProperty('UserName')) {
      this.user.UserName = this.Common.sharedDataObj.UserName
      this.user.Userpassword = this.Common.sharedDataObj.UserPassword
    }
    this.resetTokens();
  }

  resetTokens() {
    localStorage.clear();
    sessionStorage.clear();
  }


  Submit(form: NgForm) {
    // if (this.PassComp) {
    this.Common.postNoAuth('api/Users/Login', this.user).subscribe((response: any) => {
      this.auths.setCredntial(response, this.rememberme);
      console.log(response)
      this.checkForKAMUser();
      // this.router.navigate(['/']);



      // if (this.currentUser.RoleID == EnumUserRoles.PBTE) {
      //  this.router.navigate(['pbte/pbte']);
      // }
      // if (this.currentUser.RoleID == EnumUserRoles.DataEntryOperator) {
      //  this.router.navigate(['placement/self-employment-verification']);
      // }
      // else {
      //  this.router.navigate(['/']);
      // }
    },
      error => { this.Common.ShowError(error.error, 'Close'); }
    )
    // }

  }

  receiveMessage($event) {
    console.log($event)
  }

  checkForKAMUser() {
    this.currentUser = this.Common.getUserDetails();
    const userid = this.currentUser.UserID;

    this.Common.getJSON('api/KAMAssignment/RD_KAMAssignmentForFilters').subscribe((d: any) => {
      this.kamAssignedUsers = d;
      this.userids = this.kamAssignedUsers.filter(y => y.UserID === userid);
      if (this.userids.length > 0) {
        this.kamRoleId = this.userids.map(x => x.RoleID)[0];
      }

      this.route();
      // x.UserID, y => y.RoleID)
    },
    );
  }

  route() {

    // this.currentUser = this.Common.getUserDetails();
    // var userid = this.currentUser.UserID;

    // this.Common.getJSON('api/KAMAssignment/RD_KAMAssignmentForFilters').subscribe((d: any) => {
    //  this.kamAssignedUsers = d;
    //  this.userids = this.kamAssignedUsers.filter(y => y.UserID == userid);
    //  if (this.userids.length > 0) {
    //    this.kamUserFlag = true;
    //    this.kamRoleId = this.userids.map(x => x.RoleID)[0];
    //  }
    //    //x.UserID, y => y.RoleID)
    // },
    // );


    console.log(this.currentUser.RoleID)
    switch (this.currentUser.RoleID) {
      case EnumUserRoles.PBTE:
        this.router.navigate(['pbte/pbte']);
        break;
      case EnumUserRoles.DataEntryOperator:
        this.router.navigate(['placement/self-employment-verification']);
        break;
      case this.kamRoleId:
        this.router.navigate(['dashboard/kam-dashboard']);
        break;
      case EnumUserRoles.SSPRegistration:
        this.router.navigate(['profile-manage/profile']);
        break;
      default:

        this.router.navigate(['/']);
    }
    console.log(this.currentUser.RoleID + 'End==============')

  }
}
