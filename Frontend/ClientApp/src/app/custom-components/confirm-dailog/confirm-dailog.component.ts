/* **** Aamer Rehman Malik *****/
import { Component, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
@Component({
  selector: 'hrapp-confirm-dailog',
  templateUrl: './confirm-dailog.component.html',
  styleUrls: ['./confirm-dailog.component.scss']
})
export class ConfirmDailogComponent implements OnInit {
  public title: string;
  public message: string;

  constructor(public dialogRef: MatDialogRef<ConfirmDailogComponent>) {
    dialogRef.disableClose = true;

  }

  ngOnInit() {
  }
}
