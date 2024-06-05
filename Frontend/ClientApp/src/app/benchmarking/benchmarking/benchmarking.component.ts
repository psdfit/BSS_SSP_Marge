import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, FormGroupDirective } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { CommonSrvService } from '../../common-srv.service';
import { UserRightsModel } from '../../master-data/users/users.component';
import { ModelBase } from '../../shared/ModelBase';
import { BenchmarkingVerificationComponent } from '../benchmarking-verification/benchmarking-verification.component';
import { IBenchmarking } from '../Interface/IBenchmarking';
import { environment } from '../../../environments/environment';
import { animate, trigger, state, transition, style } from '@angular/animations';
import { FormControl } from '@angular/forms';



@Component({
  selector: 'hrapp-benchmarking',
  templateUrl: './benchmarking.component.html',
  styleUrls: ['./benchmarking.component.scss'],
  animations: [
    trigger('detailExpand', [
      state('collapsed', style({ height: '0px', minHeight: '0' })),
      state('expanded', style({ height: '*' })),
      transition('expanded <=> collapsed', animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)')),
    ]),
  ],
})

export class BenchmarkingComponent implements OnInit {

  benchmarkingform: FormGroup;

  displayedColumns = ['Action', 'TradeName', 'Organization', 'RecentSchemes',
    //'ProposedAmount',
    //'TotalClasses',
    'CostSharing',
    'CalculatedAmount',
    //'CalculatedAmount70', 'ProposedAmount50',
    'OfferedAmount'];
  benchmarkingArray: IBenchmarking[];
  expandedElement: BenchmarkingModel | null;;
  benchmarking: MatTableDataSource<any>;
  trade: MatTableDataSource<any>;
  class: MatTableDataSource<any>;
  formrights: UserRightsModel;
  EnText: string = "Trade Benchmarking";
  error: String;
  Cluster: any; Region: any; District: any; TSPs: any; ProgramType: any; YearWiseInflationRate: any; FilteredMonths: any; InflationMonth: any;
  InflationRate: any; benchmarkingClasses: any; shownestedtable: boolean;

  @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: false }) sort: MatSort;
  working: boolean;
  TSPsSelected: boolean = false;
  constructor(private fb: FormBuilder, private ComSrv: CommonSrvService, public dialog: MatDialog) {
    this.benchmarkingform = this.fb.group({
      //BenchmarkingID: 0,
      TradeName: "",
      ProposedAmount: "",
      ProgramTypeID: "",
      TSPID: 0,
      CostSharing: "",
      //ClassFrom: 0,
      //ClassTo: 0,
      //DistrictID: 0,
      //ClusterID: 0,
      //RegionID: 0,
      //FinancialYear: 0,
      //Month: 0,
      //Inflation: 0,
      //InRecentSchemes: 0,
      InActive: ""
    }, { updateOn: "blur" });
    this.benchmarking = new MatTableDataSource([]);
    this.formrights = ComSrv.getFormRights();
  }

  SearchTSP = new FormControl('');


  ngOnInit() {
    this.ComSrv.setTitle("Benchamarking");
    this.GetData();
  }

  GetData() {
    this.ComSrv.getJSON('api/Benchmarking/GetBenchmarking').subscribe((d: any) => {
      this.District = (d[0])
      this.Cluster = d[1];
      this.Region = d[2];
      this.YearWiseInflationRate = d[3];
      this.TSPs = d[4];
      this.ProgramType = d[5];

    }, error => this.error = error // error path
    );
  }


  openDialog(row: IBenchmarking): void {
    const dialogRef = this.dialog.open(BenchmarkingVerificationComponent, {
      minWidth: '600px',
      minHeight: '400px',
      //data: JSON.parse(JSON.stringify(row))
      data: { ...row }
    })
    dialogRef.afterClosed().subscribe(result => {

    })
  }

  FilterMonths(event: any) {
    //this.ComSrv.getJSON(`api/YearWiseInflationRate/RD_YearWiseInflationRateBy/` + event.value)
    //    .subscribe(data => {
    //        this.InflationRate = <any[]>data;
    //    },FilteredMonths
    //    )
    this.FilteredMonths = this.YearWiseInflationRate.filter(subsec => subsec.FinancialYear === event.value);
    this.InflationMonth = this.FilteredMonths;
    //this.SubSector = this.SubSectorSelected;
    //console.log(this.SubSectorSelected)

  }

  GetInflation(event: any) {
    this.InflationMonth = this.FilteredMonths.filter(subsec => subsec.Month === event.value);
    console.log(this.InflationMonth);
    var Rate = this.InflationMonth[0].Inflation;
    console.log(Rate);
    //this.Inflation.setValue(Rate);
  }

  GetRelevantClasses(row) {

    this.ComSrv.postJSON('api/Benchmarking/BenchmarkingClasses', row)
      .subscribe((d: any) => {
        this.benchmarkingClasses = d;
        if (this.benchmarkingClasses) {
          this.shownestedtable = true
          this.expandedElement = row;
        }
        //this.benchmarkingClasses.paginator = this.paginator;
        //this.benchmarkingClasses.sort = this.sort;
        //this.ComSrv.openSnackBar(this.BenchmarkingID.value > 0 ? environment.UpdateMSG.replace("${Name}", this.EnText) : environment.SaveMSG.replace("${Name}", this.EnText));
        //this.title = "Add New ";
        //this.savebtn = "Save ";
      });
  }


  //EnableCostSharing(event:any) {
  //    if (event.length) {
  //        this.CostSharing.enable();
  //        this.CostSharing.setValue('No');
  //        //this.TSPsSelected = true;
  //    }
  //}

  Submit(myForm: FormGroupDirective) {
    //myFormfilter(x => x.UserIDs = x.UserIDs.join(','));

    if (this.benchmarkingform.value['TSPID'] != 0) {
      this.benchmarkingform.value['TSPID'] = this.TSPID.value.join(',');
    }
    if (!this.benchmarkingform.valid)
      return;
    this.working = true;
    this.ComSrv.postJSON('api/Benchmarking/BenchmarkingData', this.benchmarkingform.value)
      .subscribe((d: any) => {
        if (d.length != 0) {
          this.benchmarking = new MatTableDataSource(d);
          this.benchmarkingArray = d;
          this.benchmarking.paginator = this.paginator;
          this.benchmarking.sort = this.sort;
        }
        else {
          error => this.error = error
          this.error = "No Relevant Trades found against you search";
          this.ComSrv.ShowError(this.error.toString(), "Error");
          this.benchmarking = null;


        }
        //this.ComSrv.openSnackBar(this.BenchmarkingID.value > 0 ? environment.UpdateMSG.replace("${Name}", this.EnText) : environment.SaveMSG.replace("${Name}", this.EnText));
        //this.reset(myForm);
        //this.title = "Add New ";
        //this.savebtn = "Save ";
      },
        error => this.error = error // error path
        , () => {
          this.working = false;

        });
  }

  reset(myForm: FormGroupDirective) {
    this.benchmarkingform.reset();
    myForm.resetForm();
    this.BenchmarkingID.setValue(0);
    //this.title = "Add New ";
    //this.savebtn = "Save ";
  }


  get BenchmarkingID() { return this.benchmarkingform.get("BenchmarkingID"); }
  //get DistrictID() { return this.benchmarkingform.get("DistrictID"); }
  //get ClusterID() { return this.benchmarkingform.get("ClusterID"); }
  //get RegionID() { return this.benchmarkingform.get("RegionID"); }
  //get Inflation() { return this.benchmarkingform.get("Inflation"); }
  get ProgramTypeID() { return this.benchmarkingform.get("ProgramTypeID"); }
  get CostSharing() { return this.benchmarkingform.get("CostSharing"); }
  get TSPID() { return this.benchmarkingform.get("TSPID"); }
  get TradeName() { return this.benchmarkingform.get("TradeName"); }
  get ProposedAmount() { return this.benchmarkingform.get("ProposedAmount"); }
  get CalculatedAmount() { return this.benchmarkingform.get("CalculatedAmount"); }
  get CalculatedAmount70() { return this.benchmarkingform.get("CalculatedAmount70"); }
  get ProposedAmount50() { return this.benchmarkingform.get("ProposedAmount50"); }
  //get ClassFrom() { return this.benchmarkingform.get("ClassFrom"); }
  //get ClassTo() { return this.benchmarkingform.get("ClassTo"); }
  get TotalClasses() { return this.benchmarkingform.get("TotalClasses"); }
  get InActive() { return this.benchmarkingform.get("InActive"); }

}

export class BenchmarkingModel extends ModelBase {
  BenchmarkingID: number;
  TradeName: string;
  ProposedAmount: number;
  ClassFrom: string;
  ClassTo: string;
  OfferedAmount: number;
  CalculatedAmount: number;
  CalculatedAmount70: number;
  ProposedAmount50: number;
  TotalClasses: number;
  Inflation: number;

}
