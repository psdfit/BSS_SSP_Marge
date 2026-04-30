import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuardService } from '../../app/security/auth-guard.service';
import { GeneratePrnCompletionComponent } from './generate-prn-completion/generate-prn-completion.component';
import { GenerateGuruRecommendationNoteComponent } from './generate-guru-recommendation-note/generate-guru-recommendation-note.component';
import { GeneratePrnFinalComponent } from './generate-prn-final/generate-prn-final.component';
import { GenerateTrnComponent } from './generate-trn/generate-trn.component';
import { GenerateSrnCourseraComponent } from './generate-srn-coursera/generate-srn-coursera.component';
import { GeneratePvrnComponent } from './generate-pvrn/generate-pvrn.component';
import { GenerateMrnComponent } from './generate-mrn/generate-mrn.component';
import { GeneratePcrnComponent } from './generate-pcrn/generate-pcrn.component';
import { GenerateOtrnComponent } from './generate-otrn/generate-otrn.component';
import { GenerateTakamolRecommnedationNoteComponent } from './generate-takamol-recommnedation-note/generate-takamol-recommnedation-note.component';

import { GenerateOjtComponent } from './generate-ojt/generate-ojt.component';
import { GenerateInvoiceComponent } from './generate-invoice/generate-invoice.component';
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
  {
    path: 'generate-pvrn',
    component: GeneratePvrnComponent,
    canActivate: [AuthGuardService],
    data: {
      icon: 'verified_user',
      inMenu: true,
      title: 'Generate PVRN',
    },
  },
  {
    path: 'generate-mrn',
    component: GenerateMrnComponent,
    canActivate: [AuthGuardService],
    data: {
      icon: 'verified_user',
      inMenu: true,
      title: 'Generate MRN',
    },
  },
  {
    path: 'generate-pcrn',
    component: GeneratePcrnComponent,
    canActivate: [AuthGuardService],
    data: {
      icon: 'verified_user',
      inMenu: true,
      title: 'Generate PCRN',
    },
  },
  {
    path: 'generate-otrn',
    component: GenerateOtrnComponent,
    canActivate: [AuthGuardService],
    data: {
      icon: 'verified_user',
      inMenu: true,
      title: 'Generate OTRN',
    },
  },
  {
    path: 'generate-takamol-recommnedation-note',
    component: GenerateTakamolRecommnedationNoteComponent,
    canActivate: [AuthGuardService],
    data: {
      icon: 'verified_user',
      inMenu: true,
      title: 'Generate Takamol Recommnedation Note',
    },
  },
  {
    path: 'generate-ojt',
    component: GenerateOjtComponent,
    canActivate: [AuthGuardService],
    data: {
      icon: 'verified_user',
      inMenu: true,
      title: 'Generate OJT',
    },
  },
   {
    path: 'generate-invoice',
    component: GenerateInvoiceComponent,
    canActivate: [AuthGuardService],
    data: {
      icon: 'verified_user',
      inMenu: true,
      title: 'Generate Invoice',
    },
  },
];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class GenerationRouting { }
