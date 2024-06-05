
import { CommonModule } from '@angular/common';
import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from "@angular/core";
import { SharedModule } from "../shared/shared.module";
import { ComplaintModuleRoutingModule } from './complaint-module-routing.module';
import { ComplaintHistoryDialogueComponent } from './complaint-history-dialogue/complaint-history-dialogue.component';
import { ComplaintComponent } from './complaint/complaint.component';
import { ComplaintHandlingComponent } from './complaint-handling/complaint-handling.component';
import { complaintUserComponent } from './complaintUser/complaintUser.component';
@NgModule({
  declarations: [ComplaintComponent,ComplaintHistoryDialogueComponent, ComplaintHandlingComponent,complaintUserComponent],
  imports: [
    CommonModule,SharedModule,
    ComplaintModuleRoutingModule
  ],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  
})
export class ComplaintModuleModule { }
