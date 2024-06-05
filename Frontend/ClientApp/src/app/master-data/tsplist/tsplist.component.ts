import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from '../../../environments/environment';
import { CommonSrvService } from '../../common-srv.service';
import { UserRightsModel } from '../../master-data/users/users.component';
import { ModelBase } from '../../shared/ModelBase';


@Component({
  selector: 'app-tsplist',
  templateUrl: './tsplist.component.html',
  styleUrls: ['./tsplist.component.scss']
})
export class TSPListComponent implements OnInit {
  tsplistform: FormGroup;
  title: string; savebtn: string;
  displayedColumns = ['TSPName', 'Address', 'NTN', 'PNTN', 'GST', 'FTN', 'InActive', "Action"];
  tspmaster: MatTableDataSource<any>;
  Tier: any; District = []; Scheme = [];
  formrights: UserRightsModel;
  EnText: string = "TSP Master";
  error: String;
  query = {
    order: 'TSPMasterID',
    limit: 5,
    page: 1
  };
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  working: boolean;
  constructor(private fb: FormBuilder, private ComSrv: CommonSrvService) {
    this.tsplistform = this.fb.group({
      TSPMasterID: 0,
      TSPName: ["", Validators.required],
      Address: ["", Validators.required],
     
      NTN: ["", Validators.required],
      PNTN: "",
      GST: "",
      FTN: ""
    }, { updateOn: "blur" });
    this.tspmaster = new MatTableDataSource([]);
    this.formrights = ComSrv.getFormRights();
  }


  GetData() {
      this.ComSrv.getJSON('api/TSPMaster/GetTSPMaster').subscribe((d: any) => {
        this.tspmaster = new MatTableDataSource(d[0]);
      //this.Tier = d[2];
      //this.District = d[1];
      //this.Scheme = d[3];

      this.tspmaster.paginator = this.paginator;
      this.tspmaster.sort = this.sort;
    }, error => this.error = error // error path
    );
  }
  ngOnInit() {
    this.ComSrv.setTitle("TSPDetail");
    this.title = "Add New ";
    this.savebtn = "Save ";
    this.GetData();
  }
  maskNTN = {
    mask: '000000-0'
    //,lazy: false
  };
  maskPNTN = {
    mask: '0000000-0'
    //,lazy: false
  };
  maskGST = {
    mask: '0000000-0'
    //,lazy: false
  };
  maskFTN = {
    mask: '000000-0'
    //,lazy: false
  }
  Submit() {
    if (!this.tsplistform.valid)
      return;
    this.working = true;
      this.ComSrv.postJSON('api/TSPMaster/Save', this.tsplistform.value)
      .subscribe((d: any) => {
        this.tspmaster = new MatTableDataSource(d);
        this.tspmaster.paginator = this.paginator;
        this.tspmaster.sort = this.sort;
        this.ComSrv.openSnackBar(this.TSPMasterID.value > 0 ? environment.UpdateMSG.replace("${Name}", this.EnText) : environment.SaveMSG.replace("${Name}", this.EnText));
        this.reset();
        this.title = "Add New ";
        this.savebtn = "Save ";
        this.working = false;
      },
      (error) => {
        this.error = error.error;
        this.ComSrv.ShowError(error.error);
        
      });


  }
  reset() {
    this.tsplistform.reset();
    this.TSPMasterID.setValue(0);
    this.title = "Add New ";
    this.savebtn = "Save ";
  }
  toggleEdit(row) {
    this.TSPMasterID.setValue(row.TSPMasterID);
    this.TSPName.setValue(row.TSPName);
    this.Address.setValue(row.Address);
    this.NTN.setValue(row.NTN);
    this.PNTN.setValue(row.PNTN);
    this.GST.setValue(row.GST);
    this.FTN.setValue(row.FTN);
    this.title = "Update ";
    this.savebtn = "Save ";
  }
  toggleActive(row) {
    this.ComSrv.confirm().subscribe(result => {
      if (result) {
        this.ComSrv.postJSON('api/TSPMaster/ActiveInActive', { 'TSPMasterID': row.TSPMasterID, 'InActive': row.InActive })
          .subscribe((d: any) => {
            this.ComSrv.openSnackBar(row.InActive == true ? environment.InActiveMSG.replace("${Name}", this.EnText) : environment.ActiveMSG.replace("${Name}", this.EnText));
            // this.tspmaster =new MatTableDataSource(d);
          },
          (error) => {
            this.error = error.error;
            this.ComSrv.ShowError(error.error);
            row.InActive = !row.InActive;
          });
  }
      else {
  row.InActive = !row.InActive;
}
    });
        }
applyFilter(filterValue: string) {
  filterValue = filterValue.trim(); // Remove whitespace
  filterValue = filterValue.toLowerCase(); // Datasource defaults to lowercase matches
  this.tspmaster.filter = filterValue;
}

get TSPMasterID() { return this.tsplistform.get("TSPMasterID"); }
get TSPName() { return this.tsplistform.get("TSPName"); }
get Address() { return this.tsplistform.get("Address"); }
get TSPCode() { return this.tsplistform.get("TSPCode"); }
get TSPColor() { return this.tsplistform.get("TSPColor"); }
get TierID() { return this.tsplistform.get("TierID"); }
get NTN() { return this.tsplistform.get("NTN"); }
get PNTN() { return this.tsplistform.get("PNTN"); }
get GST() { return this.tsplistform.get("GST"); }
get FTN() { return this.tsplistform.get("FTN"); }

}
export class TSPDetailModel extends ModelBase {
  TSPMasterID: number;
  TSPName: string;
  Address: string;
  TSPCode: string;
  TSPColor: string;
  TierID: number;
  NTN: number;
  PNTN: number;
  GST: string;
  FTN: number;
  TspStatusID_OLD: number;
  DistrictID: number;
  HeadName: string;
  HeadDesignation: string;
  HeadEmail: string;
  HeadLandline: string;
  OrgLandline: string;
  CPName: string;
  CPDesignation: string;
  CPLandline: string;
  CPEmail: string;
  Website: string;
  CPAdmissionsName: string;
  CPAdmissionsDesignation: string;
  CPAdmissionsLandline: string;
  CPAdmissionsEmail: string;
  CPAccountsName: string;
  CPAccountsDesignation: string;
  CPAccountsLandline: string;
  CPAccountsEmail: string;
  BankName: string;
  BankAccountNumber: string;
  AccountTitle: string;
  BankBranch: string;
  OrganizationID: number;
  SchemeID: number;

}
