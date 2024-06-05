import { Pipe, PipeTransform, NgModule } from '@angular/core';

@Pipe({
  name: 'groupBy',
})
export class GroupByPipe implements PipeTransform {
  getProperty(value: { [key: string]: any }, key: string): any {
    const keys: string[] = key.split('.');
    let result: any = value[keys.shift()!];

    for (const key of keys) {
      result = result[key];
    }

    return result;
  }

  transform(input: any, prop: string): Array<any> {
    if (!Array.isArray(input)) {
      return input;
    }
   
    const arr: { [key: string]: Array<any> } = {};

    for (const value of input) {
      const field: any = this.getProperty(value, prop);

      if (!arr[field]) {
        arr[field] = [];
      }

      arr[field].push(value);
    }

    return Object.keys(arr).map(key => ({ key, value: arr[key] }));
  }
}

//@NgModule({
//  declarations: [GroupByPipe],
//  exports: [GroupByPipe],
//})
//export class NgGroupByPipeModule {}
