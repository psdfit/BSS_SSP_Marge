import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { TPMRTPsComponent } from './tpm-rtps/tpm-rtps.component';
import { TPMCenterInspectionComponent } from './tpm-center-inspection/tpm-center-inspection.component';
import { AuthGuardService } from '../../app/security/auth-guard.service';




const routes: Routes = [{
  
    path: "tpm-rtps", component: TPMRTPsComponent, canActivate: [AuthGuardService], data: { icon: "verified_user", inMenu: true, title: "TPM-RTPs" }
},{
  
        path: "tpm-center-inspection", component: TPMCenterInspectionComponent, canActivate: [AuthGuardService], data: { icon: "verified_user", inMenu: true, title: "TPM-Center-Inspection" }
}
];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class TPMRTPsRoutingModule { }
