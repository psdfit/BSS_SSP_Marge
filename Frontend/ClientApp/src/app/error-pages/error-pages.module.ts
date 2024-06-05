import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ErrorPagesRoutingModule } from './error-pages-routing.module';
import { AccessDeniedComponent } from './access-denied/access-denied.component';


@NgModule({
  declarations: [AccessDeniedComponent],
  imports: [
    CommonModule,
    ErrorPagesRoutingModule
  ]
})
export class ErrorPagesModule { }
