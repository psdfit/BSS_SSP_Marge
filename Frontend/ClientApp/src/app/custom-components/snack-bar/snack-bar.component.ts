/* ****Aamer Rehman Malik *****/
import { Component, OnInit, Inject } from '@angular/core';

import { MAT_SNACK_BAR_DATA } from '@angular/material/snack-bar';
@Component({
  selector: 'snack-bar',
  templateUrl: './snack-bar.component.html',
  styleUrls: ['./snack-bar.component.scss']
})
export class SnackBarComponent implements OnInit {
  public title: string;
  public message: string;

  constructor(@Inject(MAT_SNACK_BAR_DATA) public data: any) {
    console.log(data);
  }

  ngOnInit() { }

  get getIcon() {
    switch (this.data.snackType) {
      case 'Success':
        return 'done';
      case 'Error':
        return 'error';
      case 'Warn':
        return 'warning';
      case 'Info':
        return 'info';
    }
  }
}
