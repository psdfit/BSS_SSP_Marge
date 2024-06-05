import {NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { SharedModule } from '../shared/shared.module';


import { ComponentDocRoutingModule } from "./component-doc-routing.module";
import { ButtonsComponent } from "./buttons/buttons.component";
import { ModuleCreateComponent } from "./module-create/module-create.component";
import { DialogueComponent } from "./dialogue/dialogue.component";
import { TablesComponent } from "./tables/tables.component";
import { ReactiveFormComponent } from "./reactive-form/reactive-form.component";
import { IconsComponent } from "./icons/icons.component";
import { ExcelExportComponent } from "./excel-export/excel-export.component";
import { CrudComponent } from "./crud/crud.component";
@NgModule({
  declarations: [
    ButtonsComponent,
    ModuleCreateComponent,
    DialogueComponent,
    TablesComponent,
    ReactiveFormComponent,
    IconsComponent,
    ExcelExportComponent,
    CrudComponent,
  ],
  imports: [CommonModule,SharedModule, ComponentDocRoutingModule],
})



export class ComponentDocModule {}
