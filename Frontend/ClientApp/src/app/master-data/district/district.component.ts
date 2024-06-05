import { Component, OnInit, ViewChild, Input } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from '../../../environments/environment';
import { FormGroup, FormBuilder, Validators,  FormGroupDirective } from '@angular/forms';
import { CommonSrvService } from '../../common-srv.service';
import { ModelBase } from '../../shared/ModelBase';
import { UserRightsModel } from '../../master-data/users/users.component';

@Component({
  selector: 'app-district',
  templateUrl: './district.component.html',
  styleUrls: ['./district.component.scss']
})
export class DistrictComponent implements OnInit {
  districtform: FormGroup;
  title: string; savebtn: string;
    displayedColumns = ['DistrictName', 'DistrictNameUrdu', 'ClusterID','RegionID','InActive', "Action"];
  district: MatTableDataSource<any>;
  Cluster: any; Region: any;
  formrights: UserRightsModel;
  EnText: string = "District";
  error: String;
  query = {
    order: 'DistrictID',
    limit: 5,
    page: 1
  };
  @Input() urdu:Input
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  working: boolean;
   
  constructor(private fb: FormBuilder, private http: CommonSrvService) {
    this.districtform = this.fb.group({
      DistrictID: 0,
      DistrictName: ["", Validators.required],
      DistrictNameUrdu: ["", Validators.required],
      ClusterID: ["", Validators.required],
      InActive: "",
      RegionID: ["", Validators.required]
    }, { updateOn: "blur" });
    this.district = new MatTableDataSource([]);
    this.formrights = http.getFormRights();
    //this.DistrictID.valueChanges.subscribe((val) => {
    //  if (val == null)
    //    this.DistrictID.setValue(0);
    //});
  }

    ChkDistrictName() {
        if (this.DistrictName.value) {
            this.http.postJSON('api/District/CheckDistrictName', { DistrictID: this.DistrictID.value, DistrictName: this.DistrictName.value }).subscribe((d: any) => {
                //this.users = d;
                if (d)
                    this.DistrictName.setErrors(null);
                else
                    this.DistrictName.setErrors({ 'duplicate': true });
            }, error => this.error = error // error path
            );
        }
    };

  GetData() {
    this.http.getJSON('api/District/GetDistrict').subscribe((d: any) => {
      this.district = new MatTableDataSource(d[0])
      this.Cluster = d[1];
      this.Region = d[2];

      this.district.paginator = this.paginator;
      this.district.sort = this.sort;
    }, error => this.error = error // error path
    );
  }
  ngOnInit() {
    this.http.setTitle("District");
    this.title = "Add New ";
    this.savebtn = "Save ";
    this.GetData();
  }

  Submit(nform: FormGroupDirective) {
    if (!this.districtform.valid)
      return;
    this.working = true;
   // if (this.DistrictID.value == null) this.DistrictID.setValue(0);
    this.http.postJSON('api/District/Save', this.districtform.value)
      .subscribe((d: any) => {
        this.district = new MatTableDataSource(d);
        this.district.paginator = this.paginator;
        this.district.sort = this.sort;
        this.http.openSnackBar(this.DistrictID.value > 0 ? environment.UpdateMSG.replace("${Name}", this.EnText) : environment.SaveMSG.replace("${Name}", this.EnText));
          this.reset(nform);
          this.DistrictID.setValue(0);
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
    
    this.districtform.reset();
   // nform.resetForm();
    this.title = "Add New ";
    this.savebtn = "Save ";
  }
  toggleEdit(row) {
    this.DistrictID.setValue(row.DistrictID);
    this.DistrictName.setValue(row.DistrictName);
    this.DistrictNameUrdu.setValue(row.DistrictNameUrdu);
    this.ClusterID.setValue(row.ClusterID);
    this.InActive.setValue(row.InActive);
    this.RegionID.setValue(row.RegionID);

    this.title = "Update ";
    this.savebtn = "Save ";
  }
  toggleActive(row) {
    this.http.confirm().subscribe(result => {
      if (result) {
        this.http.postJSON('api/District/ActiveInActive', { 'DistrictID': row.DistrictID, 'InActive': row.InActive })
          .subscribe((d: any) => {
            this.http.openSnackBar(row.InActive == true ? environment.InActiveMSG.replace("${Name}", this.EnText) : environment.ActiveMSG.replace("${Name}", this.EnText));
            // this.district =new MatTableDataSource(d);
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
    this.district.filter = filterValue;
  }

  

  get DistrictID() { return this.districtform.get("DistrictID"); }
  get DistrictName() { return this.districtform.get("DistrictName"); }
  get DistrictNameUrdu() { return this.districtform.get("DistrictNameUrdu"); }
  get ClusterID() { return this.districtform.get("ClusterID"); }
  get InActive() { return this.districtform.get("InActive"); }
  get RegionID() { return this.districtform.get("RegionID"); }
}
export class DistrictModel extends ModelBase {
  DistrictID: number;
  DistrictName: string;
  DistrictNameUrdu: string;
  ClusterID: number;
  RegionID: number;
}
