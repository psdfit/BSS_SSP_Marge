import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";

import { InternationalPlacementRoutingModule } from "./international-placement-docs-routing.module";
import { SharedModule } from "../shared/shared.module";

import { VisaStampingComponent } from "./visa-stamping/visa-stamping.component";
import { VisaStampingDocDialogComponent } from "./visa-stamping-doc-dialog/visa-stamping-doc-dialog.component";
import { MedicalCostComponent } from "./medical-cost/medial-cost.component";
import { MedicalCostDocDialogComponent } from "./medical-cost-doc-dialog/medial-cost-doc-dialog.component";
import { PrometricCostComponent } from "./prometric-cost/prometric-cost.component";
import { PrometricCostDocDialogComponent } from "./prometric-cost-doc-dialog/prometric-cost-doc-dialog.component";
import { OtherTrainingCostComponent } from "./other-training-cost/other-training-cost.component";
import { OtherTrainingCostDocDialogComponent } from "./other-training-cost-doc-dialog/other-training-cost-doc-dialog.component";

@NgModule({
  declarations: [

    VisaStampingComponent,
    MedicalCostComponent,
    PrometricCostComponent,
    OtherTrainingCostComponent,

    VisaStampingDocDialogComponent,
    MedicalCostDocDialogComponent,
    PrometricCostDocDialogComponent,
    OtherTrainingCostDocDialogComponent,

  ],
  imports: [CommonModule, SharedModule, InternationalPlacementRoutingModule],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
})
export class InternationalPlacementModule { }
