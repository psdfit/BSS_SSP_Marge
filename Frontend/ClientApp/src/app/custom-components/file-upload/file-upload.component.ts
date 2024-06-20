/* **** Aamer Rehman Malik *****/
import { Component, forwardRef, Input, OnInit } from '@angular/core';
import { NG_VALUE_ACCESSOR, ControlValueAccessor } from '@angular/forms';
import { CommonSrvService } from '../../common-srv.service';
@Component({
  selector: 'file-upload',
  templateUrl: 'file-upload.component.html',
  styleUrls: ['file-upload.component.scss'],
  providers: [{
    provide: NG_VALUE_ACCESSOR,
    useExisting: forwardRef(() => FileUploadComponent),
    multi: true,
  }],
  inputs: ['activeColor', 'baseColor', 'overlayColor']
})
export class FileUploadComponent implements ControlValueAccessor, OnInit {
  
  activeColor: string = 'green';
  baseColor: string = '#ccc';
  overlayColor: string = 'rgba(255,255,255,0.5)';
  id = 'file' + Math.random().toString(36).substring(2);
  dragging: boolean = false;
  loaded: boolean = false;
  imageLoaded: boolean = false;
  FileName: string = '';
  imageSrc: string = '';
  imgSize: number = null;
  imgSizeNote: string = '';
  imgTypeNote: string = '';
  maxSizeInBytes: number = 0;
  iconColor: string = '';
  borderColor: string = '';
  divcss: string = '100px';
  nodivcss: string = 'unset';

  @Input() pattren: string;
  @Input() disabled: boolean;
  @Input() nopreview: boolean;
  @Input() accept: string; 
  @Input() maxSize: string;///kilobytes
  writeValue(obj: any): void {
    if (obj !== undefined) {
      if (obj == null)
        this.imageSrc = '';
      else {
        this.imageSrc = obj;
        this.FileName =  'File Selected';
      }
    }
  }
  propagateChange = (_: any) => { };
  propagateTouch = (_: any) => { };
  constructor(private http: CommonSrvService) {
  }
  ngOnInit() {
    if (!this.accept) {
      this.accept = '*';
    }
    if (this.maxSize) {
      this.maxSizeInBytes = parseInt(this.maxSize) * 1024;
      this.imgSizeNote = 'Maximum upload file size : ' + this.formatBytes(this.maxSizeInBytes);
    }
    if (this.pattren == 'image.*') {
      this.imgTypeNote = 'Allowed only image file (e.g: .jpg, .jpeg, .png)';
    }
    if (this.pattren == 'image.*,pdf') {
      this.imgTypeNote = 'Allowed only image file (e.g: .jpg, .jpeg, .png,.pdf)';
    }
  }
  registerOnChange(fn) {
    this.propagateChange = fn;
  }

  registerOnTouched(fn) { this.propagateTouch = fn; }
  setDisabledState?(isDisabled: boolean): void {
    this.disabled = isDisabled;
  }

  handleDragEnter() {
    console.log("handleDragEnter");
    this.dragging = true;
  }

  handleDragLeave() {
    console.log("handleDragLeave");
    this.dragging = false;
  }

  handleDrop(e) {
    e.preventDefault();
    this.dragging = false;
    this.handleInputChange(e);
  }

  handleImageLoad() {
    this.imageLoaded = true;
    this.iconColor = this.overlayColor;
  }
  cancel(filectl: HTMLFormElement) {
    this.imageSrc = '';
    filectl.value = "";
    this.propagateChange(this.imageSrc);
    this.propagateTouch(this.imageSrc);
  }
  handleInputChange(e) {
    console.log("input change");
    var file:File = e.dataTransfer ? e.dataTransfer.files[0] : e.target.files[0];
    if (file != undefined) {
      console.log('type', file.type);
      console.log('size', file.size);

      //this.imgSize = file.size / 1024;

      if (this.maxSizeInBytes > 0) {
        if (file.size > this.maxSizeInBytes) {
          this.http.ShowError(this.imgSizeNote);
          this.cancel(e.target);
          return;
        }
      }
      var reader = new FileReader();
      if (this.pattren != undefined) {
        if (!file.type.match(this.pattren)) {
          this.http.ShowError('Invalid file format!');
          this.cancel(e.target);
          return;
        }
      }
      this.loaded = false;
      reader.onload = this._handleReaderLoaded.bind(this);
      this.FileName = file.name;
      reader.readAsDataURL(file);
    }
  }

  _handleReaderLoaded(e) {
    console.log("_handleReaderLoaded");
    var reader = e.target;

    this.imageSrc = reader.result;
    this.propagateChange(this.imageSrc);
    this.propagateTouch(this.imageSrc);
    this.loaded = true;
  }

  _setActive() {
    this.borderColor = this.activeColor;
    if (this.imageSrc.length === 0) {
      this.iconColor = this.activeColor;
    }
  }

  _setInactive() {
    this.borderColor = this.baseColor;
    if (this.imageSrc.length === 0) {
      this.iconColor = this.baseColor;
    }
  }

  formatBytes(bytes, decimals = 2) {
    if (bytes === 0) return '0 Bytes';

    const k = 1024;
    const dm = decimals < 0 ? 0 : decimals;
    const sizes = ['Bytes', 'KB', 'MB', 'GB', 'TB', 'PB', 'EB', 'ZB', 'YB'];

    const i = Math.floor(Math.log(bytes) / Math.log(k));

    return parseFloat((bytes / Math.pow(k, i)).toFixed(dm)) + ' ' + sizes[i];
  }
}
