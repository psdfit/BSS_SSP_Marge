import { __await } from 'tslib';
import { Component, ElementRef, OnInit, Renderer2 } from '@angular/core';
import { CommonSrvService } from '../common-srv.service';
import { MatDialog } from '@angular/material/dialog';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { DialogueService } from '../shared/dialogue.service';
import { ActivatedRoute, Router } from '@angular/router';
@Component({
  selector: 'app-tsp-sign-up',
  templateUrl: './tsp-sign-up.component.html',
  styleUrls: ['./tsp-sign-up.component.scss'],
})
export class TspSignUpComponent implements OnInit {
  BName: boolean = false;
  error: any;
  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private fb: FormBuilder,
    private ComSrv: CommonSrvService,
    public dialog: MatDialog,
    public dialogueService: DialogueService,
    private renderer: Renderer2,
    private el: ElementRef
  ) { }
  ngOnInit(): void {
    this.CreateForm();
  }
  signUp: FormGroup;
  hide = true;
  NTNFormat: any = "1"
  CreateForm() {
    this.signUp = this.fb.group({
      TSPID: [''],
      BusinessName: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(100)]],
      BusinessNTN: ['', [Validators.required, Validators.minLength(9)]],
      Email: ['', [Validators.required, Validators.email]],
      Mobile: ['', [Validators.required, Validators.minLength(12)]],
      Password: ['', [Validators.required, Validators.minLength(8), Validators.pattern('^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&\\-\' ]+$')]],
      CPassword: ['', [Validators.required, Validators.minLength(8), Validators.pattern('^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&\\-\' ]+$')]],
      IsHeadOffice: ['Branch Office']
    });
    this.signUp.valueChanges.subscribe((formValues) => {
   
      this.CheckMatchChanged(formValues);
    });
    this.signUp.get('BusinessName').valueChanges.subscribe((value) => {
      if (!this.isValidText(value)) {
        this.signUp.controls.BusinessName.setErrors({ customError: "Business name should only contain text." });
      }
      this.CheckNTN()
    });
    this.signUp.get('BusinessNTN').valueChanges.subscribe((value) => {
      this.CheckNTN()
    });


  }

  onFormateChange(){
    debugger;
    if(this.NTNFormat==2){
      this.signUp.get('BusinessNTN').setValidators([Validators.required, Validators.minLength(7)])
    }else{
      this.signUp.get('BusinessNTN').setValidators([Validators.required, Validators.minLength(9)])
    }
    this.signUp.get('BusinessNTN').updateValueAndValidity();
  }
  onFormateChange() {
    debugger;
    if (this.NTNFormat == 2) {
      this.signUp.get('BusinessNTN').setValidators([Validators.required, Validators.minLength(7)])
    } else {
      this.signUp.get('BusinessNTN').setValidators([Validators.required, Validators.minLength(9)])
    }
    this.signUp.get('BusinessNTN').updateValueAndValidity();
  }
  CheckMatchChanged(formValues: any) {
    if (formValues.Password != formValues.CPassword) {
      this.signUp.controls.CPassword.setErrors({ customError: 'Password and Confirm Password must be the same' });
    }
  }
  isValidText(input: string): boolean {
    const regex = /^[a-zA-Z ]*$/;
    return regex.test(input);
  }
  CheckNTN() {
    if (this.signUp.get('BusinessName').value != '' && this.signUp.get('BusinessNTN').value != '') {
      const data = {
        BusinessNTN: this.signUp.get('BusinessNTN').value,
        BusinessName: this.signUp.get('BusinessName').value
      }
      this.ComSrv.postNoAuth('api/Users/CheckNTN', data)
        .subscribe((response: any) => {
          // if (response) {
          if (response == 1) {
            this.signUp.controls.IsHeadOffice.setValue('Branch Office')
            this.signUp.controls.BusinessName.setErrors(null);
          }
          if (response == 0) {
            this.signUp.controls.IsHeadOffice.setValue('Head Office')
            this.signUp.controls.BusinessName.setErrors(null);
          }
          if (response == 2) {
            this.signUp.controls.BusinessName.setErrors({ customError: "Business Name and NTN already existed" });
            this.ComSrv.ShowError("Business Name and NTN already existed.Please Use Login interface");
          }
          // }
        }, error => {
          this.ComSrv.ShowError(error);
        })
    }
  }
  CheckEmail() {
    if (this.signUp.get('Email').valid) {
      this.ComSrv.postNoAuth('api/Users/CheckEmail', this.signUp.value)
        .subscribe((response: any) => {
          if (response) {
            if (response == 1) {
              console.log(response)
              this.signUp.controls.Email.setErrors({ customError: "Email already existed" });
              this.ComSrv.ShowError("Email already existed.Please Use another email");
            }
          }
        }, error => {
          this.ComSrv.ShowError(error);
        })
    }
  }
  onEnterKey(event: KeyboardEvent): void {
    event.preventDefault();
  }
  IsDisabled = false
  Save() {
    if (!this.isValidText(this.signUp.get('BusinessName').value)) {
      this.ComSrv.ShowError('Business name should only contain text.')
      return
    }
    if (this.signUp.valid && [7, 9].includes(this.signUp.get('BusinessNTN').value.length)) {
      this.IsDisabled = true
      this.ComSrv.postNoAuth('api/Users/SignUp', this.signUp.value)
        .subscribe((response: any) => {
          if (response) {
            this.ComSrv.openSnackBar("Saved data");
            this.ComSrv.setMessage(response)
            this.IsDisabled = false
            this.router.navigate(['verify-otp']);
          }
        })
      // this.signUp.reset();
    } else {
      this.ComSrv.ShowError("please enter valid data");
    }
  }
  getErrorMessage(errorKey: string, errorValue: any): string {
    debugger
    const error = errorValue.requiredLength != 8 && errorValue.requiredLength != 7 ? errorValue.requiredLength - 1 : errorValue.requiredLength
    const errorMessages = {
      required: 'This field is required.',
      minlength: `This field must be at least ${error} long.`,
      maxlength: `This field's text exceeds the specified maximum length.  (maxLength: ${errorValue.requiredLength} characters)`,
      email: 'Invalid email address.',
      pattern: 'Password contain 1 uppercase, 1 lowercase, 1 number, 1 special character (@$!%*?&)',
      customError: errorValue
    };
    return errorMessages[errorKey];
  }
  setFocusOnControl(controlName: string) {
    const control = this.el.nativeElement.querySelector(`[formControlName=${controlName}]`);
    this.renderer.selectRootElement(control).focus();
  }
  ngAfterViewInit(): void {
    setTimeout(() => {
      this.setFocusOnControl("BusinessNTN");
    });
  }
  
}
