import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from "@angular/core";
import { CommonModule } from "@angular/common";
import { RegistrationRoutingModule } from "./registration-routing";
import { TraineeComponent } from "./trainee/trainee.component";
import { MaterialModule } from "../shared/app.material.module";
import { SharedModule } from "../shared/shared.module";
import { TraineeVarificationComponent } from "./trainee-verification/trainee-verification.component";
import { TraineeVerificationDialogComponent } from "./trainee-verification-dialog/trainee-verification-dialog.component";
import { TraineeUpdationComponent } from "./trainee-updation/trainee-updation.component";
//import { NgxMaskModule, IConfig } from 'ngx-mask'

//export const options: Partial<IConfig> | (() => Partial<IConfig>);
@NgModule({
  declarations: [
    TraineeComponent,
    TraineeVarificationComponent,
    TraineeVerificationDialogComponent,
    TraineeUpdationComponent,
  ],
  imports: [
    //NgxMaskModule.forRoot()
    CommonModule,
    SharedModule,
    RegistrationRoutingModule,
  ],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
})
export class RegistrationModule {}
