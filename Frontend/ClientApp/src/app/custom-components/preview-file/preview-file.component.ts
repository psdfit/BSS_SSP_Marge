import { Component, OnInit, Inject, AfterViewInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
@Component({
  selector: 'app-preview-file',
  templateUrl: './preview-file.component.html',
  styleUrls: ['./preview-file.component.scss']
})
export class PreviewFileComponent implements OnInit {
  public content: any;
  public content1: any;
  DataSource: SafeResourceUrl;
  constructor(
    public dialogRef: MatDialogRef<PreviewFileComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private sanitizer: DomSanitizer
  ) { }
  ngOnInit(): void {
    this.getBlobUrl()
  }
 

  getBlobUrl() {
    const blob = this.data.blobData;
    const blobUrl = URL.createObjectURL(blob);
    this.DataSource= this.sanitizer.bypassSecurityTrustResourceUrl(blobUrl);
  }

  onCloseClick(): void {
    this.dialogRef.close();
  }
}
