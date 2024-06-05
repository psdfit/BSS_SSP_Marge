import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDatepicker } from '@angular/material/datepicker';
import * as _moment from 'moment';
import { CommonSrvService } from '../../common-srv.service';
import * as Highcharts from 'highcharts';

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
  selector: 'app-trainee-journey-all',
  templateUrl: './trainee-journey-all.component.html',
  styleUrls: ['./trainee-journey-all.component.scss']
})

export class TraineeJourneyAllComponent implements OnInit {

  constructor(private commonService: CommonSrvService) {

  }

  SearchTrade = new FormControl('');
  SearchCluster = new FormControl('');
  SearchDistrict = new FormControl('');
  SearchProgram = new FormControl('');
  SearchScheme = new FormControl('');
  SearchTSP = new FormControl('');
  SearchDuration = new FormControl('');

  selectedValueTrade: any;
  selectedValueCluster: any;
  selectedValueDistrict: any;
  selectedValueScheme: any;
  selectedValueTSP: any;
  selectedValueProgram: any;
  selectedValueStart: any = undefined;
  selectedValueEnd: any = undefined;

  Start: string;
  End: string;

  Trades: any[];
  Clusters: any[];
  Districts: any[];
  Schemes: any[];
  TSPs: any[];
  Programs: any[];

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
      text: ''
    },
    subtitle: {
      text: ''
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

  loadGraph(type, ID, text, Start = undefined, End = undefined) {
    const filterData = {
      type,
      ID
    }
    this.commonService.getJSON('api/Dashboard/TraineeJourney?type='+type+'&ID='+ID+'&StartDate='+Start+'&EndDate='+End,).subscribe((data: any[]) => {
      this.options.series[0].data = data;

      if (text == '') { this.options.subtitle.text = ''; }
      else { this.options.subtitle.text = type + ': <b>' + text + '</b> '; }

      Highcharts.chart('container', this.options);

    });
  }

  getData() {
    this.commonService.getJSON('api/Dashboard/TraineeJourneyFilters').subscribe((data: any[]) => {
      this.Trades = data['Trades'];
      this.Clusters = data['Clusters'];
      this.Districts = data['Districts'];
      this.Schemes = data['Schemes'];
      this.TSPs = data['TSPs'];
      this.Programs = data['Programs'];
      // console.log(data);
    });
  }

  onChangeTrade(event: any) {
    this.loadGraph('Trade', event.value, event.source.triggerValue,'','');
  }

  onChangeCluster(event: any) {
    // console.log(event.target.value);
    this.loadGraph('Cluster', event.value, event.source.triggerValue,'','');
  }

  onChangeDistrict(event: any) {
    // console.log(event.target.value);
    this.loadGraph('District', event.value, event.source.triggerValue,'','');
  }

  onChangeProgram(event: any) {
    // console.log(event.target.value);
    this.loadGraph('Program', event.value, event.source.triggerValue,'','');
  }

  onChangeScheme(event: any) {
    // console.log(event.target.value);
    this.loadGraph('Scheme', event.value, event.source.triggerValue,'','');
  }

  onChangeTSP(event: any) {
    // console.log(event.target.value);
    this.loadGraph('TSP', event.value, event.source.triggerValue,'','');
  }

  onChangeStartDate(event) {
    this.selectedValueStart = event.value;
    this.EmptyDopdowns();
    if (this.selectedValueEnd) {
      this.Start = _moment(new Date(this.selectedValueStart)).format('YYYY-MM-DD');
      this.End = _moment(new Date(this.selectedValueEnd)).format('YYYY-MM-DD');
      this.loadGraph('Duration', 0, this.Start+' to '+this.End, this.Start, this.End);
    }
  }

  onChangeEndDate(event) {
    this.selectedValueEnd = event.value;
    this.EmptyDopdowns();
    if (this.selectedValueEnd) {
      this.Start = _moment(new Date(this.selectedValueStart)).format('YYYY-MM-DD');
      this.End = _moment(new Date(this.selectedValueEnd)).format('YYYY-MM-DD');
      this.loadGraph('Duration', 0, this.Start + ' to ' + this.End, this.Start, this.End);
    }
  }

  EmptyCtrl(type: any) {
    // this.SearchTrade.setValue('undefined');
    if (type == 'Trade') {
      this.selectedValueCluster = undefined;
      this.selectedValueDistrict = undefined;
      this.selectedValueProgram = undefined;
      this.selectedValueScheme = undefined;
      this.selectedValueTSP = undefined;
      this.selectedValueStart = undefined;
      this.selectedValueEnd = undefined;
    } else if (type == 'Cluster') {
      this.selectedValueTrade = undefined;
      this.selectedValueDistrict = undefined;
      this.selectedValueProgram = undefined;
      this.selectedValueScheme = undefined;
      this.selectedValueTSP = undefined;
      this.selectedValueStart = undefined;
      this.selectedValueEnd = undefined;
    } else if (type == 'District') {
      this.selectedValueTrade = undefined;
      this.selectedValueCluster = undefined;
      this.selectedValueProgram = undefined;
      this.selectedValueScheme = undefined;
      this.selectedValueTSP = undefined;
      this.selectedValueStart = undefined;
      this.selectedValueEnd = undefined;
    } else if (type == 'Program') {
      this.selectedValueTrade = undefined;
      this.selectedValueDistrict = undefined;
      this.selectedValueCluster = undefined;
      this.selectedValueScheme = undefined;
      this.selectedValueTSP = undefined;
      this.selectedValueStart = undefined;
      this.selectedValueEnd = undefined;
    } else if (type == 'Scheme') {
      this.selectedValueTrade = undefined;
      this.selectedValueDistrict = undefined;
      this.selectedValueProgram = undefined;
      this.selectedValueCluster = undefined;
      this.selectedValueTSP = undefined;
      this.selectedValueStart = undefined;
      this.selectedValueEnd = undefined;
    } else if (type == 'TSP') {
      this.selectedValueTrade = undefined;
      this.selectedValueDistrict = undefined;
      this.selectedValueProgram = undefined;
      this.selectedValueScheme = undefined;
      this.selectedValueCluster = undefined;
      this.selectedValueStart = undefined;
      this.selectedValueEnd = undefined;
    }
  }
  private EmptyDopdowns() {
    this.selectedValueTrade = undefined;
    this.selectedValueCluster = undefined;
    this.selectedValueDistrict = undefined;
    this.selectedValueProgram = undefined;
    this.selectedValueScheme = undefined;
    this.selectedValueTSP = undefined;
  }

  ngOnInit(): void {
    this.commonService.setTitle('Trainee Journey');
    setTimeout(() => {
      this.loadGraph('', 0, '', '', '');
    }, 100);

    this.getData();
  }

}


