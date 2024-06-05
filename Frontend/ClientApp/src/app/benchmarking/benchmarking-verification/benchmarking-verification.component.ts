import { Component, OnInit, ViewChild, ElementRef, Inject } from '@angular/core';
import { CommonSrvService } from '../../common-srv.service';
import { MatDialogRef, MatDialog, MAT_DIALOG_DATA } from '@angular/material/dialog';

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
import { IBenchmarking } from '../Interface/IBenchmarking';
import { BenchmarkingComponent } from '../benchmarking/benchmarking.component';



@Component({
    selector: 'hrapp-benchmarking-verification',
    templateUrl: './benchmarking-verification.component.html',
    styleUrls: ['./benchmarking-verification.component.scss']
})

export class BenchmarkingVerificationComponent implements OnInit {

    benchmarkingverificationform: FormGroup;

    displayedColumns = ['OfferedAmount'];

    benchmarking: MatTableDataSource<any>;
    trade: MatTableDataSource<any>;
    class: MatTableDataSource<any>;
    formrights: UserRightsModel;
    EnText: string = "Benchmarking";
    error: String;
    //query = {
    //    order: 'BenchmarkingID',
    //    limit: 5,
    //    page: 1
    //};

    @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
    @ViewChild(MatSort, { static: false }) sort: MatSort;
    working: boolean;
    constructor(private fb: FormBuilder, private ComSrv: CommonSrvService,
        public dialog: MatDialog,
        public dialogRef: MatDialogRef<BenchmarkingVerificationComponent>,
        @Inject(MAT_DIALOG_DATA) public data: IBenchmarking
        //@Inject(MAT_DIALOG_DATA) public data: IBenchmarking[] // For multiple records

    ) {
        this.benchmarkingverificationform = this.fb.group({
            BenchmarkingID: 0,
            TradeName: this.data.TradeName,
            ProposedAmount: this.data.ProposedAmount,
            ProposedAmount50: this.data.ProposedAmount50,
            CalculatedAmount: this.data.CalculatedAmount,
            CalculatedAmount70: this.data.CalculatedAmount70,
            ClassFrom: this.data.ClassFrom,
            ClassTo: this.data.ClassTo,
            //OfferedAmount: this.data[0].OfferedAmount, // For multiple records to save
            OfferedAmount: this.data.OfferedAmount,   
            InActive: ""
        }, { updateOn: "blur" });
        //this.benchmarking = new MatTableDataSource([]);
        //this.formrights = ComSrv.getFormRights();
        //this.OfferedAmount.setValue(this.data.OfferedAmount);
        //this.OfferedAmount.setValue(C);
        //console.log('data')
    }



    ngOnInit() {
        //this.OfferedAmount =  th
    }

    CloseDialogueBox() {

        this.dialogRef.close();
    }

    Submit(myform: FormGroupDirective) {
        if (!this.benchmarkingverificationform.valid)
            return;
        this.working = true;
        this.ComSrv.postJSON('api/Benchmarking/Save', this.benchmarkingverificationform.value)
            .subscribe((d: any) => {
                this.benchmarking = new MatTableDataSource(d);
                this.benchmarking.paginator = this.paginator;
                this.benchmarking.sort = this.sort;
                this.ComSrv.openSnackBar(this.BenchmarkingID.value > 0 ? environment.UpdateMSG.replace("${Name}", this.EnText) : environment.SaveMSG.replace("${Name}", this.EnText));
                this.reset(myform);
                //this.title = "Add New ";
                //this.savebtn = "Save ";
                this.working = false;
            },
            (error) => {
                this.error = error.error;
                this.ComSrv.ShowError(error.error);
                
              });
    }

    reset(myform: FormGroupDirective) {
            this.benchmarkingverificationform.reset();
            //myform.resetFrom();
            this.BenchmarkingID.setValue(0);
            //this.title = "Add New ";
            //this.savebtn = "Save ";
        }


        get BenchmarkingID() { return this.benchmarkingverificationform.get("BenchmarkingID"); }
        get TradeName() { return this.benchmarkingverificationform.get("TradeName"); }
        get OfferedAmount() { return this.benchmarkingverificationform.get("OfferedAmount"); }
        get ProposedAmount() { return this.benchmarkingverificationform.get("ProposedAmount"); }
        get CalculatedAmount() { return this.benchmarkingverificationform.get("CalculatedAmount"); }
        get CalculatedAmount70() { return this.benchmarkingverificationform.get("CalculatedAmount70"); }
        get ProposedAmount50() { return this.benchmarkingverificationform.get("ProposedAmount50"); }
        get ClassFrom() { return this.benchmarkingverificationform.get("ClassFrom"); }
        get ClassTo() { return this.benchmarkingverificationform.get("ClassTo"); }
        get InActive() { return this.benchmarkingverificationform.get("InActive"); }

    }
