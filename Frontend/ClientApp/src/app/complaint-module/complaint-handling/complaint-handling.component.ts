import { Component, OnInit, ViewChild } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { CommonSrvService } from 'src/app/common-srv.service';
import { UserRightsModel, UsersModel } from 'src/app/master-data/users/users.component';
import { DialogueService } from 'src/app/shared/dialogue.service';

@Component({
  selector: 'app-complaint-handling',
  templateUrl: './complaint-handling.component.html',
  styleUrls: ['./complaint-handling.component.scss']
})
export class ComplaintHandlingComponent implements OnInit {
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  Complaint: MatTableDataSource<any>;
  ComplaintTSP: MatTableDataSource<any>;
  Complaintform: FormGroup;
  formrights: UserRightsModel;
  EnText: string = "Complaint";
  error: String;
  ComplaintStatusFilter = new FormControl(0);
  filters: IQueryFilters = { ComplaintStatusTypeID: 0};
/*   TSPTAB: boolean = false ;
  SystemTAB: boolean = false ; */
  currentUser: UsersModel;
  ComplaintTypeIDFilter = new FormControl(0);
  displayedColumns = ["History",'Attachedfile','ComplaintNo','ComplaintStatus','ComplaintDescription','ComplaintTypeName','ComplaintSubTypeName','TraineeName','TraineeCNIC','TraineeCode','TSPName','TSPCode'];
  ComplaintTypeddl = [];
  ComplaintTypeFirCURD = [];
  ComplaintSubTypedll = [];
  ComplaintStatusTypedll = [];
  hiddenComplainttypeID: number;
  selectedFile: string;
  ext: string;
  base64 = [];
  constructor(private http: CommonSrvService,  public dialogueService: DialogueService) {
    this.formrights = http.getFormRights();
   }

  ngOnInit(): void {
    this.currentUser = this.http.getUserDetails();
    this.filters.ComplaintStatusTypeID = 0;
    this.getComplianStatus();
    /* if(this.currentUser.UserLevel==4)
{
  this.TSPTAB = true ;
}
else{this.SystemTAB = true ;} */
    this.http.setTitle("Complaint Handling");
    this.GetComplaintTypeForCRUD();
    this.FetchComplaintForGridView();

  }
  GetComplaintTypeForCRUD() {
    debugger;
    this.http.getJSON('api/Complaint/GetComplaintTypeForCRUD').subscribe((d: any) => {
      debugger;
      this.ComplaintTypeddl = d;
    }, error => this.error = error // error path
    );
  }
  FetchComplaintForGridView() {
    debugger;
    this.http.getJSON('api/Complaint/FetchComplaintForGridView').subscribe((d: any) => {
      debugger;
      this.populateComplaintList(d[0]);
     // this.populateComplaintOfTSP(d[1]);
    }, error => this.error = error // error path
    );
  }
  applyFilter(filterValue: string) {
    filterValue = filterValue.trim(); // Remove whitespace
    filterValue = filterValue.toLowerCase(); // Datasource defaults to lowercase matches
    this.Complaint.filter = filterValue;
    this.ComplaintTSP.filter = filterValue;
}
populateComplaintList(d: any) {
  this.Complaint = new MatTableDataSource(d);
  this.Complaint.paginator = this.paginator;
  this.Complaint.sort = this.sort;
}
populateComplaintOfTSP(d: any) {
  this.ComplaintTSP = new MatTableDataSource(d);
  this.ComplaintTSP.paginator = this.paginator;
  this.ComplaintTSP.sort = this.sort;
}
ComplaintStatusChange(ComplaintStatusTypeID: any,ComplainantID:any) {
  debugger;
     this.http.confirmComplaintStatus().subscribe(result => {
      if (result) {
          this.http.postJSON('api/Complaint/complaintStatusChange', { 'ComplaintStatusTypeID': ComplaintStatusTypeID, 'ComplainantID': ComplainantID })
              .subscribe((d: any) => {
              },
                  error => this.error = error // error path
              );
      }
      else {
        return;
      }
  });
}
GetCurrentComplaintAttachements(r) {
  debugger;
  this.http.postJSON('api/Complaint/ComplaintAttachments', { "ComplainantID": r.ComplainantID }).subscribe((d: any) => {
    debugger;
    console.log(d[0].FilePath);
    if (d[0].map(x=>x.FilePath) == "") {
      this.error = "File not found against this record";
      this.http.ShowError(this.error.toString(), "Error");
      return;
    }
    debugger;
    this.base64=(d[0])
    this.base64ToBlob(this.base64[0].FilePath,r.ComplaintNo);
  });
}
getComplianStatus() {
  this.http.getJSON("api/Complaint/getComplianStatus/").subscribe(
    (data: any) => {
      debugger;
      this.ComplaintStatusTypedll = data[0];
    },
    error => {
      this.http.ShowError(error, "Error");
    }
  );
}
public base64ToBlob(b64Data, ComplaintNo) 
{
      var elem = window.document.createElement('a');
      elem.href =b64Data
      elem.download = "ComplaintNo_"+ComplaintNo+".zip";       
      document.body.appendChild(elem);
      elem.click();        
      document.body.removeChild(elem);
}
FilterByComplaintStatusType(ComplaintStatusTypeID: number) {
  debugger;
    this.http.getJSON('api/Complaint/FetchComplaintForGridView?ComplaintStatusTypeID='+ComplaintStatusTypeID).subscribe((d: any) => {
      debugger;
      this.populateComplaintList(d[0]);
     // this.populateComplaintOfTSP(d[1]);
    }, error => this.error = error // error path
    );
}
openHistoryDialogue(data: any): void {
  debugger;
  this.dialogueService.openComplaintHistoryDialogue(data);
}
}

export interface IQueryFilters {
  ComplaintStatusTypeID: number;
}
