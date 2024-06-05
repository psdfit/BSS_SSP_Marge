/* ****Aamer Rehman Malik *****/
import { Component, OnInit, ViewChild, AfterViewInit, ElementRef } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormGroupDirective, FormControl } from '@angular/forms';
import { CommonSrvService } from '../../common-srv.service';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from '../../../environments/environment';
import { confirmPasswordValidator } from '../../security/ourvalidators';
import { ModelBase } from 'src/app/shared/ModelBase';
import { merge, Observable, of as observableOf } from 'rxjs';
import { startWith, switchMap, map, catchError } from 'rxjs/operators';
import { MatSlideToggle } from '@angular/material/slide-toggle';
import { debug } from 'webpack';
import { HttpParams } from '@angular/common/http';

export class UserRightsModel {
    UserRightID: number;
    FormID: number;
    UserID: number;
    CanAdd: Boolean;
    CanEdit: Boolean;
    CanDelete: Boolean;
    CanView: Boolean;
    ModuleTitle: string;
}
export class UserOrganizationModel {
    Srno: number;
    OID: number;
    UserID: number;
    OName: string;
}
export class UsersModel extends ModelBase {
    UserID: number;
    UserName: string;
    Fname: string;
    lname: string;
    FullName: string;
    RoleID: number;
    LoginDT: string;
    Token: string;
    RoleName: number;
    UserLevel: number;
    UserImage: string;
    CnicNo: string;
    Email: string;
    CellNo: string;
    UserAddress: string;
    RoleTitle: string;
    ResetString: string;
    SessionID: string;
    Userpassword: string;
    AllowedSessions: number;
}

@Component({
    selector: 'hrapp-users',
    templateUrl: './users.component.html',
    styleUrls: ['./users.component.scss']
})
export class UsersComponent implements OnInit, AfterViewInit {
    usersform: FormGroup;
    title: string; savebtn: string;
    displayedColumns = ['UserName', 'FullName', 'Email', 'RoleID', 'InActive', "Action"];
    users: UsersModel[] = [];
    usersrights = [];
    UserOrgs = [];
    FltrFld: FormControl;
    FltrValue: FormControl;
    CanAdd: boolean = false;
    CanEdit: boolean = false;
    CanDelete: boolean = false;
    Roles = [];

    Orgs = [];
    EnText: string = "User";
    error: String;
    EditFlag: boolean = false;
    query = {
        order: 'UserID',
        limit: 5,
        page: 1
    };
    @ViewChild(MatPaginator) paginator: MatPaginator;
    @ViewChild(MatSort) sort: MatSort;
    working: boolean;
    isLoadingResults: boolean;
    resultsLength: any;
    isRateLimitReached: boolean;
    constructor(private fb: FormBuilder, private http: CommonSrvService) {
        this.usersform = this.fb.group({
            UserID: 0,
            UserName: ["", Validators.required],
            UserPassword: null,
            CPassword: null,
            Fname: ["", Validators.required],
            lname: ["", Validators.required],
            Email: ["", [Validators.required, Validators.email]],
            UserImage: "",
            UserLevel: ["", Validators.required],
            RoleID: ["", Validators.required],

            InActive: ""
        }, { updateOn: "blur" });
        this.UserPassword.valueChanges.subscribe(m => {
            this.CPassword.setValidators(confirmPasswordValidator(m));
            this.CPassword.updateValueAndValidity();
        });
        this.CPassword.valueChanges.subscribe(m => {
            this.CPassword.setValidators(confirmPasswordValidator(this.UserPassword.value));
        });

        this.FltrFld = new FormControl("");
        this.FltrValue = new FormControl("");
    }
    ngAfterViewInit() {
        this.sort.sortChange.subscribe(() => this.paginator.pageIndex = 0);

        this.paginator.pageSize = 10;
        merge(this.sort.sortChange, this.paginator.page, this.FltrValue.valueChanges).pipe(
            startWith({}),
            switchMap(() => {
                return this.getPagedData(this.sort.active, this.sort.direction, this.paginator.pageIndex + 1, this.paginator.pageSize, this.FltrFld.value, this.FltrValue.value);
            })).subscribe(data => {
                this.users = data[0];
                this.resultsLength = data[1].TotalCount;
            });
    }
    ChkUserName() {
        if (this.UserName.value) {
            this.http.postJSON('api/Users/CheckUserName', { UserID: this.UserID.value, UserName: this.UserName.value }).subscribe((d: any) => {
                //this.users = d;
                if (d)
                    this.UserName.setErrors(null);
                else {
                    this.UserName.setErrors({ 'duplicate': true });
                    this.http.ShowWarning(this.UserName.value + ' already exists.')
                }
            }, error => this.error = error // error path
            );
        }
    };
    GetData() {
        this.http.getJSON('api/Users/GetUsers').subscribe((d: any) => {
            //  this.users = d["Users"];
            this.Roles = d["Roles"];
            this.Orgs = d["Orgs"];

            //this.users.paginator = this.paginator;
            //this.users.sort = this.sort;
        }, error => this.http.ShowError(JSON.stringify(error)) // error path
        );
    }
    ngOnInit() {
        this.http.setTitle("Manage Users");
        this.title = "Add New ";
        this.savebtn = "Save ";
        this.GetData();
    }

    GetRoleRights() {
        if (!this.EditFlag && this.RoleID.value != null) {
            this.http.getJSON('api/Roles/GetRoleRights/' + this.RoleID.value).subscribe((REs: any) => {
                this.usersrights = REs;
            }, error => this.error = error // error path
            )
            this.title = "Update ";
            this.savebtn = "Save ";
        }
    }
    getPagedData(sort: string, order: string, page: number, pageSize: number, SearchColumn: string, SearchValue: string) {
        return this.http.postJSON('api/Users/RD_UsersPaged', {
            PageNo: page
            , PageSize: pageSize
            , SortColumn: sort
            , SortOrder: order
            , SearchColumn: SearchColumn
            , SearchValue: SearchValue
        });
    }
    Submit(nform: FormGroupDirective) {
        if (!this.usersform.valid)
            return;
        this.usersform.value['usersRights'] = this.usersrights;
        this.usersform.value['UserOrgs'] = this.UserOrgs.map(function (val) {
            return { "OID": val };
        });
        this.working = true;
        this.http.postJSON('api/Users/Save', this.usersform.value)
            .subscribe((d: any) => {
                this.users = d[0];
                this.resultsLength = d[1].TotalCount;
                this.http.openSnackBar(this.UserID.value > 0 ? environment.UpdateMSG.replace("${Name}", this.EnText) : environment.SaveMSG.replace("${Name}", this.EnText));
                this.reset(nform);
                this.title = "Add New ";
                this.savebtn = "Save ";
                this.working = false;
            },
            (error) => {
                this.error = error.error;
                this.http.ShowError(error.error);
                
              });
    }
    reset(nform: FormGroupDirective) {
        this.usersform.reset();
        nform.resetForm();
        this.usersrights = [];
        this.UserID.setValue(0);
        this.title = "Add New ";
        this.savebtn = "Save ";
    }
    toggleEdit(row) {
        this.EditFlag = true;
        this.RoleID.setValue(row.RoleID);
        this.UserID.setValue(row.UserID);
        this.UserName.setValue(row.UserName);
        this.UserPassword.setValue("");
        this.CPassword.setValue("");
        this.Fname.setValue(row.Fname);
        this.lname.setValue(row.lname);
        this.Email.setValue(row.Email);
        this.UserImage.setValue(row.UserImage);
        this.UserLevel.setValue(row.UserLevel);

        this.http.getJSON('api/Users/GetUserRights/' + row.UserID).subscribe((REs: any) => {
            this.usersrights = REs["Rights"];
            this.UserOrgs = REs["UserOrgs"].map(function (val) {
                return val.OID;
            });
            this.EditFlag = false;
        }, error => this.error = error
        );
        this.InActive.setValue(row.InActive);

        this.title = "Update ";
        this.savebtn = "Save ";
    }
    toggleActive(row) {
        this.http.confirm().subscribe(result => {
            if (result) {
                this.http.postJSON('api/Users/ActiveInActive', { 'UserID': row.UserID, 'InActive': row.InActive })
                    .subscribe((d: any) => {
                        this.http.openSnackBar(row.InActive == false ? environment.InActiveMSG.replace("${Name}", this.EnText) : environment.ActiveMSG.replace("${Name}", this.EnText));
                        // this.users =new MatTableDataSource(d);
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
        //  this.users.filter = filterValue;
    }

    get UserID() { return this.usersform.get("UserID"); }
    get UserName() { return this.usersform.get("UserName"); }
    get UserPassword() { return this.usersform.get("UserPassword"); }
    get Fname() { return this.usersform.get("Fname"); }
    get lname() { return this.usersform.get("lname"); }
    get CPassword() { return this.usersform.get("CPassword"); }
    get RoleID() { return this.usersform.get("RoleID"); }
    get UserImage() { return this.usersform.get("UserImage"); }
    get UserLevel() { return this.usersform.get("UserLevel"); }
    get Email() { return this.usersform.get("Email"); }

    get InActive() { return this.usersform.get("InActive"); }

    SelAdd(cur: any, All: UserRightsModel[], Prop: string) {
        All.forEach((m: UserRightsModel) => {
            m[Prop] = cur;
        });
    };
}
