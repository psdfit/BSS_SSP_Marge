import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { MasterSheetComponent } from "./master-sheet/master-sheet.component";
import { InceptionReportListComponent } from "./inception-report-list/inception-report-list.component";

import { AuthGuardService } from "../../app/security/auth-guard.service";
import { TradeLayerComponent } from "../master-data/trade-layer/trade-layer.component";
// import { TradeLayerComponent } from "../master-data/trade-layer/trade-layer.component";

const routes: Routes = [
  {
    // path: 'administration', component: AppLayoutComponent, children: [{
    path: "mastersheet",
    component: MasterSheetComponent,
    canActivate: [AuthGuardService],
    data: { icon: "verified_user", inMenu: true, title: "Master Sheet" },
  },
  {
    // path: 'administration', component: AppLayoutComponent, children: [{
    path: "TradeLayer",
    component: TradeLayerComponent,
    canActivate: [AuthGuardService],
    data: { icon: "verified_user", inMenu: true, title: "Trade Layer" },
  },
  {
    path: "inception-report-list",
    component: InceptionReportListComponent,
    canActivate: [AuthGuardService],
    data: {
      icon: "verified_user",
      inMenu: true,
      title: "Inception Report List",
    },
  },
];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class MasterSheetRoutingModule {}
