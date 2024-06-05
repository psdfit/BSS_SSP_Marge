import { Component, OnInit, Inject } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { CommonSrvService } from '../../common-srv.service';

import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'hrapp-change-password',
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.scss']
})
export class ChangePasswordComponent implements OnInit {
  
  changepasswordform: FormGroup;
  _user: any; v: string = '';
  title: string; savebtn: string; working: boolean; error: string
  EnText: string = "Change Password";
  constructor(private fb: FormBuilder, private http: CommonSrvService, public dialogRef: MatDialogRef<ChangePasswordComponent>, @Inject(MAT_DIALOG_DATA) public data: DialogData) {
    this.changepasswordform = this.fb.group({
      OldPassword: "",
      NewPassword: "",
      ConfirmPassword: ""
    }, { updateOn: "change" });
    this.changepasswordform.valueChanges.subscribe(a => {
      if (a.NewPassword !== a.ConfirmPassword) {
        this.CheckPassComp(a) 
        this.ConfirmPassword.setErrors({'mismatch': true });
      } else {
        this.ConfirmPassword.setErrors(null);
      }
    });
  }

  ngOnInit() {
    this.OldPassword.valueChanges.subscribe(a => {
      this.http.postJSON('api/Users/CheckUser', { 'UserName': this.http.getUserDetails().UserName, 'UserPassword': this.OldPassword.value })
        .subscribe((d: any) => {
          if (d)
            this.OldPassword.setErrors(null);
          else
            this.OldPassword.setErrors({ 'Invalid': true });
        }, error => this.OldPassword.setErrors({ 'Invalid': true }) // error path
        );
    });

    this.NewPassword.valueChanges.subscribe(a => {
      this.http.postJSON('api/Users/CheckUserPass', { 'UserName': this.http.getUserDetails().UserName, 'UserPassword': this.NewPassword.value })
        .subscribe((d: any) => {
          
          if (d==true){
            console.log('ok');
            
            this.NewPassword.setErrors({ 'AlreadyExisted': true });
          } else{

            this.NewPassword.setErrors(null);
          }
        }, error => this.NewPassword.setErrors({ 'Invalid': true }) // error path
        );
    });


  }
  Userpassword:any
  passwordErrors: string[] = [];
  PassComp:boolean=false;
  isOpen = false;
  passwordStrength: any = 0;
  progressBarWidth: string = '0%';
  progressBarColor: string = '';
  CheckPassComp(user) {
   
    
    this.passwordErrors = [];
   this.Userpassword=user.NewPassword.trim()
    // Password complexity rules
    const minLength = 8;
    const maxLength = 14;
    const hasUpperCase = /[A-Z]/.test(this.Userpassword);
    const hasLowerCase = /[a-z]/.test(this.Userpassword);
    const hasNumbers = /[0-9]/.test(this.Userpassword);
    const hasSpecialChar = /[!@#$%^&*()_+{}\[\]:;<>,.?~\\-]/.test(this.Userpassword);
    
   
    if (!hasLowerCase) {
      this.passwordErrors.push('At least one lowercase.');
    }
    if (!hasUpperCase) {
      this.passwordErrors.push('At least one uppercase.');
    }
    if (!hasNumbers) {
      this.passwordErrors.push('At least one number.');
    }
    if (!hasSpecialChar) {
      this.passwordErrors.push('At least one special character.');
    }
  
    
    if (this.Userpassword.length < minLength) {
      this.passwordErrors.push(`Minimum ${minLength} characters.`);
    } 
     if (this.Userpassword.length > maxLength) {
      this.passwordErrors.push(`Maximum ${maxLength} characters.`);
    }
  
    this.passwordStrength = 0;
  


    if (hasLowerCase    && this.Userpassword.length>0) this.passwordStrength++;
    if (hasUpperCase    && this.Userpassword.length>0) this.passwordStrength++;
    if (hasNumbers      && this.Userpassword.length>0) this.passwordStrength++;
    if (hasSpecialChar  && this.Userpassword.length>0) this.passwordStrength++;
  
    if (this.Userpassword.length <= maxLength) {
      this.passwordStrength++;
    }
  
    if (this.Userpassword.length >= minLength) {
      this.passwordStrength++;
    }
    this.PassComp = this.passwordStrength === 6; 
    this.progressBarWidth = (this.passwordStrength * 17.6) + '%'; 


    if (this.passwordStrength >=1 && this.passwordStrength <=2) {
      this.progressBarColor = '#d83752';
    }  if (this.passwordStrength >= 3 && this.passwordStrength <=4) {
      this.progressBarColor = '#e4a11b';
    }  if (this.passwordStrength >= 5) {
      this.progressBarColor = '#14a44d';
    }
  }

  get OldPassword() { return this.changepasswordform.get("OldPassword"); }
  get NewPassword() { return this.changepasswordform.get("NewPassword"); }
  get ConfirmPassword() { return this.changepasswordform.get("ConfirmPassword"); }
}

export interface DialogData {
  OldPassword: string;
  NewdPassword: string;
  ConfirmPassword: string;
}
