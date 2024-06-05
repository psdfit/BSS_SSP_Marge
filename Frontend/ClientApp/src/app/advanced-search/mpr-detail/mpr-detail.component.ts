import { Component, OnInit, Input } from "@angular/core";
import { CommonSrvService } from "../../common-srv.service";
import { environment } from '../../../environments/environment';

@Component({
  selector: 'mpr-detail',
  templateUrl: './mpr-detail.component.html',
  styleUrls: ['./mpr-detail.component.scss']
})


export class MPRDetailComponent implements OnInit{

  constructor(private commonService: CommonSrvService) {
  }

  environment = environment;

  @Input() ID: any;

  MPR: any;
  MPRDetail: any;
  MPRDetailMonthly: any;
  
  getData() {
    this.commonService.getJSON("api/AdvancedSearch/GetMPRDetail?MPRID=" + this.ID).subscribe((data: any[]) => {
      this.MPR = data["MPR"];
    });
  }

  getMPRDetails(r: any) {
    if (r.MPRDetail) {
      r.MPRDetail = null;
      this.MPRDetailMonthly = this.MPRDetailMonthly.filter(mpr => mpr.MPRID != r.MPRID);
      return;
    }
    this.commonService.getJSON("api/MPR/MPRTraineeDetail/" + r.MPRID).subscribe((data: any) => {
      //console.log(data);
      r.MPRDetail = data;
      this.MPRDetailMonthly.push(data);
      //console.log(r.ClassMPRDetail);
      this.MPRDetailMonthly = this.MPRDetailMonthly.reduce((accumulator, value) => accumulator.concat(value), []);
    });
  }

  ngOnInit(): void {
    this.getData();
    this.MPRDetailMonthly = [];
  }
}
