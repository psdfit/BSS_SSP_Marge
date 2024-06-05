/* ****Aamer Rehman Malik *****/
import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { CommonSrvService } from '../common-srv.service';
import { AuthService } from '../security/auth-service.service';


import { Router } from '@angular/router';
import { UsersModel } from '../master-data/users/users.component';
import { ElementSchemaRegistry } from '@angular/compiler';
@Component({
  selector: 'app-send-otp',
  templateUrl: './send-otp.component.html',
  styleUrls: ['./send-otp.component.scss']
})
export class SendOTPComponent implements OnInit {
  hide: boolean = true;
  user: any = {};
  EMail = '';
  TSPID = '';
  Rec: UsersModel;
  rememberme = false;
  constructor(private Common: CommonSrvService, private auths: AuthService, private router: Router) {
    Common.setTitle("Verify OTP");
  }

  onEnterKey(event: KeyboardEvent): void {
    event.preventDefault();
  }
  ngOnInit() {
    console.log(this.Common.sharedDataObj[0])

    if (this.Common.sharedDataObj === undefined) {
      this.router.navigate(['login']);
    } else {
      this.Common.openSnackBar("En E-Mail is sent with 6-digit Code to your E-Mail Adrress.")
      this.EMail = this.Common.sharedDataObj[0].TspEmail
    }

  }




  Submit(form: NgForm) {
    this.user['TSPID'] = this.Common.sharedDataObj[0].TspID
    console.log(this.user)
    this.Common.postNoAuth("api/Users/OTPVerification", this.user).subscribe((response: any) => {
      console.log(response[0])
      if (response) {
        this.Common.setMessage(response[0])
        this.router.navigate(['login']);
      }


    },
      error => {
        this.Common.ShowError("Invalid 6-digit Code", "Close", 10000);
      })
  }
}
