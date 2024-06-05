import { Directive, ElementRef, HostListener, OnInit } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { FormGroup } from '@angular/forms';
@Directive({
  selector: '[UrduText]'
})
export class UrduTextDirective implements OnInit {
  input: HTMLInputElement;
  fullFieldSelected = false;

  ngOnInit() {
  }

  constructor(el: ElementRef) {
    this.input = el.nativeElement;
  }

  m = { "q": "ق", "w": "و", "e": "ع", "r": "ر", "t": "ت", "y": "ے", "u": "ء", "i": "ی", "o": "ہ", "p": "پ", "a": "ا", "s": "س", "d": "د", "f": "ف", "g": "گ", "h": "ح", "j": "ج", "k": "ک", "l": "ل", "z": "ز", "x": "ش", "c": "چ", "v": "ط", "b": "ب", "n": "ن", "m": "م", "`": "ً", ",": "،", ".": "۔", "Q": "ْ", "W": "ّ", "E": "ٰ", "R": "ڑ", "T": "ٹ", "Y": "َ", "U": "ئ", "I": "ِ", "O": "ۃ", "P": "ُ", "A": "آ", "S": "ص", "D": "ڈ", "G": "غ", "H": "ھ", "J": "ض", "K": "خ", "Z": "ذ", "X": "ژ", "C": "ث", "V": "ظ", "N": "ں", "M": "٘", "~": "ٍ", "?": "؟", "F": "ٔ", "L": "ل", "B": "ب" };
  // urduNumerals = { "0": "۰", "1": "۱", "2": "۲", "3": "۳", "4": "۴", "5": "۵", "6": "۶", "7": "۷", "8": "۸", "9": "۹" }; //http://www.omniglot.com/language/numbers/urdu.htm
  //options && options.urduNumerals ? $.extend(m, urduNumerals) : null;

  last = '';

  @HostListener('keyup', ['$event', '$event.keyCode'])
  onKeyUp($event: KeyboardEvent, keyCode) {
    // if user trying to do copy & paste, then we don't want to
    // overwrite the value
    if ($event.metaKey || $event.ctrlKey) {
      return;
    }

    //if (keyCode !== TAB) {
    //  $event.preventDefault();
    //}
    let pos = this.input.selectionEnd;
    let s = this.input.value;
    let isLastPos = (pos == s.length);
    if (this.last == s) return;
    var S = [];
    for (var x = 0; x < s.length; x++) {
      var c = s.charAt(x);
      S.push(this.m[c] || c);
    }
    this.input.value = S.join('');
    this.last = this.input.value;
    if (!isLastPos) {
      this.input.selectionStart = this.input.selectionEnd = pos;
    }
    // get value for the key
  }
}


@Directive({
  selector: '[focusInvalidInput]'
})
export class FormDirective {
  constructor(private el: ElementRef) { }

  @HostListener('submit')
  onFormSubmit() {
    const invalidControl = this.el.nativeElement.querySelector('.ng-invalid');

    if (invalidControl) {
      invalidControl.focus();
    }
  }
}
