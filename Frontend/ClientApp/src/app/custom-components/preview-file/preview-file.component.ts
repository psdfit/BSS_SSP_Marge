import { Component, OnInit, Inject, AfterViewInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
@Component({
  selector: 'app-preview-file',
  templateUrl: './preview-file.component.html',
  styleUrls: ['./preview-file.component.scss']
})
export class PreviewFileComponent implements OnInit, AfterViewInit {
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
    this.DataSource = this.sanitizer.bypassSecurityTrustResourceUrl(blobUrl);
  }

  onCloseClick(): void {
    this.dialogRef.close();
  }

  ngAfterViewInit(): void {
    const iframe = document.getElementById('myIframe') as HTMLIFrameElement;

    iframe.onload = () => {
      const iframeDocument = iframe.contentDocument || iframe.contentWindow?.document;

      if (iframeDocument) {
        // Create a <style> element
        const style = iframeDocument.createElement('style');
        style.textContent = `
          img {
            width: 100%;
           height: 100%
          }
        `;
        // Append the <style> element to the <head> of the iframe document
        iframeDocument.head.appendChild(style);
      }
    };
  }

}
