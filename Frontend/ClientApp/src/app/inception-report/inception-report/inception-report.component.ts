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
import { EnumClassStatus } from '../../shared/Enumerations';
import { Router } from '@angular/router';
import parse from 'date-fns/esm/parse';


@Component({
  selector: 'hrapp-inception-report',
  templateUrl: './inception-report.component.html',
  styleUrls: ['./inception-report.component.scss']
})

export class InceptionReportComponent implements OnInit {
  inceptionreportform: FormGroup;
  invalidTLD: boolean = false;
  title: string; savebtn: string;
  displayedColumns = ['ClassID', 'ClassStartTime', 'ClassEndTime', 'ClassTotalHours', 'EnrolledTrainees', 'Shift', 'FinalSubmitted', 'InActive', "Action"];
  inceptionreport: MatTableDataSource<any>;
  ContactPerson = []; CPs = []; ClassSections: any; Instructors: any;
  ActiveClassTiming: any;
  orgConfig: IOrgConfig;
  classObj: any;
  classStartDate: Date;
  classEndDate: Date;
  StartDate: Date;
  EndDate: Date;
  minStartDate: Date;
  GetStartDate: string;
  GetEndDate: string;
  doOverlap: Boolean = true;
  APIExecuted: boolean = false;
  GetCurrentStartDate: Date;
  GetCurrentEndDate: Date;

  endDateworking = false;
  startDateworking = false;
  shiftworking = false;
  dailyHoursworking = false;
  invalidTLD: boolean = false;

  isOpenRegistration: boolean = false;
  isOpenSubmission: boolean = false;
  paramEndDate: boolean = false;
  isOpenInceptionMessage: string = "";
  formrights: UserRightsModel;
  EnText: string = "Inception Report";
  error: String;
  errorContactPerson: String;
  diffInDays: number;

  TimeDifference: number;

  query = {
    order: 'IncepReportID',
    limit: 5,
    page: 1
  };
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  working: boolean = false;
  constructor(private fb: FormBuilder, private ComSrv: CommonSrvService, private route: ActivatedRoute, private router: Router) {
    this.inceptionreportform = this.fb.group({
      IncepReportID: 0,
      ClassID: "",
      ClassCode: "",
      TradeName: "",
      SchemeName: "",
      TraineesPerClass: "",
      MinHoursPerMonth: "",
      ClassStartTime: ["", Validators.required],
      ClassEndTime: ["", Validators.required],
      ActualStartDate: "",
      ActualEndDate: "",
      ClassTotalHours: "",
      EnrolledTrainees: "",
      Shift: "",
      //CenterLocation: ["", Validators.required],
      Monday: 0,
      Tuesday: 0,
      Wednesday: 0,
      Thursday: 0,
      Friday: 0,
      Saturday: 0,
      Sunday: 0,
      FinalSubmitted: 0,
      SectionID: ["", Validators.required],
      InstrIDs: ["", Validators.required],
      InActive: ""
    }, { updateOn: "blur" });
    this.inceptionreport = new MatTableDataSource([]);
    this.formrights = ComSrv.getFormRights("inception-report");
    route.params.subscribe((par) => {
      this.ClassID.setValue(par["id"]);
      this.GetReportData(par["id"]);
      //this.GetDateDifference(par["StartDate"], par["EndDate"]);
      //this.minStartDate = par["StartDate"];
      //this.minStartDate = typeof (this.minStartDate) == 'string' ? new Date(this.minStartDate) : this.minStartDate;

      //this.minStartDate = new Date(this.minStartDate.setDate(this.minStartDate.getDate() + 1));

      //console.log(par["StartDate"]);
      //console.log(par["EndDate"]);


    });

  }

  ChkContactPerson(event: string) {
    //console.log(this.ContactPerson)
    //for (var cp of this.ContactPerson[]) {
    //    if (cp.ContactPersonMobile == event) {
    //        this.error = "This Contact Person is already added";
    //        this.ComSrv.ShowError(this.error.toString(), "Error");
    //        return false;
    //    }
    //}
    this.ComSrv.getJSON('api/ContactPerson/CheckContactPersonMobile/' + event).subscribe((d: any) => {
      //this.users = d;
      if (d) {
        this.ContactPerson.pop();
        this.ContactPerson.push(d);
      }
      else
        console.log("nothing found")
    }, error => this.error = error // error path
    );
  };

  AddContactPerson() {
    this.ContactPerson.push({
      ContactPersonID: 0,
      ContactPersonType: '',
      ContactPersonName: '',
      ContactPersonEmail: '',
      ContactPersonLandline: '',
      ContactPersonMobile: ''
    });
  }
  RemovePerson(c) {
    this.ContactPerson.splice(this.ContactPerson.indexOf(c), 1);
  }



  GenerateDateDifference() {

    console.log(this.classStartDate);
    console.log(this.ActualStartDate.value);

    this.diffInDays = Math.abs(this.ActualStartDate.value.diff(this.classStartDate, 'days'));


    this.diffInDays = Math.abs(this.ActualStartDate.value.diff(this.classStartDate, 'days'));


    console.log(this.diffInDays);

    //this.diffInDays = this.diffInDays;

    console.log(this.diffInDays);
    this.EndDate = typeof (this.classEndDate) == 'string' ? new Date(this.classEndDate) : this.classEndDate;
    let newDate = new Date(this.EndDate.setDate((this.EndDate.getDate() + this.diffInDays) + 1));
    //this.EndDate = typeof (this.EndDate) == 'string' ? new Date(this.EndDate) : this.EndDate;
    //let newDate = new Date(this.EndDate.setDate(this.EndDate.getDate() + this.diffInDays));
    this.ActualEndDate.setValue(newDate);
    console.log(this.EndDate);
    console.log(this.ActualEndDate);
  }


  GenerateShift() {
    var Stime = this.ClassStartTime.value.split(":");

    var IStime = Stime[0];
    var FStime = Number(IStime);
    if (FStime < 12) {
      console.log(this.ClassStartTime.value);
      this.Shift.setValue('1st');

    }
    else {
      this.Shift.setValue('2nd');


    }

  }

  GenerateHours() {
    if (this.ClassStartTime.value != "" && this.ClassEndTime.value != "") {
      var Stime = this.ClassStartTime.value.split(":");
      var SMtime = this.ClassStartTime.value.split(" ");
      var SM = SMtime[1];
      var IStime = Stime[0];
      var SMinTime = Number(Stime[1]);
      var FStime = Number(IStime);

      console.log(Stime)
      console.log(IStime)
      console.log(FStime);

      var Etime = this.ClassEndTime.value.split(":");
      var EMtime = this.ClassEndTime.value.split(" ");
      var EM = EMtime[1];
      var IEtime = Etime[0];
      var EMinTime = Number(Etime[1]);
      var FEtime = Number(IEtime);

      console.log(Etime)
      console.log(IEtime);
      console.log(FEtime);

      var StartTimeMinutes = (FStime * 60) + SMinTime
      var EndTimeMinutes = (FEtime * 60) + EMinTime

      this.TimeDifference = EndTimeMinutes - StartTimeMinutes


      var num = this.TimeDifference;
      var hours = (num / 60);
      var rhours = Math.floor(hours);
      var minutes = (hours - rhours) * 60;
      var rminutes = Math.round(minutes);

      var finalTime = rhours + ":" + rminutes;
      var finalTime1 = parseFloat(rhours + "." + rminutes).toFixed(2);
      var finalTime2 = parseFloat(parseFloat(rhours + "." + rminutes).toFixed(2));

      //var TimeDifferenceHoursString = (this.TimeDifference / 60).toString();
      //var TimeDifferenceStringSplitted = TimeDifferenceHoursString.split(".");
      //var TimeDifferenceHoursStringSplitted = TimeDifferenceStringSplitted[0];
      ////var TimeDifferenceHours = Number(TimeDifferenceHoursString.split("."));

      //var TimeDifferenceMinutes = (this.TimeDifference % 60).toString();

      //var TimeDifferenceHours = parseFloat(parseFloat(TimeDifferenceHoursStringSplitted + "." + TimeDifferenceMinutes).toFixed(2));

      FStime = FStime == 24 ? 0 : FStime;


      if (FStime > FEtime) {
        this.ClassEndTime.reset();
        this.error = "Class End Time Should be Greater than Start Time";
        this.ComSrv.ShowError(this.error.toString(), "Error");
        return false;
      }
      else {
        var TotalHours = FEtime - FStime;
        this.ClassTotalHours.setValue(finalTime);
      }
    }
    //if (SM == "AM" && EM == "AM") {
    //  if (FStime > FEtime) {
    //    var hours = FStime - FEtime;
    //    var TotalHours = hours;
    //    //this.ClassTotalHours.setValue(TotalHours);
    //    this.ClassEndTime.reset();
    //    this.error = "Class End Time Should be Greater than Start Time";
    //    this.ComSrv.ShowError(this.error.toString(), "Error");
    //    return false;
    //  } else {
    //    var hours = FEtime - FStime;
    //    var TotalHours = hours;
    //    this.ClassTotalHours.setValue(TotalHours);
    //  }

    //}
    //if (SM == "PM" && EM == "PM") {
    //  if (FStime > FEtime) {
    //    var hours = FStime - FEtime;
    //    var TotalHours = hours;
    //    //this.ClassTotalHours.setValue(TotalHours);
    //    this.ClassEndTime.reset();
    //    this.error = "Class End Time Should be Greater than Start Time";
    //    this.ComSrv.ShowError(this.error.toString(), "Error");
    //    return false;
    //  } else {
    //    var TotalHours = FEtime - FStime
    //    this.ClassTotalHours.setValue(TotalHours);
    //  }

    //} else {
    //  return;
    //}



  }

  GetData() {
    this.ComSrv.getJSON('api/InceptionReport/GetInceptionReport/' + this.ClassID.value).subscribe((d: any) => {
      this.inceptionreport = new MatTableDataSource(d[0]);
      this.ClassSections = d[1];
      this.Instructors = d[2];
      this.inceptionreport.paginator = this.paginator;
      this.inceptionreport.sort = this.sort;


    }, error => this.error = error// error path
    );
  };


  GetReportData(id: number) {
    this.ComSrv.postJSON('api/InceptionReport/RD_InceptionReportBy', { "ClassID": id }).subscribe((response: any) => {
      let reportSubmissionBracket = <any[]>response[0];
      let InceptionReportData = <any[]>response[1];
      this.classObj = response[2];
      this.orgConfig = response[3];
      let orgConfigList = response[3];

      this.ClassCode.setValue(this.classObj.ClassCode);
      this.SchemeName.setValue(this.classObj.SchemeName);
      this.TradeName.setValue(this.classObj.TradeName);
      this.TraineesPerClass.setValue(this.classObj.TraineesPerClass);
      this.MinHoursPerMonth.setValue(this.classObj.MinHoursPerMonth);

      if (orgConfigList.length <= 0) {
        this.error = "Please set 'Rules' of this class before creating report.";
        this.inceptionreportform.disable({ onlySelf: true });
        this.ComSrv.ShowError(this.error.toString(), "Error");
        return;
      }
      ///
      if (this.classObj.ClassStatusID != EnumClassStatus.Ready && InceptionReportData.length == 0) {
        this.error = "Class status must be 'Ready' to submit inception report.";
        this.inceptionreportform.disable({ onlySelf: true });
        this.ComSrv.ShowError(this.error.toString(), null, 6000);
        return;
      }
      if (reportSubmissionBracket.length > 0) {
        this.inceptionreportform.disable({ onlySelf: true });
        this.isOpenSubmission = false;
        this.isOpenInceptionMessage = reportSubmissionBracket[0].ErrorMessage;
        return;
      }
      this.ActualStartDate.setValue(this.classObj.StartDate);
      this.ActualEndDate.setValue(this.classObj.EndDate);

      this.endDateworking = true;
      this.startDateworking = true;

      this.EnrolledTrainees.setValue(this.classObj.TraineesPerClass);




      //BR Business Rule
      let classStartDate = this.classObj.StartDate
      this.classStartDate = this.classObj.StartDate
      let classEndDate = this.classObj.EndDate
      this.classEndDate = this.classObj.EndDate
      //let classStart = typeof (classStartDate) == "string" ? new Date(classStartDate) : classStartDate;
      //let today = new Date();
      //let openeing = new Date(classStart);
      //let closing = new Date(classStart);
      //openeing.setDate((classStart.getDate() - this.orgConfig[0].ReportBracketBefore));
      //closing.setDate((classStart.getDate() + this.orgConfig[0].ReportBracketAfter));
      //if (today >= openeing && today <= closing) {
      //    this.inceptionreportform.enable({ onlySelf: true });
      //    this.isOpenSubmission = true;
      //    this.isOpenInceptionMessage = "(Open)"
      //} else {
      //    this.inceptionreportform.disable({ onlySelf: true });
      //    this.isOpenSubmission = false;
      //    this.isOpenInceptionMessage = "(Closed)"
      //}

      let data = InceptionReportData[0];
      if (data) {
        if (data.FinalSubmitted) {
          this.inceptionreportform.disable({ onlySelf: true });
          this.isOpenSubmission = false;
          this.isOpenRegistration = true;
          this.isOpenInceptionMessage = "(Submitted)"
        }
        this.IncepReportID.setValue(data.IncepReportID);
        this.ClassID.setValue(data.ClassID);
        this.ClassStartTime.setValue(moment(data.ClassStartTime).format('HH:mm').toString());
        //this.ClassStartTime.setValue((data.ClassStartTime).format('hh:mm a'));
        //this.ClassEndTime.setValue((data.ClassEndTime).format('hh:mm a'));
        this.ClassEndTime.setValue(moment(data.ClassEndTime).format('HH:mm').toString());
        this.ActualStartDate.setValue(data.ActualStartDate);
        this.ActualEndDate.setValue(data.ActualEndDate);
        this.ClassTotalHours.setValue(data.ClassTotalHours);
        this.EnrolledTrainees.setValue(data.EnrolledTrainees);
        this.Shift.setValue(data.Shift);
        //this.CenterLocation.setValue(data.CenterLocation);
        this.Monday.setValue(data.Monday);
        this.Tuesday.setValue(data.Tuesday);
        this.Wednesday.setValue(data.Wednesday);
        this.Thursday.setValue(data.Thursday);
        this.Friday.setValue(data.Friday);
        this.Saturday.setValue(data.Saturday);
        this.Sunday.setValue(data.Sunday);
        this.FinalSubmitted.setValue(data.FinalSubmitted);
        this.SectionID.setValue(data.SectionID);
        let i = response[0].filter(x => x.InstrIDs.split(',').map(Number));
        this.InstrIDs.setValue(data.InstrIDs.split(',').map(Number));
        this.GetContactPersonData(data.IncepReportID);
      }
    }, error => this.error = error// error path
    );
  }

  GetContactPersonData(id) {
    this.ComSrv.getJSON('api/InceptionReport/GetContactPerson/' + id).subscribe((REs: any) => {
      this.ContactPerson = REs;
    }, error => this.error = error // error path
    )
  }

  ngOnInit() {
    this.ComSrv.setTitle("InceptionReport");
    this.title = "Add New ";
    this.savebtn = "Save ";
    this.GetData();
    this.paramEndDate = true;
  }

  Submit() {

    let selectedDaysCount = 0;
    if (this.Monday.value == 1) {
      selectedDaysCount++;
    }
    if (this.Tuesday.value == 1) {
      selectedDaysCount++;
    }
    if (this.Wednesday.value == 1) {
      selectedDaysCount++;
    }
    if (this.Thursday.value == 1) {
      selectedDaysCount++;
    }
    if (this.Friday.value == 1) {
      selectedDaysCount++;
    }
    if (this.Saturday.value == 1) {
      selectedDaysCount++;
    }
    if (this.Sunday.value == 1) {
      selectedDaysCount++;
    }
    if (selectedDaysCount * Number(this.TimeDifference) * 4 < (this.classObj.MinHoursPerMonth * 60)) {
      this.error = "Total Class Hours are less than class hours in Appendix";
      this.ComSrv.ShowError(this.error.toString(), "Error");
      if (this.FinalSubmitted.value == true)
        this.FinalSubmitted.setValue(false)
      return false;
    }

    if (this.ContactPerson.length == 0) {
      this.error = "Please add at-least 1 Contact Person";
      this.ComSrv.ShowError(this.error.toString(), "Error");
      if (this.FinalSubmitted.value == true)
        this.FinalSubmitted.setValue(false)
      return false;
    }

    //if (this.ContactPerson.length > 0) {
    //  this.ContactPerson.forEach(x => {
    //    if (x.ContactPersonType == '' || x.ContactPersonName == '' || x.ContactPersonEmail == '' || x.ContactPersonLandline == '' || x.ContactPersonMobile == '') {
    //      this.errorContactPerson = "Please enter all required fields of a contact person";
    //    }
    //  }
    //  );

    //  if (this.errorContactPerson) {
    //    this.ComSrv.ShowError(this.errorContactPerson.toString(), "Error");
    //    this.errorContactPerson = null;
    //    this.FinalSubmitted.setValue(false)
    //    return false;
    //  }

    //}

    if (this.ContactPerson.length > 0) {
      let tldValidationPromises = [];

      this.ContactPerson.forEach(x => {
        if (x.ContactPersonType == '' || x.ContactPersonName == '' || x.ContactPersonEmail == '' || x.ContactPersonLandline == '' || x.ContactPersonMobile == '') {
          this.errorContactPerson = "Please enter all required fields of a contact person";
        } else if (x.ContactPersonEmail.length > 0) {
          // Validate TLD only when email is provided
          const tldPromise = new Promise<void>((resolve, reject) => {
            this.ComSrv.fetchAndValidateTLD(x.ContactPersonEmail)
              .subscribe(
                (isValidTLD: boolean) => {
                  if (!isValidTLD) {
                    this.errorContactPerson = "Invalid email address: " + x.ContactPersonEmail;
                    this.invalidTLD = true;
                  }
                  resolve();  // Call resolve to indicate the promise is done
                },
                (error) => {
                  this.errorContactPerson = "Error while validating TLD: " + error;
                  reject(error);  // Call reject if there's an error
                }
              );
          });

          tldValidationPromises.push(tldPromise);
        }
      });

      // Handle the results of all TLD validations
      Promise.all(tldValidationPromises).then(() => {
        if (this.errorContactPerson) {
          this.ComSrv.ShowError(this.errorContactPerson.toString(), "Error");
          this.errorContactPerson = null;
          this.FinalSubmitted.setValue(false);
        } else {
          // Proceed if no errors
          this.FinalSubmitted.setValue(true);
        }
      }).catch(error => {
        console.error("Error during TLD validation:", error);
      });
    }

    if (!this.inceptionreportform.valid)
      return;
    this.working = true;

    //--- Getting other class timing of selected instructor of the active class.

    this.ComSrv.getJSON('api/InceptionReport/GetActiveClassTiming/' + this.InstrIDs.value.join(',')).subscribe((d: any) => {
      this.ActiveClassTiming = d[0];
      let ActiveClassTimingList = d[0];

      if (ActiveClassTimingList.length > 0) {
        this.GetStartDate = this.ActiveClassTiming[0].StartDateTime;
        this.GetEndDate = this.ActiveClassTiming[0].EndDateTime;
        this.GetCurrentStartDate = this.ActiveClassTiming[0].ActualStartDate;
        this.GetCurrentEndDate = this.ActiveClassTiming[0].ActualEndDate;
        //const datetimesa = new Date(this.GetStartDate);

        //return false;
        //const GetStartDate1 = this.formatTimeToHHMM(this.GetStartDate);
        //const GetStartDate2 = this.formatTimeToHHMM(this.GetEndDate);
        //const otherClassTime: ClassTime = {
        //  startTime: this.GetStartDate,  // Replace with actual start time
        //  endTime: this.GetEndDate  // Replace with actual end time
        //};

        //const currentClassTime: ClassTime = {
        //  startTime: this.ClassStartTime.value,  // Replace with actual start time
        //  endTime: this.ClassEndTime.value   // Replace with actual end time
        //};

        const currentClassStartDate = new Date(this.ActualStartDate.value);
        const currentClassEndDate = new Date(this.ActualEndDate.value);
        const currentClassStartTime = this.ClassStartTime.value;
        const currentClassEndTime = this.ClassEndTime.value;

        const otherClassStartDate = new Date(this.GetCurrentStartDate);
        const otherClassEndDate = new Date(this.GetCurrentEndDate);
        const otherClassStartTime = this.GetStartDate;
        const otherClassEndTime = this.GetEndDate;

        //Class funcation overlap result
        //this.doOverlap = doClassTimesOverlap(currentClassTime, otherClassTime);
        this.doOverlap = doClassTimesOverlap(
          currentClassStartDate,
          currentClassEndDate,
          currentClassStartTime,
          currentClassEndTime,
          otherClassStartDate,
          otherClassEndDate,
          otherClassStartTime,
          otherClassEndTime
        );
        if (this.doOverlap) {
          //console.log("Instructor already assigned to another class in the same time bracket!");
          this.error = "Instructor already assigned to another class in the same time bracket!";
          this.ComSrv.ShowError(this.error.toString(), "Error");
          this.FinalSubmitted.setValue(false);
          return false;
        }
        else {
          ///-----Saving Code 
          this.inceptionreportform.value["ContactPersons"] = this.ContactPerson;
          this.inceptionreportform.value['InstrIDs'] = this.InstrIDs.value.join(',');
          this.ComSrv.postJSON('api/InceptionReport/Save', this.inceptionreportform.value)
            .subscribe((d: any) => {
              this.inceptionreport = new MatTableDataSource(d);
              this.inceptionreport.paginator = this.paginator;
              this.inceptionreport.sort = this.sort;
              this.ComSrv.openSnackBar(this.IncepReportID.value > 0 ? environment.UpdateMSG.replace("${Name}", this.EnText) : environment.SaveMSG.replace("${Name}", this.EnText));
              // window.location.reload();
              this.GetReportData(this.ClassID.value);

              //this.reset();
              this.title = "Add New ";
              this.savebtn = "Save ";
            },
              error => this.error = error // error path
              , () => {
                this.working = false;
              });
        }
      }
      else {
        ///-----Saving Code 
        this.inceptionreportform.value["ContactPersons"] = this.ContactPerson;
        this.inceptionreportform.value['InstrIDs'] = this.InstrIDs.value.join(',');
        this.ComSrv.postJSON('api/InceptionReport/Save', this.inceptionreportform.value)
          .subscribe((d: any) => {
            this.inceptionreport = new MatTableDataSource(d);
            this.inceptionreport.paginator = this.paginator;
            this.inceptionreport.sort = this.sort;
            this.ComSrv.openSnackBar(this.IncepReportID.value > 0 ? environment.UpdateMSG.replace("${Name}", this.EnText) : environment.SaveMSG.replace("${Name}", this.EnText));
            // window.location.reload();
            this.GetReportData(this.ClassID.value);

            //this.reset();
            this.title = "Add New ";
            this.savebtn = "Save ";
          },
            error => this.error = error // error path
            , () => {
              this.working = false;
            });
      }
    }, error => this.error = error// error path
    );

  }


  checkOnCPEmail(email: any) {

    this.ComSrv.fetchAndValidateTLD(email)
      .subscribe(
        (isValidTLD: boolean) => {
          if (isValidTLD) {
            this.invalidTLD = false;  // Set to false if TLD is valid
          } else {
            this.invalidTLD = true;  // Set to true if TLD is invalid
          }
        }, (error) => {
          this.error = error // error path
        }
      );
  }


  formatTimeToHHMM(date: Date): string {
    const hours = date.getHours();
    const minutes = date.getMinutes();

    const formattedHours = hours < 10 ? `0${hours}` : `${hours}`;
    const formattedMinutes = minutes < 10 ? `0${minutes}` : `${minutes}`;

    return `${formattedHours}:${formattedMinutes}`;
  }

  FinalSubmit() {

    this.FinalSubmitted.setValue(true);
    this.Submit();

  }

  reset() {
    //this.inceptionreportform.reset();
    this.IncepReportID.setValue(0);

    this.ClassStartTime.reset();
    this.ClassEndTime.reset();
    this.ClassTotalHours.reset();
    this.Shift.reset();
    this.EnrolledTrainees.reset();
    //this.CenterLocation.reset();
    this.SectionID.reset();
    this.Monday.reset();
    this.Tuesday.reset();
    this.Wednesday.reset();
    this.Thursday.reset();
    this.Friday.reset();
    this.Saturday.reset();
    this.Sunday.reset();
    this.ContactPerson = null;
    this.title = "Add New ";
    this.savebtn = "Save ";
  }
  toggleEdit(row) {
    this.SectionID.setValue(row.SectionID);

    this.IncepReportID.setValue(row.IncepReportID);
    this.ClassID.setValue(row.ClassID);
    this.ClassStartTime.setValue(row.ClassStartTime);
    this.ClassEndTime.setValue(row.ClassEndTime);
    this.ClassTotalHours.setValue(row.ClassTotalHours);
    this.EnrolledTrainees.setValue(row.EnrolledTrainees);
    this.Shift.setValue(row.Shift);
    //this.CenterLocation.setValue(row.CenterLocation);
    this.Monday.setValue(row.Monday);
    this.Tuesday.setValue(row.Tuesday);
    this.Wednesday.setValue(row.Wednesday);
    this.Thursday.setValue(row.Thursday);
    this.Friday.setValue(row.Friday);
    this.Saturday.setValue(row.Saturday);
    this.Sunday.setValue(row.Sunday);
    this.FinalSubmitted.setValue(row.FinalSubmitted);
    this.InActive.setValue(row.InActive);

    this.title = "Update ";
    this.savebtn = "Save ";
  }

  routeToRegistration() {
    this.router.navigateByUrl(`/registration/trainee/${this.ClassID.value}`);

  }


  toggleActive(row) {
    this.ComSrv.confirm().subscribe(result => {
      if (result) {
        this.ComSrv.postJSON('api/InceptionReport/ActiveInActive', { 'IncepReportID': row.IncepReportID, 'InActive': row.InActive })
          .subscribe((d: any) => {
            this.ComSrv.openSnackBar(row.InActive == true ? environment.InActiveMSG.replace("${Name}", this.EnText) : environment.ActiveMSG.replace("${Name}", this.EnText));
            // this.inceptionreport =new MatTableDataSource(d);
          },
            error => this.error = error // error path
          );
      }
      else {
        row.InActive = !row.InActive;
      }
    });
  }
  applyFilter(filterValue: string) {
    filterValue = filterValue.trim(); // Remove whitespace
    filterValue = filterValue.toLowerCase(); // Datasource defaults to lowercase matches
    this.inceptionreport.filter = filterValue;
  }

  get IncepReportID() { return this.inceptionreportform.get("IncepReportID"); }
  get ClassID() { return this.inceptionreportform.get("ClassID"); }
  get ClassCode() { return this.inceptionreportform.get("ClassCode"); }
  get TradeName() { return this.inceptionreportform.get("TradeName"); }
  get SchemeName() { return this.inceptionreportform.get("SchemeName"); }
  get TraineesPerClass() { return this.inceptionreportform.get("TraineesPerClass"); }
  get MinHoursPerMonth() { return this.inceptionreportform.get("MinHoursPerMonth"); }
  get ClassStartTime() { return this.inceptionreportform.get("ClassStartTime"); }
  get ClassEndTime() { return this.inceptionreportform.get("ClassEndTime"); }
  get ActualEndDate() { return this.inceptionreportform.get("ActualEndDate"); }
  get ActualStartDate() { return this.inceptionreportform.get("ActualStartDate"); }
  get ClassTotalHours() { return this.inceptionreportform.get("ClassTotalHours"); }
  get EnrolledTrainees() { return this.inceptionreportform.get("EnrolledTrainees"); }
  get Shift() { return this.inceptionreportform.get("Shift"); }
  //get CenterLocation() { return this.inceptionreportform.get("CenterLocation"); }
  get Monday() { return this.inceptionreportform.get("Monday"); }
  get Tuesday() { return this.inceptionreportform.get("Tuesday"); }
  get Wednesday() { return this.inceptionreportform.get("Wednesday"); }
  get Thursday() { return this.inceptionreportform.get("Thursday"); }
  get Friday() { return this.inceptionreportform.get("Friday"); }
  get Saturday() { return this.inceptionreportform.get("Saturday"); }
  get Sunday() { return this.inceptionreportform.get("Sunday"); }
  get FinalSubmitted() { return this.inceptionreportform.get("FinalSubmitted"); }
  get SectionID() { return this.inceptionreportform.get("SectionID"); }
  get InstrIDs() { return this.inceptionreportform.get("InstrIDs"); }

  get InActive() { return this.inceptionreportform.get("InActive"); }

}
//Set time to string format


//Check classtime overlap of the same instructor
//function doClassTimesOverlap(time1: ClassTime, time2: ClassTime): boolean {
//  const [startHour1, startMinute1] = time1.startTime.split(':').map(Number);
//  const [endHour1, endMinute1] = time1.endTime.split(':').map(Number);
//  const [startHour2, startMinute2] = time2.startTime.split(':').map(Number);
//  const [endHour2, endMinute2] = time2.endTime.split(':').map(Number);

//  // Check for overlap
//  return (
//    (startHour1 < endHour2 || (startHour1 === endHour2 && startMinute1 < endMinute2)) &&
//    (startHour2 < endHour1 || (startHour2 === endHour1 && startMinute2 < endMinute1))
//  );
//}
function doClassTimesOverlap(
  currentClassStartDate: Date,
  currentClassEndDate: Date,
  currentClassStartTime: string, // Time in "HH:mm" format
  currentClassEndTime: string,   // Time in "HH:mm" format
  otherClassStartDate: Date,
  otherClassEndDate: Date,
  otherClassStartTime: string,   // Time in "HH:mm" format
  otherClassEndTime: string      // Time in "HH:mm" format
): boolean {
  // Check for date overlap
  const dateOverlap =
    currentClassStartDate <= otherClassEndDate &&
    currentClassEndDate >= otherClassStartDate;

  // Check for time overlap
  const timeOverlap =
    currentClassStartTime <= otherClassEndTime &&
    currentClassEndTime >= otherClassStartTime;

  // Return true if both date and time overlap
  return dateOverlap && timeOverlap;
}

interface ClassTime {
  startTime: string;
  endTime: string;
}

export class InceptionReportModel extends ModelBase {
  IncepReportID: number;
  ClassID: number;
  ClassStartTime: string;
  ClassEndTime: string;
  ClassTotalHours: number;
  EnrolledTrainees: number;
  Shift: string;
  //CenterLocation: string;
  Monday: Boolean;
  Tuesday: Boolean;
  Wednesday: Boolean;
  Thursday: Boolean;
  Friday: Boolean;
  Saturday: Boolean;
  Sunday: Boolean;
  FinalSubmitted: Boolean;
  SectionID: number;
  TradeName: string;
  SchemeName: string;
  MinHoursPerMonth: number;
  TraineesPerClass: number;


}
