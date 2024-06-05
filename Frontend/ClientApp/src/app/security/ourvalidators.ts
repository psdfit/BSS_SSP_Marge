import { AbstractControl, AsyncValidatorFn, ValidationErrors, FormGroup, ValidatorFn } from '@angular/forms';
import { Observable } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { CommonSrvService } from '../common-srv.service';

export function uniqueTitle(pkey: FormGroup,pKeyFld:string, URL: string, srv: CommonSrvService): AsyncValidatorFn {
  return (control: AbstractControl): Promise<ValidationErrors | null> | Observable<ValidationErrors | null> => {

    return srv.postJSON(URL, {
      "pKey": pkey.get(pKeyFld).value, "title": control.value
    }).pipe(map(response => { return response==false ? null : { unique: true } }));
  }
}

export function confirmPasswordValidator(confirmPassword: string): ValidatorFn {
  return (control: AbstractControl): { [key: string]: any } | null => {
    let password: string = control.value;
    let isInValid = (password !== confirmPassword) ? true : false;
    return isInValid ? { 'cnfPassword': { value: 'Invalid' } } : null;
  };
} 
