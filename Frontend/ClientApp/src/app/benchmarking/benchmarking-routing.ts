import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { BenchmarkingComponent } from './benchmarking/benchmarking.component';
import { AuthGuardService } from '../../app/security/auth-guard.service';




const routes: Routes = [{
      path: "trade-benchmarking", component: BenchmarkingComponent, canActivate: [AuthGuardService], data: { icon: "verified_user", inMenu: true, title: "Trade Benchmarking" }
}
];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class BenchmarkingRoutingModule { }
