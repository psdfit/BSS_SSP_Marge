import { CriteriaTemplateApprovalComponent } from './criteria-template-approval/criteria-template-approval.component';
import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { ApprovalsComponent } from "./approvals/approvals.component";
import { AuthGuardService } from "../../app/security/auth-guard.service";
import { SrnApprovalsComponent } from "./srn-approvals/srn-approvals.component";
import { VrnApprovalsComponent } from "./vrn-approvals/vrn-approvals.component";
import { TradeApprovalsComponent } from "./trade-approvals/trade-approvals.component";
import { RTPApprovalsComponent } from "./rtp-approvals/rtp-approvals.component";
import { PrnApprovalsComponent } from "./prn-approvals/prn-approvals.component";
import { InvoiceApprovalsComponent } from "./invoice-approvals/invoice-approvals.component";
import { PoApprovalsComponent } from "./po-approvals/po-approvals.component";
import { TrnApprovalsComponent } from "./trn-approvals/trn-approvals.component";
import { SchemeChangeRequestApprovalsComponent } from "./cr-scheme-approvals/cr-scheme-approvals.component";
import { DeletionApprovalsComponent } from "./deletion-approvals/deletion-approvals.component";
import { TSPChangeRequestApprovalsComponent } from "./cr-tsp-approvals/cr-tsp-approvals.component";
import { ClassChangeRequestApprovalsComponent } from "./cr-class-approvals/cr-class-approvals.component";
import { TraineeChangeRequestApprovalsComponent } from "./cr-trainee-approvals/cr-trainee-approvals.component";
import { VerifiedTraineeChangeRequestApprovalsComponent } from "./cr-verified-trainee-approvals/cr-verified-trainee-approvals.component";
import { InstructorChangeRequestApprovalsComponent } from "./cr-instructor-approvals/cr-instructor-approvals.component";
import { InceptionReportChangeRequestApprovalsComponent } from "./cr-inception-report-approvals/cr-inception-report-approvals.component";
import { NewInstructorRequestApprovalsComponent } from "./cr-new-instructor-approvals/cr-new-instructor-approvals.component";
import { ReplaceInstructorChangeRequestApprovalsComponent } from "./cr-replace-instructor-approvals/cr-replace-instructor-approvals.component";
import { ClassDatesChangeRequestApprovalsComponent } from "./cr-class-dates-approvals/cr-class-dates-approvals.component";
import { TestComponent } from "./test/test.component";
import { RegistrationApprovalComponent } from "./registration-approval/registration-approval.component";
import { AnnualPlanApprovalComponent } from "./annual-plan-approval/annual-plan-approval.component";
import { GurnApprovalsComponent } from "./gurn-approvals/gurn-approvals.component";

const routes: Routes = [
  {
    path: "appendix",
    component: ApprovalsComponent,
    canActivate: [AuthGuardService],
    data: {
      icon: "verified_user",
      inMenu: true,
      title: "Approval Requests",
    },
  },
  {
    path: "srn-approvals",
    component: SrnApprovalsComponent,
    canActivate: [AuthGuardService],
    data: {
      icon: "verified_user",
      inMenu: true,
      title: "SRN Approval Requests",
    },
  },
  {
    path: "gurn-approvals",
    component: GurnApprovalsComponent,
    canActivate: [AuthGuardService],
    data: {
      icon: "verified_user",
      inMenu: true,
      title: "GURN Approval Requests",
    },
  },
  {
    path: "vrn-approvals",
    component: VrnApprovalsComponent,
    canActivate: [AuthGuardService],
    data: {
      icon: "verified_user",
      inMenu: true,
      title: "VRN Approval Requests",
    },
  },
  {
    path: "trade-approvals",
    component: TradeApprovalsComponent,
    canActivate: [AuthGuardService],
    data: {
      icon: "verified_user",
      inMenu: true,
      title: "Trade Approval Requests",
    },
  },
  {
    path: "cr-scheme-approvals",
    component: SchemeChangeRequestApprovalsComponent,
    canActivate: [AuthGuardService],
    data: {
      icon: "verified_user",
      inMenu: true,
      title: "Scheme Change Request Approval",
    },
  },
  {
    path: "cr-tsp-approvals",
    component: TSPChangeRequestApprovalsComponent,
    canActivate: [AuthGuardService],
    data: {
      icon: "verified_user",
      inMenu: true,
      title: "TSP Change Request Approval",
    },
  },
  {
    path: "cr-class-approvals",
    component: ClassChangeRequestApprovalsComponent,
    canActivate: [AuthGuardService],
    data: {
      icon: "verified_user",
      inMenu: true,
      title: "Class Change Request Approval",
    },
  },
  {
    path: "cr-class-dates-approvals",
    component: ClassDatesChangeRequestApprovalsComponent,
    canActivate: [AuthGuardService],
    data: {
      icon: "verified_user",
      inMenu: true,
      title: "Class Dates Change Request Approval",
    },
  },
  {
    path: "cr-trainee-approvals",
    component: TraineeChangeRequestApprovalsComponent,
    canActivate: [AuthGuardService],
    data: {
      icon: "verified_user",
      inMenu: true,
      title: "Trainee Change Request Approval",
    },
  },
  {
    path: "cr-verified-trainee-approvals",
    component: VerifiedTraineeChangeRequestApprovalsComponent,
    canActivate: [AuthGuardService],
    data: {
      icon: "verified_user",
      inMenu: true,
      title: "Trainee Change Request Approval",
    },
  },
  {
    path: "cr-instructor-approvals",
    component: InstructorChangeRequestApprovalsComponent,
    canActivate: [AuthGuardService],
    data: {
      icon: "verified_user",
      inMenu: true,
      title: "Instructor Change Request Approval",
    },
  },
  {
    path: "cr-new-instructor-approvals",
    component: NewInstructorRequestApprovalsComponent,
    canActivate: [AuthGuardService],
    data: {
      icon: "verified_user",
      inMenu: true,
      title: "New Instructor Request Approval",
    },
  },
  {
    path: "cr-replace-instructor-approvals",
    component: ReplaceInstructorChangeRequestApprovalsComponent,
    canActivate: [AuthGuardService],
    data: {
      icon: "verified_user",
      inMenu: true,
      title: "Replace Instructor Request Approval",
    },
  },
  {
    path: "cr-inception-approvals",
    component: InceptionReportChangeRequestApprovalsComponent,
    canActivate: [AuthGuardService],
    data: {
      icon: "verified_user",
      inMenu: true,
      title: "Inception Report Change Request Approval",
    },
  },
  {
    path: "rtp-approvals",
    component: RTPApprovalsComponent,
    canActivate: [AuthGuardService],
    data: {
      icon: "verified_user",
      inMenu: true,
      title: "Trade Approval Requests",
    },
  },
  {
    path: "prn-approvals",
    component: PrnApprovalsComponent,
    canActivate: [AuthGuardService],
    data: {
      icon: "verified_user",
      inMenu: true,
      title: "PRN Approval Requests",
    },
  },
  {
    path: "invoice-approvals",
    component: InvoiceApprovalsComponent,
    canActivate: [AuthGuardService],
    data: {
      icon: "verified_user",
      inMenu: true,
      title: "Invoice Approval Requests",
    },
  },
  {
    path: "po-approvals",
    component: PoApprovalsComponent,
    canActivate: [AuthGuardService],
    data: {
      icon: "verified_user",
      inMenu: true,
      title: "PurchaseOrder Approval Requests",
    },
  },
  {
    path: "trn-approvals",
    component: TrnApprovalsComponent,
    canActivate: [AuthGuardService],
    data: {
      icon: "verified_user",
      inMenu: true,
      title: "TRN Approval Requests",
    },
  },
  {
    path: "deletion-approvals",
    component: DeletionApprovalsComponent,
    canActivate: [AuthGuardService],
    data: {
      icon: "verified_user",
      inMenu: true,
      title: "Canelation Approval Requests",
    },
  },
  {
    path: "test",
    component: TestComponent,
    // , canActivate: [AuthGuardService]
    data: {
      icon: "verified_user",
      inMenu: true,
      title: "Test",
    },
  },
  {
    path: "registration-approval",
    component: RegistrationApprovalComponent,
    canActivate: [AuthGuardService],
    data: {
      icon: "verified_user",
      inMenu: true,
      title: "Registration Approval",
    },
  },
  {
    path: "program-design-approval",
    component: AnnualPlanApprovalComponent,
    canActivate: [AuthGuardService],
    data: {
      icon: "verified_user",
      inMenu: true,
      title: "Program Design",
    },
  },
  {
    path: "criteria-template-approval",
    component: CriteriaTemplateApprovalComponent,
    canActivate: [AuthGuardService],
    data: {
      icon: "verified_user",
      inMenu: true,
      title: "Criteria Template",
    },
  },
];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ApprovalsRoutingModule {}
