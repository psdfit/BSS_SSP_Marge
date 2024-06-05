import { Component, OnInit, ViewChild } from "@angular/core";
import {} from "@angular/forms";
import { MatPaginator } from "@angular/material/paginator";
import {
  FormArray,
  FormBuilder,
  FormGroup,
  FormGroupDirective,
  Validators,
  FormsModule,
  ReactiveFormsModule,
} from "@angular/forms";

import * as XLSX from "xlsx";
import { environment } from "../../../environments/environment";
import { CommonSrvService } from "../../common-srv.service";
import { UserRightsModel } from "../../master-data/users/users.component";
import { ModelBase } from "../../shared/ModelBase";
import { MatStepper } from "@angular/material/stepper";
import { MatTableDataSource } from "@angular/material/table";
import { Observable } from "rxjs";
import { MatTabGroup } from "@angular/material/tabs";
import { IMaskPipe } from "angular-imask";
import { Route, Router } from "@angular/router";
import { DatePipe } from "@angular/common";
import { Workbook } from "exceljs";
import * as fs from "file-saver";
import { MatSort } from "@angular/material/sort";

@Component({
  selector: "app-infrastructure",
  templateUrl: "./infrastructure.component.html",
  styleUrls: ["./infrastructure.component.scss"],
})
export class InfrastructureComponent implements OnInit {
  InfrastructureFileForm: FormGroup;
  SheetName: string = "Infrastructure";
  InfrastructureForm: FormGroup;
  InfrastructureArray: FormArray;
  error: String;
  Infrastructures: [];
  working: boolean;
  constructor(
    private http: CommonSrvService,
    private _formBuilder: FormBuilder
  ) {}
  ngOnInit(): void {
    this.InfrastructureFileForm = this._formBuilder.group({
      InfrastructureExcel: ["", Validators.required],
    });
    this.InfrastructureForm = this._formBuilder.group({
      InfrastructureArray: this._formBuilder.array([]),
    });
    this.addItem();
    this.GetInfrastructures();
  }
  addItem(): void {
    this.InfrastructureArray = this.InfrastructureForm.get(
      "InfrastructureArray"
    ) as FormArray;
    this.InfrastructureArray.push(
      this.createItem("", "", "", "", 0, 0, 0, 0, 0, 0)
    );
  }
  createItem(
    stream: string,
    scheme: string,
    tsp: string,
    trade: string,
    building: number,
    furniture: number,
    bsoe: number,
    tne: number,
    total: number,
    soso5: number
  ): FormGroup {
    return this._formBuilder.group({
      Stream: stream,
      Scheme: scheme,
      TrainingServiceProvider: tsp,
      Trade: trade,
      Building: building,
      Furniture: furniture,
      BackupSourceOfElectricity: bsoe,
      ToolsAndequipment: tne,
      TotalAm: total,
      ScoreOnScaleOf5: soso5,
    });
  }
  onFileChange(ev: any) {
    debugger;
    let workBook = null;
    let jsonData = null;
    const reader = new FileReader();
    const file = ev.target.files[0];
    reader.onload = (event) => {
      const data = reader.result;
      workBook = XLSX.read(data, { type: "binary" });
      jsonData = workBook.SheetNames.reduce((initial, name) => {
        const sheet = workBook.Sheets[name];
        initial[name] = XLSX.utils.sheet_to_json(sheet);
        return initial;
      }, {});
      const dataString = JSON.stringify(jsonData);
      var array = JSON.parse(dataString)[this.SheetName];
      var infArr = this.InfrastructureForm.get(
        "InfrastructureArray"
      ) as FormArray;
      infArr.clear();
      array.forEach((element) => {
        if (element["Stream"]) {
          var build = Number(element["Building "]) * 100;
          var fur = Number(element["Furniture"]) * 100;
          var backup = Number(element["Backup Source of Electricity"]) * 100;
          var tne = Number(element["Tools and equipment "]) * 100;
          var total = build + fur + backup + tne;
          var sos = total * 0.05;

          this.InfrastructureArray.push(
            this.createItem(
              element["Stream"],
              element["Scheme"],
              element["Training Service Provider"],
              element["Trade"],
              build,
              fur,
              backup,
              tne,
              total,
              sos
            )
          );
          // this.InfrastructureArray = this.InfrastructureForm.get(
          //   "InfrastructureArray"
          // ) as FormArray;
          // this.InfrastructureArray.push(
          //   this._formBuilder.group({
          //     Stream: element["Stream"],
          //     Scheme: element["Scheme"],
          //     TrainingServiceProvider: element["Training Service Provider"],
          //     Trade: element["Trade"],
          //     Building: element["Building "],
          //     Furniture: element["Furniture"],
          //     BackupSourceOfElectricity:
          //       element["Backup Source of Electricity"],
          //     ToolsAndequipment: element["Tools and equipment "],
          //     Total: element["Total"],
          //     ScoreOnScaleOf5: element["Score on a Scale of 5"],
          //   })
          // );
        }
      });
    };
    reader.readAsBinaryString(file);
    ev.target.value = "";
  }
  SaveChanges() {
    this.http
      .postJSON("api/Infrastructure/Save", this.InfrastructureForm.value)
      .subscribe(
        (d: any) => {
          this.GetInfrastructures();
        },
        (error) => (this.error = error), // error path
        () => {}
      );
  }
  getControls() {
    return (this.InfrastructureForm.get("InfrastructureArray") as FormArray)
      .controls;
  }
  GetInfrastructures() {
    this.http.getJSON("api/Infrastructure/GetInfrastructures").subscribe(
      (d: any) => {
        this.Infrastructures = d;
        //this.schemes.paginator = this.paginator;
        //this.schemes.sort = this.sort;
      },
      (error) => (this.error = error), // error path
      () => {
        this.working = false;
      }
    );
  }
}
