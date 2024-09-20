/* **** Aamer Rehman Malik *****/
import { HashLocationStrategy, LocationStrategy } from "@angular/common";
import { HttpClientModule, HTTP_INTERCEPTORS } from "@angular/common/http";
import { CUSTOM_ELEMENTS_SCHEMA, NgModule, APP_INITIALIZER } from "@angular/core";
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { BrowserModule, Title } from "@angular/platform-browser";
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { QuillModule } from 'ngx-quill';
import { AppLayoutComponent } from "./app-layout/app-layout.component";
import { AppRoutingModule } from "./app-routing.module";
import { AppComponent } from "./app.component";
import { CommonSrvService } from "./common-srv.service";
import { ForgotPasswordComponent } from "./forgot-password/forgot-password.component";
import { HomeComponent } from "./home/home.component";
import { LoginComponent } from "./login/login.component";
import { LikePipe } from './pipes/like.pipe';
import { AuthGuardService } from "./security/auth-guard.service";
import { AuthInterceptor } from "./security/auth.interceptor";
import { DialogueService } from './shared/dialogue.service';
import { SharedModule } from './shared/shared.module';
import { AppConfigService } from './app-config.service';
import { MainModule } from './gis/main/main.module';
import { GisLayoutComponent } from './gis-layout/gis-layout.component';
import { BnNgIdleService } from 'bn-ng-idle';
import { TestDialogueTableComponent } from './custom-components/test-dialogue-table/test-dialogue-table.component';
import { TspSignUpComponent } from "./tsp-sign-up/tsp-sign-up.component";
import { SendOTPComponent } from "./send-otp/send-otp.component";
import { PreviewFileComponent } from './custom-components/preview-file/preview-file.component';
import { GeoTaggingComponent } from './custom-components/geo-tagging/geo-tagging.component';
import { TspStatusUpdateComponent } from './custom-components/tsp-status-update/tsp-status-update.component';
import { ProcessApprovedPlanComponent } from './custom-components/process-approved-plan/process-approved-plan.component';
import { ProcessApprovedPlanDialogComponent } from './custom-components/process-approved-plan-dialog/process-approved-plan-dialog.component';
import { ProcessStatusUpdateDialogComponent } from './custom-components/process-status-update-dialog/process-status-update-dialog.component';
import { InitiateAssociationDialogComponent } from './custom-components/initiate-association-dialog/initiate-association-dialog.component';
import { TspAssociationEvaluationDialogueComponent } from './custom-components/tsp-association-evaluation-dialogue/tsp-association-evaluation-dialogue.component';
import { ErrorLogTableComponent } from './custom-components/error-log-table/error-log-table.component';
import { ConfirmDailogComponent } from "./custom-components/confirm-dailog/confirm-dailog.component";
import { ApprovalDialogueBatchComponent } from './custom-components/approval-dialogue-batch/approval-dialogue-batch.component';

const app_initializerFn = (appConfig: AppConfigService) => {
  return () => {
    console.log('Init AppConfig');
    return appConfig.loadAppConfig();
  }
}
@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    TspSignUpComponent,
    SendOTPComponent,
    ForgotPasswordComponent,
    AppLayoutComponent,
    HomeComponent,
    GisLayoutComponent,
    TestDialogueTableComponent,
    PreviewFileComponent,
    GeoTaggingComponent,
    ConfirmDailogComponent,
    TspStatusUpdateComponent,
    ProcessApprovedPlanComponent,
    ProcessApprovedPlanDialogComponent,
    ProcessStatusUpdateDialogComponent,
    InitiateAssociationDialogComponent,
    TspAssociationEvaluationDialogueComponent,
    ErrorLogTableComponent,
    ApprovalDialogueBatchComponent,
  ],
  imports: [
    BrowserModule,
    SharedModule,
    MainModule,
    //MaterialModule,
    //FormsModule,
    //ReactiveFormsModule,
    BrowserAnimationsModule,
    HttpClientModule,
    AppRoutingModule,
    //NgGroupByPipeModule,
    //NgWherePipeModule,
    //NgOrderByPipeModule,
    //NgxMaterialTimepickerModule,
    QuillModule.forRoot()
  ],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  providers: [
    Title,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true
    },
    AuthGuardService, {
      provide: MatDialogRef,
      useValue: {}
    },
    DialogueService,
    MatDialog,
    CommonSrvService,
    LikePipe,
    //{ provide: MAT_MOMENT_DATE_ADAPTER_OPTIONS, useValue: { useUtc: true } },
    //{ provide: MAT_DATE_FORMATS, useValue: DateFormat },
    {
      provide: LocationStrategy,
      useClass: HashLocationStrategy
    },
    AppConfigService,
    {
      provide: APP_INITIALIZER,
      useFactory: app_initializerFn,
      multi: true,
      deps: [AppConfigService]
    },
    BnNgIdleService
  ],
  bootstrap: [AppComponent],
})
export class AppModule { }
