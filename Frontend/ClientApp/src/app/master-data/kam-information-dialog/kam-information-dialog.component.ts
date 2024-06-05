import { Component, OnInit, ViewChild, Inject } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from '../../../environments/environment';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { CommonSrvService } from '../../common-srv.service';
import { ModelBase } from '../../shared/ModelBase';
import { UserRightsModel } from '../../master-data/users/users.component';
import { Observable } from 'rxjs';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';


@Component({
  selector: 'app-kam-information-dialog',
  templateUrl: './kam-information-dialog.component.html',
  styleUrls: ['./kam-information-dialog.component.scss']
})
export class KAMInformationDialogComponent implements OnInit {
    genderform: FormGroup;
    title: string; savebtn: string;
    displayedColumns = ['KAMName', 'KAMEmail'];
    KAMInfo: MatTableDataSource<any>;
    formrights: UserRightsModel;
    EnText: string = "KAMInfo";
    error: String;
    query = {
        order: 'GenderID',
        limit: 5,
        page: 1
    };
    @ViewChild(MatPaginator) paginator: MatPaginator;
    @ViewChild(MatSort) sort: MatSort;
    working: boolean;
  constructor(private fb: FormBuilder, private http: CommonSrvService,
  @Inject(MAT_DIALOG_DATA) public data: any) {
        
    }


  GetData() {
    this.KAMInfo = this.data;
    }
    ngOnInit() {
        this.http.setTitle("KAM Information");
        this.title = "Add New ";
        this.savebtn = "Save ";
        this.GetData();
    }

   
  
}

