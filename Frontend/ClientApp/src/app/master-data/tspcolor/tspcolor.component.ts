import { Component, OnInit, ViewChild } from '@angular/core';
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

@Component({
    selector: 'app-tspcolor',
    templateUrl: './tspcolor.component.html',
    styleUrls: ['./tspcolor.component.scss']
})
export class tspcolorComponent implements OnInit {
    TSPColorform: FormGroup;
    title: string; savebtn: string;
    displayedColumns = ['ChangeColor','History','TSPName', 'NTN', 'TSPCode', 'Address', 'TSPColorName' ];
    TSPColor: MatTableDataSource<any>;
    formrights: UserRightsModel;
    EnText: string = "TSPColor";
    error: String;
    @ViewChild(MatPaginator) paginator: MatPaginator;
    @ViewChild(MatSort) sort: MatSort;
    working: boolean;
    constructor(private fb: FormBuilder, private http: CommonSrvService,public dialogueService: DialogueService) {
        this.TSPColor = new MatTableDataSource([]);
        this.formrights = http.getFormRights();
    }
   
    ngOnInit() {
        this.http.setTitle("TSP Color");
        this.title = "Add New ";
        this.savebtn = "Save ";
        this.FetchTSPColorDataForGridView();
    }
   
    FetchTSPColorDataForGridView() {
        debugger;
        this.http.getJSON('api/TSPColor/GetTSPColorMasterData').subscribe((d: any) => {
          debugger;
          this.populateTSPColorList(d);
        }, error => this.error = error // error path
        );
      }

    populateTSPColorList(d: any) {
        this.TSPColor = new MatTableDataSource(d);
        this.TSPColor.paginator = this.paginator;
        this.TSPColor.sort = this.sort;
    }
    openHistoryDialogue(data: any): void {
        debugger;
        this.dialogueService.openTSPColorChangeHistoryDialogue(data);
      }
    openTSPColorChangeDialogue(data: any): void {
        debugger;
        this.dialogueService.openTSPColorChangeDialogue(data);
      }
    applyFilter(filterValue: string) {
        filterValue = filterValue.trim(); // Remove whitespace
        filterValue = filterValue.toLowerCase(); // Datasource defaults to lowercase matches
        this.TSPColor.filter = filterValue;
    }

    get TSPMasterID() { return this.TSPColorform.get("TSPMasterID"); }
    get TSPName() { return this.TSPColorform.get("TSPName"); }
    get Address() { return this.TSPColorform.get("Address"); }
    get TSPCode() { return this.TSPColorform.get("TSPCode"); }
    get TSPColorID() { return this.TSPColorform.get("TSPColorID"); }
    get TSPColorName() { return this.TSPColorform.get("TSPColorName"); }
    get TSPColorHistoryID() { return this.TSPColorform.get("TSPColorHistoryID"); }
    get InActive() { return this.TSPColorform.get("InActive"); }
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
