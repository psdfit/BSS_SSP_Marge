import { Component, OnInit, Input } from "@angular/core";
import { CommonSrvService } from "../../common-srv.service";

@Component({
  selector: 'instructor-profile',
  templateUrl: './instructor-profile.component.html',
  styleUrls: ['./instructor-profile.component.scss']
})


export class InstructorProfileComponent implements OnInit{

  constructor(private commonService: CommonSrvService) {
    
  }


  @Input() ID: any;

  InstructorProfile: any;
  ClassesDetail: any[];

  getProfileData() {
    this.commonService.getJSON("api/AdvancedSearch/GetInstructorProfile?InstructorID=" + this.ID).subscribe((data: any[]) => {
      this.InstructorProfile = data["InstructorProfile"][0];
      this.ClassesDetail = data["ClassesDetail"];
    });
  }

  ngOnInit(): void {
    this.getProfileData();
  }
}
