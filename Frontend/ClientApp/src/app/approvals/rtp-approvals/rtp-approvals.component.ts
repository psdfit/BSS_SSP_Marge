import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormControl } from '@angular/forms';
import { CommonSrvService } from '../../common-srv.service';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { UserRightsModel } from '../../master-data/users/users.component';
import { DialogueService } from '../../shared/dialogue.service';
import { EnumApprovalProcess, EnumProgramCategory } from '../../shared/Enumerations';
import { UsersModel } from '../../master-data/users/users.component';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialog } from '@angular/material/dialog';
import { RTPApprovalDialogueComponent } from '../../inception-report/rtp-approval-dialogue/rtp-approval-dialogue.component';
import { MatTableDataSource } from '@angular/material/table';
import { ExportType, EnumExcelReportType } from '../../shared/Enumerations';
import { Workbook } from 'exceljs';
import * as fs from 'file-saver';
import { ExportExcel } from '../../shared/Interfaces';
import { DatePipe } from '@angular/common';
import { GroupByPipe } from 'angular-pipes';
import { environment } from '../../../environments/environment';
import { invalid } from 'moment';





@Component({
  selector: 'app-rtp-approvals',
  templateUrl: './rtp-approvals.component.html',
  styleUrls: ['./rtp-approvals.component.scss'],
  providers: [GroupByPipe, DatePipe]

})
export class RTPApprovalsComponent implements OnInit {
  environment = environment;
  SearchSch = new FormControl('',);
  SearchCls = new FormControl('',);
  SearchTSP = new FormControl('',);
  displayedColumns = ['GenerateRTPReport', 'SchemeName', 'TSPName',
    'TehsilName',
    'District',
    'CreatedDate',
    'ClassCode', 'TradeName',
    'TraineesPerClass', 'Duration',
    'Curriculum',
    //'RTPID',
    //'ClassID',
    'AddressOfTrainingLocation',
    'CPName', 'CPLandline',
    'StartDate',
    'IsApproved',
    //'CenterInspection',
    'NTP', 'GenerateReport','RejectRTP'];
  //schemes: MatTableDataSource<any>;

  filters: IRTPApprovalFilter = { SchemeID: 0, ClassID: 0, TSPID: 0};


  schemes: [];

  SchemeFilter = [];
  TSPDetailFilter = [];
  classesArrayFilter: any[];

  //rtplist: [];

  ActiveFormApprovalID: number;
  ChosenTradeID: number;
  userid: number;
  title: string;
  update: String;

  savebtn: string;
  formrights: UserRightsModel;
  currentUser: UsersModel;
  rtplist: MatTableDataSource<any>;
  rtpByID: MatTableDataSource<any>;
  centerInspectionArray: any;
  centerInspectionRequestArray: any;
  centerInspectionSecurityArray: any;
  centerInspectionIntegrityArray: any;
  centerInspectionInchargeArray: any;

  EnText: string = "";
  error: String;
  query = {
    order: 'RTPID',
    limit: 5,
    page: 1
  };
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  working: boolean;
  centerInspectionComplianceModel: any;
  centerInspectionTradeDetailModel: any;
  centerInspectionClassDetailModel: any;
  centerInspectionNecessaryFacilitiesModel: any;
  centerInspectionTradeToolModel: any;

  constructor(private http: CommonSrvService, public dialog: MatDialog, private dialogue: DialogueService, private _date: DatePipe
    , private groupByPipe: GroupByPipe

  ) {
    //this.schemes = new MatTableDataSource([]);
    this.formrights = http.getFormRights();
    this.rtplist = new MatTableDataSource([]);

  }

  ngOnInit(): void {
    this.currentUser = this.http.getUserDetails();
    this.userid = this.currentUser.UserID
    this.http.setTitle("RTPs");
    this.title = "";
    this.savebtn = "Approve";
    this.http.OID.subscribe(OID => {
      this.GetRTPs();
    });
  }

  getTSPDetailByScheme(schemeId: number) {
    this.classesArrayFilter = [];
    this.http.getJSON(`api/TSPDetail/GetTSPDetailByScheme/` + schemeId)
      .subscribe(data => {
        this.TSPDetailFilter = <any[]>data;
      }, error => {
        this.error = error;
      })
  }
  getClassesByTsp(tspId: number) {
    this.http.getJSON(`api/Class/GetClassesByTsp/` + tspId)
      .subscribe(data => {
        this.classesArrayFilter = <any[]>data;
      }, error => {
        this.error = error;
      })
  }

  EmptyCtrl() {
    this.SearchCls.setValue('');
    this.SearchTSP.setValue('');
    this.SearchSch.setValue('');
  }



  GetRTPs() {
    this.http.postJSON('api/RTP/GetRTPByKAMUser', { UserID: this.userid, OID: this.http.OID.value, SchemeID: this.filters.SchemeID, TSPID: this.filters.TSPID, ClassID: this.filters.ClassID }).subscribe((d: any) => {
      this.rtplist = new MatTableDataSource(d[0]);
      this.SchemeFilter = d[1];
      this.rtplist.paginator = this.paginator;
      this.rtplist.sort = this.sort;
    },
      error => this.error = error // error path
      , () => {
        this.working = false;
      });
  }

  GetCenterInspection(id: number) {
    this.http.getJSON('api/RTP/GetCenterInspectionReport/' + id).subscribe((d: any) => {
      this.centerInspectionArray = new MatTableDataSource(d[0]);
      this.centerInspectionRequestArray = d[1];
      this.centerInspectionSecurityArray = d[2];
      this.centerInspectionIntegrityArray = d[3];
      this.centerInspectionInchargeArray = d[4];
      this.centerInspectionComplianceModel = d[5];
      this.centerInspectionTradeDetailModel = d[6];
      this.centerInspectionClassDetailModel = d[7];
      this.centerInspectionNecessaryFacilitiesModel = d[8];
      this.centerInspectionTradeToolModel = d[9];

      this.generateCenterInspectionExcel();
      //this.rtplist.paginator = this.paginator;
      //this.rtplist.sort = this.sort;
    },
      error => this.error = error // error path
      , () => {
        this.working = false;
      });
  }

  GetRTP(id: number) {
    this.http.getJSON('api/RTP/GetRTPByID/' + id).subscribe((d: any) => {
      this.rtpByID = new MatTableDataSource(d[0]);

      this.exportToExcelRTP();
      //this.rtplist.paginator = this.paginator;
      //this.rtplist.sort = this.sort;
    },
      error => this.error = error // error path
      , () => {
        this.working = false;
      });
  }

  GenerateNTP(row: any) {
    //this.RTPID.setValue(this.data.RTPID)

    this.http.confirmNTP().subscribe(result => {
      if (result) {
        this.http.postJSON('api/RTP/GenerateNTP', { "RTPID": row.RTPID, "ClassID": row.ClassID })
          .subscribe((d: any) => {
            this.update = "NTP Created";
            this.http.openSnackBar(this.update.toString(), "Updated");
            //this.ComSrv.openSnackBar(row.InActive == true ? environment.InActiveMSG.replace("${Name}", this.EnText) : environment.ActiveMSG.replace("${Name}", this.EnText));
            // this.rtp =new MatTableDataSource(d);
          },
            error => this.error = error // error path
          );
      }
      else {
        row.NTP = !row.NTP;
      }
    });

  }


  generateCenterInspectionExcel() {
    let timeSpan = new Date().toISOString();

    let dataForExport = [];
    let dataForExportTradeDetail = [];
    let dataForExportClassDetail = [];
    let dataForExportTradeTool = [];

    let filteredData = [...this.centerInspectionArray.filteredData];
    let filteredDataRequest = this.centerInspectionRequestArray

    let data = {
      "Service Provider Name": this.groupByPipe.transform(filteredData, 'TSPName').map(x => x.key).join(','),
      "Name of Institute": this.groupByPipe.transform(filteredData, 'TrainingCentreName').map(x => x.key).join(','),
      "Visit Date & Time": this.groupByPipe.transform(filteredData, 'VisitDateTime').map(x => x.key).join(','),
      "Training Centre Address": this.groupByPipe.transform(filteredData, 'TrainingCentreAddress').map(x => x.key).join(','),
      "No. of Classes Inspected ": this.groupByPipe.transform(filteredData, 'ClassesInspectedCount').map(x => x.key).join(','),
      "Name of Centre Incharge ": this.groupByPipe.transform(filteredData, 'CentreInchargeName').map(x => x.key).join(','),
      "Incharge Mobile Number": this.groupByPipe.transform(filteredData, 'CentreInchargeMob').map(x => x.key).join(','),


      //filteredData.map(x => x.TrainingCentreAddress)
      //"TraineeImagesAdded": true
    };

    dataForExport = this.populateData(this.centerInspectionRequestArray);
    dataForExport.push(this.populateDataSecurity(this.centerInspectionSecurityArray))
    dataForExport.push(this.populateDataIntegrity(this.centerInspectionIntegrityArray))
    dataForExport.push(this.populateDataIncharge(this.centerInspectionInchargeArray))
    dataForExport.push(this.populateDataCompliance(this.centerInspectionComplianceModel))
    dataForExportTradeDetail.push(this.populateTradeDetailModel(this.centerInspectionTradeDetailModel))
    dataForExportClassDetail.push(this.populateClassDetailModel(this.centerInspectionClassDetailModel))
    dataForExportTradeTool.push(this.populateTradeToolModel(this.centerInspectionTradeToolModel))


    dataForExport = dataForExport.reduce((accumulator, value) => accumulator.concat(value), []);
    dataForExportTradeDetail = dataForExportTradeDetail.reduce((accumulator, value) => accumulator.concat(value), []);
    dataForExportClassDetail = dataForExportClassDetail.reduce((accumulator, value) => accumulator.concat(value), []);
    dataForExportClassDetail = dataForExportClassDetail.reduce((accumulator, value) => accumulator.concat(value), []);
    dataForExportTradeTool = dataForExportTradeTool.reduce((accumulator, value) => accumulator.concat(value), []);

    //dataForExport = this.populateData(result)

    let workbook = new Workbook();
    let workSheet = workbook.addWorksheet();

    let exportExcel: ExportExcel = {
      Title: 'Center Inspection Report',
      Author: '',
      Type: 0,
      Data: data,
      List1: this.populateData(this.centerInspectionRequestArray),
    }

    ///SET TITLEx
    let titleRow = workSheet.addRow(['Report Name :', exportExcel.Title]);
    titleRow.font = { bold: true, size: 14 }

    workSheet.addRow([]);

    if (exportExcel.Data) {
      Object.keys(exportExcel.Data).forEach(key => {
        let row = workSheet.addRow([key, exportExcel.Data[key]])
        row.height = 25;

        let qty = row.getCell(1);
        let color = 'FF99FF99';

        qty.fill = {
          type: 'pattern',
          pattern: 'solid',
          fgColor: { argb: 'cdcdcd' }
        }
        qty.border = { top: { style: 'thin' }, left: { style: 'thin' }, bottom: { style: 'thin' }, right: { style: 'thin' } }
        qty.font = { bold: true }

      })
    }

    workSheet.addRow([]);


    //let dataRowHeader = workSheet.addRow(['Parameters', 'Compliance', 'Observatory Remarks', 'Recommendation Remarks']);
    //dataRowHeader.font = { bold: true, size: 14 }
    //dataRowHeader.eachCell((cell, number) => {
    //    cell.fill = {
    //        type: 'pattern',
    //        pattern: 'solid',
    //        fgColor: { argb: 'cdcdcd' },
    //        //bgColor: { argb: 'cdcdcd' }
    //    }
    //    cell.border = { top: { style: 'thin' }, left: { style: 'thin' }, bottom: { style: 'thin' }, right: { style: 'thin' } }
    //    cell.font = { bold: true }
    //    cell.alignment = { vertical: 'middle', horizontal: 'center', readingOrder: "ltr" }
    //});



    dataForExport.forEach((item, index) => {
      let keys = Object.keys(item);
      //let values = Object.entries(item).map(([key, value]) => value);
      let values = Object.values(item);

      ///SET SERIAL NUMBER
      keys.unshift("Sr#");
      values.unshift(++index)
      index--;

      if (index == 0) {
        ///SET HEADER
        let headerRow = workSheet.addRow(keys);
        headerRow.height = 25;
        headerRow.eachCell((cell, number) => {
          cell.fill = {
            type: 'pattern',
            pattern: 'solid',
            fgColor: { argb: 'cdcdcd' },
            //bgColor: { argb: 'cdcdcd' }
          }
          cell.border = { top: { style: 'thin' }, left: { style: 'thin' }, bottom: { style: 'thin' }, right: { style: 'thin' } }
          cell.font = { bold: true }
          cell.alignment = { vertical: 'middle', horizontal: 'center', readingOrder: "ltr" }
        });
      };

      ///SET COLUMN VALUES
      let row = workSheet.addRow(values);
      row.height = 30;
      row.eachCell((cell, number) => {
        cell.border = { top: { style: 'thin' }, left: { style: 'thin' }, bottom: { style: 'thin' }, right: { style: 'thin' } };
        cell.alignment = { vertical: 'middle', readingOrder: "ltr" }
        workSheet.getColumn(number).width = 20;
      })

    });


    //Add blank row
    workSheet.addRow([]);
    //Trade Data
    let tradeTitle = workSheet.addRow(["TRADE INFORMATION (Detail of Trades to be conducted at the centre)"]);
    tradeTitle.height = 25;
    tradeTitle.font = { bold: true, size: 12 };
    tradeTitle.eachCell(cell => cell.merge)

    dataForExportTradeDetail.forEach((item, index) => {
      let keys = Object.keys(item);
      //let values = Object.entries(item).map(([key, value]) => value);
      let valuesTradeDetail = Object.values(item);

      ///SET SERIAL NUMBER
      keys.unshift("Sr#");
      valuesTradeDetail.unshift(++index)
      index--;

      if (index == 0) {
        ///SET HEADER
        let headerRow = workSheet.addRow(keys);
        headerRow.height = 25;
        headerRow.eachCell((cell, number) => {
          cell.fill = {
            type: 'pattern',
            pattern: 'solid',
            fgColor: { argb: 'cdcdcd' },
            //bgColor: { argb: 'cdcdcd' }
          }
          cell.border = { top: { style: 'thin' }, left: { style: 'thin' }, bottom: { style: 'thin' }, right: { style: 'thin' } }
          cell.font = { bold: true }
          cell.alignment = { vertical: 'middle', horizontal: 'center', readingOrder: "ltr" }
        });
      };

      ///SET COLUMN VALUES
      let row = workSheet.addRow(valuesTradeDetail);
      row.height = 30;
      row.eachCell((cell, number) => {
        cell.border = { top: { style: 'thin' }, left: { style: 'thin' }, bottom: { style: 'thin' }, right: { style: 'thin' } };
        cell.alignment = { vertical: 'middle', readingOrder: "ltr" }
        workSheet.getColumn(number).width = 20;
      })

    });



    //let tradeDataArray = [
    //  {
    //    "Trade": "Computer Network Technician",
    //    "No of Classes Per Batch": "3",
    //    "No of Contractual Trainees Per Class": "25",
    //    "The quantity is sufficient for no of contractual trainees?": "Yes",
    //    "No of items Missing": "",
    //    "No. of Rooms for Labs": "1",
    //    "Is the space sufficient for trainees per class?": "Yes",
    //    "Power Backup availability in the labs": "Yes",
    //  }]
    //this.addDataList(valuesTradeDetail, workSheet)

    //Add blank row
    workSheet.addRow([]);
    //Class Information 
    let classTitle = workSheet.addRow(["CLASS INFORMATION (Detail of Classes to be conducted at the centre)"]);
    classTitle.height = 25;
    classTitle.font = { bold: true, size: 12 };
    classTitle.eachCell(cell => cell.merge)



    dataForExportClassDetail.forEach((item, index) => {
      let keys = Object.keys(item);
      //let values = Object.entries(item).map(([key, value]) => value);
      let valuesClassDetail = Object.values(item);

      ///SET SERIAL NUMBER
      keys.unshift("Sr#");
      valuesClassDetail.unshift(++index)
      index--;

      if (index == 0) {
        ///SET HEADER
        let headerRow = workSheet.addRow(keys);
        headerRow.height = 25;
        headerRow.eachCell((cell, number) => {
          cell.fill = {
            type: 'pattern',
            pattern: 'solid',
            fgColor: { argb: 'cdcdcd' },
            //bgColor: { argb: 'cdcdcd' }
          }
          cell.border = { top: { style: 'thin' }, left: { style: 'thin' }, bottom: { style: 'thin' }, right: { style: 'thin' } }
          cell.font = { bold: true }
          cell.alignment = { vertical: 'middle', horizontal: 'center', readingOrder: "ltr" }
        });
      };

      ///SET COLUMN VALUES
      let row = workSheet.addRow(valuesClassDetail);
      row.height = 30;
      row.eachCell((cell, number) => {
        cell.border = { top: { style: 'thin' }, left: { style: 'thin' }, bottom: { style: 'thin' }, right: { style: 'thin' } };
        cell.alignment = { vertical: 'middle', readingOrder: "ltr" }
        workSheet.getColumn(number).width = 20;
      })

    });



    //let classDataArray = [
    //  {
    //    "Class code": "Computer Network Technician",
    //    "Trade": "3",
    //    "Expected starting date of class": "25",
    //    "Black-board/Whiteboard in classrooms(YES / NO)": "Yes",
    //    "Chairs/Benches in the classrooms are sufficient (According to class need) (YES / NO)": "",
    //    "Light & Bulbs availability (YES / NO)": "1",
    //    "Ventilation & Fans availability (YES / NO)": "Yes",
    //    "Is the space sufficient for Trainees per Class? (YES / NO)": "Yes",
    //  }]
    //this.addDataList(classDataArray, workSheet)
    //
    //Add blank row
    workSheet.addRow([]);
    //pragraph 
    let p = workSheet.addRow(["Are one or more of the following key facilities necessary for the approval of a centre missing?", this.centerInspectionNecessaryFacilitiesModel[0].KeyFacMissing]);
    p.height = 25;
    p.font = { bold: true, size: 12 };
    p.eachCell(cell => cell.merge)
    //
    //Add blank row
    workSheet.addRow([]);
    //custom table 
    let a = workSheet.addRow(["a", "Structural integrity compliance of building", this.centerInspectionNecessaryFacilitiesModel[0].StructIntegValue]);
    a.height = 25;
    //a.font = { bold: true, size: 12 };
    a.eachCell((cell, number) => {
      cell.border = { top: { style: 'thin' }, left: { style: 'thin' }, bottom: { style: 'thin' }, right: { style: 'thin' } };
      cell.alignment = { vertical: 'middle', readingOrder: "ltr" }
      workSheet.getColumn(number).width = 20;
    })
    //
    let b = workSheet.addRow(["b", "Chairs/benches/blackboard", this.centerInspectionNecessaryFacilitiesModel[0].KeyFacBuildingStructInteg]);
    b.height = 25;
    //b.font = { bold: true, size: 12 };
    b.eachCell((cell, number) => {
      cell.border = { top: { style: 'thin' }, left: { style: 'thin' }, bottom: { style: 'thin' }, right: { style: 'thin' } };
      cell.alignment = { vertical: 'middle', readingOrder: "ltr" }
      workSheet.getColumn(number).width = 20;
    })
    //
    let c = workSheet.addRow(["c", "Backup electricity/power supply availability (If applicable)", this.centerInspectionNecessaryFacilitiesModel[0].KeyFacElecBackup]);
    c.height = 25;
    //c.font = { bold: true, size: 12 };
    c.eachCell((cell, number) => {
      cell.border = { top: { style: 'thin' }, left: { style: 'thin' }, bottom: { style: 'thin' }, right: { style: 'thin' } };
      cell.alignment = { vertical: 'middle', readingOrder: "ltr" }
      workSheet.getColumn(number).width = 20;
    })
    //
    let d = workSheet.addRow(["d", "Workshop/equipment availability)", this.centerInspectionNecessaryFacilitiesModel[0].KeyFacEquipAval]);
    d.height = 25;
    // d.font = { bold: true, size: 12 };
    d.eachCell((cell, number) => {
      cell.border = { top: { style: 'thin' }, left: { style: 'thin' }, bottom: { style: 'thin' }, right: { style: 'thin' } };
      cell.alignment = { vertical: 'middle', readingOrder: "ltr" }
      workSheet.getColumn(number).width = 20;
    })
    //
    //Add blank row
    workSheet.addRow([]);
    //
    let f = workSheet.addRow(["Name of Monitoring Officer", this.centerInspectionNecessaryFacilitiesModel[0].FMName, "Name of TSP Representative", this.centerInspectionNecessaryFacilitiesModel[0].SignOffTspName, "Name of TPM Field Incharge", this.centerInspectionNecessaryFacilitiesModel[0].DistrictInchargeName]);
    f.height = 25;
    // d.font = { bold: true, size: 12 };
    f.eachCell((cell, number) => {
      cell.border = { top: { style: 'thin' }, left: { style: 'thin' }, bottom: { style: 'thin' }, right: { style: 'thin' } };
      cell.alignment = { vertical: 'middle', readingOrder: "ltr" }
      workSheet.getColumn(number).width = 20;
      if ((number % 2) != 0) {
        cell.fill = {
          type: 'pattern',
          pattern: 'solid',
          fgColor: { argb: 'cdcdcd' }
        }
      }
    })
    //


    //let image = workbook.addImage({ base64: item[key], extension: 'jpeg' })
    //let cell = row.getCell(indx + 1);
    //cell.value = '';
    //cell.removeName;
    ////workSheet.addImage(image, `${cell.address}:${cell.address}`);
    //workSheet.addImage(image, {
    //  tl: { col: indx + 0.5, row: row.number - 1 + 0.2 },
    //  ext: { width: 80, height: 80 },
    //  editAs: "absolute"
    //});

    let g = workSheet.addRow(["Signature", "", "Signature", "", "Signature", ""]);
    g.height = 50;

    //let imageRow = g.getCell();
    let TSPSignPath = this.centerInspectionNecessaryFacilitiesModel[0].TspSignatureImgPath;
    if (TSPSignPath != '' && TSPSignPath != null && TSPSignPath != undefined)
    {
      let base64ImagePath = 'data:image/png;base64,' + TSPSignPath;
      let image = workbook.addImage({ base64: base64ImagePath, extension: 'jpeg' })
      workSheet.addImage(image, {
        tl: { col: 3, row: 36 },
        ext: { width: 80, height: 60 },
        editAs: "absolute"
      });
    }


    let FMSignPath = this.centerInspectionNecessaryFacilitiesModel[0].FMSignatureImgPath;
    if (FMSignPath != '' && FMSignPath != null && FMSignPath != undefined)
    {

      let base64ImagePath = 'data:image/png;base64,' + FMSignPath;
      let image = workbook.addImage({ base64: base64ImagePath, extension: 'jpeg' })
      workSheet.addImage(image, {
        tl: { col: 1, row: 36 },
        ext: { width: 80, height: 60 },
        editAs: "absolute"
      });
    }


    let TSPRepSignPath = this.centerInspectionNecessaryFacilitiesModel[0].TspRepImgPath;
    if (TSPRepSignPath != '' && TSPRepSignPath != null && TSPRepSignPath != undefined) {

      let base64ImagePath = 'data:image/png;base64,' + TSPRepSignPath;
      let image = workbook.addImage({ base64: base64ImagePath, extension: 'jpeg' })
      workSheet.addImage(image, {
        tl: { col: 5, row: 36 },
        ext: { width: 80, height: 60 },
        editAs: "absolute"
      });
    }

    // d.font = { bold: true, size: 12 };
    g.eachCell((cell, number) => {
      cell.border = { top: { style: 'thin' }, left: { style: 'thin' }, bottom: { style: 'thin' }, right: { style: 'thin' } };
      cell.alignment = { vertical: 'middle', readingOrder: "ltr" }
      workSheet.getColumn(number).width = 20;
      if ((number % 2) != 0) {
        cell.fill = {
          type: 'pattern',
          pattern: 'solid',
          fgColor: { argb: 'cdcdcd' }
        }
      }
    })
    //
    let h = workSheet.addRow(["Date", this.centerInspectionNecessaryFacilitiesModel[0].FMSubmissionDateTime, "Date", this.centerInspectionNecessaryFacilitiesModel[0].FMSubmissionDateTime, "Date", this.centerInspectionNecessaryFacilitiesModel[0].FMSubmissionDateTime]);
    h.height = 50;
    // d.font = { bold: true, size: 12 };
    h.eachCell((cell, number) => {
      cell.border = { top: { style: 'thin' }, left: { style: 'thin' }, bottom: { style: 'thin' }, right: { style: 'thin' } };
      cell.alignment = { vertical: 'middle', readingOrder: "ltr" }
      workSheet.getColumn(number).width = 20;
      if ((number % 2) != 0) {
        cell.fill = {
          type: 'pattern',
          pattern: 'solid',
          fgColor: { argb: 'cdcdcd' }
        }
      }
    })
    //Add blank row
    workSheet.addRow([]);
    //
    let i = workSheet.addRow(["TSP Remarks"]);
    i.height = 25;
    i.font = { bold: true, size: 12 };
    i.eachCell(cell => cell.merge)
    //
    let j = workSheet.addRow([this.centerInspectionNecessaryFacilitiesModel[0].SignOffTspRemarks]);
    j.height = 50;
    // d.font = { bold: true, size: 12 };
    j.eachCell((cell, number) => {
      cell.border = { top: { style: 'thin' }, left: { style: 'thin' }, bottom: { style: 'thin' }, right: { style: 'thin' } };
      cell.alignment = { readingOrder: "ltr" }
      workSheet.getColumn(number).width = 40;
    })
    //Add blank row
    workSheet.addRow([]);
    //
    let k = workSheet.addRow(["Field Monitoring Remarks"]);
    k.height = 25;
    k.font = { bold: true, size: 12 };
    k.eachCell(cell => cell.merge)
    //
    let l = workSheet.addRow([this.centerInspectionNecessaryFacilitiesModel[0].SignOffFmRemarks]);
    l.height = 50;
    // d.font = { bold: true, size: 12 };
    l.eachCell((cell, number) => {
      cell.border = { top: { style: 'thin' }, left: { style: 'thin' }, bottom: { style: 'thin' }, right: { style: 'thin' } };
      cell.alignment = { readingOrder: "ltr" }
      workSheet.getColumn(number).width = 40;
    })
    //Add blank row
    workSheet.addRow([]);
    //
    let m = workSheet.addRow(["Trade Name", this.centerInspectionTradeToolModel[0].TradeName]);
    m.height = 25;
    // d.font = { bold: true, size: 12 };
    m.eachCell((cell, number) => {
      cell.border = { top: { style: 'thin' }, left: { style: 'thin' }, bottom: { style: 'thin' }, right: { style: 'thin' } };
      cell.alignment = { readingOrder: "ltr" }
      workSheet.getColumn(number).width = 40;
      if (number == 1) {
        cell.fill = {
          type: 'pattern',
          pattern: 'solid',
          fgColor: { argb: 'cdcdcd' }
        }
      }
    })
    //
    let n = workSheet.addRow(["Course Duration", this.centerInspectionTradeToolModel[0].TradeDuration]);
    n.height = 25;
    // d.font = { bold: true, size: 12 };
    n.eachCell((cell, number) => {
      cell.border = { top: { style: 'thin' }, left: { style: 'thin' }, bottom: { style: 'thin' }, right: { style: 'thin' } };
      cell.alignment = { readingOrder: "ltr" }
      workSheet.getColumn(number).width = 40;
      if (number == 1) {
        cell.fill = {
          type: 'pattern',
          pattern: 'solid',
          fgColor: { argb: 'cdcdcd' }
        }
      }
    })
    //
    let o = workSheet.addRow(["Trainees Headcount", this.centerInspectionTradeToolModel[0].headCount]);
    o.height = 25;
    // d.font = { bold: true, size: 12 };
    o.eachCell((cell, number) => {
      cell.border = { top: { style: 'thin' }, left: { style: 'thin' }, bottom: { style: 'thin' }, right: { style: 'thin' } };
      cell.alignment = { readingOrder: "ltr" }
      workSheet.getColumn(number).width = 40;
      if (number == 1) {
        cell.fill = {
          type: 'pattern',
          pattern: 'solid',
          fgColor: { argb: 'cdcdcd' }
        }
      }
    })
    //



    dataForExportTradeTool.forEach((item, index) => {
      let keys = Object.keys(item);
      //let values = Object.entries(item).map(([key, value]) => value);
      let valuesTradeTool = Object.values(item);

      ///SET SERIAL NUMBER
      keys.unshift("Sr#");
      valuesTradeTool.unshift(++index)
      index--;

      if (index == 0) {
        ///SET HEADER
        let headerRow = workSheet.addRow(keys);
        headerRow.height = 25;
        headerRow.eachCell((cell, number) => {
          cell.fill = {
            type: 'pattern',
            pattern: 'solid',
            fgColor: { argb: 'cdcdcd' },
            //bgColor: { argb: 'cdcdcd' }
          }
          cell.border = { top: { style: 'thin' }, left: { style: 'thin' }, bottom: { style: 'thin' }, right: { style: 'thin' } }
          cell.font = { bold: true }
          cell.alignment = { vertical: 'middle', horizontal: 'center', readingOrder: "ltr" }
        });
      };

      ///SET COLUMN VALUES
      let row = workSheet.addRow(valuesTradeTool);
      row.height = 30;
      row.eachCell((cell, number) => {
        cell.border = { top: { style: 'thin' }, left: { style: 'thin' }, bottom: { style: 'thin' }, right: { style: 'thin' } };
        cell.alignment = { vertical: 'middle', readingOrder: "ltr" }
        workSheet.getColumn(number).width = 20;
      })

    });





    //let tradeToolList = [
    //  {
    //    "Tool Name": "Server Machine (64 bit/32 bit)",
    //    "Tool Quantity": "2",
    //    "Tool Actual Quantity": "2"
    //  }];
    //this.addDataList(tradeToolList, workSheet);
    workbook.xlsx.writeBuffer().then((data) => {
      let blob = new Blob([data], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
      fs.saveAs(blob, `Centre_Inspection_Report.${ExportType.XLSX}`);

      //this.onNoClick();
    }).catch(error => {
      console.error(error);
      //this.onNoClick();
    });

  }
  addDataList(list, workSheet) {
    list.forEach((item, index) => {
      let keys = Object.keys(item);
      //let values = Object.entries(item).map(([key, value]) => value);
      let values = Object.values(item);

      ///SET SERIAL NUMBER
      keys.unshift("Sr#");
      values.unshift(++index)
      index--;

      if (index == 0) {
        ///SET HEADER
        let headerRow = workSheet.addRow(keys);
        headerRow.height = 25;
        headerRow.eachCell((cell, number) => {
          cell.fill = {
            type: 'pattern',
            pattern: 'solid',
            fgColor: { argb: 'cdcdcd' },
            //bgColor: { argb: 'cdcdcd' }
          }
          cell.border = { top: { style: 'thin' }, left: { style: 'thin' }, bottom: { style: 'thin' }, right: { style: 'thin' } }
          cell.font = { bold: true }
          cell.alignment = { vertical: 'middle', horizontal: 'center', readingOrder: "ltr" }
        });
      };

      ///SET COLUMN VALUES
      let row = workSheet.addRow(values);
      row.height = 30;
      row.eachCell((cell, number) => {
        cell.border = { top: { style: 'thin' }, left: { style: 'thin' }, bottom: { style: 'thin' }, right: { style: 'thin' } };
        cell.alignment = { vertical: 'middle', horizontal: 'center', readingOrder: "ltr" }
        workSheet.getColumn(number).width = 20;
      })

    });
  }
  populateData(data: any) {
    return data.map((item, index) => {
      return {
        //"Sr#": ++index
        "Parameter": item.LocationAccessSuitablity
        , "Compliance": item.LocAccessSuitabilityValue
        , "Observatory Remarks": item.LocAccessSuitabilityObservRemarks
        , "PSDF Compliance Standard/Need": "The training centre should be situated at reasonable location that is easily accessible.No garbage, drainage lines or dirt around."
        , "Recommendation Remarks": item.LocAccessSuitabilityRecomRemarks
      }
    })
  }
  populateDataSecurity(data: any) {
    return data.map((item, index) => {
      return {
        //"Sr#": ++index
        "Parameter": item.SecurityPremises
        , "Compliance": item.SecurityPremValue
        , "Observatory Remarks": item.SecurityPremObservRemarks
        , "PSDF Compliance Standard/Need": "The area where training centre is located should have proper boundary wall with secure entrance."
        , "Recommendation Remarks": item.SecurityPremRecomRemarks
      }
    })
  }
  populateDataIntegrity(data: any) {
    return data.map((item, index) => {
      return {
        //"Sr#": ++index
        "Parameter": item.StructuralIntegrityCompliance
        , "Compliance": item.StructIntegValue
        , "Observatory Remarks": item.StructIntegObservRemarks
        , "PSDF Compliance Standard/Need": "A building that is weak in structure forexample major flaws in construction of walls and ceiling; major cracks that can collapse the building or serious gaps in electrical wiring that can cause accidents is not acceptable."
        , "Recommendation Remarks": item.StructIntegRecomRemarks
      }
    })
  }
  populateDataIncharge(data: any) {
    return data.map((item, index) => {
      return {
        //"Sr#": ++index
        "Parameter": item.CentreInchargeRoom
        , "Compliance": item.CentreInchrgRoomValue
        , "Observatory Remarks": item.CentreInchrgRoomObservRemarks
        , "PSDF Compliance Standard/Need": "There should be a room for the centre incharge."
        , "Recommendation Remarks": item.CentreInchrgRoomRecomRemarks
      }
    })
  }
  populateDataCompliance(data: any) {
    console.log(data);
    return data.map((item, index) => {
      return {
        //"Sr#": ++index
        "Parameter": item.parameter
        , "Compliance": item.complaince
        , "Observatory Remarks": item.observatoryRemarks
        , "PSDF Compliance Standard/Need": item.psdfStandard
        , "Recommendation Remarks": item.recommendationRemarks
      }
    })
  }

  populateTradeDetailModel(data: any) {
    console.log(data);
    return data.map((item, index) => {
      return {
        "Trade": item.tradeName
        , "No of Classes Per Batch": item.classesPerBatch
        , "The quantity is sufficient for no of contractual trainees?": item.quantitySufficient
        , "No of items Missing": item.noOfItemsMissing
        , "No of Contractual Trainees Per Class": item.totalContractrualTraineesPerClass
        , "No. of Rooms for Labs": item.noOfRoomsForLab
        , "Is the space sufficient for trainees per class?": item.isSpaceSufficient
        , "Power Backup availability in the labs": item.powerBackupAvailability
      }
    })
  }

  populateClassDetailModel(data: any) {
    console.log(data);
    return data.map((item, index) => {
      return {
        "Class code": item.ClassCode
        , "Trade": item.TradeName
        , "Expected starting date of class": item.ExpectedStartDate
        , "Black-board/Whiteboard in classrooms(YES / NO)": item.BoardAval
        , "Chairs/Benches in the classrooms are sufficient (According to class need) (YES / NO)": item.SufficientFurniture
        , "Light & Bulbs availability (YES / NO)": item.LightAval
        , "Ventilation & Fans availability (YES / NO)": item.VentFanAval
        , "Is the space sufficient for Trainees per Class? (YES / NO)": item.ClassSpaceSufficient
      }
    })
  }
  populateTradeToolModel(data: any) {
    console.log(data);
    return data.map((item, index) => {
      return {
        "Tool Name": item.ToolName
        , "Tool Quantity": item.QuantityTotal
        , "Tool Actual Quantity": item.QuantityFound
      }
    })
  }
  populateClassNecessaryFaciltiesModel(data: any) {
    console.log(data);
    return data.map((item, index) => {
      return {
        "Class code": item.ClassCode
        , "Trade": item.TradeName
        , "Expected starting date of class": item.ExpectedStartDate
        , "Black-board/Whiteboard in classrooms(YES / NO)": item.BoardAval
        , "Chairs/Benches in the classrooms are sufficient (According to class need) (YES / NO)": item.SufficientFurniture
        , "Light & Bulbs availability (YES / NO)": item.LightAval
        , "Ventilation & Fans availability (YES / NO)": item.VentFanAval
        , "Is the space sufficient for Trainees per Class? (YES / NO)": item.ClassSpaceSufficient
      }
    })
  }



  exportToExcelRTP(name?: string) {
    let filteredData = [...this.rtpByID.filteredData]
    let filteredData2 = [...this.rtplist.filteredData]
    //let removeKeys = Object.keys(filteredData[0]).filter(x => !this.displayedColumns.includes(x));
    //let data = [];//filteredData.map(x => { removeKeys.forEach(key => delete x[key]); return x });
    //filteredData.forEach(item => {
    //    let obj = {};
    //    this.displayedColumns.forEach(key => {
    //        obj[key] = item[key] || "";
    //    });
    //    data.push(obj)
    //})

    let data = {
      "Training Scheme(s)": this.groupByPipe.transform(filteredData, 'SchemeName').map(x => x.key).join(','),
      "Name of Training Service Provider(s)": this.groupByPipe.transform(filteredData, 'TSPName').map(x => x.key).join(','),

      //"TraineeImagesAdded": true
    };



    let exportExcel: ExportExcel = {
      Title: 'RTP Report',
      Author: this.currentUser.FullName,
      Type: EnumExcelReportType.RTP,
      Data: data,
      List1: this.populateRTPData(filteredData),
    };
    this.dialogue.openExportConfirmDialogue(exportExcel).subscribe();
  }


  populateRTPData(data: any) {
    return data.map(item => {
      return {
        "Class Code": item.ClassCode
        , "Trade": item.TradeName
        , "Date of Request(dd-mm-yyyy)": this._date.transform(item.CreatedDate, 'dd-MM-yyyy')
        , "Number Of Trainees": item.TraineesPerClass
        , "Duration (In Months)": item.Duration
        , "Curriculum Followed": item.Name
        , "Address of Training Location": item.AddressOfTrainingLocation
        , "Name of Contact Perosn": item.CPName
        , "Mobile Number of Contact Person": item.CPLandline
        , "Expected Start Date": this._date.transform(item.StartDate, 'dd-MM-yyyy')

      }
    })
  }



  openApprovalDialogue(row: any): void {

    const dialogRef = this.dialog.open(RTPApprovalDialogueComponent, {
      minWidth: '800px',
      minHeight: '400px',
      //data: JSON.parse(JSON.stringify(row))
      data: { "ClassID": row.ClassID, "ClassCode": row.ClassCode, 'RTPID': row.RTPID, 'IsApproved': row.IsApproved, 'NTP': row.NTP }
    })
    dialogRef.afterClosed().subscribe(result => {
      //if (result.value.IsApproved == '' || result.value.IsRejected == '') {
      if (!result) {
        row.IsApproved = !row.IsApproved
      }
      else {
        row.IsApproved = row.IsApproved
      }

    });
  }
  openRejectionDialogue(row: any): void {

    const dialogRef = this.dialog.open(RTPApprovalDialogueComponent, {
      minWidth: '800px',
      minHeight: '400px',
      //data: JSON.parse(JSON.stringify(row))
      data: { "ClassID": row.ClassID, "ClassCode": row.ClassCode, 'RTPID': row.RTPID, 'IsApproved': row.IsApproved, 'NTP': row.NTP, 'OnRejectionDialogue' :true }
    })
    dialogRef.afterClosed().subscribe(result => {
      //if (result.value.IsApproved == '' || result.value.IsRejected == '') {
      if (!result && row.RejectRTP) {
        row.RejectRTP = !row.IsApproved
      }
      else {
        row.RejectRTP = row.RejectRTP

      }

    });
  }

  openClassJourneyDialogue(data: any): void 
  {
		debugger;
		this.dialogue.openClassJourneyDialogue(data);
  }
}



export interface IRTPApprovalFilter {
  SchemeID: number;
  TSPID: number;
  ClassID: number;
  //UserID: number;
}
