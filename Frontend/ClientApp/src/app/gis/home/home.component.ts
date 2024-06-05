import { Component, OnInit } from '@angular/core';
import * as L from 'leaflet';
import { BehaviorSubject } from 'rxjs';
import { CommonSrvService } from 'src/app/common-srv.service';
import { geoJSON } from 'leaflet';
import { FeatureCollection } from 'geojson';

var map, greenIcon, markers, districtLayer, provinceLayer, tehsilLayer;

declare const provinceGeojson: FeatureCollection;
declare const districtGeojson: FeatureCollection;
declare const tehsilsGeojson: FeatureCollection;

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {

  District: any;
  MainDistrict: any;
  Tehsil: any;
  MainTehsil: any;
  Sector: any;
  Clustor: any;
  ProgramType: any;
  cclass: number;

  selectedDistrict: any;

  error: String;
  markerObject: Array<any>;
  makeMarker = new BehaviorSubject([]);
  filters: IQueryFilters = { PTypeID: 0, SectorID: 0, ClusterID: 0, DistrictID: 0, TehsilID: 0 };



  PTypeID: number;
  sectorid: number;
  clusterid: number;
  districtid: number;
  tehsilid: number;



  constructor(
    private http: CommonSrvService,
  ) { }

  ngOnInit(): void {
    this.http.setTitle("GIS Map");


    this.InitlizeMap();
    this.GetProgramType();
    this.getSector();
    this.getCluster();
    this.getDistrict();
    this.getTehsil();
    this.GetData();


    // this.getClassData();
    // this.setMarkersToMapInit();
  }
  InitlizeMap() {
    var mbAttr = 'Map data &copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors, ' +
      'Imagery © <a href="https://www.mapbox.com/">Mapbox</a>',
      mbUrl = 'https://api.mapbox.com/styles/v1/{id}/tiles/{z}/{x}/{y}?access_token=pk.eyJ1IjoibWFwYm94IiwiYSI6ImNpejY4NXVycTA2emYycXBndHRqcmZ3N3gifQ.rJcFIG214AriISLbB6B5aw';

    var grayscale = L.tileLayer(mbUrl, { id: 'mapbox/light-v9', tileSize: 512, zoomOffset: -1, attribution: mbAttr }),
      streets = L.tileLayer(mbUrl, { id: 'mapbox/streets-v11', tileSize: 512, zoomOffset: -1, attribution: mbAttr }),
      googleStreets = L.tileLayer('http://{s}.google.com/vt/lyrs=m&x={x}&y={y}&z={z}', {
        maxZoom: 20,
        subdomains: ['mt0', 'mt1', 'mt2', 'mt3']
      }),
      googleHybrid = L.tileLayer('http://{s}.google.com/vt/lyrs=s,h&x={x}&y={y}&z={z}', {
        maxZoom: 20,
        subdomains: ['mt0', 'mt1', 'mt2', 'mt3']
      }),
      googleSat = L.tileLayer('http://{s}.google.com/vt/lyrs=s&x={x}&y={y}&z={z}', {
        maxZoom: 20,
        subdomains: ['mt0', 'mt1', 'mt2', 'mt3']
      }),
      googleTerrain = L.tileLayer('http://{s}.google.com/vt/lyrs=p&x={x}&y={y}&z={z}', {
        maxZoom: 20,
        subdomains: ['mt0', 'mt1', 'mt2', 'mt3']
      });

    map = L.map('map', {
      center: [30.5, 70.0],
      zoom: 6,
      layers: [googleStreets]
    });

    provinceLayer = L.geoJSON(provinceGeojson, {

      style: {
        "color": "#FF8E23",
        "weight": 3,
        "opacity": 0.65
      },

      // onEachFeature: onEachFeature,

      pointToLayer: function (feature, latlng) {
        return L.circleMarker(latlng, {
          radius: 8,
          fillColor: "#FF8E23",
          color: "#000",
          weight: 1,
          opacity: 1,
          fillOpacity: 0.8
        });
      }
    }).addTo(map);

    districtLayer = L.geoJSON(districtGeojson, {

      style: {
        "color": "#009AC4",
        "weight": 3,
        "opacity": 0.65
      },

      // onEachFeature: onEachFeature,

      pointToLayer: function (feature, latlng) {
        return L.circleMarker(latlng, {
          radius: 8,
          fillColor: "#ff7800",
          color: "#000",
          weight: 1,
          opacity: 1,
          fillOpacity: 0.8
        });
      }
    });

    tehsilLayer = L.geoJSON(tehsilsGeojson, {

      style: {
        "color": "#00C43E",
        "weight": 3,
        "opacity": 0.65
      },

      // onEachFeature: onEachFeature,

      pointToLayer: function (feature, latlng) {
        return L.circleMarker(latlng, {
          radius: 8,
          fillColor: "#00C43E",
          color: "#000",
          weight: 1,
          opacity: 1,
          fillOpacity: 0.8
        });
      }
    });

    let baseLayers: any = {
      "Google Street": googleStreets,
      "Google Hybrid": googleHybrid,
      "Google Satellite": googleSat,
      "Google Terrain": googleTerrain
    };

    var overlays = {
      "Province Boundry": provinceLayer,
      "District Boundry": districtLayer,
      "Tehsil Boundry": tehsilLayer
    };

    L.control.layers(baseLayers, overlays, { collapsed: false }).addTo(map);

    greenIcon = L.icon({
      iconUrl: './assets/marker-red.png',

      iconSize: [38, 38], // size of the icon
    });
  }
  Changemarker() {
    debugger;
    this.removeMarkers();
    this.GetData();
  }

  GetData() {
    this.http.postJSON('api/GIS/GetClassWithTraineeCount', { PTypeID: this.filters.PTypeID, SectorID: this.filters.SectorID, ClusterID: this.filters.ClusterID, DistrictID: this.filters.DistrictID, TehsilID: this.filters.TehsilID }).subscribe((d: any) => {
      debugger;

      //this.removeMarkers();
      this.addingMarkers(d[0]);
    }, error => this.error = error // error path
    );
  }


  GetProgramType() {
    this.http.getJSON('api/ProgramType/GetProgramType').subscribe((d: any) => {
      this.ProgramType = d;
    }, error => this.error = error // error path
    );
  }

  getSector() {
    this.http.getJSON('api/Sector/GetSector').subscribe((d: any) => {
      this.Sector = d;
    }, error => this.error = error // error path
    );
  }

  getCluster() {
    this.http.getJSON('api/Cluster/GetCluster').subscribe((d: any) => {
      this.Clustor = d;
    }, error => this.error = error // error path
    );
  }
  getDistrict() {
    debugger;
    this.filters.TehsilID = 0
    this.http.postJSON('api/District/RD_DistrictBy', { clusterid: this.filters.ClusterID }).subscribe((d: any) => {
      this.District = d;
    }, error => this.error = error // error path
    );
  }
  getTehsil() {
    debugger;
    this.http.postJSON('api/Tehsil/RD_TehsilBy', { districtid: this.filters.DistrictID }).subscribe((d: any) => {
      this.Tehsil = d;
    }, error => this.error = error // error path
    );
  }

  addingMarkers(markerArray) {
    debugger;
    //this.removeMarkers();
    // map.removeLayer();
    var lastMk = null;
    markers = L.layerGroup();
    var customOptions =
    {
      'className': 'popupCustom'
    }
    console.log(markerArray[0]);
    for (var i = 0; i < markerArray.length; i++) {
      let lon: any = markerArray[i].Longitude;
      let lat: any = markerArray[i].Latitude;

      let ClassCount: any = markerArray[i].ClassCount;
      let MaleCount: any = markerArray[i].MaleCount;
      let FemaleCount: any = markerArray[i].FemaleCount;
      let TransgenderCount: any = markerArray[i].TransgenderCount;




      var newTest: any = Object.values(markers._layers);
      // var mk = newTest.find(f => {
      //   if(f._latlng.lat == markerArray[i].Latitude && f._latlng.lng == markerArray[i].Longitude) {return true;} else {return false;}
      // });
      // if(mk){
      //  lastMk = mk;
      // if(typeof markerArray[i].ClassCode != 'undefined') {
      //   if(markerArray[i].GenderName == 'Female') {
      //     cfemale += 1;
      //   } else if(markerArray[i].GenderName == 'Male') {
      //     cmale += 1;
      //   } else if(markerArray[i].GenderName == 'Both') {
      //     cboth += 1;
      //   }
      //   this.cclass += 1;
      // }
      // } else {
      // if(markerArray[i].GenderName == 'Female') {
      //   cfemale = 1;
      // } else if(markerArray[i].GenderName == 'Male') {
      //   cmale = 1;
      // } else if(markerArray[i].GenderName == 'Both') {
      //   cboth = 1;
      // }
      // if(lastMk == null) {
      // lastMk._popup._content += "<div>Total Classes: "+ClassCount+"</div>";
      // "<div>Total Classes: "+ClassCount+"</div>";
      // }
      let popupText = "<div>Total Classes: " + ClassCount + "</div>"
        + "<div>Male: " + MaleCount + ", Female: " + FemaleCount + ", Transgender: " + TransgenderCount + "</div><table class='table' style='border-bottom:2px solid black;'>"
        //+ "<tr><td>Class Code</td><td>"+markerArray[i].ClassCode+"</td></tr>"
        + "<tr><td>TSP Name</td><td>" + markerArray[i].TSPName + "</td></tr>"
        + "<tr><td>Cluster Name</td><td>" + markerArray[i].ClusterName + "</td></tr>"
        // + "<tr><td>Gender Name</td><td>"+markerArray[i].GenderName+"</td></tr>"
        + "<tr><td>Sector Name</td><td>" + markerArray[i].SectorName + "</td></tr>"
        // + "<tr><td>Trade Name</td><td>"+markerArray[i].TradeName+"</td></tr>"
        + "<tr><td>Training Address Location</td><td>" + markerArray[i].TrainingAddressLocation + "</td></tr>"
        + "</table>";

      var markerLocation = new L.LatLng(lat, lon);
      var marker = new L.Marker(markerLocation, { icon: greenIcon });
      marker.on('click', (e: any) => {
        map.setView(e.latlng, 10);
      });
      // popupText = "<div>Total Classes: "+this.cclass+"</div>" + popupText;

      markers.addLayer(marker);
      marker.bindPopup(popupText, customOptions);
    }
    // }
    map.addLayer(markers);
  }

  // setMarkersToMapInit() {
  //   this.makeMarker.subscribe((data: any) => {
  //     this.markerObject = data;
  //     this.addingMarkers(data);
  //   });
  // }

  // getClassData() {
  //   this.http.getJSON('api/Class/GetClass').subscribe((d: any) => {
  //     this.makeMarker.next(d[0]);
  //   }, error => this.error = error // error path
  //   );
  // }

  // selectDistrict(district) {
  //   if(district == '%') {
  //     map.removeLayer(districtLayer);
  //     map.removeLayer(tehsilLayer);
  //     map.addLayer(provinceLayer);
  //     this.Tehsil = "";
  //     districtLayer.eachLayer(function(layer){
  //       layer.setStyle({color :'#009AC4'});
  //     });
  //     map.setView(new L.LatLng(30.5, 70.0), 6);
  //     this.removeMarkers();
  //     this.setMarkersForDistrict(district);
  //   } else {
  //     this.selectedDistrict = {
  //       DistrictID: district.DistrictID,
  //       DistrictName: district.DistrictName
  //     }
  //     console.log(district);
  //     map.removeLayer(provinceLayer);
  //     map.removeLayer(tehsilLayer);
  //     map.addLayer(districtLayer);
  //     this.Tehsil = this.MainTehsil.filter(tehs => tehs.DistrictID == district.DistrictID);
  //     districtLayer.eachLayer(function(layer){
  //       layer.setStyle({color :'#009AC4'});
  //       if(layer.feature.properties.ADM2_EN == district.DistrictName) {
  //         map.fitBounds(layer.getBounds());
  //         layer.setStyle({color :'cyan'});
  //       }
  //     });
  //     this.removeMarkers();
  //     this.setMarkersForDistrict(district.DistrictID); 
  //   }
  // }

  // selectTehsil(tehsil) {
  //   if(tehsil == '%') {
  //     console.log(this.selectedDistrict);
  //     map.removeLayer(provinceLayer);
  //     map.removeLayer(tehsilLayer);
  //     map.addLayer(districtLayer);
  //     this.Tehsil = this.MainTehsil.filter(tehs => tehs.DistrictID == this.selectedDistrict.DistrictID);
  //     var ddn = this.selectedDistrict.DistrictName;
  //     districtLayer.eachLayer(function(layer){
  //       layer.setStyle({color :'#009AC4'});
  //       if(layer.feature.properties.ADM2_EN == ddn) {
  //         map.fitBounds(layer.getBounds());
  //         layer.setStyle({color :'cyan'});
  //       }
  //     });
  //     this.removeMarkers();
  //     this.setMarkersForDistrict(this.selectedDistrict.DistrictID); 
  //   } else {
  //     console.log(tehsil);
  //     this.removeMarkers();
  //     this.setMarkersForTehsil(tehsil.TehsilID);

  //     map.removeLayer(provinceLayer);
  //     map.removeLayer(districtLayer);
  //     map.addLayer(tehsilLayer);
  //     tehsilLayer.eachLayer(function(layer){
  //       layer.setStyle({color :'#00C43E'});
  //       if(layer.feature.properties.ADM3_EN == tehsil.TehsilName) {
  //         console.log(layer);
  //         map.fitBounds(layer.getBounds());
  //         layer.setStyle({color :'cyan'});
  //       }
  //     });
  //   }
  // }

  // selectSector(sector) {
  //   if(sector == '%') {
  //     this.removeMarkers();
  //     this.setMarkersForSector(sector);
  //     map.setView(new L.LatLng(30.5, 70.0), 6);
  //   } else {
  //     this.removeMarkers();
  //     this.setMarkersForSector(sector.SectorID);
  //   }
  // }

  // selectCluster(clustor) {
  //   console.log(clustor);
  //   if(clustor == '%') {
  //     this.removeMarkers();
  //     this.setMarkersForClustor(clustor);
  //   } else {
  //     this.District = this.MainDistrict.filter(clus => clus.ClusterID == clustor.ClusterID);
  //     this.removeMarkers();
  //     this.setMarkersForClustor(clustor.ClusterID);
  //   }
  // }

  removeMarkers() {
    map.removeLayer(markers);
  }

  // setMarkersForDistrict(id) {
  //   if(id == '%') {
  //     var newMarkers = this.markerObject;
  //     this.addingMarkers(newMarkers);
  //   } else {
  //     var newMarkers = this.markerObject.filter(d => d.DistrictID === id);
  //     console.log(newMarkers);
  //     this.addingMarkers(newMarkers);
  //   }
  // }

  // setMarkersForClustor(id) {
  //   if(id == '%') {
  //     var newMarkers = this.markerObject;
  //     this.addingMarkers(newMarkers);
  //   } else {
  //     var newMarkers = this.markerObject.filter(d => d.ClusterID === id);
  //     this.addingMarkers(newMarkers);
  //   }
  // }

  // setMarkersForSector(id) {
  //   if(id == '%') {
  //     var newMarkers = this.markerObject;
  //     this.addingMarkers(newMarkers);
  //   } else {
  //     var newMarkers = this.markerObject.filter(d => d.SectorID === id);
  //     console.log(newMarkers);
  //     this.addingMarkers(newMarkers);
  //   }
  // }

  // setMarkersForTehsil(id) {
  //   if(id == '%') {
  //     var newMarkers = this.markerObject;
  //     this.addingMarkers(newMarkers);
  //   } else {
  //     var newMarkers = this.markerObject.filter(d => d.TehsilID === id);
  //     console.log(newMarkers);
  //     this.addingMarkers(newMarkers);
  //   }
  // }

}


export interface IQueryFilters {
  PTypeID: number;
  SectorID: number;
  ClusterID: number;
  DistrictID: number;
  TehsilID: number;
}
