import { Component, ElementRef, Inject, OnInit, ViewChild } from '@angular/core';
import { CommonSrvService } from 'src/app/common-srv.service';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ModelBase } from 'src/app/shared/ModelBase';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatTableDataSource } from '@angular/material/table';
import { UserRightsModel, UsersModel } from 'src/app/master-data/users/users.component';
import { environment } from 'src/environments/environment';
import { MatTabGroup } from '@angular/material/tabs';
@Component({
  selector: 'app-complaint-history-dialogue',
  templateUrl: './complaint-history-dialogue.component.html',
  styleUrls: ['./complaint-history-dialogue.component.scss']
})
export class ComplaintHistoryDialogueComponent implements OnInit {
  @ViewChild('myInput') myInputVariable: ElementRef;
  @ViewChild("tabGroup") tabGroup: MatTabGroup;
  Complainthistory: any[]=[];
  Complaint: MatTableDataSource<any>;
  ComplaintformDialogue: FormGroup;
  formrights: UserRightsModel;
  EnText: string = "Complaint History";
  EnTextStatus: string = "Complaint Status";
  error: String;
  ComplaintDetail:any;
  ComplaintStatusTypedll = [];
  hiddenComplainttypeID: number;
  //Count:number=0;
  //disable:string=
  Show: boolean = true ;
  ShowHideButtonDiv: boolean = true ;
  ShowHideMsgDiv:boolean=false;
  disabledFields:string="false";
  readonly:string="false";
  selectedFile: string;
  ext: string;
  currentUser: UsersModel;
  base64 = [];
  lastIndex:number;
  title: string; savebtn: string;
  constructor(private fb: FormBuilder,private http: CommonSrvService, public dialogRef: MatDialogRef<ComplaintHistoryDialogueComponent>,
  @Inject(MAT_DIALOG_DATA) public data: any) { 
    this.ComplaintformDialogue = this.fb.group({
      ComplaintStatusDetailID: 0,
      complaintStatusDetailComments: ["", Validators.required],
      ComplaintStatusTypeID: ["", Validators.required],
    })
    this.formrights = http.getFormRights();
  }
  ngOnInit(): void {
    debugger;//UserID
    this.currentUser = this.http.getUserDetails();
    this.ComplaintDetail=this.data.data;
    this.getHistory();
    this.getComplianStatus();
    this.title = "Change ";
    this.savebtn = "Save ";
  }
  Submit() {
    debugger;
    if(this.selectedFile!=null)//base64
      this.ComplaintformDialogue.value.base64 = this.selectedFile;
      this.ComplaintformDialogue.value.ext = this.ext;
    this.ComplaintformDialogue.value.ComplainantID = this.data.data.ComplainantID;
    if (!this.ComplaintformDialogue.valid)
      return;
    this.http.postJSON('api/Complaint/complaintStatusChange', this.ComplaintformDialogue.value).subscribe((d: any) => {
       this.http.openSnackBar(this.ComplaintStatusDetailID.value > 0 ? environment.UpdateMSG.replace("${Name}", this.EnText) : environment.SaveMSG.replace("${Name}", this.EnText));
      
      this.reset();
      this.ComplaintStatusTypeID.setValue(0);
      this.getHistory();
    },
    (error) => {
      this.error = error.error;
      this.http.ShowError(error.error);
      
    });
  }

  GetComplaintStatusAttachements(r) {
    debugger;
    this.http.postJSON('api/Complaint/ComplaintStatusAttachments', { "ComplaintStatusDetailID": r.ComplaintStatusDetailID }).subscribe((d: any) => {
      debugger;
      console.log(d[0].FilePath);
      if (d[0].map(x=>x.FilePath) == "") {
        this.error = "File not found against this record";
        this.http.ShowError(this.error.toString(), "Error");
        return;
      }
      debugger;
      this.base64=(d[0])
      this.base64ToBlobStatus(this.base64[0].FilePath);
    });
  }
  public base64ToBlobStatus(b64Data ) 
  {
        var elem = window.document.createElement('a');
        elem.href =b64Data
        elem.download = "ComplaintStatus.zip";        
        document.body.appendChild(elem);
        elem.click();        
        document.body.removeChild(elem);
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
  public base64ToBlob(b64Data,ComplaintNo ) 
{
      var elem = window.document.createElement('a');
      elem.href =b64Data
      elem.download = "ComplaintNo_"+ComplaintNo+".zip";        
      document.body.appendChild(elem);
      elem.click();        
      document.body.removeChild(elem);
}
//binary conversion start..
handleFileSelect(evt:any){
  debugger;
  var files = evt.target.files;
  var file = files[0];
  var ext =  file.name.split('.').pop();
  if(ext=="zip" || ext=="rar") {
    this.ext =  ext;
    if (files && file) {
    var reader = new FileReader();
    reader.onload =this._handleReaderLoaded.bind(this);
    reader.readAsBinaryString(file);
    }
  }
  else{
    this.error = "Only zip File Upload";
    this.http.ShowError(this.error.toString(), "Error");
    this.myInputVariable.nativeElement.value = "";
    return;
  }
  }
  _handleReaderLoaded(readerEvt:any) {
    debugger;
  var binaryString = readerEvt.target.result;
  this.selectedFile= btoa(binaryString);
  }
  getHistory() {
    this.http.getJSON("api/Complaint/GetComplaintHistory/" + this.data.data.ComplainantID).subscribe(
      (data: any) => {
        debugger;
        let last:any = data[data.length-1];
        this.lastIndex=data.length-1;
        if(last.ComplaintStatusType=="Reported Resolved"||last.ComplaintStatusType=="Closed")
        {
          this.disabledFields="true";
          this.readonly="true";
          this.ShowHideButtonDiv=false;
          this.ShowHideMsgDiv=true;
        }
        this.Complainthistory = data;
      },
      error => {
        this.http.ShowError(error, "Error");
      }
    );
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
  reset() {
    // this.ComplaintformDialogue.setValue({complaintStatusDetailComments: ''});
    this.ComplaintformDialogue.reset();
    this.myInputVariable.nativeElement.value = "";
  }
  toggleEdit(r) {
    debugger;
    this.tabGroup.selectedIndex = TabGroup.ComplaintformDialogue;
    this.ComplaintStatusDetailID.setValue(r.ComplaintStatusDetailID);
    this.complaintStatusDetailComments.setValue(r.complaintStatusDetailComments);
    this.ComplaintformDialogue.controls['ComplaintStatusTypeID'].patchValue(r.ComplaintStatusTypeID);   
   
}
  onNoClick(): void {
    this.dialogRef.close(false);
  }
  get ComplainantID() { return this.ComplaintformDialogue.get("ComplainantID"); }
  get ComplaintStatusDetailID() { return this.ComplaintformDialogue.get("ComplaintStatusDetailID"); }
   get complaintStatusDetailComments() { return this.ComplaintformDialogue.get("complaintStatusDetailComments"); }
   get ComplaintStatusTypeID() { return this.ComplaintformDialogue.get("ComplaintStatusTypeID"); }
}
export class ComplaintModel extends ModelBase {

  ComplainantID: number;
  ComplaintStatusDetailID: number;
  complaintStatusDetailComments: string;
  ComplaintStatusTypeID:number;
}
enum TabGroup {
  ComplaintformDialogue = 0,
}
