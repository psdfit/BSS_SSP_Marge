import { Component, OnInit, Input } from "@angular/core";
import { CommonSrvService } from "../../common-srv.service";

@Component({
  selector: 'trainee-profile',
  templateUrl: './trainee-profile.component.html',
  styleUrls: ['./trainee-profile.component.scss']
})


export class TraineeProfileComponent implements OnInit{

  constructor(private commonService: CommonSrvService) {
    
  }


  @Input() ID: any;

  TraineeProfile: any;
  MPRDetail: any[];

  getProfileData() {
    this.commonService.getJSON("api/AdvancedSearch/GetTraineeProfile?TraineeID=" + this.ID).subscribe((data: any[]) => {
      this.TraineeProfile = data["TraineeProfile"][0];
      this.MPRDetail = data["MPRDetail"];
    });
  }

  ngOnInit(): void {
    this.getProfileData();
  }
}
