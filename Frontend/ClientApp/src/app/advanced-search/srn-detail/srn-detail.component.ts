import { Component, OnInit, Input } from "@angular/core";
import { CommonSrvService } from "../../common-srv.service";
import { environment } from '../../../environments/environment';

@Component({
  selector: 'srn-detail',
  templateUrl: './srn-detail.component.html',
  styleUrls: ['./srn-detail.component.scss']
})


export class SRNDetailComponent implements OnInit{

  constructor(private commonService: CommonSrvService) {
  }

  environment = environment;

  @Input() ID: any;

  SRN: any;
  SRNDetail: any;
  SRNDetailMonthly: any;
  
  getData() {
    this.commonService.getJSON("api/AdvancedSearch/GetSRNDetail?SRNID=" + this.ID).subscribe((data: any[]) => {
      this.SRN = data["SRN"];
    });
  }

  getSRNDetails(r: any) {
    if (r.SRNDetail) {
      r.SRNDetail = null;
      this.SRNDetailMonthly = this.SRNDetailMonthly.filter(s => s.SRNID != r.SRNID);
      return;
    }
    this.commonService.getJSON("api/SRN/GetSRNDetailsFiltered/" + r.SRNID).subscribe((data: any) => {
      //console.log(data);
      r.SRNDetail = data[0];
      this.SRNDetailMonthly.push(data[0]);
      this.SRNDetailMonthly = this.SRNDetailMonthly.reduce((accumulator, value) => accumulator.concat(value), []);

    });
  }

  ngOnInit(): void {
    this.getData();
    this.SRNDetailMonthly = [];
  }
}
