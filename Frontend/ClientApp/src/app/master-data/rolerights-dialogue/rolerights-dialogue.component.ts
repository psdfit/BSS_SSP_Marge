
//
import { Component, OnInit, Inject } from "@angular/core";
import { CommonSrvService } from "../../common-srv.service";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";
import { FormGroup, FormControl, Validators } from "@angular/forms";
import { EnumApprovalStatus } from "../../shared/Enumerations";
import { UsersModel } from "../../master-data/users/users.component";
import { Router } from "@angular/router";

@Component({
  selector: 'app-rolerights-dialogue-dialogue',
  templateUrl: './rolerights-dialogue.component.html',
  styleUrls: ['./rolerights-dialogue.component.scss']
})
export class rolerightsdialogueComponent implements OnInit {
  currentUserDetails: UsersModel;
  SelectedPermission=[];
  constructor(private router: Router,
    private http: CommonSrvService,
    public dialogRef: MatDialogRef<rolerightsdialogueComponent>,
    @Inject(MAT_DIALOG_DATA) public data:any
  ) {
   
  }

  ngOnInit(): void {
    debugger;
    this.data;
    this.SelectedPermission=this.data.data.DiffRoleRights;
  }
  Save() {
    
     this.http.postJSON('api/Roles/Save', this.data.data)
      .subscribe((d: any) => {
        //this.router.navigate(['master/roles']);
        location.reload();
      },
      (error) => {
        this.http.ShowError(error.error);
        
      }); 
  }
  onNoClick(): void {
    this.dialogRef.close(false);
  }
}
