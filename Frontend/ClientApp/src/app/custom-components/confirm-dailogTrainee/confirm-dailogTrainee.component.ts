/* **** Aamer Rehman Malik *****/
import { Component, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
@Component({
  selector: 'hrapp-confirm-dailogTrainee',
  templateUrl: './confirm-dailogTrainee.component.html',
  styleUrls: ['./confirm-dailogTrainee.component.scss']
})
export class confirmdailogTraineeComponent implements OnInit {
  public title: string;
  public message: string;

  constructor(public dialogRef: MatDialogRef<confirmdailogTraineeComponent>) {
  }

  ngOnInit() {
  }
}
