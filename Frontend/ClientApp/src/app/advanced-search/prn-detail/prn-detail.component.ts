import { Component, OnInit, Input } from "@angular/core";
import { CommonSrvService } from "../../common-srv.service";
import { environment } from '../../../environments/environment';
import { EnumApprovalProcess, EnumExcelReportType } from '../../shared/Enumerations';
import { DialogueService } from '../../shared/dialogue.service';

@Component({
  selector: 'prn-detail',
  templateUrl: './prn-detail.component.html',
  styleUrls: ['./prn-detail.component.scss']
})


export class PRNDetailComponent implements OnInit{

  constructor(private commonService: CommonSrvService, private dialogue: DialogueService) {
  }

  environment = environment;

  @Input() ID: any;

  enumApprovalProcess = EnumApprovalProcess;
  processKey: string = '';
  processTypes = [
    { value: EnumApprovalProcess.PRN_R, text: "Regular" }
    , { value: EnumApprovalProcess.PRN_C, text: "Completion" }
    , { value: EnumApprovalProcess.PRN_T, text: "PRN_T" }
    , { value: EnumApprovalProcess.PRN_F, text: "Final" }
  ]

  TSPPRN: any[];
  TSPPRNDetail: any[];
  TSPPRNDetailMonthly: any[];
  
  getData() {
    this.commonService.getJSON("api/AdvancedSearch/GetPRNDetail?PRNID=" + this.ID).subscribe((data: any[]) => {
      this.TSPPRN = data["PRN"];
    });
  }

  getPRNDetail(r: any) {
    r.HasPRN = !r.HasPRN;

    if (r.PRN) {
      this.TSPPRNDetail = this.TSPPRNDetail.filter(s => s.PRNMasterID != r.PRNMasterID);

      return;
    }
    this.commonService.getJSON(`api/PRN/GetPRNForApproval/`, r.PRNMasterID).subscribe(
      (data: any) => {
        r.PRN = data;
        r.HasPRN = true;
        r.HasPRN = true;
        this.TSPPRNDetail.push(data);
        this.TSPPRNDetail = this.TSPPRNDetail.reduce((accumulator, value) => accumulator.concat(value), []);
        //console.log(this.prnDetailsArray);
      }
      , (error) => {
        console.error(JSON.stringify(error));
      }
    );
  }

  getPTBRTrainees(c: any) {
    this.commonService.postJSON(`api/PRN/GetPTBRTrainees`, { ClassCode: c.ClassCode, Month: c.Month }).subscribe(
      (data: any) => {
        c.previousPTBRTrainees = data;
      }
      , (error) => {
        console.error(JSON.stringify(error));
      }
    );
  }

  getClassMonthview(ClassID: number, Month: Date, processkey: string): void {
    this.dialogue.openClassMonthviewDialogue(ClassID, Month, processkey).subscribe(result => {
      console.log(result);
      //location.reload();
    });
  }

  ngOnInit(): void {
    this.getData();
    this.TSPPRNDetailMonthly = [];
  }
}
