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
  selector: 'app-formal-employment-verification',
  templateUrl: './formal-employment-verification.component.html',
  styleUrls: ['./formal-employment-verification.component.scss']
})
export class FormalEmploymentVerificationComponent implements OnInit {
  TraineeList: [];
  ClassList: [];
  ApprovalData: any;
  error: any;
  working: boolean;
  filters: IEmpVarificationFilter = { ClassID: 0,TSPID:0, PlacementTypeID : 0, VerificationMethodID : 0 };
  formrights: UserRightsModel;
  EmploymentTypeID: number;
  constructor(private http: CommonSrvService, private dialogue: DialogueService, public dialog: MatDialog) { }

  ngOnInit(): void {
    this.formrights = this.http.getFormRights();

    this.http.setTitle("Data Entry Operator Employment Verification");
    this.GetClasses();
    this.GetFormalEmploymentList();
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
        this.GetFormalEmploymentList();
        console.log(d);
      }
      , (error) => {
        console.error(error);
      }
    );
  };
  GetFormalEmploymentList() {

    this.http.getJSON("api/EmploymentVerification/GetFormalEmploymentList/" + this.filters.ClassID).subscribe(
      (d: any) => {
        this.TraineeList = d.FormalList;
        this.EmploymentTypeID = d.TypeID;
        console.log(d);
      }
      , (error) => {
        console.error(error);
      }
    );
  }

  openDialog(id: number,name: string): void {
    const dialogRef = this.dialog.open(EmpVerificationComponent, {
      minWidth: '1000px',
      minHeight: '600px',
      //data: JSON.parse(JSON.stringify(row))
      data: { "PlacementID": id, "TraineeName": name, "EmploymentTypeID": this.EmploymentTypeID}
      //this.GetVisitPlanData(data)
    })
    dialogRef.afterClosed().subscribe(result => {
      console.log(result);
      //this.visitPlan = result;
      //this.submitVisitPlan(result);
    })
  }

}
