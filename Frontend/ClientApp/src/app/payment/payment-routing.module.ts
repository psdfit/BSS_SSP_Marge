import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { RegistrationPaymentComponent } from './registration-payment/registration-payment.component';
import { AssociationPaymentComponent } from './association-payment/association-payment.component';
import { AuthGuardService } from '../security/auth-guard.service';


const routes: Routes = [
  {
    path: 'registration-payment',
    component: RegistrationPaymentComponent,
    // canActivate: [AuthGuardService],
    data: {
      icon: 'verified_user',
      inMenu: true,
      title: 'Registration Payment'
    }
  },
  {
    path: 'association-payment',
    component: AssociationPaymentComponent,
    // canActivate: [AuthGuardService],
    data: {
      icon: 'verified_user',
      inMenu: true,
      title: 'Association Payment'
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PaymentRoutingModule { }
