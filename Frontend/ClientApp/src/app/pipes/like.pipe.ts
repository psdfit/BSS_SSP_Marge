import { Pipe, PipeTransform } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';

@Pipe({
  name: 'like'
})
export class LikePipe implements PipeTransform {
  /**
  * Support a function or a value or the shorthand ['key', value] like the lodash shorthand.
  */
  getProperty(value: { [key: string]: any }, key: string): any {
    const keys: string[] = key.split('.');
    let result: any = value[keys.shift()!];

    for (const key of keys) {
      result = result[key];
    }

    return result;
  }
  transform(input: any, fn: any): any {
    if (!Array.isArray(input)) {
      return input
    }

    if (typeof fn === 'function') {
      return input.filter(fn);
    }
    else if (Array.isArray(fn)) {
      const [key, value] = fn;
      return input.filter((item: any) => item[key].trim().toLocaleLowerCase().indexOf(value.toLocaleLowerCase()) >= 0);
    }
    else if (fn) {
      return input.filter((item: any) => item.trim().toLocaleLowerCase().indexOf(fn.trim().toLocaleLowerCase()) >= 0);
    }
    else {
      return input;
    }
  }
}
