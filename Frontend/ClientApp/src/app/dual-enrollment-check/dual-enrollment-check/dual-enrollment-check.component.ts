import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { CommonSrvService } from '../../common-srv.service';

import { Element } from '@angular/compiler/src/render3/r3_ast';
import { MatProgressBar } from '@angular/material/progress-bar';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { FormGroup, FormBuilder, Validators, FormGroupDirective } from '@angular/forms';
import { environment } from '../../../environments/environment';
import { ModelBase } from '../../shared/ModelBase';
import { UserRightsModel } from '../../master-data/users/users.component';
import { ActivatedRoute } from '@angular/router';
import * as moment from 'moment';
import { IOrgConfig } from '../../registration/Interface/IOrgConfig';
import * as XLSX from 'xlsx';
import { ITraineeProfile } from '../../registration/Interface/ITraineeProfile';
import * as fileSaver from 'file-saver';
import { DatePipe } from '@angular/common';
import { ExportType, EnumExcelReportType } from '../../shared/Enumerations';
import { Workbook } from 'exceljs';
import * as fs from 'file-saver';
import { ExportExcel } from '../../shared/Interfaces';

@Component({
    selector: 'hrapp-dual-enrollment-check',
    templateUrl: './dual-enrollment-check.component.html',
    styleUrls: ['./dual-enrollment-check.component.scss'],
    providers: [DatePipe]
})

export class DualEnrollmentCheckComponent implements OnInit {
    dualenrollmentcheckform: FormGroup;
    title: string; savebtn: string;
    filters: IQueryFilters = { SchemeID: 0, TSPID: 0, ClassID: 0, TraineeID: 0, TradeID: 0, DistrictID: 0 };

   

    //displayedColumnsDropOutTrainees = ['ClassCode', 'Batch', 'TradeName', 'TradeID',
    //    'TraineeName', 'FatherName', 'TraineeCNIC', 'DateOfBirth', 'Education',
    //    'TraineeID', 'PBTEID', 'CollegeID', 'StatusName'
    //];
    displayedColumns = ['TraineeCNIC', 'CNICStatus'];

    Trainees: MatTableDataSource<any>;

    selectedClasses: any;
    pbteClassesArray: any;
    pbteTSPsArray: any;

    CNICsStatusList:  MatTableDataSource<any>;


    data: any;

    traineeResultStatusTypes: any;

    update: String;

    isOpenRegistration: boolean = false;
    isOpenRegistrationMessage: string = "";
    formrights: UserRightsModel;
    EnText: string = "Dual Enrollment Check";
    error: String;
    success: boolean;
    errors: boolean;
    query = {
        order: 'TraineeID',
        limit: 5,
        page: 1
    };
    @ViewChild(MatPaginator) paginator: MatPaginator;
    @ViewChild(MatSort) sort: MatSort;

    working: boolean;
    constructor(private fb: FormBuilder, private ComSrv: CommonSrvService, private route: ActivatedRoute
        , private _date: DatePipe, ) {
        this.dualenrollmentcheckform = this.fb.group({
            TraineeCNIC: "",
            InActive: ""
        }, { updateOn: "blur" });
        this.Trainees = new MatTableDataSource([]);
        this.formrights = ComSrv.getFormRights("/pbte");
    }




    ChkTraineeCNIC() {
        if (this.TraineeCNIC.value) {
            let CNIC = [{ 'TraineeCNIC':this.TraineeCNIC.value}];
            this.ComSrv.postJSON('api/TraineeProfile/GetTraineesFromFile', CNIC).subscribe((d: any) => {
                //this.users = d;
                if (d[0].CNICStatus == "Already Registered") {
                    this.errors = true;
                    this.success = false;
                }
                //this.TraineeCNIC.setErrors({ 'duplicate': true });
                //this.TraineeCNIC.setErrors({ exists: true });
                else {
                    this.success = true;
                    this.errors = false;
                }
                    //this.TraineeCNIC.setErrors({ 'unique': true });
            }, error => this.error = error // error path
            );
        }
    };


    onTraineeFileChange(ev: any) {
        let workBook = null;
        let jsonData = null;
        const reader = new FileReader();
        const file = ev.target.files[0];
        reader.onload = (event) => {
            const data = reader.result;
            workBook = XLSX.read(data, { type: 'binary' });
            jsonData = workBook.SheetNames.reduce((initial, name) => {
                const sheet = workBook.Sheets[name];
                initial[name] = XLSX.utils.sheet_to_json(sheet);
                return initial;
            }, {});

            const dataString = JSON.stringify(jsonData);
            this.data = JSON.parse(dataString);
            //console.log(this.pbteTrainees.filteredData);
            console.log(this.data.CNIC_List);

            if (!this.data.CNIC_List) {
                this.ComSrv.ShowError("Sheet with the name 'CNIC_List' not found in Excel file");
                return false;
            }


            this.ComSrv.postJSON('api/TraineeProfile/GetTraineesFromFile', this.data.CNIC_List)
                .subscribe((d: any) => {
                    this.CNICsStatusList = new MatTableDataSource(d);
                    //this.update = "PBTE imported for Trainees";
                    //this.ComSrv.openSnackBar(this.update.toString(), "Updated");
                    this.CNICsStatusList.paginator = this.paginator;
                    this.CNICsStatusList.sort = this.sort;
                },
                    error => this.error = error // error path
                );
        }
        reader.readAsBinaryString(file);
        ev.target.value = '';
    }


    ngOnInit() {
        this.ComSrv.setTitle("CNIC Dual Enrollment");
        this.title = "Add New ";
        this.savebtn = "Save ";
        //this.GetData();
    }


    applyFilter(filterValue: string) {
        filterValue = filterValue.trim(); // Remove whitespace
        filterValue = filterValue.toLowerCase(); // Datasource defaults to lowercase matches
        this.Trainees.filter = filterValue;
    }
    reset() {
        this.dualenrollmentcheckform.reset();
        this.success = false;
        this.errors = false;
    }



    SortANDPaginate() {

     
       
       
    }


    get TraineeCNIC() { return this.dualenrollmentcheckform.get("TraineeCNIC"); }
    get InActive() { return this.dualenrollmentcheckform.get("InActive"); }

}

export interface IQueryFilters {
    SchemeID: number;
    TSPID: number;
    ClassID: number;
    TraineeID: number;
    TradeID: number;
    DistrictID: number;
}
