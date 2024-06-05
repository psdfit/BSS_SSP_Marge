/* **** Aamer Rehman Malik *****/
import { CommonModule, TitleCasePipe } from "@angular/common";
import { NgModule } from "@angular/core";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { MAT_MOMENT_DATE_ADAPTER_OPTIONS } from "@angular/material-moment-adapter";
import { MAT_DATE_FORMATS } from "@angular/material/core";
import { IMaskModule, IMaskPipe } from "angular-imask";
import {
  NgFirstOrDefaultPipeModule,
  NgGroupByPipeModule,
  NgOrderByPipeModule,
  NgWherePipeModule,
} from "angular-pipes";
import { NgxMaterialTimepickerModule } from "ngx-material-timepicker";
import { QuillModule } from "ngx-quill";
import { ApprovalDialogueComponent } from "../custom-components/approval-dialogue/approval-dialogue.component";
import { ClassMonthviewComponent } from "../custom-components/class-monthview/class-monthview.component";
import { ExportConfirmDialogueComponent } from "../custom-components/export-confirm-dialogue/export-confirm-dialogue.component";
import { FileUploadComponent } from "../custom-components/file-upload/file-upload.component";
import { PrintpreviewComponent } from "../custom-components/printpreview/printpreview.component";
import { SnackBarComponent } from "../custom-components/snack-bar/snack-bar.component";
import { TStatusHistoryDialogueComponent } from "../custom-components/t-status-history-dialogue/t-status-history-dialogue.component";
import { TSPKAMHistoryDialogueComponent } from "../custom-components/tsp-kam-history-dialogue/tsp-kam-history-dialogue.component";
import { CRTraineeHistoryDialogueComponent } from "../custom-components/cr-trianee-history-dialogue/cr-trianee-history-dialogue.component";
import { TSPPendingClassesDialogueComponent } from "../custom-components/tsp-pending-classes-dialogue/tsp-pending-classes-dialogue.component";
import { DraftTraineeDialogueComponent } from "../custom-components/draft-trainee-dialogue/draft-trainee-dialogue.component";
import { KAMPendingClassesDialogueComponent } from "../custom-components/kam-pending-classes-dialogue/kam-pending-classes-dialogue.component";
import { KAMDeadlinesDialogComponent } from "../custom-components/kam-deadlines-dialog/kam-deadlines-dialog.component";
import { DecimelOnlyDirective } from "../directives/decimel-only.directive";
//import { GroupByPipe } from './GroupBy.pipe';
import { NumberOnlyDirective } from "../directives/number-only.directive";
import { NumberPercentDirective } from "../directives/percent-only.directive";
import { TitleCaseDirective } from "../directives/title-case.directive";
import { UrduTextDirective } from "../directives/urdu-text.directive";
import { ChangePasswordComponent } from "../master-data/change-password/change-password.component";
import { VisitPlanDialogComponent } from "../master-sheet/visit-plan-dialog/visit-plan-dialog.component";
import { PSPBatchDialogueComponent } from "../psp/psp-batch-dialogue/psp-batch-dialogue.component";
import { LikePipe } from "../pipes/like.pipe";
import { SafePipe } from "../pipes/safe.pipe";
import { MaterialModule } from "./app.material.module";
import { AlphaNumericDirective } from "../directives/alphanumeric.directive";
import { AlphaDirective } from "../directives/alpha.directive";
import { PhoneNumberPipe } from "../pipes/phone-number.pipe";
import { DocumentDialogComponent } from "../custom-components/document-dialog/document-dialog.component";
import { NgbModule } from "@ng-bootstrap/ng-bootstrap";
import { ScrollingModule } from "@angular/cdk/scrolling";
import { InfiniteScrollModule } from "ngx-infinite-scroll";
import { NoDoubleClickDirective } from "../directives/no-double-click.directive";
export const DateFormat = {
  parse: {
    dateInput: "input",
  },
  display: {
    dateInput: "DD-MMM-YYYY",
    monthYearLabel: "MMMM YYYY",
    dateA11yLabel: "MM/DD/YYYY",
    monthYearA11yLabel: "MMMM YYYY",
  },
};
@NgModule({
  declarations: [
    NumberOnlyDirective,
    DecimelOnlyDirective,
    NumberPercentDirective,
    DocumentDialogComponent,
    UrduTextDirective,
    PrintpreviewComponent,
    ChangePasswordComponent,
    SnackBarComponent,
    PrintpreviewComponent,
    FileUploadComponent,
    SafePipe,
    LikePipe,
    VisitPlanDialogComponent,
    PSPBatchDialogueComponent,
    TitleCaseDirective,
    ApprovalDialogueComponent,
    TStatusHistoryDialogueComponent,
    ExportConfirmDialogueComponent,
    ClassMonthviewComponent,
    AlphaNumericDirective,
    AlphaDirective,
    TSPKAMHistoryDialogueComponent,
    PhoneNumberPipe,
    TSPPendingClassesDialogueComponent,
    DraftTraineeDialogueComponent,
    CRTraineeHistoryDialogueComponent,
    KAMPendingClassesDialogueComponent,
    KAMDeadlinesDialogComponent,
    NoDoubleClickDirective,
  ],
  imports: [
    InfiniteScrollModule,
    CommonModule,
    MaterialModule,
    FormsModule,
    ReactiveFormsModule,
    NgGroupByPipeModule,
    NgWherePipeModule,
    NgFirstOrDefaultPipeModule,
    NgOrderByPipeModule,
    NgxMaterialTimepickerModule,
    IMaskModule,
    QuillModule.forRoot(),
    NgbModule,
    ScrollingModule,
  ],
  providers: [
    { provide: MAT_MOMENT_DATE_ADAPTER_OPTIONS, useValue: { useUtc: true } },
    { provide: MAT_DATE_FORMATS, useValue: DateFormat },
    TitleCasePipe,
    IMaskPipe,
  ],
  exports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MaterialModule,
    NgxMaterialTimepickerModule,
    NgGroupByPipeModule,
    NgWherePipeModule,
    NgOrderByPipeModule,
    NumberOnlyDirective,
    DecimelOnlyDirective,
    NumberPercentDirective,
    UrduTextDirective,
    PrintpreviewComponent,
    ChangePasswordComponent,
    SnackBarComponent,
    PrintpreviewComponent,
    FileUploadComponent,
    SafePipe,
    LikePipe,
    VisitPlanDialogComponent,
    PSPBatchDialogueComponent,
    IMaskModule,
    ScrollingModule,
    NgFirstOrDefaultPipeModule,
    TitleCaseDirective,
    QuillModule,
    ApprovalDialogueComponent,
    TStatusHistoryDialogueComponent,
    ExportConfirmDialogueComponent,
    ClassMonthviewComponent,
    AlphaNumericDirective,
    AlphaDirective,
    TSPKAMHistoryDialogueComponent,
    PhoneNumberPipe,
    DocumentDialogComponent,
    NgbModule,
    TSPPendingClassesDialogueComponent,
    DraftTraineeDialogueComponent,
    CRTraineeHistoryDialogueComponent,
    KAMPendingClassesDialogueComponent,
    KAMDeadlinesDialogComponent,
    NoDoubleClickDirective,
  ],
})
export class SharedModule {}
