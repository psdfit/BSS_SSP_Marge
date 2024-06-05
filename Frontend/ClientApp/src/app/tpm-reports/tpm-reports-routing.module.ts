import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { TPMReportsComponent } from './tpm-reports/tpm-reports.component';
import { AuthGuardService } from '../security/auth-guard.service';


const routes: Routes = [{
    path: "tpm-reports"
    , component: TPMReportsComponent
    , canActivate: [AuthGuardService]
    , data: {
        icon: "verified_user"
        , inMenu: true
        , title: "TPM Reports"
    }
}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class TPMReportsRoutingModule { }
