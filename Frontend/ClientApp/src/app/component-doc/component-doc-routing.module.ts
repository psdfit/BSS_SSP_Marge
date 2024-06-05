import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { AuthGuardService } from "../security/auth-guard.service";
import { ButtonsComponent } from "./buttons/buttons.component";
import { ModuleCreateComponent } from "./module-create/module-create.component";
import { DialogueComponent } from "./dialogue/dialogue.component";
import { ReactiveFormComponent } from "./reactive-form/reactive-form.component";
import { TablesComponent } from "./tables/tables.component";
import { IconsComponent } from "./icons/icons.component";
import { ExcelExportComponent } from "./excel-export/excel-export.component";
import { CrudComponent } from "./crud/crud.component";

const routes: Routes = [
  {
    path: "buttons",
    component: ButtonsComponent,
    canActivate: [AuthGuardService],
    data: {
      icon: "verified_user",
      inMenu: true,
      title: "Buttons",
    },
  },
  {
    path: "create-module",
    component: ModuleCreateComponent,
    canActivate: [AuthGuardService],
    data: {
      icon: "verified_user",
      inMenu: true,
      title: "ModuleCreate",
    },
  },
  {
    path: "dialogue",
    component: DialogueComponent,
    canActivate: [AuthGuardService],
    data: {
      icon: "verified_user",
      inMenu: true,
      title: "Dialogue",
    },
  },
  {
    path: "reactive-form",
    component: ReactiveFormComponent,
    canActivate: [AuthGuardService],
    data: {
      icon: "verified_user",
      inMenu: true,
      title: "Form",
    },
  },
  {
    path: "tables",
    component: TablesComponent,
    canActivate: [AuthGuardService],
    data: {
      icon: "verified_user",
      inMenu: true,
      title: "Tables",
    },
  },
  {
    path: "icons",
    component: IconsComponent,
    canActivate: [AuthGuardService],
    data: {
      icon: "verified_user",
      inMenu: true,
      title: "Buttons",
    },
  },
  {
    path: "excel-export",
    component: ExcelExportComponent,
    canActivate: [AuthGuardService],
    data: {
      icon: "verified_user",
      inMenu: true,
      title: "ExcelExport",
    },
  },
  {
    path: "crud",
    component: CrudComponent,
    canActivate: [AuthGuardService],
    data: {
      icon: "verified_user",
      inMenu: true,
      title: "Crud",
    },
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ComponentDocRoutingModule {}
