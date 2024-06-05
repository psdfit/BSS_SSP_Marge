import { Component, OnInit, Renderer2, AfterViewInit } from '@angular/core';
import * as L from 'leaflet';
import { CommonSrvService } from 'src/app/common-srv.service';
import { MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-geo-tagging',
  templateUrl: './geo-tagging.component.html',
  styleUrls: ['./geo-tagging.component.scss']
})
export class GeoTaggingComponent implements OnInit, AfterViewInit {
  error: any;
  map: L.Map;
  latLong: any;
  LatitudeAndLongitude: any;

  constructor(private ComSrv: CommonSrvService, private renderer: Renderer2, public dialogRef: MatDialogRef<GeoTaggingComponent>) {
    dialogRef.disableClose = true;
  }

  ngOnInit() {
    // No need to initialize the map here, do it in ngAfterViewInit()
  }

  ngAfterViewInit(): void {
    this.InitializeMap();
    this.addCustomCSS();
  }

  InitializeMap() {
    if (!navigator.geolocation) {
      // Handle error gracefully, display error message in UI
      this.error = 'Geolocation is not supported.';
      return;
    }

    navigator.geolocation.getCurrentPosition((position) => {
      const coords = position.coords;
      this.latLong = [coords.latitude, coords.longitude];
      this.ComSrv.setMessage(this.latLong);

      const mbAttr = 'Map data &copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors, ' +
        'Imagery © <a href="https://www.mapbox.com/">Mapbox</a>';

      const googleStreets = L.tileLayer('http://{s}.google.com/vt/lyrs=m&x={x}&y={y}&z={z}', {
        maxZoom: 20,
        subdomains: ['mt0', 'mt1', 'mt2', 'mt3']
      });

      this.map = L.map('map', {
        center: this.latLong,
        zoom: 13,
        layers: [googleStreets]
      });

      const popup = L.popup()
        .setLatLng(this.latLong)
        .setContent(`Current Location at: ${this.latLong[0]}, ${this.latLong[1]}`)
        .openOn(this.map);

      this.map.on('click', (event: L.LeafletMouseEvent) => {
        const clickedLatLng = event.latlng;
        this.latLong = [clickedLatLng.lat, clickedLatLng.lng];
        this.ComSrv.setMessage(this.latLong);
        const popup = L.popup()
          .setLatLng(this.latLong)
          .setContent(`Clicked at: ${this.latLong[0]}, ${this.latLong[1]}`)
          .openOn(this.map);
      });
    });
  }

  ManualAdd(value) {
    const splitData = value.split(',');
    const mappedData = splitData.map(d => Number(d));
    this.ComSrv.setMessage(mappedData);
    const popup = L.popup()
      .setLatLng(mappedData)
      .setContent(`Clicked at: ${splitData[0]}, ${splitData[1]}`)
      .openOn(this.map);
  }

  Confirm() {
    const title = 'Is your selected location correct?';
    const message = 'Are you sure?';
    this.ComSrv.confirm(title, message).subscribe(result => {
      if (result) {
        this.ComSrv.openSnackBar('Your location is selected.', 'Close');
        this.dialogRef.close(false);
      } else {
        this.dialogRef.close(false);
        this.ComSrv.ShowError('Please manually add your location.');
      }
    });
  }

  addCustomCSS() {
    const style = this.renderer.createElement('style');
    const cssRule = '.leaflet-control-attribution a { text-decoration: none; display: none; }';
    this.renderer.appendChild(style, this.renderer.createText(cssRule));
    this.renderer.appendChild(document.head, style);
  }
}
