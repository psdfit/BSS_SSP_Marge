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
  selector: 'app-rtp-approval-dialog',
  templateUrl: './rtp-approval-dialogue.component.html',
  styleUrls: ['./rtp-approval-dialogue.component.scss']
})
export class RTPApprovalDialogueComponent implements OnInit {
  approvalHistory: IApprovalHistory[] = [];
  rtpapprovalform: FormGroup;
  title: string; savebtn: string;
  formrights: UserRightsModel;
  EnText: string = "RTP";

  error: String;
  update: String;
  working: boolean;


  //canSendBack: boolean = false;
  RejectRTPDialogue: boolean = false;
  isValidApprover: boolean = false;
  ApprovalFlag: boolean = false;
  NTPFlag: boolean = false;
  isAlreadyApproved: boolean = false;
  approvedrtp: MatTableDataSource<any>;


  latest: IApprovalHistory;
  currentUserDetails: UsersModel;
  @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: false }) sort: MatSort;
  enumApprovalStatus = EnumApprovalStatus
  constructor(private fb: FormBuilder, private ComSrv: CommonSrvService, public dialogRef: MatDialogRef<RTPApprovalDialogueComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any) {
    this.rtpapprovalform = this.fb.group({
      RTPID: 0,
      //RTPValue: "",
      //Scheme: "",
      //TSP: "",
      //DateOfRequest: "",
      //DateOfRequest: "",
      ClassID: "",
      ClassCode: ["", Validators.required],
      //Trade: "",
      //Trainees: "",
      //Duration: "",
      //AddressOfTrainingLocation: "",
      Comments: ["", Validators.required],
      IsApproved: "",
      IsRejected: "",
      NTP: 0,
      InActive: ""

    }, { updateOn: "blur" });
    this.approvedrtp = new MatTableDataSource([]);
    this.formrights = ComSrv.getFormRights();
  }


  GetData() {
    this.ComSrv.getJSON('api/RTP/GetRTP').subscribe((d: any) => {
      this.approvedrtp = new MatTableDataSource(d);
      this.approvedrtp.paginator = this.paginator;
      this.approvedrtp.sort = this.sort;
    }, error => this.error = error // error path
    );
  }

  ngOnInit() {
    this.ComSrv.setTitle("RTP");
    this.title = "Add New ";
    this.savebtn = "Save ";

    this.ClassCode.setValue(this.data.ClassCode)
    this.ClassID.setValue(this.data.ClassID)

    this.NTPFlag = this.data.NTP
    this.ApprovalFlag = this.data.IsApproved
    if (this.data.OnRejectionDialogue) {
      this.RejectRTPDialogue = this.data.OnRejectionDialogue
    }
    //this.RTPValue.setValue(this.data.RTPValue)
    //this.GetData();
  }

  Submit() {
    if (!this.rtpapprovalform.valid)
      return;
    this.working = true;
    this.ComSrv.postJSON('api/RTP/ApproveRTP', this.rtpapprovalform.value)
      .subscribe((d: any) => {
        this.approvedrtp = new MatTableDataSource(d);
        this.approvedrtp.paginator = this.paginator;
        this.approvedrtp.sort = this.sort;
        if (this.ApprovalFlag) {
          this.update = "RTP Approved";
          this.ComSrv.openSnackBar(this.update.toString(), "Updated");
        }
        if (!this.ApprovalFlag) {
          this.error = "RTP Rejected";
          this.ComSrv.ShowError(this.error.toString(), "error");
        }
        this.reset();
        this.title = "Add New ";
        this.savebtn = "Save ";
      },
        error => this.error = error // error path
        , () => {
          this.working = false;

        });


  }

  ApproveRTP() {
    if (!this.rtpapprovalform.valid) {
      this.error = "Please Enter Comments to approve";
      this.ComSrv.ShowError(this.error.toString(), "error");
      return;
    }
    this.IsApproved.setValue(true);
    this.IsRejected.setValue(false);
    this.ApprovalFlag = true;
    this.Submit();

  }


  RejectRTP() {
    if (!this.rtpapprovalform.valid) {
      this.error = "Please Enter Comments to reject";
      this.ComSrv.ShowError(this.error.toString(), "error");
      return;
    }
    this.IsRejected.setValue(true);
    this.IsApproved.setValue(false);
    this.ApprovalFlag = false;

    this.Submit();

  }

  onNoClick(): void {
    this.dialogRef.close(false);
  }



  reset() {
    this.rtpapprovalform.reset();
    //myform.resetFrom();
    //this.RTPID.setValue(0);
    this.title = "Add New ";
    this.savebtn = "Save ";
  }
  toggleEdit(row) {
    //this.RTPID.setValue(row.RTPID);
    //this.ClassCode.setValue(row.ClassCode);
    //this.AddressOfTrainingLocation.setValue(row.AddressOfTrainingLocation);

    this.title = "Update ";
    this.savebtn = "Save ";
  }
  toggleActive(row) {
    this.ComSrv.confirm().subscribe(result => {
      if (result) {
        this.ComSrv.postJSON('api/RTP/ActiveInActive', { 'RTPID': row.RTPID, 'InActive': row.InActive })
          .subscribe((d: any) => {
            this.ComSrv.openSnackBar(row.InActive == true ? environment.InActiveMSG.replace("${Name}", this.EnText) : environment.ActiveMSG.replace("${Name}", this.EnText));
            // this.rtp =new MatTableDataSource(d);
          },
            error => this.error = error // error path
          );
      }
      else {
        row.InActive = !row.InActive;
      }
    });
  }
  applyFilter(filterValue: string) {
    filterValue = filterValue.trim(); // Remove whitespace
    filterValue = filterValue.toLowerCase(); // Datasource defaults to lowercase matches
    this.approvedrtp.filter = filterValue;
  }

  //get RTPID() { return this.rtpapprovalform.get("RTPID"); }
  //get RTPValue() { return this.rtpapprovalform.get("RTPValue"); }
  get ClassID() { return this.rtpapprovalform.get("ClassID"); }
  get RTPID() { return this.rtpapprovalform.get("RTPID"); }
  get ClassCode() { return this.rtpapprovalform.get("ClassCode"); }
  get Comments() { return this.rtpapprovalform.get("Comments"); }
  get IsApproved() { return this.rtpapprovalform.get("IsApproved"); }
  get IsRejected() { return this.rtpapprovalform.get("IsRejected"); }
  //get InActive() { return this.rtpapprovalform.get("InActive"); }
}
export interface IApprovalHistory {
  ApprovalHistoryID?: number;
  ProcessKey: string;
  Step?: number;
  FormID: number;
  ApproverID?: number;
  ApprovalStatusID?: number;
  Comments?: string;

  ApproverName?: string;
  ApproverIDs?: string;
  ApproverNames?: string;
  StatusDisplayName?: string;
  ModifiedDate?: Date;
  CreatedDate?: Date;
}

