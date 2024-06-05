/* **** Aamer Rehman Malik *****/
import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from '../../../environments/environment';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { CommonSrvService } from '../../common-srv.service';
import { UserRightsModel } from '../users/users.component';
import { DialogueService } from 'src/app/shared/dialogue.service';
import { rolerightsdialogueComponent } from '../rolerights-dialogue/rolerights-dialogue.component';
import { MatDialog } from '@angular/material/dialog';
@Component({
  selector: 'hrapp-roles',
  templateUrl: './roles.component.html',
  styleUrls: ['./roles.component.scss']
})
export class RolesComponent implements OnInit {
  rolesform: FormGroup;
  title: string; savebtn: string;
  displayedColumns = ['RoleTitle', 'InActive', "Action"];
  roles: MatTableDataSource<any>;
  EnText: string = "Role";
  error: String;
  appforms = []; rolesrights = [];
  Previousrolesrights=[];
  Diffirencerolesrights=[];
  temp=[];
  rights: any;
  query = {
    order: 'RoleID',
    limit: 5,
    page: 1
  };
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  working: boolean;
  constructor(private fb: FormBuilder, private http: CommonSrvService,public dialogueService: DialogueService,public dialog: MatDialog) {
    this.rolesform = this.fb.group({
      RoleID: 0,
      RoleTitle: ["", Validators.required],
      InActive: ""
    }, { updateOn: "blur" });
    this.roles = new MatTableDataSource([]);
    this.rights = http.getFormRights();
    this.RoleID.valueChanges.subscribe((val) => {
      if (val == null)
        this.RoleID.setValue(0);
    });
  }

  GetData() {
    this.http.getJSON('api/Roles/GetRoles').subscribe((d: any) => {
      this.roles = new MatTableDataSource(d[0]);
      this.rolesrights = d[1];
      this.appforms = d[1];
      this.roles.paginator = this.paginator;
      this.roles.sort = this.sort;
    }, error => this.error = error // error path
    );
  }
  ngOnInit() {
    this.http.setTitle("Manage Roles");
    this.title = "Add New ";
    this.savebtn = "Save ";
    this.GetData();
  }
 
  arrDifference(arrA, arrB) {
    return arrA.filter(item => 
      arrB.find(x => (x.CanAdd == item.CanAdd 
      && x.CanEdit == item.CanEdit
      && x.CanDelete == item.CanDelete
      && x.CanView == item.CanView)&&x.RoleRightID==item.RoleRightID) == undefined
      )
  }

  Submit() {
    if (!this.rolesform.valid)
      return;
      if(this.RoleID.value>0)
      {
        this.Diffirencerolesrights=null;
        this.Diffirencerolesrights= this.arrDifference(this.rolesrights,this.Previousrolesrights);
      }
   // this.working = true;
    this.rolesform.value["RoleRights"] = this.rolesrights;
    if(this.RoleID.value>0)
    {
      this.rolesform.value["DiffRoleRights"] = this.Diffirencerolesrights;
      this.openHistoryDialogue(this.rolesform.value);
    }
    else
    {
      this.http.postJSON('api/Roles/Save', this.rolesform.value)
      .subscribe((d: any) => {
        this.roles = new MatTableDataSource(d);
        this.roles.paginator = this.paginator;
        this.roles.sort = this.sort;
        this.http.openSnackBar(this.RoleID.value > 0 ? environment.UpdateMSG.replace("${Name}", this.EnText) : environment.SaveMSG.replace("${Name}", this.EnText));
        this.reset();
        this.title = "Add New ";
        this.savebtn = "Save ";
        this.working = false;
      },
      (error) => {
        this.error = error.error;
        this.http.ShowError(error.error);
        
      });
    }
   
 
  }
  reset() {
    this.rolesform.reset();
    this.rolesrights = this.appforms;
    this.Previousrolesrights=null;
    this.title = "Add New ";
    this.savebtn = "Save ";
  }
  toggleEdit(row) {
    this.RoleID.setValue(row.RoleID);
    this.RoleTitle.setValue(row.RoleTitle);
    this.InActive.setValue(row.InActive);
    this.http.getJSON('api/Roles/GetRoleRights/' + row.RoleID).subscribe((REs: any) => {
      this.rolesrights = JSON.parse(JSON.stringify( REs));
      this.Previousrolesrights= JSON.parse(JSON.stringify( REs));
    

    }, error => this.error = error // error path
    );
    this.title = "Update ";
    this.savebtn = "Save ";
  }
  toggleActive(row) {
    this.http.confirm().subscribe(result => {
      if (result) {
        this.http.postJSON('api/Roles/ActiveInActive', { 'RoleID': row.RoleID, 'InActive': row.InActive })
          .subscribe((d: any) => {
            this.http.openSnackBar(row.InActive ? environment.InActiveMSG.replace("${Name}", this.EnText) : environment.ActiveMSG.replace("${Name}", this.EnText));
            // this.roles =new MatTableDataSource(d);
          },
          (error) => {
            this.error = error.error;
            this.http.ShowError(error.error);
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
    this.roles.filter = filterValue;
  }

  get RoleID() { return this.rolesform.get("RoleID"); }
  get RoleTitle() { return this.rolesform.get("RoleTitle"); }
  get InActive() { return this.rolesform.get("InActive"); }
  SelAdd(cur: any, All: UserRightsModel[], Prop: string) {
    All.forEach((m: UserRightsModel) => {
      m[Prop] = cur;
    });
  };
  openHistoryDialogue(data: any): void {
    debugger;
    this.dialogueService.openRoleRightsDialogue(data);
  }

 /*  openHistoryDialogue(data: any): void {

    const dialogRef = this.dialog.open(rolerightsdialogueComponent, {
      width: '62%',
      data: { data: data }
     
    })
    dialogRef.afterClosed().subscribe(result => {
      
        this.GetData();
        this.http.openSnackBar(this.RoleID.value > 0 ? environment.UpdateMSG.replace("${Name}", this.EnText) : environment.SaveMSG.replace("${Name}", this.EnText));
        this.reset();
        this.title = "Add New ";
        this.savebtn = "Save ";
        this.working = false;
      
    })
  } */

}
