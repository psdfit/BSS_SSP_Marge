import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
@Component({
  selector: 'hrapp-printpreview',
  templateUrl: './printpreview.component.html',
  styleUrls: ['./printpreview.component.scss']
})
export class PrintpreviewComponent implements OnInit {
  public content: any;

  constructor(public dialogRef: MatDialogRef<PrintpreviewComponent>,
    @Inject(MAT_DIALOG_DATA) public data: Blob[]) {
    this.content = data;
  }

  ngOnInit() {
  }
}
