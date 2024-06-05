import { Pipe, PipeTransform } from '@angular/core';
import { IMaskPipe } from 'angular-imask';
import * as IMask from 'imask';

@Pipe({
  name: 'phoneNumber'
})
export class PhoneNumberPipe implements PipeTransform {

  constructor(private iMaskPipe: IMaskPipe) {

  }
  transform(value: string,format:string): unknown {
    return this.iMaskPipe.transform(value, new IMask.MaskedPattern({ mask: format }));
  }

}
