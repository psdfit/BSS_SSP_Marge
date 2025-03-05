import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormGroup, Validators, FormControl, FormArray, FormBuilder, AbstractControl } from '@angular/forms';
import * as XLSX from 'xlsx';
import { ExportType, EnumExcelReportType } from '../../shared/Enumerations';
import { Workbook } from 'exceljs';
import * as fs from 'file-saver';
import { ExportExcel } from '../../shared/Interfaces';
import { DatePipe } from '@angular/common';
import { CommonSrvService } from '../../common-srv.service';
import { GroupByPipe } from 'angular-pipes';
@Component({
  selector: 'app-export-confirm-dialogue',
  templateUrl: './export-confirm-dialogue.component.html',
  styleUrls: ['./export-confirm-dialogue.component.scss'],
  providers: [DatePipe, GroupByPipe]
})
export class ExportConfirmDialogueComponent implements OnInit {
  approvalHistory = [];
  exportColumnForm: FormGroup;
  columnList: string[];
  disabledExportButton: boolean;
  checkAll = true;
  constructor(private fb: FormBuilder, private datePipe: DatePipe, public dialogRef: MatDialogRef<ExportConfirmDialogueComponent>,
    @Inject(MAT_DIALOG_DATA) public input: ExportExcel, private commonService: CommonSrvService, private groupByPipe: GroupByPipe) {
    dialogRef.disableClose = true;
    if (this.input.Type === EnumExcelReportType.TSR || this.input.Type === EnumExcelReportType.TAR) {
      this.columnList = Object.keys(this.input.DataModel);
    }
    else {
      this.columnList = Object.keys(this.input.List1[0]);
    }

    const formControls = this.columnList.map((control, index) => {
      let checked = true;
      if (this.input.Type === EnumExcelReportType.MasterSheet && (index === 0 || index === 11 || index > 41 || index === 7 || index === 20 || index === 21 || index === 28)) {
        checked = false;
        this.checkAll = false;
      }
      if (this.input.Type === EnumExcelReportType.TSR && (index === 19 || index > 41)) {
        checked = false;
        this.checkAll = false;
      }
      // if (this.input.Type === EnumExcelReportType.TAR) {
      //   console.log('hello')
      //   checked = false;
      //   this.checkAll = false;
      // }
      const checkbox = this.fb.control(checked);
      checkbox.valueChanges.subscribe(value => {
        this.disabledExportButton = this.columns.controls.filter(x => x.value === false).length === this.columnList.length;
        this.checkAll = this.columns.controls.filter(x => !x.value).map(x => this.getControlName(x)).length === 0;
      });
      return checkbox;
    });
    this.exportColumnForm = this.fb.group(
      {
        columns: this.fb.array(formControls)
      });
    // this.columns.controls.forEach(x => x.valueChanges.subscribe(value => {
    //  this.enableExportButton = this.columns.controls.filter(x => x.value === false).length != this.data.length
    // }))
  }
  get columns() {
    return this.exportColumnForm.get('columns') as FormArray;
  };
  ngOnInit(): void {
  }
  exportToExcel() {
    const timeSpan = new Date().toISOString();
    const prefix = this.input.Title || 'Exported';
    const fileName = `${prefix}-${timeSpan}`;
    const uncheckedKeys = this.columns.controls.filter(x => !x.value).map(x => this.getControlName(x));
    const dataForExport = this.input.List1.map(x => { uncheckedKeys.forEach(key => delete x[key]); return x });
    const ws: XLSX.WorkSheet = XLSX.utils.json_to_sheet(dataForExport);
    const wb: XLSX.WorkBook = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, prefix);
    XLSX.writeFile(wb, `${fileName}.${ExportType.XLSX}`);
    this.onNoClick();
  }
  btnActive = true
  async generateExcel() {

    if (this.input.Type === EnumExcelReportType.TSR || this.input.Type === EnumExcelReportType.TAR) {
      this.btnActive = false
      this.input.SearchFilters.SelectedColumns = [];
      this.columns.controls.filter(x => x.value).forEach(control => {
        this.input.DataModel[this.getControlName(control)].split(',').forEach(column => {
          this.input.SearchFilters.SelectedColumns.push(column);
        })
      });
      /// remove duplicates
      this.input.SearchFilters.SelectedColumns = [...new Set(this.input.SearchFilters.SelectedColumns)]
      await this.input.LoadDataAsync(this);
      this.btnActive = true
    }

    const timeSpan = new Date().toISOString();
    const prefix = this.input.Title || 'Exported';
    const fileName = `${prefix} - ${timeSpan}`;
    const nonSelectedColumns = this.columns.controls.filter(x => !x.value).map(x => this.getControlName(x));
    const dataForExport = this.input.List1.map(x => { nonSelectedColumns.forEach(key => delete x[key]); return x });

    const workbook = new Workbook();
    const workSheet = workbook.addWorksheet(this.input.Title);


    /// SET TITLE
    const titleRow = workSheet.addRow(['Report Name :', this.input.Title]);
    titleRow.font = { bold: true, size: 14 }
    const authorRow = workSheet.addRow(['Generated By :', this.input.Author]);
    const createdTimeRow = workSheet.addRow(['Generated Time :', this.datePipe.transform(new Date(), 'dd/MM/yyyy hh:mm a')]);
    if (this.input.Type === EnumExcelReportType.MPR || this.input.Type === EnumExcelReportType.SRN) {
      const reportMonthRow = workSheet.addRow(['Report Month:', this.datePipe.transform(this.input.Month, 'MMMM yyyy')]);
    }
    // const ParameterTitle = workSheet.addRow(['Parameter :']);
    // ParameterTitle.font = { bold: true, size: 14 }

    workSheet.addRow([]);

    if (this.input.Data) {
      Object.keys(this.input.Data).forEach(key => {
        const row = workSheet.addRow([key, this.input.Data[key]])
      })
      if (this.input.Type === EnumExcelReportType.TSR || this.input.Type === EnumExcelReportType.TAR) {
        workSheet.addRow([]);
        workSheet.addRow([]);
      }
      else {
        workSheet.addRow([]);
        workSheet.addRow([]);
        workSheet.addRow([]);
      }
    }

    dataForExport.forEach((item, index) => {
      const keys = Object.keys(item);
      // let values = Object.entries(item).map(([key, value]) => value);
      const values = Object.values(item);


      if (this.input.Type === EnumExcelReportType.NTP || this.input.Type === EnumExcelReportType.RTP
        || this.input.Type === EnumExcelReportType.PRN
        || this.input.Type === EnumExcelReportType.PRN_R
        || this.input.Type === EnumExcelReportType.PRN_C
        || this.input.Type === EnumExcelReportType.PRN_F
        || this.input.Type === EnumExcelReportType.PRN_T) {
        // SET SERIAL NUMBER
        keys.unshift('Sr#');
        values.unshift(++index)
        index--;
      }
      /// SET SERIAL NUMBER
      // keys.unshift("Sr#");
      // values.unshift(++index)
      // index--;

      if (index === 0) {
        /// SET HEADER
        const headerRow = workSheet.addRow(keys);
        headerRow.height = 25;
        headerRow.eachCell((cell, number) => {
          cell.fill = {
            type: 'pattern',
            pattern: 'solid',
            fgColor: { argb: 'cdcdcd' },
            // bgColor: { argb: 'cdcdcd' }
          }
          cell.border = { top: { style: 'thin' }, left: { style: 'thin' }, bottom: { style: 'thin' }, right: { style: 'thin' } }
          cell.font = { bold: true }
          cell.alignment = { vertical: 'middle', horizontal: 'center', readingOrder: 'ltr' }
        });
      };

      /// SET COLUMN VALUES
      const row = workSheet.addRow(values);
      row.height = this.input.ImageFieldNames ? 80 : 25;;
      row.eachCell((cell, number) => {
        cell.border = { top: { style: 'thin' }, left: { style: 'thin' }, bottom: { style: 'thin' }, right: { style: 'thin' } };
        cell.alignment = { vertical: 'middle', readingOrder: 'ltr' }
        workSheet.getColumn(number).width = 20;
      })
      /// SET IMAGE

      if (this.input.Type === EnumExcelReportType.VisitPlan
        || this.input.Type === EnumExcelReportType.MasterSheet
        || this.input.Type === EnumExcelReportType.NTP
        || this.input.Type === EnumExcelReportType.MPR
        || this.input.Type === EnumExcelReportType.PO
        || this.input.Type === EnumExcelReportType.PRN
        || this.input.Type === EnumExcelReportType.PRN_C
        || this.input.Type === EnumExcelReportType.PRN_F
        || this.input.Type === EnumExcelReportType.PRN_R
        || this.input.Type === EnumExcelReportType.PRN_T
        || this.input.Type === EnumExcelReportType.SRN
        || this.input.Type === EnumExcelReportType.Invoice
        || this.input.Type === EnumExcelReportType.RTP
        || this.input.Type === EnumExcelReportType.UnVerifiedTraineesChangeRequestApproval
      ) {
      }
      else {
        keys.forEach((key, indx) => {
          if (this.input.ImageFieldNames && this.input.ImageFieldNames.length > 0) {
            if (this.input.ImageFieldNames.includes(key)) {
              if (item[key] && item[key] != '') {
                // console.log(item[key])
                const image = workbook.addImage({ base64: item[key], extension: 'jpeg' })
                const cell = row.getCell(indx + 1);
                cell.value = '';
                cell.removeName;
                // workSheet.addImage(image, `${cell.address}:${cell.address}`);
                workSheet.addImage(image, {
                  tl: { col: indx + 0.5, row: row.number - 1 + 0.2 },
                  ext: { width: 80, height: 80 },
                  editAs: 'absolute'
                });
              }
            }
          }
        });
      }



      /// SET FOOTER
      // if (index === dataForExport.length - 1) {
      //    workSheet.addRow([]);
      //    let footerRow = workSheet.addRow([`This is system generated excel file.`]);
      //    workSheet.mergeCells(`A${footerRow.number}: ${footerRow.getCell(keys.length).address}`)
      // }
    });
    // workSheet.columns.forEach(column => {
    //  column.width = 20;
    // });
    if (this.input.Type === EnumExcelReportType.PRN) {
      const rowValues = [
        'Total'
        , ''
        , ''
        , ''
        , ''
        , ''
        , ''
        , ''
        , ''
        , ''
        , ''
        , ''
        , ''
        , ''
        , dataForExport.map(x => x['Contractual Trainees']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Claimed Trainees']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Enrolled Trainees']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['CNIC Verified']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['CNIC Verified Excesses']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Dropouts Verified']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Expelled Verified']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Pass Verified']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Failed Verified']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Absent Verified']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['CNIC Unverified']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['CNIC Unverified Excesses']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Dropouts Unverified']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Expelled UnVerified']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Pass Unverified']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Failed Unverified']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Absent Unverified']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Dropout (Pass/Fail/Absent)']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Expelled (Pass/Fail/Absent)']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Deduction Since Inception Dropout']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Max Attendance']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Payment Withheld Physical Count']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Deduction Marginal']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Deduction Extra Registered For Exam']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Deduction Failed Trainees']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Deduction Uniform Bag Receiving']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Payment Withheld Since Inception UnV CNIC']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , ''// dataForExport.map(x => x["Penalty TPM Reports"]).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , ''// dataForExport.map(x => x["Penalty Imposed By MnE"]).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Reimbursement UnV Trainees']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Reimbursement Attandance']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Employment Commitment Percentage']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Completed Trainees']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Employment Commitment Trainees']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Employment Reported']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Verified Trainees']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Verified to Commitment']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Expelled Regular Verified For The Month']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Certification Cost Deduction (All Types)']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Payment To Be Released Trainees']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , ''
        , ''
        , ''

      ]
      const row = workSheet.addRow(rowValues);
      row.height = this.input.ImageFieldNames ? 80 : 25;;
      row.eachCell((cell, number) => {
        cell.border = { top: { style: 'thin' }, left: { style: 'thin' }, bottom: { style: 'thin' }, right: { style: 'thin' } };
        cell.alignment = { vertical: 'middle', readingOrder: 'ltr' }
        workSheet.getColumn(number).width = 20;
      })
    }
    else if (this.input.Type === EnumExcelReportType.PRN_C) {
      const rowValues = [
        'Total'
        , ''
        , ''
        , ''
        , ''
        , ''
        , ''
        , ''
        , ''
        , ''
        , ''
        , ''
        , ''
        , ''
        , dataForExport.map(x => x['Contractual Trainees']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Claimed Trainees']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Enrolled Trainees']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['CNIC Verified']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['CNIC Verified Excesses']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Dropouts Verified']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Expelled Verified']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Pass Verified']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Failed Verified']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Absent Verified']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['CNIC Unverified']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['CNIC UnVerified Excesses']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Dropouts Unverified']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Expelled UnVerified']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Pass Unverified']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Failed Unverified']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Absent Unverified']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Dropout (Pass/Fail/Absent)']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Expelled (Pass/Fail/Absent)']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Deduction Since Inception Dropout']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Max Attendance']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Payment Withheld Physical Count']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Deduction Marginal']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Deduction Extra Registered For Exam']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Deduction Failed Trainees']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Deduction Uniform Bag Receiving']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Payment Withheld Since Inception UnV CNIC']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Penalty TPM Reports']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Penalty Imposed By MnE']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Reimbursement UnV Trainees']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Reimbursement Attandance']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Certification Cost Deduction (All Types)']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Payment To Be Released Trainees']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , ''
        , ''
        , ''
      ]
      const row = workSheet.addRow(rowValues);
      row.height = this.input.ImageFieldNames ? 80 : 25;;
      row.eachCell((cell, number) => {
        cell.border = { top: { style: 'thin' }, left: { style: 'thin' }, bottom: { style: 'thin' }, right: { style: 'thin' } };
        cell.alignment = { vertical: 'middle', readingOrder: 'ltr' }
        workSheet.getColumn(number).width = 20;
      })
    }
    else if (this.input.Type === EnumExcelReportType.PRN_F) {
      const rowValues = [
        'Total'
        , ''
        , ''
        , ''
        , ''
        , ''
        , ''
        , ''
        , ''
        , ''
        , ''
        , ''
        , ''
        , ''
        , dataForExport.map(x => x['Contractual Trainees']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Claimed Trainees']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Enrolled Trainees']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['CNIC Verified']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['CNIC Verified Excesses']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Dropouts Verified']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Expelled Verified']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Pass Verified']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Failed Verified']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Absent Verified']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['CNIC Unverified']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['CNIC UnVerified Excesses']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Dropouts Unverified']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Expelled UnVerified']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Pass Unverified']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Failed Unverified']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Absent Unverified']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Dropout (Pass/Fail/Absent)']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Expelled (Pass/Fail/Absent)']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Deduction Since Inception Dropout']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Max Attendance']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Payment Withheld Physical Count']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Deduction Marginal']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Deduction Extra Registered For Exam']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Deduction Failed Trainees']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Deduction Uniform Bag Receiving']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Payment Withheld Since Inception UnV CNIC']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Penalty TPM Reports']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Penalty Imposed By MnE']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Reimbursement UnV Trainees']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Reimbursement Attandance']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Employment Commitment Percentage']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Completed Trainees']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Employment Commitment Trainees']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Employment Reported']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Verified Trainees']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Verified to Commitment']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Payment To Be Released Trainees']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , ''
        , ''
        , ''
      ]
      const row = workSheet.addRow(rowValues);
      row.height = this.input.ImageFieldNames ? 80 : 25;;
      row.eachCell((cell, number) => {
        cell.border = { top: { style: 'thin' }, left: { style: 'thin' }, bottom: { style: 'thin' }, right: { style: 'thin' } };
        cell.alignment = { vertical: 'middle', readingOrder: 'ltr' }
        workSheet.getColumn(number).width = 20;
      })
    }
    else if (this.input.Type === EnumExcelReportType.PRN_R) {
      const rowValues = [
        'Total'
        , ''
        , ''
        , ''
        , ''
        , ''
        , ''
        , ''
        , ''
        , ''
        , ''
        , ''
        , ''
        , ''
        , dataForExport.map(x => x['Contractual Trainees']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Claimed Trainees']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Enrolled Trainees']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['CNIC Verified']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['CNIC Verified Excesses']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Dropouts Verified']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Expelled Verified']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['CNIC Unverified']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['CNIC Unverified Excesses']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Dropouts Unverified']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Expelled UnVerified']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , ''// dataForExport.map(x => x["NonFunctional Visit 1"]).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , ''// dataForExport.map(x => x["NonFunctional Visit 2"]).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , ''// dataForExport.map(x => x["NonFunctional Visit 3"]).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , ''// dataForExport.map(x => x["NonFunctional Visit 1 Date"]).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , ''// dataForExport.map(x => x["NonFunctional Visit 2 Date"]).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , ''// dataForExport.map(x => x["NonFunctional Visit 3 Date"]).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Deduction Since Inception Dropout']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Max Attendance']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Payment Withheld Physical Count']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Deduction Marginal']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Payment Withheld Since Inception UnV CNIC']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , ''// dataForExport.map(x => x["Penalty TPM Reports"]).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , ''// dataForExport.map(x => x["Penalty Imposed By MnE"]).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Reimbursement UnV Trainees']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Reimbursement Attandance']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Expelled Regular Verified For The Month']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Payment To Be Released Trainees']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , ''
        , ''
        , ''
      ]
      const row = workSheet.addRow(rowValues);
      row.height = this.input.ImageFieldNames ? 80 : 25;;
      row.eachCell((cell, number) => {
        cell.border = { top: { style: 'thin' }, left: { style: 'thin' }, bottom: { style: 'thin' }, right: { style: 'thin' } };
        cell.alignment = { vertical: 'middle', readingOrder: 'ltr' }
        workSheet.getColumn(number).width = 20;
      })
    }
    else if (this.input.Type === EnumExcelReportType.PRN_T) {
      const rowValues = [
        'Total'
        , // dataForExport.map(x => x['Class Code']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , // dataForExport.map(x => x['Class Duration']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , // dataForExport.map(x => x['Trade Name']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , // dataForExport.map(x => x['Start Date']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , // dataForExport.map(x => x['End Date']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Contractual Trainees']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Enrolled Trainees']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Trainees Registered for Exam']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x.Pass).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x.Fail).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x.Absent).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Payment Released']).reduce((a, b) => parseFloat(a) + parseFloat(b))
      ]
      const row = workSheet.addRow(rowValues);
      row.height = this.input.ImageFieldNames ? 80 : 25;;
      row.eachCell((cell, number) => {
        cell.border = { top: { style: 'thin' }, left: { style: 'thin' }, bottom: { style: 'thin' }, right: { style: 'thin' } };
        cell.alignment = { vertical: 'middle', readingOrder: 'ltr' }
        workSheet.getColumn(number).width = 20;
      })
    }
    else if (this.input.Type === EnumExcelReportType.PendingClassesinKAMDashboard) {

      const rowValues = [
        , ''
        , ''
        , ''
        , dataForExport.map(x => x['TSP Name']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Class Code']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Class Status']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Class Start Date']).reduce((a, b) => parseFloat(a) + parseFloat(b))
        , dataForExport.map(x => x['Class End Date']).reduce((a, b) => parseFloat(a) + parseFloat(b))
      ]
      const row = workSheet.addRow(rowValues);
      row.height = this.input.ImageFieldNames ? 80 : 25;;
      row.eachCell((cell, number) => {
        cell.border = { top: { style: 'thin' }, left: { style: 'thin' }, bottom: { style: 'thin' }, right: { style: 'thin' } };
        cell.alignment = { vertical: 'middle', readingOrder: 'ltr' }
        workSheet.getColumn(number).width = 20;
      })
    }
    workbook.xlsx.writeBuffer().then((data) => {
      const blob = new Blob([data], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
      fs.saveAs(blob, `${fileName}.${ExportType.XLSX}`);
      this.onNoClick();
    }).catch(error => {
      console.error(error);
      this.onNoClick();
    });

  }
  onNoClick(): void {
    this.dialogRef.close(false);
  }
  getControlName(c: AbstractControl): string | null {
    const formGroup = c.parent.controls;
    const index = Number(Object.keys(formGroup).find(name => c === formGroup[name]));
    return this.columnList.slice(index, index + 1).toString();
  }

  toggleAll(value) {
    this.checkAll = value;
    this.columns.controls.forEach(x => x.setValue(value));
  }
  // toggleSingle(value) {
  //    this.checkAll = this.columns.controls.filter(x => !x.value).map(x => this.getControlName(x)).length == 0;
  // }
}
