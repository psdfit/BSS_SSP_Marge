/* **** Aamer Rehman Malik *****/
import { CommonModule } from '@angular/common';
import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { SharedModule } from '../shared/shared.module';
import { AdvanceSearchRoutingModule } from './search-module-routing';
import { SearchComponent } from './advance-search/search.component';
import { TraineeProfileComponent } from './trainee-profile/trainee-profile.component';
import { InstructorProfileComponent } from './instructor-profile/instructor-profile.component';
import { ClassDetailComponent } from './class-detail/class-detail.component';
import { TSPDetailComponent } from './tsp-detail/tsp-detail.component';
import { SchemeDetailComponent } from './scheme-detail/scheme-detail.component';
import { MPRDetailComponent } from './mpr-detail/mpr-detail.component';
import { PRNDetailComponent } from './prn-detail/prn-detail.component';
import { SRNDetailComponent } from './srn-detail/srn-detail.component';
import { InvoiceDetailComponent } from './invoice-detail/invoice-detail.component';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatInputModule } from '@angular/material/input';
import { MatTableModule } from '@angular/material/table';

@NgModule({
  declarations: [SearchComponent, TraineeProfileComponent, InstructorProfileComponent, ClassDetailComponent, TSPDetailComponent, SchemeDetailComponent, MPRDetailComponent, PRNDetailComponent, SRNDetailComponent, InvoiceDetailComponent],
  imports: [CommonModule, SharedModule, AdvanceSearchRoutingModule, MatToolbarModule, MatInputModule, MatTableModule],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
})
export class AdvanceSearchModule { }
