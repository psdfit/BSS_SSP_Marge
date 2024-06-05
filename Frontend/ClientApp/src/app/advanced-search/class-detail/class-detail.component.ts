import { Component, OnInit, Input } from "@angular/core";
import { CommonSrvService } from "../../common-srv.service";
import { environment } from '../../../environments/environment';

@Component({
  selector: 'class-detail',
  templateUrl: './class-detail.component.html',
  styleUrls: ['./class-detail.component.scss']
})


export class ClassDetailComponent implements OnInit{

  constructor(private commonService: CommonSrvService) {
  }

  environment = environment;

  @Input() ID: any;

  ClassDetail: any;
  ClassStatuses: any[];
  ClassInception: any;
  ClassInstructors: any[];
  ClassMPR: any[];
  ClassMPRDetail: any[];
  ClassMPRDetailMonthly: any[];
  ClassSRN: any[];
  ClassSRNDetail: any[];
  ClassSRNDetailMonthly: any[];
  ClassInvoices: any[];
  ClassPO: any[];

  getData() {
    this.commonService.getJSON("api/AdvancedSearch/GetClassDetail?ClassID=" + this.ID).subscribe((data: any[]) => {
      this.ClassDetail = data["ClassDetail"][0];
      this.ClassStatuses = data["ClassStatuses"];
      this.ClassInception = data["ClassInception"][0];
      this.ClassInstructors = data["ClassInstructors"];
      this.ClassMPR = data["ClassMPR"];
      this.ClassSRN = data["ClassSRN"];
      this.ClassPO = data["ClassPO"];
      this.ClassInvoices = data["ClassInvoices"];
      //console.log(data);
    });
  }

  getMPRDetails(r: any) {
    if (r.ClassMPRDetail) {
      r.ClassMPRDetail = null;
      this.ClassMPRDetailMonthly = this.ClassMPRDetailMonthly.filter(mpr => mpr.MPRID != r.MPRID);
      return;
    }
    this.commonService.getJSON("api/MPR/MPRTraineeDetail/" + r.MPRID).subscribe((data: any) => {
      //console.log(data);
      r.ClassMPRDetail = data;
      this.ClassMPRDetailMonthly.push(data);
      //console.log(r.ClassMPRDetail);
      this.ClassMPRDetailMonthly = this.ClassMPRDetailMonthly.reduce((accumulator, value) => accumulator.concat(value), []);
    });
  }

  getSRNDetails(r: any) {
    if (r.ClassSRNDetail) {
      r.ClassSRNDetail = null;
      this.ClassSRNDetailMonthly = this.ClassSRNDetailMonthly.filter(s => s.SRNID != r.SRNID);
      return;
    }
    this.commonService.getJSON("api/SRN/GetSRNDetailsFiltered/" + r.SRNID).subscribe((data: any) => {
      //console.log(data);
      r.ClassSRNDetail = data[0];
      this.ClassSRNDetailMonthly.push(data[0]);
      this.ClassSRNDetailMonthly = this.ClassSRNDetailMonthly.reduce((accumulator, value) => accumulator.concat(value), []);

    });
  }

  ngOnInit(): void {
    this.getData();
    this.ClassMPRDetailMonthly = [];
    this.ClassSRNDetailMonthly = [];
  }
}
