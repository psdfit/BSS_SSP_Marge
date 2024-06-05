import { Directive, HostListener } from '@angular/core';

@Directive({
  selector: '[NoDoubleClick]'
})
export class NoDoubleClickDirective {

  constructor() { }

  @HostListener('click', ['$event'])
  clickEvent(event) {
    event.srcElement.parentElement.setAttribute('disabled', true);
    setTimeout(function () {
      event.srcElement.parentElement.removeAttribute('disabled');
    }, 1000);
  }

}
