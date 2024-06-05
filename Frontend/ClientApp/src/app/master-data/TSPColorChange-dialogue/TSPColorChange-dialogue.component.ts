
import { Component, Inject, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from '../../../environments/environment';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { CommonSrvService } from '../../common-srv.service';
import { ModelBase } from '../../shared/ModelBase';
import { UserRightsModel } from '../../master-data/users/users.component';
import { Observable } from 'rxjs';
import { DialogueService } from 'src/app/shared/dialogue.service';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Router } from '@angular/router';


@Component({
  selector: 'app-TSPColorChange-dialogue',
  templateUrl: './TSPColorChange-dialogue.component.html',
  styleUrls: ['./TSPColorChange-dialogue.component.scss']
})
export class TSPColorChangedialogueComponent implements OnInit {
  GetTSPColor: [];
  formrights: UserRightsModel;
  TSPColorform: FormGroup;
  error: any;
  GetTSPMaster: any;
  EnText: string = "TSP Color Change";
  constructor(private fb: FormBuilder,private http: CommonSrvService,
    private router: Router, public dialogRef: MatDialogRef<TSPColorChangedialogueComponent>,
  @Inject(MAT_DIALOG_DATA) public data: any) {
    this.TSPColorform = this.fb.group({
      TSPMasterID: 0,
      TSPColorID: ["", Validators.required],
      InActive: ""
    }, { updateOn: "blur" });
    this.formrights = http.getFormRights();
   }
  ngOnInit(): void {
    this.GetTSPMaster=this.data.data;
    this.TSPColorChangeDll();
  }
  Submit() {
    debugger;
    this.TSPColorform.value.TSPMasterID = this.data.data.TSPMasterID;
    this.TSPColorform.value.TSPColorCode = this.TSPColorID.value.TSPColorCode;
    this.TSPColorform.value.TSPColorID = this.TSPColorID.value.TSPColorID;
    if (!this.TSPColorform.valid)
      return;
    this.http.postJSON('api/TSPColor/saveTSPColor', this.TSPColorform.value).subscribe((d: any) => {
       this.http.openSnackBar(this.TSPMasterID.value > 0 ? environment.UpdateMSG.replace("${Name}", this.EnText) : environment.SaveMSG.replace("${Name}", this.EnText));
       this.TSPColorID.setValue(0);
       debugger;
       
       this.changeLocation();
    },
    (error) => {
      this.error = error.error;
      this.http.ShowError(error.error);
      
    });
  }
  changeLocation() {

    // save current route first
    const currentRoute = this.router.url;

    this.router.navigateByUrl('/', { skipLocationChange: true }).then(() => {
        this.router.navigate([currentRoute]); // navigate to same route
    }); 
}
  TSPColorChangeDll() {
    debugger;
    this.http.getJSON("api/TSPColor/GetTSPColor/").subscribe(
      (data: any) => {
        debugger;
        this.GetTSPColor = data;
      },
      error => {
        this.http.ShowError(error, "Error");
      }
    );
  }
  onNoClick(): void {
    this.dialogRef.close(false);
  }
  get TSPMasterID() { return this.TSPColorform.get("TSPMasterID"); }
  get TSPColorID() { return this.TSPColorform.get("TSPColorID"); }
  get TSPColorName() { return this.TSPColorform.get("TSPColorName"); }
}
export class genderModel extends ModelBase {
  TSPMasterID: number;
    TSPName: string;
    Address: string;
    TSPCode: string;
    TSPColorID: number;
    TSPColorName: string;
    TSPColorHistoryID: string;
    NTN:string;
}

