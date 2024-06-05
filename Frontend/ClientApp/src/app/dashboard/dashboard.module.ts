import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { TraineeJourneyAllComponent } from "./trainee-journey-all/trainee-journey-all.component";
import { TraineeJourneyComponent } from "./trainee-journey/trainee-journey.component";
import { ClassJourneyComponent } from "./class-journey/class-journey.component";
import { ManagementComponent } from "./management/management.component";
import { KAMDashboardComponent } from "./kam/kam-dashboard.component";
import { SharedModule } from "../shared/shared.module";
import { DashboardRouting } from "./dashboard-routing";

@NgModule({
  declarations: [TraineeJourneyAllComponent, TraineeJourneyComponent, ClassJourneyComponent, ManagementComponent, KAMDashboardComponent],
  imports: [CommonModule, SharedModule, DashboardRouting ],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
})
export class DashboardModule {}
