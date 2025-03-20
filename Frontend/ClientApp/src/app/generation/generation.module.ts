import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../shared/shared.module';
import { GenerationRouting } from './generation.routing';
import { GeneratePrnCompletionComponent } from './generate-prn-completion/generate-prn-completion.component';
import { GenerateGuruRecommendationNoteComponent } from './generate-guru-recommendation-note/generate-guru-recommendation-note.component';
import { GeneratePrnFinalComponent } from './generate-prn-final/generate-prn-final.component';
import { GenerateTrnComponent } from './generate-trn/generate-trn.component';
import { GenerateSrnCourseraComponent } from './generate-srn-coursera/generate-srn-coursera.component';
import { GenerateMrnComponent } from './generate-mrn/generate-mrn.component';
import { GeneratePvrnComponent } from './generate-pvrn/generate-pvrn.component';
import { GeneratePcrnComponent } from './generate-pcrn/generate-pcrn.component';
import { GenerateOtrnComponent } from './generate-otrn/generate-otrn.component';

@NgModule({
  declarations: [
    GeneratePrnCompletionComponent,
    GeneratePrnFinalComponent,
    GenerateTrnComponent,
    GenerateSrnCourseraComponent,
    GenerateGuruRecommendationNoteComponent,
    GenerateMrnComponent,
    GeneratePvrnComponent,
    GeneratePcrnComponent,
    GenerateOtrnComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    GenerationRouting
  ],
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class GenerationModule {}
