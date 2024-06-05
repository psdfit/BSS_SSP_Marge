import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { PaymentRoutingModule } from './payment-routing.module';
import { SharedModule } from '../shared/shared.module';

import { AssociationPaymentComponent } from './association-payment/association-payment.component';
import { RegistrationPaymentComponent } from './registration-payment/registration-payment.component';


@NgModule({
  declarations: [RegistrationPaymentComponent,AssociationPaymentComponent],
  imports: [
    SharedModule,
    PaymentRoutingModule
  ],schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class PaymentModule { }
