import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BenchmarkingRoutingModule } from './benchmarking-routing';
import { SharedModule } from '../shared/shared.module';
import { BenchmarkingVerificationComponent } from './benchmarking-verification/benchmarking-verification.component';
import { BenchmarkingComponent } from './benchmarking/benchmarking.component';



@NgModule({
    declarations: [BenchmarkingComponent, BenchmarkingVerificationComponent],
  imports: [
    CommonModule

    , SharedModule
      , BenchmarkingRoutingModule
  ], schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class BenchmarkingModule { }
