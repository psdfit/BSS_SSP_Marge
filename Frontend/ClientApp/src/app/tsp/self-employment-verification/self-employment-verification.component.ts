import { Component, OnInit } from '@angular/core';

import { CommonSrvService } from '../../common-srv.service';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { UserRightsModel } from '../../master-data/users/users.component';
import { DialogueService } from '../../shared/dialogue.service';
import { EnumApprovalStatus, EnumApprovalProcess } from '../../shared/Enumerations';
import { IEmpVarificationFilter } from '../Interface/IVarificationFilter';
import { EmpVerificationComponent } from '../employment-verification/employment-verification.component';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-self-employment-verification',
  templateUrl: './self-employment-verification.component.html',
  styleUrls: ['./self-employment-verification.component.scss']
})
export class SelfEmploymentVerificationComponent implements OnInit {
  placementTypes: any[];
  placementTypeID: any;
  verificationMethods: any[];
  verificationMethodsDrp: any[];
  verificationMethodID: any;
  ClassList: [];
  TraineeList: [];
  ApprovalData: any;
  error: any;
  working: boolean;
  filters: IEmpVarificationFilter = { ClassID: 0, TSPID : 0,PlacementTypeID : 0, VerificationMethodID : 0 };
  EmploymentTypeID: number;
  constructor(private http: CommonSrvService, private dialogue: DialogueService, public dialog: MatDialog) { }

  ngOnInit(): void {
    this.http.setTitle("Call Center Employment Verification");
    this.getVerificationDropdowns();
    this.GetClasses();
    
  }

  getVerificationDropdowns() {
    this.http.getJSON(`api/TSPEmployment/EmployeeVerificationDropdown`).subscribe(
      (d: any) => {
        this.placementTypes = d.placementTypes;
        this.verificationMethods = d.verificationMethods;
        let pId = this.placementTypes.length > 0 ? this.placementTypes[0].PlacementTypeID : 1;
        this.filters.PlacementTypeID = pId;
        this.verificationMethodsDrp = this.verificationMethods.filter(x => x.PlacementTypeID == pId);
        this.GetSelfEmploymentList();
      }
      , (error) => {
        console.error(JSON.stringify(error));
      }
    );
  }

  placementTypeChange(e) {
    this.filters.VerificationMethodID = 0;
    this.verificationMethodsDrp = this.verificationMethods.filter(x => x.PlacementTypeID == e.value);
    this.GetSelfEmploymentList();
  }
  
  GetClasses() {
    this.http.getJSON(`api/TSPEmployment/GetClass`).subscribe(
      (d: any) => {
        this.ClassList = d.ClassList;
        console.log(d);
      }
      , (error) => {
        console.error(JSON.stringify(error));
      }
    );
  }
  toggleVerify(Row: any) {
    this.http.postJSON("api/EmploymentVerification/UpdateVerifyStatus/", { PlacementID: Row.PlacementID, IsVerified: true }).subscribe(
      (d: any) => {
        //this.TraineeList = d.FormalList;
        this.GetSelfEmploymentList();
        console.log(d);
      }
      , (error) => {
        console.error(error);
      }
    );
  };
  GetSelfEmploymentList() {

    this.http.getJSON(`api/TSPEmployment/GetSelfEmploymentList?pId=${this.filters.PlacementTypeID}&vmId=${this.filters.VerificationMethodID}&cId=${this.filters.ClassID}`).subscribe(
      (d: any) => {
        this.TraineeList = d.SelfList;
        this.EmploymentTypeID = d.TypeID;
        console.log(d);
      }
      , (error) => {
        console.error(error);
      }
    );
  }
  openDialog(id: number, name: string): void {
    const dialogRef = this.dialog.open(EmpVerificationComponent, {
      minWidth: '1000px',
      minHeight: '600px',
      //data: JSON.parse(JSON.stringify(row))
      data: { "PlacementID": id, "TraineeName": name, "EmploymentTypeID": this.EmploymentTypeID }
      //this.GetVisitPlanData(data)
    })
    dialogRef.afterClosed().subscribe(result => {
      console.log(result);
      //this.visitPlan = result;
      //this.submitVisitPlan(result);
    })
  }
}
