import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuardService } from '../../app/security/auth-guard.service';
import { GeneratePrnCompletionComponent } from './generate-prn-completion/generate-prn-completion.component';
import { GenerateGuruRecommendationNoteComponent } from './generate-guru-recommendation-note/generate-guru-recommendation-note.component';
import { GeneratePrnFinalComponent } from './generate-prn-final/generate-prn-final.component';
import { GenerateTrnComponent } from './generate-trn/generate-trn.component';
import { GenerateSrnCourseraComponent } from './generate-srn-coursera/generate-srn-coursera.component';

const routes: Routes = [
  {
    path: 'generate-prn-completion',
    component: GeneratePrnCompletionComponent,
    canActivate: [AuthGuardService],
    data: {
      icon: 'verified_user',
      inMenu: true,
      title: 'Generate PRN Completion',
    },
  },
  {
    path: 'generate-guru-recommendation-note',
    component: GenerateGuruRecommendationNoteComponent,
    canActivate: [AuthGuardService],
    data: {
      icon: 'verified_user',
      inMenu: true,
      title: 'Generate Guru Payment Note',
    },
  },
  {
    path: 'generate-prn-final',
    component: GeneratePrnFinalComponent,
    canActivate: [AuthGuardService],
    data: {
      icon: 'verified_user',
      inMenu: true,
      title: 'Generate PRN Final',
    },
  },
  {
    path: 'generate-trn',
    component: GenerateTrnComponent,
    canActivate: [AuthGuardService],
    data: {
      icon: 'verified_user',
      inMenu: true,
      title: 'Generate TRN',
    },
  },
  {
    path: 'generate-srn-coursera',
    component: GenerateSrnCourseraComponent,
    canActivate: [AuthGuardService],
    data: {
      icon: 'verified_user',
      inMenu: true,
      title: 'Generate Coursera SRN',
    },
  },
];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class GenerationRouting { }
