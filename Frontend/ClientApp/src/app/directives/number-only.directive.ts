import { Directive, ElementRef, HostListener, OnChanges, SimpleChanges } from '@angular/core';
import { NgControl } from '@angular/forms';

@Directive({
  selector: '[NumberOnly]'
})
export class NumberOnlyDirective {
  private regex: RegExp = new RegExp(/^[0-9]{0,12}$/g);

  // Allow key codes for special events. Reflect :
  // Backspace, tab, end, home
  private specialKeys: Array<string> = ['Backspace', 'Tab', 'End', 'Home', 'Control', 'c', 'v'];

  constructor(private ngControl: NgControl, private el: ElementRef) {
  }
  @HostListener('ngModelChange', ['$event']) onInputChange(value) {
    if (!String(value).match(this.regex)) {
      this.ngControl.control.setValue('');
      this.ngControl.valueAccessor.writeValue('');
    }
  }
  @HostListener('keydown', ['$event']) onKeyDown(event: KeyboardEvent) {
    // Allow Backspace, tab, end, and home keys
    if (this.specialKeys.indexOf(event.key) !== -1) {
      return;
    }

    // Do not use event.keycode this is deprecated.
    // See: https://developer.mozilla.org/en-US/docs/Web/API/KeyboardEvent/keyCode
    const current: string = this.el.nativeElement.value;
    // We need this because the current value on the DOM element
    // is not yet updated with the value from this event
    const next: string = current.concat(event.key);
    if (next && !String(next).match(this.regex)) {
      event.preventDefault();
    }
  }
}
