import { Directive, HostListener, ElementRef } from '@angular/core';
import { TitleCasePipe } from '@angular/common';
import { NgControl } from '@angular/forms'

@Directive({
  selector: '[TitleCase]'
  , providers: [TitleCasePipe]
})
export class TitleCaseDirective {
  constructor(private el: ElementRef, private ngControl: NgControl, private titleCasePipe: TitleCasePipe) { }
  @HostListener('ngModelChange', ['$event'])
  onInputChange(value) {
    const newValue = this.titleCasePipe.transform(value)
    //console.log(newValue);
    //this.ngControl.valueAccessor.writeValue(newValue);
    this.el.nativeElement.value = newValue;
    if (newValue && newValue != '') {
      // debugger
      this.ngControl.control.setValue(newValue, { emitViewToModelChange: false });
    }
  }
  private transform(value: string) {
    value = value.replace(/\s+/g, " ");
    let words = value.split(" ");
    for (var i = 0; i < words.length; i++) {
      let word: string = words[i];
      words[i] = this.toTitleCase(word);
    }
    return words.join(" ");
  }
  private toTitleCase(word: string): string {
    return word.substr(0, 1).toUpperCase() + word.substr(1).toLowerCase();
  }
}
