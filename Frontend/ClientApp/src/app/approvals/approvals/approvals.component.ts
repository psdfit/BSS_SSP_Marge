import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { Workbook } from 'exceljs';
import * as fs from 'file-saver';
import { CommonSrvService } from '../../common-srv.service';
import { UserRightsModel } from '../../master-data/users/users.component';
import { DialogueService } from '../../shared/dialogue.service';
import { EnumApprovalProcess, EnumProgramCategory, AppendixImportSheetNames, EnumAppForms } from '../../shared/Enumerations';
import { DatePipe } from '@angular/common';
import { environment } from '../../../environments/environment';

@Component({
  selector: 'app-approvals',
  templateUrl: './approvals.component.html',
  styleUrls: ['./approvals.component.scss'],
  providers: [DatePipe]
})
export class ApprovalsComponent implements OnInit {
  displayedColumnsScheme = ['SchemeName', 'SchemeCode', 'Description', 'CreatedDate', 'UserName', 'PTypeName', 'PCategoryName', 'FundingSourceName', 'FundingCategoryName', 'PaymentSchedule', 'Stipend', 'StipendMode', 'UniformAndBag', 'MinimumEducation', 'MaximumEducation', 'MinAge', 'MaxAge', 'GenderName', 'DualEnrollment', 'ContractAwardDate', 'BusinessRuleType', 'OName', 'Action'];
  displayedColumnsTSPs = ['TSPName', 'TSPCode', 'Address', 'TSPColor', 'Tier', 'NTN', 'PNTN', 'GST', 'FTN', 'DistrictName', 'HeadName', 'HeadDesignation', 'HeadEmail', 'HeadLandline', 'OrgLandline', 'CPName', 'CPDesignation', 'CPLandline', 'CPEmail', 'Website', 'CPAdmissionsName', 'CPAdmissionsDesignation', 'CPAdmissionsLandline', 'CPAdmissionsEmail', 'CPAccountsName', 'CPAccountsDesignation', 'CPAccountsLandline', 'CPAccountsEmail', 'BankName', 'BankAccountNumber', 'AccountTitle', 'BankBranch', 'Organization', 'Action'];
  // schemes: MatTableDataSource<any>;
  environment = environment;
  schemes: [];
  tsps: [];
  classes: [];
  instructors: any[];
  ActiveFormApprovalID: number;
  ChosenSchemeID: number;
  title: string;
  savebtn: string;
  formrights: UserRightsModel;
  EnText = '';
  error: string;
  query = {
    order: 'SchemeID',
    limit: 5,
    page: 1
  };
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  working: boolean;
  isFinancialUser: boolean = false;

  constructor(private http: CommonSrvService, private dialogue: DialogueService, private datePipe: DatePipe) {
    // this.schemes = new MatTableDataSource([]);
    this.formrights = http.getFormRights();
  }

  ngOnInit(): void {
    this.http.setTitle('Appendix');
    this.title = '';
    this.savebtn = 'Approve';
    this.http.OID.subscribe(OID => {
      this.schemes = [];
      this.tsps = [];
      this.classes = [];
      this.instructors = [];
      this.GetSubmittedSchemesForMyID();
    })
  }

  GetSubmittedSchemesForMyID() {
    this.http.getJSON(`api/Scheme/GetSubmittedSchemes?OID=${this.http.OID.value}`).subscribe((d: any) => {
      this.schemes = d;
      // this.schemes.paginator = this.paginator;
      // this.schemes.sort = this.sort;
    },
      error => this.error = error // error path
      , () => {
        this.working = false;
      });
  }
  GetTsps(SchemeID) {
    this.ChosenSchemeID = SchemeID;

    this.http.getJSON('api/TSPDetail/GetTSPBySchemeID/' + this.ChosenSchemeID).subscribe((d: any) => {
      this.tsps = d;
      // this.schemes.paginator = this.paginator;
      // this.schemes.sort = this.sort;
    },
      error => this.error = error // error path
      , () => {
        this.working = false;
      });
  }
  GetClasses() {
    this.http.getJSON('api/Class/GetClassesBySchemeID/' + this.ChosenSchemeID).subscribe((d: any) => {
      // debugger;
      this.classes = d;
    },
      error => this.error = error // error path
      , () => {
        this.working = false;
      });
  }
  GetInstructors() {
    this.http.getJSON('api/Instructor/GetInstructorsBySchemeID/' + this.ChosenSchemeID).subscribe((d: any) => {
      this.instructors = d;
    },
      error => this.error = error // error path
      , () => {
        this.working = false;
      });
  }
  exportExcel(schemeId, isClickedFinancial) {
    if (isClickedFinancial) {
      var userID = this.formrights.UserID;
      var formID = EnumAppForms.AppendixWithFinancialInformation;
      this.http.getJSON(`api/Users/CheckFinanceUserRights/${userID}/${formID}`).subscribe((d: any) => {
        var userFinancialRights = d[0];
        if (d[0].length>0) {
          if (userFinancialRights[0].CanView) {
            this.ExportAppendix(schemeId, true);
          }
          else {
            this.error = "You do not have rights to download this appendix";
            this.http.ShowError(this.error.toString(), "Error");
            return false;
          }
        }
        else {
          this.error = "You do not have rights to download this appendix";
          this.http.ShowError(this.error.toString(), "Error");
          return false;
        }
      },
        error => this.error = error // error path
        , () => {
          this.working = false;
        });
    } else {
      this.ExportAppendix(schemeId, false);
    }
  }
  checkUserFinance(id) {
    var userID = this.formrights.UserID;
    var formID = 1147;
    this.http.getJSON(`api/Users/CheckFinanceUserRights/${userID}/${formID}`).subscribe((d: any) => {
      if (d.CanView) {
        this.isFinancialUser = true;
        this.ExportAppendix(id, d.CanView);
      }
      else {
        this.isFinancialUser = false;
        this.error = "You do not have rights to download this appendix";
        this.http.ShowError(this.error.toString(), "Error");
        return false;
      }

    },
      error => this.error = error // error path
      , () => {
        this.working = false;
      });
  }

  ExportToExcel(row) {
    this.http.postJSON('api/Approval/GetFactSheet', { SchemeID: row.SchemeID }).subscribe((d: any) => {

      const data = d[0];
      // const ws: XLSX.WorkSheet = XLSX.utils.json_to_sheet(d);
      const wb = new Workbook();

      const ws = wb.addWorksheet('Fact Sheet');
      const font = {
        name: 'Calibri',
        family: 4,
        size: 11,
        // underline: true,
        bold: true
      };

      const Keys = ['', ''];
      ws.properties.defaultRowHeight = 18;
      Keys.concat(Object.keys(data));
      const Col4 = ws.getColumn(4);
      const Col5 = ws.getColumn(5);
      const Col3 = ws.getColumn(3);
      Col3.font = font;
      const RowH = ws.getRow(3);
      Col4.width = 40;
      Col5.width = 20;
      Col3.width = 28;
      Col3.alignment = { vertical: 'middle', horizontal: 'center', wrapText: true };
      Col4.values = Object.keys(data);
      Col5.values = Object.values(data);
      ws.insertRow(1, '', ''); // Row 1
      ws.mergeCells('C2:E2');
      const title = ws.getCell('C2');
      title.alignment = { vertical: 'middle', horizontal: 'center' };
      title.value = data['SchemeName'];// Row 2
      RowH.fill = {
        type: 'pattern',
        pattern: 'solid',
        fgColor: { argb: 'FFFFFF00' },
        bgColor: { argb: '80C0C0C0' }
      };
      title.fill = {
        type: 'pattern',
        pattern: 'solid',
        fgColor: { argb: 'FFFFFF00' },
        bgColor: { argb: 'FF0000FF' }
      };
      ws.insertRow(3, ['', '', 'Type', 'Indicator', 'Value']);// Row 3
      ws.mergeCells('C4:C11');
      ws.getCell('C4').value = 'Target Trainee';
      ws.mergeCells('C12:C16');
      ws.getCell('C13').value = 'Training Provider (TP) Participation';
      ws.mergeCells('C17:C24');
      ws.getCell('C17').value = 'CTM';
      ws.getCell('C25').value = 'Course Duration';
      ws.getCell('C26').value = 'Procurement Time ';
      ws.mergeCells('C27:C35');
      ws.getCell('C27').value = 'Cost';
      ws.mergeCells('C36:C41');
      ws.getCell('C36').value = 'Largest TPs';
      ws.getCell('C42').value = 'Trade';
      ws.mergeCells('C43:C44');
      ws.getCell('C43').value = 'Women ';
      ws.mergeCells('C45:C47');
      ws.getCell('C45').value = 'Placement ';

      this.WriteAndDownloadFile(wb, 'FactSheet');
    },
      error => this.error = error // error path
      , () => {
        this.working = false;
      });

  }
  openApprovalDialogue(row: any): void {
    let processKey = EnumApprovalProcess.AP_PD;
    if (row.PCategoryID == EnumProgramCategory.BusinessDevelopmentAndPartnershipsFTI
      || row.PCategoryID == EnumProgramCategory.BusinessDevelopmentAndPartnershipsCommunity
      || row.PCategoryID == EnumProgramCategory.BusinessDevelopmentAndPartnershipsIndustry) {
      processKey = EnumApprovalProcess.AP_BD;
    }
    this.dialogue.openApprovalDialogue(processKey, row.SchemeID).subscribe(result => {
      console.log(result);
      // location.reload();
    });
  }

  ExportAppendix(SchemeID: any, isAllowFinancial: boolean) {
    let scheme: any;
    let tsps: any;
    let classes: any;
    let instructors: any;

    this.http.postJSON('api/Appendix/GetAppendix', SchemeID).subscribe((d: any) => {
      scheme = d[0];
      tsps = d[1];
      classes = d[2];
      instructors = d[3];
      // console.log(scheme)
      // console.log(tsps)
      // console.log(classes)
      // console.log(instructors)
      scheme = {
        'Scheme Code': scheme.SchemeCode
        , 'Scheme Name': scheme.SchemeName
        , 'Payment Schedule': scheme.PaymentSchedule
        , Description: scheme.Description
        // , "Scheme Duraton": x.SchemeCode
        , 'Business Rules': scheme.BusinessRuleType
        , 'Funding Source': scheme.FundingSourceName
        , 'Funding Category': scheme.FundingCategoryName
        , 'Program Category': scheme.PCategoryName
        , Stipend: scheme.Stipend
        , 'Stipend Mode': scheme.StipendMode
        , 'Uniform and Bag': scheme.UniformAndBag
        , 'Minimum Education': scheme.MinimumEducationName
        , 'Maximum Education': scheme.MaximumEducationName
        , 'Minimum Age(Years)': scheme.MinAge
        , 'Maximum Age': scheme.MaxAge
        , Gender: scheme.GenderName
        , 'Program Type': scheme.PTypeName
      };
      tsps = tsps.map(x => {
        return {
          'TSP Name': x.TSPName
          , 'TSP Code': x.TSPCode
          , 'Organization	': x.OrganizationName
          // ,"Type": x.name
          , 'TSP Color': x.TSPColor
          , Tier: x.TierID
          // ,"Type": x.name
          , PNTN: x.PNTN
          , GST: x.GST
          , 'Address District': x.DistrictName
          , 'Head of Organization': x.HeadName
          , 'Designation of Head of Organization': x.HeadDesignation
          , 'Email of Head of Organization': x.HeadEmail
          , FTN: x.FTN
          , NTN: x.NTN
          , 'Mobile of Head of Organization': x.HeadLandline
          , 'Landline Organization': x.HeadLandline
          , Website: x.Website
          , 'Name of Contact Person': x.CPName
          , 'Designation of Contact Person': x.CPDesignation
          , 'Mobile / Landline of Contact Person': x.CPLandline
          , 'Email of Contact Person': x.CPEmail
          , 'Name of Contact Person Admissions': x.CPAdmissionsName
          , 'Designation of Contact Person Admissions': x.CPAdmissionsDesignation
          , 'Mobile / Landline of Contact Person Admissions': x.CPAdmissionsLandline
          , 'Email of Contact Person Admissions': x.CPAdmissionsEmail
          , 'Training District': x.Address
          , 'Name of Contact Person Accounts': x.CPAccountsName
          , 'Designation of Contact Person Accounts': x.CPAccountsDesignation
          , 'Mobile / Landline of Contact Person Accounts': x.CPAccountsLandline
          , 'Email of Contact Person Accounts': x.CPAccountsEmail
          , 'Bank Name': x.BankName
          , 'Bank Account / IBAN': x.BankAccountNumber
          , 'Account Title': x.AccountTitle
          , 'Bank Branch': x.BankBranch
        }
      });


      classes = classes.map(x => {
        var obj = {
          'TSP Name': x.TSPName
          , Sector: x.SectorName
          , 'Trade Name': x.TradeName
          , 'Class Code': x.ClassCode
          , 'Duration in Months': x.Duration
          , 'Source of Curriculum': x.SourceOfCurriculum
          , 'Entry Qualification': x.EntryQualificationName
          , 'Certification Authority': x.CertAuthName
          , 'Registration Authority': x.RegistrationAuthorityName
          , 'Program Focus': x.ProgramFocusName
          // ,"Attendance Standard Percentage": x.
          // ,"Total Trainees": x.
          , 'Trainees per Class': x.TraineesPerClass
          // , "Number of Batches": x.Batch
          // ,"Number of Classes": x.
          , 'Minimum Training Hours Per Month': x.MinHoursPerMonth
          , 'Start Date': this.datePipe.transform(x.StartDate, 'dd-MM-yyyy')
          , 'End Date': this.datePipe.transform(x.EndDate, 'dd-MM-yyyy')
          , 'Trainees Gender': x.GenderName
          , 'Address of Training Location': x.TrainingAddressLocation
          , 'Geo Tagging': x.Latitude != '' ? `${x.Latitude},${x.Longitude}` : ''
          , Province: x.ProvinceName
          , District: x.DistrictName
          , Tehsil: x.TehsilName
          , Cluster: x.ClusterName
          , 'Total Trainee Bid Price': x.OfferedPrice
          , 'Total Trainee BM Price': x.BMPrice
          // ,"Total Trainee Cost	Sales Tax Rate": x.
          // ,"Training Cost per Trainee per Month(Exclusive of Taxes)": x.
          // ,"Sales Tax	Training Cost per Trainee per Month(Inclusive  of Taxes)": x.
          , 'Uniform & Bag Cost per Trainee': x.UniformBagCost
          , 'Testing & Certification Fee per Trainee': x.PerTraineeTestCertCost
          , 'Boarding & Other Allowances per trainee': x.BoardingAllowancePerTrainee
          // ,"Employment Commitment Self	Employment Commitment Formal": x.
          // ,"Overall Employment Commitment": x.
          , Stipend: x.Stipend
          , 'Baloon Payment': x.balloonpayment
          // ,"Total Cost": x.
          , 'Training Cost Per Trainee Per Month Ex Tax': x.TrainingCostPerTraineePerMonthExTax
          , 'Training Cost Per Trainee Per Month In Tax': x.TrainingCostPerTraineePerMonthInTax
          , 'Total Cost Per Class': x.TotalCostPerClass
          , 'Total Cost Per Class In Tax': x.TotalCostPerClassInTax
          , 'Total Per Trainee Cost In Tax': x.TotalPerTraineeCostInTax
          // , "Total Testing Certification Of Class": x.TotalTestingCertificationOfClass
          , SalesTax: x.SalesTax
          , 'Sales Tax Rate': x.SalesTaxRate
          , 'BM Price': x.BMPrice
          , 'Bid Price': x.BidPrice
          , 'Boarding Allowance Per Trainee': x.BoardingAllowancePerTrainee

        }

        if (!isAllowFinancial) {
          delete obj['Training Cost Per Trainee Per Month Ex Tax'];
          delete obj['Training Cost Per Trainee Per Month In Tax'];
          delete obj['Total Cost Per Class'];
          delete obj['Total Cost Per Class In Tax'];
          delete obj['Total Per Trainee Cost In Tax'];
          delete obj['SalesTax'];
          delete obj['Sales Tax Rate'];
          delete obj['BM Price'];
          delete obj['Bid Price'];
          delete obj['Boarding Allowance Per Trainee'];
        }

        return obj;
      });


      instructors = instructors.map(x => {
        return {
          'Name of Organization': x.NameOfOrganization
          , 'Instructor Name': x.InstructorName
          , Gender: x.GenderName
          // , "Profile Picture": x.
          , 'Total Experience': x.TotalExperience
          , 'Class Code': x.ClassCode
          , 'Qualification Highest': x.QualificationHighest
          , 'CNIC of Instructor': x.CNICofInstructor
          // , "CNIC Issue Date": x.
          , Trade: x.TradeName
          , 'Address of Training Location': x.LocationAddress

        }
      });



      const wb = new Workbook();

      const ws_scheme = wb.addWorksheet(AppendixImportSheetNames.Scheme);
      const ws_tsps = wb.addWorksheet(AppendixImportSheetNames.TSP);
      const ws_classes = wb.addWorksheet(AppendixImportSheetNames.Class);
      const ws_instructors = wb.addWorksheet(AppendixImportSheetNames.Instructor);

      ws_scheme.addRow(Object.keys(scheme));
      ws_scheme.addRow(Object.values(scheme));

      ws_tsps.addRow(Object.keys(tsps[0]));
      tsps.forEach(row => {
        ws_tsps.addRow(Object.values(row));
      });

      ws_classes.addRow(Object.keys(classes[0]));
      classes.forEach(row => {
        ws_classes.addRow(Object.values(row));
      });

      ws_instructors.addRow(Object.keys(instructors[0]));
      instructors.forEach(row => {
        ws_instructors.addRow(Object.values(row));
      });

      this.WriteAndDownloadFile(wb, 'Appendix');
    });
  }

  WriteAndDownloadFile(wb: Workbook, name: string) {
    wb.xlsx.writeBuffer().then((data) => {
      const blob = new Blob([data], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
      fs.saveAs(blob, name + '.xlsx');

    }).catch(error => {
      console.error(error);
    });
  }

  // OK() { //this method is just for testing invoices generation, pls ignore this
  //    this.http.getJSON('api/Scheme/GenerateInvoice').subscribe((d: any) => {
  //        this.classes = d;
  //    });
  // }
}

