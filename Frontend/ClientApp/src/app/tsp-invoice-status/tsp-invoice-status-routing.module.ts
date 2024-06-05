import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { TSPInvoiceStatusComponent } from './tsp-invoice-status/tsp-invoice-status.component';
import { AuthGuardService } from '../security/auth-guard.service';


const routes: Routes = [{ path: "tsp-invoice-status", component: TSPInvoiceStatusComponent, canActivate: [AuthGuardService], data: { icon: "verified_user", inMenu: true, title: "tsp-invoice-status" } }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class TSPInvoiceStatusRoutingModule { }
