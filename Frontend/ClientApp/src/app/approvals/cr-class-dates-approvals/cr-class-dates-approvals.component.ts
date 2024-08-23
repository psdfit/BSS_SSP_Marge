import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { CommonSrvService } from '../../common-srv.service';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { UserRightsModel } from '../../master-data/users/users.component';
import { DialogueService } from '../../shared/dialogue.service';
import { EnumApprovalProcess, EnumProgramCategory } from '../../shared/Enumerations';
import { environment } from '../../../environments/environment';

@Component({
  selector: 'app-cr-class-dates-approvals',
  templateUrl: './cr-class-dates-approvals.component.html',
  styleUrls: ['./cr-class-dates-approvals.component.scss']
})
export class ClassDatesChangeRequestApprovalsComponent implements OnInit {
    environment = environment;

    displayedColumnsClass = ['ClassCode',
        'TradeName', 'SourceOfCurriculumName', 'EntryQualification', 'CertAuthName',
        'StartDate', 'EndDate', "Action"];

    classes: any;
    currentClassDates: [];

    ActiveFormApprovalID: number;
    ChosenTradeID: number;
    title: string;
    savebtn: string;
    formrights: UserRightsModel;
    EnText: string = "";
    error: String;
    query = {
        order: 'ClassDatesChangeRequestID',
        limit: 5,
        page: 1
    };
    @ViewChild(MatPaginator) paginator: MatPaginator;
    @ViewChild(MatSort) sort: MatSort;
    working: boolean;

    constructor(private http: CommonSrvService, private dialogue: DialogueService) {
        //this.schemes = new MatTableDataSource([]);
        this.formrights = http.getFormRights();
    }

    ngOnInit(): void {
        this.http.setTitle("Class Dates Change Request");
        this.title = "";
        this.savebtn = "Approve";
      this.GetClassDatesCRs();
    }

    currentPage: number = 1;
    rowsPerPage: number = 5;
    totalPages: number = 1;

    currentData: any[] = [];
    
   // Method to fetch class dates and set up pagination
   GetClassDatesCRs() {
    this.http.getJSON('api/ClassChangeRequest/GetClassDatesChangeRequest').subscribe(
        (response: any) => {
            this.classes = response[0];
            this.totalPages = Math.ceil(this.classes.length / this.rowsPerPage);
            this.currentData = [...this.classes];  // Keep a copy of the full dataset
            this.displayTable();  // Display the first page of data
        },
        (error) => {
            this.error = error;
            this.working = false;  // Ensure the working flag is reset
        },
        () => {
            this.working = false;  // Finalize the process
        }
    );
}

// Method to display the data for the current page
displayTable() {
    const start = (this.currentPage - 1) * this.rowsPerPage;
    const end = start + this.rowsPerPage;
    this.classes = this.currentData.slice(start, end);
}

// Method to move to the next page
nextPage() {
    if (this.currentPage < this.totalPages) {
        this.currentPage++;
        this.displayTable();
    }
}

// Method to move to the previous page
prevPage() {
    if (this.currentPage > 1) {
        this.currentPage--;
        this.displayTable();
    }
}

// Method to change the number of rows per page dynamically
changeRowsPerPage(rows) {
  debugger;
    this.rowsPerPage = parseInt(rows);
    this.totalPages = Math.ceil(this.currentData.length / this.rowsPerPage);
    this.currentPage = 1; // Reset to the first page whenever rows per page changes
    this.displayTable();
}

  GetCurrentClassDatesByID(r) {
    if (r.currentClassDates) {
      r.currentClassDates = null;

      return;
    }
    this.http.postJSON('api/Class/RD_ClassBy/', { ClassID: r.ClassID }).subscribe((d: any) => {
      r.currentClassDates = d;
    });
  }

    openApprovalDialogue(row: any): void {
        let processKey = EnumApprovalProcess.CR_CLASS_DATES;
        
        this.dialogue.openApprovalDialogue(processKey, row.ClassDatesChangeRequestID).subscribe(result => {
            console.log(result);
            //location.reload();
          this.GetClassDatesCRs();
        });
    }
    openClassJourneyDialogue(data: any): void 
    {
      debugger;
      this.dialogue.openClassJourneyDialogue(data);
    }
  

}
