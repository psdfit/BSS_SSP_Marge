import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from '../../../environments/environment';
import { FormGroup, FormBuilder, Validators, FormGroupDirective, FormControl } from '@angular/forms';
import { CommonSrvService } from '../../common-srv.service';
import { ModelBase } from '../../shared/ModelBase';
import { UserRightsModel } from '../../master-data/users/users.component';
import { DialogueService } from '../../shared/dialogue.service';


@Component({
    selector: 'app-KAM',
    templateUrl: './KAM.component.html',
    styleUrls: ['./KAM.component.scss']
})
export class KAMComponent implements OnInit {
    kamassignmentform: FormGroup;
    title: string; savebtn: string;
    filters: IQueryFilters = { RegionID: 0, DistrictID: 0, UserID: 0, TSPID: 0 };


    displayedColumns = ['Action', 'TSPID','RegionName', 'DistrictName', 'KAM'];
    displayedtspColumns = ['TspID','RegionName', 'DistrictName', 'IsSelected'];

    kamassignment: MatTableDataSource<any>;
    tspkamhistory: MatTableDataSource<any>;
    TSPsArray= [];
    Users = []; TSPs = []; kamassignmentArray = []; DistrictArray = []; RegionArray = []; TSPsCl = []; FilteredDistricts = [];
    formrights: UserRightsModel;
    EnText: string = "KAM Assignment";
    error: String;
    query = {

        order: 'KamID',
        limit: 1,
        page: 1
    };
    @ViewChild(MatPaginator) paginator: MatPaginator;
    @ViewChild(MatSort) sort: MatSort;
    working: boolean;
    regionNotSelected: boolean = true;
    showKamSavedMsg: boolean = false;
    constructor(private fb: FormBuilder, private ComSrv: CommonSrvService, public dialogueService: DialogueService) {
        this.kamassignmentform = this.fb.group({
            KamID: 0,
            UserID: ["", Validators.required],
            TspID: "",
            ClassID: "",
            InActive: ""
        }, { updateOn: "blur" });
        this.kamassignment = new MatTableDataSource([]);
        this.formrights = ComSrv.getFormRights();
        //this.UserID.valueChanges.subscribe((v) => {
        //    this.TSPs = this.TSPs.map((m) => {
        //        if (m.AssignedUser == v) {
        //            m.IsSelected = true;
        //        }
        //        else
        //            m.IsSelected = false;
        //        return m;


        //    });
        //});
    }
    NotSelected(item) {
        return item.AssignedUser == 0
    }
    GetData() {
        this.ComSrv.getJSON('api/KAMAssignment/GetKAMAssignment').subscribe((d: any) => {

            this.Users = d[0];
            this.TSPs = d[1];
            this.TSPsArray = d[1];
            this.kamassignment = new MatTableDataSource(d[2]);
            this.kamassignmentArray = d[2];
            this.RegionArray =d[3];
            this.DistrictArray = d[4];
            //this.Users = this.Users.filter(user => (user.UserLevel == 1 || user.UserLevel == 2));    // previous this check was functional
            this.Users = this.Users.filter(user => (user.RoleID == 24));

            this.RegionArray = this.RegionArray.filter(x => this.TSPs.map(y => y.RegionID).includes(x.RegionID));
            //this.DistrictArray = this.DistrictArray.filter(x => this.TSPs.map(y => y.DistrictID).includes(x.DistrictID))
            if (this.showKamSavedMsg) {
                this.ComSrv.openSnackBar(this.KamID.value > 0 ? environment.UpdateMSG.replace("${Name}", this.EnText) : environment.SaveMSG.replace("${Name}", this.EnText));
                this.showKamSavedMsg = false;
            }

            this.kamassignment.paginator = this.paginator;
            this.kamassignment.sort = this.sort;
        }, error => this.error = error // error path
        );
    }
    ngOnInit() {
        this.ComSrv.setTitle("KAM Assignment");
        this.title = "Add New ";
        this.savebtn = "Save ";
        this.GetData();
    }
  SearchUsr = new FormControl('');
  SearchDistrict = new FormControl('');
  SearchRegion = new FormControl('');
  SearchTSP = new FormControl('');
 
  EmptyCtrl(Ev: any) {
    
    this.SearchUsr.setValue('');
  }
    Submit(myform: FormGroupDirective) {
        if (!this.kamassignmentform.valid)
            return;
      var SelTsps = this.TSPs.filter((m) => {
        return m != undefined && m.IsSelected == true;
      });
        SelTsps.map((m) => {
            if (m.IsSelected == true) {
                m.UserID = this.UserID.value;
                //m.T.disable({ onlySelf: true })
                return m;
            }

        });
      if (SelTsps.length == 0) {
            this.error = "Please Select TSP";
            this.ComSrv.ShowError(this.error.toString(), "Error");
            return;
        }
        this.working = true;
        this.ComSrv.postJSON('api/KAMAssignment/Save', SelTsps)
            .subscribe((d: any) => {
                this.TSPs = d;
                this.reset();
                this.showKamSavedMsg = true;
              this.GetData();

                this.title = "Add New ";
                this.savebtn = "Save ";
                this.working = false;
               // this.ComSrv.openSnackBar(this.KamID.value > 0 ? environment.UpdateMSG.replace("${Name}", this.EnText) : environment.SaveMSG.replace("${Name}", this.EnText));
            },
            (error) => {
                this.error = error.error;
                this.ComSrv.ShowError(error.error);
                
              });

    }

    getTSPKAMHistory(tspid:number) {
        this.ComSrv.getJSON('api/KAMAssignment/GetTSPKAMHistory/' +tspid).subscribe((d: any) => {

            this.tspkamhistory = new MatTableDataSource(d[0]);

            this.tspkamhistory.paginator = this.paginator;
            this.tspkamhistory.sort = this.sort;
        }, error => this.error = error // error path
        );
    }
    
    openHistoryDialogue(data: any): void {
            if(this.formrights.CanView==true){
                this.dialogueService.openTSPKAMHistoryDialogue(data.TspID);
            }

       
    }

    FilterDistrictsByRegion(ev: any) {
        this.regionNotSelected = false;
        this.FilteredDistricts = this.DistrictArray.filter(d => d.RegionID == ev);
        this.TSPs = this.TSPsArray.filter(x => x.RegionID == this.filters.RegionID);
        this.SearchDistrict.setValue('');
    }
    FilterTSPsByDistrict(ev: any) {
        this.TSPs = this.TSPsArray.filter(x => x.RegionID == this.filters.RegionID && x.DistrictID == this.filters.DistrictID);
    }
    FilterKAMAssignmentsByTSPOrKAM() {
        if (this.filters.TSPID == 0) {
            this.kamassignment = new MatTableDataSource(this.kamassignmentArray.filter(x => x.UserID == this.filters.UserID));
        }
        if (this.filters.UserID == 0) {
            this.kamassignment = new MatTableDataSource(this.kamassignmentArray.filter(y => y.TspID == this.filters.TSPID));

        }
        if (this.filters.UserID && this.filters.TSPID) {
            this.kamassignment = new MatTableDataSource(this.kamassignmentArray.filter(x => x.TspID == this.filters.TSPID && x.UserID == this.filters.UserID));
        }
        this.kamassignment.paginator = this.paginator;
        this.kamassignment.sort = this.sort;
    }

    ResetFilters() {
        this.filters.TSPID = 0;
        this.filters.UserID = 0;
        this.kamassignment = new MatTableDataSource(this.kamassignmentArray);
        this.kamassignment.paginator = this.paginator;
        this.kamassignment.sort = this.sort;
    }

    toggleEdit(row) {
        if(this.formrights.CanEdit==true){
            this.ComSrv.confirmKAM().subscribe(result => {
                if (result) {
                    this.ComSrv.postJSON('api/KAMAssignment/Update', row)
                        .subscribe((d: any) => {
                            //this.kamassignment = d;
                          this.FilterKAMAssignmentsByTSPOrKAM();
                            this.ComSrv.openSnackBar(environment.UpdateMSG.replace("${Name}", this.EnText), this.EnText);
    
    
                        },
                        (error) => {
                            this.error = error.error;
                            this.ComSrv.ShowError(error.error);
                          });
                }
                else {
                    //row.NTP = !row.NTP;
                }
            });
    
        }
      
        //this.UserID.setValue(row.UserID);
        ////this.TSPs.setValue(row.GenderName);

        //this.TSPs = this.TSPs.map((m) => {
        //    if (m.AssignedUser == row.UserID)
        //        m.IsSelected = true;
        //    else
        //        m.IsSelected = false;
        //    return m;


        //});

        this.title = "Update ";
        this.savebtn = "Save ";
    }

    applyFilter(filterValue: string) {
        filterValue = filterValue.trim(); // Remove whitespace
        filterValue = filterValue.toLowerCase(); // Datasource defaults to lowercase matches
        this.kamassignment.filter = filterValue;
    }
    //applyFilterUnAssignedTSPs(filterValue: string) {
    //    filterValue = filterValue.trim(); // Remove whitespace
    //    filterValue = filterValue.toLowerCase(); // Datasource defaults to lowercase matches
    //    this.TSPs.filter(s => s.DistrictName === filterValue);
    //    //this.TSPsArray = this.TSPsArray.filteredData.values["DistrictName"].filter = filterValue;
    //    //this.TSPs = this.TSPs.values["DistrictName"].filter = filterValue;
    //    //this.TSPsArray.filter(x => this.TSPsArray.filteredData.map(y => y.DistrictName == filterValue).includes(filterValue));

    //}

    reset() {
        this.kamassignmentform.reset();
        //myform.resetFrom();
        this.KamID.setValue(0);
        this.title = "Add New ";
        this.savebtn = "Save ";
        this.filters.RegionID = 0;
        this.filters.DistrictID = 0;
        this.regionNotSelected = true;
        this.TSPs = this.TSPsArray;
    }



    get KamID() { return this.kamassignmentform.get("KamID"); }
    get UserID() { return this.kamassignmentform.get("UserID"); }
    get TspID() { return this.kamassignmentform.get("TspID"); }
    get ClassID() { return this.kamassignmentform.get("ClassID"); }
    get DistrictName() { return this.kamassignmentform.get("DistrictName"); }
    get InActive() { return this.kamassignmentform.get("InActive"); }
}
export class KAMAssignmentModel extends ModelBase {
    KamID: number;
    UserID: number;
    TspID: number;
    ClassID: number;
    DistrictName: string
}

export class KAMAssignedTSPsModel extends ModelBase {
    IsSelected: boolean;

}

export interface IQueryFilters {
    RegionID: number;
    DistrictID: number;
    UserID: number;
    TSPID: number;
    
}



