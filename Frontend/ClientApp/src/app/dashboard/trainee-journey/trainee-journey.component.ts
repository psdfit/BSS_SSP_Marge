import { Component, Inject, OnInit, Optional } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormGroupDirective } from '@angular/forms';
import { CommonSrvService } from '../../common-srv.service';
import * as Highcharts from 'highcharts';
import { ModelBase } from '../../shared/ModelBase';
import { ActivatedRoute } from '@angular/router';
import { UsersModel } from 'src/app/master-data/users/users.component';
import { EnumUserLevel } from 'src/app/shared/Enumerations';
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
  selector: 'app-trainee-journey',
  templateUrl: './trainee-journey.component.html',
  styleUrls: ['./trainee-journey.component.scss']
})

export class TraineeJourneyComponent implements OnInit {

  traineeform: FormGroup;
  working: boolean;
  traineecode: string;
  currentUser: UsersModel;
  CloseButtonHide = false;
  constructor(private fb: FormBuilder, private commonService: CommonSrvService, private router: ActivatedRoute,
    @Optional() public dialogRef: MatDialogRef<TraineeJourneyComponent>,
    @Optional() @Inject(MAT_DIALOG_DATA) public traineeCode: any) {
    // debugger;
    if (traineeCode != null) {
      this.traineecode = traineeCode.data;
      this.CloseButtonHide = true;
    }
    else {
      this.traineecode = this.router.snapshot.queryParamMap.get('traineeCode');
      this.traineeform = this.fb.group({
        TraineeCode: [this.traineecode],
        TraineeCNIC: ['', [Validators.minLength(15), Validators.maxLength(15)]]
      }, { updateOn: 'blur' });
      this.commonService.setTitle('Trainee Journey');
    }

  }

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
      text: 'Trainee Journey'
    },
    subtitle: {
      text: 'Overall in the System'
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

  loadGraph(traineeCode, traineeCNIC) {
    this.commonService.getJSON('api/Dashboard/TraineeJourneySingle?traineeCode=' + traineeCode + '&traineeCNIC=' + traineeCNIC)
      .subscribe((data: any[]) => {
        this.options.series[0].data = data;

        Highcharts.chart('container', this.options);

      });
  }

  Submit(nform: FormGroupDirective) {
    if (!this.traineeform.valid)
      return;
    this.working = true;
    this.loadGraph(this.traineeform.controls.TraineeCode.value, this.traineeform.controls.TraineeCNIC.value);
  }

  reset(nform: FormGroupDirective) {
    this.traineeform.reset();
  }

  ngOnInit(): void {
    // console.log(this.traineeCode.data);
    if (this.traineecode != null) { this.loadGraph(this.traineecode, null); }

    this.currentUser = this.commonService.getUserDetails();
  }
  onNoClick(): void {
    this.dialogRef.close(false);
  }
}

export class TraineeModel extends ModelBase {
  TraineeCode: string;
  TraineeCNIC: string;
}

