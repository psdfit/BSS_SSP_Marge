

import { Component, OnInit,Renderer2, AfterViewInit } from '@angular/core';
import * as L from 'leaflet';
import { CommonSrvService } from 'src/app/common-srv.service';
import { MatDialogRef } from '@angular/material/dialog';
import { ConfirmDailogComponent } from 'src/app/custom-components/confirm-dailog/confirm-dailog.component';


// declare const L: any;

@Component({
  selector: 'app-reactive-form',
  templateUrl: './reactive-form.component.html',
  styleUrls: ['./reactive-form.component.scss']
})
export class ReactiveFormComponent implements OnInit,AfterViewInit {
  title = 'locationApp';
  error: any;

 

  constructor(private ComSrv: CommonSrvService,private renderer:Renderer2,public dialogRef: MatDialogRef<ConfirmDailogComponent>) {
    dialogRef.disableClose = true;
  }


  mymap: any;
  ngOnInit() {

    if (!navigator.geolocation) {
      console.log('location is not supported');
    }else{
      navigator.geolocation.getCurrentPosition((position) => {
        const coords = position.coords;
        const latLong: any = [coords.latitude, coords.longitude];
  
        this.ComSrv.setMessage(latLong)
        this.mymap = L.map('map').setView(latLong, 13);
  
        let popup = L.popup()
          .setLatLng(latLong)
          .setContent(`Current Location  at: ${latLong[0]}, ${latLong[1]}`)
          .openOn(this.mymap);
  
        L.tileLayer('https://tile.openstreetmap.org/{z}/{x}/{y}.png', {
          maxZoom: 18,
          tileSize: 512,
          zoomOffset: -1,
          id: 'mapbox/streets-v11'
        }).addTo(this.mymap);
  
  
        let Icon = L.icon({
          iconUrl: './assets/marker-red.png',
  
          iconSize: [38, 38],
        });
        let marker = L.marker(latLong, { icon: Icon }).addTo(this.mymap);
  
        marker.bindPopup(`Current Location: ${latLong[0]}, ${latLong[1]}`).openPopup();
  
        let circle = L.circle(latLong, {
          color: 'red',
          fillColor: '#f03',
          fillOpacity: 0.1,
          radius: 400
        }).addTo(this.mymap);
  
        this.mymap.on('click', (event: L.LeafletMouseEvent) => {
          const clickedLatLng = event.latlng;
  
          // Do something with the captured location
          latLong[0] = clickedLatLng.lat
          latLong[1] = clickedLatLng.lng
          this.ComSrv.setMessage(latLong)
  
  
          let popup = L.popup()
            .setLatLng(latLong)
            .setContent(`Clicked at: ${latLong[0]}, ${latLong[1]}`)
            .openOn(this.mymap);
        });
  
  
  
  
      });
    }


  }


  Confirmation: boolean = false;

Confirm() {
  const title = "Is your selected location correct?";
  const message = "Are you sure?";

  this.ComSrv.confirm(title, message).subscribe(result => {
    console.log(result);

    if (result) {
      this.ComSrv.openSnackBar("Your location is selected.", 'Close');
      this.dialogRef.close(false)
    } else {
      this.ComSrv.ShowError("Please select your location.");
    }
  });
}


addCustomCSS() {
  // Create a style element
  const style = this.renderer.createElement('style');

  // Set your custom CSS rule
  const cssRule = '.leaflet-control-attribution a { text-decoration: none; display: none; }';

  // Add the CSS rule to the style element
  this.renderer.appendChild(style, this.renderer.createText(cssRule));

  // Append the style element to the document head
  this.renderer.appendChild(document.head, style);
}


ngAfterViewInit(): void {
  this.addCustomCSS();

}

}