import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HomeComponent } from './home.component';
import { MaterialModule } from '../../shared/app.material.module';
import { SharedModule } from 'src/app/shared/shared.module';



@NgModule({
  declarations: [HomeComponent],


  imports: [
    CommonModule, SharedModule, MaterialModule
  ],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],


})
export class HomeModule { }
