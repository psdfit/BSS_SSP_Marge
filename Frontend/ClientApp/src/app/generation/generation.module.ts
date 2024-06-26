import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../shared/shared.module';
import { GenerationRouting } from './generation.routing';
import { GeneratePrnCompletionComponent } from './generate-prn-completion/generate-prn-completion.component';
import { GeneratePrnFinalComponent } from './generate-prn-final/generate-prn-final.component';
import { GenerateTrnComponent } from './generate-trn/generate-trn.component';

@NgModule({
  declarations: [
    GeneratePrnCompletionComponent,
    GeneratePrnFinalComponent,
    GenerateTrnComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    GenerationRouting
  ],
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class GenerationModule {}
