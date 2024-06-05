import { Component, OnInit, Inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CommonSrvService } from '../../common-srv.service';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { DialogueService } from 'src/app/shared/dialogue.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'prn-approval-penalty',
  templateUrl: './prn-approval-penalty.component.html',
  styleUrls: ['./prn-approval-penalty.component.scss']
})
export class PrnApprovalPenaltyComponent implements OnInit {
  PenaltyImposedByMEform: FormGroup;
  environment = environment;
  title: string; savebtn: string; working: boolean; error: string
  EnText: string = "Penalty Imposed By ME";
  ClassCode:string="";
  ClassID:number=0;
  PRNMonth:any;
  constructor(private fb: FormBuilder, private http: CommonSrvService, private dialogue: DialogueService, public dialogRef: MatDialogRef<PrnApprovalPenaltyComponent>,
    @Inject(MAT_DIALOG_DATA) public data:any) {
    this.PenaltyImposedByMEform = this.fb.group({
      PenaltyImposedByME: ["", Validators.required],
      DeductionUniformBagReceiving:["",Validators.required],
      PenaltyAndUniBagRecvInputRemarks: ["", Validators.required]
    }, { updateOn: "blur" });
   
  }

  ngOnInit():void {
    this.ClassCode=this.data.data.ClassCode;
    this.ClassID=this.data.data.ClassID;
    this.PRNMonth=this.data.data.PRNMonth;
      this.PenaltyImposedByMEform.patchValue({
        PenaltyImposedByME: this.data.data.PenaltyImposedByME,
        PenaltyAndUniBagRecvInputRemarks:this.data.data.PenaltyAndUniBagRecvInputRemarks,
        DeductionUniformBagReceiving: this.data.data.DeductionUniformBagReceiving

      });
   
  }
  openClassJourneyDialogue(data: any): void
  {
		this.dialogue.openClassJourneyDialogue(data);
  }
  Submit() {
    
    if (!this.PenaltyImposedByMEform.valid)
      return;
  }

  
  get PenaltyAndUniBagRecvInputRemarks() { return this.PenaltyImposedByMEform.get("PenaltyAndUniBagRecvInputRemarks"); }
  get PenaltyImposedByME() { return this.PenaltyImposedByMEform.get("PenaltyImposedByME"); }
  get DeductionUniformBagReceiving() { return this.PenaltyImposedByMEform.get("DeductionUniformBagReceiving"); }
}
