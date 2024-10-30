import { filter, map } from 'rxjs/operators';
import { AfterViewInit, ChangeDetectorRef, Component, OnInit, ViewChild } from "@angular/core";
import { CommonSrvService } from "../../common-srv.service";
import { ActivatedRoute } from "@angular/router";
import { MatTableDataSource } from "@angular/material/table";
import { MatPaginator } from "@angular/material/paginator";
import { MatSort } from "@angular/material/sort";
import { MatTabGroup } from "@angular/material/tabs";
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from "@angular/forms";
import { MatOption } from "@angular/material/core";
import { MatSelect, MatSelectChange } from "@angular/material/select";
import { MatDialog } from '@angular/material/dialog';
import { ErrorLogTableComponent } from 'src/app/custom-components/error-log-table/error-log-table.component';
@Component({
  selector: "app-trade-plan",
  templateUrl: "./trade-plan.component.html",
  styleUrls: ["./trade-plan.component.scss"],
})
export class TradePlanComponent implements OnInit, AfterViewInit {
  @ViewChild("tabGroup") tabGroup: MatTabGroup;
  @ViewChild("ppaginator", { static: false }) ppaginator: MatPaginator;
  @ViewChild("psort", { static: false }) psort: MatSort;
  ProgramWiseTablesData: MatTableDataSource<any>;
  ProgramWiseTableColumns = [];
  @ViewChild("tpaginator", { static: false }) tpaginator: MatPaginator;
  @ViewChild("tsort", { static: false }) tsort: MatSort;
  TradeWiseTablesData: MatTableDataSource<any>;
  TradeWiseTableColumns = [];
  @ViewChild("lpaginator", { static: false }) lpaginator: MatPaginator;
  @ViewChild("lsort", { static: false }) lsort: MatSort;
  LotWiseTablesData: MatTableDataSource<any>;
  LotWiseTableColumns = [];
  @ViewChild("Province") Province: MatSelect;
  @ViewChild("Cluster") Cluster: MatSelect;
  @ViewChild("District") District: MatSelect;
  PSearchCtr = new FormControl("");
  CSearchCtr = new FormControl("");
  DSearchCtr = new FormControl("");
  BSearchCtr = new FormControl("");
  SelectedAll_Province: string;
  SelectedAll_Cluster: string;
  SelectedAll_District: string;
  matSelectArray: MatSelect[] = [];
  TapIndex = 0;
  PreadOnly = true;
  CreadOnly = true;
  DreadOnly = true;
  error: any;
  currentUser: any;
  GetDataObject: any = {};
  programDesign: any = [];
  trade: any = [];
  tradeLayer: any = [];
  programFocus: any;
  TradeLot: FormArray;
  TradeLotData: any[];
  selectedtradeLayer: any;
  SpacerTitle: string;
  Data: any = [];
  ProvinceData: any = [];
  ClusterData: any = [];
  DistrictData: any = [];
  programBudget: any;
  TradeDesign: any;
  ProgramDesignOn: string = "Selected";
  Names: string[];
  GenderData: any;
  IsInitiated: boolean = false;
  constructor(
    private ComSrv: CommonSrvService,
    private ActiveRoute: ActivatedRoute,
    private fb: FormBuilder,
    private cdr: ChangeDetectorRef,
    private Dialog: MatDialog
  ) {}
  ngOnInit(): void {
    this.TapIndex = 0;
    this.currentUser = this.ComSrv.getUserDetails();
    this.ProgramWiseTablesData = new MatTableDataSource([]);
    this.TradeWiseTablesData = new MatTableDataSource([]);
    this.LotWiseTablesData = new MatTableDataSource([]);
    this.PageTitle();
    this.InitTradeDesignInfo();
    this.InitSelectedTradeInfo();
    this.InitSelectedProgramInfo();
    this.LoadData();
    this.GetTradeDesign();
  }
  TradeDesignInfoForm: FormGroup;
  InitTradeDesignInfo() {
    this.TradeDesignInfoForm = this.fb.group({
      Scheme: ["", Validators.required],
      UserID: [this.currentUser.UserID],
      TradeDesignID: [0],
      Province: [""],
      Cluster: [""],
      District: [""],
      ProgramDesignOn: [""],
      ProgramFocus: ["", Validators.required],
      Trade: ["", Validators.required],
      TradeLayer: ["", Validators.required],
      CTM: ["", Validators.required],
      ExamCost: ["", Validators.required],
      TraineeContraTarget: [0, Validators.required],
      ContraTargetThreshold: [20, Validators.required],
      Threshold: [0, Validators.required],
      TraineeCompTarget: [0, Validators.required],
      PerSelectedContraTarget: [0, Validators.required],
      PerSelectedCompTarget: [0, Validators.required],
      SelectedCount: [0],
      SelectedShortList: [""],
      GenderID: ["", Validators.required],
      TradeLot: this.fb.array([]),
    });
    const clearTradeLotOnChange = (fields: string[]) => {
      fields.forEach((field) => {
        this.TradeDesignInfoForm.get(field).valueChanges.subscribe(() => {
          const d = this.TradeDesignInfoForm.get(
            "PerSelectedContraTarget"
          ).value;
          if (
            d !== "" &&
            (field === "PerSelectedContraTarget" ||
              field === "ContraTargetThreshold" ||
              field === "Province")
          ) {
            this.UpdateAutoValue();
          }
          if (this.TradeLot.length > 0) {
            this.TradeLot.clear();
          }
          // this.TradeDesignInfoForm.get('TradeLot').control.d.TraineeContTarget
        });
      });
    };
    // this.TradeDesignInfoForm.get("Province").valueChanges.subscribe(p => {
    //   this.TradeDesignInfoForm.get("SelectedCount").setValue(p.length);
    //   this.TradeDesignInfoForm.get("SelectedShortList").setValue(this.getProvinceNames(p).join(", "));
    // });
    // this.TradeDesignInfoForm.get("Cluster").valueChanges.subscribe(c => {
    //   this.TradeDesignInfoForm.get("SelectedCount").setValue(c.length);
    //   this.TradeDesignInfoForm.get("SelectedShortList").setValue(this.getClusterNames(c).join(", "));
    // });
    // this.TradeDesignInfoForm.get("District").valueChanges.subscribe(d => {
    //   this.TradeDesignInfoForm.get("SelectedCount").setValue(d.length);
    //   this.TradeDesignInfoForm.get("SelectedShortList").setValue(this.getDistrictNames(d).join(", "));
    // });
    this.TradeLot = this.TradeDesignInfoForm.get("TradeLot") as FormArray;
    const fieldsToWatch = [
      "District",
      "Cluster",
      "Province",
      "ExamCost",
      "CTM",
      "PerSelectedContraTarget",
      "ContraTargetThreshold",
      "Trade",
      "TradeLayer",
    ];
    clearTradeLotOnChange(fieldsToWatch);
  }
  controlValue: number[];
  OnLocationChange() {
    this.Names = [];
    this.controlValue = [];
    let selectedShortListControl =
      this.TradeDesignInfoForm.get("SelectedShortList");
    let selectedCountControl = this.TradeDesignInfoForm.get("SelectedCount");
    this.controlValue = this.TradeDesignInfoForm.get(
      this.ProgramDesignOn
    ).value;
    this.Names = this.getNames(this.controlValue);
    switch (this.ProgramDesignOn) {
      case "Province":
        selectedShortListControl.setValue(this.Names.join(", "));
        selectedCountControl.setValue(this.Names.length);
        break;
      case "Cluster":
        selectedShortListControl.setValue(this.Names.join(", "));
        selectedCountControl.setValue(this.Names.length);
        break;
      case "District":
        selectedShortListControl.setValue(this.Names.join(", "));
        selectedCountControl.setValue(this.Names.length);
        break;
      default:
        return;
    }
    this.UpdateAutoValue();
  }
  UpdateAutoValue() {
    const d = this.TradeDesignInfoForm.get("PerSelectedContraTarget").value;
    const threshold =
      (this.TradeDesignInfoForm.get("ContraTargetThreshold").value / 100) * d;
    const districtCount = this.TradeDesignInfoForm.get("SelectedCount").value;
    const PerSelectedCompTarget = parseInt(d) - Number(threshold);
    const traineeCompTarget = PerSelectedCompTarget * districtCount;
    const traineeContTarget = d * districtCount;
    this.TradeDesignInfoForm.patchValue({
      PerSelectedCompTarget: Math.round(PerSelectedCompTarget),
      TraineeContraTarget: Math.round(traineeContTarget),
      TraineeCompTarget: Math.round(traineeCompTarget),
    });
  }
  getNames(ids: number[]): string[] {
    switch (this.ProgramDesignOn) {
      case "Province":
        return this.getProvinceNames(ids);
      case "Cluster":
        return this.getClusterNames(ids);
      case "District":
        return this.getDistrictNames(ids);
      default:
        return [];
    }
  }
  getProvinceNames(ids: number[]): string[] {
    return this.ProvinceData.filter((d) => ids.includes(d.ProvinceID)).map(
      (d) => d.ProvinceName
    );
  }
  getClusterNames(ids: number[]): string[] {
    return this.ClusterData.filter((d) => ids.includes(d.ClusterID)).map(
      (d) => d.ClusterName
    );
  }
  getDistrictNames(ids: number[]): string[] {
    return this.DistrictData.filter((d) => ids.includes(d.DistrictID)).map(
      (d) => d.DistrictName
    );
  }
  addTradeLot() {
    if (this.TradeDesignInfoForm.invalid) {
      this.ComSrv.ShowError("All * fields are required for form submission");
      return;
    }
    const { CTM, ExamCost, PerSelectedContraTarget } =
      this.TradeDesignInfoForm.value;
    const { Stipend, bagBadgeCost } = this.selectedProgram[0];
    const { Duration } = this.selectedtradeLayer[0];
    const shortListDistrict =
      this.TradeDesignInfoForm.get("SelectedShortList").value.split(",");
    this.TradeLotData = shortListDistrict.map((district) =>
      this.createTradeLotDetail(
        district,
        CTM,
        Duration,
        Stipend,
        bagBadgeCost,
        ExamCost,
        PerSelectedContraTarget
      )
    );
    this.updateTradeLotFormArray();
  }
  createTradeLotDetail(
    district: string,
    CTM: number,
    duration: number,
    stipend: number,
    bagBadgeCost: number,
    examCost: number,
    perDisContraTarget: number
  ) {
    const totalCost = this.calculateTotalCost(
      CTM,
      duration,
      stipend,
      bagBadgeCost,
      examCost,
      perDisContraTarget
    );
    return {
      UserID: this.currentUser.UserID,
      ProgramDesignID: 0,
      TradeLayer: this.selectedtradeLayer[0].TradeName,
      AnnouncedDistrict: district,
      Duration: duration,
      TraineeContTarget: perDisContraTarget,
      CTM: CTM,
      TrainingCost: CTM * duration * perDisContraTarget,
      Stipend: stipend * duration * perDisContraTarget,
      BagAndBadge: perDisContraTarget * bagBadgeCost,
      ExamCost: examCost * perDisContraTarget,
      TotalCost: totalCost,
    };
  }
  // duration * CTM * CompletionTarget
  //  2*50*2=200
  calculateTotalCost(
    CTM: number,
    duration: number,
    stipend: number,
    bagBadgeCost: number,
    examCost: number,
    perDisContraTarget: number
  ): number {
    return (
      CTM * duration * perDisContraTarget +
      stipend * duration * perDisContraTarget +
      perDisContraTarget * bagBadgeCost +
      examCost * perDisContraTarget
    );
  }
  updateTradeLotFormArray() {
    this.TradeLot = this.TradeDesignInfoForm.get("TradeLot") as FormArray;
    this.TradeLot.clear();
    if (this.TradeLotData.length > 0) {
      this.TradeLotData.forEach((detail) => {
        this.TradeLot.push(this.fb.group(detail));
      });
    } else {
      this.ComSrv.ShowError("All * fields are required to create trade Lot.");
    }
  }
  ChangeTradeLot(index) {
    const tradeLotControl = this.TradeLot.at(index) as FormGroup;
    const districtContTarget = tradeLotControl.value.TraineeContTarget;
    tradeLotControl.patchValue(
      this.createTradeLotDetail(
        tradeLotControl.value.AnnouncedDistrict,
        tradeLotControl.value.CTM,
        tradeLotControl.value.Duration,
        this.selectedProgram[0].Stipend,
        this.selectedProgram[0].bagBadgeCost,
        this.TradeDesignInfoForm.value.ExamCost,
        districtContTarget
      )
    );
  }
  removeTradeLot(index: number) {
    const TradeLot = this.TradeDesignInfoForm.get("TradeLot") as FormArray;
    TradeLot.removeAt(index);
  }
  SelectedTradeInfo: FormGroup;
  InitSelectedTradeInfo() {
    this.SelectedTradeInfo = this.fb.group({
      SectorName: [""],
      SubSectorName: [""],
      CertAuthName: [""],
      traineeEducation: [""],
      Duration: [""],
      SourceOfCurriculum: [""],
    });
    this.SelectedTradeInfo.disable();
  }
  SelectedProgramInfo: FormGroup;
  InitSelectedProgramInfo() {
    this.SelectedProgramInfo = this.fb.group({
      ContractedTarget: [0],
      CompletionTarget: [0],
      CTM: [0],
      TrainingCost: [0],
      Stipend: [0],
      BagAndBadge: [0],
      ExamCost: [0],
      TotalCost: [0],
    });
    this.SelectedProgramInfo.disable();
  }
  GetCluster(ProvinceId) {
    this.ClusterData = [];
    if (ProvinceId == 0) {
      const cluster = this.GetDataObject.cluster.filter((c) =>
        this.selectedProgram[0].Cluster.split(",")
          .map(Number)
          .includes(c.ClusterID)
      );
      this.ClusterData = cluster;
    } else {
      const cluster = this.GetDataObject.cluster.filter((c) =>
        this.selectedProgram[0].Cluster.split(",")
          .map(Number)
          .includes(c.ClusterID)
      );
      this.ClusterData = cluster.filter((c) =>
        ProvinceId.includes(c.ProvinceID)
      );
      this.GetDistrict(this.ClusterData.map((c) => Number(c.ClusterID)));
    }
  }
  GetDistrict(ClusterId) {
    this.DistrictData = [];
    if (ClusterId == 0) {
      const district = this.GetDataObject.district.filter((d) =>
        this.selectedProgram[0].District.split(",")
          .map(Number)
          .includes(d.DistrictID)
      );
      this.DistrictData = district;
    } else {
      const district = this.GetDataObject.district.filter((d) =>
        this.selectedProgram[0].District.split(",")
          .map(Number)
          .includes(d.DistrictID)
      );
      this.DistrictData = district.filter((d) =>
        ClusterId.includes(d.ClusterID)
      );
    }
  }
  LoadData() {
    this.ComSrv.postJSON("api/ProgramDesign/LoadData", {
      UserID: this.currentUser.UserID,
    }).subscribe(
      (response) => {
        this.GetDataObject = response;
        this.programDesign = this.GetDataObject.programDesign.filter(
          (d) => d.IsInitiate == 0 || d.IsInitiate == false
        );
        this.programFocus = this.GetDataObject.programFocus;
        this.trade = this.GetDataObject.trade;
        this.tradeLayer = this.GetDataObject.tradeLayer;
        this.GenderData = this.GetDataObject.gender;
        this.LoadMatTable(
          this.GetDataObject.programWiseBudget,
          "programWiseBudget"
        );
        this.LoadMatTable(
          this.GetDataObject.tradeWiseTarget,
          "tradeWiseTarget"
        );
        this.LoadMatTable(this.GetDataObject.lotWiseTarget, "lotWiseTarget");
      },
      (error) => {
        this.ComSrv.ShowError(`${error.message}`, "Close", 500000);
      }
    );
  }
  selectedProgram: any = {};
  LoadProgramData(programId) {
    this.selectedProgram = {};
    const ProgramData = programId;
    this.SelectedProgramInfo.reset();
    this.SelectedTradeInfo.reset();
    this.InitTradeDesignInfo();
    this.TradeDesignInfoForm.patchValue({ Scheme: ProgramData });
    this.selectedProgram = this.GetDataObject.programDesign.filter(
      (p) => p.ProgramID === ProgramData
    );
    this.programBudget = this.GetDataObject.programBudget.filter(
      (p) => p.ProgramID === ProgramData
    );
    this.InitSelectedProgramInfo();
    if (this.programBudget.length > 0) {
      this.SelectedProgramInfo.patchValue(this.programBudget[0]);
    }
    this.ProgramDesignOn = this.selectedProgram[0].SchemeDesignOn;
    const IsInitiate = this.selectedProgram[0].IsInitiate;
    this.LoadGender(
      this.selectedProgram[0].Gender,
      this.selectedProgram[0].GenderID
    );
    this.ChangeSchemeDesignOn(this.selectedProgram[0].SchemeDesignOn);
  }
  LoadGender(GenderName: string, GenderID: number) {
    this.GenderData = this.GetDataObject.gender;
    if (GenderName != "Both") {
      this.GenderData = this.GenderData.filter(
        (d) => d.GenderName == GenderName
      );
      this.TradeDesignInfoForm.get("GenderID").setValue(GenderID);
    }
  }
  LoadProvince() {
    this.ProvinceData = [];
    const selectedProgramProvince =
      this.selectedProgram[0].Province.split(",").map(Number);
    this.ProvinceData = this.GetDataObject.province.filter((p) =>
      selectedProgramProvince.includes(p.ProvinceID)
    );
  }
  LoadTradeLayer(TradeId) {
    this.checkTradeLotIsCreated();
    this.tradeLayer = this.GetDataObject.tradeLayer.filter(
      (t) => t.TradeID === TradeId
    );
    this.SelectedTradeInfo.reset();
  }
  LoadTradeLayerData(TradeLayerId) {
    this.selectedtradeLayer = this.GetDataObject.tradeLayer.filter(
      (t) => t.TradeDetailMapID === TradeLayerId
    );
    this.SelectedTradeInfo.patchValue(this.selectedtradeLayer[0]);
  }

  existedTradeList: any;
  checkTradeLotIsCreated() {
    this.existedTradeList=[]
    const selectedProgram = this.programDesign.find(
      (t) => t.ProgramID === this.TradeDesignInfoForm.get("Scheme").value
    );
    const selectedTrade = this.trade.find(
      (t) => t.TradeID === this.TradeDesignInfoForm.get("Trade").value
    );

    // Check if program and trade are selected
    if (!selectedProgram) {
      this.ComSrv.ShowError("Program selection is required to proceed");
      return;
    }

    if (!selectedTrade) {
      this.ComSrv.ShowError("Trade selection is required to proceed");
      return;
    }

    // Helper to check if a similar parent trade lot exists
    if (this.EditCheck) {
      this.existedTradeList = this.GetDataObject.tradeWiseTarget
        .filter((t) => t.ParentTrade != selectedTrade.ParentTrade)
        .filter(
          (t) =>
            t.ProgramName == selectedProgram.ProgramName &&
            t.ParentTrade == selectedTrade.ParentTrade
        );
    } else {
      this.existedTradeList = this.GetDataObject.tradeWiseTarget.filter(
        (t) =>
          t.ProgramName == selectedProgram.ProgramName &&
          t.ParentTrade == selectedTrade.ParentTrade
      );
    }
    if (this.existedTradeList.length > 0) {
      this.ComSrv.ShowError("similar parent trade lot is already created.");
      return;
    }
  }

  EmptyCtrl() {
    this.PSearchCtr.setValue("");
    this.CSearchCtr.setValue("");
    this.DSearchCtr.setValue("");
    this.BSearchCtr.setValue("");
  }
  UpdateRecord(row: any) {
    this.TradeDesignInfoForm.patchValue(row);
  }
  sumArray(array) {
    return array.reduce((acc, val) => parseInt(acc) + parseInt(val), 0);
  }
  IsDisabled = false;
  async SaveTradePlanInfo() {
    this.checkTradeLotIsCreated();

    if (this.existedTradeList.length > 0) {
      return;
    }

    const { ContraTargetThreshold, TraineeContraTarget, TradeLot } =
      this.TradeDesignInfoForm.value;
    // const TraineeContThreshold = (ContraTargetThreshold / 100) * TraineeContraTarget;
    const totalTraineeContTarget = this.TradeDesignInfoForm.get(
      "TraineeContraTarget"
    ).value;
    const totalLotContractedTrainee = this.sumArray(
      TradeLot.map((d) => d.TraineeContTarget)
    );
    if (this.TradeDesignInfoForm.invalid) {
      this.ComSrv.ShowError("All * filed is required to form submission");
    }
    // if (totalTraineeContTarget < totalLotContractedTrainee) {
    debugger;
    if (Number(totalTraineeContTarget) < Number(totalLotContractedTrainee)) {
      this.ComSrv.ShowError(
        `Contracted target must be equal OR greater than total lots  target.(Contracted Target :${totalTraineeContTarget} | Lots Contracted Target:${totalLotContractedTrainee})`,
        "",
        10000
      );
      return;
    }
    if (this.TradeLot.length == 0) {
      this.ComSrv.ShowError("Minimum one Trade Lot is required.");
      return;
    }
    this.ExistedTradePlan = [];
    await this.CheckTradePlanExisted(this.TradeDesignInfoForm);
    if (this.EditCheck == true) {
      const ExistedTradeLot: any[] = this.ExistedTradePlan.filter(
        (d) => !this.TradeLots.map((d) => d.TradeLotID).includes(d.TradeLotID)
      );
      if (ExistedTradeLot.length > 0) {
        this.OpenDialogue();
        this.ComSrv.ShowError(
          `Trade Plan already exists for the selected ${this.ProgramDesignOn} value. Please deselect ${this.ProgramDesignOn}.`,
          "",
          10000
        );
        return;
      }
    } else {
      if (this.ExistedTradePlan.length > 0) {
        this.OpenDialogue();
        this.ComSrv.ShowError(
          `Trade Plan already exists for the selected ${this.ProgramDesignOn} value. Please deselect ${this.ProgramDesignOn}.`,
          "",
          10000
        );
        return;
      }
    }
    if (this.TradeDesignInfoForm.valid && this.TradeLot.length > 0) {
      this.TradeDesignInfoForm.get("ProgramDesignOn").setValue(
        this.ProgramDesignOn
      );
      this.IsDisabled = true;
      this.ComSrv.postJSON(
        "api/ProgramDesign/SaveTradeDesign",
        this.TradeDesignInfoForm.getRawValue()
      ).subscribe(
        (response) => {
          this.EditCheck = false;
          this.IsInitiated = false;
          this.TradeDesignInfoForm.enable();
          this.InitTradeDesignInfo();
          this.InitSelectedProgramInfo();
          this.InitSelectedTradeInfo();
          this.LoadData();
          this.GetTradeDesign();
          this.TradeDesignInfoForm.reset;
          const TradeLot = this.TradeDesignInfoForm.get(
            "TradeLot"
          ) as FormArray;
          TradeLot.clear();
        },
        (error) => {
          this.ComSrv.ShowError(`${error.message}`, "Close", 500000);
        }
      );
      this.ComSrv.openSnackBar("Saved data");
      this.IsDisabled = false;
    } else {
      this.ComSrv.ShowError("please enter valid data");
    }
  }
  ResetFrom() {
    if (confirm("do you want to reset form data")) {
      this.EditCheck = false;
      this.IsInitiated = false;
      this.TradeDesignInfoForm.enable();
      this.programDesign = this.GetDataObject.programDesign.filter(
        (d) => d.IsInitiate == 0 || d.IsInitiate == false
      );
      this.InitTradeDesignInfo();
      this.SelectedProgramInfo.reset();
      this.SelectedTradeInfo.reset();
      this.TradeDesignInfoForm.clearValidators();
    }
  }
  PageTitle(): void {
    this.ComSrv.setTitle(this.ActiveRoute.snapshot.data.title);
    this.SpacerTitle = this.ActiveRoute.snapshot.data.title;
  }
  SelectAll(event: any, dropDownNo: number, controlName, formGroup) {
    const matSelect = this.matSelectArray[dropDownNo - 1];
    if (event.checked) {
      matSelect.options.forEach((item: MatOption) => item.select());
      if (this[formGroup].get(controlName).value) {
        const uniqueArray = Array.from(
          new Set(this[formGroup].get(controlName).value)
        );
        this[formGroup].get(controlName).setValue(uniqueArray);
      }
    } else {
      matSelect.options.forEach((item: MatOption) => item.deselect());
    }
    if (controlName != this.ProgramDesignOn) {
    }
  }
  optionClick(event, controlName) {
    this.EmptyCtrl();
    let newStatus = true;
    event.source.options.forEach((item: MatOption) => {
      if (!item.selected && !item.disabled) {
        newStatus = false;
      }
    });
    if (event.source.ngControl.name === controlName) {
      this["SelectedAll_" + controlName] = newStatus;
    } else {
      this["SelectedAll_" + controlName] = newStatus;
    }
  }
  OpenDialogue() {
    const data = this.ExistedTradePlan;
    const dialogRef = this.Dialog.open(ErrorLogTableComponent, {
      width: "70%",
      data: data,
      disableClose: true,
    });
    dialogRef.afterClosed().subscribe((result) => {
      return;
    });
  }
  LoadMatTable(tableData: any[], ReportName: string) {
    if (tableData.length > 0) {
      switch (ReportName) {
        case "tradeWiseTarget":
          this.TradeWiseTableColumns = Object.keys(tableData[0]).filter(
            (key) => !key.toLowerCase().includes("id")
          );
          this.TradeWiseTablesData = new MatTableDataSource(tableData);
          this.TradeWiseTablesData.paginator = this.tpaginator;
          this.TradeWiseTablesData.sort = this.tsort;
          break;
        case "lotWiseTarget":
          this.LotWiseTableColumns = Object.keys(tableData[0]).filter(
            (key) => !key.toLowerCase().includes("id")
          );
          this.LotWiseTablesData = new MatTableDataSource(tableData);
          this.LotWiseTablesData.paginator = this.lpaginator;
          this.LotWiseTablesData.sort = this.lsort;
          break;
        case "programWiseBudget":
          const excludeColumnArray = [
            "ClassStartDate",
            "SSPWorkflow",
            "IsSubmitted",
            "TentativeProcessStart",
            "ProcessStatus",
            "IsWorkflowAttached",
            "Workflow",
            "IsCriteriaAttached",
            "Criteria",
            "IsInitiated",
            "ApprovalStatus",
            "IsFinalApproval",
            "WorkflowRemarks",
            "ProcessStatus",
            "AssociationStartDate",
            "AssociationEndDate",
            "IsWorkflowAttached",
            "Workflow",
            "IsCriteriaAttached",
            "Criteria",
            "CriteriaRemarks",
            "StartDate",
            "EndDate",
            "StatusRemarks",
            "IsFinalApproved",
          ];
          this.ProgramWiseTableColumns = Object.keys(tableData[0]).filter(
            (key) => !key.includes("ID") && !excludeColumnArray.includes(key)
          );
          this.ProgramWiseTablesData = new MatTableDataSource(tableData);
          this.ProgramWiseTablesData.paginator = this.ppaginator;
          this.ProgramWiseTablesData.sort = this.psort;
          break;
      }
    }
  }
  EditCheck: boolean = false;
  async EditTradePlan(row: any) {
    debugger;
    const GenderID = this.GenderData.filter(
      (g) => g.GenderName == row.Gender
    ).map((g) => g.GenderID);
    this.EditCheck = true;
    const selectedTradeDesign = await this.TradeDesign.filter(
      (d) => d.TradeDesignID == row.TradeDesignID
    );
    const programIsInitiated: any[] = this.GetDataObject.programDesign.filter(
      (d: any) =>
        d.ProgramID == selectedTradeDesign[0].Scheme &&
        (d.IsInitiate == 1 || d.IsInitiate == true)
    );
    if (programIsInitiated.length > 0) {
      this.IsInitiated = true;
      this.programDesign = this.GetDataObject.programDesign.filter(
        (d) => d.IsInitiate == 1 || d.IsInitiate == true
      );
      this.TradeDesignInfoForm.disable();
      this.ComSrv.ShowError(
        "Selected record is locked because program is Initiated ."
      );
    } else {
      this.programDesign = this.GetDataObject.programDesign.filter(
        (d) => d.IsInitiate == 0 || d.IsInitiate == false
      );
      this.TradeDesignInfoForm.enable();
      this.IsInitiated = false;
    }
    this.TradeDesignInfoForm.get("Scheme").setValue(
      selectedTradeDesign[0].Scheme
    );
    switch (selectedTradeDesign[0].ProgramDesignOn) {
      case "Province":
        this.TradeDesignInfoForm.get("Province").setValue(
          selectedTradeDesign[0].ProvinceID.split(",").map(Number)
        );
        break;
      case "Cluster":
        this.TradeDesignInfoForm.get("Province").setValue(
          selectedTradeDesign[0].ProvinceID.split(",").map(Number)
        );
        this.GetCluster(
          selectedTradeDesign[0].ProvinceID.split(",").map(Number)
        );
        this.TradeDesignInfoForm.get("Cluster").setValue(
          selectedTradeDesign[0].ClusterID.split(",").map(Number)
        );
        break;
      case "District":
        this.TradeDesignInfoForm.get("Province").setValue(
          selectedTradeDesign[0].ProvinceID.split(",").map(Number)
        );
        this.GetCluster(
          selectedTradeDesign[0].ProvinceID.split(",").map(Number)
        );
        this.TradeDesignInfoForm.get("Cluster").setValue(
          selectedTradeDesign[0].ClusterID.split(",").map(Number)
        );
        this.GetDistrict(
          selectedTradeDesign[0].ClusterID.split(",").map(Number)
        );
        this.TradeDesignInfoForm.get("District").setValue(
          selectedTradeDesign[0].DistrictID.split(",").map(Number)
        );
        break;
    }
    this.TradeDesignInfoForm.patchValue(selectedTradeDesign[0]);
    this.TradeDesignInfoForm.get("GenderID").setValue(GenderID[0]);
    await this.LoadTradeLayerData(selectedTradeDesign[0].TradeLayer);
    await this.OnLocationChange();
    setTimeout(() => {
      this.addTradeLot();
    }, 0);
    await this.GetTradeLot(selectedTradeDesign[0].TradeDesignID);
    this.TradeLot = this.TradeDesignInfoForm.get("TradeLot") as FormArray;
    this.TradeLot.controls.forEach((control: FormGroup, index: number) => {
      control.controls.TraineeContTarget.setValue(
        this.TradeLots[index].TraineeSelectedContTarget
      );
      control.controls.TotalCost.setValue(this.TradeLots[index].TotalCost);
    });
    // this.TradeDesignInfoForm.get("GenderID").setValue(selectedTradeDesign[0].GenderID)

    this.tabGroup.selectedIndex = 0;
    // } else {
    //   this.ComSrv.ShowError('Selected record is locked because program is Initiated .')
    // }
  }
  camelCaseToWords(input: string): string {
    return input.replace(/([a-z])([A-Z])/g, "$1 $2");
  }
  applyFilter(data, event) {
    debugger;

    data.filter = event.target.value.trim().toLowerCase();
    if (data.paginator) {
      data.paginator.firstPage();
    }
  }
  DataExcelExport(data: any, title) {
    this.ComSrv.ExcelExporWithForm(data, title);
  }
  ChangeSchemeDesignOn(value: string) {
    const provinceControl = this.TradeDesignInfoForm.get("Province");
    const clusterControl = this.TradeDesignInfoForm.get("Cluster");
    const districtControl = this.TradeDesignInfoForm.get("District");
    provinceControl.clearValidators();
    clusterControl.clearValidators();
    districtControl.clearValidators();
    this.TradeDesignInfoForm.get("Province").disable();
    this.TradeDesignInfoForm.get("Cluster").disable();
    this.TradeDesignInfoForm.get("District").disable();
    this.PreadOnly = true;
    this.CreadOnly = true;
    this.DreadOnly = true;
    if (value === "Province") {
      this.ClusterData = [];
      this.DistrictData = [];
      this.LoadProvince();
      provinceControl.setValidators(Validators.required);
      provinceControl.enable();
      this.PreadOnly = false;
      this.CreadOnly = true;
      this.DreadOnly = true;
    } else if (value === "Cluster") {
      this.DistrictData = [];
      this.LoadProvince();
      // this.GetCluster(0)
      clusterControl.setValidators(Validators.required);
      clusterControl.setValidators(Validators.required);
      provinceControl.enable();
      clusterControl.enable();
      this.PreadOnly = false;
      this.CreadOnly = false;
      this.DreadOnly = true;
    } else if (value === "District") {
      this.LoadProvince();
      // this.GetCluster(0)
      // this.GetDistrict(0)
      provinceControl.setValidators(Validators.required);
      clusterControl.setValidators(Validators.required);
      districtControl.setValidators(Validators.required);
      provinceControl.enable();
      clusterControl.enable();
      districtControl.enable();
      this.PreadOnly = false;
      this.CreadOnly = false;
      this.DreadOnly = false;
    }
    provinceControl.updateValueAndValidity();
    clusterControl.updateValueAndValidity();
    districtControl.updateValueAndValidity();
    // this.TradeDesignInfoForm.updateValueAndValidity
  }
  getErrorMessage(errorKey: string, errorValue: any): string {
    const errorMessages = {
      required: "This field is required.",
      minlength: `This field must be at least ${
        errorValue.requiredLength - 1
      } characters long.`,
      maxlength: `This field's text exceeds the specified maximum length.  (maxLength: ${errorValue.requiredLength} characters)`,
      email: "Invalid email address.",
      pattern: "This field is only required text",
      customError: errorValue,
    };
    return errorMessages[errorKey];
  }
  // required data for edit proccess
  async GetTradeDesign() {
    this.SPName = "RD_SSPTradeDesign";
    this.paramObject = {};
    this.TradeDesign = [];
    this.TradeDesign = await this.FetchData(this.SPName, this.paramObject);
  }
  TradePlan: any[] = [];
  ExistedTradePlan: any[] = [];
  async CheckTradePlanExisted(form: FormGroup) {
    this.SPName = "RD_SSPCheckTradePlanExisted";
    // Ensure the forEach loop is asynchronous
    await Promise.all(
      this.controlValue.map(async (LocationID) => {
        this.paramObject = {
          ProgramDesignOn: this.ProgramDesignOn,
          ProgramID: form.get("Scheme").value,
          LocationID: LocationID,
          ProgramFocusID: form.get("ProgramFocus").value,
          TradeDetailMapID: form.get("TradeLayer").value,
        };
        this.TradePlan = [];
        this.TradePlan = await this.FetchData(this.SPName, this.paramObject);
        if (this.TradePlan.length > 0) {
          this.ExistedTradePlan.push(this.TradePlan[0]);
        }
      })
    );
  }
  TradeLots = [];
  async GetTradeLot(TradeDesignID) {
    this.SPName = "Rd_SSPTradeLotByTradeDesign";
    this.paramObject = {
      TradeDesignID: TradeDesignID,
    };
    this.TradeLots = [];
    this.TradeLots = await this.FetchData(this.SPName, this.paramObject);
  }
  GetParamString(SPName: string, paramObject: any) {
    let ParamString = SPName;
    for (const key in paramObject) {
      if (Object.hasOwnProperty.call(paramObject, key)) {
        ParamString += `/${key}=${paramObject[key]}`;
      }
    }
    return ParamString;
  }
  paramObject: any = {};
  ExportReportName: string = "";
  SPName: string = "";
  async FetchData(SPName: string, paramObject: any) {
    try {
      const Param = this.GetParamString(SPName, paramObject);
      const data: any = await this.ComSrv.getJSON(
        `api/BSSReports/FetchReportData?Param=${Param}`
      ).toPromise();
      // if (data.length > 0) {
      return data;
      // } else {
      //   // this.ComSrv.ShowWarning(' No Record Found', 'Close');
      // }
    } catch (error) {
      this.error = error;
    }
  }
  ngAfterViewInit() {
    this.matSelectArray = [this.Province, this.Cluster, this.District];
    if (this.tabGroup) {
      this.tabGroup.selectedTabChange.subscribe((event) => {
        this.TapIndex = event.index;
      });
    }
  }
}
