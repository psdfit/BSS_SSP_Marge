import { Component, OnInit, ViewChild, Inject } from '@angular/core';
import { CommonSrvService } from '../../common-srv.service';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormGroup, FormBuilder, Validators, FormGroupDirective, NgForm } from '@angular/forms';
import { EnumApprovalStatus } from '../../shared/Enumerations';
import { UsersModel, UserRightsModel } from '../../master-data/users/users.component';
import { environment } from '../../../environments/environment';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-psp-batch-dialogue',
  templateUrl: './psp-batch-dialogue.component.html',
  styleUrls: ['./psp-batch-dialogue.component.scss']
})
export class PSPBatchDialogueComponent implements OnInit {
  pspbatchform: FormGroup;
  title: string; savebtn: string;
  formrights: UserRightsModel;
  EnText: string = "Batch";

  error: String;
  update: String;
  working: boolean;

  currentUserDetails: UsersModel;
  @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: false }) sort: MatSort;
  enumApprovalStatus = EnumApprovalStatus
  CompletedTrainees: [];

  constructor(private fb: FormBuilder, private ComSrv: CommonSrvService, public dialogRef: MatDialogRef<PSPBatchDialogueComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any) {
    this.pspbatchform = this.fb.group({
      PSPBatchID: 0,     
      BatchName: ["", Validators.required],
      TradeID: "",
      InActive: ""

    }, { updateOn: "blur" });
    this.formrights = ComSrv.getFormRights();
  }

  ngOnInit() {
    this.ComSrv.setTitle("PSP Batch");
    this.title = "Add New ";
    this.savebtn = "Save ";
    this.TradeID.setValue(this.data.TradeID);
    this.CompletedTrainees = this.data.TraineeData
  }

  Submit() {
    if (!this.pspbatchform.valid)
      return;
    this.working = true;
    this.pspbatchform.value['CompletedTrainees'] = this.CompletedTrainees;
    this.ComSrv.postJSON('api/PSPEmployment/SavePSPBatch', this.pspbatchform.value)
      .subscribe((d: any) => {
        if (d) {
          this.update = "New Batch Created Successfully";
          this.ComSrv.openSnackBar(this.update.toString(), "Updated"); 
        }
        this.reset();
        this.title = "Add New ";
        this.savebtn = "Save ";
      },
      (error) => {
        this.error = error.error;
        this.working = false;
        this.ComSrv.ShowError(error.error);
        
      });
  }

  onNoClick(): void {
    this.dialogRef.close(false);
  }

  reset() {
    this.pspbatchform.reset();

    this.title = "Add New ";
    this.savebtn = "Save ";
  }

  get PSPBatchID() { return this.pspbatchform.get("PSPBatchID"); }
  get BatchName() { return this.pspbatchform.get("BatchName"); }
  get TradeID() { return this.pspbatchform.get("TradeID"); }

}

