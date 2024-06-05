/* **** Aamer Rehman Malik *****/
import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ApprovalsComponent } from './approvals/approvals.component';
import { ApprovalsRoutingModule } from './approvals-routing';
import { SharedModule } from '../shared/shared.module';
import { SrnApprovalsComponent } from './srn-approvals/srn-approvals.component';
import { InvoiceApprovalsComponent } from './invoice-approvals/invoice-approvals.component';
import { TradeApprovalsComponent } from './trade-approvals/trade-approvals.component';
import { RTPApprovalsComponent } from './rtp-approvals/rtp-approvals.component';
import { PrnApprovalsComponent } from './prn-approvals/prn-approvals.component';
import { PoApprovalsComponent } from './po-approvals/po-approvals.component';
import { TrnApprovalsComponent } from './trn-approvals/trn-approvals.component';
import { SrnApprovalsDialogueComponent } from './srn-approvals-dialogue/srn-approvals-dialogue.component';
import { SchemeChangeRequestApprovalsComponent } from './cr-scheme-approvals/cr-scheme-approvals.component';
import { DeletionApprovalsComponent } from './deletion-approvals/deletion-approvals.component';
import { TSPChangeRequestApprovalsComponent } from './cr-tsp-approvals/cr-tsp-approvals.component';
import { ClassChangeRequestApprovalsComponent } from './cr-class-approvals/cr-class-approvals.component';
import { TraineeChangeRequestApprovalsComponent } from './cr-trainee-approvals/cr-trainee-approvals.component';
import { InstructorChangeRequestApprovalsComponent } from './cr-instructor-approvals/cr-instructor-approvals.component';
import { NewInstructorRequestApprovalsComponent } from './cr-new-instructor-approvals/cr-new-instructor-approvals.component';
import { InceptionReportChangeRequestApprovalsComponent } from './cr-inception-report-approvals/cr-inception-report-approvals.component';
import { ReplaceInstructorChangeRequestApprovalsComponent } from './cr-replace-instructor-approvals/cr-replace-instructor-approvals.component';
import { VerifiedTraineeChangeRequestApprovalsComponent } from './cr-verified-trainee-approvals/cr-verified-trainee-approvals.component';
import { ClassDatesChangeRequestApprovalsComponent } from './cr-class-dates-approvals/cr-class-dates-approvals.component';
import { TestComponent } from './test/test.component';
import { RegistrationApprovalComponent } from './registration-approval/registration-approval.component';
import { AnnualPlanApprovalComponent } from './annual-plan-approval/annual-plan-approval.component';

@NgModule({
  declarations: [ApprovalsComponent, SrnApprovalsComponent, DeletionApprovalsComponent, InvoiceApprovalsComponent,
    TradeApprovalsComponent, PrnApprovalsComponent, PoApprovalsComponent, RTPApprovalsComponent, TrnApprovalsComponent,
    SrnApprovalsDialogueComponent, SchemeChangeRequestApprovalsComponent, TSPChangeRequestApprovalsComponent,
    ClassChangeRequestApprovalsComponent, TraineeChangeRequestApprovalsComponent, InstructorChangeRequestApprovalsComponent,
    InceptionReportChangeRequestApprovalsComponent, NewInstructorRequestApprovalsComponent,
    ReplaceInstructorChangeRequestApprovalsComponent, VerifiedTraineeChangeRequestApprovalsComponent,
    ClassDatesChangeRequestApprovalsComponent, TestComponent,RegistrationApprovalComponent,AnnualPlanApprovalComponent],
  imports: [
    CommonModule,
    SharedModule,
    ApprovalsRoutingModule
  ], schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class ApprovalsModule { }
