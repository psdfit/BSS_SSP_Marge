import { Component, OnInit } from "@angular/core";
import { FormGroup, FormBuilder, FormControl, Validators, FormGroupDirective } from '@angular/forms';
import { CommonSrvService } from "../../common-srv.service";
import { MatDatepicker } from "@angular/material/datepicker";
import * as _moment from 'moment';
import * as Highcharts from 'highcharts';

declare var require: any;
let Boost = require('highcharts/modules/boost');
let noData = require('highcharts/modules/no-data-to-display');
let More = require('highcharts/highcharts-more');
let SolidGuage = require('highcharts/modules/solid-gauge');
let Funnel = require('highcharts/modules/funnel');

Boost(Highcharts);
noData(Highcharts);
More(Highcharts);
SolidGuage(Highcharts);
Funnel(Highcharts);
noData(Highcharts);

@Component({
  selector: 'management',
  templateUrl: './management.component.html',
  styleUrls: ['./management.component.scss']
})

export class ManagementComponent implements OnInit {

  constructor(private commonService: CommonSrvService) {

  }

  SearchTrade = new FormControl("");
  SearchCluster = new FormControl("");
  SearchDistrict = new FormControl("");
  SearchProgram = new FormControl("");
  SearchScheme = new FormControl("");
  SearchTSP = new FormControl("");
  SearchDuration = new FormControl("");

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

  ContractedTrainees: number;
  EnrolledTrainees: number;
  UnverifiedTrainees: number;
  TrainingProviders: number;
  ContractedClasses: number;

  CompletedTrainee: number;
  StipendAmount: string;
  IncomeRatioEmp: string;
  IncomeRatioUnEmp: string;
  ContractToEnroll: string;
  EnrollToComplete: string;

  Passed: number;
  EnrolledToPass: any;
  ContractToPass: any;
  CompleteToPass: any;

  AverageWageRate: any;
  AverageWageRateForecast: any;
  OpportunityCost: any;
  OpportunityCostForecast: any;
  VerifiedTrainees: any;
  ActualCTM: any;
  ContractualCTM: any;
  CompletedTrainees: any;
  VerifiedOverCommitmentRatio: any;
  ContractualROSI: number;
  ActualROSI: number;
  ForecastROSI: number;

  public optionsSDGuage: any = {
    chart: {
      type: 'solidgauge',
      height: '300px'
    },

    title: {
      text: ''
    },

    credits: {
      enabled: false
    },
    legend: {
      enabled: true,
      labelFormatter: function () {
        return this.name + ' ' + this.data[0].y + '%';
      }
    },
    tooltip: {
      borderWidth: 0,
      backgroundColor: 'none',
      shadow: false,
      style: {
        fontSize: '14px'
      },
      valueSuffix: '%',
      pointFormat: '{series.name}<br><span style="font-size:2em; color: {point.color}; font-weight: bold">{point.y}</span>',
      positioner: function (labelWidth) {
        return {
          x: (this.chart.chartWidth - labelWidth) / 2,
          y: (this.chart.plotHeight / 2) + 15
        };
      }
    },

    pane: {
      startAngle: 0,
      endAngle: 360,
      background: [{ // Track for Move
        outerRadius: '112%',
        innerRadius: '95%',
        backgroundColor: Highcharts.color("#035C9D")
          .setOpacity(0.3)
          .get(),
        borderWidth: 0
      }, { // Track for Exercise
        outerRadius: '94%',
        innerRadius: '77%',
          backgroundColor: Highcharts.color("#47A6B7")
          .setOpacity(0.3)
          .get(),
        borderWidth: 0
      }, { // Track for Stand
        outerRadius: '76%',
        innerRadius: '59%',
          backgroundColor: Highcharts.color("#3B6D82")
          .setOpacity(0.6)
          .get(),
        borderWidth: 0
      }, { // Track for Stand
        outerRadius: '58%',
        innerRadius: '41%',
          backgroundColor: Highcharts.color("#6DA2C5")
          .setOpacity(0.3)
          .get(),
        borderWidth: 0
      }]
    },

    yAxis: {
      min: 0,
      max: 100,
      lineWidth: 0,
      tickPositions: []
    },

    plotOptions: {
      solidgauge: {
        dataLabels: {
          enabled: false
        },
        linecap: 'round',
        stickyTracking: false,
        rounded: true,
        showInLegend: true
      }
    },

    series: []
  }
  public optionsSDPie: any = {
    chart: {
      plotBackgroundColor: null,
      plotBorderWidth: null,
      plotShadow: false,
      type: 'pie',
      height: '300px'
    },
    title: {
      text: ''
    },
    credits: {
      enabled: false
    },
    tooltip: {
      pointFormat: '<br>{point.name}: {point.y}<br>Total: {point.total}<br>{point.percentage:.1f} %'
    },
    accessibility: {
      point: {
        valueSuffix: '%'
      }
    },
    plotOptions: {
      pie: {
        allowPointSelect: true,
        cursor: 'pointer',
        dataLabels: {
          enabled: false
        },
        showInLegend: true
      }
    },
    series: [{
      name: 'Gender Segregation',
      colorByPoint: true,
      data: []
    }]
  }
  public optionsViolations: any = {
    chart: {
      type: 'funnel',
      height: '300px'
    },
    title: {
      text: ''
    },
    credits: {
      enabled: false
    },
    plotOptions: {
      series: {
        dataLabels: {
          enabled: true,
          format: '<b>{point.name}</b> ({point.y:,.0f})',
          allowOverlap: true,
          alignTo: "connectors",
          y: 10
        },
        neckWidth: '30%',
        neckHeight: '25%',
        width: '80%',
        height: '80%'
      }
    },
    series: [{
      name: 'Violations',
      data: []
    }]
  }
  public optionsExpDrops: any = {
    chart: {
      plotBackgroundColor: null,
      plotBorderWidth: 0,
      plotShadow: false,
      height: '300px'
    },
    title: {
      text: ''
    },
    credits: {
      enabled: false
    },
    legend: {
      enabled: true,
      labelFormatter: function () {
        //console.log(this);
        return this.name + ' (' + this.y + ')';
      }
    },
    tooltip: {
      pointFormat: '{series.name}: <b>{point.y:.1f}</b>'
    },
    accessibility: {
      point: {
        valueSuffix: '%'
      }
    },
    plotOptions: {
      pie: {
        dataLabels: {
          enabled: true,
          distance: -50,
          style: {
            fontWeight: 'bold',
            color: 'white'
          }
        },
        startAngle: -90,
        endAngle: 90,
        center: ['50%', '75%'],
        size: '110%'
      }
    },
    series: [{
      type: 'pie',
      name: '',
      showInLegend: true,
      innerSize: '50%',
      data: []
    }]
  }
  public optionsPayments: any = {
    chart: {
      type: 'waterfall',
      height: '300px'
    },

    title: {
      text: ''
    },
    credits: {
      enabled: false
    },
    xAxis: {
      type: 'category'
    },

    yAxis: {
      title: {
        text: 'PKR'
      }
    },

    legend: {
      enabled: false
    },

    tooltip: {
      pointFormat: '<b>{point.y:,.2f}</b> PKR'
    },

    series: [{
      upColor: Highcharts.getOptions().colors[2],
      color: Highcharts.getOptions().colors[3],
      data: [],
      dataLabels: {
        enabled: true,
        formatter: function () {
          return Highcharts.numberFormat(this.y / 1000, 0, ',') + 'k';
        },
        style: {
          fontWeight: 'bold'
        }
      },
      pointPadding: 0
    }]
  }
  public optionsPassed: any = {
    chart: {
      type: 'column',
      height: '300px'
    },
    title: {
      text: ''
    },
    credits: {
      enabled: false
    },
    legend: {
      enabled: true,
      labelFormatter: function () {
        return this.name + ' (' + this.yData[0] + ')';
      }
    },
    xAxis: {
      categories: [
        'Trainee Status'
      ],
      crosshair: true
    },
    yAxis: {
      min: 0,
      title: {
        text: 'No of Trainees'
      }
    },
    tooltip: {
      headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
      pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
        '<td style="padding:0"><b>{point.y:.1f} </b></td></tr>',
      footerFormat: '</table>',
      shared: true,
      useHTML: true
    },
    plotOptions: {
      column: {
        pointPadding: 0.2,
        borderWidth: 0,
        showInLegend: true
      }
    },
    series: []
  }
  public optionsPlacement: any = {
    chart: {
      type: 'pie',
      height: '300px'
    },
    title: {
      text: ''
    },
    credits: {
      enabled: false
    },
    tooltip: {
      headerFormat: '',
      pointFormat: '<span style="color:{point.color}">\u25CF</span> <b> {point.name}</b><br/>' +
        'Value: <b>{point.y}</b><br/>'
    },
    series: [{
      minPointSize: 10,
      innerSize: '20%',
      zMin: 0,
      name: 'placements',
      data: []
    }]
  }
  public optionsPlacement1: any = {
    chart: {
      type: 'funnel',
      height: '300px'
    },
    title: {
      text: ''
    },
    credits: {
      enabled: false
    },
    legend: {
      enabled: true,
      labelFormatter: function () {
        return this.name + ' (' + this.y + ' %)';
      }
    },
    plotOptions: {
      funnel: {
        showInLegend: true
      },
      series: {
        dataLabels: {
          enabled: false,
          //format: '<b>{point.name}</b> <br> ({point.y:,.0f})',
          //allowOverlap: true,
          //y: 10
        },
        neckWidth: '30%',
        neckHeight: '25%',
        width: '80%',
        height: '80%'
      }
    },
    series: [{
      name: 'Placements Reported',
      data: []
    }]
  }
  public optionsPlacement2: any = {
    chart: {
      type: 'column',
      height: '300px'
    },
    title: {
      text: ''
    },
    credits: {
      enabled: false
    },
    legend: {
      enabled: true,
      labelFormatter: function () {
        return this.name + ' (' + this.yData[0] + ')';
      }
    },
    xAxis: {
      categories: [
        'Verified Placements Ratio'
      ],
      crosshair: true
    },
    yAxis: {
      min: 0,
      title: {
        text: 'Verified Ratio'
      }
    },
    tooltip: {
      headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
      pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
        '<td style="padding:0"><b>{point.y:.1f} </b></td></tr>',
      footerFormat: '</table>',
      shared: true,
      useHTML: true
    },
    plotOptions: {
      column: {
        pointPadding: 0.2,
        borderWidth: 0,
        showInLegend: true
      }
    },
    series: []
  }

  loadGraph(type, ID, text, Start = undefined, End = undefined) {
    let filterData = {
      type: type,
      ID: ID
    }

    this.commonService.getJSON("api/Dashboard/ManagmentDashboard?type=" + type + "&ID=" + ID + "&StartDate=" + Start + "&EndDate=" + End).subscribe((data: any[]) => {
      this.optionsSDGuage.series = data["SDGuage"];
      this.optionsSDPie.series[0].data = data["SDPie"];

      //console.log(data);
      this.ContractedTrainees = data["SchemeDefination"][0].Contracted;
      this.EnrolledTrainees = data["SchemeDefination"][0].Enrolled;
      this.UnverifiedTrainees = data["SchemeDefination"][0].UnVerified;
      this.TrainingProviders = data["SchemeDefination"][0].TSPs;
      this.ContractedClasses = data["SchemeDefination"][0].ContractedClasses;

      //Monitoring
      this.optionsViolations.series[0].data = data["Violations"];
      this.optionsExpDrops.series[0].data = data["ExpelledDropouts"];

      //Exam & Certifications
      this.optionsPayments.series[0].data = data["Payments"];
      
      this.CompletedTrainee = data["Monitoring"][0].Completed;
      this.StipendAmount = data["Monitoring"][0].StipendAmount;
      this.IncomeRatioEmp = data["Monitoring"][0].EmployeedIncomeBeforeGraduation;
      this.IncomeRatioUnEmp = data["Monitoring"][0].UnEmployeedIncomeBeforeGraduation;
      this.ContractToEnroll = data["Monitoring"][0].ContractToEnrollRatio;
      this.EnrollToComplete = data["Monitoring"][0].EnrollToCompleteRatio;
      //console.log(data["ExamCertification"]);
      this.Passed = data["ExamCertification"][0].Passed;
      this.EnrolledToPass = data["ExamCertification"][0].EnrolledToPassedRatio;
      this.ContractToPass = data["ExamCertification"][0].ContractToPassedRatio;
      this.CompleteToPass = data["ExamCertification"][0].CompleteToPassedRatio;

      this.optionsPassed.series = [{
        name: 'Enrolled',
        data: [this.EnrolledTrainees]

      }, {
        name: 'Contracted',
        data: [this.ContractedTrainees]

      }, {
        name: 'Completed',
        data: [this.CompletedTrainee]

      }, {
        name: 'Passed',
        data: [this.Passed]

        }];

      //Placement
      this.optionsPlacement.series[0].data = [{
        name: 'Reported',
        y: data["Placement"][0].Placements,
        //color: Highcharts.getOptions().colors[1]
      }, {
        name: 'Verified',
        y: data["Placement"][0].PlacementsVerified,
        color: Highcharts.getOptions().colors[2]
      }];
      this.optionsPlacement1.series[0].data = [
        ['Passed to Report', data["Placement"][0].PassedToReportRatio],
        ['Enroll to Report', data["Placement"][0].EnrollToReportRatio],
        ['Contract to Report', data["Placement"][0].ContractToReportRatio],
        ['Complete to Report', data["Placement"][0].CompleteToReportRatio]        
      ]
      this.optionsPlacement2.series = [{
          name: 'Enroll to Verified',
          data: [data["Placement"][0].EnrollToVerifiedRatio]

      }, {
          name: 'Contract to Verified',
          data: [data["Placement"][0].ContractToVerifiedRatio]

      }, {
          name: 'Complete to Verified',
          data: [data["Placement"][0].CompleteToVerifiedRatio]

      }, {
          name: 'Passed to Verified',
          data: [data["Placement"][0].PassedToVerifiedRatio]

      }];

      Highcharts.chart('containerSDGuage', this.optionsSDGuage);
      Highcharts.chart('containerSDPie', this.optionsSDPie);
      Highcharts.chart('containerViolations', this.optionsViolations);
      Highcharts.chart('containerExpDrops', this.optionsExpDrops);
      Highcharts.chart('containerPayments', this.optionsPayments);
      Highcharts.chart('containerPassed', this.optionsPassed);
      Highcharts.chart('containerPlacemnent', this.optionsPlacement);
      Highcharts.chart('containerPlacemnent1', this.optionsPlacement1);
      Highcharts.chart('containerPlacemnent2', this.optionsPlacement2);

      //ROSI
      this.AverageWageRate = data["ROSI"][0].VerifiedAverageWageRate;
      this.AverageWageRateForecast = data["ROSI"][0].VerifiedAverageWageRateForecastedActual;
      this.OpportunityCost = data["ROSI"][0].OpportunityCost;
      this.OpportunityCostForecast = data["ROSI"][0].OpportunityCost;
      this.VerifiedTrainees = data["ROSI"][0].NoOfVerifiedTrainees;
      this.ActualCTM = data["ROSI"][0].ActualCTM;
      this.ContractualCTM = data["ROSI"][0].ContractualCTM;
      this.CompletedTrainees = data["ROSI"][0].NoOfActualCompletedTrainees;
      //this.VerifiedOverCommitmentRatio = data["ROSI"][0].VerifiedOverCommitmentRatio;
      //this.CancelledClasses = data["ROSI"][0].CancelledClasses;
      //this.Contractual = data["ROSI"][0].Contractual;
      //this.NetIncrease = data["ROSI"][0].NetIncrease;
      this.ContractualROSI = data["ROSI"][0].ContractualROSI;
      this.ActualROSI = data["ROSI"][0].ActualROSI;
      this.ForecastROSI = data["ROSI"][0].ReportedForcastedROSI;

    });
  }

  getData() {
    this.commonService.getJSON("api/Dashboard/TraineeJourneyFilters").subscribe((data: any[]) => {
      this.Trades = data["Trades"];
      this.Clusters = data["Clusters"];
      this.Districts = data["Districts"];
      this.Schemes = data["Schemes"];
      this.TSPs = data["TSPs"];
      this.Programs = data["Programs"];
      //console.log(data);
    });
  }

  onChangeTrade(event: any) {
    this.loadGraph('Trade', event.value, event.source.triggerValue, '', '');
  }

  onChangeCluster(event: any) {
    //console.log(event.target.value);
    this.loadGraph('Cluster', event.value, event.source.triggerValue, '', '');
  }

  onChangeDistrict(event: any) {
    //console.log(event.target.value);
    this.loadGraph('District', event.value, event.source.triggerValue, '', '');
  }

  onChangeProgram(event: any) {
    //console.log(event.target.value);
    this.loadGraph('Program', event.value, event.source.triggerValue, '', '');
  }

  onChangeScheme(event: any) {
    //console.log(event.target.value);
    this.loadGraph('Scheme', event.value, event.source.triggerValue, '', '');
  }

  onChangeTSP(event: any) {
    //console.log(event.target.value);
    this.loadGraph('TSP', event.value, event.source.triggerValue, '', '');
  }

  onChangeStartDate(event) {
    this.selectedValueStart = event.value;
    this.EmptyDopdowns();
    if (this.selectedValueEnd) {
      this.Start = _moment(new Date(this.selectedValueStart)).format("YYYY-MM-DD");
      this.End = _moment(new Date(this.selectedValueEnd)).format("YYYY-MM-DD");
      this.loadGraph('Duration', 0, this.Start + ' to ' + this.End, this.Start, this.End);
    }
  }

  onChangeEndDate(event) {
    this.selectedValueEnd = event.value;
    this.EmptyDopdowns();
    if (this.selectedValueEnd) {
      this.Start = _moment(new Date(this.selectedValueStart)).format("YYYY-MM-DD");
      this.End = _moment(new Date(this.selectedValueEnd)).format("YYYY-MM-DD");
      this.loadGraph('Duration', 0, this.Start + ' to ' + this.End, this.Start, this.End);
    }
  }

  EmptyCtrl(type: any) {
    //this.SearchTrade.setValue('undefined');
    console.log(type);
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
    this.commonService.setTitle("Management Dashboard");
    setTimeout(() => {
      this.loadGraph('', 0, '', '', '');
    }, 100);

    this.getData();
  }

}
