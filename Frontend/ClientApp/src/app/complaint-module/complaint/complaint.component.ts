import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from '../../../environments/environment';
import { FormGroup, FormBuilder, Validators, ReactiveFormsModule, FormControl, ValidatorFn } from '@angular/forms';
import { CommonSrvService } from '../../common-srv.service';
import { ModelBase } from '../../shared/ModelBase';
import { UserRightsModel, UsersModel } from '../../master-data/users/users.component';
import { Observable } from 'rxjs';
import { DialogueService } from 'src/app/shared/dialogue.service';
import { DomSanitizer } from '@angular/platform-browser';
import { MatTabGroup } from '@angular/material/tabs';
@Component({
  selector: 'app-complaint',
  templateUrl: './complaint.component.html',
  styleUrls: ['./complaint.component.scss']
})
export class ComplaintComponent implements OnInit {
  @ViewChild("tabGroup") tabGroup: MatTabGroup;
  @ViewChild('myInput') myInputVariable: ElementRef;
  @ViewChild('resumeInput', {static: true}) resumeInput;  
  title: string; savebtn: string;submittedtitle: string;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  Complaint: MatTableDataSource<any>;
  ComplaintTSP: MatTableDataSource<any>;
  Complaintform: FormGroup;
  formrights: UserRightsModel;
  EnText: string = "";
  error: String;
  ComplaintTypeIDFilter = new FormControl(0);
  displayedColumns = ["Actions",'Attachedfile','ComplaintNo','ComplaintTypeName','ComplaintSubTypeName','ComplaintDescription','ComplaintStatus','TraineeName','TraineeCNIC','TraineeCode','TSPName','TSPCode'];
  ComplaintTypeddl = [];
  Users: any[] = [];
  ComplaintTypeFirCURD = [];
  ComplaintSubTypedll = [];
  ComplaintStatusTypedll = [];
  ComplaintOfTSP = [];
  hiddenComplainttypeID: number;
  selectedFile: string;
  ext: string;
  base64 = [];
  working: boolean;
  submittedBit:string="false";
  disabledFields:string="false";
  readonly:string="false";
  isShown: boolean = true ;
  currentUser: UsersModel;
  Attachment: boolean = false ;
  isShownTraineeProfile: boolean = false ;
  isShownTSPProfile: boolean = false ;
  TraineeCNICHidden: boolean = false ;
/*   TSPTAB: boolean = false ;
  SystemTAB: boolean = false ; */
  TSPCodeHidden: boolean = false ;
  //TraineeCNICSearchField: boolean = false ;
  complaintTypeValueForDll:number;
  constructor(private fb: FormBuilder, private http: CommonSrvService, public dialogueService: DialogueService,
    public domSanitizer: DomSanitizer) {
    this.Complaintform = this.fb.group({
      ComplainantID: 0,
      ComplaintDescription: ["", Validators.required],
      ComplaintTypeID: ["", Validators.required],
      UserID: 0,
      ComplaintSubTypeID: ["", Validators.required],
      TraineeID:[""],
      TraineeCNIC: ['', [ Validators.minLength(15), Validators.maxLength(15)]],
      TraineeName:[""],
      FatherName:[""],
      EducationID:[""],
      ContactNumber1:[""],
      TraineeCode:[""],
      ClassCode:[""],
      TraineeHouseNumber:[""],
      TraineeStreetMohalla:[""],
      TraineeMauzaTown:[""],
      TrainingAddressLocation:[""],
      TSPMasterID:0,
      TSPName:[""],
      TSPCode:[""],
      Address:[""],
      HeadLandline:[""],
      InActive: ""
    }, { updateOn: "blur" });
    this.Complaint = new MatTableDataSource([]);
    this.ComplaintTSP = new MatTableDataSource([]);
    this.formrights = http.getFormRights();
    this.ComplaintTypeIDFilter.valueChanges.subscribe(value => { this.GetComplaintTSubTypeByComplaintType(value); });
  }
  //fileUrl;
  ngOnInit(): void {
    this.http.setTitle("Complaint");
    this.currentUser = this.http.getUserDetails();
    /* if(this.currentUser.UserLevel==4)
{
  this.TSPTAB = true ;
}
else{this.SystemTAB = true ;} */

    this.GetComplaintTypeForCRUD();
    this.FetchComplaintForGridView();
    this.title = "Add New ";
    this.submittedtitle="Save & Submitted";
    this.savebtn = "Save ";
  }
  onCategoryChange() {
    debugger;
  }
  GetTraineeInfoByCNIC(){
    debugger;
    
    this.http.getJSON('api/Complaint/GetTraineeInfoByCNIC?TraineeCNIC='+this.Complaintform.value.TraineeCNIC).subscribe((d: any) => {
      debugger;
      if(d!=null){
        this.isShownTraineeProfile=true;
        this.isShownTSPProfile = false ;
        this.TSPMasterID.setValue(0);
        this.TraineeID.setValue(d.TraineeID);
        this.TraineeName.setValue(d.TraineeName);
        this.FatherName.setValue(d.FatherName);
        this.TraineeCNIC.setValue(d.TraineeCNIC);
        this.TraineeCode.setValue(d.TraineeCode);
        this.ClassCode.setValue(d.ClassCode);
        this.ContactNumber1.setValue(d.ContactNumber1);
        this.TrainingAddressLocation.setValue(d.TrainingAddressLocation);
        this.TraineeHouseNumber.setValue(d.TraineeHouseNumber);
        this.TraineeStreetMohalla.setValue(d.TraineeStreetMohalla);
        this.TraineeMauzaTown.setValue(d.TraineeMauzaTown);
      }
      else{
        this.isShownTraineeProfile=false;
        this.TSPMasterID.setValue(0);
        this.TraineeID.setValue(0);
        this.http.ShowError("Trainee Profile not found");
      }
     
    }, error => this.error = error // error path
    );
  }
  GetTSPProfile() {
    debugger;
    this.http.getJSON('api/Complaint/GetTSPProfile').subscribe((d: any) => {
      debugger;
      if(d.length!=0){
        this.isShownTSPProfile = true ;
        this.isShownTraineeProfile=false;
        this.TraineeID.setValue(0);
        this.TSPMasterID.setValue(d[0].TSPMasterID);
        this.TSPName.setValue(d[0].TSPName);
        this.TSPCode.setValue(d[0].TSPCode);
        this.Address.setValue(d[0].Address);
        this.HeadLandline.setValue(d[0].HeadLandline);
      }
      else{
      this.http.ShowError("TSP Profile not found");
      }
      
    
    }, error => this.error = error // error path
    );
  }
  reset() {
    this.isShownTraineeProfile = false ;
    this.isShownTSPProfile = false ;
    this.TraineeCNICHidden = false ;
    this.isShownTraineeProfile=false;
    this.myInputVariable.nativeElement.value = "";
    this.Complaintform.reset();
    this.submittedBit ="false";
    this.title = "Add New ";
    this.savebtn = "Save ";
    this.disabledFields="false";
    this.readonly="false";
    this.isShown = true ;
  }
  submitted(d:any){
    debugger;
    this.submittedBit=d;
  }

  Submit() {
    debugger;
    if (this.submittedBit !="false")
      this.Complaintform.value.Submitted = this.submittedBit;
    /* if (this.hiddenComplainttypeID > 0)
      this.Complaintform.value.ComplaintTypeID = this.hiddenComplainttypeID; */
      if(this.selectedFile!=null)//base64
      this.Complaintform.value.base64 = this.selectedFile;
      this.Complaintform.value.ext = this.ext;
    debugger;
    if(this.Complaintform.value.ComplaintTypeID==1)
    {
      if(this.TraineeCNIC.value=="")
      {
        this.http.ShowError("Please Enter Trainee CNIC");
        return true;
      }
      if(this.TraineeID.value==0||this.TraineeID.value==null)
      {
        this.http.ShowError("Please Enter Correct Trainee CNIC");
        return true;
      }
      
    }
    if (!this.Complaintform.valid)
      return;
      this.working = true;
    this.http.postJSON('api/Complaint/save', this.Complaintform.value).subscribe((d: any) => {
      this.FetchComplaintForGridView();
      this.http.openSnackBar(this.ComplainantID.value > 0 ? environment.UpdateMSG.replace("${Name}", this.EnText) : environment.SaveMSG.replace("${Name}", this.EnText));
     this.reset();
     this.ComplainantID.setValue(0);
     this.ComplaintTypeID.setValue(0);
     this.submittedBit ="false";
     this.title = "Add New ";
     this.working = false;
     this.savebtn = "Save ";
    },
    (error) => {
      this.error = error.error;
      this.http.ShowError(error.error);
    });
  }
  toggleEditDisable(row){
    this.toggleEdit(row);
    this.disabledFields="true";
    this.readonly="true";
    this.isShown = false ;
    
  }
  
  toggleEdit(row) {
this.reset();
this.tabGroup.selectedIndex = TabGroup.Complaintform;
    this.ComplainantID.setValue(row.ComplainantID);
    this.ComplaintDescription.setValue(row.ComplaintDescription);
    this.Complaintform.controls['ComplaintTypeID'].patchValue(row.ComplaintTypeID);   
    this.complaintTypeValueForDll=row.ComplaintSubTypeID;
    this.GetComplaintTSubTypeByComplaintType(row.ComplaintTypeID)
    this.title = "Update ";
    this.savebtn = "Update "; 
     if(row.Submitted==true){
      this.TraineeCNICHidden=false;
      this.title = "View";
     }
    if(row.TraineeID>0&&(row.TSPMasterID==null||row.TSPMasterID==0)){
      this.isShownTraineeProfile=true;
      this.isShownTSPProfile = false ;
      this.TSPMasterID.setValue(0);
      this.TraineeID.setValue(row.TraineeID);
      this.TraineeName.setValue(row.TraineeName);
      this.FatherName.setValue(row.FatherName);
      this.TraineeCNIC.setValue(row.TraineeCNIC);
      this.TraineeCode.setValue(row.TraineeCode);
      this.ClassCode.setValue(row.ClassCode);
      this.ContactNumber1.setValue(row.ContactNumber1);
      this.TrainingAddressLocation.setValue(row.TrainingAddressLocation);
      this.TraineeHouseNumber.setValue(row.TraineeHouseNumber);
      this.TraineeStreetMohalla.setValue(row.TraineeStreetMohalla);
      this.TraineeMauzaTown.setValue(row.TraineeMauzaTown);
    }
    if(row.TSPMasterID>0&&(row.TraineeID==null||row.TraineeID==0)){
      this.isShownTSPProfile = true ;
      this.isShownTraineeProfile=false;
      this.TraineeID.setValue(0);
      this.TSPMasterID.setValue(row.TSPMasterID);
      this.TSPName.setValue(row.TSPName);
      this.TSPCode.setValue(row.TSPCode);
      this.Address.setValue(row.Address);
      this.HeadLandline.setValue(row.HeadLandline);
    }

    
}
  GetComplaintTSubTypeByComplaintType (ComplaintTypeID: number)  {//TraineeCNIC
    this.TraineeCNIC.setValue('');
    this.isShownTraineeProfile = false ;
    this.isShownTSPProfile = false ;
    this.TraineeCNICHidden = false ;
    if(ComplaintTypeID==2)
    this.GetTSPProfile();
    this.hiddenComplainttypeID = ComplaintTypeID;
    debugger;
    if(ComplaintTypeID==1){
    this.TraineeCNICHidden = true ;}
   else if(ComplaintTypeID==2){
    this.TraineeCNICHidden = false ;}
    else {this.TraineeCNICHidden = false ;}
    this.http.getJSON(`api/Complaint/GetComplaintTSubTypeByComplaintType?ComplaintTypeID=` + ComplaintTypeID)
      .subscribe((data: any) => {
        debugger;
        this.ComplaintSubTypedll = data[0];
        this.Complaintform.controls['ComplaintSubTypeID'].patchValue( this.complaintTypeValueForDll);
      }, error => {
        this.error = error;
      })
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
toggleActive(row,InActive:any) {
  debugger;
 /*  this.http.postJSON('api/Complaint/ActiveInActive', { 'ComplainantID': row.ComplainantID, 'InActive': del })
              .subscribe((d: any) => {
                  this.http.openSnackBar(row.InActive == true ? environment.InActiveMSG.replace("${Name}", this.EnText) : environment.ActiveMSG.replace("${Name}", this.EnText));
              },
                  error => this.error = error // error path
              ); */
  this.http.confirm().subscribe(result => {
      if (result) {
          this.http.postJSON('api/Complaint/ActiveInActive', { 'ComplainantID': row.ComplainantID, 'InActive': InActive })
              .subscribe((d: any) => {
                this.FetchComplaintForGridView();
                  this.http.openSnackBar(row.InActive == true ? environment.InActiveMSG.replace("${Name}", this.EnText) : environment.ActiveMSG.replace("${Name}", this.EnText));
             
                },
                (error) => {
                  this.error = error.error;
                  this.http.ShowError(error.error);
                  row.InActive = !row.InActive;
                });
      }
      else {
         return;
      }
  });
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
openHistoryDialogue(data: any): void {
  debugger;
  this.dialogueService.openComplaintHistoryDialogue(data);
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
  public download(downloadUrl: string): void {
    debugger;
  
    window.open(downloadUrl, '_blank');
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
  public base64ToBlob(b64Data, ComplaintNo) 
  {
        var elem = window.document.createElement('a');
        elem.href =b64Data
        elem.download = "ComplaintNo_"+ComplaintNo+".zip";         
        document.body.appendChild(elem);
        elem.click();        
        document.body.removeChild(elem);
  }
  get ComplainantID() { return this.Complaintform.get("ComplainantID"); }
  get ComplainantName() { return this.Complaintform.get("ComplainantName"); }
  get ComplaintDescription() { return this.Complaintform.get("ComplaintDescription"); }
  get ComplaintStatus() { return this.Complaintform.get("ComplaintStatus"); }
  get ComplaintTypeID() { return this.Complaintform.get("ComplaintTypeID"); }
  get ComplaintTypeName() { return this.Complaintform.get("ComplaintTypeName"); }
  get ComplaintSubTypeID() { return this.Complaintform.get("ComplaintSubTypeID"); }
  get ComplaintSubTypeName() { return this.Complaintform.get("ComplaintSubTypeName"); }
  get UserID() { return this.Complaintform.get("UserID"); }
  get TraineeID() { return this.Complaintform.get("TraineeID"); }
  get TraineeCNIC() { return this.Complaintform.get("TraineeCNIC"); }
  get TraineeCode() { return this.Complaintform.get("TraineeCode"); }
  get TraineeName() { return this.Complaintform.get("TraineeName"); }
  get FatherName() { return this.Complaintform.get("FatherName"); }
  get TraineeStreetMohalla() { return this.Complaintform.get("TraineeStreetMohalla"); }
  get TraineeMauzaTown() { return this.Complaintform.get("TraineeMauzaTown"); }
  get TraineeDistrictID() { return this.Complaintform.get("TraineeDistrictID"); }
  get TraineeTehsilID() { return this.Complaintform.get("TraineeTehsilID"); }
  get ClassCode() { return this.Complaintform.get("ClassCode"); }
  get ContactNumber1() { return this.Complaintform.get("ContactNumber1"); }
  get TrainingAddressLocation() { return this.Complaintform.get("TrainingAddressLocation"); }
  get TraineeHouseNumber() { return this.Complaintform.get("TraineeHouseNumber"); }
   get TSPMasterID() { return this.Complaintform.get("TSPMasterID"); }
  get TSPName() { return this.Complaintform.get("TSPName"); }
  get TSPCode() { return this.Complaintform.get("TSPCode"); }
  get Address() { return this.Complaintform.get("Address"); }
  get HeadLandline() { return this.Complaintform.get("HeadLandline"); }

}
export class ComplaintModel extends ModelBase {

  ComplainantID: number;
  UserID:number;
  ComplainantName: string;
  ComplaintDescription: string;
  ComplaintStatus: string;
  ComplaintTypeID: number;
  ComplaintTypeName: string;
  ComplaintSubTypeID: number;
  ComplaintSubTypeName: string;
  TraineeCNIC: string;
  TSPCode: string;
}
enum TabGroup {
  Complaintform = 0,
  ComplaintList = 1
}