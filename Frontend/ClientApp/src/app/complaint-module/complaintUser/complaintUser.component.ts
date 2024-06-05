import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from '../../../environments/environment';
import { FormGroup, FormBuilder, Validators, ReactiveFormsModule, FormControl, ValidatorFn } from '@angular/forms';
import { CommonSrvService } from '../../common-srv.service';
import { ModelBase } from '../../shared/ModelBase';
import { UserRightsModel } from '../../master-data/users/users.component';
import { Observable } from 'rxjs';
import { DialogueService } from 'src/app/shared/dialogue.service';
import { DomSanitizer } from '@angular/platform-browser';
import { MatTabGroup } from '@angular/material/tabs';
@Component({
  selector: 'app-complaintUser',
  templateUrl: './complaintUser.component.html',
  styleUrls: ['./complaintUser.component.scss']
})
export class complaintUserComponent implements OnInit {
  @ViewChild("tabGroup") tabGroup: MatTabGroup;
  @ViewChild('myInput') myInputVariable: ElementRef;
  @ViewChild('resumeInput', {static: true}) resumeInput;  
  title: string; savebtn: string;submittedtitle: string;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  ComplaintUser: MatTableDataSource<any>;
  Complaintuser: FormGroup;
  formrights: UserRightsModel;
  EnText: string = "User";
  error: String;
  Steps: any[] = [];
  ComplaintTypeIDFilter = new FormControl(0);
  displayedColumns = ['ComplaintTypeName','ComplaintSubTypeName','Users',"Actions"];
  ComplaintTypeddl = [];
  Users: any[] = [];
  ComplaintTypeFirCURD = [];
  ComplaintSubTypedll = [];
  hiddenComplainttypeID: number;
  selectedFile: string;
  ext: string;
  SearchCls = new FormControl("");
  working: boolean;
  complaintTypeValueForDll:number;
  constructor(private fb: FormBuilder, private http: CommonSrvService, public dialogueService: DialogueService,
    public domSanitizer: DomSanitizer) {
    this.Complaintuser = this.fb.group({
      ComplaintUserID: 0,
      ComplaintTypeID: ["", Validators.required],
      UserID: ["", Validators.required],
      ComplaintSubTypeID: ["", Validators.required],
      InActive: ""
    }, { updateOn: "blur" });
    this.ComplaintUser = new MatTableDataSource([]);
    this.formrights = http.getFormRights();
    this.ComplaintTypeIDFilter.valueChanges.subscribe(value => { this.GetComplaintTSubTypeByComplaintType(value); });
  }
  //fileUrl;
  ngOnInit(): void {
    this.http.setTitle("Complaint Users");
    this.GetComplaintTypeForCRUD();
    this.GFetchUsersForCRUD();
    this.FetchComplaintUserForGridView();
    this.title = "Assign ";
    this.submittedtitle="Save & Submitted";
    this.savebtn = "Save ";
  }
  onCategoryChange() {
    debugger;
  }
  reset() {
    this.Complaintuser.reset();
    this.title = "Assign ";
    this.savebtn = "Save ";
  }
  Submit() {
    debugger;

    if (!this.Complaintuser.valid)
      return;
      this.working = true;
    this.http.postJSON('api/ComplaintUser/save', this.Complaintuser.value).subscribe((d: any) => {
      debugger;
      if(d==true){
        this.FetchComplaintUserForGridView();
        this.http.openSnackBar(this.ComplaintUserID.value > 0 ? environment.UpdateMSG.replace("${Name}", this.EnText) : environment.SaveMSG.replace("${Name}", this.EnText));
        debugger;
        this.reset();
        this.ComplaintTypeID.setValue(0);
        this.title = "Assign ";
        this.savebtn = "Save ";
        this.working = false;
      }else{
        //this.reset();
        this.http.ShowError("Sorry record already exists");
      }
    
      
    },
    (error) => {
      this.error = error.error;
      this.working = false;
      this.http.ShowError(error.error);
    });
  }
  toggleEdit(row) {
    debugger;
    this.ComplaintUserID.setValue(row.ComplaintUserID);
    this.Complaintuser.controls['ComplaintTypeID'].patchValue(row.ComplaintTypeID);   
    this.complaintTypeValueForDll=row.ComplaintSubTypeID;
    this.GetComplaintTSubTypeByComplaintType(row.ComplaintTypeID)
      this.Steps = Array.from(row.UserIDs.split(','),Number);
    this.title = "Update ";
    this.savebtn = "Update ";
}
  GetComplaintTSubTypeByComplaintType (ComplaintTypeID: number)  {
    this.hiddenComplainttypeID = ComplaintTypeID;
    this.http.getJSON(`api/Complaint/GetComplaintTSubTypeByComplaintType?ComplaintTypeID=` + ComplaintTypeID)
      .subscribe((data: any) => {
        debugger;
        this.ComplaintSubTypedll = data[0];
        this.Complaintuser.controls['ComplaintSubTypeID'].patchValue( this.complaintTypeValueForDll);
      }, error => {
        this.error = error;
      })
  }
  GetComplaintTypeForCRUD() {
    debugger;
    this.http.getJSON('api/Complaint/GetComplaintTypeForCRUD').subscribe((d: any) => {
      debugger;
      this.ComplaintTypeddl = d;
    }, error => this.error = error // error path
    );
  }
  GFetchUsersForCRUD() {
    debugger;
    this.http.getJSON('api/Complaint/FetchUsers').subscribe((d: any) => {
      debugger;
      this.Users = d;
    }, error => this.error = error // error path
    );
  }
  FetchComplaintUserForGridView() {
    debugger;
    this.http.getJSON('api/ComplaintUser/FetchComplaintUserForGridView').subscribe((d: any) => {
      debugger;
      this.populateComplaintList(d[0]);
    }, error => this.error = error // error path
    );
  }
  applyFilter(filterValue: string) {
    filterValue = filterValue.trim(); // Remove whitespace
    filterValue = filterValue.toLowerCase(); // Datasource defaults to lowercase matches
    this.ComplaintUser.filter = filterValue;
}
EmptyCtrl() {
  this.SearchCls.setValue("");
}
populateComplaintList(d: any) {
  this.ComplaintUser = new MatTableDataSource(d);
  this.ComplaintUser.paginator = this.paginator;
  this.ComplaintUser.sort = this.sort;
}
  get ComplaintUserID() { return this.Complaintuser.get("ComplaintUserID"); }
  get ComplaintTypeID() { return this.Complaintuser.get("ComplaintTypeID"); }
  get ComplaintTypeName() { return this.Complaintuser.get("ComplaintTypeName"); }
  get ComplaintSubTypeID() { return this.Complaintuser.get("ComplaintSubTypeID"); }
  get ComplaintSubTypeName() { return this.Complaintuser.get("ComplaintSubTypeName"); }
   get Step() { return this.Complaintuser.get("Step"); }
  get UserID() { return this.Complaintuser.get("UserID"); }
}
export class ComplaintModel extends ModelBase {
  Step: number;
  ComplaintUserID: number;
  UserID:number;
  ComplaintStatus: string;
  ComplaintTypeID: number;
  ComplaintTypeName: string;
  ComplaintSubTypeID: number;
  ComplaintSubTypeName: string;
}
