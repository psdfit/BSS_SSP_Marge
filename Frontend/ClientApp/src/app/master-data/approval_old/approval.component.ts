import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from '../../../environments/environment';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { CommonSrvService } from '../../common-srv.service';

import { ModelBase } from '../../shared/ModelBase';
import { UserRightsModel, UsersModel } from '../../master-data/users/users.component';

@Component({
  selector: 'app-approval',
  templateUrl: './approval.component.html',
  styleUrls: ['./approval.component.scss']
})
export class ApprovalComponent implements OnInit {
  approvalform: FormGroup;
  title: string;
  savebtn: string;
  displayedColumns = ['ProcessKey', 'ApprovalProcessName', 'Action'];
  approval: any[];
  Users: any[] = [];
  Steps: any[] = [];
  Process: any[] = [];
  hasAutoApproval = false;
  isAutoApproval = false;
  dtProcess: MatTableDataSource<any> = new MatTableDataSource([]);
  formrights: UserRightsModel;
  EnText = 'Approval';
  error: string;
  maxPendingStep = 0;

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  working: boolean;

  constructor(private fb: FormBuilder, private ComSrv: CommonSrvService) {
    this.approvalform = this.fb.group({
      ApprovalD: 0,
      ProcessKey: ['', Validators.required],
      Step: ['', [Validators.required, Validators.min(1)]],
      InActive: ''
    }, { updateOn: 'blur' });
    this.formrights = ComSrv.getFormRights();
    this.Step.valueChanges.subscribe((event) => {
      if (this.Steps.length < event) {
        if (this.isAutoApproval != true) {
          while (this.Steps.length < event) {
            // tslint:disable-next-line: max-line-length
            this.Steps.push({ Step: this.Steps.length + 1, UserIDs: [], ProcessKey: this.ProcessKey.value, IsAutoApproval: this.isAutoApproval });
          }
        }
        else
          this.Steps.push({ Step: 1, UserIDs: [15], ProcessKey: this.ProcessKey.value, IsAutoApproval: this.isAutoApproval });
      }
      else {
        this.Steps.splice(event, this.Steps.length - event);
        // this.Steps = this.Steps;
      }
    });
  }
  aEqualsOne(item) {
    const ind = this.Steps.indexOf(item);

    const Arr = this.Users.filter((m) => {
      let ret = true;
      this.Steps.forEach((it, i) => {

        if (i != ind) {
          if (m.UserID === it.UserID)
            ret = false
        }
      });
      return ret;
    });
    return Arr;
  }

  GetData() {
    this.ComSrv.getJSON('api/Approval/GetApproval').subscribe((d: any) => {
      this.approval = d[0]
      this.Process = d[1];
      this.dtProcess = new MatTableDataSource(d[1]);
      this.Users = d[2];
      this.Users = this.Users.filter(user => (user.UserLevel === 1 || user.UserLevel === 2));

      this.dtProcess.paginator = this.paginator;
      this.dtProcess.sort = this.sort;
    }, error => this.error = error // error path
    );
  }
  ngOnInit() {
    this.ComSrv.setTitle('Approval');
    this.title = 'Add New ';
    this.savebtn = 'Save ';
    this.GetData();
  }

  submit() {
    if (!this.approvalform.valid)
        return;
    if(this.Step.value >= this.maxPendingStep)
    {

      var nullStepCount = 0;

      if (!this.isAutoApproval) {
        this.Steps.forEach(item => {
          if (item.UserIDs[0] == 0 || item.UserIDs.length == 0) {
            nullStepCount++;
          }
        });
      };

      if (nullStepCount > 0) {
        this.ComSrv.ShowError('Please select user for each approval step');
        return;
      }

      //this.working = true;
      // convert userIDs Array to string
      this.Steps.filter(x => x.UserIDs = x.UserIDs.join(','));
      this.Steps.map(x => x.IsAutoApproval = this.isAutoApproval);



      this.ComSrv.postJSON('api/Approval/Save', this.Steps).subscribe((d: any) => {
          // tslint:disable-next-line: max-line-length
          this.ComSrv.openSnackBar(this.ApprovalD.value > 0 ? environment.UpdateMSG.replace('${Name}', this.EnText) : environment.SaveMSG.replace('${Name}', this.EnText));
          this.reset();
          // this.title = "Add New ";
          // this.savebtn = "Save ";
          //this.working = false;
        },
          (error) => {
            this.error = error.error;
            this.ComSrv.ShowError(error.error);
          }
      );
    }
    else
      this.ComSrv.ShowWarning('There are some pending ' + this.Steps[0].ApprovalProcessName + ' at Step ' + this.maxPendingStep +' that needs to be approved', '', 10000);
  }
  reset() {
    this.approvalform.reset();
    this.ApprovalD.setValue(0);
    this.hasAutoApproval = false;
    this.isAutoApproval = false;
    this.title = 'Add New ';
    this.savebtn = 'Save ';
  }
  toggleEdit(ProcessKey) {
    this.ProcessKey.setValue(ProcessKey);

    this.ComSrv.postJSON('api/Approval/RD_Approval', { ProcessKey })
      .subscribe((d: any) => {
        // convert userIDs string to integer array
        this.Steps = d[0].filter(x => x.UserIDs = x.UserIDs.split(',').map(Number));
        this.Step.setValue(d[0].length);
        this.hasAutoApproval = d[1];
        this.isAutoApproval = d[0][0].IsAutoApproval;
        if (this.isAutoApproval)
          this.Step.disable();
        else
          this.Step.enable();
      },
        error => this.error = error // error path
        , () => {
          this.working = false;
        });
    this.ComSrv.getJSON('api/Approval/GetMaxPendindingStep?ProcessKey=' + this.ProcessKey.value).subscribe((d: any) => {
        this.maxPendingStep = d;
      },
      error => this.error = error // error path
    );
    this.title = 'Update ';
    this.savebtn = 'Save ';
  }
  toggleActive(row) {
    this.ComSrv.confirm().subscribe(result => {
      if (result) {
        this.ComSrv.postJSON('api/Approval/ActiveInActive', { ApprovalD: row.ApprovalD, InActive: row.InActive }).subscribe((d: any) => {
            // tslint:disable-next-line: max-line-length
            this.ComSrv.openSnackBar(row.InActive === true ? environment.InActiveMSG.replace('${Name}', this.EnText) : environment.ActiveMSG.replace('${Name}', this.EnText));
          },
          error => this.error = error // error path
        );
      }
      else {
        row.InActive = !row.InActive;
      }
    });
  }
  assignSteps() {
    const event: number = this.Step.value;
    if (this.Steps.length < event) {
      while (this.Steps.length < event) {
        this.Steps.push({ Step: this.Steps.length + 1, UserID: 0, ApprovalProcessName: this.ProcessKey.value });
      }
    }
    else {
      this.Steps.slice(event + 1, this.Steps.length + 1);
    }
  }

  applyFilter(filterValue: string) {
    filterValue = filterValue.trim(); // Remove whitespace
    filterValue = filterValue.toLowerCase(); // Datasource defaults to lowercase matches
    this.dtProcess.filter = filterValue;
  }

  onChangeAutoApproval() {
    if (this.isAutoApproval) {
      this.ComSrv.getJSON('api/Approval/CheckPendingApprovalStep?ProcessKey=' + this.ProcessKey.value).subscribe((d: any) => {
        this.isAutoApproval = d;
        if (this.isAutoApproval) {
          this.Step.setValue(0);
          this.Step.setValue(1);
          this.Step.disable();
        }
        else
          this.ComSrv.ShowWarning('There are some pending ' + this.Steps[0].ApprovalProcessName + ' that needs to be approved', '', 10000);
      },
        error => this.error = error // error path
        , () => {
          this.working = false;
        }
      );
    }
    else
      this.Step.enable();
  }

  get ApprovalD() { return this.approvalform.get('ApprovalD'); }
  get ProcessKey() { return this.approvalform.get('ProcessKey'); }

  get Step() { return this.approvalform.get('Step'); }
  // get UserID() { return this.approvalform.get("UserID"); }
  get InActive() { return this.approvalform.get('InActive'); }
}
export class ApprovalModel extends ModelBase {
  ApprovalD: number;
  ProcessKey: string;
  Step: number;
  UserID: number;
  IsAtutoApproved: boolean;
}
