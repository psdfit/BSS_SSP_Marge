import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { AuthGuardService } from "../security/auth-guard.service";
import { TspTraineePortalComponent } from "./tsp-trainee-portal/tsp-trainee-portal.component";
const routes: Routes = [
  {
    path: "tsp-trainee-portal", //module path from db
    component: TspTraineePortalComponent,
    canActivate: [AuthGuardService],
    data: {
      icon: "verified_user",
      inMenu: true,
      title: "TSP Trainee Portal",
    },
  },
];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class TSPTraineePortalRoutingModule {}
