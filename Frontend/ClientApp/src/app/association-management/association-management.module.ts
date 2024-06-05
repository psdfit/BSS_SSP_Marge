import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";

import { AssociationManagementRoutingModule } from "./association-management-routing.module";
import { InitiateAssociationComponent } from "./initiate-association/initiate-association.component";
import { AssociationEvaluationComponent } from "./association-evaluation/association-evaluation.component";
import { TspAssignmentComponent } from "./tsp-assignment/tsp-assignment.component";
import { AssociationDetailComponent } from "./association-detail/association-detail.component";
import { SharedModule } from "../shared/shared.module";
import { AssociationSubmissionComponent } from './association-submission/association-submission.component';

@NgModule({
  declarations: [
    InitiateAssociationComponent,
    AssociationEvaluationComponent,
    TspAssignmentComponent,
    AssociationDetailComponent,
    AssociationSubmissionComponent,
  ],
  imports: [SharedModule, AssociationManagementRoutingModule],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
})
export class AssociationManagementModule {}
