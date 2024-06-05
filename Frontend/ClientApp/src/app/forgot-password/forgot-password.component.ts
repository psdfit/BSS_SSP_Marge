/* ****Aamer Rehman Malik *****/
import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { CommonSrvService } from '../common-srv.service';
import { AuthService } from '../security/auth-service.service';


import { Router } from '@angular/router';
import { UsersModel } from '../master-data/users/users.component';
@Component({
  selector: 'hrapp-forgot',
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.scss']
})
export class ForgotPasswordComponent implements OnInit {
  hide: boolean = true;
  user: any = {};
  EMail = '';
  Rec :UsersModel;
  rememberme = false;
  constructor(private Common: CommonSrvService, private auths: AuthService, private router: Router) {
    Common.setTitle("Forgot Password");
  }
 censorWord = function (str) {
  return str[0] + "*".repeat(str.length - 2) + str.slice(-1);
};

 censorEmail = function (email) {
  var arr = email.split("@");
  return this.censorWord(arr[0]) + "@" + arr[1];
}
onEnterKey(event: KeyboardEvent): void {
  event.preventDefault();
}
  ngOnInit() {
  }
  FindUser() {
       
    const UserName =this.user.UserName; // The original value
    const ModifiedUsername = UserName.replace(/\//g, '|||'); // Replace "/" with "|||"  
    this.Common.getNoAuth(`api/Users/RD_UsersByUserName/${ModifiedUsername}`).subscribe((response: any) => {
      this.Rec = response[0];
      this.EMail = this.censorEmail(this.Rec.Email);
    },
      error => {
        this.Common.ShowError("Invalid username or password", "Close");
      })
  }
  Submit(form: NgForm) {
    const UserName =this.user.UserName; // The original value
    const ModifiedUsername = UserName.replace(/\//g, '|||'); // Replace "/" with "|||"  
    this.Common.getNoAuth("api/Users/ForgotPassword/", ModifiedUsername).subscribe((response: any) => {
      if(response)
      this.Common.openSnackBar("En E-Mail is sent with your password to your E-Mail Adrress.")
    },
      error => {
        this.Common.ShowError("Invalid username or password", "Close");
      })
  }
}
