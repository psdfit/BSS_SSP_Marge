import { Component, Inject, OnInit, Optional, ViewChild } from '@angular/core';
import { FormGroup, FormBuilder, FormControl, Validators, FormGroupDirective } from '@angular/forms';
import { CommonSrvService } from '../../common-srv.service';
import * as Highcharts from 'highcharts';
import { ModelBase } from '../../shared/ModelBase';
import { MatTabGroup } from '@angular/material/tabs';
import { ActivatedRoute } from '@angular/router';
import { UsersModel } from 'src/app/master-data/users/users.component';
import { TraineeJourneyComponent } from '../trainee-journey/trainee-journey.component';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

declare var require: any;
const Boost = require('highcharts/modules/boost');
const noData = require('highcharts/modules/no-data-to-display');
const More = require('highcharts/highcharts-more');
const Timeline = require('highcharts/modules/timeline');

Boost(Highcharts);
noData(Highcharts);
More(Highcharts);
Timeline(Highcharts);
noData(Highcharts);

@Component({
  selector: 'app-class-journey',
  templateUrl: './class-journey.component.html',
  styleUrls: ['./class-journey.component.scss']
})

export class ClassJourneyComponent implements OnInit {

  @ViewChild('tabGroup') tabGroup: MatTabGroup;
  classform: FormGroup;
  working: boolean;

  Schemes: any[];
  TSPs: any[];
  Classes: any[];
  ClassFinance: any[];

  SelectedScheme: number;
  SelectedTSP: number;
  SelectedClass: number;
  currentUser: UsersModel;
  classID: string;
  CloseButtonHide=false;

  constructor(private fb: FormBuilder, private commonService: CommonSrvService,
    private router: ActivatedRoute,@Optional() public dialogRef: MatDialogRef<TraineeJourneyComponent>,
    @Optional() @Inject(MAT_DIALOG_DATA) public data: any) {
    // this.classID = this.router.snapshot.queryParamMap.get('classID');
    // console.log('query string', this.classID);
    // debugger;
    if(data != null){
      this.classID = data.data;
      this.CloseButtonHide=true;
    }
    else{
      this.fetchSchemes();
      this.commonService.setTitle('Class Journey');
    }
    this.classform = this.fb.group({
      SearchScheme: new FormControl('', Validators.required),
      SearchTSP: new FormControl('', Validators.required),
        SearchClass: new FormControl('', Validators.required)
    }, { updateOn: 'blur' });
  }

  SearchScheme = new FormControl('', Validators.required);
  SearchTSP = new FormControl('', Validators.required);
  SearchClass = new FormControl('', Validators.required);

  public options: any = {

    // TIme line
    chart: {
      zoomType: 'x',
      type: 'timeline',
      height: 500
    },
    xAxis: {
      type: 'datetime',
      visible: false
    },
    yAxis: {
      gridLineWidth: 1,
      title: null,
      labels: {
        enabled: false
      }
    },
    legend: {
      enabled: false
    },
    credits: {
      enabled: false
    },
    title: {
      text: '',
      margin: 0
    },
    tooltip: {
      style: {
        width: 700
      }
    },
    series: [{
      dataLabels: {
        allowOverlap: false,
        format: '<span style="color:{point.color}">● </span><span style="font-weight: bold;" > ' +
          '{point.name}</span><br/>{point.label}'
      },
      marker: {
        symbol: 'circle'
      },
      data: []
    }]
  }

  fetchSchemes() {
    this.commonService.getJSON('api/Dashboard/FetchSchemes').subscribe((data: any[]) => {
      this.Schemes = data;
    });
  }
  onChangeScheme(ID: number) {
    this.SelectedScheme = ID;
    this.clearTSP();
    this.clearClasses();
    this.fetchTSPs();
  }

  fetchTSPs() {
    this.commonService.getJSON('api/Dashboard/FetchTSPsByScheme?SchemeID='+this.SelectedScheme).subscribe((data: any[]) => {
      this.TSPs = data;
    });
  }
  onChangeTSP(ID: number) {
    this.SelectedTSP = ID;
    this.clearClasses();
    this.fetchClasses();
  }
  clearTSP() {
    this.SelectedTSP = undefined;
    this.TSPs = [];
    this.classform.controls.SearchTSP.reset();
  }

  fetchClasses() {
    this.commonService.getJSON('api/Dashboard/FetchClassesBySchemeTSP?SchemeID=' + this.SelectedScheme +'&TspID='+ this.SelectedTSP)
    .subscribe((data: any[]) => {
      this.Classes = data;
    });
  }
  onChangeClass(ID: number) {
    this.SelectedClass = ID;
  }
  clearClasses() {
    this.SelectedClass = undefined;
    this.Classes = [];
    this.classform.controls.SearchClass.reset();
  }

  loadGraph(ID) {
    this.commonService.getJSON('api/Dashboard/ClassJourney?ClassID=' + ID).subscribe((data: any[]) => {
      // console.log(data);
      this.options.series[0].data = data['ChartData'];
      this.ClassFinance = data['Finance'];
      Highcharts.chart('container', this.options);

    });
  }

  Submit(nform: FormGroupDirective) {
    if (!this.classform.valid)
      return;
    this.working = true;
    this.tabGroup.selectedIndex = 0;
    this.loadGraph(this.classform.controls.SearchClass.value);
  }

  reset(nform: FormGroupDirective) {
    this.classform.reset();
  }

  onSelectedTabChange(event) {
    // if (event.index == 0) { this.selectedType = null; }
    console.log(event);
  }

  ngOnInit(): void {
    // this.fetchSchemes();
    this.currentUser=this.commonService.getUserDetails();

    if (this.classID != null) {
      this.loadGraph(this.classID);
    }
  }
  onNoClick(): void {
    this.dialogRef.close(false);
  }
}

// export class ClassModel extends ModelBase {
//  SearchScheme: string;
//  SearchTSP: string;
//  SearchClass: string;
// }

